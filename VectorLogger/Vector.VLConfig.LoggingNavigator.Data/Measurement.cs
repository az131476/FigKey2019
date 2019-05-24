using System;
using System.Collections.Generic;
using System.Linq;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class Measurement
	{
		private ulong mBegin;

		private ulong mEnd;

		private List<LogFile> mLogFiles;

		private bool mIsPermanent;

		private int mNumber;

		private bool mSelected;

		private bool mIsClosed;

		private bool mIsForcedClose;

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

		public bool IsPermanent
		{
			get
			{
				return this.mIsPermanent;
			}
			set
			{
				this.mIsPermanent = value;
			}
		}

		public int Number
		{
			get
			{
				return this.mNumber;
			}
			set
			{
				this.mNumber = value;
			}
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

		public bool Closed
		{
			get
			{
				return this.mIsClosed;
			}
		}

		public bool IsForcedClose
		{
			get
			{
				return this.mIsForcedClose;
			}
		}

		public string Name
		{
			get
			{
				return "Measurement " + this.Number + this.PermanentString;
			}
		}

		public string PermanentString
		{
			get
			{
				if (this.mIsPermanent)
				{
					return " (Permanent)";
				}
				return " (Triggered)";
			}
		}

		public string LogFileNames
		{
			get
			{
				string text = "";
				if (this.mLogFiles.Count <= 10)
				{
					foreach (LogFile current in this.mLogFiles)
					{
						text = text + current.Name + ", ";
					}
					if (text.Length > 0)
					{
						text = text.Remove(text.Length - 2);
					}
				}
				else
				{
					text = string.Concat(new string[]
					{
						this.mLogFiles[0].Name,
						", ",
						this.mLogFiles[1].Name,
						", ..., ",
						this.mLogFiles[this.mLogFiles.Count - 1].Name
					});
				}
				return text;
			}
		}

		public Measurement(int nr, LogFile logFile)
		{
			this.mLogFiles = new List<LogFile>();
			this.mSelected = true;
			this.mIsClosed = false;
			this.mIsForcedClose = false;
			this.Begin = logFile.Begin;
			this.IsPermanent = (logFile.RecordingType != 0);
			this.mNumber = nr + 1;
			this.AppendLogFile(logFile);
		}

		public void Close(ulong end)
		{
			this.mEnd = end;
			this.mIsClosed = true;
		}

		public void Close(bool forcedClose)
		{
			this.mIsForcedClose = forcedClose;
			if (this.mLogFiles.Count > 0)
			{
				this.Close(this.mLogFiles[this.mLogFiles.Count - 1].End);
			}
		}

		public void AppendLogFile(LogFile logFile)
		{
			this.mLogFiles.Add(logFile);
		}

		public bool ValidateLogfiles()
		{
			FileAccessManager instance = FileAccessManager.GetInstance();
			for (int i = this.mLogFiles.Count - 1; i >= 0; i--)
			{
				LogFile logFile = this.mLogFiles.ElementAt(i);
				if (!instance.ExistsOnFilesystem(logFile))
				{
					this.mLogFiles.RemoveAt(i);
				}
			}
			if (this.mLogFiles.Count > 0)
			{
				if (this.Begin < this.mLogFiles[0].Begin)
				{
					this.Begin = this.mLogFiles[0].Begin;
				}
				if (this.End > this.mLogFiles[this.mLogFiles.Count - 1].End)
				{
					this.End = this.mLogFiles[this.mLogFiles.Count - 1].End;
				}
			}
			return this.mLogFiles.Count > 0;
		}

		public IList<LogFile> GetLogFiles()
		{
			return this.mLogFiles;
		}
	}
}
