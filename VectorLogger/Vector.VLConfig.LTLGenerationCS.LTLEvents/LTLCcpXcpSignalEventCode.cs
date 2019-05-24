using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLCcpXcpSignalEventCode : LTLSignalEventCode
	{
		private CcpXcpSignalEvent symSigEvent;

		public LTLCcpXcpSignalEventCode(string eventFlagName, CcpXcpSignalEvent ev) : base(eventFlagName)
		{
			this.symSigEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			this.trigger.signalDefinition = this.symSigEvent.SignalDefinitionInGeneratedDatabase;
			if (this.trigger.signalDefinition == null)
			{
				ltlCode = string.Empty;
				return LTLGenerator.LTLGenerationResult.TriggerError_SignalResolve;
			}
			if (this.symSigEvent.BusType.Value == BusType.Bt_FlexRay)
			{
				ltlCode = string.Empty;
				return LTLGenerator.LTLGenerationResult.TriggerError_UnsupportedEvent;
			}
			this.trigger.busType = this.symSigEvent.BusType.Value;
			this.trigger.channelNumber = this.symSigEvent.ChannelNumber.Value;
			this.trigger.relation = this.symSigEvent.Relation.Value;
			this.trigger.lowValue = this.symSigEvent.LowValue.Value;
			this.trigger.highValue = this.symSigEvent.HighValue.Value;
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
			SignalDefinition signalDefinitionInGeneratedDatabase = this.symSigEvent.SignalDefinitionInGeneratedDatabase;
			if (signalDefinitionInGeneratedDatabase == null)
			{
				return "{ Unresolved XCP Signal Event }";
			}
			string text = string.Format("Event on XCP Signal {0}::{1} / Symbolic Signal ", this.symSigEvent.CcpXcpEcuName, this.symSigEvent.SignalName);
			if (this.symSigEvent.BusType.Value == BusType.Bt_FlexRay && this.symSigEvent.IsFlexrayPDU.Value)
			{
				return string.Format("{0}{1}::{2} (Signal on PDU) {3}", new object[]
				{
					text,
					this.symSigEvent.MessageName.Value,
					this.symSigEvent.SignalNameInGeneratedDatabase,
					LTLGenericEventCode.GetCommentCompareOperatorStringForSignals(this.symSigEvent.Relation.Value, this.symSigEvent.LowValue.Value, this.symSigEvent.HighValue.Value)
				});
			}
			if (this.symSigEvent.BusType.Value == BusType.Bt_Ethernet)
			{
				return string.Format("{0}{1}::{2} (Signal on PDU) {3}", new object[]
				{
					text,
					this.symSigEvent.MessageName.Value,
					this.symSigEvent.SignalNameInGeneratedDatabase,
					LTLGenericEventCode.GetCommentCompareOperatorStringForSignals(this.symSigEvent.Relation.Value, this.symSigEvent.LowValue.Value, this.symSigEvent.HighValue.Value)
				});
			}
			string text2;
			if (this.symSigEvent.BusType.Value == BusType.Bt_FlexRay)
			{
				text2 = string.Format("{0:D}:{1:D}:{2:D}", signalDefinitionInGeneratedDatabase.Message.ActualMessageId, signalDefinitionInGeneratedDatabase.Message.FrBaseCycle, signalDefinitionInGeneratedDatabase.Message.FrCycleRepetition);
			}
			else
			{
				text2 = string.Format("0x{0:X}", signalDefinitionInGeneratedDatabase.Message.ActualMessageId);
			}
			return string.Format("{0}{1}::{2} (ID {3}, Startbit {4}, Length {5}) {6}", new object[]
			{
				text,
				this.symSigEvent.MessageName.Value,
				this.symSigEvent.SignalNameInGeneratedDatabase,
				text2,
				signalDefinitionInGeneratedDatabase.StartBit,
				signalDefinitionInGeneratedDatabase.Length,
				LTLGenericEventCode.GetCommentCompareOperatorStringForSignals(this.symSigEvent.Relation.Value, this.symSigEvent.LowValue.Value, this.symSigEvent.HighValue.Value)
			});
		}
	}
}
