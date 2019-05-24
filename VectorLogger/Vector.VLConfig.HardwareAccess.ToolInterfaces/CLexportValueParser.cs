using System;
using System.IO;
using System.Text.RegularExpressions;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class CLexportValueParser : IProgressIndicatorValueParser
	{
		private string clfFileName;

		private string lastMemoryName;

		private int memoriesExpected;

		private int memoriesProcessed;

		private int maxProgressBarValue;

		private int stepSizeProgressBarPerMemory;

		private bool isFileContentsTableStarted;

		private bool isFileContentsTableFinished;

		private Regex contentsTableEntry;

		private Regex contentsTableEntry2;

		private Regex contentsTableEntry3;

		private Regex memoryMessage;

		private Regex percentageMessage;

		private static readonly string Contents = "Contents";

		private static readonly string EntriesPostfix = " entries";

		public CLexportValueParser(string clfFilePath, int maxProgressBarValue)
		{
			this.maxProgressBarValue = maxProgressBarValue;
			this.stepSizeProgressBarPerMemory = 1;
			this.clfFileName = Path.GetFileName(clfFilePath);
			this.lastMemoryName = string.Empty;
			this.memoriesExpected = 0;
			this.memoriesProcessed = 0;
			this.isFileContentsTableStarted = false;
			this.isFileContentsTableFinished = false;
			this.contentsTableEntry = new Regex("Memory \\w+\\s*:\\s*\\d+ entries");
			this.contentsTableEntry2 = new Regex("Trigger \\s*:\\s*");
			this.contentsTableEntry3 = new Regex("End \\s*:\\s*");
			this.memoryMessage = new Regex("Memory .*" + CLexportValueParser.EntriesPostfix);
			this.percentageMessage = new Regex("[0-9]+ %");
		}

		bool IProgressIndicatorValueParser.GetProgressBarValueFromString(string stringFromStdOut, ref int progressBarValue, ref string progressContextString)
		{
			if (stringFromStdOut == null)
			{
				return false;
			}
			if (!this.isFileContentsTableFinished)
			{
				if (this.memoriesExpected == 0 && !this.isFileContentsTableStarted && stringFromStdOut.Length > CLexportValueParser.Contents.Length && stringFromStdOut.Substring(0, CLexportValueParser.Contents.Length) == CLexportValueParser.Contents)
				{
					this.isFileContentsTableStarted = true;
				}
				if (this.isFileContentsTableStarted)
				{
					MatchCollection matchCollection = this.contentsTableEntry.Matches(stringFromStdOut);
					if (matchCollection.Count > 0)
					{
						this.memoriesExpected += matchCollection.Count;
					}
					else
					{
						MatchCollection matchCollection2 = this.contentsTableEntry2.Matches(stringFromStdOut);
						MatchCollection matchCollection3 = this.contentsTableEntry3.Matches(stringFromStdOut);
						if (matchCollection2.Count > 0 || matchCollection3.Count > 0)
						{
							return false;
						}
						if (this.memoriesExpected == 0)
						{
							return false;
						}
						this.isFileContentsTableFinished = true;
						this.stepSizeProgressBarPerMemory = this.maxProgressBarValue / this.memoriesExpected;
					}
				}
				progressContextString = string.Format(Resources.LoadingCLFFileForConversion, this.clfFileName, this.memoriesExpected);
				progressBarValue = -1;
				return true;
			}
			MatchCollection matchCollection4 = this.percentageMessage.Matches(stringFromStdOut);
			if (matchCollection4.Count <= 0)
			{
				return false;
			}
			string text = matchCollection4[matchCollection4.Count - 1].ToString();
			text = text.TrimEnd(new char[]
			{
				'%'
			});
			int num;
			if (!int.TryParse(text, out num))
			{
				return false;
			}
			MatchCollection matchCollection5 = this.memoryMessage.Matches(stringFromStdOut);
			if (matchCollection5.Count > 0)
			{
				string text2 = matchCollection5[matchCollection5.Count - 1].ToString();
				int startIndex = text2.Length - CLexportValueParser.EntriesPostfix.Length;
				text2 = text2.Remove(startIndex);
				if (string.IsNullOrEmpty(this.lastMemoryName))
				{
					this.lastMemoryName = text2;
					progressBarValue = 0;
				}
				else if (this.lastMemoryName != text2)
				{
					this.memoriesProcessed++;
					this.lastMemoryName = text2;
					progressBarValue = (int)((double)this.maxProgressBarValue * ((double)this.memoriesProcessed * 100.0 + (double)num) / ((double)this.memoriesExpected * 100.0));
				}
				else
				{
					progressBarValue = (int)((double)this.maxProgressBarValue * ((double)this.memoriesProcessed * 100.0 + (double)num) / ((double)this.memoriesExpected * 100.0));
				}
				progressBarValue = Math.Min(progressBarValue, this.maxProgressBarValue);
				progressContextString = string.Format(Resources.ConvertFileNumFromTotalFiles, new object[]
				{
					this.clfFileName,
					this.memoriesProcessed,
					this.memoriesExpected,
					text2
				});
				return true;
			}
			return false;
		}
	}
}
