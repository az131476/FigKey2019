using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.HardwareSettingsPage
{
	public class SelectEventType : Form
	{
		private ILoggerSpecifics loggerSpecifics;

		private IContainer components;

		private Label label1;

		private ComboBox comboBoxEventType;

		private Button buttonCancel;

		private Button buttonOK;

		public string SelectedEventType
		{
			get;
			set;
		}

		public SelectEventType(ILoggerSpecifics loggerSpecs)
		{
			this.InitializeComponent();
			this.loggerSpecifics = loggerSpecs;
			this.FillEvenTypeComboBox();
		}

		private void FillEvenTypeComboBox()
		{
			this.comboBoxEventType.Items.Clear();
			this.comboBoxEventType.Items.Add(Resources.StopCycDiagCommNever);
			this.comboBoxEventType.Items.Add(Resources.StopCycDiagCommWhileSymCANSignal);
			if (this.loggerSpecifics.LIN.NumberOfChannels > 0u)
			{
				this.comboBoxEventType.Items.Add(Resources.StopCycDiagCommWhileSymLINSignal);
			}
			if (this.loggerSpecifics.Flexray.NumberOfChannels > 0u)
			{
				this.comboBoxEventType.Items.Add(Resources.StopCycDiagCommWhileSymFRSignal);
			}
			if (this.loggerSpecifics.Type != LoggerType.GL1020FTE)
			{
				this.comboBoxEventType.Items.Add(Resources.StopCycDiagCommWhileCcpXcpSignal);
			}
			this.comboBoxEventType.Items.Add(Resources.StopCycDiagCommWhileCANData);
			if (this.loggerSpecifics.LIN.NumberOfChannels > 0u)
			{
				this.comboBoxEventType.Items.Add(Resources.StopCycDiagCommWhileLINData);
			}
			this.comboBoxEventType.Items.Add(Resources.StopCycDiagCommWhileCANMsgTimeout);
			if (this.loggerSpecifics.LIN.NumberOfChannels > 0u)
			{
				this.comboBoxEventType.Items.Add(Resources.StopCycDiagCommWhileLINMsgTimeout);
			}
			if (this.loggerSpecifics.IO.NumberOfDigitalInputs > 0u)
			{
				this.comboBoxEventType.Items.Add(Resources.StopCycDiagCommWhileDigIn);
				this.comboBoxEventType.Items.Add(Resources.StopCycDiagCommWhileIgnition);
			}
			this.comboBoxEventType.SelectedIndex = 0;
		}

		private void SelectAllowDiagCommEventType_Shown(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.SelectedEventType) && this.comboBoxEventType.Items.Contains(this.SelectedEventType))
			{
				this.comboBoxEventType.SelectedItem = this.SelectedEventType;
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.SelectedEventType = this.comboBoxEventType.SelectedItem.ToString();
			base.DialogResult = DialogResult.OK;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.SelectedEventType = "";
			base.DialogResult = DialogResult.Cancel;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SelectEventType));
			this.label1 = new Label();
			this.comboBoxEventType = new ComboBox();
			this.buttonCancel = new Button();
			this.buttonOK = new Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.comboBoxEventType, "comboBoxEventType");
			this.comboBoxEventType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxEventType.FormattingEnabled = true;
			this.comboBoxEventType.Name = "comboBoxEventType";
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
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
			base.Controls.Add(this.comboBoxEventType);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SelectEventType";
			base.Shown += new EventHandler(this.SelectAllowDiagCommEventType_Shown);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
