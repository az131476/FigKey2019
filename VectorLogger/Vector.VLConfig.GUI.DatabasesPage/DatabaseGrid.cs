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
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.DatabasesPage
{
	internal class DatabaseGrid : UserControl
	{
		private DatabaseConfiguration databaseConfiguration;

		private GUIElementManager_ControlGridTree guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isDatabaseFileMissing;

		private Dictionary<Database, bool> databaseToIsFileNotFound;

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

		public DatabaseConfiguration DatabaseConfiguration
		{
			get
			{
				return this.databaseConfiguration;
			}
			set
			{
				this.databaseConfiguration = value;
				if (value != null)
				{
					int focusedRowHandle = this.gridViewDatabases.FocusedRowHandle;
					this.gridControlDatabases.DataSource = this.databaseConfiguration.Databases;
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

		public IModelEditor ModelEditor
		{
			get;
			set;
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		public ILoggerSpecifics CurrentLogger
		{
			get;
			set;
		}

		public DatabaseGrid()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_ControlGridTree();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.FormatError, this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.isDatabaseFileMissing = false;
			this.databaseToIsFileNotFound = new Dictionary<Database, bool>();
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

		public void AddDatabase(Database db)
		{
			this.databaseConfiguration.AddDatabase(db);
			this.gridViewDatabases.RefreshData();
			this.ValidateInput(true);
			this.SelectRowOfDatabase(db);
		}

		public void AddDatabase(string databasePath)
		{
			if (string.IsNullOrEmpty(databasePath))
			{
				return;
			}
			string text = "";
			Cursor.Current = Cursors.WaitCursor;
			List<uint> list = new List<uint>();
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
				if (!this.ModelValidator.LoggerSpecifics.Recording.HasEthernet)
				{
					Cursor.Current = Cursors.Default;
					InformMessageBox.Error(Resources.ErrorNoEthernetDbAllowed);
					return;
				}
				if (!this.SemanticChecker.IsEthernetDbAddable())
				{
					return;
				}
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
					if (this.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels == 0u)
					{
						Cursor.Current = Cursors.Default;
						InformMessageBox.Error(Resources.ErrorXMLFileNotAUTOSAR);
						return;
					}
				}
				else
				{
					flag5 = true;
					Dictionary<string, BusType> dictionary2;
					if (this.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels == 0u)
					{
						dictionary2 = new Dictionary<string, BusType>();
						using (IEnumerator<string> enumerator = dictionary.Keys.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								string current = enumerator.Current;
								if (dictionary[current] != BusType.Bt_FlexRay)
								{
									dictionary2.Add(current, dictionary[current]);
								}
							}
							goto IL_1EC;
						}
					}
					dictionary2 = new Dictionary<string, BusType>(dictionary);
					IL_1EC:
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
					if (!this.SemanticChecker.IsFlexrayDbAddable(ref list))
					{
						return;
					}
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
					if (flag)
					{
						if (list.Count < 2)
						{
							Cursor.Current = Cursors.Default;
							InformMessageBox.Error(Resources.ErrorFlexrayDBNeedsBothChannels);
							return;
						}
						Cursor.Current = Cursors.Default;
						if (DialogResult.No == InformMessageBox.Question(Resources.FlexrayDbNeedsBothChannels))
						{
							return;
						}
						Cursor.Current = Cursors.WaitCursor;
					}
				}
			}
			else
			{
				if (this.ApplicationDatabaseManager.IsCanFdDatabase(databasePath, text) && !this.ModelValidator.LoggerSpecifics.CAN.IsFDSupported)
				{
					Cursor.Current = Cursors.Default;
					InformMessageBox.Error(Resources.ErrorCanFdNotSupportedByLoggerType);
					return;
				}
				if (this.ApplicationDatabaseManager.IsGMLANDatabase(databasePath) && InformMessageBox.Question(Resources.QuestionIsGMLANDbAddAnyway) == DialogResult.No)
				{
					return;
				}
			}
			Database database = new Database();
			database.FilePath.Value = databasePath;
			database.NetworkName.Value = text;
			database.IsAUTOSARFile = flag5;
			database.BusType.Value = busType;
			if (BusType.Bt_FlexRay == busType)
			{
				if (flag)
				{
					database.ChannelNumber.Value = Database.ChannelNumber_FlexrayAB;
				}
				else
				{
					database.ChannelNumber.Value = list[0];
				}
			}
			else
			{
				database.ChannelNumber.Value = this.ModelValidator.GetFirstActiveOrDefaultChannel(busType);
			}
			database.FilePath.Value = this.ModelValidator.GetFilePathRelativeToConfiguration(databasePath);
			uint extraCPChannel = 0u;
			IDictionary<string, bool> dictionary4;
			database.CPType.Value = this.ModelValidator.DatabaseServices.GetDatabaseCPConfiguration(database, out dictionary4, out extraCPChannel);
			if (database.CPType.Value != CPType.None)
			{
				database.CcpXcpEcuDisplayName.Value = Database.MakeCpEcuDisplayName(dictionary4.Keys);
				if (database.CcpXcpEcuDisplayName.Value != Resources_CcpXcp.CcpXcpDefaultEcuName)
				{
					foreach (Database current2 in this.databaseConfiguration.CCPXCPDatabases)
					{
						if (database.CcpXcpEcuDisplayName == current2.CcpXcpEcuDisplayName && current2.FileType != DatabaseFileType.A2L && current2.IsCPActive.Value)
						{
							InformMessageBox.Error(Resources.ErrorDuplicateEcuName);
							return;
						}
					}
				}
				if (string.IsNullOrEmpty(database.CcpXcpEcuDisplayName.Value))
				{
					database.CcpXcpEcuDisplayName.Value = Resources_CcpXcp.CcpXcpDefaultEcuName;
				}
				foreach (string current3 in dictionary4.Keys)
				{
					database.AddCpProtection(new CPProtection(current3, dictionary4[current3]));
				}
				if (BusType.Bt_FlexRay == busType)
				{
					if (list.Count < 2)
					{
						Cursor.Current = Cursors.Default;
						InformMessageBox.Error(Resources_CcpXcp.ErrorUnableToAddFlexRayDbWithCP);
						return;
					}
					database.ExtraCPChannel = extraCPChannel;
					database.ChannelNumber.Value = Database.ChannelNumber_FlexrayAB;
				}
				if (database.CpProtectionsWithSeedAndKeyRequired.Count > 0)
				{
					database.CcpXcpIsSeedAndKeyUsed = true;
				}
				database.IsCPActive.Value = false;
			}
			this.databaseConfiguration.AddDatabase(database);
			this.gridViewDatabases.RefreshData();
			this.ValidateInput(true);
			this.SelectRowOfDatabase(database);
			Cursor.Current = Cursors.Default;
		}

		public void RemoveDatabase()
		{
			Database database;
			if (this.TryGetSelectedDatabase(out database) && DialogResult.Yes == InformMessageBox.Question(Resources.QuestionRemoveSelectedDb))
			{
				this.databaseConfiguration.RemoveDatabase(database);
				this.ValidateInput(true);
			}
		}

		private void gridViewDatabases_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			Database db;
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
				Database database;
				if (this.GetDatabase(this.gridViewDatabases.GetDataSourceRowIndex(e.RowHandle), out database))
				{
					if (database.IsAUTOSARFile)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageARXML);
						return;
					}
					if (database.BusType.Value == BusType.Bt_CAN)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageDBC);
						return;
					}
					if (database.BusType.Value == BusType.Bt_LIN)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageLDF);
						return;
					}
					if (database.BusType.Value == BusType.Bt_FlexRay || database.BusType.Value == BusType.Bt_Ethernet)
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
			Database database = null;
			if (this.gridViewDatabases.FocusedColumn == this.colFileName)
			{
				if (this.TryGetSelectedDatabase(out database))
				{
					FileSystemServices.LaunchFile(this.ModelValidator.GetAbsoluteFilePath(database.FilePath.Value));
					return;
				}
			}
			else if (this.gridViewDatabases.FocusedColumn == this.colLocation && this.TryGetSelectedDatabase(out database))
			{
				string directoryName = Path.GetDirectoryName(this.ModelValidator.GetAbsoluteFilePath(database.FilePath.Value));
				FileSystemServices.LaunchDirectoryBrowser(directoryName);
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

		private void DatabaseGrid_Resize(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void UnboundColumnFileName(Database db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = Path.GetFileName(db.FilePath.Value);
			}
		}

		private void UnboundColumnNetwork(Database db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = db.NetworkName.Value;
			}
		}

		private void UnboundColumnSize(Database db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				try
				{
					FileInfo fileInfo = new FileInfo(this.ModelValidator.GetAbsoluteFilePath(db.FilePath.Value));
					e.Value = GUIUtil.MapFileSizeNumber2String(fileInfo.Length);
				}
				catch (Exception)
				{
					e.Value = Resources.Unknown;
				}
			}
		}

		private void UnboundColumnChannel(Database db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (db.BusType.Value == BusType.Bt_CAN)
				{
					if (db.ChannelNumber.Value <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels)
					{
						e.Value = GUIUtil.MapCANChannelNumber2String(db.ChannelNumber.Value);
					}
					else if (db.ChannelNumber.Value <= this.ModelValidator.LoggerSpecifics.Multibus.NumberOfChannels)
					{
						e.Value = GUIUtil.MapCANChannelNumber2String(db.ChannelNumber.Value);
					}
					else
					{
						e.Value = GUIUtil.MapCANChannelNumber2String(db.ChannelNumber.Value) + Resources.VirtualChannelPostfix;
					}
				}
				else if (db.BusType.Value == BusType.Bt_LIN)
				{
					e.Value = GUIUtil.MapLINChannelNumber2String(db.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics);
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
			uint newChannelNumber;
			if (db.BusType.Value == BusType.Bt_CAN)
			{
				newChannelNumber = GUIUtil.MapCANChannelString2Number(e.Value.ToString());
			}
			else if (db.BusType.Value == BusType.Bt_LIN)
			{
				newChannelNumber = GUIUtil.MapLINChannelString2Number(e.Value.ToString());
			}
			else
			{
				if (db.BusType.Value != BusType.Bt_FlexRay)
				{
					return;
				}
				newChannelNumber = GUIUtil.MapFlexrayChannelString2Number(e.Value.ToString());
			}
			if (this.ModelEditor.CheckAndProcessDatabaseChannelRemapping(db, newChannelNumber))
			{
				this.ValidateInput(true);
			}
		}

		private void UnboundColumnLocation(Database db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = FileSystemServices.GetFolderPathFromFilePath(db.FilePath.Value);
				this.pageValidator.Grid.StoreMapping(db.FilePath, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
			}
		}

		private void UnboundColumnNote(Database db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				string str = "";
				if (BusType.Bt_FlexRay == db.BusType.Value)
				{
					string arg = "";
					if (this.ModelValidator.DatabaseServices.GetFibexVersionString(db, out arg))
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
			if (e.KeyCode == Keys.Delete)
			{
				this.RemoveDatabase();
			}
		}

		private void ShownEditorChannel()
		{
			ComboBoxEdit comboBoxEdit = this.gridViewDatabases.ActiveEditor as ComboBoxEdit;
			if (comboBoxEdit != null)
			{
				ReadOnlyCollection<Database> readOnlyCollection = this.gridControlDatabases.DataSource as ReadOnlyCollection<Database>;
				if (readOnlyCollection == null)
				{
					return;
				}
				Database database = readOnlyCollection[this.gridViewDatabases.GetFocusedDataSourceRowIndex()];
				if (database.BusType.Value == BusType.Bt_CAN)
				{
					if (database.CPType.Value == CPType.None)
					{
						this.FillCANChannelCombobox(comboBoxEdit, true);
						return;
					}
					this.FillCANChannelCombobox(comboBoxEdit, false);
					return;
				}
				else
				{
					if (database.BusType.Value == BusType.Bt_LIN)
					{
						this.FillLINChannelCombobox(comboBoxEdit);
						return;
					}
					if (database.BusType.Value == BusType.Bt_FlexRay)
					{
						if (database.ChannelNumber.Value != Database.ChannelNumber_FlexrayAB)
						{
							this.FillFlexrayChannelCombobox(comboBoxEdit);
							return;
						}
						comboBoxEdit.Properties.Items.Add(Vocabulary.FlexrayChannelAB);
					}
				}
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
			flag &= this.ModelValidator.Validate(this.DatabaseConfiguration, PageType.Databases, isDataChanged, this.pageValidator);
			this.isDatabaseFileMissing = false;
			foreach (Database current in this.DatabaseConfiguration.Databases)
			{
				if (((IModelValidationResultCollector)this.pageValidator).GetErrorText(ValidationErrorClass.GlobalModelError, current.FilePath) == Resources.ErrorFileNotFound)
				{
					this.isDatabaseFileMissing = true;
					break;
				}
			}
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
			PageValidatorGridUtil.StoreRowMappingForVisibleRows(this.gridViewDatabases, new PageValidatorGridUtil.StoreRowMappingHandler(this.OnPageValidatorGridUtilStoreRowMapping));
		}

		private void OnPageValidatorGridUtilStoreRowMapping(int dataSourceRowIdx)
		{
			if (dataSourceRowIdx < 0 || dataSourceRowIdx >= this.databaseConfiguration.Databases.Count)
			{
				return;
			}
			Database database = this.DatabaseConfiguration.Databases[dataSourceRowIdx];
			this.StoreMapping4VisibleColumns(database, dataSourceRowIdx);
		}

		private void StoreMapping4VisibleColumns(Database database, int dataSourceIdx)
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

		private void FillCANChannelCombobox(ComboBoxEdit comboBoxEdit, bool showVirtualChannels)
		{
			uint totalNumberOfLogicalChannels = this.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN);
			for (uint num = 1u; num <= totalNumberOfLogicalChannels; num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
			if (showVirtualChannels)
			{
				uint num2 = totalNumberOfLogicalChannels + 1u;
				for (uint num3 = num2; num3 < num2 + this.ModelValidator.LoggerSpecifics.CAN.NumberOfVirtualChannels; num3 += 1u)
				{
					comboBoxEdit.Properties.Items.Add(GUIUtil.MapCANChannelNumber2String(num3) + Resources.VirtualChannelPostfix);
				}
			}
		}

		private void FillLINChannelCombobox(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_LIN); num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapLINChannelNumber2String(num, this.ModelValidator.LoggerSpecifics));
			}
		}

		private void FillFlexrayChannelCombobox(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_FlexRay); num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapFlexrayChannelNumber2String(num));
			}
		}

		public bool TryGetSelectedDatabase(out Database database)
		{
			int num;
			return this.TryGetSelectedDatabase(out database, out num);
		}

		private bool TryGetSelectedDatabase(out Database database, out int idx)
		{
			database = null;
			idx = this.gridViewDatabases.GetFocusedDataSourceRowIndex();
			if (idx < 0 || idx > this.databaseConfiguration.Databases.Count - 1)
			{
				return false;
			}
			database = this.databaseConfiguration.Databases[idx];
			return null != database;
		}

		public void SelectRowOfDatabase(Database db)
		{
			for (int i = 0; i < this.gridViewDatabases.RowCount; i++)
			{
				IList<Database> list = this.gridViewDatabases.DataSource as IList<Database>;
				if (list != null)
				{
					Database database = list[this.gridViewDatabases.GetDataSourceRowIndex(i)];
					if (database == db)
					{
						this.gridViewDatabases.FocusedRowHandle = i;
						return;
					}
				}
			}
		}

		private bool GetDatabase(int listSourceRowIndex, out Database db)
		{
			db = null;
			ReadOnlyCollection<Database> readOnlyCollection = this.gridControlDatabases.DataSource as ReadOnlyCollection<Database>;
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

		public bool Serialize(DatabasesPage databasesPage)
		{
			if (databasesPage == null)
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.gridViewDatabases.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				databasesPage.DatabasesGridLayout = Convert.ToBase64String(array, 0, array.Length);
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

		public bool DeSerialize(DatabasesPage databasesPage)
		{
			if (databasesPage == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(databasesPage.DatabasesGridLayout))
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(databasesPage.DatabasesGridLayout);
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

		private void DatabaseGrid_DragEnter(object sender, DragEventArgs e)
		{
			IList<string> list;
			if (this.AcceptFileDrop(e, out list))
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void DatabaseGrid_DragDrop(object sender, DragEventArgs e)
		{
			IList<string> list;
			if (this.AcceptFileDrop(e, out list))
			{
				using (IEnumerator<string> enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						this.AddDatabase(current);
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
				string strA = "";
				bool result = false;
				string[] array2 = array;
				int i = 0;
				while (i < array2.Length)
				{
					string text = array2[i];
					try
					{
						strA = Path.GetExtension(text);
					}
					catch
					{
						goto IL_115;
					}
					goto IL_64;
					IL_115:
					i++;
					continue;
					IL_64:
					bool flag = string.Compare(strA, Vocabulary.FileExtensionDotDBC, true) == 0;
					bool flag2 = string.Compare(strA, Vocabulary.FileExtensionDotLDF, true) == 0;
					bool flag3 = string.Compare(strA, Vocabulary.FileExtensionDotARXML, true) == 0;
					bool flag4 = string.Compare(strA, Vocabulary.FileExtensionDotXML, true) == 0;
					bool flag5 = this.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels > 0u;
					bool isARXMLDatabaseConfigurationSupported = this.ModelValidator.LoggerSpecifics.Configuration.IsARXMLDatabaseConfigurationSupported;
					if (flag || flag2 || (flag3 && isARXMLDatabaseConfigurationSupported) || (flag4 && (flag5 || isARXMLDatabaseConfigurationSupported)))
					{
						acceptedFiles.Add(text);
						result = true;
						goto IL_115;
					}
					goto IL_115;
				}
				return result;
			}
			return false;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DatabaseGrid));
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
			this.gridViewDatabases.LeftCoordChanged += new EventHandler(this.gridViewDatabases_LeftCoordChanged);
			this.gridViewDatabases.TopRowChanged += new EventHandler(this.gridViewDatabases_TopRowChanged);
			this.gridViewDatabases.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewDatabases_PopupMenuShowing);
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
			base.Name = "DatabaseGrid";
			componentResourceManager.ApplyResources(this, "$this");
			base.DragDrop += new DragEventHandler(this.DatabaseGrid_DragDrop);
			base.DragEnter += new DragEventHandler(this.DatabaseGrid_DragEnter);
			base.Resize += new EventHandler(this.DatabaseGrid_Resize);
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
