using System;
using System.Globalization;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal abstract class DpTriggerConditionSymbolicMessage : DpTriggerCondition
	{
		protected readonly SymbolicMessageEvent mEvent;

		public override string ChannelString
		{
			get
			{
				return this.mEvent.ChannelNumber.Value.ToString(CultureInfo.InvariantCulture);
			}
		}

		public abstract string MsgConditionString
		{
			get;
		}

		protected DpTriggerConditionSymbolicMessage(uint triggerIdent, uint conditionIdent, Event conditionEvent) : base(triggerIdent, conditionIdent, conditionEvent)
		{
			this.mEvent = (conditionEvent as SymbolicMessageEvent);
		}
	}
}
