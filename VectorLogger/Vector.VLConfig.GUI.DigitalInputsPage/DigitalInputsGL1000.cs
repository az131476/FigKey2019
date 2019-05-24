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
	internal class DigitalInputsGL1000 : UserControl
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

		private GroupBox groupBoxDigitalInputSendOnChannel;

		private ComboBox comboBoxDigitalInputSendOnChannel;

		private GroupBox groupBoxDigitalInputMappingMode;

		private RadioButton radioButtonDigitalInputMappingContinuous;

		private RadioButton radioButtonDigitalInputMappingIndividual;

		private RadioButton radioButtonDigitalInputMappingSeparate;

		private RadioButton radioButtonDigitalInputMappingCombined;

		private GroupBox groupBoxDigitalInput;

		private TextBox textBoxDigitalInputCANID2;

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

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

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

		public DigitalInputsGL1000()
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
			this.inputMapComboBoxFrequency = new Dictionary<uint, ComboBox>();
			this.inputMapComboBoxFrequency.Add(1u, this.comboBoxDigitalInputFrequency1);
			this.inputMapComboBoxFrequency.Add(2u, this.comboBoxDigitalInputFrequency2);
			this.inputMapCheckBoxOnChange = new Dictionary<uint, CheckBox>();
			this.inputMapCheckBoxOnChange.Add(1u, this.checkBoxDigitalInputOnChange1);
			this.inputMapCheckBoxOnChange.Add(2u, this.checkBoxDigitalInputOnChange2);
			this.inputMapTextBoxCANId = new Dictionary<uint, TextBox>();
			this.inputMapTextBoxCANId.Add(1u, this.textBoxDigitalInputCANID1);
			this.inputMapTextBoxCANId.Add(2u, this.textBoxDigitalInputCANID2);
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
				this.inputMapTextBoxCANId[key].Enabled = this.inputMapCheckBoxFrequency[key].Checked;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DigitalInputsGL1000));
			this.groupBoxDigitalInputSendOnChannel = new GroupBox();
			this.comboBoxDigitalInputSendOnChannel = new ComboBox();
			this.groupBoxDigitalInputMappingMode = new GroupBox();
			this.radioButtonDigitalInputMappingContinuous = new RadioButton();
			this.radioButtonDigitalInputMappingIndividual = new RadioButton();
			this.radioButtonDigitalInputMappingSeparate = new RadioButton();
			this.radioButtonDigitalInputMappingCombined = new RadioButton();
			this.groupBoxDigitalInput = new GroupBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.labelDigitalInputDigital1 = new Label();
			this.textBoxDigitalInputCANID2 = new TextBox();
			this.labelDigitalInputDigital2 = new Label();
			this.textBoxDigitalInputCANID1 = new TextBox();
			this.labelDigitalInputCANId2 = new Label();
			this.checkBoxDigitalInputFrequency1 = new CheckBox();
			this.checkBoxDigitalInputFrequency2 = new CheckBox();
			this.labelDigitalInputCANId1 = new Label();
			this.comboBoxDigitalInputFrequency1 = new ComboBox();
			this.checkBoxDigitalInputOnChange2 = new CheckBox();
			this.comboBoxDigitalInputFrequency2 = new ComboBox();
			this.checkBoxDigitalInputOnChange1 = new CheckBox();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.groupBoxDigitalInputSendOnChannel.SuspendLayout();
			this.groupBoxDigitalInputMappingMode.SuspendLayout();
			this.groupBoxDigitalInput.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.groupBoxDigitalInputSendOnChannel.Controls.Add(this.comboBoxDigitalInputSendOnChannel);
			componentResourceManager.ApplyResources(this.groupBoxDigitalInputSendOnChannel, "groupBoxDigitalInputSendOnChannel");
			this.groupBoxDigitalInputSendOnChannel.Name = "groupBoxDigitalInputSendOnChannel";
			this.groupBoxDigitalInputSendOnChannel.TabStop = false;
			componentResourceManager.ApplyResources(this.comboBoxDigitalInputSendOnChannel, "comboBoxDigitalInputSendOnChannel");
			this.comboBoxDigitalInputSendOnChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDigitalInputSendOnChannel.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxDigitalInputSendOnChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDigitalInputSendOnChannel.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxDigitalInputSendOnChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDigitalInputSendOnChannel.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxDigitalInputSendOnChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDigitalInputSendOnChannel.IconAlignment2"));
			this.comboBoxDigitalInputSendOnChannel.Name = "comboBoxDigitalInputSendOnChannel";
			this.comboBoxDigitalInputSendOnChannel.SelectedIndexChanged += new EventHandler(this.comboBoxDigitalInputSendOnChannel_SelectedIndexChanged);
			this.groupBoxDigitalInputMappingMode.Controls.Add(this.radioButtonDigitalInputMappingContinuous);
			this.groupBoxDigitalInputMappingMode.Controls.Add(this.radioButtonDigitalInputMappingIndividual);
			this.groupBoxDigitalInputMappingMode.Controls.Add(this.radioButtonDigitalInputMappingSeparate);
			this.groupBoxDigitalInputMappingMode.Controls.Add(this.radioButtonDigitalInputMappingCombined);
			componentResourceManager.ApplyResources(this.groupBoxDigitalInputMappingMode, "groupBoxDigitalInputMappingMode");
			this.groupBoxDigitalInputMappingMode.Name = "groupBoxDigitalInputMappingMode";
			this.groupBoxDigitalInputMappingMode.TabStop = false;
			componentResourceManager.ApplyResources(this.radioButtonDigitalInputMappingContinuous, "radioButtonDigitalInputMappingContinuous");
			this.radioButtonDigitalInputMappingContinuous.Name = "radioButtonDigitalInputMappingContinuous";
			this.radioButtonDigitalInputMappingContinuous.TabStop = true;
			this.radioButtonDigitalInputMappingContinuous.UseVisualStyleBackColor = true;
			this.radioButtonDigitalInputMappingContinuous.CheckedChanged += new EventHandler(this.radioButtonDigitalInputMappingMode_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonDigitalInputMappingIndividual, "radioButtonDigitalInputMappingIndividual");
			this.radioButtonDigitalInputMappingIndividual.Name = "radioButtonDigitalInputMappingIndividual";
			this.radioButtonDigitalInputMappingIndividual.TabStop = true;
			this.radioButtonDigitalInputMappingIndividual.UseVisualStyleBackColor = true;
			this.radioButtonDigitalInputMappingIndividual.CheckedChanged += new EventHandler(this.radioButtonDigitalInputMappingMode_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonDigitalInputMappingSeparate, "radioButtonDigitalInputMappingSeparate");
			this.radioButtonDigitalInputMappingSeparate.Name = "radioButtonDigitalInputMappingSeparate";
			this.radioButtonDigitalInputMappingSeparate.TabStop = true;
			this.radioButtonDigitalInputMappingSeparate.UseVisualStyleBackColor = true;
			this.radioButtonDigitalInputMappingSeparate.CheckedChanged += new EventHandler(this.radioButtonDigitalInputMappingMode_CheckedChanged);
			componentResourceManager.ApplyResources(this.radioButtonDigitalInputMappingCombined, "radioButtonDigitalInputMappingCombined");
			this.radioButtonDigitalInputMappingCombined.Name = "radioButtonDigitalInputMappingCombined";
			this.radioButtonDigitalInputMappingCombined.TabStop = true;
			this.radioButtonDigitalInputMappingCombined.UseVisualStyleBackColor = true;
			this.radioButtonDigitalInputMappingCombined.CheckedChanged += new EventHandler(this.radioButtonDigitalInputMappingMode_CheckedChanged);
			this.groupBoxDigitalInput.Controls.Add(this.tableLayoutPanel1);
			componentResourceManager.ApplyResources(this.groupBoxDigitalInput, "groupBoxDigitalInput");
			this.groupBoxDigitalInput.Name = "groupBoxDigitalInput";
			this.groupBoxDigitalInput.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputDigital1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.textBoxDigitalInputCANID2, 5, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputDigital2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.textBoxDigitalInputCANID1, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputCANId2, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputFrequency1, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputFrequency2, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputCANId1, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.comboBoxDigitalInputFrequency1, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputOnChange2, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.comboBoxDigitalInputFrequency2, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDigitalInputOnChange1, 3, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.labelDigitalInputDigital1, "labelDigitalInputDigital1");
			this.labelDigitalInputDigital1.Name = "labelDigitalInputDigital1";
			componentResourceManager.ApplyResources(this.textBoxDigitalInputCANID2, "textBoxDigitalInputCANID2");
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDigitalInputCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID2.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDigitalInputCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID2.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDigitalInputCANID2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID2.IconAlignment2"));
			this.textBoxDigitalInputCANID2.Name = "textBoxDigitalInputCANID2";
			this.textBoxDigitalInputCANID2.Validating += new CancelEventHandler(this.textBoxDigitalInputCANID_Validating);
			componentResourceManager.ApplyResources(this.labelDigitalInputDigital2, "labelDigitalInputDigital2");
			this.labelDigitalInputDigital2.Name = "labelDigitalInputDigital2";
			componentResourceManager.ApplyResources(this.textBoxDigitalInputCANID1, "textBoxDigitalInputCANID1");
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDigitalInputCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID1.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDigitalInputCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID1.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDigitalInputCANID1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDigitalInputCANID1.IconAlignment2"));
			this.textBoxDigitalInputCANID1.Name = "textBoxDigitalInputCANID1";
			this.textBoxDigitalInputCANID1.Validating += new CancelEventHandler(this.textBoxDigitalInputCANID_Validating);
			componentResourceManager.ApplyResources(this.labelDigitalInputCANId2, "labelDigitalInputCANId2");
			this.labelDigitalInputCANId2.Name = "labelDigitalInputCANId2";
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputFrequency1, "checkBoxDigitalInputFrequency1");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxDigitalInputFrequency1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxDigitalInputFrequency1.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxDigitalInputFrequency1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxDigitalInputFrequency1.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxDigitalInputFrequency1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxDigitalInputFrequency1.IconAlignment2"));
			this.checkBoxDigitalInputFrequency1.Name = "checkBoxDigitalInputFrequency1";
			this.checkBoxDigitalInputFrequency1.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputFrequency1.CheckedChanged += new EventHandler(this.checkBoxDigitalInputFrequency_CheckedChanged);
			this.checkBoxDigitalInputFrequency1.Paint += new PaintEventHandler(this.checkBoxDigitalInput_Paint);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputFrequency2, "checkBoxDigitalInputFrequency2");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxDigitalInputFrequency2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxDigitalInputFrequency2.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxDigitalInputFrequency2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxDigitalInputFrequency2.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxDigitalInputFrequency2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxDigitalInputFrequency2.IconAlignment2"));
			this.checkBoxDigitalInputFrequency2.Name = "checkBoxDigitalInputFrequency2";
			this.checkBoxDigitalInputFrequency2.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputFrequency2.CheckedChanged += new EventHandler(this.checkBoxDigitalInputFrequency_CheckedChanged);
			this.checkBoxDigitalInputFrequency2.Paint += new PaintEventHandler(this.checkBoxDigitalInput_Paint);
			componentResourceManager.ApplyResources(this.labelDigitalInputCANId1, "labelDigitalInputCANId1");
			this.labelDigitalInputCANId1.Name = "labelDigitalInputCANId1";
			componentResourceManager.ApplyResources(this.comboBoxDigitalInputFrequency1, "comboBoxDigitalInputFrequency1");
			this.comboBoxDigitalInputFrequency1.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDigitalInputFrequency1.FormattingEnabled = true;
			this.comboBoxDigitalInputFrequency1.Name = "comboBoxDigitalInputFrequency1";
			this.comboBoxDigitalInputFrequency1.SelectedIndexChanged += new EventHandler(this.comboBoxDigitalInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputOnChange2, "checkBoxDigitalInputOnChange2");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxDigitalInputOnChange2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxDigitalInputOnChange2.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxDigitalInputOnChange2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxDigitalInputOnChange2.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxDigitalInputOnChange2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxDigitalInputOnChange2.IconAlignment2"));
			this.checkBoxDigitalInputOnChange2.Name = "checkBoxDigitalInputOnChange2";
			this.checkBoxDigitalInputOnChange2.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputOnChange2.CheckedChanged += new EventHandler(this.checkBoxDigitalInputOnChange_CheckedChanged);
			componentResourceManager.ApplyResources(this.comboBoxDigitalInputFrequency2, "comboBoxDigitalInputFrequency2");
			this.comboBoxDigitalInputFrequency2.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDigitalInputFrequency2.FormattingEnabled = true;
			this.comboBoxDigitalInputFrequency2.Name = "comboBoxDigitalInputFrequency2";
			this.comboBoxDigitalInputFrequency2.SelectedIndexChanged += new EventHandler(this.comboBoxDigitalInputFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInputOnChange1, "checkBoxDigitalInputOnChange1");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxDigitalInputOnChange1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxDigitalInputOnChange1.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxDigitalInputOnChange1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxDigitalInputOnChange1.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxDigitalInputOnChange1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxDigitalInputOnChange1.IconAlignment2"));
			this.checkBoxDigitalInputOnChange1.Name = "checkBoxDigitalInputOnChange1";
			this.checkBoxDigitalInputOnChange1.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInputOnChange1.CheckedChanged += new EventHandler(this.checkBoxDigitalInputOnChange_CheckedChanged);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxDigitalInputSendOnChannel);
			base.Controls.Add(this.groupBoxDigitalInputMappingMode);
			base.Controls.Add(this.groupBoxDigitalInput);
			base.Name = "DigitalInputsGL1000";
			componentResourceManager.ApplyResources(this, "$this");
			this.groupBoxDigitalInputSendOnChannel.ResumeLayout(false);
			this.groupBoxDigitalInputMappingMode.ResumeLayout(false);
			this.groupBoxDigitalInputMappingMode.PerformLayout();
			this.groupBoxDigitalInput.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
