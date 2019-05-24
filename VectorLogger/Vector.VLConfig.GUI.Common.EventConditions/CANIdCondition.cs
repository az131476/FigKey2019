using System;
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
	internal class CANIdCondition : Form
	{
		private CANIdEvent canIdEvent;

		private IModelValidator modelValidator;

		private IContainer components;

		private Label labelConditionType;

		private ComboBox comboBoxConditionType;

		private Label labelID;

		private TextBox textBoxCANId;

		private TextBox textBoxCANIdLast;

		private Label labelLastCANId;

		private Label labelDescription;

		private Button buttonOk;

		private Button buttonCancel;

		private ErrorProvider errorProviderFormat;

		private CheckBox checkBoxIsExtendedId;

		private Button buttonHelp;

		private Label labelChannel;

		private ComboBox comboBoxChannel;

		private ErrorProvider errorProviderGlobalModel;

		public CANIdEvent CANIdEvent
		{
			get
			{
				return this.canIdEvent;
			}
			set
			{
				this.canIdEvent = value;
			}
		}

		public CANIdCondition(IModelValidator modelVal)
		{
			this.InitializeComponent();
			this.modelValidator = modelVal;
			this.canIdEvent = new CANIdEvent();
			this.InitRelationComboBox();
		}

		public void ResetToDefaults()
		{
			this.canIdEvent.LowId.Value = 0u;
			this.canIdEvent.HighId.Value = 0u;
			this.canIdEvent.IsExtendedId.Value = false;
			this.canIdEvent.IdRelation.Value = CondRelation.Equal;
			this.canIdEvent.ChannelNumber.Value = this.modelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_CAN);
			this.InitControls();
		}

		private void InitControls()
		{
			this.ResetErrorProvider();
			this.SubscribeControlEvents(false);
			this.comboBoxConditionType.SelectedItem = GUIUtil.MapTriggerConditionRelation2String(this.canIdEvent.IdRelation.Value);
			this.checkBoxIsExtendedId.Checked = this.canIdEvent.IsExtendedId.Value;
			this.textBoxCANId.Text = GUIUtil.CANIdToDisplayString(this.canIdEvent.LowId.Value, this.canIdEvent.IsExtendedId.Value);
			this.textBoxCANIdLast.Text = GUIUtil.CANIdToDisplayString(this.canIdEvent.HighId.Value, this.canIdEvent.IsExtendedId.Value);
			if (CondRelation.InRange == this.canIdEvent.IdRelation.Value || CondRelation.NotInRange == this.canIdEvent.IdRelation.Value)
			{
				this.textBoxCANIdLast.Enabled = true;
			}
			else
			{
				this.textBoxCANIdLast.Enabled = false;
			}
			this.comboBoxChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.canIdEvent.ChannelNumber.Value);
			this.ValidateGlobalModelErrors();
			this.SubscribeControlEvents(true);
		}

		private void InitRelationComboBox()
		{
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

		private void InitChannelComboBox()
		{
			this.comboBoxChannel.Items.Clear();
			uint totalNumberOfLogicalChannels = this.modelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN);
			for (uint num = 1u; num <= totalNumberOfLogicalChannels; num += 1u)
			{
				this.comboBoxChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
			if (this.canIdEvent.ChannelNumber.Value <= totalNumberOfLogicalChannels)
			{
				this.comboBoxChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.canIdEvent.ChannelNumber.Value);
				return;
			}
			this.comboBoxChannel.SelectedIndex = 0;
		}

		private void ResetErrorProvider()
		{
			this.errorProviderFormat.SetError(this.textBoxCANId, "");
			this.errorProviderFormat.SetError(this.textBoxCANIdLast, "");
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
		}

		private void CANIdCondition_Shown(object sender, EventArgs e)
		{
			this.InitChannelComboBox();
			this.RenderLabelsForDisplayMode();
			this.InitControls();
		}

		private void comboBoxConditionType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.canIdEvent.IdRelation.Value = GUIUtil.MapString2TriggerConditionRelation(this.comboBoxConditionType.SelectedItem.ToString());
			if (this.canIdEvent.IdRelation.Value != CondRelation.InRange && this.canIdEvent.IdRelation.Value != CondRelation.NotInRange)
			{
				this.textBoxCANIdLast.Enabled = false;
				this.canIdEvent.HighId.Value = 0u;
				this.textBoxCANIdLast.Text = GUIUtil.CANIdToDisplayString(this.canIdEvent.HighId.Value, this.canIdEvent.IsExtendedId.Value);
				this.errorProviderFormat.SetError(this.textBoxCANIdLast, "");
			}
			else
			{
				this.textBoxCANIdLast.Enabled = true;
			}
			this.ValidateChildren();
		}

		private void checkBoxIsExtendedId_CheckedChanged(object sender, EventArgs e)
		{
			if (this.checkBoxIsExtendedId.Checked)
			{
				if (!this.HasError(this.textBoxCANId))
				{
					this.textBoxCANId.Text = string.Format(Resources.ExtendedCANIdFormat, this.textBoxCANId.Text);
				}
				if (!this.HasError(this.textBoxCANIdLast))
				{
					this.textBoxCANIdLast.Text = string.Format(Resources.ExtendedCANIdFormat, this.textBoxCANIdLast.Text);
				}
				this.canIdEvent.IsExtendedId.Value = true;
			}
			else
			{
				if (!this.HasError(this.textBoxCANId))
				{
					string text = this.textBoxCANId.Text;
					if (text.Length > 0 && text.Substring(text.Length - 1).ToLower() == "x")
					{
						this.textBoxCANId.Text = text.Substring(0, text.Length - 1);
					}
				}
				if (!this.HasError(this.textBoxCANIdLast))
				{
					string text2 = this.textBoxCANIdLast.Text;
					if (text2.Length > 0 && text2.Substring(text2.Length - 1).ToLower() == "x")
					{
						this.textBoxCANIdLast.Text = text2.Substring(0, text2.Length - 1);
					}
				}
				this.canIdEvent.IsExtendedId.Value = false;
			}
			this.ValidateChildren();
		}

		private void textBoxCANId_Validating(object sender, CancelEventArgs e)
		{
			uint value;
			bool flag;
			if (GUIUtil.DisplayStringToCANId(this.textBoxCANId.Text, out value, out flag))
			{
				if (this.canIdEvent.IsExtendedId.Value != flag)
				{
					this.errorProviderFormat.SetError(this.textBoxCANId, "");
					if (this.canIdEvent.IsExtendedId.Value)
					{
						this.errorProviderFormat.SetError(this.textBoxCANId, Resources.ErrorExtendedCANIdExpected);
					}
					else
					{
						this.errorProviderFormat.SetError(this.textBoxCANId, Resources.ErrorStandardCANIdExpected);
					}
					e.Cancel = true;
					return;
				}
				this.canIdEvent.LowId.Value = value;
				uint num = Constants.MaximumStandardCANId;
				if (flag)
				{
					num = Constants.MaximumExtendedCANId;
				}
				if ((CondRelation.LessThan == this.canIdEvent.IdRelation.Value && this.canIdEvent.LowId.Value == 0u) || (CondRelation.GreaterThan == this.canIdEvent.IdRelation.Value && this.canIdEvent.LowId.Value == num))
				{
					this.errorProviderFormat.SetError(this.textBoxCANId, Resources.ErrorInvalidValueWithRelOp);
					e.Cancel = true;
					return;
				}
			}
			else
			{
				this.errorProviderFormat.SetError(this.textBoxCANId, "");
				if (this.canIdEvent.IsExtendedId.Value)
				{
					this.errorProviderFormat.SetError(this.textBoxCANId, Resources.ErrorExtendedCANIdExpected);
				}
				else
				{
					this.errorProviderFormat.SetError(this.textBoxCANId, Resources.ErrorStandardCANIdExpected);
				}
				e.Cancel = true;
			}
		}

		private void textBoxCANId_Validated(object sender, EventArgs e)
		{
			this.errorProviderFormat.SetError(this.textBoxCANId, "");
		}

		private void textBoxCANIdLast_Validating(object sender, CancelEventArgs e)
		{
			uint value;
			bool flag;
			if (!GUIUtil.DisplayStringToCANId(this.textBoxCANIdLast.Text, out value, out flag))
			{
				this.errorProviderFormat.SetError(this.textBoxCANIdLast, "");
				if (this.canIdEvent.IsExtendedId.Value)
				{
					this.errorProviderFormat.SetError(this.textBoxCANIdLast, Resources.ErrorExtendedCANIdExpected);
				}
				else
				{
					this.errorProviderFormat.SetError(this.textBoxCANIdLast, Resources.ErrorStandardCANIdExpected);
				}
				e.Cancel = true;
				return;
			}
			if (this.canIdEvent.IsExtendedId.Value == flag)
			{
				this.canIdEvent.HighId.Value = value;
				return;
			}
			this.errorProviderFormat.SetError(this.textBoxCANIdLast, "");
			if (this.canIdEvent.IsExtendedId.Value)
			{
				this.errorProviderFormat.SetError(this.textBoxCANIdLast, Resources.ErrorExtendedCANIdExpected);
			}
			else
			{
				this.errorProviderFormat.SetError(this.textBoxCANIdLast, Resources.ErrorStandardCANIdExpected);
			}
			e.Cancel = true;
		}

		private void textBoxCANIdLast_Validated(object sender, EventArgs e)
		{
			this.errorProviderFormat.SetError(this.textBoxCANIdLast, "");
		}

		private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.canIdEvent.ChannelNumber.Value = GUIUtil.MapCANChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
			this.ValidateGlobalModelErrors();
		}

		private void buttonOk_Click(object sender, EventArgs e)
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
			if ((CondRelation.InRange == this.canIdEvent.IdRelation.Value || CondRelation.NotInRange == this.canIdEvent.IdRelation.Value) && this.canIdEvent.LowId.Value > this.canIdEvent.HighId.Value)
			{
				uint value = this.canIdEvent.HighId.Value;
				this.canIdEvent.HighId.Value = this.canIdEvent.LowId.Value;
				this.canIdEvent.LowId.Value = value;
			}
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void CANIdCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
				this.checkBoxIsExtendedId.CheckedChanged += new EventHandler(this.checkBoxIsExtendedId_CheckedChanged);
				this.textBoxCANId.Validating += new CancelEventHandler(this.textBoxCANId_Validating);
				this.textBoxCANId.Validated += new EventHandler(this.textBoxCANId_Validated);
				this.textBoxCANIdLast.Validating += new CancelEventHandler(this.textBoxCANIdLast_Validating);
				this.textBoxCANIdLast.Validated += new EventHandler(this.textBoxCANIdLast_Validated);
				this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
				return;
			}
			this.comboBoxConditionType.SelectedIndexChanged -= new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			this.checkBoxIsExtendedId.CheckedChanged -= new EventHandler(this.checkBoxIsExtendedId_CheckedChanged);
			this.textBoxCANId.Validating -= new CancelEventHandler(this.textBoxCANId_Validating);
			this.textBoxCANId.Validated -= new EventHandler(this.textBoxCANId_Validated);
			this.textBoxCANIdLast.Validating -= new CancelEventHandler(this.textBoxCANIdLast_Validating);
			this.textBoxCANIdLast.Validated -= new EventHandler(this.textBoxCANIdLast_Validated);
			this.comboBoxChannel.SelectedIndexChanged -= new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
		}

		private bool HasError(Control control)
		{
			return !string.IsNullOrEmpty(this.errorProviderFormat.GetError(control));
		}

		private void RenderLabelsForDisplayMode()
		{
			string arg = Resources.DisplayModeDec;
			if (GUIUtil.IsHexadecimal)
			{
				arg = Resources.DisplayModeHex;
			}
			this.labelID.Text = string.Format(Resources.IDLabelWithMode, arg);
			this.labelLastCANId.Text = string.Format(Resources.LastIDLabelWithMode, arg);
		}

		private void ValidateGlobalModelErrors()
		{
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
			if (!this.modelValidator.IsHardwareChannelAvailable(BusType.Bt_CAN, this.canIdEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelNotAvailable);
				return;
			}
			if (!this.modelValidator.IsHardwareChannelActive(BusType.Bt_CAN, this.canIdEvent.ChannelNumber.Value))
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CANIdCondition));
			this.labelConditionType = new Label();
			this.comboBoxConditionType = new ComboBox();
			this.labelID = new Label();
			this.textBoxCANId = new TextBox();
			this.textBoxCANIdLast = new TextBox();
			this.labelLastCANId = new Label();
			this.labelDescription = new Label();
			this.buttonOk = new Button();
			this.buttonCancel = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.comboBoxChannel = new ComboBox();
			this.checkBoxIsExtendedId = new CheckBox();
			this.buttonHelp = new Button();
			this.labelChannel = new Label();
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelConditionType, "labelConditionType");
			this.errorProviderFormat.SetError(this.labelConditionType, componentResourceManager.GetString("labelConditionType.Error"));
			this.errorProviderGlobalModel.SetError(this.labelConditionType, componentResourceManager.GetString("labelConditionType.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelConditionType, (ErrorIconAlignment)componentResourceManager.GetObject("labelConditionType.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelConditionType, (ErrorIconAlignment)componentResourceManager.GetObject("labelConditionType.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelConditionType, (int)componentResourceManager.GetObject("labelConditionType.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelConditionType, (int)componentResourceManager.GetObject("labelConditionType.IconPadding1"));
			this.labelConditionType.Name = "labelConditionType";
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
			componentResourceManager.ApplyResources(this.labelID, "labelID");
			this.errorProviderFormat.SetError(this.labelID, componentResourceManager.GetString("labelID.Error"));
			this.errorProviderGlobalModel.SetError(this.labelID, componentResourceManager.GetString("labelID.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelID, (ErrorIconAlignment)componentResourceManager.GetObject("labelID.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelID, (ErrorIconAlignment)componentResourceManager.GetObject("labelID.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelID, (int)componentResourceManager.GetObject("labelID.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelID, (int)componentResourceManager.GetObject("labelID.IconPadding1"));
			this.labelID.Name = "labelID";
			componentResourceManager.ApplyResources(this.textBoxCANId, "textBoxCANId");
			this.errorProviderGlobalModel.SetError(this.textBoxCANId, componentResourceManager.GetString("textBoxCANId.Error"));
			this.errorProviderFormat.SetError(this.textBoxCANId, componentResourceManager.GetString("textBoxCANId.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCANId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCANId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCANId, (int)componentResourceManager.GetObject("textBoxCANId.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCANId, (int)componentResourceManager.GetObject("textBoxCANId.IconPadding1"));
			this.textBoxCANId.Name = "textBoxCANId";
			this.textBoxCANId.Validating += new CancelEventHandler(this.textBoxCANId_Validating);
			this.textBoxCANId.Validated += new EventHandler(this.textBoxCANId_Validated);
			componentResourceManager.ApplyResources(this.textBoxCANIdLast, "textBoxCANIdLast");
			this.errorProviderGlobalModel.SetError(this.textBoxCANIdLast, componentResourceManager.GetString("textBoxCANIdLast.Error"));
			this.errorProviderFormat.SetError(this.textBoxCANIdLast, componentResourceManager.GetString("textBoxCANIdLast.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCANIdLast, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANIdLast.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCANIdLast, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANIdLast.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCANIdLast, (int)componentResourceManager.GetObject("textBoxCANIdLast.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCANIdLast, (int)componentResourceManager.GetObject("textBoxCANIdLast.IconPadding1"));
			this.textBoxCANIdLast.Name = "textBoxCANIdLast";
			this.textBoxCANIdLast.Validating += new CancelEventHandler(this.textBoxCANIdLast_Validating);
			this.textBoxCANIdLast.Validated += new EventHandler(this.textBoxCANIdLast_Validated);
			componentResourceManager.ApplyResources(this.labelLastCANId, "labelLastCANId");
			this.errorProviderFormat.SetError(this.labelLastCANId, componentResourceManager.GetString("labelLastCANId.Error"));
			this.errorProviderGlobalModel.SetError(this.labelLastCANId, componentResourceManager.GetString("labelLastCANId.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelLastCANId, (ErrorIconAlignment)componentResourceManager.GetObject("labelLastCANId.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelLastCANId, (ErrorIconAlignment)componentResourceManager.GetObject("labelLastCANId.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelLastCANId, (int)componentResourceManager.GetObject("labelLastCANId.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelLastCANId, (int)componentResourceManager.GetObject("labelLastCANId.IconPadding1"));
			this.labelLastCANId.Name = "labelLastCANId";
			componentResourceManager.ApplyResources(this.labelDescription, "labelDescription");
			this.errorProviderFormat.SetError(this.labelDescription, componentResourceManager.GetString("labelDescription.Error"));
			this.errorProviderGlobalModel.SetError(this.labelDescription, componentResourceManager.GetString("labelDescription.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelDescription, (ErrorIconAlignment)componentResourceManager.GetObject("labelDescription.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelDescription, (ErrorIconAlignment)componentResourceManager.GetObject("labelDescription.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelDescription, (int)componentResourceManager.GetObject("labelDescription.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelDescription, (int)componentResourceManager.GetObject("labelDescription.IconPadding1"));
			this.labelDescription.Name = "labelDescription";
			componentResourceManager.ApplyResources(this.buttonOk, "buttonOk");
			this.buttonOk.DialogResult = DialogResult.OK;
			this.errorProviderFormat.SetError(this.buttonOk, componentResourceManager.GetString("buttonOk.Error"));
			this.errorProviderGlobalModel.SetError(this.buttonOk, componentResourceManager.GetString("buttonOk.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.buttonOk, (ErrorIconAlignment)componentResourceManager.GetObject("buttonOk.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonOk, (ErrorIconAlignment)componentResourceManager.GetObject("buttonOk.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.buttonOk, (int)componentResourceManager.GetObject("buttonOk.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonOk, (int)componentResourceManager.GetObject("buttonOk.IconPadding1"));
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new EventHandler(this.buttonOk_Click);
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
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderFormat, "errorProviderFormat");
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
			componentResourceManager.ApplyResources(this.checkBoxIsExtendedId, "checkBoxIsExtendedId");
			this.checkBoxIsExtendedId.Checked = true;
			this.checkBoxIsExtendedId.CheckState = CheckState.Checked;
			this.errorProviderFormat.SetError(this.checkBoxIsExtendedId, componentResourceManager.GetString("checkBoxIsExtendedId.Error"));
			this.errorProviderGlobalModel.SetError(this.checkBoxIsExtendedId, componentResourceManager.GetString("checkBoxIsExtendedId.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxIsExtendedId, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIsExtendedId.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxIsExtendedId, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIsExtendedId.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxIsExtendedId, (int)componentResourceManager.GetObject("checkBoxIsExtendedId.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxIsExtendedId, (int)componentResourceManager.GetObject("checkBoxIsExtendedId.IconPadding1"));
			this.checkBoxIsExtendedId.Name = "checkBoxIsExtendedId";
			this.checkBoxIsExtendedId.UseVisualStyleBackColor = true;
			this.checkBoxIsExtendedId.CheckedChanged += new EventHandler(this.checkBoxIsExtendedId_CheckedChanged);
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
			base.AcceptButton = this.buttonOk;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			this.AutoValidate = AutoValidate.EnableAllowFocusChange;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.labelChannel);
			base.Controls.Add(this.comboBoxChannel);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.checkBoxIsExtendedId);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOk);
			base.Controls.Add(this.labelDescription);
			base.Controls.Add(this.labelLastCANId);
			base.Controls.Add(this.textBoxCANIdLast);
			base.Controls.Add(this.textBoxCANId);
			base.Controls.Add(this.labelID);
			base.Controls.Add(this.comboBoxConditionType);
			base.Controls.Add(this.labelConditionType);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CANIdCondition";
			base.SizeGripStyle = SizeGripStyle.Hide;
			base.Shown += new EventHandler(this.CANIdCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.CANIdCondition_HelpRequested);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
