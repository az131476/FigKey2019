using System;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public interface IGL2000Device
	{
		CANTransceiverType CAN1TransceiverType
		{
			get;
		}

		CANTransceiverType CAN2TransceiverType
		{
			get;
		}

		CANTransceiverType CAN3TransceiverType
		{
			get;
		}

		CANTransceiverType CAN4TransceiverType
		{
			get;
		}
	}
}
