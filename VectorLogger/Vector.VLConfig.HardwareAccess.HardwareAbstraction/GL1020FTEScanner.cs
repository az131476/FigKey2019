using System;
using System.Collections.Generic;
using System.IO;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public class GL1020FTEScanner : ILoggerDeviceScanner
	{
		private static ILoggerSpecifics _loggerSpecifics;

		private static List<string> _additionalDrives;

		public static ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				if (GL1020FTEScanner._loggerSpecifics == null)
				{
					GL1020FTEScanner._loggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(LoggerType.GL1020FTE);
				}
				return GL1020FTEScanner._loggerSpecifics;
			}
		}

		List<string> ILoggerDeviceScanner.AdditionalDrives
		{
			get
			{
				return GL1020FTEScanner._additionalDrives;
			}
			set
			{
				GL1020FTEScanner._additionalDrives = value;
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
			GL1000ctrl gL1000ctrl = new GL1000ctrl();
			IList<string> list2 = null;
			IList<string> list3 = null;
			bool availableLoggerDrives = gL1000ctrl.GetAvailableLoggerDrives(out list2, out list3);
			if (availableLoggerDrives)
			{
				IEnumerable<DriveInfo> uSBCardReaderDrivesOfModel = FileSystemServices.GetUSBCardReaderDrivesOfModel(GL1020FTEScanner.LoggerSpecifics.DeviceAccess.InternalCardReaderModelNames, GL1020FTEScanner.LoggerSpecifics.DeviceAccess.VID_PID, false);
				foreach (string current in list2)
				{
					foreach (DriveInfo current2 in uSBCardReaderDrivesOfModel)
					{
						if (current2.Name.IndexOf(current) == 0)
						{
							ILoggerDevice item = new GL1020FTEDevice(current, true);
							list.Add(item);
							break;
						}
					}
				}
				foreach (string current3 in list3)
				{
					if (!list2.Contains(current3))
					{
						ILoggerDevice item2 = new GL1020FTEDevice(current3, false);
						list.Add(item2);
					}
				}
			}
			return list;
		}
	}
}
