using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Vector.VLConfig.GeneralUtil.GUI
{
	public class GenericCopyFilesWithProgressIndicator : GenericBackgroundWorker
	{
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

		public GenericCopyFilesWithProgressIndicator(string progressFormTitle)
		{
			this.mProgressFormTitle = progressFormTitle;
		}

		public override string GetProgressStatusText(int finishedJobs, IList<GenericBackgroundWorkerJob> runningJobs)
		{
			if (finishedJobs == base.Jobs.Count || !runningJobs.Any<GenericBackgroundWorkerJob>())
			{
				return Resources_General.CopyFilesFinished;
			}
			return string.Format(Resources_General.CopyFilesStatusText, finishedJobs, base.Jobs.Count, Path.GetFileName(((GenericCopyFilesWithProgressIndicatorJob)runningJobs.Last<GenericBackgroundWorkerJob>()).SourceFileName));
		}

		public GenericCopyFilesWithProgressIndicatorResult CopyFiles(IList<string> sourceFileList, string destinationPath, bool overwriteExisting)
		{
			return this.CopyFiles(string.Empty, sourceFileList, destinationPath, false, overwriteExisting);
		}

		public GenericCopyFilesWithProgressIndicatorResult CopyFiles(string sourceBaseFolderPath, IList<string> sourceFileList, string destinationPath, bool replicateLeafFoldersInDestination, bool overwriteExisting)
		{
			base.Jobs.Clear();
			foreach (string current in sourceFileList)
			{
				if (replicateLeafFoldersInDestination)
				{
					string text = Path.GetDirectoryName(current) ?? string.Empty;
					text = text.Remove(text.IndexOf(sourceBaseFolderPath), sourceBaseFolderPath.Length).TrimStart(new char[]
					{
						'/',
						'\\'
					});
					string path = (!string.IsNullOrEmpty(text)) ? Path.Combine(destinationPath, text) : destinationPath;
					string destinationFileName = Path.Combine(path, Path.GetFileName(current) ?? string.Empty);
					base.Jobs.Add(new GenericCopyFilesWithProgressIndicatorJob(current, destinationFileName, overwriteExisting));
				}
				else
				{
					string destinationFileName2 = Path.Combine(destinationPath, Path.GetFileName(current) ?? string.Empty);
					base.Jobs.Add(new GenericCopyFilesWithProgressIndicatorJob(current, destinationFileName2, overwriteExisting));
				}
			}
			if (DialogResult.Cancel == base.ExecuteJobs())
			{
				return new GenericCopyFilesWithProgressIndicatorResult(GenericBackgroundWorkerResult.ResultType.CanceledByUser, EnumCopyFilesResult.None, "");
			}
			return (GenericCopyFilesWithProgressIndicatorResult)base.Result;
		}
	}
}
