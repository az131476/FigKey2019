using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "FlexrayIdEvent")]
	public class FlexrayIdEvent : IdEvent
	{
		[DataMember(Name = "FlexrayIdEventBaseCycle")]
		public ValidatedProperty<uint> BaseCycle
		{
			get;
			set;
		}

		[DataMember(Name = "FlexrayIdEventCycleRepetition")]
		public ValidatedProperty<uint> CycleRepetition
		{
			get;
			set;
		}

		public FlexrayIdEvent()
		{
			this.BaseCycle = new ValidatedProperty<uint>(0u);
			this.CycleRepetition = new ValidatedProperty<uint>(1u);
		}

		public FlexrayIdEvent(FlexrayIdEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new FlexrayIdEvent(this);
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
			FlexrayIdEvent flexrayIdEvent = (FlexrayIdEvent)obj;
			return this.BaseCycle.Value == flexrayIdEvent.BaseCycle.Value && this.CycleRepetition.Value == flexrayIdEvent.CycleRepetition.Value;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode() ^ this.BaseCycle.Value.GetHashCode() ^ this.CycleRepetition.Value.GetHashCode();
		}

		public void Assign(FlexrayIdEvent other)
		{
			base.Assign(other);
			this.BaseCycle.Value = other.BaseCycle.Value;
			this.CycleRepetition.Value = other.CycleRepetition.Value;
		}
	}
}
