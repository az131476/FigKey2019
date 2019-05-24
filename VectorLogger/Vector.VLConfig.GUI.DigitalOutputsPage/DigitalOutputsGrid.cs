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

namespace Vector.VLConfig.GUI.DigitalOutputsPage
{
	internal class DigitalOutputsGrid : UserControl
	{
		private DigitalOutputsConfiguration digitalOutputsConfiguration;

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

		private Dictionary<string, int> eventTypeNameToImageIndex;

		private ImageCollection eventTypeImageCollection;

		private int oldSelectedSetEventIndex;

		private int oldSelectedResetEventIndex;

		private static readonly char[] _decDigits = new char[]
		{
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'0'
		};

		private IContainer components;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private GridControl gridControlDigitalOutputs;

		private GridView gridViewDigitalOutputs;

		private GridColumn colActive;

		private GridColumn colSetEventType;

		private GridColumn colSetCondition;

		private GridColumn colResetEventType;

		private GridColumn colResetCondition;

		private GridColumn colComment;

		private RepositoryItemCheckEdit repositoryItemCheckEditIsActive;

		private RepositoryItemImageComboBox repositoryItemImageComboBoxSetEvent;

		private RepositoryItemImageComboBox repositoryItemImageComboBoxResetEvent;

		private RepositoryItemButtonEdit repositoryItemButtonEditSetCondition;

		private RepositoryItemButtonEdit repositoryItemButtonEditResetCondition;

		private RepositoryItemMemoExEdit repositoryItemMemoExEditComment;

		private GridColumn colOutputNumber;

		private RepositoryItemComboBox repositoryItemComboBoxKeyCondition;

		private RepositoryItemComboBox repositoryItemComboBoxIgnitionCondition;

		private RepositoryItemTextEdit repositoryItemTextEditTimer;

		private XtraToolTipController toolTipController;

		private RepositoryItemTextEdit repositoryItemTextEditDummy;

		private RepositoryItemSpinEdit repositoryItemSpinEditTimer;

		public event EventHandler SelectionChanged;

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public DigitalOutputsConfiguration DigitalOutputsConfiguration
		{
			get
			{
				return this.digitalOutputsConfiguration;
			}
			set
			{
				this.digitalOutputsConfiguration = value;
				if (value != null)
				{
					if (this.ModelValidator.LoggerSpecifics.Type != this.loggerType)
					{
						this.InitEventTypeNamesSetEventInPlaceEditor();
						this.InitEventTypeNamesResetEventInPlaceEditor();
						this.loggerType = this.ModelValidator.LoggerSpecifics.Type;
					}
					int focusedRowHandle = this.gridViewDigitalOutputs.FocusedRowHandle;
					this.gridControlDigitalOutputs.DataSource = this.digitalOutputsConfiguration.Actions;
					if (focusedRowHandle >= 0 && focusedRowHandle < this.gridViewDigitalOutputs.RowCount)
					{
						this.gridViewDigitalOutputs.FocusedRowHandle = focusedRowHandle;
					}
					else
					{
						this.gridViewDigitalOutputs.FocusedRowHandle = -1;
					}
					this.ValidateInput(false);
				}
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
					this.gridViewDigitalOutputs.RefreshData();
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

		public DigitalOutputsGrid()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_ControlGridTree();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.InitializeEventTypeImageList();
			this.repositoryItemImageComboBoxSetEvent.SmallImages = this.eventTypeImageCollection;
			this.repositoryItemImageComboBoxResetEvent.SmallImages = this.eventTypeImageCollection;
			this.oldSelectedSetEventIndex = -1;
			this.oldSelectedResetEventIndex = -1;
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

		private void gridViewDigitalOutputs_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			ActionDigitalOutput action;
			if (!this.TryGetAction(e.ListSourceRowIndex, out action))
			{
				return;
			}
			if (e.Column == this.colActive)
			{
				this.UnboundColumnDataActionActive(action, e);
				return;
			}
			if (e.Column == this.colOutputNumber)
			{
				int num = e.ListSourceRowIndex + 1;
				if (this.ModelValidator.IsCommonDigitalInputOutputPinUsedAsInput(num))
				{
					e.Value = string.Format(Resources.DigOutUsedAsInput, num);
					return;
				}
				e.Value = string.Format("{0}", num);
				return;
			}
			else
			{
				if (e.Column == this.colSetEventType)
				{
					this.UnboundColumnDataSetEvent(action, e);
					return;
				}
				if (e.Column == this.colSetCondition)
				{
					this.UnboundColumnDataSetCondition(action, e);
					return;
				}
				if (e.Column == this.colResetEventType)
				{
					this.UnboundColumnDataResetEvent(action, e);
					return;
				}
				if (e.Column == this.colResetCondition)
				{
					this.UnboundColumnDataResetCondition(action, e);
					return;
				}
				if (e.Column == this.colComment)
				{
					this.UnboundColumnDataComment(action, e);
				}
				return;
			}
		}

		private void UnboundColumnDataSetEvent(ActionDigitalOutput action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = this.GetUnboundColumnDataEventString(action.Event);
			}
		}

		private void UnboundColumnDataResetEvent(ActionDigitalOutput action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (action.StopType == null)
				{
					e.Value = Resources.None;
					return;
				}
				if (action.StopType is StopOnTimer)
				{
					e.Value = Resources.Timer;
					return;
				}
				if (action.StopType is StopOnStartEvent)
				{
					e.Value = Resources.SETReset;
					return;
				}
				if (action.StopType is StopOnEvent)
				{
					e.Value = this.GetUnboundColumnDataEventString((action.StopType as StopOnEvent).Event);
				}
			}
		}

