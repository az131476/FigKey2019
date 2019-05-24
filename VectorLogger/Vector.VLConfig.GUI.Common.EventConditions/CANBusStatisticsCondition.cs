using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	internal class CANBusStatisticsCondition : Form
	{
		private CANBusStatisticsEvent canBusStatEvent;

		private IModelValidator modelValidator;

		private bool isInitControls;

		private readonly uint MinimumPercentage;

		private readonly uint MaximumPercentage = 100u;

		private readonly uint MaximumErrorFrames = 65535u;

		private readonly string ConjunctionOR = "OR";

		private readonly string ConjunctionAND = "AND";

		private IContainer components;

		private Button buttonOK;

		private Button buttonCancel;

		private ComboBox comboBoxBusloadRelation;

		private CheckBox checkBoxEnableBusload;

		private Label labelBusloadLow;

		private Label labelBusloadHigh;

		private Label labelBusloadRelation;

		private TextBox textBoxBusloadLow;

		private TextBox textBoxBusloadHigh;

		private GroupBox groupBoxBusload;

		private CheckBox checkBoxEnableErrorFrames;

		private GroupBox groupBoxErrorFrames;

		private Label labelPercent2;

		private Label labelPercent1;

		private Label labelErrorFramesNumber;

		private ComboBox comboBoxErrorFramesRelation;

		private Label labelErrorFramesRelation;

		private TextBox textBoxErrorFramesNumberPerSec;

		private ErrorProvider errorProviderFormat;

		private ComboBox comboBoxConjunction;

		private Label labelConjunction;

		private Button buttonHelp;

		private Label labelChannel;

		private ComboBox comboBoxChannel;

		private ErrorProvider errorProviderGlobalModel;

		public CANBusStatisticsEvent CANBusStatisticsEvent
		{
			get
			{
				return this.canBusStatEvent;
			}
			set
			{
				this.canBusStatEvent = value;
			}
		}

		public CANBusStatisticsCondition(IModelValidator modelVal)
		{
			this.InitializeComponent();
			this.modelValidator = modelVal;
			this.canBusStatEvent = new CANBusStatisticsEvent();
			this.isInitControls = false;
			this.InitRelationComboboxes();
		}

		public void ResetToDefaults()
		{
			this.canBusStatEvent.IsBusloadEnabled.Value = true;
			this.canBusStatEvent.BusloadRelation.Value = CondRelation.InRange;
			this.canBusStatEvent.BusloadLow.Value = 50u;
			this.canBusStatEvent.BusloadHigh.Value = this.MaximumPercentage;
			this.canBusStatEvent.IsErrorFramesEnabled.Value = true;
			this.canBusStatEvent.ErrorFramesRelation.Value = CondRelation.GreaterThanOrEqual;
			this.canBusStatEvent.ErrorFramesLow.Value = 1u;
			this.canBusStatEvent.IsANDConjunction.Value = false;
			this.canBusStatEvent.ChannelNumber.Value = this.modelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_CAN);
			this.InitControls();
		}

		private void InitControls()
		{
			this.isInitControls = true;
			this.checkBoxEnableBusload.Checked = this.canBusStatEvent.IsBusloadEnabled.Value;
			this.EnableBusloadControls(this.checkBoxEnableBusload.Checked);
			this.comboBoxBusloadRelation.SelectedItem = GUIUtil.MapTriggerConditionRelation2String(this.canBusStatEvent.BusloadRelation.Value);
			this.textBoxBusloadLow.Text = this.canBusStatEvent.BusloadLow.Value.ToString();
			this.textBoxBusloadHigh.Text = this.canBusStatEvent.BusloadHigh.Value.ToString();
			this.checkBoxEnableErrorFrames.Checked = this.canBusStatEvent.IsErrorFramesEnabled.Value;
			this.EnableErrorFramesControls(this.checkBoxEnableErrorFrames.Checked);
			this.comboBoxErrorFramesRelation.SelectedItem = GUIUtil.MapTriggerConditionRelation2String(this.canBusStatEvent.ErrorFramesRelation.Value);
			this.textBoxErrorFramesNumberPerSec.Text = this.canBusStatEvent.ErrorFramesLow.Value.ToString();
			if (this.canBusStatEvent.IsANDConjunction.Value)
			{
				this.comboBoxConjunction.SelectedItem = this.ConjunctionAND;
			}
			else
			{
				this.comboBoxConjunction.SelectedItem = this.ConjunctionOR;
			}
			this.isInitControls = false;
		}

		private void InitChannelComboBox()
		{
			this.isInitControls = true;
			this.comboBoxChannel.Items.Clear();
			uint totalNumberOfLogicalChannels = this.modelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN);
			for (uint num = 1u; num <= totalNumberOfLogicalChannels; num += 1u)
			{
				this.comboBoxChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
			if (this.canBusStatEvent.ChannelNumber.Value <= totalNumberOfLogicalChannels)
			{
				this.comboBoxChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.canBusStatEvent.ChannelNumber.Value);
			}
			else
			{
				this.comboBoxChannel.SelectedIndex = 0;
			}
			this.isInitControls = false;
		}

		private void CANBusStatisticsCondition_Shown(object sender, EventArgs e)
		{
			this.InitChannelComboBox();
			this.InitControls();
			this.ValidateInput();
		}

		private void checkBoxEnableBusload_CheckedChanged(object sender, EventArgs e)
		{
			this.EnableBusloadControls(this.checkBoxEnableBusload.Checked);
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxEnableErrorFrames_CheckedChanged(object sender, EventArgs e)
		{
			this.EnableErrorFramesControls(this.checkBoxEnableErrorFrames.Checked);
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void control_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (!this.ValidateInput())
			{
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				base.DialogResult = DialogResult.None;
				return;
			}
			if (this.HasGlobalModelErrors() && InformMessageBox.Question(Resources.InconsistSettingsCommitCondAnyway) == DialogResult.No)
			{
				return;
			}
			if (this.canBusStatEvent.BusloadLow.Value > this.canBusStatEvent.BusloadHigh.Value)
			{
				uint value = this.canBusStatEvent.BusloadLow.Value;
				this.canBusStatEvent.BusloadLow.Value = this.canBusStatEvent.BusloadHigh.Value;
				this.canBusStatEvent.BusloadHigh.Value = value;
			}
			base.DialogResult = DialogResult.OK;
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void CANBusStatisticsCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private bool ValidateInput()
		{
			this.ResetErrorProvider();
			if (!this.checkBoxEnableBusload.Checked && !this.checkBoxEnableErrorFrames.Checked)
			{
				this.errorProviderFormat.SetError(this.checkBoxEnableBusload, Resources_Trigger.ErrorAtLeastOneTriggerTypeMustBeEnabled);
				this.errorProviderFormat.SetError(this.checkBoxEnableErrorFrames, Resources_Trigger.ErrorAtLeastOneTriggerTypeMustBeEnabled);
				return false;
			}
			bool result = true;
			this.canBusStatEvent.IsBusloadEnabled.Value = this.checkBoxEnableBusload.Checked;
			if (this.checkBoxEnableBusload.Checked)
			{
				this.canBusStatEvent.BusloadRelation.Value = GUIUtil.MapString2TriggerConditionRelation(this.comboBoxBusloadRelation.SelectedItem.ToString());
				uint num = 0u;
				if (uint.TryParse(this.textBoxBusloadLow.Text, out num))
				{
					if (num <= this.MaximumPercentage)
					{
						this.canBusStatEvent.BusloadLow.Value = num;
					}
					else
					{
						this.errorProviderFormat.SetError(this.textBoxBusloadLow, Resources.ErrorGenValueOutOfRange);
						result = false;
					}
				}
				else
				{
					this.errorProviderFormat.SetError(this.textBoxBusloadLow, Resources.ErrorNumberExpected);
					result = false;
				}
				if (uint.TryParse(this.textBoxBusloadHigh.Text, out num))
				{
					if (num <= this.MaximumPercentage)
					{
						this.canBusStatEvent.BusloadHigh.Value = num;
					}
					else
					{
						this.errorProviderFormat.SetError(this.textBoxBusloadHigh, Resources.ErrorGenValueOutOfRange);
						result = false;
					}
				}
				else
				{
					this.errorProviderFormat.SetError(this.textBoxBusloadHigh, Resources.ErrorNumberExpected);
					result = false;
				}
				if (string.IsNullOrEmpty(this.errorProviderFormat.GetError(this.textBoxBusloadLow)) || string.IsNullOrEmpty(this.errorProviderFormat.GetError(this.textBoxBusloadHigh)))
				{
					if ((this.canBusStatEvent.BusloadLow.Value == this.MinimumPercentage && this.canBusStatEvent.BusloadHigh.Value == this.MaximumPercentage) || (this.canBusStatEvent.BusloadLow.Value == this.MaximumPercentage && this.canBusStatEvent.BusloadHigh.Value == this.MinimumPercentage))
					{
						this.errorProviderFormat.SetError(this.textBoxBusloadLow, Resources.ErrorBusloadRange);
						this.errorProviderFormat.SetError(this.textBoxBusloadHigh, Resources.ErrorBusloadRange);
						result = false;
					}
					else if (this.canBusStatEvent.BusloadLow.Value == this.canBusStatEvent.BusloadHigh.Value)
					{
						this.errorProviderFormat.SetError(this.textBoxBusloadLow, Resources.ErrorBusloadRangeBordersEqual);
						this.errorProviderFormat.SetError(this.textBoxBusloadHigh, Resources.ErrorBusloadRangeBordersEqual);
						result = false;
					}
				}
			}
			this.canBusStatEvent.IsErrorFramesEnabled.Value = this.checkBoxEnableErrorFrames.Checked;
			if (this.checkBoxEnableErrorFrames.Checked)
			{
				this.canBusStatEvent.ErrorFramesRelation.Value = GUIUtil.MapString2TriggerConditionRelation(this.comboBoxErrorFramesRelation.SelectedItem.ToString());
				uint num2 = 0u;
				if (uint.TryParse(this.textBoxErrorFramesNumberPerSec.Text, out num2))
				{
					if (num2 <= this.MaximumErrorFrames && num2 > 0u)
					{
						this.canBusStatEvent.ErrorFramesLow.Value = num2;
					}
					else
					{
						this.errorProviderFormat.SetError(this.textBoxErrorFramesNumberPerSec, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 1, this.MaximumErrorFrames));
						result = false;
					}
				}
				else
				{
					this.errorProviderFormat.SetError(this.textBoxErrorFramesNumberPerSec, Resources.ErrorNumberExpected);
					result = false;
				}
			}
			this.canBusStatEvent.IsANDConjunction.Value = (this.comboBoxConjunction.SelectedItem.ToString() == this.ConjunctionAND);
			this.canBusStatEvent.ChannelNumber.Value = GUIUtil.MapCANChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
			if (!this.modelValidator.IsHardwareChannelAvailable(BusType.Bt_CAN, this.canBusStatEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelNotAvailable);
			}
			else if (!this.modelValidator.IsHardwareChannelActive(BusType.Bt_CAN, this.canBusStatEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelInactive);
			}
			return result;
		}

		private bool HasGlobalModelErrors()
		{
			return !string.IsNullOrEmpty(this.errorProviderGlobalModel.GetError(this.comboBoxChannel));
		}

		private void ResetErrorProvider()
		{
			this.errorProviderFormat.SetError(this.textBoxErrorFramesNumberPerSec, "");
			this.errorProviderFormat.SetError(this.textBoxBusloadLow, "");
			this.errorProviderFormat.SetError(this.textBoxBusloadHigh, "");
			this.errorProviderFormat.SetError(this.checkBoxEnableBusload, "");
			this.errorProviderFormat.SetError(this.checkBoxEnableErrorFrames, "");
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
		}

		private void EnableBusloadControls(bool isEnabled)
		{
			this.comboBoxBusloadRelation.Enabled = isEnabled;
			this.textBoxBusloadLow.Enabled = isEnabled;
			if (!isEnabled && !string.IsNullOrEmpty(this.errorProviderFormat.GetError(this.textBoxBusloadLow)))
			{
				this.textBoxBusloadLow.Text = this.canBusStatEvent.BusloadLow.Value.ToString();
			}
			this.textBoxBusloadHigh.Enabled = isEnabled;
			if (!isEnabled && !string.IsNullOrEmpty(this.errorProviderFormat.GetError(this.textBoxBusloadHigh)))
			{
				this.textBoxBusloadHigh.Text = this.canBusStatEvent.BusloadHigh.Value.ToString();
			}
			this.EnableConjunctionCombobox();
		}

		private void EnableErrorFramesControls(bool isEnabled)
		{
			this.comboBoxErrorFramesRelation.Enabled = isEnabled;
			this.textBoxErrorFramesNumberPerSec.Enabled = isEnabled;
			if (!isEnabled && !string.IsNullOrEmpty(this.errorProviderFormat.GetError(this.textBoxErrorFramesNumberPerSec)))
			{
				this.textBoxErrorFramesNumberPerSec.Text = this.canBusStatEvent.ErrorFramesLow.Value.ToString();
			}
			this.EnableConjunctionCombobox();
		}

		private void EnableConjunctionCombobox()
		{
			if (this.checkBoxEnableBusload.Checked && this.checkBoxEnableErrorFrames.Checked)
			{
				this.comboBoxConjunction.Enabled = true;
				return;
			}
			this.comboBoxConjunction.Enabled = false;
		}

		private void InitRelationComboboxes()
		{
			this.isInitControls = true;
			this.InitBusloadRelationCombobox();
			this.InitErrorFramesRelationCombobox();
			this.InitConjunctionCombobox();
			this.isInitControls = false;
		}

		private void InitBusloadRelationCombobox()
		{
			this.comboBoxBusloadRelation.Items.Clear();
			this.comboBoxBusloadRelation.Items.Add(GUIUtil.MapTriggerConditionRelation2String(CondRelation.InRange));
			this.comboBoxBusloadRelation.Items.Add(GUIUtil.MapTriggerConditionRelation2String(CondRelation.NotInRange));
			this.comboBoxBusloadRelation.SelectedIndex = 0;
		}

		private void InitErrorFramesRelationCombobox()
		{
			this.comboBoxErrorFramesRelation.Items.Clear();
			this.comboBoxErrorFramesRelation.Items.Add(GUIUtil.MapTriggerConditionRelation2String(CondRelation.GreaterThanOrEqual));
			this.comboBoxErrorFramesRelation.Items.Add(GUIUtil.MapTriggerConditionRelation2String(CondRelation.LessThanOrEqual));
			this.comboBoxErrorFramesRelation.SelectedIndex = 0;
		}

		private void InitConjunctionCombobox()
		{
			this.comboBoxConjunction.Items.Clear();
			this.comboBoxConjunction.Items.Add(this.ConjunctionOR);
			this.comboBoxConjunction.Items.Add(this.ConjunctionAND);
			this.comboBoxConjunction.SelectedIndex = 0;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CANBusStatisticsCondition));
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.comboBoxBusloadRelation = new ComboBox();
			this.checkBoxEnableBusload = new CheckBox();
			this.labelBusloadLow = new Label();
			this.labelBusloadHigh = new Label();
			this.labelBusloadRelation = new Label();
			this.textBoxBusloadLow = new TextBox();
			this.textBoxBusloadHigh = new TextBox();
			this.groupBoxBusload = new GroupBox();
			this.labelPercent2 = new Label();
			this.labelPercent1 = new Label();
			this.checkBoxEnableErrorFrames = new CheckBox();
			this.groupBoxErrorFrames = new GroupBox();
			this.textBoxErrorFramesNumberPerSec = new TextBox();
			this.labelErrorFramesNumber = new Label();
			this.comboBoxErrorFramesRelation = new ComboBox();
			this.labelErrorFramesRelation = new Label();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.comboBoxChannel = new ComboBox();
			this.comboBoxConjunction = new ComboBox();
			this.labelConjunction = new Label();
			this.buttonHelp = new Button();
			this.labelChannel = new Label();
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.groupBoxBusload.SuspendLayout();
			this.groupBoxErrorFrames.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.comboBoxBusloadRelation.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBusloadRelation.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxBusloadRelation, "comboBoxBusloadRelation");
			this.comboBoxBusloadRelation.Name = "comboBoxBusloadRelation";
			this.comboBoxBusloadRelation.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxEnableBusload, "checkBoxEnableBusload");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxEnableBusload, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxEnableBusload.IconAlignment"));
			this.checkBoxEnableBusload.Name = "checkBoxEnableBusload";
			this.checkBoxEnableBusload.UseVisualStyleBackColor = true;
			this.checkBoxEnableBusload.CheckedChanged += new EventHandler(this.checkBoxEnableBusload_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelBusloadLow, "labelBusloadLow");
			this.labelBusloadLow.Name = "labelBusloadLow";
			componentResourceManager.ApplyResources(this.labelBusloadHigh, "labelBusloadHigh");
			this.labelBusloadHigh.Name = "labelBusloadHigh";
			componentResourceManager.ApplyResources(this.labelBusloadRelation, "labelBusloadRelation");
			this.labelBusloadRelation.Name = "labelBusloadRelation";
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxBusloadLow, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxBusloadLow.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxBusloadLow, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxBusloadLow.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxBusloadLow, "textBoxBusloadLow");
			this.textBoxBusloadLow.Name = "textBoxBusloadLow";
			this.textBoxBusloadLow.Validating += new CancelEventHandler(this.control_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxBusloadHigh, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxBusloadHigh.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxBusloadHigh, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxBusloadHigh.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxBusloadHigh, "textBoxBusloadHigh");
			this.textBoxBusloadHigh.Name = "textBoxBusloadHigh";
			this.textBoxBusloadHigh.Validating += new CancelEventHandler(this.control_Validating);
			this.groupBoxBusload.Controls.Add(this.labelPercent2);
			this.groupBoxBusload.Controls.Add(this.labelPercent1);
			this.groupBoxBusload.Controls.Add(this.textBoxBusloadHigh);
			this.groupBoxBusload.Controls.Add(this.textBoxBusloadLow);
			this.groupBoxBusload.Controls.Add(this.labelBusloadRelation);
			this.groupBoxBusload.Controls.Add(this.labelBusloadHigh);
			this.groupBoxBusload.Controls.Add(this.labelBusloadLow);
			this.groupBoxBusload.Controls.Add(this.checkBoxEnableBusload);
			this.groupBoxBusload.Controls.Add(this.comboBoxBusloadRelation);
			componentResourceManager.ApplyResources(this.groupBoxBusload, "groupBoxBusload");
			this.groupBoxBusload.Name = "groupBoxBusload";
			this.groupBoxBusload.TabStop = false;
			componentResourceManager.ApplyResources(this.labelPercent2, "labelPercent2");
			this.labelPercent2.Name = "labelPercent2";
			componentResourceManager.ApplyResources(this.labelPercent1, "labelPercent1");
			this.labelPercent1.Name = "labelPercent1";
			componentResourceManager.ApplyResources(this.checkBoxEnableErrorFrames, "checkBoxEnableErrorFrames");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxEnableErrorFrames, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxEnableErrorFrames.IconAlignment"));
			this.checkBoxEnableErrorFrames.Name = "checkBoxEnableErrorFrames";
			this.checkBoxEnableErrorFrames.UseVisualStyleBackColor = true;
			this.checkBoxEnableErrorFrames.CheckedChanged += new EventHandler(this.checkBoxEnableErrorFrames_CheckedChanged);
			this.groupBoxErrorFrames.Controls.Add(this.textBoxErrorFramesNumberPerSec);
			this.groupBoxErrorFrames.Controls.Add(this.labelErrorFramesNumber);
			this.groupBoxErrorFrames.Controls.Add(this.comboBoxErrorFramesRelation);
			this.groupBoxErrorFrames.Controls.Add(this.labelErrorFramesRelation);
			this.groupBoxErrorFrames.Controls.Add(this.checkBoxEnableErrorFrames);
			componentResourceManager.ApplyResources(this.groupBoxErrorFrames, "groupBoxErrorFrames");
			this.groupBoxErrorFrames.Name = "groupBoxErrorFrames";
			this.groupBoxErrorFrames.TabStop = false;
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxErrorFramesNumberPerSec, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxErrorFramesNumberPerSec.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxErrorFramesNumberPerSec, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxErrorFramesNumberPerSec.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxErrorFramesNumberPerSec, "textBoxErrorFramesNumberPerSec");
			this.textBoxErrorFramesNumberPerSec.Name = "textBoxErrorFramesNumberPerSec";
			this.textBoxErrorFramesNumberPerSec.Validating += new CancelEventHandler(this.control_Validating);
			componentResourceManager.ApplyResources(this.labelErrorFramesNumber, "labelErrorFramesNumber");
			this.labelErrorFramesNumber.Name = "labelErrorFramesNumber";
			this.comboBoxErrorFramesRelation.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxErrorFramesRelation.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxErrorFramesRelation, "comboBoxErrorFramesRelation");
			this.comboBoxErrorFramesRelation.Name = "comboBoxErrorFramesRelation";
			this.comboBoxErrorFramesRelation.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelErrorFramesRelation, "labelErrorFramesRelation");
			this.labelErrorFramesRelation.Name = "labelErrorFramesRelation";
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.comboBoxChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxChannel.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment1"));
			componentResourceManager.ApplyResources(this.comboBoxChannel, "comboBoxChannel");
			this.comboBoxChannel.Name = "comboBoxChannel";
			this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			this.comboBoxConjunction.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxConjunction.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxConjunction, "comboBoxConjunction");
			this.comboBoxConjunction.Name = "comboBoxConjunction";
			this.comboBoxConjunction.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelConjunction, "labelConjunction");
			this.labelConjunction.Name = "labelConjunction";
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.labelChannel, "labelChannel");
			this.labelChannel.Name = "labelChannel";
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.labelChannel);
			base.Controls.Add(this.comboBoxChannel);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.labelConjunction);
			base.Controls.Add(this.comboBoxConjunction);
			base.Controls.Add(this.groupBoxErrorFrames);
			base.Controls.Add(this.groupBoxBusload);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CANBusStatisticsCondition";
			base.Shown += new EventHandler(this.CANBusStatisticsCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.CANBusStatisticsCondition_HelpRequested);
			this.groupBoxBusload.ResumeLayout(false);
			this.groupBoxBusload.PerformLayout();
			this.groupBoxErrorFrames.ResumeLayout(false);
			this.groupBoxErrorFrames.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
