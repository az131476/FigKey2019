using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	public class ClockTimedCondition : Form
	{
		private bool isInitControls;

		private IContainer components;

		private Label labelStartTime;

		private DateTimePicker timePicker;

		private Label labelRepetition;

		private ComboBox comboBoxRepetition;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonHelp;

		public ClockTimedEvent ClockTimedEvent
		{
			get;
			set;
		}

		public ClockTimedCondition()
		{
			this.isInitControls = false;
			this.InitializeComponent();
			this.InitRepetitionCombobox();
		}

		private void InitRepetitionCombobox()
		{
			this.isInitControls = true;
			this.comboBoxRepetition.Items.Clear();
			this.comboBoxRepetition.Items.Add(Resources.RepetitionDaily);
			this.comboBoxRepetition.Items.Add(Resources.Repetition8h);
			this.comboBoxRepetition.Items.Add(Resources.Repetition4h);
			this.comboBoxRepetition.Items.Add(Resources.Repetition2h);
			this.comboBoxRepetition.Items.Add(Resources.Repetition1h);
			this.comboBoxRepetition.Items.Add(Resources.Repetition30min);
			this.comboBoxRepetition.Items.Add(Resources.Repetition15min);
			this.comboBoxRepetition.SelectedIndex = 0;
			this.isInitControls = false;
		}

		private void ClockTimedCondition_Shown(object sender, EventArgs e)
		{
			this.isInitControls = true;
			this.timePicker.Value = this.ClockTimedEvent.StartTime.Value;
			this.comboBoxRepetition.SelectedItem = ClockTimedCondition.GetEntryTextForTimeSpan(this.ClockTimedEvent.RepetitionInterval.Value);
			this.isInitControls = false;
		}

		private void timePicker_ValueChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBoxRepetition_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.Cancel;
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void ClockTimedCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private bool ValidateInput()
		{
			this.ClockTimedEvent.StartTime.Value = this.timePicker.Value;
			this.ClockTimedEvent.RepetitionInterval.Value = ClockTimedCondition.GetTimeSpanForRepetitionEntry(this.comboBoxRepetition.SelectedItem.ToString());
			return true;
		}

		private static TimeSpan GetTimeSpanForRepetitionEntry(string entryText)
		{
			if (Resources.Repetition8h == entryText)
			{
				return ClockTimedEvent.TimeSpan_8h;
			}
			if (Resources.Repetition4h == entryText)
			{
				return ClockTimedEvent.TimeSpan_4h;
			}
			if (Resources.Repetition2h == entryText)
			{
				return ClockTimedEvent.TimeSpan_2h;
			}
			if (Resources.Repetition1h == entryText)
			{
				return ClockTimedEvent.TimeSpan_1h;
			}
			if (Resources.Repetition30min == entryText)
			{
				return ClockTimedEvent.TimeSpan_30min;
			}
			if (Resources.Repetition15min == entryText)
			{
				return ClockTimedEvent.TimeSpan_15min;
			}
			return ClockTimedEvent.TimeSpan_Daily;
		}

		private static string GetEntryTextForTimeSpan(TimeSpan span)
		{
			if (span >= ClockTimedEvent.TimeSpan_Daily)
			{
				return Resources.RepetitionDaily;
			}
			if (span >= ClockTimedEvent.TimeSpan_8h)
			{
				return Resources.Repetition8h;
			}
			if (span >= ClockTimedEvent.TimeSpan_4h)
			{
				return Resources.Repetition4h;
			}
			if (span >= ClockTimedEvent.TimeSpan_2h)
			{
				return Resources.Repetition2h;
			}
			if (span >= ClockTimedEvent.TimeSpan_1h)
			{
				return Resources.Repetition1h;
			}
			if (span >= ClockTimedEvent.TimeSpan_30min)
			{
				return Resources.Repetition30min;
			}
			if (span >= ClockTimedEvent.TimeSpan_15min)
			{
				return Resources.Repetition15min;
			}
			return Resources.RepetitionDaily;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ClockTimedCondition));
			this.labelStartTime = new Label();
			this.timePicker = new DateTimePicker();
			this.labelRepetition = new Label();
			this.comboBoxRepetition = new ComboBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelStartTime, "labelStartTime");
			this.labelStartTime.Name = "labelStartTime";
			componentResourceManager.ApplyResources(this.timePicker, "timePicker");
			this.timePicker.Format = DateTimePickerFormat.Custom;
			this.timePicker.Name = "timePicker";
			this.timePicker.ShowUpDown = true;
			this.timePicker.ValueChanged += new EventHandler(this.timePicker_ValueChanged);
			componentResourceManager.ApplyResources(this.labelRepetition, "labelRepetition");
			this.labelRepetition.Name = "labelRepetition";
			this.comboBoxRepetition.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxRepetition.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxRepetition, "comboBoxRepetition");
			this.comboBoxRepetition.Name = "comboBoxRepetition";
			this.comboBoxRepetition.SelectedIndexChanged += new EventHandler(this.comboBoxRepetition_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ControlBox = false;
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.comboBoxRepetition);
			base.Controls.Add(this.labelRepetition);
			base.Controls.Add(this.timePicker);
			base.Controls.Add(this.labelStartTime);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "ClockTimedCondition";
			base.Shown += new EventHandler(this.ClockTimedCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.ClockTimedCondition_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
