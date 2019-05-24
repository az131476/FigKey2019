using System;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal enum CANwinTimeSectionType
	{
		Entire,
		StartFromTimeStamp,
		IntervalBetweenTwoTimeStamps,
		BreakAtTimeStamp,
		BreakAtDateTime,
		StartFromDateTime,
		IntervalBetweenTwoDateTimes,
		StartFromTimeStampInSeconds,
		IntervalBetweenTwoTimeStampsInSeconds
	}
}
