using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.DeviceInteraction
{
	public class SetRealtimeClock : Form
	{
		private DateTime currentDateTime;

		private readonly int TimeoutCycle = 1000;

		private string ClockMode_PC;

		private string ClockMode_Logger;

		private string ClockMode_Manual;

		private string clockMode;

		private ILoggerSpecifics loggerSpecifics;

		private ILoggerDevice device;

		private int hintGroupBoxDesignHeight;

		private int windowDesignHeight;

		private IContainer components;

		private DateTimePicker timePicker;

		private Button buttonOk;

		private Button buttonSet;

		private ComboBox comboBoxPorts;

		private Label labelPorts;

		private System.Windows.Forms.Timer timer;

		private GroupBox groupBoxConnection;

		private ComboBox comboBoxClockMode;

		private GroupBox groupBoxClock;

		private DateTimePicker datePicker;

		private Label labelDate;

		private TextBox textBoxTimeDisplay;

		private TextBox textBoxDateDisplay;

		private GroupBox groupBoxHint;

		private Label labelHint;

		private Button buttonHelp;

		private TableLayoutPanel tableLayoutPanel1;

		public ILoggerSpecifics LoggerSpecifics
		{
			set
			{
				this.loggerSpecifics = value;
			}
		}

		public ILoggerDevice LoggerDevice
		{
			set
			{
				this.device = value;
			}
		}

		public string SelectedCOMPort
		{
			get;
			set;
		}

		public SetRealtimeClock(ILoggerSpecifics specifics, ILoggerDevice currentDevice)
		{
			this.InitializeComponent();
			this.loggerSpecifics = specifics;
			this.device = currentDevice;
			this.timer.Interval = this.TimeoutCycle;
			this.currentDateTime = DateTime.Now;
			this.ClockMode_PC = Resources.ClockModePC;
			this.ClockMode_Manual = Resources.ClockModeManual;
			this.ClockMode_Logger = Resources.ClockModeLogger;
			this.hintGroupBoxDesignHeight = this.groupBoxHint.Height;
			this.windowDesignHeight = base.Height;
		}

		private void comboBoxClockMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selectedItem = this.clockMode;
			this.clockMode = this.comboBoxClockMode.SelectedItem.ToString();
			if (this.clockMode == this.ClockMode_PC)
			{
				this.SetPCTimeMode();
				return;
			}
			if (this.clockMode == this.ClockMode_Manual)
			{
				this.timer.Stop();
				this.EnableInput(true);
				this.buttonSet.Enabled = true;
				return;
			}
			if (this.clockMode == this.ClockMode_Logger)
			{
				if (this.ReadLoggerClock())
				{
					this.buttonSet.Enabled = false;
					this.EnableInput(false);
					this.timer.Start();
					this.DisplayCurrentDateTime();
					return;
				}
				this.comboBoxClockMode.SelectedItem = selectedItem;
			}
		}

		private void buttonSet_Click(object sender, EventArgs e)
		{
			if (!this.IsUSBConnectionSelected())
			{
				SerialPortServices.RealtimeClock = new DateTime(this.datePicker.Value.Year, this.datePicker.Value.Month, this.datePicker.Value.Day, this.timePicker.Value.Hour, this.timePicker.Value.Minute, this.timePicker.Value.Second);
				this.comboBoxPorts.SelectedItem.ToString();
				new SerialCommandProgress(Resources.SetRealtimeClockTitle, this.comboBoxPorts.SelectedItem.ToString(), "")
				{
					SerialCommandProcessingMethod = new ThreadStart(SerialPortServices.SetRealtimeClock),
					ProcessingTimeout = SerialPortServices.CommunicationTimeout
				}.ShowDialog();
				if (SerialPortServices.IsFinished)
				{
					this.currentDateTime = SerialPortServices.RealtimeClock;
					this.comboBoxClockMode.SelectedIndexChanged -= new EventHandler(this.comboBoxClockMode_SelectedIndexChanged);
					this.comboBoxClockMode.SelectedItem = this.ClockMode_Logger;
					this.comboBoxClockMode.SelectedIndexChanged += new EventHandler(this.comboBoxClockMode_SelectedIndexChanged);
					this.buttonSet.Enabled = false;
					this.EnableInput(false);
					this.DisplayCurrentDateTime();
					this.timer.Start();
					return;
				}
			}
			else
			{
				if (this.device == null)
				{
					InformMessageBox.Error(Resources.ErrorNoLoggerDevConnected);
					return;
				}
				if (this.device.SetRealTimeClock())
				{
					if (this.loggerSpecifics.DeviceAccess.RequiresDisconnectAfterSettingRealtimeClock)
					{
						InformMessageBox.Info(Resources.RealTimeClockSetWithDisconnect);
					}
					else
					{
						InformMessageBox.Info(Resources.RealTimeClockSet);
					}
					base.DialogResult = DialogResult.OK;
				}
			}
		}

		private void buttonOk_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
		}

		private void SetRealtimeClock_Shown(object sender, EventArgs e)
		{
			this.InitComboboxConnection();
			this.InitComboboxClockMode();
			this.InitHintText();
			this.comboBoxClockMode.SelectedIndexChanged -= new EventHandler(this.comboBoxClockMode_SelectedIndexChanged);
			this.comboBoxClockMode.SelectedItem = this.ClockMode_PC;
			this.comboBoxClockMode.SelectedIndexChanged += new EventHandler(this.comboBoxClockMode_SelectedIndexChanged);
			this.EnableInput(true);
			this.clockMode = this.ClockMode_PC;
			this.SetPCTimeMode();
			if (this.IsUSBConnectionSelected())
			{
				this.buttonSet.Focus();
			}
		}

		private void timer_Tick(object sender, EventArgs e)
		{
			if (this.comboBoxClockMode.SelectedItem.ToString() != this.ClockMode_Manual)
			{
				this.currentDateTime = this.currentDateTime.AddMilliseconds((double)this.TimeoutCycle);
				this.DisplayCurrentDateTime();
			}
		}

		private void DisplayCurrentDateTime()
		{
			this.timePicker.Value = this.currentDateTime;
			this.textBoxTimeDisplay.Text = this.timePicker.Text;
			this.datePicker.Value = this.currentDateTime;
			this.textBoxDateDisplay.Text = this.datePicker.Text;
		}

		private void EnableInput(bool isEnabled)
		{
			this.timePicker.Visible = isEnabled;
			this.timePicker.Enabled = isEnabled;
			this.textBoxTimeDisplay.Visible = !isEnabled;
			this.datePicker.Visible = isEnabled;
			this.datePicker.Enabled = isEnabled;
			this.textBoxDateDisplay.Visible = !isEnabled;
		}

		private void textBoxDateTimeDisplay_DoubleClick(object sender, EventArgs e)
		{
			if (this.loggerSpecifics.DeviceAccess.HasRealtimeClockAccessBySerialPort && this.clockMode != this.ClockMode_Manual)
			{
				this.comboBoxClockMode.SelectedItem = this.ClockMode_Manual;
			}
		}

		private void textBoxDateTimeDisplay_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (this.loggerSpecifics.DeviceAccess.HasRealtimeClockAccessBySerialPort && e.KeyChar != '\t')
			{
				this.comboBoxClockMode.SelectedItem = this.ClockMode_Manual;
			}
		}

		private void picker_Validated(object sender, EventArgs e)
		{
			DateTimePicker dateTimePicker = sender as DateTimePicker;
			if (dateTimePicker != null)
			{
				this.currentDateTime = dateTimePicker.Value;
			}
		}

		private void comboBoxPorts_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.InitComboboxClockMode();
			this.InitHintText();
			if (this.IsUSBConnectionSelected())
			{
				this.SetPCTimeMode();
				return;
			}
			this.SelectedCOMPort = this.comboBoxPorts.SelectedItem.ToString();
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void SetRealtimeClock_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private bool IsUSBConnectionSelected()
		{
			return string.Compare(this.comboBoxPorts.SelectedItem.ToString(), Resources.USB, true) == 0;
		}

		private void SetPCTimeMode()
		{
			this.buttonSet.Enabled = true;
			this.EnableInput(false);
			this.timer.Stop();
			this.currentDateTime = DateTime.Now;
			this.DisplayCurrentDateTime();
			this.timer.Start();
		}

		private bool ReadLoggerClock()
		{
			this.comboBoxPorts.SelectedItem.ToString();
			new SerialCommandProgress(Resources.ReadRealtimeClockTitle, this.comboBoxPorts.SelectedItem.ToString(), "")
			{
				SerialCommandProcessingMethod = new ThreadStart(SerialPortServices.ReadRealtimeClock),
				ProcessingTimeout = SerialPortServices.CommunicationTimeout
			}.ShowDialog();
			if (SerialPortServices.IsFinished)
			{
				this.currentDateTime = SerialPortServices.RealtimeClock;
				return true;
			}
			return false;
		}

		private void InitComboboxClockMode()
		{
			this.comboBoxClockMode.SelectedIndexChanged -= new EventHandler(this.comboBoxClockMode_SelectedIndexChanged);
			this.comboBoxClockMode.Items.Clear();
			this.comboBoxClockMode.Items.Add(this.ClockMode_PC);
			if (this.loggerSpecifics.DeviceAccess.HasRealtimeClockAccessBySerialPort && !this.IsUSBConnectionSelected())
			{
				this.comboBoxClockMode.Items.Add(this.ClockMode_Manual);
				this.comboBoxClockMode.Items.Add(this.ClockMode_Logger);
			}
			this.comboBoxClockMode.SelectedIndex = 0;
			this.comboBoxClockMode.SelectedIndexChanged += new EventHandler(this.comboBoxClockMode_SelectedIndexChanged);
		}

		private void InitComboboxConnection()
		{
			Cursor current = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			this.comboBoxPorts.Items.Clear();
			this.comboBoxPorts.Items.Add(Resources.USB);
			if (this.loggerSpecifics.DeviceAccess.HasRealtimeClockAccessBySerialPort)
			{
				string[] cOMPortNames = SerialPortServices.GetCOMPortNames();
				for (int i = 0; i < cOMPortNames.Length; i++)
				{
					string item = cOMPortNames[i];
					this.comboBoxPorts.Items.Add(item);
				}
			}
			if (this.comboBoxPorts.Items.Count > 0)
			{
				if (!string.IsNullOrEmpty(this.SelectedCOMPort) && this.comboBoxPorts.Items.Contains(this.SelectedCOMPort))
				{
					this.comboBoxPorts.SelectedItem = this.SelectedCOMPort;
				}
				else
				{
					this.comboBoxPorts.SelectedIndex = 0;
				}
			}
			Cursor.Current = current;
			Application.DoEvents();
		}

		private void InitHintText()
		{
			this.groupBoxHint.Visible = true;
			base.Height = this.windowDesignHeight;
			this.groupBoxHint.Height = this.hintGroupBoxDesignHeight;
			if (!this.IsUSBConnectionSelected())
			{
				this.labelHint.Text = Resources.RealtimeClockHintCOMConn;
				return;
			}
			if (this.loggerSpecifics.DeviceAccess.RequiresDisconnectAfterSettingRealtimeClock)
			{
				this.labelHint.Text = Resources.RealtimeClockHintUSBConn;
				return;
			}
			this.labelHint.Text = string.Empty;
			this.groupBoxHint.Height = 0;
			this.groupBoxHint.Visible = false;
			base.Height -= this.hintGroupBoxDesignHeight;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SetRealtimeClock));
			this.timePicker = new DateTimePicker();
			this.buttonOk = new Button();
			this.buttonSet = new Button();
			this.comboBoxPorts = new ComboBox();
			this.labelPorts = new Label();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.groupBoxConnection = new GroupBox();
			this.comboBoxClockMode = new ComboBox();
			this.groupBoxClock = new GroupBox();
			this.labelDate = new Label();
			this.datePicker = new DateTimePicker();
			this.textBoxTimeDisplay = new TextBox();
			this.textBoxDateDisplay = new TextBox();
			this.groupBoxHint = new GroupBox();
			this.labelHint = new Label();
			this.buttonHelp = new Button();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.groupBoxConnection.SuspendLayout();
			this.groupBoxClock.SuspendLayout();
			this.groupBoxHint.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.timePicker, "timePicker");
			this.timePicker.Format = DateTimePickerFormat.Time;
			this.timePicker.Name = "timePicker";
			this.timePicker.ShowUpDown = true;
			this.timePicker.Validated += new EventHandler(this.picker_Validated);
			componentResourceManager.ApplyResources(this.buttonOk, "buttonOk");
			this.buttonOk.DialogResult = DialogResult.Cancel;
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new EventHandler(this.buttonOk_Click);
			componentResourceManager.ApplyResources(this.buttonSet, "buttonSet");
			this.buttonSet.Name = "buttonSet";
			this.buttonSet.UseVisualStyleBackColor = true;
			this.buttonSet.Click += new EventHandler(this.buttonSet_Click);
			this.comboBoxPorts.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxPorts.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxPorts, "comboBoxPorts");
			this.comboBoxPorts.Name = "comboBoxPorts";
			this.comboBoxPorts.SelectedIndexChanged += new EventHandler(this.comboBoxPorts_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelPorts, "labelPorts");
			this.labelPorts.Name = "labelPorts";
			this.timer.Interval = 1000;
			this.timer.Tick += new EventHandler(this.timer_Tick);
			this.tableLayoutPanel1.SetColumnSpan(this.groupBoxConnection, 2);
			this.groupBoxConnection.Controls.Add(this.labelPorts);
			this.groupBoxConnection.Controls.Add(this.comboBoxPorts);
			componentResourceManager.ApplyResources(this.groupBoxConnection, "groupBoxConnection");
			this.groupBoxConnection.Name = "groupBoxConnection";
			this.groupBoxConnection.TabStop = false;
			this.comboBoxClockMode.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxClockMode.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxClockMode, "comboBoxClockMode");
			this.comboBoxClockMode.Name = "comboBoxClockMode";
			this.comboBoxClockMode.SelectedIndexChanged += new EventHandler(this.comboBoxClockMode_SelectedIndexChanged);
			this.tableLayoutPanel1.SetColumnSpan(this.groupBoxClock, 2);
			this.groupBoxClock.Controls.Add(this.labelDate);
			this.groupBoxClock.Controls.Add(this.datePicker);
			this.groupBoxClock.Controls.Add(this.comboBoxClockMode);
			this.groupBoxClock.Controls.Add(this.buttonSet);
			this.groupBoxClock.Controls.Add(this.timePicker);
			this.groupBoxClock.Controls.Add(this.textBoxTimeDisplay);
			this.groupBoxClock.Controls.Add(this.textBoxDateDisplay);
			componentResourceManager.ApplyResources(this.groupBoxClock, "groupBoxClock");
			this.groupBoxClock.Name = "groupBoxClock";
			this.groupBoxClock.TabStop = false;
			componentResourceManager.ApplyResources(this.labelDate, "labelDate");
			this.labelDate.Name = "labelDate";
			this.datePicker.Format = DateTimePickerFormat.Short;
			componentResourceManager.ApplyResources(this.datePicker, "datePicker");
			this.datePicker.Name = "datePicker";
			componentResourceManager.ApplyResources(this.textBoxTimeDisplay, "textBoxTimeDisplay");
			this.textBoxTimeDisplay.Name = "textBoxTimeDisplay";
			this.textBoxTimeDisplay.ReadOnly = true;
			this.textBoxTimeDisplay.DoubleClick += new EventHandler(this.textBoxDateTimeDisplay_DoubleClick);
			this.textBoxTimeDisplay.KeyPress += new KeyPressEventHandler(this.textBoxDateTimeDisplay_KeyPress);
			componentResourceManager.ApplyResources(this.textBoxDateDisplay, "textBoxDateDisplay");
			this.textBoxDateDisplay.Name = "textBoxDateDisplay";
			this.textBoxDateDisplay.ReadOnly = true;
			this.textBoxDateDisplay.DoubleClick += new EventHandler(this.textBoxDateTimeDisplay_DoubleClick);
			this.textBoxDateDisplay.KeyPress += new KeyPressEventHandler(this.textBoxDateTimeDisplay_KeyPress);
			componentResourceManager.ApplyResources(this.groupBoxHint, "groupBoxHint");
			this.tableLayoutPanel1.SetColumnSpan(this.groupBoxHint, 2);
			this.groupBoxHint.Controls.Add(this.labelHint);
			this.groupBoxHint.Name = "groupBoxHint";
			this.groupBoxHint.TabStop = false;
			componentResourceManager.ApplyResources(this.labelHint, "labelHint");
			this.labelHint.Name = "labelHint";
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.groupBoxConnection, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonOk, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.buttonHelp, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.groupBoxClock, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.groupBoxHint, 0, 2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			base.AcceptButton = this.buttonOk;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonOk;
			base.ControlBox = false;
			base.Controls.Add(this.tableLayoutPanel1);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SetRealtimeClock";
			base.Shown += new EventHandler(this.SetRealtimeClock_Shown);
			base.HelpRequested += new HelpEventHandler(this.SetRealtimeClock_HelpRequested);
			this.groupBoxConnection.ResumeLayout(false);
			this.groupBoxConnection.PerformLayout();
			this.groupBoxClock.ResumeLayout(false);
			this.groupBoxClock.PerformLayout();
			this.groupBoxHint.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
