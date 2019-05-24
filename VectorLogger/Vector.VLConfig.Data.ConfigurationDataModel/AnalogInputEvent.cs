using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "AnalogInputEvent")]
	public class AnalogInputEvent : Event
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

		[DataMember(Name = "AnalogInputEventInputNumber")]
		public ValidatedProperty<uint> InputNumber
		{
			get;
			set;
		}

		[DataMember(Name = "AnalogInputEventRelation")]
		public ValidatedProperty<CondRelation> Relation
		{
			get;
			set;
		}

		[DataMember(Name = "AnalogInputEventLowValue")]
		public ValidatedProperty<uint> LowValue
		{
			get;
			set;
		}

		[DataMember(Name = "AnalogInputEventHighValue")]
		public ValidatedProperty<uint> HighValue
		{
			get;
			set;
		}

		[DataMember(Name = "AnalogInputEventTolerance")]
		public ValidatedProperty<uint> Tolerance
		{
			get;
			set;
		}

		public AnalogInputEvent()
		{
			this.InputNumber = new ValidatedProperty<uint>(1u);
			this.Relation = new ValidatedProperty<CondRelation>(CondRelation.Equal);
			this.LowValue = new ValidatedProperty<uint>(0u);
			this.HighValue = new ValidatedProperty<uint>(0u);
			this.Tolerance = new ValidatedProperty<uint>(0u);
		}

		public AnalogInputEvent(AnalogInputEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new AnalogInputEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			AnalogInputEvent analogInputEvent = (AnalogInputEvent)obj;
			return this.InputNumber.Value == analogInputEvent.InputNumber.Value && this.Relation.Value == analogInputEvent.Relation.Value && this.LowValue.Value == analogInputEvent.LowValue.Value && this.HighValue.Value == analogInputEvent.HighValue.Value && this.Tolerance.Value == analogInputEvent.Tolerance.Value;
		}

		public override int GetHashCode()
		{
			return this.InputNumber.Value.GetHashCode() ^ this.Relation.Value.GetHashCode() ^ this.LowValue.Value.GetHashCode() ^ this.HighValue.Value.GetHashCode() ^ this.Tolerance.Value.GetHashCode();
		}

		public void Assign(AnalogInputEvent other)
		{
			this.InputNumber.Value = other.InputNumber.Value;
			this.Relation.Value = other.Relation.Value;
			this.LowValue.Value = other.LowValue.Value;
			this.HighValue.Value = other.HighValue.Value;
			this.Tolerance.Value = other.Tolerance.Value;
		}
	}
}
