using System;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class Lrf_decValueParser : IProgressIndicatorValueParser
	{
		private string lastFileName;

		private string entries;

		private int filesProcessed;

		private int filesExpected;

		private string lastProgressContextString;

		public Lrf_decValueParser(int numberOfExpectedFiles)
		{
			this.lastFileName = string.Empty;
			this.entries = string.Empty;
			this.lastProgressContextString = string.Empty;
			this.filesExpected = numberOfExpectedFiles;
			this.filesProcessed = 0;
		}

		bool IProgressIndicatorValueParser.GetProgressBarValueFromString(string stringFromStdOut, ref int progressBarValue, ref string progressContextString)
		{
			if (stringFromStdOut == null)
			{
				return false;
			}
			string text = string.Empty;
			if (stringFromStdOut.Contains("Handling file "))
			{
				int num = stringFromStdOut.IndexOf("'");
				if (num > 0)
				{
					int num2 = stringFromStdOut.IndexOf("'", num + 1);
					if (num2 > num)
					{
						text = stringFromStdOut.Substring(num + 1, num2 - num - 1);
						num = stringFromStdOut.IndexOf("... ", num2 + 1);
						if (num > num2)
						{
							this.entries = stringFromStdOut.Substring(num + 4).TrimEnd(new char[]
							{
								'\r',
								'\n'
							});
						}
					}
				}
			}
			if (string.IsNullOrEmpty(this.lastFileName) && !string.IsNullOrEmpty(text))
			{
				this.lastFileName = text;
				progressContextString = string.Format(Resources.DecodeFileNumFromTotalFiles, new object[]
				{
					this.filesProcessed,
					this.filesExpected,
					text,
					this.entries
				});
				this.lastProgressContextString = progressContextString;
				return true;
			}
			if (!string.IsNullOrEmpty(text) && text != this.lastFileName)
			{
				this.filesProcessed++;
				progressBarValue = this.filesProcessed;
				this.lastFileName = text;
				progressContextString = string.Format(Resources.DecodeFileNumFromTotalFiles, new object[]
				{
					this.filesProcessed,
					this.filesExpected,
					text,
					this.entries
				});
				this.lastProgressContextString = progressContextString;
			}
			else
			{
				progressContextString = string.Format(Resources.DecodeFileNumFromTotalFiles, new object[]
				{
					this.filesProcessed,
					this.filesExpected,
					this.lastFileName,
					this.entries
				});
				progressBarValue = this.filesProcessed;
			}
			return true;
		}
	}
}
