using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Conversion;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.GUI.Common.DeviceInteraction;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public abstract class LoggerDeviceWindowsFileSystem : LoggerDeviceCommon
	{
		private static readonly string configureDirectoryName = "configure";

		private static readonly string ignoreFileName = "ignore";

		protected static readonly string _ClassifyFolderName = "Classify";

		internal Dictionary<string, string> iniFilePropertiesAndValues;

		public override DateTime CompileDateTime
		{
			get
			{
				bool flag;
				this.UpdateFromMlRtIniFile(out flag);
				return this.compileDateTime;
			}
		}

		public override bool HasErrorFile
		{
			get
			{
				string path = Path.Combine(this.hardwareKey, this.LoggerSpecifics.DataStorage.ErrorFilePath);
				return FileProxy.Exists(path);
			}
		}

		public override bool IsFAT32Formatted
		{
			get
			{
				return FileSystemServices.HasFAT32FileSystem(this.hardwareKey);
			}
		}

		public override bool HasSnapshotFolderContainingLogData
		{
			get
			{
				return this.IsSnapshotFolderContainingLogData();
			}
		}

		public LoggerDeviceWindowsFileSystem()
		{
			this.iniFilePropertiesAndValues = new Dictionary<string, string>();
		}

		public override string GetGenericTransceiverTypeName(int channelNumber)
		{
			string propertyName = string.Format(MlRtIniFile.Key_CAN_num_baby, channelNumber);
			string result = "";
			string text;
			if (this.GetMlRtIniFilePropertyValue(propertyName, out text) && !string.IsNullOrEmpty(text))
			{
				int startIndex = text.IndexOf('(');
				result = text.Substring(startIndex);
			}
			return result;
		}

		public override bool Clear()
		{
			return this.ClearWindowsFormattedMedia();
		}

		public override bool WriteConfiguration(out string codMD5Hash, bool showProgressBar)
		{
			return this.WriteConfigurationToWindowsFormattedMedia(out codMD5Hash, showProgressBar);
		}

		public override bool WriteConfiguration(string codFilePath, bool showProgressBar)
		{
			return this.WriteConfigurationToWindowsFormattedMedia(codFilePath, showProgressBar) == Result.OK;
		}

		public override bool WriteAnalysisPackage(string analysisPackagePath)
		{
			return this.WriteAnalysisPackageToWindowsFormattedMedia(analysisPackagePath);
		}

		public override bool WriteProjectZIPFile(string zipFilePath)
		{
			return this.WriteProjectZIPFileToWindowsFormattedMedia(zipFilePath);
		}

		public override bool DownloadProjectZIPFile()
		{
			throw new NotImplementedException();
		}

		public override string[] GetProjectZIPFilePath()
		{
			return this.GetProjectZIPFilePathFromWindowsFormattedMedia();
		}

		public override bool CopyAnalysisPackage(string destFolder)
		{
			try
			{
				if (!this.HasAnalysisPackage())
				{
					bool result = false;
					return result;
				}
				string text = Path.Combine(destFolder, Path.GetFileName(this.GetAnalysisPackagePath()));
				bool flag = false;
				if (File.Exists(text))
				{
					if (InformMessageBox.Question(string.Format(Resources.FileAlreadyExistsOverwrite, text)) == DialogResult.Yes)
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (!flag)
				{
					bool result = true;
					return result;
				}
				File.Copy(this.GetAnalysisPackagePath(), text, true);
			}
			catch (Exception ex)
			{
				InformMessageBox.Error(ex.Message);
				bool result = false;
				return result;
			}
			return true;
		}

		public override bool HasAnalysisPackage()
		{
			return !string.IsNullOrEmpty(this.GetAnalysisPackagePath());
		}

		public override string GetAnalysisPackagePath()
		{
			string path = Path.Combine(base.HardwareKey, Vocabulary.FolderNameAnalysisPackage);
			if (DirectoryProxy.Exists(path))
			{
				try
				{
					string[] files = DirectoryProxy.GetFiles(path);
					string[] array = files;
					for (int i = 0; i < array.Length; i++)
					{
						string text = array[i];
						if (text.EndsWith("analysis.zip", true, CultureInfo.InvariantCulture))
						{
							string result = text;
							return result;
						}
					}
				}
				catch
				{
					string result = string.Empty;
					return result;
				}
			}
			return string.Empty;
		}

		public override string[] GetCODFilePath()
		{
			return this.GetCODFilePathFromWindowsFormattedMedia();
		}

		public override bool UpdateFileList()
		{
			return this.CreateFileListFromWindowsFormattedMedia();
		}

		public override bool DeleteAllLogFiles()
		{
			return this.DeleteAllFilesFromWindowsFormattedMedia();
		}

		public override bool ConvertAllLogFiles(FileConversionParameters conversionParameters, DatabaseConfiguration databaseConfiguration, string configurationFolderPath)
		{
			if (this.isMemoryCardReady && base.NumberOfTriggeredBuffers > 0u)
			{
				this.ConvertAllLogFilesFromWindowsFormattedMedia(conversionParameters, databaseConfiguration, configurationFolderPath);
				return true;
			}
			return false;
		}

		public override bool ConvertSelectedLogFiles(FileConversionParameters conversionParameters, List<ConversionJob> conversionJobs, DatabaseConfiguration databaseConfiguration, string configurationFolderPath)
		{
			if (!this.isMemoryCardReady || base.NumberOfTriggeredBuffers <= 0u || conversionJobs.Count <= 0)
			{
				return false;
			}
			string text = Path.Combine(conversionJobs[0].FileConversionParameters.DestinationFolder, this.destSubFolderNamePrimary);
			if (!FileSystemServices.EnsureDirectoryExistence(text))
			{
				InformMessageBox.Error(string.Format(Resources.ErrorUnableToCreateFolder, text));
				return false;
			}
			string text2 = "";
			if (!FileSystemServices.TryCreateTempDirectory(text, out text2))
			{
				InformMessageBox.Error(Resources.ErrorCannotCreateTemporaryDirectory);
				return false;
			}
			string text3 = "";
			if (!FileSystemServices.TryCreateTempDirectory(text, out text3))
			{
				InformMessageBox.Error(Resources.ErrorCannotCreateTemporaryDirectory);
				return false;
			}
			ReadOnlyCollection<string> readOnlyCollection = null;
			if (FileConversionHelper.IsSignalOrientedDestFormat(conversionParameters.DestinationFormat) && FileConversionHelper.UseArxmlToDBCConversion(conversionParameters))
			{
				IEnumerable<Database> conversionDatabases = AnalysisPackage.GetConversionDatabases(conversionParameters, databaseConfiguration.Databases, true);
				if (conversionDatabases != null && conversionDatabases.Any<Database>() && !GenerateDBCfromARXML.Execute(conversionDatabases, configurationFolderPath, text2, out readOnlyCollection))
				{
					InformMessageBox.Info(Resources.ConversionAborted);
					return true;
				}
			}
			if (conversionJobs[0].FileConversionParameters.SingleFile)
			{
				if (conversionJobs[0].Type == ConversionJobType.Marker)
				{
					List<ConversionJob> list = new List<ConversionJob>();
					string text4 = "inv";
					ConversionJob conversionJob = null;
					foreach (ConversionJob current in conversionJobs)
					{
						if (text4 != current.MemNumber)
						{
							text4 = current.MemNumber;
							conversionJob = new ConversionJob("AllConversionsInOneFile", current.Type, 0u, text4);
							list.Add(conversionJob);
							conversionJob.ExtractStart = current.ExtractStart;
							conversionJob.ExtractTList = new List<Tuple<int, int>>();
							conversionJob.FileConversionParameters = current.FileConversionParameters;
						}
						if (conversionJob != null)
						{
							conversionJob.SelectedFileNames.UnionWith(current.SelectedFileNames);
							conversionJob.ExtractTList.Add(Tuple.Create<int, int>(current.ExtractT1, current.ExtractT2));
						}
					}
					conversionJobs.Clear();
					conversionJobs.AddRange(list);
				}
				else
				{
					ConversionJob conversionJob2 = new ConversionJob("AllConversionsInOneFile", conversionJobs[0].Type, 0u);
					conversionJob2.ExtractStart = conversionJobs[0].ExtractStart;
					conversionJob2.ExtractTList = new List<Tuple<int, int>>();
					foreach (ConversionJob current2 in conversionJobs)
					{
						conversionJob2.SelectedFileNames.UnionWith(current2.SelectedFileNames);
						conversionJob2.ExtractTList.Add(Tuple.Create<int, int>(current2.ExtractT1, current2.ExtractT2));
						conversionJob2.AddBeginEndForLogFilesFromList(current2.ListOfBeginEndForLogFiles);
					}
					conversionJob2.FileConversionParameters = conversionJobs[0].FileConversionParameters;
					conversionJobs.Clear();
					conversionJobs.Add(conversionJob2);
				}
			}
			ConversionJobProcessor conversionJobProcessor = new ConversionJobProcessor(conversionJobs, text, text3, text2, databaseConfiguration, configurationFolderPath, this);
			conversionJobProcessor.Start();
			FileSystemServices.TryDeleteDirectory(text2);
			FileSystemServices.TryDeleteDirectory(text3);
			if (conversionParameters.GenerateVsysvarFile)
			{
				this.GenerateVSysVarFile(conversionParameters.DestinationFormat, this.LoggerSpecifics.Type, this.hardwareKey, this.LoggerSpecifics.DataStorage.LogDataIniFile2Name, text);
			}
			List<string> list2 = new List<string>();
			if (conversionParameters.CopyMediaFiles)
			{
				IList<string> mediaFilePaths = this.GetMediaFilePaths(conversionJobs, this.hardwareKey);
				list2.AddRange(mediaFilePaths);
			}
			if (list2.Count > 0)
			{
				long num = 0L;
				IList<string> filesNotPresentInDestFolder = FileSystemServices.GetFilesNotPresentInDestFolder(text, list2, out num);
				CopyFilesWithProgressIndicator copyFilesWithProgressIndicator = new CopyFilesWithProgressIndicator(filesNotPresentInDestFolder, text, true, Resources.TitleCopyFilesFromMemoryCard);
				if (copyFilesWithProgressIndicator.ShowDialog() == DialogResult.Cancel)
				{
					InformMessageBox.Info(Resources.CopyingFilesAborted);
					return true;
				}
			}
			return true;
		}

		public bool DetectAndPromptForCopyingMultimediaFiles(string destFolderPath, out List<string> sourceFilePaths, out long totalSizeOfFilesInBytes)
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
					sourceFilePaths = this.GetAllMultimediaFilePaths();
					return true;
				}
			}
			return false;
		}

		public List<string> GetAllMultimediaFilePaths()
		{
			List<string> list = new List<string>();
			foreach (ILogFile current in this.LogFileStorage.LogFiles)
			{
				if (!current.IsConvertible)
				{
					string extension = Path.GetExtension(current.DefaultName);
					if (string.Compare(extension, Vocabulary.FileExtensionDotRCL, true) == 0 || string.Compare(extension, Vocabulary.FileExtensionDotJPG, true) == 0 || string.Compare(extension, Vocabulary.FileExtensionDotWAV, true) == 0 || string.Compare(extension, Vocabulary.FileExtensionDotZIP, true) == 0)
					{
						string text = this.hardwareKey.TrimEnd(new char[]
						{
							Path.DirectorySeparatorChar
						}) + Path.DirectorySeparatorChar + current.DefaultName;
						if (FileProxy.Exists(text))
						{
							list.Add(text);
						}
					}
				}
			}
			return list;
		}

		private IList<string> GetMediaFilePaths(IList<ConversionJob> conversionJobs, string sourceFolder)
		{
			List<string> list = new List<string>();
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			HashSet<string> hashSet3 = new HashSet<string>();
			foreach (ConversionJob current in conversionJobs)
			{
				if (current.Type == ConversionJobType.File)
				{
					using (HashSet<string>.Enumerator enumerator2 = current.SelectedFileNames.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							string current2 = enumerator2.Current;
							Tuple<DateTime, DateTime> beginEndForLogFile = current.GetBeginEndForLogFile(current2);
							if (beginEndForLogFile != null)
							{
								DateTime item = beginEndForLogFile.Item1;
								int durationInSeconds = (int)(beginEndForLogFile.Item2 - beginEndForLogFile.Item1).TotalSeconds + 1;
								IList<string> imageFilePaths = FileSystemServices.GetImageFilePaths(sourceFolder, item, durationInSeconds);
								foreach (string current3 in imageFilePaths)
								{
									hashSet3.Add(current3);
								}
								IList<string> imageArchivePaths = FileSystemServices.GetImageArchivePaths(sourceFolder, item, durationInSeconds);
								foreach (string current4 in imageArchivePaths)
								{
									hashSet2.Add(current4);
								}
								IList<string> audioFilePaths = FileSystemServices.GetAudioFilePaths(sourceFolder, item, durationInSeconds);
								foreach (string current5 in audioFilePaths)
								{
									hashSet.Add(current5);
								}
							}
						}
						continue;
					}
				}
				if (current.ExtractTList != null && current.ExtractTList.Count > 0)
				{
					using (List<Tuple<int, int>>.Enumerator enumerator6 = current.ExtractTList.GetEnumerator())
					{
						while (enumerator6.MoveNext())
						{
							Tuple<int, int> current6 = enumerator6.Current;
							DateTime startTime = current.ExtractStart + new TimeSpan(0, 0, current6.Item1);
							int durationInSeconds2 = current6.Item2 - current6.Item1;
							IList<string> imageFilePaths2 = FileSystemServices.GetImageFilePaths(sourceFolder, startTime, durationInSeconds2);
							foreach (string current7 in imageFilePaths2)
							{
								hashSet3.Add(current7);
							}
							IList<string> imageArchivePaths2 = FileSystemServices.GetImageArchivePaths(sourceFolder, startTime, durationInSeconds2);
							foreach (string current8 in imageArchivePaths2)
							{
								hashSet2.Add(current8);
							}
							IList<string> audioFilePaths2 = FileSystemServices.GetAudioFilePaths(sourceFolder, startTime, durationInSeconds2);
							foreach (string current9 in audioFilePaths2)
							{
								hashSet.Add(current9);
							}
						}
						continue;
					}
				}
				DateTime startTime2 = current.ExtractStart + new TimeSpan(0, 0, current.ExtractT1);
				int durationInSeconds3 = current.ExtractT2 - current.ExtractT1;
				IList<string> imageFilePaths3 = FileSystemServices.GetImageFilePaths(sourceFolder, startTime2, durationInSeconds3);
				foreach (string current10 in imageFilePaths3)
				{
					hashSet3.Add(current10);
				}
				IList<string> imageArchivePaths3 = FileSystemServices.GetImageArchivePaths(sourceFolder, startTime2, durationInSeconds3);
				foreach (string current11 in imageArchivePaths3)
				{
					hashSet2.Add(current11);
				}
				IList<string> audioFilePaths3 = FileSystemServices.GetAudioFilePaths(sourceFolder, startTime2, durationInSeconds3);
				foreach (string current12 in audioFilePaths3)
				{
					hashSet.Add(current12);
				}
			}
			list.AddRange(hashSet3);
			list.AddRange(hashSet2);
			list.AddRange(hashSet);
			return list;
		}

		private IList<string> GetClassificationFilePaths(string sourceFolder)
		{
			List<string> list = new List<string>();
			try
			{
				string[] files = DirectoryProxy.GetFiles(sourceFolder, "*" + Vocabulary.FileExtensionDotRCL, SearchOption.TopDirectoryOnly);
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string item = array[i];
					list.Add(item);
				}
			}
			catch
			{
			}
			return list;
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
			bool flag = true;
			long num = 0L;
			List<string> list = new List<string>();
			foreach (FileInfoProxy current in this.GetFiles(this.hardwareKey, out flag))
			{
				if (allInOneJob.SelectedFileNames.Contains(current.Name))
				{
					num += current.Length;
					list.Add(current.FullName);
					allInOneJob.SelectedFileNames.Remove(current.Name);
				}
			}
			string text2 = Path.Combine(this.hardwareKey, this.LoggerSpecifics.DataStorage.LogDataIniFileName);
			string text3 = Path.Combine(this.hardwareKey, this.LoggerSpecifics.DataStorage.LogDataIniFile2Name);
			if (File.Exists(text2))
			{
				list.Add(text2);
			}
			if (File.Exists(text3))
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
			CopyFilesWithProgressIndicator copyFilesWithProgressIndicator = new CopyFilesWithProgressIndicator(this.hardwareKey, list, text, false, true, titleText);
			if (copyFilesWithProgressIndicator.ShowDialog() == DialogResult.Cancel)
			{
				InformMessageBox.Info(Resources.ConversionAborted);
				return false;
			}
			if (this.isPrimaryFileGroupCompressed)
			{
				num4++;
				if (!this.DecompressRawDataFiles(text, num3, num4, false))
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
			GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.ClfSourceFolders, text);
			destination = text;
			return true;
		}

		protected void ExtractCODCompileTime()
		{
			this.name = "";
			this.compileDateTime = DateTime.MinValue;
			string path = Path.Combine(this.hardwareKey, "configure");
			if (DirectoryProxy.Exists(path))
			{
				try
				{
					string[] files = DirectoryProxy.GetFiles(path, "*.cod");
					if (files.Count<string>() > 0)
					{
						this.name = Path.GetFileName(files[0]);
						this.compileDateTime = FileSystemServices.ExtractCODCompileTime(files[0]);
					}
				}
				catch (Exception)
				{
				}
			}
		}

		protected void UpdateFromMlRtIniFile(out bool isTransceiverInfoAvailable)
		{
			isTransceiverInfoAvailable = false;
			string text = Path.Combine(this.hardwareKey, this.LoggerSpecifics.DataStorage.LogDataIniFileName);
			string text2 = Path.Combine(this.hardwareKey, this.LoggerSpecifics.DataStorage.LogDataIniFile2Name);
			this.compileDateTime = DateTime.MinValue;
			if (this.isOnline && !FileProxy.Exists(text) && !FileProxy.Exists(text2))
			{
				return;
			}
			this.iniFilePropertiesAndValues.Clear();
			if (FileProxy.Exists(text))
			{
				FileSystemServices.GetIniFilePropertiesAndValues(text, ref this.iniFilePropertiesAndValues);
			}
			if (FileProxy.Exists(text2))
			{
				FileSystemServices.GetIniFilePropertiesAndValues(text2, ref this.iniFilePropertiesAndValues);
				isTransceiverInfoAvailable = true;
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
			string value;
			if (this.GetHostCamLicenses(out value))
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(value);
			}
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

		protected bool GetHostCamLicenses(out string licenseString)
		{
			licenseString = string.Empty;
			int num = 1;
			bool flag = true;
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			bool flag2;
			do
			{
				string key = string.Format(MlRtIniFile.Key_HostCamEnabled_Num, num);
				string key2 = string.Format(MlRtIniFile.Key_HostCamSerNum_Num, num);
				if (this.iniFilePropertiesAndValues.ContainsKey(key))
				{
					flag2 = true;
					string a = this.iniFilePropertiesAndValues[key].Trim();
					if (a == "1")
					{
						if (this.iniFilePropertiesAndValues.ContainsKey(key2))
						{
							dictionary.Add(num, this.iniFilePropertiesAndValues[key2]);
						}
					}
					else
					{
						flag = false;
					}
				}
				else
				{
					flag2 = false;
					if (num <= 1)
					{
						flag = false;
					}
				}
				num++;
			}
			while (flag2);
			if (flag && dictionary.Count == 0)
			{
				licenseString = Resources.LoggerBasedHostCAMLic;
			}
			else if (dictionary.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (KeyValuePair<int, string> current in dictionary)
				{
					if (stringBuilder.Length == 0)
					{
						stringBuilder.Append(string.Format(Resources.LicensedHostCAMOn, current.Key, current.Value));
					}
					else
					{
						stringBuilder.Append(string.Format(", {0} ({1})", current.Key, current.Value));
					}
				}
				licenseString = stringBuilder.ToString();
			}
			return !string.IsNullOrEmpty(licenseString);
		}

		protected bool IsWLANBoardInstalled(out string installedWLANModelName)
		{
			installedWLANModelName = "";
			for (int i = 1; i < 16; i++)
			{
				string propertyName = string.Format(MlRtIniFile.Key_ExtensionBoard_Num, i);
				string text;
				if (this.GetMlRtIniFilePropertyValue(propertyName, out text) && !string.IsNullOrEmpty(text) && text.IndexOf(MlRtIniFile.Value_WLANExtensionBoard_Prefix) == 0 && text.Length > MlRtIniFile.Value_WLANExtensionBoard_Prefix.Length + 1)
				{
					installedWLANModelName = text.Substring(MlRtIniFile.Value_WLANExtensionBoard_Prefix.Length, text.Length - MlRtIniFile.Value_WLANExtensionBoard_Prefix.Length - 1);
					return true;
				}
			}
			return false;
		}

		protected bool IsAnalogInputBoardInstalled()
		{
			for (int i = 1; i < 16; i++)
			{
				string propertyName = string.Format(MlRtIniFile.Key_ExtensionBoard_Num, i);
				string text;
				if (this.GetMlRtIniFilePropertyValue(propertyName, out text) && !string.IsNullOrEmpty(text) && string.Compare(MlRtIniFile.Value_AnalogExtensionBoard, text) == 0)
				{
					return true;
				}
			}
			return false;
		}

		protected CANTransceiverType GetMlRtIniFileTransceiverType(int channelNumber, CANTransceiverType defaultType)
		{
			CANTransceiverType result = defaultType;
			string propertyName = string.Format(MlRtIniFile.Key_CAN_num_baby, channelNumber);
			string encoding;
			if (this.GetMlRtIniFilePropertyValue(propertyName, out encoding))
			{
				result = MlRtIniFile.ParseTransceiverEncoding(encoding);
			}
			return result;
		}

		protected bool GetMlRtIniFilePropertyValue(string propertyName, out string propertyValue)
		{
			propertyValue = "";
			if (this.iniFilePropertiesAndValues.ContainsKey(propertyName))
			{
				propertyValue = this.iniFilePropertiesAndValues[propertyName];
				return true;
			}
			return false;
		}

		protected bool IsWindowsFormattedMediaAccessible()
		{
			if (!DirectoryProxy.Exists(this.hardwareKey))
			{
				return false;
			}
			string pathRoot = Path.GetPathRoot(this.hardwareKey);
			if (string.IsNullOrEmpty(pathRoot))
			{
				return false;
			}
			bool flag = false;
			DriveInfo driveInfo = null;
			DriveInfo[] drives = DriveInfo.GetDrives();
			DriveInfo[] array = drives;
			for (int i = 0; i < array.Length; i++)
			{
				DriveInfo driveInfo2 = array[i];
				if (driveInfo2.Name == pathRoot)
				{
					flag = true;
					driveInfo = driveInfo2;
					break;
				}
			}
			if (pathRoot[0] == Path.DirectorySeparatorChar && pathRoot[1] == Path.DirectorySeparatorChar)
			{
				base.IsLocatedAtNetwork = true;
				if (Directory.Exists(pathRoot))
				{
					return true;
				}
			}
			if (!flag)
			{
				return false;
			}
			if (driveInfo != null)
			{
				try
				{
					if ((driveInfo.DriveFormat == Constants.FileSystemFormatFAT || driveInfo.DriveFormat == Constants.FileSystemFormatFAT32 || driveInfo.DriveFormat == Constants.FileSystemFormatNTFS) && driveInfo.IsReady)
					{
						bool result = true;
						return result;
					}
				}
				catch (Exception)
				{
					bool result = false;
					return result;
				}
				return false;
			}
			return false;
		}

		protected bool IsIndexFileExisting()
		{
			if (!this.isLogFileStorageOutdated)
			{
				return this.hasIndexFile;
			}
			bool flag;
			IEnumerable<FileInfoProxy> files = this.GetFiles(base.HardwareKey, out flag);
			foreach (FileInfoProxy current in files)
			{
				if (string.Compare(current.Extension, Vocabulary.FileExtensionDotGLX, true) == 0)
				{
					return true;
				}
			}
			return false;
		}

		protected bool IsSnapshotFolderContainingLogData()
		{
			string text = "";
			try
			{
				text = Path.GetFileName(base.HardwareKey.TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar
				}));
			}
			catch (Exception)
			{
			}
			if (!string.IsNullOrEmpty(text) && string.Compare(text, LoggerDeviceCommon._SnapshotFolderName, true) == 0)
			{
				return false;
			}
			List<string> list;
			if (DirectoryProxy.Exists(base.SnapshotFolderPath) && FileSystemServices.HasSubFolders(base.SnapshotFolderPath, Constants.LogDataFolderSearchPattern, out list))
			{
				List<FileInfoProxy> list2 = new List<FileInfoProxy>();
				foreach (string current in list)
				{
					list2.AddRange(FileSystemServices.GetFiles(current));
				}
				return list2.Count > 0;
			}
			return false;
		}

		protected bool CreateFileListFromWindowsFormattedMedia()
		{
			if (!this.isLogFileStorageOutdated)
			{
				return true;
			}
			base.ResetFileList();
			if (this.hardwareKey[0] == Path.DirectorySeparatorChar && this.hardwareKey[1] == Path.DirectorySeparatorChar)
			{
				base.IsLocatedAtNetwork = true;
				if (!DirectoryProxy.Exists(this.hardwareKey))
				{
					return false;
				}
			}
			else
			{
				try
				{
					DriveInfo driveInfo = new DriveInfo(this.hardwareKey);
					if (!driveInfo.IsReady)
					{
						bool result = false;
						return result;
					}
				}
				catch (Exception)
				{
					bool result = false;
					return result;
				}
			}
			this.GetVolumeCapacity();
			this.CreateFileListForWindowsDirectory(this.hardwareKey);
			this.isLogFileStorageOutdated = false;
			return true;
		}

		protected void GetVolumeCapacity()
		{
			if (base.IsLocatedAtNetwork)
			{
				return;
			}
			this.totalSpace = 0L;
			this.freeSpace = 0L;
			try
			{
				string pathRoot = Path.GetPathRoot(this.hardwareKey);
				DriveInfo driveInfo = new DriveInfo(pathRoot);
				this.totalSpace = driveInfo.TotalSize;
				this.freeSpace = driveInfo.AvailableFreeSpace;
			}
			catch
			{
			}
		}

		protected void CreateFileListForWindowsDirectory(string path)
		{
			this.logFiles.Clear();
			bool flag;
			IEnumerable<FileInfoProxy> files = this.GetFiles(path, out flag);
			this.highestTriggerFileIndices = new uint[this.LoggerSpecifics.DataStorage.NumberOfMemories];
			foreach (FileInfoProxy current in files)
			{
				if (!current.Extension.Equals(Vocabulary.FileExtensionDotBIN, StringComparison.OrdinalIgnoreCase) || this.LoggerSpecifics.Configuration.CompilerType != EnumCompilerType.LTL)
				{
					bool flag2 = false;
					LogFile logFile = null;
					try
					{
						logFile = new LogFile(current.DirectoryName, current.Name, (uint)current.Length, current.LastWriteTime);
					}
					catch
					{
						continue;
					}
					for (uint num = 0u; num <= this.LoggerSpecifics.DataStorage.NumberOfMemories; num += 1u)
					{
						string text;
						if (flag)
						{
							text = string.Format(Constants.LogDataFileInSubFolderPrefixOnMemory, num);
						}
						else
						{
							text = string.Format(Constants.LogDataFilePrefixOnMemory, num);
						}
						if (logFile.DefaultName.IndexOf(text) == 0)
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
							else if (string.Compare(current.Extension, Vocabulary.FileExtensionDotGZ, true) == 0)
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
							else if (string.Compare(current.Extension, Vocabulary.FileExtensionDotGLX, true) == 0)
							{
								if (!this.hasIndexFile)
								{
									this.hasIndexFile = true;
								}
								logFile.TypeName = Resources.FileManagerColFileTypeNaviFile;
								logFile.IsConvertible = false;
							}
							else if (string.Compare(current.Extension, Vocabulary.FileExtensionDotMF4U, true) == 0)
							{
								logFile.TypeName = Resources.FileManagerColFileTypeLogData;
								logFile.IsConvertible = true;
								Dictionary<uint, uint> numOfLogFilesOnMemNr2;
								uint key3;
								(numOfLogFilesOnMemNr2 = this.numOfLogFilesOnMemNr)[key3 = num] = numOfLogFilesOnMemNr2[key3] + 1u;
								this.triggeredBuffers += 1u;
							}
							else
							{
								logFile.TypeName = Resources.FileManagerColFileTypeUnknownLogData;
								logFile.IsConvertible = false;
							}
							flag2 = true;
							break;
						}
					}
					if (!flag2)
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
			}
			this.isPrimaryFileGroupCompressed = false;
			if (this.latestDateTimeCompressed > this.latestDateTimeUncompressed)
			{
				this.isPrimaryFileGroupCompressed = true;
			}
			if (this.LoggerSpecifics.DataStorage.NumberOfMemories > 1u)
			{
				bool flag3 = false;
				bool flag4 = false;
				for (uint num2 = 1u; num2 <= this.LoggerSpecifics.DataStorage.NumberOfMemories; num2 += 1u)
				{
					if (this.numOfLogFilesOnMemNr[num2] > 0u)
					{
						flag4 = true;
					}
					if (this.numOfCompLogFilesOnMemNr[num2] > 0u)
					{
						flag3 = true;
					}
				}
				this.hasMixedCompUncompFiles = (flag3 && flag4);
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
				}
				else
				{
					this.destSubFolderNameSecondary = GUIUtil.ConvertDateTimeToFolderName(base.LatestDateTimeCompressedLogfiles);
				}
			}
		}

		protected IEnumerable<FileInfoProxy> GetFiles(string path, out bool isUsingSubFolders)
		{
			List<string> list = null;
			if (FileSystemServices.HasSubFolders(path, Constants.LogDataFolderSearchPattern, out list))
			{
				List<FileInfoProxy> list2 = new List<FileInfoProxy>();
				foreach (string current in list)
				{
					list2.AddRange(FileSystemServices.GetFiles(current));
				}
				IEnumerable<FileInfoProxy> files = FileSystemServices.GetFiles(path);
				string[] files2 = DirectoryProxy.GetFiles(path, "Data?F*");
				foreach (FileInfoProxy current2 in files)
				{
					if (!files2.Contains(current2.Name))
					{
						list2.Add(current2);
					}
				}
				isUsingSubFolders = true;
				return list2;
			}
			isUsingSubFolders = false;
			return FileSystemServices.GetFiles(path);
		}

		protected void ConvertAllLogFilesFromWindowsFormattedMedia(FileConversionParameters conversionParams, DatabaseConfiguration databaseConfiguration, string configFolderPath)
		{
			string hardwareKey = this.hardwareKey;
			DateTime minValue = DateTime.MinValue;
			base.GetLatestFileDate(out minValue);
			string text = Path.Combine(conversionParams.DestinationFolder, this.destSubFolderNamePrimary);
			GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.ClfExportFolders, text);
			this.ConvertFromCardReader(hardwareKey, conversionParams, text, base.GetFileNamesOfPrimaryGroup(), base.IsPrimaryFileGroupCompressed, databaseConfiguration, configFolderPath, this.isIniFileMissing, this.LoggerSpecifics, minValue);
			if (this.hasMixedCompUncompFiles)
			{
				text = Path.Combine(conversionParams.DestinationFolder, this.destSubFolderNameSecondary);
				GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.ClfExportFolders, text);
				this.ConvertFromCardReader(hardwareKey, conversionParams, text, base.GetFileNamesOfSecondaryGroup(), !base.IsPrimaryFileGroupCompressed, databaseConfiguration, configFolderPath, true, this.LoggerSpecifics, minValue);
			}
		}

		private void ConvertFromCardReader(string sourceFolder, FileConversionParameters conversionParameters, string destinationFolderPath, IList<string> filesToProcess, bool isCompressed, DatabaseConfiguration databaseConfiguration, string configurationFolderPath, bool isIniFileMissing, ILoggerSpecifics loggerSpecifics, DateTime latestFileTimestamp)
		{
			string message = "";
			string value = "";
			string value2 = "";
			if (conversionParameters == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(sourceFolder) || string.IsNullOrEmpty(destinationFolderPath))
			{
				return;
			}
			if (!conversionParameters.CopyMediaFiles)
			{
				List<string> list = new List<string>();
				foreach (string current in filesToProcess)
				{
					string extension = Path.GetExtension(current);
					if (string.IsNullOrEmpty(extension) || (string.Compare(extension, Vocabulary.FileExtensionDotJPG, true) != 0 && string.Compare(extension, Vocabulary.FileExtensionDotZIP, true) != 0 && string.Compare(extension, Vocabulary.FileExtensionDotWAV, true) != 0))
					{
						list.Add(current);
					}
				}
				filesToProcess = list;
			}
			long sizeOfSourceDataInBytes = 0L;
			if (FileSystemServices.GetDirectorySize(sourceFolder, out sizeOfSourceDataInBytes) && !base.CheckAvailableSpaceOnDestination(conversionParameters.DestinationFormat, sizeOfSourceDataInBytes, destinationFolderPath, true))
			{
				return;
			}
			if (Directory.Exists(destinationFolderPath))
			{
				if (!conversionParameters.OverwriteDestinationFiles && InformMessageBox.Question(Resources.DestinationSubDirectoryExistsOverwrite) == DialogResult.No)
				{
					return;
				}
				string[] files = Directory.GetFiles(destinationFolderPath);
				for (int i = 0; i < files.Length; i++)
				{
					string filename = files[i];
					if (!GenerationUtil.TryDeleteFile(filename, out message))
					{
						InformMessageBox.Error(message);
						return;
					}
				}
				string[] directories = Directory.GetDirectories(destinationFolderPath);
				for (int j = 0; j < directories.Length; j++)
				{
					string text = directories[j];
					string[] files2 = Directory.GetFiles(text);
					for (int k = 0; k < files2.Length; k++)
					{
						string filename2 = files2[k];
						if (!GenerationUtil.TryDeleteFile(filename2, out message))
						{
							InformMessageBox.Error(message);
							return;
						}
					}
					if (!GenerationUtil.TryDeleteDirectory(text, out message))
					{
						InformMessageBox.Error(message);
						return;
					}
				}
			}
			else
			{
				Directory.CreateDirectory(destinationFolderPath);
			}
			int num = 3;
			if (isCompressed)
			{
				num = 4;
			}
			int num2 = 1;
			FileSystemServices.WriteProtocolLine(string.Format("Card Reader: Start copying files from card at {0}", DateTime.Now));
			int num3 = DirectoryProxy.EnumerateDirectories(sourceFolder, Constants.LogDataFolderSearchPattern, SearchOption.TopDirectoryOnly).Count<string>();
			string titleText = string.Format(Resources.StepNumOfTotal, num2, num) + Resources.TitleCopyFilesFromMemoryCard;
			num2++;
			CopyFilesWithProgressIndicator copyFilesWithProgressIndicator = new CopyFilesWithProgressIndicator(sourceFolder, filesToProcess, destinationFolderPath, true, true, titleText);
			if (copyFilesWithProgressIndicator.ShowDialog() == DialogResult.Cancel)
			{
				InformMessageBox.Info(Resources.ConversionAborted);
			}
			else
			{
				if (isCompressed)
				{
					if (!this.DecompressRawDataFiles(destinationFolderPath, num, num2, num3 > 0))
					{
						return;
					}
					num2++;
				}
				if (!this.GenerateDummyIniFilesIfMissing(destinationFolderPath, loggerSpecifics))
				{
					return;
				}
				string path = Path.Combine(destinationFolderPath, loggerSpecifics.DataStorage.LogDataIniFileName);
				if (File.Exists(path))
				{
					using (StreamReader streamReader = File.OpenText(path))
					{
						value = streamReader.ReadToEnd();
					}
				}
				string path2 = Path.Combine(destinationFolderPath, loggerSpecifics.DataStorage.LogDataIniFile2Name);
				if (File.Exists(path2))
				{
					using (StreamReader streamReader2 = File.OpenText(path2))
					{
						value2 = streamReader2.ReadToEnd();
					}
				}
				FileSystemServices.WriteProtocolLine(string.Format("Card Reader: Start decoding raw files at {0}", DateTime.Now));
				if (!this.DecodeRawDataFolder(num2, num, destinationFolderPath, false, true, conversionParameters))
				{
					return;
				}
				num2++;
				ReadOnlyCollection<string> readOnlyCollection = null;
				if (FileConversionHelper.IsSignalOrientedDestFormat(conversionParameters.DestinationFormat) && FileConversionHelper.UseArxmlToDBCConversion(conversionParameters))
				{
					IEnumerable<Database> conversionDatabases = AnalysisPackage.GetConversionDatabases(conversionParameters, databaseConfiguration.Databases, true);
					if (conversionDatabases != null && conversionDatabases.Any<Database>() && !GenerateDBCfromARXML.Execute(conversionDatabases, configurationFolderPath, destinationFolderPath, out readOnlyCollection))
					{
						InformMessageBox.Info(Resources.ConversionAborted);
						return;
					}
				}
				if (!base.ConvertAndRenameFiles(conversionParameters, destinationFolderPath, databaseConfiguration, configurationFolderPath, num2, num))
				{
					return;
				}
				if (readOnlyCollection != null)
				{
					foreach (string current2 in readOnlyCollection)
					{
						if (File.Exists(current2) && !GenerationUtil.TryDeleteFile(current2, out message, true))
						{
							InformMessageBox.Error(message);
						}
					}
				}
				try
				{
					using (StreamWriter streamWriter = File.CreateText(path))
					{
						streamWriter.Write(value);
						streamWriter.Flush();
						streamWriter.Close();
					}
					using (StreamWriter streamWriter2 = File.CreateText(path2))
					{
						streamWriter2.Write(value2);
						streamWriter2.Flush();
						streamWriter2.Close();
					}
				}
				catch (Exception)
				{
				}
				if (conversionParameters.GenerateVsysvarFile)
				{
					this.GenerateVSysVarFile(conversionParameters.DestinationFormat, loggerSpecifics.Type, sourceFolder, loggerSpecifics.DataStorage.LogDataIniFile2Name, destinationFolderPath);
				}
				if (conversionParameters.DeleteSourceFilesWhenDone && conversionParameters.IsDeletionOfSourceFilesAllowed && LoggerDeviceWindowsFileSystem.ClearMemoryCard(sourceFolder, this.LoggerSpecifics, false, out message, false, true, true) == Result.Error)
				{
					InformMessageBox.Error(message);
				}
				return;
			}
		}

		public void ConvertSelectedLogFilesFromWindowsFormattedMedia(ConversionJob job, string destSubFolderPath, string tempCacheFolderPath, string tempConvertFolderPath, DatabaseConfiguration databaseConfiguration, string configFolderPath, string masterStatusTextPrefix, ref int currentMasterStep, IDoubleProgressIndicator progressIndicator, ProcessExitedDelegate processExitedDelegate)
		{
			bool flag = currentMasterStep == 0;
			List<string> list = new List<string>();
			List<string> list2 = null;
			List<string> list3 = null;
			HashSet<string> hashSet = new HashSet<string>(job.SelectedFileNames);
			if (FileSystemServices.HasSubFolders(tempCacheFolderPath, Constants.LogDataFolderSearchPattern, out list3))
			{
				foreach (string current in list3)
				{
					IEnumerable<FileInfoProxy> files = FileSystemServices.GetFiles(current);
					foreach (FileInfoProxy current2 in files)
					{
						if (base.IsPrimaryFileGroupCompressed)
						{
							string item = current2.Name + Vocabulary.FileExtensionDotGZ;
							if (hashSet.Contains(item))
							{
								hashSet.Remove(item);
							}
						}
						else if (hashSet.Contains(current2.Name))
						{
							hashSet.Remove(current2.Name);
						}
					}
				}
			}
			if (!FileSystemServices.HasSubFolders(this.hardwareKey, Constants.LogDataFolderSearchPattern, out list2))
			{
				InformMessageBox.Info(Resources.ConversionInfoNoDataAvailable);
				return;
			}
			long num = 0L;
			string text = this.hardwareKey;
			if (text.Last<char>() != Path.DirectorySeparatorChar)
			{
				text += Path.DirectorySeparatorChar;
			}
			foreach (string current3 in list2)
			{
				string fileName = Path.GetFileName(current3.TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar
				}));
				IEnumerable<FileInfoProxy> files2 = FileSystemServices.GetFiles(current3);
				foreach (FileInfoProxy current4 in files2)
				{
					if (hashSet.Contains(current4.Name))
					{
						list.Add(Path.Combine(fileName, current4.Name));
						num += current4.Length;
						hashSet.Remove(current4.Name);
						if (hashSet.Count == 0)
						{
							break;
						}
					}
				}
				if (hashSet.Count == 0)
				{
					break;
				}
			}
			List<string> list4 = new List<string>();
			foreach (string current5 in this.GetClassificationFilePaths(this.hardwareKey))
			{
				list4.Add(Path.GetFileName(current5));
			}
			list.AddRange(list4);
			if (!File.Exists(Path.Combine(tempCacheFolderPath, this.LoggerSpecifics.DataStorage.LogDataIniFileName)))
			{
				list.Add(this.LoggerSpecifics.DataStorage.LogDataIniFileName);
			}
			if (!File.Exists(Path.Combine(tempCacheFolderPath, this.LoggerSpecifics.DataStorage.LogDataIniFile2Name)))
			{
				list.Add(this.LoggerSpecifics.DataStorage.LogDataIniFile2Name);
			}
			if (hashSet.Count > 0 && !this.ConfirmToContinueWithoutMissingFiles(hashSet))
			{
				return;
			}
			if (!base.CheckAvailableSpaceOnDestination(job.FileConversionParameters.DestinationFormat, num, destSubFolderPath, false))
			{
				return;
			}
			int num2 = 3;
			if (base.IsPrimaryFileGroupCompressed)
			{
				num2 = 4;
			}
			int num3 = 1;
			progressIndicator.SetMasterValue(++currentMasterStep);
			string str = string.Format(Resources.StepNumOfTotal, num3, num2) + Resources.TitleCopyFilesFromMemoryCard;
			progressIndicator.SetMasterStatusText(masterStatusTextPrefix + " " + str);
			ProcessCopyFiles processCopyFiles = new ProcessCopyFiles(text, list, tempCacheFolderPath, progressIndicator, processExitedDelegate);
			processCopyFiles.Execute();
			progressIndicator.SetMasterValue(++currentMasterStep);
			num3++;
			if (!string.IsNullOrEmpty(processCopyFiles.ErrorText))
			{
				InformMessageBox.Error(processCopyFiles.ErrorText);
				return;
			}
			if (progressIndicator.Cancelled())
			{
				InformMessageBox.Info(Resources.ConversionAborted);
				return;
			}
			FileSystemServices.TrySetNormalFileAttributes(Path.Combine(tempCacheFolderPath, this.LoggerSpecifics.DataStorage.LogDataIniFileName));
			FileSystemServices.TrySetNormalFileAttributes(Path.Combine(tempCacheFolderPath, this.LoggerSpecifics.DataStorage.LogDataIniFile2Name));
			if (base.IsPrimaryFileGroupCompressed)
			{
				str = string.Format(Resources.StepNumOfTotal, num3, num2) + Resources.TitleDecompressFiles;
				progressIndicator.SetMasterStatusText(masterStatusTextPrefix + " " + str);
				if (!this.DecompressRawDataFiles(tempCacheFolderPath, num2, num3, true, progressIndicator, processExitedDelegate))
				{
					return;
				}
				progressIndicator.SetMasterValue(++currentMasterStep);
				num3++;
			}
			HashSet<string> hashSet2;
			if (base.IsPrimaryFileGroupCompressed)
			{
				hashSet2 = new HashSet<string>();
				using (HashSet<string>.Enumerator enumerator6 = job.SelectedFileNames.GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						string current6 = enumerator6.Current;
						hashSet2.Add(Path.GetFileNameWithoutExtension(current6));
					}
					goto IL_50F;
				}
			}
			hashSet2 = new HashSet<string>(job.SelectedFileNames);
			IL_50F:
			if (!this.MoveSelectedLogAndIniFiles(hashSet2, tempCacheFolderPath, tempConvertFolderPath))
			{
				return;
			}
			if (flag)
			{
				this.MoveRCLFilesToConvertFolder(list4, tempCacheFolderPath, tempConvertFolderPath);
			}
			if (!this.GenerateDummyIniFilesIfMissing(tempConvertFolderPath, this.LoggerSpecifics))
			{
				return;
			}
			str = string.Format(Resources.StepNumOfTotal, num3, num2) + Resources.TitleDecodeFiles;
			progressIndicator.SetMasterStatusText(masterStatusTextPrefix + " " + str);
			if (!this.DecodeRawDataFolder(num3, num2, tempConvertFolderPath, true, true, job.FileConversionParameters, progressIndicator, processExitedDelegate))
			{
				return;
			}
			progressIndicator.SetMasterValue(++currentMasterStep);
			num3++;
			str = string.Format(Resources.StepNumOfTotal, num3, num2) + Resources.TitleConvertFiles;
			progressIndicator.SetMasterStatusText(masterStatusTextPrefix + " " + str);
			if (!base.ConvertAndRenameFiles(job.FileConversionParameters, tempConvertFolderPath, databaseConfiguration, configFolderPath, job, progressIndicator, processExitedDelegate, num3, num2))
			{
				return;
			}
			progressIndicator.SetMasterValue(++currentMasterStep);
			IEnumerable<FileInfoProxy> files3 = FileSystemServices.GetFiles(tempConvertFolderPath);
			bool flag2 = File.Exists(Path.Combine(destSubFolderPath, this.LoggerSpecifics.DataStorage.LogDataIniFileName));
			bool flag3 = File.Exists(Path.Combine(destSubFolderPath, this.LoggerSpecifics.DataStorage.LogDataIniFile2Name));
			foreach (FileInfoProxy current7 in files3)
			{
				if ((string.Compare(current7.Name, this.LoggerSpecifics.DataStorage.LogDataIniFileName, true) != 0 || !flag2) && (string.Compare(current7.Name, this.LoggerSpecifics.DataStorage.LogDataIniFile2Name, true) != 0 || !flag3) && string.Compare(current7.Extension, Vocabulary.FileExtensionDotDBC, true) != 0 && string.Compare(current7.Extension, Vocabulary.FileExtensionDotRCL, true) != 0 && string.Compare(Path.GetExtension(current7.Name), ".bat", true) != 0 && !this.TryMoveFileToFolder(current7.FullName, destSubFolderPath, job.FileConversionParameters.OverwriteDestinationFiles))
				{
					return;
				}
			}
			if (flag)
			{
				this.TryMoveClassificationFolder(tempConvertFolderPath, destSubFolderPath, job.FileConversionParameters.OverwriteDestinationFiles);
			}
			if (this.MoveSelectedLogAndIniFiles(hashSet2, tempConvertFolderPath, tempCacheFolderPath))
			{
				GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.ClfSourceFolders, destSubFolderPath);
				return;
			}
		}

		private bool MoveSelectedLogAndIniFiles(HashSet<string> selectedFileNames, string sourceBaseFolder, string destBaseFolder)
		{
			string text = Path.Combine(sourceBaseFolder, this.LoggerSpecifics.DataStorage.LogDataIniFileName);
			if (File.Exists(text) && !File.Exists(Path.Combine(destBaseFolder, this.LoggerSpecifics.DataStorage.LogDataIniFileName)) && !FileSystemServices.TryMoveFile(text, destBaseFolder))
			{
				InformMessageBox.Error(string.Format(Resources.ErrorUnableToCreateFile, Path.Combine(destBaseFolder, this.LoggerSpecifics.DataStorage.LogDataIniFileName)));
				return false;
			}
			string text2 = Path.Combine(sourceBaseFolder, this.LoggerSpecifics.DataStorage.LogDataIniFile2Name);
			if (File.Exists(text2) && !File.Exists(Path.Combine(destBaseFolder, this.LoggerSpecifics.DataStorage.LogDataIniFile2Name)) && !FileSystemServices.TryMoveFile(text2, destBaseFolder))
			{
				InformMessageBox.Error(string.Format(Resources.ErrorUnableToCreateFile, Path.Combine(destBaseFolder, this.LoggerSpecifics.DataStorage.LogDataIniFile2Name)));
				return false;
			}
			HashSet<string> hashSet = new HashSet<string>(selectedFileNames);
			List<string> list;
			if (!FileSystemServices.HasSubFolders(sourceBaseFolder, Constants.LogDataFolderSearchPattern, out list))
			{
				return true;
			}
			foreach (string current in list)
			{
				IEnumerable<FileInfoProxy> files = FileSystemServices.GetFiles(current);
				foreach (FileInfoProxy current2 in files)
				{
					if (hashSet.Contains(current2.Name))
					{
						string text3 = Path.Combine(destBaseFolder, current.Split(new char[]
						{
							Path.DirectorySeparatorChar
						}).Last<string>());
						if (!Directory.Exists(text3) && !FileSystemServices.TryCreateDirectory(text3))
						{
							InformMessageBox.Error(string.Format(Resources.ErrorUnableToCreateFolder, text3));
							bool result = false;
							return result;
						}
						string path = Path.Combine(text3, current2.Name);
						if (!File.Exists(path))
						{
							if (!FileSystemServices.TryMoveFile(current2.FullName, text3))
							{
								InformMessageBox.Error(string.Format(Resources.ErrorUnableToCreateFile, Path.Combine(text3, current2.Name)));
								bool result = false;
								return result;
							}
							try
							{
								File.SetAttributes(path, FileAttributes.Normal);
							}
							catch (Exception)
							{
							}
						}
						hashSet.Remove(current2.Name);
						if (hashSet.Count == 0)
						{
							break;
						}
					}
				}
				if (hashSet.Count == 0)
				{
					break;
				}
			}
			return true;
		}

		private void MoveRCLFilesToConvertFolder(IList<string> rclFiles, string srcFolder, string destFolder)
		{
			foreach (string current in rclFiles)
			{
				string sourceFileName = Path.Combine(srcFolder, current);
				string text = Path.Combine(destFolder, current);
				try
				{
					if (!File.Exists(text))
					{
						File.Move(sourceFileName, text);
					}
				}
				catch
				{
					if (!File.Exists(text))
					{
						InformMessageBox.Error(string.Format(Resources.ErrorCannotMoveFileTo, current, destFolder));
					}
				}
			}
		}

		private void TryMoveClassificationFolder(string sourceFolder, string destFolder, bool overwrite)
		{
			string text = Path.Combine(sourceFolder, LoggerDeviceWindowsFileSystem._ClassifyFolderName);
			if (Directory.Exists(text))
			{
				string text2 = Path.Combine(destFolder, LoggerDeviceWindowsFileSystem._ClassifyFolderName);
				if (Directory.Exists(text2))
				{
					if (overwrite)
					{
						try
						{
							Directory.Delete(text2);
							goto IL_7F;
						}
						catch
						{
							goto IL_7F;
						}
					}
					int num = 2;
					do
					{
						text2 = Path.Combine(destFolder, LoggerDeviceWindowsFileSystem._ClassifyFolderName + string.Format("_({0:D})", num++));
						if (!Directory.Exists(text2))
						{
							break;
						}
					}
					while (num < 1000);
				}
				try
				{
					IL_7F:
					Directory.Move(text, text2);
				}
				catch
				{
					InformMessageBox.Error(string.Format(Resources.ErrorCannotMoveFileTo, text, text2));
				}
			}
		}

		private bool GenerateDummyIniFilesIfMissing(string destinationFolderPath, ILoggerSpecifics loggerSpecifics)
		{
			if (this.isIniFileMissing)
			{
				DateTime minValue = DateTime.MinValue;
				base.GetLatestFileDate(out minValue);
				if (!LoggerDeviceWindowsFileSystem.GenerateDummyIniFile(destinationFolderPath, loggerSpecifics, minValue))
				{
					InformMessageBox.Error(Resources.ErrorUnableToWriteIniFile);
					return false;
				}
			}
			LoggerDeviceWindowsFileSystem.GenerateDummyIniFile2(destinationFolderPath, loggerSpecifics);
			return true;
		}

		public static bool GenerateDummyIniFile(string destinationFolderPath, ILoggerSpecifics loggerSpecifics, DateTime timestamp)
		{
			if (!Directory.Exists(destinationFolderPath))
			{
				return false;
			}
			string path = Path.Combine(destinationFolderPath, loggerSpecifics.DataStorage.LogDataIniFileName);
			if (File.Exists(path))
			{
				try
				{
					File.Delete(path);
				}
				catch (Exception)
				{
					bool result = false;
					return result;
				}
			}
			string text = string.Format(Resources.DefaultFileNameFormat, loggerSpecifics.Name, "clf");
			string text2 = string.Concat(new object[]
			{
				"C",
				Path.VolumeSeparatorChar,
				Path.DirectorySeparatorChar,
				"Tmp"
			});
			string text3 = string.Format("{0:D2}.{1:D2}.{2:D4}", timestamp.Day, timestamp.Month, timestamp.Year);
			string text4 = string.Format("{0:D2}:{1:D2}:{2:D2}", timestamp.Hour, timestamp.Minute, timestamp.Second);
			string value = string.Format("FileName={0}\nFilePath={1}\nDataDate={2}\nDataTime={3}\nSerNum={4}\nCarName={5}\nDevType=0x{6:X}\nStandardPretrigger=0\nDiagMode=0\n", new object[]
			{
				text,
				text2,
				text3,
				text4,
				loggerSpecifics.DeviceAccess.DefaultDummySerialNumber,
				loggerSpecifics.DeviceAccess.DefaultDummySerialNumber,
				loggerSpecifics.DeviceAccess.DeviceType
			});
			try
			{
				StreamWriter streamWriter = File.CreateText(path);
				streamWriter.Write(value);
				streamWriter.Close();
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			return true;
		}

		public static bool GenerateDummyIniFile2(string destinationFolderPath, ILoggerSpecifics loggerSpecifics)
		{
			string path = Path.Combine(destinationFolderPath, loggerSpecifics.DataStorage.LogDataIniFile2Name);
			if (File.Exists(path))
			{
				return true;
			}
			string value = "LogErrorFrames=1";
			try
			{
				StreamWriter streamWriter = File.CreateText(path);
				streamWriter.Write(value);
				streamWriter.Close();
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		protected bool ClearWindowsFormattedMedia()
		{
			string message;
			Result result = LoggerDeviceWindowsFileSystem.ClearMemoryCard(this.hardwareKey, this.LoggerSpecifics, false, out message, false, true, false);
			if (result != Result.OK)
			{
				if (result == Result.Error)
				{
					InformMessageBox.Error(message);
				}
				return false;
			}
			return true;
		}

		protected bool DeleteAllFilesFromWindowsFormattedMedia()
		{
			string message;
			Result result = LoggerDeviceWindowsFileSystem.ClearMemoryCard(this.hardwareKey, this.LoggerSpecifics, false, out message, false, true, true);
			if (result != Result.OK)
			{
				if (result == Result.Error)
				{
					InformMessageBox.Error(message);
				}
				return false;
			}
			return true;
		}

		protected bool WriteConfigurationToWindowsFormattedMedia(out string codMD5Hash, bool showProgressBar)
		{
			codMD5Hash = string.Empty;
			if (this.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem && !FileSystemServices.HasFAT32FileSystem(this.hardwareKey))
			{
				InformMessageBox.Error(Resources.ErrorDriveIsNotFAT32Formatted);
				return false;
			}
			this.CheckForExistingLicenseFilesAndConfirmToDelete();
			string errorText;
			string compilerErrFilePath;
			Result result;
			if (showProgressBar)
			{
				result = new LoadingDialog(Resources.WritingData)
				{
					WriteToMemoryCardTask = new LoadingDialog.WriteToMemoryCardDelegate(this.WriteToMemoryCard)
				}.ExecuteWriteToMemoryCard(this.hardwareKey, out codMD5Hash, out errorText, out compilerErrFilePath);
			}
			else
			{
				result = this.WriteToMemoryCard(this.hardwareKey, out codMD5Hash, out errorText, out compilerErrFilePath);
			}
			if (result != Result.OK)
			{
				if (result == Result.Error)
				{
					GUIUtil.ReportFileGenerationResult(EnumInfoType.Error, errorText, compilerErrFilePath);
				}
				return false;
			}
			return true;
		}

		protected Result WriteConfigurationToWindowsFormattedMedia(string codFilePath, bool showProgressBar)
		{
			if (this.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem && !FileSystemServices.HasFAT32FileSystem(this.hardwareKey))
			{
				InformMessageBox.Error(Resources.ErrorDriveIsNotFAT32Formatted);
				return Result.Error;
			}
			string message;
			Result result;
			if (showProgressBar)
			{
				result = new LoadingDialog(Resources.WritingData)
				{
					CopyCODFileToMemoryCardTask = new LoadingDialog.CopyCODFileToMemoryCardDelegate(this.CopyCODFileToMemoryCard)
				}.ExecuteCopyCODFileToMemoryCard(codFilePath, this.hardwareKey, out message);
			}
			else
			{
				result = this.CopyCODFileToMemoryCard(codFilePath, this.hardwareKey, out message);
			}
			if (result != Result.OK && result == Result.Error)
			{
				InformMessageBox.Error(message);
			}
			return result;
		}

		private Result WriteToMemoryCard(string cardDrive, out string codMD5Hash, out string errorText, out string compilerErrFilePath)
		{
			codMD5Hash = string.Empty;
			compilerErrFilePath = string.Empty;
			errorText = string.Empty;
			string pathRoot = Path.GetPathRoot(cardDrive);
			Path.Combine(pathRoot, LoggerDeviceWindowsFileSystem.configureDirectoryName);
			string tempDirectoryName;
			if (!TempDirectoryManager.Instance.CreateNewTempDirectory(out tempDirectoryName))
			{
				errorText = Resources.ErrorCannotCreateTemporaryFile;
				return Result.Error;
			}
			Result result = Result.Error;
			if (this.LoggerSpecifics.Configuration.CompilerType == EnumCompilerType.LTL)
			{
				string fileName = Path.GetFileName(GenericSaveFileDialog.GetSuggestedFilename(FileType.CODFile, GenerationUtil.ProjectFileName, this.LoggerSpecifics.Name));
				string text = Path.Combine(TempDirectoryManager.Instance.GetFullTempDirectoryPath(tempDirectoryName), fileName);
				bool flag;
				if ((result = GenerationUtil.ExportToLtlOrCodFile(text, out errorText, out compilerErrFilePath, out flag)) == Result.OK && (result = LoggerDeviceWindowsFileSystem.ClearMemoryCard(cardDrive, this.LoggerSpecifics, true, out errorText, true, true, false)) == Result.OK)
				{
					this.WriteConfigurationToWindowsFormattedMedia(text, false);
					codMD5Hash = GenerationUtil.ComputeMD5Hash(text);
				}
			}
			else if (this.LoggerSpecifics.Configuration.CompilerType == EnumCompilerType.CAPL)
			{
				string fileName2 = Path.GetFileName(GenericSaveFileDialog.GetSuggestedFilename(FileType.CompiledCAPLFile, string.Empty, this.LoggerSpecifics.Name));
				string text2 = Path.Combine(TempDirectoryManager.Instance.GetFullTempDirectoryPath(tempDirectoryName), fileName2);
				string baseFilePath = Path.ChangeExtension(text2, string.Empty).TrimEnd(new char[]
				{
					'.'
				});
				if ((result = GenerationUtilVN.ExportToVNBinary(baseFilePath, out errorText)) == Result.OK && (result = LoggerDeviceWindowsFileSystem.ClearMemoryCard(cardDrive, this.LoggerSpecifics, true, out errorText, false, true, false)) == Result.OK)
				{
					this.WriteConfigurationToWindowsFormattedMedia(text2, false);
					codMD5Hash = GenerationUtilVN.ComputeMD5Hash(text2);
				}
			}
			if (GenerationUtil.AppDataAccess.AppDataRoot.GlobalOptions.KeepTempFoldersFromCodeGeneration)
			{
				return result;
			}
			TempDirectoryManager.Instance.ReleaseTempDirectory(tempDirectoryName);
			return result;
		}

		private Result CopyCODFileToMemoryCard(string codFilePath, string cardDrive, out string errorText)
		{
			string pathRoot = Path.GetPathRoot(cardDrive);
			string text = Path.Combine(pathRoot, this.LoggerSpecifics.Configuration.ConfigurationDirectoryName);
			Result result;
			if ((result = LoggerDeviceWindowsFileSystem.ClearMemoryCard(cardDrive, this.LoggerSpecifics, true, out errorText, true, true, false)) != Result.OK)
			{
				return result;
			}
			string destFileName = Path.Combine(text, Path.GetFileName(codFilePath));
			try
			{
				File.Copy(codFilePath, destFileName);
			}
			catch (Exception)
			{
				errorText = string.Format(Resources.CannotCopyFileTo, codFilePath, text);
				return Result.Error;
			}
			return Result.OK;
		}

		public static Result ClearMemoryCard(string cardDrive, ILoggerSpecifics loggerSpecifics, bool isPrecondition, out string errorText, bool createConfigureDirIfNotExists, bool maintainLicenseFileIfExists, bool deleteOnlyLogfiles)
		{
			string text = cardDrive.TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar
			}) + Path.DirectorySeparatorChar;
			string path = Path.Combine(text, loggerSpecifics.Configuration.ConfigurationDirectoryName);
			errorText = "";
			bool flag = false;
			bool flag2 = false;
			string path2 = Path.Combine(text, Vocabulary.FolderNameProjectPackage);
			string[] files;
			string[] array;
			try
			{
				files = Directory.GetFiles(text);
				if (Directory.GetDirectories(text, "!D*").Length > 0)
				{
					flag = true;
				}
				if (Directory.Exists(path))
				{
					array = Directory.GetFiles(path);
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string fileName = array2[i];
						FileInfo fileInfo = new FileInfo(fileName);
						if (string.Compare(fileInfo.Extension, Vocabulary.FileExtensionDotCOD, true) == 0)
						{
							flag2 = true;
							break;
						}
					}
				}
				else
				{
					array = new string[0];
				}
			}
			catch
			{
				errorText = string.Format(Resources.ErrorCannotAccessMemoryCardinDrive, text);
				Result result = Result.Error;
				return result;
			}
			bool flag3 = flag || files.Length > 0;
			if (flag2 && !deleteOnlyLogfiles)
			{
				if (flag3)
				{
					string message = Resources.DeleteExistingLogfilesAndConfigurationFromMemoryCard;
					if (isPrecondition)
					{
						message = Resources.ExistingLogFilesAndConfigMustBeDeletedFromMemCard + Environment.NewLine + Resources.QuestionContinue;
					}
					if (InformMessageBox.Question(message) != DialogResult.Yes)
					{
						return Result.UserAbort;
					}
				}
				else
				{
					string message = Resources.DeleteExistingConfigurationFromMemoryCard;
					if (isPrecondition)
					{
						message = Resources.ExistingConfigMustBeDeletedFromMemCard + Environment.NewLine + Resources.QuestionContinue;
					}
					if (InformMessageBox.Question(message) != DialogResult.Yes)
					{
						return Result.UserAbort;
					}
				}
			}
			else if (flag3)
			{
				string message = Resources.DeleteExistingLogfilesFromMemoryCard;
				if (isPrecondition)
				{
					message = Resources.ExistingLogFilesMustBeDeletedFromMemCard + Environment.NewLine + Resources.QuestionContinue;
				}
				if (InformMessageBox.Question(message) != DialogResult.Yes)
				{
					return Result.UserAbort;
				}
			}
			if (flag)
			{
				DeleteFoldersWithProgressIndicator deleteFoldersWithProgressIndicator = new DeleteFoldersWithProgressIndicator(text, "!D*", string.Format(Resources.TitleDeleteLogFilesFromCard, text));
				if (deleteFoldersWithProgressIndicator.ShowDialog() == DialogResult.Cancel)
				{
					return Result.UserAbort;
				}
			}
			else
			{
				DeleteFilesWithProgressIndicator deleteFilesWithProgressIndicator = new DeleteFilesWithProgressIndicator(files, string.Format(Resources.TitleDeleteLogFilesFromCard, text));
				if (deleteFilesWithProgressIndicator.ShowDialog() == DialogResult.Cancel)
				{
					return Result.UserAbort;
				}
			}
			if (!deleteOnlyLogfiles)
			{
				if (Directory.Exists(path))
				{
					string[] array3 = array;
					for (int j = 0; j < array3.Length; j++)
					{
						string text2 = array3[j];
						if ((!maintainLicenseFileIfExists || string.Compare(Path.GetExtension(text2), Vocabulary.FileExtensionDotLIC, true) != 0) && !GenerationUtil.TryDeleteFile(text2, out errorText))
						{
							Result result = Result.Error;
							return result;
						}
					}
				}
				else if (createConfigureDirIfNotExists)
				{
					try
					{
						Directory.CreateDirectory(path);
					}
					catch
					{
						errorText = string.Format(Resources.ErrorCannotAccessMemoryCardinDrive, text);
						Result result = Result.Error;
						return result;
					}
				}
				if (Directory.Exists(path2))
				{
					string[] files2 = Directory.GetFiles(path2);
					string[] array4 = files2;
					for (int k = 0; k < array4.Length; k++)
					{
						string filename = array4[k];
						GenerationUtil.TryDeleteFile(filename, out errorText);
					}
				}
				string text3 = text + LoggerDeviceCommon._SnapshotFolderName;
				if (Directory.Exists(text3) && !FileSystemServices.TryDeleteDirectory(text3))
				{
					errorText = string.Format(Resources.CannotDeleteDirectory, text3);
					return Result.Error;
				}
				string text4 = text + Vocabulary.FolderNameAnalysisPackage;
				if (Directory.Exists(text4) && !FileSystemServices.TryDeleteDirectory(text4))
				{
					errorText = string.Format(Resources.CannotDeleteDirectory, text4);
					return Result.Error;
				}
			}
			errorText = "";
			return Result.OK;
		}

		protected bool WriteAnalysisPackageToWindowsFormattedMedia(string analysisPackagepath)
		{
			if (this.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem && !FileSystemServices.HasFAT32FileSystem(this.hardwareKey))
			{
				InformMessageBox.Error(Resources.ErrorDriveIsNotFAT32Formatted);
				return false;
			}
			string text = Path.Combine(this.hardwareKey, Vocabulary.FolderNameAnalysisPackage);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string text2 = Path.Combine(text, Path.GetFileName(analysisPackagepath));
			if (!GenerationUtil.TryCopyFile(analysisPackagepath, text2, true))
			{
				InformMessageBox.Error(string.Format(Resources.CannotCopyFileTo, analysisPackagepath, text2));
				return false;
			}
			return true;
		}

		protected bool WriteProjectZIPFileToWindowsFormattedMedia(string zipFilePath)
		{
			if (this.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem && !FileSystemServices.HasFAT32FileSystem(this.hardwareKey))
			{
				InformMessageBox.Error(Resources.ErrorDriveIsNotFAT32Formatted);
				return false;
			}
			string text = Path.Combine(this.hardwareKey, Vocabulary.FolderNameProjectPackage);
			if (!Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			string text2 = Path.Combine(text, Path.GetFileName(zipFilePath));
			if (!GenerationUtil.TryCopyFile(zipFilePath, text2, true))
			{
				InformMessageBox.Error(string.Format(Resources.CannotCopyFileTo, zipFilePath, text2));
				return false;
			}
			return true;
		}

		protected string[] GetProjectZIPFilePathFromWindowsFormattedMedia()
		{
			string path = Path.Combine(this.hardwareKey, Vocabulary.FolderNameProjectPackage);
			if (DirectoryProxy.Exists(path))
			{
				return DirectoryProxy.GetFiles(path, "*.zip");
			}
			return null;
		}

		protected string[] GetCODFilePathFromWindowsFormattedMedia()
		{
			if (this.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem && !FileSystemServices.HasFAT32FileSystem(this.hardwareKey))
			{
				InformMessageBox.Error(Resources.ErrorDriveIsNotFAT32Formatted);
				return null;
			}
			string path = Path.Combine(this.hardwareKey, this.LoggerSpecifics.Configuration.ConfigurationDirectoryName);
			if (Directory.Exists(path))
			{
				string searchPattern = "*" + Vocabulary.FileExtensionDotCOD;
				return Directory.GetFiles(path, searchPattern);
			}
			return null;
		}

		protected void InstallLicense(string licenseFilePath, ILoggerSpecifics loggerSpecifics, IHardwareFrontend hardwareFrontend, List<string> additionalDrivesList)
		{
			Path.GetFileName(licenseFilePath);
			string cardReaderDrive = "";
			if (CardReaderDriveSelection.SelectCardReaderDrive(loggerSpecifics, hardwareFrontend, additionalDrivesList, out cardReaderDrive, false, LoggerType.Unknown) && this.InstallLicenseOnMemoryCard(cardReaderDrive, licenseFilePath))
			{
				InformMessageBox.Info(Resources.LicenseSuccessfullyWritten);
			}
		}

		protected bool InstallLicenseOnMemoryCard(string cardReaderDrive, string licenseFilePath)
		{
			string text = Path.GetFileName(licenseFilePath);
			string text2 = cardReaderDrive + this.LoggerSpecifics.Configuration.ConfigurationDirectoryName;
			if (Directory.Exists(text2))
			{
				string[] files = Directory.GetFiles(text2);
				if (files.Count<string>() > 0)
				{
					string strB = Path.Combine(text2, text);
					string[] array = files;
					int i = 0;
					while (i < array.Length)
					{
						string strA = array[i];
						if (string.Compare(strA, strB, true) == 0)
						{
							if (DialogResult.No == InformMessageBox.Question(Resources.LicenseSameFileAlreadyExists))
							{
								bool result = false;
								return result;
							}
							break;
						}
						else
						{
							i++;
						}
					}
					string text3 = Path.Combine(text2, LoggerDeviceWindowsFileSystem.ignoreFileName);
					if (File.Exists(text3) && !FileSystemServices.TryDeleteFile(text3))
					{
						InformMessageBox.Error(string.Format(Resources.CannotDeleteFile, text3));
					}
				}
			}
			else
			{
				try
				{
					Directory.CreateDirectory(text2);
				}
				catch (Exception)
				{
					InformMessageBox.Error(Resources.ErrorUnableToCreateConigureFolder);
					bool result = false;
					return result;
				}
			}
			text = Path.GetFileNameWithoutExtension(text) + (Path.GetExtension(text) ?? string.Empty).ToUpper();
			try
			{
				FileInfo fileInfo = new FileInfo(licenseFilePath);
				if (fileInfo.Exists)
				{
					fileInfo.CopyTo(Path.Combine(text2, text), true);
				}
			}
			catch (Exception)
			{
				InformMessageBox.Error(Resources.ErrorUnableToCopyLicenseFile);
				bool result = false;
				return result;
			}
			return true;
		}

		protected void CheckForExistingLicenseFilesAndConfirmToDelete()
		{
			string text = Path.Combine(this.hardwareKey, LoggerDeviceWindowsFileSystem.configureDirectoryName);
			IList<string> list;
			if (Directory.Exists(text) && FileSystemServices.HasLicenseFilesInFolder(text, out list))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(Resources.LicenseFilesWereFoundOnMedium);
				stringBuilder.AppendLine();
				foreach (string current in list)
				{
					stringBuilder.AppendLine(Path.GetFileName(current));
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(Resources.LicenseFilesDoYouWantToDelete);
				if (InformMessageBox.Question(stringBuilder.ToString()) == DialogResult.Yes)
				{
					foreach (string current2 in list)
					{
						if (!FileSystemServices.TryDeleteFile(current2))
						{
							InformMessageBox.Error(string.Format(Resources.CannotDeleteFile, current2));
						}
					}
				}
			}
		}

		private bool DecodeRawDataFolder(int currentStep, int totalSteps, string rawDataFolderPath, bool isKeepingRawData, bool isKeepingOriginalIndex, FileConversionParameters conversionParameters)
		{
			return this.DecodeRawDataFolder(currentStep, totalSteps, rawDataFolderPath, isKeepingRawData, isKeepingOriginalIndex, conversionParameters, null, null);
		}

		private bool DecodeRawDataFolder(int currentStep, int totalSteps, string rawDataFolderPath, bool isKeepingRawData, bool isKeepingOriginalIndex, FileConversionParameters conversionParameters, IProgressIndicator pi, ProcessExitedDelegate processExitedDelegate)
		{
			Lrf_dec lrf_dec = new Lrf_dec();
			ProgressIndicatorForm progressIndicatorForm = null;
			if (pi == null)
			{
				progressIndicatorForm = new ProgressIndicatorForm();
				pi = progressIndicatorForm;
				processExitedDelegate = new ProcessExitedDelegate(progressIndicatorForm.ProcessExited);
			}
			if (progressIndicatorForm != null)
			{
				progressIndicatorForm.Text = string.Format(Resources.StepNumOfTotal, currentStep, totalSteps) + Resources.TitleDecodeFiles;
			}
			string message;
			if (!lrf_dec.ProcessRawDataFolder(rawDataFolderPath, isKeepingRawData, isKeepingOriginalIndex, conversionParameters, out message, pi, processExitedDelegate))
			{
				InformMessageBox.Error(message);
				return false;
			}
			if (progressIndicatorForm != null)
			{
				progressIndicatorForm.ShowDialog();
			}
			else
			{
				lrf_dec.WaitForExitOrKillAfter(3600000);
			}
			if (pi.Cancelled())
			{
				InformMessageBox.Info(Resources.ConversionAborted);
				if (progressIndicatorForm != null)
				{
					progressIndicatorForm.Dispose();
				}
				return false;
			}
			if (progressIndicatorForm != null)
			{
				progressIndicatorForm.Dispose();
			}
			if (lrf_dec.LastExitCode != 0)
			{
				InformMessageBox.Error(lrf_dec.GetLastGinErrorCodeString());
				return false;
			}
			return true;
		}

		private bool DecompressRawDataFiles(string folderPath, int totalSteps, int currentStep, bool isUsingSubFolders, IProgressIndicator pi, ProcessExitedDelegate processExitedDelegate)
		{
			GZip gZip = new GZip();
			gZip.DecompressFiles(folderPath, isUsingSubFolders, pi, processExitedDelegate);
			if (pi.Cancelled())
			{
				InformMessageBox.Info(Resources.ConversionAborted);
				return false;
			}
			return true;
		}

		internal bool DecompressRawDataFiles(string folderPath, int totalSteps, int currentStep, bool isUsingSubFolders)
		{
			DecompressFilesWithProgressIndicator decompressFilesWithProgressIndicator = new DecompressFilesWithProgressIndicator(folderPath, isUsingSubFolders, string.Format(Resources.StepNumOfTotal, currentStep, totalSteps) + Resources.TitleDecompressFiles);
			if (decompressFilesWithProgressIndicator.ShowDialog() == DialogResult.Cancel)
			{
				InformMessageBox.Info(Resources.ConversionAborted);
				return false;
			}
			return true;
		}

		private bool ConfirmToContinueWithoutMissingFiles(HashSet<string> fileNames)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(Resources.LogFilesCannotBeFoundOnMedia);
			foreach (string current in fileNames)
			{
				stringBuilder.AppendLine(current);
			}
			stringBuilder.AppendLine(Resources.ConversionWillBeIncomplete + " " + Resources.QuestionContinueAnyway);
			return InformMessageBox.Question(stringBuilder.ToString()) == DialogResult.Yes;
		}

		private bool TryMoveFileToFolder(string sourceFilePath, string destinationFolder, bool isOverwritingFiles)
		{
			string fileName = Path.GetFileName(sourceFilePath);
			string text = Path.Combine(destinationFolder, fileName);
			int num = 2;
			if (!isOverwritingFiles)
			{
				goto IL_5A;
			}
			if (!File.Exists(text))
			{
				goto IL_62;
			}
			try
			{
				File.Delete(text);
				goto IL_62;
			}
			catch (Exception)
			{
				goto IL_62;
			}
			IL_33:
			text = Path.Combine(destinationFolder, string.Format("{0}_({1}){2}", Path.GetFileNameWithoutExtension(fileName), num, Path.GetExtension(fileName)));
			num++;
			IL_5A:
			if (File.Exists(text))
			{
				goto IL_33;
			}
			try
			{
				IL_62:
				File.Move(sourceFilePath, Path.Combine(destinationFolder, text));
			}
			catch (Exception)
			{
				InformMessageBox.Error(string.Format(Resources.ErrorCannotMoveFileTo, sourceFilePath, destinationFolder));
				return false;
			}
			return true;
		}

		private void GenerateVSysVarFile(FileConversionDestFormat destinationFormat, LoggerType loggerType, string sourceFolder, string iniFile2Name, string dest)
		{
			if ((destinationFormat == FileConversionDestFormat.ASC || destinationFormat == FileConversionDestFormat.BLF) && loggerType != LoggerType.GL1000 && loggerType != LoggerType.GL1020FTE)
			{
				string iniFilePath = Path.Combine(sourceFolder, iniFile2Name);
				GenerationUtil.GenerateVSysVarFileFromIniFile(dest, iniFilePath);
			}
		}
	}
}
