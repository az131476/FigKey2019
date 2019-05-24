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
	internal class LINIdCondition : Form
	{
		private LINIdEvent linIdEvent;

		private IModelValidator modelValidator;

		private int invalidChannelIndex;

		private IContainer components;

		private Label labelConditionType;

		private ComboBox comboBoxConditionType;

		private Label labelID;

		private Label labelLastId;

		private TextBox textBoxLINId;

		private TextBox textBoxLINIdLast;

		private Button buttonOk;

		private Button buttonCancel;

		private ErrorProvider errorProviderFormat;

		private Button buttonHelp;

		private Label labelChannel;

		private ComboBox comboBoxChannel;

		private ErrorProvider errorProviderGlobalModel;

		public LINIdEvent LINIdEvent
		{
			get
			{
				return this.linIdEvent;
			}
			set
			{
				this.linIdEvent = value;
			}
		}

		public LINIdCondition(IModelValidator modelVal)
		{
			this.InitializeComponent();
			this.modelValidator = modelVal;
			this.linIdEvent = new LINIdEvent();
			this.InitRelationComboBox();
			this.invalidChannelIndex = -1;
		}

		public void ResetToDefaults()
		{
			this.linIdEvent.LowId.Value = 0u;
			this.linIdEvent.HighId.Value = 0u;
			this.linIdEvent.IdRelation.Value = CondRelation.Equal;
			this.linIdEvent.ChannelNumber.Value = this.modelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_LIN);
			this.InitControls();
		}

		private void InitControls()
		{
			this.ResetErrorProvider();
			this.SubscribeControlEvents(false);
			this.comboBoxConditionType.SelectedItem = GUIUtil.MapTriggerConditionRelation2String(this.linIdEvent.IdRelation.Value);
			this.textBoxLINId.Text = GUIUtil.LINIdToDisplayString(this.linIdEvent.LowId.Value);
			this.textBoxLINIdLast.Text = GUIUtil.LINIdToDisplayString(this.linIdEvent.HighId.Value);
			if (CondRelation.InRange == this.linIdEvent.IdRelation.Value || CondRelation.NotInRange == this.linIdEvent.IdRelation.Value)
			{
				this.textBoxLINIdLast.Enabled = true;
			}
			else
			{
				this.textBoxLINIdLast.Enabled = false;
			}
			string text = GUIUtil.MapLINChannelNumber2String(this.linIdEvent.ChannelNumber.Value, this.modelValidator.LoggerSpecifics);
			if (this.comboBoxChannel.Items.Contains(text))
			{
				this.comboBoxChannel.SelectedItem = text;
			}
			else
			{
				this.invalidChannelIndex = this.comboBoxChannel.Items.Add(text);
				this.comboBoxChannel.SelectedIndex = this.invalidChannelIndex;
			}
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
			this.comboBoxChannel.SelectedIndexChanged -= new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			this.comboBoxChannel.Items.Clear();
			this.invalidChannelIndex = -1;
			uint totalNumberOfLogicalChannels = this.modelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_LIN);
			for (uint num = 1u; num <= totalNumberOfLogicalChannels; num += 1u)
			{
				this.comboBoxChannel.Items.Add(GUIUtil.MapLINChannelNumber2String(num, this.modelValidator.LoggerSpecifics));
			}
			if (this.linIdEvent.ChannelNumber.Value <= totalNumberOfLogicalChannels)
			{
				this.comboBoxChannel.SelectedItem = GUIUtil.MapLINChannelNumber2String(this.linIdEvent.ChannelNumber.Value, this.modelValidator.LoggerSpecifics);
			}
			else
			{
				this.comboBoxChannel.SelectedIndex = 0;
			}
			this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
		}

		private void ResetErrorProvider()
		{
			this.errorProviderFormat.SetError(this.textBoxLINId, "");
			this.errorProviderFormat.SetError(this.textBoxLINIdLast, "");
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
		}

		private void LINIdCondition_Shown(object sender, EventArgs e)
		{
			this.InitChannelComboBox();
			this.RenderLabelsForDisplayMode();
			this.InitControls();
		}

		private void comboBoxConditionType_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.linIdEvent.IdRelation.Value = GUIUtil.MapString2TriggerConditionRelation(this.comboBoxConditionType.SelectedItem.ToString());
			if (this.linIdEvent.IdRelation.Value != CondRelation.InRange && this.linIdEvent.IdRelation.Value != CondRelation.NotInRange)
			{
				this.textBoxLINIdLast.Enabled = false;
				this.linIdEvent.HighId.Value = 0u;
				this.textBoxLINIdLast.Text = GUIUtil.LINIdToDisplayString(this.linIdEvent.HighId.Value);
				this.errorProviderFormat.SetError(this.textBoxLINIdLast, "");
			}
			else
			{
				this.textBoxLINIdLast.Enabled = true;
			}
			this.ValidateChildren();
		}

		private void textBoxLINId_Validating(object sender, CancelEventArgs e)
		{
			uint value;
			if (GUIUtil.DisplayStringToLINId(this.textBoxLINId.Text, out value))
			{
				this.linIdEvent.LowId.Value = value;
				if ((CondRelation.LessThan == this.linIdEvent.IdRelation.Value && this.linIdEvent.LowId.Value == 0u) || (CondRelation.GreaterThan == this.linIdEvent.IdRelation.Value && this.linIdEvent.LowId.Value == Constants.MaximumLINId))
				{
					this.errorProviderFormat.SetError(this.textBoxLINId, Resources.ErrorInvalidValueWithRelOp);
					e.Cancel = true;
					return;
				}
			}
			else
			{
				this.errorProviderFormat.SetError(this.textBoxLINId, "");
				this.errorProviderFormat.SetError(this.textBoxLINId, Resources.ErrorLINIdExpected);
				e.Cancel = true;
			}
		}

		private void textBoxLINId_Validated(object sender, EventArgs e)
		{
			this.errorProviderFormat.SetError(this.textBoxLINId, "");
		}

		private void textBoxLINIdLast_Validating(object sender, CancelEventArgs e)
		{
			uint value;
			if (GUIUtil.DisplayStringToLINId(this.textBoxLINIdLast.Text, out value))
			{
				this.linIdEvent.HighId.Value = value;
				return;
			}
			this.errorProviderFormat.SetError(this.textBoxLINIdLast, "");
			this.errorProviderFormat.SetError(this.textBoxLINIdLast, Resources.ErrorLINIdExpected);
			e.Cancel = true;
		}

		private void textBoxLINIdLast_Validated(object sender, EventArgs e)
		{
			this.errorProviderFormat.SetError(this.textBoxLINIdLast, "");
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
			if ((CondRelation.InRange == this.linIdEvent.IdRelation.Value || CondRelation.NotInRange == this.linIdEvent.IdRelation.Value) && this.linIdEvent.LowId.Value > this.linIdEvent.HighId.Value)
			{
				uint value = this.linIdEvent.HighId.Value;
				this.linIdEvent.HighId.Value = this.linIdEvent.LowId.Value;
				this.linIdEvent.LowId.Value = value;
			}
		}

		private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.linIdEvent.ChannelNumber.Value = GUIUtil.MapLINChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
			if (this.invalidChannelIndex >= 0 && this.comboBoxChannel.SelectedIndex != this.invalidChannelIndex)
			{
				this.comboBoxChannel.Items.RemoveAt(this.invalidChannelIndex);
				this.invalidChannelIndex = -1;
			}
			this.ValidateGlobalModelErrors();
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void LINIdCondition_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void SubscribeControlEvents(bool doSubscribe)
		{
			if (doSubscribe)
			{
				this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
				this.textBoxLINId.Validating += new CancelEventHandler(this.textBoxLINId_Validating);
				this.textBoxLINId.Validated += new EventHandler(this.textBoxLINId_Validated);
				this.textBoxLINIdLast.Validating += new CancelEventHandler(this.textBoxLINIdLast_Validating);
				this.textBoxLINIdLast.Validated += new EventHandler(this.textBoxLINIdLast_Validated);
				this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
				return;
			}
			this.comboBoxConditionType.SelectedIndexChanged -= new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			this.textBoxLINId.Validating -= new CancelEventHandler(this.textBoxLINId_Validating);
			this.textBoxLINId.Validated -= new EventHandler(this.textBoxLINId_Validated);
			this.textBoxLINIdLast.Validating -= new CancelEventHandler(this.textBoxLINIdLast_Validating);
			this.textBoxLINIdLast.Validated -= new EventHandler(this.textBoxLINIdLast_Validated);
			this.comboBoxChannel.SelectedIndexChanged -= new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
		}

		private void RenderLabelsForDisplayMode()
		{
			string arg = Resources.DisplayModeDec;
			if (GUIUtil.IsHexadecimal)
			{
				arg = Resources.DisplayModeHex;
			}
			this.labelID.Text = string.Format(Resources.IDLabelWithMode, arg);
			this.labelLastId.Text = string.Format(Resources.LastIDLabelWithMode, arg);
		}

		private void ValidateGlobalModelErrors()
		{
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, "");
			if (!this.modelValidator.IsHardwareChannelAvailable(BusType.Bt_LIN, this.linIdEvent.ChannelNumber.Value))
			{
				this.errorProviderGlobalModel.SetError(this.comboBoxChannel, Resources.ErrorChannelNotAvailable);
				return;
			}
			if (!this.modelValidator.IsHardwareChannelActive(BusType.Bt_LIN, this.linIdEvent.ChannelNumber.Value))
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LINIdCondition));
			this.labelConditionType = new Label();
			this.comboBoxConditionType = new ComboBox();
			this.labelID = new Label();
			this.labelLastId = new Label();
			this.textBoxLINId = new TextBox();
			this.textBoxLINIdLast = new TextBox();
			this.buttonOk = new Button();
			this.buttonCancel = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.comboBoxChannel = new ComboBox();
			this.buttonHelp = new Button();
			this.labelChannel = new Label();
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelConditionType, "labelConditionType");
			this.labelConditionType.Name = "labelConditionType";
			this.comboBoxConditionType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxConditionType.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxConditionType, "comboBoxConditionType");
			this.comboBoxConditionType.Name = "comboBoxConditionType";
			this.comboBoxConditionType.SelectedIndexChanged += new EventHandler(this.comboBoxConditionType_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelID, "labelID");
			this.labelID.Name = "labelID";
			componentResourceManager.ApplyResources(this.labelLastId, "labelLastId");
			this.labelLastId.Name = "labelLastId";
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxLINId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLINId.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxLINId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLINId.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxLINId, "textBoxLINId");
			this.textBoxLINId.Name = "textBoxLINId";
			this.textBoxLINId.Validating += new CancelEventHandler(this.textBoxLINId_Validating);
			this.textBoxLINId.Validated += new EventHandler(this.textBoxLINId_Validated);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxLINIdLast, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLINIdLast.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxLINIdLast, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxLINIdLast.IconAlignment1"));
			componentResourceManager.ApplyResources(this.textBoxLINIdLast, "textBoxLINIdLast");
			this.textBoxLINIdLast.Name = "textBoxLINIdLast";
			this.textBoxLINIdLast.Validating += new CancelEventHandler(this.textBoxLINIdLast_Validating);
			this.textBoxLINIdLast.Validated += new EventHandler(this.textBoxLINIdLast_Validated);
			this.buttonOk.DialogResult = DialogResult.OK;
			componentResourceManager.ApplyResources(this.buttonOk, "buttonOk");
			this.buttonOk.Name = "buttonOk";
			this.buttonOk.UseVisualStyleBackColor = true;
			this.buttonOk.Click += new EventHandler(this.buttonOk_Click);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.comboBoxChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxChannel.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment1"));
			componentResourceManager.ApplyResources(this.comboBoxChannel, "comboBoxChannel");
			this.comboBoxChannel.Name = "comboBoxChannel";
			this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.labelChannel, "labelChannel");
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
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOk);
			base.Controls.Add(this.textBoxLINIdLast);
			base.Controls.Add(this.textBoxLINId);
			base.Controls.Add(this.labelLastId);
			base.Controls.Add(this.labelID);
			base.Controls.Add(this.comboBoxConditionType);
			base.Controls.Add(this.labelConditionType);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "LINIdCondition";
			base.Shown += new EventHandler(this.LINIdCondition_Shown);
			base.HelpRequested += new HelpEventHandler(this.LINIdCondition_HelpRequested);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
