using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "OnMsgTimeoutEvent")]
	public class MsgTimeoutEvent : Event, ISymbolicMessage
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

		[DataMember(Name = "MsgTimeoutEventBusType")]
		public ValidatedProperty<BusType> BusType
		{
			get;
			set;
		}

		[DataMember(Name = "MsgTimeoutEventIsSymbolic")]
		public ValidatedProperty<bool> IsSymbolic
		{
			get;
			set;
		}

		[DataMember(Name = "MsgTimeoutEventMessageName")]
		public ValidatedProperty<string> MessageName
		{
			get;
			set;
		}

		[DataMember(Name = "MsgTimeoutEventNetworkName")]
		public ValidatedProperty<string> NetworkName
		{
			get;
			set;
		}

		[DataMember(Name = "MsgTimeoutEventDatabaseName")]
		public ValidatedProperty<string> DatabaseName
		{
			get;
			set;
		}

		[DataMember(Name = "MsgTimeoutEventDatabasePath")]
		public ValidatedProperty<string> DatabasePath
		{
			get;
			set;
		}

		[DataMember(Name = "MsgTimeoutEventID")]
		public ValidatedProperty<uint> ID
		{
			get;
			set;
		}

		[DataMember(Name = "MsgTimeoutEventIsExtendedId")]
		public ValidatedProperty<bool> IsExtendedId
		{
			get;
			set;
		}

		[DataMember(Name = "MsgTimeoutEventChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "MsgTimeoutEventIsCycletimeFromDatabase")]
		public ValidatedProperty<bool> IsCycletimeFromDatabase
		{
			get;
			set;
		}

		[DataMember(Name = "MsgTimeoutEventUserDefinedCycleTime")]
		public ValidatedProperty<uint> UserDefinedCycleTime
		{
			get;
			set;
		}

		[DataMember(Name = "MsgTimeoutEventMaxDelay")]
		public ValidatedProperty<uint> MaxDelay
		{
			get;
			set;
		}

		public bool IsDatabaseMsgCyclic
		{
			get;
			set;
		}

		public uint DatabaseCycleTime
		{
			get;
			set;
		}

		public uint ResultingTimeout
		{
			get
			{
				if (this.IsCycletimeFromDatabase.Value)
				{
					return this.DatabaseCycleTime + this.MaxDelay.Value;
				}
				return this.UserDefinedCycleTime.Value + this.MaxDelay.Value;
			}
		}

		public MsgTimeoutEvent()
		{
			this.BusType = new ValidatedProperty<BusType>(Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN);
			this.IsSymbolic = new ValidatedProperty<bool>(false);
			this.MessageName = new ValidatedProperty<string>("");
			this.NetworkName = new ValidatedProperty<string>("");
			this.DatabaseName = new ValidatedProperty<string>("");
			this.DatabasePath = new ValidatedProperty<string>("");
			this.ID = new ValidatedProperty<uint>(0u);
			this.IsExtendedId = new ValidatedProperty<bool>(false);
			this.ChannelNumber = new ValidatedProperty<uint>(1u);
			this.IsCycletimeFromDatabase = new ValidatedProperty<bool>(false);
			this.UserDefinedCycleTime = new ValidatedProperty<uint>(0u);
			this.MaxDelay = new ValidatedProperty<uint>(0u);
			this.IsDatabaseMsgCyclic = false;
			this.DatabaseCycleTime = 0u;
		}

		public MsgTimeoutEvent(MsgTimeoutEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new MsgTimeoutEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			MsgTimeoutEvent msgTimeoutEvent = (MsgTimeoutEvent)obj;
			return this.BusType.Value == msgTimeoutEvent.BusType.Value && this.IsSymbolic.Value == msgTimeoutEvent.IsSymbolic.Value && !(this.MessageName.Value != msgTimeoutEvent.MessageName.Value) && !(this.NetworkName.Value != msgTimeoutEvent.NetworkName.Value) && !(this.DatabaseName.Value != msgTimeoutEvent.DatabaseName.Value) && !(this.DatabasePath.Value != msgTimeoutEvent.DatabasePath.Value) && this.ID.Value == msgTimeoutEvent.ID.Value && this.IsExtendedId.Value == msgTimeoutEvent.IsExtendedId.Value && this.ChannelNumber.Value == msgTimeoutEvent.ChannelNumber.Value && this.IsCycletimeFromDatabase.Value == msgTimeoutEvent.IsCycletimeFromDatabase.Value && this.UserDefinedCycleTime.Value == msgTimeoutEvent.UserDefinedCycleTime.Value && this.MaxDelay.Value == msgTimeoutEvent.MaxDelay.Value;
		}

		public override int GetHashCode()
		{
			return this.BusType.GetHashCode() ^ this.IsSymbolic.GetHashCode() ^ this.MessageName.GetHashCode() ^ this.NetworkName.GetHashCode() ^ this.DatabaseName.GetHashCode() ^ this.DatabasePath.GetHashCode() ^ this.ID.GetHashCode() ^ this.IsExtendedId.GetHashCode() ^ this.ChannelNumber.GetHashCode() ^ this.IsCycletimeFromDatabase.GetHashCode() ^ this.UserDefinedCycleTime.GetHashCode() ^ this.MaxDelay.GetHashCode();
		}

		public void Assign(MsgTimeoutEvent other)
		{
			this.BusType.Value = other.BusType.Value;
			this.IsSymbolic.Value = other.IsSymbolic.Value;
			this.MessageName.Value = other.MessageName.Value;
			this.NetworkName.Value = other.NetworkName.Value;
			this.DatabaseName.Value = other.DatabaseName.Value;
			this.DatabasePath.Value = other.DatabasePath.Value;
			this.ID.Value = other.ID.Value;
			this.IsExtendedId.Value = other.IsExtendedId.Value;
			this.ChannelNumber.Value = other.ChannelNumber.Value;
			this.IsCycletimeFromDatabase.Value = other.IsCycletimeFromDatabase.Value;
			this.UserDefinedCycleTime.Value = other.UserDefinedCycleTime.Value;
			this.MaxDelay.Value = other.MaxDelay.Value;
			this.IsDatabaseMsgCyclic = other.IsDatabaseMsgCyclic;
			this.DatabaseCycleTime = other.DatabaseCycleTime;
		}
	}
}
