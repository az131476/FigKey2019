using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common
{
	public class InputSplitSize : Form
	{
		private uint fileSize;

		private IContainer components;

		private Label labelSize;

		private TextBox textBoxSize;

		private Label labelKB;

		private Button buttonCancel;

		private Button buttonOK;

		private ErrorProvider errorProvider;

		public uint FileSizeInKB
		{
			get
			{
				return this.fileSize;
			}
		}

		public InputSplitSize()
		{
			this.InitializeComponent();
			this.fileSize = 1000u;
		}

		private void textBoxSize_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateInput();
		}

		private bool ValidateInput()
		{
			bool result = true;
			if (!uint.TryParse(this.textBoxSize.Text, out this.fileSize))
			{
				this.errorProvider.SetError(this.textBoxSize, Resources.ErrorIntExpected);
				result = false;
			}
			else
			{
				this.errorProvider.SetError(this.textBoxSize, "");
			}
			return result;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (!this.ValidateInput())
			{
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				return;
			}
			base.DialogResult = DialogResult.OK;
		}

		private void InputSplitSize_Shown(object sender, EventArgs e)
		{
			this.textBoxSize.Text = this.fileSize.ToString();
			this.errorProvider.SetError(this.textBoxSize, "");
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(InputSplitSize));
			this.labelSize = new Label();
			this.textBoxSize = new TextBox();
			this.labelKB = new Label();
			this.buttonCancel = new Button();
			this.buttonOK = new Button();
			this.errorProvider = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProvider).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelSize, "labelSize");
			this.errorProvider.SetError(this.labelSize, componentResourceManager.GetString("labelSize.Error"));
			this.errorProvider.SetIconAlignment(this.labelSize, (ErrorIconAlignment)componentResourceManager.GetObject("labelSize.IconAlignment"));
			this.errorProvider.SetIconPadding(this.labelSize, (int)componentResourceManager.GetObject("labelSize.IconPadding"));
			this.labelSize.Name = "labelSize";
			componentResourceManager.ApplyResources(this.textBoxSize, "textBoxSize");
			this.errorProvider.SetError(this.textBoxSize, componentResourceManager.GetString("textBoxSize.Error"));
			this.errorProvider.SetIconAlignment(this.textBoxSize, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxSize.IconAlignment"));
			this.errorProvider.SetIconPadding(this.textBoxSize, (int)componentResourceManager.GetObject("textBoxSize.IconPadding"));
			this.textBoxSize.Name = "textBoxSize";
			this.textBoxSize.Validating += new CancelEventHandler(this.textBoxSize_Validating);
			componentResourceManager.ApplyResources(this.labelKB, "labelKB");
			this.errorProvider.SetError(this.labelKB, componentResourceManager.GetString("labelKB.Error"));
			this.errorProvider.SetIconAlignment(this.labelKB, (ErrorIconAlignment)componentResourceManager.GetObject("labelKB.IconAlignment"));
			this.errorProvider.SetIconPadding(this.labelKB, (int)componentResourceManager.GetObject("labelKB.IconPadding"));
			this.labelKB.Name = "labelKB";
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.errorProvider.SetError(this.buttonCancel, componentResourceManager.GetString("buttonCancel.Error"));
			this.errorProvider.SetIconAlignment(this.buttonCancel, (ErrorIconAlignment)componentResourceManager.GetObject("buttonCancel.IconAlignment"));
			this.errorProvider.SetIconPadding(this.buttonCancel, (int)componentResourceManager.GetObject("buttonCancel.IconPadding"));
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.errorProvider.SetError(this.buttonOK, componentResourceManager.GetString("buttonOK.Error"));
			this.errorProvider.SetIconAlignment(this.buttonOK, (ErrorIconAlignment)componentResourceManager.GetObject("buttonOK.IconAlignment"));
			this.errorProvider.SetIconPadding(this.buttonOK, (int)componentResourceManager.GetObject("buttonOK.IconPadding"));
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProvider.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProvider, "errorProvider");
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.labelKB);
			base.Controls.Add(this.textBoxSize);
			base.Controls.Add(this.labelSize);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "InputSplitSize";
			base.Shown += new EventHandler(this.InputSplitSize_Shown);
			((ISupportInitialize)this.errorProvider).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
