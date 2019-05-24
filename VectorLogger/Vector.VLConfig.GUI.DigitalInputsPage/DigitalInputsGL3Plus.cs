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

namespace Vector.VLConfig.GUI.DigitalInputsPage
{
	internal class DigitalInputsGL3Plus : UserControl
	{
		private Dictionary<uint, CheckBox> inputMapCheckBoxFrequency;

		private Dictionary<uint, ComboBox> inputMapComboBoxFrequency;

		private Dictionary<uint, CheckBox> inputMapCheckBoxOnChange;

		private Dictionary<uint, TextBox> inputMapTextBoxCANId;

		private DigitalInputConfiguration digitalInputConfiguration;

		private DisplayMode displayMode;

		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isInitControls;

		private IContainer components;

		private ErrorProvider errorProviderGlobalModel;

		private GroupBox groupBoxDigitalInput;

		private TextBox textBoxDigitalInputCANID2;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private Label labelDigitalInputCANId2;

		private TextBox textBoxDigitalInputCANID1;

		private Label labelDigitalInputCANId1;

		private CheckBox checkBoxDigitalInputOnChange2;

		private CheckBox checkBoxDigitalInputOnChange1;

		private ComboBox comboBoxDigitalInputFrequency2;

		private ComboBox comboBoxDigitalInputFrequency1;

		private CheckBox checkBoxDigitalInputFrequency2;

		private CheckBox checkBoxDigitalInputFrequency1;

		private Label labelDigitalInputDigital2;

		private Label labelDigitalInputDigital1;

		private GroupBox groupBoxDigitalInputSendOnChannel;

		private ComboBox comboBoxDigitalInputSendOnChannel;

		private GroupBox groupBoxDigitalInputMappingMode;

		private RadioButton radioButtonDigitalInputMappingContinuous;

		private RadioButton radioButtonDigitalInputMappingIndividual;

		private RadioButton radioButtonDigitalInputMappingSeparate;

		private RadioButton radioButtonDigitalInputMappingCombined;

		private TextBox textBoxDigitalInputCANID3;

		private Label labelDigitalInputCANId3;

		private CheckBox checkBoxDigitalInputOnChange3;

		private ComboBox comboBoxDigitalInputFrequency3;

		private CheckBox checkBoxDigitalInputFrequency3;

		private Label labelDigitalInputDigital3;

		private TextBox textBoxDigitalInputCANID4;

		private Label labelDigitalInputCANId4;

		private CheckBox checkBoxDigitalInputOnChange4;

		private ComboBox comboBoxDigitalInputFrequency4;

		private CheckBox checkBoxDigitalInputFrequency4;

		private Label labelDigitalInputDigital4;

		private TextBox textBoxDigitalInputCANID8;

		private Label labelDigitalInputCANId8;

		private CheckBox checkBoxDigitalInputOnChange8;

		private ComboBox comboBoxDigitalInputFrequency8;

		private CheckBox checkBoxDigitalInputFrequency8;

		private Label labelDigitalInputDigital8;

		private TextBox textBoxDigitalInputCANID7;

		private Label labelDigitalInputCANId7;

		private CheckBox checkBoxDigitalInputOnChange7;

		private ComboBox comboBoxDigitalInputFrequency7;

		private CheckBox checkBoxDigitalInputFrequency7;

		private Label labelDigitalInputDigital7;

		private TextBox textBoxDigitalInputCANID6;

		private Label labelDigitalInputCANId6;

		private CheckBox checkBoxDigitalInputOnChange6;

		private ComboBox comboBoxDigitalInputFrequency6;

		private CheckBox checkBoxDigitalInputFrequency6;

		private Label labelDigitalInputDigital6;

		private TextBox textBoxDigitalInputCANID5;

		private Label labelDigitalInputCANId5;

		private CheckBox checkBoxDigitalInputOnChange5;

		private ComboBox comboBoxDigitalInputFrequency5;

		private CheckBox checkBoxDigitalInputFrequency5;

		private Label labelDigitalInputDigital5;

		private TableLayoutPanel tableLayoutPanel1;

