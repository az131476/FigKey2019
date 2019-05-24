using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CyclicTimerEvent")]
	public class CyclicTimerEvent : Event
	{
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

		[DataMember(Name = "CyclicTimerEventDelayCycles")]
		public ValidatedProperty<uint> DelayCycles
		{
			get;
			set;
		}

		[DataMember(Name = "CyclicTimerEventTimeUnit")]
		public ValidatedProperty<TimeUnit> TimeUnit
		{
			get;
			set;
		}

		[DataMember(Name = "CyclicTimerEventTimerInterval")]
		public ValidatedProperty<uint> Interval
		{
			get;
			set;
		}

		public CyclicTimerEvent()
		{
			this.DelayCycles = new ValidatedProperty<uint>(0u);
			this.Interval = new ValidatedProperty<uint>(1000u);
			this.TimeUnit = new ValidatedProperty<TimeUnit>(Vector.VLConfig.Data.ConfigurationDataModel.TimeUnit.MilliSec);
		}

		public CyclicTimerEvent(CyclicTimerEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new CyclicTimerEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			CyclicTimerEvent cyclicTimerEvent = (CyclicTimerEvent)obj;
			return this.DelayCycles.Value == cyclicTimerEvent.DelayCycles.Value && this.Interval.Value == cyclicTimerEvent.Interval.Value && this.TimeUnit.Value == cyclicTimerEvent.TimeUnit.Value;
		}

		public override int GetHashCode()
		{
			return this.DelayCycles.Value.GetHashCode() ^ this.Interval.Value.GetHashCode() ^ this.TimeUnit.Value.GetHashCode();
		}

		public void Assign(CyclicTimerEvent other)
		{
			this.DelayCycles.Value = other.DelayCycles.Value;
			this.Interval.Value = other.Interval.Value;
			this.TimeUnit.Value = other.TimeUnit.Value;
		}

		public uint CyclicTimeInMilliSec()
		{
			uint num = this.Interval.Value;
			if (this.TimeUnit.Value == Vector.VLConfig.Data.ConfigurationDataModel.TimeUnit.Min)
			{
				num *= 60000u;
			}
			else if (this.TimeUnit.Value == Vector.VLConfig.Data.ConfigurationDataModel.TimeUnit.Sec)
			{
				num *= 1000u;
			}
			return num;
		}
	}
}
