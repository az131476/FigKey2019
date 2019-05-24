using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum CcpXcpEcuTimestampUnit
	{
		[EnumMember(Value = "TU_Unspecified")]
		TU_Unspecified,
		[EnumMember(Value = "TU_1Picoseconds")]
		TU_1Picoseconds,
		[EnumMember(Value = "TU_10Picoseconds")]
		TU_10Picoseconds,
		[EnumMember(Value = "TU_100Picoseconds")]
		TU_100Picoseconds,
		[EnumMember(Value = "TU_1Nanoseconds")]
		TU_1Nanoseconds,
		[EnumMember(Value = "TU_10Nanoseconds")]
		TU_10Nanoseconds,
		[EnumMember(Value = "TU_100Nanoseconds")]
		TU_100Nanoseconds,
		[EnumMember(Value = "TU_1Microseconds")]
		TU_1Microseconds,
		[EnumMember(Value = "TU_10Microseconds")]
		TU_10Microseconds,
		[EnumMember(Value = "TU_100Microseconds")]
		TU_100Microseconds,
		[EnumMember(Value = "TU_1Milliseconds")]
		TU_1Milliseconds,
		[EnumMember(Value = "TU_10Milliseconds")]
		TU_10Milliseconds,
		[EnumMember(Value = "TU_100Milliseconds")]
		TU_100Milliseconds,
		[EnumMember(Value = "TU_1Seconds")]
		TU_1Seconds
	}
}
