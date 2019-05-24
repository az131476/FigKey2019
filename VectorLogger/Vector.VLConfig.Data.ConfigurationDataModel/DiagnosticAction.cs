using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DiagnosticAction")]
	public class DiagnosticAction
	{
		public bool HasOnlyConstParams;

		public TriggeredDiagnosticActionSequence TriggeredDiagnosticActionSequence;

		[DataMember(Name = "DiagnosticActionDatabasePath")]
		public ValidatedProperty<string> DatabasePath
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionEcuName")]
		public ValidatedProperty<string> EcuQualifier
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionServiceName")]
		public ValidatedProperty<string> ServiceQualifier
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionSessionType")]
		public ValidatedProperty<DiagSessionType> SessionType
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticActionMessageData")]
		public ValidatedProperty<byte[]> MessageData
		{
			get;
			set;
		}

		public DiagnosticAction()
		{
			this.DatabasePath = new ValidatedProperty<string>("");
			this.EcuQualifier = new ValidatedProperty<string>("");
			this.ServiceQualifier = new ValidatedProperty<string>("");
			this.SessionType = new ValidatedProperty<DiagSessionType>(DiagSessionType.Default);
			this.MessageData = new ValidatedProperty<byte[]>();
			this.HasOnlyConstParams = false;
		}

		public DiagnosticAction(DiagnosticAction other)
		{
			this.DatabasePath = new ValidatedProperty<string>(other.DatabasePath.Value);
			this.EcuQualifier = new ValidatedProperty<string>(other.EcuQualifier.Value);
			this.ServiceQualifier = new ValidatedProperty<string>(other.ServiceQualifier.Value);
			this.SessionType = new ValidatedProperty<DiagSessionType>(other.SessionType.Value);
			this.MessageData = new ValidatedProperty<byte[]>();
			if (other.MessageData.Value != null)
			{
				this.MessageData.Value = new byte[other.MessageData.Value.Length];
				for (int i = 0; i < other.MessageData.Value.Length; i++)
				{
					this.MessageData.Value[i] = other.MessageData.Value[i];
				}
			}
			this.HasOnlyConstParams = other.HasOnlyConstParams;
		}
	}
}
