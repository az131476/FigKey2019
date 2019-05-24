using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.EventConditions
{
	internal class MsgTimeoutCondition : Form
	{
		private MsgTimeoutEvent msgTimeoutEvent;

		private bool isDatabaseAvailable;

		private IModelValidator modelValidator;

		private IApplicationDatabaseManager dbManager;

		private int invalidChannelIndex;

		private IContainer components;

		private GroupBox groupBoxID;

		private ComboBox comboBoxChannel;

		private Label labelChannel;

		private TextBox textBoxRawMsgID;

		private RadioButton radioButtonRawMsg;

		private Button buttonSelectSymMsg;

		private TextBox textBoxMessageName;

		private RadioButton radioButtonSymMsg;

		private GroupBox groupBox1;

		private Label labelMs2;

		private TextBox textBoxCycleTimeUserDefined;

		private Label labelMs1;

		private TextBox textBoxCycleTimeFromDB;

		private RadioButton radioButtonTimeoutUserDefined;

		private RadioButton radioButtonTimeoutFromDB;

		private TextBox textBoxMaxDelay;

		private Label labelMaxDelay;

		private Label labelMs3;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonHelp;

		private GroupBox groupBoxNote;

		private Label labelActivationDelayAfterStart;

		private Label labelMs4;

		private TextBox textBoxTimeout;

		private Label labelResTimout;

		private Label labelEqual;

		private Label labelCycleTime;

		private Label labelNoteGlobalSettings;

		private Label labelNoteOnDelayGreaterCycleTime;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderGlobalModel;

		private ToolTip toolTip;

		private Label labelExtended;

		private Label label1;

		public MsgTimeoutEvent MsgTimeoutEvent
		{
			get
			{
				return this.msgTimeoutEvent;
			}
			set
			{
				this.msgTimeoutEvent = value;
			}
		}

		public MsgTimeoutCondition(IModelValidator modelVal, IApplicationDatabaseManager manager)
		{
			this.InitializeComponent();
			this.modelValidator = modelVal;
			this.dbManager = manager;
			this.msgTimeoutEvent = new MsgTimeoutEvent();
			this.isDatabaseAvailable = false;
			this.invalidChannelIndex = -1;
		}

		private void InitChannelComboBox(BusType type)
		{
			this.comboBoxChannel.Items.Clear();
			this.invalidChannelIndex = -1;
			if (type == BusType.Bt_CAN)
			{
				uint totalNumberOfLogicalChannels = this.modelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN);
				for (uint num = 1u; num <= totalNumberOfLogicalChannels; num += 1u)
				{
					this.comboBoxChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
				}
			}
			else if (type == BusType.Bt_LIN)
			{
				uint totalNumberOfLogicalChannels2 = this.modelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_LIN);
				for (uint num2 = 1u; num2 <= totalNumberOfLogicalChannels2; num2 += 1u)
				{
					this.comboBoxChannel.Items.Add(GUIUtil.MapLINChannelNumber2String(num2, this.modelValidator.LoggerSpecifics));
				}
			}
			this.comboBoxChannel.SelectedIndex = 0;
		}

		public void InitDefaultValues(BusType bustype)
		{
			this.msgTimeoutEvent.MessageName.Value = "";
			this.msgTimeoutEvent.NetworkName.Value = "";
			this.msgTimeoutEvent.DatabaseName.Value = "";
			this.msgTimeoutEvent.DatabasePath.Value = "";
			this.msgTimeoutEvent.BusType.Value = bustype;
			this.msgTimeoutEvent.ChannelNumber.Value = this.modelValidator.GetFirstActiveOrDefaultChannel(bustype);
			this.msgTimeoutEvent.ID.Value = 0u;
			this.msgTimeoutEvent.IsExtendedId.Value = false;
			this.msgTimeoutEvent.IsSymbolic.Value = false;
			this.msgTimeoutEvent.IsCycletimeFromDatabase.Value = false;
			this.msgTimeoutEvent.DatabaseCycleTime = 0u;
			this.msgTimeoutEvent.UserDefinedCycleTime.Value = Constants.DefaultUserDefinedCycleTime;
			this.msgTimeoutEvent.MaxDelay.Value = Constants.DefaultMaxDelay;
			this.msgTimeoutEvent.DatabaseCycleTime = 0u;
			this.textBoxCycleTimeFromDB.Text = "";
		}

		private void MsgTimeoutCondition_Shown(object sender, EventArgs e)
		{
			this.SubscribeControlEvents(false);
			this.InitChannelComboBox(this.msgTimeoutEvent.BusType.Value);
			this.RenderLabelsForDisplayModeAndBusType();
			this.isDatabaseAvailable = this.modelValidator.DatabaseServices.HasDatabasesConfiguredFor(this.msgTimeoutEvent.BusType.Value);
			this.SubscribeControlEvents(true);
			this.ApplyValuesToControls();
			this.labelActivationDelayAfterStart.Text = string.Format(Resources.AfterStartCondActiveAfterMs, this.modelValidator.GetEventActivationDelayAfterStart());
		}

		private void radioButtonSymMsg_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioButtonSymMsg.Checked && !this.isDatabaseAvailable)
			{
				this.radioButtonRawMsg.Checked = true;
				this.radioButtonSymMsg.Checked = false;
				string arg = Vocabulary.CAN;
				if (this.msgTimeoutEvent.BusType.Value == BusType.Bt_LIN)
				{
					arg = Vocabulary.LIN;
				}
				else if (this.msgTimeoutEvent.BusType.Value == BusType.Bt_FlexRay)
				{
					arg = Vocabulary.Flexray;
				}
				InformMessageBox.Info(string.Format(Resources.NoDatabaseAvailableForBustype, arg));
				return;
			}
			this.EnableControlsForSymbolicMessage(this.radioButtonSymMsg.Checked);
			if (this.radioButtonSymMsg.Checked && string.IsNullOrEmpty(this.textBoxMessageName.Text) && this.isDatabaseAvailable)
			{
				this.SelectSymbolicMessage();
			}
			this.ValidateInput();
		}

		private void textBoxMessageName_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateInput();
		}

		private void radioButtonRawMsg_CheckedChanged(object sender, EventArgs e)
		{
			bool @checked = this.radioButtonRawMsg.Checked;
			if (@checked && this.radioButtonTimeoutFromDB.Checked)
			{
				this.SubscribeControlEvents(false);
				this.radioButtonTimeoutFromDB.Checked = false;
				this.radioButtonTimeoutUserDefined.Checked = true;
				this.SubscribeControlEvents(true);
				this.EnableControlsForCycleTimeFromDB(false);
				this.textBoxCycleTimeFromDB.Text = "";
			}
			this.EnableControlsForSymbolicMessage(!@checked);
			this.ValidateInput();
		}

		private void buttonSelectSymMsg_Click(object sender, EventArgs e)
		{
			this.SelectSymbolicMessage();
		}

		private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ValidateInput();
			if (this.invalidChannelIndex >= 0 && this.comboBoxChannel.SelectedIndex != this.invalidChannelIndex)
			{
				this.comboBoxChannel.Items.RemoveAt(this.invalidChannelIndex);
				this.invalidChannelIndex = -1;
			}
		}

		private void textBoxRawMsgID_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateInput();
		}

		private void radioButtonTimeoutFromDB_CheckedChanged(object sender, EventArgs e)
		{
			this.EnableControlsForCycleTimeFromDB(this.radioButtonTimeoutFromDB.Checked);
			this.ValidateInput();
		}

		private void radioButtonTimeoutUserDefined_CheckedChanged(object sender, EventArgs e)
		{
			this.EnableControlsForCycleTimeFromDB(!this.radioButtonTimeoutUserDefined.Checked);
			this.ValidateInput();
		}

		private void textBoxCycleTimeUserDefined_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateInput();
		}

		private void textBoxMaxDelay_Validating(object sender, CancelEventArgs e)
		{
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
			if (!string.IsNullOrEmpty(this.errorProviderGlobalModel.GetError(this.comboBoxChannel)) && InformMessageBox.Question(Resources.InconsistSettingsCommitCondAnyway) == DialogResult.No)
			{
				return;
			}
			if (!this.msgTimeoutEvent.IsSymbolic.Value && !string.IsNullOrEmpty(this.msgTimeoutEvent.MessageName.Value))
			{
				this.msgTimeoutEvent.MessageName.Value = "";
				this.msgTimeoutEvent.NetworkName.Value = "";
				this.msgTimeoutEvent.DatabasePath.Value = "";
				this.msgTimeoutEvent.DatabaseName.Value = "";
			}
			if (this.msgTimeoutEvent.IsSymbolic.Value)
			{
				this.msgTimeoutEvent.ID.Value = 0u;
				this.msgTimeoutEvent.IsExtendedId.Value = false;
			}
			if (this.msgTimeoutEvent.IsCycletimeFromDatabase.Value)
			{
				this.msgTimeoutEvent.UserDefinedCycleTime.Value = Constants.DefaultUserDefinedCycleTime;
			}
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

		private void MsgTimeoutCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private bool ValidateInput()
		{
			this.ResetErrorProviders();
			this.buttonOK.Enabled = true;
			bool result = true;
			this.ValidateChannel();
			this.msgTimeoutEvent.IsSymbolic.Value = this.radioButtonSymMsg.Checked;
			if (this.msgTimeoutEvent.IsSymbolic.Value)
			{
				MessageDefinition messageDefinition;
				if (string.IsNullOrEmpty(this.msgTimeoutEvent.MessageName.Value))
				{
					this.errorProviderFormat.SetError(this.textBoxMessageName, Resources.ErrorNoMsgSymbolSelected);
					result = false;
				}
				else if (!this.modelValidator.DatabaseServices.IsSymbolicMessageDefined(this.msgTimeoutEvent.DatabasePath.Value, this.msgTimeoutEvent.NetworkName.Value, this.msgTimeoutEvent.MessageName.Value, this.msgTimeoutEvent.ChannelNumber.Value, this.msgTimeoutEvent.BusType.Value, out messageDefinition))
				{
					this.errorProviderGlobalModel.SetError(this.textBoxMessageName, Resources.ErrorUnresolvedMsgSymbol);
					this.buttonOK.Enabled = false;
					result = false;
				}
				else
				{
					this.msgTimeoutEvent.IsDatabaseMsgCyclic = messageDefinition.IsCyclic;
					this.msgTimeoutEvent.DatabaseCycleTime = (uint)messageDefinition.CycleTime;
					if (this.msgTimeoutEvent.IsDatabaseMsgCyclic)
					{
						this.textBoxCycleTimeFromDB.Text = this.msgTimeoutEvent.DatabaseCycleTime.ToString();
					}
					else
					{
						this.textBoxCycleTimeFromDB.Text = Resources.None;
					}
				}
			}
			else if (this.msgTimeoutEvent.BusType.Value == BusType.Bt_CAN)
			{
				if (!this.ValidateCANId())
				{
					result = false;
				}
			}
			else if (this.msgTimeoutEvent.BusType.Value == BusType.Bt_LIN && !this.ValidateLINId())
			{
				result = false;
			}
			if (!this.ValidateTimeoutValues())
			{
				result = false;
			}
			return result;
		}

		private bool ValidateChannel()
		{
			bool result = true;
			switch (this.msgTimeoutEvent.BusType.Value)
			{
			case BusType.Bt_CAN:
				this.msgTimeoutEvent.ChannelNumber.Value = GUIUtil.MapCANChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
				break;
			case BusType.Bt_LIN:
				this.msgTimeoutEvent.ChannelNumber.Value = GUIUtil.MapLINChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
				break;
			}
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
			if (!this.modelValidator.IsHardwareChannelAvailable(this.msgTimeoutEvent.BusType.Value, this.msgTimeoutEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.modelValidator.IsHardwareChannelActive(this.msgTimeoutEvent.BusType.Value, this.msgTimeoutEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelInactive);
				result = false;
			}
			return result;
		}

		private bool ValidateTimeoutValues()
		{
			bool flag = true;
			uint num = 0u;
			bool flag2 = false;
			this.textBoxTimeout.Text = Resources.Unknown;
			this.labelNoteOnDelayGreaterCycleTime.Visible = false;
			if (!uint.TryParse(this.textBoxMaxDelay.Text, out num))
			{
				this.errorProviderFormat.SetError(this.textBoxMaxDelay, Resources.ErrorNumberExpected);
				flag = false;
			}
			else if (num > Constants.OnMsgTimeoutMaxResultingDelay)
			{
				this.errorProviderFormat.SetError(this.textBoxMaxDelay, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 0, Constants.OnMsgTimeoutMaxResultingDelay));
				flag = false;
			}
			else
			{
				this.msgTimeoutEvent.MaxDelay.Value = num;
			}
			this.msgTimeoutEvent.IsCycletimeFromDatabase.Value = this.radioButtonTimeoutFromDB.Checked;
			if (!this.msgTimeoutEvent.IsCycletimeFromDatabase.Value)
			{
				if (!uint.TryParse(this.textBoxCycleTimeUserDefined.Text, out num))
				{
					this.errorProviderFormat.SetError(this.textBoxCycleTimeUserDefined, Resources.ErrorNumberExpected);
					flag = false;
				}
				else if (num > Constants.OnMsgTimeoutMaxResultingDelay)
				{
					this.errorProviderFormat.SetError(this.textBoxCycleTimeUserDefined, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 0, Constants.OnMsgTimeoutMaxResultingDelay));
					flag = false;
				}
				else
				{
					this.msgTimeoutEvent.UserDefinedCycleTime.Value = num;
					if (num > 0u)
					{
						flag2 = true;
					}
				}
			}
			else if (!this.HasError(this.textBoxMessageName))
			{
				if (this.msgTimeoutEvent.IsDatabaseMsgCyclic)
				{
					if (this.msgTimeoutEvent.DatabaseCycleTime > 0u && this.msgTimeoutEvent.DatabaseCycleTime < Constants.OnMsgTimeoutMaxResultingDelay)
					{
						flag2 = true;
					}
					else
					{
						this.errorProviderFormat.SetError(this.radioButtonTimeoutFromDB, string.Format(Resources.ErrorCycleTimeInDbOutOfRange, Constants.OnMsgTimeoutMaxResultingDelay));
					}
				}
				else
				{
					this.errorProviderFormat.SetError(this.radioButtonTimeoutFromDB, Resources.ErrorNoCycleTimeDefinedForSym);
					flag = false;
				}
			}
			else
			{
				this.textBoxCycleTimeFromDB.Text = Resources.Unknown;
				flag = false;
			}
			if (flag)
			{
				this.textBoxTimeout.Text = this.msgTimeoutEvent.ResultingTimeout.ToString();
				string value = string.Empty;
				if (this.msgTimeoutEvent.ResultingTimeout > Constants.OnMsgTimeoutMaxResultingDelay)
				{
					value = string.Format(Resources.ErrorResultTimeoutMustNotBeGreater, Constants.OnMsgTimeoutMaxResultingDelay);
				}
				else if (this.msgTimeoutEvent.ResultingTimeout == 0u)
				{
					value = string.Format(Resources.ErrorResultTimoutMustBeGreater0, new object[0]);
				}
				if (!string.IsNullOrEmpty(value))
				{
					this.errorProviderFormat.SetError(this.textBoxTimeout, value);
					this.errorProviderFormat.SetError(this.textBoxMaxDelay, value);
					if (!this.msgTimeoutEvent.IsCycletimeFromDatabase.Value)
					{
						this.errorProviderFormat.SetError(this.textBoxCycleTimeUserDefined, value);
					}
					flag = false;
				}
				if (flag2)
				{
					if (this.msgTimeoutEvent.IsCycletimeFromDatabase.Value)
					{
						this.labelNoteOnDelayGreaterCycleTime.Visible = (this.msgTimeoutEvent.DatabaseCycleTime < this.msgTimeoutEvent.MaxDelay.Value);
					}
					else
					{
						this.labelNoteOnDelayGreaterCycleTime.Visible = (this.msgTimeoutEvent.UserDefinedCycleTime.Value < this.msgTimeoutEvent.MaxDelay.Value);
					}
				}
			}
			else
			{
				this.textBoxTimeout.Text = Resources.Unknown;
			}
			return flag;
		}

		private bool ValidateCANId()
		{
			uint value;
			bool value2;
			if (GUIUtil.DisplayStringToCANId(this.textBoxRawMsgID.Text, out value, out value2))
			{
				this.msgTimeoutEvent.IsExtendedId.Value = value2;
				this.msgTimeoutEvent.ID.Value = value;
				return true;
			}
			this.errorProviderFormat.SetError(this.textBoxRawMsgID, Resources.ErrorCANIdExpected);
			return false;
		}

		private bool ValidateLINId()
		{
			uint value;
			if (GUIUtil.DisplayStringToLINId(this.textBoxRawMsgID.Text, out value))
			{
				this.msgTimeoutEvent.ID.Value = value;
				return true;
			}
			this.errorProviderFormat.SetError(this.textBoxRawMsgID, Resources.ErrorLINIdExpected);
			return false;
		}

		public bool HasError(Control control)
		{
			return !string.IsNullOrEmpty(this.errorProviderFormat.GetError(control)) || !string.IsNullOrEmpty(this.errorProviderGlobalModel.GetError(control));
		}

		private void ResetErrorProviders()
		{
			this.errorProviderFormat.SetError(this.textBoxCycleTimeUserDefined, "");
			this.errorProviderFormat.SetError(this.textBoxTimeout, "");
			this.errorProviderFormat.SetError(this.textBoxMaxDelay, "");
			this.errorProviderFormat.SetError(this.textBoxRawMsgID, "");
			this.errorProviderFormat.SetError(this.textBoxMessageName, "");
			this.errorProviderFormat.SetError(this.radioButtonTimeoutFromDB, "");
			this.errorProviderGlobalModel.SetError(this.textBoxMessageName, "");
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
		}

		private void ApplyValuesToControls()
		{
			this.SubscribeControlEvents(false);
			this.groupBoxNote.Visible = (this.modelValidator.LoggerSpecifics.Type != LoggerType.VN1630log);
			this.labelExtended.Visible = (this.msgTimeoutEvent.BusType.Value == BusType.Bt_CAN);
			this.radioButtonSymMsg.Checked = this.msgTimeoutEvent.IsSymbolic.Value;
			this.radioButtonRawMsg.Checked = !this.msgTimeoutEvent.IsSymbolic.Value;
			this.textBoxMessageName.Text = this.msgTimeoutEvent.MessageName.Value;
			if (!string.IsNullOrEmpty(this.msgTimeoutEvent.NetworkName.Value))
			{
				this.textBoxMessageName.Text = this.msgTimeoutEvent.NetworkName.Value + "::" + this.msgTimeoutEvent.MessageName.Value;
			}
			if (this.msgTimeoutEvent.BusType.Value == BusType.Bt_CAN)
			{
				this.textBoxRawMsgID.Text = GUIUtil.CANIdToDisplayString(this.msgTimeoutEvent.ID.Value, this.msgTimeoutEvent.IsExtendedId.Value);
			}
			else
			{
				this.textBoxRawMsgID.Text = GUIUtil.LINIdToDisplayString(this.msgTimeoutEvent.ID.Value);
			}
			this.EnableControlsForSymbolicMessage(this.msgTimeoutEvent.IsSymbolic.Value);
			this.ApplyChannelNumberToControl();
			this.radioButtonTimeoutFromDB.Checked = this.msgTimeoutEvent.IsCycletimeFromDatabase.Value;
			this.radioButtonTimeoutUserDefined.Checked = !this.msgTimeoutEvent.IsCycletimeFromDatabase.Value;
			if (!this.msgTimeoutEvent.IsSymbolic.Value)
			{
				this.textBoxCycleTimeFromDB.Text = "";
			}
			this.EnableControlsForCycleTimeFromDB(this.msgTimeoutEvent.IsCycletimeFromDatabase.Value);
			this.textBoxCycleTimeUserDefined.Text = this.msgTimeoutEvent.UserDefinedCycleTime.Value.ToString();
			this.textBoxMaxDelay.Text = this.msgTimeoutEvent.MaxDelay.Value.ToString();
			this.SubscribeControlEvents(true);
			this.ValidateInput();
		}

		private void ApplyChannelNumberToControl()
		{
			if (this.msgTimeoutEvent.BusType.Value == BusType.Bt_CAN)
			{
				this.comboBoxChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.msgTimeoutEvent.ChannelNumber.Value);
				return;
			}
			if (this.msgTimeoutEvent.BusType.Value == BusType.Bt_LIN)
			{
				string text = GUIUtil.MapLINChannelNumber2String(this.msgTimeoutEvent.ChannelNumber.Value, this.modelValidator.LoggerSpecifics);
				if (this.comboBoxChannel.Items.Contains(text))
				{
					this.comboBoxChannel.SelectedItem = text;
					return;
				}
				this.invalidChannelIndex = this.comboBoxChannel.Items.Add(text);
				this.comboBoxChannel.SelectedIndex = this.invalidChannelIndex;
			}
		}

		private void EnableControlsForSymbolicMessage(bool isEnabled)
		{
			this.textBoxMessageName.Enabled = isEnabled;
			this.buttonSelectSymMsg.Enabled = (isEnabled && this.isDatabaseAvailable);
			this.textBoxRawMsgID.Enabled = !isEnabled;
			this.radioButtonTimeoutFromDB.Enabled = isEnabled;
		}

		private void EnableControlsForCycleTimeFromDB(bool isEnabled)
		{
			this.textBoxCycleTimeFromDB.Enabled = isEnabled;
			this.textBoxCycleTimeUserDefined.Enabled = !isEnabled;
		}

		private void SelectSymbolicMessage()
		{
			string value = this.msgTimeoutEvent.MessageName.Value;
			string value2 = this.msgTimeoutEvent.DatabaseName.Value;
			string text = this.msgTimeoutEvent.DatabasePath.Value;
			string value3 = this.msgTimeoutEvent.NetworkName.Value;
			BusType value4 = this.msgTimeoutEvent.BusType.Value;
			bool flag = false;
			if (this.dbManager.SelectMessageInDatabase(ref value, ref value2, ref text, ref value3, ref value4, ref flag))
			{
				string message;
				if (!this.modelValidator.DatabaseServices.IsSymbolicMessageInsertAllowed(value, value3, text, value4, out message))
				{
					InformMessageBox.Error(message);
					return;
				}
				text = this.modelValidator.GetFilePathRelativeToConfiguration(text);
				IList<uint> channelAssignmentOfDatabase = this.modelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(text, value3);
				if (!channelAssignmentOfDatabase.Contains(this.msgTimeoutEvent.ChannelNumber.Value))
				{
					this.msgTimeoutEvent.ChannelNumber.Value = channelAssignmentOfDatabase[0];
					this.ApplyChannelNumberToControl();
				}
				this.msgTimeoutEvent.MessageName.Value = value;
				this.msgTimeoutEvent.DatabaseName.Value = value2;
				this.msgTimeoutEvent.DatabasePath.Value = text;
				this.msgTimeoutEvent.NetworkName.Value = value3;
				this.ApplyValuesToControls();
			}
		}

		private void RenderLabelsForDisplayModeAndBusType()
		{
			string arg = Resources.DisplayModeDec;
			if (GUIUtil.IsHexadecimal)
			{
				arg = Resources.DisplayModeHex;
			}
			this.radioButtonRawMsg.Text = string.Format(Resources.RawInputLabelWithMode, arg);
			if (this.msgTimeoutEvent.BusType.Value == BusType.Bt_CAN)
			{
				this.groupBoxID.Text = string.Format(Resources.BusTypeMsg, Vocabulary.CAN);
				return;
			}
			if (this.msgTimeoutEvent.BusType.Value == BusType.Bt_LIN)
			{
				this.groupBoxID.Text = string.Format(Resources.BusTypeMsg, Vocabulary.LIN);
			}
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.radioButtonSymMsg.CheckedChanged += new EventHandler(this.radioButtonSymMsg_CheckedChanged);
				this.textBoxMessageName.Validating += new CancelEventHandler(this.textBoxMessageName_Validating);
				this.radioButtonRawMsg.CheckedChanged += new EventHandler(this.radioButtonRawMsg_CheckedChanged);
				this.textBoxRawMsgID.Validating += new CancelEventHandler(this.textBoxRawMsgID_Validating);
				this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
				this.radioButtonTimeoutFromDB.CheckedChanged += new EventHandler(this.radioButtonTimeoutFromDB_CheckedChanged);
				this.radioButtonTimeoutUserDefined.CheckedChanged += new EventHandler(this.radioButtonTimeoutUserDefined_CheckedChanged);
				this.textBoxMaxDelay.Validating += new CancelEventHandler(this.textBoxMaxDelay_Validating);
				return;
			}
			this.radioButtonSymMsg.CheckedChanged -= new EventHandler(this.radioButtonSymMsg_CheckedChanged);
			this.textBoxMessageName.Validating -= new CancelEventHandler(this.textBoxMessageName_Validating);
			this.radioButtonRawMsg.CheckedChanged -= new EventHandler(this.radioButtonRawMsg_CheckedChanged);
			this.textBoxRawMsgID.Validating -= new CancelEventHandler(this.textBoxRawMsgID_Validating);
			this.comboBoxChannel.SelectedIndexChanged -= new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			this.radioButtonTimeoutFromDB.CheckedChanged -= new EventHandler(this.radioButtonTimeoutFromDB_CheckedChanged);
			this.radioButtonTimeoutUserDefined.CheckedChanged -= new EventHandler(this.radioButtonTimeoutUserDefined_CheckedChanged);
			this.textBoxMaxDelay.Validating -= new CancelEventHandler(this.textBoxMaxDelay_Validating);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MsgTimeoutCondition));
			this.groupBoxID = new GroupBox();
			this.labelExtended = new Label();
			this.comboBoxChannel = new ComboBox();
			this.labelChannel = new Label();
			this.textBoxRawMsgID = new TextBox();
			this.radioButtonRawMsg = new RadioButton();
			this.buttonSelectSymMsg = new Button();
			this.textBoxMessageName = new TextBox();
			this.radioButtonSymMsg = new RadioButton();
			this.groupBox1 = new GroupBox();
			this.label1 = new Label();
			this.labelNoteOnDelayGreaterCycleTime = new Label();
			this.labelEqual = new Label();
			this.labelCycleTime = new Label();
			this.labelMs4 = new Label();
			this.labelMs2 = new Label();
			this.textBoxTimeout = new TextBox();
			this.textBoxCycleTimeUserDefined = new TextBox();
			this.labelResTimout = new Label();
			this.labelMs1 = new Label();
			this.labelMaxDelay = new Label();
			this.labelMs3 = new Label();
			this.textBoxCycleTimeFromDB = new TextBox();
			this.textBoxMaxDelay = new TextBox();
			this.radioButtonTimeoutUserDefined = new RadioButton();
			this.radioButtonTimeoutFromDB = new RadioButton();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.groupBoxNote = new GroupBox();
			this.labelNoteGlobalSettings = new Label();
			this.labelActivationDelayAfterStart = new Label();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.toolTip = new ToolTip(this.components);
			this.groupBoxID.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.groupBoxNote.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.groupBoxID.Controls.Add(this.labelExtended);
			this.groupBoxID.Controls.Add(this.comboBoxChannel);
			this.groupBoxID.Controls.Add(this.labelChannel);
			this.groupBoxID.Controls.Add(this.textBoxRawMsgID);
			this.groupBoxID.Controls.Add(this.radioButtonRawMsg);
			this.groupBoxID.Controls.Add(this.buttonSelectSymMsg);
			this.groupBoxID.Controls.Add(this.textBoxMessageName);
			this.groupBoxID.Controls.Add(this.radioButtonSymMsg);
			componentResourceManager.ApplyResources(this.groupBoxID, "groupBoxID");
			this.groupBoxID.Name = "groupBoxID";
			this.groupBoxID.TabStop = false;
			componentResourceManager.ApplyResources(this.labelExtended, "labelExtended");
			this.labelExtended.Name = "labelExtended";
			this.comboBoxChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxChannel.FormattingEnabled = true;
			this.errorProviderFormat.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment1"));
			componentResourceManager.ApplyResources(this.comboBoxChannel, "comboBoxChannel");
			this.comboBoxChannel.Name = "comboBoxChannel";
			this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelChannel, "labelChannel");
			this.labelChannel.Name = "labelChannel";
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxRawMsgID, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxRawMsgID.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxRawMsgID, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxRawMsgID.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxRawMsgID, "textBoxRawMsgID");
			this.textBoxRawMsgID.Name = "textBoxRawMsgID";
			this.textBoxRawMsgID.Validating += new CancelEventHandler(this.textBoxRawMsgID_Validating);
			componentResourceManager.ApplyResources(this.radioButtonRawMsg, "radioButtonRawMsg");
			this.radioButtonRawMsg.Name = "radioButtonRawMsg";
			this.radioButtonRawMsg.UseVisualStyleBackColor = true;
			this.radioButtonRawMsg.CheckedChanged += new EventHandler(this.radioButtonRawMsg_CheckedChanged);
			componentResourceManager.ApplyResources(this.buttonSelectSymMsg, "buttonSelectSymMsg");
			this.buttonSelectSymMsg.Name = "buttonSelectSymMsg";
			this.buttonSelectSymMsg.UseVisualStyleBackColor = true;
			this.buttonSelectSymMsg.Click += new EventHandler(this.buttonSelectSymMsg_Click);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxMessageName, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMessageName.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxMessageName, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMessageName.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxMessageName, "textBoxMessageName");
			this.textBoxMessageName.Name = "textBoxMessageName";
			this.textBoxMessageName.ReadOnly = true;
			this.textBoxMessageName.Validating += new CancelEventHandler(this.textBoxMessageName_Validating);
			componentResourceManager.ApplyResources(this.radioButtonSymMsg, "radioButtonSymMsg");
			this.radioButtonSymMsg.Name = "radioButtonSymMsg";
			this.radioButtonSymMsg.UseVisualStyleBackColor = true;
			this.radioButtonSymMsg.CheckedChanged += new EventHandler(this.radioButtonSymMsg_CheckedChanged);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.labelNoteOnDelayGreaterCycleTime);
			this.groupBox1.Controls.Add(this.labelEqual);
			this.groupBox1.Controls.Add(this.labelCycleTime);
			this.groupBox1.Controls.Add(this.labelMs4);
			this.groupBox1.Controls.Add(this.labelMs2);
			this.groupBox1.Controls.Add(this.textBoxTimeout);
			this.groupBox1.Controls.Add(this.textBoxCycleTimeUserDefined);
			this.groupBox1.Controls.Add(this.labelResTimout);
			this.groupBox1.Controls.Add(this.labelMs1);
			this.groupBox1.Controls.Add(this.labelMaxDelay);
			this.groupBox1.Controls.Add(this.labelMs3);
			this.groupBox1.Controls.Add(this.textBoxCycleTimeFromDB);
			this.groupBox1.Controls.Add(this.textBoxMaxDelay);
			this.groupBox1.Controls.Add(this.radioButtonTimeoutUserDefined);
			this.groupBox1.Controls.Add(this.radioButtonTimeoutFromDB);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.labelNoteOnDelayGreaterCycleTime, "labelNoteOnDelayGreaterCycleTime");
			this.labelNoteOnDelayGreaterCycleTime.Name = "labelNoteOnDelayGreaterCycleTime";
			componentResourceManager.ApplyResources(this.labelEqual, "labelEqual");
			this.labelEqual.Name = "labelEqual";
			componentResourceManager.ApplyResources(this.labelCycleTime, "labelCycleTime");
			this.labelCycleTime.Name = "labelCycleTime";
			componentResourceManager.ApplyResources(this.labelMs4, "labelMs4");
			this.labelMs4.Name = "labelMs4";
			componentResourceManager.ApplyResources(this.labelMs2, "labelMs2");
			this.labelMs2.Name = "labelMs2";
			componentResourceManager.ApplyResources(this.textBoxTimeout, "textBoxTimeout");
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxTimeout, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxTimeout.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxTimeout, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxTimeout.IconAlignment1"));
			this.textBoxTimeout.Name = "textBoxTimeout";
			this.textBoxTimeout.ReadOnly = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCycleTimeUserDefined, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCycleTimeUserDefined.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCycleTimeUserDefined, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCycleTimeUserDefined.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxCycleTimeUserDefined, "textBoxCycleTimeUserDefined");
			this.textBoxCycleTimeUserDefined.Name = "textBoxCycleTimeUserDefined";
			this.textBoxCycleTimeUserDefined.Validating += new CancelEventHandler(this.textBoxCycleTimeUserDefined_Validating);
			componentResourceManager.ApplyResources(this.labelResTimout, "labelResTimout");
			this.labelResTimout.Name = "labelResTimout";
			componentResourceManager.ApplyResources(this.labelMs1, "labelMs1");
			this.labelMs1.Name = "labelMs1";
			componentResourceManager.ApplyResources(this.labelMaxDelay, "labelMaxDelay");
			this.labelMaxDelay.Name = "labelMaxDelay";
			componentResourceManager.ApplyResources(this.labelMs3, "labelMs3");
			this.labelMs3.Name = "labelMs3";
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCycleTimeFromDB, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCycleTimeFromDB.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCycleTimeFromDB, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCycleTimeFromDB.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxCycleTimeFromDB, "textBoxCycleTimeFromDB");
			this.textBoxCycleTimeFromDB.Name = "textBoxCycleTimeFromDB";
			this.textBoxCycleTimeFromDB.ReadOnly = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxMaxDelay, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMaxDelay.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxMaxDelay, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMaxDelay.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxMaxDelay, "textBoxMaxDelay");
			this.textBoxMaxDelay.Name = "textBoxMaxDelay";
			this.textBoxMaxDelay.Validating += new CancelEventHandler(this.textBoxMaxDelay_Validating);
			componentResourceManager.ApplyResources(this.radioButtonTimeoutUserDefined, "radioButtonTimeoutUserDefined");
			this.radioButtonTimeoutUserDefined.Name = "radioButtonTimeoutUserDefined";
			this.radioButtonTimeoutUserDefined.UseVisualStyleBackColor = true;
			this.radioButtonTimeoutUserDefined.CheckedChanged += new EventHandler(this.radioButtonTimeoutUserDefined_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonTimeoutFromDB, "radioButtonTimeoutFromDB");
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonTimeoutFromDB, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonTimeoutFromDB.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonTimeoutFromDB, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonTimeoutFromDB.IconAlignment1"));
			this.radioButtonTimeoutFromDB.Name = "radioButtonTimeoutFromDB";
			this.radioButtonTimeoutFromDB.UseVisualStyleBackColor = true;
			this.radioButtonTimeoutFromDB.CheckedChanged += new EventHandler(this.radioButtonTimeoutFromDB_CheckedChanged);
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			this.groupBoxNote.Controls.Add(this.labelNoteGlobalSettings);
			this.groupBoxNote.Controls.Add(this.labelActivationDelayAfterStart);
			componentResourceManager.ApplyResources(this.groupBoxNote, "groupBoxNote");
			this.groupBoxNote.Name = "groupBoxNote";
			this.groupBoxNote.TabStop = false;
			componentResourceManager.ApplyResources(this.labelNoteGlobalSettings, "labelNoteGlobalSettings");
			this.labelNoteGlobalSettings.Name = "labelNoteGlobalSettings";
			componentResourceManager.ApplyResources(this.labelActivationDelayAfterStart, "labelActivationDelayAfterStart");
			this.labelActivationDelayAfterStart.Name = "labelActivationDelayAfterStart";
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.groupBoxNote);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.groupBoxID);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "MsgTimeoutCondition";
			base.Shown += new EventHandler(this.MsgTimeoutCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.MsgTimeoutCondition_HelpRequested);
			this.groupBoxID.ResumeLayout(false);
			this.groupBoxID.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBoxNote.ResumeLayout(false);
			this.groupBoxNote.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
