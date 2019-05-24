using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpTriggerConditionSymbolicMessageLin : DpTriggerConditionSymbolicMessage
	{
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

		public override string MsgConditionString
		{
			get
			{
				return CaplHelper.MakeConditionString(CondRelation.Equal, "this.ID", this.MsgSymbolic + ".ID", null);
			}
		}

		public string MsgSymbolic
		{
			get
			{
				return CaplHelper.MakeLinMsgSymbolString(this.ChannelString, this.mEvent.MessageName.Value);
			}
		}

		public DpTriggerConditionSymbolicMessageLin(uint triggerIdent, uint conditionIdent, Event conditionEvent) : base(triggerIdent, conditionIdent, conditionEvent)
		{
		}
	}
}
