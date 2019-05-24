using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public interface IHardwareFrontend
	{
		IList<ILoggerDevice> CurrentLoggerDevices
		{
			get;
		}

		ReadOnlyCollection<ILoggerDevice> CurrentOnlineLoggerDevices
		{
			get;
		}

		ReadOnlyCollection<ILoggerDevice> CurrentOfflineLoggerDevices
		{
			get;
		}

		ReadOnlyCollection<ILoggerDevice> CurrentLoggerDevicesWithLoggerInfo
		{
			get;
		}

		ReadOnlyCollection<ILoggerDevice> CurrentLoggerDevicesWithIndexFile
		{
			get;
		}

		bool IsDeviceWithoutMemoryCardConnected
		{
			get;
		}

		LoggerType LoggerTypeToScan
		{
			get;
			set;
		}

		bool IsScanForAllLoggerTypesEnabled
		{
			get;
		}

		List<string> AdditionalDrives
		{
			get;
			set;
		}

		ILoggerDevice PrimaryOnlineDevice
		{
			get;
		}

		ILoggerDevice GetLoggerDeviceByDriveName(string driveName);

		void UpdateLoggerDeviceList();

		bool EnforceExplicitUpdateOfLoggerDevice(ILoggerDevice scannedLoggerDevice);

		bool IsCustomLoggerDevice(ILoggerDevice device);

		ILoggerDevice CreateCustomLoggerDevice(LoggerType type, string drivePath, IHardwareFrontendClient client);

		bool SetCustomLoggerDevice(ILoggerDevice device, IHardwareFrontendClient client);

		bool ReleaseCustomLoggerDevice(IHardwareFrontendClient client);

		void RegisterClient(IHardwareFrontendClient client);

		void EnableClientNotification(bool isEnabled);

		void EnableDeviceWatcher(ILoggerDevice device, bool isEnabled);

		bool Eject(ILoggerDevice device);

		void Dispose();
	}
}
