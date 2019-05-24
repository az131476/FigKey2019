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
	internal class SymbolicMessageCondition : Form
	{
		private SymbolicMessageEvent symMsgEvent;

		private IModelValidator modelValidator;

		private IApplicationDatabaseManager databaseManager;

		private int invalidChannelIndex;

		private IContainer components;

		private TextBox textBoxMessageName;

		private Label labelMessageName;

		private Button buttonSelectMessage;

		private Label labelChannel;

		private ComboBox comboBoxChannel;

		private Button buttonHelp;

		private Button buttonCancel;

		private Button buttonOK;

		private ErrorProvider errorProviderGlobalModel;

		private ToolTip toolTip;

		public SymbolicMessageEvent SymbolicMessageEvent
		{
			get
			{
				return this.symMsgEvent;
			}
			set
			{
				this.symMsgEvent = value;
			}
		}

		public SymbolicMessageCondition(IModelValidator modelVal, IApplicationDatabaseManager manager)
		{
			this.modelValidator = modelVal;
			this.databaseManager = manager;
			this.InitializeComponent();
			this.invalidChannelIndex = -1;
		}

		private void InitChannelComboBox()
		{
			this.SubscribeControlEvents(false);
			this.comboBoxChannel.Items.Clear();
			uint totalNumberOfLogicalChannels = this.modelValidator.GetTotalNumberOfLogicalChannels(this.symMsgEvent.BusType.Value);
			switch (this.symMsgEvent.BusType.Value)
			{
			case BusType.Bt_CAN:
				for (uint num = 1u; num <= totalNumberOfLogicalChannels; num += 1u)
				{
					this.comboBoxChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
				}
				if (this.symMsgEvent.ChannelNumber.Value <= totalNumberOfLogicalChannels)
				{
					this.comboBoxChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.symMsgEvent.ChannelNumber.Value);
				}
				else
				{
					this.comboBoxChannel.SelectedIndex = 0;
				}
				break;
			case BusType.Bt_LIN:
				for (uint num2 = 1u; num2 <= totalNumberOfLogicalChannels; num2 += 1u)
				{
					this.comboBoxChannel.Items.Add(GUIUtil.MapLINChannelNumber2String(num2, this.modelValidator.LoggerSpecifics));
				}
				if (this.symMsgEvent.ChannelNumber.Value <= totalNumberOfLogicalChannels)
				{
					this.comboBoxChannel.SelectedItem = GUIUtil.MapLINChannelNumber2String(this.symMsgEvent.ChannelNumber.Value, this.modelValidator.LoggerSpecifics);
				}
				else
				{
					this.comboBoxChannel.SelectedIndex = 0;
				}
				break;
			case BusType.Bt_FlexRay:
				for (uint num3 = 1u; num3 <= totalNumberOfLogicalChannels; num3 += 1u)
				{
					this.comboBoxChannel.Items.Add(GUIUtil.MapFlexrayChannelNumber2String(num3));
				}
				if (this.symMsgEvent.ChannelNumber.Value <= totalNumberOfLogicalChannels)
				{
					this.comboBoxChannel.SelectedItem = GUIUtil.MapFlexrayChannelNumber2String(this.symMsgEvent.ChannelNumber.Value);
				}
				else
				{
					this.comboBoxChannel.SelectedIndex = 0;
				}
				break;
			}
			this.SubscribeControlEvents(true);
		}

		private void ApplyValuesToControls()
		{
			this.SubscribeControlEvents(false);
			string arg = Vocabulary.CAN;
			if (this.symMsgEvent.BusType.Value == BusType.Bt_LIN)
			{
				arg = Vocabulary.LIN;
			}
			else if (this.symMsgEvent.BusType.Value == BusType.Bt_FlexRay)
			{
				arg = Vocabulary.Flexray;
			}
			this.labelMessageName.Text = string.Format(Resources.BusTypeMsgName, arg);
			this.textBoxMessageName.Text = this.symMsgEvent.MessageName.Value;
			this.ApplyChannelNumberToControl();
			this.SubscribeControlEvents(true);
		}

		private void ApplyChannelNumberToControl()
		{
			switch (this.symMsgEvent.BusType.Value)
			{
			case BusType.Bt_CAN:
				this.comboBoxChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.symMsgEvent.ChannelNumber.Value);
				return;
			case BusType.Bt_LIN:
			{
				string text = GUIUtil.MapLINChannelNumber2String(this.symMsgEvent.ChannelNumber.Value, this.modelValidator.LoggerSpecifics);
				if (this.comboBoxChannel.Items.Contains(text))
				{
					this.comboBoxChannel.SelectedItem = text;
				}
				else
				{
					this.invalidChannelIndex = this.comboBoxChannel.Items.Add(text);
					this.comboBoxChannel.SelectedIndex = this.invalidChannelIndex;
				}
				this.comboBoxChannel.SelectedItem = GUIUtil.MapLINChannelNumber2String(this.symMsgEvent.ChannelNumber.Value, this.modelValidator.LoggerSpecifics);
				return;
			}
			case BusType.Bt_FlexRay:
				this.comboBoxChannel.SelectedItem = GUIUtil.MapFlexrayChannelNumber2String(this.symMsgEvent.ChannelNumber.Value);
				return;
			default:
				return;
			}
		}

		private void SymbolicMessageCondition_Shown(object sender, EventArgs e)
		{
			this.InitChannelComboBox();
			this.ApplyValuesToControls();
			this.buttonOK.Enabled = this.ValidateInput();
		}

		private void buttonSelectMessage_Click(object sender, EventArgs e)
		{
			string value = this.symMsgEvent.MessageName.Value;
			string value2 = this.symMsgEvent.DatabaseName.Value;
			string text = this.symMsgEvent.DatabasePath.Value;
			string value3 = this.symMsgEvent.NetworkName.Value;
			BusType value4 = this.symMsgEvent.BusType.Value;
			uint value5 = this.symMsgEvent.ChannelNumber.Value;
			bool value6 = this.symMsgEvent.IsFlexrayPDU.Value;
			if (this.databaseManager.SelectMessageInDatabase(ref value, ref value2, ref text, ref value3, ref value4, ref value6))
			{
				string message;
				if (!this.modelValidator.DatabaseServices.IsSymbolicMessageInsertAllowed(value, value3, text, value4, out message))
				{
					InformMessageBox.Error(message);
					return;
				}
				text = this.modelValidator.GetFilePathRelativeToConfiguration(text);
				bool flag = false;
				IList<uint> channelAssignmentOfDatabase = this.modelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(text, value3);
				if (value4 == BusType.Bt_FlexRay && channelAssignmentOfDatabase[0] == Database.ChannelNumber_FlexrayAB)
				{
					if (this.symMsgEvent.MessageName.Value.EndsWith(Constants.FlexrayChannelA_Postfix) && value.EndsWith(Constants.FlexrayChannelB_Postfix))
					{
						value5 = 2u;
						flag = true;
					}
					if (this.symMsgEvent.MessageName.Value.EndsWith(Constants.FlexrayChannelB_Postfix) && value.EndsWith(Constants.FlexrayChannelA_Postfix))
					{
						value5 = 1u;
						flag = true;
					}
				}
				else if (!channelAssignmentOfDatabase.Contains(this.symMsgEvent.ChannelNumber.Value))
				{
					value5 = channelAssignmentOfDatabase[0];
					flag = true;
				}
				this.symMsgEvent.MessageName.Value = value;
				this.symMsgEvent.DatabaseName.Value = value2;
				this.symMsgEvent.DatabasePath.Value = text;
				this.symMsgEvent.NetworkName.Value = value3;
				this.symMsgEvent.BusType.Value = value4;
				this.symMsgEvent.ChannelNumber.Value = value5;
				if (flag)
				{
					this.ApplyChannelNumberToControl();
				}
				this.symMsgEvent.IsFlexrayPDU.Value = value6;
				this.ApplyValuesToControls();
				this.buttonOK.Enabled = this.ValidateInput();
			}
		}

		private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.buttonOK.Enabled = this.ValidateInput();
			if (this.invalidChannelIndex >= 0 && this.comboBoxChannel.SelectedIndex != this.invalidChannelIndex)
			{
				this.comboBoxChannel.Items.RemoveAt(this.invalidChannelIndex);
				this.invalidChannelIndex = -1;
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (!this.ValidateInput() && InformMessageBox.Question(Resources.InconsistSettingsCommitCondAnyway) == DialogResult.No)
			{
				return;
			}
			base.DialogResult = DialogResult.OK;
		}

		private void textBoxMessageName_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.textBoxMessageName, this.textBoxMessageName.Text);
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void SymbolicMessageCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private bool ValidateInput()
		{
			bool result = true;
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
			this.errorProviderGlobalModel.SetError(this.textBoxMessageName, "");
			switch (this.symMsgEvent.BusType.Value)
			{
			case BusType.Bt_CAN:
				this.symMsgEvent.ChannelNumber.Value = GUIUtil.MapCANChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
				break;
			case BusType.Bt_LIN:
				this.symMsgEvent.ChannelNumber.Value = GUIUtil.MapLINChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
				break;
			case BusType.Bt_FlexRay:
				this.symMsgEvent.ChannelNumber.Value = GUIUtil.MapFlexrayChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
				break;
			}
			if (!this.modelValidator.IsHardwareChannelAvailable(this.symMsgEvent.BusType.Value, this.symMsgEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelNotAvailable);
				result = false;
			}
			else if (!this.modelValidator.IsHardwareChannelActive(this.symMsgEvent.BusType.Value, this.symMsgEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelInactive);
				result = false;
			}
			MessageDefinition messageDefinition;
			if (!this.modelValidator.DatabaseServices.IsSymbolicMessageDefined(this.symMsgEvent.DatabasePath.Value, this.symMsgEvent.NetworkName.Value, this.symMsgEvent.MessageName.Value, this.symMsgEvent.ChannelNumber.Value, this.symMsgEvent.BusType.Value, out messageDefinition))
			{
				this.errorProviderGlobalModel.SetError(this.textBoxMessageName, Resources.ErrorUnresolvedMsgSymbol);
				result = false;
			}
			else if (BusType.Bt_FlexRay == this.symMsgEvent.BusType.Value && !this.modelValidator.ValidateSymbolicMessageChannelFromFlexrayDb(this.symMsgEvent.DatabasePath.Value, this.symMsgEvent.NetworkName.Value, this.symMsgEvent.MessageName.Value, this.symMsgEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.textBoxMessageName, Resources.ErrorUnresolvedMsgSymbolAtChn);
				result = false;
			}
			return result;
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.buttonSelectMessage.Click += new EventHandler(this.buttonSelectMessage_Click);
				this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
				return;
			}
			this.buttonSelectMessage.Click -= new EventHandler(this.buttonSelectMessage_Click);
			this.comboBoxChannel.SelectedIndexChanged -= new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SymbolicMessageCondition));
			this.textBoxMessageName = new TextBox();
			this.labelMessageName = new Label();
			this.buttonSelectMessage = new Button();
			this.labelChannel = new Label();
			this.comboBoxChannel = new ComboBox();
			this.buttonHelp = new Button();
			this.buttonCancel = new Button();
			this.buttonOK = new Button();
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.toolTip = new ToolTip(this.components);
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.textBoxMessageName, "textBoxMessageName");
			this.errorProviderGlobalModel.SetError(this.textBoxMessageName, componentResourceManager.GetString("textBoxMessageName.Error"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxMessageName, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMessageName.IconAlignment"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxMessageName, (int)componentResourceManager.GetObject("textBoxMessageName.IconPadding"));
			this.textBoxMessageName.Name = "textBoxMessageName";
			this.textBoxMessageName.ReadOnly = true;
			this.toolTip.SetToolTip(this.textBoxMessageName, componentResourceManager.GetString("textBoxMessageName.ToolTip"));
			this.textBoxMessageName.MouseEnter += new EventHandler(this.textBoxMessageName_MouseEnter);
			componentResourceManager.ApplyResources(this.labelMessageName, "labelMessageName");
			this.errorProviderGlobalModel.SetError(this.labelMessageName, componentResourceManager.GetString("labelMessageName.Error"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelMessageName, (ErrorIconAlignment)componentResourceManager.GetObject("labelMessageName.IconAlignment"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelMessageName, (int)componentResourceManager.GetObject("labelMessageName.IconPadding"));
			this.labelMessageName.Name = "labelMessageName";
			this.toolTip.SetToolTip(this.labelMessageName, componentResourceManager.GetString("labelMessageName.ToolTip"));
			componentResourceManager.ApplyResources(this.buttonSelectMessage, "buttonSelectMessage");
			this.errorProviderGlobalModel.SetError(this.buttonSelectMessage, componentResourceManager.GetString("buttonSelectMessage.Error"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonSelectMessage, (ErrorIconAlignment)componentResourceManager.GetObject("buttonSelectMessage.IconAlignment"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonSelectMessage, (int)componentResourceManager.GetObject("buttonSelectMessage.IconPadding"));
			this.buttonSelectMessage.Name = "buttonSelectMessage";
			this.toolTip.SetToolTip(this.buttonSelectMessage, componentResourceManager.GetString("buttonSelectMessage.ToolTip"));
			this.buttonSelectMessage.UseVisualStyleBackColor = true;
			this.buttonSelectMessage.Click += new EventHandler(this.buttonSelectMessage_Click);
			componentResourceManager.ApplyResources(this.labelChannel, "labelChannel");
			this.errorProviderGlobalModel.SetError(this.labelChannel, componentResourceManager.GetString("labelChannel.Error"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelChannel, (ErrorIconAlignment)componentResourceManager.GetObject("labelChannel.IconAlignment"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelChannel, (int)componentResourceManager.GetObject("labelChannel.IconPadding"));
			this.labelChannel.Name = "labelChannel";
			this.toolTip.SetToolTip(this.labelChannel, componentResourceManager.GetString("labelChannel.ToolTip"));
			componentResourceManager.ApplyResources(this.comboBoxChannel, "comboBoxChannel");
			this.comboBoxChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, componentResourceManager.GetString("comboBoxChannel.Error"));
			this.comboBoxChannel.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxChannel, (int)componentResourceManager.GetObject("comboBoxChannel.IconPadding"));
			this.comboBoxChannel.Name = "comboBoxChannel";
			this.toolTip.SetToolTip(this.comboBoxChannel, componentResourceManager.GetString("comboBoxChannel.ToolTip"));
			this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.errorProviderGlobalModel.SetError(this.buttonHelp, componentResourceManager.GetString("buttonHelp.Error"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonHelp, (ErrorIconAlignment)componentResourceManager.GetObject("buttonHelp.IconAlignment"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonHelp, (int)componentResourceManager.GetObject("buttonHelp.IconPadding"));
			this.buttonHelp.Name = "buttonHelp";
			this.toolTip.SetToolTip(this.buttonHelp, componentResourceManager.GetString("buttonHelp.ToolTip"));
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.errorProviderGlobalModel.SetError(this.buttonCancel, componentResourceManager.GetString("buttonCancel.Error"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonCancel, (ErrorIconAlignment)componentResourceManager.GetObject("buttonCancel.IconAlignment"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonCancel, (int)componentResourceManager.GetObject("buttonCancel.IconPadding"));
			this.buttonCancel.Name = "buttonCancel";
			this.toolTip.SetToolTip(this.buttonCancel, componentResourceManager.GetString("buttonCancel.ToolTip"));
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.errorProviderGlobalModel.SetError(this.buttonOK, componentResourceManager.GetString("buttonOK.Error"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonOK, (ErrorIconAlignment)componentResourceManager.GetObject("buttonOK.IconAlignment"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonOK, (int)componentResourceManager.GetObject("buttonOK.IconPadding"));
			this.buttonOK.Name = "buttonOK";
			this.toolTip.SetToolTip(this.buttonOK, componentResourceManager.GetString("buttonOK.ToolTip"));
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.comboBoxChannel);
			base.Controls.Add(this.labelChannel);
			base.Controls.Add(this.buttonSelectMessage);
			base.Controls.Add(this.labelMessageName);
			base.Controls.Add(this.textBoxMessageName);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SymbolicMessageCondition";
			this.toolTip.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			base.Shown += new EventHandler(this.SymbolicMessageCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.SymbolicMessageCondition_HelpRequested);
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
