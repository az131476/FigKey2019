using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Conversion
{
	internal class MDF4Finalizer : GenericBackgroundWorker
	{
		private readonly MDF4FinalizerTool mFinalizerTool;

		private readonly string mProgressFormTitle;

		public override string ProgressFormTitle
		{
			get
			{
				return this.mProgressFormTitle;
			}
		}

		public override bool CanCancelWork
		{
			get
			{
				return true;
			}
		}

		public override uint MaxParallelProcesses
		{
			get
			{
				return 1u;
			}
		}

		public override ProgressBarStyle ProgressBarStyle
		{
			get
			{
				return ProgressBarStyle.Continuous;
			}
		}

		public override bool ShowInTaskbar
		{
			get
			{
				return false;
			}
		}

		public MDF4Finalizer(string progressFormTitle, FileConversionParameters conversionParameters)
		{
			this.mProgressFormTitle = progressFormTitle;
			this.mFinalizerTool = new MDF4FinalizerTool(conversionParameters);
		}

		public override string GetProgressStatusText(int finishedJobs, IList<GenericBackgroundWorkerJob> runningJobs)
		{
			if (finishedJobs == base.Jobs.Count || !runningJobs.Any<GenericBackgroundWorkerJob>())
			{
				return Resources.ProgressMdfFinalizeFinished;
			}
			return string.Format(Resources.ProgressMdfFinalize, finishedJobs, base.Jobs.Count, Path.GetFileName(((MDF4FinalizerJob)runningJobs.Last<GenericBackgroundWorkerJob>()).SourceFileName));
		}

		public GenericBackgroundWorkerResult FinalizeFiles(IEnumerable<string> filesToConvert)
		{
			base.Jobs.Clear();
			foreach (string current in filesToConvert)
			{
				MDF4FinalizerJob item = new MDF4FinalizerJob(current, this.mFinalizerTool);
				base.Jobs.Add(item);
			}
			if (DialogResult.Cancel == base.ExecuteJobs())
			{
				return new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.CanceledByUser, "");
			}
			return (GenericBackgroundWorkerResult)base.Result;
		}
	}
}
