using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum DiagnosticsAddressingMode
	{
		[EnumMember(Value = "Undefined")]
		Undefined,
		[EnumMember(Value = "Normal")]
		Normal,
		[EnumMember(Value = "NormalFixed")]
		NormalFixed,
		[EnumMember(Value = "Extended")]
		Extended
	}
}
