using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "IgnitionEvent")]
	public class IgnitionEvent : Event
	{
		private ValidatedProperty<bool> mIsPointInTime;

		public override ValidatedProperty<bool> IsPointInTime
		{
			get
			{
				ValidatedProperty<bool> arg_1C_0;
				if ((arg_1C_0 = this.mIsPointInTime) == null)
				{
					arg_1C_0 = (this.mIsPointInTime = new ValidatedProperty<bool>(false));
				}
				return arg_1C_0;
			}
		}

		[DataMember(Name = "IgnitionEventIsOn")]
		public ValidatedProperty<bool> IsOn
		{
			get;
			set;
		}

		public IgnitionEvent()
		{
			this.IsOn = new ValidatedProperty<bool>(true);
		}

		public IgnitionEvent(IgnitionEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new IgnitionEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			IgnitionEvent ignitionEvent = (IgnitionEvent)obj;
			return this.IsOn.Value == ignitionEvent.IsOn.Value;
		}

		public override int GetHashCode()
		{
			return this.IsOn.Value.GetHashCode();
		}

		public void Assign(IgnitionEvent other)
		{
			this.IsOn.Value = other.IsOn.Value;
		}
	}
}
