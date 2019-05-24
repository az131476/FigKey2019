using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum TriggerMode
	{
		[EnumMember(Value = "Triggered")]
		Triggered,
		[EnumMember(Value = "Permanent")]
		Permanent,
		[EnumMember(Value = "OnOff")]
		OnOff
	}
}
