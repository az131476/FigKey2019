using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vector.VLConfig.CANwinAccess.Data;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;

namespace Vector.VLConfig.CANwinAccess
{
	public class CANwinAutomation : GenericBackgroundWorker
	{
		private static CANwinAutomation sInstance;

		private CANwinServerType mCANwinServerType;

		public static CANwinAutomation Instance
		{
			get
			{
				CANwinAutomation arg_17_0;
				if ((arg_17_0 = CANwinAutomation.sInstance) == null)
				{
					arg_17_0 = (CANwinAutomation.sInstance = new CANwinAutomation());
				}
				return arg_17_0;
			}
		}

		public override bool CanCancelWork
		{
			get
			{
				return false;
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

		private CANwinAutomation()
		{
		}

		public override string GetProgressStatusText(int finishedJobs, IList<GenericBackgroundWorkerJob> runningJobs)
		{
			return string.Format(Resources.StatusInfoOfflineConfigCreationRunning, CreateOfflineConfigJob.GetProductName(this.mCANwinServerType));
		}

		public bool CreateOfflineConfig(CANwinQuickViewData configData)
		{
			this.mCANwinServerType = configData.ServerType;
			base.Jobs.Clear();
			base.Jobs.Add(new CreateOfflineConfigJob(configData));
			base.ExecuteJobs();
			GenericBackgroundWorkerResult genericBackgroundWorkerResult = base.Result as GenericBackgroundWorkerResult;
			if (genericBackgroundWorkerResult == null)
			{
				return false;
			}
			switch (genericBackgroundWorkerResult.Type)
			{
			case GenericBackgroundWorkerResult.ResultType.Success:
				return true;
			case GenericBackgroundWorkerResult.ResultType.CanceledByUser:
				WindowUtils.SetForegroundWindow();
				InformMessageBox.Info(genericBackgroundWorkerResult.ErrorInfo);
				return false;
			case GenericBackgroundWorkerResult.ResultType.Warning:
				WindowUtils.SetForegroundWindow();
				InformMessageBox.Warning(genericBackgroundWorkerResult.ErrorInfo);
				return false;
			}
			WindowUtils.SetForegroundWindow();
			InformMessageBox.Error(genericBackgroundWorkerResult.ErrorInfo);
			return false;
		}
	}
}
