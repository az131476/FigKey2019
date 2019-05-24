using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "ClockTimedEvent")]
	public class ClockTimedEvent : Event
	{
		public static readonly TimeSpan TimeSpan_Daily = new TimeSpan(1, 0, 0, 0);

		public static readonly TimeSpan TimeSpan_8h = new TimeSpan(8, 0, 0);

		public static readonly TimeSpan TimeSpan_4h = new TimeSpan(4, 0, 0);

		public static readonly TimeSpan TimeSpan_2h = new TimeSpan(2, 0, 0);

		public static readonly TimeSpan TimeSpan_1h = new TimeSpan(1, 0, 0);

		public static readonly TimeSpan TimeSpan_30min = new TimeSpan(0, 30, 0);

		public static readonly TimeSpan TimeSpan_15min = new TimeSpan(0, 15, 0);

		private ValidatedProperty<bool> mIsPointInTime;

		public override ValidatedProperty<bool> IsPointInTime
		{
			get
			{
				ValidatedProperty<bool> arg_1C_0;
				if ((arg_1C_0 = this.mIsPointInTime) == null)
				{
					arg_1C_0 = (this.mIsPointInTime = new ValidatedProperty<bool>(true));
				}
				return arg_1C_0;
			}
		}

		[DataMember(Name = "ClockTimedEventStartTime")]
		public ValidatedProperty<DateTime> StartTime
		{
			get;
			set;
		}

		[DataMember(Name = "ClockTimedEventRepetitionIntervalMinutes")]
		public ValidatedProperty<TimeSpan> RepetitionInterval
		{
			get;
			set;
		}

		public ClockTimedEvent()
		{
			this.StartTime = new ValidatedProperty<DateTime>(new DateTime(2010, 1, 1, 0, 0, 0));
			this.RepetitionInterval = new ValidatedProperty<TimeSpan>(ClockTimedEvent.TimeSpan_Daily);
		}

		public ClockTimedEvent(ClockTimedEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new ClockTimedEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			ClockTimedEvent clockTimedEvent = (ClockTimedEvent)obj;
			return !(this.StartTime.Value != clockTimedEvent.StartTime.Value) && !(this.RepetitionInterval.Value != clockTimedEvent.RepetitionInterval.Value);
		}

		public override int GetHashCode()
		{
			return this.StartTime.Value.GetHashCode() ^ this.RepetitionInterval.Value.GetHashCode();
		}

		public void Assign(ClockTimedEvent other)
		{
			this.StartTime.Value = other.StartTime.Value;
			this.RepetitionInterval.Value = other.RepetitionInterval.Value;
		}
	}
}
