using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class CopyFilesWithProgressIndicator : Form
	{
		private IList<string> sourceFiles;

		private string destinationPath;

		private string sourceBasePath;

		private bool overwrite;

		private bool isCancelled;

		private bool replicateLeafFoldersInDestPath;

		private IContainer components;

		private Button buttonCancel;

		private TextBox textBoxStatus;

		private ProgressBar progressBar;

		public CopyFilesWithProgressIndicator(IList<string> sourceFileList, string destinationPath, bool overwriteExisting, string titleText) : this("", sourceFileList, destinationPath, false, overwriteExisting, titleText)
		{
		}

		public CopyFilesWithProgressIndicator(string sourceBaseFolderPath, IList<string> sourceFileList, string destinationPath, bool replicateLeafFoldersInDestination, bool overwriteExisting, string titleText)
		{
			this.InitializeComponent();
			this.sourceBasePath = sourceBaseFolderPath.TrimEnd(new char[]
			{
				'/',
				'\\'
			});
			this.sourceFiles = sourceFileList;
			this.destinationPath = destinationPath;
			this.overwrite = overwriteExisting;
			this.replicateLeafFoldersInDestPath = replicateLeafFoldersInDestination;
			this.isCancelled = false;
			if (!string.IsNullOrEmpty(titleText))
			{
				this.Text = titleText;
			}
		}

		private void CopyFilesWithProgressIndicator_Shown(object sender, EventArgs e)
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
					goto IL_1C7;
				}
				try
				{
					string text2 = Path.GetDirectoryName(current) ?? string.Empty;
					text2 = text2.Remove(text2.IndexOf(this.sourceBasePath), this.sourceBasePath.Length).TrimStart(new char[]
					{
						'/',
						'\\'
					});
					if (!string.IsNullOrEmpty(text2))
					{
						text = Path.Combine(this.destinationPath, text2);
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
				string text3 = Path.Combine(text, Path.GetFileName(current));
				if (!File.Exists(text3))
				{
					try
					{
						FileProxy.Copy(Path.Combine(this.sourceBasePath, current), text3, this.overwrite);
						goto IL_215;
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
				IL_215:
				num2++;
				this.textBoxStatus.Text = string.Format(Resources.CopyFileNumFromTotalFiles, num2, num);
				this.progressBar.Increment(1);
				TaskbarProgress.SetValue((double)num2, (double)num);
				continue;
				Block_7:
				try
				{
					IL_1C7:
					FileProxy.Copy(current, Path.Combine(this.destinationPath, Path.GetFileName(current)), this.overwrite);
				}
				catch (Exception)
				{
					InformMessageBox.Error(string.Format(Resources.CannotCopyFileTo, current, this.destinationPath));
					base.DialogResult = DialogResult.Cancel;
					TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
					return;
				}
				goto IL_215;
			}
			base.DialogResult = DialogResult.OK;
			TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.isCancelled = true;
		}

		private void CopyFilesWithProgressIndicator_HelpRequested(object sender, HelpEventArgs hlpevent)
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CopyFilesWithProgressIndicator));
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
			base.Name = "CopyFilesWithProgressIndicator";
			base.ShowInTaskbar = false;
			base.Shown += new EventHandler(this.CopyFilesWithProgressIndicator_Shown);
			base.HelpRequested += new HelpEventHandler(this.CopyFilesWithProgressIndicator_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
