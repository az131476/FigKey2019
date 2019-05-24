using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class LogFileInfo
	{
		private bool isFirstUpdate;

		public string InfoLoggerType
		{
			get;
			private set;
		}

		public string InfoSerialNumber
		{
			get;
			private set;
		}

		public string InfoNavJobType
		{
			get;
			private set;
		}

		public string InfoNavNumber
		{
			get;
			private set;
		}

		public string InfoNavMarkerName
		{
			get;
			private set;
		}

		public string InfoNavTriggerName
		{
			get;
			private set;
		}

		public string InfoFirstRecordingDateTime
		{
			get;
			private set;
		}

		public string InfoFirstYear
		{
			get;
			private set;
		}

		public string InfoFirstMonth
		{
			get;
			private set;
		}

		public string InfoFirstDay
		{
			get;
			private set;
		}

		public string InfoFirstHour
		{
			get;
			private set;
		}

		public string InfoFirstMinute
		{
			get;
			private set;
		}

		public string InfoFirstSecond
		{
			get;
			private set;
		}

		public string InfoGlobalCarname
		{
			get;
			private set;
		}

		public string InfoLastRecordingDateTime
		{
			get;
			private set;
		}

		public string InfoLoggerMemoryNumber
		{
			get;
			private set;
		}

		public string InfoOriginalFilePath
		{
			get;
			set;
		}

		public string InfoOriginalFilenameWithoutExtension
		{
			get;
			set;
		}

		public string InfoPartNumber
		{
			get;
			private set;
		}

		public string InfoCarname
		{
			get;
			private set;
		}

		public string InfoRecordingDateTime
		{
			get;
			private set;
		}

		public string InfoRecordingIndex
		{
			get;
			private set;
		}

		public string InfoYear
		{
			get;
			private set;
		}

		public string InfoMonth
		{
			get;
			private set;
		}

		public string InfoDay
		{
			get;
			private set;
		}

		public string InfoHour
		{
			get;
			private set;
		}

		public string InfoMinute
		{
			get;
			private set;
		}

		public string InfoSecond
		{
			get;
			private set;
		}

		public LogFileInfo(ILoggerSpecifics loggerSpecifics = null, string serialNumber = null, string carNameFromDevice = null, string clfFilePath = null, ConversionJob job = null)
		{
			this.InfoNavJobType = "";
			this.InfoNavNumber = "";
			this.InfoNavMarkerName = "";
			this.InfoNavTriggerName = "";
			this.InfoFirstRecordingDateTime = "";
			this.InfoFirstYear = "";
			this.InfoFirstMonth = "";
			this.InfoFirstDay = "";
			this.InfoFirstHour = "";
			this.InfoFirstMinute = "";
			this.InfoFirstSecond = "";
			this.InfoGlobalCarname = "";
			this.InfoLastRecordingDateTime = "";
			this.InfoLoggerMemoryNumber = "";
			if (loggerSpecifics != null)
			{
				this.InfoLoggerType = loggerSpecifics.Name;
				if (loggerSpecifics.DataStorage.NumberOfMemories > 1u && clfFilePath != null)
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(clfFilePath);
					this.InfoLoggerMemoryNumber = fileNameWithoutExtension.Substring(fileNameWithoutExtension.Length - 1, 1);
				}
			}
			this.InfoSerialNumber = serialNumber;
			if (carNameFromDevice != null)
			{
				this.InfoGlobalCarname = carNameFromDevice;
			}
			if (job != null)
			{
				this.InfoNavJobType = job.Type.ToString();
				if (job.Type == ConversionJobType.Marker)
				{
					this.InfoNavMarkerName = job.Name.Replace(" ", "_");
					this.InfoNavNumber = job.Number.ToString();
				}
				else if (job.Type == ConversionJobType.Measurement)
				{
					this.InfoNavNumber = job.Number.ToString();
				}
				else if (job.Type == ConversionJobType.Trigger)
				{
					this.InfoNavTriggerName = job.Name.Replace(" ", "_");
					this.InfoNavNumber = job.Number.ToString();
				}
			}
			this.isFirstUpdate = true;
		}

		public bool UpdateSingleLogFileInfo(string tocLine)
		{
			string[] array = tocLine.Split(new string[]
			{
				CLexport.tocSeparator
			}, StringSplitOptions.RemoveEmptyEntries);
			if (array.Count<string>() != 8)
			{
				return false;
			}
			this.InfoOriginalFilePath = array[0].Trim(new char[]
			{
				'"'
			});
			this.InfoOriginalFilenameWithoutExtension = Path.GetFileNameWithoutExtension(this.InfoOriginalFilePath);
			Match match = Regex.Match(this.InfoOriginalFilenameWithoutExtension, "\\.Part\\d+");
			if (match.Success)
			{
				this.InfoPartNumber = match.Value.Trim(new char[]
				{
					'.'
				});
				this.InfoOriginalFilenameWithoutExtension = this.InfoOriginalFilenameWithoutExtension.Replace(match.Value, "");
			}
			else
			{
				this.InfoPartNumber = string.Empty;
			}
			this.InfoCarname = array[3].Trim(new char[]
			{
				'"'
			});
			this.InfoRecordingDateTime = array[4].Trim(new char[]
			{
				'"'
			});
			this.InfoRecordingIndex = array[5].Trim(new char[]
			{
				'"'
			}).Replace("F", "");
			this.InfoYear = this.InfoRecordingDateTime.Substring(0, 4);
			this.InfoMonth = this.InfoRecordingDateTime.Substring(5, 2);
			this.InfoDay = this.InfoRecordingDateTime.Substring(8, 2);
			this.InfoHour = this.InfoRecordingDateTime.Substring(11, 2);
			this.InfoMinute = this.InfoRecordingDateTime.Substring(14, 2);
			this.InfoSecond = this.InfoRecordingDateTime.Substring(17, 2);
			if (this.isFirstUpdate)
			{
				this.InfoGlobalCarname = this.InfoCarname;
				this.InfoFirstRecordingDateTime = this.InfoRecordingDateTime;
				this.InfoFirstYear = this.InfoYear;
				this.InfoFirstMonth = this.InfoMonth;
				this.InfoFirstDay = this.InfoDay;
				this.InfoFirstHour = this.InfoHour;
				this.InfoFirstMinute = this.InfoMinute;
				this.InfoFirstSecond = this.InfoSecond;
				this.isFirstUpdate = false;
			}
			this.InfoLastRecordingDateTime = this.InfoRecordingDateTime;
			return true;
		}

		public void FillWithExampleData()
		{
			DateTime now = DateTime.Now;
			this.InfoLoggerType = "GL2000";
			this.InfoSerialNumber = "028090-123456";
			this.InfoNavJobType = "Measurement";
			this.InfoNavNumber = "12";
			this.InfoNavMarkerName = "Key1";
			this.InfoNavTriggerName = "Key2";
			this.InfoFirstRecordingDateTime = string.Format(Resources.FormatDateTimeForFilenames, new object[]
			{
				now.Year,
				now.Month,
				now.Day,
				now.Hour,
				now.Minute,
				now.Second
			});
			this.InfoFirstYear = string.Format("{0:d4}", now.Year);
			this.InfoFirstMonth = string.Format("{0:d2}", now.Month);
			this.InfoFirstDay = string.Format("{0:d2}", now.Day);
			this.InfoFirstHour = string.Format("{0:d2}", now.Hour);
			this.InfoFirstMinute = string.Format("{0:d2}", now.Minute);
			this.InfoFirstSecond = string.Format("{0:d2}", now.Second);
			this.InfoGlobalCarname = "MyCar";
			this.InfoLastRecordingDateTime = this.InfoFirstRecordingDateTime;
			this.InfoLoggerMemoryNumber = "1";
			this.InfoOriginalFilePath = string.Concat(new object[]
			{
				"D",
				Path.VolumeSeparatorChar,
				Path.DirectorySeparatorChar,
				"LogData",
				Path.DirectorySeparatorChar,
				"Data1F001.asc"
			});
			this.InfoOriginalFilenameWithoutExtension = "Data1F001";
			this.InfoPartNumber = string.Empty;
			this.InfoCarname = "MyCar";
			this.InfoRecordingDateTime = this.InfoFirstRecordingDateTime;
			this.InfoRecordingIndex = "005";
			this.InfoYear = string.Format("{0:d4}", now.Year);
			this.InfoMonth = string.Format("{0:d2}", now.Month);
			this.InfoDay = string.Format("{0:d2}", now.Day);
			this.InfoHour = string.Format("{0:d2}", now.Hour);
			this.InfoMinute = string.Format("{0:d2}", now.Minute);
			this.InfoSecond = string.Format("{0:d2}", now.Second);
		}
	}
}
