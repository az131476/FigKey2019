using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	public class OnStartCondition : Form
	{
		private uint delay;

		private IContainer components;

		private Label labelDelay;

		private TextBox textBoxDelay;

		private Button buttonOK;

		private Button buttonCancel;

		private ErrorProvider errorProvider;

		private Button buttonHelp;

		public uint Delay
		{
			get
			{
				return this.delay;
			}
			set
			{
				this.delay = value;
			}
		}

		public OnStartCondition()
		{
			this.InitializeComponent();
			this.delay = Constants.DefaultOnStartDelay;
			this.ResetErrorProvider();
		}

		public void ResetToDefaults()
		{
			this.delay = Constants.DefaultOnStartDelay;
			this.ResetErrorProvider();
		}

		private void OnStartCondition_Shown(object sender, EventArgs e)
		{
			double num = Convert.ToDouble(this.delay) / 1000.0;
			this.textBoxDelay.Text = num.ToString("F1", CultureInfo.InvariantCulture);
		}

		private void textBoxDelay_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateInput();
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void OnStartCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private bool ValidateInput()
		{
			this.ResetErrorProvider();
			double num;
			if (!GUIUtil.DisplayStringToFloatNumber(this.textBoxDelay.Text, out num))
			{
				this.errorProvider.SetError(this.textBoxDelay, Resources.ErrorNumberExpected);
				return false;
			}
			num = Math.Round(num, 1);
			this.textBoxDelay.Text = num.ToString("F1", CultureInfo.InvariantCulture);
			if (num < Constants.MinimumOnStartDelay || num > Constants.MaximumOnStartDelay)
			{
				this.errorProvider.SetError(this.textBoxDelay, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinimumOnStartDelay, Constants.MaximumOnStartDelay));
				return false;
			}
			this.delay = Convert.ToUInt32(num * 1000.0);
			return true;
		}

		private void ResetErrorProvider()
		{
			this.errorProvider.SetError(this.textBoxDelay, "");
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (this.ValidateInput())
			{
				base.DialogResult = DialogResult.OK;
				return;
			}
			InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(OnStartCondition));
			this.labelDelay = new Label();
			this.textBoxDelay = new TextBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.errorProvider = new ErrorProvider(this.components);
			this.buttonHelp = new Button();
			((ISupportInitialize)this.errorProvider).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelDelay, "labelDelay");
			this.errorProvider.SetError(this.labelDelay, componentResourceManager.GetString("labelDelay.Error"));
			this.errorProvider.SetIconAlignment(this.labelDelay, (ErrorIconAlignment)componentResourceManager.GetObject("labelDelay.IconAlignment"));
			this.errorProvider.SetIconPadding(this.labelDelay, (int)componentResourceManager.GetObject("labelDelay.IconPadding"));
			this.labelDelay.Name = "labelDelay";
			componentResourceManager.ApplyResources(this.textBoxDelay, "textBoxDelay");
			this.errorProvider.SetError(this.textBoxDelay, componentResourceManager.GetString("textBoxDelay.Error"));
			this.errorProvider.SetIconAlignment(this.textBoxDelay, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDelay.IconAlignment"));
			this.errorProvider.SetIconPadding(this.textBoxDelay, (int)componentResourceManager.GetObject("textBoxDelay.IconPadding"));
			this.textBoxDelay.Name = "textBoxDelay";
			this.textBoxDelay.Validating += new CancelEventHandler(this.textBoxDelay_Validating);
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.errorProvider.SetError(this.buttonOK, componentResourceManager.GetString("buttonOK.Error"));
			this.errorProvider.SetIconAlignment(this.buttonOK, (ErrorIconAlignment)componentResourceManager.GetObject("buttonOK.IconAlignment"));
			this.errorProvider.SetIconPadding(this.buttonOK, (int)componentResourceManager.GetObject("buttonOK.IconPadding"));
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.errorProvider.SetError(this.buttonCancel, componentResourceManager.GetString("buttonCancel.Error"));
			this.errorProvider.SetIconAlignment(this.buttonCancel, (ErrorIconAlignment)componentResourceManager.GetObject("buttonCancel.IconAlignment"));
			this.errorProvider.SetIconPadding(this.buttonCancel, (int)componentResourceManager.GetObject("buttonCancel.IconPadding"));
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProvider.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProvider, "errorProvider");
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.errorProvider.SetError(this.buttonHelp, componentResourceManager.GetString("buttonHelp.Error"));
			this.errorProvider.SetIconAlignment(this.buttonHelp, (ErrorIconAlignment)componentResourceManager.GetObject("buttonHelp.IconAlignment"));
			this.errorProvider.SetIconPadding(this.buttonHelp, (int)componentResourceManager.GetObject("buttonHelp.IconPadding"));
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.textBoxDelay);
			base.Controls.Add(this.labelDelay);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "OnStartCondition";
			base.Shown += new EventHandler(this.OnStartCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.OnStartCondition_HelpRequested);
			((ISupportInitialize)this.errorProvider).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
