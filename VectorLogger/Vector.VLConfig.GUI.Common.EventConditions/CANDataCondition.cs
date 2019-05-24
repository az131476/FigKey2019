using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	internal class CANDataCondition : Form
	{
		private CANDataEvent canDataEvent;

		private RawDataSignalByte tempRawDataSigByte;

		private RawDataSignalStartbitLength tempRawDataSigStartbitLen;

		private IModelValidator modelValidator;

		private List<CondRelation> mRelationsToHide;

		private IContainer components;

		private Label labelMsgId;

		private TextBox textBoxMessageId;

		private CheckBox checkBoxIsExtended;

		private Label labelRelOperator;

		private ComboBox comboBoxRelOperator;

		private Label labelValue;

		private TextBox textBoxValue;

		private Label labelUpperLimit;

		private TextBox textBoxUpperValue;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonHelp;

		private ErrorProvider errorProviderFormat;

		private TextBox textBoxLength;

		private TextBox textBoxStartbit;

		private RadioButton radioButtonRawSigStartbit;

		private ComboBox comboBoxByteOrder;

		private Label labelNoteExtId;

		private RadioButton radioButtonRawSigByte;

		private Label labelByteOrder;

		private Label labelLength;

		private GroupBox groupBoxMessage;

		private GroupBox groupBoxData;

		private ComboBox comboBoxChannel;

		private Label labelChannel;

		private ErrorProvider errorProviderGlobalModel;

		private TextBox textBoxDataByte;

		public CANDataEvent CANDataEvent
		{
			get
			{
				return this.canDataEvent;
			}
			set
			{
				this.canDataEvent = value;
				if (this.canDataEvent.RawDataSignal is RawDataSignalByte)
				{
					this.tempRawDataSigByte = (this.canDataEvent.RawDataSignal as RawDataSignalByte);
					return;
				}
				if (this.canDataEvent.RawDataSignal is RawDataSignalStartbitLength)
				{
					this.tempRawDataSigStartbitLen = (this.canDataEvent.RawDataSignal as RawDataSignalStartbitLength);
				}
			}
		}

		public CANDataCondition(IModelValidator modelVal, List<CondRelation> relationsToHide)
		{
			this.InitializeComponent();
			this.modelValidator = modelVal;
			this.canDataEvent = new CANDataEvent();
			this.mRelationsToHide = relationsToHide;
			this.InitRelOperatorCombobox();
			this.InitByteOrderCombobox();
			this.InitDefaultValues();
		}

		private void InitRelOperatorCombobox()
		{
			this.SubscribeControlEvents(false);
			foreach (CondRelation condRelation in Enum.GetValues(typeof(CondRelation)))
			{
				if (this.mRelationsToHide == null || !this.mRelationsToHide.Contains(condRelation))
				{
					this.comboBoxRelOperator.Items.Add(GUIUtil.MapTriggerConditionRelation2String(condRelation));
				}
			}
			if (this.comboBoxRelOperator.Items.Count > 0)
			{
				this.comboBoxRelOperator.SelectedIndex = 0;
			}
			this.SubscribeControlEvents(true);
		}

		private void InitByteOrderCombobox()
		{
			this.SubscribeControlEvents(false);
			this.comboBoxByteOrder.Items.Add(Resources.Intel);
			this.comboBoxByteOrder.Items.Add(Resources.Motorola);
			this.comboBoxByteOrder.SelectedIndex = 0;
			this.SubscribeControlEvents(true);
		}

		private void InitChannelComboBox()
		{
			this.SubscribeControlEvents(false);
			this.comboBoxChannel.Items.Clear();
			uint totalNumberOfLogicalChannels = this.modelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN);
			for (uint num = 1u; num <= totalNumberOfLogicalChannels; num += 1u)
			{
				this.comboBoxChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
			if (this.canDataEvent.ChannelNumber.Value <= totalNumberOfLogicalChannels)
			{
				this.comboBoxChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.canDataEvent.ChannelNumber.Value);
			}
			else
			{
				this.comboBoxChannel.SelectedIndex = 0;
			}
			this.SubscribeControlEvents(true);
		}

		public void InitDefaultValues()
		{
			this.canDataEvent.ID.Value = 0u;
			this.canDataEvent.IsExtendedId.Value = false;
			this.canDataEvent.ChannelNumber.Value = this.modelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_CAN);
			this.canDataEvent.LowValue.Value = 0u;
			this.canDataEvent.HighValue.Value = 0u;
			this.canDataEvent.Relation.Value = CondRelation.Equal;
			if (this.canDataEvent.RawDataSignal is RawDataSignalByte)
			{
				this.tempRawDataSigByte = (this.canDataEvent.RawDataSignal as RawDataSignalByte);
				this.tempRawDataSigStartbitLen = new RawDataSignalStartbitLength();
			}
			else if (this.canDataEvent.RawDataSignal is RawDataSignalStartbitLength)
			{
				this.tempRawDataSigByte = new RawDataSignalByte();
				this.tempRawDataSigStartbitLen = (this.canDataEvent.RawDataSignal as RawDataSignalStartbitLength);
			}
			this.tempRawDataSigByte.DataBytePos.Value = 0u;
			this.tempRawDataSigStartbitLen.StartbitPos.Value = 0u;
			this.tempRawDataSigStartbitLen.Length.Value = 1u;
			this.tempRawDataSigStartbitLen.IsMotorola.Value = false;
		}

		private void InitControls()
		{
			this.SubscribeControlEvents(false);
			this.textBoxMessageId.Text = GUIUtil.CANIdToDisplayString(this.canDataEvent.ID.Value, this.canDataEvent.IsExtendedId.Value);
			this.checkBoxIsExtended.Checked = this.canDataEvent.IsExtendedId.Value;
			this.radioButtonRawSigByte.Checked = (this.canDataEvent.RawDataSignal is RawDataSignalByte);
			this.radioButtonRawSigStartbit.Checked = (this.canDataEvent.RawDataSignal is RawDataSignalStartbitLength);
			this.EnableControlsStartbitLengthInput(this.canDataEvent.RawDataSignal is RawDataSignalStartbitLength);
			this.textBoxDataByte.Text = this.tempRawDataSigByte.DataBytePos.Value.ToString();
			this.textBoxStartbit.Text = this.tempRawDataSigStartbitLen.StartbitPos.Value.ToString();
			this.textBoxLength.Text = this.tempRawDataSigStartbitLen.Length.Value.ToString();
			if (this.tempRawDataSigStartbitLen.IsMotorola.Value)
			{
				this.comboBoxByteOrder.SelectedItem = Resources.Motorola;
			}
			else
			{
				this.comboBoxByteOrder.SelectedItem = Resources.Intel;
			}
			this.RenderLabelsForByteOrder();
			this.comboBoxRelOperator.SelectedItem = GUIUtil.MapTriggerConditionRelation2String(this.canDataEvent.Relation.Value);
			this.textBoxValue.Text = GUIUtil.NumberToDisplayString(this.canDataEvent.LowValue.Value);
			this.textBoxUpperValue.Text = GUIUtil.NumberToDisplayString(this.canDataEvent.HighValue.Value);
			this.textBoxUpperValue.Enabled = false;
			if (CondRelation.InRange == this.canDataEvent.Relation.Value || CondRelation.NotInRange == this.canDataEvent.Relation.Value)
			{
				this.textBoxUpperValue.Enabled = true;
			}
			if (this.canDataEvent.Relation.Value == CondRelation.OnChange)
			{
				this.textBoxValue.Enabled = false;
			}
			this.SubscribeControlEvents(true);
			this.ValidateInput();
		}

		private void CANDataCondition_Shown(object sender, EventArgs e)
		{
			this.InitChannelComboBox();
			this.RenderLabelsForDisplayMode();
			this.InitControls();
		}

		private void radioButtonRawSigByte_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioButtonRawSigByte.Checked)
			{
				this.canDataEvent.RawDataSignal = this.tempRawDataSigByte;
			}
			else
			{
				this.canDataEvent.RawDataSignal = this.tempRawDataSigStartbitLen;
			}
			this.EnableControlsStartbitLengthInput(false);
			this.ValidateData();
		}

		private void radioButtonRawSigStartbit_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioButtonRawSigByte.Checked)
			{
				this.canDataEvent.RawDataSignal = this.tempRawDataSigByte;
			}
			else
			{
				this.canDataEvent.RawDataSignal = this.tempRawDataSigStartbitLen;
			}
			this.EnableControlsStartbitLengthInput(true);
			this.ValidateData();
		}

		private void textBoxMessageId_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateMessageId();
		}

		private void checkBoxIsExtended_CheckedChanged(object sender, EventArgs e)
		{
			if (this.checkBoxIsExtended.Checked)
			{
				if (!this.HasError(this.textBoxMessageId))
				{
					this.textBoxMessageId.Text = string.Format(Resources.ExtendedCANIdFormat, this.textBoxMessageId.Text);
				}
			}
			else if (!this.HasError(this.textBoxMessageId))
			{
				string text = this.textBoxMessageId.Text;
				if (text.Length > 0 && text.Substring(text.Length - 1).ToLower() == "x")
				{
					this.textBoxMessageId.Text = text.Substring(0, text.Length - 1);
				}
			}
			this.ValidateMessageId();
		}

		private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.canDataEvent.ChannelNumber.Value = GUIUtil.MapCANChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
			this.ValidateGlobalModelErrors();
			this.ValidateData();
		}

		private void textBoxDataByte_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateData();
		}

		private void textBoxStartbit_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateData();
		}

		private void textBoxLength_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateData();
		}

		private void comboBoxByteOrder_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.RenderLabelsForByteOrder();
			this.ValidateData();
		}

		private void comboBoxRelOperator_SelectedIndexChanged(object sender, EventArgs e)
		{
			CondRelation condRelation = GUIUtil.MapString2TriggerConditionRelation(this.comboBoxRelOperator.SelectedItem.ToString());
			if (CondRelation.InRange == condRelation || CondRelation.NotInRange == condRelation)
			{
				this.textBoxUpperValue.Enabled = true;
				this.textBoxValue.Enabled = true;
			}
			else
			{
				this.textBoxUpperValue.Enabled = false;
				this.textBoxUpperValue.Text = "0";
				this.canDataEvent.HighValue.Value = 0u;
				if (condRelation == CondRelation.OnChange)
				{
					this.textBoxValue.Enabled = false;
					this.textBoxValue.Text = "0";
					this.canDataEvent.LowValue.Value = 0u;
				}
				else
				{
					this.textBoxValue.Enabled = true;
				}
			}
			this.ValidateData();
		}

		private void textBoxValue_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateData();
		}

		private void textBoxUpperValue_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateData();
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
			if ((this.canDataEvent.Relation.Value == CondRelation.InRange || this.canDataEvent.Relation.Value == CondRelation.NotInRange) && this.canDataEvent.LowValue.Value > this.canDataEvent.HighValue.Value)
			{
				uint value = this.canDataEvent.LowValue.Value;
				this.canDataEvent.LowValue.Value = this.canDataEvent.HighValue.Value;
				this.canDataEvent.HighValue.Value = value;
			}
			base.DialogResult = DialogResult.OK;
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void CANDataCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private bool ValidateInput()
		{
			bool flag = true;
			flag &= this.ValidateMessageId();
			flag &= this.ValidateData();
			this.ValidateGlobalModelErrors();
			return flag;
		}

		private bool ValidateMessageId()
		{
			this.canDataEvent.IsExtendedId.Value = this.checkBoxIsExtended.Checked;
			this.errorProviderFormat.SetError(this.textBoxMessageId, "");
			uint value;
			bool flag;
			if (!GUIUtil.DisplayStringToCANId(this.textBoxMessageId.Text, out value, out flag))
			{
				if (this.canDataEvent.IsExtendedId.Value)
				{
					this.errorProviderFormat.SetError(this.textBoxMessageId, Resources.ErrorExtendedCANIdExpected);
				}
				else
				{
					this.errorProviderFormat.SetError(this.textBoxMessageId, Resources.ErrorStandardCANIdExpected);
				}
				return false;
			}
			if (this.canDataEvent.IsExtendedId.Value == flag)
			{
				this.canDataEvent.ID.Value = value;
				return true;
			}
			if (this.canDataEvent.IsExtendedId.Value)
			{
				this.errorProviderFormat.SetError(this.textBoxMessageId, Resources.ErrorExtendedCANIdExpected);
			}
			else
			{
				this.errorProviderFormat.SetError(this.textBoxMessageId, Resources.ErrorStandardCANIdExpected);
			}
			return false;
		}

		private bool ValidateData()
		{
			this.ResetDataErrorProvider();
			bool flag = true;
			uint num = 255u;
			if (this.canDataEvent.RawDataSignal is RawDataSignalStartbitLength)
			{
				uint num2 = 0u;
				uint num3 = 0u;
				bool flag2 = false;
				if (!uint.TryParse(this.textBoxStartbit.Text, out num2))
				{
					this.errorProviderFormat.SetError(this.textBoxStartbit, Resources.ErrorNumberExpected);
					flag = false;
				}
				else
				{
					uint num4 = Constants.MaxCANBitPosition;
					if (this.modelValidator.IsCANChannelFDModeActive(this.canDataEvent.ChannelNumber.Value))
					{
						num4 = Constants.MaxCANFDBitPosition;
					}
					if (num2 > num4)
					{
						this.errorProviderFormat.SetError(this.textBoxStartbit, string.Format(Resources.ErrorStartbitOutOfRange, num4));
						flag = false;
					}
				}
				if (!uint.TryParse(this.textBoxLength.Text, out num3))
				{
					this.errorProviderFormat.SetError(this.textBoxLength, Resources.ErrorNumberExpected);
					flag = false;
				}
				else if (num3 < Constants.MinRawSignalLength || num3 > this.modelValidator.LoggerSpecifics.Recording.MaxRawSignalLength)
				{
					this.errorProviderFormat.SetError(this.textBoxLength, string.Format(Resources.ErrorRawSigLenOutOfRange, Constants.MinRawSignalLength, this.modelValidator.LoggerSpecifics.Recording.MaxRawSignalLength));
					flag = false;
				}
				else
				{
					num = (uint)Math.Pow(2.0, num3) - 1u;
				}
				if (flag)
				{
					uint num5 = Constants.MaxCANDataBytes;
					if (this.modelValidator.IsCANChannelFDModeActive(this.canDataEvent.ChannelNumber.Value))
					{
						num5 = Constants.MaxCANFDDataBytes;
					}
					flag2 = (this.comboBoxByteOrder.SelectedItem.ToString() == Resources.Motorola);
					if (!GUIUtil.IsRawSignalWithinDatabytes(num2, num3, flag2, num5))
					{
						string value = string.Format(Resources.ErrorRawSigOutOfRange, num5, Vocabulary.CAN);
						this.errorProviderFormat.SetError(this.textBoxStartbit, value);
						this.errorProviderFormat.SetError(this.textBoxLength, value);
						this.errorProviderFormat.SetError(this.comboBoxByteOrder, value);
						flag = false;
					}
				}
				if (flag)
				{
					(this.canDataEvent.RawDataSignal as RawDataSignalStartbitLength).StartbitPos.Value = num2;
					(this.canDataEvent.RawDataSignal as RawDataSignalStartbitLength).Length.Value = num3;
					(this.canDataEvent.RawDataSignal as RawDataSignalStartbitLength).IsMotorola.Value = flag2;
				}
			}
			else
			{
				uint num6 = 0u;
				if (!uint.TryParse(this.textBoxDataByte.Text, out num6))
				{
					this.errorProviderFormat.SetError(this.textBoxDataByte, Resources.ErrorNumberExpected);
				}
				else
				{
					uint num7 = Constants.MaxCANDataBytes;
					if (this.modelValidator.IsCANChannelFDModeActive(this.canDataEvent.ChannelNumber.Value))
					{
						num7 = Constants.MaxCANFDDataBytes;
					}
					if (num6 >= num7)
					{
						this.errorProviderFormat.SetError(this.textBoxDataByte, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 0, num7 - 1u));
					}
					else if (this.canDataEvent.RawDataSignal is RawDataSignalByte)
					{
						(this.canDataEvent.RawDataSignal as RawDataSignalByte).DataBytePos.Value = num6;
					}
				}
			}
			uint num8 = 0u;
			uint num9 = 0u;
			if (!GUIUtil.DisplayStringToNumber(this.textBoxValue.Text, out num8))
			{
				this.errorProviderFormat.SetError(this.textBoxValue, Resources.ErrorNumberExpected);
				flag = false;
			}
			else if (num8 > num)
			{
				this.errorProviderFormat.SetError(this.textBoxValue, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, GUIUtil.NumberToDisplayString(0), GUIUtil.NumberToDisplayString(num)));
				flag = false;
			}
			else
			{
				this.canDataEvent.LowValue.Value = num8;
			}
			this.canDataEvent.Relation.Value = GUIUtil.MapString2TriggerConditionRelation(this.comboBoxRelOperator.SelectedItem.ToString());
			if (this.canDataEvent.Relation.Value == CondRelation.InRange || this.canDataEvent.Relation.Value == CondRelation.NotInRange)
			{
				if (!GUIUtil.DisplayStringToNumber(this.textBoxUpperValue.Text, out num9))
				{
					this.errorProviderFormat.SetError(this.textBoxUpperValue, Resources.ErrorNumberExpected);
					flag = false;
				}
				else if (num9 > num)
				{
					this.errorProviderFormat.SetError(this.textBoxUpperValue, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, GUIUtil.NumberToDisplayString(0), GUIUtil.NumberToDisplayString(num)));
					flag = false;
				}
				else
				{
					this.canDataEvent.HighValue.Value = num9;
				}
			}
			if (flag && ((this.canDataEvent.LowValue.Value == 0u && this.canDataEvent.Relation.Value == CondRelation.LessThan) || (this.canDataEvent.LowValue.Value == num && this.canDataEvent.Relation.Value == CondRelation.GreaterThan)))
			{
				this.errorProviderFormat.SetError(this.textBoxValue, Resources.ErrorInvalidValueWithRelOp);
				flag = false;
			}
			return flag;
		}

		private void ValidateGlobalModelErrors()
		{
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
			if (!this.modelValidator.IsHardwareChannelAvailable(BusType.Bt_CAN, this.canDataEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelNotAvailable);
				return;
			}
			if (!this.modelValidator.IsHardwareChannelActive(BusType.Bt_CAN, this.canDataEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelInactive);
			}
		}

		private bool HasGlobalModelErrors()
		{
			return !string.IsNullOrEmpty(this.errorProviderGlobalModel.GetError(this.comboBoxChannel));
		}

		private bool HasError(Control control)
		{
			return !string.IsNullOrEmpty(this.errorProviderFormat.GetError(control));
		}

		private void EnableControlsStartbitLengthInput(bool isEnabled)
		{
			this.textBoxDataByte.Enabled = !isEnabled;
			this.textBoxStartbit.Enabled = isEnabled;
			this.textBoxLength.Enabled = isEnabled;
			this.comboBoxByteOrder.Enabled = isEnabled;
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.textBoxMessageId.Validating += new CancelEventHandler(this.textBoxMessageId_Validating);
				this.checkBoxIsExtended.CheckedChanged += new EventHandler(this.checkBoxIsExtended_CheckedChanged);
				this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
				this.radioButtonRawSigByte.CheckedChanged += new EventHandler(this.radioButtonRawSigByte_CheckedChanged);
				this.radioButtonRawSigStartbit.CheckedChanged += new EventHandler(this.radioButtonRawSigStartbit_CheckedChanged);
				this.textBoxDataByte.Validating += new CancelEventHandler(this.textBoxDataByte_Validating);
				this.textBoxStartbit.Validating += new CancelEventHandler(this.textBoxStartbit_Validating);
				this.textBoxLength.Validating += new CancelEventHandler(this.textBoxLength_Validating);
				this.comboBoxByteOrder.SelectedIndexChanged += new EventHandler(this.comboBoxByteOrder_SelectedIndexChanged);
				this.comboBoxRelOperator.SelectedIndexChanged += new EventHandler(this.comboBoxRelOperator_SelectedIndexChanged);
				this.textBoxValue.Validating += new CancelEventHandler(this.textBoxValue_Validating);
				this.textBoxUpperValue.Validating += new CancelEventHandler(this.textBoxUpperValue_Validating);
				return;
			}
			this.textBoxMessageId.Validating -= new CancelEventHandler(this.textBoxMessageId_Validating);
			this.checkBoxIsExtended.CheckedChanged -= new EventHandler(this.checkBoxIsExtended_CheckedChanged);
			this.comboBoxChannel.SelectedIndexChanged -= new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			this.radioButtonRawSigByte.CheckedChanged -= new EventHandler(this.radioButtonRawSigByte_CheckedChanged);
			this.radioButtonRawSigStartbit.CheckedChanged -= new EventHandler(this.radioButtonRawSigStartbit_CheckedChanged);
			this.textBoxDataByte.Validating -= new CancelEventHandler(this.textBoxDataByte_Validating);
			this.textBoxStartbit.Validating -= new CancelEventHandler(this.textBoxStartbit_Validating);
			this.textBoxLength.Validating -= new CancelEventHandler(this.textBoxLength_Validating);
			this.comboBoxByteOrder.SelectedIndexChanged -= new EventHandler(this.comboBoxByteOrder_SelectedIndexChanged);
			this.comboBoxRelOperator.SelectedIndexChanged -= new EventHandler(this.comboBoxRelOperator_SelectedIndexChanged);
			this.textBoxValue.Validating -= new CancelEventHandler(this.textBoxValue_Validating);
			this.textBoxUpperValue.Validating -= new CancelEventHandler(this.textBoxUpperValue_Validating);
		}

		private void ResetErrorProvider()
		{
			this.errorProviderFormat.SetError(this.textBoxMessageId, "");
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
			this.ResetDataErrorProvider();
		}

		private void ResetDataErrorProvider()
		{
			this.errorProviderFormat.SetError(this.textBoxDataByte, "");
			this.errorProviderFormat.SetError(this.textBoxStartbit, "");
			this.errorProviderFormat.SetError(this.textBoxLength, "");
			this.errorProviderFormat.SetError(this.textBoxValue, "");
			this.errorProviderFormat.SetError(this.textBoxUpperValue, "");
			this.errorProviderFormat.SetError(this.comboBoxByteOrder, "");
		}

		private void RenderLabelsForDisplayMode()
		{
			string arg = Resources.DisplayModeDec;
			if (GUIUtil.IsHexadecimal)
			{
				arg = Resources.DisplayModeHex;
			}
			this.labelMsgId.Text = string.Format(Resources.IDLabelWithMode, arg);
			this.labelValue.Text = string.Format(Resources.LabelSigValue, arg);
			this.labelUpperLimit.Text = string.Format(Resources.LabelUpperSigValue, arg);
		}

		private void RenderLabelsForByteOrder()
		{
			if (this.comboBoxByteOrder.SelectedItem.ToString() == Resources.Motorola)
			{
				this.radioButtonRawSigStartbit.Text = Resources.LabelStartBitMSB;
				return;
			}
			this.radioButtonRawSigStartbit.Text = Resources.LabelStartBitLSB;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CANDataCondition));
			this.labelMsgId = new Label();
			this.textBoxMessageId = new TextBox();
			this.checkBoxIsExtended = new CheckBox();
			this.labelRelOperator = new Label();
			this.comboBoxRelOperator = new ComboBox();
			this.labelValue = new Label();
			this.textBoxValue = new TextBox();
			this.labelUpperLimit = new Label();
			this.textBoxUpperValue = new TextBox();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.textBoxStartbit = new TextBox();
			this.textBoxLength = new TextBox();
			this.comboBoxByteOrder = new ComboBox();
			this.comboBoxChannel = new ComboBox();
			this.labelNoteExtId = new Label();
			this.radioButtonRawSigStartbit = new RadioButton();
			this.radioButtonRawSigByte = new RadioButton();
			this.labelLength = new Label();
			this.labelByteOrder = new Label();
			this.groupBoxData = new GroupBox();
			this.textBoxDataByte = new TextBox();
			this.groupBoxMessage = new GroupBox();
			this.labelChannel = new Label();
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			this.groupBoxData.SuspendLayout();
			this.groupBoxMessage.SuspendLayout();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelMsgId, "labelMsgId");
			this.labelMsgId.Name = "labelMsgId";
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxMessageId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMessageId.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxMessageId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMessageId.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxMessageId, "textBoxMessageId");
			this.textBoxMessageId.Name = "textBoxMessageId";
			this.textBoxMessageId.Validating += new CancelEventHandler(this.textBoxMessageId_Validating);
			componentResourceManager.ApplyResources(this.checkBoxIsExtended, "checkBoxIsExtended");
			this.checkBoxIsExtended.Name = "checkBoxIsExtended";
			this.checkBoxIsExtended.UseVisualStyleBackColor = true;
			this.checkBoxIsExtended.CheckedChanged += new EventHandler(this.checkBoxIsExtended_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelRelOperator, "labelRelOperator");
			this.labelRelOperator.Name = "labelRelOperator";
			this.comboBoxRelOperator.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxRelOperator.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxRelOperator, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxRelOperator.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxRelOperator, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxRelOperator.IconAlignment1"));
			componentResourceManager.ApplyResources(this.comboBoxRelOperator, "comboBoxRelOperator");
			this.comboBoxRelOperator.Name = "comboBoxRelOperator";
			this.comboBoxRelOperator.SelectedIndexChanged += new EventHandler(this.comboBoxRelOperator_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelValue, "labelValue");
			this.labelValue.Name = "labelValue";
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxValue.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxValue.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxValue, "textBoxValue");
			this.textBoxValue.Name = "textBoxValue";
			this.textBoxValue.Validating += new CancelEventHandler(this.textBoxValue_Validating);
			componentResourceManager.ApplyResources(this.labelUpperLimit, "labelUpperLimit");
			this.labelUpperLimit.Name = "labelUpperLimit";
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxUpperValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxUpperValue.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxUpperValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxUpperValue.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxUpperValue, "textBoxUpperValue");
			this.textBoxUpperValue.Name = "textBoxUpperValue";
			this.textBoxUpperValue.Validating += new CancelEventHandler(this.textBoxUpperValue_Validating);
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxStartbit, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxStartbit.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxStartbit, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxStartbit.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxStartbit, "textBoxStartbit");
			this.textBoxStartbit.Name = "textBoxStartbit";
			this.textBoxStartbit.Validating += new CancelEventHandler(this.textBoxStartbit_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxLength, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLength.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxLength, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLength.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxLength, "textBoxLength");
			this.textBoxLength.Name = "textBoxLength";
			this.textBoxLength.Validating += new CancelEventHandler(this.textBoxLength_Validating);
			this.comboBoxByteOrder.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxByteOrder.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxByteOrder, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxByteOrder.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxByteOrder, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxByteOrder.IconAlignment1"));
			componentResourceManager.ApplyResources(this.comboBoxByteOrder, "comboBoxByteOrder");
			this.comboBoxByteOrder.Name = "comboBoxByteOrder";
			this.comboBoxByteOrder.SelectedIndexChanged += new EventHandler(this.comboBoxByteOrder_SelectedIndexChanged);
			this.comboBoxChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxChannel.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment1"));
			componentResourceManager.ApplyResources(this.comboBoxChannel, "comboBoxChannel");
			this.comboBoxChannel.Name = "comboBoxChannel";
			this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelNoteExtId, "labelNoteExtId");
			this.labelNoteExtId.Name = "labelNoteExtId";
			componentResourceManager.ApplyResources(this.radioButtonRawSigStartbit, "radioButtonRawSigStartbit");
			this.radioButtonRawSigStartbit.Name = "radioButtonRawSigStartbit";
			this.radioButtonRawSigStartbit.TabStop = true;
			this.radioButtonRawSigStartbit.UseVisualStyleBackColor = true;
			this.radioButtonRawSigStartbit.CheckedChanged += new EventHandler(this.radioButtonRawSigStartbit_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonRawSigByte, "radioButtonRawSigByte");
			this.radioButtonRawSigByte.Name = "radioButtonRawSigByte";
			this.radioButtonRawSigByte.TabStop = true;
			this.radioButtonRawSigByte.UseVisualStyleBackColor = true;
			this.radioButtonRawSigByte.CheckedChanged += new EventHandler(this.radioButtonRawSigByte_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelLength, "labelLength");
			this.labelLength.Name = "labelLength";
			componentResourceManager.ApplyResources(this.labelByteOrder, "labelByteOrder");
			this.labelByteOrder.Name = "labelByteOrder";
			this.groupBoxData.Controls.Add(this.textBoxDataByte);
			this.groupBoxData.Controls.Add(this.labelByteOrder);
			this.groupBoxData.Controls.Add(this.labelLength);
			this.groupBoxData.Controls.Add(this.radioButtonRawSigByte);
			this.groupBoxData.Controls.Add(this.radioButtonRawSigStartbit);
			this.groupBoxData.Controls.Add(this.comboBoxByteOrder);
			this.groupBoxData.Controls.Add(this.textBoxLength);
			this.groupBoxData.Controls.Add(this.textBoxStartbit);
			this.groupBoxData.Controls.Add(this.textBoxUpperValue);
			this.groupBoxData.Controls.Add(this.labelUpperLimit);
			this.groupBoxData.Controls.Add(this.textBoxValue);
			this.groupBoxData.Controls.Add(this.labelValue);
			this.groupBoxData.Controls.Add(this.comboBoxRelOperator);
			this.groupBoxData.Controls.Add(this.labelRelOperator);
			componentResourceManager.ApplyResources(this.groupBoxData, "groupBoxData");
			this.groupBoxData.Name = "groupBoxData";
			this.groupBoxData.TabStop = false;
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDataByte, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDataByte.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDataByte, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDataByte.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxDataByte, "textBoxDataByte");
			this.textBoxDataByte.Name = "textBoxDataByte";
			this.textBoxDataByte.Validating += new CancelEventHandler(this.textBoxDataByte_Validating);
			this.groupBoxMessage.Controls.Add(this.labelChannel);
			this.groupBoxMessage.Controls.Add(this.comboBoxChannel);
			this.groupBoxMessage.Controls.Add(this.labelNoteExtId);
			this.groupBoxMessage.Controls.Add(this.checkBoxIsExtended);
			this.groupBoxMessage.Controls.Add(this.textBoxMessageId);
			this.groupBoxMessage.Controls.Add(this.labelMsgId);
			componentResourceManager.ApplyResources(this.groupBoxMessage, "groupBoxMessage");
			this.groupBoxMessage.Name = "groupBoxMessage";
			this.groupBoxMessage.TabStop = false;
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
			base.Controls.Add(this.groupBoxMessage);
			base.Controls.Add(this.groupBoxData);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CANDataCondition";
			base.Shown += new EventHandler(this.CANDataCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.CANDataCondition_HelpRequested);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			this.groupBoxData.ResumeLayout(false);
			this.groupBoxData.PerformLayout();
			this.groupBoxMessage.ResumeLayout(false);
			this.groupBoxMessage.PerformLayout();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
