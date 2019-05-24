using System;
using System.Globalization;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpTriggerConditionLinId : DpTriggerCondition
	{
		private readonly LINIdEvent mEvent;

		public override string ChannelString
		{
			get
			{
				return this.mEvent.ChannelNumber.Value.ToString(CultureInfo.InvariantCulture);
			}
		}

		public override string NameOfCaplEventHandler
		{
			get
			{
				return CaplHelper.MakeLinMsgHandlerString(this.ChannelString);
			}
		}

		public override string TemplateEventHandler
		{
			get
			{
				return "TEventHandler_Condition_Msg";
			}
		}

		public string MsgConditionString
		{
			get
			{
				return CaplHelper.MakeConditionStringLinId(this.mEvent.IdRelation.Value, "this.ID", this.mEvent.LowId.Value, this.mEvent.HighId.Value);
			}
		}

		public DpTriggerConditionLinId(uint triggerIdent, uint conditionIdent, Event conditionEvent) : base(triggerIdent, conditionIdent, conditionEvent)
		{
			this.mEvent = (conditionEvent as LINIdEvent);
		}
	}
}
