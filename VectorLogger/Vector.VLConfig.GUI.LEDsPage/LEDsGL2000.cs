using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.LEDsPage
{
	internal class LEDsGL2000 : UserControl, IValidationHost
	{
		private Dictionary<uint, LEDRow> mLedNrToRowForm;

		private LEDConfiguration mLedConfiguration;

		private GUIElementManager_Control mGuiElementManager;

		private CustomErrorProvider mCustomErrorProvider;

		private PageValidator mPageValidator;

		private IModelValidator mModelValidator;

		private IContainer components;

		private GroupBox groupBoxLEDsOnDevice;

		private TableLayoutPanel tableLayoutPanelChannels;

		private ErrorProvider mErrorProvider;

		private ErrorProvider mErrorProviderLocalModel;

		private ErrorProvider mErrorProviderGlobalModel;

		private GroupBox groupBoxRemoteControl;

		private TableLayoutPanel tableLayoutPanelRemControl;

		private LEDRow mLedRow1;

		private LEDRow mLedRow2;

		private LEDRow mLedRow3;

		private LEDRow mLedRow4;

		private LEDRow mLedRow6;

		private LEDRow mLedRow7;

		private LEDRow mLedRow8;

		private LEDRow mLedRow9;

		private LEDRow mLedRow10;

		private LEDRow mLedRow11;

		private Label mLabelAddNamesDesc;

		public LEDConfiguration LEDConfiguration
		{
			get
			{
				return this.mLedConfiguration;
			}
			set
			{
				this.mLedConfiguration = value;
				if (this.mLedConfiguration != null)
				{
					if (this.ModelValidator != null)
					{
						uint arg_3E_0 = this.ModelValidator.LoggerSpecifics.IO.NumberOfLEDsTotal;
						uint arg_3D_0 = this.mLedConfiguration.NumberOfLEDs;
					}
					this.InitLedNrToRowForm();
					this.UpdateGUI();
				}
			}
		}

		public IModelValidator ModelValidator
		{
			get
			{
				return this.mModelValidator;
			}
			set
			{
				this.mModelValidator = value;
			}
		}

		public DisplayMode DisplayMode
		{
			get;
			set;
		}

		GUIElementManager_Control IValidationHost.GUIElementManager
		{
			get
			{
				return this.mGuiElementManager;
			}
		}

		PageValidator IValidationHost.PageValidator
		{
			get
			{
				return this.mPageValidator;
			}
		}

		public LEDsGL2000()
		{
			this.InitializeComponent();
			this.mGuiElementManager = new GUIElementManager_Control();
			this.mCustomErrorProvider = new CustomErrorProvider(this.mErrorProvider);
			this.mCustomErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.mErrorProviderLocalModel);
			this.mCustomErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.mErrorProviderGlobalModel);
			this.mPageValidator = new PageValidator(this.mCustomErrorProvider);
		}

		public void Init()
		{
		}

		private void InitLedNrToRowForm()
		{
			this.mLedNrToRowForm = new Dictionary<uint, LEDRow>();
			this.mLedRow1.Init(1u, 0u, this.mModelValidator);
			this.mLedNrToRowForm.Add(1u, this.mLedRow1);
			this.mLedRow2.Init(2u, 0u, this.mModelValidator);
			this.mLedNrToRowForm.Add(2u, this.mLedRow2);
			this.mLedRow3.Init(3u, 0u, this.mModelValidator);
			this.mLedNrToRowForm.Add(3u, this.mLedRow3);
			this.mLedRow4.Init(4u, 0u, this.mModelValidator);
			this.mLedNrToRowForm.Add(4u, this.mLedRow4);
			this.mLedRow6.Init(6u, 1u, this.mModelValidator);
			this.mLedNrToRowForm.Add(6u, this.mLedRow6);
			this.mLedRow7.Init(7u, 1u, this.mModelValidator);
			this.mLedNrToRowForm.Add(7u, this.mLedRow7);
			this.mLedRow8.Init(8u, 2u, this.mModelValidator);
			this.mLedNrToRowForm.Add(8u, this.mLedRow8);
			this.mLedRow9.Init(9u, 2u, this.mModelValidator);
			this.mLedNrToRowForm.Add(9u, this.mLedRow9);
			this.mLedRow10.Init(10u, 2u, this.mModelValidator);
			this.mLedNrToRowForm.Add(10u, this.mLedRow10);
			this.mLedRow11.Init(11u, 3u, this.mModelValidator);
			this.mLedNrToRowForm.Add(11u, this.mLedRow11);
			foreach (uint current in this.mLedNrToRowForm.Keys)
			{
				this.mLedNrToRowForm[current].ModelValidator = this.ModelValidator;
				((IValidatable)this.mLedNrToRowForm[current]).ValidationHost = this;
			}
		}

		public bool ValidateInput()
		{
			if (this.mLedConfiguration == null)
			{
				return false;
			}
			bool flag = true;
			bool flag2 = false;
			this.ResetValidationFramework();
			this.mPageValidator.General.ResetAllErrorProviders();
			this.mPageValidator.General.ResetAllFormatErrors();
			foreach (uint current in this.mLedNrToRowForm.Keys)
			{
				bool flag3 = false;
				flag &= ((IValidatable)this.mLedNrToRowForm[current]).ValidateInput(ref flag3);
				flag2 |= flag3;
			}
			flag &= this.ModelValidator.Validate(this.mLedConfiguration, flag2, this.mPageValidator);
			this.mPageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
		}

		void IValidationHost.RegisterForErrorProvider(Control control)
		{
			this.mErrorProviderLocalModel.SetIconAlignment(control, ErrorIconAlignment.MiddleLeft);
			this.mErrorProviderGlobalModel.SetIconAlignment(control, ErrorIconAlignment.MiddleLeft);
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		public bool HasErrors()
		{
			return this.mPageValidator.General.HasErrors(Enum.GetValues(typeof(ValidationErrorClass)) as ValidationErrorClass[]);
		}

		public bool HasGlobalErrors()
		{
			return this.mPageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.GlobalModelError
			});
		}

		public bool HasLocalErrors()
		{
			return this.mPageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.FormatError,
				ValidationErrorClass.LocalModelError
			});
		}

		public bool HasFormatErrors()
		{
			IPageValidatorGeneral arg_13_0 = this.mPageValidator.General;
			ValidationErrorClass[] errorClasses = new ValidationErrorClass[1];
			return arg_13_0.HasErrors(errorClasses);
		}

		private void ResetValidationFramework()
		{
			this.mPageValidator.General.Reset();
			this.mGuiElementManager.Reset();
		}

		private void UpdateGUI()
		{
			if (this.mLedConfiguration == null)
			{
				return;
			}
			List<uint> list = this.mLedNrToRowForm.Keys.ToList<uint>();
			for (int i = 0; i < this.mLedConfiguration.LEDConfigList.Count; i++)
			{
				this.mLedNrToRowForm[list[i]].ConfigItem = this.mLedConfiguration.LEDConfigList[i];
			}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LEDsGL2000));
			this.groupBoxLEDsOnDevice = new GroupBox();
			this.tableLayoutPanelChannels = new TableLayoutPanel();
			this.mLedRow1 = new LEDRow();
			this.mLedRow2 = new LEDRow();
			this.mLedRow3 = new LEDRow();
			this.mLedRow4 = new LEDRow();
			this.mErrorProvider = new ErrorProvider(this.components);
			this.groupBoxRemoteControl = new GroupBox();
			this.tableLayoutPanelRemControl = new TableLayoutPanel();
			this.mLedRow6 = new LEDRow();
			this.mLedRow7 = new LEDRow();
			this.mLedRow8 = new LEDRow();
			this.mLedRow9 = new LEDRow();
			this.mLedRow10 = new LEDRow();
			this.mLedRow11 = new LEDRow();
			this.mLabelAddNamesDesc = new Label();
			this.mErrorProviderLocalModel = new ErrorProvider(this.components);
			this.mErrorProviderGlobalModel = new ErrorProvider(this.components);
			this.groupBoxLEDsOnDevice.SuspendLayout();
			this.tableLayoutPanelChannels.SuspendLayout();
			((ISupportInitialize)this.mErrorProvider).BeginInit();
			this.groupBoxRemoteControl.SuspendLayout();
			this.tableLayoutPanelRemControl.SuspendLayout();
			((ISupportInitialize)this.mErrorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.mErrorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.groupBoxLEDsOnDevice.Controls.Add(this.tableLayoutPanelChannels);
			componentResourceManager.ApplyResources(this.groupBoxLEDsOnDevice, "groupBoxLEDsOnDevice");
			this.groupBoxLEDsOnDevice.Name = "groupBoxLEDsOnDevice";
			this.groupBoxLEDsOnDevice.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanelChannels, "tableLayoutPanelChannels");
			this.tableLayoutPanelChannels.Controls.Add(this.mLedRow1, 0, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.mLedRow2, 0, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.mLedRow3, 0, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.mLedRow4, 0, 3);
			this.tableLayoutPanelChannels.Name = "tableLayoutPanelChannels";
			componentResourceManager.ApplyResources(this.mLedRow1, "mLedRow1");
			this.mLedRow1.ConfigItem = null;
			this.mLedRow1.ModelValidator = null;
			this.mLedRow1.Name = "mLedRow1";
			componentResourceManager.ApplyResources(this.mLedRow2, "mLedRow2");
			this.mLedRow2.ConfigItem = null;
			this.mLedRow2.ModelValidator = null;
			this.mLedRow2.Name = "mLedRow2";
			componentResourceManager.ApplyResources(this.mLedRow3, "mLedRow3");
			this.mLedRow3.ConfigItem = null;
			this.mLedRow3.ModelValidator = null;
			this.mLedRow3.Name = "mLedRow3";
			componentResourceManager.ApplyResources(this.mLedRow4, "mLedRow4");
			this.mLedRow4.ConfigItem = null;
			this.mLedRow4.ModelValidator = null;
			this.mLedRow4.Name = "mLedRow4";
			this.mErrorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProvider.ContainerControl = this;
			this.groupBoxRemoteControl.Controls.Add(this.tableLayoutPanelRemControl);
			componentResourceManager.ApplyResources(this.groupBoxRemoteControl, "groupBoxRemoteControl");
			this.groupBoxRemoteControl.Name = "groupBoxRemoteControl";
			this.groupBoxRemoteControl.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanelRemControl, "tableLayoutPanelRemControl");
			this.tableLayoutPanelRemControl.Controls.Add(this.mLedRow6, 0, 0);
			this.tableLayoutPanelRemControl.Controls.Add(this.mLedRow7, 0, 1);
			this.tableLayoutPanelRemControl.Controls.Add(this.mLedRow8, 0, 2);
			this.tableLayoutPanelRemControl.Controls.Add(this.mLedRow9, 0, 3);
			this.tableLayoutPanelRemControl.Controls.Add(this.mLedRow10, 0, 4);
			this.tableLayoutPanelRemControl.Controls.Add(this.mLedRow11, 0, 5);
			this.tableLayoutPanelRemControl.Controls.Add(this.mLabelAddNamesDesc, 0, 6);
			this.tableLayoutPanelRemControl.Name = "tableLayoutPanelRemControl";
			componentResourceManager.ApplyResources(this.mLedRow6, "mLedRow6");
			this.mLedRow6.ConfigItem = null;
			this.mLedRow6.ModelValidator = null;
			this.mLedRow6.Name = "mLedRow6";
			componentResourceManager.ApplyResources(this.mLedRow7, "mLedRow7");
			this.mLedRow7.ConfigItem = null;
			this.mLedRow7.ModelValidator = null;
			this.mLedRow7.Name = "mLedRow7";
			componentResourceManager.ApplyResources(this.mLedRow8, "mLedRow8");
			this.mLedRow8.ConfigItem = null;
			this.mLedRow8.ModelValidator = null;
			this.mLedRow8.Name = "mLedRow8";
			componentResourceManager.ApplyResources(this.mLedRow9, "mLedRow9");
			this.mLedRow9.ConfigItem = null;
			this.mLedRow9.ModelValidator = null;
			this.mLedRow9.Name = "mLedRow9";
			componentResourceManager.ApplyResources(this.mLedRow10, "mLedRow10");
			this.mLedRow10.ConfigItem = null;
			this.mLedRow10.ModelValidator = null;
			this.mLedRow10.Name = "mLedRow10";
			componentResourceManager.ApplyResources(this.mLedRow11, "mLedRow11");
			this.mLedRow11.ConfigItem = null;
			this.mLedRow11.ModelValidator = null;
			this.mLedRow11.Name = "mLedRow11";
			componentResourceManager.ApplyResources(this.mLabelAddNamesDesc, "mLabelAddNamesDesc");
			this.mLabelAddNamesDesc.Name = "mLabelAddNamesDesc";
			this.mErrorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProviderLocalModel.ContainerControl = this;
			this.mErrorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.mErrorProviderGlobalModel, "mErrorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxRemoteControl);
			base.Controls.Add(this.groupBoxLEDsOnDevice);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "LEDsGL2000";
			this.groupBoxLEDsOnDevice.ResumeLayout(false);
			this.tableLayoutPanelChannels.ResumeLayout(false);
			((ISupportInitialize)this.mErrorProvider).EndInit();
			this.groupBoxRemoteControl.ResumeLayout(false);
			this.tableLayoutPanelRemControl.ResumeLayout(false);
			this.tableLayoutPanelRemControl.PerformLayout();
			((ISupportInitialize)this.mErrorProviderLocalModel).EndInit();
			((ISupportInitialize)this.mErrorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
