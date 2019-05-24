using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.WLANSettingsPage
{
	public class DataSelection : Form
	{
		private IModelValidator modelValidator;

		private bool isInitControls;

		private DataTransferTrigger dataTransferTrigger;

		private bool isDownloadRingbufferEnabled;

		private uint memoriesToDownload;

		private bool isDownloadClassifEnabled;

		private bool isDownloadDriveRecEnabled;

		private IContainer components;

		private CheckBox checkBoxLogDataOnMemory;

		private CheckBox checkBoxClassifications;

		private CheckBox checkBoxDriveRecorder;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonHelp;

		private ComboBox comboBoxMemories;

		private ErrorProvider errorProviderLocalModel;

		public DataTransferTrigger DataTransferTrigger
		{
			get
			{
				return this.dataTransferTrigger;
			}
			set
			{
				this.dataTransferTrigger = value;
				this.isDownloadRingbufferEnabled = this.dataTransferTrigger.IsDownloadRingbufferEnabled.Value;
				this.memoriesToDownload = this.dataTransferTrigger.MemoriesToDownload.Value;
				this.isDownloadClassifEnabled = this.dataTransferTrigger.IsDownloadClassifEnabled.Value;
				this.isDownloadDriveRecEnabled = this.dataTransferTrigger.IsDownloadDriveRecEnabled.Value;
			}
		}

		public DataSelection(IModelValidator modelVal)
		{
			this.isInitControls = false;
			this.modelValidator = modelVal;
			this.InitializeComponent();
			this.InitDownloadRingbuffersCombobox();
		}

		private void InitDownloadRingbuffersCombobox()
		{
			this.isInitControls = true;
			this.comboBoxMemories.Items.Clear();
			if (this.modelValidator.LoggerSpecifics.Type == LoggerType.GL3000 || this.modelValidator.LoggerSpecifics.Type == LoggerType.GL4000)
			{
				this.comboBoxMemories.Items.Add(Resources.AllMemories);
			}
			for (uint num = 1u; num <= this.modelValidator.LoggerSpecifics.DataStorage.NumberOfMemories; num += 1u)
			{
				this.comboBoxMemories.Items.Add(GUIUtil.MapValueToMemoriesString(num));
			}
			if (this.comboBoxMemories.Items.Count > 0)
			{
				this.comboBoxMemories.SelectedIndex = 0;
			}
			this.isInitControls = false;
		}

		private void DataSelection_Shown(object sender, EventArgs e)
		{
			this.isInitControls = true;
			this.checkBoxLogDataOnMemory.Checked = this.isDownloadRingbufferEnabled;
			this.comboBoxMemories.SelectedItem = GUIUtil.MapValueToMemoriesString(this.memoriesToDownload);
			this.comboBoxMemories.Enabled = this.checkBoxLogDataOnMemory.Checked;
			this.checkBoxClassifications.Checked = this.isDownloadClassifEnabled;
			this.checkBoxDriveRecorder.Checked = this.isDownloadDriveRecEnabled;
			this.isInitControls = false;
			this.ValidateInput();
		}

		private void checkBox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.comboBoxMemories.Enabled = this.checkBoxLogDataOnMemory.Checked;
			this.ValidateInput();
		}

		private void comboBoxMemories_SelectedIndexChanged(object sender, EventArgs e)
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
				return;
			}
			this.dataTransferTrigger.IsDownloadRingbufferEnabled.Value = this.isDownloadRingbufferEnabled;
			this.dataTransferTrigger.MemoriesToDownload.Value = this.memoriesToDownload;
			this.dataTransferTrigger.IsDownloadClassifEnabled.Value = this.isDownloadClassifEnabled;
			this.dataTransferTrigger.IsDownloadDriveRecEnabled.Value = this.isDownloadDriveRecEnabled;
			base.DialogResult = DialogResult.OK;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void DataSelection_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void ResetErrorProvider()
		{
			this.errorProviderLocalModel.SetError(this.checkBoxLogDataOnMemory, "");
			this.errorProviderLocalModel.SetError(this.checkBoxClassifications, "");
			this.errorProviderLocalModel.SetError(this.checkBoxDriveRecorder, "");
		}

		private bool ValidateInput()
		{
			this.ResetErrorProvider();
			this.isDownloadRingbufferEnabled = this.checkBoxLogDataOnMemory.Checked;
			this.memoriesToDownload = GUIUtil.MapMemoriesStringToValue(this.comboBoxMemories.SelectedItem.ToString());
			this.isDownloadClassifEnabled = this.checkBoxClassifications.Checked;
			this.isDownloadDriveRecEnabled = this.checkBoxDriveRecorder.Checked;
			if (!this.isDownloadRingbufferEnabled && !this.isDownloadClassifEnabled && !this.isDownloadDriveRecEnabled)
			{
				this.buttonOK.Enabled = false;
				this.errorProviderLocalModel.SetError(this.checkBoxLogDataOnMemory, Resources.ErrorAtLeastOneOptionActive);
				this.errorProviderLocalModel.SetError(this.checkBoxClassifications, Resources.ErrorAtLeastOneOptionActive);
				this.errorProviderLocalModel.SetError(this.checkBoxDriveRecorder, Resources.ErrorAtLeastOneOptionActive);
				return false;
			}
			this.buttonOK.Enabled = true;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DataSelection));
			this.checkBoxLogDataOnMemory = new CheckBox();
			this.checkBoxClassifications = new CheckBox();
			this.checkBoxDriveRecorder = new CheckBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.comboBoxMemories = new ComboBox();
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.checkBoxLogDataOnMemory, "checkBoxLogDataOnMemory");
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogDataOnMemory, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogDataOnMemory.IconAlignment"));
			this.checkBoxLogDataOnMemory.Name = "checkBoxLogDataOnMemory";
			this.checkBoxLogDataOnMemory.UseVisualStyleBackColor = true;
			this.checkBoxLogDataOnMemory.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxClassifications, "checkBoxClassifications");
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxClassifications, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxClassifications.IconAlignment"));
			this.checkBoxClassifications.Name = "checkBoxClassifications";
			this.checkBoxClassifications.UseVisualStyleBackColor = true;
			this.checkBoxClassifications.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxDriveRecorder, "checkBoxDriveRecorder");
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxDriveRecorder, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxDriveRecorder.IconAlignment"));
			this.checkBoxDriveRecorder.Name = "checkBoxDriveRecorder";
			this.checkBoxDriveRecorder.UseVisualStyleBackColor = true;
			this.checkBoxDriveRecorder.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			this.comboBoxMemories.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxMemories.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxMemories, "comboBoxMemories");
			this.comboBoxMemories.Name = "comboBoxMemories";
			this.comboBoxMemories.SelectedIndexChanged += new EventHandler(this.comboBoxMemories_SelectedIndexChanged);
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.comboBoxMemories);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.checkBoxDriveRecorder);
			base.Controls.Add(this.checkBoxClassifications);
			base.Controls.Add(this.checkBoxLogDataOnMemory);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "DataSelection";
			base.Shown += new EventHandler(this.DataSelection_Shown);
			base.HelpRequested += new HelpEventHandler(this.DataSelection_HelpRequested);
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
