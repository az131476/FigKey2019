using Nini.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Conversion;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LTLGenerationCS;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public class CLexport : GenericToolInterface
	{
		internal class SiglogIniParameter
		{
			public string DBPath
			{
				get;
				private set;
			}

			public BusType BusType
			{
				get;
				private set;
			}

			public uint Channel
			{
				get;
				private set;
			}

			public string NetworkName
			{
				get;
				private set;
			}

			public bool IsXCP
			{
				get;
				private set;
			}

			public SiglogIniParameter(string dbPath, BusType busType, uint channel, string networkName, bool isXCP)
			{
				this.DBPath = dbPath;
				this.BusType = busType;
				this.Channel = channel;
				this.NetworkName = networkName;
				this.IsXCP = isXCP;
			}
		}

		private const int GiN_ErrorCode_FilForm = 32;

		public const string clExportHandlerFileName = "RenameHandler.bat";

		private string clExportHandlerPath;

		private string currentTocFilePath;

		private string clExportSiglogIniPath;

		public static readonly string tocFileName = "tocFile.txt";

		public static readonly string tocSeparator = "***";

		public CLexport()
		{
			base.FileName = "CLexport.exe";
			this.clExportHandlerPath = string.Empty;
		}

		private string GetFormatCommandLineParameter(FileConversionParameters fileConversionParameters)
		{
			switch (fileConversionParameters.DestinationFormat)
			{
			case FileConversionDestFormat.ASC:
				return "-A";
			case FileConversionDestFormat.BLF:
				return "-B";
			case FileConversionDestFormat.MDF:
				if (FileConversionHelper.UseMDFLegacyConversion(fileConversionParameters))
				{
					return "-M";
				}
				return "-MB";
			case FileConversionDestFormat.TXT:
				if (fileConversionParameters.GermanMSExcelFormat)
				{
					return "-x";
				}
				return "-X";
			case FileConversionDestFormat.XLS:
				if (fileConversionParameters.GermanMSExcelFormat)
				{
					return "-xb";
				}
				return "-Xb";
			case FileConversionDestFormat.IMG:
				return "-G";
			case FileConversionDestFormat.CLF:
				return "";
			case FileConversionDestFormat.MAT:
			case FileConversionDestFormat.HDF5:
				return this.CreateSiglogParameter(fileConversionParameters);
			}
			return "";
		}

		public bool ExportCLFAsync(string clfFile, FileConversionParameters conversionParameters, string destinationPath, DatabaseConfiguration databaseConfiguration, string configurationFolder, out string errorText, IProgressIndicator pi, ProcessExitedDelegate processExitedDelegate, ILoggerSpecifics loggerSpecifics, bool doNotUseBatchHandler = false, ConversionJob job = null)
		{
			base.LastExitCode = 0;
			if (conversionParameters.DestinationFormat == FileConversionDestFormat.CLF)
			{
				errorText = Resources.CLFexportErrorCannotConvertToCLF;
				return false;
			}
			if (!File.Exists(clfFile))
			{
				errorText = string.Format(Resources.FileDoesNotExist, clfFile);
				return false;
			}
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-v");
			base.AddCommandLineArgument("-~");
			base.AddCommandLineArgument("-o");
			base.AddCommandLineArgument("-t");
			base.AddCommandLineArgument("-l");
			base.AddCommandLineArgument("\"" + clfFile + "\"");
			base.AddCommandLineArgument(this.GetFormatCommandLineParameter(conversionParameters));
			base.AddCommandLineArgument("-O \"" + destinationPath + "\"");
			if (conversionParameters.GlobalTimestamps)
			{
				base.AddCommandLineArgument("-g");
				base.AddCommandLineArgument("-E*");
			}
			if (conversionParameters.HexadecimalNotation)
			{
				base.AddCommandLineArgument("-h");
			}
			if (conversionParameters.JumpOverSleepTime)
			{
				base.AddCommandLineArgument("-j");
			}
			if (conversionParameters.RecoveryMode)
			{
				base.AddCommandLineArgument("-r");
			}
			if (conversionParameters.RelativeTimestamps)
			{
				base.AddCommandLineArgument("-R");
			}
			if (conversionParameters.SingleFile)
			{
				base.AddCommandLineArgument("-s");
				base.AddCommandLineArgument("-E*");
			}
			if (conversionParameters.UseUnlimitedFileSize && (conversionParameters.DestinationFormat == FileConversionDestFormat.TXT || conversionParameters.DestinationFormat == FileConversionDestFormat.XLS || FileConversionHelper.UseMDFLegacyConversion(conversionParameters)))
			{
				base.AddCommandLineArgument("-u");
			}
			if (!conversionParameters.UseUnlimitedFileSize && (conversionParameters.DestinationFormat == FileConversionDestFormat.MDF || conversionParameters.DestinationFormat == FileConversionDestFormat.MAT || conversionParameters.DestinationFormat == FileConversionDestFormat.HDF5))
			{
				base.AddCommandLineArgument("-w 1500");
			}
			if (conversionParameters.DestinationFormat == FileConversionDestFormat.ASC || conversionParameters.DestinationFormat == FileConversionDestFormat.BLF || conversionParameters.DestinationFormat == FileConversionDestFormat.MDF || conversionParameters.DestinationFormat == FileConversionDestFormat.TXT || conversionParameters.DestinationFormat == FileConversionDestFormat.MAT || conversionParameters.DestinationFormat == FileConversionDestFormat.HDF5)
			{
				if (conversionParameters.SplitFilesBySize)
				{
					base.AddCommandLineArgument("-w " + conversionParameters.FileFractionSize);
				}
				if (conversionParameters.SplitFilesByTime)
				{
					string text = conversionParameters.FileFractionTime.ToString();
					if (conversionParameters.TimeBase == FileConversionTimeBase.Hour)
					{
						text += "h";
					}
					else if (conversionParameters.TimeBase == FileConversionTimeBase.Day)
					{
						text += "d";
					}
					else
					{
						text += "m";
					}
					if (conversionParameters.UseRealTimeRaster)
					{
						text = text + ";" + text;
					}
					base.AddCommandLineArgument("-wT " + text);
				}
			}
			bool flag = FileConversionHelper.IsSigLogDestFormat(conversionParameters);
			if (flag)
			{
				base.AddCommandLineArgument("SystemChannel=\"" + this.GetAbsoluteIniPathFromExecDirectory("Binlog_GL.ini") + "\"");
				if (FileConversionHelper.UseMDFCompatibilityMode(conversionParameters) && conversionParameters.FilenameFormat == FileConversionFilenameFormat.UseOriginalName)
				{
					conversionParameters.FilenameFormat = FileConversionFilenameFormat.AddPrefix;
				}
			}
			if (conversionParameters.DestinationFormat == FileConversionDestFormat.ASC || conversionParameters.DestinationFormat == FileConversionDestFormat.BLF)
			{
				base.AddCommandLineArgument("SystemChannel=\"" + this.GetAbsoluteIniPathFromExecDirectory("Binlog_GL_CANoe.ini") + "\"");
			}
			if (conversionParameters.UseChannelMapping)
			{
				this.AddChannelMapping(conversionParameters, loggerSpecifics);
			}
			if (job != null)
			{
				if (job.Type == ConversionJobType.Marker)
				{
					base.DeleteSpecificCommandLineArgument("-E*");
					base.AddCommandLineArgument("-E");
					this.AddStartTimeArgument(job, true);
				}
				else if (conversionParameters.GlobalTimestamps)
				{
					base.DeleteSpecificCommandLineArgument("-E*");
					base.AddCommandLineArgument("-E");
					this.AddStartTimeArgument(job, false);
				}
			}
			if (conversionParameters.UseMinimumDigitsForTriggerIndex)
			{
				if (conversionParameters.MinimumDigitsForTriggerIndex == 0)
				{
					base.AddCommandLineArgument("MinFileNameDigits=auto");
				}
				else
				{
					base.AddCommandLineArgument("MinFileNameDigits=" + conversionParameters.MinimumDigitsForTriggerIndex);
				}
			}
			if (!doNotUseBatchHandler)
			{
				this.CreateExportHandlerBatch(destinationPath, conversionParameters.UseDateTimeFromMeasurementStart);
				base.AddCommandLineArgument("CLexportHandler=\"" + Path.Combine(destinationPath, "RenameHandler.bat") + "\"");
			}
			if (FileConversionHelper.IsSignalOrientedDestFormat(conversionParameters.DestinationFormat))
			{
				int num = 0;
				int num2 = 0;
				foreach (Database current in AnalysisPackage.GetConversionDatabases(conversionParameters, databaseConfiguration.Databases, true))
				{
					if (current.BusType.Value == BusType.Bt_CAN || current.BusType.Value == BusType.Bt_LIN || (flag && (current.BusType.Value == BusType.Bt_FlexRay || current.BusType.Value == BusType.Bt_Ethernet)))
					{
						string path2;
						if (current.IsAUTOSARFile && current.BusType.Value == BusType.Bt_CAN && FileConversionHelper.UseArxmlToDBCConversion(conversionParameters))
						{
							string path = FileSystemServices.GenerateUniqueDbcFileName(Path.GetFileNameWithoutExtension(current.FilePath.Value), current.NetworkName.Value, num2);
							path2 = Path.Combine(destinationPath, path);
						}
						else
						{
							path2 = FileSystemServices.GetAbsolutePath(current.FilePath.Value, configurationFolder);
						}
						if (File.Exists(path2))
						{
							num++;
						}
					}
					num2++;
				}
				if (num == 0)
				{
					errorText = string.Format(Resources.ErrorUnableToConvertToSignalOrientedFormat, conversionParameters.DestinationFormat.ToString());
					return false;
				}
				num2 = 0;
				Dictionary<uint, HashSet<string>> dictionary = new Dictionary<uint, HashSet<string>>();
				Dictionary<uint, HashSet<string>> dictionary2 = new Dictionary<uint, HashSet<string>>();
				List<CLexport.SiglogIniParameter> list = new List<CLexport.SiglogIniParameter>();
				foreach (Database current2 in AnalysisPackage.GetConversionDatabases(conversionParameters, databaseConfiguration.Databases, true))
				{
					if (current2.BusType.Value == BusType.Bt_CAN)
					{
						if (!dictionary.ContainsKey(current2.ChannelNumber.Value))
						{
							dictionary.Add(current2.ChannelNumber.Value, new HashSet<string>());
							dictionary[current2.ChannelNumber.Value].Add(current2.FilePath.Value);
						}
						else
						{
							if (dictionary[current2.ChannelNumber.Value].Contains(current2.FilePath.Value))
							{
								continue;
							}
							dictionary[current2.ChannelNumber.Value].Add(current2.FilePath.Value);
						}
					}
					if (current2.BusType.Value == BusType.Bt_LIN)
					{
						if (!dictionary2.ContainsKey(current2.ChannelNumber.Value))
						{
							dictionary2.Add(current2.ChannelNumber.Value, new HashSet<string>());
							dictionary2[current2.ChannelNumber.Value].Add(current2.FilePath.Value);
						}
						else
						{
							if (dictionary2[current2.ChannelNumber.Value].Contains(current2.FilePath.Value))
							{
								continue;
							}
							dictionary2[current2.ChannelNumber.Value].Add(current2.FilePath.Value);
						}
					}
					if (flag || BusType.Bt_FlexRay != current2.BusType.Value)
					{
						uint[] array = null;
						if (current2.BusType.Value == BusType.Bt_CAN)
						{
							array = conversionParameters.CanChannelMapping;
						}
						else if (current2.BusType.Value == BusType.Bt_LIN)
						{
							array = conversionParameters.LinChannelMapping;
						}
						else if (current2.BusType.Value == BusType.Bt_FlexRay)
						{
							array = conversionParameters.FlexRayChannelMapping;
						}
						uint num3 = current2.ChannelNumber.Value;
						if (conversionParameters.UseChannelMapping && array != null && (long)array.Length >= (long)((ulong)current2.ChannelNumber.Value))
						{
							num3 = array[(int)((UIntPtr)(current2.ChannelNumber.Value - 1u))];
						}
						if (num3 > 0u)
						{
							string text2;
							if (current2.IsAUTOSARFile && current2.BusType.Value == BusType.Bt_CAN && FileConversionHelper.UseArxmlToDBCConversion(conversionParameters))
							{
								string path3 = FileSystemServices.GenerateUniqueDbcFileName(Path.GetFileNameWithoutExtension(current2.FilePath.Value), current2.NetworkName.Value, num2);
								text2 = Path.Combine(destinationPath, path3);
							}
							else
							{
								text2 = FileSystemServices.GetAbsolutePath(current2.FilePath.Value, configurationFolder);
							}
							if (File.Exists(text2))
							{
								if (!FileConversionHelper.UseSigLogINIForDBConfig(conversionParameters))
								{
									if (current2.BusType.Value == BusType.Bt_Ethernet)
									{
										base.AddCommandLineArgument(string.Format("\"{0}={1};{2}\"", LTLUtil.GetChannelString(current2.BusType.Value, num3), text2, current2.NetworkName.Value));
									}
									else if (current2.BusType.Value == BusType.Bt_FlexRay)
									{
										if (num3 == 256u)
										{
											uint channelNumber = 1u;
											uint channelNumber2 = 2u;
											if (conversionParameters.UseChannelMapping && array != null && array.Length >= 2)
											{
												channelNumber = array[0];
												channelNumber2 = array[1];
											}
											base.AddCommandLineArgument(string.Format("\"{0}={1};{2}\"", LTLUtil.GetChannelString(current2.BusType.Value, channelNumber), text2, current2.NetworkName.Value));
											base.AddCommandLineArgument(string.Format("\"{0}={1};{2}\"", LTLUtil.GetChannelString(current2.BusType.Value, channelNumber2), text2, current2.NetworkName.Value));
										}
										else
										{
											base.AddCommandLineArgument(string.Format("\"{0}={1};{2}\"", LTLUtil.GetChannelString(current2.BusType.Value, num3), text2, current2.NetworkName.Value));
										}
									}
									else if (current2.IsAUTOSARFile && current2.BusType.Value == BusType.Bt_CAN && !FileConversionHelper.UseArxmlToDBCConversion(conversionParameters))
									{
										base.AddCommandLineArgument(string.Format("\"{0}={1};{2}\"", LTLUtil.GetChannelString(current2.BusType.Value, num3), text2, current2.NetworkName.Value));
									}
									else
									{
										base.AddCommandLineArgument(string.Format("\"{0}={1}\"", LTLUtil.GetChannelString(current2.BusType.Value, num3), text2));
									}
								}
								list.Add(new CLexport.SiglogIniParameter(text2, current2.BusType.Value, num3, current2.NetworkName.Value, current2.CPType.Value != CPType.None));
							}
						}
					}
					num2++;
				}
				this.CreateSiglogINIfile(conversionParameters, destinationPath, list);
			}
			int num4 = 10000;
			CLexportValueParser progressIndicatorValueParser = new CLexportValueParser(clfFile, num4);
			pi.SetMinMax(0, num4);
			base.RunAsynchronousWithProgressBar(pi, progressIndicatorValueParser, processExitedDelegate);
			errorText = "";
			if (base.LastExitCode != 0)
			{
				errorText = this.GetLastGinErrorCodeString();
				return false;
			}
			return true;
		}

		private string GetAbsoluteIniPathFromExecDirectory(string iniFileName)
		{
			string directoryName = Path.GetDirectoryName(Application.ExecutablePath);
			return Path.Combine(directoryName, iniFileName);
		}

		private void CreateSiglogINIfile(FileConversionParameters conversionParameters, string destinationPath, List<CLexport.SiglogIniParameter> mdfDBList)
		{
			if (!FileConversionHelper.UseSigLogINIForDBConfig(conversionParameters))
			{
				return;
			}
			IniConfigSource iniConfigSource = new IniConfigSource();
			string value = "";
			string mDFVersionString = FileConversionHelper.GetMDFVersionString(conversionParameters);
			if (FileConversionHelper.UseMDFCompatibilityMode(conversionParameters))
			{
				value = "1";
			}
			IConfig config = iniConfigSource.AddConfig("MDF");
			int num = GlobalOptionsManager.GlobalOptions.MDFCompressionLevel;
			if (num == 2)
			{
				num = 6;
			}
			else if (num == 3)
			{
				num = 9;
			}
			else if (num != 1)
			{
				num = 0;
			}
			config.Set("Compression", num);
			config.Set("Version", mDFVersionString);
			if (GlobalOptionsManager.GlobalOptions.SortMDF3Files)
			{
				config.Set("SortMDF3", 1);
			}
			if (conversionParameters.MDF3SignalFormat == FileConversionMDF3SignalFormat.Name)
			{
				config.Set("MDF3SignalNameFormat", 0);
			}
			else if (conversionParameters.MDF3SignalFormat == FileConversionMDF3SignalFormat.MessageAndName)
			{
				config.Set("MDF3SignalNameFormat", 1);
			}
			if (conversionParameters.MDF3SignalFormat >= FileConversionMDF3SignalFormat.MessageAndName && conversionParameters.MDF3SignalFormatDelimiter != Vocabulary.DefaultSignalDelimiter)
			{
				config.Set("MDF3SignalNameDelimiter", conversionParameters.MDF3SignalFormatDelimiter);
			}
			config = iniConfigSource.AddConfig("MDF 3.0 Compatibility");
			config.Set("DropNonTrivialValueTables", value);
			config.Set("DropAllValueTables", value);
			config.Set("DropTxtConversionBlocks", value);
			config.Set("WriteSignalOrEventNameToComment", value);
			int num2 = 1;
			foreach (CLexport.SiglogIniParameter current in mdfDBList)
			{
				if (current.IsXCP)
				{
					uint num3 = current.Channel;
					if (conversionParameters.UseChannelMapping)
					{
						num3 = this.LookupDBChannelFromChannelMapping(conversionParameters, current.BusType, num3);
					}
					if (num3 > 0u)
					{
						config = iniConfigSource.AddConfig("DB" + num2++);
						config.Set("Path", current.DBPath);
						if (!string.IsNullOrEmpty(current.NetworkName))
						{
							config.Set("Network", current.NetworkName);
							if (num3 == 256u)
							{
								config.Set("Bus", "*");
								config.Set("Channels", "");
							}
							else
							{
								config.Set("Bus", "");
								config.Set("Channels", num3.ToString());
							}
						}
						else
						{
							config.Set("Network", "");
							config.Set("Bus", "");
							config.Set("Channels", num3.ToString());
						}
					}
				}
			}
			if (num2 <= mdfDBList.Count)
			{
				foreach (CLexport.SiglogIniParameter current2 in mdfDBList)
				{
					if (!current2.IsXCP)
					{
						uint num4 = current2.Channel;
						if (conversionParameters.UseChannelMapping)
						{
							num4 = this.LookupDBChannelFromChannelMapping(conversionParameters, current2.BusType, num4);
						}
						if (num4 > 0u)
						{
							config = iniConfigSource.AddConfig("DB" + num2++);
							config.Set("Path", current2.DBPath);
							if (!string.IsNullOrEmpty(current2.NetworkName))
							{
								config.Set("Network", current2.NetworkName);
								if (num4 == 256u)
								{
									config.Set("Bus", "*");
									config.Set("Channels", "");
								}
								else
								{
									config.Set("Bus", "");
									config.Set("Channels", num4.ToString());
								}
							}
							else
							{
								config.Set("Network", "");
								config.Set("Bus", "");
								config.Set("Channels", num4.ToString());
							}
						}
					}
				}
			}
			this.clExportSiglogIniPath = Path.Combine(destinationPath, "siglog_config.ini");
			iniConfigSource.Save(new StreamWriter(this.clExportSiglogIniPath, false, Encoding.Default));
		}

		private string CreateSiglogParameter(FileConversionParameters conversionParameters)
		{
			string str = "SigLogParams=\"Format=";
			if (conversionParameters.DestinationFormat == FileConversionDestFormat.HDF5)
			{
				str += "HDF5";
			}
			else
			{
				if (conversionParameters.DestinationFormat != FileConversionDestFormat.MAT)
				{
					return "";
				}
				str += "MATLAB";
			}
			str += " Suffix=";
			str += FileConversionHelper.GetConfiguredDestinationFormatExtension(conversionParameters).TrimStart(new char[]
			{
				'.'
			});
			int num = GlobalOptionsManager.GlobalOptions.HDF5CompressionLevel;
			if (num == 2)
			{
				num = 6;
			}
			else if (num == 3)
			{
				num = 9;
			}
			else if (num != 1)
			{
				num = 0;
			}
			str += " Compression=";
			str += num.ToString();
			if (GlobalOptionsManager.GlobalOptions.HDF5WriteUnlimitedSignalNames)
			{
				str += " WriteUnlimitedSignalNameLength=1";
			}
			if (conversionParameters.WriteRawValues)
			{
				str = " WriteRawValues=1";
			}
			return str + "\"";
		}

		private uint LookupDBChannelFromChannelMapping(FileConversionParameters conversionParameters, BusType type, uint channel)
		{
			if (type == BusType.Bt_CAN)
			{
				if ((ulong)channel < (ulong)((long)(conversionParameters.CanChannelMapping.Length - 1)))
				{
					return conversionParameters.CanChannelMapping[(int)((UIntPtr)(channel - 1u))];
				}
			}
			else if (type == BusType.Bt_LIN)
			{
				if ((ulong)channel < (ulong)((long)(conversionParameters.LinChannelMapping.Length - 1)))
				{
					return conversionParameters.LinChannelMapping[(int)((UIntPtr)(channel - 1u))];
				}
			}
			else if (type == BusType.Bt_FlexRay && (ulong)channel < (ulong)((long)(conversionParameters.FlexRayChannelMapping.Length - 1)))
			{
				return conversionParameters.FlexRayChannelMapping[(int)((UIntPtr)(channel - 1u))];
			}
			return channel;
		}

		private void AddStartTimeArgument(ConversionJob job, bool addLength = false)
		{
			if (job != null && job.ExtractStart != default(DateTime))
			{
				string str = job.ExtractStart.ToString("dd.MM.yyyy'/'HH:mm:ss");
				base.AddCommandLineArgument("Start=" + str);
				if (addLength && job.ExtractTList != null && job.ExtractTList.Count > 0)
				{
					using (List<Tuple<int, int>>.Enumerator enumerator = job.ExtractTList.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							Tuple<int, int> current = enumerator.Current;
							if (current.Item2 > 0)
							{
								base.AddCommandLineArgument(string.Format("T1={0} T2={1}", current.Item1, current.Item2));
							}
						}
						return;
					}
				}
				if (addLength && job.ExtractT2 > 0)
				{
					base.AddCommandLineArgument(string.Format("T1={0} T2={1}", job.ExtractT1, job.ExtractT2));
				}
			}
		}

		private void AddChannelMapping(FileConversionParameters conversionParameters, ILoggerSpecifics loggerSpecifics)
		{
			int num = 1;
			while ((long)num <= Math.Min((long)conversionParameters.CanChannelMapping.Length, (long)((ulong)(loggerSpecifics.CAN.NumberOfChannels + loggerSpecifics.CAN.NumberOfVirtualChannels))))
			{
				if ((ulong)conversionParameters.CanChannelMapping[num - 1] != (ulong)((long)num))
				{
					if (conversionParameters.CanChannelMapping[num - 1] != 0u)
					{
						base.AddCommandLineArgument(string.Format(Vocabulary.CANChannelName, num) + ":" + string.Format(Vocabulary.CANChannelName, conversionParameters.CanChannelMapping[num - 1]));
					}
					else
					{
						base.AddCommandLineArgument(string.Format(Vocabulary.CANChannelName, num) + ":NULL");
					}
				}
				num++;
			}
			int num2 = 1;
			while ((long)num2 <= Math.Min((long)conversionParameters.LinChannelMapping.Length, (long)((ulong)(loggerSpecifics.LIN.NumberOfChannels + Constants.MaximumNumberOfLINprobeChannels))))
			{
				if ((ulong)conversionParameters.LinChannelMapping[num2 - 1] != (ulong)((long)num2))
				{
					if (conversionParameters.LinChannelMapping[num2 - 1] != 0u)
					{
						base.AddCommandLineArgument(string.Format(Resources.LINChannelName, num2) + ":" + string.Format(Resources.LINChannelName, conversionParameters.LinChannelMapping[num2 - 1]));
					}
					else
					{
						base.AddCommandLineArgument(string.Format(Resources.LINChannelName, num2) + ":NULL");
					}
				}
				num2++;
			}
			int num3 = 1;
			while ((long)num3 <= Math.Min((long)conversionParameters.FlexRayChannelMapping.Length, (long)((ulong)loggerSpecifics.Flexray.NumberOfChannels)))
			{
				if ((ulong)conversionParameters.FlexRayChannelMapping[num3 - 1] != (ulong)((long)num3))
				{
					if (conversionParameters.FlexRayChannelMapping[num3 - 1] != 0u)
					{
						base.AddCommandLineArgument(string.Format(Resources.FlexRayShortcutForCLexport, num3) + ":" + string.Format(Resources.FlexRayShortcutForCLexport, conversionParameters.FlexRayChannelMapping[num3 - 1]));
					}
					else
					{
						base.AddCommandLineArgument(string.Format(Resources.FlexRayShortcutForCLexport, num3) + ":NULL");
					}
				}
				num3++;
			}
		}

		public override string GetLastGinErrorCodeString()
		{
			if (base.LastExitCode == 32)
			{
				return Resources.CLexportErrorInvalidInputFileFormat;
			}
			return base.GetGinErrorCodeString(base.LastExitCode);
		}

		private bool CreateExportHandlerBatch(string path, bool useDateTimeFromMeasurementStart)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("@echo off");
			stringBuilder.AppendLine("echo .");
			stringBuilder.AppendLine("echo Param 1 = original file             : %1");
			stringBuilder.AppendLine("echo Param 2 = data file folder          : %2");
			stringBuilder.AppendLine("echo Param 3 = CLF file base name        : %3");
			stringBuilder.AppendLine("echo Param 4 = car name                  : %4");
			stringBuilder.AppendLine("echo Param 5 = recording start date/time : %5");
			stringBuilder.AppendLine("echo Param 6 = recording index           : %6");
			stringBuilder.AppendLine("echo Param 7 = split file number         : %7");
			stringBuilder.AppendLine("echo Param 8 = file extension            : %8");
			stringBuilder.AppendLine("echo Param 9 = file start date/time      : %9");
			stringBuilder.AppendLine();
			this.currentTocFilePath = Path.Combine(path, CLexport.tocFileName);
			if (useDateTimeFromMeasurementStart)
			{
				stringBuilder.AppendFormat("echo %1{0}%2{0}%3{0}%4{0}%5{0}%6{0}%7{0}%8 >> \"{1}\"", CLexport.tocSeparator, this.currentTocFilePath);
			}
			else
			{
				stringBuilder.AppendFormat("echo %1{0}%2{0}%3{0}%4{0}%9{0}%6{0}%7{0}%8 >> \"{1}\"", CLexport.tocSeparator, this.currentTocFilePath);
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			try
			{
				this.clExportHandlerPath = Path.Combine(path, "RenameHandler.bat");
				File.WriteAllText(this.clExportHandlerPath, stringBuilder.ToString(), base.CurrentStdOutputTextEncoding);
			}
			catch (Exception)
			{
				this.clExportHandlerPath = string.Empty;
				InformMessageBox.Error(Resources.ErrorUnableToWriteInDestFolder);
				return false;
			}
			return true;
		}

		protected override void AsyncProcessExited(bool wasCancelled)
		{
			try
			{
				if (this.clExportHandlerPath != null)
				{
					File.Delete(this.clExportHandlerPath);
				}
				if (!string.IsNullOrEmpty(this.clExportSiglogIniPath))
				{
					File.Delete(this.clExportSiglogIniPath);
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				this.clExportHandlerPath = string.Empty;
				this.clExportSiglogIniPath = string.Empty;
				base.AsyncProcessExited(wasCancelled);
			}
		}
	}
}
