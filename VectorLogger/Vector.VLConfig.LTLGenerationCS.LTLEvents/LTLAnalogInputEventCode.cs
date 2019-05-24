using System;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLAnalogInputEventCode : LTLGenericEventCode
	{
		private AnalogInputEvent analogInputEvent;

		private string analogConditionFlagName;

		public LTLAnalogInputEventCode(string eventFlagName, AnalogInputEvent ev) : base(eventFlagName)
		{
			this.analogInputEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			this.analogConditionFlagName = string.Format("{0}_AnalogCondition", base.GetEventFlagNameWithoutDot());
			ltlCode = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			switch (this.analogInputEvent.Relation.Value)
			{
			case CondRelation.Equal:
			case CondRelation.NotEqual:
			case CondRelation.LessThan:
			case CondRelation.GreaterThan:
				return LTLGenerator.LTLGenerationResult.TriggerError_UnsupportedCondition;
			case CondRelation.LessThanOrEqual:
			{
				string arg;
				if (!LTLUtil.GetLTLCompareOperatorString(CondRelation.LessThanOrEqual, out arg))
				{
					return LTLGenerator.LTLGenerationResult.TriggerError;
				}
				string arg2;
				if (!LTLUtil.GetLTLCompareOperatorString(CondRelation.GreaterThan, out arg2))
				{
					return LTLGenerator.LTLGenerationResult.TriggerError;
				}
				stringBuilder.AppendFormat("SET = (AnaIn{0} {1} {2}) ", this.analogInputEvent.InputNumber.Value, arg, this.analogInputEvent.LowValue.Value);
				stringBuilder.AppendFormat("RESET = (AnaIn{0} {1} {2})", this.analogInputEvent.InputNumber.Value, arg2, this.analogInputEvent.LowValue.Value + this.analogInputEvent.Tolerance.Value);
				break;
			}
			case CondRelation.GreaterThanOrEqual:
			{
				string arg3;
				if (!LTLUtil.GetLTLCompareOperatorString(CondRelation.GreaterThanOrEqual, out arg3))
				{
					return LTLGenerator.LTLGenerationResult.TriggerError;
				}
				string arg4;
				if (!LTLUtil.GetLTLCompareOperatorString(CondRelation.LessThan, out arg4))
				{
					return LTLGenerator.LTLGenerationResult.TriggerError;
				}
				stringBuilder.AppendFormat("SET = (AnaIn{0} {1} {2}) ", this.analogInputEvent.InputNumber.Value, arg3, this.analogInputEvent.LowValue.Value);
				stringBuilder.AppendFormat("RESET = (AnaIn{0} {1} {2})", this.analogInputEvent.InputNumber.Value, arg4, this.analogInputEvent.LowValue.Value - this.analogInputEvent.Tolerance.Value);
				break;
			}
			case CondRelation.InRange:
			{
				stringBuilder.AppendFormat(" SET = (AnaIn{0} >= {1:D} AND AnaIn{0} <= {2:D}) ", this.analogInputEvent.InputNumber.Value, this.analogInputEvent.LowValue.Value, this.analogInputEvent.HighValue.Value);
				uint num;
				if (this.analogInputEvent.LowValue.Value > this.analogInputEvent.Tolerance.Value)
				{
					num = this.analogInputEvent.LowValue.Value - this.analogInputEvent.Tolerance.Value;
				}
				else
				{
					num = 0u;
				}
				uint num2 = this.analogInputEvent.HighValue.Value + this.analogInputEvent.Tolerance.Value;
				stringBuilder.AppendFormat(" RESET = (AnaIn{0} < {1:D}) OR (AnaIn{0} > {2:D})", this.analogInputEvent.InputNumber.Value, num, num2);
				break;
			}
			case CondRelation.NotInRange:
				stringBuilder.AppendFormat(" SET = (AnaIn{0} < {1:D}) OR (AnaIn{0} > {2:D})", this.analogInputEvent.InputNumber.Value, this.analogInputEvent.LowValue.Value, this.analogInputEvent.HighValue.Value);
				stringBuilder.AppendFormat(" RESET = (AnaIn{0} >= {1:D} AND AnaIn{0} <= {2:D})", this.analogInputEvent.InputNumber.Value, this.analogInputEvent.LowValue.Value + this.analogInputEvent.Tolerance.Value, this.analogInputEvent.HighValue.Value - this.analogInputEvent.Tolerance.Value);
				break;
			default:
				return LTLGenerator.LTLGenerationResult.TriggerError;
			}
			base.PreEventCode = string.Format("FLAG {0} {1}", this.analogConditionFlagName, stringBuilder);
			base.TriggerEvent = string.Format("SET ({0})", this.analogConditionFlagName);
			base.WhenCondition = null;
			base.AdditionalCodeBeforeFlagSet = null;
			base.AdditionalCodeAfterFlagSet = null;
			base.PostEventCode = null;
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		public override LTLGenerator.LTLGenerationResult GetCurrentStateFlag(out string ltlVariable)
		{
			ltlVariable = this.analogConditionFlagName;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			return string.Format("Event on Analog Input {0}", this.analogInputEvent.InputNumber.Value);
		}
	}
}
