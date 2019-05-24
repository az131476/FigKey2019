using Nini.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public class GL1000ctrl : GenericToolInterface
	{
		private readonly string SectionDriveLetters = "DriveLetters";

		private readonly string FieldLoggerDrivesCount = "LoggerDrivesCount";

		private readonly string FieldLoggerDrives = "LoggerDrives";

		private readonly string FieldRemovableDrivesCount = "RemovableDrivesCount";

		private readonly string FieldRemovableDrives = "RemovableDrives";

		private readonly string SectionLoggerStatus = "LoggerStatus";

		private readonly string SectionLogData = "LogData";

		private readonly string SectionConfigArea = "ConfigArea";

		private readonly string SectionUserData = "UserData";

		private readonly string FieldArticleNum = "ArticleNum";

		private readonly string FieldSerNum = "SerNum";

		private readonly string FieldCarName = "CarName";

		private readonly string FieldCardWriteCacheKB = "CardWriteCacheKB";

		private readonly string FieldCardSizeMB = "CardSizeMB";

		private readonly string FieldBufferSizeKB = "BufferSizeKB";

		private readonly string FieldSingleFile = "SingleFile";

		private readonly string FieldCAN1baby = "CAN1baby";

		private readonly string FieldCAN2baby = "CAN2baby";

		private readonly string FieldCompileTime = "CompileTime";

		private readonly string FieldRTSversion = "RTSversion";

		private readonly string FieldRecordingBufs = "RecordingBufs";

		private readonly string FieldTriggeredBufs = "TriggeredBufs";

		private readonly string FieldTriggerTime_Dec = "TriggerTime{0}";

		private readonly string FieldLicense_Dec = "License{0}";

		private readonly string FieldFileName = "FileName";

		private readonly string FieldDataSizeKB = "DataSizeKB";

		private readonly string OpenBufferName = "OpenBuffer";

		public GL1000ctrl()
		{
			base.FileName = "GL1000ctrl.exe";
		}

		public bool GetAvailableLoggerDrives(out IList<string> driveLettersConnectedLoggers, out IList<string> driveLettersMemoryCards)
		{
			driveLettersConnectedLoggers = new List<string>();
			driveLettersMemoryCards = new List<string>();
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-l");
			base.RunSynchronous();
			if (!base.ParseLastStdOutAsIni())
			{
				return false;
			}
			IConfig config = base.StdOutAsIniConfigSource.Configs[this.SectionDriveLetters];
			if (config != null)
			{
				if (config.Contains(this.FieldLoggerDrivesCount))
				{
					int @int = config.GetInt(this.FieldLoggerDrivesCount);
					string @string = config.GetString(this.FieldLoggerDrives);
					for (int i = 0; i < @int; i++)
					{
						if (i <= @string.Length)
						{
							string item = @string.Substring(i, 1);
							driveLettersConnectedLoggers.Add(item);
						}
					}
				}
				if (config.Contains(this.FieldRemovableDrivesCount))
				{
					int int2 = config.GetInt(this.FieldRemovableDrivesCount);
					string string2 = config.GetString(this.FieldRemovableDrives);
					List<string> list = new List<string>();
					for (int j = 0; j < int2; j++)
					{
						string item2 = string2.Substring(j, 1);
						list.Add(item2);
					}
					Dictionary<string, DriveInfo> dictionary = new Dictionary<string, DriveInfo>();
					foreach (string current in list)
					{
						string driveName = current + Path.VolumeSeparatorChar + Path.DirectorySeparatorChar;
						DriveInfo driveInfo = null;
						try
						{
							driveInfo = new DriveInfo(driveName);
						}
						catch (Exception)
						{
							continue;
						}
						if (driveInfo.DriveType == DriveType.Removable)
						{
							bool flag = false;
							try
							{
								string driveFormat = driveInfo.DriveFormat;
								if (driveFormat == "")
								{
									dictionary.Add(current, driveInfo);
								}
								else if (driveFormat == Constants.FileSystemFormatFAT || driveFormat == Constants.FileSystemFormatFAT32)
								{
									flag = true;
								}
							}
							catch (Exception)
							{
								dictionary.Add(current, driveInfo);
							}
							if (flag)
							{
								try
								{
									if (driveInfo.IsReady && FileSystemServices.IsDriveExistingAndEmptyExceptSystemVolumeInformation(current))
									{
										driveLettersMemoryCards.Add(current);
									}
								}
								catch (Exception)
								{
								}
							}
						}
					}
					foreach (KeyValuePair<string, DriveInfo> current2 in dictionary)
					{
						GL1000Infos gL1000Infos = default(GL1000Infos);
						IList<ILogFile> list2 = new List<ILogFile>();
						if (this.GetLoggerInfos(current2.Key, ref gL1000Infos, ref list2))
						{
							driveLettersMemoryCards.Add(current2.Key);
						}
					}
				}
				base.ClearIniParser();
				return true;
			}
			base.ClearIniParser();
			return false;
		}

		public bool GetLoggerInfos(string driveLetter, ref GL1000Infos gl1000Infos)
		{
			IList<ILogFile> list = null;
			return this.GetLoggerInfos(driveLetter, ref gl1000Infos, ref list);
		}

		public bool GetLoggerInfos(string driveLetter, ref GL1000Infos gl1000Infos, ref IList<ILogFile> logFiles)
		{
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-i");
			base.AddCommandLineArgument("-L " + driveLetter + ":");
			base.RunSynchronous();
			if (!base.ParseLastStdOutAsIni())
			{
				return false;
			}
			IConfig config = base.StdOutAsIniConfigSource.Configs[this.SectionLoggerStatus];
			if (config == null)
			{
				base.ClearIniParser();
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (config.Contains(this.FieldArticleNum))
			{
				stringBuilder.Append(config.GetString(this.FieldArticleNum));
				stringBuilder.Append("-");
			}
			if (config.Contains(this.FieldSerNum))
			{
				stringBuilder.Append(config.GetString(this.FieldSerNum));
			}
			gl1000Infos.serialNumber = stringBuilder.ToString();
			if (config.Contains(this.FieldCarName))
			{
				gl1000Infos.vehicleName = config.GetString(this.FieldCarName);
			}
			if (config.Contains(this.FieldCardWriteCacheKB))
			{
				gl1000Infos.cardWriteCacheKB = (uint)config.GetInt(this.FieldCardWriteCacheKB);
			}
			else
			{
				gl1000Infos.cardWriteCacheKB = 0u;
			}
			if (config.Contains(this.FieldCardSizeMB))
			{
				gl1000Infos.memCardSizeMB = (uint)config.GetInt(this.FieldCardSizeMB);
			}
			if (config.Contains(this.FieldSingleFile))
			{
				gl1000Infos.isSingleFile = (config.GetInt(this.FieldSingleFile) == 1);
			}
			if (config.Contains(this.FieldCAN1baby))
			{
				gl1000Infos.can1Baby = config.GetString(this.FieldCAN1baby);
			}
			if (config.Contains(this.FieldCAN2baby))
			{
				gl1000Infos.can2Baby = config.GetString(this.FieldCAN2baby);
			}
			if (config.Contains(this.FieldRTSversion))
			{
				gl1000Infos.firmwareVersion = config.GetString(this.FieldRTSversion);
			}
			uint num = 0u;
			if (config.Contains(this.FieldBufferSizeKB))
			{
				num = (uint)config.GetInt(this.FieldBufferSizeKB);
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			int num2 = 1;
			bool flag;
			do
			{
				flag = false;
				string key = string.Format(this.FieldLicense_Dec, num2);
				if (config.Contains(key))
				{
					flag = true;
					if (stringBuilder2.Length > 0)
					{
						stringBuilder2.Append(", ");
					}
					stringBuilder2.Append(config.GetString(key));
					num2++;
				}
			}
			while (flag);
			gl1000Infos.licenses = stringBuilder2.ToString();
			config = base.StdOutAsIniConfigSource.Configs[this.SectionConfigArea];
			if (config != null && config.Contains(this.FieldFileName))
			{
				gl1000Infos.configName = config.GetString(this.FieldFileName);
				if (!string.IsNullOrEmpty(gl1000Infos.configName) && config.Contains(this.FieldCompileTime))
				{
					string @string = config.GetString(this.FieldCompileTime);
					DateTime compileDateTime = new DateTime(0L);
					if (this.ParseDateTimeField(@string, ref compileDateTime))
					{
						gl1000Infos.compileDateTime = compileDateTime;
					}
				}
			}
			bool flag2 = false;
			config = base.StdOutAsIniConfigSource.Configs[this.SectionLogData];
			if (config != null)
			{
				if (!config.Contains(this.FieldRecordingBufs) || !config.Contains(this.FieldTriggeredBufs))
				{
					gl1000Infos.recordingBufs = 0u;
					gl1000Infos.triggeredBufs = 0u;
				}
				else
				{
					gl1000Infos.recordingBufs = (uint)config.GetInt(this.FieldRecordingBufs);
					gl1000Infos.triggeredBufs = (uint)config.GetInt(this.FieldTriggeredBufs);
				}
				if (config.Contains(this.FieldDataSizeKB))
				{
					gl1000Infos.dataSizeKB = (uint)config.GetInt(this.FieldDataSizeKB);
				}
				gl1000Infos.freeSpaceMB = gl1000Infos.memCardSizeMB - (gl1000Infos.triggeredBufs + gl1000Infos.recordingBufs) * num / 1024u;
				if (logFiles != null)
				{
					logFiles.Clear();
					if (gl1000Infos.triggeredBufs + gl1000Infos.recordingBufs == 0u)
					{
						base.ClearIniParser();
						return true;
					}
					if (gl1000Infos.isSingleFile)
					{
						uint num3 = (gl1000Infos.triggeredBufs + gl1000Infos.recordingBufs) * num;
						if (config.Contains(this.FieldDataSizeKB))
						{
							num3 = (uint)config.GetInt(this.FieldDataSizeKB);
						}
						DateTime timestamp = default(DateTime);
						if (gl1000Infos.triggeredBufs > 0u && !this.ParseTriggerEntry(gl1000Infos.triggeredBufs, config, out timestamp))
						{
							flag2 = true;
						}
						if (!flag2)
						{
							LogFile logFile = new LogFile("PermanentLogging", num3 * 1024u, timestamp);
							logFile.IsConvertible = true;
							logFile.TypeName = Resources.FileManagerColFileTypeLogData;
							logFiles.Add(logFile);
						}
					}
					else
					{
						for (uint num4 = 1u; num4 <= gl1000Infos.triggeredBufs + gl1000Infos.recordingBufs; num4 += 1u)
						{
							bool flag3 = false;
							DateTime timestamp2 = default(DateTime);
							if (num4 > gl1000Infos.triggeredBufs && gl1000Infos.recordingBufs > 0u)
							{
								flag3 = true;
							}
							else if (!this.ParseTriggerEntry(num4, config, out timestamp2))
							{
								flag2 = true;
								break;
							}
							string defaultName;
							if (flag3)
							{
								defaultName = this.OpenBufferName;
							}
							else
							{
								defaultName = string.Format(Path.GetFileNameWithoutExtension(Constants.DefaultRawLogDataFileName) + "{0:D3}", num4);
							}
							LogFile logFile2 = new LogFile(defaultName, num * 1024u, timestamp2);
							logFile2.IsConvertible = true;
							logFile2.TypeName = Resources.FileManagerColFileTypeLogData;
							logFiles.Add(logFile2);
						}
					}
				}
			}
			config = base.StdOutAsIniConfigSource.Configs[this.SectionUserData];
			if (config != null && config.Contains(this.FieldFileName))
			{
				gl1000Infos.userDataFileName = config.GetString(this.FieldFileName);
			}
			base.ClearIniParser();
			return !flag2;
		}

		public bool IsLoggerOfSubType1020FTE(string driveLetter)
		{
			GL1000Infos gL1000Infos = default(GL1000Infos);
			if (this.GetLoggerInfos(driveLetter, ref gL1000Infos) && !string.IsNullOrEmpty(gL1000Infos.serialNumber))
			{
				string[] array = gL1000Infos.serialNumber.Split(new char[]
				{
					'-'
				});
				if (array.Count<string>() > 1 && array[1].Substring(0, 1) == "2")
				{
					return true;
				}
			}
			return false;
		}

		public bool DownloadFile(string driveLetter, FileConversionParameters conversionParameters, string destFilePath, IProgressIndicator progressIndicator, ProcessExitedDelegate processExitedDelegate, out string errorText)
		{
			GL1000DownloadProgressValueParser progressIndicatorValueParser = new GL1000DownloadProgressValueParser();
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-R");
			base.AddCommandLineArgument(string.Format("\"{0}\"", destFilePath));
			base.AddCommandLineArgument("-o");
			base.AddCommandLineArgument("-v");
			if (!string.IsNullOrEmpty(driveLetter))
			{
				base.AddCommandLineArgument("-L " + driveLetter + ":");
			}
			if (conversionParameters.SuppressBufferConcat)
			{
				base.AddCommandLineArgument("-0");
			}
			progressIndicator.SetMinMax(0, 100);
			base.RunAsynchronousWithProgressBar(progressIndicator, progressIndicatorValueParser, processExitedDelegate);
			if (base.LastExitCode != 0)
			{
				errorText = base.GetGinErrorCodeString(base.LastExitCode);
				return false;
			}
			errorText = "";
			return true;
		}

		public bool ClearMemoryCard(string driveLetter, bool deleteWithConfigAndUserData, out string errorText)
		{
			FileSystemServices.TryDeleteSystemVolumeInformation(driveLetter);
			errorText = "";
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-v");
			base.AddCommandLineArgument("-n");
			base.AddCommandLineArgument("-d");
			if (deleteWithConfigAndUserData)
			{
				base.AddCommandLineArgument("-DC");
				base.AddCommandLineArgument("-DU");
			}
			if (!string.IsNullOrEmpty(driveLetter))
			{
				base.AddCommandLineArgument("-L " + driveLetter + ":");
			}
			base.RunSynchronous();
			if (base.LastExitCode != 0)
			{
				errorText = base.GetGinErrorCodeString(base.LastExitCode);
				return false;
			}
			return true;
		}

		public bool WriteConfiguration(string driveLetter, string codFilePath, out string errorText)
		{
			errorText = "";
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-v");
			base.AddCommandLineArgument("-n");
			base.AddCommandLineArgument(string.Format("-WC \"{0}\"", codFilePath));
			if (!string.IsNullOrEmpty(driveLetter))
			{
				base.AddCommandLineArgument("-L " + driveLetter + ":");
			}
			base.RunSynchronous();
			if (base.LastExitCode != 0)
			{
				errorText = base.GetGinErrorCodeString(base.LastExitCode);
				return false;
			}
			return true;
		}

		public void WriteUserData(string driveLetter, string filePath)
		{
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-v");
			base.AddCommandLineArgument("-n");
			base.AddCommandLineArgument(string.Format("-WU \"{0}\"", filePath));
			if (!string.IsNullOrEmpty(driveLetter))
			{
				base.AddCommandLineArgument("-L " + driveLetter + ":");
			}
			base.RunSynchronous();
			if (base.LastExitCode != 0)
			{
				throw new IOException(base.GetGinErrorCodeString(base.LastExitCode, base.LastStdErr), base.LastExitCode);
			}
		}

		public void ReadUserData(string driveLetter, string destFilePath)
		{
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-v");
			base.AddCommandLineArgument("-o");
			base.AddCommandLineArgument(string.Format("-RU \"{0}\"", destFilePath));
			if (!string.IsNullOrEmpty(driveLetter))
			{
				base.AddCommandLineArgument("-L " + driveLetter + ":");
			}
			base.RunSynchronous();
			if (base.LastExitCode != 0)
			{
				throw new IOException(base.GetGinErrorCodeString(base.LastExitCode, base.LastStdErr), base.LastExitCode);
			}
		}

		public void ReadConfiguration(string driveLetter, string destFilePath)
		{
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-v");
			base.AddCommandLineArgument("-o");
			base.AddCommandLineArgument(string.Format("-RC \"{0}\"", destFilePath));
			if (!string.IsNullOrEmpty(driveLetter))
			{
				base.AddCommandLineArgument("-L " + driveLetter + ":");
			}
			base.RunSynchronous();
			if (base.LastExitCode != 0)
			{
				throw new IOException(base.GetGinErrorCodeString(base.LastExitCode, base.LastStdErr), base.LastExitCode);
			}
		}

		public bool SetRealTimeClock(string driveLetter, out string errorText)
		{
			errorText = "";
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-v");
			base.AddCommandLineArgument("-n");
			base.AddCommandLineArgument("-T");
			base.RunSynchronous();
			if (base.LastExitCode != 0)
			{
				errorText = base.GetGinErrorCodeString(base.LastExitCode);
				return false;
			}
			return true;
		}

		public bool SetVehicleName(string name, out string errorText)
		{
			errorText = "";
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-v");
			base.AddCommandLineArgument("-n");
			base.AddCommandLineArgument(string.Format("-N \"{0}\"", name));
			base.RunSynchronous();
			if (base.LastExitCode != 0)
			{
				errorText = base.GetGinErrorCodeString(base.LastExitCode);
				return false;
			}
			return true;
		}

		private bool ParseTriggerEntry(uint triggerPos, IConfig config, out DateTime timestamp)
		{
			timestamp = default(DateTime);
			string key = string.Format(this.FieldTriggerTime_Dec, triggerPos);
			if (!config.Contains(key))
			{
				return false;
			}
			string @string = config.GetString(key);
			if (string.IsNullOrEmpty(@string))
			{
				return false;
			}
			string[] array = @string.Split(new char[]
			{
				' ',
				':',
				'.'
			});
			if (array.Count<string>() != 6)
			{
				return false;
			}
			try
			{
				timestamp = new DateTime(Convert.ToInt32(array[2]), Convert.ToInt32(array[1]), Convert.ToInt32(array[0]), Convert.ToInt32(array[3]), Convert.ToInt32(array[4]), Convert.ToInt32(array[5]));
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		private bool ParseDateTimeField(string dateTimeString, ref DateTime parsedDateTimeValue)
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
	}
}
