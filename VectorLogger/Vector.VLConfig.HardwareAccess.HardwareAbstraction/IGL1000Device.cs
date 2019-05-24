using System;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public interface IGL1000Device
	{
		CANTransceiverType CAN1TransceiverType
		{
			get;
		}

		CANTransceiverType CAN2TransceiverType
		{
			get;
		}

		uint CardWriteCacheSizeKB
		{
			get;
		}
	}
}