		public DigitalInputConfiguration DigitalInputConfiguration
		{
			get
			{
				return this.digitalInputConfiguration;
			}
			set
			{
				this.digitalInputConfiguration = value;
				if (this.digitalInputConfiguration != null)
				{
					if (this.ModelValidator != null)
					{
						ulong arg_45_0 = (ulong)this.ModelValidator.LoggerSpecifics.IO.NumberOfDigitalInputs;
						long arg_44_0 = (long)this.digitalInputConfiguration.DigitalInputs.Count;
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
				if (this.inputMapTextBoxCANId != null && this.digitalInputConfiguration != null)
				{
					uint num = 1u;
					while ((ulong)num <= (ulong)((long)this.inputMapTextBoxCANId.Count))
					{
						uint value2 = this.digitalInputConfiguration.GetDigitalInput(num).MappedCANId.Value;
						bool value3 = this.digitalInputConfiguration.GetDigitalInput(num).IsMappedCANIdExtended.Value;
						this.inputMapTextBoxCANId[num].Text = GUIUtil.CANIdToDisplayString(value2, value3);
						num += 1u;
					}
					this.ValidateInput();
				}
			}
		}

		public DigitalInputsGL3Plus()
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
			this.InitControlLists();
			this.isInitControls = true;
			this.InitAvailableFrequencies();
			this.InitSendOnChannelComboBox();
			this.isInitControls = false;
		}

		private void InitControlLists()
		{
			this.inputMapCheckBoxFrequency = new Dictionary<uint, CheckBox>();
			this.inputMapCheckBoxFrequency.Add(1u, this.checkBoxDigitalInputFrequency1);
			this.inputMapCheckBoxFrequency.Add(2u, this.checkBoxDigitalInputFrequency2);
			this.inputMapCheckBoxFrequency.Add(3u, this.checkBoxDigitalInputFrequency3);
			this.inputMapCheckBoxFrequency.Add(4u, this.checkBoxDigitalInputFrequency4);
			this.inputMapCheckBoxFrequency.Add(5u, this.checkBoxDigitalInputFrequency5);
			this.inputMapCheckBoxFrequency.Add(6u, this.checkBoxDigitalInputFrequency6);
			this.inputMapCheckBoxFrequency.Add(7u, this.checkBoxDigitalInputFrequency7);
			this.inputMapCheckBoxFrequency.Add(8u, this.checkBoxDigitalInputFrequency8);
			this.inputMapComboBoxFrequency = new Dictionary<uint, ComboBox>();
			this.inputMapComboBoxFrequency.Add(1u, this.comboBoxDigitalInputFrequency1);
			this.inputMapComboBoxFrequency.Add(2u, this.comboBoxDigitalInputFrequency2);
			this.inputMapComboBoxFrequency.Add(3u, this.comboBoxDigitalInputFrequency3);
			this.inputMapComboBoxFrequency.Add(4u, this.comboBoxDigitalInputFrequency4);
			this.inputMapComboBoxFrequency.Add(5u, this.comboBoxDigitalInputFrequency5);
			this.inputMapComboBoxFrequency.Add(6u, this.comboBoxDigitalInputFrequency6);
			this.inputMapComboBoxFrequency.Add(7u, this.comboBoxDigitalInputFrequency7);
			this.inputMapComboBoxFrequency.Add(8u, this.comboBoxDigitalInputFrequency8);
			this.inputMapCheckBoxOnChange = new Dictionary<uint, CheckBox>();
			this.inputMapCheckBoxOnChange.Add(1u, this.checkBoxDigitalInputOnChange1);
			this.inputMapCheckBoxOnChange.Add(2u, this.checkBoxDigitalInputOnChange2);
			this.inputMapCheckBoxOnChange.Add(3u, this.checkBoxDigitalInputOnChange3);
			this.inputMapCheckBoxOnChange.Add(4u, this.checkBoxDigitalInputOnChange4);
			this.inputMapCheckBoxOnChange.Add(5u, this.checkBoxDigitalInputOnChange5);
			this.inputMapCheckBoxOnChange.Add(6u, this.checkBoxDigitalInputOnChange6);
			this.inputMapCheckBoxOnChange.Add(7u, this.checkBoxDigitalInputOnChange7);
			this.inputMapCheckBoxOnChange.Add(8u, this.checkBoxDigitalInputOnChange8);
			this.inputMapTextBoxCANId = new Dictionary<uint, TextBox>();
			this.inputMapTextBoxCANId.Add(1u, this.textBoxDigitalInputCANID1);
			this.inputMapTextBoxCANId.Add(2u, this.textBoxDigitalInputCANID2);
			this.inputMapTextBoxCANId.Add(3u, this.textBoxDigitalInputCANID3);
			this.inputMapTextBoxCANId.Add(4u, this.textBoxDigitalInputCANID4);
			this.inputMapTextBoxCANId.Add(5u, this.textBoxDigitalInputCANID5);
			this.inputMapTextBoxCANId.Add(6u, this.textBoxDigitalInputCANID6);
			this.inputMapTextBoxCANId.Add(7u, this.textBoxDigitalInputCANID7);
			this.inputMapTextBoxCANId.Add(8u, this.textBoxDigitalInputCANID8);
		}

		private void InitAvailableFrequencies()
		{
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.inputMapComboBoxFrequency.Count))
			{
				this.InitAvailableFrequencies(this.inputMapComboBoxFrequency[num]);
				num += 1u;
			}
		}

		private void InitAvailableFrequencies(ComboBox comboBox)
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

