using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	internal class KeyCondition : Form
	{
		private IModelValidator modelValidator;

		private KeyEvent keyEvent;

		private IContainer components;

		private Label labelKey;

		private ComboBox comboBoxKey;

		private Button buttonHelp;

		private Button buttonCancel;

		private Button buttonOK;

		public KeyEvent KeyEvent
		{
			get
			{
				return this.keyEvent;
			}
			set
			{
				this.keyEvent = value;
			}
		}

		public KeyCondition(IModelValidator modelVal)
		{
			this.InitializeComponent();
			this.keyEvent = new KeyEvent();
			this.modelValidator = modelVal;
			this.InitKeyCombobox();
		}

		public void ResetToDefaults()
		{
			this.InitKeyCombobox();
			this.keyEvent.Number.Value = 1u;
			this.keyEvent.IsOnPanel.Value = false;
			this.ApplyValuesToControls();
		}

		private void InitKeyCombobox()
		{
			this.comboBoxKey.Items.Clear();
			for (uint num = 1u; num <= this.modelValidator.LoggerSpecifics.IO.NumberOfKeys; num += 1u)
			{
				this.comboBoxKey.Items.Add(GUIUtil.MapKeyNumber2String(num, false));
			}
			for (uint num2 = 1u; num2 <= this.modelValidator.LoggerSpecifics.IO.NumberOfPanelKeys; num2 += 1u)
			{
				this.comboBoxKey.Items.Add(GUIUtil.MapKeyNumber2String(num2, true));
			}
			for (uint num3 = 1u + Constants.CasKeyOffset; num3 <= this.modelValidator.LoggerSpecifics.IO.NumberOfCasKeys + Constants.CasKeyOffset; num3 += 1u)
			{
				this.comboBoxKey.Items.Add(GUIUtil.MapKeyNumber2String(num3, false));
			}
		}

		private void KeyCondition_Shown(object sender, EventArgs e)
		{
			this.InitKeyCombobox();
			this.ApplyValuesToControls();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.ValidateInput();
			base.DialogResult = DialogResult.OK;
		}

		private void comboBoxKey_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ValidateInput();
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void KeyCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void ValidateInput()
		{
			bool value = false;
			this.keyEvent.Number.Value = GUIUtil.MapStringToKeyNumber(this.comboBoxKey.SelectedItem.ToString(), out value);
			this.keyEvent.IsOnPanel.Value = value;
		}

		private void ApplyValuesToControls()
		{
			this.comboBoxKey.SelectedItem = GUIUtil.MapKeyNumber2String(this.keyEvent.Number.Value, this.keyEvent.IsOnPanel.Value);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(KeyCondition));
			this.labelKey = new Label();
			this.comboBoxKey = new ComboBox();
			this.buttonHelp = new Button();
			this.buttonCancel = new Button();
			this.buttonOK = new Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelKey, "labelKey");
			this.labelKey.Name = "labelKey";
			componentResourceManager.ApplyResources(this.comboBoxKey, "comboBoxKey");
			this.comboBoxKey.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxKey.FormattingEnabled = true;
			this.comboBoxKey.Name = "comboBoxKey";
			this.comboBoxKey.SelectedIndexChanged += new EventHandler(this.comboBoxKey_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.comboBoxKey);
			base.Controls.Add(this.labelKey);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "KeyCondition";
			base.Shown += new EventHandler(this.KeyCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.KeyCondition_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
