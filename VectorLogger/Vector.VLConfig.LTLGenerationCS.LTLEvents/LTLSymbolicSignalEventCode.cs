using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLSymbolicSignalEventCode : LTLSignalEventCode
	{
		private SymbolicSignalEvent symSigEvent;

		private IApplicationDatabaseManager dbManager;

		private string configurationFolderPath;

		public LTLSymbolicSignalEventCode(string eventFlagName, SymbolicSignalEvent ev, IApplicationDatabaseManager dbManager, string configurationFolderPath) : base(eventFlagName)
		{
			this.symSigEvent = ev;
			this.dbManager = dbManager;
			this.configurationFolderPath = configurationFolderPath;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			if (!this.dbManager.ResolveSignalSymbolInDatabase(FileSystemServices.GetAbsolutePath(this.symSigEvent.DatabasePath.Value, this.configurationFolderPath), this.symSigEvent.NetworkName.Value, this.symSigEvent.MessageName.Value, this.symSigEvent.SignalName.Value, out this.trigger.signalDefinition))
			{
				ltlCode = string.Empty;
				return LTLGenerator.LTLGenerationResult.TriggerError_SignalResolve;
			}
			Dictionary<double, string> textEncValueTable;
			if (this.trigger.signalDefinition.HasTextEncodedValueTable && this.dbManager.GetTextEncodedSignalValueTable(this.symSigEvent.DatabasePath.Value, this.symSigEvent.NetworkName.Value, this.symSigEvent.MessageName.Value, this.symSigEvent.SignalName.Value, out textEncValueTable))
			{
				this.trigger.textEncValueTable = textEncValueTable;
			}
			if (this.symSigEvent.BusType.Value == BusType.Bt_FlexRay)
			{
				this.trigger.frSignalList = new List<SignalDefinition>();
				if (!this.dbManager.MapFlexraySignalsFromPduToFrames(FileSystemServices.GetAbsolutePath(this.symSigEvent.DatabasePath.Value, this.configurationFolderPath), this.symSigEvent.NetworkName.Value, this.symSigEvent.MessageName.Value, this.symSigEvent.SignalName.Value, out this.trigger.frSignalList))
				{
					ltlCode = string.Empty;
					return LTLGenerator.LTLGenerationResult.TriggerError_SignalResolve;
				}
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
			SignalDefinition signalDefinition;
			if (!this.dbManager.ResolveSignalSymbolInDatabase(FileSystemServices.GetAbsolutePath(this.symSigEvent.DatabasePath.Value, this.configurationFolderPath), this.symSigEvent.NetworkName.Value, this.symSigEvent.MessageName.Value, this.symSigEvent.SignalName.Value, out signalDefinition))
			{
				return "{ Unresolved Symbolic Signal Event }";
			}
			if (this.symSigEvent.BusType.Value == BusType.Bt_FlexRay && this.symSigEvent.IsFlexrayPDU.Value)
			{
				return string.Format("Event on Symbolic Signal {0}::{1} (Signal on PDU) {2}", this.symSigEvent.MessageName.Value, this.symSigEvent.SignalName.Value, LTLGenericEventCode.GetCommentCompareOperatorStringForSignals(this.symSigEvent.Relation.Value, this.symSigEvent.LowValue.Value, this.symSigEvent.HighValue.Value));
			}
			string text;
			if (this.symSigEvent.BusType.Value == BusType.Bt_FlexRay)
			{
				text = string.Format("{0:D}:{1:D}:{2:D}", signalDefinition.Message.ActualMessageId, signalDefinition.Message.FrBaseCycle, signalDefinition.Message.FrCycleRepetition);
			}
			else
			{
				text = string.Format("0x{0:X}", signalDefinition.Message.ActualMessageId);
			}
			return string.Format("Event on Symbolic Signal {0}::{1} (ID {2}, Startbit {3}, Length {4}) {5}", new object[]
			{
				this.symSigEvent.MessageName.Value,
				this.symSigEvent.SignalName.Value,
				text,
				signalDefinition.StartBit,
				signalDefinition.Length,
				LTLGenericEventCode.GetCommentCompareOperatorStringForSignals(this.symSigEvent.Relation.Value, this.symSigEvent.LowValue.Value, this.symSigEvent.HighValue.Value)
			});
		}
	}
}
