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
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.SendMessagePage
{
	public class ConfigureSendMessage : Form
	{
		private ActionSendMessage actionSendMessage;

		private bool isDatabaseAvailable;

		private IModelValidator modelValidator;

		private IApplicationDatabaseManager dbManager;

		private Dictionary<int, TextBox> posToTextbox;

		private IContainer components;

		private RadioButton radioButtonSymbolic;

		private RadioButton radioButtonRaw;

		private GroupBox groupBoxMessage;

		private TextBox textBoxMessageName;

		private Button buttonSelectSymMsg;

		private TextBox textBoxRawMsgID;

		private ComboBox comboBoxChannel;

		private Label labelChannel;

		private Label labelExtended;

		private CheckBox checkBoxVirtual;

		private GroupBox groupBoxData;

		private TextBox textBoxByte7;

		private TextBox textBoxByte6;

		private TextBox textBoxByte5;

		private TextBox textBoxByte4;

		private TextBox textBoxByte3;

		private TextBox textBoxByte2;

		private TextBox textBoxByte1;

		private TextBox textBoxByte0;

		private Label labelByte0;

		private ComboBox comboBoxDLC;

		private Label labelDLC;

		private Label labelByte7;

		private Label labelByte6;

		private Label labelByte5;

		private Label labelByte4;

		private Label labelByte3;

		private Label labelByte2;

		private Label labelByte1;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonHelp;

		private ErrorProvider errorProviderGlobalModel;

		private ErrorProvider errorProviderFormat;

		private Label labelBytes;

		public ActionSendMessage ActionSendMessage
		{
			get
			{
				return this.actionSendMessage;
			}
			set
			{
				this.actionSendMessage = value;
			}
		}

		public ConfigureSendMessage(IModelValidator modelVal, IApplicationDatabaseManager manager)
		{
			this.InitializeComponent();
			this.modelValidator = modelVal;
			this.dbManager = manager;
			this.InitDLCComboBox();
			this.posToTextbox = new Dictionary<int, TextBox>();
			this.posToTextbox.Add(0, this.textBoxByte0);
			this.posToTextbox.Add(1, this.textBoxByte1);
			this.posToTextbox.Add(2, this.textBoxByte2);
			this.posToTextbox.Add(3, this.textBoxByte3);
			this.posToTextbox.Add(4, this.textBoxByte4);
			this.posToTextbox.Add(5, this.textBoxByte5);
			this.posToTextbox.Add(6, this.textBoxByte6);
			this.posToTextbox.Add(7, this.textBoxByte7);
			this.actionSendMessage = new ActionSendMessage();
		}

		private ConfigureSendMessage()
		{
		}

		public void InitDefaultValues()
		{
			this.actionSendMessage.IsActive.Value = true;
			this.actionSendMessage.IsSymbolic.Value = false;
			this.actionSendMessage.SymbolName.Value = "";
			this.actionSendMessage.NetworkName.Value = "";
			this.actionSendMessage.DatabaseName.Value = "";
			this.actionSendMessage.DatabasePath.Value = "";
			this.actionSendMessage.ID.Value = 0u;
			this.actionSendMessage.IsExtendedId.Value = false;
			this.actionSendMessage.ChannelNumber.Value = this.modelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_CAN);
			this.actionSendMessage.IsVirtual.Value = false;
			this.actionSendMessage.DLC = 8;
			foreach (DataItem current in this.actionSendMessage.MessageData)
			{
				current.Byte.Value = 0;
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
			for (uint num2 = 1u; num2 <= this.modelValidator.LoggerSpecifics.CAN.NumberOfVirtualChannels; num2 += 1u)
			{
				this.comboBoxChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num2 + totalNumberOfLogicalChannels) + Resources.VirtualChannelPostfix);
			}
			this.comboBoxChannel.SelectedIndex = 0;
		}

		private void InitDLCComboBox()
		{
			this.comboBoxDLC.Items.Clear();
			for (int i = 0; i <= 8; i++)
			{
				this.comboBoxDLC.Items.Add(i.ToString());
			}
		}

		private void ConfigureSendMessage_Shown(object sender, EventArgs e)
		{
			this.SubscribeControlEvents(false);
			this.InitChannelComboBox();
			this.isDatabaseAvailable = this.modelValidator.DatabaseServices.HasDatabasesConfiguredFor(BusType.Bt_CAN);
			this.RenderLabelsForDisplayMode();
			this.ApplyValuesToControls();
			this.SubscribeControlEvents(true);
			this.buttonOK.Enabled = this.ValidateInput();
		}

		private void radioButtonSymbolic_CheckedChanged(object sender, EventArgs e)
		{
			this.EnableControlsForSymbolicMessage(this.radioButtonSymbolic.Checked);
			if (this.radioButtonSymbolic.Checked)
			{
				if (string.IsNullOrEmpty(this.textBoxMessageName.Text) && this.isDatabaseAvailable)
				{
					this.SelectSymbolicMessage();
				}
				else
				{
					this.actionSendMessage.IsSymbolic.Value = this.radioButtonSymbolic.Checked;
					this.SubscribeControlEvents(false);
					this.ApplyValuesToControls();
					this.SubscribeControlEvents(true);
				}
			}
			this.ValidateInput();
		}

		private void radioButtonRaw_CheckedChanged(object sender, EventArgs e)
		{
		}

		private void buttonSelectSymMsg_Click(object sender, EventArgs e)
		{
			this.SelectSymbolicMessage();
			this.ValidateInput();
		}

		private void checkBoxVirtual_CheckedChanged(object sender, EventArgs e)
		{
			this.ValidateInput();
		}

		private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!this.EnableVirtualCheckBoxForChannel())
			{
				this.checkBoxVirtual.CheckedChanged -= new EventHandler(this.checkBoxVirtual_CheckedChanged);
				this.checkBoxVirtual.Checked = true;
				this.checkBoxVirtual.CheckedChanged += new EventHandler(this.checkBoxVirtual_CheckedChanged);
			}
			this.ValidateInput();
		}

		private void textBoxRawMsgID_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateInput();
		}

		private void comboBoxDLC_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ValidateInput();
		}

		private void textBoxByte_Validating(object sender, CancelEventArgs e)
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
			if (!this.actionSendMessage.IsSymbolic.Value)
			{
				this.actionSendMessage.SymbolName.Value = "";
				this.actionSendMessage.NetworkName.Value = "";
				this.actionSendMessage.DatabasePath.Value = "";
				this.actionSendMessage.DatabaseName.Value = "";
			}
			else
			{
				this.actionSendMessage.ID.Value = 0u;
				this.actionSendMessage.IsExtendedId.Value = false;
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

		private void ConfigureSendMessage_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private bool ValidateInput()
		{
			this.ResetErrorProviders();
			this.buttonOK.Enabled = true;
			bool result = true;
			int dLC = this.actionSendMessage.DLC;
			this.ValidateChannel();
			this.actionSendMessage.IsSymbolic.Value = this.radioButtonSymbolic.Checked;
			if (this.actionSendMessage.IsSymbolic.Value)
			{
				MessageDefinition messageDefinition;
				if (string.IsNullOrEmpty(this.actionSendMessage.SymbolName.Value))
				{
					this.errorProviderFormat.SetError(this.textBoxMessageName, Resources.ErrorNoMsgSymbolSelected);
					result = false;
				}
				else if (!this.modelValidator.DatabaseServices.IsSymbolicMessageDefined(this.actionSendMessage.DatabasePath.Value, this.actionSendMessage.NetworkName.Value, this.actionSendMessage.SymbolName.Value, this.actionSendMessage.ChannelNumber.Value, this.actionSendMessage.BusType.Value, out messageDefinition))
				{
					this.errorProviderGlobalModel.SetError(this.textBoxMessageName, Resources.ErrorUnresolvedMsgSymbol);
					this.buttonOK.Enabled = false;
					result = false;
				}
				else
				{
					this.actionSendMessage.DLC = (int)messageDefinition.DLC;
					this.comboBoxDLC.SelectedIndexChanged -= new EventHandler(this.comboBoxDLC_SelectedIndexChanged);
					this.comboBoxDLC.SelectedItem = this.actionSendMessage.DLC.ToString();
					this.comboBoxDLC.SelectedIndexChanged += new EventHandler(this.comboBoxDLC_SelectedIndexChanged);
				}
			}
			else
			{
				if (!this.ValidateCANId())
				{
					result = false;
				}
				this.actionSendMessage.DLC = int.Parse(this.comboBoxDLC.SelectedItem.ToString());
			}
			this.actionSendMessage.IsVirtual.Value = this.checkBoxVirtual.Checked;
			this.EnableByteTextboxesForDLC(dLC, this.actionSendMessage.DLC);
			for (int i = 0; i < this.actionSendMessage.DLC; i++)
			{
				uint num;
				if (!GUIUtil.DisplayStringToNumber(this.posToTextbox[i].Text, out num))
				{
					this.errorProviderFormat.SetError(this.posToTextbox[i], Resources.ErrorByteValueExpected);
					result = false;
				}
				else if (num > 255u)
				{
					string arg = "255";
					if (GUIUtil.IsHexadecimal)
					{
						arg = "FF";
					}
					this.errorProviderFormat.SetError(this.posToTextbox[i], string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 0, arg));
					result = false;
				}
				else
				{
					this.actionSendMessage.MessageData[i].Byte.Value = (byte)num;
				}
			}
			return result;
		}

		private bool ValidateCANId()
		{
			uint value;
			bool value2;
			if (GUIUtil.DisplayStringToCANId(this.textBoxRawMsgID.Text, out value, out value2))
			{
				this.actionSendMessage.IsExtendedId.Value = value2;
				this.actionSendMessage.ID.Value = value;
				return true;
			}
			this.errorProviderFormat.SetError(this.textBoxRawMsgID, Resources.ErrorCANIdExpected);
			return false;
		}

		private bool ValidateChannel()
		{
			bool result = true;
			this.actionSendMessage.ChannelNumber.Value = GUIUtil.MapCANChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
			if (this.actionSendMessage.ChannelNumber.Value <= this.modelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN))
			{
				if (!this.modelValidator.IsHardwareChannelAvailable(BusType.Bt_CAN, this.actionSendMessage.ChannelNumber.Value))
				{
					this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelNotAvailable);
					result = false;
				}
				else if (!this.modelValidator.IsHardwareChannelActive(BusType.Bt_CAN, this.actionSendMessage.ChannelNumber.Value))
				{
					this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelInactive);
					result = false;
				}
			}
			return result;
		}

		private void ResetErrorProviders()
		{
			this.errorProviderFormat.SetError(this.textBoxRawMsgID, "");
			for (int i = 0; i < 8; i++)
			{
				this.errorProviderFormat.SetError(this.posToTextbox[i], "");
			}
			this.errorProviderFormat.SetError(this.textBoxMessageName, "");
			this.errorProviderGlobalModel.SetError(this.textBoxMessageName, "");
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
		}

		private void ApplyValuesToControls()
		{
			this.radioButtonSymbolic.Checked = this.actionSendMessage.IsSymbolic.Value;
			this.radioButtonRaw.Checked = !this.actionSendMessage.IsSymbolic.Value;
			this.textBoxMessageName.Text = this.actionSendMessage.SymbolName.Value;
			this.EnableControlsForSymbolicMessage(this.actionSendMessage.IsSymbolic.Value);
			if (this.actionSendMessage.IsSymbolic.Value)
			{
				if (!string.IsNullOrEmpty(this.actionSendMessage.NetworkName.Value))
				{
					this.textBoxMessageName.Text = this.actionSendMessage.NetworkName.Value + "::" + this.actionSendMessage.SymbolName.Value;
				}
				MessageDefinition messageDefinition;
				if (this.dbManager.ResolveMessageSymbolInDatabase(this.modelValidator.GetAbsoluteFilePath(this.actionSendMessage.DatabasePath.Value), this.actionSendMessage.NetworkName.Value, this.actionSendMessage.SymbolName.Value, this.actionSendMessage.BusType.Value, out messageDefinition))
				{
					this.textBoxRawMsgID.Text = GUIUtil.CANIdToDisplayString(messageDefinition.ActualMessageId, messageDefinition.IsExtendedId);
				}
				else
				{
					this.textBoxRawMsgID.Text = Resources.Unknown;
				}
			}
			else
			{
				this.textBoxRawMsgID.Text = GUIUtil.CANIdToDisplayString(this.actionSendMessage.ID.Value, this.actionSendMessage.IsExtendedId.Value);
			}
			string selectedItem;
			if (this.actionSendMessage.ChannelNumber.Value <= this.modelValidator.LoggerSpecifics.CAN.NumberOfChannels)
			{
				selectedItem = GUIUtil.MapCANChannelNumber2String(this.actionSendMessage.ChannelNumber.Value);
			}
			else
			{
				selectedItem = GUIUtil.MapCANChannelNumber2String(this.actionSendMessage.ChannelNumber.Value) + Resources.VirtualChannelPostfix;
			}
			this.comboBoxChannel.SelectedItem = selectedItem;
			this.EnableVirtualCheckBoxForChannel();
			this.checkBoxVirtual.Checked = this.actionSendMessage.IsVirtual.Value;
			this.comboBoxDLC.SelectedItem = this.actionSendMessage.DLC.ToString();
			this.EnableByteTextboxesForDLC(this.actionSendMessage.DLC, this.actionSendMessage.DLC);
			for (int i = 0; i < this.actionSendMessage.DLC; i++)
			{
				this.posToTextbox[i].Text = GUIUtil.NumberToDisplayString((uint)this.actionSendMessage.MessageData[i].Byte.Value);
			}
		}

		private void EnableControlsForSymbolicMessage(bool isEnabled)
		{
			this.textBoxMessageName.Enabled = isEnabled;
			this.buttonSelectSymMsg.Enabled = (isEnabled && this.isDatabaseAvailable);
			this.textBoxRawMsgID.ReadOnly = isEnabled;
			this.comboBoxDLC.Enabled = !isEnabled;
		}

		private bool EnableVirtualCheckBoxForChannel()
		{
			uint num = GUIUtil.MapCANChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
			if (num > this.modelValidator.LoggerSpecifics.CAN.NumberOfChannels)
			{
				this.checkBoxVirtual.Enabled = false;
				return false;
			}
			this.checkBoxVirtual.Enabled = true;
			return true;
		}

		private void EnableByteTextboxesForDLC(int oldDLC, int newDLC)
		{
			for (int i = 0; i < 8; i++)
			{
				if (i < newDLC)
				{
					this.posToTextbox[i].Enabled = true;
					if (i >= oldDLC)
					{
						this.posToTextbox[i].Text = GUIUtil.NumberToDisplayString((int)this.actionSendMessage.MessageData[i].Byte.Value);
					}
				}
				else
				{
					this.posToTextbox[i].Enabled = false;
					this.posToTextbox[i].Text = "";
				}
			}
		}

		private void SelectSymbolicMessage()
		{
			string text = "";
			string value = "";
			string text2 = "";
			string text3 = "";
			BusType busType = BusType.Bt_CAN;
			if (this.actionSendMessage.IsSymbolic.Value)
			{
				text = this.actionSendMessage.SymbolName.Value;
				value = this.actionSendMessage.DatabaseName.Value;
				text2 = this.actionSendMessage.DatabasePath.Value;
				text3 = this.actionSendMessage.NetworkName.Value;
				busType = this.actionSendMessage.BusType.Value;
			}
			bool flag = false;
			if (this.dbManager.SelectMessageInDatabase(ref text, ref value, ref text2, ref text3, ref busType, ref flag))
			{
				string message;
				if (!this.modelValidator.DatabaseServices.IsSymbolicMessageInsertAllowed(text, text3, text2, busType, out message))
				{
					InformMessageBox.Error(message);
					return;
				}
				text2 = this.modelValidator.GetFilePathRelativeToConfiguration(text2);
				IList<uint> channelAssignmentOfDatabase = this.modelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(text2, text3);
				if (channelAssignmentOfDatabase.Count > 0)
				{
					this.actionSendMessage.ChannelNumber.Value = channelAssignmentOfDatabase[0];
				}
				this.actionSendMessage.SymbolName.Value = text;
				this.actionSendMessage.DatabaseName.Value = value;
				this.actionSendMessage.DatabasePath.Value = text2;
				this.actionSendMessage.NetworkName.Value = text3;
				this.actionSendMessage.IsSymbolic.Value = true;
				this.SubscribeControlEvents(false);
				this.ApplyValuesToControls();
				this.SubscribeControlEvents(true);
			}
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.radioButtonSymbolic.CheckedChanged += new EventHandler(this.radioButtonSymbolic_CheckedChanged);
				this.radioButtonRaw.CheckedChanged += new EventHandler(this.radioButtonRaw_CheckedChanged);
				this.textBoxRawMsgID.Validating += new CancelEventHandler(this.textBoxRawMsgID_Validating);
				this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
				this.checkBoxVirtual.CheckedChanged += new EventHandler(this.checkBoxVirtual_CheckedChanged);
				this.comboBoxDLC.SelectedIndexChanged += new EventHandler(this.comboBoxDLC_SelectedIndexChanged);
				this.textBoxByte0.Validating += new CancelEventHandler(this.textBoxByte_Validating);
				this.textBoxByte1.Validating += new CancelEventHandler(this.textBoxByte_Validating);
				this.textBoxByte2.Validating += new CancelEventHandler(this.textBoxByte_Validating);
				this.textBoxByte3.Validating += new CancelEventHandler(this.textBoxByte_Validating);
				this.textBoxByte4.Validating += new CancelEventHandler(this.textBoxByte_Validating);
				this.textBoxByte5.Validating += new CancelEventHandler(this.textBoxByte_Validating);
				this.textBoxByte6.Validating += new CancelEventHandler(this.textBoxByte_Validating);
				this.textBoxByte7.Validating += new CancelEventHandler(this.textBoxByte_Validating);
				return;
			}
			this.radioButtonSymbolic.CheckedChanged -= new EventHandler(this.radioButtonSymbolic_CheckedChanged);
			this.radioButtonRaw.CheckedChanged -= new EventHandler(this.radioButtonRaw_CheckedChanged);
			this.textBoxRawMsgID.Validating -= new CancelEventHandler(this.textBoxRawMsgID_Validating);
			this.comboBoxChannel.SelectedIndexChanged -= new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			this.checkBoxVirtual.CheckedChanged -= new EventHandler(this.checkBoxVirtual_CheckedChanged);
			this.comboBoxDLC.SelectedIndexChanged -= new EventHandler(this.comboBoxDLC_SelectedIndexChanged);
			this.textBoxByte0.Validating -= new CancelEventHandler(this.textBoxByte_Validating);
			this.textBoxByte1.Validating -= new CancelEventHandler(this.textBoxByte_Validating);
			this.textBoxByte2.Validating -= new CancelEventHandler(this.textBoxByte_Validating);
			this.textBoxByte3.Validating -= new CancelEventHandler(this.textBoxByte_Validating);
			this.textBoxByte4.Validating -= new CancelEventHandler(this.textBoxByte_Validating);
			this.textBoxByte5.Validating -= new CancelEventHandler(this.textBoxByte_Validating);
			this.textBoxByte6.Validating -= new CancelEventHandler(this.textBoxByte_Validating);
			this.textBoxByte7.Validating -= new CancelEventHandler(this.textBoxByte_Validating);
		}

		private void RenderLabelsForDisplayMode()
		{
			string arg = Resources.DisplayModeDec;
			if (GUIUtil.IsHexadecimal)
			{
				arg = Resources.DisplayModeHex;
			}
			this.radioButtonRaw.Text = string.Format(Resources.RawInputLabelWithMode, arg);
			this.labelBytes.Text = string.Format(Vocabulary.BytesWithMode, arg);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ConfigureSendMessage));
			this.radioButtonSymbolic = new RadioButton();
			this.radioButtonRaw = new RadioButton();
			this.groupBoxMessage = new GroupBox();
			this.checkBoxVirtual = new CheckBox();
			this.labelExtended = new Label();
			this.comboBoxChannel = new ComboBox();
			this.labelChannel = new Label();
			this.textBoxRawMsgID = new TextBox();
			this.buttonSelectSymMsg = new Button();
			this.textBoxMessageName = new TextBox();
			this.groupBoxData = new GroupBox();
			this.labelBytes = new Label();
			this.labelByte7 = new Label();
			this.labelByte6 = new Label();
			this.labelByte5 = new Label();
			this.labelByte4 = new Label();
			this.labelByte3 = new Label();
			this.labelByte2 = new Label();
			this.labelByte1 = new Label();
			this.textBoxByte7 = new TextBox();
			this.textBoxByte6 = new TextBox();
			this.textBoxByte5 = new TextBox();
			this.textBoxByte4 = new TextBox();
			this.textBoxByte3 = new TextBox();
			this.textBoxByte2 = new TextBox();
			this.textBoxByte1 = new TextBox();
			this.textBoxByte0 = new TextBox();
			this.labelByte0 = new Label();
			this.comboBoxDLC = new ComboBox();
			this.labelDLC = new Label();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.groupBoxMessage.SuspendLayout();
			this.groupBoxData.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.radioButtonSymbolic, "radioButtonSymbolic");
			this.errorProviderFormat.SetError(this.radioButtonSymbolic, componentResourceManager.GetString("radioButtonSymbolic.Error"));
			this.errorProviderGlobalModel.SetError(this.radioButtonSymbolic, componentResourceManager.GetString("radioButtonSymbolic.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonSymbolic, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonSymbolic.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonSymbolic, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonSymbolic.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.radioButtonSymbolic, (int)componentResourceManager.GetObject("radioButtonSymbolic.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.radioButtonSymbolic, (int)componentResourceManager.GetObject("radioButtonSymbolic.IconPadding1"));
			this.radioButtonSymbolic.Name = "radioButtonSymbolic";
			this.radioButtonSymbolic.UseVisualStyleBackColor = true;
			this.radioButtonSymbolic.CheckedChanged += new EventHandler(this.radioButtonSymbolic_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonRaw, "radioButtonRaw");
			this.radioButtonRaw.Checked = true;
			this.errorProviderFormat.SetError(this.radioButtonRaw, componentResourceManager.GetString("radioButtonRaw.Error"));
			this.errorProviderGlobalModel.SetError(this.radioButtonRaw, componentResourceManager.GetString("radioButtonRaw.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonRaw, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonRaw.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonRaw, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonRaw.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.radioButtonRaw, (int)componentResourceManager.GetObject("radioButtonRaw.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.radioButtonRaw, (int)componentResourceManager.GetObject("radioButtonRaw.IconPadding1"));
			this.radioButtonRaw.Name = "radioButtonRaw";
			this.radioButtonRaw.TabStop = true;
			this.radioButtonRaw.UseVisualStyleBackColor = true;
			this.radioButtonRaw.CheckedChanged += new EventHandler(this.radioButtonRaw_CheckedChanged);
			componentResourceManager.ApplyResources(this.groupBoxMessage, "groupBoxMessage");
			this.groupBoxMessage.Controls.Add(this.checkBoxVirtual);
			this.groupBoxMessage.Controls.Add(this.labelExtended);
			this.groupBoxMessage.Controls.Add(this.comboBoxChannel);
			this.groupBoxMessage.Controls.Add(this.labelChannel);
			this.groupBoxMessage.Controls.Add(this.textBoxRawMsgID);
			this.groupBoxMessage.Controls.Add(this.buttonSelectSymMsg);
			this.groupBoxMessage.Controls.Add(this.textBoxMessageName);
			this.groupBoxMessage.Controls.Add(this.radioButtonRaw);
			this.groupBoxMessage.Controls.Add(this.radioButtonSymbolic);
			this.errorProviderGlobalModel.SetError(this.groupBoxMessage, componentResourceManager.GetString("groupBoxMessage.Error"));
			this.errorProviderFormat.SetError(this.groupBoxMessage, componentResourceManager.GetString("groupBoxMessage.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.groupBoxMessage, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxMessage.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.groupBoxMessage, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxMessage.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.groupBoxMessage, (int)componentResourceManager.GetObject("groupBoxMessage.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.groupBoxMessage, (int)componentResourceManager.GetObject("groupBoxMessage.IconPadding1"));
			this.groupBoxMessage.Name = "groupBoxMessage";
			this.groupBoxMessage.TabStop = false;
			componentResourceManager.ApplyResources(this.checkBoxVirtual, "checkBoxVirtual");
			this.errorProviderFormat.SetError(this.checkBoxVirtual, componentResourceManager.GetString("checkBoxVirtual.Error"));
			this.errorProviderGlobalModel.SetError(this.checkBoxVirtual, componentResourceManager.GetString("checkBoxVirtual.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxVirtual, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxVirtual.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxVirtual, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxVirtual.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxVirtual, (int)componentResourceManager.GetObject("checkBoxVirtual.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxVirtual, (int)componentResourceManager.GetObject("checkBoxVirtual.IconPadding1"));
			this.checkBoxVirtual.Name = "checkBoxVirtual";
			this.checkBoxVirtual.UseVisualStyleBackColor = true;
			this.checkBoxVirtual.CheckedChanged += new EventHandler(this.checkBoxVirtual_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelExtended, "labelExtended");
			this.errorProviderFormat.SetError(this.labelExtended, componentResourceManager.GetString("labelExtended.Error"));
			this.errorProviderGlobalModel.SetError(this.labelExtended, componentResourceManager.GetString("labelExtended.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelExtended, (ErrorIconAlignment)componentResourceManager.GetObject("labelExtended.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelExtended, (ErrorIconAlignment)componentResourceManager.GetObject("labelExtended.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelExtended, (int)componentResourceManager.GetObject("labelExtended.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelExtended, (int)componentResourceManager.GetObject("labelExtended.IconPadding1"));
			this.labelExtended.Name = "labelExtended";
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
			componentResourceManager.ApplyResources(this.textBoxRawMsgID, "textBoxRawMsgID");
			this.errorProviderGlobalModel.SetError(this.textBoxRawMsgID, componentResourceManager.GetString("textBoxRawMsgID.Error"));
			this.errorProviderFormat.SetError(this.textBoxRawMsgID, componentResourceManager.GetString("textBoxRawMsgID.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxRawMsgID, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxRawMsgID.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxRawMsgID, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxRawMsgID.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxRawMsgID, (int)componentResourceManager.GetObject("textBoxRawMsgID.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxRawMsgID, (int)componentResourceManager.GetObject("textBoxRawMsgID.IconPadding1"));
			this.textBoxRawMsgID.Name = "textBoxRawMsgID";
			this.textBoxRawMsgID.Validating += new CancelEventHandler(this.textBoxRawMsgID_Validating);
			componentResourceManager.ApplyResources(this.buttonSelectSymMsg, "buttonSelectSymMsg");
			this.errorProviderFormat.SetError(this.buttonSelectSymMsg, componentResourceManager.GetString("buttonSelectSymMsg.Error"));
			this.errorProviderGlobalModel.SetError(this.buttonSelectSymMsg, componentResourceManager.GetString("buttonSelectSymMsg.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.buttonSelectSymMsg, (ErrorIconAlignment)componentResourceManager.GetObject("buttonSelectSymMsg.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonSelectSymMsg, (ErrorIconAlignment)componentResourceManager.GetObject("buttonSelectSymMsg.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.buttonSelectSymMsg, (int)componentResourceManager.GetObject("buttonSelectSymMsg.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonSelectSymMsg, (int)componentResourceManager.GetObject("buttonSelectSymMsg.IconPadding1"));
			this.buttonSelectSymMsg.Name = "buttonSelectSymMsg";
			this.buttonSelectSymMsg.UseVisualStyleBackColor = true;
			this.buttonSelectSymMsg.Click += new EventHandler(this.buttonSelectSymMsg_Click);
			componentResourceManager.ApplyResources(this.textBoxMessageName, "textBoxMessageName");
			this.errorProviderGlobalModel.SetError(this.textBoxMessageName, componentResourceManager.GetString("textBoxMessageName.Error"));
			this.errorProviderFormat.SetError(this.textBoxMessageName, componentResourceManager.GetString("textBoxMessageName.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxMessageName, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMessageName.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxMessageName, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMessageName.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxMessageName, (int)componentResourceManager.GetObject("textBoxMessageName.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxMessageName, (int)componentResourceManager.GetObject("textBoxMessageName.IconPadding1"));
			this.textBoxMessageName.Name = "textBoxMessageName";
			this.textBoxMessageName.ReadOnly = true;
			componentResourceManager.ApplyResources(this.groupBoxData, "groupBoxData");
			this.groupBoxData.Controls.Add(this.labelBytes);
			this.groupBoxData.Controls.Add(this.labelByte7);
			this.groupBoxData.Controls.Add(this.labelByte6);
			this.groupBoxData.Controls.Add(this.labelByte5);
			this.groupBoxData.Controls.Add(this.labelByte4);
			this.groupBoxData.Controls.Add(this.labelByte3);
			this.groupBoxData.Controls.Add(this.labelByte2);
			this.groupBoxData.Controls.Add(this.labelByte1);
			this.groupBoxData.Controls.Add(this.textBoxByte7);
			this.groupBoxData.Controls.Add(this.textBoxByte6);
			this.groupBoxData.Controls.Add(this.textBoxByte5);
			this.groupBoxData.Controls.Add(this.textBoxByte4);
			this.groupBoxData.Controls.Add(this.textBoxByte3);
			this.groupBoxData.Controls.Add(this.textBoxByte2);
			this.groupBoxData.Controls.Add(this.textBoxByte1);
			this.groupBoxData.Controls.Add(this.textBoxByte0);
			this.groupBoxData.Controls.Add(this.labelByte0);
			this.groupBoxData.Controls.Add(this.comboBoxDLC);
			this.groupBoxData.Controls.Add(this.labelDLC);
			this.errorProviderGlobalModel.SetError(this.groupBoxData, componentResourceManager.GetString("groupBoxData.Error"));
			this.errorProviderFormat.SetError(this.groupBoxData, componentResourceManager.GetString("groupBoxData.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.groupBoxData, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxData.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.groupBoxData, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxData.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.groupBoxData, (int)componentResourceManager.GetObject("groupBoxData.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.groupBoxData, (int)componentResourceManager.GetObject("groupBoxData.IconPadding1"));
			this.groupBoxData.Name = "groupBoxData";
			this.groupBoxData.TabStop = false;
			componentResourceManager.ApplyResources(this.labelBytes, "labelBytes");
			this.errorProviderFormat.SetError(this.labelBytes, componentResourceManager.GetString("labelBytes.Error"));
			this.errorProviderGlobalModel.SetError(this.labelBytes, componentResourceManager.GetString("labelBytes.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelBytes, (ErrorIconAlignment)componentResourceManager.GetObject("labelBytes.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelBytes, (ErrorIconAlignment)componentResourceManager.GetObject("labelBytes.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelBytes, (int)componentResourceManager.GetObject("labelBytes.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelBytes, (int)componentResourceManager.GetObject("labelBytes.IconPadding1"));
			this.labelBytes.Name = "labelBytes";
			componentResourceManager.ApplyResources(this.labelByte7, "labelByte7");
			this.errorProviderFormat.SetError(this.labelByte7, componentResourceManager.GetString("labelByte7.Error"));
			this.errorProviderGlobalModel.SetError(this.labelByte7, componentResourceManager.GetString("labelByte7.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelByte7, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte7.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelByte7, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte7.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelByte7, (int)componentResourceManager.GetObject("labelByte7.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelByte7, (int)componentResourceManager.GetObject("labelByte7.IconPadding1"));
			this.labelByte7.Name = "labelByte7";
			componentResourceManager.ApplyResources(this.labelByte6, "labelByte6");
			this.errorProviderFormat.SetError(this.labelByte6, componentResourceManager.GetString("labelByte6.Error"));
			this.errorProviderGlobalModel.SetError(this.labelByte6, componentResourceManager.GetString("labelByte6.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelByte6, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte6.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelByte6, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte6.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelByte6, (int)componentResourceManager.GetObject("labelByte6.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelByte6, (int)componentResourceManager.GetObject("labelByte6.IconPadding1"));
			this.labelByte6.Name = "labelByte6";
			componentResourceManager.ApplyResources(this.labelByte5, "labelByte5");
			this.errorProviderFormat.SetError(this.labelByte5, componentResourceManager.GetString("labelByte5.Error"));
			this.errorProviderGlobalModel.SetError(this.labelByte5, componentResourceManager.GetString("labelByte5.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelByte5, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte5.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelByte5, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte5.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelByte5, (int)componentResourceManager.GetObject("labelByte5.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelByte5, (int)componentResourceManager.GetObject("labelByte5.IconPadding1"));
			this.labelByte5.Name = "labelByte5";
			componentResourceManager.ApplyResources(this.labelByte4, "labelByte4");
			this.errorProviderFormat.SetError(this.labelByte4, componentResourceManager.GetString("labelByte4.Error"));
			this.errorProviderGlobalModel.SetError(this.labelByte4, componentResourceManager.GetString("labelByte4.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelByte4, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte4.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelByte4, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte4.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelByte4, (int)componentResourceManager.GetObject("labelByte4.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelByte4, (int)componentResourceManager.GetObject("labelByte4.IconPadding1"));
			this.labelByte4.Name = "labelByte4";
			componentResourceManager.ApplyResources(this.labelByte3, "labelByte3");
			this.errorProviderFormat.SetError(this.labelByte3, componentResourceManager.GetString("labelByte3.Error"));
			this.errorProviderGlobalModel.SetError(this.labelByte3, componentResourceManager.GetString("labelByte3.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelByte3, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte3.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelByte3, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte3.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelByte3, (int)componentResourceManager.GetObject("labelByte3.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelByte3, (int)componentResourceManager.GetObject("labelByte3.IconPadding1"));
			this.labelByte3.Name = "labelByte3";
			componentResourceManager.ApplyResources(this.labelByte2, "labelByte2");
			this.errorProviderFormat.SetError(this.labelByte2, componentResourceManager.GetString("labelByte2.Error"));
			this.errorProviderGlobalModel.SetError(this.labelByte2, componentResourceManager.GetString("labelByte2.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelByte2, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte2.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelByte2, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte2.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelByte2, (int)componentResourceManager.GetObject("labelByte2.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelByte2, (int)componentResourceManager.GetObject("labelByte2.IconPadding1"));
			this.labelByte2.Name = "labelByte2";
			componentResourceManager.ApplyResources(this.labelByte1, "labelByte1");
			this.errorProviderFormat.SetError(this.labelByte1, componentResourceManager.GetString("labelByte1.Error"));
			this.errorProviderGlobalModel.SetError(this.labelByte1, componentResourceManager.GetString("labelByte1.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelByte1, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte1.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelByte1, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte1.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelByte1, (int)componentResourceManager.GetObject("labelByte1.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelByte1, (int)componentResourceManager.GetObject("labelByte1.IconPadding1"));
			this.labelByte1.Name = "labelByte1";
			componentResourceManager.ApplyResources(this.textBoxByte7, "textBoxByte7");
			this.errorProviderGlobalModel.SetError(this.textBoxByte7, componentResourceManager.GetString("textBoxByte7.Error"));
			this.errorProviderFormat.SetError(this.textBoxByte7, componentResourceManager.GetString("textBoxByte7.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxByte7, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte7.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxByte7, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte7.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxByte7, (int)componentResourceManager.GetObject("textBoxByte7.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxByte7, (int)componentResourceManager.GetObject("textBoxByte7.IconPadding1"));
			this.textBoxByte7.Name = "textBoxByte7";
			componentResourceManager.ApplyResources(this.textBoxByte6, "textBoxByte6");
			this.errorProviderGlobalModel.SetError(this.textBoxByte6, componentResourceManager.GetString("textBoxByte6.Error"));
			this.errorProviderFormat.SetError(this.textBoxByte6, componentResourceManager.GetString("textBoxByte6.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxByte6, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte6.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxByte6, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte6.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxByte6, (int)componentResourceManager.GetObject("textBoxByte6.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxByte6, (int)componentResourceManager.GetObject("textBoxByte6.IconPadding1"));
			this.textBoxByte6.Name = "textBoxByte6";
			this.textBoxByte6.Validating += new CancelEventHandler(this.textBoxByte_Validating);
			componentResourceManager.ApplyResources(this.textBoxByte5, "textBoxByte5");
			this.errorProviderGlobalModel.SetError(this.textBoxByte5, componentResourceManager.GetString("textBoxByte5.Error"));
			this.errorProviderFormat.SetError(this.textBoxByte5, componentResourceManager.GetString("textBoxByte5.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxByte5, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte5.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxByte5, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte5.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxByte5, (int)componentResourceManager.GetObject("textBoxByte5.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxByte5, (int)componentResourceManager.GetObject("textBoxByte5.IconPadding1"));
			this.textBoxByte5.Name = "textBoxByte5";
			this.textBoxByte5.Validating += new CancelEventHandler(this.textBoxByte_Validating);
			componentResourceManager.ApplyResources(this.textBoxByte4, "textBoxByte4");
			this.errorProviderGlobalModel.SetError(this.textBoxByte4, componentResourceManager.GetString("textBoxByte4.Error"));
			this.errorProviderFormat.SetError(this.textBoxByte4, componentResourceManager.GetString("textBoxByte4.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxByte4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte4.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxByte4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte4.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxByte4, (int)componentResourceManager.GetObject("textBoxByte4.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxByte4, (int)componentResourceManager.GetObject("textBoxByte4.IconPadding1"));
			this.textBoxByte4.Name = "textBoxByte4";
			this.textBoxByte4.Validating += new CancelEventHandler(this.textBoxByte_Validating);
			componentResourceManager.ApplyResources(this.textBoxByte3, "textBoxByte3");
			this.errorProviderGlobalModel.SetError(this.textBoxByte3, componentResourceManager.GetString("textBoxByte3.Error"));
			this.errorProviderFormat.SetError(this.textBoxByte3, componentResourceManager.GetString("textBoxByte3.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxByte3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte3.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxByte3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte3.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxByte3, (int)componentResourceManager.GetObject("textBoxByte3.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxByte3, (int)componentResourceManager.GetObject("textBoxByte3.IconPadding1"));
			this.textBoxByte3.Name = "textBoxByte3";
			this.textBoxByte3.Validating += new CancelEventHandler(this.textBoxByte_Validating);
			componentResourceManager.ApplyResources(this.textBoxByte2, "textBoxByte2");
			this.errorProviderGlobalModel.SetError(this.textBoxByte2, componentResourceManager.GetString("textBoxByte2.Error"));
			this.errorProviderFormat.SetError(this.textBoxByte2, componentResourceManager.GetString("textBoxByte2.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxByte2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte2.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxByte2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte2.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxByte2, (int)componentResourceManager.GetObject("textBoxByte2.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxByte2, (int)componentResourceManager.GetObject("textBoxByte2.IconPadding1"));
			this.textBoxByte2.Name = "textBoxByte2";
			this.textBoxByte2.Validating += new CancelEventHandler(this.textBoxByte_Validating);
			componentResourceManager.ApplyResources(this.textBoxByte1, "textBoxByte1");
			this.errorProviderGlobalModel.SetError(this.textBoxByte1, componentResourceManager.GetString("textBoxByte1.Error"));
			this.errorProviderFormat.SetError(this.textBoxByte1, componentResourceManager.GetString("textBoxByte1.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxByte1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte1.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxByte1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte1.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxByte1, (int)componentResourceManager.GetObject("textBoxByte1.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxByte1, (int)componentResourceManager.GetObject("textBoxByte1.IconPadding1"));
			this.textBoxByte1.Name = "textBoxByte1";
			this.textBoxByte1.Validating += new CancelEventHandler(this.textBoxByte_Validating);
			componentResourceManager.ApplyResources(this.textBoxByte0, "textBoxByte0");
			this.errorProviderGlobalModel.SetError(this.textBoxByte0, componentResourceManager.GetString("textBoxByte0.Error"));
			this.errorProviderFormat.SetError(this.textBoxByte0, componentResourceManager.GetString("textBoxByte0.Error1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxByte0, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte0.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxByte0, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxByte0.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxByte0, (int)componentResourceManager.GetObject("textBoxByte0.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.textBoxByte0, (int)componentResourceManager.GetObject("textBoxByte0.IconPadding1"));
			this.textBoxByte0.Name = "textBoxByte0";
			this.textBoxByte0.Validating += new CancelEventHandler(this.textBoxByte_Validating);
			componentResourceManager.ApplyResources(this.labelByte0, "labelByte0");
			this.errorProviderFormat.SetError(this.labelByte0, componentResourceManager.GetString("labelByte0.Error"));
			this.errorProviderGlobalModel.SetError(this.labelByte0, componentResourceManager.GetString("labelByte0.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelByte0, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte0.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelByte0, (ErrorIconAlignment)componentResourceManager.GetObject("labelByte0.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelByte0, (int)componentResourceManager.GetObject("labelByte0.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelByte0, (int)componentResourceManager.GetObject("labelByte0.IconPadding1"));
			this.labelByte0.Name = "labelByte0";
			componentResourceManager.ApplyResources(this.comboBoxDLC, "comboBoxDLC");
			this.comboBoxDLC.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderGlobalModel.SetError(this.comboBoxDLC, componentResourceManager.GetString("comboBoxDLC.Error"));
			this.errorProviderFormat.SetError(this.comboBoxDLC, componentResourceManager.GetString("comboBoxDLC.Error1"));
			this.comboBoxDLC.FormattingEnabled = true;
			this.errorProviderFormat.SetIconAlignment(this.comboBoxDLC, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDLC.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxDLC, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDLC.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxDLC, (int)componentResourceManager.GetObject("comboBoxDLC.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxDLC, (int)componentResourceManager.GetObject("comboBoxDLC.IconPadding1"));
			this.comboBoxDLC.Name = "comboBoxDLC";
			this.comboBoxDLC.SelectedIndexChanged += new EventHandler(this.comboBoxDLC_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelDLC, "labelDLC");
			this.errorProviderFormat.SetError(this.labelDLC, componentResourceManager.GetString("labelDLC.Error"));
			this.errorProviderGlobalModel.SetError(this.labelDLC, componentResourceManager.GetString("labelDLC.Error1"));
			this.errorProviderFormat.SetIconAlignment(this.labelDLC, (ErrorIconAlignment)componentResourceManager.GetObject("labelDLC.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelDLC, (ErrorIconAlignment)componentResourceManager.GetObject("labelDLC.IconAlignment1"));
			this.errorProviderFormat.SetIconPadding(this.labelDLC, (int)componentResourceManager.GetObject("labelDLC.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelDLC, (int)componentResourceManager.GetObject("labelDLC.IconPadding1"));
			this.labelDLC.Name = "labelDLC";
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
			this.buttonCancel.Click += new EventHandler(this.buttonCancel_Click);
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
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.groupBoxData);
			base.Controls.Add(this.groupBoxMessage);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "ConfigureSendMessage";
			base.Shown += new EventHandler(this.ConfigureSendMessage_Shown);
			base.HelpRequested += new HelpEventHandler(this.ConfigureSendMessage_HelpRequested);
			this.groupBoxMessage.ResumeLayout(false);
			this.groupBoxMessage.PerformLayout();
			this.groupBoxData.ResumeLayout(false);
			this.groupBoxData.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
