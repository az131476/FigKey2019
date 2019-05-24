using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.LEDsPage
{
	internal class LEDRow : UserControl, IValidatable
	{
		private LEDConfigListItem mConfigItem;

		private IValidationHost mValidationHost;

		private uint mPosition;

		private uint mFootnoteIndex;

		private bool mIsInitControls;

		private IContainer components;

		private TableLayoutPanel tableLayoutPanel;

		private Label mLabelLEDNr;

		private ComboBox mComboBoxState;

		private Label mLabelParameterName;

		private ComboBox mComboBoxParameter;

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public uint PositionNumber
		{
			get
			{
				return this.mPosition;
			}
		}

		public LEDConfigListItem ConfigItem
		{
			get
			{
				return this.mConfigItem;
			}
			set
			{
				this.mConfigItem = value;
				this.UpdateGUI();
			}
		}

		IValidationHost IValidatable.ValidationHost
		{
			get
			{
				return this.mValidationHost;
			}
			set
			{
				this.mValidationHost = value;
				if (this.mValidationHost != null)
				{
					this.mValidationHost.RegisterForErrorProvider(this.mComboBoxState);
					this.mValidationHost.RegisterForErrorProvider(this.mComboBoxParameter);
				}
			}
		}

		private IPageValidatorControl PageValidatorControl
		{
			get
			{
				return this.mValidationHost.PageValidator.Control;
			}
		}

		private GUIElementManager_Control GUIElementManager
		{
			get
			{
				return this.mValidationHost.GUIElementManager;
			}
		}

		public LEDRow()
		{
			this.InitializeComponent();
		}

		public void Init(uint rowPosition, uint footnoteIndex, IModelValidator modelValidator)
		{
			this.mPosition = rowPosition;
			this.mFootnoteIndex = footnoteIndex;
			this.ModelValidator = modelValidator;
			StringBuilder stringBuilder = new StringBuilder();
			for (uint num = 0u; num < this.mFootnoteIndex; num += 1u)
			{
				stringBuilder.Append('*');
			}
			this.mLabelLEDNr.Text = string.Format("LED {0} {1}:", this.mPosition, stringBuilder.ToString());
			this.InitLedStateComboBox();
		}

		private void InitLedStateComboBox()
		{
			this.mComboBoxState.Items.Clear();
			foreach (LEDState lEDState in Enum.GetValues(typeof(LEDState)))
			{
				if ((lEDState != LEDState.MOSTLock || this.ModelValidator.LoggerSpecifics.Recording.IsMOST150Supported) && (lEDState != LEDState.CANoeMeasurementOn || this.ModelValidator.LoggerSpecifics.DataTransfer.HasInterfaceMode) && (lEDState != LEDState.LINError || this.ModelValidator.LoggerSpecifics.LIN.NumberOfChannels != 0u) && ((lEDState != LEDState.CcpXcpError && lEDState != LEDState.CcpXcpOk) || this.ModelValidator.LoggerSpecifics.CAN.ChannelsWithOutputSupport.Count != 0))
				{
					this.mComboBoxState.Items.Add(GUIUtil.MapLEDState2String(lEDState));
				}
			}
		}

		private void InitChannelParameterControls(BusType busType, bool isWildcardSelectable)
		{
			this.mLabelParameterName.Text = Resources.LabelChannel;
			this.mComboBoxParameter.Items.Clear();
			if (busType == BusType.Bt_CAN)
			{
				if (isWildcardSelectable)
				{
					this.mComboBoxParameter.Items.Add(Resources.AnyCAN);
				}
				for (uint num = 1u; num <= this.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN); num += 1u)
				{
					this.mComboBoxParameter.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
				}
				return;
			}
			if (busType == BusType.Bt_LIN)
			{
				if (isWildcardSelectable)
				{
					this.mComboBoxParameter.Items.Add(Resources.AnyLIN);
				}
				for (uint num2 = 1u; num2 <= this.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_LIN); num2 += 1u)
				{
					this.mComboBoxParameter.Items.Add(GUIUtil.MapLINChannelNumber2String(num2, num2 > this.ModelValidator.LoggerSpecifics.LIN.NumberOfChannels));
				}
			}
		}

		private void InitMemoryParameterControls(bool isORWildcardAllowed, bool isANDWildcardAllowed)
		{
			this.mLabelParameterName.Text = Resources.LabelMemory;
			this.mComboBoxParameter.Items.Clear();
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories; num += 1u)
			{
				this.mComboBoxParameter.Items.Add(GUIUtil.MapValueToMemoriesLogicalString(num));
			}
			if (isORWildcardAllowed)
			{
				this.mComboBoxParameter.Items.Add(GUIUtil.MapValueToMemoriesLogicalString(2147483646u));
			}
			if (isANDWildcardAllowed)
			{
				this.mComboBoxParameter.Items.Add(GUIUtil.MapValueToMemoriesLogicalString(2147483647u));
			}
		}

		private void mComboBoxState_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.mIsInitControls)
			{
				return;
			}
			this.mValidationHost.ValidateInput();
		}

		private void mComboBoxParameter_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.mIsInitControls)
			{
				return;
			}
			this.mValidationHost.ValidateInput();
		}

		bool IValidatable.ValidateInput(ref bool valueChanged)
		{
			if (this.mConfigItem == null || this.ModelValidator == null)
			{
				return true;
			}
			bool flag = true;
			bool flag2;
			flag &= this.PageValidatorControl.UpdateModel<LEDState>(GUIUtil.MapString2LEDState(this.mComboBoxState.SelectedItem.ToString()), this.mConfigItem.State, this.GUIElementManager.GetGUIElement(this.mComboBoxState), out flag2);
			bool flag3 = flag2;
			valueChanged |= flag2;
			if (this.mConfigItem.UsedParameters == LEDItemParameter.ChannelWithWildcard || this.mConfigItem.UsedParameters == LEDItemParameter.ChannelSingle)
			{
				this.mConfigItem.ParameterMemory.Value = 0u;
				if (this.mConfigItem.UsedBusType == BusType.Bt_LIN)
				{
					if (flag3)
					{
						if (this.mConfigItem.UsedParameters == LEDItemParameter.ChannelSingle)
						{
							this.mConfigItem.ParameterChannelNumber.Value = 1u;
						}
						else
						{
							this.mConfigItem.ParameterChannelNumber.Value = 4294967295u;
						}
					}
					else
					{
						flag &= this.PageValidatorControl.UpdateModel<uint>(GUIUtil.MapLINChannelString2Number(this.mComboBoxParameter.SelectedItem.ToString()), this.mConfigItem.ParameterChannelNumber, this.GUIElementManager.GetGUIElement(this.mComboBoxParameter), out flag2);
						valueChanged |= flag2;
					}
				}
				else if (this.mConfigItem.UsedBusType == BusType.Bt_CAN)
				{
					if (flag3)
					{
						if (this.mConfigItem.UsedParameters == LEDItemParameter.ChannelSingle)
						{
							this.mConfigItem.ParameterChannelNumber.Value = 1u;
						}
						else
						{
							this.mConfigItem.ParameterChannelNumber.Value = 4294967295u;
						}
					}
					else
					{
						flag &= this.PageValidatorControl.UpdateModel<uint>(GUIUtil.MapCANChannelString2Number(this.mComboBoxParameter.SelectedItem.ToString()), this.mConfigItem.ParameterChannelNumber, this.GUIElementManager.GetGUIElement(this.mComboBoxParameter), out flag2);
						valueChanged |= flag2;
					}
				}
			}
			else if ((this.mConfigItem.UsedParameters & LEDItemParameter.MemorySingle) == LEDItemParameter.MemorySingle || (this.mConfigItem.UsedParameters & LEDItemParameter.MemoryWithORWildcard) == LEDItemParameter.MemoryWithORWildcard || (this.mConfigItem.UsedParameters & LEDItemParameter.MemoryWithANDWildcard) == LEDItemParameter.MemoryWithANDWildcard)
			{
				this.mConfigItem.ParameterChannelNumber.Value = 0u;
				if (flag3 || this.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories < 2u)
				{
					this.mConfigItem.ParameterMemory.Value = 2147483646u;
				}
				else
				{
					flag &= this.PageValidatorControl.UpdateModel<uint>(GUIUtil.MapMemoriesLogicalStringToValue(this.mComboBoxParameter.SelectedItem.ToString()), this.mConfigItem.ParameterMemory, this.GUIElementManager.GetGUIElement(this.mComboBoxParameter), out flag2);
					valueChanged |= flag2;
				}
			}
			else
			{
				this.mConfigItem.ParameterMemory.Value = 0u;
				this.mConfigItem.ParameterChannelNumber.Value = 0u;
			}
			return flag;
		}

		private void UpdateGUI()
		{
			if (this.mConfigItem == null)
			{
				return;
			}
			this.mIsInitControls = true;
			this.mComboBoxState.SelectedItem = GUIUtil.MapLEDState2String(this.mConfigItem.State.Value);
			this.mLabelParameterName.Visible = (this.ConfigItem.UsedParameters != LEDItemParameter.None);
			this.mComboBoxParameter.Visible = (this.ConfigItem.UsedParameters != LEDItemParameter.None);
			if (this.ConfigItem.UsedParameters == LEDItemParameter.ChannelWithWildcard || this.ConfigItem.UsedParameters == LEDItemParameter.ChannelSingle)
			{
				bool isWildcardSelectable = this.ConfigItem.UsedParameters == LEDItemParameter.ChannelWithWildcard;
				if (this.ConfigItem.UsedBusType == BusType.Bt_CAN)
				{
					this.InitChannelParameterControls(BusType.Bt_CAN, isWildcardSelectable);
					this.mComboBoxParameter.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.mConfigItem.ParameterChannelNumber.Value);
				}
				else if (this.ConfigItem.UsedBusType == BusType.Bt_LIN)
				{
					this.InitChannelParameterControls(BusType.Bt_LIN, isWildcardSelectable);
					if (this.mConfigItem.ParameterChannelNumber.Value > this.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_LIN) && this.mConfigItem.ParameterChannelNumber.Value <= this.ModelValidator.LoggerSpecifics.LIN.NumberOfChannels + this.ModelValidator.LoggerSpecifics.LIN.NumberOfLINprobeChannels)
					{
						this.mComboBoxParameter.Items.Add(GUIUtil.MapLINChannelNumber2String(this.mConfigItem.ParameterChannelNumber.Value, true));
					}
					this.mComboBoxParameter.SelectedItem = GUIUtil.MapLINChannelNumber2String(this.mConfigItem.ParameterChannelNumber.Value, this.ModelValidator.LoggerSpecifics);
				}
			}
			else if ((this.ConfigItem.UsedParameters & LEDItemParameter.MemorySingle) == LEDItemParameter.MemorySingle || (this.ConfigItem.UsedParameters & LEDItemParameter.MemoryWithORWildcard) == LEDItemParameter.MemoryWithORWildcard || (this.ConfigItem.UsedParameters & LEDItemParameter.MemoryWithANDWildcard) == LEDItemParameter.MemoryWithANDWildcard)
			{
				if (this.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories > 1u)
				{
					bool isORWildcardAllowed = (this.ConfigItem.UsedParameters & LEDItemParameter.MemoryWithORWildcard) == LEDItemParameter.MemoryWithORWildcard;
					bool isANDWildcardAllowed = (this.ConfigItem.UsedParameters & LEDItemParameter.MemoryWithANDWildcard) == LEDItemParameter.MemoryWithANDWildcard;
					this.InitMemoryParameterControls(isORWildcardAllowed, isANDWildcardAllowed);
					this.mComboBoxParameter.SelectedItem = GUIUtil.MapValueToMemoriesLogicalString(this.mConfigItem.ParameterMemory.Value);
				}
				else
				{
					this.mLabelParameterName.Visible = false;
					this.mComboBoxParameter.Visible = false;
				}
			}
			this.mIsInitControls = false;
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
			this.tableLayoutPanel = new TableLayoutPanel();
			this.mLabelLEDNr = new Label();
			this.mComboBoxState = new ComboBox();
			this.mLabelParameterName = new Label();
			this.mComboBoxParameter = new ComboBox();
			this.tableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			this.tableLayoutPanel.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.tableLayoutPanel.ColumnCount = 6;
			this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80f));
			this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170f));
			this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
			this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80f));
			this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170f));
			this.tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20f));
			this.tableLayoutPanel.Controls.Add(this.mLabelLEDNr, 0, 0);
			this.tableLayoutPanel.Controls.Add(this.mComboBoxState, 1, 0);
			this.tableLayoutPanel.Controls.Add(this.mLabelParameterName, 3, 0);
			this.tableLayoutPanel.Controls.Add(this.mComboBoxParameter, 4, 0);
			this.tableLayoutPanel.Location = new Point(3, 0);
			this.tableLayoutPanel.Name = "tableLayoutPanel";
			this.tableLayoutPanel.RowCount = 1;
			this.tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
			this.tableLayoutPanel.Size = new Size(551, 25);
			this.tableLayoutPanel.TabIndex = 0;
			this.mLabelLEDNr.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.mLabelLEDNr.AutoSize = true;
			this.mLabelLEDNr.Location = new Point(3, 3);
			this.mLabelLEDNr.Margin = new Padding(3);
			this.mLabelLEDNr.Name = "mLabelLEDNr";
			this.mLabelLEDNr.Size = new Size(74, 19);
			this.mLabelLEDNr.TabIndex = 0;
			this.mLabelLEDNr.Text = "LED 1";
			this.mComboBoxState.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.mComboBoxState.DropDownStyle = ComboBoxStyle.DropDownList;
			this.mComboBoxState.FormattingEnabled = true;
			this.mComboBoxState.Location = new Point(80, 0);
			this.mComboBoxState.Margin = new Padding(0);
			this.mComboBoxState.Name = "mComboBoxState";
			this.mComboBoxState.Size = new Size(170, 21);
			this.mComboBoxState.TabIndex = 1;
			this.mComboBoxState.SelectedIndexChanged += new EventHandler(this.mComboBoxState_SelectedIndexChanged);
			this.mLabelParameterName.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.mLabelParameterName.AutoSize = true;
			this.mLabelParameterName.Location = new Point(273, 3);
			this.mLabelParameterName.Margin = new Padding(3);
			this.mLabelParameterName.Name = "mLabelParameterName";
			this.mLabelParameterName.Size = new Size(74, 19);
			this.mLabelParameterName.TabIndex = 2;
			this.mLabelParameterName.Text = "Parameter:";
			this.mComboBoxParameter.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.mComboBoxParameter.DropDownStyle = ComboBoxStyle.DropDownList;
			this.mComboBoxParameter.FormattingEnabled = true;
			this.mComboBoxParameter.Location = new Point(350, 0);
			this.mComboBoxParameter.Margin = new Padding(0);
			this.mComboBoxParameter.Name = "mComboBoxParameter";
			this.mComboBoxParameter.Size = new Size(170, 21);
			this.mComboBoxParameter.TabIndex = 3;
			this.mComboBoxParameter.SelectedIndexChanged += new EventHandler(this.mComboBoxParameter_SelectedIndexChanged);
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.tableLayoutPanel);
			base.Name = "LEDRow";
			base.Size = new Size(557, 25);
			this.tableLayoutPanel.ResumeLayout(false);
			this.tableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
