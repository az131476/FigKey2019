using System;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public interface IGL3000Device
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

		CANTransceiverType CAN5TransceiverType
		{
			get;
		}

		CANTransceiverType CAN6TransceiverType
		{
			get;
		}

		CANTransceiverType CAN7TransceiverType
		{
			get;
		}

		CANTransceiverType CAN8TransceiverType
		{
			get;
		}

		CANTransceiverType CAN9TransceiverType
		{
			get;
		}

		bool IsAnalogExtensionBoardInstalled
		{
			get;
		}

		bool IsWLANExtensionBoardInstalled
		{
			get;
		}

		string InstalledWLANBoardModelName
		{
			get;
		}
	}
}
