using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum DiagDbType
	{
		[EnumMember(Value = "None")]
		None,
		[EnumMember(Value = "ODX")]
		ODX,
		[EnumMember(Value = "PDX")]
		PDX,
		[EnumMember(Value = "CDD")]
		CDD,
		[EnumMember(Value = "OBD")]
		OBD,
		[EnumMember(Value = "MDX")]
		MDX
	}
}
