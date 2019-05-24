using System;
using System.ComponentModel;
using System.IO;

namespace Vector.VLConfig.GeneralUtil.GUI
{
	public class GenericCopyFilesWithProgressIndicatorJob : GenericBackgroundWorkerJob
	{
		private static readonly double sBytesToMBytes = Math.Pow(2.0, 20.0);

		private readonly string mDestinationFileName;

		private readonly bool mOverwrite;

		public string SourceFileName
		{
			get;
			private set;
		}

		public GenericCopyFilesWithProgressIndicatorJob(string sourceFileName, string destinationFileName, bool overwrite)
		{
			this.SourceFileName = sourceFileName;
			this.mDestinationFileName = destinationFileName;
			this.mOverwrite = overwrite;
			FileInfo fileInfo = new FileInfo(this.SourceFileName);
			if (fileInfo.Exists)
			{
				base.Weight = (int)Math.Ceiling((double)fileInfo.Length / GenericCopyFilesWithProgressIndicatorJob.sBytesToMBytes);
			}
		}

		protected override void OnDoWork(object sender, DoWorkEventArgs e)
		{
			BackgroundWorker backgroundWorker = sender as BackgroundWorker;
			GenericCopyFilesWithProgressIndicatorJob genericCopyFilesWithProgressIndicatorJob = e.Argument as GenericCopyFilesWithProgressIndicatorJob;
			if (backgroundWorker == null || genericCopyFilesWithProgressIndicatorJob != this)
			{
				e.Result = new GenericCopyFilesWithProgressIndicatorResult(GenericBackgroundWorkerResult.ResultType.InternalError, EnumCopyFilesResult.None, "");
				return;
			}
			if (File.Exists(this.mDestinationFileName) && !this.mOverwrite)
			{
				e.Result = new GenericCopyFilesWithProgressIndicatorResult(GenericBackgroundWorkerResult.ResultType.Error, EnumCopyFilesResult.DestinationFileAlreadyExists, "");
				return;
			}
			string directoryName = Path.GetDirectoryName(this.mDestinationFileName);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			e.Result = new GenericCopyFilesWithProgressIndicatorResult(GenericBackgroundWorkerResult.ResultType.Success, EnumCopyFilesResult.None, "");
			byte[] array = new byte[1048576];
			try
			{
				using (FileStream fileStream = new FileStream(this.SourceFileName, FileMode.Open, FileAccess.Read))
				{
					long length = fileStream.Length;
					using (FileStream fileStream2 = new FileStream(this.mDestinationFileName, FileMode.Create, FileAccess.Write))
					{
						long num = 0L;
						int num2 = 0;
						int num3;
						while ((num3 = fileStream.Read(array, 0, array.Length)) > 0)
						{
							fileStream2.Write(array, 0, num3);
							num += (long)num3;
							int num4 = (int)Math.Floor((double)num * 100.0 / (double)length);
							if (num2 != num4)
							{
								num2 = num4;
								backgroundWorker.ReportProgress(num4);
							}
							if (backgroundWorker.CancellationPending)
							{
								e.Result = new GenericCopyFilesWithProgressIndicatorResult(GenericBackgroundWorkerResult.ResultType.CanceledByUser, EnumCopyFilesResult.None, "");
								e.Cancel = true;
								break;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				e.Result = new GenericCopyFilesWithProgressIndicatorResult(GenericBackgroundWorkerResult.ResultType.Error, EnumCopyFilesResult.None, string.Concat(new string[]
				{
					ex.Message,
					Environment.NewLine,
					Path.GetFileName(this.SourceFileName),
					" -> ",
					Path.GetFileName(this.mDestinationFileName)
				}));
			}
		}
	}
}
