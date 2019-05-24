using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class CopyFilesWithProgressIndicatorFromZip : Form
	{
		private IList<string> sourceFiles;

		private string destinationPath;

		private string sourceBasePath;

		private bool overwrite;

		private bool isCancelled;

		private bool replicateLeafFoldersInDestPath;

		private ZipFile zipFile;

		private IContainer components;

		private Button buttonCancel;

		private TextBox textBoxStatus;

		private ProgressBar progressBar;

		public CopyFilesWithProgressIndicatorFromZip(ZipFile sourceBaseZipFile, string sourceBaseFolderPath, IList<string> sourceFileList, string destinationPath, bool replicateLeafFoldersInDestination, bool overwriteExisting, string titleText)
		{
			this.InitializeComponent();
			this.sourceBasePath = sourceBaseFolderPath;
			this.sourceFiles = sourceFileList;
			this.destinationPath = destinationPath;
			this.overwrite = overwriteExisting;
			this.replicateLeafFoldersInDestPath = replicateLeafFoldersInDestination;
			this.isCancelled = false;
			this.zipFile = sourceBaseZipFile;
			if (!string.IsNullOrEmpty(titleText))
			{
				this.Text = titleText;
			}
		}

		private void CopyFilesWithProgressIndicatorFromZip_Shown(object sender, EventArgs e)
		{
			if (this.sourceFiles == null)
			{
				base.DialogResult = DialogResult.Cancel;
				return;
			}
			int num = this.sourceFiles.Count<string>();
			this.progressBar.Maximum = num;
			this.textBoxStatus.Text = string.Format(Resources.CopyFileNumFromTotalFiles, 0, num);
			TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.Normal);
			TaskbarProgress.SetValue(0.0, (double)num);
			int num2 = 0;
			string text = "";
			foreach (string current in this.sourceFiles)
			{
				Application.DoEvents();
				if (this.isCancelled)
				{
					base.DialogResult = DialogResult.Cancel;
					TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
					return;
				}
				if (!this.replicateLeafFoldersInDestPath)
				{
					goto IL_18A;
				}
				try
				{
					string directoryName = Path.GetDirectoryName(current);
					if (!string.IsNullOrEmpty(directoryName))
					{
						text = Path.Combine(this.destinationPath, directoryName);
						if (!Directory.Exists(text))
						{
							Directory.CreateDirectory(text);
						}
					}
					else
					{
						text = this.destinationPath;
					}
				}
				catch (Exception)
				{
					InformMessageBox.Error(string.Format(Resources.ErrorUnableToCreateCorrespSubFolder, current, this.destinationPath));
					base.DialogResult = DialogResult.Cancel;
					TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
					return;
				}
				string text2 = Path.Combine(text, Path.GetFileName(current));
				if (!File.Exists(text2))
				{
					try
					{
						this.zipFile.FlattenFoldersOnExtract = false;
						ZipEntry zipEntry = this.zipFile[current];
						zipEntry.Extract(text2, ExtractExistingFileAction.OverwriteSilently);
						goto IL_1E2;
					}
					catch (Exception)
					{
						InformMessageBox.Error(string.Format(Resources.CannotCopyFileTo, current, text));
						base.DialogResult = DialogResult.Cancel;
						TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
						return;
					}
					goto Block_7;
				}
				IL_1E2:
				num2++;
				this.textBoxStatus.Text = string.Format(Resources.CopyFileNumFromTotalFiles, num2, num);
				this.progressBar.Increment(1);
				TaskbarProgress.SetValue((double)num2, (double)num);
				continue;
				Block_7:
				try
				{
					IL_18A:
					this.zipFile.FlattenFoldersOnExtract = true;
					ZipEntry zipEntry2 = this.zipFile[current];
					zipEntry2.Extract(this.destinationPath, ExtractExistingFileAction.OverwriteSilently);
				}
				catch (Exception)
				{
					InformMessageBox.Error(string.Format(Resources.CannotCopyFileTo, current, this.destinationPath));
					base.DialogResult = DialogResult.Cancel;
					TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
					return;
				}
				goto IL_1E2;
			}
			base.DialogResult = DialogResult.OK;
			TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.isCancelled = true;
		}

		private void CopyFilesWithProgressIndicatorFromZip_HelpRequested(object sender, HelpEventArgs hlpevent)
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CopyFilesWithProgressIndicatorFromZip));
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
			base.Name = "CopyFilesWithProgressIndicatorFromZip";
			base.ShowInTaskbar = false;
			base.Shown += new EventHandler(this.CopyFilesWithProgressIndicatorFromZip_Shown);
			base.HelpRequested += new HelpEventHandler(this.CopyFilesWithProgressIndicatorFromZip_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
