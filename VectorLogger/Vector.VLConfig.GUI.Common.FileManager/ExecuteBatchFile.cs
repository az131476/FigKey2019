using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class ExecuteBatchFile : Form
	{
		private string batchFilePath;

		private string commandLineArguments;

		private BackgroundWorker backgroundWorker;

		private string errorText;

		private bool isAbortedByUser;

		private IContainer components;

		private TextBox textBoxStatus;

		private Button buttonCancel;

		public string ErrorText
		{
			get
			{
				return this.errorText;
			}
		}

		public bool IsAbortedByUser
		{
			get
			{
				return this.isAbortedByUser;
			}
		}

		public ExecuteBatchFile(string filePath, IList<string> commandLineArgs)
		{
			this.InitializeComponent();
			this.batchFilePath = filePath;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (string current in commandLineArgs)
			{
				stringBuilder.Append("\"" + current + "\" ");
			}
			this.commandLineArguments = stringBuilder.ToString();
			this.errorText = "";
			this.isAbortedByUser = false;
		}

		private void InitializeBackgroundWorker()
		{
			this.backgroundWorker = new BackgroundWorker();
			this.backgroundWorker.WorkerReportsProgress = false;
			this.backgroundWorker.WorkerSupportsCancellation = true;
			this.backgroundWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
		}

		private void ExecuteBatchFile_Shown(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.batchFilePath) || !File.Exists(this.batchFilePath))
			{
				base.DialogResult = DialogResult.Cancel;
				this.errorText = string.Format("Failed to execute batch file '{0}'", this.batchFilePath);
				return;
			}
			this.InitializeBackgroundWorker();
			this.backgroundWorker.RunWorkerAsync();
			string text = string.Format("Executing batch file '{0} {1}'.", Path.GetFileName(this.batchFilePath), this.commandLineArguments);
			string text2 = text;
			while (this.backgroundWorker.IsBusy)
			{
				this.textBoxStatus.Text = text2;
				text2 += ".";
				if (text2.Length - text.Length >= 40)
				{
					text2 = text;
				}
				Thread.Sleep(300);
				base.Invalidate();
				Application.DoEvents();
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			if (this.backgroundWorker.IsBusy && !this.backgroundWorker.CancellationPending)
			{
				this.isAbortedByUser = true;
				this.backgroundWorker.CancelAsync();
			}
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			Process process = new Process();
			try
			{
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.FileName = this.batchFilePath;
				process.StartInfo.CreateNoWindow = false;
				process.StartInfo.Arguments = this.commandLineArguments;
				process.Start();
				goto IL_97;
			}
			catch (Exception)
			{
				this.errorText = string.Format("Failed to execute batch file '{0}'", this.batchFilePath);
				base.DialogResult = DialogResult.Cancel;
				goto IL_97;
			}
			IL_6F:
			if (this.backgroundWorker.CancellationPending)
			{
				try
				{
					process.Kill();
				}
				catch (Exception)
				{
				}
			}
			Thread.Sleep(100);
			IL_97:
			if (process.HasExited)
			{
				return;
			}
			goto IL_6F;
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			if (this.isAbortedByUser)
			{
				base.DialogResult = DialogResult.Cancel;
				return;
			}
			base.DialogResult = DialogResult.OK;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ExecuteBatchFile));
			this.textBoxStatus = new TextBox();
			this.buttonCancel = new Button();
			base.SuspendLayout();
			this.textBoxStatus.Location = new Point(48, 12);
			this.textBoxStatus.Name = "textBoxStatus";
			this.textBoxStatus.ReadOnly = true;
			this.textBoxStatus.Size = new Size(520, 20);
			this.textBoxStatus.TabIndex = 0;
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Location = new Point(272, 49);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(75, 23);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ClientSize = new Size(614, 84);
			base.ControlBox = false;
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.textBoxStatus);
			this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "ExecuteBatchFile";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "ExecuteBatchFile";
			base.Shown += new EventHandler(this.ExecuteBatchFile_Shown);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
