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
	internal class AnalogInputsGL2000 : UserControl
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

		private ComboBox comboBoxChannel;

		private ToolTip toolTip;

		private GroupBox groupBoxMapping;

		private Label labelCANMode;

		private Label labelCANChannel;

		private CheckBox checkBoxMapToCANMessage;

		private CheckBox checkBoxMapToSystemChannel;

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

		public AnalogInputsGL2000()
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
					break;
				}
				default:
					return;
				}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AnalogInputsGL2000));
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
			this.labelCANID1 = new Label();
			this.labelCANID2 = new Label();
			this.labelCANID3 = new Label();
			this.labelCANID4 = new Label();
			this.textBoxAnalogInputCANID4 = new TextBox();
			this.checkBoxAveraging = new CheckBox();
			this.radioButtonMappingModeContinuous = new RadioButton();
			this.radioButtonMappingModeIndividual = new RadioButton();
			this.radioButtonMappingModeFixed = new RadioButton();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.comboBoxChannel = new ComboBox();
			this.groupBoxMapping = new GroupBox();
			this.labelCANMode = new Label();
			this.labelCANChannel = new Label();
			this.checkBoxMapToCANMessage = new CheckBox();
			this.checkBoxMapToSystemChannel = new CheckBox();
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.toolTip = new ToolTip(this.components);
			this.groupBoxAnalogInputs.SuspendLayout();
			this.tableLayoutPanelAnalogInputs.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			this.groupBoxMapping.SuspendLayout();
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
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput3, 0, 2);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAnalogInput4, 0, 3);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency1, 1, 0);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency2, 1, 1);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency3, 1, 2);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.comboBoxAnalogInputFrequency4, 1, 3);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID1, 3, 0);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID2, 3, 1);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID3, 3, 2);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID1, 2, 0);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID2, 2, 1);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID3, 2, 2);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.labelCANID4, 2, 3);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.textBoxAnalogInputCANID4, 3, 3);
			this.tableLayoutPanelAnalogInputs.Controls.Add(this.checkBoxAveraging, 1, 4);
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
			componentResourceManager.ApplyResources(this.labelCANID1, "labelCANID1");
			this.labelCANID1.Name = "labelCANID1";
			componentResourceManager.ApplyResources(this.labelCANID2, "labelCANID2");
			this.labelCANID2.Name = "labelCANID2";
			componentResourceManager.ApplyResources(this.labelCANID3, "labelCANID3");
			this.labelCANID3.Name = "labelCANID3";
			componentResourceManager.ApplyResources(this.labelCANID4, "labelCANID4");
			this.labelCANID4.Name = "labelCANID4";
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxAnalogInputCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID4.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxAnalogInputCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID4.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxAnalogInputCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxAnalogInputCANID4.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxAnalogInputCANID4, "textBoxAnalogInputCANID4");
			this.textBoxAnalogInputCANID4.Name = "textBoxAnalogInputCANID4";
			this.textBoxAnalogInputCANID4.Validating += new CancelEventHandler(this.textBoxAnalogInputCANID_Validating);
			componentResourceManager.ApplyResources(this.checkBoxAveraging, "checkBoxAveraging");
			this.checkBoxAveraging.Checked = true;
			this.checkBoxAveraging.CheckState = CheckState.Checked;
			this.checkBoxAveraging.Name = "checkBoxAveraging";
			this.checkBoxAveraging.UseVisualStyleBackColor = true;
			this.checkBoxAveraging.CheckedChanged += new EventHandler(this.checkBoxAveraging_CheckedChanged);
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
			this.comboBoxChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxChannel.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxChannel.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxChannel, "comboBoxChannel");
			this.comboBoxChannel.Name = "comboBoxChannel";
			this.comboBoxChannel.SelectedIndexChanged += new EventHandler(this.comboBoxChannel_SelectedIndexChanged);
			this.groupBoxMapping.Controls.Add(this.radioButtonMappingModeContinuous);
			this.groupBoxMapping.Controls.Add(this.radioButtonMappingModeIndividual);
			this.groupBoxMapping.Controls.Add(this.labelCANMode);
			this.groupBoxMapping.Controls.Add(this.radioButtonMappingModeFixed);
			this.groupBoxMapping.Controls.Add(this.comboBoxChannel);
			this.groupBoxMapping.Controls.Add(this.labelCANChannel);
			this.groupBoxMapping.Controls.Add(this.checkBoxMapToCANMessage);
			this.groupBoxMapping.Controls.Add(this.checkBoxMapToSystemChannel);
			componentResourceManager.ApplyResources(this.groupBoxMapping, "groupBoxMapping");
			this.groupBoxMapping.Name = "groupBoxMapping";
			this.groupBoxMapping.TabStop = false;
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
			base.Name = "AnalogInputsGL2000";
			this.groupBoxAnalogInputs.ResumeLayout(false);
			this.tableLayoutPanelAnalogInputs.ResumeLayout(false);
			this.tableLayoutPanelAnalogInputs.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			this.groupBoxMapping.ResumeLayout(false);
			this.groupBoxMapping.PerformLayout();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
