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
	internal class LINDataCondition : Form
	{
		private LINDataEvent linDataEvent;

		private RawDataSignalByte tempRawDataSigByte;

		private RawDataSignalStartbitLength tempRawDataSigStartbitLen;

		private IModelValidator modelValidator;

		private List<CondRelation> relationsToHide;

		private int invalidChannelIndex;

		private IContainer components;

		private Label labelMsgId;

		private TextBox textBoxMessageId;

		private ComboBox comboBoxDataByte;

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

		private RadioButton radioButtonRawSigByte;

		private Label labelByteOrder;

		private Label labelLength;

		private GroupBox groupBoxMessage;

		private GroupBox groupBoxData;

		private Label labelChannel;

		private ComboBox comboBoxChannel;

		private ErrorProvider errorProviderGlobalModel;

		public LINDataEvent LINDataEvent
		{
			get
			{
				return this.linDataEvent;
			}
			set
			{
				this.linDataEvent = value;
				if (this.linDataEvent.RawDataSignal is RawDataSignalByte)
				{
					this.tempRawDataSigByte = (this.linDataEvent.RawDataSignal as RawDataSignalByte);
					return;
				}
				if (this.linDataEvent.RawDataSignal is RawDataSignalStartbitLength)
				{
					this.tempRawDataSigStartbitLen = (this.linDataEvent.RawDataSignal as RawDataSignalStartbitLength);
				}
			}
		}

		public LINDataCondition(IModelValidator modelVal, List<CondRelation> relToHide)
		{
			this.InitializeComponent();
			this.modelValidator = modelVal;
			this.linDataEvent = new LINDataEvent();
			this.relationsToHide = relToHide;
			this.invalidChannelIndex = -1;
			this.SubscribeControlEvents(false);
			this.InitRelOperatorCombobox();
			this.InitDataByteCombobox();
			this.InitByteOrderCombobox();
			this.SubscribeControlEvents(true);
			this.InitDefaultValues();
		}

		private void InitRelOperatorCombobox()
		{
			foreach (CondRelation condRelation in Enum.GetValues(typeof(CondRelation)))
			{
				if (this.relationsToHide == null || !this.relationsToHide.Contains(condRelation))
				{
					this.comboBoxRelOperator.Items.Add(GUIUtil.MapTriggerConditionRelation2String(condRelation));
				}
			}
			if (this.comboBoxRelOperator.Items.Count > 0)
			{
				this.comboBoxRelOperator.SelectedIndex = 0;
			}
		}

		private void InitDataByteCombobox()
		{
			int num = 0;
			while ((long)num < (long)((ulong)Constants.MaxLINDataBytes))
			{
				this.comboBoxDataByte.Items.Add(string.Format(Resources.DataBytePos, num));
				num++;
			}
			this.comboBoxDataByte.SelectedIndex = 0;
		}

		private void InitByteOrderCombobox()
		{
			this.comboBoxByteOrder.Items.Add(Resources.Intel);
			this.comboBoxByteOrder.Items.Add(Resources.Motorola);
			this.comboBoxByteOrder.SelectedIndex = 0;
		}

		private void InitChannelComboBox()
		{
			this.SubscribeControlEvents(false);
			this.comboBoxChannel.Items.Clear();
			this.invalidChannelIndex = -1;
			uint totalNumberOfLogicalChannels = this.modelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_LIN);
			for (uint num = 1u; num <= totalNumberOfLogicalChannels; num += 1u)
			{
				this.comboBoxChannel.Items.Add(GUIUtil.MapLINChannelNumber2String(num, this.modelValidator.LoggerSpecifics));
			}
			this.comboBoxChannel.SelectedIndex = 0;
			this.SubscribeControlEvents(true);
		}

		public void InitDefaultValues()
		{
			this.linDataEvent.ID.Value = 0u;
			this.linDataEvent.ChannelNumber.Value = this.modelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_LIN);
			this.linDataEvent.LowValue.Value = 0u;
			this.linDataEvent.HighValue.Value = 0u;
			this.linDataEvent.Relation.Value = CondRelation.Equal;
			if (this.linDataEvent.RawDataSignal is RawDataSignalByte)
			{
				this.tempRawDataSigByte = (this.linDataEvent.RawDataSignal as RawDataSignalByte);
				this.tempRawDataSigStartbitLen = new RawDataSignalStartbitLength();
			}
			else if (this.linDataEvent.RawDataSignal is RawDataSignalStartbitLength)
			{
				this.tempRawDataSigByte = new RawDataSignalByte();
				this.tempRawDataSigStartbitLen = (this.linDataEvent.RawDataSignal as RawDataSignalStartbitLength);
			}
			this.tempRawDataSigByte.DataBytePos.Value = 0u;
			this.tempRawDataSigStartbitLen.StartbitPos.Value = 0u;
			this.tempRawDataSigStartbitLen.Length.Value = 1u;
			this.tempRawDataSigStartbitLen.IsMotorola.Value = false;
		}

		private void InitControls()
		{
			this.SubscribeControlEvents(false);
			this.textBoxMessageId.Text = GUIUtil.LINIdToDisplayString(this.linDataEvent.ID.Value);
			string text = GUIUtil.MapLINChannelNumber2String(this.linDataEvent.ChannelNumber.Value, this.modelValidator.LoggerSpecifics);
			if (this.comboBoxChannel.Items.Contains(text))
			{
				this.comboBoxChannel.SelectedItem = GUIUtil.MapLINChannelNumber2String(this.linDataEvent.ChannelNumber.Value, this.modelValidator.LoggerSpecifics);
			}
			else
			{
				this.invalidChannelIndex = this.comboBoxChannel.Items.Add(text);
				this.comboBoxChannel.SelectedIndex = this.invalidChannelIndex;
			}
			this.radioButtonRawSigByte.Checked = (this.linDataEvent.RawDataSignal is RawDataSignalByte);
			this.radioButtonRawSigStartbit.Checked = (this.linDataEvent.RawDataSignal is RawDataSignalStartbitLength);
			this.EnableControlsStartbitLengthInput(this.linDataEvent.RawDataSignal is RawDataSignalStartbitLength);
			this.comboBoxDataByte.SelectedIndex = (int)this.tempRawDataSigByte.DataBytePos.Value;
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
			this.comboBoxRelOperator.SelectedItem = GUIUtil.MapTriggerConditionRelation2String(this.linDataEvent.Relation.Value);
			this.textBoxValue.Text = GUIUtil.NumberToDisplayString(this.linDataEvent.LowValue.Value);
			this.textBoxUpperValue.Text = GUIUtil.NumberToDisplayString(this.linDataEvent.HighValue.Value);
			this.textBoxUpperValue.Enabled = false;
			if (CondRelation.InRange == this.linDataEvent.Relation.Value || CondRelation.NotInRange == this.linDataEvent.Relation.Value)
			{
				this.textBoxUpperValue.Enabled = true;
			}
			if (this.linDataEvent.Relation.Value == CondRelation.OnChange)
			{
				this.textBoxValue.Enabled = false;
			}
			this.SubscribeControlEvents(true);
			this.ValidateInput();
		}

		private void LINDataCondition_Shown(object sender, EventArgs e)
		{
			this.InitChannelComboBox();
			this.RenderLabelsForDisplayMode();
			this.InitControls();
		}

		private void radioButtonRawSigByte_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioButtonRawSigByte.Checked)
			{
				this.linDataEvent.RawDataSignal = this.tempRawDataSigByte;
			}
			else
			{
				this.linDataEvent.RawDataSignal = this.tempRawDataSigStartbitLen;
			}
			this.EnableControlsStartbitLengthInput(false);
			this.ValidateData();
		}

		private void radioButtonRawSigStartbit_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioButtonRawSigByte.Checked)
			{
				this.linDataEvent.RawDataSignal = this.tempRawDataSigByte;
			}
			else
			{
				this.linDataEvent.RawDataSignal = this.tempRawDataSigStartbitLen;
			}
			this.EnableControlsStartbitLengthInput(true);
			this.ValidateData();
		}

		private void textBoxMessageId_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateMessageId();
		}

		private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.linDataEvent.ChannelNumber.Value = GUIUtil.MapLINChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
			if (this.invalidChannelIndex >= 0 && this.comboBoxChannel.SelectedIndex != this.invalidChannelIndex)
			{
				this.comboBoxChannel.Items.RemoveAt(this.invalidChannelIndex);
				this.invalidChannelIndex = -1;
			}
			this.ValidateGlobalModelErrors();
		}

		private void comboBoxDataByte_SelectedIndexChanged(object sender, EventArgs e)
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
				this.linDataEvent.HighValue.Value = 0u;
				if (condRelation == CondRelation.OnChange)
				{
					this.textBoxValue.Enabled = false;
					this.textBoxValue.Text = "0";
					this.linDataEvent.LowValue.Value = 0u;
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
			if ((this.linDataEvent.Relation.Value == CondRelation.InRange || this.linDataEvent.Relation.Value == CondRelation.NotInRange) && this.linDataEvent.LowValue.Value > this.linDataEvent.HighValue.Value)
			{
				uint value = this.linDataEvent.LowValue.Value;
				this.linDataEvent.LowValue.Value = this.linDataEvent.HighValue.Value;
				this.linDataEvent.HighValue.Value = value;
			}
			base.DialogResult = DialogResult.OK;
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void LINDataCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
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
			this.errorProviderFormat.SetError(this.textBoxMessageId, "");
			uint value;
			if (!GUIUtil.DisplayStringToLINId(this.textBoxMessageId.Text, out value))
			{
				this.errorProviderFormat.SetError(this.textBoxMessageId, Resources.ErrorLINIdExpected);
				return false;
			}
			this.linDataEvent.ID.Value = value;
			return true;
		}

		private bool ValidateData()
		{
			this.ResetDataErrorProvider();
			bool flag = true;
			uint num = 255u;
			if (this.linDataEvent.RawDataSignal is RawDataSignalStartbitLength)
			{
				uint num2 = 0u;
				uint num3 = 0u;
				bool flag2 = false;
				if (!uint.TryParse(this.textBoxStartbit.Text, out num2))
				{
					this.errorProviderFormat.SetError(this.textBoxStartbit, Resources.ErrorNumberExpected);
					flag = false;
				}
				else if (num2 > Constants.MaxLINBitPosition)
				{
					this.errorProviderFormat.SetError(this.textBoxStartbit, string.Format(Resources.ErrorStartbitOutOfRange, Constants.MaxLINBitPosition));
					flag = false;
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
					flag2 = (this.comboBoxByteOrder.SelectedItem.ToString() == Resources.Motorola);
					if (!GUIUtil.IsRawSignalWithinDatabytes(num2, num3, flag2, Constants.MaxLINDataBytes))
					{
						string value = string.Format(Resources.ErrorRawSigOutOfRange, Constants.MaxLINDataBytes, Vocabulary.LIN);
						this.errorProviderFormat.SetError(this.textBoxStartbit, value);
						this.errorProviderFormat.SetError(this.textBoxLength, value);
						this.errorProviderFormat.SetError(this.comboBoxByteOrder, value);
						flag = false;
					}
				}
				if (flag)
				{
					(this.linDataEvent.RawDataSignal as RawDataSignalStartbitLength).StartbitPos.Value = num2;
					(this.linDataEvent.RawDataSignal as RawDataSignalStartbitLength).Length.Value = num3;
					(this.linDataEvent.RawDataSignal as RawDataSignalStartbitLength).IsMotorola.Value = flag2;
				}
			}
			else
			{
				RawDataSignalByte rawDataSignalByte = this.linDataEvent.RawDataSignal as RawDataSignalByte;
				if (rawDataSignalByte != null)
				{
					rawDataSignalByte.DataBytePos.Value = (uint)this.comboBoxDataByte.SelectedIndex;
				}
			}
			uint num4 = 0u;
			uint num5 = 0u;
			if (!GUIUtil.DisplayStringToNumber(this.textBoxValue.Text, out num4))
			{
				this.errorProviderFormat.SetError(this.textBoxValue, Resources.ErrorNumberExpected);
				flag = false;
			}
			else if (num4 > num)
			{
				this.errorProviderFormat.SetError(this.textBoxValue, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, GUIUtil.NumberToDisplayString(0), GUIUtil.NumberToDisplayString(num)));
				flag = false;
			}
			else
			{
				this.linDataEvent.LowValue.Value = num4;
			}
			this.linDataEvent.Relation.Value = GUIUtil.MapString2TriggerConditionRelation(this.comboBoxRelOperator.SelectedItem.ToString());
			if (this.linDataEvent.Relation.Value == CondRelation.InRange || this.linDataEvent.Relation.Value == CondRelation.NotInRange)
			{
				if (!GUIUtil.DisplayStringToNumber(this.textBoxUpperValue.Text, out num5))
				{
					this.errorProviderFormat.SetError(this.textBoxUpperValue, Resources.ErrorNumberExpected);
					flag = false;
				}
				else if (num5 > num)
				{
					this.errorProviderFormat.SetError(this.textBoxUpperValue, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, GUIUtil.NumberToDisplayString(0), GUIUtil.NumberToDisplayString(num)));
					flag = false;
				}
				else
				{
					this.linDataEvent.HighValue.Value = num5;
				}
			}
			if (flag && ((this.linDataEvent.LowValue.Value == 0u && this.linDataEvent.Relation.Value == CondRelation.LessThan) || (this.linDataEvent.LowValue.Value == num && this.linDataEvent.Relation.Value == CondRelation.GreaterThan)))
			{
				this.errorProviderFormat.SetError(this.textBoxValue, Resources.ErrorInvalidValueWithRelOp);
				flag = false;
			}
			return flag;
		}

		private void ValidateGlobalModelErrors()
		{
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
			if (!this.modelValidator.IsHardwareChannelAvailable(BusType.Bt_LIN, this.linDataEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelNotAvailable);
				return;
			}
			if (!this.modelValidator.IsHardwareChannelActive(BusType.Bt_LIN, this.linDataEvent.ChannelNumber.Value))
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
			this.comboBoxDataByte.Enabled = !isEnabled;
			this.textBoxStartbit.Enabled = isEnabled;
			this.textBoxLength.Enabled = isEnabled;
			this.comboBoxByteOrder.Enabled = isEnabled;
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.textBoxMessageId.Validating += new CancelEventHandler(this.textBoxMessageId_Validating);
				this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
				this.radioButtonRawSigByte.CheckedChanged += new EventHandler(this.radioButtonRawSigByte_CheckedChanged);
				this.radioButtonRawSigStartbit.CheckedChanged += new EventHandler(this.radioButtonRawSigStartbit_CheckedChanged);
				this.comboBoxDataByte.SelectedIndexChanged += new EventHandler(this.comboBoxDataByte_SelectedIndexChanged);
				this.textBoxStartbit.Validating += new CancelEventHandler(this.textBoxStartbit_Validating);
				this.textBoxLength.Validating += new CancelEventHandler(this.textBoxLength_Validating);
				this.comboBoxByteOrder.SelectedIndexChanged += new EventHandler(this.comboBoxByteOrder_SelectedIndexChanged);
				this.comboBoxRelOperator.SelectedIndexChanged += new EventHandler(this.comboBoxRelOperator_SelectedIndexChanged);
				this.textBoxValue.Validating += new CancelEventHandler(this.textBoxValue_Validating);
				this.textBoxUpperValue.Validating += new CancelEventHandler(this.textBoxUpperValue_Validating);
				return;
			}
			this.textBoxMessageId.Validating -= new CancelEventHandler(this.textBoxMessageId_Validating);
			this.comboBoxChannel.SelectedIndexChanged -= new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			this.radioButtonRawSigByte.CheckedChanged -= new EventHandler(this.radioButtonRawSigByte_CheckedChanged);
			this.radioButtonRawSigStartbit.CheckedChanged -= new EventHandler(this.radioButtonRawSigStartbit_CheckedChanged);
			this.comboBoxDataByte.SelectedIndexChanged -= new EventHandler(this.comboBoxDataByte_SelectedIndexChanged);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LINDataCondition));
			this.labelMsgId = new Label();
			this.textBoxMessageId = new TextBox();
			this.comboBoxDataByte = new ComboBox();
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
			this.radioButtonRawSigStartbit = new RadioButton();
			this.radioButtonRawSigByte = new RadioButton();
			this.labelLength = new Label();
			this.labelByteOrder = new Label();
			this.groupBoxData = new GroupBox();
			this.groupBoxMessage = new GroupBox();
			this.labelChannel = new Label();
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			this.groupBoxData.SuspendLayout();
			this.groupBoxMessage.SuspendLayout();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelMsgId, "labelMsgId");
			this.errorProviderFormat.SetError(this.labelMsgId, componentResourceManager.GetString("labelMsgId.Error"));
			this.errorProviderGlobalModel.SetError(this.labelMsgId, componentResourceManager.GetString("labelMsgId.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelMsgId, (ErrorIconAlignment)componentResourceManager.GetObject("labelMsgId.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelMsgId, (ErrorIconAlignment)componentResourceManager.GetObject("labelMsgId.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelMsgId, (int)componentResourceManager.GetObject("labelMsgId.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelMsgId, (int)componentResourceManager.GetObject("labelMsgId.IconPadding1"));
			this.labelMsgId.Name = "labelMsgId";
			componentResourceManager.ApplyResources(this.textBoxMessageId, "textBoxMessageId");
			this.errorProviderGlobalModel.SetError(this.textBoxMessageId, componentResourceManager.GetString("textBoxMessageId.Error"));
			this.errorProviderFormat.SetError(this.textBoxMessageId, componentResourceManager.GetString("textBoxMessageId.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxMessageId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMessageId.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxMessageId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMessageId.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxMessageId, (int)componentResourceManager.GetObject("textBoxMessageId.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxMessageId, (int)componentResourceManager.GetObject("textBoxMessageId.IconPadding1"));
			this.textBoxMessageId.Name = "textBoxMessageId";
			this.textBoxMessageId.Validating += new CancelEventHandler(this.textBoxMessageId_Validating);
			componentResourceManager.ApplyResources(this.comboBoxDataByte, "comboBoxDataByte");
			this.comboBoxDataByte.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderGlobalModel.SetError(this.comboBoxDataByte, componentResourceManager.GetString("comboBoxDataByte.Error"));
			this.errorProviderFormat.SetError(this.comboBoxDataByte, componentResourceManager.GetString("comboBoxDataByte.Error1"));
			this.comboBoxDataByte.FormattingEnabled = true;
			this.errorProviderFormat.SetIconAlignment(this.comboBoxDataByte, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDataByte.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxDataByte, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDataByte.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxDataByte, (int)componentResourceManager.GetObject("comboBoxDataByte.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxDataByte, (int)componentResourceManager.GetObject("comboBoxDataByte.IconPadding1"));
			this.comboBoxDataByte.Name = "comboBoxDataByte";
			this.comboBoxDataByte.SelectedIndexChanged += new EventHandler(this.comboBoxDataByte_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelRelOperator, "labelRelOperator");
			this.errorProviderFormat.SetError(this.labelRelOperator, componentResourceManager.GetString("labelRelOperator.Error"));
			this.errorProviderGlobalModel.SetError(this.labelRelOperator, componentResourceManager.GetString("labelRelOperator.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelRelOperator, (ErrorIconAlignment)componentResourceManager.GetObject("labelRelOperator.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelRelOperator, (ErrorIconAlignment)componentResourceManager.GetObject("labelRelOperator.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelRelOperator, (int)componentResourceManager.GetObject("labelRelOperator.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelRelOperator, (int)componentResourceManager.GetObject("labelRelOperator.IconPadding1"));
			this.labelRelOperator.Name = "labelRelOperator";
			componentResourceManager.ApplyResources(this.comboBoxRelOperator, "comboBoxRelOperator");
			this.comboBoxRelOperator.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderGlobalModel.SetError(this.comboBoxRelOperator, componentResourceManager.GetString("comboBoxRelOperator.Error"));
			this.errorProviderFormat.SetError(this.comboBoxRelOperator, componentResourceManager.GetString("comboBoxRelOperator.Error1"));
			this.comboBoxRelOperator.FormattingEnabled = true;
			this.errorProviderFormat.SetIconAlignment(this.comboBoxRelOperator, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxRelOperator.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxRelOperator, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxRelOperator.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxRelOperator, (int)componentResourceManager.GetObject("comboBoxRelOperator.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxRelOperator, (int)componentResourceManager.GetObject("comboBoxRelOperator.IconPadding1"));
			this.comboBoxRelOperator.Name = "comboBoxRelOperator";
			this.comboBoxRelOperator.SelectedIndexChanged += new EventHandler(this.comboBoxRelOperator_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelValue, "labelValue");
			this.errorProviderFormat.SetError(this.labelValue, componentResourceManager.GetString("labelValue.Error"));
			this.errorProviderGlobalModel.SetError(this.labelValue, componentResourceManager.GetString("labelValue.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelValue, (ErrorIconAlignment)componentResourceManager.GetObject("labelValue.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelValue, (ErrorIconAlignment)componentResourceManager.GetObject("labelValue.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelValue, (int)componentResourceManager.GetObject("labelValue.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelValue, (int)componentResourceManager.GetObject("labelValue.IconPadding1"));
			this.labelValue.Name = "labelValue";
			componentResourceManager.ApplyResources(this.textBoxValue, "textBoxValue");
			this.errorProviderGlobalModel.SetError(this.textBoxValue, componentResourceManager.GetString("textBoxValue.Error"));
			this.errorProviderFormat.SetError(this.textBoxValue, componentResourceManager.GetString("textBoxValue.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxValue.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxValue.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxValue, (int)componentResourceManager.GetObject("textBoxValue.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxValue, (int)componentResourceManager.GetObject("textBoxValue.IconPadding1"));
			this.textBoxValue.Name = "textBoxValue";
			this.textBoxValue.Validating += new CancelEventHandler(this.textBoxValue_Validating);
			componentResourceManager.ApplyResources(this.labelUpperLimit, "labelUpperLimit");
			this.errorProviderFormat.SetError(this.labelUpperLimit, componentResourceManager.GetString("labelUpperLimit.Error"));
			this.errorProviderGlobalModel.SetError(this.labelUpperLimit, componentResourceManager.GetString("labelUpperLimit.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelUpperLimit, (ErrorIconAlignment)componentResourceManager.GetObject("labelUpperLimit.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelUpperLimit, (ErrorIconAlignment)componentResourceManager.GetObject("labelUpperLimit.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelUpperLimit, (int)componentResourceManager.GetObject("labelUpperLimit.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelUpperLimit, (int)componentResourceManager.GetObject("labelUpperLimit.IconPadding1"));
			this.labelUpperLimit.Name = "labelUpperLimit";
			componentResourceManager.ApplyResources(this.textBoxUpperValue, "textBoxUpperValue");
			this.errorProviderGlobalModel.SetError(this.textBoxUpperValue, componentResourceManager.GetString("textBoxUpperValue.Error"));
			this.errorProviderFormat.SetError(this.textBoxUpperValue, componentResourceManager.GetString("textBoxUpperValue.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxUpperValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxUpperValue.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxUpperValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxUpperValue.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxUpperValue, (int)componentResourceManager.GetObject("textBoxUpperValue.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxUpperValue, (int)componentResourceManager.GetObject("textBoxUpperValue.IconPadding1"));
			this.textBoxUpperValue.Name = "textBoxUpperValue";
			this.textBoxUpperValue.Validating += new CancelEventHandler(this.textBoxUpperValue_Validating);
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
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderFormat, "errorProviderFormat");
			componentResourceManager.ApplyResources(this.textBoxStartbit, "textBoxStartbit");
			this.errorProviderGlobalModel.SetError(this.textBoxStartbit, componentResourceManager.GetString("textBoxStartbit.Error"));
			this.errorProviderFormat.SetError(this.textBoxStartbit, componentResourceManager.GetString("textBoxStartbit.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxStartbit, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxStartbit.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxStartbit, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxStartbit.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxStartbit, (int)componentResourceManager.GetObject("textBoxStartbit.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxStartbit, (int)componentResourceManager.GetObject("textBoxStartbit.IconPadding1"));
			this.textBoxStartbit.Name = "textBoxStartbit";
			this.textBoxStartbit.Validating += new CancelEventHandler(this.textBoxStartbit_Validating);
			componentResourceManager.ApplyResources(this.textBoxLength, "textBoxLength");
			this.errorProviderGlobalModel.SetError(this.textBoxLength, componentResourceManager.GetString("textBoxLength.Error"));
			this.errorProviderFormat.SetError(this.textBoxLength, componentResourceManager.GetString("textBoxLength.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxLength, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLength.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxLength, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLength.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxLength, (int)componentResourceManager.GetObject("textBoxLength.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxLength, (int)componentResourceManager.GetObject("textBoxLength.IconPadding1"));
			this.textBoxLength.Name = "textBoxLength";
			this.textBoxLength.Validating += new CancelEventHandler(this.textBoxLength_Validating);
			componentResourceManager.ApplyResources(this.comboBoxByteOrder, "comboBoxByteOrder");
			this.comboBoxByteOrder.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderGlobalModel.SetError(this.comboBoxByteOrder, componentResourceManager.GetString("comboBoxByteOrder.Error"));
			this.errorProviderFormat.SetError(this.comboBoxByteOrder, componentResourceManager.GetString("comboBoxByteOrder.Error1"));
			this.comboBoxByteOrder.FormattingEnabled = true;
			this.errorProviderFormat.SetIconAlignment(this.comboBoxByteOrder, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxByteOrder.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxByteOrder, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxByteOrder.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxByteOrder, (int)componentResourceManager.GetObject("comboBoxByteOrder.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxByteOrder, (int)componentResourceManager.GetObject("comboBoxByteOrder.IconPadding1"));
			this.comboBoxByteOrder.Name = "comboBoxByteOrder";
			this.comboBoxByteOrder.SelectedIndexChanged += new EventHandler(this.comboBoxByteOrder_SelectedIndexChanged);
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
			componentResourceManager.ApplyResources(this.radioButtonRawSigStartbit, "radioButtonRawSigStartbit");
			this.errorProviderFormat.SetError(this.radioButtonRawSigStartbit, componentResourceManager.GetString("radioButtonRawSigStartbit.Error"));
			this.errorProviderGlobalModel.SetError(this.radioButtonRawSigStartbit, componentResourceManager.GetString("radioButtonRawSigStartbit.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonRawSigStartbit, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonRawSigStartbit.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonRawSigStartbit, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonRawSigStartbit.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.radioButtonRawSigStartbit, (int)componentResourceManager.GetObject("radioButtonRawSigStartbit.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.radioButtonRawSigStartbit, (int)componentResourceManager.GetObject("radioButtonRawSigStartbit.IconPadding1"));
			this.radioButtonRawSigStartbit.Name = "radioButtonRawSigStartbit";
			this.radioButtonRawSigStartbit.TabStop = true;
			this.radioButtonRawSigStartbit.UseVisualStyleBackColor = true;
			this.radioButtonRawSigStartbit.CheckedChanged += new EventHandler(this.radioButtonRawSigStartbit_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonRawSigByte, "radioButtonRawSigByte");
			this.errorProviderFormat.SetError(this.radioButtonRawSigByte, componentResourceManager.GetString("radioButtonRawSigByte.Error"));
			this.errorProviderGlobalModel.SetError(this.radioButtonRawSigByte, componentResourceManager.GetString("radioButtonRawSigByte.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonRawSigByte, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonRawSigByte.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonRawSigByte, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonRawSigByte.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.radioButtonRawSigByte, (int)componentResourceManager.GetObject("radioButtonRawSigByte.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.radioButtonRawSigByte, (int)componentResourceManager.GetObject("radioButtonRawSigByte.IconPadding1"));
			this.radioButtonRawSigByte.Name = "radioButtonRawSigByte";
			this.radioButtonRawSigByte.TabStop = true;
			this.radioButtonRawSigByte.UseVisualStyleBackColor = true;
			this.radioButtonRawSigByte.CheckedChanged += new EventHandler(this.radioButtonRawSigByte_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelLength, "labelLength");
			this.errorProviderFormat.SetError(this.labelLength, componentResourceManager.GetString("labelLength.Error"));
			this.errorProviderGlobalModel.SetError(this.labelLength, componentResourceManager.GetString("labelLength.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelLength, (ErrorIconAlignment)componentResourceManager.GetObject("labelLength.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelLength, (ErrorIconAlignment)componentResourceManager.GetObject("labelLength.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelLength, (int)componentResourceManager.GetObject("labelLength.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelLength, (int)componentResourceManager.GetObject("labelLength.IconPadding1"));
			this.labelLength.Name = "labelLength";
			componentResourceManager.ApplyResources(this.labelByteOrder, "labelByteOrder");
			this.errorProviderFormat.SetError(this.labelByteOrder, componentResourceManager.GetString("labelByteOrder.Error"));
			this.errorProviderGlobalModel.SetError(this.labelByteOrder, componentResourceManager.GetString("labelByteOrder.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelByteOrder, (ErrorIconAlignment)componentResourceManager.GetObject("labelByteOrder.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelByteOrder, (ErrorIconAlignment)componentResourceManager.GetObject("labelByteOrder.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelByteOrder, (int)componentResourceManager.GetObject("labelByteOrder.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelByteOrder, (int)componentResourceManager.GetObject("labelByteOrder.IconPadding1"));
			this.labelByteOrder.Name = "labelByteOrder";
			componentResourceManager.ApplyResources(this.groupBoxData, "groupBoxData");
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
			this.groupBoxData.Controls.Add(this.comboBoxDataByte);
			this.errorProviderGlobalModel.SetError(this.groupBoxData, componentResourceManager.GetString("groupBoxData.Error"));
			this.errorProviderFormat.SetError(this.groupBoxData, componentResourceManager.GetString("groupBoxData.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.groupBoxData, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxData.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.groupBoxData, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxData.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.groupBoxData, (int)componentResourceManager.GetObject("groupBoxData.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.groupBoxData, (int)componentResourceManager.GetObject("groupBoxData.IconPadding1"));
			this.groupBoxData.Name = "groupBoxData";
			this.groupBoxData.TabStop = false;
			componentResourceManager.ApplyResources(this.groupBoxMessage, "groupBoxMessage");
			this.groupBoxMessage.Controls.Add(this.labelChannel);
			this.groupBoxMessage.Controls.Add(this.comboBoxChannel);
			this.groupBoxMessage.Controls.Add(this.textBoxMessageId);
			this.groupBoxMessage.Controls.Add(this.labelMsgId);
			this.errorProviderGlobalModel.SetError(this.groupBoxMessage, componentResourceManager.GetString("groupBoxMessage.Error"));
			this.errorProviderFormat.SetError(this.groupBoxMessage, componentResourceManager.GetString("groupBoxMessage.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.groupBoxMessage, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxMessage.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.groupBoxMessage, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxMessage.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.groupBoxMessage, (int)componentResourceManager.GetObject("groupBoxMessage.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.groupBoxMessage, (int)componentResourceManager.GetObject("groupBoxMessage.IconPadding1"));
			this.groupBoxMessage.Name = "groupBoxMessage";
			this.groupBoxMessage.TabStop = false;
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
			base.Name = "LINDataCondition";
			base.Shown += new EventHandler(this.LINDataCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.LINDataCondition_HelpRequested);
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
