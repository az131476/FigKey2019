using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	public class CyclicTimerCondition : Form
	{
		private uint interval_ms;

		private uint interval_s;

		private uint interval_min;

		private CyclicTimerEvent cyclicTimerEvent;

		private bool isInitControls;

		private uint MinimumOnCyclicTimer_ms;

		private IContainer components;

		private TextBox textBoxCycleTime_ms;

		private Button buttonOK;

		private Button buttonCancel;

		private ErrorProvider errorProvider;

		private RadioButton radioButtonMilliSec;

		private GroupBox groupBoxCycleTime;

		private TextBox textBoxCycleTime_min;

		private RadioButton radioButtonMin;

		private TextBox textBoxCycleTime_s;

		private RadioButton radioButtonSec;

		private GroupBox groupBoxDelay;

		private Label labelDelayCycles;

		private TextBox textBoxDelayCycles;

		private Label labelDelayTimeValue;

		private Label labelDelayTime;

		private Button buttonHelp;

		public CyclicTimerEvent CyclicTimerEvent
		{
			get
			{
				return this.cyclicTimerEvent;
			}
			set
			{
				this.cyclicTimerEvent = value;
			}
		}

		public CyclicTimerCondition()
		{
			this.InitializeComponent();
			this.cyclicTimerEvent = new CyclicTimerEvent();
			this.interval_ms = 1u;
			this.interval_s = 1u;
			this.interval_min = 1u;
			this.isInitControls = false;
			this.MinimumOnCyclicTimer_ms = Constants.MinimumOnCyclicTimer;
			this.ResetErrorProvider();
		}

		public void ResetToDefaults()
		{
			if (this.cyclicTimerEvent == null)
			{
				this.cyclicTimerEvent = new CyclicTimerEvent();
			}
			this.cyclicTimerEvent.TimeUnit.Value = TimeUnit.Sec;
			this.cyclicTimerEvent.Interval.Value = Constants.DefaultOnCyclicTimer_s;
			this.cyclicTimerEvent.DelayCycles.Value = 1u;
		}

		public void IncreaseMinimum()
		{
			this.radioButtonMilliSec.Text = this.radioButtonMilliSec.Text.Replace("1", "1000");
			this.MinimumOnCyclicTimer_ms = 1000u;
		}

		private void CyclicTimerCondition_Shown(object sender, EventArgs e)
		{
			this.isInitControls = true;
			this.textBoxCycleTime_ms.Enabled = false;
			this.textBoxCycleTime_s.Enabled = false;
			this.textBoxCycleTime_min.Enabled = false;
			this.ResetErrorProvider();
			this.textBoxDelayCycles.Text = this.cyclicTimerEvent.DelayCycles.Value.ToString();
			this.interval_ms = Constants.DefaultOnCyclicTimer_ms;
			this.interval_s = Constants.DefaultOnCyclicTimer_s;
			this.interval_min = Constants.DefaultOnCyclicTimer_m;
			this.textBoxCycleTime_ms.Text = this.interval_ms.ToString();
			this.textBoxCycleTime_s.Text = this.interval_s.ToString();
			this.textBoxCycleTime_min.Text = this.interval_min.ToString();
			switch (this.cyclicTimerEvent.TimeUnit.Value)
			{
			case TimeUnit.Sec:
				this.textBoxCycleTime_s.Text = this.cyclicTimerEvent.Interval.Value.ToString();
				this.radioButtonSec.Checked = true;
				this.textBoxCycleTime_s.Enabled = true;
				break;
			case TimeUnit.Min:
				this.textBoxCycleTime_min.Text = this.cyclicTimerEvent.Interval.Value.ToString();
				this.radioButtonMin.Checked = true;
				this.textBoxCycleTime_min.Enabled = true;
				break;
			default:
				this.textBoxCycleTime_ms.Text = this.cyclicTimerEvent.Interval.Value.ToString();
				this.radioButtonMilliSec.Checked = true;
				this.textBoxCycleTime_ms.Enabled = true;
				break;
			}
			this.GenerateResultingDelayTimeString();
			this.isInitControls = false;
		}

		private void textBoxDelay_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void textBoxCycleTime_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
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

		private void radioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.textBoxCycleTime_ms.Enabled = this.radioButtonMilliSec.Checked;
			if (!this.textBoxCycleTime_ms.Enabled && !string.IsNullOrEmpty(this.errorProvider.GetError(this.textBoxCycleTime_ms)))
			{
				this.errorProvider.SetError(this.textBoxCycleTime_ms, "");
				this.textBoxCycleTime_ms.Text = this.interval_ms.ToString();
			}
			this.textBoxCycleTime_s.Enabled = this.radioButtonSec.Checked;
			if (!this.textBoxCycleTime_s.Enabled && !string.IsNullOrEmpty(this.errorProvider.GetError(this.textBoxCycleTime_s)))
			{
				this.errorProvider.SetError(this.textBoxCycleTime_s, "");
				this.textBoxCycleTime_s.Text = this.interval_s.ToString();
			}
			this.textBoxCycleTime_min.Enabled = this.radioButtonMin.Checked;
			if (!this.textBoxCycleTime_min.Enabled && !string.IsNullOrEmpty(this.errorProvider.GetError(this.textBoxCycleTime_min)))
			{
				this.errorProvider.SetError(this.textBoxCycleTime_min, "");
				this.textBoxCycleTime_min.Text = this.interval_min.ToString();
			}
			if (this.radioButtonSec.Checked)
			{
				this.cyclicTimerEvent.TimeUnit.Value = TimeUnit.Sec;
			}
			else if (this.radioButtonMin.Checked)
			{
				this.cyclicTimerEvent.TimeUnit.Value = TimeUnit.Min;
			}
			else
			{
				this.cyclicTimerEvent.TimeUnit.Value = TimeUnit.MilliSec;
			}
			this.ValidateInput();
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void CyclicTimerCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private bool ValidateInput()
		{
			bool flag = true;
			this.ResetErrorProvider();
			uint num;
			if (!uint.TryParse(this.textBoxDelayCycles.Text, NumberStyles.Integer, ProgramUtils.Culture, out num))
			{
				this.errorProvider.SetError(this.textBoxDelayCycles, Resources.ErrorNumberExpected);
				flag = false;
			}
			else
			{
				this.textBoxDelayCycles.Text = num.ToString();
				if (num < Constants.MinimumOnStartDelayCycles || num > Constants.MaximumOnStartDelayCycles)
				{
					this.errorProvider.SetError(this.textBoxDelayCycles, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinimumOnStartDelayCycles, Constants.MaximumOnStartDelayCycles));
					flag = false;
				}
				else
				{
					this.cyclicTimerEvent.DelayCycles.Value = num;
				}
			}
			uint num2 = 0u;
			switch (this.cyclicTimerEvent.TimeUnit.Value)
			{
			case TimeUnit.Sec:
				if (!uint.TryParse(this.textBoxCycleTime_s.Text, out num2))
				{
					this.errorProvider.SetError(this.textBoxCycleTime_s, Resources.ErrorNumberExpected);
					flag = false;
				}
				else if (num2 < Constants.MinimumOnCyclicTimer || num2 > Constants.MaximumOnCyclicTimer_s)
				{
					this.errorProvider.SetError(this.textBoxCycleTime_s, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinimumOnCyclicTimer, Constants.MaximumOnCyclicTimer_s));
					flag = false;
				}
				else
				{
					this.interval_s = num2;
					this.cyclicTimerEvent.Interval.Value = this.interval_s;
				}
				break;
			case TimeUnit.Min:
				if (!uint.TryParse(this.textBoxCycleTime_min.Text, out num2))
				{
					this.errorProvider.SetError(this.textBoxCycleTime_min, Resources.ErrorNumberExpected);
					flag = false;
				}
				else if (num2 < Constants.MinimumOnCyclicTimer || num2 > Constants.MaximumOnCyclicTimer_m)
				{
					this.errorProvider.SetError(this.textBoxCycleTime_min, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, Constants.MinimumOnCyclicTimer, Constants.MaximumOnCyclicTimer_m));
					flag = false;
				}
				else
				{
					this.interval_min = num2;
					this.cyclicTimerEvent.Interval.Value = this.interval_min;
				}
				break;
			default:
				if (!uint.TryParse(this.textBoxCycleTime_ms.Text, out num2))
				{
					this.errorProvider.SetError(this.textBoxCycleTime_ms, Resources.ErrorNumberExpected);
					flag = false;
				}
				else if (num2 < this.MinimumOnCyclicTimer_ms || num2 > Constants.MaximumOnCyclicTimer_ms)
				{
					this.errorProvider.SetError(this.textBoxCycleTime_ms, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, this.MinimumOnCyclicTimer_ms, Constants.MaximumOnCyclicTimer_ms));
					flag = false;
				}
				else
				{
					this.interval_ms = num2;
					this.cyclicTimerEvent.Interval.Value = this.interval_ms;
				}
				break;
			}
			if (flag)
			{
				this.GenerateResultingDelayTimeString();
			}
			else
			{
				this.labelDelayTimeValue.Text = Resources.Unknown;
			}
			return flag;
		}

		private void ResetErrorProvider()
		{
			this.errorProvider.SetError(this.textBoxDelayCycles, "");
			this.errorProvider.SetError(this.textBoxCycleTime_ms, "");
			this.errorProvider.SetError(this.textBoxCycleTime_s, "");
			this.errorProvider.SetError(this.textBoxCycleTime_min, "");
		}

		private void GenerateResultingDelayTimeString()
		{
			uint num = this.cyclicTimerEvent.Interval.Value * this.cyclicTimerEvent.DelayCycles.Value;
			this.labelDelayTimeValue.Text = string.Format("{0} {1}", num, GUIUtil.GetTimeUnitString(this.cyclicTimerEvent.TimeUnit.Value));
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CyclicTimerCondition));
			this.textBoxCycleTime_ms = new TextBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.errorProvider = new ErrorProvider(this.components);
			this.textBoxCycleTime_s = new TextBox();
			this.textBoxCycleTime_min = new TextBox();
			this.textBoxDelayCycles = new TextBox();
			this.radioButtonMilliSec = new RadioButton();
			this.radioButtonSec = new RadioButton();
			this.radioButtonMin = new RadioButton();
			this.groupBoxCycleTime = new GroupBox();
			this.groupBoxDelay = new GroupBox();
			this.labelDelayTimeValue = new Label();
			this.labelDelayTime = new Label();
			this.labelDelayCycles = new Label();
			this.buttonHelp = new Button();
			((ISupportInitialize)this.errorProvider).BeginInit();
			this.groupBoxCycleTime.SuspendLayout();
			this.groupBoxDelay.SuspendLayout();
			base.SuspendLayout();
			this.errorProvider.SetIconAlignment(this.textBoxCycleTime_ms, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCycleTime_ms.IconAlignment"));
			componentResourceManager.ApplyResources(this.textBoxCycleTime_ms, "textBoxCycleTime_ms");
			this.textBoxCycleTime_ms.Name = "textBoxCycleTime_ms";
			this.textBoxCycleTime_ms.Validating += new CancelEventHandler(this.textBoxCycleTime_Validating);
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProvider.ContainerControl = this;
			this.errorProvider.SetIconAlignment(this.textBoxCycleTime_s, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCycleTime_s.IconAlignment"));
			componentResourceManager.ApplyResources(this.textBoxCycleTime_s, "textBoxCycleTime_s");
			this.textBoxCycleTime_s.Name = "textBoxCycleTime_s";
			this.textBoxCycleTime_s.Validating += new CancelEventHandler(this.textBoxCycleTime_Validating);
			this.errorProvider.SetIconAlignment(this.textBoxCycleTime_min, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCycleTime_min.IconAlignment"));
			componentResourceManager.ApplyResources(this.textBoxCycleTime_min, "textBoxCycleTime_min");
			this.textBoxCycleTime_min.Name = "textBoxCycleTime_min";
			this.textBoxCycleTime_min.Validating += new CancelEventHandler(this.textBoxCycleTime_Validating);
			this.errorProvider.SetIconAlignment(this.textBoxDelayCycles, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDelayCycles.IconAlignment"));
			componentResourceManager.ApplyResources(this.textBoxDelayCycles, "textBoxDelayCycles");
			this.textBoxDelayCycles.Name = "textBoxDelayCycles";
			this.textBoxDelayCycles.Validating += new CancelEventHandler(this.textBoxDelay_Validating);
			componentResourceManager.ApplyResources(this.radioButtonMilliSec, "radioButtonMilliSec");
			this.radioButtonMilliSec.Name = "radioButtonMilliSec";
			this.radioButtonMilliSec.UseVisualStyleBackColor = true;
			this.radioButtonMilliSec.CheckedChanged += new EventHandler(this.radioButton_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonSec, "radioButtonSec");
			this.radioButtonSec.Name = "radioButtonSec";
			this.radioButtonSec.UseVisualStyleBackColor = true;
			this.radioButtonSec.CheckedChanged += new EventHandler(this.radioButton_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonMin, "radioButtonMin");
			this.radioButtonMin.Name = "radioButtonMin";
			this.radioButtonMin.UseVisualStyleBackColor = true;
			this.radioButtonMin.CheckedChanged += new EventHandler(this.radioButton_CheckedChanged);
			this.groupBoxCycleTime.Controls.Add(this.textBoxCycleTime_min);
			this.groupBoxCycleTime.Controls.Add(this.radioButtonMin);
			this.groupBoxCycleTime.Controls.Add(this.textBoxCycleTime_s);
			this.groupBoxCycleTime.Controls.Add(this.radioButtonSec);
			this.groupBoxCycleTime.Controls.Add(this.radioButtonMilliSec);
			this.groupBoxCycleTime.Controls.Add(this.textBoxCycleTime_ms);
			componentResourceManager.ApplyResources(this.groupBoxCycleTime, "groupBoxCycleTime");
			this.groupBoxCycleTime.Name = "groupBoxCycleTime";
			this.groupBoxCycleTime.TabStop = false;
			this.groupBoxDelay.Controls.Add(this.labelDelayTimeValue);
			this.groupBoxDelay.Controls.Add(this.labelDelayTime);
			this.groupBoxDelay.Controls.Add(this.textBoxDelayCycles);
			this.groupBoxDelay.Controls.Add(this.labelDelayCycles);
			componentResourceManager.ApplyResources(this.groupBoxDelay, "groupBoxDelay");
			this.groupBoxDelay.Name = "groupBoxDelay";
			this.groupBoxDelay.TabStop = false;
			componentResourceManager.ApplyResources(this.labelDelayTimeValue, "labelDelayTimeValue");
			this.labelDelayTimeValue.Name = "labelDelayTimeValue";
			componentResourceManager.ApplyResources(this.labelDelayTime, "labelDelayTime");
			this.labelDelayTime.Name = "labelDelayTime";
			componentResourceManager.ApplyResources(this.labelDelayCycles, "labelDelayCycles");
			this.labelDelayCycles.Name = "labelDelayCycles";
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.groupBoxDelay);
			base.Controls.Add(this.groupBoxCycleTime);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CyclicTimerCondition";
			base.Shown += new EventHandler(this.CyclicTimerCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.CyclicTimerCondition_HelpRequested);
			((ISupportInitialize)this.errorProvider).EndInit();
			this.groupBoxCycleTime.ResumeLayout(false);
			this.groupBoxCycleTime.PerformLayout();
			this.groupBoxDelay.ResumeLayout(false);
			this.groupBoxDelay.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
