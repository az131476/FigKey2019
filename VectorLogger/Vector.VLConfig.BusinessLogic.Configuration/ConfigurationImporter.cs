using Nini.Config;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public class ConfigurationImporter
	{
		private const int LoggerTypeValueGL1000 = 28069;

		private const int LoggerTypeValueMultilog = 28033;

		private const int LoggerTypeValueCANLog4 = 28022;

		private const int ChannelBusType_Inactive = 0;

		private const int ChannelBusType_CAN = 1;

		private const int ChannelBusType_LIN = 2;

		private const int AnaInputsMappingMode_Grouped = 1;

		private const int AnaInputsMappingMode_Individual = 2;

		private const int AnaInputsMappingMode_Continuous = 3;

		private const int LEDState_AlwaysOn = 1;

		private const int LEDState_AlwaysBlinking = 2;

		private const int LEDState_TriggerActive = 3;

		private const int LEDState_EndOfMeasurement = 10;

		private const int LEDState_MemoryFull = 4;

		private const int LEDState_CANLINError = 7;

		private const int LEDState_LoggingActive = 11;

		private const uint LINChannelValueMask = 16u;

		private const uint AnalogInput_Enabled = 65536u;

		private const uint AnalogInput_Disabled = 0u;

		private const int FilterType_Default = 1;

		private const int FilterType_Channel = 28;

		private const int FilterType_Channel_V1 = 26;

		private const int FilterType_CanId = 2;

		private const int FilterType_LinId = 11;

		private const int FilterType_CanSymbolicMsg = 6;

		private const int FilterType_LinSymbolicMsg = 14;

		private const int FilterAction_Pass = 1;

		private const int FilterAction_Stop = 2;

		private const int Condition_Equal = 1;

		private const int Condition_NotEqual = 2;

		private const int Condition_Less = 3;

		private const int Condition_LessOrEqual = 4;

		private const int Condition_Greater = 5;

		private const int Condition_GreaterOrEqual = 6;

		private const int Condition_InRange = 7;

		private const int Condition_NotInRange = 8;

		private const int TriggerType_CANId = 2;

		private const int TriggerType_CANData = 4;

		private const int TriggerType_LINId = 11;

		private const int TriggerType_LINData = 13;

		private const int TriggerType_SymbolicCANMsg = 8;

		private const int TriggerType_SymbolicLINMsg = 15;

		private const int TriggerType_SymbolicCANSig = 6;

		private const int TriggerType_SymbolicLINSig = 14;

		private const int TriggerType_DigitalInput = 26;

		private const int TriggerEffect_Trigger = 1;

		private const int TriggerEffect_Single = 5;

		private const int TriggerEffect_EndMeasurement = 4;

		private const int TriggerEffect_LoggingOn = 2;

		private const int TriggerEffect_LoggingOff = 3;

		private const int TriggerDigitalInput_EdgeLowToHigh = 1;

		private const int TriggerDigitalInput_EdgeHighToLow = 2;

		private const int TriggerAction_Mask_Beep = 512;

		private const int MaxBytePosition = 7;

		private static readonly string SectionGENERAL = "GENERAL";

		private static readonly string SectionVLLOG = "VLLOG";

		private static readonly string SectionLOG = "LOG";

		private static readonly string SectionFILTERS = "FILTERS";

		private static readonly string SectionDBS = "DBS";

		private static readonly string SectionCHANNEL = "CHANNEL";

		private static readonly string SectionSPECIAL = "SPECIAL";

		private static readonly string SectionINCLUDEFILES = "INCLUDEFILES";

		private static readonly string SectionLOGCFG = "LOGCFG";

		private static readonly string Field_LoggerType = "LoggerType";

		private static readonly string Field_ccpExchangeIdEnabled = "ccpExchangeIdEnabled";

		private static readonly string Field_prefSsTit = "prefSsTit";

		private static readonly string Field_prefSsDesc = "prefSsDesc";

		private static readonly string Field_rbSize = "rbSize";

		private static readonly string Field_rbMode = "rbMode";

		private static readonly string Field_sleepTout = "sleepTout";

		private static readonly string Field_postTrg = "postTrg";

		private static readonly string Field_ftyp_Hex = "ftyp{0:x2}";

		private static readonly string Field_fact_Hex = "fact{0:x2}";

		private static readonly string Field_fvco_Hex = "fvco{0:x2}";

		private static readonly string Field_flov_Hex = "flov{0:x2}";

		private static readonly string Field_fhiv_Hex = "fhiv{0:x2}";

		private static readonly string Field_fpa0_Hex = "fpa0{0:x2}";

		private static readonly string Field_fda0_Hex = "fda0{0:x2}";

		private static readonly string Field_fda1_Hex = "fda1{0:x2}";

		private static readonly string Field_db_Hex = "db{0:x2}";

		private static readonly string Field_dbch_Hex = "dbch{0:x2}";

		private static readonly string Field_dbskbf_Hex = "dbskbf{0:x2}";

		private static readonly string Field_ch_Dec_Bus = "ch{0:D}Bus";

		private static readonly string Field_ch_Dec_BusSilent = "ch{0:D}BusSilent";

		private static readonly string Field_ch_Dec_KeepAwake = "ch{0:D}KeepAwake";

		private static readonly string Field_ch_Dec_Avaluea = "ch{0:D}Avaluea";

		private static readonly string Field_LogErrorframes = "LogErrorframes";

		private static readonly string Field_LINch_Dec_Bus = "LINch{0:D}Bus";

		private static readonly string Field_LINch_Dec_KeepAwake = "LINch{0:D}KeepAwake";

		private static readonly string Field_LINch_Dec_LinBdrate = "LINch{0:D}LinBdrate";

		private static readonly string Field_AnaInMappingMode = "AnaInMappingMode";

		private static readonly string Field_AnaInMsgChannel = "AnaInMsgChannel";

		private static readonly string Field_AnaIn_Dec_Enabled = "AnaIn{0:D}Enabled";

		private static readonly string Field_AnaIn_Dec_Frequency = "AnaIn{0:D}Frequency";

		private static readonly string Field_AnaIn_Dec_MsgID = "AnaIn{0:D}MsgID";

		private static readonly string Field_LED_Dec_Enabled = "LED{0:D}Enabled";

		private static readonly string Field_LED_Dec_Config = "LED{0:D}Config";

		private static readonly string Field_logDateTime = "logDateTime";

		private static readonly string Field_logDateTime_msgID = "logDateTime_msgID";

		private static readonly string Field_logDateTime_channel = "logDateTime_channel";

		private static readonly string Field_logKeepAwakeOnIgn = "logKeepAwakeOnIgn";

		private static readonly string Field_includeLTLCodeInCODFile = "includeLTLCodeInCODFile";

		private static readonly string Field_Inc_Dec_Filename = "Inc{0:D}Filename";

		private static readonly string Field_Inc_Dec_Param_Dec = "Inc{0:D}Param{1:D}";

		private static readonly string Field_isLogModeLongTerm = "isLogModeLongTerm";

		private static readonly string Field_isLogModeOnOff = "isLogModeOnOff";

		private static readonly string Field_typ_Hex = "typ{0:x2}";

		private static readonly string Field_act_Hex = "act{0:x2}";

		private static readonly string Field_vco_Hex = "vco{0:x2}";

		private static readonly string Field_lov_Hex = "lov{0:x2}";

		private static readonly string Field_hiv_Hex = "hiv{0:x2}";

		private static readonly string Field_pa_Hex_Hex = "pa{0:x1}{1:x2}";

		private static readonly string Field_da0_Hex = "da0{0:x2}";

		private static readonly string Field_da1_Hex = "da1{0:x2}";

		private ConfigurationManager configManager;

		private Dictionary<string, string> oldCANDbNameToNewDbPath;

		private Dictionary<string, string> oldLINDbNameToNewDbPath;

		public ConfigurationImporter(ConfigurationManager configMan)
		{
			this.configManager = configMan;
			this.oldCANDbNameToNewDbPath = new Dictionary<string, string>();
			this.oldLINDbNameToNewDbPath = new Dictionary<string, string>();
		}

		public static LoggerType GetLoggerTypeOfFile(string filepath)
		{
			if (string.IsNullOrEmpty(filepath) || !File.Exists(filepath))
			{
				return LoggerType.Unknown;
			}
			IConfigSource configSource = null;
			try
			{
				configSource = new IniConfigSource(filepath);
			}
			catch (Exception)
			{
				return LoggerType.Unknown;
			}
			LoggerType result = LoggerType.Unknown;
			IConfig config = configSource.Configs[ConfigurationImporter.SectionGENERAL];
			if (config != null && config.Contains(ConfigurationImporter.Field_LoggerType))
			{
				string @string = config.GetString(ConfigurationImporter.Field_LoggerType);
				long num;
				if (ConfigurationImporter.ParseNumericalValueString(@string, out num))
				{
					result = ConfigurationImporter.MapToLoggerType((int)num);
				}
			}
			return result;
		}

		public bool ImportFile(string filepath, ILoggerSpecifics loggerSpecifics)
		{
			this.configManager.InitializeDefaultConfiguration(loggerSpecifics);
			IConfigSource iniConfigSource = null;
			try
			{
				iniConfigSource = new IniConfigSource(filepath);
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			this.oldCANDbNameToNewDbPath.Clear();
			this.oldLINDbNameToNewDbPath.Clear();
			StringBuilder stringBuilder = new StringBuilder();
			if (!this.ReadAndApplyGeneralSection(iniConfigSource, this.configManager.VLProject))
			{
				InformMessageBox.Error(Resources.ImportGenericError, new string[]
				{
					string.Empty,
					ConfigurationImporter.SectionGENERAL
				});
				return false;
			}
			if (!this.ReadAndApplyVlLogSection(iniConfigSource, this.configManager.VLProject))
			{
				InformMessageBox.Error(Resources.ImportGenericError, new string[]
				{
					Resources.ImportErrorHardwareSettings,
					ConfigurationImporter.SectionVLLOG
				});
				return false;
			}
			if (!this.ReadAndApplyLogSection(iniConfigSource, this.configManager.VLProject))
			{
				InformMessageBox.Error(Resources.ImportGenericError, new string[]
				{
					Resources.ImportErrorTrigger,
					ConfigurationImporter.SectionLOG
				});
				return false;
			}
			if (!this.ReadAndApplyDbsSection(iniConfigSource, this.configManager.VLProject, filepath, this.configManager.Service.ApplicationDatabaseManager))
			{
				InformMessageBox.Error(Resources.ImportGenericError, new string[]
				{
					Resources.ImportErrorDatabase,
					ConfigurationImporter.SectionDBS
				});
				return false;
			}
			string message;
			if (!this.ReadAndApplyChannelSection(iniConfigSource, this.configManager.VLProject, ref stringBuilder, out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			if (!this.ReadAndApplySpecialSection(iniConfigSource, this.configManager.VLProject, ref stringBuilder))
			{
				InformMessageBox.Error(Resources.ImportGenericError, new string[]
				{
					Resources.ImportErrorSpecialFeatures,
					ConfigurationImporter.SectionSPECIAL
				});
				return false;
			}
			if (!this.ReadAndApplyIncludeFilesSection(iniConfigSource, this.configManager.VLProject, filepath))
			{
				InformMessageBox.Error(Resources.ImportGenericError, new string[]
				{
					Resources.ImportErrorInclude,
					ConfigurationImporter.SectionINCLUDEFILES
				});
				return false;
			}
			if (!this.ReadAndApplyFiltersSection(iniConfigSource, this.configManager.VLProject, filepath, this.configManager.Service.ApplicationDatabaseManager, ref stringBuilder))
			{
				InformMessageBox.Error(Resources.ImportGenericError, new string[]
				{
					Resources.ImportErrorFilters,
					ConfigurationImporter.SectionFILTERS
				});
				return false;
			}
			if (!this.ReadAndApplyTriggersSection(iniConfigSource, this.configManager.VLProject, filepath, this.configManager.Service.ApplicationDatabaseManager, ref stringBuilder))
			{
				InformMessageBox.Error(Resources.ImportGenericError, new string[]
				{
					Resources.ImportErrorTrigger,
					ConfigurationImporter.SectionLOGCFG
				});
				return false;
			}
			if (!this.configManager.VLProject.SetFilePathFromImportedVLCFile(filepath))
			{
				return false;
			}
			if (stringBuilder.Length > 0)
			{
				DisplayReport.ShowDisplayReportDialog(Resources.TitleImportReport, Resources.ImportReportHeadline, stringBuilder.ToString(), false);
			}
			this.configManager.VLProject.IsDirty = false;
			return true;
		}

		private bool ReadAndApplyGeneralSection(IConfigSource iniConfigSource, VLProject vlProject)
		{
			IConfig config = iniConfigSource.Configs[ConfigurationImporter.SectionGENERAL];
			if (config == null)
			{
				return false;
			}
			if (config.Contains(ConfigurationImporter.Field_ccpExchangeIdEnabled))
			{
				bool value;
				if (!ConfigurationImporter.ParseBooleanValueString(config.GetString(ConfigurationImporter.Field_ccpExchangeIdEnabled), out value))
				{
					return false;
				}
				vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.EnableExchangeIdHandling.Value = value;
			}
			if (config.Contains(ConfigurationImporter.Field_prefSsTit))
			{
				string @string = config.GetString(ConfigurationImporter.Field_prefSsTit);
				if (@string.IndexOfAny("{}".ToCharArray()) >= 0)
				{
					return false;
				}
				vlProject.ProjectRoot.LoggingConfiguration.CommentConfiguration.Name.Value = @string;
			}
			if (config.Contains(ConfigurationImporter.Field_prefSsDesc))
			{
				string string2 = config.GetString(ConfigurationImporter.Field_prefSsDesc);
				if (string2.IndexOfAny("{}".ToCharArray()) >= 0)
				{
					return false;
				}
				vlProject.ProjectRoot.LoggingConfiguration.CommentConfiguration.Comment.Value = string2;
			}
			return true;
		}

		private bool ReadAndApplyVlLogSection(IConfigSource iniConfigSource, VLProject vlProject)
		{
			IConfig config = iniConfigSource.Configs[ConfigurationImporter.SectionVLLOG];
			if (config == null)
			{
				return false;
			}
			if (config.Contains(ConfigurationImporter.Field_rbSize))
			{
				long num;
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(ConfigurationImporter.Field_rbSize), out num))
				{
					return false;
				}
				vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[0].MemoryRingBuffer.Size.Value = (uint)num * 1024u;
			}
			if (config.Contains(ConfigurationImporter.Field_rbMode))
			{
				bool flag;
				if (!ConfigurationImporter.ParseBooleanValueString(config.GetString(ConfigurationImporter.Field_rbMode), out flag))
				{
					return false;
				}
				if (flag)
				{
					using (IEnumerator<TriggerConfiguration> enumerator = vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							TriggerConfiguration current = enumerator.Current;
							current.MemoryRingBuffer.OperatingMode.Value = RingBufferOperatingMode.overwriteOldest;
						}
						goto IL_141;
					}
				}
				foreach (TriggerConfiguration current2 in vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations)
				{
					current2.MemoryRingBuffer.OperatingMode.Value = RingBufferOperatingMode.stopLogging;
				}
			}
			IL_141:
			if (config.Contains(ConfigurationImporter.Field_sleepTout))
			{
				long num2;
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(ConfigurationImporter.Field_sleepTout), out num2))
				{
					return false;
				}
				vlProject.ProjectRoot.HardwareConfiguration.LogDataStorage.TimeoutToSleep.Value = (uint)num2;
			}
			return true;
		}

		private bool ReadAndApplyLogSection(IConfigSource iniConfigSource, VLProject vlProject)
		{
			IConfig config = iniConfigSource.Configs[ConfigurationImporter.SectionLOG];
			if (config == null)
			{
				return false;
			}
			if (config.Contains(ConfigurationImporter.Field_postTrg))
			{
				long num;
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(ConfigurationImporter.Field_postTrg), out num))
				{
					return false;
				}
				vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[0].PostTriggerTime.Value = (uint)num;
			}
			return true;
		}

		private bool ReadAndApplyDbsSection(IConfigSource iniConfigSource, VLProject vlProject, string configFilePath, IApplicationDatabaseManager dbManager)
		{
			IConfig config = iniConfigSource.Configs[ConfigurationImporter.SectionDBS];
			if (config == null)
			{
				return false;
			}
			string folderPathFromFilePath = FileSystemServices.GetFolderPathFromFilePath(configFilePath);
			int num = 0;
			string key = string.Format(ConfigurationImporter.Field_db_Hex, num);
			while (config.Contains(key))
			{
				string text = config.GetString(key);
				string text2 = Path.GetExtension(text) ?? string.Empty;
				BusType busType = text2.ToLower().Contains("dbc") ? BusType.Bt_CAN : BusType.Bt_LIN;
				if (!File.Exists(text))
				{
					string message = string.Format(Resources.DatabaseMissingReplace, text);
					if (InformMessageBox.Question(message) == DialogResult.No)
					{
						return false;
					}
					string fileName = Path.GetFileName(text);
					GenericOpenFileDialog.FileName = fileName;
					if (BusType.Bt_LIN == busType)
					{
						if (GenericOpenFileDialog.ShowDialog(FileType.LDFDatabase) != DialogResult.OK)
						{
							return false;
						}
					}
					else if (GenericOpenFileDialog.ShowDialog(FileType.DBCDatabase) != DialogResult.OK)
					{
						return false;
					}
					text = GenericOpenFileDialog.FileName;
					string key2 = Path.GetFileNameWithoutExtension(fileName) ?? string.Empty;
					if (BusType.Bt_LIN == busType)
					{
						if (!this.oldLINDbNameToNewDbPath.ContainsKey(key2))
						{
							this.oldLINDbNameToNewDbPath.Add(key2, text);
						}
					}
					else if (!this.oldCANDbNameToNewDbPath.ContainsKey(key2))
					{
						this.oldCANDbNameToNewDbPath.Add(key2, text);
					}
				}
				string key3 = string.Format(ConfigurationImporter.Field_dbch_Hex, num);
				if (!config.Contains(key3))
				{
					return false;
				}
				long num2;
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key3), out num2))
				{
					return false;
				}
				BusType busType2;
				uint value;
				if (!this.MapChannelValueToActualBusTypeAndChannelNr((uint)num2, out busType2, out value))
				{
					return false;
				}
				if (busType != busType2)
				{
					return false;
				}
				Database database = new Database();
				database.FilePath.Value = text;
				if (FileSystemServices.TryMakeFilePathRelativeToConfiguration(folderPathFromFilePath, ref text))
				{
					database.FilePath.Value = text;
				}
				database.BusType.Value = busType;
				database.ChannelNumber.Value = value;
				database.NetworkName.Value = string.Empty;
				database.CPType.Value = CPType.None;
				if (BusType.Bt_CAN == busType)
				{
					if (!File.Exists(text))
					{
						database.CPType.Value = CPType.None;
						string key4 = string.Format(ConfigurationImporter.Field_dbskbf_Hex, num);
						if (config.Contains(key4))
						{
							string @string = config.GetString(key4);
							database.AddCpProtection(new CPProtection("ECU_from_database", !string.IsNullOrEmpty(@string)));
							if (FileSystemServices.TryMakeFilePathRelativeToConfiguration(folderPathFromFilePath, ref @string))
							{
								database.CpProtections[0].SeedAndKeyFilePath.Value = @string;
							}
						}
					}
					else
					{
						IDictionary<string, bool> dictionary;
						uint num3;
						database.CPType.Value = dbManager.GetDatabaseCPConfiguration(database, folderPathFromFilePath, out dictionary, out num3);
						if (database.CPType.Value != CPType.None && dictionary.Count > 0)
						{
							database.AddCpProtection(new CPProtection("ECU_from_database", dictionary.First<KeyValuePair<string, bool>>().Value));
							string key5 = string.Format(ConfigurationImporter.Field_dbskbf_Hex, num);
							if (config.Contains(key5) && database.CpProtections[0].HasSeedAndKey.Value)
							{
								string string2 = config.GetString(key5);
								database.CpProtections[0].SeedAndKeyFilePath.Value = string2;
								if (FileSystemServices.TryMakeFilePathRelativeToConfiguration(folderPathFromFilePath, ref string2))
								{
									database.CpProtections[0].SeedAndKeyFilePath.Value = string2;
								}
							}
						}
					}
				}
				vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.AddDatabase(database);
				num++;
				key = string.Format(ConfigurationImporter.Field_db_Hex, num);
			}
			dbManager.UpdateApplicationDatabaseConfiguration(vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration, vlProject.ProjectRoot.HardwareConfiguration.MultibusChannelConfiguration, vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration, folderPathFromFilePath);
			return true;
		}

		private bool ReadAndApplyChannelSection(IConfigSource iniConfigSource, VLProject vlProject, ref StringBuilder configChangedReport, out string errorText)
		{
			errorText = "";
			IConfig config = iniConfigSource.Configs[ConfigurationImporter.SectionCHANNEL];
			if (config == null)
			{
				errorText = string.Format(Resources.ImportGenericError, string.Empty, ConfigurationImporter.SectionCHANNEL);
				return false;
			}
			if (!this.ReadAndApplyCANChannelsFromChannelSection(config, vlProject, ref configChangedReport))
			{
				errorText = string.Format(Resources.ImportGenericError, Resources.ImportErrorCANChannels, ConfigurationImporter.SectionCHANNEL);
				return false;
			}
			if (!this.ReadAndApplyLINChannelsFromChannelSection(config, vlProject, ref configChangedReport))
			{
				errorText = string.Format(Resources.ImportGenericError, Resources.ImportErrorLINChannels, ConfigurationImporter.SectionCHANNEL);
				return false;
			}
			if (!this.ReadAndApplyAnalogInputsFromChannelSection(config, vlProject, ref configChangedReport))
			{
				errorText = string.Format(Resources.ImportGenericError, Resources.ImportErrorAnaInputs, ConfigurationImporter.SectionCHANNEL);
				return false;
			}
			if (!this.ReadAndApplyLEDConfigurationFromChannelSection(config, vlProject, ref configChangedReport))
			{
				errorText = string.Format(Resources.ImportGenericError, Resources.ImportErrorLEDs, ConfigurationImporter.SectionCHANNEL);
				return false;
			}
			return true;
		}

		private bool ReadAndApplyCANChannelsFromChannelSection(IConfig config, VLProject vlProject, ref StringBuilder configChangedReport)
		{
			IList<uint> standardCANBaudrates = GUIUtil.GetStandardCANBaudrates();
			int num = 0;
			while ((long)num < (long)((ulong)this.configManager.Service.LoggerSpecifics.CAN.NumberOfChannels))
			{
				string key = string.Format(ConfigurationImporter.Field_ch_Dec_Bus, num);
				if (!config.Contains(key))
				{
					return false;
				}
				long num2;
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num2))
				{
					return false;
				}
				bool value;
				if (num2 == 1L)
				{
					value = true;
				}
				else
				{
					if (num2 != 0L)
					{
						return false;
					}
					value = false;
				}
				string key2 = string.Format(ConfigurationImporter.Field_ch_Dec_Avaluea, num);
				if (!config.Contains(key2))
				{
					return false;
				}
				long num3;
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key2), out num3))
				{
					return false;
				}
				if (!standardCANBaudrates.Contains((uint)num3))
				{
					return false;
				}
				string key3 = string.Format(ConfigurationImporter.Field_ch_Dec_BusSilent, num);
				if (!config.Contains(key3))
				{
					return false;
				}
				bool flag;
				if (!ConfigurationImporter.ParseBooleanValueString(config.GetString(key3), out flag))
				{
					return false;
				}
				string key4 = string.Format(ConfigurationImporter.Field_ch_Dec_KeepAwake, num);
				if (!config.Contains(key4))
				{
					return false;
				}
				bool value2;
				if (!ConfigurationImporter.ParseBooleanValueString(config.GetString(key4), out value2))
				{
					return false;
				}
				uint num4 = (uint)(num + 1);
				CANChannel cANChannel = vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration.GetCANChannel(num4);
				cANChannel.IsActive.Value = value;
				CANStdChipConfiguration cANChipConfiguration = new CANStdChipConfiguration();
				CANChipConfigurationManager.ApplyPredefinedSettingForBaudrate(num4, (uint)num3, ref cANChipConfiguration);
				cANChannel.CANChipConfiguration = cANChipConfiguration;
				cANChannel.IsOutputActive.Value = !flag;
				cANChannel.IsKeepAwakeActive.Value = value2;
				num++;
			}
			if (!config.Contains(ConfigurationImporter.Field_LogErrorframes))
			{
				return false;
			}
			bool value3;
			if (!ConfigurationImporter.ParseBooleanValueString(config.GetString(ConfigurationImporter.Field_LogErrorframes), out value3))
			{
				return false;
			}
			foreach (CANChannel current in vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration.CANChannels)
			{
				current.LogErrorFrames.Value = value3;
			}
			foreach (ValidatedProperty<bool> current2 in vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration.LogErrorFramesOnMemories)
			{
				current2.Value = value3;
			}
			return true;
		}

		private bool ReadAndApplyLINChannelsFromChannelSection(IConfig config, VLProject vlProject, ref StringBuilder configChangedReport)
		{
			IList<uint> standardLINBaudrates = GUIUtil.GetStandardLINBaudrates();
			int num = 0;
			while ((long)num < (long)((ulong)this.configManager.Service.LoggerSpecifics.LIN.NumberOfChannels))
			{
				string key = string.Format(ConfigurationImporter.Field_LINch_Dec_Bus, num);
				if (!config.Contains(key))
				{
					return false;
				}
				long num2;
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num2))
				{
					return false;
				}
				bool value;
				if (num2 == 2L)
				{
					value = true;
				}
				else
				{
					if (num2 != 0L)
					{
						return false;
					}
					value = false;
				}
				string key2 = string.Format(ConfigurationImporter.Field_LINch_Dec_LinBdrate, num);
				if (!config.Contains(key2))
				{
					return false;
				}
				long num3;
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key2), out num3))
				{
					return false;
				}
				if (!standardLINBaudrates.Contains((uint)num3))
				{
					return false;
				}
				string key3 = string.Format(ConfigurationImporter.Field_LINch_Dec_KeepAwake, num);
				if (!config.Contains(key3))
				{
					return false;
				}
				bool value2;
				if (!ConfigurationImporter.ParseBooleanValueString(config.GetString(key3), out value2))
				{
					return false;
				}
				LINChannel lINChannel = vlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration.GetLINChannel((uint)(num + 1));
				lINChannel.IsActive.Value = value;
				lINChannel.SpeedRate.Value = (uint)num3;
				lINChannel.IsKeepAwakeActive.Value = value2;
				lINChannel.ProtocolVersion.Value = Constants.DefaultLINProtocolVersion;
				lINChannel.UseDbConfigValues.Value = false;
				num++;
			}
			return true;
		}

		private bool ReadAndApplyAnalogInputsFromChannelSection(IConfig config, VLProject vlProject, ref StringBuilder configChangedReport)
		{
			bool flag = false;
			AnalogInputsCANMappingMode value;
			if (!config.Contains(ConfigurationImporter.Field_AnaInMappingMode))
			{
				value = AnalogInputsCANMappingMode.ContinuousIndividualIDs;
				flag = true;
				configChangedReport.AppendLine(string.Format(Resources.ImportChangedAnaInputMapModeDefault, GUIUtil.CANIdToDisplayString(Constants.DefaultAnalogInputMappedCANId, true)));
			}
			else
			{
				long modeCode;
				if (!ConfigurationImporter.ParseNumericalValueString(config.Get(ConfigurationImporter.Field_AnaInMappingMode), out modeCode))
				{
					return false;
				}
				if (!ConfigurationImporter.MapToAnalogInputsMappingMode(modeCode, out value))
				{
					return false;
				}
			}
			vlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration.AnalogInputsCANMappingMode.Value = value;
			long num;
			if (!config.Contains(ConfigurationImporter.Field_AnaInMsgChannel))
			{
				if (!flag)
				{
					return false;
				}
				num = 3L;
				configChangedReport.AppendLine(string.Format(Resources.ImportChangedAnaInputChannelDefault, num));
			}
			else
			{
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(ConfigurationImporter.Field_AnaInMsgChannel), out num))
				{
					return false;
				}
				if (num > (long)((ulong)(this.configManager.Service.LoggerSpecifics.CAN.NumberOfChannels + this.configManager.Service.LoggerSpecifics.CAN.NumberOfVirtualChannels)))
				{
					return false;
				}
			}
			vlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration.CanChannel.Value = (uint)num;
			IList<uint> standardAnalogInputFrequencies = GUIUtil.GetStandardAnalogInputFrequencies();
			int num2 = 0;
			while ((long)num2 < (long)((ulong)this.configManager.Service.LoggerSpecifics.IO.NumberOfAnalogInputs))
			{
				string key = string.Format(ConfigurationImporter.Field_AnaIn_Dec_Enabled, num2);
				if (!config.Contains(key))
				{
					return false;
				}
				long enabledValue;
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out enabledValue))
				{
					return false;
				}
				bool value2;
				if (!ConfigurationImporter.MapAnalogInputEnabledState(enabledValue, out value2))
				{
					return false;
				}
				string key2 = string.Format(ConfigurationImporter.Field_AnaIn_Dec_Frequency, num2);
				if (!config.Contains(key2))
				{
					return false;
				}
				long num3;
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key2), out num3))
				{
					return false;
				}
				uint num4 = 1000u / (uint)num3;
				if (!standardAnalogInputFrequencies.Contains(num4))
				{
					return false;
				}
				uint value3;
				bool value4;
				if (flag)
				{
					value3 = Constants.DefaultAnalogInputMappedCANId + (uint)num2;
					value4 = true;
				}
				else
				{
					string key3 = string.Format(ConfigurationImporter.Field_AnaIn_Dec_MsgID, num2);
					if (!config.Contains(key3))
					{
						return false;
					}
					long num5;
					if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key3), out num5))
					{
						return false;
					}
					MessageDefinition messageDefinition = new MessageDefinition((uint)num5);
					value3 = messageDefinition.ActualMessageId;
					value4 = messageDefinition.IsExtendedId;
				}
				AnalogInput analogInput = vlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration.GetAnalogInput((uint)(num2 + 1));
				analogInput.IsActive.Value = value2;
				analogInput.Frequency.Value = num4;
				analogInput.MappedCANId.Value = value3;
				analogInput.IsMappedCANIdExtended.Value = value4;
				num2++;
			}
			return true;
		}

		private bool ReadAndApplyLEDConfigurationFromChannelSection(IConfig config, VLProject vlProject, ref StringBuilder configChangedReport)
		{
			int num = 0;
			while ((long)num < (long)((ulong)this.configManager.Service.LoggerSpecifics.IO.NumberOfLEDsTotal))
			{
				string key = string.Format(ConfigurationImporter.Field_LED_Dec_Enabled, num);
				if (!config.Contains(key))
				{
					return false;
				}
				bool flag;
				if (!ConfigurationImporter.ParseBooleanValueString(config.GetString(key), out flag))
				{
					return false;
				}
				if (!flag)
				{
					vlProject.ProjectRoot.OutputConfiguration.LEDConfiguration.LEDConfigList[num].State.Value = LEDState.Disabled;
				}
				else
				{
					string key2 = string.Format(ConfigurationImporter.Field_LED_Dec_Config, num);
					if (!config.Contains(key2))
					{
						return false;
					}
					long codeLedState;
					if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key2), out codeLedState))
					{
						return false;
					}
					LEDState value;
					if (!ConfigurationImporter.MapToLEDState(codeLedState, out value))
					{
						return false;
					}
					vlProject.ProjectRoot.OutputConfiguration.LEDConfiguration.LEDConfigList[num].State.Value = value;
				}
				num++;
			}
			return true;
		}

		private bool ReadAndApplySpecialSection(IConfigSource iniConfigSource, VLProject vlProject, ref StringBuilder configChangedReport)
		{
			IConfig config = iniConfigSource.Configs[ConfigurationImporter.SectionSPECIAL];
			if (config == null)
			{
				return false;
			}
			if (!config.Contains(ConfigurationImporter.Field_logDateTime))
			{
				return false;
			}
			bool value;
			if (!ConfigurationImporter.ParseBooleanValueString(config.GetString(ConfigurationImporter.Field_logDateTime), out value))
			{
				return false;
			}
			uint num;
			bool flag;
			if (!config.Contains(ConfigurationImporter.Field_logDateTime_msgID))
			{
				num = this.configManager.Service.LoggerSpecifics.Recording.DefaultLogDateTimeCANId;
				flag = this.configManager.Service.LoggerSpecifics.Recording.DefaultLogDateTimeIsExtendedCANId;
				configChangedReport.AppendLine(string.Format(Resources.ImportChangedSpecialLogDateTimeIdDefault, GUIUtil.CANIdToDisplayString(num, flag)));
			}
			else
			{
				long num2;
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(ConfigurationImporter.Field_logDateTime_msgID), out num2))
				{
					return false;
				}
				MessageDefinition messageDefinition = new MessageDefinition((uint)num2);
				num = messageDefinition.ActualMessageId;
				flag = messageDefinition.IsExtendedId;
			}
			long num3;
			if (!config.Contains(ConfigurationImporter.Field_logDateTime_channel))
			{
				num3 = (long)((ulong)this.configManager.Service.LoggerSpecifics.Recording.DefaultLogDateTimeChannel);
				configChangedReport.AppendLine(string.Format(Resources.ImportChangedSpecialLogDateTimeChnDefault, num3));
			}
			else
			{
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(ConfigurationImporter.Field_logDateTime_channel), out num3))
				{
					return false;
				}
				if (num3 > (long)((ulong)(this.configManager.Service.LoggerSpecifics.CAN.NumberOfChannels + this.configManager.Service.LoggerSpecifics.CAN.NumberOfVirtualChannels)))
				{
					return false;
				}
			}
			if (!config.Contains(ConfigurationImporter.Field_logKeepAwakeOnIgn))
			{
				return false;
			}
			bool value2;
			if (!ConfigurationImporter.ParseBooleanValueString(config.GetString(ConfigurationImporter.Field_logKeepAwakeOnIgn), out value2))
			{
				return false;
			}
			bool value3;
			if (!config.Contains(ConfigurationImporter.Field_includeLTLCodeInCODFile))
			{
				value3 = true;
				configChangedReport.AppendLine(Resources.ImportChangedSpecialIncLTLCodeDefault);
			}
			else if (!ConfigurationImporter.ParseBooleanValueString(config.GetString(ConfigurationImporter.Field_includeLTLCodeInCODFile), out value3))
			{
				return false;
			}
			SpecialFeaturesConfiguration specialFeaturesConfiguration = vlProject.ProjectRoot.LoggingConfiguration.SpecialFeaturesConfiguration;
			specialFeaturesConfiguration.IsLogDateTimeEnabled.Value = value;
			specialFeaturesConfiguration.LogDateTimeCANId.Value = num;
			specialFeaturesConfiguration.LogDateTimeIsExtendedId.Value = flag;
			specialFeaturesConfiguration.LogDateTimeChannel.Value = (uint)num3;
			specialFeaturesConfiguration.IsIgnitionKeepsLoggerAwakeEnabled.Value = value2;
			specialFeaturesConfiguration.IsIncludeLTLCodeEnabled.Value = value3;
			return true;
		}

		private bool ReadAndApplyIncludeFilesSection(IConfigSource iniConfigSource, VLProject vlProject, string configFilePath)
		{
			IConfig config = iniConfigSource.Configs[ConfigurationImporter.SectionINCLUDEFILES];
			if (config == null)
			{
				return true;
			}
			string folderPathFromFilePath = FileSystemServices.GetFolderPathFromFilePath(configFilePath);
			int num = 0;
			string key = string.Format(ConfigurationImporter.Field_Inc_Dec_Filename, num);
			while (config.Contains(key))
			{
				string @string = config.GetString(key);
				IncludeFile includeFile = new IncludeFile();
				includeFile.FilePath.Value = @string;
				if (FileSystemServices.TryMakeFilePathRelativeToConfiguration(folderPathFromFilePath, ref @string))
				{
					includeFile.FilePath.Value = @string;
				}
				int num2 = 0;
				string key2 = string.Format(ConfigurationImporter.Field_Inc_Dec_Param_Dec, num, num2);
				while (config.Contains(key2))
				{
					includeFile.Parameters.Add(new ValidatedProperty<string>(config.GetString(key2)));
					num2++;
					key2 = string.Format(ConfigurationImporter.Field_Inc_Dec_Param_Dec, num, num2);
				}
				vlProject.ProjectRoot.LoggingConfiguration.IncludeFileConfiguration.AddIncludeFile(includeFile);
				num++;
				key = string.Format(ConfigurationImporter.Field_Inc_Dec_Filename, num);
			}
			return true;
		}

		private bool ReadAndApplyFiltersSection(IConfigSource iniConfigSource, VLProject vlProject, string configFilePath, IApplicationDatabaseManager dbManager, ref StringBuilder configChangedReport)
		{
			FileSystemServices.GetFolderPathFromFilePath(configFilePath);
			IConfig config = iniConfigSource.Configs[ConfigurationImporter.SectionFILTERS];
			if (config == null)
			{
				return false;
			}
			int num = 0;
			string key = string.Format(ConfigurationImporter.Field_ftyp_Hex, num);
			while (config.Contains(key))
			{
				long num2;
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num2))
				{
					return false;
				}
				uint num3 = 1u;
				uint value = 0u;
				uint value2 = 0u;
				bool flag = false;
				string value3 = "";
				string text = "";
				Database database = null;
				BusType busType = BusType.Bt_None;
				bool flag2 = false;
				long num4 = num2;
				if (num4 <= 6L)
				{
					FilterActionType value4;
					if (num4 <= 2L)
					{
						if (num4 < 1L)
						{
							return false;
						}
						switch ((int)(num4 - 1L))
						{
						case 0:
						{
							DefaultFilter defaultFilter = new DefaultFilter();
							if (!this.ReadFilterAction(config, num, out value4))
							{
								return false;
							}
							defaultFilter.Action.Value = value4;
							vlProject.ProjectRoot.LoggingConfiguration.FilterConfigurations[0].AddFilter(defaultFilter);
							goto IL_65B;
						}
						case 1:
							if (!this.ReadFilterAction(config, num, out value4))
							{
								return false;
							}
							if (!ConfigurationImporter.ReadFilterLowCANId(config, num, out value, out flag))
							{
								return false;
							}
							if (!ConfigurationImporter.ReadFilterIsRange(config, num, out flag2))
							{
								return false;
							}
							if (flag2)
							{
								bool flag3 = false;
								if (!ConfigurationImporter.ReadFilterHighCANId(config, num, out value2, out flag3))
								{
									return false;
								}
								if (flag != flag3)
								{
									return false;
								}
							}
							if (!this.ReadFilterChannel(config, num, out busType, out num3))
							{
								return false;
							}
							if (BusType.Bt_CAN == busType)
							{
								CANIdFilter cANIdFilter = new CANIdFilter();
								cANIdFilter.Action.Value = value4;
								cANIdFilter.CANId.Value = value;
								cANIdFilter.CANIdLast.Value = value2;
								cANIdFilter.IsIdRange.Value = flag2;
								cANIdFilter.ChannelNumber.Value = num3;
								cANIdFilter.IsExtendedId.Value = flag;
								vlProject.ProjectRoot.LoggingConfiguration.FilterConfigurations[0].AddFilter(cANIdFilter);
								goto IL_65B;
							}
							return false;
						}
					}
					if (num4 != 6L)
					{
						return false;
					}
					if (!this.ReadFilterAction(config, num, out value4))
					{
						return false;
					}
					if (!this.ReadFilterChannel(config, num, out busType, out num3))
					{
						return false;
					}
					if (BusType.Bt_CAN != busType)
					{
						return false;
					}
					if (!ConfigurationImporter.ReadFilterSymbolicMsgId(config, num, out value3))
					{
						return false;
					}
					if (!ConfigurationImporter.ReadFilterSymbolicDatabaseName(config, num, out text))
					{
						return false;
					}
					if (this.oldCANDbNameToNewDbPath.ContainsKey(text))
					{
						if (!vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.TryGetDatabase(this.oldCANDbNameToNewDbPath[text], "", num3, BusType.Bt_CAN, out database))
						{
							return false;
						}
						text = Path.GetFileNameWithoutExtension(this.oldCANDbNameToNewDbPath[text]);
					}
					else if (!vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.TryGetDatabaseByFileName(text, num3, BusType.Bt_CAN, out database))
					{
						return false;
					}
					SymbolicMessageFilter symbolicMessageFilter = new SymbolicMessageFilter();
					symbolicMessageFilter.ChannelNumber.Value = num3;
					symbolicMessageFilter.BusType.Value = BusType.Bt_CAN;
					symbolicMessageFilter.Action.Value = value4;
					symbolicMessageFilter.MessageName.Value = value3;
					symbolicMessageFilter.DatabaseName.Value = text;
					symbolicMessageFilter.DatabasePath.Value = database.FilePath.Value;
					symbolicMessageFilter.NetworkName.Value = "";
					vlProject.ProjectRoot.LoggingConfiguration.FilterConfigurations[0].AddFilter(symbolicMessageFilter);
				}
				else if (num4 != 11L)
				{
					if (num4 != 14L)
					{
						if (num4 > 28L || num4 < 26L)
						{
							return false;
						}
						switch ((int)(num4 - 26L))
						{
						case 0:
						case 2:
						{
							FilterActionType value4;
							if (!this.ReadFilterAction(config, num, out value4))
							{
								return false;
							}
							if (!this.ReadFilterChannel(config, num, out busType, out num3))
							{
								return false;
							}
							if (busType != BusType.Bt_CAN && busType != BusType.Bt_LIN)
							{
								return false;
							}
							ChannelFilter channelFilter = new ChannelFilter();
							channelFilter.BusType.Value = busType;
							channelFilter.ChannelNumber.Value = num3;
							channelFilter.Action.Value = value4;
							vlProject.ProjectRoot.LoggingConfiguration.FilterConfigurations[0].AddFilter(channelFilter);
							break;
						}
						case 1:
							return false;
						default:
							return false;
						}
					}
					else
					{
						FilterActionType value4;
						if (!this.ReadFilterAction(config, num, out value4))
						{
							return false;
						}
						if (!this.ReadFilterChannel(config, num, out busType, out num3))
						{
							return false;
						}
						if (BusType.Bt_LIN != busType)
						{
							return false;
						}
						if (!ConfigurationImporter.ReadFilterSymbolicMsgId(config, num, out value3))
						{
							return false;
						}
						if (!ConfigurationImporter.ReadFilterSymbolicDatabaseName(config, num, out text))
						{
							return false;
						}
						if (this.oldLINDbNameToNewDbPath.ContainsKey(text))
						{
							if (!vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.TryGetDatabase(this.oldLINDbNameToNewDbPath[text], "", num3, BusType.Bt_LIN, out database))
							{
								return false;
							}
							text = Path.GetFileNameWithoutExtension(this.oldLINDbNameToNewDbPath[text]);
						}
						else if (!vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.TryGetDatabaseByFileName(text, num3, BusType.Bt_LIN, out database))
						{
							return false;
						}
						SymbolicMessageFilter symbolicMessageFilter2 = new SymbolicMessageFilter();
						symbolicMessageFilter2.ChannelNumber.Value = num3;
						symbolicMessageFilter2.BusType.Value = BusType.Bt_LIN;
						symbolicMessageFilter2.Action.Value = value4;
						symbolicMessageFilter2.MessageName.Value = value3;
						symbolicMessageFilter2.DatabaseName.Value = text;
						symbolicMessageFilter2.DatabasePath.Value = database.FilePath.Value;
						symbolicMessageFilter2.NetworkName.Value = "";
						vlProject.ProjectRoot.LoggingConfiguration.FilterConfigurations[0].AddFilter(symbolicMessageFilter2);
					}
				}
				else
				{
					FilterActionType value4;
					if (!this.ReadFilterAction(config, num, out value4))
					{
						return false;
					}
					if (!ConfigurationImporter.ReadFilterLowLINId(config, num, out value))
					{
						return false;
					}
					if (!ConfigurationImporter.ReadFilterIsRange(config, num, out flag2))
					{
						return false;
					}
					if (flag2 && !ConfigurationImporter.ReadFilterHighLINId(config, num, out value2))
					{
						return false;
					}
					if (!this.ReadFilterChannel(config, num, out busType, out num3))
					{
						return false;
					}
					if (BusType.Bt_LIN != busType)
					{
						return false;
					}
					LINIdFilter lINIdFilter = new LINIdFilter();
					lINIdFilter.Action.Value = value4;
					lINIdFilter.LINId.Value = value;
					lINIdFilter.LINIdLast.Value = value2;
					lINIdFilter.IsIdRange.Value = flag2;
					lINIdFilter.ChannelNumber.Value = num3;
					vlProject.ProjectRoot.LoggingConfiguration.FilterConfigurations[0].AddFilter(lINIdFilter);
				}
				IL_65B:
				num++;
				key = string.Format(ConfigurationImporter.Field_ftyp_Hex, num);
			}
			return true;
		}

		private bool ReadFilterAction(IConfig config, int filterCounter, out FilterActionType action)
		{
			action = FilterActionType.Pass;
			string key = string.Format(ConfigurationImporter.Field_fact_Hex, filterCounter);
			if (!config.Contains(key))
			{
				return false;
			}
			long num;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num))
			{
				return false;
			}
			switch ((int)num)
			{
			case 1:
				action = FilterActionType.Pass;
				break;
			case 2:
				action = FilterActionType.Stop;
				break;
			default:
				return false;
			}
			return true;
		}

		private bool ReadFilterChannel(IConfig config, int filterCounter, out BusType busTypeFromChannelValue, out uint channelNr)
		{
			channelNr = 1u;
			busTypeFromChannelValue = BusType.Bt_None;
			string key = string.Format(ConfigurationImporter.Field_fpa0_Hex, filterCounter);
			if (!config.Contains(key))
			{
				return false;
			}
			long num;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num))
			{
				return false;
			}
			if (!this.MapChannelValueToActualBusTypeAndChannelNr((uint)num, out busTypeFromChannelValue, out channelNr))
			{
				return false;
			}
			if (busTypeFromChannelValue == BusType.Bt_LIN)
			{
				if (channelNr > this.configManager.Service.LoggerSpecifics.LIN.NumberOfChannels)
				{
					return false;
				}
			}
			else if (channelNr > this.configManager.Service.LoggerSpecifics.CAN.NumberOfChannels)
			{
				return false;
			}
			return true;
		}

		private static bool ReadFilterIsRange(IConfig config, int filterCounter, out bool isRange)
		{
			isRange = false;
			CondRelation condRelation = CondRelation.Equal;
			string key = string.Format(ConfigurationImporter.Field_fvco_Hex, filterCounter);
			if (!config.Contains(key))
			{
				return false;
			}
			long conditionCode;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out conditionCode))
			{
				return false;
			}
			if (!ConfigurationImporter.MapToCondition(conditionCode, out condRelation))
			{
				return false;
			}
			if (condRelation == CondRelation.Equal)
			{
				isRange = false;
			}
			else
			{
				if (condRelation != CondRelation.InRange)
				{
					return false;
				}
				isRange = true;
			}
			return true;
		}

		private static bool ReadFilterLowCANId(IConfig config, int filterCounter, out uint id, out bool isExtended)
		{
			id = 0u;
			isExtended = false;
			string key = string.Format(ConfigurationImporter.Field_flov_Hex, filterCounter);
			if (!config.Contains(key))
			{
				return true;
			}
			long num;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num))
			{
				return false;
			}
			MessageDefinition messageDefinition = new MessageDefinition((uint)num);
			id = messageDefinition.ActualMessageId;
			isExtended = messageDefinition.IsExtendedId;
			return ConfigurationImporter.IsValidCANId(messageDefinition);
		}

		private static bool ReadFilterHighCANId(IConfig config, int filterCounter, out uint id, out bool isExtended)
		{
			id = 0u;
			isExtended = false;
			string key = string.Format(ConfigurationImporter.Field_fhiv_Hex, filterCounter);
			if (!config.Contains(key))
			{
				return true;
			}
			long num;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num))
			{
				return false;
			}
			MessageDefinition messageDefinition = new MessageDefinition((uint)num);
			id = messageDefinition.ActualMessageId;
			isExtended = messageDefinition.IsExtendedId;
			return ConfigurationImporter.IsValidCANId(messageDefinition);
		}

		private static bool ReadFilterLowLINId(IConfig config, int filterCounter, out uint id)
		{
			id = 0u;
			string key = string.Format(ConfigurationImporter.Field_flov_Hex, filterCounter);
			if (!config.Contains(key))
			{
				return true;
			}
			long num;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num))
			{
				return false;
			}
			id = (uint)num;
			return id <= Constants.MaximumLINId;
		}

		private static bool ReadFilterHighLINId(IConfig config, int filterCounter, out uint id)
		{
			id = 0u;
			string key = string.Format(ConfigurationImporter.Field_fhiv_Hex, filterCounter);
			if (!config.Contains(key))
			{
				return true;
			}
			long num;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num))
			{
				return false;
			}
			id = (uint)num;
			return id <= Constants.MaximumLINId;
		}

		private static bool ReadFilterSymbolicMsgId(IConfig config, int filterCounter, out string messageName)
		{
			messageName = "";
			string key = string.Format(ConfigurationImporter.Field_fda0_Hex, filterCounter);
			return config.Contains(key) && ConfigurationImporter.ConvertCompressedBinaryHexDumpToText(config.GetString(key), out messageName);
		}

		private static bool ReadFilterSymbolicDatabaseName(IConfig config, int filterCounter, out string databaseName)
		{
			databaseName = "";
			string key = string.Format(ConfigurationImporter.Field_fda1_Hex, filterCounter);
			return config.Contains(key) && ConfigurationImporter.ConvertCompressedBinaryHexDumpToText(config.GetString(key), out databaseName);
		}

		private bool ReadAndApplyTriggersSection(IConfigSource iniConfigSource, VLProject vlProject, string configFilePath, IApplicationDatabaseManager dbManager, ref StringBuilder configChangedReport)
		{
			string folderPathFromFilePath = FileSystemServices.GetFolderPathFromFilePath(configFilePath);
			IConfig config = iniConfigSource.Configs[ConfigurationImporter.SectionLOGCFG];
			if (config == null)
			{
				return false;
			}
			TriggerMode value;
			if (!this.ReadTriggerMode(config, out value))
			{
				value = TriggerMode.Triggered;
				configChangedReport.AppendLine(Resources.ImportChangedTriggerModeDefault);
			}
			vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[0].TriggerMode.Value = value;
			int num = 0;
			string key = string.Format(ConfigurationImporter.Field_typ_Hex, num);
			int num2 = 0;
			int num3 = 0;
			while (config.Contains(key))
			{
				long num4;
				if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num4))
				{
					return false;
				}
				TriggerEffect value2 = TriggerEffect.Unknown;
				TriggerAction value3 = TriggerAction.Unknown;
				uint num5 = 1u;
				long num6 = 0L;
				long num7 = 0L;
				string text = "";
				string value4 = string.Format("Trigger_{0}", num + 1);
				Database database = null;
				BusType busType = BusType.Bt_None;
				CondRelation condRelation = CondRelation.Equal;
				uint value5 = 0u;
				long num8 = num4;
				if (num8 > 15L)
				{
					goto IL_137;
				}
				if (num8 >= 2L)
				{
					switch ((int)(num8 - 2L))
					{
					case 0:
					{
						if (!this.ReadTriggerEffectAndAction(config, num, out value2, out value3))
						{
							return false;
						}
						if (!this.ReadLowValue(config, num, out num6))
						{
							return false;
						}
						MessageDefinition messageDefinition = new MessageDefinition((uint)num6);
						if (!ConfigurationImporter.IsValidCANId(messageDefinition))
						{
							return false;
						}
						uint value6 = 0u;
						if (!this.ReadTriggerCondition(config, num, out condRelation))
						{
							return false;
						}
						if (condRelation == CondRelation.InRange || condRelation == CondRelation.NotInRange)
						{
							if (!this.ReadHighValue(config, num, out num7))
							{
								return false;
							}
							MessageDefinition messageDefinition2 = new MessageDefinition((uint)num7);
							if (!ConfigurationImporter.IsValidCANId(messageDefinition2))
							{
								return false;
							}
							if (messageDefinition.IsExtendedId != messageDefinition2.IsExtendedId)
							{
								return false;
							}
							value6 = messageDefinition2.ActualMessageId;
						}
						if (!this.ReadTriggerChannel(config, num, out busType, out num5))
						{
							return false;
						}
						if (BusType.Bt_CAN != busType)
						{
							return false;
						}
						RecordTrigger recordTrigger = RecordTrigger.CreateCanIdTrigger();
						recordTrigger.Name.Value = value4;
						CANIdEvent cANIdEvent = recordTrigger.Event as CANIdEvent;
						if (cANIdEvent == null)
						{
							return false;
						}
						recordTrigger.Action.Value = value3;
						recordTrigger.TriggerEffect.Value = value2;
						cANIdEvent.ChannelNumber.Value = num5;
						cANIdEvent.IdRelation.Value = condRelation;
						cANIdEvent.LowId.Value = messageDefinition.ActualMessageId;
						cANIdEvent.HighId.Value = value6;
						cANIdEvent.IsExtendedId.Value = messageDefinition.IsExtendedId;
						if (!this.InsertTrigger(vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[0], recordTrigger))
						{
							return false;
						}
						goto IL_1144;
					}
					case 1:
					case 3:
					case 5:
					case 7:
					case 8:
					case 10:
						return false;
					case 2:
					{
						if (!this.ReadTriggerEffectAndAction(config, num, out value2, out value3))
						{
							return false;
						}
						if (!this.ReadTriggerDataBytePos(config, num, out value5))
						{
							return false;
						}
						if (!this.ReadLowValue(config, num, out num6))
						{
							return false;
						}
						if (!ConfigurationImporter.IsByteValue((uint)num6))
						{
							return false;
						}
						if (!this.ReadTriggerCondition(config, num, out condRelation))
						{
							return false;
						}
						if (condRelation == CondRelation.InRange || condRelation == CondRelation.NotInRange)
						{
							if (!this.ReadHighValue(config, num, out num7))
							{
								return false;
							}
							if (!ConfigurationImporter.IsByteValue((uint)num7))
							{
								return false;
							}
						}
						if (!this.ReadTriggerChannel(config, num, out busType, out num5))
						{
							return false;
						}
						if (BusType.Bt_CAN != busType)
						{
							return false;
						}
						uint num9;
						if (!this.ReadTriggerDataMessageId(config, num, out num9))
						{
							return false;
						}
						MessageDefinition messageDefinition3 = new MessageDefinition(num9);
						if (!ConfigurationImporter.IsValidCANId(messageDefinition3))
						{
							return false;
						}
						RecordTrigger recordTrigger2 = RecordTrigger.CreateCanDataTrigger();
						recordTrigger2.Name.Value = value4;
						CANDataEvent cANDataEvent = recordTrigger2.Event as CANDataEvent;
						if (cANDataEvent == null)
						{
							return false;
						}
						cANDataEvent.ChannelNumber.Value = num5;
						recordTrigger2.Action.Value = value3;
						recordTrigger2.TriggerEffect.Value = value2;
						cANDataEvent.ID.Value = messageDefinition3.ActualMessageId;
						cANDataEvent.IsExtendedId.Value = messageDefinition3.IsExtendedId;
						cANDataEvent.RawDataSignal = new RawDataSignalByte
						{
							DataBytePos = 
							{
								Value = value5
							}
						};
						cANDataEvent.LowValue.Value = (uint)num6;
						cANDataEvent.HighValue.Value = (uint)num7;
						cANDataEvent.Relation.Value = condRelation;
						if (!this.InsertTrigger(vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[0], recordTrigger2))
						{
							return false;
						}
						goto IL_1144;
					}
					case 4:
					{
						if (!this.ReadTriggerEffectAndAction(config, num, out value2, out value3))
						{
							return false;
						}
						if (!this.ReadTriggerChannel(config, num, out busType, out num5))
						{
							return false;
						}
						if (BusType.Bt_CAN != busType)
						{
							return false;
						}
						IList<string> list;
						if (!this.ReadTriggerSymbolicNames(config, num, out list))
						{
							return false;
						}
						if (list.Count<string>() < 2)
						{
							return false;
						}
						string text2 = list[0];
						string text3 = list[1];
						if (!this.ReadTriggerDatabaseName(config, num, out text))
						{
							return false;
						}
						if (this.oldCANDbNameToNewDbPath.ContainsKey(text))
						{
							if (!vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.TryGetDatabase(this.oldCANDbNameToNewDbPath[text], "", num5, BusType.Bt_CAN, out database))
							{
								return false;
							}
							text = Path.GetFileNameWithoutExtension(this.oldCANDbNameToNewDbPath[text]);
						}
						else if (!vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.TryGetDatabaseByFileName(text, num5, BusType.Bt_CAN, out database))
						{
							return false;
						}
						SignalDefinition signalDefinition;
						if (!dbManager.ResolveSignalSymbolInDatabase(FileSystemServices.GetAbsolutePath(database.FilePath.Value, folderPathFromFilePath), "", text2, text3, out signalDefinition))
						{
							configChangedReport.AppendLine(string.Format(Resources.SkippedSymCANSigTrigger, text3));
							goto IL_1144;
						}
						if (!this.ReadLowValue(config, num, out num6))
						{
							return false;
						}
						if (!this.ReadTriggerCondition(config, num, out condRelation))
						{
							return false;
						}
						if ((condRelation == CondRelation.InRange || condRelation == CondRelation.NotInRange) && !this.ReadHighValue(config, num, out num7))
						{
							return false;
						}
						RecordTrigger recordTrigger3 = RecordTrigger.CreateSymbolicSignalTrigger();
						recordTrigger3.Name.Value = value4;
						SymbolicSignalEvent symbolicSignalEvent = recordTrigger3.Event as SymbolicSignalEvent;
						if (symbolicSignalEvent == null)
						{
							return false;
						}
						recordTrigger3.TriggerEffect.Value = value2;
						recordTrigger3.Action.Value = value3;
						symbolicSignalEvent.BusType.Value = BusType.Bt_CAN;
						symbolicSignalEvent.ChannelNumber.Value = num5;
						symbolicSignalEvent.MessageName.Value = text2;
						symbolicSignalEvent.SignalName.Value = text3;
						symbolicSignalEvent.DatabasePath.Value = database.FilePath.Value;
						symbolicSignalEvent.NetworkName.Value = "";
						symbolicSignalEvent.DatabaseName.Value = text;
						double value7 = 0.0;
						dbManager.PhysicalSignalValueToRawValue(FileSystemServices.GetAbsolutePath(database.FilePath.Value, folderPathFromFilePath), "", text2, text3, (double)num6, out value7);
						symbolicSignalEvent.LowValue.Value = value7;
						double value8 = 0.0;
						dbManager.PhysicalSignalValueToRawValue(FileSystemServices.GetAbsolutePath(database.FilePath.Value, folderPathFromFilePath), "", text2, text3, (double)num7, out value8);
						symbolicSignalEvent.HighValue.Value = value8;
						symbolicSignalEvent.Relation.Value = condRelation;
						if (!this.InsertTrigger(vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[0], recordTrigger3))
						{
							return false;
						}
						goto IL_1144;
					}
					case 6:
					{
						if (!this.ReadTriggerEffectAndAction(config, num, out value2, out value3))
						{
							return false;
						}
						if (!this.ReadTriggerChannel(config, num, out busType, out num5))
						{
							return false;
						}
						if (BusType.Bt_CAN != busType)
						{
							return false;
						}
						IList<string> source;
						if (!this.ReadTriggerSymbolicNames(config, num, out source))
						{
							return false;
						}
						if (source.Count<string>() == 0)
						{
							return false;
						}
						string text2 = source.First<string>();
						if (!this.ReadTriggerDatabaseName(config, num, out text))
						{
							return false;
						}
						if (this.oldCANDbNameToNewDbPath.ContainsKey(text))
						{
							if (!vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.TryGetDatabase(this.oldCANDbNameToNewDbPath[text], "", num5, BusType.Bt_CAN, out database))
							{
								return false;
							}
							text = Path.GetFileNameWithoutExtension(this.oldCANDbNameToNewDbPath[text]);
						}
						else if (!vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.TryGetDatabaseByFileName(text, num5, BusType.Bt_CAN, out database))
						{
							return false;
						}
						RecordTrigger recordTrigger4 = RecordTrigger.CreateSymbolicMessageTrigger();
						recordTrigger4.Name.Value = value4;
						SymbolicMessageEvent symbolicMessageEvent = recordTrigger4.Event as SymbolicMessageEvent;
						if (symbolicMessageEvent == null)
						{
							return false;
						}
						recordTrigger4.TriggerEffect.Value = value2;
						recordTrigger4.Action.Value = value3;
						symbolicMessageEvent.BusType.Value = BusType.Bt_CAN;
						symbolicMessageEvent.ChannelNumber.Value = num5;
						symbolicMessageEvent.MessageName.Value = text2;
						symbolicMessageEvent.DatabaseName.Value = text;
						symbolicMessageEvent.DatabasePath.Value = database.FilePath.Value;
						symbolicMessageEvent.NetworkName.Value = "";
						if (!this.InsertTrigger(vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[0], recordTrigger4))
						{
							return false;
						}
						goto IL_1144;
					}
					case 9:
					{
						if (!this.ReadTriggerEffectAndAction(config, num, out value2, out value3))
						{
							return false;
						}
						if (!this.ReadLowValue(config, num, out num6))
						{
							return false;
						}
						if (num6 > (long)((ulong)Constants.MaximumLINId))
						{
							return false;
						}
						if (!this.ReadTriggerCondition(config, num, out condRelation))
						{
							return false;
						}
						if (condRelation == CondRelation.InRange || condRelation == CondRelation.NotInRange)
						{
							if (!this.ReadHighValue(config, num, out num7))
							{
								return false;
							}
							if (num7 > (long)((ulong)Constants.MaximumLINId))
							{
								return false;
							}
						}
						if (!this.ReadTriggerChannel(config, num, out busType, out num5))
						{
							return false;
						}
						if (BusType.Bt_LIN != busType)
						{
							return false;
						}
						RecordTrigger recordTrigger5 = RecordTrigger.CreateLinIdTrigger();
						recordTrigger5.Name.Value = value4;
						LINIdEvent lINIdEvent = recordTrigger5.Event as LINIdEvent;
						if (lINIdEvent == null)
						{
							return false;
						}
						recordTrigger5.Action.Value = value3;
						recordTrigger5.TriggerEffect.Value = value2;
						lINIdEvent.ChannelNumber.Value = num5;
						lINIdEvent.IdRelation.Value = condRelation;
						lINIdEvent.LowId.Value = (uint)num6;
						lINIdEvent.HighId.Value = (uint)num7;
						if (!this.InsertTrigger(vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[0], recordTrigger5))
						{
							return false;
						}
						goto IL_1144;
					}
					case 11:
					{
						if (!this.ReadTriggerEffectAndAction(config, num, out value2, out value3))
						{
							return false;
						}
						if (!this.ReadTriggerDataBytePos(config, num, out value5))
						{
							return false;
						}
						if (!this.ReadLowValue(config, num, out num6))
						{
							return false;
						}
						if (!ConfigurationImporter.IsByteValue((uint)num6))
						{
							return false;
						}
						if (!this.ReadTriggerCondition(config, num, out condRelation))
						{
							return false;
						}
						if (condRelation == CondRelation.InRange || condRelation == CondRelation.NotInRange)
						{
							if (!this.ReadHighValue(config, num, out num7))
							{
								return false;
							}
							if (!ConfigurationImporter.IsByteValue((uint)num7))
							{
								return false;
							}
						}
						if (!this.ReadTriggerChannel(config, num, out busType, out num5))
						{
							return false;
						}
						if (BusType.Bt_LIN != busType)
						{
							return false;
						}
						uint num9;
						if (!this.ReadTriggerDataMessageId(config, num, out num9))
						{
							return false;
						}
						if (!ConfigurationImporter.IsValidLINId(num9))
						{
							return false;
						}
						RecordTrigger recordTrigger6 = RecordTrigger.CreateLinDataTrigger();
						recordTrigger6.Name.Value = value4;
						LINDataEvent lINDataEvent = recordTrigger6.Event as LINDataEvent;
						if (lINDataEvent == null)
						{
							return false;
						}
						lINDataEvent.ChannelNumber.Value = num5;
						recordTrigger6.Action.Value = value3;
						recordTrigger6.TriggerEffect.Value = value2;
						lINDataEvent.ID.Value = num9;
						lINDataEvent.RawDataSignal = new RawDataSignalByte
						{
							DataBytePos = 
							{
								Value = value5
							}
						};
						lINDataEvent.LowValue.Value = (uint)num6;
						lINDataEvent.HighValue.Value = (uint)num7;
						lINDataEvent.Relation.Value = condRelation;
						if (!this.InsertTrigger(vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[0], recordTrigger6))
						{
							return false;
						}
						goto IL_1144;
					}
					case 12:
					{
						if (!this.ReadTriggerEffectAndAction(config, num, out value2, out value3))
						{
							return false;
						}
						if (!this.ReadTriggerChannel(config, num, out busType, out num5))
						{
							return false;
						}
						if (BusType.Bt_LIN != busType)
						{
							return false;
						}
						IList<string> list2;
						if (!this.ReadTriggerSymbolicNames(config, num, out list2))
						{
							return false;
						}
						if (list2.Count<string>() < 2)
						{
							return false;
						}
						string text2 = list2[0];
						string text3 = list2[1];
						if (!this.ReadTriggerDatabaseName(config, num, out text))
						{
							return false;
						}
						if (this.oldLINDbNameToNewDbPath.ContainsKey(text))
						{
							if (!vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.TryGetDatabase(this.oldLINDbNameToNewDbPath[text], "", num5, BusType.Bt_LIN, out database))
							{
								return false;
							}
							text = Path.GetFileNameWithoutExtension(this.oldLINDbNameToNewDbPath[text]);
						}
						else if (!vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.TryGetDatabaseByFileName(text, num5, BusType.Bt_LIN, out database))
						{
							return false;
						}
						SignalDefinition signalDefinition2;
						if (!dbManager.ResolveSignalSymbolInDatabase(FileSystemServices.GetAbsolutePath(database.FilePath.Value, folderPathFromFilePath), "", text2, text3, out signalDefinition2))
						{
							configChangedReport.AppendLine(string.Format(Resources.SkippedSymLINSigTrigger, text3));
							goto IL_1144;
						}
						if (!this.ReadLowValue(config, num, out num6))
						{
							return false;
						}
						if (!this.ReadTriggerCondition(config, num, out condRelation))
						{
							return false;
						}
						if ((condRelation == CondRelation.InRange || condRelation == CondRelation.NotInRange) && !this.ReadHighValue(config, num, out num7))
						{
							return false;
						}
						RecordTrigger recordTrigger7 = RecordTrigger.CreateSymbolicSignalTrigger();
						recordTrigger7.Name.Value = value4;
						SymbolicSignalEvent symbolicSignalEvent2 = recordTrigger7.Event as SymbolicSignalEvent;
						if (symbolicSignalEvent2 == null)
						{
							return false;
						}
						recordTrigger7.TriggerEffect.Value = value2;
						recordTrigger7.Action.Value = value3;
						symbolicSignalEvent2.BusType.Value = BusType.Bt_LIN;
						symbolicSignalEvent2.ChannelNumber.Value = num5;
						symbolicSignalEvent2.MessageName.Value = text2;
						symbolicSignalEvent2.SignalName.Value = text3;
						symbolicSignalEvent2.DatabasePath.Value = database.FilePath.Value;
						symbolicSignalEvent2.NetworkName.Value = "";
						symbolicSignalEvent2.DatabaseName.Value = text;
						double value9 = 0.0;
						dbManager.PhysicalSignalValueToRawValue(FileSystemServices.GetAbsolutePath(database.FilePath.Value, folderPathFromFilePath), "", text2, text3, (double)num6, out value9);
						symbolicSignalEvent2.LowValue.Value = value9;
						double value10 = 0.0;
						dbManager.PhysicalSignalValueToRawValue(FileSystemServices.GetAbsolutePath(database.FilePath.Value, folderPathFromFilePath), "", text2, text3, (double)num7, out value10);
						symbolicSignalEvent2.HighValue.Value = value10;
						symbolicSignalEvent2.Relation.Value = condRelation;
						if (!this.InsertTrigger(vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[0], recordTrigger7))
						{
							return false;
						}
						goto IL_1144;
					}
					case 13:
					{
						if (!this.ReadTriggerEffectAndAction(config, num, out value2, out value3))
						{
							return false;
						}
						if (!this.ReadTriggerChannel(config, num, out busType, out num5))
						{
							return false;
						}
						if (BusType.Bt_LIN != busType)
						{
							return false;
						}
						IList<string> source2;
						if (!this.ReadTriggerSymbolicNames(config, num, out source2))
						{
							return false;
						}
						if (source2.Count<string>() == 0)
						{
							return false;
						}
						string text2 = source2.First<string>();
						if (!this.ReadTriggerDatabaseName(config, num, out text))
						{
							return false;
						}
						if (this.oldLINDbNameToNewDbPath.ContainsKey(text))
						{
							if (!vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.TryGetDatabase(this.oldLINDbNameToNewDbPath[text], "", num5, BusType.Bt_LIN, out database))
							{
								return false;
							}
							text = Path.GetFileNameWithoutExtension(this.oldLINDbNameToNewDbPath[text]);
						}
						else if (!vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.TryGetDatabaseByFileName(text, num5, BusType.Bt_LIN, out database))
						{
							return false;
						}
						RecordTrigger recordTrigger8 = RecordTrigger.CreateSymbolicMessageTrigger();
						recordTrigger8.Name.Value = value4;
						SymbolicMessageEvent symbolicMessageEvent2 = recordTrigger8.Event as SymbolicMessageEvent;
						if (symbolicMessageEvent2 == null)
						{
							return false;
						}
						recordTrigger8.TriggerEffect.Value = value2;
						recordTrigger8.Action.Value = value3;
						symbolicMessageEvent2.BusType.Value = BusType.Bt_LIN;
						symbolicMessageEvent2.ChannelNumber.Value = num5;
						symbolicMessageEvent2.MessageName.Value = text2;
						symbolicMessageEvent2.DatabaseName.Value = text;
						symbolicMessageEvent2.DatabasePath.Value = database.FilePath.Value;
						symbolicMessageEvent2.NetworkName.Value = "";
						if (!this.InsertTrigger(vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[0], recordTrigger8))
						{
							return false;
						}
						goto IL_1144;
					}
					}
					goto IL_137;
				}
				return false;
				IL_1144:
				num++;
				key = string.Format(ConfigurationImporter.Field_typ_Hex, num);
				continue;
				IL_137:
				if (num8 == 26L)
				{
					if (!this.ReadTriggerEffectAndAction(config, num, out value2, out value3))
					{
						return false;
					}
					if (!this.ReadTriggerDigitalInputNr(config, num, out num5))
					{
						return false;
					}
					bool value11;
					if (!this.ReadTriggerDigitalInputEdge(config, num, out value11))
					{
						return false;
					}
					RecordTrigger recordTrigger9 = RecordTrigger.CreateDigitalInputTrigger();
					recordTrigger9.Name.Value = value4;
					DigitalInputEvent digitalInputEvent = recordTrigger9.Event as DigitalInputEvent;
					if (digitalInputEvent == null)
					{
						return false;
					}
					recordTrigger9.Action.Value = value3;
					recordTrigger9.TriggerEffect.Value = value2;
					digitalInputEvent.DigitalInput.Value = num5;
					digitalInputEvent.Edge.Value = value11;
					if (!this.InsertTrigger(vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations[0], recordTrigger9))
					{
						return false;
					}
					goto IL_1144;
				}
				return false;
			}
			if (num2 > 0)
			{
				configChangedReport.AppendLine(string.Format(Resources.ImportChangedSkippedCANDataTrigger, num2));
			}
			if (num3 > 0)
			{
				configChangedReport.AppendLine(string.Format(Resources.ImportChangedSkippedLINDataTrigger, num3));
			}
			return true;
		}

		private bool InsertTrigger(TriggerConfiguration triggerConfig, RecordTrigger trigger)
		{
			if (triggerConfig.TriggerMode.Value == TriggerMode.OnOff)
			{
				if ((long)triggerConfig.OnOffTriggers.Count > (long)((ulong)(this.configManager.Service.LoggerSpecifics.DataStorage.NumberOfMemories * 2u)))
				{
					return false;
				}
				if (trigger.TriggerEffect.Value != TriggerEffect.LoggingOn && trigger.TriggerEffect.Value != TriggerEffect.LoggingOff)
				{
					return false;
				}
				triggerConfig.AddOnOffTrigger(trigger);
			}
			else
			{
				triggerConfig.AddTrigger(trigger);
			}
			return true;
		}

		private bool ReadTriggerMode(IConfig config, out TriggerMode mode)
		{
			mode = TriggerMode.Triggered;
			if (!config.Contains(ConfigurationImporter.Field_isLogModeLongTerm))
			{
				return false;
			}
			if (!config.Contains(ConfigurationImporter.Field_isLogModeOnOff))
			{
				return false;
			}
			bool flag;
			if (!ConfigurationImporter.ParseBooleanValueString(config.GetString(ConfigurationImporter.Field_isLogModeLongTerm), out flag))
			{
				return false;
			}
			bool flag2;
			if (!ConfigurationImporter.ParseBooleanValueString(config.GetString(ConfigurationImporter.Field_isLogModeOnOff), out flag2))
			{
				return false;
			}
			if (flag)
			{
				mode = TriggerMode.Permanent;
			}
			else if (flag2)
			{
				mode = TriggerMode.OnOff;
			}
			else
			{
				mode = TriggerMode.Triggered;
			}
			return true;
		}

		private bool ReadTriggerEffectAndAction(IConfig config, int triggerCounter, out TriggerEffect effect, out TriggerAction action)
		{
			effect = TriggerEffect.Unknown;
			action = TriggerAction.None;
			string key = string.Format(ConfigurationImporter.Field_act_Hex, triggerCounter);
			if (!config.Contains(key))
			{
				return false;
			}
			long num;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num))
			{
				return false;
			}
			if ((num & 512L) == 512L)
			{
				action = TriggerAction.Beep;
			}
			long num2 = num & 15L;
			long num3 = num2;
			if (num3 <= 5L && num3 >= 1L)
			{
				switch ((int)(num3 - 1L))
				{
				case 0:
					effect = TriggerEffect.Normal;
					break;
				case 1:
					effect = TriggerEffect.LoggingOn;
					break;
				case 2:
					effect = TriggerEffect.LoggingOff;
					break;
				case 3:
					effect = TriggerEffect.EndMeasurement;
					break;
				case 4:
					effect = TriggerEffect.Single;
					break;
				default:
					return false;
				}
				return true;
			}
			return false;
		}

		private bool ReadTriggerCondition(IConfig config, int triggerCounter, out CondRelation condition)
		{
			condition = CondRelation.Equal;
			string key = string.Format(ConfigurationImporter.Field_vco_Hex, triggerCounter);
			if (!config.Contains(key))
			{
				return false;
			}
			long num;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num))
			{
				return false;
			}
			long num2 = num;
			if (num2 <= 8L && num2 >= 1L)
			{
				switch ((int)(num2 - 1L))
				{
				case 0:
					condition = CondRelation.Equal;
					break;
				case 1:
					condition = CondRelation.NotEqual;
					break;
				case 2:
					condition = CondRelation.LessThan;
					break;
				case 3:
					condition = CondRelation.LessThanOrEqual;
					break;
				case 4:
					condition = CondRelation.GreaterThan;
					break;
				case 5:
					condition = CondRelation.GreaterThanOrEqual;
					break;
				case 6:
					condition = CondRelation.InRange;
					break;
				case 7:
					condition = CondRelation.NotInRange;
					break;
				default:
					return false;
				}
				return true;
			}
			return false;
		}

		private bool ReadTriggerChannel(IConfig config, int triggerCounter, out BusType busTypeFromChannelValue, out uint channelNr)
		{
			channelNr = 1u;
			busTypeFromChannelValue = BusType.Bt_None;
			string key = string.Format(ConfigurationImporter.Field_pa_Hex_Hex, 0, triggerCounter);
			if (!config.Contains(key))
			{
				return false;
			}
			long num;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num))
			{
				return false;
			}
			if (!this.MapChannelValueToActualBusTypeAndChannelNr((uint)num, out busTypeFromChannelValue, out channelNr))
			{
				return false;
			}
			if (busTypeFromChannelValue == BusType.Bt_LIN)
			{
				if (channelNr > this.configManager.Service.LoggerSpecifics.LIN.NumberOfChannels)
				{
					return false;
				}
			}
			else if (channelNr > this.configManager.Service.LoggerSpecifics.CAN.NumberOfChannels)
			{
				return false;
			}
			return true;
		}

		private bool ReadTriggerDataBytePos(IConfig config, int triggerCounter, out uint pos)
		{
			pos = 0u;
			string key = string.Format(ConfigurationImporter.Field_pa_Hex_Hex, 2, triggerCounter);
			if (!config.Contains(key))
			{
				return true;
			}
			long num;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num))
			{
				return false;
			}
			pos = (uint)num;
			return pos <= 7u;
		}

		private bool ReadTriggerDataMessageId(IConfig config, int triggerCounter, out uint messageId)
		{
			messageId = 0u;
			string key = string.Format(ConfigurationImporter.Field_pa_Hex_Hex, 1, triggerCounter);
			if (!config.Contains(key))
			{
				return true;
			}
			long num;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num))
			{
				return false;
			}
			messageId = (uint)num;
			return true;
		}

		private bool ReadLowValue(IConfig config, int triggerCounter, out long lowValue)
		{
			lowValue = 0L;
			string key = string.Format(ConfigurationImporter.Field_lov_Hex, triggerCounter);
			return !config.Contains(key) || ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out lowValue);
		}

		private bool ReadHighValue(IConfig config, int triggerCounter, out long highValue)
		{
			highValue = 0L;
			string key = string.Format(ConfigurationImporter.Field_hiv_Hex, triggerCounter);
			return !config.Contains(key) || ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out highValue);
		}

		private bool ReadTriggerSymbolicNames(IConfig config, int triggerCounter, out IList<string> symbolicNames)
		{
			symbolicNames = new List<string>();
			string key = string.Format(ConfigurationImporter.Field_da0_Hex, triggerCounter);
			return config.Contains(key) && ConfigurationImporter.ConvertCompressedBinaryHexDumpToMultipleTexts(config.GetString(key), out symbolicNames);
		}

		private bool ReadTriggerDatabaseName(IConfig config, int triggerCounter, out string databaseName)
		{
			databaseName = "";
			string key = string.Format(ConfigurationImporter.Field_da1_Hex, triggerCounter);
			return config.Contains(key) && ConfigurationImporter.ConvertCompressedBinaryHexDumpToText(config.GetString(key), out databaseName);
		}

		private bool ReadTriggerDigitalInputNr(IConfig config, int triggerCounter, out uint inputNr)
		{
			inputNr = 1u;
			string key = string.Format(ConfigurationImporter.Field_pa_Hex_Hex, 2, triggerCounter);
			if (!config.Contains(key))
			{
				return false;
			}
			long num;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num))
			{
				return false;
			}
			inputNr = (uint)num;
			return inputNr <= this.configManager.Service.LoggerSpecifics.IO.NumberOfDigitalInputs;
		}

		private bool ReadTriggerDigitalInputEdge(IConfig config, int triggerCounter, out bool isEdgeLowToHigh)
		{
			isEdgeLowToHigh = true;
			string key = string.Format(ConfigurationImporter.Field_pa_Hex_Hex, 3, triggerCounter);
			if (!config.Contains(key))
			{
				return false;
			}
			long num;
			if (!ConfigurationImporter.ParseNumericalValueString(config.GetString(key), out num))
			{
				return false;
			}
			long num2 = num;
			if (num2 <= 2L && num2 >= 1L)
			{
				switch ((int)(num2 - 1L))
				{
				case 0:
					isEdgeLowToHigh = true;
					break;
				case 1:
					isEdgeLowToHigh = false;
					break;
				default:
					return false;
				}
				return true;
			}
			return false;
		}

		private static LoggerType MapToLoggerType(int typecode)
		{
			if (typecode == 28069)
			{
				return LoggerType.GL1000;
			}
			return LoggerType.Unknown;
		}

		private static bool MapToAnalogInputsMappingMode(long modeCode, out AnalogInputsCANMappingMode mappingMode)
		{
			mappingMode = AnalogInputsCANMappingMode.SameFixedIDs;
			if (modeCode <= 3L && modeCode >= 1L)
			{
				switch ((int)(modeCode - 1L))
				{
				case 0:
					mappingMode = AnalogInputsCANMappingMode.SameFixedIDs;
					break;
				case 1:
					mappingMode = AnalogInputsCANMappingMode.IndividualIDs;
					break;
				case 2:
					mappingMode = AnalogInputsCANMappingMode.ContinuousIndividualIDs;
					break;
				default:
					return false;
				}
				return true;
			}
			return false;
		}

		private static bool MapAnalogInputEnabledState(long enabledValue, out bool isEnabled)
		{
			isEnabled = false;
			uint num = (uint)enabledValue;
			if (num != 0u)
			{
				if (num != 65536u)
				{
					return false;
				}
				isEnabled = true;
			}
			else
			{
				isEnabled = false;
			}
			return true;
		}

		private static bool MapToLEDState(long codeLedState, out LEDState ledState)
		{
			switch ((int)codeLedState)
			{
			case 1:
				ledState = LEDState.AlwaysOn;
				return true;
			case 2:
				ledState = LEDState.AlwaysBlinking;
				return true;
			case 3:
				ledState = LEDState.TriggerActive;
				return true;
			case 4:
				ledState = LEDState.MemoryFull;
				return true;
			case 7:
				ledState = LEDState.CANLINError;
				return true;
			case 10:
				ledState = LEDState.EndOfMeasurement;
				return true;
			case 11:
				ledState = LEDState.LoggingActive;
				return true;
			}
			ledState = LEDState.Disabled;
			return false;
		}

		private static bool MapToCondition(long conditionCode, out CondRelation condition)
		{
			condition = CondRelation.Equal;
			switch ((int)conditionCode)
			{
			case 1:
				condition = CondRelation.Equal;
				break;
			case 2:
				condition = CondRelation.NotEqual;
				break;
			case 3:
				condition = CondRelation.LessThan;
				break;
			case 4:
				condition = CondRelation.LessThanOrEqual;
				break;
			case 5:
				condition = CondRelation.GreaterThan;
				break;
			case 6:
				condition = CondRelation.GreaterThanOrEqual;
				break;
			case 7:
				condition = CondRelation.InRange;
				break;
			case 8:
				condition = CondRelation.NotInRange;
				break;
			default:
				return false;
			}
			return true;
		}

		private static bool ParseNumericalValueString(string numValueString, out long value)
		{
			value = 0L;
			numValueString = numValueString.Trim();
			if (numValueString.IndexOf("0x") == 0)
			{
				numValueString = numValueString.Substring(2);
				if (long.TryParse(numValueString, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out value))
				{
					return true;
				}
			}
			else if (long.TryParse(numValueString, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
			{
				return true;
			}
			return false;
		}

		private static bool ParseBooleanValueString(string boolValueString, out bool value)
		{
			value = false;
			long num;
			if (ConfigurationImporter.ParseNumericalValueString(boolValueString, out num))
			{
				if (num == 0L)
				{
					value = false;
					return true;
				}
				if (num == 1L)
				{
					value = true;
					return true;
				}
			}
			return false;
		}

		private bool MapActualCardSizeToNominalCardSize(int actualSize, out uint nominalSize)
		{
			nominalSize = 0u;
			IList<uint> memoryCardSizes = GUIUtil.GetMemoryCardSizes();
			foreach (uint current in memoryCardSizes)
			{
				if (actualSize <= (int)current)
				{
					nominalSize = current;
					return true;
				}
			}
			return false;
		}

		private bool MapChannelValueToActualBusTypeAndChannelNr(uint channelValue, out BusType busType, out uint channelNr)
		{
			if ((channelValue & 16u) != 16u)
			{
				channelNr = channelValue;
				busType = BusType.Bt_CAN;
				if (channelNr > this.configManager.Service.LoggerSpecifics.CAN.NumberOfChannels)
				{
					return false;
				}
			}
			else
			{
				channelNr = (channelValue & 4294967279u);
				busType = BusType.Bt_LIN;
				if (channelNr > this.configManager.Service.LoggerSpecifics.LIN.NumberOfChannels)
				{
					return false;
				}
			}
			return true;
		}

		private static bool ConvertCompressedBinaryHexDumpToText(string compressedHexDump, out string text)
		{
			text = "";
			int num = compressedHexDump.IndexOf('#');
			string hexByteDump;
			if (num > 0)
			{
				hexByteDump = compressedHexDump.Substring(0, num);
			}
			else
			{
				if (num == 0)
				{
					return true;
				}
				hexByteDump = compressedHexDump;
			}
			return ConfigurationImporter.ConvertHexByteDumpToText(hexByteDump, out text);
		}

		private static bool ConvertCompressedBinaryHexDumpToMultipleTexts(string compressedHexDump, out IList<string> texts)
		{
			texts = new List<string>();
			List<string> list = new List<string>();
			string[] array = compressedHexDump.Split(new char[]
			{
				'#'
			});
			for (int i = 0; i < array.Count<string>(); i++)
			{
				if (array[i].Length > 1)
				{
					if (i == 0)
					{
						list.Add(array[i]);
					}
					else
					{
						list.Add(array[i].Substring(1));
					}
				}
			}
			for (int j = 0; j < list.Count<string>(); j++)
			{
				string item = "";
				if (!ConfigurationImporter.ConvertHexByteDumpToText(list[j], out item))
				{
					return false;
				}
				texts.Add(item);
			}
			return true;
		}

		private static bool ConvertHexByteDumpToText(string hexByteDump, out string text)
		{
			text = "";
			if (hexByteDump.Length % 2 > 0)
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < hexByteDump.Length; i += 2)
			{
				string value = hexByteDump.Substring(i, 2);
				try
				{
					stringBuilder.Append(Convert.ToChar(Convert.ToByte(value, 16)));
				}
				catch (Exception)
				{
					return false;
				}
			}
			text = stringBuilder.ToString();
			return true;
		}

		private static bool IsValidCANId(MessageDefinition msgDef)
		{
			if (msgDef.IsExtendedId)
			{
				if (msgDef.ActualMessageId > Constants.MaximumExtendedCANId)
				{
					return false;
				}
			}
			else if (msgDef.ActualMessageId > Constants.MaximumStandardCANId)
			{
				return false;
			}
			return true;
		}

		private static bool IsValidLINId(uint id)
		{
			return id <= Constants.MaximumLINId;
		}

		private static bool IsByteValue(uint value)
		{
			return value <= 255u;
		}
	}
}
