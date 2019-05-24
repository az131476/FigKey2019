using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpTriggerConditionSymbolicSignalLin : DpTriggerConditionSymbolicSignal
	{
		public override string NameOfCaplEventHandler
		{
			get
			{
				return CaplHelper.MakeLinMsgHandlerString(this.ChannelString);
			}
		}

		public override string MsgSymbolic
		{
			get
			{
				return CaplHelper.MakeLinMsgSymbolString(this.ChannelString, this.mEvent.MessageName.Value);
			}
		}

		public DpTriggerConditionSymbolicSignalLin(uint triggerIdent, uint conditionIdent, Event conditionEvent) : base(triggerIdent, conditionIdent, conditionEvent)
		{
		}
	}
}
