using System;
using System.Collections.Generic;
using System.Globalization;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class WarningParser : IProgressIndicatorValueParser
	{
		private const string cTimestampWarningText = "Warning: MDFSORT: Declining time stamps ";

		private const string cErrorText = "Error:";

		private const string cWarningText = "Warning:";

		private readonly List<string> mErrors = new List<string>();

		private readonly List<string> mWarnings = new List<string>();

		public IEnumerable<string> Errors
		{
			get
			{
				return this.mErrors;
			}
		}

		public IEnumerable<string> Warnings
		{
			get
			{
				return this.mErrors;
			}
		}

		public bool GetProgressBarValueFromString(string stringFromStdOut, ref int progressBarValue, ref string progressContextString)
		{
			if (string.IsNullOrEmpty(stringFromStdOut))
			{
				return false;
			}
			if (stringFromStdOut.StartsWith("Warning: MDFSORT: Declining time stamps ", true, CultureInfo.InvariantCulture))
			{
				this.mErrors.Add(Resources.MDFFinalizeTimeStampErrorText);
			}
			else if (stringFromStdOut.StartsWith("Error:", true, CultureInfo.InvariantCulture))
			{
				this.mErrors.Add(stringFromStdOut.Substring("Error:".Length).TrimStart(new char[0]));
			}
			else if (stringFromStdOut.StartsWith("Warning:", true, CultureInfo.InvariantCulture))
			{
				this.mWarnings.Add(stringFromStdOut.Substring("Warning:".Length).TrimStart(new char[0]));
			}
			return true;
		}
	}
}
