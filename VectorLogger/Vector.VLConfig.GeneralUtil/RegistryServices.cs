using Microsoft.Win32;
using System;
using System.IO;

namespace Vector.VLConfig.GeneralUtil
{
	public class RegistryServices
	{
		private static readonly string _RegPath_GiN_LINprobe_EN = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\LINprobe configuration program";

		private static readonly string _RegPath_GiN_LINprobe_DE = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\LINprobe Konfigurationsprogramm";

		private static readonly string _RegPath_GiN_LINprobe_V2 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\G.i.N.\\LPconf";

		private static readonly string _RegPath_GiN_CANgps_EN = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\CANgps configuration program";

		private static readonly string _RegPath_GiN_CANgps_DE = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\CANgps Konfigurationsprogramm";

		private static readonly string _RegPath_GiN_CANgps_V2 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\G.i.N.\\GPSconf";

		private static readonly string _RegPath_GiN_CANshunt_EN = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\CANshunt configuration program";

		private static readonly string _RegPath_GiN_CANshunt_DE = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\CANshunt Konfigurationsprogramm";

		private static readonly string _RegPath_GiN_CANshunt_V2 = "HKEY_LOCAL_MACHINE\\SOFTWARE\\G.i.N.\\CSHconf";

		private static readonly string _GiN_valueName = "DisplayIcon";

		private static readonly string _GiN_valueName_V2 = "Exe";

		private static readonly string _RegPath_GiN_MLCenter = "HKEY_LOCAL_MACHINE\\SOFTWARE\\G.i.N.\\MLCenter";

		private static readonly string _RegPath_GiN_MLTools = "HKEY_LOCAL_MACHINE\\SOFTWARE\\G.i.N.\\MLtools";

		private static readonly string _GiN_path_valueName = "Path";

		private static readonly string _GiN_MLSetup_Name = "MLSetup.exe";

		private static readonly string _GiN_MLSetup_Name_V2 = "MLSet.exe";

		private static readonly string _RegPath_Vector_CANape = "HKEY_LOCAL_MACHINE\\SOFTWARE\\VECTOR\\CANape";

		private static readonly string _RegPath_Vector_vSignalyzer = "HKEY_LOCAL_MACHINE\\SOFTWARE\\VECTOR\\vSignalyzer";

		private static readonly string _Vector_path_valueName = "Path";

		public static bool IsGinLINprobeInstalled(out string pathOfApp)
		{
			return RegistryServices.IsGinAppInstalledV2(RegistryServices._RegPath_GiN_LINprobe_V2, out pathOfApp) || RegistryServices.IsGinAppInstalled(RegistryServices._RegPath_GiN_LINprobe_EN, out pathOfApp) || RegistryServices.IsGinAppInstalled(RegistryServices._RegPath_GiN_LINprobe_DE, out pathOfApp);
		}

		public static bool IsGinCANgpsInstalled(out string pathOfApp)
		{
			return RegistryServices.IsGinAppInstalledV2(RegistryServices._RegPath_GiN_CANgps_V2, out pathOfApp) || RegistryServices.IsGinAppInstalled(RegistryServices._RegPath_GiN_CANgps_EN, out pathOfApp) || RegistryServices.IsGinAppInstalled(RegistryServices._RegPath_GiN_CANgps_DE, out pathOfApp);
		}

		public static bool IsGinCANshuntInstalled(out string pathOfApp)
		{
			return RegistryServices.IsGinAppInstalledV2(RegistryServices._RegPath_GiN_CANshunt_V2, out pathOfApp) || RegistryServices.IsGinAppInstalled(RegistryServices._RegPath_GiN_CANshunt_EN, out pathOfApp) || RegistryServices.IsGinAppInstalled(RegistryServices._RegPath_GiN_CANshunt_DE, out pathOfApp);
		}

		public static bool IsGinMLSetupInstalled(out string pathOfApp)
		{
			return RegistryServices.IsGinMLSetupInstalled_Generic(RegistryServices._GiN_MLSetup_Name_V2, out pathOfApp) || RegistryServices.IsGinMLSetupInstalled_Generic(RegistryServices._GiN_MLSetup_Name, out pathOfApp);
		}

		public static bool IsVectorCANapeInstalled(out string pathOfApp)
		{
			return RegistryServices.IsVectorAppInstalled(RegistryServices._RegPath_Vector_CANape, out pathOfApp);
		}

		public static bool IsVectorvSignalyzerInstalled(out string pathOfApp)
		{
			return RegistryServices.IsVectorAppInstalled(RegistryServices._RegPath_Vector_vSignalyzer, out pathOfApp);
		}

		private static bool IsGinMLSetupInstalled_Generic(string mlSetupExeName, out string pathOfApp)
		{
			pathOfApp = "";
			object value = Registry.GetValue(RegistryServices._RegPath_GiN_MLCenter, RegistryServices._GiN_path_valueName, null);
			if (value != null)
			{
				string text;
				try
				{
					text = Path.Combine((string)value, mlSetupExeName);
				}
				catch (Exception)
				{
					bool result = false;
					return result;
				}
				if (File.Exists(text))
				{
					pathOfApp = text;
					return true;
				}
			}
			else
			{
				value = Registry.GetValue(RegistryServices._RegPath_GiN_MLTools, RegistryServices._GiN_path_valueName, null);
				if (value != null)
				{
					string text;
					try
					{
						text = Path.Combine((string)value, mlSetupExeName);
					}
					catch (Exception)
					{
						bool result = false;
						return result;
					}
					if (File.Exists(text))
					{
						pathOfApp = text;
						return true;
					}
				}
			}
			return false;
		}

		private static bool IsGinAppInstalled(string registryPath, out string pathOfApp)
		{
			pathOfApp = "";
			object value = Registry.GetValue(registryPath, RegistryServices._GiN_valueName, null);
			if (value != null)
			{
				string text;
				try
				{
					text = (string)value;
				}
				catch (Exception)
				{
					bool result = false;
					return result;
				}
				if (File.Exists(text))
				{
					pathOfApp = text;
					return true;
				}
				return false;
			}
			return false;
		}

		private static bool IsGinAppInstalledV2(string registryPath, out string pathOfApp)
		{
			pathOfApp = "";
			object value = Registry.GetValue(registryPath, RegistryServices._GiN_valueName_V2, null);
			if (value != null)
			{
				string text;
				try
				{
					text = (string)value;
				}
				catch (Exception)
				{
					bool result = false;
					return result;
				}
				if (File.Exists(text))
				{
					pathOfApp = text;
					return true;
				}
				return false;
			}
			return false;
		}

		private static bool IsVectorAppInstalled(string registryPath, out string pathOfApp)
		{
			pathOfApp = string.Empty;
			object value = Registry.GetValue(registryPath, RegistryServices._Vector_path_valueName, null);
			if (value != null)
			{
				string text;
				try
				{
					text = (string)value;
				}
				catch (Exception)
				{
					bool result = false;
					return result;
				}
				if (Directory.Exists(text))
				{
					pathOfApp = text;
					return true;
				}
				return false;
			}
			return false;
		}
	}
}
