using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CcpXcpSignalConfiguration")]
	public class CcpXcpSignalConfiguration : Feature, IFeatureSymbolicDefinitions, IFeatureCcpXcpSignalDefinitions, IFeatureEvents
	{
		[DataMember(Name = "CcpXcpActionList")]
		private List<ActionCcpXcp> actionList;

		private List<CcpXcpSignal> signalList;

		private IList<ISymbolicMessage> symbolicMsgs;

		private IList<ISymbolicSignal> symbolicSignals;

		private List<DiagnosticAction> diagActions;

		private IList<ICcpXcpSignal> ccpXcpSignals;

		private IList<Event> events;

		private ValidatedProperty<bool> statisticsViolated;

		[DataMember(Name = "CcpXcpSignalConfigurationCycleTimeForNonCyclicDaqEvents")]
		public ValidatedProperty<uint> CycleTimeForNonCyclicDaqEvents
		{
			get;
			set;
		}

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
				foreach (ActionCcpXcp current in this.actionList)
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
					if (current.StopType is StopOnEvent)
					{
						Event @event = (current.StopType as StopOnEvent).Event;
						if (@event is ISymbolicMessage)
						{
							if (@event is MsgTimeoutEvent)
							{
								if ((@event as MsgTimeoutEvent).IsSymbolic.Value)
								{
									this.symbolicMsgs.Add(@event as ISymbolicMessage);
								}
							}
							else
							{
								this.symbolicMsgs.Add(@event as ISymbolicMessage);
							}
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
				foreach (ActionCcpXcp current in this.actionList)
				{
					if (current.Event is ISymbolicSignal)
					{
						this.symbolicSignals.Add(current.Event as ISymbolicSignal);
					}
					if (current.StopType is StopOnEvent && (current.StopType as StopOnEvent).Event is ISymbolicSignal)
					{
						this.symbolicSignals.Add((current.StopType as StopOnEvent).Event as ISymbolicSignal);
					}
				}
				return this.symbolicSignals;
			}
		}

		IList<DiagnosticAction> IFeatureSymbolicDefinitions.SymbolicDiagnosticActions
		{
			get
			{
				List<DiagnosticAction> arg_1B_0;
				if ((arg_1B_0 = this.diagActions) == null)
				{
					arg_1B_0 = (this.diagActions = new List<DiagnosticAction>());
				}
				return arg_1B_0;
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
				foreach (CcpXcpSignal current in this.Signals)
				{
					this.ccpXcpSignals.Add(current);
				}
				return this.ccpXcpSignals;
			}
		}

		public ReadOnlyCollection<CcpXcpSignal> Signals
		{
			get
			{
				if (this.signalList != null && !this.IsDirty)
				{
					if (!this.actionList.Any((ActionCcpXcp action) => action.IsDirty))
					{
						goto IL_A1;
					}
				}
				this.signalList = new List<CcpXcpSignal>();
				foreach (ActionCcpXcp current in this.Actions)
				{
					this.signalList.AddRange(current.Signals);
					current.SetClean();
				}
				this.IsDirty = false;
				IL_A1:
				return new ReadOnlyCollection<CcpXcpSignal>(this.signalList);
			}
		}

		public ReadOnlyCollection<CcpXcpSignal> ActiveSignalsIncludingDummies
		{
			get
			{
				return new ReadOnlyCollection<CcpXcpSignal>((from CcpXcpSignal sig in this.Signals
				where sig.IsActive.Value
				select sig).ToList<CcpXcpSignal>());
			}
		}

		public ReadOnlyCollection<CcpXcpSignal> ActiveSignals
		{
			get
			{
				return new ReadOnlyCollection<CcpXcpSignal>((from CcpXcpSignal sig in this.Signals
				where sig.IsActive.Value && !(sig is CcpXcpSignalDummy) && sig.ActionCcpXcp.IsActive.Value
				select sig).ToList<CcpXcpSignal>());
			}
		}

		public ReadOnlyCollection<ActionCcpXcp> Actions
		{
			get
			{
				return new ReadOnlyCollection<ActionCcpXcp>(this.actionList);
			}
		}

		public ReadOnlyCollection<ActionCcpXcp> ActiveActions
		{
			get
			{
				return new ReadOnlyCollection<ActionCcpXcp>((from ActionCcpXcp action in this.actionList
				where action.IsActive.Value
				select action).ToList<ActionCcpXcp>());
			}
		}

		public ValidatedProperty<bool> StatisticsViolated
		{
			get
			{
				ValidatedProperty<bool> arg_1C_0;
				if ((arg_1C_0 = this.statisticsViolated) == null)
				{
					arg_1C_0 = (this.statisticsViolated = new ValidatedProperty<bool>(false));
				}
				return arg_1C_0;
			}
		}

		private bool IsDirty
		{
			get;
			set;
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is CcpXcpSignalConfiguration || otherFeature is CANChannelConfiguration || otherFeature is LINChannelConfiguration || otherFeature is FlexrayChannelConfiguration || otherFeature is DatabaseConfiguration)
			{
				updateService.Notify<CcpXcpSignalConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<CcpXcpSignalConfiguration>(this);
		}

		IList<Event> IFeatureEvents.GetEvents(bool activeEventsOnly)
		{
			if (this.events == null)
			{
				this.events = new List<Event>();
			}
			this.events.Clear();
			ReadOnlyCollection<ActionCcpXcp> readOnlyCollection = activeEventsOnly ? this.ActiveActions : this.Actions;
			foreach (ActionCcpXcp current in readOnlyCollection)
			{
				if (current.Event != null)
				{
					this.events.Add(current.Event);
				}
				if (current.StopType is StopOnEvent)
				{
					this.events.Add((current.StopType as StopOnEvent).Event);
				}
			}
			return this.events;
		}

		public CcpXcpSignalConfiguration()
		{
			this.CycleTimeForNonCyclicDaqEvents = new ValidatedProperty<uint>(10000u);
			this.actionList = new List<ActionCcpXcp>();
			this.IsDirty = true;
		}

		public void AddSignal(CcpXcpSignal sig, ActionCcpXcp action)
		{
			if (action != null)
			{
				action.AddSignal(sig);
			}
		}

		public void AddSignals(List<CcpXcpSignal> signalsToAdd, ActionCcpXcp action)
		{
			action.AddSignals(signalsToAdd);
		}

		public bool RemoveSignal(CcpXcpSignal sig, ActionCcpXcp action)
		{
			bool result = false;
			if (action != null)
			{
				result = action.RemoveSignal(sig);
			}
			return result;
		}

		public void InsertSignal(CcpXcpSignal signal, ActionCcpXcp action, int insertPos)
		{
			action.InsertSignal(signal, insertPos);
		}

		public void AddAction(ActionCcpXcp action)
		{
			this.actionList.Add(action);
			this.IsDirty = true;
		}

		public void InsertAction(ActionCcpXcp action, int insertPos)
		{
			if (insertPos > this.actionList.Count - 1)
			{
				this.AddAction(action);
			}
			else
			{
				this.actionList.Insert(insertPos, action);
			}
			this.IsDirty = true;
		}

		public bool RemoveAction(ActionCcpXcp action)
		{
			bool result = this.actionList.Remove(action);
			this.IsDirty = true;
			return result;
		}

		public void RemoveActions(List<ActionCcpXcp> delList)
		{
			if (delList != null && delList.Any<ActionCcpXcp>())
			{
				this.actionList = this.actionList.Except(delList).ToList<ActionCcpXcp>();
			}
			this.IsDirty = true;
		}
	}
}
