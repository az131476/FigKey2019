using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.SpecialFeaturesPage
{
	public class SpecialFeaturesVN1630log : UserControl
	{
		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isInitControls;

		private SpecialFeaturesConfiguration specialFeaturesConfiguration;

		private DisplayMode displayMode;

		private IContainer components;

		private GroupBox groupBoxSpecialFeatures;

		private CheckBox checkBoxIsOverloadBuzzerActive;

		private Label labelMaxLogFileSize;

		private ComboBox comboBoxMaxLogFileSize;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private Label labelMB;

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
			}
		}

		public SpecialFeaturesVN1630log()
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
			this.InitMaxLogFileSizeComboBox();
			this.isInitControls = false;
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		private void InitMaxLogFileSizeComboBox()
		{
			this.comboBoxMaxLogFileSize.Items.Clear();
			foreach (uint current in GUIUtil.GetMaxLogFileSizes())
			{
				this.comboBoxMaxLogFileSize.Items.Add(current.ToString());
			}
			this.comboBoxMaxLogFileSize.SelectedIndex = this.comboBoxMaxLogFileSize.Items.Count - 1;
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
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxIsOverloadBuzzerActive.Checked, this.specialFeaturesConfiguration.IsOverloadBuzzerActive, this.guiElementManager.GetGUIElement(this.checkBoxIsOverloadBuzzerActive), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_UInt32(this.comboBoxMaxLogFileSize.Text, this.specialFeaturesConfiguration.MaxLogFileSize, this.guiElementManager.GetGUIElement(this.comboBoxMaxLogFileSize), out flag3);
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

		private void checkBoxIsOverloadBuzzerActive_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBoxMaxLogFileSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBoxMaxLogFileSize_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void UpdateGUI()
		{
			this.isInitControls = true;
			this.checkBoxIsOverloadBuzzerActive.Checked = this.specialFeaturesConfiguration.IsOverloadBuzzerActive.Value;
			if (this.comboBoxMaxLogFileSize.Items.Contains(this.specialFeaturesConfiguration.MaxLogFileSize.Value.ToString()))
			{
				this.comboBoxMaxLogFileSize.SelectedItem = this.specialFeaturesConfiguration.MaxLogFileSize.Value.ToString();
			}
			else
			{
				int selectedIndex = this.comboBoxMaxLogFileSize.Items.Add(this.specialFeaturesConfiguration.MaxLogFileSize.Value.ToString());
				this.comboBoxMaxLogFileSize.SelectedIndex = selectedIndex;
			}
			this.isInitControls = false;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SpecialFeaturesVN1630log));
			this.groupBoxSpecialFeatures = new GroupBox();
			this.comboBoxMaxLogFileSize = new ComboBox();
			this.labelMaxLogFileSize = new Label();
			this.checkBoxIsOverloadBuzzerActive = new CheckBox();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.labelMB = new Label();
			this.groupBoxSpecialFeatures.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.groupBoxSpecialFeatures.Controls.Add(this.labelMB);
			this.groupBoxSpecialFeatures.Controls.Add(this.comboBoxMaxLogFileSize);
			this.groupBoxSpecialFeatures.Controls.Add(this.labelMaxLogFileSize);
			this.groupBoxSpecialFeatures.Controls.Add(this.checkBoxIsOverloadBuzzerActive);
			componentResourceManager.ApplyResources(this.groupBoxSpecialFeatures, "groupBoxSpecialFeatures");
			this.groupBoxSpecialFeatures.Name = "groupBoxSpecialFeatures";
			this.groupBoxSpecialFeatures.TabStop = false;
			this.comboBoxMaxLogFileSize.FormattingEnabled = true;
			this.errorProviderFormat.SetIconAlignment(this.comboBoxMaxLogFileSize, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxMaxLogFileSize.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxMaxLogFileSize, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxMaxLogFileSize.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxMaxLogFileSize, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxMaxLogFileSize.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxMaxLogFileSize, "comboBoxMaxLogFileSize");
			this.comboBoxMaxLogFileSize.Name = "comboBoxMaxLogFileSize";
			this.comboBoxMaxLogFileSize.SelectedIndexChanged += new EventHandler(this.comboBoxMaxLogFileSize_SelectedIndexChanged);
			this.comboBoxMaxLogFileSize.Validating += new CancelEventHandler(this.comboBoxMaxLogFileSize_Validating);
			componentResourceManager.ApplyResources(this.labelMaxLogFileSize, "labelMaxLogFileSize");
			this.labelMaxLogFileSize.Name = "labelMaxLogFileSize";
			componentResourceManager.ApplyResources(this.checkBoxIsOverloadBuzzerActive, "checkBoxIsOverloadBuzzerActive");
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxIsOverloadBuzzerActive, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIsOverloadBuzzerActive.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxIsOverloadBuzzerActive, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIsOverloadBuzzerActive.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxIsOverloadBuzzerActive, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIsOverloadBuzzerActive.IconAlignment2"));
			this.checkBoxIsOverloadBuzzerActive.Name = "checkBoxIsOverloadBuzzerActive";
			this.checkBoxIsOverloadBuzzerActive.UseVisualStyleBackColor = true;
			this.checkBoxIsOverloadBuzzerActive.CheckedChanged += new EventHandler(this.checkBoxIsOverloadBuzzerActive_CheckedChanged);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this.labelMB, "labelMB");
			this.labelMB.Name = "labelMB";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxSpecialFeatures);
			base.Name = "SpecialFeaturesVN1630log";
			this.groupBoxSpecialFeatures.ResumeLayout(false);
			this.groupBoxSpecialFeatures.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
