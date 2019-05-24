using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "SymbolicSignalEvent")]
	public class SymbolicSignalEvent : Event, ISymbolicSignalEvent, ISymbolicSignal
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

		[DataMember(Name = "SymbolicSignalEventBusType")]
		public ValidatedProperty<BusType> BusType
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicSignalEventChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicSignalEventNetworkName")]
		public ValidatedProperty<string> NetworkName
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicSignalEventMessageName")]
		public ValidatedProperty<string> MessageName
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicSignalEventSignalName")]
		public ValidatedProperty<string> SignalName
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicSignalEventDatabaseName")]
		public ValidatedProperty<string> DatabaseName
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicSignalEventDatabasePath")]
		public ValidatedProperty<string> DatabasePath
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicSignalEventRelation")]
		public ValidatedProperty<CondRelation> Relation
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicSignalEventLowValue")]
		public ValidatedProperty<double> LowValue
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicSignalEventHighValue")]
		public ValidatedProperty<double> HighValue
		{
			get;
			set;
		}

		[DataMember(Name = "SymbolicSignalEventIsFlexrayPDU")]
		public ValidatedProperty<bool> IsFlexrayPDU
		{
			get;
			set;
		}

		public ValidatedProperty<string> CcpXcpEcuName
		{
			get;
			set;
		}

		public SymbolicSignalEvent()
		{
			this.BusType = new ValidatedProperty<BusType>();
			this.ChannelNumber = new ValidatedProperty<uint>();
			this.NetworkName = new ValidatedProperty<string>();
			this.MessageName = new ValidatedProperty<string>();
			this.SignalName = new ValidatedProperty<string>();
			this.DatabaseName = new ValidatedProperty<string>();
			this.DatabasePath = new ValidatedProperty<string>();
			this.Relation = new ValidatedProperty<CondRelation>();
			this.LowValue = new ValidatedProperty<double>();
			this.HighValue = new ValidatedProperty<double>();
			this.IsFlexrayPDU = new ValidatedProperty<bool>(false);
		}

		public SymbolicSignalEvent(ISymbolicSignalEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new SymbolicSignalEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			ISymbolicSignalEvent symbolicSignalEvent = (ISymbolicSignalEvent)obj;
			return ((this.BusType == null && symbolicSignalEvent.BusType == null) || (!(this.BusType != null ^ symbolicSignalEvent.BusType != null) && this.BusType.Value == symbolicSignalEvent.BusType.Value)) && ((this.ChannelNumber == null && symbolicSignalEvent.ChannelNumber == null) || (!(this.ChannelNumber != null ^ symbolicSignalEvent.ChannelNumber != null) && this.ChannelNumber.Value == symbolicSignalEvent.ChannelNumber.Value)) && ((this.NetworkName == null && symbolicSignalEvent.NetworkName == null) || (!(this.NetworkName != null ^ symbolicSignalEvent.NetworkName != null) && !(this.NetworkName.Value != symbolicSignalEvent.NetworkName.Value))) && ((this.MessageName == null && symbolicSignalEvent.MessageName == null) || (!(this.MessageName != null ^ symbolicSignalEvent.MessageName != null) && !(this.MessageName.Value != symbolicSignalEvent.MessageName.Value))) && ((this.SignalName == null && symbolicSignalEvent.SignalName == null) || (!(this.SignalName != null ^ symbolicSignalEvent.SignalName != null) && !(this.SignalName.Value != symbolicSignalEvent.SignalName.Value))) && ((this.DatabaseName == null && symbolicSignalEvent.DatabaseName == null) || (!(this.DatabaseName != null ^ symbolicSignalEvent.DatabaseName != null) && !(this.DatabaseName.Value != symbolicSignalEvent.DatabaseName.Value))) && ((this.DatabasePath == null && symbolicSignalEvent.DatabasePath == null) || (!(this.DatabasePath != null ^ symbolicSignalEvent.DatabasePath != null) && !(this.DatabasePath.Value != symbolicSignalEvent.DatabasePath.Value))) && ((this.Relation == null && symbolicSignalEvent.Relation == null) || (!(this.Relation != null ^ symbolicSignalEvent.Relation != null) && this.Relation.Value == symbolicSignalEvent.Relation.Value)) && ((this.LowValue == null && symbolicSignalEvent.LowValue == null) || (!(this.LowValue != null ^ symbolicSignalEvent.LowValue != null) && this.LowValue.Value == symbolicSignalEvent.LowValue.Value)) && ((this.HighValue == null && symbolicSignalEvent.HighValue == null) || (!(this.HighValue != null ^ symbolicSignalEvent.HighValue != null) && this.HighValue.Value == symbolicSignalEvent.HighValue.Value)) && ((this.IsFlexrayPDU == null && symbolicSignalEvent.IsFlexrayPDU == null) || (!(this.IsFlexrayPDU != null ^ symbolicSignalEvent.IsFlexrayPDU != null) && this.IsFlexrayPDU.Value == symbolicSignalEvent.IsFlexrayPDU.Value)) && ((this.CcpXcpEcuName == null && symbolicSignalEvent.CcpXcpEcuName == null) || (!(this.CcpXcpEcuName != null ^ symbolicSignalEvent.CcpXcpEcuName != null) && !(this.CcpXcpEcuName.Value != symbolicSignalEvent.CcpXcpEcuName.Value)));
		}

		public override int GetHashCode()
		{
			return this.BusType.Value.GetHashCode() ^ this.ChannelNumber.Value.GetHashCode() ^ this.NetworkName.Value.GetHashCode() ^ this.MessageName.Value.GetHashCode() ^ this.SignalName.Value.GetHashCode() ^ this.DatabaseName.Value.GetHashCode() ^ this.DatabasePath.Value.GetHashCode() ^ this.Relation.Value.GetHashCode() ^ this.LowValue.Value.GetHashCode() ^ this.HighValue.Value.GetHashCode() ^ this.IsFlexrayPDU.Value.GetHashCode();
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
