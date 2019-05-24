using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum CcpXcpEcuTimestampCalculationMethod
	{
		[EnumMember(Value = "Unspecified")]
		Unspecified,
		[EnumMember(Value = "Multiplication")]
		Multiplication,
		[EnumMember(Value = "Division")]
		Division
	}
}
