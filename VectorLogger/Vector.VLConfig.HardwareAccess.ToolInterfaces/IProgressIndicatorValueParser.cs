using System;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public interface IProgressIndicatorValueParser
	{
		bool GetProgressBarValueFromString(string stringFromStdOut, ref int progressBarValue, ref string progressContextString);
	}
}
