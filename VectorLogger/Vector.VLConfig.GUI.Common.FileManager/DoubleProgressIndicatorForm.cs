using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil.GUI;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class DoubleProgressIndicatorForm : Form
	{
		public delegate void ButtonClickedHandler();

		private SingleProgressBarIndicator masterIndicator;

		private SingleProgressBarIndicator slaveIndicator;

		private IContainer components;

		private TextBox textBoxMasterStatus;

		private ProgressBar progressBarMaster;

		private TextBox textBoxSlaveStatus;

		private ProgressBar progressBarSlave;

		private Button buttonCancel;

		public event DoubleProgressIndicatorForm.ButtonClickedHandler CancelButtonClicked;

		public SingleProgressBarIndicator MasterIndicator
		{
			get
			{
				return this.masterIndicator;
			}
		}

		public SingleProgressBarIndicator SlaveIndicator
		{
			get
			{
				return this.slaveIndicator;
			}
		}

		public DoubleProgressIndicatorForm()
		{
			this.InitializeComponent();
			this.masterIndicator = new SingleProgressBarIndicator(this, ref this.textBoxMasterStatus, ref this.progressBarMaster, true);
			this.slaveIndicator = new SingleProgressBarIndicator(this, ref this.textBoxSlaveStatus, ref this.progressBarSlave, false);
			TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.Normal);
		}

		private void Raise_CancelButtonClicked()
		{
			if (this.CancelButtonClicked != null)
			{
				this.CancelButtonClicked();
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.Raise_CancelButtonClicked();
		}

		private void DoubleProgressIndicatorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DoubleProgressIndicatorForm));
			this.textBoxMasterStatus = new TextBox();
			this.progressBarMaster = new ProgressBar();
			this.textBoxSlaveStatus = new TextBox();
			this.progressBarSlave = new ProgressBar();
			this.buttonCancel = new Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.textBoxMasterStatus, "textBoxMasterStatus");
			this.textBoxMasterStatus.Name = "textBoxMasterStatus";
			this.textBoxMasterStatus.ReadOnly = true;
			componentResourceManager.ApplyResources(this.progressBarMaster, "progressBarMaster");
			this.progressBarMaster.Name = "progressBarMaster";
			componentResourceManager.ApplyResources(this.textBoxSlaveStatus, "textBoxSlaveStatus");
			this.textBoxSlaveStatus.Name = "textBoxSlaveStatus";
			this.textBoxSlaveStatus.ReadOnly = true;
			componentResourceManager.ApplyResources(this.progressBarSlave, "progressBarSlave");
			this.progressBarSlave.Name = "progressBarSlave";
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ControlBox = false;
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.progressBarSlave);
			base.Controls.Add(this.textBoxSlaveStatus);
			base.Controls.Add(this.progressBarMaster);
			base.Controls.Add(this.textBoxMasterStatus);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DoubleProgressIndicatorForm";
			base.ShowInTaskbar = false;
			base.FormClosing += new FormClosingEventHandler(this.DoubleProgressIndicatorForm_FormClosing);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
