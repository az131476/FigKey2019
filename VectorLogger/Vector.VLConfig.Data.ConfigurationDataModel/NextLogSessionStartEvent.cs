using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "NextLogSessionStartEvent")]
	public class NextLogSessionStartEvent : Event
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

		[DataMember(Name = "NextLogSessionStartEventDelay")]
		public ValidatedProperty<uint> Delay
		{
			get;
			set;
		}

		public NextLogSessionStartEvent()
		{
			this.Delay = new ValidatedProperty<uint>(0u);
		}

		public NextLogSessionStartEvent(NextLogSessionStartEvent other) : this()
		{
			this.Delay.Value = other.Delay.Value;
		}

		public override object Clone()
		{
			return new NextLogSessionStartEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			NextLogSessionStartEvent nextLogSessionStartEvent = (NextLogSessionStartEvent)obj;
			return this.Delay.Value == nextLogSessionStartEvent.Delay.Value;
		}

		public override int GetHashCode()
		{
			return this.Delay.Value.GetHashCode();
		}

		public void Assign(NextLogSessionStartEvent other)
		{
			this.Delay.Value = other.Delay.Value;
		}
	}
}
