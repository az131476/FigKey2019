using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Conversion;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public abstract class LoggerDeviceGL1XXX : LoggerDeviceCommon
	{
		protected CANTransceiverType can1TransceiverType;

		protected CANTransceiverType can2TransceiverType;

		protected uint cardWriteCacheSizeKB;

		protected string can1GenericTransceiverName;

		protected string can2GenericTransceiverName;

		private long mDataSize;

		private string zipTempFileDir;

		public override ILogFileStorage LogFileStorage
		{
			get
			{
				return this;
			}
		}

		public override bool HasIndexFile
		{
			get
			{
				return false;
			}
		}

		public override bool HasErrorFile
		{
			get
			{
				return false;
			}
		}

		public override bool HasProperClusterSize
		{
			get
			{
				return true;
			}
		}

		public override bool IsFAT32Formatted
		{
			get
			{
				return false;
			}
		}

		public override bool HasSnapshotFolderContainingLogData
		{
			get
			{
				return false;
			}
		}

		public override string GetGenericTransceiverTypeName(int channelNumber)
		{
			string result = "";
			if (channelNumber == 1)
			{
				result = this.can1GenericTransceiverName;
			}
			if (channelNumber == 2)
			{
				result = this.can2GenericTransceiverName;
			}
			return result;
		}

		public override bool Update()
		{
			this.isMemoryCardReady = true;
			this.hasLoggerInfo = false;
			this.hasIndexFile = false;
			bool flag = false;
			string b = this.hardwareKey + Path.VolumeSeparatorChar + Path.DirectorySeparatorChar;
			bool flag2 = false;
			DriveInfo driveInfo = null;
			DriveInfo[] drives = DriveInfo.GetDrives();
			DriveInfo[] array = drives;
			for (int i = 0; i < array.Length; i++)
			{
				DriveInfo driveInfo2 = array[i];
				if (driveInfo2.Name == b)
				{
					flag2 = true;
					driveInfo = driveInfo2;
					break;
				}
			}
			if (!flag2)
			{
				this.isMemoryCardReady = false;
				return false;
			}
			if (driveInfo != null)
			{
				try
				{
					if ((driveInfo.DriveFormat == Constants.FileSystemFormatFAT || driveInfo.DriveFormat == Constants.FileSystemFormatFAT32 || driveInfo.DriveFormat == Constants.FileSystemFormatNTFS) && driveInfo.IsReady)
					{
						flag = true;
						this.isMemoryCardReady = FileSystemServices.IsDriveExistingAndEmptyExceptSystemVolumeInformation(driveInfo.Name);
					}
				}
				catch (Exception)
				{
					this.isMemoryCardReady = true;
				}
			}
			if (!this.isMemoryCardReady)
			{
				return this.isOnline;
			}
			if (flag)
			{
				try
				{
					this.totalSpace = driveInfo.TotalSize;
					this.freeSpace = driveInfo.AvailableFreeSpace;
				}
				catch (Exception)
				{
				}
				return true;
			}
			GL1000ctrl gL1000ctrl = new GL1000ctrl();
			GL1000Infos gL1000Infos = default(GL1000Infos);
			this.hasLoggerInfo = false;
			this.hasConfiguration = false;
			if (gL1000ctrl.GetLoggerInfos(base.HardwareKey, ref gL1000Infos, ref this.logFiles))
			{
				this.name = gL1000Infos.configName;
				if (!string.IsNullOrEmpty(this.name))
				{
					this.hasConfiguration = true;
				}
				this.compileDateTime = gL1000Infos.compileDateTime;
				this.serialNumber = gL1000Infos.serialNumber;
				if (!string.IsNullOrEmpty(this.serialNumber))
				{
					this.hasLoggerInfo = true;
				}
				if (!string.IsNullOrEmpty(gL1000Infos.vehicleName))
				{
					this.vehicleName = gL1000Infos.vehicleName;
				}
				this.installedLicenses = gL1000Infos.licenses;
				this.firmwareVersion = gL1000Infos.firmwareVersion;
				this.can1TransceiverType = gL1000ctrl.ParseTransceiverEncoding(gL1000Infos.can1Baby);
				this.can2TransceiverType = gL1000ctrl.ParseTransceiverEncoding(gL1000Infos.can2Baby);
				this.can1GenericTransceiverName = "";
				if (!string.IsNullOrEmpty(gL1000Infos.can1Baby))
				{
					int startIndex = gL1000Infos.can1Baby.IndexOf('(');
					this.can1GenericTransceiverName = gL1000Infos.can1Baby.Substring(startIndex);
				}
				this.can2GenericTransceiverName = "";
				if (!string.IsNullOrEmpty(gL1000Infos.can2Baby))
				{
					int startIndex2 = gL1000Infos.can2Baby.IndexOf('(');
					this.can2GenericTransceiverName = gL1000Infos.can2Baby.Substring(startIndex2);
				}
				this.cardWriteCacheSizeKB = gL1000Infos.cardWriteCacheKB;
				this.totalSpace = (long)((ulong)gL1000Infos.memCardSizeMB * 1048576uL);
				this.freeSpace = (long)((ulong)gL1000Infos.freeSpaceMB * 1048576uL);
				this.mDataSize = (long)((ulong)gL1000Infos.dataSizeKB * 1024uL);
				this.recordingBuffers = gL1000Infos.recordingBufs;
				this.triggeredBuffers = gL1000Infos.triggeredBufs;
				this.latestDateTimeUncompressed = DateTime.MinValue;
				foreach (ILogFile current in this.logFiles)
				{
					if (current.Timestamp > this.latestDateTimeUncompressed)
					{
						this.latestDateTimeUncompressed = current.Timestamp;
					}
				}
				string destSubFolderNamePrimary;
				base.GenerateFolderNameFromLatestFileDate(out destSubFolderNamePrimary);
				this.destSubFolderNamePrimary = destSubFolderNamePrimary;
				this.isIniFileMissing = true;
				this.isLogFileStorageOutdated = false;
				return true;
			}
			this.isMemoryCardReady = false;
			this.isLogFileStorageOutdated = false;
			return false;
		}

		public override bool Clear()
		{
			return this.Clear(false);
		}

		private bool Clear(bool isPrecondition)
		{
			GL1000ctrl gL1000ctrl = new GL1000ctrl();
			string message = "";
			bool flag = !string.IsNullOrEmpty(this.name);
			bool flag2 = base.NumberOfRecordingBuffers + base.NumberOfTriggeredBuffers > 0u;
			if (flag)
			{
				if (flag2)
				{
					string message2 = Resources.DeleteExistingLogfilesAndConfigurationFromMemoryCard;
					if (isPrecondition)
					{
						message2 = Resources.ExistingLogFilesAndConfigMustBeDeletedFromMemCard + Environment.NewLine + Resources.QuestionContinue;
					}
					if (InformMessageBox.Question(message2) != DialogResult.Yes)
					{
						return false;
					}
				}
				else
				{
					string message2 = Resources.DeleteExistingConfigurationFromMemoryCard;
					if (isPrecondition)
					{
						message2 = Resources.ExistingConfigMustBeDeletedFromMemCard + Environment.NewLine + Resources.QuestionContinue;
					}
					if (InformMessageBox.Question(message2) != DialogResult.Yes)
					{
						return false;
					}
				}
			}
			else if (flag2)
			{
				string message2 = Resources.DeleteExistingLogfilesFromMemoryCard;
				if (isPrecondition)
				{
					message2 = Resources.ExistingLogFilesMustBeDeletedFromMemCard + Environment.NewLine + Resources.QuestionContinue;
				}
				if (InformMessageBox.Question(message2) != DialogResult.Yes)
				{
					return false;
				}
			}
			else
			{
				gL1000ctrl.ClearMemoryCard(base.HardwareKey, true, out message);
			}
			if (!gL1000ctrl.ClearMemoryCard(base.HardwareKey, true, out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			return true;
		}

		public override bool FormatCard()
		{
			throw new NotImplementedException();
		}

		public override bool WriteConfiguration(out string codMD5Hash, bool showProgressBar)
		{
			if (showProgressBar)
			{
				return new LoadingDialog(Resources.WritingData)
				{
					WriteGL1000ConfigurationTask = new LoadingDialog.WriteGL1000ConfigurationDelegate(this.WriteConfiguration)
				}.ExecuteWriteGL1000Configuration(out codMD5Hash);
			}
			return this.WriteConfiguration(out codMD5Hash);
		}

		public bool WriteConfiguration(out string codMD5Hash)
		{
			codMD5Hash = string.Empty;
			string tempDirectoryName;
			if (!TempDirectoryManager.Instance.CreateNewTempDirectory(out tempDirectoryName))
			{
				InformMessageBox.Error(Resources.ErrorCannotCreateTemporaryFile);
				return false;
			}
			string projectFileName = GenerationUtil.ProjectFileName;
			string path;
			if (!string.IsNullOrEmpty(projectFileName))
			{
				path = Path.ChangeExtension(projectFileName, Vocabulary.FileExtensionDotCOD);
			}
			else
			{
				path = string.Format(Resources.DefaultFileNameFormat, this.LoggerSpecifics.Name, "cod");
			}
			string text = Path.Combine(TempDirectoryManager.Instance.GetFullTempDirectoryPath(tempDirectoryName), path);
			string errorText;
			string compilerErrFilePath;
			bool flag;
			if (GenerationUtil.ExportToLtlOrCodFile(text, out errorText, out compilerErrFilePath, out flag) == Result.Error)
			{
				GUIUtil.ReportFileGenerationResult(EnumInfoType.Error, errorText, compilerErrFilePath);
				return false;
			}
			this.compileDateTime = FileSystemServices.ExtractCODCompileTime(text);
			codMD5Hash = GenerationUtil.ComputeMD5Hash(text);
			return this.WriteConfiguration(text, false);
		}

		public override bool WriteConfiguration(string codFilePath, bool showProgressBar)
		{
			if (showProgressBar)
			{
				return new LoadingDialog(Resources.WritingData)
				{
					CopyGL1000ConfigurationTask = new LoadingDialog.CopyGL1000ConfigurationDelegate(this.WriteConfiguration)
				}.ExecuteCopyGL1000Configuration(codFilePath);
			}
			return this.WriteConfiguration(codFilePath);
		}

		public bool WriteConfiguration(string codFilePath)
		{
			if (!this.Clear(true))
			{
				return false;
			}
			GL1000ctrl gL1000ctrl = new GL1000ctrl();
			string message;
			if (!gL1000ctrl.WriteConfiguration(base.HardwareKey, codFilePath, out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			return true;
		}

		public override bool WriteAnalysisPackage(string analysisPackagePath)
		{
			GL1000ctrl gL1000ctrl = new GL1000ctrl();
			gL1000ctrl.WriteUserData(base.HardwareKey, analysisPackagePath);
			return true;
		}

		public override bool WriteProjectZIPFile(string zipFilePath)
		{
			GL1000ctrl gL1000ctrl = new GL1000ctrl();
			gL1000ctrl.WriteUserData(base.HardwareKey, zipFilePath);
			return true;
		}

		public override string[] GetProjectZIPFilePath()
		{
			string[] array = null;
			try
			{
				GL1000ctrl gL1000ctrl = new GL1000ctrl();
				GL1000Infos gL1000Infos = default(GL1000Infos);
				if (gL1000ctrl.GetLoggerInfos(base.HardwareKey, ref gL1000Infos) && gL1000Infos.userDataFileName != null && gL1000Infos.userDataFileName.Length > 0 && string.Equals(Path.GetExtension(gL1000Infos.userDataFileName), ".zip", StringComparison.OrdinalIgnoreCase))
				{
					if (gL1000Infos.userDataFileName.EndsWith("analysis.zip", true, CultureInfo.InvariantCulture))
					{
						return array;
					}
					if (this.zipTempFileDir == null || this.zipTempFileDir.Length == 0)
					{
						TempDirectoryManager.Instance.CreateNewTempDirectory(out this.zipTempFileDir);
					}
					array = new string[1];
					if (!string.IsNullOrEmpty(this.zipTempFileDir))
					{
						array[0] = Path.Combine(TempDirectoryManager.Instance.GetFullTempDirectoryPath(this.zipTempFileDir), gL1000Infos.userDataFileName);
					}
					else
					{
						array[0] = gL1000Infos.userDataFileName;
					}
				}
			}
			catch (Exception ex)
			{
				InformMessageBox.Error(ex.Message);
			}
			return array;
		}

		public override bool DownloadProjectZIPFile()
		{
			try
			{
				GL1000ctrl gL1000ctrl = new GL1000ctrl();
				GL1000Infos gL1000Infos = default(GL1000Infos);
				if (!gL1000ctrl.GetLoggerInfos(base.HardwareKey, ref gL1000Infos) || gL1000Infos.userDataFileName == null || gL1000Infos.userDataFileName.Length <= 0 || !string.Equals(Path.GetExtension(gL1000Infos.userDataFileName), ".zip", StringComparison.OrdinalIgnoreCase))
				{
					bool result = false;
					return result;
				}
				if (this.zipTempFileDir == null || this.zipTempFileDir.Length == 0)
				{
					TempDirectoryManager.Instance.CreateNewTempDirectory(out this.zipTempFileDir);
				}
				string[] array = new string[]
				{
					Path.Combine(TempDirectoryManager.Instance.GetFullTempDirectoryPath(this.zipTempFileDir), gL1000Infos.userDataFileName)
				};
				gL1000ctrl.ReadUserData(base.HardwareKey, array[0]);
			}
			catch (Exception ex)
			{
				InformMessageBox.Error(ex.Message);
				bool result = false;
				return result;
			}
			return true;
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
				GL1000ctrl gL1000ctrl = new GL1000ctrl();
				string text = Path.Combine(destFolder, this.GetAnalysisPackagePath());
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
					bool result = false;
					return result;
				}
				gL1000ctrl.ReadUserData(base.HardwareKey, text);
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
			string result = null;
			try
			{
				GL1000ctrl gL1000ctrl = new GL1000ctrl();
				GL1000Infos gL1000Infos = default(GL1000Infos);
				if (gL1000ctrl.GetLoggerInfos(base.HardwareKey, ref gL1000Infos) && gL1000Infos.userDataFileName != null && gL1000Infos.userDataFileName.Length > 0 && gL1000Infos.userDataFileName.EndsWith("analysis.zip", true, CultureInfo.InvariantCulture))
				{
					result = gL1000Infos.userDataFileName;
				}
			}
			catch (Exception ex)
			{
				InformMessageBox.Error(ex.Message);
			}
			return result;
		}

		public override string[] GetCODFilePath()
		{
			string[] array = null;
			try
			{
				GL1000ctrl gL1000ctrl = new GL1000ctrl();
				GL1000Infos gL1000Infos = default(GL1000Infos);
				if (gL1000ctrl.GetLoggerInfos(base.HardwareKey, ref gL1000Infos) && gL1000Infos.configName != null && gL1000Infos.configName.Length > 0)
				{
					array = new string[]
					{
						Path.Combine(Path.GetTempPath(), gL1000Infos.configName)
					};
					gL1000ctrl.ReadConfiguration(base.HardwareKey, array[0]);
				}
			}
			catch (Exception ex)
			{
				InformMessageBox.Error(ex.Message);
			}
			return array;
		}

		public override bool WriteLicense(string licenseFilePath)
		{
			if (InformMessageBox.Question(Resources.QuestionInstallLicDeletesLogFiles) == DialogResult.No)
			{
				return false;
			}
			string message = "";
			SecWrite secWrite = new SecWrite();
			if (!secWrite.InstallLicenseFile(licenseFilePath, this.LoggerSpecifics.DeviceAccess.DeviceType, out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			InformMessageBox.Info(Resources.LicenseSuccessfullyWritten);
			return true;
		}

		public override bool SetRealTimeClock()
		{
			GL1000ctrl gL1000ctrl = new GL1000ctrl();
			string message = "";
			if (!gL1000ctrl.SetRealTimeClock(base.HardwareKey, out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			return true;
		}

		public override bool SetVehicleName(string name)
		{
			GL1000ctrl gL1000ctrl = new GL1000ctrl();
			string message = "";
			if (!this.isOnline)
			{
				InformMessageBox.Error(Resources.ErrorNoLoggerDevConnected);
				return false;
			}
			if (!gL1000ctrl.SetVehicleName(name, out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			return true;
		}

		public override bool GetCANTransceiverTypeForChannel(uint channelNr, out CANTransceiverType transceiverType)
		{
			transceiverType = CANTransceiverType.None;
			switch (channelNr)
			{
			case 1u:
				transceiverType = this.can1TransceiverType;
				return true;
			case 2u:
				transceiverType = this.can2TransceiverType;
				return true;
			default:
				return false;
			}
		}

		public override bool UpdateFileList()
		{
			return this.Update();
		}

		public override bool DeleteAllLogFiles()
		{
			bool flag = base.NumberOfRecordingBuffers + base.NumberOfTriggeredBuffers > 0u;
			if (flag && InformMessageBox.Question(Resources.DeleteExistingLogfilesFromMemoryCard) != DialogResult.Yes)
			{
				return false;
			}
			GL1000ctrl gL1000ctrl = new GL1000ctrl();
			string message = "";
			if (!gL1000ctrl.ClearMemoryCard(base.HardwareKey, false, out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			return true;
		}

		public override bool ConvertAllLogFiles(FileConversionParameters conversionParameters, DatabaseConfiguration databaseConfiguration, string configurationFolderPath)
		{
			string message = "";
			if (!Directory.Exists(conversionParameters.DestinationFolder))
			{
				InformMessageBox.Error(string.Format(Resources.ErrorFolderNotFound, conversionParameters.DestinationFolder));
				return false;
			}
			string text = Path.Combine(conversionParameters.DestinationFolder, this.destSubFolderNamePrimary);
			if (!base.CheckAvailableSpaceOnDestination(conversionParameters.DestinationFormat, this.mDataSize, text, true))
			{
				return false;
			}
			if (Directory.Exists(text))
			{
				if (!conversionParameters.OverwriteDestinationFiles && InformMessageBox.Question(Resources.DestinationSubDirectoryExistsOverwrite) == DialogResult.No)
				{
					return false;
				}
				if (Directory.Exists(text))
				{
					string[] files = Directory.GetFiles(text);
					for (int i = 0; i < files.Length; i++)
					{
						string text2 = files[i];
						try
						{
							File.Delete(text2);
						}
						catch
						{
							InformMessageBox.Error(string.Format(Resources.CannotDeleteFile, text2));
							bool result = false;
							return result;
						}
					}
				}
			}
			else
			{
				try
				{
					Directory.CreateDirectory(text);
				}
				catch
				{
					InformMessageBox.Error(string.Format(Resources.ErrorCannotCreateDir, text));
					bool result = false;
					return result;
				}
			}
			ProgressIndicatorForm progressIndicatorForm = new ProgressIndicatorForm();
			progressIndicatorForm.Text = Resources.TitleCopyDecodeFilesFromDevice;
			GL1000ctrl gL1000ctrl = new GL1000ctrl();
			if (!gL1000ctrl.DownloadFile(base.HardwareKey, conversionParameters, Path.Combine(text, Constants.DefaultRawLogDataFileName), progressIndicatorForm, new ProcessExitedDelegate(progressIndicatorForm.ProcessExited), out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			progressIndicatorForm.ShowDialog();
			if (progressIndicatorForm.Cancelled())
			{
				InformMessageBox.Info(Resources.ConversionAborted);
				progressIndicatorForm.Dispose();
				return false;
			}
			progressIndicatorForm.Dispose();
			if (gL1000ctrl.LastExitCode != 0)
			{
				message = gL1000ctrl.GetLastGinErrorCodeString();
				InformMessageBox.Error(message);
				return false;
			}
			ReadOnlyCollection<string> readOnlyCollection = null;
			if (FileConversionHelper.IsSignalOrientedDestFormat(conversionParameters.DestinationFormat) && FileConversionHelper.UseArxmlToDBCConversion(conversionParameters))
			{
				IEnumerable<Database> conversionDatabases = AnalysisPackage.GetConversionDatabases(conversionParameters, databaseConfiguration.Databases, true);
				if (conversionDatabases != null && conversionDatabases.Any<Database>() && !GenerateDBCfromARXML.Execute(conversionDatabases, configurationFolderPath, text, out readOnlyCollection))
				{
					InformMessageBox.Info(Resources.ConversionAborted);
					return true;
				}
			}
			if (!base.ConvertAndRenameFiles(conversionParameters, text, databaseConfiguration, configurationFolderPath, 2, 2))
			{
				return false;
			}
			if (readOnlyCollection != null)
			{
				foreach (string current in readOnlyCollection)
				{
					if (File.Exists(current) && !GenerationUtil.TryDeleteFile(current, out message, true))
					{
						InformMessageBox.Error(message);
					}
				}
			}
			GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.ClfSourceFolders, text);
			if (conversionParameters.DeleteSourceFilesWhenDone && !gL1000ctrl.ClearMemoryCard(base.HardwareKey, false, out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			return true;
		}

		public override bool ConvertSelectedLogFiles(FileConversionParameters conversionParameters, List<ConversionJob> conversionJobs, DatabaseConfiguration databaseConfiguration, string configurationFolderPath)
		{
			throw new NotImplementedException();
		}

		public override bool CopyAndBatchExportSelectedLogFiles(FileConversionParameters conversionParameters, ConversionJob allInOneJob, string pathToExportBatchFile, ref string destination)
		{
			throw new NotImplementedException();
		}
	}
}
