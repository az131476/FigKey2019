using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class IndexFileCollection
	{
		private List<string> mFilePaths;

		private List<Measurement> mMeasurements;

		public static byte[] SignatureBytes = Encoding.UTF8.GetBytes("GiNindex");

		public static readonly Regex regex = new Regex(".*D(\\d+)F(\\d*)X?.(GLX|IDX)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		private string mLoggerMemNumber;

		public string LoggerMemNumber
		{
			get
			{
				return this.mLoggerMemNumber;
			}
		}

		public string GetLastFilePath
		{
			get
			{
				if (this.mFilePaths != null && this.mFilePaths.Count > 0)
				{
					return this.mFilePaths[this.mFilePaths.Count - 1];
				}
				return "";
			}
		}

		public IndexFileCollection(string path)
		{
			this.mMeasurements = new List<Measurement>();
			this.mLoggerMemNumber = IndexFileCollection.ParseLoggerMemNumberFromFilename(path);
			this.mFilePaths = new List<string>();
			this.AddIndexFilePath(path);
		}

		public void AddIndexFilePath(string path)
		{
			this.mFilePaths.Add(path);
		}

		public bool HasMeasurements()
		{
			return this.mMeasurements != null && this.mMeasurements.Count > 0;
		}

		public void AddNewMeasurement(LogFile firstLogFile, int measurementCountTotal)
		{
			this.mMeasurements.Add(new Measurement(measurementCountTotal, firstLogFile));
		}

		public void CloseMeasurement(ulong end)
		{
			if (this.mMeasurements.Count > 0)
			{
				this.mMeasurements[this.mMeasurements.Count - 1].Close(end);
				if (!this.mMeasurements[this.mMeasurements.Count - 1].ValidateLogfiles())
				{
					this.mMeasurements.RemoveAt(this.mMeasurements.Count - 1);
				}
			}
		}

		public Measurement GetCurrentMeasurement()
		{
			if (this.mMeasurements.Count > 0)
			{
				return this.mMeasurements[this.mMeasurements.Count - 1];
			}
			return null;
		}

		public bool FinalizeMeasurementsOnMemory(bool recoverMeasurements)
		{
			bool flag = false;
			for (int i = 0; i < this.mMeasurements.Count; i++)
			{
				if (!this.mMeasurements[i].Closed)
				{
					bool flag2 = i + 1 >= this.mMeasurements.Count;
					if (flag2 || (this.mMeasurements[i].Begin != this.mMeasurements[i + 1].Begin && recoverMeasurements))
					{
						this.mMeasurements[i].Close(!flag2 || flag);
						flag = true;
					}
				}
			}
			return flag;
		}

		public void FinalizeMeasurementNumbering(ref int begin)
		{
			for (int i = 0; i < this.mMeasurements.Count; i++)
			{
				if (this.mMeasurements[i].Closed)
				{
					this.mMeasurements[i].Number = begin + i;
				}
			}
			begin += this.mMeasurements.Count;
		}

		public void AppendLogFile(LogFile logFile, int measurementCountTotal)
		{
			if (this.mMeasurements.Count > 0)
			{
				this.mMeasurements[this.mMeasurements.Count - 1].AppendLogFile(logFile);
				return;
			}
			this.AddNewMeasurement(logFile, measurementCountTotal);
		}

		public IList<Measurement> GetMeasurements()
		{
			return this.mMeasurements;
		}

		public static string ParseLoggerMemNumberFromFilename(string path)
		{
			MatchCollection matchCollection = IndexFileCollection.regex.Matches(path);
			if (matchCollection.Count > 0 && matchCollection[0].Groups.Count > 1)
			{
				return matchCollection[0].Groups[1].ToString();
			}
			return "1";
		}

		public static void Reset()
		{
		}

		public bool Contains(string indexFilePath)
		{
			return this.mFilePaths.Any((string fp) => string.Equals(fp, indexFilePath, StringComparison.OrdinalIgnoreCase));
		}
	}
}
