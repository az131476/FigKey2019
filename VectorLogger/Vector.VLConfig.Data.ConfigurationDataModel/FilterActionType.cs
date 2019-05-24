using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum FilterActionType
	{
		[EnumMember(Value = "Stop")]
		Stop,
		[EnumMember(Value = "Pass")]
		Pass,
		[EnumMember(Value = "Limit")]
		Limit
	}
}
