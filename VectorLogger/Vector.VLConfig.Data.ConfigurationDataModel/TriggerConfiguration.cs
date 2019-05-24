using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "TriggerConfiguration")]
	public class TriggerConfiguration : Feature, IFeatureSymbolicDefinitions, IFeatureCcpXcpSignalDefinitions, IFeatureEvents
	{
		private List<ISymbolicMessage> symbolicMsgs;

		private List<ISymbolicSignal> symbolicSignals;

		private List<DiagnosticAction> diagActions;

		private List<ICcpXcpSignal> ccpXcpSignals;

		private IList<Event> events;

		[DataMember(Name = "TriggerConfigurationTriggerMode")]
		public ValidatedProperty<TriggerMode> TriggerMode;

		[DataMember(Name = "TriggerConfigurationPostTriggerTime")]
		public ValidatedProperty<uint> PostTriggerTime;

		public int MemoryNr;

		[DataMember(Name = "TriggerConfigurationTriggerList")]
		private List<RecordTrigger> mTriggerList;

		[DataMember(Name = "TriggerConfigurationTriggerListOnOff")]
		private List<RecordTrigger> mTriggerListOnOff;

		[DataMember(Name = "TriggerConfigurationTriggerListPermanent")]
		private List<RecordTrigger> mTriggerListPermanent;

		[DataMember(Name = "TriggerConfigurationMemoryRingBuffer")]
		private MemoryRingBuffer mEmoryRingBuffer;

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
				TriggerConfiguration.AddSymbolicMessages(ref this.symbolicMsgs, this.mTriggerList);
				TriggerConfiguration.AddSymbolicMessages(ref this.symbolicMsgs, this.mTriggerListOnOff);
				TriggerConfiguration.AddSymbolicMessages(ref this.symbolicMsgs, this.mTriggerListPermanent);
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
				TriggerConfiguration.AddSymbolicSignals(ref this.symbolicSignals, this.mTriggerList);
				TriggerConfiguration.AddSymbolicSignals(ref this.symbolicSignals, this.mTriggerListOnOff);
				TriggerConfiguration.AddSymbolicSignals(ref this.symbolicSignals, this.mTriggerListPermanent);
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
				TriggerConfiguration.AddCcpXcpSignals(ref this.ccpXcpSignals, this.mTriggerList);
				TriggerConfiguration.AddCcpXcpSignals(ref this.ccpXcpSignals, this.mTriggerListOnOff);
				TriggerConfiguration.AddCcpXcpSignals(ref this.ccpXcpSignals, this.mTriggerListPermanent);
				return this.ccpXcpSignals;
			}
		}

		public bool HasActiveVoCanEventConfigured
		{
			get
			{
				ReadOnlyCollection<RecordTrigger> source;
				switch (this.TriggerMode.Value)
				{
				case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Triggered:
					source = this.ActiveTriggers;
					break;
				case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Permanent:
					source = this.ActivePermanentMarkers;
					break;
				case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.OnOff:
					source = this.ActiveOnOffTriggers;
					break;
				default:
					return false;
				}
				return source.Any((RecordTrigger trigger) => trigger.Event is VoCanRecordingEvent);
			}
		}

		public ReadOnlyCollection<RecordTrigger> CurrentList
		{
			get
			{
				switch (this.TriggerMode.Value)
				{
				case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Triggered:
					return this.Triggers;
				case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Permanent:
					return this.PermanentMarkers;
				case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.OnOff:
					return this.OnOffTriggers;
				default:
					return null;
				}
			}
		}

		public ReadOnlyCollection<RecordTrigger> Triggers
		{
			get
			{
				return new ReadOnlyCollection<RecordTrigger>(this.mTriggerList);
			}
		}

		public ReadOnlyCollection<RecordTrigger> ActiveTriggers
		{
			get
			{
				return new ReadOnlyCollection<RecordTrigger>((from RecordTrigger trigger in this.mTriggerList
				where trigger.IsActive.Value
				select trigger).ToList<RecordTrigger>());
			}
		}

		public IList<VoCanRecordingEvent> GetActiveVoCanRecordingEventsInTriggers
		{
			get
			{
				List<VoCanRecordingEvent> list = new List<VoCanRecordingEvent>();
				foreach (RecordTrigger current in this.mTriggerList)
				{
					if (current.IsActive.Value)
					{
						if (current.Event is VoCanRecordingEvent)
						{
							list.Add(current.Event as VoCanRecordingEvent);
						}
						else if (current.Event is CombinedEvent)
						{
							list.AddRange((current.Event as CombinedEvent).GetActiveChildVoCANEvents());
						}
					}
				}
				return list;
			}
		}

		public int GetNumOfActiveCasKeyEventsInTriggers
		{
			get
			{
				int num = 0;
				foreach (RecordTrigger current in this.mTriggerList)
				{
					if (current.IsActive.Value)
					{
						if (current.Event is KeyEvent && (current.Event as KeyEvent).IsCasKey)
						{
							num++;
						}
						else if (current.Event is CombinedEvent)
						{
							num += (current.Event as CombinedEvent).GetNumberOfActiveChildCasKeyEvents();
						}
					}
				}
				return num;
			}
		}

		public ReadOnlyCollection<RecordTrigger> PermanentMarkers
		{
			get
			{
				return new ReadOnlyCollection<RecordTrigger>(this.mTriggerListPermanent);
			}
		}

		public ReadOnlyCollection<RecordTrigger> ActivePermanentMarkers
		{
			get
			{
				return new ReadOnlyCollection<RecordTrigger>((from RecordTrigger trigger in this.mTriggerListPermanent
				where trigger.IsActive.Value
				select trigger).ToList<RecordTrigger>());
			}
		}

		public IList<VoCanRecordingEvent> GetActiveVoCanRecordingEventsInPermanentMarkers
		{
			get
			{
				List<VoCanRecordingEvent> list = new List<VoCanRecordingEvent>();
				foreach (RecordTrigger current in this.mTriggerListPermanent)
				{
					if (current.IsActive.Value)
					{
						if (current.Event is VoCanRecordingEvent)
						{
							list.Add(current.Event as VoCanRecordingEvent);
						}
						else if (current.Event is CombinedEvent)
						{
							list.AddRange((current.Event as CombinedEvent).GetActiveChildVoCANEvents());
						}
					}
				}
				return list;
			}
		}

		public int GetNumOfActiveCasKeyEventsInPermanentMarkers
		{
			get
			{
				int num = 0;
				foreach (RecordTrigger current in this.mTriggerListPermanent)
				{
					if (current.IsActive.Value)
					{
						if (current.Event is KeyEvent && (current.Event as KeyEvent).IsCasKey)
						{
							num++;
						}
						else if (current.Event is CombinedEvent)
						{
							num += (current.Event as CombinedEvent).GetNumberOfActiveChildCasKeyEvents();
						}
					}
				}
				return num;
			}
		}

		public ReadOnlyCollection<RecordTrigger> OnOffTriggers
		{
			get
			{
				return new ReadOnlyCollection<RecordTrigger>(this.mTriggerListOnOff);
			}
		}

		public ReadOnlyCollection<RecordTrigger> ActiveOnOffTriggers
		{
			get
			{
				return new ReadOnlyCollection<RecordTrigger>((from RecordTrigger trigger in this.mTriggerListOnOff
				where trigger.IsActive.Value
				select trigger).ToList<RecordTrigger>());
			}
		}

		public ReadOnlyCollection<RecordTrigger> OnOffTriggersOnly
		{
			get
			{
				return new ReadOnlyCollection<RecordTrigger>((from RecordTrigger trigger in this.mTriggerListOnOff
				where trigger.TriggerEffect.Value != TriggerEffect.Marker
				select trigger).ToList<RecordTrigger>());
			}
		}

		public ReadOnlyCollection<RecordTrigger> ActiveOnOffMarkersOnly
		{
			get
			{
				return new ReadOnlyCollection<RecordTrigger>((from RecordTrigger trigger in this.mTriggerListOnOff
				where trigger.IsActive.Value && trigger.TriggerEffect.Value == TriggerEffect.Marker
				select trigger).ToList<RecordTrigger>());
			}
		}

		public IList<VoCanRecordingEvent> GetActiveVoCanRecordingEventsInOnOffTriggers
		{
			get
			{
				List<VoCanRecordingEvent> list = new List<VoCanRecordingEvent>();
				foreach (RecordTrigger current in this.mTriggerListOnOff)
				{
					if (current.IsActive.Value)
					{
						if (current.Event is VoCanRecordingEvent)
						{
							list.Add(current.Event as VoCanRecordingEvent);
						}
						else if (current.Event is CombinedEvent)
						{
							list.AddRange((current.Event as CombinedEvent).GetActiveChildVoCANEvents());
						}
					}
				}
				return list;
			}
		}

		public int GetNumOfActiveCasKeyEventsInOnOffTriggers
		{
			get
			{
				int num = 0;
				foreach (RecordTrigger current in this.mTriggerListOnOff)
				{
					if (current.IsActive.Value)
					{
						if (current.Event is KeyEvent && (current.Event as KeyEvent).IsCasKey)
						{
							num++;
						}
						else if (current.Event is CombinedEvent)
						{
							num += (current.Event as CombinedEvent).GetNumberOfActiveChildCasKeyEvents();
						}
					}
				}
				return num;
			}
		}

		public MemoryRingBuffer MemoryRingBuffer
		{
			get
			{
				return this.mEmoryRingBuffer;
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is TriggerConfiguration || otherFeature is CANChannelConfiguration || otherFeature is LINChannelConfiguration || otherFeature is FlexrayChannelConfiguration || otherFeature is MultibusChannelConfiguration || otherFeature is DatabaseConfiguration || otherFeature is CcpXcpSignalConfiguration || otherFeature is LEDConfiguration || otherFeature is DigitalOutputsConfiguration || otherFeature is SendMessageConfiguration || otherFeature is WLANConfiguration || otherFeature is IncludeFileConfiguration)
			{
				updateService.Notify<TriggerConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<TriggerConfiguration>(this);
		}

		private static void AddSymbolicMessages(ref List<ISymbolicMessage> symMessages, List<RecordTrigger> recTriggers)
		{
			foreach (RecordTrigger current in recTriggers)
			{
				if (current.Event is ISymbolicMessage)
				{
					if (current.Event is MsgTimeoutEvent)
					{
						if ((current.Event as MsgTimeoutEvent).IsSymbolic.Value)
						{
							symMessages.Add(current.Event as ISymbolicMessage);
						}
					}
					else
					{
						symMessages.Add(current.Event as ISymbolicMessage);
					}
				}
				else if (current.Event is CombinedEvent)
				{
					TriggerConfiguration.AddSymbolicMessages(ref symMessages, current.Event as CombinedEvent);
				}
			}
		}

		private static void AddSymbolicMessages(ref List<ISymbolicMessage> symMessages, IList<Event> eventList)
		{
			foreach (Event current in eventList)
			{
				if (current is ISymbolicMessage)
				{
					if (current is MsgTimeoutEvent)
					{
						if ((current as MsgTimeoutEvent).IsSymbolic.Value)
						{
							symMessages.Add(current as ISymbolicMessage);
						}
					}
					else
					{
						symMessages.Add(current as ISymbolicMessage);
					}
				}
				else if (current is CombinedEvent)
				{
					TriggerConfiguration.AddSymbolicMessages(ref symMessages, current as CombinedEvent);
				}
			}
		}

		private static void AddSymbolicSignals(ref List<ISymbolicSignal> symSignals, List<RecordTrigger> recTriggers)
		{
			foreach (RecordTrigger current in recTriggers)
			{
				if (current.Event is ISymbolicSignal)
				{
					symSignals.Add(current.Event as ISymbolicSignal);
				}
				else if (current.Event is CombinedEvent)
				{
					TriggerConfiguration.AddSymbolicSignals(ref symSignals, current.Event as CombinedEvent);
				}
			}
		}

		private static void AddSymbolicSignals(ref List<ISymbolicSignal> symSignals, IList<Event> eventList)
		{
			foreach (Event current in eventList)
			{
				if (current is ISymbolicSignal)
				{
					symSignals.Add(current as ISymbolicSignal);
				}
				else if (current is CombinedEvent)
				{
					TriggerConfiguration.AddSymbolicSignals(ref symSignals, current as CombinedEvent);
				}
			}
		}

		private static void AddCcpXcpSignals(ref List<ICcpXcpSignal> ccpXcpSignals, List<RecordTrigger> recTriggers)
		{
			foreach (RecordTrigger current in recTriggers)
			{
				if (current.Event is ICcpXcpSignal)
				{
					ccpXcpSignals.Add(current.Event as ICcpXcpSignal);
				}
				else if (current.Event is CombinedEvent)
				{
					TriggerConfiguration.AddCcpXcpSignals(ref ccpXcpSignals, current.Event as CombinedEvent);
				}
			}
		}

		private static void AddCcpXcpSignals(ref List<ICcpXcpSignal> ccpXcpSignals, IList<Event> eventList)
		{
			foreach (Event current in eventList)
			{
				if (current is ICcpXcpSignal)
				{
					ccpXcpSignals.Add(current as ICcpXcpSignal);
				}
				else if (current is CombinedEvent)
				{
					TriggerConfiguration.AddCcpXcpSignals(ref ccpXcpSignals, current as CombinedEvent);
				}
			}
		}

		IList<Event> IFeatureEvents.GetEvents(bool activeEventsOnly)
		{
			if (this.events == null)
			{
				this.events = new List<Event>();
			}
			this.events.Clear();
			if (this.MemoryRingBuffer.IsActive.Value || !activeEventsOnly)
			{
				IEnumerable<RecordTrigger> enumerable;
				if (activeEventsOnly)
				{
					enumerable = this.GetAllActiveRecordTriggers();
				}
				else
				{
					enumerable = this.GetAllRecordTriggers();
				}
				foreach (RecordTrigger current in enumerable)
				{
					if (!activeEventsOnly || current.IsActive.Value)
					{
						this.events.Add(current.Event);
						CombinedEvent combinedEvent = current.Event as CombinedEvent;
						if (combinedEvent != null)
						{
							if (activeEventsOnly)
							{
								using (IEnumerator<Event> enumerator2 = combinedEvent.GetAllActiveChildren().GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										Event current2 = enumerator2.Current;
										this.events.Add(current2);
									}
									continue;
								}
							}
							foreach (Event current3 in combinedEvent.GetAllChildren())
							{
								this.events.Add(current3);
							}
						}
					}
				}
			}
			return this.events;
		}

		public TriggerConfiguration(int memNr)
		{
			this.MemoryNr = memNr;
			this.mTriggerList = new List<RecordTrigger>();
			this.mTriggerListOnOff = new List<RecordTrigger>();
			this.mTriggerListPermanent = new List<RecordTrigger>();
			this.TriggerMode = new ValidatedProperty<TriggerMode>(Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Triggered);
			this.PostTriggerTime = new ValidatedProperty<uint>(0u);
			this.mEmoryRingBuffer = new MemoryRingBuffer();
		}

		public IEnumerable<RecordTrigger> GetAllActiveRecordTriggers()
		{
			switch (this.TriggerMode.Value)
			{
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Triggered:
				foreach (RecordTrigger current in from recordTrigger in this.mTriggerList
				where recordTrigger.IsActive.Value
				select recordTrigger)
				{
					yield return current;
				}
				break;
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Permanent:
				foreach (RecordTrigger current2 in from recordTrigger in this.mTriggerListPermanent
				where recordTrigger.IsActive.Value
				select recordTrigger)
				{
					yield return current2;
				}
				break;
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.OnOff:
				foreach (RecordTrigger current3 in from recordTrigger in this.mTriggerListOnOff
				where recordTrigger.IsActive.Value
				select recordTrigger)
				{
					yield return current3;
				}
				break;
			}
			yield break;
		}

		public IEnumerable<RecordTrigger> GetAllRecordTriggers()
		{
			foreach (RecordTrigger current in this.mTriggerList)
			{
				yield return current;
			}
			foreach (RecordTrigger current2 in this.mTriggerListOnOff)
			{
				yield return current2;
			}
			foreach (RecordTrigger current3 in this.mTriggerListPermanent)
			{
				yield return current3;
			}
			yield break;
		}

		public void AddToCurrentList(RecordTrigger trigger)
		{
			switch (this.TriggerMode.Value)
			{
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Triggered:
				this.AddTrigger(trigger);
				return;
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Permanent:
				this.AddPermanentTrigger(trigger);
				return;
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.OnOff:
				this.AddOnOffTrigger(trigger);
				return;
			default:
				return;
			}
		}

		public bool InsertInCurrentList(RecordTrigger trigger, int index)
		{
			switch (this.TriggerMode.Value)
			{
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Triggered:
				return this.InsertTrigger(index, trigger);
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Permanent:
				return this.InsertPermanentTrigger(index, trigger);
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.OnOff:
				return this.InsertOnOffTrigger(index, trigger);
			default:
				return false;
			}
		}

		public bool RemoveFromCurrentList(RecordTrigger trigger)
		{
			switch (this.TriggerMode.Value)
			{
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Triggered:
				return this.RemoveTrigger(trigger);
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Permanent:
				return this.RemovePermanentTrigger(trigger);
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.OnOff:
				return this.RemoveOnOffTrigger(trigger);
			default:
				return false;
			}
		}

		public void ClearCurrentList()
		{
			switch (this.TriggerMode.Value)
			{
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Triggered:
				this.ClearTriggers();
				return;
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Permanent:
				this.ClearPermanentTriggers();
				return;
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.OnOff:
				this.ClearOnOffTriggers();
				return;
			default:
				return;
			}
		}

		public int IndexInCurrentList(RecordTrigger trigger)
		{
			switch (this.TriggerMode.Value)
			{
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Triggered:
				return this.IndexOf(trigger);
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Permanent:
				return this.IndexOfPermanentTrigger(trigger);
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.OnOff:
				return this.IndexOfOnOffTrigger(trigger);
			default:
				return -1;
			}
		}

		public IList<SymbolicMessageEvent> GetSymbolicMessageEventsForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<SymbolicMessageEvent> result = new List<SymbolicMessageEvent>();
			foreach (RecordTrigger current in this.Triggers)
			{
				TriggerConfiguration.GetSymbolicMessageEventsForDatabaseOnChannelRecursive(databasePath, channel, busType, current.Event, ref result);
			}
			return result;
		}

		public IList<SymbolicSignalEvent> GetSymbolicSignalEventsForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<SymbolicSignalEvent> result = new List<SymbolicSignalEvent>();
			foreach (RecordTrigger current in this.Triggers)
			{
				TriggerConfiguration.GetSymbolicSignalEventsForDatabaseOnChannelRecursive(databasePath, channel, busType, current.Event, ref result);
			}
			return result;
		}

		public IList<MsgTimeoutEvent> GetSymbolicMsgTimeoutEventsForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<MsgTimeoutEvent> result = new List<MsgTimeoutEvent>();
			foreach (RecordTrigger current in this.Triggers)
			{
				TriggerConfiguration.GetSymbolicMsgTimeoutEventsForDatabaseOnChannelRecursive(databasePath, channel, busType, current.Event, ref result);
			}
			return result;
		}

		public bool InsertTrigger(int idx, RecordTrigger trigger)
		{
			if (idx >= 0 && idx < this.mTriggerList.Count)
			{
				this.mTriggerList.Insert(idx, trigger);
				return true;
			}
			if (idx == this.mTriggerList.Count)
			{
				this.mTriggerList.Add(trigger);
				return true;
			}
			return false;
		}

		public void AddTrigger(RecordTrigger trigger)
		{
			this.mTriggerList.Add(trigger);
		}

		public RecordTrigger AddCanIdTrigger()
		{
			RecordTrigger recordTrigger = RecordTrigger.CreateCanIdTrigger();
			this.mTriggerList.Add(recordTrigger);
			return recordTrigger;
		}

		public bool RemoveTrigger(RecordTrigger trigger)
		{
			if (!this.mTriggerList.Contains(trigger))
			{
				return false;
			}
			this.mTriggerList.Remove(trigger);
			return true;
		}

		public void ClearTriggers()
		{
			this.mTriggerList.Clear();
		}

		public int IndexOf(RecordTrigger trigger)
		{
			return this.mTriggerList.IndexOf(trigger);
		}

		public IList<SymbolicMessageEvent> GetSymbolicMessageEventsPermMarkerForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<SymbolicMessageEvent> result = new List<SymbolicMessageEvent>();
			foreach (RecordTrigger current in this.PermanentMarkers)
			{
				TriggerConfiguration.GetSymbolicMessageEventsForDatabaseOnChannelRecursive(databasePath, channel, busType, current.Event, ref result);
			}
			return result;
		}

		public IList<SymbolicSignalEvent> GetSymbolicSignalEventsPermMarkerForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<SymbolicSignalEvent> result = new List<SymbolicSignalEvent>();
			foreach (RecordTrigger current in this.PermanentMarkers)
			{
				TriggerConfiguration.GetSymbolicSignalEventsForDatabaseOnChannelRecursive(databasePath, channel, busType, current.Event, ref result);
			}
			return result;
		}

		public IList<MsgTimeoutEvent> GetSymbolicMsgTimeoutEventsPermMarkerForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<MsgTimeoutEvent> result = new List<MsgTimeoutEvent>();
			foreach (RecordTrigger current in this.PermanentMarkers)
			{
				TriggerConfiguration.GetSymbolicMsgTimeoutEventsForDatabaseOnChannelRecursive(databasePath, channel, busType, current.Event, ref result);
			}
			return result;
		}

		public bool InsertPermanentTrigger(int idx, RecordTrigger trigger)
		{
			if (idx >= 0 && idx < this.mTriggerListPermanent.Count)
			{
				this.mTriggerListPermanent.Insert(idx, trigger);
				return true;
			}
			if (idx == this.mTriggerListPermanent.Count)
			{
				this.mTriggerListPermanent.Add(trigger);
				return true;
			}
			return false;
		}

		public void AddPermanentTrigger(RecordTrigger trigger)
		{
			this.mTriggerListPermanent.Add(trigger);
		}

		public bool RemovePermanentTrigger(RecordTrigger trigger)
		{
			if (!this.mTriggerListPermanent.Contains(trigger))
			{
				return false;
			}
			this.mTriggerListPermanent.Remove(trigger);
			return true;
		}

		public void ClearPermanentTriggers()
		{
			this.mTriggerListPermanent.Clear();
		}

		public int IndexOfPermanentTrigger(RecordTrigger trigger)
		{
			return this.mTriggerListPermanent.IndexOf(trigger);
		}

		public IList<SymbolicMessageEvent> GetSymbolicMessageEventsOnOffForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<SymbolicMessageEvent> result = new List<SymbolicMessageEvent>();
			foreach (RecordTrigger current in this.OnOffTriggers)
			{
				TriggerConfiguration.GetSymbolicMessageEventsForDatabaseOnChannelRecursive(databasePath, channel, busType, current.Event, ref result);
			}
			return result;
		}

		public IList<SymbolicSignalEvent> GetSymbolicSignalEventsOnOffForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<SymbolicSignalEvent> result = new List<SymbolicSignalEvent>();
			foreach (RecordTrigger current in this.OnOffTriggers)
			{
				TriggerConfiguration.GetSymbolicSignalEventsForDatabaseOnChannelRecursive(databasePath, channel, busType, current.Event, ref result);
			}
			return result;
		}

		public IList<MsgTimeoutEvent> GetSymbolicMsgTimeoutEventsOnOffForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<MsgTimeoutEvent> result = new List<MsgTimeoutEvent>();
			foreach (RecordTrigger current in this.OnOffTriggers)
			{
				TriggerConfiguration.GetSymbolicMsgTimeoutEventsForDatabaseOnChannelRecursive(databasePath, channel, busType, current.Event, ref result);
			}
			return result;
		}

		public bool InsertOnOffTrigger(int idx, RecordTrigger trigger)
		{
			if (idx >= 0 && idx < this.mTriggerListOnOff.Count)
			{
				this.mTriggerListOnOff.Insert(idx, trigger);
				return true;
			}
			if (idx == this.mTriggerListOnOff.Count)
			{
				this.mTriggerListOnOff.Add(trigger);
				return true;
			}
			return false;
		}

		public void AddOnOffTrigger(RecordTrigger trigger)
		{
			this.mTriggerListOnOff.Add(trigger);
		}

		public bool RemoveOnOffTrigger(RecordTrigger trigger)
		{
			if (!this.mTriggerListOnOff.Contains(trigger))
			{
				return false;
			}
			this.mTriggerListOnOff.Remove(trigger);
			return true;
		}

		public void ClearOnOffTriggers()
		{
			this.mTriggerListOnOff.Clear();
		}

		public int IndexOfOnOffTrigger(RecordTrigger trigger)
		{
			return this.mTriggerListOnOff.IndexOf(trigger);
		}

		private static void GetSymbolicMessageEventsForDatabaseOnChannelRecursive(string databasePath, uint channel, BusType busType, Event eventNode, ref List<SymbolicMessageEvent> symMsgEventList)
		{
			SymbolicMessageEvent symbolicMessageEvent = eventNode as SymbolicMessageEvent;
			if (symbolicMessageEvent != null)
			{
				if (string.Compare(symbolicMessageEvent.DatabasePath.Value, databasePath, StringComparison.OrdinalIgnoreCase) == 0 && ((symbolicMessageEvent.ChannelNumber.Value == channel && symbolicMessageEvent.BusType.Value == busType) || (symbolicMessageEvent.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
				{
					symMsgEventList.Add(symbolicMessageEvent);
					return;
				}
			}
			else if (eventNode is CombinedEvent)
			{
				CombinedEvent combinedEvent = eventNode as CombinedEvent;
				foreach (Event current in combinedEvent)
				{
					TriggerConfiguration.GetSymbolicMessageEventsForDatabaseOnChannelRecursive(databasePath, channel, busType, current, ref symMsgEventList);
				}
			}
		}

		private static void GetSymbolicSignalEventsForDatabaseOnChannelRecursive(string databasePath, uint channel, BusType busType, Event eventNode, ref List<SymbolicSignalEvent> symSigEventList)
		{
			SymbolicSignalEvent symbolicSignalEvent = eventNode as SymbolicSignalEvent;
			if (symbolicSignalEvent != null)
			{
				if (string.Compare(symbolicSignalEvent.DatabasePath.Value, databasePath, StringComparison.OrdinalIgnoreCase) == 0 && ((symbolicSignalEvent.ChannelNumber.Value == channel && symbolicSignalEvent.BusType.Value == busType) || (symbolicSignalEvent.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
				{
					symSigEventList.Add(symbolicSignalEvent);
					return;
				}
			}
			else if (eventNode is CombinedEvent)
			{
				CombinedEvent combinedEvent = eventNode as CombinedEvent;
				foreach (Event current in combinedEvent)
				{
					TriggerConfiguration.GetSymbolicSignalEventsForDatabaseOnChannelRecursive(databasePath, channel, busType, current, ref symSigEventList);
				}
			}
		}

		private static void GetSymbolicMsgTimeoutEventsForDatabaseOnChannelRecursive(string databasePath, uint channel, BusType busType, Event eventNode, ref List<MsgTimeoutEvent> symMsgTimeoutEventList)
		{
			MsgTimeoutEvent msgTimeoutEvent = eventNode as MsgTimeoutEvent;
			if (msgTimeoutEvent != null)
			{
				if (msgTimeoutEvent.IsSymbolic.Value && string.Compare(msgTimeoutEvent.DatabasePath.Value, databasePath, StringComparison.OrdinalIgnoreCase) == 0 && ((msgTimeoutEvent.ChannelNumber.Value == channel && msgTimeoutEvent.BusType.Value == busType) || (msgTimeoutEvent.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
				{
					symMsgTimeoutEventList.Add(msgTimeoutEvent);
					return;
				}
			}
			else if (eventNode is CombinedEvent)
			{
				CombinedEvent combinedEvent = eventNode as CombinedEvent;
				foreach (Event current in combinedEvent)
				{
					TriggerConfiguration.GetSymbolicMsgTimeoutEventsForDatabaseOnChannelRecursive(databasePath, channel, busType, current, ref symMsgTimeoutEventList);
				}
			}
		}

		public void SetUniqueIdsInEvents()
		{
			foreach (RecordTrigger current in this.mTriggerList)
			{
				current.SetUniqueId();
			}
			foreach (RecordTrigger current2 in this.mTriggerListOnOff)
			{
				current2.SetUniqueId();
			}
			foreach (RecordTrigger current3 in this.mTriggerListPermanent)
			{
				current3.SetUniqueId();
			}
		}

		public IList<ulong> GetUniqueIdsOfTriggersForName(string triggerName, TriggerMode triggerMode)
		{
			List<ulong> list = new List<ulong>();
			switch (triggerMode)
			{
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Triggered:
				using (List<RecordTrigger>.Enumerator enumerator = this.mTriggerList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						RecordTrigger current = enumerator.Current;
						if (current.Name.Value == triggerName)
						{
							list.Add(current.Event.Id);
						}
					}
					return list;
				}
				break;
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.Permanent:
				goto IL_DB;
			case Vector.VLConfig.Data.ConfigurationDataModel.TriggerMode.OnOff:
				break;
			default:
				return list;
			}
			using (List<RecordTrigger>.Enumerator enumerator2 = this.mTriggerListOnOff.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RecordTrigger current2 = enumerator2.Current;
					if (current2.Name.Value == triggerName)
					{
						list.Add(current2.Event.Id);
					}
				}
				return list;
			}
			IL_DB:
			foreach (RecordTrigger current3 in this.mTriggerListPermanent)
			{
				if (current3.Name.Value == triggerName)
				{
					list.Add(current3.Event.Id);
				}
			}
			return list;
		}
	}
}
