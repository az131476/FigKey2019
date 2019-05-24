using System;
using System.Globalization;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpTriggerConditionAnalogInput : DpTriggerCondition
	{
		private readonly AnalogInputEvent mEvent;

		public override string ChannelString
		{
			get
			{
				return string.Empty;
			}
		}

		public override string NameOfCaplEventHandler
		{
			get
			{
				return string.Format("on sysvar_update sysvar::IO::VN1600_1::AIN", new object[0]);
			}
		}

		public override string TemplateEventHandler
		{
			get
			{
				return "TEventHandler_Condition_AnalogInput";
			}
		}

		public uint Tolerance
		{
			get
			{
				return this.mEvent.Tolerance.Value;
			}
		}

		public string ValueConditionString
		{
			get
			{
				uint num = this.mEvent.LowValue.Value;
				uint num2 = this.mEvent.HighValue.Value;
				switch (this.mEvent.Relation.Value)
				{
				case CondRelation.LessThanOrEqual:
					num += this.Tolerance;
					break;
				case CondRelation.GreaterThanOrEqual:
					num -= this.Tolerance;
					break;
				case CondRelation.InRange:
					num -= this.Tolerance;
					num2 += this.Tolerance;
					break;
				case CondRelation.NotInRange:
					num += this.Tolerance;
					num2 -= this.Tolerance;
					break;
				}
				return CaplHelper.MakeConditionString(this.mEvent.Relation.Value, "(@this*1000)", num.ToString(CultureInfo.InvariantCulture), num2.ToString(CultureInfo.InvariantCulture));
			}
		}

		public DpTriggerConditionAnalogInput(uint triggerIdent, uint conditionIdent, Event conditionEvent) : base(triggerIdent, conditionIdent, conditionEvent)
		{
			this.mEvent = (conditionEvent as AnalogInputEvent);
		}
	}
}
