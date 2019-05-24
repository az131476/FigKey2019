using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common
{
	public class OverwriteFilesConfirmation : Form
	{
		private OverwriteFilesResult result;

		private IContainer components;

		private Button buttonYes;

		private Button buttonYesToAll;

		private Button buttonAbort;

		private Label labelText;

		private Button buttonNo;

		public OverwriteFilesResult Result
		{
			get
			{
				return this.result;
			}
		}

		public OverwriteFilesConfirmation(string fileName)
		{
			this.InitializeComponent();
			this.labelText.Text = string.Format(Resources.OverwriteFilesConfirmation, fileName);
			this.result = OverwriteFilesResult.Yes;
		}

		public static OverwriteFilesResult Show(string fileName)
		{
			OverwriteFilesConfirmation overwriteFilesConfirmation = new OverwriteFilesConfirmation(fileName);
			overwriteFilesConfirmation.ShowDialog();
			return overwriteFilesConfirmation.Result;
		}

		private void buttonYes_Click(object sender, EventArgs e)
		{
			this.result = OverwriteFilesResult.Yes;
			base.DialogResult = DialogResult.OK;
		}

		private void buttonYesToAll_Click(object sender, EventArgs e)
		{
			this.result = OverwriteFilesResult.YesToAll;
			base.DialogResult = DialogResult.OK;
		}

		private void buttonNo_Click(object sender, EventArgs e)
		{
			this.result = OverwriteFilesResult.No;
			base.DialogResult = DialogResult.OK;
		}

		private void buttonAbort_Click(object sender, EventArgs e)
		{
			this.result = OverwriteFilesResult.Abort;
			base.DialogResult = DialogResult.Cancel;
		}

		private void OverwriteFilesConfirmation_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(OverwriteFilesConfirmation));
			this.buttonYes = new Button();
			this.buttonYesToAll = new Button();
			this.buttonAbort = new Button();
			this.labelText = new Label();
			this.buttonNo = new Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.buttonYes, "buttonYes");
			this.buttonYes.Name = "buttonYes";
			this.buttonYes.UseVisualStyleBackColor = true;
			this.buttonYes.Click += new EventHandler(this.buttonYes_Click);
			componentResourceManager.ApplyResources(this.buttonYesToAll, "buttonYesToAll");
			this.buttonYesToAll.Name = "buttonYesToAll";
			this.buttonYesToAll.UseVisualStyleBackColor = true;
			this.buttonYesToAll.Click += new EventHandler(this.buttonYesToAll_Click);
			componentResourceManager.ApplyResources(this.buttonAbort, "buttonAbort");
			this.buttonAbort.DialogResult = DialogResult.Cancel;
			this.buttonAbort.Name = "buttonAbort";
			this.buttonAbort.UseVisualStyleBackColor = true;
			this.buttonAbort.Click += new EventHandler(this.buttonAbort_Click);
			componentResourceManager.ApplyResources(this.labelText, "labelText");
			this.labelText.Name = "labelText";
			componentResourceManager.ApplyResources(this.buttonNo, "buttonNo");
			this.buttonNo.Name = "buttonNo";
			this.buttonNo.UseVisualStyleBackColor = true;
			this.buttonNo.Click += new EventHandler(this.buttonNo_Click);
			base.AcceptButton = this.buttonYes;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonAbort;
			base.ControlBox = false;
			base.Controls.Add(this.buttonNo);
			base.Controls.Add(this.labelText);
			base.Controls.Add(this.buttonAbort);
			base.Controls.Add(this.buttonYesToAll);
			base.Controls.Add(this.buttonYes);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "OverwriteFilesConfirmation";
			base.HelpRequested += new HelpEventHandler(this.OverwriteFilesConfirmation_HelpRequested);
			base.ResumeLayout(false);
		}
	}
}
