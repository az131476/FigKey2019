using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Conversion;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class FileConversionHelper
	{
		private class FileConversionParameter
		{
			public EnumFileConversionParameter Type
			{
				get;
				private set;
			}

			public bool CanBeChangedByUser
			{
				get;
				private set;
			}

			public bool FixedValue
			{
				get;
				private set;
			}

			public FileConversionParameter(EnumFileConversionParameter type)
			{
				this.Type = type;
				this.CanBeChangedByUser = true;
				this.FixedValue = false;
			}

			public FileConversionParameter(EnumFileConversionParameter type, bool canBeChangedByUser, bool fixedValue)
			{
				this.Type = type;
				this.CanBeChangedByUser = canBeChangedByUser;
				this.FixedValue = fixedValue;
			}
		}

		public const string MacroSymbol_DateTime = "{DATE_TIME}";

		public const string MacroSymbol_Year = "{YEAR}";

		public const string MacroSymbol_Month = "{MONTH}";

		public const string MacroSymbol_Day = "{DAY}";

		public const string MacroSymbol_Hour = "{HOUR}";

		public const string MacroSymbol_Min = "{MINUTE}";

		public const string MacroSymbol_Sec = "{SECOND}";

		public const string MacroSymbol_Carname = "{CARNAME}";

		public const string MacroSymbol_Index = "{INDEX}";

		public const string MacroSymbol_Memory = "{MEMORY}";

		public const string MacroSymbol_SerialNr = "{SN}";

		public const string MacroSymbol_LoggerType = "{LOGGERTYPE}";

		public const string MacroSymbol_JobType = "{NAV_TYP}";

		public const string MacroSymbol_NavNumber = "{NAV_NO}";

		public const string MacroSymbol_MarkerName = "{MARKER_NAME}";

		public const string MacroSymbol_TriggerName = "{TRIGGER_NAME}";

		public const string MacroSymbol_CLFName = "{CLF_NAME}";

		public const string DefaultFileNamePattern = "Data{MEMORY}F{INDEX}";

		public const string DefaultFileNamePatternCLFExport = "{CLF_NAME}F{INDEX}";

		private const string MDFVersion200 = "2.0";

		private const string MDFVersion210 = "2.1";

		private const string MDFVersion300 = "3.0";

		private const string MDFVersion300DAT = "3.0 DAT";

		private const string MDFVersion310 = "3.1";

		private const string MDFVersion310GiN = "3.1 GiN";

		private const string MDFVersion320 = "3.2";

		private const string MDFVersion330 = "3.3";

		private const string MDFVersion400 = "4.0";

		private const string MDFVersion410 = "4.1";

		public const string PseudoMacroSymbol_OriginalnameBeforeRename = "{PSEUDO_ORIGINALNAME}";

		public static readonly string FileNameAllowedAdditionalChars;

		public static readonly string FileNameMacroChars;

		private static Dictionary<string, string> _macroNameToSymbol;

		private static HashSet<string> _basicMacroNames;

		private static HashSet<string> _basicCLFExportMacroNames;

		private static HashSet<string> _navigatorMacroNames;

		private static HashSet<string> _superMacroNames;

		private static HashSet<string> _extraCLFExportMacroNames;

		private static readonly string[] MDFVersionsArray;

		private static readonly Dictionary<FileConversionDestFormat, List<FileConversionHelper.FileConversionParameter>> sUserChangeableFileConversionParameters;

		static FileConversionHelper()
		{
			FileConversionHelper.FileNameAllowedAdditionalChars = "()[]_-+=.";
			FileConversionHelper.FileNameMacroChars = "{}";
			FileConversionHelper.MDFVersionsArray = null;
			FileConversionHelper.sUserChangeableFileConversionParameters = new Dictionary<FileConversionDestFormat, List<FileConversionHelper.FileConversionParameter>>
			{
				{
					FileConversionDestFormat.ASC,
					new List<FileConversionHelper.FileConversionParameter>
					{
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.HexadecimalNotation),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SingleFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.GlobalTimestamps),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.RelativeTimestamps),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.JumpOverSleepTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.RecoveryMode),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SaveRawFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SuppressBufferConcat),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitFilesBySize),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.CreateVsysvarFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitFilesByTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.UseMinDigitsForTriggerIndex),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.CopyMediaFiles)
					}
				},
				{
					FileConversionDestFormat.BinlogASC,
					new List<FileConversionHelper.FileConversionParameter>
					{
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.HexadecimalNotation, false, true),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SaveRawFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.CopyMediaFiles)
					}
				},
				{
					FileConversionDestFormat.BLF,
					new List<FileConversionHelper.FileConversionParameter>
					{
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SingleFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.GlobalTimestamps),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.JumpOverSleepTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.RecoveryMode),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SaveRawFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SuppressBufferConcat),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitFilesBySize),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.CreateVsysvarFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitFilesByTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.UseMinDigitsForTriggerIndex),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.CopyMediaFiles)
					}
				},
				{
					FileConversionDestFormat.BinlogBLF,
					new List<FileConversionHelper.FileConversionParameter>
					{
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SaveRawFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.CopyMediaFiles)
					}
				},
				{
					FileConversionDestFormat.MDF,
					new List<FileConversionHelper.FileConversionParameter>
					{
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SingleFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.GlobalTimestamps),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.JumpOverSleepTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.RecoveryMode),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SaveRawFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SuppressBufferConcat),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitFilesBySize),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitByLoc),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitFilesByTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.UseMinDigitsForTriggerIndex),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.CopyMediaFiles)
					}
				},
				{
					FileConversionDestFormat.MF4,
					new List<FileConversionHelper.FileConversionParameter>
					{
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SaveRawFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.CopyMediaFiles)
					}
				},
				{
					FileConversionDestFormat.TXT,
					new List<FileConversionHelper.FileConversionParameter>
					{
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.HexadecimalNotation),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SingleFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.GlobalTimestamps),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.RelativeTimestamps),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.GermanMSExcelFormat),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.JumpOverSleepTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.RecoveryMode),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SaveRawFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SuppressBufferConcat),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitFilesBySize),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitByLoc),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitFilesByTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.UseMinDigitsForTriggerIndex),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.CopyMediaFiles)
					}
				},
				{
					FileConversionDestFormat.XLS,
					new List<FileConversionHelper.FileConversionParameter>
					{
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SingleFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.GlobalTimestamps),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.RelativeTimestamps),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.GermanMSExcelFormat),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.JumpOverSleepTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.RecoveryMode),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SaveRawFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SuppressBufferConcat),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitByLoc),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.UseMinDigitsForTriggerIndex),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.CopyMediaFiles)
					}
				},
				{
					FileConversionDestFormat.IMG,
					new List<FileConversionHelper.FileConversionParameter>
					{
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SingleFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.GlobalTimestamps),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.JumpOverSleepTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.RecoveryMode),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SaveRawFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SuppressBufferConcat),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.UseMinDigitsForTriggerIndex),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.CopyMediaFiles)
					}
				},
				{
					FileConversionDestFormat.CLF,
					new List<FileConversionHelper.FileConversionParameter>()
				},
				{
					FileConversionDestFormat.MAT,
					new List<FileConversionHelper.FileConversionParameter>
					{
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SingleFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.GlobalTimestamps),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.JumpOverSleepTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.RecoveryMode),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SaveRawFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SuppressBufferConcat),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitFilesBySize),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitByLoc),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitFilesByTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.UseMinDigitsForTriggerIndex),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.CopyMediaFiles),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.WriteRawValues)
					}
				},
				{
					FileConversionDestFormat.HDF5,
					new List<FileConversionHelper.FileConversionParameter>
					{
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SingleFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.GlobalTimestamps),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.JumpOverSleepTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.RecoveryMode),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SaveRawFile),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SuppressBufferConcat),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitFilesBySize),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitByLoc),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.SplitFilesByTime),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.UseMinDigitsForTriggerIndex),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.CopyMediaFiles),
						new FileConversionHelper.FileConversionParameter(EnumFileConversionParameter.WriteRawValues)
					}
				}
			};
			FileConversionHelper._macroNameToSymbol = new Dictionary<string, string>();
			FileConversionHelper._basicMacroNames = new HashSet<string>();
			FileConversionHelper._basicCLFExportMacroNames = new HashSet<string>();
			FileConversionHelper._navigatorMacroNames = new HashSet<string>();
			FileConversionHelper._extraCLFExportMacroNames = new HashSet<string>();
			FileConversionHelper._superMacroNames = new HashSet<string>();
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroDateTime, "{DATE_TIME}");
			FileConversionHelper._basicMacroNames.Add(Resources.FileNameMacroDateTime);
			FileConversionHelper._basicCLFExportMacroNames.Add(Resources.FileNameMacroDateTime);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroYear, "{YEAR}");
			FileConversionHelper._basicMacroNames.Add(Resources.FileNameMacroYear);
			FileConversionHelper._basicCLFExportMacroNames.Add(Resources.FileNameMacroYear);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroMonth, "{MONTH}");
			FileConversionHelper._basicMacroNames.Add(Resources.FileNameMacroMonth);
			FileConversionHelper._basicCLFExportMacroNames.Add(Resources.FileNameMacroMonth);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroDay, "{DAY}");
			FileConversionHelper._basicMacroNames.Add(Resources.FileNameMacroDay);
			FileConversionHelper._basicCLFExportMacroNames.Add(Resources.FileNameMacroDay);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroHour, "{HOUR}");
			FileConversionHelper._basicMacroNames.Add(Resources.FileNameMacroHour);
			FileConversionHelper._basicCLFExportMacroNames.Add(Resources.FileNameMacroHour);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroMin, "{MINUTE}");
			FileConversionHelper._basicMacroNames.Add(Resources.FileNameMacroMin);
			FileConversionHelper._basicCLFExportMacroNames.Add(Resources.FileNameMacroMin);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroSec, "{SECOND}");
			FileConversionHelper._basicMacroNames.Add(Resources.FileNameMacroSec);
			FileConversionHelper._basicCLFExportMacroNames.Add(Resources.FileNameMacroSec);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroIndex, "{INDEX}");
			FileConversionHelper._basicMacroNames.Add(Resources.FileNameMacroIndex);
			FileConversionHelper._basicCLFExportMacroNames.Add(Resources.FileNameMacroIndex);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroCarname, "{CARNAME}");
			FileConversionHelper._basicMacroNames.Add(Resources.FileNameMacroCarname);
			FileConversionHelper._basicCLFExportMacroNames.Add(Resources.FileNameMacroCarname);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroMemory, "{MEMORY}");
			FileConversionHelper._basicMacroNames.Add(Resources.FileNameMacroMemory);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroSerialNr, "{SN}");
			FileConversionHelper._basicMacroNames.Add(Resources.FileNameMacroSerialNr);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroLoggerType, "{LOGGERTYPE}");
			FileConversionHelper._basicMacroNames.Add(Resources.FileNameMacroLoggerType);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroJobType, "{NAV_TYP}");
			FileConversionHelper._navigatorMacroNames.Add(Resources.FileNameMacroJobType);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroMeasurementNr, "{NAV_NO}");
			FileConversionHelper._navigatorMacroNames.Add(Resources.FileNameMacroMeasurementNr);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroMarkerName, "{MARKER_NAME}");
			FileConversionHelper._navigatorMacroNames.Add(Resources.FileNameMacroMarkerName);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroTriggerName, "{TRIGGER_NAME}");
			FileConversionHelper._navigatorMacroNames.Add(Resources.FileNameMacroTriggerName);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroCLFName, "{CLF_NAME}");
			FileConversionHelper._extraCLFExportMacroNames.Add(Resources.FileNameMacroCLFName);
			FileConversionHelper._macroNameToSymbol.Add(Resources.FileNameMacroOriginalName, "Data{MEMORY}F{INDEX}");
			FileConversionHelper._superMacroNames.Add(Resources.FileNameMacroOriginalName);
			FileConversionHelper.MDFVersionsArray = new string[]
			{
				"2.0",
				"2.1",
				"3.0",
				"3.0 DAT",
				"3.1",
				"3.1 GiN",
				"3.2",
				"3.3",
				"4.0",
				"4.1"
			};
		}

		public static List<string> GetBasicMacroNames()
		{
			return FileConversionHelper.GetBasicMacroNames(null);
		}

		public static List<string> GetBasicMacroNames(ILoggerSpecifics loggerSpecifics)
		{
			if (loggerSpecifics == null)
			{
				return new List<string>(FileConversionHelper._basicMacroNames);
			}
			List<string> list = new List<string>();
			foreach (string current in FileConversionHelper._basicMacroNames)
			{
				if (loggerSpecifics.DeviceAccess.IsSetVehicleNameSupported || !(current == Resources.FileNameMacroCarname))
				{
					list.Add(current);
				}
			}
			return list;
		}

		public static List<string> GetBasicCLFExportMacroNames()
		{
			return new List<string>(FileConversionHelper._basicCLFExportMacroNames);
		}

		public static List<string> GetNavigatorMacroNames()
		{
			return new List<string>(FileConversionHelper._navigatorMacroNames);
		}

		public static List<string> GetExtraCLFExportMacroNames()
		{
			return new List<string>(FileConversionHelper._extraCLFExportMacroNames);
		}

		public static List<string> GetSuperMacroNames()
		{
			return new List<string>(FileConversionHelper._superMacroNames);
		}

		private static List<string> GetMacroSymbols()
		{
			return new List<string>(FileConversionHelper._macroNameToSymbol.Values);
		}

		private static List<string> GetBasicMacroSymbols(bool isCLFConversion, ILoggerSpecifics loggerSpecifics)
		{
			List<string> list = new List<string>();
			HashSet<string> hashSet;
			if (!isCLFConversion)
			{
				hashSet = new HashSet<string>(FileConversionHelper._basicMacroNames);
				if (loggerSpecifics != null && !loggerSpecifics.DeviceAccess.IsSetVehicleNameSupported)
				{
					hashSet.Remove(Resources.FileNameMacroCarname);
				}
			}
			else
			{
				hashSet = new HashSet<string>(FileConversionHelper._basicCLFExportMacroNames);
			}
			foreach (string current in hashSet)
			{
				list.Add(FileConversionHelper._macroNameToSymbol[current]);
			}
			return list;
		}

		private static List<string> GetNavigatorMacroSymbols()
		{
			List<string> list = new List<string>();
			foreach (string current in FileConversionHelper._navigatorMacroNames)
			{
				list.Add(FileConversionHelper._macroNameToSymbol[current]);
			}
			return list;
		}

		private static List<string> GetExtraCLFExportMacroSymbols()
		{
			List<string> list = new List<string>();
			foreach (string current in FileConversionHelper._extraCLFExportMacroNames)
			{
				list.Add(FileConversionHelper._macroNameToSymbol[current]);
			}
			return list;
		}

		private static List<string> GetSuperMacroSymbols()
		{
			List<string> list = new List<string>();
			foreach (string current in FileConversionHelper._superMacroNames)
			{
				list.Add(FileConversionHelper._macroNameToSymbol[current]);
			}
			return list;
		}

		public static string GetMacroSymbolForName(string macroName)
		{
			if (FileConversionHelper._macroNameToSymbol.ContainsKey(macroName))
			{
				return FileConversionHelper._macroNameToSymbol[macroName];
			}
			return "";
		}

		public static bool ValidateMacrosInFileNamePattern(ref TextBox textBox, ref ErrorProvider errorProvider, bool isCLFExport, bool allowNavigatorMacros, ILoggerSpecifics loggerSpecifics, out string exampleFileName)
		{
			exampleFileName = "";
			int num = -1;
			List<string> list = new List<string>();
			List<string> basicMacroSymbols;
			if (!isCLFExport)
			{
				basicMacroSymbols = FileConversionHelper.GetBasicMacroSymbols(false, loggerSpecifics);
				if (allowNavigatorMacros)
				{
					basicMacroSymbols.AddRange(FileConversionHelper.GetNavigatorMacroSymbols());
				}
				basicMacroSymbols.AddRange(FileConversionHelper.GetSuperMacroSymbols());
			}
			else
			{
				basicMacroSymbols = FileConversionHelper.GetBasicMacroSymbols(true, null);
				if (allowNavigatorMacros)
				{
					basicMacroSymbols.AddRange(FileConversionHelper.GetNavigatorMacroSymbols());
				}
				basicMacroSymbols.AddRange(FileConversionHelper.GetExtraCLFExportMacroSymbols());
			}
			for (int i = 0; i < textBox.Text.Length; i++)
			{
				if (textBox.Text[i] == '{')
				{
					if (num >= 0)
					{
						errorProvider.SetError(textBox, Resources.ErrorSecondOpenBraceFound);
						return false;
					}
					num = i;
				}
				else if (textBox.Text[i] == '}')
				{
					if (num < 0)
					{
						errorProvider.SetError(textBox, Resources.ErrorCloseBraceFoundWithoutMatch);
						return false;
					}
					string text = textBox.Text.Substring(num, i + 1 - num);
					if (!basicMacroSymbols.Contains(text))
					{
						errorProvider.SetError(textBox, string.Format(Resources.ErrorUnknownMacroFound, text));
						return false;
					}
					num = -1;
					list.Add(text);
				}
			}
			if (num >= 0)
			{
				errorProvider.SetError(textBox, Resources.ErrorOpenBraceFoundWithoutMatch);
				return false;
			}
			if (list.Count == 0)
			{
				errorProvider.SetError(textBox, Resources.ErrorAtLeastOneMacroMustBeUsed);
				return false;
			}
			LogFileInfo logFileInfo = new LogFileInfo(null, null, null, null, null);
			logFileInfo.FillWithExampleData();
			exampleFileName = FileConversionHelper.FillOutMacros(textBox.Text, logFileInfo, false, "Data1.clf");
			errorProvider.SetError(textBox, "");
			return true;
		}

		public static string FillOutMacros(string filenameWithMacros, LogFileInfo logFileInfo, bool isCLF, string clfFileName)
		{
			string text = filenameWithMacros.Replace("{MEMORY}", logFileInfo.InfoLoggerMemoryNumber);
			text = text.Replace("{SN}", logFileInfo.InfoSerialNumber);
			text = text.Replace("{LOGGERTYPE}", logFileInfo.InfoLoggerType);
			text = text.Replace("{NAV_TYP}", logFileInfo.InfoNavJobType);
			text = text.Replace("{NAV_NO}", logFileInfo.InfoNavNumber);
			text = text.Replace("{MARKER_NAME}", logFileInfo.InfoNavMarkerName);
			text = text.Replace("{TRIGGER_NAME}", logFileInfo.InfoNavTriggerName);
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(clfFileName);
			text = text.Replace("{CLF_NAME}", fileNameWithoutExtension);
			if (isCLF)
			{
				text = text.Replace("{PSEUDO_ORIGINALNAME}", fileNameWithoutExtension);
				text = text.Replace("{DATE_TIME}", logFileInfo.InfoFirstRecordingDateTime);
				text = text.Replace("{YEAR}", logFileInfo.InfoFirstYear);
				text = text.Replace("{MONTH}", logFileInfo.InfoFirstMonth);
				text = text.Replace("{DAY}", logFileInfo.InfoFirstDay);
				text = text.Replace("{HOUR}", logFileInfo.InfoFirstHour);
				text = text.Replace("{MINUTE}", logFileInfo.InfoFirstMinute);
				text = text.Replace("{SECOND}", logFileInfo.InfoFirstSecond);
				text = text.Replace("{CARNAME}", logFileInfo.InfoGlobalCarname);
				text = text.Replace("{INDEX}", "");
			}
			else
			{
				text = text.Replace("{PSEUDO_ORIGINALNAME}", logFileInfo.InfoOriginalFilenameWithoutExtension);
				text = text.Replace("{DATE_TIME}", logFileInfo.InfoRecordingDateTime);
				text = text.Replace("{YEAR}", logFileInfo.InfoYear);
				text = text.Replace("{MONTH}", logFileInfo.InfoMonth);
				text = text.Replace("{DAY}", logFileInfo.InfoDay);
				text = text.Replace("{HOUR}", logFileInfo.InfoHour);
				text = text.Replace("{MINUTE}", logFileInfo.InfoMinute);
				text = text.Replace("{SECOND}", logFileInfo.InfoSecond);
				text = text.Replace("{CARNAME}", logFileInfo.InfoCarname);
				text = text.Replace("{INDEX}", logFileInfo.InfoRecordingIndex);
				if (logFileInfo.InfoPartNumber.Length > 0)
				{
					text = text + "." + logFileInfo.InfoPartNumber;
				}
			}
			return text;
		}

		public static string RenameFile(string originalFilePath, string newFilePath, out string errorText, LogFileInfo logFileInfo, bool isCLF = false)
		{
			errorText = "";
			string text = newFilePath;
			if (string.Compare(newFilePath, originalFilePath, true) != 0)
			{
				if (File.Exists(newFilePath))
				{
					uint num = 0u;
					string text2;
					while (true)
					{
						num += 1u;
						if (num <= 1u)
						{
							if (!isCLF && logFileInfo.InfoRecordingIndex.Length > 0)
							{
								text2 = Path.Combine(Path.GetDirectoryName(newFilePath), Path.GetFileNameWithoutExtension(newFilePath) + string.Format("_{0}", logFileInfo.InfoRecordingIndex) + Path.GetExtension(newFilePath));
								if (!File.Exists(text2))
								{
									break;
								}
							}
						}
						else
						{
							text = Path.Combine(Path.GetDirectoryName(newFilePath), Path.GetFileNameWithoutExtension(newFilePath) + string.Format("_({0:D})", num) + Path.GetExtension(newFilePath));
						}
						if (!File.Exists(text) || num >= 1000u)
						{
							goto IL_C8;
						}
					}
					text = text2;
				}
				IL_C8:
				GenerationUtil.TryMoveFile(originalFilePath, text, out errorText);
			}
			return text;
		}

		public static uint[] GetDefaultChannelMapping(BusType type)
		{
			uint sourceChannels = 16u;
			if (type == BusType.Bt_LIN)
			{
				sourceChannels = 2u + Constants.MaximumNumberOfLINprobeChannels;
			}
			if (type == BusType.Bt_FlexRay)
			{
				sourceChannels = 2u;
			}
			return FileConversionHelper.GetDefaultChannelMapping(sourceChannels);
		}

		public static uint[] GetDefaultChannelMapping(uint sourceChannels)
		{
			uint[] array = new uint[sourceChannels];
			for (uint num = 1u; num <= sourceChannels; num += 1u)
			{
				array[(int)((UIntPtr)(num - 1u))] = num;
			}
			return array;
		}

		public static string GetName(EnumViewType type)
		{
			switch (type)
			{
			case EnumViewType.ClfExport:
				return Resources.ViewNameClfExport;
			case EnumViewType.Classic:
				return Resources.ViewNameClassic;
			case EnumViewType.Navigator:
				return Resources.ViewNameNavigator;
			default:
				return string.Empty;
			}
		}

		public static IList<string> GetDestinationFormatVersions(FileConversionParameters parameters)
		{
			List<string> result = new List<string>();
			if (parameters.DestinationFormat == FileConversionDestFormat.MDF)
			{
				result = FileConversionHelper.MDFVersionsArray.ToList<string>();
			}
			return result;
		}

		public static string GetConfiguredDestinationFormatVersion(FileConversionParameters parameters)
		{
			string text = "";
			if (parameters.DestinationFormat == FileConversionDestFormat.MDF)
			{
				text = "4.1";
			}
			string text2 = parameters.DestinationFormatVersionStrings[(int)parameters.DestinationFormat];
			if (text2 == null || text2.Length == 0)
			{
				text2 = text;
			}
			return text2;
		}

		public static bool UseMDFLegacyConversion(FileConversionParameters parameters)
		{
			bool result = false;
			if (parameters.DestinationFormat == FileConversionDestFormat.MDF)
			{
				string text = parameters.DestinationFormatVersionStrings[(int)parameters.DestinationFormat];
				if (text != null && text == "3.1 GiN")
				{
					result = true;
				}
			}
			return result;
		}

		public static bool UseMDFCompatibilityMode(FileConversionParameters parameters)
		{
			bool result = false;
			if (parameters.DestinationFormat == FileConversionDestFormat.MDF)
			{
				string text = parameters.DestinationFormatVersionStrings[(int)parameters.DestinationFormat];
				if (text != null && text == "3.0 DAT")
				{
					result = true;
				}
			}
			return result;
		}

		public static string GetMDFVersionString(FileConversionParameters parameters)
		{
			string result = "";
			if (parameters.DestinationFormat == FileConversionDestFormat.MDF)
			{
				string text = parameters.DestinationFormatVersionStrings[(int)parameters.DestinationFormat];
				if (text != null)
				{
					double num;
					if (text == "3.0 DAT")
					{
						result = "300";
					}
					else if (text == "3.1 GiN")
					{
						result = "310";
					}
					else if (double.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out num))
					{
						result = Convert.ToInt32(num * 100.0).ToString();
					}
				}
			}
			return result;
		}

		public static void SetMDFDefaultVersion(FileConversionParameters parameters)
		{
			parameters.DestinationFormat = FileConversionDestFormat.MDF;
			parameters.DestinationFormatVersionStrings[4] = "4.1";
		}

		public static bool ShouldExtensionBeReplaced(FileConversionParameters parameters)
		{
			return (parameters.DestinationFormat == FileConversionDestFormat.MDF && FileConversionHelper.GetConfiguredDestinationFormatExtension(parameters) == Vocabulary.FileExtensionDotMF4) || ((parameters.DestinationFormat == FileConversionDestFormat.XLS || parameters.DestinationFormat == FileConversionDestFormat.TXT) && FileConversionHelper.GetConfiguredDestinationFormatExtension(parameters) == Vocabulary.FileExtensionDotCSV) || (parameters.DestinationFormat == FileConversionDestFormat.HDF5 && (FileConversionHelper.GetConfiguredDestinationFormatExtension(parameters) == Vocabulary.FileExtensionDotHE5 || FileConversionHelper.GetConfiguredDestinationFormatExtension(parameters) == Vocabulary.FileExtensionDotHDF5));
		}

		public static IList<string> GetDestinationFormatExtensions(FileConversionParameters parameters)
		{
			List<string> list = new List<string>();
			list.Add(FileConversionHelper.GetDefaultFileExtension(parameters));
			if (parameters.DestinationFormat == FileConversionDestFormat.MDF)
			{
				if (FileConversionHelper.GetConfiguredDestinationFormatVersion(parameters) == "4.0" || FileConversionHelper.GetConfiguredDestinationFormatVersion(parameters) == "4.1")
				{
					list.Add(Vocabulary.FileExtensionDotMDF);
				}
			}
			else if (parameters.DestinationFormat == FileConversionDestFormat.XLS || parameters.DestinationFormat == FileConversionDestFormat.TXT)
			{
				list.Add(Vocabulary.FileExtensionDotCSV);
			}
			else if (parameters.DestinationFormat == FileConversionDestFormat.HDF5)
			{
				list.Add(Vocabulary.FileExtensionDotHE5);
				list.Add(Vocabulary.FileExtensionDotHDF5);
			}
			return list;
		}

		public static string GetConfiguredDestinationFormatExtension(FileConversionParameters parameters)
		{
			string text = parameters.DestinationFormatExtensions[(int)parameters.DestinationFormat];
			if (text == null || text.Length == 0)
			{
				text = FileConversionHelper.GetDefaultFileExtension(parameters);
			}
			return text;
		}

		public static string GetDefaultFileExtension(FileConversionParameters parameters)
		{
			switch (parameters.DestinationFormat)
			{
			case FileConversionDestFormat.ASC:
			case FileConversionDestFormat.BinlogASC:
				return Vocabulary.FileExtensionDotASC;
			case FileConversionDestFormat.BLF:
			case FileConversionDestFormat.BinlogBLF:
				return Vocabulary.FileExtensionDotBLF;
			case FileConversionDestFormat.MDF:
				if (FileConversionHelper.UseMDFCompatibilityMode(parameters))
				{
					return Vocabulary.FileExtensionDotDAT;
				}
				if (FileConversionHelper.GetConfiguredDestinationFormatVersion(parameters) == "4.0" || FileConversionHelper.GetConfiguredDestinationFormatVersion(parameters) == "4.1")
				{
					return Vocabulary.FileExtensionDotMF4;
				}
				return Vocabulary.FileExtensionDotMDF;
			case FileConversionDestFormat.MF4:
				return Vocabulary.FileExtensionDotMF4;
			case FileConversionDestFormat.TXT:
				return Vocabulary.FileExtensionDotTXT;
			case FileConversionDestFormat.XLS:
				return Vocabulary.FileExtensionDotTXT;
			case FileConversionDestFormat.IMG:
				return Vocabulary.FileExtensionDotIMG;
			case FileConversionDestFormat.CLF:
				return Vocabulary.FileExtensionDotCLF;
			case FileConversionDestFormat.MAT:
				return Vocabulary.FileExtensionDotMAT;
			case FileConversionDestFormat.HDF5:
				return Vocabulary.FileExtensionDotH5;
			default:
				return "";
			}
		}

		public static string FileConversionDestFormat2String(FileConversionDestFormat format)
		{
			switch (format)
			{
			case FileConversionDestFormat.ASC:
			case FileConversionDestFormat.BinlogASC:
				return Resources.DestinationFileFormatASC;
			case FileConversionDestFormat.BLF:
			case FileConversionDestFormat.BinlogBLF:
				return Resources.DestinationFileFormatBLF;
			case FileConversionDestFormat.MDF:
				return Resources.DestinationFileFormatMDF;
			case FileConversionDestFormat.MF4:
				return Resources.DestinationFileFormatMF4;
			case FileConversionDestFormat.TXT:
				return Resources.DestinationFileFormatTXT;
			case FileConversionDestFormat.XLS:
				return Resources.DestinationFileFormatXLS;
			case FileConversionDestFormat.IMG:
				return Resources.DestinationFileFormatIMG;
			case FileConversionDestFormat.CLF:
				return Resources.DestinationFileFormatCLF;
			case FileConversionDestFormat.MAT:
				return Resources.DestinationFileFormatMAT;
			case FileConversionDestFormat.HDF5:
				return Resources.DestinationFileFormatHDF5;
			default:
				return "";
			}
		}

		public static string FileConversionFilenameFormat2String(FileConversionFilenameFormat format)
		{
			switch (format)
			{
			case FileConversionFilenameFormat.UseOriginalName:
				return Resources.ConversionUseOriginalName;
			case FileConversionFilenameFormat.AddPrefix:
				return Resources.ConversionAddPrefix;
			case FileConversionFilenameFormat.UseCustomName:
				return Resources.ConversionUseCustomName;
			default:
				return "";
			}
		}

		public static FileConversionFilenameFormat String2FileConversionFilenameFormat(string formatName)
		{
			foreach (FileConversionFilenameFormat fileConversionFilenameFormat in Enum.GetValues(typeof(FileConversionFilenameFormat)))
			{
				if (FileConversionHelper.FileConversionFilenameFormat2String(fileConversionFilenameFormat) == formatName)
				{
					return fileConversionFilenameFormat;
				}
			}
			return FileConversionFilenameFormat.UseOriginalName;
		}

		public static string FileConversionTimeBase2String(FileConversionTimeBase timeBase)
		{
			switch (timeBase)
			{
			case FileConversionTimeBase.Minute:
				return Resources.ConversionTimeBaseMinute;
			case FileConversionTimeBase.Hour:
				return Resources.ConversionTimeBaseHour;
			case FileConversionTimeBase.Day:
				return Resources.ConversionTimeBaseDay;
			default:
				return "";
			}
		}

		public static FileConversionTimeBase String2FileConversionTimeBase(string timeBaseName)
		{
			foreach (FileConversionTimeBase fileConversionTimeBase in Enum.GetValues(typeof(FileConversionTimeBase)))
			{
				if (FileConversionHelper.FileConversionTimeBase2String(fileConversionTimeBase) == timeBaseName)
				{
					return fileConversionTimeBase;
				}
			}
			return FileConversionTimeBase.Minute;
		}

		public static FileConversionParameters GetDefaultParameters(LoggerType loggerType = LoggerType.Unknown)
		{
			ILoggerSpecifics loggerSpecifics = (loggerType == LoggerType.Unknown) ? null : LoggerSpecificsFactory.CreateLoggerSpecifics(loggerType);
			return FileConversionHelper.GetDefaultParameters(loggerSpecifics);
		}

		public static FileConversionParameters GetDefaultParameters(ILoggerSpecifics loggerSpecifics)
		{
			return new FileConversionParameters
			{
				DestinationFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal),
				DestinationFormat = (loggerSpecifics != null) ? loggerSpecifics.FileConversion.DefaultDestFormat : FileConversionDestFormat.ASC,
				DestinationFormatVersionStrings = new string[Enum.GetValues(typeof(FileConversionDestFormat)).Length],
				DestinationFormatExtensions = new string[Enum.GetValues(typeof(FileConversionDestFormat)).Length],
				DeleteSourceFilesWhenDone = false,
				OverwriteDestinationFiles = false,
				SaveRawFile = true,
				HexadecimalNotation = false,
				SingleFile = false,
				GlobalTimestamps = false,
				RelativeTimestamps = false,
				GermanMSExcelFormat = false,
				UseUnlimitedFileSize = false,
				SplitFilesBySize = false,
				FileFractionSize = 100,
				SplitFilesByTime = false,
				UseRealTimeRaster = false,
				TimeBase = FileConversionTimeBase.Hour,
				FileFractionTime = 1,
				GenerateVsysvarFile = false,
				JumpOverSleepTime = false,
				RecoveryMode = false,
				SuppressBufferConcat = false,
				FilenameFormat = FileConversionFilenameFormat.UseOriginalName,
				Prefix = string.Empty,
				CustomFilename = "Data{MEMORY}F{INDEX}",
				CopyMediaFiles = true,
				UseChannelMapping = false,
				CanChannelMapping = FileConversionHelper.GetDefaultChannelMapping(BusType.Bt_CAN),
				LinChannelMapping = FileConversionHelper.GetDefaultChannelMapping(BusType.Bt_LIN),
				FlexRayChannelMapping = FileConversionHelper.GetDefaultChannelMapping(BusType.Bt_FlexRay),
				HideChannelMappingIdentities = false,
				UseDateTimeFromMeasurementStart = false,
				UseMinimumDigitsForTriggerIndex = false,
				MinimumDigitsForTriggerIndex = Constants.DefaultMinimumDigitsForTriggerIndex,
				ExportDatabaseConfiguration = null
			};
		}

		public static bool EnableAndSetByFormat(CheckBox checkBox, FileConversionDestFormat format, EnumFileConversionParameter param)
		{
			bool enabled = false;
			if (FileConversionHelper.sUserChangeableFileConversionParameters.ContainsKey(format) && FileConversionHelper.sUserChangeableFileConversionParameters[format].Any((FileConversionHelper.FileConversionParameter t) => t.Type == param))
			{
				FileConversionHelper.FileConversionParameter fileConversionParameter = FileConversionHelper.sUserChangeableFileConversionParameters[format].First((FileConversionHelper.FileConversionParameter t) => t.Type == param);
				enabled = fileConversionParameter.CanBeChangedByUser;
				bool arg_74_0 = fileConversionParameter.FixedValue;
			}
			checkBox.Enabled = enabled;
			return checkBox.Checked;
		}

		public static bool IsSignalOrientedDestFormat(FileConversionDestFormat format)
		{
			return FileConversionDestFormat.MDF == format || FileConversionDestFormat.XLS == format || FileConversionDestFormat.MAT == format || FileConversionDestFormat.HDF5 == format;
		}

		public static bool IsSigLogDestFormat(FileConversionParameters parameters)
		{
			return !FileConversionHelper.UseMDFLegacyConversion(parameters) && (FileConversionDestFormat.MDF == parameters.DestinationFormat || FileConversionDestFormat.MAT == parameters.DestinationFormat || FileConversionDestFormat.HDF5 == parameters.DestinationFormat);
		}

		public static bool UseSigLogINIForDBConfig(FileConversionParameters parameters)
		{
			return FileConversionHelper.IsSigLogDestFormat(parameters) && FileConversionDestFormat.MDF == parameters.DestinationFormat;
		}

		public static bool UseArxmlToDBCConversion(FileConversionParameters parameters)
		{
			return FileConversionHelper.UseMDFLegacyConversion(parameters);
		}

		public static bool PerformChecksForSignalOrientedDestFormat(FileConversionDestFormat destFormat, FileConversionParameters fileConversionParameters, IPropertyWindow propertyWindow)
		{
			if (!FileConversionHelper.IsSignalOrientedDestFormat(destFormat))
			{
				return true;
			}
			Result result = AnalysisPackage.Extract(fileConversionParameters.ExportDatabaseConfiguration);
			if (result == Result.UserAbort)
			{
				return false;
			}
			if (result == Result.Error)
			{
				InformMessageBox.Error(Resources.AnalysisErrorWhileExtracting);
				return false;
			}
			string reportText;
			if (propertyWindow.SemanticChecker.HasMultipleMsgIDsInDBsOnSameChannel(fileConversionParameters, out reportText) && DisplayReportWithQuestion.ShowDisplayReportDialog(Resources.FileConversionWinCaption, Resources.ChannelsHaveDBsMultipleDefsForSameID, reportText, Resources.DataFromMultipleDefMsgIDNotConverted + Resources.QuestionContinueAnyway, false) == DialogResult.No)
			{
				return false;
			}
			bool allowFlexRay = !FileConversionHelper.UseMDFLegacyConversion(fileConversionParameters);
			int num;
			propertyWindow.ModelValidator.DatabaseServices.HasDatabasesAccessibleForSignalConversion(out num, fileConversionParameters, allowFlexRay);
			return num <= 0 || InformMessageBox.Question(Resources.ConvReqAccessToDBsButNotAllAccessible + " " + Resources.QuestionContinueAnyway) != DialogResult.No;
		}
	}
}
