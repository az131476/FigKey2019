using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.DeviceInteraction
{
	public class SerialCommandProgress : Form
	{
		private int timeProcessed;

		private Thread workerThread;

		private string portName;

		private string successText;

		private IContainer components;

		private Label labelStatus;

		private System.Windows.Forms.Timer timerProgress;

		private Button buttonCancel;

		private ProgressBar progressBar;

		private PictureBox pictureBoxIcon;

		private Button buttonOK;

		private Label labelError;

		public ThreadStart SerialCommandProcessingMethod
		{
			get;
			set;
		}

		public int ProcessingTimeout
		{
			get;
			set;
		}

		public SerialCommandProgress(string titleText, string port, string reportSuccessText)
		{
			this.InitializeComponent();
			this.Text = titleText;
			this.portName = port;
			this.labelError.Text = Resources.ErrorNoResponseFromDevice;
			this.successText = reportSuccessText;
		}

		private void SerialCommandProgress_Shown(object sender, EventArgs e)
		{
			this.pictureBoxIcon.Image = SystemIcons.Error.ToBitmap();
			this.pictureBoxIcon.Visible = false;
			this.buttonOK.Visible = false;
			this.labelError.Visible = false;
			if (this.SerialCommandProcessingMethod == null || string.IsNullOrEmpty(this.portName))
			{
				base.DialogResult = DialogResult.Cancel;
				return;
			}
			this.timeProcessed = 0;
			this.progressBar.Minimum = 0;
			this.progressBar.Maximum = this.ProcessingTimeout;
			this.progressBar.Value = 0;
			SerialPortServices.PortName = this.portName;
			this.workerThread = new Thread(this.SerialCommandProcessingMethod);
			this.workerThread.Start();
			this.timerProgress.Start();
		}

		private void timerProgress_Tick(object sender, EventArgs e)
		{
			this.timeProcessed += this.timerProgress.Interval;
			this.progressBar.Value = this.timeProcessed;
			if (this.timeProcessed < this.ProcessingTimeout)
			{
				int arg_4D_0 = (this.ProcessingTimeout - this.timeProcessed) / 1000;
				if (SerialPortServices.IsAborted)
				{
					switch (SerialPortServices.LastErrorCode)
					{
					case SerialPortServices.ErrorCode.CannotSetIPAddress:
						this.labelError.Text = Resources.ErrorFailedToSetCustomIPAddress;
						break;
					case SerialPortServices.ErrorCode.CannotSetSubnetMask:
						this.labelError.Text = Resources.ErrorFailedToSetCustomSubnetMask;
						break;
					default:
						this.labelError.Text = Resources.ErrorNoResponseFromDevice;
						break;
					}
					this.ForceTermination();
					this.DisplayErrorState();
					return;
				}
				if (SerialPortServices.IsFinished)
				{
					this.timerProgress.Stop();
					this.workerThread.Join();
					if (SerialPortServices.IsDefaultIPAddress)
					{
						uint num;
						uint num2;
						uint num3;
						uint num4;
						SerialPortServices.GetIpAddressBytes(SerialPortServices.IPAddress, out num, out num2, out num3, out num4);
						this.labelError.Text = string.Format(Resources.EthernetCannotSetCustomIPAddress, new object[]
						{
							num,
							num2,
							num3,
							num4
						});
						this.DisplayErrorState();
						return;
					}
					if (!string.IsNullOrEmpty(this.successText))
					{
						this.DisplaySuccessState();
						return;
					}
					base.DialogResult = DialogResult.OK;
					return;
				}
			}
			else
			{
				SerialPortServices.RequestAbort();
				this.ForceTermination();
				this.DisplayErrorState();
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			SerialPortServices.RequestAbort();
			this.ForceTermination();
			base.Close();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
		}

		private void SerialCommandProgress_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void ForceTermination()
		{
			this.timerProgress.Stop();
			this.workerThread.Join();
		}

		private void DisplayErrorState()
		{
			this.progressBar.Visible = false;
			this.buttonCancel.Visible = false;
			this.labelStatus.Visible = false;
			this.labelError.Visible = true;
			this.pictureBoxIcon.Visible = true;
			this.buttonOK.Visible = true;
			this.buttonOK.Focus();
		}

		private void DisplaySuccessState()
		{
			this.progressBar.Visible = false;
			this.buttonCancel.Visible = false;
			this.labelStatus.Visible = false;
			this.labelError.Text = this.successText;
			this.labelError.Visible = true;
			this.pictureBoxIcon.Image = SystemIcons.Information.ToBitmap();
			this.pictureBoxIcon.Visible = true;
			this.buttonOK.Visible = true;
			this.buttonOK.Focus();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SerialCommandProgress));
			this.labelStatus = new Label();
			this.timerProgress = new System.Windows.Forms.Timer(this.components);
			this.buttonCancel = new Button();
			this.progressBar = new ProgressBar();
			this.pictureBoxIcon = new PictureBox();
			this.buttonOK = new Button();
			this.labelError = new Label();
			((ISupportInitialize)this.pictureBoxIcon).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelStatus, "labelStatus");
			this.labelStatus.Name = "labelStatus";
			this.timerProgress.Interval = 1000;
			this.timerProgress.Tick += new EventHandler(this.timerProgress_Tick);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this.progressBar, "progressBar");
			this.progressBar.Name = "progressBar";
			componentResourceManager.ApplyResources(this.pictureBoxIcon, "pictureBoxIcon");
			this.pictureBoxIcon.Name = "pictureBoxIcon";
			this.pictureBoxIcon.TabStop = false;
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this.labelError, "labelError");
			this.labelError.Name = "labelError";
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ControlBox = false;
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.pictureBoxIcon);
			base.Controls.Add(this.progressBar);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.labelError);
			base.Controls.Add(this.labelStatus);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SerialCommandProgress";
			base.Shown += new EventHandler(this.SerialCommandProgress_Shown);
			base.HelpRequested += new HelpEventHandler(this.SerialCommandProgress_HelpRequested);
			((ISupportInitialize)this.pictureBoxIcon).EndInit();
			base.ResumeLayout(false);
		}
	}
}
