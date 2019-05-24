using System;
using System.Collections.ObjectModel;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpTrigger
	{
		private readonly uint mConditionIdent;

		public uint Ident
		{
			get;
			private set;
		}

		public RecordTrigger RecordTrigger
		{
			get;
			private set;
		}

		public Event Event
		{
			get
			{
				return this.RecordTrigger.Event;
			}
		}

		public bool IsLoggingOnTrigger
		{
			get
			{
				return this.RecordTrigger.TriggerEffect.Value == TriggerEffect.LoggingOn;
			}
		}

		public bool IsLoggingOffTrigger
		{
			get
			{
				return this.RecordTrigger.TriggerEffect.Value == TriggerEffect.LoggingOff;
			}
		}

		public bool TriggerActionBeep
		{
			get
			{
				return this.RecordTrigger.Action.Value == TriggerAction.Beep;
			}
		}

		public DpTriggerCondition Condition
		{
			get;
			private set;
		}

		public ReadOnlyCollection<DpTriggerCondition> ChildConditionListFlat
		{
			get
			{
				return new ReadOnlyCollection<DpTriggerCondition>(this.Condition.ChildConditionListFlat);
			}
		}

		public DpTrigger(uint triggerIdent, RecordTrigger trigger, uint conditionIdent)
		{
			this.Ident = triggerIdent;
			this.RecordTrigger = trigger;
			this.mConditionIdent = conditionIdent;
			this.Condition = DpTriggerCondition.Create(this.Ident, this.mConditionIdent, this.RecordTrigger.Event, true);
		}
	}
}
