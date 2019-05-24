using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CANDataEvent")]
	public class CANDataEvent : Event
	{
		private ValidatedProperty<bool> mIsPointInTime;

		public override ValidatedProperty<bool> IsPointInTime
		{
			get
			{
				if (this.mIsPointInTime == null)
				{
					this.mIsPointInTime = new ValidatedProperty<bool>(false);
				}
				this.mIsPointInTime.Value = (this.Relation.Value == CondRelation.OnChange);
				return this.mIsPointInTime;
			}
		}

		[DataMember(Name = "CANDataEventChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "CANDataEventID")]
		public ValidatedProperty<uint> ID
		{
			get;
			set;
		}

		[DataMember(Name = "CANDataEventIsExtendedId")]
		public ValidatedProperty<bool> IsExtendedId
		{
			get;
			set;
		}

		[DataMember(Name = "CANDataEventRawDataSignal")]
		public RawDataSignal RawDataSignal
		{
			get;
			set;
		}

		[DataMember(Name = "CANDataEventRelation")]
		public ValidatedProperty<CondRelation> Relation
		{
			get;
			set;
		}

		[DataMember(Name = "CANDataEventLowValue")]
		public ValidatedProperty<uint> LowValue
		{
			get;
			set;
		}

		[DataMember(Name = "CANDataEventHighValue")]
		public ValidatedProperty<uint> HighValue
		{
			get;
			set;
		}

		public CANDataEvent()
		{
			this.ChannelNumber = new ValidatedProperty<uint>(1u);
			this.ID = new ValidatedProperty<uint>(0u);
			this.IsExtendedId = new ValidatedProperty<bool>(false);
			this.Relation = new ValidatedProperty<CondRelation>(CondRelation.Equal);
			this.RawDataSignal = new RawDataSignalByte();
			this.LowValue = new ValidatedProperty<uint>(0u);
			this.HighValue = new ValidatedProperty<uint>(0u);
		}

		public CANDataEvent(CANDataEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new CANDataEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			CANDataEvent cANDataEvent = (CANDataEvent)obj;
			if (this.ChannelNumber.Value != cANDataEvent.ChannelNumber.Value)
			{
				return false;
			}
			if (this.ID.Value != cANDataEvent.ID.Value)
			{
				return false;
			}
			if (this.IsExtendedId.Value != cANDataEvent.IsExtendedId.Value)
			{
				return false;
			}
			if (this.Relation.Value != cANDataEvent.Relation.Value)
			{
				return false;
			}
			if (this.RawDataSignal.GetType() != cANDataEvent.RawDataSignal.GetType())
			{
				return false;
			}
			if (this.RawDataSignal is RawDataSignalByte)
			{
				if (!(cANDataEvent.RawDataSignal is RawDataSignalByte))
				{
					return false;
				}
				if ((this.RawDataSignal as RawDataSignalByte).DataBytePos.Value != (cANDataEvent.RawDataSignal as RawDataSignalByte).DataBytePos.Value)
				{
					return false;
				}
			}
			else if (this.RawDataSignal is RawDataSignalStartbitLength)
			{
				RawDataSignalStartbitLength rawDataSignalStartbitLength = this.RawDataSignal as RawDataSignalStartbitLength;
				RawDataSignalStartbitLength rawDataSignalStartbitLength2 = cANDataEvent.RawDataSignal as RawDataSignalStartbitLength;
				if (rawDataSignalStartbitLength2 == null)
				{
					return false;
				}
				if (rawDataSignalStartbitLength.IsMotorola.Value != rawDataSignalStartbitLength2.IsMotorola.Value)
				{
					return false;
				}
				if (rawDataSignalStartbitLength.Length.Value != rawDataSignalStartbitLength2.Length.Value)
				{
					return false;
				}
				if (rawDataSignalStartbitLength.StartbitPos.Value != rawDataSignalStartbitLength2.StartbitPos.Value)
				{
					return false;
				}
			}
			return this.LowValue.Value == cANDataEvent.LowValue.Value && this.HighValue.Value == cANDataEvent.HighValue.Value;
		}

		public override int GetHashCode()
		{
			int num = 0;
			if (this.RawDataSignal is RawDataSignalByte)
			{
				num = (this.RawDataSignal as RawDataSignalByte).DataBytePos.Value.GetHashCode();
			}
			else if (this.RawDataSignal is RawDataSignalStartbitLength)
			{
				RawDataSignalStartbitLength rawDataSignalStartbitLength = this.RawDataSignal as RawDataSignalStartbitLength;
				num = (rawDataSignalStartbitLength.IsMotorola.Value.GetHashCode() ^ rawDataSignalStartbitLength.Length.Value.GetHashCode() ^ rawDataSignalStartbitLength.StartbitPos.Value.GetHashCode());
			}
			return this.ChannelNumber.Value.GetHashCode() ^ this.ID.Value.GetHashCode() ^ this.IsExtendedId.Value.GetHashCode() ^ this.Relation.Value.GetHashCode() ^ num ^ this.LowValue.Value.GetHashCode() ^ this.HighValue.Value.GetHashCode();
		}

		public void Assign(CANDataEvent other)
		{
			this.ChannelNumber.Value = other.ChannelNumber.Value;
			this.ID.Value = other.ID.Value;
			this.IsExtendedId.Value = other.IsExtendedId.Value;
			this.Relation.Value = other.Relation.Value;
			if (other.RawDataSignal is RawDataSignalByte)
			{
				this.RawDataSignal = new RawDataSignalByte
				{
					DataBytePos = 
					{
						Value = (other.RawDataSignal as RawDataSignalByte).DataBytePos.Value
					}
				};
			}
			else if (other.RawDataSignal is RawDataSignalStartbitLength)
			{
				this.RawDataSignal = new RawDataSignalStartbitLength
				{
					IsMotorola = 
					{
						Value = (other.RawDataSignal as RawDataSignalStartbitLength).IsMotorola.Value
					},
					Length = 
					{
						Value = (other.RawDataSignal as RawDataSignalStartbitLength).Length.Value
					},
					StartbitPos = 
					{
						Value = (other.RawDataSignal as RawDataSignalStartbitLength).StartbitPos.Value
					}
				};
			}
			this.LowValue.Value = other.LowValue.Value;
			this.HighValue.Value = other.HighValue.Value;
		}
	}
}
