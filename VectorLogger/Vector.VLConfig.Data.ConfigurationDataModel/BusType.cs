using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum BusType
	{
		[EnumMember(Value = "None")]
		Bt_None,
		[EnumMember(Value = "CAN")]
		Bt_CAN,
		[EnumMember(Value = "LIN")]
		Bt_LIN,
		[EnumMember(Value = "FlexRay")]
		Bt_FlexRay,
		[EnumMember(Value = "J1708")]
		Bt_J1708,
		[EnumMember(Value = "Ethernet")]
		Bt_Ethernet,
		[EnumMember(Value = "Wildcard")]
		Bt_Wildcard,
		[EnumMember(Value = "MOST")]
		Bt_MOST
	}
}
