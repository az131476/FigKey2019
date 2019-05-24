using System;

namespace Vector.VLConfig.GeneralUtil.GUI
{
	public class GenericCopyFilesWithProgressIndicatorResult : GenericBackgroundWorkerResult
	{
		public EnumCopyFilesResult SpecificType
		{
			get;
			private set;
		}

		public override string ErrorInfo
		{
			get
			{
				switch (this.SpecificType)
				{
				case EnumCopyFilesResult.None:
					return base.ErrorInfo;
				case EnumCopyFilesResult.DestinationFileAlreadyExists:
					return string.Format(Resources_General.GenericBackgroundWorkerResultDestinationFileAlreadyExists, base.ErrorText);
				default:
					return base.ErrorText;
				}
			}
		}

		public GenericCopyFilesWithProgressIndicatorResult(GenericBackgroundWorkerResult.ResultType type, EnumCopyFilesResult specificType = EnumCopyFilesResult.None, string errorText = "") : base(type, errorText)
		{
			this.SpecificType = specificType;
		}
	}
}
