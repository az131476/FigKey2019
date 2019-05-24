using System;
using System.Collections.Generic;
using System.Globalization;

namespace Vector.VLConfig.GeneralUtil.QuickView
{
	public class OfflineSourceConfig
	{
		private ulong mTimePosToSetAfterMeasurementEndNs;

		public static uint DefaultPlaybackDuration = 15u;

		public readonly string StartTimeStamp = "00:00:00.000";

		public uint PlaybackDuration
		{
			get;
			set;
		}

		public List<Tuple<string, string>> OfflineSourceFiles
		{
			get;
			private set;
		}

		public TimeSectionType TimeSectionType
		{
			get;
			private set;
		}

		public string EndTimeStamp
		{
			get
			{
				return this.GetEndTimeStamp();
			}
		}

		public DateTime StartDateTime
		{
			get;
			private set;
		}

		public DateTime EndDateTime
		{
			get;
			private set;
		}

		public bool SetTimePosAfterMeasurementEnd
		{
			get;
			set;
		}

		public ulong TimePosToSetAfterMeasurementEndNs
		{
			get
			{
				return this.mTimePosToSetAfterMeasurementEndNs;
			}
			set
			{
				this.mTimePosToSetAfterMeasurementEndNs = value;
				this.SetTimePosAfterMeasurementEnd = true;
			}
		}

		public SelectionType SelectionType
		{
			get;
			set;
		}

		public string MeasurementName
		{
			get;
			set;
		}

		public int MeasurementNumber
		{
			get;
			set;
		}

		public string LoggerMemNumber
		{
			get;
			set;
		}

		public OfflineSourceConfig()
		{
			this.PlaybackDuration = OfflineSourceConfig.DefaultPlaybackDuration;
			this.OfflineSourceFiles = new List<Tuple<string, string>>();
			this.TimeSectionType = TimeSectionType.BreakAtTimeStamp;
			this.StartDateTime = default(DateTime);
			this.EndDateTime = default(DateTime);
			this.SetTimePosAfterMeasurementEnd = false;
			this.mTimePosToSetAfterMeasurementEndNs = 0uL;
			this.SelectionType = SelectionType.Measurement;
			this.MeasurementName = string.Empty;
			this.MeasurementNumber = 0;
			this.LoggerMemNumber = string.Empty;
		}

		public void CopyFrom(OfflineSourceConfig other)
		{
			this.PlaybackDuration = OfflineSourceConfig.DefaultPlaybackDuration;
			this.OfflineSourceFiles.Clear();
			this.TimeSectionType = TimeSectionType.BreakAtTimeStamp;
			this.StartDateTime = default(DateTime);
			this.EndDateTime = default(DateTime);
			this.SetTimePosAfterMeasurementEnd = false;
			this.mTimePosToSetAfterMeasurementEndNs = 0uL;
			this.SelectionType = SelectionType.Measurement;
			this.MeasurementName = string.Empty;
			this.MeasurementNumber = 0;
			this.LoggerMemNumber = string.Empty;
			if (other != null)
			{
				this.PlaybackDuration = other.PlaybackDuration;
				foreach (Tuple<string, string> current in other.OfflineSourceFiles)
				{
					this.OfflineSourceFiles.Add(new Tuple<string, string>(current.Item1, current.Item2));
				}
				this.TimeSectionType = other.TimeSectionType;
				this.StartDateTime = new DateTime(other.StartDateTime.Ticks);
				this.EndDateTime = new DateTime(other.EndDateTime.Ticks);
				this.SetTimePosAfterMeasurementEnd = other.SetTimePosAfterMeasurementEnd;
				this.mTimePosToSetAfterMeasurementEndNs = other.mTimePosToSetAfterMeasurementEndNs;
				this.SelectionType = other.SelectionType;
				this.MeasurementName = other.MeasurementName;
				this.MeasurementNumber = other.MeasurementNumber;
				this.LoggerMemNumber = other.LoggerMemNumber;
			}
		}

		public void SetTimeSection(DateTime startTime, DateTime endTime)
		{
			this.StartDateTime = startTime;
			this.EndDateTime = endTime.AddSeconds(1.0);
			this.TimeSectionType = TimeSectionType.IntervalBetweenTwoDateTimes;
		}

		private string GetEndTimeStamp()
		{
			TimeSpan timeSpan = new TimeSpan(0, (int)this.PlaybackDuration, 0);
			return string.Concat(new string[]
			{
				Math.Floor(timeSpan.TotalHours).ToString("00", CultureInfo.InvariantCulture),
				":",
				timeSpan.Minutes.ToString("00", CultureInfo.InvariantCulture),
				":",
				timeSpan.Seconds.ToString("00", CultureInfo.InvariantCulture),
				".",
				timeSpan.Milliseconds.ToString("000", CultureInfo.InvariantCulture)
			});
		}
	}
}
