using System;
using System.Collections.Generic;
using System.IO;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	internal class GL4000Scanner : ILoggerDeviceScanner
	{
		private static ILoggerSpecifics _loggerSpecifics;

		private static List<string> _additionalDrives;

		public static ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				if (GL4000Scanner._loggerSpecifics == null)
				{
					GL4000Scanner._loggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(LoggerType.GL4000);
				}
				return GL4000Scanner._loggerSpecifics;
			}
		}

		List<string> ILoggerDeviceScanner.AdditionalDrives
		{
			get
			{
				return GL4000Scanner._additionalDrives;
			}
			set
			{
				GL4000Scanner._additionalDrives = value;
			}
		}

		bool ILoggerDeviceScanner.IsDeviceWithoutMemoryCardConnected
		{
			get
			{
				return false;
			}
		}

		IList<ILoggerDevice> ILoggerDeviceScanner.ScanForLoggerDevices()
		{
			IList<ILoggerDevice> list = new List<ILoggerDevice>();
			IEnumerable<DriveInfo> uSBCardReaderDrivesOfModel = FileSystemServices.GetUSBCardReaderDrivesOfModel(GL4000Scanner.LoggerSpecifics.DeviceAccess.InternalCardReaderModelNames, GL4000Scanner.LoggerSpecifics.DeviceAccess.VID_PID, true);
			List<string> list2 = new List<string>();
			foreach (DriveInfo current in uSBCardReaderDrivesOfModel)
			{
				ILoggerDevice item = new GL4000Device(current.Name, true);
				list2.Add(current.Name);
				list.Add(item);
			}
			IEnumerable<DriveInfo> availableRemovableDrives = FileSystemServices.GetAvailableRemovableDrives(GL4000Scanner.LoggerSpecifics.DeviceAccess.AccessCardReaderDrivesOnly);
			foreach (DriveInfo current2 in availableRemovableDrives)
			{
				if (!list2.Contains(current2.Name) && MlRtIniFile.IsMemoryCardContentCompatibleToLogger(current2.Name, GL4000Scanner.LoggerSpecifics))
				{
					ILoggerDevice item2 = new GL4000Device(current2.Name, false);
					list.Add(item2);
				}
			}
			if (GL4000Scanner._additionalDrives != null)
			{
				foreach (string current3 in GL4000Scanner._additionalDrives)
				{
					bool flag = false;
					foreach (ILoggerDevice current4 in list)
					{
						if (current3.IndexOf(current4.HardwareKey) == 0)
						{
							flag = true;
							break;
						}
					}
					if (!flag && MlRtIniFile.IsMemoryCardContentCompatibleToLogger(current3, GL4000Scanner.LoggerSpecifics))
					{
						ILoggerDevice item3 = new GL4000Device(current3, false);
						list.Add(item3);
					}
				}
			}
			return list;
		}
	}
}
