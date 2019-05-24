using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LTLGenerationCS.LTLEvents;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLDiagnostics : LTLGenericCodePart
	{
		private static readonly string ConstantNameForIsoTpNormal = "ISOTP_NORMAL";

		private static readonly string ConstantNameForIsoTpExtended = "ISOTP_EXTENDED";

		private static readonly string ConstantNameForExtendedSession = "EXTENDED_SESSION";

		private static readonly string Name_ExternalTesterPresentFlag = "DiagExternalTesterPresent";

		private ILoggerSpecifics loggerSpecifics;

		private DiagnosticsDatabaseConfiguration diagDbConfig;

		private DiagnosticActionsConfiguration diagActionsConfig;

		private IDiagSymbolsManager diagSymbolsManager;

		private IApplicationDatabaseManager databaseManager;

		private string configurationFolderPath;

		public LTLDiagnostics(ILoggerSpecifics loggerSpecifics, DiagnosticsDatabaseConfiguration diagDbConfig, DiagnosticActionsConfiguration diagActionsConfig, IDiagSymbolsManager diagSymbolsManager, IApplicationDatabaseManager databaseManager, string configurationFolderPath)
		{
			this.loggerSpecifics = loggerSpecifics;
			this.diagDbConfig = diagDbConfig;
			this.diagActionsConfig = diagActionsConfig;
			this.diagSymbolsManager = diagSymbolsManager;
			this.databaseManager = databaseManager;
			this.configurationFolderPath = configurationFolderPath;
		}

		public LTLGenerator.LTLGenerationResult GenerateDiagCode()
		{
			if (this.diagActionsConfig.TriggeredActionSequences.Count <= 0)
			{
				return LTLGenerator.LTLGenerationResult.OK;
			}
			uint num = 0u;
			int num2 = 0;
			while (num2 < this.diagActionsConfig.recordDiagCommOnMemories.Count && (long)(num2 + 1) <= (long)((ulong)this.loggerSpecifics.DataStorage.NumberOfMemories))
			{
				if (this.diagActionsConfig.recordDiagCommOnMemories[num2].Value)
				{
					num += 1u << num2 + 1;
				}
				num2++;
			}
			base.LtlSystemCode.AppendFormat("LogDiagMessages = 0x{0:X}", num);
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment("DIAGNOSTICS"));
			base.LtlCode.AppendLine();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("CONST {0} = 0", LTLDiagnostics.ConstantNameForIsoTpNormal);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("CONST {0} = 1", LTLDiagnostics.ConstantNameForIsoTpExtended);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("CONST {0} = 1", LTLDiagnostics.ConstantNameForExtendedSession);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			base.LtlCode.Append(LTLUtil.GetSignAlignedText(stringBuilder.ToString(), "="));
			LTLGenerator.LTLGenerationResult result;
			if ((result = this.GenerateExternalTesterDetection()) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			if ((result = this.GenerateEcuDefinitions()) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			if ((result = this.GenerateServicesListsAndEvents()) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateEcuDefinitions()
		{
			base.LtlCode.AppendLine(LTLUtil.GetFormattedSubsectionHeaderComment("ECU definitions"));
			base.LtlCode.AppendLine();
			HashSet<DiagnosticsECU> actuallyUsedEcus = this.GetActuallyUsedEcus();
			foreach (DiagnosticsECU current in actuallyUsedEcus)
			{
				StringBuilder stringBuilder = new StringBuilder();
				switch (current.DiagnosticCommParamsECU.DiagAddressingMode.Value)
				{
				case DiagnosticsAddressingMode.Undefined:
					goto IL_21F;
				case DiagnosticsAddressingMode.Normal:
				case DiagnosticsAddressingMode.NormalFixed:
					stringBuilder.AppendFormat("DIAGECU {0} = (TP = {1} ", current.Qualifier.Value, LTLDiagnostics.ConstantNameForIsoTpNormal);
					stringBuilder.AppendLine();
					stringBuilder.AppendFormat("  ({0}", LTLUtil.GetIdString(BusType.Bt_CAN, current.ChannelNumber.Value, current.DiagnosticCommParamsECU.PhysRequestMsgIsExtendedId.Value, current.DiagnosticCommParamsECU.PhysRequestMsgId.Value, 0u, 0u, false, 0u));
					stringBuilder.AppendLine();
					stringBuilder.AppendFormat("   {0})", LTLUtil.GetIdString(BusType.Bt_CAN, current.ChannelNumber.Value, current.DiagnosticCommParamsECU.ResponseMsgIsExtendedId.Value, current.DiagnosticCommParamsECU.ResponseMsgId.Value, 0u, 0u, false, 0u));
					break;
				case DiagnosticsAddressingMode.Extended:
				{
					uint arg_132_0 = current.DiagnosticCommParamsECU.ExtAddressingModeRqExtension.Value;
					uint arg_143_0 = current.DiagnosticCommParamsECU.ExtAddressingModeRsExtension.Value;
					stringBuilder.AppendFormat("DIAGECU {0} = (TP = {1} ", current.Qualifier.Value, LTLDiagnostics.ConstantNameForIsoTpExtended);
					stringBuilder.AppendLine();
					stringBuilder.AppendFormat("  ({0} 0x{1:X2}", LTLUtil.GetIdString(BusType.Bt_CAN, current.ChannelNumber.Value, current.DiagnosticCommParamsECU.PhysRequestMsgIsExtendedId.Value, current.DiagnosticCommParamsECU.PhysRequestMsgId.Value, 0u, 0u, false, 0u), current.DiagnosticCommParamsECU.ExtAddressingModeRqExtension.Value);
					stringBuilder.AppendLine();
					stringBuilder.AppendFormat("   {0} 0x{1:X2})", LTLUtil.GetIdString(BusType.Bt_CAN, current.ChannelNumber.Value, current.DiagnosticCommParamsECU.ResponseMsgIsExtendedId.Value, current.DiagnosticCommParamsECU.ResponseMsgId.Value, 0u, 0u, false, 0u), current.DiagnosticCommParamsECU.ExtAddressingModeRsExtension.Value);
					break;
				}
				default:
					goto IL_21F;
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("  REQUEST_DEFAULTSESSION  = {0}", this.GetMessageFormattedInBrackets(current.DiagnosticCommParamsECU.DefaultSessionId.Value));
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("  REQUEST_EXTENDEDSESSION = {0}", this.GetMessageFormattedInBrackets(current.DiagnosticCommParamsECU.ExtendedSessionId.Value));
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("  P2Timeout = {0} {{ms}}", current.DiagnosticCommParamsECU.P2Timeout.Value);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("  P2Extension = {0} {{ms}}", current.DiagnosticCommParamsECU.P2Extension.Value);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("  UseStopCommunication = {0}", current.DiagnosticCommParamsECU.UseStopCommRequest.Value ? "On" : "Off");
				stringBuilder.AppendLine();
				bool value = current.DiagnosticCommParamsECU.UsePaddingBytes.Value;
				stringBuilder.AppendFormat("  UsePaddingBytes = {0}", value ? "On" : "Off");
				stringBuilder.AppendLine();
				if (value)
				{
					stringBuilder.AppendFormat("  PaddingByteValue = 0x{0:X2}", current.DiagnosticCommParamsECU.PaddingByteValue.Value);
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendFormat("  STMin = {0}", current.DiagnosticCommParamsECU.STMin.Value);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("  FirstSequenceNumber = {0}", (int)(current.DiagnosticCommParamsECU.FirstConsecFrameValue.Value & 15));
				stringBuilder.AppendLine();
				uint num = 0u;
				LTLGenerator.LTLGenerationResult result;
				if (!this.diagSymbolsManager.GetDtcLength(FileSystemServices.GetAbsolutePath(current.Database.FilePath.Value, this.configurationFolderPath), current.Qualifier.Value, current.Variant.Value, out num))
				{
					result = LTLGenerator.LTLGenerationResult.DiagError;
					return result;
				}
				if (2u <= num && num <= 3u)
				{
					stringBuilder.AppendFormat("  DTCLength = {0}", num);
				}
				else
				{
					stringBuilder.AppendFormat("  {{ DTCLegth = {0} / no DTCs defined in database? }}", num);
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("  )");
				stringBuilder.AppendLine();
				base.LtlCode.Append(LTLUtil.GetSignAlignedText(stringBuilder.ToString(), '='));
				continue;
				IL_21F:
				result = LTLGenerator.LTLGenerationResult.DiagError_UnknownAddressingMode;
				return result;
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateServicesListsAndEvents()
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			uint num = 0u;
			uint num2 = 0u;
			bool flag = GlobalSettings.VehicleSleepIndicationFlag.Length > 0;
			foreach (TriggeredDiagnosticActionSequence current in from tas in this.diagActionsConfig.TriggeredActionSequences
			where tas.Actions.Any<DiagnosticAction>() && !(tas.Actions[0] is DiagnosticDummyAction)
			select tas)
			{
				num2 += 1u;
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.AppendFormat("DIAGLIST {0} = [", this.GetLtlListName(num2));
				int length = stringBuilder3.Length;
				uint num3 = 0u;
				HashSet<string> hashSet = new HashSet<string>();
				foreach (DiagnosticAction current2 in current.Actions.Where((DiagnosticAction a) => !(a is DiagnosticDummyAction)))
				{
					DiagnosticsDatabase diagnosticsDatabase;
					DiagnosticsECU diagnosticsECU;
					if (this.diagDbConfig.TryGetDiagnosticsDatabase(current2.DatabasePath.Value, out diagnosticsDatabase) && diagnosticsDatabase.TryGetECU(current2.EcuQualifier.Value, out diagnosticsECU))
					{
						num3 += 1u;
						StringBuilder stringBuilder4 = new StringBuilder();
						stringBuilder4.AppendFormat("{0} {1}", diagnosticsECU.Qualifier.Value, this.GetMessageFormattedInBrackets(current2.MessageData.Value));
						if (current2.SessionType.Value == DiagSessionType.Extended)
						{
							stringBuilder4.AppendFormat("  SESSIONTYPE = {0}", LTLDiagnostics.ConstantNameForExtendedSession);
						}
						else if (current2.SessionType.Value == DiagSessionType.DynamicFromDB)
						{
							DiagSessionType serviceSessionTypeFromDB = this.diagSymbolsManager.GetServiceSessionTypeFromDB(current2.EcuQualifier.Value, diagnosticsECU.Variant.Value, current2.ServiceQualifier.Value, current2.MessageData.Value);
							if (serviceSessionTypeFromDB == DiagSessionType.Extended)
							{
								stringBuilder4.AppendFormat("  SESSIONTYPE = {0}", LTLDiagnostics.ConstantNameForExtendedSession);
							}
						}
						if (current2 is DiagnosticSignalRequest)
						{
							if (!hashSet.Add(stringBuilder4.ToString()))
							{
								continue;
							}
							DiagnosticSignalRequest key = current2 as DiagnosticSignalRequest;
							string ltlServiceName = this.GetLtlServiceName(current2.ServiceQualifier.Value, num + 1u);
							if (!GlobalSettings.ServiceQualifiers.ContainsKey(key))
							{
								GlobalSettings.ServiceQualifiers.Add(key, ltlServiceName);
							}
						}
						num += 1u;
						stringBuilder.AppendFormat("DIAGSERVICE {0} = {1}", this.GetLtlServiceName(current2.ServiceQualifier.Value, num), stringBuilder4.ToString());
						stringBuilder.AppendLine();
						if (num3 > 1u)
						{
							stringBuilder3.AppendLine();
							stringBuilder3.Append(' ', length);
						}
						stringBuilder3.AppendFormat("{0}", this.GetLtlServiceName(current2.ServiceQualifier.Value, num));
					}
				}
				stringBuilder3.AppendLine("]");
				LTLGenericEventCode lTLGenericEventCode;
				if (!LTLEventFactory.GetLtlEventCodeObject(current.Event, this.GetLtlListPollBit(num2), this.databaseManager, this.configurationFolderPath, out lTLGenericEventCode))
				{
					return LTLGenerator.LTLGenerationResult.DiagError_UnsupportedEvent;
				}
				lTLGenericEventCode.AddBlockCondition(LTLDiagnostics.Name_ExternalTesterPresentFlag);
				if (flag && current.Event is CyclicTimerEvent)
				{
					lTLGenericEventCode.AddBlockCondition(GlobalSettings.VehicleSleepIndicationFlag);
				}
				string value;
				lTLGenericEventCode.GenerateCode(out value);
				stringBuilder2.AppendLine(LTLUtil.GetFormattedSubsectionHeaderComment(string.Format("List {0:D}:", num2)));
				stringBuilder2.Append(stringBuilder3);
				stringBuilder2.AppendLine();
				stringBuilder2.Append(value);
				stringBuilder2.AppendLine();
			}
			base.LtlCode.AppendLine(LTLUtil.GetFormattedSubsectionHeaderComment("Service Definitions:"));
			string signAlignedText = LTLUtil.GetSignAlignedText(stringBuilder.ToString(), '=');
			signAlignedText = LTLUtil.GetSignAlignedText(signAlignedText, '[');
			base.LtlCode.Append(signAlignedText);
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine(LTLUtil.GetFormattedSubsectionHeaderComment("Service Lists requested on event:"));
			base.LtlCode.Append(stringBuilder2);
			base.LtlCode.AppendLine();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateExternalTesterDetection()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(LTLUtil.GetFormattedSubsectionHeaderComment("External tester detection"));
			StringBuilder stringBuilder2 = new StringBuilder();
			HashSet<DiagnosticsECU> actuallyUsedEcus = this.GetActuallyUsedEcus();
			bool flag = true;
			foreach (DiagnosticsECU current in actuallyUsedEcus)
			{
				if (!flag)
				{
					stringBuilder2.AppendLine();
					stringBuilder2.Append(" ");
				}
				flag = false;
				stringBuilder2.AppendFormat("{0}", LTLUtil.GetIdString(BusType.Bt_CAN, current.ChannelNumber.Value, current.DiagnosticCommParamsECU.PhysRequestMsgIsExtendedId.Value, current.DiagnosticCommParamsECU.PhysRequestMsgId.Value, 0u, 0u, false, 0u));
			}
			stringBuilder.AppendFormat("VAR {0}         = FREE[1]", LTLDiagnostics.Name_ExternalTesterPresentFlag);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("VAR {0}_Timeout = FREE[16]", LTLDiagnostics.Name_ExternalTesterPresentFlag);
			stringBuilder.AppendLine();
			StringBuilder stringBuilder3 = new StringBuilder();
			stringBuilder3.AppendFormat("EVENT ON RECEIVE ({0}) BEGIN", stringBuilder2.ToString());
			stringBuilder3.AppendLine();
			stringBuilder3.AppendFormat("  CALC Systemrequest = DiagAbortRequest WHEN (This.Rx)", new object[0]);
			stringBuilder3.AppendLine();
			stringBuilder3.AppendFormat("  CALC {0}         = (1) WHEN (This.Rx)", LTLDiagnostics.Name_ExternalTesterPresentFlag);
			stringBuilder3.AppendLine();
			if (this.diagActionsConfig.IsDiagCommRestartEnabled.Value)
			{
				stringBuilder3.AppendFormat("  CALC {0}_Timeout = ({1:D}) WHEN (This.Rx)", LTLDiagnostics.Name_ExternalTesterPresentFlag, this.diagActionsConfig.DiagCommRestartTime.Value * 60u);
				stringBuilder3.AppendLine();
			}
			stringBuilder3.AppendLine("END");
			stringBuilder3.AppendLine();
			stringBuilder3 = new StringBuilder(LTLUtil.GetSignAlignedText(stringBuilder3.ToString(), "="));
			stringBuilder3 = new StringBuilder(LTLUtil.GetSignAlignedText(stringBuilder3.ToString(), "WHEN"));
			stringBuilder.Append(stringBuilder3);
			if (this.diagActionsConfig.IsDiagCommRestartEnabled.Value)
			{
				stringBuilder3 = new StringBuilder();
				stringBuilder3.AppendFormat("EVENT ON CYCLE ({0:D}) BEGIN", 1000);
				stringBuilder3.AppendLine();
				stringBuilder3.AppendFormat("  CALC {0}_Timeout = {0}_Timeout - 1 WHEN ({0}_Timeout > 0)", LTLDiagnostics.Name_ExternalTesterPresentFlag);
				stringBuilder3.AppendLine();
				stringBuilder3.AppendFormat("  CALC {0} = (0) WHEN ({0}_Timeout = 0)", LTLDiagnostics.Name_ExternalTesterPresentFlag);
				stringBuilder3.AppendLine();
				stringBuilder3.AppendLine("END");
				stringBuilder3.AppendLine();
				stringBuilder3 = new StringBuilder(LTLUtil.GetSignAlignedText(stringBuilder3.ToString(), "="));
				stringBuilder3 = new StringBuilder(LTLUtil.GetSignAlignedText(stringBuilder3.ToString(), "WHEN"));
				stringBuilder.Append(stringBuilder3);
			}
			base.LtlCode.Append(LTLUtil.GetSignAlignedText(stringBuilder.ToString(), "CAN"));
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private HashSet<DiagnosticsECU> GetActuallyUsedEcus()
		{
			HashSet<DiagnosticsECU> hashSet = new HashSet<DiagnosticsECU>();
			foreach (TriggeredDiagnosticActionSequence current in this.diagActionsConfig.TriggeredActionSequences)
			{
				foreach (DiagnosticAction current2 in from a in current.Actions
				where !(a is DiagnosticDummyAction)
				select a)
				{
					DiagnosticsDatabase diagnosticsDatabase;
					DiagnosticsECU item;
					if (this.diagDbConfig.TryGetDiagnosticsDatabase(current2.DatabasePath.Value, out diagnosticsDatabase) && diagnosticsDatabase.TryGetECU(current2.EcuQualifier.Value, out item))
					{
						hashSet.Add(item);
					}
				}
			}
			return hashSet;
		}

		private string GetLtlServiceName(string serviceQualifier, uint indexNumber)
		{
			return string.Format("{0}_{1:D}", serviceQualifier, indexNumber);
		}

		private string GetLtlListName(uint listIndex)
		{
			return string.Format("ServiceList{0:D}", listIndex);
		}

		private string GetLtlListPollBit(uint listIndex)
		{
			return string.Format("{0}.Poll", this.GetLtlListName(listIndex));
		}

		private byte GetMessageSid(byte[] messageByteDump)
		{
			if (messageByteDump.Length > 0)
			{
				return messageByteDump[0];
			}
			return 0;
		}

		private string GetMessageParameterDumpAsHexStream(byte[] messageByteDump)
		{
			if (messageByteDump.Length > 1)
			{
				StringBuilder stringBuilder = new StringBuilder();
				uint num = 1u;
				while ((ulong)num < (ulong)((long)messageByteDump.Length))
				{
					if (num >= 2u)
					{
						stringBuilder.Append(" ");
					}
					stringBuilder.AppendFormat("0x{0:X2}", messageByteDump[(int)((UIntPtr)num)]);
					num += 1u;
				}
				return stringBuilder.ToString();
			}
			return string.Empty;
		}

		private string GetMessageFormattedInBrackets(byte[] messagebyteDump)
		{
			if (messagebyteDump.Length <= 0)
			{
				return string.Empty;
			}
			return string.Format("[0x{0:X2}, {1}]", this.GetMessageSid(messagebyteDump), this.GetMessageParameterDumpAsHexStream(messagebyteDump));
		}

		private string GetMessageFormattedInBrackets(ulong messageDump)
		{
			IList<byte> list = new List<byte>();
			bool flag = false;
			int i = 7;
			while (i >= 0)
			{
				byte b = (byte)(messageDump >> i * 8);
				if (flag)
				{
					goto IL_28;
				}
				if (b > 0)
				{
					flag = true;
					goto IL_28;
				}
				IL_2F:
				i--;
				continue;
				IL_28:
				list.Add(b);
				goto IL_2F;
			}
			byte[] array = new byte[list.Count];
			for (int j = 0; j < list.Count; j++)
			{
				array[j] = list[j];
			}
			return this.GetMessageFormattedInBrackets(array);
		}
	}
}
