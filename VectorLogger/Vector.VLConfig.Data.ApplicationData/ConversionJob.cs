using System;
using System.Collections.Generic;

namespace Vector.VLConfig.Data.ApplicationData
{
	public class ConversionJob
	{
		private Dictionary<string, Tuple<DateTime, DateTime>> listOfBeginEndForLogFiles;

		public string Name
		{
			get;
			set;
		}

		public ConversionJobType Type
		{
			get;
			set;
		}

		public DateTime StartTime
		{
			get;
			set;
		}

		public uint Number
		{
			get;
			set;
		}

		public string MemNumber
		{
			get;
			set;
		}

		public HashSet<string> SelectedFileNames
		{
			get;
			set;
		}

		public FileConversionParameters FileConversionParameters
		{
			get;
			set;
		}

		public DateTime ExtractStart
		{
			get;
			set;
		}

		public int ExtractT1
		{
			get;
			set;
		}

		public int ExtractT2
		{
			get;
			set;
		}

		public List<Tuple<int, int>> ExtractTList
		{
			get;
			set;
		}

		public Dictionary<string, Tuple<DateTime, DateTime>> ListOfBeginEndForLogFiles
		{
			get
			{
				return this.listOfBeginEndForLogFiles;
			}
		}

		public ConversionJob()
		{
			this.SelectedFileNames = new HashSet<string>();
			this.ExtractStart = default(DateTime);
			this.ExtractT2 = 0;
		}

		public ConversionJob(string name, ConversionJobType type, uint number) : this(name, type, number, "")
		{
		}

		public ConversionJob(string name, ConversionJobType type, uint number, string MemNumber)
		{
			this.Name = name;
			this.Type = type;
			this.Number = number;
			this.SelectedFileNames = new HashSet<string>();
			this.ExtractStart = default(DateTime);
			this.ExtractT1 = 0;
			this.ExtractT2 = 0;
			this.ExtractTList = null;
			this.MemNumber = MemNumber;
			this.listOfBeginEndForLogFiles = new Dictionary<string, Tuple<DateTime, DateTime>>();
		}

		public void AddBeginEndForLogFile(string name, DateTime begin, DateTime end)
		{
			if (!this.listOfBeginEndForLogFiles.ContainsKey(name))
			{
				this.listOfBeginEndForLogFiles.Add(name, new Tuple<DateTime, DateTime>(begin, end));
			}
		}

		public Tuple<DateTime, DateTime> GetBeginEndForLogFile(string name)
		{
			if (this.listOfBeginEndForLogFiles.ContainsKey(name))
			{
				return this.listOfBeginEndForLogFiles[name];
			}
			return null;
		}

		public void AddBeginEndForLogFilesFromList(Dictionary<string, Tuple<DateTime, DateTime>> list)
		{
			foreach (KeyValuePair<string, Tuple<DateTime, DateTime>> current in list)
			{
				if (!this.listOfBeginEndForLogFiles.ContainsKey(current.Key))
				{
					this.listOfBeginEndForLogFiles.Add(current.Key, current.Value);
				}
			}
		}
	}
}
