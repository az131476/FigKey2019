using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	internal class FlexrayIdCondition : Form
	{
		private FlexrayIdEvent flexrayIdEvent;

		private IModelValidator modelValidator;

		private IContainer components;

		private Button buttonHelp;

		private Button buttonCancel;

		private Button buttonOK;

		private Label labelRelation;

		private ComboBox comboBoxConditionType;

		private TextBox textBoxId;

		private TextBox textBoxIdLast;

		private Label labelSlotId;

		private Label labelIdLast;

		private TextBox textBoxCycleTime;

		private Label labelCycleTime;

		private Label labelCycleRepetition;

		private ErrorProvider errorProviderFormat;

		private ComboBox comboBoxCycleRepetition;

		private ComboBox comboBoxChannel;

		private Label labelChannel;

		private ErrorProvider errorProviderGlobalModel;

		public FlexrayIdEvent FlexrayIdEvent
		{
			get
			{
				return this.flexrayIdEvent;
			}
			set
			{
				this.flexrayIdEvent = value;
			}
		}

		public FlexrayIdCondition(IModelValidator modelVal)
		{
			this.InitializeComponent();
			this.modelValidator = modelVal;
			this.flexrayIdEvent = new FlexrayIdEvent();
			this.InitRelationComboBox();
			this.ResetToDefaults();
		}

		public void ResetToDefaults()
		{
			this.flexrayIdEvent.LowId.Value = Constants.MinimumFlexraySlotId;
			this.flexrayIdEvent.HighId.Value = Constants.MinimumFlexraySlotId;
			this.flexrayIdEvent.IdRelation.Value = CondRelation.Equal;
			this.flexrayIdEvent.BaseCycle.Value = 0u;
			this.flexrayIdEvent.CycleRepetition.Value = 1u;
			this.flexrayIdEvent.ChannelNumber.Value = this.modelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_FlexRay);
			this.InitControls();
		}

		private void InitControls()
		{
			this.ResetErrorProvider();
			this.SubscribeControlEvents(false);
			this.textBoxId.Text = GUIUtil.FlexraySlotIdToDisplayString(this.flexrayIdEvent.LowId.Value);
			this.textBoxIdLast.Text = GUIUtil.FlexraySlotIdToDisplayString(this.flexrayIdEvent.HighId.Value);
			this.textBoxCycleTime.Text = this.flexrayIdEvent.BaseCycle.Value.ToString();
			this.InitCycleRepetitionComboBox();
			if (this.comboBoxCycleRepetition.Items.Contains(this.flexrayIdEvent.CycleRepetition.Value.ToString()))
			{
				this.comboBoxCycleRepetition.SelectedItem = this.flexrayIdEvent.CycleRepetition.Value.ToString();
			}
			string text = GUIUtil.MapTriggerConditionRelation2String(this.flexrayIdEvent.IdRelation.Value);
			if (this.comboBoxConditionType.Items.Contains(text))
			{
				this.comboBoxConditionType.SelectedItem = text;
			}
			this.EnableControlsAndSetValuesForRelation();
			this.comboBoxChannel.SelectedItem = GUIUtil.MapFlexrayChannelNumber2String(this.flexrayIdEvent.ChannelNumber.Value);
			this.ValidateGlobalModelErrors();
			this.SubscribeControlEvents(true);
		}

		private void InitRelationComboBox()
		{
			this.comboBoxConditionType.Items.Clear();
			foreach (CondRelation condRelation in Enum.GetValues(typeof(CondRelation)))
			{
				if (condRelation != CondRelation.OnChange)
				{
					this.comboBoxConditionType.Items.Add(GUIUtil.MapTriggerConditionRelation2String(condRelation));
				}
			}
			if (this.comboBoxConditionType.Items.Count > 0)
			{
				this.comboBoxConditionType.SelectedIndex = 0;
			}
		}

		private void InitCycleRepetitionComboBox()
		{
			IList<uint> flexrayCycleRepetitionValues = GUIUtil.GetFlexrayCycleRepetitionValues(false);
			string text = "";
			if (this.comboBoxCycleRepetition.SelectedItem != null)
			{
				text = this.comboBoxCycleRepetition.SelectedItem.ToString();
			}
			this.comboBoxCycleRepetition.Items.Clear();
			foreach (uint current in flexrayCycleRepetitionValues)
			{
				this.comboBoxCycleRepetition.Items.Add(current.ToString());
			}
			if (!string.IsNullOrEmpty(text) && this.comboBoxCycleRepetition.Items.Contains(text))
			{
				this.comboBoxCycleRepetition.SelectedItem = text;
				return;
			}
			this.comboBoxCycleRepetition.SelectedIndex = 0;
		}

		private void InitChannelComboBox()
		{
			this.comboBoxChannel.Items.Clear();
			uint totalNumberOfLogicalChannels = this.modelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_FlexRay);
			for (uint num = 1u; num <= totalNumberOfLogicalChannels; num += 1u)
			{
				this.comboBoxChannel.Items.Add(GUIUtil.MapFlexrayChannelNumber2String(num));
			}
			if (this.flexrayIdEvent.ChannelNumber.Value <= totalNumberOfLogicalChannels)
			{
				this.comboBoxChannel.SelectedItem = GUIUtil.MapFlexrayChannelNumber2String(this.flexrayIdEvent.ChannelNumber.Value);
				return;
			}
			this.comboBoxChannel.SelectedIndex = 0;
		}

		private void FlexrayIdCondition_Shown(object sender, EventArgs e)
		{
			this.InitChannelComboBox();
			this.InitControls();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (!this.ValidateChildren())
			{
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				base.DialogResult = DialogResult.None;
				return;
			}
			if (this.HasGlobalModelErrors() && InformMessageBox.Question(Resources.InconsistSettingsCommitCondAnyway) == DialogResult.No)
			{
				base.DialogResult = DialogResult.None;
				return;
			}
			if ((CondRelation.InRange == this.flexrayIdEvent.IdRelation.Value || CondRelation.NotInRange == this.flexrayIdEvent.IdRelation.Value) && this.flexrayIdEvent.LowId.Value > this.flexrayIdEvent.HighId.Value)
			{
				uint value = this.flexrayIdEvent.HighId.Value;
				this.flexrayIdEvent.HighId.Value = this.flexrayIdEvent.LowId.Value;
				this.flexrayIdEvent.LowId.Value = value;
			}
			base.DialogResult = DialogResult.OK;
		}

		private void comboBoxConditionType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.flexrayIdEvent.IdRelation.Value = GUIUtil.MapString2TriggerConditionRelation(this.comboBoxConditionType.SelectedItem.ToString());
			this.EnableControlsAndSetValuesForRelation();
			this.ValidateChildren();
		}

		private void comboBoxCycleRepetition_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.flexrayIdEvent.CycleRepetition.Value = uint.Parse(this.comboBoxCycleRepetition.SelectedItem.ToString());
		}

		private void textBoxId_Validating(object sender, CancelEventArgs e)
		{
			uint value;
			if (GUIUtil.DisplayStringToFlexraySlotId(this.textBoxId.Text, out value))
			{
				this.flexrayIdEvent.LowId.Value = value;
				if ((CondRelation.LessThan == this.flexrayIdEvent.IdRelation.Value && this.flexrayIdEvent.LowId.Value == Constants.MinimumFlexraySlotId) || (CondRelation.GreaterThan == this.flexrayIdEvent.IdRelation.Value && this.flexrayIdEvent.LowId.Value == Constants.MaximumFlexraySlotId))
				{
					this.errorProviderFormat.SetError(this.textBoxId, Resources.ErrorInvalidValueWithRelOp);
					e.Cancel = true;
					return;
				}
			}
			else
			{
				this.errorProviderFormat.SetError(this.textBoxId, "");
				this.errorProviderFormat.SetError(this.textBoxId, string.Format(Resources.ErrorFlexraySlotExpected, Constants.MinimumFlexraySlotId, Constants.MaximumFlexraySlotId));
				e.Cancel = true;
			}
		}

		private void textBoxId_Validated(object sender, EventArgs e)
		{
			this.errorProviderFormat.SetError(this.textBoxId, "");
		}

		private void textBoxIdLast_Validating(object sender, CancelEventArgs e)
		{
			uint value;
			if (GUIUtil.DisplayStringToFlexraySlotId(this.textBoxIdLast.Text, out value))
			{
				this.flexrayIdEvent.HighId.Value = value;
				return;
			}
			this.errorProviderFormat.SetError(this.textBoxIdLast, "");
			this.errorProviderFormat.SetError(this.textBoxIdLast, string.Format(Resources.ErrorFlexraySlotExpected, Constants.MinimumFlexraySlotId, Constants.MaximumFlexraySlotId));
			e.Cancel = true;
		}

		private void textBoxIdLast_Validated(object sender, EventArgs e)
		{
			this.errorProviderFormat.SetError(this.textBoxIdLast, "");
		}

		private void textBoxCycleTime_Validating(object sender, CancelEventArgs e)
		{
			uint num;
			if (!uint.TryParse(this.textBoxCycleTime.Text, out num))
			{
				this.errorProviderFormat.SetError(this.textBoxCycleTime, "");
				this.errorProviderFormat.SetError(this.textBoxCycleTime, Resources.ErrorNumberExpected);
				e.Cancel = true;
				return;
			}
			if (num > Constants.MaximumFlexrayBaseCycle)
			{
				this.errorProviderFormat.SetError(this.textBoxCycleTime, "");
				this.errorProviderFormat.SetError(this.textBoxCycleTime, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 0, Constants.MaximumFlexrayBaseCycle));
				e.Cancel = true;
				return;
			}
			this.flexrayIdEvent.BaseCycle.Value = num;
		}

		private void textBoxCycleTime_Validated(object sender, EventArgs e)
		{
			this.errorProviderFormat.SetError(this.textBoxCycleTime, "");
		}

		private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.flexrayIdEvent.ChannelNumber.Value = GUIUtil.MapFlexrayChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
			this.ValidateGlobalModelErrors();
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void FlexrayIdCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void EnableControlsAndSetValuesForRelation()
		{
			if (this.flexrayIdEvent.IdRelation.Value == CondRelation.Equal)
			{
				this.textBoxIdLast.Enabled = false;
				this.flexrayIdEvent.HighId.Value = Constants.MinimumFlexraySlotId;
				this.textBoxIdLast.Text = GUIUtil.FlexraySlotIdToDisplayString(this.flexrayIdEvent.HighId.Value);
				this.errorProviderFormat.SetError(this.textBoxIdLast, "");
				this.textBoxCycleTime.Enabled = true;
				this.comboBoxCycleRepetition.Enabled = true;
				return;
			}
			if (this.flexrayIdEvent.IdRelation.Value == CondRelation.InRange || this.flexrayIdEvent.IdRelation.Value == CondRelation.NotInRange)
			{
				this.textBoxIdLast.Enabled = true;
			}
			else
			{
				this.textBoxIdLast.Enabled = false;
			}
			this.textBoxCycleTime.Text = "0";
			this.textBoxCycleTime.Enabled = false;
			this.errorProviderFormat.SetError(this.textBoxCycleTime, "");
			this.comboBoxCycleRepetition.SelectedItem = "1";
			this.comboBoxCycleRepetition.Enabled = false;
		}

		private void ResetErrorProvider()
		{
			this.errorProviderFormat.SetError(this.textBoxId, "");
			this.errorProviderFormat.SetError(this.textBoxIdLast, "");
			this.errorProviderFormat.SetError(this.textBoxCycleTime, "");
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
				this.comboBoxCycleRepetition.SelectedIndexChanged += new EventHandler(this.comboBoxCycleRepetition_SelectedIndexChanged);
				this.textBoxId.Validating += new CancelEventHandler(this.textBoxId_Validating);
				this.textBoxId.Validated += new EventHandler(this.textBoxId_Validated);
				this.textBoxIdLast.Validating += new CancelEventHandler(this.textBoxIdLast_Validating);
				this.textBoxIdLast.Validated += new EventHandler(this.textBoxIdLast_Validated);
				this.textBoxCycleTime.Validating += new CancelEventHandler(this.textBoxCycleTime_Validating);
				this.textBoxCycleTime.Validated += new EventHandler(this.textBoxCycleTime_Validated);
				this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
				return;
			}
			this.comboBoxConditionType.SelectedIndexChanged -= new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			this.comboBoxCycleRepetition.SelectedIndexChanged -= new EventHandler(this.comboBoxCycleRepetition_SelectedIndexChanged);
			this.textBoxId.Validating -= new CancelEventHandler(this.textBoxId_Validating);
			this.textBoxId.Validated -= new EventHandler(this.textBoxId_Validated);
			this.textBoxIdLast.Validating -= new CancelEventHandler(this.textBoxIdLast_Validating);
			this.textBoxIdLast.Validated -= new EventHandler(this.textBoxIdLast_Validated);
			this.textBoxCycleTime.Validating -= new CancelEventHandler(this.textBoxCycleTime_Validating);
			this.textBoxCycleTime.Validated -= new EventHandler(this.textBoxCycleTime_Validated);
			this.comboBoxChannel.SelectedIndexChanged -= new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
		}

		private void ValidateGlobalModelErrors()
		{
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
			if (!this.modelValidator.IsHardwareChannelAvailable(BusType.Bt_FlexRay, this.flexrayIdEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelNotAvailable);
				return;
			}
			if (!this.modelValidator.IsHardwareChannelActive(BusType.Bt_FlexRay, this.flexrayIdEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelInactive);
			}
		}

		private bool HasGlobalModelErrors()
		{
			return !string.IsNullOrEmpty(this.errorProviderGlobalModel.GetError(this.comboBoxChannel));
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FlexrayIdCondition));
			this.buttonHelp = new Button();
			this.buttonCancel = new Button();
			this.buttonOK = new Button();
			this.labelRelation = new Label();
			this.comboBoxConditionType = new ComboBox();
			this.textBoxId = new TextBox();
			this.textBoxIdLast = new TextBox();
			this.labelSlotId = new Label();
			this.labelIdLast = new Label();
			this.textBoxCycleTime = new TextBox();
			this.labelCycleTime = new Label();
			this.labelCycleRepetition = new Label();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.comboBoxCycleRepetition = new ComboBox();
			this.comboBoxChannel = new ComboBox();
			this.labelChannel = new Label();
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.errorProviderFormat.SetError(this.buttonHelp, componentResourceManager.GetString("buttonHelp.Error"));
			this.errorProviderGlobalModel.SetError(this.buttonHelp, componentResourceManager.GetString("buttonHelp.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.buttonHelp, (ErrorIconAlignment)componentResourceManager.GetObject("buttonHelp.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonHelp, (ErrorIconAlignment)componentResourceManager.GetObject("buttonHelp.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.buttonHelp, (int)componentResourceManager.GetObject("buttonHelp.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonHelp, (int)componentResourceManager.GetObject("buttonHelp.IconPadding1"));
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.errorProviderFormat.SetError(this.buttonCancel, componentResourceManager.GetString("buttonCancel.Error"));
			this.errorProviderGlobalModel.SetError(this.buttonCancel, componentResourceManager.GetString("buttonCancel.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.buttonCancel, (ErrorIconAlignment)componentResourceManager.GetObject("buttonCancel.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonCancel, (ErrorIconAlignment)componentResourceManager.GetObject("buttonCancel.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.buttonCancel, (int)componentResourceManager.GetObject("buttonCancel.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonCancel, (int)componentResourceManager.GetObject("buttonCancel.IconPadding1"));
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.errorProviderFormat.SetError(this.buttonOK, componentResourceManager.GetString("buttonOK.Error"));
			this.errorProviderGlobalModel.SetError(this.buttonOK, componentResourceManager.GetString("buttonOK.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.buttonOK, (ErrorIconAlignment)componentResourceManager.GetObject("buttonOK.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonOK, (ErrorIconAlignment)componentResourceManager.GetObject("buttonOK.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.buttonOK, (int)componentResourceManager.GetObject("buttonOK.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonOK, (int)componentResourceManager.GetObject("buttonOK.IconPadding1"));
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this.labelRelation, "labelRelation");
			this.errorProviderFormat.SetError(this.labelRelation, componentResourceManager.GetString("labelRelation.Error"));
			this.errorProviderGlobalModel.SetError(this.labelRelation, componentResourceManager.GetString("labelRelation.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelRelation, (ErrorIconAlignment)componentResourceManager.GetObject("labelRelation.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelRelation, (ErrorIconAlignment)componentResourceManager.GetObject("labelRelation.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelRelation, (int)componentResourceManager.GetObject("labelRelation.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelRelation, (int)componentResourceManager.GetObject("labelRelation.IconPadding1"));
			this.labelRelation.Name = "labelRelation";
			componentResourceManager.ApplyResources(this.comboBoxConditionType, "comboBoxConditionType");
			this.comboBoxConditionType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderGlobalModel.SetError(this.comboBoxConditionType, componentResourceManager.GetString("comboBoxConditionType.Error"));
			this.errorProviderFormat.SetError(this.comboBoxConditionType, componentResourceManager.GetString("comboBoxConditionType.Error1"));
			this.comboBoxConditionType.FormattingEnabled = true;
			this.errorProviderFormat.SetIconAlignment(this.comboBoxConditionType, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxConditionType.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxConditionType, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxConditionType.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxConditionType, (int)componentResourceManager.GetObject("comboBoxConditionType.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxConditionType, (int)componentResourceManager.GetObject("comboBoxConditionType.IconPadding1"));
			this.comboBoxConditionType.Name = "comboBoxConditionType";
			this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.textBoxId, "textBoxId");
			this.errorProviderGlobalModel.SetError(this.textBoxId, componentResourceManager.GetString("textBoxId.Error"));
			this.errorProviderFormat.SetError(this.textBoxId, componentResourceManager.GetString("textBoxId.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxId.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxId.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxId, (int)componentResourceManager.GetObject("textBoxId.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxId, (int)componentResourceManager.GetObject("textBoxId.IconPadding1"));
			this.textBoxId.Name = "textBoxId";
			this.textBoxId.Validating += new CancelEventHandler(this.textBoxId_Validating);
			this.textBoxId.Validated += new EventHandler(this.textBoxId_Validated);
			componentResourceManager.ApplyResources(this.textBoxIdLast, "textBoxIdLast");
			this.errorProviderGlobalModel.SetError(this.textBoxIdLast, componentResourceManager.GetString("textBoxIdLast.Error"));
			this.errorProviderFormat.SetError(this.textBoxIdLast, componentResourceManager.GetString("textBoxIdLast.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxIdLast, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxIdLast.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxIdLast, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxIdLast.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxIdLast, (int)componentResourceManager.GetObject("textBoxIdLast.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxIdLast, (int)componentResourceManager.GetObject("textBoxIdLast.IconPadding1"));
			this.textBoxIdLast.Name = "textBoxIdLast";
			this.textBoxIdLast.Validating += new CancelEventHandler(this.textBoxIdLast_Validating);
			this.textBoxIdLast.Validated += new EventHandler(this.textBoxIdLast_Validated);
			componentResourceManager.ApplyResources(this.labelSlotId, "labelSlotId");
			this.errorProviderFormat.SetError(this.labelSlotId, componentResourceManager.GetString("labelSlotId.Error"));
			this.errorProviderGlobalModel.SetError(this.labelSlotId, componentResourceManager.GetString("labelSlotId.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelSlotId, (ErrorIconAlignment)componentResourceManager.GetObject("labelSlotId.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelSlotId, (ErrorIconAlignment)componentResourceManager.GetObject("labelSlotId.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelSlotId, (int)componentResourceManager.GetObject("labelSlotId.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelSlotId, (int)componentResourceManager.GetObject("labelSlotId.IconPadding1"));
			this.labelSlotId.Name = "labelSlotId";
			componentResourceManager.ApplyResources(this.labelIdLast, "labelIdLast");
			this.errorProviderFormat.SetError(this.labelIdLast, componentResourceManager.GetString("labelIdLast.Error"));
			this.errorProviderGlobalModel.SetError(this.labelIdLast, componentResourceManager.GetString("labelIdLast.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelIdLast, (ErrorIconAlignment)componentResourceManager.GetObject("labelIdLast.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelIdLast, (ErrorIconAlignment)componentResourceManager.GetObject("labelIdLast.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelIdLast, (int)componentResourceManager.GetObject("labelIdLast.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelIdLast, (int)componentResourceManager.GetObject("labelIdLast.IconPadding1"));
			this.labelIdLast.Name = "labelIdLast";
			componentResourceManager.ApplyResources(this.textBoxCycleTime, "textBoxCycleTime");
			this.errorProviderGlobalModel.SetError(this.textBoxCycleTime, componentResourceManager.GetString("textBoxCycleTime.Error"));
			this.errorProviderFormat.SetError(this.textBoxCycleTime, componentResourceManager.GetString("textBoxCycleTime.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCycleTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCycleTime.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCycleTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCycleTime.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCycleTime, (int)componentResourceManager.GetObject("textBoxCycleTime.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCycleTime, (int)componentResourceManager.GetObject("textBoxCycleTime.IconPadding1"));
			this.textBoxCycleTime.Name = "textBoxCycleTime";
			this.textBoxCycleTime.Validating += new CancelEventHandler(this.textBoxCycleTime_Validating);
			this.textBoxCycleTime.Validated += new EventHandler(this.textBoxCycleTime_Validated);
			componentResourceManager.ApplyResources(this.labelCycleTime, "labelCycleTime");
			this.errorProviderFormat.SetError(this.labelCycleTime, componentResourceManager.GetString("labelCycleTime.Error"));
			this.errorProviderGlobalModel.SetError(this.labelCycleTime, componentResourceManager.GetString("labelCycleTime.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelCycleTime, (ErrorIconAlignment)componentResourceManager.GetObject("labelCycleTime.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelCycleTime, (ErrorIconAlignment)componentResourceManager.GetObject("labelCycleTime.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelCycleTime, (int)componentResourceManager.GetObject("labelCycleTime.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelCycleTime, (int)componentResourceManager.GetObject("labelCycleTime.IconPadding1"));
			this.labelCycleTime.Name = "labelCycleTime";
			componentResourceManager.ApplyResources(this.labelCycleRepetition, "labelCycleRepetition");
			this.errorProviderFormat.SetError(this.labelCycleRepetition, componentResourceManager.GetString("labelCycleRepetition.Error"));
			this.errorProviderGlobalModel.SetError(this.labelCycleRepetition, componentResourceManager.GetString("labelCycleRepetition.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelCycleRepetition, (ErrorIconAlignment)componentResourceManager.GetObject("labelCycleRepetition.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelCycleRepetition, (ErrorIconAlignment)componentResourceManager.GetObject("labelCycleRepetition.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelCycleRepetition, (int)componentResourceManager.GetObject("labelCycleRepetition.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelCycleRepetition, (int)componentResourceManager.GetObject("labelCycleRepetition.IconPadding1"));
			this.labelCycleRepetition.Name = "labelCycleRepetition";
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderFormat, "errorProviderFormat");
			componentResourceManager.ApplyResources(this.comboBoxCycleRepetition, "comboBoxCycleRepetition");
			this.comboBoxCycleRepetition.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderGlobalModel.SetError(this.comboBoxCycleRepetition, componentResourceManager.GetString("comboBoxCycleRepetition.Error"));
			this.errorProviderFormat.SetError(this.comboBoxCycleRepetition, componentResourceManager.GetString("comboBoxCycleRepetition.Error1"));
			this.comboBoxCycleRepetition.FormattingEnabled = true;
			this.errorProviderFormat.SetIconAlignment(this.comboBoxCycleRepetition, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxCycleRepetition.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxCycleRepetition, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxCycleRepetition.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxCycleRepetition, (int)componentResourceManager.GetObject("comboBoxCycleRepetition.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxCycleRepetition, (int)componentResourceManager.GetObject("comboBoxCycleRepetition.IconPadding1"));
			this.comboBoxCycleRepetition.Name = "comboBoxCycleRepetition";
			this.comboBoxCycleRepetition.SelectedIndexChanged += new EventHandler(this.comboBoxCycleRepetition_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.comboBoxChannel, "comboBoxChannel");
			this.comboBoxChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, componentResourceManager.GetString("comboBoxChannel.Error"));
			this.errorProviderFormat.SetError(this.comboBoxChannel, componentResourceManager.GetString("comboBoxChannel.Error1"));
			this.comboBoxChannel.FormattingEnabled = true;
			this.errorProviderFormat.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxChannel, (int)componentResourceManager.GetObject("comboBoxChannel.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxChannel, (int)componentResourceManager.GetObject("comboBoxChannel.IconPadding1"));
			this.comboBoxChannel.Name = "comboBoxChannel";
			this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelChannel, "labelChannel");
			this.errorProviderFormat.SetError(this.labelChannel, componentResourceManager.GetString("labelChannel.Error"));
			this.errorProviderGlobalModel.SetError(this.labelChannel, componentResourceManager.GetString("labelChannel.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelChannel, (ErrorIconAlignment)componentResourceManager.GetObject("labelChannel.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelChannel, (ErrorIconAlignment)componentResourceManager.GetObject("labelChannel.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelChannel, (int)componentResourceManager.GetObject("labelChannel.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelChannel, (int)componentResourceManager.GetObject("labelChannel.IconPadding1"));
			this.labelChannel.Name = "labelChannel";
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoValidate = AutoValidate.EnableAllowFocusChange;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.comboBoxChannel);
			base.Controls.Add(this.labelChannel);
			base.Controls.Add(this.comboBoxCycleRepetition);
			base.Controls.Add(this.labelCycleRepetition);
			base.Controls.Add(this.labelCycleTime);
			base.Controls.Add(this.textBoxCycleTime);
			base.Controls.Add(this.labelIdLast);
			base.Controls.Add(this.labelSlotId);
			base.Controls.Add(this.textBoxIdLast);
			base.Controls.Add(this.textBoxId);
			base.Controls.Add(this.comboBoxConditionType);
			base.Controls.Add(this.labelRelation);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonHelp);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "FlexrayIdCondition";
			base.Shown += new EventHandler(this.FlexrayIdCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.FlexrayIdCondition_HelpRequested);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
