using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Conversion
{
	public class BinlogConversion : GenericBackgroundWorker
	{
		private readonly FileConversionParameters mConvParams;

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

		public BinlogConversion(string progressFormTitle, FileConversionParameters conversionParameters)
		{
			this.mProgressFormTitle = progressFormTitle;
			this.mConvParams = conversionParameters;
		}

		public override string GetProgressStatusText(int finishedJobs, IList<GenericBackgroundWorkerJob> runningJobs)
		{
			if (finishedJobs == base.Jobs.Count || !runningJobs.Any<GenericBackgroundWorkerJob>())
			{
				return Resources.ProgressConvertToDestFormatFinished;
			}
			return string.Format(Resources.ProgressConvertToDestFormat, finishedJobs, base.Jobs.Count, Path.GetFileName(((BinlogConversionJob)runningJobs.Last<GenericBackgroundWorkerJob>()).SourceFileName));
		}

		public BinlogConversionResult ConvertFiles(IEnumerable<string> filesToConvert)
		{
			base.Jobs.Clear();
			foreach (string current in filesToConvert)
			{
				BinlogConversionJob item = new BinlogConversionJob(current, Path.ChangeExtension(current, FileConversionHelper.GetConfiguredDestinationFormatExtension(this.mConvParams)), this.mConvParams.OverwriteDestinationFiles);
				base.Jobs.Add(item);
			}
			if (DialogResult.Cancel == base.ExecuteJobs())
			{
				return new BinlogConversionResult(GenericBackgroundWorkerResult.ResultType.CanceledByUser, BinlogConversionResult.EnumSpecificType.None, "");
			}
			return (BinlogConversionResult)base.Result;
		}
	}
}
