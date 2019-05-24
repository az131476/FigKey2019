using System;
using System.Collections.Generic;
using Vector.VLConfig.LoggingNavigator.Data;

namespace Vector.VLConfig.LoggingNavigator.Export
{
	public class ExportManager
	{
		public ExportManager()
		{
			ExportJob.Reset();
		}

		public void ExportMeasurements(IList<Measurement> measurementList, out IList<ExportJob> list)
		{
			List<ExportJob> list2 = new List<ExportJob>();
			foreach (Measurement current in measurementList)
			{
				if (current.Selected)
				{
					ExportJob exportJob = new ExportJob(current.Name, current.Number, current.Begin, current.End);
					foreach (LogFile current2 in current.GetLogFiles())
					{
						exportJob.AddLogFile(current2);
					}
					list2.Add(exportJob);
				}
			}
			list2.Sort();
			list = list2;
		}

		public void ExportMarker(IList<Marker> markerList, out IList<ExportJob> list, ulong before, ulong after)
		{
			List<ExportJob> list2 = new List<ExportJob>();
			foreach (Marker current in markerList)
			{
				if (current.Selected && current.SelectedForExport)
				{
					ulong begin = current.TimeSpec - before;
					ulong end = current.TimeSpec + after;
					ExportJob exportJob = new ExportJob(current.Name, current.LabelOccurenceCount, begin, end);
					foreach (LogFile current2 in current.GetLogFilesForExport())
					{
						exportJob.AddLogFile(current2);
					}
					list2.Add(exportJob);
				}
			}
			list2.Sort();
			list = list2;
		}

		public void ExportLogFiles(INavigation indexManager, IList<LogFile> logFileList, out IList<ExportJob> list)
		{
			indexManager.GetAvailableLoggerMemories();
			List<ExportJob> list2 = new List<ExportJob>();
			ExportJob exportJob = new ExportJob("Logfiles");
			ulong num = 18446744073709551615uL;
			ulong num2 = 0uL;
			foreach (LogFile current in logFileList)
			{
				if (current.Selected)
				{
					if (current.Begin < num)
					{
						num = current.Begin;
					}
					if (current.End > num2)
					{
						num2 = current.End;
					}
					exportJob.AddLogFile(current);
				}
			}
			if (num >= 18446744073709551615uL)
			{
				num = 0uL;
			}
			if (num2 <= 0uL)
			{
				num2 = 0uL;
			}
			exportJob.SetBegin(num);
			exportJob.SetEnd(num2);
			if (exportJob.LogFileList != null && exportJob.LogFileList.Count > 0)
			{
				list2.Add(exportJob);
			}
			list2.Sort();
			list = list2;
		}

		public void ExportTrigger(IList<Trigger> triggerList, out IList<ExportJob> list)
		{
			List<ExportJob> list2 = new List<ExportJob>();
			foreach (Trigger current in triggerList)
			{
				if (current.Selected && current.SelectedForExport)
				{
					Measurement measurement = current.Measurement;
					ExportJob exportJob = new ExportJob(current.Name, current.LabelOccurenceCount, measurement.Begin, measurement.End);
					foreach (LogFile current2 in measurement.GetLogFiles())
					{
						exportJob.AddLogFile(current2);
					}
					list2.Add(exportJob);
				}
			}
			list2.Sort();
			list = list2;
		}

		public void CreateSingleExportJobFromList(IList<ExportJob> list, string name, out ExportJob job)
		{
			job = new ExportJob(name);
			foreach (ExportJob current in list)
			{
				foreach (LogFile current2 in current.LogFileList)
				{
					job.AddLogFile(current2);
				}
			}
		}
	}
}
