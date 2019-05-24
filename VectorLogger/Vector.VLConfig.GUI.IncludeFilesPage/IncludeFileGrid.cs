using DevExpress.Data;
using DevExpress.LookAndFeel;
using DevExpress.Utils;
using DevExpress.Utils.Menu;
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
using Vector.UtilityFunctions.XtraGrid;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.IncludeFilesPage
{
	public class IncludeFileGrid : UserControl
	{
		private IncludeFileConfiguration mIncludeFileConfiguration;

		private readonly GUIElementManager_ControlGridTree mGuiElementManager;

		private readonly CustomErrorProvider mCustomErrorProvider;

		private readonly PageValidator mPageValidator;

		private readonly List<IncludeFilePresenter> mIncludeFilePresenters = new List<IncludeFilePresenter>();

		private readonly BindingList<IncludeFileParameterPresenter> mBindingListParameters = new BindingList<IncludeFileParameterPresenter>();

		private readonly GeneralService mGridViewGeneralService;

		private readonly KeyboardNavigationService mGridViewKeyboardNavigationService;

		private IContainer components;

		private GridControl mGridControlIncludeFiles;

		private GridView mGridViewIncludeFiles;

		private Button mButtonMoveFirst;

		private Button mButtonMoveLast;

		private Button mButtonMoveDown;

		private Button mButtonMoveUp;

		private XtraToolTipController mXtraToolTipController;

		private GridColumn mGridColumnFileName;

		private GridColumn mGridColumnName;

		private GridColumn mGridColumnValue;

		private GridColumn mGridColumnDescription;

		private GridColumn mGridColumnParamNumber;

		private GridColumn mGridColumnType;

		private RepositoryItemImageComboBox mRepositoryItemImageComboBoxParamType;

		private RepositoryItemImageComboBox mRepositoryItemImageComboBoxGroupImage;

		private ErrorProvider mErrorProviderFormat;

		private ErrorProvider mErrorProviderLocalModel;

		private ErrorProvider mErrorProviderGlobalModel;

		private ContextMenuStrip mContextMenuStrip;

		private ToolStripMenuItem mToolStripMenuItemEditFile;

		private ToolStripMenuItem mToolStripMenuItemOpenFolder;

		public event EventHandler SelectionChanged;

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public IncludeFileConfiguration IncludeFileConfiguration
		{
			get
			{
				return this.mIncludeFileConfiguration;
			}
			set
			{
				this.mIncludeFileConfiguration = value;
				bool isDataChanged;
				this.UpdateFromConfig(out isDataChanged);
				if (this.mIncludeFileConfiguration != null)
				{
					this.ValidateInput(isDataChanged);
				}
			}
		}

		public IncludeFileGrid()
		{
			this.InitializeComponent();
			this.mGridViewGeneralService = new GeneralService(this.mGridViewIncludeFiles);
			this.mGridViewGeneralService.InitAppearance();
			this.mGridViewKeyboardNavigationService = new KeyboardNavigationService(this.mGridViewIncludeFiles);
			this.mGuiElementManager = new GUIElementManager_ControlGridTree();
			this.mCustomErrorProvider = new CustomErrorProvider(this.mErrorProviderGlobalModel);
			this.mCustomErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.mErrorProviderLocalModel);
			this.mCustomErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.FormatError, this.mErrorProviderFormat);
			this.mPageValidator = new PageValidator(this.mCustomErrorProvider);
			IncludeFileManager.Instance.ResultCollector = this.mPageValidator.ResultCollector;
			this.InitImageComboBoxParamType();
			this.InitImageComboBoxGroupImage();
			this.mGridControlIncludeFiles.DataSource = this.mBindingListParameters;
		}

		private void UpdateFromConfig()
		{
			bool flag;
			this.UpdateFromConfig(out flag);
		}

		private void UpdateFromConfig(out bool hasChanged)
		{
			this.mBindingListParameters.RaiseListChangedEvents = false;
			this.mBindingListParameters.Clear();
			this.mIncludeFilePresenters.Clear();
			IncludeFileManager.Instance.Refresh(this.mIncludeFileConfiguration, out hasChanged);
			if (this.mIncludeFileConfiguration == null || this.ModelValidator == null)
			{
				this.mBindingListParameters.RaiseListChangedEvents = true;
				this.mGridViewIncludeFiles.RefreshData();
				return;
			}
			this.mIncludeFilePresenters.AddRange(IncludeFileManager.Instance.Files);
			foreach (IncludeFileParameterPresenter current in IncludeFileManager.Instance.Parameters)
			{
				this.mBindingListParameters.Add(current);
			}
			this.UpdateImageComboBoxGroupImage();
			this.mBindingListParameters.RaiseListChangedEvents = true;
			this.mGridViewIncludeFiles.RefreshData();
			this.ExpandAllNonEmptyGroups();
		}

		private void ExpandAllNonEmptyGroups()
		{
			foreach (IncludeFile current in this.IncludeFileConfiguration.IncludeFiles)
			{
				int rowHandle;
				if (this.TryGetGroupRowHandle(current, out rowHandle))
				{
					int childRowCount = this.mGridViewIncludeFiles.GetChildRowCount(rowHandle);
					if (childRowCount > 1 || (childRowCount == 1 && this.GetParamsOfIncFile(current).First<IncludeFileParameterPresenter>().ParameterType != IncludeFileParameter.ParamType.Dummy))
					{
						this.mGridViewIncludeFiles.ExpandGroupRow(rowHandle);
					}
					else
					{
						this.mGridViewIncludeFiles.CollapseGroupRow(rowHandle);
					}
				}
			}
		}

		private void InitImageComboBoxParamType()
		{
			this.mRepositoryItemImageComboBoxParamType.SmallImages = MainImageList.Instance.ImageList;
			this.mRepositoryItemImageComboBoxParamType.Items.Add(new ImageComboBoxItem(string.Empty, 0, MainImageList.Instance.GetImageIndex(MainImageList.IconIndex.NoImage, false)));
			this.mRepositoryItemImageComboBoxParamType.Items.Add(new ImageComboBoxItem(string.Empty, 1, MainImageList.Instance.GetImageIndex(MainImageList.IconIndex.InstanceParameter, false)));
			this.mRepositoryItemImageComboBoxParamType.Items.Add(new ImageComboBoxItem(string.Empty, 2, MainImageList.Instance.GetImageIndex(MainImageList.IconIndex.InputParameter, false)));
			this.mRepositoryItemImageComboBoxParamType.Items.Add(new ImageComboBoxItem(string.Empty, 3, MainImageList.Instance.GetImageIndex(MainImageList.IconIndex.OutputParameter, false)));
		}

		private void InitImageComboBoxGroupImage()
		{
			this.mRepositoryItemImageComboBoxGroupImage.SmallImages = MainImageList.Instance.ImageList;
		}

		private void UpdateImageComboBoxGroupImage()
		{
			this.mRepositoryItemImageComboBoxGroupImage.Items.Clear();
			foreach (IncludeFilePresenter current in this.mIncludeFilePresenters)
			{
				int imageIndex = MainImageList.Instance.GetImageIndex(MainImageList.IconIndex.NoImage, false);
				this.mRepositoryItemImageComboBoxGroupImage.Items.Add((current.GroupImageIndex == imageIndex) ? new ImageComboBoxItem(current.FileName, current.AbsoluteFilePath) : new ImageComboBoxItem(current.FileName, current.AbsoluteFilePath, current.GroupImageIndex));
			}
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

		public void RefreshView()
		{
			bool isDataChanged;
			this.UpdateFromConfig(out isDataChanged);
			this.ValidateInput(isDataChanged);
		}

		public void AddIncludeFile(string includeFilePath)
		{
			IncludeFile includeFile = new IncludeFile();
			includeFile.FilePath.Value = this.ModelValidator.GetFilePathRelativeToConfiguration(includeFilePath);
			this.mIncludeFileConfiguration.AddIncludeFile(includeFile);
			this.UpdateFromConfig();
			this.ValidateInput(true);
			this.SelectRowOfIncludeFile(includeFile);
		}

		public void RemoveIncludeFile(IncludeFile includeFile)
		{
			this.mIncludeFileConfiguration.RemoveIncludeFile(includeFile);
			this.UpdateFromConfig();
			this.ValidateInput(true);
		}

		public bool TryGetSelectedIncludeFile(out IncludeFile includeFile)
		{
			int num;
			return this.TryGetSelectedIncludeFile(out includeFile, out num);
		}

		private bool TryGetSelectedIncludeFile(out IncludeFile includeFile, out int idx)
		{
			includeFile = null;
			idx = 0;
			if (this.mIncludeFileConfiguration == null)
			{
				return false;
			}
			IncludeFileParameterPresenter includeFileParameterPresenter;
			if (!this.TryGetSelectedPresenter(out includeFileParameterPresenter))
			{
				return false;
			}
			includeFile = includeFileParameterPresenter.IncludeFile;
			idx = this.mIncludeFileConfiguration.IncludeFiles.IndexOf(includeFile);
			return idx >= 0 && includeFile != null;
		}

		private bool TryGetIncludeFile(int rowHandle, out IncludeFile includeFile)
		{
			includeFile = null;
			IncludeFileParameterPresenter includeFileParameterPresenter;
			if (!this.TryGetPresenter(rowHandle, out includeFileParameterPresenter))
			{
				return false;
			}
			includeFile = includeFileParameterPresenter.IncludeFile;
			return includeFile != null;
		}

		private bool TryGetSelectedPresenter(out IncludeFileParameterPresenter dataRecord)
		{
			dataRecord = (this.mGridViewIncludeFiles.GetFocusedRow() as IncludeFileParameterPresenter);
			return dataRecord != null;
		}

		private bool TryGetPresenter(int rowHandle, out IncludeFileParameterPresenter dataRecord)
		{
			dataRecord = (this.mGridViewIncludeFiles.GetRow(rowHandle) as IncludeFileParameterPresenter);
			return dataRecord != null;
		}

		private bool TryGetGroupRowHandle(IncludeFile includeFile, out int groupRowHandle)
		{
			groupRowHandle = -1;
			if (includeFile == null || !this.IncludeFileConfiguration.IncludeFiles.Contains(includeFile))
			{
				return false;
			}
			List<IncludeFileParameterPresenter> source = (from param in this.mBindingListParameters
			where param.IncludeFile == includeFile
			select param).ToList<IncludeFileParameterPresenter>();
			return source.Any<IncludeFileParameterPresenter>() && this.TryGetGroupRowHandle(source.First<IncludeFileParameterPresenter>(), out groupRowHandle);
		}

		private bool TryGetGroupRowHandle(IncludeFileParameterPresenter dataRecord, out int groupRowHandle)
		{
			groupRowHandle = -1;
			if (dataRecord == null || !this.mBindingListParameters.Contains(dataRecord))
			{
				return false;
			}
			int dataSourceIndex = this.mBindingListParameters.IndexOf(dataRecord);
			int rowHandle = this.mGridViewIncludeFiles.GetRowHandle(dataSourceIndex);
			groupRowHandle = this.mGridViewIncludeFiles.GetParentRowHandle(rowHandle);
			return true;
		}

		private void SelectRowOfIncludeFile(IncludeFile includeFile)
		{
			int focusedRowHandle;
			if (this.TryGetGroupRowHandle(includeFile, out focusedRowHandle))
			{
				this.mGridViewIncludeFiles.FocusedRowHandle = focusedRowHandle;
			}
		}

		private IEnumerable<IncludeFileParameterPresenter> GetParamsOfIncFile(IncludeFile incFile)
		{
			return from param in this.mBindingListParameters
			where param.IncludeFile == incFile
			select param;
		}

		public void DisplayErrors()
		{
			this.StoreMapping4VisibleCells();
			this.mPageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
		}

		public bool ValidateInput(bool isDataChanged = false)
		{
			this.ResetValidationFramework();
			this.StoreMapping4VisibleCells();
			bool flag = true;
			flag &= this.ModelValidator.Validate(this.IncludeFileConfiguration, isDataChanged, this.mPageValidator);
			flag &= IncludeFileManager.Instance.ValidateParsedData();
			this.UpdateImageComboBoxGroupImage();
			this.mPageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		public bool HasErrors()
		{
			return this.mPageValidator.General.HasErrors(Enum.GetValues(typeof(ValidationErrorClass)) as ValidationErrorClass[]);
		}

		public bool HasGlobalErrors()
		{
			return this.mPageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.GlobalModelError
			});
		}

		public bool HasLocalErrors()
		{
			return this.mPageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.FormatError,
				ValidationErrorClass.LocalModelError
			});
		}

		public bool HasFormatErrors()
		{
			IPageValidatorGeneral arg_13_0 = this.mPageValidator.General;
			ValidationErrorClass[] errorClasses = new ValidationErrorClass[1];
			return arg_13_0.HasErrors(errorClasses);
		}

		private void ResetValidationFramework()
		{
			this.mPageValidator.General.Reset();
			this.mGuiElementManager.Reset();
		}

		private void StoreMapping4VisibleCells()
		{
			PageValidatorGridUtil.StoreRowMappingForVisibleRows(this.mGridViewIncludeFiles, new PageValidatorGridUtil.StoreRowMappingHandler(this.OnPageValidatorGridUtilStoreRowMapping));
		}

		private void OnPageValidatorGridUtilStoreRowMapping(int dataSourceRowIdx)
		{
			if (dataSourceRowIdx < 0 || dataSourceRowIdx >= this.mBindingListParameters.Count)
			{
				return;
			}
			IncludeFileParameterPresenter includeFileParameterPresenter = this.mBindingListParameters[dataSourceRowIdx];
			this.StoreMapping4Column(dataSourceRowIdx, this.mGridColumnName, includeFileParameterPresenter.NameProperty);
			this.StoreMapping4Column(dataSourceRowIdx, this.mGridColumnValue, includeFileParameterPresenter.ValueProperty);
		}

		private void StoreMapping4Column(int dataSourceIdx, GridColumn column, IValidatedProperty validatedProperty)
		{
			if (PageValidatorGridUtil.IsColumnVisible(column, this.mGridViewIncludeFiles))
			{
				IValidatedGUIElement gUIElement = this.mGuiElementManager.GetGUIElement(column, dataSourceIdx);
				this.mPageValidator.Grid.StoreMapping(validatedProperty, gUIElement);
			}
		}

		private void GridControlIncludeFiles_EditorKeyDown(object sender, KeyEventArgs e)
		{
			this.mGridViewKeyboardNavigationService.GridControlEditorKeyDown(e);
		}

		private void GridControlIncludeFiles_ProcessGridKey(object sender, KeyEventArgs e)
		{
			this.mGridViewKeyboardNavigationService.GridControlProcessGridKey(e);
		}

		private void GridViewIncludeFiles_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			this.mGridViewGeneralService.CustomDrawCell<IncludeFileParameterPresenter>(e, new GeneralService.IsReadOnlyAtAll<IncludeFileParameterPresenter>(this.IsReadOnlyAtAll), new GeneralService.IsReadOnlyByCellContent<IncludeFileParameterPresenter>(this.IsReadOnlyByCellContent));
			IValidatedGUIElement gUIElement = this.mGuiElementManager.GetGUIElement(e.Column, this.mGridViewIncludeFiles.GetDataSourceRowIndex(e.RowHandle));
			this.mCustomErrorProvider.Grid.DisplayError(gUIElement, e);
		}

		private void GridViewIncludeFiles_DoubleClick(object sender, EventArgs e)
		{
			Point pt = this.mGridControlIncludeFiles.PointToClient(Control.MousePosition);
			GridHitInfo gridHitInfo = this.mGridViewIncludeFiles.CalcHitInfo(pt);
			if (gridHitInfo == null || gridHitInfo.HitTest == GridHitTest.None || gridHitInfo.HitTest == GridHitTest.EmptyRow)
			{
				return;
			}
			if (gridHitInfo.RowHandle < 0)
			{
				return;
			}
			IncludeFileParameterPresenter includeFileParameterPresenter;
			if (!this.TryGetPresenter(gridHitInfo.RowHandle, out includeFileParameterPresenter))
			{
				return;
			}
			if (!this.IsReadOnlyAtAll(includeFileParameterPresenter, gridHitInfo.Column) && !this.IsReadOnlyByCellContent(includeFileParameterPresenter, gridHitInfo.Column))
			{
				return;
			}
			if (!File.Exists(includeFileParameterPresenter.AbsoluteFilePath))
			{
				return;
			}
			FileSystemServices.LaunchFile(includeFileParameterPresenter.AbsoluteFilePath);
		}

		private void GridViewIncludeFiles_FocusedColumnChanged(object sender, FocusedColumnChangedEventArgs e)
		{
			this.mGridViewGeneralService.FocusedColumnChanged(e);
		}

		private void GridViewIncludeFiles_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
			this.mGridViewGeneralService.FocusedRowChanged(e);
			IncludeFileParameterPresenter includeFileParameterPresenter;
			if (!this.TryGetSelectedPresenter(out includeFileParameterPresenter))
			{
				this.mButtonMoveFirst.Enabled = false;
				this.mButtonMoveUp.Enabled = false;
				this.mButtonMoveDown.Enabled = false;
				this.mButtonMoveLast.Enabled = false;
			}
			else
			{
				this.mButtonMoveFirst.Enabled = (includeFileParameterPresenter.IncludeFileIndex > 0);
				this.mButtonMoveUp.Enabled = (includeFileParameterPresenter.IncludeFileIndex > 0);
				this.mButtonMoveDown.Enabled = (includeFileParameterPresenter.IncludeFileIndex < this.mIncludeFileConfiguration.IncludeFiles.Count - 1);
				this.mButtonMoveLast.Enabled = (includeFileParameterPresenter.IncludeFileIndex < this.mIncludeFileConfiguration.IncludeFiles.Count - 1);
			}
			this.Raise_SelectionChanged(this, EventArgs.Empty);
		}

		private void GridViewIncludeFiles_HiddenEditor(object sender, EventArgs e)
		{
			this.ValidateInput(true);
		}

		private void GridViewIncludeFiles_KeyDown(object sender, KeyEventArgs e)
		{
			IncludeFile includeFile;
			if (e.KeyCode == Keys.Delete && this.TryGetSelectedIncludeFile(out includeFile))
			{
				this.RemoveIncludeFile(includeFile);
			}
		}

		private void GridViewIncludeFiles_LeftCoordChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void GridViewIncludeFiles_GroupRowExpanded(object sender, RowEventArgs e)
		{
			this.mGridControlIncludeFiles.Refresh();
			this.DisplayErrors();
		}

		private void GridViewIncludeFiles_MouseDown(object sender, MouseEventArgs e)
		{
			this.mGridViewGeneralService.MouseDown(e);
		}

		private void GridViewIncludeFiles_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			this.mGridViewGeneralService.PopupMenuShowing(e);
			if (e.MenuType == GridMenuType.Column)
			{
				List<string> list = new List<string>
				{
					GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnClearSorting)
				};
				foreach (DXMenuItem dXMenuItem in e.Menu.Items)
				{
					if (list.Contains(dXMenuItem.Caption))
					{
						dXMenuItem.Visible = false;
					}
				}
			}
		}

		private void GridViewIncludeFiles_ShowingEditor(object sender, CancelEventArgs e)
		{
			IncludeFileParameterPresenter dataRecord;
			if (this.TryGetSelectedPresenter(out dataRecord))
			{
				this.mGridViewGeneralService.ShowingEditor<IncludeFileParameterPresenter>(e, dataRecord, this.mGridViewIncludeFiles.FocusedColumn, new GeneralService.IsReadOnlyAtAll<IncludeFileParameterPresenter>(this.IsReadOnlyAtAll), new GeneralService.IsReadOnlyByCellContent<IncludeFileParameterPresenter>(this.IsReadOnlyByCellContent));
				return;
			}
			e.Cancel = true;
		}

		private void GridViewIncludeFiles_ShownEditor(object sender, EventArgs e)
		{
			this.mGridViewKeyboardNavigationService.GridViewShownEditor();
		}

		private void GridViewIncludeFiles_TopRowChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void IncludeFileGrid_Resize(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private bool IsReadOnlyAtAll(IncludeFileParameterPresenter dataRecord, GridColumn column)
		{
			return false;
		}

		private bool IsReadOnlyByCellContent(IncludeFileParameterPresenter dataRecord, GridColumn column)
		{
			return column != this.mGridColumnValue || dataRecord.ParameterType == IncludeFileParameter.ParamType.Dummy || dataRecord.ParameterType == IncludeFileParameter.ParamType.Out;
		}

		public bool Serialize(IncludeFilesPage includeFilesPage)
		{
			if (includeFilesPage == null)
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.mGridViewIncludeFiles.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				includeFilesPage.IncludeFilesGridLayout = Convert.ToBase64String(array, 0, array.Length);
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

		public bool DeSerialize(IncludeFilesPage includeFilesPage)
		{
			if (includeFilesPage == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(includeFilesPage.IncludeFilesGridLayout))
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(includeFilesPage.IncludeFilesGridLayout);
				memoryStream = new MemoryStream(buffer);
				this.mGridViewIncludeFiles.RestoreLayoutFromStream(memoryStream);
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

		private void ButtonMoveFirst_Click(object sender, EventArgs e)
		{
			IncludeFile includeFile;
			int num;
			if (this.TryGetSelectedIncludeFile(out includeFile, out num))
			{
				if (num == 0)
				{
					return;
				}
				this.mIncludeFileConfiguration.RemoveIncludeFile(includeFile);
				this.mIncludeFileConfiguration.InsertIncludeFile(0, includeFile);
				this.UpdateFromConfig();
				this.ValidateInput(true);
				this.SelectRowOfIncludeFile(includeFile);
			}
		}

		private void ButtonMoveUp_Click(object sender, EventArgs e)
		{
			IncludeFile includeFile;
			int num;
			if (this.TryGetSelectedIncludeFile(out includeFile, out num))
			{
				if (num == 0)
				{
					return;
				}
				this.mIncludeFileConfiguration.RemoveIncludeFile(includeFile);
				this.mIncludeFileConfiguration.InsertIncludeFile(num - 1, includeFile);
				this.UpdateFromConfig();
				this.ValidateInput(true);
				this.SelectRowOfIncludeFile(includeFile);
			}
		}

		private void ButtonMoveDown_Click(object sender, EventArgs e)
		{
			IncludeFile includeFile;
			int num;
			if (this.TryGetSelectedIncludeFile(out includeFile, out num))
			{
				if (num == this.mIncludeFileConfiguration.IncludeFiles.Count - 1)
				{
					return;
				}
				this.mIncludeFileConfiguration.RemoveIncludeFile(includeFile);
				this.mIncludeFileConfiguration.InsertIncludeFile(num + 1, includeFile);
				this.UpdateFromConfig();
				this.ValidateInput(true);
				this.SelectRowOfIncludeFile(includeFile);
			}
		}

		private void ButtonMoveLast_Click(object sender, EventArgs e)
		{
			IncludeFile includeFile;
			int num;
			if (this.TryGetSelectedIncludeFile(out includeFile, out num))
			{
				if (num == this.mIncludeFileConfiguration.IncludeFiles.Count - 1)
				{
					return;
				}
				this.mIncludeFileConfiguration.RemoveIncludeFile(includeFile);
				this.mIncludeFileConfiguration.AddIncludeFile(includeFile);
				this.UpdateFromConfig();
				this.ValidateInput(true);
				this.SelectRowOfIncludeFile(includeFile);
			}
		}

		private void IncludeFileGrid_DragEnter(object sender, DragEventArgs e)
		{
			IList<string> list;
			e.Effect = (this.AcceptFileDrop(e, out list) ? DragDropEffects.Copy : DragDropEffects.None);
		}

		private void IncludeFileGrid_DragDrop(object sender, DragEventArgs e)
		{
			IList<string> list;
			if (this.AcceptFileDrop(e, out list))
			{
				using (IEnumerator<string> enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						this.AddIncludeFile(current);
					}
					return;
				}
			}
			e.Effect = DragDropEffects.None;
		}

		private bool AcceptFileDrop(DragEventArgs e, out IList<string> acceptedFiles)
		{
			acceptedFiles = null;
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				acceptedFiles = new List<string>();
				string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
				bool result = false;
				string[] array2 = array;
				int i = 0;
				while (i < array2.Length)
				{
					string text = array2[i];
					string extension;
					try
					{
						extension = Path.GetExtension(text);
					}
					catch
					{
						goto IL_7A;
					}
					goto IL_5D;
					IL_7A:
					i++;
					continue;
					IL_5D:
					if (string.Compare(extension, Vocabulary.FileExtensionDotINC, StringComparison.OrdinalIgnoreCase) == 0)
					{
						acceptedFiles.Add(text);
						result = true;
						goto IL_7A;
					}
					goto IL_7A;
				}
				return result;
			}
			return false;
		}

		private void XtraToolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
		{
			GridHitInfo gridHitInfo = this.mGridViewIncludeFiles.CalcHitInfo(e.ControlMousePosition);
			if (gridHitInfo == null)
			{
				return;
			}
			IncludeFileParameterPresenter includeFileParameterPresenter = this.mGridViewIncludeFiles.GetRow(gridHitInfo.RowHandle) as IncludeFileParameterPresenter;
			if (includeFileParameterPresenter == null)
			{
				return;
			}
			CellToolTipInfo @object = new CellToolTipInfo(gridHitInfo.RowHandle, gridHitInfo.Column, "cell");
			if (gridHitInfo.RowHandle < 0)
			{
				string errorText = this.mPageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, includeFileParameterPresenter.Parent.IncFileProperty);
				if (!string.IsNullOrEmpty(errorText))
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(errorText);
					stringBuilder.Append(Environment.NewLine);
					stringBuilder.Append(PathUtil.GetShortenedPath(includeFileParameterPresenter.AbsoluteFilePath, this.Font, this.mXtraToolTipController.MaxWidth, false));
					e.Info = new ToolTipControlInfo(@object, stringBuilder.ToString(), string.Empty, ToolTipIconType.Warning);
					return;
				}
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.Append(PathUtil.GetShortenedPath(includeFileParameterPresenter.AbsoluteFilePath, this.Font, this.mXtraToolTipController.MaxWidth, false));
				if (!string.IsNullOrEmpty(includeFileParameterPresenter.FileDescription))
				{
					stringBuilder2.Append(Environment.NewLine);
					stringBuilder2.Append(Environment.NewLine);
					stringBuilder2.Append(includeFileParameterPresenter.FileDescription);
				}
				e.Info = new ToolTipControlInfo(@object, stringBuilder2.ToString());
				return;
			}
			else
			{
				if (gridHitInfo.Column == this.mGridColumnParamNumber)
				{
					return;
				}
				if (gridHitInfo.Column != this.mGridColumnType)
				{
					if (gridHitInfo.Column == this.mGridColumnName)
					{
						if (!string.IsNullOrEmpty(includeFileParameterPresenter.LtlName))
						{
							string errorText2 = this.mPageValidator.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, includeFileParameterPresenter.NameProperty);
							if (!string.IsNullOrEmpty(errorText2))
							{
								StringBuilder stringBuilder3 = new StringBuilder();
								stringBuilder3.Append(errorText2);
								stringBuilder3.Append(Environment.NewLine);
								stringBuilder3.Append(string.Format(Resources_IncFiles.TooltipLtlName, includeFileParameterPresenter.LtlName));
								e.Info = new ToolTipControlInfo(@object, stringBuilder3.ToString(), string.Empty, ToolTipIconType.Warning);
								return;
							}
							string text = string.Format(Resources_IncFiles.TooltipLtlName, includeFileParameterPresenter.LtlName);
							e.Info = new ToolTipControlInfo(@object, text);
							return;
						}
					}
					else
					{
						if (gridHitInfo.Column == this.mGridColumnValue)
						{
							return;
						}
						if (gridHitInfo.Column == this.mGridColumnDescription && !string.IsNullOrEmpty(includeFileParameterPresenter.ExtendedDescription))
						{
							e.Info = new ToolTipControlInfo(@object, includeFileParameterPresenter.ExtendedDescription);
						}
					}
					return;
				}
				if (includeFileParameterPresenter.ParameterType == IncludeFileParameter.ParamType.Instance)
				{
					e.Info = new ToolTipControlInfo(@object, Resources_IncFiles.TooltipParameterTypeInstance);
					return;
				}
				if (includeFileParameterPresenter.ParameterType == IncludeFileParameter.ParamType.In)
				{
					e.Info = new ToolTipControlInfo(@object, Resources_IncFiles.TooltipParameterTypeIn);
					return;
				}
				if (includeFileParameterPresenter.ParameterType == IncludeFileParameter.ParamType.Out)
				{
					e.Info = new ToolTipControlInfo(@object, Resources_IncFiles.TooltipParameterTypeOut);
					return;
				}
				e.Info = new ToolTipControlInfo(@object, Resources_IncFiles.TooltipParameterTypeDummy);
				return;
			}
		}

		private bool TryGetAbsoluteIncludeFilePathByHitTest(out string absoluteFilePath)
		{
			absoluteFilePath = string.Empty;
			Point pt = this.mGridControlIncludeFiles.PointToClient(Control.MousePosition);
			GridHitInfo gridHitInfo = this.mGridViewIncludeFiles.CalcHitInfo(pt);
			if (gridHitInfo == null || gridHitInfo.HitTest == GridHitTest.None || gridHitInfo.HitTest == GridHitTest.EmptyRow)
			{
				return false;
			}
			IncludeFile includeFile;
			if (!this.TryGetIncludeFile(gridHitInfo.RowHandle, out includeFile))
			{
				return false;
			}
			absoluteFilePath = this.ModelValidator.GetAbsoluteFilePath(includeFile.FilePath.Value);
			return !string.IsNullOrEmpty(absoluteFilePath);
		}

		private void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			string path;
			if (!this.TryGetAbsoluteIncludeFilePathByHitTest(out path))
			{
				e.Cancel = true;
				return;
			}
			this.mToolStripMenuItemEditFile.Enabled = File.Exists(path);
			string directoryName = Path.GetDirectoryName(path);
			this.mToolStripMenuItemOpenFolder.Enabled = (!string.IsNullOrEmpty(directoryName) && Directory.Exists(directoryName));
		}

		private void ContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			string text;
			if (!this.TryGetAbsoluteIncludeFilePathByHitTest(out text))
			{
				return;
			}
			if (e.ClickedItem == this.mToolStripMenuItemEditFile)
			{
				FileSystemServices.LaunchFile(text);
				return;
			}
			if (e.ClickedItem == this.mToolStripMenuItemOpenFolder)
			{
				string directoryName = Path.GetDirectoryName(text);
				FileSystemServices.LaunchDirectoryBrowser(directoryName);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(IncludeFileGrid));
			this.mGridControlIncludeFiles = new GridControl();
			this.mContextMenuStrip = new ContextMenuStrip();
			this.mToolStripMenuItemEditFile = new ToolStripMenuItem();
			this.mToolStripMenuItemOpenFolder = new ToolStripMenuItem();
			this.mGridViewIncludeFiles = new GridView();
			this.mGridColumnFileName = new GridColumn();
			this.mRepositoryItemImageComboBoxGroupImage = new RepositoryItemImageComboBox();
			this.mGridColumnParamNumber = new GridColumn();
			this.mGridColumnType = new GridColumn();
			this.mRepositoryItemImageComboBoxParamType = new RepositoryItemImageComboBox();
			this.mGridColumnName = new GridColumn();
			this.mGridColumnValue = new GridColumn();
			this.mGridColumnDescription = new GridColumn();
			this.mXtraToolTipController = new XtraToolTipController();
			this.mButtonMoveFirst = new Button();
			this.mButtonMoveUp = new Button();
			this.mButtonMoveDown = new Button();
			this.mButtonMoveLast = new Button();
			this.mErrorProviderFormat = new ErrorProvider();
			this.mErrorProviderLocalModel = new ErrorProvider();
			this.mErrorProviderGlobalModel = new ErrorProvider();
			((ISupportInitialize)this.mGridControlIncludeFiles).BeginInit();
			this.mContextMenuStrip.SuspendLayout();
			((ISupportInitialize)this.mGridViewIncludeFiles).BeginInit();
			((ISupportInitialize)this.mRepositoryItemImageComboBoxGroupImage).BeginInit();
			((ISupportInitialize)this.mRepositoryItemImageComboBoxParamType).BeginInit();
			((ISupportInitialize)this.mErrorProviderFormat).BeginInit();
			((ISupportInitialize)this.mErrorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.mErrorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mGridControlIncludeFiles, "mGridControlIncludeFiles");
			this.mGridControlIncludeFiles.ContextMenuStrip = this.mContextMenuStrip;
			this.mGridControlIncludeFiles.LookAndFeel.Style = LookAndFeelStyle.Style3D;
			this.mGridControlIncludeFiles.LookAndFeel.UseDefaultLookAndFeel = false;
			this.mGridControlIncludeFiles.LookAndFeel.UseWindowsXPTheme = true;
			this.mGridControlIncludeFiles.MainView = this.mGridViewIncludeFiles;
			this.mGridControlIncludeFiles.Name = "mGridControlIncludeFiles";
			this.mGridControlIncludeFiles.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.mRepositoryItemImageComboBoxParamType,
				this.mRepositoryItemImageComboBoxGroupImage
			});
			this.mGridControlIncludeFiles.ToolTipController = this.mXtraToolTipController;
			this.mGridControlIncludeFiles.ViewCollection.AddRange(new BaseView[]
			{
				this.mGridViewIncludeFiles
			});
			this.mGridControlIncludeFiles.ProcessGridKey += new KeyEventHandler(this.GridControlIncludeFiles_ProcessGridKey);
			this.mGridControlIncludeFiles.EditorKeyDown += new KeyEventHandler(this.GridControlIncludeFiles_EditorKeyDown);
			this.mContextMenuStrip.Items.AddRange(new ToolStripItem[]
			{
				this.mToolStripMenuItemEditFile,
				this.mToolStripMenuItemOpenFolder
			});
			this.mContextMenuStrip.Name = "mContextMenuStrip";
			componentResourceManager.ApplyResources(this.mContextMenuStrip, "mContextMenuStrip");
			this.mContextMenuStrip.Opening += new CancelEventHandler(this.ContextMenuStrip_Opening);
			this.mContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.ContextMenuStrip_ItemClicked);
			this.mToolStripMenuItemEditFile.Image = Resources.ImageIncludeFile;
			this.mToolStripMenuItemEditFile.Name = "mToolStripMenuItemEditFile";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemEditFile, "mToolStripMenuItemEditFile");
			this.mToolStripMenuItemOpenFolder.Image = Resources.ImageOpenFolder;
			this.mToolStripMenuItemOpenFolder.Name = "mToolStripMenuItemOpenFolder";
			componentResourceManager.ApplyResources(this.mToolStripMenuItemOpenFolder, "mToolStripMenuItemOpenFolder");
			this.mGridViewIncludeFiles.Columns.AddRange(new GridColumn[]
			{
				this.mGridColumnFileName,
				this.mGridColumnParamNumber,
				this.mGridColumnType,
				this.mGridColumnName,
				this.mGridColumnValue,
				this.mGridColumnDescription
			});
			this.mGridViewIncludeFiles.GridControl = this.mGridControlIncludeFiles;
			this.mGridViewIncludeFiles.GroupCount = 1;
			componentResourceManager.ApplyResources(this.mGridViewIncludeFiles, "mGridViewIncludeFiles");
			this.mGridViewIncludeFiles.Name = "mGridViewIncludeFiles";
			this.mGridViewIncludeFiles.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
			this.mGridViewIncludeFiles.OptionsBehavior.ImmediateUpdateRowPosition = false;
			this.mGridViewIncludeFiles.OptionsCustomization.AllowColumnMoving = false;
			this.mGridViewIncludeFiles.OptionsCustomization.AllowFilter = false;
			this.mGridViewIncludeFiles.OptionsCustomization.AllowGroup = false;
			this.mGridViewIncludeFiles.OptionsCustomization.AllowQuickHideColumns = false;
			this.mGridViewIncludeFiles.OptionsCustomization.AllowSort = false;
			this.mGridViewIncludeFiles.OptionsFilter.AllowFilterEditor = false;
			this.mGridViewIncludeFiles.OptionsFind.AllowFindPanel = false;
			this.mGridViewIncludeFiles.OptionsLayout.Columns.StoreAppearance = true;
			this.mGridViewIncludeFiles.OptionsLayout.StoreAllOptions = true;
			this.mGridViewIncludeFiles.OptionsLayout.StoreAppearance = true;
			this.mGridViewIncludeFiles.OptionsNavigation.UseTabKey = false;
			this.mGridViewIncludeFiles.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
			this.mGridViewIncludeFiles.OptionsView.ShowGroupPanel = false;
			this.mGridViewIncludeFiles.OptionsView.ShowIndicator = false;
			this.mGridViewIncludeFiles.ShowButtonMode = ShowButtonModeEnum.ShowOnlyInEditor;
			this.mGridViewIncludeFiles.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.mGridColumnFileName, ColumnSortOrder.Ascending)
			});
			this.mGridViewIncludeFiles.CustomDrawCell += new RowCellCustomDrawEventHandler(this.GridViewIncludeFiles_CustomDrawCell);
			this.mGridViewIncludeFiles.LeftCoordChanged += new EventHandler(this.GridViewIncludeFiles_LeftCoordChanged);
			this.mGridViewIncludeFiles.TopRowChanged += new EventHandler(this.GridViewIncludeFiles_TopRowChanged);
			this.mGridViewIncludeFiles.PopupMenuShowing += new PopupMenuShowingEventHandler(this.GridViewIncludeFiles_PopupMenuShowing);
			this.mGridViewIncludeFiles.GroupRowExpanded += new RowEventHandler(this.GridViewIncludeFiles_GroupRowExpanded);
			this.mGridViewIncludeFiles.ShowingEditor += new CancelEventHandler(this.GridViewIncludeFiles_ShowingEditor);
			this.mGridViewIncludeFiles.HiddenEditor += new EventHandler(this.GridViewIncludeFiles_HiddenEditor);
			this.mGridViewIncludeFiles.ShownEditor += new EventHandler(this.GridViewIncludeFiles_ShownEditor);
			this.mGridViewIncludeFiles.FocusedRowChanged += new FocusedRowChangedEventHandler(this.GridViewIncludeFiles_FocusedRowChanged);
			this.mGridViewIncludeFiles.FocusedColumnChanged += new FocusedColumnChangedEventHandler(this.GridViewIncludeFiles_FocusedColumnChanged);
			this.mGridViewIncludeFiles.KeyDown += new KeyEventHandler(this.GridViewIncludeFiles_KeyDown);
			this.mGridViewIncludeFiles.MouseDown += new MouseEventHandler(this.GridViewIncludeFiles_MouseDown);
			this.mGridViewIncludeFiles.DoubleClick += new EventHandler(this.GridViewIncludeFiles_DoubleClick);
			componentResourceManager.ApplyResources(this.mGridColumnFileName, "mGridColumnFileName");
			this.mGridColumnFileName.ColumnEdit = this.mRepositoryItemImageComboBoxGroupImage;
			this.mGridColumnFileName.FieldName = "AbsoluteFilePath";
			this.mGridColumnFileName.FieldNameSortGroup = "IncludeFileIndex";
			this.mGridColumnFileName.Name = "mGridColumnFileName";
			componentResourceManager.ApplyResources(this.mRepositoryItemImageComboBoxGroupImage, "mRepositoryItemImageComboBoxGroupImage");
			this.mRepositoryItemImageComboBoxGroupImage.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("mRepositoryItemImageComboBoxGroupImage.Buttons"))
			});
			this.mRepositoryItemImageComboBoxGroupImage.Name = "mRepositoryItemImageComboBoxGroupImage";
			this.mRepositoryItemImageComboBoxGroupImage.ReadOnly = true;
			this.mGridColumnParamNumber.AppearanceCell.Options.UseTextOptions = true;
			this.mGridColumnParamNumber.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center;
			componentResourceManager.ApplyResources(this.mGridColumnParamNumber, "mGridColumnParamNumber");
			this.mGridColumnParamNumber.FieldName = "ParameterNumber";
			this.mGridColumnParamNumber.MaxWidth = 25;
			this.mGridColumnParamNumber.MinWidth = 25;
			this.mGridColumnParamNumber.Name = "mGridColumnParamNumber";
			this.mGridColumnParamNumber.OptionsColumn.FixedWidth = true;
			componentResourceManager.ApplyResources(this.mGridColumnType, "mGridColumnType");
			this.mGridColumnType.ColumnEdit = this.mRepositoryItemImageComboBoxParamType;
			this.mGridColumnType.FieldName = "TypeIndex";
			this.mGridColumnType.MaxWidth = 25;
			this.mGridColumnType.MinWidth = 25;
			this.mGridColumnType.Name = "mGridColumnType";
			this.mGridColumnType.OptionsColumn.FixedWidth = true;
			componentResourceManager.ApplyResources(this.mRepositoryItemImageComboBoxParamType, "mRepositoryItemImageComboBoxParamType");
			this.mRepositoryItemImageComboBoxParamType.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("mRepositoryItemImageComboBoxParamType.Buttons"))
			});
			this.mRepositoryItemImageComboBoxParamType.Name = "mRepositoryItemImageComboBoxParamType";
			this.mRepositoryItemImageComboBoxParamType.ReadOnly = true;
			componentResourceManager.ApplyResources(this.mGridColumnName, "mGridColumnName");
			this.mGridColumnName.FieldName = "Name";
			this.mGridColumnName.Name = "mGridColumnName";
			componentResourceManager.ApplyResources(this.mGridColumnValue, "mGridColumnValue");
			this.mGridColumnValue.FieldName = "Value";
			this.mGridColumnValue.Name = "mGridColumnValue";
			componentResourceManager.ApplyResources(this.mGridColumnDescription, "mGridColumnDescription");
			this.mGridColumnDescription.FieldName = "Description";
			this.mGridColumnDescription.Name = "mGridColumnDescription";
			this.mXtraToolTipController.AllowHtmlText = true;
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
			this.mXtraToolTipController.AutoPopDelay = 60000;
			this.mXtraToolTipController.MaxWidth = 800;
			this.mXtraToolTipController.ShowPrefix = true;
			this.mXtraToolTipController.UseNativeLookAndFeel = true;
			this.mXtraToolTipController.GetActiveObjectInfo += new ToolTipControllerGetActiveObjectInfoEventHandler(this.XtraToolTipController_GetActiveObjectInfo);
			componentResourceManager.ApplyResources(this.mButtonMoveFirst, "mButtonMoveFirst");
			this.mButtonMoveFirst.Image = Resources.ImageMoveFirst;
			this.mButtonMoveFirst.Name = "mButtonMoveFirst";
			this.mButtonMoveFirst.UseVisualStyleBackColor = true;
			this.mButtonMoveFirst.Click += new EventHandler(this.ButtonMoveFirst_Click);
			componentResourceManager.ApplyResources(this.mButtonMoveUp, "mButtonMoveUp");
			this.mButtonMoveUp.Image = Resources.ImageMovePrev;
			this.mButtonMoveUp.Name = "mButtonMoveUp";
			this.mButtonMoveUp.UseVisualStyleBackColor = true;
			this.mButtonMoveUp.Click += new EventHandler(this.ButtonMoveUp_Click);
			componentResourceManager.ApplyResources(this.mButtonMoveDown, "mButtonMoveDown");
			this.mButtonMoveDown.Image = Resources.ImageMoveNext;
			this.mButtonMoveDown.Name = "mButtonMoveDown";
			this.mButtonMoveDown.UseVisualStyleBackColor = true;
			this.mButtonMoveDown.Click += new EventHandler(this.ButtonMoveDown_Click);
			componentResourceManager.ApplyResources(this.mButtonMoveLast, "mButtonMoveLast");
			this.mButtonMoveLast.Image = Resources.ImageMoveLast;
			this.mButtonMoveLast.Name = "mButtonMoveLast";
			this.mButtonMoveLast.UseVisualStyleBackColor = true;
			this.mButtonMoveLast.Click += new EventHandler(this.ButtonMoveLast_Click);
			this.mErrorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProviderFormat.ContainerControl = this;
			this.mErrorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProviderLocalModel.ContainerControl = this;
			this.mErrorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.mErrorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.mErrorProviderGlobalModel, "mErrorProviderGlobalModel");
			this.AllowDrop = true;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.mButtonMoveLast);
			base.Controls.Add(this.mButtonMoveDown);
			base.Controls.Add(this.mButtonMoveUp);
			base.Controls.Add(this.mButtonMoveFirst);
			base.Controls.Add(this.mGridControlIncludeFiles);
			base.Name = "IncludeFileGrid";
			componentResourceManager.ApplyResources(this, "$this");
			base.DragDrop += new DragEventHandler(this.IncludeFileGrid_DragDrop);
			base.DragEnter += new DragEventHandler(this.IncludeFileGrid_DragEnter);
			base.Resize += new EventHandler(this.IncludeFileGrid_Resize);
			((ISupportInitialize)this.mGridControlIncludeFiles).EndInit();
			this.mContextMenuStrip.ResumeLayout(false);
			((ISupportInitialize)this.mGridViewIncludeFiles).EndInit();
			((ISupportInitialize)this.mRepositoryItemImageComboBoxGroupImage).EndInit();
			((ISupportInitialize)this.mRepositoryItemImageComboBoxParamType).EndInit();
			((ISupportInitialize)this.mErrorProviderFormat).EndInit();
			((ISupportInitialize)this.mErrorProviderLocalModel).EndInit();
			((ISupportInitialize)this.mErrorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
