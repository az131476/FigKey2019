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
	internal class AnalogInputsGL3Plus : UserControl
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

		private GroupBox groupBoxMapping;

		private RadioButton radioButtonMappingModeFixed;

		private RadioButton radioButtonMappingModeContinuous;

		private RadioButton radioButtonMappingModeIndividual;

		private Label labelCANID1;

		private Label labelCANID2;

		private Label labelCANID3;

		private Label labelCANID4;

		private ErrorProvider errorProviderFormat;

		private CheckBox checkBoxAnalogInput5;

		private CheckBox checkBoxAnalogInput6;

		private CheckBox checkBoxAnalogInput7;

		private CheckBox checkBoxAnalogInput8;

		private CheckBox checkBoxAnalogInput9;

		private CheckBox checkBoxAnalogInput10;

		private CheckBox checkBoxAnalogInput11;

		private CheckBox checkBoxAnalogInput12;

		private CheckBox checkBoxAnalogInput13;

		private CheckBox checkBoxAnalogInput14;

		private ComboBox comboBoxAnalogInputFrequency5;

		private ComboBox comboBoxAnalogInputFrequency6;

		private ComboBox comboBoxAnalogInputFrequency7;

		private ComboBox comboBoxAnalogInputFrequency8;

		private ComboBox comboBoxAnalogInputFrequency9;

		private ComboBox comboBoxAnalogInputFrequency10;

		private ComboBox comboBoxAnalogInputFrequency11;

		private ComboBox comboBoxAnalogInputFrequency12;

		private ComboBox comboBoxAnalogInputFrequency13;

		private ComboBox comboBoxAnalogInputFrequency14;

		private Label labelCANID5;

		private Label labelCANID6;

		private Label labelCANID7;

		private Label labelCANID8;

		private Label labelCANID9;

		private Label labelCANID10;

		private Label labelCANID11;

		private Label labelCANID12;

		private Label labelCANID13;

		private Label labelCANID14;

		private TextBox textBoxAnalogInputCANID5;

		private TextBox textBoxAnalogInputCANID6;

		private TextBox textBoxAnalogInputCANID7;

		private TextBox textBoxAnalogInputCANID8;

		private TextBox textBoxAnalogInputCANID9;

		private TextBox textBoxAnalogInputCANID10;

		private TextBox textBoxAnalogInputCANID11;

		private TextBox textBoxAnalogInputCANID12;

		private TextBox textBoxAnalogInputCANID13;

		private TextBox textBoxAnalogInputCANID14;

		private Label labelAnalogExtensionBoard;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private ComboBox comboBoxChannel;

		private ToolTip toolTip;

		private CheckBox checkBoxMapToCANMessage;

		private CheckBox checkBoxMapToSystemChannel;

		private Label labelCANChannel;

		private Label labelCANMode;

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

		public AnalogInputsGL3Plus()
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
			this.inputNr2CheckBoxInput.Add(5u, this.checkBoxAnalogInput5);
			this.inputNr2CheckBoxInput.Add(6u, this.checkBoxAnalogInput6);
			this.inputNr2CheckBoxInput.Add(7u, this.checkBoxAnalogInput7);
			this.inputNr2CheckBoxInput.Add(8u, this.checkBoxAnalogInput8);
			this.inputNr2CheckBoxInput.Add(9u, this.checkBoxAnalogInput9);
			this.inputNr2CheckBoxInput.Add(10u, this.checkBoxAnalogInput10);
			this.inputNr2CheckBoxInput.Add(11u, this.checkBoxAnalogInput11);
			this.inputNr2CheckBoxInput.Add(12u, this.checkBoxAnalogInput12);
			this.inputNr2CheckBoxInput.Add(13u, this.checkBoxAnalogInput13);
			this.inputNr2CheckBoxInput.Add(14u, this.checkBoxAnalogInput14);
			this.inputNr2ComboBoxFrequency = new Dictionary<uint, ComboBox>();
			this.inputNr2ComboBoxFrequency.Add(1u, this.comboBoxAnalogInputFrequency1);
			this.inputNr2ComboBoxFrequency.Add(2u, this.comboBoxAnalogInputFrequency2);
			this.inputNr2ComboBoxFrequency.Add(3u, this.comboBoxAnalogInputFrequency3);
			this.inputNr2ComboBoxFrequency.Add(4u, this.comboBoxAnalogInputFrequency4);
			this.inputNr2ComboBoxFrequency.Add(5u, this.comboBoxAnalogInputFrequency5);
			this.inputNr2ComboBoxFrequency.Add(6u, this.comboBoxAnalogInputFrequency6);
			this.inputNr2ComboBoxFrequency.Add(7u, this.comboBoxAnalogInputFrequency7);
			this.inputNr2ComboBoxFrequency.Add(8u, this.comboBoxAnalogInputFrequency8);
			this.inputNr2ComboBoxFrequency.Add(9u, this.comboBoxAnalogInputFrequency9);
			this.inputNr2ComboBoxFrequency.Add(10u, this.comboBoxAnalogInputFrequency10);
			this.inputNr2ComboBoxFrequency.Add(11u, this.comboBoxAnalogInputFrequency11);
			this.inputNr2ComboBoxFrequency.Add(12u, this.comboBoxAnalogInputFrequency12);
			this.inputNr2ComboBoxFrequency.Add(13u, this.comboBoxAnalogInputFrequency13);
			this.inputNr2ComboBoxFrequency.Add(14u, this.comboBoxAnalogInputFrequency14);
			this.inputNr2TextBoxCANId = new Dictionary<uint, TextBox>();
			this.inputNr2TextBoxCANId.Add(1u, this.textBoxAnalogInputCANID1);
			this.inputNr2TextBoxCANId.Add(2u, this.textBoxAnalogInputCANID2);
			this.inputNr2TextBoxCANId.Add(3u, this.textBoxAnalogInputCANID3);
			this.inputNr2TextBoxCANId.Add(4u, this.textBoxAnalogInputCANID4);
			this.inputNr2TextBoxCANId.Add(5u, this.textBoxAnalogInputCANID5);
			this.inputNr2TextBoxCANId.Add(6u, this.textBoxAnalogInputCANID6);
			this.inputNr2TextBoxCANId.Add(7u, this.textBoxAnalogInputCANID7);
			this.inputNr2TextBoxCANId.Add(8u, this.textBoxAnalogInputCANID8);
			this.inputNr2TextBoxCANId.Add(9u, this.textBoxAnalogInputCANID9);
			this.inputNr2TextBoxCANId.Add(10u, this.textBoxAnalogInputCANID10);
			this.inputNr2TextBoxCANId.Add(11u, this.textBoxAnalogInputCANID11);
			this.inputNr2TextBoxCANId.Add(12u, this.textBoxAnalogInputCANID12);
			this.inputNr2TextBoxCANId.Add(13u, this.textBoxAnalogInputCANID13);
			this.inputNr2TextBoxCANId.Add(14u, this.textBoxAnalogInputCANID14);
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
				this.toolTip.SetToolTip(this.radioButtonMappingModeFixed, Resources.AnalogInputsMappingModeFixed);
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

		private void checkBoxMapToSystemChannel_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxMapToCANMessage_CheckedChanged(object sender, EventArgs e)
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
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxMapToSystemChannel.Checked, this.AnalogInputConfiguration.MapToSystemChannel, this.guiElementManager.GetGUIElement(this.checkBoxMapToSystemChannel), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxMapToCANMessage.Checked, this.AnalogInputConfiguration.MapToCANMessage, this.guiElementManager.GetGUIElement(this.checkBoxMapToCANMessage), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxAveraging.Checked, this.AnalogInputConfiguration.Averaging, this.guiElementManager.GetGUIElement(this.checkBoxAveraging), out flag3);
			flag2 |= flag3;
			if (this.AnalogInputConfiguration.MapToCANMessage.Value)
			{
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
						uint masterInputNrForFrequencyInFixedMode = GUIUtil.GetMasterInputNrForFrequencyInFixedMode(num);
						uint value;
						if (mappingMode == AnalogInputsCANMappingMode.SameFixedIDs && num != masterInputNrForFrequencyInFixedMode)
						{
							value = this.analogInputConfiguration.GetAnalogInput(masterInputNrForFrequencyInFixedMode).Frequency.Value;
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
			}
			else
			{
				uint num2 = 1u;
				while ((ulong)num2 <= (ulong)((long)this.inputNr2TextBoxCANId.Count))
				{
					flag &= this.pageValidator.Control.UpdateModel<bool>(this.inputNr2CheckBoxInput[num2].Checked, this.AnalogInputConfiguration.GetAnalogInput(num2).IsActive, this.guiElementManager.GetGUIElement(this.inputNr2CheckBoxInput[num2]), out flag3);
					flag2 |= flag3;
					if (this.AnalogInputConfiguration.GetAnalogInput(num2).IsActive.Value)
					{
						uint selectedFrequency = this.GetSelectedFrequency(this.inputNr2ComboBoxFrequency[num2]);
						flag &= this.pageValidator.Control.UpdateModel<uint>(selectedFrequency, this.analogInputConfiguration.GetAnalogInput(num2).Frequency, this.guiElementManager.GetGUIElement(this.inputNr2ComboBoxFrequency[num2]), out flag3);
						flag2 |= flag3;
					}
					num2 += 1u;
				}
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
				this.comboBoxChannel.Enabled = this.checkBoxMapToCANMessage.Checked;
				this.checkBoxAveraging.Enabled = (this.checkBoxMapToCANMessage.Checked || this.checkBoxMapToSystemChannel.Checked);
			}
			if (!this.checkBoxMapToCANMessage.Checked)
			{
				this.inputNr2ComboBoxFrequency[key].Enabled = (this.inputNr2CheckBoxInput[key].Checked && this.inputNr2CheckBoxInput[key].Enabled);
				return;
			}
			if (this.analogInputConfiguration.AnalogInputsCANMappingMode.Value == AnalogInputsCANMappingMode.SameFixedIDs)
			{
				if (key == 1u)
				{
					this.inputNr2TextBoxCANId[key].Enabled = true;
					this.inputNr2ComboBoxFrequency[key].Enabled = true;
					return;
				}
				this.inputNr2TextBoxCANId[key].Enabled = false;
				if (((key - 1u) % Constants.NumberOfAnalogInputsOnOneMessage > 0u && key <= 6u) || ((key - 7u) % Constants.NumberOfAnalogInputsOnOneMessage > 0u && key > 6u))
				{
					this.inputNr2ComboBoxFrequency[key].Enabled = false;
					return;
				}
				this.inputNr2ComboBoxFrequency[key].Enabled = true;
				return;
			}
			else
			{
				if (this.analogInputConfiguration.AnalogInputsCANMappingMode.Value != AnalogInputsCANMappingMode.ContinuousIndividualIDs)
				{
					this.inputNr2TextBoxCANId[key].Enabled = this.inputNr2CheckBoxInput[key].Checked;
					this.inputNr2ComboBoxFrequency[key].Enabled = this.inputNr2CheckBoxInput[key].Checked;
					return;
				}
				if (key == 1u)
				{
					this.inputNr2TextBoxCANId[key].Enabled = true;
					this.inputNr2ComboBoxFrequency[key].Enabled = this.inputNr2CheckBoxInput[key].Checked;
					return;
				}
				this.inputNr2TextBoxCANId[key].Enabled = false;
				this.inputNr2ComboBoxFrequency[key].Enabled = this.inputNr2CheckBoxInput[key].Checked;
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
			this.checkBoxMapToSystemChannel.Checked = this.AnalogInputConfiguration.MapToSystemChannel.Value;
			this.checkBoxMapToCANMessage.Checked = this.AnalogInputConfiguration.MapToCANMessage.Value;
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
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.analogInputConfiguration.AnalogInputs.Count))
			{
				this.inputNr2TextBoxCANId[num].Enabled = false;
				this.inputNr2ComboBoxFrequency[num].Enabled = false;
				num += 1u;
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
			this.radioButtonMappingModeFixed.Enabled = this.checkBoxMapToCANMessage.Checked;
			this.radioButtonMappingModeContinuous.Enabled = this.checkBoxMapToCANMessage.Checked;
			this.radioButtonMappingModeIndividual.Enabled = this.checkBoxMapToCANMessage.Checked;
			this.checkBoxAveraging.Enabled = false;
			uint num2 = 1u;
			while ((ulong)num2 <= (ulong)((long)this.analogInputConfiguration.AnalogInputs.Count))
			{
				this.inputNr2CheckBoxInput[num2].Enabled = (this.checkBoxMapToCANMessage.Checked || this.checkBoxMapToSystemChannel.Checked);
				AnalogInput analogInput = this.analogInputConfiguration.GetAnalogInput(num2);
				if (!this.pageValidator.General.HasFormatError(analogInput.IsActive))
				{
					this.inputNr2CheckBoxInput[num2].Checked = analogInput.IsActive.Value;
				}
				this.EnableControlsInRow(this.inputNr2CheckBoxInput[num2]);
				if (!this.pageValidator.General.HasFormatError(analogInput.Frequency))
				{
					this.inputNr2ComboBoxFrequency[num2].SelectedItem = GUIUtil.MapAnalogInputsFrequency2String(analogInput.Frequency.Value);
				}
				if (!this.pageValidator.General.HasFormatError(analogInput.MappedCANId))
				{
					this.inputNr2TextBoxCANId[num2].Text = GUIUtil.CANIdToDisplayString(analogInput.MappedCANId.Value, analogInput.IsMappedCANIdExtended.Value);
				}
				num2 += 1u;
			}
			this.isInitControls = false;
			this.ValidateInput();
		}

		private void EnableControlsForMappingMode(AnalogInputsCANMappingMode mappingMode)
		{
			if (this.checkBoxMapToCANMessage.Checked)
			{
				switch (this.analogInputConfiguration.AnalogInputsCANMappingMode.Value)
				{
				case AnalogInputsCANMappingMode.SameFixedIDs:
				{
					uint num = 2u;
					while ((ulong)num <= (ulong)((long)this.analogInputConfiguration.AnalogInputs.Count))
					{
						if (((num - 1u) % Constants.NumberOfAnalogInputsOnOneMessage == 0u && num <= 6u) || ((num - 7u) % Constants.NumberOfAnalogInputsOnOneMessage == 0u && num > 6u))
						{
							this.inputNr2ComboBoxFrequency[num].Enabled = true;
						}
						else
						{
							this.inputNr2ComboBoxFrequency[num].Enabled = false;
						}
						this.inputNr2TextBoxCANId[num].Enabled = false;
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
					break;
				}
				default:
					return;
				}
			}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AnalogInputsGL3Plus));
			this.groupBoxAnalogInputs = new GroupBox();
			this.tableLayoutPanelAnalogInputs = new TableLayoutPanel();
			this.checkBoxAnalogInput1 = new CheckBox();
			this.checkBoxAnalogInput2 = new CheckBox();
			this.labelAnalogExtensionBoard = new Label();
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
			this.checkBoxAnalogInput5 = new CheckBox();
			this.checkBoxAnalogInput6 = new CheckBox();
			this.checkBoxAnalogInput7 = new CheckBox();
			this.checkBoxAnalogInput8 = new CheckBox();
			this.checkBoxAnalogInput9 = new CheckBox();
			this.checkBoxAnalogInput10 = new CheckBox();
			this.checkBoxAnalogInput11 = new CheckBox();
			this.checkBoxAnalogInput12 = new CheckBox();
			this.checkBoxAnalogInput13 = new CheckBox();
			this.checkBoxAnalogInput14 = new CheckBox();
			this.comboBoxAnalogInputFrequency5 = new ComboBox();
			this.comboBoxAnalogInputFrequency6 = new ComboBox();
			this.comboBoxAnalogInputFrequency7 = new ComboBox();
			this.comboBoxAnalogInputFrequency8 = new ComboBox();
			this.comboBoxAnalogInputFrequency9 = new ComboBox();
			this.comboBoxAnalogInputFrequency10 = new ComboBox();
			this.comboBoxAnalogInputFrequency11 = new ComboBox();
			this.comboBoxAnalogInputFrequency12 = new ComboBox();
			this.comboBoxAnalogInputFrequency13 = new ComboBox();
			this.comboBoxAnalogInputFrequency14 = new ComboBox();
			this.labelCANID5 = new Label();
			this.labelCANID6 = new Label();
			this.labelCANID7 = new Label();
			this.labelCANID8 = new Label();
			this.labelCANID9 = new Label();
			this.labelCANID10 = new Label();
			this.labelCANID11 = new Label();
			this.labelCANID12 = new Label();
			this.labelCANID13 = new Label();
			this.labelCANID14 = new Label();
			this.textBoxAnalogInputCANID5 = new TextBox();
			this.textBoxAnalogInputCANID6 = new TextBox();
			this.textBoxAnalogInputCANID7 = new TextBox();
			this.textBoxAnalogInputCANID8 = new TextBox();
			this.textBoxAnalogInputCANID9 = new TextBox();
			this.textBoxAnalogInputCANID10 = new TextBox();
			this.textBoxAnalogInputCANID11 = new TextBox();
			this.textBoxAnalogInputCANID12 = new TextBox();
			this.textBoxAnalogInputCANID13 = new TextBox();
			this.textBoxAnalogInputCANID14 = new TextBox();
			this.checkBoxAveraging = new CheckBox();
			this.groupBoxMapping = new GroupBox();
			this.comboBoxChannel = new ComboBox();
			this.labelCANMode = new Label();
			this.labelCANChannel = new Label();
			this.checkBoxMapToCANMessage = new CheckBox();
			this.checkBoxMapToSystemChannel = new CheckBox();
			this.radioButtonMappingModeContinuous = new RadioButton();
			this.radioButtonMappingModeIndividual = new RadioButton();
			this.radioButtonMappingModeFixed = new RadioButton();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.toolTip = new ToolTip(this.components);
			this.groupBoxAnalogInputs.SuspendLayout();
			this.tableLayoutPanelAnalogInputs.SuspendLayout();
			this.groupBoxMapping.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.groupBoxAnalogInputs.Controls.Add(this.tableLayoutPanelAnalogInputs);
			componentResourceManager.ApplyResources(this.groupBoxAnalogInputs, "groupBoxAnalogInputs");
			this.groupBoxAnalogInputs.Name = "groupBoxAnalogInputs";
			this.groupBoxAnalogInputs.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanelAnalogInputs, "tableLayoutPanelAnalogInputs");
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput1, 0, 0);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput2, 0, 1);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelAnalogExtensionBoard, 0, 6);
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
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput5, 0, 4);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput6, 0, 5);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput7, 0, 7);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput8, 0, 8);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput9, 0, 9);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput10, 0, 10);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput11, 0, 11);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput12, 0, 12);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput13, 0, 13);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput14, 0, 14);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency5, 1, 4);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency6, 1, 5);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency7, 1, 7);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency8, 1, 8);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency9, 1, 9);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency10, 1, 10);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency11, 1, 11);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency12, 1, 12);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency13, 1, 13);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency14, 1, 14);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID5, 2, 4);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID6, 2, 5);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID7, 2, 7);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID8, 2, 8);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID9, 2, 9);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID10, 2, 10);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID11, 2, 11);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID12, 2, 12);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID13, 2, 13);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID14, 2, 14);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID5, 3, 4);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID6, 3, 5);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID7, 3, 7);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID8, 3, 8);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID9, 3, 9);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID10, 3, 10);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID11, 3, 11);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID12, 3, 12);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID13, 3, 13);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID14, 3, 14);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAveraging, 1, 15);
			this.tableLayoutPanelAnalogInputs.Name = "tableLayoutPanelAnalogInputs";
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput1, "checkBoxAnalogInput1");
			this.checkBoxAnalogInput1.Checked = true;
			this.checkBoxAnalogInput1.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput1.Name = "checkBoxAnalogInput1";
			this.checkBoxAnalogInput1.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput1.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput2, "checkBoxAnalogInput2");
			this.checkBoxAnalogInput2.Checked = true;
			this.checkBoxAnalogInput2.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput2.Name = "checkBoxAnalogInput2";
			this.checkBoxAnalogInput2.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput2.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelAnalogExtensionBoard, "labelAnalogExtensionBoard");
			this.tableLayoutPanelAnalogInputs.SetColumnSpan(this.labelAnalogExtensionBoard, 2);
			this.labelAnalogExtensionBoard.Name = "labelAnalogExtensionBoard";
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput3, "checkBoxAnalogInput3");
			this.checkBoxAnalogInput3.Checked = true;
			this.checkBoxAnalogInput3.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput3.Name = "checkBoxAnalogInput3";
			this.checkBoxAnalogInput3.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput3.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput4, "checkBoxAnalogInput4");
			this.checkBoxAnalogInput4.Checked = true;
			this.checkBoxAnalogInput4.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput4.Name = "checkBoxAnalogInput4";
			this.checkBoxAnalogInput4.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput4.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			this.comboBoxAnalogInputFrequency1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency1.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency1, "comboBoxAnalogInputFrequency1");
			this.comboBoxAnalogInputFrequency1.Name = "comboBoxAnalogInputFrequency1";
			this.comboBoxAnalogInputFrequency1.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			this.comboBoxAnalogInputFrequency2.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency2.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency2, "comboBoxAnalogInputFrequency2");
			this.comboBoxAnalogInputFrequency2.Name = "comboBoxAnalogInputFrequency2";
			this.comboBoxAnalogInputFrequency2.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			this.comboBoxAnalogInputFrequency3.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency3.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency3, "comboBoxAnalogInputFrequency3");
			this.comboBoxAnalogInputFrequency3.Name = "comboBoxAnalogInputFrequency3";
			this.comboBoxAnalogInputFrequency3.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			this.comboBoxAnalogInputFrequency4.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency4.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency4, "comboBoxAnalogInputFrequency4");
			this.comboBoxAnalogInputFrequency4.Name = "comboBoxAnalogInputFrequency4";
			this.comboBoxAnalogInputFrequency4.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID1.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID1.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID1.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID1, "textBoxAnalogInputCANID1");
			this.textBoxAnalogInputCANID1.Name = "textBoxAnalogInputCANID1";
			this.textBoxAnalogInputCANID1.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID2.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID2.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID2.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID2, "textBoxAnalogInputCANID2");
			this.textBoxAnalogInputCANID2.Name = "textBoxAnalogInputCANID2";
			this.textBoxAnalogInputCANID2.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID3.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID3.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID3.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID3, "textBoxAnalogInputCANID3");
			this.textBoxAnalogInputCANID3.Name = "textBoxAnalogInputCANID3";
			this.textBoxAnalogInputCANID3.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID4.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID4.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID4.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID4, "textBoxAnalogInputCANID4");
			this.textBoxAnalogInputCANID4.Name = "textBoxAnalogInputCANID4";
			this.textBoxAnalogInputCANID4.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			componentResourceManager.ApplyResources(this.labelCANID1, "labelCANID1");
			this.labelCANID1.Name = "labelCANID1";
			componentResourceManager.ApplyResources(this.labelCANID2, "labelCANID2");
			this.labelCANID2.Name = "labelCANID2";
			componentResourceManager.ApplyResources(this.labelCANID3, "labelCANID3");
			this.labelCANID3.Name = "labelCANID3";
			componentResourceManager.ApplyResources(this.labelCANID4, "labelCANID4");
			this.labelCANID4.Name = "labelCANID4";
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput5, "checkBoxAnalogInput5");
			this.checkBoxAnalogInput5.Checked = true;
			this.checkBoxAnalogInput5.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput5.Name = "checkBoxAnalogInput5";
			this.checkBoxAnalogInput5.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput5.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput6, "checkBoxAnalogInput6");
			this.checkBoxAnalogInput6.Checked = true;
			this.checkBoxAnalogInput6.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput6.Name = "checkBoxAnalogInput6";
			this.checkBoxAnalogInput6.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput6.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput7, "checkBoxAnalogInput7");
			this.checkBoxAnalogInput7.Checked = true;
			this.checkBoxAnalogInput7.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput7.Name = "checkBoxAnalogInput7";
			this.checkBoxAnalogInput7.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput7.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput8, "checkBoxAnalogInput8");
			this.checkBoxAnalogInput8.Checked = true;
			this.checkBoxAnalogInput8.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput8.Name = "checkBoxAnalogInput8";
			this.checkBoxAnalogInput8.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput8.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput9, "checkBoxAnalogInput9");
			this.checkBoxAnalogInput9.Checked = true;
			this.checkBoxAnalogInput9.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput9.Name = "checkBoxAnalogInput9";
			this.checkBoxAnalogInput9.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput9.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput10, "checkBoxAnalogInput10");
			this.checkBoxAnalogInput10.Checked = true;
			this.checkBoxAnalogInput10.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput10.Name = "checkBoxAnalogInput10";
			this.checkBoxAnalogInput10.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput10.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput11, "checkBoxAnalogInput11");
			this.checkBoxAnalogInput11.Checked = true;
			this.checkBoxAnalogInput11.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput11.Name = "checkBoxAnalogInput11";
			this.checkBoxAnalogInput11.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput11.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput12, "checkBoxAnalogInput12");
			this.checkBoxAnalogInput12.Checked = true;
			this.checkBoxAnalogInput12.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput12.Name = "checkBoxAnalogInput12";
			this.checkBoxAnalogInput12.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput12.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput13, "checkBoxAnalogInput13");
			this.checkBoxAnalogInput13.Checked = true;
			this.checkBoxAnalogInput13.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput13.Name = "checkBoxAnalogInput13";
			this.checkBoxAnalogInput13.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput13.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput14, "checkBoxAnalogInput14");
			this.checkBoxAnalogInput14.Checked = true;
			this.checkBoxAnalogInput14.CheckState = CheckState.Checked;
			this.checkBoxAnalogInput14.Name = "checkBoxAnalogInput14";
			this.checkBoxAnalogInput14.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput14.CheckedChanged += new EventHandler(this.checkBoxAnalogInput_CheckedChanged);
			this.comboBoxAnalogInputFrequency5.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency5.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency5, "comboBoxAnalogInputFrequency5");
			this.comboBoxAnalogInputFrequency5.Name = "comboBoxAnalogInputFrequency5";
			this.comboBoxAnalogInputFrequency5.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			this.comboBoxAnalogInputFrequency6.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency6.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency6, "comboBoxAnalogInputFrequency6");
			this.comboBoxAnalogInputFrequency6.Name = "comboBoxAnalogInputFrequency6";
			this.comboBoxAnalogInputFrequency6.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			this.comboBoxAnalogInputFrequency7.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency7.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency7, "comboBoxAnalogInputFrequency7");
			this.comboBoxAnalogInputFrequency7.Name = "comboBoxAnalogInputFrequency7";
			this.comboBoxAnalogInputFrequency7.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			this.comboBoxAnalogInputFrequency8.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency8.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency8, "comboBoxAnalogInputFrequency8");
			this.comboBoxAnalogInputFrequency8.Name = "comboBoxAnalogInputFrequency8";
			this.comboBoxAnalogInputFrequency8.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			this.comboBoxAnalogInputFrequency9.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency9.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency9, "comboBoxAnalogInputFrequency9");
			this.comboBoxAnalogInputFrequency9.Name = "comboBoxAnalogInputFrequency9";
			this.comboBoxAnalogInputFrequency9.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			this.comboBoxAnalogInputFrequency10.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency10.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency10, "comboBoxAnalogInputFrequency10");
			this.comboBoxAnalogInputFrequency10.Name = "comboBoxAnalogInputFrequency10";
			this.comboBoxAnalogInputFrequency10.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			this.comboBoxAnalogInputFrequency11.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency11.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency11, "comboBoxAnalogInputFrequency11");
			this.comboBoxAnalogInputFrequency11.Name = "comboBoxAnalogInputFrequency11";
			this.comboBoxAnalogInputFrequency11.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			this.comboBoxAnalogInputFrequency12.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency12.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency12, "comboBoxAnalogInputFrequency12");
			this.comboBoxAnalogInputFrequency12.Name = "comboBoxAnalogInputFrequency12";
			this.comboBoxAnalogInputFrequency12.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			this.comboBoxAnalogInputFrequency13.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency13.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency13, "comboBoxAnalogInputFrequency13");
			this.comboBoxAnalogInputFrequency13.Name = "comboBoxAnalogInputFrequency13";
			this.comboBoxAnalogInputFrequency13.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			this.comboBoxAnalogInputFrequency14.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputFrequency14.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputFrequency14, "comboBoxAnalogInputFrequency14");
			this.comboBoxAnalogInputFrequency14.Name = "comboBoxAnalogInputFrequency14";
			this.comboBoxAnalogInputFrequency14.SelectedIndexChanged += new EventHandler(this.comboBoxAnalogInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelCANID5, "labelCANID5");
			this.labelCANID5.Name = "labelCANID5";
			componentResourceManager.ApplyResources(this.labelCANID6, "labelCANID6");
			this.labelCANID6.Name = "labelCANID6";
			componentResourceManager.ApplyResources(this.labelCANID7, "labelCANID7");
			this.labelCANID7.Name = "labelCANID7";
			componentResourceManager.ApplyResources(this.labelCANID8, "labelCANID8");
			this.labelCANID8.Name = "labelCANID8";
			componentResourceManager.ApplyResources(this.labelCANID9, "labelCANID9");
			this.labelCANID9.Name = "labelCANID9";
			componentResourceManager.ApplyResources(this.labelCANID10, "labelCANID10");
			this.labelCANID10.Name = "labelCANID10";
			componentResourceManager.ApplyResources(this.labelCANID11, "labelCANID11");
			this.labelCANID11.Name = "labelCANID11";
			componentResourceManager.ApplyResources(this.labelCANID12, "labelCANID12");
			this.labelCANID12.Name = "labelCANID12";
			componentResourceManager.ApplyResources(this.labelCANID13, "labelCANID13");
			this.labelCANID13.Name = "labelCANID13";
			componentResourceManager.ApplyResources(this.labelCANID14, "labelCANID14");
			this.labelCANID14.Name = "labelCANID14";
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID5, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID5.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID5, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID5.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID5, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID5.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID5, "textBoxAnalogInputCANID5");
			this.textBoxAnalogInputCANID5.Name = "textBoxAnalogInputCANID5";
			this.textBoxAnalogInputCANID5.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID6, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID6.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID6, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID6.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID6, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID6.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID6, "textBoxAnalogInputCANID6");
			this.textBoxAnalogInputCANID6.Name = "textBoxAnalogInputCANID6";
			this.textBoxAnalogInputCANID6.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID7, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID7.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID7, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID7.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID7, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID7.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID7, "textBoxAnalogInputCANID7");
			this.textBoxAnalogInputCANID7.Name = "textBoxAnalogInputCANID7";
			this.textBoxAnalogInputCANID7.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID8, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID8.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID8, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID8.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID8, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID8.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID8, "textBoxAnalogInputCANID8");
			this.textBoxAnalogInputCANID8.Name = "textBoxAnalogInputCANID8";
			this.textBoxAnalogInputCANID8.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID9, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID9.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID9, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID9.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID9, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID9.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID9, "textBoxAnalogInputCANID9");
			this.textBoxAnalogInputCANID9.Name = "textBoxAnalogInputCANID9";
			this.textBoxAnalogInputCANID9.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID10, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID10.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID10, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID10.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID10, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID10.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID10, "textBoxAnalogInputCANID10");
			this.textBoxAnalogInputCANID10.Name = "textBoxAnalogInputCANID10";
			this.textBoxAnalogInputCANID10.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID11, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID11.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID11, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID11.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID11, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID11.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID11, "textBoxAnalogInputCANID11");
			this.textBoxAnalogInputCANID11.Name = "textBoxAnalogInputCANID11";
			this.textBoxAnalogInputCANID11.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID12, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID12.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID12, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID12.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID12, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID12.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID12, "textBoxAnalogInputCANID12");
			this.textBoxAnalogInputCANID12.Name = "textBoxAnalogInputCANID12";
			this.textBoxAnalogInputCANID12.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID13, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID13.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID13, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID13.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID13, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID13.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID13, "textBoxAnalogInputCANID13");
			this.textBoxAnalogInputCANID13.Name = "textBoxAnalogInputCANID13";
			this.textBoxAnalogInputCANID13.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID14, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID14.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID14, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID14.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID14, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID14.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID14, "textBoxAnalogInputCANID14");
			this.textBoxAnalogInputCANID14.Name = "textBoxAnalogInputCANID14";
			this.textBoxAnalogInputCANID14.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			componentResourceManager.ApplyResources(this.checkBoxAveraging, "checkBoxAveraging");
			this.checkBoxAveraging.Checked = true;
			this.checkBoxAveraging.CheckState = CheckState.Checked;
			this.checkBoxAveraging.Name = "checkBoxAveraging";
			this.checkBoxAveraging.UseVisualStyleBackColor = true;
			this.checkBoxAveraging.CheckedChanged += new EventHandler(this.checkBoxAveraging_CheckedChanged);
			this.groupBoxMapping.Controls.Add(this.comboBoxChannel);
			this.groupBoxMapping.Controls.Add(this.labelCANMode);
			this.groupBoxMapping.Controls.Add(this.labelCANChannel);
			this.groupBoxMapping.Controls.Add(this.checkBoxMapToCANMessage);
			this.groupBoxMapping.Controls.Add(this.checkBoxMapToSystemChannel);
			this.groupBoxMapping.Controls.Add(this.radioButtonMappingModeContinuous);
			this.groupBoxMapping.Controls.Add(this.radioButtonMappingModeIndividual);
			this.groupBoxMapping.Controls.Add(this.radioButtonMappingModeFixed);
			componentResourceManager.ApplyResources(this.groupBoxMapping, "groupBoxMapping");
			this.groupBoxMapping.Name = "groupBoxMapping";
			this.groupBoxMapping.TabStop = false;
			this.comboBoxChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxChannel.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxChannel, "comboBoxChannel");
			this.comboBoxChannel.Name = "comboBoxChannel";
			this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelCANMode, "labelCANMode");
			this.labelCANMode.Name = "labelCANMode";
			componentResourceManager.ApplyResources(this.labelCANChannel, "labelCANChannel");
			this.labelCANChannel.Name = "labelCANChannel";
			componentResourceManager.ApplyResources(this.checkBoxMapToCANMessage, "checkBoxMapToCANMessage");
			this.checkBoxMapToCANMessage.Name = "checkBoxMapToCANMessage";
			this.checkBoxMapToCANMessage.UseVisualStyleBackColor = true;
			this.checkBoxMapToCANMessage.CheckedChanged += new EventHandler(this.checkBoxMapToCANMessage_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxMapToSystemChannel, "checkBoxMapToSystemChannel");
			this.checkBoxMapToSystemChannel.Name = "checkBoxMapToSystemChannel";
			this.checkBoxMapToSystemChannel.UseVisualStyleBackColor = true;
			this.checkBoxMapToSystemChannel.CheckedChanged += new EventHandler(this.checkBoxMapToSystemChannel_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonMappingModeContinuous, "radioButtonMappingModeContinuous");
			this.radioButtonMappingModeContinuous.Checked = true;
			this.errorProviderLocalModel.SetIconAlignment(this.radioButtonMappingModeContinuous, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeContinuous.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonMappingModeContinuous, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeContinuous.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonMappingModeContinuous, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeContinuous.IconAlignment2"));
			this.radioButtonMappingModeContinuous.Name = "radioButtonMappingModeContinuous";
			this.radioButtonMappingModeContinuous.TabStop = true;
			this.radioButtonMappingModeContinuous.UseVisualStyleBackColor = true;
			this.radioButtonMappingModeContinuous.CheckedChanged += new EventHandler(this.radioButtonMappingMode_CheckedChanged);
			this.radioButtonMappingModeContinuous.MouseEnter += new EventHandler(this.OnRadioButtonMappingMode_MouseEnter);
			componentResourceManager.ApplyResources(this.radioButtonMappingModeIndividual, "radioButtonMappingModeIndividual");
			this.errorProviderLocalModel.SetIconAlignment(this.radioButtonMappingModeIndividual, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeIndividual.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonMappingModeIndividual, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeIndividual.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonMappingModeIndividual, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeIndividual.IconAlignment2"));
			this.radioButtonMappingModeIndividual.Name = "radioButtonMappingModeIndividual";
			this.radioButtonMappingModeIndividual.UseVisualStyleBackColor = true;
			this.radioButtonMappingModeIndividual.CheckedChanged += new EventHandler(this.radioButtonMappingMode_CheckedChanged);
			this.radioButtonMappingModeIndividual.MouseEnter += new EventHandler(this.OnRadioButtonMappingMode_MouseEnter);
			componentResourceManager.ApplyResources(this.radioButtonMappingModeFixed, "radioButtonMappingModeFixed");
			this.errorProviderLocalModel.SetIconAlignment(this.radioButtonMappingModeFixed, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeFixed.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.radioButtonMappingModeFixed, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeFixed.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.radioButtonMappingModeFixed, (ErrorIconAlignment)componentResourceManager.GetObject("radioButtonMappingModeFixed.IconAlignment2"));
			this.radioButtonMappingModeFixed.Name = "radioButtonMappingModeFixed";
			this.radioButtonMappingModeFixed.UseVisualStyleBackColor = true;
			this.radioButtonMappingModeFixed.CheckedChanged += new EventHandler(this.radioButtonMappingMode_CheckedChanged);
			this.radioButtonMappingModeFixed.MouseEnter += new EventHandler(this.OnRadioButtonMappingMode_MouseEnter);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			this.AutoValidate = AutoValidate.EnableAllowFocusChange;
			base.Controls.Add(this.groupBoxMapping);
			base.Controls.Add(this.groupBoxAnalogInputs);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "AnalogInputsGL3Plus";
			this.groupBoxAnalogInputs.ResumeLayout(false);
			this.tableLayoutPanelAnalogInputs.ResumeLayout(false);
			this.tableLayoutPanelAnalogInputs.PerformLayout();
			this.groupBoxMapping.ResumeLayout(false);
			this.groupBoxMapping.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
