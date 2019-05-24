using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Vector.VLConfig.BinlogConverter;
using Vector.VLConfig.GeneralUtil.GUI;

namespace Vector.VLConfig.BusinessLogic.Conversion
{
	public class BinlogConversionJob : GenericBackgroundWorkerJob
	{
		private readonly string mDestinationFileName;

		private readonly bool mOverwrite;

		private readonly Version mAppVersion = new Version(Application.ProductVersion);

		public string SourceFileName
		{
			get;
			private set;
		}

		public BinlogConversionJob(string sourceFileName, string destinationFileName, bool overwrite)
		{
			this.SourceFileName = sourceFileName;
			this.mDestinationFileName = destinationFileName;
			this.mOverwrite = overwrite;
			FileInfo fileInfo = new FileInfo(this.SourceFileName);
			base.Weight = (int)Math.Round((double)fileInfo.Length / Math.Pow(2.0, 20.0));
		}

		protected override void OnDoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			BinlogConversionJob binlogConversionJob = e.Argument as BinlogConversionJob;
			if (backgroundWorker == null || binlogConversionJob != this)
			{
				e.Result = new BinlogConversionResult(GenericBackgroundWorkerResult.ResultType.InternalError, BinlogConversionResult.EnumSpecificType.None, "");
				return;
			}
			if (File.Exists(this.mDestinationFileName) && !this.mOverwrite)
			{
				e.Result = new BinlogConversionResult(GenericBackgroundWorkerResult.ResultType.Error, BinlogConversionResult.EnumSpecificType.DestinationFileAlreadyExists, "");
				return;
			}
			using (BLConvertNet bLConvertNet = new BLConvertNet(backgroundWorker))
			{
				bLConvertNet.SetApplicationVersion(this.mAppVersion);
				BLConvertNet.EnumResult convRes = bLConvertNet.ConvertFile(this.SourceFileName, this.mDestinationFileName);
				GenericBackgroundWorkerResult genericResult = this.GetGenericResult(convRes, bLConvertNet.GetLastErrorInfo());
				e.Result = genericResult;
				if (genericResult.Type == GenericBackgroundWorkerResult.ResultType.CanceledByUser)
				{
					e.Cancel = true;
				}
			}
		}

		private BinlogConversionResult GetGenericResult(BLConvertNet.EnumResult convRes, string errorText = "")
		{
			switch (convRes)
			{
			case BLConvertNet.EnumResult.Success:
				return new BinlogConversionResult(GenericBackgroundWorkerResult.ResultType.Success, BinlogConversionResult.EnumSpecificType.None, errorText);
			case BLConvertNet.EnumResult.CanceledByUser:
				return new BinlogConversionResult(GenericBackgroundWorkerResult.ResultType.CanceledByUser, BinlogConversionResult.EnumSpecificType.None, errorText);
			case BLConvertNet.EnumResult.Err_InvalidParameter:
				return new BinlogConversionResult(GenericBackgroundWorkerResult.ResultType.InternalError, BinlogConversionResult.EnumSpecificType.None, string.Format("Invalid parameter: {0}", errorText));
			case BLConvertNet.EnumResult.Err_SourceFormatNotSupported:
				return new BinlogConversionResult(GenericBackgroundWorkerResult.ResultType.InternalError, BinlogConversionResult.EnumSpecificType.None, string.Format("Format of source file not supported: {0}", errorText));
			case BLConvertNet.EnumResult.Err_TargetFormatNotSupported:
				return new BinlogConversionResult(GenericBackgroundWorkerResult.ResultType.InternalError, BinlogConversionResult.EnumSpecificType.None, string.Format("Format of destination file not supported: {0}", errorText));
			case BLConvertNet.EnumResult.Err_IdenticalFormat:
				return new BinlogConversionResult(GenericBackgroundWorkerResult.ResultType.InternalError, BinlogConversionResult.EnumSpecificType.None, "Source and destination have identical format.");
			case BLConvertNet.EnumResult.Err_UnableToOpenSourceFile:
				return new BinlogConversionResult(GenericBackgroundWorkerResult.ResultType.Error, BinlogConversionResult.EnumSpecificType.UnableToOpenSourceFile, errorText);
			case BLConvertNet.EnumResult.Err_UnableToOpenTargetFile:
				return new BinlogConversionResult(GenericBackgroundWorkerResult.ResultType.Error, BinlogConversionResult.EnumSpecificType.UnableToOpenDestinationFile, errorText);
			case BLConvertNet.EnumResult.Err_WhileConverting:
				return new BinlogConversionResult(GenericBackgroundWorkerResult.ResultType.InternalError, BinlogConversionResult.EnumSpecificType.None, "General error while converting.");
			default:
				return new BinlogConversionResult(GenericBackgroundWorkerResult.ResultType.InternalError, BinlogConversionResult.EnumSpecificType.None, "Unknown conversion result: " + errorText);
			}
		}
	}
}
