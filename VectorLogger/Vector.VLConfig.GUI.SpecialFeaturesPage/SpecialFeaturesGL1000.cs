using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.SpecialFeaturesPage
{
	public class SpecialFeaturesGL1000 : UserControl
	{
		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isInitControls;

		private SpecialFeaturesConfiguration specialFeaturesConfiguration;

		private DisplayMode displayMode;

		private IContainer components;

		private CheckBox checkBoxLogDateTime;

		private ComboBox comboBoxLogDateTimeChannel;

		private Label labelInMessageId;

		private TextBox textBoxDateTimeMessageId;

		private CheckBox checkBoxIncludeLTLCode;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private GroupBox groupBoxSpecialFeatures;

		private CheckBox checkBoxIgnitionKeepsLoggerAwake;

		public SpecialFeaturesConfiguration SpecialFeaturesConfiguration
		{
			get
			{
				return this.specialFeaturesConfiguration;
			}
			set
			{
				this.specialFeaturesConfiguration = value;
				if (this.specialFeaturesConfiguration != null)
				{
					this.UpdateGUI(false);
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
				if (this.SpecialFeaturesConfiguration != null)
				{
					this.textBoxDateTimeMessageId.Text = GUIUtil.CANIdToDisplayString(this.SpecialFeaturesConfiguration.LogDateTimeCANId.Value, this.SpecialFeaturesConfiguration.LogDateTimeIsExtendedId.Value);
					this.ValidateInput();
				}
			}
		}

		public SpecialFeaturesGL1000()
		{
			this.InitializeComponent();
			this.isInitControls = false;
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
		}

		public void Init()
		{
			this.isInitControls = true;
			this.InitLogDateTimeChannels();
			this.isInitControls = false;
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		public void InitLogDateTimeChannels()
		{
			this.comboBoxLogDateTimeChannel.Items.Clear();
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels; num += 1u)
			{
				this.comboBoxLogDateTimeChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
			uint num2 = this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels + 1u;
			for (uint num3 = num2; num3 < this.ModelValidator.LoggerSpecifics.CAN.NumberOfVirtualChannels + num2; num3 += 1u)
			{
				this.comboBoxLogDateTimeChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num3) + Resources.VirtualChannelPostfix);
			}
			if (this.comboBoxLogDateTimeChannel.Items.Count > 0)
			{
				this.comboBoxLogDateTimeChannel.SelectedIndex = 0;
			}
		}

		public bool ValidateInput()
		{
			if (this.specialFeaturesConfiguration == null)
			{
				return false;
			}
			bool flag = true;
			bool flag2 = false;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			bool flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxLogDateTime.Checked, this.specialFeaturesConfiguration.IsLogDateTimeEnabled, this.guiElementManager.GetGUIElement(this.checkBoxLogDateTime), out flag3);
			flag2 |= flag3;
			if (this.specialFeaturesConfiguration.IsLogDateTimeEnabled.Value)
			{
				flag &= this.pageValidator.Control.UpdateModel<uint>(GUIUtil.MapCANChannelString2Number(this.comboBoxLogDateTimeChannel.SelectedItem.ToString()), this.specialFeaturesConfiguration.LogDateTimeChannel, this.guiElementManager.GetGUIElement(this.comboBoxLogDateTimeChannel), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.textBoxDateTimeMessageId.Text, this.specialFeaturesConfiguration.LogDateTimeCANId, this.specialFeaturesConfiguration.LogDateTimeIsExtendedId, this.guiElementManager.GetGUIElement(this.textBoxDateTimeMessageId), out flag3);
				flag2 |= flag3;
			}
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxIncludeLTLCode.Checked, this.specialFeaturesConfiguration.IsIncludeLTLCodeEnabled, this.guiElementManager.GetGUIElement(this.checkBoxIncludeLTLCode), out flag3);
			flag2 |= flag3;
			flag = this.pageValidator.Control.UpdateModel<bool>(this.checkBoxIgnitionKeepsLoggerAwake.Checked, this.specialFeaturesConfiguration.IsIgnitionKeepsLoggerAwakeEnabled, this.guiElementManager.GetGUIElement(this.checkBoxIgnitionKeepsLoggerAwake), out flag3);
			flag2 |= flag3;
			flag &= this.ModelValidator.Validate(this.specialFeaturesConfiguration, flag2, this.pageValidator);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
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

		private void checkBoxLogDateTime_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
			this.EnableLogDateTimeControls();
		}

		private void control_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
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

		private void UpdateGUI(bool overrideFormatErrors)
		{
			if (this.specialFeaturesConfiguration == null)
			{
				return;
			}
			this.isInitControls = true;
			if (overrideFormatErrors || !this.pageValidator.General.HasFormatError(this.SpecialFeaturesConfiguration.IsLogDateTimeEnabled))
			{
				this.checkBoxLogDateTime.Checked = this.SpecialFeaturesConfiguration.IsLogDateTimeEnabled.Value;
			}
			if (overrideFormatErrors || !this.pageValidator.General.HasFormatError(this.specialFeaturesConfiguration.LogDateTimeChannel))
			{
				if (this.specialFeaturesConfiguration.LogDateTimeChannel.Value <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels)
				{
					this.comboBoxLogDateTimeChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.specialFeaturesConfiguration.LogDateTimeChannel.Value);
				}
				else
				{
					this.comboBoxLogDateTimeChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.specialFeaturesConfiguration.LogDateTimeChannel.Value) + Resources.VirtualChannelPostfix;
				}
			}
			if (overrideFormatErrors || !this.pageValidator.General.HasFormatError(this.SpecialFeaturesConfiguration.LogDateTimeCANId))
			{
				this.textBoxDateTimeMessageId.Text = GUIUtil.CANIdToDisplayString(this.SpecialFeaturesConfiguration.LogDateTimeCANId.Value, this.SpecialFeaturesConfiguration.LogDateTimeIsExtendedId.Value);
			}
			if (overrideFormatErrors || !this.pageValidator.General.HasFormatError(this.SpecialFeaturesConfiguration.IsIncludeLTLCodeEnabled))
			{
				this.checkBoxIncludeLTLCode.Checked = this.SpecialFeaturesConfiguration.IsIncludeLTLCodeEnabled.Value;
			}
			if (overrideFormatErrors || !this.pageValidator.General.HasFormatError(this.SpecialFeaturesConfiguration.IsIgnitionKeepsLoggerAwakeEnabled))
			{
				this.checkBoxIgnitionKeepsLoggerAwake.Checked = this.SpecialFeaturesConfiguration.IsIgnitionKeepsLoggerAwakeEnabled.Value;
			}
			this.isInitControls = false;
			this.ValidateInput();
			this.EnableLogDateTimeControls();
		}

		private void EnableLogDateTimeControls()
		{
			this.comboBoxLogDateTimeChannel.Enabled = this.SpecialFeaturesConfiguration.IsLogDateTimeEnabled.Value;
			this.textBoxDateTimeMessageId.Enabled = this.SpecialFeaturesConfiguration.IsLogDateTimeEnabled.Value;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SpecialFeaturesGL1000));
			this.checkBoxLogDateTime = new CheckBox();
			this.comboBoxLogDateTimeChannel = new ComboBox();
			this.labelInMessageId = new Label();
			this.textBoxDateTimeMessageId = new TextBox();
			this.checkBoxIncludeLTLCode = new CheckBox();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.groupBoxSpecialFeatures = new GroupBox();
			this.checkBoxIgnitionKeepsLoggerAwake = new CheckBox();
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			this.groupBoxSpecialFeatures.SuspendLayout();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.checkBoxLogDateTime, "checkBoxLogDateTime");
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxLogDateTime, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogDateTime.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxLogDateTime, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogDateTime.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxLogDateTime, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxLogDateTime.IconAlignment2"));
			this.checkBoxLogDateTime.Name = "checkBoxLogDateTime";
			this.checkBoxLogDateTime.UseVisualStyleBackColor = true;
			this.checkBoxLogDateTime.CheckedChanged += new EventHandler(this.checkBoxLogDateTime_CheckedChanged);
			this.comboBoxLogDateTimeChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxLogDateTimeChannel.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxLogDateTimeChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxLogDateTimeChannel.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxLogDateTimeChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxLogDateTimeChannel.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxLogDateTimeChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxLogDateTimeChannel.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxLogDateTimeChannel, "comboBoxLogDateTimeChannel");
			this.comboBoxLogDateTimeChannel.Name = "comboBoxLogDateTimeChannel";
			this.comboBoxLogDateTimeChannel.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelInMessageId, "labelInMessageId");
			this.labelInMessageId.Name = "labelInMessageId";
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDateTimeMessageId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDateTimeMessageId.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDateTimeMessageId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDateTimeMessageId.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDateTimeMessageId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDateTimeMessageId.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxDateTimeMessageId, "textBoxDateTimeMessageId");
			this.textBoxDateTimeMessageId.Name = "textBoxDateTimeMessageId";
			this.textBoxDateTimeMessageId.Validating += new CancelEventHandler(this.control_Validating);
			componentResourceManager.ApplyResources(this.checkBoxIncludeLTLCode, "checkBoxIncludeLTLCode");
			this.checkBoxIncludeLTLCode.Checked = true;
			this.checkBoxIncludeLTLCode.CheckState = CheckState.Checked;
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxIncludeLTLCode, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIncludeLTLCode.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxIncludeLTLCode, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIncludeLTLCode.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxIncludeLTLCode, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIncludeLTLCode.IconAlignment2"));
			this.checkBoxIncludeLTLCode.Name = "checkBoxIncludeLTLCode";
			this.checkBoxIncludeLTLCode.UseVisualStyleBackColor = true;
			this.checkBoxIncludeLTLCode.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.groupBoxSpecialFeatures.Controls.Add(this.checkBoxIgnitionKeepsLoggerAwake);
			this.groupBoxSpecialFeatures.Controls.Add(this.checkBoxIncludeLTLCode);
			this.groupBoxSpecialFeatures.Controls.Add(this.textBoxDateTimeMessageId);
			this.groupBoxSpecialFeatures.Controls.Add(this.labelInMessageId);
			this.groupBoxSpecialFeatures.Controls.Add(this.comboBoxLogDateTimeChannel);
			this.groupBoxSpecialFeatures.Controls.Add(this.checkBoxLogDateTime);
			componentResourceManager.ApplyResources(this.groupBoxSpecialFeatures, "groupBoxSpecialFeatures");
			this.groupBoxSpecialFeatures.Name = "groupBoxSpecialFeatures";
			this.groupBoxSpecialFeatures.TabStop = false;
			componentResourceManager.ApplyResources(this.checkBoxIgnitionKeepsLoggerAwake, "checkBoxIgnitionKeepsLoggerAwake");
			this.checkBoxIgnitionKeepsLoggerAwake.Name = "checkBoxIgnitionKeepsLoggerAwake";
			this.checkBoxIgnitionKeepsLoggerAwake.UseVisualStyleBackColor = true;
			this.checkBoxIgnitionKeepsLoggerAwake.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxSpecialFeatures);
			componentResourceManager.ApplyResources(this, "$this");
			this.errorProviderFormat.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment1"));
			base.Name = "SpecialFeaturesGL1000";
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			this.groupBoxSpecialFeatures.ResumeLayout(false);
			this.groupBoxSpecialFeatures.PerformLayout();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
