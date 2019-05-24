using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public class ConfigurationManager : IConfigurationManagerService
	{
		public delegate void DataModelChangedHandler(Feature data);

		private VLProject vlProject;

		private IApplicationDatabaseManager applicationDatabaseManager;

		private IDiagSymbolsManager diagSymbolsManager;

		private ILoggerSpecifics loggerSpecifics;

		private IUpdateService updateService;

		private ModelValidator modelValidator;

		private SemanticChecker semanticChecker;

		private ModelEditor modelEditor;

		private DatabaseServices databaseServices;

		private ConfigurationImporter configurationImporter;

		private ConfigurationManager.DataModelChangedHandler dataModelChanged;

		public event ConfigurationManager.DataModelChangedHandler DataModelChanged
		{
			add
			{
				this.dataModelChanged = (ConfigurationManager.DataModelChangedHandler)Delegate.Combine(this.dataModelChanged, value);
			}
			remove
			{
				this.dataModelChanged = (ConfigurationManager.DataModelChangedHandler)Delegate.Remove(this.dataModelChanged, value);
			}
		}

		public VLProject VLProject
		{
			get
			{
				return this.vlProject;
			}
		}

		public IConfigurationManagerService Service
		{
			get
			{
				return this;
			}
		}

		public ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return this.loggerSpecifics;
			}
			set
			{
				this.loggerSpecifics = value;
			}
		}

		public IModelValidator ModelValidator
		{
			get
			{
				if (this.modelValidator == null)
				{
					this.modelValidator = new ModelValidator(this);
				}
				return this.modelValidator;
			}
		}

		public ISemanticChecker SemanticChecker
		{
			get
			{
				if (this.semanticChecker == null)
				{
					this.semanticChecker = new SemanticChecker(this);
				}
				return this.semanticChecker;
			}
		}

		public IModelEditor ModelEditor
		{
			get
			{
				if (this.modelEditor == null)
				{
					this.modelEditor = new ModelEditor(this);
				}
				return this.modelEditor;
			}
		}

		public IDatabaseServices DatabaseServices
		{
			get
			{
				if (this.databaseServices == null)
				{
					this.databaseServices = new DatabaseServices(this);
				}
				return this.databaseServices;
			}
		}

		public ConfigurationImporter ConfigurationImporter
		{
			get
			{
				if (this.configurationImporter == null)
				{
					this.configurationImporter = new ConfigurationImporter(this);
				}
				return this.configurationImporter;
			}
		}

		ProjectRoot IConfigurationManagerService.ProjectRoot
		{
			get
			{
				return this.vlProject.ProjectRoot;
			}
		}

		IFeatureRegistration IConfigurationManagerService.FeatureRegistration
		{
			get
			{
				return this.vlProject;
			}
		}

		IUpdateService IConfigurationManagerService.UpdateService
		{
			get
			{
				return this.updateService;
			}
		}

		public MultibusChannelConfiguration MultibusChannelConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.HardwareConfiguration.MultibusChannelConfiguration;
			}
		}

		public CANChannelConfiguration CANChannelConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration;
			}
		}

		public LINChannelConfiguration LINChannelConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration;
			}
		}

		public FlexrayChannelConfiguration FlexrayChannelConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.HardwareConfiguration.FlexrayChannelConfiguration;
			}
		}

		public MOST150ChannelConfiguration MOST150ChannelConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration;
			}
		}

		public LogDataStorage LogDataStorage
		{
			get
			{
				return this.vlProject.ProjectRoot.HardwareConfiguration.LogDataStorage;
			}
		}

		public DatabaseConfiguration DatabaseConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration;
			}
		}

		public DiagnosticsDatabaseConfiguration DiagnosticsDatabaseConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticsDatabaseConfiguration;
			}
		}

		public DiagnosticActionsConfiguration DiagnosticActionsConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticActionsConfiguration;
			}
		}

		public IList<FilterConfiguration> FilterConfigurations
		{
			get
			{
				return this.vlProject.ProjectRoot.LoggingConfiguration.FilterConfigurations;
			}
		}

		public IList<TriggerConfiguration> TriggerConfigurations
		{
			get
			{
				return this.vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations;
			}
		}

		public SendMessageConfiguration SendMessageConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.OutputConfiguration.SendMessageConfiguration;
			}
		}

		public DigitalOutputsConfiguration DigitalOutputsConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.OutputConfiguration.DigitalOutputsConfiguration;
			}
		}

		public DigitalInputConfiguration DigitalInputConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.HardwareConfiguration.DigitalInputConfiguration;
			}
		}

		public SpecialFeaturesConfiguration SpecialFeaturesConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.LoggingConfiguration.SpecialFeaturesConfiguration;
			}
		}

		public InterfaceModeConfiguration InterfaceModeConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration;
			}
		}

		public AnalogInputConfiguration AnalogInputConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration;
			}
		}

		public IncludeFileConfiguration IncludeFileConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.LoggingConfiguration.IncludeFileConfiguration;
			}
		}

		public GPSConfiguration GPSConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration;
			}
		}

		public WLANConfiguration WLANConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration;
			}
		}

		public EthernetConfiguration EthernetConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.HardwareConfiguration.EthernetConfiguration;
			}
		}

		public LEDConfiguration LEDConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.OutputConfiguration.LEDConfiguration;
			}
		}

		public MetaInformation MetaInformation
		{
			get
			{
				return this.vlProject.ProjectRoot.MetaInformation;
			}
		}

		public CcpXcpSignalConfiguration CcpXcpSignalConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.LoggingConfiguration.CcpXcpSignalConfiguration;
			}
		}

		IApplicationDatabaseManager IConfigurationManagerService.ApplicationDatabaseManager
		{
			get
			{
				return this.applicationDatabaseManager;
			}
		}

		IDiagSymbolsManager IConfigurationManagerService.DiagSymbolsManager
		{
			get
			{
				return this.diagSymbolsManager;
			}
		}

		string IConfigurationManagerService.ConfigFolderPath
		{
			get
			{
				return this.vlProject.GetProjectFolder();
			}
		}

		ILoggerSpecifics IConfigurationManagerService.LoggerSpecifics
		{
			get
			{
				return this.loggerSpecifics;
			}
			set
			{
				this.loggerSpecifics = value;
			}
		}

		IDatabaseServices IConfigurationManagerService.DatabaseServices
		{
			get
			{
				return this.DatabaseServices;
			}
		}

		public ConfigurationManager(IUpdateService service, VLProject vlProj, IApplicationDatabaseManager appDbMan, IDiagSymbolsManager diagSymbolsMan)
		{
			this.applicationDatabaseManager = appDbMan;
			this.diagSymbolsManager = diagSymbolsMan;
			this.vlProject = vlProj;
			this.updateService = service;
			GenerationUtil.ConfigManager = this;
			GenerationUtilVN.ConfigManager = this;
		}

		public void InitializeDefaultConfiguration(ILoggerSpecifics currLoggerSpecifics)
		{
			this.loggerSpecifics = currLoggerSpecifics;
			this.vlProject.CreateEmptyProject(this.loggerSpecifics.Type);
			this.vlProject.ProjectRoot.ApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			this.vlProject.ProjectRoot.HardwareConfiguration.LogDataStorage.IsEnterSleepModeEnabled.Value = true;
			this.vlProject.ProjectRoot.HardwareConfiguration.LogDataStorage.TimeoutToSleep.Value = Constants.DefaultTimeoutToSleep;
			this.vlProject.ProjectRoot.HardwareConfiguration.LogDataStorage.IsFastWakeUpEnabled.Value = false;
			this.vlProject.ProjectRoot.HardwareConfiguration.LogDataStorage.EventActivationDelayAfterStart.Value = Constants.DefaultEventActivationDelay_ms;
			for (uint num = 1u; num <= this.loggerSpecifics.CAN.NumberOfChannels; num += 1u)
			{
				CANChannel cANChannel = new CANChannel();
				if (this.loggerSpecifics.CAN.DefaultActiveChannels.Contains(num))
				{
					cANChannel.IsActive.Value = true;
				}
				else
				{
					cANChannel.IsActive.Value = false;
				}
				cANChannel.IsKeepAwakeActive.Value = true;
				cANChannel.IsWakeUpEnabled.Value = this.loggerSpecifics.CAN.ChannelsWithWakeUpSupport.Contains(num);
				cANChannel.IsOutputActive.Value = true;
				CANStdChipConfiguration cANChipConfiguration = new CANStdChipConfiguration();
				CANChipConfigurationManager.ApplyPredefinedSettingForBaudrate(num, Constants.DefaultCANBaudrateHighSpeed, ref cANChipConfiguration);
				cANChannel.CANChipConfiguration = cANChipConfiguration;
				this.vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration.AddCANChannel(cANChannel);
			}
			for (uint num2 = 1u; num2 <= this.loggerSpecifics.DataStorage.NumberOfMemories; num2 += 1u)
			{
				this.vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration.LogErrorFramesOnMemories.Add(new ValidatedProperty<bool>(true));
			}
			for (uint num3 = 1u; num3 <= this.loggerSpecifics.LIN.NumberOfChannels; num3 += 1u)
			{
				LINChannel lINChannel = new LINChannel();
				if (this.loggerSpecifics.LIN.DefaultActiveChannels.Contains(num3))
				{
					lINChannel.IsActive.Value = true;
				}
				else
				{
					lINChannel.IsActive.Value = false;
				}
				lINChannel.IsKeepAwakeActive.Value = true;
				lINChannel.IsWakeUpEnabled.Value = true;
				lINChannel.SpeedRate.Value = Constants.DefaultLINBaudrate;
				lINChannel.ProtocolVersion.Value = Constants.DefaultLINProtocolVersion;
				lINChannel.UseDbConfigValues.Value = false;
				this.vlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration.AddLINChannel(lINChannel);
			}
			this.vlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration.IsUsingLinProbe.Value = false;
			this.vlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration.CANChannelNrUsedForLinProbe.Value = this.loggerSpecifics.CAN.NumberOfChannels;
			for (uint num4 = 1u; num4 <= this.loggerSpecifics.LIN.NumberOfLINprobeChannels; num4 += 1u)
			{
				LINprobeChannel lINprobeChannel = new LINprobeChannel();
				lINprobeChannel.SpeedRate.Value = Constants.DefaultLINBaudrate;
				this.vlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration.AddLINprobeChannel(lINprobeChannel);
			}
			this.vlProject.ProjectRoot.HardwareConfiguration.MultibusChannelConfiguration.NumberOfChannels = this.loggerSpecifics.Multibus.NumberOfChannels;
			for (uint num5 = 1u; num5 <= this.loggerSpecifics.Multibus.NumberOfChannels; num5 += 1u)
			{
				HardwareChannel channel = this.vlProject.ProjectRoot.HardwareConfiguration.MultibusChannelConfiguration.GetChannel(num5);
				Vector.VLConfig.BusinessLogic.Configuration.ModelEditor.InitializeMultibusChannel(num5, ref channel);
				channel.IsActive.Value = (num5 <= this.loggerSpecifics.Multibus.NumberOfBuildInCANTransceivers);
			}
			this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsChannelEnabled.Value = false;
			this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsKeepAwakeEnabled.Value = true;
			this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsLogStatusEventsEnabled.Value = true;
			this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsAutoStatusEventRepEnabled.Value = false;
			this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsLogControlMsgsEnabled.Value = true;
			this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsLogAsyncMDPEnabled.Value = true;
			this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsLogAsyncMEPEnabled.Value = false;
			for (uint num6 = 1u; num6 <= this.loggerSpecifics.Flexray.NumberOfChannels; num6 += 1u)
			{
				FlexrayChannel flexrayChannel = new FlexrayChannel();
				if (this.loggerSpecifics.Flexray.DefaultActiveChannels.Contains(num6))
				{
					flexrayChannel.IsActive.Value = true;
				}
				else
				{
					flexrayChannel.IsActive.Value = false;
				}
				flexrayChannel.IsKeepAwakeActive.Value = true;
				flexrayChannel.IsWakeUpEnabled.Value = true;
				this.vlProject.ProjectRoot.HardwareConfiguration.FlexrayChannelConfiguration.AddFlexrayChannel(flexrayChannel);
			}
			this.vlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration.MapToSystemChannel.Value = this.loggerSpecifics.IO.AnalogMapToSystemChannel;
			this.vlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration.MapToCANMessage.Value = this.loggerSpecifics.IO.AnalogMapToCANMessage;
			this.vlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration.AnalogInputsCANMappingMode.Value = this.loggerSpecifics.IO.DefaultAnalogInputsMappingMode;
			this.vlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration.CanChannel.Value = this.loggerSpecifics.IO.DefaultAnalogInputsChannel;
			uint num7 = Constants.DefaultAnalogInputMappedCANId;
			for (uint num8 = 1u; num8 <= this.loggerSpecifics.IO.NumberOfAnalogInputs; num8 += 1u)
			{
				AnalogInput analogInput = new AnalogInput();
				analogInput.IsActive.Value = false;
				analogInput.Frequency.Value = Constants.DefaultAnalogInputFrequency;
				if (this.vlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration.AnalogInputsCANMappingMode.Value == AnalogInputsCANMappingMode.SameFixedIDs)
				{
					if ((num8 > 1u && num8 <= 6u && (num8 - 1u) % Constants.NumberOfAnalogInputsOnOneMessage == 0u) || (num8 > 6u && (num8 - 7u) % Constants.NumberOfAnalogInputsOnOneMessage == 0u))
					{
						num7 += 1u;
					}
				}
				else if (num8 > 1u)
				{
					num7 += 1u;
				}
				analogInput.MappedCANId.Value = num7;
				analogInput.IsMappedCANIdExtended.Value = true;
				this.vlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration.AddAnalogInput(analogInput);
			}
			this.vlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration.Averaging.Value = Constants.AnalogInputsAveraging;
			this.vlProject.ProjectRoot.HardwareConfiguration.DigitalInputConfiguration.DigitalInputsMappingMode.Value = this.loggerSpecifics.IO.DefaultDigitalInputsMappingMode;
			this.vlProject.ProjectRoot.HardwareConfiguration.DigitalInputConfiguration.CanChannel.Value = this.loggerSpecifics.IO.DefaultDigitalInputsChannel;
			num7 = Constants.DefaultDigitalInputMappedCANId;
			for (uint num9 = 1u; num9 <= this.loggerSpecifics.IO.NumberOfDigitalInputs; num9 += 1u)
			{
				DigitalInput digitalInput = new DigitalInput();
				digitalInput.IsActiveOnChange.Value = false;
				digitalInput.IsActiveFrequency.Value = false;
				digitalInput.Frequency.Value = Constants.DefaultDigitalInputFrequency;
				digitalInput.MappedCANId.Value = num7;
				if (this.vlProject.ProjectRoot.HardwareConfiguration.DigitalInputConfiguration.DigitalInputsMappingMode.Value == DigitalInputsMappingMode.ContinuousIndividualIDs)
				{
					num7 += 1u;
				}
				digitalInput.IsMappedCANIdExtended.Value = true;
				this.vlProject.ProjectRoot.HardwareConfiguration.DigitalInputConfiguration.AddDigitalInput(digitalInput);
			}
			this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.UseInterfaceMode.Value = false;
			this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.IpAddress.Value = Constants.DefaultInterfaceModeIPAddr;
			this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.SubnetMask.Value = Constants.DefaultInterfaceModeSubnetMask;
			this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.Port.Value = Constants.DefaultInterfaceModePort;
			this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.MarkerCANId.Value = Constants.DefaultInterfaceModeMarkerCANId;
			this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.IsMarkerCANIdExtended.Value = true;
			this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.MarkerChannel.Value = this.loggerSpecifics.Recording.DefaultLogDateTimeChannel;
			this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.IsSendPhysTxActive.Value = true;
			this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.IsSendLoggedTxActive.Value = false;
			this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.IsSendErrorFramesActive.Value = true;
			this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration.MapToSystemChannel.Value = false;
			this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration.MapToCANMessage.Value = false;
			this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration.StartCANId.Value = this.loggerSpecifics.GPS.DefaultLogSerialGPSStartCANId;
			this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration.IsExtendedStartCANId.Value = this.loggerSpecifics.GPS.DefaultLogSerialGPSIsExtendedStartCANId;
			this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration.CANChannel.Value = this.loggerSpecifics.GPS.DefaultLogGPSChannel;
			this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration.Database.Value = "";
			this.vlProject.ProjectRoot.HardwareConfiguration.EthernetConfiguration.Eth1Ip.Value = "192.168.165.10";
			this.vlProject.ProjectRoot.HardwareConfiguration.EthernetConfiguration.Eth1KeepAwake.Value = false;
			this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.EnableExchangeIdHandling.Value = true;
			this.vlProject.ProjectRoot.LoggingConfiguration.NumberOfMemoryUnits = this.loggerSpecifics.DataStorage.NumberOfMemories;
			foreach (FilterConfiguration current in this.vlProject.ProjectRoot.LoggingConfiguration.FilterConfigurations)
			{
				DefaultFilter defaultFilter = current.AddDefaultFilter();
				defaultFilter.Action.Value = FilterActionType.Pass;
			}
			bool value = true;
			foreach (TriggerConfiguration current2 in this.vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations)
			{
				current2.TriggerMode.Value = TriggerMode.Permanent;
				current2.PostTriggerTime.Value = currLoggerSpecifics.Recording.DefaultPostTriggerTimeMilliseconds;
				this.vlProject.ProjectRoot.MetaInformation.BufferSizeCalculatorInformation.PreTriggerTimeSeconds.Value = currLoggerSpecifics.Recording.DefaultPreTriggerTimeSeconds;
				current2.MemoryRingBuffer.IsActive.Value = value;
				current2.MemoryRingBuffer.Size.Value = this.loggerSpecifics.DataStorage.DefaultRingBufferSize;
				current2.MemoryRingBuffer.StoreRAMOnShutdown.Value = false;
				current2.MemoryRingBuffer.MaxNumberOfFiles.Value = this.loggerSpecifics.DataStorage.MaxLoggerFiles;
				current2.MemoryRingBuffer.OperatingMode.Value = this.loggerSpecifics.DataStorage.DefaultOperatingMode;
				value = false;
			}
			this.vlProject.ProjectRoot.LoggingConfiguration.SpecialFeaturesConfiguration.IsLogDateTimeEnabled.Value = false;
			this.vlProject.ProjectRoot.LoggingConfiguration.SpecialFeaturesConfiguration.LogDateTimeChannel.Value = this.loggerSpecifics.Recording.DefaultLogDateTimeChannel;
			this.vlProject.ProjectRoot.LoggingConfiguration.SpecialFeaturesConfiguration.LogDateTimeCANId.Value = this.loggerSpecifics.Recording.DefaultLogDateTimeCANId;
			this.vlProject.ProjectRoot.LoggingConfiguration.SpecialFeaturesConfiguration.LogDateTimeIsExtendedId.Value = this.loggerSpecifics.Recording.DefaultLogDateTimeIsExtendedCANId;
			this.vlProject.ProjectRoot.LoggingConfiguration.SpecialFeaturesConfiguration.IsIncludeLTLCodeEnabled.Value = true;
			this.vlProject.ProjectRoot.LoggingConfiguration.SpecialFeaturesConfiguration.MaxLogFileSize.Value = VN16XXlog.DefaultLogFileSize;
			this.vlProject.ProjectRoot.LoggingConfiguration.SpecialFeaturesConfiguration.IsIgnitionKeepsLoggerAwakeEnabled.Value = false;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.IsStartWLANor3GOnShutdownEnabled.Value = false;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.AnalogInputNumber.Value = 6u;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.IsWLANor3GDownloadRingbufferEnabled.Value = true;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.WLANor3GRingbuffersToDownload.Value = 2147483647u;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.IsWLANor3GDownloadClassificationEnabled.Value = true;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.IsWLANor3GDownloadDriveRecorderEnabled.Value = true;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.IsStartWLANOnEventEnabled.Value = false;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.IsWLANDownloadRingbufferEnabled.Value = true;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.WLANRingbuffersToDownload.Value = 2147483647u;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.IsWLANDownloadClassificationEnabled.Value = true;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.IsWLANDownloadDriveRecorderEnabled.Value = true;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.IsWLANFallbackTo3GEnabled.Value = true;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.LoggerIp.Value = Constants.DefaultWlanLoggerIp;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.GatewayIp.Value = Constants.DefaultWlanGatewayIp;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.MLserverIp.Value = Constants.DefaultWlanMLserverIp;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.SubnetMask.Value = Constants.DefaultWlanSubnetMask;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.IsStartThreeGOnEventEnabled.Value = false;
			this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.PartialDownload.Value = PartialDownloadType.PartialDownloadOn;
			if (this.loggerSpecifics.Type == LoggerType.GL2000)
			{
				this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.PartialDownload.Value = PartialDownloadType.PartialDownloadOnInSameFolder;
			}
			this.vlProject.ProjectRoot.OutputConfiguration.LEDConfiguration.NumberOfLEDs = this.loggerSpecifics.IO.NumberOfLEDsTotal;
			if (this.vlProject.ProjectRoot.OutputConfiguration.LEDConfiguration.NumberOfLEDs > 0u)
			{
				this.vlProject.ProjectRoot.OutputConfiguration.LEDConfiguration.LEDConfigList[0].State.Value = LEDState.AlwaysBlinking;
			}
			this.vlProject.ProjectRoot.OutputConfiguration.DigitalOutputsConfiguration.ClearActions();
			int num10 = 0;
			while ((long)num10 < (long)((ulong)this.loggerSpecifics.IO.NumberOfDigitalInputs))
			{
				ActionDigitalOutput actionDigitalOutput = new ActionDigitalOutput();
				actionDigitalOutput.IsActive.Value = false;
				actionDigitalOutput.Event = new OnStartEvent();
				actionDigitalOutput.StopType = null;
				this.vlProject.ProjectRoot.OutputConfiguration.DigitalOutputsConfiguration.AddAction(actionDigitalOutput);
				num10++;
			}
			this.SetRuntimeProperties();
			this.vlProject.IsDirty = false;
		}

		public bool LoadFile(string fileAndPath, out ILoggerSpecifics loggerSpecificsOfFile, out bool isCopyCreated)
		{
			bool flag = false;
			string fileExtensionDotVLC = Vocabulary.FileExtensionDotVLC;
			string text = fileAndPath;
			isCopyCreated = false;
			bool flag2;
			if (string.Compare(Path.GetExtension(fileAndPath), fileExtensionDotVLC, true) == 0)
			{
				string tempDirectoryName = "";
				string text2 = "";
				if (TempDirectoryManager.Instance.CreateNewTempDirectory(out tempDirectoryName))
				{
					text2 = Path.Combine(TempDirectoryManager.Instance.GetFullTempDirectoryPath(tempDirectoryName), Path.GetFileName(fileAndPath) ?? string.Empty);
					try
					{
						File.Copy(fileAndPath, text2, true);
						File.SetAttributes(text2, FileAttributes.Normal);
						isCopyCreated = true;
					}
					catch (Exception)
					{
					}
				}
				LoggerType loggerType = LoggerType.Unknown;
				if (isCopyCreated)
				{
					loggerType = ConfigurationImporter.GetLoggerTypeOfFile(text2);
					text = text2;
				}
				if (loggerType == LoggerType.GL1000)
				{
					loggerSpecificsOfFile = LoggerSpecificsFactory.CreateLoggerSpecifics(loggerType);
					flag2 = this.ConfigurationImporter.ImportFile(text, loggerSpecificsOfFile);
					TempDirectoryManager.Instance.ReleaseTempDirectory(tempDirectoryName);
				}
				else
				{
					flag2 = false;
					loggerSpecificsOfFile = null;
					if (loggerType == LoggerType.Unknown)
					{
						InformMessageBox.Error(Resources.FileOpenError, new string[]
						{
							Path.GetFileName(text)
						});
					}
					else
					{
						InformMessageBox.Error(Resources.ImportOfUnsupportedLoggerTypeConfig, new string[]
						{
							Path.GetFileName(text)
						});
					}
				}
			}
			else
			{
				flag2 = this.vlProject.LoadProjectFile(text, out loggerSpecificsOfFile, out flag);
				if (!flag2)
				{
					loggerSpecificsOfFile = null;
					if (flag)
					{
						InformMessageBox.Error(Resources.IncompatibleFileVersion, new string[]
						{
							Path.GetFileName(text)
						});
					}
					else
					{
						InformMessageBox.Error(Resources.FileOpenError, new string[]
						{
							Path.GetFileName(text)
						});
					}
				}
			}
			if (flag2)
			{
				this.loggerSpecifics = loggerSpecificsOfFile;
				this.SetRuntimeProperties();
			}
			return flag2;
		}

		public bool SaveFile()
		{
			if (!this.vlProject.SaveProjectFile(this.VLProject.FilePath))
			{
				InformMessageBox.Error(Resources.FileSaveError, new string[]
				{
					this.VLProject.FilePath
				});
				return false;
			}
			return true;
		}

		public bool SaveFileAs(string fileNameAndPath)
		{
			if (DialogResult.OK != GenericSaveFileDialog.ShowDialog(FileType.ProjectFile, fileNameAndPath, this.loggerSpecifics.Name, this.vlProject.GetProjectFolder()))
			{
				return false;
			}
			string fileName = GenericSaveFileDialog.FileName;
			string directoryName = Path.GetDirectoryName(fileName);
			this.PropagateConfigurationFolderPath(directoryName);
			this.vlProject.ProjectRoot.ApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			if (!this.vlProject.SaveProjectFile(fileName))
			{
				InformMessageBox.Error(Resources.FileSaveError, new string[]
				{
					fileName
				});
				this.PropagateConfigurationFolderPath(this.vlProject.GetProjectFolder());
				return false;
			}
			return true;
		}

		public bool UpdateDatabaseManagers(bool checkForMissingDiagDescriptions)
		{
			bool flag = false;
			bool result = false;
			string text = "";
			using (new WaitCursor())
			{
				this.applicationDatabaseManager.CloseAllDatabases();
				this.applicationDatabaseManager.UpdateApplicationDatabaseConfiguration(this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration, this.vlProject.ProjectRoot.HardwareConfiguration.MultibusChannelConfiguration, this.vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration, this.vlProject.GetProjectFolder());
				this.diagSymbolsManager.CloseAllDatabases();
			}
			if (checkForMissingDiagDescriptions)
			{
				result = this.CheckForAndReplaceMissingDiagDescriptions();
			}
			using (new WaitCursor())
			{
				this.diagSymbolsManager.UpdateDatabaseConfiguration(this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticsDatabaseConfiguration, this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticActionsConfiguration, FileSystemServices.GetFolderPathFromFilePath(this.vlProject.FilePath));
				flag = this.ModelEditor.AutoCorrectDiagCommParams(this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticsDatabaseConfiguration.Databases, false, out text);
			}
			string str;
			bool flag2 = CcpXcpManager.Instance().SetDatabaseDefaultsForMissingEcuSettings(this.vlProject.ProjectRoot, out str);
			if (flag2)
			{
				text += str;
				flag = true;
			}
			if (flag)
			{
				DisplayReport.ShowDisplayReportDialog(Resources.ChangedConfigReport, string.Format(Resources.CorrectionsPerfOnFileName, this.vlProject.GetProjectFileName()), new RtfText(text).ToString(), true);
				result = true;
			}
			this.SetRuntimeProperties();
			return result;
		}

		private bool CheckForAndReplaceMissingDiagDescriptions()
		{
			bool result = false;
			foreach (DiagnosticsDatabase current in this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticsDatabaseConfiguration.Databases)
			{
				string absolutePath = FileSystemServices.GetAbsolutePath(current.FilePath.Value, this.vlProject.GetProjectFolder());
				if (!File.Exists(absolutePath))
				{
					string fileName = Path.GetFileName(current.FilePath.Value);
					if (InformMessageBox.Question(string.Format(Resources.DiagDescFileNotFoundReplace, fileName, FileSystemServices.GetFolderPathFromFilePath(absolutePath))) == DialogResult.Yes)
					{
						GenericOpenFileDialog.InitialDirectory = this.vlProject.GetProjectFolder();
						GenericOpenFileDialog.FileName = fileName;
						if (GenericOpenFileDialog.ShowDialog(GUIUtil.GetFileTypeFor(current.Type.Value)) == DialogResult.OK)
						{
							string value = current.FilePath.Value;
							string fileName2 = GenericOpenFileDialog.FileName;
							if (FileSystemServices.TryMakeFilePathRelativeToConfiguration(this.vlProject.GetProjectFolder(), ref fileName2))
							{
								current.FilePath.Value = fileName2;
							}
							else
							{
								current.FilePath.Value = GenericOpenFileDialog.FileName;
							}
							foreach (DiagnosticAction current2 in this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticActionsConfiguration.DiagnosticActions)
							{
								if (current2.DatabasePath.Value == value)
								{
									current2.DatabasePath.Value = current.FilePath.Value;
								}
							}
							result = true;
						}
					}
				}
			}
			return result;
		}

		public void SetRuntimeProperties()
		{
			string folderPathFromFilePath = FileSystemServices.GetFolderPathFromFilePath(this.vlProject.FilePath);
			foreach (Database current in this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.AllDescriptionFiles)
			{
				current.InitializeValidatedRuntimeProperties();
			}
			foreach (Database current2 in this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.CCPXCPDatabases)
			{
				if (current2.FileType != DatabaseFileType.A2L)
				{
					IDictionary<string, bool> dictionary;
					uint extraCPChannel;
					if (this.applicationDatabaseManager.GetDatabaseCPConfiguration(current2, folderPathFromFilePath, out dictionary, out extraCPChannel) != CPType.None)
					{
						current2.CcpXcpEcuDisplayName.Value = Database.MakeCpEcuDisplayName(dictionary.Keys);
						current2.CcpXcpIsSeedAndKeyUsed = (dictionary.Count > 0 && dictionary.First<KeyValuePair<string, bool>>().Value);
					}
					if (BusType.Bt_FlexRay == current2.BusType.Value)
					{
						current2.ExtraCPChannel = extraCPChannel;
					}
				}
			}
			foreach (DiagnosticsECU current3 in this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticsDatabaseConfiguration.ECUs)
			{
				foreach (DiagnosticsDatabase current4 in this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticsDatabaseConfiguration.Databases)
				{
					if (current4.ECUs.Contains(current3))
					{
						current3.Database = current4;
						break;
					}
				}
			}
			foreach (DiagnosticAction current5 in from a in this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticActionsConfiguration.DiagnosticActions
			where !(a is DiagnosticDummyAction)
			select a)
			{
				foreach (TriggeredDiagnosticActionSequence current6 in this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticActionsConfiguration.TriggeredActionSequences)
				{
					if (current6.Actions.Contains(current5))
					{
						current5.TriggeredDiagnosticActionSequence = current6;
						break;
					}
				}
			}
			foreach (ActionCcpXcp current7 in this.vlProject.ProjectRoot.LoggingConfiguration.CcpXcpSignalConfiguration.Actions)
			{
				foreach (CcpXcpSignal current8 in current7.Signals)
				{
					current8.ActionCcpXcp = current7;
				}
			}
			for (int i = 0; i < this.vlProject.ProjectRoot.LoggingConfiguration.FilterConfigurations.Count; i++)
			{
				this.vlProject.ProjectRoot.LoggingConfiguration.FilterConfigurations[i].MemoryNr = i + 1;
			}
			for (int j = 0; j < this.vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations.Count; j++)
			{
				this.vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[j].MemoryNr = j + 1;
				this.vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[j].SetUniqueIdsInEvents();
			}
			foreach (DataTransferTrigger current9 in this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration.ThreeGDataTransferTriggerConfiguration.DataTransferTriggers)
			{
				if (current9.Event is ReferencedRecordTriggerNameEvent)
				{
					ReferencedRecordTriggerNameEvent referencedRecordTriggerNameEvent = current9.Event as ReferencedRecordTriggerNameEvent;
					for (uint num = 1u; num <= this.loggerSpecifics.DataStorage.NumberOfMemories; num += 1u)
					{
						string.Compare(referencedRecordTriggerNameEvent.NameOfTrigger.Value, string.Format(ReferencedRecordTriggerNameEvent.WildcardTriggerNameOnMemory, num));
					}
					IList<ulong> list = this.ModelValidator.GetUniqueIdsForNameOfActiveRecordTrigger(referencedRecordTriggerNameEvent.NameOfTrigger.Value);
					if (list.Count > 0)
					{
						referencedRecordTriggerNameEvent.UniqueIdOfTrigger = list[0];
					}
					else
					{
						foreach (TriggerConfiguration current10 in this.vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations)
						{
							list = current10.GetUniqueIdsOfTriggersForName(referencedRecordTriggerNameEvent.NameOfTrigger.Value, TriggerMode.Triggered);
							if (list.Count > 0)
							{
								referencedRecordTriggerNameEvent.UniqueIdOfTrigger = list[0];
								break;
							}
						}
					}
				}
			}
			string absoluteFilePath = this.Service.GetAbsoluteFilePath(this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration.Database.Value);
			if (absoluteFilePath.Length > 0)
			{
				uint[] array;
				int longitudeLatitudeMode;
				double altitudeFactor;
				this.applicationDatabaseManager.ResolveGPSMessageSymbolsInDatabase(absoluteFilePath, out array, out longitudeLatitudeMode, out altitudeFactor);
				this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration.CANIdDateTimeAltitude.Value = array[0];
				this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration.CANIdLongitudeLatitude.Value = array[1];
				this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration.CANIdVelocityDirection.Value = array[2];
				this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration.LongitudeLatitudeMode = longitudeLatitudeMode;
				this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration.AltitudeFactor = altitudeFactor;
			}
		}

		public void PropagateConfigurationFolderPath(string newConfigFolderPath)
		{
			string projectFolder = this.vlProject.GetProjectFolder();
			foreach (IFeatureSymbolicDefinitions current in VLProject.FeatureRegistration.FeaturesUsingSymbolicDefinitions)
			{
				for (int i = 0; i < current.SymbolicMessages.Count; i++)
				{
					current.SymbolicMessages[i].DatabasePath.Value = FileSystemServices.AdjustReferencedFilePath(current.SymbolicMessages[i].DatabasePath.Value, projectFolder, newConfigFolderPath);
				}
				for (int j = 0; j < current.SymbolicSignals.Count; j++)
				{
					current.SymbolicSignals[j].DatabasePath.Value = FileSystemServices.AdjustReferencedFilePath(current.SymbolicSignals[j].DatabasePath.Value, projectFolder, newConfigFolderPath);
				}
				for (int k = 0; k < current.SymbolicDiagnosticActions.Count; k++)
				{
					current.SymbolicDiagnosticActions[k].DatabasePath.Value = FileSystemServices.AdjustReferencedFilePath(current.SymbolicDiagnosticActions[k].DatabasePath.Value, projectFolder, newConfigFolderPath);
				}
			}
			foreach (IFeatureReferencedFiles current2 in VLProject.FeatureRegistration.FeaturesUsingReferencedFiles)
			{
				for (int l = 0; l < current2.ReferencedFiles.Count; l++)
				{
					current2.ReferencedFiles[l].FilePath.Value = FileSystemServices.AdjustReferencedFilePath(current2.ReferencedFiles[l].FilePath.Value, projectFolder, newConfigFolderPath);
				}
			}
			CcpXcpManager.Instance().UpdateDatabaseFilePaths(projectFolder, newConfigFolderPath);
		}

		private static void AdjustReferencedFilePathOfSymbolicEvent(ref Event ev, string oldConfigFolderPath, string newConfigFolderPath)
		{
			if (ev is SymbolicMessageEvent)
			{
				SymbolicMessageEvent symbolicMessageEvent = ev as SymbolicMessageEvent;
				symbolicMessageEvent.DatabasePath.Value = FileSystemServices.AdjustReferencedFilePath(symbolicMessageEvent.DatabasePath.Value, oldConfigFolderPath, newConfigFolderPath);
				return;
			}
			if (ev is SymbolicSignalEvent)
			{
				SymbolicSignalEvent symbolicSignalEvent = ev as SymbolicSignalEvent;
				symbolicSignalEvent.DatabasePath.Value = FileSystemServices.AdjustReferencedFilePath(symbolicSignalEvent.DatabasePath.Value, oldConfigFolderPath, newConfigFolderPath);
				return;
			}
			if (ev is MsgTimeoutEvent)
			{
				MsgTimeoutEvent msgTimeoutEvent = ev as MsgTimeoutEvent;
				if (!string.IsNullOrEmpty(msgTimeoutEvent.DatabasePath.Value))
				{
					msgTimeoutEvent.DatabasePath.Value = FileSystemServices.AdjustReferencedFilePath(msgTimeoutEvent.DatabasePath.Value, oldConfigFolderPath, newConfigFolderPath);
					return;
				}
			}
			else if (ev is CombinedEvent)
			{
				CombinedEvent combinedEvent = ev as CombinedEvent;
				for (int i = 0; i < combinedEvent.Count; i++)
				{
					Event @event = combinedEvent[i];
					ConfigurationManager.AdjustReferencedFilePathOfSymbolicEvent(ref @event, oldConfigFolderPath, newConfigFolderPath);
				}
			}
		}

		void IConfigurationManagerService.DataModelHasChanged(Feature changedFeature)
		{
			if (this.dataModelChanged != null)
			{
				this.dataModelChanged(changedFeature);
			}
		}

		uint IConfigurationManagerService.GetFirstActiveOrDefaultChannel(BusType busType)
		{
			switch (busType)
			{
			case BusType.Bt_CAN:
			{
				uint num = 1u;
				while ((ulong)num <= (ulong)((long)this.CANChannelConfiguration.CANChannels.Count))
				{
					if (this.CANChannelConfiguration.GetCANChannel(num).IsActive.Value)
					{
						return num;
					}
					num += 1u;
				}
				foreach (KeyValuePair<uint, CANChannel> current in this.MultibusChannelConfiguration.CANChannels)
				{
					if (current.Value.IsActive.Value)
					{
						uint key = current.Key;
						return key;
					}
				}
				return 1u;
			}
			case BusType.Bt_LIN:
			{
				uint num2 = 1u;
				while ((ulong)num2 <= (ulong)((long)this.LINChannelConfiguration.LINChannels.Count))
				{
					if (this.LINChannelConfiguration.GetLINChannel(num2).IsActive.Value)
					{
						return num2;
					}
					num2 += 1u;
				}
				foreach (KeyValuePair<uint, LINChannel> current2 in this.MultibusChannelConfiguration.LINChannels)
				{
					if (current2.Value.IsActive.Value)
					{
						uint key = current2.Key;
						return key;
					}
				}
				return 1u;
			}
			case BusType.Bt_FlexRay:
			{
				uint num3 = 1u;
				while ((ulong)num3 >= (ulong)((long)this.FlexrayChannelConfiguration.FlexrayChannels.Count))
				{
					if (this.FlexrayChannelConfiguration.GetFlexrayChannel(num3).IsActive.Value)
					{
						return num3;
					}
					num3 += 1u;
				}
				return 1u;
			}
			case BusType.Bt_Ethernet:
				return 1u;
			}
			return 1u;
		}

		uint IConfigurationManagerService.GetTotalNumberOfLogicalChannels(BusType busType)
		{
			switch (busType)
			{
			case BusType.Bt_CAN:
				return Math.Max(this.LoggerSpecifics.CAN.NumberOfChannels, this.LoggerSpecifics.Multibus.NumberOfChannels);
			case BusType.Bt_LIN:
			{
				uint num = Math.Max(this.LoggerSpecifics.LIN.NumberOfChannels, this.LoggerSpecifics.Multibus.NumberOfPiggyConfigurableChannels);
				if (this.LINChannelConfiguration.IsUsingLinProbe.Value)
				{
					num += this.LoggerSpecifics.LIN.NumberOfLINprobeChannels;
				}
				return num;
			}
			case BusType.Bt_FlexRay:
				return this.LoggerSpecifics.Flexray.NumberOfChannels;
			case BusType.Bt_J1708:
				return this.LoggerSpecifics.Multibus.NumberOfPiggyConfigurableChannels;
			default:
				return 0u;
			}
		}

		bool IConfigurationManagerService.IsHardwareChannelAvailable(BusType busType, uint channelNr)
		{
			switch (busType)
			{
			case BusType.Bt_CAN:
				if (this.CANChannelConfiguration.CANChannels.Count > 0)
				{
					return (ulong)channelNr <= (ulong)((long)this.CANChannelConfiguration.CANChannels.Count);
				}
				return this.MultibusChannelConfiguration.NumberOfChannels > 0u && this.MultibusChannelConfiguration.CANChannels.ContainsKey(channelNr);
			case BusType.Bt_LIN:
				if (this.LINChannelConfiguration.LINChannels.Count <= 0)
				{
					return this.MultibusChannelConfiguration.NumberOfChannels > 0u && this.MultibusChannelConfiguration.LINChannels.ContainsKey(channelNr);
				}
				if (this.LINChannelConfiguration.IsUsingLinProbe.Value)
				{
					return (ulong)channelNr <= (ulong)((long)this.LINChannelConfiguration.LINChannels.Count + (long)((ulong)this.LoggerSpecifics.LIN.NumberOfLINprobeChannels));
				}
				return (ulong)channelNr <= (ulong)((long)this.LINChannelConfiguration.LINChannels.Count);
			case BusType.Bt_FlexRay:
				return this.FlexrayChannelConfiguration.FlexrayChannels.Count > 0 && (ulong)channelNr <= (ulong)((long)this.FlexrayChannelConfiguration.FlexrayChannels.Count);
			case BusType.Bt_J1708:
				return this.MultibusChannelConfiguration.NumberOfChannels > 0u && this.MultibusChannelConfiguration.J1708Channels.ContainsKey(channelNr);
			default:
				return false;
			}
		}

		bool IConfigurationManagerService.IsHardwareChannelActive(BusType busType, uint channelNr)
		{
			switch (busType)
			{
			case BusType.Bt_CAN:
				if (this.CANChannelConfiguration.CANChannels.Count > 0)
				{
					return this.CANChannelConfiguration.GetActiveCANChannelNumbers().Contains(channelNr);
				}
				return this.MultibusChannelConfiguration.NumberOfChannels > 0u && this.MultibusChannelConfiguration.CANChannels.ContainsKey(channelNr) && this.MultibusChannelConfiguration.CANChannels[channelNr].IsActive.Value;
			case BusType.Bt_LIN:
				if (this.LINChannelConfiguration.LINChannels.Count <= 0)
				{
					return this.MultibusChannelConfiguration.NumberOfChannels > 0u && this.MultibusChannelConfiguration.LINChannels.ContainsKey(channelNr) && this.MultibusChannelConfiguration.LINChannels[channelNr].IsActive.Value;
				}
				if ((ulong)channelNr <= (ulong)((long)this.LINChannelConfiguration.LINChannels.Count))
				{
					return this.LINChannelConfiguration.GetActiveLINChannelNumbers().Contains(channelNr);
				}
				return channelNr <= this.LoggerSpecifics.LIN.NumberOfChannels + this.LoggerSpecifics.LIN.NumberOfLINprobeChannels;
			case BusType.Bt_FlexRay:
				return this.FlexrayChannelConfiguration.FlexrayChannels.Count > 0 && this.FlexrayChannelConfiguration.GetActiveFlexrayChannelNumbers().Contains(channelNr);
			case BusType.Bt_J1708:
				return this.MultibusChannelConfiguration.NumberOfChannels > 0u && this.MultibusChannelConfiguration.J1708Channels.ContainsKey(channelNr) && this.MultibusChannelConfiguration.J1708Channels[channelNr].IsActive.Value;
			default:
				return false;
			}
		}

		HardwareChannel IConfigurationManagerService.GetHardwareChannel(BusType busType, uint channelNr)
		{
			switch (busType)
			{
			case BusType.Bt_CAN:
				if ((ulong)channelNr <= (ulong)((long)this.CANChannelConfiguration.CANChannels.Count))
				{
					return this.CANChannelConfiguration.GetCANChannel(channelNr);
				}
				if (this.MultibusChannelConfiguration.NumberOfChannels > 0u && this.MultibusChannelConfiguration.CANChannels.ContainsKey(channelNr))
				{
					return this.MultibusChannelConfiguration.CANChannels[channelNr];
				}
				return null;
			case BusType.Bt_LIN:
				if ((ulong)channelNr <= (ulong)((long)this.LINChannelConfiguration.LINChannels.Count))
				{
					return this.LINChannelConfiguration.GetLINChannel(channelNr);
				}
				if (this.MultibusChannelConfiguration.NumberOfChannels > 0u && this.MultibusChannelConfiguration.LINChannels.ContainsKey(channelNr))
				{
					return this.MultibusChannelConfiguration.LINChannels[channelNr];
				}
				return null;
			case BusType.Bt_FlexRay:
				if ((ulong)channelNr <= (ulong)((long)this.FlexrayChannelConfiguration.FlexrayChannels.Count))
				{
					return this.FlexrayChannelConfiguration.GetFlexrayChannel(channelNr);
				}
				return null;
			case BusType.Bt_J1708:
				if (this.MultibusChannelConfiguration.NumberOfChannels > 0u && this.MultibusChannelConfiguration.J1708Channels.ContainsKey(channelNr))
				{
					return this.MultibusChannelConfiguration.J1708Channels[channelNr];
				}
				return null;
			default:
				return null;
			}
		}

		bool IConfigurationManagerService.IsCANChannelFDModeActive(uint canChannelNr)
		{
			if (this.CANChannelConfiguration.CANChannels.Count > 0 && (ulong)canChannelNr <= (ulong)((long)this.CANChannelConfiguration.CANChannels.Count))
			{
				CANChannel cANChannel = this.CANChannelConfiguration.GetCANChannel(canChannelNr);
				return cANChannel.CANChipConfiguration.IsCANFD;
			}
			return this.MultibusChannelConfiguration.NumberOfChannels > 0u && this.MultibusChannelConfiguration.CANChannels.ContainsKey(canChannelNr) && this.MultibusChannelConfiguration.CANChannels[canChannelNr].CANChipConfiguration.IsCANFD;
		}

		string IConfigurationManagerService.GetFilePathRelativeToConfiguration(string absFilePath)
		{
			string result = absFilePath;
			if (FileSystemServices.TryMakeFilePathRelativeToConfiguration(this.vlProject.GetProjectFolder(), ref absFilePath))
			{
				result = absFilePath;
			}
			return result;
		}

		string IConfigurationManagerService.GetAbsoluteFilePath(string pathRelativeToConfiguration)
		{
			return FileSystemServices.GetAbsolutePath(pathRelativeToConfiguration, this.vlProject.GetProjectFolder());
		}

		void IConfigurationManagerService.NotifyAllDependentFeatures(Feature changedFeature)
		{
			foreach (Feature current in ((IConfigurationManagerService)this).FeatureRegistration.Features)
			{
				current.UpdateOnDependency(changedFeature, ((IConfigurationManagerService)this).UpdateService);
			}
			((IConfigurationManagerService)this).DataModelHasChanged(changedFeature);
		}
	}
}
