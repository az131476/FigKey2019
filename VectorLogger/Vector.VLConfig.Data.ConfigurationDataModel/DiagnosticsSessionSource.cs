using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum DiagnosticsSessionSource
	{
		[EnumMember(Value = "UserDefined")]
		UserDefined,
		[EnumMember(Value = "DatabaseDefined")]
		DatabaseDefined,
		[EnumMember(Value = "UDS_Default")]
		UDS_Default,
		[EnumMember(Value = "UDS_Extended")]
		UDS_Extended,
		[EnumMember(Value = "KWP_Default")]
		KWP_Default
	}
}
