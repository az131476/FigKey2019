using System;
using System.Globalization;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal abstract class DpTriggerConditionMsgTimeout : DpTriggerCondition
	{
		protected readonly MsgTimeoutEvent mEvent;

		public override string ChannelString
		{
			get
			{
				return this.mEvent.ChannelNumber.Value.ToString(CultureInfo.InvariantCulture);
			}
		}

		public override string TemplateVariables
		{
			get
			{
				return "TVariables_Condition_MsgTimeout";
			}
		}

		public override string TemplateEventHandler
		{
			get
			{
				return "TEventHandler_Condition_MsgTimeout";
			}
		}

		public override bool TemplateCallsProcessTriggerInEventHandler
		{
			get
			{
				return true;
			}
		}

		public override string TemplateAdditionalFunction
		{
			get
			{
				return "TAdditional_Condition_MsgTimeout";
			}
		}

		public override string TemplateStartupCode
		{
			get
			{
				return "TAdditional_Condition_MsgTimeout_Startup";
			}
		}

		public uint ResultingTimeout
		{
			get
			{
				return this.mEvent.ResultingTimeout;
			}
		}

		public string MsgConditionString
		{
			get
			{
				if (!string.IsNullOrEmpty(this.mEvent.MessageName.Value))
				{
					return CaplHelper.MakeConditionString(CondRelation.Equal, "this.ID", this.MsgSymbolic + ".ID", null);
				}
				string valueLowString;
				if (this.mEvent.BusType.Value == BusType.Bt_CAN && this.mEvent.IsExtendedId.Value)
				{
					valueLowString = "mkExtId(" + this.mEvent.ID.Value.ToString(CultureInfo.InvariantCulture) + ")";
				}
				else
				{
					valueLowString = this.mEvent.ID.Value.ToString(CultureInfo.InvariantCulture);
				}
				return CaplHelper.MakeConditionString(CondRelation.Equal, "this.ID", valueLowString, null);
			}
		}

		public abstract string MsgSymbolic
		{
			get;
		}

		protected DpTriggerConditionMsgTimeout(uint triggerIdent, uint conditionIdent, Event conditionEvent) : base(triggerIdent, conditionIdent, conditionEvent)
		{
			this.mEvent = (conditionEvent as MsgTimeoutEvent);
		}

		public override bool Validate(IApplicationDatabaseManager dbManager, string configurationFolderPath)
		{
			if (!this.mEvent.IsSymbolic.Value)
			{
				return true;
			}
			MessageDefinition messageDefinition;
			if (!dbManager.ResolveMessageSymbolInDatabase(FileSystemServices.GetAbsolutePath(this.mEvent.DatabasePath.Value, configurationFolderPath), this.mEvent.NetworkName.Value, this.mEvent.MessageName.Value, this.mEvent.BusType.Value, out messageDefinition))
			{
				return false;
			}
			this.mEvent.DatabaseCycleTime = (uint)messageDefinition.CycleTime;
			return true;
		}
	}
}
