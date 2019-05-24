using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction.VN16XXlog;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.DeviceInteraction
{
	internal class FirmwareDownloadDialog : Form
	{
		private class BgWorkResult
		{
			public Result Result
			{
				get;
				set;
			}

			public string ErrorText
			{
				get;
				set;
			}
		}

		private const int cEstimatedDownloadTimeSeconds = 40;

		private const int cDownloadTimeoutSeconds = 80;

		private byte[] mFirmwareImage;

		private string mFirmwareImageError;

		private readonly VN16XXlogDevice mDevice;

		private readonly BackgroundWorker mBgWorker = new BackgroundWorker();

		private readonly GlobalOptions mGlobalOptions;

		private IContainer components;

		private Button mButtonClose;

		private Label label1;

		private Label mLabelSelectedFirmwareVersion;

		private TableLayoutPanel tableLayoutPanel1;

		private Label mLabelInstalledFirmwareVersion;

		private Label label3;

		private Label mLabelSelectImage;

		private Button mButtonSelectFirmwareImage;

		private TextBox mTextBoxFirmwareImageFile;

		private Button mButtonDownload;

		private ProgressBar mProgressBarDownload;

		private Label mLabelDownloadHint1;

		private Label mLabelDownloadHint2;

		private Timer mTimerProgress;

		private Timer mTimerTimeout;

		public Version FirmwareImageVersion
		{
			get;
			private set;
		}

		public Result UpdateResult
		{
			get;
			private set;
		}

		public string UpdateErrorText
		{
			get;
			private set;
		}

		public FirmwareDownloadDialog(VN16XXlogDevice device, GlobalOptions globalOptions)
		{
			this.mDevice = device;
			this.mGlobalOptions = globalOptions;
			this.UpdateResult = Result.OK;
			this.UpdateErrorText = string.Empty;
			this.InitializeComponent();
			this.mBgWorker.DoWork += new DoWorkEventHandler(this.BgWorker_DoWork);
			this.mBgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(this.BgWorker_RunWorkerCompleted);
			this.mTimerProgress.Interval = 100;
			this.mTimerProgress.Tick += new EventHandler(this.TimerProgress_Tick);
			this.mProgressBarDownload.Minimum = 0;
			this.mProgressBarDownload.Maximum = 40000 / this.mTimerProgress.Interval;
			this.mTimerTimeout.Interval = 80000;
			this.mTimerTimeout.Tick += new EventHandler(this.TimerTimeout_Tick);
			this.UpdateControls();
			this.mTextBoxFirmwareImageFile.Text = Path.Combine(FileSystemServices.GetApplicationPath(), Vocabulary.FileNameVN1630logFirmwareImage);
		}

		private void UpdateControls()
		{
			this.mLabelInstalledFirmwareVersion.Text = string.Format(Resources.ErrorUnableToAccessDevice, this.mDevice.GetHwTypeName());
			this.mLabelSelectedFirmwareVersion.Text = Resources.ErrorNoImageSelected;
			if (this.mDevice != null && this.mDevice.IsOnline)
			{
				this.mLabelInstalledFirmwareVersion.Text = this.mDevice.FirmwareVersion;
			}
			if (this.FirmwareImageVersion != null)
			{
				this.mLabelSelectedFirmwareVersion.Text = this.FirmwareImageVersion.ToString();
			}
			else if (string.IsNullOrEmpty(this.mFirmwareImageError))
			{
				this.mLabelSelectedFirmwareVersion.Text = Resources.ErrorNoImageSelected;
			}
			else
			{
				this.mLabelSelectedFirmwareVersion.Text = this.mFirmwareImageError;
			}
			this.mButtonDownload.Enabled = (this.mDevice != null && this.mDevice.IsOnline && this.mFirmwareImage != null);
		}

		[Conditional("DEBUG")]
		private void EnableDebugControls()
		{
			this.mLabelSelectImage.Visible = true;
			this.mTextBoxFirmwareImageFile.Visible = true;
			this.mButtonSelectFirmwareImage.Visible = true;
			base.Height += 100;
		}

		private void TextBoxFirmwareImageFile_TextChanged(object sender, EventArgs e)
		{
			this.mFirmwareImage = null;
			this.FirmwareImageVersion = null;
			this.mFirmwareImageError = null;
			Version firmwareImageVersion;
			VN16XXlogUtils.ReadFirmwareImage(this.mTextBoxFirmwareImageFile.Text, out this.mFirmwareImage, out firmwareImageVersion, out this.mFirmwareImageError);
			this.FirmwareImageVersion = firmwareImageVersion;
			this.UpdateControls();
		}

		private void ButtonSelectFirmwareImage_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "VN1630log firmware image (*.vLogFw)|*.vLogFw";
				openFileDialog.InitialDirectory = this.mGlobalOptions.RecentFirmwareFolder;
				openFileDialog.RestoreDirectory = true;
				if (DialogResult.OK == openFileDialog.ShowDialog())
				{
					this.mTextBoxFirmwareImageFile.Text = openFileDialog.FileName;
					this.mGlobalOptions.RecentFirmwareFolder = Path.GetDirectoryName(openFileDialog.FileName);
				}
			}
		}

		private void TimerProgress_Tick(object sender, EventArgs e)
		{
			if (this.mProgressBarDownload.Value < this.mProgressBarDownload.Maximum - 1)
			{
				this.mProgressBarDownload.Value++;
				return;
			}
			this.mTimerProgress.Stop();
		}

		private void TimerTimeout_Tick(object sender, EventArgs e)
		{
			this.mBgWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(this.BgWorker_RunWorkerCompleted);
			this.UpdateResult = Result.Error;
			this.UpdateErrorText = string.Format(Resources.ErrorFirmwareDownloadDidNotFinishWithinTimeout, 80);
			this.CleanupAndCloseDialog();
		}

		private void ButtonDownload_Click(object sender, EventArgs e)
		{
			this.mTextBoxFirmwareImageFile.Enabled = false;
			this.mButtonSelectFirmwareImage.Enabled = false;
			this.mButtonDownload.Visible = false;
			this.mLabelDownloadHint1.Visible = true;
			this.mProgressBarDownload.Visible = true;
			this.mLabelDownloadHint2.Visible = true;
			this.mButtonClose.Visible = false;
			base.UseWaitCursor = true;
			this.UpdateResult = Result.OK;
			this.UpdateErrorText = string.Empty;
			this.mTimerTimeout.Start();
			this.mProgressBarDownload.Value = 0;
			this.mTimerProgress.Start();
			this.mBgWorker.RunWorkerAsync();
		}

		private void BgWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			string errorText;
			Result result = VN16XXlogUtils.DownloadFirmwareImage(this.mFirmwareImage, this.mDevice, out errorText);
			e.Result = new FirmwareDownloadDialog.BgWorkResult
			{
				Result = result,
				ErrorText = errorText
			};
		}

		private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			FirmwareDownloadDialog.BgWorkResult bgWorkResult = e.Result as FirmwareDownloadDialog.BgWorkResult;
			Trace.Assert(bgWorkResult != null);
			this.UpdateResult = bgWorkResult.Result;
			this.UpdateErrorText = bgWorkResult.ErrorText;
			this.CleanupAndCloseDialog();
		}

		private void CleanupAndCloseDialog()
		{
			base.UseWaitCursor = false;
			this.mTextBoxFirmwareImageFile.Enabled = true;
			this.mButtonSelectFirmwareImage.Enabled = true;
			this.mButtonDownload.Visible = true;
			this.mLabelDownloadHint1.Visible = false;
			this.mProgressBarDownload.Visible = false;
			this.mLabelDownloadHint2.Visible = false;
			this.mButtonClose.Visible = true;
			this.mTimerTimeout.Stop();
			this.mTimerProgress.Stop();
			base.DialogResult = DialogResult.OK;
			base.Close();
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
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FirmwareDownloadDialog));
			this.mButtonClose = new Button();
			this.label1 = new Label();
			this.mLabelSelectedFirmwareVersion = new Label();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.mButtonDownload = new Button();
			this.mLabelInstalledFirmwareVersion = new Label();
			this.label3 = new Label();
			this.mProgressBarDownload = new ProgressBar();
			this.mLabelDownloadHint1 = new Label();
			this.mLabelDownloadHint2 = new Label();
			this.mLabelSelectImage = new Label();
			this.mButtonSelectFirmwareImage = new Button();
			this.mTextBoxFirmwareImageFile = new TextBox();
			this.mTimerProgress = new Timer(this.components);
			this.mTimerTimeout = new Timer(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mButtonClose, "mButtonClose");
			this.mButtonClose.DialogResult = DialogResult.Cancel;
			this.mButtonClose.Name = "mButtonClose";
			this.mButtonClose.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.mLabelSelectedFirmwareVersion, "mLabelSelectedFirmwareVersion");
			this.mLabelSelectedFirmwareVersion.Name = "mLabelSelectedFirmwareVersion";
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.mLabelInstalledFirmwareVersion, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.mTextBoxFirmwareImageFile, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.mButtonSelectFirmwareImage, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.mLabelSelectImage, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.mLabelSelectedFirmwareVersion, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.mProgressBarDownload, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.mLabelDownloadHint1, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.mLabelDownloadHint2, 0, 4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.mButtonDownload, "mButtonDownload");
			this.mButtonDownload.Name = "mButtonDownload";
			this.mButtonDownload.UseVisualStyleBackColor = true;
			this.mButtonDownload.Click += new EventHandler(this.ButtonDownload_Click);
			componentResourceManager.ApplyResources(this.mLabelInstalledFirmwareVersion, "mLabelInstalledFirmwareVersion");
			this.mLabelInstalledFirmwareVersion.Name = "mLabelInstalledFirmwareVersion";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.tableLayoutPanel1.SetColumnSpan(this.mProgressBarDownload, 2);
			componentResourceManager.ApplyResources(this.mProgressBarDownload, "mProgressBarDownload");
			this.mProgressBarDownload.MarqueeAnimationSpeed = 35;
			this.mProgressBarDownload.Name = "mProgressBarDownload";
			this.mProgressBarDownload.Style = ProgressBarStyle.Continuous;
			componentResourceManager.ApplyResources(this.mLabelDownloadHint1, "mLabelDownloadHint1");
			this.tableLayoutPanel1.SetColumnSpan(this.mLabelDownloadHint1, 2);
			this.mLabelDownloadHint1.Name = "mLabelDownloadHint1";
			componentResourceManager.ApplyResources(this.mLabelDownloadHint2, "mLabelDownloadHint2");
			this.tableLayoutPanel1.SetColumnSpan(this.mLabelDownloadHint2, 2);
			this.mLabelDownloadHint2.Name = "mLabelDownloadHint2";
			componentResourceManager.ApplyResources(this.mLabelSelectImage, "mLabelSelectImage");
			this.mLabelSelectImage.Name = "mLabelSelectImage";
			componentResourceManager.ApplyResources(this.mButtonSelectFirmwareImage, "mButtonSelectFirmwareImage");
			this.mButtonSelectFirmwareImage.Name = "mButtonSelectFirmwareImage";
			this.mButtonSelectFirmwareImage.UseVisualStyleBackColor = true;
			this.mButtonSelectFirmwareImage.Click += new EventHandler(this.ButtonSelectFirmwareImage_Click);
			this.tableLayoutPanel1.SetColumnSpan(this.mTextBoxFirmwareImageFile, 2);
			componentResourceManager.ApplyResources(this.mTextBoxFirmwareImageFile, "mTextBoxFirmwareImageFile");
			this.mTextBoxFirmwareImageFile.Name = "mTextBoxFirmwareImageFile";
			this.mTextBoxFirmwareImageFile.TextChanged += new EventHandler(this.TextBoxFirmwareImageFile_TextChanged);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.mButtonClose;
			base.ControlBox = false;
			base.Controls.Add(this.mButtonDownload);
			base.Controls.Add(this.mButtonClose);
			base.Controls.Add(this.tableLayoutPanel1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "FirmwareDownloadDialog";
			base.ShowInTaskbar = false;
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
