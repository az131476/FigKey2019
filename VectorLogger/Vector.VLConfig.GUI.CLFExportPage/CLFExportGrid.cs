using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.CLFExportPage
{
	public class CLFExportGrid : UserControl
	{
		private delegate void GridRowContextMenuHandler(object sender, EventArgs e);

		private DisplayedFolderFileList folderFileList;

		private IContainer components;

		private GridControl gridControlCLFExport;

		private GridView gridViewCLFExport;

		private GridColumn colFilename;

		private GridColumn colFileSize;

		private GridColumn colType;

		private GridColumn colDateTimeModified;

		private GridColumn colDestinationFilename;

		private BindingSource displayFileItemBindingSource;

		private GridColumn colConvert;

		private RepositoryItemCheckEdit repositoryItemCheckEditIsEnabled;

		private XtraToolTipController toolTipController;

		public event EventHandler ItemIsEnabledChanged;

		public DisplayedFolderFileList DisplayedFolderFileList
		{
			get
			{
				return this.folderFileList;
			}
		}

		public ILoggerSpecifics LoggerSpecifics
		{
			get;
			set;
		}

		public CLFExportGrid()
		{
			this.InitializeComponent();
			this.folderFileList = new DisplayedFolderFileList();
			this.folderFileList.AddSourceFolderExtFilter("clf");
			this.gridControlCLFExport.DataSource = this.folderFileList.FileList;
		}

		private void Raise_ItemIsEnabledChanged(object sender, EventArgs e)
		{
			if (this.ItemIsEnabledChanged != null)
			{
				this.ItemIsEnabledChanged(sender, e);
			}
		}

		public int DisplayFileList(string sourceFolderPath)
		{
			int result = this.DisplayedFolderFileList.ReadSourceFolder(sourceFolderPath);
			this.gridViewCLFExport.RefreshData();
			this.Raise_ItemIsEnabledChanged(this, EventArgs.Empty);
			return result;
		}

		public void RefreshGridView()
		{
			this.gridViewCLFExport.RefreshData();
		}

		private void gridViewCLFExport_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (e.MenuType == GridMenuType.Column)
			{
				this.CustomizeColumnHeaderMenu(e);
			}
			if (e.MenuType == GridMenuType.Row)
			{
				this.DisplayGridRowContextMenu(e);
			}
		}

		private void gridViewCLFExport_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			DisplayedFileItem item;
			if (!this.GetDisplayedFileItem(e.ListSourceRowIndex, out item))
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
			if (e.Column == this.colFileSize)
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
				return;
			}
			if (e.Column == this.colDestinationFilename)
			{
				this.UnboundColumnDestinationFile(item, e);
			}
		}

		private void gridViewCLFExport_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			DisplayedFileItem displayedFileItem;
			if (e.Column == this.colDestinationFilename && this.GetDisplayedFileItem(this.gridViewCLFExport.GetDataSourceRowIndex(e.RowHandle), out displayedFileItem) && displayedFileItem.IsEnabled && displayedFileItem.IsDestinationFileExisting)
			{
				e.Appearance.BackColor = Color.Orange;
			}
		}

		private void gridViewCLFExport_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
		{
			if (e.Column == this.colFilename)
			{
				DisplayedFileItem displayedFileItem = (DisplayedFileItem)e.RowObject1;
				DisplayedFileItem displayedFileItem2 = (DisplayedFileItem)e.RowObject2;
				e.Result = FileSystemServices.NaturalCompare(displayedFileItem.Filename, displayedFileItem2.Filename);
				e.Handled = true;
				return;
			}
			if (e.Column == this.colFileSize)
			{
				DisplayedFileItem displayedFileItem3 = (DisplayedFileItem)e.RowObject1;
				DisplayedFileItem displayedFileItem4 = (DisplayedFileItem)e.RowObject2;
				if (displayedFileItem3.FileSize < displayedFileItem4.FileSize)
				{
					e.Result = -1;
				}
				else if (displayedFileItem3.FileSize > displayedFileItem4.FileSize)
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

		private void repositoryItemCheckEditIsEnabled_CheckedChanged(object sender, EventArgs e)
		{
			this.gridViewCLFExport.PostEditor();
		}

		private void UnboundColumnFileName(DisplayedFileItem item, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = item.Filename;
			}
		}

		private void UnboundColumnConvert(DisplayedFileItem item, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = item.IsEnabled;
				return;
			}
			if ((bool)e.Value)
			{
				this.folderFileList.EnableFileItem(item);
			}
			else
			{
				this.folderFileList.DisableFileItem(item);
			}
			this.Raise_ItemIsEnabledChanged(this, EventArgs.Empty);
		}

		private void UnboundColumnFileSize(DisplayedFileItem item, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = GUIUtil.MapFileSizeNumber2String(item.FileSize);
			}
		}

		private void UnboundColumnDateTimeModified(DisplayedFileItem item, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = item.DateTimeModified;
			}
		}

		private void UnboundColumnType(DisplayedFileItem item, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData && GUIUtil.HasFileExtension(item.Filename, "clf"))
			{
				e.Value = Resources.FileManagerColFileTypeLogData;
			}
		}

		private void UnboundColumnDestinationFile(DisplayedFileItem item, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData && item.IsEnabled)
			{
				e.Value = item.DestinationFilename;
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
			e.Menu.Items.Add(this.CreateMenuItem(Resources.FileManagerContextMenuEnableSelected, new CLFExportGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuEnableSelected), false));
			e.Menu.Items.Add(this.CreateMenuItem(Resources.FileManagerContextMenuDisableSelected, new CLFExportGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuDisableSelected), false));
			e.Menu.Items.Add(this.CreateMenuItem(Resources.FileManagerContextMenuEnableAll, new CLFExportGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuEnableAll), true));
			e.Menu.Items.Add(this.CreateMenuItem(Resources.FileManagerContextMenuDisableAll, new CLFExportGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuDisableAll), false));
			e.Menu.Items.Add(this.CreateMenuItem(Resources.FileManagerContextMenuShowMetaInformations, new CLFExportGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuShowMetaInfo), true));
			e.Menu.Items.Add(this.CreateMenuItem(Resources.FileManagerContextMenuRefresh, new CLFExportGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuRefresh), true));
		}

		private DXMenuItem CreateMenuItem(string caption, CLFExportGrid.GridRowContextMenuHandler target, bool isBeginOfGroup)
		{
			return new DXMenuItem(caption, new EventHandler(target.Invoke))
			{
				BeginGroup = isBeginOfGroup
			};
		}

		private void OnGridRowContextMenuEnableAll(object sender, EventArgs e)
		{
			this.DisplayedFolderFileList.EnableAllFiles();
			this.gridViewCLFExport.RefreshData();
			this.Raise_ItemIsEnabledChanged(this, EventArgs.Empty);
		}

		private void OnGridRowContextMenuDisableAll(object sender, EventArgs e)
		{
			this.DisplayedFolderFileList.DisableAllFiles();
			this.gridViewCLFExport.RefreshData();
			this.Raise_ItemIsEnabledChanged(this, EventArgs.Empty);
		}

		private void OnGridRowContextMenuEnableSelected(object sender, EventArgs e)
		{
			int[] selectedRows = this.gridViewCLFExport.GetSelectedRows();
			for (int i = 0; i < selectedRows.Count<int>(); i++)
			{
				DisplayedFileItem item;
				if (this.GetDisplayedFileItem(this.gridViewCLFExport.GetDataSourceRowIndex(selectedRows[i]), out item))
				{
					this.folderFileList.EnableFileItem(item);
				}
			}
			this.gridViewCLFExport.RefreshData();
			this.Raise_ItemIsEnabledChanged(this, EventArgs.Empty);
		}

		private void OnGridRowContextMenuDisableSelected(object sender, EventArgs e)
		{
			int[] selectedRows = this.gridViewCLFExport.GetSelectedRows();
			for (int i = 0; i < selectedRows.Count<int>(); i++)
			{
				DisplayedFileItem item;
				if (this.GetDisplayedFileItem(this.gridViewCLFExport.GetDataSourceRowIndex(selectedRows[i]), out item))
				{
					this.folderFileList.DisableFileItem(item);
				}
			}
			this.gridViewCLFExport.RefreshData();
			this.Raise_ItemIsEnabledChanged(this, EventArgs.Empty);
		}

		private void OnGridRowContextMenuShowMetaInfo(object sender, EventArgs e)
		{
			DisplayedFileItem displayedFileItem;
			if (this.GetDisplayedFileItem(this.gridViewCLFExport.GetDataSourceRowIndex(this.gridViewCLFExport.FocusedRowHandle), out displayedFileItem) && displayedFileItem != null)
			{
				if (displayedFileItem.MetaInformations == null)
				{
					CLFText cLFText = new CLFText();
					string str = cLFText.RetrieveMetaInfos(Path.Combine(this.folderFileList.SourceFolder, displayedFileItem.Filename));
					displayedFileItem.MetaInformations = "CLF file: " + displayedFileItem.Filename + Environment.NewLine + str;
				}
				MetaInfoDialog metaInfoDialog = new MetaInfoDialog(displayedFileItem.MetaInformations);
				metaInfoDialog.ShowDialog(this);
			}
		}

		private void OnGridRowContextMenuRefresh(object sender, EventArgs e)
		{
			this.DisplayedFolderFileList.ReadSourceFolder(this.DisplayedFolderFileList.SourceFolder);
			this.gridViewCLFExport.RefreshData();
		}

		private void toolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
		{
			ToolTipControlInfo info = null;
			try
			{
				GridHitInfo gridHitInfo = this.gridViewCLFExport.CalcHitInfo(e.ControlMousePosition);
				DisplayedFileItem displayedFileItem;
				if (gridHitInfo.InRowCell && this.GetDisplayedFileItem(this.gridViewCLFExport.GetDataSourceRowIndex(gridHitInfo.RowHandle), out displayedFileItem) && displayedFileItem.IsEnabled && displayedFileItem.IsDestinationFileExisting)
				{
					info = new ToolTipControlInfo(new CellToolTipInfo(gridHitInfo.RowHandle, gridHitInfo.Column, "cell"), Resources.FileManagerDestinationFileExists);
					e.Info = info;
				}
			}
			catch
			{
				e.Info = info;
			}
		}

		private bool GetDisplayedFileItem(int listSourceRowIndex, out DisplayedFileItem item)
		{
			item = null;
			if (listSourceRowIndex < 0 || listSourceRowIndex > this.DisplayedFolderFileList.FileList.Count - 1)
			{
				return false;
			}
			item = this.DisplayedFolderFileList.FileList[listSourceRowIndex];
			return null != item;
		}

		public bool Serialize(CLFExportPage clfExportPage)
		{
			if (clfExportPage == null)
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.gridViewCLFExport.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				clfExportPage.CLFExportGridLayout = Convert.ToBase64String(array, 0, array.Length);
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

		public bool DeSerialize(CLFExportPage clfExportPage)
		{
			if (clfExportPage == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(clfExportPage.CLFExportGridLayout))
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(clfExportPage.CLFExportGridLayout);
				memoryStream = new MemoryStream(buffer);
				this.gridViewCLFExport.RestoreLayoutFromStream(memoryStream);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CLFExportGrid));
			this.gridControlCLFExport = new GridControl();
			this.gridViewCLFExport = new GridView();
			this.colConvert = new GridColumn();
			this.repositoryItemCheckEditIsEnabled = new RepositoryItemCheckEdit();
			this.colFilename = new GridColumn();
			this.colFileSize = new GridColumn();
			this.colType = new GridColumn();
			this.colDateTimeModified = new GridColumn();
			this.colDestinationFilename = new GridColumn();
			this.toolTipController = new XtraToolTipController(this.components);
			this.displayFileItemBindingSource = new BindingSource(this.components);
			((ISupportInitialize)this.gridControlCLFExport).BeginInit();
			((ISupportInitialize)this.gridViewCLFExport).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsEnabled).BeginInit();
			((ISupportInitialize)this.displayFileItemBindingSource).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.gridControlCLFExport, "gridControlCLFExport");
			this.gridControlCLFExport.MainView = this.gridViewCLFExport;
			this.gridControlCLFExport.Name = "gridControlCLFExport";
			this.gridControlCLFExport.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemCheckEditIsEnabled
			});
			this.gridControlCLFExport.ToolTipController = this.toolTipController;
			this.gridControlCLFExport.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewCLFExport
			});
			this.gridViewCLFExport.Columns.AddRange(new GridColumn[]
			{
				this.colConvert,
				this.colFilename,
				this.colFileSize,
				this.colType,
				this.colDateTimeModified,
				this.colDestinationFilename
			});
			this.gridViewCLFExport.GridControl = this.gridControlCLFExport;
			this.gridViewCLFExport.Name = "gridViewCLFExport";
			this.gridViewCLFExport.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
			this.gridViewCLFExport.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
			this.gridViewCLFExport.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;
			this.gridViewCLFExport.OptionsCustomization.AllowColumnMoving = false;
			this.gridViewCLFExport.OptionsCustomization.AllowFilter = false;
			this.gridViewCLFExport.OptionsCustomization.AllowGroup = false;
			this.gridViewCLFExport.OptionsCustomization.AllowQuickHideColumns = false;
			this.gridViewCLFExport.OptionsSelection.MultiSelect = true;
			this.gridViewCLFExport.OptionsView.ShowGroupPanel = false;
			this.gridViewCLFExport.OptionsView.ShowIndicator = false;
			this.gridViewCLFExport.PaintStyleName = "WindowsXP";
			this.gridViewCLFExport.RowHeight = 20;
			this.gridViewCLFExport.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.colFilename, ColumnSortOrder.Ascending)
			});
			this.gridViewCLFExport.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewCLFExport_CustomDrawCell);
			this.gridViewCLFExport.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewCLFExport_PopupMenuShowing);
			this.gridViewCLFExport.CustomColumnSort += new CustomColumnSortEventHandler(this.gridViewCLFExport_CustomColumnSort);
			this.gridViewCLFExport.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewCLFExport_CustomUnboundColumnData);
			componentResourceManager.ApplyResources(this.colConvert, "colConvert");
			this.colConvert.ColumnEdit = this.repositoryItemCheckEditIsEnabled;
			this.colConvert.FieldName = "anyBoolean1";
			this.colConvert.Name = "colConvert";
			this.colConvert.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditIsEnabled, "repositoryItemCheckEditIsEnabled");
			this.repositoryItemCheckEditIsEnabled.Name = "repositoryItemCheckEditIsEnabled";
			this.repositoryItemCheckEditIsEnabled.CheckedChanged += new EventHandler(this.repositoryItemCheckEditIsEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.colFilename, "colFilename");
			this.colFilename.FieldName = "anyString1";
			this.colFilename.Name = "colFilename";
			this.colFilename.OptionsColumn.AllowEdit = false;
			this.colFilename.OptionsColumn.AllowSort = DefaultBoolean.True;
			this.colFilename.SortMode = ColumnSortMode.Custom;
			this.colFilename.UnboundType = DevExpress.Data.UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colFileSize, "colFileSize");
			this.colFileSize.FieldName = "anyString2";
			this.colFileSize.Name = "colFileSize";
			this.colFileSize.OptionsColumn.AllowEdit = false;
			this.colFileSize.OptionsColumn.AllowSort = DefaultBoolean.True;
			this.colFileSize.SortMode = ColumnSortMode.Custom;
			this.colFileSize.UnboundType = DevExpress.Data.UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colType, "colType");
			this.colType.FieldName = "anyString3";
			this.colType.Name = "colType";
			this.colType.OptionsColumn.AllowEdit = false;
			this.colType.UnboundType = DevExpress.Data.UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colDateTimeModified, "colDateTimeModified");
			this.colDateTimeModified.DisplayFormat.FormatString = "G";
			this.colDateTimeModified.DisplayFormat.FormatType = FormatType.DateTime;
			this.colDateTimeModified.FieldName = "anyDateTime1";
			this.colDateTimeModified.Name = "colDateTimeModified";
			this.colDateTimeModified.OptionsColumn.AllowEdit = false;
			this.colDateTimeModified.OptionsColumn.AllowSort = DefaultBoolean.True;
			this.colDateTimeModified.UnboundType = DevExpress.Data.UnboundColumnType.DateTime;
			componentResourceManager.ApplyResources(this.colDestinationFilename, "colDestinationFilename");
			this.colDestinationFilename.FieldName = "anyString4";
			this.colDestinationFilename.Name = "colDestinationFilename";
			this.colDestinationFilename.OptionsColumn.AllowEdit = false;
			this.colDestinationFilename.OptionsColumn.AllowSort = DefaultBoolean.False;
			this.colDestinationFilename.UnboundType = DevExpress.Data.UnboundColumnType.String;
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
			this.displayFileItemBindingSource.DataSource = typeof(DisplayedFileItem);
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.gridControlCLFExport);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "CLFExportGrid";
			((ISupportInitialize)this.gridControlCLFExport).EndInit();
			((ISupportInitialize)this.gridViewCLFExport).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsEnabled).EndInit();
			((ISupportInitialize)this.displayFileItemBindingSource).EndInit();
			base.ResumeLayout(false);
		}
	}
}
