using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LTLGenerationCS.Properties;

namespace Vector.VLConfig.LTLGenerationCS
{
	public class LTLGenerator
	{
		public enum LTLGenerationResult
		{
			OK,
			Error,
			CANchannelError,
			LINchannelError,
			FlexRayChannelError,
			AnalogInputError,
			LEDError,
			LogDateTimeError,
			FilterError,
			FilterError_MessageResolve,
			FilterError_SigListResolve,
			FilterError_SigListReadFailed,
			TriggerError,
			TriggerError_SignalResolve,
			TriggerError_MessageResolve,
			TriggerError_Bitlength,
			TriggerErrorFlexRayMultiplexedSignal,
			TriggerError_UnsupportedEvent,
			TriggerError_UnsupportedCondition,
			TriggerError_TriggerNameResolve,
			UnknownOperatingMode,
			IncludeFileError,
			LTLFileWriteError,
			CCPXCPError,
			WLANConfigurationError,
			DiagError,
			DiagError_UnsupportedEvent,
			DigitalInputError,
			StopCyclicEventsError,
			DiagError_UnknownAddressingMode,
			ActionError,
			ActionsDigitalOutputError,
			ActionsSendMessageError,
			ActionsSendMessageError_MessageResolve,
			EventsVoCANMultipleConfiguredError,
			EventInWrongConext,
			SignalExportListError_SignalResolve,
			GPSerror
		}

		private VLProject vlProject;

		private ILoggerSpecifics loggerSpecifics;

		private IApplicationDatabaseManager databaseManager;

		private IDiagSymbolsManager diagSymbolsManager;

		private CcpXcpGenerationInfo ccpXcpGenerationInfo;

		private string configurationFolderPath;

		private StringBuilder ltlCode;

		private StringBuilder LtlCode
		{
			get
			{
				return this.ltlCode;
			}
		}

		public LTLGenerator(VLProject vlProject, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, IDiagSymbolsManager diagSymbolsManager, CcpXcpGenerationInfo ccpXcpGenerationInfo, string configurationFolderPath)
		{
			this.vlProject = vlProject;
			this.loggerSpecifics = loggerSpecifics;
			this.databaseManager = databaseManager;
			this.diagSymbolsManager = diagSymbolsManager;
			this.ccpXcpGenerationInfo = ccpXcpGenerationInfo;
			this.configurationFolderPath = configurationFolderPath;
			GlobalSettings.SetProjectRootAndLoggerSpecifics(this.vlProject.ProjectRoot, this.loggerSpecifics);
			this.ltlCode = new StringBuilder();
		}

