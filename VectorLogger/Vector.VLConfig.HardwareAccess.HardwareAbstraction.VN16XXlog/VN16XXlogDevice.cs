using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Conversion;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.DeviceInteraction;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.XlApiNet;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction.VN16XXlog
{
	public class VN16XXlogDevice : LoggerDeviceWindowsFileSystem, ICaplCompilerClient, IMultibusChannelDevice
	{
		private readonly byte mHwType;

		private readonly byte mHwIndex;

		private VN16XXlogHardwareInfo mHwInfo;

		private VN16XXlogHardwareInfo mHwInfoConfig;

		private ulong mProperClusterSizeInBytes;

		public string DriverVersion
		{
			get;
			private set;
		}

		public string FirmwareVersionInterfaceMode
		{
			get;
			private set;
		}

		public VN16XXlogHardwareInfo HwInfo
		{
			get
			{
				return this.mHwInfo;
			}
		}

		public EnumInfoType AdditionalInfoType
		{
			get;
			private set;
		}

		public string AdditionalInfoText
		{
			get;
			private set;
		}

		private Dictionary<uint, CANChannel> ConfigCANChannels
		{
			get
			{
				return GenerationUtilVN.ConfigManager.Service.MultibusChannelConfiguration.CANChannels;
			}
		}

		private Dictionary<uint, CANChannel> ConfigCANFDChannels
		{
			get
			{
				return GenerationUtilVN.ConfigManager.Service.MultibusChannelConfiguration.CANFDChannels;
			}
		}

		private Dictionary<uint, CANChannel> ConfigCANStdChannels
		{
			get
			{
				return GenerationUtilVN.ConfigManager.Service.MultibusChannelConfiguration.CANStdChannels;
			}
		}

		private Dictionary<uint, LINChannel> ConfigLINChannels
		{
			get
			{
				return GenerationUtilVN.ConfigManager.Service.MultibusChannelConfiguration.LINChannels;
			}
		}

		private DatabaseConfiguration DatabaseConfiguration
		{
			get
			{
				return GenerationUtilVN.ConfigManager.Service.DatabaseConfiguration;
			}
		}

		public override LoggerType LoggerType
		{
			get
			{
				return LoggerType.VN1630log;
			}
		}

		public override ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return VN16XXlogScanner.LoggerSpecifics;
			}
		}

		public override DateTime CompileDateTime
		{
			get
			{
				return this.compileDateTime;
			}
		}

		public override bool HasIndexFile
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
				ulong num;
				this.DetermineProperClusterSize(out num);
				return this.mProperClusterSizeInBytes == num;
			}
		}

		public override bool HasSnapshotFolderContainingLogData
		{
			get
			{
				return false;
			}
		}

		public override ILogFileStorage LogFileStorage
		{
			get
			{
				return this;
			}
		}

		private bool HardwareConfigurationIsSound
		{
			get
			{
				foreach (s_xl_channel_config current in this.mHwInfo.ChannelInfos)
				{
					if (current.hwChannel > 1)
					{
						break;
					}
					XlChannelConfigErrors configError = (XlChannelConfigErrors)current.configError;
					if (configError != XlChannelConfigErrors.None)
					{
						return false;
					}
				}
				return true;
			}
		}

		public VN16XXlogDevice(byte hwType, byte hwIndex, string hardwareKey, bool isOnline)
		{
			this.mHwType = hwType;
			this.mHwIndex = hwIndex;
			this.hardwareKey = hardwareKey;
			this.isOnline = isOnline;
		}

		public string GetHwTypeName()
		{
			return XlUtils.GetHwName((XlHwType)this.mHwType);
		}

		public bool UpdateFirmware(GlobalOptions globalOptions)
		{
			if (!this.HardwareConfigurationIsSound)
			{
				InformMessageBox.Warning(Resources.FirmwareUpdateNotPossibleDueToInvalidHardwareConfiguration);
				return false;
			}
			bool result;
			using (FirmwareDownloadDialog firmwareDownloadDialog = new FirmwareDownloadDialog(this, globalOptions))
			{
				DialogResult dialogResult = firmwareDownloadDialog.ShowDialog();
				if (dialogResult == DialogResult.OK)
				{
					if (firmwareDownloadDialog.UpdateResult == Result.OK)
					{
						InformMessageBox.Info(string.Format(Resources.FirmwareUpdateSuccessfull, firmwareDownloadDialog.FirmwareImageVersion));
					}
					else
					{
						InformMessageBox.Error(firmwareDownloadDialog.UpdateErrorText);
					}
					result = (firmwareDownloadDialog.UpdateResult == Result.OK);
				}
				else
				{
					result = true;
				}
			}
			return result;
		}

		public bool GetComplierCommandLineArgs(string baseFilePath, out string commandLineArgs, out string errorText)
		{
			string item = baseFilePath + Vocabulary.FileExtensionDotCAN;
			errorText = string.Empty;
			commandLineArgs = string.Empty;
			List<string> list = new List<string>();
			list.Add("/r1");
			list.Add("/o0x400b");
			if (this.mHwInfo == null || this.mHwInfo.CobInfo.memBaseAddr == 0u)
			{
				errorText = string.Format(Resources.ErrorUnableToAccessDevice, this.GetHwTypeName());
				return false;
			}
			List<string> arg_92_0 = list;
			string arg_8D_0 = "-TargetBaseAddress 0x";
			uint memBaseAddr = this.mHwInfo.CobInfo.memBaseAddr;
			arg_92_0.Add(arg_8D_0 + memBaseAddr.ToString("X"));
			list.Add("/aLIN#2");
			list.Add("/aCAN#4");
			IList<uint> channelsForCobNode = this.GetChannelsForCobNode(BusType.Bt_LIN);
			list.AddRange(from chn in channelsForCobNode
			select string.Concat(new object[]
			{
				"/bLIN",
				chn,
				"#LIN",
				chn
			}));
			list.AddRange(from chn in channelsForCobNode
			select "/kLIN" + chn);
			IList<uint> channelsForCobNode2 = this.GetChannelsForCobNode(BusType.Bt_CAN);
			list.AddRange(from chn in channelsForCobNode2
			select "/kCAN" + chn);
			list.Add("/jVLConfig");
			using (IEnumerator<uint> enumerator = channelsForCobNode.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					uint tmpChn = enumerator.Current;
					IList<Database> dBsForChannel = this.GetDBsForChannel(BusType.Bt_LIN, tmpChn);
					list.AddRange(from tmpDb in dBsForChannel
					select this.ComposeDbPathArg(tmpDb, tmpChn));
				}
			}
			using (IEnumerator<uint> enumerator2 = channelsForCobNode2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					uint tmpChn = enumerator2.Current;
					IList<Database> dBsForChannel2 = this.GetDBsForChannel(BusType.Bt_CAN, tmpChn);
					list.AddRange(from tmpDb in dBsForChannel2
					select this.ComposeDbPathArg(tmpDb, tmpChn));
				}
			}
			string targetFolderPath = baseFilePath.Substring(0, baseFilePath.LastIndexOf(Path.DirectorySeparatorChar));
			string text;
			if (GenerationUtilVN.GenerateVSysVarFileForDAIO(targetFolderPath, out errorText, out text) == Result.OK && !string.IsNullOrEmpty(text))
			{
				list.Add("-SysvarFile " + text);
			}
			list.Add(item);
			commandLineArgs = string.Join(" ", list);
			return true;
		}

		public bool GetLinkerCommandLineArgs(string baseFilePath, out string commandLineArgs, out string errorText)
		{
			string str = baseFilePath + Vocabulary.FileExtensionDotCBF;
			errorText = string.Empty;
			commandLineArgs = string.Empty;
			List<string> list = new List<string>();
			list.Add("-Link");
			if (this.mHwInfo == null || this.mHwInfo.CobInfo.memBaseAddr == 0u)
			{
				errorText = string.Format(Resources.ErrorUnableToAccessDevice, this.GetHwTypeName());
				return false;
			}
			List<string> arg_87_0 = list;
			string arg_82_0 = "-TargetBaseAddress 0x";
			uint memBaseAddr = this.mHwInfo.CobInfo.memBaseAddr;
			arg_87_0.Add(arg_82_0 + memBaseAddr.ToString("X"));
			list.Add("-LinkerTraceFlags 0");
			list.Add("-LinkerOutputName " + baseFilePath);
			IList<uint> channelsForCobNode = this.GetChannelsForCobNode(BusType.Bt_LIN);
			list.AddRange(from chn in channelsForCobNode
			select "/kLIN" + chn);
			IList<uint> channelsForCobNode2 = this.GetChannelsForCobNode(BusType.Bt_CAN);
			list.AddRange(from chn in channelsForCobNode2
			select "/kCAN" + chn);
			using (IEnumerator<uint> enumerator = channelsForCobNode.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					uint tmpChn = enumerator.Current;
					IList<Database> dBsForChannel = this.GetDBsForChannel(BusType.Bt_LIN, tmpChn);
					list.AddRange(from tmpDb in dBsForChannel
					select this.ComposeDbPathArg(tmpDb, tmpChn));
				}
			}
			using (IEnumerator<uint> enumerator2 = channelsForCobNode2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					uint tmpChn = enumerator2.Current;
					IList<Database> dBsForChannel2 = this.GetDBsForChannel(BusType.Bt_CAN, tmpChn);
					list.AddRange(from tmpDb in dBsForChannel2
					select this.ComposeDbPathArg(tmpDb, tmpChn));
				}
			}
			list.AddRange(from chn in channelsForCobNode
			select "-transId 1#LIN" + chn + "#1");
			list.AddRange(from chn in channelsForCobNode2
			select "-transId 1#CAN" + chn + "#1");
			bool flag = this.HasCanFD(channelsForCobNode2);
			bool flag2 = this.HasCanStd(channelsForCobNode2);
			int num = 0;
			if (flag && !flag2)
			{
				num = 1;
			}
			else if (!flag && flag2)
			{
				num = 2;
			}
			else if (flag && flag2)
			{
				num = 3;
			}
			list.Add("-CreateMdfHeader " + num);
			list.Add("-RuntimeLibraryType 4");
			list.Add(str + "#1");
			commandLineArgs = string.Join(" ", list);
			return true;
		}

		private IList<uint> GetChannelsForCobNode(BusType busType)
		{
			List<uint> list = new List<uint>();
			switch (busType)
			{
			case BusType.Bt_CAN:
			{
				IList<uint> source = (from t in this.ConfigCANChannels.Keys
				where this.ConfigCANChannels[t].IsActive.Value
				select t).ToList<uint>();
				uint[] channels = this.mHwInfoConfig.GetChannels(XlBusType.XL_BUS_TYPE_CAN);
				list.AddRange(source.Where(new Func<uint, bool>(channels.Contains<uint>)));
				break;
			}
			case BusType.Bt_LIN:
			{
				IList<uint> source2 = (from t in this.ConfigLINChannels.Keys
				where this.ConfigLINChannels[t].IsActive.Value
				select t).ToList<uint>();
				uint[] channels2 = this.mHwInfoConfig.GetChannels(XlBusType.XL_BUS_TYPE_LIN);
				list.AddRange(source2.Where(new Func<uint, bool>(channels2.Contains<uint>)));
				break;
			}
			}
			return list;
		}

		private IList<Database> GetDBsForChannel(BusType busType, uint channelNumber)
		{
			return (from tmpDb in this.DatabaseConfiguration.BusDatabases
			where tmpDb.BusType.Value == busType && tmpDb.ChannelNumber.Value == channelNumber
			select tmpDb).ToList<Database>();
		}

		private string ComposeDbPathArg(Database db, uint chn)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("/d");
			stringBuilder.Append(this.GetAbsoluteFilePath(db.FilePath.Value));
			stringBuilder.Append("#");
			stringBuilder.Append(FileSystemHelpers.MakeNicePath(db.AliasName));
			stringBuilder.Append("#");
			stringBuilder.Append(chn);
			return stringBuilder.ToString();
		}

		private bool HasCanFD(IEnumerable<uint> canChns)
		{
			return canChns.Any(new Func<uint, bool>(this.ConfigCANFDChannels.ContainsKey));
		}

		private bool HasCanStd(IEnumerable<uint> canChns)
		{
			return canChns.Any(new Func<uint, bool>(this.ConfigCANStdChannels.ContainsKey));
		}

		private string GetAbsoluteFilePath(string relativeFilePath)
		{
			return FileSystemHelpers.MakeNicePath(GenerationUtilVN.ConfigManager.Service.GetAbsoluteFilePath(relativeFilePath));
		}

		public string GetTransceiverTypeName(int channelNumber)
		{
			if (this.GetTransceiverType(channelNumber) == XlTransceiverType.XL_TRANSCEIVER_TYPE_NONE)
			{
				return Resources.None;
			}
			int index = channelNumber - 1;
			return this.mHwInfo.ChannelInfos[index].transceiverName;
		}

		public BusType GetTransceiverBusType(int channelNumber)
		{
			if (this.GetTransceiverType(channelNumber) == XlTransceiverType.XL_TRANSCEIVER_TYPE_NONE)
			{
				return BusType.Bt_None;
			}
			int index = channelNumber - 1;
			XlBusType busType = (XlBusType)this.mHwInfo.ChannelInfos[index].busParams.busType;
			switch (busType)
			{
			case XlBusType.XL_BUS_TYPE_CAN:
				return BusType.Bt_CAN;
			case XlBusType.XL_BUS_TYPE_LIN:
				return BusType.Bt_LIN;
			case (XlBusType)3u:
				break;
			case XlBusType.XL_BUS_TYPE_FLEXRAY:
				return BusType.Bt_FlexRay;
			default:
				if (busType == XlBusType.XL_BUS_TYPE_J1708)
				{
					return BusType.Bt_J1708;
				}
				if (busType == XlBusType.XL_BUS_TYPE_ETHERNET)
				{
					return BusType.Bt_Ethernet;
				}
				break;
			}
			return BusType.Bt_None;
		}

		public bool GetCANTransceiverCapabilities(int channelNumber, out bool isLowSpeed, out bool isFD)
		{
			isLowSpeed = false;
			isFD = false;
			if (this.GetTransceiverBusType(channelNumber) != BusType.Bt_CAN)
			{
				return false;
			}
			int index = channelNumber - 1;
			XlChannelFlags channelCapabilities = (XlChannelFlags)this.mHwInfo.ChannelInfos[index].channelCapabilities;
			isFD = channelCapabilities.HasFlag((XlChannelFlags)2147483648u);
			bool arg_84_1;
			if (!isFD)
			{
				s_xl_bus_params busParams = this.mHwInfo.ChannelInfos[channelNumber - 1].busParams;
				arg_84_1 = (busParams.can.bitRate <= Constants.MaximumCANLowSpeedTransceiverRate);
			}
			else
			{
				arg_84_1 = false;
			}
			isLowSpeed = arg_84_1;
			return true;
		}

		public override string GetGenericTransceiverTypeName(int channelNumber)
		{
			switch (channelNumber)
			{
			case 1:
				return XlUtils.GetTransceiverName(XlTransceiverType.XL_TRANSCEIVER_TYPE_CAN_1051_CAP_FIX);
			case 2:
				return XlUtils.GetTransceiverName(XlTransceiverType.XL_TRANSCEIVER_TYPE_CAN_1051_CAP_FIX);
			default:
				return Vocabulary.NotAvailable;
			}
		}

		public override bool Update()
		{
			this.mHwInfo = null;
			this.serialNumber = string.Empty;
			this.firmwareVersion = string.Empty;
			this.DriverVersion = string.Empty;
			this.FirmwareVersionInterfaceMode = string.Empty;
			this.hasLoggerInfo = false;
			this.mHwInfoConfig = VN16XXlogHardwareInfo.CreateFromConfig();
			this.mHwInfo = (VN16XXlogScanner.GetHardwareInfo(this.mHwType, this.mHwIndex) ?? this.mHwInfoConfig);
			if (!this.mHwInfo.ChannelInfos.Any<s_xl_channel_config>())
			{
				return true;
			}
			this.isMemoryCardReady = base.IsWindowsFormattedMediaAccessible();
			if (this.isMemoryCardReady)
			{
				this.UpdateConfigFileDisplayValues();
			}
			this.UpdateAdditionalInfo();
			this.hasLoggerInfo = true;
			this.serialNumber = this.mHwInfo.SerialNumber.ToString(CultureInfo.InvariantCulture);
			this.DriverVersion = this.GetVersionString(this.mHwInfo.DriverVersion);
			this.FirmwareVersionInterfaceMode = this.GetVersionString(this.mHwInfo.FirmwareVersion);
			this.firmwareVersion = this.GetVersionString(this.mHwInfo.CobInfo.firmwareVersionOnFlash.dwVersion);
			return true;
		}

		public override bool FormatCard()
		{
			if (DialogResult.Yes != InformMessageBox.Question(string.Format(Resources.QuestionStartOfFormatting, this.hardwareKey)))
			{
				return false;
			}
			this.DetermineProperClusterSize();
			FormatMemoryCard formatMemoryCard = new FormatMemoryCard();
			bool flag = formatMemoryCard.FormatFAT32(this.hardwareKey, this.mProperClusterSizeInBytes, Vocabulary.VolumeLabelVN1630log);
			InformMessageBox.Info(flag ? Resources.MemCardFormatSuccess : Resources.MemCardFormatFailed);
			return flag;
		}

		public override bool WriteConfiguration(out string codMD5Hash, bool showProgressBar)
		{
			GenerationUtilVN.CaplCompilerClient = this;
			bool result = base.WriteConfiguration(out codMD5Hash, showProgressBar);
			GenerationUtilVN.CaplCompilerClient = null;
			return result;
		}

		public override bool SetRealTimeClock()
		{
			s_xl_channel_config s_xl_channel_config;
			if (this.HwInfo == null || !this.mHwInfo.ChannelInfos.Any<s_xl_channel_config>() || !this.HwInfo.GetFirstCanChannel(out s_xl_channel_config))
			{
				return false;
			}
			XlApi xlApi = new XlApi();
			bool result;
			using (XlDriver xlDriver = new XlDriver(xlApi))
			{
				if (xlDriver.Status != XlStatus.XL_SUCCESS)
				{
					result = false;
				}
				else
				{
					using (XlPort xlPort = new XlPort(xlApi, Application.ProductName, s_xl_channel_config.channelMask, 131072u, 0u, s_xl_channel_config.busParams.busType))
					{
						if (xlPort.Status != XlStatus.XL_SUCCESS)
						{
							result = false;
						}
						else
						{
							s_xl_rtc_timedate dateTime = default(s_xl_rtc_timedate).Init();
							DateTime now = DateTime.Now;
							dateTime.tm_year = now.Year;
							dateTime.tm_month = now.Month;
							dateTime.tm_mday = now.Day;
							dateTime.tm_hour = now.Hour;
							dateTime.tm_min = now.Minute;
							dateTime.tm_sec = now.Second;
							result = (XlStatus.XL_SUCCESS == xlApi.XlRtcSetTimeDate(xlPort.PortHandle, xlPort.AccessMask, dateTime));
						}
					}
				}
			}
			return result;
		}

		public override bool SetVehicleName(string newName)
		{
			return false;
		}

		public override bool WriteLicense(string licenseFilePath)
		{
			return false;
		}

		public override bool GetCANTransceiverTypeForChannel(uint channelNr, out CANTransceiverType transceiverType)
		{
			transceiverType = CANTransceiverType.Unknown;
			return false;
		}

		public override bool DeleteAllLogFiles()
		{
			if (DialogResult.Yes != InformMessageBox.Question(Resources.DeleteExistingLogfilesFromMemoryCard))
			{
				return false;
			}
			foreach (ILogFile current in this.logFiles)
			{
				if (Resources.FileManagerColFileTypeLogData.Equals(current.TypeName))
				{
					FileInfo fileInfo = new FileInfo(current.FullPath + Path.DirectorySeparatorChar + current.DefaultName);
					if (fileInfo.Exists)
					{
						fileInfo.Delete();
					}
				}
			}
			DriveInfo driveInfo = new DriveInfo(this.hardwareKey);
			DirectoryInfo[] directories = driveInfo.RootDirectory.GetDirectories();
			DirectoryInfo[] array = directories;
			for (int i = 0; i < array.Length; i++)
			{
				DirectoryInfo dirInfo = array[i];
				this.DeleteEmptyDirectoriesRecursive(dirInfo);
			}
			if (this.HasErrorFile)
			{
				string path = Path.Combine(this.hardwareKey, this.LoggerSpecifics.DataStorage.ErrorFilePath);
				if (File.Exists(path))
				{
					try
					{
						File.Delete(path);
					}
					catch
					{
						InformMessageBox.Error(string.Format(Resources.CannotDeleteFile, Path.GetFileName(this.LoggerSpecifics.DataStorage.ErrorFilePath)));
					}
				}
			}
			return true;
		}

		public override bool ConvertAllLogFiles(FileConversionParameters conversionParameters, DatabaseConfiguration databaseConfiguration, string configurationFolderPath)
		{
			List<string> list = new List<string>();
			foreach (ILogFile current in this.LogFileStorage.LogFiles)
			{
				if (current.IsSelected)
				{
					list.Add(current.FullFilePath);
				}
			}
			this.ConvertFromCardReader(base.HardwareKey, conversionParameters, Path.Combine(conversionParameters.DestinationFolder, this.LogFileStorage.DestSubFolderNamePrimary), list, databaseConfiguration, configurationFolderPath, this.LoggerSpecifics, this.LogFileStorage.LatestDateTimeUncompressedLogFiles);
			return true;
		}

		private void ConvertFromCardReader(string sourceFolder, FileConversionParameters conversionParameters, string destinationFolderPath, IList<string> filesToProcess, DatabaseConfiguration databaseConfiguration, string configurationFolderPath, ILoggerSpecifics loggerSpecifics, DateTime latestFileTimestamp)
		{
			string empty = string.Empty;
			if (conversionParameters == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(sourceFolder) || string.IsNullOrEmpty(destinationFolderPath))
			{
				return;
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
					if (!GenerationUtil.TryDeleteFile(filename, out empty))
					{
						InformMessageBox.Error(empty);
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
						if (!GenerationUtil.TryDeleteFile(filename2, out empty))
						{
							InformMessageBox.Error(empty);
							return;
						}
					}
					if (!GenerationUtil.TryDeleteDirectory(text, out empty))
					{
						InformMessageBox.Error(empty);
						return;
					}
				}
			}
			else
			{
				Directory.CreateDirectory(destinationFolderPath);
			}
			int num = (conversionParameters.DestinationFormat == FileConversionDestFormat.MF4) ? 2 : 3;
			int num2 = 1;
			FileSystemServices.WriteProtocolLine(string.Format("Card Reader: Start copying files from card at {0}", DateTime.Now));
			Directory.EnumerateDirectories(sourceFolder, Constants.LogDataFolderSearchPattern, SearchOption.TopDirectoryOnly).Count<string>();
			string progressFormTitle = string.Format(Resources.StepNumOfTotal, num2, num) + Resources.TitleCopyFilesFromMemoryCard;
			GenericCopyFilesWithProgressIndicator genericCopyFilesWithProgressIndicator = new GenericCopyFilesWithProgressIndicator(progressFormTitle);
			GenericCopyFilesWithProgressIndicatorResult genericCopyFilesWithProgressIndicatorResult = genericCopyFilesWithProgressIndicator.CopyFiles(filesToProcess, destinationFolderPath, conversionParameters.OverwriteDestinationFiles);
			if (genericCopyFilesWithProgressIndicatorResult.Type == GenericBackgroundWorkerResult.ResultType.CanceledByUser)
			{
				InformMessageBox.Info(genericCopyFilesWithProgressIndicatorResult.ErrorInfo);
			}
			else
			{
				if (genericCopyFilesWithProgressIndicatorResult.Type == GenericBackgroundWorkerResult.ResultType.Warning)
				{
					InformMessageBox.Warning(genericCopyFilesWithProgressIndicatorResult.ErrorInfo);
					return;
				}
				if (genericCopyFilesWithProgressIndicatorResult.Type != GenericBackgroundWorkerResult.ResultType.Success)
				{
					InformMessageBox.Error(genericCopyFilesWithProgressIndicatorResult.ErrorInfo);
					return;
				}
				num2++;
				IEnumerable<string> files3 = Directory.GetFiles(destinationFolderPath, MDF4FinalizerTool.SearchPatternAllUnfinishedMf4Files);
				progressFormTitle = string.Format(Resources.StepNumOfTotal, num2, num) + Resources.ProgressMdfFinalizeTitle;
				MDF4Finalizer mDF4Finalizer = new MDF4Finalizer(progressFormTitle, conversionParameters);
				List<string> list = new List<string>();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(Resources.LogFilesNotConvertedNotContainData);
				List<string> list2 = new List<string>();
				foreach (string current in files3)
				{
					FileInfo fileInfo = new FileInfo(current);
					try
					{
						if (fileInfo.Length == 0L)
						{
							list.Add(current);
							stringBuilder.AppendLine(Path.GetFileName(current));
							continue;
						}
					}
					catch
					{
						list.Add(current);
						stringBuilder.AppendLine(Path.GetFileName(current));
						continue;
					}
					list2.Add(current);
				}
				if (list.Count > 0)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine(Resources.QuestionContinueAnyway);
					if (InformMessageBox.Question(stringBuilder.ToString()) == DialogResult.No)
					{
						return;
					}
				}
				GenericBackgroundWorkerResult genericBackgroundWorkerResult = mDF4Finalizer.FinalizeFiles(list2);
				if (genericBackgroundWorkerResult.Type == GenericBackgroundWorkerResult.ResultType.CanceledByUser)
				{
					InformMessageBox.Info(genericBackgroundWorkerResult.ErrorInfo);
					return;
				}
				if (genericBackgroundWorkerResult.Type == GenericBackgroundWorkerResult.ResultType.Warning)
				{
					InformMessageBox.Warning(genericBackgroundWorkerResult.ErrorInfo);
					return;
				}
				if (genericBackgroundWorkerResult.Type != GenericBackgroundWorkerResult.ResultType.Success)
				{
					InformMessageBox.Error(genericBackgroundWorkerResult.ErrorInfo);
					return;
				}
				if (conversionParameters.DestinationFormat != FileConversionDestFormat.MF4)
				{
					num2++;
					IEnumerable<string> filesToConvert = from t in Directory.GetFiles(destinationFolderPath, MDF4FinalizerTool.SearchPatternAllFinishedMf4Files)
					where t.EndsWith("mf4", StringComparison.OrdinalIgnoreCase)
					select t;
					progressFormTitle = string.Format(Resources.StepNumOfTotal, num2, num) + Resources.ProgressConvertToDestFormatTitle;
					BinlogConversion binlogConversion = new BinlogConversion(progressFormTitle, conversionParameters);
					BinlogConversionResult binlogConversionResult = binlogConversion.ConvertFiles(filesToConvert);
					if (binlogConversionResult.Type == GenericBackgroundWorkerResult.ResultType.CanceledByUser)
					{
						InformMessageBox.Info(binlogConversionResult.ErrorInfo);
						return;
					}
					if (binlogConversionResult.Type == GenericBackgroundWorkerResult.ResultType.Warning)
					{
						InformMessageBox.Warning(binlogConversionResult.ErrorInfo);
						return;
					}
					if (binlogConversionResult.Type != GenericBackgroundWorkerResult.ResultType.Success)
					{
						InformMessageBox.Error(binlogConversionResult.ErrorInfo);
						return;
					}
				}
				if (conversionParameters.DestinationFormat != FileConversionDestFormat.MF4)
				{
					using (new WaitCursor())
					{
						IEnumerable<string> enumerable = from t in Directory.GetFiles(destinationFolderPath, MDF4FinalizerTool.SearchPatternAllFinishedMf4Files)
						where t.EndsWith("mf4", StringComparison.OrdinalIgnoreCase)
						select t;
						foreach (string current2 in enumerable)
						{
							GenerationUtil.TryDeleteFile(current2, out empty);
						}
					}
				}
				if (!conversionParameters.SaveRawFile)
				{
					using (new WaitCursor())
					{
						IEnumerable<string> files4 = Directory.GetFiles(destinationFolderPath, MDF4FinalizerTool.SearchPatternAllUnfinishedMf4Files);
						foreach (string current3 in files4)
						{
							GenerationUtil.TryDeleteFile(current3, out empty);
						}
					}
				}
				return;
			}
		}

		public override bool ConvertSelectedLogFiles(FileConversionParameters conversionParameters, List<ConversionJob> conversionJobs, DatabaseConfiguration databaseConfiguration, string configurationFolderPath)
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception ex)
			{
				InformMessageBox.Error(ex.Message + Environment.NewLine + Environment.NewLine + ex.StackTrace);
			}
			return false;
		}

		public override bool CopyAndBatchExportSelectedLogFiles(FileConversionParameters conversionParameters, ConversionJob allInOneJob, string pathToExportBatchFile, ref string destination)
		{
			return false;
		}

		private XlTransceiverType GetTransceiverType(int channelNumber)
		{
			int num = channelNumber - 1;
			if (num < 0 || num >= this.mHwInfo.ChannelInfos.Count)
			{
				return XlTransceiverType.XL_TRANSCEIVER_TYPE_NONE;
			}
			return (XlTransceiverType)this.mHwInfo.ChannelInfos[num].transceiverType;
		}

		private string GetVersionString(uint version)
		{
			uint num = version >> 24;
			uint num2 = version >> 16 & 255u;
			uint num3 = version & 65535u;
			return string.Concat(new object[]
			{
				num,
				".",
				num2,
				".",
				num3
			});
		}

		private void DetermineProperClusterSize()
		{
			ulong num;
			this.DetermineProperClusterSize(out num);
		}

		private void DetermineProperClusterSize(out ulong actualClusterSize)
		{
			if (!base.IsMemoryCardReady)
			{
				this.mProperClusterSizeInBytes = 0uL;
				actualClusterSize = 0uL;
				return;
			}
			ulong num;
			if (!FileSystemServices.GetMemoryCardClusterSize(this.hardwareKey, out actualClusterSize, out num))
			{
				return;
			}
			if (num <= Constants.CardSize512MB_Bytes)
			{
				this.mProperClusterSizeInBytes = Constants.ClusterSize4k_Bytes;
				return;
			}
			if (num <= Constants.CardSize1GB_Bytes)
			{
				this.mProperClusterSizeInBytes = Constants.ClusterSize8k_Bytes;
				return;
			}
			if (num <= Constants.CardSize2GB_Bytes)
			{
				this.mProperClusterSizeInBytes = Constants.ClusterSize16k_Bytes;
				return;
			}
			this.mProperClusterSizeInBytes = Constants.ClusterSize32k_Bytes;
		}

		private void UpdateConfigFileDisplayValues()
		{
			this.name = string.Empty;
			if (!this.isMemoryCardReady)
			{
				return;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(this.hardwareKey);
			if (!directoryInfo.Exists)
			{
				return;
			}
			FileInfo[] files = directoryInfo.GetFiles("*", SearchOption.TopDirectoryOnly);
			IList<FileInfo> source = (from t in files
			where Vocabulary.FileNameCompiledCAPL.Equals(t.Name, StringComparison.OrdinalIgnoreCase)
			select t).ToList<FileInfo>();
			if (!source.Any<FileInfo>())
			{
				return;
			}
			this.name = source.First<FileInfo>().Name;
			this.compileDateTime = source.First<FileInfo>().LastWriteTime;
		}

		private void DeleteEmptyDirectoriesRecursive(DirectoryInfo dirInfo)
		{
			if (dirInfo == null || !dirInfo.Exists)
			{
				return;
			}
			DirectoryInfo[] directories = dirInfo.GetDirectories();
			if (directories.Length > 0)
			{
				DirectoryInfo[] array = directories;
				for (int i = 0; i < array.Length; i++)
				{
					DirectoryInfo dirInfo2 = array[i];
					this.DeleteEmptyDirectoriesRecursive(dirInfo2);
				}
			}
			if (!dirInfo.GetFileSystemInfos().Any<FileSystemInfo>())
			{
				dirInfo.Delete();
			}
		}

		private void UpdateAdditionalInfo()
		{
			this.AdditionalInfoType = EnumInfoType.None;
			this.AdditionalInfoText = string.Empty;
			RtfText rtfText = new RtfText();
			foreach (s_xl_channel_config current in this.mHwInfo.ChannelInfos)
			{
				XlChannelConfigErrors configError = (XlChannelConfigErrors)current.configError;
				if (configError != XlChannelConfigErrors.None)
				{
					if (current.hwChannel > 1)
					{
						break;
					}
					if (configError.HasFlag(XlChannelConfigErrors.XL_CHANNEL_CONFIG_ERROR_DIP_SWITCH_SETTING_INVALID) && configError.HasFlag(XlChannelConfigErrors.XL_CHANNEL_CONFIG_ERROR_TRANSCEIVER_NOT_PRESENT))
					{
						this.AdditionalInfoType = EnumInfoType.Error;
						if (rtfText.IsEmpty)
						{
							rtfText.Append(Resources.DeviceVN1630logInvalidHardwareConfigurationRtfPart);
						}
						rtfText.Append(string.Format(Resources.DeviceVN1630logChannelPiggyExpectedButNotFoundRtfPart, (int)(current.hwChannel + 1)));
					}
				}
			}
			if (!rtfText.IsEmpty)
			{
				this.AdditionalInfoText = rtfText.ToString();
			}
			if (this.AdditionalInfoType != EnumInfoType.None)
			{
				return;
			}
			foreach (s_xl_channel_config current2 in this.mHwInfo.ChannelInfos)
			{
				XlChannelConfigErrors configError2 = (XlChannelConfigErrors)current2.configError;
				if (configError2 != XlChannelConfigErrors.None)
				{
					if (current2.hwChannel > 1)
					{
						break;
					}
					if (configError2.HasFlag(XlChannelConfigErrors.XL_CHANNEL_CONFIG_ERROR_DIP_SWITCH_SETTING_INVALID))
					{
						this.AdditionalInfoType = EnumInfoType.Error;
						if (rtfText.IsEmpty)
						{
							rtfText.Append(Resources.DeviceVN1630logInvalidHardwareConfigurationRtfPart);
						}
						rtfText.Append(string.Format(Resources.DeviceVN1630logChannelNoPiggyExpectedButFoundRtfPart, (int)(current2.hwChannel + 1)));
					}
				}
			}
			if (!rtfText.IsEmpty)
			{
				this.AdditionalInfoText = rtfText.ToString();
			}
			if (this.AdditionalInfoType != EnumInfoType.None)
			{
				return;
			}
			foreach (s_xl_channel_config current3 in this.mHwInfo.ChannelInfos)
			{
				XlChannelConfigErrors configError3 = (XlChannelConfigErrors)current3.configError;
				if (configError3 != XlChannelConfigErrors.None)
				{
					if (current3.hwChannel > 1)
					{
						break;
					}
					if (configError3.HasFlag(XlChannelConfigErrors.XL_CHANNEL_CONFIG_ERROR_BUS_TYPE_MISMATCH))
					{
						this.AdditionalInfoType = EnumInfoType.Error;
						if (rtfText.IsEmpty)
						{
							rtfText.Append(Resources.DeviceVN1630logInvalidHardwareConfigurationRtfPart);
						}
						rtfText.Append(string.Format(Resources.DeviceVN1630logChannelInvalidBusTypeRtfPart, (int)(current3.hwChannel + 1)));
					}
				}
			}
			if (!rtfText.IsEmpty)
			{
				this.AdditionalInfoText = rtfText.ToString();
			}
			if (this.AdditionalInfoType != EnumInfoType.None)
			{
				return;
			}
			foreach (s_xl_channel_config current4 in this.mHwInfo.ChannelInfos)
			{
				XlChannelConfigErrors configError4 = (XlChannelConfigErrors)current4.configError;
				if (configError4 != XlChannelConfigErrors.None)
				{
					if (current4.hwChannel > 1)
					{
						break;
					}
					this.AdditionalInfoType = EnumInfoType.Error;
					if (rtfText.IsEmpty)
					{
						rtfText.Append(Resources.DeviceVN1630logInvalidHardwareConfigurationRtfPart);
					}
					rtfText.Append(string.Format(Resources.DeviceVN1630logChannelGeneralErrorRtfPart, (int)(current4.hwChannel + 1)));
				}
			}
			if (!rtfText.IsEmpty)
			{
				this.AdditionalInfoText = rtfText.ToString();
			}
			if (this.AdditionalInfoType != EnumInfoType.None)
			{
				return;
			}
			if (!base.IsMemoryCardReady)
			{
				this.AdditionalInfoType = EnumInfoType.Info;
				rtfText.Append(Resources.DeviceVN1630logConnectedButCardSlotEmptyRtfPart);
				this.AdditionalInfoText = rtfText.ToString();
				return;
			}
			if (base.IsMemoryCardReady && !this.HasProperClusterSize)
			{
				this.AdditionalInfoType = EnumInfoType.Info;
				this.AdditionalInfoText = string.Empty;
			}
		}
	}
}
