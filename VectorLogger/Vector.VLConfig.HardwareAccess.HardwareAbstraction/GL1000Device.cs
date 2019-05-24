using System;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public class GL1000Device : LoggerDeviceGL1XXX, IGL1000Device
	{
		public override LoggerType LoggerType
		{
			get
			{
				return LoggerType.GL1000;
			}
		}

		public override ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return GL1000Scanner.LoggerSpecifics;
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

		public uint CardWriteCacheSizeKB
		{
			get
			{
				return this.cardWriteCacheSizeKB;
			}
		}

		public GL1000Device(string hardwareKey, bool isOnline)
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
