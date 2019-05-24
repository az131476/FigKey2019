using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum DatabaseFileType
	{
		[EnumMember(Value = "Unspecified")]
		Unspecified,
		[EnumMember(Value = "A2L")]
		A2L,
		[EnumMember(Value = "DBC")]
		DBC,
		[EnumMember(Value = "XML")]
		XML
	}
}
