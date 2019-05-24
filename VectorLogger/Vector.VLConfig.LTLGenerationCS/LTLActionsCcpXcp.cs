using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLActionsCcpXcp : LTLGenericAction
	{
		private CcpXcpSignalConfiguration signalConfiguration;

		private CcpXcpGenerationInfo generationInfo;

		private StringBuilder controlCode;

		protected override string SectionHeaderComment
		{
			get
			{
				return "CCP/XCP ACTIONS";
			}
		}

		public LTLActionsCcpXcp(CcpXcpSignalConfiguration aSignalConfiguration, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath, CcpXcpGenerationInfo aCcpXcpGenerationInfo) : base(loggerSpecifics, databaseManager, configurationFolderPath)
		{
			this.signalConfiguration = aSignalConfiguration;
			this.generationInfo = aCcpXcpGenerationInfo;
			this.controlCode = new StringBuilder();
		}

		protected override bool IsFeatureSupportedByLogger()
		{
			return this.loggerSpecifics.Recording.IsCcpXcpSupported;
		}

		protected override bool IsNoActionAvtive(out string textForComment)
		{
			textForComment = "no CCP/XCP action configured";
			return !this.signalConfiguration.ActiveActions.Any<ActionCcpXcp>();
		}

		protected override List<Vector.VLConfig.Data.ConfigurationDataModel.Action> GetActionList()
		{
			List<Vector.VLConfig.Data.ConfigurationDataModel.Action> list = new List<Vector.VLConfig.Data.ConfigurationDataModel.Action>();
			foreach (ActionCcpXcp current in this.signalConfiguration.Actions)
			{
				if (current.Event != null)
				{
					list.Add(current);
				}
			}
			return list;
		}

		protected override string GetSetVariableName(int index)
		{
			return string.Format("SetCcpXcpAction{0:D}", index);
		}

		protected override string GetResetVariableName(int index)
		{
			return string.Format("ResetCcpXcpAction{0:D}", index);
		}

		protected override string GetSetResetFlagName(int index)
		{
			return string.Format("CcpXcpActionFlag{0:D}", index);
		}

		protected override string GetActionItemSubSectionComment(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index)
		{
			return string.Format("CCP/XCP request activation {0:D} ({1})", index, base.GetResetTypeStringForComment(base.GetResetType(action.StopType)));
		}

		protected override LTLGenerator.LTLGenerationResult BuildCodeBeforeActions(out string code)
		{
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override LTLGenerator.LTLGenerationResult BuildActionCode(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index, out string code)
		{
			code = string.Empty;
			ActionCcpXcp actionCcpXcp = action as ActionCcpXcp;
			if (actionCcpXcp == null)
			{
				return LTLGenerator.LTLGenerationResult.ActionError;
			}
			List<Tuple<string, uint>> list = new List<Tuple<string, uint>>();
			foreach (CcpXcpDatabaseInfo current in this.generationInfo.DatabaseInfos)
			{
				foreach (CcpXcpEcuInfo current2 in current.EcuInfoList)
				{
					if (current2.Ecu != null)
					{
						foreach (CcpXcpSignalListInfo current3 in current2.SignalListInfos)
						{
							if (!string.IsNullOrEmpty(current3.NameIfTriggeredPollingList) && current3.IsPolling && current3.Signals.Any<CcpXcpSignal>() && current3.ActionReference == action)
							{
								string item = current.IncludeFileInfo.Prefix + current3.NameIfTriggeredPollingList;
								uint item2;
								if (uint.TryParse(current3.Signals[0].PollingTime.Value, out item2))
								{
									list.Add(new Tuple<string, uint>(item, item2));
								}
							}
						}
					}
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			if (actionCcpXcp.Mode == ActionCcpXcp.ActivationMode.Triggered)
			{
				bool flag = actionCcpXcp.StartDelay.Value > 0u;
				string text = this.GetSetVariableName(index);
				if (flag)
				{
					text += "Delayed";
					stringBuilder.AppendFormat("TIMER {0} TIME = {1} ({2})", text, actionCcpXcp.StartDelay.Value, this.GetSetVariableName(index));
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendFormat("EVENT ON SET ({0}) BEGIN", text);
				stringBuilder.AppendLine();
				foreach (Tuple<string, uint> current4 in list)
				{
					stringBuilder.AppendFormat("  CALC {0}.Poll = (1)", current4.Item1);
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendFormat("  CALC {0} = (0)", this.GetSetVariableName(index));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("END");
				stringBuilder.AppendLine();
			}
			else
			{
				bool flag2 = actionCcpXcp.StartDelay.Value > 0u || (!(action.StopType is StopOnTimer) && actionCcpXcp.StopDelay.Value > 0u);
				string text2 = this.GetSetResetFlagName(index);
				string text3 = this.GetSetVariableName(index);
				string text4 = this.GetResetVariableName(index);
				if (flag2)
				{
					text2 += "Delayed";
					if (actionCcpXcp.StartDelay.Value != 0u)
					{
						text3 += "Delayed";
						stringBuilder.AppendFormat("TIMER {0} TIME = {1} ({2})", text3, actionCcpXcp.StartDelay.Value, this.GetSetResetFlagName(index));
						stringBuilder.AppendLine();
					}
					uint num;
					if (action.StopType is StopOnTimer)
					{
						num = 0u;
					}
					else
					{
						num = actionCcpXcp.StopDelay.Value;
					}
					if (num != 0u)
					{
						text4 += "Delayed";
						stringBuilder.AppendFormat("TIMER {0} TIME = {1} (NOT {2})", text4, num, this.GetSetResetFlagName(index));
						stringBuilder.AppendLine();
					}
					if (actionCcpXcp.StartDelay.Value == 0u)
					{
						stringBuilder.AppendFormat("FLAG {0} SET = ({1}) RESET = ({2} AND NOT {1})", text2, this.GetSetResetFlagName(index), text4);
					}
					else if (num == 0u)
					{
						stringBuilder.AppendFormat("FLAG {0} SET = ({1}) RESET = (NOT {2})", text2, text3, this.GetSetResetFlagName(index));
					}
					else
					{
						stringBuilder.AppendFormat("FLAG {0} SET = ({1} AND NOT {2}) RESET = ({2})", text2, text3, text4);
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
				}
				foreach (Tuple<string, uint> current5 in list)
				{
					stringBuilder.AppendFormat("EVENT ON CYCLE ({0}) BEGIN", current5.Item2);
					stringBuilder.AppendLine();
					stringBuilder.AppendFormat("  CALC {0}.Poll = (1) WHEN ({1})", current5.Item1, text2);
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("END");
					stringBuilder.AppendLine();
				}
			}
			code = stringBuilder.ToString();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override LTLGenerator.LTLGenerationResult BuildInactiveActionCode(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index, out string code)
		{
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override LTLGenerator.LTLGenerationResult BuildCodeAfterActions(out string code)
		{
			List<CcpXcpDatabaseInfo> list = new List<CcpXcpDatabaseInfo>();
			foreach (CcpXcpDatabaseInfo current in this.generationInfo.DatabaseInfos)
			{
				if (current.BusType == BusType.Bt_CAN)
				{
					foreach (CcpXcpEcuInfo current2 in current.EcuInfoList)
					{
						if (current2.Ecu != null)
						{
							bool flag = false;
							foreach (CcpXcpSignalListInfo current3 in current2.SignalListInfos)
							{
								if (!current3.IsPolling || current3.IsAlwaysActive())
								{
									flag = true;
								}
							}
							if (!flag)
							{
								list.Add(current);
								break;
							}
						}
					}
				}
			}
			if (list.Any<CcpXcpDatabaseInfo>())
			{
				this.controlCode.AppendLine("{ Send dummy requests to keep CCP/XCP communication active }");
			}
			foreach (CcpXcpDatabaseInfo current4 in list)
			{
				if (current4.EcuInfoList.Any<CcpXcpEcuInfo>())
				{
					CcpXcpEcuInfo ccpXcpEcuInfo = current4.EcuInfoList[0];
					foreach (string current5 in current4.IncludeFileInfo.TimeoutVariableNameList)
					{
						string arg = current5.Replace("%2%", current4.IncludeFileInfo.Prefix);
						string text = null;
						if (ccpXcpEcuInfo.Database.CPType.Value == CPType.CCP)
						{
							text = "0x17(7:0) 0x00(15:8)";
						}
						else if (ccpXcpEcuInfo.Database.CPType.Value == CPType.XCP)
						{
							text = "0xFD(7:0)";
						}
						if (!string.IsNullOrEmpty(text))
						{
							this.controlCode.AppendLine("EVENT");
							this.controlCode.AppendFormat("  ON CALC ({0} = 1) BEGIN", arg);
							this.controlCode.AppendLine();
							this.controlCode.AppendFormat("    TRANSMIT {0} [{1}] LOG", LTLUtil.GetIdString(BusType.Bt_CAN, current4.ChannelNumber, ccpXcpEcuInfo.EffectiveIsCanRequestIdExtended, ccpXcpEcuInfo.EffectiveCanRequestID), text);
							this.controlCode.AppendLine();
							this.controlCode.AppendLine("  END");
							this.controlCode.AppendLine();
						}
					}
				}
			}
			code = this.controlCode.ToString();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override uint GetAdditionalStopTimerDuration(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			ActionCcpXcp actionCcpXcp = action as ActionCcpXcp;
			if (actionCcpXcp == null || actionCcpXcp.Mode != ActionCcpXcp.ActivationMode.Conditional)
			{
				return 0u;
			}
			if (actionCcpXcp.StopType is StopOnTimer)
			{
				return actionCcpXcp.StartDelay.Value;
			}
			return 0u;
		}
	}
}
