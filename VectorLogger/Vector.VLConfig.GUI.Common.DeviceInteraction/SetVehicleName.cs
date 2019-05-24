using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.DeviceInteraction
{
	public class SetVehicleName : Form
	{
		private ILoggerDevice loggerDevice;

		private readonly string allowedSpecialChars = " _-!$()";

		private readonly int maxNameLength = 31;

		private IContainer components;

		private Label labelCurrentName;

		private Label labelNewName;

		private Label labelCurrentNameValue;

		private TextBox textBoxNewName;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonHelp;

		private ErrorProvider errorProviderFormat;

		private PictureBox pictureBoxInfo;

		private Label labelWarningPowerSupply;

		private Label labelWarningLogDataFiles;

		private PictureBox pictureBoxWarning;

		public SetVehicleName(ILoggerDevice device)
		{
			this.InitializeComponent();
			this.pictureBoxWarning.Image = SystemIcons.Warning.ToBitmap();
			this.pictureBoxInfo.Image = SystemIcons.Information.ToBitmap();
			this.loggerDevice = device;
		}

		private void SetVehicleName_Shown(object sender, EventArgs e)
		{
			this.labelCurrentNameValue.Text = this.loggerDevice.VehicleName;
			this.textBoxNewName.Text = this.loggerDevice.VehicleName;
			bool visible = false;
			bool visible2 = false;
			if (this.loggerDevice.LoggerType == LoggerType.GL1000)
			{
				visible2 = true;
				uint num = 0u;
				string s = this.loggerDevice.SerialNumber.Split(new char[]
				{
					'-'
				}).Last<string>();
				if (uint.TryParse(s, out num) && (num <= Constants.MaxSerialNumberGL1000_DeviantHwBehaviour || (num >= Constants.SerialNumberOffsetGL1010 && num <= Constants.MaxSerialNumberGL1010_DeviantHwBehaviour)) && Environment.OSVersion.Platform == PlatformID.Win32NT && Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor == 1)
				{
					visible = true;
				}
			}
			this.pictureBoxWarning.Visible = visible2;
			this.labelWarningLogDataFiles.Visible = visible2;
			this.pictureBoxInfo.Visible = visible;
			this.labelWarningPowerSupply.Visible = visible;
		}

		private void textBoxNewName_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateInput();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (!this.ValidateInput())
			{
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				base.DialogResult = DialogResult.None;
				return;
			}
			if (this.loggerDevice.SetVehicleName(this.textBoxNewName.Text))
			{
				InformMessageBox.Info(Resources.VehicleNameSet);
				base.DialogResult = DialogResult.OK;
			}
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void SetVehicleName_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private bool ValidateInput()
		{
			string text = this.textBoxNewName.Text;
			if (string.IsNullOrEmpty(text))
			{
				this.errorProviderFormat.SetError(this.textBoxNewName, Resources.ErrorNameMustNotBeEmpty);
				return false;
			}
			if (text.Length > this.maxNameLength)
			{
				this.errorProviderFormat.SetError(this.textBoxNewName, string.Format(Resources.ErrorInputExceedsMaxLen, this.maxNameLength));
				return false;
			}
			for (int i = 0; i < text.Length; i++)
			{
				if (text[i] > '\u007f' || (!char.IsLetterOrDigit(text[i]) && this.allowedSpecialChars.IndexOf(text[i]) < 0))
				{
					this.errorProviderFormat.SetError(this.textBoxNewName, string.Format(Resources.ErrorInvalidCharsFoundGen, GUIUtil.GetBlankSeparatedCharList(this.allowedSpecialChars)));
					return false;
				}
			}
			this.errorProviderFormat.SetError(this.textBoxNewName, "");
			return true;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SetVehicleName));
			this.labelCurrentName = new Label();
			this.labelNewName = new Label();
			this.labelCurrentNameValue = new Label();
			this.textBoxNewName = new TextBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.pictureBoxInfo = new PictureBox();
			this.labelWarningPowerSupply = new Label();
			this.pictureBoxWarning = new PictureBox();
			this.labelWarningLogDataFiles = new Label();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.pictureBoxInfo).BeginInit();
			((ISupportInitialize)this.pictureBoxWarning).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelCurrentName, "labelCurrentName");
			this.labelCurrentName.Name = "labelCurrentName";
			componentResourceManager.ApplyResources(this.labelNewName, "labelNewName");
			this.labelNewName.Name = "labelNewName";
			componentResourceManager.ApplyResources(this.labelCurrentNameValue, "labelCurrentNameValue");
			this.labelCurrentNameValue.Name = "labelCurrentNameValue";
			this.errorProviderFormat.SetIconAlignment(this.textBoxNewName, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxNewName.IconAlignment"));
			componentResourceManager.ApplyResources(this.textBoxNewName, "textBoxNewName");
			this.textBoxNewName.Name = "textBoxNewName";
			this.textBoxNewName.Validating += new CancelEventHandler(this.textBoxNewName_Validating);
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.pictureBoxInfo, "pictureBoxInfo");
			this.pictureBoxInfo.Name = "pictureBoxInfo";
			this.pictureBoxInfo.TabStop = false;
			componentResourceManager.ApplyResources(this.labelWarningPowerSupply, "labelWarningPowerSupply");
			this.labelWarningPowerSupply.Name = "labelWarningPowerSupply";
			componentResourceManager.ApplyResources(this.pictureBoxWarning, "pictureBoxWarning");
			this.pictureBoxWarning.Name = "pictureBoxWarning";
			this.pictureBoxWarning.TabStop = false;
			componentResourceManager.ApplyResources(this.labelWarningLogDataFiles, "labelWarningLogDataFiles");
			this.labelWarningLogDataFiles.Name = "labelWarningLogDataFiles";
			base.AcceptButton = this.buttonCancel;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.labelWarningLogDataFiles);
			base.Controls.Add(this.pictureBoxWarning);
			base.Controls.Add(this.labelWarningPowerSupply);
			base.Controls.Add(this.pictureBoxInfo);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.textBoxNewName);
			base.Controls.Add(this.labelCurrentNameValue);
			base.Controls.Add(this.labelNewName);
			base.Controls.Add(this.labelCurrentName);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "SetVehicleName";
			base.Shown += new EventHandler(this.SetVehicleName_Shown);
			base.HelpRequested += new HelpEventHandler(this.SetVehicleName_HelpRequested);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.pictureBoxInfo).EndInit();
			((ISupportInitialize)this.pictureBoxWarning).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
