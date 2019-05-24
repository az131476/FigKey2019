using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "SymbolicMessageEvent")]
	public class SymbolicMessageEvent : Event, ISymbolicMessage
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

		[DataMember(Name = "SymbolicMessageEventBusType")]
		public ValidatedProperty<BusType> BusType
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicMessageEventChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicMessageEventMessageName")]
		public ValidatedProperty<string> MessageName
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicMessageEventNetworkName")]
		public ValidatedProperty<string> NetworkName
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicMessageEventDatabaseName")]
		public ValidatedProperty<string> DatabaseName
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicMessageEventDatabasePath")]
		public ValidatedProperty<string> DatabasePath
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicMessageEventIsFlexrayPDU")]
		public ValidatedProperty<bool> IsFlexrayPDU
		{
			get;
			set;
		}

		public SymbolicMessageEvent()
		{
			this.BusType = new ValidatedProperty<BusType>();
			this.ChannelNumber = new ValidatedProperty<uint>();
			this.MessageName = new ValidatedProperty<string>();
			this.NetworkName = new ValidatedProperty<string>();
			this.DatabaseName = new ValidatedProperty<string>();
			this.DatabasePath = new ValidatedProperty<string>();
			this.IsFlexrayPDU = new ValidatedProperty<bool>(false);
		}

		public SymbolicMessageEvent(SymbolicMessageEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new SymbolicMessageEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			SymbolicMessageEvent symbolicMessageEvent = (SymbolicMessageEvent)obj;
			return this.BusType.Value == symbolicMessageEvent.BusType.Value && this.ChannelNumber.Value == symbolicMessageEvent.ChannelNumber.Value && !(this.MessageName.Value != symbolicMessageEvent.MessageName.Value) && !(this.NetworkName.Value != symbolicMessageEvent.NetworkName.Value) && !(this.DatabaseName.Value != symbolicMessageEvent.DatabaseName.Value) && !(this.DatabasePath.Value != symbolicMessageEvent.DatabasePath.Value) && this.IsFlexrayPDU.Value == symbolicMessageEvent.IsFlexrayPDU.Value;
		}

		public override int GetHashCode()
		{
			return this.BusType.Value.GetHashCode() ^ this.ChannelNumber.Value.GetHashCode() ^ this.MessageName.Value.GetHashCode() ^ this.NetworkName.Value.GetHashCode() ^ this.DatabaseName.Value.GetHashCode() ^ this.DatabasePath.Value.GetHashCode() ^ this.IsFlexrayPDU.Value.GetHashCode();
		}

		public void Assign(SymbolicMessageEvent other)
		{
			this.BusType.Value = other.BusType.Value;
			this.ChannelNumber.Value = other.ChannelNumber.Value;
			this.MessageName.Value = other.MessageName.Value;
			this.NetworkName.Value = other.NetworkName.Value;
			this.DatabaseName.Value = other.DatabaseName.Value;
			this.DatabasePath.Value = other.DatabasePath.Value;
			this.IsFlexrayPDU.Value = other.IsFlexrayPDU.Value;
		}
	}
}
