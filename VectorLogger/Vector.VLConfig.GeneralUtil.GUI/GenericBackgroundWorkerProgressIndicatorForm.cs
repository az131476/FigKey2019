using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Vector.VLConfig.GeneralUtil.GUI
{
	public class GenericBackgroundWorkerProgressIndicatorForm : Form
	{
		private class JobData
		{
			public readonly GenericBackgroundWorkerJob Job;

			public bool IsRunning;

			private bool mIsFinished;

			private int mProgressPercent;

			public bool IsPending
			{
				get
				{
					return !this.IsRunning && !this.IsFinished;
				}
			}

			public bool IsFinished
			{
				get
				{
					return this.mIsFinished;
				}
				set
				{
					this.mIsFinished |= value;
					if (!this.mIsFinished)
					{
						return;
					}
					this.mProgressPercent = 100;
					this.IsRunning = false;
				}
			}

			public int ProgressPercent
			{
				get
				{
					return this.mProgressPercent;
				}
				set
				{
					this.mProgressPercent = Math.Max(0, Math.Min(value, 100));
				}
			}

			public JobData(GenericBackgroundWorkerJob job)
			{
				this.Job = job;
				this.IsRunning = false;
				this.IsFinished = false;
				this.ProgressPercent = 0;
			}
		}

		private readonly GenericBackgroundWorker mGenericBackgroundWorker;

		private readonly Dictionary<BackgroundWorker, GenericBackgroundWorkerProgressIndicatorForm.JobData> mBackgroundWorkerPool;

		private readonly List<GenericBackgroundWorkerProgressIndicatorForm.JobData> mJobs;

		private bool mCanceled;

		private bool mHasError;

		private IContainer components;

		private TextBox mTextBoxStatus;

		private ProgressBar mProgressBarMainProgress;

		private Button mButtonCancel;

		private TableLayoutPanel mTableLayoutPanel;

		private int TotalJobs
		{
			get
			{
				return this.mJobs.Count;
			}
		}

		private int PendingJobs
		{
			get
			{
				return this.mJobs.Count((GenericBackgroundWorkerProgressIndicatorForm.JobData t) => t.IsPending);
			}
		}

		private int RunningJobs
		{
			get
			{
				return this.mJobs.Count((GenericBackgroundWorkerProgressIndicatorForm.JobData t) => t.IsRunning);
			}
		}

		private int FinishedJobs
		{
			get
			{
				return this.mJobs.Count((GenericBackgroundWorkerProgressIndicatorForm.JobData t) => t.IsFinished);
			}
		}

		private int FreeBackgroundWorkers
		{
			get
			{
				return this.mBackgroundWorkerPool.Values.Count((GenericBackgroundWorkerProgressIndicatorForm.JobData t) => t == null);
			}
		}

		public object Result
		{
			get;
			private set;
		}

		public GenericBackgroundWorkerProgressIndicatorForm(GenericBackgroundWorker genericBackgroundWorker)
		{
			this.mGenericBackgroundWorker = genericBackgroundWorker;
			this.mBackgroundWorkerPool = new Dictionary<BackgroundWorker, GenericBackgroundWorkerProgressIndicatorForm.JobData>();
			this.mJobs = new List<GenericBackgroundWorkerProgressIndicatorForm.JobData>();
			this.mCanceled = false;
			this.mHasError = false;
			this.InitializeComponent();
			base.Shown += new EventHandler(this.GenericBackgroundWorkerProgressIndicatorForm_Shown);
			base.FormClosing += new FormClosingEventHandler(this.GenericBackgroundWorkerProgressIndicatorForm_FormClosing);
			this.mButtonCancel.Enabled = (this.mGenericBackgroundWorker != null && this.mGenericBackgroundWorker.CanCancelWork);
			base.ShowInTaskbar = (this.mGenericBackgroundWorker != null && this.mGenericBackgroundWorker.ShowInTaskbar);
			this.SetTaskbarProgressState(TaskbarProgress.TaskbarStates.NoProgress);
		}

		private void SetTaskbarProgressState(TaskbarProgress.TaskbarStates taskbarState)
		{
			if (base.ShowInTaskbar)
			{
				TaskbarProgress.SetState(base.Handle, taskbarState);
				return;
			}
			TaskbarProgress.SetState(taskbarState);
		}

		private void SetTaskbarProgressValue(double progressValue, double progressMax)
		{
			if (this.mProgressBarMainProgress.Style == ProgressBarStyle.Marquee)
			{
				return;
			}
			if (base.ShowInTaskbar)
			{
				TaskbarProgress.SetValue(base.Handle, progressValue, progressMax);
				return;
			}
			TaskbarProgress.SetValue(progressValue, progressMax);
		}

		private void GenericBackgroundWorkerProgressIndicatorForm_Shown(object sender, EventArgs e)
		{
			base.Height = base.Height - base.ClientSize.Height + this.mButtonCancel.Bottom + 13;
			this.PrepareExecution();
		}

		private void GenericBackgroundWorkerProgressIndicatorForm_FormClosing(object sender, EventArgs e)
		{
			this.SetTaskbarProgressState(TaskbarProgress.TaskbarStates.NoProgress);
		}

		private void ButtonCancel_Click(object sender, EventArgs e)
		{
			if (this.mGenericBackgroundWorker == null || !this.mGenericBackgroundWorker.CanCancelWork)
			{
				return;
			}
			IEnumerable<BackgroundWorker> runningBackgroundWorkers = this.GetRunningBackgroundWorkers();
			foreach (BackgroundWorker current in runningBackgroundWorkers)
			{
				current.CancelAsync();
			}
			this.mCanceled = true;
			this.UpdateStatusText();
		}

		private void JobFinished(object sender, RunWorkerCompletedEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			if (backgroundWorker == null)
			{
				return;
			}
			GenericBackgroundWorkerProgressIndicatorForm.JobData jobData = this.mBackgroundWorkerPool[backgroundWorker];
			backgroundWorker.DoWork -= new DoWorkEventHandler(jobData.Job.DoWork);
			jobData.IsFinished = true;
			this.mBackgroundWorkerPool[backgroundWorker] = null;
			this.UpdateProgressValue();
			this.UpdateStatusText();
			this.mCanceled = (this.mCanceled || e.Cancelled);
			this.Result = (this.mCanceled ? null : e.Result);
			GenericBackgroundWorkerResult genericBackgroundWorkerResult = this.Result as GenericBackgroundWorkerResult;
			this.mHasError = (genericBackgroundWorkerResult != null && genericBackgroundWorkerResult.Type != GenericBackgroundWorkerResult.ResultType.Success);
			if (this.mHasError && this.PendingJobs > 0)
			{
				DialogResult dialogResult = InformMessageBox.Show(EnumInfoType.Error, EnumQuestionType.Question, genericBackgroundWorkerResult.ErrorInfo + Environment.NewLine + Environment.NewLine + Resources_General.QuestionContinueAnyway);
				if (dialogResult == DialogResult.No)
				{
					this.mCanceled = true;
				}
				if (dialogResult == DialogResult.Yes)
				{
					this.mHasError = false;
				}
			}
			if (!this.mCanceled && !this.mHasError && this.FinishedJobs + this.RunningJobs < this.TotalJobs)
			{
				this.RunNextJob();
				return;
			}
			if (this.RunningJobs == 0)
			{
				this.CleanupAndClose();
			}
		}

		private void JobProgressChanged(object sender, ProgressChangedEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			if (backgroundWorker == null || !this.mBackgroundWorkerPool.ContainsKey(backgroundWorker))
			{
				return;
			}
			int num = Math.Max(0, Math.Min(e.ProgressPercentage, 100));
			if (num != this.mBackgroundWorkerPool[backgroundWorker].ProgressPercent)
			{
				this.mBackgroundWorkerPool[backgroundWorker].ProgressPercent = num;
				this.UpdateProgressValue();
			}
		}

		private BackgroundWorker GetFreeBackgroundWorker()
		{
			if (this.FreeBackgroundWorkers <= 0)
			{
				return null;
			}
			return this.mBackgroundWorkerPool.Keys.First((BackgroundWorker t) => this.mBackgroundWorkerPool[t] == null);
		}

		private IEnumerable<BackgroundWorker> GetRunningBackgroundWorkers()
		{
			return from t in this.mBackgroundWorkerPool.Keys
			where this.mBackgroundWorkerPool[t] != null
			select t;
		}

		private GenericBackgroundWorkerProgressIndicatorForm.JobData GetNextJob()
		{
			if (this.PendingJobs <= 0)
			{
				return null;
			}
			return this.mJobs.First((GenericBackgroundWorkerProgressIndicatorForm.JobData t) => t.IsPending);
		}

		private void PrepareExecution()
		{
			this.Result = null;
			this.mBackgroundWorkerPool.Clear();
			this.mJobs.Clear();
			this.mCanceled = false;
			this.mHasError = false;
			if (this.mGenericBackgroundWorker == null || !this.mGenericBackgroundWorker.Jobs.Any<GenericBackgroundWorkerJob>())
			{
				base.DialogResult = DialogResult.OK;
				base.Close();
				return;
			}
			foreach (GenericBackgroundWorkerJob current in this.mGenericBackgroundWorker.Jobs)
			{
				this.mJobs.Add(new GenericBackgroundWorkerProgressIndicatorForm.JobData(current));
			}
			int num = (int)((this.mGenericBackgroundWorker.MaxParallelProcesses > 0u && (ulong)this.mGenericBackgroundWorker.MaxParallelProcesses < (ulong)((long)Environment.ProcessorCount)) ? this.mGenericBackgroundWorker.MaxParallelProcesses : ((uint)Environment.ProcessorCount));
			for (int i = 0; i < num; i++)
			{
				BackgroundWorker backgroundWorker = new BackgroundWorker();
				backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.JobFinished);
				backgroundWorker.WorkerReportsProgress = true;
				backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(this.JobProgressChanged);
				backgroundWorker.WorkerSupportsCancellation = true;
				this.mBackgroundWorkerPool[backgroundWorker] = null;
			}
			this.Text = this.mGenericBackgroundWorker.ProgressFormTitle;
			this.mProgressBarMainProgress.Minimum = 0;
			this.mProgressBarMainProgress.Maximum = this.CalcMaxProgressValue();
			this.mProgressBarMainProgress.Style = this.mGenericBackgroundWorker.ProgressBarStyle;
			if (this.mProgressBarMainProgress.Style == ProgressBarStyle.Marquee)
			{
				this.SetTaskbarProgressState(TaskbarProgress.TaskbarStates.Indeterminate);
			}
			else
			{
				this.SetTaskbarProgressValue(0.0, (double)(this.mProgressBarMainProgress.Maximum - this.mProgressBarMainProgress.Minimum));
			}
			this.UpdateProgressValue();
			this.RunNextJob();
		}

		private void RunNextJob()
		{
			if (this.mCanceled || this.mHasError)
			{
				return;
			}
			if (this.FreeBackgroundWorkers == 0)
			{
				return;
			}
			if (this.PendingJobs == 0)
			{
				return;
			}
			BackgroundWorker freeBackgroundWorker = this.GetFreeBackgroundWorker();
			GenericBackgroundWorkerProgressIndicatorForm.JobData nextJob = this.GetNextJob();
			this.mBackgroundWorkerPool[freeBackgroundWorker] = nextJob;
			freeBackgroundWorker.DoWork += new DoWorkEventHandler(nextJob.Job.DoWork);
			nextJob.IsRunning = true;
			this.UpdateStatusText();
			freeBackgroundWorker.RunWorkerAsync(nextJob.Job);
			this.RunNextJob();
		}

		private void UpdateStatusText()
		{
			if (this.mCanceled || this.mHasError)
			{
				this.mTextBoxStatus.Text = Resources_General.GenericBackgroundWorkerWaitingForBackgroundTasksToFinish;
				return;
			}
			this.mTextBoxStatus.Text = this.mGenericBackgroundWorker.GetProgressStatusText(this.FinishedJobs, (from t in this.mJobs
			where t.IsRunning
			select t.Job).ToList<GenericBackgroundWorkerJob>());
		}

		private void UpdateProgressValue()
		{
			int num;
			try
			{
				num = this.mJobs.Sum((GenericBackgroundWorkerProgressIndicatorForm.JobData t) => t.Job.Weight * t.ProgressPercent);
			}
			catch (OverflowException)
			{
				num = 2147483647;
			}
			this.mProgressBarMainProgress.Value = num;
			this.SetTaskbarProgressValue((double)num, (double)(this.mProgressBarMainProgress.Maximum - this.mProgressBarMainProgress.Minimum));
		}

		private void CleanupAndClose()
		{
			this.mJobs.Clear();
			foreach (BackgroundWorker current in this.mBackgroundWorkerPool.Keys)
			{
				current.ProgressChanged -= new ProgressChangedEventHandler(this.JobProgressChanged);
				current.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(this.JobFinished);
				current.Dispose();
			}
			this.mBackgroundWorkerPool.Clear();
			base.DialogResult = (this.mCanceled ? DialogResult.Cancel : DialogResult.OK);
			base.Close();
		}

		private int CalcMaxProgressValue()
		{
			int result;
			try
			{
				result = this.mJobs.Sum((GenericBackgroundWorkerProgressIndicatorForm.JobData t) => t.Job.Weight * 100);
			}
			catch (OverflowException)
			{
				result = 2147483647;
			}
			return result;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GenericBackgroundWorkerProgressIndicatorForm));
			this.mTextBoxStatus = new TextBox();
			this.mProgressBarMainProgress = new ProgressBar();
			this.mButtonCancel = new Button();
			this.mTableLayoutPanel = new TableLayoutPanel();
			this.mTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mTextBoxStatus, "mTextBoxStatus");
			this.mTextBoxStatus.BorderStyle = BorderStyle.None;
			this.mTableLayoutPanel.SetColumnSpan(this.mTextBoxStatus, 3);
			this.mTextBoxStatus.Name = "mTextBoxStatus";
			this.mTextBoxStatus.ReadOnly = true;
			componentResourceManager.ApplyResources(this.mProgressBarMainProgress, "mProgressBarMainProgress");
			this.mTableLayoutPanel.SetColumnSpan(this.mProgressBarMainProgress, 3);
			this.mProgressBarMainProgress.Name = "mProgressBarMainProgress";
			componentResourceManager.ApplyResources(this.mButtonCancel, "mButtonCancel");
			this.mButtonCancel.Name = "mButtonCancel";
			this.mButtonCancel.UseVisualStyleBackColor = true;
			this.mButtonCancel.Click += new EventHandler(this.ButtonCancel_Click);
			componentResourceManager.ApplyResources(this.mTableLayoutPanel, "mTableLayoutPanel");
			this.mTableLayoutPanel.Controls.Add(this.mTextBoxStatus, 1, 1);
			this.mTableLayoutPanel.Controls.Add(this.mButtonCancel, 2, 4);
			this.mTableLayoutPanel.Controls.Add(this.mProgressBarMainProgress, 1, 2);
			this.mTableLayoutPanel.Name = "mTableLayoutPanel";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ControlBox = false;
			base.Controls.Add(this.mTableLayoutPanel);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "GenericBackgroundWorkerProgressIndicatorForm";
			this.mTableLayoutPanel.ResumeLayout(false);
			this.mTableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
