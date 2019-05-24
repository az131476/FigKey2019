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
	internal class CANChannelsGL2000 : UserControl
	{
		private Dictionary<uint, CheckBox> channelNr2CheckBoxChannel;

		private Dictionary<uint, ComboBox> channelNr2ComboBoxBaudrate;

		private Dictionary<uint, CheckBox> channelNr2CheckBoxKeepAwake;

		private Dictionary<uint, CheckBox> channelNr2CheckBoxOutputACK;

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

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private CheckBox checkBoxCAN3;

		private ComboBox comboBoxBaudrateCAN3;

		private CheckBox checkBoxOutputACKCAN3;

		private CheckBox checkBoxKeepAwakeCAN3;

		private CheckBox checkBoxCAN4;

		private ComboBox comboBoxBaudrateCAN4;

		private CheckBox checkBoxOutputACKCAN4;

		private CheckBox checkBoxKeepAwakeCAN4;

		private PictureBox pictureBoxIconLINProbeInfo1;

		private PictureBox pictureBoxIconLINProbeInfo2;

		private PictureBox pictureBoxIconLINProbeInfo3;

		private PictureBox pictureBoxIconLINProbeInfo4;

		private ToolTip toolTip;

		private CheckBox checkBoxLogErrorFramesCAN1;

		private CheckBox checkBoxLogErrorFramesCAN2;

		private CheckBox checkBoxLogErrorFramesCAN3;

		private CheckBox checkBoxLogErrorFramesCAN4;

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

		public CANChannelsGL2000()
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
			this.channelNr2ComboBoxBaudrate = new Dictionary<uint, ComboBox>();
			this.channelNr2ComboBoxBaudrate.Add(1u, this.comboBoxBaudrateCAN1);
			this.channelNr2ComboBoxBaudrate.Add(2u, this.comboBoxBaudrateCAN2);
			this.channelNr2ComboBoxBaudrate.Add(3u, this.comboBoxBaudrateCAN3);
			this.channelNr2ComboBoxBaudrate.Add(4u, this.comboBoxBaudrateCAN4);
			this.channelNr2CheckBoxKeepAwake = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxKeepAwake.Add(1u, this.checkBoxKeepAwakeCAN1);
			this.channelNr2CheckBoxKeepAwake.Add(2u, this.checkBoxKeepAwakeCAN2);
			this.channelNr2CheckBoxKeepAwake.Add(3u, this.checkBoxKeepAwakeCAN3);
			this.channelNr2CheckBoxKeepAwake.Add(4u, this.checkBoxKeepAwakeCAN4);
			this.channelNr2CheckBoxOutputACK = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxOutputACK.Add(1u, this.checkBoxOutputACKCAN1);
			this.channelNr2CheckBoxOutputACK.Add(2u, this.checkBoxOutputACKCAN2);
			this.channelNr2CheckBoxOutputACK.Add(3u, this.checkBoxOutputACKCAN3);
			this.channelNr2CheckBoxOutputACK.Add(4u, this.checkBoxOutputACKCAN4);
			this.channelNr2CheckBoxLogErrorFrames = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxLogErrorFrames.Add(1u, this.checkBoxLogErrorFramesCAN1);
			this.channelNr2CheckBoxLogErrorFrames.Add(2u, this.checkBoxLogErrorFramesCAN2);
			this.channelNr2CheckBoxLogErrorFrames.Add(3u, this.checkBoxLogErrorFramesCAN3);
			this.channelNr2CheckBoxLogErrorFrames.Add(4u, this.checkBoxLogErrorFramesCAN4);
		}

		private void InitPossibleSpeedRates()
		{
			this.InitPossibleSpeedRates(this.comboBoxBaudrateCAN1);
			this.InitPossibleSpeedRates(this.comboBoxBaudrateCAN2);
			this.InitPossibleSpeedRates(this.comboBoxBaudrateCAN3);
			this.InitPossibleSpeedRates(this.comboBoxBaudrateCAN4);
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
			this.pictureBoxIconLINProbeInfo1.Image = Resources.IconInfo.ToBitmap();
			this.pictureBoxIconLINProbeInfo2.Image = Resources.IconInfo.ToBitmap();
			this.pictureBoxIconLINProbeInfo3.Image = Resources.IconInfo.ToBitmap();
			this.pictureBoxIconLINProbeInfo4.Image = Resources.IconInfo.ToBitmap();
			this.pictureBoxIconLINProbeInfo1.Visible = false;
			this.pictureBoxIconLINProbeInfo2.Visible = false;
			this.pictureBoxIconLINProbeInfo3.Visible = false;
			this.pictureBoxIconLINProbeInfo4.Visible = false;
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
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.channelNr2CheckBoxOutputACK[num].Checked, this.CANChannelConfiguration.GetCANChannel(num).IsOutputActive, this.guiElementManager.GetGUIElement(this.channelNr2CheckBoxOutputACK[num]), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.channelNr2CheckBoxLogErrorFrames[num].Checked, this.CANChannelConfiguration.GetCANChannel(num).LogErrorFrames, this.guiElementManager.GetGUIElement(this.channelNr2CheckBoxLogErrorFrames[num]), out flag3);
				flag2 |= flag3;
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
			for (uint num = 1u; num <= 4u; num += 1u)
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CANChannelsGL2000));
			this.richTextBoxDescription = new RichTextBox();
			this.comboBoxBaudrateCAN1 = new ComboBox();
			this.checkBoxOutputACKCAN1 = new CheckBox();
			this.checkBoxKeepAwakeCAN1 = new CheckBox();
			this.groupBoxChannels = new GroupBox();
			this.tableLayoutPanelChannels = new TableLayoutPanel();
			this.checkBoxCAN2 = new CheckBox();
			this.checkBoxKeepAwakeCAN2 = new CheckBox();
			this.checkBoxOutputACKCAN2 = new CheckBox();
			this.comboBoxBaudrateCAN2 = new ComboBox();
			this.checkBoxCAN1 = new CheckBox();
			this.checkBoxCAN3 = new CheckBox();
			this.comboBoxBaudrateCAN3 = new ComboBox();
			this.checkBoxOutputACKCAN3 = new CheckBox();
			this.checkBoxKeepAwakeCAN3 = new CheckBox();
			this.checkBoxCAN4 = new CheckBox();
			this.comboBoxBaudrateCAN4 = new ComboBox();
			this.checkBoxOutputACKCAN4 = new CheckBox();
			this.checkBoxKeepAwakeCAN4 = new CheckBox();
			this.pictureBoxIconLINProbeInfo1 = new PictureBox();
			this.pictureBoxIconLINProbeInfo2 = new PictureBox();
			this.pictureBoxIconLINProbeInfo3 = new PictureBox();
			this.pictureBoxIconLINProbeInfo4 = new PictureBox();
			this.checkBoxLogErrorFramesCAN1 = new CheckBox();
			this.checkBoxLogErrorFramesCAN2 = new CheckBox();
			this.checkBoxLogErrorFramesCAN3 = new CheckBox();
			this.checkBoxLogErrorFramesCAN4 = new CheckBox();
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
			this.groupBoxChannels.Controls.Add(this.tableLayoutPanelChannels);
			componentResourceManager.ApplyResources(this.groupBoxChannels, "groupBoxChannels");
			this.groupBoxChannels.Name = "groupBoxChannels";
			this.groupBoxChannels.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanelChannels, "tableLayoutPanelChannels");
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxCAN2, 1, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeCAN2, 5, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeCAN1, 5, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxOutputACKCAN2, 4, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateCAN1, 3, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxOutputACKCAN1, 4, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateCAN2, 3, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxCAN1, 1, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxCAN3, 1, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateCAN3, 3, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxOutputACKCAN3, 4, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeCAN3, 5, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxCAN4, 1, 3);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateCAN4, 3, 3);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxOutputACKCAN4, 4, 3);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeCAN4, 5, 3);
			this.tableLayoutPanelChannels.Controls.Add(this.pictureBoxIconLINProbeInfo1, 2, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.pictureBoxIconLINProbeInfo2, 2, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.pictureBoxIconLINProbeInfo3, 2, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.pictureBoxIconLINProbeInfo4, 2, 3);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLogErrorFramesCAN1, 6, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLogErrorFramesCAN2, 6, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLogErrorFramesCAN3, 6, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLogErrorFramesCAN4, 6, 3);
			this.tableLayoutPanelChannels.Name = "tableLayoutPanelChannels";
			componentResourceManager.ApplyResources(this.checkBoxCAN2, "checkBoxCAN2");
			this.checkBoxCAN2.Checked = true;
			this.checkBoxCAN2.CheckState = CheckState.Checked;
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
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxCAN1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN1.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxCAN1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN1.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxCAN1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxCAN1.IconAlignment2"));
			this.checkBoxCAN1.Name = "checkBoxCAN1";
			this.checkBoxCAN1.UseVisualStyleBackColor = true;
			this.checkBoxCAN1.CheckedChanged += new EventHandler(this.checkBoxCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxCAN3, "checkBoxCAN3");
			this.checkBoxCAN3.Checked = true;
			this.checkBoxCAN3.CheckState = CheckState.Checked;
			this.checkBoxCAN3.Name = "checkBoxCAN3";
			this.checkBoxCAN3.UseVisualStyleBackColor = true;
			this.checkBoxCAN3.CheckedChanged += new EventHandler(this.checkBoxCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.comboBoxBaudrateCAN3, "comboBoxBaudrateCAN3");
			this.comboBoxBaudrateCAN3.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrateCAN3.FormattingEnabled = true;
			this.comboBoxBaudrateCAN3.Name = "comboBoxBaudrateCAN3";
			this.comboBoxBaudrateCAN3.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrateCAN_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxOutputACKCAN3, "checkBoxOutputACKCAN3");
			this.checkBoxOutputACKCAN3.Name = "checkBoxOutputACKCAN3";
			this.checkBoxOutputACKCAN3.UseVisualStyleBackColor = true;
			this.checkBoxOutputACKCAN3.CheckedChanged += new EventHandler(this.checkBoxOutputACKCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeCAN3, "checkBoxKeepAwakeCAN3");
			this.checkBoxKeepAwakeCAN3.Name = "checkBoxKeepAwakeCAN3";
			this.checkBoxKeepAwakeCAN3.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeCAN3.CheckedChanged += new EventHandler(this.checkBoxKeepAwakeCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxCAN4, "checkBoxCAN4");
			this.checkBoxCAN4.Checked = true;
			this.checkBoxCAN4.CheckState = CheckState.Checked;
			this.checkBoxCAN4.Name = "checkBoxCAN4";
			this.checkBoxCAN4.UseVisualStyleBackColor = true;
			this.checkBoxCAN4.CheckedChanged += new EventHandler(this.checkBoxCAN_CheckedChanged);
			this.comboBoxBaudrateCAN4.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrateCAN4.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxBaudrateCAN4, "comboBoxBaudrateCAN4");
			this.comboBoxBaudrateCAN4.Name = "comboBoxBaudrateCAN4";
			this.comboBoxBaudrateCAN4.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrateCAN_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxOutputACKCAN4, "checkBoxOutputACKCAN4");
			this.checkBoxOutputACKCAN4.Name = "checkBoxOutputACKCAN4";
			this.checkBoxOutputACKCAN4.UseVisualStyleBackColor = true;
			this.checkBoxOutputACKCAN4.CheckedChanged += new EventHandler(this.checkBoxOutputACKCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeCAN4, "checkBoxKeepAwakeCAN4");
			this.checkBoxKeepAwakeCAN4.Name = "checkBoxKeepAwakeCAN4";
			this.checkBoxKeepAwakeCAN4.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeCAN4.CheckedChanged += new EventHandler(this.checkBoxKeepAwakeCAN_CheckedChanged);
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
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesCAN1, "checkBoxLogErrorFramesCAN1");
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN1.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesCAN1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN1.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN1.IconAlignment2"));
			this.checkBoxLogErrorFramesCAN1.Name = "checkBoxLogErrorFramesCAN1";
			this.checkBoxLogErrorFramesCAN1.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesCAN1.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesCAN1_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesCAN2, "checkBoxLogErrorFramesCAN2");
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN2.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesCAN2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN2.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN2.IconAlignment2"));
			this.checkBoxLogErrorFramesCAN2.Name = "checkBoxLogErrorFramesCAN2";
			this.checkBoxLogErrorFramesCAN2.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesCAN2.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesCAN1_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesCAN3, "checkBoxLogErrorFramesCAN3");
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN3, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN3.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesCAN3, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN3.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN3, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN3.IconAlignment2"));
			this.checkBoxLogErrorFramesCAN3.Name = "checkBoxLogErrorFramesCAN3";
			this.checkBoxLogErrorFramesCAN3.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesCAN3.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesCAN1_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxLogErrorFramesCAN4, "checkBoxLogErrorFramesCAN4");
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN4, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN4.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogErrorFramesCAN4, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN4.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogErrorFramesCAN4, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogErrorFramesCAN4.IconAlignment2"));
			this.checkBoxLogErrorFramesCAN4.Name = "checkBoxLogErrorFramesCAN4";
			this.checkBoxLogErrorFramesCAN4.UseVisualStyleBackColor = true;
			this.checkBoxLogErrorFramesCAN4.CheckedChanged += new EventHandler(this.checkBoxLogErrorFramesCAN1_CheckedChanged);
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
			base.Name = "CANChannelsGL2000";
			this.groupBoxChannels.ResumeLayout(false);
			this.tableLayoutPanelChannels.ResumeLayout(false);
			this.tableLayoutPanelChannels.PerformLayout();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo1).EndInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo2).EndInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo3).EndInit();
			((ISupportInitialize)this.pictureBoxIconLINProbeInfo4).EndInit();
			this.groupBoxDescription.ResumeLayout(false);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
