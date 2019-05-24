using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class TimeSpec
	{
		private const double NanosecondsPerSecond = 1000000000.0;

		private ulong mValue;

		private LoggerType mCode;

		private bool mIsConverted;

		private DateTime mDateTime;

		private static DateTime BaseTime = DateTime.Parse("01 Jan 1980 00:00:00");

		private static LoggerType sActiveLoggerType = LoggerType.GL3xxx;

		private static Dictionary<LoggerType, ulong> ticks = new Dictionary<LoggerType, ulong>
		{
			{
				LoggerType.MultiLog,
				65536uL
			},
			{
				LoggerType.HLogger,
				200000uL
			},
			{
				LoggerType.GL1xxx,
				200000uL
			},
			{
				LoggerType.GL2xxx,
				1000000uL
			},
			{
				LoggerType.GL3xxx,
				1000000uL
			},
			{
				LoggerType.GL4xxx,
				1000000uL
			}
		};

		public static LoggerType ActiveLoggerType
		{
			get
			{
				return TimeSpec.sActiveLoggerType;
			}
		}

		public DateTime DateTime
		{
			get
			{
				if (!this.mIsConverted)
				{
					this.mDateTime = TimeSpec.ConvertToDateTime(this.mValue, this.mCode);
				}
				return this.mDateTime;
			}
		}

		public DateTime DateTimeTicks
		{
			get
			{
				if (!this.mIsConverted)
				{
					this.mDateTime = TimeSpec.ConvertToDateTimeTicks(this.mValue, this.mCode);
				}
				return this.mDateTime;
			}
		}

		public static ulong CurrentTickRate
		{
			get
			{
				return TimeSpec.ticks[TimeSpec.sActiveLoggerType];
			}
		}

		public ulong Value
		{
			get
			{
				return this.mValue;
			}
			set
			{
				this.mValue = value;
				this.mIsConverted = false;
			}
		}

		public ulong ValueNs
		{
			get
			{
				return Convert.ToUInt64(this.Value * (1000000000.0 / TimeSpec.CurrentTickRate));
			}
		}

		public TimeSpec(ulong value) : this(value, TimeSpec.sActiveLoggerType)
		{
		}

		public TimeSpec(DateTime time) : this(time, TimeSpec.sActiveLoggerType)
		{
		}

		private TimeSpec(ulong value, LoggerType typcode)
		{
			this.mValue = value;
			this.mCode = typcode;
			this.mIsConverted = false;
		}

		private TimeSpec(DateTime time, LoggerType typcode)
		{
			this.mDateTime = time;
			this.mValue = TimeSpec.ConvertFromDateTime(time, typcode);
			this.mCode = typcode;
			this.mIsConverted = true;
		}

		public static DateTime ConvertToDateTime(ulong value, LoggerType typcode)
		{
			ulong num = value / TimeSpec.ticks[typcode];
			DateTime result = DateTime.MinValue;
			try
			{
				result = TimeSpec.BaseTime.AddSeconds(num);
			}
			catch (ArgumentOutOfRangeException)
			{
				if (num > 0uL)
				{
					result = DateTime.MaxValue;
				}
				else
				{
					result = DateTime.MinValue;
				}
			}
			return result;
		}

		public static DateTime ConvertToDateTimeTicks(ulong value, LoggerType typcode)
		{
			long num = (long)(value / TimeSpec.ticks[typcode] * 10000000.0);
			DateTime result;
			try
			{
				result = TimeSpec.BaseTime.AddTicks(num);
			}
			catch (ArgumentOutOfRangeException)
			{
				result = ((num > 0L) ? DateTime.MaxValue : DateTime.MinValue);
			}
			return result;
		}

		public static ulong ConvertFromDateTime(DateTime time, LoggerType typcode)
		{
			ulong num = (ulong)(time - TimeSpec.BaseTime).TotalSeconds;
			return num * TimeSpec.ticks[typcode];
		}

		public static ulong ConvertFromDateTimeWithMilliseconds(DateTime time, LoggerType typcode)
		{
			ulong num = (ulong)(time - TimeSpec.BaseTime).TotalMilliseconds;
			return num * TimeSpec.ticks[typcode] / 1000uL;
		}

		public static void SetActiveLoggerType(LoggerType type)
		{
			TimeSpec.sActiveLoggerType = type;
			ulong arg_21_0 = TimeSpec.ticks[type];
			ulong arg_20_0 = TimeSpec.ticks[LoggerType.GL3xxx];
		}

		public static ulong ConvertToNanoseconds(ulong value)
		{
			return Convert.ToUInt64(value * (1000000000.0 / TimeSpec.CurrentTickRate));
		}
	}
}
