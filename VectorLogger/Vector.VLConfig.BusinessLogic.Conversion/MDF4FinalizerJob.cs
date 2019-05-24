using System;
using System.ComponentModel;
using System.IO;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;

namespace Vector.VLConfig.BusinessLogic.Conversion
{
	internal class MDF4FinalizerJob : GenericBackgroundWorkerJob
	{
		private readonly MDF4FinalizerTool mFinalizerTool;

		public string SourceFileName
		{
			get;
			private set;
		}

		public MDF4FinalizerJob(string sourceFileName, MDF4FinalizerTool finalizerTool)
		{
			this.SourceFileName = sourceFileName;
			this.mFinalizerTool = finalizerTool;
			FileInfo fileInfo = new FileInfo(this.SourceFileName);
			base.Weight = (int)Math.Round((double)fileInfo.Length / Math.Pow(2.0, 20.0));
		}

		protected override void OnDoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			MDF4FinalizerJob mDF4FinalizerJob = e.Argument as MDF4FinalizerJob;
			if (backgroundWorker == null || mDF4FinalizerJob != this)
			{
				e.Result = new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.InternalError, "");
				return;
			}
			string errorText;
			Result convRes = this.mFinalizerTool.FinalizeFile(this.SourceFileName, out errorText);
			e.Result = this.GetGenericResult(convRes, errorText);
		}

		private GenericBackgroundWorkerResult GetGenericResult(Result convRes, string errorText = "")
		{
			switch (convRes)
			{
			case Result.OK:
				return new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Success, errorText);
			case Result.Error:
				return new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.Error, errorText);
			case Result.UserAbort:
				return new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.CanceledByUser, errorText);
			default:
				return new GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType.InternalError, errorText);
			}
		}
	}
}
