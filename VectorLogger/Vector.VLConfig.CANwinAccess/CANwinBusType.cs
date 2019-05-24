using System;

namespace Vector.VLConfig.CANwinAccess
{
	public enum CANwinBusType
	{
		Invalid,
		CAN,
		J1939,
		VAN,
		TTP,
		LIN,
		MOST,
		FLEXRAY,
		reserved,
		J1708,
		Ethernet = 11,
		Wlan = 13,
		AFDX,
		KLineBt
	}
}
