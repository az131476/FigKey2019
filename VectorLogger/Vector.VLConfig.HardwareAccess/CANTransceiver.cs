using System;

namespace Vector.VLConfig.HardwareAccess
{
	public class CANTransceiver
	{
		public static bool IsLowSpeed(CANTransceiverType type)
		{
			switch (type)
			{
			case CANTransceiverType.TJA1054:
			case CANTransceiverType.TLE6255:
				break;
			default:
				switch (type)
				{
				case CANTransceiverType.TJA1054_Isol:
				case CANTransceiverType.TJA1055:
				case CANTransceiverType.TJA1055Isol:
					return true;
				}
				return false;
			}
			return true;
		}

		public static bool IsWakeupCapable(CANTransceiverType type)
		{
			switch (type)
			{
			case CANTransceiverType.PCA82C251:
			case CANTransceiverType.TJA1050:
				return false;
			default:
				return true;
			}
		}
	}
}
