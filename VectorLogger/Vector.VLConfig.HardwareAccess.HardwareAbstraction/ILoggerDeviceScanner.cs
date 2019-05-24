using System;
using System.Collections.Generic;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	internal interface ILoggerDeviceScanner
	{
		List<string> AdditionalDrives
		{
			get;
			set;
		}

		bool IsDeviceWithoutMemoryCardConnected
		{
			get;
		}

		IList<ILoggerDevice> ScanForLoggerDevices();
	}
}
