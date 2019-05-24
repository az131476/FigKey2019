using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "RecordTrigger")]
	public class RecordTrigger : Action
	{
		[DataMember(Name = "RecordTriggerEffect")]
		public ValidatedProperty<TriggerEffect> TriggerEffect
		{
			get;
			set;
		}

		[DataMember(Name = "RecordTriggerAction")]
		public ValidatedProperty<TriggerAction> Action
		{
			get;
			set;
		}

		[DataMember(Name = "RecordTriggerName")]
		public ValidatedProperty<string> Name
		{
			get;
			set;
		}

		public static RecordTrigger Create(Event ev)
		{
			return new RecordTrigger
			{
				Event = ev
			};
		}

		public static RecordTrigger CreateCanDataTrigger()
		{
			return new RecordTrigger
			{
				Event = new CANDataEvent()
			};
		}

		public static RecordTrigger CreateCanIdTrigger()
		{
			return new RecordTrigger
			{
				Event = new CANIdEvent()
			};
		}

		public static RecordTrigger CreateDigitalInputTrigger()
		{
			return new RecordTrigger
			{
				Event = new DigitalInputEvent()
			};
		}

		public static RecordTrigger CreateAnalogInputTrigger()
		{
			return new RecordTrigger
			{
				Event = new AnalogInputEvent()
			};
		}

		public static RecordTrigger CreateFlexrayIdTrigger()
		{
			return new RecordTrigger
			{
				Event = new FlexrayIdEvent()
			};
		}

		public static RecordTrigger CreateKeyTrigger()
		{
			return new RecordTrigger
			{
				Event = new KeyEvent()
			};
		}

		public static RecordTrigger CreateLinDataTrigger()
		{
			return new RecordTrigger
			{
				Event = new LINDataEvent()
			};
		}

		public static RecordTrigger CreateLinIdTrigger()
		{
			return new RecordTrigger
			{
				Event = new LINIdEvent()
			};
		}

		public static RecordTrigger CreateSymbolicMessageTrigger()
		{
			return new RecordTrigger
			{
				Event = new SymbolicMessageEvent()
			};
		}

		public static RecordTrigger CreateSymbolicSignalTrigger()
		{
			return new RecordTrigger
			{
				Event = new SymbolicSignalEvent()
			};
		}

		public static RecordTrigger CreateOnMessageTimeoutTrigger()
		{
			return new RecordTrigger
			{
				Event = new MsgTimeoutEvent()
			};
		}

		public static RecordTrigger CreateCanBusStatisticsTrigger()
		{
			return new RecordTrigger
			{
				Event = new CANBusStatisticsEvent()
			};
		}

		public static RecordTrigger CreateIgnitionTrigger()
		{
			return new RecordTrigger
			{
				Event = new IgnitionEvent()
			};
		}

		public static RecordTrigger CreateVoCanRecordingTrigger()
		{
			return new RecordTrigger
			{
				Event = new VoCanRecordingEvent()
			};
		}

		public static RecordTrigger CreateConditionGroupTrigger()
		{
			return new RecordTrigger
			{
				Event = new CombinedEvent()
			};
		}

		public static RecordTrigger CreateCcpXcpSignalTrigger()
		{
			return new RecordTrigger
			{
				Event = new CcpXcpSignalEvent()
			};
		}

		public static RecordTrigger CreateDiagnosticSignalTrigger()
		{
			return new RecordTrigger
			{
				Event = new DiagnosticSignalEvent()
			};
		}

		public static RecordTrigger CreateIncEventTrigger()
		{
			return new RecordTrigger
			{
				Event = new IncEvent()
			};
		}

		public RecordTrigger(RecordTrigger other)
		{
			this.TriggerEffect = new ValidatedProperty<TriggerEffect>(other.TriggerEffect.Value);
			this.Action = new ValidatedProperty<TriggerAction>(other.Action.Value);
			this.Comment = new ValidatedProperty<string>(other.Comment.Value);
			this.IsActive = new ValidatedProperty<bool>(other.IsActive.Value);
			this.Name = new ValidatedProperty<string>(other.Name.Value);
			this.Event = (Event)other.Event.Clone();
		}

		private RecordTrigger()
		{
			this.TriggerEffect = new ValidatedProperty<TriggerEffect>();
			this.Action = new ValidatedProperty<TriggerAction>();
			this.Comment = new ValidatedProperty<string>();
			this.IsActive = new ValidatedProperty<bool>(true);
			this.Name = new ValidatedProperty<string>();
		}

		public ReadOnlyCollection<Event> GetAllChildren()
		{
			CombinedEvent combinedEvent = this.Event as CombinedEvent;
			IList<Event> list = (combinedEvent != null) ? combinedEvent.GetAllChildren() : new List<Event>();
			return new ReadOnlyCollection<Event>(list);
		}

		public void SetUniqueId()
		{
			if (this.Event != null)
			{
				this.Event.SetUniqueId();
			}
		}
	}
}
