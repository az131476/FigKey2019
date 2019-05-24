using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "SendMessageConfiguration")]
	public class SendMessageConfiguration : Feature, IFeatureSymbolicDefinitions, IFeatureCcpXcpSignalDefinitions, IFeatureTransmitMessages, IFeatureEvents
	{
		private IList<ISymbolicMessage> symbolicMsgs;

		private IList<ISymbolicSignal> symbolicSignals;

		private List<DiagnosticAction> diagActions;

		private IList<ICcpXcpSignal> ccpXcpSignals;

		private IList<Event> events;

		[DataMember(Name = "SendMessageConfigurationActionList")]
		private List<ActionSendMessage> actionList;

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
				return this;
			}
		}

		public override IFeatureEvents Events
		{
			get
			{
				return this;
			}
		}

		public override IFeatureVirtualLogMessages VirtualLogMessages
		{
			get
			{
				return null;
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
				foreach (ActionSendMessage current in this.actionList)
				{
					if (current.IsSymbolic.Value)
					{
						this.symbolicMsgs.Add(current);
					}
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
				foreach (ActionSendMessage current in this.actionList)
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
				foreach (ActionSendMessage current in this.actionList)
				{
					if (current.Event is ICcpXcpSignal)
					{
						this.ccpXcpSignals.Add(current.Event as ICcpXcpSignal);
					}
				}
				return this.ccpXcpSignals;
			}
		}

		IList<ITransmitMessageChannel> IFeatureTransmitMessages.ActiveTransmitMessageChannels
		{
			get
			{
				List<ITransmitMessageChannel> list = new List<ITransmitMessageChannel>();
				foreach (ActionSendMessage current in this.actionList)
				{
					if (current.IsActive.Value && !current.IsVirtual.Value)
					{
						list.Add(current);
					}
				}
				return list;
			}
		}

		public ReadOnlyCollection<ActionSendMessage> Actions
		{
			get
			{
				return new ReadOnlyCollection<ActionSendMessage>(this.actionList);
			}
		}

		public bool HasActiveVoCANEventConfigured
		{
			get
			{
				foreach (ActionSendMessage current in this.actionList)
				{
					if (current.IsActive.Value && current.Event is VoCanRecordingEvent)
					{
						return true;
					}
				}
				return false;
			}
		}

		public ReadOnlyCollection<ActionSendMessage> ActiveVoCanActions
		{
			get
			{
				return new ReadOnlyCollection<ActionSendMessage>((from ActionSendMessage action in this.actionList
				where action.Event is VoCanRecordingEvent && action.IsActive.Value
				select action).ToList<ActionSendMessage>());
			}
		}

		public ReadOnlyCollection<ActionSendMessage> ActiveCasKeyActions
		{
			get
			{
				return new ReadOnlyCollection<ActionSendMessage>((from ActionSendMessage action in this.actionList
				where action.Event is KeyEvent && action.IsActive.Value && (action.Event as KeyEvent).IsCasKey
				select action).ToList<ActionSendMessage>());
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is SendMessageConfiguration || otherFeature is CANChannelConfiguration || otherFeature is LINChannelConfiguration || otherFeature is FlexrayChannelConfiguration || otherFeature is CcpXcpSignalConfiguration || otherFeature is DatabaseConfiguration || otherFeature is LEDConfiguration || otherFeature is TriggerConfiguration)
			{
				updateService.Notify<SendMessageConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<SendMessageConfiguration>(this);
		}

		IList<Event> IFeatureEvents.GetEvents(bool activeEventsOnly)
		{
			if (this.events == null)
			{
				this.events = new List<Event>();
			}
			this.events.Clear();
			foreach (ActionSendMessage current in this.actionList)
			{
				if (!activeEventsOnly || current.IsActive.Value)
				{
					this.events.Add(current.Event);
				}
			}
			return this.events;
		}

		public SendMessageConfiguration()
		{
			this.actionList = new List<ActionSendMessage>();
		}

		public int GetActiveActionsCount()
		{
			int num = 0;
			foreach (ActionSendMessage current in this.Actions)
			{
				if (current.IsActive.Value)
				{
					num++;
				}
			}
			return num;
		}

		public void AddAction(ActionSendMessage action)
		{
			this.actionList.Add(action);
		}

		public bool InsertAction(int idx, ActionSendMessage action)
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

		public bool RemoveAction(ActionSendMessage action)
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

		public IList<ActionSendMessage> GetActionsForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<ActionSendMessage> list = new List<ActionSendMessage>();
			foreach (ActionSendMessage current in this.actionList)
			{
				if (current.IsSymbolic.Value && string.Compare(current.DatabasePath.Value, databasePath, true) == 0 && ((current.ChannelNumber.Value == channel && current.BusType.Value == busType) || (current.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
				{
					list.Add(current);
				}
			}
			return list;
		}

		public IList<SymbolicMessageEvent> GetSymbolicMessageEventsForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<SymbolicMessageEvent> list = new List<SymbolicMessageEvent>();
			foreach (ActionSendMessage current in this.actionList)
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

		public IList<SymbolicSignalEvent> GetSymbolicSignalEventsForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<SymbolicSignalEvent> list = new List<SymbolicSignalEvent>();
			foreach (ActionSendMessage current in this.actionList)
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
			foreach (ActionSendMessage current in this.actionList)
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
	}
}
