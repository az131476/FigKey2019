using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class DeleteFoldersWithProgressIndicator : Form
	{
		private string path;

		private string subFolderMask;

		private bool isCancelled;

		private IContainer components;

		private TextBox textBoxStatus;

		private ProgressBar progressBar;

		private Button buttonCancel;

		public DeleteFoldersWithProgressIndicator(string path, string subfolderMask, string titleText)
		{
			this.InitializeComponent();
			this.path = path;
			this.subFolderMask = subfolderMask;
			this.isCancelled = false;
			if (!string.IsNullOrEmpty(titleText))
			{
				this.Text = titleText;
			}
		}

		private void DeleteFilesWithProgressIndicator_Shown(object sender, EventArgs e)
		{
			if (this.path == null)
			{
				base.DialogResult = DialogResult.Cancel;
				return;
			}
			int num = 0;
			string[] files = Directory.GetFiles(this.path);
			string[] directories = Directory.GetDirectories(this.path, this.subFolderMask);
			num += files.Length;
			string[] array = directories;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				string[] files2 = Directory.GetFiles(text);
				num += files2.Length;
			}
			this.progressBar.Maximum = num;
			this.textBoxStatus.Text = string.Format(Resources.DeleteFileNumFromTotalFiles, 0, num);
			int num2 = 0;
			string[] array2 = files;
			int j = 0;
			while (j < array2.Length)
			{
				string arg = array2[j];
				Application.DoEvents();
				if (!this.isCancelled)
				{
					try
					{
						File.Delete(arg);
					}
					catch (Exception)
					{
						InformMessageBox.Error(string.Format(Resources.CannotDeleteFile, arg));
						base.DialogResult = DialogResult.Cancel;
						return;
					}
					num2++;
					this.textBoxStatus.Text = string.Format(Resources.DeleteFileNumFromTotalFiles, num2, num);
					this.progressBar.Increment(1);
					j++;
					continue;
				}
				base.DialogResult = DialogResult.Cancel;
				return;
			}
			string[] array3 = directories;
			for (int k = 0; k < array3.Length; k++)
			{
				string arg2 = array3[k];
				string[] files3 = Directory.GetFiles(arg2);
				string[] array4 = files3;
				for (int l = 0; l < array4.Length; l++)
				{
					string arg3 = array4[l];
					Application.DoEvents();
					if (this.isCancelled)
					{
						base.DialogResult = DialogResult.Cancel;
						return;
					}
					try
					{
						File.Delete(arg3);
					}
					catch (Exception)
					{
						InformMessageBox.Error(string.Format(Resources.CannotDeleteFile, arg3));
						base.DialogResult = DialogResult.Cancel;
						return;
					}
					num2++;
					this.textBoxStatus.Text = string.Format(Resources.DeleteFileNumFromTotalFiles, num2, num);
					this.progressBar.Increment(1);
				}
				try
				{
					Directory.Delete(arg2);
				}
				catch (Exception)
				{
					InformMessageBox.Error(string.Format(Resources.CannotDeleteDirectory, arg2));
					base.DialogResult = DialogResult.Cancel;
					return;
				}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DeleteFoldersWithProgressIndicator));
			this.textBoxStatus = new TextBox();
			this.progressBar = new ProgressBar();
			this.buttonCancel = new Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.textBoxStatus, "textBoxStatus");
			this.textBoxStatus.Name = "textBoxStatus";
			this.textBoxStatus.ReadOnly = true;
			componentResourceManager.ApplyResources(this.progressBar, "progressBar");
			this.progressBar.Name = "progressBar";
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
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
			base.Name = "DeleteFoldersWithProgressIndicator";
			base.Shown += new EventHandler(this.DeleteFilesWithProgressIndicator_Shown);
			base.HelpRequested += new HelpEventHandler(this.DeleteFilesWithProgressIndicator_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
