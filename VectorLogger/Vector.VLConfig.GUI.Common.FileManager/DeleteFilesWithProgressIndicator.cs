using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class DeleteFilesWithProgressIndicator : Form
	{
		private string[] filesToDelete;

		private bool isCancelled;

		private IContainer components;

		private TextBox textBoxStatus;

		private ProgressBar progressBar;

		private Button buttonCancel;

		public DeleteFilesWithProgressIndicator(string[] filesToDeleteList, string titleText)
		{
			this.InitializeComponent();
			this.filesToDelete = filesToDeleteList;
			this.isCancelled = false;
			if (!string.IsNullOrEmpty(titleText))
			{
				this.Text = titleText;
			}
		}

		private void DeleteFilesWithProgressIndicator_Shown(object sender, EventArgs e)
		{
			if (this.filesToDelete == null)
			{
				base.DialogResult = DialogResult.Cancel;
				return;
			}
			int num = this.filesToDelete.Count<string>();
			this.progressBar.Maximum = num;
			this.textBoxStatus.Text = string.Format(Resources.DeleteFileNumFromTotalFiles, 0, num);
			int num2 = 0;
			string[] array = this.filesToDelete;
			int i = 0;
			while (i < array.Length)
			{
				string text = array[i];
				Application.DoEvents();
				if (!this.isCancelled)
				{
					try
					{
						File.Delete(text);
					}
					catch (Exception)
					{
						InformMessageBox.Error(string.Format(Resources.CannotDeleteFile, text));
						base.DialogResult = DialogResult.Cancel;
						return;
					}
					num2++;
					this.textBoxStatus.Text = string.Format(Resources.DeleteFileNumFromTotalFiles, num2, num);
					this.progressBar.Increment(1);
					i++;
					continue;
				}
				base.DialogResult = DialogResult.Cancel;
				return;
			}
			base.DialogResult = DialogResult.OK;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.isCancelled = true;
		}

		private void DeleteFilesWithProgressIndicator_HelpRequested(object sender, HelpEventArgs hlpevent)
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DeleteFilesWithProgressIndicator));
			this.textBoxStatus = new TextBox();
			this.progressBar = new ProgressBar();
			this.buttonCancel = new Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.textBoxStatus, "textBoxStatus");
			this.textBoxStatus.Name = "textBoxStatus";
			this.textBoxStatus.ReadOnly = true;
			componentResourceManager.ApplyResources(this.progressBar, "progressBar");
			this.progressBar.Name = "progressBar";
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.progressBar);
			base.Controls.Add(this.textBoxStatus);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DeleteFilesWithProgressIndicator";
			base.ShowInTaskbar = false;
			base.Shown += new EventHandler(this.DeleteFilesWithProgressIndicator_Shown);
			base.HelpRequested += new HelpEventHandler(this.DeleteFilesWithProgressIndicator_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
