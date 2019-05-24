using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Menu;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Vector.ConfigurationDialog;
using Vector.UtilityFunctions;
using Vector.UtilityFunctions.XtraGrid;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Options
{
	public class ConversionProfiles : UserControl
	{
		private LoggerType mLoggerType;

		private readonly GeneralService mGridGeneralService;

		private readonly KeyboardNavigationService mGridKeyboardNavigationService;

		private readonly BindingList<FileConversionProfile> mBindingList = new BindingList<FileConversionProfile>();

		private IContainer components;

		private TitledGroup titledGroup1;

		private Button mButtonMoveTop;

		private Button mButtonMoveUp;

		private Button mButtonMoveDown;

		private Button mButtonMoveBottom;

		private Button mButtonDelete;

		private Button mButtonImport;

		private GridControl mGridControl;

		private GridView mGridView;

		private GridColumn mGridColumnDisplayName;

		private GridColumn mGridColumnFile;

		private GridColumn mGridColumnState;

		private RepositoryItemImageComboBox mRepositoryItemImageComboBoxState;

		private XtraToolTipController mXtraToolTipController;

		private ContextMenuStrip mContextMenuStrip;

		private ToolStripMenuItem mToolStripMenuItemOpenInExplorer;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem mToolStripMenuItemMoveTop;

		private ToolStripMenuItem mToolStripMenuItemMoveUp;

		private ToolStripMenuItem mToolStripMenuItemMoveDown;

		private ToolStripMenuItem mToolStripMenuItemMoveBottom;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripMenuItem mToolStripMenuItemRemove;

		public LoggerType LoggerType
		{
			set
			{
				this.mLoggerType = value;
			}
		}

		private bool CanExecuteProfileAction
		{
			get
			{
				return this.GetProfile() != null;
			}
		}

		private bool CanOpenInExplorer
		{
			get
			{
				string path = this.GetPath();
				if (string.IsNullOrEmpty(path))
				{
					return false;
				}
				DirectoryInfo directoryInfo = new DirectoryInfo(path);
				return directoryInfo.Exists;
			}
		}

		public ConversionProfiles(LoggerType loggerType)
		{
			this.mLoggerType = loggerType;
			this.InitializeComponent();
			this.mGridGeneralService = new GeneralService(this.mGridView);
			this.mGridKeyboardNavigationService = new KeyboardNavigationService(this.mGridView);
			this.mRepositoryItemImageComboBoxState.SmallImages = MainImageList.Instance.ImageList;
			for (int i = 0; i <= MainImageList.Instance.ImageList.Images.Count; i++)
			{
				this.mRepositoryItemImageComboBoxState.Items.Add(new ImageComboBoxItem(string.Empty, i, i));
			}
			this.mGridControl.DataSource = this.mBindingList;
		}

		public ConversionProfiles()
		{
			this.mLoggerType = LoggerType.Unknown;
			this.InitializeComponent();
			this.mGridGeneralService = new GeneralService(this.mGridView);
			this.mGridKeyboardNavigationService = new KeyboardNavigationService(this.mGridView);
			this.mRepositoryItemImageComboBoxState.SmallImages = MainImageList.Instance.ImageList;
			for (int i = 0; i <= MainImageList.Instance.ImageList.Images.Count; i++)
			{
				this.mRepositoryItemImageComboBoxState.Items.Add(new ImageComboBoxItem(string.Empty, i, i));
			}
			this.mGridControl.DataSource = this.mBindingList;
		}

		private void InitFromPersistence()
		{
			int num = -1;
			if (this.mGridView.SelectedRowsCount > 0)
			{
				num = this.mGridView.GetSelectedRows()[0];
			}
			FileConversionProfile fileConversionProfile = null;
			if (num >= 0)
			{
				fileConversionProfile = (this.mGridView.GetRow(num) as FileConversionProfile);
			}
			this.mBindingList.RaiseListChangedEvents = false;
			this.mBindingList.Clear();
			ReadOnlyCollection<FileConversionProfile> profiles = FileConversionProfileManager.Instance.GetProfiles();
			foreach (FileConversionProfile current in profiles)
			{
				current.LoadFromFile();
				current.LoggerTypeForImageIndex = this.mLoggerType;
				this.mBindingList.Add(current);
			}
			this.mBindingList.RaiseListChangedEvents = true;
			this.mGridView.RefreshData();
			if (fileConversionProfile != null && this.mBindingList.Contains(fileConversionProfile))
			{
				this.mGridView.FocusedRowHandle = this.mGridView.GetRowHandle(this.mBindingList.IndexOf(fileConversionProfile));
			}
			else if (num >= 0 && num < this.mGridView.RowCount)
			{
				this.mGridView.FocusedRowHandle = num;
			}
			else if (num >= 0 && num >= this.mGridView.RowCount)
			{
				this.mGridView.FocusedRowHandle = this.mGridView.RowCount - 1;
			}
			else if (this.mGridView.RowCount > 0)
			{
				this.mGridView.FocusedRowHandle = 0;
			}
			this.UpdateButtons();
		}

		public void Cancel()
		{
			FileConversionProfileManager.Instance.InitFromPersistence();
		}

		public void Init()
		{
			this.InitFromPersistence();
		}

		public bool Save()
		{
			if (!FileConversionProfileManager.Instance.HasChanged)
			{
				return false;
			}
			FileConversionProfileManager.Instance.SaveToPersistence();
			return true;
		}

		private void UpdateButtons()
		{
			this.mButtonImport.Enabled = true;
			this.mButtonDelete.Enabled = this.CanExecuteProfileAction;
			this.mButtonMoveTop.Enabled = (this.CanExecuteProfileAction && this.mGridView.FocusedRowHandle > 0);
			this.mButtonMoveUp.Enabled = this.mButtonMoveTop.Enabled;
			this.mButtonMoveDown.Enabled = (this.CanExecuteProfileAction && this.mGridView.FocusedRowHandle < this.mGridView.RowCount - 1);
			this.mButtonMoveBottom.Enabled = this.mButtonMoveDown.Enabled;
		}

		private void UpdateContextMenu()
		{
			this.mToolStripMenuItemOpenInExplorer.Enabled = this.CanOpenInExplorer;
			this.mToolStripMenuItemMoveTop.Enabled = (this.CanExecuteProfileAction && this.mGridView.FocusedRowHandle > 0);
			this.mToolStripMenuItemMoveUp.Enabled = this.mButtonMoveTop.Enabled;
			this.mToolStripMenuItemMoveDown.Enabled = (this.CanExecuteProfileAction && this.mGridView.FocusedRowHandle < this.mGridView.RowCount - 1);
			this.mToolStripMenuItemMoveBottom.Enabled = this.mButtonMoveDown.Enabled;
			this.mToolStripMenuItemRemove.Enabled = this.CanExecuteProfileAction;
		}

		private void Import()
		{
			FileConversionProfileManager.Instance.LoadProfile(EnumViewType.Common, this.mLoggerType);
			this.InitFromPersistence();
		}

		private void Remove()
		{
			FileConversionProfileManager.Instance.RemoveProfile(this.GetProfile(), false);
			this.InitFromPersistence();
		}

		private void MoveTop()
		{
			FileConversionProfileManager.Instance.MoveTop(this.GetProfile());
			this.InitFromPersistence();
		}

		private void MoveUp()
		{
			FileConversionProfileManager.Instance.MoveUp(this.GetProfile());
			this.InitFromPersistence();
		}

		private void MoveDown()
		{
			FileConversionProfileManager.Instance.MoveDown(this.GetProfile());
			this.InitFromPersistence();
		}

		private void MoveBottom()
		{
			FileConversionProfileManager.Instance.MoveBottom(this.GetProfile());
			this.InitFromPersistence();
		}

		private FileConversionProfile GetProfile()
		{
			if (this.mGridView.FocusedRowHandle < 0)
			{
				return null;
			}
			return this.mGridView.GetRow(this.mGridView.FocusedRowHandle) as FileConversionProfile;
		}

		private void OpenInExplorer()
		{
			Process.Start(this.GetPath());
		}

		private string GetPath()
		{
			if (this.mGridView.FocusedRowHandle < 0)
			{
				return string.Empty;
			}
			FileConversionProfile fileConversionProfile = this.mGridView.GetRow(this.mGridView.FocusedRowHandle) as FileConversionProfile;
			if (fileConversionProfile == null)
			{
				return string.Empty;
			}
			return Path.GetDirectoryName(fileConversionProfile.FilePath);
		}

		private void ConversionProfiles_Load(object sender, EventArgs e)
		{
			this.mGridGeneralService.InitAppearance();
		}

		private void GridView_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (!(e.Menu is GridViewColumnMenu))
			{
				return;
			}
			this.mGridGeneralService.PopupMenuShowing(e);
			DXMenuItem dXMenuItem = null;
			DXMenuItem dXMenuItem2 = null;
			DXMenuItem dXMenuItem3 = null;
			foreach (DXMenuItem dXMenuItem4 in e.Menu.Items)
			{
				GridStringId gridStringId = (GridStringId)dXMenuItem4.Tag;
				if (gridStringId != GridStringId.MenuColumnColumnCustomization)
				{
					if (gridStringId != GridStringId.MenuColumnBestFitAllColumns)
					{
						if (gridStringId == GridStringId.MenuColumnClearSorting)
						{
							dXMenuItem3 = dXMenuItem4;
						}
					}
					else
					{
						dXMenuItem2 = dXMenuItem4;
					}
				}
				else
				{
					dXMenuItem = dXMenuItem4;
				}
			}
			if (dXMenuItem3 != null)
			{
				e.Menu.Items.Remove(dXMenuItem3);
			}
			if (dXMenuItem != null)
			{
				e.Menu.Items.Remove(dXMenuItem);
			}
			if (dXMenuItem2 != null)
			{
				dXMenuItem2.BeginGroup = false;
			}
		}

		private void GridControl_ProcessGridKey(object sender, KeyEventArgs e)
		{
			this.mGridKeyboardNavigationService.GridControlProcessGridKey(e);
		}

		private void GridControl_EditorKeyDown(object sender, KeyEventArgs e)
		{
			this.mGridKeyboardNavigationService.GridControlEditorKeyDown(e);
		}

		private void GridView_ShowingEditor(object sender, CancelEventArgs e)
		{
			this.mGridGeneralService.ShowingEditor<FileConversionProfile>(e, this.GetProfile(), this.mGridView.FocusedColumn, new GeneralService.IsReadOnlyAtAll<FileConversionProfile>(this.IsCellReadOnlyAtAll), new GeneralService.IsReadOnlyByCellContent<FileConversionProfile>(this.IsCellReadOnlyByCellContent));
		}

		private bool IsCellReadOnlyAtAll(FileConversionProfile profile, GridColumn column)
		{
			return profile == null || column != this.mGridColumnDisplayName || profile.HasErrors(EnumViewType.Common);
		}

		private bool IsCellReadOnlyByCellContent(FileConversionProfile profile, GridColumn column)
		{
			return false;
		}

		private void GridView_ShownEditor(object sender, EventArgs e)
		{
			this.mGridKeyboardNavigationService.GridViewShownEditor();
		}

		private void GridView_HiddenEditor(object sender, EventArgs e)
		{
			this.UpdateButtons();
		}

		private void GridView_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
		{
			this.UpdateButtons();
		}

		private void GridView_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
			this.UpdateButtons();
		}

		private void XtraToolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
		{
			if (e.SelectedControl != this.mGridControl)
			{
				return;
			}
			GridView gridView = this.mGridControl.GetViewAt(e.ControlMousePosition) as GridView;
			if (gridView != this.mGridView || this.mGridView == null)
			{
				return;
			}
			GridHitInfo gridHitInfo = this.mGridView.CalcHitInfo(e.ControlMousePosition);
			if (gridHitInfo == null || gridHitInfo.Column == null || !gridHitInfo.InRowCell)
			{
				return;
			}
			FileConversionProfile fileConversionProfile = this.mGridView.GetRow(gridHitInfo.RowHandle) as FileConversionProfile;
			if (fileConversionProfile == null)
			{
				return;
			}
			string text = string.Empty;
			if (gridHitInfo.Column == this.mGridColumnState)
			{
				if (fileConversionProfile.HasErrors(EnumViewType.Common))
				{
					text = fileConversionProfile.GetErrorsText(EnumViewType.Common);
				}
				else if (fileConversionProfile.HasWarnings(EnumViewType.Common))
				{
					text = fileConversionProfile.GetWarningsText(EnumViewType.Common);
					if (fileConversionProfile.HasInfos(EnumViewType.Common, this.mLoggerType))
					{
						text = text + Environment.NewLine + fileConversionProfile.GetInfosText(EnumViewType.Common, this.mLoggerType);
					}
				}
				else if (fileConversionProfile.HasInfos(EnumViewType.Common, this.mLoggerType))
				{
					text = fileConversionProfile.GetInfosText(EnumViewType.Common, this.mLoggerType);
				}
			}
			if (gridHitInfo.Column == this.mGridColumnFile)
			{
				text = fileConversionProfile.FilePath;
			}
			if (!string.IsNullOrEmpty(text))
			{
				e.Info = new ToolTipControlInfo(new CellToolTipInfo(gridHitInfo.RowHandle, gridHitInfo.Column, "cell"), text);
			}
		}

		private void Button_Click(object sender, EventArgs e)
		{
			if (sender == this.mButtonImport)
			{
				this.Import();
				return;
			}
			if (sender == this.mButtonDelete)
			{
				this.Remove();
				return;
			}
			if (sender == this.mButtonMoveTop)
			{
				this.MoveTop();
				return;
			}
			if (sender == this.mButtonMoveUp)
			{
				this.MoveUp();
				return;
			}
			if (sender == this.mButtonMoveDown)
			{
				this.MoveDown();
				return;
			}
			if (sender == this.mButtonMoveBottom)
			{
				this.MoveBottom();
			}
		}

		private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			this.UpdateContextMenu();
		}

		private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem == this.mToolStripMenuItemOpenInExplorer)
			{
				this.OpenInExplorer();
				return;
			}
			if (e.ClickedItem == this.mToolStripMenuItemMoveTop)
			{
				this.MoveTop();
				return;
			}
			if (e.ClickedItem == this.mToolStripMenuItemMoveUp)
			{
				this.MoveUp();
				return;
			}
			if (e.ClickedItem == this.mToolStripMenuItemMoveDown)
			{
				this.MoveDown();
				return;
			}
			if (e.ClickedItem == this.mToolStripMenuItemMoveBottom)
			{
				this.MoveBottom();
				return;
			}
			if (e.ClickedItem == this.mToolStripMenuItemRemove)
			{
				this.Remove();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ConversionProfiles));
			this.titledGroup1 = new TitledGroup();
			this.mGridControl = new GridControl();
			this.mContextMenuStrip = new ContextMenuStrip(this.components);
			this.mToolStripMenuItemOpenInExplorer = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.mToolStripMenuItemMoveTop = new ToolStripMenuItem();
			this.mToolStripMenuItemMoveUp = new ToolStripMenuItem();
			this.mToolStripMenuItemMoveDown = new ToolStripMenuItem();
			this.mToolStripMenuItemMoveBottom = new ToolStripMenuItem();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.mToolStripMenuItemRemove = new ToolStripMenuItem();
			this.mGridView = new GridView();
			this.mGridColumnState = new GridColumn();
			this.mRepositoryItemImageComboBoxState = new RepositoryItemImageComboBox();
			this.mGridColumnDisplayName = new GridColumn();
			this.mGridColumnFile = new GridColumn();
			this.mXtraToolTipController = new XtraToolTipController(this.components);
			this.mButtonMoveTop = new Button();
			this.mButtonMoveUp = new Button();
			this.mButtonMoveDown = new Button();
			this.mButtonMoveBottom = new Button();
			this.mButtonDelete = new Button();
			this.mButtonImport = new Button();
			this.titledGroup1.SuspendLayout();
			((ISupportInitialize)this.mGridControl).BeginInit();
			this.mContextMenuStrip.SuspendLayout();
			((ISupportInitialize)this.mGridView).BeginInit();
			((ISupportInitialize)this.mRepositoryItemImageComboBoxState).BeginInit();
			base.SuspendLayout();
			this.titledGroup1.AutoSizeGroup = true;
			this.titledGroup1.BackColor = SystemColors.Window;
			this.titledGroup1.Controls.Add(this.mGridControl);
			this.titledGroup1.Controls.Add(this.mButtonMoveTop);
			this.titledGroup1.Controls.Add(this.mButtonMoveUp);
			this.titledGroup1.Controls.Add(this.mButtonMoveDown);
			this.titledGroup1.Controls.Add(this.mButtonMoveBottom);
			this.titledGroup1.Controls.Add(this.mButtonDelete);
			this.titledGroup1.Controls.Add(this.mButtonImport);
			componentResourceManager.ApplyResources(this.titledGroup1, "titledGroup1");
			this.titledGroup1.Image = null;
			this.titledGroup1.Name = "titledGroup1";
			this.mXtraToolTipController.SetTitle(this.titledGroup1, componentResourceManager.GetString("titledGroup1.Title"));
			componentResourceManager.ApplyResources(this.mGridControl, "mGridControl");
			this.mGridControl.ContextMenuStrip = this.mContextMenuStrip;
			this.mGridControl.LookAndFeel.Style = LookAndFeelStyle.Style3D;
			this.mGridControl.LookAndFeel.UseDefaultLookAndFeel = false;
			this.mGridControl.LookAndFeel.UseWindowsXPTheme = true;
			this.mGridControl.MainView = this.mGridView;
			this.mGridControl.Name = "mGridControl";
			this.mGridControl.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.mRepositoryItemImageComboBoxState
			});
			this.mGridControl.ToolTipController = this.mXtraToolTipController;
			this.mGridControl.ViewCollection.AddRange(new BaseView[]
			{
				this.mGridView
			});
			this.mGridControl.ProcessGridKey += new KeyEventHandler(this.GridControl_ProcessGridKey);
			this.mGridControl.EditorKeyDown += new KeyEventHandler(this.GridControl_EditorKeyDown);
			this.mContextMenuStrip.Items.AddRange(new ToolStripItem[]
			{
				this.mToolStripMenuItemOpenInExplorer,
				this.toolStripSeparator1,
				this.mToolStripMenuItemMoveTop,
				this.mToolStripMenuItemMoveUp,
				this.mToolStripMenuItemMoveDown,
				this.mToolStripMenuItemMoveBottom,
				this.toolStripSeparator2,
				this.mToolStripMenuItemRemove
			});
			this.mContextMenuStrip.Name = "mContextMenuStrip";
			componentResourceManager.ApplyResources(this.mContextMenuStrip, "mContextMenuStrip");
			this.mContextMenuStrip.Opening += new CancelEventHandler(this.ContextMenuStrip_Opening);
			this.mContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.ContextMenuStrip_ItemClicked);
			this.mToolStripMenuItemOpenInExplorer.Image = Resources.ImageOpenFolder;
			this.mToolStripMenuItemOpenInExplorer.Name = "mToolStripMenuItemOpenInExplorer";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemOpenInExplorer, "mToolStripMenuItemOpenInExplorer");
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			componentResourceManager.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			this.mToolStripMenuItemMoveTop.Image = Resources.ImageMoveFirst;
			this.mToolStripMenuItemMoveTop.Name = "mToolStripMenuItemMoveTop";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemMoveTop, "mToolStripMenuItemMoveTop");
			this.mToolStripMenuItemMoveUp.Image = Resources.ImageMovePrev;
			this.mToolStripMenuItemMoveUp.Name = "mToolStripMenuItemMoveUp";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemMoveUp, "mToolStripMenuItemMoveUp");
			this.mToolStripMenuItemMoveDown.Image = Resources.ImageMoveNext;
			this.mToolStripMenuItemMoveDown.Name = "mToolStripMenuItemMoveDown";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemMoveDown, "mToolStripMenuItemMoveDown");
			this.mToolStripMenuItemMoveBottom.Image = Resources.ImageMoveLast;
			this.mToolStripMenuItemMoveBottom.Name = "mToolStripMenuItemMoveBottom";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemMoveBottom, "mToolStripMenuItemMoveBottom");
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			componentResourceManager.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			this.mToolStripMenuItemRemove.Image = Resources.ImageDelete;
			this.mToolStripMenuItemRemove.Name = "mToolStripMenuItemRemove";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemRemove, "mToolStripMenuItemRemove");
			this.mGridView.Columns.AddRange(new GridColumn[]
			{
				this.mGridColumnState,
				this.mGridColumnDisplayName,
				this.mGridColumnFile
			});
			this.mGridView.GridControl = this.mGridControl;
			this.mGridView.Name = "mGridView";
			this.mGridView.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
			this.mGridView.OptionsBehavior.ImmediateUpdateRowPosition = false;
			this.mGridView.OptionsCustomization.AllowFilter = false;
			this.mGridView.OptionsCustomization.AllowGroup = false;
			this.mGridView.OptionsCustomization.AllowQuickHideColumns = false;
			this.mGridView.OptionsCustomization.AllowSort = false;
			this.mGridView.OptionsFilter.AllowFilterEditor = false;
			this.mGridView.OptionsFind.AllowFindPanel = false;
			this.mGridView.OptionsLayout.Columns.StoreAllOptions = true;
			this.mGridView.OptionsLayout.Columns.StoreAppearance = true;
			this.mGridView.OptionsLayout.StoreAllOptions = true;
			this.mGridView.OptionsLayout.StoreAppearance = true;
			this.mGridView.OptionsNavigation.UseTabKey = false;
			this.mGridView.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
			this.mGridView.OptionsView.ShowGroupPanel = false;
			this.mGridView.OptionsView.ShowIndicator = false;
			this.mGridView.PopupMenuShowing += new PopupMenuShowingEventHandler(this.GridView_PopupMenuShowing);
			this.mGridView.ShowingEditor += new CancelEventHandler(this.GridView_ShowingEditor);
			this.mGridView.HiddenEditor += new EventHandler(this.GridView_HiddenEditor);
			this.mGridView.ShownEditor += new EventHandler(this.GridView_ShownEditor);
			this.mGridView.FocusedRowChanged += new FocusedRowChangedEventHandler(this.GridView_FocusedRowChanged);
			this.mGridView.FocusedColumnChanged += new FocusedColumnChangedEventHandler(this.GridView_FocusedColumnChanged);
			this.mGridColumnState.ColumnEdit = this.mRepositoryItemImageComboBoxState;
			this.mGridColumnState.FieldName = "ImageIndex";
			this.mGridColumnState.MaxWidth = 21;
			this.mGridColumnState.MinWidth = 21;
			this.mGridColumnState.Name = "mGridColumnState";
			this.mGridColumnState.OptionsColumn.AllowEdit = false;
			this.mGridColumnState.OptionsColumn.AllowFocus = false;
			this.mGridColumnState.OptionsColumn.AllowMove = false;
			this.mGridColumnState.OptionsColumn.AllowSize = false;
			this.mGridColumnState.OptionsColumn.FixedWidth = true;
			this.mGridColumnState.OptionsColumn.ReadOnly = true;
			this.mGridColumnState.OptionsColumn.ShowCaption = false;
			componentResourceManager.ApplyResources(this.mGridColumnState, "mGridColumnState");
			componentResourceManager.ApplyResources(this.mRepositoryItemImageComboBoxState, "mRepositoryItemImageComboBoxState");
			this.mRepositoryItemImageComboBoxState.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("mRepositoryItemImageComboBoxState.Buttons"))
			});
			this.mRepositoryItemImageComboBoxState.Name = "mRepositoryItemImageComboBoxState";
			componentResourceManager.ApplyResources(this.mGridColumnDisplayName, "mGridColumnDisplayName");
			this.mGridColumnDisplayName.FieldName = "DisplayName";
			this.mGridColumnDisplayName.Name = "mGridColumnDisplayName";
			componentResourceManager.ApplyResources(this.mGridColumnFile, "mGridColumnFile");
			this.mGridColumnFile.FieldName = "File";
			this.mGridColumnFile.Name = "mGridColumnFile";
			this.mGridColumnFile.OptionsColumn.AllowEdit = false;
			this.mGridColumnFile.OptionsColumn.AllowFocus = false;
			this.mGridColumnFile.OptionsColumn.ReadOnly = true;
			this.mXtraToolTipController.Appearance.BackColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.Appearance.BackColor");
			this.mXtraToolTipController.Appearance.ForeColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.Appearance.ForeColor");
			this.mXtraToolTipController.Appearance.Options.UseBackColor = true;
			this.mXtraToolTipController.Appearance.Options.UseForeColor = true;
			this.mXtraToolTipController.Appearance.Options.UseTextOptions = true;
			this.mXtraToolTipController.Appearance.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.mXtraToolTipController.AppearanceTitle.BackColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.AppearanceTitle.BackColor");
			this.mXtraToolTipController.AppearanceTitle.ForeColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.AppearanceTitle.ForeColor");
			this.mXtraToolTipController.AppearanceTitle.Options.UseBackColor = true;
			this.mXtraToolTipController.AppearanceTitle.Options.UseForeColor = true;
			this.mXtraToolTipController.AppearanceTitle.Options.UseTextOptions = true;
			this.mXtraToolTipController.AppearanceTitle.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.mXtraToolTipController.MaxWidth = 1000;
			this.mXtraToolTipController.ShowPrefix = true;
			this.mXtraToolTipController.UseNativeLookAndFeel = true;
			this.mXtraToolTipController.GetActiveObjectInfo += new ToolTipControllerGetActiveObjectInfoEventHandler(this.XtraToolTipController_GetActiveObjectInfo);
			componentResourceManager.ApplyResources(this.mButtonMoveTop, "mButtonMoveTop");
			this.mButtonMoveTop.Image = Resources.ImageMoveFirst;
			this.mButtonMoveTop.Name = "mButtonMoveTop";
			this.mXtraToolTipController.SetToolTip(this.mButtonMoveTop, componentResourceManager.GetString("mButtonMoveTop.ToolTip"));
			this.mButtonMoveTop.UseVisualStyleBackColor = true;
			this.mButtonMoveTop.Click += new EventHandler(this.Button_Click);
			componentResourceManager.ApplyResources(this.mButtonMoveUp, "mButtonMoveUp");
			this.mButtonMoveUp.Image = Resources.ImageMovePrev;
			this.mButtonMoveUp.Name = "mButtonMoveUp";
			this.mXtraToolTipController.SetToolTip(this.mButtonMoveUp, componentResourceManager.GetString("mButtonMoveUp.ToolTip"));
			this.mButtonMoveUp.UseVisualStyleBackColor = true;
			this.mButtonMoveUp.Click += new EventHandler(this.Button_Click);
			componentResourceManager.ApplyResources(this.mButtonMoveDown, "mButtonMoveDown");
			this.mButtonMoveDown.Image = Resources.ImageMoveNext;
			this.mButtonMoveDown.Name = "mButtonMoveDown";
			this.mXtraToolTipController.SetToolTip(this.mButtonMoveDown, componentResourceManager.GetString("mButtonMoveDown.ToolTip"));
			this.mButtonMoveDown.UseVisualStyleBackColor = true;
			this.mButtonMoveDown.Click += new EventHandler(this.Button_Click);
			componentResourceManager.ApplyResources(this.mButtonMoveBottom, "mButtonMoveBottom");
			this.mButtonMoveBottom.Image = Resources.ImageMoveLast;
			this.mButtonMoveBottom.Name = "mButtonMoveBottom";
			this.mXtraToolTipController.SetToolTip(this.mButtonMoveBottom, componentResourceManager.GetString("mButtonMoveBottom.ToolTip"));
			this.mButtonMoveBottom.UseVisualStyleBackColor = true;
			this.mButtonMoveBottom.Click += new EventHandler(this.Button_Click);
			this.mButtonDelete.Image = Resources.ImageDelete;
			componentResourceManager.ApplyResources(this.mButtonDelete, "mButtonDelete");
			this.mButtonDelete.Name = "mButtonDelete";
			this.mXtraToolTipController.SetToolTip(this.mButtonDelete, componentResourceManager.GetString("mButtonDelete.ToolTip"));
			this.mButtonDelete.UseVisualStyleBackColor = true;
			this.mButtonDelete.Click += new EventHandler(this.Button_Click);
			this.mButtonImport.Image = Resources.ImageImportConvertSettings;
			componentResourceManager.ApplyResources(this.mButtonImport, "mButtonImport");
			this.mButtonImport.Name = "mButtonImport";
			this.mXtraToolTipController.SetToolTip(this.mButtonImport, componentResourceManager.GetString("mButtonImport.ToolTip"));
			this.mButtonImport.UseVisualStyleBackColor = true;
			this.mButtonImport.Click += new EventHandler(this.Button_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.titledGroup1);
			base.Name = "ConversionProfiles";
			base.Load += new EventHandler(this.ConversionProfiles_Load);
			this.titledGroup1.ResumeLayout(false);
			((ISupportInitialize)this.mGridControl).EndInit();
			this.mContextMenuStrip.ResumeLayout(false);
			((ISupportInitialize)this.mGridView).EndInit();
			((ISupportInitialize)this.mRepositoryItemImageComboBoxState).EndInit();
			base.ResumeLayout(false);
		}
	}
}
