using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Vector.VLConfig.CANwinAccess.COM;
using Vector.VLConfig.CANwinAccess.Data;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;

namespace Vector.VLConfig.CANwinAccess
{
	internal class CreateOfflineConfigJob : GenericBackgroundWorkerJob
	{
		private const int WaitCycleMs = 100;

		private const int MaxWaitMs = 10000;

		private const int MaxWaitForMeasurementEndMs = 10000;

		private readonly CANwinQuickViewData mConfigData;

		private readonly ManualResetEvent mMeasurementEndEvent = new ManualResetEvent(false);

		public CreateOfflineConfigJob(CANwinQuickViewData configData)
		{
			this.mConfigData = configData;
		}

		protected override void OnDoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			CreateOfflineConfigJob createOfflineConfigJob = e.Argument as CreateOfflineConfigJob;
			if (backgroundWorker == null || createOfflineConfigJob != this || this.mConfigData == null)
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.InternalError, "Invalid Job initialization.");
				return;
			}
			backgroundWorker.ReportProgress(0);
			if (!this.CheckConfigurationData(e))
			{
				return;
			}
			backgroundWorker.ReportProgress(5);
			if (!this.CloseAllProcessesExceptOne(this.mConfigData.ServerType, e))
			{
				return;
			}
			backgroundWorker.ReportProgress(15);
			using (CANwinApplication cANwinApplication = CANwinApplication.Create(this.mConfigData.ServerType))
			{
				if (cANwinApplication == null || !cANwinApplication.IsInitialized)
				{
					e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, string.Format(Resources.ErrorNoApplicationRegistered, this.mConfigData.ProductName, Constants.ProductMinVersionName));
					return;
				}
				backgroundWorker.ReportProgress(50);
				this.ShowCANwinWindow(false);
				if (!cANwinApplication.CheckVersion())
				{
					e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, string.Format(Resources.ErrorMinimumProductVersion, this.mConfigData.ProductName, Constants.ProductMinVersionName));
					this.ShowCANwinWindow(true);
					return;
				}
				backgroundWorker.ReportProgress(55);
				if (!this.CanDiscardConfig(cANwinApplication, e))
				{
					this.ShowCANwinWindow(true);
					return;
				}
				backgroundWorker.ReportProgress(60);
				string arg;
				if (!this.LoadTemplate(cANwinApplication, out arg))
				{
					e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, string.Format(Resources.ErrorUnableToLoadTemplate, arg));
					this.ShowCANwinWindow(true);
					return;
				}
				backgroundWorker.ReportProgress(65);
				if (!this.CleanupTemplate(cANwinApplication))
				{
					e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, string.Format(Resources.ErrorUnableToPrepareTemplate, this.mConfigData.ProductName));
					this.ShowCANwinWindow(true);
					return;
				}
				backgroundWorker.ReportProgress(70);
				if (!this.PrepareConfigFolder(cANwinApplication))
				{
					e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, string.Format(Resources.ErrorUnableToPrepareConfigFolder, this.mConfigData.ProductName, this.mConfigData.ConfigFolder));
					this.ShowCANwinWindow(true);
					return;
				}
				if (!this.InitialSaveConfig(cANwinApplication))
				{
					e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, string.Format(Resources.ErrorUnableToSaveConfig, this.mConfigData.ProductName));
					this.ShowCANwinWindow(true);
					return;
				}
				backgroundWorker.ReportProgress(75);
				if (!this.ConfigureOfflineConfig(cANwinApplication, e, backgroundWorker))
				{
					this.ShowCANwinWindow(true);
					return;
				}
				if (!cANwinApplication.Configuration.Save(null, false))
				{
					e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, string.Format(Resources.ErrorUnableToSaveConfig, this.mConfigData.ProductName));
					this.ShowCANwinWindow(true);
					return;
				}
				this.ShowCANwinWindow(true);
				if (!this.StartMeasurementAndWaitForEnd(cANwinApplication, e))
				{
					return;
				}
				if (!this.SetTimePosition(cANwinApplication))
				{
					e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.InternalError, "");
					return;
				}
			}
			backgroundWorker.ReportProgress(100);
			if (e.Result == null)
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Success, "");
			}
		}

		private bool CloseAllProcessesExceptOneOfType(CANwinServerType serverTypeToKeep, DoWorkEventArgs e)
		{
			bool flag = true;
			foreach (CANwinServerType cANwinServerType in Enum.GetValues(typeof(CANwinServerType)))
			{
				IEnumerable<Process> runningProcesses = this.GetRunningProcesses(cANwinServerType);
				flag &= this.CloseProcesses(cANwinServerType, runningProcesses, e, (cANwinServerType == serverTypeToKeep) ? 1 : 0);
			}
			return flag;
		}

		private bool CloseAllProcessesExceptOne(CANwinServerType canwinServerType, DoWorkEventArgs e)
		{
			IEnumerable<Process> runningProcesses = this.GetRunningProcesses(canwinServerType);
			return this.CloseProcesses(canwinServerType, runningProcesses, e, 1);
		}

		private IEnumerable<Process> GetRunningProcesses(CANwinServerType canwinServerType)
		{
			List<Process> list = new List<Process>();
			list.AddRange(ProcessUtils.GetRunningProcesses(CreateOfflineConfigJob.GetProcessName(canwinServerType, false)));
			list.AddRange(ProcessUtils.GetRunningProcesses(CreateOfflineConfigJob.GetProcessName(canwinServerType, true)));
			return list;
		}

		private bool CloseProcesses(CANwinServerType canwinServerType, IEnumerable<Process> processes, DoWorkEventArgs e, int numberOfProcessesToKeepAlive = 0)
		{
			List<Process> list = (from process in processes
			where !process.HasExited
			select process).ToList<Process>();
			if (list.Count <= numberOfProcessesToKeepAlive)
			{
				return true;
			}
			int num = list.Count - numberOfProcessesToKeepAlive;
			for (int i = 0; i < num; i++)
			{
				bool flag = this.CloseProcess(canwinServerType, list);
				if (!flag)
				{
					break;
				}
			}
			if (list.Count((Process process) => !process.HasExited) > numberOfProcessesToKeepAlive)
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, string.Format(Resources.ErrorUnableToCloseProcesses, CreateOfflineConfigJob.GetProductName(canwinServerType)));
				return false;
			}
			return true;
		}

		private bool CloseProcess(CANwinServerType canwinServerType, List<Process> processes)
		{
			int num = processes.Count((Process process) => !process.HasExited);
			using (CANwinApplication cANwinApplication = CANwinApplication.Create(canwinServerType))
			{
				bool flag;
				bool flag2;
				if (!cANwinApplication.Measurement.GetRunning(out flag) || !cANwinApplication.Configuration.GetModified(out flag2))
				{
					bool result = false;
					return result;
				}
				if (this.IsCANwinQuickViewConfig(cANwinApplication))
				{
					if (flag && !cANwinApplication.Measurement.Stop())
					{
						bool result = false;
						return result;
					}
					if (flag2 && !cANwinApplication.Configuration.SetModified(false))
					{
						bool result = false;
						return result;
					}
					if (!cANwinApplication.Quit())
					{
						bool result = false;
						return result;
					}
				}
				else if (flag || flag2 || !cANwinApplication.Quit())
				{
					bool result = false;
					return result;
				}
			}
			for (int i = 0; i < 10000; i += 100)
			{
				if (processes.Count((Process process) => !process.HasExited) != num)
				{
					break;
				}
				Thread.Sleep(100);
			}
			return processes.Count((Process process) => !process.HasExited) < num;
		}

		private void ShowCANwinWindow(bool showWindow = true)
		{
			if (showWindow)
			{
				if (!WindowUtils.SetForegroundWindow(CreateOfflineConfigJob.GetProcessName(this.mConfigData.ServerType, true)))
				{
					WindowUtils.SetForegroundWindow(CreateOfflineConfigJob.GetProcessName(this.mConfigData.ServerType, false));
					return;
				}
			}
			else
			{
				WindowUtils.SetForegroundWindow();
			}
		}

		private bool CheckConfigurationData(DoWorkEventArgs e)
		{
			if (this.mConfigData.ServerType != CANwinServerType.CANoe && this.mConfigData.ServerType != CANwinServerType.CANalyzer)
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.InternalError, "Illegal ServerType.");
				return false;
			}
			if (this.mConfigData.BusConfiguration == null || this.mConfigData.SysvarFiles == null || this.mConfigData.OfflineSourceConfig == null)
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.InternalError, "Invalid object initialization.");
				return false;
			}
			if (string.IsNullOrEmpty(this.mConfigData.ConfigFolder))
			{
				this.mConfigData.BaseConfigFolder = Path.Combine(Path.GetTempPath(), "VLConfig_QuickView");
			}
			if (!Path.IsPathRooted(this.mConfigData.ConfigFolder))
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, Resources.ErrorIllegalConfigFolder);
				return false;
			}
			if (this.mConfigData.UseUserDefinedTemplate)
			{
				try
				{
					FileInfo fileInfo = new FileInfo(this.mConfigData.UserDefinedTemplate);
					if (!fileInfo.Exists)
					{
						e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, Resources.ErrorIllegalUserDefinedTemplate);
						bool result = false;
						return result;
					}
					string b = (this.mConfigData.ServerType == CANwinServerType.CANoe) ? ".tcn" : ".tcw";
					if (!string.Equals(Path.GetExtension(fileInfo.FullName), b))
					{
						e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, Resources.ErrorIllegalUserDefinedTemplate);
						bool result = false;
						return result;
					}
				}
				catch
				{
					e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, Resources.ErrorIllegalUserDefinedTemplate);
					bool result = false;
					return result;
				}
			}
			if (this.mConfigData.OfflineSourceConfig.OfflineSourceFiles == null)
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.InternalError, "Invalid object initialization.");
				return false;
			}
			if (!this.mConfigData.OfflineSourceConfig.OfflineSourceFiles.Any<Tuple<string, string>>())
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Warning, Resources.ErrorNoOfflineSourceFilesConfigured);
			}
			return true;
		}

		private bool CanDiscardConfig(CANwinApplication canwinApplication, DoWorkEventArgs e)
		{
			if (this.IsCANwinQuickViewConfig(canwinApplication))
			{
				return true;
			}
			bool flag;
			if (!canwinApplication.Measurement.GetRunning(out flag))
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.InternalError, "");
				return false;
			}
			bool flag2;
			if (!canwinApplication.Configuration.GetModified(out flag2))
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.InternalError, "");
				return false;
			}
			if (flag || flag2)
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, string.Format(Resources.ErrorMeasurementRunningOrModified, this.mConfigData.ProductName));
				return false;
			}
			return true;
		}

		private bool IsCANwinQuickViewConfig(CANwinApplication canwinApplication)
		{
			string value;
			if (!canwinApplication.Configuration.GetFullName(out value))
			{
				return false;
			}
			string arg = canwinApplication.ServerType.ToString();
			string path = string.Format(Vocabulary.ConfigNameFormat, arg);
			string text = Path.Combine(this.mConfigData.ConfigFolder, path);
			return text.Equals(value);
		}

		private bool LoadTemplate(CANwinApplication canwinApplication, out string templatePath)
		{
			templatePath = string.Empty;
			bool flag;
			if (!canwinApplication.Measurement.GetRunning(out flag))
			{
				return false;
			}
			if (flag && !canwinApplication.Measurement.Stop())
			{
				return false;
			}
			bool flag2;
			if (!canwinApplication.Configuration.GetModified(out flag2))
			{
				return false;
			}
			if (flag2 && !canwinApplication.Configuration.SetModified(false))
			{
				return false;
			}
			if (this.mConfigData.UseUserDefinedTemplate)
			{
				templatePath = this.mConfigData.UserDefinedTemplate;
			}
			else
			{
				string path = (this.mConfigData.ServerType == CANwinServerType.CANoe) ? Vocabulary.OfflineTemplateCANoe : Vocabulary.OfflineTemplateCANalyzer;
				templatePath = Path.Combine(FileSystemServices.GetApplicationPath(), path);
			}
			return this.IsCANwinQuickViewConfig(canwinApplication) || (!string.IsNullOrEmpty(templatePath) && canwinApplication.Open(templatePath));
		}

		private bool CleanupTemplate(CANwinApplication canwinApplication)
		{
			bool flag = canwinApplication.System.CleanupVariablesFiles();
			if (canwinApplication.ServerType == CANwinServerType.CANoe)
			{
				flag &= canwinApplication.Configuration.SimulationSetup.CleanupDatabases();
			}
			return flag;
		}

		private bool PrepareConfigFolder(CANwinApplication canwinApplication)
		{
			if (this.IsCANwinQuickViewConfig(canwinApplication))
			{
				DirectoryInfo directoryInfo;
				try
				{
					directoryInfo = new DirectoryInfo(this.mConfigData.DatabasesFolder);
				}
				catch (Exception)
				{
					bool result = false;
					return result;
				}
				if (directoryInfo.Exists)
				{
					if (!FileSystemServices.TryDeleteDirectoryRecursive(directoryInfo, true, false))
					{
						return false;
					}
					directoryInfo.Refresh();
				}
				if (!directoryInfo.Exists)
				{
					directoryInfo.Create();
				}
				return true;
			}
			DirectoryInfo directoryInfo2;
			try
			{
				directoryInfo2 = new DirectoryInfo(this.mConfigData.ConfigFolder);
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			if (directoryInfo2.Exists)
			{
				if (!FileSystemServices.TryDeleteDirectoryRecursive(directoryInfo2, true, false))
				{
					return false;
				}
				directoryInfo2.Refresh();
			}
			if (!directoryInfo2.Exists)
			{
				directoryInfo2.Create();
			}
			try
			{
				directoryInfo2 = new DirectoryInfo(this.mConfigData.DatabasesFolder);
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			if (!directoryInfo2.Exists)
			{
				directoryInfo2.Create();
			}
			return true;
		}

		private bool InitialSaveConfig(CANwinApplication canwinApplication)
		{
			string arg = canwinApplication.ServerType.ToString();
			string path = string.Format(Vocabulary.ConfigNameFormat, arg);
			string filePath = Path.Combine(this.mConfigData.ConfigFolder, path);
			return canwinApplication.Configuration.Save(filePath, false);
		}

		private bool ConfigureOfflineConfig(CANwinApplication canwinApplication, DoWorkEventArgs e, BackgroundWorker bgw)
		{
			if (!this.ConfigureChannels(canwinApplication))
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, Resources.ErrorUnableToConfigureChannels);
				return false;
			}
			bgw.ReportProgress(80);
			if (!canwinApplication.Configuration.SetMode(ConfigurationMode.Offline))
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, Resources.ErrorUnableToActivateOfflineMode);
				return false;
			}
			if (!this.ConfigureOfflineSource(canwinApplication))
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, Resources.ErrorUnableToConfigureOfflineSource);
				return false;
			}
			if (!canwinApplication.Configuration.OfflineSetup.ClearChannelMapping())
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, Resources.ErrorUnableToConfigureChannelMapping);
				return false;
			}
			bgw.ReportProgress(85);
			if (!this.ConfigureDatabases(canwinApplication))
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, Resources.ErrorUnableToConfigureDatabases);
				return false;
			}
			if (!this.ConfigureSysvarFiles(canwinApplication))
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, Resources.ErrorUnableToConfigureSystemVariables);
				return false;
			}
			bgw.ReportProgress(90);
			if (!canwinApplication.Configuration.OfflineSetup.ViewSynchronization.SetEnabled(true))
			{
				InformMessageBox.Warning(Resources.WarningUnableToActivateViewSync);
			}
			bgw.ReportProgress(95);
			return true;
		}

		private bool ConfigureChannels(CANwinApplication canwinApplication)
		{
			return canwinApplication.Configuration.GeneralSetup.SetChannelUsage(this.mConfigData.BusConfiguration) && (canwinApplication.ServerType != CANwinServerType.CANoe || canwinApplication.Configuration.SimulationSetup.ConfigureNetworks(this.mConfigData.BusConfiguration));
		}

		private bool ConfigureOfflineSource(CANwinApplication canwinApplication)
		{
			foreach (Tuple<string, string> current in this.mConfigData.OfflineSourceConfig.OfflineSourceFiles)
			{
				string extension;
				if (string.IsNullOrEmpty(current.Item2) || string.IsNullOrEmpty(extension = Path.GetExtension(current.Item2)) || string.Compare(extension, ".glx", StringComparison.OrdinalIgnoreCase) != 0)
				{
					bool result = false;
					return result;
				}
				if (!File.Exists(current.Item2))
				{
					bool result = false;
					return result;
				}
			}
			return canwinApplication.Configuration.OfflineSetup.ConfigureSource(this.mConfigData.OfflineSourceConfig);
		}

		private bool ConfigureDatabases(CANwinApplication canwinApplication)
		{
			foreach (CANwinBusItem current in this.mConfigData.BusConfiguration)
			{
				List<string> list = new List<string>();
				foreach (string current2 in current.AbsoluteDatabaseFilePaths)
				{
					if (!string.IsNullOrEmpty(current2))
					{
						FileInfo fileInfo = new FileInfo(current2);
						string destFileName = Path.Combine(this.mConfigData.DatabasesFolder, Path.GetFileName(current2));
						FileInfo fileInfo2 = fileInfo.CopyTo(destFileName);
						list.Add(fileInfo2.FullName);
					}
				}
				current.AbsoluteDatabaseFilePaths.Clear();
				foreach (string current3 in list)
				{
					current.AbsoluteDatabaseFilePaths.Add(current3);
				}
			}
			if (!(canwinApplication is CANalyzerApplication))
			{
				return canwinApplication.Configuration.SimulationSetup.AssignDatabases(this.mConfigData.BusConfiguration);
			}
			return canwinApplication.Configuration.GeneralSetup.AssignDatabases(this.mConfigData.BusConfiguration);
		}

		private bool ConfigureSysvarFiles(CANwinApplication canwinApplication)
		{
			List<string> list = new List<string>();
			foreach (string current in this.mConfigData.SysvarFiles)
			{
				try
				{
					FileInfo fileInfo = new FileInfo(current);
					string destFileName = Path.Combine(this.mConfigData.DatabasesFolder, fileInfo.Name);
					list.Add(fileInfo.CopyTo(destFileName, true).FullName);
				}
				catch
				{
					return false;
				}
			}
			this.mConfigData.SysvarFiles.Clear();
			this.mConfigData.SysvarFiles.AddRange(list);
			return canwinApplication.System.AssignVariablesFiles(this.mConfigData.SysvarFiles);
		}

		private bool StartMeasurementAndWaitForEnd(CANwinApplication canwinApplication, DoWorkEventArgs e)
		{
			bool setTimePosAfterMeasurementEnd = this.mConfigData.OfflineSourceConfig.SetTimePosAfterMeasurementEnd;
			this.mMeasurementEndEvent.Reset();
			if (setTimePosAfterMeasurementEnd)
			{
				canwinApplication.Measurement.OnExit += new VoidEventHandler(this.Measurement_OnExit);
			}
			if (!canwinApplication.Measurement.Start())
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, Resources.ErrorUnableToStartMeasurement);
				if (setTimePosAfterMeasurementEnd)
				{
					canwinApplication.Measurement.OnExit -= new VoidEventHandler(this.Measurement_OnExit);
				}
				return false;
			}
			if (setTimePosAfterMeasurementEnd)
			{
				this.mMeasurementEndEvent.WaitOne(10000);
			}
			this.mMeasurementEndEvent.Reset();
			if (setTimePosAfterMeasurementEnd)
			{
				canwinApplication.Measurement.OnExit -= new VoidEventHandler(this.Measurement_OnExit);
			}
			return true;
		}

		private void Measurement_OnExit()
		{
			this.mMeasurementEndEvent.Set();
		}

		private bool SetTimePosition(CANwinApplication canwinApplication)
		{
			bool flag;
			return !this.mConfigData.OfflineSourceConfig.SetTimePosAfterMeasurementEnd || (canwinApplication.Measurement.GetRunning(out flag) && (flag || canwinApplication.Configuration.OfflineSetup.ViewSynchronization.SetTimeNs(this.mConfigData.OfflineSourceConfig.TimePosToSetAfterMeasurementEndNs)));
		}

		private static string GetProcessName(CANwinServerType canwinServerType, bool is64Bit)
		{
			switch (canwinServerType)
			{
			case CANwinServerType.CANoe:
				if (!is64Bit)
				{
					return Vocabulary.ProcessNameCANoe32;
				}
				return Vocabulary.ProcessNameCANoe64;
			case CANwinServerType.CANalyzer:
				if (!is64Bit)
				{
					return Vocabulary.ProcessNameCANalyzer32;
				}
				return Vocabulary.ProcessNameCANalyzer64;
			default:
				return Resources_General.Unknown;
			}
		}

		public static string GetProductName(CANwinServerType serverType)
		{
			switch (serverType)
			{
			case CANwinServerType.CANoe:
				return Vocabulary.ProductNameCANoe;
			case CANwinServerType.CANalyzer:
				return Vocabulary.ProductNameCANalyzer;
			default:
				return Resources_General.Unknown;
			}
		}

		[Conditional("DEBUG")]
		private static void DebugInfo(Exception e)
		{
		}

		[Conditional("DEBUG")]
		private static void DebugInfo(string message)
		{
		}
	}
}
