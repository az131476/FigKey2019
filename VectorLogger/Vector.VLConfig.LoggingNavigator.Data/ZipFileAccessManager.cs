using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.LoggingNavigator.Export;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class ZipFileAccessManager : FileAccessManager
	{
		private string mZipFilePath;

		private ZipFile mZipFile;

		private IList<ZipEntry> mZipEntries;

		private long mMaxBytes;

		private int mFileCopyCurrentPosition;

		public ZipFileAccessManager(string zipFilePath)
		{
			this.mZipFilePath = zipFilePath;
			this.mZipFile = ZipFile.Read(this.mZipFilePath);
		}

		public override Dictionary<string, Stream> GetListOfIndexFiles()
		{
			Dictionary<string, Stream> dictionary = new Dictionary<string, Stream>();
			IEnumerable<ZipEntry> zipEntryDataFilesInRootDataFolders = this.GetZipEntryDataFilesInRootDataFolders();
			foreach (ZipEntry current in zipEntryDataFilesInRootDataFolders)
			{
				string fileName = current.FileName;
				if (fileName.ToLower().EndsWith(Vocabulary.FileExtensionDotGLX))
				{
					Stream zipEntryAsStream = this.GetZipEntryAsStream(current);
					dictionary.Add(fileName, zipEntryAsStream);
				}
			}
			return dictionary;
		}

		private IEnumerable<ZipEntry> GetZipEntryDataFilesInRootDataFolders()
		{
			string text = "!D?F*X";
			List<ZipEntry> list = new List<ZipEntry>();
			string selectionCriteria = "name = '" + text + "\\*' AND type = F";
			ICollection<ZipEntry> collection = this.mZipFile.SelectEntries(selectionCriteria);
			list.AddRange(collection);
			if (!list.Any<ZipEntry>())
			{
				ZipEntry entry = this.mZipFile.Entries.First<ZipEntry>();
				string rootBasePath = this.GetRootBasePath(entry);
				selectionCriteria = "name = '" + rootBasePath + text + "\\*' AND type = F";
				collection = this.mZipFile.SelectEntries(selectionCriteria);
				list.AddRange(collection);
			}
			return list;
		}

		private string GetRootBasePath(ZipEntry entry)
		{
			string text = "";
			if (entry != null)
			{
				string[] array = entry.FileName.Split(new char[]
				{
					'/'
				});
				if (array.Count<string>() > 1)
				{
					text = array[0] + "/";
				}
				else
				{
					text = entry.FileName;
				}
				foreach (ZipEntry current in this.mZipFile.Entries)
				{
					if (!current.FileName.StartsWith(text))
					{
						return "";
					}
				}
				return text;
			}
			return text;
		}

		public override Stream GetIniFileAsStream(string iniFileName)
		{
			ZipEntry zipEntry = this.mZipFile[iniFileName];
			if (zipEntry == null)
			{
				ZipEntry entry = this.mZipFile.Entries.First<ZipEntry>();
				string rootBasePath = this.GetRootBasePath(entry);
				zipEntry = this.mZipFile[rootBasePath + iniFileName];
			}
			if (zipEntry == null)
			{
				return null;
			}
			return this.GetZipEntryAsStream(zipEntry);
		}

		public override Stream GetWavFileAsStream(string path)
		{
			ZipEntry zipEntry = this.mZipFile[path];
			if (zipEntry == null)
			{
				return null;
			}
			return this.GetZipEntryAsStream(zipEntry);
		}

		private Stream GetZipEntryAsStream(ZipEntry e)
		{
			Stream stream = new MemoryStream();
			e.Extract(stream);
			stream.Seek(0L, SeekOrigin.Begin);
			return stream;
		}

		public override bool GetNextFileToCopy(ref string name, ref long size, ref long maxSize, ref long count, ref long countTotal)
		{
			if (this.mFileCopyCurrentPosition >= this.mZipEntries.Count)
			{
				return false;
			}
			ZipEntry zipEntry = this.mZipEntries[this.mFileCopyCurrentPosition];
			name = zipEntry.FileName;
			size = zipEntry.UncompressedSize;
			maxSize = this.mMaxBytes;
			count = (long)this.mFileCopyCurrentPosition;
			countTotal = (long)this.mZipEntries.Count;
			return true;
		}

		public override bool CopyNextFileToDestination(string targetPath)
		{
			if (this.mFileCopyCurrentPosition >= this.mZipEntries.Count)
			{
				return false;
			}
			ZipEntry zipEntry = this.mZipEntries[this.mFileCopyCurrentPosition];
			zipEntry.Extract(targetPath, ExtractExistingFileAction.DoNotOverwrite);
			this.mFileCopyCurrentPosition++;
			return true;
		}

		public override bool ExistsOnFilesystem(LogFile logFile)
		{
			ZipEntry zipEntry = this.GetZipEntry(logFile);
			return zipEntry != null;
		}

		public override bool ExistsOnFilesystem(string path)
		{
			return this.mZipFile[path] != null;
		}

		public override void SetFilesToCopy(string path, IList<ExportJob> jobs)
		{
			this.mMaxBytes = 0L;
			this.mZipEntries = new List<ZipEntry>();
			this.mFileCopyCurrentPosition = 0;
			foreach (ExportJob current in jobs)
			{
				foreach (LogFile current2 in current.LogFileList)
				{
					ZipEntry zipEntry = this.GetZipEntry(current2);
					if (zipEntry != null)
					{
						this.mZipEntries.Add(zipEntry);
						this.mMaxBytes += zipEntry.UncompressedSize;
					}
				}
			}
			path = "\\";
			ZipEntry zipEntry2 = this.mZipFile[path + "ml_rt.ini"];
			if (zipEntry2 != null)
			{
				this.mZipEntries.Add(zipEntry2);
				this.mMaxBytes += zipEntry2.UncompressedSize;
			}
			zipEntry2 = this.mZipFile[path + "ml_rt.ltl"];
			if (zipEntry2 != null)
			{
				this.mZipEntries.Add(zipEntry2);
				this.mMaxBytes += zipEntry2.UncompressedSize;
			}
			zipEntry2 = this.mZipFile[path + "ml_rt2.ini"];
			if (zipEntry2 != null)
			{
				this.mZipEntries.Add(zipEntry2);
				this.mMaxBytes += zipEntry2.UncompressedSize;
			}
		}

		private ZipEntry GetZipEntry(LogFile logFile)
		{
			ZipEntry zipEntry = this.mZipFile[logFile.Path + logFile.Name];
			ZipEntry zipEntry2 = this.mZipFile[logFile.Path + logFile.Name + Vocabulary.FileExtensionDotGZ];
			ZipEntry zipEntry3 = this.mZipFile[logFile.Path + logFile.Name_old];
			if (zipEntry != null)
			{
				return zipEntry;
			}
			if (zipEntry2 != null)
			{
				return zipEntry2;
			}
			if (zipEntry3 != null)
			{
				return zipEntry3;
			}
			return null;
		}

		public override string[] GetTopLevelFiles()
		{
			List<string> list = new List<string>();
			try
			{
				ICollection<ZipEntry> collection = this.mZipFile.SelectEntries("name = '*' AND name != '*\\*.*' AND type = F");
				if (!collection.Any<ZipEntry>())
				{
					ZipEntry zipEntry = this.mZipFile.Entries.First<ZipEntry>();
					string fileName = zipEntry.FileName;
					collection = this.mZipFile.SelectEntries(string.Concat(new string[]
					{
						"name = '",
						fileName,
						"*' AND name != '",
						fileName,
						"*\\*.*' AND type = F"
					}));
				}
				foreach (ZipEntry current in collection)
				{
					list.Add(current.FileName);
				}
			}
			catch (Exception)
			{
			}
			return list.ToArray();
		}

		public override long GetFilesSize(string path)
		{
			ZipEntry zipEntry = this.mZipFile[path];
			if (zipEntry == null)
			{
				return 0L;
			}
			return zipEntry.UncompressedSize;
		}

		public override void Close()
		{
			this.mZipFile.Dispose();
		}
	}
}
