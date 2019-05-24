using System;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLIDEventCode : LTLGenericEventCode
	{
		private IdEvent idEvent;

		public LTLIDEventCode(string eventFlagName, IdEvent ev) : base(eventFlagName)
		{
			this.idEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			ltlCode = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			uint frBaseCycle = 0u;
			uint frRepetition = 1u;
			BusType busType;
			if (this.idEvent is CANIdEvent)
			{
				busType = BusType.Bt_CAN;
				CANIdEvent cANIdEvent = this.idEvent as CANIdEvent;
				flag = cANIdEvent.IsExtendedId.Value;
			}
			else if (this.idEvent is LINIdEvent)
			{
				busType = BusType.Bt_LIN;
				if (flag)
				{
					return LTLGenerator.LTLGenerationResult.TriggerError;
				}
			}
			else
			{
				if (!(this.idEvent is FlexrayIdEvent))
				{
					return LTLGenerator.LTLGenerationResult.TriggerError;
				}
				busType = BusType.Bt_FlexRay;
				FlexrayIdEvent flexrayIdEvent = this.idEvent as FlexrayIdEvent;
				frBaseCycle = flexrayIdEvent.BaseCycle.Value;
				frRepetition = flexrayIdEvent.CycleRepetition.Value;
			}
			uint highestID = LTLUtil.GetHighestID(busType, flag);
			switch (this.idEvent.IdRelation.Value)
			{
			case CondRelation.Equal:
				stringBuilder.Append(LTLUtil.GetIdString(busType, this.idEvent.ChannelNumber.Value, flag, this.idEvent.LowId.Value, frBaseCycle, frRepetition, false, 0u));
				break;
			case CondRelation.NotEqual:
				if (this.idEvent.LowId.Value != 0u)
				{
					stringBuilder.Append(LTLUtil.GetIdString(busType, this.idEvent.ChannelNumber.Value, flag, 0u, frBaseCycle, frRepetition, true, this.idEvent.LowId.Value - 1u));
				}
				if (this.idEvent.LowId.Value != LTLUtil.GetHighestID(busType, flag))
				{
					stringBuilder.Append("  ");
					stringBuilder.Append(LTLUtil.GetIdString(busType, this.idEvent.ChannelNumber.Value, flag, this.idEvent.LowId.Value + 1u, frBaseCycle, frRepetition, true, highestID));
				}
				break;
			case CondRelation.LessThan:
				if (this.idEvent.LowId.Value == 0u)
				{
					base.IsNeverActiveTrigger = true;
					return LTLGenerator.LTLGenerationResult.OK;
				}
				stringBuilder.Append(LTLUtil.GetIdString(busType, this.idEvent.ChannelNumber.Value, flag, 0u, frBaseCycle, frRepetition, true, this.idEvent.LowId.Value - 1u));
				break;
			case CondRelation.LessThanOrEqual:
				stringBuilder.Append(LTLUtil.GetIdString(busType, this.idEvent.ChannelNumber.Value, flag, 0u, frBaseCycle, frRepetition, true, this.idEvent.LowId.Value));
				break;
			case CondRelation.GreaterThan:
				if (this.idEvent.LowId.Value == LTLUtil.GetHighestID(busType, flag))
				{
					base.IsNeverActiveTrigger = true;
					return LTLGenerator.LTLGenerationResult.OK;
				}
				stringBuilder.Append(LTLUtil.GetIdString(busType, this.idEvent.ChannelNumber.Value, flag, this.idEvent.LowId.Value + 1u, frBaseCycle, frRepetition, true, highestID));
				break;
			case CondRelation.GreaterThanOrEqual:
				stringBuilder.Append(LTLUtil.GetIdString(busType, this.idEvent.ChannelNumber.Value, flag, this.idEvent.LowId.Value, frBaseCycle, frRepetition, true, highestID));
				break;
			case CondRelation.InRange:
				stringBuilder.Append(LTLUtil.GetIdString(busType, this.idEvent.ChannelNumber.Value, flag, this.idEvent.LowId.Value, frBaseCycle, frRepetition, true, this.idEvent.HighId.Value));
				break;
			case CondRelation.NotInRange:
				if (this.idEvent.LowId.Value == 0u && this.idEvent.HighId.Value == LTLUtil.GetHighestID(busType, flag))
				{
					base.IsNeverActiveTrigger = true;
					return LTLGenerator.LTLGenerationResult.OK;
				}
				if (this.idEvent.LowId.Value != 0u)
				{
					stringBuilder.Append(LTLUtil.GetIdString(busType, this.idEvent.ChannelNumber.Value, flag, 0u, frBaseCycle, frRepetition, true, this.idEvent.LowId.Value - 1u));
				}
				if (this.idEvent.HighId.Value != LTLUtil.GetHighestID(busType, flag))
				{
					stringBuilder.Append("  ");
					stringBuilder.Append(LTLUtil.GetIdString(busType, this.idEvent.ChannelNumber.Value, flag, this.idEvent.HighId.Value + 1u, frBaseCycle, frRepetition, true, highestID));
				}
				break;
			default:
				return LTLGenerator.LTLGenerationResult.TriggerError;
			}
			base.PreEventCode = null;
			base.TriggerEvent = string.Format("RECEIVE ({0})", stringBuilder);
			base.WhenCondition = null;
			base.AdditionalCodeBeforeFlagSet = null;
			base.AdditionalCodeAfterFlagSet = null;
			base.PostEventCode = null;
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			string arg;
			if (this.idEvent is CANIdEvent)
			{
				arg = string.Format("CAN ID {0}", LTLIDEventCode.GetCommentCompareOperatorStringForIDs(this.idEvent.IdRelation.Value, this.idEvent.LowId.Value, this.idEvent.HighId.Value, true));
			}
			else if (this.idEvent is LINIdEvent)
			{
				arg = string.Format("LIN ID {0}", LTLIDEventCode.GetCommentCompareOperatorStringForIDs(this.idEvent.IdRelation.Value, this.idEvent.LowId.Value, this.idEvent.HighId.Value, true));
			}
			else if (this.idEvent is FlexrayIdEvent)
			{
				arg = string.Format("FR ID {0}", LTLIDEventCode.GetCommentCompareOperatorStringForIDs(this.idEvent.IdRelation.Value, this.idEvent.LowId.Value, this.idEvent.HighId.Value, false));
			}
			else
			{
				arg = "Unknown bus or event type";
			}
			return string.Format("Event on {0}", arg);
		}

		private static string GetCommentCompareOperatorStringForIDs(CondRelation condRelation, uint lowValue, uint highValue, bool hexValues)
		{
			string arg;
			string arg2;
			if (hexValues)
			{
				arg = string.Format("0x{0:X}", lowValue);
				arg2 = string.Format("0x{0:X}", highValue);
			}
			else
			{
				arg = string.Format("{0:D}", lowValue);
				arg2 = string.Format("{0:D}", highValue);
			}
			switch (condRelation)
			{
			case CondRelation.Equal:
				return string.Format("= {0}", arg);
			case CondRelation.NotEqual:
				return string.Format("!= {0}", arg);
			case CondRelation.LessThan:
				return string.Format("< {0}", arg);
			case CondRelation.LessThanOrEqual:
				return string.Format("<= {0}", arg);
			case CondRelation.GreaterThan:
				return string.Format("> {0}", arg);
			case CondRelation.GreaterThanOrEqual:
				return string.Format(">= {0}", arg);
			case CondRelation.InRange:
				return string.Format("IN RANGE [{0}; {1}]", arg, arg2);
			case CondRelation.NotInRange:
				return string.Format("OUT OF RANGE [{0}; {1}]", arg, arg2);
			default:
				return "no compare";
			}
		}
	}
}
