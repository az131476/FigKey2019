using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DiagnosticCommParamsECU")]
	public class DiagnosticCommParamsECU
	{
		[DataMember(Name = "DiagnosticCommParamsECUUseParamValuesFromDb")]
		public ValidatedProperty<bool> UseParamValuesFromDb
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUInferfaceQualifier")]
		public ValidatedProperty<string> InterfaceQualifier
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUDiagProtocol")]
		public ValidatedProperty<DiagnosticsProtocolType> DiagProtocol
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUDiagAddressingMode")]
		public ValidatedProperty<DiagnosticsAddressingMode> DiagAddressingMode
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUDiagExtAddressingModeRqExt")]
		public ValidatedProperty<uint> ExtAddressingModeRqExtension
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUDiagExtAddressingModeRsExt")]
		public ValidatedProperty<uint> ExtAddressingModeRsExtension
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUP2Timeout")]
		public ValidatedProperty<uint> P2Timeout
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUP2Extension")]
		public ValidatedProperty<uint> P2Extension
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUPhysRequestMsgId")]
		public ValidatedProperty<uint> PhysRequestMsgId
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUPhysRequestMsgIsExtendedId")]
		public ValidatedProperty<bool> PhysRequestMsgIsExtendedId
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUResponseMsgId")]
		public ValidatedProperty<uint> ResponseMsgId
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsResponseMsgIsExtendedId")]
		public ValidatedProperty<bool> ResponseMsgIsExtendedId
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUSTMin")]
		public ValidatedProperty<uint> STMin
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUUsePaddingBytes")]
		public ValidatedProperty<bool> UsePaddingBytes
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUPaddingByteValue")]
		public ValidatedProperty<byte> PaddingByteValue
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUFirstConsecFrameValue")]
		public ValidatedProperty<byte> FirstConsecFrameValue
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUDefaultSessionSource")]
		public ValidatedProperty<DiagnosticsSessionSource> DefaultSessionSource
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUDefaultSessionName")]
		public ValidatedProperty<string> DefaultSessionName
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUDefaultSessionIdBytes")]
		public ValidatedProperty<ulong> DefaultSessionId
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUExtendedSessionSource")]
		public ValidatedProperty<DiagnosticsSessionSource> ExtendedSessionSource
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUExtendedSessionName")]
		public ValidatedProperty<string> ExtendedSessionName
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUExtendedSessionIdBytes")]
		public ValidatedProperty<ulong> ExtendedSessionId
		{
			get;
			set;
		}

		[DataMember(Name = "DiagnosticCommParamsECUUseStopCommRequest")]
		public ValidatedProperty<bool> UseStopCommRequest
		{
			get;
			set;
		}

		public bool HasDiagProtocolFromDb
		{
			get;
			set;
		}

		public bool HasDiagAddrModeFromDb
		{
			get;
			set;
		}

		public bool HasExtAddressingModeRqExtension
		{
			get;
			set;
		}

		public bool HasExtAddressingModeRsExtension
		{
			get;
			set;
		}

		public bool HasP2TimeoutFromDb
		{
			get;
			set;
		}

		public bool HasP2ExtensionFromDb
		{
			get;
			set;
		}

		public bool HasPhysRequestMsgFromDb
		{
			get;
			set;
		}

		public bool HasRespMsgFromDb
		{
			get;
			set;
		}

		public bool HasUsePaddingBytesFromDb
		{
			get;
			set;
		}

		public bool HasPaddingBytesFromDb
		{
			get;
			set;
		}

		public bool HasFirstConsecFrameValueFromDb
		{
			get;
			set;
		}

		public DiagnosticCommParamsECU()
		{
			this.UseParamValuesFromDb = new ValidatedProperty<bool>(false);
			this.InterfaceQualifier = new ValidatedProperty<string>("");
			this.DiagProtocol = new ValidatedProperty<DiagnosticsProtocolType>(DiagnosticsProtocolType.Undefined);
			this.DiagAddressingMode = new ValidatedProperty<DiagnosticsAddressingMode>(DiagnosticsAddressingMode.Undefined);
			this.ExtAddressingModeRqExtension = new ValidatedProperty<uint>(0u);
			this.ExtAddressingModeRsExtension = new ValidatedProperty<uint>(0u);
			this.P2Timeout = new ValidatedProperty<uint>(250u);
			this.P2Extension = new ValidatedProperty<uint>(250u);
			this.PhysRequestMsgId = new ValidatedProperty<uint>(0u);
			this.PhysRequestMsgIsExtendedId = new ValidatedProperty<bool>(false);
			this.ResponseMsgId = new ValidatedProperty<uint>(0u);
			this.ResponseMsgIsExtendedId = new ValidatedProperty<bool>(false);
			this.STMin = new ValidatedProperty<uint>(40u);
			this.UsePaddingBytes = new ValidatedProperty<bool>(true);
			this.PaddingByteValue = new ValidatedProperty<byte>(0);
			this.FirstConsecFrameValue = new ValidatedProperty<byte>(1);
			this.DefaultSessionSource = new ValidatedProperty<DiagnosticsSessionSource>(DiagnosticsSessionSource.UserDefined);
			this.DefaultSessionName = new ValidatedProperty<string>("");
			this.DefaultSessionId = new ValidatedProperty<ulong>();
			this.ExtendedSessionSource = new ValidatedProperty<DiagnosticsSessionSource>(DiagnosticsSessionSource.UserDefined);
			this.ExtendedSessionName = new ValidatedProperty<string>("");
			this.ExtendedSessionId = new ValidatedProperty<ulong>(0uL);
			this.UseStopCommRequest = new ValidatedProperty<bool>(false);
			this.ResetHasFromDbFlags();
		}

		public DiagnosticCommParamsECU(DiagnosticCommParamsECU other)
		{
			this.UseParamValuesFromDb = new ValidatedProperty<bool>(other.UseParamValuesFromDb.Value);
			this.InterfaceQualifier = new ValidatedProperty<string>(other.InterfaceQualifier.Value);
			this.DiagProtocol = new ValidatedProperty<DiagnosticsProtocolType>(other.DiagProtocol.Value);
			this.DiagAddressingMode = new ValidatedProperty<DiagnosticsAddressingMode>(other.DiagAddressingMode.Value);
			this.ExtAddressingModeRqExtension = new ValidatedProperty<uint>(other.ExtAddressingModeRqExtension.Value);
			this.ExtAddressingModeRsExtension = new ValidatedProperty<uint>(other.ExtAddressingModeRsExtension.Value);
			this.P2Timeout = new ValidatedProperty<uint>(other.P2Timeout.Value);
			this.P2Extension = new ValidatedProperty<uint>(other.P2Extension.Value);
			this.PhysRequestMsgId = new ValidatedProperty<uint>(other.PhysRequestMsgId.Value);
			this.PhysRequestMsgIsExtendedId = new ValidatedProperty<bool>(other.PhysRequestMsgIsExtendedId.Value);
			this.ResponseMsgId = new ValidatedProperty<uint>(other.ResponseMsgId.Value);
			this.ResponseMsgIsExtendedId = new ValidatedProperty<bool>(other.ResponseMsgIsExtendedId.Value);
			this.STMin = new ValidatedProperty<uint>(other.STMin.Value);
			this.UsePaddingBytes = new ValidatedProperty<bool>(other.UsePaddingBytes.Value);
			this.PaddingByteValue = new ValidatedProperty<byte>(other.PaddingByteValue.Value);
			this.FirstConsecFrameValue = new ValidatedProperty<byte>(other.FirstConsecFrameValue.Value);
			this.DefaultSessionSource = new ValidatedProperty<DiagnosticsSessionSource>(other.DefaultSessionSource.Value);
			this.DefaultSessionName = new ValidatedProperty<string>(other.DefaultSessionName.Value);
			this.DefaultSessionId = new ValidatedProperty<ulong>(other.DefaultSessionId.Value);
			this.ExtendedSessionSource = new ValidatedProperty<DiagnosticsSessionSource>(other.ExtendedSessionSource.Value);
			this.ExtendedSessionName = new ValidatedProperty<string>(other.ExtendedSessionName.Value);
			this.ExtendedSessionId = new ValidatedProperty<ulong>(other.ExtendedSessionId.Value);
			this.UseStopCommRequest = new ValidatedProperty<bool>(other.UseStopCommRequest.Value);
			this.ResetHasFromDbFlags();
		}

		public void ResetHasFromDbFlags()
		{
			this.HasDiagProtocolFromDb = false;
			this.HasDiagAddrModeFromDb = false;
			this.HasExtAddressingModeRqExtension = false;
			this.HasExtAddressingModeRsExtension = false;
			this.HasP2TimeoutFromDb = false;
			this.HasP2ExtensionFromDb = false;
			this.HasPhysRequestMsgFromDb = false;
			this.HasRespMsgFromDb = false;
			this.HasUsePaddingBytesFromDb = false;
			this.HasPaddingBytesFromDb = false;
			this.HasFirstConsecFrameValueFromDb = false;
		}
	}
}
