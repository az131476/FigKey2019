using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.LoggerSpecifics;
using Vector.XlApiNet;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction.VN16XXlog
{
	public class VN16XXlogScanner : ILoggerDeviceScanner
	{
		private const byte cDefaultHwType = 79;

		private const string cLogDataFolderNamePrefix = "!D1F";

		private static ILoggerSpecifics sLoggerSpecifics;

		private static List<string> sAdditionalDrives;

		private static readonly Dictionary<int, VN16XXlogHardwareInfo> sHwInfos = new Dictionary<int, VN16XXlogHardwareInfo>();

		public static ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				ILoggerSpecifics arg_18_0;
				if ((arg_18_0 = VN16XXlogScanner.sLoggerSpecifics) == null)
				{
					arg_18_0 = (VN16XXlogScanner.sLoggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(LoggerType.VN1630log));
				}
				return arg_18_0;
			}
		}

		public List<string> AdditionalDrives
		{
			get
			{
				return VN16XXlogScanner.sAdditionalDrives;
			}
			set
			{
				VN16XXlogScanner.sAdditionalDrives = value;
			}
		}

		bool ILoggerDeviceScanner.IsDeviceWithoutMemoryCardConnected
		{
			get
			{
				return false;
			}
		}

		public static VN16XXlogHardwareInfo GetHardwareInfo(byte hwType, byte hwIndex)
		{
			int key = VN16XXlogScanner.GetKey(hwType, hwIndex);
			VN16XXlogHardwareInfo vN16XXlogHardwareInfo = VN16XXlogScanner.sHwInfos.ContainsKey(key) ? VN16XXlogScanner.sHwInfos[key] : null;
			if (vN16XXlogHardwareInfo == null)
			{
				return null;
			}
			XlApi xlApi = new XlApi();
			vN16XXlogHardwareInfo.CobInfo = VN16XXlogScanner.GetCobFrameworkInfo(xlApi, vN16XXlogHardwareInfo);
			return vN16XXlogHardwareInfo;
		}

		public IList<ILoggerDevice> ScanForLoggerDevices()
		{
			VN16XXlogScanner.sHwInfos.Clear();
			IList<string> list = new List<string>();
			IList<ILoggerDevice> list2 = new List<ILoggerDevice>();
			XlApi xlApi = new XlApi();
			VN16XXlogScanner.CollectHwInfos(xlApi, VN16XXlogScanner.sHwInfos);
			foreach (VN16XXlogHardwareInfo current in VN16XXlogScanner.sHwInfos.Values)
			{
				string arg;
				if (xlApi.XlLogGetMountPoint((uint)current.HwType, (uint)current.HwIndex, out arg) == XlStatus.XL_SUCCESS)
				{
					string text = arg + Path.VolumeSeparatorChar + Path.DirectorySeparatorChar;
					DriveInfo driveInfo = null;
					try
					{
						driveInfo = new DriveInfo(text);
					}
					catch
					{
						continue;
					}
					if (driveInfo.IsReady)
					{
						list.Add(text);
						list2.Add(new VN16XXlogDevice(current.HwType, current.HwIndex, text, true));
					}
				}
				current.CobInfo = VN16XXlogScanner.GetCobFrameworkInfo(xlApi, current);
			}
			IEnumerable<DriveInfo> availableRemovableDrives = FileSystemServices.GetAvailableRemovableDrives(VN16XXlogScanner.LoggerSpecifics.DeviceAccess.AccessCardReaderDrivesOnly);
			foreach (DriveInfo current2 in availableRemovableDrives)
			{
				if (!list.Contains(current2.Name) && VN16XXlogScanner.IsMemoryCardContentCompatibleToLogger(current2.Name))
				{
					list2.Add(new VN16XXlogDevice(79, 255, current2.Name, false));
				}
			}
			if (VN16XXlogScanner.sAdditionalDrives != null)
			{
				using (List<string>.Enumerator enumerator3 = VN16XXlogScanner.sAdditionalDrives.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						string tmpDrive = enumerator3.Current;
						if (!list2.Any((ILoggerDevice t) => tmpDrive.IndexOf(t.HardwareKey, StringComparison.Ordinal) == 0) && VN16XXlogScanner.IsMemoryCardContentCompatibleToLogger(tmpDrive))
						{
							list2.Add(new VN16XXlogDevice(79, 255, tmpDrive, false));
						}
					}
				}
			}
			return list2;
		}

		private static bool IsVn16XXlogDevice(XlHwType hwType)
		{
			return hwType == XlHwType.XL_HWTYPE_VN1630_LOG;
		}

		private static int GetKey(byte hwType, byte hwIndex)
		{
			return (int)hwType << 8 | (int)hwIndex;
		}

		public static bool IsMemoryCardContentCompatibleToLogger(string memCardPath)
		{
			DirectoryInfo directoryInfo;
			try
			{
				directoryInfo = new DirectoryInfo(memCardPath);
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			if (!directoryInfo.Exists)
			{
				return false;
			}
			if (!DirectoryProxy.Exists(memCardPath))
			{
				return false;
			}
			string[] source = FileProxy.ExcludeByAttributes(DirectoryProxy.GetFiles(memCardPath), FileAttributes.Hidden | FileAttributes.System);
			if (source.Any((string t) => !t.EndsWith(Vocabulary.FileNameCompiledCAPL) && !t.EndsWith(Vocabulary.FileNameVN1630logErrorLog)))
			{
				return false;
			}
			string[] source2 = DirectoryProxy.ExcludeByAttributes(DirectoryProxy.GetDirectories(memCardPath), FileAttributes.Hidden | FileAttributes.System);
			return !source2.Any((string t) => !(Path.GetFileName(t) ?? string.Empty).StartsWith("!D1F") && !t.EndsWith(Vocabulary.FolderNameProjectPackage));
		}

		private static void CollectHwInfos(XlApi xlApi, Dictionary<int, VN16XXlogHardwareInfo> devices)
		{
			devices.Clear();
			s_xl_driver_config s_xl_driver_config;
			if (xlApi.XlGetDriverConfig(out s_xl_driver_config) != XlStatus.XL_SUCCESS)
			{
				return;
			}
			uint channelCount = s_xl_driver_config.channelCount;
			for (uint num = 0u; num < channelCount; num += 1u)
			{
				s_xl_channel_config item = s_xl_driver_config.channel[(int)((UIntPtr)num)];
				if (VN16XXlogScanner.IsVn16XXlogDevice((XlHwType)item.hwType))
				{
					int key = VN16XXlogScanner.GetKey(item.hwType, item.hwIndex);
					if (!devices.ContainsKey(key))
					{
						devices.Add(key, new VN16XXlogHardwareInfo(item.hwType, item.hwIndex));
					}
					devices[key].ChannelInfos.Add(item);
				}
			}
		}

		private static s_xl_cobfwk_info GetCobFrameworkInfo(XlApi xlApi, VN16XXlogHardwareInfo hwInfo)
		{
			s_xl_cobfwk_info result = default(s_xl_cobfwk_info).Init();
			foreach (s_xl_channel_config current in hwInfo.ChannelInfos)
			{
				if (current.busParams.busType == 1u)
				{
					using (XlPort xlPort = new XlPort(xlApi, Application.ProductName, current.channelMask, 131072u, 0u, current.busParams.busType))
					{
						if (xlPort.Status == XlStatus.XL_SUCCESS)
						{
							s_xl_cobfwk_info s_xl_cobfwk_info = default(s_xl_cobfwk_info);
							if (xlApi.XlCobFwkQueryFwkInfo(xlPort.PortHandle, xlPort.AccessMask, out s_xl_cobfwk_info) == XlStatus.XL_SUCCESS)
							{
								result = s_xl_cobfwk_info;
								break;
							}
						}
					}
				}
			}
			return result;
		}
	}
}
