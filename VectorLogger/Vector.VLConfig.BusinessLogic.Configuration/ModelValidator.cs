using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Vector.McLoggerDatabaseGenerator;
using Vector.McModule;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GUI;
using Vector.VLConfig.GUI.IncludeFilesPage;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public class ModelValidator : IModelValidator
	{
		private static readonly char[] invalidTriggerCommentChars = new char[]
		{
			'{',
			'}'
		};

		private static readonly char[] invalidIncludeParameterChars = new char[]
		{
			'"'
		};

		private static readonly string triggerNameWhitelistValidCharacterList = "a-z A-Z 0-9 - / _ , . : ; < > = ! + & ( ) [ ]";

		private static readonly string triggerNameRegexWhitelistString = ModelValidator.CreateWhitelistRegexFromString(ModelValidator.triggerNameWhitelistValidCharacterList, true);

		private static readonly Regex triggerNameRegexWhitelist = new Regex(ModelValidator.triggerNameRegexWhitelistString, RegexOptions.Compiled);

		private static readonly string caplCompliantCharactersList = "a-z A-Z 0-9 _";

		private static readonly string caplCompliantRegexWhitelistString = ModelValidator.CreateWhitelistRegexFromString(ModelValidator.caplCompliantCharactersList, false);

		public static readonly Regex CaplCompliantWhitelist = new Regex(ModelValidator.caplCompliantRegexWhitelistString);

		private IConfigurationManagerService configManager;

		private List<int> activeMemories;

		private MultibusChannelConfiguration MultibusChannelConfiguration
		{
			get
			{
				return this.configManager.MultibusChannelConfiguration;
			}
		}

		private CANChannelConfiguration CANChannelConfiguration
		{
			get
			{
				return this.configManager.CANChannelConfiguration;
			}
		}

		private LINChannelConfiguration LINChannelConfiguration
		{
			get
			{
				return this.configManager.LINChannelConfiguration;
			}
		}

		private FlexrayChannelConfiguration FlexrayChannelConfiguration
		{
			get
			{
				return this.configManager.FlexrayChannelConfiguration;
			}
		}

		private MOST150ChannelConfiguration MOST150ChannelConfiguration
		{
			get
			{
				return this.configManager.MOST150ChannelConfiguration;
			}
		}

		private LogDataStorage LogDataStorage
		{
			get
			{
				return this.configManager.LogDataStorage;
			}
		}

		private DatabaseConfiguration DatabaseConfiguration
		{
			get
			{
				return this.configManager.DatabaseConfiguration;
			}
		}

		private DiagnosticsDatabaseConfiguration DiagnosticsDatabaseConfiguration
		{
			get
			{
				return this.configManager.DiagnosticsDatabaseConfiguration;
			}
		}

		private DiagnosticActionsConfiguration DiagnosticActionsConfiguration
		{
			get
			{
				return this.configManager.DiagnosticActionsConfiguration;
			}
		}

		private IList<FilterConfiguration> FilterConfigurations
		{
			get
			{
				return this.configManager.FilterConfigurations;
			}
		}

		private IList<TriggerConfiguration> TriggerConfigurations
		{
			get
			{
				return this.configManager.TriggerConfigurations;
			}
		}

		private SendMessageConfiguration SendMessageConfiguration
		{
			get
			{
				return this.configManager.SendMessageConfiguration;
			}
		}

		private DigitalOutputsConfiguration DigitalOutputsConfiguration
		{
			get
			{
				return this.configManager.DigitalOutputsConfiguration;
			}
		}

		private DigitalInputConfiguration DigitalInputConfiguration
		{
			get
			{
				return this.configManager.DigitalInputConfiguration;
			}
		}

		private SpecialFeaturesConfiguration SpecialFeaturesConfiguration
		{
			get
			{
				return this.configManager.SpecialFeaturesConfiguration;
			}
		}

		private InterfaceModeConfiguration InterfaceModeConfiguration
		{
			get
			{
				return this.configManager.InterfaceModeConfiguration;
			}
		}

		private AnalogInputConfiguration AnalogInputConfiguration
		{
			get
			{
				return this.configManager.AnalogInputConfiguration;
			}
		}

		private IncludeFileConfiguration IncludeFileConfiguration
		{
			get
			{
				return this.configManager.IncludeFileConfiguration;
			}
		}

		private GPSConfiguration GPSConfiguration
		{
			get
			{
				return this.configManager.GPSConfiguration;
			}
		}

		private WLANConfiguration WLANConfiguration
		{
			get
			{
				return this.configManager.WLANConfiguration;
			}
		}

		private EthernetConfiguration EthernetConfiguration
		{
			get
			{
				return this.configManager.EthernetConfiguration;
			}
		}

		private LEDConfiguration LEDConfiguration
		{
			get
			{
				return this.configManager.LEDConfiguration;
			}
		}

		private IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.configManager.ApplicationDatabaseManager;
			}
		}

		private IDiagSymbolsManager DiagSymbolsManager
		{
			get
			{
				return this.configManager.DiagSymbolsManager;
			}
		}

		private string ConfigFolderPath
		{
			get
			{
				return this.configManager.ConfigFolderPath;
			}
		}

		private CcpXcpSignalConfiguration CcpXcpSignalConfiguration
		{
			get
			{
				return this.configManager.CcpXcpSignalConfiguration;
			}
		}

		public ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return this.configManager.LoggerSpecifics;
			}
			set
			{
				this.configManager.LoggerSpecifics = value;
			}
		}

		IDatabaseServices IModelValidator.DatabaseServices
		{
			get
			{
				return this.configManager.DatabaseServices;
			}
		}

		IList<int> IModelValidator.GetActiveMemoryNumbers
		{
			get
			{
				this.activeMemories.Clear();
				foreach (TriggerConfiguration current in this.TriggerConfigurations)
				{
					if (current.MemoryRingBuffer.IsActive.Value)
					{
						this.activeMemories.Add(current.MemoryNr);
					}
				}
				return this.activeMemories;
			}
		}

		public ModelValidator(IConfigurationManagerService configMan)
		{
			this.configManager = configMan;
			this.activeMemories = new List<int>();
		}

		string IModelValidator.GetFilePathRelativeToConfiguration(string absFilePath)
		{
			return this.configManager.GetFilePathRelativeToConfiguration(absFilePath);
		}

		string IModelValidator.GetAbsoluteFilePath(string pathRelativeToConfiguration)
		{
			return this.configManager.GetAbsoluteFilePath(pathRelativeToConfiguration);
		}

		uint IModelValidator.GetFirstActiveOrDefaultChannel(BusType busType)
		{
			return this.configManager.GetFirstActiveOrDefaultChannel(busType);
		}

		public uint GetTotalNumberOfLogicalChannels(BusType busType)
		{
			return this.configManager.GetTotalNumberOfLogicalChannels(busType);
		}

		public bool IsHardwareChannelAvailable(BusType busType, uint channelNr)
		{
			return this.configManager.IsHardwareChannelAvailable(busType, channelNr);
		}

		public bool IsHardwareChannelActive(BusType busType, uint channelNr)
		{
			return this.configManager.IsHardwareChannelActive(busType, channelNr);
		}

		public HardwareChannel GetHardwareChannel(BusType busType, uint channelNr)
		{
			return this.configManager.GetHardwareChannel(busType, channelNr);
		}

		public bool IsCANChannelFDModeActive(uint canChannelNr)
		{
			return this.configManager.IsCANChannelFDModeActive(canChannelNr);
		}

		bool IModelValidator.IsActiveVoCANEventConfigured(out int numberOfConfiguredActiveVoCANEvents, out bool isLed8Used)
		{
			numberOfConfiguredActiveVoCANEvents = 0;
			isLed8Used = false;
			foreach (TriggerConfiguration current in this.TriggerConfigurations)
			{
				if (current.MemoryRingBuffer.IsActive.Value)
				{
					switch (current.TriggerMode.Value)
					{
					case TriggerMode.Triggered:
						using (IEnumerator<VoCanRecordingEvent> enumerator2 = current.GetActiveVoCanRecordingEventsInTriggers.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								VoCanRecordingEvent current2 = enumerator2.Current;
								numberOfConfiguredActiveVoCANEvents++;
								if (current2.IsRecordingLEDActive.Value)
								{
									isLed8Used = true;
								}
							}
							continue;
						}
						break;
					case TriggerMode.Permanent:
						goto IL_F5;
					case TriggerMode.OnOff:
						break;
					default:
						continue;
					}
					using (IEnumerator<VoCanRecordingEvent> enumerator3 = current.GetActiveVoCanRecordingEventsInOnOffTriggers.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							VoCanRecordingEvent current3 = enumerator3.Current;
							numberOfConfiguredActiveVoCANEvents++;
							if (current3.IsRecordingLEDActive.Value)
							{
								isLed8Used = true;
							}
						}
						continue;
					}
					IL_F5:
					foreach (VoCanRecordingEvent current4 in current.GetActiveVoCanRecordingEventsInPermanentMarkers)
					{
						numberOfConfiguredActiveVoCANEvents++;
						if (current4.IsRecordingLEDActive.Value)
						{
							isLed8Used = true;
						}
					}
				}
			}
			numberOfConfiguredActiveVoCANEvents += this.SendMessageConfiguration.ActiveVoCanActions.Count;
			foreach (ActionSendMessage current5 in this.SendMessageConfiguration.ActiveVoCanActions)
			{
				if (current5.Event is VoCanRecordingEvent && (current5.Event as VoCanRecordingEvent).IsRecordingLEDActive.Value)
				{
					isLed8Used = true;
					break;
				}
			}
			return numberOfConfiguredActiveVoCANEvents > 0;
		}

		bool IModelValidator.IsActiveCasKeyEventConfigured(out int numberOfActiveCasKeyEvents)
		{
			numberOfActiveCasKeyEvents = 0;
			foreach (TriggerConfiguration current in this.TriggerConfigurations)
			{
				if (current.MemoryRingBuffer.IsActive.Value)
				{
					switch (current.TriggerMode.Value)
					{
					case TriggerMode.Triggered:
						numberOfActiveCasKeyEvents += current.GetNumOfActiveCasKeyEventsInTriggers;
						break;
					case TriggerMode.Permanent:
						numberOfActiveCasKeyEvents += current.GetNumOfActiveCasKeyEventsInPermanentMarkers;
						break;
					case TriggerMode.OnOff:
						numberOfActiveCasKeyEvents += current.GetNumOfActiveCasKeyEventsInOnOffTriggers;
						break;
					}
				}
			}
			numberOfActiveCasKeyEvents += this.SendMessageConfiguration.ActiveCasKeyActions.Count;
			numberOfActiveCasKeyEvents += this.DigitalOutputsConfiguration.ActiveCasKeyActions.Count;
			numberOfActiveCasKeyEvents += this.DiagnosticActionsConfiguration.CasKeySequences.Count;
			numberOfActiveCasKeyEvents += this.WLANConfiguration.ActiveCasKeyEvents.Count;
			return numberOfActiveCasKeyEvents > 0;
		}

		bool IModelValidator.Validate(Feature feature, bool isDataChanged, IModelValidationResultCollector resultCollector)
		{
			return ((IModelValidator)this).Validate(feature, PageType._None_, isDataChanged, resultCollector);
		}

		bool IModelValidator.Validate(Feature feature, PageType pageContext, bool isDataChanged, IModelValidationResultCollector resultCollector)
		{
			resultCollector.ResetAllModelErrors();
			bool flag = true;
			flag &= this.ValidateGenericFeature(feature, pageContext, isDataChanged, resultCollector);
			flag &= this.ValidateSpecificFeature(feature, pageContext, resultCollector);
			if (isDataChanged)
			{
				this.configManager.NotifyAllDependentFeatures(feature);
			}
			return flag;
		}

		private bool ValidateGenericFeature(Feature feature, PageType pageContext, bool isDataChanged, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			flag &= this.ValidateGenericFeatureUsingTransmitMessages(feature, pageContext, resultCollector);
			flag &= this.ValidateGenericFeatureUsingSymbolicDefinitions(feature, resultCollector);
			return flag & this.ValidateGenericFeatureUsingVirtualLogMessages(feature, isDataChanged, resultCollector);
		}

		private bool ValidateGenericFeatureUsingTransmitMessages(Feature feature, PageType pageContext, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (feature is IFeatureTransmitMessages)
			{
				if (feature is DatabaseConfiguration && pageContext != PageType.CcpXcpDescriptions)
				{
					return result;
				}
				IList<ITransmitMessageChannel> activeTransmitMessageChannels = (feature as IFeatureTransmitMessages).ActiveTransmitMessageChannels;
				foreach (ITransmitMessageChannel current in activeTransmitMessageChannels)
				{
					if (current.BusType.Value == BusType.Bt_CAN)
					{
						HardwareChannel hardwareChannel = this.configManager.GetHardwareChannel(BusType.Bt_CAN, current.ChannelNumber.Value);
						if (hardwareChannel is CANChannel)
						{
							CANChannel cANChannel = hardwareChannel as CANChannel;
							if (cANChannel.IsActive.Value && !cANChannel.IsOutputActive.Value)
							{
								resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.ChannelNumber, Resources.ErrorChannelOutputDisabled);
								result = false;
							}
						}
					}
				}
			}
			return result;
		}

		private bool ValidateGenericFeatureUsingSymbolicDefinitions(Feature feature, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (this.configManager.LoggerSpecifics.Configuration.CompilerType == EnumCompilerType.CAPL && feature is IFeatureSymbolicDefinitions)
			{
				foreach (ISymbolicMessage current in (feature as IFeatureSymbolicDefinitions).SymbolicMessages)
				{
					if (this.configManager.DatabaseServices.IsCaplKeyword(current.MessageName.Value))
					{
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current.MessageName, string.Format(Resources.ErrorSymMsgIllegal, current.MessageName.Value));
						result = false;
					}
					if (this.configManager.DatabaseServices.IsCaplKeyword(current.NetworkName.Value))
					{
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current.NetworkName, string.Format(Resources.ErrorSymNetworkIllegal, current.NetworkName.Value));
						result = false;
					}
				}
				foreach (ISymbolicSignal current2 in (feature as IFeatureSymbolicDefinitions).SymbolicSignals)
				{
					if (this.configManager.DatabaseServices.IsCaplKeyword(current2.SignalName.Value))
					{
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current2.SignalName, string.Format(Resources.ErrorSymSigIllegal, current2.SignalName.Value));
						result = false;
					}
					if (this.configManager.DatabaseServices.IsCaplKeyword(current2.MessageName.Value))
					{
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current2.MessageName, string.Format(Resources.ErrorSymMsgIllegal, current2.MessageName.Value));
						result = false;
					}
					if (this.configManager.DatabaseServices.IsCaplKeyword(current2.NetworkName.Value))
					{
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current2.NetworkName, string.Format(Resources.ErrorSymNetworkIllegal, current2.NetworkName.Value));
						result = false;
					}
				}
			}
			return result;
		}

		private bool ValidateGenericFeatureUsingVirtualLogMessages(Feature featureToValidate, bool isDataChanged, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			if (featureToValidate is IFeatureVirtualLogMessages)
			{
				IFeatureVirtualLogMessages featureVirtualLogMessages = featureToValidate as IFeatureVirtualLogMessages;
				foreach (IFeatureVirtualLogMessages current in this.configManager.FeatureRegistration.FeaturesUsingVirtualLogMessages)
				{
					if (!UsedIdsManager.AreVirtualLogMsgIdsUpdated(current) || current == featureVirtualLogMessages)
					{
						UsedIdsManager.UpdateVirtualLogMsgIds(current);
					}
				}
				flag &= UsedIdsManager.ValidateVirtualLogMsgs(featureVirtualLogMessages, resultCollector);
				if (isDataChanged)
				{
					foreach (Feature current2 in this.configManager.FeatureRegistration.Features)
					{
						if (current2 is IFeatureVirtualLogMessages)
						{
							current2.Update(this.configManager.UpdateService);
						}
					}
				}
			}
			return flag;
		}

		private bool ValidateSpecificFeature(Feature feature, PageType pageContext, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			if (feature is CANChannelConfiguration)
			{
				flag &= this.ValidateCANChannelConfiguration(feature as CANChannelConfiguration, resultCollector);
			}
			else if (feature is LINChannelConfiguration)
			{
				flag &= this.ValidateLINChannelConfiguration(feature as LINChannelConfiguration, resultCollector);
			}
			else if (feature is FlexrayChannelConfiguration)
			{
				flag = flag;
			}
			else if (feature is MultibusChannelConfiguration)
			{
				flag &= this.ValidateMultibusChannelConfiguration(feature as MultibusChannelConfiguration, resultCollector);
			}
			else if (feature is LogDataStorage)
			{
				flag &= this.ValidateLogDataStorage(feature as LogDataStorage, this.DatabaseConfiguration, this.ConfigFolderPath, this.ApplicationDatabaseManager, this.DigitalOutputsConfiguration, resultCollector);
			}
			else if (feature is MOST150ChannelConfiguration)
			{
				flag &= this.ValidateMOST150ChannelConfiguration(feature as MOST150ChannelConfiguration, resultCollector);
			}
			else if (feature is CommentConfiguration)
			{
				flag = flag;
			}
			else if (feature is DatabaseConfiguration)
			{
				if (pageContext == PageType.CcpXcpDescriptions)
				{
					flag &= this.ValidateCCPXCPConfiguration(feature as DatabaseConfiguration, this.MultibusChannelConfiguration, this.CANChannelConfiguration, this.FlexrayChannelConfiguration, this.MOST150ChannelConfiguration, this.EthernetConfiguration, this.ConfigFolderPath, this.ApplicationDatabaseManager, resultCollector);
				}
				else
				{
					flag &= this.ValidateDatabaseConfiguration(feature as DatabaseConfiguration, this.MultibusChannelConfiguration, this.CANChannelConfiguration, this.LINChannelConfiguration, this.FlexrayChannelConfiguration, this.ConfigFolderPath, resultCollector);
				}
			}
			else if (feature is ExportDatabaseConfiguration)
			{
				flag &= this.ValidateExportDatabaseConfiguration(feature as ExportDatabaseConfiguration, this.ConfigFolderPath, resultCollector);
			}
			else if (feature is CcpXcpSignalConfiguration)
			{
				flag &= this.ValidateCCPXCPSignalConfiguration(feature as CcpXcpSignalConfiguration, this.DatabaseConfiguration, this.ConfigFolderPath, this.ApplicationDatabaseManager, this.DigitalOutputsConfiguration, resultCollector);
			}
			else if (feature is DiagnosticsDatabaseConfiguration)
			{
				flag &= this.ValidateDiagnosticsDatabaseConfiguration(feature as DiagnosticsDatabaseConfiguration, this.CANChannelConfiguration, this.ConfigFolderPath, this.DiagSymbolsManager, resultCollector);
			}
			else if (feature is DiagnosticActionsConfiguration)
			{
				flag &= this.ValidateDiagnosticActionsConfiguration(feature as DiagnosticActionsConfiguration, this.DiagnosticsDatabaseConfiguration, this.DatabaseConfiguration, this.ConfigFolderPath, this.DiagSymbolsManager, this.ApplicationDatabaseManager, this.DigitalOutputsConfiguration, resultCollector);
			}
			else if (feature is FilterConfiguration)
			{
				flag &= this.ValidateFilterConfiguration(feature as FilterConfiguration, this.DatabaseConfiguration, this.ConfigFolderPath, this.ApplicationDatabaseManager, resultCollector);
			}
			else if (feature is TriggerConfiguration)
			{
				flag &= this.ValidateTriggerList(feature as TriggerConfiguration, this.DatabaseConfiguration, this.ConfigFolderPath, this.ApplicationDatabaseManager, this.DigitalOutputsConfiguration, this.SendMessageConfiguration, resultCollector);
			}
			else if (feature is SendMessageConfiguration)
			{
				flag &= this.ValidateSendMessageConfiguration(feature as SendMessageConfiguration, this.DatabaseConfiguration, this.ConfigFolderPath, this.ApplicationDatabaseManager, this.DigitalOutputsConfiguration, resultCollector);
			}
			else if (feature is DigitalOutputsConfiguration)
			{
				flag &= this.ValidateDigitalOutputsConfiguration(feature as DigitalOutputsConfiguration, this.DigitalInputConfiguration, this.DatabaseConfiguration, this.ConfigFolderPath, this.ApplicationDatabaseManager, this.TriggerConfigurations, this.DiagnosticActionsConfiguration, this.SendMessageConfiguration, this.LogDataStorage, resultCollector);
			}
			else if (feature is SpecialFeaturesConfiguration)
			{
				if (this.configManager.LoggerSpecifics.Type == LoggerType.GL3000 || this.configManager.LoggerSpecifics.Type == LoggerType.GL4000)
				{
					flag &= this.ValidateSpecialFeaturesGL3Plus(feature as SpecialFeaturesConfiguration, this.CANChannelConfiguration, resultCollector);
				}
				else if (this.configManager.LoggerSpecifics.Type == LoggerType.VN1630log)
				{
					flag &= this.ValidateSpecialFeaturesVN1630log(feature as SpecialFeaturesConfiguration, resultCollector);
				}
				else
				{
					flag &= this.ValidateSpecialFeatures(feature as SpecialFeaturesConfiguration, this.CANChannelConfiguration, resultCollector);
				}
			}
			else if (feature is InterfaceModeConfiguration)
			{
				if (pageContext == PageType.InterfaceMode_SignalExport)
				{
					flag &= this.ValidateSignalExportInterfaceMode(feature as InterfaceModeConfiguration, this.CANChannelConfiguration, resultCollector);
				}
				else
				{
					flag &= this.ValidateInterfaceMode(feature as InterfaceModeConfiguration, this.CANChannelConfiguration, resultCollector);
				}
			}
			else if (feature is AnalogInputConfiguration)
			{
				flag &= this.ValidateAnalogInputConfiguration(feature as AnalogInputConfiguration, this.CANChannelConfiguration, resultCollector);
			}
			else if (feature is DigitalInputConfiguration)
			{
				flag &= this.ValidateDigitalInputConfiguration(feature as DigitalInputConfiguration, this.DigitalOutputsConfiguration, this.CANChannelConfiguration, resultCollector);
			}
			else if (feature is IncludeFileConfiguration)
			{
				flag &= this.ValidateIncludeFileConfiguration(feature as IncludeFileConfiguration, this.ConfigFolderPath, resultCollector);
			}
			else if (feature is GPSConfiguration)
			{
				flag &= this.ValidateGPS(feature as GPSConfiguration, this.CANChannelConfiguration, resultCollector);
			}
			else if (feature is WLANConfiguration)
			{
				flag &= this.ValidateWLANSettings(feature as WLANConfiguration, this.LogDataStorage, resultCollector);
			}
			else if (feature is LEDConfiguration)
			{
				flag &= this.ValidateLedConfiguration(feature as LEDConfiguration, resultCollector);
			}
			return flag;
		}

		bool IModelValidator.IsPrescalerValueOfCANChannelValid(uint channelNr, CANChannelConfiguration canChannelConfig)
		{
			CANChannel cANChannel = canChannelConfig.GetCANChannel(channelNr);
			if (cANChannel.CANChipConfiguration.IsCANFD)
			{
				return true;
			}
			CANStdChipConfiguration cANStdChipConfiguration = cANChannel.CANChipConfiguration as CANStdChipConfiguration;
			if (cANStdChipConfiguration == null)
			{
				return false;
			}
			if (this.LoggerSpecifics.CAN.AuxChannelMaxPrescalerValue > 0u && channelNr == this.LoggerSpecifics.CAN.AuxChannel)
			{
				if (cANStdChipConfiguration.Prescaler.Value > this.LoggerSpecifics.CAN.AuxChannelMaxPrescalerValue)
				{
					return false;
				}
			}
			else if (this.LoggerSpecifics.CAN.MaxPrescalerValue > 0u && cANStdChipConfiguration.Prescaler.Value > this.LoggerSpecifics.CAN.MaxPrescalerValue)
			{
				return false;
			}
			return true;
		}

		uint IModelValidator.GetMaxPrescalerValue(uint channelNr)
		{
			if (channelNr == this.LoggerSpecifics.CAN.AuxChannel)
			{
				return this.LoggerSpecifics.CAN.AuxChannelMaxPrescalerValue;
			}
			return this.LoggerSpecifics.CAN.MaxPrescalerValue;
		}

		uint IModelValidator.GetEventActivationDelayAfterStart()
		{
			return this.LogDataStorage.EventActivationDelayAfterStart.Value;
		}

		bool IModelValidator.ValidateCcpXcpDatabaseSettings(Database database, EthernetConfiguration ethernetConfiguration, IModelValidationResultCollector resultCollector)
		{
			return this.ValidateCcpXcpDatabaseSettings(database, ethernetConfiguration, resultCollector);
		}

		bool IModelValidator.ValidateCcpXcpEcuIdSettings(Database database, IModelValidationResultCollector resultCollector)
		{
			return this.ValidateCcpXcpEcuIdSettings(database, this.DatabaseConfiguration, this.configManager.CcpXcpSignalConfiguration, this.CANChannelConfiguration, resultCollector);
		}

		bool IModelValidator.ValidateDiagnosticActionsGeneralSettings(DiagnosticActionsConfiguration actionsConfig, bool isDataChanged, IModelValidationResultCollector resultCollector)
		{
			bool result = this.ValidateDiagnosticActionsGeneralSettings(actionsConfig, resultCollector);
			if (isDataChanged)
			{
				this.configManager.NotifyAllDependentFeatures(actionsConfig);
			}
			return result;
		}

		bool IModelValidator.ValidateIndependentDiagCommParamsEcu(DiagnosticCommParamsECU commParamsEcu, IModelValidationResultCollector resultCollector)
		{
			return this.ValidateIndependentDiagCommParamsEcu(commParamsEcu, resultCollector);
		}

		bool IModelValidator.ValidateOverallCommParamsSessionSettings(IDiagnosticsDatabaseConfiguration config, IModelValidationResultCollector resultCollector, bool useSingleErrorPerEcu)
		{
			return this.ValidateOverallCommParamsSessionSettings(config, resultCollector, useSingleErrorPerEcu);
		}

		bool IModelValidator.ValidateOverallCommParamsRequestResponseIds(IDiagnosticsDatabaseConfiguration config, IModelValidationResultCollector resultCollector, bool useSingleErrorPerEcu)
		{
			return this.ValidateOverallCommParamsRequestResponseIds(config, resultCollector, useSingleErrorPerEcu);
		}

		List<string> IModelValidator.GetUndefinedEcuQualifiersOfDiagDatabase(DiagnosticsDatabase database)
		{
			return this.GetUndefinedEcuQualifiersOfDiagDatabase(database, this.ConfigFolderPath, this.DiagSymbolsManager);
		}

		bool IModelValidator.ValidateDatabaseFileExistence(DiagnosticsDatabase db, IModelValidationResultCollector resultCollector)
		{
			return this.ValidateDatabaseFileExistence(db, this.ConfigFolderPath, resultCollector);
		}

		bool IModelValidator.ValidateDatabaseConsistency(Database db, IModelValidationResultCollector resultCollector)
		{
			return this.ValidateDatabaseConsistency(db, resultCollector);
		}

		bool IModelValidator.HasDiagnosticsDatabasesConfigured()
		{
			return this.configManager.DiagnosticsDatabaseConfiguration.Databases.Count > 0;
		}

		bool IModelValidator.HasChannelAssignedForDiagnosticsECU(string descFilePath, string ecuQualifier, out uint channelNr)
		{
			channelNr = 0u;
			DiagnosticsDatabase diagnosticsDatabase;
			DiagnosticsECU diagnosticsECU;
			if (this.DiagnosticsDatabaseConfiguration.TryGetDiagnosticsDatabase(descFilePath, out diagnosticsDatabase) && diagnosticsDatabase.TryGetECU(ecuQualifier, out diagnosticsECU))
			{
				channelNr = diagnosticsECU.ChannelNumber.Value;
				return true;
			}
			return false;
		}

		bool IModelValidator.IsDiagECUConfigured(string descFilePath, string ecuQualifier, out string currentVariantNameOfEcu)
		{
			DiagnosticsDatabase diagnosticsDatabase;
			DiagnosticsECU diagnosticsECU;
			if (this.DiagnosticsDatabaseConfiguration.TryGetDiagnosticsDatabase(descFilePath, out diagnosticsDatabase) && diagnosticsDatabase.TryGetECU(ecuQualifier, out diagnosticsECU))
			{
				currentVariantNameOfEcu = diagnosticsECU.Variant.Value;
				return true;
			}
			currentVariantNameOfEcu = "";
			return false;
		}

		bool IModelValidator.ValidateRingBufferSettings(TriggerConfiguration triggerConfiguration, bool isDataChanged, IModelValidationResultCollector resultCollector)
		{
			bool result = this.ValidateRingBufferSettings(triggerConfiguration, resultCollector);
			if (isDataChanged)
			{
				this.configManager.NotifyAllDependentFeatures(triggerConfiguration);
			}
			return result;
		}

		bool IModelValidator.ValidatePostTriggerTime(TriggerConfiguration triggerConfiguration, bool isDataChanged, IModelValidationResultCollector resultCollector)
		{
			bool result = this.ValidatePostTriggerTime(triggerConfiguration, resultCollector);
			if (isDataChanged)
			{
				this.configManager.NotifyAllDependentFeatures(triggerConfiguration);
			}
			return result;
		}

		bool IModelValidator.ValidateSymbolicMessageChannelFromFlexrayDb(string databasePath, string networkName, string messageName, uint messageChannel)
		{
			IList<Database> databases = this.DatabaseConfiguration.GetDatabases(databasePath, networkName);
			return databases.Count > 0 && this.ValidateSymbolicMessageChannelFromFlexrayDb(messageName, messageChannel, databases[0]);
		}

		string IModelValidator.ReplaceInvalidMarkerNameCharactersIfPossible(string str)
		{
			return ModelValidator.ReplaceInvalidCharactersIfPossible(str);
		}

		IDictionary<ulong, string> IModelValidator.GetUniqueIdAndNameOfActiveRecordTriggers()
		{
			Dictionary<ulong, string> dictionary = new Dictionary<ulong, string>();
			foreach (TriggerConfiguration current in this.TriggerConfigurations)
			{
				if (current.MemoryRingBuffer.IsActive.Value)
				{
					switch (current.TriggerMode.Value)
					{
					case TriggerMode.Triggered:
						foreach (RecordTrigger current2 in current.Triggers)
						{
							if (current2.IsActive.Value)
							{
								dictionary.Add(current2.Event.Id, current2.Name.Value);
							}
						}
						break;
					}
				}
			}
			return dictionary;
		}

		IList<ulong> IModelValidator.GetUniqueIdsForNameOfActiveRecordTrigger(string triggerName)
		{
			List<ulong> list = new List<ulong>();
			foreach (TriggerConfiguration current in this.TriggerConfigurations)
			{
				if (current.MemoryRingBuffer.IsActive.Value)
				{
					switch (current.TriggerMode.Value)
					{
					case TriggerMode.Triggered:
						foreach (RecordTrigger current2 in current.Triggers)
						{
							if (current2.Name.Value == triggerName)
							{
								list.Add(current2.Event.Id);
							}
						}
						break;
					}
				}
			}
			return list;
		}

		IList<string> IModelValidator.GetSortedNamesOfActiveRecordTriggers()
		{
			IDictionary<ulong, string> uniqueIdAndNameOfActiveRecordTriggers = ((IModelValidator)this).GetUniqueIdAndNameOfActiveRecordTriggers();
			SortedSet<string> sortedSet = new SortedSet<string>();
			foreach (ulong current in uniqueIdAndNameOfActiveRecordTriggers.Keys)
			{
				sortedSet.Add(uniqueIdAndNameOfActiveRecordTriggers[current]);
			}
			return sortedSet.ToList<string>();
		}

		bool IModelValidator.IsCommonDigitalInputOutputPinUsedAsInput(int outputNr)
		{
			return this.configManager.LoggerSpecifics.IO.IsDigitalOutputSupported && this.configManager.LoggerSpecifics.IO.IsDigitalInputOutputCommonPin && this.DigitalInputConfiguration != null && (this.DigitalInputConfiguration.DigitalInputs[outputNr - 1].IsActiveFrequency.Value || this.DigitalInputConfiguration.DigitalInputs[outputNr - 1].IsActiveOnChange.Value);
		}

		bool IModelValidator.ValidateThreeGDataTransferTriggers(DataTransferTriggerConfiguration threeGDataTransferTriggerConfig, bool isDataChanged, IModelValidationResultCollector resultCollector)
		{
			if (this.WLANConfiguration.ThreeGDataTransferTriggerConfiguration != threeGDataTransferTriggerConfig)
			{
				return true;
			}
			bool result = this.ValidateThreeGDataTransferTriggers(this.WLANConfiguration, threeGDataTransferTriggerConfig, resultCollector);
			if (isDataChanged)
			{
				this.configManager.NotifyAllDependentFeatures(this.WLANConfiguration);
			}
			return result;
		}

		bool IModelValidator.IsLedDisabled(uint ledNumber)
		{
			return ledNumber < 1u || (ulong)(ledNumber - 1u) >= (ulong)((long)this.configManager.LEDConfiguration.LEDConfigList.Count) || this.configManager.LEDConfiguration.LEDConfigList[(int)(ledNumber - 1u)].State.Value == LEDState.Disabled;
		}

		bool IModelValidator.ValidateEvent(Event ev, IModelValidationResultCollector resultCollector)
		{
			return this.ValidateEvent(ev, this.DatabaseConfiguration, this.ConfigFolderPath, this.ApplicationDatabaseManager, this.DigitalOutputsConfiguration, resultCollector);
		}

		private bool ValidateCANChannelConfiguration(CANChannelConfiguration canChannelConfig, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			flag &= this.ValidateCANAuxChannelSettings(canChannelConfig, resultCollector);
			return flag & this.ValidateCanErroFrameLogging(canChannelConfig, resultCollector);
		}

		private bool ValidateCANAuxChannelSettings(CANChannelConfiguration canChannelConfig, IModelValidationResultCollector resultCollector)
		{
			if (this.LoggerSpecifics.CAN.AuxChannel <= this.LoggerSpecifics.CAN.NumberOfChannels)
			{
				CANChannel cANChannel = canChannelConfig.GetCANChannel(this.LoggerSpecifics.CAN.AuxChannel);
				if (cANChannel != null && (!cANChannel.IsActive.Value || cANChannel.CANChipConfiguration.Baudrate != Constants.AuxChannelBaudrate || !cANChannel.IsOutputActive.Value))
				{
					int num;
					bool flag;
					int num2;
					if (((IModelValidator)this).IsActiveVoCANEventConfigured(out num, out flag) || ((IModelValidator)this).IsActiveCasKeyEventConfigured(out num2))
					{
						string format = Resources.CasKeyRequiresCanChnSettingsNotSet;
						if (num > 0)
						{
							format = Resources.VoCanRequiresCanChnSettingsNotSet;
						}
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, cANChannel.IsActive, string.Format(format, this.LoggerSpecifics.CAN.AuxChannel, Constants.AuxChannelBaudrate));
						return false;
					}
					for (uint num3 = 7u - (Constants.StartIndexForRemoteLeds - this.configManager.LoggerSpecifics.IO.NumberOfLEDsOnBoard); num3 < this.configManager.LoggerSpecifics.IO.NumberOfLEDsTotal; num3 += 1u)
					{
						if (num3 < (uint)this.configManager.LEDConfiguration.LEDConfigList.Count && this.LEDConfiguration.LEDConfigList[(int)num3].State.Value != LEDState.Disabled)
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, cANChannel.IsActive, string.Format(Resources.AuxLedRequiresCanChnSettingsNotSet, this.LoggerSpecifics.CAN.AuxChannel, Constants.AuxChannelBaudrate));
							return false;
						}
					}
				}
			}
			return true;
		}

		private bool ValidateCanErroFrameLogging(CANChannelConfiguration canChannelConfig, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			List<ValidatedProperty<bool>> chnls = canChannelConfig.LogErrorFramesOnMemories;
			if (!(from errorLogging in chnls
			where errorLogging.Value && ((IModelValidator)this).GetActiveMemoryNumbers.Contains(chnls.IndexOf(errorLogging) + 1)
			select errorLogging).Any<ValidatedProperty<bool>>())
			{
				foreach (CANChannel current in canChannelConfig.ActiveCanChannels)
				{
					if (current.LogErrorFrames.Value)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.LogErrorFrames, Resources.ErrorSelectAtLeastOneMemoryForErrorFrameLogging);
						result = false;
					}
				}
			}
			return result;
		}

		private bool ValidateCANChannelOutputActive(CANChannelConfiguration canChannelConfig, IModelValidationResultCollector resultCollector)
		{
			SortedSet<uint> sortedSet = new SortedSet<uint>();
			foreach (IFeatureTransmitMessages current in this.configManager.FeatureRegistration.FeaturesUsingTransmitMessages)
			{
				foreach (ITransmitMessageChannel current2 in current.ActiveTransmitMessageChannels)
				{
					if (current2.BusType.Value == BusType.Bt_CAN)
					{
						sortedSet.Add(current2.ChannelNumber.Value);
					}
				}
			}
			if (sortedSet.Count == 0)
			{
				return true;
			}
			bool result = true;
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)canChannelConfig.CANChannels.Count))
			{
				CANChannel cANChannel = canChannelConfig.GetCANChannel(num);
				if (cANChannel.IsActive.Value && !cANChannel.IsOutputActive.Value && sortedSet.Contains(num))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, cANChannel.IsOutputActive, "Configuration is using channel for message transmission.");
					result = false;
				}
				num += 1u;
			}
			return result;
		}

		private bool ValidateLINChannelConfiguration(LINChannelConfiguration linChannelConfig, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			IList<uint> activeLINChannelNumbers = linChannelConfig.GetActiveLINChannelNumbers();
			foreach (uint current in activeLINChannelNumbers)
			{
				LINChannel lINChannel = linChannelConfig.GetLINChannel(current);
				flag &= this.ValidateLINChannelChipConfiguration(current, lINChannel, resultCollector);
			}
			if (linChannelConfig.IsUsingLinProbe.Value)
			{
				if (!this.CANChannelConfiguration.GetActiveCANChannelNumbers().Contains(linChannelConfig.CANChannelNrUsedForLinProbe.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, linChannelConfig.CANChannelNrUsedForLinProbe, Resources.ErrorChannelInactive);
					flag = false;
				}
				uint num = this.LoggerSpecifics.LIN.NumberOfChannels + 1u;
				for (uint num2 = num; num2 < num + this.LoggerSpecifics.LIN.NumberOfLINprobeChannels; num2 += 1u)
				{
					flag &= this.ValidateLINChannelChipConfiguration(num2, linChannelConfig.GetLINprobeChannel(num2), resultCollector);
				}
			}
			return flag;
		}

		private bool ValidateLINChannelChipConfiguration(uint channelNr, LINChannel linChannel, IModelValidationResultCollector resultCollector)
		{
			if (!linChannel.UseDbConfigValues.Value)
			{
				return true;
			}
			int num;
			string text;
			int num2;
			bool linChipConfigFromLdfDatabase = ((IModelValidator)this).DatabaseServices.GetLinChipConfigFromLdfDatabase(channelNr, out num, out text, out num2);
			if (num2 == 0)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, linChannel.UseDbConfigValues, Resources.ErrorLinChannelChipConfigNoDB);
				return false;
			}
			if (num2 >= 2)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, linChannel.UseDbConfigValues, Resources.ErrorLinChannelChipConfigTooManyDBs);
				return false;
			}
			if (!linChipConfigFromLdfDatabase)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, linChannel.UseDbConfigValues, Resources.ErrorLinChannelChipConfigNoDataAvailable);
				return false;
			}
			return true;
		}

		private bool ValidateMultibusChannelConfiguration(MultibusChannelConfiguration multibusChannelConfig, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			if (this.LoggerSpecifics.Multibus.NumberOfChannels > 0u)
			{
				if (!multibusChannelConfig.Channels.Any((HardwareChannel channel) => channel.IsActive.Value))
				{
					foreach (HardwareChannel current in multibusChannelConfig.Channels)
					{
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current.IsActive, Resources.ErrorAtLeastOneChannel);
					}
					flag = false;
				}
				bool flag2 = multibusChannelConfig.GetChannel(this.LoggerSpecifics.Multibus.ChannelLINStartIndex) is LINChannel;
				for (uint num = this.LoggerSpecifics.Multibus.ChannelLINStartIndex + 1u; num <= this.LoggerSpecifics.Multibus.NumberOfPiggyConfigurableChannels; num += 1u)
				{
					HardwareChannel channel2 = multibusChannelConfig.GetChannel(num);
					if (!flag2 && channel2.IsActive.Value && channel2 is LINChannel)
					{
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, channel2.BusType, string.Format(Resources.ErrorChnCanNotBeUsedAsLINChn, num));
						flag = false;
					}
				}
				foreach (uint current2 in multibusChannelConfig.LINChannels.Keys)
				{
					LINChannel lINChannel = multibusChannelConfig.LINChannels[current2];
					if (lINChannel.IsActive.Value)
					{
						flag &= this.ValidateLINChannelChipConfiguration(current2, lINChannel, resultCollector);
					}
				}
			}
			return flag;
		}

		private bool ValidateLogDataStorage(LogDataStorage logDataStorage, DatabaseConfiguration databaseConfig, string configFolderPath, IApplicationDatabaseManager databaseManager, DigitalOutputsConfiguration digitalOutputsConfig, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			flag &= this.ValidateTimeoutToSleep(logDataStorage, resultCollector);
			flag &= this.ValidateEventActivationDelay(logDataStorage, resultCollector);
			if (logDataStorage.StopCyclicCommunicationEvent != null)
			{
				flag &= this.ValidateStopCyclicCommEvent(logDataStorage, databaseConfig, configFolderPath, databaseManager, digitalOutputsConfig, resultCollector);
			}
			return flag;
		}

		private bool ValidateTimeoutToSleep(LogDataStorage logDataStorage, IModelValidationResultCollector resultCollector)
		{
			if (logDataStorage.IsEnterSleepModeEnabled.Value && (logDataStorage.TimeoutToSleep.Value < Constants.MinimumTimeoutToSleep || logDataStorage.TimeoutToSleep.Value > Constants.MaximumTimeoutToSleep))
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, logDataStorage.TimeoutToSleep, string.Format(Resources.ErrorTimeoutToSleepOutOfRange, Constants.MinimumTimeoutToSleep, Constants.MaximumTimeoutToSleep));
				return false;
			}
			return true;
		}

		private bool ValidateEventActivationDelay(LogDataStorage logDataStorage, IModelValidationResultCollector resultCollector)
		{
			if (logDataStorage.EventActivationDelayAfterStart.Value > Constants.MaxEventActivationDelay_ms)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, logDataStorage.EventActivationDelayAfterStart, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 0, Constants.MaxEventActivationDelay_ms));
				return false;
			}
			return true;
		}

		private bool ValidateStopCyclicCommEvent(LogDataStorage logDataStorage, DatabaseConfiguration databaseConfig, string configFolderPath, IApplicationDatabaseManager databaseManager, DigitalOutputsConfiguration digitalOutputsConfig, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (logDataStorage.StopCyclicCommunicationEvent is ISymbolicSignalEvent)
			{
				result = this.ValidateSymbolicSignalEvent(logDataStorage.StopCyclicCommunicationEvent as ISymbolicSignalEvent, databaseConfig, configFolderPath, databaseManager, resultCollector);
			}
			else if (logDataStorage.StopCyclicCommunicationEvent is CANDataEvent)
			{
				result = this.ValidateCANDataEventChannel(logDataStorage.StopCyclicCommunicationEvent as CANDataEvent, resultCollector);
			}
			else if (logDataStorage.StopCyclicCommunicationEvent is LINDataEvent)
			{
				result = this.ValidateLINDataEventChannel(logDataStorage.StopCyclicCommunicationEvent as LINDataEvent, resultCollector);
			}
			else if (logDataStorage.StopCyclicCommunicationEvent is MsgTimeoutEvent)
			{
				result = this.ValidateMsgTimeoutEvent(logDataStorage.StopCyclicCommunicationEvent as MsgTimeoutEvent, databaseConfig, configFolderPath, databaseManager, resultCollector);
			}
			else if (logDataStorage.StopCyclicCommunicationEvent is DigitalInputEvent && digitalOutputsConfig != null)
			{
				result = this.ValidateDigitalInputEvent(logDataStorage.StopCyclicCommunicationEvent as DigitalInputEvent, digitalOutputsConfig, resultCollector);
			}
			return result;
		}

		private bool ValidateMOST150ChannelConfiguration(MOST150ChannelConfiguration most150ChannelConfig, IModelValidationResultCollector resultCollector)
		{
			if (most150ChannelConfig.IsChannelEnabled.Value && !most150ChannelConfig.IsLogStatusEventsEnabled.Value && !most150ChannelConfig.IsLogControlMsgsEnabled.Value && !most150ChannelConfig.IsLogAsyncMDPEnabled.Value && !most150ChannelConfig.IsLogAsyncMEPEnabled.Value)
			{
				string errorOneTypeOfEventMustBeEnabled = Resources.ErrorOneTypeOfEventMustBeEnabled;
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, most150ChannelConfig.IsLogStatusEventsEnabled, errorOneTypeOfEventMustBeEnabled);
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, most150ChannelConfig.IsLogControlMsgsEnabled, errorOneTypeOfEventMustBeEnabled);
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, most150ChannelConfig.IsLogAsyncMDPEnabled, errorOneTypeOfEventMustBeEnabled);
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, most150ChannelConfig.IsLogAsyncMEPEnabled, errorOneTypeOfEventMustBeEnabled);
				return false;
			}
			return true;
		}

		private bool ValidateDatabaseConfiguration(DatabaseConfiguration databaseConfiguration, MultibusChannelConfiguration multibusChannelConfiguration, CANChannelConfiguration canChannelConfiguration, LINChannelConfiguration linChannelConfiguration, FlexrayChannelConfiguration flexrayChannelConfiguration, string configFolderPath, IModelValidationResultCollector resultCollector)
		{
			HashSet<Database> filesNotFound = new HashSet<Database>(from db in this.DatabaseConfiguration.AllDescriptionFiles
			where db.IsFileNotFound
			select db);
			bool flag = true;
			bool flag2 = this.ValidateMultipleFlexrayChannelAssignments(databaseConfiguration, flexrayChannelConfiguration, resultCollector);
			flag &= flag2;
			foreach (Database current in databaseConfiguration.Databases)
			{
				if (!this.ValidateDatabaseFileExistence(current, configFolderPath, resultCollector))
				{
					flag = false;
				}
				if (BusType.Bt_CAN == current.BusType.Value)
				{
					flag &= this.ValidateCanDatabaseChannel(current, canChannelConfiguration, multibusChannelConfiguration, resultCollector);
				}
				else if (BusType.Bt_LIN == current.BusType.Value)
				{
					flag &= this.ValidateLINDatabaseChannel(current, linChannelConfiguration, multibusChannelConfiguration, resultCollector);
				}
				else if (BusType.Bt_FlexRay == current.BusType.Value && flag2)
				{
					flag &= this.ValidateFlexrayDatabaseChannel(current, flexrayChannelConfiguration, resultCollector);
				}
			}
			if (this.DatabaseConfiguration.AllDescriptionFiles.Any((Database db) => filesNotFound.Contains(db) && !db.IsFileNotFound))
			{
				this.ApplicationDatabaseManager.UpdateApplicationDatabaseConfiguration(this.DatabaseConfiguration, this.MultibusChannelConfiguration, this.CANChannelConfiguration, this.ConfigFolderPath);
			}
			return flag;
		}

		private bool ValidateExportDatabaseConfiguration(ExportDatabaseConfiguration exportDatabaseConfiguration, string configFolderPath, IModelValidationResultCollector resultCollector)
		{
			new HashSet<Database>(from db in exportDatabaseConfiguration.AllDescriptionFiles
			where db.IsFileNotFound
			select db);
			bool result = true;
			foreach (ExportDatabase current in exportDatabaseConfiguration.ExportDatabases)
			{
				if (current.IsInsideAnalysisPackage())
				{
					FileInfo fileInfo = null;
					if (!string.IsNullOrEmpty(current.AnalysisPackagePath.Value))
					{
						fileInfo = new FileInfo(FileSystemServices.GetAbsolutePath(current.AnalysisPackagePath.Value, configFolderPath));
					}
					current.IsFileNotFound = false;
					if (fileInfo == null || !fileInfo.Exists)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.AnalysisPackagePath, Resources.ErrorFileNotFound);
						current.IsFileNotFound = true;
					}
				}
				else if (!this.ValidateDatabaseFileExistence(current, configFolderPath, resultCollector))
				{
					result = false;
				}
			}
			return result;
		}

		private bool ValidateMultipleFlexrayChannelAssignments(DatabaseConfiguration databaseConfiguration, FlexrayChannelConfiguration flexrayChannelConfiguration, IModelValidationResultCollector resultCollector)
		{
			if (flexrayChannelConfiguration.FlexrayChannels.Count == 0)
			{
				return true;
			}
			bool result = true;
			Dictionary<uint, uint> dictionary = new Dictionary<uint, uint>();
			foreach (Database current in databaseConfiguration.FlexrayDatabases)
			{
				if (dictionary.ContainsKey(current.ChannelNumber.Value))
				{
					Dictionary<uint, uint> dictionary2;
					uint value;
					(dictionary2 = dictionary)[value = current.ChannelNumber.Value] = dictionary2[value] + 1u;
				}
				else
				{
					dictionary.Add(current.ChannelNumber.Value, 1u);
				}
			}
			foreach (uint current2 in dictionary.Keys)
			{
				if (dictionary[current2] > 1u)
				{
					foreach (Database current3 in databaseConfiguration.FlexrayDatabases)
					{
						if (current3.ChannelNumber.Value == current2)
						{
							resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current3.ChannelNumber, Resources.ErrorFlexrayChnOnlyOneDb);
						}
					}
					result = false;
				}
			}
			return result;
		}

		private bool ValidateCCPXCPConfiguration(DatabaseConfiguration databaseConfiguration, MultibusChannelConfiguration multibusChannelConfiguration, CANChannelConfiguration canChannelConfiguration, FlexrayChannelConfiguration flexrayChannelConfiguration, MOST150ChannelConfiguration most150ChannelConfiguration, EthernetConfiguration ethernetConfiguration, string configFolderPath, IApplicationDatabaseManager dbManager, IModelValidationResultCollector resultCollector)
		{
			HashSet<Database> filesNotFound = new HashSet<Database>(from db in this.DatabaseConfiguration.AllDescriptionFiles
			where db.IsFileNotFound
			select db);
			if (databaseConfiguration == null)
			{
				return true;
			}
			bool flag = true;
			foreach (Database current in databaseConfiguration.ActiveCCPXCPDatabases)
			{
				bool flag2 = this.ValidateDatabaseFileExistence(current, configFolderPath, resultCollector);
				if (!flag2)
				{
					current.IsInconsistent = false;
				}
				flag &= flag2;
				if (BusType.Bt_FlexRay == current.BusType.Value)
				{
					flag &= this.ValidateFlexrayDatabaseChannel(current, flexrayChannelConfiguration, resultCollector);
					flag &= this.ValidateXcpFlexrayDatabaseChannel(current, resultCollector);
					flag &= this.ValidateMatchingFlexRayFibex(current, configFolderPath, resultCollector);
				}
				else if (BusType.Bt_Ethernet == current.BusType.Value)
				{
					if (most150ChannelConfiguration.IsChannelEnabled.Value)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.CcpXcpEcuDisplayName, Resources_CcpXcp.ErrorCcpXcpEthernetAndMost150NotAllowed);
						flag = false;
					}
				}
				else
				{
					flag &= this.ValidateCanDatabaseChannel(current, canChannelConfiguration, multibusChannelConfiguration, resultCollector);
				}
				if (flag2)
				{
					flag &= this.ValidateCcpXcpExistenceInDatabase(current, configFolderPath, dbManager, resultCollector);
				}
				flag &= this.ValidateCcpXcpEcuName(databaseConfiguration, current, resultCollector);
				flag &= this.ValidateDatabaseConsistency(current, resultCollector);
				if (!this.ValidateCcpXcpDatabaseSettings(current, ethernetConfiguration, resultCollector) | !this.ValidateCcpXcpEcuIdSettings(current, databaseConfiguration, this.CcpXcpSignalConfiguration, this.CANChannelConfiguration, resultCollector))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.CcpXcpIsSeedAndKeyUsedValidatedProperty, Resources_CcpXcp.CcpXcpDatabaseSettingsErroneous);
					flag = false;
				}
			}
			if (this.DatabaseConfiguration.AllDescriptionFiles.Any((Database db) => filesNotFound.Contains(db) && !db.IsFileNotFound))
			{
				this.ApplicationDatabaseManager.UpdateApplicationDatabaseConfiguration(this.DatabaseConfiguration, this.MultibusChannelConfiguration, this.CANChannelConfiguration, this.ConfigFolderPath);
			}
			return flag;
		}

		private bool ValidateCCPXCPSignalConfiguration(CcpXcpSignalConfiguration signalConfiguration, DatabaseConfiguration databaseConfiguration, string configFolderPath, IApplicationDatabaseManager appDbManager, DigitalOutputsConfiguration digitalOutputsConfig, IModelValidationResultCollector resultCollector)
		{
			if (signalConfiguration == null)
			{
				return true;
			}
			if (!signalConfiguration.Actions.Any<ActionCcpXcp>())
			{
				return true;
			}
			bool flag = true;
			flag = this.ValidateCCPXCPSignalActionsConfiguration(signalConfiguration, databaseConfiguration, appDbManager, configFolderPath, digitalOutputsConfig, resultCollector);
			ReadOnlyCollection<CcpXcpSignal> activeSignals = signalConfiguration.ActiveSignals;
			if (!activeSignals.Any<CcpXcpSignal>())
			{
				return flag;
			}
			HashSet<CcpXcpSignalComparer> hashSet = new HashSet<CcpXcpSignalComparer>();
			using (IEnumerator<CcpXcpSignal> enumerator = activeSignals.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					CcpXcpSignal signal = enumerator.Current;
					if (!signal.DatabaseDisabled && signal.MeasurementMode.Value == CcpXcpMeasurementMode.DAQ && (signal.DaqEvents == null || signal.DaqEvents.Count < 1))
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, signal.MeasurementMode, Resources_CcpXcp.CcpXcpNoDaqEventsFound);
						flag = false;
					}
					uint num;
					if (signal.MeasurementMode.Value == CcpXcpMeasurementMode.Polling && (!uint.TryParse(signal.PollingTime.Value, out num) || num < 1u))
					{
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.DaqEventId, Resources.OutOfRangeGenError);
						flag = false;
					}
					if (!hashSet.Add(new CcpXcpSignalComparer(signal)))
					{
						foreach (CcpXcpSignal current in from sig in activeSignals
						where new CcpXcpSignalComparer(sig).Equals(signal)
						select sig)
						{
							resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current.Name, Resources_CcpXcp.ErrorCcpXcpDuplicatedSignal);
						}
						flag = false;
					}
					if (signal.ActionCcpXcp.IsActive.Value)
					{
						ActionCcpXcp actionCcpXcp = signal.ActionCcpXcp;
						if (actionCcpXcp.Mode != ActionCcpXcp.ActivationMode.Always && signal.MeasurementMode.Value == CcpXcpMeasurementMode.DAQ)
						{
							resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.MeasurementMode, Resources_CcpXcp.ErrorCcpXcpDAQOnlyInPermanent);
							flag = false;
						}
						uint num2;
						if (actionCcpXcp.Event is CyclicTimerEvent && signal.MeasurementMode.Value == CcpXcpMeasurementMode.Polling && uint.TryParse(signal.PollingTime.Value, out num2) && (actionCcpXcp.Event as CyclicTimerEvent).CyclicTimeInMilliSec() <= num2)
						{
							resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.PollingTime, Resources_CcpXcp.ErrorCcpXcpPollingTimeLargerThanTimer);
							flag = false;
						}
						uint num3;
						if (actionCcpXcp.StopType is StopOnTimer && signal.MeasurementMode.Value == CcpXcpMeasurementMode.Polling && uint.TryParse(signal.PollingTime.Value, out num3) && (actionCcpXcp.StopType as StopOnTimer).Duration.Value <= num3)
						{
							resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.PollingTime, Resources_CcpXcp.ErrorCcpXcpPollingTimeLargerThanTimer);
							flag = false;
						}
					}
					bool flag2 = true;
					if (!signal.DatabaseDisabled)
					{
						if (databaseConfiguration.ActiveCCPXCPDatabases.All((Database d) => d.CcpXcpEcuDisplayName.Value != signal.EcuName.Value))
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, signal.EcuName, Resources_CcpXcp.ErrorCcpXcpEcuNameNotFound);
							flag2 = false;
						}
					}
					flag &= flag2;
					Database database = databaseConfiguration.CCPXCPDatabases.FirstOrDefault((Database d) => d.CcpXcpEcuDisplayName.Value == signal.EcuName.Value);
					if (database != null)
					{
						if (database.IsInconsistent)
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, signal.EcuName, Resources_CcpXcp.ErrorCcpXcpDatabaseErroneous);
							flag = false;
						}
						else
						{
							A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(database);
							if (a2LDatabase != null && a2LDatabase.LoggerEcu != null)
							{
								ISignal signal2 = a2LDatabase.GetSignal(signal.Name.Value);
								if (signal2 == null)
								{
									if (!signal.DatabaseDisabled && flag2)
									{
										flag = false;
										resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, signal.EcuName, Resources_CcpXcp.CcpXcpSignalNotFoundInEcu);
									}
								}
								else
								{
									LcValidateSignalResult lcValidateSignalResult = LcValidateSignalResult.kOk;
									if (signal.MeasurementMode.Value == CcpXcpMeasurementMode.DAQ)
									{
										if (signal.DaqEvents != null && signal.DaqEvents.Count > 0)
										{
											lcValidateSignalResult = a2LDatabase.LoggerEcu.ValidateSignalDaq(signal2, signal.DaqEventId.Value);
										}
									}
									else if (signal.MeasurementMode.Value == CcpXcpMeasurementMode.Polling)
									{
										try
										{
											lcValidateSignalResult = a2LDatabase.LoggerEcu.ValidateSignalPolling(signal2, Convert.ToUInt32(signal.PollingTime.Value));
										}
										catch (Exception)
										{
										}
									}
									if (lcValidateSignalResult != LcValidateSignalResult.kOk)
									{
										flag = false;
										switch (lcValidateSignalResult)
										{
										case LcValidateSignalResult.kErrorDimension:
											resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.Name, Resources_CcpXcp.CcpXcpSignalErrorDimension);
											continue;
										case LcValidateSignalResult.kErrorOffset:
											resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.Name, Resources_CcpXcp.CcpXcpSignalErrorOffset);
											continue;
										case LcValidateSignalResult.kErrorType:
											resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.Name, Resources_CcpXcp.CcpXcpSignalErrorType);
											continue;
										case LcValidateSignalResult.kErrorValueType:
											resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.Name, Resources_CcpXcp.CcpXcpSignalErrorValueType);
											continue;
										case LcValidateSignalResult.kErrorDaqEventId:
											if (signal.MeasurementMode.Value == CcpXcpMeasurementMode.DAQ)
											{
												resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, signal.DaqEventId, Resources_CcpXcp.CcpXcpEventForDaqIdNotFound);
												continue;
											}
											continue;
										case LcValidateSignalResult.kErrorPollingCycle:
											if (signal.MeasurementMode.Value == CcpXcpMeasurementMode.Polling)
											{
												resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.PollingTime, Resources_CcpXcp.CcpXcpSignalErrorPollingCycle);
												continue;
											}
											continue;
										case LcValidateSignalResult.kPollingNotSupported:
											if (signal.MeasurementMode.Value == CcpXcpMeasurementMode.Polling)
											{
												resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.MeasurementMode, Resources_CcpXcp.CcpXcpSignalErrorPollingNotSupported);
												continue;
											}
											continue;
										case LcValidateSignalResult.kNonCyclicDaqNotSupported:
											resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.DaqEventId, Resources_CcpXcp.CcpXcpSignalErrorNonCyclic);
											continue;
										case LcValidateSignalResult.kErrorAddress:
											resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.Name, Resources_CcpXcp.CcpXcpSignalErrorAddress);
											continue;
										case LcValidateSignalResult.kErrorPollingBitMaskMultiByte:
											if (signal.MeasurementMode.Value == CcpXcpMeasurementMode.Polling)
											{
												resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.MeasurementMode, Resources_CcpXcp.CcpXcpSignalErrorPollingBitMaskMultiByteNotSupported);
												continue;
											}
											continue;
										}
										resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, signal.Name, Resources_CcpXcp.CcpXcpSignalErrorUnknown);
									}
								}
							}
						}
					}
				}
			}
			return flag;
		}

		private bool ValidateCCPXCPSignalActionsConfiguration(CcpXcpSignalConfiguration signalConfiguration, DatabaseConfiguration databasesConfig, IApplicationDatabaseManager databaseManager, string configFolderPath, DigitalOutputsConfiguration digitalOutputsConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			foreach (ActionCcpXcp current in signalConfiguration.Actions)
			{
				for (int i = 0; i < 2; i++)
				{
					Event @event = current.Event;
					if (current.IsActive.Value)
					{
						if (i == 1)
						{
							if (!(current.StopType is StopOnEvent) || (current.StopType as StopOnEvent).Event == null)
							{
								goto IL_1DE;
							}
							@event = (current.StopType as StopOnEvent).Event;
						}
						if (@event is CANIdEvent)
						{
							flag &= this.ValidateCANIdEventChannel(@event as CANIdEvent, resultCollector);
						}
						else if (@event is LINIdEvent)
						{
							flag &= this.ValidateLINIdEventChannel(@event as LINIdEvent, resultCollector);
						}
						else if (@event is FlexrayIdEvent)
						{
							flag &= this.ValidateFlexrayIdEventChannel(@event as FlexrayIdEvent, resultCollector);
						}
						else if (@event is CANDataEvent)
						{
							flag &= this.ValidateCANDataEventChannel(@event as CANDataEvent, resultCollector);
						}
						else if (@event is LINDataEvent)
						{
							flag &= this.ValidateLINDataEventChannel(@event as LINDataEvent, resultCollector);
						}
						else if (@event is SymbolicMessageEvent)
						{
							flag &= this.ValidateSymbolicMessageEvent(@event as SymbolicMessageEvent, databasesConfig, configFolderPath, databaseManager, resultCollector);
						}
						else if (@event is ISymbolicSignalEvent)
						{
							flag &= this.ValidateSymbolicSignalEvent(@event as ISymbolicSignalEvent, databasesConfig, configFolderPath, databaseManager, resultCollector);
						}
						else if (@event is MsgTimeoutEvent)
						{
							flag &= this.ValidateMsgTimeoutEvent(@event as MsgTimeoutEvent, databasesConfig, configFolderPath, databaseManager, resultCollector);
						}
						else if (@event is DigitalInputEvent)
						{
							flag &= this.ValidateDigitalInputEvent(@event as DigitalInputEvent, digitalOutputsConfiguration, resultCollector);
						}
						else if (@event is KeyEvent)
						{
							flag &= this.ValidateKeyEvent(@event as KeyEvent, resultCollector);
						}
					}
					IL_1DE:;
				}
			}
			return flag;
		}

		private bool ValidateCanDatabaseChannel(Database database, CANChannelConfiguration canChannelConfiguration, MultibusChannelConfiguration multibusChannelConfiguration, IModelValidationResultCollector resultCollector)
		{
			if (BusType.Bt_CAN != database.BusType.Value)
			{
				return true;
			}
			if ((long)canChannelConfiguration.CANChannels.Count >= (long)((ulong)database.ChannelNumber.Value))
			{
				if (!canChannelConfiguration.GetCANChannel(database.ChannelNumber.Value).IsActive.Value)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.ChannelNumber, Resources.ErrorChannelInactive);
					return false;
				}
			}
			else if (multibusChannelConfiguration.NumberOfChannels >= database.ChannelNumber.Value)
			{
				if (!multibusChannelConfiguration.CANChannels.ContainsKey(database.ChannelNumber.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.ChannelNumber, Resources.ErrorChannelNotAvailable);
					return false;
				}
				if (!multibusChannelConfiguration.CANChannels[database.ChannelNumber.Value].IsActive.Value)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.ChannelNumber, Resources.ErrorChannelInactive);
					return false;
				}
				string absoluteFilePath = this.configManager.GetAbsoluteFilePath(database.FilePath.Value);
				if (File.Exists(absoluteFilePath) && !multibusChannelConfiguration.CANChannels[database.ChannelNumber.Value].CANChipConfiguration.IsCANFD && this.ApplicationDatabaseManager.IsCanFdDatabase(absoluteFilePath, database.NetworkName.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.ChannelNumber, Resources.ErrorChnNotCANFDButDB);
					return false;
				}
			}
			return true;
		}

		private bool ValidateLINDatabaseChannel(Database database, LINChannelConfiguration linChannelConfiguration, MultibusChannelConfiguration multibusChannelConfiguration, IModelValidationResultCollector resultCollector)
		{
			if (BusType.Bt_LIN == database.BusType.Value)
			{
				if (linChannelConfiguration.LINChannels.Count > 0)
				{
					if ((ulong)database.ChannelNumber.Value <= (ulong)((long)linChannelConfiguration.LINChannels.Count))
					{
						if (!linChannelConfiguration.GetLINChannel(database.ChannelNumber.Value).IsActive.Value)
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.ChannelNumber, Resources.ErrorChannelInactive);
							return false;
						}
					}
					else
					{
						uint num = 0u;
						if (linChannelConfiguration.IsUsingLinProbe.Value)
						{
							num = this.LoggerSpecifics.LIN.NumberOfLINprobeChannels;
						}
						if ((ulong)database.ChannelNumber.Value > (ulong)((long)linChannelConfiguration.LINChannels.Count + (long)((ulong)num)))
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.ChannelNumber, Resources.ErrorChannelNotAvailable);
							return true;
						}
					}
				}
				else if (multibusChannelConfiguration.NumberOfChannels > 0u)
				{
					if (!multibusChannelConfiguration.LINChannels.ContainsKey(database.ChannelNumber.Value))
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.ChannelNumber, Resources.ErrorChannelNotAvailable);
						return false;
					}
					if (!multibusChannelConfiguration.LINChannels[database.ChannelNumber.Value].IsActive.Value)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.ChannelNumber, Resources.ErrorChannelInactive);
						return false;
					}
				}
			}
			return true;
		}

		private bool ValidateFlexrayDatabaseChannel(Database database, FlexrayChannelConfiguration flexrayChannelConfiguration, IModelValidationResultCollector resultCollector)
		{
			if (BusType.Bt_FlexRay == database.BusType.Value)
			{
				if (database.ChannelNumber.Value == Database.ChannelNumber_FlexrayAB)
				{
					if (!flexrayChannelConfiguration.GetFlexrayChannel(1u).IsActive.Value || !flexrayChannelConfiguration.GetFlexrayChannel(2u).IsActive.Value)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.ChannelNumber, Resources.ErrorChannelsInactive);
						return false;
					}
					return true;
				}
				else if ((long)flexrayChannelConfiguration.FlexrayChannels.Count < (long)((ulong)(database.ChannelNumber.Value - 1u)) || !flexrayChannelConfiguration.GetFlexrayChannel(database.ChannelNumber.Value).IsActive.Value)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.ChannelNumber, Resources.ErrorChannelInactive);
					return false;
				}
			}
			return true;
		}

		private bool ValidateXcpFlexrayDatabaseChannel(Database database, IModelValidationResultCollector resultCollector)
		{
			if (BusType.Bt_FlexRay == database.BusType.Value && database.CPType.Value != CPType.None && database.IsCPActive.Value)
			{
				if (this.DatabaseConfiguration.CCPXCPDatabases.Count((Database db) => db.BusType.Value == BusType.Bt_FlexRay && db.IsCPActive.Value && db.IsBusDatabase) > 1)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.ChannelNumber, Resources_CcpXcp.ErrorMultipleCpFlexRayDatabases);
					return false;
				}
			}
			return true;
		}

		private bool ValidateMatchingFlexRayFibex(Database database, string configFolderPath, IModelValidationResultCollector resultCollector)
		{
			if (BusType.Bt_FlexRay == database.BusType.Value && database.FileType == DatabaseFileType.A2L && database.CcpXcpEcuList != null && database.CcpXcpEcuList.Any<CcpXcpEcu>() && !this.ApplicationDatabaseManager.FlexRayDatabasesContainXcpSlave(database.CcpXcpEcuDisplayName.Value, configFolderPath, 0u))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.CcpXcpEcuDisplayName, Resources_CcpXcp.ErrorCcpXcpNoFibexFoundForEcu);
				return false;
			}
			return true;
		}

		private bool ValidateDatabaseFileExistence(Database database, string configFolderPath, IModelValidationResultCollector resultCollector)
		{
			FileInfo fileInfo = new FileInfo(FileSystemServices.GetAbsolutePath(database.FilePath.Value, configFolderPath));
			if (!fileInfo.Exists)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.FilePath, Resources.ErrorFileNotFound);
				database.IsFileNotFound = true;
				return false;
			}
			database.IsFileNotFound = false;
			return true;
		}

		private bool ValidateDatabaseConsistency(Database database, IModelValidationResultCollector resultCollector)
		{
			if (database.IsInconsistent)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.FilePath, Resources_CcpXcp.ErrorCcpXcpDatabaseInconsistency);
				return false;
			}
			return true;
		}

		private bool ValidateCcpXcpExistenceInDatabase(Database database, string configFolderPath, IApplicationDatabaseManager dbManager, IModelValidationResultCollector resultCollector)
		{
			if (database.FileType == DatabaseFileType.A2L || !database.IsCPActive.Value)
			{
				return true;
			}
			IDictionary<string, bool> dictionary;
			uint num;
			if (dbManager.GetDatabaseCPConfiguration(database, configFolderPath, out dictionary, out num) == CPType.None)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.IsCPActive, Resources_CcpXcp.ErrorMissingCCPXCPFunctionality);
				return false;
			}
			return true;
		}

		private bool ValidateCcpXcpEcuName(DatabaseConfiguration databaseConfiguration, Database database, IModelValidationResultCollector resultCollector)
		{
			if (database.CcpXcpEcuList == null)
			{
				return true;
			}
			string name = database.CcpXcpEcuDisplayName.Value;
			if (database.FileType == DatabaseFileType.A2L && (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name)))
			{
				if (database.BusType.Value == BusType.Bt_FlexRay)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.CcpXcpEcuDisplayName, Resources_CcpXcp.ErrorCcpXcpNoEcuAssigned);
				}
				else
				{
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, database.CcpXcpEcuDisplayName, Resources_CcpXcp.InvalidCcpXcpEcuNameEmpty);
				}
				return false;
			}
			if (name != null && name.Length >= 200)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, database.CcpXcpEcuDisplayName, string.Format(Resources_CcpXcp.InvalidCcpXcpEcuNameTooLong, 200));
				return false;
			}
			if (name != null && database.FileType == DatabaseFileType.A2L && !ModelValidator.CaplCompliantWhitelist.IsMatch(name))
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, database.CcpXcpEcuDisplayName, string.Format(Resources.InvalidNameStringWhitelist, name, ModelValidator.caplCompliantCharactersList));
				return false;
			}
			if (name != null && databaseConfiguration.ActiveCCPXCPDatabases.Any((Database db) => db != database && string.Compare(db.CcpXcpEcuDisplayName.Value, name, StringComparison.OrdinalIgnoreCase) == 0))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.CcpXcpEcuDisplayName, string.Format(Resources_CcpXcp.InvalidCcpXcpEcuNameDuplicate, name));
				return false;
			}
			return true;
		}

		private bool ValidateCcpXcpDatabaseSettings(Database database, EthernetConfiguration ethernetConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			if (database.CcpXcpIsSeedAndKeyUsed || (database.CcpXcpUseDbParams && database.CpProtectionsWithSeedAndKeyRequired.Any<CPProtection>()))
			{
				if (database.SeedAndKeyPath == null || string.IsNullOrEmpty(database.SeedAndKeyPath.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.SeedAndKeyPath, Resources.ErrorSKBFileMissing);
					flag = false;
				}
				else
				{
					try
					{
						FileInfo fileInfo = new FileInfo(FileSystemServices.GetAbsolutePath(database.SeedAndKeyPath.Value, this.ConfigFolderPath));
						if (!fileInfo.Exists)
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.SeedAndKeyPath, Resources.ErrorFileNotFound);
							flag = false;
						}
					}
					catch (Exception)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.SeedAndKeyPath, Resources.ErrorFileNotFound);
						flag = false;
					}
				}
			}
			if (database.FileType == DatabaseFileType.A2L && database.CPType.Value == CPType.XCP && (database.BusType.Value == BusType.Bt_FlexRay || database.BusType.Value == BusType.Bt_Ethernet))
			{
				uint num = Constants.MaxCcpXcpFlexRayDTO;
				if (database.BusType.Value == BusType.Bt_Ethernet)
				{
					num = Constants.MaxCcpXcpUdpDTO;
				}
				uint num2;
				if (CcpXcpManager.Instance().GetMaxDTO(database, out num2))
				{
					if (num2 < Constants.MinCcpXcpDTO || num2 > num)
					{
						resultCollector.SetErrorText(database.CcpXcpEcuList[0].UseDbParams ? ValidationErrorClass.GlobalModelError : ValidationErrorClass.FormatError, database.CcpXcpEcuList[0].MaxDTOValidatedProperty, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinCcpXcpDTO, num));
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
				if (database.CcpXcpEcuList != null && database.CcpXcpEcuList.Any<CcpXcpEcu>() && !database.CcpXcpEcuList[0].UseVxModule)
				{
					num = Constants.MaxCcpXcpFlexRayCTO;
					if (database.BusType.Value == BusType.Bt_Ethernet)
					{
						num = Constants.MaxCcpXcpEthernetCTO;
					}
					uint num3;
					if (CcpXcpManager.Instance().GetMaxCTO(database, out num3))
					{
						if (num3 < Constants.MinCcpXcpCTO || num3 > num)
						{
							resultCollector.SetErrorText(ValidationErrorClass.FormatError, database.CcpXcpEcuList[0].MaxCTOValidatedProperty, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinCcpXcpCTO, num));
							flag = false;
						}
					}
					else
					{
						flag = false;
					}
				}
			}
			bool flag2;
			if (database.FileType == DatabaseFileType.A2L && database.CPType.Value == CPType.XCP && database.CcpXcpEcuList != null && database.CcpXcpEcuList.Any<CcpXcpEcu>() && CcpXcpManager.Instance().GetUseEcuTimestamp(database, out flag2) && flag2)
			{
				CcpXcpEcuTimestampUnit ccpXcpEcuTimestampUnit;
				if (CcpXcpManager.Instance().GetEcuTimestampUnit(database, out ccpXcpEcuTimestampUnit))
				{
					switch (ccpXcpEcuTimestampUnit)
					{
					case CcpXcpEcuTimestampUnit.TU_1Nanoseconds:
					case CcpXcpEcuTimestampUnit.TU_10Nanoseconds:
					case CcpXcpEcuTimestampUnit.TU_100Nanoseconds:
					case CcpXcpEcuTimestampUnit.TU_1Microseconds:
					case CcpXcpEcuTimestampUnit.TU_10Microseconds:
					case CcpXcpEcuTimestampUnit.TU_100Microseconds:
					case CcpXcpEcuTimestampUnit.TU_1Milliseconds:
					case CcpXcpEcuTimestampUnit.TU_10Milliseconds:
					case CcpXcpEcuTimestampUnit.TU_100Milliseconds:
					case CcpXcpEcuTimestampUnit.TU_1Seconds:
						break;
					default:
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.CcpXcpEcuList[0].EcuTimestampUnitValidatedProperty, Resources_CcpXcp.CcpXcpErrorTimestampUnit);
						flag = false;
						break;
					}
				}
				else
				{
					flag = false;
				}
				uint num4;
				if (CcpXcpManager.Instance().GetEcuTimestampTicks(database, out num4))
				{
					if (num4 < Constants.MinCcpXcpEcuTimestampTicks || num4 > Constants.MaxCcpXcpEcuTimestampTicks)
					{
						resultCollector.SetErrorText(database.CcpXcpEcuList[0].UseDbParams ? ValidationErrorClass.GlobalModelError : ValidationErrorClass.FormatError, database.CcpXcpEcuList[0].EcuTimestampTicksValidatedProperty, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinCcpXcpEcuTimestampTicks, Constants.MaxCcpXcpEcuTimestampTicks));
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
				uint num5;
				if (CcpXcpManager.Instance().GetEcuTimestampWidth(database, out num5))
				{
					if (num5 != 1u && num5 != 2u && num5 != 4u)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.CcpXcpEcuList[0].EcuTimestampWidthValidatedProperty, Resources.ErrorGenValueOutOfRange);
						flag = false;
					}
				}
				else
				{
					flag = false;
				}
			}
			flag &= this.ValidateCcpXcpEcuNax(database, resultCollector);
			if (database.BusType.Value == BusType.Bt_Ethernet)
			{
				flag &= this.ValidateCcpXcpEcuIPConfig(database, ethernetConfiguration, resultCollector);
			}
			return flag;
		}

		private bool ValidateCcpXcpEcuIdSettings(Database database, DatabaseConfiguration databaseConfiguration, CcpXcpSignalConfiguration signalConfiguration, CANChannelConfiguration canChannelConfig, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (database == null || database.FileType != DatabaseFileType.A2L)
			{
				return result;
			}
			ReadOnlyCollection<CcpXcpSignal> activeSignals = signalConfiguration.ActiveSignals;
			using (IEnumerator<uint> enumerator = canChannelConfig.GetActiveCANChannelNumbers().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					uint channelNumber = enumerator.Current;
					if (channelNumber == database.ChannelNumber.Value)
					{
						Dictionary<uint, List<IValidatedProperty>> dictionary = new Dictionary<uint, List<IValidatedProperty>>();
						foreach (Database current in from db in databaseConfiguration.ActiveCCPXCPDatabases
						where db.FileType == DatabaseFileType.A2L && db.ChannelNumber.Value == channelNumber
						select db)
						{
							if (current.CcpXcpEcuList.Any<CcpXcpEcu>())
							{
								Database currentDatabase = current;
								if (string.Compare(database.FilePath.Value, current.FilePath.Value, StringComparison.OrdinalIgnoreCase) == 0)
								{
									currentDatabase = database;
								}
								uint key;
								if (CcpXcpManager.Instance().GetCanRequestId(currentDatabase, out key))
								{
									if (!dictionary.ContainsKey(key))
									{
										dictionary.Add(key, new List<IValidatedProperty>());
									}
									dictionary[key].Add(currentDatabase.CcpXcpEcuList[0].CanRequestIDValidatedProperty);
								}
								uint key2;
								if (CcpXcpManager.Instance().GetCanResponseId(currentDatabase, out key2))
								{
									if (!dictionary.ContainsKey(key2))
									{
										dictionary.Add(key2, new List<IValidatedProperty>());
									}
									dictionary[key2].Add(currentDatabase.CcpXcpEcuList[0].CanResponseIDValidatedProperty);
								}
								uint key3;
								if (activeSignals.Any((CcpXcpSignal sig) => sig.EcuName.Value == currentDatabase.CcpXcpEcuList[0].CcpXcpEcuDisplayName && sig.MeasurementMode.Value == CcpXcpMeasurementMode.Polling) && CcpXcpManager.Instance().GetCanFirstID(currentDatabase, out key3))
								{
									if (!dictionary.ContainsKey(key3))
									{
										dictionary.Add(key3, new List<IValidatedProperty>());
									}
									dictionary[key3].Add(currentDatabase.CcpXcpEcuList[0].CanFirstIDValidatedProperty);
								}
							}
						}
						foreach (KeyValuePair<uint, List<IValidatedProperty>> current2 in from idPropertyList in dictionary
						where idPropertyList.Value.Count<IValidatedProperty>() > 1
						select idPropertyList)
						{
							foreach (IValidatedProperty current3 in current2.Value)
							{
								resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current3, Resources.ErrorIdAlreadyUsedByAnotherSettingOrEcu);
							}
							if (database.CcpXcpEcuList[0].CanFirstID == current2.Key || database.CcpXcpEcuList[0].CanRequestID == current2.Key || database.CcpXcpEcuList[0].CanResponseID == current2.Key)
							{
								result = false;
							}
						}
					}
				}
			}
			return result;
		}

		private bool ValidateCcpXcpEcuNax(Database database, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (database.FileType != DatabaseFileType.A2L || database.BusType.Value != BusType.Bt_FlexRay || !database.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return true;
			}
			uint nodeAdress1;
			if (!CcpXcpManager.Instance().GetNodeAddress(database, out nodeAdress1))
			{
				return true;
			}
			uint nodeAdress2;
			if (this.DatabaseConfiguration.ActiveCCPXCPDatabases.Any((Database db) => db.FileType == DatabaseFileType.A2L && database.BusType.Value == BusType.Bt_FlexRay && string.Compare(db.FilePath.Value, database.FilePath.Value, true, CultureInfo.InvariantCulture) != 0 && db.CcpXcpEcuList.Any<CcpXcpEcu>() && CcpXcpManager.Instance().GetNodeAddress(db, out nodeAdress2) && nodeAdress1 == nodeAdress2))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.CcpXcpEcuList[0].FlexRayXcpNodeAdressValidatedProperty, Resources.ErrorDuplicatedNodeAddress);
				result = false;
			}
			return result;
		}

		private bool ValidateCcpXcpEcuIPConfig(Database database, EthernetConfiguration ethernetConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (database == null || ethernetConfiguration == null)
			{
				return false;
			}
			if (database.CcpXcpEcuList != null && database.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				IPAddress iPAddress;
				if (!CcpXcpManager.Instance().GetHostIp(database, out iPAddress))
				{
					return false;
				}
				uint num;
				if (!CcpXcpManager.Instance().GetHostPort(database, out num))
				{
					return false;
				}
				uint num2;
				if (!CcpXcpManager.Instance().GetHostPort2(database, out num2))
				{
					return false;
				}
				IPAddress obj;
				if (!IPAddress.TryParse(ethernetConfiguration.Eth1Ip.Value, out obj))
				{
					return false;
				}
				if (iPAddress.Equals(obj))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.CcpXcpEcuList[0].EthernetHostValidatedProperty, Resources.ErrorLoggerAndEcuIpMustDiffer);
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, ethernetConfiguration.Eth1Ip, Resources.ErrorLoggerAndEcuIpMustDiffer);
					result = false;
				}
				if (database.CcpXcpEcuList[0].UseVxModule && num == num2)
				{
					PageValidator pageValidator = resultCollector as PageValidator;
					if (pageValidator == null || (!pageValidator.General.HasError(database.CcpXcpEcuList[0].EthernetPortValidatedProperty) && !pageValidator.General.HasError(database.CcpXcpEcuList[0].EthernetPort2ValidatedProperty)))
					{
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, database.CcpXcpEcuList[0].EthernetPortValidatedProperty, Resources.ErrorDifferentPortNumbers);
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, database.CcpXcpEcuList[0].EthernetPort2ValidatedProperty, Resources.ErrorDifferentPortNumbers);
					}
					result = false;
				}
				foreach (Database current in from db in this.DatabaseConfiguration.ActiveCCPXCPDatabases
				where db.FileType == DatabaseFileType.A2L && db.CcpXcpEcuList != null && db.CcpXcpEcuList.Any<CcpXcpEcu>() && (db.BusType.Value == BusType.Bt_Ethernet || db.CcpXcpEcuList[0].UseVxModule) && string.Compare(db.FilePath.Value, database.FilePath.Value, true, CultureInfo.InvariantCulture) != 0
				select db)
				{
					IPAddress obj2;
					uint num3;
					uint num4;
					if (CcpXcpManager.Instance().GetHostIp(current, out obj2) && CcpXcpManager.Instance().GetHostPort(current, out num3) && CcpXcpManager.Instance().GetHostPort2(current, out num4) && iPAddress.Equals(obj2) && num == num3)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.CcpXcpEcuList[0].EthernetHostValidatedProperty, Resources.ErrorIpAlreadyInUseByAnotherECU);
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.CcpXcpEcuList[0].EthernetPortValidatedProperty, Resources.ErrorPortAlreadyInUseByAnotherECU);
						result = false;
					}
				}
			}
			return result;
		}

		private bool ValidateDiagnosticsDatabaseConfiguration(DiagnosticsDatabaseConfiguration databaseConfig, CANChannelConfiguration canChannelConfig, string configFolderPath, IDiagSymbolsManager dsManager, IModelValidationResultCollector resultCollector)
		{
			if (databaseConfig == null)
			{
				return true;
			}
			bool flag = true;
			foreach (DiagnosticsECU current in databaseConfig.ECUs)
			{
				flag &= this.ValidateDatabaseFileExistence(current.Database, configFolderPath, resultCollector);
				flag &= this.ValidateEcuAndVariantExistence(current, configFolderPath, dsManager, resultCollector);
				flag &= this.ValidateDiagECUChannel(current, canChannelConfig, resultCollector);
			}
			flag &= this.ValidateOverallCommParamsRequestResponseIds(databaseConfig, resultCollector, true);
			flag &= this.ValidateOverallCommParamsSessionSettings(databaseConfig, resultCollector, true);
			return flag;
		}

		private bool ValidateDatabaseFileExistence(DiagnosticsDatabase database, string configFolderPath, IModelValidationResultCollector resultCollector)
		{
			string absolutePath = FileSystemServices.GetAbsolutePath(database.FilePath.Value, configFolderPath);
			try
			{
				if (!File.Exists(absolutePath))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.FilePath, Resources.ErrorFileNotFound);
					bool result = false;
					return result;
				}
			}
			catch (Exception)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, database.FilePath, Resources.ErrorFileNotFound);
				bool result = false;
				return result;
			}
			return true;
		}

		private bool ValidateEcuAndVariantExistence(DiagnosticsECU ecu, string configFolderPath, IDiagSymbolsManager dsManager, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!dsManager.ResolveEcuQualifier(this.configManager.GetAbsoluteFilePath(ecu.Database.FilePath.Value), ecu.Qualifier.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, ecu.Qualifier, Resources.ErrorEcuNotFound);
				result = false;
			}
			if (!dsManager.ResolveEcuVariant(this.configManager.GetAbsoluteFilePath(ecu.Database.FilePath.Value), ecu.Qualifier.Value, ecu.Variant.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, ecu.Variant, Resources.ErrorEcuVariantNotFound);
				result = false;
			}
			return result;
		}

		private List<string> GetUndefinedEcuQualifiersOfDiagDatabase(DiagnosticsDatabase database, string configFolderPath, IDiagSymbolsManager dsManager)
		{
			List<string> list = new List<string>();
			foreach (DiagnosticsECU current in database.ECUs)
			{
				if (!dsManager.ResolveEcuQualifier(this.configManager.GetAbsoluteFilePath(current.Database.FilePath.Value), current.Qualifier.Value))
				{
					list.Add(current.Qualifier.Value);
				}
			}
			return list;
		}

		private bool ValidateDiagECUChannel(DiagnosticsECU ecu, CANChannelConfiguration canChannelConfig, IModelValidationResultCollector resultCollector)
		{
			if (!canChannelConfig.GetCANChannel(ecu.ChannelNumber.Value).IsActive.Value)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, ecu.ChannelNumber, Resources.ErrorChannelInactive);
				return false;
			}
			return true;
		}

		private bool ValidateOverallCommParamsRequestResponseIds(IDiagnosticsDatabaseConfiguration config, IModelValidationResultCollector resultCollector, bool useSingleErrorPerEcu)
		{
			Dictionary<uint, Dictionary<uint, List<DiagnosticCommParamsECU>>> dictionary = new Dictionary<uint, Dictionary<uint, List<DiagnosticCommParamsECU>>>();
			Dictionary<uint, Dictionary<uint, List<DiagnosticCommParamsECU>>> dictionary2 = new Dictionary<uint, Dictionary<uint, List<DiagnosticCommParamsECU>>>();
			foreach (DiagnosticsECU current in config.ECUs)
			{
				Dictionary<uint, List<DiagnosticCommParamsECU>> dictionary3;
				if (!dictionary.ContainsKey(current.ChannelNumber.Value))
				{
					dictionary3 = new Dictionary<uint, List<DiagnosticCommParamsECU>>();
					dictionary.Add(current.ChannelNumber.Value, dictionary3);
				}
				else
				{
					dictionary3 = dictionary[current.ChannelNumber.Value];
				}
				uint num = current.DiagnosticCommParamsECU.PhysRequestMsgId.Value;
				if (current.DiagnosticCommParamsECU.PhysRequestMsgIsExtendedId.Value)
				{
					num |= Constants.CANDbIsExtendedIdMask;
				}
				if (!dictionary3.ContainsKey(num))
				{
					dictionary3.Add(num, new List<DiagnosticCommParamsECU>
					{
						current.DiagnosticCommParamsECU
					});
				}
				else
				{
					dictionary3[num].Add(current.DiagnosticCommParamsECU);
				}
				Dictionary<uint, List<DiagnosticCommParamsECU>> dictionary4;
				if (!dictionary2.ContainsKey(current.ChannelNumber.Value))
				{
					dictionary4 = new Dictionary<uint, List<DiagnosticCommParamsECU>>();
					dictionary2.Add(current.ChannelNumber.Value, dictionary4);
				}
				else
				{
					dictionary4 = dictionary2[current.ChannelNumber.Value];
				}
				uint num2 = current.DiagnosticCommParamsECU.ResponseMsgId.Value;
				if (current.DiagnosticCommParamsECU.ResponseMsgIsExtendedId.Value)
				{
					num2 |= Constants.CANDbIsExtendedIdMask;
				}
				if (!dictionary4.ContainsKey(num2))
				{
					dictionary4.Add(num2, new List<DiagnosticCommParamsECU>
					{
						current.DiagnosticCommParamsECU
					});
				}
				else
				{
					dictionary4[num2].Add(current.DiagnosticCommParamsECU);
				}
			}
			bool result = true;
			for (uint num3 = 1u; num3 <= this.LoggerSpecifics.CAN.NumberOfChannels; num3 += 1u)
			{
				if (dictionary.ContainsKey(num3) && dictionary2.ContainsKey(num3))
				{
					Dictionary<uint, List<DiagnosticCommParamsECU>> dictionary3 = dictionary[num3];
					Dictionary<uint, List<DiagnosticCommParamsECU>> dictionary4 = dictionary2[num3];
					foreach (uint current2 in dictionary3.Keys)
					{
						if (dictionary3[current2].Count > 1)
						{
							foreach (DiagnosticCommParamsECU current3 in dictionary3[current2])
							{
								if (useSingleErrorPerEcu)
								{
									resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current3.PhysRequestMsgId, Resources.ErrorInvalidEcuSettings);
								}
								else
								{
									resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current3.PhysRequestMsgId, Resources.ErrorReqIdUsedInOtherEcu);
								}
								result = false;
							}
						}
						if (dictionary4.ContainsKey(current2))
						{
							foreach (DiagnosticCommParamsECU current4 in dictionary4[current2])
							{
								if (useSingleErrorPerEcu)
								{
									resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current4.ResponseMsgId, Resources.ErrorInvalidEcuSettings);
								}
								else
								{
									resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current4.ResponseMsgId, Resources.ErrorRespIdUsedInOtherEcu);
								}
							}
							foreach (DiagnosticCommParamsECU current5 in dictionary3[current2])
							{
								if (dictionary4[current2].Contains(current5))
								{
									if (useSingleErrorPerEcu)
									{
										resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current5.PhysRequestMsgId, Resources.ErrorInvalidEcuSettings);
									}
									else
									{
										resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current5.PhysRequestMsgId, Resources.ErrorIdenticalReqRespCanIdsInEcu);
										resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current5.ResponseMsgId, Resources.ErrorIdenticalReqRespCanIdsInEcu);
									}
								}
								else if (useSingleErrorPerEcu)
								{
									resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current5.PhysRequestMsgId, Resources.ErrorInvalidEcuSettings);
								}
								else
								{
									resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current5.PhysRequestMsgId, Resources.ErrorReqIdUsedInOtherEcu);
								}
							}
							dictionary4.Remove(current2);
							result = false;
						}
					}
					foreach (uint current6 in dictionary4.Keys)
					{
						if (dictionary4[current6].Count > 1)
						{
							foreach (DiagnosticCommParamsECU current7 in dictionary4[current6])
							{
								if (useSingleErrorPerEcu)
								{
									resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current7.ResponseMsgId, Resources.ErrorInvalidEcuSettings);
								}
								else
								{
									resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current7.ResponseMsgId, Resources.ErrorRespIdUsedInOtherEcu);
								}
							}
							result = false;
						}
					}
				}
			}
			return result;
		}

		private bool ValidateOverallCommParamsSessionSettings(IDiagnosticsDatabaseConfiguration config, IModelValidationResultCollector resultCollector, bool useSingleErrorPerEcu)
		{
			bool result = true;
			foreach (DiagnosticsECU current in config.ECUs)
			{
				if (current.DiagnosticCommParamsECU.DefaultSessionId.Value == current.DiagnosticCommParamsECU.ExtendedSessionId.Value)
				{
					if (useSingleErrorPerEcu)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.DiagnosticCommParamsECU.DefaultSessionId, Resources.ErrorInvalidEcuSettings);
					}
					else
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.DiagnosticCommParamsECU.DefaultSessionId, Resources.ErrorSessionBytesMustDiffer);
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.DiagnosticCommParamsECU.ExtendedSessionId, Resources.ErrorSessionBytesMustDiffer);
					}
					result = false;
				}
			}
			return result;
		}

		private bool ValidateIndependentDiagCommParamsEcu(DiagnosticCommParamsECU commParamsEcu, IModelValidationResultCollector resultCollector)
		{
			resultCollector.ResetAllModelErrors();
			bool result = true;
			if (commParamsEcu.P2Timeout.Value > Constants.MaximumP2Timeout || commParamsEcu.P2Timeout.Value < Constants.MinimumP2Timeout)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, commParamsEcu.P2Timeout, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinimumP2Timeout, Constants.MaximumP2Timeout));
				result = false;
			}
			if (commParamsEcu.P2Extension.Value > Constants.MaximumP2Extension || commParamsEcu.P2Extension.Value < Constants.MinimumP2Extension)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, commParamsEcu.P2Extension, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinimumP2Extension, Constants.MinimumP2Extension));
				result = false;
			}
			if (commParamsEcu.STMin.Value > Constants.MaximumSTMin || commParamsEcu.STMin.Value < Constants.MinimumSTMin)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, commParamsEcu.STMin, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinimumSTMin, Constants.MaximumSTMin));
				result = false;
			}
			if ((commParamsEcu.DiagProtocol.Value == DiagnosticsProtocolType.KWP && commParamsEcu.DefaultSessionSource.Value == DiagnosticsSessionSource.UDS_Default) || (commParamsEcu.DiagProtocol.Value == DiagnosticsProtocolType.UDS && commParamsEcu.DefaultSessionSource.Value == DiagnosticsSessionSource.KWP_Default))
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, commParamsEcu.DefaultSessionSource, Resources.ErrorSessionDiagProtMismatch);
				result = false;
			}
			if (commParamsEcu.DiagProtocol.Value == DiagnosticsProtocolType.KWP && commParamsEcu.ExtendedSessionSource.Value == DiagnosticsSessionSource.UDS_Extended)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, commParamsEcu.ExtendedSessionSource, Resources.ErrorSessionDiagProtMismatch);
				result = false;
			}
			if (commParamsEcu.DefaultSessionId.Value == 0uL)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, commParamsEcu.DefaultSessionId, Resources.ErrorSessionIdMustNotBeZero);
				result = false;
			}
			if (commParamsEcu.ExtendedSessionId.Value == 0uL)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, commParamsEcu.ExtendedSessionId, Resources.ErrorSessionIdMustNotBeZero);
				result = false;
			}
			return result;
		}

		private bool ValidateDiagnosticActionsConfiguration(DiagnosticActionsConfiguration actionsConfig, DiagnosticsDatabaseConfiguration diagDatabasesConfig, DatabaseConfiguration databaseConfig, string configFolderPath, IDiagSymbolsManager dsManager, IApplicationDatabaseManager appDbManager, DigitalOutputsConfiguration digitalOutputsConfig, IModelValidationResultCollector resultCollector)
		{
			if (actionsConfig == null)
			{
				return true;
			}
			bool result = true;
			result = this.ValidateDiagnosticActionSequenceEvents(actionsConfig, databaseConfig, appDbManager, configFolderPath, digitalOutputsConfig, resultCollector);
			DiagnosticsDatabase diagnosticsDatabase = null;
			DiagnosticsECU diagnosticsECU = null;
			foreach (DiagnosticAction current in from da in actionsConfig.DiagnosticActions
			where !(da is DiagnosticDummyAction)
			select da)
			{
				bool flag = false;
				if (!diagDatabasesConfig.TryGetDiagnosticsDatabase(current.DatabasePath.Value, out diagnosticsDatabase))
				{
					flag = true;
				}
				if (!flag)
				{
					string absolutePath = FileSystemServices.GetAbsolutePath(diagnosticsDatabase.FilePath.Value, configFolderPath);
					try
					{
						if (!File.Exists(absolutePath))
						{
							flag = true;
						}
					}
					catch (Exception)
					{
						flag = true;
					}
				}
				if (!flag && !diagnosticsDatabase.TryGetECU(current.EcuQualifier.Value, out diagnosticsECU))
				{
					flag = true;
				}
				if (!flag && !dsManager.ResolveEcuQualifier(this.configManager.GetAbsoluteFilePath(diagnosticsECU.Database.FilePath.Value), diagnosticsECU.Qualifier.Value))
				{
					flag = true;
				}
				if (flag)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.EcuQualifier, Resources.ErrorEcuNotFound);
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.ServiceQualifier, Resources.ErrorUnableToResolveService);
					if (current is DiagnosticSignalRequest)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, (current as DiagnosticSignalRequest).SignalQualifier, Resources.ErrorUnableToResolveSignal);
					}
					result = false;
				}
				else
				{
					bool hasOnlyConstParams = false;
					if (!dsManager.ResolveService(this.configManager.GetAbsoluteFilePath(diagnosticsDatabase.FilePath.Value), diagnosticsECU.Qualifier.Value, diagnosticsECU.Variant.Value, current.ServiceQualifier.Value, out hasOnlyConstParams))
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.ServiceQualifier, Resources.ErrorUnableToResolveService);
						if (current is DiagnosticSignalRequest)
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, (current as DiagnosticSignalRequest).SignalQualifier, Resources.ErrorUnableToResolveSignal);
						}
						result = false;
					}
					else
					{
						current.HasOnlyConstParams = hasOnlyConstParams;
						DiagnosticSignalRequest diagnosticSignalRequest = current as DiagnosticSignalRequest;
						if (diagnosticSignalRequest != null && !dsManager.ResolveSignal(this.configManager.GetAbsoluteFilePath(diagnosticsDatabase.FilePath.Value), diagnosticsECU.Qualifier.Value, diagnosticsECU.Variant.Value, diagnosticSignalRequest.DidId.Value, diagnosticSignalRequest.SignalQualifier.Value))
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, diagnosticSignalRequest.SignalQualifier, Resources.ErrorUnableToResolveSignal);
							result = false;
						}
					}
				}
			}
			return result;
		}

		private bool ValidateDiagnosticActionSequenceEvents(DiagnosticActionsConfiguration actionsConfig, DatabaseConfiguration databasesConfig, IApplicationDatabaseManager databaseManager, string configFolderPath, DigitalOutputsConfiguration digitalOutputsConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			foreach (TriggeredDiagnosticActionSequence current in actionsConfig.TriggeredActionSequences)
			{
				if (current.Event is CANIdEvent)
				{
					result = this.ValidateCANIdEventChannel(current.Event as CANIdEvent, resultCollector);
				}
				else if (current.Event is LINIdEvent)
				{
					result = this.ValidateLINIdEventChannel(current.Event as LINIdEvent, resultCollector);
				}
				else if (current.Event is FlexrayIdEvent)
				{
					result = this.ValidateFlexrayIdEventChannel(current.Event as FlexrayIdEvent, resultCollector);
				}
				else if (current.Event is CANDataEvent)
				{
					result = this.ValidateCANDataEventChannel(current.Event as CANDataEvent, resultCollector);
				}
				else if (current.Event is LINDataEvent)
				{
					result = this.ValidateLINDataEventChannel(current.Event as LINDataEvent, resultCollector);
				}
				else if (current.Event is SymbolicMessageEvent)
				{
					result = this.ValidateSymbolicMessageEvent(current.Event as SymbolicMessageEvent, databasesConfig, configFolderPath, databaseManager, resultCollector);
				}
				else if (current.Event is ISymbolicSignalEvent)
				{
					result = this.ValidateSymbolicSignalEvent(current.Event as ISymbolicSignalEvent, databasesConfig, configFolderPath, databaseManager, resultCollector);
				}
				else if (current.Event is MsgTimeoutEvent)
				{
					result = this.ValidateMsgTimeoutEvent(current.Event as MsgTimeoutEvent, databasesConfig, configFolderPath, databaseManager, resultCollector);
				}
				else if (current.Event is DigitalInputEvent)
				{
					result = this.ValidateDigitalInputEvent(current.Event as DigitalInputEvent, digitalOutputsConfiguration, resultCollector);
				}
				else if (current.Event is KeyEvent)
				{
					result = this.ValidateKeyEvent(current.Event as KeyEvent, resultCollector);
				}
			}
			return result;
		}

		private bool ValidateDiagnosticActionsGeneralSettings(DiagnosticActionsConfiguration actionsConfig, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			resultCollector.ResetAllModelErrors();
			if (actionsConfig.IsDiagCommRestartEnabled.Value && (actionsConfig.DiagCommRestartTime.Value < Constants.MinimumDiagCommRestartTime || actionsConfig.DiagCommRestartTime.Value > Constants.MaximumDiagCommRestartTime))
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, actionsConfig.DiagCommRestartTime, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinimumDiagCommRestartTime, Constants.MaximumDiagCommRestartTime));
				result = false;
			}
			if (this.LoggerSpecifics.DataStorage.NumberOfMemories > 1u)
			{
				bool flag = false;
				int num = 0;
				while ((long)num < (long)((ulong)this.LoggerSpecifics.DataStorage.NumberOfMemories))
				{
					if (actionsConfig.recordDiagCommOnMemories[num].Value)
					{
						flag = true;
						break;
					}
					num++;
				}
				if (!flag)
				{
					int num2 = 0;
					while ((long)num2 < (long)((ulong)this.LoggerSpecifics.DataStorage.NumberOfMemories))
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, actionsConfig.recordDiagCommOnMemories[num2], Resources.ErrorAtLeastOneMemoryActive);
						result = false;
						num2++;
					}
				}
			}
			return result;
		}

		private bool ValidateFilterConfiguration(FilterConfiguration filterConfiguration, DatabaseConfiguration databaseConfiguration, string configFolderPath, IApplicationDatabaseManager databaseManager, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			bool flag2 = false;
			UsedLimitFilterIdsCount.Update(filterConfiguration, databaseManager, configFolderPath, out flag2);
			foreach (Filter current in filterConfiguration.Filters)
			{
				if (current.IsActive.Value)
				{
					if (flag2 && current.IsActive.Value && current.Action.Value == FilterActionType.Limit)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.Action, string.Format(Resources.ErrorNumOfIdsLimitFiltersExceeded, UsedLimitFilterIdsCount.MaxNumberOfIdsInLimitFilters));
						flag = false;
					}
					if (current is CANIdFilter)
					{
						flag &= this.ValidateCANIdFilterChannel(current as CANIdFilter, resultCollector);
					}
					else if (current is LINIdFilter)
					{
						flag &= this.ValidateLINIdFilterChannel(current as LINIdFilter, resultCollector);
					}
					else if (current is FlexrayIdFilter)
					{
						flag &= this.ValidateFlexrayIdFilterChannel(current as FlexrayIdFilter, resultCollector);
					}
					else if (current is ChannelFilter)
					{
						flag &= this.ValidateChannelFilter(current as ChannelFilter, resultCollector);
					}
					else if (current is SymbolicMessageFilter)
					{
						flag &= this.ValidateSymbolicFilter(current as SymbolicMessageFilter, databaseConfiguration, configFolderPath, databaseManager, resultCollector);
					}
					else if (current is SignalListFileFilter)
					{
						flag &= this.ValidateSignalListFileFilter(current as SignalListFileFilter, this.CANChannelConfiguration, this.LINChannelConfiguration, databaseConfiguration, configFolderPath, resultCollector);
					}
				}
			}
			return flag;
		}

		private bool ValidateCANIdFilterChannel(CANIdFilter filter, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!this.IsHardwareChannelAvailable(BusType.Bt_CAN, filter.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.IsHardwareChannelActive(BusType.Bt_CAN, filter.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorChannelInactive);
				result = false;
			}
			return result;
		}

		private bool ValidateLINIdFilterChannel(LINIdFilter filter, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!this.IsHardwareChannelAvailable(BusType.Bt_LIN, filter.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.IsHardwareChannelActive(BusType.Bt_LIN, filter.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorChannelInactive);
				result = false;
			}
			return result;
		}

		private bool ValidateFlexrayIdFilterChannel(FlexrayIdFilter filter, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!this.IsHardwareChannelAvailable(BusType.Bt_FlexRay, filter.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.IsHardwareChannelActive(BusType.Bt_FlexRay, filter.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorChannelInactive);
				result = false;
			}
			return result;
		}

		private bool ValidateChannelFilter(ChannelFilter filter, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!this.IsHardwareChannelAvailable(filter.BusType.Value, filter.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.IsHardwareChannelActive(filter.BusType.Value, filter.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorChannelInactive);
				result = false;
			}
			return result;
		}

		private bool ValidateSymbolicFilter(SymbolicMessageFilter filter, DatabaseConfiguration databaseConfiguration, string configFolderPath, IApplicationDatabaseManager databaseManager, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!this.IsHardwareChannelAvailable(filter.BusType.Value, filter.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.IsHardwareChannelActive(filter.BusType.Value, filter.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorChannelInactive);
				result = false;
			}
			string absolutePath = FileSystemServices.GetAbsolutePath(filter.DatabasePath.Value, configFolderPath);
			Database database;
			MessageDefinition messageDefinition;
			if (!databaseConfiguration.TryGetDatabase(filter.DatabasePath.Value, filter.NetworkName.Value, filter.ChannelNumber.Value, filter.BusType.Value, out database) || !databaseManager.ResolveMessageSymbolInDatabase(absolutePath, filter.NetworkName.Value, filter.MessageName.Value, filter.BusType.Value, out messageDefinition))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.MessageName, Resources.ErrorUnresolvedMsgSymbol);
				result = false;
			}
			if (BusType.Bt_FlexRay == filter.BusType.Value && database != null && !this.ValidateSymbolicMessageChannelFromFlexrayDb(filter.MessageName.Value, filter.ChannelNumber.Value, database))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.MessageName, Resources.ErrorUnresolvedMsgSymbolAtChn);
				result = false;
			}
			return result;
		}

		private bool ValidateSignalListFileFilter(SignalListFileFilter filter, CANChannelConfiguration canChannelConfiguration, LINChannelConfiguration linChannelConfiguration, DatabaseConfiguration databaseConfiguration, string configFolderPath, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			string absolutePath = FileSystemServices.GetAbsolutePath(filter.FilePath.Value, configFolderPath);
			FileInfo fileInfo = new FileInfo(absolutePath);
			if (!fileInfo.Exists)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.FilePath, Resources.ErrorFileNotFound);
				result = false;
			}
			if (BusType.Bt_CAN == filter.BusType.Value)
			{
				if ((long)canChannelConfiguration.CANChannels.Count < (long)((ulong)(filter.ChannelNumber.Value - 1u)) || !canChannelConfiguration.GetCANChannel(filter.ChannelNumber.Value).IsActive.Value)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorChannelInactive);
					result = false;
				}
				else if (!databaseConfiguration.IsDatabaseAvailableFor(BusType.Bt_CAN, filter.ChannelNumber.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorNoCANDatabaseOnChannel);
					result = false;
				}
			}
			else if (BusType.Bt_LIN == filter.BusType.Value)
			{
				if ((long)linChannelConfiguration.LINChannels.Count < (long)((ulong)(filter.ChannelNumber.Value - 1u)) || !linChannelConfiguration.GetLINChannel(filter.ChannelNumber.Value).IsActive.Value)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorChannelInactive);
					result = false;
				}
				else if (!databaseConfiguration.IsDatabaseAvailableFor(BusType.Bt_LIN, filter.ChannelNumber.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, filter.ChannelNumber, Resources.ErrorNoLINDatabaseOnChannel);
					result = false;
				}
			}
			return result;
		}

		private bool ValidateRingBufferSettings(TriggerConfiguration triggerConfiguration, IModelValidationResultCollector resultCollector)
		{
			if (triggerConfiguration.MemoryRingBuffer.IsActive.Value)
			{
				bool result = true;
				if (RingBufferMemoriesManager.MinRingBufferSizeKB > (long)((ulong)triggerConfiguration.MemoryRingBuffer.Size.Value) || RingBufferMemoriesManager.MaxSumRingBufferSizesKB < (long)((ulong)triggerConfiguration.MemoryRingBuffer.Size.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, triggerConfiguration.MemoryRingBuffer.Size, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, GUIUtil.GetFractionedMByteDisplayString((uint)RingBufferMemoriesManager.MinRingBufferSizeKB), GUIUtil.GetFractionedMByteDisplayString((uint)RingBufferMemoriesManager.MaxSumRingBufferSizesKB)));
					result = false;
				}
				else if (RingBufferMemoriesManager.GetMaxAvailableRingBufferSize(triggerConfiguration.MemoryNr) < (long)((ulong)triggerConfiguration.MemoryRingBuffer.Size.Value))
				{
					RingBufferMemoriesManager.SetMemoryRingBufferSize(triggerConfiguration.MemoryNr, triggerConfiguration.MemoryRingBuffer.Size.Value);
					if (RingBufferMemoriesManager.GetMaxAvailableRingBufferSize(triggerConfiguration.MemoryNr) > 0L)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, triggerConfiguration.MemoryRingBuffer.Size, string.Format(Resources.ErrorRingBufferOutOfRange, GUIUtil.GetFractionedMByteDisplayString(this.LoggerSpecifics.DataStorage.MinRingBufferSize), GUIUtil.GetFractionedMByteDisplayString((uint)RingBufferMemoriesManager.GetMaxAvailableRingBufferSize(triggerConfiguration.MemoryNr))));
					}
					else
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, triggerConfiguration.MemoryRingBuffer.Size, Resources.ErrorRingBufferOutOfRangeOtherMustReduce);
					}
					result = false;
				}
				else
				{
					RingBufferMemoriesManager.SetMemoryRingBufferSize(triggerConfiguration.MemoryNr, triggerConfiguration.MemoryRingBuffer.Size.Value);
				}
				if (triggerConfiguration.MemoryRingBuffer.MaxNumberOfFiles.Value < this.LoggerSpecifics.DataStorage.MinLoggerFiles || triggerConfiguration.MemoryRingBuffer.MaxNumberOfFiles.Value > this.LoggerSpecifics.DataStorage.MaxLoggerFiles)
				{
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, triggerConfiguration.MemoryRingBuffer.MaxNumberOfFiles, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, this.LoggerSpecifics.DataStorage.MinLoggerFiles, this.LoggerSpecifics.DataStorage.MaxLoggerFiles));
					result = false;
				}
				if (triggerConfiguration.MemoryRingBuffer.OperatingMode.Value == RingBufferOperatingMode.stopLogging && this.WLANConfiguration.IsStartThreeGOnEventEnabled.Value)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, triggerConfiguration.MemoryRingBuffer.OperatingMode, Resources.ErrorLoggingModeAnd3GNotAllowed);
					result = false;
				}
				return result;
			}
			RingBufferMemoriesManager.SetMemoryRingBufferSize(triggerConfiguration.MemoryNr, 0u);
			if (RingBufferMemoriesManager.AreAllMemoriesInactive())
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, triggerConfiguration.MemoryRingBuffer.IsActive, Resources.ErrorAllMemoriesInactive);
				return false;
			}
			return true;
		}

		private bool ValidatePostTriggerTime(TriggerConfiguration triggerConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (triggerConfiguration.TriggerMode.Value == TriggerMode.Triggered && triggerConfiguration.PostTriggerTime.Value > Constants.MaximumPostTriggerTime)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, triggerConfiguration.PostTriggerTime, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 0, Constants.MaximumPostTriggerTime));
				result = false;
			}
			return result;
		}

		private bool ValidateTriggerList(TriggerConfiguration triggerConfiguration, DatabaseConfiguration databaseConfiguration, string configFolderPath, IApplicationDatabaseManager databaseManager, DigitalOutputsConfiguration digitalOutputsConfiguration, SendMessageConfiguration sendMessageConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			ReadOnlyCollection<RecordTrigger> readOnlyCollection;
			if (triggerConfiguration.TriggerMode.Value == TriggerMode.Permanent)
			{
				readOnlyCollection = triggerConfiguration.PermanentMarkers;
			}
			else if (triggerConfiguration.TriggerMode.Value == TriggerMode.OnOff)
			{
				readOnlyCollection = triggerConfiguration.OnOffTriggers;
			}
			else
			{
				readOnlyCollection = triggerConfiguration.Triggers;
			}
			foreach (RecordTrigger current in readOnlyCollection)
			{
				if (current.IsActive.Value)
				{
					flag &= this.ValidateTriggerComment(current, resultCollector);
					flag &= this.ValidateTriggerName(current, triggerConfiguration, resultCollector);
					flag &= this.ValidateEvent(current.Event, databaseConfiguration, configFolderPath, databaseManager, this.LoggerSpecifics.IO.IsDigitalInputOutputCommonPin ? digitalOutputsConfiguration : null, resultCollector);
				}
			}
			return flag;
		}

		private bool ValidateTriggerComment(RecordTrigger trigger, IModelValidationResultCollector resultCollector)
		{
			if (string.IsNullOrEmpty(trigger.Comment.Value))
			{
				return true;
			}
			if (trigger.Comment.Value.IndexOfAny(ModelValidator.invalidTriggerCommentChars) >= 0)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, trigger.Comment, string.Format(Resources.InvalidCommentString, "Comment"));
				return false;
			}
			return true;
		}

		private bool ValidateTriggerName(RecordTrigger trigger, TriggerConfiguration triggerConfiguration, IModelValidationResultCollector resultCollector)
		{
			if (trigger.TriggerEffect.Value != TriggerEffect.Marker && triggerConfiguration.TriggerMode.Value != TriggerMode.Triggered)
			{
				return true;
			}
			if (string.IsNullOrEmpty(trigger.Name.Value) || string.IsNullOrEmpty(trigger.Name.Value.Trim()))
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, trigger.Name, string.Format(Resources_Trigger.InvalidTriggerNameEmpty, new object[0]));
				return false;
			}
			if (trigger.Name.Value.Length >= 200)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, trigger.Name, string.Format(Resources_Trigger.InvalidTriggerNameTooLong, 200));
				return false;
			}
			if (!ModelValidator.triggerNameRegexWhitelist.IsMatch(trigger.Name.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, trigger.Name, string.Format(Resources_Trigger.InvalidTriggerNameStringWhitelist, trigger.Name.Value, ModelValidator.triggerNameWhitelistValidCharacterList));
				return false;
			}
			if (this.IsActiveTriggerEventNameUsedMoreThanOnce(trigger.Name.Value))
			{
				string errorText = string.Empty;
				if (trigger.TriggerEffect.Value == TriggerEffect.Marker)
				{
					errorText = string.Format(Resources.InvalidMarkerNameDuplicate, trigger.Name.Value);
				}
				else
				{
					errorText = string.Format(Resources_Trigger.InvalidTriggerNameDuplicate, trigger.Name.Value);
				}
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, trigger.Name, errorText);
				return false;
			}
			return true;
		}

		private bool IsActiveTriggerEventNameUsedMoreThanOnce(string eventName)
		{
			int num = 0;
			foreach (TriggerConfiguration current in this.TriggerConfigurations)
			{
				ReadOnlyCollection<RecordTrigger> readOnlyCollection;
				if (current.TriggerMode.Value == TriggerMode.OnOff)
				{
					readOnlyCollection = current.OnOffTriggers;
				}
				else if (current.TriggerMode.Value == TriggerMode.Permanent)
				{
					readOnlyCollection = current.PermanentMarkers;
				}
				else
				{
					readOnlyCollection = current.Triggers;
				}
				foreach (RecordTrigger current2 in readOnlyCollection)
				{
					if (current2.IsActive.Value && current2.Name.Value == eventName)
					{
						num++;
					}
				}
			}
			return num > 1;
		}

		private bool ValidateEvent(Event ev, DatabaseConfiguration databaseConfiguration, string configFolderPath, IApplicationDatabaseManager databaseManager, DigitalOutputsConfiguration digitalOutputsConfiguration, IModelValidationResultCollector resultCollector)
		{
			if (ev is CANIdEvent)
			{
				return this.ValidateCANIdEventChannel(ev as CANIdEvent, resultCollector);
			}
			if (ev is LINIdEvent)
			{
				return this.ValidateLINIdEventChannel(ev as LINIdEvent, resultCollector);
			}
			if (ev is FlexrayIdEvent)
			{
				return this.ValidateFlexrayIdEventChannel(ev as FlexrayIdEvent, resultCollector);
			}
			if (ev is CANDataEvent)
			{
				return this.ValidateCANDataEventChannel(ev as CANDataEvent, resultCollector);
			}
			if (ev is LINDataEvent)
			{
				return this.ValidateLINDataEventChannel(ev as LINDataEvent, resultCollector);
			}
			if (ev is SymbolicMessageEvent)
			{
				return this.ValidateSymbolicMessageEvent(ev as SymbolicMessageEvent, databaseConfiguration, configFolderPath, databaseManager, resultCollector);
			}
			if (ev is ISymbolicSignalEvent)
			{
				return this.ValidateSymbolicSignalEvent(ev as ISymbolicSignalEvent, databaseConfiguration, configFolderPath, databaseManager, resultCollector);
			}
			if (ev is MsgTimeoutEvent)
			{
				return this.ValidateMsgTimeoutEvent(ev as MsgTimeoutEvent, databaseConfiguration, configFolderPath, databaseManager, resultCollector);
			}
			if (ev is CANBusStatisticsEvent)
			{
				return this.ValidateCANBusStatisticsEventChannel(ev as CANBusStatisticsEvent, resultCollector);
			}
			if (ev is DigitalInputEvent)
			{
				return this.ValidateDigitalInputEvent(ev as DigitalInputEvent, digitalOutputsConfiguration, resultCollector);
			}
			if (ev is VoCanRecordingEvent)
			{
				return this.ValidateVoCANEvent(ev as VoCanRecordingEvent, resultCollector);
			}
			if (ev is KeyEvent)
			{
				return this.ValidateKeyEvent(ev as KeyEvent, resultCollector);
			}
			if (ev is CombinedEvent)
			{
				return this.ValidateCombinedEvent(ev as CombinedEvent, databaseConfiguration, configFolderPath, databaseManager, digitalOutputsConfiguration, resultCollector);
			}
			return !(ev is IncEvent) || this.ValidateIncEvent(ev as IncEvent, resultCollector);
		}

		private bool ValidateCANIdEventChannel(CANIdEvent canIdEvent, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!this.IsHardwareChannelAvailable(BusType.Bt_CAN, canIdEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, canIdEvent.ChannelNumber, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.IsHardwareChannelActive(BusType.Bt_CAN, canIdEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, canIdEvent.ChannelNumber, Resources.ErrorChannelInactive);
				result = false;
			}
			return result;
		}

		private bool ValidateFlexrayIdEventChannel(FlexrayIdEvent frIdEvent, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!this.IsHardwareChannelAvailable(BusType.Bt_FlexRay, frIdEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, frIdEvent.ChannelNumber, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.IsHardwareChannelActive(BusType.Bt_FlexRay, frIdEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, frIdEvent.ChannelNumber, Resources.ErrorChannelInactive);
				result = false;
			}
			return result;
		}

		private bool ValidateCANBusStatisticsEventChannel(CANBusStatisticsEvent busStatEvent, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!this.IsHardwareChannelAvailable(BusType.Bt_CAN, busStatEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, busStatEvent.ChannelNumber, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.IsHardwareChannelActive(BusType.Bt_CAN, busStatEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, busStatEvent.ChannelNumber, Resources.ErrorChannelInactive);
				result = false;
			}
			return result;
		}

		private bool ValidateLINIdEventChannel(LINIdEvent linIdEvent, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!this.IsHardwareChannelAvailable(BusType.Bt_LIN, linIdEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, linIdEvent.ChannelNumber, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.IsHardwareChannelActive(BusType.Bt_LIN, linIdEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, linIdEvent.ChannelNumber, Resources.ErrorChannelInactive);
				result = false;
			}
			return result;
		}

		private bool ValidateCANDataEventChannel(CANDataEvent canDataEvent, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!this.IsHardwareChannelAvailable(BusType.Bt_CAN, canDataEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, canDataEvent.ChannelNumber, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else
			{
				if (!this.IsHardwareChannelActive(BusType.Bt_CAN, canDataEvent.ChannelNumber.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, canDataEvent.ChannelNumber, Resources.ErrorChannelInactive);
					result = false;
				}
				CANChannel cANChannel = this.CANChannelConfiguration.GetCANChannel(canDataEvent.ChannelNumber.Value);
				if (cANChannel == null)
				{
					cANChannel = this.MultibusChannelConfiguration.CANChannels[canDataEvent.ChannelNumber.Value];
				}
				if (!cANChannel.CANChipConfiguration.IsCANFD)
				{
					if (canDataEvent.RawDataSignal is RawDataSignalByte)
					{
						if ((canDataEvent.RawDataSignal as RawDataSignalByte).DataBytePos.Value >= Constants.MaxCANDataBytes)
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, (canDataEvent.RawDataSignal as RawDataSignalByte).DataBytePos, Resources.ErrorDataPosOutOfRangeForStdCAN);
							result = false;
						}
					}
					else if (canDataEvent.RawDataSignal is RawDataSignalStartbitLength)
					{
						RawDataSignalStartbitLength rawDataSignalStartbitLength = canDataEvent.RawDataSignal as RawDataSignalStartbitLength;
						if (!GUIUtil.IsRawSignalWithinDatabytes(rawDataSignalStartbitLength.StartbitPos.Value, rawDataSignalStartbitLength.Length.Value, rawDataSignalStartbitLength.IsMotorola.Value, Constants.MaxCANDataBytes))
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, (canDataEvent.RawDataSignal as RawDataSignalStartbitLength).StartbitPos, Resources.ErrorDataPosOutOfRangeForStdCAN);
							result = false;
						}
					}
				}
			}
			return result;
		}

		private bool ValidateLINDataEventChannel(LINDataEvent linDataEvent, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!this.IsHardwareChannelAvailable(BusType.Bt_LIN, linDataEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, linDataEvent.ChannelNumber, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.IsHardwareChannelActive(BusType.Bt_LIN, linDataEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, linDataEvent.ChannelNumber, Resources.ErrorChannelInactive);
				result = false;
			}
			return result;
		}

		private bool ValidateSymbolicMessageEvent(SymbolicMessageEvent symMsgEvent, DatabaseConfiguration databaseConfiguration, string configFolderPath, IApplicationDatabaseManager databaseManager, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!this.IsHardwareChannelAvailable(symMsgEvent.BusType.Value, symMsgEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, symMsgEvent.ChannelNumber, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.IsHardwareChannelActive(symMsgEvent.BusType.Value, symMsgEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, symMsgEvent.ChannelNumber, Resources.ErrorChannelInactive);
				result = false;
			}
			string absolutePath = FileSystemServices.GetAbsolutePath(symMsgEvent.DatabasePath.Value, configFolderPath);
			Database database;
			MessageDefinition messageDefinition;
			if (!databaseConfiguration.TryGetDatabase(symMsgEvent.DatabasePath.Value, symMsgEvent.NetworkName.Value, symMsgEvent.ChannelNumber.Value, symMsgEvent.BusType.Value, out database) || !databaseManager.ResolveMessageSymbolInDatabase(absolutePath, symMsgEvent.NetworkName.Value, symMsgEvent.MessageName.Value, symMsgEvent.BusType.Value, out messageDefinition))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, symMsgEvent.MessageName, Resources.ErrorUnresolvedMsgSymbol);
				result = false;
			}
			if (BusType.Bt_FlexRay == symMsgEvent.BusType.Value && database != null && !this.ValidateSymbolicMessageChannelFromFlexrayDb(symMsgEvent.MessageName.Value, symMsgEvent.ChannelNumber.Value, database))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, symMsgEvent.MessageName, Resources.ErrorUnresolvedMsgSymbolAtChn);
				result = false;
			}
			return result;
		}

		private bool ValidateSymbolicSignalEvent(ISymbolicSignalEvent symSigEvent, DatabaseConfiguration databaseConfiguration, string configFolderPath, IApplicationDatabaseManager databaseManager, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (symSigEvent is SymbolicSignalEvent)
			{
				if (!this.IsHardwareChannelAvailable(symSigEvent.BusType.Value, symSigEvent.ChannelNumber.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, symSigEvent.ChannelNumber, Resources.ErrorChannelNotAvailable);
					result = false;
				}
				else if (!this.IsHardwareChannelActive(symSigEvent.BusType.Value, symSigEvent.ChannelNumber.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, symSigEvent.ChannelNumber, Resources.ErrorChannelInactive);
					result = false;
				}
				string absolutePath = FileSystemServices.GetAbsolutePath(symSigEvent.DatabasePath.Value, configFolderPath);
				Database database;
				SignalDefinition signalDefinition;
				if (!databaseConfiguration.TryGetDatabase(symSigEvent.DatabasePath.Value, symSigEvent.NetworkName.Value, symSigEvent.ChannelNumber.Value, symSigEvent.BusType.Value, out database) || !databaseManager.ResolveSignalSymbolInDatabase(absolutePath, symSigEvent.NetworkName.Value, symSigEvent.MessageName.Value, symSigEvent.SignalName.Value, out signalDefinition))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, symSigEvent.MessageName, Resources.ErrorUnresolvedSigSymbol);
					result = false;
				}
				if (BusType.Bt_FlexRay == symSigEvent.BusType.Value && database != null && !this.ValidateSymbolicMessageChannelFromFlexrayDb(symSigEvent.MessageName.Value, symSigEvent.ChannelNumber.Value, database))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, symSigEvent.MessageName, Resources.ErrorUnresolvedMsgSymbolAtChn);
					result = false;
				}
			}
			else if (symSigEvent is DiagnosticSignalEvent)
			{
				DiagnosticSignalEvent diagnosticSignalEvent = symSigEvent as DiagnosticSignalEvent;
				DiagnosticsDatabase diagnosticsDatabase;
				DiagnosticsECU diagnosticsECU;
				SignalDefinition signalDefinition2;
				if (!this.DiagnosticsDatabaseConfiguration.TryGetDiagnosticsDatabase(diagnosticSignalEvent.DatabasePath.Value, out diagnosticsDatabase))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, symSigEvent.SignalName, Resources.ErrorUnresolvedSigSymbol);
					result = false;
				}
				else if (!diagnosticsDatabase.TryGetECU(diagnosticSignalEvent.DiagnosticEcuName.Value, out diagnosticsECU))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, symSigEvent.SignalName, Resources.ErrorUnresolvedSigSymbol);
					result = false;
				}
				else if (!this.DiagSymbolsManager.GetSignalDefinition(diagnosticSignalEvent.DiagnosticEcuName.Value, diagnosticSignalEvent.DiagnosticVariant.Value, diagnosticSignalEvent.DiagnosticDid.Value, symSigEvent.SignalName.Value, out signalDefinition2))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, symSigEvent.SignalName, Resources.ErrorUnresolvedSigSymbol);
					result = false;
				}
				else if (this.DiagnosticActionsConfiguration.DiagnosticSignalRequestOfSignalEvent(diagnosticSignalEvent) == null)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, symSigEvent.SignalName, Resources.ErrorUnresolvedSigSymbolRequestNotFund);
					result = false;
				}
			}
			else if (symSigEvent is CcpXcpSignalEvent && CcpXcpManager.Instance().GetSignalDefinition(symSigEvent.SignalName.Value, symSigEvent.CcpXcpEcuName.Value) == null)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, symSigEvent.SignalName, Resources.ErrorUnresolvedSigSymbol);
				result = false;
			}
			return result;
		}

		private bool ValidateSymbolicMessageChannelFromFlexrayDb(string messageName, uint messageChannel, Database flexrayDb)
		{
			return flexrayDb.ChannelNumber.Value != Database.ChannelNumber_FlexrayAB || ((!messageName.EndsWith(Constants.FlexrayChannelA_Postfix) || messageChannel == 1u) && (!messageName.EndsWith(Constants.FlexrayChannelB_Postfix) || messageChannel == 2u));
		}

		private bool ValidateMsgTimeoutEvent(MsgTimeoutEvent msgTimeoutEvent, DatabaseConfiguration databaseConfig, string configFolderPath, IApplicationDatabaseManager databaseManager, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!this.IsHardwareChannelAvailable(msgTimeoutEvent.BusType.Value, msgTimeoutEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, msgTimeoutEvent.ChannelNumber, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.IsHardwareChannelActive(msgTimeoutEvent.BusType.Value, msgTimeoutEvent.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, msgTimeoutEvent.ChannelNumber, Resources.ErrorChannelInactive);
				result = false;
			}
			if (msgTimeoutEvent.IsSymbolic.Value)
			{
				string absolutePath = FileSystemServices.GetAbsolutePath(msgTimeoutEvent.DatabasePath.Value, configFolderPath);
				MessageDefinition messageDefinition = null;
				bool flag = true;
				Database database;
				if (!databaseConfig.TryGetDatabase(msgTimeoutEvent.DatabasePath.Value, msgTimeoutEvent.NetworkName.Value, msgTimeoutEvent.ChannelNumber.Value, msgTimeoutEvent.BusType.Value, out database) || !databaseManager.ResolveMessageSymbolInDatabase(absolutePath, msgTimeoutEvent.NetworkName.Value, msgTimeoutEvent.MessageName.Value, msgTimeoutEvent.BusType.Value, out messageDefinition))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, msgTimeoutEvent.MessageName, Resources.ErrorUnresolvedMsgSymbol);
					result = false;
					flag = false;
				}
				if (msgTimeoutEvent.IsCycletimeFromDatabase.Value && flag && !messageDefinition.IsCyclic)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, msgTimeoutEvent.MessageName, Resources.ErrorNoCycleTimeDefinedForSym);
					result = false;
				}
			}
			return result;
		}

		private bool ValidateDigitalInputEvent(DigitalInputEvent ev, DigitalOutputsConfiguration digitalOutputsConfig, IModelValidationResultCollector resultCollector)
		{
			if (!this.LoggerSpecifics.IO.IsDigitalInputOutputCommonPin || digitalOutputsConfig == null)
			{
				return true;
			}
			int num = (int)(ev.DigitalInput.Value - 1u);
			if (num < digitalOutputsConfig.Actions.Count && digitalOutputsConfig.Actions[num].IsActive.Value)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, ev.DigitalInput, Resources.ErrorSamePinUsedAsDigOutput);
				return false;
			}
			return true;
		}

		private bool ValidateVoCANEvent(VoCanRecordingEvent voCanEvent, IModelValidationResultCollector resultCollector)
		{
			int num;
			bool flag;
			if (((IModelValidator)this).IsActiveVoCANEventConfigured(out num, out flag) && num > 1)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, voCanEvent.IsUsingCASM2T3L, Resources_Trigger.ErrorOnlyOneVoCANEventAllowed);
				return false;
			}
			if (voCanEvent.IsRecordingLEDActive.Value)
			{
				uint num2 = 7u - (Constants.StartIndexForRemoteLeds - this.configManager.LoggerSpecifics.IO.NumberOfLEDsOnBoard);
				if ((ulong)num2 < (ulong)((long)this.LEDConfiguration.LEDConfigList.Count) && this.LEDConfiguration.LEDConfigList[(int)num2].State.Value != LEDState.Disabled)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, voCanEvent.IsUsingCASM2T3L, Resources.ErrorLed8ConfiguredMoreThanOnce);
					return false;
				}
			}
			if (this.LoggerSpecifics.CAN.AuxChannel <= this.LoggerSpecifics.CAN.NumberOfChannels)
			{
				CANChannel cANChannel = this.GetHardwareChannel(BusType.Bt_CAN, this.LoggerSpecifics.CAN.AuxChannel) as CANChannel;
				if (cANChannel != null && (!cANChannel.IsActive.Value || cANChannel.CANChipConfiguration.Baudrate != Constants.AuxChannelBaudrate || !cANChannel.IsOutputActive.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, voCanEvent.IsUsingCASM2T3L, string.Format(Resources.VoCanRequiresCanChnSettingsNotSet, this.LoggerSpecifics.CAN.AuxChannel, Constants.AuxChannelBaudrate));
					return false;
				}
			}
			return true;
		}

		private bool ValidateKeyEvent(KeyEvent keyEvent, IModelValidationResultCollector resultCollector)
		{
			if (keyEvent.IsCasKey && this.LoggerSpecifics.CAN.AuxChannel <= this.LoggerSpecifics.CAN.NumberOfChannels)
			{
				CANChannel cANChannel = this.GetHardwareChannel(BusType.Bt_CAN, this.LoggerSpecifics.CAN.AuxChannel) as CANChannel;
				if (cANChannel != null && (!cANChannel.IsActive.Value || cANChannel.CANChipConfiguration.Baudrate != Constants.AuxChannelBaudrate || !cANChannel.IsOutputActive.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, keyEvent.IsOnPanel, string.Format(Resources.CasKeyRequiresCanChnSettingsNotSet, this.LoggerSpecifics.CAN.AuxChannel, Constants.AuxChannelBaudrate));
					return false;
				}
			}
			return true;
		}

		private bool ValidateCombinedEvent(CombinedEvent ev, DatabaseConfiguration databaseConfiguration, string configFolderPath, IApplicationDatabaseManager databaseManager, DigitalOutputsConfiguration digitalOutputsConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			List<Event> list = ev.Where(new Func<Event, bool>(ev.ChildIsActive)).ToList<Event>();
			List<Event> list2 = (from t in list
			where t.IsPointInTime.Value
			select t).ToList<Event>();
			if (ev.IsConjunction.Value && list2.Count > 1)
			{
				flag = false;
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, ev.IsConjunction, Resources.ErrorOnlyOnePointInTimeEventAllowed);
				foreach (Event current in list2)
				{
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current.IsPointInTime, Resources.ErrorOnlyOnePointInTimeEventAllowed);
				}
			}
			foreach (Event current2 in list)
			{
				flag &= this.ValidateEvent(current2, databaseConfiguration, configFolderPath, databaseManager, digitalOutputsConfiguration, resultCollector);
			}
			if (list.Count < 2)
			{
				flag = false;
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, ev.IsPointInTime, Resources.ErrorCombinedEventWithLessThanTwoConditions);
			}
			return flag;
		}

		private bool ValidateIncEvent(IncEvent incEvent, IModelValidationResultCollector resultCollector)
		{
			IncludeFilePresenter incFilePres;
			if (!IncludeFileManager.Instance.TryGetIncludeFile(incEvent, out incFilePres))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, incEvent.FilePath, Resources_IncFiles.ErrorFileDoesNotExist);
				return false;
			}
			string firstErrorText = IncludeFileManager.Instance.GetFirstErrorText(incFilePres);
			if (!string.IsNullOrEmpty(firstErrorText))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, incEvent.FilePath, firstErrorText);
				return false;
			}
			IncludeFileParameterPresenter includeFileParameterPresenter;
			if (!IncludeFileManager.Instance.TryGetOutParameter(incEvent, out includeFileParameterPresenter, false))
			{
				if (!IncludeFileManager.Instance.TryGetOutParameter(incEvent, out includeFileParameterPresenter, true))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, incEvent.ParamIndex, Resources_IncFiles.ErrorOutParameterDoesNotExist);
					return false;
				}
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, incEvent.ParamIndex, Resources_IncFiles.ErrorInstanceDoesNotExist);
				return false;
			}
			else
			{
				if (includeFileParameterPresenter.Parent.InstanceParameter != null)
				{
					firstErrorText = IncludeFileManager.Instance.GetFirstErrorText(includeFileParameterPresenter.Parent.InstanceParameter);
					if (!string.IsNullOrEmpty(firstErrorText))
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, incEvent.FilePath, firstErrorText);
						return false;
					}
				}
				firstErrorText = IncludeFileManager.Instance.GetFirstErrorText(includeFileParameterPresenter);
				if (!string.IsNullOrEmpty(firstErrorText))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, incEvent.ParamIndex, firstErrorText);
					return false;
				}
				incEvent.LtlName = includeFileParameterPresenter.LtlName;
				return true;
			}
		}

		public static string CreateWhitelistRegexFromString(string str, bool includeWhitespace)
		{
			str = str.Replace("- ", "\\- ");
			str = str.Replace("]", "\\]");
			str = str.Replace(" ", "");
			if (includeWhitespace)
			{
				str = "^([" + str + "\\s]+)$";
			}
			else
			{
				str = "^([" + str + "]+)$";
			}
			return str;
		}

		public static string ReplaceInvalidCharactersIfPossible(string str)
		{
			str = str.Replace("ä", "ae");
			str = str.Replace("ö", "oe");
			str = str.Replace("ü", "ue");
			str = str.Replace("Ä", "Ae");
			str = str.Replace("Ö", "Oe");
			str = str.Replace("Ü", "Ue");
			str = str.Replace("ß", "ss");
			return str;
		}

		private bool ValidateSendMessageConfiguration(SendMessageConfiguration sendMessageConfiguration, DatabaseConfiguration databaseConfiguration, string configFolderPath, IApplicationDatabaseManager databaseManager, DigitalOutputsConfiguration digitalOutputsConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			foreach (ActionSendMessage current in sendMessageConfiguration.Actions)
			{
				if (current.IsActive.Value)
				{
					flag &= this.ValidateActionSendMessageParameter(current, databaseConfiguration, configFolderPath, databaseManager, resultCollector);
					flag &= this.ValidateEvent(current.Event, databaseConfiguration, configFolderPath, databaseManager, digitalOutputsConfiguration, resultCollector);
				}
			}
			return flag;
		}

		private bool ValidateActionSendMessageParameter(ActionSendMessage action, DatabaseConfiguration databaseConfiguration, string configFolderPath, IApplicationDatabaseManager databaseManager, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (!string.IsNullOrEmpty(action.Comment.Value) && action.Comment.Value.IndexOfAny(ModelValidator.invalidTriggerCommentChars) >= 0)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, action.Comment, string.Format(Resources.InvalidCommentString, Resources.Comment));
				result = false;
			}
			if (BusType.Bt_CAN == action.BusType.Value)
			{
				if ((long)this.CANChannelConfiguration.CANChannels.Count >= (long)((ulong)action.ChannelNumber.Value))
				{
					if (!this.CANChannelConfiguration.GetCANChannel(action.ChannelNumber.Value).IsActive.Value)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, action.ChannelNumber, Resources.ErrorChannelInactive);
						result = false;
					}
					else
					{
						this.CANChannelConfiguration.GetCANChannel(action.ChannelNumber.Value);
					}
				}
				else if (this.MultibusChannelConfiguration.NumberOfChannels >= action.ChannelNumber.Value)
				{
					if (this.MultibusChannelConfiguration.CANChannels.ContainsKey(action.ChannelNumber.Value))
					{
						if (!this.MultibusChannelConfiguration.CANChannels[action.ChannelNumber.Value].IsActive.Value)
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, action.ChannelNumber, Resources.ErrorChannelInactive);
							result = false;
						}
						else
						{
							CANChannel arg_17B_0 = this.MultibusChannelConfiguration.CANChannels[action.ChannelNumber.Value];
						}
					}
					else
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, action.ChannelNumber, Resources.ErrorChannelNotAvailable);
						result = false;
					}
				}
			}
			else if (BusType.Bt_LIN == action.BusType.Value)
			{
				if (!this.IsHardwareChannelAvailable(BusType.Bt_LIN, action.ChannelNumber.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, action.ChannelNumber, Resources.ErrorChannelNotAvailable);
					result = false;
				}
				else if (!this.IsHardwareChannelActive(BusType.Bt_LIN, action.ChannelNumber.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, action.ChannelNumber, Resources.ErrorChannelInactive);
					result = false;
				}
			}
			if (action.IsSymbolic.Value)
			{
				string absolutePath = FileSystemServices.GetAbsolutePath(action.DatabasePath.Value, configFolderPath);
				Database database;
				MessageDefinition messageDefinition;
				if (!databaseConfiguration.TryGetDatabase(action.DatabasePath.Value, action.NetworkName.Value, action.ChannelNumber.Value, action.BusType.Value, out database) || !databaseManager.ResolveMessageSymbolInDatabase(absolutePath, action.NetworkName.Value, action.SymbolName.Value, action.BusType.Value, out messageDefinition))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, action.SymbolName, Resources.ErrorUnresolvedMsgSymbol);
					result = false;
				}
			}
			return result;
		}

		private bool ValidateDigitalOutputsConfiguration(DigitalOutputsConfiguration digitalOutputsConfiguration, DigitalInputConfiguration digitalInputConfiguration, DatabaseConfiguration databaseConfiguration, string configFolderPath, IApplicationDatabaseManager databaseManager, IList<TriggerConfiguration> triggerConfigurations, DiagnosticActionsConfiguration diagActionsConfig, SendMessageConfiguration sendMessageConfig, LogDataStorage logDataStorage, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			if (this.LoggerSpecifics.IO.IsDigitalInputOutputCommonPin && digitalInputConfiguration != null)
			{
				for (int i = 0; i < Math.Min(digitalOutputsConfiguration.Actions.Count, digitalInputConfiguration.DigitalInputs.Count); i++)
				{
					if (digitalOutputsConfiguration.Actions[i].IsActive.Value && (digitalInputConfiguration.DigitalInputs[i].IsActiveFrequency.Value || digitalInputConfiguration.DigitalInputs[i].IsActiveOnChange.Value))
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, digitalOutputsConfiguration.Actions[i].IsActive, Resources.ErrorSamePinUsedInDigInConf);
						flag = false;
					}
				}
				foreach (ActionDigitalOutput current in digitalOutputsConfiguration.Actions)
				{
					if (current.IsActive.Value)
					{
						if (current.Event is DigitalInputEvent)
						{
							int num = (int)((current.Event as DigitalInputEvent).DigitalInput.Value - 1u);
							if (num < digitalOutputsConfiguration.Actions.Count && digitalOutputsConfiguration.Actions[num].IsActive.Value)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, digitalOutputsConfiguration.Actions[num].IsActive, Resources.ErrorSamePinUsedAsInput);
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current.Event as DigitalInputEvent).DigitalInput, Resources.ErrorSamePinUsedAsOutput);
								flag = false;
							}
						}
						if (current.StopType is StopOnEvent)
						{
							DigitalInputEvent digitalInputEvent = (current.StopType as StopOnEvent).Event as DigitalInputEvent;
							if (digitalInputEvent != null)
							{
								int num2 = (int)(digitalInputEvent.DigitalInput.Value - 1u);
								if (num2 < digitalOutputsConfiguration.Actions.Count && digitalOutputsConfiguration.Actions[num2].IsActive.Value)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, digitalOutputsConfiguration.Actions[num2].IsActive, Resources.ErrorSamePinUsedAsInput);
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, digitalInputEvent.DigitalInput, Resources.ErrorSamePinUsedAsOutput);
									flag = false;
								}
							}
						}
					}
				}
				foreach (TriggerConfiguration current2 in triggerConfigurations)
				{
					if (current2.MemoryRingBuffer.IsActive.Value)
					{
						ReadOnlyCollection<RecordTrigger> readOnlyCollection = null;
						if (current2.TriggerMode.Value == TriggerMode.Triggered)
						{
							readOnlyCollection = current2.ActiveTriggers;
						}
						else if (current2.TriggerMode.Value == TriggerMode.OnOff)
						{
							readOnlyCollection = current2.ActiveOnOffTriggers;
						}
						else if (current2.TriggerMode.Value == TriggerMode.Permanent)
						{
							readOnlyCollection = current2.ActivePermanentMarkers;
						}
						if (readOnlyCollection != null)
						{
							foreach (RecordTrigger current3 in readOnlyCollection)
							{
								flag &= this.ValidateDigitalInputOutputActiveState(current3.Event, digitalOutputsConfiguration, resultCollector, Resources.ErrorSamePinUsedAsInputInTrigger);
							}
						}
					}
				}
				foreach (TriggeredDiagnosticActionSequence current4 in diagActionsConfig.TriggeredActionSequences)
				{
					flag &= this.ValidateDigitalInputOutputActiveState(current4.Event, digitalOutputsConfiguration, resultCollector, Resources.ErrorSamePinUsedAsInputInDiagReq);
				}
				foreach (ActionSendMessage current5 in sendMessageConfig.Actions)
				{
					flag &= this.ValidateDigitalInputOutputActiveState(current5.Event, digitalOutputsConfiguration, resultCollector, Resources.ErrorSamePinUsedAsInputInTransmitMsg);
				}
				flag &= this.ValidateDigitalInputOutputActiveState(logDataStorage.StopCyclicCommunicationEvent, digitalOutputsConfiguration, resultCollector, Resources.ErrorSamePinUsedAsInputInHWSettings);
			}
			foreach (ActionDigitalOutput current6 in digitalOutputsConfiguration.Actions)
			{
				if (current6.IsActive.Value)
				{
					if (!string.IsNullOrEmpty(current6.Comment.Value) && current6.Comment.Value.IndexOfAny(ModelValidator.invalidTriggerCommentChars) >= 0)
					{
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current6.Comment, string.Format(Resources.InvalidCommentString, Resources.Comment));
						flag = false;
					}
					flag &= this.ValidateEvent(current6.Event, databaseConfiguration, configFolderPath, databaseManager, null, resultCollector);
					if (current6.StopType is StopOnEvent)
					{
						StopOnEvent stopOnEvent = current6.StopType as StopOnEvent;
						flag &= this.ValidateEvent(stopOnEvent.Event, databaseConfiguration, configFolderPath, databaseManager, null, resultCollector);
						if (current6.Event.Equals(stopOnEvent.Event))
						{
							if (current6.Event is IdEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as IdEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								if (stopOnEvent.Event is IdEvent)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (stopOnEvent.Event as IdEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								}
							}
							else if (current6.Event is CANDataEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as CANDataEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								if (stopOnEvent.Event is CANDataEvent)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (stopOnEvent.Event as CANDataEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								}
							}
							else if (current6.Event is LINDataEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as LINDataEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								if (stopOnEvent.Event is LINDataEvent)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (stopOnEvent.Event as LINDataEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								}
							}
							else if (current6.Event is SymbolicMessageEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as SymbolicMessageEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								if (stopOnEvent.Event is SymbolicMessageEvent)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (stopOnEvent.Event as SymbolicMessageEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								}
							}
							else if (current6.Event is SymbolicSignalEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as SymbolicSignalEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								if (stopOnEvent.Event is SymbolicSignalEvent)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (stopOnEvent.Event as SymbolicSignalEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								}
							}
							else if (current6.Event is MsgTimeoutEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as MsgTimeoutEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								if (stopOnEvent.Event is MsgTimeoutEvent)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (stopOnEvent.Event as MsgTimeoutEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								}
							}
							else if (current6.Event is AnalogInputEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as AnalogInputEvent).InputNumber, Resources.ErrorSETandRESETMustDiffer);
								if (stopOnEvent.Event is AnalogInputEvent)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (stopOnEvent.Event as AnalogInputEvent).InputNumber, Resources.ErrorSETandRESETMustDiffer);
								}
							}
							else if (current6.Event is DigitalInputEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as DigitalInputEvent).DigitalInput, Resources.ErrorSETandRESETMustDiffer);
								if (stopOnEvent.Event is DigitalInputEvent)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (stopOnEvent.Event as DigitalInputEvent).DigitalInput, Resources.ErrorSETandRESETMustDiffer);
								}
							}
							else if (current6.Event is KeyEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as KeyEvent).Number, Resources.ErrorSETandRESETMustDiffer);
								if (stopOnEvent.Event is KeyEvent)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (stopOnEvent.Event as KeyEvent).Number, Resources.ErrorSETandRESETMustDiffer);
								}
							}
							else if (current6.Event is CANBusStatisticsEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as CANBusStatisticsEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								if (stopOnEvent.Event is CANBusStatisticsEvent)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (stopOnEvent.Event as CANBusStatisticsEvent).ChannelNumber, Resources.ErrorSETandRESETMustDiffer);
								}
							}
							else if (current6.Event is IgnitionEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as IgnitionEvent).IsOn, Resources.ErrorSETandRESETMustDiffer);
								if (stopOnEvent.Event is IgnitionEvent)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (stopOnEvent.Event as IgnitionEvent).IsOn, Resources.ErrorSETandRESETMustDiffer);
								}
							}
							flag = false;
						}
					}
					else if (current6.StopType is StopOnStartEvent)
					{
						StopOnStartEvent stopOnStartEvent = current6.StopType as StopOnStartEvent;
						if (stopOnStartEvent.IsStopOnResetOfStartEvent.Value)
						{
							if (current6.Event is OnStartEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as OnStartEvent).Delay, Resources.ErrorSETEventCantBeUsedWithRESETEvent);
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, stopOnStartEvent.IsStopOnResetOfStartEvent, Resources.ErrorSETEventCantBeUsedWithRESETEvent);
								flag = false;
							}
							else if (current6.Event is SymbolicMessageEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as SymbolicMessageEvent).ChannelNumber, Resources.ErrorSETEventCantBeUsedWithRESETEvent);
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, stopOnStartEvent.IsStopOnResetOfStartEvent, Resources.ErrorSETEventCantBeUsedWithRESETEvent);
								flag = false;
							}
							else if (current6.Event is IdEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as IdEvent).ChannelNumber, Resources.ErrorSETEventCantBeUsedWithRESETEvent);
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, stopOnStartEvent.IsStopOnResetOfStartEvent, Resources.ErrorSETEventCantBeUsedWithRESETEvent);
								flag = false;
							}
							else if (current6.Event is MsgTimeoutEvent)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current6.Event as MsgTimeoutEvent).ChannelNumber, Resources.ErrorSETEventCantBeUsedWithRESETEvent);
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, stopOnStartEvent.IsStopOnResetOfStartEvent, Resources.ErrorSETEventCantBeUsedWithRESETEvent);
								flag = false;
							}
							else if (current6.Event is ISymbolicSignalEvent)
							{
								if ((current6.Event as ISymbolicSignalEvent).Relation.Value == CondRelation.OnChange)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, stopOnStartEvent.IsStopOnResetOfStartEvent, Resources.ErrorSETEventCantBeUsedWithRESETEvent);
									flag = false;
								}
							}
							else if (current6.Event is CANDataEvent)
							{
								if ((current6.Event as CANDataEvent).Relation.Value == CondRelation.OnChange)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, stopOnStartEvent.IsStopOnResetOfStartEvent, Resources.ErrorSETEventCantBeUsedWithRESETEvent);
									flag = false;
								}
							}
							else if (current6.Event is LINDataEvent && (current6.Event as LINDataEvent).Relation.Value == CondRelation.OnChange)
							{
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, stopOnStartEvent.IsStopOnResetOfStartEvent, Resources.ErrorSETEventCantBeUsedWithRESETEvent);
								flag = false;
							}
						}
					}
				}
			}
			return flag;
		}

		private bool ValidateDigitalInputOutputActiveState(Event ev, DigitalOutputsConfiguration digitalOutputsConfiguration, IModelValidationResultCollector resultCollector, string errorText)
		{
			bool result = true;
			if (ev is DigitalInputEvent)
			{
				int num = (int)((ev as DigitalInputEvent).DigitalInput.Value - 1u);
				if (num < digitalOutputsConfiguration.Actions.Count && digitalOutputsConfiguration.Actions[num].IsActive.Value)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, digitalOutputsConfiguration.Actions[num].IsActive, errorText);
					result = false;
				}
			}
			return result;
		}

		private bool ValidateSpecialFeatures(SpecialFeaturesConfiguration specialFeatureConfiguration, CANChannelConfiguration canChannelConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			if (specialFeatureConfiguration.IsLogDateTimeEnabled.Value)
			{
				flag &= this.ValidateLogDateTimeChannel(specialFeatureConfiguration, canChannelConfiguration, resultCollector);
			}
			return flag;
		}

		private bool ValidateSpecialFeaturesVN1630log(SpecialFeaturesConfiguration specialFeaturesConfig, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			uint num = 1u;
			uint maxLogFileSizeLimitationMB = this.LoggerSpecifics.DataStorage.MaxLogFileSizeLimitationMB;
			if (specialFeaturesConfig.MaxLogFileSize.Value < num || specialFeaturesConfig.MaxLogFileSize.Value > maxLogFileSizeLimitationMB)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, specialFeaturesConfig.MaxLogFileSize, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, num, maxLogFileSizeLimitationMB));
				result = false;
			}
			return result;
		}

		private bool ValidateSpecialFeaturesGL3Plus(SpecialFeaturesConfiguration specialFeatureConfiguration, CANChannelConfiguration canChannelConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			if (specialFeatureConfiguration.IsLogDateTimeEnabled.Value)
			{
				flag &= this.ValidateLogDateTimeChannel(specialFeatureConfiguration, canChannelConfiguration, resultCollector);
			}
			return flag;
		}

		private bool ValidateLogDateTimeChannel(SpecialFeaturesConfiguration specialFeatureConfiguration, CANChannelConfiguration canChannelConfiguration, IModelValidationResultCollector resultCollector)
		{
			uint value = specialFeatureConfiguration.LogDateTimeChannel.Value;
			if ((ulong)value <= (ulong)((long)canChannelConfiguration.CANChannels.Count) && !canChannelConfiguration.GetCANChannel(value).IsActive.Value)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, specialFeatureConfiguration.LogDateTimeChannel, Resources.ErrorChannelInactive);
				return false;
			}
			return true;
		}

		private bool ValidateInterfaceMode(InterfaceModeConfiguration interfaceModeConfiguration, CANChannelConfiguration canChannelConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			if (interfaceModeConfiguration.Port.Value < Constants.MinInterfaceModePort || interfaceModeConfiguration.Port.Value > Constants.MaxInterfaceModePort)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, interfaceModeConfiguration.Port, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinInterfaceModePort, Constants.MaxInterfaceModePort));
				flag = false;
			}
			if ((ulong)interfaceModeConfiguration.MarkerChannel.Value <= (ulong)((long)canChannelConfiguration.CANChannels.Count) && !canChannelConfiguration.GetCANChannel(interfaceModeConfiguration.MarkerChannel.Value).IsActive.Value)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, interfaceModeConfiguration.MarkerChannel, Resources.ErrorChannelInactive);
				flag = false;
			}
			return flag & this.ValidateLoggerIpConfiguration(interfaceModeConfiguration, this.configManager.WLANConfiguration, resultCollector);
		}

		private bool ValidateSignalExportInterfaceMode(InterfaceModeConfiguration interfaceModeConfiguration, CANChannelConfiguration canChannelConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			if (interfaceModeConfiguration.UseSignalExport.Value)
			{
				if (interfaceModeConfiguration.UseCustomWebDisplay.Value)
				{
					if (string.IsNullOrEmpty(interfaceModeConfiguration.CustomWebDisplay.FilePath.Value))
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, interfaceModeConfiguration.CustomWebDisplay.FilePath, Resources.PleaseSelectWebDisplayPage);
						flag = false;
					}
					else if (!GUIUtil.FileAccessible(FileSystemServices.GetAbsolutePath(interfaceModeConfiguration.CustomWebDisplay.FilePath.Value, this.ConfigFolderPath)))
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, interfaceModeConfiguration.CustomWebDisplay.FilePath, Resources.ErrorFileNotFound);
						flag = false;
					}
					else if ((Path.GetExtension(interfaceModeConfiguration.CustomWebDisplay.FilePath.Value) ?? string.Empty).ToLower() != Vocabulary.FileExtensionDotHTM.ToLower())
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, interfaceModeConfiguration.CustomWebDisplay.FilePath, Resources.ErrorIncorrectFileType);
						flag = false;
					}
					else if (Path.GetFileNameWithoutExtension(interfaceModeConfiguration.CustomWebDisplay.FilePath.Value) != Path.GetFileNameWithoutExtension(Resources.WebDisplayIndexName))
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, interfaceModeConfiguration.CustomWebDisplay.FilePath, Resources.ErrorIncorrectFileName);
						flag = false;
					}
				}
				flag &= this.ValidateWebDisplayExportSignalNameUniqueness(interfaceModeConfiguration, resultCollector);
				foreach (WebDisplayExportSignal current in interfaceModeConfiguration.ActiveWebDisplayExportSignals)
				{
					if (current.Type == WebDisplayExportSignalType.Signal)
					{
						flag &= this.ValidateSymbolicWebDisplayExportSignal(current, canChannelConfiguration, this.configManager.LINChannelConfiguration, this.configManager.FlexrayChannelConfiguration, this.DatabaseConfiguration, this.ConfigFolderPath, this.ApplicationDatabaseManager, resultCollector);
					}
					flag &= this.ValidateWebDisplayExportSignalName(current, resultCollector);
					flag &= this.ValidateWebDisplayExportSignalComment(current, resultCollector);
				}
				if (interfaceModeConfiguration.ExportCycle.Value < Constants.ExportCycle_Min || interfaceModeConfiguration.ExportCycle.Value > Constants.ExportCycle_Max)
				{
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, interfaceModeConfiguration.ExportCycle, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.ExportCycle_Min, Constants.ExportCycle_Max));
					flag = false;
				}
			}
			return flag;
		}

		private bool ValidateSymbolicWebDisplayExportSignal(WebDisplayExportSignal expSig, CANChannelConfiguration canChannelConfiguration, LINChannelConfiguration linChannelConfiguration, FlexrayChannelConfiguration flexrayChannelConfiguration, DatabaseConfiguration databaseConfiguration, string configFolderPath, IApplicationDatabaseManager databaseManager, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (BusType.Bt_CAN == expSig.BusType.Value && canChannelConfiguration != null)
			{
				if (!canChannelConfiguration.GetActiveCANChannelNumbers().Contains(expSig.ChannelNumber.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, expSig.ChannelNumber, Resources.ErrorChannelInactive);
					result = false;
				}
			}
			else if (BusType.Bt_LIN == expSig.BusType.Value && linChannelConfiguration != null)
			{
				if (!linChannelConfiguration.GetActiveLINChannelNumbers().Contains(expSig.ChannelNumber.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, expSig.ChannelNumber, Resources.ErrorChannelInactive);
					result = false;
				}
			}
			else if (BusType.Bt_FlexRay == expSig.BusType.Value && flexrayChannelConfiguration != null && !flexrayChannelConfiguration.GetActiveFlexrayChannelNumbers().Contains(expSig.ChannelNumber.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, expSig.ChannelNumber, Resources.ErrorChannelInactive);
				result = false;
			}
			string absolutePath = FileSystemServices.GetAbsolutePath(expSig.DatabasePath.Value, configFolderPath);
			Database database;
			SignalDefinition signalDefinition;
			if (!databaseConfiguration.TryGetDatabase(expSig.DatabasePath.Value, expSig.NetworkName.Value, expSig.ChannelNumber.Value, expSig.BusType.Value, out database) || !databaseManager.ResolveSignalSymbolInDatabase(absolutePath, expSig.NetworkName.Value, expSig.MessageName.Value, expSig.SignalName.Value, out signalDefinition))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, expSig.MessageName, Resources.ErrorUnresolvedSigSymbol);
				result = false;
			}
			if (BusType.Bt_FlexRay == expSig.BusType.Value && database != null && !this.ValidateSymbolicMessageChannelFromFlexrayDb(expSig.MessageName.Value, expSig.ChannelNumber.Value, database))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, expSig.MessageName, Resources.ErrorUnresolvedMsgSymbolAtChn);
				result = false;
			}
			return result;
		}

		private bool ValidateWebDisplayExportSignalNameUniqueness(InterfaceModeConfiguration interfaceModeConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			foreach (WebDisplayExportSignal current in interfaceModeConfiguration.ActiveWebDisplayExportSignals)
			{
				int num = 0;
				foreach (WebDisplayExportSignal current2 in interfaceModeConfiguration.ActiveWebDisplayExportSignals)
				{
					if (string.Compare(current.Name.Value, current2.Name.Value, true) == 0)
					{
						num++;
					}
				}
				if (num > 1)
				{
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current.Name, Resources.ErrorWebDisplayExportSignalNameNotUnique);
					result = false;
				}
			}
			return result;
		}

		private bool ValidateWebDisplayExportSignalName(WebDisplayExportSignal expSig, IModelValidationResultCollector resultCollector)
		{
			if (string.IsNullOrEmpty(expSig.Name.Value) || string.IsNullOrWhiteSpace(expSig.Name.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, expSig.Name, Resources.ErrorNameMustNotBeEmpty);
				return false;
			}
			if (char.IsNumber(expSig.Name.Value[0]))
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, expSig.Name, Resources.ErrorNameStartsWithNumber);
				return false;
			}
			if (!GenerationUtil.StringIsCaplCompliant(expSig.Name.Value))
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, expSig.Name, string.Format(Resources.InvalidNameStringWhitelist, expSig.Name.Value, ModelValidator.caplCompliantCharactersList));
				return false;
			}
			return true;
		}

		private bool ValidateWebDisplayExportSignalComment(WebDisplayExportSignal expSig, IModelValidationResultCollector resultCollector)
		{
			int num = 1000;
			if (expSig.Comment.Value.Count<char>() > num)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, expSig.Comment, string.Format(Resources.ErrorInputExceedsMaxLen, num));
				return false;
			}
			char[] array = new char[]
			{
				'='
			};
			char[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				char value = array2[i];
				if (expSig.Comment.Value.Contains(value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, expSig.Comment, string.Format(Resources.ErrorIllegalChars, string.Join<char>(" ", from p in array
					select p)));
					return false;
				}
			}
			return true;
		}

		private bool ValidateAnalogInputConfiguration(AnalogInputConfiguration analogInputConfiguration, CANChannelConfiguration canChannelConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			bool flag2 = false;
			foreach (AnalogInput current in analogInputConfiguration.AnalogInputs)
			{
				if (current.IsActive.Value)
				{
					flag2 = true;
					break;
				}
			}
			if (flag2 && analogInputConfiguration.CanChannel.Value <= this.LoggerSpecifics.CAN.NumberOfChannels)
			{
				CANChannel cANChannel = canChannelConfiguration.GetCANChannel(analogInputConfiguration.CanChannel.Value);
				if (cANChannel != null && !cANChannel.IsActive.Value)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, analogInputConfiguration.CanChannel, Resources.ErrorChannelInactive);
					flag = false;
				}
			}
			if (analogInputConfiguration.AnalogInputsCANMappingMode.Value == AnalogInputsCANMappingMode.SameFixedIDs)
			{
				if (this.LoggerSpecifics.IO.NumberOfAnalogInputs > 4u)
				{
					uint num = Constants.MaximumExtendedCANId;
					bool value = analogInputConfiguration.GetAnalogInput(1u).IsMappedCANIdExtended.Value;
					if (!value)
					{
						num = Constants.MaximumStandardCANId;
					}
					num -= 3u;
					if (analogInputConfiguration.GetAnalogInput(1u).MappedCANId.Value > num)
					{
						string arg = GUIUtil.CANIdToDisplayString(num, value);
						string arg2 = GUIUtil.CANIdToDisplayString(0u, value);
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, analogInputConfiguration.GetAnalogInput(1u).MappedCANId, string.Format(Resources.ErrorIdOutOfRange, arg2, arg));
						return false;
					}
				}
			}
			else if (AnalogInputsCANMappingMode.ContinuousIndividualIDs == analogInputConfiguration.AnalogInputsCANMappingMode.Value)
			{
				uint num2 = Constants.MaximumExtendedCANId;
				bool value2 = analogInputConfiguration.GetAnalogInput(1u).IsMappedCANIdExtended.Value;
				if (!value2)
				{
					num2 = Constants.MaximumStandardCANId;
				}
				num2 -= this.LoggerSpecifics.IO.NumberOfAnalogInputs - 1u;
				if (analogInputConfiguration.GetAnalogInput(1u).MappedCANId.Value > num2)
				{
					string arg3 = GUIUtil.CANIdToDisplayString(num2, value2);
					string arg4 = GUIUtil.CANIdToDisplayString(0u, value2);
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, analogInputConfiguration.GetAnalogInput(1u).MappedCANId, string.Format(Resources.ErrorIdOutOfRange, arg4, arg3));
					return false;
				}
			}
			else if (AnalogInputsCANMappingMode.IndividualIDs == analogInputConfiguration.AnalogInputsCANMappingMode.Value)
			{
				bool flag3 = false;
				HashSet<uint> hashSet = new HashSet<uint>();
				for (uint num3 = 1u; num3 <= this.LoggerSpecifics.IO.NumberOfAnalogInputs; num3 += 1u)
				{
					if (analogInputConfiguration.GetAnalogInput(num3).IsActive.Value)
					{
						uint num4 = analogInputConfiguration.GetAnalogInput(num3).MappedCANId.Value;
						if (analogInputConfiguration.GetAnalogInput(num3).IsMappedCANIdExtended.Value)
						{
							num4 |= Constants.CANDbIsExtendedIdMask;
						}
						if (hashSet.Contains(num4))
						{
							resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, analogInputConfiguration.GetAnalogInput(num3).MappedCANId, Resources.ErrorSameIdForDifferentInputs);
							flag3 = true;
						}
						else
						{
							hashSet.Add(num4);
						}
					}
				}
				flag &= !flag3;
			}
			return flag;
		}

		private bool ValidateDigitalInputConfiguration(DigitalInputConfiguration digitalInputConfiguration, DigitalOutputsConfiguration digitalOutputsConfiguration, CANChannelConfiguration canChannelConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			bool flag2 = false;
			for (int i = 0; i < digitalInputConfiguration.DigitalInputs.Count; i++)
			{
				if (digitalInputConfiguration.DigitalInputs[i].IsActiveFrequency.Value)
				{
					if (this.LoggerSpecifics.IO.IsDigitalInputOutputCommonPin && digitalOutputsConfiguration != null && digitalOutputsConfiguration.Actions[i].IsActive.Value)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, digitalInputConfiguration.DigitalInputs[i].IsActiveFrequency, Resources.ErrorSamePinUsedInDigOutConf);
						flag = false;
					}
					flag2 = true;
				}
				if (digitalInputConfiguration.DigitalInputs[i].IsActiveOnChange.Value)
				{
					if (this.LoggerSpecifics.IO.IsDigitalInputOutputCommonPin && digitalOutputsConfiguration != null && digitalOutputsConfiguration.Actions[i].IsActive.Value)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, digitalInputConfiguration.DigitalInputs[i].IsActiveOnChange, Resources.ErrorSamePinUsedInDigOutConf);
						flag = false;
					}
					flag2 = true;
				}
			}
			if (flag2 && digitalInputConfiguration.CanChannel.Value <= this.LoggerSpecifics.CAN.NumberOfChannels)
			{
				CANChannel cANChannel = canChannelConfiguration.GetCANChannel(digitalInputConfiguration.CanChannel.Value);
				if (cANChannel != null && !cANChannel.IsActive.Value)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, digitalInputConfiguration.CanChannel, Resources.ErrorChannelInactive);
					flag = false;
				}
			}
			if (DigitalInputsMappingMode.ContinuousIndividualIDs == digitalInputConfiguration.DigitalInputsMappingMode.Value)
			{
				uint num = Constants.MaximumExtendedCANId;
				bool value = digitalInputConfiguration.GetDigitalInput(1u).IsMappedCANIdExtended.Value;
				if (!value)
				{
					num = Constants.MaximumStandardCANId;
				}
				num -= this.LoggerSpecifics.IO.NumberOfDigitalInputs - 1u;
				if (digitalInputConfiguration.GetDigitalInput(1u).MappedCANId.Value > num)
				{
					string arg = GUIUtil.CANIdToDisplayString(num, value);
					string arg2 = GUIUtil.CANIdToDisplayString(0u, value);
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, digitalInputConfiguration.GetDigitalInput(1u).MappedCANId, string.Format(Resources.ErrorIdOutOfRange, arg2, arg));
					return false;
				}
			}
			else if (DigitalInputsMappingMode.IndividualIDs == digitalInputConfiguration.DigitalInputsMappingMode.Value)
			{
				bool flag3 = false;
				HashSet<uint> hashSet = new HashSet<uint>();
				for (uint num2 = 1u; num2 <= this.LoggerSpecifics.IO.NumberOfDigitalInputs; num2 += 1u)
				{
					if (digitalInputConfiguration.GetDigitalInput(num2).IsActiveFrequency.Value || digitalInputConfiguration.GetDigitalInput(num2).IsActiveOnChange.Value)
					{
						uint num3 = digitalInputConfiguration.GetDigitalInput(num2).MappedCANId.Value;
						if (digitalInputConfiguration.GetDigitalInput(num2).IsMappedCANIdExtended.Value)
						{
							num3 |= 4026531840u;
						}
						if (hashSet.Contains(num3))
						{
							resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, digitalInputConfiguration.GetDigitalInput(num2).MappedCANId, Resources.ErrorSameIdForDifferentInputs);
							flag3 = true;
						}
						else
						{
							hashSet.Add(num3);
						}
					}
				}
				flag &= !flag3;
			}
			return flag;
		}

		private bool ValidateIncludeFileConfiguration(IncludeFileConfiguration includeFileConfiguration, string configFolderPath, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			foreach (IncludeFile current in includeFileConfiguration.IncludeFiles)
			{
				string absolutePath = FileSystemServices.GetAbsolutePath(current.FilePath.Value, configFolderPath);
				FileInfo fileInfo = new FileInfo(absolutePath);
				if (!fileInfo.Exists)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.FilePath, Resources.ErrorFileNotFound);
					result = false;
				}
				foreach (ValidatedProperty<string> current2 in current.Parameters)
				{
					if (!string.IsNullOrEmpty(current2.Value) && current2.Value.IndexOfAny(ModelValidator.invalidIncludeParameterChars) >= 0)
					{
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current2, Resources.ErrorIncludeFileParameter);
						result = false;
					}
				}
			}
			return result;
		}

		private bool ValidateGPS(GPSConfiguration gpsConfig, CANChannelConfiguration canChannelConfig, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (gpsConfig.MapToCANMessage.Value)
			{
				uint num;
				if (gpsConfig.IsExtendedStartCANId.Value)
				{
					num = Constants.MaximumExtendedCANId - 5u;
				}
				else
				{
					num = Constants.MaximumStandardCANId - 5u;
				}
				if (gpsConfig.StartCANId.Value > num)
				{
					string arg = GUIUtil.CANIdToDisplayString(num, gpsConfig.IsExtendedStartCANId.Value);
					string arg2 = GUIUtil.CANIdToDisplayString(0u, gpsConfig.IsExtendedStartCANId.Value);
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, gpsConfig.StartCANId, string.Format(Resources.ErrorIdOutOfRange, arg2, arg));
					result = false;
				}
				if ((ulong)gpsConfig.CANChannel.Value <= (ulong)((long)canChannelConfig.CANChannels.Count) && !canChannelConfig.GetCANChannel(gpsConfig.CANChannel.Value).IsActive.Value)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, gpsConfig.CANChannel, Resources.ErrorChannelInactive);
					return false;
				}
			}
			else if (gpsConfig.MapToSystemChannel.Value)
			{
				if (this.LoggerSpecifics.GPS.HasCANgpsSupport)
				{
					if (gpsConfig.Database.Value.Length == 0)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, gpsConfig.Database, Resources.ErrorNoGPSDatabase);
						result = false;
					}
					else if (gpsConfig.CANIdDateTimeAltitude.Value == 0u || gpsConfig.CANIdLongitudeLatitude.Value == 0u || gpsConfig.CANIdVelocityDirection.Value == 0u)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, gpsConfig.Database, Resources.ErrorBadGPSDatabase);
						result = false;
					}
					else if (gpsConfig.LongitudeLatitudeMode != 1)
					{
						resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, gpsConfig.Database, string.Format(Resources.ErrorGPSDatabaseWithUnsupportedDataMessage2Mode, gpsConfig.LongitudeLatitudeMode));
						result = false;
					}
				}
				if ((ulong)gpsConfig.CANChannel.Value <= (ulong)((long)canChannelConfig.CANChannels.Count) && !canChannelConfig.GetCANChannel(gpsConfig.CANChannel.Value).IsActive.Value)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, gpsConfig.CANChannel, Resources.ErrorChannelInactive);
					return false;
				}
			}
			return result;
		}

		private bool ValidateWLANSettings(WLANConfiguration wlanConfig, LogDataStorage logDataStorage, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			if (wlanConfig.IsStartWLANor3GOnShutdownEnabled.Value && !wlanConfig.IsWLANor3GDownloadRingbufferEnabled.Value && !wlanConfig.IsWLANor3GDownloadClassificationEnabled.Value && !wlanConfig.IsWLANor3GDownloadDriveRecorderEnabled.Value)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, wlanConfig.IsWLANor3GDownloadRingbufferEnabled, Resources.ErrorAtLeastOneTypeDownload);
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, wlanConfig.IsWLANor3GDownloadClassificationEnabled, Resources.ErrorAtLeastOneTypeDownload);
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, wlanConfig.IsWLANor3GDownloadDriveRecorderEnabled, Resources.ErrorAtLeastOneTypeDownload);
				flag = false;
			}
			if (wlanConfig.IsStartWLANOnEventEnabled.Value && !wlanConfig.IsWLANDownloadRingbufferEnabled.Value && !wlanConfig.IsWLANDownloadClassificationEnabled.Value && !wlanConfig.IsWLANDownloadDriveRecorderEnabled.Value)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, wlanConfig.IsWLANDownloadRingbufferEnabled, Resources.ErrorAtLeastOneTypeDownload);
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, wlanConfig.IsWLANDownloadClassificationEnabled, Resources.ErrorAtLeastOneTypeDownload);
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, wlanConfig.IsWLANDownloadDriveRecorderEnabled, Resources.ErrorAtLeastOneTypeDownload);
				flag = false;
			}
			if (wlanConfig.IsStartWLANOnEventEnabled.Value && wlanConfig.StartWLANEvent is KeyEvent)
			{
				flag &= this.ValidateKeyEvent(wlanConfig.StartWLANEvent as KeyEvent, resultCollector);
			}
			if (wlanConfig.IsStartWLANor3GOnShutdownEnabled.Value && (!logDataStorage.IsEnterSleepModeEnabled.Value || (logDataStorage.IsEnterSleepModeEnabled.Value && logDataStorage.TimeoutToSleep.Value == 0u)))
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, wlanConfig.IsStartWLANor3GOnShutdownEnabled, Resources.ErrorTimoutToSleepActiveForWLAN3G);
				flag = false;
			}
			flag &= this.ValidateLoggerIpConfiguration(this.configManager.InterfaceModeConfiguration, wlanConfig, resultCollector);
			if (wlanConfig.IsStartThreeGOnEventEnabled.Value && this.LoggerSpecifics.DataTransfer.IsMLserverSetupInLTL)
			{
				if (wlanConfig.LoggerIp.Value.Equals(wlanConfig.GatewayIp.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, wlanConfig.LoggerIp, Resources.ErrorLoggerIpEqualsGatewayIp);
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, wlanConfig.GatewayIp, Resources.ErrorLoggerIpEqualsGatewayIp);
					flag = false;
				}
				if (wlanConfig.LoggerIp.Value.Equals(wlanConfig.MLserverIp.Value))
				{
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, wlanConfig.LoggerIp, Resources.ErrorLoggerIpEqualsMLserverIp);
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, wlanConfig.MLserverIp, Resources.ErrorLoggerIpEqualsMLserverIp);
					flag = false;
				}
				if (flag)
				{
					flag &= this.ValidateSubnetConfiguration(wlanConfig, resultCollector);
				}
			}
			bool flag2 = false;
			foreach (TriggerConfiguration current in this.TriggerConfigurations)
			{
				if (current.MemoryRingBuffer.IsActive.Value && current.MemoryRingBuffer.OperatingMode.Value == RingBufferOperatingMode.stopLogging)
				{
					flag2 = true;
				}
			}
			if (wlanConfig.IsStartThreeGOnEventEnabled.Value && flag2)
			{
				resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, wlanConfig.IsStartThreeGOnEventEnabled, Resources.Error3GandLoggingModeNotAllowed);
				flag = false;
			}
			return flag;
		}

		private bool ValidateSubnetConfiguration(WLANConfiguration wlanConfig, IModelValidationResultCollector resultCollector)
		{
			uint num = GUIUtil.ConvertIPToUint(wlanConfig.SubnetMask.Value);
			uint num2 = GUIUtil.ConvertIPToUint(wlanConfig.LoggerIp.Value);
			uint num3 = GUIUtil.ConvertIPToUint(wlanConfig.GatewayIp.Value);
			uint num4 = num2 & num;
			uint num5 = num3 & num;
			if (num4 != num5)
			{
				resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, wlanConfig.GatewayIp, Resources.ErrorGatewaySubnetDifferentToLoggerSubnet);
				return false;
			}
			return true;
		}

		private bool ValidateThreeGDataTransferTriggers(WLANConfiguration wlanConfiguration, DataTransferTriggerConfiguration threeGDataTransferTriggerConfig, IModelValidationResultCollector resultCollector)
		{
			bool flag = true;
			KeyEvent keyEvent = null;
			if (wlanConfiguration.IsStartWLANOnEventEnabled.Value && wlanConfiguration.StartWLANEvent is KeyEvent)
			{
				keyEvent = (wlanConfiguration.StartWLANEvent as KeyEvent);
			}
			List<DataTransferTrigger> list = new List<DataTransferTrigger>();
			List<DataTransferTrigger> list2 = new List<DataTransferTrigger>();
			List<DataTransferTrigger> list3 = new List<DataTransferTrigger>();
			List<DataTransferTrigger> list4 = new List<DataTransferTrigger>();
			foreach (DataTransferTrigger current in threeGDataTransferTriggerConfig.DataTransferTriggers)
			{
				if (current.IsActive.Value)
				{
					if (current.Event is KeyEvent)
					{
						flag &= this.ValidateKeyEvent(current.Event as KeyEvent, resultCollector);
						if (keyEvent != null && keyEvent.Equals(current.Event as KeyEvent))
						{
							resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (current.Event as KeyEvent).Number, Resources.ErrorStartConnCondMustDiffer);
							flag = false;
						}
						list3.Add(current);
					}
					else if (current.Event is NextLogSessionStartEvent)
					{
						list.Add(current);
					}
					else if (current.Event is OnShutdownEvent)
					{
						list2.Add(current);
					}
					else if (current.Event is ReferencedRecordTriggerNameEvent)
					{
						list4.Add(current);
					}
				}
			}
			if (list.Count > 1)
			{
				foreach (DataTransferTrigger current2 in list)
				{
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current2.IsActive, Resources.ErrorOnlyOneNextLogSessionStartEvent);
				}
				flag = false;
			}
			if (list2.Count > 1)
			{
				foreach (DataTransferTrigger current3 in list2)
				{
					resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, current3.IsActive, Resources.ErrorOnlyOneShutdownEvent);
				}
				flag = false;
			}
			if (list3.Count > 1)
			{
				int num = 0;
				while (num + 1 < list3.Count)
				{
					KeyEvent keyEvent2 = list3[num].Event as KeyEvent;
					if (keyEvent2 != null)
					{
						for (int i = num + 1; i < list3.Count; i++)
						{
							KeyEvent keyEvent3 = list3[i].Event as KeyEvent;
							if (keyEvent2.Equals(keyEvent3))
							{
								if (keyEvent3 != null)
								{
									resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, keyEvent3.Number, Resources.ErrorKeyEvCondMustDiffer);
								}
								resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, keyEvent2.Number, Resources.ErrorKeyEvCondMustDiffer);
								flag = false;
							}
						}
					}
					num++;
				}
			}
			if (list4.Count > 0)
			{
				IDictionary<ulong, string> uniqueIdAndNameOfActiveRecordTriggers = ((IModelValidator)this).GetUniqueIdAndNameOfActiveRecordTriggers();
				Dictionary<string, ReferencedRecordTriggerNameEvent> dictionary = new Dictionary<string, ReferencedRecordTriggerNameEvent>();
				foreach (DataTransferTrigger current4 in list4)
				{
					ReferencedRecordTriggerNameEvent referencedRecordTriggerNameEvent = current4.Event as ReferencedRecordTriggerNameEvent;
					if (!dictionary.ContainsKey(referencedRecordTriggerNameEvent.NameOfTrigger.Value))
					{
						dictionary.Add(referencedRecordTriggerNameEvent.NameOfTrigger.Value, referencedRecordTriggerNameEvent);
						bool flag2 = false;
						for (uint num2 = 1u; num2 <= this.LoggerSpecifics.DataStorage.NumberOfMemories; num2 += 1u)
						{
							if (string.Compare(referencedRecordTriggerNameEvent.NameOfTrigger.Value, string.Format(ReferencedRecordTriggerNameEvent.WildcardTriggerNameOnMemory, num2)) == 0)
							{
								if ((ulong)(num2 - 1u) < (ulong)((long)this.TriggerConfigurations.Count))
								{
									if (!this.TriggerConfigurations[(int)(num2 - 1u)].MemoryRingBuffer.IsActive.Value)
									{
										resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, referencedRecordTriggerNameEvent.NameOfTrigger, Resources_Trigger.ErrorMemoryIsInactive);
										flag = false;
									}
									else if (this.TriggerConfigurations[(int)(num2 - 1u)].TriggerMode.Value != TriggerMode.Triggered)
									{
										resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, referencedRecordTriggerNameEvent.NameOfTrigger, Resources_Trigger.ErrorTriggerModeNotTriggered);
										flag = false;
									}
								}
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							if (!uniqueIdAndNameOfActiveRecordTriggers.ContainsKey(referencedRecordTriggerNameEvent.UniqueIdOfTrigger))
							{
								resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, referencedRecordTriggerNameEvent.NameOfTrigger, Resources_Trigger.ErrorCannotResolveTriggerName);
								flag = false;
							}
							else
							{
								IList<ulong> uniqueIdsForNameOfActiveRecordTrigger = ((IModelValidator)this).GetUniqueIdsForNameOfActiveRecordTrigger(referencedRecordTriggerNameEvent.NameOfTrigger.Value);
								if (uniqueIdsForNameOfActiveRecordTrigger.Count > 1)
								{
									resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, referencedRecordTriggerNameEvent.NameOfTrigger, Resources_Trigger.ErrorTriggerNameUsedMoreThanOnce);
									flag = false;
								}
							}
						}
					}
					else
					{
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, referencedRecordTriggerNameEvent.NameOfTrigger, Resources_Trigger.ErrorMultiple3GConLoggingTriggerSameCond);
						resultCollector.SetErrorText(ValidationErrorClass.LocalModelError, dictionary[referencedRecordTriggerNameEvent.NameOfTrigger.Value].NameOfTrigger, Resources_Trigger.ErrorMultiple3GConLoggingTriggerSameCond);
						flag = false;
					}
				}
			}
			return flag;
		}

		private bool ValidateLedConfiguration(LEDConfiguration ledConfiguration, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			foreach (LEDConfigListItem current in ledConfiguration.LEDConfigList)
			{
				if (current.UsedParameters == LEDItemParameter.ChannelWithWildcard || current.UsedParameters == LEDItemParameter.ChannelSingle)
				{
					if (current.ParameterChannelNumber.Value != 4294967295u)
					{
						if (!this.IsHardwareChannelAvailable(current.UsedBusType, current.ParameterChannelNumber.Value))
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.ParameterChannelNumber, Resources.ErrorChannelNotAvailable);
							result = false;
						}
						else if (!this.IsHardwareChannelActive(current.UsedBusType, current.ParameterChannelNumber.Value))
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.ParameterChannelNumber, Resources.ErrorChannelInactive);
							result = false;
						}
					}
				}
				else if (((current.UsedParameters & LEDItemParameter.MemorySingle) == LEDItemParameter.MemorySingle || (current.UsedParameters & LEDItemParameter.MemoryWithORWildcard) == LEDItemParameter.MemoryWithORWildcard || (current.UsedParameters & LEDItemParameter.MemoryWithANDWildcard) == LEDItemParameter.MemoryWithANDWildcard) && current.ParameterMemory.Value != 2147483647u && current.ParameterMemory.Value != 2147483646u && current.ParameterMemory.Value != 0u && (long)this.TriggerConfigurations.Count <= (long)((ulong)current.ParameterMemory.Value) && !this.TriggerConfigurations[(int)(current.ParameterMemory.Value - 1u)].MemoryRingBuffer.IsActive.Value)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.ParameterMemory, Resources.ErrorMemoryInactive);
					result = false;
				}
			}
			if (this.configManager.LoggerSpecifics.IO.NumberOfLEDsTotal >= 8u)
			{
				uint num = 7u - (Constants.StartIndexForRemoteLeds - this.configManager.LoggerSpecifics.IO.NumberOfLEDsOnBoard);
				int num2;
				bool flag;
				if ((ulong)num < (ulong)((long)this.LEDConfiguration.LEDConfigList.Count) && ledConfiguration.LEDConfigList[(int)num].State.Value != LEDState.Disabled && ((IModelValidator)this).IsActiveVoCANEventConfigured(out num2, out flag) && flag)
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, ledConfiguration.LEDConfigList[(int)num].State, Resources.ErrorVoCAnEventAlreadyUsesLed8);
					result = false;
				}
			}
			if (this.LoggerSpecifics.CAN.AuxChannel <= this.LoggerSpecifics.CAN.NumberOfChannels)
			{
				CANChannel cANChannel = this.configManager.CANChannelConfiguration.GetCANChannel(this.LoggerSpecifics.CAN.AuxChannel);
				if (cANChannel != null && (!cANChannel.IsActive.Value || cANChannel.CANChipConfiguration.Baudrate != Constants.AuxChannelBaudrate || !cANChannel.IsOutputActive.Value))
				{
					for (uint num3 = 7u - (Constants.StartIndexForRemoteLeds - this.configManager.LoggerSpecifics.IO.NumberOfLEDsOnBoard); num3 < this.configManager.LoggerSpecifics.IO.NumberOfLEDsTotal; num3 += 1u)
					{
						if ((ulong)num3 < (ulong)((long)ledConfiguration.LEDConfigList.Count) && ledConfiguration.LEDConfigList[(int)num3].State.Value != LEDState.Disabled)
						{
							resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, ledConfiguration.LEDConfigList[(int)num3].State, string.Format(Resources.AuxLedRequiresCanChnSettingsNotSet, this.LoggerSpecifics.CAN.AuxChannel, Constants.AuxChannelBaudrate));
							result = false;
						}
					}
				}
			}
			return result;
		}

		private bool ValidateLoggerIpConfiguration(InterfaceModeConfiguration interfaceModeConfiguration, WLANConfiguration wlanConfig, IModelValidationResultCollector resultCollector)
		{
			bool result = true;
			if (interfaceModeConfiguration.UseInterfaceMode.Value && wlanConfig.IsStartThreeGOnEventEnabled.Value && this.LoggerSpecifics.DataTransfer.IsMLserverSetupInLTL)
			{
				IPAddress iPAddress;
				if (!IPAddress.TryParse(wlanConfig.LoggerIp.Value, out iPAddress))
				{
					return false;
				}
				IPAddress obj;
				if (!IPAddress.TryParse(interfaceModeConfiguration.IpAddress.Value, out obj))
				{
					return false;
				}
				if (!iPAddress.Equals(obj))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, interfaceModeConfiguration.IpAddress, Resources.ErrorDifferentIPsForInterfaceModeAnd3G);
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, wlanConfig.LoggerIp, Resources.ErrorDifferentIPsForInterfaceModeAnd3G);
					result = false;
				}
				IPAddress iPAddress2;
				if (!IPAddress.TryParse(wlanConfig.SubnetMask.Value, out iPAddress2))
				{
					return false;
				}
				IPAddress obj2;
				if (!IPAddress.TryParse(interfaceModeConfiguration.SubnetMask.Value, out obj2))
				{
					return false;
				}
				if (!iPAddress2.Equals(obj2))
				{
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, interfaceModeConfiguration.SubnetMask, Resources.ErrorDifferentSubnetMasksForInterfaceModeAnd3G);
					resultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, wlanConfig.SubnetMask, Resources.ErrorDifferentSubnetMasksForInterfaceModeAnd3G);
					result = false;
				}
			}
			return result;
		}
	}
}
