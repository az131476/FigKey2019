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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.DatabasesPage
{
	internal class ExportDatabaseGrid : UserControl
	{
		private ExportDatabaseConfiguration exportDatabaseConfiguration;

		private GUIElementManager_ControlGridTree guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isDatabaseFileMissing;

		private Dictionary<ExportDatabase, bool> databaseToIsFileNotFound;

		private FileConversionParameters parameters;

		private ILoggerSpecifics MAX_LoggerSpecifics;

		private IList<ExportDatabase> exportDatabasesFromConfig;

		private IContainer components;

		private GridControl gridControlDatabases;

		private GridView gridViewDatabases;

		private BindingSource databaseBindingSource;

		private GridColumn colSize;

		private GridColumn colLocation;

		private GridColumn colFileName;

		private GridColumn colChannel;

		private GridColumn colNote;

		private RepositoryItemComboBox repositoryItemComboBox;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private GridColumn colNetwork;

		private XtraToolTipController toolTipController;

		public event EventHandler SelectionChanged;

		public ExportDatabaseConfiguration ExportDatabaseConfiguration
		{
			get
			{
				return this.exportDatabaseConfiguration;
			}
			set
			{
				this.exportDatabaseConfiguration = value;
				if (value != null)
				{
					int focusedRowHandle = this.gridViewDatabases.FocusedRowHandle;
					this.gridControlDatabases.DataSource = this.SelectedDatabaseList();
					if (focusedRowHandle >= 0 && focusedRowHandle < this.gridViewDatabases.RowCount)
					{
						this.gridViewDatabases.FocusedRowHandle = focusedRowHandle;
					}
					this.ValidateInput(false);
				}
			}
		}

		public bool IsDatabaseFileMissing
		{
			get
			{
				return this.isDatabaseFileMissing;
			}
		}

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

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		public IConfigurationManagerService ConfigurationManagerService
		{
			get;
			set;
		}

		public FileConversionParameters FileConversionParameters
		{
			get
			{
				return this.parameters;
			}
			set
			{
				this.parameters = value;
				if (this.parameters != null)
				{
					this.parameters.ApplyExportDatabasePersistenceToConfiguration();
					this.exportDatabaseConfiguration = this.parameters.ExportDatabaseConfiguration;
					this.RefreshGridData();
				}
			}
		}

		public ILoggerSpecifics CurrentLogger
		{
			get;
			set;
		}

		public ExportDatabaseGrid()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_ControlGridTree();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.FormatError, this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.isDatabaseFileMissing = false;
			this.databaseToIsFileNotFound = new Dictionary<ExportDatabase, bool>();
			this.MAX_LoggerSpecifics = new GL4000();
			this.exportDatabasesFromConfig = new List<ExportDatabase>();
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

		public void AddDatabase(Database new_db)
		{
			this.AddDatabase(new ExportDatabase(new_db)
			{
				RefType = ExportDatabase.ReferenceType.DBconfig
			});
		}

		public void AddDatabase(ExportDatabase new_db)
		{
			this.exportDatabaseConfiguration.AddDatabase(new_db);
			this.gridViewDatabases.RefreshData();
			this.RefreshGridData();
			this.SelectRowOfDatabase(new_db);
		}

		public void AddAutomaticAnalysisPackageDatabase(ExportDatabase new_db)
		{
			this.ExportDatabaseConfiguration.AnalysisPackageDatabaseList.Add(new_db);
			this.gridViewDatabases.RefreshData();
			this.RefreshGridData();
			this.SelectRowOfDatabase(new_db);
		}

		public void ClearAutomaticAnalysisPackageDatabase()
		{
			if (this.ExportDatabaseConfiguration != null)
			{
				this.ExportDatabaseConfiguration.AnalysisPackageDatabaseList.Clear();
				this.RefreshGridData();
			}
		}

		public void AddDatabase(string databasePath)
		{
			if (string.IsNullOrEmpty(databasePath))
			{
				return;
			}
			string text = "";
			Cursor.Current = Cursors.WaitCursor;
			bool flag = false;
			BusType busType = BusType.Bt_CAN;
			string text2 = string.Empty;
			string extension = Path.GetExtension(databasePath);
			if (extension.Length > 1)
			{
				text2 = extension.ToLower().Substring(1);
			}
			bool flag2 = text2.IndexOf("ldf", StringComparison.Ordinal) == 0;
			bool flag3 = text2.IndexOf("arxml", StringComparison.Ordinal) == 0;
			bool flag4 = text2.IndexOf("xml", StringComparison.Ordinal) == 0;
			bool flag5 = false;
			if (flag4 && this.ApplicationDatabaseManager.IsEthernetDatabase(databasePath, out text))
			{
				busType = BusType.Bt_Ethernet;
			}
			else if (flag2)
			{
				busType = BusType.Bt_LIN;
			}
			else if (flag3 || flag4)
			{
				IDictionary<string, BusType> dictionary;
				if (!this.ApplicationDatabaseManager.IsAutosarDescriptionFile(databasePath, out dictionary))
				{
					if (flag3)
					{
						Cursor.Current = Cursors.Default;
						InformMessageBox.Error(Resources.ErrorARXMLFileNotAUTOSAR);
						return;
					}
				}
				else
				{
					flag5 = true;
					Dictionary<string, BusType> dictionary2 = new Dictionary<string, BusType>(dictionary);
					if (dictionary2.Count == 0)
					{
						InformMessageBox.Error(Resources.ErrorNoNetworksOfSupportedBusType);
						return;
					}
					if (dictionary2.Count > 1)
					{
						NetworkSelection networkSelection = new NetworkSelection();
						networkSelection.Filename = Path.GetFileName(databasePath);
						networkSelection.NetworkNamesToBusType = dictionary2;
						if (networkSelection.ShowDialog() != DialogResult.OK)
						{
							return;
						}
						text = networkSelection.SelectedNetworkName;
					}
					else
					{
						text = dictionary.First<KeyValuePair<string, BusType>>().Key;
					}
					busType = dictionary[text];
				}
				Cursor.Current = Cursors.WaitCursor;
				if ((flag4 && !flag5) || busType == BusType.Bt_FlexRay)
				{
					busType = BusType.Bt_FlexRay;
					IDictionary<string, bool> dictionary3;
					if (!this.ApplicationDatabaseManager.IsFlexrayDatabase(databasePath, out dictionary3))
					{
						Cursor.Current = Cursors.Default;
						InformMessageBox.Error(Resources.ErrorXMLIsNotFlexrayOrEthernetDb);
						return;
					}
					if (!flag5)
					{
						text = dictionary3.First<KeyValuePair<string, bool>>().Key;
						flag = dictionary3.First<KeyValuePair<string, bool>>().Value;
					}
					else
					{
						if (!dictionary3.ContainsKey(text))
						{
							Cursor.Current = Cursors.Default;
							InformMessageBox.Error(Resources.ErrorSelFlexRayClusterNotSupported);
							return;
						}
						flag = dictionary3[text];
					}
				}
			}
			else if (this.ApplicationDatabaseManager.IsGMLANDatabase(databasePath) && InformMessageBox.Question(Resources.QuestionIsGMLANDbAddAnyway) == DialogResult.No)
			{
				return;
			}
			ExportDatabase exportDatabase = new ExportDatabase();
			exportDatabase.FilePath.Value = databasePath;
			exportDatabase.NetworkName.Value = text;
			exportDatabase.IsAUTOSARFile = flag5;
			exportDatabase.BusType.Value = busType;
			exportDatabase.RefType = ExportDatabase.ReferenceType.Path;
			if (BusType.Bt_FlexRay == busType)
			{
				if (flag)
				{
					exportDatabase.ChannelNumber.Value = Database.ChannelNumber_FlexrayAB;
				}
			}
			else
			{
				exportDatabase.ChannelNumber.Value = this.ModelValidator.GetFirstActiveOrDefaultChannel(busType);
			}
			IDictionary<string, bool> dictionary4 = new Dictionary<string, bool>();
			uint extraCPChannel = 0u;
			exportDatabase.CPType.Value = this.ConfigurationManagerService.DatabaseServices.GetDatabaseCPConfiguration(exportDatabase, out dictionary4, out extraCPChannel);
			if (exportDatabase.CPType.Value != CPType.None)
			{
				exportDatabase.CcpXcpEcuDisplayName.Value = Database.MakeCpEcuDisplayName(dictionary4.Keys);
				if (exportDatabase.CcpXcpEcuDisplayName.Value != Resources_CcpXcp.CcpXcpDefaultEcuName)
				{
					foreach (Database current in this.exportDatabaseConfiguration.CCPXCPDatabases)
					{
						if (exportDatabase.CcpXcpEcuDisplayName == current.CcpXcpEcuDisplayName && current.FileType != DatabaseFileType.A2L && current.IsCPActive.Value)
						{
							InformMessageBox.Error(Resources.ErrorDuplicateEcuName);
							return;
						}
					}
				}
				if (string.IsNullOrEmpty(exportDatabase.CcpXcpEcuDisplayName.Value))
				{
					exportDatabase.CcpXcpEcuDisplayName.Value = Resources_CcpXcp.CcpXcpDefaultEcuName;
				}
				foreach (string current2 in dictionary4.Keys)
				{
					exportDatabase.AddCpProtection(new CPProtection(current2, dictionary4[current2]));
				}
				if (BusType.Bt_FlexRay == busType)
				{
					exportDatabase.ExtraCPChannel = extraCPChannel;
					exportDatabase.ChannelNumber.Value = Database.ChannelNumber_FlexrayAB;
				}
				if (exportDatabase.CpProtectionsWithSeedAndKeyRequired.Count > 0)
				{
					exportDatabase.CcpXcpIsSeedAndKeyUsed = true;
				}
				exportDatabase.IsCPActive.Value = false;
			}
			this.exportDatabaseConfiguration.AddDatabase(exportDatabase);
			this.RefreshGridData();
			this.SelectRowOfDatabase(exportDatabase);
			Cursor.Current = Cursors.Default;
		}

		public void RemoveDatabase()
		{
			ExportDatabase database;
			if (this.TryGetSelectedDatabase(out database) && DialogResult.Yes == InformMessageBox.Question(Resources.QuestionRemoveSelectedDb))
			{
				this.exportDatabaseConfiguration.RemoveDatabase(database);
				this.RefreshGridData();
			}
		}

		public void ClearDatabases()
		{
			this.exportDatabaseConfiguration.ClearDatabases();
			this.RefreshGridData();
		}

		public void ClearDatabasesFromAnalysisPackage()
		{
			if (this.exportDatabaseConfiguration != null)
			{
				this.exportDatabaseConfiguration.ClearDatabasesFromAnalysisPackage();
				this.RefreshGridData();
			}
		}

		public string GetAnalysisPackagePath()
		{
			string result = string.Empty;
			ExportDatabase exportDatabase = this.ExportDatabaseConfiguration.AnalysisPackageDatabaseList.FirstOrDefault((ExportDatabase x) => x.IsFromAnalysisPackage() && !string.IsNullOrEmpty(x.AnalysisPackagePath.Value));
			if (exportDatabase != null)
			{
				result = exportDatabase.AnalysisPackagePath.Value;
			}
			return result;
		}

		private void gridViewDatabases_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			ExportDatabase db;
			if (!this.GetDatabase(e.ListSourceRowIndex, out db))
			{
				return;
			}
			if (e.Column == this.colFileName)
			{
				this.UnboundColumnFileName(db, e);
				return;
			}
			if (e.Column == this.colNetwork)
			{
				this.UnboundColumnNetwork(db, e);
				return;
			}
			if (e.Column == this.colSize)
			{
				this.UnboundColumnSize(db, e);
				return;
			}
			if (e.Column == this.colChannel)
			{
				this.UnboundColumnChannel(db, e);
				return;
			}
			if (e.Column == this.colLocation)
			{
				this.UnboundColumnLocation(db, e);
				return;
			}
			if (e.Column == this.colNote)
			{
				this.UnboundColumnNote(db, e);
			}
		}

		private void gridViewDatabases_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			if (e.Column == this.colFileName)
			{
				ExportDatabase exportDatabase;
				if (this.GetDatabase(this.gridViewDatabases.GetDataSourceRowIndex(e.RowHandle), out exportDatabase))
				{
					if (exportDatabase.IsAUTOSARFile)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageARXML);
						return;
					}
					if (exportDatabase.BusType.Value == BusType.Bt_CAN)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageDBC);
						return;
					}
					if (exportDatabase.BusType.Value == BusType.Bt_LIN)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageLDF);
						return;
					}
					if (exportDatabase.BusType.Value == BusType.Bt_FlexRay || exportDatabase.BusType.Value == BusType.Bt_Ethernet)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageXML);
						return;
					}
				}
			}
			else
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(e.Column, this.gridViewDatabases.GetDataSourceRowIndex(e.RowHandle));
				this.customErrorProvider.Grid.DisplayError(gUIElement, e);
			}
		}

		private void gridViewDatabases_ShownEditor(object sender, EventArgs e)
		{
			if (this.gridViewDatabases.FocusedColumn == this.colChannel)
			{
				this.ShownEditorChannel();
			}
		}

		private void repositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewDatabases.PostEditor();
		}

		private void gridViewDatabases_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
			this.Raise_SelectionChanged(this, EventArgs.Empty);
		}

		private void gridViewDatabases_DoubleClick(object sender, EventArgs e)
		{
			ExportDatabase exportDatabase = null;
			if (this.gridViewDatabases.FocusedColumn == this.colFileName)
			{
				if (this.TryGetSelectedDatabase(out exportDatabase) && !exportDatabase.IsInsideAnalysisPackage())
				{
					FileSystemServices.LaunchFile(this.ConfigurationManagerService.GetAbsoluteFilePath(exportDatabase.FilePath.Value));
					return;
				}
			}
			else if (this.gridViewDatabases.FocusedColumn == this.colLocation && this.TryGetSelectedDatabase(out exportDatabase))
			{
				string directoryPath;
				if (exportDatabase.IsInsideAnalysisPackage())
				{
					directoryPath = this.ConfigurationManagerService.GetAbsoluteFilePath(exportDatabase.AnalysisPackagePath.Value);
				}
				else
				{
					directoryPath = Path.GetDirectoryName(this.ConfigurationManagerService.GetAbsoluteFilePath(exportDatabase.FilePath.Value));
				}
				FileSystemServices.LaunchDirectoryBrowser(directoryPath);
			}
		}

		private void gridViewDatabases_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (e.MenuType == GridMenuType.Column)
			{
				this.CustomizeColumnHeaderMenu(e);
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

		private void gridViewDatabases_TopRowChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridViewDatabases_LeftCoordChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridViewDatabases_Resize(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridViewDatabases_RowStyle(object sender, RowStyleEventArgs e)
		{
			if (this.exportDatabaseConfiguration != null && this.exportDatabaseConfiguration.Mode != ExportDatabaseConfiguration.DBSelectionMode.Manual)
			{
				e.HighPriority = true;
				e.Appearance.ForeColor = SystemColors.GrayText;
				return;
			}
			ExportDatabase exportDatabase;
			if (this.GetDatabase(this.gridViewDatabases.GetDataSourceRowIndex(e.RowHandle), out exportDatabase) && exportDatabase.IsFromAnalysisPackage())
			{
				e.HighPriority = true;
				e.Appearance.ForeColor = SystemColors.GrayText;
			}
		}

		private void UnboundColumnFileName(ExportDatabase db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = Path.GetFileName(db.FilePath.Value);
			}
		}

		private void UnboundColumnNetwork(ExportDatabase db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = db.NetworkName.Value;
			}
		}

		private void UnboundColumnSize(ExportDatabase db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				try
				{
					if (db.IsInsideAnalysisPackage())
					{
						FileInfoProxy fileInfoProxy = new FileInfoProxy(db.AnalysisPackagePath.Value + Path.DirectorySeparatorChar + db.FilePath);
						e.Value = GUIUtil.MapFileSizeNumber2String(fileInfoProxy.Length);
					}
					else
					{
						FileInfo fileInfo = new FileInfo(this.ConfigurationManagerService.GetAbsoluteFilePath(db.FilePath.Value));
						e.Value = GUIUtil.MapFileSizeNumber2String(fileInfo.Length);
					}
				}
				catch (Exception)
				{
					e.Value = Resources.Unknown;
				}
			}
		}

		private void UnboundColumnChannel(ExportDatabase db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (db.BusType.Value == BusType.Bt_CAN)
				{
					e.Value = GUIUtil.MapCANChannelNumber2String(db.ChannelNumber.Value);
				}
				else if (db.BusType.Value == BusType.Bt_LIN)
				{
					e.Value = GUIUtil.MapLINChannelNumber2String(db.ChannelNumber.Value, false);
				}
				else if (db.BusType.Value == BusType.Bt_FlexRay)
				{
					e.Value = GUIUtil.MapFlexrayChannelNumber2String(db.ChannelNumber.Value);
				}
				else if (db.BusType.Value == BusType.Bt_Ethernet)
				{
					e.Value = GUIUtil.MapEthernetChannelNumber2String(db.ChannelNumber.Value);
				}
				this.pageValidator.Grid.StoreMapping(db.ChannelNumber, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
				return;
			}
			uint value;
			if (db.BusType.Value == BusType.Bt_CAN)
			{
				value = GUIUtil.MapCANChannelString2Number(e.Value.ToString());
			}
			else if (db.BusType.Value == BusType.Bt_LIN)
			{
				value = GUIUtil.MapLINChannelString2Number(e.Value.ToString());
			}
			else
			{
				if (db.BusType.Value != BusType.Bt_FlexRay)
				{
					return;
				}
				value = GUIUtil.MapFlexrayChannelString2Number(e.Value.ToString());
			}
			db.ChannelNumber.Value = value;
			this.ValidateInput(false);
		}

		private void UnboundColumnLocation(ExportDatabase db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (db.IsInsideAnalysisPackage())
				{
					e.Value = db.AnalysisPackagePath;
				}
				else
				{
					e.Value = FileSystemServices.GetFolderPathFromFilePath(db.FilePath.Value);
				}
				this.pageValidator.Grid.StoreMapping(db.FilePath, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
			}
		}

		private void UnboundColumnNote(ExportDatabase db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				string str = "";
				if (BusType.Bt_FlexRay == db.BusType.Value)
				{
					string arg = "";
					if (this.ConfigurationManagerService.DatabaseServices.GetFibexVersionString(db, out arg))
					{
						str = string.Format(Resources.FibexVersion, arg);
					}
				}
				string str2 = Resources_CcpXcp.NoteCCPXCPDisabledPostfix;
				if (db.IsCPActive.Value)
				{
					str2 = Resources_CcpXcp.NoteCCPXCPEnabledPostfix;
				}
				string str3;
				if (CPType.CCP == db.CPType.Value)
				{
					if (db.CpProtectionsWithSeedAndKeyRequired.Count > 0)
					{
						str3 = Resources_CcpXcp.CCPSetupWithSeedAndKeyPresent + str2;
					}
					else
					{
						str3 = Resources_CcpXcp.CCPSetupPresent + str2;
					}
				}
				else if (CPType.XCP == db.CPType.Value)
				{
					if (db.CpProtectionsWithSeedAndKeyRequired.Count > 0)
					{
						str3 = Resources_CcpXcp.XCPSetupWithSeedAndKeyPresent + str2;
					}
					else
					{
						str3 = Resources_CcpXcp.XCPSetupPresent + str2;
					}
				}
				else
				{
					str3 = string.Empty;
				}
				e.Value = str + str3;
			}
		}

		private void gridViewDatabases_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete && this.ExportDatabaseConfiguration.Mode == ExportDatabaseConfiguration.DBSelectionMode.Manual)
			{
				this.RemoveDatabase();
			}
		}

		private void ShownEditorChannel()
		{
			ComboBoxEdit comboBoxEdit = this.gridViewDatabases.ActiveEditor as ComboBoxEdit;
			if (comboBoxEdit != null)
			{
				ReadOnlyCollection<ExportDatabase> readOnlyCollection = this.gridControlDatabases.DataSource as ReadOnlyCollection<ExportDatabase>;
				if (readOnlyCollection == null)
				{
					return;
				}
				ExportDatabase exportDatabase = readOnlyCollection[this.gridViewDatabases.GetFocusedDataSourceRowIndex()];
				if (exportDatabase.BusType.Value == BusType.Bt_CAN)
				{
					if (exportDatabase.CPType.Value == CPType.None)
					{
						this.FillCANChannelCombobox(comboBoxEdit, true);
						return;
					}
					this.FillCANChannelCombobox(comboBoxEdit, false);
					return;
				}
				else
				{
					if (exportDatabase.BusType.Value == BusType.Bt_LIN)
					{
						this.FillLINChannelCombobox(comboBoxEdit);
						return;
					}
					if (exportDatabase.BusType.Value == BusType.Bt_FlexRay)
					{
						if (exportDatabase.ChannelNumber.Value != Database.ChannelNumber_FlexrayAB)
						{
							this.FillFlexrayChannelCombobox(comboBoxEdit);
							return;
						}
						comboBoxEdit.Properties.Items.Add(Vocabulary.FlexrayChannelAB);
					}
				}
			}
		}

		private void gridViewDatabases_ShowingEditor(object sender, CancelEventArgs e)
		{
			if (this.ExportDatabaseConfiguration.Mode != ExportDatabaseConfiguration.DBSelectionMode.Manual)
			{
				e.Cancel = true;
				return;
			}
			ExportDatabase exportDatabase;
			if (this.TryGetSelectedDatabase(out exportDatabase) && exportDatabase.IsFromAnalysisPackage())
			{
				e.Cancel = true;
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
			if (this.ModelValidator == null)
			{
				return true;
			}
			bool flag = true;
			flag &= this.ModelValidator.Validate(this.ExportDatabaseConfiguration, PageType.ExportDatabases, isDataChanged, this.pageValidator);
			this.isDatabaseFileMissing = false;
			foreach (ExportDatabase current in this.SelectedDatabaseList())
			{
				if (((IModelValidationResultCollector)this.pageValidator).GetErrorText(ValidationErrorClass.GlobalModelError, current.FilePath) == Resources.ErrorFileNotFound)
				{
					this.isDatabaseFileMissing = true;
					break;
				}
			}
			if (this.exportDatabaseConfiguration != null && this.ConfigurationManagerService != null)
			{
				this.ApplicationDatabaseManager.UpdateApplicationExportDatabaseConfiguration(this.exportDatabaseConfiguration, this.ConfigurationManagerService.ConfigFolderPath);
			}
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
		}

		public void RefreshFromConfig()
		{
			if (this.ConfigurationManagerService != null)
			{
				this.exportDatabasesFromConfig = (from x in this.ConfigurationManagerService.DatabaseConfiguration.Databases
				select new ExportDatabase(x)).ToList<ExportDatabase>();
				this.RefreshGridData();
			}
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
			PageValidatorGridUtil.StoreRowMappingForVisibleRows(this.gridViewDatabases, new PageValidatorGridUtil.StoreRowMappingHandler(this.OnPageValidatorGridUtilStoreRowMapping));
		}

		private void OnPageValidatorGridUtilStoreRowMapping(int dataSourceRowIdx)
		{
			if (dataSourceRowIdx < 0 || dataSourceRowIdx >= this.SelectedDatabaseList().Count)
			{
				return;
			}
			ExportDatabase database = this.SelectedDatabaseList()[dataSourceRowIdx];
			this.StoreMapping4VisibleColumns(database, dataSourceRowIdx);
		}

		private void StoreMapping4VisibleColumns(ExportDatabase database, int dataSourceIdx)
		{
			if (PageValidatorGridUtil.IsColumnVisible(this.colChannel, this.gridViewDatabases))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colChannel, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(database.ChannelNumber, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colLocation, this.gridViewDatabases))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colLocation, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(database.FilePath, gUIElement);
			}
		}

		public void RefreshGridData()
		{
			if (this.exportDatabaseConfiguration != null)
			{
				int focusedRowHandle = this.gridViewDatabases.FocusedRowHandle;
				this.gridControlDatabases.DataSource = this.SelectedDatabaseList();
				if (focusedRowHandle >= 0 && focusedRowHandle < this.gridViewDatabases.RowCount)
				{
					this.gridViewDatabases.FocusedRowHandle = focusedRowHandle;
				}
				this.gridViewDatabases.RefreshData();
				this.ApplyControlsToParameters();
				this.ValidateInput(false);
			}
		}

		public void ApplyControlsToParameters()
		{
			if (this.parameters != null)
			{
				this.parameters.SyncExportDatabaseConfigurationToPersistence();
			}
		}

		public void EnableGridControl(bool enabled)
		{
			this.gridViewDatabases.OptionsBehavior.Editable = enabled;
			this.ApplyControlsToParameters();
		}

		private void FillCANChannelCombobox(ComboBoxEdit comboBoxEdit, bool showVirtualChannels)
		{
			uint totalNumberOfLogicalChannels = this.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN);
			for (uint num = 1u; num <= totalNumberOfLogicalChannels; num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
			if (showVirtualChannels)
			{
				uint num2 = totalNumberOfLogicalChannels + 1u;
				for (uint num3 = num2; num3 < num2 + this.ConfigurationManagerService.LoggerSpecifics.CAN.NumberOfVirtualChannels; num3 += 1u)
				{
					comboBoxEdit.Properties.Items.Add(GUIUtil.MapCANChannelNumber2String(num3) + Resources.VirtualChannelPostfix);
				}
			}
		}

		private void FillLINChannelCombobox(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.GetTotalNumberOfLogicalChannels(BusType.Bt_LIN); num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapLINChannelNumber2String(num, this.ConfigurationManagerService.LoggerSpecifics));
			}
		}

		private void FillFlexrayChannelCombobox(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.GetTotalNumberOfLogicalChannels(BusType.Bt_FlexRay); num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapFlexrayChannelNumber2String(num));
			}
		}

		private uint GetTotalNumberOfLogicalChannels(BusType busType)
		{
			switch (busType)
			{
			case BusType.Bt_CAN:
				return Math.Max(this.MAX_LoggerSpecifics.CAN.NumberOfChannels, this.MAX_LoggerSpecifics.Multibus.NumberOfChannels);
			case BusType.Bt_LIN:
			{
				uint num = Math.Max(this.MAX_LoggerSpecifics.LIN.NumberOfChannels, this.MAX_LoggerSpecifics.Multibus.NumberOfPiggyConfigurableChannels);
				return num + this.MAX_LoggerSpecifics.LIN.NumberOfLINprobeChannels;
			}
			case BusType.Bt_FlexRay:
				return this.MAX_LoggerSpecifics.Flexray.NumberOfChannels;
			case BusType.Bt_J1708:
				return this.MAX_LoggerSpecifics.Multibus.NumberOfPiggyConfigurableChannels;
			default:
				return 0u;
			}
		}

		public bool TryGetSelectedDatabase(out ExportDatabase database)
		{
			int num;
			return this.TryGetSelectedDatabase(out database, out num);
		}

		private bool TryGetSelectedDatabase(out ExportDatabase database, out int idx)
		{
			database = null;
			idx = this.gridViewDatabases.GetFocusedDataSourceRowIndex();
			IList<ExportDatabase> list = this.SelectedDatabaseList();
			if (idx < 0 || idx > list.Count - 1)
			{
				return false;
			}
			database = list[idx];
			return null != database;
		}

		public void SelectRowOfDatabase(ExportDatabase db)
		{
			for (int i = 0; i < this.gridViewDatabases.RowCount; i++)
			{
				IList<ExportDatabase> list = this.gridViewDatabases.DataSource as IList<ExportDatabase>;
				if (list != null)
				{
					ReadOnlyCollection<ExportDatabase> readOnlyCollection = new ReadOnlyCollection<ExportDatabase>(list);
					if (readOnlyCollection != null)
					{
						ExportDatabase exportDatabase = readOnlyCollection[this.gridViewDatabases.GetDataSourceRowIndex(i)];
						if (exportDatabase == db)
						{
							this.gridViewDatabases.FocusedRowHandle = i;
							return;
						}
					}
				}
			}
		}

		private bool GetDatabase(int listSourceRowIndex, out ExportDatabase db)
		{
			db = null;
			IList<ExportDatabase> list = this.gridControlDatabases.DataSource as IList<ExportDatabase>;
			if (list == null)
			{
				return false;
			}
			ReadOnlyCollection<ExportDatabase> readOnlyCollection = new ReadOnlyCollection<ExportDatabase>(list);
			if (readOnlyCollection == null)
			{
				return false;
			}
			if (listSourceRowIndex < 0 || listSourceRowIndex > readOnlyCollection.Count - 1)
			{
				return false;
			}
			db = readOnlyCollection[listSourceRowIndex];
			return null != db;
		}

		private IList<ExportDatabase> SelectedDatabaseList()
		{
			if (this.ExportDatabaseConfiguration.Mode == ExportDatabaseConfiguration.DBSelectionMode.FromAnalysisPackage)
			{
				return this.ExportDatabaseConfiguration.AnalysisPackageDatabaseList;
			}
			if (this.ExportDatabaseConfiguration.Mode == ExportDatabaseConfiguration.DBSelectionMode.FromConfig && this.ConfigurationManagerService != null)
			{
				return this.exportDatabasesFromConfig;
			}
			return this.exportDatabaseConfiguration.Databases;
		}

		public bool Serialize(ExportDatabasesPage exportDatabasesPage)
		{
			if (exportDatabasesPage == null)
			{
				return false;
			}
			exportDatabasesPage.FileConversionParameters = this.FileConversionParameters;
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.gridViewDatabases.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				exportDatabasesPage.DatabasesGridLayout = Convert.ToBase64String(array, 0, array.Length);
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

		public bool DeSerialize(ExportDatabasesPage exportDatabasesPage)
		{
			if (exportDatabasesPage == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(exportDatabasesPage.DatabasesGridLayout))
			{
				return false;
			}
			if (exportDatabasesPage.FileConversionParameters != null)
			{
				this.FileConversionParameters = exportDatabasesPage.FileConversionParameters;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(exportDatabasesPage.DatabasesGridLayout);
				memoryStream = new MemoryStream(buffer);
				this.gridViewDatabases.RestoreLayoutFromStream(memoryStream);
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

		private void toolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
		{
			ToolTipControlInfo toolTipControlInfo = null;
			GridView gridView = this.gridControlDatabases.GetViewAt(e.ControlMousePosition) as GridView;
			if (gridView == null)
			{
				return;
			}
			GridHitInfo gridHitInfo = gridView.CalcHitInfo(e.ControlMousePosition);
			if (gridHitInfo.InRowCell && gridHitInfo.Column.AbsoluteIndex == 4)
			{
				string text = string.Format("<i>{0}</i>", Resources.DoubleClickOpensWindowExplorer);
				string text2 = string.Empty;
				toolTipControlInfo = new ToolTipControlInfo();
				toolTipControlInfo.AllowHtmlText = DefaultBoolean.True;
				if (e.Info == null)
				{
					text2 = text;
				}
				else
				{
					text2 = string.Format("{0}<br>{1}", e.Info.Text, text);
				}
				toolTipControlInfo.Object = gridHitInfo.HitTest.ToString() + gridHitInfo.RowHandle.ToString();
				toolTipControlInfo.Text = text2;
			}
			if (toolTipControlInfo != null)
			{
				e.Info = toolTipControlInfo;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ExportDatabaseGrid));
			this.gridControlDatabases = new GridControl();
			this.databaseBindingSource = new BindingSource(this.components);
			this.gridViewDatabases = new GridView();
			this.colFileName = new GridColumn();
			this.colNetwork = new GridColumn();
			this.colChannel = new GridColumn();
			this.repositoryItemComboBox = new RepositoryItemComboBox();
			this.colSize = new GridColumn();
			this.colLocation = new GridColumn();
			this.colNote = new GridColumn();
			this.toolTipController = new XtraToolTipController(this.components);
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.gridControlDatabases).BeginInit();
			((ISupportInitialize)this.databaseBindingSource).BeginInit();
			((ISupportInitialize)this.gridViewDatabases).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBox).BeginInit();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.gridControlDatabases.DataSource = this.databaseBindingSource;
			componentResourceManager.ApplyResources(this.gridControlDatabases, "gridControlDatabases");
			this.gridControlDatabases.MainView = this.gridViewDatabases;
			this.gridControlDatabases.Name = "gridControlDatabases";
			this.gridControlDatabases.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemComboBox
			});
			this.gridControlDatabases.ToolTipController = this.toolTipController;
			this.gridControlDatabases.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewDatabases
			});
			this.gridControlDatabases.Resize += new EventHandler(this.gridViewDatabases_Resize);
			this.databaseBindingSource.DataSource = typeof(Database);
			this.gridViewDatabases.Columns.AddRange(new GridColumn[]
			{
				this.colFileName,
				this.colNetwork,
				this.colChannel,
				this.colSize,
				this.colLocation,
				this.colNote
			});
			this.gridViewDatabases.GridControl = this.gridControlDatabases;
			this.gridViewDatabases.Name = "gridViewDatabases";
			this.gridViewDatabases.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
			this.gridViewDatabases.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
			this.gridViewDatabases.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
			this.gridViewDatabases.OptionsCustomization.AllowFilter = false;
			this.gridViewDatabases.OptionsCustomization.AllowGroup = false;
			this.gridViewDatabases.OptionsCustomization.AllowQuickHideColumns = false;
			this.gridViewDatabases.OptionsDetail.EnableMasterViewMode = false;
			this.gridViewDatabases.OptionsView.ShowGroupPanel = false;
			this.gridViewDatabases.OptionsView.ShowIndicator = false;
			this.gridViewDatabases.PaintStyleName = "WindowsXP";
			this.gridViewDatabases.RowHeight = 20;
			this.gridViewDatabases.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewDatabases_CustomDrawCell);
			this.gridViewDatabases.RowStyle += new RowStyleEventHandler(this.gridViewDatabases_RowStyle);
			this.gridViewDatabases.LeftCoordChanged += new EventHandler(this.gridViewDatabases_LeftCoordChanged);
			this.gridViewDatabases.TopRowChanged += new EventHandler(this.gridViewDatabases_TopRowChanged);
			this.gridViewDatabases.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewDatabases_PopupMenuShowing);
			this.gridViewDatabases.ShowingEditor += new CancelEventHandler(this.gridViewDatabases_ShowingEditor);
			this.gridViewDatabases.ShownEditor += new EventHandler(this.gridViewDatabases_ShownEditor);
			this.gridViewDatabases.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gridViewDatabases_FocusedRowChanged);
			this.gridViewDatabases.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewDatabases_CustomUnboundColumnData);
			this.gridViewDatabases.KeyDown += new KeyEventHandler(this.gridViewDatabases_KeyDown);
			this.gridViewDatabases.DoubleClick += new EventHandler(this.gridViewDatabases_DoubleClick);
			componentResourceManager.ApplyResources(this.colFileName, "colFileName");
			this.colFileName.FieldName = "anyString1";
			this.colFileName.Name = "colFileName";
			this.colFileName.OptionsColumn.AllowEdit = false;
			this.colFileName.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colNetwork, "colNetwork");
			this.colNetwork.FieldName = "anyString6";
			this.colNetwork.Name = "colNetwork";
			this.colNetwork.OptionsColumn.AllowEdit = false;
			this.colNetwork.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colChannel, "colChannel");
			this.colChannel.ColumnEdit = this.repositoryItemComboBox;
			this.colChannel.FieldName = "anyString2";
			this.colChannel.Name = "colChannel";
			this.colChannel.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemComboBox, "repositoryItemComboBox");
			this.repositoryItemComboBox.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBox.Buttons"))
			});
			this.repositoryItemComboBox.Name = "repositoryItemComboBox";
			this.repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBox.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBox_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.colSize, "colSize");
			this.colSize.FieldName = "anyString3";
			this.colSize.Name = "colSize";
			this.colSize.OptionsColumn.AllowEdit = false;
			this.colSize.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colLocation, "colLocation");
			this.colLocation.FieldName = "anyString4";
			this.colLocation.Name = "colLocation";
			this.colLocation.OptionsColumn.AllowEdit = false;
			this.colLocation.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colNote, "colNote");
			this.colNote.FieldName = "anyString5";
			this.colNote.Name = "colNote";
			this.colNote.OptionsColumn.AllowEdit = false;
			this.colNote.UnboundType = UnboundColumnType.String;
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
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			this.AllowDrop = true;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.gridControlDatabases);
			base.Name = "ExportDatabaseGrid";
			componentResourceManager.ApplyResources(this, "$this");
			((ISupportInitialize)this.gridControlDatabases).EndInit();
			((ISupportInitialize)this.databaseBindingSource).EndInit();
			((ISupportInitialize)this.gridViewDatabases).EndInit();
			((ISupportInitialize)this.repositoryItemComboBox).EndInit();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
