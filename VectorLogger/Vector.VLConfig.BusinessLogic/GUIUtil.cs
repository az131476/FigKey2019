using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI;
using Vector.VLConfig.GUI.CcpXcpDescriptionsPage;
using Vector.VLConfig.GUI.CcpXcpSignalRequestsPage;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.GUI.Common.DeviceInteraction;
using Vector.VLConfig.GUI.Common.EventConditions;
using Vector.VLConfig.GUI.DatabasesPage;
using Vector.VLConfig.GUI.DiagnosticActionsPage;
using Vector.VLConfig.GUI.DiagnosticsDatabasesPage;
using Vector.VLConfig.GUI.FiltersPage;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.GUI.IncludeFilesPage;
using Vector.VLConfig.GUI.SendMessagePage;
using Vector.VLConfig.GUI.TriggersPage;
using Vector.VLConfig.GUI.WLANSettingsPage;
using Vector.VLConfig.HardwareAccess;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic
{
	public static class GUIUtil
	{
		public enum FileDropContent
		{
			File,
			Files,
			Folder,
			Folders,
			FilesAndFolders,
			IllegalDrop
		}

		private const string _LINTransceiverTypeName_TJA1020 = "TJA1020";

		private const string _LINTransceiverTypeName_TJA1021 = "TJA1021";

		private const string _LINTransceiverTypeName_TJA1020_or_TJA1021 = "TJA1020 / TJA1021";

		public const string cEventTypeCombinedEvent = "combined event";

		private static bool _isHexadecimal;

		private static readonly string SolidStateDisk;

		private static IList<uint> _standardCANBaudrates;

		private static IList<uint> _standardCANFDArbBaudrates;

		private static IList<uint> _standardCANFDDataBaudrates;

		private static IList<uint> _standardLINBaudrates;

		private static IList<uint> _standardJ1708Baudrates;

		public static readonly string UserdefBaudrateValueEntryPostfix;

		private static IList<uint> _standardAnalogInputFrequencies;

		private static IEnumerable<string> sFilterTypeNamesInMenu;

		private static IList<string> sEventTypeNamesInMenu;

		private static IList<string> sEventTypeNamesInCol;

		private static IList<uint> _memoryCardSizes;

		private static IList<uint> _ringBufferSizes;

		private static IList<uint> _maxLogFileSizes;

		private static readonly string _HexDigits;

		private static readonly int HelpPageID_DeviceInformation;

		private static readonly int HelpPageID_Comment;

		private static readonly int HelpPageID_HardwareSettings;

		private static readonly int HelpPageID_CANChannels;

		private static readonly int HelpPageID_LINChannels;

		private static readonly int HelpPageID_FlexRayChannels;

		private static readonly int HelpPageID_MOST150Channels;

		private static readonly int HelpPageID_MultibusChannels;

		public static readonly int HelpPageID_CANHardwareSettings;

		private static readonly int HelpPageID_AnalogInputs;

		private static readonly int HelpPageID_DigitalInputs;

		private static readonly int HelpPageID_CombinedAnalogDigitalInputs;

		private static readonly int HelpPageID_LEDs;

		private static readonly int HelpPageID_InterfaceMode;

		private static readonly int HelpPageID_Databases;

		private static readonly int HelpPageID_CCPXCP;

		private static readonly int HelpPageID_CcpXcpSignalRequests;

		private static readonly int HelpPageID_CcpXcpProtocolSettings;

		private static readonly int HelpPageID_CcpXcpProtocolSelection;

		private static readonly int HelpPageID_CcpXcpSymbolSelection;

		private static readonly int HelpPageID_CcpXcpSignalActions;

		private static readonly int HelpPageID_Filters;

		private static readonly int HelpPageID_Triggers;

		private static readonly int HelpPageID_BufferSizeCalculator;

		private static readonly int HelpPageID_EventsConditions;

		private static readonly int HelpPageID_SpecialFeatures;

		private static readonly int HelpPageID_IncludeFiles;

		private static readonly int HelpPageID_WLANSettings;

		private static readonly int HelpPageID_DiagnosticsDatabases;

		private static readonly int HelpPageID_DiagCommParameters;

		private static readonly int HelpPageID_DiagECUSelection;

		private static readonly int HelpPageID_DiagnosticsActions;

		public static readonly int HelpPageID_DiagnosticsServiceParameterDialog;

		public static readonly int HelpPageID_DiagnosticsSignalSelectionDialog;

		private static readonly int HelpPageID_LoggerDevice;

		private static readonly int HelpPageID_LoggerDeviceNavigator;

		private static readonly int HelpPageID_CardReader;

		private static readonly int HelpPageID_CardReaderNavigator;

		private static readonly int HelpPageID_CLFExport;

		private static readonly int HelpPageID_SendMessage;

		private static readonly int HelpPageID_SendMessageDialog;

		private static readonly int HelpPageID_DigitalOutputs;

		private static readonly int HelpPageID_RealtimeClock;

		private static readonly int HelpPageID_EthernetAddress;

		public static readonly int HelpPageID_AddCardReaderDrives;

		private static readonly int HelpPageID_VehicleName;

		private static readonly int HelpPageID_GPS;

		private static readonly int HelpPageID_CANgps;

		public static readonly int HelpPageID_OptionsDialog;

		public static readonly int HelpPageID_PackAndGo;

		private static readonly int HelpPageID_ExportDatabases;

		private static double sGuiScaleFactorX;

		private static double sGuiScaleFactorY;

		private static double sDPIScaleFactorX;

		private static double sDPIScaleFactorY;

		private static IList<string> sWebDisplayExportSignalTypeNamesInMenu;

		private static IList<string> sWebDisplayExportSignalTypeNamesInCol;

		public static bool IsHexadecimal
		{
			get
			{
				return GUIUtil._isHexadecimal;
			}
			set
			{
				GUIUtil._isHexadecimal = value;
			}
		}

		public static string EditUserdefDropdownEntry
		{
			get
			{
				return Resources.UserDefinedDropdownEntry;
			}
		}

		private static IEnumerable<string> FilterTypeNamesInMenu
		{
			get
			{
				IEnumerable<string> arg_87_0;
				if ((arg_87_0 = GUIUtil.sFilterTypeNamesInMenu) == null)
				{
					arg_87_0 = (GUIUtil.sFilterTypeNamesInMenu = new List<string>
					{
						Resources.FilterTypeNameCANChn,
						Resources.FilterTypeNameLINChn,
						Resources.FilterTypeNameFlexRayChn,
						Resources.FilterTypeNameSymbolicCAN,
						Resources.FilterTypeNameSymbolicLIN,
						Resources.FilterTypeNameSymbolicFlexray,
						Resources.FilterTypeNameCANId,
						Resources.FilterTypeNameLINId,
						Resources.FilterTypeNameFlexrayId,
						Resources.FilterTypeNameCANSignalList
					});
				}
				return arg_87_0;
			}
		}

		private static IEnumerable<string> EventTypeNamesInMenu
		{
			get
			{
				IList<string> arg_12C_0;
				if ((arg_12C_0 = GUIUtil.sEventTypeNamesInMenu) == null)
				{
					arg_12C_0 = (GUIUtil.sEventTypeNamesInMenu = new List<string>
					{
						"combined event",
						Vocabulary.TriggerTypeNameColOnStart,
						Resources_Trigger.TriggerTypeNameColOnCycTimer,
						Resources_Trigger.TriggerTypeNameColSymbolicCAN,
						Resources_Trigger.TriggerTypeNameColSymbolicLIN,
						Resources_Trigger.TriggerTypeNameColSymbolicFlexray,
						Resources_Trigger.TriggerTypeNameColSymbolicSigCAN,
						Resources_Trigger.TriggerTypeNameColSymbolicSigLIN,
						Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray,
						Vocabulary.TriggerTypeNameColCANId,
						Vocabulary.TriggerTypeNameColLINId,
						Resources_Trigger.TriggerTypeNameColFlexray,
						Resources_Trigger.TriggerTypeNameColCANData,
						Resources_Trigger.TriggerTypeNameColLINData,
						Resources_Trigger.TriggerTypeNameColCANBusStatistics,
						Resources_Trigger.TriggerTypeNameColCcpXcpSignal,
						Resources_Trigger.TriggerTypeNameColDiagnosticSignal,
						Resources_Trigger.TriggerTypeNameColAnalogInput,
						Resources_Trigger.TriggerTypeNameColDigitalInput,
						Resources_Trigger.TriggerTypeNameColIgnition,
						Resources_Trigger.TriggerTypeNameColKey,
						Resources_Trigger.TriggerTypeNameColCANMsgTimeout,
						Resources_Trigger.TriggerTypeNameColLINMsgTimeout,
						Resources_Trigger.TriggerTypeNameColVoCanRecording,
						Resources_Trigger.TriggerTypeNameColIncEvent
					});
				}
				return arg_12C_0;
			}
		}

		public static IEnumerable<string> EventTypeNamesInCol
		{
			get
			{
				IList<string> arg_100_0;
				if ((arg_100_0 = GUIUtil.sEventTypeNamesInCol) == null)
				{
					arg_100_0 = (GUIUtil.sEventTypeNamesInCol = new List<string>
					{
						Resources_Trigger.TriggerTypeNameColSymbolicCAN,
						Resources_Trigger.TriggerTypeNameColSymbolicLIN,
						Resources_Trigger.TriggerTypeNameColSymbolicFlexray,
						Resources_Trigger.TriggerTypeNameColSymbolicSigCAN,
						Resources_Trigger.TriggerTypeNameColSymbolicSigLIN,
						Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray,
						Vocabulary.TriggerTypeNameColCANId,
						Vocabulary.TriggerTypeNameColLINId,
						Resources_Trigger.TriggerTypeNameColFlexray,
						Resources_Trigger.TriggerTypeNameColCANData,
						Resources_Trigger.TriggerTypeNameColLINData,
						Resources_Trigger.TriggerTypeNameColCANBusStatistics,
						Resources_Trigger.TriggerTypeNameColCcpXcpSignal,
						Resources_Trigger.TriggerTypeNameColDiagnosticSignal,
						Resources_Trigger.TriggerTypeNameColAnalogInput,
						Resources_Trigger.TriggerTypeNameColDigitalInput,
						Resources_Trigger.TriggerTypeNameColIgnition,
						Resources_Trigger.TriggerTypeNameColKey,
						Resources_Trigger.TriggerTypeNameColCANMsgTimeout,
						Resources_Trigger.TriggerTypeNameColLINMsgTimeout,
						Resources_Trigger.TriggerTypeNameColVoCanRecording
					});
				}
				return arg_100_0;
			}
		}

		private static IEnumerable<string> WebDisplayExportSignalTypeNamesInMenu
		{
			get
			{
				IList<string> arg_50_0;
				if ((arg_50_0 = GUIUtil.sWebDisplayExportSignalTypeNamesInMenu) == null)
				{
					arg_50_0 = (GUIUtil.sWebDisplayExportSignalTypeNamesInMenu = new List<string>
					{
						Resources_Trigger.TriggerTypeNameColSymbolicSigCAN,
						Resources_Trigger.TriggerTypeNameColSymbolicSigLIN,
						Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray,
						Resources.WebDisplayExportSignalTypeNameColLtlSystemInformation,
						Resources.WebDisplayExportSignalTypeNameColStatusVariable
					});
				}
				return arg_50_0;
			}
		}

		public static IEnumerable<string> WebDisplayExportSignalTypeNamesInCol
		{
			get
			{
				IList<string> arg_50_0;
				if ((arg_50_0 = GUIUtil.sWebDisplayExportSignalTypeNamesInCol) == null)
				{
					arg_50_0 = (GUIUtil.sWebDisplayExportSignalTypeNamesInCol = new List<string>
					{
						Resources_Trigger.TriggerTypeNameColSymbolicSigCAN,
						Resources_Trigger.TriggerTypeNameColSymbolicSigLIN,
						Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray,
						Resources.WebDisplayExportSignalTypeNameColLtlSystemInformation,
						Resources.WebDisplayExportSignalTypeNameColStatusVariable
					});
				}
				return arg_50_0;
			}
		}

		private static string GetNumberPrefix()
		{
			if (GUIUtil._isHexadecimal)
			{
				return "0x";
			}
			return "";
		}

		static GUIUtil()
		{
			GUIUtil.SolidStateDisk = "Solid State Disk";
			GUIUtil.UserdefBaudrateValueEntryPostfix = " *";
			GUIUtil._HexDigits = "0123456789abcdefABCDEF";
			GUIUtil.HelpPageID_DeviceInformation = 1000;
			GUIUtil.HelpPageID_Comment = 1500;
			GUIUtil.HelpPageID_HardwareSettings = 2000;
			GUIUtil.HelpPageID_CANChannels = 3000;
			GUIUtil.HelpPageID_LINChannels = 4000;
			GUIUtil.HelpPageID_FlexRayChannels = 5000;
			GUIUtil.HelpPageID_MOST150Channels = 5500;
			GUIUtil.HelpPageID_MultibusChannels = 5600;
			GUIUtil.HelpPageID_CANHardwareSettings = 5700;
			GUIUtil.HelpPageID_AnalogInputs = 6000;
			GUIUtil.HelpPageID_DigitalInputs = 6500;
			GUIUtil.HelpPageID_CombinedAnalogDigitalInputs = 6600;
			GUIUtil.HelpPageID_LEDs = 7000;
			GUIUtil.HelpPageID_InterfaceMode = 7500;
			GUIUtil.HelpPageID_Databases = 10000;
			GUIUtil.HelpPageID_CCPXCP = 11000;
			GUIUtil.HelpPageID_CcpXcpSignalRequests = 11100;
			GUIUtil.HelpPageID_CcpXcpProtocolSettings = 11200;
			GUIUtil.HelpPageID_CcpXcpProtocolSelection = 11300;
			GUIUtil.HelpPageID_CcpXcpSymbolSelection = 11400;
			GUIUtil.HelpPageID_CcpXcpSignalActions = 11500;
			GUIUtil.HelpPageID_Filters = 12000;
			GUIUtil.HelpPageID_Triggers = 13000;
			GUIUtil.HelpPageID_BufferSizeCalculator = 13100;
			GUIUtil.HelpPageID_EventsConditions = 13500;
			GUIUtil.HelpPageID_SpecialFeatures = 14000;
			GUIUtil.HelpPageID_IncludeFiles = 15000;
			GUIUtil.HelpPageID_WLANSettings = 16000;
			GUIUtil.HelpPageID_DiagnosticsDatabases = 17000;
			GUIUtil.HelpPageID_DiagCommParameters = 17100;
			GUIUtil.HelpPageID_DiagECUSelection = 17200;
			GUIUtil.HelpPageID_DiagnosticsActions = 18000;
			GUIUtil.HelpPageID_DiagnosticsServiceParameterDialog = 18100;
			GUIUtil.HelpPageID_DiagnosticsSignalSelectionDialog = 18200;
			GUIUtil.HelpPageID_LoggerDevice = 20000;
			GUIUtil.HelpPageID_LoggerDeviceNavigator = 20100;
			GUIUtil.HelpPageID_CardReader = 21000;
			GUIUtil.HelpPageID_CardReaderNavigator = 21100;
			GUIUtil.HelpPageID_CLFExport = 22000;
			GUIUtil.HelpPageID_SendMessage = 23000;
			GUIUtil.HelpPageID_SendMessageDialog = 23100;
			GUIUtil.HelpPageID_DigitalOutputs = 24000;
			GUIUtil.HelpPageID_RealtimeClock = 30000;
			GUIUtil.HelpPageID_EthernetAddress = 30100;
			GUIUtil.HelpPageID_AddCardReaderDrives = 30200;
			GUIUtil.HelpPageID_VehicleName = 30300;
			GUIUtil.HelpPageID_GPS = 31000;
			GUIUtil.HelpPageID_CANgps = 31100;
			GUIUtil.HelpPageID_OptionsDialog = 31400;
			GUIUtil.HelpPageID_PackAndGo = 32000;
			GUIUtil.HelpPageID_ExportDatabases = 33000;
			GUIUtil.sGuiScaleFactorX = 1.0;
			GUIUtil.sGuiScaleFactorY = 1.0;
			GUIUtil.sDPIScaleFactorX = 1.0;
			GUIUtil.sDPIScaleFactorY = 1.0;
			GUIUtil._isHexadecimal = false;
		}

		public static string GetTimeUnitString(TimeUnit unit)
		{
			switch (unit)
			{
			case TimeUnit.MilliSec:
				return Resources.MilliSecsShort;
			case TimeUnit.Sec:
				return Resources.SecsShort;
			case TimeUnit.Min:
				return Resources.MinShort;
			default:
				return "";
			}
		}

		public static string MapLoggerType2String(LoggerType loggerType)
		{
			switch (loggerType)
			{
			case LoggerType.Unknown:
				return Resources.Unknown;
			case LoggerType.GL1000:
				return string.Format(Resources.LoggerTypeSeries, "GL1000");
			case LoggerType.GL1020FTE:
				return "GL1020FTE";
			case LoggerType.GL2000:
				return string.Format(Resources.LoggerTypeSeries, "GL2000");
			case LoggerType.GL3000:
				return string.Format(Resources.LoggerTypeSeries, "GL3000");
			case LoggerType.GL4000:
				return string.Format(Resources.LoggerTypeSeries, "GL4000");
			case LoggerType.VN1630log:
				return "VN1630 log";
			default:
				return "";
			}
		}

		public static LoggerType MapString2LoggerType(string myLoggerName)
		{
			foreach (LoggerType loggerType in Enum.GetValues(typeof(LoggerType)))
			{
				string a = GUIUtil.MapLoggerType2String(loggerType);
				if (a == myLoggerName)
				{
					return loggerType;
				}
			}
			return LoggerType.GL1000;
		}

		public static string GetGL3PlusLINTransceiverTypeFromSerialNr(string serialNr)
		{
			string result = "TJA1020 / TJA1021";
			if (!string.IsNullOrEmpty(serialNr))
			{
				result = "TJA1020";
				string[] array = serialNr.Split(new char[]
				{
					'-'
				});
				uint num;
				if (array.Count<string>() > 1 && uint.TryParse(array[1], out num) && num % 100000u >= 500u)
				{
					result = "TJA1021";
				}
			}
			return result;
		}

		public static string GetGL10X0LINTranceiverTypeFromSerialNr(string serialNr)
		{
			string result = "TJA1020 / TJA1021";
			if (!string.IsNullOrEmpty(serialNr))
			{
				result = "TJA1020";
				string[] array = serialNr.Split(new char[]
				{
					'-'
				});
				if (array.Count<string>() > 1)
				{
					uint num;
					if (array[0].Contains("28069"))
					{
						if (uint.TryParse(array[1], out num) && num >= 2717u)
						{
							result = "TJA1021";
						}
					}
					else if (array[0].Contains("28081") && uint.TryParse(array[1], out num) && num >= 100621u)
					{
						result = "TJA1021";
					}
				}
			}
			return result;
		}

		public static string GetGL1000LoggerSubtypeNameFromSerialNr(string serialNr)
		{
			string result = "GL1000";
			if (!string.IsNullOrEmpty(serialNr))
			{
				string[] array = serialNr.Split(new char[]
				{
					'-'
				});
				if (array.Count<string>() > 1 && array[0].Contains("28081"))
				{
					result = "GL1010";
				}
			}
			return result;
		}

		public static string GetGL2000LoggerSubtypeNameFromSerialNr(string serialNr)
		{
			string result = "GL2000";
			if (!string.IsNullOrEmpty(serialNr))
			{
				string[] array = serialNr.Split(new char[]
				{
					'-'
				});
				if (array.Count<string>() > 1)
				{
					if (array[0].Contains("28106"))
					{
						result = "GL2010";
					}
					else if (array[0].Contains("28120"))
					{
						result = "GL2000 V2";
					}
				}
			}
			return result;
		}

		public static string GetGL3000LoggerSubtypeNameFromSerialNr(string serialNr, string logDiskInfo)
		{
			string result = "GL3X00";
			if (!string.IsNullOrEmpty(serialNr))
			{
				string[] array = serialNr.Split(new char[]
				{
					'-'
				});
				if (array.Count<string>() > 1)
				{
					if (array[0].Length >= 8)
					{
						if (!string.IsNullOrEmpty(logDiskInfo) && logDiskInfo.IndexOf(GUIUtil.SolidStateDisk) >= 0)
						{
							result = "GL3200";
						}
					}
					else
					{
						result = "GL3000";
						if (array[0].Contains("28087"))
						{
							result = "GL3100";
						}
						else if (array[0].Contains("28088"))
						{
							result = "GL3200";
						}
					}
				}
			}
			return result;
		}

		public static string GetGL4000LoggerSubtypeNameFromSerialNr(string serialNr, string logDiskInfo)
		{
			string result = "GL4X00";
			if (!string.IsNullOrEmpty(serialNr))
			{
				string[] array = serialNr.Split(new char[]
				{
					'-'
				});
				if (array.Count<string>() > 1)
				{
					if (array[0].Length >= 8)
					{
						if (!string.IsNullOrEmpty(logDiskInfo))
						{
							result = "GL4000";
							if (logDiskInfo.IndexOf(GUIUtil.SolidStateDisk) >= 0)
							{
								result = "GL4200";
							}
						}
					}
					else
					{
						result = "GL4000";
						if (array[0].Contains("28076"))
						{
							result = "GL4200";
						}
					}
				}
			}
			return result;
		}

		public static string MapTransceiverTypeToName(CANTransceiverType type)
		{
			string result;
			switch (type)
			{
			case CANTransceiverType.None:
				result = "no chip";
				break;
			case CANTransceiverType.PCA82C251:
				result = "PCA82C251 (CAN High-speed)";
				break;
			case CANTransceiverType.TJA1050:
				result = "TJA1050 (CAN High-speed)";
				break;
			case CANTransceiverType.TJA1040:
				result = "TJA1040 (CAN High-speed)";
				break;
			case CANTransceiverType.TLE6251DS:
				result = "TLE6251DS (CAN High-speed)";
				break;
			case CANTransceiverType.SN65HVD1040:
				result = "SN65HVD1040-Q1 (CAN High-speed)";
				break;
			case CANTransceiverType.TJA1041A:
				result = "TJA1041A (CAN High-speed)";
				break;
			case CANTransceiverType.TJA1041Plus:
				result = "TJA1041(+) (CAN High-speed)";
				break;
			case CANTransceiverType.TJA1041:
				result = "TJA1041 (CAN High-speed)";
				break;
			case CANTransceiverType.TJA1043:
				result = "TJA1043 (CAN High-speed)";
				break;
			case CANTransceiverType.TJA1054:
				result = "TJA1054 (CAN Low-speed)";
				break;
			case CANTransceiverType.TLE6255:
				result = "TLE6255 (CAN Single Wire)";
				break;
			case CANTransceiverType.WabcoWCLA:
				result = "Wabco WCLA (CAN Truck&Trailer)";
				break;
			case CANTransceiverType._1041A:
				result = "1041A (CAN High-speed)";
				break;
			case CANTransceiverType.TLE6251G:
				result = "TLE6251G (CAN High-speed)";
				break;
			case CANTransceiverType.TJA1041_Isol:
				result = "TJA1041 (CAN High-speed), galv. decoupled";
				break;
			case CANTransceiverType.TJA1041A_Isol:
				result = "TJA1041 (CAN High-speed), galv. decoupled";
				break;
			case CANTransceiverType.TJA1054_Isol:
				result = "TJA1054 (CAN Low-speed), galv. decoupled";
				break;
			case CANTransceiverType.TJA1042:
				result = "TJA1042 (CAN High-speed)";
				break;
			case CANTransceiverType.TJA1043_Isol:
				result = "TJA1043 (CAN High-speed), galv. decoupled";
				break;
			case CANTransceiverType.TJA1055:
				result = "TJA1055 (CAN Low-speed)";
				break;
			case CANTransceiverType.TJA1055Isol:
				result = "TJA1055 (CAN Low-speed), galv. decoupled";
				break;
			case CANTransceiverType.AD3053:
				result = "AD3053, galv. decoupled";
				break;
			case CANTransceiverType.TJA1041A_or_TJA1043:
				result = "TJA1041A / TJA1043 (CAN High-speed)";
				break;
			case CANTransceiverType.PCA82C251_or_TJA1042:
				result = "PCA82C251 / TJA1042 (CAN High-speed)";
				break;
			default:
				result = "Unknown";
				break;
			}
			return result;
		}

		public static IList<string> GetMultibusTypeNames(IList<BusType> busTypes)
		{
			List<string> list = new List<string>();
			foreach (BusType current in busTypes)
			{
				list.Add(GUIUtil.MapMultibusType2String(current));
			}
			return list;
		}

		public static string MapMultibusType2String(BusType type)
		{
			string result = "";
			switch (type)
			{
			case BusType.Bt_CAN:
				result = string.Format(Resources.BusTypeName, Vocabulary.CAN);
				break;
			case BusType.Bt_LIN:
				result = string.Format(Resources.BusTypeName, Vocabulary.LIN);
				break;
			case BusType.Bt_FlexRay:
				result = string.Format(Resources.BusTypeName, Vocabulary.Flexray);
				break;
			case BusType.Bt_J1708:
				result = string.Format(Resources.BusTypeName, Vocabulary.J1708);
				break;
			}
			return result;
		}

		public static BusType MapMultibusString2Type(string busTypeName)
		{
			foreach (BusType busType in Enum.GetValues(typeof(BusType)))
			{
				if (busTypeName == GUIUtil.MapMultibusType2String(busType))
				{
					return busType;
				}
			}
			return BusType.Bt_None;
		}

		public static IList<uint> GetJ1708MaxInterCharBitTimes()
		{
			List<uint> list = new List<uint>();
			for (uint num = 2u; num <= 9u; num += 1u)
			{
				list.Add(num);
			}
			return list;
		}

		public static string MapJ1708MaxInterCharBitTime2String(uint bitTime)
		{
			return string.Format(Resources.NumBits, bitTime);
		}

		public static uint MapString2J1708MaxInterCharBitTime(string text)
		{
			foreach (uint current in GUIUtil.GetJ1708MaxInterCharBitTimes())
			{
				if (text == GUIUtil.MapJ1708MaxInterCharBitTime2String(current))
				{
					return current;
				}
			}
			return 0u;
		}

		public static IList<uint> GetStandardCANBaudrates()
		{
			if (GUIUtil._standardCANBaudrates == null)
			{
				GUIUtil._standardCANBaudrates = new List<uint>();
				GUIUtil._standardCANBaudrates.Add(33333u);
				GUIUtil._standardCANBaudrates.Add(50000u);
				GUIUtil._standardCANBaudrates.Add(62500u);
				GUIUtil._standardCANBaudrates.Add(83333u);
				GUIUtil._standardCANBaudrates.Add(100000u);
				GUIUtil._standardCANBaudrates.Add(125000u);
				GUIUtil._standardCANBaudrates.Add(250000u);
				GUIUtil._standardCANBaudrates.Add(500000u);
				GUIUtil._standardCANBaudrates.Add(666666u);
				GUIUtil._standardCANBaudrates.Add(800000u);
				GUIUtil._standardCANBaudrates.Add(1000000u);
			}
			return GUIUtil._standardCANBaudrates;
		}

		public static IList<uint> GetStandardCANFDArbBaudrates()
		{
			if (GUIUtil._standardCANFDArbBaudrates == null)
			{
				GUIUtil._standardCANFDArbBaudrates = new List<uint>();
				GUIUtil._standardCANFDArbBaudrates.Add(500000u);
			}
			return GUIUtil._standardCANFDArbBaudrates;
		}

		public static IList<uint> GetStandardCANFDDataBaudrates()
		{
			if (GUIUtil._standardCANFDDataBaudrates == null)
			{
				GUIUtil._standardCANFDDataBaudrates = new List<uint>();
				GUIUtil._standardCANFDDataBaudrates.Add(1000000u);
				GUIUtil._standardCANFDDataBaudrates.Add(2000000u);
				GUIUtil._standardCANFDDataBaudrates.Add(4000000u);
				GUIUtil._standardCANFDDataBaudrates.Add(5000000u);
			}
			return GUIUtil._standardCANFDDataBaudrates;
		}

		public static IList<uint> GetStandardLINBaudrates()
		{
			if (GUIUtil._standardLINBaudrates == null)
			{
				GUIUtil._standardLINBaudrates = new List<uint>();
				GUIUtil._standardLINBaudrates.Add(1000u);
				GUIUtil._standardLINBaudrates.Add(1200u);
				GUIUtil._standardLINBaudrates.Add(2400u);
				GUIUtil._standardLINBaudrates.Add(4800u);
				GUIUtil._standardLINBaudrates.Add(9600u);
				GUIUtil._standardLINBaudrates.Add(10417u);
				GUIUtil._standardLINBaudrates.Add(19200u);
				GUIUtil._standardLINBaudrates.Add(20000u);
			}
			return GUIUtil._standardLINBaudrates;
		}

		public static IList<uint> GetStandardJ1708Baudrates()
		{
			if (GUIUtil._standardJ1708Baudrates == null)
			{
				GUIUtil._standardJ1708Baudrates = new List<uint>();
				GUIUtil._standardJ1708Baudrates.Add(9600u);
			}
			return GUIUtil._standardJ1708Baudrates;
		}

		public static string MapBaudrate2String(uint baudRate)
		{
			return string.Format(Resources.SpeedRateString, baudRate.ToString("N0", ProgramUtils.Culture));
		}

		public static uint MapString2Baudrate(string baudrateString)
		{
			IList<uint> list = GUIUtil.GetStandardCANBaudrates();
			foreach (uint current in list)
			{
				if (baudrateString == GUIUtil.MapBaudrate2String(current))
				{
					uint result = current;
					return result;
				}
			}
			list = GUIUtil.GetStandardCANFDArbBaudrates();
			foreach (uint current2 in list)
			{
				if (baudrateString == GUIUtil.MapBaudrate2String(current2))
				{
					uint result = current2;
					return result;
				}
			}
			list = GUIUtil.GetStandardCANFDDataBaudrates();
			foreach (uint current3 in list)
			{
				if (baudrateString == GUIUtil.MapBaudrate2String(current3))
				{
					uint result = current3;
					return result;
				}
			}
			list = GUIUtil.GetStandardLINBaudrates();
			foreach (uint current4 in list)
			{
				if (baudrateString == GUIUtil.MapBaudrate2String(current4))
				{
					uint result = current4;
					return result;
				}
			}
			return 0u;
		}

		public static int GetIndexOfUserdefBaudrateValueEntry(ComboBox box)
		{
			for (int i = 0; i < box.Items.Count; i++)
			{
				string text = box.Items[i].ToString();
				if (!string.IsNullOrEmpty(text) && text.Length > GUIUtil.UserdefBaudrateValueEntryPostfix.Length && text.Substring(text.Length - GUIUtil.UserdefBaudrateValueEntryPostfix.Length) == GUIUtil.UserdefBaudrateValueEntryPostfix)
				{
					return i;
				}
			}
			return -1;
		}

		public static int GetInsertIndexOfUserdefBaudrateValueEntry(ComboBox box, uint userdefBaudrate)
		{
			int indexOfUserdefBaudrateValueEntry = GUIUtil.GetIndexOfUserdefBaudrateValueEntry(box);
			for (int i = 0; i < box.Items.Count; i++)
			{
				if (indexOfUserdefBaudrateValueEntry != i && !(box.Items[i].ToString() == GUIUtil.EditUserdefDropdownEntry) && GUIUtil.MapString2Baudrate(box.Items[i].ToString()) > userdefBaudrate)
				{
					return i;
				}
			}
			return box.Items.Count - 1;
		}

		public static string MapAnalogInputNumber2String(uint inputNr)
		{
			return string.Format(Vocabulary.AnalogInputName, inputNr);
		}

		public static uint MapAnalogInputString2Number(string inputString)
		{
			for (uint num = 1u; num < 20u; num += 1u)
			{
				if (GUIUtil.MapAnalogInputNumber2String(num) == inputString)
				{
					return num;
				}
			}
			return 0u;
		}

		public static IList<uint> GetStandardAnalogInputFrequencies()
		{
			if (GUIUtil._standardAnalogInputFrequencies == null)
			{
				GUIUtil._standardAnalogInputFrequencies = new List<uint>();
				GUIUtil._standardAnalogInputFrequencies.Add(1u);
				GUIUtil._standardAnalogInputFrequencies.Add(2u);
				GUIUtil._standardAnalogInputFrequencies.Add(5u);
				GUIUtil._standardAnalogInputFrequencies.Add(10u);
				GUIUtil._standardAnalogInputFrequencies.Add(20u);
				GUIUtil._standardAnalogInputFrequencies.Add(50u);
				GUIUtil._standardAnalogInputFrequencies.Add(100u);
				GUIUtil._standardAnalogInputFrequencies.Add(200u);
				GUIUtil._standardAnalogInputFrequencies.Add(500u);
				GUIUtil._standardAnalogInputFrequencies.Add(1000u);
			}
			return GUIUtil._standardAnalogInputFrequencies;
		}

		public static string MapAnalogInputsFrequency2String(uint frequency)
		{
			return string.Format(Resources.FrequencyInHz, frequency.ToString("N0", ProgramUtils.Culture));
		}

		public static uint MapString2AnalogInputsFrequency(string frequencyString)
		{
			IList<uint> standardAnalogInputFrequencies = GUIUtil.GetStandardAnalogInputFrequencies();
			foreach (uint current in standardAnalogInputFrequencies)
			{
				if (frequencyString == GUIUtil.MapAnalogInputsFrequency2String(current))
				{
					return current;
				}
			}
			return 0u;
		}

		public static uint CalculateMappedCANId(uint baseCanId, bool isExtendedId, uint inputNr, bool isFixedIdsMode)
		{
			uint num;
			if (isFixedIdsMode)
			{
				if (inputNr <= 6u)
				{
					num = baseCanId + (inputNr - 1u) / Constants.NumberOfAnalogInputsOnOneMessage;
				}
				else
				{
					num = baseCanId + 2u + (inputNr - 7u) / Constants.NumberOfAnalogInputsOnOneMessage;
				}
			}
			else
			{
				num = baseCanId + (inputNr - 1u);
			}
			if (isExtendedId)
			{
				if (num > Constants.MaximumExtendedCANId)
				{
					num = Constants.MaximumExtendedCANId;
				}
			}
			else if (num > Constants.MaximumStandardCANId)
			{
				num = Constants.MaximumStandardCANId;
			}
			return num;
		}

		public static uint GetMasterInputNrForFrequencyInFixedMode(uint inputNr)
		{
			if (inputNr <= 6u)
			{
				return (inputNr - 1u) / Constants.NumberOfAnalogInputsOnOneMessage * Constants.NumberOfAnalogInputsOnOneMessage + 1u;
			}
			return (inputNr - 7u) / Constants.NumberOfAnalogInputsOnOneMessage * Constants.NumberOfAnalogInputsOnOneMessage + 7u;
		}

		public static string MapCANChannelNumber2String(uint channelNr)
		{
			if (channelNr == 4294967295u)
			{
				return Resources.AnyCAN;
			}
			return string.Format(Vocabulary.CANChannelName, channelNr);
		}

		public static uint MapCANChannelString2Number(string channelString)
		{
			if (channelString == Resources.AnyCAN)
			{
				return 4294967295u;
			}
			if (channelString.IndexOf(Vocabulary.CAN) == 0 && channelString.Length > Vocabulary.CAN.Length)
			{
				string text = channelString.Substring(Vocabulary.CAN.Length);
				int num = text.IndexOf(Resources.VirtualChannelPostfix);
				if (num >= 0)
				{
					text = text.Substring(0, num);
				}
				uint result;
				if (uint.TryParse(text, out result))
				{
					return result;
				}
			}
			return 0u;
		}

		public static string MapLINChannelNumber2String(uint channelNr, ILoggerSpecifics loggerSpecifics)
		{
			if (channelNr == 4294967295u)
			{
				return Resources.AnyLIN;
			}
			if (channelNr <= loggerSpecifics.LIN.NumberOfChannels)
			{
				return string.Format(Resources.LINChannelName, channelNr);
			}
			if (channelNr <= loggerSpecifics.Multibus.NumberOfChannels)
			{
				return string.Format(Resources.LINChannelName, channelNr);
			}
			return string.Format(Resources.LINChannelName, channelNr) + Resources.LINprobeChannelPostfix;
		}

		public static string MapLINChannelNumber2String(uint channelNr, bool isLINprobeChannel)
		{
			if (channelNr == 4294967295u)
			{
				return Resources.AnyLIN;
			}
			if (!isLINprobeChannel)
			{
				return string.Format(Resources.LINChannelName, channelNr);
			}
			return string.Format(Resources.LINChannelName, channelNr) + Resources.LINprobeChannelPostfix;
		}

		public static uint MapLINChannelString2Number(string channelString)
		{
			if (channelString == Resources.AnyLIN)
			{
				return 4294967295u;
			}
			if (channelString.IndexOf(Vocabulary.LIN) == 0 && channelString.Length > Vocabulary.LIN.Length)
			{
				int num = channelString.IndexOf(Resources.LINprobeChannelPostfix);
				if (num > 0)
				{
					channelString = channelString.Substring(0, num);
				}
				uint result;
				if (uint.TryParse(channelString.Substring(Vocabulary.LIN.Length), out result))
				{
					return result;
				}
			}
			return 0u;
		}

		public static string MapFlexrayChannelNumber2String(uint channelNr)
		{
			if (channelNr == Database.ChannelNumber_FlexrayAB)
			{
				return Vocabulary.FlexrayChannelAB;
			}
			return string.Format(Vocabulary.FlexrayChannelName, channelNr);
		}

		public static string MapFlexrayABChannel2String(uint channelNr)
		{
			if (channelNr == Database.ChannelNumber_FlexrayAB)
			{
				return Vocabulary.FlexrayChannelAB;
			}
			if (channelNr == 0u)
			{
				return string.Format(Vocabulary.FlexrayChannelName, channelNr);
			}
			string text = string.Empty;
			text += Math.Ceiling(channelNr / 2.0);
			if (channelNr % 2u == 0u)
			{
				text += "B";
			}
			else
			{
				text += "A";
			}
			return string.Format(Vocabulary.FlexrayChannelName, text);
		}

		public static uint MapFlexrayChannelString2Number(string channelString)
		{
			if (channelString == Vocabulary.FlexrayChannelAB)
			{
				return Database.ChannelNumber_FlexrayAB;
			}
			uint result;
			if (channelString.IndexOf(Vocabulary.Flexray) == 0 && channelString.Length > Vocabulary.Flexray.Length && uint.TryParse(channelString.Substring(Vocabulary.Flexray.Length), out result))
			{
				return result;
			}
			return 0u;
		}

		public static uint MapFlexrayABChannelString2ABChannel(string channelString)
		{
			uint num = 0u;
			if (channelString == Vocabulary.FlexrayChannelAB)
			{
				num = Database.ChannelNumber_FlexrayAB;
			}
			else if (channelString.IndexOf(Vocabulary.Flexray) == 0 && channelString.Length > Vocabulary.Flexray.Length)
			{
				uint num2;
				if (uint.TryParse(channelString.Substring(Vocabulary.Flexray.Length, channelString.Length - Vocabulary.Flexray.Length - 1), out num2))
				{
					num = num2 * 2u - 1u;
				}
				if (string.Compare(channelString.Substring(channelString.Length - 1, 1), "B", true) == 0)
				{
					num += 1u;
				}
			}
			if (num == 0u)
			{
				return 0u;
			}
			return num;
		}

		public static string MapEthernetChannelNumber2String(uint channelNr)
		{
			return string.Format(Resources.EthernetChannelName, channelNr);
		}

		public static string MapBusType2String(BusType busType)
		{
			switch (busType)
			{
			case BusType.Bt_CAN:
				return Vocabulary.CAN;
			case BusType.Bt_LIN:
				return Vocabulary.LIN;
			case BusType.Bt_FlexRay:
				return Vocabulary.Flexray;
			case BusType.Bt_J1708:
				return Vocabulary.J1708;
			case BusType.Bt_Ethernet:
				return Resources.Ethernet;
			default:
				return Resources.Unknown;
			}
		}

		public static string MapXcpProtocol2String(CcpXcpProtocol xcpProtocol)
		{
			switch (xcpProtocol)
			{
			case CcpXcpProtocol.CcpXcpPr_CAN:
				return Vocabulary.XcpOnCan;
			case CcpXcpProtocol.CcpXcpPr_FlexRay:
				return Vocabulary.XcpOnFlexRay;
			case CcpXcpProtocol.CcpXcpPr_UDP:
				return Vocabulary.XcpOnEthernetUdp;
			case CcpXcpProtocol.CcpXcpPr_TCP:
				return Vocabulary.XcpOnEthernetTcp;
			case CcpXcpProtocol.CcpXcpPr_CCP:
				return Vocabulary.CPTypeShortNameCCP;
			default:
				return Resources.Unknown;
			}
		}

		public static CcpXcpProtocol MapString2XcpProtocol(string xcpProtocolName)
		{
			foreach (CcpXcpProtocol ccpXcpProtocol in Enum.GetValues(typeof(CcpXcpProtocol)))
			{
				string a = GUIUtil.MapXcpProtocol2String(ccpXcpProtocol);
				if (a == xcpProtocolName)
				{
					return ccpXcpProtocol;
				}
			}
			return CcpXcpProtocol.CcpXcpPr_None;
		}

		public static string MapDigitalInputNumber2String(uint inputNr)
		{
			return string.Format(Resources.DigitalInputName, inputNr);
		}

		public static uint MapDigitalInputString2Number(string inputString)
		{
			uint result;
			if (inputString.IndexOf(Resources.DigitalInputPrefix) == 0 && inputString.Length > Resources.DigitalInputPrefix.Length && uint.TryParse(inputString.Substring(Resources.DigitalInputPrefix.Length), out result))
			{
				return result;
			}
			return 0u;
		}

		public static bool TryMapDigitalInputString2Number(string inputString, out uint number)
		{
			number = 0u;
			return inputString.IndexOf(Resources.DigitalInputPrefix) == 0 && inputString.Length > Resources.DigitalInputPrefix.Length && uint.TryParse(inputString.Substring(Resources.DigitalInputPrefix.Length), out number);
		}

		public static uint CalculateDigitalInputMappedCANId(uint baseCanId, bool isExtendedId, uint inputNr, bool isFixedIdsMode)
		{
			uint num;
			if (isFixedIdsMode)
			{
				num = baseCanId;
			}
			else
			{
				num = baseCanId + (inputNr - 1u);
			}
			if (isExtendedId)
			{
				if (num > Constants.MaximumExtendedCANId)
				{
					num = Constants.MaximumExtendedCANId;
				}
			}
			else if (num > Constants.MaximumStandardCANId)
			{
				num = Constants.MaximumStandardCANId;
			}
			return num;
		}

		public static string GetShortCPTypeName(CPType type)
		{
			switch (type)
			{
			case CPType.CCP:
				return Vocabulary.CPTypeShortNameCCP;
			case CPType.CCP101:
				return Vocabulary.CPTypeShortNameCCP101;
			case CPType.XCP:
				return Vocabulary.CPTypeShortNameXCP;
			default:
				return Resources_CcpXcp.CPTypeShortNameNone;
			}
		}

		public static string MapDiagnosticsProtocol2String(DiagnosticsProtocolType type)
		{
			switch (type)
			{
			case DiagnosticsProtocolType.KWP:
				return Resources.DiagProtocolTypeKWP2000;
			case DiagnosticsProtocolType.UDS:
				return Resources.DiagProtocolTypeUDS;
			default:
				return Resources.Unknown;
			}
		}

		public static DiagnosticsProtocolType MapString2DiagnosticsProtocol(string valueStr)
		{
			foreach (DiagnosticsProtocolType diagnosticsProtocolType in Enum.GetValues(typeof(DiagnosticsProtocolType)))
			{
				if (valueStr == GUIUtil.MapDiagnosticsProtocol2String(diagnosticsProtocolType))
				{
					return diagnosticsProtocolType;
				}
			}
			return DiagnosticsProtocolType.Undefined;
		}

		public static string MapDiagnosticsAddressingMode2String(DiagnosticsAddressingMode mode)
		{
			switch (mode)
			{
			case DiagnosticsAddressingMode.Normal:
				return Resources.DiagnosticsAddrModeNormal;
			case DiagnosticsAddressingMode.NormalFixed:
				return Resources.DiagnosticsAddrModeNormalFixed;
			case DiagnosticsAddressingMode.Extended:
				return Resources.DiagnosticsAddrModeExtended;
			default:
				return Resources.Unknown;
			}
		}

		public static DiagnosticsAddressingMode MapString2DiagnosticsAddressingMode(string name)
		{
			foreach (DiagnosticsAddressingMode diagnosticsAddressingMode in Enum.GetValues(typeof(DiagnosticsAddressingMode)))
			{
				if (name == GUIUtil.MapDiagnosticsAddressingMode2String(diagnosticsAddressingMode))
				{
					return diagnosticsAddressingMode;
				}
			}
			return DiagnosticsAddressingMode.Undefined;
		}

		public static string MapDiagSessionType2String(DiagSessionType session)
		{
			switch (session)
			{
			case DiagSessionType.Unknown:
				return Resources.DiagSessionTypeNone;
			case DiagSessionType.Default:
				return Resources.DiagSessionTypeDefault;
			case DiagSessionType.Extended:
				return Resources.DiagSessionTypeExtended;
			default:
				return Resources.Unknown;
			}
		}

		public static string MapDiagSessionSourceToString(DiagnosticsSessionSource source)
		{
			switch (source)
			{
			case DiagnosticsSessionSource.UserDefined:
				return Resources.DiagSession_UserDefined;
			case DiagnosticsSessionSource.UDS_Default:
				return Resources.DiagSession_UDSDefault;
			case DiagnosticsSessionSource.UDS_Extended:
				return Resources.DiagSession_UDSExtended;
			case DiagnosticsSessionSource.KWP_Default:
				return Resources.DiagSession_KWPDefault;
			}
			return "";
		}

		public static DiagnosticsSessionSource MapStringToDiagSessionSource(string sessionSourceName)
		{
			foreach (DiagnosticsSessionSource diagnosticsSessionSource in Enum.GetValues(typeof(DiagnosticsSessionSource)))
			{
				if (sessionSourceName == GUIUtil.MapDiagSessionSourceToString(diagnosticsSessionSource))
				{
					return diagnosticsSessionSource;
				}
			}
			return DiagnosticsSessionSource.DatabaseDefined;
		}

		public static FileType GetFileTypeFor(DiagDbType diagDbType)
		{
			switch (diagDbType)
			{
			case DiagDbType.ODX:
				return FileType.ODXDiagDesc;
			case DiagDbType.PDX:
				return FileType.PDXDiagDesc;
			case DiagDbType.CDD:
				return FileType.CDDDiagDesc;
			case DiagDbType.MDX:
				return FileType.MDXDiagDesc;
			}
			return FileType.AnyDiagDesc;
		}

		public static double RawSignalValueToPhysicalValue(double rawValue, SignalDefinition sigDef)
		{
			if (sigDef != null)
			{
				return rawValue * sigDef.Factor + sigDef.Offset;
			}
			return 0.0;
		}

		public static string MapFilterConditionToString(SymbolicMessageFilter filter, IDatabaseServices databaseServices)
		{
			MessageDefinition messageDefinition;
			if (BusType.Bt_FlexRay != filter.BusType.Value)
			{
				string arg;
				if (databaseServices.IsSymbolicMessageDefined(filter.DatabasePath.Value, filter.NetworkName.Value, filter.MessageName.Value, filter.ChannelNumber.Value, filter.BusType.Value, out messageDefinition))
				{
					if (BusType.Bt_CAN == filter.BusType.Value)
					{
						arg = GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(messageDefinition.ActualMessageId, messageDefinition.IsExtendedId);
					}
					else if (BusType.Bt_LIN == filter.BusType.Value)
					{
						arg = GUIUtil.GetNumberPrefix() + GUIUtil.LINIdToDisplayString(messageDefinition.CanDbMessageId);
					}
					else
					{
						arg = GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(messageDefinition.CanDbMessageId);
					}
				}
				else
				{
					arg = Resources.Unknown;
				}
				string arg2 = filter.MessageName.Value;
				if (!string.IsNullOrEmpty(filter.NetworkName.Value))
				{
					arg2 = filter.NetworkName.Value + "::" + filter.MessageName.Value;
				}
				return string.Format(Resources.FilterConditionSymbolic, arg2, arg, filter.DatabaseName);
			}
			string text = Resources.Unknown;
			string text2 = "";
			string text3 = Resources.Unknown;
			string text4 = Resources.Unknown;
			string text5 = Resources.Unknown;
			if (databaseServices.IsSymbolicMessageDefined(filter.DatabasePath.Value, filter.NetworkName.Value, filter.MessageName.Value, filter.ChannelNumber.Value, filter.BusType.Value, out messageDefinition))
			{
				if (!filter.IsFlexrayPDU.Value)
				{
					text = Resources.SymFlexrayMsgTypFrame;
					text3 = messageDefinition.CanDbMessageId.ToString();
					text4 = messageDefinition.FrBaseCycle.ToString();
					text5 = messageDefinition.FrCycleRepetition.ToString();
					text2 = ";";
				}
				IList<MessageDefinition> list = null;
				IList<string> list2 = null;
				bool isFibexVersionGreaterThan = false;
				if (databaseServices.GetFlexrayFrameOrPDUInfo(filter.DatabasePath.Value, filter.NetworkName.Value, filter.MessageName.Value, filter.IsFlexrayPDU.Value, out list, out list2, out isFibexVersionGreaterThan))
				{
					filter.IsFibexVersionGreaterThan2 = isFibexVersionGreaterThan;
					if (filter.IsFlexrayPDU.Value)
					{
						text = Resources.SymFlexrayMsgTypPDU;
						if (list.Count > 0)
						{
							text3 = list[0].ActualMessageId.ToString();
							text4 = list[0].FrBaseCycle.ToString();
							text5 = list[0].FrCycleRepetition.ToString();
						}
						if (list.Count > 1)
						{
							text2 = "],...";
						}
						else
						{
							text2 = "] ";
						}
					}
					else if (list.Count > 1)
					{
						text = Resources.SymFlexrayMsgTypMultiFrame;
					}
				}
			}
			string text6 = filter.MessageName.Value;
			if (!string.IsNullOrEmpty(filter.NetworkName.Value))
			{
				text6 = filter.NetworkName.Value + "::" + filter.MessageName.Value;
			}
			return string.Format(Resources.SymFlexrayMsgCondition, new object[]
			{
				text6,
				text,
				text3,
				text4,
				text5,
				text2,
				filter.DatabaseName.Value
			});
		}

		public static string MapFilterConditionToString(CANIdFilter filter)
		{
			if (filter.IsIdRange.Value)
			{
				return string.Format(Resources.FilterConditionIdRange, GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(filter.CANId.Value, filter.IsExtendedId.Value), GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(filter.CANIdLast.Value, filter.IsExtendedId.Value));
			}
			return string.Format(Resources.FilterConditionIdValue, GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(filter.CANId.Value, filter.IsExtendedId.Value));
		}

		public static string MapFilterConditionToString(LINIdFilter filter)
		{
			if (filter.IsIdRange.Value)
			{
				return string.Format(Resources.FilterConditionIdRange, GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(filter.LINId.Value), GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(filter.LINIdLast.Value));
			}
			return string.Format(Resources.FilterConditionIdValue, GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(filter.LINId.Value));
		}

		public static string MapFilterConditionToString(FlexrayIdFilter filter)
		{
			string str;
			if (filter.IsIdRange.Value)
			{
				str = string.Format(Resources.FilterConditionSlotIdRange, filter.FlexrayId.Value, filter.FlexrayIdLast.Value);
			}
			else
			{
				str = string.Format(Resources.FilterConditionSlotIdValue, filter.FlexrayId.Value);
			}
			return str + string.Format(Resources.ConditionFlexrayCycles, filter.BaseCycle.Value, filter.CycleRepetition.Value);
		}

		public static string MapFilterConditionToString(SignalListFileFilter filter)
		{
			return string.Format(Resources.FilterConditionSignalListFile, filter.FilePath.Value, filter.Column.Value + 1u);
		}

		public static void InitSplitButtonMenuFilterTypes(SplitButtonEx splitButtonEx)
		{
			foreach (string current in GUIUtil.FilterTypeNamesInMenu)
			{
				string groupName = string.Empty;
				if (current == Resources.FilterTypeNameCANChn || current == Resources.FilterTypeNameSymbolicCAN || current == Resources.FilterTypeNameCANId)
				{
					groupName = "CAN";
				}
				else if (current == Resources.FilterTypeNameLINChn || current == Resources.FilterTypeNameSymbolicLIN || current == Resources.FilterTypeNameLINId)
				{
					groupName = "LIN";
				}
				else if (current == Resources.FilterTypeNameFlexRayChn || current == Resources.FilterTypeNameSymbolicFlexray || current == Resources.FilterTypeNameFlexrayId)
				{
					groupName = "FlexRay";
				}
				splitButtonEx.AddItem(current, GUIUtil.GetFilterTypeImage(current), groupName);
			}
		}

		private static Image GetFilterTypeImage(string eventName)
		{
			MainImageList.IconIndex baseImgIndex = MainImageList.IconIndex.NoImage;
			if (eventName == Resources.FilterTypeNameCANChn)
			{
				baseImgIndex = MainImageList.IconIndex.ChannelFilterCAN;
			}
			else if (eventName == Resources.FilterTypeNameFlexRayChn)
			{
				baseImgIndex = MainImageList.IconIndex.ChannelFilterFlexray;
			}
			else if (eventName == Resources.FilterTypeNameLINChn)
			{
				baseImgIndex = MainImageList.IconIndex.ChannelFilterLIN;
			}
			else if (eventName == Resources.FilterTypeNameSymbolicCAN)
			{
				baseImgIndex = MainImageList.IconIndex.SymbolicMessageCAN;
			}
			else if (eventName == Resources.FilterTypeNameSymbolicFlexray)
			{
				baseImgIndex = MainImageList.IconIndex.SymbolicMessageFlexray;
			}
			else if (eventName == Resources.FilterTypeNameSymbolicLIN)
			{
				baseImgIndex = MainImageList.IconIndex.SymbolicMessageLIN;
			}
			else if (eventName == Resources.FilterTypeNameCANId)
			{
				baseImgIndex = MainImageList.IconIndex.RawMessageCAN;
			}
			else if (eventName == Resources.FilterTypeNameFlexrayId)
			{
				baseImgIndex = MainImageList.IconIndex.RawMessageFlexray;
			}
			else if (eventName == Resources.FilterTypeNameLINId)
			{
				baseImgIndex = MainImageList.IconIndex.RawMessageLIN;
			}
			else if (eventName == Resources.FilterTypeNameCANSignalList)
			{
				baseImgIndex = MainImageList.IconIndex.SymbolicSignalCANList;
			}
			return MainImageList.Instance.GetImage(baseImgIndex);
		}

		public static MainImageList.IconIndex GetEventTypeIconIndex(Event ev)
		{
			if (ev is CANIdEvent)
			{
				return MainImageList.IconIndex.RawMessageCAN;
			}
			if (ev is FlexrayIdEvent)
			{
				return MainImageList.IconIndex.RawMessageFlexray;
			}
			if (ev is LINIdEvent)
			{
				return MainImageList.IconIndex.RawMessageLIN;
			}
			if (ev is SymbolicMessageEvent)
			{
				switch ((ev as SymbolicMessageEvent).BusType.Value)
				{
				case BusType.Bt_CAN:
					return MainImageList.IconIndex.SymbolicMessageCAN;
				case BusType.Bt_LIN:
					return MainImageList.IconIndex.SymbolicMessageLIN;
				case BusType.Bt_FlexRay:
					return MainImageList.IconIndex.SymbolicMessageFlexray;
				default:
					return MainImageList.IconIndex.SymbolicMessageCAN;
				}
			}
			else if (ev is SymbolicSignalEvent)
			{
				switch ((ev as SymbolicSignalEvent).BusType.Value)
				{
				case BusType.Bt_CAN:
					return MainImageList.IconIndex.SymbolicSignalCAN;
				case BusType.Bt_LIN:
					return MainImageList.IconIndex.SymbolicSignalLIN;
				case BusType.Bt_FlexRay:
					return MainImageList.IconIndex.SymbolicSignalFlexray;
				default:
					return MainImageList.IconIndex.SymbolicMessageCAN;
				}
			}
			else
			{
				if (ev is KeyEvent)
				{
					return MainImageList.IconIndex.Key;
				}
				if (ev is DigitalInputEvent)
				{
					return MainImageList.IconIndex.DigitalInput;
				}
				if (ev is AnalogInputEvent)
				{
					return MainImageList.IconIndex.AnalogInput;
				}
				if (ev is CANBusStatisticsEvent)
				{
					return MainImageList.IconIndex.CANBusStatistics;
				}
				if (ev is CANDataEvent)
				{
					return MainImageList.IconIndex.DataTriggerCAN;
				}
				if (ev is LINDataEvent)
				{
					return MainImageList.IconIndex.DataTriggerLIN;
				}
				if (ev is IgnitionEvent)
				{
					return MainImageList.IconIndex.OnIgnition;
				}
				if (ev is VoCanRecordingEvent)
				{
					return MainImageList.IconIndex.VoCanRecoding;
				}
				if (ev is MsgTimeoutEvent)
				{
					switch ((ev as MsgTimeoutEvent).BusType.Value)
					{
					case BusType.Bt_CAN:
						return MainImageList.IconIndex.MessageTimeoutCAN;
					case BusType.Bt_LIN:
						return MainImageList.IconIndex.MessageTimeoutLIN;
					case BusType.Bt_FlexRay:
						return MainImageList.IconIndex.MessageTimeoutFlexray;
					default:
						return MainImageList.IconIndex.SymbolicMessageCAN;
					}
				}
				else
				{
					if (ev is CombinedEvent)
					{
						return MainImageList.IconIndex.ConditionGroup;
					}
					if (ev is CcpXcpSignalEvent)
					{
						return MainImageList.IconIndex.CcpXcpSignal;
					}
					if (ev is DiagnosticSignalEvent)
					{
						return MainImageList.IconIndex.DiagnosticSignal;
					}
					if (ev is IncEvent)
					{
						return MainImageList.IconIndex.IncludeFile;
					}
					return MainImageList.IconIndex.NoImage;
				}
			}
		}

		public static string GetUnboundColumnDataConditionString(RecordTrigger trigger, IDatabaseServices dbService)
		{
			if (trigger.Event is IdEvent)
			{
				return GUIUtil.MapEventCondition2String(trigger.Event as IdEvent);
			}
			if (trigger.Event is SymbolicMessageEvent)
			{
				return GUIUtil.MapEventCondition2String(trigger.Event as SymbolicMessageEvent, dbService);
			}
			if (trigger.Event is SymbolicSignalEvent)
			{
				return GUIUtil.MapEventCondition2String(trigger.Event as SymbolicSignalEvent, dbService);
			}
			if (trigger.Event is DigitalInputEvent)
			{
				return GUIUtil.MapEventCondition2String(trigger.Event as DigitalInputEvent);
			}
			if (trigger.Event is AnalogInputEvent)
			{
				return GUIUtil.MapEventCondition2String(trigger.Event as AnalogInputEvent);
			}
			if (trigger.Event is CANDataEvent)
			{
				return GUIUtil.MapEventCondition2String(trigger.Event as CANDataEvent);
			}
			if (trigger.Event is LINDataEvent)
			{
				return GUIUtil.MapEventCondition2String(trigger.Event as LINDataEvent);
			}
			if (trigger.Event is MsgTimeoutEvent)
			{
				return GUIUtil.MapEventCondition2String(trigger.Event as MsgTimeoutEvent, dbService);
			}
			if (trigger.Event is KeyEvent)
			{
				if ((trigger.Event as KeyEvent).IsOnPanel.Value)
				{
					return string.Format(Resources_Trigger.TriggerKeyOnPanel, string.Empty);
				}
				if ((trigger.Event as KeyEvent).IsCasKey)
				{
					return string.Format(Resources_Trigger.TriggerKeyOnCas, string.Empty);
				}
				return string.Format(Resources_Trigger.TriggerKeyOnRemoteControl, string.Empty);
			}
			else
			{
				if (trigger.Event is CANBusStatisticsEvent)
				{
					return GUIUtil.MapEventCondition2String(trigger.Event as CANBusStatisticsEvent);
				}
				if (trigger.Event is IgnitionEvent)
				{
					return GUIUtil.MapEventCondition2String(trigger.Event as IgnitionEvent);
				}
				if (trigger.Event is VoCanRecordingEvent)
				{
					return GUIUtil.MapEventCondition2String(trigger.Event as VoCanRecordingEvent);
				}
				return string.Empty;
			}
		}

		public static string MapEventCondition2String(IdEvent idEvent)
		{
			string arg = GUIUtil.MapTriggerConditionRelation2String(idEvent.IdRelation.Value);
			string arg2 = string.Empty;
			string text = string.Empty;
			string arg3 = string.Empty;
			string format = string.Empty;
			string str = string.Empty;
			if (idEvent is CANIdEvent)
			{
				text = GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(idEvent.LowId.Value, (idEvent as CANIdEvent).IsExtendedId.Value);
				arg3 = GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(idEvent.HighId.Value, (idEvent as CANIdEvent).IsExtendedId.Value);
				format = Vocabulary.CANIdCondition;
			}
			else if (idEvent is LINIdEvent)
			{
				text = GUIUtil.GetNumberPrefix() + GUIUtil.LINIdToDisplayString(idEvent.LowId.Value);
				arg3 = GUIUtil.GetNumberPrefix() + GUIUtil.LINIdToDisplayString(idEvent.HighId.Value);
				format = Resources.LINIdCondition;
			}
			else
			{
				if (!(idEvent is FlexrayIdEvent))
				{
					return "";
				}
				FlexrayIdEvent flexrayIdEvent = idEvent as FlexrayIdEvent;
				format = Resources.SlotIdCondition;
				text = GUIUtil.FlexraySlotIdToDisplayString(idEvent.LowId.Value);
				arg3 = GUIUtil.FlexraySlotIdToDisplayString(idEvent.HighId.Value);
				str = string.Format(Resources.ConditionFlexrayCycles, flexrayIdEvent.BaseCycle.Value, flexrayIdEvent.CycleRepetition.Value);
			}
			switch (idEvent.IdRelation.Value)
			{
			case CondRelation.Equal:
			case CondRelation.NotEqual:
			case CondRelation.LessThan:
			case CondRelation.LessThanOrEqual:
			case CondRelation.GreaterThan:
			case CondRelation.GreaterThanOrEqual:
				arg2 = text;
				break;
			case CondRelation.InRange:
			case CondRelation.NotInRange:
				arg2 = string.Format(Resources.Range, text, arg3);
				break;
			}
			return string.Format(format, arg, arg2) + str;
		}

		public static string MapEventCondition2String(CANDataEvent canDataEvent)
		{
			uint num = 0u;
			bool flag = false;
			uint num2 = 0u;
			uint num3 = 0u;
			string text = "";
			if (canDataEvent.RawDataSignal is RawDataSignalByte)
			{
				num = (canDataEvent.RawDataSignal as RawDataSignalByte).DataBytePos.Value;
			}
			else if (canDataEvent.RawDataSignal is RawDataSignalStartbitLength)
			{
				flag = true;
				num2 = (canDataEvent.RawDataSignal as RawDataSignalStartbitLength).StartbitPos.Value;
				num3 = (canDataEvent.RawDataSignal as RawDataSignalStartbitLength).Length.Value;
				text = Resources.Intel;
				if ((canDataEvent.RawDataSignal as RawDataSignalStartbitLength).IsMotorola.Value)
				{
					text = Resources.Motorola;
				}
			}
			string text2 = GUIUtil.MapTriggerConditionRelation2String(canDataEvent.Relation.Value);
			string text3 = string.Empty;
			switch (canDataEvent.Relation.Value)
			{
			case CondRelation.Equal:
			case CondRelation.NotEqual:
			case CondRelation.LessThan:
			case CondRelation.LessThanOrEqual:
			case CondRelation.GreaterThan:
			case CondRelation.GreaterThanOrEqual:
				text3 = GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(canDataEvent.LowValue.Value);
				break;
			case CondRelation.InRange:
				text3 = string.Format(Resources.Range, GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(canDataEvent.LowValue.Value), GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(canDataEvent.HighValue.Value));
				break;
			case CondRelation.NotInRange:
				text3 = string.Format(Resources.Range, GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(canDataEvent.LowValue.Value), GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(canDataEvent.HighValue.Value));
				break;
			}
			if (flag)
			{
				return string.Format(Resources.DataConditionStartBit, new object[]
				{
					GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(canDataEvent.ID.Value, canDataEvent.IsExtendedId.Value),
					num2,
					num3,
					text,
					text2,
					text3
				});
			}
			return string.Format(Resources.DataConditionByte, new object[]
			{
				GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(canDataEvent.ID.Value, canDataEvent.IsExtendedId.Value),
				num,
				text2,
				text3
			});
		}

		public static string MapEventCondition2String(LINDataEvent linDataEvent)
		{
			uint num = 0u;
			bool flag = false;
			uint num2 = 0u;
			uint num3 = 0u;
			string text = "";
			if (linDataEvent.RawDataSignal is RawDataSignalByte)
			{
				num = (linDataEvent.RawDataSignal as RawDataSignalByte).DataBytePos.Value;
			}
			else if (linDataEvent.RawDataSignal is RawDataSignalStartbitLength)
			{
				flag = true;
				num2 = (linDataEvent.RawDataSignal as RawDataSignalStartbitLength).StartbitPos.Value;
				num3 = (linDataEvent.RawDataSignal as RawDataSignalStartbitLength).Length.Value;
				text = Resources.Intel;
				if ((linDataEvent.RawDataSignal as RawDataSignalStartbitLength).IsMotorola.Value)
				{
					text = Resources.Motorola;
				}
			}
			string text2 = GUIUtil.MapTriggerConditionRelation2String(linDataEvent.Relation.Value);
			string text3 = string.Empty;
			switch (linDataEvent.Relation.Value)
			{
			case CondRelation.Equal:
			case CondRelation.NotEqual:
			case CondRelation.LessThan:
			case CondRelation.LessThanOrEqual:
			case CondRelation.GreaterThan:
			case CondRelation.GreaterThanOrEqual:
				text3 = GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(linDataEvent.LowValue.Value);
				break;
			case CondRelation.InRange:
				text3 = string.Format(Resources.Range, GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(linDataEvent.LowValue.Value), GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(linDataEvent.HighValue.Value));
				break;
			case CondRelation.NotInRange:
				text3 = string.Format(Resources.Range, GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(linDataEvent.LowValue.Value), GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(linDataEvent.HighValue.Value));
				break;
			}
			if (flag)
			{
				return string.Format(Resources.DataConditionStartBit, new object[]
				{
					GUIUtil.GetNumberPrefix() + GUIUtil.LINIdToDisplayString(linDataEvent.ID.Value),
					num2,
					num3,
					text,
					text2,
					text3
				});
			}
			return string.Format(Resources.DataConditionByte, new object[]
			{
				GUIUtil.GetNumberPrefix() + GUIUtil.LINIdToDisplayString(linDataEvent.ID.Value),
				num,
				text2,
				text3
			});
		}

		public static string MapEventCondition2String(SymbolicMessageEvent symMsgEvent, IDatabaseServices databaseServices)
		{
			MessageDefinition messageDefinition;
			if (BusType.Bt_FlexRay != symMsgEvent.BusType.Value)
			{
				string arg;
				if (databaseServices.IsSymbolicMessageDefined(symMsgEvent.DatabasePath.Value, symMsgEvent.NetworkName.Value, symMsgEvent.MessageName.Value, symMsgEvent.ChannelNumber.Value, symMsgEvent.BusType.Value, out messageDefinition))
				{
					if (BusType.Bt_CAN == symMsgEvent.BusType.Value)
					{
						arg = GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(messageDefinition.ActualMessageId, messageDefinition.IsExtendedId);
					}
					else if (BusType.Bt_LIN == symMsgEvent.BusType.Value)
					{
						arg = GUIUtil.GetNumberPrefix() + GUIUtil.LINIdToDisplayString(messageDefinition.CanDbMessageId);
					}
					else
					{
						arg = GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(messageDefinition.CanDbMessageId);
					}
				}
				else
				{
					arg = "?";
				}
				string arg2 = symMsgEvent.MessageName.Value;
				if (!string.IsNullOrEmpty(symMsgEvent.NetworkName.Value))
				{
					arg2 = symMsgEvent.NetworkName.Value + "::" + symMsgEvent.MessageName.Value;
				}
				return string.Format(Vocabulary.TriggerConditionSymbolicMessage, arg2, arg, symMsgEvent.DatabaseName.Value);
			}
			string text = Resources.Unknown;
			string text2 = "";
			string text3 = Resources.Unknown;
			string text4 = Resources.Unknown;
			string text5 = Resources.Unknown;
			if (databaseServices.IsSymbolicMessageDefined(symMsgEvent.DatabasePath.Value, symMsgEvent.NetworkName.Value, symMsgEvent.MessageName.Value, symMsgEvent.ChannelNumber.Value, symMsgEvent.BusType.Value, out messageDefinition))
			{
				if (!symMsgEvent.IsFlexrayPDU.Value)
				{
					text = Resources.SymFlexrayMsgTypFrame;
					text3 = messageDefinition.CanDbMessageId.ToString();
					text4 = messageDefinition.FrBaseCycle.ToString();
					text5 = messageDefinition.FrCycleRepetition.ToString();
					text2 = ";";
				}
				IList<MessageDefinition> list = null;
				IList<string> list2 = null;
				bool flag = false;
				if (databaseServices.GetFlexrayFrameOrPDUInfo(symMsgEvent.DatabasePath.Value, symMsgEvent.NetworkName.Value, symMsgEvent.MessageName.Value, symMsgEvent.IsFlexrayPDU.Value, out list, out list2, out flag))
				{
					if (symMsgEvent.IsFlexrayPDU.Value)
					{
						text = Resources.SymFlexrayMsgTypPDU;
						if (list.Count > 0)
						{
							text3 = list[0].ActualMessageId.ToString();
							text4 = list[0].FrBaseCycle.ToString();
							text5 = list[0].FrCycleRepetition.ToString();
						}
						if (list.Count > 1)
						{
							text2 = "],...";
						}
						else
						{
							text2 = "] ";
						}
					}
					else if (list.Count > 1)
					{
						text = Resources.SymFlexrayMsgTypMultiFrame;
					}
				}
			}
			string text6 = symMsgEvent.MessageName.Value;
			if (!string.IsNullOrEmpty(symMsgEvent.NetworkName.Value))
			{
				text6 = symMsgEvent.NetworkName.Value + "::" + symMsgEvent.MessageName.Value;
			}
			return string.Format(Resources.SymFlexrayMsgCondition, new object[]
			{
				text6,
				text,
				text3,
				text4,
				text5,
				text2,
				symMsgEvent.DatabaseName.Value
			});
		}

		public static string MapEventCondition2String(ISymbolicSignalEvent symSigEvent, IDatabaseServices databaseServices)
		{
			SignalDefinition signalDefinition = null;
			bool flag = true;
			string text = Resources.Unknown;
			string text2 = Resources.Unknown;
			string text3 = string.Empty;
			bool flag2 = false;
			if (symSigEvent is SymbolicSignalEvent)
			{
				flag2 = databaseServices.IsSymbolicSignalDefined(symSigEvent.DatabasePath.Value, symSigEvent.NetworkName.Value, symSigEvent.MessageName.Value, symSigEvent.SignalName.Value, symSigEvent.ChannelNumber.Value, symSigEvent.BusType.Value, out signalDefinition);
			}
			else if (symSigEvent is DiagnosticSignalEvent)
			{
				DiagnosticSignalEvent diagnosticSignalEvent = symSigEvent as DiagnosticSignalEvent;
				flag2 = DiagSymbolsManager.Instance().GetSignalDefinition(diagnosticSignalEvent.DiagnosticEcuName.Value, diagnosticSignalEvent.DiagnosticVariant.Value, diagnosticSignalEvent.DiagnosticDid.Value, symSigEvent.SignalName.Value, out signalDefinition);
			}
			else
			{
				signalDefinition = CcpXcpManager.Instance().GetSignalDefinition(symSigEvent.SignalName.Value, symSigEvent.CcpXcpEcuName.Value);
				if (signalDefinition != null)
				{
					flag2 = true;
				}
			}
			if (!flag2)
			{
				signalDefinition = new SignalDefinition();
				signalDefinition.SetDummySignal();
				flag = false;
			}
			bool flag3 = false;
			bool flag4 = false;
			switch (symSigEvent.Relation.Value)
			{
			case CondRelation.Equal:
			case CondRelation.NotEqual:
			case CondRelation.LessThan:
			case CondRelation.LessThanOrEqual:
			case CondRelation.GreaterThan:
			case CondRelation.GreaterThanOrEqual:
				if (flag)
				{
					double physicalValue = 0.0;
					if (symSigEvent is SymbolicSignalEvent)
					{
						string empty = string.Empty;
						if (databaseServices.IsSignalValueTextEncoded(symSigEvent.DatabasePath.Value, symSigEvent.NetworkName.Value, symSigEvent.MessageName.Value, symSigEvent.SignalName.Value, symSigEvent.LowValue.Value, out empty))
						{
							text = empty;
							flag3 = true;
						}
						else
						{
							databaseServices.RawSignalValueToPhysicalValue(symSigEvent.DatabasePath.Value, symSigEvent.NetworkName.Value, symSigEvent.MessageName.Value, symSigEvent.SignalName.Value, symSigEvent.LowValue.Value, out physicalValue);
							text = signalDefinition.PhysicalValueToString(physicalValue);
						}
					}
					else
					{
						physicalValue = CcpXcpManager.Instance().RawSignalValueToPhysicalValue(symSigEvent.LowValue.Value, signalDefinition);
						text = signalDefinition.PhysicalValueToString(physicalValue);
					}
				}
				text3 = string.Format(Resources.SignalValueSingle, new object[]
				{
					text,
					flag3 ? "" : signalDefinition.Unit,
					GUIUtil.GetNumberPrefix(),
					GUIUtil.RawSignalValueToDisplayString(symSigEvent.LowValue.Value, signalDefinition)
				});
				break;
			case CondRelation.InRange:
			case CondRelation.NotInRange:
				if (flag)
				{
					double physicalValue2 = 0.0;
					double physicalValue3 = 0.0;
					if (symSigEvent is SymbolicSignalEvent)
					{
						string empty2 = string.Empty;
						if (databaseServices.IsSignalValueTextEncoded(symSigEvent.DatabasePath.Value, symSigEvent.NetworkName.Value, symSigEvent.MessageName.Value, symSigEvent.SignalName.Value, symSigEvent.LowValue.Value, out empty2))
						{
							text = empty2;
							flag3 = true;
						}
						else
						{
							databaseServices.RawSignalValueToPhysicalValue(symSigEvent.DatabasePath.Value, symSigEvent.NetworkName.Value, symSigEvent.MessageName.Value, symSigEvent.SignalName.Value, symSigEvent.LowValue.Value, out physicalValue2);
							text = signalDefinition.PhysicalValueToString(physicalValue2);
						}
						if (databaseServices.IsSignalValueTextEncoded(symSigEvent.DatabasePath.Value, symSigEvent.NetworkName.Value, symSigEvent.MessageName.Value, symSigEvent.SignalName.Value, symSigEvent.HighValue.Value, out empty2))
						{
							text2 = empty2;
							flag4 = true;
						}
						else
						{
							databaseServices.RawSignalValueToPhysicalValue(symSigEvent.DatabasePath.Value, symSigEvent.NetworkName.Value, symSigEvent.MessageName.Value, symSigEvent.SignalName.Value, symSigEvent.HighValue.Value, out physicalValue3);
							text2 = signalDefinition.PhysicalValueToString(physicalValue3);
						}
					}
					else
					{
						physicalValue2 = CcpXcpManager.Instance().RawSignalValueToPhysicalValue(symSigEvent.LowValue.Value, signalDefinition);
						physicalValue3 = CcpXcpManager.Instance().RawSignalValueToPhysicalValue(symSigEvent.HighValue.Value, signalDefinition);
						text = signalDefinition.PhysicalValueToString(physicalValue2);
						text2 = signalDefinition.PhysicalValueToString(physicalValue3);
					}
				}
				text3 = string.Format(Resources.SignalValueRange, new object[]
				{
					text,
					flag3 ? "" : signalDefinition.Unit,
					GUIUtil.GetNumberPrefix(),
					GUIUtil.RawSignalValueToDisplayString(symSigEvent.LowValue.Value, signalDefinition),
					text2,
					flag4 ? "" : signalDefinition.Unit,
					GUIUtil.GetNumberPrefix(),
					GUIUtil.RawSignalValueToDisplayString(symSigEvent.HighValue.Value, signalDefinition)
				});
				break;
			}
			string text4 = string.Empty;
			if (symSigEvent is SymbolicSignalEvent)
			{
				text4 = symSigEvent.MessageName.Value;
				if (!string.IsNullOrEmpty(symSigEvent.NetworkName.Value))
				{
					text4 = symSigEvent.NetworkName.Value + "::" + symSigEvent.MessageName.Value;
				}
			}
			else if (!string.IsNullOrEmpty(symSigEvent.CcpXcpEcuName.Value))
			{
				text4 = symSigEvent.CcpXcpEcuName.Value;
			}
			return string.Format(Vocabulary.TriggerConditionSymbolicSignal, new object[]
			{
				text4,
				symSigEvent.SignalName.Value,
				GUIUtil.MapTriggerConditionRelation2String(symSigEvent.Relation.Value),
				text3
			});
		}

		public static string MapEventCondition2String(MsgTimeoutEvent msgTimeoutEv, IDatabaseServices databaseServices)
		{
			bool flag = true;
			string arg3;
			if (msgTimeoutEv.IsSymbolic.Value)
			{
				MessageDefinition messageDefinition;
				string arg;
				if (databaseServices.IsSymbolicMessageDefined(msgTimeoutEv.DatabasePath.Value, msgTimeoutEv.NetworkName.Value, msgTimeoutEv.MessageName.Value, msgTimeoutEv.ChannelNumber.Value, msgTimeoutEv.BusType.Value, out messageDefinition))
				{
					if (BusType.Bt_CAN == msgTimeoutEv.BusType.Value)
					{
						arg = GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(messageDefinition.ActualMessageId, messageDefinition.IsExtendedId);
					}
					else if (BusType.Bt_LIN == msgTimeoutEv.BusType.Value)
					{
						arg = GUIUtil.GetNumberPrefix() + GUIUtil.LINIdToDisplayString(messageDefinition.CanDbMessageId);
					}
					else
					{
						arg = GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(messageDefinition.CanDbMessageId);
					}
					msgTimeoutEv.DatabaseCycleTime = (uint)messageDefinition.CycleTime;
					msgTimeoutEv.IsDatabaseMsgCyclic = messageDefinition.IsCyclic;
				}
				else
				{
					arg = "?";
					flag = false;
					msgTimeoutEv.DatabaseCycleTime = 0u;
				}
				string arg2 = msgTimeoutEv.MessageName.Value;
				if (!string.IsNullOrEmpty(msgTimeoutEv.NetworkName.Value))
				{
					arg2 = msgTimeoutEv.NetworkName.Value + "::" + msgTimeoutEv.MessageName.Value;
				}
				arg3 = string.Format(Vocabulary.TriggerConditionSymbolicMessage, arg2, arg, msgTimeoutEv.DatabaseName.Value);
			}
			else
			{
				arg3 = "";
				if (BusType.Bt_CAN == msgTimeoutEv.BusType.Value)
				{
					arg3 = "ID = " + GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(msgTimeoutEv.ID.Value, msgTimeoutEv.IsExtendedId.Value);
				}
				else if (BusType.Bt_LIN == msgTimeoutEv.BusType.Value)
				{
					arg3 = "ID =  " + GUIUtil.GetNumberPrefix() + GUIUtil.LINIdToDisplayString(msgTimeoutEv.ID.Value);
				}
			}
			string arg4;
			if (msgTimeoutEv.IsCycletimeFromDatabase.Value && (!flag || !msgTimeoutEv.IsDatabaseMsgCyclic))
			{
				arg4 = "?";
			}
			else
			{
				arg4 = msgTimeoutEv.ResultingTimeout.ToString();
			}
			return string.Format(Resources_Trigger.TriggerCondMsgTimeout, arg3, arg4);
		}

		public static string MapEventCondition2String(CANBusStatisticsEvent statEvent)
		{
			string result = "";
			string text = "";
			string text2 = "";
			if (statEvent.IsBusloadEnabled.Value)
			{
				text = string.Format(Resources_Trigger.TriggerConditionCANBusstatisticsBusload, GUIUtil.MapTriggerConditionRelation2String(statEvent.BusloadRelation.Value), statEvent.BusloadLow.Value, statEvent.BusloadHigh.Value);
			}
			if (statEvent.IsErrorFramesEnabled.Value)
			{
				text2 = string.Format(Resources_Trigger.TriggerConditionCANBusstatisticsErrorFrames, GUIUtil.MapTriggerConditionRelation2String(statEvent.ErrorFramesRelation.Value), statEvent.ErrorFramesLow.Value);
			}
			if (statEvent.IsBusloadEnabled.Value && statEvent.IsErrorFramesEnabled.Value)
			{
				if (statEvent.IsANDConjunction.Value)
				{
					result = string.Format(Resources_Trigger.TriggerConditionCANBusstatisticsAND, text, text2);
				}
				else
				{
					result = string.Format(Resources_Trigger.TriggerConditionCANBusstatisticsOR, text, text2);
				}
			}
			else
			{
				if (statEvent.IsBusloadEnabled.Value)
				{
					result = text;
				}
				if (statEvent.IsErrorFramesEnabled.Value)
				{
					result = text2;
				}
			}
			return result;
		}

		public static string MapEventCondition2String(DigitalInputEvent digInEvent)
		{
			return GUIUtil.MapEventCondition2String(digInEvent, false);
		}

		public static string MapEventCondition2String(DigitalInputEvent digInEvent, bool isSignalState)
		{
			string arg;
			if (!isSignalState)
			{
				if (digInEvent.Edge.Value)
				{
					arg = Resources.DigitalInputEdgeLowToHigh;
				}
				else
				{
					arg = Resources.DigitalInputEdgeHighToLow;
				}
			}
			else if (digInEvent.Edge.Value)
			{
				arg = Resources.DigitalInputStateHigh;
			}
			else
			{
				arg = Resources.DigitalInputStateLow;
			}
			if (!digInEvent.IsDebounceActive.Value)
			{
				return string.Format(Resources.DigInEventCondNoDebounce, arg);
			}
			return string.Format(Resources.DigInEventCondDebounceTime, arg, digInEvent.DebounceTime.Value);
		}

		public static string MapEventCondition2String(AnalogInputEvent analogInputEvent)
		{
			string result = "";
			switch (analogInputEvent.Relation.Value)
			{
			case CondRelation.Equal:
			case CondRelation.NotEqual:
			case CondRelation.LessThan:
			case CondRelation.LessThanOrEqual:
			case CondRelation.GreaterThan:
			case CondRelation.GreaterThanOrEqual:
				result = string.Format(Resources_Trigger.TriggerConditionAnalogInput, GUIUtil.MapTriggerConditionRelation2String(analogInputEvent.Relation.Value), analogInputEvent.LowValue, analogInputEvent.Tolerance.Value);
				break;
			case CondRelation.InRange:
			case CondRelation.NotInRange:
				result = string.Format(Resources_Trigger.TriggerConditionAnalogInputRange, new object[]
				{
					GUIUtil.MapTriggerConditionRelation2String(analogInputEvent.Relation.Value),
					analogInputEvent.LowValue.Value,
					analogInputEvent.HighValue.Value,
					analogInputEvent.Tolerance.Value
				});
				break;
			}
			return result;
		}

		public static string MapEventCondition2String(IgnitionEvent ignitionEvent)
		{
			if (ignitionEvent.IsOn.Value)
			{
				return Resources.IgnitionOn;
			}
			return Resources.IgnitionOff;
		}

		public static string MapEventCondition2String(VoCanRecordingEvent voCanEvent)
		{
			string text = string.Format(Resources_Trigger.TriggerConditionVoCanPostfix, voCanEvent.IsBeepOnEndOn.Value ? Resources.StatusOn : Resources.StatusOff, voCanEvent.IsRecordingLEDActive.Value ? Resources.StatusOn : Resources.StatusOff);
			if (voCanEvent.IsFixedRecordDuration.Value)
			{
				return string.Format(Resources_Trigger.TriggerConditionVoCanRecordingFixed, voCanEvent.IsUsingCASM2T3L.Value ? Resources.VoCanNameCASM2T3L : Resources.VoCanNameStd, voCanEvent.Duration_s.Value, text);
			}
			return string.Format(Resources_Trigger.TriggerConditionVoCanRecordingDyn, voCanEvent.IsUsingCASM2T3L.Value ? Resources.VoCanNameCASM2T3L : Resources.VoCanNameStd, text);
		}

		public static string MapEventCondition2String(ClockTimedEvent timedEvent)
		{
			if (timedEvent.RepetitionInterval.Value == ClockTimedEvent.TimeSpan_Daily)
			{
				return string.Format(Resources.ClockTimeDaily, timedEvent.StartTime.Value.ToShortTimeString());
			}
			if (timedEvent.RepetitionInterval.Value >= ClockTimedEvent.TimeSpan_2h)
			{
				return string.Format(Resources.ClockTimeStartAndRepEveryXHours, timedEvent.StartTime.Value.ToShortTimeString(), timedEvent.RepetitionInterval.Value.Hours);
			}
			if (timedEvent.RepetitionInterval.Value == ClockTimedEvent.TimeSpan_1h)
			{
				return string.Format(Resources.ClockTimeStartAndRepEveryHour, timedEvent.StartTime.Value.ToShortTimeString());
			}
			return string.Format(Resources.ClockTimeStartAndRepEveryXMin, timedEvent.StartTime.Value.ToShortTimeString(), timedEvent.RepetitionInterval.Value.Minutes);
		}

		public static string MapEventCondition2String(IncEvent incEvent)
		{
			IncludeFileParameterPresenter includeFileParameterPresenter = IncludeFileManager.Instance.OutParameters.FirstOrDefault((IncludeFileParameterPresenter param) => incEvent.FilePath.Value.Equals(param.IncludeFile.FilePath.Value) && incEvent.ParamIndex.Value.Equals(param.ParameterIndex));
			string value = (includeFileParameterPresenter != null) ? includeFileParameterPresenter.Name : string.Format(Resources_IncFiles.InParameterDefaultName, incEvent.ParamIndex.Value + 1);
			string value2 = GUIUtil.MapTriggerConditionRelation2String(incEvent.Relation.Value);
			string text = GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(incEvent.LowValue.Value);
			string arg = GUIUtil.GetNumberPrefix() + GUIUtil.NumberToDisplayString(incEvent.HighValue.Value);
			string value3;
			switch (incEvent.Relation.Value)
			{
			case CondRelation.Equal:
			case CondRelation.NotEqual:
			case CondRelation.LessThan:
			case CondRelation.LessThanOrEqual:
			case CondRelation.GreaterThan:
			case CondRelation.GreaterThanOrEqual:
				value3 = text;
				break;
			case CondRelation.InRange:
			case CondRelation.NotInRange:
				value3 = string.Format(Resources.Range, text, arg);
				break;
			default:
				value3 = string.Empty;
				break;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(value);
			stringBuilder.Append(" ");
			stringBuilder.Append(value2);
			stringBuilder.Append(" ");
			stringBuilder.Append(value3);
			return stringBuilder.ToString();
		}

		public static string MapTriggerConditionRelation2String(CondRelation conditionRelation)
		{
			switch (conditionRelation)
			{
			case CondRelation.Equal:
				return Resources.ConditionEqual;
			case CondRelation.NotEqual:
				return Resources.ConditionNotEqual;
			case CondRelation.LessThan:
				return Resources.ConditionLess;
			case CondRelation.LessThanOrEqual:
				return Resources.ConditionLessOrEqual;
			case CondRelation.GreaterThan:
				return Resources.ConditionGreater;
			case CondRelation.GreaterThanOrEqual:
				return Resources.CondtionGreaterOrEqual;
			case CondRelation.InRange:
				return Resources.ConditionInRange;
			case CondRelation.NotInRange:
				return Resources.ConditionNotInRange;
			case CondRelation.OnChange:
				return Resources.ConditionOnChange;
			default:
				return string.Empty;
			}
		}

		public static string MapEventCondition2String(CombinedEvent combinedEvent)
		{
			if (!combinedEvent.IsConjunction.Value)
			{
				return Resources_Trigger.TriggerConditionGroupOR;
			}
			return Resources_Trigger.TriggerConditionGroupAND;
		}

		public static CondRelation MapString2TriggerConditionRelation(string relationString)
		{
			foreach (CondRelation condRelation in Enum.GetValues(typeof(CondRelation)))
			{
				if (relationString == GUIUtil.MapTriggerConditionRelation2String(condRelation))
				{
					return condRelation;
				}
			}
			return CondRelation.Equal;
		}

		public static string MapEventCondition2MarkerName(SymbolicMessageEvent symMsgEvent, IDatabaseServices databaseService)
		{
			return symMsgEvent.MessageName.Value;
		}

		public static string MapEventCondition2MarkerName(DigitalInputEvent digInEvent)
		{
			string arg;
			if (digInEvent.Edge.Value)
			{
				arg = "_HIGH";
			}
			else
			{
				arg = "_LOW";
			}
			return "DigIn" + digInEvent.DigitalInput + arg;
		}

		public static string MapEventCondition2MarkerName(AnalogInputEvent analogInputEvent)
		{
			return string.Concat(new object[]
			{
				"Analog",
				analogInputEvent.InputNumber,
				GUIUtil.GetCommentCompareOperatorStringForSignals(analogInputEvent.Relation.Value, analogInputEvent.LowValue.Value, analogInputEvent.HighValue.Value),
				"mV"
			});
		}

		public static string MapEventCondition2MarkerName(CANBusStatisticsEvent statEvent)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			if (statEvent.IsBusloadEnabled.Value)
			{
				text2 = "Busload" + GUIUtil.GetCommentCompareOperatorStringForSignals(statEvent.BusloadRelation.Value, statEvent.BusloadLow.Value, statEvent.BusloadHigh.Value);
			}
			if (statEvent.IsErrorFramesEnabled.Value)
			{
				text3 = "ErrorFrames" + GUIUtil.GetCommentCompareOperatorStringForSignals(statEvent.ErrorFramesRelation.Value, statEvent.ErrorFramesLow.Value, statEvent.ErrorFramesHigh.Value);
			}
			if (statEvent.IsBusloadEnabled.Value && statEvent.IsErrorFramesEnabled.Value)
			{
				if (statEvent.IsANDConjunction.Value)
				{
					text = text2 + "_AND_" + text3;
				}
				else
				{
					text = text2 + "_OR_" + text3;
				}
			}
			else
			{
				if (statEvent.IsBusloadEnabled.Value)
				{
					text = text2;
				}
				if (statEvent.IsErrorFramesEnabled.Value)
				{
					text = text3;
				}
			}
			return text.Replace("%", "");
		}

		public static string MapEventCondition2MarkerName(ISymbolicSignalEvent symSigEvent, IDatabaseServices databaseService)
		{
			SignalDefinition signalDefinition;
			if (symSigEvent is SymbolicSignalEvent)
			{
				if (!databaseService.IsSymbolicSignalDefined(symSigEvent.DatabasePath.Value, symSigEvent.NetworkName.Value, symSigEvent.MessageName.Value, symSigEvent.SignalName.Value, symSigEvent.ChannelNumber.Value, symSigEvent.BusType.Value, out signalDefinition))
				{
					return "Unresolved_Symbolic_Signal_Event";
				}
			}
			else if (symSigEvent is DiagnosticSignalEvent)
			{
				DiagnosticSignalEvent diagnosticSignalEvent = symSigEvent as DiagnosticSignalEvent;
				bool signalDefinition2 = DiagSymbolsManager.Instance().GetSignalDefinition(diagnosticSignalEvent.DiagnosticEcuName.Value, diagnosticSignalEvent.DiagnosticVariant.Value, diagnosticSignalEvent.DiagnosticDid.Value, symSigEvent.SignalName.Value, out signalDefinition);
				if (!signalDefinition2 || signalDefinition == null)
				{
					return "Unresolved_Diagnostic_Signal_Event";
				}
			}
			else
			{
				signalDefinition = CcpXcpManager.Instance().GetSignalDefinition(symSigEvent.SignalName.Value, symSigEvent.CcpXcpEcuName.Value);
				if (signalDefinition == null)
				{
					return "Unresolved_CcpXcp_Signal_Event";
				}
			}
			string text = string.Empty;
			if (!string.IsNullOrEmpty(signalDefinition.Unit))
			{
				text = Regex.Replace(signalDefinition.Unit, "[^a-zA-Z0-9_]", "");
			}
			double num = 0.0;
			double num2 = 0.0;
			string text2 = string.Empty;
			string text3 = string.Empty;
			if (symSigEvent is SymbolicSignalEvent)
			{
				databaseService.RawSignalValueToPhysicalValue(symSigEvent.DatabasePath.Value, symSigEvent.NetworkName.Value, symSigEvent.MessageName.Value, symSigEvent.SignalName.Value, symSigEvent.LowValue.Value, out num);
			}
			else
			{
				num = CcpXcpManager.Instance().RawSignalValueToPhysicalValue(symSigEvent.LowValue.Value, signalDefinition);
			}
			text2 += GUIUtil.MakePhysicalValueCaplCompliant(ref num);
			if (symSigEvent.Relation.Value != CondRelation.InRange && symSigEvent.Relation.Value != CondRelation.NotInRange)
			{
				text2 += text;
			}
			else
			{
				if (symSigEvent is SymbolicSignalEvent)
				{
					databaseService.RawSignalValueToPhysicalValue(symSigEvent.DatabasePath.Value, symSigEvent.NetworkName.Value, symSigEvent.MessageName.Value, symSigEvent.SignalName.Value, symSigEvent.HighValue.Value, out num2);
				}
				else
				{
					num2 = CcpXcpManager.Instance().RawSignalValueToPhysicalValue(symSigEvent.HighValue.Value, signalDefinition);
				}
				text3 = text3 + GUIUtil.MakePhysicalValueCaplCompliant(ref num2) + text;
			}
			string text4 = symSigEvent.SignalName.Value;
			if (symSigEvent is CcpXcpSignalEvent || symSigEvent is DiagnosticSignalEvent)
			{
				text4 = GUIUtil.RemoveNonCaplCompliantChars(text4);
			}
			return string.Format("{0}{1}", text4, GUIUtil.GetCommentCompareOperatorStringForSignals(symSigEvent.Relation.Value, text2, text3));
		}

		public static string RemoveNonCaplCompliantChars(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return value;
			}
			string result;
			try
			{
				result = Regex.Replace(value, "[^a-zA-Z0-9_]", "_");
			}
			catch (Exception)
			{
				result = value;
			}
			return result;
		}

		private static string MakePhysicalValueCaplCompliant(ref double value)
		{
			string str = string.Empty;
			if (value < 0.0)
			{
				str += "minus";
				value = Math.Abs(value);
			}
			value = Math.Floor(value);
			return str + value.ToString("F0");
		}

		private static string GetCaplCompliantNumberString(int number)
		{
			if (GUIUtil._isHexadecimal)
			{
				return number.ToString("X");
			}
			string str = string.Empty;
			if (number < 0)
			{
				str += "minus";
				number = Math.Abs(number);
			}
			return str + number.ToString("D");
		}

		private static string GetCommentCompareOperatorStringForSignals(CondRelation condRelation, double lowValue, double highValue)
		{
			return GUIUtil.GetCommentCompareOperatorStringForSignals(condRelation, lowValue.ToString(), highValue.ToString());
		}

		private static string GetCommentCompareOperatorStringForSignals(CondRelation condRelation, string lowValue, string highValue)
		{
			switch (condRelation)
			{
			case CondRelation.Equal:
				return string.Format("_{0}", lowValue);
			case CondRelation.NotEqual:
				return string.Format("_neq_{0}", lowValue);
			case CondRelation.LessThan:
				return string.Format("_lt_{0}", lowValue);
			case CondRelation.LessThanOrEqual:
				return string.Format("_lte_{0}", lowValue);
			case CondRelation.GreaterThan:
				return string.Format("_gt_{0}", lowValue);
			case CondRelation.GreaterThanOrEqual:
				return string.Format("_gte_{0}", lowValue);
			case CondRelation.InRange:
				return string.Format("_IN_{0}_to_{1}", lowValue, highValue);
			case CondRelation.NotInRange:
				return string.Format("_OUT_{0}_to_{1}", lowValue, highValue);
			case CondRelation.OnChange:
				return string.Format("_OnChange", new object[0]);
			default:
				return "no compare";
			}
		}

		public static string MapEventCondition2MarkerName(IdEvent idEvent)
		{
			string lowValue = string.Empty;
			string highValue = string.Empty;
			string str = string.Empty;
			string str2 = string.Empty;
			if (idEvent is CANIdEvent)
			{
				lowValue = GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(idEvent.LowId.Value, (idEvent as CANIdEvent).IsExtendedId.Value);
				highValue = GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(idEvent.HighId.Value, (idEvent as CANIdEvent).IsExtendedId.Value);
				str = string.Format("CAN{0:D}_ID", idEvent.ChannelNumber.Value);
			}
			else if (idEvent is LINIdEvent)
			{
				lowValue = GUIUtil.GetNumberPrefix() + GUIUtil.LINIdToDisplayString(idEvent.LowId.Value);
				highValue = GUIUtil.GetNumberPrefix() + GUIUtil.LINIdToDisplayString(idEvent.HighId.Value);
				str = string.Format("LIN{0:D}_ID", idEvent.ChannelNumber.Value);
			}
			else
			{
				if (!(idEvent is FlexrayIdEvent))
				{
					return "";
				}
				FlexrayIdEvent flexrayIdEvent = idEvent as FlexrayIdEvent;
				str = string.Format("FlexRay{0:D}_frame", idEvent.ChannelNumber.Value);
				lowValue = GUIUtil.FlexraySlotIdToDisplayString(idEvent.LowId.Value);
				highValue = GUIUtil.FlexraySlotIdToDisplayString(idEvent.HighId.Value);
				if (idEvent.IdRelation.Value != CondRelation.NotInRange && idEvent.IdRelation.Value != CondRelation.InRange)
				{
					str2 = string.Concat(new object[]
					{
						"_",
						flexrayIdEvent.BaseCycle.Value,
						"_",
						flexrayIdEvent.CycleRepetition.Value
					});
				}
			}
			string commentCompareOperatorStringForSignals = GUIUtil.GetCommentCompareOperatorStringForSignals(idEvent.IdRelation.Value, lowValue, highValue);
			return str + commentCompareOperatorStringForSignals + str2;
		}

		public static string MapEventCondition2MarkerName(CANDataEvent canDataEvent)
		{
			uint num = 0u;
			bool flag = false;
			if (canDataEvent.RawDataSignal is RawDataSignalByte)
			{
				num = (canDataEvent.RawDataSignal as RawDataSignalByte).DataBytePos.Value;
			}
			else if (canDataEvent.RawDataSignal is RawDataSignalStartbitLength)
			{
				flag = true;
			}
			string commentCompareOperatorStringForSignals = GUIUtil.GetCommentCompareOperatorStringForSignals(canDataEvent.Relation.Value, canDataEvent.LowValue.Value, canDataEvent.HighValue.Value);
			if (flag)
			{
				return string.Format("ID_{0}_RawSignal{1}", GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(canDataEvent.ID.Value, canDataEvent.IsExtendedId.Value), commentCompareOperatorStringForSignals);
			}
			return string.Format("ID_{0}_Byte{1}{2}", GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(canDataEvent.ID.Value, canDataEvent.IsExtendedId.Value), num, commentCompareOperatorStringForSignals);
		}

		public static string MapEventCondition2MarkerName(LINDataEvent linDataEvent)
		{
			uint num = 0u;
			bool flag = false;
			if (linDataEvent.RawDataSignal is RawDataSignalByte)
			{
				num = (linDataEvent.RawDataSignal as RawDataSignalByte).DataBytePos.Value;
			}
			else if (linDataEvent.RawDataSignal is RawDataSignalStartbitLength)
			{
				flag = true;
			}
			string commentCompareOperatorStringForSignals = GUIUtil.GetCommentCompareOperatorStringForSignals(linDataEvent.Relation.Value, linDataEvent.LowValue.Value, linDataEvent.HighValue.Value);
			if (flag)
			{
				return string.Format("ID_{0}_RawSignal{1}", GUIUtil.GetNumberPrefix() + GUIUtil.LINIdToDisplayString(linDataEvent.ID.Value), commentCompareOperatorStringForSignals);
			}
			return string.Format("ID_{0}_Byte{1}{2}", GUIUtil.GetNumberPrefix() + GUIUtil.LINIdToDisplayString(linDataEvent.ID.Value), num, commentCompareOperatorStringForSignals);
		}

		public static string MapEventCondition2MarkerName(IgnitionEvent ignitionEvent)
		{
			if (ignitionEvent.IsOn.Value)
			{
				return "Ignition_On";
			}
			return "Ignition_Off";
		}

		public static string MapEventCondition2MarkerName(MsgTimeoutEvent msgTimeoutEv, IDatabaseServices databaseServices)
		{
			string str;
			if (msgTimeoutEv.IsSymbolic.Value)
			{
				str = msgTimeoutEv.MessageName.Value;
			}
			else
			{
				str = "";
				if (BusType.Bt_CAN == msgTimeoutEv.BusType.Value)
				{
					str = "ID_" + GUIUtil.GetNumberPrefix() + GUIUtil.CANIdToDisplayString(msgTimeoutEv.ID.Value, msgTimeoutEv.IsExtendedId.Value);
				}
				else if (BusType.Bt_LIN == msgTimeoutEv.BusType.Value)
				{
					str = "ID_" + GUIUtil.GetNumberPrefix() + GUIUtil.LINIdToDisplayString(msgTimeoutEv.ID.Value);
				}
			}
			return "Timeout_on_" + str;
		}

		public static string MapEventCondition2MarkerName(IncEvent incEvent)
		{
			IncludeFileParameterPresenter includeFileParameterPresenter = IncludeFileManager.Instance.OutParameters.FirstOrDefault((IncludeFileParameterPresenter param) => incEvent.FilePath.Value.Equals(param.IncludeFile.FilePath.Value) && incEvent.ParamIndex.Value.Equals(param.ParameterIndex));
			string text = (includeFileParameterPresenter != null) ? includeFileParameterPresenter.Name : string.Format(Resources_IncFiles.InParameterDefaultName, incEvent.ParamIndex.Value + 1);
			text = GUIUtil.RemoveNonCaplCompliantChars(text);
			string lowValue = GUIUtil.GetNumberPrefix() + GUIUtil.GetCaplCompliantNumberString(incEvent.LowValue.Value);
			string highValue = GUIUtil.GetNumberPrefix() + GUIUtil.GetCaplCompliantNumberString(incEvent.HighValue.Value);
			string commentCompareOperatorStringForSignals = GUIUtil.GetCommentCompareOperatorStringForSignals(incEvent.Relation.Value, lowValue, highValue);
			return text + commentCompareOperatorStringForSignals;
		}

		public static void InitSplitButtonMenuEventTypes(SplitButtonEx splitButtonEx)
		{
			foreach (string current in GUIUtil.EventTypeNamesInMenu)
			{
				string groupName = string.Empty;
				object tag = null;
				if (current == "combined event")
				{
					groupName = "complex";
					tag = "combined event";
				}
				else if (current == Vocabulary.TriggerTypeNameColOnStart || current == Resources_Trigger.TriggerTypeNameColOnCycTimer)
				{
					groupName = "timed";
				}
				else if (current == Resources_Trigger.TriggerTypeNameColCANBusStatistics || current == Resources_Trigger.TriggerTypeNameColCANData || current == Vocabulary.TriggerTypeNameColCANId || current == Resources_Trigger.TriggerTypeNameColCANMsgTimeout || current == Resources_Trigger.TriggerTypeNameColSymbolicCAN || current == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
				{
					groupName = "CAN";
				}
				else if (current == Resources_Trigger.TriggerTypeNameColLINData || current == Vocabulary.TriggerTypeNameColLINId || current == Resources_Trigger.TriggerTypeNameColLINMsgTimeout || current == Resources_Trigger.TriggerTypeNameColSymbolicLIN || current == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
				{
					groupName = "LIN";
				}
				else if (current == Resources_Trigger.TriggerTypeNameColFlexray || current == Resources_Trigger.TriggerTypeNameColSymbolicFlexray || current == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
				{
					groupName = "FlexRay";
				}
				else if (current == Resources_Trigger.TriggerTypeNameColAnalogInput || current == Resources_Trigger.TriggerTypeNameColDigitalInput || current == Resources_Trigger.TriggerTypeNameColIgnition || current == Resources_Trigger.TriggerTypeNameColKey)
				{
					groupName = "IO";
				}
				else if (current == Resources_Trigger.TriggerTypeNameColVoCanRecording)
				{
					groupName = "VoCAN";
				}
				else if (current == Resources_Trigger.TriggerTypeNameColCcpXcpSignal)
				{
					groupName = "CcpXcp";
				}
				else if (current == Resources_Trigger.TriggerTypeNameColDiagnosticSignal)
				{
					groupName = "Diagnostic";
				}
				else if (current == Resources_Trigger.TriggerTypeNameColIncEvent)
				{
					groupName = "Extensions";
				}
				ToolStripItem toolStripItem = splitButtonEx.AddItem(current, GUIUtil.GetEventTypeImage(current), groupName);
				toolStripItem.Tag = tag;
			}
		}

		private static Image GetEventTypeImage(string eventName)
		{
			MainImageList.IconIndex baseImgIndex = MainImageList.IconIndex.NoImage;
			if (eventName == "combined event")
			{
				baseImgIndex = MainImageList.IconIndex.ConditionGroup;
			}
			else if (eventName == Vocabulary.TriggerTypeNameColOnStart)
			{
				baseImgIndex = MainImageList.IconIndex.Start;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColOnCycTimer)
			{
				baseImgIndex = MainImageList.IconIndex.CyclicTimer;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColSymbolicCAN)
			{
				baseImgIndex = MainImageList.IconIndex.SymbolicMessageCAN;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColSymbolicFlexray)
			{
				baseImgIndex = MainImageList.IconIndex.SymbolicMessageFlexray;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColSymbolicLIN)
			{
				baseImgIndex = MainImageList.IconIndex.SymbolicMessageLIN;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
			{
				baseImgIndex = MainImageList.IconIndex.SymbolicSignalCAN;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
			{
				baseImgIndex = MainImageList.IconIndex.SymbolicSignalFlexray;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
			{
				baseImgIndex = MainImageList.IconIndex.SymbolicSignalLIN;
			}
			else if (eventName == Vocabulary.TriggerTypeNameColCANId)
			{
				baseImgIndex = MainImageList.IconIndex.RawMessageCAN;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColFlexray)
			{
				baseImgIndex = MainImageList.IconIndex.RawMessageFlexray;
			}
			else if (eventName == Vocabulary.TriggerTypeNameColLINId)
			{
				baseImgIndex = MainImageList.IconIndex.RawMessageLIN;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColCANData)
			{
				baseImgIndex = MainImageList.IconIndex.DataTriggerCAN;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColLINData)
			{
				baseImgIndex = MainImageList.IconIndex.DataTriggerLIN;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColCANBusStatistics)
			{
				baseImgIndex = MainImageList.IconIndex.CANBusStatistics;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColAnalogInput)
			{
				baseImgIndex = MainImageList.IconIndex.AnalogInput;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColDigitalInput)
			{
				baseImgIndex = MainImageList.IconIndex.DigitalInput;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColIgnition)
			{
				baseImgIndex = MainImageList.IconIndex.OnIgnition;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColKey)
			{
				baseImgIndex = MainImageList.IconIndex.Key;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColCANMsgTimeout)
			{
				baseImgIndex = MainImageList.IconIndex.MessageTimeoutCAN;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColLINMsgTimeout)
			{
				baseImgIndex = MainImageList.IconIndex.MessageTimeoutLIN;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColVoCanRecording)
			{
				baseImgIndex = MainImageList.IconIndex.VoCanRecoding;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColCcpXcpSignal)
			{
				baseImgIndex = MainImageList.IconIndex.CcpXcpSignal;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColDiagnosticSignal)
			{
				baseImgIndex = MainImageList.IconIndex.DiagnosticSignal;
			}
			else if (eventName == Resources_Trigger.TriggerTypeNameColIncEvent)
			{
				baseImgIndex = MainImageList.IconIndex.IncludeFile;
			}
			return MainImageList.Instance.GetImage(baseImgIndex);
		}

		public static string MapTriggerEffect2String(TriggerEffect triggerType)
		{
			switch (triggerType)
			{
			case TriggerEffect.Normal:
				return Vocabulary.TriggerEffectNormal;
			case TriggerEffect.Single:
				return Resources_Trigger.TriggerEffectSingle;
			case TriggerEffect.EndMeasurement:
				return Resources_Trigger.TriggerEffectEndMeasurement;
			case TriggerEffect.LoggingOn:
				return Resources_Trigger.TriggerEffectLoggingOn;
			case TriggerEffect.LoggingOff:
				return Resources_Trigger.TriggerEffectLoggingOff;
			case TriggerEffect.Marker:
				return Vocabulary.TriggerEffectMarker;
			default:
				return Vocabulary.TriggerEffectNormal;
			}
		}

		public static TriggerEffect MapTriggerString2Effect(string triggerTypeString)
		{
			if (triggerTypeString == Vocabulary.TriggerEffectNormal)
			{
				return TriggerEffect.Normal;
			}
			if (triggerTypeString == Resources_Trigger.TriggerEffectSingle)
			{
				return TriggerEffect.Single;
			}
			if (triggerTypeString == Resources_Trigger.TriggerEffectEndMeasurement)
			{
				return TriggerEffect.EndMeasurement;
			}
			if (triggerTypeString == Resources_Trigger.TriggerEffectLoggingOn)
			{
				return TriggerEffect.LoggingOn;
			}
			if (triggerTypeString == Resources_Trigger.TriggerEffectLoggingOff)
			{
				return TriggerEffect.LoggingOff;
			}
			if (triggerTypeString == Vocabulary.TriggerEffectMarker)
			{
				return TriggerEffect.Marker;
			}
			return TriggerEffect.Normal;
		}

		public static TriggerAction MapString2TriggerAction(string actionString)
		{
			if (actionString == Resources_Trigger.TriggerActionNone)
			{
				return TriggerAction.None;
			}
			if (actionString == Resources_Trigger.TriggerActionBeep)
			{
				return TriggerAction.Beep;
			}
			if (actionString == Resources_Trigger.TriggerActionUnknown)
			{
				return TriggerAction.Unknown;
			}
			return TriggerAction.Unknown;
		}

		public static string MapTriggerAction2String(TriggerAction action)
		{
			if (action == TriggerAction.None)
			{
				return Resources_Trigger.TriggerActionNone;
			}
			if (TriggerAction.Beep == action)
			{
				return Resources_Trigger.TriggerActionBeep;
			}
			if (TriggerAction.Unknown == action)
			{
				return Resources_Trigger.TriggerActionUnknown;
			}
			return "";
		}

		public static string MapKeyNumber2String(uint keyNumber, bool isOnPanel)
		{
			if (isOnPanel)
			{
				return string.Format(Resources.PanelKeyName, keyNumber);
			}
			if (keyNumber > Constants.CasKeyOffset)
			{
				return string.Format(Resources.KeyNameCas, keyNumber - Constants.CasKeyOffset);
			}
			return string.Format(Resources.KeyName, keyNumber);
		}

		public static uint MapStringToKeyNumber(string keyNameString, out bool isOnPanel)
		{
			isOnPanel = false;
			uint num = 0u;
			if (keyNameString.IndexOf(Resources.KeyPrefix) == 0 && keyNameString.Length > Resources.KeyPrefix.Length)
			{
				if (uint.TryParse(keyNameString.Substring(Resources.KeyPrefix.Length), out num))
				{
					return num;
				}
			}
			else if (keyNameString.IndexOf(Resources.PanelKeyPrefix) == 0 && keyNameString.Length > Resources.PanelKeyPrefix.Length)
			{
				isOnPanel = true;
				if (uint.TryParse(keyNameString.Substring(Resources.PanelKeyPrefix.Length), out num))
				{
					return num;
				}
			}
			else if (keyNameString.IndexOf(Resources.CasKeyPrefix) == 0 && keyNameString.Length > Resources.CasKeyPrefix.Length && uint.TryParse(keyNameString.Substring(Resources.CasKeyPrefix.Length), out num))
			{
				return num + Constants.CasKeyOffset;
			}
			return num;
		}

		public static bool TryMapStringToKeyNumber(string keyNameString, out uint number, out bool isOnPanel)
		{
			number = 0u;
			isOnPanel = false;
			if (keyNameString.IndexOf(Resources.KeyPrefix) == 0 && keyNameString.Length > Resources.KeyPrefix.Length)
			{
				return uint.TryParse(keyNameString.Substring(Resources.KeyPrefix.Length), out number);
			}
			if (keyNameString.IndexOf(Resources.PanelKeyPrefix) == 0 && keyNameString.Length > Resources.PanelKeyPrefix.Length)
			{
				isOnPanel = true;
				return uint.TryParse(keyNameString.Substring(Resources.PanelKeyPrefix.Length), out number);
			}
			if (keyNameString.IndexOf(Resources.CasKeyPrefix) == 0 && keyNameString.Length > Resources.CasKeyPrefix.Length)
			{
				bool result = uint.TryParse(keyNameString.Substring(Resources.CasKeyPrefix.Length), out number);
				number += Constants.CasKeyOffset;
				return result;
			}
			return false;
		}

		public static string MapDataTransferMode2String(DataTransferModeType mode)
		{
			if (mode == DataTransferModeType.RecordWhileDataTransfer)
			{
				return Resources.DataTransferModeRecord;
			}
			if (mode == DataTransferModeType.StopWhileDataTransfer)
			{
				return Resources.DataTransferModeStop;
			}
			return "";
		}

		public static DataTransferModeType MapString2DataTransferMode(string text)
		{
			if (text == Resources.DataTransferModeRecord)
			{
				return DataTransferModeType.RecordWhileDataTransfer;
			}
			return DataTransferModeType.StopWhileDataTransfer;
		}

		public static string MapPartialDownloadType2String(PartialDownloadType type)
		{
			if (type == PartialDownloadType.PartialDownloadOff)
			{
				return Resources.Off;
			}
			if (type == PartialDownloadType.PartialDownloadOn)
			{
				return Resources.On;
			}
			if (type == PartialDownloadType.PartialDownloadOnInSameFolder)
			{
				return Resources.OnInSameFolder;
			}
			return "";
		}

		public static PartialDownloadType MapString2PartialDownloadType(string text)
		{
			foreach (PartialDownloadType partialDownloadType in Enum.GetValues(typeof(PartialDownloadType)))
			{
				if (GUIUtil.MapPartialDownloadType2String(partialDownloadType) == text)
				{
					return partialDownloadType;
				}
			}
			return PartialDownloadType.PartialDownloadOff;
		}

		public static string MapMemoryNumber2String(uint memoryNumber)
		{
			return string.Format(Resources.MemoryName, memoryNumber);
		}

		public static uint MapMemoryString2Number(string memoryString)
		{
			uint result;
			if (memoryString.IndexOf(Resources.Memory) == 0 && memoryString.Length > Resources.Memory.Length && uint.TryParse(memoryString.Substring(Resources.Memory.Length), out result))
			{
				return result;
			}
			return 0u;
		}

		public static uint MapMemoriesStringToValue(string memoriesString)
		{
			uint result = 0u;
			if (memoriesString.IndexOf(Resources.AllMemories) == 0)
			{
				result = 2147483647u;
			}
			else if (memoriesString.IndexOf(Resources.Memory) == 0 && memoriesString.Length > Resources.Memory.Length && !uint.TryParse(memoriesString.Substring(Resources.Memory.Length), out result))
			{
				result = 0u;
			}
			return result;
		}

		public static string MapValueToMemoriesString(uint value)
		{
			if (value == 2147483647u)
			{
				return Resources.AllMemories;
			}
			return string.Format(Resources.MemoryName, value);
		}

		public static uint MapMemoriesLogicalStringToValue(string memoriesString)
		{
			uint result = 0u;
			if (memoriesString.IndexOf(Resources.AllMemoriesAND) == 0)
			{
				result = 2147483647u;
			}
			else if (memoriesString.IndexOf(Resources.AllMemoriesOR) == 0)
			{
				result = 2147483646u;
			}
			else if (memoriesString.IndexOf(Resources.Memory) == 0 && memoriesString.Length > Resources.Memory.Length && !uint.TryParse(memoriesString.Substring(Resources.Memory.Length), out result))
			{
				result = 0u;
			}
			return result;
		}

		public static string MapValueToMemoriesLogicalString(uint value)
		{
			if (value == 2147483647u)
			{
				return Resources.AllMemoriesAND;
			}
			if (value == 2147483646u)
			{
				return Resources.AllMemoriesOR;
			}
			return string.Format(Resources.MemoryName, value);
		}

		public static string MapFileSizeNumber2String(long size)
		{
			CultureInfo provider = new CultureInfo("en-us");
			if (size > 1048576L)
			{
				double num = (double)size / 1048576.0;
				return string.Format(Resources.FileSizeInMB, num.ToString("F02", provider));
			}
			if (size > 1024L)
			{
				double num2 = (double)size / 1024.0;
				return string.Format(Resources.FileSizeInKB, num2.ToString("F02", provider));
			}
			return string.Format(Resources.FileSizeInB, size);
		}

		public static bool HasFileExtension(string filename, string extension)
		{
			if (string.IsNullOrEmpty(filename) || string.IsNullOrEmpty(extension))
			{
				return false;
			}
			int num = filename.LastIndexOf('.');
			return num >= 0 && num != filename.Length - 1 && filename.Substring(num + 1).ToLower() == extension.ToLower();
		}

		public static string ConvertDateTimeToFolderName(DateTime timestamp)
		{
			string format = "yyyy-MM-dd_HH-mm-ss";
			return timestamp.ToString(format);
		}

		public static string FilePathToShortendDisplayPath(string filepath, Font proposedFont, Size proposedSize)
		{
			if (string.IsNullOrEmpty(filepath))
			{
				return string.Empty;
			}
			string text = string.Copy(filepath);
			TextRenderer.MeasureText(text, proposedFont, proposedSize, TextFormatFlags.ModifyString | TextFormatFlags.PathEllipsis);
			int num = text.IndexOf('\0');
			if (num != -1)
			{
				text = text.Substring(0, num);
			}
			return text;
		}

		public static bool FileAccessible(string path)
		{
			try
			{
				File.GetAccessControl(path);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static bool IsZipFile(string path)
		{
			return ZipFile.IsZipFile(path);
		}

		public static bool IsAbsPathWithinZipFile(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return false;
			}
			if (!Path.IsPathRooted(path))
			{
				return false;
			}
			string text = string.Empty;
			bool result;
			try
			{
				text = Path.GetDirectoryName(path);
				while (!string.IsNullOrEmpty(text))
				{
					if (GUIUtil.IsZipFile(text))
					{
						result = true;
						return result;
					}
					text = Path.GetDirectoryName(text);
				}
				result = false;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public static void ExtractIfRequiredAndLaunchFile(string absFilePath)
		{
			bool flag = GUIUtil.IsAbsPathWithinZipFile(absFilePath);
			if (!flag && File.Exists(absFilePath))
			{
				FileSystemServices.LaunchFile(absFilePath);
				return;
			}
			if (flag && FileProxy.Exists(absFilePath))
			{
				using (BrowseFolderDialog browseFolderDialog = new BrowseFolderDialog())
				{
					browseFolderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
					browseFolderDialog.Title = Resources.TitleSelectFolderForExtraction;
					if (browseFolderDialog.ShowDialog() == DialogResult.OK && Directory.Exists(browseFolderDialog.SelectedPath))
					{
						string text = Path.Combine(browseFolderDialog.SelectedPath, Path.GetFileName(absFilePath));
						if (File.Exists(text))
						{
							InformMessageBox.Error(Resources.ErrorFileWithSameNameExistsInSelDir);
						}
						else
						{
							FileProxy.Copy(absFilePath, text);
							FileSystemServices.LaunchFile(text);
						}
					}
				}
				return;
			}
			InformMessageBox.Error(Resources.ErrorFileNotFound);
		}

		public static bool FolderAccessible(string path)
		{
			try
			{
				DirectoryProxy.GetAccessControl(path);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static IList<uint> GetMemoryCardSizes()
		{
			if (GUIUtil._memoryCardSizes == null)
			{
				GUIUtil._memoryCardSizes = new List<uint>();
				GUIUtil._memoryCardSizes.Add(512u);
				GUIUtil._memoryCardSizes.Add(1024u);
				GUIUtil._memoryCardSizes.Add(2048u);
				GUIUtil._memoryCardSizes.Add(4096u);
				GUIUtil._memoryCardSizes.Add(8192u);
				GUIUtil._memoryCardSizes.Add(16384u);
				GUIUtil._memoryCardSizes.Add(32768u);
				GUIUtil._memoryCardSizes.Add(65536u);
			}
			return GUIUtil._memoryCardSizes;
		}

		public static string MapMemoryCardSize2String(uint cardSize)
		{
			if (cardSize < 1000u)
			{
				return string.Format(Resources.SizeInMB, cardSize);
			}
			return string.Format(Resources.SizeInGB, cardSize / 1024u);
		}

		public static uint MapString2MemoryCardSize(string cardSizeString)
		{
			IList<uint> memoryCardSizes = GUIUtil.GetMemoryCardSizes();
			foreach (uint current in memoryCardSizes)
			{
				if (cardSizeString == GUIUtil.MapMemoryCardSize2String(current))
				{
					return current;
				}
			}
			return 0u;
		}

		public static IList<uint> GetRingBufferSizes()
		{
			if (GUIUtil._ringBufferSizes == null)
			{
				GUIUtil._ringBufferSizes = new List<uint>();
				GUIUtil._ringBufferSizes.Add(1u);
				GUIUtil._ringBufferSizes.Add(2u);
				GUIUtil._ringBufferSizes.Add(5u);
				GUIUtil._ringBufferSizes.Add(10u);
				GUIUtil._ringBufferSizes.Add(20u);
				GUIUtil._ringBufferSizes.Add(50u);
				GUIUtil._ringBufferSizes.Add(100u);
				GUIUtil._ringBufferSizes.Add(200u);
				GUIUtil._ringBufferSizes.Add(500u);
				GUIUtil._ringBufferSizes.Add(1000u);
				GUIUtil._ringBufferSizes.Add(2048u);
			}
			return GUIUtil._ringBufferSizes;
		}

		public static uint GetActualMaxRingBufferSizeSDCard(uint nominalCardSize)
		{
			uint result = 117u;
			if (nominalCardSize >= 256u)
			{
				result = 238u;
			}
			if (nominalCardSize >= 512u)
			{
				result = 486u;
			}
			if (nominalCardSize >= 1024u)
			{
				result = 965u;
			}
			if (nominalCardSize >= 2048u)
			{
				result = 1959u;
			}
			if (nominalCardSize >= 4096u)
			{
				result = 3898u;
			}
			if (nominalCardSize >= 8192u)
			{
				result = 7809u;
			}
			if (nominalCardSize >= 16384u)
			{
				result = 15193u;
			}
			if (nominalCardSize >= 32768u)
			{
				result = 30433u;
			}
			if (nominalCardSize >= 65536u)
			{
				result = 60866u;
			}
			return result;
		}

		public static uint GetActualMaxRingBufferSizeCFCard(uint nominalCardSize)
		{
			uint result = 117u;
			if (nominalCardSize >= 256u)
			{
				result = 238u;
			}
			if (nominalCardSize >= 512u)
			{
				result = 486u;
			}
			if (nominalCardSize >= 1024u)
			{
				result = 965u;
			}
			if (nominalCardSize >= 2048u)
			{
				result = 1959u;
			}
			if (nominalCardSize >= 4096u)
			{
				result = 3898u;
			}
			if (nominalCardSize >= 8192u)
			{
				result = 7911u;
			}
			if (nominalCardSize >= 16384u)
			{
				result = 15193u;
			}
			if (nominalCardSize >= 32768u)
			{
				result = 30433u;
			}
			if (nominalCardSize >= 65536u)
			{
				result = 60866u;
			}
			return result;
		}

		public static IList<uint> GetMaxLogFileSizes()
		{
			if (GUIUtil._maxLogFileSizes == null)
			{
				GUIUtil._maxLogFileSizes = new List<uint>();
				GUIUtil._maxLogFileSizes.Add(1u);
				GUIUtil._maxLogFileSizes.Add(2u);
				GUIUtil._maxLogFileSizes.Add(5u);
				GUIUtil._maxLogFileSizes.Add(10u);
				GUIUtil._maxLogFileSizes.Add(20u);
				GUIUtil._maxLogFileSizes.Add(50u);
				GUIUtil._maxLogFileSizes.Add(100u);
				GUIUtil._maxLogFileSizes.Add(250u);
				GUIUtil._maxLogFileSizes.Add(500u);
				GUIUtil._maxLogFileSizes.Add(1024u);
			}
			return GUIUtil._maxLogFileSizes;
		}

		public static string MapRingBufferOperatingMode2String(RingBufferOperatingMode operatingMode)
		{
			switch (operatingMode)
			{
			case RingBufferOperatingMode.overwriteOldest:
				return Resources.RingBufferOpModeOverwriteOldest;
			case RingBufferOperatingMode.stopLogging:
				return Resources.RingBufferOpModeStopLogging;
			default:
				return "";
			}
		}

		public static RingBufferOperatingMode MapString2RingBufferOperatingMode(string myOperatingModeString)
		{
			foreach (RingBufferOperatingMode ringBufferOperatingMode in Enum.GetValues(typeof(RingBufferOperatingMode)))
			{
				if (GUIUtil.MapRingBufferOperatingMode2String(ringBufferOperatingMode) == myOperatingModeString)
				{
					return ringBufferOperatingMode;
				}
			}
			return RingBufferOperatingMode.overwriteOldest;
		}

		public static string MapLogDataMemoryType2String(LogDataMemoryType memoryType)
		{
			switch (memoryType)
			{
			case LogDataMemoryType.SDCard:
				return Resources.LogDataMemoryTypeSDCard;
			case LogDataMemoryType.CFCard:
				return Resources.LogDataMemoryTypeCFCard;
			case LogDataMemoryType.CFCardUSB:
				return Resources.LogDataMemoryTypeCFCardUSB;
			case LogDataMemoryType.HardDisk:
				return Resources.LogDataMemoryTypeHardDisk;
			case LogDataMemoryType.USBDevice:
				return Resources.LogDataMemoryTypeUSB;
			default:
				return "";
			}
		}

		public static LogDataMemoryType MapString2LogDataMemoryType(string myMemoryTypeString)
		{
			foreach (LogDataMemoryType logDataMemoryType in Enum.GetValues(typeof(LogDataMemoryType)))
			{
				string a = GUIUtil.MapLogDataMemoryType2String(logDataMemoryType);
				if (a == myMemoryTypeString)
				{
					return logDataMemoryType;
				}
			}
			return LogDataMemoryType.SDCard;
		}

		public static string MapLEDState2String(LEDState ledState)
		{
			switch (ledState)
			{
			case LEDState.Disabled:
				return Resources.LEDStateDisabled;
			case LEDState.AlwaysOn:
				return Resources.LEDStateAlwaysOn;
			case LEDState.AlwaysBlinking:
				return Resources.LEDStateAlwaysBlinking;
			case LEDState.TriggerActive:
				return Resources_Trigger.LEDStateTriggerActive;
			case LEDState.LoggingActive:
				return Resources.LEDStateLoggingActive;
			case LEDState.EndOfMeasurement:
				return Resources.LEDStateEndOfMeasurement;
			case LEDState.MemoryFull:
				return Resources.LEDStateMemoryFull;
			case LEDState.CANLINError:
				return Resources.LEDStateCANLINError;
			case LEDState.CANError:
				return Resources.LEDStateCANError;
			case LEDState.LINError:
				return Resources.LEDStateLINError;
			case LEDState.CANoeMeasurementOn:
				return Resources.LEDStateCANoeMeasureOn;
			case LEDState.MOSTLock:
				return Resources.LEDStateMOSTLock;
			case LEDState.CcpXcpError:
				return Resources_CcpXcp.LEDStateXcpError;
			case LEDState.CcpXcpOk:
				return Vocabulary.LEDStateXcpOk;
			default:
				return "";
			}
		}

		public static LEDState MapString2LEDState(string myLEDStateString)
		{
			foreach (LEDState lEDState in Enum.GetValues(typeof(LEDState)))
			{
				if (GUIUtil.MapLEDState2String(lEDState) == myLEDStateString)
				{
					return lEDState;
				}
			}
			return LEDState.Disabled;
		}

		public static string NumberToDisplayString(uint number)
		{
			if (GUIUtil._isHexadecimal)
			{
				return number.ToString("X");
			}
			return number.ToString("D");
		}

		public static bool DisplayStringToNumber(string numberString, out uint number)
		{
			uint num = 0u;
			if (GUIUtil._isHexadecimal)
			{
				if (numberString.ToUpper().StartsWith("0X"))
				{
					numberString = numberString.Substring(2);
				}
				if (uint.TryParse(numberString, NumberStyles.HexNumber, null, out num))
				{
					number = num;
					return true;
				}
				number = 0u;
				return false;
			}
			else
			{
				if (uint.TryParse(numberString, NumberStyles.Integer, null, out num))
				{
					number = num;
					return true;
				}
				number = 0u;
				return false;
			}
		}

		public static string NumberToDisplayString(int number)
		{
			if (GUIUtil._isHexadecimal)
			{
				return number.ToString("X");
			}
			return number.ToString("D");
		}

		public static bool DisplayStringToNumber(string numberString, out int number)
		{
			if (GUIUtil._isHexadecimal)
			{
				if (numberString.ToUpper().StartsWith("0X"))
				{
					numberString = numberString.Substring(2);
				}
				int num;
				if (int.TryParse(numberString, NumberStyles.HexNumber, null, out num))
				{
					number = num;
					return true;
				}
				number = 0;
				return false;
			}
			else
			{
				int num;
				if (int.TryParse(numberString, NumberStyles.Integer, null, out num))
				{
					number = num;
					return true;
				}
				number = 0;
				return false;
			}
		}

		public static bool DisplayStringHexToNumber(string numberString, uint maxNumberOfBytes, out ulong numberValue, out bool isLengthExceeded)
		{
			numberValue = 0uL;
			isLengthExceeded = false;
			if (maxNumberOfBytes > 8u || maxNumberOfBytes == 0u)
			{
				maxNumberOfBytes = 8u;
			}
			numberString = numberString.Trim();
			if (numberString.ToUpper().StartsWith("0X"))
			{
				numberString = numberString.Substring(2);
			}
			if ((long)numberString.Length > (long)((ulong)(maxNumberOfBytes * 2u)))
			{
				isLengthExceeded = true;
				return false;
			}
			return ulong.TryParse(numberString, NumberStyles.HexNumber, null, out numberValue);
		}

		public static string NumberToDisplayStringHex(ulong numberValue, uint maxNumberOfBytes)
		{
			byte[] bytes = BitConverter.GetBytes(numberValue);
			StringBuilder stringBuilder = new StringBuilder();
			int num = bytes.Length - 1;
			if (maxNumberOfBytes > 0u)
			{
				num = Math.Min((int)(maxNumberOfBytes - 1u), 7);
			}
			for (int i = num; i >= 0; i--)
			{
				if (i <= 0 || bytes[i] != 0 || stringBuilder.Length != 0)
				{
					stringBuilder.AppendFormat("{0:X2}", bytes[i]);
				}
			}
			return stringBuilder.ToString().Trim();
		}

		public static bool DisplayStringHexToByteArray(string numberString, out byte[] byteArray)
		{
			byteArray = null;
			if (numberString.ToUpper().StartsWith("0X"))
			{
				numberString = numberString.Substring(2);
			}
			numberString = numberString.Trim();
			if (numberString.Length % 2 != 0)
			{
				return false;
			}
			for (int i = 0; i < numberString.Length; i++)
			{
				if (GUIUtil._HexDigits.IndexOf(numberString[i]) < 0)
				{
					return false;
				}
			}
			byteArray = new byte[numberString.Length / 2];
			for (int j = 0; j < byteArray.Length; j++)
			{
				string s = numberString.Substring(j * 2, 2);
				byteArray[j] = byte.Parse(s, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
			}
			return true;
		}

		public static string ByteArrayToDisplayStringHex(byte[] byteArray)
		{
			if (byteArray == null)
			{
				return string.Empty;
			}
			return BitConverter.ToString(byteArray).Replace("-", "");
		}

		public static bool DisplayStringToFloatNumber(string numberString, out double number)
		{
			number = 0.0;
			return double.TryParse(numberString, NumberStyles.Float, CultureInfo.CurrentCulture, out number) || double.TryParse(numberString, NumberStyles.Float, CultureInfo.InvariantCulture, out number);
		}

		public static bool DisplayStringToPhysicalValue(string numberString, out double number)
		{
			number = 0.0;
			return double.TryParse(numberString, NumberStyles.Float, CultureInfo.CurrentCulture, out number) || double.TryParse(numberString, NumberStyles.Float, CultureInfo.InvariantCulture, out number);
		}

		private static double GetMaximumValueForBitWidth(uint numberOfBits)
		{
			if (numberOfBits > 32u)
			{
				numberOfBits = 32u;
			}
			return Math.Pow(2.0, numberOfBits) - 1.0;
		}

		private static int GetHighest1Bit(uint number)
		{
			int result = 0;
			for (int i = 0; i < 32; i++)
			{
				uint num = number >> i;
				if ((num & 1u) == 1u)
				{
					result = i;
				}
			}
			return result;
		}

		public static bool DisplayStringToRawSignalValue(string valueString, SignalDefinition signalDefinition, out double rawSignalValue, out bool isOutOfRange)
		{
			isOutOfRange = false;
			rawSignalValue = 0.0;
			if (!GUIUtil._isHexadecimal)
			{
				if (signalDefinition.IsSigned)
				{
					int num;
					if (!int.TryParse(valueString, NumberStyles.Integer, null, out num))
					{
						return false;
					}
					uint num2 = (uint)GUIUtil.GetMaximumValueForBitWidth(signalDefinition.Length - 1u);
					int num3 = (int)(num2 * 4294967295u - 1u);
					int num4 = (int)num2;
					if (num > num4 || num < num3)
					{
						isOutOfRange = true;
						return false;
					}
					rawSignalValue = (double)num;
				}
				else
				{
					uint num5;
					if (!uint.TryParse(valueString, NumberStyles.Integer, null, out num5))
					{
						return false;
					}
					uint num6 = (uint)GUIUtil.GetMaximumValueForBitWidth(signalDefinition.Length);
					if (num5 > num6)
					{
						isOutOfRange = true;
						return false;
					}
					rawSignalValue = num5;
				}
				return true;
			}
			if (valueString.ToUpper().StartsWith("0X"))
			{
				valueString = valueString.Substring(2);
			}
			uint num7 = 0u;
			if (!uint.TryParse(valueString, NumberStyles.HexNumber, null, out num7))
			{
				return false;
			}
			if (num7 > (uint)GUIUtil.GetMaximumValueForBitWidth(signalDefinition.Length))
			{
				isOutOfRange = true;
				return false;
			}
			if (signalDefinition.IsSigned)
			{
				int highest1Bit = GUIUtil.GetHighest1Bit(num7);
				if (highest1Bit > 0 && (long)highest1Bit == (long)((ulong)(signalDefinition.Length - 1u)))
				{
					uint num8 = (uint)GUIUtil.GetMaximumValueForBitWidth((uint)highest1Bit);
					uint num9 = num8 - (num7 & num8) + 1u;
					rawSignalValue = num9 * -1.0;
				}
				else
				{
					rawSignalValue = num7;
				}
			}
			else
			{
				rawSignalValue = num7;
			}
			return true;
		}

		public static string RawSignalValueToDisplayString(double rawSignalValue, SignalDefinition signalDefintion)
		{
			string result;
			if (GUIUtil._isHexadecimal)
			{
				if (signalDefintion.IsSigned)
				{
					uint num = (uint)Math.Abs(rawSignalValue);
					uint num2 = num;
					if (rawSignalValue < 0.0)
					{
						num2 = (uint)GUIUtil.GetMaximumValueForBitWidth(signalDefintion.Length) - (num - 1u);
					}
					result = num2.ToString("X");
				}
				else
				{
					result = ((uint)rawSignalValue).ToString("X");
				}
			}
			else if (signalDefintion.IsSigned)
			{
				result = ((int)rawSignalValue).ToString("D");
			}
			else
			{
				result = ((uint)rawSignalValue).ToString("D");
			}
			return result;
		}

		public static bool IsRawSignalWithinDatabytes(uint startBit, uint signalLength, bool isMotorola, uint databytesLength)
		{
			if (!isMotorola)
			{
				if (startBit + signalLength > databytesLength * 8u)
				{
					return false;
				}
			}
			else
			{
				uint num = signalLength;
				uint num2 = startBit / 8u;
				uint num3 = startBit % 8u;
				while (num > 0u && num2 < databytesLength)
				{
					uint num4;
					if (num3 < num)
					{
						num4 = 0u;
					}
					else
					{
						num4 = num3 - num + 1u;
					}
					num -= num3 + 1u - num4;
					num2 += 1u;
					num3 = 7u;
				}
				if (num > 0u || num2 > databytesLength)
				{
					return false;
				}
			}
			return true;
		}

		public static string CANIdToDisplayString(uint canId, bool isExtended)
		{
			if (isExtended)
			{
				return string.Format("{0}x", GUIUtil.NumberToDisplayString(canId));
			}
			return GUIUtil.NumberToDisplayString(canId);
		}

		public static string CANDbCANIdToDisplayString(uint canDbCANId)
		{
			bool flag = (canDbCANId & Constants.CANDbIsExtendedIdMask) == Constants.CANDbIsExtendedIdMask;
			if (flag)
			{
				return string.Format("{0}x", GUIUtil.NumberToDisplayString(canDbCANId & ~Constants.CANDbIsExtendedIdMask));
			}
			return GUIUtil.NumberToDisplayString(canDbCANId);
		}

		public static bool DisplayStringToCANId(string canIdString, out uint canId, out bool isExtended)
		{
			isExtended = false;
			if (string.IsNullOrEmpty(canIdString))
			{
				canId = 0u;
				return false;
			}
			string text = canIdString.Substring(canIdString.Length - 1);
			if ("x" == text.ToLower())
			{
				isExtended = true;
				if (GUIUtil.DisplayStringToNumber(canIdString.Substring(0, canIdString.Length - 1), out canId) && canId <= Constants.MaximumExtendedCANId)
				{
					return true;
				}
			}
			else if (GUIUtil.DisplayStringToNumber(canIdString, out canId) && canId <= Constants.MaximumStandardCANId)
			{
				return true;
			}
			return false;
		}

		public static string LINIdToDisplayString(uint linId)
		{
			return GUIUtil.NumberToDisplayString(linId);
		}

		public static bool DisplayStringToLINId(string linIdString, out uint linId)
		{
			if (GUIUtil.DisplayStringToNumber(linIdString, out linId) && linId <= Constants.MaximumLINId)
			{
				return true;
			}
			linId = 0u;
			return false;
		}

		public static string FlexraySlotIdToDisplayString(uint slotId)
		{
			return slotId.ToString();
		}

		public static bool DisplayStringToFlexraySlotId(string flexrayIdString, out uint slotId)
		{
			if (uint.TryParse(flexrayIdString, out slotId) && slotId >= Constants.MinimumFlexraySlotId && slotId <= Constants.MaximumFlexraySlotId)
			{
				return true;
			}
			slotId = 0u;
			return false;
		}

		public static IList<uint> GetFlexrayCycleRepetitionValues(bool showOnlyFirstAndLastValues)
		{
			List<uint> list = new List<uint>();
			list.Add(1u);
			if (!showOnlyFirstAndLastValues)
			{
				list.Add(2u);
				list.Add(4u);
				list.Add(8u);
				list.Add(16u);
				list.Add(32u);
			}
			list.Add(64u);
			return list;
		}

		public static string GetSizeStringMBForBytes(long sizeInBytes)
		{
			return string.Format(Resources.SizeInMB, sizeInBytes / 1048576L);
		}

		public static string GetSizeStringKBForBytes(long sizeInBytes)
		{
			return string.Format(Resources.SizeInKB, sizeInBytes / 1024L);
		}

		public static string GetPercentageStringForFreeSpace(long totalSpace, long freeSpace)
		{
			if (totalSpace <= 0L)
			{
				return string.Empty;
			}
			double num = (double)freeSpace / (double)totalSpace * 100.0;
			num = Math.Floor(num * 10.0) / 10.0;
			return string.Format("({0:0.0}%)", num);
		}

		public static long GetSizeMBInBytes(long sizeInMBs)
		{
			return sizeInMBs * 1024L * 1024L;
		}

		public static bool TryConvertSizeToKBytes(double sizeInMBytes, out uint sizeInKBytes)
		{
			sizeInKBytes = 0u;
			try
			{
				sizeInKBytes = Convert.ToUInt32(Math.Round(sizeInMBytes * 1024.0));
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static double RoundSize(double size, uint numOfDecimalPlaces)
		{
			return Math.Round(size * Math.Pow(10.0, numOfDecimalPlaces), MidpointRounding.AwayFromZero) / Math.Pow(10.0, numOfDecimalPlaces);
		}

		public static double ConvertSizeToMBytes(uint sizeInKBytes, uint numOfDecimalPlaces)
		{
			double size = Convert.ToDouble(sizeInKBytes) / 1024.0;
			return GUIUtil.RoundSize(size, numOfDecimalPlaces);
		}

		public static bool IsInteger(double value)
		{
			return Math.Ceiling(value) == Math.Floor(value);
		}

		public static string GetFractionedMByteDisplayString(uint sizeInKBytes)
		{
			uint num = 2u;
			double value = GUIUtil.ConvertSizeToMBytes(sizeInKBytes, num);
			if (GUIUtil.IsInteger(value))
			{
				return Convert.ToUInt32(value).ToString();
			}
			return value.ToString(string.Format("F{0}", num), CultureInfo.InvariantCulture.NumberFormat);
		}

		public static void ReportFileGenerationResult(EnumInfoType infoType, string errorText, string compilerErrFilePath)
		{
			if (!string.IsNullOrEmpty(compilerErrFilePath) && File.Exists(compilerErrFilePath))
			{
				if (DialogResult.Yes == InformMessageBox.Show(infoType, EnumQuestionType.Question, errorText + " " + Resources.DoYouWantToOpenErrFile, new string[]
				{
					Resources.Error
				}))
				{
					FileSystemServices.LaunchFile(compilerErrFilePath);
					return;
				}
			}
			else
			{
				InformMessageBox.Error(errorText);
			}
		}

		public static string GetCommaSeparatedQuotedCharList(string charStr)
		{
			if (charStr.Length < 1)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("'{0}'", charStr[0]);
			for (int i = 1; i < charStr.Length; i++)
			{
				stringBuilder.AppendFormat(", '{0}'", charStr[i]);
			}
			return stringBuilder.ToString();
		}

		public static string GetBlankSeparatedCharList(string charStrWithOptionalSpace)
		{
			if (charStrWithOptionalSpace.Length < 1)
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			int num = charStrWithOptionalSpace.IndexOf(' ');
			if (num >= 0)
			{
				flag = true;
				charStrWithOptionalSpace = charStrWithOptionalSpace.Remove(num, 1);
			}
			stringBuilder.AppendFormat("{0}", charStrWithOptionalSpace[0]);
			for (int i = 1; i < charStrWithOptionalSpace.Length; i++)
			{
				stringBuilder.AppendFormat(" {0}", charStrWithOptionalSpace[i]);
			}
			if (flag)
			{
				stringBuilder.AppendFormat(" {0}", Resources.AndSpaceChar);
			}
			return stringBuilder.ToString();
		}

		public static int GetHelpIdForPropertyPage(PageType page)
		{
			switch (page)
			{
			case PageType.DeviceInformation:
				return GUIUtil.HelpPageID_DeviceInformation;
			case PageType.HardwareSettings:
				return GUIUtil.HelpPageID_HardwareSettings;
			case PageType.CANChannels:
				return GUIUtil.HelpPageID_CANChannels;
			case PageType.LINChannels:
				return GUIUtil.HelpPageID_LINChannels;
			case PageType.FlexrayChannels:
				return GUIUtil.HelpPageID_FlexRayChannels;
			case PageType.MOST150Channels:
				return GUIUtil.HelpPageID_MOST150Channels;
			case PageType.MultibusChannels:
				return GUIUtil.HelpPageID_MultibusChannels;
			case PageType.AnalogInputs:
				return GUIUtil.HelpPageID_AnalogInputs;
			case PageType.DigitalInputs:
				return GUIUtil.HelpPageID_DigitalInputs;
			case PageType.CombinedAnalogDigitalInputs:
				return GUIUtil.HelpPageID_CombinedAnalogDigitalInputs;
			case PageType.LEDs:
				return GUIUtil.HelpPageID_LEDs;
			case PageType.Databases:
				return GUIUtil.HelpPageID_Databases;
			case PageType.ExportDatabases:
				return GUIUtil.HelpPageID_ExportDatabases;
			case PageType.Filters1:
			case PageType.Filters2:
				return GUIUtil.HelpPageID_Filters;
			case PageType.Triggers1:
			case PageType.Triggers2:
				return GUIUtil.HelpPageID_Triggers;
			case PageType.SpecialFeatures:
				return GUIUtil.HelpPageID_SpecialFeatures;
			case PageType.IncludeFiles:
				return GUIUtil.HelpPageID_IncludeFiles;
			case PageType.WLANSettings:
				return GUIUtil.HelpPageID_WLANSettings;
			case PageType.LoggerDevice:
				return GUIUtil.HelpPageID_LoggerDevice;
			case PageType.LoggerDeviceNavigator:
				return GUIUtil.HelpPageID_LoggerDeviceNavigator;
			case PageType.Comment:
				return GUIUtil.HelpPageID_Comment;
			case PageType.CardReader:
				return GUIUtil.HelpPageID_CardReader;
			case PageType.CardReaderNavigator:
				return GUIUtil.HelpPageID_CardReaderNavigator;
			case PageType.CLFExport:
				return GUIUtil.HelpPageID_CLFExport;
			case PageType.InterfaceMode:
				return GUIUtil.HelpPageID_InterfaceMode;
			case PageType.DiagnosticsDatabases:
				return GUIUtil.HelpPageID_DiagnosticsDatabases;
			case PageType.DiagnosticActions:
				return GUIUtil.HelpPageID_DiagnosticsActions;
			case PageType.DigitalOutputs:
				return GUIUtil.HelpPageID_DigitalOutputs;
			case PageType.SendMessage:
				return GUIUtil.HelpPageID_SendMessage;
			case PageType.GPS:
				return GUIUtil.HelpPageID_GPS;
			case PageType.CANgps:
				return GUIUtil.HelpPageID_CANgps;
			case PageType.CcpXcpDescriptions:
				return GUIUtil.HelpPageID_CCPXCP;
			case PageType.CcpXcpSignalRequests:
				return GUIUtil.HelpPageID_CcpXcpSignalRequests;
			}
			return 0;
		}

		public static int GetHelpIdForFormType(Form form)
		{
			if (form is CardReaderDriveSelection)
			{
				return 0;
			}
			if (form is DisplayReport)
			{
				return 0;
			}
			if (form is SetEthernetAddress)
			{
				return GUIUtil.HelpPageID_EthernetAddress;
			}
			if (form is SetRealtimeClock)
			{
				return GUIUtil.HelpPageID_RealtimeClock;
			}
			if (form is Vector.VLConfig.GUI.FiltersPage.CANIdCondition)
			{
				return GUIUtil.HelpPageID_Filters;
			}
			if (form is Vector.VLConfig.GUI.FiltersPage.LINIdCondition)
			{
				return GUIUtil.HelpPageID_Filters;
			}
			if (form is Vector.VLConfig.GUI.FiltersPage.FlexrayIdCondition)
			{
				return GUIUtil.HelpPageID_Filters;
			}
			if (form is SelectColumnInCSVFile)
			{
				return GUIUtil.HelpPageID_Filters;
			}
			if (form is SymbolicFlexrayCondition)
			{
				return GUIUtil.HelpPageID_Filters;
			}
			if (form is SetLimitRate)
			{
				return GUIUtil.HelpPageID_Filters;
			}
			if (form is CANBusStatisticsCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is Vector.VLConfig.GUI.Common.EventConditions.CANIdCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is Vector.VLConfig.GUI.Common.EventConditions.LINIdCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is Vector.VLConfig.GUI.Common.EventConditions.FlexrayIdCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is SymbolicMessageCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is SymbolicSignalCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is CANDataCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is LINDataCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is MsgTimeoutCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is DigitalInputCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is AnalogInputCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is IgnitionCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is VoCanRecordingCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is KeyCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is CommParameters)
			{
				return GUIUtil.HelpPageID_DiagCommParameters;
			}
			if (form is SelectECUs)
			{
				return GUIUtil.HelpPageID_DiagECUSelection;
			}
			if (form is NetworkSelection)
			{
				return GUIUtil.HelpPageID_Databases;
			}
			if (form is CyclicTimerCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is OnStartCondition)
			{
				return GUIUtil.HelpPageID_EventsConditions;
			}
			if (form is ConfigureSendMessage)
			{
				return GUIUtil.HelpPageID_SendMessageDialog;
			}
			if (form is SetVehicleName)
			{
				return GUIUtil.HelpPageID_VehicleName;
			}
			if (form is ClockTimedCondition)
			{
				return GUIUtil.HelpPageID_WLANSettings;
			}
			if (form is DataSelection)
			{
				return GUIUtil.HelpPageID_WLANSettings;
			}
			if (form is BufferSizeCalculatorForm)
			{
				return GUIUtil.HelpPageID_BufferSizeCalculator;
			}
			if (form is CardSizeNumOfFilesCalculator)
			{
				return GUIUtil.HelpPageID_Triggers;
			}
			if (form is ProtocolSettingsDialog)
			{
				return GUIUtil.HelpPageID_CcpXcpProtocolSettings;
			}
			if (form is ProtocolSelectionDialog)
			{
				return GUIUtil.HelpPageID_CcpXcpProtocolSelection;
			}
			if (form is CcpXcpSymbolSelection)
			{
				return GUIUtil.HelpPageID_CcpXcpSymbolSelection;
			}
			if (form is DiagnosticSignalSelection)
			{
				return GUIUtil.HelpPageID_DiagnosticsSignalSelectionDialog;
			}
			if (form is CcpXcpSignalListSelection)
			{
				return GUIUtil.HelpPageID_CcpXcpSignalActions;
			}
			return 0;
		}

		public static bool AdjustScreenPositionToVisibleArea(ref int top, ref int left, ref int height, ref int width)
		{
			Rectangle rect = new Rectangle(left, top, width, height);
			Rectangle workingArea = Screen.GetWorkingArea(rect);
			if (workingArea.Left <= left && workingArea.Top <= top && workingArea.Left + workingArea.Width >= left + width && workingArea.Top + workingArea.Height >= top + height)
			{
				return false;
			}
			int num = width / 4;
			int num2 = height / 4;
			bool result = false;
			if (left < workingArea.Left && width - (workingArea.Left - left) < num)
			{
				left = workingArea.Left + workingArea.Width / 2 - width;
				result = true;
			}
			else if (left + width > workingArea.Left + workingArea.Width && width - (left + width - (workingArea.Left + workingArea.Width)) < num)
			{
				left = workingArea.Left + workingArea.Width / 2;
				result = true;
			}
			if (top < workingArea.Top && height - (workingArea.Top - top) < num2)
			{
				top = workingArea.Top + workingArea.Height / 2 - height;
				result = true;
			}
			else if (top + height > workingArea.Top + workingArea.Height && height - (top + height - (workingArea.Top + workingArea.Height)) < num2)
			{
				top = workingArea.Top + workingArea.Height / 2;
				result = true;
			}
			return result;
		}

		public static GUIUtil.FileDropContent GetKindOfDrop(DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (array.Count<string>() == 1)
				{
					if (DirectoryProxy.Exists(array[0]))
					{
						return GUIUtil.FileDropContent.Folder;
					}
					if (FileProxy.Exists(array[0]))
					{
						return GUIUtil.FileDropContent.File;
					}
				}
				else if (array.Count<string>() > 1)
				{
					bool flag = false;
					bool flag2 = false;
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string path = array2[i];
						if (FileProxy.Exists(path))
						{
							flag = true;
						}
						if (DirectoryProxy.Exists(path))
						{
							flag2 = true;
						}
						if (flag && flag2)
						{
							return GUIUtil.FileDropContent.FilesAndFolders;
						}
					}
					if (flag)
					{
						return GUIUtil.FileDropContent.Files;
					}
					if (flag2)
					{
						return GUIUtil.FileDropContent.Folders;
					}
				}
			}
			return GUIUtil.FileDropContent.IllegalDrop;
		}

		public static bool ConvertYesNoString2Bool(string value, ref bool result)
		{
			if (value == Resources.Yes)
			{
				result = true;
				return true;
			}
			if (value == Resources.No)
			{
				result = false;
				return true;
			}
			return false;
		}

		public static string ConvertBool2YesNoString(bool value)
		{
			if (value)
			{
				return Resources.Yes;
			}
			return Resources.No;
		}

		public static bool ConvertOnOffString2Bool(string value, ref bool result)
		{
			if (value == Resources.On)
			{
				result = true;
				return true;
			}
			if (value == Resources.Off)
			{
				result = false;
				return true;
			}
			return false;
		}

		public static string ConvertBool2OnOffString(bool value)
		{
			if (value)
			{
				return Resources.On;
			}
			return Resources.Off;
		}

		public static bool ConvertActiveString2Bool(string value, ref bool result)
		{
			if (value == Resources.Activated)
			{
				result = true;
				return true;
			}
			if (value == Resources.Deactivated)
			{
				result = false;
				return true;
			}
			return false;
		}

		public static string ConvertBool2ActiveString(bool value)
		{
			if (value)
			{
				return Resources.Activated;
			}
			return Resources.Deactivated;
		}

		public static void InitGuiScaling(Form mainForm)
		{
			GUIUtil.sGuiScaleFactorX = 1.0;
			GUIUtil.sGuiScaleFactorY = 1.0;
			GUIUtil.sDPIScaleFactorX = 1.0;
			GUIUtil.sDPIScaleFactorY = 1.0;
			if (mainForm == null)
			{
				throw new ArgumentException("Form must be null");
			}
			if (mainForm.AutoScaleMode == AutoScaleMode.Inherit)
			{
				throw new ArgumentException("Form must have AutoScaleMode != Inherit");
			}
			if (mainForm.AutoScaleMode == AutoScaleMode.None)
			{
				return;
			}
			using (Graphics graphics = mainForm.CreateGraphics())
			{
				GUIUtil.sDPIScaleFactorX = (double)graphics.DpiX / 96.0;
				GUIUtil.sDPIScaleFactorY = (double)graphics.DpiY / 96.0;
			}
			ResourceManager resourceManager = new ResourceManager(typeof(MainWindow).FullName, Assembly.GetExecutingAssembly());
			object @object = resourceManager.GetObject("$this.AutoScaleDimensions");
			SizeF sizeF = new SizeF(0f, 0f);
			if (@object != null)
			{
				sizeF = (SizeF)@object;
			}
			if (!sizeF.IsEmpty)
			{
				GUIUtil.sGuiScaleFactorX = (double)(mainForm.AutoScaleDimensions.Width / sizeF.Width);
				GUIUtil.sGuiScaleFactorY = (double)(mainForm.AutoScaleDimensions.Height / sizeF.Height);
				return;
			}
			SizeF autoScaleDimensions;
			if (mainForm.AutoScaleMode == AutoScaleMode.Font)
			{
				autoScaleDimensions = new SizeF(6f, 13f);
			}
			else if (mainForm.AutoScaleMode == AutoScaleMode.Dpi)
			{
				autoScaleDimensions = new SizeF(96f, 96f);
			}
			else
			{
				autoScaleDimensions = mainForm.AutoScaleDimensions;
			}
			GUIUtil.sGuiScaleFactorX = (double)(mainForm.AutoScaleDimensions.Width / autoScaleDimensions.Width);
			GUIUtil.sGuiScaleFactorY = (double)(mainForm.AutoScaleDimensions.Height / autoScaleDimensions.Height);
		}

		public static int GuiScaleX(int originalX)
		{
			return (int)((double)originalX * GUIUtil.sGuiScaleFactorX);
		}

		public static int GuiScaleY(int originalY)
		{
			return (int)((double)originalY * GUIUtil.sGuiScaleFactorY);
		}

		public static Size GuiScaleSize(Size originalSize)
		{
			return new Size(GUIUtil.GuiScaleX(originalSize.Width), GUIUtil.GuiScaleY(originalSize.Height));
		}

		public static int GuiScaleX_DPI(int originalX)
		{
			return (int)((double)originalX * GUIUtil.sDPIScaleFactorX);
		}

		public static int GuiScaleY_DPI(int originalY)
		{
			return (int)((double)originalY * GUIUtil.sDPIScaleFactorY);
		}

		public static Size GuiScaleSize_DPI(Size originalSize)
		{
			return new Size(GUIUtil.GuiScaleX_DPI(originalSize.Width), GUIUtil.GuiScaleY_DPI(originalSize.Height));
		}

		public static void InitSplitButtonMenuWebDisplayExportSignalTypes(SplitButtonEx splitButtonEx)
		{
			foreach (string current in GUIUtil.WebDisplayExportSignalTypeNamesInMenu)
			{
				string groupName = string.Empty;
				object tag = null;
				if (current == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
				{
					groupName = "Signals";
				}
				else if (current == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
				{
					groupName = "Signals";
				}
				else if (current == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
				{
					groupName = "Signals";
				}
				else if (current == Resources.WebDisplayExportSignalTypeNameColLtlSystemInformation)
				{
					groupName = "Variables";
				}
				else if (current == Resources.WebDisplayExportSignalTypeNameColStatusVariable)
				{
					groupName = "Variables";
				}
				ToolStripItem toolStripItem = splitButtonEx.AddItem(current, GUIUtil.GetWebDisplayExportSignalTypeImage(current), groupName);
				toolStripItem.Tag = tag;
			}
		}

		private static Image GetWebDisplayExportSignalTypeImage(string expSigName)
		{
			MainImageList.IconIndex baseImgIndex = MainImageList.IconIndex.NoImage;
			if (expSigName == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
			{
				baseImgIndex = MainImageList.IconIndex.SymbolicSignalCAN;
			}
			else if (expSigName == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
			{
				baseImgIndex = MainImageList.IconIndex.SymbolicSignalFlexray;
			}
			else if (expSigName == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
			{
				baseImgIndex = MainImageList.IconIndex.SymbolicSignalLIN;
			}
			else if (expSigName == Resources.WebDisplayExportSignalTypeNameColLtlSystemInformation)
			{
				baseImgIndex = MainImageList.IconIndex.LtlSystemInformation;
			}
			else if (expSigName == Resources.WebDisplayExportSignalTypeNameColStatusVariable)
			{
				baseImgIndex = MainImageList.IconIndex.SystemVariable;
			}
			return MainImageList.Instance.GetImage(baseImgIndex);
		}

		public static uint ConvertIPToUint(string ipAddressString)
		{
			if (string.IsNullOrEmpty(ipAddressString))
			{
				return 0u;
			}
			IPAddress iPAddress;
			if (!IPAddress.TryParse(ipAddressString, out iPAddress))
			{
				return 0u;
			}
			byte[] addressBytes = iPAddress.GetAddressBytes();
			uint num = (uint)((uint)addressBytes[3] << 24);
			num += (uint)((uint)addressBytes[2] << 16);
			num += (uint)((uint)addressBytes[1] << 8);
			return num + (uint)addressBytes[0];
		}
	}
}
