using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "LogDataStorage")]
	public class LogDataStorage : Feature, IFeatureSymbolicDefinitions, IFeatureCcpXcpSignalDefinitions, IFeatureEvents
	{
		private IList<ISymbolicMessage> symbolicMsgs;

		private IList<ISymbolicSignal> symbolicSignals;

		private List<DiagnosticAction> diagActions;

		private IList<ICcpXcpSignal> ccpXcpSignals;

		[DataMember(Name = "LogDataStorageStopCyclicCommunicationEvent")]
		public Event StopCyclicCommunicationEvent;

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
				if (this.StopCyclicCommunicationEvent is ISymbolicMessage)
				{
					if (this.StopCyclicCommunicationEvent is MsgTimeoutEvent)
					{
						if ((this.StopCyclicCommunicationEvent as MsgTimeoutEvent).IsSymbolic.Value)
						{
							this.symbolicMsgs.Add(this.StopCyclicCommunicationEvent as ISymbolicMessage);
						}
					}
					else
					{
						this.symbolicMsgs.Add(this.StopCyclicCommunicationEvent as ISymbolicMessage);
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
				if (this.StopCyclicCommunicationEvent is ISymbolicSignal)
				{
					this.symbolicSignals.Add(this.StopCyclicCommunicationEvent as ISymbolicSignal);
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
				if (this.StopCyclicCommunicationEvent is ICcpXcpSignal)
				{
					this.ccpXcpSignals.Add(this.StopCyclicCommunicationEvent as ICcpXcpSignal);
				}
				return this.ccpXcpSignals;
			}
		}

		[DataMember(Name = "LogDataStorageIsEnterSleepModeEnabled")]
		public ValidatedProperty<bool> IsEnterSleepModeEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "LogDataStorageTimeoutToSleep")]
		public ValidatedProperty<uint> TimeoutToSleep
		{
			get;
			set;
		}

		[DataMember(Name = "LogDataStorageIsFastWakeUpEnabled")]
		public ValidatedProperty<bool> IsFastWakeUpEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "LogDataStorageUseDataCompression")]
		public ValidatedProperty<bool> UseDataCompression
		{
			get;
			set;
		}

		[DataMember(Name = "LogDataStorageEventActivationDelayAfterStart")]
		public ValidatedProperty<uint> EventActivationDelayAfterStart
		{
			get;
			set;
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is LogDataStorage || otherFeature is CANChannelConfiguration || otherFeature is LINChannelConfiguration || otherFeature is FlexrayChannelConfiguration || otherFeature is DatabaseConfiguration || otherFeature is CcpXcpSignalConfiguration || otherFeature is DigitalOutputsConfiguration || otherFeature is WLANConfiguration)
			{
				updateService.Notify<LogDataStorage>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<LogDataStorage>(this);
		}

		IList<Event> IFeatureEvents.GetEvents(bool activeEventsOnly)
		{
			IList<Event> list = new List<Event>();
			if (this.StopCyclicCommunicationEvent != null)
			{
				list.Add(this.StopCyclicCommunicationEvent);
			}
			return list;
		}

		public LogDataStorage()
		{
			this.IsEnterSleepModeEnabled = new ValidatedProperty<bool>(false);
			this.TimeoutToSleep = new ValidatedProperty<uint>(0u);
			this.IsFastWakeUpEnabled = new ValidatedProperty<bool>(false);
			this.UseDataCompression = new ValidatedProperty<bool>(false);
			this.EventActivationDelayAfterStart = new ValidatedProperty<uint>(0u);
		}
	}
}
