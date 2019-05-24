using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Vector.VLConfig.LoggingNavigator.Data;

namespace Vector.VLConfig.LoggingNavigator.Export
{
	public class FileCopyDialog : Form
	{
		private delegate void FileCopyProgressChanged(FileCopyDialog.CopyProgressInfo info);

		private delegate void FileCopyError(FileCopyDialog.CopyProgressError error);

		private class CopyProgressInfo
		{
			private string mName;

			private long mBytes;

			private long mMaxbytes;

			private long mCurrentCount;

			private long mTotalCount;

			public string Name
			{
				get
				{
					return this.mName;
				}
				set
				{
					this.mName = value;
				}
			}

			public long Bytes
			{
				get
				{
					return this.mBytes;
				}
				set
				{
					this.mBytes = value;
				}
			}

			public long Maxbytes
			{
				get
				{
					return this.mMaxbytes;
				}
				set
				{
					this.mMaxbytes = value;
				}
			}

			public long CurrentFileNr
			{
				get
				{
					return this.mCurrentCount;
				}
				set
				{
					this.mCurrentCount = value;
				}
			}

			public long TotalFileCount
			{
				get
				{
					return this.mTotalCount;
				}
				set
				{
					this.mTotalCount = value;
				}
			}

			public CopyProgressInfo(string name, long bytes, long maxbytes, long currentCount, long totalCount)
			{
				this.mName = name;
				this.mBytes = bytes;
				this.mMaxbytes = maxbytes;
				this.mCurrentCount = currentCount;
				this.mTotalCount = totalCount;
			}
		}

		private class CopyProgressError
		{
			private string mMsg;

			private string mPath;

			private DialogResult mResult;

			public string Msg
			{
				get
				{
					return this.mMsg;
				}
				set
				{
					this.mMsg = value;
				}
			}

			public string Path
			{
				get
				{
					return this.mPath;
				}
				set
				{
					this.mPath = value;
				}
			}

			public DialogResult Result
			{
				get
				{
					return this.mResult;
				}
				set
				{
					this.mResult = value;
				}
			}

			public CopyProgressError(Exception ex, string path)
			{
				this.mMsg = ex.Message;
				this.mPath = path;
				this.mResult = DialogResult.Cancel;
			}
		}

		private BackgroundWorker mFileCopyWorker;

		private FileCopyDialog.FileCopyProgressChanged OnFileProgressChanged;

		private FileCopyDialog.FileCopyError OnFileCopyError;

		private string mTargetPath;

		private string mConversionBatchFilePath = "Convert.bat";

		private IContainer components;

		private ProgressBar mProgressBarCopy;

		private Button mButtonCopyCancel;

		private TextBox mTextBoxCopyStatus;

		public FileCopyDialog()
		{
			this.InitializeComponent();
			this.mFileCopyWorker = new BackgroundWorker();
			this.mFileCopyWorker.DoWork += new DoWorkEventHandler(this.FileCopyDialog_DoWork);
			this.mFileCopyWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.FileCopyDialog_RunWorkerCompleted);
			this.mFileCopyWorker.WorkerSupportsCancellation = true;
			this.OnFileProgressChanged = (FileCopyDialog.FileCopyProgressChanged)Delegate.Combine(this.OnFileProgressChanged, new FileCopyDialog.FileCopyProgressChanged(this.FileCopyDialog_FileCopyProgressChanged));
			this.OnFileCopyError = (FileCopyDialog.FileCopyError)Delegate.Combine(this.OnFileCopyError, new FileCopyDialog.FileCopyError(this.FileCopyDialog_FileCopyError));
		}

		private void FileCopyDialog_DoWork(object sender, DoWorkEventArgs e)
		{
			long num = 0L;
			if (!Directory.Exists(this.mTargetPath))
			{
				Directory.CreateDirectory(this.mTargetPath);
			}
			FileAccessManager instance = FileAccessManager.GetInstance();
			string text = "";
			long num2 = 0L;
			long maxbytes = 0L;
			long currentCount = 0L;
			long totalCount = 0L;
			while (instance.GetNextFileToCopy(ref text, ref num2, ref maxbytes, ref currentCount, ref totalCount))
			{
				try
				{
					base.BeginInvoke(this.OnFileProgressChanged, new object[]
					{
						new FileCopyDialog.CopyProgressInfo(text, num, maxbytes, currentCount, totalCount)
					});
					instance.CopyNextFileToDestination(this.mTargetPath);
				}
				catch (Exception ex)
				{
					FileCopyDialog.CopyProgressError copyProgressError = new FileCopyDialog.CopyProgressError(ex, text);
					base.Invoke(this.OnFileCopyError, new object[]
					{
						copyProgressError
					});
					if (copyProgressError.Result == DialogResult.Cancel)
					{
						base.DialogResult = DialogResult.Cancel;
						break;
					}
				}
				num += num2;
			}
		}

