using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public class ModelEditor : IModelEditor
	{
		private IConfigurationManagerService configManager;

		private readonly string _RtfLineBreak = "\\line ";

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

		private ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return this.configManager.LoggerSpecifics;
			}
		}

		private IFeatureRegistration FeatureRegistration
		{
			get
			{
				return this.configManager.FeatureRegistration;
			}
		}

		private ProjectRoot ProjectRoot
		{
			get
			{
				return this.configManager.ProjectRoot;
			}
		}

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

		public ModelEditor(IConfigurationManagerService configMan)
		{
			this.configManager = configMan;
		}

		bool IModelEditor.CheckAndProcessDatabaseChannelRemapping(Database db, uint newChannelNumber)
		{
			return this.CheckAndProcessDatabaseChannelRemapping(db, newChannelNumber, this.DatabaseConfiguration);
		}

		void IModelEditor.ReplaceDatabaseAndUpdateSymbolicDefinitions(Database dbToReplace, string newDatabasePath)
		{
			this.ReplaceDatabaseAndUpdateSymbolicDefinitions(dbToReplace, newDatabasePath);
		}

		bool IModelEditor.ReplaceDatabase(string filepath, Database dbToReplace)
		{
			return this.ReplaceDatabase(filepath, dbToReplace);
		}

		DSMResult IModelEditor.ReplaceDiagnosticsDatabase(ref DiagnosticsDatabase dbToReplace, string newDatabasePath)
		{
			return this.ReplaceDiagnosticsDatabase(ref dbToReplace, this.DiagnosticsDatabaseConfiguration, this.DiagnosticActionsConfiguration, this.DiagSymbolsManager, this.ConfigFolderPath, newDatabasePath);
		}

		bool IModelEditor.InitDiagCommParamsEcu(DiagnosticsECU ecu)
		{
			return this.InitDiagCommParamsEcu(ecu, this.DiagSymbolsManager);
		}

		bool IModelEditor.AutoCorrectDiagCommParams(ReadOnlyCollection<DiagnosticsDatabase> diagDbs, bool isReplace, out string correctionReport)
		{
			return this.AutoCorrectDiagCommParams(diagDbs, this.DiagSymbolsManager, this.ConfigFolderPath, isReplace, out correctionReport);
		}

		void IModelEditor.SetChannelConfigurationForVoCAN()
		{
			CANChannel cANChannel = this.configManager.GetHardwareChannel(BusType.Bt_CAN, this.LoggerSpecifics.CAN.AuxChannel) as CANChannel;
			if (cANChannel == null)
			{
				return;
			}
			cANChannel.IsActive.Value = true;
			CANStdChipConfiguration cANStdChipConfiguration = cANChannel.CANChipConfiguration as CANStdChipConfiguration;
			CANChipConfigurationManager.ApplyPredefinedSettingForBaudrate(this.LoggerSpecifics.CAN.AuxChannel, Constants.AuxChannelBaudrate, ref cANStdChipConfiguration);
			cANChannel.IsOutputActive.Value = true;
			if (this.CANChannelConfiguration.CANChannels.Contains(cANChannel))
			{
				this.configManager.UpdateService.Notify<CANChannelConfiguration>(this.CANChannelConfiguration);
				return;
			}
			if (this.MultibusChannelConfiguration.CANChannels.ContainsValue(cANChannel))
			{
				this.configManager.UpdateService.Notify<CANChannelConfiguration>(this.CANChannelConfiguration);
			}
		}

		void IModelEditor.ProclaimCcpXcpEcuRenaming(string ecuNameToReplace, string newEcuName)
		{
			this.ProclaimCcpXcpEcuRenaming(ecuNameToReplace, newEcuName);
		}

		void IModelEditor.UpdateReferencedTriggerNameInDataTransferTriggers(ulong uniqueIdOfLoggingTrigger, string newName)
		{
			this.UpdateReferencedTriggerNameInDataTransferTriggers(uniqueIdOfLoggingTrigger, newName, this.WLANConfiguration.ThreeGDataTransferTriggerConfiguration);
		}

		public static void InitializeMultibusChannel(uint channelNr, ref HardwareChannel channel)
		{
			channel.IsActive.Value = true;
			if (channel is CANChannel)
			{
				CANChannel cANChannel = channel as CANChannel;
				CANStdChipConfiguration cANChipConfiguration = new CANStdChipConfiguration();
				cANChannel.CANChipConfiguration = cANChipConfiguration;
				CANChipConfigurationManager.ApplyPredefinedSettingForBaudrate(channelNr, Constants.DefaultCANBaudrateHighSpeed, ref cANChipConfiguration);
				cANChannel.IsKeepAwakeActive.Value = false;
				cANChannel.IsWakeUpEnabled.Value = false;
				cANChannel.IsOutputActive.Value = true;
				return;
			}
			if (channel is LINChannel)
			{
				LINChannel lINChannel = channel as LINChannel;
				lINChannel.IsKeepAwakeActive.Value = false;
				lINChannel.IsWakeUpEnabled.Value = false;
				lINChannel.SpeedRate.Value = Constants.DefaultLINBaudrate;
				lINChannel.ProtocolVersion.Value = Constants.DefaultLINProtocolVersion;
				lINChannel.UseDbConfigValues.Value = false;
				return;
			}
			if (channel is J1708Channel)
			{
				J1708Channel j1708Channel = channel as J1708Channel;
				j1708Channel.MaxInterCharBitTime.Value = 2u;
				j1708Channel.SpeedRate.Value = Constants.DefaultJ1708Baudrate;
			}
		}

		private bool CheckAndProcessDatabaseChannelRemapping(Database db, uint newChannelNumber, DatabaseConfiguration databaseConfiguration)
		{
			if (db.ChannelNumber.Value == newChannelNumber)
			{
				return true;
			}
			bool flag = false;
			foreach (Database current in databaseConfiguration.Databases)
			{
				if (current != db && current.FilePath.Value == db.FilePath.Value && current.ChannelNumber.Value == db.ChannelNumber.Value && current.BusType.Value == db.BusType.Value)
				{
					flag = true;
					break;
				}
			}
			List<ISymbolicMessage> list = new List<ISymbolicMessage>();
			List<ISymbolicSignal> list2 = new List<ISymbolicSignal>();
			if (!flag)
			{
				foreach (IFeatureSymbolicDefinitions current2 in this.FeatureRegistration.FeaturesUsingSymbolicDefinitions)
				{
					foreach (ISymbolicMessage current3 in current2.SymbolicMessages)
					{
						if (string.Compare(current3.DatabasePath.Value, db.FilePath.Value, true) == 0 && current3.BusType.Value == db.BusType.Value && current3.ChannelNumber.Value == db.ChannelNumber.Value)
						{
							list.Add(current3);
						}
					}
					foreach (ISymbolicSignal current4 in current2.SymbolicSignals)
					{
						if (string.Compare(current4.DatabasePath.Value, db.FilePath.Value, true) == 0 && current4.BusType.Value == db.BusType.Value && current4.ChannelNumber.Value == db.ChannelNumber.Value)
						{
							list2.Add(current4);
						}
					}
				}
				if (db.CcpXcpEcuList.Count > 0)
				{
					foreach (IFeatureCcpXcpSignalDefinitions current5 in this.FeatureRegistration.FeaturesUsingCcpXcpSignalDefinitions)
					{
						foreach (ICcpXcpSignal current6 in current5.CcpXcpSignals)
						{
							if (current6 is CcpXcpSignalEvent && current6.EcuName.Value == db.CcpXcpEcuList[0].CcpXcpEcuDisplayName)
							{
								(current6 as CcpXcpSignalEvent).ChannelNumber.Value = newChannelNumber;
							}
						}
					}
				}
			}
			if (list.Count > 0 || list2.Count > 0)
			{
				if (BusType.Bt_CAN == db.BusType.Value && newChannelNumber > this.configManager.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN))
				{
					if (DialogResult.No == InformMessageBox.Question(Resources.UnableToRemapChannelOnChangeOfDbChannel))
					{
						return false;
					}
				}
				else
				{
					string message = Resources_Trigger.ChannelRemappingOnChangeOfDbChannel;
					if (!this.LoggerSpecifics.Recording.IsDiagnosticsSupported)
					{
						message = Resources_Trigger.ChannelRemappingOnChangeOfDbChannelTriggerAndFilterOnly;
					}
					DialogResult dialogResult = InformMessageBox.QuestionWithCancel(message);
					if (dialogResult == DialogResult.Cancel)
					{
						return false;
					}
					if (dialogResult == DialogResult.Yes)
					{
						foreach (ISymbolicMessage current7 in list)
						{
							current7.ChannelNumber.Value = newChannelNumber;
						}
						foreach (ISymbolicSignal current8 in list2)
						{
							current8.ChannelNumber.Value = newChannelNumber;
						}
					}
				}
			}
			db.ChannelNumber.Value = newChannelNumber;
			this.configManager.ApplicationDatabaseManager.UpdateApplicationDatabaseConfiguration(this.DatabaseConfiguration, this.MultibusChannelConfiguration, this.CANChannelConfiguration, this.ConfigFolderPath);
			this.configManager.NotifyAllDependentFeatures(this.DatabaseConfiguration);
			return true;
		}

		private void ReplaceDatabaseAndUpdateSymbolicDefinitions(Database dbToReplace, string newDatabasePath)
		{
			string value = dbToReplace.FilePath.Value;
			uint value2 = dbToReplace.ChannelNumber.Value;
			BusType value3 = dbToReplace.BusType.Value;
			dbToReplace.FilePath.Value = newDatabasePath;
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(newDatabasePath);
			uint num = 0u;
			uint num2 = 0u;
			foreach (IFeatureSymbolicDefinitions current in this.FeatureRegistration.FeaturesUsingSymbolicDefinitions)
			{
				foreach (ISymbolicMessage current2 in current.SymbolicMessages)
				{
					if (string.Compare(current2.DatabasePath.Value, value, true) == 0 && current2.BusType.Value == value3 && current2.ChannelNumber.Value == value2)
					{
						current2.DatabasePath.Value = newDatabasePath;
						current2.DatabaseName.Value = fileNameWithoutExtension;
						num += 1u;
					}
				}
				foreach (ISymbolicSignal current3 in current.SymbolicSignals)
				{
					if (string.Compare(current3.DatabasePath.Value, value, true) == 0 && current3.BusType.Value == value3 && current3.ChannelNumber.Value == value2)
					{
						current3.DatabasePath.Value = newDatabasePath;
						current3.DatabaseName.Value = fileNameWithoutExtension;
						num2 += 1u;
					}
				}
			}
			if (num > 0u || num2 > 0u)
			{
				this.configManager.ApplicationDatabaseManager.UpdateApplicationDatabaseConfiguration(this.DatabaseConfiguration, this.MultibusChannelConfiguration, this.CANChannelConfiguration, this.ConfigFolderPath);
				this.configManager.NotifyAllDependentFeatures(this.DatabaseConfiguration);
			}
		}

		private bool ReplaceDatabase(string filepath, Database dbToReplace)
		{
			string arg;
			if (BusType.Bt_LIN == dbToReplace.BusType.Value)
			{
				arg = string.Format(Resources.LINChannelName, dbToReplace.ChannelNumber.Value);
			}
			else if (BusType.Bt_FlexRay == dbToReplace.BusType.Value)
			{
				arg = GUIUtil.MapFlexrayChannelNumber2String(dbToReplace.ChannelNumber.Value);
			}
			else
			{
				arg = string.Format(Vocabulary.CANChannelName, dbToReplace.ChannelNumber.Value);
			}
			if (this.ApplicationDatabaseManager.IsCanFdDatabase(filepath, string.Empty) && !this.configManager.LoggerSpecifics.CAN.IsFDSupported)
			{
				Cursor.Current = Cursors.Default;
				InformMessageBox.Error(Resources.ErrorCanFdNotSupportedByLoggerType);
				return false;
			}
			string message = string.Format(Resources.ConfirmDatabaseReplace, dbToReplace.FilePath.Value, arg, filepath);
			if (DialogResult.Yes == InformMessageBox.Question(message))
			{
				filepath = this.configManager.GetFilePathRelativeToConfiguration(filepath);
				this.ReplaceDatabaseAndUpdateSymbolicDefinitions(dbToReplace, filepath);
				uint extraCPChannel = 0u;
				CPType value = dbToReplace.CPType.Value;
				IDictionary<string, bool> dictionary;
				dbToReplace.CPType.Value = this.configManager.DatabaseServices.GetDatabaseCPConfiguration(dbToReplace, out dictionary, out extraCPChannel);
				if (dbToReplace.CPType.Value != CPType.None)
				{
					dbToReplace.CcpXcpIsSeedAndKeyUsed = false;
					if (dbToReplace.FileType != DatabaseFileType.A2L)
					{
						dbToReplace.CcpXcpEcuDisplayName.Value = Database.MakeCpEcuDisplayName(dictionary.Keys);
						dbToReplace.CcpXcpIsSeedAndKeyUsed = dictionary.Values.Contains(true);
					}
					if (BusType.Bt_FlexRay == dbToReplace.BusType.Value)
					{
						dbToReplace.ExtraCPChannel = extraCPChannel;
					}
					if (value != dbToReplace.CPType.Value)
					{
						dbToReplace.ClearCpProtections();
						if (dictionary.Count > 0)
						{
							dbToReplace.AddCpProtection(new CPProtection(dictionary.First<KeyValuePair<string, bool>>().Key, dictionary.First<KeyValuePair<string, bool>>().Value));
						}
					}
					else if (dictionary.Count > 0)
					{
						if (dbToReplace.CpProtections.Count > 0)
						{
							dbToReplace.CpProtections[0].ECUName.Value = dictionary.First<KeyValuePair<string, bool>>().Key;
							dbToReplace.CpProtections[0].HasSeedAndKey.Value = dictionary.First<KeyValuePair<string, bool>>().Value;
							if (!dbToReplace.CpProtections[0].HasSeedAndKey.Value)
							{
								dbToReplace.CpProtections[0].SeedAndKeyFilePath.Value = "";
							}
						}
						else
						{
							dbToReplace.AddCpProtection(new CPProtection(dictionary.First<KeyValuePair<string, bool>>().Key, dictionary.First<KeyValuePair<string, bool>>().Value));
						}
					}
					else
					{
						dbToReplace.ClearCpProtections();
					}
				}
				else if (value != CPType.None)
				{
					dbToReplace.ClearCpProtections();
					dbToReplace.IsCPActive.Value = false;
				}
				return true;
			}
			return false;
		}

		private DSMResult ReplaceDiagnosticsDatabase(ref DiagnosticsDatabase dbToReplace, DiagnosticsDatabaseConfiguration databaseConfig, DiagnosticActionsConfiguration actionsConfig, IDiagSymbolsManager dsManager, string configFolderPath, string newDatabasePath)
		{
			string arg_0C_0 = dbToReplace.FilePath.Value;
			List<string> list = new List<string>();
			foreach (DiagnosticsECU current in dbToReplace.ECUs)
			{
				list.Add(current.Qualifier.Value);
			}
			dsManager.RemoveDiagnosticsDatabase(this.configManager.GetAbsoluteFilePath(dbToReplace.FilePath.Value));
			DiagnosticsDatabase diagnosticsDatabase;
			DSMResult dSMResult = dsManager.LoadDiagnosticsDatabase(this.DiagnosticActionsConfiguration, newDatabasePath, 1u, list, out diagnosticsDatabase);
			if (dSMResult == DSMResult.OK)
			{
				string filePathRelativeToConfiguration = this.configManager.GetFilePathRelativeToConfiguration(newDatabasePath);
				diagnosticsDatabase.FilePath.Value = filePathRelativeToConfiguration;
				diagnosticsDatabase.RemoveAllECUs();
				foreach (DiagnosticsECU current2 in dbToReplace.ECUs)
				{
					diagnosticsDatabase.AddECU(current2);
				}
				databaseConfig.ReplaceDatabase(dbToReplace, diagnosticsDatabase);
				dbToReplace = diagnosticsDatabase;
				dsManager.UpdateDatabaseConfiguration(databaseConfig, actionsConfig, configFolderPath);
				foreach (IFeatureSymbolicDefinitions current3 in this.FeatureRegistration.FeaturesUsingSymbolicDefinitions)
				{
					foreach (DiagnosticAction current4 in current3.SymbolicDiagnosticActions)
					{
						current4.DatabasePath.Value = filePathRelativeToConfiguration;
					}
				}
			}
			return dSMResult;
		}

		private bool HasConsistentCommParamsCanIds(DiagnosticCommParamsECU myCommParams, DiagnosticsDatabaseConfiguration databaseConfig)
		{
			if (myCommParams.PhysRequestMsgId.Value == myCommParams.ResponseMsgId.Value && myCommParams.PhysRequestMsgIsExtendedId.Value == myCommParams.ResponseMsgIsExtendedId.Value)
			{
				return false;
			}
			foreach (DiagnosticsECU current in databaseConfig.ECUs)
			{
				if (current.DiagnosticCommParamsECU != myCommParams)
				{
					if (current.DiagnosticCommParamsECU.PhysRequestMsgId.Value == myCommParams.PhysRequestMsgId.Value && current.DiagnosticCommParamsECU.PhysRequestMsgIsExtendedId.Value == myCommParams.PhysRequestMsgIsExtendedId.Value)
					{
						bool result = false;
						return result;
					}
					if (current.DiagnosticCommParamsECU.ResponseMsgId.Value == myCommParams.ResponseMsgId.Value && current.DiagnosticCommParamsECU.ResponseMsgIsExtendedId.Value == myCommParams.ResponseMsgIsExtendedId.Value)
					{
						bool result = false;
						return result;
					}
				}
			}
			return true;
		}

		private void SetDiagCommParamsEcuDefaultValues(ref DiagnosticCommParamsECU commParams)
		{
			commParams.UseParamValuesFromDb.Value = false;
			commParams.DiagProtocol.Value = DiagnosticsProtocolType.UDS;
			commParams.P2Timeout.Value = Constants.DefaultP2Timeout;
			commParams.P2Extension.Value = Constants.DefaultP2Extension;
			commParams.PhysRequestMsgId.Value = Constants.DefaultPhysRequestMsgId;
			commParams.PhysRequestMsgIsExtendedId.Value = Constants.DefaultPhysRequestMsgIsExtendedId;
			commParams.ResponseMsgId.Value = Constants.DefaultResponseMsgId;
			commParams.ResponseMsgIsExtendedId.Value = Constants.DefaultResponseMsgIsExtendedId;
			commParams.DiagAddressingMode.Value = DiagnosticsAddressingMode.Normal;
			commParams.ExtAddressingModeRqExtension.Value = 0u;
			commParams.ExtAddressingModeRsExtension.Value = 0u;
			commParams.STMin.Value = Constants.DefaultSTMin;
			commParams.UsePaddingBytes.Value = Constants.DefaultUsePaddingBytes;
			commParams.PaddingByteValue.Value = Constants.DefaultPaddingByteValue;
			commParams.FirstConsecFrameValue.Value = Constants.DefaultFirstConsecFrameValue;
			commParams.UseStopCommRequest.Value = false;
			commParams.DefaultSessionSource.Value = DiagnosticsSessionSource.UDS_Default;
			commParams.DefaultSessionName.Value = "";
			commParams.DefaultSessionId.Value = Constants.SessionId_UDSDefault;
			commParams.ExtendedSessionSource.Value = DiagnosticsSessionSource.UDS_Extended;
			commParams.ExtendedSessionName.Value = "";
			commParams.ExtendedSessionId.Value = Constants.SessionId_UDSExtended;
		}

		private bool InitDiagCommParamsEcu(DiagnosticsECU ecu, IDiagSymbolsManager dsManager)
		{
			bool result = false;
			this.SetDiagCommParamsEcuDefaultValues(ref ecu.DiagnosticCommParamsECU);
			IList<string> list = null;
			if (dsManager.GetValidCommInterfaceQualifiers(ecu.Qualifier.Value, out list) && list.Count > 0)
			{
				result = dsManager.GetCommInterfaceParamValuesFromDb(ecu.Qualifier.Value, list[0], ref ecu.DiagnosticCommParamsECU);
			}
			if (!dsManager.GetSessionControlParamValuesFromDb(ecu.Qualifier.Value, ecu.Variant.Value, ref ecu.DiagnosticCommParamsECU))
			{
				if (ecu.DiagnosticCommParamsECU.DefaultSessionSource.Value != DiagnosticsSessionSource.DatabaseDefined)
				{
					this.SetStandardDefaultSessionForDiagProtocol(ref ecu.DiagnosticCommParamsECU);
				}
				if (ecu.DiagnosticCommParamsECU.ExtendedSessionSource.Value != DiagnosticsSessionSource.DatabaseDefined)
				{
					this.SetStandardExtendedSessionForDiagProtocol(ref ecu.DiagnosticCommParamsECU);
				}
			}
			return result;
		}

		private void SetStandardDefaultSessionForDiagProtocol(ref DiagnosticCommParamsECU commParams)
		{
			if (commParams.DiagProtocol.Value == DiagnosticsProtocolType.KWP)
			{
				commParams.DefaultSessionSource.Value = DiagnosticsSessionSource.KWP_Default;
				commParams.DefaultSessionName.Value = "";
				commParams.DefaultSessionId.Value = Constants.SessionId_KWPDefault;
				return;
			}
			if (commParams.DiagProtocol.Value == DiagnosticsProtocolType.UDS)
			{
				commParams.DefaultSessionSource.Value = DiagnosticsSessionSource.UDS_Default;
				commParams.DefaultSessionName.Value = "";
				commParams.DefaultSessionId.Value = Constants.SessionId_UDSDefault;
				return;
			}
			commParams.DefaultSessionSource.Value = DiagnosticsSessionSource.UserDefined;
			commParams.DefaultSessionName.Value = "";
			commParams.DefaultSessionId.Value = Constants.SessionId_InitialDefaultUserDef;
		}

		private void SetStandardExtendedSessionForDiagProtocol(ref DiagnosticCommParamsECU commParams)
		{
			if (commParams.DiagProtocol.Value == DiagnosticsProtocolType.UDS)
			{
				commParams.ExtendedSessionSource.Value = DiagnosticsSessionSource.UDS_Extended;
				commParams.ExtendedSessionName.Value = "";
				commParams.ExtendedSessionId.Value = Constants.SessionId_UDSExtended;
				return;
			}
			commParams.ExtendedSessionSource.Value = DiagnosticsSessionSource.UserDefined;
			commParams.ExtendedSessionName.Value = "";
			commParams.ExtendedSessionId.Value = Constants.SessionId_InitialExtendedUserDef;
		}

		private bool AutoCorrectDiagCommParams(ReadOnlyCollection<DiagnosticsDatabase> diagDbs, IDiagSymbolsManager dsManager, string configFolderPath, bool isReplace, out string correctionReport)
		{
			correctionReport = "";
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			int num = 0;
			int num2 = 0;
			string str = "";
			string text = "";
			string text2 = "";
			foreach (DiagnosticsDatabase current in diagDbs)
			{
				bool flag2 = false;
				string value = string.Format(Resources.RtfDiagDescHasChanged, Path.GetFileName(current.FilePath.Value));
				if (isReplace)
				{
					value = string.Format(Resources.RtfChangesReplaceDescFile, Path.GetFileName(current.FilePath.Value));
				}
				if (!File.Exists(FileSystemServices.GetAbsolutePath(current.FilePath.Value, configFolderPath)))
				{
					value = string.Format(Resources.RtfDiagDescNotFound, Path.GetFileName(current.FilePath.Value));
				}
				foreach (DiagnosticsECU current2 in current.ECUs)
				{
					int length = stringBuilder.Length;
					if (this.AutoCorrectDiagCommParamsEcu(current2, dsManager, out str))
					{
						if (!flag2)
						{
							stringBuilder.Append(value);
							flag2 = true;
						}
						stringBuilder.Append(str + this._RtfLineBreak);
						num++;
					}
					if (this.AutoCorrectDiagSessionParamsEcu(current2, dsManager, out text, out text2))
					{
						if (!flag2)
						{
							stringBuilder.Append(value);
							flag2 = true;
						}
						if (!string.IsNullOrEmpty(text))
						{
							stringBuilder.Append(text + this._RtfLineBreak);
							num++;
						}
						if (!string.IsNullOrEmpty(text2))
						{
							stringBuilder.Append(text2 + this._RtfLineBreak);
							num++;
						}
					}
					if (stringBuilder.Length > length)
					{
						stringBuilder.Append(this._RtfLineBreak);
						num2++;
					}
				}
				if (flag2)
				{
					flag = true;
				}
			}
			if (flag)
			{
				correctionReport = string.Format(Resources.RtfTotalDeviationsInConfigDiagDescDetected, num, num2) + stringBuilder;
			}
			return flag;
		}

		private bool AutoCorrectDiagCommParamsEcu(DiagnosticsECU ecu, IDiagSymbolsManager dsManager, out string correctionDone)
		{
			correctionDone = "";
			bool result = false;
			if (ecu.DiagnosticCommParamsECU.UseParamValuesFromDb.Value)
			{
				IList<string> list2;
				if (!string.IsNullOrEmpty(ecu.DiagnosticCommParamsECU.InterfaceQualifier.Value))
				{
					if (dsManager.ResolveCommInterfaceQualifier(ecu.Qualifier.Value, ecu.DiagnosticCommParamsECU.InterfaceQualifier.Value))
					{
						DiagnosticCommParamsECU diagnosticCommParamsECU = new DiagnosticCommParamsECU(ecu.DiagnosticCommParamsECU);
						if (dsManager.GetCommInterfaceParamValuesFromDb(ecu.Qualifier.Value, ecu.DiagnosticCommParamsECU.InterfaceQualifier.Value, ref diagnosticCommParamsECU) && (ecu.DiagnosticCommParamsECU.P2Timeout.Value != diagnosticCommParamsECU.P2Timeout.Value || ecu.DiagnosticCommParamsECU.P2Extension.Value != diagnosticCommParamsECU.P2Extension.Value || ecu.DiagnosticCommParamsECU.PhysRequestMsgId.Value != diagnosticCommParamsECU.PhysRequestMsgId.Value || ecu.DiagnosticCommParamsECU.PhysRequestMsgIsExtendedId.Value != diagnosticCommParamsECU.PhysRequestMsgIsExtendedId.Value || ecu.DiagnosticCommParamsECU.ResponseMsgId.Value != diagnosticCommParamsECU.ResponseMsgId.Value || ecu.DiagnosticCommParamsECU.ResponseMsgIsExtendedId.Value != diagnosticCommParamsECU.ResponseMsgIsExtendedId.Value || ecu.DiagnosticCommParamsECU.DiagAddressingMode.Value != diagnosticCommParamsECU.DiagAddressingMode.Value || (ecu.DiagnosticCommParamsECU.DiagAddressingMode.Value == DiagnosticsAddressingMode.Extended && (ecu.DiagnosticCommParamsECU.ExtAddressingModeRqExtension.Value != diagnosticCommParamsECU.ExtAddressingModeRqExtension.Value || ecu.DiagnosticCommParamsECU.ExtAddressingModeRsExtension.Value != diagnosticCommParamsECU.ExtAddressingModeRsExtension.Value)) || ecu.DiagnosticCommParamsECU.UsePaddingBytes.Value != diagnosticCommParamsECU.UsePaddingBytes.Value || ecu.DiagnosticCommParamsECU.PaddingByteValue.Value != diagnosticCommParamsECU.PaddingByteValue.Value || ecu.DiagnosticCommParamsECU.FirstConsecFrameValue.Value != diagnosticCommParamsECU.FirstConsecFrameValue.Value))
						{
							ecu.DiagnosticCommParamsECU = diagnosticCommParamsECU;
							correctionDone = string.Format(Resources.DiagCommInterfaceParamsChanged, ecu.Qualifier.Value);
							result = true;
						}
					}
					else
					{
						string value = ecu.DiagnosticCommParamsECU.InterfaceQualifier.Value;
						IList<string> list;
						dsManager.GetValidCommInterfaceQualifiers(ecu.Qualifier.Value, out list);
						if (list.Count > 0)
						{
							if (dsManager.GetCommInterfaceParamValuesFromDb(ecu.Qualifier.Value, list[0], ref ecu.DiagnosticCommParamsECU))
							{
								correctionDone = string.Format(Resources.DiagCommInterfaceNotFoundReplaced, value, ecu.Qualifier.Value, list[0]);
								result = true;
							}
						}
						else
						{
							correctionDone = string.Format(Resources.DiagCommInterfaceAndAltNotFound, ecu.DiagnosticCommParamsECU.InterfaceQualifier.Value, ecu.Qualifier.Value);
							ecu.DiagnosticCommParamsECU.InterfaceQualifier.Value = "";
							result = true;
						}
					}
				}
				else if (dsManager.GetValidCommInterfaceQualifiers(ecu.Qualifier.Value, out list2) && list2.Count > 0)
				{
					string commInterfaceQualifier = list2[0];
					string arg = "";
					dsManager.GetCommInterfaceName(ecu.Qualifier.Value, commInterfaceQualifier, out arg);
					correctionDone = string.Format(Resources.DiagCommInterfaceAutoFound, ecu.Qualifier.Value, arg);
					dsManager.GetCommInterfaceParamValuesFromDb(ecu.Qualifier.Value, commInterfaceQualifier, ref ecu.DiagnosticCommParamsECU);
					result = true;
				}
			}
			return result;
		}

		private bool AutoCorrectDiagSessionParamsEcu(DiagnosticsECU ecu, IDiagSymbolsManager dsManager, out string correctionDoneDefault, out string correctionDoneExtended)
		{
			correctionDoneDefault = "";
			correctionDoneExtended = "";
			bool result = false;
			DiagnosticCommParamsECU diagnosticCommParamsECU = null;
			if (ecu.DiagnosticCommParamsECU.UseParamValuesFromDb.Value)
			{
				Dictionary<string, ulong> dictionary;
				dsManager.GetAvailableSessions(ecu.Qualifier.Value, ecu.Variant.Value, out dictionary);
				if (ecu.DiagnosticCommParamsECU.DefaultSessionSource.Value == DiagnosticsSessionSource.DatabaseDefined)
				{
					if (!dictionary.ContainsKey(ecu.DiagnosticCommParamsECU.DefaultSessionName.Value))
					{
						correctionDoneDefault = string.Format(Resources.DiagDefaultSessionNotFound, ecu.DiagnosticCommParamsECU.DefaultSessionName.Value, ecu.Qualifier.Value);
						if (ecu.DiagnosticCommParamsECU.DiagProtocol.Value == DiagnosticsProtocolType.KWP)
						{
							ecu.DiagnosticCommParamsECU.DefaultSessionSource.Value = DiagnosticsSessionSource.KWP_Default;
							ecu.DiagnosticCommParamsECU.DefaultSessionName.Value = "";
							ecu.DiagnosticCommParamsECU.DefaultSessionId.Value = Constants.SessionId_KWPDefault;
						}
						else if (ecu.DiagnosticCommParamsECU.DiagProtocol.Value == DiagnosticsProtocolType.UDS)
						{
							ecu.DiagnosticCommParamsECU.DefaultSessionSource.Value = DiagnosticsSessionSource.UDS_Default;
							ecu.DiagnosticCommParamsECU.DefaultSessionName.Value = "";
							ecu.DiagnosticCommParamsECU.DefaultSessionId.Value = Constants.SessionId_UDSDefault;
						}
						else
						{
							ecu.DiagnosticCommParamsECU.DefaultSessionSource.Value = DiagnosticsSessionSource.UserDefined;
							ecu.DiagnosticCommParamsECU.DefaultSessionName.Value = "";
						}
						result = true;
					}
					else if (dictionary[ecu.DiagnosticCommParamsECU.DefaultSessionName.Value] != ecu.DiagnosticCommParamsECU.DefaultSessionId.Value)
					{
						ecu.DiagnosticCommParamsECU.DefaultSessionId.Value = dictionary[ecu.DiagnosticCommParamsECU.DefaultSessionName.Value];
						correctionDoneDefault = string.Format(Resources.DiagSessionDefaultByteDumpChanged, ecu.DiagnosticCommParamsECU.DefaultSessionName.Value, ecu.Qualifier.Value);
						result = true;
					}
				}
				else if (dictionary.Count > 0)
				{
					diagnosticCommParamsECU = new DiagnosticCommParamsECU();
					diagnosticCommParamsECU.DiagProtocol.Value = ecu.DiagnosticCommParamsECU.DiagProtocol.Value;
					if (dsManager.GetSessionControlParamValuesFromDb(ecu.Qualifier.Value, ecu.Variant.Value, ref diagnosticCommParamsECU) && diagnosticCommParamsECU.DefaultSessionSource.Value == DiagnosticsSessionSource.DatabaseDefined)
					{
						ecu.DiagnosticCommParamsECU.DefaultSessionSource.Value = DiagnosticsSessionSource.DatabaseDefined;
						ecu.DiagnosticCommParamsECU.DefaultSessionName.Value = diagnosticCommParamsECU.DefaultSessionName.Value;
						ecu.DiagnosticCommParamsECU.DefaultSessionId.Value = diagnosticCommParamsECU.DefaultSessionId.Value;
						correctionDoneDefault = string.Format(Resources.DiagSessionDefaultAutoFound, ecu.Qualifier.Value, ecu.DiagnosticCommParamsECU.DefaultSessionName.Value);
						result = true;
					}
				}
				if (ecu.DiagnosticCommParamsECU.ExtendedSessionSource.Value == DiagnosticsSessionSource.DatabaseDefined)
				{
					if (!dictionary.ContainsKey(ecu.DiagnosticCommParamsECU.ExtendedSessionName.Value))
					{
						correctionDoneExtended = string.Format(Resources.DiagExtendedSessionNotFound, ecu.DiagnosticCommParamsECU.ExtendedSessionName.Value, ecu.Qualifier.Value);
						if (ecu.DiagnosticCommParamsECU.DiagProtocol.Value == DiagnosticsProtocolType.UDS)
						{
							ecu.DiagnosticCommParamsECU.ExtendedSessionSource.Value = DiagnosticsSessionSource.UDS_Extended;
							ecu.DiagnosticCommParamsECU.ExtendedSessionName.Value = "";
							ecu.DiagnosticCommParamsECU.ExtendedSessionId.Value = Constants.SessionId_UDSExtended;
						}
						else
						{
							ecu.DiagnosticCommParamsECU.ExtendedSessionSource.Value = DiagnosticsSessionSource.UserDefined;
							ecu.DiagnosticCommParamsECU.ExtendedSessionName.Value = "";
						}
						result = true;
					}
					else if (dictionary[ecu.DiagnosticCommParamsECU.ExtendedSessionName.Value] != ecu.DiagnosticCommParamsECU.ExtendedSessionId.Value)
					{
						ecu.DiagnosticCommParamsECU.ExtendedSessionId.Value = dictionary[ecu.DiagnosticCommParamsECU.ExtendedSessionName.Value];
						correctionDoneExtended = string.Format(Resources.DiagSessionExtendedByteDumpChanged, ecu.DiagnosticCommParamsECU.ExtendedSessionName.Value, ecu.Qualifier.Value);
						result = true;
					}
				}
				else if (dictionary.Count > 0)
				{
					if (diagnosticCommParamsECU == null)
					{
						diagnosticCommParamsECU = new DiagnosticCommParamsECU();
						diagnosticCommParamsECU.DiagProtocol.Value = ecu.DiagnosticCommParamsECU.DiagProtocol.Value;
						dsManager.GetSessionControlParamValuesFromDb(ecu.Qualifier.Value, ecu.Variant.Value, ref diagnosticCommParamsECU);
					}
					if (diagnosticCommParamsECU.ExtendedSessionSource.Value == DiagnosticsSessionSource.DatabaseDefined)
					{
						ecu.DiagnosticCommParamsECU.ExtendedSessionSource.Value = DiagnosticsSessionSource.DatabaseDefined;
						ecu.DiagnosticCommParamsECU.ExtendedSessionName.Value = diagnosticCommParamsECU.ExtendedSessionName.Value;
						ecu.DiagnosticCommParamsECU.ExtendedSessionId.Value = diagnosticCommParamsECU.ExtendedSessionId.Value;
						correctionDoneExtended = string.Format(Resources.DiagSessionExtendedAutoFound, ecu.Qualifier.Value, ecu.DiagnosticCommParamsECU.ExtendedSessionName.Value);
						result = true;
					}
				}
			}
			return result;
		}

		private void ProclaimCcpXcpEcuRenaming(string ecuNameToReplace, string newEcuName)
		{
			bool flag = false;
			foreach (IFeatureCcpXcpSignalDefinitions current in this.FeatureRegistration.FeaturesUsingCcpXcpSignalDefinitions)
			{
				foreach (ICcpXcpSignal current2 in current.CcpXcpSignals)
				{
					if (current2.EcuName.Value == ecuNameToReplace)
					{
						current2.EcuName.Value = newEcuName;
						flag = true;
					}
				}
			}
			if (flag)
			{
				this.configManager.NotifyAllDependentFeatures(this.DatabaseConfiguration);
			}
		}

		private void UpdateReferencedTriggerNameInDataTransferTriggers(ulong uniqueIdOfLoggingTrigger, string newName, DataTransferTriggerConfiguration dataTransferConfig)
		{
			foreach (DataTransferTrigger current in dataTransferConfig.DataTransferTriggers)
			{
				if (current.Event is ReferencedRecordTriggerNameEvent && (current.Event as ReferencedRecordTriggerNameEvent).UniqueIdOfTrigger != 0uL && (current.Event as ReferencedRecordTriggerNameEvent).UniqueIdOfTrigger == uniqueIdOfLoggingTrigger)
				{
					(current.Event as ReferencedRecordTriggerNameEvent).NameOfTrigger.Value = newName;
				}
			}
		}
	}
}
