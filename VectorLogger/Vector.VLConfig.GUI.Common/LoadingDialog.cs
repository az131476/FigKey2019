using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.GeneralUtil.GUI;

namespace Vector.VLConfig.GUI.Common
{
	public class LoadingDialog : Form
	{
		public delegate Result WriteToMemoryCardDelegate(string cardDrive, out string codMD5Hash, out string errorText, out string compilerErrFilePath);

		public delegate Result CopyCODFileToMemoryCardDelegate(string codFilePath, string cardDrive, out string errorText);

		public delegate bool WriteGL1000ConfigurationDelegate(out string codMD5Hash);

		public delegate bool CopyGL1000ConfigurationDelegate(string codFilePath);

		private string cardDrive;

		private string codMD5Hash;

		private string errorText;

		private string compilerErrFilePath;

		private string codFilePath;

		private Result result = Result.Error;

		private bool resultBool;

		private IContainer components;

		private ProgressBar progressBar1;

		private BackgroundWorker backgroundWorker;

		public LoadingDialog.WriteToMemoryCardDelegate WriteToMemoryCardTask
		{
			get;
			set;
		}

		public LoadingDialog.CopyCODFileToMemoryCardDelegate CopyCODFileToMemoryCardTask
		{
			get;
			set;
		}

		public LoadingDialog.WriteGL1000ConfigurationDelegate WriteGL1000ConfigurationTask
		{
			get;
			set;
		}

		public LoadingDialog.CopyGL1000ConfigurationDelegate CopyGL1000ConfigurationTask
		{
			get;
			set;
		}

		public LoadingDialog()
		{
			this.InitializeComponent();
		}

		public LoadingDialog(string title) : this()
		{
			this.Text = title;
		}

		private void LoadingDialog_Shown(object sender, EventArgs e)
		{
			InformMessageBox.Owner = this;
			this.backgroundWorker.RunWorkerAsync();
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			base.Close();
			InformMessageBox.Owner = null;
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			ProgramUtils.ApplyProgramUICulture();
			if (this.WriteToMemoryCardTask != null)
			{
				this.result = this.WriteToMemoryCardTask(this.cardDrive, out this.codMD5Hash, out this.errorText, out this.compilerErrFilePath);
			}
			if (this.CopyCODFileToMemoryCardTask != null)
			{
				this.result = this.CopyCODFileToMemoryCardTask(this.codFilePath, this.cardDrive, out this.errorText);
			}
			if (this.WriteGL1000ConfigurationTask != null)
			{
				this.resultBool = this.WriteGL1000ConfigurationTask(out this.codMD5Hash);
			}
			if (this.CopyGL1000ConfigurationTask != null)
			{
				this.resultBool = this.CopyGL1000ConfigurationTask(this.codFilePath);
			}
		}

		public Result ExecuteWriteToMemoryCard(string cardDrive, out string codMD5Hash, out string errorText, out string compilerErrFilePath)
		{
			this.cardDrive = cardDrive;
			this.codMD5Hash = string.Empty;
			this.errorText = string.Empty;
			this.compilerErrFilePath = string.Empty;
			base.ShowDialog();
			codMD5Hash = this.codMD5Hash;
			errorText = this.errorText;
			compilerErrFilePath = this.compilerErrFilePath;
			return this.result;
		}

		public Result ExecuteCopyCODFileToMemoryCard(string codFilePath, string cardDrive, out string errorText)
		{
			this.codFilePath = codFilePath;
			this.cardDrive = cardDrive;
			this.errorText = string.Empty;
			this.compilerErrFilePath = string.Empty;
			base.ShowDialog();
			errorText = this.errorText;
			return this.result;
		}

		public bool ExecuteWriteGL1000Configuration(out string codMD5Hash)
		{
			this.codMD5Hash = string.Empty;
			base.ShowDialog();
			codMD5Hash = this.codMD5Hash;
			return this.resultBool;
		}

		public bool ExecuteCopyGL1000Configuration(string codFilePath)
		{
			this.codFilePath = codFilePath;
			base.ShowDialog();
			return this.resultBool;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LoadingDialog));
			this.progressBar1 = new ProgressBar();
			this.backgroundWorker = new BackgroundWorker();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.progressBar1, "progressBar1");
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Step = 1;
			this.progressBar1.Style = ProgressBarStyle.Marquee;
			this.progressBar1.UseWaitCursor = true;
			this.backgroundWorker.DoWork += new DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ControlBox = false;
			base.Controls.Add(this.progressBar1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "LoadingDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.UseWaitCursor = true;
			base.Shown += new EventHandler(this.LoadingDialog_Shown);
			base.ResumeLayout(false);
		}
	}
}
