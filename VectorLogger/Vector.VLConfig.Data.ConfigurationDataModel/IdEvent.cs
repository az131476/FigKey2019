using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "IdEvent")]
	public abstract class IdEvent : Event
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

		[DataMember(Name = "IdEventChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "IdEventIdRelation")]
		public ValidatedProperty<CondRelation> IdRelation
		{
			get;
			set;
		}

		[DataMember(Name = "IdEventLowId")]
		public ValidatedProperty<uint> LowId
		{
			get;
			set;
		}

		[DataMember(Name = "IdEventHighId")]
		public ValidatedProperty<uint> HighId
		{
			get;
			set;
		}

		protected IdEvent()
		{
			this.ChannelNumber = new ValidatedProperty<uint>(1u);
			this.IdRelation = new ValidatedProperty<CondRelation>(CondRelation.Equal);
			this.LowId = new ValidatedProperty<uint>(0u);
			this.HighId = new ValidatedProperty<uint>(0u);
		}

		protected IdEvent(IdEvent other) : this()
		{
			this.Assign(other);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			IdEvent idEvent = (IdEvent)obj;
			return this.ChannelNumber.Value == idEvent.ChannelNumber.Value && this.IdRelation.Value == idEvent.IdRelation.Value && this.LowId.Value == idEvent.LowId.Value && this.HighId.Value == idEvent.HighId.Value;
		}

		public override int GetHashCode()
		{
			return this.ChannelNumber.Value.GetHashCode() ^ this.IdRelation.Value.GetHashCode() ^ this.LowId.Value.GetHashCode() ^ this.HighId.Value.GetHashCode();
		}

		public void Assign(IdEvent other)
		{
			this.ChannelNumber.Value = other.ChannelNumber.Value;
			this.IdRelation.Value = other.IdRelation.Value;
			this.LowId.Value = other.LowId.Value;
			this.HighId.Value = other.HighId.Value;
		}
	}
}
