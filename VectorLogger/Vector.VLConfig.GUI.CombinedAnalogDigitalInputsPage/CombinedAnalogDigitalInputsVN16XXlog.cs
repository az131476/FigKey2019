using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.CombinedAnalogDigitalInputsPage
{
	public class CombinedAnalogDigitalInputsVN16XXlog : UserControl
	{
		private AnalogInputConfiguration analogInputConfiguration;

		private DigitalInputConfiguration digitalInputConfiguration;

		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isInitControls;

		private IContainer components;

		private GroupBox groupBoxAnalogDigitalInputs;

		private CheckBox checkBoxDigitalInput2;

		private CheckBox checkBoxDigitalInput1;

		private CheckBox checkBoxAnalogInput1;

		private ComboBox comboBoxSamplingFrequency;

		private Label labelSamplingFrequency;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		public DigitalInputConfiguration DigitalInputConfiguration
		{
			get
			{
				return this.digitalInputConfiguration;
			}
			set
			{
				this.digitalInputConfiguration = value;
				if (this.digitalInputConfiguration != null && this.analogInputConfiguration != null && this.ModelValidator != null)
				{
					this.UpdateGUI();
				}
			}
		}

		public AnalogInputConfiguration AnalogInputConfiguration
		{
			get
			{
				return this.analogInputConfiguration;
			}
			set
			{
				this.analogInputConfiguration = value;
				if (this.analogInputConfiguration != null && this.digitalInputConfiguration != null && this.ModelValidator != null)
				{
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

		public CombinedAnalogDigitalInputsVN16XXlog()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.isInitControls = true;
			this.InitSamplingFrequencyComboBox();
			this.isInitControls = false;
		}

		public void Init()
		{
		}

		private void InitSamplingFrequencyComboBox()
		{
			this.comboBoxSamplingFrequency.Items.Clear();
			IList<uint> standardAnalogInputFrequencies = GUIUtil.GetStandardAnalogInputFrequencies();
			foreach (uint current in standardAnalogInputFrequencies)
			{
				this.comboBoxSamplingFrequency.Items.Add(GUIUtil.MapAnalogInputsFrequency2String(current));
			}
			this.comboBoxSamplingFrequency.SelectedIndex = 0;
		}

		private void comboBoxSamplingFrequency_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		public bool ValidateInput()
		{
			bool flag = true;
			bool flag2 = false;
			bool flag3 = false;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			uint value = GUIUtil.MapString2AnalogInputsFrequency(this.comboBoxSamplingFrequency.SelectedItem.ToString());
			bool flag4;
			flag &= this.pageValidator.Control.UpdateModel<uint>(value, this.analogInputConfiguration.AnalogInputs[0].Frequency, this.guiElementManager.GetGUIElement(this.comboBoxSamplingFrequency), out flag4);
			flag2 |= flag4;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxAnalogInput1.Checked, this.analogInputConfiguration.AnalogInputs[0].IsActive, this.guiElementManager.GetGUIElement(this.checkBoxAnalogInput1), out flag4);
			flag2 |= flag4;
			flag &= this.pageValidator.Control.UpdateModel<uint>(value, this.digitalInputConfiguration.DigitalInputs[0].Frequency, this.guiElementManager.GetGUIElement(this.comboBoxSamplingFrequency), out flag4);
			flag3 |= flag4;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxDigitalInput1.Checked, this.digitalInputConfiguration.DigitalInputs[0].IsActiveFrequency, this.guiElementManager.GetGUIElement(this.checkBoxDigitalInput1), out flag4);
			flag3 |= flag4;
			flag &= this.pageValidator.Control.UpdateModel<uint>(value, this.digitalInputConfiguration.DigitalInputs[1].Frequency, this.guiElementManager.GetGUIElement(this.comboBoxSamplingFrequency), out flag4);
			flag3 |= flag4;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxDigitalInput2.Checked, this.digitalInputConfiguration.DigitalInputs[1].IsActiveFrequency, this.guiElementManager.GetGUIElement(this.checkBoxDigitalInput2), out flag4);
			flag3 |= flag4;
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			this.ModelValidator.Validate(this.analogInputConfiguration, flag2, this.pageValidator);
			this.ModelValidator.Validate(this.digitalInputConfiguration, flag3, this.pageValidator);
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
			this.isInitControls = true;
			this.comboBoxSamplingFrequency.SelectedItem = GUIUtil.MapAnalogInputsFrequency2String(this.analogInputConfiguration.AnalogInputs[0].Frequency.Value);
			this.checkBoxAnalogInput1.Checked = this.analogInputConfiguration.AnalogInputs[0].IsActive.Value;
			this.checkBoxDigitalInput1.Checked = this.digitalInputConfiguration.DigitalInputs[0].IsActiveFrequency.Value;
			this.checkBoxDigitalInput2.Checked = this.digitalInputConfiguration.DigitalInputs[1].IsActiveFrequency.Value;
			this.isInitControls = false;
			this.ValidateInput();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CombinedAnalogDigitalInputsVN16XXlog));
			this.groupBoxAnalogDigitalInputs = new GroupBox();
			this.checkBoxDigitalInput2 = new CheckBox();
			this.checkBoxDigitalInput1 = new CheckBox();
			this.checkBoxAnalogInput1 = new CheckBox();
			this.comboBoxSamplingFrequency = new ComboBox();
			this.labelSamplingFrequency = new Label();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.groupBoxAnalogDigitalInputs.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.groupBoxAnalogDigitalInputs.Controls.Add(this.checkBoxDigitalInput2);
			this.groupBoxAnalogDigitalInputs.Controls.Add(this.checkBoxDigitalInput1);
			this.groupBoxAnalogDigitalInputs.Controls.Add(this.checkBoxAnalogInput1);
			this.groupBoxAnalogDigitalInputs.Controls.Add(this.comboBoxSamplingFrequency);
			this.groupBoxAnalogDigitalInputs.Controls.Add(this.labelSamplingFrequency);
			componentResourceManager.ApplyResources(this.groupBoxAnalogDigitalInputs, "groupBoxAnalogDigitalInputs");
			this.groupBoxAnalogDigitalInputs.Name = "groupBoxAnalogDigitalInputs";
			this.groupBoxAnalogDigitalInputs.TabStop = false;
			componentResourceManager.ApplyResources(this.checkBoxDigitalInput2, "checkBoxDigitalInput2");
			this.checkBoxDigitalInput2.Name = "checkBoxDigitalInput2";
			this.checkBoxDigitalInput2.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInput2.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxDigitalInput1, "checkBoxDigitalInput1");
			this.checkBoxDigitalInput1.Name = "checkBoxDigitalInput1";
			this.checkBoxDigitalInput1.UseVisualStyleBackColor = true;
			this.checkBoxDigitalInput1.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAnalogInput1, "checkBoxAnalogInput1");
			this.checkBoxAnalogInput1.Name = "checkBoxAnalogInput1";
			this.checkBoxAnalogInput1.UseVisualStyleBackColor = true;
			this.checkBoxAnalogInput1.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			this.comboBoxSamplingFrequency.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxSamplingFrequency.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxSamplingFrequency, "comboBoxSamplingFrequency");
			this.comboBoxSamplingFrequency.Name = "comboBoxSamplingFrequency";
			this.comboBoxSamplingFrequency.SelectedIndexChanged += new EventHandler(this.comboBoxSamplingFrequency_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelSamplingFrequency, "labelSamplingFrequency");
			this.labelSamplingFrequency.Name = "labelSamplingFrequency";
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxAnalogDigitalInputs);
			base.Name = "CombinedAnalogDigitalInputsVN16XXlog";
			this.groupBoxAnalogDigitalInputs.ResumeLayout(false);
			this.groupBoxAnalogDigitalInputs.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
