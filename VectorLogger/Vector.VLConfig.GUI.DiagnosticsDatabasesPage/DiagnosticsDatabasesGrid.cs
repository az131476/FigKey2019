using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Utils.Drawing;
using DevExpress.Utils.Menu;
using DevExpress.XtraEditors;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.DiagnosticsDatabasesPage
{
	internal class DiagnosticsDatabasesGrid : UserControl
	{
		private delegate void GridRowContextMenuHandler(object sender, EventArgs e);

		private DiagnosticsDatabaseConfiguration databaseConfiguration;

		private IDiagSymbolsManager diagSymbolsManager;

		private GUIElementManager_ControlGridTree guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isGroupRowSelected;

		private Dictionary<DiagnosticsDatabase, bool> expandStates;

		private CommParameters commParamsDialog;

		private SelectECUs selectEcusDialog;

		private DisplayMode displayMode;

		private static readonly int iconSize = 16;

		private IContainer components;

		private GridControl gridControlDiagnosticsDatabases;

		private GridView gridViewDiagnosticsDatabases;

		private GridColumn colFileName;

		private GridColumn colChannel;

		private RepositoryItemComboBox repositoryItemComboBox;

		private GridColumn colECU;

		private GridColumn colDiagnosticsProtocol;

		private BindingSource diagDatabaseBindingSource;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private GridColumn colVariant;

		private XtraToolTipController toolTipController;

		private GridColumn colCommInterface;

		private RepositoryItemButtonEdit repositoryItemButtonEditCommInterface;

		public event EventHandler SelectionChanged;

		public DiagnosticsDatabaseConfiguration DatabaseConfiguration
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
					int focusedRowHandle = this.gridViewDiagnosticsDatabases.FocusedRowHandle;
					Cursor.Current = Cursors.WaitCursor;
					this.gridControlDiagnosticsDatabases.DataSource = this.databaseConfiguration.ECUs;
					Cursor.Current = Cursors.WaitCursor;
					this.gridViewDiagnosticsDatabases.RefreshData();
					this.SetExpandStates();
					if (focusedRowHandle >= 0 && focusedRowHandle < this.gridViewDiagnosticsDatabases.RowCount)
					{
						this.gridViewDiagnosticsDatabases.FocusedRowHandle = focusedRowHandle;
					}
					this.ValidateInput(false);
				}
			}
		}

		public DiagnosticActionsConfiguration DiagnosticActionsConfiguration
		{
			get;
			set;
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
					this.gridViewDiagnosticsDatabases.RefreshData();
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
			get
			{
				return this.diagSymbolsManager;
			}
			set
			{
				this.diagSymbolsManager = value;
			}
		}

		public SelectECUs SelectECUsDialog
		{
			get
			{
				if (this.selectEcusDialog == null)
				{
					this.selectEcusDialog = new SelectECUs();
				}
				this.selectEcusDialog.DiagnosticsDatabaseConfiguration = this.databaseConfiguration;
				this.selectEcusDialog.ModelValidator = this.ModelValidator;
				return this.selectEcusDialog;
			}
		}

		public ILoggerSpecifics CurrentLogger
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

		public DiagnosticsDatabasesGrid()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_ControlGridTree();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.FormatError, this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.expandStates = new Dictionary<DiagnosticsDatabase, bool>();
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

		public void AddDatabase(string filepath)
		{
			this.AddDatabase(filepath, true);
		}

		public void AddDatabase(string filepath, bool askForEditCommParams)
		{
			foreach (DiagnosticsDatabase current in this.databaseConfiguration.Databases)
			{
				if (string.Compare(filepath, this.ModelValidator.GetAbsoluteFilePath(current.FilePath.Value), true) == 0)
				{
					InformMessageBox.Error(Resources.ErrorDSMDuplicateDb);
					return;
				}
			}
			Cursor.Current = Cursors.WaitCursor;
			IList<string> list;
			DSMResult dSMResult = this.DiagSymbolsManager.GetAllEcuQualifiersOfDatabaseFile(filepath, out list);
			Cursor.Current = Cursors.Default;
			if (dSMResult == DSMResult.OK)
			{
				IList<string> list2;
				if (list.Count == 1)
				{
					foreach (DiagnosticsECU current2 in this.databaseConfiguration.ECUs)
					{
						if (current2.Qualifier.Value == list[0])
						{
							InformMessageBox.Error(string.Format(Resources.ErrorUnableToLoadDiagDescDuplicateEcu, list[0]));
							return;
						}
					}
					list2 = list;
				}
				else
				{
					this.SelectECUsDialog.EcuQualifier2IsActive.Clear();
					foreach (string current3 in list)
					{
						this.SelectECUsDialog.EcuQualifier2IsActive.Add(current3, false);
					}
					this.SelectECUsDialog.AbsDatabaseFilePath = filepath;
					this.SelectECUsDialog.UndefinedEcuQualifiers = new List<string>();
					if (this.SelectECUsDialog.ShowDialog() == DialogResult.Cancel)
					{
						return;
					}
					list2 = this.SelectECUsDialog.GetSelectedEcus();
				}
				Cursor.Current = Cursors.WaitCursor;
				DiagnosticsDatabase diagnosticsDatabase;
				dSMResult = this.DiagSymbolsManager.LoadDiagnosticsDatabase(this.DiagnosticActionsConfiguration, filepath, this.ModelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_CAN), list2, out diagnosticsDatabase);
				Cursor.Current = Cursors.Default;
				if (dSMResult == DSMResult.OK)
				{
					diagnosticsDatabase.FilePath.Value = this.ModelValidator.GetFilePathRelativeToConfiguration(filepath);
					this.DetermineAndProcessEcusWithInvalidCommInterfaces(list2, ref diagnosticsDatabase);
					Cursor.Current = Cursors.WaitCursor;
					if (diagnosticsDatabase.ECUs.Count <= 0)
					{
						this.DiagSymbolsManager.RemoveDiagnosticsDatabase(filepath);
						return;
					}
					this.AddDatabase(diagnosticsDatabase, askForEditCommParams);
				}
			}
			this.DisplayDiagSymbolsManagerError(dSMResult);
		}

		private void AddDatabase(DiagnosticsDatabase db, bool askForEditCommParams)
		{
			this.SaveExpandStates();
			DiagnosticsECU diagnosticsECU = null;
			Cursor.Current = Cursors.WaitCursor;
			foreach (DiagnosticsECU current in db.ECUs)
			{
				if (!this.ModelEditor.InitDiagCommParamsEcu(current) && diagnosticsECU == null)
				{
					diagnosticsECU = current;
				}
			}
			Cursor.Current = Cursors.WaitCursor;
			if (this.SemanticChecker.IsDiagDescriptionContainingOemSpecificEcus(db, Constants.ManufacturerName_VW, DiagnosticsProtocolType.KWP))
			{
				string message = string.Format(Resources.DiagDbVWActivateStopComm, Path.GetFileName(db.FilePath.Value));
				if (InformMessageBox.Question(message) == DialogResult.Yes)
				{
					foreach (DiagnosticsECU current2 in db.ECUs)
					{
						if (current2.DiagnosticCommParamsECU.DiagProtocol.Value == DiagnosticsProtocolType.KWP)
						{
							current2.DiagnosticCommParamsECU.UseStopCommRequest.Value = true;
						}
					}
				}
			}
			Cursor.Current = Cursors.WaitCursor;
			this.databaseConfiguration.AddDatabase(db);
			this.ValidateInput(true);
			this.SelectRowOfDatabase(db);
			if (diagnosticsECU != null && askForEditCommParams && DialogResult.Yes == InformMessageBox.Question(Resources.SomeCommInterfacesMissing))
			{
				this.EditCommParameters(diagnosticsECU);
			}
		}

		public void RemoveDatabase(DiagnosticsDatabase db)
		{
			this.SaveExpandStates();
			this.databaseConfiguration.RemoveDatabase(db);
			this.ValidateInput(true);
		}

		public void SelectEcus()
		{
			DiagnosticsDatabase database;
			if (this.TryGetSelectedDatabase(out database))
			{
				List<string> undefinedEcuQualifiersOfDiagDatabase = this.ModelValidator.GetUndefinedEcuQualifiersOfDiagDatabase(database);
				this.SelectEcus(ref database, undefinedEcuQualifiersOfDiagDatabase);
			}
		}

		public void SelectEcus(ref DiagnosticsDatabase db, List<string> undefinedEcus)
		{
			Cursor.Current = Cursors.WaitCursor;
			IList<string> list;
			DSMResult allEcuQualifiersOfDatabaseFile = this.DiagSymbolsManager.GetAllEcuQualifiersOfDatabaseFile(this.ModelValidator.GetAbsoluteFilePath(db.FilePath.Value), out list);
			Cursor.Current = Cursors.Default;
			if (allEcuQualifiersOfDatabaseFile != DSMResult.OK)
			{
				this.DisplayDiagSymbolsManagerError(allEcuQualifiersOfDatabaseFile);
				return;
			}
			this.SelectECUsDialog.AbsDatabaseFilePath = this.ModelValidator.GetAbsoluteFilePath(db.FilePath.Value);
			this.SelectECUsDialog.EcuQualifier2IsActive.Clear();
			foreach (string current in list)
			{
				bool value = false;
				DiagnosticsECU diagnosticsECU;
				if (db.TryGetECU(current, out diagnosticsECU))
				{
					value = true;
				}
				this.selectEcusDialog.EcuQualifier2IsActive.Add(current, value);
			}
			foreach (string current2 in undefinedEcus)
			{
				if (!this.selectEcusDialog.EcuQualifier2IsActive.ContainsKey(current2))
				{
					this.selectEcusDialog.EcuQualifier2IsActive.Add(current2, true);
				}
			}
			this.selectEcusDialog.UndefinedEcuQualifiers = undefinedEcus;
			DiagnosticsECU diagnosticsECU2 = null;
			if (this.selectEcusDialog.ShowDialog() == DialogResult.OK)
			{
				this.SaveExpandStates();
				List<string> list2 = new List<string>();
				foreach (DiagnosticsECU current3 in db.ECUs)
				{
					list2.Add(current3.Qualifier.Value);
				}
				List<string> list3 = new List<string>();
				IList<string> selectedEcus = this.selectEcusDialog.GetSelectedEcus();
				foreach (string current4 in selectedEcus)
				{
					if (!list2.Contains(current4))
					{
						list3.Add(current4);
					}
				}
				this.DiagSymbolsManager.UpdateDatebaseEcuList(this.ModelValidator.GetAbsoluteFilePath(db.FilePath.Value), this.ModelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_CAN), selectedEcus, ref db);
				this.DetermineAndProcessEcusWithInvalidCommInterfaces(list3, ref db);
				if (db.ECUs.Count > 0)
				{
					foreach (string current5 in list3)
					{
						DiagnosticsECU diagnosticsECU3;
						if (db.TryGetECU(current5, out diagnosticsECU3) && !this.ModelEditor.InitDiagCommParamsEcu(diagnosticsECU3) && diagnosticsECU2 == null)
						{
							diagnosticsECU2 = diagnosticsECU3;
						}
					}
					this.ValidateInput(true);
				}
				else
				{
					this.databaseConfiguration.RemoveDatabase(db);
					this.ValidateInput(true);
					InformMessageBox.Info(string.Format(Resources.NoMoreEcusConfiguredFromDb, Path.GetFileName(db.FilePath.Value)));
				}
			}
			if (diagnosticsECU2 != null && DialogResult.Yes == InformMessageBox.Question(Resources.SomeCommInterfacesMissing))
			{
				this.EditCommParameters(diagnosticsECU2);
			}
		}

		private void DetermineAndProcessEcusWithInvalidCommInterfaces(IList<string> ecusToAddToConfig, ref DiagnosticsDatabase db)
		{
			List<string> list = new List<string>();
			Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
			Cursor.Current = Cursors.WaitCursor;
			foreach (string current in ecusToAddToConfig)
			{
				IList<string> list2;
				IList<string> list3;
				if (this.diagSymbolsManager.GetAllCommInterfaceQualifiers(current, out list2, out list3) && list3.Count > 0)
				{
					if (list2.Count == 0)
					{
						list.Add(current);
					}
					else
					{
						dictionary.Add(current, new List<string>(list3));
					}
				}
			}
			Cursor.Current = Cursors.Default;
			if (list.Count > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(Resources.EcusHaveNoValidCommInterface1);
				foreach (string current2 in list)
				{
					stringBuilder.AppendLine(current2);
				}
				stringBuilder.AppendLine(Resources.EcusHaveNoValidCommInterface2);
				if (InformMessageBox.Question(stringBuilder.ToString()) == DialogResult.No)
				{
					List<string> list4 = new List<string>();
					foreach (DiagnosticsECU current3 in db.ECUs)
					{
						if (!list.Contains(current3.Qualifier.Value))
						{
							list4.Add(current3.Qualifier.Value);
						}
					}
					this.DiagSymbolsManager.UpdateDatebaseEcuList(this.ModelValidator.GetAbsoluteFilePath(db.FilePath.Value), this.ModelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_CAN), list4, ref db);
				}
			}
			if (dictionary.Count > 0)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendLine(Resources.EcusHaveInvalidCommInterface1);
				stringBuilder2.AppendLine();
				foreach (string current4 in dictionary.Keys)
				{
					stringBuilder2.AppendFormat("{0}: ", current4);
					foreach (string current5 in dictionary[current4])
					{
						stringBuilder2.AppendFormat("{0}, ", current5);
					}
					stringBuilder2.Length -= 2;
					stringBuilder2.AppendLine();
				}
				stringBuilder2.AppendLine();
				stringBuilder2.AppendLine(Resources.EcusHaveInvalidCommInterface2);
				InformMessageBox.Warning(stringBuilder2.ToString());
			}
		}

		public void EditCommParameters()
		{
			CommParameters commParametersDialog = this.GetCommParametersDialog();
			DiagnosticsECU preselectedEcu;
			if (this.isGroupRowSelected)
			{
				commParametersDialog.PreselectedEcu = null;
			}
			else if (this.TryGetSelectedECU(out preselectedEcu))
			{
				commParametersDialog.PreselectedEcu = preselectedEcu;
			}
			if (DialogResult.OK == commParametersDialog.ShowDialog())
			{
				this.ValidateInput(true);
			}
		}

		private void EditCommParameters(DiagnosticsECU preselEcu)
		{
			CommParameters commParametersDialog = this.GetCommParametersDialog();
			commParametersDialog.PreselectedEcu = preselEcu;
			if (DialogResult.OK == commParametersDialog.ShowDialog())
			{
				this.ValidateInput(true);
			}
		}

		public void DisplayDiagSymbolsManagerError(DSMResult res)
		{
			if (res != DSMResult.OK)
			{
				switch (res)
				{
				case DSMResult.DuplicateEcuQualifier:
					InformMessageBox.Error(Resources.ErrorDSMDuplicateEcu);
					return;
				case DSMResult.NoEcuInDatabase:
					InformMessageBox.Error(Resources.ErrorDSMNoEcu);
					return;
				case DSMResult.UnknownFileType:
					InformMessageBox.Error(Resources.ErrorDSMUnknownFileFormat);
					return;
				case DSMResult.DuplicateDatabase:
					InformMessageBox.Error(Resources.ErrorDSMDuplicateDb);
					return;
				case DSMResult.FailedToLoadCddDescFile:
					InformMessageBox.Error(Resources.ErrorDSMFailedToLoadUpdatePersist);
					return;
				}
				InformMessageBox.Error(Resources.ErrorDSMFailedToLoad);
			}
		}

		private void gridViewDiagnosticsDatabases_DoubleClick(object sender, EventArgs e)
		{
			DiagnosticsECU diagnosticsECU = null;
			if (this.gridViewDiagnosticsDatabases.FocusedColumn == this.colFileName && this.TryGetSelectedECU(out diagnosticsECU))
			{
				FileSystemServices.LaunchFile(this.ModelValidator.GetAbsoluteFilePath(diagnosticsECU.Database.FilePath.Value));
			}
		}

		private void gridViewDiagnosticsDatabases_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(e.Column, this.gridViewDiagnosticsDatabases.GetDataSourceRowIndex(e.RowHandle));
			this.customErrorProvider.Grid.DisplayError(gUIElement, e);
		}

		private void gridViewDiagnosticsDatabases_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
		{
			int childRowHandle = this.gridViewDiagnosticsDatabases.GetChildRowHandle(e.RowHandle, 0);
			int dataSourceRowIndex = this.gridViewDiagnosticsDatabases.GetDataSourceRowIndex(childRowHandle);
			if (dataSourceRowIndex < 0 || dataSourceRowIndex >= this.databaseConfiguration.ECUs.Count)
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
			Rectangle arg_71_0 = gridGroupRowInfo.DataBounds;
			Rectangle buttonBounds = gridGroupRowInfo.ButtonBounds;
			Rectangle bounds = e.Bounds;
			bounds.X = buttonBounds.Right + 4;
			new ObjectInfoArgs(e.Cache);
			gridGroupRowPainter.DrawGroupRowBackground(e.Info);
			OpenCloseButtonInfoArgs e2 = new OpenCloseButtonInfoArgs(e.Cache, gridGroupRowInfo.ButtonBounds, gridGroupRowInfo.GroupExpanded, gridGroupRowInfo.AppearanceGroupButton, ObjectState.Normal);
			if (!gridGroupRowInfo.ButtonBounds.IsEmpty)
			{
				gridGroupRowPainter.ElementsPainter.OpenCloseButton.DrawObject(e2);
			}
			Point location = bounds.Location;
			int num = (bounds.Height - DiagnosticsDatabasesGrid.iconSize) / 2;
			DiagnosticsECU diagnosticsECU = this.databaseConfiguration.ECUs[dataSourceRowIndex];
			if (!this.ModelValidator.ValidateDatabaseFileExistence(diagnosticsECU.Database, this.pageValidator))
			{
				Rectangle rect = new Rectangle(location.X, location.Y + num, DiagnosticsDatabasesGrid.iconSize, DiagnosticsDatabasesGrid.iconSize);
				e.Graphics.DrawImageUnscaled(Resources.IconGlobalModelError.ToBitmap(), rect);
				location.X += DiagnosticsDatabasesGrid.iconSize + 2;
			}
			Rectangle rect2 = new Rectangle(location.X, location.Y + num, DiagnosticsDatabasesGrid.iconSize, DiagnosticsDatabasesGrid.iconSize);
			if (diagnosticsECU.Database.Type.Value == DiagDbType.ODX)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageDiagDbODX, rect2);
			}
			else if (diagnosticsECU.Database.Type.Value == DiagDbType.PDX)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageDiagDbPDX, rect2);
			}
			else if (diagnosticsECU.Database.Type.Value == DiagDbType.MDX)
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageDiagDbMDX, rect2);
			}
			else
			{
				e.Graphics.DrawImageUnscaled(Resources.ImageDiagDbCDD, rect2);
			}
			location.X += DiagnosticsDatabasesGrid.iconSize + 2;
			bounds.Location = location;
			GridView gridView = sender as GridView;
			string text = (gridView != null) ? gridView.GetGroupRowDisplayText(e.RowHandle) : string.Empty;
			e.Appearance.DrawString(e.Cache, text, bounds);
			e.Handled = true;
		}

		private void gridViewDiagnosticsDatabases_CustomColumnSort(object sender, CustomColumnSortEventArgs e)
		{
			if (e.Column == this.colFileName)
			{
				DiagnosticsECU diagnosticsECU = this.databaseConfiguration.ECUs[e.ListSourceRowIndex1];
				DiagnosticsECU diagnosticsECU2 = this.databaseConfiguration.ECUs[e.ListSourceRowIndex2];
				int num = this.databaseConfiguration.Databases.IndexOf(diagnosticsECU.Database);
				int num2 = this.databaseConfiguration.Databases.IndexOf(diagnosticsECU2.Database);
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

		private void gridViewDiagnosticsDatabases_CustomColumnGroup(object sender, CustomColumnSortEventArgs e)
		{
			if (e.Column == this.colFileName)
			{
				DiagnosticsECU diagnosticsECU = this.databaseConfiguration.ECUs[e.ListSourceRowIndex1];
				DiagnosticsECU diagnosticsECU2 = this.databaseConfiguration.ECUs[e.ListSourceRowIndex2];
				if (diagnosticsECU.Database == diagnosticsECU2.Database)
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

		private void gridViewDiagnosticsDatabases_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			DiagnosticsECU ecu;
			if (!this.GetECU(e.ListSourceRowIndex, out ecu))
			{
				return;
			}
			if (e.Column == this.colFileName)
			{
				this.UnboundColumnFileName(ecu, e);
				return;
			}
			if (e.Column == this.colChannel)
			{
				this.UnboundColumnChannel(ecu, e);
				return;
			}
			if (e.Column == this.colECU)
			{
				this.UnboundColumnECU(ecu, e);
				return;
			}
			if (e.Column == this.colVariant)
			{
				this.UnboundColumnVariant(ecu, e);
				return;
			}
			if (e.Column == this.colDiagnosticsProtocol)
			{
				this.UnboundColumnDiagnosticsProtocol(ecu, e);
				return;
			}
			if (e.Column == this.colCommInterface)
			{
				this.UnboundColumnCommInterface(ecu, e);
			}
		}

		private void gridViewDiagnosticsDatabases_ShownEditor(object sender, EventArgs e)
		{
			if (this.gridViewDiagnosticsDatabases.FocusedColumn == this.colChannel)
			{
				this.ShownEditorChannel();
				return;
			}
			if (this.gridViewDiagnosticsDatabases.FocusedColumn == this.colVariant)
			{
				this.ShownEditorVariant();
			}
		}

		private void gridViewDiagnosticsDatabases_KeyDown(object sender, KeyEventArgs e)
		{
		}

		private void gridViewDiagnosticsDatabases_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
			this.isGroupRowSelected = (e.FocusedRowHandle < 0 && this.gridViewDiagnosticsDatabases.RowCount > 0);
			this.Raise_SelectionChanged(this, EventArgs.Empty);
		}

		private void gridViewDiagnosticsDatabases_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
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

		private void DisplayGridRowContextMenu(PopupMenuShowingEventArgs e)
		{
			int arg_0B_0 = e.HitInfo.RowHandle;
			e.Menu.Items.Clear();
			if (this.isGroupRowSelected)
			{
				e.Menu.Items.Add(this.CreateMenuItem(Resources.OpenDescription, new DiagnosticsDatabasesGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuOpenDatabase), false));
				e.Menu.Items.Add(this.CreateMenuItem(Resources.OpenContainingFolder, new DiagnosticsDatabasesGrid.GridRowContextMenuHandler(this.OnGridRowContextMenuOpenFolder), false));
				bool flag = false;
				DiagnosticsDatabase diagnosticsDatabase;
				if (this.TryGetSelectedDatabase(out diagnosticsDatabase) && diagnosticsDatabase.TotalNumberOfEcusInFile == 1u)
				{
					e.Menu.Items.Add(this.CreateMenuItem(Resources.ECUSettings, new DiagnosticsDatabasesGrid.GridRowContextMenuHandler(this.OnGridRowContextEditCommParams), true));
					flag = true;
				}
				if (!flag)
				{
					e.Menu.Items.Add(this.CreateMenuItem(Resources.SelectEcus, new DiagnosticsDatabasesGrid.GridRowContextMenuHandler(this.OnGridRowContextSelectEcus), true));
					e.Menu.Items.Add(this.CreateMenuItem(Resources.ECUSettings, new DiagnosticsDatabasesGrid.GridRowContextMenuHandler(this.OnGridRowContextEditCommParams), false));
					return;
				}
			}
			else
			{
				e.Menu.Items.Add(this.CreateMenuItem(Resources.ECUSettings, new DiagnosticsDatabasesGrid.GridRowContextMenuHandler(this.OnGridRowContextEditCommParams), false));
			}
		}

		private DXMenuItem CreateMenuItem(string caption, DiagnosticsDatabasesGrid.GridRowContextMenuHandler target, bool isBeginOfGroup)
		{
			return new DXMenuItem(caption, new EventHandler(target.Invoke))
			{
				BeginGroup = isBeginOfGroup
			};
		}

		private void OnGridRowContextMenuOpenDatabase(object sender, EventArgs e)
		{
			DiagnosticsDatabase diagnosticsDatabase;
			if (this.TryGetSelectedDatabase(out diagnosticsDatabase))
			{
				FileSystemServices.LaunchFile(this.ModelValidator.GetAbsoluteFilePath(diagnosticsDatabase.FilePath.Value));
			}
		}

		private void OnGridRowContextMenuOpenFolder(object sender, EventArgs e)
		{
			DiagnosticsDatabase diagnosticsDatabase;
			if (this.TryGetSelectedDatabase(out diagnosticsDatabase))
			{
				string directoryName = Path.GetDirectoryName(this.ModelValidator.GetAbsoluteFilePath(diagnosticsDatabase.FilePath.Value));
				FileSystemServices.LaunchDirectoryBrowser(directoryName);
			}
		}

		private void OnGridRowContextSelectEcus(object sender, EventArgs e)
		{
			this.SelectEcus();
		}

		private void OnGridRowContextEditCommParams(object sender, EventArgs e)
		{
			this.EditCommParameters();
		}

		private void gridViewDiagnosticsDatabases_TopRowChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridViewDiagnosticsDatabases_LeftCoordChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void DiagnosticsDatabasesGrid_Resize(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void UnboundColumnFileName(DiagnosticsECU ecu, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				string str = "";
				try
				{
					FileInfo fileInfo = new FileInfo(this.ModelValidator.GetAbsoluteFilePath(ecu.Database.FilePath.Value));
					str = string.Format(Resources.DiagDbGridRowInfoTxt, GUIUtil.MapFileSizeNumber2String(fileInfo.Length), ecu.Database.ECUs.Count, ecu.Database.TotalNumberOfEcusInFile);
				}
				catch (Exception)
				{
					str = " (?)";
				}
				e.Value = ecu.Database.FilePath.Value + str;
			}
		}

		private void UnboundColumnChannel(DiagnosticsECU ecu, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = GUIUtil.MapCANChannelNumber2String(ecu.ChannelNumber.Value);
				this.pageValidator.Grid.StoreMapping(ecu.ChannelNumber, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
				return;
			}
			this.SaveExpandStates();
			uint value = GUIUtil.MapCANChannelString2Number(e.Value.ToString());
			bool flag;
			this.pageValidator.Grid.UpdateModel<uint>(value, ecu.ChannelNumber, out flag);
			if (flag)
			{
				this.ValidateInput(true);
			}
		}

		private void UnboundColumnLocation(DiagnosticsECU ecu, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = FileSystemServices.GetFolderPathFromFilePath(ecu.Database.FilePath.Value);
				this.pageValidator.Grid.StoreMapping(ecu.Database.FilePath, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
			}
		}

		private void UnboundColumnECU(DiagnosticsECU ecu, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = ecu.Qualifier.Value;
			}
		}

		private void UnboundColumnVariant(DiagnosticsECU ecu, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = ecu.Variant.Value;
				return;
			}
			this.SaveExpandStates();
			bool flag;
			this.pageValidator.Grid.UpdateModel<string>((string)e.Value, ecu.Variant, out flag);
			if (flag)
			{
				this.ValidateInput(true);
			}
		}

		private void UnboundColumnDiagnosticsProtocol(DiagnosticsECU ecu, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = GUIUtil.MapDiagnosticsProtocol2String(ecu.DiagnosticCommParamsECU.DiagProtocol.Value);
			}
		}

		private void UnboundColumnCommInterface(DiagnosticsECU ecu, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				string arg = Resources.UserDefined;
				string text;
				if (ecu.DiagnosticCommParamsECU.UseParamValuesFromDb.Value && this.DiagSymbolsManager.GetCommInterfaceName(ecu.Qualifier.Value, ecu.DiagnosticCommParamsECU.InterfaceQualifier.Value, out text))
				{
					arg = text;
				}
				e.Value = string.Format(Resources.CommInterfaceGridCellInfoTxt, GUIUtil.CANIdToDisplayString(ecu.DiagnosticCommParamsECU.PhysRequestMsgId.Value, ecu.DiagnosticCommParamsECU.PhysRequestMsgIsExtendedId.Value), GUIUtil.CANIdToDisplayString(ecu.DiagnosticCommParamsECU.ResponseMsgId.Value, ecu.DiagnosticCommParamsECU.ResponseMsgIsExtendedId.Value), arg);
			}
		}

		private void ShownEditorChannel()
		{
			ComboBoxEdit comboBoxEdit = this.gridViewDiagnosticsDatabases.ActiveEditor as ComboBoxEdit;
			if (comboBoxEdit != null)
			{
				this.FillCANChannelCombobox(comboBoxEdit);
			}
		}

		private void FillCANChannelCombobox(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.ModelValidator.GetTotalNumberOfLogicalChannels(BusType.Bt_CAN); num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
		}

		private void ShownEditorVariant()
		{
			ComboBoxEdit comboBoxEdit = this.gridViewDiagnosticsDatabases.ActiveEditor as ComboBoxEdit;
			if (comboBoxEdit != null)
			{
				ReadOnlyCollection<DiagnosticsECU> readOnlyCollection = this.gridControlDiagnosticsDatabases.DataSource as ReadOnlyCollection<DiagnosticsECU>;
				if (readOnlyCollection != null)
				{
					DiagnosticsECU ecu = readOnlyCollection[this.gridViewDiagnosticsDatabases.GetFocusedDataSourceRowIndex()];
					this.FillVariantsCombobox(comboBoxEdit, ecu);
				}
			}
		}

		private void FillVariantsCombobox(ComboBoxEdit comboBoxEdit, DiagnosticsECU ecu)
		{
			IList<string> list;
			if (this.diagSymbolsManager.GetVariantQualifiers(this.ModelValidator.GetAbsoluteFilePath(ecu.Database.FilePath.Value), ecu.Qualifier.Value, out list))
			{
				foreach (string current in list)
				{
					comboBoxEdit.Properties.Items.Add(current);
				}
			}
		}

		private void repositoryItemButtonEditCommInterface_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			this.EditCommParameters();
		}

		private void repositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewDiagnosticsDatabases.PostEditor();
		}

		public void DisplayErrors()
		{
			GridViewInfo gridViewInfo = this.gridViewDiagnosticsDatabases.GetViewInfo() as GridViewInfo;
			using (Graphics graphics = Graphics.FromHwnd(base.Handle))
			{
				if (gridViewInfo != null)
				{
					gridViewInfo.Calc(graphics, this.gridViewDiagnosticsDatabases.ViewRect);
				}
			}
			this.StoreMapping4VisibleCells();
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
		}

		public bool ValidateInput(bool isDataChanged = false)
		{
			this.ResetValidationFramework();
			this.StoreMapping4VisibleCells();
			bool flag = true;
			flag &= this.ModelValidator.Validate(this.DatabaseConfiguration, isDataChanged, this.pageValidator);
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
			PageValidatorGridUtil.StoreRowMappingForVisibleRows(this.gridViewDiagnosticsDatabases, new PageValidatorGridUtil.StoreRowMappingHandler(this.OnPageValidatorGridUtilStoreRowMapping));
		}

		private void OnPageValidatorGridUtilStoreRowMapping(int dataSourceRowIdx)
		{
			if (dataSourceRowIdx < 0 || dataSourceRowIdx >= this.databaseConfiguration.ECUs.Count)
			{
				return;
			}
			DiagnosticsECU ecu = this.DatabaseConfiguration.ECUs[dataSourceRowIdx];
			this.StoreMapping4VisibleColumns(ecu, dataSourceRowIdx);
		}

		private void StoreMapping4VisibleColumns(DiagnosticsECU ecu, int dataSourceIdx)
		{
			if (PageValidatorGridUtil.IsColumnVisible(this.colFileName, this.gridViewDiagnosticsDatabases))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colFileName, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(ecu.Database.FilePath, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colChannel, this.gridViewDiagnosticsDatabases))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colChannel, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(ecu.ChannelNumber, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colECU, this.gridViewDiagnosticsDatabases))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colECU, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(ecu.Qualifier, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colVariant, this.gridViewDiagnosticsDatabases))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colVariant, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(ecu.Variant, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colCommInterface, this.gridViewDiagnosticsDatabases))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colCommInterface, dataSourceIdx);
				IEnumerable<IValidatedProperty> validatedPropertyListCommInterface = this.GetValidatedPropertyListCommInterface(ecu.DiagnosticCommParamsECU);
				foreach (IValidatedProperty current in validatedPropertyListCommInterface)
				{
					this.pageValidator.Grid.StoreMapping(current, gUIElement);
				}
			}
		}

		private IEnumerable<IValidatedProperty> GetValidatedPropertyListCommInterface(DiagnosticCommParamsECU commParams)
		{
			return new List<IValidatedProperty>
			{
				commParams.PhysRequestMsgId,
				commParams.PhysRequestMsgIsExtendedId,
				commParams.ResponseMsgId,
				commParams.ResponseMsgIsExtendedId,
				commParams.DefaultSessionId,
				commParams.ExtendedSessionId
			};
		}

		public bool TryGetSelectedDatabase(out DiagnosticsDatabase database)
		{
			int num;
			return this.TryGetSelectedDatabase(out database, out num);
		}

		private bool TryGetSelectedDatabase(out DiagnosticsDatabase database, out int idx)
		{
			database = null;
			idx = this.gridViewDiagnosticsDatabases.GetFocusedDataSourceRowIndex();
			if (idx < 0)
			{
				idx = this.gridViewDiagnosticsDatabases.GetDataRowHandleByGroupRowHandle(this.gridViewDiagnosticsDatabases.FocusedRowHandle);
			}
			if (idx < 0 || idx > this.databaseConfiguration.ECUs.Count - 1)
			{
				return false;
			}
			DiagnosticsECU diagnosticsECU = this.databaseConfiguration.ECUs[idx];
			database = diagnosticsECU.Database;
			return null != database;
		}

		public void SelectRowOfDatabase(DiagnosticsDatabase db)
		{
			for (int i = 0; i < this.gridViewDiagnosticsDatabases.RowCount; i++)
			{
				IList<DiagnosticsECU> list = this.gridViewDiagnosticsDatabases.DataSource as IList<DiagnosticsECU>;
				if (list != null)
				{
					DiagnosticsECU diagnosticsECU = list[this.gridViewDiagnosticsDatabases.GetDataSourceRowIndex(i)];
					if (diagnosticsECU.Database == db)
					{
						this.gridViewDiagnosticsDatabases.FocusedRowHandle = i;
						return;
					}
				}
			}
		}

		public bool TryGetSelectedECU(out DiagnosticsECU ecu)
		{
			int num;
			return this.TryGetSelectedECU(out ecu, out num);
		}

		private bool TryGetSelectedECU(out DiagnosticsECU ecu, out int idx)
		{
			ecu = null;
			idx = this.gridViewDiagnosticsDatabases.GetFocusedDataSourceRowIndex();
			if (idx < 0 || idx > this.databaseConfiguration.ECUs.Count - 1)
			{
				return false;
			}
			ecu = this.databaseConfiguration.ECUs[idx];
			return null != ecu;
		}

		public void SelectRowOfECU(DiagnosticsECU ecu)
		{
			for (int i = 0; i < this.gridViewDiagnosticsDatabases.RowCount; i++)
			{
				IList<DiagnosticsECU> list = this.gridViewDiagnosticsDatabases.DataSource as IList<DiagnosticsECU>;
				if (list != null)
				{
					DiagnosticsECU diagnosticsECU = list[this.gridViewDiagnosticsDatabases.GetDataSourceRowIndex(i)];
					if (diagnosticsECU == ecu)
					{
						this.gridViewDiagnosticsDatabases.FocusedRowHandle = i;
						return;
					}
				}
			}
		}

		private void SaveExpandStates()
		{
			this.expandStates.Clear();
			for (int i = -1; i >= -2147483648; i--)
			{
				if (!this.gridViewDiagnosticsDatabases.IsValidRowHandle(i))
				{
					return;
				}
				int childRowHandle = this.gridViewDiagnosticsDatabases.GetChildRowHandle(i, 0);
				if (this.gridViewDiagnosticsDatabases.IsValidRowHandle(childRowHandle))
				{
					int dataSourceRowIndex = this.gridViewDiagnosticsDatabases.GetDataSourceRowIndex(childRowHandle);
					DiagnosticsECU diagnosticsECU = this.databaseConfiguration.ECUs[dataSourceRowIndex];
					this.expandStates.Add(diagnosticsECU.Database, this.gridViewDiagnosticsDatabases.GetRowExpanded(i));
				}
			}
		}

		private void SetExpandStates()
		{
			for (int i = -1; i >= -2147483648; i--)
			{
				if (!this.gridViewDiagnosticsDatabases.IsValidRowHandle(i))
				{
					return;
				}
				int childRowHandle = this.gridViewDiagnosticsDatabases.GetChildRowHandle(i, 0);
				if (this.gridViewDiagnosticsDatabases.IsValidRowHandle(childRowHandle))
				{
					int dataSourceRowIndex = this.gridViewDiagnosticsDatabases.GetDataSourceRowIndex(childRowHandle);
					DiagnosticsECU diagnosticsECU = this.databaseConfiguration.ECUs[dataSourceRowIndex];
					if (this.expandStates.ContainsKey(diagnosticsECU.Database))
					{
						this.gridViewDiagnosticsDatabases.SetRowExpanded(i, this.expandStates[diagnosticsECU.Database]);
					}
					else
					{
						this.gridViewDiagnosticsDatabases.SetRowExpanded(i, true);
					}
				}
			}
		}

		private bool GetECU(int listSourceRowIndex, out DiagnosticsECU ecu)
		{
			ecu = null;
			ReadOnlyCollection<DiagnosticsECU> readOnlyCollection = this.gridControlDiagnosticsDatabases.DataSource as ReadOnlyCollection<DiagnosticsECU>;
			if (readOnlyCollection == null)
			{
				return false;
			}
			if (listSourceRowIndex < 0 || listSourceRowIndex > readOnlyCollection.Count - 1)
			{
				return false;
			}
			ecu = readOnlyCollection[listSourceRowIndex];
			return null != ecu;
		}

		public CommParameters GetCommParametersDialog()
		{
			if (this.commParamsDialog == null)
			{
				this.commParamsDialog = new CommParameters(this.ModelValidator);
				this.commParamsDialog.DiagSymbolsManager = this.DiagSymbolsManager;
			}
			this.commParamsDialog.DiagnosticsDatabaseConfiguration = this.DatabaseConfiguration;
			return this.commParamsDialog;
		}

		private void toolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
		{
			ToolTipControlInfo info = null;
			try
			{
				GridHitInfo gridHitInfo = this.gridViewDiagnosticsDatabases.CalcHitInfo(e.ControlMousePosition);
				if (gridHitInfo.InRow && this.gridViewDiagnosticsDatabases.IsGroupRow(gridHitInfo.RowHandle))
				{
					int childRowHandle = this.gridViewDiagnosticsDatabases.GetChildRowHandle(gridHitInfo.RowHandle, 0);
					int dataSourceRowIndex = this.gridViewDiagnosticsDatabases.GetDataSourceRowIndex(childRowHandle);
					if (dataSourceRowIndex >= 0 && dataSourceRowIndex < this.databaseConfiguration.ECUs.Count)
					{
						DiagnosticsECU diagnosticsECU = this.databaseConfiguration.ECUs[dataSourceRowIndex];
						if (!this.ModelValidator.ValidateDatabaseFileExistence(diagnosticsECU.Database, this.pageValidator) && gridHitInfo.HitPoint.X > DiagnosticsDatabasesGrid.iconSize && gridHitInfo.HitPoint.X < DiagnosticsDatabasesGrid.iconSize * 2)
						{
							info = new ToolTipControlInfo(new CellToolTipInfo(gridHitInfo.RowHandle, gridHitInfo.Column, "cell"), Resources.ErrorFileNotFound, ToolTipIconType.Warning);
							e.Info = info;
						}
					}
				}
			}
			catch
			{
				e.Info = info;
			}
		}

		public bool Serialize(DiagnosticsDatabasesPage databasesPage)
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
				this.gridViewDiagnosticsDatabases.SaveLayoutToStream(memoryStream);
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

		public bool DeSerialize(DiagnosticsDatabasesPage databasesPage)
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
				this.gridViewDiagnosticsDatabases.RestoreLayoutFromStream(memoryStream);
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

		private void DiagnosticsDatabasesGrid_DragEnter(object sender, DragEventArgs e)
		{
			IList<string> list;
			if (this.AcceptFileDrop(e, out list))
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void DiagnosticsDatabasesGrid_DragDrop(object sender, DragEventArgs e)
		{
			IList<string> list;
			if (this.AcceptFileDrop(e, out list))
			{
				bool askForEditCommParams = list.Count == 1;
				using (IEnumerator<string> enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						this.AddDatabase(current, askForEditCommParams);
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
						goto IL_B3;
					}
					goto IL_64;
					IL_B3:
					i++;
					continue;
					IL_64:
					if (string.Compare(strA, Vocabulary.FileExtensionDotCDD, true) == 0 || string.Compare(strA, Vocabulary.FileExtensionDotODX, true) == 0 || string.Compare(strA, Vocabulary.FileExtensionDotPDX, true) == 0 || string.Compare(strA, Vocabulary.FileExtensionDotMDX, true) == 0)
					{
						acceptedFiles.Add(text);
						result = true;
						goto IL_B3;
					}
					goto IL_B3;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DiagnosticsDatabasesGrid));
			this.gridControlDiagnosticsDatabases = new GridControl();
			this.diagDatabaseBindingSource = new BindingSource();
			this.gridViewDiagnosticsDatabases = new GridView();
			this.colFileName = new GridColumn();
			this.colChannel = new GridColumn();
			this.repositoryItemComboBox = new RepositoryItemComboBox();
			this.colECU = new GridColumn();
			this.colVariant = new GridColumn();
			this.colDiagnosticsProtocol = new GridColumn();
			this.colCommInterface = new GridColumn();
			this.repositoryItemButtonEditCommInterface = new RepositoryItemButtonEdit();
			this.toolTipController = new XtraToolTipController();
			this.errorProviderFormat = new ErrorProvider();
			this.errorProviderLocalModel = new ErrorProvider();
			this.errorProviderGlobalModel = new ErrorProvider();
			((ISupportInitialize)this.gridControlDiagnosticsDatabases).BeginInit();
			((ISupportInitialize)this.diagDatabaseBindingSource).BeginInit();
			((ISupportInitialize)this.gridViewDiagnosticsDatabases).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBox).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEditCommInterface).BeginInit();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.gridControlDiagnosticsDatabases.DataSource = this.diagDatabaseBindingSource;
			componentResourceManager.ApplyResources(this.gridControlDiagnosticsDatabases, "gridControlDiagnosticsDatabases");
			this.gridControlDiagnosticsDatabases.MainView = this.gridViewDiagnosticsDatabases;
			this.gridControlDiagnosticsDatabases.Name = "gridControlDiagnosticsDatabases";
			this.gridControlDiagnosticsDatabases.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemComboBox,
				this.repositoryItemButtonEditCommInterface
			});
			this.gridControlDiagnosticsDatabases.ToolTipController = this.toolTipController;
			this.gridControlDiagnosticsDatabases.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewDiagnosticsDatabases
			});
			this.diagDatabaseBindingSource.DataSource = typeof(DiagnosticsECU);
			this.gridViewDiagnosticsDatabases.Columns.AddRange(new GridColumn[]
			{
				this.colFileName,
				this.colChannel,
				this.colECU,
				this.colVariant,
				this.colDiagnosticsProtocol,
				this.colCommInterface
			});
			this.gridViewDiagnosticsDatabases.GridControl = this.gridControlDiagnosticsDatabases;
			this.gridViewDiagnosticsDatabases.GroupCount = 1;
			this.gridViewDiagnosticsDatabases.Name = "gridViewDiagnosticsDatabases";
			this.gridViewDiagnosticsDatabases.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
			this.gridViewDiagnosticsDatabases.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
			this.gridViewDiagnosticsDatabases.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
			this.gridViewDiagnosticsDatabases.OptionsCustomization.AllowFilter = false;
			this.gridViewDiagnosticsDatabases.OptionsCustomization.AllowQuickHideColumns = false;
			this.gridViewDiagnosticsDatabases.OptionsSelection.EnableAppearanceHideSelection = false;
			this.gridViewDiagnosticsDatabases.OptionsView.ShowGroupedColumns = true;
			this.gridViewDiagnosticsDatabases.OptionsView.ShowGroupPanel = false;
			this.gridViewDiagnosticsDatabases.OptionsView.ShowIndicator = false;
			this.gridViewDiagnosticsDatabases.PaintStyleName = "WindowsXP";
			this.gridViewDiagnosticsDatabases.RowHeight = 20;
			this.gridViewDiagnosticsDatabases.SortInfo.AddRange(new GridColumnSortInfo[]
			{
				new GridColumnSortInfo(this.colFileName, ColumnSortOrder.Ascending)
			});
			this.gridViewDiagnosticsDatabases.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewDiagnosticsDatabases_CustomDrawCell);
			this.gridViewDiagnosticsDatabases.CustomDrawGroupRow += new RowObjectCustomDrawEventHandler(this.gridViewDiagnosticsDatabases_CustomDrawGroupRow);
			this.gridViewDiagnosticsDatabases.LeftCoordChanged += new EventHandler(this.gridViewDiagnosticsDatabases_LeftCoordChanged);
			this.gridViewDiagnosticsDatabases.TopRowChanged += new EventHandler(this.gridViewDiagnosticsDatabases_TopRowChanged);
			this.gridViewDiagnosticsDatabases.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewDiagnosticsDatabases_PopupMenuShowing);
			this.gridViewDiagnosticsDatabases.CustomColumnGroup += new CustomColumnSortEventHandler(this.gridViewDiagnosticsDatabases_CustomColumnGroup);
			this.gridViewDiagnosticsDatabases.ShownEditor += new EventHandler(this.gridViewDiagnosticsDatabases_ShownEditor);
			this.gridViewDiagnosticsDatabases.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gridViewDiagnosticsDatabases_FocusedRowChanged);
			this.gridViewDiagnosticsDatabases.CustomColumnSort += new CustomColumnSortEventHandler(this.gridViewDiagnosticsDatabases_CustomColumnSort);
			this.gridViewDiagnosticsDatabases.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewDiagnosticsDatabases_CustomUnboundColumnData);
			this.gridViewDiagnosticsDatabases.KeyDown += new KeyEventHandler(this.gridViewDiagnosticsDatabases_KeyDown);
			this.gridViewDiagnosticsDatabases.DoubleClick += new EventHandler(this.gridViewDiagnosticsDatabases_DoubleClick);
			componentResourceManager.ApplyResources(this.colFileName, "colFileName");
			this.colFileName.FieldName = "anyString1";
			this.colFileName.Name = "colFileName";
			this.colFileName.OptionsColumn.AllowEdit = false;
			this.colFileName.SortMode = ColumnSortMode.Custom;
			this.colFileName.UnboundType = UnboundColumnType.String;
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
			componentResourceManager.ApplyResources(this.colECU, "colECU");
			this.colECU.FieldName = "anyString3";
			this.colECU.Name = "colECU";
			this.colECU.OptionsColumn.AllowEdit = false;
			this.colECU.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colVariant, "colVariant");
			this.colVariant.ColumnEdit = this.repositoryItemComboBox;
			this.colVariant.FieldName = "anyString4";
			this.colVariant.Name = "colVariant";
			this.colVariant.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colDiagnosticsProtocol, "colDiagnosticsProtocol");
			this.colDiagnosticsProtocol.FieldName = "anyString7";
			this.colDiagnosticsProtocol.Name = "colDiagnosticsProtocol";
			this.colDiagnosticsProtocol.OptionsColumn.AllowEdit = false;
			this.colDiagnosticsProtocol.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colCommInterface, "colCommInterface");
			this.colCommInterface.ColumnEdit = this.repositoryItemButtonEditCommInterface;
			this.colCommInterface.FieldName = "anyString8";
			this.colCommInterface.Name = "colCommInterface";
			this.colCommInterface.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonEditCommInterface, "repositoryItemButtonEditCommInterface");
			this.repositoryItemButtonEditCommInterface.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEditCommInterface.Name = "repositoryItemButtonEditCommInterface";
			this.repositoryItemButtonEditCommInterface.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEditCommInterface.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEditCommInterface_ButtonClick);
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
			base.Controls.Add(this.gridControlDiagnosticsDatabases);
			base.Name = "DiagnosticsDatabasesGrid";
			componentResourceManager.ApplyResources(this, "$this");
			base.DragDrop += new DragEventHandler(this.DiagnosticsDatabasesGrid_DragDrop);
			base.DragEnter += new DragEventHandler(this.DiagnosticsDatabasesGrid_DragEnter);
			base.Resize += new EventHandler(this.DiagnosticsDatabasesGrid_Resize);
			((ISupportInitialize)this.gridControlDiagnosticsDatabases).EndInit();
			((ISupportInitialize)this.diagDatabaseBindingSource).EndInit();
			((ISupportInitialize)this.gridViewDiagnosticsDatabases).EndInit();
			((ISupportInitialize)this.repositoryItemComboBox).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEditCommInterface).EndInit();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
