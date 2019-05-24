using System;
using System.Collections.Generic;
using System.Text;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LTLGenerationCS.Utility;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public abstract class LTLSignalEventCode : LTLGenericEventCode
	{
		protected struct TriggerDefinition
		{
			public SignalDefinition signalDefinition;

			public Dictionary<double, string> textEncValueTable;

			public IList<SignalDefinition> frSignalList;

			public BusType busType;

			public uint channelNumber;

			public CondRelation relation;

			public double lowValue;

			public double highValue;
		}

		protected LTLSignalEventCode.TriggerDefinition trigger;

		public LTLSignalEventCode(string eventFlagName) : base(eventFlagName)
		{
			this.trigger.busType = BusType.Bt_None;
			this.trigger.channelNumber = 0u;
			this.trigger.signalDefinition = null;
		}

		public abstract LTLGenerator.LTLGenerationResult GenerateLtlSignalDefinition(out string ltlSignalDefinition, SignalDefinition signalDefinition, Dictionary<double, string> textEncValueTable, BusType busType, uint channelNumber, string signalBaseName, long initValue, IList<SignalDefinition> frSignalList, bool doAddMetaData, bool doAddSignalPostfixToName, out string multiplexorCondition);

		public abstract string GenerateLtlTriggerEventDefinition(bool is32BitSignal, string eventFlagNameWithoutDot);

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			ltlCode = string.Empty;
			string eventFlagNameWithoutDot = base.GetEventFlagNameWithoutDot();
			if (this.trigger.signalDefinition == null)
			{
				return LTLGenerator.LTLGenerationResult.TriggerError;
			}
			string text;
			long signalInitValue = LTLSignalEventCode.GetSignalInitValue(this.trigger.relation, this.trigger.lowValue, this.trigger.highValue, this.trigger.signalDefinition.Length, this.trigger.signalDefinition.IsSigned, out text);
			string value = "";
			if (this.trigger.signalDefinition.Length > 32u)
			{
				return LTLGenerator.LTLGenerationResult.TriggerError_Bitlength;
			}
			string str;
			LTLGenerator.LTLGenerationResult result;
			if ((result = this.GenerateLtlSignalDefinition(out value, this.trigger.signalDefinition, this.trigger.textEncValueTable, this.trigger.busType, this.trigger.channelNumber, eventFlagNameWithoutDot, signalInitValue, this.trigger.frSignalList, false, true, out str)) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(value);
			if (text.Length > 0)
			{
				stringBuilder.AppendLine(text);
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			if (this.trigger.relation == CondRelation.OnChange)
			{
				if (!this.Is32BitSignal())
				{
					stringBuilder.AppendFormat("VAR {0}_LastValue = FREE[{1:D}] {2}", eventFlagNameWithoutDot, this.trigger.signalDefinition.Length, this.trigger.signalDefinition.IsSigned ? "SIGNED" : "");
					stringBuilder.AppendLine();
					base.WhenCondition = string.Format("{0}_Signal <> {0}_LastValue", eventFlagNameWithoutDot);
					stringBuilder2.AppendFormat("  CALC {0}_LastValue = ({0}_Signal)", eventFlagNameWithoutDot);
				}
				else
				{
					uint highPartLength = LTLUtil.GetHighPartLength(this.trigger.signalDefinition);
					uint lowPartLength = LTLUtil.GetLowPartLength(this.trigger.signalDefinition);
					stringBuilder.AppendFormat("VAR {0}_LastValue.HIGH = FREE[{1:D}] {2}", eventFlagNameWithoutDot, (highPartLength > 1u) ? highPartLength : 2u, this.trigger.signalDefinition.IsSigned ? "SIGNED" : "");
					stringBuilder.AppendLine();
					stringBuilder.AppendFormat("VAR {0}_LastValue.LOW  = FREE[{1:D}] ", eventFlagNameWithoutDot, lowPartLength);
					stringBuilder.AppendLine();
					StringBuilder stringBuilder3 = new StringBuilder();
					stringBuilder3.AppendLine("  VAR  signalHasChanged = FREE[1]");
					OrList orList = new OrList();
					orList.Add(string.Format("{0}_Signal.HIGH <> {0}_LastValue.HIGH", eventFlagNameWithoutDot));
					orList.Add(string.Format("{0}_Signal.LOW  <> {0}_LastValue.LOW", eventFlagNameWithoutDot));
					stringBuilder3.AppendFormat("  CALC signalHasChanged = {0}", orList.ToLTLCode());
					base.AdditionalCodeBeforeFlagSet = stringBuilder3.ToString();
					base.WhenCondition = "signalHasChanged";
					stringBuilder2.AppendFormat("  CALC {0}_LastValue.HIGH = ({0}_Signal.HIGH)", eventFlagNameWithoutDot);
					stringBuilder2.AppendFormat("  CALC {0}_LastValue.LOW  = ({0}_Signal.LOW)", eventFlagNameWithoutDot);
				}
			}
			else
			{
				stringBuilder.AppendFormat("VAR {0}_LastState = FREE[1]", eventFlagNameWithoutDot);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("VAR {0}_State     = FREE[1]", eventFlagNameWithoutDot);
				StringBuilder stringBuilder4 = new StringBuilder();
				stringBuilder4.AppendFormat("  VAR  {0}_CurrentState = FREE[1]", eventFlagNameWithoutDot);
				stringBuilder4.AppendLine();
				ushort num = 0;
				ushort compareValue = 0;
				int num2 = 0;
				int compareValue2 = 0;
				try
				{
					bool flag = this.trigger.signalDefinition.Length == 17u && this.trigger.signalDefinition.IsSigned;
					num = LTLUtil.LowPart(this.trigger.lowValue, flag);
					compareValue = LTLUtil.LowPart(this.trigger.highValue, flag);
					num2 = LTLUtil.HighPart(this.trigger.lowValue, flag);
					compareValue2 = LTLUtil.HighPart(this.trigger.highValue, flag);
				}
				catch
				{
					return LTLGenerator.LTLGenerationResult.TriggerError;
				}
				if (!this.Is32BitSignal())
				{
					switch (this.trigger.relation)
					{
					case CondRelation.Equal:
					case CondRelation.NotEqual:
					case CondRelation.LessThan:
					case CondRelation.LessThanOrEqual:
					case CondRelation.GreaterThan:
					case CondRelation.GreaterThanOrEqual:
					{
						string arg;
						if (!LTLUtil.GetLTLCompareOperatorString(this.trigger.relation, out arg))
						{
							return LTLGenerator.LTLGenerationResult.TriggerError;
						}
						stringBuilder4.AppendFormat("  CALC {0}_CurrentState = ({0}_Signal {1} {2})", eventFlagNameWithoutDot, arg, (int)this.trigger.lowValue);
						break;
					}
					case CondRelation.InRange:
					{
						string arg2 = string.Format("{0}_Signal >= {1:D}", eventFlagNameWithoutDot, (int)this.trigger.lowValue);
						string arg3 = string.Format("{0}_Signal <= {1:D}", eventFlagNameWithoutDot, (int)this.trigger.highValue);
						stringBuilder4.AppendFormat("  CALC {0}_CurrentState = ({1} AND {2})", eventFlagNameWithoutDot, arg2, arg3);
						break;
					}
					case CondRelation.NotInRange:
					{
						string text2;
						if (this.trigger.lowValue <= 0.0 && !this.trigger.signalDefinition.IsSigned)
						{
							text2 = string.Empty;
						}
						else
						{
							text2 = string.Format("{0}_Signal < {1:D}", eventFlagNameWithoutDot, (int)this.trigger.lowValue);
						}
						string text3 = string.Format("{0}_Signal > {1:D}", eventFlagNameWithoutDot, (int)this.trigger.highValue);
						stringBuilder4.AppendFormat("  CALC {0}_CurrentState = ({1}{2}{3})", new object[]
						{
							eventFlagNameWithoutDot,
							text2,
							(text2.Length > 0) ? ") OR (" : "",
							text3
						});
						break;
					}
					default:
						return LTLGenerator.LTLGenerationResult.TriggerError;
					}
				}
				else
				{
					switch (this.trigger.relation)
					{
					case CondRelation.Equal:
						stringBuilder4.AppendFormat("  VAR  match = FREE[1]", new object[0]);
						stringBuilder4.AppendLine();
						stringBuilder4.AppendFormat("  CALC match = ({0}_Signal.HIGH = {1} AND {0}_Signal.LOW = {2}) ", eventFlagNameWithoutDot, num2, num);
						stringBuilder4.AppendLine();
						stringBuilder4.AppendFormat("  CALC {0}_CurrentState = (match)", eventFlagNameWithoutDot);
						break;
					case CondRelation.NotEqual:
						stringBuilder4.AppendFormat("  VAR  match = FREE[1]", new object[0]);
						stringBuilder4.AppendLine();
						stringBuilder4.AppendFormat("  CALC match = ({0}_Signal.HIGH = {1} AND {0}_Signal.LOW = {2}) ", eventFlagNameWithoutDot, num2, num);
						stringBuilder4.AppendLine();
						stringBuilder4.AppendFormat("  CALC {0}_CurrentState = (NOT match)", eventFlagNameWithoutDot);
						break;
					case CondRelation.LessThan:
					case CondRelation.LessThanOrEqual:
					case CondRelation.GreaterThan:
					case CondRelation.GreaterThanOrEqual:
					{
						string text4;
						if (!LTLUtil.GetLTLCompareOperatorString(this.trigger.relation, out text4))
						{
							return LTLGenerator.LTLGenerationResult.TriggerError;
						}
						CondRelation condRelation = this.trigger.relation;
						if (this.trigger.relation == CondRelation.GreaterThanOrEqual)
						{
							condRelation = CondRelation.GreaterThan;
						}
						if (this.trigger.relation == CondRelation.LessThanOrEqual)
						{
							condRelation = CondRelation.LessThan;
						}
						string text5;
						if (!LTLUtil.GetLTLCompareOperatorString(condRelation, out text5))
						{
							return LTLGenerator.LTLGenerationResult.TriggerError;
						}
						Variable variable = new Variable(string.Format("{0}_Signal.HIGH", eventFlagNameWithoutDot), (byte)LTLUtil.GetHighPartLength(this.trigger.signalDefinition), this.trigger.signalDefinition.IsSigned);
						Variable variable2 = new Variable(string.Format("{0}_Signal.LOW", eventFlagNameWithoutDot), (byte)LTLUtil.GetLowPartLength(this.trigger.signalDefinition), false);
						AndList andList = new AndList();
						LogicalCondition logicalCondition = new LogicalCondition(variable, condRelation, num2);
						andList.Add(logicalCondition);
						AndList andList2 = new AndList();
						LogicalCondition logicalCondition2 = new LogicalCondition(variable, CondRelation.Equal, num2);
						andList2.Add(logicalCondition2);
						LogicalCondition logicalCondition3 = new LogicalCondition(variable2, this.trigger.relation, Convert.ToInt32(num));
						andList2.Add(logicalCondition3);
						stringBuilder4.AppendFormat("  VAR  match = FREE[1]", new object[0]);
						stringBuilder4.AppendLine();
						stringBuilder4.AppendFormat("  CALC match =    {0}", andList.ToLTLCode());
						stringBuilder4.AppendLine();
						stringBuilder4.AppendFormat("               OR {0}", andList2.ToLTLCode());
						stringBuilder4.AppendLine();
						stringBuilder4.AppendFormat("  CALC {0}_CurrentState = (match)", eventFlagNameWithoutDot);
						new OrList();
						break;
					}
					case CondRelation.InRange:
					case CondRelation.NotInRange:
					{
						Variable variable3 = new Variable(string.Format("{0}_Signal.HIGH", eventFlagNameWithoutDot), (byte)LTLUtil.GetLowPartLength(this.trigger.signalDefinition), this.trigger.signalDefinition.IsSigned);
						Variable variable4 = new Variable(string.Format("{0}_Signal.LOW", eventFlagNameWithoutDot), (byte)LTLUtil.GetLowPartLength(this.trigger.signalDefinition), false);
						AndList andList3 = new AndList();
						LogicalCondition logicalCondition4 = new LogicalCondition(variable3, CondRelation.LessThan, num2);
						andList3.Add(logicalCondition4);
						AndList andList4 = new AndList();
						LogicalCondition logicalCondition5 = new LogicalCondition(variable3, CondRelation.Equal, num2);
						andList4.Add(logicalCondition5);
						LogicalCondition logicalCondition6 = new LogicalCondition(variable4, CondRelation.LessThan, (int)num);
						andList4.Add(logicalCondition6);
						AndList andList5 = new AndList();
						LogicalCondition logicalCondition7 = new LogicalCondition(variable3, CondRelation.Equal, compareValue2);
						andList5.Add(logicalCondition7);
						LogicalCondition logicalCondition8 = new LogicalCondition(variable4, CondRelation.GreaterThan, (int)compareValue);
						andList5.Add(logicalCondition8);
						AndList andList6 = new AndList();
						LogicalCondition logicalCondition9 = new LogicalCondition(variable3, CondRelation.GreaterThan, compareValue2);
						andList6.Add(logicalCondition9);
						stringBuilder4.AppendFormat("  VAR  mismatch = FREE[1]", new object[0]);
						stringBuilder4.AppendLine();
						stringBuilder4.AppendFormat("  CALC mismatch =    {0} ", andList3.ToLTLCode());
						stringBuilder4.AppendLine();
						stringBuilder4.AppendFormat("                  OR {0} ", andList4.ToLTLCode());
						stringBuilder4.AppendLine();
						stringBuilder4.AppendFormat("                  OR {0} ", andList5.ToLTLCode());
						stringBuilder4.AppendLine();
						stringBuilder4.AppendFormat("                  OR {0}", andList6.ToLTLCode());
						stringBuilder4.AppendLine();
						if (this.trigger.relation == CondRelation.InRange)
						{
							stringBuilder4.AppendFormat("  CALC {0}_CurrentState = (NOT mismatch)", eventFlagNameWithoutDot);
						}
						else
						{
							stringBuilder4.AppendFormat("  CALC {0}_CurrentState = (mismatch)", eventFlagNameWithoutDot);
						}
						break;
					}
					default:
						return LTLGenerator.LTLGenerationResult.TriggerError;
					}
				}
				stringBuilder4.AppendLine();
				stringBuilder4.AppendFormat("  CALC {0}_State = ({0}_CurrentState)", eventFlagNameWithoutDot);
				base.AdditionalCodeBeforeFlagSet = stringBuilder4.ToString();
				base.WhenCondition = string.Format("{0}_CurrentState AND NOT {0}_LastState {1}", eventFlagNameWithoutDot, this.trigger.signalDefinition.IsMultiplexed ? ("AND " + str) : "");
				stringBuilder2.AppendFormat("  CALC {0}_LastState = ({0}_CurrentState) {1}", eventFlagNameWithoutDot, this.trigger.signalDefinition.IsMultiplexed ? (" WHEN (" + str + ")") : "");
			}
			base.PreEventCode = stringBuilder.ToString();
			base.TriggerEvent = this.GenerateLtlTriggerEventDefinition(this.Is32BitSignal(), eventFlagNameWithoutDot);
			base.AdditionalCodeAfterFlagSet = stringBuilder2.ToString();
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		public override LTLGenerator.LTLGenerationResult GetCurrentStateFlag(out string ltlVariable)
		{
			ltlVariable = string.Format("{0}_State", base.GetEventFlagNameWithoutDot());
			if (this.trigger.relation == CondRelation.OnChange)
			{
				return LTLGenerator.LTLGenerationResult.EventInWrongConext;
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private static long GetSignalInitValue(CondRelation relation, double lowValue, double highValue, uint signalLength, bool isSigned, out string errorComment)
		{
			long num = Convert.ToInt64(lowValue);
			long num2 = Convert.ToInt64(highValue);
			long result = 0L;
			errorComment = "";
			long num3 = 1L;
			long num4 = 0L;
			for (uint num5 = 1u; num5 <= signalLength; num5 += 1u)
			{
				num3 *= 2L;
			}
			if (isSigned)
			{
				num3 /= 2L;
				num4 = -num3;
			}
			num3 -= 1L;
			switch (relation)
			{
			case CondRelation.Equal:
				if (num == 0L)
				{
					result = 1L;
				}
				break;
			case CondRelation.NotEqual:
				result = num;
				break;
			case CondRelation.LessThan:
			case CondRelation.LessThanOrEqual:
			{
				long num6 = num;
				if (relation == CondRelation.LessThanOrEqual)
				{
					num6 = num + 1L;
				}
				if (num6 > 0L)
				{
					if (num6 > num3)
					{
						errorComment = string.Format("{{INIT = {0} not allowed due to bitlength, would be greater then maximum value! - leaving INIT=0}}", num6);
					}
					else
					{
						result = num6;
					}
				}
				break;
			}
			case CondRelation.GreaterThan:
			case CondRelation.GreaterThanOrEqual:
			{
				long num7 = num;
				if (relation == CondRelation.GreaterThanOrEqual)
				{
					num7 = num - 1L;
				}
				if (num7 < 0L)
				{
					if (!isSigned)
					{
						errorComment = string.Format("{{INIT = {0} not allowed for unsigned values! - leaving INIT=0}}", num7);
					}
					else if (num7 < num4)
					{
						errorComment = string.Format("{{INIT = {0} not allowed due to bitlength, would be less then minimum value! - leaving INIT=0}}", num7);
					}
					else
					{
						result = num7;
					}
				}
				break;
			}
			case CondRelation.InRange:
				if (num <= 0L && 0L <= num2)
				{
					if (num2 + 1L <= num3)
					{
						result = num2 + 1L;
					}
					else if (num - 1L >= num4)
					{
						result = num - 1L;
					}
					else
					{
						errorComment = string.Format("{{neither INIT = {0} not INIT = {1} would be allowed because it doesn't fit in bitlength. Range over full bitlength? Leaving INIT=0}}", num2 + 1L, num - 1L);
					}
				}
				break;
			case CondRelation.NotInRange:
				if (num > 0L || num2 < 0L)
				{
					result = num;
				}
				break;
			case CondRelation.OnChange:
				break;
			default:
				errorComment = string.Format("{{unknown relation}}", new object[0]);
				break;
			}
			return result;
		}

		private bool Is32BitSignal()
		{
			return this.trigger.signalDefinition.Length > 16u;
		}
	}
}
