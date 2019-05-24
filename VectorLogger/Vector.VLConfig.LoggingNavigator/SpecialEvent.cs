using System;

namespace Vector.VLConfig.LoggingNavigator
{
	internal class SpecialEvent
	{
		private double _Timestamp;

		private string _Label;

		private uint _Severity;

		public double Timestamp
		{
			get
			{
				return this._Timestamp;
			}
		}

		public string Label
		{
			get
			{
				return this._Label;
			}
		}

		public uint Severity
		{
			get
			{
				return this._Severity;
			}
		}

		public SpecialEvent(double timestamp, string label, uint severity)
		{
			this._Timestamp = timestamp;
			this._Label = label;
			this._Severity = severity;
		}
	}
}
