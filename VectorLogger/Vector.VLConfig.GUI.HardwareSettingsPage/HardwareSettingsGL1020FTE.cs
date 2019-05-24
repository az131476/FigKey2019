using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.HardwareSettingsPage
{
	internal class HardwareSettingsGL1020FTE : UserControl
	{
		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isInitControls;

		private LogDataStorage logDataStorage;

		private DisplayMode displayMode;

		private IContainer components;

		private GroupBox groupBoxTimeoutToSleep;

		private TextBox textBoxTimeoutToSleep;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private CheckBox checkBoxEnterSleepMode;

		private Label labelSec;

		private GroupBox groupBoxActivationDelay;

		private Label labelMs;

		private TextBox textBoxMsgTimeoutActivationDelay;

		private Label labelActivationOnMsgTimeout;

		private Label labelInfoSuppressOnMsgTimeout;

		private Label labelSleepIndication;

		private Label labelStopCyclicCommInfo;

		private TextBox textBoxEventType;

		private TextBox textBoxCondition;

		private Button buttonChangeEventType;

		private Button buttonChangeCondition;

		private ToolTip toolTip;

		public LogDataStorage LogDataStorage
		{
			get
			{
				return this.logDataStorage;
			}
			set
			{
				this.logDataStorage = value;
				if (this.logDataStorage != null)
				{
					this.UpdateGUI();
				}
			}
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
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

		private bool GUIIsEnterSleepModeEnabled
		{
			get
			{
				return this.checkBoxEnterSleepMode.Checked;
			}
			set
			{
				this.checkBoxEnterSleepMode.Checked = value;
			}
		}

		private uint GUITimeoutToSleep
		{
			get
			{
				uint result;
				if (uint.TryParse(this.textBoxTimeoutToSleep.Text, out result))
				{
					return result;
				}
				return 0u;
			}
			set
			{
				this.textBoxTimeoutToSleep.Text = value.ToString();
			}
		}

		private uint GUIEventActivationDelayAfterStart
		{
			get
			{
				uint result;
				if (uint.TryParse(this.textBoxMsgTimeoutActivationDelay.Text, out result))
				{
					return result;
				}
				return 0u;
			}
			set
			{
				this.textBoxMsgTimeoutActivationDelay.Text = value.ToString();
			}
		}

		public HardwareSettingsGL1020FTE()
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
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		private void control_Validating(object sender, CancelEventArgs e)
		{
			this.ValidateInput(false);
		}

		private void checkBox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput(false);
		}

		private void checkBoxEnterSleepMode_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.textBoxTimeoutToSleep.Enabled = this.checkBoxEnterSleepMode.Checked;
			this.ValidateInput(false);
		}

		private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput(false);
		}

		private void buttonChangeEventType_Click(object sender, EventArgs e)
		{
			using (SelectEventType selectEventType = new SelectEventType(this.ModelValidator.LoggerSpecifics))
			{
				selectEventType.SelectedEventType = this.textBoxEventType.Text;
				if (selectEventType.ShowDialog() == DialogResult.OK)
				{
					if (selectEventType.SelectedEventType != this.textBoxEventType.Text)
					{
						this.CreateNewConditionForEventType(selectEventType.SelectedEventType);
					}
					else if (this.logDataStorage.StopCyclicCommunicationEvent != null)
					{
						this.EditCurrentCondition();
					}
				}
			}
		}

		private void buttonChangeCondition_Click(object sender, EventArgs e)
		{
			this.EditCurrentCondition();
		}

		private void textBoxCondition_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.textBoxCondition, this.textBoxCondition.Text);
		}

		public bool ValidateInput(bool isDataChanged = false)
		{
			if (this.logDataStorage == null)
			{
				return false;
			}
			bool flag = true;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			bool flag2;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxEnterSleepMode.Checked, this.logDataStorage.IsEnterSleepModeEnabled, this.guiElementManager.GetGUIElement(this.checkBoxEnterSleepMode), out flag2);
			bool flag3 = isDataChanged | flag2;
			if (this.checkBoxEnterSleepMode.Checked)
			{
				flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_UInt32(this.textBoxTimeoutToSleep.Text, this.logDataStorage.TimeoutToSleep, this.guiElementManager.GetGUIElement(this.textBoxTimeoutToSleep), out flag2);
				flag3 |= flag2;
			}
			else
			{
				this.GUITimeoutToSleep = this.logDataStorage.TimeoutToSleep.Value;
			}
			flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_UInt32(this.textBoxMsgTimeoutActivationDelay.Text, this.logDataStorage.EventActivationDelayAfterStart, this.guiElementManager.GetGUIElement(this.textBoxMsgTimeoutActivationDelay), out flag2);
			flag3 |= flag2;
			if (this.logDataStorage.StopCyclicCommunicationEvent is SymbolicSignalEvent)
			{
				SymbolicSignalEvent symbolicSignalEvent = this.logDataStorage.StopCyclicCommunicationEvent as SymbolicSignalEvent;
				flag &= this.pageValidator.Control.UpdateModel<uint>(symbolicSignalEvent.ChannelNumber.Value, symbolicSignalEvent.ChannelNumber, this.guiElementManager.GetGUIElement(this.textBoxCondition), out flag2);
				flag3 |= flag2;
				flag &= this.pageValidator.Control.UpdateModel<string>(symbolicSignalEvent.MessageName.Value, symbolicSignalEvent.MessageName, this.guiElementManager.GetGUIElement(this.textBoxCondition), out flag2);
				flag3 |= flag2;
			}
			else if (this.logDataStorage.StopCyclicCommunicationEvent is CANDataEvent)
			{
				CANDataEvent cANDataEvent = this.logDataStorage.StopCyclicCommunicationEvent as CANDataEvent;
				flag &= this.pageValidator.Control.UpdateModel<uint>(cANDataEvent.ChannelNumber.Value, cANDataEvent.ChannelNumber, this.guiElementManager.GetGUIElement(this.textBoxCondition), out flag2);
				flag3 |= flag2;
			}
			else if (this.logDataStorage.StopCyclicCommunicationEvent is MsgTimeoutEvent)
			{
				MsgTimeoutEvent msgTimeoutEvent = this.logDataStorage.StopCyclicCommunicationEvent as MsgTimeoutEvent;
				flag &= this.pageValidator.Control.UpdateModel<uint>(msgTimeoutEvent.ChannelNumber.Value, msgTimeoutEvent.ChannelNumber, this.guiElementManager.GetGUIElement(this.textBoxCondition), out flag2);
				flag3 |= flag2;
				flag &= this.pageValidator.Control.UpdateModel<string>(msgTimeoutEvent.MessageName.Value, msgTimeoutEvent.MessageName, this.guiElementManager.GetGUIElement(this.textBoxCondition), out flag2);
				flag3 |= flag2;
			}
			flag &= this.ModelValidator.Validate(this.logDataStorage, flag3, this.pageValidator);
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

		private void UpdateGUI()
		{
			if (this.logDataStorage == null)
			{
				return;
			}
			this.isInitControls = true;
			if (!this.pageValidator.General.HasFormatError(this.logDataStorage.IsEnterSleepModeEnabled))
			{
				this.GUIIsEnterSleepModeEnabled = this.logDataStorage.IsEnterSleepModeEnabled.Value;
			}
			this.textBoxTimeoutToSleep.Enabled = this.GUIIsEnterSleepModeEnabled;
			if (!this.pageValidator.General.HasFormatError(this.logDataStorage.TimeoutToSleep))
			{
				this.GUITimeoutToSleep = this.logDataStorage.TimeoutToSleep.Value;
			}
			if (!this.pageValidator.General.HasFormatError(this.logDataStorage.EventActivationDelayAfterStart))
			{
				this.GUIEventActivationDelayAfterStart = this.logDataStorage.EventActivationDelayAfterStart.Value;
			}
			HardwareSettingsCommon.DisplayStopCycDiagCommEventCondition(this.logDataStorage.StopCyclicCommunicationEvent, this.ModelValidator.DatabaseServices, this.ModelValidator.LoggerSpecifics, ref this.textBoxEventType, ref this.textBoxCondition, ref this.buttonChangeCondition);
			this.isInitControls = false;
			this.ValidateInput(false);
		}

		private void CreateNewConditionForEventType(string eventTypeName)
		{
			Event stopCyclicCommunicationEvent;
			bool flag = HardwareSettingsCommon.CreateStopCycDiagCommEvent(eventTypeName, this.ModelValidator, this.ApplicationDatabaseManager, out stopCyclicCommunicationEvent);
			if (flag)
			{
				this.logDataStorage.StopCyclicCommunicationEvent = stopCyclicCommunicationEvent;
				HardwareSettingsCommon.DisplayStopCycDiagCommEventCondition(this.logDataStorage.StopCyclicCommunicationEvent, this.ModelValidator.DatabaseServices, this.ModelValidator.LoggerSpecifics, ref this.textBoxEventType, ref this.textBoxCondition, ref this.buttonChangeCondition);
				this.ValidateInput(true);
			}
		}

		private void EditCurrentCondition()
		{
			bool flag = HardwareSettingsCommon.EditStopCycDiagCommEvent(this.ModelValidator, this.ApplicationDatabaseManager, ref this.logDataStorage.StopCyclicCommunicationEvent);
			if (flag)
			{
				HardwareSettingsCommon.DisplayStopCycDiagCommEventCondition(this.logDataStorage.StopCyclicCommunicationEvent, this.ModelValidator.DatabaseServices, this.ModelValidator.LoggerSpecifics, ref this.textBoxEventType, ref this.textBoxCondition, ref this.buttonChangeCondition);
				this.ValidateInput(true);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(HardwareSettingsGL1020FTE));
			this.groupBoxTimeoutToSleep = new GroupBox();
			this.checkBoxEnterSleepMode = new CheckBox();
			this.textBoxTimeoutToSleep = new TextBox();
			this.labelSec = new Label();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.textBoxMsgTimeoutActivationDelay = new TextBox();
			this.textBoxCondition = new TextBox();
			this.textBoxEventType = new TextBox();
			this.groupBoxActivationDelay = new GroupBox();
			this.buttonChangeCondition = new Button();
			this.buttonChangeEventType = new Button();
			this.labelStopCyclicCommInfo = new Label();
			this.labelSleepIndication = new Label();
			this.labelInfoSuppressOnMsgTimeout = new Label();
			this.labelMs = new Label();
			this.labelActivationOnMsgTimeout = new Label();
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.toolTip = new ToolTip(this.components);
			this.groupBoxTimeoutToSleep.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			this.groupBoxActivationDelay.SuspendLayout();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.groupBoxTimeoutToSleep.Controls.Add(this.checkBoxEnterSleepMode);
			this.groupBoxTimeoutToSleep.Controls.Add(this.textBoxTimeoutToSleep);
			this.groupBoxTimeoutToSleep.Controls.Add(this.labelSec);
			componentResourceManager.ApplyResources(this.groupBoxTimeoutToSleep, "groupBoxTimeoutToSleep");
			this.groupBoxTimeoutToSleep.Name = "groupBoxTimeoutToSleep";
			this.groupBoxTimeoutToSleep.TabStop = false;
			componentResourceManager.ApplyResources(this.checkBoxEnterSleepMode, "checkBoxEnterSleepMode");
			this.checkBoxEnterSleepMode.Name = "checkBoxEnterSleepMode";
			this.checkBoxEnterSleepMode.UseVisualStyleBackColor = true;
			this.checkBoxEnterSleepMode.CheckedChanged += new EventHandler(this.checkBoxEnterSleepMode_CheckedChanged);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxTimeoutToSleep, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxTimeoutToSleep.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxTimeoutToSleep, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxTimeoutToSleep.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxTimeoutToSleep, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxTimeoutToSleep.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxTimeoutToSleep, "textBoxTimeoutToSleep");
			this.textBoxTimeoutToSleep.Name = "textBoxTimeoutToSleep";
			this.textBoxTimeoutToSleep.Validating += new CancelEventHandler(this.control_Validating);
			componentResourceManager.ApplyResources(this.labelSec, "labelSec");
			this.labelSec.Name = "labelSec";
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxMsgTimeoutActivationDelay, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMsgTimeoutActivationDelay.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxMsgTimeoutActivationDelay, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMsgTimeoutActivationDelay.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxMsgTimeoutActivationDelay, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMsgTimeoutActivationDelay.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxMsgTimeoutActivationDelay, "textBoxMsgTimeoutActivationDelay");
			this.textBoxMsgTimeoutActivationDelay.Name = "textBoxMsgTimeoutActivationDelay";
			this.textBoxMsgTimeoutActivationDelay.Validating += new CancelEventHandler(this.control_Validating);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCondition, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCondition.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCondition, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCondition.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCondition, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCondition.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxCondition, "textBoxCondition");
			this.textBoxCondition.Name = "textBoxCondition";
			this.textBoxCondition.ReadOnly = true;
			this.textBoxCondition.MouseEnter += new EventHandler(this.textBoxCondition_MouseEnter);
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxEventType, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxEventType.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxEventType, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxEventType.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxEventType, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxEventType.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxEventType, "textBoxEventType");
			this.textBoxEventType.Name = "textBoxEventType";
			this.textBoxEventType.ReadOnly = true;
			this.groupBoxActivationDelay.Controls.Add(this.buttonChangeCondition);
			this.groupBoxActivationDelay.Controls.Add(this.buttonChangeEventType);
			this.groupBoxActivationDelay.Controls.Add(this.textBoxCondition);
			this.groupBoxActivationDelay.Controls.Add(this.textBoxEventType);
			this.groupBoxActivationDelay.Controls.Add(this.labelStopCyclicCommInfo);
			this.groupBoxActivationDelay.Controls.Add(this.labelSleepIndication);
			this.groupBoxActivationDelay.Controls.Add(this.labelInfoSuppressOnMsgTimeout);
			this.groupBoxActivationDelay.Controls.Add(this.labelMs);
			this.groupBoxActivationDelay.Controls.Add(this.textBoxMsgTimeoutActivationDelay);
			this.groupBoxActivationDelay.Controls.Add(this.labelActivationOnMsgTimeout);
			componentResourceManager.ApplyResources(this.groupBoxActivationDelay, "groupBoxActivationDelay");
			this.groupBoxActivationDelay.Name = "groupBoxActivationDelay";
			this.groupBoxActivationDelay.TabStop = false;
			componentResourceManager.ApplyResources(this.buttonChangeCondition, "buttonChangeCondition");
			this.buttonChangeCondition.Name = "buttonChangeCondition";
			this.buttonChangeCondition.UseVisualStyleBackColor = true;
			this.buttonChangeCondition.Click += new EventHandler(this.buttonChangeCondition_Click);
			componentResourceManager.ApplyResources(this.buttonChangeEventType, "buttonChangeEventType");
			this.buttonChangeEventType.Name = "buttonChangeEventType";
			this.buttonChangeEventType.UseVisualStyleBackColor = true;
			this.buttonChangeEventType.Click += new EventHandler(this.buttonChangeEventType_Click);
			componentResourceManager.ApplyResources(this.labelStopCyclicCommInfo, "labelStopCyclicCommInfo");
			this.labelStopCyclicCommInfo.Name = "labelStopCyclicCommInfo";
			componentResourceManager.ApplyResources(this.labelSleepIndication, "labelSleepIndication");
			this.labelSleepIndication.Name = "labelSleepIndication";
			componentResourceManager.ApplyResources(this.labelInfoSuppressOnMsgTimeout, "labelInfoSuppressOnMsgTimeout");
			this.labelInfoSuppressOnMsgTimeout.Name = "labelInfoSuppressOnMsgTimeout";
			componentResourceManager.ApplyResources(this.labelMs, "labelMs");
			this.labelMs.Name = "labelMs";
			componentResourceManager.ApplyResources(this.labelActivationOnMsgTimeout, "labelActivationOnMsgTimeout");
			this.labelActivationOnMsgTimeout.Name = "labelActivationOnMsgTimeout";
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			this.AutoValidate = AutoValidate.EnableAllowFocusChange;
			base.Controls.Add(this.groupBoxActivationDelay);
			base.Controls.Add(this.groupBoxTimeoutToSleep);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "HardwareSettingsGL1020FTE";
			this.groupBoxTimeoutToSleep.ResumeLayout(false);
			this.groupBoxTimeoutToSleep.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			this.groupBoxActivationDelay.ResumeLayout(false);
			this.groupBoxActivationDelay.PerformLayout();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
