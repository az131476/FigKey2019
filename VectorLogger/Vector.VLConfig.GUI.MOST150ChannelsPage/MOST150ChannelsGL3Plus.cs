using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.MOST150ChannelsPage
{
	public class MOST150ChannelsGL3Plus : UserControl
	{
		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isInitControls;

		private MOST150ChannelConfiguration most150ChannelConfiguration;

		private IContainer components;

		private GroupBox groupBoxMOST150;

		private CheckBox checkBoxEnableAutoStatusEvRep;

		private CheckBox checkBoxAsyncMEP;

		private CheckBox checkBoxAsyncMDP;

		private CheckBox checkBoxControlMsgs;

		private CheckBox checkBoxStatusEvents;

		private Label labelLog;

		private CheckBox checkBoxKeepAwake;

		private CheckBox checkBoxEnableChannels;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		public MOST150ChannelConfiguration MOST150ChannelConfiguration
		{
			get
			{
				return this.most150ChannelConfiguration;
			}
			set
			{
				this.most150ChannelConfiguration = value;
				this.UpdateGUI();
			}
		}

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public MOST150ChannelsGL3Plus()
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
		}

		private void checkBoxEnableChannels_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.EnableControls();
			this.ValidateInput();
		}

		private void checkBoxStatusEvents_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.EnableStatusEventSlaveControls();
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
			if (this.most150ChannelConfiguration == null)
			{
				return false;
			}
			bool flag = true;
			bool flag2 = false;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			bool flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxEnableChannels.Checked, this.most150ChannelConfiguration.IsChannelEnabled, this.guiElementManager.GetGUIElement(this.checkBoxEnableChannels), out flag3);
			flag2 |= flag3;
			if (this.checkBoxEnableChannels.Checked)
			{
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxKeepAwake.Checked, this.most150ChannelConfiguration.IsKeepAwakeEnabled, this.guiElementManager.GetGUIElement(this.checkBoxKeepAwake), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxStatusEvents.Checked, this.most150ChannelConfiguration.IsLogStatusEventsEnabled, this.guiElementManager.GetGUIElement(this.checkBoxStatusEvents), out flag3);
				flag2 |= flag3;
				if (this.checkBoxStatusEvents.Checked)
				{
					flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxEnableAutoStatusEvRep.Checked, this.most150ChannelConfiguration.IsAutoStatusEventRepEnabled, this.guiElementManager.GetGUIElement(this.checkBoxEnableAutoStatusEvRep), out flag3);
					flag2 |= flag3;
				}
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxControlMsgs.Checked, this.most150ChannelConfiguration.IsLogControlMsgsEnabled, this.guiElementManager.GetGUIElement(this.checkBoxControlMsgs), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxAsyncMDP.Checked, this.most150ChannelConfiguration.IsLogAsyncMDPEnabled, this.guiElementManager.GetGUIElement(this.checkBoxAsyncMDP), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxAsyncMEP.Checked, this.most150ChannelConfiguration.IsLogAsyncMEPEnabled, this.guiElementManager.GetGUIElement(this.checkBoxAsyncMEP), out flag3);
				flag2 |= flag3;
			}
			flag &= this.ModelValidator.Validate(this.most150ChannelConfiguration, flag2, this.pageValidator);
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
			if (this.most150ChannelConfiguration == null)
			{
				return;
			}
			this.isInitControls = true;
			this.checkBoxEnableChannels.Checked = this.most150ChannelConfiguration.IsChannelEnabled.Value;
			this.checkBoxControlMsgs.Checked = this.most150ChannelConfiguration.IsLogControlMsgsEnabled.Value;
			this.checkBoxStatusEvents.Checked = this.most150ChannelConfiguration.IsLogStatusEventsEnabled.Value;
			this.checkBoxAsyncMDP.Checked = this.most150ChannelConfiguration.IsLogAsyncMDPEnabled.Value;
			this.checkBoxAsyncMEP.Checked = this.most150ChannelConfiguration.IsLogAsyncMEPEnabled.Value;
			this.checkBoxKeepAwake.Checked = this.most150ChannelConfiguration.IsKeepAwakeEnabled.Value;
			this.checkBoxEnableAutoStatusEvRep.Checked = this.most150ChannelConfiguration.IsAutoStatusEventRepEnabled.Value;
			this.EnableControls();
			this.isInitControls = false;
			this.ValidateInput();
		}

		private void EnableControls()
		{
			bool @checked = this.checkBoxEnableChannels.Checked;
			this.checkBoxControlMsgs.Enabled = @checked;
			this.checkBoxStatusEvents.Enabled = @checked;
			this.checkBoxAsyncMDP.Enabled = @checked;
			this.checkBoxAsyncMEP.Enabled = @checked;
			this.checkBoxKeepAwake.Enabled = @checked;
			this.EnableStatusEventSlaveControls();
		}

		private void EnableStatusEventSlaveControls()
		{
			this.checkBoxEnableAutoStatusEvRep.Enabled = (this.checkBoxStatusEvents.Enabled && this.checkBoxStatusEvents.Checked);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MOST150ChannelsGL3Plus));
			this.groupBoxMOST150 = new GroupBox();
			this.checkBoxEnableAutoStatusEvRep = new CheckBox();
			this.checkBoxAsyncMEP = new CheckBox();
			this.checkBoxAsyncMDP = new CheckBox();
			this.checkBoxControlMsgs = new CheckBox();
			this.checkBoxStatusEvents = new CheckBox();
			this.labelLog = new Label();
			this.checkBoxKeepAwake = new CheckBox();
			this.checkBoxEnableChannels = new CheckBox();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.groupBoxMOST150.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxMOST150, "groupBoxMOST150");
			this.groupBoxMOST150.Controls.Add(this.checkBoxEnableAutoStatusEvRep);
			this.groupBoxMOST150.Controls.Add(this.checkBoxAsyncMEP);
			this.groupBoxMOST150.Controls.Add(this.checkBoxAsyncMDP);
			this.groupBoxMOST150.Controls.Add(this.checkBoxControlMsgs);
			this.groupBoxMOST150.Controls.Add(this.checkBoxStatusEvents);
			this.groupBoxMOST150.Controls.Add(this.labelLog);
			this.groupBoxMOST150.Controls.Add(this.checkBoxKeepAwake);
			this.groupBoxMOST150.Controls.Add(this.checkBoxEnableChannels);
			this.errorProviderFormat.SetError(this.groupBoxMOST150, componentResourceManager.GetString("groupBoxMOST150.Error"));
			this.errorProviderGlobalModel.SetError(this.groupBoxMOST150, componentResourceManager.GetString("groupBoxMOST150.Error1"));
			this.errorProviderLocalModel.SetError(this.groupBoxMOST150, componentResourceManager.GetString("groupBoxMOST150.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.groupBoxMOST150, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxMOST150.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.groupBoxMOST150, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxMOST150.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.groupBoxMOST150, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxMOST150.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.groupBoxMOST150, (int)componentResourceManager.GetObject("groupBoxMOST150.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.groupBoxMOST150, (int)componentResourceManager.GetObject("groupBoxMOST150.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.groupBoxMOST150, (int)componentResourceManager.GetObject("groupBoxMOST150.IconPadding2"));
			this.groupBoxMOST150.Name = "groupBoxMOST150";
			this.groupBoxMOST150.TabStop = false;
			componentResourceManager.ApplyResources(this.checkBoxEnableAutoStatusEvRep, "checkBoxEnableAutoStatusEvRep");
			this.errorProviderGlobalModel.SetError(this.checkBoxEnableAutoStatusEvRep, componentResourceManager.GetString("checkBoxEnableAutoStatusEvRep.Error"));
			this.errorProviderFormat.SetError(this.checkBoxEnableAutoStatusEvRep, componentResourceManager.GetString("checkBoxEnableAutoStatusEvRep.Error1"));
			this.errorProviderLocalModel.SetError(this.checkBoxEnableAutoStatusEvRep, componentResourceManager.GetString("checkBoxEnableAutoStatusEvRep.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxEnableAutoStatusEvRep, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxEnableAutoStatusEvRep.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxEnableAutoStatusEvRep, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxEnableAutoStatusEvRep.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxEnableAutoStatusEvRep, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxEnableAutoStatusEvRep.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxEnableAutoStatusEvRep, (int)componentResourceManager.GetObject("checkBoxEnableAutoStatusEvRep.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxEnableAutoStatusEvRep, (int)componentResourceManager.GetObject("checkBoxEnableAutoStatusEvRep.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxEnableAutoStatusEvRep, (int)componentResourceManager.GetObject("checkBoxEnableAutoStatusEvRep.IconPadding2"));
			this.checkBoxEnableAutoStatusEvRep.Name = "checkBoxEnableAutoStatusEvRep";
			this.checkBoxEnableAutoStatusEvRep.UseVisualStyleBackColor = true;
			this.checkBoxEnableAutoStatusEvRep.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAsyncMEP, "checkBoxAsyncMEP");
			this.errorProviderGlobalModel.SetError(this.checkBoxAsyncMEP, componentResourceManager.GetString("checkBoxAsyncMEP.Error"));
			this.errorProviderFormat.SetError(this.checkBoxAsyncMEP, componentResourceManager.GetString("checkBoxAsyncMEP.Error1"));
			this.errorProviderLocalModel.SetError(this.checkBoxAsyncMEP, componentResourceManager.GetString("checkBoxAsyncMEP.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxAsyncMEP, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAsyncMEP.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxAsyncMEP, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAsyncMEP.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxAsyncMEP, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAsyncMEP.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxAsyncMEP, (int)componentResourceManager.GetObject("checkBoxAsyncMEP.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxAsyncMEP, (int)componentResourceManager.GetObject("checkBoxAsyncMEP.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxAsyncMEP, (int)componentResourceManager.GetObject("checkBoxAsyncMEP.IconPadding2"));
			this.checkBoxAsyncMEP.Name = "checkBoxAsyncMEP";
			this.checkBoxAsyncMEP.UseVisualStyleBackColor = true;
			this.checkBoxAsyncMEP.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAsyncMDP, "checkBoxAsyncMDP");
			this.errorProviderGlobalModel.SetError(this.checkBoxAsyncMDP, componentResourceManager.GetString("checkBoxAsyncMDP.Error"));
			this.errorProviderFormat.SetError(this.checkBoxAsyncMDP, componentResourceManager.GetString("checkBoxAsyncMDP.Error1"));
			this.errorProviderLocalModel.SetError(this.checkBoxAsyncMDP, componentResourceManager.GetString("checkBoxAsyncMDP.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxAsyncMDP, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAsyncMDP.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxAsyncMDP, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAsyncMDP.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxAsyncMDP, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxAsyncMDP.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxAsyncMDP, (int)componentResourceManager.GetObject("checkBoxAsyncMDP.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxAsyncMDP, (int)componentResourceManager.GetObject("checkBoxAsyncMDP.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxAsyncMDP, (int)componentResourceManager.GetObject("checkBoxAsyncMDP.IconPadding2"));
			this.checkBoxAsyncMDP.Name = "checkBoxAsyncMDP";
			this.checkBoxAsyncMDP.UseVisualStyleBackColor = true;
			this.checkBoxAsyncMDP.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxControlMsgs, "checkBoxControlMsgs");
			this.errorProviderGlobalModel.SetError(this.checkBoxControlMsgs, componentResourceManager.GetString("checkBoxControlMsgs.Error"));
			this.errorProviderFormat.SetError(this.checkBoxControlMsgs, componentResourceManager.GetString("checkBoxControlMsgs.Error1"));
			this.errorProviderLocalModel.SetError(this.checkBoxControlMsgs, componentResourceManager.GetString("checkBoxControlMsgs.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxControlMsgs, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxControlMsgs.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxControlMsgs, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxControlMsgs.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxControlMsgs, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxControlMsgs.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxControlMsgs, (int)componentResourceManager.GetObject("checkBoxControlMsgs.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxControlMsgs, (int)componentResourceManager.GetObject("checkBoxControlMsgs.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxControlMsgs, (int)componentResourceManager.GetObject("checkBoxControlMsgs.IconPadding2"));
			this.checkBoxControlMsgs.Name = "checkBoxControlMsgs";
			this.checkBoxControlMsgs.UseVisualStyleBackColor = true;
			this.checkBoxControlMsgs.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxStatusEvents, "checkBoxStatusEvents");
			this.errorProviderGlobalModel.SetError(this.checkBoxStatusEvents, componentResourceManager.GetString("checkBoxStatusEvents.Error"));
			this.errorProviderFormat.SetError(this.checkBoxStatusEvents, componentResourceManager.GetString("checkBoxStatusEvents.Error1"));
			this.errorProviderLocalModel.SetError(this.checkBoxStatusEvents, componentResourceManager.GetString("checkBoxStatusEvents.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxStatusEvents, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStatusEvents.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxStatusEvents, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStatusEvents.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxStatusEvents, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStatusEvents.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxStatusEvents, (int)componentResourceManager.GetObject("checkBoxStatusEvents.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxStatusEvents, (int)componentResourceManager.GetObject("checkBoxStatusEvents.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxStatusEvents, (int)componentResourceManager.GetObject("checkBoxStatusEvents.IconPadding2"));
			this.checkBoxStatusEvents.Name = "checkBoxStatusEvents";
			this.checkBoxStatusEvents.UseVisualStyleBackColor = true;
			this.checkBoxStatusEvents.CheckedChanged += new EventHandler(this.checkBoxStatusEvents_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelLog, "labelLog");
			this.errorProviderFormat.SetError(this.labelLog, componentResourceManager.GetString("labelLog.Error"));
			this.errorProviderGlobalModel.SetError(this.labelLog, componentResourceManager.GetString("labelLog.Error1"));
			this.errorProviderLocalModel.SetError(this.labelLog, componentResourceManager.GetString("labelLog.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelLog, (ErrorIconAlignment)componentResourceManager.GetObject("labelLog.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelLog, (ErrorIconAlignment)componentResourceManager.GetObject("labelLog.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelLog, (ErrorIconAlignment)componentResourceManager.GetObject("labelLog.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelLog, (int)componentResourceManager.GetObject("labelLog.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelLog, (int)componentResourceManager.GetObject("labelLog.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelLog, (int)componentResourceManager.GetObject("labelLog.IconPadding2"));
			this.labelLog.Name = "labelLog";
			componentResourceManager.ApplyResources(this.checkBoxKeepAwake, "checkBoxKeepAwake");
			this.errorProviderGlobalModel.SetError(this.checkBoxKeepAwake, componentResourceManager.GetString("checkBoxKeepAwake.Error"));
			this.errorProviderFormat.SetError(this.checkBoxKeepAwake, componentResourceManager.GetString("checkBoxKeepAwake.Error1"));
			this.errorProviderLocalModel.SetError(this.checkBoxKeepAwake, componentResourceManager.GetString("checkBoxKeepAwake.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxKeepAwake, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxKeepAwake.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxKeepAwake, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxKeepAwake.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxKeepAwake, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxKeepAwake.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxKeepAwake, (int)componentResourceManager.GetObject("checkBoxKeepAwake.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxKeepAwake, (int)componentResourceManager.GetObject("checkBoxKeepAwake.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxKeepAwake, (int)componentResourceManager.GetObject("checkBoxKeepAwake.IconPadding2"));
			this.checkBoxKeepAwake.Name = "checkBoxKeepAwake";
			this.checkBoxKeepAwake.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwake.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxEnableChannels, "checkBoxEnableChannels");
			this.errorProviderGlobalModel.SetError(this.checkBoxEnableChannels, componentResourceManager.GetString("checkBoxEnableChannels.Error"));
			this.errorProviderFormat.SetError(this.checkBoxEnableChannels, componentResourceManager.GetString("checkBoxEnableChannels.Error1"));
			this.errorProviderLocalModel.SetError(this.checkBoxEnableChannels, componentResourceManager.GetString("checkBoxEnableChannels.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxEnableChannels, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxEnableChannels.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxEnableChannels, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxEnableChannels.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxEnableChannels, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxEnableChannels.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxEnableChannels, (int)componentResourceManager.GetObject("checkBoxEnableChannels.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxEnableChannels, (int)componentResourceManager.GetObject("checkBoxEnableChannels.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxEnableChannels, (int)componentResourceManager.GetObject("checkBoxEnableChannels.IconPadding2"));
			this.checkBoxEnableChannels.Name = "checkBoxEnableChannels";
			this.checkBoxEnableChannels.UseVisualStyleBackColor = true;
			this.checkBoxEnableChannels.CheckedChanged += new EventHandler(this.checkBoxEnableChannels_CheckedChanged);
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
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxMOST150);
			this.errorProviderLocalModel.SetError(this, componentResourceManager.GetString("$this.Error"));
			this.errorProviderFormat.SetError(this, componentResourceManager.GetString("$this.Error1"));
			this.errorProviderGlobalModel.SetError(this, componentResourceManager.GetString("$this.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding2"));
			base.Name = "MOST150ChannelsGL3Plus";
			this.groupBoxMOST150.ResumeLayout(false);
			this.groupBoxMOST150.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
