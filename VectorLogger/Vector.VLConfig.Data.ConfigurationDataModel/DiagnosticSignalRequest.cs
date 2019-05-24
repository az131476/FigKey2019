using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DiagnosticSignalRequest")]
	public class DiagnosticSignalRequest : DiagnosticAction
	{
		[DataMember(Name = "DiagnosticSignalRequestQualifier")]
		public ValidatedProperty<string> SignalQualifier
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticSignalRequestDidId")]
		public ValidatedProperty<string> DidId
		{
			get;
			set;
		}

		public DiagnosticSignalRequest()
		{
			this.SignalQualifier = new ValidatedProperty<string>(string.Empty);
			this.DidId = new ValidatedProperty<string>(string.Empty);
		}

		public DiagnosticSignalRequest(DiagnosticSignalRequest other) : base(other)
		{
			this.SignalQualifier = new ValidatedProperty<string>(other.SignalQualifier.Value);
			this.DidId = new ValidatedProperty<string>(other.DidId.Value);
		}

		public DiagnosticSignalRequest(string qualifier, string didId, string serviceQualifier, string ecuQualifier, string databasePath, byte[] messageData, DiagSessionType sessiontype)
		{
			this.SignalQualifier = new ValidatedProperty<string>(qualifier);
			this.DidId = new ValidatedProperty<string>(didId);
			base.ServiceQualifier = new ValidatedProperty<string>(serviceQualifier);
			base.EcuQualifier = new ValidatedProperty<string>(ecuQualifier);
			base.DatabasePath = new ValidatedProperty<string>(databasePath);
			base.MessageData = new ValidatedProperty<byte[]>(messageData);
			base.SessionType = new ValidatedProperty<DiagSessionType>(sessiontype);
		}

		public bool Equals(DiagnosticSignalRequest other)
		{
			return other != null && (base.EcuQualifier.Value == other.EcuQualifier.Value && base.ServiceQualifier.Value == other.ServiceQualifier.Value && this.SignalQualifier.Value == other.SignalQualifier.Value) && base.SessionType.Value == other.SessionType.Value;
		}
	}
}
