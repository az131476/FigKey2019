using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpTriggerConditionMsgTimeoutCan : DpTriggerConditionMsgTimeout
	{
		public override string NameOfCaplEventHandler
		{
			get
			{
				return CaplHelper.MakeCanMsgHandlerString(this.ChannelString);
			}
		}

		public override string MsgSymbolic
		{
			get
			{
				return CaplHelper.MakeCanMsgSymbolString(this.ChannelString, this.mEvent.MessageName.Value);
			}
		}

		public DpTriggerConditionMsgTimeoutCan(uint triggerIdent, uint conditionIdent, Event conditionEvent) : base(triggerIdent, conditionIdent, conditionEvent)
		{
		}
	}
}
