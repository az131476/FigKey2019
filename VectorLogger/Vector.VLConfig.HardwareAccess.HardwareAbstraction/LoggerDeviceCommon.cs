using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public abstract class LoggerDeviceCommon : ILogFileStorage, ILoggerDevice
	{
		protected const int TimeoutToWaitForProcessFinished_ms = 3600000;

		protected string hardwareKey;

		protected string serialNumber;

		protected string vehicleName;

		protected string name;

		protected string firmwareVersion;

		protected DateTime compileDateTime;

		protected string installedLicenses;

		protected bool isOnline;

		protected bool isMemoryCardReady;

		protected bool hasLoggerInfo;

		protected bool hasConfiguration;

		protected bool hasIndexFile;

		protected string logDiskInfo;

		protected long totalSpace;

		protected long freeSpace;

		protected uint triggeredBuffers;

		protected uint recordingBuffers;

		protected uint[] highestTriggerFileIndices;

		protected IList<ILogFile> logFiles;

		protected Dictionary<uint, uint> numOfLogFilesOnMemNr;

		protected Dictionary<uint, uint> numOfCompLogFilesOnMemNr;

		protected uint numOfClassificFiles;

		protected uint numOfWavFiles;

		protected uint numOfJpegFiles;

		protected uint numOfZipArchives;

		protected bool isPrimaryFileGroupCompressed;

		protected DateTime latestDateTimeCompressed;

		protected DateTime latestDateTimeUncompressed;

		protected bool hasMixedCompUncompFiles;

		protected string destSubFolderNamePrimary;

		protected string destSubFolderNameSecondary;

		protected bool isIniFileMissing;

		protected bool isLogFileStorageOutdated;

		protected static readonly string _SnapshotFolderName = "snapshot";

		public abstract LoggerType LoggerType
		{
			get;
		}

		public abstract ILoggerSpecifics LoggerSpecifics
		{
			get;
		}

		public string HardwareKey
		{
			get
			{
				return this.hardwareKey;
			}
		}

		public string SerialNumber
		{
			get
			{
				return this.serialNumber;
			}
		}

		public string VehicleName
		{
			get
			{
				return this.vehicleName;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public string FirmwareVersion
		{
			get
			{
				return this.firmwareVersion;
			}
		}

		public virtual DateTime CompileDateTime
		{
			get
			{
				return this.compileDateTime;
			}
		}

		public string InstalledLicenses
		{
			get
			{
				return this.installedLicenses;
			}
		}

		public bool IsOnline
		{
			get
			{
				return this.isOnline;
			}
		}

		public bool IsMemoryCardReady
		{
			get
			{
				return this.isMemoryCardReady;
			}
		}

		public bool HasLoggerInfo
		{
			get
			{
				return this.hasLoggerInfo;
			}
		}

		public bool HasConfiguration
		{
			get
			{
				return this.hasConfiguration;
			}
		}

		public abstract bool HasIndexFile
		{
			get;
		}

		public abstract bool HasErrorFile
		{
			get;
		}

		public bool IsLocatedAtNetwork
		{
			get;
			protected set;
		}

		public bool IsZipArchive
		{
			get
			{
				if (this.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem && !string.IsNullOrEmpty(this.HardwareKey))
				{
					string extension = Path.GetExtension(this.HardwareKey);
					if (!string.IsNullOrEmpty(extension) && string.Compare(extension, Vocabulary.FileExtensionDotZIP, true) == 0)
					{
						return true;
					}
				}
				return false;
			}
		}

		public abstract bool HasSnapshotFolderContainingLogData
		{
			get;
		}

		public string SnapshotFolderPath
		{
			get
			{
				return string.Concat(new object[]
				{
					this.HardwareKey.TrimEnd(new char[]
					{
						Path.DirectorySeparatorChar
					}),
					Path.DirectorySeparatorChar,
					LoggerDeviceCommon._SnapshotFolderName,
					Path.DirectorySeparatorChar
				});
			}
		}

		public string LogDiskInfo
		{
			get
			{
				return this.logDiskInfo;
			}
		}

		public abstract bool HasProperClusterSize
		{
			get;
		}

		public abstract bool IsFAT32Formatted
		{
			get;
		}

		public abstract ILogFileStorage LogFileStorage
		{
			get;
		}

		public long TotalSpace
		{
			get
			{
				return this.totalSpace;
			}
		}

		public long FreeSpace
		{
			get
			{
				return this.freeSpace;
			}
		}

		public uint NumberOfTriggeredBuffers
		{
			get
			{
				return this.triggeredBuffers;
			}
		}

		public uint NumberOfRecordingBuffers
		{
			get
			{
				return this.recordingBuffers;
			}
		}

		public uint[] HighestTriggerFileIndices
		{
			get
			{
				return this.highestTriggerFileIndices;
			}
		}

		public ReadOnlyCollection<ILogFile> LogFiles
		{
			get
			{
				return new ReadOnlyCollection<ILogFile>(this.logFiles);
			}
		}

		public bool IsOutdated
		{
			get
			{
				return this.isLogFileStorageOutdated;
			}
		}

		public bool HasMixedCompUncompFiles
		{
			get
			{
				return this.hasMixedCompUncompFiles;
			}
		}

		public bool IsPrimaryFileGroupCompressed
		{
			get
			{
				return this.isPrimaryFileGroupCompressed;
			}
		}

		public DateTime LatestDateTimeCompressedLogfiles
		{
			get
			{
				return this.latestDateTimeCompressed;
			}
		}

		public DateTime LatestDateTimeUncompressedLogFiles
		{
			get
			{
				return this.latestDateTimeUncompressed;
			}
		}

		public uint NumberOfClassificFiles
		{
			get
			{
				return this.numOfClassificFiles;
			}
		}

		public uint NumberOfJpegFiles
		{
			get
			{
				return this.numOfJpegFiles;
			}
		}

		public uint NumberOfZipArchives
		{
			get
			{
				return this.numOfZipArchives;
			}
		}

		public uint NumberOfWavFiles
		{
			get
			{
				return this.numOfWavFiles;
			}
		}

		public uint NumberOfDriveRecorderFiles
		{
			get
			{
				uint num = 0u;
				if (this.numOfLogFilesOnMemNr.ContainsKey(0u))
				{
					num += this.numOfLogFilesOnMemNr[0u];
				}
				if (this.numOfCompLogFilesOnMemNr.ContainsKey(0u))
				{
					num += this.numOfCompLogFilesOnMemNr[0u];
				}
				return num;
			}
		}

		public string DestSubFolderNamePrimary
		{
			get
			{
				return this.destSubFolderNamePrimary;
			}
		}

		public string DestSubFolderNameSecondary
		{
			get
			{
				return this.destSubFolderNameSecondary;
			}
		}

		public LoggerDeviceCommon()
		{
			this.logFiles = new List<ILogFile>();
			this.numOfLogFilesOnMemNr = new Dictionary<uint, uint>();
			this.numOfCompLogFilesOnMemNr = new Dictionary<uint, uint>();
			this.Reset();
		}

		private void Reset()
		{
			this.name = "";
			this.serialNumber = "";
			this.vehicleName = "";
			this.totalSpace = 0L;
			this.freeSpace = 0L;
			this.compileDateTime = new DateTime(0L);
			this.installedLicenses = "";
			this.isMemoryCardReady = true;
			this.hasLoggerInfo = false;
			this.hasConfiguration = false;
			this.IsLocatedAtNetwork = false;
			this.logDiskInfo = "";
			this.ResetFileList();
		}

		protected void ResetFileList()
		{
			this.logFiles.Clear();
			this.isLogFileStorageOutdated = true;
			this.triggeredBuffers = 0u;
			this.recordingBuffers = 0u;
			this.highestTriggerFileIndices = new uint[1];
			this.numOfLogFilesOnMemNr.Clear();
			this.numOfCompLogFilesOnMemNr.Clear();
			for (uint num = 0u; num <= this.LoggerSpecifics.DataStorage.NumberOfMemories; num += 1u)
			{
				this.numOfLogFilesOnMemNr[num] = 0u;
				this.numOfCompLogFilesOnMemNr[num] = 0u;
			}
			this.numOfClassificFiles = 0u;
			this.numOfWavFiles = 0u;
			this.numOfJpegFiles = 0u;
			this.numOfZipArchives = 0u;
			this.hasIndexFile = false;
			this.isPrimaryFileGroupCompressed = false;
			this.latestDateTimeCompressed = DateTime.MinValue;
			this.latestDateTimeUncompressed = DateTime.MinValue;
			this.hasMixedCompUncompFiles = false;
			this.isIniFileMissing = true;
			this.destSubFolderNamePrimary = "";
			this.destSubFolderNameSecondary = "";
		}

		public abstract string GetGenericTransceiverTypeName(int channelNumber);

		public abstract bool Update();

		public abstract bool ConvertAllLogFiles(FileConversionParameters conversionParameters, DatabaseConfiguration databaseConfiguration, string configurationFolderPath);

		public abstract bool ConvertSelectedLogFiles(FileConversionParameters conversionParameters, List<ConversionJob> conversionJobs, DatabaseConfiguration databaseConfiguration, string configurationFolderPath);

		public abstract bool CopyAndBatchExportSelectedLogFiles(FileConversionParameters conversionParameters, ConversionJob allInOneJob, string pathToExportBatchFile, ref string destination);

		public abstract bool Clear();

		public abstract bool FormatCard();

		public abstract bool WriteConfiguration(out string codMD5Hash, bool showProgressBar);

		public abstract bool WriteConfiguration(string codFilePath, bool showProgressBar);

		public abstract bool WriteAnalysisPackage(string analysisPackagePath);

		public abstract bool WriteProjectZIPFile(string zipFilePath);

		public abstract string[] GetProjectZIPFilePath();

		public abstract bool DownloadProjectZIPFile();

		public abstract bool CopyAnalysisPackage(string destFolder);

		public abstract bool HasAnalysisPackage();

		public abstract string GetAnalysisPackagePath();

		public abstract string[] GetCODFilePath();

		public abstract bool WriteLicense(string licenseFilePath);

		public abstract bool SetRealTimeClock();

		public abstract bool SetVehicleName(string name);

		public abstract bool GetCANTransceiverTypeForChannel(uint channelNr, out CANTransceiverType transceiverType);

		public void DataSourceHasChanged()
		{
			this.isLogFileStorageOutdated = true;
		}

		public abstract bool UpdateFileList();

		public abstract bool DeleteAllLogFiles();

		public uint NumberOfLogFilesOnMemory(uint memoryNr)
		{
			if (this.numOfLogFilesOnMemNr.ContainsKey(memoryNr))
			{
				return this.numOfLogFilesOnMemNr[memoryNr];
			}
			return 0u;
		}

		public uint NumberOfCompLogFilesOnMemory(uint memoryNr)
		{
			if (this.numOfCompLogFilesOnMemNr.ContainsKey(memoryNr))
			{
				return this.numOfCompLogFilesOnMemNr[memoryNr];
			}
			return 0u;
		}

		public bool GenerateFolderNameFromLatestFileDate(out string folderName)
		{
			folderName = "";
			DateTime dateTime;
			if (!this.GetLatestFileDate(out dateTime))
			{
				return false;
			}
			if (dateTime.Year >= Constants.EarliestValidLogDateYear)
			{
				string format = "yyyy-MM-dd_HH-mm-ss";
				folderName = dateTime.ToString(format);
			}
			else
			{
				folderName = "NoDate";
			}
			return true;
		}

		protected bool GetLatestFileDate(out DateTime latestTimestamp)
		{
			latestTimestamp = DateTime.MinValue;
			if (this.logFiles.Count == 0)
			{
				return false;
			}
			foreach (ILogFile current in this.logFiles)
			{
				if (current.Timestamp > latestTimestamp)
				{
					latestTimestamp = current.Timestamp;
				}
			}
			return true;
		}

		public IList<string> GetFileNamesOfPrimaryGroup()
		{
			List<string> list = new List<string>();
			foreach (ILogFile current in this.logFiles)
			{
				if (!current.IsSecondary)
				{
					list.Add(Path.Combine(current.FullPath, current.DefaultName));
				}
			}
			return list;
		}

		public IList<string> GetFileNamesOfSecondaryGroup()
		{
			List<string> list = new List<string>();
			foreach (ILogFile current in this.logFiles)
			{
				if (current.IsSecondary)
				{
					list.Add(Path.Combine(current.FullPath, current.DefaultName));
				}
			}
			return list;
		}

		public IList<string> GetDriveRecorderFileNamesOfPrimaryGroup()
		{
			List<string> list = new List<string>();
			foreach (ILogFile current in this.logFiles)
			{
				if (!current.IsSecondary && current.TypeName == Resources.FileManagerColFileTypeDriveRecLogData)
				{
					list.Add(current.DefaultName);
				}
			}
			return list;
		}

		public IList<string> GetDriveRecorderFileNamesOfSecondaryGroup()
		{
			List<string> list = new List<string>();
			foreach (ILogFile current in this.logFiles)
			{
				if (current.IsSecondary && current.TypeName == Resources.FileManagerColFileTypeDriveRecLogData)
				{
					list.Add(current.DefaultName);
				}
			}
			return list;
		}

		void ILogFileStorage.Reset()
		{
			this.Reset();
		}

		protected bool ConvertAndRenameFiles(FileConversionParameters conversionParameters, string destinationFolderPath, DatabaseConfiguration databaseConfiguration, string configurationFolderPath, int currStep, int totalSteps)
		{
			return this.ConvertAndRenameFiles(conversionParameters, destinationFolderPath, databaseConfiguration, configurationFolderPath, null, null, null, currStep, totalSteps);
		}

		protected bool ConvertAndRenameFiles(FileConversionParameters conversionParameters, string destinationFolderPath, DatabaseConfiguration databaseConfiguration, string configurationFolderPath, ConversionJob job, IProgressIndicator pi, ProcessExitedDelegate processExitedDelegate, int currStep, int totalSteps)
		{
			string arg_06_0 = conversionParameters.DestinationFolder;
			string message = "";
			string[] files = Directory.GetFiles(destinationFolderPath, "*.clf");
			int num = 1;
			bool flag = pi != null;
			string[] array = files;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				bool flag2 = false;
				ProgressIndicatorForm progressIndicatorForm = null;
				if (!flag)
				{
					progressIndicatorForm = new ProgressIndicatorForm();
					pi = progressIndicatorForm;
					processExitedDelegate = new ProcessExitedDelegate(progressIndicatorForm.ProcessExited);
				}
				FileSystemServices.WriteProtocolLine(string.Format("Card Reader: Start conversion of files at {0}", DateTime.Now));
				if (conversionParameters.DestinationFormat != FileConversionDestFormat.CLF)
				{
					CLexport cLexport = new CLexport();
					if (progressIndicatorForm != null)
					{
						progressIndicatorForm.Text = string.Format(Resources.TitleConvertFilesFromDevice, new object[]
						{
							currStep,
							totalSteps,
							num,
							files.Count<string>()
						});
					}
					bool result;
					if (!cLexport.ExportCLFAsync(text, conversionParameters, destinationFolderPath, databaseConfiguration, configurationFolderPath, out message, pi, processExitedDelegate, this.LoggerSpecifics, false, job))
					{
						InformMessageBox.Error(message);
						result = false;
					}
					else
					{
						if (progressIndicatorForm != null)
						{
							progressIndicatorForm.ShowDialog();
							Thread.Sleep(100);
						}
						else
						{
							cLexport.WaitForExitOrKillAfter(3600000);
							Thread.Sleep(0);
						}
						if (pi.Cancelled())
						{
							InformMessageBox.Info(Resources.ConversionAborted);
							if (progressIndicatorForm != null)
							{
								progressIndicatorForm.Dispose();
							}
							result = false;
						}
						else if (cLexport.LastExitCode != 0)
						{
							InformMessageBox.Error(cLexport.GetLastGinErrorCodeString());
							result = false;
						}
						else
						{
							num++;
							if (conversionParameters.SaveRawFile || conversionParameters.DestinationFormat == FileConversionDestFormat.CLF)
							{
								goto IL_1C0;
							}
							if (!GenerationUtil.TryDeleteFile(text, out message))
							{
								InformMessageBox.Info(message);
								goto IL_1C0;
							}
							flag2 = true;
							goto IL_1C0;
						}
					}
					return result;
				}
				IL_1C0:
				LoggerDeviceCommon.RenameAfterConvert(conversionParameters, destinationFolderPath, job, this.LoggerSpecifics, this.SerialNumber, this.VehicleName, out message, !flag2, text);
				if (progressIndicatorForm != null)
				{
					progressIndicatorForm.Dispose();
				}
			}
			return true;
		}

		public static void RenameAfterConvertFromCLF(FileConversionParameters conversionParameters, string destinationDirectory, out string errorText, string clfFilePath)
		{
			LoggerDeviceCommon.RenameAfterConvert(conversionParameters, destinationDirectory, null, null, null, null, out errorText, false, clfFilePath);
		}

		private static void RenameAfterConvert(FileConversionParameters conversionParameters, string workingDirectory, ConversionJob job, ILoggerSpecifics loggerSpecifics, string serialNumber, string carNameFromDevice, out string errorText, bool doRenameCLFtoo, string clfFilePath)
		{
			errorText = "";
			string text = Path.Combine(workingDirectory, CLexport.tocFileName);
			bool flag = FileConversionHelper.ShouldExtensionBeReplaced(conversionParameters);
			if (job != null || conversionParameters.FilenameFormat == FileConversionFilenameFormat.UseCustomName || conversionParameters.FilenameFormat == FileConversionFilenameFormat.AddPrefix || flag)
			{
				string text2;
				if (conversionParameters.FilenameFormat == FileConversionFilenameFormat.UseCustomName)
				{
					text2 = conversionParameters.CustomFilename;
				}
				else if (job != null)
				{
					if (job.Type == ConversionJobType.Measurement)
					{
						if (job.FileConversionParameters.SingleFile)
						{
							text2 = "Measurements_merged";
							if (loggerSpecifics.DataStorage.NumberOfMemories > 1u)
							{
								text2 = string.Format("Measurements_merged_Memory{0}", "{MEMORY}");
							}
						}
						else
						{
							text2 = "Measurement_{NAV_NO}";
						}
					}
					else if (job.Type == ConversionJobType.Marker)
					{
						if (job.FileConversionParameters.SingleFile)
						{
							text2 = "Markers_merged";
							if (loggerSpecifics.DataStorage.NumberOfMemories > 1u)
							{
								text2 = string.Format("Markers_merged_Memory{0}", "{MEMORY}");
							}
						}
						else
						{
							text2 = "{MARKER_NAME}_{NAV_NO}";
						}
					}
					else if (job.Type == ConversionJobType.Trigger)
					{
						if (job.FileConversionParameters.SingleFile)
						{
							text2 = "Triggers_merged";
							if (loggerSpecifics.DataStorage.NumberOfMemories > 1u)
							{
								text2 = string.Format("Triggers_merged_Memory{0}", "{MEMORY}");
							}
						}
						else
						{
							text2 = "{TRIGGER_NAME}_{NAV_NO}";
						}
					}
					else
					{
						text2 = "{PSEUDO_ORIGINALNAME}";
					}
					if (conversionParameters.FilenameFormat == FileConversionFilenameFormat.AddPrefix)
					{
						text2 = conversionParameters.Prefix + text2;
					}
				}
				else if (conversionParameters.FilenameFormat == FileConversionFilenameFormat.AddPrefix)
				{
					text2 = conversionParameters.Prefix + "{PSEUDO_ORIGINALNAME}";
				}
				else if (flag)
				{
					text2 = "{PSEUDO_ORIGINALNAME}";
				}
				else
				{
					text2 = "{PSEUDO_ORIGINALNAME}";
				}
				LogFileInfo logFileInfo = new LogFileInfo(loggerSpecifics, serialNumber, carNameFromDevice, clfFilePath, job);
				if (File.Exists(text))
				{
					string[] array = File.ReadAllLines(text);
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string tocLine = array2[i];
						logFileInfo.UpdateSingleLogFileInfo(tocLine);
						string text3 = text2;
						text3 = FileConversionHelper.FillOutMacros(text3, logFileInfo, false, Path.GetFileName(clfFilePath));
						text3 = FileSystemServices.MakeFilenameCompatible(text3, true, 240);
						string originalFilePath = Path.Combine(workingDirectory, Path.GetFileName(logFileInfo.InfoOriginalFilePath));
						string configuredDestinationFormatExtension = FileConversionHelper.GetConfiguredDestinationFormatExtension(conversionParameters);
						string newFilePath = FileSystemServices.PathCombineWithNameLimitation(workingDirectory, text3, configuredDestinationFormatExtension, 250);
						FileConversionHelper.RenameFile(originalFilePath, newFilePath, out errorText, logFileInfo, false);
					}
				}
				if (doRenameCLFtoo && clfFilePath != null)
				{
					string text4 = text2;
					text4 = FileConversionHelper.FillOutMacros(text4, logFileInfo, true, Path.GetFileName(clfFilePath));
					text4 = FileSystemServices.MakeFilenameCompatible(text4, true, 250);
					string newFilePath2 = FileSystemServices.PathCombineWithNameLimitation(Path.GetDirectoryName(clfFilePath), text4, Vocabulary.FileExtensionDotCLF, 250);
					FileConversionHelper.RenameFile(clfFilePath, newFilePath2, out errorText, logFileInfo, true);
				}
			}
			if (File.Exists(text))
			{
				GenerationUtil.TryDeleteFile(text, out errorText);
			}
		}

		protected bool CheckAvailableSpaceOnDestination(FileConversionDestFormat destinationFormat, long sizeOfSourceDataInBytes, string destFolderPath, bool isContentOfDestFolderToBeDeleted)
		{
			long num = 0L;
			long num2 = 0L;
			if (FileSystemServices.GetVolumeSizes(destFolderPath, out num2, out num))
			{
				if (isContentOfDestFolderToBeDeleted)
				{
					long num3 = 0L;
					if (FileSystemServices.GetDirectorySize(destFolderPath, out num3))
					{
						num += num3;
					}
				}
				double num4 = 3.0;
				if (destinationFormat == FileConversionDestFormat.ASC)
				{
					num4 = 5.0;
				}
				if (destinationFormat == FileConversionDestFormat.BLF)
				{
					num4 = 2.5;
				}
				if (destinationFormat == FileConversionDestFormat.CLF)
				{
					num4 = 2.0;
				}
				if (destinationFormat == FileConversionDestFormat.TXT)
				{
					num4 = 5.0;
				}
				if (destinationFormat == FileConversionDestFormat.XLS)
				{
					num4 = 5.0;
				}
				if (this.LoggerSpecifics.Type == LoggerType.GL1000 || this.LoggerSpecifics.Type == LoggerType.GL1020FTE)
				{
					num4 -= 1.0;
				}
				else if (this.IsPrimaryFileGroupCompressed)
				{
					num4 = num4 * 5.0 + 1.0;
				}
				if ((double)num <= (double)sizeOfSourceDataInBytes * num4)
				{
					long num5 = Convert.ToInt64(num4);
					if (InformMessageBox.Question(string.Format(Resources.DestVolHasNotReqDiskSpace, GUIUtil.GetSizeStringMBForBytes(sizeOfSourceDataInBytes * num5)) + " " + Resources.QuestionContinueAnyway) == DialogResult.No)
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
