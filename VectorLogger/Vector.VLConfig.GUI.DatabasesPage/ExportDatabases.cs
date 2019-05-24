using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Conversion;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.DatabasesPage
{
	public class ExportDatabases : UserControl, IPropertyWindow, IConfigClipboardClient, IUpdateObserver<LoggerType>, IUpdateObserver<ExportDatabaseConfiguration>, IUpdateObserver
	{
		public delegate void StatusChangedHandler();

		public delegate void ModeChangedHandler();

		private LoggerType loggerType;

		private IContainer components;

		private Button buttonAdd;

		private ExportDatabaseGrid exportDatabaseGrid;

		private Button buttonRemove;

		private TableLayoutPanel tableLayoutPanel1;

		private Button buttonImport;

		private Button buttonLoadAnalysisPackage;

		private Panel panel1;

		private Button buttonSaveAs;

		private ToolTip toolTip1;

		private Button buttonRemoveAll;

		private ComboBox comboBoxDBSelectionMode;

		private Button buttonBrowseToAnalysisPackage;

		public event ExportDatabases.StatusChangedHandler StatusChanged;

		public event ExportDatabases.ModeChangedHandler ModeChanged;

		public event FileConversion.FileConversionParametersChangedHandler FileConversionParametersChanged;

		public new event EventHandler Validating;

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.exportDatabaseGrid.ApplicationDatabaseManager;
			}
			set
			{
				this.exportDatabaseGrid.ApplicationDatabaseManager = value;
				if (this.ApplicationDatabaseManager != null)
				{
					this.ApplicationDatabaseManager.AddExportDatabaseConfiguration(this.exportDatabaseGrid.ExportDatabaseConfiguration);
				}
			}
		}

		public IConfigurationManagerService ConfigurationManagerService
		{
			get
			{
				return this.exportDatabaseGrid.ConfigurationManagerService;
			}
			set
			{
				this.exportDatabaseGrid.ConfigurationManagerService = value;
				if (this.exportDatabaseGrid.ConfigurationManagerService != null)
				{
					this.ApplicationDatabaseManager = this.exportDatabaseGrid.ConfigurationManagerService.ApplicationDatabaseManager;
				}
			}
		}

		public FileConversionParameters FileConversionParameters
		{
			get
			{
				return this.exportDatabaseGrid.FileConversionParameters;
			}
			set
			{
				this.exportDatabaseGrid.FileConversionParameters = value;
				if (value != null)
				{
					this.EnableControls(this.exportDatabaseGrid.ExportDatabaseConfiguration.IsExportDatabaseConfigurationEnabled);
					foreach (TaggedItem<ExportDatabaseConfiguration.DBSelectionMode> taggedItem in this.comboBoxDBSelectionMode.Items)
					{
						if (taggedItem.Tag == value.ExportDatabaseConfiguration.Mode)
						{
							this.comboBoxDBSelectionMode.SelectedItem = taggedItem;
						}
					}
					this.ValidateInput();
				}
			}
		}

		public AnalysisPackageParameters AnalysisPackageParameters
		{
			get;
			set;
		}

		public IModelValidator ModelValidator
		{
			get
			{
				return this.exportDatabaseGrid.ModelValidator;
			}
			set
			{
				this.exportDatabaseGrid.ModelValidator = value;
			}
		}

		public bool IsVLExportMode
		{
			get;
			set;
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.exportDatabaseGrid.ModelValidator;
			}
			set
			{
				this.exportDatabaseGrid.ModelValidator = value;
			}
		}

		public ISemanticChecker SemanticChecker
		{
			get
			{
				return this.exportDatabaseGrid.SemanticChecker;
			}
			set
			{
				this.exportDatabaseGrid.SemanticChecker = value;
			}
		}

		IModelEditor IPropertyWindow.ModelEditor
		{
			get;
			set;
		}

		public DisplayMode DisplayMode
		{
			get;
			set;
		}

		IUpdateService IPropertyWindow.UpdateService
		{
			get;
			set;
		}

		IUpdateObserver IPropertyWindow.UpdateObserver
		{
			get
			{
				return this;
			}
		}

		PageType IPropertyWindow.Type
		{
			get
			{
				return PageType.ExportDatabases;
			}
		}

		bool IPropertyWindow.IsVisible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				bool visible = base.Visible;
				base.Visible = value;
				if (!visible && base.Visible)
				{
					this.exportDatabaseGrid.DisplayErrors();
				}
			}
		}

		public List<Vector.VLConfig.Data.ConfigurationDataModel.Action> FocusedActions
		{
			get
			{
				return null;
			}
		}

		public ExportDatabases()
		{
			this.InitializeComponent();
			this.exportDatabaseGrid.SelectionChanged += new EventHandler(this.OnDatabaseGridSelectionChanged);
			this.IsVLExportMode = false;
			this.comboBoxDBSelectionMode.Items.Add(new TaggedItem<ExportDatabaseConfiguration.DBSelectionMode>(Resources.ExportDatabasesConfigSelectionModeFromConfig, ExportDatabaseConfiguration.DBSelectionMode.FromConfig));
			this.comboBoxDBSelectionMode.Items.Add(new TaggedItem<ExportDatabaseConfiguration.DBSelectionMode>(Resources.ExportDatabasesConfigSelectionModeManual, ExportDatabaseConfiguration.DBSelectionMode.Manual));
			this.comboBoxDBSelectionMode.Items.Add(new TaggedItem<ExportDatabaseConfiguration.DBSelectionMode>(Resources.ExportDatabasesConfigSelectionModeAnalysisPackage, ExportDatabaseConfiguration.DBSelectionMode.FromAnalysisPackage));
			this.comboBoxDBSelectionMode.SelectedIndex = 0;
		}

		private void buttonAdd_Click(object sender, EventArgs e)
		{
			FileType fileType = FileType.AnyDatabase;
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(fileType))
			{
				this.exportDatabaseGrid.AddDatabase(GenericOpenFileDialog.FileName);
			}
			this.exportDatabaseGrid.ValidateInput(false);
			this.FireStatusChanged();
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			this.exportDatabaseGrid.RemoveDatabase();
			this.FireStatusChanged();
		}

		private void buttonRemoveAll_Click(object sender, EventArgs e)
		{
			if (DialogResult.Yes == InformMessageBox.Show(EnumInfoType.Info, EnumQuestionType.QuestionDefaultNo, Resources.AnalysisRemoveDatabasesFromList))
			{
				this.exportDatabaseGrid.ClearDatabases();
				this.FireStatusChanged();
			}
		}

		private void buttonImport_Click(object sender, EventArgs e)
		{
			this.ImportDatabasesFromConfiguration(false);
		}

		public void ImportDatabasesFromConfiguration(bool clearCurrentDBs = false)
		{
			if (clearCurrentDBs)
			{
				this.exportDatabaseGrid.ClearDatabases();
			}
			foreach (Database current in this.ConfigurationManagerService.DatabaseConfiguration.Databases)
			{
				this.exportDatabaseGrid.AddDatabase(current);
			}
			this.exportDatabaseGrid.ValidateInput(false);
			this.FireStatusChanged();
		}

		private void buttonSaveAs_Click(object sender, EventArgs e)
		{
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				string text = "";
				saveFileDialog.Filter = Resources_Files.FileFilterAnalysisDatabasefile;
				if (!string.IsNullOrEmpty(text))
				{
					saveFileDialog.InitialDirectory = Path.GetDirectoryName(text);
				}
				if (DialogResult.OK == saveFileDialog.ShowDialog())
				{
					if (File.Exists(saveFileDialog.FileName))
					{
						File.Delete(saveFileDialog.FileName);
					}
					if (!AnalysisPackage.CreateChannelMappingFile(this.exportDatabaseGrid.ExportDatabaseConfiguration.Databases, saveFileDialog.FileName, "", "", null))
					{
						InformMessageBox.Error(Resources.FileSaveError, new string[]
						{
							saveFileDialog.FileName
						});
					}
				}
			}
		}

		private void checkBoxUseFromConfiguration_CheckedChanged(object sender, EventArgs e)
		{
			this.EnableControls();
			this.FireStatusChanged();
		}

		private void buttonBrowseToAnalysisPackage_Click(object sender, EventArgs e)
		{
			string analysisPackagePath = this.exportDatabaseGrid.ExportDatabaseConfiguration.GetAnalysisPackagePath();
			if (!string.IsNullOrEmpty(analysisPackagePath))
			{
				FileSystemServices.LaunchDirectoryBrowser(analysisPackagePath);
			}
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return feature is ExportDatabaseConfiguration;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.Databases);
			}
			if (this.exportDatabaseGrid.FileConversionParameters == null)
			{
				this.exportDatabaseGrid.FileConversionParameters = FileConversionHelper.GetDefaultParameters(this.ModelValidator.LoggerSpecifics);
			}
			this.exportDatabaseGrid.Init();
		}

		bool IPropertyWindow.ValidateInput()
		{
			return this.exportDatabaseGrid.ValidateInput(false);
		}

		void IPropertyWindow.Reset()
		{
			this.exportDatabaseGrid.Reset();
		}

		bool IPropertyWindow.HasErrors()
		{
			return this.exportDatabaseGrid.HasErrors();
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			return this.exportDatabaseGrid.HasGlobalErrors();
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			return this.exportDatabaseGrid.HasLocalErrors();
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			return this.exportDatabaseGrid.HasFormatErrors();
		}

		void IUpdateObserver<ExportDatabaseConfiguration>.Update(ExportDatabaseConfiguration data)
		{
			this.exportDatabaseGrid.ExportDatabaseConfiguration = data;
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType data)
		{
			if (this.loggerType == data)
			{
				return;
			}
			this.loggerType = data;
		}

		public ConfigClipboardManager.AcceptType Accept(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			return ConfigClipboardManager.AcceptType.None;
		}

		public bool Insert(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			return false;
		}

		public bool Insert(Event evt)
		{
			return false;
		}

		private void OnDatabaseGridSelectionChanged(object sender, EventArgs e)
		{
			ExportDatabase exportDatabase;
			bool flag = this.exportDatabaseGrid.TryGetSelectedDatabase(out exportDatabase) && this.exportDatabaseGrid.ExportDatabaseConfiguration.Mode == ExportDatabaseConfiguration.DBSelectionMode.Manual;
			this.buttonRemove.Enabled = (flag && !exportDatabase.IsInsideAnalysisPackage());
			this.buttonRemoveAll.Enabled = flag;
			this.buttonSaveAs.Enabled = flag;
			this.buttonRemoveAll.Enabled = flag;
		}

		private void EnableControls()
		{
			bool enabled = this.exportDatabaseGrid.ExportDatabaseConfiguration != null && this.exportDatabaseGrid.ExportDatabaseConfiguration.Mode == ExportDatabaseConfiguration.DBSelectionMode.Manual;
			this.EnableControls(enabled);
		}

		private void EnableControls(bool enabled)
		{
			this.buttonAdd.Enabled = enabled;
			this.buttonRemove.Enabled = enabled;
			this.buttonRemoveAll.Enabled = enabled;
			this.buttonSaveAs.Enabled = enabled;
			if (this.ModelValidator != null)
			{
				this.buttonImport.Enabled = (enabled && this.ModelValidator.DatabaseServices.HasDatabasesAccessibleFromConfiguration(true));
			}
			this.buttonLoadAnalysisPackage.Enabled = enabled;
			this.exportDatabaseGrid.EnableGridControl(enabled);
			if (enabled)
			{
				this.OnDatabaseGridSelectionChanged(this, EventArgs.Empty);
			}
			string value = (this.exportDatabaseGrid != null && this.exportDatabaseGrid.ExportDatabaseConfiguration != null) ? this.exportDatabaseGrid.ExportDatabaseConfiguration.GetAnalysisPackagePath() : string.Empty;
			if (!string.IsNullOrEmpty(value) && this.exportDatabaseGrid.ExportDatabaseConfiguration.Mode == ExportDatabaseConfiguration.DBSelectionMode.FromAnalysisPackage)
			{
				this.buttonBrowseToAnalysisPackage.Enabled = true;
				return;
			}
			this.buttonBrowseToAnalysisPackage.Enabled = false;
		}

		private void FireStatusChanged()
		{
			if (this.StatusChanged != null)
			{
				this.StatusChanged();
			}
			AnalysisPackage.ExportDatabasesConfigurationChanged();
			this.Raise_Validating(this, EventArgs.Empty);
		}

		private void FireModeChanged()
		{
			if (this.ModeChanged != null)
			{
				this.ModeChanged();
			}
			AnalysisPackage.ExportDatabasesConfigurationChanged();
			this.Raise_Validating(this, EventArgs.Empty);
		}

		private void Raise_Validating(object sender, EventArgs e)
		{
			if (this.Validating != null)
			{
				this.Validating(sender, e);
			}
		}

		private void buttonLoadAnalysisPackage_Click(object sender, EventArgs e)
		{
			GenericOpenFileDialog.FileName = "";
			DialogResult dialogResult;
			if (this.IsVLExportMode)
			{
				dialogResult = GenericOpenFileDialog.ShowDialog("", "Analysis Package (*.gla, *.zip, *.glc)|*.gla;*.zip;*.glc");
			}
			else
			{
				dialogResult = GenericOpenFileDialog.ShowDialog("", FileType.AnalysisPackage);
			}
			if (DialogResult.OK == dialogResult)
			{
				this.LoadAnalysisPackage(GenericOpenFileDialog.FileName);
			}
		}

		public void LoadAnalysisPackage(string analysis_path)
		{
			this.exportDatabaseGrid.ClearDatabasesFromAnalysisPackage();
			if (this.exportDatabaseGrid.ExportDatabaseConfiguration.Databases.Any<ExportDatabase>())
			{
				if (this.AnalysisPackageParameters.DoNotAskToRemoveExistingDatabases && this.AnalysisPackageParameters.AutomaticallyRemoveExistingDatabases)
				{
					this.exportDatabaseGrid.ClearDatabases();
				}
				else if (!this.AnalysisPackageParameters.DoNotAskToRemoveExistingDatabases && DialogResult.Yes == InformMessageBox.Show(EnumInfoType.Info, EnumQuestionType.QuestionDefaultNo, Resources.AnalysisRemoveDatabasesFromList))
				{
					this.exportDatabaseGrid.ClearDatabases();
				}
			}
			bool flag = false;
			string a = (Path.GetExtension(analysis_path) ?? string.Empty).ToLower();
			if (a == Vocabulary.FileExtensionDotZIP)
			{
				flag |= this.LoadFromStream(AnalysisPackage.OpenChannelMappingFile(analysis_path), analysis_path, false);
			}
			else if (a == Vocabulary.FileExtensionDotGLC)
			{
				VLProject vLProject = new VLProject();
				ILoggerSpecifics loggerSpecifics;
				bool flag2;
				vLProject.LoadProjectFile(analysis_path, out loggerSpecifics, out flag2);
				this.exportDatabaseGrid.ClearDatabases();
				foreach (Database current in vLProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.Databases)
				{
					if (current.CcpXcpEcuDisplayName == null)
					{
						current.InitializeValidatedRuntimeProperties();
					}
					current.FilePath.Value = FileSystemServices.GetAbsolutePath(current.FilePath.Value, vLProject.GetProjectFolder());
					this.exportDatabaseGrid.AddDatabase(current);
				}
				vLProject.CloseProject();
				flag = (this.exportDatabaseGrid.ExportDatabaseConfiguration.Databases.Count > 0);
				this.exportDatabaseGrid.ValidateInput(false);
				this.FireStatusChanged();
			}
			else
			{
				flag |= this.LoadFromFile(analysis_path);
			}
			if (!flag)
			{
				InformMessageBox.Info(Resources.AnalysisErrorWhileLoading);
			}
			this.ValidateInput();
			this.FireStatusChanged();
		}

		public void LoadAnalysisPackageAutomatically(string analysis_path)
		{
			this.exportDatabaseGrid.ClearAutomaticAnalysisPackageDatabase();
			bool flag = false;
			string a = (Path.GetExtension(analysis_path) ?? string.Empty).ToLower();
			if (a == Vocabulary.FileExtensionDotZIP)
			{
				flag |= this.LoadFromStream(AnalysisPackage.OpenChannelMappingFile(analysis_path), analysis_path, true);
			}
			if (!flag)
			{
				InformMessageBox.Info(Resources.AnalysisErrorWhileLoading);
			}
			this.ValidateInput();
			this.FireStatusChanged();
		}

		public void ClearAutomaticAnalysisPackage()
		{
			this.exportDatabaseGrid.ClearAutomaticAnalysisPackageDatabase();
		}

		public void ValidateInput()
		{
			this.UpdateSelectionMode();
			if (this.exportDatabaseGrid.ExportDatabaseConfiguration != null && this.exportDatabaseGrid.ExportDatabaseConfiguration.Mode == ExportDatabaseConfiguration.DBSelectionMode.FromConfig)
			{
				this.exportDatabaseGrid.RefreshFromConfig();
			}
			this.Raise_Validating(this, EventArgs.Empty);
			this.EnableControls();
			this.exportDatabaseGrid.ValidateInput(false);
		}

		public bool HasValidationError()
		{
			if (this.FileConversionParameters != null && FileConversionHelper.IsSignalOrientedDestFormat(this.FileConversionParameters.DestinationFormat))
			{
				bool allowFlexRay = !FileConversionHelper.UseMDFLegacyConversion(this.FileConversionParameters);
				if (!this.exportDatabaseGrid.ModelValidator.DatabaseServices.HasDatabasesAccessibleForSignalConversion(this.FileConversionParameters, allowFlexRay))
				{
					return true;
				}
			}
			return false;
		}

		private bool LoadFromFile(string filePath)
		{
			FileInfo fileInfo = new FileInfo(filePath);
			if (!fileInfo.Exists)
			{
				return false;
			}
			bool result = false;
			FileStream fileStream = null;
			try
			{
				fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read);
				result = this.LoadFromStream(fileStream, filePath, false);
			}
			catch (Exception)
			{
				if (fileStream != null)
				{
					fileStream.Close();
				}
				result = false;
			}
			return result;
		}

		private bool LoadFromStream(Stream stream, string analysisFilePath, bool automaticAnalysisPackage = false)
		{
			XmlDocument xmlDocument = new XmlDocument();
			if (stream == null)
			{
				return false;
			}
			try
			{
				xmlDocument.Load(stream);
				stream.Close();
			}
			catch (Exception)
			{
				if (stream != null)
				{
					stream.Close();
				}
				bool result = false;
				return result;
			}
			if (xmlDocument.GetElementsByTagName("AnalysisPackage").Count != 1)
			{
				return false;
			}
			if (xmlDocument.GetElementsByTagName("DatabaseList").Count != 1)
			{
				return false;
			}
			XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("Database");
			foreach (XmlNode node in elementsByTagName)
			{
				ExportDatabase exportDatabase = this.CreateDatabaseFromXMLnode(node, analysisFilePath);
				if (exportDatabase != null)
				{
					if (automaticAnalysisPackage)
					{
						this.exportDatabaseGrid.AddAutomaticAnalysisPackageDatabase(exportDatabase);
					}
					else
					{
						this.exportDatabaseGrid.AddDatabase(exportDatabase);
					}
				}
			}
			return true;
		}

		private ExportDatabase CreateDatabaseFromXMLnode(XmlNode node, string filePath)
		{
			string stringFromXMLnode = this.GetStringFromXMLnode(node, "Path");
			string stringFromXMLnode2 = this.GetStringFromXMLnode(node, "OriginalPath");
			BusType enumFromXMLnode = this.GetEnumFromXMLnode<BusType>(node, "BusType", BusType.Bt_None);
			int intFromXMLnode = this.GetIntFromXMLnode(node, "Channel");
			ExportDatabase.DBType enumFromXMLnode2 = this.GetEnumFromXMLnode<ExportDatabase.DBType>(node, "Type", ExportDatabase.DBType.NonBus);
			string stringFromXMLnode3 = this.GetStringFromXMLnode(node, "NetworkName");
			ExportDatabase.ReferenceType enumFromXMLnode3 = this.GetEnumFromXMLnode<ExportDatabase.ReferenceType>(node, "ReferenceType", ExportDatabase.ReferenceType.Path);
			ExportDatabase exportDatabase = new ExportDatabase();
			exportDatabase.FilePath.Value = stringFromXMLnode;
			exportDatabase.BusType.Value = enumFromXMLnode;
			exportDatabase.ChannelNumber.Value = (uint)intFromXMLnode;
			exportDatabase.AnalysisPackagePath.Value = filePath;
			exportDatabase.RefType = enumFromXMLnode3;
			exportDatabase.NetworkName.Value = stringFromXMLnode3;
			exportDatabase.Type = enumFromXMLnode2;
			if (enumFromXMLnode3 == ExportDatabase.ReferenceType.AnalysisRelative && filePath.EndsWith(Vocabulary.FileExtensionDotGLA))
			{
				exportDatabase.AnalysisPackagePath.Value = stringFromXMLnode2;
				try
				{
					FileInfo fileInfo = new FileInfo(filePath);
					if (fileInfo.Directory != null)
					{
						string path = Path.Combine(fileInfo.Directory.FullName, exportDatabase.FilePath.Value);
						if (File.Exists(path))
						{
							exportDatabase.FilePath.Value = Path.Combine(fileInfo.Directory.FullName, exportDatabase.FilePath.Value);
							exportDatabase.RefType = ExportDatabase.ReferenceType.AnalysisAbsolute;
						}
					}
				}
				catch (Exception)
				{
				}
			}
			return exportDatabase;
		}

		private string GetStringFromXMLnode(XmlNode node, string name)
		{
			string result = "";
			if (node[name] != null)
			{
				result = node[name].InnerText;
			}
			return result;
		}

		private int GetIntFromXMLnode(XmlNode node, string name)
		{
			if (node[name] != null)
			{
				int result = 0;
				if (int.TryParse(node[name].InnerText, out result))
				{
					return result;
				}
			}
			return 0;
		}

		private T GetEnumFromXMLnode<T>(XmlNode node, string name, T defaultValue) where T : struct
		{
			T result;
			if (node[name] != null && Enum.TryParse<T>(node[name].InnerText, out result))
			{
				return result;
			}
			return defaultValue;
		}

		public bool Serialize(ExportDatabasesPage exportDatabasesPage)
		{
			if (exportDatabasesPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.exportDatabaseGrid.Serialize(exportDatabasesPage);
		}

		public bool DeSerialize(ExportDatabasesPage exportDatabasesPage)
		{
			if (exportDatabasesPage == null)
			{
				return false;
			}
			bool flag = true;
			flag &= this.exportDatabaseGrid.DeSerialize(exportDatabasesPage);
			if (flag)
			{
				this.FileConversionParameters = this.exportDatabaseGrid.FileConversionParameters;
			}
			return flag;
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
				foreach (string current in list)
				{
					if (current.EndsWith(Vocabulary.FileExtensionDotGLA) || current.EndsWith(Vocabulary.FileExtensionDotZIP) || current.EndsWith(Vocabulary.FileExtensionDotGLC))
					{
						this.LoadAnalysisPackage(current);
					}
					else
					{
						this.exportDatabaseGrid.AddDatabase(current);
					}
				}
				foreach (TaggedItem<ExportDatabaseConfiguration.DBSelectionMode> taggedItem in this.comboBoxDBSelectionMode.Items)
				{
					if (taggedItem.Tag == ExportDatabaseConfiguration.DBSelectionMode.Manual)
					{
						this.comboBoxDBSelectionMode.SelectedItem = taggedItem;
					}
				}
				this.FireStatusChanged();
				return;
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
						goto IL_13C;
					}
					goto IL_64;
					IL_13C:
					i++;
					continue;
					IL_64:
					bool flag = string.Compare(strA, Vocabulary.FileExtensionDotDBC, true) == 0;
					bool flag2 = string.Compare(strA, Vocabulary.FileExtensionDotLDF, true) == 0;
					bool flag3 = string.Compare(strA, Vocabulary.FileExtensionDotARXML, true) == 0;
					bool flag4 = string.Compare(strA, Vocabulary.FileExtensionDotXML, true) == 0;
					bool flag5 = string.Compare(strA, Vocabulary.FileExtensionDotGLA, true) == 0 || string.Compare(strA, Vocabulary.FileExtensionDotZIP, true) == 0;
					bool flag6 = string.Compare(strA, Vocabulary.FileExtensionDotGLC, true) == 0;
					bool flag7 = true;
					bool flag8 = true;
					if (flag || flag2 || (flag3 && flag8) || (flag4 && (flag7 || flag8)) || flag5 || (flag6 && this.IsVLExportMode))
					{
						acceptedFiles.Add(text);
						result = true;
						goto IL_13C;
					}
					goto IL_13C;
				}
				return result;
			}
			return false;
		}

		public void EnableVLExportMode()
		{
			this.comboBoxDBSelectionMode.Items.RemoveAt(0);
			this.buttonImport.Visible = false;
			if (this.tableLayoutPanel1.ColumnStyles[1] != null)
			{
				this.tableLayoutPanel1.ColumnStyles[1].SizeType = SizeType.Absolute;
				this.tableLayoutPanel1.ColumnStyles[1].Width = 0f;
			}
			this.IsVLExportMode = true;
		}

		public void HideControlsForCLFExportPage()
		{
			this.comboBoxDBSelectionMode.Items.RemoveAt(2);
		}

		public void ClearDatabasesFromAnalysisPackage()
		{
			this.exportDatabaseGrid.ClearDatabasesFromAnalysisPackage();
		}

		public string GetAnalysisPackagePath()
		{
			if (this.exportDatabaseGrid == null)
			{
				return string.Empty;
			}
			return this.exportDatabaseGrid.GetAnalysisPackagePath();
		}

		private void comboBoxDBSelectionMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.UpdateSelectionMode();
			this.ValidateInput();
			this.FireModeChanged();
			if (this.FileConversionParametersChanged != null)
			{
				this.FileConversionParametersChanged(this, null);
			}
		}

		private void UpdateSelectionMode()
		{
			TaggedItem<ExportDatabaseConfiguration.DBSelectionMode> taggedItem = (TaggedItem<ExportDatabaseConfiguration.DBSelectionMode>)this.comboBoxDBSelectionMode.SelectedItem;
			if (taggedItem != null && this.exportDatabaseGrid != null && this.exportDatabaseGrid.ExportDatabaseConfiguration != null)
			{
				this.exportDatabaseGrid.ExportDatabaseConfiguration.Mode = taggedItem.Tag;
				this.exportDatabaseGrid.RefreshGridData();
				this.EnableControls();
				this.FireStatusChanged();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ExportDatabases));
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.buttonLoadAnalysisPackage = new Button();
			this.panel1 = new Panel();
			this.comboBoxDBSelectionMode = new ComboBox();
			this.buttonImport = new Button();
			this.buttonAdd = new Button();
			this.buttonRemove = new Button();
			this.buttonSaveAs = new Button();
			this.buttonRemoveAll = new Button();
			this.buttonBrowseToAnalysisPackage = new Button();
			this.toolTip1 = new ToolTip(this.components);
			this.exportDatabaseGrid = new ExportDatabaseGrid();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.buttonLoadAnalysisPackage, 6, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonImport, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonAdd, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonRemove, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonSaveAs, 7, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonRemoveAll, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonBrowseToAnalysisPackage, 8, 0);
			this.tableLayoutPanel1.Cursor = Cursors.Default;
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.buttonLoadAnalysisPackage.Image = Resources.ImageOpen;
			componentResourceManager.ApplyResources(this.buttonLoadAnalysisPackage, "buttonLoadAnalysisPackage");
			this.buttonLoadAnalysisPackage.Name = "buttonLoadAnalysisPackage";
			this.toolTip1.SetToolTip(this.buttonLoadAnalysisPackage, componentResourceManager.GetString("buttonLoadAnalysisPackage.ToolTip"));
			this.buttonLoadAnalysisPackage.UseVisualStyleBackColor = true;
			this.buttonLoadAnalysisPackage.Click += new EventHandler(this.buttonLoadAnalysisPackage_Click);
			this.panel1.Controls.Add(this.comboBoxDBSelectionMode);
			componentResourceManager.ApplyResources(this.panel1, "panel1");
			this.panel1.Name = "panel1";
			this.comboBoxDBSelectionMode.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDBSelectionMode.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxDBSelectionMode, "comboBoxDBSelectionMode");
			this.comboBoxDBSelectionMode.Name = "comboBoxDBSelectionMode";
			this.comboBoxDBSelectionMode.SelectedIndexChanged += new EventHandler(this.comboBoxDBSelectionMode_SelectedIndexChanged);
			this.buttonImport.Image = Resources.ImageImport;
			componentResourceManager.ApplyResources(this.buttonImport, "buttonImport");
			this.buttonImport.Name = "buttonImport";
			this.toolTip1.SetToolTip(this.buttonImport, componentResourceManager.GetString("buttonImport.ToolTip"));
			this.buttonImport.UseVisualStyleBackColor = true;
			this.buttonImport.Click += new EventHandler(this.buttonImport_Click);
			this.buttonAdd.Image = Resources.ImagePlus;
			componentResourceManager.ApplyResources(this.buttonAdd, "buttonAdd");
			this.buttonAdd.Name = "buttonAdd";
			this.toolTip1.SetToolTip(this.buttonAdd, componentResourceManager.GetString("buttonAdd.ToolTip"));
			this.buttonAdd.UseVisualStyleBackColor = true;
			this.buttonAdd.Click += new EventHandler(this.buttonAdd_Click);
			componentResourceManager.ApplyResources(this.buttonRemove, "buttonRemove");
			this.buttonRemove.Name = "buttonRemove";
			this.toolTip1.SetToolTip(this.buttonRemove, componentResourceManager.GetString("buttonRemove.ToolTip"));
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
			componentResourceManager.ApplyResources(this.buttonSaveAs, "buttonSaveAs");
			this.buttonSaveAs.Name = "buttonSaveAs";
			this.toolTip1.SetToolTip(this.buttonSaveAs, componentResourceManager.GetString("buttonSaveAs.ToolTip"));
			this.buttonSaveAs.UseVisualStyleBackColor = true;
			this.buttonSaveAs.Click += new EventHandler(this.buttonSaveAs_Click);
			componentResourceManager.ApplyResources(this.buttonRemoveAll, "buttonRemoveAll");
			this.buttonRemoveAll.Image = Resources.ImageClearAllTab;
			this.buttonRemoveAll.Name = "buttonRemoveAll";
			this.toolTip1.SetToolTip(this.buttonRemoveAll, componentResourceManager.GetString("buttonRemoveAll.ToolTip"));
			this.buttonRemoveAll.UseVisualStyleBackColor = true;
			this.buttonRemoveAll.Click += new EventHandler(this.buttonRemoveAll_Click);
			this.buttonBrowseToAnalysisPackage.Image = Resources.ImageFileManager;
			componentResourceManager.ApplyResources(this.buttonBrowseToAnalysisPackage, "buttonBrowseToAnalysisPackage");
			this.buttonBrowseToAnalysisPackage.Name = "buttonBrowseToAnalysisPackage";
			this.toolTip1.SetToolTip(this.buttonBrowseToAnalysisPackage, componentResourceManager.GetString("buttonBrowseToAnalysisPackage.ToolTip"));
			this.buttonBrowseToAnalysisPackage.UseVisualStyleBackColor = true;
			this.buttonBrowseToAnalysisPackage.Click += new EventHandler(this.buttonBrowseToAnalysisPackage_Click);
			this.exportDatabaseGrid.AllowDrop = true;
			componentResourceManager.ApplyResources(this.exportDatabaseGrid, "exportDatabaseGrid");
			this.exportDatabaseGrid.ApplicationDatabaseManager = null;
			this.exportDatabaseGrid.ConfigurationManagerService = null;
			this.exportDatabaseGrid.CurrentLogger = null;
			this.exportDatabaseGrid.ExportDatabaseConfiguration = null;
			this.exportDatabaseGrid.FileConversionParameters = null;
			this.exportDatabaseGrid.ModelValidator = null;
			this.exportDatabaseGrid.Name = "exportDatabaseGrid";
			this.exportDatabaseGrid.SemanticChecker = null;
			this.exportDatabaseGrid.DragDrop += new DragEventHandler(this.DatabaseGrid_DragDrop);
			this.exportDatabaseGrid.DragEnter += new DragEventHandler(this.DatabaseGrid_DragEnter);
			this.AllowDrop = true;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.tableLayoutPanel1);
			base.Controls.Add(this.exportDatabaseGrid);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "ExportDatabases";
			base.DragDrop += new DragEventHandler(this.DatabaseGrid_DragDrop);
			base.DragEnter += new DragEventHandler(this.DatabaseGrid_DragEnter);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
