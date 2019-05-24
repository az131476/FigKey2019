using System;

namespace Vector.VLConfig.GeneralUtil.GUI
{
	public class GenericBackgroundWorkerResult
	{
		public enum ResultType
		{
			Success,
			CanceledByUser,
			Warning,
			Error,
			InternalError
		}

		public virtual string ErrorInfo
		{
			get
			{
				switch (this.Type)
				{
				case GenericBackgroundWorkerResult.ResultType.Success:
					return string.Empty;
				case GenericBackgroundWorkerResult.ResultType.CanceledByUser:
					return Resources_General.GenericBackgroundWorkerCanceledByUser;
				case GenericBackgroundWorkerResult.ResultType.Warning:
					return Resources_General.Warning + ": " + this.ErrorText;
				case GenericBackgroundWorkerResult.ResultType.Error:
					return Resources_General.Error + ": " + this.ErrorText;
				case GenericBackgroundWorkerResult.ResultType.InternalError:
					return Resources_General.InternalError + ": " + this.ErrorText;
				default:
					return this.ErrorText;
				}
			}
		}

		public GenericBackgroundWorkerResult.ResultType Type
		{
			get;
			private set;
		}

		protected string ErrorText
		{
			get;
			private set;
		}

		public GenericBackgroundWorkerResult(GenericBackgroundWorkerResult.ResultType type, string errorText = "")
		{
			this.Type = type;
			this.ErrorText = errorText;
		}
	}
}