		private void FileCopyDialog_FileCopyProgressChanged(FileCopyDialog.CopyProgressInfo info)
		{
			this.mProgressBarCopy.Value = (int)(100.0 * (double)info.Bytes / (double)info.Maxbytes);
			this.mTextBoxCopyStatus.Text = "Copying " + info.Name;
			TextBox expr_46 = this.mTextBoxCopyStatus;
			object text = expr_46.Text;
			expr_46.Text = string.Concat(new object[]
			{
				text,
				"  (",
				info.CurrentFileNr,
				"/",
				info.TotalFileCount,
				")"
			});
		}

		private void FileCopyDialog_FileCopyError(FileCopyDialog.CopyProgressError error)
		{
			string arg = "Please make sure you have write access to \"" + this.mTargetPath + "\".";
			string text = string.Format("Error copying file {0}\n{1}\nClick OK to continue copying files", error.Path, arg);
			error.Result = MessageBox.Show(text, "Error while copying files", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
		}

		private void FileCopyDialog_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (base.DialogResult == DialogResult.Cancel)
			{
				return;
			}
			this.mTextBoxCopyStatus.Text = "Executing Conversion...";
			this.ExecuteConversionBatch();
			this.mTextBoxCopyStatus.Text = "Done.";
			base.DialogResult = DialogResult.OK;
		}

		private void mButtonCopyCancel_Click(object sender, EventArgs e)
		{
			this.mFileCopyWorker.CancelAsync();
			base.DialogResult = DialogResult.Cancel;
		}

		private void FileCopyDialog_Shown(object sender, EventArgs e)
		{
			this.mFileCopyWorker.RunWorkerAsync();
		}

		private void ExecuteConversionBatch()
		{
			Process process = new Process();
			try
			{
				process.StartInfo.UseShellExecute = true;
				process.StartInfo.FileName = this.mConversionBatchFilePath;
				process.StartInfo.CreateNoWindow = false;
				process.StartInfo.Arguments = "\"" + this.mTargetPath + "\"";
				process.Start();
				process.WaitForExit();
			}
			catch (Exception ex)
			{
				string text = "Unable to convert log files. An error occurred during the execution of the conversion script!";
				MessageBox.Show(text, "Error while converting files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				Console.WriteLine(ex.Message);
			}
		}

		public void SetFilesToCopy(string path, IList<ExportJob> jobs, string targetPath)
		{
			this.mTargetPath = targetPath;
			FileAccessManager instance = FileAccessManager.GetInstance();
			instance.SetFilesToCopy(path, jobs);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.mProgressBarCopy = new ProgressBar();
			this.mButtonCopyCancel = new Button();
			this.mTextBoxCopyStatus = new TextBox();
			base.SuspendLayout();
			this.mProgressBarCopy.Location = new Point(13, 39);
			this.mProgressBarCopy.Name = "mProgressBarCopy";
			this.mProgressBarCopy.Size = new Size(259, 23);
			this.mProgressBarCopy.TabIndex = 0;
			this.mButtonCopyCancel.Location = new Point(105, 68);
			this.mButtonCopyCancel.Name = "mButtonCopyCancel";
			this.mButtonCopyCancel.Size = new Size(75, 23);
			this.mButtonCopyCancel.TabIndex = 2;
			this.mButtonCopyCancel.Text = "Cancel";
			this.mButtonCopyCancel.UseVisualStyleBackColor = true;
			this.mButtonCopyCancel.Click += new EventHandler(this.mButtonCopyCancel_Click);
			this.mTextBoxCopyStatus.Location = new Point(13, 13);
			this.mTextBoxCopyStatus.Name = "mTextBoxCopyStatus";
			this.mTextBoxCopyStatus.ReadOnly = true;
			this.mTextBoxCopyStatus.Size = new Size(259, 20);
			this.mTextBoxCopyStatus.TabIndex = 3;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(284, 99);
			base.Controls.Add(this.mTextBoxCopyStatus);
			base.Controls.Add(this.mButtonCopyCancel);
			base.Controls.Add(this.mProgressBarCopy);
			base.Name = "FileCopyDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			this.Text = "Export Status";
			base.Shown += new EventHandler(this.FileCopyDialog_Shown);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
