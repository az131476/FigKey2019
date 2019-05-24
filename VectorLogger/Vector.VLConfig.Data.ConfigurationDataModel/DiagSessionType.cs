using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum DiagSessionType
	{
		[EnumMember(Value = "Unknown")]
		Unknown,
		[EnumMember(Value = "Default")]
		Default,
		[EnumMember(Value = "Extended")]
		Extended,
		[EnumMember(Value = "DynamicFromDB")]
		DynamicFromDB
	}
}
