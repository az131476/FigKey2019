using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.FlexrayChannelsPage
{
	internal class FlexrayChannelsGL4000 : UserControl
	{
		private Dictionary<uint, CheckBox> channelNr2CheckBoxIsActive;

		private Dictionary<uint, CheckBox> channelNr2CheckBoxKeepAwake;

		private Dictionary<uint, CheckBox> channelNr2CheckBoxWakeUpEnabled;

		private FlexrayChannelConfiguration flexrayChannelConfiguration;

		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isInitControls;

		private IContainer components;

		private GroupBox groupBoxChannels;

		private TableLayoutPanel tableLayoutPanelChannels;

		private Label labelCapacity1;

		private Label labelCapacity2;

		private CheckBox checkBoxKeepAwakeFlexray1;

		private CheckBox checkBoxKeepAwakeFlexray2;

		private CheckBox checkBoxWakeUpFlexray1;

		private CheckBox checkBoxWakeUpFlexray2;

		private GroupBox groupBoxDescription;

		private RichTextBox richTextBox1;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private CheckBox checkBoxFlexray;

		private Label labelFlexray1;

		private Label label1;

		private CheckBox checkBoxFlexray1;

		private CheckBox checkBoxFlexray2;

		public FlexrayChannelConfiguration FlexrayChannelConfiguration
		{
			get
			{
				return this.flexrayChannelConfiguration;
			}
			set
			{
				this.flexrayChannelConfiguration = value;
				if (this.flexrayChannelConfiguration != null)
				{
					if (this.ModelValidator != null)
					{
						ulong arg_45_0 = (ulong)this.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels;
						long arg_44_0 = (long)this.flexrayChannelConfiguration.FlexrayChannels.Count;
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
			get;
			set;
		}

		public FlexrayChannelsGL4000()
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
			this.InitChannelNr2ControlLists();
		}

		private void InitChannelNr2ControlLists()
		{
			this.channelNr2CheckBoxIsActive = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxIsActive.Add(1u, this.checkBoxFlexray1);
			this.channelNr2CheckBoxIsActive.Add(2u, this.checkBoxFlexray2);
			this.channelNr2CheckBoxKeepAwake = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxKeepAwake.Add(1u, this.checkBoxKeepAwakeFlexray1);
			this.channelNr2CheckBoxKeepAwake.Add(2u, this.checkBoxKeepAwakeFlexray2);
			this.channelNr2CheckBoxWakeUpEnabled = new Dictionary<uint, CheckBox>();
			this.channelNr2CheckBoxWakeUpEnabled.Add(1u, this.checkBoxWakeUpFlexray1);
			this.channelNr2CheckBoxWakeUpEnabled.Add(2u, this.checkBoxWakeUpFlexray2);
		}

		private void checkBoxFlexray_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.EnableControls();
			this.checkBoxFlexray1.Checked = this.checkBoxFlexray.Checked;
			this.checkBoxFlexray2.Checked = this.checkBoxFlexray.Checked;
			this.ValidateInput();
		}

		private void checkBoxKeepAwake_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxWakeUp_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void radioButtonUsage_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		public bool ValidateInput()
		{
			if (this.flexrayChannelConfiguration == null)
			{
				return false;
			}
			bool flag = true;
			bool flag2 = false;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.channelNr2CheckBoxKeepAwake.Count))
			{
				bool flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.channelNr2CheckBoxIsActive[num].Checked, this.flexrayChannelConfiguration.GetFlexrayChannel(num).IsActive, this.guiElementManager.GetGUIElement(this.channelNr2CheckBoxIsActive[num]), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.channelNr2CheckBoxKeepAwake[num].Checked, this.flexrayChannelConfiguration.GetFlexrayChannel(num).IsKeepAwakeActive, this.guiElementManager.GetGUIElement(this.channelNr2CheckBoxKeepAwake[num]), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.channelNr2CheckBoxWakeUpEnabled[num].Checked, this.flexrayChannelConfiguration.GetFlexrayChannel(num).IsWakeUpEnabled, this.guiElementManager.GetGUIElement(this.channelNr2CheckBoxWakeUpEnabled[num]), out flag3);
				flag2 |= flag3;
				num += 1u;
			}
			this.ModelValidator.Validate(this.flexrayChannelConfiguration, flag2, this.pageValidator);
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

		private void EnableControls()
		{
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.flexrayChannelConfiguration.FlexrayChannels.Count))
			{
				this.channelNr2CheckBoxKeepAwake[num].Enabled = this.checkBoxFlexray.Checked;
				this.channelNr2CheckBoxWakeUpEnabled[num].Enabled = this.checkBoxFlexray.Checked;
				num += 1u;
			}
		}

		private void UpdateGUI()
		{
			if (this.flexrayChannelConfiguration == null)
			{
				return;
			}
			this.isInitControls = true;
			bool flag = false;
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.flexrayChannelConfiguration.FlexrayChannels.Count))
			{
				FlexrayChannel flexrayChannel = this.flexrayChannelConfiguration.GetFlexrayChannel(num);
				this.channelNr2CheckBoxIsActive[num].Checked = flexrayChannel.IsActive.Value;
				flag |= flexrayChannel.IsActive.Value;
				this.channelNr2CheckBoxKeepAwake[num].Checked = flexrayChannel.IsKeepAwakeActive.Value;
				this.channelNr2CheckBoxWakeUpEnabled[num].Checked = flexrayChannel.IsWakeUpEnabled.Value;
				num += 1u;
			}
			this.checkBoxFlexray.Checked = flag;
			this.EnableControls();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FlexrayChannelsGL4000));
			this.groupBoxChannels = new GroupBox();
			this.tableLayoutPanelChannels = new TableLayoutPanel();
			this.labelCapacity1 = new Label();
			this.labelCapacity2 = new Label();
			this.checkBoxKeepAwakeFlexray1 = new CheckBox();
			this.checkBoxKeepAwakeFlexray2 = new CheckBox();
			this.checkBoxWakeUpFlexray1 = new CheckBox();
			this.checkBoxWakeUpFlexray2 = new CheckBox();
			this.checkBoxFlexray = new CheckBox();
			this.labelFlexray1 = new Label();
			this.label1 = new Label();
			this.checkBoxFlexray1 = new CheckBox();
			this.checkBoxFlexray2 = new CheckBox();
			this.groupBoxDescription = new GroupBox();
			this.richTextBox1 = new RichTextBox();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.groupBoxChannels.SuspendLayout();
			this.tableLayoutPanelChannels.SuspendLayout();
			this.groupBoxDescription.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.groupBoxChannels.Controls.Add(this.tableLayoutPanelChannels);
			componentResourceManager.ApplyResources(this.groupBoxChannels, "groupBoxChannels");
			this.groupBoxChannels.Name = "groupBoxChannels";
			this.groupBoxChannels.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanelChannels, "tableLayoutPanelChannels");
			this.tableLayoutPanelChannels.Controls.Add(this.labelCapacity1, 1, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.labelCapacity2, 1, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeFlexray1, 2, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxKeepAwakeFlexray2, 2, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxWakeUpFlexray1, 3, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxWakeUpFlexray2, 3, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxFlexray, 0, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.labelFlexray1, 0, 1);
			this.tableLayoutPanelChannels.Controls.Add(this.label1, 0, 2);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxFlexray1, 2, 0);
			this.tableLayoutPanelChannels.Controls.Add(this.checkBoxFlexray2, 3, 0);
			this.tableLayoutPanelChannels.Name = "tableLayoutPanelChannels";
			componentResourceManager.ApplyResources(this.labelCapacity1, "labelCapacity1");
			this.labelCapacity1.Name = "labelCapacity1";
			componentResourceManager.ApplyResources(this.labelCapacity2, "labelCapacity2");
			this.labelCapacity2.Name = "labelCapacity2";
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeFlexray1, "checkBoxKeepAwakeFlexray1");
			this.checkBoxKeepAwakeFlexray1.Name = "checkBoxKeepAwakeFlexray1";
			this.checkBoxKeepAwakeFlexray1.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeFlexray1.CheckedChanged += new EventHandler(this.checkBoxKeepAwake_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxKeepAwakeFlexray2, "checkBoxKeepAwakeFlexray2");
			this.checkBoxKeepAwakeFlexray2.Name = "checkBoxKeepAwakeFlexray2";
			this.checkBoxKeepAwakeFlexray2.UseVisualStyleBackColor = true;
			this.checkBoxKeepAwakeFlexray2.CheckedChanged += new EventHandler(this.checkBoxKeepAwake_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxWakeUpFlexray1, "checkBoxWakeUpFlexray1");
			this.checkBoxWakeUpFlexray1.Name = "checkBoxWakeUpFlexray1";
			this.checkBoxWakeUpFlexray1.UseVisualStyleBackColor = true;
			this.checkBoxWakeUpFlexray1.CheckedChanged += new EventHandler(this.checkBoxWakeUp_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxWakeUpFlexray2, "checkBoxWakeUpFlexray2");
			this.checkBoxWakeUpFlexray2.Name = "checkBoxWakeUpFlexray2";
			this.checkBoxWakeUpFlexray2.UseVisualStyleBackColor = true;
			this.checkBoxWakeUpFlexray2.CheckedChanged += new EventHandler(this.checkBoxWakeUp_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxFlexray, "checkBoxFlexray");
			this.tableLayoutPanelChannels.SetColumnSpan(this.checkBoxFlexray, 2);
			this.checkBoxFlexray.Name = "checkBoxFlexray";
			this.checkBoxFlexray.UseVisualStyleBackColor = true;
			this.checkBoxFlexray.CheckedChanged += new EventHandler(this.checkBoxFlexray_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelFlexray1, "labelFlexray1");
			this.labelFlexray1.Name = "labelFlexray1";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.checkBoxFlexray1, "checkBoxFlexray1");
			this.checkBoxFlexray1.Name = "checkBoxFlexray1";
			this.checkBoxFlexray1.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBoxFlexray2, "checkBoxFlexray2");
			this.checkBoxFlexray2.Name = "checkBoxFlexray2";
			this.checkBoxFlexray2.UseVisualStyleBackColor = true;
			this.groupBoxDescription.Controls.Add(this.richTextBox1);
			componentResourceManager.ApplyResources(this.groupBoxDescription, "groupBoxDescription");
			this.groupBoxDescription.Name = "groupBoxDescription";
			this.groupBoxDescription.TabStop = false;
			this.richTextBox1.BorderStyle = BorderStyle.None;
			componentResourceManager.ApplyResources(this.richTextBox1, "richTextBox1");
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxDescription);
			base.Controls.Add(this.groupBoxChannels);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "FlexrayChannelsGL4000";
			this.groupBoxChannels.ResumeLayout(false);
			this.tableLayoutPanelChannels.ResumeLayout(false);
			this.tableLayoutPanelChannels.PerformLayout();
			this.groupBoxDescription.ResumeLayout(false);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
