using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "OnStartEvent")]
	public class OnStartEvent : Event
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

		[DataMember(Name = "OnStartEventDelay")]
		public ValidatedProperty<uint> Delay
		{
			get;
			set;
		}

		public OnStartEvent()
		{
			this.Delay = new ValidatedProperty<uint>(0u);
		}

		public OnStartEvent(OnStartEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new OnStartEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			OnStartEvent onStartEvent = (OnStartEvent)obj;
			return this.Delay.Value == onStartEvent.Delay.Value;
		}

		public override int GetHashCode()
		{
			return this.Delay.Value.GetHashCode();
		}

		public void Assign(OnStartEvent other)
		{
			this.Delay.Value = other.Delay.Value;
		}
	}
}