		public LTLGenerator.LTLGenerationResult GenerateLTL(string ltlFile, out string errorText)
		{
			try
			{
				this.ltlCode.Length = 0;
				GlobalSettings.Reset();
				LTLGenerator.LTLGenerationResult lTLGenerationResult;
				if ((lTLGenerationResult = this.LTLAddHeader()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLSystemSettings lTLSystemSettings = new LTLSystemSettings(this.vlProject, this.loggerSpecifics, this.databaseManager);
				if ((lTLGenerationResult = lTLSystemSettings.GenerateLTLSystemSettings()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLCommon lTLCommon = new LTLCommon(this.vlProject.ProjectRoot.HardwareConfiguration.LogDataStorage, this.loggerSpecifics, this.databaseManager, this.configurationFolderPath);
				if ((lTLGenerationResult = lTLCommon.GenerateCommonLTLCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLCcpXcp lTLCcpXcp = new LTLCcpXcp(this.vlProject, this.ccpXcpGenerationInfo);
				if ((lTLGenerationResult = lTLCcpXcp.GenerateCcpXcpCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLDiagnostics lTLDiagnostics = new LTLDiagnostics(this.loggerSpecifics, this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticsDatabaseConfiguration, this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticActionsConfiguration, this.diagSymbolsManager, this.databaseManager, this.configurationFolderPath);
				if ((lTLGenerationResult = lTLDiagnostics.GenerateDiagCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLFilter lTLFilter = new LTLFilter(this.vlProject.ProjectRoot.LoggingConfiguration.FilterConfigurationsOfActiveMemories, this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration, this.loggerSpecifics, this.databaseManager, this.configurationFolderPath);
				if ((lTLGenerationResult = lTLFilter.GenerateLTLFilterCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = lTLFilter.LastErrorText;
					if (string.IsNullOrEmpty(errorText))
					{
						errorText = this.GetErrorString(lTLGenerationResult);
					}
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLTrigger lTLTrigger = new LTLTrigger(this.vlProject, this.loggerSpecifics, this.databaseManager, lTLSystemSettings, this.configurationFolderPath);
				if ((lTLGenerationResult = lTLTrigger.GenerateLTLTriggerCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLMarker lTLMarker = new LTLMarker(this.vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories, this.loggerSpecifics, this.databaseManager, this.configurationFolderPath);
				if ((lTLGenerationResult = lTLMarker.GenerateLTLActionCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLAnalogInputs lTLAnalogInputs = new LTLAnalogInputs(this.vlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration, this.loggerSpecifics);
				if ((lTLGenerationResult = lTLAnalogInputs.GenerateLTLAnalogInputsCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLDigitalInputs lTLDigitalInputs = new LTLDigitalInputs(this.vlProject.ProjectRoot.HardwareConfiguration.DigitalInputConfiguration, this.loggerSpecifics);
				if ((lTLGenerationResult = lTLDigitalInputs.GenerateLTLDigitalInputsCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLSpecialFeatures lTLSpecialFeatures = new LTLSpecialFeatures(this.vlProject.ProjectRoot.LoggingConfiguration.SpecialFeaturesConfiguration, this.loggerSpecifics, this.vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories[0].TriggerMode.Value);
				if ((lTLGenerationResult = lTLSpecialFeatures.GenerateSpecialFeaturesCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLDigitalOutputs lTLDigitalOutputs = new LTLDigitalOutputs(this.vlProject.ProjectRoot.OutputConfiguration.DigitalOutputsConfiguration, this.loggerSpecifics, this.databaseManager, this.configurationFolderPath);
				if ((lTLGenerationResult = lTLDigitalOutputs.GenerateLTLActionCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLSendMessage lTLSendMessage = new LTLSendMessage(this.vlProject.ProjectRoot.OutputConfiguration.SendMessageConfiguration, this.loggerSpecifics, this.databaseManager, this.configurationFolderPath);
				if ((lTLGenerationResult = lTLSendMessage.GenerateLTLActionCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLActionsCcpXcp lTLActionsCcpXcp = new LTLActionsCcpXcp(this.vlProject.ProjectRoot.LoggingConfiguration.CcpXcpSignalConfiguration, this.loggerSpecifics, this.databaseManager, this.configurationFolderPath, this.ccpXcpGenerationInfo);
				if ((lTLGenerationResult = lTLActionsCcpXcp.GenerateLTLActionCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLWLANSettings lTLWLANSettings = new LTLWLANSettings(this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration, this.loggerSpecifics, this.databaseManager, this.configurationFolderPath);
				if ((lTLGenerationResult = lTLWLANSettings.GenerateWLANSettingsCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LtlIncludeFiles ltlIncludeFiles = new LtlIncludeFiles(this.vlProject.ProjectRoot.LoggingConfiguration.IncludeFileConfiguration, this.configurationFolderPath);
				ltlIncludeFiles.GenerateIncludeFileCode();
				LtlLeds ltlLeds = new LtlLeds(this.vlProject, this.loggerSpecifics, this.ccpXcpGenerationInfo);
				ltlLeds.GenerateLtlLedCode();
				LTLInterfaceMode lTLInterfaceMode = new LTLInterfaceMode(this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration, this.loggerSpecifics, this.databaseManager, this.configurationFolderPath);
				if ((lTLGenerationResult = lTLInterfaceMode.GenerateInterfaceModeCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				LTLGPS lTLGPS = new LTLGPS(this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration, this.loggerSpecifics);
				if ((lTLGenerationResult = lTLGPS.GenerateLTLGPSCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					errorText = this.GetErrorString(lTLGenerationResult);
					LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
					return result;
				}
				lTLSystemSettings.AppendExternalSpecificSystemSettings(lTLAnalogInputs.LtlSystemCode);
				lTLSystemSettings.AppendExternalSpecificSystemSettings(lTLTrigger.LtlSystemCode);
				lTLSystemSettings.AppendExternalSpecificSystemSettings(lTLCcpXcp.LtlSystemCode);
				lTLSystemSettings.AppendExternalSpecificSystemSettings(lTLWLANSettings.LtlSystemCode);
				lTLSystemSettings.AppendExternalSpecificSystemSettings(lTLFilter.LtlSystemCode);
				lTLSystemSettings.AppendExternalSpecificSystemSettings(lTLDiagnostics.LtlSystemCode);
				lTLSystemSettings.AppendExternalSpecificSystemSettings(ltlLeds.LtlSystemCode);
				lTLSystemSettings.AppendExternalSpecificSystemSettings(lTLInterfaceMode.LtlSystemCode);
				lTLSystemSettings.AppendExternalSpecificSystemSettings(lTLCommon.LtlSystemCode);
				lTLSystemSettings.AppendExternalSpecificSystemSettings(lTLGPS.LtlSystemCode);
				if (GlobalSettings.IsAuxSwitchBoxUsed)
				{
					StringBuilder stringBuilder = new StringBuilder();
					if (this.loggerSpecifics.CAN.AuxChannel > this.loggerSpecifics.CAN.NumberOfChannels)
					{
						stringBuilder.AppendFormat("CAN{0:D}Timing         = Timing500k", this.loggerSpecifics.CAN.AuxChannel);
						stringBuilder.AppendLine();
						stringBuilder.AppendFormat("CAN{0:D}Output         = On", this.loggerSpecifics.CAN.AuxChannel);
						stringBuilder.AppendLine();
						stringBuilder.AppendFormat("CAN{0:D}KeepAwake      = Off", this.loggerSpecifics.CAN.AuxChannel);
						stringBuilder.AppendLine();
					}
					stringBuilder.AppendLine("AUX_SwitchBox      = On");
					stringBuilder.AppendLine();
					lTLSystemSettings.AppendExternalSpecificSystemSettings(stringBuilder);
				}
				string value = string.Empty;
				if (GlobalSettings.IsMinutesOfDayUsed)
				{
					StringBuilder stringBuilder2 = new StringBuilder();
					stringBuilder2.AppendFormat("VAR  {0} = FREE[16]", GlobalSettings.MinutesOfDayVariableName);
					stringBuilder2.AppendLine();
					stringBuilder2.AppendFormat("CALC {0} = hour * 60 + minute", GlobalSettings.MinutesOfDayVariableName);
					stringBuilder2.AppendLine();
					value = stringBuilder2.ToString();
				}
				this.ltlCode.Append(lTLSystemSettings.GetSystemSettingsLTLCode());
				this.ltlCode.Append(lTLCommon.LtlCode);
				this.ltlCode.Append(value);
				this.ltlCode.Append(lTLCcpXcp.LtlCode);
				this.ltlCode.Append(ltlIncludeFiles.LtlCode);
				this.ltlCode.Append(lTLDiagnostics.LtlCode);
				this.ltlCode.Append(lTLFilter.LtlCode);
				this.ltlCode.Append(lTLTrigger.LtlCode);
				this.ltlCode.Append(lTLMarker.LtlCode);
				this.ltlCode.Append(lTLAnalogInputs.LtlCode);
				this.ltlCode.Append(lTLDigitalInputs.LtlCode);
				this.ltlCode.Append(lTLGPS.LtlCode);
				this.ltlCode.Append(lTLSpecialFeatures.LtlSpecialFeatureDateTimeLoggingCode);
				this.ltlCode.Append(lTLSpecialFeatures.LtlSpecialFeatureIgnitionKeepsLoggerAwake);
				this.ltlCode.Append(lTLDigitalOutputs.LtlCode);
				this.ltlCode.Append(lTLSendMessage.LtlCode);
				this.ltlCode.Append(lTLActionsCcpXcp.LtlCode);
				this.ltlCode.Append(lTLInterfaceMode.LtlCode);
				this.ltlCode.Append(lTLWLANSettings.LtlCode);
				this.ltlCode.Append(ltlLeds.LtlCode);
				this.ltlCode.Append(this.LtlEnd());
			}
			catch (LTLGenerationException ex)
			{
				errorText = this.GetErrorString(ex.Result);
				LTLGenerator.LTLGenerationResult result = ex.Result;
				return result;
			}
			try
			{
				File.WriteAllText(ltlFile, this.LtlCode.ToString(), Encoding.Default);
			}
			catch (Exception)
			{
				errorText = this.GetErrorString(LTLGenerator.LTLGenerationResult.LTLFileWriteError);
				LTLGenerator.LTLGenerationResult result = LTLGenerator.LTLGenerationResult.LTLFileWriteError;
				return result;
			}
			errorText = Resources.EmptyString;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult LTLAddHeader()
		{
			StringBuilder stringBuilder = new StringBuilder();
			new FileInfo("VectorLoggerConfigurator.exe");
			string str = "";
			string str2 = "";
			try
			{
				string fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "VectorLoggerConfigurator.exe";
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fileName);
				str = versionInfo.FileVersion;
				str2 = versionInfo.LegalCopyright;
			}
			catch (FileNotFoundException ex)
			{
				string arg_67_0 = ex.Message;
				str = "(unknown version)";
				str2 = "(c) Vector Informatik GmbH.";
			}
			int num = 82;
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append('-', num - 2);
			string text = stringBuilder2.ToString();
			stringBuilder.AppendLine("{ " + text);
			stringBuilder.AppendLine(" ");
			stringBuilder.AppendLine("   Vector Logger Configurator " + str);
			stringBuilder.AppendLine(" ");
			stringBuilder.AppendLine("   LTL Configuration ");
			stringBuilder.AppendLine("   Logger Device: " + this.loggerSpecifics.Name);
			stringBuilder.AppendLine(" ");
			stringBuilder.AppendLine("   " + str2);
			stringBuilder.AppendLine(" ");
			if (!this.vlProject.ProjectRoot.LoggingConfiguration.CommentConfiguration.CopyToLTL.Value)
			{
				stringBuilder.AppendLine(text + " }");
			}
			else
			{
				stringBuilder.AppendLine("  " + text);
				stringBuilder.AppendLine(" ");
				int num2 = 14;
				stringBuilder.AppendLine(string.Format("{0}{1}", LTLUtil.AdjustStringLength("   GLC file:", num2), this.vlProject.FilePath));
				stringBuilder.AppendLine(string.Format("{0}{1}", LTLUtil.AdjustStringLength("   Name:", num2), this.vlProject.ProjectRoot.LoggingConfiguration.CommentConfiguration.Name.Value));
				stringBuilder.AppendLine(string.Format("{0}{1}", LTLUtil.AdjustStringLength("   Version:", num2), this.vlProject.ProjectRoot.LoggingConfiguration.CommentConfiguration.Version.Value));
				string text2 = "   Comment:";
				if (num2 + this.vlProject.ProjectRoot.LoggingConfiguration.CommentConfiguration.Comment.Value.Length <= num)
				{
					stringBuilder.AppendLine(string.Format("{0}{1}", LTLUtil.AdjustStringLength(text2, num2), this.vlProject.ProjectRoot.LoggingConfiguration.CommentConfiguration.Comment.Value));
				}
				else
				{
					stringBuilder.AppendLine(LTLUtil.AdjustStringLength(text2, num2));
					LTLUtil.AppendTextWithLineLengthLimit(ref stringBuilder, this.vlProject.ProjectRoot.LoggingConfiguration.CommentConfiguration.Comment.Value, num, 3);
				}
				stringBuilder.AppendLine(" ");
				stringBuilder.AppendLine(text + " }");
			}
			stringBuilder.AppendLine(" ");
			this.ltlCode.Append(stringBuilder);
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private string LtlEnd()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("END");
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		private string GetErrorString(LTLGenerator.LTLGenerationResult result)
		{
			switch (result)
			{
			case LTLGenerator.LTLGenerationResult.OK:
				return Resources.EmptyString;
			case LTLGenerator.LTLGenerationResult.Error:
				return Resources.LTLError_GeneralError;
			case LTLGenerator.LTLGenerationResult.CANchannelError:
				return Resources.LTLError_CANchannelError;
			case LTLGenerator.LTLGenerationResult.LINchannelError:
				return Resources.LTLError_LINchannelError;
			case LTLGenerator.LTLGenerationResult.FlexRayChannelError:
				return Resources.LTLError_FlexRayChannelError;
			case LTLGenerator.LTLGenerationResult.AnalogInputError:
				return Resources.LTLError_AnalogInputsError;
			case LTLGenerator.LTLGenerationResult.LEDError:
				return Resources.LTLError_LEDError;
			case LTLGenerator.LTLGenerationResult.LogDateTimeError:
				return Resources.LTLError_LogDateTimeError;
			case LTLGenerator.LTLGenerationResult.FilterError:
				return Resources.LTLError_FilterError;
			case LTLGenerator.LTLGenerationResult.FilterError_MessageResolve:
				return Resources.LTLError_FilterError_MessageResolve;
			case LTLGenerator.LTLGenerationResult.FilterError_SigListResolve:
				return Resources.LTLError_FilterError_SigListResolve;
			case LTLGenerator.LTLGenerationResult.FilterError_SigListReadFailed:
				return Resources.LTLError_FilterError_SigListReadFailed;
			case LTLGenerator.LTLGenerationResult.TriggerError:
				return Resources.LTLError_TriggerError;
			case LTLGenerator.LTLGenerationResult.TriggerError_SignalResolve:
				return Resources.LTLError_TriggerError_SignalResolve;
			case LTLGenerator.LTLGenerationResult.TriggerError_MessageResolve:
				return Resources.LTLError_TriggerError_MessageResolve;
			case LTLGenerator.LTLGenerationResult.TriggerError_Bitlength:
				return Resources.LTLError_TriggerError_Bitlength;
			case LTLGenerator.LTLGenerationResult.TriggerErrorFlexRayMultiplexedSignal:
				return Resources.LTLError_TriggerError_FlexRayMultiplexedSignal;
			case LTLGenerator.LTLGenerationResult.TriggerError_UnsupportedEvent:
				return Resources.LTLError_TriggerError_UnsupportedEvent;
			case LTLGenerator.LTLGenerationResult.TriggerError_UnsupportedCondition:
				return Resources.LTLError_TriggerError_UnsupportedCondition;
			case LTLGenerator.LTLGenerationResult.TriggerError_TriggerNameResolve:
				return Resources.LTLError_TriggerError_TriggerNameResolve;
			case LTLGenerator.LTLGenerationResult.UnknownOperatingMode:
				return Resources.LTLError_UnknownOperatingMode;
			case LTLGenerator.LTLGenerationResult.IncludeFileError:
				return Resources.LTLError_IncludeFileError;
			case LTLGenerator.LTLGenerationResult.LTLFileWriteError:
				return Resources.LTLError_LTLFileWriteError;
			case LTLGenerator.LTLGenerationResult.CCPXCPError:
				return Resources.LTLError_CCPXCPError;
			case LTLGenerator.LTLGenerationResult.WLANConfigurationError:
				return Resources.LTLError_WLANSettingsInvalid;
			case LTLGenerator.LTLGenerationResult.DiagError:
				return Resources.LTLError_DiagError;
			case LTLGenerator.LTLGenerationResult.DiagError_UnsupportedEvent:
				return Resources.LTLError_DiagError_UnsupportedEvent;
			case LTLGenerator.LTLGenerationResult.DigitalInputError:
				return Resources.LTLError_DigitalInputError;
			case LTLGenerator.LTLGenerationResult.StopCyclicEventsError:
				return Resources.LTLError_StopCyclicEventsError;
			case LTLGenerator.LTLGenerationResult.DiagError_UnknownAddressingMode:
				return Resources.LTLError_DiagError_UnknownAddressingMode;
			case LTLGenerator.LTLGenerationResult.ActionError:
				return Resources.LTLError_ActionsError;
			case LTLGenerator.LTLGenerationResult.ActionsDigitalOutputError:
				return Resources.LTLError_ActionsDigitalOutputError;
			case LTLGenerator.LTLGenerationResult.ActionsSendMessageError:
				return Resources.LTLError_ActionsSendMessageError;
			case LTLGenerator.LTLGenerationResult.ActionsSendMessageError_MessageResolve:
				return Resources.LTLError_ActionsSendMessage_MessageResolve;
			case LTLGenerator.LTLGenerationResult.EventsVoCANMultipleConfiguredError:
				return Resources.LTLError_VoCANMultipleConfigured;
			case LTLGenerator.LTLGenerationResult.EventInWrongConext:
				return Resources.LTLError_EventInWrongContext;
			case LTLGenerator.LTLGenerationResult.SignalExportListError_SignalResolve:
				return Resources.LTLError_SignalExportListError_SignalResolve;
			case LTLGenerator.LTLGenerationResult.GPSerror:
				return Resources.LTLError_GPSerror;
			default:
				return Resources.EmptyString;
			}
		}
	}
}
