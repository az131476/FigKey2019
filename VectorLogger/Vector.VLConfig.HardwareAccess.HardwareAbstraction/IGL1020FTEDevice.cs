using System;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public interface IGL1020FTEDevice
	{
		CANTransceiverType CAN1TransceiverType
		{
			get;
		}

		CANTransceiverType CAN2TransceiverType
		{
			get;
		}
	}
}