		private void UnboundColumnDataSetCondition(ActionDigitalOutput action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = this.GetUnboundColumnDataConditionString(action.Event);
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

		private void UnboundColumnDataResetCondition(ActionDigitalOutput action, CustomColumnDataEventArgs e)
		{
			if (!e.IsGetData)
			{
				if (e.IsSetData)
				{
					StopOnEvent stopOnEvent = action.StopType as StopOnEvent;
					if (stopOnEvent != null)
					{
						KeyEvent keyEvent = stopOnEvent.Event as KeyEvent;
						IgnitionEvent ignitionEvent = stopOnEvent.Event as IgnitionEvent;
						if (keyEvent != null)
						{
							bool flag = false;
							bool value = false;
							uint value2 = GUIUtil.MapStringToKeyNumber(e.Value.ToString(), out value);
							bool flag2;
							this.pageValidator.Grid.UpdateModel<uint>(value2, keyEvent.Number, out flag2);
							this.pageValidator.Grid.UpdateModel<bool>(value, keyEvent.IsOnPanel, out flag);
							if (flag2 || flag)
							{
								this.ValidateInput(true);
								return;
							}
						}
						else if (ignitionEvent != null)
						{
							bool flag3 = false;
							this.pageValidator.Grid.UpdateModel<bool>(Resources.IgnitionOn == e.Value.ToString(), ignitionEvent.IsOn, out flag3);
							if (flag3)
							{
								this.ValidateInput(true);
								return;
							}
						}
					}
					else if (action.StopType is StopOnTimer)
					{
						if (e.Value == null)
						{
							this.ValidateInput(false);
							return;
						}
						uint num;
						if (uint.TryParse(e.Value.ToString(), out num))
						{
							uint num2 = num % Constants.MinimumTimerValue_ms;
							if (num2 > 0u)
							{
								num = num / Constants.MinimumTimerValue_ms * Constants.MinimumTimerValue_ms;
								if (num2 > Constants.MinimumTimerValue_ms / 2u)
								{
									num += Constants.MinimumTimerValue_ms;
								}
							}
							bool flag4 = false;
							this.pageValidator.Grid.UpdateModel<uint>(num, (action.StopType as StopOnTimer).Duration, out flag4);
							if (flag4)
							{
								this.ValidateInput(true);
							}
						}
					}
				}
				return;
			}
			if (action.StopType is StopOnEvent)
			{
				e.Value = this.GetUnboundColumnDataConditionString((action.StopType as StopOnEvent).Event);
				return;
			}
			if (action.StopType is StopOnTimer)
			{
				e.Value = (action.StopType as StopOnTimer).Duration.Value;
				return;
			}
			e.Value = "";
		}

		private string GetUnboundColumnDataEventString(Event ev)
		{
			if (ev is OnStartEvent)
			{
				return Vocabulary.TriggerTypeNameColOnStart;
			}
			if (ev is CANIdEvent)
			{
				return Vocabulary.TriggerTypeNameColCANId;
			}
			if (ev is LINIdEvent)
			{
				return Vocabulary.TriggerTypeNameColLINId;
			}
			if (ev is FlexrayIdEvent)
			{
				return Resources_Trigger.TriggerTypeNameColFlexray;
			}
			if (ev is SymbolicMessageEvent)
			{
				SymbolicMessageEvent symbolicMessageEvent = ev as SymbolicMessageEvent;
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
			else if (ev is SymbolicSignalEvent)
			{
				SymbolicSignalEvent symbolicSignalEvent = ev as SymbolicSignalEvent;
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
				if (ev is CcpXcpSignalEvent)
				{
					return Resources_Trigger.TriggerTypeNameColCcpXcpSignal;
				}
				if (ev is DiagnosticSignalEvent)
				{
					return Resources_Trigger.TriggerTypeNameColDiagnosticSignal;
				}
				if (ev is CANDataEvent)
				{
					return Resources_Trigger.TriggerTypeNameColCANData;
				}
				if (ev is LINDataEvent)
				{
					return Resources_Trigger.TriggerTypeNameColLINData;
				}
				if (ev is MsgTimeoutEvent)
				{
					MsgTimeoutEvent msgTimeoutEvent = ev as MsgTimeoutEvent;
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
					if (ev is DigitalInputEvent)
					{
						return Resources_Trigger.TriggerTypeNameColDigitalInput;
					}
					if (ev is AnalogInputEvent)
					{
						return Resources_Trigger.TriggerTypeNameColAnalogInput;
					}
					if (ev is KeyEvent)
					{
						return Resources_Trigger.TriggerTypeNameColKey;
					}
					if (ev is CANBusStatisticsEvent)
					{
						return Resources_Trigger.TriggerTypeNameColCANBusStatistics;
					}
					if (ev is IgnitionEvent)
					{
						return Resources_Trigger.TriggerTypeNameColIgnition;
					}
					if (ev is VoCanRecordingEvent)
					{
						return Resources_Trigger.TriggerTypeNameColVoCanRecording;
					}
				}
			}
			return string.Empty;
		}

		private string GetUnboundColumnDataConditionString(Event ev)
		{
			if (ev is OnStartEvent)
			{
				double num = Convert.ToDouble((ev as OnStartEvent).Delay.Value) / 1000.0;
				return string.Format(Resources.DiagEvOnStartGroupRow, num.ToString("F1"));
			}
			if (ev is IdEvent)
			{
				IdEvent idEvent = ev as IdEvent;
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
			else if (ev is SymbolicMessageEvent)
			{
				SymbolicMessageEvent symbolicMessageEvent = ev as SymbolicMessageEvent;
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
			else if (ev is SymbolicSignalEvent)
			{
				SymbolicSignalEvent symbolicSignalEvent = ev as SymbolicSignalEvent;
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
				if (ev is CcpXcpSignalEvent)
				{
					CcpXcpSignalEvent symSigEvent = ev as CcpXcpSignalEvent;
					return GUIUtil.MapEventCondition2String(symSigEvent, this.ModelValidator.DatabaseServices);
				}
				if (ev is DiagnosticSignalEvent)
				{
					DiagnosticSignalEvent symSigEvent2 = ev as DiagnosticSignalEvent;
					return GUIUtil.MapEventCondition2String(symSigEvent2, this.ModelValidator.DatabaseServices);
				}
				if (ev is DigitalInputEvent)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapDigitalInputNumber2String((ev as DigitalInputEvent).DigitalInput.Value), GUIUtil.MapEventCondition2String(ev as DigitalInputEvent));
				}
				if (ev is AnalogInputEvent)
				{
					return string.Format("{0} {1}", GUIUtil.MapAnalogInputNumber2String((ev as AnalogInputEvent).InputNumber.Value), GUIUtil.MapEventCondition2String(ev as AnalogInputEvent));
				}
				if (ev is CANDataEvent)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String((ev as CANDataEvent).ChannelNumber.Value), GUIUtil.MapEventCondition2String(ev as CANDataEvent));
				}
				if (ev is LINDataEvent)
				{
					return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String((ev as LINDataEvent).ChannelNumber.Value, this.ModelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(ev as LINDataEvent));
				}
				if (ev is MsgTimeoutEvent)
				{
					MsgTimeoutEvent msgTimeoutEvent = ev as MsgTimeoutEvent;
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
					if (ev is KeyEvent)
					{
						return GUIUtil.MapKeyNumber2String((ev as KeyEvent).Number.Value, (ev as KeyEvent).IsOnPanel.Value);
					}
					if (ev is CANBusStatisticsEvent)
					{
						return string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String((ev as CANBusStatisticsEvent).ChannelNumber.Value), GUIUtil.MapEventCondition2String(ev as CANBusStatisticsEvent));
					}
					if (ev is IgnitionEvent)
					{
						return GUIUtil.MapEventCondition2String(ev as IgnitionEvent);
					}
				}
			}
			return string.Empty;
		}

		private void UnboundColumnDataActionActive(ActionDigitalOutput action, CustomColumnDataEventArgs e)
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

		private void UnboundColumnDataComment(ActionDigitalOutput action, CustomColumnDataEventArgs e)
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

		private void gridViewDigitalOutputs_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			if (e.Column == this.colSetEventType || e.Column == this.colResetEventType || e.Column == this.colSetCondition)
			{
				ActionDigitalOutput actionDigitalOutput;
				if (this.TryGetAction(this.gridViewDigitalOutputs.GetDataSourceRowIndex(e.RowHandle), out actionDigitalOutput))
				{
					if (e.Column == this.colSetEventType)
					{
						if (this.HasLocalModelErrorStopOnStartEventIncompatible(actionDigitalOutput))
						{
							GridUtil.DrawImageTextCell(e, this.errorProviderLocalModel.Icon.ToBitmap());
							return;
						}
						this.DrawEventImageTextCell(actionDigitalOutput.Event, e);
						return;
					}
					else if (e.Column == this.colSetCondition)
					{
						if (!this.HasEventLocalModelErrorStopOnStartEventIncompatible(actionDigitalOutput.Event))
						{
							IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(e.Column, this.gridViewDigitalOutputs.GetDataSourceRowIndex(e.RowHandle));
							this.customErrorProvider.Grid.DisplayError(gUIElement, e);
							return;
						}
					}
					else if (actionDigitalOutput.StopType is StopOnStartEvent)
					{
						if (this.HasLocalModelErrorStopOnStartEventIncompatible(actionDigitalOutput))
						{
							GridUtil.DrawImageTextCell(e, this.errorProviderLocalModel.Icon.ToBitmap());
							return;
						}
						GridUtil.DrawImageTextCell(e, Resources.ImageNotSET);
						return;
					}
					else
					{
						if (actionDigitalOutput.StopType is StopOnTimer)
						{
							GridUtil.DrawImageTextCell(e, Resources.ImageTimer);
							return;
						}
						if (actionDigitalOutput.StopType is StopOnEvent)
						{
							this.DrawEventImageTextCell((actionDigitalOutput.StopType as StopOnEvent).Event, e);
							return;
						}
						if (actionDigitalOutput.StopType == null)
						{
							GridUtil.DrawImageTextCell(e, Resources.ImageNone);
							return;
						}
					}
				}
			}
			else
			{
				IValidatedGUIElement gUIElement2 = this.guiElementManager.GetGUIElement(e.Column, this.gridViewDigitalOutputs.GetDataSourceRowIndex(e.RowHandle));
				this.customErrorProvider.Grid.DisplayError(gUIElement2, e);
			}
		}

		private void DrawEventImageTextCell(Event ev, RowCellCustomDrawEventArgs e)
		{
			if (ev is CANIdEvent)
			{
				GridUtil.DrawImageTextCell(e, Resources.ImageIDMessageCAN);
			}
			if (ev is FlexrayIdEvent)
			{
				GridUtil.DrawImageTextCell(e, Resources.ImageIDMessageFlexray);
			}
			if (ev is LINIdEvent)
			{
				GridUtil.DrawImageTextCell(e, Resources.ImageIDMessageLIN);
				return;
			}
			if (ev is SymbolicMessageEvent)
			{
				switch ((ev as SymbolicMessageEvent).BusType.Value)
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
			else if (ev is SymbolicSignalEvent)
			{
				switch ((ev as SymbolicSignalEvent).BusType.Value)
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
				if (ev is KeyEvent)
				{
					GridUtil.DrawImageTextCell(e, Resources.ImageKey);
					return;
				}
				if (ev is DigitalInputEvent)
				{
					GridUtil.DrawImageTextCell(e, Resources.ImageDigitalInput);
					return;
				}
				if (ev is AnalogInputEvent)
				{
					GridUtil.DrawImageTextCell(e, Resources.ImageAnalogInputSignal);
					return;
				}
				if (ev is CANBusStatisticsEvent)
				{
					GridUtil.DrawImageTextCell(e, Resources.ImageCANBusStatistics);
					return;
				}
				if (ev is CANDataEvent)
				{
					GridUtil.DrawImageTextCell(e, Resources.ImageDataTriggerCAN);
					return;
				}
				if (ev is LINDataEvent)
				{
					GridUtil.DrawImageTextCell(e, Resources.ImageDataTriggerLIN);
					return;
				}
				if (ev is IgnitionEvent)
				{
					GridUtil.DrawImageTextCell(e, Resources.ImageOnIgnition);
					return;
				}
				if (ev is VoCanRecordingEvent)
				{
					GridUtil.DrawImageTextCell(e, Resources.ImageVoCanRecoding);
					return;
				}
				if (ev is MsgTimeoutEvent)
				{
					switch ((ev as MsgTimeoutEvent).BusType.Value)
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
						break;
					}
				}
				return;
			}
		}

		private void gridViewDigitalOutputs_RowStyle(object sender, RowStyleEventArgs e)
		{
			ActionDigitalOutput actionDigitalOutput;
			if (this.TryGetAction(this.gridViewDigitalOutputs.GetDataSourceRowIndex(e.RowHandle), out actionDigitalOutput) && !actionDigitalOutput.IsActive.Value)
			{
				e.Appearance.ForeColor = SystemColors.GrayText;
			}
		}

		private void gridViewDigitalOutputs_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (e.MenuType == GridMenuType.Column)
			{
				string localizedString = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnGroupBox);
				string localizedString2 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuGroupPanelShow);
				string localizedString3 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnFilter);
				string localizedString4 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnFilterEditor);
				string localizedString5 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnGroup);
				string localizedString6 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnUnGroup);
				string localizedString7 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnSortAscending);
				string localizedString8 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnSortDescending);
				string localizedString9 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnRemoveColumn);
				string localizedString10 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnClearSorting);
				string localizedString11 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnColumnCustomization);
				for (int i = e.Menu.Items.Count - 1; i >= 0; i--)
				{
					string caption = e.Menu.Items[i].Caption;
					if (localizedString5 == caption || localizedString6 == caption || caption.Contains(localizedString) || localizedString2 == caption || localizedString3 == caption || localizedString4 == caption || localizedString7 == caption || localizedString8 == caption || localizedString9 == caption || localizedString10 == caption || localizedString11 == caption)
					{
						e.Menu.Items.RemoveAt(i);
					}
				}
			}
		}

		private void gridViewDigitalOutputs_TopRowChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridViewDigitalOutputs_LeftCoordChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void DigitalOutputsGrid_Resize(object sender, EventArgs e)
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
			flag &= this.ModelValidator.Validate(this.digitalOutputsConfiguration, isDataChanged, this.pageValidator);
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

		private bool HasLocalModelErrorStopOnStartEventIncompatible(ActionDigitalOutput action)
		{
			if (action.StopType is StopOnStartEvent)
			{
				string errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.LocalModelError, (action.StopType as StopOnStartEvent).IsStopOnResetOfStartEvent);
				if (errorText == Resources.ErrorSETEventCantBeUsedWithRESETEvent)
				{
					return true;
				}
			}
			return false;
		}

		private bool HasEventLocalModelErrorStopOnStartEventIncompatible(Event ev)
		{
			if (ev != null)
			{
				IEnumerable<IValidatedProperty> validatedPropertyListCondition = this.GetValidatedPropertyListCondition(ev);
				foreach (IValidatedProperty current in validatedPropertyListCondition)
				{
					string errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.LocalModelError, current);
					if (!string.IsNullOrEmpty(errorText) && errorText == Resources.ErrorSETEventCantBeUsedWithRESETEvent)
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		private void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
		}

		private void StoreMapping4VisibleCells()
		{
			PageValidatorGridUtil.StoreRowMappingForVisibleRows(this.gridViewDigitalOutputs, new PageValidatorGridUtil.StoreRowMappingHandler(this.OnPageValidatorGridUtilStoreRowMapping));
		}

		private void OnPageValidatorGridUtilStoreRowMapping(int dataSourceRowIdx)
		{
			if (dataSourceRowIdx < 0 || dataSourceRowIdx >= this.digitalOutputsConfiguration.Actions.Count)
			{
				return;
			}
			ActionDigitalOutput action = this.digitalOutputsConfiguration.Actions[dataSourceRowIdx];
			this.StoreMapping4VisibleColumns(action, dataSourceRowIdx);
		}

		private void StoreMapping4VisibleColumns(ActionDigitalOutput action, int dataSourceIdx)
		{
			if (PageValidatorGridUtil.IsColumnVisible(this.colActive, this.gridViewDigitalOutputs))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colActive, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(action.IsActive, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colSetCondition, this.gridViewDigitalOutputs))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colSetCondition, dataSourceIdx);
				IEnumerable<IValidatedProperty> validatedPropertyListCondition = this.GetValidatedPropertyListCondition(action.Event);
				foreach (IValidatedProperty current in validatedPropertyListCondition)
				{
					this.pageValidator.Grid.StoreMapping(current, gUIElement);
				}
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colResetCondition, this.gridViewDigitalOutputs))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colResetCondition, dataSourceIdx);
				if (action.StopType is StopOnEvent)
				{
					IEnumerable<IValidatedProperty> validatedPropertyListCondition = this.GetValidatedPropertyListCondition((action.StopType as StopOnEvent).Event);
					using (IEnumerator<IValidatedProperty> enumerator2 = validatedPropertyListCondition.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							IValidatedProperty current2 = enumerator2.Current;
							this.pageValidator.Grid.StoreMapping(current2, gUIElement);
						}
						goto IL_17C;
					}
				}
				if (action.StopType is StopOnTimer)
				{
					this.pageValidator.Grid.StoreMapping((action.StopType as StopOnTimer).Duration, gUIElement);
				}
			}
			IL_17C:
			if (PageValidatorGridUtil.IsColumnVisible(this.colComment, this.gridViewDigitalOutputs))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colComment, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(action.Comment, gUIElement);
			}
		}

		private IEnumerable<IValidatedProperty> GetValidatedPropertyListCondition(Event ev)
		{
			List<IValidatedProperty> list = new List<IValidatedProperty>();
			if (ev is IdEvent)
			{
				list.Add((ev as IdEvent).ChannelNumber);
				list.Add((ev as IdEvent).IdRelation);
				list.Add((ev as IdEvent).LowId);
				list.Add((ev as IdEvent).HighId);
				if (ev is CANIdEvent)
				{
					list.Add((ev as CANIdEvent).IsExtendedId);
				}
			}
			else if (ev is SymbolicMessageEvent)
			{
				list.Add((ev as SymbolicMessageEvent).ChannelNumber);
				list.Add((ev as SymbolicMessageEvent).DatabasePath);
				list.Add((ev as SymbolicMessageEvent).NetworkName);
				list.Add((ev as SymbolicMessageEvent).MessageName);
				list.Add((ev as SymbolicMessageEvent).BusType);
			}
			else if (ev is ISymbolicSignalEvent)
			{
				list.Add((ev as ISymbolicSignalEvent).LowValue);
				list.Add((ev as ISymbolicSignalEvent).HighValue);
				list.Add((ev as ISymbolicSignalEvent).SignalName);
				list.Add((ev as ISymbolicSignalEvent).Relation);
				if (ev is SymbolicSignalEvent)
				{
					list.Add((ev as ISymbolicSignalEvent).ChannelNumber);
					list.Add((ev as ISymbolicSignalEvent).DatabasePath);
					list.Add((ev as ISymbolicSignalEvent).NetworkName);
					list.Add((ev as ISymbolicSignalEvent).MessageName);
				}
			}
			else if (ev is DigitalInputEvent)
			{
				list.Add((ev as DigitalInputEvent).Edge);
				list.Add((ev as DigitalInputEvent).DigitalInput);
				list.Add((ev as DigitalInputEvent).DebounceTime);
				list.Add((ev as DigitalInputEvent).IsDebounceActive);
			}
			else if (ev is AnalogInputEvent)
			{
				list.Add((ev as AnalogInputEvent).InputNumber);
				list.Add((ev as AnalogInputEvent).LowValue);
				list.Add((ev as AnalogInputEvent).HighValue);
				list.Add((ev as AnalogInputEvent).Relation);
				list.Add((ev as AnalogInputEvent).Tolerance);
			}
			else if (ev is CANDataEvent)
			{
				CANDataEvent cANDataEvent = ev as CANDataEvent;
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
			else if (ev is LINDataEvent)
			{
				LINDataEvent lINDataEvent = ev as LINDataEvent;
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
			else if (ev is MsgTimeoutEvent)
			{
				list.Add((ev as MsgTimeoutEvent).DatabasePath);
				list.Add((ev as MsgTimeoutEvent).DatabaseName);
				list.Add((ev as MsgTimeoutEvent).MessageName);
				list.Add((ev as MsgTimeoutEvent).NetworkName);
				list.Add((ev as MsgTimeoutEvent).ChannelNumber);
				list.Add((ev as MsgTimeoutEvent).BusType);
				list.Add((ev as MsgTimeoutEvent).IsSymbolic);
				list.Add((ev as MsgTimeoutEvent).ID);
				list.Add((ev as MsgTimeoutEvent).IsExtendedId);
				list.Add((ev as MsgTimeoutEvent).IsCycletimeFromDatabase);
				list.Add((ev as MsgTimeoutEvent).UserDefinedCycleTime);
				list.Add((ev as MsgTimeoutEvent).MaxDelay);
			}
			else if (ev is KeyEvent)
			{
				list.Add((ev as KeyEvent).Number);
				list.Add((ev as KeyEvent).IsOnPanel);
			}
			else if (ev is CANBusStatisticsEvent)
			{
				list.Add((ev as CANBusStatisticsEvent).ChannelNumber);
				list.Add((ev as CANBusStatisticsEvent).IsBusloadEnabled);
				list.Add((ev as CANBusStatisticsEvent).BusloadRelation);
				list.Add((ev as CANBusStatisticsEvent).BusloadLow);
				list.Add((ev as CANBusStatisticsEvent).BusloadHigh);
				list.Add((ev as CANBusStatisticsEvent).IsErrorFramesEnabled);
				list.Add((ev as CANBusStatisticsEvent).ErrorFramesRelation);
				list.Add((ev as CANBusStatisticsEvent).ErrorFramesLow);
			}
			else if (ev is IgnitionEvent)
			{
				list.Add((ev as IgnitionEvent).IsOn);
			}
			else if (ev is VoCanRecordingEvent)
			{
				list.Add((ev as VoCanRecordingEvent).Duration_s);
				list.Add((ev as VoCanRecordingEvent).IsBeepOnEndOn);
			}
			return list;
		}

		public bool TryGetSelectedAction(out ActionDigitalOutput action)
		{
			int num;
			return this.TryGetSelectedAction(out action, out num);
		}

		private bool TryGetSelectedAction(out ActionDigitalOutput action, out int idx)
		{
			action = null;
			idx = this.gridViewDigitalOutputs.GetFocusedDataSourceRowIndex();
			if (idx < 0 || idx > this.digitalOutputsConfiguration.Actions.Count - 1)
			{
				return false;
			}
			action = this.digitalOutputsConfiguration.Actions[idx];
			return null != action;
		}

		private bool TryGetAction(int listSourceRowIndex, out ActionDigitalOutput action)
		{
			action = null;
			if (this.digitalOutputsConfiguration == null)
			{
				return false;
			}
			if (listSourceRowIndex < 0 || listSourceRowIndex > this.digitalOutputsConfiguration.Actions.Count - 1)
			{
				return false;
			}
			action = this.digitalOutputsConfiguration.Actions[listSourceRowIndex];
			return null != action;
		}

		public void SelectRowOfAction(ActionDigitalOutput action)
		{
			for (int i = 0; i < this.gridViewDigitalOutputs.RowCount; i++)
			{
				IList<ActionDigitalOutput> list = this.gridViewDigitalOutputs.DataSource as IList<ActionDigitalOutput>;
				if (list != null)
				{
					ActionDigitalOutput actionDigitalOutput = list[this.gridViewDigitalOutputs.GetDataSourceRowIndex(i)];
					if (actionDigitalOutput == action)
					{
						this.gridViewDigitalOutputs.FocusedRowHandle = i;
						return;
					}
				}
			}
		}

		private void gridViewDigitalOutputs_Click(object sender, EventArgs e)
		{
			GridHitInfo gridHitInfo = this.gridViewDigitalOutputs.CalcHitInfo(this.gridControlDigitalOutputs.PointToClient(Control.MousePosition));
			ActionDigitalOutput actionDigitalOutput;
			if (gridHitInfo.InRowCell && gridHitInfo.Column == this.colActive && this.TryGetAction(this.gridViewDigitalOutputs.GetDataSourceRowIndex(gridHitInfo.RowHandle), out actionDigitalOutput))
			{
				actionDigitalOutput.IsActive.Value = !actionDigitalOutput.IsActive.Value;
			}
		}

		private void gridViewDigitalOutputs_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			ActionDigitalOutput actionDigitalOutput2;
			if (e.Column == this.colSetCondition)
			{
				ActionDigitalOutput actionDigitalOutput;
				if (this.TryGetAction(e.RowHandle, out actionDigitalOutput))
				{
					if (actionDigitalOutput.Event is KeyEvent)
					{
						e.RepositoryItem = this.repositoryItemComboBoxKeyCondition;
						return;
					}
					if (actionDigitalOutput.Event is IgnitionEvent)
					{
						e.RepositoryItem = this.repositoryItemComboBoxIgnitionCondition;
						return;
					}
					e.RepositoryItem = this.repositoryItemButtonEditSetCondition;
					return;
				}
			}
			else if (e.Column == this.colResetCondition && this.TryGetAction(e.RowHandle, out actionDigitalOutput2))
			{
				if (actionDigitalOutput2.StopType is StopOnEvent)
				{
					StopOnEvent stopOnEvent = actionDigitalOutput2.StopType as StopOnEvent;
					if (stopOnEvent.Event is KeyEvent)
					{
						e.RepositoryItem = this.repositoryItemComboBoxKeyCondition;
						return;
					}
					if (stopOnEvent.Event is IgnitionEvent)
					{
						e.RepositoryItem = this.repositoryItemComboBoxIgnitionCondition;
						return;
					}
					e.RepositoryItem = this.repositoryItemButtonEditResetCondition;
					return;
				}
				else
				{
					if (actionDigitalOutput2.StopType is StopOnTimer)
					{
						e.RepositoryItem = this.repositoryItemSpinEditTimer;
						return;
					}
					e.RepositoryItem = this.repositoryItemTextEditDummy;
				}
			}
		}

		private void repositoryItemImageComboBoxSetEvent_SelectedIndexChanged(object sender, EventArgs e)
		{
			ImageComboBoxEdit imageComboBoxEdit = sender as ImageComboBoxEdit;
			if (imageComboBoxEdit != null && imageComboBoxEdit.SelectedIndex != this.oldSelectedSetEventIndex)
			{
				Event @event = this.CreateAndEditEvent(imageComboBoxEdit.SelectedItem.ToString());
				if (@event != null)
				{
					ActionDigitalOutput actionDigitalOutput;
					if (!this.TryGetSelectedAction(out actionDigitalOutput))
					{
						return;
					}
					actionDigitalOutput.Event = @event;
					actionDigitalOutput.IsActive.Value = true;
					this.gridViewDigitalOutputs.RefreshData();
					this.ValidateInput(true);
				}
				else
				{
					imageComboBoxEdit.SelectedIndex = this.oldSelectedSetEventIndex;
					this.gridViewDigitalOutputs.RefreshData();
				}
			}
			this.oldSelectedSetEventIndex = -1;
			this.gridViewDigitalOutputs.PostEditor();
		}

		private void repositoryItemImageComboBoxResetEvent_SelectedIndexChanged(object sender, EventArgs e)
		{
			ImageComboBoxEdit imageComboBoxEdit = sender as ImageComboBoxEdit;
			if (imageComboBoxEdit != null && imageComboBoxEdit.SelectedIndex != this.oldSelectedResetEventIndex)
			{
				ActionDigitalOutput actionDigitalOutput;
				if (!this.TryGetSelectedAction(out actionDigitalOutput))
				{
					return;
				}
				string text = imageComboBoxEdit.SelectedItem.ToString();
				if (text == Resources.Timer)
				{
					actionDigitalOutput.StopType = new StopOnTimer(Constants.MinimumTimerValue_ms);
					actionDigitalOutput.IsActive.Value = true;
					this.gridViewDigitalOutputs.RefreshData();
					this.ValidateInput(true);
				}
				else if (text == Resources.SETReset)
				{
					actionDigitalOutput.StopType = new StopOnStartEvent();
					actionDigitalOutput.IsActive.Value = true;
					this.gridViewDigitalOutputs.RefreshData();
					this.ValidateInput(true);
				}
				else if (text == Resources.None)
				{
					actionDigitalOutput.StopType = null;
					actionDigitalOutput.IsActive.Value = true;
					this.gridViewDigitalOutputs.RefreshData();
					this.ValidateInput(true);
				}
				else
				{
					Event @event = this.CreateAndEditEvent(text);
					if (@event != null)
					{
						actionDigitalOutput.StopType = new StopOnEvent(@event);
						actionDigitalOutput.IsActive.Value = true;
						this.gridViewDigitalOutputs.RefreshData();
						this.ValidateInput(true);
					}
					else
					{
						imageComboBoxEdit.SelectedIndex = this.oldSelectedResetEventIndex;
						this.gridViewDigitalOutputs.RefreshData();
					}
				}
			}
			this.oldSelectedResetEventIndex = -1;
			this.gridViewDigitalOutputs.PostEditor();
		}

		private void repositoryItemButtonEditSetCondition_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			int arg_0B_0 = this.gridViewDigitalOutputs.FocusedRowHandle;
			ActionDigitalOutput actionDigitalOutput;
			if (!this.TryGetSelectedAction(out actionDigitalOutput))
			{
				return;
			}
			if (this.EditEventCondition(actionDigitalOutput.Event))
			{
				actionDigitalOutput.IsActive.Value = true;
				this.gridViewDigitalOutputs.RefreshData();
				this.ValidateInput(true);
			}
		}

		private void repositoryItemButtonEditResetCondition_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			int arg_0B_0 = this.gridViewDigitalOutputs.FocusedRowHandle;
			ActionDigitalOutput actionDigitalOutput;
			if (!this.TryGetSelectedAction(out actionDigitalOutput))
			{
				return;
			}
			if (actionDigitalOutput.StopType is StopOnEvent && this.EditEventCondition((actionDigitalOutput.StopType as StopOnEvent).Event))
			{
				actionDigitalOutput.IsActive.Value = true;
				this.gridViewDigitalOutputs.RefreshData();
				this.ValidateInput(true);
			}
		}

		private void gridViewDigitalOutputs_ShowingEditor(object sender, CancelEventArgs e)
		{
			ActionDigitalOutput actionDigitalOutput;
			if (this.TryGetSelectedAction(out actionDigitalOutput) && this.gridViewDigitalOutputs.FocusedColumn == this.colResetCondition && actionDigitalOutput.StopType == null)
			{
				e.Cancel = true;
			}
		}

		private void gridViewDigitalOutputs_ShownEditor(object sender, EventArgs e)
		{
			ActionDigitalOutput actionDigitalOutput3;
			if (this.gridViewDigitalOutputs.FocusedColumn == this.colSetEventType)
			{
				ImageComboBoxEdit imageComboBoxEdit = this.gridViewDigitalOutputs.ActiveEditor as ImageComboBoxEdit;
				if (imageComboBoxEdit != null)
				{
					this.oldSelectedSetEventIndex = imageComboBoxEdit.SelectedIndex;
					return;
				}
			}
			else if (this.gridViewDigitalOutputs.FocusedColumn == this.colResetEventType)
			{
				ImageComboBoxEdit imageComboBoxEdit2 = this.gridViewDigitalOutputs.ActiveEditor as ImageComboBoxEdit;
				if (imageComboBoxEdit2 != null)
				{
					this.oldSelectedResetEventIndex = imageComboBoxEdit2.SelectedIndex;
					ActionDigitalOutput actionDigitalOutput;
					if (this.TryGetSelectedAction(out actionDigitalOutput) && (actionDigitalOutput.Event is OnStartEvent || actionDigitalOutput.Event is SymbolicMessageEvent || actionDigitalOutput.Event is IdEvent || actionDigitalOutput.Event is MsgTimeoutEvent || (actionDigitalOutput.Event is ISymbolicSignalEvent && (actionDigitalOutput.Event as ISymbolicSignalEvent).Relation.Value == CondRelation.OnChange)))
					{
						for (int i = 0; i < imageComboBoxEdit2.Properties.Items.Count; i++)
						{
							if (imageComboBoxEdit2.Properties.Items[i].ToString() == Resources.SETReset)
							{
								imageComboBoxEdit2.Properties.Items.RemoveAt(i);
								return;
							}
						}
						return;
					}
				}
			}
			else if (this.gridViewDigitalOutputs.FocusedColumn == this.colSetCondition)
			{
				ActionDigitalOutput actionDigitalOutput2;
				if (this.TryGetSelectedAction(out actionDigitalOutput2))
				{
					this.PrepareConditionAlternativeInplaceEditor(actionDigitalOutput2.Event);
					return;
				}
			}
			else if (this.gridViewDigitalOutputs.FocusedColumn == this.colResetCondition && this.TryGetSelectedAction(out actionDigitalOutput3) && actionDigitalOutput3.StopType is StopOnEvent)
			{
				this.PrepareConditionAlternativeInplaceEditor((actionDigitalOutput3.StopType as StopOnEvent).Event);
			}
		}

		private void PrepareConditionAlternativeInplaceEditor(Event ev)
		{
			if (ev is IgnitionEvent)
			{
				ComboBoxEdit comboBoxEdit = this.gridViewDigitalOutputs.ActiveEditor as ComboBoxEdit;
				if (comboBoxEdit != null)
				{
					comboBoxEdit.Properties.Items.Clear();
					comboBoxEdit.Properties.Items.Add(Resources.IgnitionOff);
					comboBoxEdit.Properties.Items.Add(Resources.IgnitionOn);
					return;
				}
			}
			else if (ev is KeyEvent)
			{
				ComboBoxEdit comboBoxEdit2 = this.gridViewDigitalOutputs.ActiveEditor as ComboBoxEdit;
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

		private void repositoryItemMemoExEditComment_EditValueChanged(object sender, EventArgs e)
		{
			this.gridViewDigitalOutputs.PostEditor();
		}

		private void repositoryItemCheckEditIsActive_CheckedChanged(object sender, EventArgs e)
		{
			this.gridViewDigitalOutputs.PostEditor();
		}

		private void repositoryItemComboBoxKeyCondition_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewDigitalOutputs.PostEditor();
		}

		private void repositoryItemComboBoxIgnitionCondition_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewDigitalOutputs.PostEditor();
		}

		private void toolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
		{
			try
			{
				GridHitInfo gridHitInfo = this.gridViewDigitalOutputs.CalcHitInfo(e.ControlMousePosition);
				if (gridHitInfo.IsValid)
				{
					ActionDigitalOutput actionDigitalOutput;
					if (gridHitInfo.Column == this.colComment && this.TryGetAction(this.gridViewDigitalOutputs.GetDataSourceRowIndex(gridHitInfo.RowHandle), out actionDigitalOutput) && !this.pageValidator.General.HasError(actionDigitalOutput.Comment))
					{
						object rowCellValue = this.gridViewDigitalOutputs.GetRowCellValue(gridHitInfo.RowHandle, this.colComment);
						if (rowCellValue != null && !string.IsNullOrEmpty(rowCellValue.ToString()))
						{
							e.Info = new ToolTipControlInfo(new CellToolTipInfo(gridHitInfo.RowHandle, gridHitInfo.Column, "cell"), rowCellValue.ToString());
							return;
						}
					}
					if ((gridHitInfo.Column == this.colSetEventType || gridHitInfo.Column == this.colResetEventType) && this.TryGetAction(this.gridViewDigitalOutputs.GetDataSourceRowIndex(gridHitInfo.RowHandle), out actionDigitalOutput) && this.HasLocalModelErrorStopOnStartEventIncompatible(actionDigitalOutput))
					{
						e.Info = new ToolTipControlInfo(new CellToolTipInfo(gridHitInfo.RowHandle, gridHitInfo.Column, "cell"), Resources.ErrorSETEventCantBeUsedWithRESETEvent);
					}
				}
			}
			catch
			{
			}
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

		public CANBusStatisticsEvent CreateCANBusStatisticsEvent()
		{
			CANBusStatisticsCondition cANBusStatisticsDialog = this.GetCANBusStatisticsDialog();
			if (DialogResult.OK == cANBusStatisticsDialog.ShowDialog())
			{
				return new CANBusStatisticsEvent(cANBusStatisticsDialog.CANBusStatisticsEvent);
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

		private bool EditEventCondition(Event ev)
		{
			if (ev is OnStartEvent)
			{
				return this.EditOnStartEventCondition(ev as OnStartEvent);
			}
			if (ev is CANIdEvent)
			{
				return this.EditCANIdCondition(ev as CANIdEvent);
			}
			if (ev is LINIdEvent)
			{
				return this.EditLINIdCondition(ev as LINIdEvent);
			}
			if (ev is FlexrayIdEvent)
			{
				return this.EditFlexrayIdCondition(ev as FlexrayIdEvent);
			}
			if (ev is CANDataEvent)
			{
				return this.EditCANDataTriggerCondition(ev as CANDataEvent);
			}
			if (ev is LINDataEvent)
			{
				return this.EditLINDataCondition(ev as LINDataEvent);
			}
			if (ev is SymbolicMessageEvent)
			{
				return this.EditSymbolicMessageCondition(ev as SymbolicMessageEvent);
			}
			if (ev is ISymbolicSignalEvent)
			{
				return this.EditSymbolicSignalCondition(ev as ISymbolicSignalEvent);
			}
			if (ev is MsgTimeoutEvent)
			{
				return this.EditMsgTimeoutCondition(ev as MsgTimeoutEvent);
			}
			if (ev is CANBusStatisticsEvent)
			{
				return this.EditCANBusStatisticsCondition(ev as CANBusStatisticsEvent);
			}
			if (ev is KeyEvent)
			{
				return this.EditKeyCondition(ev as KeyEvent);
			}
			if (ev is DigitalInputEvent)
			{
				return this.EditDigitalInputCondition(ev as DigitalInputEvent);
			}
			if (ev is AnalogInputEvent)
			{
				return this.EditAnalogInputCondition(ev as AnalogInputEvent);
			}
			return ev is IgnitionEvent && this.EditIgnitionCondition(ev as IgnitionEvent);
		}

		private bool EditOnStartEventCondition(OnStartEvent onStartEvent)
		{
			OnStartCondition onStartConditionDialog = this.GetOnStartConditionDialog();
			onStartConditionDialog.Delay = onStartEvent.Delay.Value;
			if (DialogResult.OK == onStartConditionDialog.ShowDialog() && onStartEvent.Delay.Value != onStartConditionDialog.Delay)
			{
				onStartEvent.Delay.Value = onStartConditionDialog.Delay;
				return true;
			}
			return false;
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

		public MsgTimeoutCondition GetMsgTimeoutConditionDialog()
		{
			if (this.msgTimeoutConditionDialog == null)
			{
				this.msgTimeoutConditionDialog = new MsgTimeoutCondition(this.ModelValidator, this.ApplicationDatabaseManager);
			}
			return this.msgTimeoutConditionDialog;
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
			this.eventTypeImageCollection.AddImage(Resources.ImageCycMsgTimeoutLIN);
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
			this.eventTypeImageCollection.AddImage(Resources.ImageTimer);
			this.eventTypeNameToImageIndex.Add(Resources.Timer, 19);
			this.eventTypeImageCollection.AddImage(Resources.ImageNone);
			this.eventTypeNameToImageIndex.Add(Resources.None, 20);
			this.eventTypeImageCollection.AddImage(Resources.ImageNotSET);
			this.eventTypeNameToImageIndex.Add(Resources.SETReset, 21);
			this.eventTypeImageCollection.AddImage(Resources.ImageSymbSignalCcpXcp);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColCcpXcpSignal, 22);
			this.eventTypeImageCollection.AddImage(Resources.ImageDiagSignal);
			this.eventTypeNameToImageIndex.Add(Resources_Trigger.TriggerTypeNameColDiagnosticSignal, 23);
		}

		private IList<string> GetAvailableEventTypesForCurrentLogger()
		{
			List<string> list = new List<string>();
			list.Add(Vocabulary.TriggerTypeNameColOnStart);
			foreach (string current in GUIUtil.EventTypeNamesInCol)
			{
				if ((!(current == Resources_Trigger.TriggerTypeNameColKey) || this.ModelValidator.LoggerSpecifics.IO.NumberOfKeys != 0u || this.ModelValidator.LoggerSpecifics.IO.NumberOfPanelKeys != 0u) && (this.ModelValidator.LoggerSpecifics.LIN.NumberOfChannels != 0u || (!(current == Vocabulary.TriggerTypeNameColLINId) && !(current == Resources_Trigger.TriggerTypeNameColLINData) && !(current == Resources_Trigger.TriggerTypeNameColLINMsgTimeout) && !(current == Resources_Trigger.TriggerTypeNameColSymbolicLIN) && !(current == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN))) && (this.ModelValidator.LoggerSpecifics.IO.NumberOfDigitalInputs != 0u || !(current == Resources_Trigger.TriggerTypeNameColDigitalInput)) && (this.ModelValidator.LoggerSpecifics.Recording.IsIgnitionEventSupported || !(current == Resources_Trigger.TriggerTypeNameColIgnition)) && (this.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels != 0u || (!(current == Resources_Trigger.TriggerTypeNameColSymbolicFlexray) && !(current == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray) && !(current == Resources_Trigger.TriggerTypeNameColFlexray))) && !(current == Resources_Trigger.TriggerTypeNameColVoCanRecording) && (this.ModelValidator.LoggerSpecifics.Recording.IsCCPXCPSignalEventSupported || !(current == Resources_Trigger.TriggerTypeNameColCcpXcpSignal)) && (this.ModelValidator.LoggerSpecifics.Recording.IsDiagnosticsSupported || !(current == Resources_Trigger.TriggerTypeNameColDiagnosticSignal)))
				{
					list.Add(current);
				}
			}
			return list;
		}

		private void InitEventTypeNamesSetEventInPlaceEditor()
		{
			this.repositoryItemImageComboBoxSetEvent.Items.Clear();
			IList<string> availableEventTypesForCurrentLogger = this.GetAvailableEventTypesForCurrentLogger();
			foreach (string current in availableEventTypesForCurrentLogger)
			{
				this.repositoryItemImageComboBoxSetEvent.Items.Add(new ImageComboBoxItem(current, this.eventTypeNameToImageIndex[current]));
			}
		}

		private void InitEventTypeNamesResetEventInPlaceEditor()
		{
			this.repositoryItemImageComboBoxResetEvent.Items.Clear();
			this.repositoryItemImageComboBoxResetEvent.Items.Add(new ImageComboBoxItem(Resources.None, this.eventTypeNameToImageIndex[Resources.None]));
			this.repositoryItemImageComboBoxResetEvent.Items.Add(new ImageComboBoxItem(Resources.Timer, this.eventTypeNameToImageIndex[Resources.Timer]));
			this.repositoryItemImageComboBoxResetEvent.Items.Add(new ImageComboBoxItem(Resources.SETReset, this.eventTypeNameToImageIndex[Resources.SETReset]));
			IList<string> availableEventTypesForCurrentLogger = this.GetAvailableEventTypesForCurrentLogger();
			foreach (string current in availableEventTypesForCurrentLogger)
			{
				if (!(current == Vocabulary.TriggerTypeNameColOnStart))
				{
					this.repositoryItemImageComboBoxResetEvent.Items.Add(new ImageComboBoxItem(current, this.eventTypeNameToImageIndex[current]));
				}
			}
		}

		private void InitEventTypeNamesResetEventInPlaceEditor(ActionDigitalOutput selectedAction, ref ImageComboBoxEdit imageComboBoxEdit)
		{
			imageComboBoxEdit.Properties.Items.Clear();
			imageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(Resources.None, this.eventTypeNameToImageIndex[Resources.None]));
			imageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(Resources.Timer, this.eventTypeNameToImageIndex[Resources.Timer]));
			if ((selectedAction.Event is SymbolicSignalEvent && (selectedAction.Event as SymbolicSignalEvent).Relation.Value != CondRelation.OnChange) || selectedAction.Event is CANDataEvent || selectedAction.Event is LINDataEvent || selectedAction.Event is DigitalInputEvent || selectedAction.Event is AnalogInputEvent || selectedAction.Event is KeyEvent || selectedAction.Event is IgnitionEvent)
			{
				imageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(Resources.SETReset, this.eventTypeNameToImageIndex[Resources.SETReset]));
			}
			IList<string> availableEventTypesForCurrentLogger = this.GetAvailableEventTypesForCurrentLogger();
			foreach (string current in availableEventTypesForCurrentLogger)
			{
				if (!(current == Vocabulary.TriggerTypeNameColOnStart))
				{
					imageComboBoxEdit.Properties.Items.Add(new ImageComboBoxItem(current, this.eventTypeNameToImageIndex[current]));
				}
			}
		}

		public bool Serialize(DigitalOutputsPage digitalOutputsPage)
		{
			if (digitalOutputsPage == null)
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.gridViewDigitalOutputs.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				digitalOutputsPage.DigitalOutputsGridLayout = Convert.ToBase64String(array, 0, array.Length);
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

		public bool DeSerialize(DigitalOutputsPage digitalOutputsPage)
		{
			if (digitalOutputsPage == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(digitalOutputsPage.DigitalOutputsGridLayout))
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(digitalOutputsPage.DigitalOutputsGridLayout);
				memoryStream = new MemoryStream(buffer);
				this.gridViewDigitalOutputs.RestoreLayoutFromStream(memoryStream);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DigitalOutputsGrid));
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.gridControlDigitalOutputs = new GridControl();
			this.gridViewDigitalOutputs = new GridView();
			this.colActive = new GridColumn();
			this.repositoryItemCheckEditIsActive = new RepositoryItemCheckEdit();
			this.colOutputNumber = new GridColumn();
			this.colSetEventType = new GridColumn();
			this.repositoryItemImageComboBoxSetEvent = new RepositoryItemImageComboBox();
			this.colSetCondition = new GridColumn();
			this.repositoryItemButtonEditSetCondition = new RepositoryItemButtonEdit();
			this.colResetEventType = new GridColumn();
			this.repositoryItemImageComboBoxResetEvent = new RepositoryItemImageComboBox();
			this.colResetCondition = new GridColumn();
			this.repositoryItemButtonEditResetCondition = new RepositoryItemButtonEdit();
			this.colComment = new GridColumn();
			this.repositoryItemMemoExEditComment = new RepositoryItemMemoExEdit();
			this.repositoryItemComboBoxKeyCondition = new RepositoryItemComboBox();
			this.repositoryItemComboBoxIgnitionCondition = new RepositoryItemComboBox();
			this.repositoryItemTextEditTimer = new RepositoryItemTextEdit();
			this.repositoryItemTextEditDummy = new RepositoryItemTextEdit();
			this.repositoryItemSpinEditTimer = new RepositoryItemSpinEdit();
			this.toolTipController = new XtraToolTipController(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.gridControlDigitalOutputs).BeginInit();
			((ISupportInitialize)this.gridViewDigitalOutputs).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsActive).BeginInit();
			((ISupportInitialize)this.repositoryItemImageComboBoxSetEvent).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEditSetCondition).BeginInit();
			((ISupportInitialize)this.repositoryItemImageComboBoxResetEvent).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEditResetCondition).BeginInit();
			((ISupportInitialize)this.repositoryItemMemoExEditComment).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBoxKeyCondition).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBoxIgnitionCondition).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEditTimer).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).BeginInit();
			((ISupportInitialize)this.repositoryItemSpinEditTimer).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.gridControlDigitalOutputs, "gridControlDigitalOutputs");
			this.gridControlDigitalOutputs.MainView = this.gridViewDigitalOutputs;
			this.gridControlDigitalOutputs.Name = "gridControlDigitalOutputs";
			this.gridControlDigitalOutputs.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemCheckEditIsActive,
				this.repositoryItemImageComboBoxSetEvent,
				this.repositoryItemImageComboBoxResetEvent,
				this.repositoryItemButtonEditSetCondition,
				this.repositoryItemButtonEditResetCondition,
				this.repositoryItemMemoExEditComment,
				this.repositoryItemComboBoxKeyCondition,
				this.repositoryItemComboBoxIgnitionCondition,
				this.repositoryItemTextEditTimer,
				this.repositoryItemTextEditDummy,
				this.repositoryItemSpinEditTimer
			});
			this.gridControlDigitalOutputs.ToolTipController = this.toolTipController;
			this.gridControlDigitalOutputs.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewDigitalOutputs
			});
			this.gridViewDigitalOutputs.Columns.AddRange(new GridColumn[]
			{
				this.colActive,
				this.colOutputNumber,
				this.colSetEventType,
				this.colSetCondition,
				this.colResetEventType,
				this.colResetCondition,
				this.colComment
			});
			this.gridViewDigitalOutputs.GridControl = this.gridControlDigitalOutputs;
			this.gridViewDigitalOutputs.Name = "gridViewDigitalOutputs";
			this.gridViewDigitalOutputs.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;
			this.gridViewDigitalOutputs.OptionsCustomization.AllowFilter = false;
			this.gridViewDigitalOutputs.OptionsCustomization.AllowGroup = false;
			this.gridViewDigitalOutputs.OptionsCustomization.AllowQuickHideColumns = false;
			this.gridViewDigitalOutputs.OptionsCustomization.AllowSort = false;
			this.gridViewDigitalOutputs.OptionsView.ShowGroupPanel = false;
			this.gridViewDigitalOutputs.OptionsView.ShowIndicator = false;
			this.gridViewDigitalOutputs.PaintStyleName = "WindowsXP";
			this.gridViewDigitalOutputs.RowHeight = 20;
			this.gridViewDigitalOutputs.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewDigitalOutputs_CustomDrawCell);
			this.gridViewDigitalOutputs.RowStyle += new RowStyleEventHandler(this.gridViewDigitalOutputs_RowStyle);
			this.gridViewDigitalOutputs.CustomRowCellEdit += new CustomRowCellEditEventHandler(this.gridViewDigitalOutputs_CustomRowCellEdit);
			this.gridViewDigitalOutputs.LeftCoordChanged += new EventHandler(this.gridViewDigitalOutputs_LeftCoordChanged);
			this.gridViewDigitalOutputs.TopRowChanged += new EventHandler(this.gridViewDigitalOutputs_TopRowChanged);
			this.gridViewDigitalOutputs.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewDigitalOutputs_PopupMenuShowing);
			this.gridViewDigitalOutputs.ShowingEditor += new CancelEventHandler(this.gridViewDigitalOutputs_ShowingEditor);
			this.gridViewDigitalOutputs.ShownEditor += new EventHandler(this.gridViewDigitalOutputs_ShownEditor);
			this.gridViewDigitalOutputs.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewDigitalOutputs_CustomUnboundColumnData);
			this.gridViewDigitalOutputs.Click += new EventHandler(this.gridViewDigitalOutputs_Click);
			componentResourceManager.ApplyResources(this.colActive, "colActive");
			this.colActive.ColumnEdit = this.repositoryItemCheckEditIsActive;
			this.colActive.FieldName = "colActiveField";
			this.colActive.Name = "colActive";
			this.colActive.UnboundType = UnboundColumnType.Boolean;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditIsActive, "repositoryItemCheckEditIsActive");
			this.repositoryItemCheckEditIsActive.Name = "repositoryItemCheckEditIsActive";
			this.repositoryItemCheckEditIsActive.CheckedChanged += new EventHandler(this.repositoryItemCheckEditIsActive_CheckedChanged);
			componentResourceManager.ApplyResources(this.colOutputNumber, "colOutputNumber");
			this.colOutputNumber.FieldName = "colOutputNumberField";
			this.colOutputNumber.Name = "colOutputNumber";
			this.colOutputNumber.OptionsColumn.AllowEdit = false;
			this.colOutputNumber.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colSetEventType, "colSetEventType");
			this.colSetEventType.ColumnEdit = this.repositoryItemImageComboBoxSetEvent;
			this.colSetEventType.FieldName = "colSetEventTypeField";
			this.colSetEventType.Name = "colSetEventType";
			this.colSetEventType.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemImageComboBoxSetEvent, "repositoryItemImageComboBoxSetEvent");
			this.repositoryItemImageComboBoxSetEvent.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemImageComboBoxSetEvent.Buttons"))
			});
			this.repositoryItemImageComboBoxSetEvent.Name = "repositoryItemImageComboBoxSetEvent";
			this.repositoryItemImageComboBoxSetEvent.SelectedIndexChanged += new EventHandler(this.repositoryItemImageComboBoxSetEvent_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.colSetCondition, "colSetCondition");
			this.colSetCondition.ColumnEdit = this.repositoryItemButtonEditSetCondition;
			this.colSetCondition.FieldName = "colSetConditionField";
			this.colSetCondition.Name = "colSetCondition";
			this.colSetCondition.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonEditSetCondition, "repositoryItemButtonEditSetCondition");
			this.repositoryItemButtonEditSetCondition.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEditSetCondition.Name = "repositoryItemButtonEditSetCondition";
			this.repositoryItemButtonEditSetCondition.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEditSetCondition.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEditSetCondition_ButtonClick);
			componentResourceManager.ApplyResources(this.colResetEventType, "colResetEventType");
			this.colResetEventType.ColumnEdit = this.repositoryItemImageComboBoxResetEvent;
			this.colResetEventType.FieldName = "colResetEventTypeField";
			this.colResetEventType.Name = "colResetEventType";
			this.colResetEventType.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemImageComboBoxResetEvent, "repositoryItemImageComboBoxResetEvent");
			this.repositoryItemImageComboBoxResetEvent.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemImageComboBoxResetEvent.Buttons"))
			});
			this.repositoryItemImageComboBoxResetEvent.Name = "repositoryItemImageComboBoxResetEvent";
			this.repositoryItemImageComboBoxResetEvent.SelectedIndexChanged += new EventHandler(this.repositoryItemImageComboBoxResetEvent_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.colResetCondition, "colResetCondition");
			this.colResetCondition.ColumnEdit = this.repositoryItemButtonEditResetCondition;
			this.colResetCondition.FieldName = "colResetConditionField";
			this.colResetCondition.Name = "colResetCondition";
			this.colResetCondition.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonEditResetCondition, "repositoryItemButtonEditResetCondition");
			this.repositoryItemButtonEditResetCondition.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEditResetCondition.Name = "repositoryItemButtonEditResetCondition";
			this.repositoryItemButtonEditResetCondition.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEditResetCondition.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEditResetCondition_ButtonClick);
			componentResourceManager.ApplyResources(this.colComment, "colComment");
			this.colComment.ColumnEdit = this.repositoryItemMemoExEditComment;
			this.colComment.FieldName = "colCommentField";
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
			componentResourceManager.ApplyResources(this.repositoryItemTextEditTimer, "repositoryItemTextEditTimer");
			this.repositoryItemTextEditTimer.Name = "repositoryItemTextEditTimer";
			componentResourceManager.ApplyResources(this.repositoryItemTextEditDummy, "repositoryItemTextEditDummy");
			this.repositoryItemTextEditDummy.Name = "repositoryItemTextEditDummy";
			componentResourceManager.ApplyResources(this.repositoryItemSpinEditTimer, "repositoryItemSpinEditTimer");
			this.repositoryItemSpinEditTimer.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemSpinEditTimer.DisplayFormat.FormatString = "{0} ms";
			this.repositoryItemSpinEditTimer.DisplayFormat.FormatType = FormatType.Numeric;
			RepositoryItemSpinEdit arg_B7F_0 = this.repositoryItemSpinEditTimer;
			int[] array = new int[4];
			array[0] = 100;
			arg_B7F_0.Increment = new decimal(array);
			this.repositoryItemSpinEditTimer.IsFloatValue = false;
			this.repositoryItemSpinEditTimer.Mask.EditMask = componentResourceManager.GetString("repositoryItemSpinEditTimer.Mask.EditMask");
			RepositoryItemSpinEdit arg_BC9_0 = this.repositoryItemSpinEditTimer;
			int[] array2 = new int[4];
			array2[0] = 60000;
			arg_BC9_0.MaxValue = new decimal(array2);
			RepositoryItemSpinEdit arg_BE9_0 = this.repositoryItemSpinEditTimer;
			int[] array3 = new int[4];
			array3[0] = 100;
			arg_BE9_0.MinValue = new decimal(array3);
			this.repositoryItemSpinEditTimer.Name = "repositoryItemSpinEditTimer";
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
			base.Controls.Add(this.gridControlDigitalOutputs);
			base.Name = "DigitalOutputsGrid";
			componentResourceManager.ApplyResources(this, "$this");
			base.Resize += new EventHandler(this.DigitalOutputsGrid_Resize);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.gridControlDigitalOutputs).EndInit();
			((ISupportInitialize)this.gridViewDigitalOutputs).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsActive).EndInit();
			((ISupportInitialize)this.repositoryItemImageComboBoxSetEvent).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEditSetCondition).EndInit();
			((ISupportInitialize)this.repositoryItemImageComboBoxResetEvent).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEditResetCondition).EndInit();
			((ISupportInitialize)this.repositoryItemMemoExEditComment).EndInit();
			((ISupportInitialize)this.repositoryItemComboBoxKeyCondition).EndInit();
			((ISupportInitialize)this.repositoryItemComboBoxIgnitionCondition).EndInit();
			((ISupportInitialize)this.repositoryItemTextEditTimer).EndInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).EndInit();
			((ISupportInitialize)this.repositoryItemSpinEditTimer).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
