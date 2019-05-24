using System;
using System.Globalization;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal abstract class DpTriggerConditionSymbolicSignal : DpTriggerCondition
	{
		protected readonly SymbolicSignalEvent mEvent;

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
				if (this.mEvent.Relation.Value != CondRelation.OnChange)
				{
					return "TVariables_Condition";
				}
				return "TVariables_Condition_SymbolicSignal_OnChange";
			}
		}

		public override string TemplateEventHandler
		{
			get
			{
				if (this.mEvent.Relation.Value != CondRelation.OnChange)
				{
					return "TEventHandler_Condition_SymbolicSignal";
				}
				return "TEventHandler_Condition_SymbolicSignal_OnChange";
			}
		}

		public string MsgConditionString
		{
			get
			{
				return CaplHelper.MakeConditionString(CondRelation.Equal, "this.ID", this.MsgSymbolic + ".ID", null);
			}
		}

		public string ValueConditionString
		{
			get
			{
				string variableString = string.Format("${0}::{1}.raw", this.MsgSymbolic, this.mEvent.SignalName.Value);
				return CaplHelper.MakeConditionString(this.mEvent.Relation.Value, variableString, this.mEvent.LowValue.Value.ToString(CultureInfo.InvariantCulture), this.mEvent.HighValue.Value.ToString(CultureInfo.InvariantCulture));
			}
		}

		public abstract string MsgSymbolic
		{
			get;
		}

		protected DpTriggerConditionSymbolicSignal(uint triggerIdent, uint conditionIdent, Event conditionEvent) : base(triggerIdent, conditionIdent, conditionEvent)
		{
			this.mEvent = (conditionEvent as SymbolicSignalEvent);
		}
	}
}
