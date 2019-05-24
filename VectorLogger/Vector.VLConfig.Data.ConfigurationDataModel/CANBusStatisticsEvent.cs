using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CANBusStatisticsEvent")]
	public class CANBusStatisticsEvent : Event
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

		[DataMember(Name = "CANBusStatisticsEventChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "CANBusStatisticsEventIsBusloadEnabled")]
		public ValidatedProperty<bool> IsBusloadEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "CANBusStatisticsEventBusloadRelation")]
		public ValidatedProperty<CondRelation> BusloadRelation
		{
			get;
			set;
		}

		[DataMember(Name = "CANBusStatisticsEventBusloadLow")]
		public ValidatedProperty<uint> BusloadLow
		{
			get;
			set;
		}

		[DataMember(Name = "CANBusStatisticsEventBusloadHigh")]
		public ValidatedProperty<uint> BusloadHigh
		{
			get;
			set;
		}

		[DataMember(Name = "CANBusStatisticsEventIsErrorFramesEnabled")]
		public ValidatedProperty<bool> IsErrorFramesEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "CANBusStatisticsEventErrorFramesRelation")]
		public ValidatedProperty<CondRelation> ErrorFramesRelation
		{
			get;
			set;
		}

		[DataMember(Name = "CANBusStatisticsEventErrorFramesLow")]
		public ValidatedProperty<uint> ErrorFramesLow
		{
			get;
			set;
		}

		[DataMember(Name = "CANBusStatisticsEventErrorFramesHigh")]
		public ValidatedProperty<uint> ErrorFramesHigh
		{
			get;
			set;
		}

		[DataMember(Name = "CANBusStatisticsEventIsANDConjunction")]
		public ValidatedProperty<bool> IsANDConjunction
		{
			get;
			set;
		}

		public CANBusStatisticsEvent()
		{
			this.ChannelNumber = new ValidatedProperty<uint>(1u);
			this.IsBusloadEnabled = new ValidatedProperty<bool>(true);
			this.BusloadRelation = new ValidatedProperty<CondRelation>(CondRelation.InRange);
			this.BusloadLow = new ValidatedProperty<uint>(50u);
			this.BusloadHigh = new ValidatedProperty<uint>(100u);
			this.IsErrorFramesEnabled = new ValidatedProperty<bool>(true);
			this.ErrorFramesRelation = new ValidatedProperty<CondRelation>(CondRelation.GreaterThanOrEqual);
			this.ErrorFramesLow = new ValidatedProperty<uint>(1u);
			this.ErrorFramesHigh = new ValidatedProperty<uint>(1u);
			this.IsANDConjunction = new ValidatedProperty<bool>(false);
		}

		public CANBusStatisticsEvent(CANBusStatisticsEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new CANBusStatisticsEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			CANBusStatisticsEvent cANBusStatisticsEvent = (CANBusStatisticsEvent)obj;
			return this.ChannelNumber.Value == cANBusStatisticsEvent.ChannelNumber.Value && this.IsBusloadEnabled.Value == cANBusStatisticsEvent.IsBusloadEnabled.Value && this.BusloadRelation.Value == cANBusStatisticsEvent.BusloadRelation.Value && this.BusloadLow.Value == cANBusStatisticsEvent.BusloadLow.Value && this.BusloadHigh.Value == cANBusStatisticsEvent.BusloadHigh.Value && this.IsErrorFramesEnabled.Value == cANBusStatisticsEvent.IsErrorFramesEnabled.Value && this.ErrorFramesRelation.Value == cANBusStatisticsEvent.ErrorFramesRelation.Value && this.ErrorFramesLow.Value == cANBusStatisticsEvent.ErrorFramesLow.Value && this.ErrorFramesHigh.Value == cANBusStatisticsEvent.ErrorFramesHigh.Value && this.IsANDConjunction.Value == cANBusStatisticsEvent.IsANDConjunction.Value;
		}

		public override int GetHashCode()
		{
			return this.ChannelNumber.Value.GetHashCode() ^ this.IsBusloadEnabled.Value.GetHashCode() ^ this.BusloadRelation.Value.GetHashCode() ^ this.BusloadLow.Value.GetHashCode() ^ this.BusloadHigh.Value.GetHashCode() ^ this.IsErrorFramesEnabled.Value.GetHashCode() ^ this.ErrorFramesRelation.Value.GetHashCode() ^ this.ErrorFramesLow.Value.GetHashCode() ^ this.ErrorFramesHigh.Value.GetHashCode() ^ this.IsANDConjunction.Value.GetHashCode();
		}

		public void Assign(CANBusStatisticsEvent other)
		{
			this.ChannelNumber.Value = other.ChannelNumber.Value;
			this.IsBusloadEnabled.Value = other.IsBusloadEnabled.Value;
			this.BusloadRelation.Value = other.BusloadRelation.Value;
			this.BusloadLow.Value = other.BusloadLow.Value;
			this.BusloadHigh.Value = other.BusloadHigh.Value;
			this.IsErrorFramesEnabled.Value = other.IsErrorFramesEnabled.Value;
			this.ErrorFramesRelation.Value = other.ErrorFramesRelation.Value;
			this.ErrorFramesLow.Value = other.ErrorFramesLow.Value;
			this.ErrorFramesHigh.Value = other.ErrorFramesHigh.Value;
			this.IsANDConjunction.Value = other.IsANDConjunction.Value;
		}
	}
}
