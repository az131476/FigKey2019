using System;
using System.Collections.Generic;
using System.IO;
using Vector.VLConfig.LoggingNavigator.Data;

namespace Vector.VLConfig.LoggingNavigator.Export
{
	public class ExportJob : IComparable
	{
		private string mName;

		private IList<LogFile> mLogFileList;

		private int mNr;

		private ushort mLoggerType;

		private ushort mLoggerFirmware;

		private string mLoggerMemNumber;

		private HashSet<string> mIndexFileSet;

		private static Dictionary<string, int> sUsedNames = new Dictionary<string, int>();

		public DateTime Begin
		{
			get;
			set;
		}

		public DateTime End
		{
			get;
			set;
		}

		public string Name
		{
			get
			{
				return this.mName;
			}
			set
			{
				if (ExportJob.sUsedNames.ContainsKey(value))
				{
					Dictionary<string, int> dictionary;
					(dictionary = ExportJob.sUsedNames)[value] = dictionary[value] + 1;
				}
				else
				{
					ExportJob.sUsedNames.Add(value, 0);
				}
				this.mName = value;
			}
		}

		public int NameOccurenceCount
		{
			get
			{
				if (ExportJob.sUsedNames.ContainsKey(this.mName))
				{
					return ExportJob.sUsedNames[this.mName];
				}
				return 0;
			}
		}

		public ushort LoggerType
		{
			get
			{
				return this.mLoggerType;
			}
		}

		public ushort LoggerFirmware
		{
			get
			{
				return this.mLoggerFirmware;
			}
		}

		public string LoggerMemNumber
		{
			get
			{
				return this.mLoggerMemNumber;
			}
		}

		public int Number
		{
			get
			{
				return this.mNr;
			}
		}

		public IList<LogFile> LogFileList
		{
			get
			{
				return this.mLogFileList;
			}
		}

		public HashSet<string> IndexFileList
		{
			get
			{
				return this.mIndexFileSet;
			}
		}

		public static void Reset()
		{
			ExportJob.sUsedNames.Clear();
		}

		public ExportJob(string name) : this(name, 1, 0uL, 0uL)
		{
		}

		public ExportJob(string name, int nr, ulong begin, ulong end)
		{
			this.Name = name;
			this.mLogFileList = new List<LogFile>();
			this.mNr = nr;
			this.mLoggerType = 0;
			this.mLoggerFirmware = 0;
			this.mLoggerMemNumber = "";
			this.mIndexFileSet = new HashSet<string>();
			this.SetBegin(begin);
			this.SetEnd(end);
		}

		public void SetBegin(ulong begin)
		{
			this.Begin = new TimeSpec(begin).DateTime;
		}

		public void SetEnd(ulong end)
		{
			this.End = new TimeSpec(end).DateTime;
		}

		public void AddLogFile(LogFile file)
		{
			if (this.mLogFileList.Count < 1)
			{
				this.mLoggerType = file.LoggerType;
				this.mLoggerFirmware = file.LoggerFirmware;
				this.mLoggerMemNumber = file.LoggerMemNumber;
			}
			this.mLogFileList.Add(file);
			this.AddIndexFileName(file.IndexFilePath);
		}

		private void AddIndexFileName(string indexFilePath)
		{
			this.mIndexFileSet.Add(Path.GetFileName(indexFilePath));
		}

		private string CreateNameFromDate(string prefix, ulong begin, ulong end)
		{
			string text = prefix + "_";
			DateTime dateTime = new TimeSpec(begin).DateTime;
			DateTime dateTime2 = new TimeSpec(end).DateTime;
			if (dateTime.Date == dateTime2.Date)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					dateTime.Date.ToString("d"),
					" _ ",
					dateTime.ToString("HHmm"),
					"-",
					dateTime2.ToString("HHmm")
				});
			}
			else
			{
				text = text + dateTime.Date.ToString("d") + "-" + dateTime2.Date.ToString("d");
			}
			return text;
		}

		public int CompareTo(object obj)
		{
			if (obj == null)
			{
				return 1;
			}
			if (!(obj is ExportJob))
			{
				return 1;
			}
			ExportJob exportJob = (ExportJob)obj;
			if (this.LoggerMemNumber != exportJob.LoggerMemNumber)
			{
				return this.LoggerMemNumber.CompareTo(exportJob.LoggerMemNumber);
			}
			if (this.Begin.CompareTo(exportJob.Begin) == 0)
			{
				return this.mNr.CompareTo(exportJob.mNr);
			}
			return this.Begin.CompareTo(exportJob.Begin);
		}
	}
}
