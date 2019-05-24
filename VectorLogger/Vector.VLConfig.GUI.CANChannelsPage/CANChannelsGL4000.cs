using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.CANChannelsPage
{
	internal class CANChannelsGL4000 : UserControl
	{
		private Dictionary<uint, CheckBox> channelNr2CheckBoxChannel;

		private Dictionary<uint, ComboBox> channelNr2ComboBoxBaudrate;

		private Dictionary<uint, CheckBox> channelNr2CheckBoxKeepAwake;

		private Dictionary<uint, CheckBox> channelNr2CheckBoxOutputACK;

		private Dictionary<uint, CheckBox> channelNr2CheckBoxWakeUpEnabled;

		private Dictionary<uint, CheckBox> channelNr2CheckBoxLogErrorFrames;

		private Dictionary<uint, PictureBox> channelNr2LINprobeInfoIcon;

		private CANChannelConfiguration canChannelConfiguration;

		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isInitControls;

		private IContainer components;

		private RichTextBox richTextBoxDescription;

		private ComboBox comboBoxBaudrateCAN1;

		private CheckBox checkBoxOutputACKCAN1;

		private CheckBox checkBoxKeepAwakeCAN1;

		private GroupBox groupBoxChannels;

		private TableLayoutPanel tableLayoutPanelChannels;

		private GroupBox groupBoxDescription;

		private CheckBox checkBoxCAN1;

		private CheckBox checkBoxCAN2;

		private CheckBox checkBoxKeepAwakeCAN2;

		private CheckBox checkBoxOutputACKCAN2;

		private ComboBox comboBoxBaudrateCAN2;

		private CheckBox checkBoxCAN3;

		private CheckBox checkBoxCAN4;

		private CheckBox checkBoxCAN5;

		private CheckBox checkBoxCAN6;

		private CheckBox checkBoxCAN7;

		private CheckBox checkBoxCAN8;

		private ComboBox comboBoxBaudrateCAN3;

		private ComboBox comboBoxBaudrateCAN4;

		private ComboBox comboBoxBaudrateCAN5;

		private ComboBox comboBoxBaudrateCAN6;

		private ComboBox comboBoxBaudrateCAN7;

		private ComboBox comboBoxBaudrateCAN8;

		private CheckBox checkBoxOutputACKCAN3;

		private CheckBox checkBoxOutputACKCAN4;

		private CheckBox checkBoxOutputACKCAN5;

		private CheckBox checkBoxOutputACKCAN6;

		private CheckBox checkBoxOutputACKCAN7;

		private CheckBox checkBoxOutputACKCAN8;

		private CheckBox checkBoxKeepAwakeCAN3;

		private CheckBox checkBoxKeepAwakeCAN4;

		private CheckBox checkBoxKeepAwakeCAN5;

		private CheckBox checkBoxKeepAwakeCAN6;

		private CheckBox checkBoxKeepAwakeCAN7;

		private CheckBox checkBoxKeepAwakeCAN8;

		private CheckBox checkBoxCAN9;

		private ComboBox comboBoxBaudrateCAN9;

		private CheckBox checkBoxOutputACKCAN9;

		private CheckBox checkBoxKeepAwakeCAN9;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private CheckBox checkBoxEnableWakeUpCAN1;

		private CheckBox checkBoxEnableWakeUpCAN2;

		private CheckBox checkBoxEnableWakeUpCAN3;

		private CheckBox checkBoxEnableWakeUpCAN4;

		private CheckBox checkBoxEnableWakeUpCAN5;

		private CheckBox checkBoxEnableWakeUpCAN6;

		private CheckBox checkBoxEnableWakeUpCAN7;

		private CheckBox checkBoxEnableWakeUpCAN8;

		private PictureBox pictureBoxIconLINProbeInfo1;

		private PictureBox pictureBoxIconLINProbeInfo2;

		private PictureBox pictureBoxIconLINProbeInfo3;

		private PictureBox pictureBoxIconLINProbeInfo4;

		private PictureBox pictureBoxIconLINProbeInfo5;

		private PictureBox pictureBoxIconLINProbeInfo6;

		private PictureBox pictureBoxIconLINProbeInfo7;

		private PictureBox pictureBoxIconLINProbeInfo8;

		private PictureBox pictureBoxIconLINProbeInfo9;

		private ToolTip toolTip;

		private CheckBox checkBoxLogErrorFramesCAN1;

		private CheckBox checkBoxLogErrorFramesCAN2;

		private CheckBox checkBoxLogErrorFramesCAN3;

		private CheckBox checkBoxLogErrorFramesCAN4;

		private CheckBox checkBoxLogErrorFramesCAN5;

		private CheckBox checkBoxLogErrorFramesCAN6;

		private CheckBox checkBoxLogErrorFramesCAN7;

		private CheckBox checkBoxLogErrorFramesCAN8;

		private CheckBox checkBoxLogErrorFramesCAN9;

		private Label labelLogErrorFramesOnMemory;

		private CheckBox checkBoxLogErrorFramesMemory2;

		private CheckBox checkBoxLogErrorFramesMemory1;

		public CANChannelConfiguration CANChannelConfiguration
		{
			get
			{
				return this.canChannelConfiguration;
			}
			set
			{
				this.canChannelConfiguration = value;
				if (this.canChannelConfiguration != null)
				{
					if (this.ModelValidator != null)
					{
						ulong arg_45_0 = (ulong)this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels;
						long arg_44_0 = (long)this.canChannelConfiguration.CANChannels.Count;
					}
					this.UpdateGUI();
				}
				this.EnableCheckBoxsLogErrorFramesMemory();
			}
		}

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public DisplayMode DisplayMode
		{
			get;
			set;
		}

		public IConfigurationManagerService ConfigurationManagerService
		{
			get;
			set;
		}

		public CANChannelsGL4000()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.isInitControls = false;
			this.InitChannelNr2ControlLists();
			this.InitLINProbeInfoIcons();
		}

		public void Init()
		{
			this.isInitControls = true;
			this.InitPossibleSpeedRates();
			this.isInitControls = false;
		}

		private void InitChannelNr2ControlLists()
		{
			this.channelNr2CheckBoxChannel = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxChannel.Add(1u, this.checkBoxCAN1);
			this.channelNr2CheckBoxChannel.Add(2u, this.checkBoxCAN2);
			this.channelNr2CheckBoxChannel.Add(3u, this.checkBoxCAN3);
			this.channelNr2CheckBoxChannel.Add(4u, this.checkBoxCAN4);
			this.channelNr2CheckBoxChannel.Add(5u, this.checkBoxCAN5);
			this.channelNr2CheckBoxChannel.Add(6u, this.checkBoxCAN6);
			this.channelNr2CheckBoxChannel.Add(7u, this.checkBoxCAN7);
			this.channelNr2CheckBoxChannel.Add(8u, this.checkBoxCAN8);
			this.channelNr2CheckBoxChannel.Add(9u, this.checkBoxCAN9);
			this.channelNr2ComboBoxBaudrate = new Dictionary<uint, ComboBox>();
			this.channelNr2ComboBoxBaudrate.Add(1u, this.comboBoxBaudrateCAN1);
			this.channelNr2ComboBoxBaudrate.Add(2u, this.comboBoxBaudrateCAN2);
			this.channelNr2ComboBoxBaudrate.Add(3u, this.comboBoxBaudrateCAN3);
			this.channelNr2ComboBoxBaudrate.Add(4u, this.comboBoxBaudrateCAN4);
			this.channelNr2ComboBoxBaudrate.Add(5u, this.comboBoxBaudrateCAN5);
			this.channelNr2ComboBoxBaudrate.Add(6u, this.comboBoxBaudrateCAN6);
			this.channelNr2ComboBoxBaudrate.Add(7u, this.comboBoxBaudrateCAN7);
			this.channelNr2ComboBoxBaudrate.Add(8u, this.comboBoxBaudrateCAN8);
			this.channelNr2ComboBoxBaudrate.Add(9u, this.comboBoxBaudrateCAN9);
			this.channelNr2CheckBoxKeepAwake = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxKeepAwake.Add(1u, this.checkBoxKeepAwakeCAN1);
			this.channelNr2CheckBoxKeepAwake.Add(2u, this.checkBoxKeepAwakeCAN2);
			this.channelNr2CheckBoxKeepAwake.Add(3u, this.checkBoxKeepAwakeCAN3);
			this.channelNr2CheckBoxKeepAwake.Add(4u, this.checkBoxKeepAwakeCAN4);
			this.channelNr2CheckBoxKeepAwake.Add(5u, this.checkBoxKeepAwakeCAN5);
			this.channelNr2CheckBoxKeepAwake.Add(6u, this.checkBoxKeepAwakeCAN6);
			this.channelNr2CheckBoxKeepAwake.Add(7u, this.checkBoxKeepAwakeCAN7);
			this.channelNr2CheckBoxKeepAwake.Add(8u, this.checkBoxKeepAwakeCAN8);
			this.channelNr2CheckBoxKeepAwake.Add(9u, this.checkBoxKeepAwakeCAN9);
			this.channelNr2CheckBoxWakeUpEnabled = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxWakeUpEnabled.Add(1u, this.checkBoxEnableWakeUpCAN1);
			this.channelNr2CheckBoxWakeUpEnabled.Add(2u, this.checkBoxEnableWakeUpCAN2);
			this.channelNr2CheckBoxWakeUpEnabled.Add(3u, this.checkBoxEnableWakeUpCAN3);
			this.channelNr2CheckBoxWakeUpEnabled.Add(4u, this.checkBoxEnableWakeUpCAN4);
			this.channelNr2CheckBoxWakeUpEnabled.Add(5u, this.checkBoxEnableWakeUpCAN5);
			this.channelNr2CheckBoxWakeUpEnabled.Add(6u, this.checkBoxEnableWakeUpCAN6);
			this.channelNr2CheckBoxWakeUpEnabled.Add(7u, this.checkBoxEnableWakeUpCAN7);
			this.channelNr2CheckBoxWakeUpEnabled.Add(8u, this.checkBoxEnableWakeUpCAN8);
			this.channelNr2CheckBoxOutputACK = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxOutputACK.Add(1u, this.checkBoxOutputACKCAN1);
			this.channelNr2CheckBoxOutputACK.Add(2u, this.checkBoxOutputACKCAN2);
			this.channelNr2CheckBoxOutputACK.Add(3u, this.checkBoxOutputACKCAN3);
			this.channelNr2CheckBoxOutputACK.Add(4u, this.checkBoxOutputACKCAN4);
			this.channelNr2CheckBoxOutputACK.Add(5u, this.checkBoxOutputACKCAN5);
			this.channelNr2CheckBoxOutputACK.Add(6u, this.checkBoxOutputACKCAN6);
			this.channelNr2CheckBoxOutputACK.Add(7u, this.checkBoxOutputACKCAN7);
			this.channelNr2CheckBoxOutputACK.Add(8u, this.checkBoxOutputACKCAN8);
			this.channelNr2CheckBoxOutputACK.Add(9u, this.checkBoxOutputACKCAN9);
			this.channelNr2CheckBoxLogErrorFrames = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxLogErrorFrames.Add(1u, this.checkBoxLogErrorFramesCAN1);
			this.channelNr2CheckBoxLogErrorFrames.Add(2u, this.checkBoxLogErrorFramesCAN2);
			this.channelNr2CheckBoxLogErrorFrames.Add(3u, this.checkBoxLogErrorFramesCAN3);
			this.channelNr2CheckBoxLogErrorFrames.Add(4u, this.checkBoxLogErrorFramesCAN4);
			this.channelNr2CheckBoxLogErrorFrames.Add(5u, this.checkBoxLogErrorFramesCAN5);
			this.channelNr2CheckBoxLogErrorFrames.Add(6u, this.checkBoxLogErrorFramesCAN6);
			this.channelNr2CheckBoxLogErrorFrames.Add(7u, this.checkBoxLogErrorFramesCAN7);
			this.channelNr2CheckBoxLogErrorFrames.Add(8u, this.checkBoxLogErrorFramesCAN8);
			this.channelNr2CheckBoxLogErrorFrames.Add(9u, this.checkBoxLogErrorFramesCAN9);
		}

		private void InitPossibleSpeedRates()
		{
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.channelNr2ComboBoxBaudrate.Count))
			{
				this.InitPossibleSpeedRates(this.channelNr2ComboBoxBaudrate[num]);
				num += 1u;
			}
		}

		private void InitPossibleSpeedRates(ComboBox comboBox)
		{
			comboBox.Items.Clear();
			IList<uint> standardCANBaudrates = GUIUtil.GetStandardCANBaudrates();
			foreach (uint current in standardCANBaudrates)
			{
				comboBox.Items.Add(GUIUtil.MapBaudrate2String(current));
			}
			comboBox.Items.Add(GUIUtil.EditUserdefDropdownEntry);
			this.UpdateUserdefinedSpeedrateValue(this.GetChannelIndexForComboBox(comboBox));
			comboBox.SelectedIndex = 0;
		}

		private void InitLINProbeInfoIcons()
		{
			this.channelNr2LINprobeInfoIcon = new Dictionary<uint, PictureBox>();
			this.channelNr2LINprobeInfoIcon.Add(1u, this.pictureBoxIconLINProbeInfo1);
			this.channelNr2LINprobeInfoIcon.Add(2u, this.pictureBoxIconLINProbeInfo2);
			this.channelNr2LINprobeInfoIcon.Add(3u, this.pictureBoxIconLINProbeInfo3);
			this.channelNr2LINprobeInfoIcon.Add(4u, this.pictureBoxIconLINProbeInfo4);
			this.channelNr2LINprobeInfoIcon.Add(5u, this.pictureBoxIconLINProbeInfo5);
			this.channelNr2LINprobeInfoIcon.Add(6u, this.pictureBoxIconLINProbeInfo6);
			this.channelNr2LINprobeInfoIcon.Add(7u, this.pictureBoxIconLINProbeInfo7);
			this.channelNr2LINprobeInfoIcon.Add(8u, this.pictureBoxIconLINProbeInfo8);
			this.channelNr2LINprobeInfoIcon.Add(9u, this.pictureBoxIconLINProbeInfo9);
			this.pictureBoxIconLINProbeInfo1.Image = Resources.IconInfo.ToBitmap();
			this.pictureBoxIconLINProbeInfo2.Image = Resources.IconInfo.ToBitmap();
			this.pictureBoxIconLINProbeInfo3.Image = Resources.IconInfo.ToBitmap();
			this.pictureBoxIconLINProbeInfo4.Image = Resources.IconInfo.ToBitmap();
			this.pictureBoxIconLINProbeInfo5.Image = Resources.IconInfo.ToBitmap();
			this.pictureBoxIconLINProbeInfo6.Image = Resources.IconInfo.ToBitmap();
			this.pictureBoxIconLINProbeInfo7.Image = Resources.IconInfo.ToBitmap();
			this.pictureBoxIconLINProbeInfo8.Image = Resources.IconInfo.ToBitmap();
			this.pictureBoxIconLINProbeInfo9.Image = Resources.IconInfo.ToBitmap();
			this.pictureBoxIconLINProbeInfo1.Visible = false;
			this.pictureBoxIconLINProbeInfo2.Visible = false;
			this.pictureBoxIconLINProbeInfo3.Visible = false;
			this.pictureBoxIconLINProbeInfo4.Visible = false;
			this.pictureBoxIconLINProbeInfo5.Visible = false;
			this.pictureBoxIconLINProbeInfo6.Visible = false;
			this.pictureBoxIconLINProbeInfo7.Visible = false;
			this.pictureBoxIconLINProbeInfo8.Visible = false;
			this.pictureBoxIconLINProbeInfo9.Visible = false;
		}

		private void checkBoxCAN_CheckedChanged(object sender, EventArgs e)
		{
			this.EnableControlsInRow(sender as CheckBox);
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBoxBaudrateCAN_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			uint channelIndexForComboBox = this.GetChannelIndexForComboBox(sender as ComboBox);
			CANChannel cANChannel = this.canChannelConfiguration.GetCANChannel(channelIndexForComboBox);
			if (this.channelNr2ComboBoxBaudrate[channelIndexForComboBox].SelectedItem.ToString() == GUIUtil.EditUserdefDropdownEntry)
			{
				uint num = 0u;
				bool flag = CANChipConfigurationManager.EditUserdefinedCANStdSetting(channelIndexForComboBox, cANChannel.CANChipConfiguration as CANStdChipConfiguration, out num);
				if (flag)
				{
					this.UpdateAndSelectUserdefinedSpeedrateValue(channelIndexForComboBox);
				}
				else if (num != 0u)
				{
					this.channelNr2ComboBoxBaudrate[channelIndexForComboBox].SelectedItem = GUIUtil.MapBaudrate2String(num);
				}
				else if (CANChipConfigurationManager.IsEqualAnyPredefinedSetting(channelIndexForComboBox, cANChannel.CANChipConfiguration as CANStdChipConfiguration))
				{
					this.channelNr2ComboBoxBaudrate[channelIndexForComboBox].SelectedItem = GUIUtil.MapBaudrate2String(cANChannel.CANChipConfiguration.Baudrate);
				}
				else
				{
					this.channelNr2ComboBoxBaudrate[channelIndexForComboBox].SelectedIndex = GUIUtil.GetIndexOfUserdefBaudrateValueEntry(this.channelNr2ComboBoxBaudrate[channelIndexForComboBox]);
				}
			}
			this.ValidateInput();
		}

		private void checkBoxOutputACKCAN_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxKeepAwakeCAN_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxWakeUpEnabled_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxLogErrorFramesCAN1_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.EnableCheckBoxsLogErrorFramesMemory();
			this.ValidateInput();
		}

		private void checkBoxLogErrorFramesMemory1_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxLogErrorFramesMemory2_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void pictureBoxIconLINProbeInfo_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(sender as PictureBox, Resources.ConfiguredForLINprobe);
		}

		public bool ValidateInput()
		{
			if (this.CANChannelConfiguration == null)
			{
				return false;
			}
			bool flag = true;
			bool flag2 = false;
			this.ResetValidationFramework();
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.channelNr2CheckBoxChannel.Count))
			{
				bool flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.channelNr2CheckBoxChannel[num].Checked, this.CANChannelConfiguration.GetCANChannel(num).IsActive, this.guiElementManager.GetGUIElement(this.channelNr2CheckBoxChannel[num]), out flag3);
				flag2 |= flag3;
				bool flag4 = false;
				CANStdChipConfiguration chipCfg = this.CANChannelConfiguration.GetCANChannel(num).CANChipConfiguration as CANStdChipConfiguration;
				if (this.channelNr2ComboBoxBaudrate[num].SelectedIndex == GUIUtil.GetIndexOfUserdefBaudrateValueEntry(this.channelNr2ComboBoxBaudrate[num]))
				{
					if (!CANChipConfigurationManager.IsEqualUserdefinedSetting(num, chipCfg))
					{
						CANChipConfigurationManager.ApplyUserdefinedSetting(num, ref chipCfg);
						flag2 = true;
						flag4 = true;
					}
				}
				else
				{
					uint baudrate = GUIUtil.MapString2Baudrate(this.channelNr2ComboBoxBaudrate[num].SelectedItem.ToString());
					if (!CANChipConfigurationManager.IsEqualPredefinedSettingForBaudrate(num, baudrate, chipCfg))
					{
						CANChipConfigurationManager.ApplyPredefinedSettingForBaudrate(num, baudrate, ref chipCfg);
						flag2 = true;
						flag4 = true;
					}
				}
				if (flag4 && !this.ModelValidator.IsPrescalerValueOfCANChannelValid(num, this.canChannelConfiguration))
				{
					InformMessageBox.Info(string.Format(Resources.BaudrateSettingInvalidPrescalerAutoCorrect, this.ModelValidator.GetMaxPrescalerValue(num)));
				}
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.channelNr2CheckBoxKeepAwake[num].Checked, this.CANChannelConfiguration.GetCANChannel(num).IsKeepAwakeActive, this.guiElementManager.GetGUIElement(this.channelNr2CheckBoxKeepAwake[num]), out flag3);
				flag2 |= flag3;
				if ((ulong)(num - 1u) < (ulong)((long)this.channelNr2CheckBoxWakeUpEnabled.Count))
				{
					flag &= this.pageValidator.Control.UpdateModel<bool>(this.channelNr2CheckBoxWakeUpEnabled[num].Checked, this.CANChannelConfiguration.GetCANChannel(num).IsWakeUpEnabled, this.guiElementManager.GetGUIElement(this.channelNr2CheckBoxWakeUpEnabled[num]), out flag3);
					flag2 |= flag3;
				}
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.channelNr2CheckBoxOutputACK[num].Checked, this.CANChannelConfiguration.GetCANChannel(num).IsOutputActive, this.guiElementManager.GetGUIElement(this.channelNr2CheckBoxOutputACK[num]), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.channelNr2CheckBoxLogErrorFrames[num].Checked, this.CANChannelConfiguration.GetCANChannel(num).LogErrorFrames, this.guiElementManager.GetGUIElement(this.channelNr2CheckBoxLogErrorFrames[num]), out flag3);
				flag2 |= flag3;
				if (this.checkBoxLogErrorFramesMemory1.Enabled)
				{
					flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxLogErrorFramesMemory1.Checked, this.canChannelConfiguration.LogErrorFramesOnMemories[0], this.guiElementManager.GetGUIElement(this.checkBoxLogErrorFramesMemory1), out flag3);
					flag2 |= flag3;
				}
				if (this.checkBoxLogErrorFramesMemory2.Enabled)
				{
					flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxLogErrorFramesMemory2.Checked, this.canChannelConfiguration.LogErrorFramesOnMemories[1], this.guiElementManager.GetGUIElement(this.checkBoxLogErrorFramesMemory2), out flag3);
					flag2 |= flag3;
				}
				num += 1u;
			}
			flag &= this.ModelValidator.Validate(this.CANChannelConfiguration, flag2, this.pageValidator);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		public bool HasErrors()
		{
			return this.pageValidator.General.HasErrors(Enum.GetValues(typeof(ValidationErrorClass)) as ValidationErrorClass[]);
		}

		public bool HasGlobalErrors()
		{
			return this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.GlobalModelError
			});
		}

		public bool HasLocalErrors()
		{
			return this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.FormatError,
				ValidationErrorClass.LocalModelError
			});
		}

		public bool HasFormatErrors()
		{
			IPageValidatorGeneral arg_13_0 = this.pageValidator.General;
			ValidationErrorClass[] errorClasses = new ValidationErrorClass[1];
			return arg_13_0.HasErrors(errorClasses);
		}

		private void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
		}

		private void EnableCheckBoxsLogErrorFramesMemory()
		{
			bool flag = (from chn in this.channelNr2CheckBoxLogErrorFrames.Values
			where chn.Checked
			select chn).Any<CheckBox>();
			this.checkBoxLogErrorFramesMemory1.Enabled = (flag && this.ModelValidator.GetActiveMemoryNumbers.Contains(1));
			this.checkBoxLogErrorFramesMemory2.Enabled = (flag && this.ModelValidator.GetActiveMemoryNumbers.Contains(2));
		}

		private void EnableControlsInRow(CheckBox checkBox)
		{
			KeyValuePair<uint, CheckBox> keyValuePair = this.channelNr2CheckBoxChannel.FirstOrDefault((KeyValuePair<uint, CheckBox> r) => r.Value == checkBox);
			if (keyValuePair.Equals(default(KeyValuePair<uint, CheckBox>)))
			{
				return;
			}
			uint key = keyValuePair.Key;
			this.EnableControlsInRow(key);
		}

		private void EnableControlsInRow(uint channelNr)
		{
			this.channelNr2ComboBoxBaudrate[channelNr].Enabled = this.channelNr2CheckBoxChannel[channelNr].Checked;
			this.channelNr2CheckBoxKeepAwake[channelNr].Enabled = this.channelNr2CheckBoxChannel[channelNr].Checked;
			this.channelNr2CheckBoxOutputACK[channelNr].Enabled = this.channelNr2CheckBoxChannel[channelNr].Checked;
			if ((ulong)(channelNr - 1u) < (ulong)((long)this.channelNr2CheckBoxWakeUpEnabled.Count))
			{
				this.channelNr2CheckBoxWakeUpEnabled[channelNr].Enabled = this.channelNr2CheckBoxChannel[channelNr].Checked;
			}
			this.channelNr2CheckBoxLogErrorFrames[channelNr].Enabled = this.channelNr2CheckBoxChannel[channelNr].Checked;
		}

		private void UpdateGUI()
		{
			if (this.canChannelConfiguration == null)
			{
				return;
			}
			this.isInitControls = true;
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.canChannelConfiguration.CANChannels.Count))
			{
				CANChannel cANChannel = this.canChannelConfiguration.GetCANChannel(num);
				CANStdChipConfiguration cANStdChipConfiguration = cANChannel.CANChipConfiguration as CANStdChipConfiguration;
				if (cANStdChipConfiguration != null)
				{
					this.channelNr2CheckBoxChannel[num].Checked = cANChannel.IsActive.Value;
					this.EnableControlsInRow(num);
					if (CANChipConfigurationManager.IsEqualAnyPredefinedSetting(num, cANStdChipConfiguration))
					{
						this.channelNr2ComboBoxBaudrate[num].SelectedItem = GUIUtil.MapBaudrate2String(cANStdChipConfiguration.Baudrate);
					}
					else if (CANChipConfigurationManager.IsEqualUserdefinedSetting(num, cANStdChipConfiguration))
					{
						this.channelNr2ComboBoxBaudrate[num].SelectedIndex = GUIUtil.GetIndexOfUserdefBaudrateValueEntry(this.channelNr2ComboBoxBaudrate[num]);
					}
					else
					{
						CANChipConfigurationManager.StoreUserdefinedSetting(num, cANStdChipConfiguration);
						this.channelNr2ComboBoxBaudrate[num].SelectedIndex = this.UpdateUserdefinedSpeedrateValue(num);
					}
					this.channelNr2CheckBoxKeepAwake[num].Checked = cANChannel.IsKeepAwakeActive.Value;
					this.channelNr2CheckBoxOutputACK[num].Checked = cANChannel.IsOutputActive.Value;
					if ((ulong)(num - 1u) < (ulong)((long)this.channelNr2CheckBoxWakeUpEnabled.Count))
					{
						this.channelNr2CheckBoxWakeUpEnabled[num].Checked = cANChannel.IsWakeUpEnabled.Value;
					}
					this.channelNr2CheckBoxLogErrorFrames[num].Checked = cANChannel.LogErrorFrames.Value;
				}
				num += 1u;
			}
			uint num2 = 1u;
			while ((ulong)num2 <= (ulong)((long)this.channelNr2LINprobeInfoIcon.Count))
			{
				this.channelNr2LINprobeInfoIcon[num2].Visible = false;
				num2 += 1u;
			}
			if (this.ConfigurationManagerService.LINChannelConfiguration.IsUsingLinProbe.Value)
			{
				this.channelNr2LINprobeInfoIcon[this.ConfigurationManagerService.LINChannelConfiguration.CANChannelNrUsedForLinProbe.Value].Visible = true;
			}
			this.checkBoxLogErrorFramesMemory1.Checked = this.canChannelConfiguration.LogErrorFramesOnMemories[0].Value;
			this.checkBoxLogErrorFramesMemory2.Checked = this.canChannelConfiguration.LogErrorFramesOnMemories[1].Value;
			this.EnableCheckBoxsLogErrorFramesMemory();
			this.isInitControls = false;
			this.ValidateInput();
		}

		private uint GetSelectedBaudrate(uint channelNr)
		{
			return this.GetSelectedBaudrate(this.channelNr2ComboBoxBaudrate[channelNr]);
		}

		private uint GetSelectedBaudrate(ComboBox comboBox)
		{
			return GUIUtil.MapString2Baudrate(comboBox.SelectedItem.ToString());
		}

		private uint GetChannelIndexForComboBox(ComboBox box)
		{
			for (uint num = 1u; num <= 9u; num += 1u)
			{
				if (this.channelNr2ComboBoxBaudrate[num] == box)
				{
					return num;
				}
			}
			return 1u;
		}

		private void UpdateAndSelectUserdefinedSpeedrateValue(uint channelNr)
		{
			int num = this.UpdateUserdefinedSpeedrateValue(channelNr);
			if (num >= 0)
			{
				this.channelNr2ComboBoxBaudrate[channelNr].SelectedIndex = num;
				return;
			}
			CANChannel cANChannel = this.canChannelConfiguration.GetCANChannel(channelNr);
			string text = GUIUtil.MapBaudrate2String(cANChannel.CANChipConfiguration.Baudrate);
			if (this.channelNr2ComboBoxBaudrate[channelNr].Items.Contains(text))
			{
				this.channelNr2ComboBoxBaudrate[channelNr].SelectedItem = text;
				return;
			}
			this.channelNr2ComboBoxBaudrate[channelNr].SelectedIndex = 0;
		}

		private int UpdateUserdefinedSpeedrateValue(uint channelNr)
		{
			int result = -1;
			uint num = 0u;
			if (CANChipConfigurationManager.GetUserdefinedBaudrate(channelNr, out num))
			{
				string item = GUIUtil.MapBaudrate2String(num) + GUIUtil.UserdefBaudrateValueEntryPostfix;
				int indexOfUserdefBaudrateValueEntry = GUIUtil.GetIndexOfUserdefBaudrateValueEntry(this.channelNr2ComboBoxBaudrate[channelNr]);
				if (indexOfUserdefBaudrateValueEntry >= 0)
				{
					this.channelNr2ComboBoxBaudrate[channelNr].Items.RemoveAt(indexOfUserdefBaudrateValueEntry);
				}
				int insertIndexOfUserdefBaudrateValueEntry = GUIUtil.GetInsertIndexOfUserdefBaudrateValueEntry(this.channelNr2ComboBoxBaudrate[channelNr], num);
				this.channelNr2ComboBoxBaudrate[channelNr].Items.Insert(insertIndexOfUserdefBaudrateValueEntry, item);
				result = insertIndexOfUserdefBaudrateValueEntry;
			}
			else
			{
				int indexOfUserdefBaudrateValueEntry2 = GUIUtil.GetIndexOfUserdefBaudrateValueEntry(this.channelNr2ComboBoxBaudrate[channelNr]);
				if (indexOfUserdefBaudrateValueEntry2 >= 0)
				{
					this.channelNr2ComboBoxBaudrate[channelNr].Items.RemoveAt(indexOfUserdefBaudrateValueEntry2);
				}
			}
			return result;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CANChannelsGL4000));
			this.richTextBoxDescription = new RichTextBox();
			this.comboBoxBaudrateCAN1 = new ComboBox();
			this.checkBoxOutputACKCAN1 = new CheckBox();
			this.checkBoxKeepAwakeCAN1 = new CheckBox();
			this.groupBoxChannels = new GroupBox();
			this.checkBoxLogErrorFramesMemory2 = new CheckBox();
			this.checkBoxLogErrorFramesMemory1 = new CheckBox();
			this.tableLayoutPanelChannels = new TableLayoutPanel();
			this.checkBoxCAN2 = new CheckBox();
			this.checkBoxKeepAwakeCAN2 = new CheckBox();
			this.checkBoxOutputACKCAN2 = new CheckBox();
			this.comboBoxBaudrateCAN2 = new ComboBox();
			this.checkBoxCAN1 = new CheckBox();
			this.checkBoxCAN3 = new CheckBox();
			this.checkBoxCAN4 = new CheckBox();
			this.checkBoxCAN5 = new CheckBox();
			this.checkBoxCAN6 = new CheckBox();
			this.checkBoxCAN7 = new CheckBox();
			this.checkBoxCAN8 = new CheckBox();
			this.comboBoxBaudrateCAN3 = new ComboBox();
			this.comboBoxBaudrateCAN5 = new ComboBox();
			this.comboBoxBaudrateCAN6 = new ComboBox();
			this.comboBoxBaudrateCAN7 = new ComboBox();
			this.comboBoxBaudrateCAN8 = new ComboBox();
			this.checkBoxOutputACKCAN3 = new CheckBox();
			this.checkBoxOutputACKCAN4 = new CheckBox();
			this.checkBoxOutputACKCAN5 = new CheckBox();
			this.checkBoxOutputACKCAN6 = new CheckBox();
			this.checkBoxOutputACKCAN7 = new CheckBox();
			this.checkBoxOutputACKCAN8 = new CheckBox();
			this.checkBoxKeepAwakeCAN3 = new CheckBox();
			this.checkBoxKeepAwakeCAN4 = new CheckBox();
			this.checkBoxKeepAwakeCAN5 = new CheckBox();
			this.checkBoxKeepAwakeCAN6 = new CheckBox();
			this.checkBoxKeepAwakeCAN7 = new CheckBox();
			this.checkBoxKeepAwakeCAN8 = new CheckBox();
			this.checkBoxCAN9 = new CheckBox();
			this.comboBoxBaudrateCAN9 = new ComboBox();
			this.checkBoxOutputACKCAN9 = new CheckBox();
			this.checkBoxKeepAwakeCAN9 = new CheckBox();
			this.checkBoxEnableWakeUpCAN1 = new CheckBox();
			this.checkBoxEnableWakeUpCAN2 = new CheckBox();
			this.checkBoxEnableWakeUpCAN3 = new CheckBox();
			this.checkBoxEnableWakeUpCAN4 = new CheckBox();
			this.checkBoxEnableWakeUpCAN5 = new CheckBox();
			this.checkBoxEnableWakeUpCAN6 = new CheckBox();
			this.checkBoxEnableWakeUpCAN7 = new CheckBox();
			this.checkBoxEnableWakeUpCAN8 = new CheckBox();
			this.comboBoxBaudrateCAN4 = new ComboBox();
			this.pictureBoxIconLINProbeInfo1 = new PictureBox();
			this.pictureBoxIconLINProbeInfo2 = new PictureBox();
			this.pictureBoxIconLINProbeInfo3 = new PictureBox();
			this.pictureBoxIconLINProbeInfo4 = new PictureBox();
			this.pictureBoxIconLINProbeInfo5 = new PictureBox();
			this.pictureBoxIconLINProbeInfo6 = new PictureBox();
			this.pictureBoxIconLINProbeInfo7 = new PictureBox();
			this.pictureBoxIconLINProbeInfo8 = new PictureBox();
			this.pictureBoxIconLINProbeInfo9 = new PictureBox();
			this.checkBoxLogErrorFramesCAN1 = new CheckBox();
			this.checkBoxLogErrorFramesCAN2 = new CheckBox();
			this.checkBoxLogErrorFramesCAN3 = new CheckBox();
			this.checkBoxLogErrorFramesCAN4 = new CheckBox();
			this.checkBoxLogErrorFramesCAN5 = new CheckBox();
			this.checkBoxLogErrorFramesCAN6 = new CheckBox();
			this.checkBoxLogErrorFramesCAN7 = new CheckBox();
			this.checkBoxLogErrorFramesCAN8 = new CheckBox();
			this.checkBoxLogErrorFramesCAN9 = new CheckBox();
			this.labelLogErrorFramesOnMemory = new Label();
			this.groupBoxDescription = new GroupBox();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.toolTip = new ToolTip(this.components);
			this.groupBoxChannels.SuspendLayout();
			this.tableLayoutPanelChannels.SuspendLayout();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo1).BeginInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo2).BeginInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo3).BeginInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo4).BeginInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo5).BeginInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo6).BeginInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo7).BeginInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo8).BeginInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo9).BeginInit();
			this.groupBoxDescription.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.richTextBoxDescription, "richTextBoxDescription");
			this.richTextBoxDescription.BorderStyle = BorderStyle.None;
			this.richTextBoxDescription.Name = "richTextBoxDescription";
			this.richTextBoxDescription.ReadOnly = true;
			this.richTextBoxDescription.TabStop = false;
			componentResourceManager.ApplyResources(this.comboBoxBaudrateCAN1, "comboBoxBaudrateCAN1");
			this.comboBoxBaudrateCAN1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrateCAN1.FormattingEnabled = true;
			this.comboBoxBaudrateCAN1.Name = "comboBoxBaudrateCAN1";
			this.comboBoxBaudrateCAN1.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrateCAN_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxOutputACKCAN1, "checkBoxOutputACKCAN1");
			this.checkBoxOutputACKCAN1.Name = "checkBoxOutputACKCAN1";
			this.checkBoxOutputACKCAN1.UseVisualStyleBackColor = true;
			this.checkBoxOutputACKCAN1.CheckedChanged += new EventHandler(this.checkBoxOutputACKCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeCAN1, "checkBoxKeepAwakeCAN1");
			this.checkBoxKeepAwakeCAN1.Name = "checkBoxKeepAwakeCAN1";
			this.checkBoxKeepAwakeCAN1.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeCAN1.CheckedChanged += new EventHandler(this.checkBoxKeepAwakeCAN_CheckedChanged);
			this.groupBoxChannels.Controls.Add(this.checkBoxLogErrorFramesMemory2);
			this.groupBoxChannels.Controls.Add(this.checkBoxLogErrorFramesMemory1);
			this.groupBoxChannels.Controls.Add(this.tableLayoutPanelChannels);
			this.groupBoxChannels.Controls.Add(this.labelLogErrorFramesOnMemory);
			componentResourceManager.ApplyResources(this.groupBoxChannels, "groupBoxChannels");
			this.groupBoxChannels.Name = "groupBoxChannels";
			this.groupBoxChannels.TabStop = false;
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesMemory2, "checkBoxLogErrorFramesMemory2");
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesMemory2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesMemory2.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesMemory2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesMemory2.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesMemory2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesMemory2.IconAlignment2"));
			this.checkBoxLogErrorFramesMemory2.Name = "checkBoxLogErrorFramesMemory2";
			this.checkBoxLogErrorFramesMemory2.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesMemory2.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesMemory2_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesMemory1, "checkBoxLogErrorFramesMemory1");
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesMemory1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesMemory1.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesMemory1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesMemory1.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesMemory1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesMemory1.IconAlignment2"));
			this.checkBoxLogErrorFramesMemory1.Name = "checkBoxLogErrorFramesMemory1";
			this.checkBoxLogErrorFramesMemory1.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesMemory1.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesMemory1_CheckedChanged);
			componentResourceManager.ApplyResources(this.tableLayoutPanelChannels, "tableLayoutPanelChannels");
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxCAN2, 1, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeCAN2, 5, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeCAN1, 5, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxOutputACKCAN2, 4, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxOutputACKCAN1, 4, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateCAN2, 3, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxCAN1, 1, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxCAN3, 1, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxCAN4, 1, 3);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxCAN5, 1, 4);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxCAN6, 1, 5);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxCAN7, 1, 6);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxCAN8, 1, 7);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateCAN3, 3, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateCAN5, 3, 4);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateCAN6, 3, 5);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateCAN7, 3, 6);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateCAN8, 3, 7);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxOutputACKCAN3, 4, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxOutputACKCAN4, 4, 3);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxOutputACKCAN5, 4, 4);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxOutputACKCAN6, 4, 5);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxOutputACKCAN7, 4, 6);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxOutputACKCAN8, 4, 7);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeCAN3, 5, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeCAN4, 5, 3);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeCAN5, 5, 4);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeCAN6, 5, 5);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeCAN7, 5, 6);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeCAN8, 5, 7);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxCAN9, 1, 8);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateCAN9, 3, 8);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxOutputACKCAN9, 4, 8);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeCAN9, 5, 8);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxEnableWakeUpCAN1, 6, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxEnableWakeUpCAN2, 6, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxEnableWakeUpCAN3, 6, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxEnableWakeUpCAN4, 6, 3);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxEnableWakeUpCAN5, 6, 4);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxEnableWakeUpCAN6, 6, 5);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxEnableWakeUpCAN7, 6, 6);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxEnableWakeUpCAN8, 6, 7);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateCAN1, 3, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateCAN4, 3, 3);
			this.tableLayoutPanelChannels.Controls.Add(this.pictureBoxIconLINProbeInfo1, 2, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.pictureBoxIconLINProbeInfo2, 2, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.pictureBoxIconLINProbeInfo3, 2, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.pictureBoxIconLINProbeInfo4, 2, 3);
			this.tableLayoutPanelChannels.Controls.Add(this.pictureBoxIconLINProbeInfo5, 2, 4);
			this.tableLayoutPanelChannels.Controls.Add(this.pictureBoxIconLINProbeInfo6, 2, 5);
			this.tableLayoutPanelChannels.Controls.Add(this.pictureBoxIconLINProbeInfo7, 2, 6);
			this.tableLayoutPanelChannels.Controls.Add(this.pictureBoxIconLINProbeInfo8, 2, 7);
			this.tableLayoutPanelChannels.Controls.Add(this.pictureBoxIconLINProbeInfo9, 2, 8);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLogErrorFramesCAN1, 7, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLogErrorFramesCAN2, 7, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLogErrorFramesCAN3, 7, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLogErrorFramesCAN4, 7, 3);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLogErrorFramesCAN5, 7, 4);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLogErrorFramesCAN6, 7, 5);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLogErrorFramesCAN7, 7, 6);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLogErrorFramesCAN8, 7, 7);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLogErrorFramesCAN9, 7, 8);
			this.tableLayoutPanelChannels.Name = "tableLayoutPanelChannels";
			componentResourceManager.ApplyResources(this.checkBoxCAN2, "checkBoxCAN2");
			this.checkBoxCAN2.Checked = true;
			this.checkBoxCAN2.CheckState = CheckState.Checked;
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxCAN2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN2.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxCAN2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN2.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxCAN2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN2.IconAlignment2"));
			this.checkBoxCAN2.Name = "checkBoxCAN2";
			this.checkBoxCAN2.UseVisualStyleBackColor = true;
			this.checkBoxCAN2.CheckedChanged += new EventHandler(this.checkBoxCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeCAN2, "checkBoxKeepAwakeCAN2");
			this.checkBoxKeepAwakeCAN2.Name = "checkBoxKeepAwakeCAN2";
			this.checkBoxKeepAwakeCAN2.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeCAN2.CheckedChanged += new EventHandler(this.checkBoxKeepAwakeCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxOutputACKCAN2, "checkBoxOutputACKCAN2");
			this.checkBoxOutputACKCAN2.Name = "checkBoxOutputACKCAN2";
			this.checkBoxOutputACKCAN2.UseVisualStyleBackColor = true;
			this.checkBoxOutputACKCAN2.CheckedChanged += new EventHandler(this.checkBoxOutputACKCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.comboBoxBaudrateCAN2, "comboBoxBaudrateCAN2");
			this.comboBoxBaudrateCAN2.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrateCAN2.FormattingEnabled = true;
			this.comboBoxBaudrateCAN2.Name = "comboBoxBaudrateCAN2";
			this.comboBoxBaudrateCAN2.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrateCAN_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxCAN1, "checkBoxCAN1");
			this.checkBoxCAN1.Checked = true;
			this.checkBoxCAN1.CheckState = CheckState.Checked;
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxCAN1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN1.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxCAN1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN1.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxCAN1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN1.IconAlignment2"));
			this.checkBoxCAN1.Name = "checkBoxCAN1";
			this.checkBoxCAN1.UseVisualStyleBackColor = true;
			this.checkBoxCAN1.CheckedChanged += new EventHandler(this.checkBoxCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxCAN3, "checkBoxCAN3");
			this.checkBoxCAN3.Checked = true;
			this.checkBoxCAN3.CheckState = CheckState.Checked;
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxCAN3, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN3.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxCAN3, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN3.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxCAN3, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN3.IconAlignment2"));
			this.checkBoxCAN3.Name = "checkBoxCAN3";
			this.checkBoxCAN3.UseVisualStyleBackColor = true;
			this.checkBoxCAN3.CheckedChanged += new EventHandler(this.checkBoxCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxCAN4, "checkBoxCAN4");
			this.checkBoxCAN4.Checked = true;
			this.checkBoxCAN4.CheckState = CheckState.Checked;
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxCAN4, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN4.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxCAN4, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN4.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxCAN4, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN4.IconAlignment2"));
			this.checkBoxCAN4.Name = "checkBoxCAN4";
			this.checkBoxCAN4.UseVisualStyleBackColor = true;
			this.checkBoxCAN4.CheckedChanged += new EventHandler(this.checkBoxCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxCAN5, "checkBoxCAN5");
			this.checkBoxCAN5.Checked = true;
			this.checkBoxCAN5.CheckState = CheckState.Checked;
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxCAN5, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN5.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxCAN5, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN5.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxCAN5, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN5.IconAlignment2"));
			this.checkBoxCAN5.Name = "checkBoxCAN5";
			this.checkBoxCAN5.UseVisualStyleBackColor = true;
			this.checkBoxCAN5.CheckedChanged += new EventHandler(this.checkBoxCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxCAN6, "checkBoxCAN6");
			this.checkBoxCAN6.Checked = true;
			this.checkBoxCAN6.CheckState = CheckState.Checked;
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxCAN6, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN6.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxCAN6, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN6.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxCAN6, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN6.IconAlignment2"));
			this.checkBoxCAN6.Name = "checkBoxCAN6";
			this.checkBoxCAN6.UseVisualStyleBackColor = true;
			this.checkBoxCAN6.CheckedChanged += new EventHandler(this.checkBoxCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxCAN7, "checkBoxCAN7");
			this.checkBoxCAN7.Checked = true;
			this.checkBoxCAN7.CheckState = CheckState.Checked;
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxCAN7, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN7.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxCAN7, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN7.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxCAN7, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN7.IconAlignment2"));
			this.checkBoxCAN7.Name = "checkBoxCAN7";
			this.checkBoxCAN7.UseVisualStyleBackColor = true;
			this.checkBoxCAN7.CheckedChanged += new EventHandler(this.checkBoxCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxCAN8, "checkBoxCAN8");
			this.checkBoxCAN8.Checked = true;
			this.checkBoxCAN8.CheckState = CheckState.Checked;
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxCAN8, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN8.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxCAN8, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN8.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxCAN8, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN8.IconAlignment2"));
			this.checkBoxCAN8.Name = "checkBoxCAN8";
			this.checkBoxCAN8.UseVisualStyleBackColor = true;
			this.checkBoxCAN8.CheckedChanged += new EventHandler(this.checkBoxCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.comboBoxBaudrateCAN3, "comboBoxBaudrateCAN3");
			this.comboBoxBaudrateCAN3.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrateCAN3.FormattingEnabled = true;
			this.comboBoxBaudrateCAN3.Name = "comboBoxBaudrateCAN3";
			this.comboBoxBaudrateCAN3.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrateCAN_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.comboBoxBaudrateCAN5, "comboBoxBaudrateCAN5");
			this.comboBoxBaudrateCAN5.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrateCAN5.FormattingEnabled = true;
			this.comboBoxBaudrateCAN5.Name = "comboBoxBaudrateCAN5";
			this.comboBoxBaudrateCAN5.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrateCAN_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.comboBoxBaudrateCAN6, "comboBoxBaudrateCAN6");
			this.comboBoxBaudrateCAN6.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrateCAN6.FormattingEnabled = true;
			this.comboBoxBaudrateCAN6.Name = "comboBoxBaudrateCAN6";
			this.comboBoxBaudrateCAN6.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrateCAN_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.comboBoxBaudrateCAN7, "comboBoxBaudrateCAN7");
			this.comboBoxBaudrateCAN7.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrateCAN7.FormattingEnabled = true;
			this.comboBoxBaudrateCAN7.Name = "comboBoxBaudrateCAN7";
			this.comboBoxBaudrateCAN7.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrateCAN_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.comboBoxBaudrateCAN8, "comboBoxBaudrateCAN8");
			this.comboBoxBaudrateCAN8.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrateCAN8.FormattingEnabled = true;
			this.comboBoxBaudrateCAN8.Name = "comboBoxBaudrateCAN8";
			this.comboBoxBaudrateCAN8.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrateCAN_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxOutputACKCAN3, "checkBoxOutputACKCAN3");
			this.checkBoxOutputACKCAN3.Name = "checkBoxOutputACKCAN3";
			this.checkBoxOutputACKCAN3.UseVisualStyleBackColor = true;
			this.checkBoxOutputACKCAN3.CheckedChanged += new EventHandler(this.checkBoxOutputACKCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxOutputACKCAN4, "checkBoxOutputACKCAN4");
			this.checkBoxOutputACKCAN4.Name = "checkBoxOutputACKCAN4";
			this.checkBoxOutputACKCAN4.UseVisualStyleBackColor = true;
			this.checkBoxOutputACKCAN4.CheckedChanged += new EventHandler(this.checkBoxOutputACKCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxOutputACKCAN5, "checkBoxOutputACKCAN5");
			this.checkBoxOutputACKCAN5.Name = "checkBoxOutputACKCAN5";
			this.checkBoxOutputACKCAN5.UseVisualStyleBackColor = true;
			this.checkBoxOutputACKCAN5.CheckedChanged += new EventHandler(this.checkBoxOutputACKCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxOutputACKCAN6, "checkBoxOutputACKCAN6");
			this.checkBoxOutputACKCAN6.Name = "checkBoxOutputACKCAN6";
			this.checkBoxOutputACKCAN6.UseVisualStyleBackColor = true;
			this.checkBoxOutputACKCAN6.CheckedChanged += new EventHandler(this.checkBoxOutputACKCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxOutputACKCAN7, "checkBoxOutputACKCAN7");
			this.checkBoxOutputACKCAN7.Name = "checkBoxOutputACKCAN7";
			this.checkBoxOutputACKCAN7.UseVisualStyleBackColor = true;
			this.checkBoxOutputACKCAN7.CheckedChanged += new EventHandler(this.checkBoxOutputACKCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxOutputACKCAN8, "checkBoxOutputACKCAN8");
			this.checkBoxOutputACKCAN8.Name = "checkBoxOutputACKCAN8";
			this.checkBoxOutputACKCAN8.UseVisualStyleBackColor = true;
			this.checkBoxOutputACKCAN8.CheckedChanged += new EventHandler(this.checkBoxOutputACKCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeCAN3, "checkBoxKeepAwakeCAN3");
			this.checkBoxKeepAwakeCAN3.Name = "checkBoxKeepAwakeCAN3";
			this.checkBoxKeepAwakeCAN3.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeCAN3.CheckedChanged += new EventHandler(this.checkBoxKeepAwakeCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeCAN4, "checkBoxKeepAwakeCAN4");
			this.checkBoxKeepAwakeCAN4.Name = "checkBoxKeepAwakeCAN4";
			this.checkBoxKeepAwakeCAN4.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeCAN4.CheckedChanged += new EventHandler(this.checkBoxKeepAwakeCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeCAN5, "checkBoxKeepAwakeCAN5");
			this.checkBoxKeepAwakeCAN5.Name = "checkBoxKeepAwakeCAN5";
			this.checkBoxKeepAwakeCAN5.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeCAN5.CheckedChanged += new EventHandler(this.checkBoxKeepAwakeCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeCAN6, "checkBoxKeepAwakeCAN6");
			this.checkBoxKeepAwakeCAN6.Name = "checkBoxKeepAwakeCAN6";
			this.checkBoxKeepAwakeCAN6.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeCAN6.CheckedChanged += new EventHandler(this.checkBoxKeepAwakeCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeCAN7, "checkBoxKeepAwakeCAN7");
			this.checkBoxKeepAwakeCAN7.Name = "checkBoxKeepAwakeCAN7";
			this.checkBoxKeepAwakeCAN7.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeCAN7.CheckedChanged += new EventHandler(this.checkBoxKeepAwakeCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeCAN8, "checkBoxKeepAwakeCAN8");
			this.checkBoxKeepAwakeCAN8.Name = "checkBoxKeepAwakeCAN8";
			this.checkBoxKeepAwakeCAN8.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeCAN8.CheckedChanged += new EventHandler(this.checkBoxKeepAwakeCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxCAN9, "checkBoxCAN9");
			this.checkBoxCAN9.Checked = true;
			this.checkBoxCAN9.CheckState = CheckState.Checked;
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxCAN9, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN9.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxCAN9, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN9.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxCAN9, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN9.IconAlignment2"));
			this.checkBoxCAN9.Name = "checkBoxCAN9";
			this.checkBoxCAN9.UseVisualStyleBackColor = true;
			this.checkBoxCAN9.CheckedChanged += new EventHandler(this.checkBoxCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.comboBoxBaudrateCAN9, "comboBoxBaudrateCAN9");
			this.comboBoxBaudrateCAN9.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrateCAN9.FormattingEnabled = true;
			this.comboBoxBaudrateCAN9.Name = "comboBoxBaudrateCAN9";
			this.comboBoxBaudrateCAN9.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrateCAN_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxOutputACKCAN9, "checkBoxOutputACKCAN9");
			this.checkBoxOutputACKCAN9.Name = "checkBoxOutputACKCAN9";
			this.checkBoxOutputACKCAN9.UseVisualStyleBackColor = true;
			this.checkBoxOutputACKCAN9.CheckedChanged += new EventHandler(this.checkBoxOutputACKCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeCAN9, "checkBoxKeepAwakeCAN9");
			this.checkBoxKeepAwakeCAN9.Name = "checkBoxKeepAwakeCAN9";
			this.checkBoxKeepAwakeCAN9.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeCAN9.CheckedChanged += new EventHandler(this.checkBoxKeepAwakeCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxEnableWakeUpCAN1, "checkBoxEnableWakeUpCAN1");
			this.checkBoxEnableWakeUpCAN1.Name = "checkBoxEnableWakeUpCAN1";
			this.checkBoxEnableWakeUpCAN1.UseVisualStyleBackColor = true;
			this.checkBoxEnableWakeUpCAN1.CheckedChanged += new EventHandler(this.checkBoxWakeUpEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxEnableWakeUpCAN2, "checkBoxEnableWakeUpCAN2");
			this.checkBoxEnableWakeUpCAN2.Name = "checkBoxEnableWakeUpCAN2";
			this.checkBoxEnableWakeUpCAN2.UseVisualStyleBackColor = true;
			this.checkBoxEnableWakeUpCAN2.CheckedChanged += new EventHandler(this.checkBoxWakeUpEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxEnableWakeUpCAN3, "checkBoxEnableWakeUpCAN3");
			this.checkBoxEnableWakeUpCAN3.Name = "checkBoxEnableWakeUpCAN3";
			this.checkBoxEnableWakeUpCAN3.UseVisualStyleBackColor = true;
			this.checkBoxEnableWakeUpCAN3.CheckedChanged += new EventHandler(this.checkBoxWakeUpEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxEnableWakeUpCAN4, "checkBoxEnableWakeUpCAN4");
			this.checkBoxEnableWakeUpCAN4.Name = "checkBoxEnableWakeUpCAN4";
			this.checkBoxEnableWakeUpCAN4.UseVisualStyleBackColor = true;
			this.checkBoxEnableWakeUpCAN4.CheckedChanged += new EventHandler(this.checkBoxWakeUpEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxEnableWakeUpCAN5, "checkBoxEnableWakeUpCAN5");
			this.checkBoxEnableWakeUpCAN5.Name = "checkBoxEnableWakeUpCAN5";
			this.checkBoxEnableWakeUpCAN5.UseVisualStyleBackColor = true;
			this.checkBoxEnableWakeUpCAN5.CheckedChanged += new EventHandler(this.checkBoxWakeUpEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxEnableWakeUpCAN6, "checkBoxEnableWakeUpCAN6");
			this.checkBoxEnableWakeUpCAN6.Name = "checkBoxEnableWakeUpCAN6";
			this.checkBoxEnableWakeUpCAN6.UseVisualStyleBackColor = true;
			this.checkBoxEnableWakeUpCAN6.CheckedChanged += new EventHandler(this.checkBoxWakeUpEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxEnableWakeUpCAN7, "checkBoxEnableWakeUpCAN7");
			this.checkBoxEnableWakeUpCAN7.Name = "checkBoxEnableWakeUpCAN7";
			this.checkBoxEnableWakeUpCAN7.UseVisualStyleBackColor = true;
			this.checkBoxEnableWakeUpCAN7.CheckedChanged += new EventHandler(this.checkBoxWakeUpEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxEnableWakeUpCAN8, "checkBoxEnableWakeUpCAN8");
			this.checkBoxEnableWakeUpCAN8.Name = "checkBoxEnableWakeUpCAN8";
			this.checkBoxEnableWakeUpCAN8.UseVisualStyleBackColor = true;
			this.checkBoxEnableWakeUpCAN8.CheckedChanged += new EventHandler(this.checkBoxWakeUpEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.comboBoxBaudrateCAN4, "comboBoxBaudrateCAN4");
			this.comboBoxBaudrateCAN4.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrateCAN4.FormattingEnabled = true;
			this.comboBoxBaudrateCAN4.Name = "comboBoxBaudrateCAN4";
			this.comboBoxBaudrateCAN4.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrateCAN_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.pictureBoxIconLINProbeInfo1, "pictureBoxIconLINProbeInfo1");
			this.pictureBoxIconLINProbeInfo1.Name = "pictureBoxIconLINProbeInfo1";
			this.pictureBoxIconLINProbeInfo1.TabStop = false;
			this.pictureBoxIconLINProbeInfo1.MouseEnter += new EventHandler(this.pictureBoxIconLINProbeInfo_MouseEnter);
			componentResourceManager.ApplyResources(this.pictureBoxIconLINProbeInfo2, "pictureBoxIconLINProbeInfo2");
			this.pictureBoxIconLINProbeInfo2.Name = "pictureBoxIconLINProbeInfo2";
			this.pictureBoxIconLINProbeInfo2.TabStop = false;
			this.pictureBoxIconLINProbeInfo2.MouseEnter += new EventHandler(this.pictureBoxIconLINProbeInfo_MouseEnter);
			componentResourceManager.ApplyResources(this.pictureBoxIconLINProbeInfo3, "pictureBoxIconLINProbeInfo3");
			this.pictureBoxIconLINProbeInfo3.Name = "pictureBoxIconLINProbeInfo3";
			this.pictureBoxIconLINProbeInfo3.TabStop = false;
			this.pictureBoxIconLINProbeInfo3.MouseEnter += new EventHandler(this.pictureBoxIconLINProbeInfo_MouseEnter);
			componentResourceManager.ApplyResources(this.pictureBoxIconLINProbeInfo4, "pictureBoxIconLINProbeInfo4");
			this.pictureBoxIconLINProbeInfo4.Name = "pictureBoxIconLINProbeInfo4";
			this.pictureBoxIconLINProbeInfo4.TabStop = false;
			this.pictureBoxIconLINProbeInfo4.MouseEnter += new EventHandler(this.pictureBoxIconLINProbeInfo_MouseEnter);
			componentResourceManager.ApplyResources(this.pictureBoxIconLINProbeInfo5, "pictureBoxIconLINProbeInfo5");
			this.pictureBoxIconLINProbeInfo5.Name = "pictureBoxIconLINProbeInfo5";
			this.pictureBoxIconLINProbeInfo5.TabStop = false;
			this.pictureBoxIconLINProbeInfo5.MouseEnter += new EventHandler(this.pictureBoxIconLINProbeInfo_MouseEnter);
			componentResourceManager.ApplyResources(this.pictureBoxIconLINProbeInfo6, "pictureBoxIconLINProbeInfo6");
			this.pictureBoxIconLINProbeInfo6.Name = "pictureBoxIconLINProbeInfo6";
			this.pictureBoxIconLINProbeInfo6.TabStop = false;
			this.pictureBoxIconLINProbeInfo6.MouseEnter += new EventHandler(this.pictureBoxIconLINProbeInfo_MouseEnter);
			componentResourceManager.ApplyResources(this.pictureBoxIconLINProbeInfo7, "pictureBoxIconLINProbeInfo7");
			this.pictureBoxIconLINProbeInfo7.Name = "pictureBoxIconLINProbeInfo7";
			this.pictureBoxIconLINProbeInfo7.TabStop = false;
			this.pictureBoxIconLINProbeInfo7.MouseEnter += new EventHandler(this.pictureBoxIconLINProbeInfo_MouseEnter);
			componentResourceManager.ApplyResources(this.pictureBoxIconLINProbeInfo8, "pictureBoxIconLINProbeInfo8");
			this.pictureBoxIconLINProbeInfo8.Name = "pictureBoxIconLINProbeInfo8";
			this.pictureBoxIconLINProbeInfo8.TabStop = false;
			this.pictureBoxIconLINProbeInfo8.MouseEnter += new EventHandler(this.pictureBoxIconLINProbeInfo_MouseEnter);
			componentResourceManager.ApplyResources(this.pictureBoxIconLINProbeInfo9, "pictureBoxIconLINProbeInfo9");
			this.pictureBoxIconLINProbeInfo9.Name = "pictureBoxIconLINProbeInfo9";
			this.pictureBoxIconLINProbeInfo9.TabStop = false;
			this.pictureBoxIconLINProbeInfo9.MouseEnter += new EventHandler(this.pictureBoxIconLINProbeInfo_MouseEnter);
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesCAN1, "checkBoxLogErrorFramesCAN1");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN1.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN1.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesCAN1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN1.IconAlignment2"));
			this.checkBoxLogErrorFramesCAN1.Name = "checkBoxLogErrorFramesCAN1";
			this.checkBoxLogErrorFramesCAN1.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesCAN1.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesCAN1_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesCAN2, "checkBoxLogErrorFramesCAN2");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN2.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN2.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesCAN2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN2.IconAlignment2"));
			this.checkBoxLogErrorFramesCAN2.Name = "checkBoxLogErrorFramesCAN2";
			this.checkBoxLogErrorFramesCAN2.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesCAN2.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesCAN1_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesCAN3, "checkBoxLogErrorFramesCAN3");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN3, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN3.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN3, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN3.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesCAN3, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN3.IconAlignment2"));
			this.checkBoxLogErrorFramesCAN3.Name = "checkBoxLogErrorFramesCAN3";
			this.checkBoxLogErrorFramesCAN3.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesCAN3.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesCAN1_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesCAN4, "checkBoxLogErrorFramesCAN4");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN4, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN4.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN4, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN4.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesCAN4, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN4.IconAlignment2"));
			this.checkBoxLogErrorFramesCAN4.Name = "checkBoxLogErrorFramesCAN4";
			this.checkBoxLogErrorFramesCAN4.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesCAN4.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesCAN1_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesCAN5, "checkBoxLogErrorFramesCAN5");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN5, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN5.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN5, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN5.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesCAN5, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN5.IconAlignment2"));
			this.checkBoxLogErrorFramesCAN5.Name = "checkBoxLogErrorFramesCAN5";
			this.checkBoxLogErrorFramesCAN5.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesCAN5.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesCAN1_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesCAN6, "checkBoxLogErrorFramesCAN6");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN6, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN6.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN6, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN6.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesCAN6, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN6.IconAlignment2"));
			this.checkBoxLogErrorFramesCAN6.Name = "checkBoxLogErrorFramesCAN6";
			this.checkBoxLogErrorFramesCAN6.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesCAN6.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesCAN1_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesCAN7, "checkBoxLogErrorFramesCAN7");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN7, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN7.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN7, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN7.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesCAN7, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN7.IconAlignment2"));
			this.checkBoxLogErrorFramesCAN7.Name = "checkBoxLogErrorFramesCAN7";
			this.checkBoxLogErrorFramesCAN7.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesCAN7.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesCAN1_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesCAN8, "checkBoxLogErrorFramesCAN8");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN8, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN8.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN8, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN8.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesCAN8, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN8.IconAlignment2"));
			this.checkBoxLogErrorFramesCAN8.Name = "checkBoxLogErrorFramesCAN8";
			this.checkBoxLogErrorFramesCAN8.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesCAN8.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesCAN1_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesCAN9, "checkBoxLogErrorFramesCAN9");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN9, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN9.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN9, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN9.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesCAN9, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN9.IconAlignment2"));
			this.checkBoxLogErrorFramesCAN9.Name = "checkBoxLogErrorFramesCAN9";
			this.checkBoxLogErrorFramesCAN9.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesCAN9.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesCAN1_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelLogErrorFramesOnMemory, "labelLogErrorFramesOnMemory");
			this.labelLogErrorFramesOnMemory.Name = "labelLogErrorFramesOnMemory";
			this.groupBoxDescription.Controls.Add(this.richTextBoxDescription);
			componentResourceManager.ApplyResources(this.groupBoxDescription, "groupBoxDescription");
			this.groupBoxDescription.Name = "groupBoxDescription";
			this.groupBoxDescription.TabStop = false;
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxDescription);
			base.Controls.Add(this.groupBoxChannels);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "CANChannelsGL4000";
			this.groupBoxChannels.ResumeLayout(false);
			this.groupBoxChannels.PerformLayout();
			this.tableLayoutPanelChannels.ResumeLayout(false);
			this.tableLayoutPanelChannels.PerformLayout();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo1).EndInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo2).EndInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo3).EndInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo4).EndInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo5).EndInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo6).EndInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo7).EndInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo8).EndInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo9).EndInit();
			this.groupBoxDescription.ResumeLayout(false);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
