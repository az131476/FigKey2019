using System;
using System.Collections.Generic;
using System.IO;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public class GL1000Scanner : ILoggerDeviceScanner
	{
		private static ILoggerSpecifics _loggerSpecifics;

		private static List<string> _additionalDrives;

		private static bool _isDeviceWithoutMemoryCardConnected;

		public static ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				if (GL1000Scanner._loggerSpecifics == null)
				{
					GL1000Scanner._loggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(LoggerType.GL1000);
				}
				return GL1000Scanner._loggerSpecifics;
			}
		}

		List<string> ILoggerDeviceScanner.AdditionalDrives
		{
			get
			{
				return GL1000Scanner._additionalDrives;
			}
			set
			{
				GL1000Scanner._additionalDrives = value;
			}
		}

		bool ILoggerDeviceScanner.IsDeviceWithoutMemoryCardConnected
		{
			get
			{
				return GL1000Scanner._isDeviceWithoutMemoryCardConnected;
			}
		}

		IList<ILoggerDevice> ILoggerDeviceScanner.ScanForLoggerDevices()
		{
			IList<ILoggerDevice> list = new List<ILoggerDevice>();
			GL1000Scanner._isDeviceWithoutMemoryCardConnected = false;
			GL1000ctrl gL1000ctrl = new GL1000ctrl();
			IList<string> list2 = null;
			IList<string> list3 = null;
			bool availableLoggerDrives = gL1000ctrl.GetAvailableLoggerDrives(out list2, out list3);
			int num = 0;
			if (availableLoggerDrives)
			{
				IEnumerable<DriveInfo> uSBCardReaderDrivesOfModel = FileSystemServices.GetUSBCardReaderDrivesOfModel(GL1000Scanner.LoggerSpecifics.DeviceAccess.InternalCardReaderModelNames, GL1000Scanner.LoggerSpecifics.DeviceAccess.VID_PID, false);
				foreach (string current in list2)
				{
					foreach (DriveInfo current2 in uSBCardReaderDrivesOfModel)
					{
						if (current2.Name.IndexOf(current) == 0)
						{
							ILoggerDevice item = new GL1000Device(current, true);
							list.Add(item);
							num++;
							break;
						}
					}
				}
				foreach (string current3 in list3)
				{
					if (!list2.Contains(current3))
					{
						ILoggerDevice item2 = new GL1000Device(current3, false);
						list.Add(item2);
					}
				}
			}
			if (list3 != null && list2.Count > 0 && num == 0)
			{
				GL1000Scanner._isDeviceWithoutMemoryCardConnected = true;
			}
			return list;
		}
	}
}
