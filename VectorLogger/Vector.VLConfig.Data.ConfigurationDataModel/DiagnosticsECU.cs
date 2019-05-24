using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DiagnosticsECU")]
	public class DiagnosticsECU : ITransmitMessageChannel
	{
		private ValidatedProperty<BusType> busType;

		[DataMember(Name = "DiagnosticsECUQualifier")]
		public ValidatedProperty<string> Qualifier;

		[DataMember(Name = "DiagnosticsECUVariant")]
		public ValidatedProperty<string> Variant;

		[DataMember(Name = "DiagnosticsECUCommParams")]
		public DiagnosticCommParamsECU DiagnosticCommParamsECU;

		public DiagnosticsDatabase Database;

		[DataMember(Name = "DiagnosticsECUChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		public ValidatedProperty<BusType> BusType
		{
			get
			{
				if (this.busType == null)
				{
					this.busType = new ValidatedProperty<BusType>(Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN);
				}
				return this.busType;
			}
		}

		public DiagnosticsECU()
		{
			this.Qualifier = new ValidatedProperty<string>("");
			this.Variant = new ValidatedProperty<string>("");
			this.ChannelNumber = new ValidatedProperty<uint>(1u);
			this.DiagnosticCommParamsECU = new DiagnosticCommParamsECU();
		}

		public DiagnosticsECU(DiagnosticsECU other)
		{
			this.Qualifier = new ValidatedProperty<string>(other.Qualifier.Value);
			this.Variant = new ValidatedProperty<string>(other.Variant.Value);
			this.ChannelNumber = new ValidatedProperty<uint>(other.ChannelNumber.Value);
			this.DiagnosticCommParamsECU = new DiagnosticCommParamsECU(other.DiagnosticCommParamsECU);
		}
	}
}
