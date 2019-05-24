using System;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLCyclicEventCode : LTLGenericEventCode
	{
		private CyclicTimerEvent cyclicTimerEvent;

		public LTLCyclicEventCode(string eventFlagName, CyclicTimerEvent ev) : base(eventFlagName)
		{
			this.cyclicTimerEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			bool flag = this.cyclicTimerEvent.DelayCycles.Value > 0u;
			switch (this.cyclicTimerEvent.TimeUnit.Value)
			{
			case TimeUnit.MilliSec:
			{
				uint value = this.cyclicTimerEvent.DelayCycles.Value;
				base.TriggerEvent = string.Format("CYCLE ({0})", this.cyclicTimerEvent.Interval.Value);
				if (flag)
				{
					base.PreEventCode = string.Format("VAR {0} = FREE[8] INIT = {1}", this.GetLtlDelayVariableName(), value);
					base.WhenCondition = string.Format("{0} = 0", this.GetLtlDelayVariableName());
					base.AdditionalCodeAfterFlagSet = string.Format("  CALC {0} = ({0} - 1) WHEN ({0} > 0)", this.GetLtlDelayVariableName());
				}
				break;
			}
			case TimeUnit.Sec:
			case TimeUnit.Min:
			{
				uint num = 1000u;
				if (this.cyclicTimerEvent.TimeUnit.Value == TimeUnit.Min)
				{
					num = 60000u;
				}
				StringBuilder stringBuilder = new StringBuilder();
				uint value = this.cyclicTimerEvent.DelayCycles.Value;
				if (flag)
				{
					stringBuilder.AppendFormat("VAR {0} = FREE[8] INIT = {1}", this.GetLtlDelayVariableName(), value);
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendFormat("VAR {0} = FREE[16] INIT = 1", this.GetLtlLongCycleCounterName());
				base.PreEventCode = stringBuilder.ToString();
				base.TriggerEvent = string.Format("CYCLE ({0})", num);
				base.AdditionalCodeBeforeFlagSet = string.Format("  CALC {0} = ({0} - 1)", this.GetLtlLongCycleCounterName());
				base.WhenCondition = string.Format("{0} = 0{1}", this.GetLtlLongCycleCounterName(), flag ? string.Format(" AND {0} = 0", this.GetLtlDelayVariableName()) : "");
				StringBuilder stringBuilder2 = new StringBuilder();
				if (flag)
				{
					stringBuilder2.AppendFormat("  CALC {0} = ({0} - 1) WHEN ({1} = 0 AND {0} > 0)", this.GetLtlDelayVariableName(), this.GetLtlLongCycleCounterName());
					stringBuilder2.AppendLine();
				}
				stringBuilder2.AppendFormat("  CALC {0} = ({1}) WHEN ({0} = 0)", this.GetLtlLongCycleCounterName(), this.cyclicTimerEvent.Interval.Value);
				base.AdditionalCodeAfterFlagSet = stringBuilder2.ToString();
				break;
			}
			default:
				ltlCode = string.Empty;
				return LTLGenerator.LTLGenerationResult.DiagError;
			}
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private string GetLtlDelayVariableName()
		{
			return string.Format("{0}_delayCyclesAfterStart", base.GetEventFlagNameWithoutDot());
		}

		private string GetLtlLongCycleCounterName()
		{
			return string.Format("{0}_longCycleCounter", base.GetEventFlagNameWithoutDot());
		}

		protected override string GetComment()
		{
			return string.Format("Event on cyclic timer, every {0} {1}, {2} cycles delay after start", this.cyclicTimerEvent.Interval.Value.ToString().ToLower(), this.cyclicTimerEvent.TimeUnit.Value, this.cyclicTimerEvent.DelayCycles.Value);
		}
	}
}
