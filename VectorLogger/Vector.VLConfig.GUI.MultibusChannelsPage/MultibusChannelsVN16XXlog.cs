using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.MultibusChannelsPage
{
	internal class MultibusChannelsVN16XXlog : UserControl, IValidationHost
	{
		private MultibusChannelConfiguration multibusChannelConfiguration;

		private IApplicationDatabaseManager applicationDatabaseManager;

		private IModelValidator modelValidator;

		private readonly GUIElementManager_Control guiElementManager;

		private readonly PageValidator pageValidator;

		private IContainer components;

		private GroupBox groupBoxChannels;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private MultibusChannel multibusChannel1;

		private MultibusChannel multibusChannel2;

		private MultibusChannel multibusChannel3;

		private MultibusChannel multibusChannel4;

		public MultibusChannelConfiguration MultibusChannelConfiguration
		{
			get
			{
				return this.multibusChannelConfiguration;
			}
			set
			{
				this.multibusChannelConfiguration = value;
				if (this.multibusChannelConfiguration != null && this.ModelValidator != null)
				{
					uint arg_3E_0 = this.ModelValidator.LoggerSpecifics.Multibus.NumberOfChannels;
					uint arg_3D_0 = this.multibusChannelConfiguration.NumberOfChannels;
				}
				this.multibusChannel1.MultibusChannelConfiguration = value;
				this.multibusChannel2.MultibusChannelConfiguration = value;
				this.multibusChannel3.MultibusChannelConfiguration = value;
				this.multibusChannel4.MultibusChannelConfiguration = value;
				this.ValidateInput();
			}
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.applicationDatabaseManager;
			}
			set
			{
				this.applicationDatabaseManager = value;
				this.multibusChannel1.ApplicationDatabaseManager = value;
				this.multibusChannel2.ApplicationDatabaseManager = value;
				this.multibusChannel3.ApplicationDatabaseManager = value;
				this.multibusChannel4.ApplicationDatabaseManager = value;
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
				this.multibusChannel1.ModelValidator = value;
				this.multibusChannel2.ModelValidator = value;
				this.multibusChannel3.ModelValidator = value;
				this.multibusChannel4.ModelValidator = value;
			}
		}

		public DisplayMode DisplayMode
		{
			get;
			set;
		}

		public IHardwareFrontend HardwareFrontend
		{
			get
			{
				return this.multibusChannel1.HardwareFrontend;
			}
			set
			{
				this.multibusChannel1.HardwareFrontend = value;
				this.multibusChannel2.HardwareFrontend = value;
				this.multibusChannel3.HardwareFrontend = value;
				this.multibusChannel4.HardwareFrontend = value;
			}
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

		public MultibusChannelsVN16XXlog()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_Control();
			CustomErrorProvider customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(customErrorProvider);
			this.InitChannelControls();
		}

		public void Init()
		{
		}

		private void InitChannelControls()
		{
			((IValidatable)this.multibusChannel1).ValidationHost = this;
			((IValidatable)this.multibusChannel2).ValidationHost = this;
			((IValidatable)this.multibusChannel3).ValidationHost = this;
			((IValidatable)this.multibusChannel4).ValidationHost = this;
			this.multibusChannel1.ChannelNr = 1u;
			this.multibusChannel2.ChannelNr = 2u;
			this.multibusChannel3.ChannelNr = 3u;
			this.multibusChannel4.ChannelNr = 4u;
			this.multibusChannel1.SetPossibleBusTypes(new List<BusType>
			{
				BusType.Bt_CAN,
				BusType.Bt_LIN
			});
			this.multibusChannel2.SetPossibleBusTypes(new List<BusType>
			{
				BusType.Bt_CAN,
				BusType.Bt_LIN
			});
			this.multibusChannel3.SetPossibleBusTypes(new List<BusType>
			{
				BusType.Bt_CAN
			});
			this.multibusChannel4.SetPossibleBusTypes(new List<BusType>
			{
				BusType.Bt_CAN
			});
			this.multibusChannel1.SupportsCANFD = true;
			this.multibusChannel2.SupportsCANFD = true;
			this.multibusChannel3.SupportsCANFD = true;
			this.multibusChannel4.SupportsCANFD = true;
		}

		public bool ValidateInput()
		{
			if (this.multibusChannelConfiguration == null)
			{
				return false;
			}
			bool flag = true;
			bool isDataChanged = false;
			this.ResetValidationFramework();
			flag &= ((IValidatable)this.multibusChannel1).ValidateInput(ref isDataChanged);
			flag &= ((IValidatable)this.multibusChannel2).ValidateInput(ref isDataChanged);
			flag &= ((IValidatable)this.multibusChannel3).ValidateInput(ref isDataChanged);
			flag &= ((IValidatable)this.multibusChannel4).ValidateInput(ref isDataChanged);
			flag &= this.ModelValidator.Validate(this.multibusChannelConfiguration, isDataChanged, this.pageValidator);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
		}

		void IValidationHost.RegisterForErrorProvider(Control control)
		{
			this.errorProviderFormat.SetIconAlignment(control, ErrorIconAlignment.MiddleLeft);
			this.errorProviderLocalModel.SetIconAlignment(control, ErrorIconAlignment.MiddleLeft);
			this.errorProviderGlobalModel.SetIconAlignment(control, ErrorIconAlignment.MiddleLeft);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MultibusChannelsVN16XXlog));
			this.groupBoxChannels = new GroupBox();
			this.multibusChannel1 = new MultibusChannel();
			this.multibusChannel2 = new MultibusChannel();
			this.multibusChannel4 = new MultibusChannel();
			this.multibusChannel3 = new MultibusChannel();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.groupBoxChannels.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.groupBoxChannels.Controls.Add(this.multibusChannel1);
			this.groupBoxChannels.Controls.Add(this.multibusChannel2);
			this.groupBoxChannels.Controls.Add(this.multibusChannel4);
			this.groupBoxChannels.Controls.Add(this.multibusChannel3);
			componentResourceManager.ApplyResources(this.groupBoxChannels, "groupBoxChannels");
			this.groupBoxChannels.Name = "groupBoxChannels";
			this.groupBoxChannels.TabStop = false;
			this.multibusChannel1.ApplicationDatabaseManager = null;
			this.multibusChannel1.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.multibusChannel1, "multibusChannel1");
			this.multibusChannel1.ModelValidator = null;
			this.multibusChannel1.MultibusChannelConfiguration = null;
			this.multibusChannel1.Name = "multibusChannel1";
			this.multibusChannel1.SupportsCANFD = false;
			this.multibusChannel2.ApplicationDatabaseManager = null;
			this.multibusChannel2.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.multibusChannel2, "multibusChannel2");
			this.multibusChannel2.ModelValidator = null;
			this.multibusChannel2.MultibusChannelConfiguration = null;
			this.multibusChannel2.Name = "multibusChannel2";
			this.multibusChannel2.SupportsCANFD = false;
			this.multibusChannel4.ApplicationDatabaseManager = null;
			this.multibusChannel4.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.multibusChannel4, "multibusChannel4");
			this.multibusChannel4.ModelValidator = null;
			this.multibusChannel4.MultibusChannelConfiguration = null;
			this.multibusChannel4.Name = "multibusChannel4";
			this.multibusChannel4.SupportsCANFD = false;
			this.multibusChannel3.ApplicationDatabaseManager = null;
			this.multibusChannel3.ChannelNr = 0u;
			componentResourceManager.ApplyResources(this.multibusChannel3, "multibusChannel3");
			this.multibusChannel3.ModelValidator = null;
			this.multibusChannel3.MultibusChannelConfiguration = null;
			this.multibusChannel3.Name = "multibusChannel3";
			this.multibusChannel3.SupportsCANFD = false;
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxChannels);
			base.Name = "MultibusChannelsVN16XXlog";
			this.groupBoxChannels.ResumeLayout(false);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
