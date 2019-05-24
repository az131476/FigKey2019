using System;
using System.Runtime.Serialization;
using Vector.VLConfig.Data.ApplicationData;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DiagnosticSignalEvent")]
	public class DiagnosticSignalEvent : Event, ISymbolicSignalEvent, ISymbolicSignal
	{
		private ValidatedProperty<bool> mIsPointInTime;

		private ValidatedProperty<string> mNetworkName;

		private ValidatedProperty<string> mMessageName;

		private ValidatedProperty<string> mDatabaseName;

		private ValidatedProperty<bool> mIsFlexrayPdu;

		[DataMember(Name = "DiagnosticActionEventSignalName")]
		public ValidatedProperty<string> SignalName
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionEventChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionEventBusType")]
		public ValidatedProperty<BusType> BusType
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionEventDatabasePath")]
		public ValidatedProperty<string> DatabasePath
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionSignalEventRelation")]
		public ValidatedProperty<CondRelation> Relation
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionSignalEventLowValue")]
		public ValidatedProperty<double> LowValue
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionSignalEventHighValue")]
		public ValidatedProperty<double> HighValue
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionSignalEventDiagnosticVariant")]
		public ValidatedProperty<string> DiagnosticVariant
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionSignalEventDiagnosticDid")]
		public ValidatedProperty<string> DiagnosticDid
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionSignalEventDiagnosticEcuName")]
		public ValidatedProperty<string> DiagnosticEcuName
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionSignalEventDiagnosticServiceName")]
		public ValidatedProperty<string> DiagnosticServiceName
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionSignalEventMessageData")]
		public ValidatedProperty<byte[]> DiagnosticMessageData
		{
			get;
			set;
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

		public ValidatedProperty<string> CcpXcpEcuName
		{
			get
			{
				return this.DiagnosticEcuName;
			}
			set
			{
				this.DiagnosticEcuName = value;
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

		public DiagnosticSignalEvent()
		{
			this.SignalName = new ValidatedProperty<string>();
			this.ChannelNumber = new ValidatedProperty<uint>();
			this.BusType = new ValidatedProperty<BusType>();
			this.DatabasePath = new ValidatedProperty<string>();
			this.Relation = new ValidatedProperty<CondRelation>();
			this.LowValue = new ValidatedProperty<double>();
			this.HighValue = new ValidatedProperty<double>();
			this.DiagnosticEcuName = new ValidatedProperty<string>();
			this.DiagnosticServiceName = new ValidatedProperty<string>();
			this.DiagnosticVariant = new ValidatedProperty<string>();
			this.DiagnosticDid = new ValidatedProperty<string>();
			this.DiagnosticMessageData = new ValidatedProperty<byte[]>();
		}

		public DiagnosticSignalEvent(ISymbolicSignalEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new DiagnosticSignalEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			DiagnosticSignalEvent diagnosticSignalEvent = (DiagnosticSignalEvent)obj;
			return this.BusType.Value == diagnosticSignalEvent.BusType.Value && this.ChannelNumber.Value == diagnosticSignalEvent.ChannelNumber.Value && !(this.NetworkName.Value != diagnosticSignalEvent.NetworkName.Value) && !(this.MessageName.Value != diagnosticSignalEvent.MessageName.Value) && !(this.SignalName.Value != diagnosticSignalEvent.SignalName.Value) && !(this.DatabaseName.Value != diagnosticSignalEvent.DatabaseName.Value) && !(this.DatabasePath.Value != diagnosticSignalEvent.DatabasePath.Value) && this.Relation.Value == diagnosticSignalEvent.Relation.Value && this.LowValue.Value.Equals(diagnosticSignalEvent.LowValue.Value) && this.HighValue.Value.Equals(diagnosticSignalEvent.HighValue.Value) && this.IsFlexrayPDU.Value == diagnosticSignalEvent.IsFlexrayPDU.Value && !(this.DiagnosticEcuName.Value != diagnosticSignalEvent.DiagnosticEcuName.Value) && !(this.DiagnosticServiceName.Value != diagnosticSignalEvent.DiagnosticServiceName.Value) && !(this.DiagnosticVariant.Value != diagnosticSignalEvent.DiagnosticVariant.Value) && !(this.DiagnosticDid.Value != diagnosticSignalEvent.DiagnosticDid.Value) && this.DiagnosticMessageData.Value == diagnosticSignalEvent.DiagnosticMessageData.Value;
		}

		public override int GetHashCode()
		{
			return this.SignalName.Value.GetHashCode() ^ this.Relation.Value.GetHashCode() ^ this.LowValue.Value.GetHashCode() ^ this.HighValue.Value.GetHashCode() ^ this.DiagnosticVariant.Value.GetHashCode() ^ this.DiagnosticDid.Value.GetHashCode() ^ this.DiagnosticEcuName.Value.GetHashCode();
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
			if (other is DiagnosticSignalEvent)
			{
				DiagnosticSignalEvent diagnosticSignalEvent = other as DiagnosticSignalEvent;
				this.DiagnosticEcuName = ((diagnosticSignalEvent.DiagnosticEcuName == null) ? null : new ValidatedProperty<string>(diagnosticSignalEvent.DiagnosticEcuName.Value));
				this.DiagnosticServiceName = ((diagnosticSignalEvent.DiagnosticServiceName == null) ? null : new ValidatedProperty<string>(diagnosticSignalEvent.DiagnosticServiceName.Value));
				this.DiagnosticVariant = ((diagnosticSignalEvent.DiagnosticVariant == null) ? null : new ValidatedProperty<string>(diagnosticSignalEvent.DiagnosticVariant.Value));
				this.DiagnosticDid = ((diagnosticSignalEvent.DiagnosticDid == null) ? null : new ValidatedProperty<string>(diagnosticSignalEvent.DiagnosticDid.Value));
				this.DiagnosticMessageData = ((diagnosticSignalEvent.DiagnosticMessageData == null) ? null : new ValidatedProperty<byte[]>(diagnosticSignalEvent.DiagnosticMessageData.Value));
				return;
			}
			this.DiagnosticEcuName = new ValidatedProperty<string>();
			this.DiagnosticServiceName = new ValidatedProperty<string>();
			this.DiagnosticVariant = new ValidatedProperty<string>();
			this.DiagnosticDid = new ValidatedProperty<string>();
			this.DiagnosticMessageData = new ValidatedProperty<byte[]>();
		}
	}
}
