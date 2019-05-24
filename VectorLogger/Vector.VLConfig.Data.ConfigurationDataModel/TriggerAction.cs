using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum TriggerAction
	{
		[EnumMember(Value = "None")]
		None,
		[EnumMember(Value = "Beep")]
		Beep,
		[EnumMember(Value = "Unknown")]
		Unknown
	}
}
