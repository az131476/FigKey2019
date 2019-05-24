using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DiagnosticActionsConfiguration")]
	public class DiagnosticActionsConfiguration : Feature, IFeatureSymbolicDefinitions, IFeatureCcpXcpSignalDefinitions, IFeatureEvents
	{
		private IList<ISymbolicMessage> symbolicMsgs;

		private IList<ISymbolicSignal> symbolicSignals;

		private List<DiagnosticAction> diagActions;

		private IList<ICcpXcpSignal> ccpXcpSignals;

		private IList<Event> events;

		[DataMember(Name = "DiagnosticActionsConfigurationTriggeredSequences")]
		private List<TriggeredDiagnosticActionSequence> triggeredActionSequences;

		[DataMember(Name = "DiagnosticActionsConfigurationIsDiagCommRestartEnabled")]
		public ValidatedProperty<bool> IsDiagCommRestartEnabled;

		[DataMember(Name = "DiagnosticActionsConfigurationDiagCommRestartTime")]
		public ValidatedProperty<uint> DiagCommRestartTime;

		[DataMember(Name = "DiagnosticActionsConfigurationRecordDiagCommMemories")]
		public List<ValidatedProperty<bool>> recordDiagCommOnMemories;

		public override IFeatureReferencedFiles ReferencedFiles
		{
			get
			{
				return null;
			}
		}

		public override IFeatureSymbolicDefinitions SymbolicDefinitions
		{
			get
			{
				return this;
			}
		}

		public override IFeatureCcpXcpSignalDefinitions CcpXcpSignalDefinitions
		{
			get
			{
				return this;
			}
		}

		public override IFeatureTransmitMessages TransmitMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureVirtualLogMessages VirtualLogMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureEvents Events
		{
			get
			{
				return this;
			}
		}

		IList<ISymbolicMessage> IFeatureSymbolicDefinitions.SymbolicMessages
		{
			get
			{
				if (this.symbolicMsgs == null)
				{
					this.symbolicMsgs = new List<ISymbolicMessage>();
				}
				this.symbolicMsgs.Clear();
				foreach (TriggeredDiagnosticActionSequence current in this.triggeredActionSequences)
				{
					if (current.Event is ISymbolicMessage)
					{
						if (current.Event is MsgTimeoutEvent)
						{
							if ((current.Event as MsgTimeoutEvent).IsSymbolic.Value)
							{
								this.symbolicMsgs.Add(current.Event as ISymbolicMessage);
							}
						}
						else
						{
							this.symbolicMsgs.Add(current.Event as ISymbolicMessage);
						}
					}
				}
				return this.symbolicMsgs;
			}
		}

		IList<ISymbolicSignal> IFeatureSymbolicDefinitions.SymbolicSignals
		{
			get
			{
				if (this.symbolicSignals == null)
				{
					this.symbolicSignals = new List<ISymbolicSignal>();
				}
				this.symbolicSignals.Clear();
				foreach (TriggeredDiagnosticActionSequence current in this.triggeredActionSequences)
				{
					if (current.Event is ISymbolicSignal)
					{
						this.symbolicSignals.Add(current.Event as ISymbolicSignal);
					}
				}
				return this.symbolicSignals;
			}
		}

		IList<DiagnosticAction> IFeatureSymbolicDefinitions.SymbolicDiagnosticActions
		{
			get
			{
				if (this.diagActions == null)
				{
					this.diagActions = new List<DiagnosticAction>();
				}
				this.diagActions.Clear();
				foreach (TriggeredDiagnosticActionSequence current in this.triggeredActionSequences)
				{
					foreach (DiagnosticAction current2 in current.Actions)
					{
						this.diagActions.Add(current2);
					}
				}
				return this.diagActions;
			}
		}

		IList<ICcpXcpSignal> IFeatureCcpXcpSignalDefinitions.CcpXcpSignals
		{
			get
			{
				if (this.ccpXcpSignals == null)
				{
					this.ccpXcpSignals = new List<ICcpXcpSignal>();
				}
				this.ccpXcpSignals.Clear();
				foreach (TriggeredDiagnosticActionSequence current in this.triggeredActionSequences)
				{
					if (current.Event is ICcpXcpSignal)
					{
						this.ccpXcpSignals.Add(current.Event as ICcpXcpSignal);
					}
				}
				return this.ccpXcpSignals;
			}
		}

		public ReadOnlyCollection<TriggeredDiagnosticActionSequence> TriggeredActionSequences
		{
			get
			{
				return new ReadOnlyCollection<TriggeredDiagnosticActionSequence>(this.triggeredActionSequences);
			}
		}

		public ReadOnlyCollection<DiagnosticAction> DiagnosticActions
		{
			get
			{
				List<DiagnosticAction> list = new List<DiagnosticAction>();
				foreach (TriggeredDiagnosticActionSequence current in this.triggeredActionSequences)
				{
					foreach (DiagnosticAction current2 in current.Actions)
					{
						list.Add(current2);
					}
				}
				return new ReadOnlyCollection<DiagnosticAction>(list);
			}
		}

		public ReadOnlyCollection<TriggeredDiagnosticActionSequence> CasKeySequences
		{
			get
			{
				List<TriggeredDiagnosticActionSequence> list = new List<TriggeredDiagnosticActionSequence>();
				foreach (TriggeredDiagnosticActionSequence current in this.triggeredActionSequences)
				{
					if (current.Event is KeyEvent && (current.Event as KeyEvent).IsCasKey)
					{
						list.Add(current);
					}
				}
				return new ReadOnlyCollection<TriggeredDiagnosticActionSequence>(list);
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is DiagnosticActionsConfiguration || otherFeature is CANChannelConfiguration || otherFeature is LINChannelConfiguration || otherFeature is FlexrayChannelConfiguration || otherFeature is CcpXcpSignalConfiguration || otherFeature is DiagnosticsDatabaseConfiguration || otherFeature is DatabaseConfiguration)
			{
				updateService.Notify<DiagnosticActionsConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<DiagnosticActionsConfiguration>(this);
		}

		IList<Event> IFeatureEvents.GetEvents(bool activeEventsOnly)
		{
			if (this.events == null)
			{
				this.events = new List<Event>();
			}
			this.events.Clear();
			foreach (DiagnosticAction current in this.DiagnosticActions)
			{
				if (current.TriggeredDiagnosticActionSequence != null)
				{
					this.events.Add(current.TriggeredDiagnosticActionSequence.Event);
				}
			}
			return this.events;
		}

		public DiagnosticActionsConfiguration()
		{
			this.triggeredActionSequences = new List<TriggeredDiagnosticActionSequence>();
			this.IsDiagCommRestartEnabled = new ValidatedProperty<bool>(true);
			this.DiagCommRestartTime = new ValidatedProperty<uint>(1u);
			this.recordDiagCommOnMemories = new List<ValidatedProperty<bool>>();
			this.recordDiagCommOnMemories.Add(new ValidatedProperty<bool>(true));
			this.recordDiagCommOnMemories.Add(new ValidatedProperty<bool>(true));
		}

		public ReadOnlyCollection<DiagnosticAction> DiagnosticActionsOfDescription(string path)
		{
			List<DiagnosticAction> list = new List<DiagnosticAction>();
			foreach (TriggeredDiagnosticActionSequence current in this.triggeredActionSequences)
			{
				foreach (DiagnosticAction current2 in current.Actions)
				{
					if (current2.DatabasePath.Value == path)
					{
						list.Add(current2);
					}
				}
			}
			return new ReadOnlyCollection<DiagnosticAction>(list);
		}

		public DiagnosticSignalRequest DiagnosticSignalRequestOfSignalEvent(DiagnosticSignalEvent signalEvent)
		{
			foreach (TriggeredDiagnosticActionSequence current in this.triggeredActionSequences)
			{
				foreach (DiagnosticAction current2 in current.Actions)
				{
					DiagnosticSignalRequest diagnosticSignalRequest = current2 as DiagnosticSignalRequest;
					if (diagnosticSignalRequest != null && diagnosticSignalRequest.DatabasePath.Value == signalEvent.DatabasePath.Value && diagnosticSignalRequest.EcuQualifier.Value == signalEvent.DiagnosticEcuName.Value && diagnosticSignalRequest.ServiceQualifier.Value == signalEvent.DiagnosticServiceName.Value && diagnosticSignalRequest.DidId.Value == signalEvent.DiagnosticDid.Value && diagnosticSignalRequest.SignalQualifier.Value == signalEvent.SignalName.Value)
					{
						return diagnosticSignalRequest;
					}
				}
			}
			return null;
		}

		public IList<SymbolicMessageEvent> GetSymbolicMessageEventsForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<SymbolicMessageEvent> list = new List<SymbolicMessageEvent>();
			foreach (TriggeredDiagnosticActionSequence current in this.triggeredActionSequences)
			{
				if (current.Event is SymbolicMessageEvent)
				{
					SymbolicMessageEvent symbolicMessageEvent = current.Event as SymbolicMessageEvent;
					if (string.Compare(symbolicMessageEvent.DatabasePath.Value, databasePath, true) == 0 && ((symbolicMessageEvent.ChannelNumber.Value == channel && symbolicMessageEvent.BusType.Value == busType) || (symbolicMessageEvent.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
					{
						list.Add(symbolicMessageEvent);
					}
				}
			}
			return list;
		}

		public IList<SymbolicSignalEvent> GetSymbolicSignaEventsForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<SymbolicSignalEvent> list = new List<SymbolicSignalEvent>();
			foreach (TriggeredDiagnosticActionSequence current in this.triggeredActionSequences)
			{
				if (current.Event is SymbolicSignalEvent)
				{
					SymbolicSignalEvent symbolicSignalEvent = current.Event as SymbolicSignalEvent;
					if (string.Compare(symbolicSignalEvent.DatabasePath.Value, databasePath, true) == 0 && ((symbolicSignalEvent.ChannelNumber.Value == channel && symbolicSignalEvent.BusType.Value == busType) || (symbolicSignalEvent.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
					{
						list.Add(symbolicSignalEvent);
					}
				}
			}
			return list;
		}

		public IList<MsgTimeoutEvent> GetSymbolicMsgTimeoutEventsForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<MsgTimeoutEvent> list = new List<MsgTimeoutEvent>();
			foreach (TriggeredDiagnosticActionSequence current in this.triggeredActionSequences)
			{
				if (current.Event is MsgTimeoutEvent && (current.Event as MsgTimeoutEvent).IsSymbolic.Value)
				{
					MsgTimeoutEvent msgTimeoutEvent = current.Event as MsgTimeoutEvent;
					if (string.Compare(msgTimeoutEvent.DatabasePath.Value, databasePath, true) == 0 && ((msgTimeoutEvent.ChannelNumber.Value == channel && msgTimeoutEvent.BusType.Value == busType) || (msgTimeoutEvent.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
					{
						list.Add(msgTimeoutEvent);
					}
				}
			}
			return list;
		}

		public void AddActionSequence(TriggeredDiagnosticActionSequence seq)
		{
			this.triggeredActionSequences.Add(seq);
		}

		public void InsertActionSequence(TriggeredDiagnosticActionSequence seq, int insertPos)
		{
			if (insertPos > this.triggeredActionSequences.Count - 1)
			{
				this.AddActionSequence(seq);
				return;
			}
			this.triggeredActionSequences.Insert(insertPos, seq);
		}

		public bool RemoveActionSequence(TriggeredDiagnosticActionSequence seq)
		{
			if (this.triggeredActionSequences.Contains(seq))
			{
				this.triggeredActionSequences.Remove(seq);
				return true;
			}
			return false;
		}
	}
}
