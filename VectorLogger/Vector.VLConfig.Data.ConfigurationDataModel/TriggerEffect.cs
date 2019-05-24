using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum TriggerEffect
	{
		[EnumMember(Value = "Normal")]
		Normal,
		[EnumMember(Value = "Single")]
		Single,
		[EnumMember(Value = "EndMeasurement")]
		EndMeasurement,
		[EnumMember(Value = "LoggingOn")]
		LoggingOn,
		[EnumMember(Value = "LoggingOff")]
		LoggingOff,
		[EnumMember(Value = "Marker")]
		Marker,
		[EnumMember(Value = "Unknown")]
		Unknown
	}
}
