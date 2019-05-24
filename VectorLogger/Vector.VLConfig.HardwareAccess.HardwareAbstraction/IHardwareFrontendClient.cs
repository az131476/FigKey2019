using System;
using System.Collections.Generic;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public interface IHardwareFrontendClient
	{
		IHardwareFrontend HardwareFrontend
		{
			get;
			set;
		}

		void DeviceUpdated(ILoggerDevice device);

		void DevicesAdded(IList<ILoggerDevice> devices);

		void DevicesRemoved(IList<ILoggerDevice> devices);

		void AdditionalDrivesListChanged(IList<string> additionalDrivesList);
	}
}
