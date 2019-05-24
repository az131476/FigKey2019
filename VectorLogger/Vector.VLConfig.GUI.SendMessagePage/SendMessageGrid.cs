using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.EventConditions;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.SendMessagePage
{
	internal class SendMessageGrid : UserControl
	{
		private SendMessageConfiguration sendMessageConfiguration;

		private DisplayMode displayMode;

		private LoggerType loggerType;

		private GUIElementManager_ControlGridTree guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private OnStartCondition onStartCondDialog;

		private CyclicTimerCondition cyclicTimerCondDialog;

		private CANIdCondition canIdConditionDialog;

		private LINIdCondition linIdConditionDialog;

		private FlexrayIdCondition flexrayConditionDialog;

		private CANDataCondition canDataConditionDialog;

		private LINDataCondition linDataConditionDialog;

		private SymbolicMessageCondition messageConditionDialog;

		private KeyCondition keyConditionDialog;

		private DigitalInputCondition digInConditionDialog;

		private AnalogInputCondition analogInputConditionDialog;

		private IgnitionCondition ignitionConditionDialog;

		private MsgTimeoutCondition msgTimeoutConditionDialog;

		private CANBusStatisticsCondition canBusStatisticsDialog;

		private VoCanRecordingCondition voCANRecordingDialog;

		private ConfigureSendMessage configureSendMsgDialog;

		private Dictionary<string, int> eventTypeNameToImageIndex;

		private ImageCollection eventTypeImageCollection;

		private int oldSelectedEventIndex;

		private IContainer components;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private GridControl gridControlSendMessage;

		private GridView gridViewSendMessage;

		private GridColumn colActive;

		private RepositoryItemCheckEdit repositoryItemCheckEditIsActive;

		private GridColumn colStartEventType;

		private GridColumn colStartCondition;

		private RepositoryItemButtonEdit repositoryItemButtonEditStartCondition;

		private GridColumn colSendMessage;

		private RepositoryItemButtonEdit repositoryItemButtonEditSendMessage;

		private GridColumn colComment;

		private RepositoryItemMemoExEdit repositoryItemMemoExEditComment;

		private BindingSource sendMessageBindingSource;

		private RepositoryItemImageComboBox repositoryItemImageComboBoxStartEvent;

		private RepositoryItemComboBox repositoryItemComboBoxKeyCondition;

		private RepositoryItemComboBox repositoryItemComboBoxIgnitionCondition;

		private XtraToolTipController toolTipController;

		public event EventHandler SelectionChanged;

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public ISemanticChecker SemanticChecker
		{
			get;
			set;
		}

		public IModelEditor ModelEditor
		{
			get;
			set;
		}

		public SendMessageConfiguration SendMessageConfiguration
		{
			get
			{
				return this.sendMessageConfiguration;
			}
			set
			{
				this.sendMessageConfiguration = value;
				if (value != null)
				{
					int focusedRowHandle = this.gridViewSendMessage.FocusedRowHandle;
					this.gridControlSendMessage.DataSource = this.SendMessageConfiguration.Actions;
					if (focusedRowHandle >= 0 && focusedRowHandle < this.gridViewSendMessage.RowCount)
					{
						this.gridViewSendMessage.FocusedRowHandle = focusedRowHandle;
					}
					else
					{
						this.gridViewSendMessage.FocusedRowHandle = -1;
					}
					this.ValidateInput(false);
				}
			}
		}

		public LoggerType LoggerType
		{
			get
			{
				return this.loggerType;
			}
			set
			{
				if (this.loggerType != value)
				{
					this.InitEventTypeNamesInPlaceEditor();
				}
				this.loggerType = value;
			}
		}

		public DisplayMode DisplayMode
		{
			get
			{
				return this.displayMode;
			}
			set
			{
				if (this.displayMode != null)
				{
					this.gridViewSendMessage.RefreshData();
				}
				this.displayMode = value;
			}
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		public DiagnosticActionsConfiguration DiagnosticActionsConfiguration
		{
			get;
			set;
		}

		public DiagnosticsDatabaseConfiguration DiagnosticDatabaseConfiguration
		{
			get;
			set;
		}

		public SendMessageGrid()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_ControlGridTree();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.InitializeEventTypeImageList();
			this.repositoryItemImageComboBoxStartEvent.SmallImages = this.eventTypeImageCollection;
			this.oldSelectedEventIndex = -1;
		}

		private void Raise_SelectionChanged(object sender, EventArgs e)
		{
			if (this.SelectionChanged != null)
			{
				this.SelectionChanged(sender, e);
			}
		}

		public void Init()
		{
		}

		public void AddAction(ActionSendMessage action)
		{
			this.sendMessageConfiguration.AddAction(action);
			this.gridViewSendMessage.RefreshData();
			this.gridViewSendMessage.GetRowHandle(this.gridViewSendMessage.GetFocusedDataSourceRowIndex());
			this.ValidateInput(true);
			this.SelectRowOfAction(action);
		}

		public void RemoveAction(ActionSendMessage action)
		{
			this.sendMessageConfiguration.RemoveAction(action);
			this.gridViewSendMessage.RefreshData();
			this.gridViewSendMessage.GetRowHandle(this.gridViewSendMessage.GetFocusedDataSourceRowIndex());
			this.ValidateInput(true);
		}

		private void gridViewSendMessage_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			ActionSendMessage action;
			if (!this.TryGetAction(e.ListSourceRowIndex, out action))
			{
				return;
			}
			if (e.Column == this.colStartEventType)
			{
				this.UnboundColumnDataEvent(action, e);
				return;
			}
			if (e.Column == this.colStartCondition)
			{
				this.UnboundColumnDataCondition(action, e);
				return;
			}
			if (e.Column == this.colSendMessage)
			{
				this.UnboundColumnDataSendMessage(action, e);
				return;
			}
			if (e.Column == this.colActive)
			{
				this.UnboundColumnDataActionActive(action, e);
				return;
			}
			if (e.Column == this.colComment)
			{
				this.UnboundColumnDataComment(action, e);
			}
		}

		private void UnboundColumnDataEvent(ActionSendMessage action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = this.GetUnboundColumnDataEventString(action);
			}
		}

		private string GetUnboundColumnDataEventString(ActionSendMessage action)
		{
			if (action.Event is OnStartEvent)
			{
				return Vocabulary.TriggerTypeNameColOnStart;
			}
			if (action.Event is CANIdEvent)
			{
				return Vocabulary.TriggerTypeNameColCANId;
			}
			if (action.Event is LINIdEvent)
			{
				return Vocabulary.TriggerTypeNameColLINId;
			}
			if (action.Event is FlexrayIdEvent)
			{
				return Resources_Trigger.TriggerTypeNameColFlexray;
			}
			if (action.Event is SymbolicMessageEvent)
			{
				SymbolicMessageEvent symbolicMessageEvent = action.Event as SymbolicMessageEvent;
				if (BusType.Bt_CAN == symbolicMessageEvent.BusType.Value)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicCAN;
				}
				if (BusType.Bt_LIN == symbolicMessageEvent.BusType.Value)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicLIN;
				}
				if (BusType.Bt_FlexRay == symbolicMessageEvent.BusType.Value)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicFlexray;
				}
			}
			else if (action.Event is SymbolicSignalEvent)
			{
				SymbolicSignalEvent symbolicSignalEvent = action.Event as SymbolicSignalEvent;
				if (BusType.Bt_CAN == symbolicSignalEvent.BusType.Value)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicSigCAN;
				}
				if (BusType.Bt_LIN == symbolicSignalEvent.BusType.Value)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicSigLIN;
				}
				if (BusType.Bt_FlexRay == symbolicSignalEvent.BusType.Value)
				{
					return Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray;
				}
			}
			else
			{
				if (action.Event is CcpXcpSignalEvent)
				{
					return Resources_Trigger.TriggerTypeNameColCcpXcpSignal;
				}
				if (action.Event is DiagnosticSignalEvent)
				{
					return Resources_Trigger.TriggerTypeNameColDiagnosticSignal;
				}
				if (action.Event is CANDataEvent)
				{
					return Resources_Trigger.TriggerTypeNameColCANData;
				}
				if (action.Event is LINDataEvent)
				{
					return Resources_Trigger.TriggerTypeNameColLINData;
				}
				if (action.Event is MsgTimeoutEvent)
				{
					MsgTimeoutEvent msgTimeoutEvent = action.Event as MsgTimeoutEvent;
					if (BusType.Bt_CAN == msgTimeoutEvent.BusType.Value)
					{
						return Resources_Trigger.TriggerTypeNameColCANMsgTimeout;
					}
					if (BusType.Bt_LIN == msgTimeoutEvent.BusType.Value)
					{
						return Resources_Trigger.TriggerTypeNameColLINMsgTimeout;
					}
				}
				else
				{
					if (action.Event is DigitalInputEvent)
					{
						return Resources_Trigger.TriggerTypeNameColDigitalInput;
					}
					if (action.Event is AnalogInputEvent)
					{
						return Resources_Trigger.TriggerTypeNameColAnalogInput;
					}
					if (action.Event is KeyEvent)
					{
						return Resources_Trigger.TriggerTypeNameColKey;
					}
					if (action.Event is CANBusStatisticsEvent)
					{
						return Resources_Trigger.TriggerTypeNameColCANBusStatistics;
					}
					if (action.Event is IgnitionEvent)
					{
						return Resources_Trigger.TriggerTypeNameColIgnition;
					}
					if (action.Event is VoCanRecordingEvent)
					{
						return Resources_Trigger.TriggerTypeNameColVoCanRecording;
					}
				}
			}
			return string.Empty;
		}

		private void UnboundColumnDataCondition(ActionSendMessage action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = this.GetUnboundColumnDataConditionString(action);
				return;
			}
			if (e.IsSetData)
			{
				if (action.Event is KeyEvent)
				{
					bool flag = false;
					bool value = false;
					uint value2 = GUIUtil.MapStringToKeyNumber(e.Value.ToString(), out value);
					bool flag2;
					this.pageValidator.Grid.UpdateModel<uint>(value2, (action.Event as KeyEvent).Number, out flag2);
					this.pageValidator.Grid.UpdateModel<bool>(value, (action.Event as KeyEvent).IsOnPanel, out flag);
					if (flag2 || flag)
					{
						this.ValidateInput(true);
						return;
					}
				}
				else if (action.Event is IgnitionEvent)
				{
					bool flag3 = false;
					this.pageValidator.Grid.UpdateModel<bool>(Resources.IgnitionOn == e.Value.ToString(), (action.Event as IgnitionEvent).IsOn, out flag3);
					if (flag3)
					{
						this.ValidateInput(true);
					}
				}
			}
		}

		private string GetUnboundColumnDataConditionString(ActionSendMessage action)
		{
			if (action.Event is OnStartEvent)
			{
				double num = Convert.ToDouble((action.Event as OnStartEvent).Delay.Value) / 1000.0;
				return string.Format(Resources.DiagEvOnStartGroupRow, num.ToString("F1"));
			}
			if (action.Event is IdEvent)
			{
				IdEvent idEvent = action.Event as IdEvent;
				if (idEvent is CANIdEvent)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(idEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(idEvent));
				}
				if (idEvent is LINIdEvent)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(idEvent.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(idEvent));
				}
				if (idEvent is FlexrayIdEvent)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapFlexrayChannelNumber2String(idEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(idEvent));
				}
			}
			else if (action.Event is SymbolicMessageEvent)
			{
				SymbolicMessageEvent symbolicMessageEvent = action.Event as SymbolicMessageEvent;
				if (symbolicMessageEvent.BusType.Value == BusType.Bt_CAN)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(symbolicMessageEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(symbolicMessageEvent, this.ModelValidator.DatabaseServices));
				}
				if (symbolicMessageEvent.BusType.Value == BusType.Bt_LIN)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(symbolicMessageEvent.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(symbolicMessageEvent, this.ModelValidator.DatabaseServices));
				}
				if (symbolicMessageEvent.BusType.Value == BusType.Bt_FlexRay)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapFlexrayChannelNumber2String(symbolicMessageEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(symbolicMessageEvent, this.ModelValidator.DatabaseServices));
				}
			}
			else if (action.Event is SymbolicSignalEvent)
			{
				SymbolicSignalEvent symbolicSignalEvent = action.Event as SymbolicSignalEvent;
				if (symbolicSignalEvent.BusType.Value == BusType.Bt_CAN)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(symbolicSignalEvent, this.ModelValidator.DatabaseServices));
				}
				if (symbolicSignalEvent.BusType.Value == BusType.Bt_LIN)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(symbolicSignalEvent, this.ModelValidator.DatabaseServices));
				}
				if (symbolicSignalEvent.BusType.Value == BusType.Bt_FlexRay)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapFlexrayChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(symbolicSignalEvent, this.ModelValidator.DatabaseServices));
				}
			}
			else
			{
				if (action.Event is CcpXcpSignalEvent)
				{
					CcpXcpSignalEvent symSigEvent = action.Event as CcpXcpSignalEvent;
					return GUIUtil.MapEventCondition2String(symSigEvent, this.ModelValidator.DatabaseServices);
				}
				if (action.Event is DiagnosticSignalEvent)
				{
					DiagnosticSignalEvent symSigEvent2 = action.Event as DiagnosticSignalEvent;
					return GUIUtil.MapEventCondition2String(symSigEvent2, this.ModelValidator.DatabaseServices);
				}
				if (action.Event is DigitalInputEvent)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapDigitalInputNumber2String((action.Event as DigitalInputEvent).DigitalInput.Value), GUIUtil.MapEventCondition2String(action.Event as DigitalInputEvent));
				}
				if (action.Event is AnalogInputEvent)
				{
					return string.Format("{0} {1}", GUIUtil.MapAnalogInputNumber2String((action.Event as AnalogInputEvent).InputNumber.Value), GUIUtil.MapEventCondition2String(action.Event as AnalogInputEvent));
				}
				if (action.Event is CANDataEvent)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String((action.Event as CANDataEvent).ChannelNumber.Value), GUIUtil.MapEventCondition2String(action.Event as CANDataEvent));
				}
				if (action.Event is LINDataEvent)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String((action.Event as LINDataEvent).ChannelNumber.Value, this.ModelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(action.Event as LINDataEvent));
				}
				if (action.Event is MsgTimeoutEvent)
				{
					MsgTimeoutEvent msgTimeoutEvent = action.Event as MsgTimeoutEvent;
					if (msgTimeoutEvent.BusType.Value == BusType.Bt_CAN)
					{
						return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(msgTimeoutEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(msgTimeoutEvent, this.ModelValidator.DatabaseServices));
					}
					if (msgTimeoutEvent.BusType.Value == BusType.Bt_LIN)
					{
						return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(msgTimeoutEvent.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(msgTimeoutEvent, this.ModelValidator.DatabaseServices));
					}
				}
				else
				{
					if (action.Event is KeyEvent)
					{
						return GUIUtil.MapKeyNumber2String((action.Event as KeyEvent).Number.Value, (action.Event as KeyEvent).IsOnPanel.Value);
					}
					if (action.Event is CANBusStatisticsEvent)
					{
						return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String((action.Event as CANBusStatisticsEvent).ChannelNumber.Value), GUIUtil.MapEventCondition2String(action.Event as CANBusStatisticsEvent));
					}
					if (action.Event is IgnitionEvent)
					{
						return GUIUtil.MapEventCondition2String(action.Event as IgnitionEvent);
					}
					if (action.Event is VoCanRecordingEvent)
					{
						return GUIUtil.MapEventCondition2String(action.Event as VoCanRecordingEvent);
					}
				}
			}
			return string.Empty;
		}

		private void UnboundColumnDataSendMessage(ActionSendMessage action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("{0}; ID: {1}; [", GUIUtil.MapCANChannelNumber2String(action.ChannelNumber.Value), action.IsSymbolic.Value ? action.SymbolName.Value : GUIUtil.CANIdToDisplayString(action.ID.Value, action.IsExtendedId.Value));
				foreach (DataItem current in action.MessageData)
				{
					if (GUIUtil.IsHexadecimal)
					{
						stringBuilder.AppendFormat(" 0x{0:X2}", current.Byte.Value);
					}
					else
					{
						stringBuilder.AppendFormat(" {0}", GUIUtil.NumberToDisplayString((int)current.Byte.Value));
					}
				}
				stringBuilder.Append("]");
				if (action.IsVirtual.Value)
				{
					stringBuilder.Append("; " + Resources.Virtual);
				}
				e.Value = stringBuilder.ToString();
			}
		}

		private void UnboundColumnDataActionActive(ActionSendMessage action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = action.IsActive.Value;
				return;
			}
			bool value = Convert.ToBoolean(e.Value);
			bool flag;
			this.pageValidator.Grid.UpdateModel<bool>(value, action.IsActive, out flag);
			if (flag)
			{
				this.ValidateInput(true);
			}
		}

		private void UnboundColumnDataComment(ActionSendMessage action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = action.Comment.Value;
				return;
			}
			bool flag;
			this.pageValidator.Grid.UpdateModel<string>(e.Value.ToString(), action.Comment, out flag);
			if (flag)
			{
				this.ValidateInput(true);
			}
		}

		private void gridViewSendMessage_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			if (e.Column == this.colStartEventType)
			{
				ActionSendMessage actionSendMessage;
				if (this.TryGetAction(this.gridViewSendMessage.GetDataSourceRowIndex(e.RowHandle), out actionSendMessage))
				{
					if (actionSendMessage.Event is CANIdEvent)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageIDMessageCAN);
					}
					if (actionSendMessage.Event is FlexrayIdEvent)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageIDMessageFlexray);
					}
					if (actionSendMessage.Event is LINIdEvent)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageIDMessageLIN);
						return;
					}
					if (actionSendMessage.Event is SymbolicMessageEvent)
					{
						switch ((actionSendMessage.Event as SymbolicMessageEvent).BusType.Value)
						{
						case BusType.Bt_CAN:
							GridUtil.DrawImageTextCell(e, Resources.ImageSymbMessageCAN);
							return;
						case BusType.Bt_LIN:
							GridUtil.DrawImageTextCell(e, Resources.ImageSymbMessageLIN);
							return;
						case BusType.Bt_FlexRay:
							GridUtil.DrawImageTextCell(e, Resources.ImageSymbMessageFlexray);
							return;
						default:
							GridUtil.DrawImageTextCell(e, Resources.ImageSymbMessageCAN);
							return;
						}
					}
					else if (actionSendMessage.Event is SymbolicSignalEvent)
					{
						switch ((actionSendMessage.Event as SymbolicSignalEvent).BusType.Value)
						{
						case BusType.Bt_CAN:
							GridUtil.DrawImageTextCell(e, Resources.ImageSymbSignalCAN);
							return;
						case BusType.Bt_LIN:
							GridUtil.DrawImageTextCell(e, Resources.ImageSymbSignalLIN);
							return;
						case BusType.Bt_FlexRay:
							GridUtil.DrawImageTextCell(e, Resources.ImageSymbSignalFlexRay);
							return;
						default:
							GridUtil.DrawImageTextCell(e, Resources.ImageSymbSignalCAN);
							return;
						}
					}
					else
					{
						if (actionSendMessage.Event is KeyEvent)
						{
							GridUtil.DrawImageTextCell(e, Resources.ImageKey);
							return;
						}
						if (actionSendMessage.Event is DigitalInputEvent)
						{
							GridUtil.DrawImageTextCell(e, Resources.ImageDigitalInput);
							return;
						}
						if (actionSendMessage.Event is AnalogInputEvent)
						{
							GridUtil.DrawImageTextCell(e, Resources.ImageAnalogInputSignal);
							return;
						}
						if (actionSendMessage.Event is CANBusStatisticsEvent)
						{
							GridUtil.DrawImageTextCell(e, Resources.ImageCANBusStatistics);
							return;
						}
						if (actionSendMessage.Event is CANDataEvent)
						{
							GridUtil.DrawImageTextCell(e, Resources.ImageDataTriggerCAN);
							return;
						}
						if (actionSendMessage.Event is LINDataEvent)
						{
							GridUtil.DrawImageTextCell(e, Resources.ImageDataTriggerLIN);
							return;
						}
						if (actionSendMessage.Event is IgnitionEvent)
						{
							GridUtil.DrawImageTextCell(e, Resources.ImageOnIgnition);
							return;
						}
						if (actionSendMessage.Event is VoCanRecordingEvent)
						{
							if (string.IsNullOrEmpty(this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (actionSendMessage.Event as VoCanRecordingEvent).IsUsingCASM2T3L)))
							{
								GridUtil.DrawImageTextCell(e, Resources.ImageVoCanRecoding);
								return;
							}
							GridUtil.DrawImageTextCell(e, this.errorProviderGlobalModel.Icon.ToBitmap());
							return;
						}
						else if (actionSendMessage.Event is MsgTimeoutEvent)
						{
							switch ((actionSendMessage.Event as MsgTimeoutEvent).BusType.Value)
							{
							case BusType.Bt_CAN:
								GridUtil.DrawImageTextCell(e, Resources.ImageCycMsgTimeoutCAN);
								return;
							case BusType.Bt_LIN:
								GridUtil.DrawImageTextCell(e, Resources.ImageCycMsgTimeoutLIN);
								return;
							case BusType.Bt_FlexRay:
								GridUtil.DrawImageTextCell(e, Resources.ImageCycMsgTimeoutFlexray);
								return;
							default:
								GridUtil.DrawImageTextCell(e, Resources.ImageCycMsgTimeoutCAN);
								return;
							}
						}
					}
				}
			}
			else
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(e.Column, this.gridViewSendMessage.GetDataSourceRowIndex(e.RowHandle));
				this.customErrorProvider.Grid.DisplayError(gUIElement, e);
			}
		}

		private void gridViewSendMessage_RowStyle(object sender, RowStyleEventArgs e)
		{
			ActionSendMessage actionSendMessage;
			if (this.TryGetAction(this.gridViewSendMessage.GetDataSourceRowIndex(e.RowHandle), out actionSendMessage) && !actionSendMessage.IsActive.Value)
			{
				e.Appearance.ForeColor = SystemColors.GrayText;
			}
		}

		private void gridViewSendMessage_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (e.MenuType == GridMenuType.Column)
			{
				string localizedString = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnGroupBox);
				string localizedString2 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuGroupPanelShow);
				string localizedString3 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnFilter);
				string localizedString4 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnFilterEditor);
				string localizedString5 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnGroup);
				string localizedString6 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnUnGroup);
				string localizedString7 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnRemoveColumn);
				string localizedString8 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnColumnCustomization);
				for (int i = e.Menu.Items.Count - 1; i >= 0; i--)
				{
					string caption = e.Menu.Items[i].Caption;
					if (localizedString5 == caption || localizedString6 == caption || caption.Contains(localizedString) || localizedString2 == caption || localizedString3 == caption || localizedString4 == caption || localizedString7 == caption || localizedString8 == caption)
					{
						e.Menu.Items.RemoveAt(i);
					}
				}
			}
		}

		private void gridViewSendMessage_TopRowChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridViewSendMessage_LeftCoordChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void SendMessageGrid_Resize(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		public void DisplayErrors()
		{
			this.StoreMapping4VisibleCells();
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
		}

		public bool ValidateInput(bool isDataChanged = false)
		{
			this.ResetValidationFramework();
			this.StoreMapping4VisibleCells();
			bool flag = true;
			flag &= this.ModelValidator.Validate(this.sendMessageConfiguration, isDataChanged, this.pageValidator);
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

		private void StoreMapping4VisibleCells()
		{
			PageValidatorGridUtil.StoreRowMappingForVisibleRows(this.gridViewSendMessage, new PageValidatorGridUtil.StoreRowMappingHandler(this.OnPageValidatorGridUtilStoreRowMapping));
		}

		private void OnPageValidatorGridUtilStoreRowMapping(int dataSourceRowIdx)
		{
			if (dataSourceRowIdx < 0 || dataSourceRowIdx >= this.sendMessageConfiguration.Actions.Count)
			{
				return;
			}
			ActionSendMessage action = this.sendMessageConfiguration.Actions[dataSourceRowIdx];
			this.StoreMapping4VisibleColumns(action, dataSourceRowIdx);
		}

		private void StoreMapping4VisibleColumns(ActionSendMessage action, int dataSourceIdx)
		{
			if (PageValidatorGridUtil.IsColumnVisible(this.colStartEventType, this.gridViewSendMessage))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colStartEventType, dataSourceIdx);
				if (action.Event is VoCanRecordingEvent)
				{
					this.pageValidator.Grid.StoreMapping((action.Event as VoCanRecordingEvent).IsUsingCASM2T3L, gUIElement);
				}
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colStartCondition, this.gridViewSendMessage))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colStartCondition, dataSourceIdx);
				IEnumerable<IValidatedProperty> validatedPropertyListCondition = this.GetValidatedPropertyListCondition(action);
				foreach (IValidatedProperty current in validatedPropertyListCondition)
				{
					this.pageValidator.Grid.StoreMapping(current, gUIElement);
				}
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colSendMessage, this.gridViewSendMessage))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colSendMessage, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(action.BusType, gUIElement);
				this.pageValidator.Grid.StoreMapping(action.ChannelNumber, gUIElement);
				this.pageValidator.Grid.StoreMapping(action.IsSymbolic, gUIElement);
				this.pageValidator.Grid.StoreMapping(action.DatabasePath, gUIElement);
				this.pageValidator.Grid.StoreMapping(action.NetworkName, gUIElement);
				this.pageValidator.Grid.StoreMapping(action.SymbolName, gUIElement);
				this.pageValidator.Grid.StoreMapping(action.IsVirtual, gUIElement);
				this.pageValidator.Grid.StoreMapping(action.ID, gUIElement);
				this.pageValidator.Grid.StoreMapping(action.IsExtendedId, gUIElement);
				foreach (DataItem current2 in action.MessageData)
				{
					this.pageValidator.Grid.StoreMapping(current2.Byte, gUIElement);
				}
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colComment, this.gridViewSendMessage))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colComment, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(action.Comment, gUIElement);
			}
		}

		private IEnumerable<IValidatedProperty> GetValidatedPropertyListCondition(ActionSendMessage action)
		{
			List<IValidatedProperty> list = new List<IValidatedProperty>();
			if (action.Event is IdEvent)
			{
				list.Add((action.Event as IdEvent).ChannelNumber);
				list.Add((action.Event as IdEvent).IdRelation);
				list.Add((action.Event as IdEvent).LowId);
				list.Add((action.Event as IdEvent).HighId);
				if (action.Event is CANIdEvent)
				{
					list.Add((action.Event as CANIdEvent).IsExtendedId);
				}
			}
			else if (action.Event is SymbolicMessageEvent)
			{
				list.Add((action.Event as SymbolicMessageEvent).ChannelNumber);
				list.Add((action.Event as SymbolicMessageEvent).DatabasePath);
				list.Add((action.Event as SymbolicMessageEvent).NetworkName);
				list.Add((action.Event as SymbolicMessageEvent).MessageName);
				list.Add((action.Event as SymbolicMessageEvent).BusType);
			}
			else if (action.Event is ISymbolicSignalEvent)
			{
				list.Add((action.Event as ISymbolicSignalEvent).LowValue);
				list.Add((action.Event as ISymbolicSignalEvent).HighValue);
				list.Add((action.Event as ISymbolicSignalEvent).SignalName);
				list.Add((action.Event as ISymbolicSignalEvent).Relation);
				if (action.Event is SymbolicSignalEvent)
				{
					list.Add((action.Event as ISymbolicSignalEvent).ChannelNumber);
					list.Add((action.Event as ISymbolicSignalEvent).NetworkName);
					list.Add((action.Event as ISymbolicSignalEvent).MessageName);
					list.Add((action.Event as ISymbolicSignalEvent).DatabasePath);
				}
			}
			else if (action.Event is DigitalInputEvent)
			{
				list.Add((action.Event as DigitalInputEvent).Edge);
				list.Add((action.Event as DigitalInputEvent).DigitalInput);
				list.Add((action.Event as DigitalInputEvent).DebounceTime);
				list.Add((action.Event as DigitalInputEvent).IsDebounceActive);
			}
			else if (action.Event is AnalogInputEvent)
			{
				list.Add((action.Event as AnalogInputEvent).InputNumber);
				list.Add((action.Event as AnalogInputEvent).LowValue);
				list.Add((action.Event as AnalogInputEvent).HighValue);
				list.Add((action.Event as AnalogInputEvent).Relation);
				list.Add((action.Event as AnalogInputEvent).Tolerance);
			}
			else if (action.Event is CANDataEvent)
			{
				CANDataEvent cANDataEvent = action.Event as CANDataEvent;
				list.Add(cANDataEvent.ChannelNumber);
				list.Add(cANDataEvent.ID);
				list.Add(cANDataEvent.IsExtendedId);
				if (cANDataEvent.RawDataSignal is RawDataSignalByte)
				{
					list.Add((cANDataEvent.RawDataSignal as RawDataSignalByte).DataBytePos);
				}
				else if (cANDataEvent.RawDataSignal is RawDataSignalStartbitLength)
				{
					list.Add((cANDataEvent.RawDataSignal as RawDataSignalStartbitLength).StartbitPos);
					list.Add((cANDataEvent.RawDataSignal as RawDataSignalStartbitLength).Length);
					list.Add((cANDataEvent.RawDataSignal as RawDataSignalStartbitLength).IsMotorola);
				}
				list.Add(cANDataEvent.Relation);
				list.Add(cANDataEvent.LowValue);
				list.Add(cANDataEvent.HighValue);
			}
			else if (action.Event is LINDataEvent)
			{
				LINDataEvent lINDataEvent = action.Event as LINDataEvent;
				list.Add(lINDataEvent.ChannelNumber);
				list.Add(lINDataEvent.ID);
				if (lINDataEvent.RawDataSignal is RawDataSignalByte)
				{
					list.Add((lINDataEvent.RawDataSignal as RawDataSignalByte).DataBytePos);
				}
				else if (lINDataEvent.RawDataSignal is RawDataSignalStartbitLength)
				{
					list.Add((lINDataEvent.RawDataSignal as RawDataSignalStartbitLength).StartbitPos);
					list.Add((lINDataEvent.RawDataSignal as RawDataSignalStartbitLength).Length);
					list.Add((lINDataEvent.RawDataSignal as RawDataSignalStartbitLength).IsMotorola);
				}
				list.Add(lINDataEvent.Relation);
				list.Add(lINDataEvent.LowValue);
				list.Add(lINDataEvent.HighValue);
			}
			else if (action.Event is MsgTimeoutEvent)
			{
				list.Add((action.Event as MsgTimeoutEvent).DatabasePath);
				list.Add((action.Event as MsgTimeoutEvent).DatabaseName);
				list.Add((action.Event as MsgTimeoutEvent).MessageName);
				list.Add((action.Event as MsgTimeoutEvent).NetworkName);
				list.Add((action.Event as MsgTimeoutEvent).ChannelNumber);
				list.Add((action.Event as MsgTimeoutEvent).BusType);
				list.Add((action.Event as MsgTimeoutEvent).IsSymbolic);
				list.Add((action.Event as MsgTimeoutEvent).ID);
				list.Add((action.Event as MsgTimeoutEvent).IsExtendedId);
				list.Add((action.Event as MsgTimeoutEvent).IsCycletimeFromDatabase);
				list.Add((action.Event as MsgTimeoutEvent).UserDefinedCycleTime);
				list.Add((action.Event as MsgTimeoutEvent).MaxDelay);
			}
			else if (action.Event is KeyEvent)
			{
				list.Add((action.Event as KeyEvent).Number);
				list.Add((action.Event as KeyEvent).IsOnPanel);
			}
			else if (action.Event is CANBusStatisticsEvent)
			{
				list.Add((action.Event as CANBusStatisticsEvent).ChannelNumber);
				list.Add((action.Event as CANBusStatisticsEvent).IsBusloadEnabled);
				list.Add((action.Event as CANBusStatisticsEvent).BusloadRelation);
				list.Add((action.Event as CANBusStatisticsEvent).BusloadLow);
				list.Add((action.Event as CANBusStatisticsEvent).BusloadHigh);
				list.Add((action.Event as CANBusStatisticsEvent).IsErrorFramesEnabled);
				list.Add((action.Event as CANBusStatisticsEvent).ErrorFramesRelation);
				list.Add((action.Event as CANBusStatisticsEvent).ErrorFramesLow);
			}
			else if (action.Event is IgnitionEvent)
			{
				list.Add((action.Event as IgnitionEvent).IsOn);
			}
			else if (action.Event is VoCanRecordingEvent)
			{
				list.Add((action.Event as VoCanRecordingEvent).Duration_s);
				list.Add((action.Event as VoCanRecordingEvent).IsBeepOnEndOn);
				list.Add((action.Event as VoCanRecordingEvent).IsRecordingLEDActive);
			}
			return list;
		}

		public bool TryGetSelectedAction(out ActionSendMessage action)
		{
			int num;
			return this.TryGetSelectedAction(out action, out num);
		}

		private bool TryGetSelectedAction(out ActionSendMessage action, out int idx)
		{
			action = null;
			idx = this.gridViewSendMessage.GetFocusedDataSourceRowIndex();
			if (idx < 0 || idx > this.sendMessageConfiguration.Actions.Count - 1)
			{
				return false;
			}
			action = this.sendMessageConfiguration.Actions[idx];
			return null != action;
		}

		private bool TryGetAction(int listSourceRowIndex, out ActionSendMessage action)
		{
			action = null;
			if (this.sendMessageConfiguration == null)
			{
				return false;
			}
			if (listSourceRowIndex < 0 || listSourceRowIndex > this.sendMessageConfiguration.Actions.Count - 1)
			{
				return false;
			}
			action = this.sendMessageConfiguration.Actions[listSourceRowIndex];
			return null != action;
		}

		public void SelectRowOfAction(ActionSendMessage action)
		{
			for (int i = 0; i < this.gridViewSendMessage.RowCount; i++)
			{
				IList<ActionSendMessage> list = this.gridViewSendMessage.DataSource as IList<ActionSendMessage>;
				if (list != null)
				{
					ActionSendMessage actionSendMessage = list[this.gridViewSendMessage.GetDataSourceRowIndex(i)];
					if (actionSendMessage == action)
					{
						this.gridViewSendMessage.FocusedRowHandle = i;
						return;
					}
				}
			}
		}

		private void repositoryItemButtonEditStartCondition_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			int arg_0B_0 = this.gridViewSendMessage.FocusedRowHandle;
			ActionSendMessage action;
			if (!this.TryGetSelectedAction(out action))
			{
				return;
			}
			if (this.EditEventCondition(action))
			{
				this.gridViewSendMessage.RefreshData();
				this.ValidateInput(true);
			}
		}

		private void repositoryItemButtonEditSendMessage_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			int arg_0B_0 = this.gridViewSendMessage.FocusedRowHandle;
			ActionSendMessage action;
			if (!this.TryGetSelectedAction(out action))
			{
				return;
			}
			if (this.EditSendMessage(action))
			{
				this.gridViewSendMessage.RefreshData();
				this.ValidateInput(true);
			}
		}

		private void repositoryItemMemoExEditComment_EditValueChanged(object sender, EventArgs e)
		{
			this.gridViewSendMessage.PostEditor();
		}

		private void repositoryItemCheckEditIsActive_CheckedChanged(object sender, EventArgs e)
		{
			this.gridViewSendMessage.PostEditor();
		}

		private void gridViewSendMessage_Click(object sender, EventArgs e)
		{
			GridHitInfo gridHitInfo = this.gridViewSendMessage.CalcHitInfo(this.gridControlSendMessage.PointToClient(Control.MousePosition));
			ActionSendMessage actionSendMessage;
			if (gridHitInfo.InRowCell && gridHitInfo.Column == this.colActive && this.TryGetAction(this.gridViewSendMessage.GetDataSourceRowIndex(gridHitInfo.RowHandle), out actionSendMessage))
			{
				actionSendMessage.IsActive.Value = !actionSendMessage.IsActive.Value;
			}
		}

		private void gridViewSendMessage_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			ActionSendMessage actionSendMessage;
			if (e.Column == this.colStartCondition && this.TryGetAction(this.gridViewSendMessage.GetDataSourceRowIndex(e.RowHandle), out actionSendMessage))
			{
				if (actionSendMessage.Event is KeyEvent)
				{
					e.RepositoryItem = this.repositoryItemComboBoxKeyCondition;
					return;
				}
				if (actionSendMessage.Event is IgnitionEvent)
				{
					e.RepositoryItem = this.repositoryItemComboBoxIgnitionCondition;
					return;
				}
				e.RepositoryItem = this.repositoryItemButtonEditStartCondition;
			}
		}

		private void gridViewSendMessage_ShownEditor(object sender, EventArgs e)
		{
			if (this.gridViewSendMessage.FocusedColumn == this.colStartEventType)
			{
				this.ShownEditorEventType();
				return;
			}
			ActionSendMessage actionSendMessage;
			if (this.gridViewSendMessage.FocusedColumn == this.colStartCondition && this.TryGetSelectedAction(out actionSendMessage))
			{
				if (actionSendMessage.Event is IgnitionEvent)
				{
					ComboBoxEdit comboBoxEdit = this.gridViewSendMessage.ActiveEditor as ComboBoxEdit;
					if (comboBoxEdit != null)
					{
						comboBoxEdit.Properties.Items.Clear();
						comboBoxEdit.Properties.Items.Add(Resources.IgnitionOff);
						comboBoxEdit.Properties.Items.Add(Resources.IgnitionOn);
						return;
					}
				}
				else if (actionSendMessage.Event is KeyEvent)
				{
					ComboBoxEdit comboBoxEdit2 = this.gridViewSendMessage.ActiveEditor as ComboBoxEdit;
					if (comboBoxEdit2 != null)
					{
						comboBoxEdit2.Properties.Items.Clear();
						for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.IO.NumberOfKeys; num += 1u)
						{
							comboBoxEdit2.Properties.Items.Add(GUIUtil.MapKeyNumber2String(num, false));
						}
						for (uint num2 = 1u; num2 <= this.ModelValidator.LoggerSpecifics.IO.NumberOfPanelKeys; num2 += 1u)
						{
							comboBoxEdit2.Properties.Items.Add(GUIUtil.MapKeyNumber2String(num2, true));
						}
						for (uint num3 = 1u + Constants.CasKeyOffset; num3 <= this.ModelValidator.LoggerSpecifics.IO.NumberOfCasKeys + Constants.CasKeyOffset; num3 += 1u)
						{
							comboBoxEdit2.Properties.Items.Add(GUIUtil.MapKeyNumber2String(num3, false));
						}
					}
				}
			}
		}

		private void repositoryItemComboBoxKeyCondition_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewSendMessage.PostEditor();
		}

		private void repositoryItemComboBoxIgnitionCondition_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewSendMessage.PostEditor();
		}

		private void repositoryItemImageComboBoxStartEvent_SelectedIndexChanged(object sender, EventArgs e)
		{
			ImageComboBoxEdit imageComboBoxEdit = sender as ImageComboBoxEdit;
			if (imageComboBoxEdit != null && imageComboBoxEdit.SelectedIndex != this.oldSelectedEventIndex)
			{
				Event @event = this.CreateAndEditEvent(imageComboBoxEdit.SelectedItem.ToString());
				if (@event != null)
				{
					ActionSendMessage actionSendMessage;
					if (!this.TryGetSelectedAction(out actionSendMessage))
					{
						return;
					}
					actionSendMessage.Event = @event;
					this.gridViewSendMessage.RefreshData();
					this.ValidateInput(true);
				}
				else
				{
					imageComboBoxEdit.SelectedIndex = this.oldSelectedEventIndex;
					this.gridViewSendMessage.RefreshData();
				}
			}
			this.oldSelectedEventIndex = -1;
			this.gridViewSendMessage.PostEditor();
		}

		private void ShownEditorEventType()
		{
			ImageComboBoxEdit imageComboBoxEdit = this.gridViewSendMessage.ActiveEditor as ImageComboBoxEdit;
			if (imageComboBoxEdit != null)
			{
				this.oldSelectedEventIndex = imageComboBoxEdit.SelectedIndex;
			}
		}

		private void toolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
		{
			try
			{
				GridHitInfo gridHitInfo = this.gridViewSendMessage.CalcHitInfo(e.ControlMousePosition);
				if (gridHitInfo.IsValid)
				{
					ActionSendMessage actionSendMessage;
					if (gridHitInfo.Column == this.colComment && this.TryGetAction(this.gridViewSendMessage.GetDataSourceRowIndex(gridHitInfo.RowHandle), out actionSendMessage) && !this.pageValidator.General.HasError(actionSendMessage.Comment))
					{
						object rowCellValue = this.gridViewSendMessage.GetRowCellValue(gridHitInfo.RowHandle, this.colComment);
						if (rowCellValue != null && !string.IsNullOrEmpty(rowCellValue.ToString()))
						{
							e.Info = new ToolTipControlInfo(new CellToolTipInfo(gridHitInfo.RowHandle, gridHitInfo.Column, "cell"), rowCellValue.ToString());
							return;
						}
					}
					if (gridHitInfo.Column == this.colStartEventType && this.TryGetAction(this.gridViewSendMessage.GetDataSourceRowIndex(gridHitInfo.RowHandle), out actionSendMessage) && actionSendMessage.Event is VoCanRecordingEvent)
					{
						string errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (actionSendMessage.Event as VoCanRecordingEvent).IsUsingCASM2T3L);
						if (!string.IsNullOrEmpty(errorText))
						{
							e.Info = new ToolTipControlInfo(new CellToolTipInfo(gridHitInfo.RowHandle, gridHitInfo.Column, "cell"), errorText);
						}
					}
				}
			}
			catch
			{
			}
		}

		private bool EditSendMessage(ActionSendMessage action)
		{
			ConfigureSendMessage configureSendMessageDialog = this.GetConfigureSendMessageDialog();
			configureSendMessageDialog.ActionSendMessage = new ActionSendMessage(action);
			if (DialogResult.OK == configureSendMessageDialog.ShowDialog() && !action.Equals(configureSendMessageDialog.ActionSendMessage))
			{
				action.Assign(configureSendMessageDialog.ActionSendMessage);
				return true;
			}
			return false;
		}

		private Event CreateAndEditEvent(string eventTypeName)
		{
			if (eventTypeName == Vocabulary.TriggerTypeNameColOnStart)
			{
				return this.CreateOnStartEvent();
			}
			if (eventTypeName == Vocabulary.TriggerTypeNameColCANId)
			{
				return this.CreateCANIdEvent();
			}
			if (eventTypeName == Vocabulary.TriggerTypeNameColLINId)
			{
				return this.CreateLINIdEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColFlexray)
			{
				return this.CreateFlexrayIdEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColCANData)
			{
				return this.CreateCANDataEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColLINData)
			{
				return this.CreateLINDataEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicCAN)
			{
				return this.CreateSymMessageEvent(BusType.Bt_CAN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicLIN)
			{
				return this.CreateSymMessageEvent(BusType.Bt_LIN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicFlexray)
			{
				return this.CreateSymMessageEvent(BusType.Bt_FlexRay);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
			{
				return this.CreateSymSignalEvent(BusType.Bt_CAN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
			{
				return this.CreateSymSignalEvent(BusType.Bt_LIN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
			{
				return this.CreateSymSignalEvent(BusType.Bt_FlexRay);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColCANMsgTimeout)
			{
				return this.CreateMsgTimeoutEvent(BusType.Bt_CAN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColLINMsgTimeout)
			{
				return this.CreateMsgTimeoutEvent(BusType.Bt_LIN);
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColDigitalInput)
			{
				return this.CreateDigitalInputEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColAnalogInput)
			{
				return this.CreateAnalogInputEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColCANBusStatistics)
			{
				return this.CreateCANBusStatisticsEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColKey)
			{
				return this.CreateKeyEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColIgnition)
			{
				return this.CreateIgnitionEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColVoCanRecording)
			{
				return this.CreateVoCANRecordingEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColCcpXcpSignal)
			{
				return this.CreateCcpXcpSignalEvent();
			}
			if (eventTypeName == Resources_Trigger.TriggerTypeNameColDiagnosticSignal)
			{
				return this.CreateDiagnosticSignalEvent();
			}
			return null;
		}

		public OnStartEvent CreateOnStartEvent()
		{
			OnStartCondition onStartConditionDialog = this.GetOnStartConditionDialog();
			if (DialogResult.OK == onStartConditionDialog.ShowDialog())
			{
				return new OnStartEvent
				{
					Delay = 
					{
						Value = onStartConditionDialog.Delay
					}
				};
			}
			return null;
		}

		public CyclicTimerEvent CreateCyclicTimerEvent()
		{
			CyclicTimerCondition cyclicTimerConditionDialog = this.GetCyclicTimerConditionDialog();
			if (DialogResult.OK == cyclicTimerConditionDialog.ShowDialog())
			{
				return new CyclicTimerEvent(cyclicTimerConditionDialog.CyclicTimerEvent);
			}
			return null;
		}

		public CANIdEvent CreateCANIdEvent()
		{
			CANIdCondition cANIdConditionDialog = this.GetCANIdConditionDialog();
			if (DialogResult.OK == cANIdConditionDialog.ShowDialog())
			{
				return new CANIdEvent(cANIdConditionDialog.CANIdEvent);
			}
			return null;
		}

		public LINIdEvent CreateLINIdEvent()
		{
			LINIdCondition lINIdConditionDialog = this.GetLINIdConditionDialog();
			if (DialogResult.OK == lINIdConditionDialog.ShowDialog())
			{
				return new LINIdEvent(lINIdConditionDialog.LINIdEvent);
			}
			return null;
		}

		public FlexrayIdEvent CreateFlexrayIdEvent()
		{
			FlexrayIdCondition flexrayIdConditionDialog = this.GetFlexrayIdConditionDialog();
			if (DialogResult.OK == flexrayIdConditionDialog.ShowDialog())
			{
				return new FlexrayIdEvent(flexrayIdConditionDialog.FlexrayIdEvent);
			}
			return null;
		}

		public CANDataEvent CreateCANDataEvent()
		{
			CANDataCondition cANDataConditionDialog = this.GetCANDataConditionDialog();
			if (DialogResult.OK == cANDataConditionDialog.ShowDialog())
			{
				return new CANDataEvent(cANDataConditionDialog.CANDataEvent);
			}
			return null;
		}

		public LINDataEvent CreateLINDataEvent()
		{
			LINDataCondition lINDataConditionDialog = this.GetLINDataConditionDialog();
			if (DialogResult.OK == lINDataConditionDialog.ShowDialog())
			{
				return new LINDataEvent(lINDataConditionDialog.LINDataEvent);
			}
			return null;
		}

		public SymbolicMessageEvent CreateSymMessageEvent(BusType busType)
		{
			if (!this.ModelValidator.DatabaseServices.HasDatabasesConfiguredFor(busType))
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
			if (!this.ApplicationDatabaseManager.SelectMessageInDatabase(ref text, ref value, ref text2, ref text3, ref busType, ref value2))
			{
				return null;
			}
			string message;
			if (!this.ModelValidator.DatabaseServices.IsSymbolicMessageInsertAllowed(text, text3, text2, busType, out message))
			{
				InformMessageBox.Error(message);
				return null;
			}
			SymbolicMessageEvent symbolicMessageEvent = new SymbolicMessageEvent();
			symbolicMessageEvent.MessageName.Value = text;
			symbolicMessageEvent.DatabaseName.Value = value;
			symbolicMessageEvent.DatabasePath.Value = this.ModelValidator.GetFilePathRelativeToConfiguration(text2);
			symbolicMessageEvent.NetworkName.Value = text3;
			symbolicMessageEvent.BusType.Value = busType;
			symbolicMessageEvent.IsFlexrayPDU.Value = value2;
			symbolicMessageEvent.ChannelNumber.Value = 1u;
			IList<uint> channelAssignmentOfDatabase = this.ModelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(symbolicMessageEvent.DatabasePath.Value, symbolicMessageEvent.NetworkName.Value);
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
			return symbolicMessageEvent;
		}

		public SymbolicSignalEvent CreateSymSignalEvent(BusType busType)
		{
			if (!this.ModelValidator.DatabaseServices.HasDatabasesConfiguredFor(busType))
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
			if (this.ApplicationDatabaseManager.SelectSignalInDatabase(ref text, ref text2, ref value, ref text3, ref text4, ref busType, ref value2))
			{
				string message;
				if (!this.ModelValidator.DatabaseServices.IsSymbolicSignalInsertAllowed(text2, text, text4, text3, busType, out message))
				{
					InformMessageBox.Error(message);
					return null;
				}
				SymbolicSignalEvent symbolicSignalEvent = new SymbolicSignalEvent();
				symbolicSignalEvent.MessageName.Value = text;
				symbolicSignalEvent.SignalName.Value = text2;
				symbolicSignalEvent.DatabaseName.Value = value;
				symbolicSignalEvent.DatabasePath.Value = this.ModelValidator.GetFilePathRelativeToConfiguration(text3);
				symbolicSignalEvent.NetworkName.Value = text4;
				symbolicSignalEvent.ChannelNumber.Value = 1u;
				IList<uint> channelAssignmentOfDatabase = this.ModelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(symbolicSignalEvent.DatabasePath.Value, symbolicSignalEvent.NetworkName.Value);
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
				using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.ModelValidator, this.ApplicationDatabaseManager, null))
				{
					symbolicSignalCondition.SignalEvent = new SymbolicSignalEvent(symbolicSignalEvent);
					if (DialogResult.OK == symbolicSignalCondition.ShowDialog())
					{
						symbolicSignalEvent.Assign(symbolicSignalCondition.SignalEvent);
						return symbolicSignalEvent;
					}
				}
			}
			return null;
		}

		public MsgTimeoutEvent CreateMsgTimeoutEvent(BusType busType)
		{
			MsgTimeoutCondition msgTimeoutCondition = this.GetMsgTimeoutConditionDialog();
			msgTimeoutCondition.InitDefaultValues(busType);
			if (DialogResult.OK == msgTimeoutCondition.ShowDialog())
			{
				return new MsgTimeoutEvent(msgTimeoutCondition.MsgTimeoutEvent);
			}
			return null;
		}

		public DigitalInputEvent CreateDigitalInputEvent()
		{
			DigitalInputCondition digitalInputConditionDialog = this.GetDigitalInputConditionDialog();
			if (DialogResult.OK == digitalInputConditionDialog.ShowDialog())
			{
				return new DigitalInputEvent(digitalInputConditionDialog.DigitalInputEvent);
			}
			return null;
		}

		public AnalogInputEvent CreateAnalogInputEvent()
		{
			AnalogInputCondition analogInputCondition = this.GetAnalogInputConditionDialog();
			if (DialogResult.OK == analogInputCondition.ShowDialog())
			{
				return new AnalogInputEvent(analogInputCondition.AnalogInputEvent);
			}
			return null;
		}

		public KeyEvent CreateKeyEvent()
		{
			KeyCondition keyCondition = this.GetKeyConditionDialog();
			if (DialogResult.OK == keyCondition.ShowDialog())
			{
				return new KeyEvent(keyCondition.KeyEvent);
			}
			return null;
		}

		public IgnitionEvent CreateIgnitionEvent()
		{
			IgnitionCondition ignitionCondition = this.GetIgnitionConditionDialog();
			if (DialogResult.OK == ignitionCondition.ShowDialog())
			{
				return new IgnitionEvent(ignitionCondition.IgnitionEvent);
			}
			return null;
		}

		public VoCanRecordingEvent CreateVoCANRecordingEvent()
		{
			int num;
			bool flag;
			if (this.ModelValidator.IsActiveVoCANEventConfigured(out num, out flag))
			{
				InformMessageBox.Error(Resources_Trigger.ErrorOnlyOneVoCANEventAllowed);
				return null;
			}
			VoCanRecordingCondition voCANRecordingConditionDialog = this.GetVoCANRecordingConditionDialog();
			if (DialogResult.OK == voCANRecordingConditionDialog.ShowDialog())
			{
				VoCanRecordingEvent result = new VoCanRecordingEvent(voCANRecordingConditionDialog.VoCanRecordingEvent);
				string value;
				if (!this.SemanticChecker.IsCANChannelConfigSoundForVoCANTrigger(out value))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendLine(value);
					stringBuilder.AppendLine(Resources.QuestionAdjustSettingsAutomatically);
					if (InformMessageBox.Question(stringBuilder.ToString()) == DialogResult.Yes)
					{
						this.ModelEditor.SetChannelConfigurationForVoCAN();
					}
				}
				return result;
			}
			return null;
		}

		public CANBusStatisticsEvent CreateCANBusStatisticsEvent()
		{
			CANBusStatisticsCondition cANBusStatisticsDialog = this.GetCANBusStatisticsDialog();
			if (DialogResult.OK == cANBusStatisticsDialog.ShowDialog())
			{
				return new CANBusStatisticsEvent(cANBusStatisticsDialog.CANBusStatisticsEvent);
			}
			return null;
		}

		public CcpXcpSignalEvent CreateCcpXcpSignalEvent()
		{
			if (!CcpXcpManager.Instance().CheckTriggerSignalEvents())
			{
				return null;
			}
			CcpXcpSignalEvent ccpXcpSignalEvent = new CcpXcpSignalEvent();
			if (ccpXcpSignalEvent == null)
			{
				return null;
			}
			ccpXcpSignalEvent.SignalName.Value = string.Empty;
			ccpXcpSignalEvent.CcpXcpEcuName.Value = string.Empty;
			ccpXcpSignalEvent.LowValue.Value = 0.0;
			ccpXcpSignalEvent.HighValue.Value = 0.0;
			ccpXcpSignalEvent.Relation.Value = CondRelation.Equal;
			using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.ModelValidator, this.ApplicationDatabaseManager, null))
			{
				symbolicSignalCondition.SignalEvent = new CcpXcpSignalEvent(ccpXcpSignalEvent);
				symbolicSignalCondition.CcpXcpSignalConfiguration = CcpXcpManager.Instance().ConfigurationManagerService.CcpXcpSignalConfiguration;
				if (DialogResult.OK == symbolicSignalCondition.ShowDialog())
				{
					ccpXcpSignalEvent.Assign(symbolicSignalCondition.SignalEvent);
					return ccpXcpSignalEvent;
				}
			}
			return null;
		}

		public DiagnosticSignalEvent CreateDiagnosticSignalEvent()
		{
			if (!this.ModelValidator.HasDiagnosticsDatabasesConfigured())
			{
				InformMessageBox.Error(Resources.ErrorUnableToAddSigReq);
				return null;
			}
			if (!this.DiagnosticActionsConfiguration.DiagnosticActions.Any<DiagnosticAction>())
			{
				InformMessageBox.Error(Resources.ErrorNoDiagnosticSignalRequests);
				return null;
			}
			DiagnosticSignalEvent diagnosticSignalEvent = new DiagnosticSignalEvent();
			if (diagnosticSignalEvent == null)
			{
				return null;
			}
			diagnosticSignalEvent.SignalName.Value = string.Empty;
			diagnosticSignalEvent.DiagnosticEcuName.Value = string.Empty;
			diagnosticSignalEvent.LowValue.Value = 0.0;
			diagnosticSignalEvent.HighValue.Value = 0.0;
			diagnosticSignalEvent.Relation.Value = CondRelation.Equal;
			using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.ModelValidator, this.ApplicationDatabaseManager, null))
			{
				symbolicSignalCondition.SignalEvent = new DiagnosticSignalEvent(diagnosticSignalEvent);
				symbolicSignalCondition.DiagnosticActionsConfiguration = this.DiagnosticActionsConfiguration;
				symbolicSignalCondition.DiagnosticsDatabaseConfiguration = this.DiagnosticDatabaseConfiguration;
				if (DialogResult.OK == symbolicSignalCondition.ShowDialog())
				{
					diagnosticSignalEvent.Assign(symbolicSignalCondition.SignalEvent);
					return diagnosticSignalEvent;
				}
			}
			return null;
		}

		private bool EditEventCondition(ActionSendMessage action)
		{
			if (action.Event is CANIdEvent)
			{
				return this.EditCANIdCondition(action.Event as CANIdEvent);
			}
			if (action.Event is LINIdEvent)
			{
				return this.EditLINIdCondition(action.Event as LINIdEvent);
			}
			if (action.Event is FlexrayIdEvent)
			{
				return this.EditFlexrayIdCondition(action.Event as FlexrayIdEvent);
			}
			if (action.Event is CANDataEvent)
			{
				return this.EditCANDataTriggerCondition(action.Event as CANDataEvent);
			}
			if (action.Event is LINDataEvent)
			{
				return this.EditLINDataCondition(action.Event as LINDataEvent);
			}
			if (action.Event is SymbolicMessageEvent)
			{
				return this.EditSymbolicMessageCondition(action.Event as SymbolicMessageEvent);
			}
			if (action.Event is ISymbolicSignalEvent)
			{
				return this.EditSymbolicSignalCondition(action.Event as ISymbolicSignalEvent);
			}
			if (action.Event is MsgTimeoutEvent)
			{
				return this.EditMsgTimeoutCondition(action.Event as MsgTimeoutEvent);
			}
			if (action.Event is CANBusStatisticsEvent)
			{
				return this.EditCANBusStatisticsCondition(action.Event as CANBusStatisticsEvent);
			}
			if (action.Event is KeyEvent)
			{
				return this.EditKeyCondition(action.Event as KeyEvent);
			}
			if (action.Event is DigitalInputEvent)
			{
				return this.EditDigitalInputCondition(action.Event as DigitalInputEvent);
			}
			if (action.Event is AnalogInputEvent)
			{
				return this.EditAnalogInputCondition(action.Event as AnalogInputEvent);
			}
			if (action.Event is VoCanRecordingEvent)
			{
				return this.EditVoCANRecordingEvent(action.Event as VoCanRecordingEvent);
			}
			return action.Event is IgnitionEvent && this.EditIgnitionCondition(action.Event as IgnitionEvent);
		}

		private bool EditCANIdCondition(CANIdEvent canIdEvent)
		{
			if (canIdEvent == null)
			{
				return false;
			}
			CANIdCondition cANIdConditionDialog = this.GetCANIdConditionDialog();
			cANIdConditionDialog.CANIdEvent = new CANIdEvent(canIdEvent);
			if (DialogResult.OK == cANIdConditionDialog.ShowDialog() && !canIdEvent.Equals(cANIdConditionDialog.CANIdEvent))
			{
				canIdEvent.Assign(cANIdConditionDialog.CANIdEvent);
				return true;
			}
			return false;
		}

		private bool EditFlexrayIdCondition(FlexrayIdEvent frIdEvent)
		{
			if (frIdEvent == null)
			{
				return false;
			}
			FlexrayIdCondition flexrayIdConditionDialog = this.GetFlexrayIdConditionDialog();
			flexrayIdConditionDialog.FlexrayIdEvent = new FlexrayIdEvent(frIdEvent);
			if (DialogResult.OK == flexrayIdConditionDialog.ShowDialog() && !frIdEvent.Equals(flexrayIdConditionDialog.FlexrayIdEvent))
			{
				frIdEvent.Assign(flexrayIdConditionDialog.FlexrayIdEvent);
				return true;
			}
			return false;
		}

		private bool EditCANBusStatisticsCondition(CANBusStatisticsEvent busStatEvent)
		{
			if (busStatEvent == null)
			{
				return false;
			}
			CANBusStatisticsCondition cANBusStatisticsDialog = this.GetCANBusStatisticsDialog();
			cANBusStatisticsDialog.CANBusStatisticsEvent = new CANBusStatisticsEvent(busStatEvent);
			if (DialogResult.OK == cANBusStatisticsDialog.ShowDialog() && !busStatEvent.Equals(cANBusStatisticsDialog.CANBusStatisticsEvent))
			{
				busStatEvent.Assign(cANBusStatisticsDialog.CANBusStatisticsEvent);
				return true;
			}
			return false;
		}

		private bool EditLINIdCondition(LINIdEvent linIdEvent)
		{
			if (linIdEvent == null)
			{
				return false;
			}
			LINIdCondition lINIdConditionDialog = this.GetLINIdConditionDialog();
			lINIdConditionDialog.LINIdEvent = new LINIdEvent(linIdEvent);
			if (DialogResult.OK == lINIdConditionDialog.ShowDialog() && !linIdEvent.Equals(lINIdConditionDialog.LINIdEvent))
			{
				linIdEvent.Assign(lINIdConditionDialog.LINIdEvent);
				return true;
			}
			return false;
		}

		private bool EditCANDataTriggerCondition(CANDataEvent canDataEvent)
		{
			if (canDataEvent == null)
			{
				return false;
			}
			CANDataCondition cANDataConditionDialog = this.GetCANDataConditionDialog();
			cANDataConditionDialog.CANDataEvent = new CANDataEvent(canDataEvent);
			if (DialogResult.OK == cANDataConditionDialog.ShowDialog() && !canDataEvent.Equals(cANDataConditionDialog.CANDataEvent))
			{
				canDataEvent.Assign(cANDataConditionDialog.CANDataEvent);
				return true;
			}
			return false;
		}

		private bool EditLINDataCondition(LINDataEvent linDataEvent)
		{
			if (linDataEvent == null)
			{
				return false;
			}
			LINDataCondition lINDataConditionDialog = this.GetLINDataConditionDialog();
			lINDataConditionDialog.LINDataEvent = new LINDataEvent(linDataEvent);
			if (DialogResult.OK == lINDataConditionDialog.ShowDialog() && !linDataEvent.Equals(lINDataConditionDialog.LINDataEvent))
			{
				linDataEvent.Assign(lINDataConditionDialog.LINDataEvent);
				return true;
			}
			return false;
		}

		private bool EditSymbolicMessageCondition(SymbolicMessageEvent symbolicMsgEvent)
		{
			if (symbolicMsgEvent == null)
			{
				return false;
			}
			SymbolicMessageCondition symbolicMessageCondition = this.GetMessageConditionDialog();
			symbolicMessageCondition.SymbolicMessageEvent = new SymbolicMessageEvent(symbolicMsgEvent);
			if (DialogResult.OK == symbolicMessageCondition.ShowDialog() && !symbolicMsgEvent.Equals(symbolicMessageCondition.SymbolicMessageEvent))
			{
				symbolicMsgEvent.Assign(symbolicMessageCondition.SymbolicMessageEvent);
				return true;
			}
			return false;
		}

		private bool EditSymbolicSignalCondition(ISymbolicSignalEvent symbolicSigEvent)
		{
			if (symbolicSigEvent == null)
			{
				return false;
			}
			using (SymbolicSignalCondition symbolicSignalCondition = new SymbolicSignalCondition(this.ModelValidator, this.ApplicationDatabaseManager, null))
			{
				if (symbolicSigEvent is SymbolicSignalEvent)
				{
					symbolicSignalCondition.SignalEvent = new SymbolicSignalEvent(symbolicSigEvent);
				}
				else if (symbolicSigEvent is CcpXcpSignalEvent)
				{
					symbolicSignalCondition.CcpXcpSignalConfiguration = CcpXcpManager.Instance().ConfigurationManagerService.CcpXcpSignalConfiguration;
					symbolicSignalCondition.SignalEvent = new CcpXcpSignalEvent(symbolicSigEvent);
				}
				else
				{
					if (!(symbolicSigEvent is DiagnosticSignalEvent))
					{
						bool result = false;
						return result;
					}
					symbolicSignalCondition.DiagnosticActionsConfiguration = this.DiagnosticActionsConfiguration;
					symbolicSignalCondition.DiagnosticsDatabaseConfiguration = this.DiagnosticDatabaseConfiguration;
					symbolicSignalCondition.SignalEvent = new DiagnosticSignalEvent(symbolicSigEvent);
				}
				if (DialogResult.OK == symbolicSignalCondition.ShowDialog() && !symbolicSigEvent.Equals(symbolicSignalCondition.SignalEvent))
				{
					symbolicSigEvent.Assign(symbolicSignalCondition.SignalEvent);
					bool result = true;
					return result;
				}
			}
			return false;
		}

		private bool EditMsgTimeoutCondition(MsgTimeoutEvent msgTimeoutEvent)
		{
			if (msgTimeoutEvent == null)
			{
				return false;
			}
			MsgTimeoutCondition msgTimeoutCondition = this.GetMsgTimeoutConditionDialog();
			msgTimeoutCondition.MsgTimeoutEvent = new MsgTimeoutEvent(msgTimeoutEvent);
			if (DialogResult.OK == msgTimeoutCondition.ShowDialog() && !msgTimeoutEvent.Equals(msgTimeoutCondition.MsgTimeoutEvent))
			{
				msgTimeoutEvent.Assign(msgTimeoutCondition.MsgTimeoutEvent);
				return true;
			}
			return false;
		}

		private bool EditKeyCondition(KeyEvent keyEvent)
		{
			if (keyEvent == null)
			{
				return false;
			}
			KeyCondition keyCondition = this.GetKeyConditionDialog();
			keyCondition.KeyEvent = new KeyEvent(keyEvent);
			if (DialogResult.OK == keyCondition.ShowDialog() && !keyEvent.Equals(keyCondition.KeyEvent))
			{
				keyEvent.Assign(keyCondition.KeyEvent);
				return true;
			}
			return false;
		}

		private bool EditDigitalInputCondition(DigitalInputEvent digInEvent)
		{
			if (digInEvent == null)
			{
				return false;
			}
			DigitalInputCondition digitalInputConditionDialog = this.GetDigitalInputConditionDialog();
			digitalInputConditionDialog.DigitalInputEvent = new DigitalInputEvent(digInEvent);
			if (DialogResult.OK == digitalInputConditionDialog.ShowDialog() && !digInEvent.Equals(digitalInputConditionDialog.DigitalInputEvent))
			{
				digInEvent.Assign(digitalInputConditionDialog.DigitalInputEvent);
				return true;
			}
			return false;
		}

		private bool EditAnalogInputCondition(AnalogInputEvent analogInputEvent)
		{
			if (analogInputEvent == null)
			{
				return false;
			}
			AnalogInputCondition analogInputCondition = this.GetAnalogInputConditionDialog();
			analogInputCondition.AnalogInputEvent = new AnalogInputEvent(analogInputEvent);
			if (DialogResult.OK == analogInputCondition.ShowDialog() && !analogInputEvent.Equals(analogInputCondition.AnalogInputEvent))
			{
				analogInputEvent.Assign(analogInputCondition.AnalogInputEvent);
				return true;
			}
			return false;
		}

		private bool EditVoCANRecordingEvent(VoCanRecordingEvent voCANRecordingEvent)
		{
			if (voCANRecordingEvent == null)
			{
				return false;
			}
			VoCanRecordingCondition voCANRecordingConditionDialog = this.GetVoCANRecordingConditionDialog();
			voCANRecordingConditionDialog.VoCanRecordingEvent = new VoCanRecordingEvent(voCANRecordingEvent);
			if (DialogResult.OK == voCANRecordingConditionDialog.ShowDialog() && !voCANRecordingEvent.Equals(voCANRecordingConditionDialog.VoCanRecordingEvent))
			{
				voCANRecordingEvent.Assign(voCANRecordingConditionDialog.VoCanRecordingEvent);
				return true;
			}
			return false;
		}

		private bool EditIgnitionCondition(IgnitionEvent ignitionEvent)
		{
			if (ignitionEvent == null)
			{
				return false;
			}
			IgnitionCondition ignitionCondition = this.GetIgnitionConditionDialog();
			ignitionCondition.IgnitionEvent = new IgnitionEvent(ignitionEvent);
			if (DialogResult.OK == ignitionCondition.ShowDialog() && !ignitionEvent.Equals(ignitionCondition.IgnitionEvent))
			{
				ignitionEvent.Assign(ignitionCondition.IgnitionEvent);
				return true;
			}
			return false;
		}

		public OnStartCondition GetOnStartConditionDialog()
		{
			if (this.onStartCondDialog == null)
			{
				this.onStartCondDialog = new OnStartCondition();
			}
			this.onStartCondDialog.ResetToDefaults();
			return this.onStartCondDialog;
		}

		public CyclicTimerCondition GetCyclicTimerConditionDialog()
		{
			if (this.cyclicTimerCondDialog == null)
			{
				this.cyclicTimerCondDialog = new CyclicTimerCondition();
			}
			this.cyclicTimerCondDialog.ResetToDefaults();
			return this.cyclicTimerCondDialog;
		}

		public CANIdCondition GetCANIdConditionDialog()
		{
			if (this.canIdConditionDialog == null)
			{
				this.canIdConditionDialog = new CANIdCondition(this.ModelValidator);
			}
			this.canIdConditionDialog.ResetToDefaults();
			return this.canIdConditionDialog;
		}

		public LINIdCondition GetLINIdConditionDialog()
		{
			if (this.linIdConditionDialog == null)
			{
				this.linIdConditionDialog = new LINIdCondition(this.ModelValidator);
			}
			this.linIdConditionDialog.ResetToDefaults();
			return this.linIdConditionDialog;
		}

		public FlexrayIdCondition GetFlexrayIdConditionDialog()
		{
			if (this.flexrayConditionDialog == null)
			{
				this.flexrayConditionDialog = new FlexrayIdCondition(this.ModelValidator);
			}
			this.flexrayConditionDialog.ResetToDefaults();
			return this.flexrayConditionDialog;
		}

		public CANDataCondition GetCANDataConditionDialog()
		{
			if (this.canDataConditionDialog == null)
			{
				this.canDataConditionDialog = new CANDataCondition(this.ModelValidator, null);
			}
			this.canDataConditionDialog.InitDefaultValues();
			return this.canDataConditionDialog;
		}

		public LINDataCondition GetLINDataConditionDialog()
		{
			if (this.linDataConditionDialog == null)
			{
				this.linDataConditionDialog = new LINDataCondition(this.ModelValidator, null);
			}
			this.linDataConditionDialog.InitDefaultValues();
			return this.linDataConditionDialog;
		}

		public SymbolicMessageCondition GetMessageConditionDialog()
		{
			if (this.messageConditionDialog == null)
			{
				this.messageConditionDialog = new SymbolicMessageCondition(this.ModelValidator, this.ApplicationDatabaseManager);
			}
			return this.messageConditionDialog;
		}

		public KeyCondition GetKeyConditionDialog()
		{
			if (this.keyConditionDialog == null)
			{
				this.keyConditionDialog = new KeyCondition(this.ModelValidator);
			}
			this.keyConditionDialog.ResetToDefaults();
			return this.keyConditionDialog;
		}

		public DigitalInputCondition GetDigitalInputConditionDialog()
		{
			if (this.digInConditionDialog == null)
			{
				this.digInConditionDialog = new DigitalInputCondition(this.ModelValidator);
			}
			this.digInConditionDialog.ResetToDefaults();
			return this.digInConditionDialog;
		}

		public AnalogInputCondition GetAnalogInputConditionDialog()
		{
			if (this.analogInputConditionDialog == null)
			{
				this.analogInputConditionDialog = new AnalogInputCondition(this.ModelValidator);
			}
			this.analogInputConditionDialog.ResetToDefaults();
			return this.analogInputConditionDialog;
		}

		public CANBusStatisticsCondition GetCANBusStatisticsDialog()
		{
			if (this.canBusStatisticsDialog == null)
			{
				this.canBusStatisticsDialog = new CANBusStatisticsCondition(this.ModelValidator);
			}
			this.canBusStatisticsDialog.ResetToDefaults();
			return this.canBusStatisticsDialog;
		}

		public IgnitionCondition GetIgnitionConditionDialog()
		{
			if (this.ignitionConditionDialog == null)
			{
				this.ignitionConditionDialog = new IgnitionCondition();
			}
			this.ignitionConditionDialog.ResetToDefaults();
			return this.ignitionConditionDialog;
		}

		public VoCanRecordingCondition GetVoCANRecordingConditionDialog()
		{
			if (this.voCANRecordingDialog == null)
			{
				this.voCANRecordingDialog = new VoCanRecordingCondition(this.ModelValidator);
			}
			this.voCANRecordingDialog.ResetToDefaults();
			return this.voCANRecordingDialog;
		}

		public MsgTimeoutCondition GetMsgTimeoutConditionDialog()
		{
			if (this.msgTimeoutConditionDialog == null)
			{
				this.msgTimeoutConditionDialog = new MsgTimeoutCondition(this.ModelValidator, this.ApplicationDatabaseManager);
			}
			return this.msgTimeoutConditionDialog;
		}

		public ConfigureSendMessage GetConfigureSendMessageDialog()
		{
			if (this.configureSendMsgDialog == null)
			{
				this.configureSendMsgDialog = new ConfigureSendMessage(this.ModelValidator, this.ApplicationDatabaseManager);
			}
			return this.configureSendMsgDialog;
		}

		private void InitializeEventTypeImageList()
		{
			this.eventTypeImageCollection = new ImageCollection();
			this.eventTypeNameToImageIndex = new Dictionary<string, int>();
			this.eventTypeImageCollection.AddImage(Resources.ImageStart);
			this.eventTypeNameToImageIndex.Add(Vocabulary.TriggerTypeNameColOnStart, 0);
			this.eventTypeImageCollection.AddImage(Resources.ImageIDMessageCAN);
			this.eventTypeNameToImageIndex.Add(Vocabulary.TriggerTypeNameColCANId, 1);
			this.eventTypeImageCollection.AddImage(Resources.ImageIDMessageLIN);
			this.eventTypeNameToImageIndex.Add(Vocabulary.TriggerTypeNameColLINId, 2);
			this.eventTypeImageCollection.AddImage(Resources.ImageIDMessageFlexray);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColFlexray, 3);
			this.eventTypeImageCollection.AddImage(Resources.ImageSymbMessageCAN);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColSymbolicCAN, 4);
			this.eventTypeImageCollection.AddImage(Resources.ImageSymbMessageLIN);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColSymbolicLIN, 5);
			this.eventTypeImageCollection.AddImage(Resources.ImageSymbMessageFlexray);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColSymbolicFlexray, 6);
			this.eventTypeImageCollection.AddImage(Resources.ImageSymbSignalCAN);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColSymbolicSigCAN, 7);
			this.eventTypeImageCollection.AddImage(Resources.ImageSymbSignalLIN);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColSymbolicSigLIN, 8);
			this.eventTypeImageCollection.AddImage(Resources.ImageSymbSignalFlexRay);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray, 9);
			this.eventTypeImageCollection.AddImage(Resources.ImageDataTriggerCAN);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColCANData, 10);
			this.eventTypeImageCollection.AddImage(Resources.ImageDataTriggerLIN);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColLINData, 11);
			this.eventTypeImageCollection.AddImage(Resources.ImageCycMsgTimeoutCAN);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColCANMsgTimeout, 12);
			this.eventTypeImageCollection.AddImage(Resources.ImageCycMsgTimeoutFlexray);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColLINMsgTimeout, 13);
			this.eventTypeImageCollection.AddImage(Resources.ImageCANBusStatistics);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColCANBusStatistics, 14);
			this.eventTypeImageCollection.AddImage(Resources.ImageAnalogInputSignal);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColAnalogInput, 15);
			this.eventTypeImageCollection.AddImage(Resources.ImageDigitalInput);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColDigitalInput, 16);
			this.eventTypeImageCollection.AddImage(Resources.ImageKey);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColKey, 17);
			this.eventTypeImageCollection.AddImage(Resources.ImageOnIgnition);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColIgnition, 18);
			this.eventTypeImageCollection.AddImage(Resources.ImageVoCanRecoding);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColVoCanRecording, 19);
			this.eventTypeImageCollection.AddImage(Resources.ImageSymbSignalCcpXcp);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColCcpXcpSignal, 20);
			this.eventTypeImageCollection.AddImage(Resources.ImageDiagSignal);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColDiagnosticSignal, 21);
		}

		public void InitEventTypeNamesInPlaceEditor()
		{
			this.repositoryItemImageComboBoxStartEvent.Items.Clear();
			this.repositoryItemImageComboBoxStartEvent.Items.Add(new ImageComboBoxItem(Vocabulary.TriggerTypeNameColOnStart, this.eventTypeNameToImageIndex[Vocabulary.TriggerTypeNameColOnStart]));
			foreach (string current in GUIUtil.EventTypeNamesInCol)
			{
				if ((!(current == Resources_Trigger.TriggerTypeNameColKey) || this.ModelValidator.LoggerSpecifics.IO.NumberOfKeys != 0u || this.ModelValidator.LoggerSpecifics.IO.NumberOfPanelKeys != 0u) && (this.ModelValidator.LoggerSpecifics.LIN.NumberOfChannels != 0u || (!(current == Vocabulary.TriggerTypeNameColLINId) && !(current == Resources_Trigger.TriggerTypeNameColLINData) && !(current == Resources_Trigger.TriggerTypeNameColLINMsgTimeout) && !(current == Resources_Trigger.TriggerTypeNameColSymbolicLIN) && !(current == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN))) && (this.ModelValidator.LoggerSpecifics.IO.NumberOfDigitalInputs != 0u || (!(current == Resources_Trigger.TriggerTypeNameColDigitalInput) && !(current == Resources_Trigger.TriggerTypeNameColIgnition))) && (this.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels != 0u || (!(current == Resources_Trigger.TriggerTypeNameColSymbolicFlexray) && !(current == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray) && !(current == Resources_Trigger.TriggerTypeNameColFlexray))) && (this.ModelValidator.LoggerSpecifics.Recording.IsVoCANSupported || !(current == Resources_Trigger.TriggerTypeNameColVoCanRecording)))
				{
					this.repositoryItemImageComboBoxStartEvent.Items.Add(new ImageComboBoxItem(current, this.eventTypeNameToImageIndex[current]));
				}
			}
		}

		public bool Serialize(SendMessagePage sendMessagePage)
		{
			if (sendMessagePage == null)
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.gridViewSendMessage.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				sendMessagePage.SendMessageGridLayout = Convert.ToBase64String(array, 0, array.Length);
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
			}
			return result;
		}

		public bool DeSerialize(SendMessagePage sendMessagePage)
		{
			if (sendMessagePage == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(sendMessagePage.SendMessageGridLayout))
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(sendMessagePage.SendMessageGridLayout);
				memoryStream = new MemoryStream(buffer);
				this.gridViewSendMessage.RestoreLayoutFromStream(memoryStream);
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
			}
			return result;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SendMessageGrid));
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.gridControlSendMessage = new GridControl();
			this.sendMessageBindingSource = new BindingSource(this.components);
			this.gridViewSendMessage = new GridView();
			this.colActive = new GridColumn();
			this.repositoryItemCheckEditIsActive = new RepositoryItemCheckEdit();
			this.colStartEventType = new GridColumn();
			this.repositoryItemImageComboBoxStartEvent = new RepositoryItemImageComboBox();
			this.colStartCondition = new GridColumn();
			this.repositoryItemButtonEditStartCondition = new RepositoryItemButtonEdit();
			this.colSendMessage = new GridColumn();
			this.repositoryItemButtonEditSendMessage = new RepositoryItemButtonEdit();
			this.colComment = new GridColumn();
			this.repositoryItemMemoExEditComment = new RepositoryItemMemoExEdit();
			this.repositoryItemComboBoxKeyCondition = new RepositoryItemComboBox();
			this.repositoryItemComboBoxIgnitionCondition = new RepositoryItemComboBox();
			this.toolTipController = new XtraToolTipController(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.gridControlSendMessage).BeginInit();
			((ISupportInitialize)this.sendMessageBindingSource).BeginInit();
			((ISupportInitialize)this.gridViewSendMessage).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsActive).BeginInit();
			((ISupportInitialize)this.repositoryItemImageComboBoxStartEvent).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEditStartCondition).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEditSendMessage).BeginInit();
			((ISupportInitialize)this.repositoryItemMemoExEditComment).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBoxKeyCondition).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBoxIgnitionCondition).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.gridControlSendMessage, "gridControlSendMessage");
			this.gridControlSendMessage.DataSource = this.sendMessageBindingSource;
			this.gridControlSendMessage.MainView = this.gridViewSendMessage;
			this.gridControlSendMessage.Name = "gridControlSendMessage";
			this.gridControlSendMessage.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemCheckEditIsActive,
				this.repositoryItemButtonEditStartCondition,
				this.repositoryItemButtonEditSendMessage,
				this.repositoryItemMemoExEditComment,
				this.repositoryItemImageComboBoxStartEvent,
				this.repositoryItemComboBoxKeyCondition,
				this.repositoryItemComboBoxIgnitionCondition
			});
			this.gridControlSendMessage.ToolTipController = this.toolTipController;
			this.gridControlSendMessage.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewSendMessage
			});
			this.sendMessageBindingSource.DataSource = typeof(ActionSendMessage);
			this.gridViewSendMessage.Columns.AddRange(new GridColumn[]
			{
				this.colActive,
				this.colStartEventType,
				this.colStartCondition,
				this.colSendMessage,
				this.colComment
			});
			this.gridViewSendMessage.GridControl = this.gridControlSendMessage;
			this.gridViewSendMessage.Name = "gridViewSendMessage";
			this.gridViewSendMessage.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;
			this.gridViewSendMessage.OptionsCustomization.AllowFilter = false;
			this.gridViewSendMessage.OptionsCustomization.AllowGroup = false;
			this.gridViewSendMessage.OptionsCustomization.AllowQuickHideColumns = false;
			this.gridViewSendMessage.OptionsView.ShowGroupPanel = false;
			this.gridViewSendMessage.OptionsView.ShowIndicator = false;
			this.gridViewSendMessage.PaintStyleName = "WindowsXP";
			this.gridViewSendMessage.RowHeight = 20;
			this.gridViewSendMessage.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewSendMessage_CustomDrawCell);
			this.gridViewSendMessage.RowStyle += new RowStyleEventHandler(this.gridViewSendMessage_RowStyle);
			this.gridViewSendMessage.CustomRowCellEdit += new CustomRowCellEditEventHandler(this.gridViewSendMessage_CustomRowCellEdit);
			this.gridViewSendMessage.LeftCoordChanged += new EventHandler(this.gridViewSendMessage_LeftCoordChanged);
			this.gridViewSendMessage.TopRowChanged += new EventHandler(this.gridViewSendMessage_TopRowChanged);
			this.gridViewSendMessage.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewSendMessage_PopupMenuShowing);
			this.gridViewSendMessage.ShownEditor += new EventHandler(this.gridViewSendMessage_ShownEditor);
			this.gridViewSendMessage.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewSendMessage_CustomUnboundColumnData);
			this.gridViewSendMessage.Click += new EventHandler(this.gridViewSendMessage_Click);
			componentResourceManager.ApplyResources(this.colActive, "colActive");
			this.colActive.ColumnEdit = this.repositoryItemCheckEditIsActive;
			this.colActive.FieldName = "anyBoolean1";
			this.colActive.Name = "colActive";
			this.colActive.UnboundType = UnboundColumnType.Boolean;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditIsActive, "repositoryItemCheckEditIsActive");
			this.repositoryItemCheckEditIsActive.Name = "repositoryItemCheckEditIsActive";
			this.repositoryItemCheckEditIsActive.CheckedChanged += new EventHandler(this.repositoryItemCheckEditIsActive_CheckedChanged);
			componentResourceManager.ApplyResources(this.colStartEventType, "colStartEventType");
			this.colStartEventType.ColumnEdit = this.repositoryItemImageComboBoxStartEvent;
			this.colStartEventType.FieldName = "colStartEventType";
			this.colStartEventType.Name = "colStartEventType";
			this.colStartEventType.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemImageComboBoxStartEvent, "repositoryItemImageComboBoxStartEvent");
			this.repositoryItemImageComboBoxStartEvent.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemImageComboBoxStartEvent.Buttons"))
			});
			this.repositoryItemImageComboBoxStartEvent.Name = "repositoryItemImageComboBoxStartEvent";
			this.repositoryItemImageComboBoxStartEvent.SelectedIndexChanged += new EventHandler(this.repositoryItemImageComboBoxStartEvent_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.colStartCondition, "colStartCondition");
			this.colStartCondition.ColumnEdit = this.repositoryItemButtonEditStartCondition;
			this.colStartCondition.FieldName = "colStartConditionField";
			this.colStartCondition.Name = "colStartCondition";
			this.colStartCondition.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonEditStartCondition, "repositoryItemButtonEditStartCondition");
			this.repositoryItemButtonEditStartCondition.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEditStartCondition.Name = "repositoryItemButtonEditStartCondition";
			this.repositoryItemButtonEditStartCondition.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEditStartCondition.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEditStartCondition_ButtonClick);
			componentResourceManager.ApplyResources(this.colSendMessage, "colSendMessage");
			this.colSendMessage.ColumnEdit = this.repositoryItemButtonEditSendMessage;
			this.colSendMessage.FieldName = "colSendMessageField";
			this.colSendMessage.Name = "colSendMessage";
			this.colSendMessage.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonEditSendMessage, "repositoryItemButtonEditSendMessage");
			this.repositoryItemButtonEditSendMessage.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEditSendMessage.Name = "repositoryItemButtonEditSendMessage";
			this.repositoryItemButtonEditSendMessage.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEditSendMessage.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEditSendMessage_ButtonClick);
			componentResourceManager.ApplyResources(this.colComment, "colComment");
			this.colComment.ColumnEdit = this.repositoryItemMemoExEditComment;
			this.colComment.FieldName = "anyString1";
			this.colComment.Name = "colComment";
			this.colComment.UnboundType = UnboundColumnType.String;
			this.repositoryItemMemoExEditComment.AcceptsReturn = false;
			componentResourceManager.ApplyResources(this.repositoryItemMemoExEditComment, "repositoryItemMemoExEditComment");
			this.repositoryItemMemoExEditComment.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemMemoExEditComment.Buttons"))
			});
			this.repositoryItemMemoExEditComment.MaxLength = 8190;
			this.repositoryItemMemoExEditComment.Name = "repositoryItemMemoExEditComment";
			this.repositoryItemMemoExEditComment.ShowIcon = false;
			this.repositoryItemMemoExEditComment.EditValueChanged += new EventHandler(this.repositoryItemMemoExEditComment_EditValueChanged);
			componentResourceManager.ApplyResources(this.repositoryItemComboBoxKeyCondition, "repositoryItemComboBoxKeyCondition");
			this.repositoryItemComboBoxKeyCondition.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBoxKeyCondition.Buttons"))
			});
			this.repositoryItemComboBoxKeyCondition.Name = "repositoryItemComboBoxKeyCondition";
			this.repositoryItemComboBoxKeyCondition.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBoxKeyCondition.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBoxKeyCondition_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.repositoryItemComboBoxIgnitionCondition, "repositoryItemComboBoxIgnitionCondition");
			this.repositoryItemComboBoxIgnitionCondition.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBoxIgnitionCondition.Buttons"))
			});
			this.repositoryItemComboBoxIgnitionCondition.Name = "repositoryItemComboBoxIgnitionCondition";
			this.repositoryItemComboBoxIgnitionCondition.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBoxIgnitionCondition.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBoxIgnitionCondition_SelectedIndexChanged);
			this.toolTipController.Appearance.BackColor = (Color)componentResourceManager.GetObject("toolTipController.Appearance.BackColor");
			this.toolTipController.Appearance.ForeColor = (Color)componentResourceManager.GetObject("toolTipController.Appearance.ForeColor");
			this.toolTipController.Appearance.Options.UseBackColor = true;
			this.toolTipController.Appearance.Options.UseForeColor = true;
			this.toolTipController.Appearance.Options.UseTextOptions = true;
			this.toolTipController.Appearance.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.toolTipController.AppearanceTitle.BackColor = (Color)componentResourceManager.GetObject("toolTipController.AppearanceTitle.BackColor");
			this.toolTipController.AppearanceTitle.ForeColor = (Color)componentResourceManager.GetObject("toolTipController.AppearanceTitle.ForeColor");
			this.toolTipController.AppearanceTitle.Options.UseBackColor = true;
			this.toolTipController.AppearanceTitle.Options.UseForeColor = true;
			this.toolTipController.AppearanceTitle.Options.UseTextOptions = true;
			this.toolTipController.AppearanceTitle.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.toolTipController.MaxWidth = 500;
			this.toolTipController.ShowPrefix = true;
			this.toolTipController.UseNativeLookAndFeel = true;
			this.toolTipController.GetActiveObjectInfo += new ToolTipControllerGetActiveObjectInfoEventHandler(this.toolTipController_GetActiveObjectInfo);
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.gridControlSendMessage);
			base.Name = "SendMessageGrid";
			componentResourceManager.ApplyResources(this, "$this");
			base.Resize += new EventHandler(this.SendMessageGrid_Resize);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.gridControlSendMessage).EndInit();
			((ISupportInitialize)this.sendMessageBindingSource).EndInit();
			((ISupportInitialize)this.gridViewSendMessage).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsActive).EndInit();
			((ISupportInitialize)this.repositoryItemImageComboBoxStartEvent).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEditStartCondition).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEditSendMessage).EndInit();
			((ISupportInitialize)this.repositoryItemMemoExEditComment).EndInit();
			((ISupportInitialize)this.repositoryItemComboBoxKeyCondition).EndInit();
			((ISupportInitialize)this.repositoryItemComboBoxIgnitionCondition).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
