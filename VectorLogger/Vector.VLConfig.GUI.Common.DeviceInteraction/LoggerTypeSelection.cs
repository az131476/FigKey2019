using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.DeviceInteraction
{
	public class LoggerTypeSelection : Form
	{
		private LoggerType selectedLoggerType;

		private IContainer components;

		private Label labelSelectLoggerType;

		private ComboBox comboBoxLoggerType;

		private Button buttonOK;

		private Button buttonCancel;

		public LoggerType SelectedLoggerType
		{
			get
			{
				return this.selectedLoggerType;
			}
		}

		public LoggerTypeSelection(LoggerType defaultLoggerType) : this(defaultLoggerType, false)
		{
		}

		public LoggerTypeSelection(LoggerType defaultLoggerType, bool showOnStartup)
		{
			this.InitializeComponent();
			if (showOnStartup)
			{
				this.labelSelectLoggerType.Text = Resources.StartupDefaultLoggerType;
			}
			int selectedIndex = 0;
			this.selectedLoggerType = defaultLoggerType;
			foreach (LoggerType loggerType in Enum.GetValues(typeof(LoggerType)))
			{
				if (loggerType != LoggerType.Unknown)
				{
					if (defaultLoggerType == loggerType)
					{
						selectedIndex = this.comboBoxLoggerType.Items.Add(GUIUtil.MapLoggerType2String(loggerType));
					}
					else
					{
						this.comboBoxLoggerType.Items.Add(GUIUtil.MapLoggerType2String(loggerType));
					}
				}
			}
			this.comboBoxLoggerType.SelectedIndex = selectedIndex;
		}

		private void comboBoxLoggerType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.selectedLoggerType = GUIUtil.MapString2LoggerType(this.comboBoxLoggerType.SelectedItem.ToString());
		}

		private void LoggerTypeSelection_HelpRequested(object sender, HelpEventArgs hlpevent)
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LoggerTypeSelection));
			this.labelSelectLoggerType = new Label();
			this.comboBoxLoggerType = new ComboBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelSelectLoggerType, "labelSelectLoggerType");
			this.labelSelectLoggerType.Name = "labelSelectLoggerType";
			componentResourceManager.ApplyResources(this.comboBoxLoggerType, "comboBoxLoggerType");
			this.comboBoxLoggerType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxLoggerType.FormattingEnabled = true;
			this.comboBoxLoggerType.Name = "comboBoxLoggerType";
			this.comboBoxLoggerType.SelectedIndexChanged += new EventHandler(this.comboBoxLoggerType_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.DialogResult = DialogResult.OK;
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.comboBoxLoggerType);
			base.Controls.Add(this.labelSelectLoggerType);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "LoggerTypeSelection";
			base.ShowIcon = false;
			base.HelpRequested += new HelpEventHandler(this.LoggerTypeSelection_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
