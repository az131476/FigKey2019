using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LoggingNavigator;
using Vector.VLConfig.LoggingNavigator.Data;
using Vector.VLConfig.LoggingNavigator.Export;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	internal class NavigatorHelper
	{
		internal static bool GetConversionJobs(ILoggerDevice currentDevice, INavigationGUI navigator, FileConversionParameters conversionParameters, out List<ConversionJob> conversionJobs)
		{
			conversionJobs = new List<ConversionJob>();
			IList<ExportJob> list;
			if (!navigator.GetExportJobs(out list))
			{
				return false;
			}
			ExportJob exportJob = null;
			foreach (ExportJob current in list)
			{
				ConversionJobType conversionJobType = ConversionJobType.File;
				switch (navigator.GetExportType())
				{
				case ExportType.Measurements:
					conversionJobType = ConversionJobType.Measurement;
					break;
				case ExportType.Marker:
					conversionJobType = ConversionJobType.Marker;
					break;
				case ExportType.Trigger:
					conversionJobType = ConversionJobType.Trigger;
					break;
				}
				ConversionJob conversionJob = new ConversionJob(current.Name, conversionJobType, (uint)current.Number, current.LoggerMemNumber);
				conversionJob.FileConversionParameters = new FileConversionParameters(conversionParameters);
				if (conversionJob.FileConversionParameters.UseMinimumDigitsForTriggerIndex && conversionJob.FileConversionParameters.MinimumDigitsForTriggerIndex == 0 && conversionJobType != ConversionJobType.File)
				{
					try
					{
						conversionJob.FileConversionParameters.MinimumDigitsForTriggerIndex = currentDevice.LogFileStorage.HighestTriggerFileIndices[Convert.ToInt32(conversionJob.MemNumber) - 1].ToString(CultureInfo.InvariantCulture).Length;
					}
					catch (Exception)
					{
						conversionJob.FileConversionParameters.MinimumDigitsForTriggerIndex = Constants.DefaultMinimumDigitsForTriggerIndex;
					}
				}
				foreach (Vector.VLConfig.LoggingNavigator.Data.LogFile current2 in current.LogFileList)
				{
					if (!currentDevice.LogFileStorage.IsPrimaryFileGroupCompressed)
					{
						conversionJob.SelectedFileNames.Add(current2.Name);
						conversionJob.AddBeginEndForLogFile(current2.Name, current2.GetBeginDateTime(), current2.GetEndDateTime());
					}
					else
					{
						conversionJob.SelectedFileNames.Add(current2.Name + Vocabulary.FileExtensionDotGZ);
						conversionJob.AddBeginEndForLogFile(current2.Name, current2.GetBeginDateTime(), current2.GetEndDateTime());
					}
				}
				if (exportJob == null || exportJob.LoggerMemNumber != current.LoggerMemNumber)
				{
					exportJob = current;
				}
				conversionJob.ExtractStart = exportJob.Begin;
				conversionJob.ExtractT1 = 0;
				conversionJob.ExtractT2 = 0;
				if (conversionJobType == ConversionJobType.Marker || conversionJobType == ConversionJobType.Measurement || conversionJobType == ConversionJobType.Trigger)
				{
					if (conversionParameters.GlobalTimestamps || conversionParameters.SingleFile)
					{
						conversionJob.ExtractStart = exportJob.Begin;
						conversionJob.ExtractT1 = (int)(current.Begin - exportJob.Begin).TotalSeconds;
						conversionJob.ExtractT2 = conversionJob.ExtractT1 + (int)(current.End - current.Begin).TotalSeconds + 1;
					}
					else
					{
						conversionJob.ExtractStart = current.Begin;
						conversionJob.ExtractT1 = 0;
						conversionJob.ExtractT2 = (int)(current.End - current.Begin).TotalSeconds + 1;
					}
				}
				conversionJobs.Add(conversionJob);
			}
			if (currentDevice.LogFileStorage.NumberOfDriveRecorderFiles > 0u)
			{
				ConversionJob conversionJob2 = new ConversionJob("DriveRecorder", ConversionJobType.File, 0u);
				conversionJob2.FileConversionParameters = conversionParameters;
				IList<string> driveRecorderFileNamesOfPrimaryGroup = currentDevice.LogFileStorage.GetDriveRecorderFileNamesOfPrimaryGroup();
				foreach (string current3 in driveRecorderFileNamesOfPrimaryGroup)
				{
					conversionJob2.SelectedFileNames.Add(current3);
				}
				conversionJobs.Add(conversionJob2);
			}
			return true;
		}

		internal static string GetTotalFileSizeOfExportJobs(ILoggerDevice currentDevice, IList<ExportJob> jobs)
		{
			long num = 0L;
			HashSet<string> hashSet = new HashSet<string>();
			foreach (ExportJob current in jobs)
			{
				foreach (Vector.VLConfig.LoggingNavigator.Data.LogFile current2 in current.LogFileList)
				{
					hashSet.Add(current2.Name);
				}
			}
			foreach (ILogFile current3 in currentDevice.LogFileStorage.LogFiles)
			{
				if (hashSet.Contains(current3.DefaultName))
				{
					num += (long)((ulong)current3.FileSize);
				}
			}
			if (num < 1048576L && num > 0L)
			{
				return GUIUtil.GetSizeStringKBForBytes(num);
			}
			return GUIUtil.GetSizeStringMBForBytes(num);
		}

		internal static void OpenIndexFilesFromSource(FilemanagerGui filemanagerGui, ILoggerDevice currentDevice, INavigationGUI navigator, UserControl userControl)
		{
			if (currentDevice == null)
			{
				return;
			}
			filemanagerGui.Enabled = currentDevice.HasIndexFile;
			Cursor.Current = Cursors.WaitCursor;
			if (currentDevice.HasIndexFile)
			{
				NavigatorHelper.SetFileNavigatorLoggerType(navigator, currentDevice.LoggerType);
				navigator.RecoverMeasurements = GlobalRuntimeSettings.GetInstance().RecoverMeasurements;
				if (navigator.HasErrorInIndexFile())
				{
					InformMessageBox.Warning(Resources.WarningCorruptedIndexFile);
				}
			}
			if (Application.OpenForms == null || Application.OpenForms.Count >= 1)
			{
				ActivityIndicatorForm aiForm = new ActivityIndicatorForm();
				ProcessExitedDelegate processExitedDelegate = new ProcessExitedDelegate(aiForm.ProcessExited);
				aiForm.Text = Resources.ActivityTitle;
				aiForm.SetStatusText(Resources.ActivityTextLoading);
				System.Timers.Timer showDialogTimer = new System.Timers.Timer(500.0);
				ManualResetEvent finishedEvent = new ManualResetEvent(false);
				BackgroundWorker backgroundWorker = new BackgroundWorker();
				backgroundWorker.DoWork += delegate(object sender, DoWorkEventArgs arguments)
				{
					try
					{
						showDialogTimer.Start();
						currentDevice.LogFileStorage.UpdateFileList();
						finishedEvent.Set();
						if (currentDevice.HasIndexFile)
						{
							navigator.OpenIndexFilesFromSource(currentDevice.HardwareKey);
						}
						else
						{
							navigator.Close();
						}
					}
					catch (Exception)
					{
						finishedEvent.Set();
					}
				};
				backgroundWorker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs arguments)
				{
					showDialogTimer.Stop();
					processExitedDelegate();
				};
				showDialogTimer.Elapsed += delegate(object sender, ElapsedEventArgs arguments)
				{
					showDialogTimer.Stop();
					userControl.Invoke(new Action(delegate
					{
						try
						{
							aiForm.ShowDialog();
							aiForm.Dispose();
						}
						catch (Exception)
						{
						}
					}));
				};
				backgroundWorker.RunWorkerAsync();
				finishedEvent.WaitOne();
				Cursor.Current = Cursors.Default;
				return;
			}
			currentDevice.LogFileStorage.UpdateFileList();
			if (currentDevice.HasIndexFile)
			{
				navigator.OpenIndexFilesFromSource(currentDevice.HardwareKey);
				return;
			}
			navigator.Close();
		}

		internal static void SetFileNavigatorLoggerType(INavigationGUI navigator, Vector.VLConfig.LoggerSpecifics.LoggerType type)
		{
			switch (type)
			{
			case Vector.VLConfig.LoggerSpecifics.LoggerType.GL1000:
				navigator.SetActiveLoggerType(Vector.VLConfig.LoggingNavigator.Data.LoggerType.GL1xxx);
				return;
			case Vector.VLConfig.LoggerSpecifics.LoggerType.GL1020FTE:
				navigator.SetActiveLoggerType(Vector.VLConfig.LoggingNavigator.Data.LoggerType.GL1xxx);
				return;
			case Vector.VLConfig.LoggerSpecifics.LoggerType.GL2000:
				navigator.SetActiveLoggerType(Vector.VLConfig.LoggingNavigator.Data.LoggerType.GL2xxx);
				return;
			case Vector.VLConfig.LoggerSpecifics.LoggerType.GL3000:
				navigator.SetActiveLoggerType(Vector.VLConfig.LoggingNavigator.Data.LoggerType.GL3xxx);
				return;
			case Vector.VLConfig.LoggerSpecifics.LoggerType.GL4000:
				navigator.SetActiveLoggerType(Vector.VLConfig.LoggingNavigator.Data.LoggerType.GL4xxx);
				return;
			default:
				return;
			}
		}
	}
}
