using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Mask;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.CardReaderPage
{
	internal class CardReaderGrid : UserControl
	{
		private delegate void GridRowContextMenuHandler(object sender, EventArgs e);

		private IHardwareFrontend hardwareFrontend;

		private ILoggerDevice currentDevice;

		private string destinationFolder;

		private bool destFolderContainsSubFolderPrimary;

		private bool destFolderContainsSubFolderSecondary;

		private IContainer components;

		private GridControl gridControlCardReader;

		private GridView gridViewCardReader;

		private GridColumn colFilename;

		private GridColumn colSize;

		private GridColumn colType;

		private GridColumn colDateTimeModified;

		private XtraToolTipController toolTipController;

		private RepositoryItemHyperLinkEdit repositoryItemHyperLinkEdit1;

		private GridColumn colConvert;

		private RepositoryItemCheckEdit checkEditFileSelect;

		public event EventHandler DeleteAllFiles;

		public event EventHandler FileIsSelectedChanged;

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public ILoggerDevice CurrentDevice
		{
			get
			{
				return this.currentDevice;
			}
			set
			{
				this.currentDevice = value;
			}
		}

		public bool IsCurrentDeviceCustomDirectory
		{
			get;
			set;
		}

		public string DestinationFolder
		{
			get
			{
				return this.destinationFolder;
			}
			set
			{
				this.destinationFolder = value;
				this.UpdateDestFolderContainsSubFolderIndicators();
			}
		}

		public IHardwareFrontend HardwareFrontend
		{
			get
			{
				return this.hardwareFrontend;
			}
			set
			{
				this.hardwareFrontend = value;
			}
		}

		public CardReaderGrid()
		{
			this.InitializeComponent();
			this.destFolderContainsSubFolderPrimary = false;
			this.currentDevice = null;
			this.IsCurrentDeviceCustomDirectory = true;
		}

		private void Raise_DeleteAllFiles(object sender, EventArgs e)
		{
			if (this.DeleteAllFiles != null)
			{
				this.DeleteAllFiles(sender, e);
			}
		}

		private void Raise_FileIsSelectedChanged(object sender, EventArgs e)
		{
			if (this.FileIsSelectedChanged != null)
			{
				this.FileIsSelectedChanged(sender, e);
			}
		}

		public bool DisplayFileList()
		{
			if (this.ModelValidator != null && this.ModelValidator.LoggerSpecifics.FileConversion.HasSelectableLogFiles)
			{
				this.colConvert.Visible = true;
				this.colConvert.VisibleIndex = 0;
			}
			else
			{
				this.colConvert.Visible = false;
			}
			bool flag = false;
			if (this.currentDevice != null)
			{
				this.gridControlCardReader.DataSource = this.currentDevice.LogFileStorage.LogFiles;
				this.UpdateDestFolderContainsSubFolderIndicators();
				flag = true;
			}
			if (!flag)
			{
				this.gridControlCardReader.DataSource = null;
			}
			this.gridViewCardReader.RefreshData();
			return flag;
		}

		private void gridViewCardReader_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			ILogFile item;
			if (!this.TryGetLogFileByIndex(out item, e.ListSourceRowIndex))
			{
				return;
			}
			if (e.Column == this.colFilename)
			{
				this.UnboundColumnFileName(item, e);
				return;
			}
			if (e.Column == this.colConvert)
			{
				this.UnboundColumnConvert(item, e);
				return;
			}
			if (e.Column == this.colSize)
			{
				this.UnboundColumnFileSize(item, e);
				return;
			}
			if (e.Column == this.colDateTimeModified)
			{
				this.UnboundColumnDateTimeModified(item, e);
				return;
			}
			if (e.Column == this.colType)
			{
				this.UnboundColumnType(item, e);
			}
		}

		private void gridViewCardReader_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
		{
			if (e.Column == this.colFilename)
			{
				ILogFile logFile = (ILogFile)e.RowObject1;
				ILogFile logFile2 = (ILogFile)e.RowObject2;
				e.Result = FileSystemServices.NaturalCompare(logFile.DefaultName, logFile2.DefaultName);
				e.Handled = true;
				return;
			}
			if (e.Column == this.colSize)
			{
				ILogFile logFile3 = (ILogFile)e.RowObject1;
				ILogFile logFile4 = (ILogFile)e.RowObject2;
				if (logFile3.FileSize < logFile4.FileSize)
				{
					e.Result = -1;
				}
				else if (logFile3.FileSize > logFile4.FileSize)
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
			if (e.Column == this.colDateTimeModified)
			{
				ILogFile logFile5 = (ILogFile)e.RowObject1;
				ILogFile logFile6 = (ILogFile)e.RowObject2;
				e.Result = logFile5.Timestamp.CompareTo(logFile6.Timestamp);
				e.Handled = true;
				return;
			}
			e.Handled = false;
		}

		private void gridViewCardReader_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (e.MenuType == GridMenuType.Column)
			{
				this.CustomizeColumnHeaderMenu(e);
			}
			if (e.MenuType == GridMenuType.Row && this.currentDevice.LoggerSpecifics.FileConversion.HasSelectableLogFiles)
			{
				this.DisplayGridRowContextMenu(e);
			}
		}

		private void gridViewCardReader_DoubleClick(object sender, EventArgs e)
		{
			if (this.gridViewCardReader.FocusedColumn != this.colFilename)
			{
				return;
			}
			if (!this.ModelValidator.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
			{
				return;
			}
			ILogFile logFile;
			if (this.TryGetSelectedLogFileItem(out logFile))
			{
				string extension = Path.GetExtension(logFile.DefaultName);
				if (string.Compare(Vocabulary.FileExtensionDotINI, extension, true) == 0 || string.Compare(Vocabulary.FileExtensionDotLTL, extension, true) == 0)
				{
					FileSystemServices.LaunchFile(Path.Combine(this.currentDevice.HardwareKey, logFile.DefaultName));
				}
			}
		}

		private void checkEditFileSelect_CheckedChanged(object sender, EventArgs e)
		{
			this.gridViewCardReader.PostEditor();
		}

		private void gridViewCardReader_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			if (e.Column == this.colConvert)
			{
				CheckEditViewInfo checkEditViewInfo = ((GridCellInfo)e.Cell).ViewInfo as CheckEditViewInfo;
				if (checkEditViewInfo != null)
				{
					ILogFile logFile;
					if (this.TryGetLogFileByIndex(out logFile, this.gridViewCardReader.GetDataSourceRowIndex(e.RowHandle)) && logFile.IsConvertible)
					{
						checkEditViewInfo.AllowOverridedState = true;
						checkEditViewInfo.OverridedState = ObjectState.Normal;
						checkEditViewInfo.CalcViewInfo(e.Graphics);
						return;
					}
					checkEditViewInfo.AllowOverridedState = true;
					checkEditViewInfo.OverridedState = ObjectState.Disabled;
					checkEditViewInfo.CalcViewInfo(e.Graphics);
				}
			}
		}

		private void gridViewCardReader_ShowingEditor(object sender, CancelEventArgs e)
		{
			ILogFile logFile;
			if (!this.TryGetSelectedLogFileItem(out logFile) || !logFile.IsConvertible)
			{
				e.Cancel = true;
			}
		}

		private void UnboundColumnFileName(ILogFile item, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = item.DefaultName;
			}
		}

		private void UnboundColumnConvert(ILogFile item, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = (item.IsSelected && item.IsConvertible);
				return;
			}
			item.IsSelected = (bool)e.Value;
			this.Raise_FileIsSelectedChanged(this, EventArgs.Empty);
		}

		private void UnboundColumnFileSize(ILogFile item, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = GUIUtil.MapFileSizeNumber2String((long)((ulong)item.FileSize));
			}
		}

		private void UnboundColumnDateTimeModified(ILogFile item, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (item.Timestamp.Year >= Constants.EarliestValidLogDateYear)
				{
					e.Value = item.Timestamp;
					return;
				}
				e.Value = "";
			}
		}

		private void UnboundColumnType(ILogFile item, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = item.TypeName;
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
			for (int i = e.Menu.Items.Count - 1; i >= 0; i--)
			{
				string caption = e.Menu.Items[i].Caption;
				if (localizedString5 == caption || localizedString6 == caption || caption.Contains(localizedString) || localizedString2 == caption || localizedString3 == caption || localizedString4 == caption || localizedString7 == caption)
				{
					e.Menu.Items.RemoveAt(i);
				}
			}
		}

		private void DisplayGridRowContextMenu(PopupMenuShowingEventArgs e)
		{
			int arg_0B_0 = e.HitInfo.RowHandle;
			e.Menu.Items.Clear();
			e.Menu.Items.Add(this.CreateMenuItem(Resources.FileManagerContextMenuEnableSelected, new CardReaderGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuSelectMarked), false));
			e.Menu.Items.Add(this.CreateMenuItem(Resources.FileManagerContextMenuDisableSelected, new CardReaderGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuDeselectMarked), false));
			e.Menu.Items.Add(this.CreateMenuItem(Resources.FileManagerContextMenuEnableAll, new CardReaderGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuSelectAll), true));
			e.Menu.Items.Add(this.CreateMenuItem(Resources.FileManagerContextMenuDisableAll, new CardReaderGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuDeselectAll), false));
			if (!this.IsCurrentDeviceCustomDirectory)
			{
				e.Menu.Items.Add(this.CreateMenuItem(Resources.FileManagerContextMenuDeleteAll, new CardReaderGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuDeleteAll), true));
			}
		}

		private DXMenuItem CreateMenuItem(string caption, CardReaderGrid.GridRowContextMenuHandler target, bool isBeginOfGroup)
		{
			return new DXMenuItem(caption, new EventHandler(target.Invoke))
			{
				BeginGroup = isBeginOfGroup
			};
		}

		private void OnGridRowContextMenuDeleteAll(object sender, EventArgs e)
		{
			this.Raise_DeleteAllFiles(this, EventArgs.Empty);
		}

		private void OnGridRowContextMenuSelectAll(object sender, EventArgs e)
		{
			this.SellectAllFiles(true);
			this.gridViewCardReader.RefreshData();
			this.Raise_FileIsSelectedChanged(this, EventArgs.Empty);
		}

		private void OnGridRowContextMenuDeselectAll(object sender, EventArgs e)
		{
			this.SellectAllFiles(false);
			this.gridViewCardReader.RefreshData();
			this.Raise_FileIsSelectedChanged(this, EventArgs.Empty);
		}

		private void OnGridRowContextMenuSelectMarked(object sender, EventArgs e)
		{
			int[] selectedRows = this.gridViewCardReader.GetSelectedRows();
			for (int i = 0; i < selectedRows.Count<int>(); i++)
			{
				ILogFile logFile;
				if (this.TryGetLogFileByIndex(out logFile, this.gridViewCardReader.GetDataSourceRowIndex(selectedRows[i])))
				{
					logFile.IsSelected = logFile.IsConvertible;
				}
			}
			this.gridViewCardReader.RefreshData();
			this.Raise_FileIsSelectedChanged(this, EventArgs.Empty);
		}

		private void OnGridRowContextMenuDeselectMarked(object sender, EventArgs e)
		{
			int[] selectedRows = this.gridViewCardReader.GetSelectedRows();
			for (int i = 0; i < selectedRows.Count<int>(); i++)
			{
				ILogFile logFile;
				if (this.TryGetLogFileByIndex(out logFile, this.gridViewCardReader.GetDataSourceRowIndex(selectedRows[i])))
				{
					logFile.IsSelected = false;
				}
			}
			this.gridViewCardReader.RefreshData();
			this.Raise_FileIsSelectedChanged(this, EventArgs.Empty);
		}

		private bool TryGetSelectedLogFileItem(out ILogFile item)
		{
			int num;
			return this.TryGetSelectedLogFileItem(out item, out num);
		}

		private bool TryGetSelectedLogFileItem(out ILogFile item, out int idx)
		{
			item = null;
			idx = this.gridViewCardReader.GetFocusedDataSourceRowIndex();
			if (this.currentDevice == null || idx < 0 || idx > this.currentDevice.LogFileStorage.LogFiles.Count - 1)
			{
				return false;
			}
			item = this.currentDevice.LogFileStorage.LogFiles[idx];
			return null != item;
		}

		public bool TryGetLogFileByIndex(out ILogFile logFile, int rowIndex)
		{
			logFile = null;
			if (rowIndex < 0 || this.currentDevice == null || rowIndex >= this.currentDevice.LogFileStorage.LogFiles.Count)
			{
				return false;
			}
			logFile = this.currentDevice.LogFileStorage.LogFiles[rowIndex];
			return null != logFile;
		}

		private void SellectAllFiles(bool select)
		{
			foreach (ILogFile current in this.currentDevice.LogFileStorage.LogFiles)
			{
				current.IsSelected = (current.IsConvertible & select);
			}
		}

		private void UpdateDestFolderContainsSubFolderIndicators()
		{
			this.destFolderContainsSubFolderPrimary = false;
			this.destFolderContainsSubFolderSecondary = false;
			if (this.currentDevice != null && !string.IsNullOrEmpty(this.destinationFolder))
			{
				if (!string.IsNullOrEmpty(this.currentDevice.LogFileStorage.DestSubFolderNamePrimary))
				{
					this.destFolderContainsSubFolderPrimary = Directory.Exists(Path.Combine(this.destinationFolder, this.currentDevice.LogFileStorage.DestSubFolderNamePrimary));
				}
				if (!string.IsNullOrEmpty(this.currentDevice.LogFileStorage.DestSubFolderNameSecondary))
				{
					this.destFolderContainsSubFolderSecondary = Directory.Exists(Path.Combine(this.destinationFolder, this.currentDevice.LogFileStorage.DestSubFolderNameSecondary));
				}
				this.gridViewCardReader.RefreshData();
			}
		}

		public bool Serialize(CardReaderPage cardReaderPage)
		{
			if (cardReaderPage == null)
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.gridViewCardReader.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				cardReaderPage.CardReaderGridLayout = Convert.ToBase64String(array, 0, array.Length);
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

		public bool DeSerialize(CardReaderPage cardReaderPage)
		{
			if (cardReaderPage == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(cardReaderPage.CardReaderGridLayout))
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(cardReaderPage.CardReaderGridLayout);
				memoryStream = new MemoryStream(buffer);
				this.gridViewCardReader.RestoreLayoutFromStream(memoryStream);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CardReaderGrid));
			this.gridControlCardReader = new GridControl();
			this.gridViewCardReader = new GridView();
			this.colConvert = new GridColumn();
			this.checkEditFileSelect = new RepositoryItemCheckEdit();
			this.colFilename = new GridColumn();
			this.colSize = new GridColumn();
			this.colType = new GridColumn();
			this.colDateTimeModified = new GridColumn();
			this.repositoryItemHyperLinkEdit1 = new RepositoryItemHyperLinkEdit();
			this.toolTipController = new XtraToolTipController();
			((ISupportInitialize)this.gridControlCardReader).BeginInit();
			((ISupportInitialize)this.gridViewCardReader).BeginInit();
			((ISupportInitialize)this.checkEditFileSelect).BeginInit();
			((ISupportInitialize)this.repositoryItemHyperLinkEdit1).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.gridControlCardReader, "gridControlCardReader");
			this.gridControlCardReader.EmbeddedNavigator.AccessibleDescription = componentResourceManager.GetString("gridControlCardReader.EmbeddedNavigator.AccessibleDescription");
			this.gridControlCardReader.EmbeddedNavigator.AccessibleName = componentResourceManager.GetString("gridControlCardReader.EmbeddedNavigator.AccessibleName");
			this.gridControlCardReader.EmbeddedNavigator.AllowHtmlTextInToolTip = (DefaultBoolean)componentResourceManager.GetObject("gridControlCardReader.EmbeddedNavigator.AllowHtmlTextInToolTip");
			this.gridControlCardReader.EmbeddedNavigator.Anchor = (AnchorStyles)componentResourceManager.GetObject("gridControlCardReader.EmbeddedNavigator.Anchor");
			this.gridControlCardReader.EmbeddedNavigator.BackgroundImage = (Image)componentResourceManager.GetObject("gridControlCardReader.EmbeddedNavigator.BackgroundImage");
			this.gridControlCardReader.EmbeddedNavigator.BackgroundImageLayout = (ImageLayout)componentResourceManager.GetObject("gridControlCardReader.EmbeddedNavigator.BackgroundImageLayout");
			this.gridControlCardReader.EmbeddedNavigator.ImeMode = (ImeMode)componentResourceManager.GetObject("gridControlCardReader.EmbeddedNavigator.ImeMode");
			this.gridControlCardReader.EmbeddedNavigator.MaximumSize = (Size)componentResourceManager.GetObject("gridControlCardReader.EmbeddedNavigator.MaximumSize");
			this.gridControlCardReader.EmbeddedNavigator.TextLocation = (NavigatorButtonsTextLocation)componentResourceManager.GetObject("gridControlCardReader.EmbeddedNavigator.TextLocation");
			this.gridControlCardReader.EmbeddedNavigator.ToolTip = componentResourceManager.GetString("gridControlCardReader.EmbeddedNavigator.ToolTip");
			this.gridControlCardReader.EmbeddedNavigator.ToolTipIconType = (ToolTipIconType)componentResourceManager.GetObject("gridControlCardReader.EmbeddedNavigator.ToolTipIconType");
			this.gridControlCardReader.EmbeddedNavigator.ToolTipTitle = componentResourceManager.GetString("gridControlCardReader.EmbeddedNavigator.ToolTipTitle");
			this.gridControlCardReader.MainView = this.gridViewCardReader;
			this.gridControlCardReader.Name = "gridControlCardReader";
			this.gridControlCardReader.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemHyperLinkEdit1,
				this.checkEditFileSelect
			});
			this.gridControlCardReader.ToolTipController = this.toolTipController;
			this.gridControlCardReader.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewCardReader
			});
			this.gridControlCardReader.DoubleClick += new EventHandler(this.gridViewCardReader_DoubleClick);
			componentResourceManager.ApplyResources(this.gridViewCardReader, "gridViewCardReader");
			this.gridViewCardReader.Columns.AddRange(new GridColumn[]
			{
				this.colConvert,
				this.colFilename,
				this.colSize,
				this.colType,
				this.colDateTimeModified
			});
			this.gridViewCardReader.GridControl = this.gridControlCardReader;
			this.gridViewCardReader.Name = "gridViewCardReader";
			this.gridViewCardReader.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
			this.gridViewCardReader.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
			this.gridViewCardReader.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;
			this.gridViewCardReader.OptionsCustomization.AllowColumnMoving = false;
			this.gridViewCardReader.OptionsCustomization.AllowFilter = false;
			this.gridViewCardReader.OptionsCustomization.AllowGroup = false;
			this.gridViewCardReader.OptionsCustomization.AllowQuickHideColumns = false;
			this.gridViewCardReader.OptionsView.ShowGroupPanel = false;
			this.gridViewCardReader.OptionsView.ShowIndicator = false;
			this.gridViewCardReader.PaintStyleName = "WindowsXP";
			this.gridViewCardReader.RowHeight = 20;
			this.gridViewCardReader.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.colFilename, ColumnSortOrder.Ascending)
			});
			this.gridViewCardReader.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewCardReader_CustomDrawCell);
			this.gridViewCardReader.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewCardReader_PopupMenuShowing);
			this.gridViewCardReader.ShowingEditor += new CancelEventHandler(this.gridViewCardReader_ShowingEditor);
			this.gridViewCardReader.CustomColumnSort += new CustomColumnSortEventHandler(this.gridViewCardReader_CustomColumnSort);
			this.gridViewCardReader.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewCardReader_CustomUnboundColumnData);
			componentResourceManager.ApplyResources(this.colConvert, "colConvert");
			this.colConvert.ColumnEdit = this.checkEditFileSelect;
			this.colConvert.FieldName = "colConvert";
			this.colConvert.Name = "colConvert";
			this.colConvert.Summary.AddRange(new GridSummaryItem[]
			{
				new GridColumnSummaryItem()
			});
			this.colConvert.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
			componentResourceManager.ApplyResources(this.checkEditFileSelect, "checkEditFileSelect");
			this.checkEditFileSelect.Name = "checkEditFileSelect";
			this.checkEditFileSelect.CheckedChanged += new EventHandler(this.checkEditFileSelect_CheckedChanged);
			componentResourceManager.ApplyResources(this.colFilename, "colFilename");
			this.colFilename.FieldName = "anyString1";
			this.colFilename.Name = "colFilename";
			this.colFilename.OptionsColumn.AllowEdit = false;
			this.colFilename.OptionsColumn.AllowSort = DefaultBoolean.True;
			this.colFilename.SortMode = ColumnSortMode.Custom;
			this.colFilename.UnboundType = DevExpress.Data.UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colSize, "colSize");
			this.colSize.FieldName = "anyString2";
			this.colSize.Name = "colSize";
			this.colSize.OptionsColumn.AllowEdit = false;
			this.colSize.OptionsColumn.AllowSort = DefaultBoolean.True;
			this.colSize.SortMode = ColumnSortMode.Custom;
			this.colSize.UnboundType = DevExpress.Data.UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colType, "colType");
			this.colType.FieldName = "anyString3";
			this.colType.Name = "colType";
			this.colType.OptionsColumn.AllowEdit = false;
			this.colType.OptionsColumn.AllowSort = DefaultBoolean.True;
			this.colType.UnboundType = DevExpress.Data.UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colDateTimeModified, "colDateTimeModified");
			this.colDateTimeModified.FieldName = "anyString4";
			this.colDateTimeModified.Name = "colDateTimeModified";
			this.colDateTimeModified.OptionsColumn.AllowEdit = false;
			this.colDateTimeModified.OptionsColumn.AllowSort = DefaultBoolean.True;
			this.colDateTimeModified.SortMode = ColumnSortMode.Custom;
			this.colDateTimeModified.UnboundType = DevExpress.Data.UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemHyperLinkEdit1, "repositoryItemHyperLinkEdit1");
			this.repositoryItemHyperLinkEdit1.Mask.AutoComplete = (AutoCompleteType)componentResourceManager.GetObject("repositoryItemHyperLinkEdit1.Mask.AutoComplete");
			this.repositoryItemHyperLinkEdit1.Mask.BeepOnError = (bool)componentResourceManager.GetObject("repositoryItemHyperLinkEdit1.Mask.BeepOnError");
			this.repositoryItemHyperLinkEdit1.Mask.EditMask = componentResourceManager.GetString("repositoryItemHyperLinkEdit1.Mask.EditMask");
			this.repositoryItemHyperLinkEdit1.Mask.IgnoreMaskBlank = (bool)componentResourceManager.GetObject("repositoryItemHyperLinkEdit1.Mask.IgnoreMaskBlank");
			this.repositoryItemHyperLinkEdit1.Mask.MaskType = (MaskType)componentResourceManager.GetObject("repositoryItemHyperLinkEdit1.Mask.MaskType");
			this.repositoryItemHyperLinkEdit1.Mask.PlaceHolder = (char)componentResourceManager.GetObject("repositoryItemHyperLinkEdit1.Mask.PlaceHolder");
			this.repositoryItemHyperLinkEdit1.Mask.SaveLiteral = (bool)componentResourceManager.GetObject("repositoryItemHyperLinkEdit1.Mask.SaveLiteral");
			this.repositoryItemHyperLinkEdit1.Mask.ShowPlaceHolders = (bool)componentResourceManager.GetObject("repositoryItemHyperLinkEdit1.Mask.ShowPlaceHolders");
			this.repositoryItemHyperLinkEdit1.Mask.UseMaskAsDisplayFormat = (bool)componentResourceManager.GetObject("repositoryItemHyperLinkEdit1.Mask.UseMaskAsDisplayFormat");
			this.repositoryItemHyperLinkEdit1.Name = "repositoryItemHyperLinkEdit1";
			this.toolTipController.Appearance.BackColor = (Color)componentResourceManager.GetObject("toolTipController.Appearance.BackColor");
			this.toolTipController.Appearance.ForeColor = (Color)componentResourceManager.GetObject("toolTipController.Appearance.ForeColor");
			this.toolTipController.Appearance.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("toolTipController.Appearance.GradientMode");
			this.toolTipController.Appearance.Image = (Image)componentResourceManager.GetObject("toolTipController.Appearance.Image");
			this.toolTipController.Appearance.Options.UseBackColor = true;
			this.toolTipController.Appearance.Options.UseForeColor = true;
			this.toolTipController.Appearance.Options.UseTextOptions = true;
			this.toolTipController.Appearance.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.toolTipController.AppearanceTitle.BackColor = (Color)componentResourceManager.GetObject("toolTipController.AppearanceTitle.BackColor");
			this.toolTipController.AppearanceTitle.ForeColor = (Color)componentResourceManager.GetObject("toolTipController.AppearanceTitle.ForeColor");
			this.toolTipController.AppearanceTitle.GradientMode = (LinearGradientMode)componentResourceManager.GetObject("toolTipController.AppearanceTitle.GradientMode");
			this.toolTipController.AppearanceTitle.Image = (Image)componentResourceManager.GetObject("toolTipController.AppearanceTitle.Image");
			this.toolTipController.AppearanceTitle.Options.UseBackColor = true;
			this.toolTipController.AppearanceTitle.Options.UseForeColor = true;
			this.toolTipController.AppearanceTitle.Options.UseTextOptions = true;
			this.toolTipController.AppearanceTitle.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.toolTipController.MaxWidth = 500;
			this.toolTipController.ShowPrefix = true;
			this.toolTipController.UseNativeLookAndFeel = true;
			componentResourceManager.ApplyResources(this, "$this");
			this.toolTipController.SetAllowHtmlText(this, (DefaultBoolean)componentResourceManager.GetObject("$this.AllowHtmlText"));
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.gridControlCardReader);
			base.Name = "CardReaderGrid";
			this.toolTipController.SetTitle(this, componentResourceManager.GetString("$this.Title"));
			this.toolTipController.SetToolTip(this, componentResourceManager.GetString("$this.ToolTip"));
			this.toolTipController.SetToolTipIconType(this, (ToolTipIconType)componentResourceManager.GetObject("$this.ToolTipIconType"));
			((ISupportInitialize)this.gridControlCardReader).EndInit();
			((ISupportInitialize)this.gridViewCardReader).EndInit();
			((ISupportInitialize)this.checkEditFileSelect).EndInit();
			((ISupportInitialize)this.repositoryItemHyperLinkEdit1).EndInit();
			base.ResumeLayout(false);
		}
	}
}
