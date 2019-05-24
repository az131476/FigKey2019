using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class Marker : Entry
	{
		private uint mId;

		private string mLabel;

		private string mName;

		private string mLoggerMemNumber;

		private int mLabelOccurenceCount;

		private uint mType;

		private uint mInstance;

		private bool mSelected;

		private bool mSelectedForExport;

		private IList<LogFile> mLogFilesExport;

		private IdManager mIdManager;

		public uint Type
		{
			get
			{
				return this.mType;
			}
		}

		public uint Instance
		{
			get
			{
				return this.mInstance;
			}
		}

		public uint ID
		{
			get
			{
				return this.mId;
			}
		}

		public override string Label
		{
			get
			{
				return this.mLabel;
			}
		}

		public override string Name
		{
			get
			{
				return this.mName;
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

		public bool SelectedForExport
		{
			get
			{
				return this.mSelectedForExport;
			}
			set
			{
				this.mSelectedForExport = value;
			}
		}

		public string LoggerMemNumber
		{
			get
			{
				return this.mLoggerMemNumber;
			}
		}

		public string LabelAndOccurences
		{
			get
			{
				if (this.mLabelOccurenceCount <= 0)
				{
					return this.Label;
				}
				return string.Concat(new object[]
				{
					this.Label,
					" (",
					this.LabelOccurenceCount,
					")"
				});
			}
		}

		public int LabelOccurenceCount
		{
			get
			{
				return this.mLabelOccurenceCount;
			}
		}

		public override string Tooltip
		{
			get
			{
				TimeSpec timeSpec = new TimeSpec(base.TimeSpec);
				return string.Concat(new object[]
				{
					"Marker: ",
					this.Name,
					" - ",
					timeSpec.DateTime
				});
			}
		}

		public double Timestamp
		{
			get
			{
				return base.TimeSpec;
			}
		}

		public Marker(uint id, uint tag, uint filepos, ulong timespec, string loggerMemNumber, IdManager idManager) : base(tag, filepos, timespec)
		{
			this.mId = id;
			this.mLoggerMemNumber = loggerMemNumber;
			this.mSelectedForExport = true;
			this.mLogFilesExport = new List<LogFile>();
			this.mIdManager = idManager;
			this.mType = this.mId;
			this.mInstance = this.mId;
			int markerIdCounterBits = this.mIdManager.GetMarkerIdCounterBits();
			if (markerIdCounterBits > 0 && markerIdCounterBits < 24)
			{
				this.mType &= 16777215u >> markerIdCounterBits;
				this.mInstance >>= 24 - markerIdCounterBits;
				this.SetLabel(this.mType, this.mInstance);
			}
			else
			{
				this.mLabel = Marker.GetInternalLabel(id);
				this.mLabelOccurenceCount = this.mIdManager.GetMarkerLabelOccurenceCount(id);
			}
			string text;
			if (this.mIdManager.GetMarkerNameByID(this.Type, out text))
			{
				this.mName = text;
				return;
			}
			this.mName = "Marker (" + this.Type.ToString("X") + ")";
		}

		private static string GetInternalLabel(uint id)
		{
			string str = (char)(65u + id % 26u) + ((id >= 26u) ? string.Concat(id / 26u) : "");
			return "M" + str;
		}

		private void SetLabel(uint type, uint instance)
		{
			if (type > 0u)
			{
				type -= 1u;
			}
			string str = (char)(65u + type % 26u) + ((type >= 26u) ? string.Concat(type / 26u) : "");
			this.mLabel = "M" + str;
			this.mLabelOccurenceCount = (int)instance;
		}

		public void AddLogFileForExport(LogFile log)
		{
			this.mLogFilesExport.Add(log);
		}

		public void ClearLogFilesForExport()
		{
			this.mLogFilesExport.Clear();
		}

		public IList<LogFile> GetLogFilesForExport()
		{
			return this.mLogFilesExport;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is Marker))
			{
				return false;
			}
			Marker marker = obj as Marker;
			return this.Timestamp == marker.Timestamp && this.Label == marker.Label;
		}

		public override int GetHashCode()
		{
			return this.Timestamp.GetHashCode() * 17 + this.Label.GetHashCode();
		}
	}
}
