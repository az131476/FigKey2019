using System;
using System.Globalization;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpTriggerConditionLinData : DpTriggerCondition
	{
		private readonly LINDataEvent mEvent;

		public override string ChannelString
		{
			get
			{
				return this.mEvent.ChannelNumber.Value.ToString(CultureInfo.InvariantCulture);
			}
		}

		public override string NameOfCaplEventHandler
		{
			get
			{
				return CaplHelper.MakeLinMsgHandlerString(this.ChannelString);
			}
		}

		public override string TemplateVariables
		{
			get
			{
				if (this.mEvent.Relation.Value != CondRelation.OnChange)
				{
					return "TVariables_Condition";
				}
				return "TVariables_Condition_Data_OnChange";
			}
		}

		public override string TemplateEventHandler
		{
			get
			{
				if (this.mEvent.Relation.Value != CondRelation.OnChange)
				{
					return "TEventHandler_Condition_Data";
				}
				return "TEventHandler_Condition_Data_OnChange";
			}
		}

		public string ValueConditionString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.mEvent.Relation.Value == CondRelation.OnChange)
				{
					if (this.mEvent.RawDataSignal is RawDataSignalByte)
					{
						RawDataSignalByte rawDataSignalByte = this.mEvent.RawDataSignal as RawDataSignalByte;
						string variableString = string.Format("this.Byte({0})", rawDataSignalByte.DataBytePos);
						stringBuilder.Append(CaplHelper.MakeConditionString(this.mEvent.Relation.Value, variableString, this.mEvent.LowValue.Value.ToString(CultureInfo.InvariantCulture), this.mEvent.HighValue.Value.ToString(CultureInfo.InvariantCulture)));
					}
					else if (this.mEvent.RawDataSignal is RawDataSignalStartbitLength)
					{
						RawDataSignalStartbitLength rawDataSignalStartbitLength = this.mEvent.RawDataSignal as RawDataSignalStartbitLength;
						string variableString2 = string.Format("GetBits(this.QWord(0), {0}, {1})", rawDataSignalStartbitLength.StartbitPos, rawDataSignalStartbitLength.Length);
						stringBuilder.Append(CaplHelper.MakeConditionString(this.mEvent.Relation.Value, variableString2, this.mEvent.LowValue.Value.ToString(CultureInfo.InvariantCulture), this.mEvent.HighValue.Value.ToString(CultureInfo.InvariantCulture)));
					}
					else
					{
						stringBuilder.Append("UNKNOWN RawDataSignal TYPE");
					}
				}
				else if (this.mEvent.RawDataSignal is RawDataSignalByte)
				{
					RawDataSignalByte rawDataSignalByte2 = this.mEvent.RawDataSignal as RawDataSignalByte;
					stringBuilder.Append("((this.DLC>");
					stringBuilder.Append(rawDataSignalByte2.DataBytePos);
					stringBuilder.Append(") && ");
					string variableString3 = string.Format("this.Byte({0})", rawDataSignalByte2.DataBytePos);
					stringBuilder.Append(CaplHelper.MakeConditionString(this.mEvent.Relation.Value, variableString3, this.mEvent.LowValue.Value.ToString(CultureInfo.InvariantCulture), this.mEvent.HighValue.Value.ToString(CultureInfo.InvariantCulture)));
					stringBuilder.Append(")");
				}
				else if (this.mEvent.RawDataSignal is RawDataSignalStartbitLength)
				{
					RawDataSignalStartbitLength rawDataSignalStartbitLength2 = this.mEvent.RawDataSignal as RawDataSignalStartbitLength;
					stringBuilder.Append("(((this.DLC*8)>=");
					stringBuilder.Append(rawDataSignalStartbitLength2.StartbitPos.Value + rawDataSignalStartbitLength2.Length.Value);
					stringBuilder.Append(") && ");
					string variableString4 = string.Format("GetBits(this.QWord(0), {0}, {1})", rawDataSignalStartbitLength2.StartbitPos, rawDataSignalStartbitLength2.Length);
					stringBuilder.Append(CaplHelper.MakeConditionString(this.mEvent.Relation.Value, variableString4, this.mEvent.LowValue.Value.ToString(CultureInfo.InvariantCulture), this.mEvent.HighValue.Value.ToString(CultureInfo.InvariantCulture)));
					stringBuilder.Append(")");
				}
				else
				{
					stringBuilder.Append("UNKNOWN RawDataSignal TYPE");
				}
				return stringBuilder.ToString();
			}
		}

		public string ValueConditionCheck
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this.mEvent.Relation.Value == CondRelation.OnChange)
				{
					if (this.mEvent.RawDataSignal is RawDataSignalByte)
					{
						RawDataSignalByte rawDataSignalByte = this.mEvent.RawDataSignal as RawDataSignalByte;
						stringBuilder.Append("(this.DLC>");
						stringBuilder.Append(rawDataSignalByte.DataBytePos);
						stringBuilder.Append(")");
					}
					else if (this.mEvent.RawDataSignal is RawDataSignalStartbitLength)
					{
						RawDataSignalStartbitLength rawDataSignalStartbitLength = this.mEvent.RawDataSignal as RawDataSignalStartbitLength;
						stringBuilder.Append("((this.DLC*8)>=");
						stringBuilder.Append(rawDataSignalStartbitLength.StartbitPos.Value + rawDataSignalStartbitLength.Length.Value);
						stringBuilder.Append(")");
					}
					else
					{
						stringBuilder.Append("UNKNOWN RawDataSignal TYPE");
					}
				}
				return stringBuilder.ToString();
			}
		}

		public string MsgConditionString
		{
			get
			{
				return CaplHelper.MakeConditionStringLinId(CondRelation.Equal, "this.ID", this.mEvent.ID.Value, 0u);
			}
		}

		public DpTriggerConditionLinData(uint triggerIdent, uint conditionIdent, Event conditionEvent) : base(triggerIdent, conditionIdent, conditionEvent)
		{
			this.mEvent = (conditionEvent as LINDataEvent);
		}
	}
}
