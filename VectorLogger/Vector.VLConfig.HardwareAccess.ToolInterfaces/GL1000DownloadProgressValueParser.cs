using System;
using System.Text.RegularExpressions;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public class GL1000DownloadProgressValueParser : IProgressIndicatorValueParser
	{
		bool IProgressIndicatorValueParser.GetProgressBarValueFromString(string stringFromStdOut, ref int progressBarValue, ref string progressContextString)
		{
			progressContextString = "";
			if (stringFromStdOut == null)
			{
				return false;
			}
			progressContextString = stringFromStdOut.TrimEnd(new char[]
			{
				'\r',
				'\n'
			});
			MatchCollection matchCollection = Regex.Matches(stringFromStdOut, "[0-9]+ %");
			if (matchCollection.Count <= 0)
			{
				return false;
			}
			string text = matchCollection[matchCollection.Count - 1].ToString();
			text = text.TrimEnd(new char[]
			{
				'%'
			});
			int num;
			if (int.TryParse(text, out num))
			{
				progressBarValue = num;
				return true;
			}
			return false;
		}
	}
}
