using System;
using System.Globalization;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpTriggerConditionDigitalInput : DpTriggerCondition
	{
		private readonly DigitalInputEvent mEvent;

		public override string ChannelString
		{
			get
			{
				uint value = this.mEvent.DigitalInput.Value;
				return (value - 1u).ToString(CultureInfo.InvariantCulture);
			}
		}

		public override string NameOfCaplEventHandler
		{
			get
			{
				return string.Format("on sysvar_update sysvar::IO::VN1600_1::DIN{0}", this.ChannelString);
			}
		}

		public override string TemplateVariables
		{
			get
			{
				return "TVariables_Condition_DigitalInput";
			}
		}

		public override string TemplateEventHandler
		{
			get
			{
				return "TEventHandler_Condition_DigitalInput";
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
				return "TAdditional_Condition_DigitalInput";
			}
		}

		public bool Edge
		{
			get
			{
				return this.mEvent.Edge.Value;
			}
		}

		public uint DebounceTime
		{
			get
			{
				return this.mEvent.DebounceTime.Value;
			}
		}

		public string ValueConditionString
		{
			get
			{
				return CaplHelper.MakeConditionString(CondRelation.Equal, "@this", (this.mEvent.Edge.Value ? 1 : 0).ToString(CultureInfo.InvariantCulture), null);
			}
		}

		public DpTriggerConditionDigitalInput(uint triggerIdent, uint conditionIdent, Event conditionEvent) : base(triggerIdent, conditionIdent, conditionEvent)
		{
			this.mEvent = (conditionEvent as DigitalInputEvent);
		}
	}
}
