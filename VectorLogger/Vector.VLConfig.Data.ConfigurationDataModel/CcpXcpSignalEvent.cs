using System;
using System.Runtime.Serialization;
using Vector.VLConfig.Data.ApplicationData;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CcpXcpSignalEvent")]
	public class CcpXcpSignalEvent : Event, ISymbolicSignalEvent, ISymbolicSignal, ICcpXcpSignal
	{
		private ValidatedProperty<bool> mIsPointInTime;

		private ValidatedProperty<BusType> mBusType;

		private ValidatedProperty<uint> mChannelNumber;

		private ValidatedProperty<string> mNetworkName;

		private ValidatedProperty<string> mMessageName;

		private ValidatedProperty<string> mDatabaseName;

		private ValidatedProperty<string> mDatabasePath;

		private ValidatedProperty<bool> mIsFlexrayPdu;

		[DataMember(Name = "CcpXcpSignalEventSignalName")]
		public ValidatedProperty<string> SignalName
		{
			get;
			set;
		}

		[DataMember(Name = "CcpXcpSignalEventRelation")]
		public ValidatedProperty<CondRelation> Relation
		{
			get;
			set;
		}

		[DataMember(Name = "CcpXcpSignalEventLowValue")]
		public ValidatedProperty<double> LowValue
		{
			get;
			set;
		}

		[DataMember(Name = "CcpXcpSignalEventHighValue")]
		public ValidatedProperty<double> HighValue
		{
			get;
			set;
		}

		[DataMember(Name = "CcpXcpSignalEventCcpXcpEcuName")]
		public ValidatedProperty<string> CcpXcpEcuName
		{
			get;
			set;
		}

		public ValidatedProperty<string> Name
		{
			get
			{
				return this.SignalName;
			}
			set
			{
				this.SignalName = value;
			}
		}

		public ValidatedProperty<string> EcuName
		{
			get
			{
				return this.CcpXcpEcuName;
			}
			set
			{
				this.CcpXcpEcuName = value;
			}
		}

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

		public ValidatedProperty<BusType> BusType
		{
			get
			{
				ValidatedProperty<BusType> arg_1B_0;
				if ((arg_1B_0 = this.mBusType) == null)
				{
					arg_1B_0 = (this.mBusType = new ValidatedProperty<BusType>());
				}
				return arg_1B_0;
			}
			set
			{
				this.mBusType = value;
			}
		}

		public ValidatedProperty<uint> ChannelNumber
		{
			get
			{
				ValidatedProperty<uint> arg_1B_0;
				if ((arg_1B_0 = this.mChannelNumber) == null)
				{
					arg_1B_0 = (this.mChannelNumber = new ValidatedProperty<uint>());
				}
				return arg_1B_0;
			}
			set
			{
				this.mChannelNumber = value;
			}
		}

		public ValidatedProperty<string> NetworkName
		{
			get
			{
				ValidatedProperty<string> arg_1B_0;
				if ((arg_1B_0 = this.mNetworkName) == null)
				{
					arg_1B_0 = (this.mNetworkName = new ValidatedProperty<string>());
				}
				return arg_1B_0;
			}
			set
			{
				this.mNetworkName = value;
			}
		}

		public ValidatedProperty<string> MessageName
		{
			get
			{
				ValidatedProperty<string> arg_1B_0;
				if ((arg_1B_0 = this.mMessageName) == null)
				{
					arg_1B_0 = (this.mMessageName = new ValidatedProperty<string>());
				}
				return arg_1B_0;
			}
			set
			{
				this.mMessageName = value;
			}
		}

		public ValidatedProperty<string> DatabaseName
		{
			get
			{
				ValidatedProperty<string> arg_1B_0;
				if ((arg_1B_0 = this.mDatabaseName) == null)
				{
					arg_1B_0 = (this.mDatabaseName = new ValidatedProperty<string>());
				}
				return arg_1B_0;
			}
			set
			{
				this.mDatabaseName = value;
			}
		}

		public ValidatedProperty<string> DatabasePath
		{
			get
			{
				ValidatedProperty<string> arg_1B_0;
				if ((arg_1B_0 = this.mDatabasePath) == null)
				{
					arg_1B_0 = (this.mDatabasePath = new ValidatedProperty<string>());
				}
				return arg_1B_0;
			}
			set
			{
				this.mDatabasePath = value;
			}
		}

		public ValidatedProperty<bool> IsFlexrayPDU
		{
			get
			{
				ValidatedProperty<bool> arg_1B_0;
				if ((arg_1B_0 = this.mIsFlexrayPdu) == null)
				{
					arg_1B_0 = (this.mIsFlexrayPdu = new ValidatedProperty<bool>());
				}
				return arg_1B_0;
			}
			set
			{
				this.mIsFlexrayPdu = value;
			}
		}

		public string SignalNameInGeneratedDatabase
		{
			get;
			set;
		}

		public SignalDefinition SignalDefinitionInGeneratedDatabase
		{
			get;
			set;
		}

		public CcpXcpSignalEvent()
		{
			this.SignalName = new ValidatedProperty<string>();
			this.Relation = new ValidatedProperty<CondRelation>();
			this.LowValue = new ValidatedProperty<double>();
			this.HighValue = new ValidatedProperty<double>();
			this.CcpXcpEcuName = new ValidatedProperty<string>();
		}

		public CcpXcpSignalEvent(ISymbolicSignalEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new CcpXcpSignalEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			CcpXcpSignalEvent ccpXcpSignalEvent = (CcpXcpSignalEvent)obj;
			return this.BusType.Value == ccpXcpSignalEvent.BusType.Value && this.ChannelNumber.Value == ccpXcpSignalEvent.ChannelNumber.Value && !(this.NetworkName.Value != ccpXcpSignalEvent.NetworkName.Value) && !(this.MessageName.Value != ccpXcpSignalEvent.MessageName.Value) && !(this.SignalName.Value != ccpXcpSignalEvent.SignalName.Value) && !(this.DatabaseName.Value != ccpXcpSignalEvent.DatabaseName.Value) && !(this.DatabasePath.Value != ccpXcpSignalEvent.DatabasePath.Value) && this.Relation.Value == ccpXcpSignalEvent.Relation.Value && this.LowValue.Value.Equals(ccpXcpSignalEvent.LowValue.Value) && this.HighValue.Value.Equals(ccpXcpSignalEvent.HighValue.Value) && this.IsFlexrayPDU.Value == ccpXcpSignalEvent.IsFlexrayPDU.Value && !(this.CcpXcpEcuName.Value != ccpXcpSignalEvent.CcpXcpEcuName.Value);
		}

		public override int GetHashCode()
		{
			return this.SignalName.Value.GetHashCode() ^ this.Relation.Value.GetHashCode() ^ this.LowValue.Value.GetHashCode() ^ this.HighValue.Value.GetHashCode() ^ this.CcpXcpEcuName.Value.GetHashCode();
		}

		public void Assign(ISymbolicSignalEvent other)
		{
			this.BusType = ((other.BusType == null) ? null : new ValidatedProperty<BusType>(other.BusType.Value));
			this.ChannelNumber = ((other.ChannelNumber == null) ? null : new ValidatedProperty<uint>(other.ChannelNumber.Value));
			this.NetworkName = ((other.NetworkName == null) ? null : new ValidatedProperty<string>(other.NetworkName.Value));
			this.MessageName = ((other.MessageName == null) ? null : new ValidatedProperty<string>(other.MessageName.Value));
			this.SignalName = ((other.SignalName == null) ? null : new ValidatedProperty<string>(other.SignalName.Value));
			this.DatabaseName = ((other.DatabaseName == null) ? null : new ValidatedProperty<string>(other.DatabaseName.Value));
			this.DatabasePath = ((other.DatabasePath == null) ? null : new ValidatedProperty<string>(other.DatabasePath.Value));
			this.Relation = ((other.Relation == null) ? null : new ValidatedProperty<CondRelation>(other.Relation.Value));
			this.LowValue = ((other.LowValue == null) ? null : new ValidatedProperty<double>(other.LowValue.Value));
			this.HighValue = ((other.HighValue == null) ? null : new ValidatedProperty<double>(other.HighValue.Value));
			this.IsFlexrayPDU = ((other.IsFlexrayPDU == null) ? null : new ValidatedProperty<bool>(other.IsFlexrayPDU.Value));
			this.CcpXcpEcuName = ((other.CcpXcpEcuName == null) ? null : new ValidatedProperty<string>(other.CcpXcpEcuName.Value));
		}
	}
}
