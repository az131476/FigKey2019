using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public class MlRtIniFile
	{
		public static readonly string Key_LogErrorFrames = "LogErrorFrames";

		public static readonly string Key_Logger1GlobalStamps = "Logger1GlobalStamps";

		public static readonly string Key_Logger1SingleFile = "Logger1SingleFile";

		public static readonly string Key_DevType = "DevType";

		public static readonly string Key_PartialDownload = "PartialDownload";

		public static readonly string Key_RTSversion = "RTSversion";

		public static readonly string Key_CompilationTimeStamp = "CompilationTimeStamp";

		public static readonly string Key_License_Num = "License{0}";

		public static readonly string Key_HostCamEnabled_Num = "HostCam{0}Enabled";

		public static readonly string Key_HostCamSerNum_Num = "HostCam{0}SerNum";

		public static readonly string Key_CAN1baby = "CAN1baby";

		public static readonly string Key_CAN2baby = "CAN2baby";

		public static readonly string Key_CAN_num_baby = "CAN{0}baby";

		public static readonly string Key_FileName = "FileName";

		public static readonly string Key_FilePath = "FilePath";

		public static readonly string Key_DataDate = "DataDate";

		public static readonly string Key_SerNum = "SerNum";

		public static readonly string Key_StandardPretrigger = "StandardPretrigger";

		public static readonly string Key_ExtensionBoard_Num = "ExtensionBoard{0}";

		public static readonly string Key_LogDiskInfo = "LogDiskInfo";

		public static readonly string Key_LocalCarName = "LocalCarName";

		public static readonly string Value_AnalogExtensionBoard = "A8I";

		public static readonly string Value_WLANExtensionBoard_Prefix = "WLAN(";

		public static CANTransceiverType ParseTransceiverEncoding(string encoding)
		{
			if (string.IsNullOrEmpty(encoding))
			{
				return CANTransceiverType.None;
			}
			if (encoding.CompareTo("---") == 0)
			{
				return CANTransceiverType.None;
			}
			if (encoding.Contains("H/251"))
			{
				return CANTransceiverType.PCA82C251;
			}
			if (encoding.Contains("H/1050"))
			{
				return CANTransceiverType.TJA1050;
			}
			if (encoding.Contains("HT/1040"))
			{
				return CANTransceiverType.TJA1040;
			}
			if (encoding.Contains("HT/6251D"))
			{
				return CANTransceiverType.TLE6251DS;
			}
			if (encoding.Contains("HT/651040"))
			{
				return CANTransceiverType.SN65HVD1040;
			}
			if (encoding.Contains("Hw/1041A+"))
			{
				return CANTransceiverType.TJA1041A;
			}
			if (encoding.Contains("Hw/1041+"))
			{
				return CANTransceiverType.TJA1041Plus;
			}
			if (encoding.Contains("Hw/1041"))
			{
				return CANTransceiverType.TJA1041;
			}
			if (encoding.Contains("Lw/1054"))
			{
				return CANTransceiverType.TJA1054;
			}
			if (encoding.Contains("Sw/6255"))
			{
				return CANTransceiverType.TLE6255;
			}
			if (encoding.Contains("Tw/WCLA"))
			{
				return CANTransceiverType.WabcoWCLA;
			}
			if (encoding.Contains("HTw/1041A"))
			{
				return CANTransceiverType._1041A;
			}
			if (encoding.Contains("HTw/6251G"))
			{
				return CANTransceiverType.TLE6251G;
			}
			if (encoding.Contains("HIw/1041A"))
			{
				return CANTransceiverType.TJA1041A_Isol;
			}
			if (encoding.Contains("HIw/1041"))
			{
				return CANTransceiverType.TJA1041_Isol;
			}
			if (encoding.Contains("LIw/1054"))
			{
				return CANTransceiverType.TJA1054_Isol;
			}
			if (encoding.Contains("H/1042"))
			{
				return CANTransceiverType.TJA1042;
			}
			if (encoding.Contains("Hw/1043"))
			{
				return CANTransceiverType.TJA1043;
			}
			if (encoding.Contains("HIw/1043"))
			{
				return CANTransceiverType.TJA1043_Isol;
			}
			if (encoding.Contains("Lw/1055"))
			{
				return CANTransceiverType.TJA1055;
			}
			if (encoding.Contains("LIw/1055"))
			{
				return CANTransceiverType.TJA1055Isol;
			}
			if (encoding.Contains("HI/3053"))
			{
				return CANTransceiverType.AD3053;
			}
			return CANTransceiverType.Unknown;
		}

		public static bool ParseDateTimeField(string dateTimeString, ref DateTime parsedDateTimeValue)
		{
			if (string.IsNullOrEmpty(dateTimeString))
			{
				return false;
			}
			string[] array = dateTimeString.Split(new char[]
			{
				' ',
				':',
				'.'
			});
			if (array.Count<string>() == 6)
			{
				parsedDateTimeValue = new DateTime(Convert.ToInt32(array[2]), Convert.ToInt32(array[1]), Convert.ToInt32(array[0]), Convert.ToInt32(array[3]), Convert.ToInt32(array[4]), Convert.ToInt32(array[5]));
				return true;
			}
			return false;
		}

		public static bool IsMemoryCardContentCompatibleToLogger(string memCardPath, ILoggerSpecifics loggerSpecifics)
		{
			string strB = "0x" + loggerSpecifics.DeviceAccess.DeviceType.ToString("X");
			bool result = false;
			string text = Path.Combine(memCardPath, loggerSpecifics.DataStorage.LogDataIniFileName);
			string text2 = Path.Combine(memCardPath, loggerSpecifics.DataStorage.LogDataIniFile2Name);
			if (FileProxy.Exists(text))
			{
				string strA;
				if (FileSystemServices.GetIniFilePropertyValue(text, MlRtIniFile.Key_DevType, out strA) && string.Compare(strA, strB) == 0)
				{
					result = true;
				}
			}
			else if (FileProxy.Exists(text2))
			{
				string strA;
				if (FileSystemServices.GetIniFilePropertyValue(text2, MlRtIniFile.Key_DevType, out strA) && string.Compare(strA, strB) == 0)
				{
					result = true;
				}
			}
			else
			{
				string path = Path.Combine(memCardPath, Vocabulary.FileNameCompiledCAPL);
				result = !FileProxy.Exists(path);
			}
			return result;
		}

		public static ILoggerSpecifics LoadLoggerTypeFromMemoryCardContent(string memCardPath)
		{
			bool flag = false;
			bool flag2 = false;
			string text = Path.Combine(memCardPath, Constants.LogDataIniFileName);
			string text2 = Path.Combine(memCardPath, Constants.LogDataIniFile2Name);
			string text3 = "";
			if (FileProxy.Exists(text))
			{
				if (FileSystemServices.GetIniFilePropertyValue(text, MlRtIniFile.Key_DevType, out text3))
				{
					flag = true;
				}
			}
			else if (FileProxy.Exists(text2))
			{
				if (FileSystemServices.GetIniFilePropertyValue(text2, MlRtIniFile.Key_DevType, out text3))
				{
					flag = true;
				}
			}
			else
			{
				string path = Path.Combine(memCardPath, Vocabulary.FileNameCompiledCAPL);
				if (File.Exists(path))
				{
					flag2 = true;
				}
			}
			bool flag3 = false;
			bool flag4 = false;
			try
			{
				if (memCardPath.Length < 2)
				{
					memCardPath = string.Format("{0}{1}{2}", memCardPath, Path.VolumeSeparatorChar, Path.DirectorySeparatorChar);
				}
				FileInfo fileInfo = new FileInfo(memCardPath);
				if (fileInfo.Directory != null)
				{
					DriveInfo driveInfo = new DriveInfo(fileInfo.Directory.Root.FullName);
					flag3 = (driveInfo.DriveFormat == Constants.FileSystemFormatFAT32 || driveInfo.DriveFormat == Constants.FileSystemFormatFAT || driveInfo.DriveFormat == Constants.FileSystemFormatNTFS);
				}
			}
			catch (Exception)
			{
				flag4 = true;
			}
			ILoggerSpecifics loggerSpecifics;
			if (flag)
			{
				uint devType = 0u;
				text3 = text3.Substring(2);
				bool flag5 = uint.TryParse(text3, NumberStyles.HexNumber, null, out devType);
				loggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(devType);
				if (!flag5 || loggerSpecifics == null)
				{
					loggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(LoggerType.GL4000);
				}
			}
			else if (flag2)
			{
				loggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(LoggerType.VN1630log);
			}
			else if (!flag3 && !flag4)
			{
				loggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(LoggerType.GL1000);
			}
			else
			{
				loggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(LoggerType.GL4000);
			}
			return loggerSpecifics;
		}
	}
}
