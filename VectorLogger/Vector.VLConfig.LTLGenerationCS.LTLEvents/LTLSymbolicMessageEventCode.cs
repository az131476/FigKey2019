using System;
using System.Collections.Generic;
using System.Text;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLSymbolicMessageEventCode : LTLGenericEventCode
	{
		private SymbolicMessageEvent symMsgEvent;

		private IApplicationDatabaseManager dbManager;

		private string configurationFolderPath;

		public LTLSymbolicMessageEventCode(string eventFlagName, SymbolicMessageEvent ev, IApplicationDatabaseManager dbManager, string configurationFolderPath) : base(eventFlagName)
		{
			this.symMsgEvent = ev;
			this.dbManager = dbManager;
			this.configurationFolderPath = configurationFolderPath;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			MessageDefinition messageDefinition;
			if (!this.dbManager.ResolveMessageSymbolInDatabase(FileSystemServices.GetAbsolutePath(this.symMsgEvent.DatabasePath.Value, this.configurationFolderPath), this.symMsgEvent.NetworkName.Value, this.symMsgEvent.MessageName.Value, this.symMsgEvent.BusType.Value, out messageDefinition))
			{
				ltlCode = string.Empty;
				return LTLGenerator.LTLGenerationResult.TriggerError_MessageResolve;
			}
			bool isExtendedId = messageDefinition.IsExtendedId;
			if (this.symMsgEvent.BusType.Value == BusType.Bt_FlexRay)
			{
				IList<MessageDefinition> list = null;
				IList<string> list2 = null;
				bool flag;
				this.dbManager.GetFlexrayFrameOrPDUInfo(FileSystemServices.GetAbsolutePath(this.symMsgEvent.DatabasePath.Value, this.configurationFolderPath), this.symMsgEvent.NetworkName.Value, this.symMsgEvent.MessageName.Value, this.symMsgEvent.IsFlexrayPDU.Value, out list, out list2, out flag);
				int num = 0;
				using (IEnumerator<MessageDefinition> enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MessageDefinition current = enumerator.Current;
						if (num > 0)
						{
							stringBuilder.AppendLine();
							stringBuilder.AppendFormat("{0,-18:}", " ");
						}
						stringBuilder.Append(LTLUtil.GetIdString(this.symMsgEvent.BusType.Value, this.symMsgEvent.ChannelNumber.Value, false, current.ActualMessageId, (uint)current.FrBaseCycle, (uint)current.FrCycleRepetition, false, 0u));
						num++;
					}
					goto IL_1CD;
				}
			}
			stringBuilder.Append(LTLUtil.GetIdString(this.symMsgEvent.BusType.Value, this.symMsgEvent.ChannelNumber.Value, isExtendedId, messageDefinition.ActualMessageId, (uint)messageDefinition.FrBaseCycle, (uint)messageDefinition.FrCycleRepetition, false, 0u));
			IL_1CD:
			base.PreEventCode = null;
			base.TriggerEvent = string.Format("RECEIVE ({0})", stringBuilder);
			base.WhenCondition = null;
			base.AdditionalCodeBeforeFlagSet = null;
			base.AdditionalCodeAfterFlagSet = null;
			base.PostEventCode = null;
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			MessageDefinition messageDefinition;
			if (!this.dbManager.ResolveMessageSymbolInDatabase(FileSystemServices.GetAbsolutePath(this.symMsgEvent.DatabasePath.Value, this.configurationFolderPath), this.symMsgEvent.NetworkName.Value, this.symMsgEvent.MessageName.Value, this.symMsgEvent.BusType.Value, out messageDefinition))
			{
				return "{ Unresolved Symbolic Message Event }";
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.symMsgEvent.BusType.Value == BusType.Bt_FlexRay)
			{
				if (this.symMsgEvent.IsFlexrayPDU.Value)
				{
					stringBuilder.AppendFormat("Event on Symbolic FlexRay PDU {0}", this.symMsgEvent.MessageName.Value);
				}
				else
				{
					stringBuilder.AppendFormat("Event on Symbolic FlexRay Frame {0} (ID {1:D}:{2:D}:{3:D})", new object[]
					{
						this.symMsgEvent.MessageName.Value,
						messageDefinition.ActualMessageId,
						messageDefinition.FrBaseCycle,
						messageDefinition.FrCycleRepetition
					});
				}
			}
			else
			{
				stringBuilder.AppendFormat("Event on Symbolic Message {0} (ID 0x{1:X})", this.symMsgEvent.MessageName.Value, messageDefinition.ActualMessageId);
			}
			return stringBuilder.ToString();
		}
	}
}
