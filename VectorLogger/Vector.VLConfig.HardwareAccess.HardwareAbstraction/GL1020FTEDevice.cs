using System;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	internal class GL1020FTEDevice : LoggerDeviceGL1XXX, IGL1020FTEDevice
	{
		public override LoggerType LoggerType
		{
			get
			{
				return LoggerType.GL1020FTE;
			}
		}

		public override ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return GL1020FTEScanner.LoggerSpecifics;
			}
		}

		public CANTransceiverType CAN1TransceiverType
		{
			get
			{
				return this.can1TransceiverType;
			}
		}

		public CANTransceiverType CAN2TransceiverType
		{
			get
			{
				return this.can2TransceiverType;
			}
		}

		public GL1020FTEDevice(string hardwareKey, bool isOnline)
		{
			this.hardwareKey = hardwareKey;
			this.isOnline = isOnline;
			this.can1TransceiverType = CANTransceiverType.None;
			this.can2TransceiverType = CANTransceiverType.None;
			this.can1GenericTransceiverName = "";
			this.can2GenericTransceiverName = "";
		}
	}
}
