using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	public class IgnitionCondition : Form
	{
		private IgnitionEvent ignitionEvent;

		private IContainer components;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonHelp;

		private RadioButton radioButtonOn;

		private RadioButton radioButtonOff;

		public IgnitionEvent IgnitionEvent
		{
			get
			{
				return this.ignitionEvent;
			}
			set
			{
				this.ignitionEvent = value;
			}
		}

		public IgnitionCondition()
		{
			this.InitializeComponent();
			this.ignitionEvent = new IgnitionEvent();
		}

		public void ResetToDefaults()
		{
			this.ignitionEvent.IsOn.Value = true;
		}

		private void IgnitionCondition_Shown(object sender, EventArgs e)
		{
			this.radioButtonOn.Checked = this.ignitionEvent.IsOn.Value;
			this.radioButtonOff.Checked = !this.ignitionEvent.IsOn.Value;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.ignitionEvent.IsOn.Value = this.radioButtonOn.Checked;
			base.DialogResult = DialogResult.OK;
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void IgnitionCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(IgnitionCondition));
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.radioButtonOn = new RadioButton();
			this.radioButtonOff = new RadioButton();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.radioButtonOn, "radioButtonOn");
			this.radioButtonOn.Name = "radioButtonOn";
			this.radioButtonOn.TabStop = true;
			this.radioButtonOn.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButtonOff, "radioButtonOff");
			this.radioButtonOff.Name = "radioButtonOff";
			this.radioButtonOff.TabStop = true;
			this.radioButtonOff.UseVisualStyleBackColor = true;
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.radioButtonOff);
			base.Controls.Add(this.radioButtonOn);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "IgnitionCondition";
			base.Shown += new EventHandler(this.IgnitionCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.IgnitionCondition_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
