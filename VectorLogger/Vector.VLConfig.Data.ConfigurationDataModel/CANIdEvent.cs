using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CANIdEvent")]
	public class CANIdEvent : IdEvent
	{
		[DataMember(Name = "CANIdEventIsExtendedId")]
		public ValidatedProperty<bool> IsExtendedId;

		public CANIdEvent()
		{
			this.IsExtendedId = new ValidatedProperty<bool>();
		}

		public CANIdEvent(CANIdEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new CANIdEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			if (!base.Equals(obj))
			{
				return false;
			}
			CANIdEvent cANIdEvent = (CANIdEvent)obj;
			return this.IsExtendedId.Value == cANIdEvent.IsExtendedId.Value;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.IsExtendedId.Value.GetHashCode();
		}

		public void Assign(CANIdEvent other)
		{
			base.Assign(other);
			this.IsExtendedId.Value = other.IsExtendedId.Value;
		}
	}
}
