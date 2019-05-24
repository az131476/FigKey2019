using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.LINChannelsPage
{
	internal class LINChannelsGL1000 : UserControl, IValidationHost
	{
		private Dictionary<uint, CheckBox> channelNr2CheckBoxChannel;

		private Dictionary<uint, ComboBox> channelNr2ComboBoxBaudrate;

		private Dictionary<uint, CheckBox> channelNr2CheckBoxKeepAwake;

		private Dictionary<uint, CheckBox> channelNr2CheckBoxUseDbValues;

		private Dictionary<uint, LINprobeChannelRow> channelNr2LINprobeRow;

		private LINChannelConfiguration linChannelConfiguration;

		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isInitControls;

		private string appPathToLINprobeConfigurator;

		private int tableLayoutPanelLINprobeDesignHeight;

		private int groupBoxLINprobeMinimumHeight;

		private int tableRowLINprobeMinimumHeight;

		private IModelValidator modelValidator;

		private IContainer components;

		private RichTextBox richTextBoxDescription;

		private ComboBox comboBoxBaudrateLIN1;

		private CheckBox checkBoxKeepAwakeLIN1;

		private GroupBox groupBoxChannels;

		private TableLayoutPanel tableLayoutPanelChannels;

		private GroupBox groupBoxDescription;

		private CheckBox checkBoxLIN1;

		private CheckBox checkBoxLIN2;

		private CheckBox checkBoxKeepAwakeLIN2;

		private ComboBox comboBoxBaudrateLIN2;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private CheckBox checkBoxUseDbValues1;

		private CheckBox checkBoxUseDbValues2;

		private GroupBox groupBoxLinProbe;

		private ComboBox comboBoxCanChannelNr;

		private CheckBox checkBoxUseLinProbe;

		private Button buttonLinProbeConfigurator;

		private TableLayoutPanel tableLayoutPanelLevel1;

		private TableLayoutPanel tableLayoutPanelLINprobe;

		private LINprobeChannelRow linprobeChannelRow3;

		private LINprobeChannelRow linprobeChannelRow4;

		private LINprobeChannelRow linprobeChannelRow5;

		private LINprobeChannelRow linprobeChannelRow6;

		private LINprobeChannelRow linprobeChannelRow7;

		private LINprobeChannelRow linprobeChannelRow8;

		private LINprobeChannelRow linprobeChannelRow9;

		private LINprobeChannelRow linprobeChannelRow10;

		private LINprobeChannelRow linprobeChannelRow11;

		private LINprobeChannelRow linprobeChannelRow12;

		public LINChannelConfiguration LINChannelConfiguration
		{
			get
			{
				return this.linChannelConfiguration;
			}
			set
			{
				this.linChannelConfiguration = value;
				if (this.linChannelConfiguration != null)
				{
					if (this.ModelValidator != null)
					{
						ulong arg_45_0 = (ulong)this.ModelValidator.LoggerSpecifics.LIN.NumberOfChannels;
						long arg_44_0 = (long)this.linChannelConfiguration.LINChannels.Count;
					}
					this.UpdateGUI();
				}
			}
		}

		public IModelValidator ModelValidator
		{
			get
			{
				return this.modelValidator;
			}
			set
			{
				this.modelValidator = value;
				uint num = 3u;
				while ((ulong)num <= (ulong)((long)(this.channelNr2LINprobeRow.Count + 2)))
				{
					this.channelNr2LINprobeRow[num].ModelValidator = this.modelValidator;
					num += 1u;
				}
			}
		}

		public DisplayMode DisplayMode
		{
			get;
			set;
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		GUIElementManager_Control IValidationHost.GUIElementManager
		{
			get
			{
				return this.guiElementManager;
			}
		}

		PageValidator IValidationHost.PageValidator
		{
			get
			{
				return this.pageValidator;
			}
		}

		public LINChannelsGL1000()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.InitLINprobeChannelTable();
			this.isInitControls = false;
		}

		public void Init()
		{
			this.InitChannelNr2ControlLists();
			this.isInitControls = true;
			this.InitPossibleSpeedRates();
			this.InitCANChannelList();
			this.isInitControls = false;
			this.InitLINprobeConfiguratorAccess();
		}

		private void InitChannelNr2ControlLists()
		{
			this.channelNr2CheckBoxChannel = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxChannel.Add(1u, this.checkBoxLIN1);
			this.channelNr2CheckBoxChannel.Add(2u, this.checkBoxLIN2);
			this.channelNr2ComboBoxBaudrate = new Dictionary<uint, ComboBox>();
			this.channelNr2ComboBoxBaudrate.Add(1u, this.comboBoxBaudrateLIN1);
			this.channelNr2ComboBoxBaudrate.Add(2u, this.comboBoxBaudrateLIN2);
			this.channelNr2CheckBoxKeepAwake = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxKeepAwake.Add(1u, this.checkBoxKeepAwakeLIN1);
			this.channelNr2CheckBoxKeepAwake.Add(2u, this.checkBoxKeepAwakeLIN2);
			this.channelNr2CheckBoxUseDbValues = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxUseDbValues.Add(1u, this.checkBoxUseDbValues1);
			this.channelNr2CheckBoxUseDbValues.Add(2u, this.checkBoxUseDbValues2);
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
			IList<uint> standardLINBaudrates = GUIUtil.GetStandardLINBaudrates();
			foreach (uint current in standardLINBaudrates)
			{
				comboBox.Items.Add(GUIUtil.MapBaudrate2String(current));
			}
			comboBox.SelectedIndex = 0;
		}

		private void InitCANChannelList()
		{
			this.comboBoxCanChannelNr.Items.Clear();
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels; num += 1u)
			{
				this.comboBoxCanChannelNr.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
			this.comboBoxCanChannelNr.SelectedIndex = 0;
		}

		private void InitLINprobeConfiguratorAccess()
		{
			this.buttonLinProbeConfigurator.Enabled = RegistryServices.IsGinLINprobeInstalled(out this.appPathToLINprobeConfigurator);
		}

		private void InitLINprobeChannelTable()
		{
			this.appPathToLINprobeConfigurator = string.Empty;
			this.tableLayoutPanelLINprobeDesignHeight = this.tableLayoutPanelLINprobe.Height;
			this.groupBoxLINprobeMinimumHeight = this.groupBoxLinProbe.Height - this.tableLayoutPanelLINprobeDesignHeight + 1;
			this.tableLayoutPanelLevel1.RowStyles[1].SizeType = SizeType.Absolute;
			this.tableRowLINprobeMinimumHeight = (int)this.tableLayoutPanelLevel1.RowStyles[1].Height - this.tableLayoutPanelLINprobeDesignHeight + 1;
			this.channelNr2LINprobeRow = new Dictionary<uint, LINprobeChannelRow>();
			this.channelNr2LINprobeRow.Add(3u, this.linprobeChannelRow3);
			this.channelNr2LINprobeRow.Add(4u, this.linprobeChannelRow4);
			this.channelNr2LINprobeRow.Add(5u, this.linprobeChannelRow5);
			this.channelNr2LINprobeRow.Add(6u, this.linprobeChannelRow6);
			this.channelNr2LINprobeRow.Add(7u, this.linprobeChannelRow7);
			this.channelNr2LINprobeRow.Add(8u, this.linprobeChannelRow8);
			this.channelNr2LINprobeRow.Add(9u, this.linprobeChannelRow9);
			this.channelNr2LINprobeRow.Add(10u, this.linprobeChannelRow10);
			this.channelNr2LINprobeRow.Add(11u, this.linprobeChannelRow11);
			this.channelNr2LINprobeRow.Add(12u, this.linprobeChannelRow12);
			uint num = 3u;
			while ((ulong)num <= (ulong)((long)(this.channelNr2LINprobeRow.Count + 2)))
			{
				this.channelNr2LINprobeRow[num].ChannelNr = num;
				this.channelNr2LINprobeRow[num].ModelValidator = this.ModelValidator;
				((IValidatable)this.channelNr2LINprobeRow[num]).ValidationHost = this;
				num += 1u;
			}
		}

		private void checkBoxLIN_CheckedChanged(object sender, EventArgs e)
		{
			this.EnableControlsInRow(sender as CheckBox);
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBoxBaudrateLIN_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxKeepAwakeLIN_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxUseDbValues_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxUseLinProbe_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.comboBoxCanChannelNr.Enabled = this.checkBoxUseLinProbe.Checked;
			this.ValidateInput();
		}

		private void comboBoxCanChannelNr_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void buttonLinProbeConfigurator_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.appPathToLINprobeConfigurator))
			{
				FileSystemServices.LaunchFile(this.appPathToLINprobeConfigurator);
			}
		}

		void IValidationHost.RegisterForErrorProvider(Control control)
		{
			this.errorProviderFormat.SetIconAlignment(control, ErrorIconAlignment.MiddleLeft);
			this.errorProviderLocalModel.SetIconAlignment(control, ErrorIconAlignment.MiddleLeft);
			this.errorProviderGlobalModel.SetIconAlignment(control, ErrorIconAlignment.MiddleLeft);
		}

		public bool ValidateInput()
		{
			if (this.LINChannelConfiguration == null)
			{
				return false;
			}
			this.DisplayBaudratesFromDb();
			bool flag = true;
			bool flag2 = false;
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
			uint num = 1u;
			bool flag3;
			while ((ulong)num <= (ulong)((long)this.channelNr2CheckBoxChannel.Count))
			{
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.channelNr2CheckBoxChannel[num].Checked, this.LINChannelConfiguration.GetLINChannel(num).IsActive, this.guiElementManager.GetGUIElement(this.channelNr2CheckBoxChannel[num]), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<uint>(this.GetSelectedBaudrate(num), this.LINChannelConfiguration.GetLINChannel(num).SpeedRate, this.guiElementManager.GetGUIElement(this.channelNr2ComboBoxBaudrate[num]), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.channelNr2CheckBoxKeepAwake[num].Checked, this.LINChannelConfiguration.GetLINChannel(num).IsKeepAwakeActive, this.guiElementManager.GetGUIElement(this.channelNr2CheckBoxKeepAwake[num]), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.channelNr2CheckBoxUseDbValues[num].Checked, this.LINChannelConfiguration.GetLINChannel(num).UseDbConfigValues, this.guiElementManager.GetGUIElement(this.channelNr2CheckBoxUseDbValues[num]), out flag3);
				flag2 |= flag3;
				num += 1u;
			}
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxUseLinProbe.Checked, this.LINChannelConfiguration.IsUsingLinProbe, this.guiElementManager.GetGUIElement(this.checkBoxUseLinProbe), out flag3);
			flag2 |= flag3;
			if (this.checkBoxUseLinProbe.Checked)
			{
				flag &= this.pageValidator.Control.UpdateModel<uint>(GUIUtil.MapCANChannelString2Number(this.comboBoxCanChannelNr.SelectedItem.ToString()), this.LINChannelConfiguration.CANChannelNrUsedForLinProbe, this.guiElementManager.GetGUIElement(this.comboBoxCanChannelNr), out flag3);
				flag2 |= flag3;
				foreach (LINprobeChannelRow current in this.channelNr2LINprobeRow.Values)
				{
					flag &= ((IValidatable)current).ValidateInput(ref flag2);
				}
			}
			flag &= this.ModelValidator.Validate(this.linChannelConfiguration, flag2, this.pageValidator);
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
			this.channelNr2ComboBoxBaudrate[channelNr].Enabled = (this.channelNr2CheckBoxChannel[channelNr].Checked && !this.channelNr2CheckBoxUseDbValues[channelNr].Checked);
			this.channelNr2CheckBoxKeepAwake[channelNr].Enabled = this.channelNr2CheckBoxChannel[channelNr].Checked;
			this.channelNr2CheckBoxUseDbValues[channelNr].Enabled = this.channelNr2CheckBoxChannel[channelNr].Checked;
		}

		private void UpdateGUI()
		{
			if (this.linChannelConfiguration == null)
			{
				return;
			}
			this.isInitControls = true;
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.linChannelConfiguration.LINChannels.Count))
			{
				LINChannel lINChannel = this.linChannelConfiguration.GetLINChannel(num);
				this.channelNr2CheckBoxChannel[num].Checked = lINChannel.IsActive.Value;
				this.channelNr2ComboBoxBaudrate[num].SelectedItem = GUIUtil.MapBaudrate2String(lINChannel.SpeedRate.Value);
				this.channelNr2CheckBoxUseDbValues[num].Checked = lINChannel.UseDbConfigValues.Value;
				this.DisplayBaudrateFromDb(num);
				this.EnableControlsInRow(num);
				this.channelNr2CheckBoxKeepAwake[num].Checked = lINChannel.IsKeepAwakeActive.Value;
				num += 1u;
			}
			this.checkBoxUseLinProbe.Checked = this.linChannelConfiguration.IsUsingLinProbe.Value;
			this.DisplayLINprobeChannelsTable(this.checkBoxUseLinProbe.Checked);
			this.comboBoxCanChannelNr.Enabled = this.checkBoxUseLinProbe.Checked;
			this.comboBoxCanChannelNr.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.linChannelConfiguration.CANChannelNrUsedForLinProbe.Value);
			uint num2 = 3u;
			while ((ulong)num2 <= (ulong)((long)(this.channelNr2LINprobeRow.Count + 2)))
			{
				this.channelNr2LINprobeRow[num2].LINprobeChannel = this.linChannelConfiguration.GetLINprobeChannel(num2);
				num2 += 1u;
			}
			this.isInitControls = false;
			this.ValidateInput();
		}

		private void DisplayLINprobeChannelsTable(bool isVisible)
		{
			if (isVisible)
			{
				int num = GUIUtil.GuiScaleY(this.tableLayoutPanelLINprobeDesignHeight);
				this.tableLayoutPanelLINprobe.Height = num;
				this.groupBoxLinProbe.Height = GUIUtil.GuiScaleY(this.groupBoxLINprobeMinimumHeight) + num;
				this.tableLayoutPanelLevel1.RowStyles[1].SizeType = SizeType.Absolute;
				this.tableLayoutPanelLevel1.RowStyles[1].Height = (float)(this.tableRowLINprobeMinimumHeight + num);
				return;
			}
			this.tableLayoutPanelLINprobe.Height = 0;
			this.groupBoxLinProbe.Height = this.groupBoxLINprobeMinimumHeight;
			this.tableLayoutPanelLevel1.RowStyles[1].SizeType = SizeType.Absolute;
			this.tableLayoutPanelLevel1.RowStyles[1].Height = (float)this.tableRowLINprobeMinimumHeight;
		}

		private void DisplayBaudratesFromDb()
		{
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.linChannelConfiguration.LINChannels.Count))
			{
				this.DisplayBaudrateFromDb(num);
				num += 1u;
			}
		}

		private void DisplayBaudrateFromDb(uint channelNr)
		{
			int num;
			string text;
			int num2;
			bool linChipConfigFromLdfDatabase = this.ModelValidator.DatabaseServices.GetLinChipConfigFromLdfDatabase(channelNr, out num, out text, out num2);
			this.channelNr2CheckBoxUseDbValues[channelNr].Text = (linChipConfigFromLdfDatabase ? string.Format(Resources.LINChannelDbBaudrate, num) : string.Format(Resources.LINChannelDbBaudrate, "-"));
		}

		private uint GetSelectedBaudrate(uint channelNr)
		{
			return this.GetSelectedBaudrate(this.channelNr2ComboBoxBaudrate[channelNr]);
		}

		private uint GetSelectedBaudrate(ComboBox comboBox)
		{
			return GUIUtil.MapString2Baudrate(comboBox.SelectedItem.ToString());
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LINChannelsGL1000));
			this.richTextBoxDescription = new RichTextBox();
			this.comboBoxBaudrateLIN1 = new ComboBox();
			this.checkBoxKeepAwakeLIN1 = new CheckBox();
			this.groupBoxChannels = new GroupBox();
			this.tableLayoutPanelChannels = new TableLayoutPanel();
			this.checkBoxUseDbValues2 = new CheckBox();
			this.checkBoxUseDbValues1 = new CheckBox();
			this.checkBoxLIN2 = new CheckBox();
			this.comboBoxBaudrateLIN2 = new ComboBox();
			this.checkBoxLIN1 = new CheckBox();
			this.checkBoxKeepAwakeLIN2 = new CheckBox();
			this.groupBoxDescription = new GroupBox();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.checkBoxUseLinProbe = new CheckBox();
			this.comboBoxCanChannelNr = new ComboBox();
			this.groupBoxLinProbe = new GroupBox();
			this.tableLayoutPanelLINprobe = new TableLayoutPanel();
			this.linprobeChannelRow3 = new LINprobeChannelRow();
			this.linprobeChannelRow4 = new LINprobeChannelRow();
			this.linprobeChannelRow5 = new LINprobeChannelRow();
			this.linprobeChannelRow6 = new LINprobeChannelRow();
			this.linprobeChannelRow7 = new LINprobeChannelRow();
			this.linprobeChannelRow8 = new LINprobeChannelRow();
			this.linprobeChannelRow9 = new LINprobeChannelRow();
			this.linprobeChannelRow10 = new LINprobeChannelRow();
			this.linprobeChannelRow11 = new LINprobeChannelRow();
			this.linprobeChannelRow12 = new LINprobeChannelRow();
			this.buttonLinProbeConfigurator = new Button();
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.tableLayoutPanelLevel1 = new TableLayoutPanel();
			this.groupBoxChannels.SuspendLayout();
			this.tableLayoutPanelChannels.SuspendLayout();
			this.groupBoxDescription.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			this.groupBoxLinProbe.SuspendLayout();
			this.tableLayoutPanelLINprobe.SuspendLayout();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			this.tableLayoutPanelLevel1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.richTextBoxDescription, "richTextBoxDescription");
			this.richTextBoxDescription.BorderStyle = BorderStyle.None;
			this.richTextBoxDescription.Name = "richTextBoxDescription";
			this.richTextBoxDescription.ReadOnly = true;
			componentResourceManager.ApplyResources(this.comboBoxBaudrateLIN1, "comboBoxBaudrateLIN1");
			this.comboBoxBaudrateLIN1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrateLIN1.FormattingEnabled = true;
			this.comboBoxBaudrateLIN1.Name = "comboBoxBaudrateLIN1";
			this.comboBoxBaudrateLIN1.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrateLIN_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeLIN1, "checkBoxKeepAwakeLIN1");
			this.checkBoxKeepAwakeLIN1.Name = "checkBoxKeepAwakeLIN1";
			this.checkBoxKeepAwakeLIN1.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeLIN1.CheckedChanged += new EventHandler(this.checkBoxKeepAwakeLIN_CheckedChanged);
			this.groupBoxChannels.Controls.Add(this.tableLayoutPanelChannels);
			componentResourceManager.ApplyResources(this.groupBoxChannels, "groupBoxChannels");
			this.groupBoxChannels.Name = "groupBoxChannels";
			this.groupBoxChannels.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanelChannels, "tableLayoutPanelChannels");
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxUseDbValues2, 3, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxUseDbValues1, 3, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLIN2, 0, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateLIN1, 1, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.comboBoxBaudrateLIN2, 1, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxLIN1, 0, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeLIN1, 4, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeLIN2, 4, 1);
			this.tableLayoutPanelChannels.Name = "tableLayoutPanelChannels";
			componentResourceManager.ApplyResources(this.checkBoxUseDbValues2, "checkBoxUseDbValues2");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxUseDbValues2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxUseDbValues2.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxUseDbValues2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxUseDbValues2.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxUseDbValues2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxUseDbValues2.IconAlignment2"));
			this.checkBoxUseDbValues2.Name = "checkBoxUseDbValues2";
			this.checkBoxUseDbValues2.UseVisualStyleBackColor = true;
			this.checkBoxUseDbValues2.CheckedChanged += new EventHandler(this.checkBoxUseDbValues_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxUseDbValues1, "checkBoxUseDbValues1");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxUseDbValues1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxUseDbValues1.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxUseDbValues1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxUseDbValues1.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxUseDbValues1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxUseDbValues1.IconAlignment2"));
			this.checkBoxUseDbValues1.Name = "checkBoxUseDbValues1";
			this.checkBoxUseDbValues1.UseVisualStyleBackColor = true;
			this.checkBoxUseDbValues1.CheckedChanged += new EventHandler(this.checkBoxUseDbValues_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxLIN2, "checkBoxLIN2");
			this.checkBoxLIN2.Checked = true;
			this.checkBoxLIN2.CheckState = CheckState.Checked;
			this.checkBoxLIN2.Name = "checkBoxLIN2";
			this.checkBoxLIN2.UseVisualStyleBackColor = true;
			this.checkBoxLIN2.CheckedChanged += new EventHandler(this.checkBoxLIN_CheckedChanged);
			componentResourceManager.ApplyResources(this.comboBoxBaudrateLIN2, "comboBoxBaudrateLIN2");
			this.comboBoxBaudrateLIN2.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxBaudrateLIN2.FormattingEnabled = true;
			this.comboBoxBaudrateLIN2.Name = "comboBoxBaudrateLIN2";
			this.comboBoxBaudrateLIN2.SelectedIndexChanged += new EventHandler(this.comboBoxBaudrateLIN_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxLIN1, "checkBoxLIN1");
			this.checkBoxLIN1.Checked = true;
			this.checkBoxLIN1.CheckState = CheckState.Checked;
			this.checkBoxLIN1.Name = "checkBoxLIN1";
			this.checkBoxLIN1.UseVisualStyleBackColor = true;
			this.checkBoxLIN1.CheckedChanged += new EventHandler(this.checkBoxLIN_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeLIN2, "checkBoxKeepAwakeLIN2");
			this.checkBoxKeepAwakeLIN2.Name = "checkBoxKeepAwakeLIN2";
			this.checkBoxKeepAwakeLIN2.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeLIN2.CheckedChanged += new EventHandler(this.checkBoxKeepAwakeLIN_CheckedChanged);
			this.groupBoxDescription.Controls.Add(this.richTextBoxDescription);
			componentResourceManager.ApplyResources(this.groupBoxDescription, "groupBoxDescription");
			this.groupBoxDescription.Name = "groupBoxDescription";
			this.groupBoxDescription.TabStop = false;
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.checkBoxUseLinProbe, "checkBoxUseLinProbe");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxUseLinProbe, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxUseLinProbe.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxUseLinProbe, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxUseLinProbe.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxUseLinProbe, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxUseLinProbe.IconAlignment2"));
			this.checkBoxUseLinProbe.Name = "checkBoxUseLinProbe";
			this.checkBoxUseLinProbe.UseVisualStyleBackColor = true;
			this.checkBoxUseLinProbe.CheckedChanged += new EventHandler(this.checkBoxUseLinProbe_CheckedChanged);
			this.comboBoxCanChannelNr.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxCanChannelNr.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxCanChannelNr, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxCanChannelNr.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxCanChannelNr, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxCanChannelNr.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxCanChannelNr, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxCanChannelNr.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxCanChannelNr, "comboBoxCanChannelNr");
			this.comboBoxCanChannelNr.Name = "comboBoxCanChannelNr";
			this.comboBoxCanChannelNr.SelectedIndexChanged += new EventHandler(this.comboBoxCanChannelNr_SelectedIndexChanged);
			this.groupBoxLinProbe.Controls.Add(this.tableLayoutPanelLINprobe);
			this.groupBoxLinProbe.Controls.Add(this.buttonLinProbeConfigurator);
			this.groupBoxLinProbe.Controls.Add(this.comboBoxCanChannelNr);
			this.groupBoxLinProbe.Controls.Add(this.checkBoxUseLinProbe);
			componentResourceManager.ApplyResources(this.groupBoxLinProbe, "groupBoxLinProbe");
			this.groupBoxLinProbe.Name = "groupBoxLinProbe";
			this.groupBoxLinProbe.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanelLINprobe, "tableLayoutPanelLINprobe");
			this.tableLayoutPanelLINprobe.Controls.Add(this.linprobeChannelRow3, 1, 0);
			this.tableLayoutPanelLINprobe.Controls.Add(this.linprobeChannelRow4, 1, 1);
			this.tableLayoutPanelLINprobe.Controls.Add(this.linprobeChannelRow5, 1, 2);
			this.tableLayoutPanelLINprobe.Controls.Add(this.linprobeChannelRow6, 1, 3);
			this.tableLayoutPanelLINprobe.Controls.Add(this.linprobeChannelRow7, 1, 4);
			this.tableLayoutPanelLINprobe.Controls.Add(this.linprobeChannelRow8, 1, 5);
			this.tableLayoutPanelLINprobe.Controls.Add(this.linprobeChannelRow9, 1, 6);
			this.tableLayoutPanelLINprobe.Controls.Add(this.linprobeChannelRow10, 1, 7);
			this.tableLayoutPanelLINprobe.Controls.Add(this.linprobeChannelRow11, 1, 8);
			this.tableLayoutPanelLINprobe.Controls.Add(this.linprobeChannelRow12, 1, 9);
			this.tableLayoutPanelLINprobe.Name = "tableLayoutPanelLINprobe";
			this.linprobeChannelRow3.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.linprobeChannelRow3, "linprobeChannelRow3");
			this.linprobeChannelRow3.LINprobeChannel = null;
			this.linprobeChannelRow3.ModelValidator = null;
			this.linprobeChannelRow3.Name = "linprobeChannelRow3";
			this.linprobeChannelRow4.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.linprobeChannelRow4, "linprobeChannelRow4");
			this.linprobeChannelRow4.LINprobeChannel = null;
			this.linprobeChannelRow4.ModelValidator = null;
			this.linprobeChannelRow4.Name = "linprobeChannelRow4";
			this.linprobeChannelRow5.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.linprobeChannelRow5, "linprobeChannelRow5");
			this.linprobeChannelRow5.LINprobeChannel = null;
			this.linprobeChannelRow5.ModelValidator = null;
			this.linprobeChannelRow5.Name = "linprobeChannelRow5";
			this.linprobeChannelRow6.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.linprobeChannelRow6, "linprobeChannelRow6");
			this.linprobeChannelRow6.LINprobeChannel = null;
			this.linprobeChannelRow6.ModelValidator = null;
			this.linprobeChannelRow6.Name = "linprobeChannelRow6";
			this.linprobeChannelRow7.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.linprobeChannelRow7, "linprobeChannelRow7");
			this.linprobeChannelRow7.LINprobeChannel = null;
			this.linprobeChannelRow7.ModelValidator = null;
			this.linprobeChannelRow7.Name = "linprobeChannelRow7";
			this.linprobeChannelRow8.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.linprobeChannelRow8, "linprobeChannelRow8");
			this.linprobeChannelRow8.LINprobeChannel = null;
			this.linprobeChannelRow8.ModelValidator = null;
			this.linprobeChannelRow8.Name = "linprobeChannelRow8";
			this.linprobeChannelRow9.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.linprobeChannelRow9, "linprobeChannelRow9");
			this.linprobeChannelRow9.LINprobeChannel = null;
			this.linprobeChannelRow9.ModelValidator = null;
			this.linprobeChannelRow9.Name = "linprobeChannelRow9";
			this.linprobeChannelRow10.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.linprobeChannelRow10, "linprobeChannelRow10");
			this.linprobeChannelRow10.LINprobeChannel = null;
			this.linprobeChannelRow10.ModelValidator = null;
			this.linprobeChannelRow10.Name = "linprobeChannelRow10";
			this.linprobeChannelRow11.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.linprobeChannelRow11, "linprobeChannelRow11");
			this.linprobeChannelRow11.LINprobeChannel = null;
			this.linprobeChannelRow11.ModelValidator = null;
			this.linprobeChannelRow11.Name = "linprobeChannelRow11";
			this.linprobeChannelRow12.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.linprobeChannelRow12, "linprobeChannelRow12");
			this.linprobeChannelRow12.LINprobeChannel = null;
			this.linprobeChannelRow12.ModelValidator = null;
			this.linprobeChannelRow12.Name = "linprobeChannelRow12";
			componentResourceManager.ApplyResources(this.buttonLinProbeConfigurator, "buttonLinProbeConfigurator");
			this.buttonLinProbeConfigurator.Name = "buttonLinProbeConfigurator";
			this.buttonLinProbeConfigurator.UseVisualStyleBackColor = true;
			this.buttonLinProbeConfigurator.Click += new EventHandler(this.buttonLinProbeConfigurator_Click);
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this.tableLayoutPanelLevel1, "tableLayoutPanelLevel1");
			this.tableLayoutPanelLevel1.Controls.Add(this.groupBoxChannels, 0, 0);
			this.tableLayoutPanelLevel1.Controls.Add(this.groupBoxDescription, 0, 2);
			this.tableLayoutPanelLevel1.Controls.Add(this.groupBoxLinProbe, 0, 1);
			this.tableLayoutPanelLevel1.Name = "tableLayoutPanelLevel1";
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.tableLayoutPanelLevel1);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "LINChannelsGL1000";
			this.groupBoxChannels.ResumeLayout(false);
			this.tableLayoutPanelChannels.ResumeLayout(false);
			this.tableLayoutPanelChannels.PerformLayout();
			this.groupBoxDescription.ResumeLayout(false);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			this.groupBoxLinProbe.ResumeLayout(false);
			this.groupBoxLinProbe.PerformLayout();
			this.tableLayoutPanelLINprobe.ResumeLayout(false);
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			this.tableLayoutPanelLevel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
