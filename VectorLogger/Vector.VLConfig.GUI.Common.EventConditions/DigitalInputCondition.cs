using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	internal class DigitalInputCondition : Form
	{
		private DigitalInputEvent digitalInputEvent;

		private IModelValidator modelValidator;

		private bool mIsSignalState;

		private IContainer components;

		private Label labelInput;

		private ComboBox comboBoxChannel;

		private Label labelEdge;

		private ComboBox comboBoxEdge;

		private Button buttonHelp;

		private Button buttonCancel;

		private Button buttonOK;

		private Label labelState;

		private CheckBox checkBoxUseDebounce;

		private Label labelMs;

		private ComboBox comboBoxDebounceTime;

		public DigitalInputEvent DigitalInputEvent
		{
			get
			{
				return this.digitalInputEvent;
			}
			set
			{
				this.digitalInputEvent = value;
			}
		}

		public DigitalInputCondition(IModelValidator modelVal)
		{
			this.InitializeComponent();
			this.modelValidator = modelVal;
			this.digitalInputEvent = new DigitalInputEvent();
			this.mIsSignalState = false;
			this.InitEdgeLabelAndComboBox();
			this.InitDebounceTimeComboBox();
		}

		public DigitalInputCondition(IModelValidator modelVal, bool isSigState)
		{
			this.InitializeComponent();
			this.modelValidator = modelVal;
			this.digitalInputEvent = new DigitalInputEvent();
			this.mIsSignalState = isSigState;
			this.InitEdgeLabelAndComboBox();
			this.InitDebounceTimeComboBox();
		}

		public void ResetToDefaults()
		{
			this.digitalInputEvent.DigitalInput.Value = 1u;
			this.digitalInputEvent.Edge.Value = false;
			this.digitalInputEvent.IsDebounceActive.Value = true;
			this.digitalInputEvent.DebounceTime.Value = Constants.DefaultDigInDebaunceTime_ms;
			this.ApplyValuesToControls();
		}

		private void InitChannelComboBox()
		{
			this.comboBoxChannel.Items.Clear();
			for (uint num = 1u; num <= this.modelValidator.LoggerSpecifics.IO.NumberOfDigitalInputs; num += 1u)
			{
				this.comboBoxChannel.Items.Add(GUIUtil.MapDigitalInputNumber2String(num));
			}
		}

		private void InitEdgeLabelAndComboBox()
		{
			this.comboBoxEdge.SelectedIndexChanged -= new EventHandler(this.comboBoxEdge_SelectedIndexChanged);
			this.comboBoxEdge.Items.Clear();
			if (this.mIsSignalState)
			{
				this.labelState.Visible = true;
				this.labelEdge.Visible = false;
				this.comboBoxEdge.Items.Add(Resources.DigitalInputStateHigh);
				this.comboBoxEdge.Items.Add(Resources.DigitalInputStateLow);
			}
			else
			{
				this.labelState.Visible = false;
				this.labelEdge.Visible = true;
				this.comboBoxEdge.Items.Add(Resources.DigitalInputEdgeLowToHigh);
				this.comboBoxEdge.Items.Add(Resources.DigitalInputEdgeHighToLow);
			}
			this.comboBoxEdge.SelectedIndex = 0;
			this.comboBoxEdge.SelectedIndexChanged += new EventHandler(this.comboBoxEdge_SelectedIndexChanged);
		}

		private void InitDebounceTimeComboBox()
		{
			this.comboBoxDebounceTime.SelectedIndexChanged -= new EventHandler(this.comboBoxDebounceTime_SelectedIndexChanged);
			this.comboBoxDebounceTime.Items.Clear();
			for (uint num = 50u; num <= 1000u; num += 50u)
			{
				this.comboBoxDebounceTime.Items.Add(num.ToString());
			}
			this.comboBoxDebounceTime.SelectedIndex = 0;
			this.comboBoxDebounceTime.SelectedIndexChanged -= new EventHandler(this.comboBoxDebounceTime_SelectedIndexChanged);
		}

		private void DigitalInputCondition_Shown(object sender, EventArgs e)
		{
			this.InitChannelComboBox();
			this.ApplyValuesToControls();
		}

		private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ValidateInput();
		}

		private void comboBoxEdge_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ValidateInput();
		}

		private void checkBoxUseDebounce_CheckedChanged(object sender, EventArgs e)
		{
			this.comboBoxDebounceTime.Enabled = this.checkBoxUseDebounce.Checked;
			this.ValidateInput();
		}

		private void comboBoxDebounceTime_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ValidateInput();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.ValidateInput();
			base.DialogResult = DialogResult.OK;
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void DigitalInputCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void ValidateInput()
		{
			if (this.mIsSignalState)
			{
				this.digitalInputEvent.Edge.Value = (Resources.DigitalInputStateHigh == this.comboBoxEdge.SelectedItem.ToString());
			}
			else
			{
				this.digitalInputEvent.Edge.Value = (Resources.DigitalInputEdgeLowToHigh == this.comboBoxEdge.SelectedItem.ToString());
			}
			this.digitalInputEvent.DigitalInput.Value = GUIUtil.MapDigitalInputString2Number(this.comboBoxChannel.SelectedItem.ToString());
			this.digitalInputEvent.IsDebounceActive.Value = this.checkBoxUseDebounce.Checked;
			if (this.digitalInputEvent.IsDebounceActive.Value)
			{
				this.digitalInputEvent.DebounceTime.Value = uint.Parse(this.comboBoxDebounceTime.SelectedItem.ToString());
			}
		}

		private void ApplyValuesToControls()
		{
			this.SubscribeControlEvents(false);
			this.comboBoxEdge.SelectedItem = this.MapEdgeStateToString(this.digitalInputEvent.Edge.Value, this.mIsSignalState);
			this.comboBoxChannel.SelectedItem = GUIUtil.MapDigitalInputNumber2String(this.digitalInputEvent.DigitalInput.Value);
			this.checkBoxUseDebounce.Checked = this.digitalInputEvent.IsDebounceActive.Value;
			this.comboBoxDebounceTime.Enabled = this.checkBoxUseDebounce.Checked;
			this.comboBoxDebounceTime.SelectedItem = this.digitalInputEvent.DebounceTime.Value.ToString();
			this.SubscribeControlEvents(true);
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.comboBoxEdge.SelectedIndexChanged += new EventHandler(this.comboBoxEdge_SelectedIndexChanged);
				this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
				this.checkBoxUseDebounce.CheckedChanged += new EventHandler(this.checkBoxUseDebounce_CheckedChanged);
				this.comboBoxDebounceTime.SelectedIndexChanged += new EventHandler(this.comboBoxDebounceTime_SelectedIndexChanged);
				return;
			}
			this.comboBoxEdge.SelectedIndexChanged -= new EventHandler(this.comboBoxEdge_SelectedIndexChanged);
			this.comboBoxChannel.SelectedIndexChanged -= new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			this.checkBoxUseDebounce.CheckedChanged -= new EventHandler(this.checkBoxUseDebounce_CheckedChanged);
			this.comboBoxDebounceTime.SelectedIndexChanged -= new EventHandler(this.comboBoxDebounceTime_SelectedIndexChanged);
		}

		private string MapEdgeStateToString(bool edge, bool isSignalState)
		{
			if (!isSignalState)
			{
				if (edge)
				{
					return Resources.DigitalInputEdgeLowToHigh;
				}
				return Resources.DigitalInputEdgeHighToLow;
			}
			else
			{
				if (edge)
				{
					return Resources.DigitalInputStateHigh;
				}
				return Resources.DigitalInputStateLow;
			}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DigitalInputCondition));
			this.labelInput = new Label();
			this.comboBoxChannel = new ComboBox();
			this.labelEdge = new Label();
			this.comboBoxEdge = new ComboBox();
			this.buttonHelp = new Button();
			this.buttonCancel = new Button();
			this.buttonOK = new Button();
			this.labelState = new Label();
			this.checkBoxUseDebounce = new CheckBox();
			this.labelMs = new Label();
			this.comboBoxDebounceTime = new ComboBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelInput, "labelInput");
			this.labelInput.Name = "labelInput";
			componentResourceManager.ApplyResources(this.comboBoxChannel, "comboBoxChannel");
			this.comboBoxChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxChannel.FormattingEnabled = true;
			this.comboBoxChannel.Name = "comboBoxChannel";
			this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelEdge, "labelEdge");
			this.labelEdge.Name = "labelEdge";
			componentResourceManager.ApplyResources(this.comboBoxEdge, "comboBoxEdge");
			this.comboBoxEdge.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxEdge.FormattingEnabled = true;
			this.comboBoxEdge.Name = "comboBoxEdge";
			this.comboBoxEdge.SelectedIndexChanged += new EventHandler(this.comboBoxEdge_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this.labelState, "labelState");
			this.labelState.Name = "labelState";
			componentResourceManager.ApplyResources(this.checkBoxUseDebounce, "checkBoxUseDebounce");
			this.checkBoxUseDebounce.Checked = true;
			this.checkBoxUseDebounce.CheckState = CheckState.Checked;
			this.checkBoxUseDebounce.Name = "checkBoxUseDebounce";
			this.checkBoxUseDebounce.UseVisualStyleBackColor = true;
			this.checkBoxUseDebounce.CheckedChanged += new EventHandler(this.checkBoxUseDebounce_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelMs, "labelMs");
			this.labelMs.Name = "labelMs";
			componentResourceManager.ApplyResources(this.comboBoxDebounceTime, "comboBoxDebounceTime");
			this.comboBoxDebounceTime.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDebounceTime.FormattingEnabled = true;
			this.comboBoxDebounceTime.Name = "comboBoxDebounceTime";
			this.comboBoxDebounceTime.SelectedIndexChanged += new EventHandler(this.comboBoxDebounceTime_SelectedIndexChanged);
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.comboBoxDebounceTime);
			base.Controls.Add(this.labelMs);
			base.Controls.Add(this.checkBoxUseDebounce);
			base.Controls.Add(this.labelState);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.comboBoxEdge);
			base.Controls.Add(this.labelEdge);
			base.Controls.Add(this.comboBoxChannel);
			base.Controls.Add(this.labelInput);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DigitalInputCondition";
			base.Shown += new EventHandler(this.DigitalInputCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.DigitalInputCondition_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
