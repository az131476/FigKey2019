using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vector.VLConfig.GeneralUtil.GUI
{
	public abstract class GenericBackgroundWorker
	{
		private readonly List<GenericBackgroundWorkerJob> mJobs = new List<GenericBackgroundWorkerJob>();

		public virtual string ProgressFormTitle
		{
			get
			{
				return Vocabulary.VLConfigApplicationTitle;
			}
		}

		public virtual bool CanCancelWork
		{
			get
			{
				return true;
			}
		}

		public virtual uint MaxParallelProcesses
		{
			get
			{
				return 1u;
			}
		}

		public virtual ProgressBarStyle ProgressBarStyle
		{
			get
			{
				return ProgressBarStyle.Continuous;
			}
		}

		public virtual bool ShowInTaskbar
		{
			get
			{
				return false;
			}
		}

		public IList<GenericBackgroundWorkerJob> Jobs
		{
			get
			{
				return this.mJobs;
			}
		}

		protected object Result
		{
			get;
			private set;
		}

		public virtual string GetProgressStatusText(int finishedJobs, IList<GenericBackgroundWorkerJob> runningJobs)
		{
			return string.Format(Resources_General.GenericBackgroundWorkerStatusText, finishedJobs, this.Jobs.Count);
		}

		protected DialogResult ExecuteJobs()
		{
			this.Result = null;
			DialogResult result;
			using (GenericBackgroundWorkerProgressIndicatorForm genericBackgroundWorkerProgressIndicatorForm = new GenericBackgroundWorkerProgressIndicatorForm(this))
			{
				DialogResult dialogResult = genericBackgroundWorkerProgressIndicatorForm.ShowDialog();
				this.Result = genericBackgroundWorkerProgressIndicatorForm.Result;
				result = dialogResult;
			}
			return result;
		}
	}
}
