using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.CcpXcpSignalRequestsPage
{
	public class CcpXcpSignalListSelection : Form
	{
		private SplitButtonClientContainer splitButtonContainerStart;

		private SplitButtonClientContainer splitButtonContainerStop;

		private ActionCcpXcp currentAction;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private IContainer components;

		private TableLayoutPanel tableLayoutPanel1;

		private Label labelStart;

		private SplitButton splitButtonStart;

		private Label labelActivation;

		private SplitButton splitButtonStop;

		private Label labelStop;

		private System.Windows.Forms.ComboBox comboBoxActivation;

		private TextBox textBoxConditionStart;

		private TextBox textBoxConditionStop;

		private Button buttonEditConditionStart;

		private Button buttonEditConditionStop;

		private Button buttonOK;

		private Button buttonCancel;

		private Button buttonHelp;

		private Label labelStartDelay;

		private Label labelStopDelay;

		private Label labelDelayStartMs;

		private Label labelDelayStopMs;

		private Label labelActivationNote;

		private ErrorProvider errorProvider;

		private ErrorProvider errorProviderGlobalModel;

		private SpinEdit spinEditDelayStart;

		private SpinEdit spinEditDelayStop;

		private IModelValidator ModelValidator
		{
			get;
			set;
		}

		private IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		private string ResultActivationMode
		{
			get
			{
				return this.comboBoxActivation.Text;
			}
		}

		public CcpXcpSignalConfiguration SignalConfiguration
		{
			get;
			set;
		}

		public ActionCcpXcp ResultAction
		{
			get
			{
				this.currentAction.IsActive.Value = true;
				if (this.ResultActivationMode == Resources_CcpXcp.SignalActionActivationAlways)
				{
					this.currentAction.Event = null;
					this.currentAction.StopType = null;
				}
				else if (this.ResultActivationMode == Resources_CcpXcp.SignalActionActivationOff)
				{
					this.currentAction.IsActive.Value = false;
				}
				else if (this.ResultActivationMode == Resources_CcpXcp.SignalActionActivationTriggered)
				{
					this.currentAction.StopType = new StopImmediate();
				}
				return this.currentAction;
			}
		}

		public CcpXcpSignalListSelection(IModelValidator modelValidator, IApplicationDatabaseManager applicationDatabaseManager, CcpXcpSignalConfiguration signalConfiguration, ActionCcpXcp initialAction = null)
		{
			this.InitializeComponent();
			this.customErrorProvider = new CustomErrorProvider(this.errorProvider);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.FormatError, this.errorProvider);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.ModelValidator = modelValidator;
			this.ApplicationDatabaseManager = applicationDatabaseManager;
			this.SignalConfiguration = signalConfiguration;
			if (initialAction == null)
			{
				this.currentAction = CcpXcpSignalListSelection.CreateDefaultAction();
			}
			else
			{
				this.currentAction = new ActionCcpXcp(initialAction);
			}
			this.Init(initialAction);
			this.ApplyCurrentActionToControls();
			this.ValidateGlobalModelErrors();
		}

		private void Init(ActionCcpXcp initialAction)
		{
			this.comboBoxActivation.Items.Add(Resources_CcpXcp.SignalActionActivationOff);
			this.comboBoxActivation.Items.Add(Resources_CcpXcp.SignalActionActivationAlways);
			this.comboBoxActivation.Items.Add(Resources_CcpXcp.SignalActionActivationConditional);
			this.comboBoxActivation.Items.Add(Resources_CcpXcp.SignalActionActivationTriggered);
			this.splitButtonContainerStart = new SplitButtonClientContainer(initialAction, this.splitButtonStart, true, this.ModelValidator, this.ApplicationDatabaseManager);
			SplitButtonClientContainer expr_7D = this.splitButtonContainerStart;
			expr_7D.EventChanged = (SplitButtonClientContainer.EventChangedDelegate)Delegate.Combine(expr_7D.EventChanged, new SplitButtonClientContainer.EventChangedDelegate(this.ActionChangedStart));
			this.splitButtonContainerStop = new SplitButtonClientContainer(initialAction, this.splitButtonStop, false, this.ModelValidator, this.ApplicationDatabaseManager);
			SplitButtonClientContainer expr_C3 = this.splitButtonContainerStop;
			expr_C3.EventChanged = (SplitButtonClientContainer.EventChangedDelegate)Delegate.Combine(expr_C3.EventChanged, new SplitButtonClientContainer.EventChangedDelegate(this.ActionChangedStop));
			this.splitButtonContainerStart.ResultEvent = this.currentAction.Event;
			if (this.currentAction.StopType is StopOnEvent)
			{
				this.splitButtonContainerStop.ResultEvent = (this.currentAction.StopType as StopOnEvent).Event;
			}
			if (!this.currentAction.IsActive.Value)
			{
				this.comboBoxActivation.SelectedIndex = 0;
				return;
			}
			switch (this.currentAction.Mode)
			{
			case ActionCcpXcp.ActivationMode.Always:
				this.comboBoxActivation.SelectedIndex = 1;
				return;
			case ActionCcpXcp.ActivationMode.Triggered:
				this.comboBoxActivation.SelectedIndex = 3;
				return;
			case ActionCcpXcp.ActivationMode.Conditional:
				this.comboBoxActivation.SelectedIndex = 2;
				return;
			default:
				return;
			}
		}

		public static ActionCcpXcp CreateDefaultAction()
		{
			return new ActionCcpXcp(null)
			{
				IsActive = new ValidatedProperty<bool>(true),
				Event = null,
				StopType = null
			};
		}

		private void ApplyCurrentActionToControls()
		{
			this.textBoxConditionStart.Text = CcpXcpSignalRequestsGrid.CreateNameFromEvent(this.currentAction.Event, this.ModelValidator);
			this.spinEditDelayStart.Text = this.currentAction.StartDelay.Value.ToString();
			this.buttonEditConditionStart.Enabled = (this.splitButtonContainerStart.ResultEvent != null);
			this.textBoxConditionStop.Text = string.Empty;
			this.spinEditDelayStop.Text = this.currentAction.StopDelay.Value.ToString();
			this.buttonEditConditionStop.Enabled = (this.splitButtonContainerStop.ResultEvent != null);
			if (this.currentAction.StopType is StopOnEvent)
			{
				StopOnEvent stopOnEvent = (StopOnEvent)this.currentAction.StopType;
				this.textBoxConditionStop.Text = CcpXcpSignalRequestsGrid.CreateNameFromEvent(stopOnEvent.Event, this.ModelValidator);
				this.spinEditDelayStop.Text = this.currentAction.StopDelay.Value.ToString();
				this.labelStopDelay.Text = Resources.Delay + ":";
				return;
			}
			if (this.currentAction.StopType is StopOnTimer)
			{
				this.textBoxConditionStop.Text = Resources.Timer;
				this.spinEditDelayStop.Text = (this.currentAction.StopType as StopOnTimer).Duration.Value.ToString();
				this.labelStopDelay.Text = Resources.Duration + ":";
				this.buttonEditConditionStop.Enabled = false;
			}
		}

		private void ActionChangedStart()
		{
			this.currentAction.Event = this.splitButtonContainerStart.ResultEvent;
			this.ApplyCurrentActionToControls();
			this.ValidateStartEvent();
			this.ValidateGlobalModelErrors();
		}

		private void ActionChangedStop()
		{
			if (this.splitButtonContainerStop.IsStopOnTimerEvent)
			{
				if (this.currentAction.StopDelay.Value < 1u)
				{
					this.currentAction.StopDelay.Value = 1000u;
				}
				this.currentAction.StopType = new StopOnTimer(this.currentAction.StopDelay.Value);
			}
			else if (this.splitButtonContainerStop.ResultEvent != null)
			{
				this.currentAction.StopType = new StopOnEvent(this.splitButtonContainerStop.ResultEvent);
			}
			this.ApplyCurrentActionToControls();
			this.ValidateStopEvent();
			this.ValidateChildren();
			this.ValidateGlobalModelErrors();
		}

		private void comboBoxActivation_SelectedIndexChanged(object sender, EventArgs e)
		{
			string text = this.comboBoxActivation.Text;
			bool flag = false;
			bool flag2 = false;
			this.labelActivationNote.Visible = false;
			if (text == Resources_CcpXcp.SignalActionActivationOff)
			{
				flag = false;
				flag2 = false;
			}
			else if (text == Resources_CcpXcp.SignalActionActivationAlways)
			{
				flag = false;
				flag2 = false;
			}
			else if (text == Resources_CcpXcp.SignalActionActivationConditional)
			{
				flag = true;
				flag2 = true;
				this.labelActivationNote.Visible = true;
			}
			else if (text == Resources_CcpXcp.SignalActionActivationTriggered)
			{
				flag = true;
				flag2 = false;
				this.labelActivationNote.Visible = true;
			}
			this.splitButtonStart.Enabled = flag;
			this.textBoxConditionStart.Enabled = flag;
			this.buttonEditConditionStart.Enabled = (flag && this.splitButtonContainerStart.ResultEvent != null);
			this.spinEditDelayStart.Enabled = flag;
			this.splitButtonStop.Enabled = flag2;
			this.textBoxConditionStop.Enabled = flag2;
			this.buttonEditConditionStop.Enabled = (flag2 && this.splitButtonContainerStop.ResultEvent != null);
			this.spinEditDelayStop.Enabled = flag2;
			this.ResetValidation();
			this.ValidateGlobalModelErrors();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			if (!this.ValidateLocalErrors())
			{
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				base.DialogResult = DialogResult.None;
			}
		}

		private void CcpXcpSignalListSelection_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (base.DialogResult == DialogResult.OK && !this.ValidateLocalErrors())
			{
				e.Cancel = true;
			}
		}

		private void buttonEditConditionStart_Click(object sender, EventArgs e)
		{
			this.splitButtonContainerStart.EditEventCondition();
		}

		private void buttonEditConditionStop_Click(object sender, EventArgs e)
		{
			this.splitButtonContainerStop.EditEventCondition();
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void CcpXcpSignalListSelection_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private bool ValidateLocalErrors()
		{
			bool flag = true;
			flag &= this.ValidateStartEvent();
			flag &= this.ValidateStopEvent();
			return flag & this.ValidateChildren();
		}

		private bool ValidateStartEvent()
		{
			this.errorProvider.SetError(this.labelStart, "");
			if (this.IsStartEventActive() && this.currentAction.Event == null)
			{
				this.errorProvider.SetError(this.labelStart, Resources_CcpXcp.ErrorSignalActionNoStartEvent);
				return false;
			}
			return true;
		}

		private bool ValidateStopEvent()
		{
			this.errorProvider.SetError(this.labelStop, "");
			if (this.IsStopEventActive() && (this.currentAction.StopType == null || this.currentAction.StopType is StopImmediate))
			{
				this.errorProvider.SetError(this.labelStop, Resources_CcpXcp.ErrorSignalActionNoStopEvent);
				return false;
			}
			return true;
		}

		private void spinEditDelayStart_Validating(object sender, CancelEventArgs e)
		{
			int minimumTimerValue_ms = (int)Constants.MinimumTimerValue_ms;
			if (this.IsStartEventActive())
			{
				int num = (int)this.spinEditDelayStart.Value;
				int num2 = num % minimumTimerValue_ms;
				if (num2 > 0)
				{
					num = num / minimumTimerValue_ms * minimumTimerValue_ms;
					if (num2 > minimumTimerValue_ms / 2)
					{
						num += minimumTimerValue_ms;
					}
					this.spinEditDelayStart.Value = num;
				}
				if (this.currentAction.Event is CyclicTimerEvent && (ulong)(this.currentAction.Event as CyclicTimerEvent).CyclicTimeInMilliSec() < (ulong)((long)num))
				{
					this.errorProvider.SetError(this.labelDelayStartMs, Resources_CcpXcp.ErrorCcpXcpDelayLargerThanTimer);
					e.Cancel = true;
				}
			}
		}

		private void spinEditDelayStop_Validating(object sender, CancelEventArgs e)
		{
			uint minimumTimerValue_ms = Constants.MinimumTimerValue_ms;
			if (this.IsStopEventActive())
			{
				uint num = (uint)this.spinEditDelayStop.Value;
				uint num2 = num % minimumTimerValue_ms;
				if (num2 > 0u)
				{
					num = num / minimumTimerValue_ms * minimumTimerValue_ms;
					if (num2 > minimumTimerValue_ms / 2u)
					{
						num += minimumTimerValue_ms;
					}
					this.spinEditDelayStop.Value = num;
				}
				if (this.currentAction.StopType is StopOnTimer && num < 1u)
				{
					this.errorProvider.SetError(this.labelDelayStopMs, string.Format(Resources_CcpXcp.ErrorCcpXcpTimerDurationTooShort, minimumTimerValue_ms));
					e.Cancel = true;
				}
			}
		}

		private void spinEditDelayStart_Validated(object sender, EventArgs e)
		{
			this.currentAction.StartDelay.Value = (uint)this.spinEditDelayStart.Value;
			this.errorProvider.SetError(this.labelDelayStartMs, "");
		}

		private void spinEditDelayStop_Validated(object sender, EventArgs e)
		{
			this.currentAction.StopDelay.Value = (uint)this.spinEditDelayStop.Value;
			if (this.currentAction.StopType is StopOnTimer)
			{
				(this.currentAction.StopType as StopOnTimer).Duration.Value = this.currentAction.StopDelay.Value;
			}
			this.errorProvider.SetError(this.labelDelayStopMs, "");
		}

		private bool IsStartEventActive()
		{
			return this.ResultActivationMode == Resources_CcpXcp.SignalActionActivationTriggered || this.ResultActivationMode == Resources_CcpXcp.SignalActionActivationConditional;
		}

		private bool IsStopEventActive()
		{
			return this.ResultActivationMode == Resources_CcpXcp.SignalActionActivationConditional;
		}

		private void ResetValidation()
		{
			this.errorProvider.Clear();
			this.errorProviderGlobalModel.Clear();
		}

		private void ValidateGlobalModelErrors()
		{
			this.errorProviderGlobalModel.SetError(this.labelStart, "");
			this.errorProviderGlobalModel.SetError(this.labelStop, "");
			string value = "";
			if (this.IsStartEventActive() && this.HasEventGlobalModelErrors(this.currentAction.Event, out value))
			{
				this.errorProviderGlobalModel.SetError(this.labelStart, value);
			}
			value = "";
			if (this.currentAction.StopType is StopOnEvent && this.IsStopEventActive() && this.HasEventGlobalModelErrors((this.currentAction.StopType as StopOnEvent).Event, out value))
			{
				this.errorProviderGlobalModel.SetError(this.labelStop, value);
			}
		}

		private bool HasEventGlobalModelErrors(Event ev, out string errorText)
		{
			bool flag = this.ModelValidator.ValidateEvent(ev, this.pageValidator);
			if (flag)
			{
				errorText = "";
				return true;
			}
			errorText = string.Empty;
			if (ev is IdEvent)
			{
				if (this.pageValidator.General.HasError((ev as IdEvent).ChannelNumber))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as IdEvent).ChannelNumber);
					return true;
				}
			}
			else if (ev is CANDataEvent)
			{
				if (this.pageValidator.General.HasError((ev as CANDataEvent).ChannelNumber))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as CANDataEvent).ChannelNumber);
					return true;
				}
			}
			else if (ev is LINDataEvent)
			{
				if (this.pageValidator.General.HasError((ev as LINDataEvent).ChannelNumber))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as LINDataEvent).ChannelNumber);
					return true;
				}
			}
			else if (ev is SymbolicMessageEvent)
			{
				if (this.pageValidator.General.HasError((ev as SymbolicMessageEvent).MessageName))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as SymbolicMessageEvent).MessageName);
					return true;
				}
				if (this.pageValidator.General.HasError((ev as SymbolicMessageEvent).ChannelNumber))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as SymbolicMessageEvent).ChannelNumber);
					return true;
				}
			}
			else if (ev is SymbolicSignalEvent)
			{
				if (this.pageValidator.General.HasError((ev as SymbolicSignalEvent).SignalName))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as SymbolicSignalEvent).SignalName);
					return true;
				}
				if (this.pageValidator.General.HasError((ev as SymbolicSignalEvent).MessageName))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as SymbolicSignalEvent).MessageName);
					return true;
				}
				if (this.pageValidator.General.HasError((ev as SymbolicSignalEvent).ChannelNumber))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as SymbolicSignalEvent).ChannelNumber);
					return true;
				}
			}
			else if (ev is CcpXcpSignalEvent)
			{
				if (this.pageValidator.General.HasError((ev as CcpXcpSignalEvent).SignalName))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as CcpXcpSignalEvent).SignalName);
					return true;
				}
			}
			else if (ev is MsgTimeoutEvent)
			{
				if (this.pageValidator.General.HasError((ev as MsgTimeoutEvent).MessageName))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as MsgTimeoutEvent).MessageName);
					return true;
				}
				if (this.pageValidator.General.HasError((ev as MsgTimeoutEvent).ChannelNumber))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as MsgTimeoutEvent).ChannelNumber);
					return true;
				}
			}
			else if (ev is DigitalInputEvent)
			{
				if (this.pageValidator.General.HasError((ev as DigitalInputEvent).DigitalInput))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as DigitalInputEvent).DigitalInput);
					return true;
				}
			}
			else if (ev is KeyEvent && this.pageValidator.General.HasError((ev as KeyEvent).IsOnPanel))
			{
				errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as KeyEvent).IsOnPanel);
				return true;
			}
			return false;
		}

		private void spinEditDelayStart_InvalidValue(object sender, InvalidValueExceptionEventArgs e)
		{
			e.ExceptionMode = ExceptionMode.NoAction;
		}

		private void spinEditDelayStop_InvalidValue(object sender, InvalidValueExceptionEventArgs e)
		{
			e.ExceptionMode = ExceptionMode.NoAction;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CcpXcpSignalListSelection));
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.labelStart = new Label();
			this.splitButtonStart = new SplitButton();
			this.labelActivation = new Label();
			this.splitButtonStop = new SplitButton();
			this.labelStop = new Label();
			this.comboBoxActivation = new System.Windows.Forms.ComboBox();
			this.textBoxConditionStart = new TextBox();
			this.textBoxConditionStop = new TextBox();
			this.buttonEditConditionStart = new Button();
			this.buttonEditConditionStop = new Button();
			this.labelStartDelay = new Label();
			this.labelStopDelay = new Label();
			this.labelDelayStartMs = new Label();
			this.labelDelayStopMs = new Label();
			this.labelActivationNote = new Label();
			this.spinEditDelayStart = new SpinEdit();
			this.spinEditDelayStop = new SpinEdit();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			this.buttonHelp = new Button();
			this.errorProvider = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			((ISupportInitialize)this.spinEditDelayStart.Properties).BeginInit();
			((ISupportInitialize)this.spinEditDelayStop.Properties).BeginInit();
			((ISupportInitialize)this.errorProvider).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.labelStart, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.splitButtonStart, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelActivation, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.splitButtonStop, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelStop, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.comboBoxActivation, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.textBoxConditionStart, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.textBoxConditionStop, 2, 2);
			this.tableLayoutPanel1.Controls.Add(this.buttonEditConditionStart, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.buttonEditConditionStop, 3, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelStartDelay, 4, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelStopDelay, 4, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelDelayStartMs, 6, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelDelayStopMs, 6, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelActivationNote, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.spinEditDelayStart, 5, 1);
			this.tableLayoutPanel1.Controls.Add(this.spinEditDelayStop, 5, 2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.labelStart, "labelStart");
			this.labelStart.Name = "labelStart";
			componentResourceManager.ApplyResources(this.splitButtonStart, "splitButtonStart");
			this.splitButtonStart.Name = "splitButtonStart";
			this.splitButtonStart.ShowSplitAlways = true;
			this.splitButtonStart.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.labelActivation, "labelActivation");
			this.labelActivation.Name = "labelActivation";
			componentResourceManager.ApplyResources(this.splitButtonStop, "splitButtonStop");
			this.splitButtonStop.Name = "splitButtonStop";
			this.splitButtonStop.ShowSplitAlways = true;
			this.splitButtonStop.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.labelStop, "labelStop");
			this.labelStop.Name = "labelStop";
			componentResourceManager.ApplyResources(this.comboBoxActivation, "comboBoxActivation");
			this.comboBoxActivation.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxActivation.FormattingEnabled = true;
			this.comboBoxActivation.Name = "comboBoxActivation";
			this.comboBoxActivation.SelectedIndexChanged += new EventHandler(this.comboBoxActivation_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.textBoxConditionStart, "textBoxConditionStart");
			this.textBoxConditionStart.Name = "textBoxConditionStart";
			this.textBoxConditionStart.ReadOnly = true;
			componentResourceManager.ApplyResources(this.textBoxConditionStop, "textBoxConditionStop");
			this.textBoxConditionStop.Name = "textBoxConditionStop";
			this.textBoxConditionStop.ReadOnly = true;
			componentResourceManager.ApplyResources(this.buttonEditConditionStart, "buttonEditConditionStart");
			this.buttonEditConditionStart.Name = "buttonEditConditionStart";
			this.buttonEditConditionStart.UseVisualStyleBackColor = true;
			this.buttonEditConditionStart.Click += new EventHandler(this.buttonEditConditionStart_Click);
			componentResourceManager.ApplyResources(this.buttonEditConditionStop, "buttonEditConditionStop");
			this.buttonEditConditionStop.Name = "buttonEditConditionStop";
			this.buttonEditConditionStop.UseVisualStyleBackColor = true;
			this.buttonEditConditionStop.Click += new EventHandler(this.buttonEditConditionStop_Click);
			componentResourceManager.ApplyResources(this.labelStartDelay, "labelStartDelay");
			this.labelStartDelay.Name = "labelStartDelay";
			componentResourceManager.ApplyResources(this.labelStopDelay, "labelStopDelay");
			this.labelStopDelay.Name = "labelStopDelay";
			componentResourceManager.ApplyResources(this.labelDelayStartMs, "labelDelayStartMs");
			this.labelDelayStartMs.Name = "labelDelayStartMs";
			componentResourceManager.ApplyResources(this.labelDelayStopMs, "labelDelayStopMs");
			this.labelDelayStopMs.Name = "labelDelayStopMs";
			componentResourceManager.ApplyResources(this.labelActivationNote, "labelActivationNote");
			this.tableLayoutPanel1.SetColumnSpan(this.labelActivationNote, 5);
			this.labelActivationNote.Name = "labelActivationNote";
			componentResourceManager.ApplyResources(this.spinEditDelayStart, "spinEditDelayStart");
			this.spinEditDelayStart.Name = "spinEditDelayStart";
			this.spinEditDelayStart.Properties.AutoHeight = (bool)componentResourceManager.GetObject("spinEditDelayStart.Properties.AutoHeight");
			this.spinEditDelayStart.Properties.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			RepositoryItemSpinEdit arg_669_0 = this.spinEditDelayStart.Properties;
			int[] array = new int[4];
			array[0] = 100;
			arg_669_0.Increment = new decimal(array);
			this.spinEditDelayStart.Properties.IsFloatValue = false;
			this.spinEditDelayStart.Properties.Mask.EditMask = componentResourceManager.GetString("spinEditDelayStart.Properties.Mask.EditMask");
			RepositoryItemSpinEdit arg_6BF_0 = this.spinEditDelayStart.Properties;
			int[] array2 = new int[4];
			array2[0] = 3000000;
			arg_6BF_0.MaxValue = new decimal(array2);
			this.spinEditDelayStart.InvalidValue += new InvalidValueExceptionEventHandler(this.spinEditDelayStart_InvalidValue);
			this.spinEditDelayStart.Validating += new CancelEventHandler(this.spinEditDelayStart_Validating);
			this.spinEditDelayStart.Validated += new EventHandler(this.spinEditDelayStart_Validated);
			componentResourceManager.ApplyResources(this.spinEditDelayStop, "spinEditDelayStop");
			this.spinEditDelayStop.Name = "spinEditDelayStop";
			this.spinEditDelayStop.Properties.AutoHeight = (bool)componentResourceManager.GetObject("spinEditDelayStop.Properties.AutoHeight");
			this.spinEditDelayStop.Properties.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.spinEditDelayStop.Properties.DisplayFormat.FormatString = "{0}";
			this.spinEditDelayStop.Properties.DisplayFormat.FormatType = FormatType.Numeric;
			RepositoryItemSpinEdit arg_7C6_0 = this.spinEditDelayStop.Properties;
			int[] array3 = new int[4];
			array3[0] = 100;
			arg_7C6_0.Increment = new decimal(array3);
			this.spinEditDelayStop.Properties.IsFloatValue = false;
			this.spinEditDelayStop.Properties.Mask.EditMask = componentResourceManager.GetString("spinEditDelayStop.Properties.Mask.EditMask");
			RepositoryItemSpinEdit arg_81F_0 = this.spinEditDelayStop.Properties;
			int[] array4 = new int[4];
			array4[0] = 3000000;
			arg_81F_0.MaxValue = new decimal(array4);
			this.spinEditDelayStop.InvalidValue += new InvalidValueExceptionEventHandler(this.spinEditDelayStop_InvalidValue);
			this.spinEditDelayStop.Validating += new CancelEventHandler(this.spinEditDelayStop_Validating);
			this.spinEditDelayStop.Validated += new EventHandler(this.spinEditDelayStop_Validated);
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.DialogResult = DialogResult.OK;
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.CausesValidation = false;
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProvider.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.tableLayoutPanel1);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CcpXcpSignalListSelection";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.FormClosing += new FormClosingEventHandler(this.CcpXcpSignalListSelection_FormClosing);
			base.HelpRequested += new HelpEventHandler(this.CcpXcpSignalListSelection_HelpRequested);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((ISupportInitialize)this.spinEditDelayStart.Properties).EndInit();
			((ISupportInitialize)this.spinEditDelayStop.Properties).EndInit();
			((ISupportInitialize)this.errorProvider).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
