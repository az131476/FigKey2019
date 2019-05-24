using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class DecompressFilesWithProgressIndicator : Form
	{
		private string pathToFolder;

		private bool isCancelled;

		private bool isUsingSubFolders;

		private IContainer components;

		private Button buttonCancel;

		private TextBox textBoxStatus;

		private ProgressBar progressBar;

		public DecompressFilesWithProgressIndicator(string filePathToCompressedFiles, bool useSubFolders, string titleText)
		{
			this.InitializeComponent();
			this.pathToFolder = filePathToCompressedFiles;
			this.isCancelled = false;
			if (!string.IsNullOrEmpty(titleText))
			{
				this.Text = titleText;
			}
			this.isUsingSubFolders = useSubFolders;
		}

		private void DecompressFilesWithProgressIndicator_Shown(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.pathToFolder))
			{
				base.DialogResult = DialogResult.Cancel;
				return;
			}
			GZip gZip = new GZip();
			List<string> list = new List<string>();
			if (this.isUsingSubFolders)
			{
				string[] directories = Directory.GetDirectories(this.pathToFolder, Constants.LogDataFolderSearchPattern, SearchOption.TopDirectoryOnly);
				string[] array = directories;
				for (int i = 0; i < array.Length; i++)
				{
					string path = array[i];
					list.AddRange(Directory.EnumerateFiles(path, "*.gz"));
				}
			}
			else
			{
				list.AddRange(Directory.EnumerateFiles(this.pathToFolder, "*.gz"));
			}
			int num = list.Count<string>();
			this.progressBar.Maximum = num;
			this.textBoxStatus.Text = string.Format(Resources.NumOfTotalDecompressed, 0, num);
			TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.Normal);
			TaskbarProgress.SetValue(0.0, (double)num);
			int num2 = 0;
			foreach (string current in list)
			{
				Application.DoEvents();
				if (this.isCancelled)
				{
					base.DialogResult = DialogResult.Cancel;
					TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
					return;
				}
				string message;
				if (!gZip.DecompressFile(current, out message))
				{
					InformMessageBox.Error(message);
					base.DialogResult = DialogResult.Cancel;
					TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
					return;
				}
				num2++;
				this.textBoxStatus.Text = string.Format(Resources.NumOfTotalDecompressed, num2, num);
				this.progressBar.Increment(1);
				TaskbarProgress.SetValue((double)num2, (double)num);
			}
			base.DialogResult = DialogResult.OK;
			TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.isCancelled = true;
		}

		private void DecompressFilesWithProgressIndicator_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DecompressFilesWithProgressIndicator));
			this.buttonCancel = new Button();
			this.textBoxStatus = new TextBox();
			this.progressBar = new ProgressBar();
			base.SuspendLayout();
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this.textBoxStatus, "textBoxStatus");
			this.textBoxStatus.Name = "textBoxStatus";
			this.textBoxStatus.ReadOnly = true;
			componentResourceManager.ApplyResources(this.progressBar, "progressBar");
			this.progressBar.Name = "progressBar";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.progressBar);
			base.Controls.Add(this.textBoxStatus);
			base.Controls.Add(this.buttonCancel);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DecompressFilesWithProgressIndicator";
			base.ShowInTaskbar = false;
			base.Shown += new EventHandler(this.DecompressFilesWithProgressIndicator_Shown);
			base.HelpRequested += new HelpEventHandler(this.DecompressFilesWithProgressIndicator_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
