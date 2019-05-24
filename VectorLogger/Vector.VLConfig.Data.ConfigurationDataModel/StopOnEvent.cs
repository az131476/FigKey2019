using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "StopOnEvent")]
	public class StopOnEvent : StopType
	{
		[DataMember(Name = "StopOnEventEvent")]
		public Event Event;

		public StopOnEvent()
		{
			this.Event = null;
		}

		public StopOnEvent(Event ev)
		{
			this.Event = ev;
		}

		public StopOnEvent(StopOnEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new StopOnEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			StopOnEvent stopOnEvent = (StopOnEvent)obj;
			return !this.Event.Equals(stopOnEvent.Event);
		}

		public override int GetHashCode()
		{
			return this.Event.GetHashCode();
		}

		public void Assign(StopOnEvent other)
		{
			this.Event = other.Event;
		}
	}
}
