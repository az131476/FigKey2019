using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DigitalOutputsConfiguration")]
	public class DigitalOutputsConfiguration : Feature, IFeatureSymbolicDefinitions, IFeatureCcpXcpSignalDefinitions, IFeatureEvents
	{
		private IList<ICcpXcpSignal> ccpXcpSignals;

		private IList<ISymbolicMessage> symbolicMsgs;

		private IList<ISymbolicSignal> symbolicSignals;

		private List<DiagnosticAction> diagActions;

		private IList<Event> events;

		[DataMember(Name = "DigitalOutputsConfigurationActionList")]
		private List<ActionDigitalOutput> actionList;

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

		IList<ICcpXcpSignal> IFeatureCcpXcpSignalDefinitions.CcpXcpSignals
		{
			get
			{
				if (this.ccpXcpSignals == null)
				{
					this.ccpXcpSignals = new List<ICcpXcpSignal>();
				}
				this.ccpXcpSignals.Clear();
				foreach (ActionDigitalOutput current in this.actionList)
				{
					if (current.Event is ICcpXcpSignal)
					{
						this.ccpXcpSignals.Add(current.Event as ICcpXcpSignal);
					}
					if (current.StopType is StopOnEvent)
					{
						Event @event = (current.StopType as StopOnEvent).Event;
						if (@event is ICcpXcpSignal)
						{
							this.ccpXcpSignals.Add(@event as ICcpXcpSignal);
						}
					}
				}
				return this.ccpXcpSignals;
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
				foreach (ActionDigitalOutput current in this.actionList)
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
				foreach (ActionDigitalOutput current in this.actionList)
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
				if (this.diagActions == null)
				{
					this.diagActions = new List<DiagnosticAction>();
				}
				return this.diagActions;
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

		public ReadOnlyCollection<ActionDigitalOutput> Actions
		{
			get
			{
				return new ReadOnlyCollection<ActionDigitalOutput>(this.actionList);
			}
		}

		public ReadOnlyCollection<ActionDigitalOutput> ActiveCasKeyActions
		{
			get
			{
				return new ReadOnlyCollection<ActionDigitalOutput>((from ActionDigitalOutput action in this.actionList
				where action.Event is KeyEvent && action.IsActive.Value && (action.Event as KeyEvent).IsCasKey
				select action).ToList<ActionDigitalOutput>());
			}
		}

		IList<Event> IFeatureEvents.GetEvents(bool activeEventsOnly)
		{
			if (this.events == null)
			{
				this.events = new List<Event>();
			}
			this.events.Clear();
			foreach (ActionDigitalOutput current in this.actionList)
			{
				if (!activeEventsOnly || current.IsActive.Value)
				{
					this.events.Add(current.Event);
					if (current.StopType is StopOnEvent)
					{
						this.events.Add((current.StopType as StopOnEvent).Event);
					}
				}
			}
			return this.events;
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is DigitalOutputsConfiguration || otherFeature is CANChannelConfiguration || otherFeature is LINChannelConfiguration || otherFeature is FlexrayChannelConfiguration || otherFeature is CcpXcpSignalConfiguration || otherFeature is DatabaseConfiguration || otherFeature is DigitalInputConfiguration)
			{
				updateService.Notify<DigitalOutputsConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<DigitalOutputsConfiguration>(this);
		}

		public DigitalOutputsConfiguration()
		{
			this.actionList = new List<ActionDigitalOutput>();
		}

		public bool IsAnyDigitalOutputActive()
		{
			foreach (ActionDigitalOutput current in this.Actions)
			{
				if (current.IsActive.Value)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsDigitalOutputActive(int pinNo)
		{
			return pinNo <= this.actionList.Count && this.actionList[pinNo - 1].IsActive.Value;
		}

		public void AddAction(ActionDigitalOutput action)
		{
			this.actionList.Add(action);
		}

		public bool InsertAction(int idx, ActionDigitalOutput action)
		{
			if (idx >= 0 && idx < this.actionList.Count)
			{
				this.actionList.Insert(idx, action);
				return true;
			}
			if (idx == this.actionList.Count)
			{
				this.actionList.Add(action);
				return true;
			}
			return false;
		}

		public bool RemoveAction(ActionDigitalOutput action)
		{
			if (this.actionList.Contains(action))
			{
				this.actionList.Remove(action);
				return true;
			}
			return false;
		}

		public void ClearActions()
		{
			this.actionList.Clear();
		}

		public IList<SymbolicMessageEvent> GetSymbolicMessageEventsForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<SymbolicMessageEvent> list = new List<SymbolicMessageEvent>();
			foreach (ActionDigitalOutput current in this.actionList)
			{
				if (current.Event is SymbolicMessageEvent)
				{
					SymbolicMessageEvent symbolicMessageEvent = current.Event as SymbolicMessageEvent;
					if (string.Compare(symbolicMessageEvent.DatabasePath.Value, databasePath, true) == 0 && ((symbolicMessageEvent.ChannelNumber.Value == channel && symbolicMessageEvent.BusType.Value == busType) || (symbolicMessageEvent.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
					{
						list.Add(symbolicMessageEvent);
					}
				}
				if (current.StopType is StopOnEvent && (current.StopType as StopOnEvent).Event is SymbolicMessageEvent)
				{
					SymbolicMessageEvent symbolicMessageEvent2 = (current.StopType as StopOnEvent).Event as SymbolicMessageEvent;
					if (symbolicMessageEvent2 != null && string.Compare(symbolicMessageEvent2.DatabasePath.Value, databasePath, true) == 0 && ((symbolicMessageEvent2.ChannelNumber.Value == channel && symbolicMessageEvent2.BusType.Value == busType) || (symbolicMessageEvent2.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
					{
						list.Add(symbolicMessageEvent2);
					}
				}
			}
			return list;
		}

		public IList<SymbolicSignalEvent> GetSymbolicSignalEventsForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<SymbolicSignalEvent> list = new List<SymbolicSignalEvent>();
			foreach (ActionDigitalOutput current in this.actionList)
			{
				if (current.Event is SymbolicSignalEvent)
				{
					SymbolicSignalEvent symbolicSignalEvent = current.Event as SymbolicSignalEvent;
					if (string.Compare(symbolicSignalEvent.DatabasePath.Value, databasePath, true) == 0 && ((symbolicSignalEvent.ChannelNumber.Value == channel && symbolicSignalEvent.BusType.Value == busType) || (symbolicSignalEvent.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
					{
						list.Add(symbolicSignalEvent);
					}
				}
				if (current.StopType is StopOnEvent && (current.StopType as StopOnEvent).Event is SymbolicSignalEvent)
				{
					SymbolicSignalEvent symbolicSignalEvent2 = (current.StopType as StopOnEvent).Event as SymbolicSignalEvent;
					if (symbolicSignalEvent2 != null && string.Compare(symbolicSignalEvent2.DatabasePath.Value, databasePath, true) == 0 && ((symbolicSignalEvent2.ChannelNumber.Value == channel && symbolicSignalEvent2.BusType.Value == busType) || (symbolicSignalEvent2.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
					{
						list.Add((current.StopType as StopOnEvent).Event as SymbolicSignalEvent);
					}
				}
			}
			return list;
		}

		public IList<MsgTimeoutEvent> GetSymbolicMsgTimeoutEventsForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<MsgTimeoutEvent> list = new List<MsgTimeoutEvent>();
			foreach (ActionDigitalOutput current in this.actionList)
			{
				if (current.Event is MsgTimeoutEvent && (current.Event as MsgTimeoutEvent).IsSymbolic.Value)
				{
					MsgTimeoutEvent msgTimeoutEvent = current.Event as MsgTimeoutEvent;
					if (string.Compare(msgTimeoutEvent.DatabasePath.Value, databasePath, true) == 0 && ((msgTimeoutEvent.ChannelNumber.Value == channel && msgTimeoutEvent.BusType.Value == busType) || (msgTimeoutEvent.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
					{
						list.Add(msgTimeoutEvent);
					}
				}
				if (current.StopType is StopOnEvent)
				{
					MsgTimeoutEvent msgTimeoutEvent2 = (current.StopType as StopOnEvent).Event as MsgTimeoutEvent;
					if (msgTimeoutEvent2 != null && msgTimeoutEvent2.IsSymbolic.Value && string.Compare(msgTimeoutEvent2.DatabasePath.Value, databasePath, true) == 0 && ((msgTimeoutEvent2.ChannelNumber.Value == channel && msgTimeoutEvent2.BusType.Value == busType) || (msgTimeoutEvent2.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
					{
						list.Add(msgTimeoutEvent2);
					}
				}
			}
			return list;
		}
	}
}
