using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class IndexManager : INavigation
	{
		private enum RecordTag : uint
		{
			FileValidHeader = 1722695u,
			LogfileHeaderMSB = 16777216u,
			StartNewMeasurement = 268435456u,
			StartNewFile,
			EndMeasurement = 268435472u,
			EndFileFull,
			Trigger = 268435712u,
			NamedTrigger = 553648128u,
			CANoeMeasurementStart = 268435728u,
			CANoeMeasurementStop,
			NaviHelper = 268439552u,
			MarkerMSB = 536870912u,
			InfoRefMSB = 805306368u
		}

		private List<IndexFileCollection> mIndexFiles;

		private List<string> mVoiceRecords;

		private IndexFileCollection mCurrentIndexFile;

		private static uint sSizeOfRecord = 16u;

		private int mCurrentMemoryNr;

		private bool mHasMissingLogFiles;

		private bool mHasError;

		private int mMeasurementCountTotal;

		private static bool DEBUG = false;

		private static bool AUDI_PROTOTYPE = false;

		private bool mRecoverMeasurements = true;

		private IdManager mIdManager;

		public bool RecoverMeasurements
		{
			get
			{
				return this.mRecoverMeasurements;
			}
			set
			{
				this.mRecoverMeasurements = value;
			}
		}

		public IndexManager()
		{
			this.Clear();
		}

		public IList<Measurement> GetMeasurements()
		{
			List<Measurement> list = new List<Measurement>();
			foreach (IndexFileCollection current in this.mIndexFiles)
			{
				list.AddRange(current.GetMeasurements());
			}
			return list;
		}

		public IList<Measurement> GetMeasurements(string loggerMemNumber)
		{
			List<Measurement> list = new List<Measurement>();
			foreach (IndexFileCollection current in this.mIndexFiles)
			{
				if (current.LoggerMemNumber.Equals(loggerMemNumber))
				{
					list.AddRange(current.GetMeasurements());
				}
			}
			return list;
		}

		public IList<string> GetVoiceRecordFiles()
		{
			return this.mVoiceRecords;
		}

		public void Clear()
		{
			this.mIndexFiles = new List<IndexFileCollection>();
			this.mVoiceRecords = new List<string>();
			IndexFileCollection.Reset();
			Entry.Reset();
			this.mCurrentMemoryNr = 0;
			this.mHasMissingLogFiles = false;
			this.mHasError = false;
			this.mMeasurementCountTotal = 0;
		}

		public bool IsEmpty()
		{
			return this.mIndexFiles.Count < 1;
		}

		public bool HasMissingLogfiles()
		{
			return this.mHasMissingLogFiles;
		}

		public bool HasIndexError()
		{
			return this.mHasError;
		}

		public string[] GetAvailableLoggerMemories()
		{
			List<string> list = new List<string>();
			foreach (IndexFileCollection current in this.mIndexFiles)
			{
				if (!list.Contains(current.LoggerMemNumber))
				{
					list.Add(current.LoggerMemNumber);
				}
			}
			return list.ToArray();
		}

		public ulong GetGlobalBegin(string indexFilePath)
		{
			IndexFileCollection indexFileCollection = this.mIndexFiles.FirstOrDefault((IndexFileCollection idf) => idf.Contains(indexFilePath));
			if (indexFileCollection == null)
			{
				return 0uL;
			}
			List<Measurement> list = new List<Measurement>(indexFileCollection.GetMeasurements());
			if (!list.Any<Measurement>())
			{
				return 0uL;
			}
			list.Sort((Measurement mm1, Measurement mm2) => mm1.Begin.CompareTo(mm2.Begin));
			return list.First<Measurement>().Begin;
		}

		public bool ReadIndexFilesFromSource(string sourcePath)
		{
			if (!File.Exists(sourcePath) && !Directory.Exists(sourcePath))
			{
				return false;
			}
			bool flag = true;
			FileAccessManager fileAccessManager = FileAccessManager.CreateFileAccessManager(sourcePath);
			SortedDictionary<string, Stream> sortedDictionary = this.SortIndexFileList(fileAccessManager.GetListOfIndexFiles());
			this.mIdManager = new IdManager();
			this.mIdManager.Load(sourcePath);
			bool flag2 = false;
			foreach (KeyValuePair<string, Stream> current in sortedDictionary)
			{
				bool flag3 = true;
				string key = current.Key;
				Stream value = current.Value;
				int num = IndexManager.ConvertToInt(IndexFileCollection.ParseLoggerMemNumberFromFilename(key));
				bool flag4 = num > this.mCurrentMemoryNr;
				this.mCurrentMemoryNr = num;
				if (this.mCurrentIndexFile != null && flag4)
				{
					flag2 |= this.mCurrentIndexFile.FinalizeMeasurementsOnMemory(this.mRecoverMeasurements);
				}
				if (!IndexManager.AUDI_PROTOTYPE || this.mCurrentMemoryNr == 2)
				{
					flag3 = this.ReadIndexFile(value, key, flag4);
				}
				flag = (flag && flag3);
			}
			if (this.mCurrentIndexFile != null)
			{
				flag2 |= this.mCurrentIndexFile.FinalizeMeasurementsOnMemory(this.mRecoverMeasurements);
			}
			if (flag2)
			{
				int num2 = 1;
				foreach (IndexFileCollection current2 in this.mIndexFiles)
				{
					current2.FinalizeMeasurementNumbering(ref num2);
				}
			}
			this.ReadMediaFilesFromSource();
			return flag;
		}

		private bool ReadMediaFilesFromSource()
		{
			string[] topLevelFiles;
			try
			{
				topLevelFiles = FileAccessManager.GetInstance().GetTopLevelFiles();
			}
			catch (UnauthorizedAccessException)
			{
				bool result = false;
				return result;
			}
			if (topLevelFiles == null || topLevelFiles.Length < 1)
			{
				return false;
			}
			string[] array = topLevelFiles;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				string text2 = "";
				try
				{
					text2 = Path.GetExtension(text);
				}
				catch (ArgumentException)
				{
					bool result = false;
					return result;
				}
				if (text2.ToLower() == ".wav")
				{
					this.mVoiceRecords.Add(text);
				}
			}
			return true;
		}

		private SortedDictionary<string, Stream> SortIndexFileList(Dictionary<string, Stream> indexFileList)
		{
			SortedDictionary<string, Stream> sortedDictionary = new SortedDictionary<string, Stream>(new CustomIndexFileNameComparer());
			foreach (KeyValuePair<string, Stream> current in indexFileList)
			{
				if (!sortedDictionary.ContainsKey(current.Key))
				{
					sortedDictionary.Add(current.Key, current.Value);
				}
			}
			return sortedDictionary;
		}

		private bool ReadIndexFile(string filePath)
		{
			Stream stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
			return this.ReadIndexFile(stream, filePath, true);
		}

		private bool ReadIndexFile(Stream stream, string filePath, bool firstIndexFile)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			uint pos = 0u;
			if (binaryReader.BaseStream.Length < (long)((ulong)IndexManager.sSizeOfRecord))
			{
				return false;
			}
			uint num = binaryReader.ReadUInt32();
			uint num2 = binaryReader.ReadUInt32();
			byte[] sig = binaryReader.ReadBytes(8);
			if (num != 1722695u || num2 != 0u || !this.CheckIndexSignature(sig))
			{
				return false;
			}
			if (IndexManager.DEBUG)
			{
				Console.WriteLine("Adding index file");
			}
			if (firstIndexFile)
			{
				this.mCurrentIndexFile = new IndexFileCollection(filePath);
				this.mIndexFiles.Add(this.mCurrentIndexFile);
			}
			else
			{
				this.mCurrentIndexFile.AddIndexFilePath(filePath);
			}
			while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
			{
				uint num3 = 0u;
				if (!this.ReadLogFile(pos, binaryReader, out num3, firstIndexFile, filePath))
				{
					if (IndexManager.DEBUG)
					{
						Console.WriteLine(string.Concat(new object[]
						{
							"LogFile Header not valid: skipping ",
							num3,
							" entries / ",
							num3 * IndexManager.sSizeOfRecord,
							" bytes!"
						}));
					}
					int count = (int)(num3 * IndexManager.sSizeOfRecord);
					try
					{
						binaryReader.ReadBytes(count);
					}
					catch (Exception)
					{
						this.mHasError = true;
						break;
					}
				}
			}
			binaryReader.Close();
			stream.Close();
			return true;
		}

		private bool ReadLogFile(uint pos, BinaryReader br, out uint entries, bool firstIndexFile, string indexFilePath)
		{
			uint num = br.ReadUInt32();
			entries = br.ReadUInt32();
			ushort indexFormatVersion = br.ReadUInt16();
			ushort loggerType = br.ReadUInt16();
			ushort loggerFirmware = br.ReadUInt16();
			byte recordingType = br.ReadByte();
			br.ReadByte();
			if ((num & 4278190080u) != 16777216u)
			{
				return false;
			}
			uint num2 = num & 16777215u;
			if (IndexManager.DEBUG)
			{
				Console.WriteLine("Adding Logfile - " + num2);
			}
			LogFile logFile = new LogFile();
			logFile.Tag = num;
			logFile.Entries = entries;
			logFile.IndexFormatVersion = indexFormatVersion;
			logFile.LoggerType = loggerType;
			logFile.LoggerFirmware = loggerFirmware;
			logFile.RecordingType = recordingType;
			logFile.ID = num2;
			logFile.LoggerMemNumber = this.mCurrentIndexFile.LoggerMemNumber;
			logFile.IndexFilePath = indexFilePath;
			logFile.Path = Path.GetDirectoryName(this.mCurrentIndexFile.GetLastFilePath) + Path.DirectorySeparatorChar;
			if (!FileAccessManager.GetInstance().ExistsOnFilesystem(logFile))
			{
				if (this.mCurrentIndexFile.HasMeasurements() || !firstIndexFile)
				{
					this.mHasMissingLogFiles = true;
				}
				return false;
			}
			for (uint num3 = 0u; num3 < entries; num3 += 1u)
			{
				this.ReadRecord(pos, br, logFile, num3);
			}
			return true;
		}

		private void ReadRecord(uint pos, BinaryReader br, LogFile logFile, uint entryNr)
		{
			uint num = br.ReadUInt32();
			uint filepos = br.ReadUInt32();
			ulong num2 = br.ReadUInt64();
			if (IndexManager.DEBUG)
			{
				Console.Write("Adding entry - ");
			}
			if (entryNr == 0u && num != 268435456u && num != 268435457u)
			{
				logFile.Begin = num2;
				this.mCurrentIndexFile.AddNewMeasurement(logFile, this.mMeasurementCountTotal);
			}
			if (num == 268435456u)
			{
				if (IndexManager.DEBUG)
				{
					Console.WriteLine("Start; new measurement (" + num2 + ")");
				}
				logFile.Begin = num2;
				this.mCurrentIndexFile.AddNewMeasurement(logFile, this.mMeasurementCountTotal);
				return;
			}
			if (num == 268435457u)
			{
				if (IndexManager.DEBUG)
				{
					Console.WriteLine("Start;    existing measurement, new file (" + num2 + ")");
				}
				logFile.Begin = num2;
				this.mCurrentIndexFile.AppendLogFile(logFile, this.mMeasurementCountTotal);
				return;
			}
			if (num == 268435472u)
			{
				if (IndexManager.DEBUG)
				{
					Console.WriteLine("Ende; measurement finished (" + num2 + ")");
				}
				logFile.End = num2;
				this.mCurrentIndexFile.CloseMeasurement(num2);
				this.mMeasurementCountTotal++;
				return;
			}
			if (num == 268435473u)
			{
				if (IndexManager.DEBUG)
				{
					Console.WriteLine("Ende; file full (" + num2 + ")");
				}
				logFile.End = num2;
				return;
			}
			if (num == 268435712u)
			{
				if (IndexManager.DEBUG)
				{
					Console.WriteLine("Trigger (" + num2 + ")");
				}
				Trigger entry = new Trigger(0u, !logFile.IsPermanent, num, filepos, num2, this.mCurrentIndexFile.GetCurrentMeasurement(), this.mIdManager);
				logFile.AddEntry(entry);
				return;
			}
			if ((num & 4278190080u) == 553648128u)
			{
				uint id = num & 16777215u;
				if (IndexManager.DEBUG)
				{
					Console.WriteLine("NamedTrigger (" + num2 + ")");
				}
				Trigger entry2 = new Trigger(id, !logFile.IsPermanent, num, filepos, num2, this.mCurrentIndexFile.GetCurrentMeasurement(), this.mIdManager);
				logFile.AddEntry(entry2);
				return;
			}
			if (num == 268435728u)
			{
				if (IndexManager.DEBUG)
				{
					Console.WriteLine("CANoe Measurement Start");
					return;
				}
			}
			else if (num == 268435729u)
			{
				if (IndexManager.DEBUG)
				{
					Console.WriteLine("CANoe Measurement Stop");
					return;
				}
			}
			else if (num == 268439552u)
			{
				if (IndexManager.DEBUG)
				{
					Console.WriteLine("Navi Helper");
					return;
				}
			}
			else
			{
				if ((num & 4278190080u) == 536870912u)
				{
					uint id2 = num & 16777215u;
					if (IndexManager.DEBUG)
					{
						Console.WriteLine("Marker (" + num2 + ")");
					}
					logFile.AddEntry(new Marker(id2, num, filepos, num2, logFile.LoggerMemNumber, this.mIdManager));
					return;
				}
				if ((num & 4278190080u) == 805306368u)
				{
					if (IndexManager.DEBUG)
					{
						Console.WriteLine("Info Ref");
						return;
					}
				}
				else if (IndexManager.DEBUG)
				{
					Console.WriteLine("Other - N/A");
				}
			}
		}

		private bool CheckIndexSignature(byte[] sig)
		{
			if (sig == null || sig.Length != IndexFileCollection.SignatureBytes.Length)
			{
				return false;
			}
			for (int i = 0; i < sig.Length; i++)
			{
				if (sig[i] != IndexFileCollection.SignatureBytes[i])
				{
					return false;
				}
			}
			return true;
		}

		public static int ConvertToInt(string val)
		{
			int result = 0;
			try
			{
				result = Convert.ToInt32(val);
			}
			catch (Exception)
			{
			}
			return result;
		}
	}
}
