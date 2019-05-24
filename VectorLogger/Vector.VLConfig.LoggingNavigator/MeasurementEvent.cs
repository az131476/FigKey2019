using System;

namespace Vector.VLConfig.LoggingNavigator
{
	internal class MeasurementEvent
	{
		private double _TimestampBegin;

		private double _TimestampEnd;

		private MeasurementType _type;

		public double TimestampBegin
		{
			get
			{
				return this._TimestampBegin;
			}
		}

		public double TimestampEnd
		{
			get
			{
				return this._TimestampEnd;
			}
		}

		public MeasurementType Type
		{
			get
			{
				return this._type;
			}
		}

		public MeasurementEvent(double timestampBegin, double timestampEnd, MeasurementType type)
		{
			this._TimestampBegin = timestampBegin;
			this._TimestampEnd = timestampEnd;
			this._type = type;
		}
	}
}
