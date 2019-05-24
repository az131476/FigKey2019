using System;
using System.Text;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLMessageTimeoutEventCode : LTLGenericEventCode
	{
		private MsgTimeoutEvent msgTimeoutEvent;

		private IApplicationDatabaseManager dbManager;

		private string configurationFolderPath;

		public LTLMessageTimeoutEventCode(string eventFlagName, MsgTimeoutEvent ev, IApplicationDatabaseManager dbManager, string configurationFolderPath) : base(eventFlagName)
		{
			this.msgTimeoutEvent = ev;
			this.dbManager = dbManager;
			this.configurationFolderPath = configurationFolderPath;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			if (GlobalSettings.VehicleSleepIndicationFlag.Length > 0)
			{
				this.AddBlockCondition(GlobalSettings.VehicleSleepIndicationFlag);
			}
			bool isCanExtendedId = this.msgTimeoutEvent.IsExtendedId.Value;
			uint id = this.msgTimeoutEvent.ID.Value;
			if (this.msgTimeoutEvent.IsSymbolic.Value)
			{
				MessageDefinition messageDefinition;
				if (!this.dbManager.ResolveMessageSymbolInDatabase(FileSystemServices.GetAbsolutePath(this.msgTimeoutEvent.DatabasePath.Value, this.configurationFolderPath), this.msgTimeoutEvent.NetworkName.Value, this.msgTimeoutEvent.MessageName.Value, this.msgTimeoutEvent.BusType.Value, out messageDefinition))
				{
					ltlCode = string.Empty;
					return LTLGenerator.LTLGenerationResult.TriggerError_MessageResolve;
				}
				isCanExtendedId = messageDefinition.IsExtendedId;
				id = messageDefinition.ActualMessageId;
				this.msgTimeoutEvent.DatabaseCycleTime = (uint)messageDefinition.CycleTime;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("TIMEOUT {0} TIME = {1:D}  ({2})", this.GetTriggerVarName(), this.msgTimeoutEvent.ResultingTimeout, LTLUtil.GetIdString(this.msgTimeoutEvent.BusType.Value, this.msgTimeoutEvent.ChannelNumber.Value, isCanExtendedId, id));
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("        RESET = (NOT {0})", LTLUtil.StartTimeDelayFlag);
			base.PreEventCode = stringBuilder.ToString();
			base.TriggerEvent = string.Format("SET ({0})", this.GetTriggerVarName());
			base.WhenCondition = null;
			base.AdditionalCodeBeforeFlagSet = null;
			base.AdditionalCodeAfterFlagSet = null;
			base.PostEventCode = null;
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private string GetTriggerVarName()
		{
			return string.Format("{0}_Timeout", base.GetEventFlagNameWithoutDot());
		}

		protected override string GetComment()
		{
			return string.Format("Event on {0} {1} message {2} timeout for {3} ms", new object[]
			{
				this.msgTimeoutEvent.IsSymbolic.Value ? "symbolic" : "raw",
				LTLUtil.GetBusTypeString(this.msgTimeoutEvent.BusType.Value),
				this.msgTimeoutEvent.IsSymbolic.Value ? this.msgTimeoutEvent.MessageName.Value : LTLUtil.GetIdString(this.msgTimeoutEvent.BusType.Value, this.msgTimeoutEvent.ChannelNumber.Value, this.msgTimeoutEvent.IsExtendedId.Value, this.msgTimeoutEvent.ID.Value),
				this.msgTimeoutEvent.ResultingTimeout
			});
		}

		public override LTLGenerator.LTLGenerationResult GetCurrentStateFlag(out string ltlVariable)
		{
			ltlVariable = this.GetTriggerVarName();
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
