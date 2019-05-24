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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.FiltersPage
{
	internal class FilterGrid : UserControl
	{
		private Dictionary<Filter, string> filter2CommentList;

		private FilterConfiguration filterConfiguration;

		private DisplayMode displayMode;

		private bool isSymSelOpened;

		private GUIElementManager_ControlGridTree guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private IContainer components;

		private GridControl gridControlFilters;

		private GridView gridViewFilters;

		private GridColumn colAction;

		private GridColumn colType;

		private GridColumn colChannel;

		private GridColumn colCondition;

		private RepositoryItemComboBox repositoryItemComboBoxLogger;

		private RepositoryItemComboBox repositoryItemComboBoxChannel;

		private RepositoryItemImageComboBox repositoryItemImageComboBoxAction;

		private Button buttonMoveFirst;

		private Button buttonMoveUp;

		private Button buttonMoveDown;

		private Button buttonMoveLast;

		private BindingSource filterBindingSource;

		private ImageList imageList;

		private RepositoryItemButtonEdit repositoryItemButtonEditCondition;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private RepositoryItemTextEdit repositoryItemTextEditDummy;

		private GridColumn colActive;

		private RepositoryItemCheckEdit repositoryItemCheckEditIsActive;

		private RepositoryItemCheckEdit repositoryItemCheckEditDummy;

		private GridColumn colRate;

		private RepositoryItemButtonEdit repositoryItemButtonEditRate;

		private XtraToolTipController toolTipController;

		public event EventHandler SelectionChanged;

		public FilterConfiguration FilterConfiguration
		{
			get
			{
				return this.filterConfiguration;
			}
			set
			{
				this.filterConfiguration = value;
				if (value != null)
				{
					int focusedRowHandle = this.gridViewFilters.FocusedRowHandle;
					this.ResetValidationFramework();
					this.gridControlFilters.DataSource = this.filterConfiguration.Filters;
					this.ValidateInput(false);
					this.gridViewFilters.RefreshData();
					if (focusedRowHandle >= 0 && focusedRowHandle < this.gridViewFilters.RowCount)
					{
						this.gridViewFilters.FocusedRowHandle = focusedRowHandle;
					}
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
					this.gridViewFilters.RefreshData();
				}
				this.displayMode = value;
			}
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		public FilterGrid()
		{
			this.InitializeComponent();
			this.filter2CommentList = new Dictionary<Filter, string>();
			this.guiElementManager = new GUIElementManager_ControlGridTree();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.isSymSelOpened = false;
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

		public void AddFilter(Filter filter)
		{
			this.filterConfiguration.AddFilter(filter);
			this.gridViewFilters.RefreshData();
			this.ValidateInput(true);
			this.SelectRowOfFilter(filter);
		}

		public void RemoveFilter(Filter filter)
		{
			this.filterConfiguration.RemoveFilter(filter);
			this.gridViewFilters.RefreshData();
			this.ValidateInput(true);
		}

		private void buttonMoveFirst_Click(object sender, EventArgs e)
		{
			Filter filter;
			int num;
			if (this.TryGetSelectedFilter(out filter, out num))
			{
				if (num == 0)
				{
					return;
				}
				this.filterConfiguration.RemoveFilter(filter);
				this.filterConfiguration.InsertFilter(1, filter);
				this.gridViewFilters.RefreshData();
				this.ValidateInput(true);
				this.gridViewFilters.FocusedRowHandle = 1;
			}
		}

		private void buttonMoveUp_Click(object sender, EventArgs e)
		{
			Filter filter;
			int num;
			if (this.TryGetSelectedFilter(out filter, out num))
			{
				if (num == 0)
				{
					return;
				}
				this.filterConfiguration.RemoveFilter(filter);
				this.filterConfiguration.InsertFilter(num - 1, filter);
				this.gridViewFilters.RefreshData();
				this.ValidateInput(true);
				this.gridViewFilters.FocusedRowHandle = num - 1;
			}
		}

		private void buttonMoveDown_Click(object sender, EventArgs e)
		{
			Filter filter;
			int num;
			if (this.TryGetSelectedFilter(out filter, out num))
			{
				if (num == this.filterConfiguration.Filters.Count - 1)
				{
					return;
				}
				this.filterConfiguration.RemoveFilter(filter);
				this.filterConfiguration.InsertFilter(num + 1, filter);
				this.gridViewFilters.RefreshData();
				this.ValidateInput(true);
				this.gridViewFilters.FocusedRowHandle = num + 1;
			}
		}

		private void buttonMoveLast_Click(object sender, EventArgs e)
		{
			Filter filter;
			int num;
			if (this.TryGetSelectedFilter(out filter, out num))
			{
				if (num == this.filterConfiguration.Filters.Count - 1)
				{
					return;
				}
				this.filterConfiguration.RemoveFilter(filter);
				this.filterConfiguration.AddFilter(filter);
				this.gridViewFilters.RefreshData();
				this.ValidateInput(true);
				this.gridViewFilters.FocusedRowHandle = this.filterConfiguration.Filters.Count - 1;
			}
		}

		private void gridViewFilters_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			Filter filter;
			if (!this.GetFilter(e.ListSourceRowIndex, out filter))
			{
				return;
			}
			if (e.Column == this.colAction)
			{
				this.UnboundColumnDataAction(filter, e);
				return;
			}
			if (e.Column == this.colRate)
			{
				this.UnboundColumnRate(filter, e);
				return;
			}
			if (e.Column == this.colActive)
			{
				this.UnboundColumnDataActive(filter, e);
				return;
			}
			if (e.Column == this.colType)
			{
				this.UnboundColumnDataType(filter, e);
				return;
			}
			if (e.Column == this.colChannel)
			{
				this.UnboundColumnDataChannel(filter, e);
				return;
			}
			if (e.Column == this.colCondition)
			{
				this.UnboundColumnDataCondition(filter, e);
			}
		}

		private void gridViewFilters_ShowingEditor(object sender, CancelEventArgs e)
		{
			Filter filter;
			if (this.TryGetSelectedFilter(out filter))
			{
				if (this.gridViewFilters.FocusedColumn == this.colRate && filter.Action.Value != FilterActionType.Limit)
				{
					e.Cancel = true;
				}
				if (filter is DefaultFilter)
				{
					if (this.gridViewFilters.FocusedColumn == this.colCondition || this.gridViewFilters.FocusedColumn == this.colChannel || this.gridViewFilters.FocusedColumn == this.colActive)
					{
						e.Cancel = true;
						return;
					}
				}
				else if (filter is ChannelFilter && this.gridViewFilters.FocusedColumn == this.colCondition)
				{
					e.Cancel = true;
				}
			}
		}

		private void gridViewFilters_ShownEditor(object sender, EventArgs e)
		{
			if (this.gridViewFilters.FocusedColumn != this.colChannel)
			{
				if (this.gridViewFilters.FocusedColumn == this.colAction)
				{
					this.ShownEditorAction();
				}
				return;
			}
			Filter filter;
			if (!this.TryGetSelectedFilter(out filter) || filter is DefaultFilter)
			{
				return;
			}
			this.ShownEditorChannel();
		}

		private void gridViewFilters_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
			Filter filter;
			if (!this.TryGetSelectedFilter(out filter) || filter is DefaultFilter)
			{
				this.buttonMoveFirst.Enabled = false;
				this.buttonMoveUp.Enabled = false;
				this.buttonMoveDown.Enabled = false;
				this.buttonMoveLast.Enabled = false;
			}
			else
			{
				this.buttonMoveFirst.Enabled = (e.FocusedRowHandle > 1);
				this.buttonMoveUp.Enabled = (e.FocusedRowHandle > 1);
				this.buttonMoveDown.Enabled = (e.FocusedRowHandle < this.FilterConfiguration.Filters.Count - 1);
				this.buttonMoveLast.Enabled = (e.FocusedRowHandle < this.FilterConfiguration.Filters.Count - 1);
			}
			this.Raise_SelectionChanged(this, EventArgs.Empty);
		}

		private void gridViewFilters_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
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

		private void gridViewFilters_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			Filter filter;
			if (this.TryGetFilterByIndex(out filter, e.RowHandle))
			{
				if (e.Column == this.colRate && filter.Action.Value != FilterActionType.Limit)
				{
					e.RepositoryItem = this.repositoryItemTextEditDummy;
				}
				if (filter is DefaultFilter)
				{
					if (e.Column == this.colCondition || e.Column == this.colChannel)
					{
						e.RepositoryItem = this.repositoryItemTextEditDummy;
						return;
					}
					if (e.Column == this.colActive)
					{
						e.RepositoryItem = this.repositoryItemCheckEditDummy;
						e.RepositoryItem.Enabled = false;
						return;
					}
				}
				else if (filter is ChannelFilter && e.Column == this.colCondition)
				{
					e.RepositoryItem = this.repositoryItemTextEditDummy;
				}
			}
		}

		private void gridViewFilters_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			if (e.Column == this.colType)
			{
				Filter filter;
				if (this.GetFilter(this.gridViewFilters.GetDataSourceRowIndex(e.RowHandle), out filter))
				{
					if (filter is CANIdFilter)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageIDMessageCAN);
					}
					if (filter is FlexrayIdFilter)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageIDMessageFlexray);
					}
					if (filter is LINIdFilter)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageIDMessageLIN);
						return;
					}
					if (filter is SymbolicMessageFilter)
					{
						switch ((filter as SymbolicMessageFilter).BusType.Value)
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
					else if (filter is ChannelFilter)
					{
						switch ((filter as ChannelFilter).BusType.Value)
						{
						case BusType.Bt_CAN:
							GridUtil.DrawImageTextCell(e, Resources.ImageChnFilterCAN);
							return;
						case BusType.Bt_LIN:
							GridUtil.DrawImageTextCell(e, Resources.ImageChnFilterLIN);
							return;
						case BusType.Bt_FlexRay:
							GridUtil.DrawImageTextCell(e, Resources.ImageChnFilterFlexray);
							return;
						default:
							GridUtil.DrawImageTextCell(e, Resources.ImageChnFilterCAN);
							return;
						}
					}
					else if (filter is SignalListFileFilter)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageSymbSignalList);
						return;
					}
				}
			}
			else
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(e.Column, this.gridViewFilters.GetDataSourceRowIndex(e.RowHandle));
				this.customErrorProvider.Grid.DisplayError(gUIElement, e);
			}
		}

		private void gridViewFilters_RowStyle(object sender, RowStyleEventArgs e)
		{
			Filter filter;
			if (this.TryGetFilterByIndex(out filter, this.gridViewFilters.GetDataSourceRowIndex(e.RowHandle)) && !filter.IsActive.Value)
			{
				e.Appearance.ForeColor = SystemColors.GrayText;
			}
		}

		private void gridViewFilters_KeyDown(object sender, KeyEventArgs e)
		{
			Filter filter;
			if (e.KeyCode == Keys.Delete && this.TryGetSelectedFilter(out filter))
			{
				if (filter is DefaultFilter)
				{
					return;
				}
				this.RemoveFilter(filter);
			}
		}

		private void gridViewFilters_TopRowChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridViewFilters_LeftCoordChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void FilterGrid_Resize(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void repositoryItemButtonEditCondition_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			bool flag = false;
			int arg_0D_0 = this.gridViewFilters.FocusedRowHandle;
			Filter filter;
			if (!this.TryGetSelectedFilter(out filter))
			{
				return;
			}
			if (filter is SymbolicMessageFilter)
			{
				SymbolicMessageFilter symbolicMessageFilter = filter as SymbolicMessageFilter;
				string value = symbolicMessageFilter.MessageName.Value;
				string value2 = symbolicMessageFilter.DatabaseName.Value;
				string text = symbolicMessageFilter.DatabasePath.Value;
				string value3 = symbolicMessageFilter.NetworkName.Value;
				uint arg_6C_0 = symbolicMessageFilter.ChannelNumber.Value;
				BusType value4 = symbolicMessageFilter.BusType.Value;
				bool value5 = symbolicMessageFilter.IsFlexrayPDU.Value;
				MessageDefinition messageDefinition = null;
				bool flag2 = this.ApplicationDatabaseManager.ResolveMessageSymbolInDatabase(this.ModelValidator.GetAbsoluteFilePath(text), value3, value, value4, out messageDefinition);
				if (BusType.Bt_FlexRay == symbolicMessageFilter.BusType.Value && flag2)
				{
					if (new SymbolicFlexrayCondition(this.ApplicationDatabaseManager, this.ModelValidator)
					{
						Filter = symbolicMessageFilter
					}.ShowDialog() != DialogResult.OK)
					{
						return;
					}
					if (symbolicMessageFilter.MessageName.Value != value || symbolicMessageFilter.DatabaseName.Value != value2 || symbolicMessageFilter.DatabasePath.Value != text || symbolicMessageFilter.NetworkName.Value != value3 || symbolicMessageFilter.BusType.Value != value4 || symbolicMessageFilter.IsFlexrayPDU.Value != value5)
					{
						IList<uint> channelAssignmentOfDatabase = this.ModelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(symbolicMessageFilter.DatabasePath.Value, symbolicMessageFilter.NetworkName.Value);
						if (symbolicMessageFilter.DatabasePath.Value != text || symbolicMessageFilter.NetworkName.Value != value3)
						{
							if (channelAssignmentOfDatabase.Count > 0)
							{
								symbolicMessageFilter.ChannelNumber.Value = channelAssignmentOfDatabase[0];
							}
						}
						else if (channelAssignmentOfDatabase.Count > 0 && channelAssignmentOfDatabase[0] == Database.ChannelNumber_FlexrayAB)
						{
							if (symbolicMessageFilter.MessageName.Value.EndsWith(Constants.FlexrayChannelA_Postfix) && value.EndsWith(Constants.FlexrayChannelB_Postfix))
							{
								symbolicMessageFilter.ChannelNumber.Value = 1u;
							}
							if (symbolicMessageFilter.MessageName.Value.EndsWith(Constants.FlexrayChannelB_Postfix) && value.EndsWith(Constants.FlexrayChannelA_Postfix))
							{
								symbolicMessageFilter.ChannelNumber.Value = 2u;
							}
						}
						flag = true;
					}
				}
				else
				{
					if (this.isSymSelOpened)
					{
						return;
					}
					this.isSymSelOpened = true;
					if (this.ApplicationDatabaseManager.SelectMessageInDatabase(ref value, ref value2, ref text, ref value3, ref value4, ref value5))
					{
						this.isSymSelOpened = false;
						string message;
						if (!this.ModelValidator.DatabaseServices.IsSymbolicMessageInsertAllowed(value, value3, text, value4, out message))
						{
							InformMessageBox.Error(message);
							return;
						}
						text = this.ModelValidator.GetFilePathRelativeToConfiguration(text);
						if (symbolicMessageFilter.MessageName.Value != value || symbolicMessageFilter.DatabaseName.Value != value2 || symbolicMessageFilter.DatabasePath.Value != text || symbolicMessageFilter.NetworkName.Value != value3 || symbolicMessageFilter.BusType.Value != value4 || symbolicMessageFilter.IsFlexrayPDU.Value != value5)
						{
							flag = true;
							symbolicMessageFilter.MessageName.Value = value;
							symbolicMessageFilter.DatabaseName.Value = value2;
							if (symbolicMessageFilter.DatabasePath.Value != text || symbolicMessageFilter.NetworkName.Value != value3)
							{
								IList<uint> channelAssignmentOfDatabase2 = this.ModelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(text, value3);
								if (channelAssignmentOfDatabase2.Count > 0)
								{
									symbolicMessageFilter.ChannelNumber.Value = channelAssignmentOfDatabase2[0];
								}
							}
							symbolicMessageFilter.DatabasePath.Value = text;
							symbolicMessageFilter.BusType.Value = value4;
							symbolicMessageFilter.IsFlexrayPDU.Value = value5;
						}
					}
					this.isSymSelOpened = false;
				}
			}
			else if (filter is CANIdFilter)
			{
				CANIdFilter cANIdFilter = filter as CANIdFilter;
				CANIdCondition cANIdCondition = new CANIdCondition();
				cANIdCondition.IsExtendedId = cANIdFilter.IsExtendedId.Value;
				cANIdCondition.IsRange = cANIdFilter.IsIdRange.Value;
				cANIdCondition.MessageID = cANIdFilter.CANId.Value;
				cANIdCondition.LastMessageID = cANIdFilter.CANIdLast.Value;
				if (DialogResult.OK == cANIdCondition.ShowDialog() && (cANIdFilter.IsExtendedId.Value != cANIdCondition.IsExtendedId || cANIdFilter.IsIdRange.Value != cANIdCondition.IsRange || cANIdFilter.CANId.Value != cANIdCondition.MessageID || cANIdFilter.CANIdLast.Value != cANIdCondition.LastMessageID))
				{
					cANIdFilter.IsExtendedId.Value = cANIdCondition.IsExtendedId;
					cANIdFilter.IsIdRange.Value = cANIdCondition.IsRange;
					cANIdFilter.CANId.Value = cANIdCondition.MessageID;
					cANIdFilter.CANIdLast.Value = cANIdCondition.LastMessageID;
					flag = true;
				}
			}
			else if (filter is LINIdFilter)
			{
				LINIdFilter lINIdFilter = filter as LINIdFilter;
				LINIdCondition lINIdCondition = new LINIdCondition();
				lINIdCondition.IsRange = lINIdFilter.IsIdRange.Value;
				lINIdCondition.MessageID = lINIdFilter.LINId.Value;
				lINIdCondition.LastMessageID = lINIdFilter.LINIdLast.Value;
				if (DialogResult.OK == lINIdCondition.ShowDialog() && (lINIdFilter.IsIdRange.Value != lINIdCondition.IsRange || lINIdFilter.LINId.Value != lINIdCondition.MessageID || lINIdFilter.LINIdLast.Value != lINIdCondition.LastMessageID))
				{
					lINIdFilter.IsIdRange.Value = lINIdCondition.IsRange;
					lINIdFilter.LINId.Value = lINIdCondition.MessageID;
					lINIdFilter.LINIdLast.Value = lINIdCondition.LastMessageID;
					flag = true;
				}
			}
			else if (filter is FlexrayIdFilter)
			{
				FlexrayIdFilter flexrayIdFilter = filter as FlexrayIdFilter;
				FlexrayIdCondition flexrayIdCondition = new FlexrayIdCondition();
				flexrayIdCondition.IsIdRange = flexrayIdFilter.IsIdRange.Value;
				flexrayIdCondition.FrameId = flexrayIdFilter.FlexrayId.Value;
				flexrayIdCondition.LastFrameId = flexrayIdFilter.FlexrayIdLast.Value;
				flexrayIdCondition.CycleTime = flexrayIdFilter.BaseCycle.Value;
				flexrayIdCondition.CycleRepetiton = flexrayIdFilter.CycleRepetition.Value;
				if (DialogResult.OK == flexrayIdCondition.ShowDialog() && (flexrayIdFilter.IsIdRange.Value != flexrayIdCondition.IsIdRange || flexrayIdFilter.FlexrayId.Value != flexrayIdCondition.FrameId || flexrayIdFilter.FlexrayIdLast.Value != flexrayIdCondition.LastFrameId || flexrayIdFilter.BaseCycle.Value != flexrayIdCondition.CycleTime || flexrayIdFilter.CycleRepetition.Value != flexrayIdCondition.CycleRepetiton))
				{
					flexrayIdFilter.IsIdRange.Value = flexrayIdCondition.IsIdRange;
					flexrayIdFilter.FlexrayId.Value = flexrayIdCondition.FrameId;
					flexrayIdFilter.FlexrayIdLast.Value = flexrayIdCondition.LastFrameId;
					flexrayIdFilter.BaseCycle.Value = flexrayIdCondition.CycleTime;
					flexrayIdFilter.CycleRepetition.Value = flexrayIdCondition.CycleRepetiton;
					flag = true;
				}
			}
			else if (filter is SignalListFileFilter)
			{
				SignalListFileFilter signalListFileFilter = filter as SignalListFileFilter;
				string absoluteFilePath = this.ModelValidator.GetAbsoluteFilePath(signalListFileFilter.FilePath.Value);
				FileSystemServices.LaunchFile(absoluteFilePath);
			}
			if (flag)
			{
				this.gridViewFilters.RefreshData();
				this.ValidateInput(true);
			}
		}

		private void repositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewFilters.PostEditor();
		}

		private void repositoryItemCheckEditIsActive_CheckedChanged(object sender, EventArgs e)
		{
			this.gridViewFilters.PostEditor();
		}

		private void repositoryItemButtonEditRate_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			this.EditActionLimitRate();
		}

		private void EditActionLimitRate()
		{
			Filter filter;
			if (!this.TryGetSelectedFilter(out filter))
			{
				return;
			}
			using (SetLimitRate setLimitRate = new SetLimitRate(this.ModelValidator))
			{
				setLimitRate.Filter = filter;
				uint value = filter.LimitIntervalPerFrame.Value;
				if (setLimitRate.ShowDialog() == DialogResult.OK && value != filter.LimitIntervalPerFrame.Value)
				{
					this.gridViewFilters.RefreshData();
					this.ValidateInput(true);
				}
			}
		}

		private void UnboundColumnDataAction(Filter filter, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (filter.Action.Value == FilterActionType.Stop)
				{
					e.Value = Resources.FilterActionStop;
				}
				else if (filter.Action.Value == FilterActionType.Limit)
				{
					e.Value = Resources.FilterActionLimit;
				}
				else
				{
					e.Value = Resources.FilterActionPass;
				}
				this.pageValidator.Grid.StoreMapping(filter.Action, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
				return;
			}
			FilterActionType filterActionType;
			if (e.Value.ToString() == Resources.FilterActionStop)
			{
				filterActionType = FilterActionType.Stop;
			}
			else if (e.Value.ToString() == Resources.FilterActionLimit)
			{
				filterActionType = FilterActionType.Limit;
			}
			else
			{
				filterActionType = FilterActionType.Pass;
			}
			FilterActionType value = filter.Action.Value;
			if (value == FilterActionType.Limit && filterActionType != FilterActionType.Limit)
			{
				filter.LimitIntervalPerFrame.Value = Constants.DefaultLimitInterval_ms;
			}
			bool flag;
			this.pageValidator.Grid.UpdateModel<FilterActionType>(filterActionType, filter.Action, out flag);
			if (flag)
			{
				this.ValidateInput(true);
			}
			if (filterActionType == FilterActionType.Limit)
			{
				this.EditActionLimitRate();
			}
		}

		private void UnboundColumnRate(Filter filter, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (filter.Action.Value == FilterActionType.Limit)
				{
					e.Value = string.Format(Resources.FilterLimitRate, filter.LimitIntervalPerFrame.Value);
					return;
				}
				if (filter.Action.Value == FilterActionType.Pass)
				{
					e.Value = Resources.FilterPassRate;
					return;
				}
				e.Value = "";
			}
		}

		private void UnboundColumnDataActive(Filter filter, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (filter is DefaultFilter)
				{
					e.Value = true;
				}
				else
				{
					e.Value = filter.IsActive.Value;
				}
				this.pageValidator.Grid.StoreMapping(filter.IsActive, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
				return;
			}
			bool value = Convert.ToBoolean(e.Value);
			bool flag;
			this.pageValidator.Grid.UpdateModel<bool>(value, filter.IsActive, out flag);
			if (flag)
			{
				this.ValidateInput(true);
			}
		}

		private void UnboundColumnDataType(Filter filter, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (filter is DefaultFilter)
				{
					e.Value = Resources.FilterTypeNameDefault;
					return;
				}
				if (filter is ChannelFilter)
				{
					ChannelFilter channelFilter = filter as ChannelFilter;
					if (BusType.Bt_CAN == channelFilter.BusType.Value)
					{
						e.Value = Resources.FilterTypeNameCANChn;
						return;
					}
					if (BusType.Bt_LIN == channelFilter.BusType.Value)
					{
						e.Value = Resources.FilterTypeNameLINChn;
						return;
					}
					if (BusType.Bt_FlexRay == channelFilter.BusType.Value)
					{
						e.Value = Resources.FilterTypeNameFlexRayChn;
						return;
					}
				}
				else if (filter is SymbolicMessageFilter)
				{
					SymbolicMessageFilter symbolicMessageFilter = filter as SymbolicMessageFilter;
					if (BusType.Bt_CAN == symbolicMessageFilter.BusType.Value)
					{
						e.Value = Resources.FilterTypeNameSymbolicCAN;
						return;
					}
					if (BusType.Bt_LIN == symbolicMessageFilter.BusType.Value)
					{
						e.Value = Resources.FilterTypeNameSymbolicLIN;
						return;
					}
					if (BusType.Bt_FlexRay == symbolicMessageFilter.BusType.Value)
					{
						e.Value = Resources.FilterTypeNameSymbolicFlexray;
						return;
					}
				}
				else
				{
					if (filter is CANIdFilter)
					{
						e.Value = Resources.FilterTypeNameCANId;
						return;
					}
					if (filter is LINIdFilter)
					{
						e.Value = Resources.FilterTypeNameLINId;
						return;
					}
					if (filter is FlexrayIdFilter)
					{
						e.Value = Resources.FilterTypeNameFlexrayId;
						return;
					}
					if (filter is SignalListFileFilter)
					{
						e.Value = Resources.FilterTypeNameCANSignalList;
					}
				}
			}
		}

		private void UnboundColumnDataChannel(Filter filter, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (filter is DefaultFilter)
				{
					e.Value = Resources.FilterChannelColAll;
					return;
				}
				if (filter is ChannelFilter)
				{
					ChannelFilter channelFilter = filter as ChannelFilter;
					if (BusType.Bt_CAN == channelFilter.BusType.Value)
					{
						e.Value = GUIUtil.MapCANChannelNumber2String(channelFilter.ChannelNumber.Value);
					}
					else if (BusType.Bt_LIN == channelFilter.BusType.Value)
					{
						e.Value = GUIUtil.MapLINChannelNumber2String(channelFilter.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics);
					}
					else if (BusType.Bt_FlexRay == channelFilter.BusType.Value)
					{
						e.Value = GUIUtil.MapFlexrayChannelNumber2String(channelFilter.ChannelNumber.Value);
					}
					this.pageValidator.Grid.StoreMapping(channelFilter.ChannelNumber, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
					return;
				}
				if (filter is SymbolicMessageFilter)
				{
					SymbolicMessageFilter symbolicMessageFilter = filter as SymbolicMessageFilter;
					if (BusType.Bt_CAN == symbolicMessageFilter.BusType.Value)
					{
						e.Value = GUIUtil.MapCANChannelNumber2String(symbolicMessageFilter.ChannelNumber.Value);
					}
					else if (BusType.Bt_LIN == symbolicMessageFilter.BusType.Value)
					{
						e.Value = GUIUtil.MapLINChannelNumber2String(symbolicMessageFilter.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics);
					}
					else if (BusType.Bt_FlexRay == symbolicMessageFilter.BusType.Value)
					{
						e.Value = GUIUtil.MapFlexrayChannelNumber2String(symbolicMessageFilter.ChannelNumber.Value);
					}
					this.pageValidator.Grid.StoreMapping(symbolicMessageFilter.ChannelNumber, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
					return;
				}
				if (filter is CANIdFilter)
				{
					CANIdFilter cANIdFilter = filter as CANIdFilter;
					e.Value = GUIUtil.MapCANChannelNumber2String(cANIdFilter.ChannelNumber.Value);
					this.pageValidator.Grid.StoreMapping(cANIdFilter.ChannelNumber, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
					return;
				}
				if (filter is LINIdFilter)
				{
					LINIdFilter lINIdFilter = filter as LINIdFilter;
					e.Value = GUIUtil.MapLINChannelNumber2String(lINIdFilter.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics);
					this.pageValidator.Grid.StoreMapping(lINIdFilter.ChannelNumber, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
					return;
				}
				if (filter is FlexrayIdFilter)
				{
					FlexrayIdFilter flexrayIdFilter = filter as FlexrayIdFilter;
					e.Value = GUIUtil.MapFlexrayChannelNumber2String(flexrayIdFilter.ChannelNumber.Value);
					this.pageValidator.Grid.StoreMapping(flexrayIdFilter.ChannelNumber, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
					return;
				}
				if (filter is SignalListFileFilter)
				{
					SignalListFileFilter signalListFileFilter = filter as SignalListFileFilter;
					if (BusType.Bt_CAN == signalListFileFilter.BusType.Value)
					{
						e.Value = GUIUtil.MapCANChannelNumber2String(signalListFileFilter.ChannelNumber.Value);
					}
					else if (BusType.Bt_LIN == signalListFileFilter.BusType.Value)
					{
						e.Value = GUIUtil.MapLINChannelNumber2String(signalListFileFilter.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics);
					}
					this.pageValidator.Grid.StoreMapping(signalListFileFilter.ChannelNumber, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
					return;
				}
			}
			else
			{
				bool flag = false;
				if (filter is ChannelFilter)
				{
					ChannelFilter channelFilter2 = filter as ChannelFilter;
					uint value = 1u;
					if (BusType.Bt_CAN == channelFilter2.BusType.Value)
					{
						value = GUIUtil.MapCANChannelString2Number(e.Value.ToString());
					}
					else if (BusType.Bt_LIN == channelFilter2.BusType.Value)
					{
						value = GUIUtil.MapLINChannelString2Number(e.Value.ToString());
					}
					else if (BusType.Bt_FlexRay == channelFilter2.BusType.Value)
					{
						value = GUIUtil.MapFlexrayChannelString2Number(e.Value.ToString());
					}
					this.pageValidator.Grid.UpdateModel<uint>(value, channelFilter2.ChannelNumber, out flag);
				}
				else if (filter is SymbolicMessageFilter)
				{
					SymbolicMessageFilter symbolicMessageFilter2 = filter as SymbolicMessageFilter;
					uint value2 = 1u;
					if (BusType.Bt_CAN == symbolicMessageFilter2.BusType.Value)
					{
						value2 = GUIUtil.MapCANChannelString2Number(e.Value.ToString());
					}
					else if (BusType.Bt_LIN == symbolicMessageFilter2.BusType.Value)
					{
						value2 = GUIUtil.MapLINChannelString2Number(e.Value.ToString());
					}
					else if (BusType.Bt_FlexRay == symbolicMessageFilter2.BusType.Value)
					{
						value2 = GUIUtil.MapFlexrayChannelString2Number(e.Value.ToString());
					}
					this.pageValidator.Grid.UpdateModel<uint>(value2, symbolicMessageFilter2.ChannelNumber, out flag);
				}
				else if (filter is CANIdFilter)
				{
					CANIdFilter cANIdFilter2 = filter as CANIdFilter;
					this.pageValidator.Grid.UpdateModel<uint>(GUIUtil.MapCANChannelString2Number(e.Value.ToString()), cANIdFilter2.ChannelNumber, out flag);
				}
				else if (filter is LINIdFilter)
				{
					LINIdFilter lINIdFilter2 = filter as LINIdFilter;
					this.pageValidator.Grid.UpdateModel<uint>(GUIUtil.MapLINChannelString2Number(e.Value.ToString()), lINIdFilter2.ChannelNumber, out flag);
				}
				else if (filter is FlexrayIdFilter)
				{
					FlexrayIdFilter flexrayIdFilter2 = filter as FlexrayIdFilter;
					this.pageValidator.Grid.UpdateModel<uint>(GUIUtil.MapFlexrayChannelString2Number(e.Value.ToString()), flexrayIdFilter2.ChannelNumber, out flag);
				}
				else if (filter is SignalListFileFilter)
				{
					SignalListFileFilter signalListFileFilter2 = filter as SignalListFileFilter;
					uint value3 = 1u;
					if (BusType.Bt_CAN == signalListFileFilter2.BusType.Value)
					{
						value3 = GUIUtil.MapCANChannelString2Number(e.Value.ToString());
					}
					else if (BusType.Bt_LIN == signalListFileFilter2.BusType.Value)
					{
						value3 = GUIUtil.MapLINChannelString2Number(e.Value.ToString());
					}
					this.pageValidator.Grid.UpdateModel<uint>(value3, signalListFileFilter2.ChannelNumber, out flag);
				}
				if (flag)
				{
					this.ValidateInput(true);
				}
			}
		}

		private void UnboundColumnDataCondition(Filter filter, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = this.GetUnboundColumnDataConditionString(filter);
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex);
				IEnumerable<IValidatedProperty> validatedPropertyListCondition = this.GetValidatedPropertyListCondition(filter);
				foreach (IValidatedProperty current in validatedPropertyListCondition)
				{
					this.pageValidator.Grid.StoreMapping(current, gUIElement);
				}
			}
		}

		private string GetUnboundColumnDataConditionString(Filter filter)
		{
			if (filter is SymbolicMessageFilter)
			{
				return GUIUtil.MapFilterConditionToString(filter as SymbolicMessageFilter, this.ModelValidator.DatabaseServices);
			}
			if (filter is CANIdFilter)
			{
				return GUIUtil.MapFilterConditionToString(filter as CANIdFilter);
			}
			if (filter is LINIdFilter)
			{
				return GUIUtil.MapFilterConditionToString(filter as LINIdFilter);
			}
			if (filter is FlexrayIdFilter)
			{
				return GUIUtil.MapFilterConditionToString(filter as FlexrayIdFilter);
			}
			if (filter is SignalListFileFilter)
			{
				return GUIUtil.MapFilterConditionToString(filter as SignalListFileFilter);
			}
			return string.Empty;
		}

		private IEnumerable<IValidatedProperty> GetValidatedPropertyListCondition(Filter filter)
		{
			List<IValidatedProperty> list = new List<IValidatedProperty>();
			if (filter is SymbolicMessageFilter)
			{
				list.Add((filter as SymbolicMessageFilter).DatabasePath);
				list.Add((filter as SymbolicMessageFilter).DatabaseName);
				list.Add((filter as SymbolicMessageFilter).MessageName);
				list.Add((filter as SymbolicMessageFilter).BusType);
			}
			else if (filter is CANIdFilter)
			{
				list.Add((filter as CANIdFilter).IsIdRange);
				list.Add((filter as CANIdFilter).IsExtendedId);
				list.Add((filter as CANIdFilter).CANId);
				list.Add((filter as CANIdFilter).CANIdLast);
			}
			else if (filter is LINIdFilter)
			{
				list.Add((filter as LINIdFilter).IsIdRange);
				list.Add((filter as LINIdFilter).LINId);
				list.Add((filter as LINIdFilter).LINIdLast);
			}
			else if (filter is FlexrayIdFilter)
			{
				list.Add((filter as FlexrayIdFilter).IsIdRange);
				list.Add((filter as FlexrayIdFilter).FlexrayId);
				list.Add((filter as FlexrayIdFilter).FlexrayIdLast);
				list.Add((filter as FlexrayIdFilter).BaseCycle);
				list.Add((filter as FlexrayIdFilter).CycleRepetition);
			}
			else if (filter is SignalListFileFilter)
			{
				list.Add((filter as SignalListFileFilter).FilePath);
			}
			return list;
		}

		private void ShownEditorChannel()
		{
			ComboBoxEdit comboBoxEdit = this.gridViewFilters.ActiveEditor as ComboBoxEdit;
			if (comboBoxEdit != null)
			{
				Filter filter;
				if (!this.GetFilter(this.gridViewFilters.GetFocusedDataSourceRowIndex(), out filter))
				{
					return;
				}
				if (filter is CANIdFilter)
				{
					this.FillCANChannelComboBox(comboBoxEdit);
					return;
				}
				if (filter is LINIdFilter)
				{
					this.FillLINChannelComboBox(comboBoxEdit);
					return;
				}
				if (filter is FlexrayIdFilter)
				{
					this.FillFlexrayChannelComboBox(comboBoxEdit);
					return;
				}
				if (filter is ChannelFilter)
				{
					ChannelFilter channelFilter = filter as ChannelFilter;
					if (BusType.Bt_CAN == channelFilter.BusType.Value)
					{
						this.FillCANChannelComboBox(comboBoxEdit);
						return;
					}
					if (BusType.Bt_LIN == channelFilter.BusType.Value)
					{
						this.FillLINChannelComboBox(comboBoxEdit);
						return;
					}
					if (BusType.Bt_FlexRay == channelFilter.BusType.Value)
					{
						this.FillFlexrayChannelComboBox(comboBoxEdit);
						return;
					}
				}
				else if (filter is SymbolicMessageFilter)
				{
					SymbolicMessageFilter symbolicMessageFilter = filter as SymbolicMessageFilter;
					if (BusType.Bt_CAN == symbolicMessageFilter.BusType.Value)
					{
						this.FillCANChannelComboBox(comboBoxEdit);
						return;
					}
					if (BusType.Bt_LIN == symbolicMessageFilter.BusType.Value)
					{
						this.FillLINChannelComboBox(comboBoxEdit);
						return;
					}
					if (BusType.Bt_FlexRay == symbolicMessageFilter.BusType.Value)
					{
						this.FillFlexrayChannelComboBox(comboBoxEdit);
						return;
					}
				}
				else if (filter is SignalListFileFilter)
				{
					SignalListFileFilter signalListFileFilter = filter as SignalListFileFilter;
					if (BusType.Bt_CAN == signalListFileFilter.BusType.Value)
					{
						this.FillCANChannelComboBox(comboBoxEdit);
						return;
					}
					if (BusType.Bt_LIN == signalListFileFilter.BusType.Value)
					{
						this.FillLINChannelComboBox(comboBoxEdit);
					}
				}
			}
		}

		private void ShownEditorAction()
		{
			ComboBoxEdit comboBoxEdit = this.gridViewFilters.ActiveEditor as ComboBoxEdit;
			Filter filter;
			if (comboBoxEdit != null && this.GetFilter(this.gridViewFilters.GetFocusedDataSourceRowIndex(), out filter) && (filter is DefaultFilter || filter is ChannelFilter || !this.ModelValidator.LoggerSpecifics.Recording.IsLimitFilterSupported))
			{
				object obj = null;
				foreach (object current in comboBoxEdit.Properties.Items)
				{
					if (comboBoxEdit.Properties.Items.GetItemDescription(current) == Resources.FilterActionLimit)
					{
						obj = current;
						break;
					}
				}
				if (obj != null)
				{
					comboBoxEdit.Properties.Items.Remove(obj);
				}
			}
		}

		private void ShownEditorLogger()
		{
			ComboBoxEdit comboBoxEdit = this.gridViewFilters.ActiveEditor as ComboBoxEdit;
			if (comboBoxEdit != null)
			{
				this.FillMemoryComboBox(comboBoxEdit);
			}
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
			flag &= this.ModelValidator.Validate(this.filterConfiguration, isDataChanged, this.pageValidator);
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
			PageValidatorGridUtil.StoreRowMappingForVisibleRows(this.gridViewFilters, new PageValidatorGridUtil.StoreRowMappingHandler(this.OnPageValidatorGridUtilStoreRowMapping));
		}

		private void OnPageValidatorGridUtilStoreRowMapping(int dataSourceRowIdx)
		{
			if (dataSourceRowIdx < 0 || dataSourceRowIdx >= this.filterConfiguration.Filters.Count)
			{
				return;
			}
			Filter filter = this.filterConfiguration.Filters[dataSourceRowIdx];
			this.StoreMapping4VisibleColumns(filter, dataSourceRowIdx);
		}

		private void StoreMapping4VisibleColumns(Filter filter, int dataSourceIdx)
		{
			if (PageValidatorGridUtil.IsColumnVisible(this.colChannel, this.gridViewFilters))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colChannel, dataSourceIdx);
				if (filter is ChannelFilter)
				{
					this.pageValidator.Grid.StoreMapping((filter as ChannelFilter).ChannelNumber, gUIElement);
				}
				else if (filter is CANIdFilter)
				{
					this.pageValidator.Grid.StoreMapping((filter as CANIdFilter).ChannelNumber, gUIElement);
				}
				else if (filter is LINIdFilter)
				{
					this.pageValidator.Grid.StoreMapping((filter as LINIdFilter).ChannelNumber, gUIElement);
				}
				else if (filter is FlexrayIdFilter)
				{
					this.pageValidator.Grid.StoreMapping((filter as FlexrayIdFilter).ChannelNumber, gUIElement);
				}
				else if (filter is SymbolicMessageFilter)
				{
					this.pageValidator.Grid.StoreMapping((filter as SymbolicMessageFilter).ChannelNumber, gUIElement);
				}
				else if (filter is SignalListFileFilter)
				{
					this.pageValidator.Grid.StoreMapping((filter as SignalListFileFilter).ChannelNumber, gUIElement);
				}
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colCondition, this.gridViewFilters))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colCondition, dataSourceIdx);
				IEnumerable<IValidatedProperty> validatedPropertyListCondition = this.GetValidatedPropertyListCondition(filter);
				foreach (IValidatedProperty current in validatedPropertyListCondition)
				{
					this.pageValidator.Grid.StoreMapping(current, gUIElement);
				}
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colAction, this.gridViewFilters))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colAction, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(filter.Action, gUIElement);
			}
		}

		private void FillCANChannelComboBox(ComboBoxEdit comboBoxEdit)
		{
			comboBoxEdit.Properties.Items.Clear();
			for (uint num = 1u; num <= this.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN); num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
		}

		private void FillLINChannelComboBox(ComboBoxEdit comboBoxEdit)
		{
			comboBoxEdit.Properties.Items.Clear();
			for (uint num = 1u; num <= this.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_LIN); num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapLINChannelNumber2String(num, this.ModelValidator.LoggerSpecifics));
			}
		}

		private void FillFlexrayChannelComboBox(ComboBoxEdit comboBoxEdit)
		{
			comboBoxEdit.Properties.Items.Clear();
			for (uint num = 1u; num <= this.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_FlexRay); num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapFlexrayChannelNumber2String(num));
			}
		}

		private void FillMemoryComboBox(ComboBoxEdit comboBoxEdit)
		{
			comboBoxEdit.Properties.Items.Clear();
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories; num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapMemoryNumber2String(num));
			}
		}

		public bool TryGetSelectedFilter(out Filter filter)
		{
			int num;
			return this.TryGetSelectedFilter(out filter, out num);
		}

		public bool TryGetSelectedFilter(out Filter filter, out int idx)
		{
			filter = null;
			idx = this.gridViewFilters.GetFocusedDataSourceRowIndex();
			if (idx < 0 || idx > this.filterConfiguration.Filters.Count - 1)
			{
				return false;
			}
			filter = this.filterConfiguration.Filters[idx];
			return null != filter;
		}

		public bool TryGetFilterByIndex(out Filter filter, int idx)
		{
			filter = null;
			if (this.filterConfiguration == null)
			{
				return false;
			}
			if (idx < 0 || idx > this.filterConfiguration.Filters.Count - 1)
			{
				return false;
			}
			filter = this.filterConfiguration.Filters[idx];
			return null != filter;
		}

		public void SelectRowOfFilter(Filter filter)
		{
			for (int i = 0; i < this.gridViewFilters.RowCount; i++)
			{
				IList<Filter> list = this.gridViewFilters.DataSource as IList<Filter>;
				if (list != null)
				{
					Filter filter2 = list[this.gridViewFilters.GetDataSourceRowIndex(i)];
					if (filter2 == filter)
					{
						this.gridViewFilters.FocusedRowHandle = i;
						return;
					}
				}
			}
		}

		private bool GetFilter(int listSourceRowIndex, out Filter filter)
		{
			filter = null;
			if (listSourceRowIndex < 0 || listSourceRowIndex > this.filterConfiguration.Filters.Count - 1)
			{
				return false;
			}
			filter = this.filterConfiguration.Filters[listSourceRowIndex];
			return null != filter;
		}

		public bool Serialize(FiltersPage filtersPage)
		{
			if (filtersPage == null)
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.gridViewFilters.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				filtersPage.FiltersGridLayout = Convert.ToBase64String(array, 0, array.Length);
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

		public bool DeSerialize(FiltersPage filtersPage)
		{
			if (filtersPage == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(filtersPage.FiltersGridLayout))
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(filtersPage.FiltersGridLayout);
				memoryStream = new MemoryStream(buffer);
				this.gridViewFilters.RestoreLayoutFromStream(memoryStream);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FilterGrid));
			this.gridControlFilters = new GridControl();
			this.filterBindingSource = new BindingSource(this.components);
			this.gridViewFilters = new GridView();
			this.colActive = new GridColumn();
			this.repositoryItemCheckEditIsActive = new RepositoryItemCheckEdit();
			this.colAction = new GridColumn();
			this.repositoryItemImageComboBoxAction = new RepositoryItemImageComboBox();
			this.imageList = new ImageList(this.components);
			this.colRate = new GridColumn();
			this.repositoryItemButtonEditRate = new RepositoryItemButtonEdit();
			this.colType = new GridColumn();
			this.colChannel = new GridColumn();
			this.repositoryItemComboBoxChannel = new RepositoryItemComboBox();
			this.colCondition = new GridColumn();
			this.repositoryItemButtonEditCondition = new RepositoryItemButtonEdit();
			this.repositoryItemComboBoxLogger = new RepositoryItemComboBox();
			this.repositoryItemTextEditDummy = new RepositoryItemTextEdit();
			this.repositoryItemCheckEditDummy = new RepositoryItemCheckEdit();
			this.toolTipController = new XtraToolTipController(this.components);
			this.buttonMoveFirst = new Button();
			this.buttonMoveUp = new Button();
			this.buttonMoveDown = new Button();
			this.buttonMoveLast = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.gridControlFilters).BeginInit();
			((ISupportInitialize)this.filterBindingSource).BeginInit();
			((ISupportInitialize)this.gridViewFilters).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsActive).BeginInit();
			((ISupportInitialize)this.repositoryItemImageComboBoxAction).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEditRate).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBoxChannel).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEditCondition).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBoxLogger).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditDummy).BeginInit();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.gridControlFilters, "gridControlFilters");
			this.gridControlFilters.DataSource = this.filterBindingSource;
			this.gridControlFilters.MainView = this.gridViewFilters;
			this.gridControlFilters.Name = "gridControlFilters";
			this.gridControlFilters.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemComboBoxChannel,
				this.repositoryItemComboBoxLogger,
				this.repositoryItemImageComboBoxAction,
				this.repositoryItemButtonEditCondition,
				this.repositoryItemTextEditDummy,
				this.repositoryItemCheckEditIsActive,
				this.repositoryItemCheckEditDummy,
				this.repositoryItemButtonEditRate
			});
			this.gridControlFilters.ToolTipController = this.toolTipController;
			this.gridControlFilters.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewFilters
			});
			this.filterBindingSource.DataSource = typeof(Filter);
			this.gridViewFilters.Columns.AddRange(new GridColumn[]
			{
				this.colActive,
				this.colAction,
				this.colRate,
				this.colType,
				this.colChannel,
				this.colCondition
			});
			this.gridViewFilters.GridControl = this.gridControlFilters;
			this.gridViewFilters.Name = "gridViewFilters";
			this.gridViewFilters.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
			this.gridViewFilters.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
			this.gridViewFilters.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;
			this.gridViewFilters.OptionsCustomization.AllowFilter = false;
			this.gridViewFilters.OptionsCustomization.AllowGroup = false;
			this.gridViewFilters.OptionsCustomization.AllowQuickHideColumns = false;
			this.gridViewFilters.OptionsCustomization.AllowSort = false;
			this.gridViewFilters.OptionsView.ShowGroupPanel = false;
			this.gridViewFilters.OptionsView.ShowIndicator = false;
			this.gridViewFilters.PaintStyleName = "WindowsXP";
			this.gridViewFilters.RowHeight = 20;
			this.gridViewFilters.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewFilters_CustomDrawCell);
			this.gridViewFilters.RowStyle += new RowStyleEventHandler(this.gridViewFilters_RowStyle);
			this.gridViewFilters.CustomRowCellEdit += new CustomRowCellEditEventHandler(this.gridViewFilters_CustomRowCellEdit);
			this.gridViewFilters.LeftCoordChanged += new EventHandler(this.gridViewFilters_LeftCoordChanged);
			this.gridViewFilters.TopRowChanged += new EventHandler(this.gridViewFilters_TopRowChanged);
			this.gridViewFilters.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewFilters_PopupMenuShowing);
			this.gridViewFilters.ShowingEditor += new CancelEventHandler(this.gridViewFilters_ShowingEditor);
			this.gridViewFilters.ShownEditor += new EventHandler(this.gridViewFilters_ShownEditor);
			this.gridViewFilters.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gridViewFilters_FocusedRowChanged);
			this.gridViewFilters.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewFilters_CustomUnboundColumnData);
			this.gridViewFilters.KeyDown += new KeyEventHandler(this.gridViewFilters_KeyDown);
			componentResourceManager.ApplyResources(this.colActive, "colActive");
			this.colActive.ColumnEdit = this.repositoryItemCheckEditIsActive;
			this.colActive.FieldName = "anyBoolean1";
			this.colActive.Name = "colActive";
			this.colActive.UnboundType = UnboundColumnType.Boolean;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditIsActive, "repositoryItemCheckEditIsActive");
			this.repositoryItemCheckEditIsActive.Name = "repositoryItemCheckEditIsActive";
			this.repositoryItemCheckEditIsActive.CheckedChanged += new EventHandler(this.repositoryItemCheckEditIsActive_CheckedChanged);
			componentResourceManager.ApplyResources(this.colAction, "colAction");
			this.colAction.ColumnEdit = this.repositoryItemImageComboBoxAction;
			this.colAction.FieldName = "anyString1";
			this.colAction.Name = "colAction";
			this.colAction.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemImageComboBoxAction, "repositoryItemImageComboBoxAction");
			this.repositoryItemImageComboBoxAction.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemImageComboBoxAction.Buttons"))
			});
			this.repositoryItemImageComboBoxAction.Items.AddRange(new ImageComboBoxItem[]
			{
				new ImageComboBoxItem(componentResourceManager.GetString("repositoryItemImageComboBoxAction.Items"), componentResourceManager.GetString("repositoryItemImageComboBoxAction.Items1"), (int)componentResourceManager.GetObject("repositoryItemImageComboBoxAction.Items2")),
				new ImageComboBoxItem(componentResourceManager.GetString("repositoryItemImageComboBoxAction.Items3"), componentResourceManager.GetString("repositoryItemImageComboBoxAction.Items4"), (int)componentResourceManager.GetObject("repositoryItemImageComboBoxAction.Items5")),
				new ImageComboBoxItem(componentResourceManager.GetString("repositoryItemImageComboBoxAction.Items6"), componentResourceManager.GetString("repositoryItemImageComboBoxAction.Items7"), (int)componentResourceManager.GetObject("repositoryItemImageComboBoxAction.Items8"))
			});
			this.repositoryItemImageComboBoxAction.Name = "repositoryItemImageComboBoxAction";
			this.repositoryItemImageComboBoxAction.SmallImages = this.imageList;
			this.repositoryItemImageComboBoxAction.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBox_SelectedIndexChanged);
			this.imageList.ImageStream = (ImageListStreamer)componentResourceManager.GetObject("imageList.ImageStream");
			this.imageList.TransparentColor = Color.Transparent;
			this.imageList.Images.SetKeyName(0, "ImagePassFilter.png");
			this.imageList.Images.SetKeyName(1, "ImageLimitFilter.png");
			this.imageList.Images.SetKeyName(2, "ImageBlockFilter.png");
			componentResourceManager.ApplyResources(this.colRate, "colRate");
			this.colRate.ColumnEdit = this.repositoryItemButtonEditRate;
			this.colRate.FieldName = "anyString2";
			this.colRate.Name = "colRate";
			this.colRate.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonEditRate, "repositoryItemButtonEditRate");
			this.repositoryItemButtonEditRate.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEditRate.Name = "repositoryItemButtonEditRate";
			this.repositoryItemButtonEditRate.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEditRate.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEditRate_ButtonClick);
			componentResourceManager.ApplyResources(this.colType, "colType");
			this.colType.FieldName = "anyString3";
			this.colType.Name = "colType";
			this.colType.OptionsColumn.AllowEdit = false;
			this.colType.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colChannel, "colChannel");
			this.colChannel.ColumnEdit = this.repositoryItemComboBoxChannel;
			this.colChannel.FieldName = "anyString4";
			this.colChannel.Name = "colChannel";
			this.colChannel.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemComboBoxChannel, "repositoryItemComboBoxChannel");
			this.repositoryItemComboBoxChannel.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBoxChannel.Buttons"))
			});
			this.repositoryItemComboBoxChannel.Name = "repositoryItemComboBoxChannel";
			this.repositoryItemComboBoxChannel.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBoxChannel.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBox_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.colCondition, "colCondition");
			this.colCondition.ColumnEdit = this.repositoryItemButtonEditCondition;
			this.colCondition.FieldName = "anyString5";
			this.colCondition.Name = "colCondition";
			this.colCondition.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonEditCondition, "repositoryItemButtonEditCondition");
			this.repositoryItemButtonEditCondition.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEditCondition.Name = "repositoryItemButtonEditCondition";
			this.repositoryItemButtonEditCondition.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEditCondition.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEditCondition_ButtonClick);
			componentResourceManager.ApplyResources(this.repositoryItemComboBoxLogger, "repositoryItemComboBoxLogger");
			this.repositoryItemComboBoxLogger.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBoxLogger.Buttons"))
			});
			this.repositoryItemComboBoxLogger.Name = "repositoryItemComboBoxLogger";
			this.repositoryItemComboBoxLogger.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBoxLogger.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBox_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.repositoryItemTextEditDummy, "repositoryItemTextEditDummy");
			this.repositoryItemTextEditDummy.Name = "repositoryItemTextEditDummy";
			this.repositoryItemCheckEditDummy.AllowGrayed = true;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditDummy, "repositoryItemCheckEditDummy");
			this.repositoryItemCheckEditDummy.Name = "repositoryItemCheckEditDummy";
			this.repositoryItemCheckEditDummy.NullStyle = StyleIndeterminate.Inactive;
			this.repositoryItemCheckEditDummy.ReadOnly = true;
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
			base.Controls.Add(this.gridControlFilters);
			base.Name = "FilterGrid";
			componentResourceManager.ApplyResources(this, "$this");
			base.Resize += new EventHandler(this.FilterGrid_Resize);
			((ISupportInitialize)this.gridControlFilters).EndInit();
			((ISupportInitialize)this.filterBindingSource).EndInit();
			((ISupportInitialize)this.gridViewFilters).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsActive).EndInit();
			((ISupportInitialize)this.repositoryItemImageComboBoxAction).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEditRate).EndInit();
			((ISupportInitialize)this.repositoryItemComboBoxChannel).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEditCondition).EndInit();
			((ISupportInitialize)this.repositoryItemComboBoxLogger).EndInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditDummy).EndInit();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
