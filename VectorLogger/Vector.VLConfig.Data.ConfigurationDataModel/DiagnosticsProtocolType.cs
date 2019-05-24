using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum DiagnosticsProtocolType
	{
		[EnumMember(Value = "Undefined")]
		Undefined,
		[EnumMember(Value = "KWP")]
		KWP,
		[EnumMember(Value = "UDS")]
		UDS
	}
}
