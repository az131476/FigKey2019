using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.AnalogInputsPage
{
	internal class AnalogInputsGL1000 : UserControl
	{
		private Dictionary<uint, CheckBox> inputNr2CheckBoxInput;

		private Dictionary<uint, ComboBox> inputNr2ComboBoxFrequency;

		private Dictionary<uint, TextBox> inputNr2TextBoxCANId;

		private AnalogInputConfiguration analogInputConfiguration;

		private DisplayMode displayMode;

		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isInitControls;

		private IContainer components;

		private GroupBox groupBoxAnalogInputs;

		private TableLayoutPanel tableLayoutPanelAnalogInputs;

		private CheckBox checkBoxAnalogInput1;

		private CheckBox checkBoxAnalogInput2;

		private CheckBox checkBoxAnalogInput3;

		private CheckBox checkBoxAnalogInput4;

		private ComboBox comboBoxAnalogInputFrequency1;

		private ComboBox comboBoxAnalogInputFrequency2;

		private ComboBox comboBoxAnalogInputFrequency3;

		private ComboBox comboBoxAnalogInputFrequency4;

		private TextBox textBoxAnalogInputCANID1;

		private TextBox textBoxAnalogInputCANID2;

		private TextBox textBoxAnalogInputCANID3;

		private TextBox textBoxAnalogInputCANID4;

		private GroupBox groupBoxMappingMode;

		private RadioButton radioButtonMappingModeFixed;

		private RadioButton radioButtonMappingModeContinuous;

		private RadioButton radioButtonMappingModeIndividual;

		private Label labelCANID1;

		private Label labelCANID2;

		private Label labelCANID3;

		private Label labelCANID4;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private GroupBox groupBoxSendOnChannel;

		private ComboBox comboBoxChannel;

		private ToolTip toolTip;

		private CheckBox checkBoxAveraging;

		public AnalogInputConfiguration AnalogInputConfiguration
		{
			get
			{
				return this.analogInputConfiguration;
			}
			set
			{
				this.analogInputConfiguration = value;
				if (this.analogInputConfiguration != null)
				{
					if (this.ModelValidator != null)
					{
						ulong arg_45_0 = (ulong)this.ModelValidator.LoggerSpecifics.IO.NumberOfAnalogInputs;
						long arg_44_0 = (long)this.analogInputConfiguration.AnalogInputs.Count;
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
			get
			{
				return this.displayMode;
			}
			set
			{
				this.displayMode = value;
				if (this.inputNr2TextBoxCANId != null && this.analogInputConfiguration != null)
				{
					uint num = 1u;
					while ((ulong)num <= (ulong)((long)this.inputNr2TextBoxCANId.Count))
					{
						uint value2 = this.analogInputConfiguration.GetAnalogInput(num).MappedCANId.Value;
						bool value3 = this.analogInputConfiguration.GetAnalogInput(num).IsMappedCANIdExtended.Value;
						this.inputNr2TextBoxCANId[num].Text = GUIUtil.CANIdToDisplayString(value2, value3);
						num += 1u;
					}
					this.ValidateInput();
				}
			}
		}

		public AnalogInputsGL1000()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.isInitControls = false;
		}

		public void Init()
		{
			this.InitInputNr2ControlLists();
			this.isInitControls = true;
			this.InitPossibleFrequencies();
			this.InitChannelComboBox();
			this.isInitControls = false;
		}

		private void InitInputNr2ControlLists()
		{
			this.inputNr2CheckBoxInput = new Dictionary<uint, CheckBox>();
			this.inputNr2CheckBoxInput.Add(1u, this.checkBoxAnalogInput1);
			this.inputNr2CheckBoxInput.Add(2u, this.checkBoxAnalogInput2);
			this.inputNr2CheckBoxInput.Add(3u, this.checkBoxAnalogInput3);
			this.inputNr2CheckBoxInput.Add(4u, this.checkBoxAnalogInput4);
			this.inputNr2ComboBoxFrequency = new Dictionary<uint, ComboBox>();
			this.inputNr2ComboBoxFrequency.Add(1u, this.comboBoxAnalogInputFrequency1);
			this.inputNr2ComboBoxFrequency.Add(2u, this.comboBoxAnalogInputFrequency2);
			this.inputNr2ComboBoxFrequency.Add(3u, this.comboBoxAnalogInputFrequency3);
			this.inputNr2ComboBoxFrequency.Add(4u, this.comboBoxAnalogInputFrequency4);
			this.inputNr2TextBoxCANId = new Dictionary<uint, TextBox>();
			this.inputNr2TextBoxCANId.Add(1u, this.textBoxAnalogInputCANID1);
			this.inputNr2TextBoxCANId.Add(2u, this.textBoxAnalogInputCANID2);
			this.inputNr2TextBoxCANId.Add(3u, this.textBoxAnalogInputCANID3);
			this.inputNr2TextBoxCANId.Add(4u, this.textBoxAnalogInputCANID4);
		}

		private void InitPossibleFrequencies()
		{
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.inputNr2ComboBoxFrequency.Count))
			{
				this.InitPossibleFrequencies(this.inputNr2ComboBoxFrequency[num]);
				num += 1u;
			}
		}

		private void InitPossibleFrequencies(ComboBox comboBox)
		{
			comboBox.Items.Clear();
			IList<uint> standardAnalogInputFrequencies = GUIUtil.GetStandardAnalogInputFrequencies();
			foreach (uint current in standardAnalogInputFrequencies)
			{
				comboBox.Items.Add(GUIUtil.MapAnalogInputsFrequency2String(current));
			}
			if (comboBox.Items.Count > 0)
			{
				comboBox.SelectedIndex = 0;
			}
		}

		private void InitChannelComboBox()
		{
			this.comboBoxChannel.Items.Clear();
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels; num += 1u)
			{
				this.comboBoxChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
			uint num2 = this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels + 1u;
			for (uint num3 = num2; num3 < num2 + this.ModelValidator.LoggerSpecifics.CAN.NumberOfVirtualChannels; num3 += 1u)
			{
				this.comboBoxChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num3) + Resources.VirtualChannelPostfix);
			}
			if (this.comboBoxChannel.Items.Count > 0)
			{
				this.comboBoxChannel.SelectedIndex = 0;
			}
		}

		private void checkBoxAnalogInput_CheckedChanged(object sender, EventArgs e)
		{
			this.EnableControlsInRow(sender as CheckBox);
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBoxAnalogInputFrequency_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void radioButtonMappingMode_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.EnableControlsForMappingMode(this.GetMappingMode());
			this.ValidateInput();
		}

		private void textBoxAnalogInputCANID_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBoxChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void OnRadioButtonMappingMode_MouseEnter(object sender, EventArgs e)
		{
			if (this.radioButtonMappingModeFixed == sender as RadioButton)
			{
				this.toolTip.SetToolTip(this.radioButtonMappingModeFixed, Resources.AnalogInputsMappingModeFixedGL1000);
				return;
			}
			if (this.radioButtonMappingModeIndividual == sender as RadioButton)
			{
				this.toolTip.SetToolTip(this.radioButtonMappingModeIndividual, Resources.AnalogInputsMappingModeIndividual);
				return;
			}
			if (this.radioButtonMappingModeContinuous == sender as RadioButton)
			{
				this.toolTip.SetToolTip(this.radioButtonMappingModeContinuous, Resources.AnalogInputsMappingModeContinuous);
			}
		}

		private void checkBoxAveraging_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		public bool ValidateInput()
		{
			if (this.analogInputConfiguration == null)
			{
				return false;
			}
			bool flag = true;
			bool flag2 = false;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			bool flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxAveraging.Checked, this.AnalogInputConfiguration.Averaging, this.guiElementManager.GetGUIElement(this.checkBoxAveraging), out flag3);
			flag2 |= flag3;
			AnalogInputsCANMappingMode mappingMode = this.GetMappingMode();
			flag &= this.pageValidator.Control.UpdateModel<AnalogInputsCANMappingMode>(mappingMode, this.AnalogInputConfiguration.AnalogInputsCANMappingMode, this.guiElementManager.GetGUIElement(this.radioButtonMappingModeFixed), out flag3);
			flag2 |= flag3;
			if (this.comboBoxChannel.Enabled)
			{
				uint selectedChannel = this.GetSelectedChannel();
				flag &= this.pageValidator.Control.UpdateModel<uint>(selectedChannel, this.AnalogInputConfiguration.CanChannel, this.guiElementManager.GetGUIElement(this.comboBoxChannel), out flag3);
				flag2 |= flag3;
			}
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.inputNr2CheckBoxInput[1u].Checked, this.AnalogInputConfiguration.GetAnalogInput(1u).IsActive, this.guiElementManager.GetGUIElement(this.inputNr2CheckBoxInput[1u]), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.UpdateModel<uint>(this.GetSelectedFrequency(this.inputNr2ComboBoxFrequency[1u]), this.analogInputConfiguration.GetAnalogInput(1u).Frequency, this.guiElementManager.GetGUIElement(this.inputNr2ComboBoxFrequency[1u]), out flag3);
			flag2 |= flag3;
			bool flag4 = false;
			if (AnalogInputsCANMappingMode.IndividualIDs != mappingMode || this.inputNr2TextBoxCANId[1u].Enabled)
			{
				flag4 = this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.inputNr2TextBoxCANId[1u].Text, this.analogInputConfiguration.GetAnalogInput(1u).MappedCANId, this.analogInputConfiguration.GetAnalogInput(1u).IsMappedCANIdExtended, this.guiElementManager.GetGUIElement(this.inputNr2TextBoxCANId[1u]), out flag3);
				flag &= flag4;
				flag2 |= flag3;
			}
			uint num = 2u;
			while ((ulong)num <= (ulong)((long)this.inputNr2TextBoxCANId.Count))
			{
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.inputNr2CheckBoxInput[num].Checked, this.AnalogInputConfiguration.GetAnalogInput(num).IsActive, this.guiElementManager.GetGUIElement(this.inputNr2CheckBoxInput[num]), out flag3);
				flag2 |= flag3;
				if (this.AnalogInputConfiguration.GetAnalogInput(num).IsActive.Value || mappingMode == AnalogInputsCANMappingMode.SameFixedIDs)
				{
					uint value;
					if (mappingMode == AnalogInputsCANMappingMode.SameFixedIDs)
					{
						value = this.analogInputConfiguration.GetAnalogInput(1u).Frequency.Value;
					}
					else
					{
						value = this.GetSelectedFrequency(this.inputNr2ComboBoxFrequency[num]);
					}
					flag &= this.pageValidator.Control.UpdateModel<uint>(value, this.analogInputConfiguration.GetAnalogInput(num).Frequency, this.guiElementManager.GetGUIElement(this.inputNr2ComboBoxFrequency[num]), out flag3);
					flag2 |= flag3;
				}
				if ((AnalogInputsCANMappingMode.IndividualIDs == mappingMode && this.inputNr2TextBoxCANId[num].Enabled) || (AnalogInputsCANMappingMode.IndividualIDs != mappingMode && flag4))
				{
					string valueAsStringFromGUI;
					if (AnalogInputsCANMappingMode.IndividualIDs != mappingMode)
					{
						bool isFixedIdsMode = AnalogInputsCANMappingMode.SameFixedIDs == mappingMode;
						uint canId = GUIUtil.CalculateMappedCANId(this.analogInputConfiguration.GetAnalogInput(1u).MappedCANId.Value, this.analogInputConfiguration.GetAnalogInput(1u).IsMappedCANIdExtended.Value, num, isFixedIdsMode);
						valueAsStringFromGUI = GUIUtil.CANIdToDisplayString(canId, this.analogInputConfiguration.GetAnalogInput(1u).IsMappedCANIdExtended.Value);
					}
					else
					{
						valueAsStringFromGUI = this.inputNr2TextBoxCANId[num].Text;
					}
					flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(valueAsStringFromGUI, this.analogInputConfiguration.GetAnalogInput(num).MappedCANId, this.analogInputConfiguration.GetAnalogInput(num).IsMappedCANIdExtended, this.guiElementManager.GetGUIElement(this.inputNr2TextBoxCANId[num]), out flag3);
					flag2 |= flag3;
				}
				num += 1u;
			}
			flag &= this.ModelValidator.Validate(this.analogInputConfiguration, flag2, this.pageValidator);
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
			KeyValuePair<uint, CheckBox> keyValuePair = this.inputNr2CheckBoxInput.FirstOrDefault((KeyValuePair<uint, CheckBox> r) => r.Value == checkBox);
			if (keyValuePair.Equals(default(KeyValuePair<uint, CheckBox>)))
			{
				return;
			}
			uint key = keyValuePair.Key;
			if (this.inputNr2CheckBoxInput[key].Checked)
			{
				this.comboBoxChannel.Enabled = true;
				this.checkBoxAveraging.Enabled = true;
			}
			if (key > 1u)
			{
				if (this.analogInputConfiguration.AnalogInputsCANMappingMode.Value == AnalogInputsCANMappingMode.SameFixedIDs)
				{
					this.inputNr2ComboBoxFrequency[key].Enabled = false;
					this.inputNr2TextBoxCANId[key].Enabled = false;
					return;
				}
				if (this.analogInputConfiguration.AnalogInputsCANMappingMode.Value == AnalogInputsCANMappingMode.ContinuousIndividualIDs)
				{
					this.inputNr2ComboBoxFrequency[key].Enabled = this.inputNr2CheckBoxInput[key].Checked;
					this.inputNr2TextBoxCANId[key].Enabled = false;
					return;
				}
				this.inputNr2ComboBoxFrequency[key].Enabled = this.inputNr2CheckBoxInput[key].Checked;
				this.inputNr2TextBoxCANId[key].Enabled = this.inputNr2CheckBoxInput[key].Checked;
				return;
			}
			else
			{
				if (this.analogInputConfiguration.AnalogInputsCANMappingMode.Value == AnalogInputsCANMappingMode.SameFixedIDs)
				{
					this.inputNr2ComboBoxFrequency[key].Enabled = true;
				}
				else
				{
					this.inputNr2ComboBoxFrequency[key].Enabled = this.inputNr2CheckBoxInput[key].Checked;
				}
				if (this.analogInputConfiguration.AnalogInputsCANMappingMode.Value == AnalogInputsCANMappingMode.SameFixedIDs || this.analogInputConfiguration.AnalogInputsCANMappingMode.Value == AnalogInputsCANMappingMode.ContinuousIndividualIDs)
				{
					this.inputNr2TextBoxCANId[key].Enabled = true;
					return;
				}
				this.inputNr2TextBoxCANId[key].Enabled = this.inputNr2CheckBoxInput[key].Checked;
				return;
			}
		}

		private uint GetSelectedFrequency(uint inputNr)
		{
			return this.GetSelectedFrequency(this.inputNr2ComboBoxFrequency[inputNr]);
		}

		private uint GetSelectedFrequency(ComboBox comboBox)
		{
			return GUIUtil.MapString2AnalogInputsFrequency(comboBox.SelectedItem.ToString());
		}

		private AnalogInputsCANMappingMode GetMappingMode()
		{
			if (this.radioButtonMappingModeContinuous.Checked)
			{
				return AnalogInputsCANMappingMode.ContinuousIndividualIDs;
			}
			if (this.radioButtonMappingModeFixed.Checked)
			{
				return AnalogInputsCANMappingMode.SameFixedIDs;
			}
			return AnalogInputsCANMappingMode.IndividualIDs;
		}

		private uint GetSelectedChannel()
		{
			return GUIUtil.MapCANChannelString2Number(this.comboBoxChannel.SelectedItem.ToString());
		}

		private void UpdateGUI()
		{
			if (this.analogInputConfiguration == null)
			{
				return;
			}
			this.isInitControls = true;
			this.checkBoxAveraging.Checked = this.AnalogInputConfiguration.Averaging.Value;
			if (!this.pageValidator.General.HasFormatError(this.analogInputConfiguration.CanChannel))
			{
				if (this.analogInputConfiguration.CanChannel.Value <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels)
				{
					this.comboBoxChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.analogInputConfiguration.CanChannel.Value);
				}
				else
				{
					this.comboBoxChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.analogInputConfiguration.CanChannel.Value) + Resources.VirtualChannelPostfix;
				}
			}
			if (!this.pageValidator.General.HasFormatError(this.analogInputConfiguration.AnalogInputsCANMappingMode))
			{
				this.EnableControlsForMappingMode(this.analogInputConfiguration.AnalogInputsCANMappingMode.Value);
				switch (this.analogInputConfiguration.AnalogInputsCANMappingMode.Value)
				{
				case AnalogInputsCANMappingMode.IndividualIDs:
					this.radioButtonMappingModeIndividual.Checked = true;
					break;
				case AnalogInputsCANMappingMode.ContinuousIndividualIDs:
					this.radioButtonMappingModeContinuous.Checked = true;
					break;
				default:
					this.radioButtonMappingModeFixed.Checked = true;
					break;
				}
			}
			this.comboBoxChannel.Enabled = false;
			this.checkBoxAveraging.Enabled = false;
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.analogInputConfiguration.AnalogInputs.Count))
			{
				AnalogInput analogInput = this.analogInputConfiguration.GetAnalogInput(num);
				if (!this.pageValidator.General.HasFormatError(analogInput.IsActive))
				{
					this.inputNr2CheckBoxInput[num].Checked = analogInput.IsActive.Value;
				}
				this.EnableControlsInRow(this.inputNr2CheckBoxInput[num]);
				if (!this.pageValidator.General.HasFormatError(analogInput.Frequency))
				{
					this.inputNr2ComboBoxFrequency[num].SelectedItem = GUIUtil.MapAnalogInputsFrequency2String(analogInput.Frequency.Value);
				}
				if (!this.pageValidator.General.HasFormatError(analogInput.MappedCANId))
				{
					this.inputNr2TextBoxCANId[num].Text = GUIUtil.CANIdToDisplayString(analogInput.MappedCANId.Value, analogInput.IsMappedCANIdExtended.Value);
				}
				num += 1u;
			}
			this.isInitControls = false;
			this.ValidateInput();
		}

		private void EnableControlsForMappingMode(AnalogInputsCANMappingMode mappingMode)
		{
			switch (this.analogInputConfiguration.AnalogInputsCANMappingMode.Value)
			{
			case AnalogInputsCANMappingMode.SameFixedIDs:
			{
				uint num = 2u;
				while ((ulong)num <= (ulong)((long)this.analogInputConfiguration.AnalogInputs.Count))
				{
					this.inputNr2TextBoxCANId[num].Enabled = false;
					this.inputNr2ComboBoxFrequency[num].Enabled = false;
					num += 1u;
				}
				return;
			}
			case AnalogInputsCANMappingMode.IndividualIDs:
			{
				uint num2 = 2u;
				while ((ulong)num2 <= (ulong)((long)this.analogInputConfiguration.AnalogInputs.Count))
				{
					this.inputNr2TextBoxCANId[num2].Enabled = this.inputNr2CheckBoxInput[num2].Checked;
					this.inputNr2ComboBoxFrequency[num2].Enabled = this.inputNr2CheckBoxInput[num2].Checked;
					num2 += 1u;
				}
				return;
			}
			case AnalogInputsCANMappingMode.ContinuousIndividualIDs:
			{
				uint num3 = 2u;
				while ((ulong)num3 <= (ulong)((long)this.analogInputConfiguration.AnalogInputs.Count))
				{
					this.inputNr2TextBoxCANId[num3].Enabled = false;
					this.inputNr2ComboBoxFrequency[num3].Enabled = this.inputNr2CheckBoxInput[num3].Checked;
					num3 += 1u;
				}
				return;
			}
			default:
				return;
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.components != null)
				{
					this.components.Dispose();
				}
				this.customErrorProvider.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AnalogInputsGL1000));
			this.groupBoxAnalogInputs = new GroupBox();
			this.tableLayoutPanelAnalogInputs = new TableLayoutPanel();
			this.checkBoxAnalogInput1 = new CheckBox();
			this.checkBoxAnalogInput2 = new CheckBox();
			this.checkBoxAnalogInput3 = new CheckBox();
			this.checkBoxAnalogInput4 = new CheckBox();
			this.comboBoxAnalogInputFrequency1 = new ComboBox();
			this.comboBoxAnalogInputFrequency2 = new ComboBox();
			this.comboBoxAnalogInputFrequency3 = new ComboBox();
			this.comboBoxAnalogInputFrequency4 = new ComboBox();
			this.textBoxAnalogInputCANID1 = new TextBox();
			this.textBoxAnalogInputCANID2 = new TextBox();
			this.textBoxAnalogInputCANID3 = new TextBox();
			this.textBoxAnalogInputCANID4 = new TextBox();
			this.labelCANID1 = new Label();
			this.labelCANID2 = new Label();
			this.labelCANID3 = new Label();
			this.labelCANID4 = new Label();
			this.checkBoxAveraging = new CheckBox();
			this.groupBoxMappingMode = new GroupBox();
			this.radioButtonMappingModeContinuous = new RadioButton();
			this.radioButtonMappingModeIndividual = new RadioButton();
			this.radioButtonMappingModeFixed = new RadioButton();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.comboBoxChannel = new ComboBox();
			this.groupBoxSendOnChannel = new GroupBox();
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.toolTip = new ToolTip(this.components);
			this.groupBoxAnalogInputs.SuspendLayout();
			this.tableLayoutPanelAnalogInputs.SuspendLayout();
			this.groupBoxMappingMode.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			this.groupBoxSendOnChannel.SuspendLayout();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxAnalogInputs, "groupBoxAnalogInputs");
			this.groupBoxAnalogInputs.Controls.Add(this.tableLayoutPanelAnalogInputs);
			this.errorProviderFormat.SetError(this.groupBoxAnalogInputs, componentResourceManager.GetString("groupBoxAnalogInputs.Error"));
			this.errorProviderGlobalModel.SetError(this.groupBoxAnalogInputs, componentResourceManager.GetString("groupBoxAnalogInputs.Error1"));
			this.errorProviderLocalModel.SetError(this.groupBoxAnalogInputs, componentResourceManager.GetString("groupBoxAnalogInputs.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.groupBoxAnalogInputs, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxAnalogInputs.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.groupBoxAnalogInputs, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxAnalogInputs.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.groupBoxAnalogInputs, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxAnalogInputs.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.groupBoxAnalogInputs, (int)componentResourceManager.GetObject("groupBoxAnalogInputs.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.groupBoxAnalogInputs, (int)componentResourceManager.GetObject("groupBoxAnalogInputs.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.groupBoxAnalogInputs, (int)componentResourceManager.GetObject("groupBoxAnalogInputs.IconPadding2"));
			this.groupBoxAnalogInputs.Name = "groupBoxAnalogInputs";
			this.groupBoxAnalogInputs.TabStop = false;
			this.toolTip.SetToolTip(this.groupBoxAnalogInputs, componentResourceManager.GetString("groupBoxAnalogInputs.ToolTip"));
			componentResourceManager.ApplyResources(this.tableLayoutPanelAnalogInputs, "tableLayoutPanelAnalogInputs");
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput1, 0, 0);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput2, 0, 1);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput3, 0, 2);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput4, 0, 3);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency1, 1, 0);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency2, 1, 1);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency3, 1, 2);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency4, 1, 3);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID1, 3, 0);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID2, 3, 1);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID3, 3, 2);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID4, 3, 3);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID1, 2, 0);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID2, 2, 1);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID3, 2, 2);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID4, 2, 3);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAveraging, 1, 4);
			this.errorProviderLocalModel.SetError(this.tableLayoutPanelAnalogInputs, componentResourceManager.GetString("tableLayoutPanelAnalogInputs.Error"));
			this.errorProviderFormat.SetError(this.tableLayoutPanelAnalogInputs, componentResourceManager.GetString("tableLayoutPanelAnalogInputs.Error1"));
			this.errorProviderGlobalModel.SetError(this.tableLayoutPanelAnalogInputs, componentResourceManager.GetString("tableLayoutPanelAnalogInputs.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.tableLayoutPanelAnalogInputs, (ErrorIconAlignment)componentResourceManager.GetObject("tableLayoutPanelAnalogInputs.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.tableLayoutPanelAnalogInputs, (ErrorIconAlignment)componentResourceManager.GetObject("tableLayoutPanelAnalogInputs.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.tableLayoutPanelAnalogInputs, (ErrorIconAlignment)componentResourceManager.GetObject("tableLayoutPanelAnalogInputs.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.tableLayoutPanelAnalogInputs, (int)componentResourceManager.GetObject("tableLayoutPanelAnalogInputs.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.tableLayoutPanelAnalogInputs, (int)componentResourceManager.GetObject("tableLayoutPanelAnalogInputs.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.tableLayoutPanelAnalogInputs, (int)componentResourceManager.GetObject("tableLayoutPanelAnalogInputs.IconPadding2"));
			this.tableLayoutPanelAnalogInputs.Name = "tableLayoutPanelAnalogInputs";
			this.toolTip.SetToolTip(this.tableLayoutPanelAnalogInputs, componentResourceManager.GetString("tableLayoutPanelAnalogInputs.ToolTip"));
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput1, "checkBoxAnalogInput1");
			this.checkBoxAnalogInput1.Checked = true;
			this.checkBoxAnalogInput1.CheckState = CheckState.Checked;
			this.errorProviderLocalModel.SetError(this.checkBoxAnalogInput1, componentResourceManager.GetString("checkBoxAnalogInput1.Error"));
			this.errorProviderFormat.SetError(this.checkBoxAnalogInput1, componentResourceManager.GetString("checkBoxAnalogInput1.Error1"));
			this.errorProviderGlobalModel.SetError(this.checkBoxAnalogInput1, componentResourceManager.GetString("checkBoxAnalogInput1.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxAnalogInput1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAnalogInput1.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxAnalogInput1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAnalogInput1.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxAnalogInput1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAnalogInput1.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxAnalogInput1, (int)componentResourceManager.GetObject("checkBoxAnalogInput1.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxAnalogInput1, (int)componentResourceManager.GetObject("checkBoxAnalogInput1.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxAnalogInput1, (int)componentResourceManager.GetObject("checkBoxAnalogInput1.IconPadding2"));
			this.checkBoxAnalogInput1.Name = "checkBoxAnalogInput1";
			this.toolTip.SetToolTip(this.checkBoxAnalogInput1, componentResourceManager.GetString("checkBoxAnalogInput1.ToolTip"));
			this.checkBoxAnalogInput1.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput1.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput2, "checkBoxAnalogInput2");
			this.checkBoxAnalogInput2.Checked = true;
			this.checkBoxAnalogInput2.CheckState = CheckState.Checked;
			this.errorProviderLocalModel.SetError(this.checkBoxAnalogInput2, componentResourceManager.GetString("checkBoxAnalogInput2.Error"));
			this.errorProviderFormat.SetError(this.checkBoxAnalogInput2, componentResourceManager.GetString("checkBoxAnalogInput2.Error1"));
			this.errorProviderGlobalModel.SetError(this.checkBoxAnalogInput2, componentResourceManager.GetString("checkBoxAnalogInput2.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxAnalogInput2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAnalogInput2.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxAnalogInput2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAnalogInput2.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxAnalogInput2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAnalogInput2.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxAnalogInput2, (int)componentResourceManager.GetObject("checkBoxAnalogInput2.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxAnalogInput2, (int)componentResourceManager.GetObject("checkBoxAnalogInput2.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxAnalogInput2, (int)componentResourceManager.GetObject("checkBoxAnalogInput2.IconPadding2"));
			this.checkBoxAnalogInput2.Name = "checkBoxAnalogInput2";
			this.toolTip.SetToolTip(this.checkBoxAnalogInput2, componentResourceManager.GetString("checkBoxAnalogInput2.ToolTip"));
			this.checkBoxAnalogInput2.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput2.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput3, "checkBoxAnalogInput3");
			this.checkBoxAnalogInput3.Checked = true;
			this.checkBoxAnalogInput3.CheckState = CheckState.Checked;
			this.errorProviderLocalModel.SetError(this.checkBoxAnalogInput3, componentResourceManager.GetString("checkBoxAnalogInput3.Error"));
			this.errorProviderFormat.SetError(this.checkBoxAnalogInput3, componentResourceManager.GetString("checkBoxAnalogInput3.Error1"));
			this.errorProviderGlobalModel.SetError(this.checkBoxAnalogInput3, componentResourceManager.GetString("checkBoxAnalogInput3.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxAnalogInput3, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAnalogInput3.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxAnalogInput3, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAnalogInput3.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxAnalogInput3, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAnalogInput3.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxAnalogInput3, (int)componentResourceManager.GetObject("checkBoxAnalogInput3.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxAnalogInput3, (int)componentResourceManager.GetObject("checkBoxAnalogInput3.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxAnalogInput3, (int)componentResourceManager.GetObject("checkBoxAnalogInput3.IconPadding2"));
			this.checkBoxAnalogInput3.Name = "checkBoxAnalogInput3";
			this.toolTip.SetToolTip(this.checkBoxAnalogInput3, componentResourceManager.GetString("checkBoxAnalogInput3.ToolTip"));
			this.checkBoxAnalogInput3.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput3.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput4, "checkBoxAnalogInput4");
			this.checkBoxAnalogInput4.Checked = true;
			this.checkBoxAnalogInput4.CheckState = CheckState.Checked;
			this.errorProviderLocalModel.SetError(this.checkBoxAnalogInput4, componentResourceManager.GetString("checkBoxAnalogInput4.Error"));
			this.errorProviderFormat.SetError(this.checkBoxAnalogInput4, componentResourceManager.GetString("checkBoxAnalogInput4.Error1"));
			this.errorProviderGlobalModel.SetError(this.checkBoxAnalogInput4, componentResourceManager.GetString("checkBoxAnalogInput4.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxAnalogInput4, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAnalogInput4.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxAnalogInput4, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAnalogInput4.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxAnalogInput4, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAnalogInput4.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxAnalogInput4, (int)componentResourceManager.GetObject("checkBoxAnalogInput4.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxAnalogInput4, (int)componentResourceManager.GetObject("checkBoxAnalogInput4.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxAnalogInput4, (int)componentResourceManager.GetObject("checkBoxAnalogInput4.IconPadding2"));
			this.checkBoxAnalogInput4.Name = "checkBoxAnalogInput4";
			this.toolTip.SetToolTip(this.checkBoxAnalogInput4, componentResourceManager.GetString("checkBoxAnalogInput4.ToolTip"));
			this.checkBoxAnalogInput4.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput4.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency1, "comboBoxAnalogInputFrequency1");
			this.comboBoxAnalogInputFrequency1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderFormat.SetError(this.comboBoxAnalogInputFrequency1, componentResourceManager.GetString("comboBoxAnalogInputFrequency1.Error"));
			this.errorProviderLocalModel.SetError(this.comboBoxAnalogInputFrequency1, componentResourceManager.GetString("comboBoxAnalogInputFrequency1.Error1"));
			this.errorProviderGlobalModel.SetError(this.comboBoxAnalogInputFrequency1, componentResourceManager.GetString("comboBoxAnalogInputFrequency1.Error2"));
			this.comboBoxAnalogInputFrequency1.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxAnalogInputFrequency1, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputFrequency1.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxAnalogInputFrequency1, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputFrequency1.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxAnalogInputFrequency1, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputFrequency1.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxAnalogInputFrequency1, (int)componentResourceManager.GetObject("comboBoxAnalogInputFrequency1.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.comboBoxAnalogInputFrequency1, (int)componentResourceManager.GetObject("comboBoxAnalogInputFrequency1.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxAnalogInputFrequency1, (int)componentResourceManager.GetObject("comboBoxAnalogInputFrequency1.IconPadding2"));
			this.comboBoxAnalogInputFrequency1.Name = "comboBoxAnalogInputFrequency1";
			this.toolTip.SetToolTip(this.comboBoxAnalogInputFrequency1, componentResourceManager.GetString("comboBoxAnalogInputFrequency1.ToolTip"));
			this.comboBoxAnalogInputFrequency1.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency2, "comboBoxAnalogInputFrequency2");
			this.comboBoxAnalogInputFrequency2.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderFormat.SetError(this.comboBoxAnalogInputFrequency2, componentResourceManager.GetString("comboBoxAnalogInputFrequency2.Error"));
			this.errorProviderLocalModel.SetError(this.comboBoxAnalogInputFrequency2, componentResourceManager.GetString("comboBoxAnalogInputFrequency2.Error1"));
			this.errorProviderGlobalModel.SetError(this.comboBoxAnalogInputFrequency2, componentResourceManager.GetString("comboBoxAnalogInputFrequency2.Error2"));
			this.comboBoxAnalogInputFrequency2.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxAnalogInputFrequency2, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputFrequency2.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxAnalogInputFrequency2, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputFrequency2.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxAnalogInputFrequency2, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputFrequency2.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxAnalogInputFrequency2, (int)componentResourceManager.GetObject("comboBoxAnalogInputFrequency2.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.comboBoxAnalogInputFrequency2, (int)componentResourceManager.GetObject("comboBoxAnalogInputFrequency2.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxAnalogInputFrequency2, (int)componentResourceManager.GetObject("comboBoxAnalogInputFrequency2.IconPadding2"));
			this.comboBoxAnalogInputFrequency2.Name = "comboBoxAnalogInputFrequency2";
			this.toolTip.SetToolTip(this.comboBoxAnalogInputFrequency2, componentResourceManager.GetString("comboBoxAnalogInputFrequency2.ToolTip"));
			this.comboBoxAnalogInputFrequency2.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency3, "comboBoxAnalogInputFrequency3");
			this.comboBoxAnalogInputFrequency3.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderFormat.SetError(this.comboBoxAnalogInputFrequency3, componentResourceManager.GetString("comboBoxAnalogInputFrequency3.Error"));
			this.errorProviderLocalModel.SetError(this.comboBoxAnalogInputFrequency3, componentResourceManager.GetString("comboBoxAnalogInputFrequency3.Error1"));
			this.errorProviderGlobalModel.SetError(this.comboBoxAnalogInputFrequency3, componentResourceManager.GetString("comboBoxAnalogInputFrequency3.Error2"));
			this.comboBoxAnalogInputFrequency3.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxAnalogInputFrequency3, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputFrequency3.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxAnalogInputFrequency3, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputFrequency3.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxAnalogInputFrequency3, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputFrequency3.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxAnalogInputFrequency3, (int)componentResourceManager.GetObject("comboBoxAnalogInputFrequency3.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.comboBoxAnalogInputFrequency3, (int)componentResourceManager.GetObject("comboBoxAnalogInputFrequency3.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxAnalogInputFrequency3, (int)componentResourceManager.GetObject("comboBoxAnalogInputFrequency3.IconPadding2"));
			this.comboBoxAnalogInputFrequency3.Name = "comboBoxAnalogInputFrequency3";
			this.toolTip.SetToolTip(this.comboBoxAnalogInputFrequency3, componentResourceManager.GetString("comboBoxAnalogInputFrequency3.ToolTip"));
			this.comboBoxAnalogInputFrequency3.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency4, "comboBoxAnalogInputFrequency4");
			this.comboBoxAnalogInputFrequency4.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderFormat.SetError(this.comboBoxAnalogInputFrequency4, componentResourceManager.GetString("comboBoxAnalogInputFrequency4.Error"));
			this.errorProviderLocalModel.SetError(this.comboBoxAnalogInputFrequency4, componentResourceManager.GetString("comboBoxAnalogInputFrequency4.Error1"));
			this.errorProviderGlobalModel.SetError(this.comboBoxAnalogInputFrequency4, componentResourceManager.GetString("comboBoxAnalogInputFrequency4.Error2"));
			this.comboBoxAnalogInputFrequency4.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxAnalogInputFrequency4, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputFrequency4.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxAnalogInputFrequency4, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputFrequency4.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxAnalogInputFrequency4, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputFrequency4.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxAnalogInputFrequency4, (int)componentResourceManager.GetObject("comboBoxAnalogInputFrequency4.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.comboBoxAnalogInputFrequency4, (int)componentResourceManager.GetObject("comboBoxAnalogInputFrequency4.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxAnalogInputFrequency4, (int)componentResourceManager.GetObject("comboBoxAnalogInputFrequency4.IconPadding2"));
			this.comboBoxAnalogInputFrequency4.Name = "comboBoxAnalogInputFrequency4";
			this.toolTip.SetToolTip(this.comboBoxAnalogInputFrequency4, componentResourceManager.GetString("comboBoxAnalogInputFrequency4.ToolTip"));
			this.comboBoxAnalogInputFrequency4.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID1, "textBoxAnalogInputCANID1");
			this.errorProviderLocalModel.SetError(this.textBoxAnalogInputCANID1, componentResourceManager.GetString("textBoxAnalogInputCANID1.Error"));
			this.errorProviderGlobalModel.SetError(this.textBoxAnalogInputCANID1, componentResourceManager.GetString("textBoxAnalogInputCANID1.Error1"));
			this.errorProviderFormat.SetError(this.textBoxAnalogInputCANID1, componentResourceManager.GetString("textBoxAnalogInputCANID1.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID1.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID1.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID1.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.textBoxAnalogInputCANID1, (int)componentResourceManager.GetObject("textBoxAnalogInputCANID1.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxAnalogInputCANID1, (int)componentResourceManager.GetObject("textBoxAnalogInputCANID1.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxAnalogInputCANID1, (int)componentResourceManager.GetObject("textBoxAnalogInputCANID1.IconPadding2"));
			this.textBoxAnalogInputCANID1.Name = "textBoxAnalogInputCANID1";
			this.toolTip.SetToolTip(this.textBoxAnalogInputCANID1, componentResourceManager.GetString("textBoxAnalogInputCANID1.ToolTip"));
			this.textBoxAnalogInputCANID1.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID2, "textBoxAnalogInputCANID2");
			this.errorProviderLocalModel.SetError(this.textBoxAnalogInputCANID2, componentResourceManager.GetString("textBoxAnalogInputCANID2.Error"));
			this.errorProviderGlobalModel.SetError(this.textBoxAnalogInputCANID2, componentResourceManager.GetString("textBoxAnalogInputCANID2.Error1"));
			this.errorProviderFormat.SetError(this.textBoxAnalogInputCANID2, componentResourceManager.GetString("textBoxAnalogInputCANID2.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID2.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID2.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID2.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.textBoxAnalogInputCANID2, (int)componentResourceManager.GetObject("textBoxAnalogInputCANID2.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxAnalogInputCANID2, (int)componentResourceManager.GetObject("textBoxAnalogInputCANID2.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxAnalogInputCANID2, (int)componentResourceManager.GetObject("textBoxAnalogInputCANID2.IconPadding2"));
			this.textBoxAnalogInputCANID2.Name = "textBoxAnalogInputCANID2";
			this.toolTip.SetToolTip(this.textBoxAnalogInputCANID2, componentResourceManager.GetString("textBoxAnalogInputCANID2.ToolTip"));
			this.textBoxAnalogInputCANID2.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID3, "textBoxAnalogInputCANID3");
			this.errorProviderLocalModel.SetError(this.textBoxAnalogInputCANID3, componentResourceManager.GetString("textBoxAnalogInputCANID3.Error"));
			this.errorProviderGlobalModel.SetError(this.textBoxAnalogInputCANID3, componentResourceManager.GetString("textBoxAnalogInputCANID3.Error1"));
			this.errorProviderFormat.SetError(this.textBoxAnalogInputCANID3, componentResourceManager.GetString("textBoxAnalogInputCANID3.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID3.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID3.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID3.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.textBoxAnalogInputCANID3, (int)componentResourceManager.GetObject("textBoxAnalogInputCANID3.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxAnalogInputCANID3, (int)componentResourceManager.GetObject("textBoxAnalogInputCANID3.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxAnalogInputCANID3, (int)componentResourceManager.GetObject("textBoxAnalogInputCANID3.IconPadding2"));
			this.textBoxAnalogInputCANID3.Name = "textBoxAnalogInputCANID3";
			this.toolTip.SetToolTip(this.textBoxAnalogInputCANID3, componentResourceManager.GetString("textBoxAnalogInputCANID3.ToolTip"));
			this.textBoxAnalogInputCANID3.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID4, "textBoxAnalogInputCANID4");
			this.errorProviderLocalModel.SetError(this.textBoxAnalogInputCANID4, componentResourceManager.GetString("textBoxAnalogInputCANID4.Error"));
			this.errorProviderGlobalModel.SetError(this.textBoxAnalogInputCANID4, componentResourceManager.GetString("textBoxAnalogInputCANID4.Error1"));
			this.errorProviderFormat.SetError(this.textBoxAnalogInputCANID4, componentResourceManager.GetString("textBoxAnalogInputCANID4.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID4.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID4.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID4.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.textBoxAnalogInputCANID4, (int)componentResourceManager.GetObject("textBoxAnalogInputCANID4.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxAnalogInputCANID4, (int)componentResourceManager.GetObject("textBoxAnalogInputCANID4.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxAnalogInputCANID4, (int)componentResourceManager.GetObject("textBoxAnalogInputCANID4.IconPadding2"));
			this.textBoxAnalogInputCANID4.Name = "textBoxAnalogInputCANID4";
			this.toolTip.SetToolTip(this.textBoxAnalogInputCANID4, componentResourceManager.GetString("textBoxAnalogInputCANID4.ToolTip"));
			this.textBoxAnalogInputCANID4.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			componentResourceManager.ApplyResources(this.labelCANID1, "labelCANID1");
			this.errorProviderGlobalModel.SetError(this.labelCANID1, componentResourceManager.GetString("labelCANID1.Error"));
			this.errorProviderLocalModel.SetError(this.labelCANID1, componentResourceManager.GetString("labelCANID1.Error1"));
			this.errorProviderFormat.SetError(this.labelCANID1, componentResourceManager.GetString("labelCANID1.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANID1.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANID1.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANID1.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.labelCANID1, (int)componentResourceManager.GetObject("labelCANID1.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelCANID1, (int)componentResourceManager.GetObject("labelCANID1.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelCANID1, (int)componentResourceManager.GetObject("labelCANID1.IconPadding2"));
			this.labelCANID1.Name = "labelCANID1";
			this.toolTip.SetToolTip(this.labelCANID1, componentResourceManager.GetString("labelCANID1.ToolTip"));
			componentResourceManager.ApplyResources(this.labelCANID2, "labelCANID2");
			this.errorProviderGlobalModel.SetError(this.labelCANID2, componentResourceManager.GetString("labelCANID2.Error"));
			this.errorProviderLocalModel.SetError(this.labelCANID2, componentResourceManager.GetString("labelCANID2.Error1"));
			this.errorProviderFormat.SetError(this.labelCANID2, componentResourceManager.GetString("labelCANID2.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANID2.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANID2.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANID2.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.labelCANID2, (int)componentResourceManager.GetObject("labelCANID2.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelCANID2, (int)componentResourceManager.GetObject("labelCANID2.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelCANID2, (int)componentResourceManager.GetObject("labelCANID2.IconPadding2"));
			this.labelCANID2.Name = "labelCANID2";
			this.toolTip.SetToolTip(this.labelCANID2, componentResourceManager.GetString("labelCANID2.ToolTip"));
			componentResourceManager.ApplyResources(this.labelCANID3, "labelCANID3");
			this.errorProviderGlobalModel.SetError(this.labelCANID3, componentResourceManager.GetString("labelCANID3.Error"));
			this.errorProviderLocalModel.SetError(this.labelCANID3, componentResourceManager.GetString("labelCANID3.Error1"));
			this.errorProviderFormat.SetError(this.labelCANID3, componentResourceManager.GetString("labelCANID3.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelCANID3, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANID3.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelCANID3, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANID3.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelCANID3, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANID3.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.labelCANID3, (int)componentResourceManager.GetObject("labelCANID3.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelCANID3, (int)componentResourceManager.GetObject("labelCANID3.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelCANID3, (int)componentResourceManager.GetObject("labelCANID3.IconPadding2"));
			this.labelCANID3.Name = "labelCANID3";
			this.toolTip.SetToolTip(this.labelCANID3, componentResourceManager.GetString("labelCANID3.ToolTip"));
			componentResourceManager.ApplyResources(this.labelCANID4, "labelCANID4");
			this.errorProviderGlobalModel.SetError(this.labelCANID4, componentResourceManager.GetString("labelCANID4.Error"));
			this.errorProviderLocalModel.SetError(this.labelCANID4, componentResourceManager.GetString("labelCANID4.Error1"));
			this.errorProviderFormat.SetError(this.labelCANID4, componentResourceManager.GetString("labelCANID4.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANID4.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANID4.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANID4.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.labelCANID4, (int)componentResourceManager.GetObject("labelCANID4.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelCANID4, (int)componentResourceManager.GetObject("labelCANID4.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelCANID4, (int)componentResourceManager.GetObject("labelCANID4.IconPadding2"));
			this.labelCANID4.Name = "labelCANID4";
			this.toolTip.SetToolTip(this.labelCANID4, componentResourceManager.GetString("labelCANID4.ToolTip"));
			componentResourceManager.ApplyResources(this.checkBoxAveraging, "checkBoxAveraging");
			this.checkBoxAveraging.Checked = true;
			this.checkBoxAveraging.CheckState = CheckState.Checked;
			this.errorProviderLocalModel.SetError(this.checkBoxAveraging, componentResourceManager.GetString("checkBoxAveraging.Error"));
			this.errorProviderFormat.SetError(this.checkBoxAveraging, componentResourceManager.GetString("checkBoxAveraging.Error1"));
			this.errorProviderGlobalModel.SetError(this.checkBoxAveraging, componentResourceManager.GetString("checkBoxAveraging.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxAveraging, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAveraging.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxAveraging, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAveraging.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxAveraging, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAveraging.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxAveraging, (int)componentResourceManager.GetObject("checkBoxAveraging.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxAveraging, (int)componentResourceManager.GetObject("checkBoxAveraging.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxAveraging, (int)componentResourceManager.GetObject("checkBoxAveraging.IconPadding2"));
			this.checkBoxAveraging.Name = "checkBoxAveraging";
			this.toolTip.SetToolTip(this.checkBoxAveraging, componentResourceManager.GetString("checkBoxAveraging.ToolTip"));
			this.checkBoxAveraging.UseVisualStyleBackColor = true;
			this.checkBoxAveraging.CheckedChanged += new EventHandler(this.checkBoxAveraging_CheckedChanged);
			componentResourceManager.ApplyResources(this.groupBoxMappingMode, "groupBoxMappingMode");
			this.groupBoxMappingMode.Controls.Add(this.radioButtonMappingModeContinuous);
			this.groupBoxMappingMode.Controls.Add(this.radioButtonMappingModeIndividual);
			this.groupBoxMappingMode.Controls.Add(this.radioButtonMappingModeFixed);
			this.errorProviderFormat.SetError(this.groupBoxMappingMode, componentResourceManager.GetString("groupBoxMappingMode.Error"));
			this.errorProviderGlobalModel.SetError(this.groupBoxMappingMode, componentResourceManager.GetString("groupBoxMappingMode.Error1"));
			this.errorProviderLocalModel.SetError(this.groupBoxMappingMode, componentResourceManager.GetString("groupBoxMappingMode.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.groupBoxMappingMode, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxMappingMode.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.groupBoxMappingMode, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxMappingMode.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.groupBoxMappingMode, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxMappingMode.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.groupBoxMappingMode, (int)componentResourceManager.GetObject("groupBoxMappingMode.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.groupBoxMappingMode, (int)componentResourceManager.GetObject("groupBoxMappingMode.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.groupBoxMappingMode, (int)componentResourceManager.GetObject("groupBoxMappingMode.IconPadding2"));
			this.groupBoxMappingMode.Name = "groupBoxMappingMode";
			this.groupBoxMappingMode.TabStop = false;
			this.toolTip.SetToolTip(this.groupBoxMappingMode, componentResourceManager.GetString("groupBoxMappingMode.ToolTip"));
			componentResourceManager.ApplyResources(this.radioButtonMappingModeContinuous, "radioButtonMappingModeContinuous");
			this.errorProviderGlobalModel.SetError(this.radioButtonMappingModeContinuous, componentResourceManager.GetString("radioButtonMappingModeContinuous.Error"));
			this.errorProviderLocalModel.SetError(this.radioButtonMappingModeContinuous, componentResourceManager.GetString("radioButtonMappingModeContinuous.Error1"));
			this.errorProviderFormat.SetError(this.radioButtonMappingModeContinuous, componentResourceManager.GetString("radioButtonMappingModeContinuous.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonMappingModeContinuous, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeContinuous.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonMappingModeContinuous, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeContinuous.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.radioButtonMappingModeContinuous, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeContinuous.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.radioButtonMappingModeContinuous, (int)componentResourceManager.GetObject("radioButtonMappingModeContinuous.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.radioButtonMappingModeContinuous, (int)componentResourceManager.GetObject("radioButtonMappingModeContinuous.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.radioButtonMappingModeContinuous, (int)componentResourceManager.GetObject("radioButtonMappingModeContinuous.IconPadding2"));
			this.radioButtonMappingModeContinuous.Name = "radioButtonMappingModeContinuous";
			this.toolTip.SetToolTip(this.radioButtonMappingModeContinuous, componentResourceManager.GetString("radioButtonMappingModeContinuous.ToolTip"));
			this.radioButtonMappingModeContinuous.UseVisualStyleBackColor = true;
			this.radioButtonMappingModeContinuous.CheckedChanged += new EventHandler(this.radioButtonMappingMode_CheckedChanged);
			this.radioButtonMappingModeContinuous.MouseEnter += new EventHandler(this.OnRadioButtonMappingMode_MouseEnter);
			componentResourceManager.ApplyResources(this.radioButtonMappingModeIndividual, "radioButtonMappingModeIndividual");
			this.errorProviderGlobalModel.SetError(this.radioButtonMappingModeIndividual, componentResourceManager.GetString("radioButtonMappingModeIndividual.Error"));
			this.errorProviderLocalModel.SetError(this.radioButtonMappingModeIndividual, componentResourceManager.GetString("radioButtonMappingModeIndividual.Error1"));
			this.errorProviderFormat.SetError(this.radioButtonMappingModeIndividual, componentResourceManager.GetString("radioButtonMappingModeIndividual.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonMappingModeIndividual, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeIndividual.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonMappingModeIndividual, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeIndividual.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.radioButtonMappingModeIndividual, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeIndividual.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.radioButtonMappingModeIndividual, (int)componentResourceManager.GetObject("radioButtonMappingModeIndividual.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.radioButtonMappingModeIndividual, (int)componentResourceManager.GetObject("radioButtonMappingModeIndividual.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.radioButtonMappingModeIndividual, (int)componentResourceManager.GetObject("radioButtonMappingModeIndividual.IconPadding2"));
			this.radioButtonMappingModeIndividual.Name = "radioButtonMappingModeIndividual";
			this.toolTip.SetToolTip(this.radioButtonMappingModeIndividual, componentResourceManager.GetString("radioButtonMappingModeIndividual.ToolTip"));
			this.radioButtonMappingModeIndividual.UseVisualStyleBackColor = true;
			this.radioButtonMappingModeIndividual.CheckedChanged += new EventHandler(this.radioButtonMappingMode_CheckedChanged);
			this.radioButtonMappingModeIndividual.MouseEnter += new EventHandler(this.OnRadioButtonMappingMode_MouseEnter);
			componentResourceManager.ApplyResources(this.radioButtonMappingModeFixed, "radioButtonMappingModeFixed");
			this.radioButtonMappingModeFixed.Checked = true;
			this.errorProviderGlobalModel.SetError(this.radioButtonMappingModeFixed, componentResourceManager.GetString("radioButtonMappingModeFixed.Error"));
			this.errorProviderLocalModel.SetError(this.radioButtonMappingModeFixed, componentResourceManager.GetString("radioButtonMappingModeFixed.Error1"));
			this.errorProviderFormat.SetError(this.radioButtonMappingModeFixed, componentResourceManager.GetString("radioButtonMappingModeFixed.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonMappingModeFixed, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeFixed.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonMappingModeFixed, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeFixed.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.radioButtonMappingModeFixed, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeFixed.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.radioButtonMappingModeFixed, (int)componentResourceManager.GetObject("radioButtonMappingModeFixed.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.radioButtonMappingModeFixed, (int)componentResourceManager.GetObject("radioButtonMappingModeFixed.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.radioButtonMappingModeFixed, (int)componentResourceManager.GetObject("radioButtonMappingModeFixed.IconPadding2"));
			this.radioButtonMappingModeFixed.Name = "radioButtonMappingModeFixed";
			this.radioButtonMappingModeFixed.TabStop = true;
			this.toolTip.SetToolTip(this.radioButtonMappingModeFixed, componentResourceManager.GetString("radioButtonMappingModeFixed.ToolTip"));
			this.radioButtonMappingModeFixed.UseVisualStyleBackColor = true;
			this.radioButtonMappingModeFixed.CheckedChanged += new EventHandler(this.radioButtonMappingMode_CheckedChanged);
			this.radioButtonMappingModeFixed.MouseEnter += new EventHandler(this.OnRadioButtonMappingMode_MouseEnter);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderFormat, "errorProviderFormat");
			componentResourceManager.ApplyResources(this.comboBoxChannel, "comboBoxChannel");
			this.comboBoxChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderGlobalModel.SetError(this.comboBoxChannel, componentResourceManager.GetString("comboBoxChannel.Error"));
			this.errorProviderFormat.SetError(this.comboBoxChannel, componentResourceManager.GetString("comboBoxChannel.Error1"));
			this.errorProviderLocalModel.SetError(this.comboBoxChannel, componentResourceManager.GetString("comboBoxChannel.Error2"));
			this.comboBoxChannel.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxChannel, (int)componentResourceManager.GetObject("comboBoxChannel.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.comboBoxChannel, (int)componentResourceManager.GetObject("comboBoxChannel.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxChannel, (int)componentResourceManager.GetObject("comboBoxChannel.IconPadding2"));
			this.comboBoxChannel.Name = "comboBoxChannel";
			this.toolTip.SetToolTip(this.comboBoxChannel, componentResourceManager.GetString("comboBoxChannel.ToolTip"));
			this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.groupBoxSendOnChannel, "groupBoxSendOnChannel");
			this.groupBoxSendOnChannel.Controls.Add(this.comboBoxChannel);
			this.errorProviderFormat.SetError(this.groupBoxSendOnChannel, componentResourceManager.GetString("groupBoxSendOnChannel.Error"));
			this.errorProviderGlobalModel.SetError(this.groupBoxSendOnChannel, componentResourceManager.GetString("groupBoxSendOnChannel.Error1"));
			this.errorProviderLocalModel.SetError(this.groupBoxSendOnChannel, componentResourceManager.GetString("groupBoxSendOnChannel.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.groupBoxSendOnChannel, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxSendOnChannel.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.groupBoxSendOnChannel, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxSendOnChannel.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.groupBoxSendOnChannel, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxSendOnChannel.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this.groupBoxSendOnChannel, (int)componentResourceManager.GetObject("groupBoxSendOnChannel.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.groupBoxSendOnChannel, (int)componentResourceManager.GetObject("groupBoxSendOnChannel.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.groupBoxSendOnChannel, (int)componentResourceManager.GetObject("groupBoxSendOnChannel.IconPadding2"));
			this.groupBoxSendOnChannel.Name = "groupBoxSendOnChannel";
			this.groupBoxSendOnChannel.TabStop = false;
			this.toolTip.SetToolTip(this.groupBoxSendOnChannel, componentResourceManager.GetString("groupBoxSendOnChannel.ToolTip"));
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderLocalModel, "errorProviderLocalModel");
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			this.AutoValidate = AutoValidate.EnableAllowFocusChange;
			base.Controls.Add(this.groupBoxSendOnChannel);
			base.Controls.Add(this.groupBoxMappingMode);
			base.Controls.Add(this.groupBoxAnalogInputs);
			this.errorProviderFormat.SetError(this, componentResourceManager.GetString("$this.Error"));
			this.errorProviderLocalModel.SetError(this, componentResourceManager.GetString("$this.Error1"));
			this.errorProviderGlobalModel.SetError(this, componentResourceManager.GetString("$this.Error2"));
			this.errorProviderFormat.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding2"));
			base.Name = "AnalogInputsGL1000";
			this.toolTip.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			this.groupBoxAnalogInputs.ResumeLayout(false);
			this.tableLayoutPanelAnalogInputs.ResumeLayout(false);
			this.tableLayoutPanelAnalogInputs.PerformLayout();
			this.groupBoxMappingMode.ResumeLayout(false);
			this.groupBoxMappingMode.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			this.groupBoxSendOnChannel.ResumeLayout(false);
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
