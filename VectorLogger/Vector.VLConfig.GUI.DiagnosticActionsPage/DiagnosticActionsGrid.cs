using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Drawing;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
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
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.EventConditions;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.DiagnosticActionsPage
{
	internal class DiagnosticActionsGrid : UserControl
	{
		private delegate void GridRowContextMenuHandler(object sender, EventArgs e);

		private DiagnosticActionsConfiguration diagnosticActionsConfiguration;

		private DisplayMode displayMode;

		private DiagnosticActionsPage actionsPageLayout;

		private bool isSymSelOpened;

		private GUIElementManager_ControlGridTree guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isGroupRowSelected;

		private bool isExpandingLocked;

		private Dictionary<TriggeredDiagnosticActionSequence, bool> expandStates;

		private Dictionary<DiagnosticAction, string> disassParamsValuesCell;

		private Dictionary<DiagnosticAction, string> disassParamsValuesTooltip;

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

		private DiagnosticAction actionInClipboard;

		private TriggeredDiagnosticActionSequence sequenceInClipboard;

		private GridHitInfo downHitInfo;

		private static readonly int iconSize = 16;

		private IContainer components;

		private GridControl gridControlDiagnosticActions;

		private GridView gridViewDiagnosticActions;

		private GridColumn colEvent;

		private GridColumn colService;

		private GridColumn colEcu;

		private GridColumn colParameter;

		private Button buttonMoveFirst;

		private Button buttonMoveUp;

		private Button buttonMoveDown;

		private Button buttonMoveLast;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private BindingSource diagActionsBindingSource;

		private RepositoryItemButtonEdit repositoryItemButtonEditParams;

		private XtraToolTipController toolTipController;

		private RepositoryItemButtonEdit repositoryItemButtonEditService;

		private GridColumn colRawMessage;

		private GridColumn colSession;

		private RepositoryItemTextEdit repositoryItemTextEditDummy;

		private GridColumn colSignal;

		private RepositoryItemButtonEdit repositoryItemButtonEditSignal;

		public event EventHandler SelectionChanged;

		public DiagnosticActionsConfiguration DiagnosticActionsConfiguration
		{
			get
			{
				return this.diagnosticActionsConfiguration;
			}
			set
			{
				this.diagnosticActionsConfiguration = value;
				if (value != null)
				{
					int focusedRowHandle = this.gridViewDiagnosticActions.FocusedRowHandle;
					this.RenderParameterDisplayStrings();
					this.gridControlDiagnosticActions.DataSource = this.diagnosticActionsConfiguration.DiagnosticActions;
					this.gridViewDiagnosticActions.RefreshData();
					this.SetExpandStates();
					if ((focusedRowHandle >= 0 && focusedRowHandle < this.gridViewDiagnosticActions.RowCount) || (focusedRowHandle < 0 && focusedRowHandle >= -this.GetGroupRowCount()))
					{
						this.gridViewDiagnosticActions.FocusedRowHandle = focusedRowHandle;
					}
					this.UpdateUpDownButtons();
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
				if (this.displayMode != null)
				{
					this.gridViewDiagnosticActions.RefreshData();
				}
				this.displayMode = value;
			}
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		public IDiagSymbolsManager DiagSymbolsManager
		{
			get;
			set;
		}

		public DiagnosticsDatabaseConfiguration DiagnosticDatabaseConfiguration
		{
			get;
			set;
		}

		public bool IsGroupRowSelected
		{
			get
			{
				return this.isGroupRowSelected;
			}
		}

		public DiagnosticActionsGrid()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_ControlGridTree();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.isGroupRowSelected = false;
			this.isExpandingLocked = false;
			this.expandStates = new Dictionary<TriggeredDiagnosticActionSequence, bool>();
			this.disassParamsValuesCell = new Dictionary<DiagnosticAction, string>();
			this.disassParamsValuesTooltip = new Dictionary<DiagnosticAction, string>();
			this.actionInClipboard = null;
			this.sequenceInClipboard = null;
			this.downHitInfo = null;
			this.gridViewDiagnosticActions.ForceDoubleClick = true;
			this.isSymSelOpened = false;
		}

		public void Init()
		{
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		private void Raise_SelectionChanged(object sender, EventArgs e)
		{
			if (this.SelectionChanged != null)
			{
				this.SelectionChanged(sender, e);
			}
		}

		private void gridViewDiagnosticActions_DoubleClick(object sender, EventArgs e)
		{
			GridHitInfo gridHitInfo = this.gridViewDiagnosticActions.CalcHitInfo(this.gridControlDiagnosticActions.PointToClient(Control.MousePosition));
			if (gridHitInfo.InRowCell)
			{
				if (this.gridViewDiagnosticActions.FocusedColumn == this.colParameter || this.gridViewDiagnosticActions.FocusedColumn == this.colRawMessage || this.gridViewDiagnosticActions.FocusedColumn == this.colSession)
				{
					DiagnosticAction diagnosticAction;
					if (this.TryGetSelectedAction(out diagnosticAction))
					{
						if (diagnosticAction is DiagnosticDummyAction || (diagnosticAction is DiagnosticSignalRequest && this.gridViewDiagnosticActions.FocusedColumn != this.colSession))
						{
							return;
						}
						if (this.gridViewDiagnosticActions.FocusedColumn != this.colSession && diagnosticAction.HasOnlyConstParams)
						{
							return;
						}
						this.EditServiceParameters(ref diagnosticAction);
						return;
					}
				}
				else if (this.gridViewDiagnosticActions.FocusedColumn == this.colService)
				{
					DiagnosticAction diagnosticAction2;
					if (this.TryGetSelectedAction(out diagnosticAction2) && (diagnosticAction2 is DiagnosticDummyAction || diagnosticAction2 is DiagnosticSignalRequest))
					{
						return;
					}
					this.ReplaceAction();
					return;
				}
			}
			else
			{
				if (gridHitInfo.InColumnPanel)
				{
					gridHitInfo.Column.BestFit();
					return;
				}
				if (this.isGroupRowSelected && gridHitInfo.HitTest == GridHitTest.Row)
				{
					this.EditActionSequenceEventCondition();
					this.isExpandingLocked = true;
				}
			}
		}

		private void gridViewDiagnosticActions_GroupRowExpanding(object sender, RowAllowEventArgs e)
		{
			e.Allow = !this.isExpandingLocked;
			this.isExpandingLocked = false;
		}

		private void gridViewDiagnosticActions_GroupRowExpanded(object sender, RowEventArgs e)
		{
			this.gridControlDiagnosticActions.Refresh();
			this.DisplayErrors();
		}

		private void gridViewDiagnosticActions_GroupRowCollapsing(object sender, RowAllowEventArgs e)
		{
			e.Allow = !this.isExpandingLocked;
			this.isExpandingLocked = false;
		}

		private void gridViewDiagnosticActions_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			DiagnosticAction action;
			if (!this.TryGetAction(e.ListSourceRowIndex, out action))
			{
				return;
			}
			if (e.Column == this.colEvent)
			{
				this.UnboundColumnDataEvent(action, e);
				return;
			}
			if (e.Column == this.colSignal)
			{
				this.UnboundColumnSignal(action, e);
				return;
			}
			if (e.Column == this.colEcu)
			{
				this.UnboundColumnDataEcuName(action, e);
				return;
			}
			if (e.Column == this.colService)
			{
				this.UnboundColumnDataService(action, e);
				return;
			}
			if (e.Column == this.colParameter)
			{
				this.UnboundColumnDataParameter(action, e);
				return;
			}
			if (e.Column == this.colRawMessage)
			{
				this.UnboundColumnRawMessage(action, e);
				return;
			}
			if (e.Column == this.colSession)
			{
				this.UnboundColumnSession(action, e);
			}
		}

		private void gridViewDiagnosticActions_ShowingEditor(object sender, CancelEventArgs e)
		{
			DiagnosticAction diagnosticAction;
			if (this.TryGetSelectedAction(out diagnosticAction))
			{
				if (diagnosticAction is DiagnosticDummyAction)
				{
					e.Cancel = true;
					return;
				}
				if (this.gridViewDiagnosticActions.FocusedColumn == this.colSignal && !(diagnosticAction is DiagnosticSignalRequest))
				{
					e.Cancel = true;
					return;
				}
				if (this.gridViewDiagnosticActions.FocusedColumn == this.colService && diagnosticAction is DiagnosticSignalRequest)
				{
					e.Cancel = true;
					return;
				}
				if ((diagnosticAction.HasOnlyConstParams || diagnosticAction is DiagnosticSignalRequest) && (this.gridViewDiagnosticActions.FocusedColumn == this.colParameter || this.gridViewDiagnosticActions.FocusedColumn == this.colRawMessage))
				{
					e.Cancel = true;
				}
			}
		}

		private void gridViewDiagnosticActions_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			DiagnosticAction diagnosticAction;
			if (this.TryGetAction(this.gridViewDiagnosticActions.GetDataSourceRowIndex(e.RowHandle), out diagnosticAction))
			{
				if (diagnosticAction is DiagnosticDummyAction)
				{
					e.RepositoryItem = this.repositoryItemTextEditDummy;
					return;
				}
				if (this.gridViewDiagnosticActions.FocusedColumn == this.colSignal && !(diagnosticAction is DiagnosticSignalRequest))
				{
					e.RepositoryItem = this.repositoryItemTextEditDummy;
					return;
				}
				if (this.gridViewDiagnosticActions.FocusedColumn == this.colService && diagnosticAction is DiagnosticSignalRequest)
				{
					e.RepositoryItem = this.repositoryItemTextEditDummy;
					return;
				}
				if ((diagnosticAction.HasOnlyConstParams || diagnosticAction is DiagnosticSignalRequest) && (e.Column == this.colParameter || e.Column == this.colRawMessage))
				{
					e.RepositoryItem = this.repositoryItemTextEditDummy;
				}
			}
		}

		private void gridViewDiagnosticActions_ShownEditor(object sender, EventArgs e)
		{
		}

		private void gridViewDiagnosticActions_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				if (this.isGroupRowSelected)
				{
					this.RemoveActionSequence(true);
					return;
				}
				DiagnosticAction diagnosticAction;
				if (this.TryGetSelectedAction(out diagnosticAction) && !(diagnosticAction is DiagnosticDummyAction))
				{
					this.RemoveAction();
					return;
				}
			}
			else if (e.Modifiers == Keys.Control)
			{
				if (e.KeyCode == Keys.X)
				{
					if (!this.isGroupRowSelected)
					{
						this.CutAction();
						return;
					}
				}
				else if (e.KeyCode == Keys.C)
				{
					if (this.isGroupRowSelected)
					{
						this.CopySequence();
						return;
					}
					this.CopyAction();
					return;
				}
				else if (e.KeyCode == Keys.V)
				{
					if (this.isGroupRowSelected)
					{
						this.PasteSequence();
						return;
					}
					this.PasteAction();
				}
			}
		}

		private void gridViewDiagnosticActions_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
			this.isGroupRowSelected = (e.FocusedRowHandle < 0);
			this.Raise_SelectionChanged(this, EventArgs.Empty);
			this.UpdateUpDownButtons();
		}

		private void gridViewDiagnosticActions_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
		{
			int childRowHandle = this.gridViewDiagnosticActions.GetChildRowHandle(e.RowHandle, 0);
			int dataSourceRowIndex = this.gridViewDiagnosticActions.GetDataSourceRowIndex(childRowHandle);
			if (dataSourceRowIndex < 0 || dataSourceRowIndex >= this.diagnosticActionsConfiguration.DiagnosticActions.Count)
			{
				e.Handled = false;
				return;
			}
			GridGroupRowPainter gridGroupRowPainter = e.Painter as GridGroupRowPainter;
			if (gridGroupRowPainter == null)
			{
				return;
			}
			GridGroupRowInfo gridGroupRowInfo = e.Info as GridGroupRowInfo;
			if (gridGroupRowInfo == null)
			{
				return;
			}
			Rectangle buttonBounds = gridGroupRowInfo.ButtonBounds;
			Rectangle bounds = e.Bounds;
			bounds.X = buttonBounds.Right + 4;
			gridGroupRowPainter.DrawGroupRowBackground(e.Info);
			OpenCloseButtonInfoArgs e2 = new OpenCloseButtonInfoArgs(e.Cache, gridGroupRowInfo.ButtonBounds, gridGroupRowInfo.GroupExpanded, gridGroupRowInfo.AppearanceGroupButton, ObjectState.Normal);
			if (!gridGroupRowInfo.ButtonBounds.IsEmpty)
			{
				gridGroupRowPainter.ElementsPainter.OpenCloseButton.DrawObject(e2);
			}
			Point location = bounds.Location;
			int num = (bounds.Height - DiagnosticActionsGrid.iconSize) / 2;
			DiagnosticAction diagnosticAction = this.diagnosticActionsConfiguration.DiagnosticActions[dataSourceRowIndex];
			string text;
			if (this.HasEventGlobalModelErrors(diagnosticAction.TriggeredDiagnosticActionSequence.Event, out text))
			{
				Rectangle rect = new Rectangle(location.X, location.Y + num, DiagnosticActionsGrid.iconSize, DiagnosticActionsGrid.iconSize);
				e.Graphics.DrawImageUnscaled(Resources.IconGlobalModelError.ToBitmap(), rect);
				location.X += DiagnosticActionsGrid.iconSize + 2;
			}
			Rectangle rect2 = new Rectangle(location.X, location.Y + num, DiagnosticActionsGrid.iconSize, DiagnosticActionsGrid.iconSize);
			if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is CyclicTimerEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageCyclicTimer, rect2);
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is OnStartEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageStart, rect2);
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is CANIdEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageIDMessageCAN, rect2);
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is FlexrayIdEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageIDMessageFlexray, rect2);
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is LINIdEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageIDMessageLIN, rect2);
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is CANDataEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageDataTriggerCAN, rect2);
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is LINDataEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageDataTriggerLIN, rect2);
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is SymbolicMessageEvent)
			{
				switch ((diagnosticAction.TriggeredDiagnosticActionSequence.Event as SymbolicMessageEvent).BusType.Value)
				{
				case BusType.Bt_CAN:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbMessageCAN, rect2);
					break;
				case BusType.Bt_LIN:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbMessageLIN, rect2);
					break;
				case BusType.Bt_FlexRay:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbMessageFlexray, rect2);
					break;
				default:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbMessageCAN, rect2);
					break;
				}
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is SymbolicSignalEvent)
			{
				switch ((diagnosticAction.TriggeredDiagnosticActionSequence.Event as SymbolicSignalEvent).BusType.Value)
				{
				case BusType.Bt_CAN:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbSignalCAN, rect2);
					break;
				case BusType.Bt_LIN:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbSignalLIN, rect2);
					break;
				case BusType.Bt_FlexRay:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbSignalFlexRay, rect2);
					break;
				default:
					e.Graphics.DrawImageUnscaled(Resources.ImageSymbSignalCAN, rect2);
					break;
				}
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is CcpXcpSignalEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageSymbSignalCcpXcp, rect2);
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is DiagnosticSignalEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageDiagSignal, rect2);
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is MsgTimeoutEvent)
			{
				switch ((diagnosticAction.TriggeredDiagnosticActionSequence.Event as MsgTimeoutEvent).BusType.Value)
				{
				case BusType.Bt_CAN:
					e.Graphics.DrawImageUnscaled(Resources.ImageCycMsgTimeoutCAN, rect2);
					break;
				case BusType.Bt_LIN:
					e.Graphics.DrawImageUnscaled(Resources.ImageCycMsgTimeoutLIN, rect2);
					break;
				case BusType.Bt_FlexRay:
					e.Graphics.DrawImageUnscaled(Resources.ImageCycMsgTimeoutFlexray, rect2);
					break;
				default:
					e.Graphics.DrawImageUnscaled(Resources.ImageCycMsgTimeoutCAN, rect2);
					break;
				}
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is DigitalInputEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageDigitalInput, rect2);
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is AnalogInputEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageAnalogInputSignal, rect2);
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is KeyEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageKey, rect2);
			}
			else if (diagnosticAction.TriggeredDiagnosticActionSequence.Event is IgnitionEvent)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageOnIgnition, rect2);
			}
			location.X += DiagnosticActionsGrid.iconSize + 2;
			bounds.Location = location;
			GridView gridView = sender as GridView;
			string text2 = (gridView != null) ? gridView.GetGroupRowDisplayText(e.RowHandle) : string.Empty;
			e.Appearance.DrawString(e.Cache, text2, bounds);
			e.Handled = true;
		}

		private void gridViewDiagnosticActions_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(e.Column, this.gridViewDiagnosticActions.GetDataSourceRowIndex(e.RowHandle));
			this.customErrorProvider.Grid.DisplayError(gUIElement, e);
			DiagnosticAction diagnosticAction = this.diagnosticActionsConfiguration.DiagnosticActions[this.gridViewDiagnosticActions.GetDataSourceRowIndex(e.RowHandle)];
			if (diagnosticAction != null && (diagnosticAction.HasOnlyConstParams || diagnosticAction is DiagnosticSignalRequest) && (e.Column == this.colParameter || e.Column == this.colRawMessage))
			{
				e.Appearance.ForeColor = SystemColors.GrayText;
			}
		}

		private void gridViewDiagnosticActions_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
		{
			if (e.Column == this.colEvent)
			{
				DiagnosticAction diagnosticAction = this.diagnosticActionsConfiguration.DiagnosticActions[e.ListSourceRowIndex1];
				DiagnosticAction diagnosticAction2 = this.diagnosticActionsConfiguration.DiagnosticActions[e.ListSourceRowIndex2];
				int num = this.diagnosticActionsConfiguration.TriggeredActionSequences.IndexOf(diagnosticAction.TriggeredDiagnosticActionSequence);
				int num2 = this.diagnosticActionsConfiguration.TriggeredActionSequences.IndexOf(diagnosticAction2.TriggeredDiagnosticActionSequence);
				if (num < num2)
				{
					e.Result = -1;
				}
				else if (num > num2)
				{
					e.Result = 1;
				}
				else
				{
					e.Result = 0;
				}
				e.Handled = true;
				return;
			}
			e.Handled = false;
		}

		private void gridViewDiagnosticActions_CustomColumnGroup(object sender, CustomColumnSortEventArgs e)
		{
			if (e.Column == this.colEvent)
			{
				DiagnosticAction diagnosticAction = this.diagnosticActionsConfiguration.DiagnosticActions[e.ListSourceRowIndex1];
				DiagnosticAction diagnosticAction2 = this.diagnosticActionsConfiguration.DiagnosticActions[e.ListSourceRowIndex2];
				if (diagnosticAction.TriggeredDiagnosticActionSequence == diagnosticAction2.TriggeredDiagnosticActionSequence)
				{
					e.Result = 0;
				}
				else
				{
					e.Result = 1;
				}
				e.Handled = true;
				return;
			}
			e.Handled = false;
		}

		private void gridViewDiagnosticActions_TopRowChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridViewDiagnosticActions_LeftCoordChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void DiagnosticActionsGrid_Resize(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridViewDiagnosticActions_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (e.MenuType == GridMenuType.Column)
			{
				this.CustomizeColumnHeaderMenu(e);
			}
			if (e.MenuType == GridMenuType.Row)
			{
				if (this.isGroupRowSelected)
				{
					this.DisplayGridRowEventContextMenu(e);
					return;
				}
				this.DisplayGridRowActionMenu(e);
			}
		}

		private void CustomizeColumnHeaderMenu(PopupMenuShowingEventArgs e)
		{
			string localizedString = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnGroupBox);
			string localizedString2 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuGroupPanelShow);
			string localizedString3 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnFilter);
			string localizedString4 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnFilterEditor);
			string localizedString5 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnGroup);
			string localizedString6 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnUnGroup);
			string localizedString7 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnRemoveColumn);
			string localizedString8 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnClearSorting);
			string localizedString9 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnColumnCustomization);
			string localizedString10 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnSortAscending);
			string localizedString11 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnSortDescending);
			for (int i = e.Menu.Items.Count - 1; i >= 0; i--)
			{
				string caption = e.Menu.Items[i].Caption;
				if (localizedString5 == caption || localizedString6 == caption || caption.Contains(localizedString) || localizedString2 == caption || localizedString3 == caption || localizedString4 == caption || localizedString7 == caption || localizedString8 == caption || localizedString9 == caption || localizedString10 == caption || localizedString11 == caption)
				{
					e.Menu.Items.RemoveAt(i);
				}
			}
		}

		private DXMenuItem CreateMenuItem(string caption, DiagnosticActionsGrid.GridRowContextMenuHandler target, bool isBeginOfGroup)
		{
			return new DXMenuItem(caption, new EventHandler(target.Invoke))
			{
				BeginGroup = isBeginOfGroup
			};
		}

		private void DisplayGridRowEventContextMenu(PopupMenuShowingEventArgs e)
		{
			e.Menu.Items.Clear();
			e.Menu.Items.Add(this.CreateMenuItem(Resources.EditCondition, new DiagnosticActionsGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuEditEvent), false));
			e.Menu.Items.Add(this.CreateMenuItem(Resources.RemoveEvent, new DiagnosticActionsGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuRemoveEvent), true));
			e.Menu.Items.Add(this.CreateMenuItem(Resources.CopyEvent, new DiagnosticActionsGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuCopyEvent), false));
			int index = e.Menu.Items.Add(this.CreateMenuItem(Resources.PasteEvent, new DiagnosticActionsGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuPasteEvent), false));
			e.Menu.Items.Add(this.CreateMenuItem(Resources.AddSignalRequest, new DiagnosticActionsGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuAddSignalRequest), true));
			e.Menu.Items.Add(this.CreateMenuItem(Resources.AddServiceRequest, new DiagnosticActionsGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuAddAction), false));
			e.Menu.Items[index].Enabled = (null != this.sequenceInClipboard);
		}

		private void DisplayGridRowActionMenu(PopupMenuShowingEventArgs e)
		{
			e.Menu.Items.Clear();
			e.Menu.Items.Add(this.CreateMenuItem(Resources.InsertSignalRequest, new DiagnosticActionsGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuInsertSignalRequest), false));
			e.Menu.Items.Add(this.CreateMenuItem(Resources.InsertServiceRequest, new DiagnosticActionsGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuInsertAction), false));
			int index = e.Menu.Items.Add(this.CreateMenuItem(Resources.RemoveRequest, new DiagnosticActionsGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuRemoveAction), true));
			int index2 = e.Menu.Items.Add(this.CreateMenuItem(Resources.CutRequest, new DiagnosticActionsGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuCutAction), true));
			int index3 = e.Menu.Items.Add(this.CreateMenuItem(Resources.CopyRequest, new DiagnosticActionsGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuCopyAction), false));
			int index4 = e.Menu.Items.Add(this.CreateMenuItem(Resources.PasteRequest, new DiagnosticActionsGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuPasteAction), false));
			e.Menu.Items[index4].Enabled = (this.actionInClipboard != null && !(this.actionInClipboard is DiagnosticDummyAction));
			DiagnosticAction diagnosticAction;
			if (this.TryGetSelectedAction(out diagnosticAction))
			{
				e.Menu.Items[index].Enabled = !(diagnosticAction is DiagnosticDummyAction);
				e.Menu.Items[index2].Enabled = !(diagnosticAction is DiagnosticDummyAction);
				e.Menu.Items[index3].Enabled = !(diagnosticAction is DiagnosticDummyAction);
			}
		}

		private void OnGridRowContextMenuEditEvent(object sender, EventArgs e)
		{
			this.EditActionSequenceEventCondition();
		}

		private void OnGridRowContextMenuRemoveEvent(object sender, EventArgs e)
		{
			this.RemoveActionSequence(true);
		}

		private void OnGridRowContextMenuCopyEvent(object sender, EventArgs e)
		{
			this.CopySequence();
		}

		private void OnGridRowContextMenuPasteEvent(object sender, EventArgs e)
		{
			this.PasteSequence();
		}

		private void OnGridRowContextMenuAddAction(object sender, EventArgs e)
		{
			this.InsertAction(true);
		}

		private void OnGridRowContextMenuAddSignalRequest(object sender, EventArgs e)
		{
			this.InsertSignalRequest(true);
		}

		private void OnGridRowContextMenuRemoveAction(object sender, EventArgs e)
		{
			this.RemoveAction();
		}

		private void OnGridRowContextMenuInsertAction(object sender, EventArgs e)
		{
			this.InsertAction(false);
		}

		private void OnGridRowContextMenuInsertSignalRequest(object sender, EventArgs e)
		{
			this.InsertSignalRequest(false);
		}

		private void OnGridRowContextMenuCutAction(object sender, EventArgs e)
		{
			this.CutAction();
		}

		private void OnGridRowContextMenuCopyAction(object sender, EventArgs e)
		{
			this.CopyAction();
		}

		private void OnGridRowContextMenuPasteAction(object sender, EventArgs e)
		{
			this.PasteAction();
		}

		public void AddActionSequence(TriggeredDiagnosticActionSequence seq)
		{
			this.SaveExpandStates();
			TriggeredDiagnosticActionSequence value;
			if (this.TryGetSelectedActionSequence(out value))
			{
				int insertPos = this.diagnosticActionsConfiguration.TriggeredActionSequences.IndexOf(value) + 1;
				this.diagnosticActionsConfiguration.InsertActionSequence(seq, insertPos);
			}
			else
			{
				this.diagnosticActionsConfiguration.AddActionSequence(seq);
			}
			this.ValidateInput(true);
			this.SelectRowOfActionSequence(seq);
		}

		public void RemoveActionSequence(bool showQuestion = true)
		{
			TriggeredDiagnosticActionSequence seq;
			if (!this.TryGetSelectedActionSequence(out seq))
			{
				return;
			}
			if (showQuestion && InformMessageBox.Question(Resources.QuestionRemoveSelDiagActionSeq) != DialogResult.Yes)
			{
				return;
			}
			this.diagnosticActionsConfiguration.RemoveActionSequence(seq);
			this.ValidateInput(true);
		}

		public void EditActionSequenceEventCondition()
		{
			this.SaveExpandStates();
			TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence;
			if (!this.TryGetSelectedActionSequence(out triggeredDiagnosticActionSequence))
			{
				return;
			}
			if (this.EditEventCondition(triggeredDiagnosticActionSequence.Event))
			{
				this.ValidateInput(true);
			}
		}

		private bool EditEventCondition(Event ev)
		{
			if (ev is OnStartEvent)
			{
				return this.EditOnStartEventCondition(ev as OnStartEvent);
			}
			if (ev is CyclicTimerEvent)
			{
				return this.EditCyclicTimerEventCondition(ev as CyclicTimerEvent);
			}
			if (ev is CANIdEvent)
			{
				return this.EditCANIdEventCondition(ev as CANIdEvent);
			}
			if (ev is LINIdEvent)
			{
				return this.EditLINIdEventCondition(ev as LINIdEvent);
			}
			if (ev is FlexrayIdEvent)
			{
				return this.EditFlexrayIdEventCondition(ev as FlexrayIdEvent);
			}
			if (ev is CANDataEvent)
			{
				return this.EditCANDataEventCondition(ev as CANDataEvent);
			}
			if (ev is LINDataEvent)
			{
				return this.EditLINDataEventCondition(ev as LINDataEvent);
			}
			if (ev is SymbolicMessageEvent)
			{
				return this.EditSymbolicMessageEventCondition(ev as SymbolicMessageEvent);
			}
			if (ev is ISymbolicSignalEvent)
			{
				return this.EditSymbolicSignalEventCondition(ev as ISymbolicSignalEvent);
			}
			if (ev is MsgTimeoutEvent)
			{
				return this.EditMsgTimeoutEventCondition(ev as MsgTimeoutEvent);
			}
			if (ev is DigitalInputEvent)
			{
				return this.EditDigitalInputEventCondition(ev as DigitalInputEvent);
			}
			if (ev is AnalogInputEvent)
			{
				return this.EditAnalogInputEventCondition(ev as AnalogInputEvent);
			}
			if (ev is KeyEvent)
			{
				return this.EditKeyEventCondition(ev as KeyEvent);
			}
			return ev is IgnitionEvent && this.EditIgnitionEventCondition(ev as IgnitionEvent);
		}

		public bool EditOnStartEventCondition(OnStartEvent onStartEvent)
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

		public bool EditCyclicTimerEventCondition(CyclicTimerEvent cyclicTimerEvent)
		{
			if (cyclicTimerEvent == null)
			{
				return false;
			}
			CyclicTimerCondition cyclicTimerConditionDialog = this.GetCyclicTimerConditionDialog();
			cyclicTimerConditionDialog.CyclicTimerEvent = new CyclicTimerEvent(cyclicTimerEvent);
			if (DialogResult.OK == cyclicTimerConditionDialog.ShowDialog() && !cyclicTimerEvent.Equals(cyclicTimerConditionDialog.CyclicTimerEvent))
			{
				cyclicTimerEvent.Assign(cyclicTimerConditionDialog.CyclicTimerEvent);
				return true;
			}
			return false;
		}

		public bool EditCANIdEventCondition(CANIdEvent canIdEvent)
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

		private bool EditLINIdEventCondition(LINIdEvent linIdEvent)
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

		private bool EditFlexrayIdEventCondition(FlexrayIdEvent frIdEvent)
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

		private bool EditCANDataEventCondition(CANDataEvent canDataEvent)
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

		private bool EditLINDataEventCondition(LINDataEvent linDataEvent)
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

		private bool EditSymbolicMessageEventCondition(SymbolicMessageEvent symbolicMsgEvent)
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

		private bool EditSymbolicSignalEventCondition(ISymbolicSignalEvent symbolicSigEvent)
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

		private bool EditMsgTimeoutEventCondition(MsgTimeoutEvent msgTimeoutEvent)
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

		private bool EditKeyEventCondition(KeyEvent keyEvent)
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

		private bool EditDigitalInputEventCondition(DigitalInputEvent digInEvent)
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

		private bool EditAnalogInputEventCondition(AnalogInputEvent analogInEvent)
		{
			if (analogInEvent == null)
			{
				return false;
			}
			AnalogInputCondition analogInputCondition = this.GetAnalogInputConditionDialog();
			analogInputCondition.AnalogInputEvent = new AnalogInputEvent(analogInEvent);
			if (DialogResult.OK == analogInputCondition.ShowDialog() && !analogInEvent.Equals(analogInputCondition.AnalogInputEvent))
			{
				analogInEvent.Assign(analogInputCondition.AnalogInputEvent);
				return true;
			}
			return false;
		}

		private bool EditIgnitionEventCondition(IgnitionEvent ignitionEvent)
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

		private void UnboundColumnDataEvent(DiagnosticAction action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (action.TriggeredDiagnosticActionSequence.Event is OnStartEvent)
				{
					double num = Convert.ToDouble((action.TriggeredDiagnosticActionSequence.Event as OnStartEvent).Delay.Value) / 1000.0;
					e.Value = string.Format(Resources.DiagEvOnStartGroupRow, num.ToString("F1", CultureInfo.InvariantCulture));
					return;
				}
				if (action.TriggeredDiagnosticActionSequence.Event is CyclicTimerEvent)
				{
					CyclicTimerEvent cyclicTimerEvent = action.TriggeredDiagnosticActionSequence.Event as CyclicTimerEvent;
					e.Value = string.Format(Resources.DiagEvCyclicTimerGroupRow, new object[]
					{
						cyclicTimerEvent.Interval.Value,
						GUIUtil.GetTimeUnitString(cyclicTimerEvent.TimeUnit.Value),
						cyclicTimerEvent.DelayCycles.Value * cyclicTimerEvent.Interval.Value,
						GUIUtil.GetTimeUnitString(cyclicTimerEvent.TimeUnit.Value)
					});
					return;
				}
				if (action.TriggeredDiagnosticActionSequence.Event is IdEvent)
				{
					IdEvent idEvent = action.TriggeredDiagnosticActionSequence.Event as IdEvent;
					if (idEvent is CANIdEvent)
					{
						e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(idEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(idEvent));
						return;
					}
					if (idEvent is LINIdEvent)
					{
						e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(idEvent.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(idEvent));
						return;
					}
					if (idEvent is FlexrayIdEvent)
					{
						e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapFlexrayChannelNumber2String(idEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(idEvent));
						return;
					}
				}
				else
				{
					if (action.TriggeredDiagnosticActionSequence.Event is CANDataEvent)
					{
						CANDataEvent cANDataEvent = action.TriggeredDiagnosticActionSequence.Event as CANDataEvent;
						e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(cANDataEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(cANDataEvent));
						return;
					}
					if (action.TriggeredDiagnosticActionSequence.Event is LINDataEvent)
					{
						LINDataEvent lINDataEvent = action.TriggeredDiagnosticActionSequence.Event as LINDataEvent;
						e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(lINDataEvent.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(lINDataEvent));
						return;
					}
					if (action.TriggeredDiagnosticActionSequence.Event is SymbolicMessageEvent)
					{
						SymbolicMessageEvent symbolicMessageEvent = action.TriggeredDiagnosticActionSequence.Event as SymbolicMessageEvent;
						if (symbolicMessageEvent.BusType.Value == BusType.Bt_CAN)
						{
							e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(symbolicMessageEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(symbolicMessageEvent, this.ModelValidator.DatabaseServices));
							return;
						}
						if (symbolicMessageEvent.BusType.Value == BusType.Bt_LIN)
						{
							e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(symbolicMessageEvent.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(symbolicMessageEvent, this.ModelValidator.DatabaseServices));
							return;
						}
						if (symbolicMessageEvent.BusType.Value == BusType.Bt_FlexRay)
						{
							e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapFlexrayChannelNumber2String(symbolicMessageEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(symbolicMessageEvent, this.ModelValidator.DatabaseServices));
							return;
						}
					}
					else if (action.TriggeredDiagnosticActionSequence.Event is SymbolicSignalEvent)
					{
						SymbolicSignalEvent symbolicSignalEvent = action.TriggeredDiagnosticActionSequence.Event as SymbolicSignalEvent;
						if (symbolicSignalEvent.BusType.Value == BusType.Bt_CAN)
						{
							e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(symbolicSignalEvent, this.ModelValidator.DatabaseServices));
							return;
						}
						if (symbolicSignalEvent.BusType.Value == BusType.Bt_LIN)
						{
							e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(symbolicSignalEvent, this.ModelValidator.DatabaseServices));
							return;
						}
						if (symbolicSignalEvent.BusType.Value == BusType.Bt_FlexRay)
						{
							e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapFlexrayChannelNumber2String(symbolicSignalEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(symbolicSignalEvent, this.ModelValidator.DatabaseServices));
							return;
						}
					}
					else
					{
						if (action.TriggeredDiagnosticActionSequence.Event is CcpXcpSignalEvent)
						{
							CcpXcpSignalEvent symSigEvent = action.TriggeredDiagnosticActionSequence.Event as CcpXcpSignalEvent;
							e.Value = GUIUtil.MapEventCondition2String(symSigEvent, this.ModelValidator.DatabaseServices);
							return;
						}
						if (action.TriggeredDiagnosticActionSequence.Event is DiagnosticSignalEvent)
						{
							DiagnosticSignalEvent symSigEvent2 = action.TriggeredDiagnosticActionSequence.Event as DiagnosticSignalEvent;
							e.Value = GUIUtil.MapEventCondition2String(symSigEvent2, this.ModelValidator.DatabaseServices);
							return;
						}
						if (action.TriggeredDiagnosticActionSequence.Event is MsgTimeoutEvent)
						{
							MsgTimeoutEvent msgTimeoutEvent = action.TriggeredDiagnosticActionSequence.Event as MsgTimeoutEvent;
							if (msgTimeoutEvent.BusType.Value == BusType.Bt_CAN)
							{
								e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapCANChannelNumber2String(msgTimeoutEvent.ChannelNumber.Value), GUIUtil.MapEventCondition2String(msgTimeoutEvent, this.ModelValidator.DatabaseServices));
								return;
							}
							if (msgTimeoutEvent.BusType.Value == BusType.Bt_LIN)
							{
								e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapLINChannelNumber2String(msgTimeoutEvent.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics), GUIUtil.MapEventCondition2String(msgTimeoutEvent, this.ModelValidator.DatabaseServices));
								return;
							}
						}
						else
						{
							if (action.TriggeredDiagnosticActionSequence.Event is DigitalInputEvent)
							{
								DigitalInputEvent digitalInputEvent = action.TriggeredDiagnosticActionSequence.Event as DigitalInputEvent;
								e.Value = string.Format(Resources.ChannelAndEventFormat, GUIUtil.MapDigitalInputNumber2String(digitalInputEvent.DigitalInput.Value), GUIUtil.MapEventCondition2String(digitalInputEvent));
								return;
							}
							if (action.TriggeredDiagnosticActionSequence.Event is AnalogInputEvent)
							{
								AnalogInputEvent analogInputEvent = action.TriggeredDiagnosticActionSequence.Event as AnalogInputEvent;
								e.Value = string.Format("{0} {1}", GUIUtil.MapAnalogInputNumber2String(analogInputEvent.InputNumber.Value), GUIUtil.MapEventCondition2String(analogInputEvent));
								return;
							}
							if (action.TriggeredDiagnosticActionSequence.Event is KeyEvent)
							{
								KeyEvent keyEvent = action.TriggeredDiagnosticActionSequence.Event as KeyEvent;
								e.Value = GUIUtil.MapKeyNumber2String(keyEvent.Number.Value, keyEvent.IsOnPanel.Value);
								return;
							}
							if (action.TriggeredDiagnosticActionSequence.Event is IgnitionEvent)
							{
								IgnitionEvent ignitionEvent = action.TriggeredDiagnosticActionSequence.Event as IgnitionEvent;
								e.Value = GUIUtil.MapEventCondition2String(ignitionEvent);
								return;
							}
							e.Value = Resources.Unknown;
						}
					}
				}
			}
		}

		private void UnboundColumnDataEcuName(DiagnosticAction action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (action is DiagnosticDummyAction)
				{
					e.Value = string.Empty;
					return;
				}
				uint num;
				if (this.ModelValidator.HasChannelAssignedForDiagnosticsECU(action.DatabasePath.Value, action.EcuQualifier.Value, out num))
				{
					e.Value = string.Format(Resources.EcuOnChannel, action.EcuQualifier.Value, num);
					return;
				}
				e.Value = action.EcuQualifier.Value;
			}
		}

		private void UnboundColumnDataService(DiagnosticAction action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (action is DiagnosticDummyAction)
				{
					e.Value = string.Empty;
					return;
				}
				e.Value = action.ServiceQualifier.Value;
			}
		}

		private void UnboundColumnDataParameter(DiagnosticAction action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (action is DiagnosticDummyAction)
				{
					e.Value = string.Empty;
					return;
				}
				e.Value = this.disassParamsValuesCell[action];
			}
		}

		private void UnboundColumnRawMessage(DiagnosticAction action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (action is DiagnosticDummyAction)
				{
					e.Value = string.Empty;
					return;
				}
				e.Value = "0x" + GUIUtil.ByteArrayToDisplayStringHex(action.MessageData.Value);
			}
		}

		private void UnboundColumnSession(DiagnosticAction action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (action is DiagnosticDummyAction)
				{
					e.Value = string.Empty;
					return;
				}
				if (action.SessionType.Value == DiagSessionType.DynamicFromDB)
				{
					string variantQualifier;
					if (this.ModelValidator.IsDiagECUConfigured(action.DatabasePath.Value, action.EcuQualifier.Value, out variantQualifier))
					{
						DiagSessionType serviceSessionTypeFromDB = this.DiagSymbolsManager.GetServiceSessionTypeFromDB(action.EcuQualifier.Value, variantQualifier, action.ServiceQualifier.Value, action.MessageData.Value);
						e.Value = GUIUtil.MapDiagSessionType2String(serviceSessionTypeFromDB);
						return;
					}
					e.Value = GUIUtil.MapDiagSessionType2String(DiagSessionType.Default);
					return;
				}
				else
				{
					e.Value = GUIUtil.MapDiagSessionType2String(action.SessionType.Value);
				}
			}
		}

		private void UnboundColumnSignal(DiagnosticAction action, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (!(action is DiagnosticSignalRequest))
				{
					e.Value = string.Empty;
					return;
				}
				e.Value = (action as DiagnosticSignalRequest).SignalQualifier.Value;
			}
		}

		private void toolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
		{
			ToolTipControlInfo info = null;
			try
			{
				GridHitInfo gridHitInfo = this.gridViewDiagnosticActions.CalcHitInfo(e.ControlMousePosition);
				DiagnosticAction diagnosticAction;
				string text;
				if (gridHitInfo.Column == this.colParameter)
				{
					DiagnosticAction key;
					if (this.TryGetAction(this.gridViewDiagnosticActions.GetDataSourceRowIndex(gridHitInfo.RowHandle), out key))
					{
						info = new ToolTipControlInfo(new CellToolTipInfo(gridHitInfo.RowHandle, gridHitInfo.Column, "cell"), this.disassParamsValuesTooltip[key]);
						e.Info = info;
					}
				}
				else if (gridHitInfo.RowHandle < 0 && this.TryGetAction(this.gridViewDiagnosticActions.GetDataSourceRowIndex(gridHitInfo.RowHandle), out diagnosticAction) && this.HasEventGlobalModelErrors(diagnosticAction.TriggeredDiagnosticActionSequence.Event, out text))
				{
					info = new ToolTipControlInfo(new CellToolTipInfo(gridHitInfo.RowHandle, gridHitInfo.Column, "cell"), text);
					e.Info = info;
				}
			}
			catch
			{
				e.Info = info;
			}
		}

		private void repositoryItemButtonEditParams_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			DiagnosticAction diagnosticAction;
			if (!this.TryGetSelectedAction(out diagnosticAction))
			{
				return;
			}
			if (this.EditServiceParameters(ref diagnosticAction))
			{
				this.gridViewDiagnosticActions.RefreshData();
				this.ValidateInput(true);
			}
		}

		private void repositoryItemButtonEditService_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			this.ReplaceAction();
		}

		private void repositoryItemButtonEditSignal_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			this.ReplaceSignal(null);
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
			flag &= this.ModelValidator.Validate(this.diagnosticActionsConfiguration, isDataChanged, this.pageValidator);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			this.RenderParameterDisplayStrings();
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

		private bool HasEventGlobalModelErrors(Event ev, out string errorText)
		{
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
			else if (ev is DiagnosticSignalEvent)
			{
				if (this.pageValidator.General.HasError((ev as DiagnosticSignalEvent).SignalName))
				{
					errorText = this.pageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, (ev as DiagnosticSignalEvent).SignalName);
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

		private void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
		}

		private void StoreMapping4VisibleCells()
		{
			PageValidatorGridUtil.StoreRowMappingForVisibleRows(this.gridViewDiagnosticActions, new PageValidatorGridUtil.StoreRowMappingHandler(this.OnPageValidatorGridUtilStoreRowMapping));
		}

		private void OnPageValidatorGridUtilStoreRowMapping(int dataSourceRowIdx)
		{
			if (dataSourceRowIdx < 0 || dataSourceRowIdx >= this.diagnosticActionsConfiguration.DiagnosticActions.Count)
			{
				return;
			}
			DiagnosticAction action = this.diagnosticActionsConfiguration.DiagnosticActions[dataSourceRowIdx];
			this.StoreMapping4VisibleColumns(action, dataSourceRowIdx);
		}

		private void StoreMapping4VisibleColumns(DiagnosticAction action, int dataSourceIdx)
		{
			if (PageValidatorGridUtil.IsColumnVisible(this.colEvent, this.gridViewDiagnosticActions))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colEvent, dataSourceIdx);
				if (action.TriggeredDiagnosticActionSequence.Event is IdEvent)
				{
					this.pageValidator.Grid.StoreMapping((action.TriggeredDiagnosticActionSequence.Event as IdEvent).ChannelNumber, gUIElement);
				}
				else if (action.TriggeredDiagnosticActionSequence.Event is CANDataEvent)
				{
					this.pageValidator.Grid.StoreMapping((action.TriggeredDiagnosticActionSequence.Event as CANDataEvent).ChannelNumber, gUIElement);
				}
				else if (action.TriggeredDiagnosticActionSequence.Event is LINDataEvent)
				{
					this.pageValidator.Grid.StoreMapping((action.TriggeredDiagnosticActionSequence.Event as LINDataEvent).ChannelNumber, gUIElement);
				}
				else if (action.TriggeredDiagnosticActionSequence.Event is SymbolicMessageEvent)
				{
					SymbolicMessageEvent symbolicMessageEvent = action.TriggeredDiagnosticActionSequence.Event as SymbolicMessageEvent;
					this.pageValidator.Grid.StoreMapping(symbolicMessageEvent.ChannelNumber, gUIElement);
					this.pageValidator.Grid.StoreMapping(symbolicMessageEvent.MessageName, gUIElement);
				}
				else if (action.TriggeredDiagnosticActionSequence.Event is SymbolicSignalEvent)
				{
					SymbolicSignalEvent symbolicSignalEvent = action.TriggeredDiagnosticActionSequence.Event as SymbolicSignalEvent;
					this.pageValidator.Grid.StoreMapping(symbolicSignalEvent.ChannelNumber, gUIElement);
					this.pageValidator.Grid.StoreMapping(symbolicSignalEvent.MessageName, gUIElement);
					this.pageValidator.Grid.StoreMapping(symbolicSignalEvent.SignalName, gUIElement);
				}
				else if (action.TriggeredDiagnosticActionSequence.Event is CcpXcpSignalEvent)
				{
					CcpXcpSignalEvent ccpXcpSignalEvent = action.TriggeredDiagnosticActionSequence.Event as CcpXcpSignalEvent;
					this.pageValidator.Grid.StoreMapping(ccpXcpSignalEvent.SignalName, gUIElement);
				}
				else if (action.TriggeredDiagnosticActionSequence.Event is DiagnosticSignalEvent)
				{
					DiagnosticSignalEvent diagnosticSignalEvent = action.TriggeredDiagnosticActionSequence.Event as DiagnosticSignalEvent;
					this.pageValidator.Grid.StoreMapping(diagnosticSignalEvent.SignalName, gUIElement);
				}
				else if (action.TriggeredDiagnosticActionSequence.Event is MsgTimeoutEvent)
				{
					MsgTimeoutEvent msgTimeoutEvent = action.TriggeredDiagnosticActionSequence.Event as MsgTimeoutEvent;
					this.pageValidator.Grid.StoreMapping(msgTimeoutEvent.ChannelNumber, gUIElement);
					this.pageValidator.Grid.StoreMapping(msgTimeoutEvent.MessageName, gUIElement);
				}
				else if (action.TriggeredDiagnosticActionSequence.Event is DigitalInputEvent)
				{
					this.pageValidator.Grid.StoreMapping((action.TriggeredDiagnosticActionSequence.Event as DigitalInputEvent).DigitalInput, gUIElement);
				}
				else if (action.TriggeredDiagnosticActionSequence.Event is KeyEvent)
				{
					this.pageValidator.Grid.StoreMapping((action.TriggeredDiagnosticActionSequence.Event as KeyEvent).IsOnPanel, gUIElement);
				}
			}
			if (action is DiagnosticSignalRequest && PageValidatorGridUtil.IsColumnVisible(this.colSignal, this.gridViewDiagnosticActions))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colSignal, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping((action as DiagnosticSignalRequest).SignalQualifier, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colService, this.gridViewDiagnosticActions))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colService, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(action.ServiceQualifier, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colEcu, this.gridViewDiagnosticActions))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colEcu, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(action.EcuQualifier, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colParameter, this.gridViewDiagnosticActions))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colParameter, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(action.MessageData, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colRawMessage, this.gridViewDiagnosticActions))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colRawMessage, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(action.MessageData, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colSession, this.gridViewDiagnosticActions))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colSession, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(action.SessionType, gUIElement);
			}
		}

		public bool TryGetSelectedActionSequence(out TriggeredDiagnosticActionSequence seq)
		{
			int num;
			return this.TryGetSelectedActionSequence(out seq, out num);
		}

		private bool TryGetSelectedActionSequence(out TriggeredDiagnosticActionSequence seq, out int groupRowHandle)
		{
			seq = null;
			groupRowHandle = -1;
			if (this.isGroupRowSelected)
			{
				groupRowHandle = this.gridViewDiagnosticActions.FocusedRowHandle;
				if (this.gridViewDiagnosticActions.GetChildRowCount(groupRowHandle) == 0)
				{
					return false;
				}
				int dataSourceRowIndex = this.gridViewDiagnosticActions.GetDataSourceRowIndex(this.gridViewDiagnosticActions.GetChildRowHandle(groupRowHandle, 0));
				DiagnosticAction diagnosticAction;
				if (!this.TryGetAction(dataSourceRowIndex, out diagnosticAction))
				{
					return false;
				}
				seq = diagnosticAction.TriggeredDiagnosticActionSequence;
			}
			else
			{
				int focusedRowHandle = this.gridViewDiagnosticActions.FocusedRowHandle;
				if (!this.gridViewDiagnosticActions.IsValidRowHandle(focusedRowHandle))
				{
					return false;
				}
				groupRowHandle = this.gridViewDiagnosticActions.GetParentRowHandle(focusedRowHandle);
				DiagnosticAction diagnosticAction2 = this.diagnosticActionsConfiguration.DiagnosticActions[this.gridViewDiagnosticActions.GetDataSourceRowIndex(focusedRowHandle)];
				seq = diagnosticAction2.TriggeredDiagnosticActionSequence;
			}
			return null != seq;
		}

		public void SelectRowOfActionSequence(TriggeredDiagnosticActionSequence seq)
		{
			for (int i = 0; i < this.gridViewDiagnosticActions.RowCount; i++)
			{
				IList<DiagnosticAction> list = this.gridViewDiagnosticActions.DataSource as IList<DiagnosticAction>;
				if (list != null)
				{
					TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence = list[this.gridViewDiagnosticActions.GetDataSourceRowIndex(i)].TriggeredDiagnosticActionSequence;
					if (triggeredDiagnosticActionSequence == seq)
					{
						if (this.gridViewDiagnosticActions.IsValidRowHandle(i))
						{
							this.gridViewDiagnosticActions.FocusedRowHandle = this.gridViewDiagnosticActions.GetParentRowHandle(i);
							return;
						}
						break;
					}
				}
			}
		}

		public bool TryGetSelectedAction(out DiagnosticAction action)
		{
			int num;
			return this.TryGetSelectedAction(out action, out num);
		}

		private bool TryGetSelectedAction(out DiagnosticAction action, out int idx)
		{
			action = null;
			idx = this.gridViewDiagnosticActions.GetFocusedDataSourceRowIndex();
			if (idx < 0 || idx > this.diagnosticActionsConfiguration.DiagnosticActions.Count - 1)
			{
				return false;
			}
			action = this.diagnosticActionsConfiguration.DiagnosticActions[idx];
			return null != action;
		}

		private bool TryGetAction(int listSourceRowIndex, out DiagnosticAction action)
		{
			action = null;
			if (listSourceRowIndex < 0 || listSourceRowIndex > this.diagnosticActionsConfiguration.DiagnosticActions.Count - 1)
			{
				return false;
			}
			action = this.diagnosticActionsConfiguration.DiagnosticActions[listSourceRowIndex];
			return null != action;
		}

		public void SelectRowOfAction(DiagnosticAction action)
		{
			for (int i = 0; i < this.gridViewDiagnosticActions.RowCount; i++)
			{
				IList<DiagnosticAction> list = this.gridViewDiagnosticActions.DataSource as IList<DiagnosticAction>;
				if (list != null)
				{
					DiagnosticAction diagnosticAction = list[this.gridViewDiagnosticActions.GetDataSourceRowIndex(i)];
					if (diagnosticAction == action)
					{
						this.gridViewDiagnosticActions.FocusedRowHandle = i;
						return;
					}
				}
			}
		}

		private void UpdateUpDownButtons()
		{
			int focusedRowHandle = this.gridViewDiagnosticActions.FocusedRowHandle;
			if (this.isGroupRowSelected)
			{
				int groupRowCount = this.GetGroupRowCount();
				this.buttonMoveFirst.Enabled = (groupRowCount > 0 && focusedRowHandle < -1);
				this.buttonMoveUp.Enabled = (groupRowCount > 0 && focusedRowHandle < -1);
				this.buttonMoveDown.Enabled = (groupRowCount > 0 && focusedRowHandle > -groupRowCount);
				this.buttonMoveLast.Enabled = (groupRowCount > 0 && focusedRowHandle > -groupRowCount);
				return;
			}
			int parentRowHandle = this.gridViewDiagnosticActions.GetParentRowHandle(focusedRowHandle);
			int childRowCount = this.gridViewDiagnosticActions.GetChildRowCount(parentRowHandle);
			int childRowHandle = this.gridViewDiagnosticActions.GetChildRowHandle(parentRowHandle, 0);
			this.buttonMoveFirst.Enabled = (childRowCount > 1 && focusedRowHandle > childRowHandle);
			this.buttonMoveUp.Enabled = (childRowCount > 1 && focusedRowHandle > childRowHandle);
			this.buttonMoveDown.Enabled = (childRowCount > 1 && focusedRowHandle < childRowHandle + childRowCount - 1);
			this.buttonMoveLast.Enabled = (childRowCount > 1 && focusedRowHandle < childRowHandle + childRowCount - 1);
		}

		private int GetGroupRowCount()
		{
			for (int i = -1; i >= -2147483648; i--)
			{
				if (!this.gridViewDiagnosticActions.IsValidRowHandle(i))
				{
					return -(i + 1);
				}
			}
			return 0;
		}

		private void buttonMoveFirst_Click(object sender, EventArgs e)
		{
			this.SaveExpandStates();
			DiagnosticAction diagnosticAction;
			if (this.isGroupRowSelected)
			{
				TriggeredDiagnosticActionSequence seq;
				if (this.TryGetSelectedActionSequence(out seq))
				{
					this.diagnosticActionsConfiguration.RemoveActionSequence(seq);
					this.diagnosticActionsConfiguration.InsertActionSequence(seq, 0);
					this.gridViewDiagnosticActions.FocusedRowHandle = -1;
				}
			}
			else if (this.TryGetSelectedAction(out diagnosticAction))
			{
				TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence = diagnosticAction.TriggeredDiagnosticActionSequence;
				triggeredDiagnosticActionSequence.RemoveAction(diagnosticAction);
				triggeredDiagnosticActionSequence.InsertAction(diagnosticAction, 0);
				int focusedRowHandle = this.gridViewDiagnosticActions.FocusedRowHandle;
				int parentRowHandle = this.gridViewDiagnosticActions.GetParentRowHandle(focusedRowHandle);
				this.gridViewDiagnosticActions.FocusedRowHandle = this.gridViewDiagnosticActions.GetChildRowHandle(parentRowHandle, 0);
			}
			this.ValidateInput(true);
		}

		private void buttonMoveUp_Click(object sender, EventArgs e)
		{
			this.SaveExpandStates();
			DiagnosticAction diagnosticAction;
			if (this.isGroupRowSelected)
			{
				TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence;
				if (this.TryGetSelectedActionSequence(out triggeredDiagnosticActionSequence))
				{
					int num = this.diagnosticActionsConfiguration.TriggeredActionSequences.IndexOf(triggeredDiagnosticActionSequence);
					this.diagnosticActionsConfiguration.RemoveActionSequence(triggeredDiagnosticActionSequence);
					this.diagnosticActionsConfiguration.InsertActionSequence(triggeredDiagnosticActionSequence, num - 1);
					this.gridViewDiagnosticActions.FocusedRowHandle++;
				}
			}
			else if (this.TryGetSelectedAction(out diagnosticAction))
			{
				TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence2 = diagnosticAction.TriggeredDiagnosticActionSequence;
				int num2 = triggeredDiagnosticActionSequence2.Actions.IndexOf(diagnosticAction);
				triggeredDiagnosticActionSequence2.RemoveAction(diagnosticAction);
				triggeredDiagnosticActionSequence2.InsertAction(diagnosticAction, num2 - 1);
				this.gridViewDiagnosticActions.FocusedRowHandle--;
			}
			this.ValidateInput(true);
		}

		private void buttonMoveDown_Click(object sender, EventArgs e)
		{
			this.SaveExpandStates();
			DiagnosticAction diagnosticAction;
			if (this.isGroupRowSelected)
			{
				TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence;
				if (this.TryGetSelectedActionSequence(out triggeredDiagnosticActionSequence))
				{
					int num = this.diagnosticActionsConfiguration.TriggeredActionSequences.IndexOf(triggeredDiagnosticActionSequence);
					this.diagnosticActionsConfiguration.RemoveActionSequence(triggeredDiagnosticActionSequence);
					this.diagnosticActionsConfiguration.InsertActionSequence(triggeredDiagnosticActionSequence, num + 1);
					this.gridViewDiagnosticActions.FocusedRowHandle--;
				}
			}
			else if (this.TryGetSelectedAction(out diagnosticAction))
			{
				TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence2 = diagnosticAction.TriggeredDiagnosticActionSequence;
				int num2 = triggeredDiagnosticActionSequence2.Actions.IndexOf(diagnosticAction);
				triggeredDiagnosticActionSequence2.RemoveAction(diagnosticAction);
				triggeredDiagnosticActionSequence2.InsertAction(diagnosticAction, num2 + 1);
				this.gridViewDiagnosticActions.FocusedRowHandle++;
			}
			this.ValidateInput(true);
		}

		private void buttonMoveLast_Click(object sender, EventArgs e)
		{
			this.SaveExpandStates();
			DiagnosticAction diagnosticAction;
			if (this.isGroupRowSelected)
			{
				TriggeredDiagnosticActionSequence seq;
				if (this.TryGetSelectedActionSequence(out seq))
				{
					this.diagnosticActionsConfiguration.RemoveActionSequence(seq);
					this.diagnosticActionsConfiguration.AddActionSequence(seq);
					this.gridViewDiagnosticActions.FocusedRowHandle = -this.GetGroupRowCount();
				}
			}
			else if (this.TryGetSelectedAction(out diagnosticAction))
			{
				TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence = diagnosticAction.TriggeredDiagnosticActionSequence;
				triggeredDiagnosticActionSequence.RemoveAction(diagnosticAction);
				triggeredDiagnosticActionSequence.AddAction(diagnosticAction);
				int focusedRowHandle = this.gridViewDiagnosticActions.FocusedRowHandle;
				int parentRowHandle = this.gridViewDiagnosticActions.GetParentRowHandle(focusedRowHandle);
				int childRowHandle = this.gridViewDiagnosticActions.GetChildRowHandle(parentRowHandle, 0);
				this.gridViewDiagnosticActions.FocusedRowHandle = childRowHandle + this.gridViewDiagnosticActions.GetChildRowCount(parentRowHandle) - 1;
			}
			this.ValidateInput(true);
		}

		private void SaveExpandStates()
		{
			this.expandStates.Clear();
			for (int i = -1; i >= -2147483648; i--)
			{
				if (!this.gridViewDiagnosticActions.IsValidRowHandle(i))
				{
					return;
				}
				int childRowHandle = this.gridViewDiagnosticActions.GetChildRowHandle(i, 0);
				if (this.gridViewDiagnosticActions.IsValidRowHandle(childRowHandle))
				{
					int dataSourceRowIndex = this.gridViewDiagnosticActions.GetDataSourceRowIndex(childRowHandle);
					DiagnosticAction diagnosticAction = this.diagnosticActionsConfiguration.DiagnosticActions[dataSourceRowIndex];
					this.expandStates.Add(diagnosticAction.TriggeredDiagnosticActionSequence, this.gridViewDiagnosticActions.GetRowExpanded(i));
				}
			}
		}

		private void SetExpandStates()
		{
			for (int i = -1; i >= -2147483648; i--)
			{
				if (!this.gridViewDiagnosticActions.IsValidRowHandle(i))
				{
					return;
				}
				int childRowHandle = this.gridViewDiagnosticActions.GetChildRowHandle(i, 0);
				if (this.gridViewDiagnosticActions.IsValidRowHandle(childRowHandle))
				{
					int dataSourceRowIndex = this.gridViewDiagnosticActions.GetDataSourceRowIndex(childRowHandle);
					DiagnosticAction diagnosticAction = this.diagnosticActionsConfiguration.DiagnosticActions[dataSourceRowIndex];
					if (this.expandStates.ContainsKey(diagnosticAction.TriggeredDiagnosticActionSequence))
					{
						this.gridViewDiagnosticActions.SetRowExpanded(i, this.expandStates[diagnosticAction.TriggeredDiagnosticActionSequence]);
					}
					else
					{
						this.gridViewDiagnosticActions.SetRowExpanded(i, true);
					}
				}
			}
		}

		private void RenderParameterDisplayStrings()
		{
			this.disassParamsValuesCell.Clear();
			this.disassParamsValuesTooltip.Clear();
			foreach (DiagnosticAction current in from da in this.diagnosticActionsConfiguration.DiagnosticActions
			where !(da is DiagnosticDummyAction)
			select da)
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				bool flag = false;
				string variantQualifier;
				IList<KeyValuePair<string, string>> list;
				if (this.ModelValidator.IsDiagECUConfigured(current.DatabasePath.Value, current.EcuQualifier.Value, out variantQualifier) && this.DiagSymbolsManager.GetDisassembledMessageParams(this.ModelValidator.GetAbsoluteFilePath(current.DatabasePath.Value), current.EcuQualifier.Value, variantQualifier, current.ServiceQualifier.Value, current.MessageData.Value, out list))
				{
					if (current.HasOnlyConstParams)
					{
						stringBuilder2.Append(Resources.ConstantParams);
					}
					int num = 0;
					foreach (KeyValuePair<string, string> current2 in list)
					{
						if (num != 0)
						{
							if (num > 1)
							{
								stringBuilder.Append("; ");
								stringBuilder2.Append("\n");
							}
							stringBuilder.Append(current2.Value);
							stringBuilder2.AppendFormat("{0} = {1}", current2.Key, current2.Value);
						}
						num++;
					}
					this.disassParamsValuesCell.Add(current, stringBuilder.ToString());
					this.disassParamsValuesTooltip.Add(current, stringBuilder2.ToString());
					flag = true;
				}
				if (!flag)
				{
					this.disassParamsValuesCell.Add(current, "0x" + GUIUtil.ByteArrayToDisplayStringHex(current.MessageData.Value));
					this.disassParamsValuesTooltip.Add(current, "0x" + GUIUtil.ByteArrayToDisplayStringHex(current.MessageData.Value));
				}
			}
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

		public bool Serialize(DiagnosticActionsPage actionsPage)
		{
			if (actionsPage == null)
			{
				return false;
			}
			this.actionsPageLayout = actionsPage;
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.gridViewDiagnosticActions.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				actionsPage.ActionsGridLayout = Convert.ToBase64String(array, 0, array.Length);
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

		public bool DeSerialize(DiagnosticActionsPage actionsPage)
		{
			if (actionsPage == null)
			{
				return false;
			}
			this.actionsPageLayout = actionsPage;
			if (string.IsNullOrEmpty(actionsPage.ActionsGridLayout))
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(actionsPage.ActionsGridLayout);
				memoryStream = new MemoryStream(buffer);
				this.gridViewDiagnosticActions.RestoreLayoutFromStream(memoryStream);
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

		public void InsertSignalRequest(bool insertAtEnd)
		{
			if (!this.ModelValidator.HasDiagnosticsDatabasesConfigured())
			{
				InformMessageBox.Error(Resources.ErrorUnableToAddSigReq);
				return;
			}
			using (DiagnosticSignalSelection diagnosticSignalSelection = new DiagnosticSignalSelection(this.DiagSymbolsManager, this.DiagnosticDatabaseConfiguration, this.actionsPageLayout, new DiagnosticSignalSelection.FocusedSignalApplied(this.InsertSignalRequestFromApplyButton), insertAtEnd))
			{
				diagnosticSignalSelection.ShowDialog();
				if (diagnosticSignalSelection.DialogResult == DialogResult.OK)
				{
					this.InsertSignalRequest(insertAtEnd, diagnosticSignalSelection.FocusedSignal);
				}
			}
		}

		public void InsertSignalRequest(bool insertAtEnd, DiagnosticSignalRequest signal)
		{
			if (signal != null)
			{
				TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence;
				if (!this.TryGetSelectedActionSequence(out triggeredDiagnosticActionSequence))
				{
					return;
				}
				int num = -1;
				DiagnosticAction diagnosticAction;
				if (this.isGroupRowSelected || insertAtEnd)
				{
					triggeredDiagnosticActionSequence.AddAction(signal);
				}
				else if (this.TryGetSelectedAction(out diagnosticAction))
				{
					if (num < 0)
					{
						num = diagnosticAction.TriggeredDiagnosticActionSequence.Actions.IndexOf(diagnosticAction);
					}
					else
					{
						num++;
					}
					triggeredDiagnosticActionSequence.InsertAction(signal, num);
				}
				else
				{
					triggeredDiagnosticActionSequence.AddAction(signal);
				}
				this.ValidateInput(true);
				this.SelectRowOfAction(signal);
			}
		}

		private void InsertSignalRequestFromApplyButton(DiagnosticSignalRequest signal, bool insertAtEnd)
		{
			this.InsertSignalRequest(insertAtEnd, signal);
		}

		public bool ReplaceSignal(DiagnosticAction selectedAction = null)
		{
			if (!this.ModelValidator.HasDiagnosticsDatabasesConfigured())
			{
				InformMessageBox.Error(Resources.ErrorUnableToAddSigReq);
				return false;
			}
			this.SaveExpandStates();
			if ((selectedAction != null || this.TryGetSelectedAction(out selectedAction)) && selectedAction is DiagnosticSignalRequest)
			{
				using (DiagnosticSignalSelection diagnosticSignalSelection = new DiagnosticSignalSelection(this.DiagSymbolsManager, this.DiagnosticDatabaseConfiguration, this.actionsPageLayout, selectedAction as DiagnosticSignalRequest, null))
				{
					diagnosticSignalSelection.ShowDialog();
					if (diagnosticSignalSelection.DialogResult == DialogResult.OK && diagnosticSignalSelection.FocusedSignal != null)
					{
						int insertPos = selectedAction.TriggeredDiagnosticActionSequence.Actions.IndexOf(selectedAction);
						TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence = selectedAction.TriggeredDiagnosticActionSequence;
						triggeredDiagnosticActionSequence.RemoveAction(selectedAction);
						triggeredDiagnosticActionSequence.InsertAction(diagnosticSignalSelection.FocusedSignal, insertPos);
						this.ValidateInput(true);
						return true;
					}
				}
				return false;
			}
			return false;
		}

		public void InsertAction(bool insertAtEnd)
		{
			if (!this.ModelValidator.HasDiagnosticsDatabasesConfigured())
			{
				InformMessageBox.Error(Resources.ErrorUnableToAddServReq);
				return;
			}
			this.SaveExpandStates();
			TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence;
			if (!this.TryGetSelectedActionSequence(out triggeredDiagnosticActionSequence))
			{
				return;
			}
			Dictionary<DiagnosticAction, string> dictionary;
			if (!this.SelectDiagnosticServices(true, out dictionary))
			{
				return;
			}
			int num = -1;
			DiagnosticAction diagnosticAction = null;
			foreach (DiagnosticAction current in dictionary.Keys)
			{
				byte[] value = current.MessageData.Value;
				DiagSessionType value2 = DiagSessionType.Unknown;
				if (current.HasOnlyConstParams || dictionary.Count > 1)
				{
					if (!this.DiagSymbolsManager.InitDefaultServiceParameter(current.EcuQualifier.Value, dictionary[current], current.ServiceQualifier.Value, out value, out value2))
					{
						continue;
					}
				}
				else if (!this.DiagSymbolsManager.EditServiceParameter(current.EcuQualifier.Value, dictionary[current], current.ServiceQualifier.Value, ref value, ref value2))
				{
					continue;
				}
				current.MessageData.Value = value;
				current.SessionType.Value = value2;
				DiagnosticAction diagnosticAction2;
				if (this.isGroupRowSelected || insertAtEnd)
				{
					triggeredDiagnosticActionSequence.AddAction(current);
				}
				else if (this.TryGetSelectedAction(out diagnosticAction2))
				{
					if (num < 0)
					{
						num = diagnosticAction2.TriggeredDiagnosticActionSequence.Actions.IndexOf(diagnosticAction2);
					}
					else
					{
						num++;
					}
					triggeredDiagnosticActionSequence.InsertAction(current, num);
				}
				else
				{
					triggeredDiagnosticActionSequence.AddAction(current);
				}
				diagnosticAction = current;
			}
			if (diagnosticAction != null)
			{
				this.ValidateInput(true);
				this.SelectRowOfAction(diagnosticAction);
			}
		}

		public void RemoveAction()
		{
			this.SaveExpandStates();
			DiagnosticAction diagnosticAction;
			if (!this.TryGetSelectedAction(out diagnosticAction))
			{
				return;
			}
			if (diagnosticAction.TriggeredDiagnosticActionSequence.Actions.Count == 1)
			{
				if (DialogResult.Yes == InformMessageBox.Question(Resources.RemovalOfServiceRemovesEvent))
				{
					this.RemoveActionSequence(false);
				}
				return;
			}
			diagnosticAction.TriggeredDiagnosticActionSequence.RemoveAction(diagnosticAction);
			this.ValidateInput(true);
		}

		public void ReplaceAction()
		{
			if (!this.ModelValidator.HasDiagnosticsDatabasesConfigured())
			{
				InformMessageBox.Error(Resources.ErrorUnableToAddServReq);
				return;
			}
			this.SaveExpandStates();
			DiagnosticAction diagnosticAction;
			Dictionary<DiagnosticAction, string> dictionary;
			if (this.TryGetSelectedAction(out diagnosticAction) && this.SelectDiagnosticServices(false, out dictionary))
			{
				DiagnosticAction diagnosticAction2 = dictionary.Keys.First<DiagnosticAction>();
				byte[] value = null;
				DiagSessionType value2 = DiagSessionType.Unknown;
				if (diagnosticAction2.HasOnlyConstParams)
				{
					if (!this.DiagSymbolsManager.InitDefaultServiceParameter(diagnosticAction2.EcuQualifier.Value, dictionary[diagnosticAction2], diagnosticAction2.ServiceQualifier.Value, out value, out value2))
					{
						return;
					}
				}
				else if (!this.DiagSymbolsManager.EditServiceParameter(diagnosticAction2.EcuQualifier.Value, dictionary[diagnosticAction2], diagnosticAction2.ServiceQualifier.Value, ref value, ref value2))
				{
					return;
				}
				diagnosticAction2.MessageData.Value = value;
				diagnosticAction2.SessionType.Value = value2;
				int insertPos = diagnosticAction.TriggeredDiagnosticActionSequence.Actions.IndexOf(diagnosticAction);
				TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence = diagnosticAction.TriggeredDiagnosticActionSequence;
				triggeredDiagnosticActionSequence.RemoveAction(diagnosticAction);
				triggeredDiagnosticActionSequence.InsertAction(diagnosticAction2, insertPos);
				this.ValidateInput(true);
			}
		}

		public void CutAction()
		{
			this.SaveExpandStates();
			DiagnosticAction diagnosticAction;
			if (!this.TryGetSelectedAction(out diagnosticAction) || diagnosticAction is DiagnosticDummyAction)
			{
				return;
			}
			this.actionInClipboard = diagnosticAction;
			if (diagnosticAction.TriggeredDiagnosticActionSequence.Actions.Count == 1)
			{
				if (DialogResult.Yes != InformMessageBox.Question(Resources.RemovalOfServiceRemovesEvent))
				{
					this.actionInClipboard = null;
					return;
				}
				this.RemoveActionSequence(true);
			}
			else
			{
				diagnosticAction.TriggeredDiagnosticActionSequence.RemoveAction(diagnosticAction);
			}
			this.sequenceInClipboard = null;
			this.ValidateInput(true);
		}

		public void CopyAction()
		{
			DiagnosticAction diagnosticAction;
			if (!this.TryGetSelectedAction(out diagnosticAction) || diagnosticAction is DiagnosticDummyAction)
			{
				return;
			}
			if (diagnosticAction is DiagnosticSignalRequest)
			{
				this.actionInClipboard = new DiagnosticSignalRequest(diagnosticAction as DiagnosticSignalRequest);
			}
			else
			{
				this.actionInClipboard = new DiagnosticAction(diagnosticAction);
			}
			this.sequenceInClipboard = null;
		}

		public void PasteAction()
		{
			if (this.actionInClipboard != null)
			{
				DiagnosticAction action;
				if (this.actionInClipboard is DiagnosticSignalRequest)
				{
					action = new DiagnosticSignalRequest(this.actionInClipboard as DiagnosticSignalRequest);
				}
				else
				{
					action = new DiagnosticAction(this.actionInClipboard);
				}
				TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence;
				if (!this.TryGetSelectedActionSequence(out triggeredDiagnosticActionSequence))
				{
					return;
				}
				DiagnosticAction diagnosticAction;
				if (this.isGroupRowSelected)
				{
					triggeredDiagnosticActionSequence.AddAction(action);
				}
				else if (this.TryGetSelectedAction(out diagnosticAction))
				{
					int insertPos = diagnosticAction.TriggeredDiagnosticActionSequence.Actions.IndexOf(diagnosticAction);
					triggeredDiagnosticActionSequence.InsertAction(action, insertPos);
				}
				else
				{
					triggeredDiagnosticActionSequence.AddAction(action);
				}
				this.ValidateInput(true);
				this.SelectRowOfAction(action);
			}
		}

		public void CopySequence()
		{
			TriggeredDiagnosticActionSequence other;
			if (!this.TryGetSelectedActionSequence(out other))
			{
				return;
			}
			this.sequenceInClipboard = new TriggeredDiagnosticActionSequence(other);
			this.actionInClipboard = null;
		}

		public void PasteSequence()
		{
			if (this.sequenceInClipboard != null && this.isGroupRowSelected)
			{
				TriggeredDiagnosticActionSequence seq = new TriggeredDiagnosticActionSequence(this.sequenceInClipboard);
				int focusedRowHandle = this.gridViewDiagnosticActions.FocusedRowHandle;
				this.diagnosticActionsConfiguration.InsertActionSequence(seq, -focusedRowHandle - 1);
				this.ValidateInput(true);
				this.gridViewDiagnosticActions.FocusedRowHandle = focusedRowHandle;
			}
		}

		public bool SelectDiagnosticServices(bool isMultiSelectEnabled, out Dictionary<DiagnosticAction, string> actionsWithEcuVariantName)
		{
			actionsWithEcuVariantName = new Dictionary<DiagnosticAction, string>();
			while (actionsWithEcuVariantName.Count == 0)
			{
				if (this.isSymSelOpened)
				{
					return false;
				}
				this.isSymSelOpened = true;
				List<DiagnosticAction> list;
				if (!this.DiagSymbolsManager.SelectDiagnosticServiceAction(isMultiSelectEnabled, out list))
				{
					this.isSymSelOpened = false;
					return false;
				}
				this.isSymSelOpened = false;
				foreach (DiagnosticAction current in list)
				{
					current.DatabasePath.Value = this.ModelValidator.GetFilePathRelativeToConfiguration(current.DatabasePath.Value);
					string text;
					if (this.ModelValidator.IsDiagECUConfigured(current.DatabasePath.Value, current.EcuQualifier.Value, out text))
					{
						bool hasOnlyConstParams;
						DSMResult serviceComplexityStatus = this.DiagSymbolsManager.GetServiceComplexityStatus(current.EcuQualifier.Value, text, current.ServiceQualifier.Value, out hasOnlyConstParams);
						DSMResult dSMResult = serviceComplexityStatus;
						if (dSMResult != DSMResult.OK)
						{
							switch (dSMResult)
							{
							case DSMResult.ServiceTooLong:
								InformMessageBox.Error(string.Format(Resources.ErrorServiceTooLong, current.ServiceQualifier.Value, current.EcuQualifier.Value));
								break;
							case DSMResult.ServiceTooComplexIterator:
								InformMessageBox.Error(string.Format(Resources.ErrorServiceTooComplexIter, current.ServiceQualifier.Value, current.EcuQualifier.Value));
								break;
							case DSMResult.ServiceTooComplexMultiplexor:
								InformMessageBox.Error(string.Format(Resources.ErrorServiceTooComplexMulti, current.ServiceQualifier.Value, current.EcuQualifier.Value));
								break;
							default:
								InformMessageBox.Error(string.Format(Resources.ErrorServiceNotSupported, current.ServiceQualifier.Value, current.EcuQualifier.Value));
								break;
							}
						}
						else
						{
							current.HasOnlyConstParams = hasOnlyConstParams;
							actionsWithEcuVariantName.Add(current, text);
						}
					}
				}
			}
			return actionsWithEcuVariantName.Count > 0;
		}

		private bool EditServiceParameters(ref DiagnosticAction action)
		{
			if (action is DiagnosticSignalRequest)
			{
				string variantQualifier;
				if (this.pageValidator.General.HasError((action as DiagnosticSignalRequest).SignalQualifier) || !this.ModelValidator.IsDiagECUConfigured(action.DatabasePath.Value, action.EcuQualifier.Value, out variantQualifier))
				{
					InformMessageBox.Error(Resources.ErrorUnableToResolveSignal);
					return false;
				}
				return this.ReplaceSignal(action);
			}
			else
			{
				string variantQualifier;
				if (!this.ModelValidator.IsDiagECUConfigured(action.DatabasePath.Value, action.EcuQualifier.Value, out variantQualifier))
				{
					InformMessageBox.Error(Resources.ErrorUnableToResolveService);
					return false;
				}
				byte[] value = action.MessageData.Value;
				DiagSessionType value2 = action.SessionType.Value;
				if (!this.DiagSymbolsManager.EditServiceParameter(action.EcuQualifier.Value, variantQualifier, action.ServiceQualifier.Value, ref value, ref value2))
				{
					return false;
				}
				action.MessageData.Value = value;
				action.SessionType.Value = value2;
				return true;
			}
		}

		private void gridViewDiagnosticActions_MouseDown(object sender, MouseEventArgs e)
		{
			this.OnGridMouseDown(e.X, e.Y, e.Button);
		}

		private void gridViewDiagnosticActions_MouseMove(object sender, MouseEventArgs e)
		{
			this.OnGridMouseMove(e.X, e.Y, e.Button);
		}

		private void repositoryItemButtonEditParams_MouseDown(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlDiagnosticActions.PointToClient(Control.MousePosition);
			this.OnGridMouseDown(point.X, point.Y, e.Button);
		}

		private void repositoryItemButtonEditParams_MouseMove(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlDiagnosticActions.PointToClient(Control.MousePosition);
			this.OnGridMouseMove(point.X, point.Y, e.Button);
		}

		private void repositoryItemButtonEditService_MouseDown(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlDiagnosticActions.PointToClient(Control.MousePosition);
			this.OnGridMouseDown(point.X, point.Y, e.Button);
		}

		private void repositoryItemButtonEditService_MouseMove(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlDiagnosticActions.PointToClient(Control.MousePosition);
			this.OnGridMouseMove(point.X, point.Y, e.Button);
		}

		private void repositoryItemButtonEditSignal_MouseDown(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlDiagnosticActions.PointToClient(Control.MousePosition);
			this.OnGridMouseDown(point.X, point.Y, e.Button);
		}

		private void repositoryItemButtonEditSignal_MouseMove(object sender, MouseEventArgs e)
		{
			Point point = this.gridControlDiagnosticActions.PointToClient(Control.MousePosition);
			this.OnGridMouseMove(point.X, point.Y, e.Button);
		}

		private void OnGridMouseDown(int x, int y, MouseButtons mouseButtons)
		{
			this.downHitInfo = null;
			GridHitInfo gridHitInfo = this.gridViewDiagnosticActions.CalcHitInfo(new Point(x, y));
			if (Control.ModifierKeys != Keys.None)
			{
				return;
			}
			if (mouseButtons == MouseButtons.Left && gridHitInfo.InRow && gridHitInfo.RowHandle != -2147483647)
			{
				this.downHitInfo = gridHitInfo;
			}
		}

		private void OnGridMouseMove(int x, int y, MouseButtons mouseButtons)
		{
			if (mouseButtons == MouseButtons.Left && this.downHitInfo != null)
			{
				Size dragSize = SystemInformation.DragSize;
				Rectangle rectangle = new Rectangle(new Point(this.downHitInfo.HitPoint.X - dragSize.Width / 2, this.downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);
				if (!rectangle.Contains(new Point(x, y)))
				{
					this.gridControlDiagnosticActions.DoDragDrop(this.downHitInfo, DragDropEffects.All);
					this.downHitInfo = null;
				}
			}
		}

		private void gridControlDiagnosticActions_DragOver(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(typeof(GridHitInfo)))
			{
				if (!(e.Data.GetData(typeof(GridHitInfo)) is GridHitInfo))
				{
					return;
				}
				GridHitInfo gridHitInfo = this.gridViewDiagnosticActions.CalcHitInfo(this.gridControlDiagnosticActions.PointToClient(new Point(e.X, e.Y)));
				DiagnosticAction diagnosticAction;
				if (this.TryGetAction(this.gridViewDiagnosticActions.GetDataSourceRowIndex(this.downHitInfo.RowHandle), out diagnosticAction) && diagnosticAction is DiagnosticDummyAction && this.gridViewDiagnosticActions.IsDataRow(this.downHitInfo.RowHandle))
				{
					e.Effect = DragDropEffects.None;
					return;
				}
				if (gridHitInfo.InRow && gridHitInfo.RowHandle != this.downHitInfo.RowHandle && gridHitInfo.RowHandle != -2147483647)
				{
					if ((!this.gridViewDiagnosticActions.IsDataRow(this.downHitInfo.RowHandle) && this.gridViewDiagnosticActions.IsDataRow(gridHitInfo.RowHandle) && this.gridViewDiagnosticActions.GetParentRowHandle(gridHitInfo.RowHandle) == this.downHitInfo.RowHandle) || (this.gridViewDiagnosticActions.IsDataRow(this.downHitInfo.RowHandle) && !this.gridViewDiagnosticActions.IsDataRow(gridHitInfo.RowHandle) && this.gridViewDiagnosticActions.GetParentRowHandle(this.downHitInfo.RowHandle) == gridHitInfo.RowHandle))
					{
						e.Effect = DragDropEffects.None;
						return;
					}
					e.Effect = DragDropEffects.Move;
					return;
				}
				else
				{
					e.Effect = DragDropEffects.None;
				}
			}
		}

		private void gridControlDiagnosticActions_DragDrop(object sender, DragEventArgs e)
		{
			GridHitInfo gridHitInfo = e.Data.GetData(typeof(GridHitInfo)) as GridHitInfo;
			GridHitInfo gridHitInfo2 = this.gridViewDiagnosticActions.CalcHitInfo(this.gridControlDiagnosticActions.PointToClient(new Point(e.X, e.Y)));
			if (gridHitInfo != null)
			{
				int rowHandle = gridHitInfo.RowHandle;
				int rowHandle2 = gridHitInfo2.RowHandle;
				this.MoveRow(rowHandle, rowHandle2);
			}
		}

		private void MoveRow(int sourceRow, int targetRow)
		{
			if (sourceRow == targetRow)
			{
				return;
			}
			if (!this.gridViewDiagnosticActions.IsGroupRow(sourceRow))
			{
				this.SaveExpandStates();
				DiagnosticAction diagnosticAction;
				if (!this.TryGetAction(this.gridViewDiagnosticActions.GetDataSourceRowIndex(sourceRow), out diagnosticAction))
				{
					return;
				}
				if (diagnosticAction.TriggeredDiagnosticActionSequence.Actions.Count == 1)
				{
					if (DialogResult.Yes != InformMessageBox.Question(Resources.RemovalOfServiceRemovesEvent))
					{
						return;
					}
					int parentRowHandle = this.gridViewDiagnosticActions.GetParentRowHandle(targetRow);
					int visibleIndex = this.gridViewDiagnosticActions.GetVisibleIndex(parentRowHandle);
					this.diagnosticActionsConfiguration.RemoveActionSequence(diagnosticAction.TriggeredDiagnosticActionSequence);
					if (targetRow < 0 && visibleIndex > targetRow)
					{
						targetRow++;
					}
				}
				else
				{
					diagnosticAction.TriggeredDiagnosticActionSequence.RemoveAction(diagnosticAction);
				}
				if (this.gridViewDiagnosticActions.IsGroupRow(targetRow))
				{
					int num = this.gridViewDiagnosticActions.GetDataSourceRowIndex(this.gridViewDiagnosticActions.GetChildRowHandle(targetRow, 0));
					if (sourceRow < num)
					{
						num--;
					}
					DiagnosticAction diagnosticAction2;
					if (!this.TryGetAction(num, out diagnosticAction2))
					{
						return;
					}
					TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence = diagnosticAction2.TriggeredDiagnosticActionSequence;
					triggeredDiagnosticActionSequence.AddAction(diagnosticAction);
				}
				else
				{
					if (sourceRow < targetRow)
					{
						targetRow--;
					}
					DiagnosticAction diagnosticAction3;
					if (this.TryGetAction(targetRow, out diagnosticAction3))
					{
						int insertPos = diagnosticAction3.TriggeredDiagnosticActionSequence.Actions.IndexOf(diagnosticAction3);
						diagnosticAction3.TriggeredDiagnosticActionSequence.InsertAction(diagnosticAction, insertPos);
					}
				}
				this.ValidateInput(true);
				this.SelectRowOfAction(diagnosticAction);
				return;
			}
			else
			{
				DiagnosticAction diagnosticAction4;
				if (!this.TryGetAction(this.gridViewDiagnosticActions.GetDataSourceRowIndex(sourceRow), out diagnosticAction4))
				{
					return;
				}
				TriggeredDiagnosticActionSequence triggeredDiagnosticActionSequence2 = diagnosticAction4.TriggeredDiagnosticActionSequence;
				if (this.gridViewDiagnosticActions.IsDataRow(targetRow))
				{
					targetRow = this.gridViewDiagnosticActions.GetParentRowHandle(targetRow);
				}
				this.diagnosticActionsConfiguration.RemoveActionSequence(triggeredDiagnosticActionSequence2);
				if (Math.Abs(sourceRow) < Math.Abs(targetRow))
				{
					targetRow++;
				}
				this.diagnosticActionsConfiguration.InsertActionSequence(triggeredDiagnosticActionSequence2, -1 - targetRow);
				this.ValidateInput(true);
				this.gridViewDiagnosticActions.FocusedRowHandle = targetRow;
				return;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DiagnosticActionsGrid));
			this.gridControlDiagnosticActions = new GridControl();
			this.diagActionsBindingSource = new BindingSource(this.components);
			this.gridViewDiagnosticActions = new GridView();
			this.colEvent = new GridColumn();
			this.colSignal = new GridColumn();
			this.repositoryItemButtonEditSignal = new RepositoryItemButtonEdit();
			this.colService = new GridColumn();
			this.repositoryItemButtonEditService = new RepositoryItemButtonEdit();
			this.colEcu = new GridColumn();
			this.colParameter = new GridColumn();
			this.repositoryItemButtonEditParams = new RepositoryItemButtonEdit();
			this.colRawMessage = new GridColumn();
			this.colSession = new GridColumn();
			this.repositoryItemTextEditDummy = new RepositoryItemTextEdit();
			this.toolTipController = new XtraToolTipController(this.components);
			this.buttonMoveFirst = new Button();
			this.buttonMoveUp = new Button();
			this.buttonMoveDown = new Button();
			this.buttonMoveLast = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.gridControlDiagnosticActions).BeginInit();
			((ISupportInitialize)this.diagActionsBindingSource).BeginInit();
			((ISupportInitialize)this.gridViewDiagnosticActions).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEditSignal).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEditService).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEditParams).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).BeginInit();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.gridControlDiagnosticActions.AllowDrop = true;
			componentResourceManager.ApplyResources(this.gridControlDiagnosticActions, "gridControlDiagnosticActions");
			this.gridControlDiagnosticActions.DataSource = this.diagActionsBindingSource;
			this.gridControlDiagnosticActions.MainView = this.gridViewDiagnosticActions;
			this.gridControlDiagnosticActions.Name = "gridControlDiagnosticActions";
			this.gridControlDiagnosticActions.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemButtonEditParams,
				this.repositoryItemButtonEditService,
				this.repositoryItemTextEditDummy,
				this.repositoryItemButtonEditSignal
			});
			this.gridControlDiagnosticActions.ToolTipController = this.toolTipController;
			this.gridControlDiagnosticActions.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewDiagnosticActions
			});
			this.gridControlDiagnosticActions.DragDrop += new DragEventHandler(this.gridControlDiagnosticActions_DragDrop);
			this.gridControlDiagnosticActions.DragOver += new DragEventHandler(this.gridControlDiagnosticActions_DragOver);
			this.diagActionsBindingSource.DataSource = typeof(DiagnosticAction);
			this.gridViewDiagnosticActions.Columns.AddRange(new GridColumn[]
			{
				this.colEvent,
				this.colSignal,
				this.colService,
				this.colEcu,
				this.colParameter,
				this.colRawMessage,
				this.colSession
			});
			this.gridViewDiagnosticActions.GridControl = this.gridControlDiagnosticActions;
			this.gridViewDiagnosticActions.GroupCount = 1;
			this.gridViewDiagnosticActions.Name = "gridViewDiagnosticActions";
			this.gridViewDiagnosticActions.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
			this.gridViewDiagnosticActions.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
			this.gridViewDiagnosticActions.OptionsBehavior.AutoExpandAllGroups = true;
			this.gridViewDiagnosticActions.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
			this.gridViewDiagnosticActions.OptionsCustomization.AllowFilter = false;
			this.gridViewDiagnosticActions.OptionsCustomization.AllowQuickHideColumns = false;
			this.gridViewDiagnosticActions.OptionsSelection.EnableAppearanceHideSelection = false;
			this.gridViewDiagnosticActions.OptionsView.ShowGroupPanel = false;
			this.gridViewDiagnosticActions.OptionsView.ShowIndicator = false;
			this.gridViewDiagnosticActions.PaintStyleName = "WindowsXP";
			this.gridViewDiagnosticActions.RowHeight = 20;
			this.gridViewDiagnosticActions.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.colEvent, ColumnSortOrder.Ascending)
			});
			this.gridViewDiagnosticActions.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewDiagnosticActions_CustomDrawCell);
			this.gridViewDiagnosticActions.CustomDrawGroupRow += new RowObjectCustomDrawEventHandler(this.gridViewDiagnosticActions_CustomDrawGroupRow);
			this.gridViewDiagnosticActions.CustomRowCellEdit += new CustomRowCellEditEventHandler(this.gridViewDiagnosticActions_CustomRowCellEdit);
			this.gridViewDiagnosticActions.LeftCoordChanged += new EventHandler(this.gridViewDiagnosticActions_LeftCoordChanged);
			this.gridViewDiagnosticActions.TopRowChanged += new EventHandler(this.gridViewDiagnosticActions_TopRowChanged);
			this.gridViewDiagnosticActions.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewDiagnosticActions_PopupMenuShowing);
			this.gridViewDiagnosticActions.GroupRowExpanded += new RowEventHandler(this.gridViewDiagnosticActions_GroupRowExpanded);
			this.gridViewDiagnosticActions.GroupRowCollapsing += new RowAllowEventHandler(this.gridViewDiagnosticActions_GroupRowCollapsing);
			this.gridViewDiagnosticActions.GroupRowExpanding += new RowAllowEventHandler(this.gridViewDiagnosticActions_GroupRowExpanding);
			this.gridViewDiagnosticActions.CustomColumnGroup += new CustomColumnSortEventHandler(this.gridViewDiagnosticActions_CustomColumnGroup);
			this.gridViewDiagnosticActions.ShowingEditor += new CancelEventHandler(this.gridViewDiagnosticActions_ShowingEditor);
			this.gridViewDiagnosticActions.ShownEditor += new EventHandler(this.gridViewDiagnosticActions_ShownEditor);
			this.gridViewDiagnosticActions.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gridViewDiagnosticActions_FocusedRowChanged);
			this.gridViewDiagnosticActions.CustomColumnSort += new CustomColumnSortEventHandler(this.gridViewDiagnosticActions_CustomColumnSort);
			this.gridViewDiagnosticActions.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewDiagnosticActions_CustomUnboundColumnData);
			this.gridViewDiagnosticActions.KeyDown += new KeyEventHandler(this.gridViewDiagnosticActions_KeyDown);
			this.gridViewDiagnosticActions.MouseDown += new MouseEventHandler(this.gridViewDiagnosticActions_MouseDown);
			this.gridViewDiagnosticActions.MouseMove += new MouseEventHandler(this.gridViewDiagnosticActions_MouseMove);
			this.gridViewDiagnosticActions.DoubleClick += new EventHandler(this.gridViewDiagnosticActions_DoubleClick);
			componentResourceManager.ApplyResources(this.colEvent, "colEvent");
			this.colEvent.FieldName = "anyString1";
			this.colEvent.Name = "colEvent";
			this.colEvent.OptionsColumn.AllowEdit = false;
			this.colEvent.OptionsColumn.AllowSort = DefaultBoolean.True;
			this.colEvent.SortMode = ColumnSortMode.Custom;
			this.colEvent.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colSignal, "colSignal");
			this.colSignal.ColumnEdit = this.repositoryItemButtonEditSignal;
			this.colSignal.FieldName = "colSignal";
			this.colSignal.Name = "colSignal";
			this.colSignal.OptionsColumn.AllowSort = DefaultBoolean.False;
			this.colSignal.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonEditSignal, "repositoryItemButtonEditSignal");
			this.repositoryItemButtonEditSignal.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEditSignal.Name = "repositoryItemButtonEditSignal";
			this.repositoryItemButtonEditSignal.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEditSignal.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEditSignal_ButtonClick);
			this.repositoryItemButtonEditSignal.MouseDown += new MouseEventHandler(this.repositoryItemButtonEditSignal_MouseDown);
			this.repositoryItemButtonEditSignal.MouseMove += new MouseEventHandler(this.repositoryItemButtonEditSignal_MouseMove);
			componentResourceManager.ApplyResources(this.colService, "colService");
			this.colService.ColumnEdit = this.repositoryItemButtonEditService;
			this.colService.FieldName = "anyString3";
			this.colService.Name = "colService";
			this.colService.OptionsColumn.AllowSort = DefaultBoolean.False;
			this.colService.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonEditService, "repositoryItemButtonEditService");
			this.repositoryItemButtonEditService.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEditService.Name = "repositoryItemButtonEditService";
			this.repositoryItemButtonEditService.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEditService.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEditService_ButtonClick);
			this.repositoryItemButtonEditService.MouseDown += new MouseEventHandler(this.repositoryItemButtonEditService_MouseDown);
			this.repositoryItemButtonEditService.MouseMove += new MouseEventHandler(this.repositoryItemButtonEditService_MouseMove);
			componentResourceManager.ApplyResources(this.colEcu, "colEcu");
			this.colEcu.FieldName = "anyString4";
			this.colEcu.Name = "colEcu";
			this.colEcu.OptionsColumn.AllowEdit = false;
			this.colEcu.OptionsColumn.AllowSort = DefaultBoolean.False;
			this.colEcu.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colParameter, "colParameter");
			this.colParameter.ColumnEdit = this.repositoryItemButtonEditParams;
			this.colParameter.FieldName = "anyString5";
			this.colParameter.Name = "colParameter";
			this.colParameter.OptionsColumn.AllowSort = DefaultBoolean.False;
			this.colParameter.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonEditParams, "repositoryItemButtonEditParams");
			this.repositoryItemButtonEditParams.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEditParams.Name = "repositoryItemButtonEditParams";
			this.repositoryItemButtonEditParams.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEditParams.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEditParams_ButtonClick);
			this.repositoryItemButtonEditParams.MouseDown += new MouseEventHandler(this.repositoryItemButtonEditParams_MouseDown);
			this.repositoryItemButtonEditParams.MouseMove += new MouseEventHandler(this.repositoryItemButtonEditParams_MouseMove);
			componentResourceManager.ApplyResources(this.colRawMessage, "colRawMessage");
			this.colRawMessage.ColumnEdit = this.repositoryItemButtonEditParams;
			this.colRawMessage.FieldName = "anyString6";
			this.colRawMessage.Name = "colRawMessage";
			this.colRawMessage.OptionsColumn.AllowSort = DefaultBoolean.False;
			this.colRawMessage.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colSession, "colSession");
			this.colSession.ColumnEdit = this.repositoryItemButtonEditParams;
			this.colSession.FieldName = "anyString7";
			this.colSession.Name = "colSession";
			this.colSession.OptionsColumn.AllowSort = DefaultBoolean.False;
			this.colSession.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemTextEditDummy, "repositoryItemTextEditDummy");
			this.repositoryItemTextEditDummy.Name = "repositoryItemTextEditDummy";
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
			componentResourceManager.ApplyResources(this.buttonMoveFirst, "buttonMoveFirst");
			this.buttonMoveFirst.Image = Resources.ImageMoveFirst;
			this.buttonMoveFirst.Name = "buttonMoveFirst";
			this.buttonMoveFirst.UseVisualStyleBackColor = true;
			this.buttonMoveFirst.Click += new EventHandler(this.buttonMoveFirst_Click);
			componentResourceManager.ApplyResources(this.buttonMoveUp, "buttonMoveUp");
			this.buttonMoveUp.Image = Resources.ImageMovePrev;
			this.buttonMoveUp.Name = "buttonMoveUp";
			this.buttonMoveUp.UseVisualStyleBackColor = true;
			this.buttonMoveUp.Click += new EventHandler(this.buttonMoveUp_Click);
			componentResourceManager.ApplyResources(this.buttonMoveDown, "buttonMoveDown");
			this.buttonMoveDown.Image = Resources.ImageMoveNext;
			this.buttonMoveDown.Name = "buttonMoveDown";
			this.buttonMoveDown.UseVisualStyleBackColor = true;
			this.buttonMoveDown.Click += new EventHandler(this.buttonMoveDown_Click);
			componentResourceManager.ApplyResources(this.buttonMoveLast, "buttonMoveLast");
			this.buttonMoveLast.Image = Resources.ImageMoveLast;
			this.buttonMoveLast.Name = "buttonMoveLast";
			this.buttonMoveLast.UseVisualStyleBackColor = true;
			this.buttonMoveLast.Click += new EventHandler(this.buttonMoveLast_Click);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.buttonMoveLast);
			base.Controls.Add(this.buttonMoveDown);
			base.Controls.Add(this.buttonMoveUp);
			base.Controls.Add(this.buttonMoveFirst);
			base.Controls.Add(this.gridControlDiagnosticActions);
			base.Name = "DiagnosticActionsGrid";
			componentResourceManager.ApplyResources(this, "$this");
			base.Resize += new EventHandler(this.DiagnosticActionsGrid_Resize);
			((ISupportInitialize)this.gridControlDiagnosticActions).EndInit();
			((ISupportInitialize)this.diagActionsBindingSource).EndInit();
			((ISupportInitialize)this.gridViewDiagnosticActions).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEditSignal).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEditService).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEditParams).EndInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).EndInit();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
