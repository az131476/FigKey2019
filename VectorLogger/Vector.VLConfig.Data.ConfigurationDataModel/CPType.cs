using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum CPType
	{
		[EnumMember(Value = "None")]
		None,
		[EnumMember(Value = "CCP")]
		CCP,
		[EnumMember(Value = "CCP 1.01")]
		CCP101,
		[EnumMember(Value = "XCP")]
		XCP
	}
}
