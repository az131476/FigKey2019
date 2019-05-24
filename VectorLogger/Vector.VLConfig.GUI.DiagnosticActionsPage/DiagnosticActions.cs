using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.EventConditions;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.DiagnosticActionsPage
{
	public class DiagnosticActions : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<DiagnosticActionsConfiguration>, IUpdateObserver<DiagnosticsDatabaseConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver<LoggerType>, IUpdateObserver, ISplitButtonExClient
	{
		public delegate void DataChangedHandler(DiagnosticActionsConfiguration data);

		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private LoggerType loggerType;

		private readonly SplitButtonEx mSplitButtonEx;

		private IContainer components;

		private DiagnosticActionsGrid diagnosticActionsGrid;

		private GroupBox groupBoxDiagActions;

		private TableLayoutPanel tableLayoutPanel1;

		private SplitButton mSplitButton;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private Button buttonEditEvent;

		private Label labelEvent;

		private Label labelRequest;

		private GroupBox groupBoxGeneral;

		private GeneralSettingsGL1000 generalSettingsGL1000;

		private GeneralSettingsGL3Plus generalSettingsGL3Plus;

		private SplitButton mSplitButtonAddRequest;

		private Button buttonRemove;

		private Button buttonRemoveAction;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.diagnosticActionsGrid.ApplicationDatabaseManager;
			}
			set
			{
				this.diagnosticActionsGrid.ApplicationDatabaseManager = value;
			}
		}

		public IDiagSymbolsManager DiagSymbolsManager
		{
			get
			{
				return this.diagnosticActionsGrid.DiagSymbolsManager;
			}
			set
			{
				this.diagnosticActionsGrid.DiagSymbolsManager = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.diagnosticActionsGrid.ModelValidator;
			}
			set
			{
				this.diagnosticActionsGrid.ModelValidator = value;
				this.generalSettingsGL1000.ModelValidator = value;
				this.generalSettingsGL3Plus.ModelValidator = value;
			}
		}

		ISemanticChecker IPropertyWindow.SemanticChecker
		{
			get;
			set;
		}

		IModelEditor IPropertyWindow.ModelEditor
		{
			get;
			set;
		}

		IUpdateService IPropertyWindow.UpdateService
		{
			get;
			set;
		}

		IUpdateObserver IPropertyWindow.UpdateObserver
		{
			get
			{
				return this;
			}
		}

		PageType IPropertyWindow.Type
		{
			get
			{
				return PageType.DiagnosticActions;
			}
		}

		bool IPropertyWindow.IsVisible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				bool visible = base.Visible;
				base.Visible = value;
				if (!visible && base.Visible)
				{
					this.diagnosticActionsGrid.DisplayErrors();
				}
			}
		}

		public List<Vector.VLConfig.Data.ConfigurationDataModel.Action> FocusedActions
		{
			get
			{
				return null;
			}
		}

		public SplitButton SplitButton
		{
			get
			{
				return this.mSplitButton;
			}
		}

		public string SplitButtonEmptyDefault
		{
			get
			{
				return Resources.SplitButtonEmptyDefault;
			}
		}

		public DiagnosticActions()
		{
			this.InitializeComponent();
			this.diagnosticActionsGrid.SelectionChanged += new EventHandler(this.OnDiagnosticActionsGridSelectionChanged);
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.mSplitButtonEx = new SplitButtonEx(this);
			this.mSplitButtonAddRequest.SplitMenuStrip = new ContextMenuStrip();
			this.mSplitButtonAddRequest.SplitMenuStrip.Items.Add(new ToolStripMenuItem(Resources.SignalRequest, Resources.ImageDiagSignal, new EventHandler(this.AddSignalRequest)));
			this.mSplitButtonAddRequest.SplitMenuStrip.Items.Add(new ToolStripMenuItem(Resources.ServiceRequest, Resources.ImageDiagService, new EventHandler(this.AddServiceRequest)));
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is DiagnosticActionsConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.DiagnosticActions);
			}
			this.mSplitButton.AutoSize = false;
			GUIUtil.InitSplitButtonMenuEventTypes(this.mSplitButtonEx);
			this.diagnosticActionsGrid.Init();
		}

		void IPropertyWindow.Reset()
		{
			this.mSplitButtonEx.UpdateSplitMenu();
			this.ResetValidationFramework();
			this.diagnosticActionsGrid.Reset();
			this.generalSettingsGL1000.Reset();
			this.generalSettingsGL3Plus.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			if (!this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.Recording.IsDiagnosticsSupported)
			{
				return true;
			}
			bool flag = true;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL2000:
				flag &= this.generalSettingsGL1000.ValidateInput();
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				flag &= this.generalSettingsGL3Plus.ValidateInput();
				break;
			case LoggerType.VN1630log:
				return true;
			}
			flag &= this.diagnosticActionsGrid.ValidateInput(false);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
		}

		bool IPropertyWindow.HasErrors()
		{
			if (!this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.Recording.IsDiagnosticsSupported)
			{
				return false;
			}
			bool flag = this.pageValidator.General.HasErrors(Enum.GetValues(typeof(ValidationErrorClass)) as ValidationErrorClass[]);
			flag |= this.diagnosticActionsGrid.HasErrors();
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL2000:
				flag |= this.generalSettingsGL1000.HasErrors();
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				flag |= this.generalSettingsGL3Plus.HasErrors();
				break;
			case LoggerType.VN1630log:
				flag = false;
				break;
			}
			return flag;
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			if (!this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.Recording.IsDiagnosticsSupported)
			{
				return false;
			}
			bool flag = this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.GlobalModelError
			});
			flag |= this.diagnosticActionsGrid.HasGlobalErrors();
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL2000:
				flag |= this.generalSettingsGL1000.HasGlobalErrors();
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				flag |= this.generalSettingsGL3Plus.HasGlobalErrors();
				break;
			case LoggerType.VN1630log:
				flag = false;
				break;
			}
			return flag;
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			if (!this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.Recording.IsDiagnosticsSupported)
			{
				return false;
			}
			bool flag = this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.FormatError,
				ValidationErrorClass.LocalModelError
			});
			flag |= this.diagnosticActionsGrid.HasLocalErrors();
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL2000:
				flag |= this.generalSettingsGL1000.HasLocalErrors();
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				flag |= this.generalSettingsGL3Plus.HasLocalErrors();
				break;
			case LoggerType.VN1630log:
				flag = false;
				break;
			}
			return flag;
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			if (!this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.Recording.IsDiagnosticsSupported)
			{
				return false;
			}
			IPageValidatorGeneral arg_34_0 = this.pageValidator.General;
			ValidationErrorClass[] errorClasses = new ValidationErrorClass[1];
			bool flag = arg_34_0.HasErrors(errorClasses);
			flag |= this.diagnosticActionsGrid.HasFormatErrors();
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL2000:
				flag |= this.generalSettingsGL1000.HasFormatErrors();
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				flag |= this.generalSettingsGL3Plus.HasFormatErrors();
				break;
			case LoggerType.VN1630log:
				flag = false;
				break;
			}
			return flag;
		}

		private void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
		}

		void IUpdateObserver<DiagnosticActionsConfiguration>.Update(DiagnosticActionsConfiguration data)
		{
			this.mSplitButtonEx.UpdateSplitMenu();
			this.diagnosticActionsGrid.DiagnosticActionsConfiguration = data;
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL2000:
				this.generalSettingsGL1000.DiagnosticActionsConfiguration = data;
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.generalSettingsGL3Plus.DiagnosticActionsConfiguration = data;
				break;
			}
			((IPropertyWindow)this).ValidateInput();
		}

		void IUpdateObserver<DiagnosticsDatabaseConfiguration>.Update(DiagnosticsDatabaseConfiguration data)
		{
			if (this.loggerType != LoggerType.GL1020FTE)
			{
				this.diagnosticActionsGrid.DiagnosticDatabaseConfiguration = data;
			}
		}

		void IUpdateObserver<DisplayMode>.Update(DisplayMode data)
		{
			if (!this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.Recording.IsDiagnosticsSupported)
			{
				return;
			}
			this.diagnosticActionsGrid.DisplayMode = data;
			this.generalSettingsGL1000.DisplayMode = data;
			this.generalSettingsGL3Plus.DisplayMode = data;
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType data)
		{
			if (this.loggerType == data)
			{
				return;
			}
			this.loggerType = data;
			this.generalSettingsGL1000.Visible = false;
			this.generalSettingsGL3Plus.Visible = false;
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
			case LoggerType.GL2000:
				this.generalSettingsGL1000.Visible = true;
				return;
			case LoggerType.GL1020FTE:
			case LoggerType.VN1630log:
				break;
			case LoggerType.GL3000:
			case LoggerType.GL4000:
				this.generalSettingsGL3Plus.Visible = true;
				break;
			default:
				return;
			}
		}

		public ConfigClipboardManager.AcceptType Accept(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			return ConfigClipboardManager.AcceptType.None;
		}

		public bool Insert(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			return false;
		}

		public bool Insert(Event evt)
		{
			return false;
		}

		private void OnDiagnosticActionsGridSelectionChanged(object sender, EventArgs e)
		{
			this.buttonEditEvent.Enabled = false;
			this.buttonRemove.Enabled = false;
			this.mSplitButtonAddRequest.Enabled = false;
			this.buttonRemoveAction.Enabled = false;
			DiagnosticAction diagnosticAction = null;
			if (this.diagnosticActionsGrid.TryGetSelectedAction(out diagnosticAction))
			{
				this.buttonEditEvent.Enabled = this.diagnosticActionsGrid.IsGroupRowSelected;
				this.buttonRemove.Enabled = this.diagnosticActionsGrid.IsGroupRowSelected;
				this.mSplitButtonAddRequest.Enabled = this.diagnosticActionsGrid.DiagnosticActionsConfiguration.DiagnosticActions.Any<DiagnosticAction>();
				this.buttonRemoveAction.Enabled = (!this.diagnosticActionsGrid.IsGroupRowSelected && !(diagnosticAction is DiagnosticDummyAction));
			}
		}

		private void mSplitButtonAddRequest_Click(object sender, EventArgs e)
		{
			this.mSplitButtonAddRequest.ContextMenuStrip.Show(this.mSplitButtonAddRequest, new Point(0, this.mSplitButtonAddRequest.Bottom - this.mSplitButtonAddRequest.Top));
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			this.diagnosticActionsGrid.RemoveActionSequence(true);
		}

		private void buttonEditEvent_Click(object sender, EventArgs e)
		{
			this.diagnosticActionsGrid.EditActionSequenceEventCondition();
		}

		private void buttonRemoveAction_Click(object sender, EventArgs e)
		{
			this.diagnosticActionsGrid.RemoveAction();
		}

		public bool IsItemVisible(ToolStripItem item)
		{
			string text = item.Text;
			if (text == Vocabulary.TriggerTypeNameColOnStart || text == Resources_Trigger.TriggerTypeNameColOnCycTimer)
			{
				return true;
			}
			if (text == Resources_Trigger.TriggerTypeNameColCANData || text == Vocabulary.TriggerTypeNameColCANId || text == Resources_Trigger.TriggerTypeNameColCANMsgTimeout || text == Resources_Trigger.TriggerTypeNameColSymbolicCAN || text == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
			{
				return this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColLINData || text == Vocabulary.TriggerTypeNameColLINId || text == Resources_Trigger.TriggerTypeNameColLINMsgTimeout || text == Resources_Trigger.TriggerTypeNameColSymbolicLIN || text == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
			{
				return this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.LIN.NumberOfChannels > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColFlexray || text == Resources_Trigger.TriggerTypeNameColSymbolicFlexray || text == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
			{
				return this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColKey)
			{
				return this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.IO.NumberOfKeys > 0u || this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.IO.NumberOfPanelKeys > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColAnalogInput)
			{
				return this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.IO.NumberOfAnalogInputs > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColDigitalInput)
			{
				return this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.IO.NumberOfDigitalInputs > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColIgnition)
			{
				return this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.Recording.IsIgnitionEventSupported;
			}
			if (text == Resources_Trigger.TriggerTypeNameColVoCanRecording)
			{
				return false;
			}
			if (text == Resources_Trigger.TriggerTypeNameColCcpXcpSignal)
			{
				return this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.Recording.IsCCPXCPSignalEventSupported;
			}
			return text == Resources_Trigger.TriggerTypeNameColDiagnosticSignal && this.diagnosticActionsGrid.ModelValidator.LoggerSpecifics.Recording.IsDiagnosticsSupported;
		}

		public void ItemClicked(ToolStripItem item)
		{
			this.AddItem(item.Text);
		}

		public void DefaultActionClicked()
		{
			this.AddItem(this.mSplitButtonEx.DefaultAction);
		}

		private void AddItem(string itemText)
		{
			if (!this.diagnosticActionsGrid.ModelValidator.HasDiagnosticsDatabasesConfigured())
			{
				InformMessageBox.Error(Resources.ErrorUnableToCreateServReqSeq);
				return;
			}
			TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence = this.CreateActionSequence(itemText);
			if (triggeredDiagnosticActionSequence != null)
			{
				this.diagnosticActionsGrid.AddActionSequence(triggeredDiagnosticActionSequence);
			}
		}

		private void AddSignalRequest(object sender, EventArgs e)
		{
			this.UpdateRequestSplitButton(false);
			this.diagnosticActionsGrid.InsertSignalRequest(true);
		}

		private void AddServiceRequest(object sender, EventArgs e)
		{
			this.UpdateRequestSplitButton(true);
			this.diagnosticActionsGrid.InsertAction(true);
		}

		private void UpdateRequestSplitButton(bool serviceRequestUsed)
		{
			this.mSplitButtonAddRequest.Click -= new EventHandler(this.AddSignalRequest);
			this.mSplitButtonAddRequest.Click -= new EventHandler(this.AddServiceRequest);
			this.mSplitButtonAddRequest.Click -= new EventHandler(this.mSplitButtonAddRequest_Click);
			if (serviceRequestUsed)
			{
				this.mSplitButtonAddRequest.Click += new EventHandler(this.AddServiceRequest);
				this.mSplitButtonAddRequest.Text = Resources.ServiceRequest;
				return;
			}
			this.mSplitButtonAddRequest.Click += new EventHandler(this.AddSignalRequest);
			this.mSplitButtonAddRequest.Text = Resources.SignalRequest;
		}

		private TriggeredDiagnosticActionSequence CreateActionSequence(string eventTypeName)
		{
			if (eventTypeName == Vocabulary.TriggerTypeNameColOnStart)
			{
				return this.CreateOnStartSequence();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColOnCycTimer)
			{
				return this.CreateCyclicTimerSequence();
			}
			if (eventTypeName == Vocabulary.TriggerTypeNameColCANId)
			{
				return this.CreateCANIdSequence();
			}
			if (eventTypeName == Vocabulary.TriggerTypeNameColLINId)
			{
				return this.CreateLINIdSequence();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColCANData)
			{
				return this.CreateCANDataSequence();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColLINData)
			{
				return this.CreateLINDataSequence();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColFlexray)
			{
				return this.CreateFlexrayIdSequence();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicCAN)
			{
				return this.CreateSymMessageSequence(BusType.Bt_CAN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicLIN)
			{
				return this.CreateSymMessageSequence(BusType.Bt_LIN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicFlexray)
			{
				return this.CreateSymMessageSequence(BusType.Bt_FlexRay);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
			{
				return this.CreateSymSignalSequence(BusType.Bt_CAN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
			{
				return this.CreateSymSignalSequence(BusType.Bt_LIN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
			{
				return this.CreateSymSignalSequence(BusType.Bt_FlexRay);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColCANMsgTimeout)
			{
				return this.CreateMsgTimeoutSequence(BusType.Bt_CAN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColLINMsgTimeout)
			{
				return this.CreateMsgTimeoutSequence(BusType.Bt_LIN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColDigitalInput)
			{
				return this.CreateDigitalInputSequence();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColAnalogInput)
			{
				return this.CreateAnalogInputSequence();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColKey)
			{
				return this.CreateKeySequence();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColIgnition)
			{
				return this.CreateIgnitionSequence();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColCcpXcpSignal)
			{
				return this.CreateCcpXcpSignalSequence();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColDiagnosticSignal)
			{
				return this.CreateDiagnosticSignalSequence();
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateOnStartSequence()
		{
			OnStartCondition onStartConditionDialog = this.diagnosticActionsGrid.GetOnStartConditionDialog();
			if (DialogResult.OK == onStartConditionDialog.ShowDialog())
			{
				return new TriggeredDiagnosticActionSequence(new OnStartEvent
				{
					Delay = 
					{
						Value = onStartConditionDialog.Delay
					}
				});
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateCyclicTimerSequence()
		{
			CyclicTimerCondition cyclicTimerConditionDialog = this.diagnosticActionsGrid.GetCyclicTimerConditionDialog();
			if (DialogResult.OK == cyclicTimerConditionDialog.ShowDialog())
			{
				CyclicTimerEvent ev = new CyclicTimerEvent(cyclicTimerConditionDialog.CyclicTimerEvent);
				return new TriggeredDiagnosticActionSequence(ev);
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateCANIdSequence()
		{
			CANIdCondition cANIdConditionDialog = this.diagnosticActionsGrid.GetCANIdConditionDialog();
			if (DialogResult.OK == cANIdConditionDialog.ShowDialog())
			{
				CANIdEvent ev = new CANIdEvent(cANIdConditionDialog.CANIdEvent);
				return new TriggeredDiagnosticActionSequence(ev);
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateLINIdSequence()
		{
			LINIdCondition lINIdConditionDialog = this.diagnosticActionsGrid.GetLINIdConditionDialog();
			if (DialogResult.OK == lINIdConditionDialog.ShowDialog())
			{
				LINIdEvent ev = new LINIdEvent(lINIdConditionDialog.LINIdEvent);
				return new TriggeredDiagnosticActionSequence(ev);
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateFlexrayIdSequence()
		{
			FlexrayIdCondition flexrayIdConditionDialog = this.diagnosticActionsGrid.GetFlexrayIdConditionDialog();
			if (DialogResult.OK == flexrayIdConditionDialog.ShowDialog())
			{
				FlexrayIdEvent ev = new FlexrayIdEvent(flexrayIdConditionDialog.FlexrayIdEvent);
				return new TriggeredDiagnosticActionSequence(ev);
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateCANDataSequence()
		{
			CANDataCondition cANDataConditionDialog = this.diagnosticActionsGrid.GetCANDataConditionDialog();
			if (DialogResult.OK == cANDataConditionDialog.ShowDialog())
			{
				CANDataEvent ev = new CANDataEvent(cANDataConditionDialog.CANDataEvent);
				return new TriggeredDiagnosticActionSequence(ev);
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateLINDataSequence()
		{
			LINDataCondition lINDataConditionDialog = this.diagnosticActionsGrid.GetLINDataConditionDialog();
			if (DialogResult.OK == lINDataConditionDialog.ShowDialog())
			{
				LINDataEvent ev = new LINDataEvent(lINDataConditionDialog.LINDataEvent);
				return new TriggeredDiagnosticActionSequence(ev);
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateSymMessageSequence(BusType busType)
		{
			if (!this.diagnosticActionsGrid.ModelValidator.DatabaseServices.HasDatabasesConfiguredFor(busType))
			{
				string arg = Vocabulary.CAN;
				if (busType == BusType.Bt_LIN)
				{
					arg = Vocabulary.LIN;
				}
				else if (busType == BusType.Bt_FlexRay)
				{
					arg = Vocabulary.Flexray;
				}
				InformMessageBox.Info(string.Format(Resources.NoDatabaseAvailableForBustype, arg));
				return null;
			}
			string text = "";
			string text2 = "";
			string value = "";
			string text3 = "";
			bool value2 = false;
			if (!this.diagnosticActionsGrid.ApplicationDatabaseManager.SelectMessageInDatabase(ref text, ref value, ref text2, ref text3, ref busType, ref value2))
			{
				return null;
			}
			string message;
			if (!this.diagnosticActionsGrid.ModelValidator.DatabaseServices.IsSymbolicMessageInsertAllowed(text, text3, text2, busType, out message))
			{
				InformMessageBox.Error(message);
				return null;
			}
			SymbolicMessageEvent symbolicMessageEvent = new SymbolicMessageEvent();
			symbolicMessageEvent.MessageName.Value = text;
			symbolicMessageEvent.DatabaseName.Value = value;
			symbolicMessageEvent.DatabasePath.Value = this.diagnosticActionsGrid.ModelValidator.GetFilePathRelativeToConfiguration(text2);
			symbolicMessageEvent.NetworkName.Value = text3;
			symbolicMessageEvent.BusType.Value = busType;
			symbolicMessageEvent.IsFlexrayPDU.Value = value2;
			symbolicMessageEvent.ChannelNumber.Value = 1u;
			IList<uint> channelAssignmentOfDatabase = this.diagnosticActionsGrid.ModelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(symbolicMessageEvent.DatabasePath.Value, text3);
			if (channelAssignmentOfDatabase.Count > 0)
			{
				symbolicMessageEvent.ChannelNumber.Value = channelAssignmentOfDatabase[0];
				if (symbolicMessageEvent.ChannelNumber.Value == Database.ChannelNumber_FlexrayAB && BusType.Bt_FlexRay == busType)
				{
					symbolicMessageEvent.ChannelNumber.Value = 1u;
					if (text.EndsWith(Constants.FlexrayChannelB_Postfix))
					{
						symbolicMessageEvent.ChannelNumber.Value = 2u;
					}
				}
			}
			return new TriggeredDiagnosticActionSequence(symbolicMessageEvent);
		}

		private TriggeredDiagnosticActionSequence CreateSymSignalSequence(BusType busType)
		{
			if (!this.diagnosticActionsGrid.ModelValidator.DatabaseServices.HasDatabasesConfiguredFor(busType))
			{
				string arg = Vocabulary.CAN;
				if (busType == BusType.Bt_LIN)
				{
					arg = Vocabulary.LIN;
				}
				else if (busType == BusType.Bt_FlexRay)
				{
					arg = Vocabulary.Flexray;
				}
				InformMessageBox.Info(string.Format(Resources.NoDatabaseAvailableForBustype, arg));
				return null;
			}
			string text = "";
			string text2 = "";
			string text3 = "";
			string value = "";
			string text4 = "";
			bool value2 = false;
			if (this.diagnosticActionsGrid.ApplicationDatabaseManager.SelectSignalInDatabase(ref text, ref text2, ref value, ref text3, ref text4, ref busType, ref value2))
			{
				string message;
				if (!this.diagnosticActionsGrid.ModelValidator.DatabaseServices.IsSymbolicSignalInsertAllowed(text2, text, text4, text3, busType, out message))
				{
					InformMessageBox.Error(message);
					return null;
				}
				SymbolicSignalEvent symbolicSignalEvent = new SymbolicSignalEvent();
				symbolicSignalEvent.MessageName.Value = text;
				symbolicSignalEvent.SignalName.Value = text2;
				symbolicSignalEvent.DatabaseName.Value = value;
				symbolicSignalEvent.DatabasePath.Value = this.diagnosticActionsGrid.ModelValidator.GetFilePathRelativeToConfiguration(text3);
				symbolicSignalEvent.NetworkName.Value = text4;
				symbolicSignalEvent.ChannelNumber.Value = 1u;
				IList<uint> channelAssignmentOfDatabase = this.diagnosticActionsGrid.ModelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(symbolicSignalEvent.DatabasePath.Value, text4);
				if (channelAssignmentOfDatabase.Count > 0)
				{
					symbolicSignalEvent.ChannelNumber.Value = channelAssignmentOfDatabase[0];
					if (symbolicSignalEvent.ChannelNumber.Value == Database.ChannelNumber_FlexrayAB && BusType.Bt_FlexRay == busType)
					{
						symbolicSignalEvent.ChannelNumber.Value = 1u;
						if (text.EndsWith(Constants.FlexrayChannelB_Postfix))
						{
							symbolicSignalEvent.ChannelNumber.Value = 2u;
						}
					}
				}
				symbolicSignalEvent.BusType.Value = busType;
				symbolicSignalEvent.IsFlexrayPDU.Value = value2;
				symbolicSignalEvent.LowValue.Value = 0.0;
				symbolicSignalEvent.HighValue.Value = 0.0;
				symbolicSignalEvent.Relation.Value = CondRelation.Equal;
				using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.diagnosticActionsGrid.ModelValidator, this.ApplicationDatabaseManager, null))
				{
					symbolicSignalCondition.SignalEvent = new SymbolicSignalEvent(symbolicSignalEvent);
					if (DialogResult.OK == symbolicSignalCondition.ShowDialog())
					{
						symbolicSignalEvent.Assign(symbolicSignalCondition.SignalEvent);
						return new TriggeredDiagnosticActionSequence(symbolicSignalEvent);
					}
				}
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateMsgTimeoutSequence(BusType busType)
		{
			MsgTimeoutCondition msgTimeoutConditionDialog = this.diagnosticActionsGrid.GetMsgTimeoutConditionDialog();
			msgTimeoutConditionDialog.InitDefaultValues(busType);
			if (DialogResult.OK == msgTimeoutConditionDialog.ShowDialog())
			{
				MsgTimeoutEvent ev = new MsgTimeoutEvent(msgTimeoutConditionDialog.MsgTimeoutEvent);
				return new TriggeredDiagnosticActionSequence(ev);
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateDigitalInputSequence()
		{
			DigitalInputCondition digitalInputConditionDialog = this.diagnosticActionsGrid.GetDigitalInputConditionDialog();
			if (DialogResult.OK == digitalInputConditionDialog.ShowDialog())
			{
				DigitalInputEvent ev = new DigitalInputEvent(digitalInputConditionDialog.DigitalInputEvent);
				return new TriggeredDiagnosticActionSequence(ev);
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateAnalogInputSequence()
		{
			AnalogInputCondition analogInputConditionDialog = this.diagnosticActionsGrid.GetAnalogInputConditionDialog();
			if (DialogResult.OK == analogInputConditionDialog.ShowDialog())
			{
				AnalogInputEvent ev = new AnalogInputEvent(analogInputConditionDialog.AnalogInputEvent);
				return new TriggeredDiagnosticActionSequence(ev);
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateKeySequence()
		{
			KeyCondition keyConditionDialog = this.diagnosticActionsGrid.GetKeyConditionDialog();
			if (DialogResult.OK == keyConditionDialog.ShowDialog())
			{
				KeyEvent ev = new KeyEvent(keyConditionDialog.KeyEvent);
				return new TriggeredDiagnosticActionSequence(ev);
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateIgnitionSequence()
		{
			IgnitionCondition ignitionConditionDialog = this.diagnosticActionsGrid.GetIgnitionConditionDialog();
			if (DialogResult.OK == ignitionConditionDialog.ShowDialog())
			{
				IgnitionEvent ev = new IgnitionEvent(ignitionConditionDialog.IgnitionEvent);
				return new TriggeredDiagnosticActionSequence(ev);
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateCcpXcpSignalSequence()
		{
			if (!CcpXcpManager.Instance().CheckTriggerSignalEvents())
			{
				return null;
			}
			RecordTrigger recordTrigger = RecordTrigger.CreateCcpXcpSignalTrigger();
			CcpXcpSignalEvent ccpXcpSignalEvent = recordTrigger.Event as CcpXcpSignalEvent;
			if (ccpXcpSignalEvent == null)
			{
				return null;
			}
			ccpXcpSignalEvent.SignalName.Value = string.Empty;
			ccpXcpSignalEvent.LowValue.Value = 0.0;
			ccpXcpSignalEvent.HighValue.Value = 0.0;
			ccpXcpSignalEvent.Relation.Value = CondRelation.Equal;
			ccpXcpSignalEvent.CcpXcpEcuName.Value = string.Empty;
			using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.diagnosticActionsGrid.ModelValidator, this.ApplicationDatabaseManager, null))
			{
				symbolicSignalCondition.SignalEvent = new CcpXcpSignalEvent(ccpXcpSignalEvent);
				symbolicSignalCondition.CcpXcpSignalConfiguration = CcpXcpManager.Instance().ConfigurationManagerService.CcpXcpSignalConfiguration;
				if (DialogResult.OK == symbolicSignalCondition.ShowDialog())
				{
					ccpXcpSignalEvent.Assign(symbolicSignalCondition.SignalEvent);
					return new TriggeredDiagnosticActionSequence(ccpXcpSignalEvent);
				}
			}
			return null;
		}

		private TriggeredDiagnosticActionSequence CreateDiagnosticSignalSequence()
		{
			if (!this.diagnosticActionsGrid.ModelValidator.HasDiagnosticsDatabasesConfigured())
			{
				InformMessageBox.Error(Resources.ErrorUnableToAddSigReq);
				return null;
			}
			if (!this.diagnosticActionsGrid.DiagnosticActionsConfiguration.DiagnosticActions.Any<DiagnosticAction>())
			{
				InformMessageBox.Error(Resources.ErrorNoDiagnosticSignalRequests);
				return null;
			}
			RecordTrigger recordTrigger = RecordTrigger.CreateDiagnosticSignalTrigger();
			DiagnosticSignalEvent diagnosticSignalEvent = recordTrigger.Event as DiagnosticSignalEvent;
			if (diagnosticSignalEvent == null)
			{
				return null;
			}
			diagnosticSignalEvent.SignalName.Value = string.Empty;
			diagnosticSignalEvent.LowValue.Value = 0.0;
			diagnosticSignalEvent.HighValue.Value = 0.0;
			diagnosticSignalEvent.Relation.Value = CondRelation.Equal;
			diagnosticSignalEvent.DiagnosticEcuName.Value = string.Empty;
			using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.diagnosticActionsGrid.ModelValidator, this.ApplicationDatabaseManager, null))
			{
				symbolicSignalCondition.SignalEvent = new DiagnosticSignalEvent(diagnosticSignalEvent);
				symbolicSignalCondition.DiagnosticActionsConfiguration = this.diagnosticActionsGrid.DiagnosticActionsConfiguration;
				symbolicSignalCondition.DiagnosticsDatabaseConfiguration = this.diagnosticActionsGrid.DiagnosticDatabaseConfiguration;
				if (DialogResult.OK == symbolicSignalCondition.ShowDialog())
				{
					diagnosticSignalEvent.Assign(symbolicSignalCondition.SignalEvent);
					return new TriggeredDiagnosticActionSequence(diagnosticSignalEvent);
				}
			}
			return null;
		}

		public bool Serialize(DiagnosticActionsPage actionsPage)
		{
			if (actionsPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.diagnosticActionsGrid.Serialize(actionsPage);
		}

		public bool DeSerialize(DiagnosticActionsPage actionsPage)
		{
			if (actionsPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.diagnosticActionsGrid.DeSerialize(actionsPage);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DiagnosticActions));
			this.groupBoxDiagActions = new GroupBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.labelEvent = new Label();
			this.mSplitButton = new SplitButton();
			this.labelRequest = new Label();
			this.mSplitButtonAddRequest = new SplitButton();
			this.buttonRemoveAction = new Button();
			this.buttonRemove = new Button();
			this.buttonEditEvent = new Button();
			this.diagnosticActionsGrid = new DiagnosticActionsGrid();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.groupBoxGeneral = new GroupBox();
			this.generalSettingsGL3Plus = new GeneralSettingsGL3Plus();
			this.generalSettingsGL1000 = new GeneralSettingsGL1000();
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.groupBoxDiagActions.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			this.groupBoxGeneral.SuspendLayout();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxDiagActions, "groupBoxDiagActions");
			this.groupBoxDiagActions.Controls.Add(this.tableLayoutPanel1);
			this.groupBoxDiagActions.Controls.Add(this.diagnosticActionsGrid);
			this.errorProviderGlobalModel.SetError(this.groupBoxDiagActions, componentResourceManager.GetString("groupBoxDiagActions.Error"));
			this.errorProviderFormat.SetError(this.groupBoxDiagActions, componentResourceManager.GetString("groupBoxDiagActions.Error1"));
			this.errorProviderLocalModel.SetError(this.groupBoxDiagActions, componentResourceManager.GetString("groupBoxDiagActions.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.groupBoxDiagActions, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxDiagActions.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.groupBoxDiagActions, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxDiagActions.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.groupBoxDiagActions, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxDiagActions.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.groupBoxDiagActions, (int)componentResourceManager.GetObject("groupBoxDiagActions.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.groupBoxDiagActions, (int)componentResourceManager.GetObject("groupBoxDiagActions.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.groupBoxDiagActions, (int)componentResourceManager.GetObject("groupBoxDiagActions.IconPadding2"));
			this.groupBoxDiagActions.Name = "groupBoxDiagActions";
			this.groupBoxDiagActions.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.labelEvent, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.mSplitButton, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelRequest, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.mSplitButtonAddRequest, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.buttonRemoveAction, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.buttonRemove, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonEditEvent, 3, 0);
			this.errorProviderGlobalModel.SetError(this.tableLayoutPanel1, componentResourceManager.GetString("tableLayoutPanel1.Error"));
			this.errorProviderLocalModel.SetError(this.tableLayoutPanel1, componentResourceManager.GetString("tableLayoutPanel1.Error1"));
			this.errorProviderFormat.SetError(this.tableLayoutPanel1, componentResourceManager.GetString("tableLayoutPanel1.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.tableLayoutPanel1, (ErrorIconAlignment)componentResourceManager.GetObject("tableLayoutPanel1.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.tableLayoutPanel1, (ErrorIconAlignment)componentResourceManager.GetObject("tableLayoutPanel1.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.tableLayoutPanel1, (ErrorIconAlignment)componentResourceManager.GetObject("tableLayoutPanel1.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.tableLayoutPanel1, (int)componentResourceManager.GetObject("tableLayoutPanel1.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this.tableLayoutPanel1, (int)componentResourceManager.GetObject("tableLayoutPanel1.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.tableLayoutPanel1, (int)componentResourceManager.GetObject("tableLayoutPanel1.IconPadding2"));
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.labelEvent, "labelEvent");
			this.errorProviderLocalModel.SetError(this.labelEvent, componentResourceManager.GetString("labelEvent.Error"));
			this.errorProviderFormat.SetError(this.labelEvent, componentResourceManager.GetString("labelEvent.Error1"));
			this.errorProviderGlobalModel.SetError(this.labelEvent, componentResourceManager.GetString("labelEvent.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelEvent, (ErrorIconAlignment)componentResourceManager.GetObject("labelEvent.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelEvent, (ErrorIconAlignment)componentResourceManager.GetObject("labelEvent.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.labelEvent, (ErrorIconAlignment)componentResourceManager.GetObject("labelEvent.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelEvent, (int)componentResourceManager.GetObject("labelEvent.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelEvent, (int)componentResourceManager.GetObject("labelEvent.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelEvent, (int)componentResourceManager.GetObject("labelEvent.IconPadding2"));
			this.labelEvent.Name = "labelEvent";
			componentResourceManager.ApplyResources(this.mSplitButton, "mSplitButton");
			this.errorProviderLocalModel.SetError(this.mSplitButton, componentResourceManager.GetString("mSplitButton.Error"));
			this.errorProviderFormat.SetError(this.mSplitButton, componentResourceManager.GetString("mSplitButton.Error1"));
			this.errorProviderGlobalModel.SetError(this.mSplitButton, componentResourceManager.GetString("mSplitButton.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.mSplitButton, (ErrorIconAlignment)componentResourceManager.GetObject("mSplitButton.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.mSplitButton, (ErrorIconAlignment)componentResourceManager.GetObject("mSplitButton.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.mSplitButton, (ErrorIconAlignment)componentResourceManager.GetObject("mSplitButton.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.mSplitButton, (int)componentResourceManager.GetObject("mSplitButton.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.mSplitButton, (int)componentResourceManager.GetObject("mSplitButton.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.mSplitButton, (int)componentResourceManager.GetObject("mSplitButton.IconPadding2"));
			this.mSplitButton.Name = "mSplitButton";
			this.mSplitButton.ShowSplitAlways = true;
			this.mSplitButton.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.labelRequest, "labelRequest");
			this.errorProviderLocalModel.SetError(this.labelRequest, componentResourceManager.GetString("labelRequest.Error"));
			this.errorProviderFormat.SetError(this.labelRequest, componentResourceManager.GetString("labelRequest.Error1"));
			this.errorProviderGlobalModel.SetError(this.labelRequest, componentResourceManager.GetString("labelRequest.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelRequest, (ErrorIconAlignment)componentResourceManager.GetObject("labelRequest.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelRequest, (ErrorIconAlignment)componentResourceManager.GetObject("labelRequest.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.labelRequest, (ErrorIconAlignment)componentResourceManager.GetObject("labelRequest.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelRequest, (int)componentResourceManager.GetObject("labelRequest.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelRequest, (int)componentResourceManager.GetObject("labelRequest.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelRequest, (int)componentResourceManager.GetObject("labelRequest.IconPadding2"));
			this.labelRequest.Name = "labelRequest";
			componentResourceManager.ApplyResources(this.mSplitButtonAddRequest, "mSplitButtonAddRequest");
			this.errorProviderLocalModel.SetError(this.mSplitButtonAddRequest, componentResourceManager.GetString("mSplitButtonAddRequest.Error"));
			this.errorProviderFormat.SetError(this.mSplitButtonAddRequest, componentResourceManager.GetString("mSplitButtonAddRequest.Error1"));
			this.errorProviderGlobalModel.SetError(this.mSplitButtonAddRequest, componentResourceManager.GetString("mSplitButtonAddRequest.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.mSplitButtonAddRequest, (ErrorIconAlignment)componentResourceManager.GetObject("mSplitButtonAddRequest.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.mSplitButtonAddRequest, (ErrorIconAlignment)componentResourceManager.GetObject("mSplitButtonAddRequest.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.mSplitButtonAddRequest, (ErrorIconAlignment)componentResourceManager.GetObject("mSplitButtonAddRequest.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.mSplitButtonAddRequest, (int)componentResourceManager.GetObject("mSplitButtonAddRequest.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.mSplitButtonAddRequest, (int)componentResourceManager.GetObject("mSplitButtonAddRequest.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.mSplitButtonAddRequest, (int)componentResourceManager.GetObject("mSplitButtonAddRequest.IconPadding2"));
			this.mSplitButtonAddRequest.Name = "mSplitButtonAddRequest";
			this.mSplitButtonAddRequest.ShowSplitAlways = true;
			this.mSplitButtonAddRequest.UseVisualStyleBackColor = true;
			this.mSplitButtonAddRequest.Click += new EventHandler(this.mSplitButtonAddRequest_Click);
			componentResourceManager.ApplyResources(this.buttonRemoveAction, "buttonRemoveAction");
			this.errorProviderGlobalModel.SetError(this.buttonRemoveAction, componentResourceManager.GetString("buttonRemoveAction.Error"));
			this.errorProviderFormat.SetError(this.buttonRemoveAction, componentResourceManager.GetString("buttonRemoveAction.Error1"));
			this.errorProviderLocalModel.SetError(this.buttonRemoveAction, componentResourceManager.GetString("buttonRemoveAction.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonRemoveAction, (ErrorIconAlignment)componentResourceManager.GetObject("buttonRemoveAction.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.buttonRemoveAction, (ErrorIconAlignment)componentResourceManager.GetObject("buttonRemoveAction.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.buttonRemoveAction, (ErrorIconAlignment)componentResourceManager.GetObject("buttonRemoveAction.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonRemoveAction, (int)componentResourceManager.GetObject("buttonRemoveAction.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.buttonRemoveAction, (int)componentResourceManager.GetObject("buttonRemoveAction.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.buttonRemoveAction, (int)componentResourceManager.GetObject("buttonRemoveAction.IconPadding2"));
			this.buttonRemoveAction.Image = Resources.ImageDelete;
			this.buttonRemoveAction.Name = "buttonRemoveAction";
			this.buttonRemoveAction.UseVisualStyleBackColor = true;
			this.buttonRemoveAction.Click += new EventHandler(this.buttonRemoveAction_Click);
			componentResourceManager.ApplyResources(this.buttonRemove, "buttonRemove");
			this.errorProviderGlobalModel.SetError(this.buttonRemove, componentResourceManager.GetString("buttonRemove.Error"));
			this.errorProviderFormat.SetError(this.buttonRemove, componentResourceManager.GetString("buttonRemove.Error1"));
			this.errorProviderLocalModel.SetError(this.buttonRemove, componentResourceManager.GetString("buttonRemove.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonRemove, (ErrorIconAlignment)componentResourceManager.GetObject("buttonRemove.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.buttonRemove, (ErrorIconAlignment)componentResourceManager.GetObject("buttonRemove.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.buttonRemove, (ErrorIconAlignment)componentResourceManager.GetObject("buttonRemove.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonRemove, (int)componentResourceManager.GetObject("buttonRemove.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.buttonRemove, (int)componentResourceManager.GetObject("buttonRemove.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.buttonRemove, (int)componentResourceManager.GetObject("buttonRemove.IconPadding2"));
			this.buttonRemove.Image = Resources.ImageDelete;
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
			componentResourceManager.ApplyResources(this.buttonEditEvent, "buttonEditEvent");
			this.errorProviderGlobalModel.SetError(this.buttonEditEvent, componentResourceManager.GetString("buttonEditEvent.Error"));
			this.errorProviderFormat.SetError(this.buttonEditEvent, componentResourceManager.GetString("buttonEditEvent.Error1"));
			this.errorProviderLocalModel.SetError(this.buttonEditEvent, componentResourceManager.GetString("buttonEditEvent.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.buttonEditEvent, (ErrorIconAlignment)componentResourceManager.GetObject("buttonEditEvent.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.buttonEditEvent, (ErrorIconAlignment)componentResourceManager.GetObject("buttonEditEvent.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.buttonEditEvent, (ErrorIconAlignment)componentResourceManager.GetObject("buttonEditEvent.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.buttonEditEvent, (int)componentResourceManager.GetObject("buttonEditEvent.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.buttonEditEvent, (int)componentResourceManager.GetObject("buttonEditEvent.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.buttonEditEvent, (int)componentResourceManager.GetObject("buttonEditEvent.IconPadding2"));
			this.buttonEditEvent.Name = "buttonEditEvent";
			this.buttonEditEvent.UseVisualStyleBackColor = true;
			this.buttonEditEvent.Click += new EventHandler(this.buttonEditEvent_Click);
			componentResourceManager.ApplyResources(this.diagnosticActionsGrid, "diagnosticActionsGrid");
			this.diagnosticActionsGrid.ApplicationDatabaseManager = null;
			this.diagnosticActionsGrid.DiagnosticActionsConfiguration = null;
			this.diagnosticActionsGrid.DiagnosticDatabaseConfiguration = null;
			this.diagnosticActionsGrid.DiagSymbolsManager = null;
			this.diagnosticActionsGrid.DisplayMode = null;
			this.errorProviderGlobalModel.SetError(this.diagnosticActionsGrid, componentResourceManager.GetString("diagnosticActionsGrid.Error"));
			this.errorProviderFormat.SetError(this.diagnosticActionsGrid, componentResourceManager.GetString("diagnosticActionsGrid.Error1"));
			this.errorProviderLocalModel.SetError(this.diagnosticActionsGrid, componentResourceManager.GetString("diagnosticActionsGrid.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.diagnosticActionsGrid, (ErrorIconAlignment)componentResourceManager.GetObject("diagnosticActionsGrid.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.diagnosticActionsGrid, (ErrorIconAlignment)componentResourceManager.GetObject("diagnosticActionsGrid.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.diagnosticActionsGrid, (ErrorIconAlignment)componentResourceManager.GetObject("diagnosticActionsGrid.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.diagnosticActionsGrid, (int)componentResourceManager.GetObject("diagnosticActionsGrid.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.diagnosticActionsGrid, (int)componentResourceManager.GetObject("diagnosticActionsGrid.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.diagnosticActionsGrid, (int)componentResourceManager.GetObject("diagnosticActionsGrid.IconPadding2"));
			this.diagnosticActionsGrid.ModelValidator = null;
			this.diagnosticActionsGrid.Name = "diagnosticActionsGrid";
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderFormat, "errorProviderFormat");
			componentResourceManager.ApplyResources(this.groupBoxGeneral, "groupBoxGeneral");
			this.groupBoxGeneral.Controls.Add(this.generalSettingsGL3Plus);
			this.groupBoxGeneral.Controls.Add(this.generalSettingsGL1000);
			this.errorProviderGlobalModel.SetError(this.groupBoxGeneral, componentResourceManager.GetString("groupBoxGeneral.Error"));
			this.errorProviderFormat.SetError(this.groupBoxGeneral, componentResourceManager.GetString("groupBoxGeneral.Error1"));
			this.errorProviderLocalModel.SetError(this.groupBoxGeneral, componentResourceManager.GetString("groupBoxGeneral.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.groupBoxGeneral, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxGeneral.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.groupBoxGeneral, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxGeneral.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.groupBoxGeneral, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxGeneral.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.groupBoxGeneral, (int)componentResourceManager.GetObject("groupBoxGeneral.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.groupBoxGeneral, (int)componentResourceManager.GetObject("groupBoxGeneral.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.groupBoxGeneral, (int)componentResourceManager.GetObject("groupBoxGeneral.IconPadding2"));
			this.groupBoxGeneral.Name = "groupBoxGeneral";
			this.groupBoxGeneral.TabStop = false;
			componentResourceManager.ApplyResources(this.generalSettingsGL3Plus, "generalSettingsGL3Plus");
			this.generalSettingsGL3Plus.DiagnosticActionsConfiguration = null;
			this.generalSettingsGL3Plus.DisplayMode = null;
			this.errorProviderGlobalModel.SetError(this.generalSettingsGL3Plus, componentResourceManager.GetString("generalSettingsGL3Plus.Error"));
			this.errorProviderFormat.SetError(this.generalSettingsGL3Plus, componentResourceManager.GetString("generalSettingsGL3Plus.Error1"));
			this.errorProviderLocalModel.SetError(this.generalSettingsGL3Plus, componentResourceManager.GetString("generalSettingsGL3Plus.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.generalSettingsGL3Plus, (ErrorIconAlignment)componentResourceManager.GetObject("generalSettingsGL3Plus.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.generalSettingsGL3Plus, (ErrorIconAlignment)componentResourceManager.GetObject("generalSettingsGL3Plus.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.generalSettingsGL3Plus, (ErrorIconAlignment)componentResourceManager.GetObject("generalSettingsGL3Plus.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.generalSettingsGL3Plus, (int)componentResourceManager.GetObject("generalSettingsGL3Plus.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.generalSettingsGL3Plus, (int)componentResourceManager.GetObject("generalSettingsGL3Plus.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.generalSettingsGL3Plus, (int)componentResourceManager.GetObject("generalSettingsGL3Plus.IconPadding2"));
			this.generalSettingsGL3Plus.ModelValidator = null;
			this.generalSettingsGL3Plus.Name = "generalSettingsGL3Plus";
			componentResourceManager.ApplyResources(this.generalSettingsGL1000, "generalSettingsGL1000");
			this.generalSettingsGL1000.DiagnosticActionsConfiguration = null;
			this.generalSettingsGL1000.DisplayMode = null;
			this.errorProviderGlobalModel.SetError(this.generalSettingsGL1000, componentResourceManager.GetString("generalSettingsGL1000.Error"));
			this.errorProviderFormat.SetError(this.generalSettingsGL1000, componentResourceManager.GetString("generalSettingsGL1000.Error1"));
			this.errorProviderLocalModel.SetError(this.generalSettingsGL1000, componentResourceManager.GetString("generalSettingsGL1000.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.generalSettingsGL1000, (ErrorIconAlignment)componentResourceManager.GetObject("generalSettingsGL1000.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.generalSettingsGL1000, (ErrorIconAlignment)componentResourceManager.GetObject("generalSettingsGL1000.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.generalSettingsGL1000, (ErrorIconAlignment)componentResourceManager.GetObject("generalSettingsGL1000.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.generalSettingsGL1000, (int)componentResourceManager.GetObject("generalSettingsGL1000.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.generalSettingsGL1000, (int)componentResourceManager.GetObject("generalSettingsGL1000.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.generalSettingsGL1000, (int)componentResourceManager.GetObject("generalSettingsGL1000.IconPadding2"));
			this.generalSettingsGL1000.ModelValidator = null;
			this.generalSettingsGL1000.Name = "generalSettingsGL1000";
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderLocalModel, "errorProviderLocalModel");
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxGeneral);
			base.Controls.Add(this.groupBoxDiagActions);
			this.errorProviderGlobalModel.SetError(this, componentResourceManager.GetString("$this.Error"));
			this.errorProviderLocalModel.SetError(this, componentResourceManager.GetString("$this.Error1"));
			this.errorProviderFormat.SetError(this, componentResourceManager.GetString("$this.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding2"));
			base.Name = "DiagnosticActions";
			this.groupBoxDiagActions.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			this.groupBoxGeneral.ResumeLayout(false);
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
