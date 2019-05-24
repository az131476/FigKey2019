using System;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLCANBusStatisticsEventCode : LTLGenericEventCode
	{
		private CANBusStatisticsEvent canBusStatEvent;

		public LTLCANBusStatisticsEventCode(string eventFlagName, CANBusStatisticsEvent ev) : base(eventFlagName)
		{
			this.canBusStatEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			ltlCode = string.Empty;
			if (!this.canBusStatEvent.IsBusloadEnabled.Value && !this.canBusStatEvent.IsErrorFramesEnabled.Value)
			{
				return LTLGenerator.LTLGenerationResult.TriggerError;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.canBusStatEvent.IsBusloadEnabled.Value)
			{
				switch (this.canBusStatEvent.BusloadRelation.Value)
				{
				case CondRelation.InRange:
				{
					string value = string.Format("CAN{0:D}BusLoad >= {1:D}", this.canBusStatEvent.ChannelNumber.Value, this.canBusStatEvent.BusloadLow.Value);
					string value2 = string.Format("CAN{0:D}BusLoad <= {1:D}", this.canBusStatEvent.ChannelNumber.Value, this.canBusStatEvent.BusloadHigh.Value);
					if (this.canBusStatEvent.BusloadLow.Value == 0u && this.canBusStatEvent.BusloadHigh.Value >= 100u)
					{
						return LTLGenerator.LTLGenerationResult.TriggerError;
					}
					if (this.canBusStatEvent.BusloadLow.Value > 0u)
					{
						stringBuilder.Append(value);
					}
					if (this.canBusStatEvent.BusloadHigh.Value < 100u)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(" AND ");
						}
						stringBuilder.Append(value2);
					}
					stringBuilder.Insert(0, "(");
					stringBuilder.Append(")");
					break;
				}
				case CondRelation.NotInRange:
				{
					string value3 = string.Format("(CAN{0:D}BusLoad < {1:D})", this.canBusStatEvent.ChannelNumber.Value, this.canBusStatEvent.BusloadLow.Value);
					string value4 = string.Format("(CAN{0:D}BusLoad > {1:D})", this.canBusStatEvent.ChannelNumber.Value, this.canBusStatEvent.BusloadHigh.Value);
					if (this.canBusStatEvent.BusloadLow.Value == 0u && this.canBusStatEvent.BusloadHigh.Value >= 100u)
					{
						return LTLGenerator.LTLGenerationResult.TriggerError;
					}
					if (this.canBusStatEvent.BusloadLow.Value > 0u)
					{
						stringBuilder.Append(value3);
					}
					if (this.canBusStatEvent.BusloadHigh.Value < 100u)
					{
						if (stringBuilder.Length > 0)
						{
							stringBuilder.Append(" OR ");
						}
						stringBuilder.Append(value4);
					}
					break;
				}
				default:
					return LTLGenerator.LTLGenerationResult.TriggerError;
				}
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			if (this.canBusStatEvent.IsErrorFramesEnabled.Value)
			{
				switch (this.canBusStatEvent.ErrorFramesRelation.Value)
				{
				case CondRelation.LessThanOrEqual:
				case CondRelation.GreaterThanOrEqual:
				{
					string arg;
					LTLUtil.GetLTLCompareOperatorString(this.canBusStatEvent.ErrorFramesRelation.Value, out arg);
					stringBuilder2.AppendFormat("{0} {1} {2}", LTLUtil.GetCANErrorrateVariableName(this.canBusStatEvent.ChannelNumber.Value), arg, this.canBusStatEvent.ErrorFramesLow.Value);
					goto IL_31C;
				}
				}
				return LTLGenerator.LTLGenerationResult.TriggerError;
			}
			IL_31C:
			StringBuilder stringBuilder3 = new StringBuilder();
			stringBuilder3.AppendFormat("VAR  {0}_BusStatisticCondition = FREE[1]", this.eventFlagName);
			stringBuilder3.AppendLine();
			if (this.canBusStatEvent.IsBusloadEnabled.Value)
			{
				stringBuilder3.AppendFormat("FLAG {0}_BusloadCondition = {1}", this.eventFlagName, stringBuilder);
				stringBuilder3.AppendLine();
			}
			stringBuilder3.AppendFormat("EVENT ON CYCLE ({0:D}) BEGIN", LTLUtil.BusLoadInterval);
			stringBuilder3.AppendLine();
			string text = ") OR (";
			if (this.canBusStatEvent.IsANDConjunction.Value)
			{
				text = " AND ";
			}
			stringBuilder3.AppendFormat("  CALC {0}_BusStatisticCondition = ({1}{2}{3})", new object[]
			{
				this.eventFlagName,
				this.canBusStatEvent.IsBusloadEnabled.Value ? string.Format("{0}_BusloadCondition", this.eventFlagName) : "",
				(stringBuilder.Length > 0 && stringBuilder2.Length > 0) ? text : "",
				stringBuilder2
			});
			stringBuilder3.AppendLine();
			stringBuilder3.AppendLine("END");
			base.PreEventCode = stringBuilder3.ToString();
			base.TriggerEvent = string.Format("SET ({0}_BusStatisticCondition)", this.eventFlagName);
			base.WhenCondition = null;
			base.AdditionalCodeBeforeFlagSet = null;
			base.AdditionalCodeAfterFlagSet = null;
			base.PostEventCode = null;
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			string text = string.Empty;
			if (this.canBusStatEvent.IsBusloadEnabled.Value)
			{
				text = string.Format("Busload {0} %", LTLGenericEventCode.GetCommentCompareOperatorStringForSignals(this.canBusStatEvent.BusloadRelation.Value, this.canBusStatEvent.BusloadLow.Value, this.canBusStatEvent.BusloadHigh.Value));
			}
			string text2 = string.Empty;
			if (this.canBusStatEvent.IsErrorFramesEnabled.Value)
			{
				text2 = string.Format("Errorframes/s {0} ", LTLGenericEventCode.GetCommentCompareOperatorStringForSignals(this.canBusStatEvent.ErrorFramesRelation.Value, this.canBusStatEvent.ErrorFramesLow.Value, this.canBusStatEvent.ErrorFramesHigh.Value));
			}
			string arg = string.Empty;
			if (text.Length > 0 && text2.Length > 0)
			{
				arg = " OR ";
				if (this.canBusStatEvent.IsANDConjunction.Value)
				{
					arg = " AND ";
				}
			}
			return string.Format("Event on bus statistics: {0} {1} {2}", text, arg, text2);
		}

		public override LTLGenerator.LTLGenerationResult GetCurrentStateFlag(out string ltlVariable)
		{
			ltlVariable = string.Format("{0}_BusStatisticCondition", this.eventFlagName);
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
