using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DiagSymbolsAccess;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLDiagnosticSignalEventCode : LTLSignalEventCode
	{
		private DiagnosticSignalEvent symSigEvent;

		public LTLDiagnosticSignalEventCode(string eventFlagName, DiagnosticSignalEvent ev) : base(eventFlagName)
		{
			this.symSigEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			SignalDefinition signalDefinition = null;
			if (DiagSymbolsManager.Instance().GetSignalDefinition(this.symSigEvent.DiagnosticEcuName.Value, this.symSigEvent.DiagnosticVariant.Value, this.symSigEvent.DiagnosticDid.Value, this.symSigEvent.SignalName.Value, out signalDefinition))
			{
				this.trigger.signalDefinition = signalDefinition;
			}
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
			string value = GlobalSettings.ServiceQualifiers.FirstOrDefault((KeyValuePair<DiagnosticSignalRequest, string> kvp) => kvp.Key.DatabasePath.Value == this.symSigEvent.DatabasePath.Value && kvp.Key.EcuQualifier.Value == this.symSigEvent.DiagnosticEcuName.Value && kvp.Key.ServiceQualifier.Value == this.symSigEvent.DiagnosticServiceName.Value && kvp.Key.DidId.Value == this.symSigEvent.DiagnosticDid.Value).Value;
			StringBuilder stringBuilder = new StringBuilder();
			LTLGenerator.LTLGenerationResult result = LTLUtil.GenerateDiagSignal(out stringBuilder, value, this.trigger.signalDefinition, signalBaseName, true);
			multiplexorCondition = string.Empty;
			stringBuilder.AppendLine();
			ltlSignalDefinition = stringBuilder.ToString();
			return result;
		}

		public override string GenerateLtlTriggerEventDefinition(bool is32BitSignal, string eventFlagNameWithoutDot)
		{
			string result;
			if (!is32BitSignal)
			{
				result = string.Format("DIAG_RECEIVE ({0}_Signal)", eventFlagNameWithoutDot);
			}
			else
			{
				result = string.Format("DIAG_RECEIVE ({0}_Signal.LOW)", eventFlagNameWithoutDot);
			}
			return result;
		}

		protected override string GetComment()
		{
			SignalDefinition signalDefinition;
			if (!DiagSymbolsManager.Instance().GetSignalDefinition(this.symSigEvent.DiagnosticEcuName.Value, this.symSigEvent.DiagnosticVariant.Value, this.symSigEvent.DiagnosticDid.Value, this.symSigEvent.SignalName.Value, out signalDefinition))
			{
				signalDefinition = null;
			}
			if (signalDefinition == null)
			{
				return "{ Unresolved Diagnostic Signal Event }";
			}
			string text = string.Format("Event on Diagnostic Signal {0}::{1}::{2} / Symbolic Signal ", this.symSigEvent.DiagnosticEcuName, this.symSigEvent.DiagnosticServiceName, this.symSigEvent.SignalName);
			return string.Format("{0}{1} (Startbit {2}, Length {3}) {4}", new object[]
			{
				text,
				this.symSigEvent.MessageName.Value,
				signalDefinition.StartBit,
				signalDefinition.Length,
				LTLGenericEventCode.GetCommentCompareOperatorStringForSignals(this.symSigEvent.Relation.Value, this.symSigEvent.LowValue.Value, this.symSigEvent.HighValue.Value)
			});
		}
	}
}
