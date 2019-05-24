using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "StopOnTimer")]
	public class StopOnTimer : StopType
	{
		[DataMember(Name = "StopOnTimerDuration")]
		public ValidatedProperty<uint> Duration;

		public StopOnTimer(uint defaultDuration)
		{
			this.Duration = new ValidatedProperty<uint>(defaultDuration);
		}

		public StopOnTimer(StopOnTimer other) : this(0u)
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new StopOnTimer(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			StopOnTimer stopOnTimer = (StopOnTimer)obj;
			return this.Duration.Value == stopOnTimer.Duration.Value;
		}

		public override int GetHashCode()
		{
			return this.Duration.Value.GetHashCode();
		}

		public void Assign(StopOnTimer other)
		{
			this.Duration.Value = other.Duration.Value;
		}
	}
}
