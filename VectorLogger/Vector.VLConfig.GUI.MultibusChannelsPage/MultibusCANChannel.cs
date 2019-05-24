using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.MultibusChannelsPage
{
	internal class MultibusCANChannel : UserControl, IValidatable
	{
		private CANChannel channel;

		private IValidationHost validationHost;

		private bool supportsCANFD;

		private bool isInitControls;

		private IContainer components;

		private ComboBox comboBoxSpeedRate;

		private CheckBox checkBoxAck;

		private Label labelMode;

		private ComboBox comboBoxMode;

		private Label labelDataRate;

		private ComboBox comboBoxDataRate;

		private bool IsCANFDMode
		{
			get
			{
				return this.channel != null && this.channel.CANChipConfiguration != null && this.channel.CANChipConfiguration.IsCANFD;
			}
		}

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public uint ChannelNr
		{
			get;
			set;
		}

		public bool SupportsCANFD
		{
			get
			{
				return this.supportsCANFD;
			}
			set
			{
				this.supportsCANFD = value;
				this.UpdateGUI();
			}
		}

		public CANChannel Channel
		{
			get
			{
				return this.channel;
			}
			set
			{
				this.channel = value;
				this.UpdateGUI();
			}
		}

		IValidationHost IValidatable.ValidationHost
		{
			get
			{
				return this.validationHost;
			}
			set
			{
				this.validationHost = value;
			}
		}

		private IPageValidatorControl PageValidatorControl
		{
			get
			{
				return this.validationHost.PageValidator.Control;
			}
		}

		private GUIElementManager_Control GUIElementManager
		{
			get
			{
				return this.validationHost.GUIElementManager;
			}
		}

		public MultibusCANChannel()
		{
			this.InitializeComponent();
			this.isInitControls = true;
			this.InitSpeedRateComboBox();
			this.InitCanModeComboBox();
			this.InitDataRateComboBox();
			this.isInitControls = false;
		}

		private void InitSpeedRateComboBox()
		{
			this.comboBoxSpeedRate.Items.Clear();
			IList<uint> list = this.IsCANFDMode ? GUIUtil.GetStandardCANFDArbBaudrates() : GUIUtil.GetStandardCANBaudrates();
			foreach (uint current in list)
			{
				this.comboBoxSpeedRate.Items.Add(new BaudrateComboboxItem(current));
			}
			this.comboBoxSpeedRate.Items.Add(new BaudrateComboboxItem(Resources.UserDefinedDropdownEntry));
			this.comboBoxSpeedRate.SelectedIndex = 0;
		}

		private void InitCanModeComboBox()
		{
			this.comboBoxMode.Items.Clear();
			this.comboBoxMode.Items.Add(Vocabulary.CAN);
			this.comboBoxMode.Items.Add(Vocabulary.CANFD);
			this.comboBoxMode.SelectedIndex = 0;
		}

		private void InitDataRateComboBox()
		{
			this.comboBoxDataRate.Items.Clear();
			IList<uint> standardCANFDDataBaudrates = GUIUtil.GetStandardCANFDDataBaudrates();
			foreach (uint current in standardCANFDDataBaudrates)
			{
				this.comboBoxDataRate.Items.Add(new BaudrateComboboxItem(current));
			}
			this.comboBoxDataRate.Items.Add(new BaudrateComboboxItem(Resources.UserDefinedDropdownEntry));
			this.comboBoxDataRate.SelectedIndex = 0;
		}

		private void UpdateGUI()
		{
			if (this.channel == null || this.ChannelNr < 1u)
			{
				return;
			}
			this.isInitControls = true;
			this.checkBoxAck.Checked = this.channel.IsOutputActive.Value;
			this.comboBoxMode.SelectedItem = (this.IsCANFDMode ? Vocabulary.CANFD : Vocabulary.CAN);
			this.labelDataRate.Visible = this.IsCANFDMode;
			this.comboBoxDataRate.Visible = this.IsCANFDMode;
			this.InitSpeedRateComboBox();
			CANFDChipConfiguration cANFDChipConfiguration = this.channel.CANChipConfiguration as CANFDChipConfiguration;
			CANStdChipConfiguration cANStdChipConfiguration = this.channel.CANChipConfiguration as CANStdChipConfiguration;
			if (cANFDChipConfiguration != null)
			{
				if (CANChipConfigurationManager.IsEqualAnyPredefinedFDArbSetting(cANFDChipConfiguration))
				{
					BaudrateComboboxItem.Select(this.comboBoxSpeedRate, cANFDChipConfiguration.Baudrate);
				}
				else
				{
					if (!CANChipConfigurationManager.IsEqualUserdefinedFDArbSetting(this.ChannelNr, cANFDChipConfiguration))
					{
						CANChipConfigurationManager.StoreUserdefinedFDArbSetting(this.ChannelNr, cANFDChipConfiguration);
					}
					this.UpdateAndSelectUserdefinedSpeedrateValue();
				}
				this.InitDataRateComboBox();
				if (CANChipConfigurationManager.IsEqualAnyPredefinedFDDataSetting(cANFDChipConfiguration))
				{
					BaudrateComboboxItem.Select(this.comboBoxDataRate, cANFDChipConfiguration.DataBaudrate);
				}
				else
				{
					if (!CANChipConfigurationManager.IsEqualUserdefinedFDDataSetting(this.ChannelNr, cANFDChipConfiguration))
					{
						CANChipConfigurationManager.StoreUserdefinedFDDataSetting(this.ChannelNr, cANFDChipConfiguration);
					}
					this.UpdateAndSelectUserdefinedDatarateValue();
				}
			}
			else if (cANStdChipConfiguration != null)
			{
				if (CANChipConfigurationManager.IsEqualAnyPredefinedSetting(this.ChannelNr, cANStdChipConfiguration))
				{
					BaudrateComboboxItem.Select(this.comboBoxSpeedRate, cANStdChipConfiguration.Baudrate);
				}
				else
				{
					if (!CANChipConfigurationManager.IsEqualUserdefinedSetting(this.ChannelNr, cANStdChipConfiguration))
					{
						CANChipConfigurationManager.StoreUserdefinedSetting(this.ChannelNr, cANStdChipConfiguration);
					}
					this.UpdateAndSelectUserdefinedSpeedrateValue();
				}
			}
			this.Refresh();
			this.isInitControls = false;
		}

		private void UpdateAndSelectUserdefinedSpeedrateValue()
		{
			BaudrateComboboxItem.RemoveUserDefined(this.comboBoxSpeedRate);
			if (!CANChipConfigurationManager.HasUserdefinedSetting(this.ChannelNr))
			{
				return;
			}
			uint baudrate;
			if (this.IsCANFDMode)
			{
				CANChipConfigurationManager.GetUserdefinedFDArbBaudrate(this.ChannelNr, out baudrate);
			}
			else
			{
				CANChipConfigurationManager.GetUserdefinedBaudrate(this.ChannelNr, out baudrate);
			}
			BaudrateComboboxItem.InsertUserDefined(this.comboBoxSpeedRate, baudrate);
			BaudrateComboboxItem.SelectUserDefined(this.comboBoxSpeedRate);
		}

		private void UpdateAndSelectUserdefinedDatarateValue()
		{
			BaudrateComboboxItem.RemoveUserDefined(this.comboBoxDataRate);
			if (!CANChipConfigurationManager.HasUserdefinedSetting(this.ChannelNr))
			{
				return;
			}
			uint baudrate;
			CANChipConfigurationManager.GetUserdefinedFDDataBaudrate(this.ChannelNr, out baudrate);
			BaudrateComboboxItem.InsertUserDefined(this.comboBoxDataRate, baudrate);
			BaudrateComboboxItem.SelectUserDefined(this.comboBoxDataRate);
		}

		private void control_DataChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.validationHost.ValidateInput();
		}

		private void comboBoxMode_DataChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.validationHost.ValidateInput();
		}

		private void comboBoxSpeedRate_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			if (this.comboBoxSpeedRate.SelectedItem.ToString() == Resources.UserDefinedDropdownEntry)
			{
				if (this.IsCANFDMode)
				{
					this.EditAndSelectUserdefinedCANFDSetting();
				}
				else
				{
					uint num = 0u;
					bool flag = CANChipConfigurationManager.EditUserdefinedCANStdSetting(this.ChannelNr, this.channel.CANChipConfiguration as CANStdChipConfiguration, out num);
					this.isInitControls = true;
					if (flag)
					{
						this.UpdateAndSelectUserdefinedSpeedrateValue();
					}
					else if (num != 0u)
					{
						BaudrateComboboxItem.Select(this.comboBoxSpeedRate, num);
					}
					else if (CANChipConfigurationManager.IsEqualAnyPredefinedSetting(this.ChannelNr, this.channel.CANChipConfiguration as CANStdChipConfiguration))
					{
						BaudrateComboboxItem.Select(this.comboBoxSpeedRate, this.channel.CANChipConfiguration.Baudrate);
					}
					else
					{
						BaudrateComboboxItem.SelectUserDefined(this.comboBoxSpeedRate);
					}
					this.isInitControls = false;
				}
			}
			this.validationHost.ValidateInput();
		}

		private void comboBoxDataRate_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			if (this.comboBoxDataRate.SelectedItem.ToString() == GUIUtil.EditUserdefDropdownEntry)
			{
				this.EditAndSelectUserdefinedCANFDSetting();
			}
			this.validationHost.ValidateInput();
		}

		private void EditAndSelectUserdefinedCANFDSetting()
		{
			uint num = 0u;
			uint num2 = 0u;
			bool flag = CANChipConfigurationManager.EditUserdefinedCANFDSetting(this.ChannelNr, this.channel.CANChipConfiguration as CANFDChipConfiguration, out num, out num2);
			this.isInitControls = true;
			if (flag)
			{
				if (num != 0u)
				{
					BaudrateComboboxItem.Select(this.comboBoxSpeedRate, num);
				}
				else
				{
					this.UpdateAndSelectUserdefinedSpeedrateValue();
				}
				if (num2 != 0u)
				{
					BaudrateComboboxItem.Select(this.comboBoxDataRate, num2);
				}
				else
				{
					this.UpdateAndSelectUserdefinedDatarateValue();
				}
			}
			else
			{
				if (num != 0u)
				{
					BaudrateComboboxItem.Select(this.comboBoxSpeedRate, num);
				}
				else if (CANChipConfigurationManager.IsEqualAnyPredefinedFDArbSetting(this.channel.CANChipConfiguration as CANFDChipConfiguration))
				{
					BaudrateComboboxItem.Select(this.comboBoxSpeedRate, this.channel.CANChipConfiguration.Baudrate);
				}
				else
				{
					BaudrateComboboxItem.SelectUserDefined(this.comboBoxSpeedRate);
				}
				if (num2 != 0u)
				{
					BaudrateComboboxItem.Select(this.comboBoxDataRate, num2);
				}
				else
				{
					CANFDChipConfiguration cANFDChipConfiguration = this.channel.CANChipConfiguration as CANFDChipConfiguration;
					if (cANFDChipConfiguration != null && CANChipConfigurationManager.IsEqualAnyPredefinedFDDataSetting(cANFDChipConfiguration))
					{
						BaudrateComboboxItem.Select(this.comboBoxDataRate, cANFDChipConfiguration.DataBaudrate);
					}
					else
					{
						BaudrateComboboxItem.SelectUserDefined(this.comboBoxDataRate);
					}
				}
			}
			this.isInitControls = false;
		}

		bool IValidatable.ValidateInput(ref bool valueChanged)
		{
			if (this.channel == null)
			{
				return true;
			}
			bool flag = true;
			bool flag2;
			flag &= this.PageValidatorControl.UpdateModel<bool>(this.checkBoxAck.Checked, this.channel.IsOutputActive, this.GUIElementManager.GetGUIElement(this.checkBoxAck), out flag2);
			valueChanged |= flag2;
			bool flag3 = this.comboBoxMode.SelectedItem.ToString() == Vocabulary.CANFD;
			if (flag3 != this.channel.CANChipConfiguration.IsCANFD)
			{
				valueChanged = true;
			}
			if (flag3)
			{
				if (!(this.channel.CANChipConfiguration is CANFDChipConfiguration))
				{
					this.channel.CANChipConfiguration = new CANFDChipConfiguration();
					this.isInitControls = true;
					this.InitSpeedRateComboBox();
					BaudrateComboboxItem.Select(this.comboBoxSpeedRate, Constants.DefaultCANBaudrateHighSpeed);
					this.InitDataRateComboBox();
					BaudrateComboboxItem.Select(this.comboBoxDataRate, Constants.DefaultCANFDDataRate);
					this.isInitControls = false;
					CANChipConfigurationManager.DeleteUserdefinedSetting(this.ChannelNr);
					valueChanged = true;
				}
				CANFDChipConfiguration chipCfg = this.channel.CANChipConfiguration as CANFDChipConfiguration;
				bool flag4;
				uint selected = BaudrateComboboxItem.GetSelected(this.comboBoxSpeedRate, out flag4);
				if (flag4)
				{
					if (!CANChipConfigurationManager.IsEqualUserdefinedFDArbSetting(this.ChannelNr, chipCfg))
					{
						CANChipConfigurationManager.ApplyUserdefinedFDArbSetting(this.ChannelNr, ref chipCfg);
						valueChanged = true;
					}
				}
				else if (!CANChipConfigurationManager.IsEqualPredefinedSettingForFDArbBaudrate(selected, chipCfg))
				{
					CANChipConfigurationManager.ApplyPredefinedSettingForFDArbBaudrate(selected, ref chipCfg);
					valueChanged = true;
				}
				uint selected2 = BaudrateComboboxItem.GetSelected(this.comboBoxDataRate, out flag4);
				if (flag4)
				{
					if (!CANChipConfigurationManager.IsEqualUserdefinedFDDataSetting(this.ChannelNr, chipCfg))
					{
						CANChipConfigurationManager.ApplyUserdefinedFDDataSetting(this.ChannelNr, ref chipCfg);
						valueChanged = true;
					}
				}
				else if (!CANChipConfigurationManager.IsEqualPredefinedSettingForFDDataBaudrate(selected2, chipCfg))
				{
					CANChipConfigurationManager.ApplyPredefinedSettingForFDDataBaudrate(selected2, ref chipCfg);
					valueChanged = true;
				}
			}
			else
			{
				if (!(this.channel.CANChipConfiguration is CANStdChipConfiguration))
				{
					this.channel.CANChipConfiguration = new CANStdChipConfiguration();
					this.isInitControls = true;
					this.InitSpeedRateComboBox();
					BaudrateComboboxItem.Select(this.comboBoxSpeedRate, Constants.DefaultCANBaudrateHighSpeed);
					this.isInitControls = false;
					CANChipConfigurationManager.DeleteUserdefinedSetting(this.ChannelNr);
					valueChanged = true;
				}
				CANStdChipConfiguration chipCfg2 = this.channel.CANChipConfiguration as CANStdChipConfiguration;
				bool flag5;
				uint selected3 = BaudrateComboboxItem.GetSelected(this.comboBoxSpeedRate, out flag5);
				if (flag5)
				{
					if (!CANChipConfigurationManager.IsEqualUserdefinedSetting(this.ChannelNr, chipCfg2))
					{
						CANChipConfigurationManager.ApplyUserdefinedSetting(this.ChannelNr, ref chipCfg2);
						valueChanged = true;
					}
				}
				else if (!CANChipConfigurationManager.IsEqualPredefinedSettingForBaudrate(this.ChannelNr, selected3, chipCfg2))
				{
					CANChipConfigurationManager.ApplyPredefinedSettingForBaudrate(this.ChannelNr, selected3, ref chipCfg2);
					valueChanged = true;
				}
			}
			return flag;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MultibusCANChannel));
			this.comboBoxSpeedRate = new ComboBox();
			this.checkBoxAck = new CheckBox();
			this.labelMode = new Label();
			this.comboBoxMode = new ComboBox();
			this.labelDataRate = new Label();
			this.comboBoxDataRate = new ComboBox();
			base.SuspendLayout();
			this.comboBoxSpeedRate.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxSpeedRate.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxSpeedRate, "comboBoxSpeedRate");
			this.comboBoxSpeedRate.Name = "comboBoxSpeedRate";
			this.comboBoxSpeedRate.SelectedIndexChanged += new EventHandler(this.comboBoxSpeedRate_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxAck, "checkBoxAck");
			this.checkBoxAck.Name = "checkBoxAck";
			this.checkBoxAck.UseVisualStyleBackColor = true;
			this.checkBoxAck.CheckedChanged += new EventHandler(this.control_DataChanged);
			componentResourceManager.ApplyResources(this.labelMode, "labelMode");
			this.labelMode.Name = "labelMode";
			this.comboBoxMode.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxMode.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxMode, "comboBoxMode");
			this.comboBoxMode.Name = "comboBoxMode";
			this.comboBoxMode.SelectedIndexChanged += new EventHandler(this.comboBoxMode_DataChanged);
			componentResourceManager.ApplyResources(this.labelDataRate, "labelDataRate");
			this.labelDataRate.Name = "labelDataRate";
			this.comboBoxDataRate.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDataRate.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxDataRate, "comboBoxDataRate");
			this.comboBoxDataRate.Name = "comboBoxDataRate";
			this.comboBoxDataRate.SelectedIndexChanged += new EventHandler(this.comboBoxDataRate_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.comboBoxDataRate);
			base.Controls.Add(this.labelDataRate);
			base.Controls.Add(this.comboBoxMode);
			base.Controls.Add(this.labelMode);
			base.Controls.Add(this.checkBoxAck);
			base.Controls.Add(this.comboBoxSpeedRate);
			base.Name = "MultibusCANChannel";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
