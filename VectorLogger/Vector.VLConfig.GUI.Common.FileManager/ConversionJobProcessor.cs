using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Threading;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class ConversionJobProcessor : DoubleProgressIndicatorForm, IDoubleProgressIndicator, IProgressIndicator
	{
		private delegate void SetTextCallback(string text);

		private delegate void SetValueCallback(int value);

		private delegate void SetMinMaxCallback(int min, int max);

		private delegate void StopCallback();

		private BackgroundWorker backgroundWorker;

		private List<ConversionJob> jobsToProcess;

		private string targetSubFolderPath;

		private string tempConvertFolderPath;

		private string tempCacheFolderPath;

		private DatabaseConfiguration dbConfig;

		private string configFolderPath;

		private LoggerDeviceWindowsFileSystem device;

		private bool isMasterMinMaxSet;

		private int masterMinValue;

		private int masterMaxValue;

		private bool isMasterValueSet;

		private int masterValue;

		private bool isMasterStatusTextSet;

		private string masterStatusText;

		private bool isSlaveMinMaxSet;

		private int slaveMinValue;

		private int slaveMaxValue;

		private bool isSlaveValueSet;

		private int slaveValue;

		private bool isSlaveStatusTextSet;

		private string slaveStatusText;

		private bool isCancelClicked;

		private CultureInfo initialCulture;

		public ConversionJobProcessor(List<ConversionJob> conversionJobs, string destSubFolderPath, string tempCacheDirPath, string tempConvertDirPath, DatabaseConfiguration databaseConfiguration, string configurationFolderPath, LoggerDeviceWindowsFileSystem loggerDevice)
		{
			base.CancelButtonClicked += new DoubleProgressIndicatorForm.ButtonClickedHandler(this.CancelClicked);
			this.isCancelClicked = false;
			this.InitializeBackgroundWorker();
			this.jobsToProcess = conversionJobs;
			this.targetSubFolderPath = destSubFolderPath;
			this.tempConvertFolderPath = tempConvertDirPath;
			this.tempCacheFolderPath = tempCacheDirPath;
			this.dbConfig = databaseConfiguration;
			this.configFolderPath = configurationFolderPath;
			this.device = loggerDevice;
			this.Text = Resources.TitleConvertingLogData;
			this.initialCulture = new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name);
		}

		private void InitializeBackgroundWorker()
		{
			this.backgroundWorker = new BackgroundWorker();
			this.backgroundWorker.WorkerReportsProgress = true;
			this.backgroundWorker.WorkerSupportsCancellation = true;
			this.backgroundWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
			this.backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
		}

		private void InitDataExchange()
		{
			this.isMasterMinMaxSet = false;
			this.masterMinValue = 0;
			this.masterMaxValue = 1;
			this.isMasterValueSet = false;
			this.masterValue = 0;
			this.isMasterStatusTextSet = false;
			this.masterStatusText = "";
			this.isSlaveMinMaxSet = false;
			this.slaveMinValue = 0;
			this.slaveMaxValue = 1;
			this.isSlaveValueSet = false;
			this.slaveValue = 0;
			this.isSlaveStatusTextSet = false;
			this.slaveStatusText = "";
		}

		public void Start()
		{
			this.backgroundWorker.RunWorkerAsync();
			base.ShowDialog();
		}

		public void ProcessExited()
		{
			if (base.InvokeRequired)
			{
				ConversionJobProcessor.StopCallback method = new ConversionJobProcessor.StopCallback(this.Stop);
				base.Invoke(method);
				return;
			}
			this.Stop();
		}

		public void Stop()
		{
			if (base.SlaveIndicator.Cancelled())
			{
				this.backgroundWorker.CancelAsync();
				base.SlaveIndicator.ProcessExited();
			}
		}

		private void CancelClicked()
		{
			Monitor.Enter(this);
			this.isCancelClicked = true;
			Monitor.Exit(this);
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			Thread.CurrentThread.CurrentUICulture = this.initialCulture;
			this.masterMaxValue = this.jobsToProcess.Count;
			int num = 0;
			int num2 = 4;
			if (this.device.IsPrimaryFileGroupCompressed)
			{
				num2 = 5;
			}
			base.MasterIndicator.SetMinMax(0, this.jobsToProcess.Count * num2);
			for (int i = 0; i < this.jobsToProcess.Count; i++)
			{
				if (backgroundWorker != null && backgroundWorker.CancellationPending)
				{
					e.Cancel = true;
					break;
				}
				string masterStatusTextPrefix = string.Format(Resources.JobConversionStatus, this.jobsToProcess[i].Name, i, this.jobsToProcess.Count);
				this.device.ConvertSelectedLogFilesFromWindowsFormattedMedia(this.jobsToProcess[i], this.targetSubFolderPath, this.tempCacheFolderPath, this.tempConvertFolderPath, this.dbConfig, this.configFolderPath, masterStatusTextPrefix, ref num, this, new ProcessExitedDelegate(this.Stop));
				if (this.Cancelled())
				{
					base.MasterIndicator.ProcessExited();
					return;
				}
			}
			base.MasterIndicator.ProcessExited();
		}

		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			Thread.CurrentThread.CurrentUICulture = this.initialCulture;
			if (this.isSlaveMinMaxSet)
			{
				base.SlaveIndicator.SetValue(0);
				base.SlaveIndicator.SetMinMax(this.slaveMinValue, this.slaveMaxValue);
				this.isSlaveMinMaxSet = false;
			}
			else if (this.isSlaveValueSet)
			{
				base.SlaveIndicator.SetValue(this.slaveValue);
				this.isSlaveValueSet = false;
			}
			else if (this.isSlaveStatusTextSet)
			{
				base.SlaveIndicator.SetStatusText(this.slaveStatusText);
				this.isSlaveStatusTextSet = false;
			}
			else if (this.isMasterMinMaxSet)
			{
				base.MasterIndicator.SetValue(0);
				base.MasterIndicator.SetMinMax(this.masterMinValue, this.masterMaxValue);
				this.isMasterMinMaxSet = false;
			}
			else if (this.isMasterValueSet)
			{
				base.MasterIndicator.SetValue(this.masterValue);
				this.isMasterValueSet = false;
			}
			else if (this.isMasterStatusTextSet)
			{
				base.MasterIndicator.SetStatusText(this.masterStatusText);
				this.isMasterStatusTextSet = false;
			}
			this.Refresh();
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			this.Stop();
		}

		public new void Activate()
		{
		}

		public new void Deactivate()
		{
		}

		public void SetMinMax(int min, int max)
		{
			this.isSlaveMinMaxSet = true;
			this.slaveMinValue = min;
			this.slaveMaxValue = max;
			this.slaveValue = min;
			this.backgroundWorker.ReportProgress(0);
		}

		public void SetValue(int value)
		{
			this.isSlaveValueSet = true;
			this.slaveValue = value;
			this.backgroundWorker.ReportProgress(0);
		}

		public void SetStatusText(string text)
		{
			this.isSlaveStatusTextSet = true;
			this.slaveStatusText = text;
			this.backgroundWorker.ReportProgress(0);
		}

		public void SetMasterMinMax(int min, int max)
		{
			this.isMasterMinMaxSet = true;
			this.masterMinValue = min;
			this.masterMaxValue = max;
			this.backgroundWorker.ReportProgress(0);
		}

		public void SetMasterValue(int value)
		{
			this.isMasterValueSet = true;
			this.masterValue = value;
			this.backgroundWorker.ReportProgress(0);
		}

		public void SetMasterStatusText(string text)
		{
			this.isMasterStatusTextSet = true;
			this.masterStatusText = text;
			this.backgroundWorker.ReportProgress(0);
		}

		public bool Cancelled()
		{
			Monitor.Enter(this);
			bool result = this.isCancelClicked;
			Monitor.Exit(this);
			return result;
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ConversionJobProcessor));
			base.SuspendLayout();
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.ClientSize = new Size(814, 223);
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "ConversionJobProcessor";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
