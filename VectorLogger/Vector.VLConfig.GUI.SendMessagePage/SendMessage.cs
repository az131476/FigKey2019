using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.SendMessagePage
{
	public class SendMessage : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<SendMessageConfiguration>, IUpdateObserver<DisplayMode>, IUpdateObserver<LoggerType>, IUpdateObserver<DiagnosticActionsConfiguration>, IUpdateObserver<DiagnosticsDatabaseConfiguration>, IUpdateObserver, ISplitButtonExClient
	{
		public delegate void DataChangedHandler(SendMessageConfiguration data);

		private readonly SplitButtonEx mSplitButtonEx;

		private IContainer components;

		private GroupBox groupBoxSendMessage;

		private SendMessageGrid sendMessageGrid;

		private SplitButton mSplitButton;

		private Button buttonRemove;

		private Label mLabelAddEvent;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.sendMessageGrid.ApplicationDatabaseManager;
			}
			set
			{
				this.sendMessageGrid.ApplicationDatabaseManager = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.sendMessageGrid.ModelValidator;
			}
			set
			{
				this.sendMessageGrid.ModelValidator = value;
			}
		}

		ISemanticChecker IPropertyWindow.SemanticChecker
		{
			get
			{
				return this.sendMessageGrid.SemanticChecker;
			}
			set
			{
				this.sendMessageGrid.SemanticChecker = value;
			}
		}

		IModelEditor IPropertyWindow.ModelEditor
		{
			get
			{
				return this.sendMessageGrid.ModelEditor;
			}
			set
			{
				this.sendMessageGrid.ModelEditor = value;
			}
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
				return PageType.SendMessage;
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
					this.sendMessageGrid.DisplayErrors();
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

		public SendMessage()
		{
			this.InitializeComponent();
			this.sendMessageGrid.SelectionChanged += new EventHandler(this.OnSendMessageGridSelectionChanged);
			this.mSplitButtonEx = new SplitButtonEx(this);
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is SendMessageConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.SendMessage);
			}
			GUIUtil.InitSplitButtonMenuEventTypes(this.mSplitButtonEx);
			this.sendMessageGrid.Init();
		}

		void IPropertyWindow.Reset()
		{
			this.mSplitButtonEx.UpdateSplitMenu();
			this.sendMessageGrid.Reset();
		}

		bool IPropertyWindow.ValidateInput()
		{
			return this.sendMessageGrid.ModelValidator.LoggerSpecifics.CAN.ChannelsWithOutputSupport.Count <= 0 || this.sendMessageGrid.ValidateInput(false);
		}

		bool IPropertyWindow.HasErrors()
		{
			return this.sendMessageGrid.ModelValidator.LoggerSpecifics.CAN.ChannelsWithOutputSupport.Count > 0 && this.sendMessageGrid.HasErrors();
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			return this.sendMessageGrid.ModelValidator.LoggerSpecifics.CAN.ChannelsWithOutputSupport.Count > 0 && this.sendMessageGrid.HasGlobalErrors();
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			return this.sendMessageGrid.ModelValidator.LoggerSpecifics.CAN.ChannelsWithOutputSupport.Count > 0 && this.sendMessageGrid.HasLocalErrors();
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			return this.sendMessageGrid.ModelValidator.LoggerSpecifics.CAN.ChannelsWithOutputSupport.Count > 0 && this.sendMessageGrid.HasFormatErrors();
		}

		void IUpdateObserver<SendMessageConfiguration>.Update(SendMessageConfiguration data)
		{
			if (this.sendMessageGrid.ModelValidator.LoggerSpecifics.CAN.ChannelsWithOutputSupport.Count > 0)
			{
				this.sendMessageGrid.SendMessageConfiguration = data;
			}
		}

		void IUpdateObserver<DisplayMode>.Update(DisplayMode data)
		{
			if (this.sendMessageGrid.ModelValidator.LoggerSpecifics.CAN.ChannelsWithOutputSupport.Count > 0)
			{
				this.sendMessageGrid.DisplayMode = data;
			}
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType data)
		{
			if (this.sendMessageGrid.ModelValidator.LoggerSpecifics.CAN.ChannelsWithOutputSupport.Count > 0 && this.sendMessageGrid.LoggerType != data)
			{
				this.mSplitButtonEx.UpdateSplitMenu();
			}
			this.sendMessageGrid.LoggerType = data;
		}

		void IUpdateObserver<DiagnosticActionsConfiguration>.Update(DiagnosticActionsConfiguration data)
		{
			if (this.sendMessageGrid.ModelValidator.LoggerSpecifics.CAN.ChannelsWithOutputSupport.Count > 0)
			{
				this.sendMessageGrid.DiagnosticActionsConfiguration = data;
				((IPropertyWindow)this).ValidateInput();
			}
		}

		void IUpdateObserver<DiagnosticsDatabaseConfiguration>.Update(DiagnosticsDatabaseConfiguration data)
		{
			if (this.sendMessageGrid.ModelValidator.LoggerSpecifics.CAN.ChannelsWithOutputSupport.Count > 0)
			{
				this.sendMessageGrid.DiagnosticDatabaseConfiguration = data;
				((IPropertyWindow)this).ValidateInput();
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

		private void OnSendMessageGridSelectionChanged(object sender, EventArgs e)
		{
			ActionSendMessage actionSendMessage;
			this.buttonRemove.Enabled = this.sendMessageGrid.TryGetSelectedAction(out actionSendMessage);
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			ActionSendMessage action;
			if (this.sendMessageGrid.TryGetSelectedAction(out action))
			{
				this.sendMessageGrid.RemoveAction(action);
			}
		}

		public bool IsItemVisible(ToolStripItem item)
		{
			string text = item.Text;
			if (text == Resources_Trigger.TriggerTypeNameColCANBusStatistics || text == Resources_Trigger.TriggerTypeNameColCANData || text == Vocabulary.TriggerTypeNameColCANId || text == Resources_Trigger.TriggerTypeNameColCANMsgTimeout || text == Resources_Trigger.TriggerTypeNameColSymbolicCAN || text == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
			{
				return this.sendMessageGrid.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColLINData || text == Vocabulary.TriggerTypeNameColLINId || text == Resources_Trigger.TriggerTypeNameColLINMsgTimeout || text == Resources_Trigger.TriggerTypeNameColSymbolicLIN || text == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
			{
				return this.sendMessageGrid.ModelValidator.LoggerSpecifics.LIN.NumberOfChannels > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColFlexray || text == Resources_Trigger.TriggerTypeNameColSymbolicFlexray || text == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
			{
				return this.sendMessageGrid.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColKey)
			{
				return this.sendMessageGrid.ModelValidator.LoggerSpecifics.IO.NumberOfKeys > 0u || this.sendMessageGrid.ModelValidator.LoggerSpecifics.IO.NumberOfPanelKeys > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColAnalogInput)
			{
				return this.sendMessageGrid.ModelValidator.LoggerSpecifics.IO.NumberOfAnalogInputs > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColDigitalInput)
			{
				return this.sendMessageGrid.ModelValidator.LoggerSpecifics.IO.NumberOfDigitalInputs > 0u;
			}
			if (text == Resources_Trigger.TriggerTypeNameColIgnition)
			{
				return this.sendMessageGrid.ModelValidator.LoggerSpecifics.Recording.IsIgnitionEventSupported;
			}
			if (text == Resources_Trigger.TriggerTypeNameColVoCanRecording)
			{
				return this.sendMessageGrid.ModelValidator.LoggerSpecifics.Recording.IsVoCANSupported;
			}
			if (text == Resources_Trigger.TriggerTypeNameColCcpXcpSignal)
			{
				return this.sendMessageGrid.ModelValidator.LoggerSpecifics.Recording.IsCCPXCPSignalEventSupported;
			}
			return text == Resources_Trigger.TriggerTypeNameColDiagnosticSignal && this.sendMessageGrid.ModelValidator.LoggerSpecifics.Recording.IsDiagnosticsSupported;
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
			ActionSendMessage actionSendMessage = this.CreateActionForSelectedEventType(itemText);
			if (actionSendMessage == null)
			{
				return;
			}
			this.sendMessageGrid.AddAction(actionSendMessage);
		}

		private ActionSendMessage CreateActionForSelectedEventType(string eventTypeName)
		{
			Event @event;
			if (eventTypeName == Vocabulary.TriggerTypeNameColOnStart)
			{
				@event = this.sendMessageGrid.CreateOnStartEvent();
			}
			else if (eventTypeName == Vocabulary.TriggerTypeNameColCANId)
			{
				@event = this.sendMessageGrid.CreateCANIdEvent();
			}
			else if (eventTypeName == Vocabulary.TriggerTypeNameColLINId)
			{
				@event = this.sendMessageGrid.CreateLINIdEvent();
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColCANData)
			{
				@event = this.sendMessageGrid.CreateCANDataEvent();
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColLINData)
			{
				@event = this.sendMessageGrid.CreateLINDataEvent();
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColFlexray)
			{
				@event = this.sendMessageGrid.CreateFlexrayIdEvent();
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicCAN)
			{
				@event = this.sendMessageGrid.CreateSymMessageEvent(BusType.Bt_CAN);
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicLIN)
			{
				@event = this.sendMessageGrid.CreateSymMessageEvent(BusType.Bt_LIN);
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicFlexray)
			{
				@event = this.sendMessageGrid.CreateSymMessageEvent(BusType.Bt_FlexRay);
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
			{
				@event = this.sendMessageGrid.CreateSymSignalEvent(BusType.Bt_CAN);
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
			{
				@event = this.sendMessageGrid.CreateSymSignalEvent(BusType.Bt_LIN);
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
			{
				@event = this.sendMessageGrid.CreateSymSignalEvent(BusType.Bt_FlexRay);
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColCANMsgTimeout)
			{
				@event = this.sendMessageGrid.CreateMsgTimeoutEvent(BusType.Bt_CAN);
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColLINMsgTimeout)
			{
				@event = this.sendMessageGrid.CreateMsgTimeoutEvent(BusType.Bt_LIN);
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColDigitalInput)
			{
				@event = this.sendMessageGrid.CreateDigitalInputEvent();
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColAnalogInput)
			{
				@event = this.sendMessageGrid.CreateAnalogInputEvent();
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColKey)
			{
				@event = this.sendMessageGrid.CreateKeyEvent();
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColVoCanRecording)
			{
				@event = this.sendMessageGrid.CreateVoCANRecordingEvent();
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColIgnition)
			{
				@event = this.sendMessageGrid.CreateIgnitionEvent();
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColCANBusStatistics)
			{
				@event = this.sendMessageGrid.CreateCANBusStatisticsEvent();
			}
			else if (eventTypeName == Resources_Trigger.TriggerTypeNameColCcpXcpSignal)
			{
				@event = this.sendMessageGrid.CreateCcpXcpSignalEvent();
			}
			else
			{
				if (!(eventTypeName == Resources_Trigger.TriggerTypeNameColDiagnosticSignal))
				{
					return null;
				}
				@event = this.sendMessageGrid.CreateDiagnosticSignalEvent();
			}
			if (@event != null)
			{
				return this.CreateActionSendMessage(@event);
			}
			return null;
		}

		private ActionSendMessage CreateActionSendMessage(Event ev)
		{
			ActionSendMessage actionSendMessage = new ActionSendMessage(ev, new StopImmediate());
			ConfigureSendMessage configureSendMessageDialog = this.sendMessageGrid.GetConfigureSendMessageDialog();
			configureSendMessageDialog.ActionSendMessage = actionSendMessage;
			configureSendMessageDialog.InitDefaultValues();
			if (configureSendMessageDialog.ShowDialog() == DialogResult.OK)
			{
				return new ActionSendMessage(configureSendMessageDialog.ActionSendMessage);
			}
			return null;
		}

		public bool Serialize(SendMessagePage sendMessagePage)
		{
			if (sendMessagePage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.sendMessageGrid.Serialize(sendMessagePage);
		}

		public bool DeSerialize(SendMessagePage sendMessagePage)
		{
			if (sendMessagePage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.sendMessageGrid.DeSerialize(sendMessagePage);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SendMessage));
			this.groupBoxSendMessage = new GroupBox();
			this.buttonRemove = new Button();
			this.mSplitButton = new SplitButton();
			this.mLabelAddEvent = new Label();
			this.sendMessageGrid = new SendMessageGrid();
			this.groupBoxSendMessage.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxSendMessage, "groupBoxSendMessage");
			this.groupBoxSendMessage.Controls.Add(this.buttonRemove);
			this.groupBoxSendMessage.Controls.Add(this.mSplitButton);
			this.groupBoxSendMessage.Controls.Add(this.mLabelAddEvent);
			this.groupBoxSendMessage.Controls.Add(this.sendMessageGrid);
			this.groupBoxSendMessage.Name = "groupBoxSendMessage";
			this.groupBoxSendMessage.TabStop = false;
			componentResourceManager.ApplyResources(this.buttonRemove, "buttonRemove");
			this.buttonRemove.Image = Resources.ImageDelete;
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
			componentResourceManager.ApplyResources(this.mSplitButton, "mSplitButton");
			this.mSplitButton.Name = "mSplitButton";
			this.mSplitButton.ShowSplitAlways = true;
			this.mSplitButton.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mLabelAddEvent, "mLabelAddEvent");
			this.mLabelAddEvent.Name = "mLabelAddEvent";
			componentResourceManager.ApplyResources(this.sendMessageGrid, "sendMessageGrid");
			this.sendMessageGrid.ApplicationDatabaseManager = null;
			this.sendMessageGrid.DisplayMode = null;
			this.sendMessageGrid.LoggerType = LoggerType.Unknown;
			this.sendMessageGrid.ModelValidator = null;
			this.sendMessageGrid.Name = "sendMessageGrid";
			this.sendMessageGrid.SendMessageConfiguration = null;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxSendMessage);
			base.Name = "SendMessage";
			componentResourceManager.ApplyResources(this, "$this");
			this.groupBoxSendMessage.ResumeLayout(false);
			this.groupBoxSendMessage.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
