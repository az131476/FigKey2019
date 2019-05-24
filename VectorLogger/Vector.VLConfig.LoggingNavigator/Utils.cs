using System;
using Vector.VLConfig.LoggingNavigator.Data;

namespace Vector.VLConfig.LoggingNavigator
{
	public class Utils
	{
		public static string ConvertDateTimeToFolderName(DateTime timestamp)
		{
			string format = "yyyy-MM-dd_HH-mm-ss";
			return timestamp.ToString(format);
		}

		public static void CalculateAndUpdateMarkerOffsets(ulong beforeValue, string before_unit, ulong afterValue, string after_unit, out ulong before, out ulong after)
		{
			if (before_unit == "h")
			{
				before = beforeValue * TimeSpec.CurrentTickRate * 3600uL;
			}
			else if (before_unit == "min")
			{
				before = beforeValue * TimeSpec.CurrentTickRate * 60uL;
			}
			else if (before_unit == "s")
			{
				before = beforeValue * TimeSpec.CurrentTickRate;
			}
			else if (before_unit == "ms")
			{
				before = beforeValue * TimeSpec.CurrentTickRate / 1000uL;
			}
			else
			{
				before = 0uL;
			}
			if (after_unit == "h")
			{
				after = afterValue * TimeSpec.CurrentTickRate * 3600uL;
				return;
			}
			if (after_unit == "min")
			{
				after = afterValue * TimeSpec.CurrentTickRate * 60uL;
				return;
			}
			if (after_unit == "s")
			{
				after = afterValue * TimeSpec.CurrentTickRate;
				return;
			}
			if (after_unit == "ms")
			{
				after = afterValue * TimeSpec.CurrentTickRate / 1000uL;
				return;
			}
			after = 0uL;
		}

		public static bool LogFileMatchesMarker(Marker m, LogFile l, ulong offset_before, ulong offset_after)
		{
			ulong num = m.TimeSpec - offset_before;
			ulong num2 = m.TimeSpec + offset_after;
			return m.LoggerMemNumber.Equals(l.LoggerMemNumber) && ((m.TimeSpec >= l.Begin && num <= l.End) || (num2 >= l.Begin && m.TimeSpec <= l.End));
		}
	}
}
