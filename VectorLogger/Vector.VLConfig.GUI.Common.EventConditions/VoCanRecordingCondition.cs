using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	public class VoCanRecordingCondition : Form
	{
		private bool isInitControls;

		private IModelValidator modelValidator;

		private IContainer components;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonHelp;

		private CheckBox checkBoxIsBeepActive;

		private ErrorProvider errorProviderFormat;

		private GroupBox groupBoxDuration;

		private RadioButton radioButtonDurationFixed;

		private RadioButton radioButtonDurationDynamic;

		private Label labelSec;

		private TextBox textBoxDuration;

		private CheckBox checkBoxVoCanLed;

		private GroupBox groupBoxType;

		private RadioButton radioButtonDeviceCASM2T3L;

		private RadioButton radioButtonDeviceVoCAN;

		private ErrorProvider errorProviderGlobalModel;

		public VoCanRecordingEvent VoCanRecordingEvent
		{
			get;
			set;
		}

		public VoCanRecordingCondition(IModelValidator modelVal)
		{
			this.InitializeComponent();
			this.modelValidator = modelVal;
			this.isInitControls = false;
			this.VoCanRecordingEvent = new VoCanRecordingEvent();
			this.ResetToDefaults();
			this.ApplyValuesToControls();
		}

		public void ResetToDefaults()
		{
			this.VoCanRecordingEvent.IsUsingCASM2T3L.Value = false;
			this.VoCanRecordingEvent.IsFixedRecordDuration.Value = false;
			this.VoCanRecordingEvent.Duration_s.Value = Constants.DefaultVoCanRecordingDuration_s;
			this.VoCanRecordingEvent.IsBeepOnEndOn.Value = true;
			this.VoCanRecordingEvent.IsRecordingLEDActive.Value = true;
			this.ResetErrorProvider();
		}

		private void VoCanRecordingCondition_Shown(object sender, EventArgs e)
		{
			this.ApplyValuesToControls();
		}

		private void radioButtonDevice_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxIsBeepActive_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxVoCanLed_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void radioButtonDuration_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.textBoxDuration.Enabled = this.radioButtonDurationFixed.Checked;
			this.ValidateInput();
		}

		private void textBoxDuration_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
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
			base.DialogResult = DialogResult.OK;
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void VoCanRecordingCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private bool ValidateInput()
		{
			bool flag = true;
			this.ResetErrorProvider();
			uint value = this.VoCanRecordingEvent.Duration_s.Value;
			if (this.radioButtonDurationFixed.Checked)
			{
				if (!uint.TryParse(this.textBoxDuration.Text, out value))
				{
					this.errorProviderFormat.SetError(this.textBoxDuration, Resources.ErrorNumberExpected);
					flag = false;
				}
				else if (value > Constants.MaximumVoCanRecordingDuration_s || value < Constants.MinimumVoCanRecordingDuration_s)
				{
					this.errorProviderFormat.SetError(this.textBoxDuration, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinimumVoCanRecordingDuration_s, Constants.MaximumVoCanRecordingDuration_s));
					flag = false;
				}
			}
			else
			{
				this.textBoxDuration.Text = value.ToString();
			}
			if (flag)
			{
				this.VoCanRecordingEvent.IsUsingCASM2T3L.Value = this.radioButtonDeviceCASM2T3L.Checked;
				this.VoCanRecordingEvent.IsFixedRecordDuration.Value = this.radioButtonDurationFixed.Checked;
				this.VoCanRecordingEvent.Duration_s.Value = value;
				this.VoCanRecordingEvent.IsBeepOnEndOn.Value = this.checkBoxIsBeepActive.Checked;
				this.VoCanRecordingEvent.IsRecordingLEDActive.Value = this.checkBoxVoCanLed.Checked;
			}
			uint arg_153_0 = this.modelValidator.LoggerSpecifics.IO.NumberOfLEDsTotal;
			uint arg_169_0 = this.modelValidator.LoggerSpecifics.IO.NumberOfLEDsOnBoard;
			uint ledNumber = 8u - (Constants.StartIndexForRemoteLeds - this.modelValidator.LoggerSpecifics.IO.NumberOfLEDsOnBoard);
			bool flag2 = !this.modelValidator.IsLedDisabled(ledNumber);
			if (this.VoCanRecordingEvent.IsRecordingLEDActive.Value && flag2)
			{
				this.errorProviderGlobalModel.SetError(this.checkBoxVoCanLed, Resources.ErrorLed8ConfiguredMoreThanOnce);
			}
			return flag;
		}

		private void ResetErrorProvider()
		{
			this.errorProviderFormat.SetError(this.textBoxDuration, "");
			this.errorProviderGlobalModel.SetError(this.checkBoxVoCanLed, "");
		}

		private void ApplyValuesToControls()
		{
			this.isInitControls = true;
			this.radioButtonDeviceCASM2T3L.Checked = this.VoCanRecordingEvent.IsUsingCASM2T3L.Value;
			this.radioButtonDeviceVoCAN.Checked = !this.VoCanRecordingEvent.IsUsingCASM2T3L.Value;
			this.radioButtonDurationFixed.Checked = this.VoCanRecordingEvent.IsFixedRecordDuration.Value;
			this.radioButtonDurationDynamic.Checked = !this.VoCanRecordingEvent.IsFixedRecordDuration.Value;
			this.textBoxDuration.Text = this.VoCanRecordingEvent.Duration_s.Value.ToString();
			this.textBoxDuration.Enabled = this.radioButtonDurationFixed.Checked;
			this.checkBoxIsBeepActive.Checked = this.VoCanRecordingEvent.IsBeepOnEndOn.Value;
			this.checkBoxVoCanLed.Checked = this.VoCanRecordingEvent.IsRecordingLEDActive.Value;
			this.isInitControls = false;
			this.ValidateInput();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(VoCanRecordingCondition));
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.checkBoxIsBeepActive = new CheckBox();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.textBoxDuration = new TextBox();
			this.labelSec = new Label();
			this.groupBoxDuration = new GroupBox();
			this.radioButtonDurationFixed = new RadioButton();
			this.radioButtonDurationDynamic = new RadioButton();
			this.checkBoxVoCanLed = new CheckBox();
			this.groupBoxType = new GroupBox();
			this.radioButtonDeviceCASM2T3L = new RadioButton();
			this.radioButtonDeviceVoCAN = new RadioButton();
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			this.groupBoxDuration.SuspendLayout();
			this.groupBoxType.SuspendLayout();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
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
			componentResourceManager.ApplyResources(this.checkBoxIsBeepActive, "checkBoxIsBeepActive");
			this.checkBoxIsBeepActive.Name = "checkBoxIsBeepActive";
			this.checkBoxIsBeepActive.UseVisualStyleBackColor = true;
			this.checkBoxIsBeepActive.CheckedChanged += new EventHandler(this.checkBoxIsBeepActive_CheckedChanged);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderFormat.SetIconAlignment(this.textBoxDuration, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDuration.IconAlignment"));
			componentResourceManager.ApplyResources(this.textBoxDuration, "textBoxDuration");
			this.textBoxDuration.Name = "textBoxDuration";
			this.textBoxDuration.Validating += new CancelEventHandler(this.textBoxDuration_Validating);
			componentResourceManager.ApplyResources(this.labelSec, "labelSec");
			this.labelSec.Name = "labelSec";
			this.groupBoxDuration.Controls.Add(this.radioButtonDurationFixed);
			this.groupBoxDuration.Controls.Add(this.radioButtonDurationDynamic);
			this.groupBoxDuration.Controls.Add(this.labelSec);
			this.groupBoxDuration.Controls.Add(this.textBoxDuration);
			componentResourceManager.ApplyResources(this.groupBoxDuration, "groupBoxDuration");
			this.groupBoxDuration.Name = "groupBoxDuration";
			this.groupBoxDuration.TabStop = false;
			componentResourceManager.ApplyResources(this.radioButtonDurationFixed, "radioButtonDurationFixed");
			this.radioButtonDurationFixed.Name = "radioButtonDurationFixed";
			this.radioButtonDurationFixed.TabStop = true;
			this.radioButtonDurationFixed.UseVisualStyleBackColor = true;
			this.radioButtonDurationFixed.CheckedChanged += new EventHandler(this.radioButtonDuration_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonDurationDynamic, "radioButtonDurationDynamic");
			this.radioButtonDurationDynamic.Name = "radioButtonDurationDynamic";
			this.radioButtonDurationDynamic.TabStop = true;
			this.radioButtonDurationDynamic.UseVisualStyleBackColor = true;
			this.radioButtonDurationDynamic.CheckedChanged += new EventHandler(this.radioButtonDuration_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxVoCanLed, "checkBoxVoCanLed");
			this.checkBoxVoCanLed.Name = "checkBoxVoCanLed";
			this.checkBoxVoCanLed.UseVisualStyleBackColor = true;
			this.checkBoxVoCanLed.CheckedChanged += new EventHandler(this.checkBoxVoCanLed_CheckedChanged);
			this.groupBoxType.Controls.Add(this.radioButtonDeviceCASM2T3L);
			this.groupBoxType.Controls.Add(this.radioButtonDeviceVoCAN);
			componentResourceManager.ApplyResources(this.groupBoxType, "groupBoxType");
			this.groupBoxType.Name = "groupBoxType";
			this.groupBoxType.TabStop = false;
			componentResourceManager.ApplyResources(this.radioButtonDeviceCASM2T3L, "radioButtonDeviceCASM2T3L");
			this.radioButtonDeviceCASM2T3L.Name = "radioButtonDeviceCASM2T3L";
			this.radioButtonDeviceCASM2T3L.TabStop = true;
			this.radioButtonDeviceCASM2T3L.UseVisualStyleBackColor = true;
			this.radioButtonDeviceCASM2T3L.CheckedChanged += new EventHandler(this.radioButtonDevice_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonDeviceVoCAN, "radioButtonDeviceVoCAN");
			this.radioButtonDeviceVoCAN.Name = "radioButtonDeviceVoCAN";
			this.radioButtonDeviceVoCAN.TabStop = true;
			this.radioButtonDeviceVoCAN.UseVisualStyleBackColor = true;
			this.radioButtonDeviceVoCAN.CheckedChanged += new EventHandler(this.radioButtonDevice_CheckedChanged);
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.groupBoxType);
			base.Controls.Add(this.checkBoxVoCanLed);
			base.Controls.Add(this.groupBoxDuration);
			base.Controls.Add(this.checkBoxIsBeepActive);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "VoCanRecordingCondition";
			base.Shown += new EventHandler(this.VoCanRecordingCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.VoCanRecordingCondition_HelpRequested);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			this.groupBoxDuration.ResumeLayout(false);
			this.groupBoxDuration.PerformLayout();
			this.groupBoxType.ResumeLayout(false);
			this.groupBoxType.PerformLayout();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
