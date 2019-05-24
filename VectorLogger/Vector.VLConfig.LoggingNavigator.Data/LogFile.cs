using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class LogFile
	{
		private List<Entry> mEntryList;

		private string mPath;

		private uint mTag;

		private uint mEntries;

		private ushort mIndexFormatVersion;

		private ushort mLoggerType;

		private ushort mLoggerFirmware;

		private byte mRecordingType;

		private ulong mBegin;

		private ulong mEnd;

		private uint mId;

		private string mLoggerMemNumber;

		private bool mSelected;

		public string Path
		{
			get
			{
				return this.mPath;
			}
			set
			{
				this.mPath = value;
			}
		}

		public ulong Begin
		{
			get
			{
				return this.mBegin;
			}
			set
			{
				this.mBegin = value;
			}
		}

		public ulong End
		{
			get
			{
				return this.mEnd;
			}
			set
			{
				this.mEnd = value;
			}
		}

		public uint Tag
		{
			get
			{
				return this.mTag;
			}
			set
			{
				this.mTag = value;
			}
		}

		public uint Entries
		{
			get
			{
				return this.mEntries;
			}
			set
			{
				this.mEntries = value;
			}
		}

		public ushort IndexFormatVersion
		{
			get
			{
				return this.mIndexFormatVersion;
			}
			set
			{
				this.mIndexFormatVersion = value;
			}
		}

		public ushort LoggerType
		{
			get
			{
				return this.mLoggerType;
			}
			set
			{
				this.mLoggerType = value;
			}
		}

		public ushort LoggerFirmware
		{
			get
			{
				return this.mLoggerFirmware;
			}
			set
			{
				this.mLoggerFirmware = value;
			}
		}

		public byte RecordingType
		{
			get
			{
				return this.mRecordingType;
			}
			set
			{
				this.mRecordingType = value;
			}
		}

		public bool IsPermanent
		{
			get
			{
				return this.RecordingType != 0;
			}
		}

		public string LoggerMemNumber
		{
			get
			{
				return this.mLoggerMemNumber;
			}
			set
			{
				this.mLoggerMemNumber = value;
			}
		}

		public uint ID
		{
			get
			{
				return this.mId;
			}
			set
			{
				this.mId = value;
			}
		}

		public string Name
		{
			get
			{
				return "D" + this.LoggerMemNumber + "F" + this.ID.ToString();
			}
		}

		public string Name_old
		{
			get
			{
				return "Data" + this.LoggerMemNumber + "F" + this.ID.ToString();
			}
		}

		public string IndexFilePath
		{
			get;
			set;
		}

		public bool Selected
		{
			get
			{
				return this.mSelected;
			}
			set
			{
				this.mSelected = value;
			}
		}

		public LogFile()
		{
			this.mSelected = true;
			this.mEntryList = new List<Entry>();
		}

		public DateTime GetBeginDateTime()
		{
			return new TimeSpec(this.Begin).DateTime;
		}

		public DateTime GetEndDateTime()
		{
			return new TimeSpec(this.End).DateTime;
		}

		public void AddEntry(Entry entry)
		{
			this.mEntryList.Add(entry);
		}

		public IList<Entry> GetEntries()
		{
			return this.mEntryList;
		}
	}
}
