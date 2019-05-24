using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common
{
	public class PasswordDialog : Form
	{
		public enum Context
		{
			Neutral,
			AnalysisPackage,
			PackAndGo
		}

		private int messageHeight;

		private IContainer components;

		private CheckBox checkBoxShowPassword;

		private TextBox textBoxPassword;

		private Button buttonOk;

		private Button buttonCancel;

		private Label labelConfirm;

		private TextBox textBoxConfirmPassword;

		private CheckBox checkBoxRememberPassword;

		private Label labelNote;

		private Panel panelMain;

		private TableLayoutPanel tableLayoutPanel;

		private Panel panelMesage;

		private Label labelMessage;

		public string Password
		{
			get
			{
				return this.textBoxPassword.Text;
			}
		}

		public bool RememberPassword
		{
			get
			{
				return this.checkBoxRememberPassword.Checked && !string.IsNullOrEmpty(this.textBoxPassword.Text);
			}
		}

		public string Message
		{
			get
			{
				return this.labelMessage.Text;
			}
			set
			{
				this.labelMessage.Text = value;
				if (string.IsNullOrEmpty(this.labelMessage.Text) && this.tableLayoutPanel.RowStyles[0].Height > 0f)
				{
					this.tableLayoutPanel.RowStyles[0].Height = 0f;
					base.Height -= this.messageHeight;
					return;
				}
				if (this.tableLayoutPanel.RowStyles[0].Height == 0f)
				{
					base.Height += this.messageHeight;
					this.tableLayoutPanel.RowStyles[0].Height = (float)this.messageHeight;
				}
			}
		}

		private bool ConfirmationRequired
		{
			get;
			set;
		}

		public PasswordDialog(PasswordDialog.Context context = PasswordDialog.Context.Neutral, bool confirmationRequired = true)
		{
			this.InitializeComponent();
			this.messageHeight = Convert.ToInt32(this.tableLayoutPanel.RowStyles[0].Height);
			this.Message = null;
			this.ConfirmationRequired = confirmationRequired;
			if (context == PasswordDialog.Context.AnalysisPackage)
			{
				this.labelNote.Text = Resources.RememberPasswordNoteAnalysisPackage;
			}
			else if (context == PasswordDialog.Context.PackAndGo)
			{
				this.labelNote.Text = Resources.RememberPasswordNotePackAndGo;
			}
			this.UpdateControls();
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			if (this.textBoxPassword.Text.Length == 0)
			{
				InformMessageBox.Error(Resources.ErrorPasswordNotSet);
				this.textBoxPassword.Focus();
				base.DialogResult = DialogResult.None;
				return;
			}
			if (this.ConfirmationRequired && !this.checkBoxShowPassword.Checked && this.textBoxPassword.Text != this.textBoxConfirmPassword.Text)
			{
				InformMessageBox.Error(Resources.ErrorPasswordsDontMatch);
				this.textBoxPassword.Focus();
				base.DialogResult = DialogResult.None;
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.textBoxPassword.Text = "";
			this.checkBoxRememberPassword.Checked = false;
		}

		private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateControls();
		}

		private void UpdateControls()
		{
			this.labelConfirm.Enabled = (!this.checkBoxShowPassword.Checked && this.ConfirmationRequired);
			this.textBoxConfirmPassword.Enabled = (!this.checkBoxShowPassword.Checked && this.ConfirmationRequired);
			this.textBoxPassword.UseSystemPasswordChar = !this.checkBoxShowPassword.Checked;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(PasswordDialog));
			this.checkBoxShowPassword = new CheckBox();
			this.textBoxPassword = new TextBox();
			this.buttonOk = new Button();
			this.buttonCancel = new Button();
			this.labelConfirm = new Label();
			this.textBoxConfirmPassword = new TextBox();
			this.checkBoxRememberPassword = new CheckBox();
			this.labelNote = new Label();
			this.panelMain = new Panel();
			this.tableLayoutPanel = new TableLayoutPanel();
			this.panelMesage = new Panel();
			this.labelMessage = new Label();
			this.panelMain.SuspendLayout();
			this.tableLayoutPanel.SuspendLayout();
			this.panelMesage.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.checkBoxShowPassword, "checkBoxShowPassword");
			this.checkBoxShowPassword.Name = "checkBoxShowPassword";
			this.checkBoxShowPassword.UseVisualStyleBackColor = true;
			this.checkBoxShowPassword.CheckedChanged += new EventHandler(this.checkBoxShowPassword_CheckedChanged);
			componentResourceManager.ApplyResources(this.textBoxPassword, "textBoxPassword");
			this.textBoxPassword.Name = "textBoxPassword";
			this.textBoxPassword.UseSystemPasswordChar = true;
			componentResourceManager.ApplyResources(this.buttonOk, "buttonOk");
			this.buttonOk.DialogResult = DialogResult.OK;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new EventHandler(this.buttonOk_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this.labelConfirm, "labelConfirm");
			this.labelConfirm.Name = "labelConfirm";
			componentResourceManager.ApplyResources(this.textBoxConfirmPassword, "textBoxConfirmPassword");
			this.textBoxConfirmPassword.Name = "textBoxConfirmPassword";
			this.textBoxConfirmPassword.UseSystemPasswordChar = true;
			componentResourceManager.ApplyResources(this.checkBoxRememberPassword, "checkBoxRememberPassword");
			this.checkBoxRememberPassword.Name = "checkBoxRememberPassword";
			this.checkBoxRememberPassword.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.labelNote, "labelNote");
			this.labelNote.Name = "labelNote";
			componentResourceManager.ApplyResources(this.panelMain, "panelMain");
			this.panelMain.Controls.Add(this.textBoxPassword);
			this.panelMain.Controls.Add(this.labelNote);
			this.panelMain.Controls.Add(this.checkBoxShowPassword);
			this.panelMain.Controls.Add(this.checkBoxRememberPassword);
			this.panelMain.Controls.Add(this.textBoxConfirmPassword);
			this.panelMain.Controls.Add(this.labelConfirm);
			this.panelMain.Name = "panelMain";
			componentResourceManager.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
			this.tableLayoutPanel.Controls.Add(this.panelMain, 0, 1);
			this.tableLayoutPanel.Controls.Add(this.panelMesage, 0, 0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.panelMesage.Controls.Add(this.labelMessage);
			componentResourceManager.ApplyResources(this.panelMesage, "panelMesage");
			this.panelMesage.Name = "panelMesage";
			componentResourceManager.ApplyResources(this.labelMessage, "labelMessage");
			this.labelMessage.Name = "labelMessage";
			base.AcceptButton = this.buttonOk;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOk);
			base.Controls.Add(this.tableLayoutPanel);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "PasswordDialog";
			this.panelMain.ResumeLayout(false);
			this.panelMain.PerformLayout();
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			this.panelMesage.ResumeLayout(false);
			this.panelMesage.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
