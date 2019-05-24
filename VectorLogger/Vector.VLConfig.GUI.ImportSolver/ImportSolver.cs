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
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.ConfigurationPersistency;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.ImportSolver
{
	public class ImportSolver : Form
	{
		private enum Action
		{
			kUseZIP,
			kUsePath,
			kReplace
		}

		private class Setting
		{
			public List<IFile> ConfiguredFiles
			{
				get;
				private set;
			}

			public IFile ConfiguredFile
			{
				get
				{
					return this.ConfiguredFiles[0];
				}
			}

			public string FilePath
			{
				get;
				private set;
			}

			public string FileName
			{
				get
				{
					return Path.GetFileName(this.FilePath);
				}
			}

			public string NewFileName
			{
				get;
				set;
			}

			public string Location
			{
				get
				{
					return FileSystemServices.GetFolderPathFromFilePath(this.FilePath);
				}
			}

			public ImportSolver.Action Action
			{
				get;
				set;
			}

			public string NewLocation
			{
				get;
				set;
			}

			public bool ExistsInZIP
			{
				get;
				set;
			}

			public Setting(ref IFile configuredFile, string newLocation, bool fileExistsInZip)
			{
				this.ExistsInZIP = fileExistsInZip;
				this.NewFileName = "";
				this.NewLocation = newLocation;
				this.ConfiguredFiles = new List<IFile>();
				this.ConfiguredFiles.Add(configuredFile);
				this.FilePath = this.ConfiguredFile.FilePath.Value;
				this.Action = this.GetDefaultAction(ref configuredFile);
			}

			public bool AddConfiguredFileInstance(ref IFile configuredFile)
			{
				if (!this.ConfiguredFiles.Contains(configuredFile))
				{
					this.ConfiguredFiles.Add(configuredFile);
					return true;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				ImportSolver.Setting setting = obj as ImportSolver.Setting;
				return setting != null && this.ConfiguredFile.FilePath.Value == setting.ConfiguredFile.FilePath.Value;
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			private ImportSolver.Action GetDefaultAction(ref IFile configuredFile)
			{
				bool flag = File.Exists(configuredFile.FilePath.Value);
				if (flag && this.ExistsInZIP)
				{
					return ImportSolver.Action.kUseZIP;
				}
				if (!flag && this.ExistsInZIP)
				{
					return ImportSolver.Action.kUseZIP;
				}
				return ImportSolver.Action.kUsePath;
			}
		}

		private ConfigurationManager configManager;

		private readonly string[] comboBoxItems1 = new string[]
		{
			Resources.ImportSolverActionUseFileFromZIP,
			Resources.ImportSolverActionUseFileFromPath,
			Resources.ImportSolverActionReplace
		};

		private readonly string[] comboBoxItems2 = new string[]
		{
			Resources.ImportSolverActionUseFileFromPath,
			Resources.ImportSolverActionReplace
		};

		private IContainer components;

		private Label label;

		private GridControl gridControlExternalFileImport;

		private GridView gridViewExternalFilesImport;

		private GridColumn gridColumnFileName;

		private GridColumn gridColumnLocation;

		private GridColumn gridColumnAction;

		private RepositoryItemComboBox repositoryItemComboBox;

		private GridColumn gridColumnNewLocation;

		private Button buttonHelp;

		private Button buttonCancel;

		private Button buttonOK;

		private BindingSource iFileBindingSource;

		private XtraToolTipController toolTipController;

		private ProjectImporter ProjectImporter
		{
			get;
			set;
		}

		private List<ImportSolver.Setting> Settings
		{
			get;
			set;
		}

		public bool InconsistenciesExist
		{
			get
			{
				return this.Settings.Count > 0;
			}
		}

		public ImportSolver(ref ProjectImporter projectImporter, ref ConfigurationManager configMan)
		{
			this.InitializeComponent();
			this.iFileBindingSource.DataSource = typeof(ImportSolver.Setting);
			this.ProjectImporter = projectImporter;
			this.configManager = configMan;
			this.Settings = this.CreateDefaultSettings(configMan.VLProject);
			this.gridControlExternalFileImport.DataSource = this.Settings;
			this.gridViewExternalFilesImport.BestFitColumns();
		}

		private List<ImportSolver.Setting> CreateDefaultSettings(VLProject project)
		{
			List<IFile> list = this.GatherExternalAndMissingProjectFiles(project);
			List<ImportSolver.Setting> list2 = new List<ImportSolver.Setting>();
			foreach (IFile current in list)
			{
				bool flag = false;
				string newLocation = "";
				if (FileSystemServices.IsAbsolutePath(current.FilePath.Value))
				{
					ProjectImporter.ExternalFile externalFile = null;
					flag = this.ProjectImporter.ExternalFiles.TryGetValue(current.FilePath.Value, out externalFile);
					if (flag && externalFile != null)
					{
						newLocation = Path.DirectorySeparatorChar + externalFile.DirectoryPathInArchive;
					}
				}
				IFile file = current;
				ImportSolver.Setting item = new ImportSolver.Setting(ref file, newLocation, flag);
				int num = list2.IndexOf(item);
				if (num == -1)
				{
					list2.Add(item);
				}
				else
				{
					list2[num].AddConfiguredFileInstance(ref file);
				}
			}
			return list2;
		}

		private List<IFile> GatherExternalAndMissingProjectFiles(VLProject project)
		{
			List<IFile> result = new List<IFile>();
			ReadOnlyCollection<Database> allDescriptionFiles = project.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.AllDescriptionFiles;
			this.Gather<Database>(ref allDescriptionFiles, ref result);
			ReadOnlyCollection<Database> cCPXCPDatabases = project.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.CCPXCPDatabases;
			this.Gather<Database>(ref cCPXCPDatabases, ref result);
			ReadOnlyCollection<DiagnosticsDatabase> databases = project.ProjectRoot.LoggingConfiguration.DiagnosticsDatabaseConfiguration.Databases;
			this.Gather<DiagnosticsDatabase>(ref databases, ref result);
			ReadOnlyCollection<IncludeFile> includeFiles = project.ProjectRoot.LoggingConfiguration.IncludeFileConfiguration.IncludeFiles;
			this.Gather<IncludeFile>(ref includeFiles, ref result);
			WebDisplayFile customWebDisplay = project.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.CustomWebDisplay;
			if (customWebDisplay != null)
			{
				ReadOnlyCollection<WebDisplayFile> readOnlyCollection = new ReadOnlyCollection<WebDisplayFile>(new List<WebDisplayFile>
				{
					customWebDisplay
				});
				this.Gather<WebDisplayFile>(ref readOnlyCollection, ref result);
			}
			GPSConfiguration gPSConfiguration = project.ProjectRoot.HardwareConfiguration.GPSConfiguration;
			ReadOnlyCollection<GPSConfiguration> readOnlyCollection2 = new ReadOnlyCollection<GPSConfiguration>(new List<GPSConfiguration>
			{
				gPSConfiguration
			});
			this.Gather<GPSConfiguration>(ref readOnlyCollection2, ref result);
			return result;
		}

		private void Gather<T>(ref ReadOnlyCollection<T> collection, ref List<IFile> fileList) where T : IFile
		{
			foreach (IFile file in collection)
			{
				if (FileSystemServices.IsAbsolutePath(file.FilePath.Value))
				{
					fileList.Add(file);
				}
				else if (file.FilePath.Value.Length > 0)
				{
					string absolutePath = FileSystemServices.GetAbsolutePath(file.FilePath.Value, this.configManager.VLProject.GetProjectFolder());
					if (!File.Exists(absolutePath))
					{
						fileList.Add(file);
					}
				}
				Database database = file as Database;
				if (database != null)
				{
					ReadOnlyCollection<CPProtection> cpProtections = database.CpProtections;
					this.Gather<CPProtection>(ref cpProtections, ref fileList);
				}
			}
		}

		private void gridViewExternalFilesImport_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			if (e.Column == this.gridColumnFileName)
			{
				this.UnboundColumnFileName(e);
				return;
			}
			if (e.Column == this.gridColumnLocation)
			{
				this.UnboundColumnLocation(e);
				return;
			}
			if (e.Column == this.gridColumnAction)
			{
				this.UnboundColumnAction(e);
				return;
			}
			if (e.Column == this.gridColumnNewLocation)
			{
				this.UnboundColumnNewLocation(e);
			}
		}

		private void gridViewExternalFilesImport_ShownEditor(object sender, EventArgs e)
		{
			if (this.gridViewExternalFilesImport.FocusedColumn == this.gridColumnAction)
			{
				this.ShownEditorAction();
			}
		}

		private void repositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewExternalFilesImport.PostEditor();
		}

		private void gridViewExternalFilesImport_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			if (e.Column == this.gridColumnLocation)
			{
				ImportSolver.Setting setting = this.gridViewExternalFilesImport.GetRow(e.RowHandle) as ImportSolver.Setting;
				GridCellInfo gridCellInfo = e.Cell as GridCellInfo;
				if (gridCellInfo != null && setting != null && !File.Exists(setting.ConfiguredFile.FilePath.Value))
				{
					gridCellInfo.ViewInfo.ErrorIconText = Resources.ErrorFileNotFound;
					gridCellInfo.ViewInfo.ShowErrorIcon = true;
					gridCellInfo.ViewInfo.ErrorIcon = Resources.ImageWarning;
					gridCellInfo.ViewInfo.CalcViewInfo(e.Graphics);
				}
			}
		}

		private void gridViewExternalFilesImport_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (e.MenuType == GridMenuType.Column)
			{
				this.CustomizeColumnHeaderMenu(e);
			}
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			foreach (ImportSolver.Setting current in this.Settings)
			{
				string text = current.NewFileName;
				if (text.Length == 0)
				{
					text = current.FileName;
				}
				string text2 = current.NewLocation + Path.DirectorySeparatorChar + text;
				FileSystemServices.TryMakeFilePathRelativeToConfiguration(this.configManager.VLProject.GetProjectFolder(), ref text2);
				if (text2.Length == 0)
				{
					text2 = current.NewLocation + Path.DirectorySeparatorChar + text;
				}
				switch (current.Action)
				{
				case ImportSolver.Action.kUseZIP:
				{
					ProjectImporter.ExternalFile externalFile;
					if (this.ProjectImporter.ExternalFiles.TryGetValue(current.ConfiguredFile.FilePath.Value, out externalFile) && this.ProjectImporter.ImportFile(current.FileName, externalFile.DirectoryPathInArchive, this.configManager.VLProject.GetProjectFolder() + current.NewLocation))
					{
						foreach (IFile current2 in current.ConfiguredFiles)
						{
							this.ReplacePath(current2, text2);
						}
					}
					if (current.FileName == Resources.WebDisplayIndexName)
					{
						this.ImportWebDisplayUserFiles(current);
					}
					break;
				}
				case ImportSolver.Action.kReplace:
					foreach (IFile current3 in current.ConfiguredFiles)
					{
						this.ReplacePath(current3, text2);
					}
					break;
				}
			}
		}

		private void buttonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(GUIUtil.HelpPageID_PackAndGo);
		}

		private void ImportSolver_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(GUIUtil.HelpPageID_PackAndGo);
		}

		private void ReplacePath(IFile file, string newFilePath)
		{
			Database database = file as Database;
			DiagnosticsDatabase diagnosticsDatabase = file as DiagnosticsDatabase;
			if (database != null)
			{
				this.configManager.ModelEditor.ReplaceDatabaseAndUpdateSymbolicDefinitions(database, newFilePath);
				return;
			}
			if (diagnosticsDatabase != null)
			{
				DiagnosticsDatabaseConfiguration arg_47_0 = this.configManager.VLProject.ProjectRoot.LoggingConfiguration.DiagnosticsDatabaseConfiguration;
				DiagnosticActionsConfiguration arg_62_0 = this.configManager.VLProject.ProjectRoot.LoggingConfiguration.DiagnosticActionsConfiguration;
				string absolutePath = FileSystemServices.GetAbsolutePath(newFilePath, this.configManager.VLProject.GetProjectFolder());
				this.configManager.ModelEditor.ReplaceDiagnosticsDatabase(ref diagnosticsDatabase, absolutePath);
				return;
			}
			file.FilePath.Value = newFilePath;
		}

		private bool ImportWebDisplayUserFiles(ImportSolver.Setting setting)
		{
			bool flag = true;
			string path = Path.Combine(Path.GetDirectoryName(setting.FilePath), "userfiles");
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			foreach (KeyValuePair<string, ProjectImporter.ExternalFile> current in this.ProjectImporter.ExternalFiles)
			{
				DirectoryInfo directoryInfo2 = new DirectoryInfo(current.Value.ExtractDirectory);
				bool flag2 = directoryInfo.FullName == directoryInfo2.FullName;
				while (directoryInfo2.Parent != null && !flag2)
				{
					if (directoryInfo2.Parent.FullName == directoryInfo.FullName)
					{
						flag2 = true;
					}
					else
					{
						directoryInfo2 = directoryInfo2.Parent;
					}
				}
				if (flag2)
				{
					flag &= this.ProjectImporter.ImportFile(Path.GetFileName(current.Key), current.Value.DirectoryPathInArchive, Path.Combine(this.configManager.VLProject.GetProjectFolder(), current.Value.DirectoryPathInArchive));
				}
			}
			return flag;
		}

		private void UnboundColumnFileName(CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				ImportSolver.Setting setting = e.Row as ImportSolver.Setting;
				if (setting != null)
				{
					if (setting.NewFileName.Length == 0)
					{
						e.Value = setting.FileName;
						return;
					}
					e.Value = setting.NewFileName;
				}
			}
		}

		private void UnboundColumnLocation(CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				ImportSolver.Setting setting = e.Row as ImportSolver.Setting;
				if (setting != null)
				{
					e.Value = setting.Location;
				}
			}
		}

		private void UnboundColumnAction(CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				ImportSolver.Setting setting = e.Row as ImportSolver.Setting;
				if (setting != null)
				{
					e.Value = this.GetActionString(setting.Action);
					return;
				}
			}
			else
			{
				ImportSolver.Setting setting2 = e.Row as ImportSolver.Setting;
				if (setting2 != null)
				{
					ImportSolver.Action action = this.GetActionByString(e.Value as string);
					ImportSolver.Action action2 = setting2.Action;
					if (action2 != action)
					{
						ProjectImporter.ExternalFile externalFile;
						if (setting2.ExistsInZIP && this.ProjectImporter.ExternalFiles.TryGetValue(setting2.ConfiguredFile.FilePath.Value, out externalFile))
						{
							setting2.NewLocation = Path.DirectorySeparatorChar + externalFile.DirectoryPathInArchive;
						}
						setting2.NewFileName = "";
					}
					if (action == ImportSolver.Action.kReplace)
					{
						FileType fileType = FileType.AnyDatabase;
						string extension = Path.GetExtension(setting2.FileName);
						if (Resources_Files.FileFilterIncludeFile.IndexOf(extension) > 0)
						{
							fileType = FileType.INCFile;
						}
						else if (Resources_Files.FileFilterDBC.IndexOf(extension) > 0)
						{
							fileType = FileType.DBCDatabase;
						}
						else if (Resources_Files.FileFilterLDF.IndexOf(extension) > 0)
						{
							fileType = FileType.LDFDatabase;
						}
						else if (Resources_Files.FileFilterDatabaseXML.IndexOf(extension) > 0)
						{
							fileType = FileType.XMLDatabase;
						}
						else if ("arxml".IndexOf(extension) > 0)
						{
							fileType = FileType.ARXMLDatabase;
						}
						else if (Resources_Files.FileFilterDiagnosticsDbCDD.IndexOf(extension) > 0)
						{
							fileType = FileType.CDDDiagDesc;
						}
						else if (Resources_Files.FileFilterDiagnosticsDbODX.IndexOf(extension) > 0)
						{
							fileType = FileType.ODXDiagDesc;
						}
						else if (Resources_Files.FileFilterDiagnosticsDbPDX.IndexOf(extension) > 0)
						{
							fileType = FileType.PDXDiagDesc;
						}
						else if (Resources_Files.FileFilterDiagnosticsDbMDX.IndexOf(extension) > 0)
						{
							fileType = FileType.MDXDiagDesc;
						}
						else if (Resources_Files.FileFilterSKB.IndexOf(extension) > 0)
						{
							fileType = FileType.SKBFile;
						}
						else if (Vocabulary.FileFilterWebDisplayIndex.IndexOf(extension) > 0)
						{
							fileType = FileType.WebDisplayIndex;
						}
						GenericOpenFileDialog.FileName = "";
						if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(fileType))
						{
							string extension2 = Path.GetExtension(GenericOpenFileDialog.FileName);
							if (extension2 == extension)
							{
								setting2.NewFileName = Path.GetFileName(GenericOpenFileDialog.FileName);
								setting2.NewLocation = Path.GetDirectoryName(GenericOpenFileDialog.FileName);
							}
							else
							{
								InformMessageBox.Error(Resources.ImportSolverIncompatibleFileTypeError, new string[]
								{
									extension,
									extension2
								});
								action = action2;
							}
						}
						else
						{
							action = action2;
						}
					}
					if (action != action2)
					{
						setting2.Action = action;
					}
				}
			}
		}

		private void UnboundColumnNewLocation(CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				ImportSolver.Setting setting = e.Row as ImportSolver.Setting;
				if (setting != null && setting.Action != ImportSolver.Action.kUsePath)
				{
					e.Value = setting.NewLocation;
				}
			}
		}

		private void ShownEditorAction()
		{
			ImportSolver.Setting setting = this.gridViewExternalFilesImport.GetFocusedRow() as ImportSolver.Setting;
			ComboBoxEdit comboBoxEdit = this.gridViewExternalFilesImport.ActiveEditor as ComboBoxEdit;
			if (setting != null && comboBoxEdit != null)
			{
				comboBoxEdit.Properties.Items.AddRange(this.GetComboBoxItems(setting.ConfiguredFile));
			}
		}

		private string[] GetComboBoxItems(IFile f)
		{
			if (f == null)
			{
				return this.comboBoxItems1;
			}
			ProjectImporter.ExternalFile externalFile;
			bool flag = this.ProjectImporter.ExternalFiles.TryGetValue(f.FilePath.Value, out externalFile);
			if (flag)
			{
				return this.comboBoxItems1;
			}
			return this.comboBoxItems2;
		}

		private string GetActionString(ImportSolver.Action option)
		{
			return this.comboBoxItems1[(int)option];
		}

		private ImportSolver.Action GetActionByString(string strOption)
		{
			for (int i = 0; i < this.comboBoxItems1.Length; i++)
			{
				if (this.comboBoxItems1[i] == strOption)
				{
					return (ImportSolver.Action)i;
				}
			}
			return ImportSolver.Action.kUsePath;
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
			for (int i = e.Menu.Items.Count - 1; i >= 0; i--)
			{
				string caption = e.Menu.Items[i].Caption;
				if (localizedString5 == caption || localizedString6 == caption || caption.Contains(localizedString) || localizedString2 == caption || localizedString3 == caption || localizedString4 == caption || localizedString7 == caption || localizedString8 == caption || localizedString9 == caption)
				{
					e.Menu.Items.RemoveAt(i);
				}
			}
		}

		private void repositoryItemComboBox_Closed(object sender, ClosedEventArgs e)
		{
			this.gridViewExternalFilesImport.PostEditor();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ImportSolver));
			this.label = new Label();
			this.gridControlExternalFileImport = new GridControl();
			this.iFileBindingSource = new BindingSource(this.components);
			this.gridViewExternalFilesImport = new GridView();
			this.gridColumnFileName = new GridColumn();
			this.gridColumnLocation = new GridColumn();
			this.gridColumnAction = new GridColumn();
			this.repositoryItemComboBox = new RepositoryItemComboBox();
			this.gridColumnNewLocation = new GridColumn();
			this.toolTipController = new XtraToolTipController(this.components);
			this.buttonHelp = new Button();
			this.buttonCancel = new Button();
			this.buttonOK = new Button();
			((ISupportInitialize)this.gridControlExternalFileImport).BeginInit();
			((ISupportInitialize)this.iFileBindingSource).BeginInit();
			((ISupportInitialize)this.gridViewExternalFilesImport).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBox).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.label, "label");
			this.label.Name = "label";
			componentResourceManager.ApplyResources(this.gridControlExternalFileImport, "gridControlExternalFileImport");
			this.gridControlExternalFileImport.DataSource = this.iFileBindingSource;
			this.gridControlExternalFileImport.MainView = this.gridViewExternalFilesImport;
			this.gridControlExternalFileImport.Name = "gridControlExternalFileImport";
			this.gridControlExternalFileImport.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemComboBox
			});
			this.gridControlExternalFileImport.ToolTipController = this.toolTipController;
			this.gridControlExternalFileImport.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewExternalFilesImport
			});
			this.gridViewExternalFilesImport.Columns.AddRange(new GridColumn[]
			{
				this.gridColumnFileName,
				this.gridColumnLocation,
				this.gridColumnAction,
				this.gridColumnNewLocation
			});
			this.gridViewExternalFilesImport.GridControl = this.gridControlExternalFileImport;
			this.gridViewExternalFilesImport.Name = "gridViewExternalFilesImport";
			this.gridViewExternalFilesImport.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
			this.gridViewExternalFilesImport.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
			this.gridViewExternalFilesImport.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
			this.gridViewExternalFilesImport.OptionsCustomization.AllowFilter = false;
			this.gridViewExternalFilesImport.OptionsCustomization.AllowGroup = false;
			this.gridViewExternalFilesImport.OptionsCustomization.AllowQuickHideColumns = false;
			this.gridViewExternalFilesImport.OptionsDetail.EnableMasterViewMode = false;
			this.gridViewExternalFilesImport.OptionsView.ShowGroupPanel = false;
			this.gridViewExternalFilesImport.OptionsView.ShowIndicator = false;
			this.gridViewExternalFilesImport.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewExternalFilesImport_CustomDrawCell);
			this.gridViewExternalFilesImport.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewExternalFilesImport_PopupMenuShowing);
			this.gridViewExternalFilesImport.ShownEditor += new EventHandler(this.gridViewExternalFilesImport_ShownEditor);
			this.gridViewExternalFilesImport.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewExternalFilesImport_CustomUnboundColumnData);
			componentResourceManager.ApplyResources(this.gridColumnFileName, "gridColumnFileName");
			this.gridColumnFileName.FieldName = "gridColumnFileName";
			this.gridColumnFileName.Name = "gridColumnFileName";
			this.gridColumnFileName.OptionsColumn.AllowEdit = false;
			this.gridColumnFileName.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.gridColumnLocation, "gridColumnLocation");
			this.gridColumnLocation.FieldName = "gridColumnLocation";
			this.gridColumnLocation.Name = "gridColumnLocation";
			this.gridColumnLocation.OptionsColumn.AllowEdit = false;
			this.gridColumnLocation.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.gridColumnAction, "gridColumnAction");
			this.gridColumnAction.ColumnEdit = this.repositoryItemComboBox;
			this.gridColumnAction.FieldName = "gridColumnAction";
			this.gridColumnAction.Name = "gridColumnAction";
			this.gridColumnAction.UnboundType = UnboundColumnType.String;
			this.repositoryItemComboBox.AllowNullInput = DefaultBoolean.False;
			componentResourceManager.ApplyResources(this.repositoryItemComboBox, "repositoryItemComboBox");
			this.repositoryItemComboBox.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBox.Buttons"))
			});
			this.repositoryItemComboBox.Name = "repositoryItemComboBox";
			this.repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBox.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBox_SelectedIndexChanged);
			this.repositoryItemComboBox.Closed += new ClosedEventHandler(this.repositoryItemComboBox_Closed);
			componentResourceManager.ApplyResources(this.gridColumnNewLocation, "gridColumnNewLocation");
			this.gridColumnNewLocation.FieldName = "gridColumnNewLocation";
			this.gridColumnNewLocation.Name = "gridColumnNewLocation";
			this.gridColumnNewLocation.OptionsColumn.AllowEdit = false;
			this.gridColumnNewLocation.UnboundType = UnboundColumnType.String;
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
			componentResourceManager.ApplyResources(this.buttonHelp, "buttonHelp");
			this.buttonHelp.Name = "buttonHelp";
			this.buttonHelp.UseVisualStyleBackColor = true;
			this.buttonHelp.Click += new EventHandler(this.buttonHelp_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.DialogResult = DialogResult.OK;
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ControlBox = false;
			base.Controls.Add(this.buttonHelp);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.gridControlExternalFileImport);
			base.Controls.Add(this.label);
			base.Name = "ImportSolver";
			base.HelpRequested += new HelpEventHandler(this.ImportSolver_HelpRequested);
			((ISupportInitialize)this.gridControlExternalFileImport).EndInit();
			((ISupportInitialize)this.iFileBindingSource).EndInit();
			((ISupportInitialize)this.gridViewExternalFilesImport).EndInit();
			((ISupportInitialize)this.repositoryItemComboBox).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
