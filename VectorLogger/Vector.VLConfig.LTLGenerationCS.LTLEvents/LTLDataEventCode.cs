using System;
using System.Collections.Generic;
using System.Text;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLDataEventCode : LTLSignalEventCode
	{
		private Event dataEvent;

		public LTLDataEventCode(string eventFlagName, Event ev) : base(eventFlagName)
		{
			this.dataEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			if (this.dataEvent is CANDataEvent)
			{
				this.trigger.busType = BusType.Bt_CAN;
				CANDataEvent cANDataEvent = this.dataEvent as CANDataEvent;
				this.trigger.channelNumber = cANDataEvent.ChannelNumber.Value;
				this.trigger.lowValue = cANDataEvent.LowValue.Value;
				this.trigger.highValue = cANDataEvent.HighValue.Value;
				this.trigger.relation = cANDataEvent.Relation.Value;
				MessageDefinition message = new MessageDefinition(cANDataEvent.ID.Value, cANDataEvent.IsExtendedId.Value);
				this.trigger.signalDefinition = new SignalDefinition();
				if (cANDataEvent.RawDataSignal is RawDataSignalByte)
				{
					RawDataSignalByte rawDataSignalByte = cANDataEvent.RawDataSignal as RawDataSignalByte;
					this.trigger.signalDefinition.SetSignal(message, rawDataSignalByte.DataBytePos.Value * 8u, 8u, false, false, true);
				}
				else if (cANDataEvent.RawDataSignal is RawDataSignalStartbitLength)
				{
					RawDataSignalStartbitLength rawDataSignalStartbitLength = cANDataEvent.RawDataSignal as RawDataSignalStartbitLength;
					this.trigger.signalDefinition.SetSignal(message, rawDataSignalStartbitLength.StartbitPos.Value, rawDataSignalStartbitLength.Length.Value, rawDataSignalStartbitLength.IsMotorola.Value, false, true);
				}
			}
			else
			{
				if (!(this.dataEvent is LINDataEvent))
				{
					ltlCode = string.Empty;
					return LTLGenerator.LTLGenerationResult.TriggerError;
				}
				this.trigger.busType = BusType.Bt_LIN;
				LINDataEvent lINDataEvent = this.dataEvent as LINDataEvent;
				this.trigger.channelNumber = lINDataEvent.ChannelNumber.Value;
				this.trigger.lowValue = lINDataEvent.LowValue.Value;
				this.trigger.highValue = lINDataEvent.HighValue.Value;
				this.trigger.relation = lINDataEvent.Relation.Value;
				if (lINDataEvent.RawDataSignal is RawDataSignalByte)
				{
					RawDataSignalByte rawDataSignalByte2 = lINDataEvent.RawDataSignal as RawDataSignalByte;
					this.trigger.signalDefinition = new SignalDefinition(lINDataEvent.ID.Value, rawDataSignalByte2.DataBytePos.Value * 8u, 8u, false, false, true);
				}
				else if (lINDataEvent.RawDataSignal is RawDataSignalStartbitLength)
				{
					RawDataSignalStartbitLength rawDataSignalStartbitLength2 = lINDataEvent.RawDataSignal as RawDataSignalStartbitLength;
					this.trigger.signalDefinition = new SignalDefinition(lINDataEvent.ID.Value, rawDataSignalStartbitLength2.StartbitPos.Value, rawDataSignalStartbitLength2.Length.Value, rawDataSignalStartbitLength2.IsMotorola.Value, false, true);
				}
			}
			return base.GenerateCode(out ltlCode);
		}

		public override LTLGenerator.LTLGenerationResult GenerateLtlSignalDefinition(out string ltlSignalDefinition, SignalDefinition signalDefinition, Dictionary<double, string> textEncValueTable, BusType busType, uint channelNumber, string signalBaseName, long initValue, IList<SignalDefinition> frSignalList, bool doAddMetaData, bool doAddSignalPostfixToName, out string multiplexorCondition)
		{
			return LTLUtil.GenerateLtlSignalDefinition(out ltlSignalDefinition, this.trigger.signalDefinition, this.trigger.textEncValueTable, this.trigger.busType, this.trigger.channelNumber, signalBaseName, initValue, this.trigger.frSignalList, false, true, out multiplexorCondition);
		}

		public override string GenerateLtlTriggerEventDefinition(bool is32BitSignal, string eventFlagNameWithoutDot)
		{
			return LTLUtil.GenerateLtlTriggerEventDefinition(is32BitSignal, eventFlagNameWithoutDot);
		}

		protected override string GetComment()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.dataEvent is CANDataEvent)
			{
				CANDataEvent cANDataEvent = this.dataEvent as CANDataEvent;
				RawDataSignal rawDataSignal = cANDataEvent.RawDataSignal;
				if (rawDataSignal is RawDataSignalByte)
				{
					RawDataSignalByte rawDataSignalByte = rawDataSignal as RawDataSignalByte;
					stringBuilder.AppendFormat("Event on CAN data, ID 0x{0:X}, Byte {1} {2}", cANDataEvent.ID.Value, rawDataSignalByte.DataBytePos.Value, LTLGenericEventCode.GetCommentCompareOperatorStringForSignals(cANDataEvent.Relation.Value, cANDataEvent.LowValue.Value, cANDataEvent.HighValue.Value));
				}
				else if (rawDataSignal is RawDataSignalStartbitLength)
				{
					RawDataSignalStartbitLength rawDataSignalStartbitLength = rawDataSignal as RawDataSignalStartbitLength;
					stringBuilder.AppendFormat("Event on CAN data, ID 0x{0:X}, (startbit: {1}, length: {2}) {3}", new object[]
					{
						cANDataEvent.ID.Value,
						rawDataSignalStartbitLength.StartbitPos.Value,
						rawDataSignalStartbitLength.Length.Value,
						LTLGenericEventCode.GetCommentCompareOperatorStringForSignals(cANDataEvent.Relation.Value, cANDataEvent.LowValue.Value, cANDataEvent.HighValue.Value)
					});
				}
			}
			else if (this.dataEvent is LINDataEvent)
			{
				LINDataEvent lINDataEvent = this.dataEvent as LINDataEvent;
				RawDataSignal rawDataSignal2 = lINDataEvent.RawDataSignal;
				if (rawDataSignal2 is RawDataSignalByte)
				{
					RawDataSignalByte rawDataSignalByte2 = rawDataSignal2 as RawDataSignalByte;
					stringBuilder.AppendFormat("Event on LIN data, ID 0x{0:X}, Byte {1} {2}", lINDataEvent.ID.Value, rawDataSignalByte2.DataBytePos.Value, LTLGenericEventCode.GetCommentCompareOperatorStringForSignals(lINDataEvent.Relation.Value, lINDataEvent.LowValue.Value, lINDataEvent.HighValue.Value));
				}
				else if (rawDataSignal2 is RawDataSignalStartbitLength)
				{
					RawDataSignalStartbitLength rawDataSignalStartbitLength2 = rawDataSignal2 as RawDataSignalStartbitLength;
					stringBuilder.AppendFormat("Event on LIN data, ID 0x{0:X}, (startbit: {1}, length: {2}) {3}", new object[]
					{
						lINDataEvent.ID.Value,
						rawDataSignalStartbitLength2.StartbitPos.Value,
						rawDataSignalStartbitLength2.Length.Value,
						LTLGenericEventCode.GetCommentCompareOperatorStringForSignals(lINDataEvent.Relation.Value, lINDataEvent.LowValue.Value, lINDataEvent.HighValue.Value)
					});
				}
			}
			return stringBuilder.ToString();
		}
	}
}
