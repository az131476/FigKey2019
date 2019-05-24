using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "StopOnStartEvent")]
	public class StopOnStartEvent : StopType
	{
		[DataMember(Name = "StopOnStartEventIsStopOnResetOfStartEvent")]
		public ValidatedProperty<bool> IsStopOnResetOfStartEvent;

		public StopOnStartEvent()
		{
			this.IsStopOnResetOfStartEvent = new ValidatedProperty<bool>(true);
		}

		public StopOnStartEvent(bool isOnResetOfStartEvent)
		{
			this.IsStopOnResetOfStartEvent = new ValidatedProperty<bool>(isOnResetOfStartEvent);
		}

		public StopOnStartEvent(StopOnStartEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new StopOnStartEvent(this);
		}

		public override bool Equals(object obj)
		{
			return obj != null && !(base.GetType() != obj.GetType()) && obj is StopOnStartEvent && this.IsStopOnResetOfStartEvent.Value == (obj as StopOnStartEvent).IsStopOnResetOfStartEvent.Value;
		}

		public override int GetHashCode()
		{
			return this.IsStopOnResetOfStartEvent.Value.GetHashCode();
		}

		public void Assign(StopOnStartEvent other)
		{
			this.IsStopOnResetOfStartEvent.Value = other.IsStopOnResetOfStartEvent.Value;
		}
	}
}
