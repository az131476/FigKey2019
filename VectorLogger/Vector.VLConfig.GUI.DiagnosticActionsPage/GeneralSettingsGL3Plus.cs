using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.DiagnosticActionsPage
{
	public class GeneralSettingsGL3Plus : UserControl
	{
		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private DiagnosticActionsConfiguration diagActionsConfig;

		private bool isInitControls;

		private DisplayMode displayMode;

		private IContainer components;

		private Label labelRecordDiagMsgs;

		private CheckBox checkBoxRecordDiagMsgsMemory1;

		private CheckBox checkBoxRecordDiagMsgsMemory2;

		private CheckBox checkBoxIsRestartDiagEnabled;

		private TextBox textBoxDiagCommRestartTime;

		private Label labelMin;

		private Label labelExternalTesterDesc;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		public DiagnosticActionsConfiguration DiagnosticActionsConfiguration
		{
			get
			{
				return this.diagActionsConfig;
			}
			set
			{
				this.diagActionsConfig = value;
				if (this.diagActionsConfig != null)
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
				this.UpdateGUI();
			}
		}

		public GeneralSettingsGL3Plus()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.FormatError, this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.isInitControls = false;
		}

		private void checkBoxRecordDiagMsgsMemory1_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxRecordDiagMsgsMemory2_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxIsRestartDiagEnabled_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			CheckBox checkBox = sender as CheckBox;
			if (checkBox == null)
			{
				return;
			}
			this.textBoxDiagCommRestartTime.Enabled = checkBox.Checked;
			this.ValidateInput();
		}

		private void textBoxDiagCommRestartTime_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		private void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
		}

		public bool ValidateInput()
		{
			bool flag = true;
			bool flag2 = false;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			bool flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxRecordDiagMsgsMemory1.Checked, this.diagActionsConfig.recordDiagCommOnMemories[0], this.guiElementManager.GetGUIElement(this.checkBoxRecordDiagMsgsMemory1), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxRecordDiagMsgsMemory2.Checked, this.diagActionsConfig.recordDiagCommOnMemories[1], this.guiElementManager.GetGUIElement(this.checkBoxRecordDiagMsgsMemory2), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxIsRestartDiagEnabled.Checked, this.diagActionsConfig.IsDiagCommRestartEnabled, this.guiElementManager.GetGUIElement(this.checkBoxIsRestartDiagEnabled), out flag3);
			flag2 |= flag3;
			if (this.diagActionsConfig.IsDiagCommRestartEnabled.Value)
			{
				flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_UInt32(this.textBoxDiagCommRestartTime.Text, this.diagActionsConfig.DiagCommRestartTime, this.guiElementManager.GetGUIElement(this.textBoxDiagCommRestartTime), out flag3);
				flag2 |= flag3;
			}
			flag &= this.ModelValidator.ValidateDiagnosticActionsGeneralSettings(this.diagActionsConfig, flag2, this.pageValidator);
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

		private void UpdateGUI()
		{
			if (this.diagActionsConfig == null)
			{
				return;
			}
			this.isInitControls = true;
			this.checkBoxRecordDiagMsgsMemory1.Checked = this.diagActionsConfig.recordDiagCommOnMemories[0].Value;
			this.checkBoxRecordDiagMsgsMemory2.Checked = this.diagActionsConfig.recordDiagCommOnMemories[1].Value;
			this.checkBoxIsRestartDiagEnabled.Checked = this.diagActionsConfig.IsDiagCommRestartEnabled.Value;
			this.textBoxDiagCommRestartTime.Enabled = this.checkBoxIsRestartDiagEnabled.Checked;
			this.textBoxDiagCommRestartTime.Text = this.diagActionsConfig.DiagCommRestartTime.Value.ToString();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GeneralSettingsGL3Plus));
			this.labelRecordDiagMsgs = new Label();
			this.checkBoxRecordDiagMsgsMemory1 = new CheckBox();
			this.checkBoxRecordDiagMsgsMemory2 = new CheckBox();
			this.checkBoxIsRestartDiagEnabled = new CheckBox();
			this.textBoxDiagCommRestartTime = new TextBox();
			this.labelMin = new Label();
			this.labelExternalTesterDesc = new Label();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelRecordDiagMsgs, "labelRecordDiagMsgs");
			this.errorProviderFormat.SetError(this.labelRecordDiagMsgs, componentResourceManager.GetString("labelRecordDiagMsgs.Error"));
			this.errorProviderGlobalModel.SetError(this.labelRecordDiagMsgs, componentResourceManager.GetString("labelRecordDiagMsgs.Error1"));
			this.errorProviderLocalModel.SetError(this.labelRecordDiagMsgs, componentResourceManager.GetString("labelRecordDiagMsgs.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelRecordDiagMsgs, (ErrorIconAlignment)componentResourceManager.GetObject("labelRecordDiagMsgs.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelRecordDiagMsgs, (ErrorIconAlignment)componentResourceManager.GetObject("labelRecordDiagMsgs.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelRecordDiagMsgs, (ErrorIconAlignment)componentResourceManager.GetObject("labelRecordDiagMsgs.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelRecordDiagMsgs, (int)componentResourceManager.GetObject("labelRecordDiagMsgs.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelRecordDiagMsgs, (int)componentResourceManager.GetObject("labelRecordDiagMsgs.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelRecordDiagMsgs, (int)componentResourceManager.GetObject("labelRecordDiagMsgs.IconPadding2"));
			this.labelRecordDiagMsgs.Name = "labelRecordDiagMsgs";
			componentResourceManager.ApplyResources(this.checkBoxRecordDiagMsgsMemory1, "checkBoxRecordDiagMsgsMemory1");
			this.errorProviderGlobalModel.SetError(this.checkBoxRecordDiagMsgsMemory1, componentResourceManager.GetString("checkBoxRecordDiagMsgsMemory1.Error"));
			this.errorProviderFormat.SetError(this.checkBoxRecordDiagMsgsMemory1, componentResourceManager.GetString("checkBoxRecordDiagMsgsMemory1.Error1"));
			this.errorProviderLocalModel.SetError(this.checkBoxRecordDiagMsgsMemory1, componentResourceManager.GetString("checkBoxRecordDiagMsgsMemory1.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxRecordDiagMsgsMemory1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxRecordDiagMsgsMemory1.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxRecordDiagMsgsMemory1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxRecordDiagMsgsMemory1.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxRecordDiagMsgsMemory1, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxRecordDiagMsgsMemory1.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxRecordDiagMsgsMemory1, (int)componentResourceManager.GetObject("checkBoxRecordDiagMsgsMemory1.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxRecordDiagMsgsMemory1, (int)componentResourceManager.GetObject("checkBoxRecordDiagMsgsMemory1.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxRecordDiagMsgsMemory1, (int)componentResourceManager.GetObject("checkBoxRecordDiagMsgsMemory1.IconPadding2"));
			this.checkBoxRecordDiagMsgsMemory1.Name = "checkBoxRecordDiagMsgsMemory1";
			this.checkBoxRecordDiagMsgsMemory1.UseVisualStyleBackColor = true;
			this.checkBoxRecordDiagMsgsMemory1.CheckedChanged += new EventHandler(this.checkBoxRecordDiagMsgsMemory1_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxRecordDiagMsgsMemory2, "checkBoxRecordDiagMsgsMemory2");
			this.errorProviderGlobalModel.SetError(this.checkBoxRecordDiagMsgsMemory2, componentResourceManager.GetString("checkBoxRecordDiagMsgsMemory2.Error"));
			this.errorProviderFormat.SetError(this.checkBoxRecordDiagMsgsMemory2, componentResourceManager.GetString("checkBoxRecordDiagMsgsMemory2.Error1"));
			this.errorProviderLocalModel.SetError(this.checkBoxRecordDiagMsgsMemory2, componentResourceManager.GetString("checkBoxRecordDiagMsgsMemory2.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxRecordDiagMsgsMemory2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxRecordDiagMsgsMemory2.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxRecordDiagMsgsMemory2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxRecordDiagMsgsMemory2.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxRecordDiagMsgsMemory2, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxRecordDiagMsgsMemory2.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxRecordDiagMsgsMemory2, (int)componentResourceManager.GetObject("checkBoxRecordDiagMsgsMemory2.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxRecordDiagMsgsMemory2, (int)componentResourceManager.GetObject("checkBoxRecordDiagMsgsMemory2.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxRecordDiagMsgsMemory2, (int)componentResourceManager.GetObject("checkBoxRecordDiagMsgsMemory2.IconPadding2"));
			this.checkBoxRecordDiagMsgsMemory2.Name = "checkBoxRecordDiagMsgsMemory2";
			this.checkBoxRecordDiagMsgsMemory2.UseVisualStyleBackColor = true;
			this.checkBoxRecordDiagMsgsMemory2.CheckedChanged += new EventHandler(this.checkBoxRecordDiagMsgsMemory2_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxIsRestartDiagEnabled, "checkBoxIsRestartDiagEnabled");
			this.errorProviderGlobalModel.SetError(this.checkBoxIsRestartDiagEnabled, componentResourceManager.GetString("checkBoxIsRestartDiagEnabled.Error"));
			this.errorProviderFormat.SetError(this.checkBoxIsRestartDiagEnabled, componentResourceManager.GetString("checkBoxIsRestartDiagEnabled.Error1"));
			this.errorProviderLocalModel.SetError(this.checkBoxIsRestartDiagEnabled, componentResourceManager.GetString("checkBoxIsRestartDiagEnabled.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxIsRestartDiagEnabled, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIsRestartDiagEnabled.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxIsRestartDiagEnabled, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIsRestartDiagEnabled.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxIsRestartDiagEnabled, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxIsRestartDiagEnabled.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxIsRestartDiagEnabled, (int)componentResourceManager.GetObject("checkBoxIsRestartDiagEnabled.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxIsRestartDiagEnabled, (int)componentResourceManager.GetObject("checkBoxIsRestartDiagEnabled.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxIsRestartDiagEnabled, (int)componentResourceManager.GetObject("checkBoxIsRestartDiagEnabled.IconPadding2"));
			this.checkBoxIsRestartDiagEnabled.Name = "checkBoxIsRestartDiagEnabled";
			this.checkBoxIsRestartDiagEnabled.UseVisualStyleBackColor = true;
			this.checkBoxIsRestartDiagEnabled.CheckedChanged += new EventHandler(this.checkBoxIsRestartDiagEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.textBoxDiagCommRestartTime, "textBoxDiagCommRestartTime");
			this.errorProviderGlobalModel.SetError(this.textBoxDiagCommRestartTime, componentResourceManager.GetString("textBoxDiagCommRestartTime.Error"));
			this.errorProviderFormat.SetError(this.textBoxDiagCommRestartTime, componentResourceManager.GetString("textBoxDiagCommRestartTime.Error1"));
			this.errorProviderLocalModel.SetError(this.textBoxDiagCommRestartTime, componentResourceManager.GetString("textBoxDiagCommRestartTime.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDiagCommRestartTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDiagCommRestartTime.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDiagCommRestartTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDiagCommRestartTime.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDiagCommRestartTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDiagCommRestartTime.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxDiagCommRestartTime, (int)componentResourceManager.GetObject("textBoxDiagCommRestartTime.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxDiagCommRestartTime, (int)componentResourceManager.GetObject("textBoxDiagCommRestartTime.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.textBoxDiagCommRestartTime, (int)componentResourceManager.GetObject("textBoxDiagCommRestartTime.IconPadding2"));
			this.textBoxDiagCommRestartTime.Name = "textBoxDiagCommRestartTime";
			this.textBoxDiagCommRestartTime.Validating += new CancelEventHandler(this.textBoxDiagCommRestartTime_Validating);
			componentResourceManager.ApplyResources(this.labelMin, "labelMin");
			this.errorProviderFormat.SetError(this.labelMin, componentResourceManager.GetString("labelMin.Error"));
			this.errorProviderGlobalModel.SetError(this.labelMin, componentResourceManager.GetString("labelMin.Error1"));
			this.errorProviderLocalModel.SetError(this.labelMin, componentResourceManager.GetString("labelMin.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelMin, (ErrorIconAlignment)componentResourceManager.GetObject("labelMin.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelMin, (ErrorIconAlignment)componentResourceManager.GetObject("labelMin.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelMin, (ErrorIconAlignment)componentResourceManager.GetObject("labelMin.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelMin, (int)componentResourceManager.GetObject("labelMin.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelMin, (int)componentResourceManager.GetObject("labelMin.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelMin, (int)componentResourceManager.GetObject("labelMin.IconPadding2"));
			this.labelMin.Name = "labelMin";
			componentResourceManager.ApplyResources(this.labelExternalTesterDesc, "labelExternalTesterDesc");
			this.errorProviderFormat.SetError(this.labelExternalTesterDesc, componentResourceManager.GetString("labelExternalTesterDesc.Error"));
			this.errorProviderGlobalModel.SetError(this.labelExternalTesterDesc, componentResourceManager.GetString("labelExternalTesterDesc.Error1"));
			this.errorProviderLocalModel.SetError(this.labelExternalTesterDesc, componentResourceManager.GetString("labelExternalTesterDesc.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelExternalTesterDesc, (ErrorIconAlignment)componentResourceManager.GetObject("labelExternalTesterDesc.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelExternalTesterDesc, (ErrorIconAlignment)componentResourceManager.GetObject("labelExternalTesterDesc.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelExternalTesterDesc, (ErrorIconAlignment)componentResourceManager.GetObject("labelExternalTesterDesc.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelExternalTesterDesc, (int)componentResourceManager.GetObject("labelExternalTesterDesc.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelExternalTesterDesc, (int)componentResourceManager.GetObject("labelExternalTesterDesc.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelExternalTesterDesc, (int)componentResourceManager.GetObject("labelExternalTesterDesc.IconPadding2"));
			this.labelExternalTesterDesc.Name = "labelExternalTesterDesc";
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderFormat, "errorProviderFormat");
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderLocalModel, "errorProviderLocalModel");
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.labelExternalTesterDesc);
			base.Controls.Add(this.labelMin);
			base.Controls.Add(this.textBoxDiagCommRestartTime);
			base.Controls.Add(this.checkBoxIsRestartDiagEnabled);
			base.Controls.Add(this.checkBoxRecordDiagMsgsMemory2);
			base.Controls.Add(this.checkBoxRecordDiagMsgsMemory1);
			base.Controls.Add(this.labelRecordDiagMsgs);
			this.errorProviderLocalModel.SetError(this, componentResourceManager.GetString("$this.Error"));
			this.errorProviderFormat.SetError(this, componentResourceManager.GetString("$this.Error1"));
			this.errorProviderGlobalModel.SetError(this, componentResourceManager.GetString("$this.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding2"));
			base.Name = "GeneralSettingsGL3Plus";
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
