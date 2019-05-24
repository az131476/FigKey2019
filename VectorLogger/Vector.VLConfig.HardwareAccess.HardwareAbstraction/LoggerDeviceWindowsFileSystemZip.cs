using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public class LoggerDeviceWindowsFileSystemZip : LoggerDeviceWindowsFileSystem
	{
		private ILoggerDevice internalLoggerDevice;

		private ZipFile zipFile;

		private string rootBasePath;

		public override LoggerType LoggerType
		{
			get
			{
				return this.internalLoggerDevice.LoggerType;
			}
		}

		public override ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				if (this.internalLoggerDevice != null)
				{
					return this.internalLoggerDevice.LoggerSpecifics;
				}
				return GL3000Scanner.LoggerSpecifics;
			}
		}

		public override bool HasIndexFile
		{
			get
			{
				return this.internalLoggerDevice.HasIndexFile;
			}
		}

		public override bool HasErrorFile
		{
			get
			{
				return this.internalLoggerDevice.HasErrorFile;
			}
		}

		public override ILogFileStorage LogFileStorage
		{
			get
			{
				return this;
			}
		}

		public override bool HasProperClusterSize
		{
			get
			{
				return true;
			}
		}

		public LoggerDeviceWindowsFileSystemZip(string hardwareKey, bool isOnline, ILoggerDevice internalDevice)
		{
			this.hardwareKey = hardwareKey;
			this.isOnline = isOnline;
			this.internalLoggerDevice = internalDevice;
			this.zipFile = ZipFile.Read(hardwareKey);
			this.rootBasePath = "";
		}

		public override bool Update()
		{
			if (!this.isLogFileStorageOutdated)
			{
				return true;
			}
			this.isMemoryCardReady = base.IsWindowsFormattedMediaAccessible();
			if (!this.isMemoryCardReady)
			{
				this.hasLoggerInfo = false;
				return false;
			}
			this.CreateFileListForWindowsDirectory(this.hardwareKey);
			this.UpdateFromMlRtIniFile();
			this.isLogFileStorageOutdated = false;
			return true;
		}

		public override bool FormatCard()
		{
			throw new NotImplementedException();
		}

		public override bool WriteLicense(string licenseFilePath)
		{
			return false;
		}

		public override bool SetRealTimeClock()
		{
			return true;
		}

		public override bool SetVehicleName(string name)
		{
			return true;
		}

		public override bool GetCANTransceiverTypeForChannel(uint channelNr, out CANTransceiverType transceiverType)
		{
			if (this.internalLoggerDevice != null)
			{
				return this.internalLoggerDevice.GetCANTransceiverTypeForChannel(channelNr, out transceiverType);
			}
			transceiverType = CANTransceiverType.None;
			return false;
		}

		private IEnumerable<ZipEntry> GetZipEntires(ZipFile zipfile)
		{
			List<ZipEntry> list = new List<ZipEntry>();
			string selectionCriteria = "name = '" + Constants.LogDataFolderSearchPattern + "\\*' AND type = F";
			ICollection<ZipEntry> collection = this.zipFile.SelectEntries(selectionCriteria);
			list.AddRange(collection);
			ICollection<ZipEntry> collection2 = this.zipFile.SelectEntries("name = '*' AND name != '*\\*.*' AND type = F");
			list.AddRange(collection2);
			if (!list.Any<ZipEntry>())
			{
				ZipEntry zipEntry = zipfile.Entries.First<ZipEntry>();
				this.rootBasePath = zipEntry.FileName;
				selectionCriteria = "name = '" + this.rootBasePath + Constants.LogDataFolderSearchPattern + "\\*' AND type = F";
				collection = this.zipFile.SelectEntries(selectionCriteria);
				list.AddRange(collection);
				string selectionCriteria2 = string.Concat(new string[]
				{
					"name = '",
					this.rootBasePath,
					"*' AND name != '",
					this.rootBasePath,
					"*\\*.*' AND type = F"
				});
				collection2 = this.zipFile.SelectEntries(selectionCriteria2);
				list.AddRange(collection2);
			}
			return list;
		}

		protected new void CreateFileListForWindowsDirectory(string path)
		{
			this.logFiles.Clear();
			IEnumerable<ZipEntry> zipEntires = this.GetZipEntires(this.zipFile);
			this.highestTriggerFileIndices = new uint[this.LoggerSpecifics.DataStorage.NumberOfMemories];
			foreach (ZipEntry current in zipEntires)
			{
				bool flag = false;
				string fileName = current.FileName;
				LogFile logFile = new LogFile(fileName, (uint)current.UncompressedSize, current.CreationTime);
				for (uint num = 0u; num <= this.LoggerSpecifics.DataStorage.NumberOfMemories; num += 1u)
				{
					string text = string.Format(Constants.LogDataFileInSubFolderPrefixOnMemory, num);
					if (Path.GetFileName(logFile.DefaultName).IndexOf(text) == 0)
					{
						if (logFile.DefaultName.IndexOf('.') < 0)
						{
							if (num == 0u)
							{
								logFile.TypeName = Resources.FileManagerColFileTypeDriveRecLogData;
							}
							else
							{
								logFile.TypeName = Resources.FileManagerColFileTypeLogData;
								if (this.latestDateTimeUncompressed < logFile.Timestamp)
								{
									this.latestDateTimeUncompressed = logFile.Timestamp;
								}
								try
								{
									this.highestTriggerFileIndices[(int)((UIntPtr)(num - 1u))] = Math.Max(this.highestTriggerFileIndices[(int)((UIntPtr)(num - 1u))], Convert.ToUInt32(logFile.DefaultName.Remove(0, text.Length)));
								}
								catch (Exception)
								{
								}
							}
							logFile.IsConvertible = true;
							Dictionary<uint, uint> numOfLogFilesOnMemNr;
							uint key;
							(numOfLogFilesOnMemNr = this.numOfLogFilesOnMemNr)[key = num] = numOfLogFilesOnMemNr[key] + 1u;
							this.triggeredBuffers += 1u;
						}
						else if (string.Compare(Path.GetExtension(logFile.DefaultName), Vocabulary.FileExtensionDotGZ, true) == 0)
						{
							if (num == 0u)
							{
								logFile.TypeName = Resources.FileManagerColFileTypeCompDriveRecLogData;
							}
							else
							{
								logFile.TypeName = Resources.FileManagerColFileTypeCompLogData;
								if (this.latestDateTimeCompressed < logFile.Timestamp)
								{
									this.latestDateTimeCompressed = logFile.Timestamp;
								}
								try
								{
									string text2 = logFile.DefaultName.Remove(0, text.Length);
									text2 = text2.Remove(text2.Length - text.Length);
									this.highestTriggerFileIndices[(int)((UIntPtr)(num - 1u))] = Math.Max(this.highestTriggerFileIndices[(int)((UIntPtr)(num - 1u))], Convert.ToUInt32(text2));
								}
								catch (Exception)
								{
								}
							}
							logFile.IsConvertible = true;
							Dictionary<uint, uint> numOfCompLogFilesOnMemNr;
							uint key2;
							(numOfCompLogFilesOnMemNr = this.numOfCompLogFilesOnMemNr)[key2 = num] = numOfCompLogFilesOnMemNr[key2] + 1u;
							this.triggeredBuffers += 1u;
						}
						else if (string.Compare(Path.GetExtension(logFile.DefaultName), Vocabulary.FileExtensionDotGLX, true) == 0)
						{
							if (!this.hasIndexFile)
							{
								this.hasIndexFile = true;
							}
							logFile.TypeName = Resources.FileManagerColFileTypeNaviFile;
							logFile.IsConvertible = false;
						}
						else
						{
							logFile.TypeName = Resources.FileManagerColFileTypeUnknownLogData;
							logFile.IsConvertible = false;
						}
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					logFile.TypeName = FileSystemServices.CreateGenericFileTypeName(logFile.DefaultName);
					if (string.Compare(Path.GetExtension(logFile.DefaultName), Vocabulary.FileExtensionDotRCL, true) == 0)
					{
						logFile.IsConvertible = false;
						this.numOfClassificFiles += 1u;
					}
					else if (string.Compare(Path.GetExtension(logFile.DefaultName), Vocabulary.FileExtensionDotJPG, true) == 0)
					{
						logFile.IsConvertible = false;
						this.numOfJpegFiles += 1u;
					}
					else if (string.Compare(Path.GetExtension(logFile.DefaultName), Vocabulary.FileExtensionDotZIP, true) == 0)
					{
						logFile.IsConvertible = false;
						this.numOfZipArchives += 1u;
					}
					else if (string.Compare(Path.GetExtension(logFile.DefaultName), Vocabulary.FileExtensionDotWAV, true) == 0)
					{
						logFile.IsConvertible = false;
						this.numOfWavFiles += 1u;
					}
					else if (string.Compare(logFile.DefaultName, Path.GetFileName(this.LoggerSpecifics.DataStorage.ErrorFilePath), true) == 0)
					{
						logFile.IsConvertible = false;
					}
					else
					{
						logFile.IsConvertible = false;
					}
				}
				if (this.LoggerSpecifics.FileConversion.HasSelectableLogFiles)
				{
					logFile.IsSelected = logFile.IsConvertible;
				}
				this.logFiles.Add(logFile);
			}
			this.isPrimaryFileGroupCompressed = false;
			if (this.latestDateTimeCompressed > this.latestDateTimeUncompressed)
			{
				this.isPrimaryFileGroupCompressed = true;
			}
			if (this.LoggerSpecifics.DataStorage.NumberOfMemories > 1u)
			{
				bool flag2 = false;
				bool flag3 = false;
				for (uint num2 = 1u; num2 <= this.LoggerSpecifics.DataStorage.NumberOfMemories; num2 += 1u)
				{
					if (this.numOfLogFilesOnMemNr[num2] > 0u)
					{
						flag3 = true;
					}
					if (this.numOfCompLogFilesOnMemNr[num2] > 0u)
					{
						flag2 = true;
					}
				}
				this.hasMixedCompUncompFiles = (flag2 && flag3);
				if (this.hasMixedCompUncompFiles)
				{
					DateTime t = this.latestDateTimeCompressed;
					if (this.isPrimaryFileGroupCompressed)
					{
						t = this.latestDateTimeUncompressed;
					}
					using (IEnumerator<ILogFile> enumerator2 = this.logFiles.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							LogFile logFile2 = (LogFile)enumerator2.Current;
							if (logFile2.Timestamp <= t)
							{
								logFile2.IsSecondary = true;
							}
						}
					}
				}
			}
			string destSubFolderNamePrimary;
			if (FileSystemServices.GenerateDestinationFolderNameFromIniFile(Path.Combine(path, this.LoggerSpecifics.DataStorage.LogDataIniFileName), out destSubFolderNamePrimary))
			{
				this.destSubFolderNamePrimary = destSubFolderNamePrimary;
				this.isIniFileMissing = false;
			}
			else
			{
				base.GenerateFolderNameFromLatestFileDate(out destSubFolderNamePrimary);
				this.destSubFolderNamePrimary = destSubFolderNamePrimary;
				this.isIniFileMissing = true;
			}
			if (this.hasMixedCompUncompFiles)
			{
				if (this.isPrimaryFileGroupCompressed)
				{
					this.destSubFolderNameSecondary = GUIUtil.ConvertDateTimeToFolderName(base.LatestDateTimeUncompressedLogFiles);
					return;
				}
				this.destSubFolderNameSecondary = GUIUtil.ConvertDateTimeToFolderName(base.LatestDateTimeCompressedLogfiles);
			}
		}

		public override bool CopyAndBatchExportSelectedLogFiles(FileConversionParameters conversionParameters, ConversionJob allInOneJob, string pathToExportBatchFile, ref string destination)
		{
			if (!this.isMemoryCardReady || base.NumberOfTriggeredBuffers <= 0u)
			{
				return false;
			}
			string message = "";
			string path = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
			string text = Path.Combine(allInOneJob.FileConversionParameters.DestinationFolder, path);
			if (!FileSystemServices.EnsureDirectoryExistence(text))
			{
				InformMessageBox.Error(string.Format(Resources.ErrorUnableToCreateFolder, text));
				return false;
			}
			string[] files = Directory.GetFiles(text);
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				string filename = array[i];
				if (!GenerationUtil.TryDeleteFile(filename, out message))
				{
					InformMessageBox.Error(message);
					return false;
				}
			}
			long num = 0L;
			List<string> list = new List<string>();
			foreach (ZipEntry current in this.zipFile)
			{
				string fileName = Path.GetFileName(current.FileName);
				if (allInOneJob.SelectedFileNames.Contains(fileName))
				{
					num += current.UncompressedSize;
					list.Add(current.FileName);
					allInOneJob.SelectedFileNames.Remove(fileName);
				}
			}
			string text2 = this.rootBasePath + this.LoggerSpecifics.DataStorage.LogDataIniFileName;
			string text3 = this.rootBasePath + this.LoggerSpecifics.DataStorage.LogDataIniFile2Name;
			if (this.zipFile[text2] != null)
			{
				list.Add(text2);
			}
			if (this.zipFile[text3] != null)
			{
				list.Add(text3);
			}
			List<string> collection;
			long num2;
			if (this.DetectAndPromptForCopyingMultimediaFiles(text, out collection, out num2))
			{
				list.AddRange(collection);
				num += num2;
			}
			if (!base.CheckAvailableSpaceOnDestination(conversionParameters.DestinationFormat, num, text, true))
			{
				return false;
			}
			int num3 = 2;
			if (this.isPrimaryFileGroupCompressed)
			{
				num3 = 3;
			}
			int num4 = 1;
			string titleText = string.Format(Resources.StepNumOfTotal, num4, num3) + Resources.TitleCopyFilesFromMemoryCard;
			CopyFilesWithProgressIndicatorFromZip copyFilesWithProgressIndicatorFromZip = new CopyFilesWithProgressIndicatorFromZip(this.zipFile, this.hardwareKey, list, text, false, true, titleText);
			if (copyFilesWithProgressIndicatorFromZip.ShowDialog() == DialogResult.Cancel)
			{
				InformMessageBox.Info(Resources.ConversionAborted);
				return false;
			}
			if (this.isPrimaryFileGroupCompressed)
			{
				num4++;
				if (!base.DecompressRawDataFiles(text, num3, num4, false))
				{
					return false;
				}
			}
			ExecuteBatchFile executeBatchFile = new ExecuteBatchFile(pathToExportBatchFile, new List<string>
			{
				text
			});
			num4++;
			executeBatchFile.Text = string.Format(Resources.StepNumOfTotal, num4, num3) + Resources.TitleExecuteBatchFile;
			if (executeBatchFile.ShowDialog() == DialogResult.Cancel)
			{
				if (executeBatchFile.IsAbortedByUser)
				{
					InformMessageBox.Info(Resources.ConversionAborted);
				}
				else
				{
					InformMessageBox.Error(executeBatchFile.ErrorText);
				}
				return false;
			}
			destination = text;
			return true;
		}

		protected new bool DetectAndPromptForCopyingMultimediaFiles(string destFolderPath, out List<string> sourceFilePaths, out long totalSizeOfFilesInBytes)
		{
			sourceFilePaths = new List<string>();
			totalSizeOfFilesInBytes = 0L;
			if (this.numOfClassificFiles > 0u || this.numOfJpegFiles > 0u || this.LogFileStorage.NumberOfWavFiles > 0u || this.LogFileStorage.NumberOfZipArchives > 0u)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(string.Format(Resources.AdditionalLogFilesOnDir, this.hardwareKey));
				if (this.LogFileStorage.NumberOfWavFiles > 0u)
				{
					stringBuilder.AppendLine(string.Format("{0} {1}", this.LogFileStorage.NumberOfWavFiles, Resources.AudioFiles));
				}
				if (this.LogFileStorage.NumberOfJpegFiles > 0u)
				{
					stringBuilder.AppendLine(string.Format("{0} {1}", this.LogFileStorage.NumberOfJpegFiles, Resources.ImageFiles));
				}
				if (this.LogFileStorage.NumberOfZipArchives > 0u)
				{
					stringBuilder.AppendLine(string.Format("{0} {1}", this.LogFileStorage.NumberOfZipArchives, Resources.ImageArchives));
				}
				if (this.LogFileStorage.NumberOfClassificFiles > 0u)
				{
					stringBuilder.AppendLine(string.Format("{0} {1}", this.LogFileStorage.NumberOfClassificFiles, Resources.ClassificFiles));
				}
				stringBuilder.AppendLine(string.Format(Resources.QuestionCopyFilesToDestDir, destFolderPath));
				if (InformMessageBox.Question(stringBuilder.ToString()) == DialogResult.Yes)
				{
					foreach (ILogFile current in this.LogFileStorage.LogFiles)
					{
						if (!current.IsConvertible)
						{
							string extension = Path.GetExtension(current.DefaultName);
							if (string.Compare(extension, Vocabulary.FileExtensionDotRCL, true) == 0 || string.Compare(extension, Vocabulary.FileExtensionDotJPG, true) == 0 || string.Compare(extension, Vocabulary.FileExtensionDotWAV, true) == 0 || string.Compare(extension, Vocabulary.FileExtensionDotZIP, true) == 0)
							{
								string text = this.zipFile[""] + current.DefaultName;
								if (this.zipFile[text] != null)
								{
									sourceFilePaths.Add(text);
								}
							}
						}
					}
					return true;
				}
			}
			return false;
		}

		protected void UpdateFromMlRtIniFile()
		{
			string logDataIniFileName = this.LoggerSpecifics.DataStorage.LogDataIniFileName;
			string logDataIniFile2Name = this.LoggerSpecifics.DataStorage.LogDataIniFile2Name;
			if (this.isOnline && !this.FileExistsInZip(logDataIniFileName) && !this.FileExistsInZip(logDataIniFile2Name))
			{
				return;
			}
			this.iniFilePropertiesAndValues.Clear();
			if (this.FileExistsInZip(logDataIniFileName))
			{
				this.GetIniFilePropertiesAndValuesFromZip(logDataIniFileName, ref this.iniFilePropertiesAndValues);
			}
			else if (this.FileExistsInZip(this.rootBasePath + logDataIniFileName))
			{
				this.GetIniFilePropertiesAndValuesFromZip(this.rootBasePath + logDataIniFileName, ref this.iniFilePropertiesAndValues);
			}
			if (this.FileExistsInZip(this.rootBasePath + logDataIniFile2Name))
			{
				this.GetIniFilePropertiesAndValuesFromZip(logDataIniFile2Name, ref this.iniFilePropertiesAndValues);
			}
			else if (this.FileExistsInZip(this.rootBasePath + logDataIniFile2Name))
			{
				this.GetIniFilePropertiesAndValuesFromZip(logDataIniFile2Name, ref this.iniFilePropertiesAndValues);
			}
			this.name = "";
			if (this.iniFilePropertiesAndValues.ContainsKey(MlRtIniFile.Key_FileName))
			{
				this.name = this.iniFilePropertiesAndValues[MlRtIniFile.Key_FileName];
			}
			this.hasConfiguration = !string.IsNullOrEmpty(this.name);
			if (this.iniFilePropertiesAndValues.ContainsKey(MlRtIniFile.Key_CompilationTimeStamp))
			{
				MlRtIniFile.ParseDateTimeField(this.iniFilePropertiesAndValues[MlRtIniFile.Key_CompilationTimeStamp], ref this.compileDateTime);
			}
			this.serialNumber = "";
			if (this.iniFilePropertiesAndValues.ContainsKey(MlRtIniFile.Key_SerNum))
			{
				this.serialNumber = this.iniFilePropertiesAndValues[MlRtIniFile.Key_SerNum];
			}
			this.hasLoggerInfo = !string.IsNullOrEmpty(this.serialNumber);
			this.vehicleName = "";
			if (this.iniFilePropertiesAndValues.ContainsKey(MlRtIniFile.Key_LocalCarName))
			{
				this.vehicleName = this.iniFilePropertiesAndValues[MlRtIniFile.Key_LocalCarName];
			}
			this.installedLicenses = "";
			StringBuilder stringBuilder = new StringBuilder();
			int num = 1;
			bool flag;
			do
			{
				flag = false;
				string key = string.Format(MlRtIniFile.Key_License_Num, num);
				if (this.iniFilePropertiesAndValues.ContainsKey(key))
				{
					flag = true;
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(", ");
					}
					stringBuilder.Append(this.iniFilePropertiesAndValues[key]);
					num++;
				}
			}
			while (flag);
			this.installedLicenses = stringBuilder.ToString();
			this.firmwareVersion = "";
			if (this.iniFilePropertiesAndValues.ContainsKey(MlRtIniFile.Key_RTSversion))
			{
				this.firmwareVersion = this.iniFilePropertiesAndValues[MlRtIniFile.Key_RTSversion];
			}
			this.logDiskInfo = "";
			if (this.iniFilePropertiesAndValues.ContainsKey(MlRtIniFile.Key_LogDiskInfo))
			{
				this.logDiskInfo = this.iniFilePropertiesAndValues[MlRtIniFile.Key_LogDiskInfo];
			}
		}

		private bool FileExistsInZip(string filename)
		{
			return this.zipFile != null && this.zipFile[filename] != null;
		}

		public bool GetIniFilePropertiesAndValuesFromZip(string filepath, ref Dictionary<string, string> propertiesAndValues)
		{
			if (this.zipFile[filepath] == null)
			{
				return false;
			}
			bool result;
			try
			{
				ZipEntry zipEntry = this.zipFile[filepath];
				Stream stream = new MemoryStream();
				zipEntry.Extract(stream);
				stream.Seek(0L, SeekOrigin.Begin);
				StreamReader streamReader = new StreamReader(stream);
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					if (text.Length > 0)
					{
						int num = text.IndexOf('=');
						if (num >= 0)
						{
							string key = text.Substring(0, num);
							string value = "";
							if (num + 1 < text.Length)
							{
								value = text.Substring(num + 1);
							}
							if (!propertiesAndValues.ContainsKey(key))
							{
								propertiesAndValues.Add(key, value);
							}
						}
					}
				}
				streamReader.Close();
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public bool IsZipFileContentCompatibleToLogger()
		{
			string strB = "0x" + this.LoggerSpecifics.DeviceAccess.DeviceType.ToString("X");
			bool result = false;
			if (this.iniFilePropertiesAndValues.ContainsKey(MlRtIniFile.Key_DevType))
			{
				string strA = this.iniFilePropertiesAndValues[MlRtIniFile.Key_DevType];
				if (string.Compare(strA, strB) == 0)
				{
					result = true;
				}
			}
			return result;
		}
	}
}
