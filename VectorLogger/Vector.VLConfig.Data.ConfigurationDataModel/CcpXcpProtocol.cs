using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum CcpXcpProtocol
	{
		[EnumMember(Value = "None")]
		CcpXcpPr_None,
		[EnumMember(Value = "CAN")]
		CcpXcpPr_CAN,
		[EnumMember(Value = "FlexRay")]
		CcpXcpPr_FlexRay,
		[EnumMember(Value = "UDP")]
		CcpXcpPr_UDP,
		[EnumMember(Value = "TCP")]
		CcpXcpPr_TCP,
		[EnumMember(Value = "CCP")]
		CcpXcpPr_CCP
	}
}
