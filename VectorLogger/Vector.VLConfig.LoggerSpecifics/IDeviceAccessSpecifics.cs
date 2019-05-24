using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LoggerSpecifics
{
	public interface IDeviceAccessSpecifics
	{
		string DefaultDummySerialNumber
		{
			get;
		}

		uint DeviceType
		{
			get;
		}

		bool IsUsingSecWrite
		{
			get;
		}

		bool AccessCardReaderDrivesOnly
		{
			get;
		}

		bool IsUSBConnectionSupported
		{
			get;
		}

		bool HasRealtimeClockAccessBySerialPort
		{
			get;
		}

		bool RequiresDisconnectAfterSettingRealtimeClock
		{
			get;
		}

		IList<string> InternalCardReaderModelNames
		{
			get;
		}

		string VID_PID
		{
			get;
		}

		bool IsSetVehicleNameSupported
		{
			get;
		}

		bool IsWriteLicenseSupported
		{
			get;
		}

		bool IsUpdateFirmwareSupported
		{
			get;
		}

		bool IsMemoryCardFormattingSupported
		{
			get;
		}
	}
}
