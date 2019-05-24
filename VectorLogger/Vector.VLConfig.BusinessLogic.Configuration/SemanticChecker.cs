using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using Vector.VLConfig.BusinessLogic.Conversion;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.TriggersPage;
using Vector.VLConfig.HardwareAccess;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public class SemanticChecker : ISemanticChecker
	{
		private IConfigurationManagerService configManager;

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

		private ProjectRoot ProjectRoot
		{
			get
			{
				return this.configManager.ProjectRoot;
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

		public ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return this.configManager.LoggerSpecifics;
			}
		}

		public SemanticChecker(IConfigurationManagerService configMan)
		{
			this.configManager = configMan;
		}

		bool ISemanticChecker.IsConfigurationSound(GlobalOptions globalOptions, out string errorText)
		{
			return this.IsConfigurationSound(globalOptions, this.ProjectRoot, this.ConfigFolderPath, this.ApplicationDatabaseManager, out errorText);
		}

		bool ISemanticChecker.IsTriggerConfigurationSound(out string errorText)
		{
			return this.IsTriggerConfigurationSound(this.ProjectRoot, out errorText);
		}

		bool ISemanticChecker.IsTriggerConfigurationCaplCompliant(out string errorText)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string value;
			bool flag = this.IsTriggerConfigurationCaplCompliant(this.ProjectRoot, out value);
			if (!flag)
			{
				stringBuilder.AppendLine(value);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine(Resources.QuestionCreateSysVarDefsForNonCaplCompliantNames);
			}
			errorText = stringBuilder.ToString();
			return flag;
		}

		bool ISemanticChecker.IsDiagEventsCyclicTimerIntervalSound(out string errorText)
		{
			return this.IsDiagEventsCyclicTimerIntervalSound(this.ProjectRoot, out errorText);
		}

		bool ISemanticChecker.IsCCPTimeoutConfigurationSound(out string errorText)
		{
			return this.IsCCPTimeoutConfigurationSound(this.DatabaseConfiguration, this.LogDataStorage, this.ConfigFolderPath, this.ApplicationDatabaseManager, out errorText);
		}

		bool ISemanticChecker.IsConfigurationSoundForOnlineLogger(ILoggerDevice device, out string errorText)
		{
			return this.IsConfigurationSoundForOnlineLogger(this.ProjectRoot, device, this.LoggerSpecifics, out errorText);
		}

		bool ISemanticChecker.IsCANChannelConfigSoundForVoCANTrigger(out string errorText)
		{
			return this.IsCANChannelConfigSoundForVoCANTrigger(out errorText);
		}

		bool ISemanticChecker.IsWLANConfigSoundWithInstalledExtensionBoard(ILoggerDevice device, out string errorText)
		{
			return this.IsWLANConfigSoundWithInstalledExtensionBoard(this.WLANConfiguration, device, this.LoggerSpecifics, out errorText);
		}

		bool ISemanticChecker.IsAnalogInputConfigSoundWithInstalledExtensionBoard(ILoggerDevice device, out string errorText)
		{
			return this.IsAnalogInputConfigSoundWithInstalledExtensionBoard(this.AnalogInputConfiguration, device, this.LoggerSpecifics, out errorText);
		}

		bool ISemanticChecker.HasMultipleMsgIDsInDBsOnSameChannel(FileConversionParameters conversionParameters, out string msgText)
		{
			return SemanticChecker.HasMultipleMsgIDsInDBsOnSameChannel(conversionParameters, this.DatabaseConfiguration, this.ConfigFolderPath, this.ApplicationDatabaseManager, this.LoggerSpecifics, out msgText);
		}

		bool ISemanticChecker.IsFlexrayDbAddable(ref List<uint> freeChannelNumbers)
		{
			return this.IsFlexrayDbAddable(this.DatabaseConfiguration, this.FlexrayChannelConfiguration, ref freeChannelNumbers);
		}

		bool ISemanticChecker.IsDiagDescriptionContainingOemSpecificEcus(DiagnosticsDatabase db, string requiredOemName, DiagnosticsProtocolType requiredProtType)
		{
			return this.IsDiagDescriptionContainingOemSpecificEcus(db, this.ConfigFolderPath, this.DiagSymbolsManager, requiredOemName, requiredProtType);
		}

		bool ISemanticChecker.IsEthernetDbAddable()
		{
			return this.IsEthernetDbAddable(this.DatabaseConfiguration);
		}

		private bool IsConfigurationSound(GlobalOptions globalOptions, ProjectRoot root, string configFolderPath, IApplicationDatabaseManager dbManager, out string errorText)
		{
			errorText = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			string value;
			if (!this.IsTriggerConfigurationSound(root, out value))
			{
				stringBuilder.AppendLine(value);
			}
			if (!this.IsDiagEventsCyclicTimerIntervalSound(root, out value))
			{
				stringBuilder.AppendLine(value);
				stringBuilder.AppendLine();
			}
			if (!this.IsCCPTimeoutConfigurationSound(root.LoggingConfiguration.DatabaseConfiguration, root.HardwareConfiguration.LogDataStorage, configFolderPath, dbManager, out value))
			{
				stringBuilder.AppendLine(value);
				stringBuilder.AppendLine();
			}
			if (!this.IsWLANConfigDataTransferConfigSound(root.LoggingConfiguration.WLANConfiguration, out value))
			{
				stringBuilder.AppendLine(value);
				stringBuilder.AppendLine();
			}
			if (!this.IsCANChannelsOuputIncludeFilesSound(root.HardwareConfiguration.CANChannelConfiguration, root.LoggingConfiguration.IncludeFileConfiguration, out value))
			{
				stringBuilder.AppendLine(value);
				stringBuilder.AppendLine();
			}
			if (globalOptions.CaplComplianceTest && !this.IsTriggerConfigurationCaplCompliant(root, out value))
			{
				stringBuilder.AppendLine(value);
				stringBuilder.AppendLine();
			}
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine(Resources.QuestionContinueAnyway);
				errorText = stringBuilder.ToString();
				return false;
			}
			return true;
		}

		private bool IsTriggerConfigurationSound(ProjectRoot root, out string errorText)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool result = true;
			bool flag = root.LoggingConfiguration.NumberOfMemoryUnits > 1u;
			foreach (TriggerConfiguration current in root.LoggingConfiguration.TriggerConfigurationsOfActiveMemories)
			{
				if (current.MemoryRingBuffer.IsActive.Value)
				{
					if (current.TriggerMode.Value == TriggerMode.Triggered)
					{
						if (current.ActiveTriggers.Count == 0)
						{
							stringBuilder.AppendLine(flag ? string.Format(Resources_Trigger.ConfigUsesTriggeredLogOnMemButNoTriggers, current.MemoryNr) : Resources_Trigger.ConfigUsesTriggeredLoggingButNoTriggers);
							result = false;
						}
					}
					else if (current.TriggerMode.Value == TriggerMode.OnOff)
					{
						bool flag2;
						if (current.ActiveOnOffTriggers.Count == 0)
						{
							flag2 = true;
						}
						else
						{
							flag2 = current.OnOffTriggers.All((RecordTrigger trigger) => trigger.TriggerEffect.Value != TriggerEffect.LoggingOn);
						}
						if (flag2)
						{
							stringBuilder.AppendLine(flag ? string.Format(Resources_Trigger.ConfigUsesCondOnOffLogOnMemButNoOnTrigger, current.MemoryNr) : Resources_Trigger.ConfigUsesCondOnOffLoggingButNoOnTrigger);
							result = false;
						}
					}
					if (this.LoggerSpecifics.DataStorage.RingBufferSizeAppliesToPreTriggerTimeOnly && current.TriggerMode.Value == TriggerMode.Triggered)
					{
						BufferSizeCalculator bufferSizeCalculator = new BufferSizeCalculator(this.configManager, current.PostTriggerTime.Value);
						if (!bufferSizeCalculator.Calculate())
						{
							stringBuilder.AppendLine(Resources.ErrorPreTriggerTimeOutOfRage);
							result = false;
						}
					}
				}
			}
			errorText = stringBuilder.ToString();
			return result;
		}

		private bool IsTriggerConfigurationCaplCompliant(ProjectRoot root, out string errorText)
		{
			bool result = true;
			errorText = string.Empty;
			List<string> list = new List<string>();
			foreach (TriggerConfiguration current in root.LoggingConfiguration.TriggerConfigurationsOfActiveMemories)
			{
				ReadOnlyCollection<RecordTrigger> readOnlyCollection = null;
				switch (current.TriggerMode.Value)
				{
				case TriggerMode.Triggered:
					readOnlyCollection = current.ActiveTriggers;
					break;
				case TriggerMode.Permanent:
					readOnlyCollection = current.ActivePermanentMarkers;
					break;
				case TriggerMode.OnOff:
					readOnlyCollection = current.ActiveOnOffMarkersOnly;
					break;
				}
				if (readOnlyCollection != null)
				{
					foreach (RecordTrigger current2 in readOnlyCollection)
					{
						list.Add(current2.Name.Value);
					}
				}
			}
			foreach (string current3 in list)
			{
				if (!GenerationUtil.StringIsCaplCompliant(current3))
				{
					result = false;
					errorText = Resources_Trigger.ConfigUsesNonCaplCompliantTriggerNames;
					break;
				}
			}
			return result;
		}

		private bool IsDiagEventsCyclicTimerIntervalSound(ProjectRoot root, out string errorText)
		{
			errorText = string.Empty;
			if (!root.HardwareConfiguration.LogDataStorage.IsEnterSleepModeEnabled.Value || (root.HardwareConfiguration.LogDataStorage.IsEnterSleepModeEnabled.Value && root.HardwareConfiguration.LogDataStorage.StopCyclicCommunicationEvent != null))
			{
				return true;
			}
			uint num = root.HardwareConfiguration.LogDataStorage.TimeoutToSleep.Value * 1000u;
			foreach (TriggeredDiagnosticActionSequence current in root.LoggingConfiguration.DiagnosticActionsConfiguration.TriggeredActionSequences)
			{
				if (current.Event is CyclicTimerEvent)
				{
					CyclicTimerEvent cyclicTimerEvent = current.Event as CyclicTimerEvent;
					uint num2 = 0u;
					switch (cyclicTimerEvent.TimeUnit.Value)
					{
					case TimeUnit.MilliSec:
						num2 = cyclicTimerEvent.Interval.Value;
						break;
					case TimeUnit.Sec:
						num2 = cyclicTimerEvent.Interval.Value * 1000u;
						break;
					case TimeUnit.Min:
						num2 = cyclicTimerEvent.Interval.Value * 1000u * 60u;
						break;
					}
					if (num2 <= num)
					{
						errorText = Resources.ConfigUsesCycTimerEvIntervalShorterThan;
						return false;
					}
				}
			}
			return true;
		}

		private bool IsCCPTimeoutConfigurationSound(DatabaseConfiguration databaseConfig, LogDataStorage logDataStorage, string configFolderPath, IApplicationDatabaseManager dbManager, out string errorText)
		{
			errorText = "";
			bool result = true;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			if (!logDataStorage.IsEnterSleepModeEnabled.Value || (logDataStorage.IsEnterSleepModeEnabled.Value && logDataStorage.StopCyclicCommunicationEvent != null))
			{
				flag = false;
			}
			uint num = 0u;
			bool flag2 = false;
			foreach (Database current in databaseConfig.ActiveCCPXCPDatabases)
			{
				uint num2 = 0u;
				if (dbManager.GetShortestCPTimeout(current, configFolderPath, out num2))
				{
					if (num2 == 0u)
					{
						stringBuilder.AppendLine(string.Format(Resources_CcpXcp.DbContainsCCPTimoutOfZeroRaisedTo1000, Path.GetFileName(current.FilePath.Value)));
						num2 = 1000u;
						result = false;
					}
					if (!flag2 || num2 < num)
					{
						num = num2;
						flag2 = true;
					}
				}
			}
			if (flag && flag2 && num <= logDataStorage.TimeoutToSleep.Value * 1000u)
			{
				uint num3 = num / 1000u;
				if (num3 * 1000u == num && num3 > 0u)
				{
					num3 -= 1u;
				}
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(string.Format(Resources_CcpXcp.CCPTimeoutShorterThanTimeoutToSleep, num, logDataStorage.TimeoutToSleep.Value));
				if (num3 >= Constants.MinimumTimeoutToSleep)
				{
					stringBuilder.AppendLine(string.Format(Resources.EnableEnterSleepModeIndicEventOrTimeout, num3));
				}
				else
				{
					stringBuilder.AppendLine(Resources.EnableEnterSleepModeIndicEvent);
				}
				result = false;
			}
			errorText = stringBuilder.ToString();
			return result;
		}

		private bool IsConfigurationSoundForOnlineLogger(ProjectRoot root, ILoggerDevice device, ILoggerSpecifics loggerSpecifics, out string errorText)
		{
			errorText = string.Empty;
			if (!device.HasLoggerInfo)
			{
				return true;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (root.LoggingConfiguration.DatabaseConfiguration.ActiveCCPXCPDatabases.Count > 0 && device.InstalledLicenses.IndexOf("CCP/XCP") < 0)
			{
				stringBuilder.AppendLine(Resources_CcpXcp.CCPXCPLicenseMissing);
				stringBuilder.AppendLine();
			}
			string value;
			foreach (uint current in loggerSpecifics.CAN.ChannelsWithOptionalTransceivers)
			{
				CANTransceiverType transceiver = CANTransceiverType.None;
				if (device.GetCANTransceiverTypeForChannel(current, out transceiver) && !this.IsCANChannelConfigAndTransceiverSound(current, root.HardwareConfiguration.CANChannelConfiguration.GetCANChannel(current), transceiver, loggerSpecifics.CAN.ChannelsWithWakeUpSupport.Contains(current), out value))
				{
					stringBuilder.AppendLine(value);
				}
			}
			if (!this.IsMultibusChannelConfigAndTransceiverSound(root.HardwareConfiguration.MultibusChannelConfiguration, device, loggerSpecifics, out value))
			{
				stringBuilder.AppendLine(value);
			}
			if (!this.IsAnalogInputConfigSoundWithInstalledExtensionBoard(root.HardwareConfiguration.AnalogInputConfiguration, device, loggerSpecifics, out value))
			{
				stringBuilder.AppendLine(value);
				stringBuilder.AppendLine();
			}
			if (!this.IsWLANConfigSoundWithInstalledExtensionBoard(root.LoggingConfiguration.WLANConfiguration, device, loggerSpecifics, out value))
			{
				stringBuilder.AppendLine(value);
				stringBuilder.AppendLine();
			}
			errorText = stringBuilder.ToString();
			return string.IsNullOrEmpty(errorText);
		}

		private bool IsCANChannelConfigAndTransceiverSound(uint index, CANChannel channel, CANTransceiverType transceiver, bool isWakeUpSupported, out string errorText)
		{
			errorText = string.Empty;
			if (channel.IsActive.Value)
			{
				if (transceiver == CANTransceiverType.None)
				{
					errorText = string.Format(Resources.ChannelActiveButNoTransceiver, index) + Environment.NewLine;
					return false;
				}
				StringBuilder stringBuilder = new StringBuilder();
				if (channel.CANChipConfiguration.Baudrate > Constants.MaximumCANLowSpeedTransceiverRate && CANTransceiver.IsLowSpeed(transceiver))
				{
					stringBuilder.AppendLine(string.Format(Resources.ChannelLowSpeedTransceiverMismatch, index));
				}
				if (isWakeUpSupported && channel.IsWakeUpEnabled.Value && !CANTransceiver.IsWakeupCapable(transceiver))
				{
					stringBuilder.AppendLine(string.Format(Resources.ChannelWakeupTransceiverNotCapable, index));
				}
				errorText = stringBuilder.ToString();
				if (!string.IsNullOrEmpty(errorText))
				{
					return false;
				}
			}
			return true;
		}

		private bool IsCANChannelConfigSoundForVoCANTrigger(out string errorText)
		{
			errorText = "";
			if (this.LoggerSpecifics.CAN.AuxChannel <= this.LoggerSpecifics.CAN.NumberOfChannels)
			{
				CANChannel cANChannel = this.configManager.GetHardwareChannel(BusType.Bt_CAN, this.LoggerSpecifics.CAN.AuxChannel) as CANChannel;
				if (cANChannel != null && (!cANChannel.IsActive.Value || cANChannel.CANChipConfiguration.Baudrate != Constants.AuxChannelBaudrate || !cANChannel.IsOutputActive.Value))
				{
					errorText = string.Format(Resources.VoCanRequiresCanChnSettingsNotSet, this.LoggerSpecifics.CAN.AuxChannel, Constants.AuxChannelBaudrate);
					return false;
				}
			}
			return true;
		}

		private bool IsWLANConfigSoundWithInstalledExtensionBoard(WLANConfiguration wlanConfiguration, ILoggerDevice device, ILoggerSpecifics loggerSpecifics, out string errorText)
		{
			errorText = "";
			bool isWLANExtensionBoardInstalled;
			if (device is IGL3000Device)
			{
				IGL3000Device iGL3000Device = device as IGL3000Device;
				isWLANExtensionBoardInstalled = iGL3000Device.IsWLANExtensionBoardInstalled;
			}
			else
			{
				if (!(device is IGL4000Device))
				{
					return true;
				}
				IGL4000Device iGL4000Device = device as IGL4000Device;
				isWLANExtensionBoardInstalled = iGL4000Device.IsWLANExtensionBoardInstalled;
			}
			if (isWLANExtensionBoardInstalled)
			{
				return true;
			}
			if (wlanConfiguration.IsStartWLANOnEventEnabled.Value || wlanConfiguration.IsStartWLANor3GOnShutdownEnabled.Value)
			{
				errorText = Resources.NoWLANExtBoardInstalled;
				return false;
			}
			return true;
		}

		private bool IsWLANConfigDataTransferConfigSound(WLANConfiguration wlanConfig, out string errorText)
		{
			errorText = "";
			if (wlanConfig.IsStartThreeGOnEventEnabled.Value && wlanConfig.ThreeGDataTransferTriggerConfiguration.ActiveDataTransferTriggers.Count == 0)
			{
				errorText = Resources.DataTrans3GConnActiveButNoEvent;
				return false;
			}
			return true;
		}

		private bool IsCANChannelsOuputIncludeFilesSound(CANChannelConfiguration canChannelConfig, IncludeFileConfiguration includeFileConfig, out string errorText)
		{
			errorText = "";
			bool result = true;
			if (includeFileConfig.IncludeFiles.Count > 0)
			{
				IList<uint> activeCANChannelNumbers = canChannelConfig.GetActiveCANChannelNumbers();
				foreach (uint current in activeCANChannelNumbers)
				{
					CANChannel cANChannel = canChannelConfig.GetCANChannel(current);
					if (!cANChannel.IsOutputActive.Value)
					{
						errorText = Resources.CANChannelsOutputInactiveButIncludeFiles;
						result = false;
						break;
					}
				}
			}
			return result;
		}

		private bool IsAnalogInputConfigSoundWithInstalledExtensionBoard(AnalogInputConfiguration analogInputConfig, ILoggerDevice device, ILoggerSpecifics loggerSpecifics, out string errorText)
		{
			errorText = "";
			bool isAnalogExtensionBoardInstalled;
			if (device is IGL3000Device)
			{
				IGL3000Device iGL3000Device = device as IGL3000Device;
				isAnalogExtensionBoardInstalled = iGL3000Device.IsAnalogExtensionBoardInstalled;
			}
			else
			{
				if (!(device is IGL4000Device))
				{
					return true;
				}
				IGL4000Device iGL4000Device = device as IGL4000Device;
				isAnalogExtensionBoardInstalled = iGL4000Device.IsAnalogExtensionBoardInstalled;
			}
			if (isAnalogExtensionBoardInstalled)
			{
				return true;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (uint num = loggerSpecifics.IO.NumberOfAnalogInputs - loggerSpecifics.IO.NumberOfAnalogInputsOnboard - 1u; num <= loggerSpecifics.IO.NumberOfAnalogInputs; num += 1u)
			{
				if (analogInputConfig.GetAnalogInput(num).IsActive.Value)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.AppendFormat(", {0}", num);
					}
					else
					{
						stringBuilder.Append(num.ToString());
					}
				}
			}
			if (stringBuilder.Length > 0)
			{
				errorText = string.Format(Resources.NoAnalogInputExtBoardInstalled, loggerSpecifics.IO.NumberOfAnalogInputsOnboard, stringBuilder.ToString());
				return false;
			}
			return true;
		}

		private static bool HasMultipleMsgIDsInDBsOnSameChannel(FileConversionParameters conversionParameters, DatabaseConfiguration databaseConfig, string configFolderPath, IApplicationDatabaseManager appDbManager, ILoggerSpecifics loggerSpecifics, out string msgText)
		{
			msgText = "";
			bool flag = false;
			bool flag2 = false;
			StringBuilder stringBuilder = new StringBuilder();
			string value;
			if (SemanticChecker.HasMultipleMsgIDsInDBsOnSameChannelForBusType(conversionParameters, databaseConfig, configFolderPath, BusType.Bt_CAN, appDbManager, loggerSpecifics, out value))
			{
				stringBuilder.AppendLine(value);
				flag = true;
			}
			string value2;
			if (SemanticChecker.HasMultipleMsgIDsInDBsOnSameChannelForBusType(conversionParameters, databaseConfig, configFolderPath, BusType.Bt_LIN, appDbManager, loggerSpecifics, out value2))
			{
				stringBuilder.AppendLine(value2);
				flag2 = true;
			}
			if (flag || flag2)
			{
				msgText = stringBuilder.ToString();
				return true;
			}
			return false;
		}

		private static bool HasMultipleMsgIDsInDBsOnSameChannelForBusType(FileConversionParameters conversionParameters, DatabaseConfiguration databaseConfig, string configFolderPath, BusType busType, IApplicationDatabaseManager appDbManager, ILoggerSpecifics loggerSpecifics, out string reportString)
		{
			Dictionary<uint, List<Database>> dictionary = new Dictionary<uint, List<Database>>();
			foreach (Database current in AnalysisPackage.GetConversionDatabases(conversionParameters, databaseConfig.Databases, true))
			{
				if (current.BusType.Value == busType && current.CPType.Value == CPType.None)
				{
					uint key = current.ChannelNumber.Value;
					if (conversionParameters.UseChannelMapping)
					{
						uint[] array = conversionParameters.CanChannelMapping;
						if (current.BusType.Value == BusType.Bt_LIN)
						{
							array = conversionParameters.LinChannelMapping;
						}
						if ((long)array.Length >= (long)((ulong)current.ChannelNumber.Value))
						{
							key = array[(int)((UIntPtr)(current.ChannelNumber.Value - 1u))];
						}
					}
					if (!dictionary.ContainsKey(key))
					{
						dictionary.Add(key, new List<Database>());
					}
					dictionary[key].Add(current);
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			int num = 15;
			foreach (uint current2 in dictionary.Keys)
			{
				IDictionary<uint, Dictionary<string, string>> dictionary2;
				if (dictionary[current2].Count > 1 && current2 > 0u && appDbManager.GetDuplicateMsgIDsOfConfiguredDatabases(dictionary[current2], configFolderPath, busType, out dictionary2))
				{
					int num2 = 0;
					if (busType == BusType.Bt_CAN)
					{
						stringBuilder.AppendLine(GUIUtil.MapCANChannelNumber2String(current2) + ":");
						using (IEnumerator<uint> enumerator3 = dictionary2.Keys.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								uint current3 = enumerator3.Current;
								stringBuilder.AppendLine("  " + GUIUtil.CANDbCANIdToDisplayString(current3) + ": \t" + SemanticChecker.GenerateDBNameList(dictionary2[current3]));
								num2++;
								if (num2 >= num && dictionary2.Count > num)
								{
									stringBuilder.AppendLine("  " + string.Format(Resources.MoreMultipleMsgIdDefOnChannel, dictionary2.Count - num, GUIUtil.MapCANChannelNumber2String(current2)));
									break;
								}
							}
							continue;
						}
					}
					if (busType == BusType.Bt_LIN)
					{
						stringBuilder.AppendLine(GUIUtil.MapLINChannelNumber2String(current2, loggerSpecifics) + ":");
						foreach (uint current4 in dictionary2.Keys)
						{
							stringBuilder.AppendLine("  " + GUIUtil.LINIdToDisplayString(current4) + ": \t" + SemanticChecker.GenerateDBNameList(dictionary2[current4]));
							num2++;
							if (num2 >= num && dictionary2.Count > num)
							{
								stringBuilder.AppendLine("  " + string.Format(Resources.MoreMultipleMsgIdDefOnChannel, dictionary2.Count - num, GUIUtil.MapLINChannelNumber2String(current2, loggerSpecifics)));
								break;
							}
						}
					}
				}
			}
			reportString = stringBuilder.ToString();
			return !string.IsNullOrEmpty(reportString);
		}

		private static string GenerateDBNameList(Dictionary<string, string> dbNameToSymMsgName)
		{
			StringBuilder stringBuilder = new StringBuilder();
			int arg_0C_0 = dbNameToSymMsgName.Count;
			int num = 0;
			foreach (string current in dbNameToSymMsgName.Keys)
			{
				stringBuilder.Append(current);
				num++;
				if (num < dbNameToSymMsgName.Count)
				{
					stringBuilder.Append(", ");
				}
			}
			return stringBuilder.ToString();
		}

		private bool IsFlexrayDbAddable(DatabaseConfiguration databaseConfig, FlexrayChannelConfiguration flexrayChannelConfig, ref List<uint> freeChannelNumbers)
		{
			ReadOnlyCollection<Database> flexrayDatabases = databaseConfig.FlexrayDatabases;
			if ((long)flexrayDatabases.Count >= (long)((ulong)this.LoggerSpecifics.Flexray.NumberOfChannels))
			{
				InformMessageBox.Error(Resources.ErrorUnableToAddMoreFlexrayDbs);
				return false;
			}
			freeChannelNumbers.Clear();
			for (uint num = 1u; num <= this.LoggerSpecifics.Flexray.NumberOfChannels; num += 1u)
			{
				freeChannelNumbers.Add(num);
			}
			if (flexrayDatabases.Count == 0)
			{
				return true;
			}
			if (flexrayDatabases[0].ChannelNumber.Value == Database.ChannelNumber_FlexrayAB)
			{
				InformMessageBox.Error(Resources.ErrorUnableToAddMoreFlexrayDbs);
				return false;
			}
			if (CPType.XCP == flexrayDatabases[0].CPType.Value)
			{
				InformMessageBox.Error(Resources_CcpXcp.ErrorFlexrayDbXCPAlreadyConfigured);
				return false;
			}
			foreach (Database current in flexrayDatabases)
			{
				if (freeChannelNumbers.Contains(current.ChannelNumber.Value))
				{
					freeChannelNumbers.Remove(current.ChannelNumber.Value);
				}
			}
			return freeChannelNumbers.Count > 0;
		}

		private bool IsDiagDescriptionContainingOemSpecificEcus(DiagnosticsDatabase db, string configFolderPath, IDiagSymbolsManager dsManager, string requiredOemName, DiagnosticsProtocolType requiredProtType)
		{
			string absolutePath = FileSystemServices.GetAbsolutePath(db.FilePath.Value, configFolderPath);
			IList<string> list;
			dsManager.GetEcusInDiagnosticsDatabaseFromOEMHeuristic(absolutePath, requiredOemName, out list);
			bool result = false;
			if (list.Count > 0)
			{
				foreach (DiagnosticsECU current in db.ECUs)
				{
					if (current.DiagnosticCommParamsECU.DiagProtocol.Value == requiredProtType && list.Contains(current.Qualifier.Value))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		private bool IsMultibusChannelConfigAndTransceiverSound(MultibusChannelConfiguration multibusChannelConfig, ILoggerDevice device, ILoggerSpecifics loggerSpecifics, out string errorText)
		{
			bool flag = true;
			errorText = "";
			StringBuilder stringBuilder = new StringBuilder();
			if (device is IMultibusChannelDevice && loggerSpecifics.Multibus.NumberOfChannels > 0u)
			{
				IMultibusChannelDevice multibusChannelDevice = device as IMultibusChannelDevice;
				for (uint num = 1u; num <= loggerSpecifics.Multibus.NumberOfChannels; num += 1u)
				{
					HardwareChannel channel = multibusChannelConfig.GetChannel(num);
					if (channel.IsActive.Value)
					{
						BusType transceiverBusType = multibusChannelDevice.GetTransceiverBusType((int)num);
						if (channel is CANChannel)
						{
							if (transceiverBusType != BusType.Bt_CAN)
							{
								stringBuilder.AppendLine(string.Format(Resources.ErrorMultibusChnConfigConnDevTransceiver, num, Vocabulary.CAN));
								flag = false;
							}
							else
							{
								bool flag2;
								bool flag3;
								multibusChannelDevice.GetCANTransceiverCapabilities((int)num, out flag2, out flag3);
								if (!flag3)
								{
									if (multibusChannelConfig.CANChannels[num].CANChipConfiguration.IsCANFD)
									{
										stringBuilder.AppendLine(string.Format(Resources.ErrorMultibusChnConfigCANFDDevTransceiverNotFD, num));
										flag = false;
									}
									else if (flag2 && multibusChannelConfig.CANChannels[num].CANChipConfiguration.Baudrate > Constants.MaximumCANLowSpeedTransceiverRate)
									{
										stringBuilder.AppendLine(string.Format(Resources.ErrorMultibusChnConfigBaudrateExceedsCANLow, num));
										flag = false;
									}
								}
							}
						}
						else if (channel is LINChannel)
						{
							if (transceiverBusType != BusType.Bt_LIN)
							{
								stringBuilder.AppendLine(string.Format(Resources.ErrorMultibusChnConfigConnDevTransceiver, num, Vocabulary.LIN));
								flag = false;
							}
						}
						else if (channel is J1708Channel)
						{
							if (transceiverBusType != BusType.Bt_J1708)
							{
								stringBuilder.AppendLine(string.Format(Resources.ErrorMultibusChnConfigConnDevTransceiver, num, Vocabulary.J1708));
								flag = false;
							}
						}
						else
						{
							stringBuilder.AppendLine(string.Format(Resources.ErrorMultibusChnConfigConnDevTransceiver, num, GUIUtil.MapBusType2String(BusType.Bt_None)));
							flag = false;
						}
					}
				}
			}
			if (!flag)
			{
				errorText = stringBuilder.ToString();
			}
			return flag;
		}

		private bool IsEthernetDbAddable(DatabaseConfiguration databaseConfig)
		{
			ReadOnlyCollection<Database> databases = databaseConfig.Databases;
			foreach (Database current in databases)
			{
				if (current.BusType.Value == BusType.Bt_Ethernet)
				{
					InformMessageBox.Error(Resources.ErrorUnableToAddMoreEthernetDbs);
					return false;
				}
			}
			return true;
		}
	}
}
