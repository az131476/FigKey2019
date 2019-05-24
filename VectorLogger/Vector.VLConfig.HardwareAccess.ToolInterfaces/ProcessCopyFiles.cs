using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public class ProcessCopyFiles
	{
		private Thread copyThread;

		private IProgressIndicator indicator;

		private string sourceBasePath;

		private string destPath;

		private IList<string> sourceFilePaths;

		private bool replicateLeafFoldersInDestPath;

		private bool isOverwriteActive;

		private CultureInfo initialCulture;

		private ProcessExitedDelegate threadExitedDelegate;

		public string ErrorText
		{
			get;
			set;
		}

		public ProcessCopyFiles(string sourceBaseFolderPath, IList<string> sourceFilePathsRelativeToBase, string destinationPath, IProgressIndicator progressIndicator, ProcessExitedDelegate processExitedDelegate)
		{
			this.sourceBasePath = sourceBaseFolderPath;
			this.sourceFilePaths = sourceFilePathsRelativeToBase;
			this.destPath = destinationPath;
			this.indicator = progressIndicator;
			this.replicateLeafFoldersInDestPath = true;
			this.isOverwriteActive = true;
			this.threadExitedDelegate = processExitedDelegate;
			this.initialCulture = new CultureInfo(Thread.CurrentThread.CurrentUICulture.Name);
		}

		public void Execute()
		{
			this.ErrorText = "";
			this.copyThread = new Thread(new ThreadStart(this.CopyThreadProc));
			this.copyThread.Name = "CopyFiles";
			this.copyThread.Start();
			this.copyThread.Join();
		}

		public void Cancel()
		{
			if (this.copyThread.IsAlive)
			{
				this.copyThread.Abort();
			}
		}

		private void CopyThreadProc()
		{
			Thread.CurrentThread.CurrentUICulture = this.initialCulture;
			int num = this.sourceFilePaths.Count<string>();
			if (num > 0)
			{
				this.indicator.SetMinMax(0, num);
				this.indicator.SetStatusText(string.Format(Resources.CopyFileNumFromTotalFiles, 0, num));
				this.indicator.SetValue(0);
				int num2 = 0;
				string text = "";
				foreach (string current in this.sourceFilePaths)
				{
					if (this.indicator.Cancelled())
					{
						break;
					}
					if (!this.replicateLeafFoldersInDestPath)
					{
						goto IL_16B;
					}
					try
					{
						string directoryName = Path.GetDirectoryName(current);
						if (!string.IsNullOrEmpty(directoryName))
						{
							text = Path.Combine(this.destPath, directoryName);
							if (!Directory.Exists(text))
							{
								Directory.CreateDirectory(text);
							}
						}
						else
						{
							text = this.destPath;
						}
					}
					catch (Exception)
					{
						this.ErrorText = string.Format(Resources.ErrorUnableToCreateCorrespSubFolder, current, this.destPath);
						this.threadExitedDelegate();
						break;
					}
					string text2 = Path.Combine(text, Path.GetFileName(current));
					if (!File.Exists(text2))
					{
						try
						{
							FileProxy.Copy(Path.Combine(this.sourceBasePath, current), text2, this.isOverwriteActive);
							goto IL_1B8;
						}
						catch (Exception)
						{
							this.ErrorText = string.Format(Resources.CannotCopyFileTo, current, text);
							this.threadExitedDelegate();
							break;
						}
						goto Block_7;
					}
					IL_1B8:
					num2++;
					this.indicator.SetStatusText(string.Format(Resources.CopyFileNumFromTotalFiles, num2, num));
					this.indicator.SetValue(num2);
					continue;
					Block_7:
					try
					{
						IL_16B:
						FileProxy.Copy(current, Path.Combine(this.destPath, Path.GetFileName(current)), this.isOverwriteActive);
					}
					catch (Exception)
					{
						this.ErrorText = string.Format(Resources.CannotCopyFileTo, current, this.destPath);
						this.threadExitedDelegate();
						break;
					}
					goto IL_1B8;
				}
			}
		}
	}
}