		private void InitSendOnChannelComboBox()
		{
			this.comboBoxDigitalInputSendOnChannel.Items.Clear();
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels; num += 1u)
			{
				this.comboBoxDigitalInputSendOnChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
			uint num2 = this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels + 1u;
			for (uint num3 = num2; num3 < num2 + this.ModelValidator.LoggerSpecifics.CAN.NumberOfVirtualChannels; num3 += 1u)
			{
				this.comboBoxDigitalInputSendOnChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num3) + Resources.VirtualChannelPostfix);
			}
			if (this.comboBoxDigitalInputSendOnChannel.Items.Count > 0)
			{
				this.comboBoxDigitalInputSendOnChannel.SelectedIndex = 0;
			}
		}

		public bool ValidateInput()
		{
			if (this.digitalInputConfiguration == null)
			{
				return false;
			}
			bool flag = true;
			bool flag2 = false;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			DigitalInputsMappingMode mappingMode = this.GetMappingMode();
			bool flag3;
			flag &= this.pageValidator.Control.UpdateModel<DigitalInputsMappingMode>(mappingMode, this.DigitalInputConfiguration.DigitalInputsMappingMode, this.guiElementManager.GetGUIElement(this.radioButtonDigitalInputMappingSeparate), out flag3);
			flag2 |= flag3;
			if (this.comboBoxDigitalInputSendOnChannel.Enabled)
			{
				uint selectedChannel = this.GetSelectedChannel();
				flag &= this.pageValidator.Control.UpdateModel<uint>(selectedChannel, this.DigitalInputConfiguration.CanChannel, this.guiElementManager.GetGUIElement(this.comboBoxDigitalInputSendOnChannel), out flag3);
				flag2 |= flag3;
			}
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.inputMapCheckBoxFrequency[1u].Checked, this.DigitalInputConfiguration.GetDigitalInput(1u).IsActiveFrequency, this.guiElementManager.GetGUIElement(this.inputMapCheckBoxFrequency[1u]), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.UpdateModel<uint>(this.GetSelectedFrequency(this.inputMapComboBoxFrequency[1u]), this.digitalInputConfiguration.GetDigitalInput(1u).Frequency, this.guiElementManager.GetGUIElement(this.inputMapComboBoxFrequency[1u]), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.inputMapCheckBoxOnChange[1u].Checked, this.DigitalInputConfiguration.GetDigitalInput(1u).IsActiveOnChange, this.guiElementManager.GetGUIElement(this.inputMapCheckBoxOnChange[1u]), out flag3);
			flag2 |= flag3;
			bool flag4 = false;
			if (DigitalInputsMappingMode.IndividualIDs != mappingMode || this.inputMapTextBoxCANId[1u].Enabled)
			{
				flag4 = this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.inputMapTextBoxCANId[1u].Text, this.digitalInputConfiguration.GetDigitalInput(1u).MappedCANId, this.digitalInputConfiguration.GetDigitalInput(1u).IsMappedCANIdExtended, this.guiElementManager.GetGUIElement(this.inputMapTextBoxCANId[1u]), out flag3);
				flag &= flag4;
				flag2 |= flag3;
			}
			uint num = 2u;
			while ((ulong)num <= (ulong)((long)this.inputMapTextBoxCANId.Count))
			{
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.inputMapCheckBoxFrequency[num].Checked, this.DigitalInputConfiguration.GetDigitalInput(num).IsActiveFrequency, this.guiElementManager.GetGUIElement(this.inputMapCheckBoxFrequency[num]), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.inputMapCheckBoxOnChange[num].Checked, this.DigitalInputConfiguration.GetDigitalInput(num).IsActiveOnChange, this.guiElementManager.GetGUIElement(this.inputMapCheckBoxOnChange[num]), out flag3);
				flag2 |= flag3;
				if (this.DigitalInputConfiguration.GetDigitalInput(num).IsActiveFrequency.Value || this.DigitalInputConfiguration.GetDigitalInput(num).IsActiveOnChange.Value || DigitalInputsMappingMode.GroupedSeparateIDs == mappingMode || mappingMode == DigitalInputsMappingMode.GroupedCombinedIDs)
				{
					uint value;
					if (DigitalInputsMappingMode.GroupedSeparateIDs == mappingMode || mappingMode == DigitalInputsMappingMode.GroupedCombinedIDs)
					{
						value = this.digitalInputConfiguration.GetDigitalInput(1u).Frequency.Value;
					}
					else
					{
						value = this.GetSelectedFrequency(this.inputMapComboBoxFrequency[num]);
					}
					flag &= this.pageValidator.Control.UpdateModel<uint>(value, this.digitalInputConfiguration.GetDigitalInput(num).Frequency, this.guiElementManager.GetGUIElement(this.inputMapComboBoxFrequency[num]), out flag3);
					flag2 |= flag3;
				}
				if ((DigitalInputsMappingMode.IndividualIDs == mappingMode && this.inputMapTextBoxCANId[num].Enabled) || (DigitalInputsMappingMode.IndividualIDs != mappingMode && flag4))
				{
					string valueAsStringFromGUI;
					if (DigitalInputsMappingMode.IndividualIDs != mappingMode)
					{
						bool isFixedIdsMode = DigitalInputsMappingMode.GroupedSeparateIDs == mappingMode || DigitalInputsMappingMode.GroupedCombinedIDs == mappingMode;
						uint canId = GUIUtil.CalculateDigitalInputMappedCANId(this.digitalInputConfiguration.GetDigitalInput(1u).MappedCANId.Value, this.digitalInputConfiguration.GetDigitalInput(1u).IsMappedCANIdExtended.Value, num, isFixedIdsMode);
						valueAsStringFromGUI = GUIUtil.CANIdToDisplayString(canId, this.digitalInputConfiguration.GetDigitalInput(1u).IsMappedCANIdExtended.Value);
					}
					else
					{
						valueAsStringFromGUI = this.inputMapTextBoxCANId[num].Text;
					}
					flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(valueAsStringFromGUI, this.digitalInputConfiguration.GetDigitalInput(num).MappedCANId, this.digitalInputConfiguration.GetDigitalInput(num).IsMappedCANIdExtended, this.guiElementManager.GetGUIElement(this.inputMapTextBoxCANId[num]), out flag3);
					flag2 |= flag3;
				}
				num += 1u;
			}
			flag &= this.ModelValidator.Validate(this.digitalInputConfiguration, flag2, this.pageValidator);
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

		private void UpdateGUI()
		{
			if (this.digitalInputConfiguration == null)
			{
				return;
			}
			this.isInitControls = true;
			if (!this.pageValidator.General.HasFormatError(this.digitalInputConfiguration.CanChannel))
			{
				if (this.digitalInputConfiguration.CanChannel.Value <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels)
				{
					this.comboBoxDigitalInputSendOnChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.digitalInputConfiguration.CanChannel.Value);
				}
				else
				{
					this.comboBoxDigitalInputSendOnChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.digitalInputConfiguration.CanChannel.Value) + Resources.VirtualChannelPostfix;
				}
			}
			if (!this.pageValidator.General.HasFormatError(this.digitalInputConfiguration.DigitalInputsMappingMode))
			{
				this.EnableControlsForMappingMode(this.digitalInputConfiguration.DigitalInputsMappingMode.Value);
				switch (this.digitalInputConfiguration.DigitalInputsMappingMode.Value)
				{
				case DigitalInputsMappingMode.GroupedCombinedIDs:
					this.radioButtonDigitalInputMappingCombined.Checked = true;
					goto IL_14D;
				case DigitalInputsMappingMode.IndividualIDs:
					this.radioButtonDigitalInputMappingIndividual.Checked = true;
					goto IL_14D;
				case DigitalInputsMappingMode.ContinuousIndividualIDs:
					this.radioButtonDigitalInputMappingContinuous.Checked = true;
					goto IL_14D;
				}
				this.radioButtonDigitalInputMappingSeparate.Checked = true;
			}
			IL_14D:
			this.comboBoxDigitalInputSendOnChannel.Enabled = false;
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.digitalInputConfiguration.DigitalInputs.Count))
			{
				DigitalInput digitalInput = this.digitalInputConfiguration.GetDigitalInput(num);
				if (!this.pageValidator.General.HasFormatError(digitalInput.IsActiveFrequency))
				{
					this.inputMapCheckBoxFrequency[num].Checked = digitalInput.IsActiveFrequency.Value;
				}
				this.EnableControlsInRow(this.inputMapCheckBoxFrequency[num]);
				if (!this.pageValidator.General.HasFormatError(digitalInput.IsActiveOnChange))
				{
					this.inputMapCheckBoxOnChange[num].Checked = digitalInput.IsActiveOnChange.Value;
				}
				this.EnableControlsInRow(this.inputMapCheckBoxOnChange[num]);
				if (!this.pageValidator.General.HasFormatError(digitalInput.Frequency))
				{
					this.inputMapComboBoxFrequency[num].SelectedItem = GUIUtil.MapAnalogInputsFrequency2String(digitalInput.Frequency.Value);
				}
				if (!this.pageValidator.General.HasFormatError(digitalInput.MappedCANId))
				{
					this.inputMapTextBoxCANId[num].Text = GUIUtil.CANIdToDisplayString(digitalInput.MappedCANId.Value, digitalInput.IsMappedCANIdExtended.Value);
				}
				num += 1u;
			}
			this.isInitControls = false;
			this.ValidateInput();
		}

		private void EnableControlsForMappingMode(DigitalInputsMappingMode mappingMode)
		{
			switch (this.digitalInputConfiguration.DigitalInputsMappingMode.Value)
			{
			case DigitalInputsMappingMode.GroupedCombinedIDs:
			case DigitalInputsMappingMode.GroupedSeparateIDs:
			{
				uint num = 2u;
				while ((ulong)num <= (ulong)((long)this.digitalInputConfiguration.DigitalInputs.Count))
				{
					this.inputMapTextBoxCANId[num].Enabled = false;
					this.inputMapComboBoxFrequency[num].Enabled = false;
					num += 1u;
				}
				return;
			}
			case DigitalInputsMappingMode.IndividualIDs:
			{
				uint num2 = 2u;
				while ((ulong)num2 <= (ulong)((long)this.digitalInputConfiguration.DigitalInputs.Count))
				{
					this.inputMapTextBoxCANId[num2].Enabled = this.HasActiveCheckbox(num2);
					this.inputMapComboBoxFrequency[num2].Enabled = this.inputMapCheckBoxFrequency[num2].Checked;
					num2 += 1u;
				}
				return;
			}
			case DigitalInputsMappingMode.ContinuousIndividualIDs:
			{
				uint num3 = 2u;
				while ((ulong)num3 <= (ulong)((long)this.digitalInputConfiguration.DigitalInputs.Count))
				{
					this.inputMapTextBoxCANId[num3].Enabled = false;
					this.inputMapComboBoxFrequency[num3].Enabled = this.inputMapCheckBoxFrequency[num3].Checked;
					num3 += 1u;
				}
				return;
			}
			default:
				return;
			}
		}

		private void EnableControlsInRow(CheckBox checkBox)
		{
			KeyValuePair<uint, CheckBox> keyValuePair = this.inputMapCheckBoxFrequency.FirstOrDefault((KeyValuePair<uint, CheckBox> r) => r.Value == checkBox);
			KeyValuePair<uint, CheckBox> keyValuePair2 = this.inputMapCheckBoxOnChange.FirstOrDefault((KeyValuePair<uint, CheckBox> r) => r.Value == checkBox);
			uint key;
			if (!keyValuePair.Equals(default(KeyValuePair<uint, CheckBox>)))
			{
				key = keyValuePair.Key;
			}
			else
			{
				if (keyValuePair2.Equals(default(KeyValuePair<uint, CheckBox>)))
				{
					return;
				}
				key = keyValuePair2.Key;
			}
			if (this.HasActiveCheckbox(key))
			{
				this.comboBoxDigitalInputSendOnChannel.Enabled = true;
			}
			if (key > 1u)
			{
				if (this.digitalInputConfiguration.DigitalInputsMappingMode.Value == DigitalInputsMappingMode.GroupedSeparateIDs || this.digitalInputConfiguration.DigitalInputsMappingMode.Value == DigitalInputsMappingMode.GroupedCombinedIDs)
				{
					this.inputMapComboBoxFrequency[key].Enabled = false;
					this.inputMapTextBoxCANId[key].Enabled = false;
					return;
				}
				if (this.digitalInputConfiguration.DigitalInputsMappingMode.Value == DigitalInputsMappingMode.ContinuousIndividualIDs)
				{
					this.inputMapComboBoxFrequency[key].Enabled = this.inputMapCheckBoxFrequency[key].Checked;
					this.inputMapTextBoxCANId[key].Enabled = false;
					return;
				}
				this.inputMapComboBoxFrequency[key].Enabled = this.inputMapCheckBoxFrequency[key].Checked;
				this.inputMapTextBoxCANId[key].Enabled = this.HasActiveCheckbox(key);
				return;
			}
			else
			{
				if (this.digitalInputConfiguration.DigitalInputsMappingMode.Value == DigitalInputsMappingMode.GroupedSeparateIDs || this.digitalInputConfiguration.DigitalInputsMappingMode.Value == DigitalInputsMappingMode.GroupedCombinedIDs)
				{
					this.inputMapComboBoxFrequency[key].Enabled = true;
				}
				else
				{
					this.inputMapComboBoxFrequency[key].Enabled = this.inputMapCheckBoxFrequency[key].Checked;
				}
				if (this.digitalInputConfiguration.DigitalInputsMappingMode.Value == DigitalInputsMappingMode.GroupedSeparateIDs || this.digitalInputConfiguration.DigitalInputsMappingMode.Value == DigitalInputsMappingMode.GroupedCombinedIDs || this.digitalInputConfiguration.DigitalInputsMappingMode.Value == DigitalInputsMappingMode.ContinuousIndividualIDs)
				{
					this.inputMapTextBoxCANId[key].Enabled = true;
					return;
				}
				this.inputMapTextBoxCANId[key].Enabled = this.HasActiveCheckbox(key);
				return;
			}
		}

		private uint GetSelectedFrequency(uint inputNr)
		{
			return this.GetSelectedFrequency(this.inputMapComboBoxFrequency[inputNr]);
		}

		private uint GetSelectedFrequency(ComboBox comboBox)
		{
			return GUIUtil.MapString2AnalogInputsFrequency(comboBox.SelectedItem.ToString());
		}

		private DigitalInputsMappingMode GetMappingMode()
		{
			if (this.radioButtonDigitalInputMappingContinuous.Checked)
			{
				return DigitalInputsMappingMode.ContinuousIndividualIDs;
			}
			if (this.radioButtonDigitalInputMappingSeparate.Checked)
			{
				return DigitalInputsMappingMode.GroupedSeparateIDs;
			}
			if (this.radioButtonDigitalInputMappingCombined.Checked)
			{
				return DigitalInputsMappingMode.GroupedCombinedIDs;
			}
			return DigitalInputsMappingMode.IndividualIDs;
		}

		private uint GetSelectedChannel()
		{
			return GUIUtil.MapCANChannelString2Number(this.comboBoxDigitalInputSendOnChannel.SelectedItem.ToString());
		}

		private bool HasActiveCheckbox(uint inputNr)
		{
			return this.inputMapCheckBoxFrequency[inputNr].Checked || this.inputMapCheckBoxOnChange[inputNr].Checked;
		}

		private void checkBoxDigitalInputFrequency_CheckedChanged(object sender, EventArgs e)
		{
			this.EnableControlsInRow(sender as CheckBox);
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxDigitalInputOnChange_CheckedChanged(object sender, EventArgs e)
		{
			this.EnableControlsInRow(sender as CheckBox);
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void radioButtonDigitalInputMappingMode_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.EnableControlsForMappingMode(this.GetMappingMode());
			this.ValidateInput();
		}

		private void comboBoxDigitalInputSendOnChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBoxDigitalInputFrequency_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void textBoxDigitalInputCANID_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxDigitalInput_Paint(object sender, PaintEventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox != null && checkBox.Focused)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, e.ClipRectangle, checkBox.ForeColor, checkBox.BackColor);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DigitalInputsGL3Plus));
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.textBoxDigitalInputCANID2 = new TextBox();
			this.textBoxDigitalInputCANID1 = new TextBox();
			this.textBoxDigitalInputCANID3 = new TextBox();
			this.textBoxDigitalInputCANID4 = new TextBox();
			this.textBoxDigitalInputCANID5 = new TextBox();
			this.textBoxDigitalInputCANID6 = new TextBox();
			this.textBoxDigitalInputCANID7 = new TextBox();
			this.textBoxDigitalInputCANID8 = new TextBox();
			this.comboBoxDigitalInputSendOnChannel = new ComboBox();
			this.groupBoxDigitalInput = new GroupBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.labelDigitalInputDigital1 = new Label();
			this.labelDigitalInputDigital3 = new Label();
			this.labelDigitalInputCANId8 = new Label();
			this.labelDigitalInputDigital2 = new Label();
			this.checkBoxDigitalInputOnChange8 = new CheckBox();
			this.labelDigitalInputCANId7 = new Label();
			this.checkBoxDigitalInputOnChange2 = new CheckBox();
			this.checkBoxDigitalInputOnChange1 = new CheckBox();
			this.labelDigitalInputCANId6 = new Label();
			this.labelDigitalInputDigital5 = new Label();
			this.checkBoxDigitalInputOnChange7 = new CheckBox();
			this.labelDigitalInputCANId5 = new Label();
			this.labelDigitalInputDigital4 = new Label();
			this.labelDigitalInputDigital7 = new Label();
			this.labelDigitalInputCANId4 = new Label();
			this.comboBoxDigitalInputFrequency8 = new ComboBox();
			this.checkBoxDigitalInputOnChange6 = new CheckBox();
			this.labelDigitalInputCANId3 = new Label();
			this.labelDigitalInputDigital8 = new Label();
			this.checkBoxDigitalInputFrequency8 = new CheckBox();
			this.labelDigitalInputCANId2 = new Label();
			this.labelDigitalInputDigital6 = new Label();
			this.checkBoxDigitalInputOnChange5 = new CheckBox();
			this.labelDigitalInputCANId1 = new Label();
			this.checkBoxDigitalInputFrequency1 = new CheckBox();
			this.comboBoxDigitalInputFrequency7 = new ComboBox();
			this.checkBoxDigitalInputFrequency2 = new CheckBox();
			this.checkBoxDigitalInputOnChange4 = new CheckBox();
			this.checkBoxDigitalInputFrequency3 = new CheckBox();
			this.checkBoxDigitalInputFrequency4 = new CheckBox();
			this.checkBoxDigitalInputFrequency7 = new CheckBox();
			this.checkBoxDigitalInputOnChange3 = new CheckBox();
			this.comboBoxDigitalInputFrequency6 = new ComboBox();
			this.checkBoxDigitalInputFrequency5 = new CheckBox();
			this.checkBoxDigitalInputFrequency6 = new CheckBox();
			this.comboBoxDigitalInputFrequency1 = new ComboBox();
			this.comboBoxDigitalInputFrequency2 = new ComboBox();
			this.comboBoxDigitalInputFrequency5 = new ComboBox();
			this.comboBoxDigitalInputFrequency3 = new ComboBox();
			this.comboBoxDigitalInputFrequency4 = new ComboBox();
			this.radioButtonDigitalInputMappingCombined = new RadioButton();
			this.radioButtonDigitalInputMappingSeparate = new RadioButton();
			this.groupBoxDigitalInputSendOnChannel = new GroupBox();
			this.radioButtonDigitalInputMappingIndividual = new RadioButton();
			this.radioButtonDigitalInputMappingContinuous = new RadioButton();
			this.groupBoxDigitalInputMappingMode = new GroupBox();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			this.groupBoxDigitalInput.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupBoxDigitalInputSendOnChannel.SuspendLayout();
			this.groupBoxDigitalInputMappingMode.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			base.SuspendLayout();
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this.textBoxDigitalInputCANID2, "textBoxDigitalInputCANID2");
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDigitalInputCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID2.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDigitalInputCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID2.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDigitalInputCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID2.IconAlignment2"));
			this.textBoxDigitalInputCANID2.Name = "textBoxDigitalInputCANID2";
			this.textBoxDigitalInputCANID2.Validating += new CancelEventHandler(this.textBoxDigitalInputCANID_Validating);
			componentResourceManager.ApplyResources(this.textBoxDigitalInputCANID1, "textBoxDigitalInputCANID1");
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDigitalInputCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID1.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDigitalInputCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID1.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDigitalInputCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID1.IconAlignment2"));
			this.textBoxDigitalInputCANID1.Name = "textBoxDigitalInputCANID1";
			this.textBoxDigitalInputCANID1.Validating += new CancelEventHandler(this.textBoxDigitalInputCANID_Validating);
			componentResourceManager.ApplyResources(this.textBoxDigitalInputCANID3, "textBoxDigitalInputCANID3");
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDigitalInputCANID3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID3.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDigitalInputCANID3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID3.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDigitalInputCANID3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID3.IconAlignment2"));
			this.textBoxDigitalInputCANID3.Name = "textBoxDigitalInputCANID3";
			this.textBoxDigitalInputCANID3.Validating += new CancelEventHandler(this.textBoxDigitalInputCANID_Validating);
			componentResourceManager.ApplyResources(this.textBoxDigitalInputCANID4, "textBoxDigitalInputCANID4");
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDigitalInputCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID4.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDigitalInputCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID4.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDigitalInputCANID4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID4.IconAlignment2"));
			this.textBoxDigitalInputCANID4.Name = "textBoxDigitalInputCANID4";
			this.textBoxDigitalInputCANID4.Validating += new CancelEventHandler(this.textBoxDigitalInputCANID_Validating);
			componentResourceManager.ApplyResources(this.textBoxDigitalInputCANID5, "textBoxDigitalInputCANID5");
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDigitalInputCANID5, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID5.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDigitalInputCANID5, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID5.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDigitalInputCANID5, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID5.IconAlignment2"));
			this.textBoxDigitalInputCANID5.Name = "textBoxDigitalInputCANID5";
			this.textBoxDigitalInputCANID5.Validating += new CancelEventHandler(this.textBoxDigitalInputCANID_Validating);
			componentResourceManager.ApplyResources(this.textBoxDigitalInputCANID6, "textBoxDigitalInputCANID6");
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDigitalInputCANID6, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID6.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDigitalInputCANID6, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID6.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDigitalInputCANID6, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID6.IconAlignment2"));
			this.textBoxDigitalInputCANID6.Name = "textBoxDigitalInputCANID6";
			this.textBoxDigitalInputCANID6.Validating += new CancelEventHandler(this.textBoxDigitalInputCANID_Validating);
			componentResourceManager.ApplyResources(this.textBoxDigitalInputCANID7, "textBoxDigitalInputCANID7");
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDigitalInputCANID7, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID7.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDigitalInputCANID7, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID7.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDigitalInputCANID7, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID7.IconAlignment2"));
			this.textBoxDigitalInputCANID7.Name = "textBoxDigitalInputCANID7";
			this.textBoxDigitalInputCANID7.Validating += new CancelEventHandler(this.textBoxDigitalInputCANID_Validating);
			componentResourceManager.ApplyResources(this.textBoxDigitalInputCANID8, "textBoxDigitalInputCANID8");
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDigitalInputCANID8, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID8.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDigitalInputCANID8, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID8.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDigitalInputCANID8, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID8.IconAlignment2"));
			this.textBoxDigitalInputCANID8.Name = "textBoxDigitalInputCANID8";
			this.textBoxDigitalInputCANID8.Validating += new CancelEventHandler(this.textBoxDigitalInputCANID_Validating);
			componentResourceManager.ApplyResources(this.comboBoxDigitalInputSendOnChannel, "comboBoxDigitalInputSendOnChannel");
			this.comboBoxDigitalInputSendOnChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDigitalInputSendOnChannel.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxDigitalInputSendOnChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDigitalInputSendOnChannel.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxDigitalInputSendOnChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDigitalInputSendOnChannel.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxDigitalInputSendOnChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDigitalInputSendOnChannel.IconAlignment2"));
			this.comboBoxDigitalInputSendOnChannel.Name = "comboBoxDigitalInputSendOnChannel";
			this.comboBoxDigitalInputSendOnChannel.SelectedIndexChanged += new EventHandler(this.comboBoxDigitalInputSendOnChannel_SelectedIndexChanged);
			this.groupBoxDigitalInput.Controls.Add(this.tableLayoutPanel1);
			componentResourceManager.ApplyResources(this.groupBoxDigitalInput, "groupBoxDigitalInput");
			this.groupBoxDigitalInput.Name = "groupBoxDigitalInput";
			this.groupBoxDigitalInput.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputDigital1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.textBoxDigitalInputCANID8, 5, 7);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputDigital3, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.textBoxDigitalInputCANID7, 5, 6);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputCANId8, 4, 7);
			this.tableLayoutPanel1.Controls.Add(this.textBoxDigitalInputCANID6, 5, 5);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputDigital2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.textBoxDigitalInputCANID5, 5, 4);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputOnChange8, 3, 7);
			this.tableLayoutPanel1.Controls.Add(this.textBoxDigitalInputCANID4, 5, 3);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputCANId7, 4, 6);
			this.tableLayoutPanel1.Controls.Add(this.textBoxDigitalInputCANID3, 5, 2);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputOnChange2, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.textBoxDigitalInputCANID2, 5, 1);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputOnChange1, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.textBoxDigitalInputCANID1, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputCANId6, 4, 5);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputDigital5, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputOnChange7, 3, 6);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputCANId5, 4, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputDigital4, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputDigital7, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputCANId4, 4, 3);
			this.tableLayoutPanel1.Controls.Add(this.comboBoxDigitalInputFrequency8, 2, 7);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputOnChange6, 3, 5);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputCANId3, 4, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputDigital8, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputFrequency8, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputCANId2, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputDigital6, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputOnChange5, 3, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputCANId1, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputFrequency1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.comboBoxDigitalInputFrequency7, 2, 6);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputFrequency2, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputOnChange4, 3, 3);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputFrequency3, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputFrequency4, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputFrequency7, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputOnChange3, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.comboBoxDigitalInputFrequency6, 2, 5);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputFrequency5, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputFrequency6, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.comboBoxDigitalInputFrequency1, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.comboBoxDigitalInputFrequency2, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.comboBoxDigitalInputFrequency5, 2, 4);
			this.tableLayoutPanel1.Controls.Add(this.comboBoxDigitalInputFrequency3, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.comboBoxDigitalInputFrequency4, 2, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.labelDigitalInputDigital1, "labelDigitalInputDigital1");
			this.labelDigitalInputDigital1.Name = "labelDigitalInputDigital1";
			componentResourceManager.ApplyResources(this.labelDigitalInputDigital3, "labelDigitalInputDigital3");
			this.labelDigitalInputDigital3.Name = "labelDigitalInputDigital3";
			componentResourceManager.ApplyResources(this.labelDigitalInputCANId8, "labelDigitalInputCANId8");
			this.labelDigitalInputCANId8.Name = "labelDigitalInputCANId8";
			componentResourceManager.ApplyResources(this.labelDigitalInputDigital2, "labelDigitalInputDigital2");
			this.labelDigitalInputDigital2.Name = "labelDigitalInputDigital2";
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputOnChange8, "checkBoxDigitalInputOnChange8");
			this.checkBoxDigitalInputOnChange8.Name = "checkBoxDigitalInputOnChange8";
			this.checkBoxDigitalInputOnChange8.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputOnChange8.CheckedChanged += new EventHandler(this.checkBoxDigitalInputOnChange_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelDigitalInputCANId7, "labelDigitalInputCANId7");
			this.labelDigitalInputCANId7.Name = "labelDigitalInputCANId7";
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputOnChange2, "checkBoxDigitalInputOnChange2");
			this.checkBoxDigitalInputOnChange2.Name = "checkBoxDigitalInputOnChange2";
			this.checkBoxDigitalInputOnChange2.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputOnChange2.CheckedChanged += new EventHandler(this.checkBoxDigitalInputOnChange_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputOnChange1, "checkBoxDigitalInputOnChange1");
			this.checkBoxDigitalInputOnChange1.Name = "checkBoxDigitalInputOnChange1";
			this.checkBoxDigitalInputOnChange1.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputOnChange1.CheckedChanged += new EventHandler(this.checkBoxDigitalInputOnChange_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelDigitalInputCANId6, "labelDigitalInputCANId6");
			this.labelDigitalInputCANId6.Name = "labelDigitalInputCANId6";
			componentResourceManager.ApplyResources(this.labelDigitalInputDigital5, "labelDigitalInputDigital5");
			this.labelDigitalInputDigital5.Name = "labelDigitalInputDigital5";
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputOnChange7, "checkBoxDigitalInputOnChange7");
			this.checkBoxDigitalInputOnChange7.Name = "checkBoxDigitalInputOnChange7";
			this.checkBoxDigitalInputOnChange7.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputOnChange7.CheckedChanged += new EventHandler(this.checkBoxDigitalInputOnChange_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelDigitalInputCANId5, "labelDigitalInputCANId5");
			this.labelDigitalInputCANId5.Name = "labelDigitalInputCANId5";
			componentResourceManager.ApplyResources(this.labelDigitalInputDigital4, "labelDigitalInputDigital4");
			this.labelDigitalInputDigital4.Name = "labelDigitalInputDigital4";
			componentResourceManager.ApplyResources(this.labelDigitalInputDigital7, "labelDigitalInputDigital7");
			this.labelDigitalInputDigital7.Name = "labelDigitalInputDigital7";
			componentResourceManager.ApplyResources(this.labelDigitalInputCANId4, "labelDigitalInputCANId4");
			this.labelDigitalInputCANId4.Name = "labelDigitalInputCANId4";
			componentResourceManager.ApplyResources(this.comboBoxDigitalInputFrequency8, "comboBoxDigitalInputFrequency8");
			this.comboBoxDigitalInputFrequency8.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDigitalInputFrequency8.FormattingEnabled = true;
			this.comboBoxDigitalInputFrequency8.Name = "comboBoxDigitalInputFrequency8";
			this.comboBoxDigitalInputFrequency8.SelectedIndexChanged += new EventHandler(this.comboBoxDigitalInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputOnChange6, "checkBoxDigitalInputOnChange6");
			this.checkBoxDigitalInputOnChange6.Name = "checkBoxDigitalInputOnChange6";
			this.checkBoxDigitalInputOnChange6.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputOnChange6.CheckedChanged += new EventHandler(this.checkBoxDigitalInputOnChange_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelDigitalInputCANId3, "labelDigitalInputCANId3");
			this.labelDigitalInputCANId3.Name = "labelDigitalInputCANId3";
			componentResourceManager.ApplyResources(this.labelDigitalInputDigital8, "labelDigitalInputDigital8");
			this.labelDigitalInputDigital8.Name = "labelDigitalInputDigital8";
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputFrequency8, "checkBoxDigitalInputFrequency8");
			this.checkBoxDigitalInputFrequency8.Name = "checkBoxDigitalInputFrequency8";
			this.checkBoxDigitalInputFrequency8.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputFrequency8.CheckedChanged += new EventHandler(this.checkBoxDigitalInputFrequency_CheckedChanged);
			this.checkBoxDigitalInputFrequency8.Paint += new PaintEventHandler(this.checkBoxDigitalInput_Paint);
			componentResourceManager.ApplyResources(this.labelDigitalInputCANId2, "labelDigitalInputCANId2");
			this.labelDigitalInputCANId2.Name = "labelDigitalInputCANId2";
			componentResourceManager.ApplyResources(this.labelDigitalInputDigital6, "labelDigitalInputDigital6");
			this.labelDigitalInputDigital6.Name = "labelDigitalInputDigital6";
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputOnChange5, "checkBoxDigitalInputOnChange5");
			this.checkBoxDigitalInputOnChange5.Name = "checkBoxDigitalInputOnChange5";
			this.checkBoxDigitalInputOnChange5.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputOnChange5.CheckedChanged += new EventHandler(this.checkBoxDigitalInputOnChange_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelDigitalInputCANId1, "labelDigitalInputCANId1");
			this.labelDigitalInputCANId1.Name = "labelDigitalInputCANId1";
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputFrequency1, "checkBoxDigitalInputFrequency1");
			this.checkBoxDigitalInputFrequency1.Name = "checkBoxDigitalInputFrequency1";
			this.checkBoxDigitalInputFrequency1.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputFrequency1.CheckedChanged += new EventHandler(this.checkBoxDigitalInputFrequency_CheckedChanged);
			this.checkBoxDigitalInputFrequency1.Paint += new PaintEventHandler(this.checkBoxDigitalInput_Paint);
			componentResourceManager.ApplyResources(this.comboBoxDigitalInputFrequency7, "comboBoxDigitalInputFrequency7");
			this.comboBoxDigitalInputFrequency7.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDigitalInputFrequency7.FormattingEnabled = true;
			this.comboBoxDigitalInputFrequency7.Name = "comboBoxDigitalInputFrequency7";
			this.comboBoxDigitalInputFrequency7.SelectedIndexChanged += new EventHandler(this.comboBoxDigitalInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputFrequency2, "checkBoxDigitalInputFrequency2");
			this.checkBoxDigitalInputFrequency2.Name = "checkBoxDigitalInputFrequency2";
			this.checkBoxDigitalInputFrequency2.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputFrequency2.CheckedChanged += new EventHandler(this.checkBoxDigitalInputFrequency_CheckedChanged);
			this.checkBoxDigitalInputFrequency2.Paint += new PaintEventHandler(this.checkBoxDigitalInput_Paint);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputOnChange4, "checkBoxDigitalInputOnChange4");
			this.checkBoxDigitalInputOnChange4.Name = "checkBoxDigitalInputOnChange4";
			this.checkBoxDigitalInputOnChange4.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputOnChange4.CheckedChanged += new EventHandler(this.checkBoxDigitalInputOnChange_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputFrequency3, "checkBoxDigitalInputFrequency3");
			this.checkBoxDigitalInputFrequency3.Name = "checkBoxDigitalInputFrequency3";
			this.checkBoxDigitalInputFrequency3.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputFrequency3.CheckedChanged += new EventHandler(this.checkBoxDigitalInputFrequency_CheckedChanged);
			this.checkBoxDigitalInputFrequency3.Paint += new PaintEventHandler(this.checkBoxDigitalInput_Paint);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputFrequency4, "checkBoxDigitalInputFrequency4");
			this.checkBoxDigitalInputFrequency4.Name = "checkBoxDigitalInputFrequency4";
			this.checkBoxDigitalInputFrequency4.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputFrequency4.CheckedChanged += new EventHandler(this.checkBoxDigitalInputFrequency_CheckedChanged);
			this.checkBoxDigitalInputFrequency4.Paint += new PaintEventHandler(this.checkBoxDigitalInput_Paint);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputFrequency7, "checkBoxDigitalInputFrequency7");
			this.checkBoxDigitalInputFrequency7.Name = "checkBoxDigitalInputFrequency7";
			this.checkBoxDigitalInputFrequency7.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputFrequency7.CheckedChanged += new EventHandler(this.checkBoxDigitalInputFrequency_CheckedChanged);
			this.checkBoxDigitalInputFrequency7.Paint += new PaintEventHandler(this.checkBoxDigitalInput_Paint);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputOnChange3, "checkBoxDigitalInputOnChange3");
			this.checkBoxDigitalInputOnChange3.Name = "checkBoxDigitalInputOnChange3";
			this.checkBoxDigitalInputOnChange3.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputOnChange3.CheckedChanged += new EventHandler(this.checkBoxDigitalInputOnChange_CheckedChanged);
			componentResourceManager.ApplyResources(this.comboBoxDigitalInputFrequency6, "comboBoxDigitalInputFrequency6");
			this.comboBoxDigitalInputFrequency6.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDigitalInputFrequency6.FormattingEnabled = true;
			this.comboBoxDigitalInputFrequency6.Name = "comboBoxDigitalInputFrequency6";
			this.comboBoxDigitalInputFrequency6.SelectedIndexChanged += new EventHandler(this.comboBoxDigitalInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputFrequency5, "checkBoxDigitalInputFrequency5");
			this.checkBoxDigitalInputFrequency5.Name = "checkBoxDigitalInputFrequency5";
			this.checkBoxDigitalInputFrequency5.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputFrequency5.CheckedChanged += new EventHandler(this.checkBoxDigitalInputFrequency_CheckedChanged);
			this.checkBoxDigitalInputFrequency5.Paint += new PaintEventHandler(this.checkBoxDigitalInput_Paint);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputFrequency6, "checkBoxDigitalInputFrequency6");
			this.checkBoxDigitalInputFrequency6.Name = "checkBoxDigitalInputFrequency6";
			this.checkBoxDigitalInputFrequency6.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputFrequency6.CheckedChanged += new EventHandler(this.checkBoxDigitalInputFrequency_CheckedChanged);
			this.checkBoxDigitalInputFrequency6.Paint += new PaintEventHandler(this.checkBoxDigitalInput_Paint);
			componentResourceManager.ApplyResources(this.comboBoxDigitalInputFrequency1, "comboBoxDigitalInputFrequency1");
			this.comboBoxDigitalInputFrequency1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDigitalInputFrequency1.FormattingEnabled = true;
			this.comboBoxDigitalInputFrequency1.Name = "comboBoxDigitalInputFrequency1";
			this.comboBoxDigitalInputFrequency1.SelectedIndexChanged += new EventHandler(this.comboBoxDigitalInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.comboBoxDigitalInputFrequency2, "comboBoxDigitalInputFrequency2");
			this.comboBoxDigitalInputFrequency2.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDigitalInputFrequency2.FormattingEnabled = true;
			this.comboBoxDigitalInputFrequency2.Name = "comboBoxDigitalInputFrequency2";
			this.comboBoxDigitalInputFrequency2.SelectedIndexChanged += new EventHandler(this.comboBoxDigitalInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.comboBoxDigitalInputFrequency5, "comboBoxDigitalInputFrequency5");
			this.comboBoxDigitalInputFrequency5.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDigitalInputFrequency5.FormattingEnabled = true;
			this.comboBoxDigitalInputFrequency5.Name = "comboBoxDigitalInputFrequency5";
			this.comboBoxDigitalInputFrequency5.SelectedIndexChanged += new EventHandler(this.comboBoxDigitalInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.comboBoxDigitalInputFrequency3, "comboBoxDigitalInputFrequency3");
			this.comboBoxDigitalInputFrequency3.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDigitalInputFrequency3.FormattingEnabled = true;
			this.comboBoxDigitalInputFrequency3.Name = "comboBoxDigitalInputFrequency3";
			this.comboBoxDigitalInputFrequency3.SelectedIndexChanged += new EventHandler(this.comboBoxDigitalInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.comboBoxDigitalInputFrequency4, "comboBoxDigitalInputFrequency4");
			this.comboBoxDigitalInputFrequency4.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDigitalInputFrequency4.FormattingEnabled = true;
			this.comboBoxDigitalInputFrequency4.Name = "comboBoxDigitalInputFrequency4";
			this.comboBoxDigitalInputFrequency4.SelectedIndexChanged += new EventHandler(this.comboBoxDigitalInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.radioButtonDigitalInputMappingCombined, "radioButtonDigitalInputMappingCombined");
			this.radioButtonDigitalInputMappingCombined.Name = "radioButtonDigitalInputMappingCombined";
			this.radioButtonDigitalInputMappingCombined.TabStop = true;
			this.radioButtonDigitalInputMappingCombined.UseVisualStyleBackColor = true;
			this.radioButtonDigitalInputMappingCombined.CheckedChanged += new EventHandler(this.radioButtonDigitalInputMappingMode_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonDigitalInputMappingSeparate, "radioButtonDigitalInputMappingSeparate");
			this.radioButtonDigitalInputMappingSeparate.Name = "radioButtonDigitalInputMappingSeparate";
			this.radioButtonDigitalInputMappingSeparate.TabStop = true;
			this.radioButtonDigitalInputMappingSeparate.UseVisualStyleBackColor = true;
			this.radioButtonDigitalInputMappingSeparate.CheckedChanged += new EventHandler(this.radioButtonDigitalInputMappingMode_CheckedChanged);
			this.groupBoxDigitalInputSendOnChannel.Controls.Add(this.comboBoxDigitalInputSendOnChannel);
			componentResourceManager.ApplyResources(this.groupBoxDigitalInputSendOnChannel, "groupBoxDigitalInputSendOnChannel");
			this.groupBoxDigitalInputSendOnChannel.Name = "groupBoxDigitalInputSendOnChannel";
			this.groupBoxDigitalInputSendOnChannel.TabStop = false;
			componentResourceManager.ApplyResources(this.radioButtonDigitalInputMappingIndividual, "radioButtonDigitalInputMappingIndividual");
			this.radioButtonDigitalInputMappingIndividual.Name = "radioButtonDigitalInputMappingIndividual";
			this.radioButtonDigitalInputMappingIndividual.TabStop = true;
			this.radioButtonDigitalInputMappingIndividual.UseVisualStyleBackColor = true;
			this.radioButtonDigitalInputMappingIndividual.CheckedChanged += new EventHandler(this.radioButtonDigitalInputMappingMode_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonDigitalInputMappingContinuous, "radioButtonDigitalInputMappingContinuous");
			this.radioButtonDigitalInputMappingContinuous.Name = "radioButtonDigitalInputMappingContinuous";
			this.radioButtonDigitalInputMappingContinuous.TabStop = true;
			this.radioButtonDigitalInputMappingContinuous.UseVisualStyleBackColor = true;
			this.radioButtonDigitalInputMappingContinuous.CheckedChanged += new EventHandler(this.radioButtonDigitalInputMappingMode_CheckedChanged);
			this.groupBoxDigitalInputMappingMode.Controls.Add(this.radioButtonDigitalInputMappingContinuous);
			this.groupBoxDigitalInputMappingMode.Controls.Add(this.radioButtonDigitalInputMappingIndividual);
			this.groupBoxDigitalInputMappingMode.Controls.Add(this.radioButtonDigitalInputMappingSeparate);
			this.groupBoxDigitalInputMappingMode.Controls.Add(this.radioButtonDigitalInputMappingCombined);
			componentResourceManager.ApplyResources(this.groupBoxDigitalInputMappingMode, "groupBoxDigitalInputMappingMode");
			this.groupBoxDigitalInputMappingMode.Name = "groupBoxDigitalInputMappingMode";
			this.groupBoxDigitalInputMappingMode.TabStop = false;
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxDigitalInput);
			base.Controls.Add(this.groupBoxDigitalInputSendOnChannel);
			base.Controls.Add(this.groupBoxDigitalInputMappingMode);
			base.Name = "DigitalInputsGL3Plus";
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			this.groupBoxDigitalInput.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.groupBoxDigitalInputSendOnChannel.ResumeLayout(false);
			this.groupBoxDigitalInputMappingMode.ResumeLayout(false);
			this.groupBoxDigitalInputMappingMode.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
