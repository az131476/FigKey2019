using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Conversion;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.GUI.DatabasesPage;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.CLFExportPage
{
	public class CLFExport : UserControl, IPropertyWindow, IConfigClipboardClient, IFileConversionParametersClient, IUpdateObserver<DatabaseConfiguration>, IUpdateObserver<LoggerType>, IUpdateObserver
	{
		public delegate void ConversionEnabledChangedDelegate(object sender, EventArgs e);

		private FileConversionParameters conversionParameters;

		private IModelValidator modelValidator;

		private DatabaseConfiguration databaseConfiguration;

		private string destinationFolder;

		private FileConversionDestFormat destinationFormat;

		private string destinationFileExtension;

		private string filePrefix;

		private string customfilename;

		private FileConversionFilenameFormat filenameFormat;

		private bool isFolderChanged;

		private string configurationFolderPath;

		private IContainer components;

		private GroupBox groupBoxCurrentFolder;

		private TextBox textBoxCurrentFolderPath;

		private TableLayoutPanel tableLayoutPanelCurrentFolder;

		private CLFExportGrid clfExportGrid;

		private FileConversion fileConversion;

		private FileSystemWatcher folderWatcher;

		private SplitButton splitButtonSelectSourceFolder;

		public event CLFExport.ConversionEnabledChangedDelegate ConversionEnabledChanged;

		public string ConfigurationFolderPath
		{
			get
			{
				return this.configurationFolderPath;
			}
			set
			{
				this.configurationFolderPath = value;
			}
		}

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.modelValidator;
			}
			set
			{
				this.modelValidator = value;
				this.clfExportGrid.LoggerSpecifics = this.modelValidator.LoggerSpecifics;
				this.fileConversion.Init(this.modelValidator.LoggerSpecifics, true, this);
			}
		}

		ISemanticChecker IPropertyWindow.SemanticChecker
		{
			get;
			set;
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
				return PageType.CLFExport;
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
				base.Visible = value;
				if (base.Visible && this.isFolderChanged)
				{
					this.isFolderChanged = false;
					this.DisplayFileList(this.textBoxCurrentFolderPath.Text);
				}
			}
		}

		public IConfigurationManagerService ConfigurationManagerService
		{
			set
			{
				this.fileConversion.ConfigurationManagerService = value;
			}
		}

		public AnalysisPackageParameters AnalysisPackageParameters
		{
			get
			{
				return this.fileConversion.AnalysisPackageParameters;
			}
			set
			{
				this.fileConversion.AnalysisPackageParameters = value;
			}
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		EnumViewType IFileConversionParametersClient.ViewType
		{
			get
			{
				return EnumViewType.ClfExport;
			}
		}

		LoggerType IFileConversionParametersClient.LoggerType
		{
			get
			{
				return this.fileConversion.LoggerSpecifics.Type;
			}
		}

		FileConversionParameters IFileConversionParametersClient.FileConversionParameters
		{
			get
			{
				return this.conversionParameters;
			}
			set
			{
				this.conversionParameters = value;
				this.fileConversion.FileConversionParameters = value;
				this.fileConversion.ExportDatabases.FileConversionParameters = value;
				this.OnFileConversionParametersChanged(this, EventArgs.Empty);
				this.EnableConversionAndSetInfoText();
			}
		}

		bool IFileConversionParametersClient.CanConvert
		{
			get
			{
				return this.fileConversion.IsConversionEnabled;
			}
		}

		public string ConversionTargetFolder
		{
			get
			{
				if (!string.IsNullOrEmpty(this.conversionParameters.DestinationFolder))
				{
					return this.conversionParameters.DestinationFolder;
				}
				return "";
			}
		}

		public List<Vector.VLConfig.Data.ConfigurationDataModel.Action> FocusedActions
		{
			get
			{
				return null;
			}
		}

		public CLFExport()
		{
			this.InitializeComponent();
			this.clfExportGrid.ItemIsEnabledChanged += new EventHandler(this.OnCLFExportGridItemEnabledChanged);
			this.isFolderChanged = false;
			this.groupBoxCurrentFolder.AllowDrop = true;
			this.splitButtonSelectSourceFolder.AutoSize = false;
			this.splitButtonSelectSourceFolder.SplitMenu = new ContextMenu();
			this.fileConversion.ExportDatabases.StatusChanged += new ExportDatabases.StatusChangedHandler(this.ExportDatabasesStatusChanged);
			this.fileConversion.ExportDatabases.HideControlsForCLFExportPage();
			this.UpdateSplitButtonSelectSourceFolder();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return false;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.CLFExport);
			}
			if (this.conversionParameters == null)
			{
				this.conversionParameters = FileConversionHelper.GetDefaultParameters(this.fileConversion.LoggerSpecifics);
				this.conversionParameters.SaveRawFile = false;
				this.conversionParameters.CustomFilename = "{CLF_NAME}F{INDEX}";
			}
			else if (!Directory.Exists(this.conversionParameters.DestinationFolder))
			{
				this.conversionParameters.DestinationFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
			this.fileConversion.FileConversionParameters = this.conversionParameters;
			this.destinationFolder = this.conversionParameters.DestinationFolder;
			this.destinationFormat = this.conversionParameters.DestinationFormat;
			this.destinationFileExtension = FileConversionHelper.GetConfiguredDestinationFormatExtension(this.conversionParameters);
			this.filePrefix = this.conversionParameters.Prefix;
			this.customfilename = this.conversionParameters.CustomFilename;
			this.filenameFormat = this.conversionParameters.FilenameFormat;
			this.clfExportGrid.DisplayedFolderFileList.SetDestinationFolder(this.conversionParameters.DestinationFolder);
			this.clfExportGrid.DisplayedFolderFileList.SetDestinationFileFormat(this.conversionParameters);
			if (this.filenameFormat == FileConversionFilenameFormat.UseCustomName)
			{
				this.clfExportGrid.DisplayedFolderFileList.SetDestinationFilenameFormat(this.filenameFormat, this.customfilename);
			}
			else
			{
				this.clfExportGrid.DisplayedFolderFileList.SetDestinationFilenameFormat(this.filenameFormat, this.filePrefix);
			}
			this.DisplayFileList(this.fileConversion.SourceFolder);
			this.fileConversion.ParametersChanged += new FileConversion.FileConversionParametersChangedHandler(this.OnFileConversionParametersChanged);
			this.fileConversion.StartConversion += new FileConversion.StartConversionHandler(this.OnStartConversion);
			this.fileConversion.IsConversionEnabled = false;
			this.isFolderChanged = false;
			this.fileConversion.ExportDatabases.ModelValidator = this.modelValidator;
			this.fileConversion.ExportDatabases.ApplicationDatabaseManager = this.ApplicationDatabaseManager;
			this.fileConversion.ExportDatabases.FileConversionParameters = this.conversionParameters;
			this.fileConversion.ExportDatabases.SemanticChecker = ((IPropertyWindow)this).SemanticChecker;
			this.fileConversion.ExportDatabases.AnalysisPackageParameters = this.AnalysisPackageParameters;
		}

		void IPropertyWindow.Reset()
		{
		}

		bool IPropertyWindow.ValidateInput()
		{
			this.fileConversion.ExportDatabases.ValidateInput();
			this.EnableConversionAndSetInfoText();
			return true;
		}

		bool IPropertyWindow.HasErrors()
		{
			return false;
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			return false;
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			return false;
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			return false;
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

		public void RefreshView()
		{
			this.DisplayFileList(this.textBoxCurrentFolderPath.Text);
			this.fileConversion.RefreshStatus();
		}

		public void OpenSourceFolder(string path)
		{
			this.DisplayFileList(path);
			GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.ClfSourceFolders, path);
			this.UpdateSplitButtonSelectSourceFolder();
		}

		public void StartConversion()
		{
			this.Convert();
		}

		public void ShowExportDatabaseTab(bool show)
		{
			this.fileConversion.ShowExportDatabaseTab(show);
		}

		public void SetExportDatabaseConfiguration(ExportDatabaseConfiguration config, ExportDatabases exportDatabases)
		{
			this.conversionParameters.ExportDatabaseConfiguration = config;
			this.fileConversion.SetExportDatabaseConfiguration(config, exportDatabases);
		}

		public void FireConversionEnabledChanged()
		{
			if (this.ConversionEnabledChanged != null)
			{
				this.ConversionEnabledChanged(this, new EventArgs());
			}
		}

		private void splitButtonSelectSourceFolder_Enter(object sender, EventArgs e)
		{
			this.UpdateSplitButtonSelectSourceFolder();
		}

		private void CLFExport_VisibleChanged(object sender, EventArgs e)
		{
			if (base.Visible)
			{
				this.UpdateSplitButtonSelectSourceFolder();
				this.fileConversion.ExportDatabases.ValidateInput();
			}
		}

		private void splitButtonSelectSourceFolder_Click(object sender, EventArgs e)
		{
			using (BrowseFolderDialog browseFolderDialog = new BrowseFolderDialog())
			{
				if (GlobalOptionsManager.GetRecentFolders(GlobalOptionsManager.ListSelector.ClfSourceFolders).Count > 0)
				{
					browseFolderDialog.SelectedPath = GlobalOptionsManager.GetRecentFolders(GlobalOptionsManager.ListSelector.ClfSourceFolders)[0];
				}
				else
				{
					browseFolderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				}
				browseFolderDialog.SelectedPath = this.textBoxCurrentFolderPath.Text;
				browseFolderDialog.Title = Resources.SelectSourceFolder;
				if (DialogResult.OK == browseFolderDialog.ShowDialog())
				{
					if (!string.IsNullOrEmpty(browseFolderDialog.SelectedPath) && Directory.Exists(browseFolderDialog.SelectedPath))
					{
						if (!GUIUtil.FolderAccessible(browseFolderDialog.SelectedPath))
						{
							InformMessageBox.Error(Resources.ErrorUnaccessiblePath);
						}
						else
						{
							this.DisplayFileList(browseFolderDialog.SelectedPath);
							GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.ClfSourceFolders, browseFolderDialog.SelectedPath);
							this.UpdateSplitButtonSelectSourceFolder();
						}
					}
				}
			}
		}

		private void splitButtonSelectSourceFolderMenuItem_Click(object sender, EventArgs e)
		{
			if (sender is MenuItem)
			{
				MenuItem menuItem = sender as MenuItem;
				if (menuItem == null)
				{
					return;
				}
				string text = "";
				if (menuItem.Tag is string)
				{
					text = (menuItem.Tag as string);
				}
				bool flag = GlobalOptionsManager.FolderAccessible(GlobalOptionsManager.ListSelector.ClfSourceFolders, text);
				this.UpdateSplitButtonSelectSourceFolder();
				if (flag)
				{
					this.DisplayFileList(text);
					GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.ClfSourceFolders, text);
					this.UpdateSplitButtonSelectSourceFolder();
				}
			}
		}

		private void textBoxCurrentFolderPath_DoubleClick(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.textBoxCurrentFolderPath.Text))
			{
				FileSystemServices.LaunchDirectoryBrowser(this.textBoxCurrentFolderPath.Text);
			}
		}

		public void OnFileConversionParametersChanged(object sender, EventArgs e)
		{
			FileConversionParametersChangedEventArgs fileConversionParametersChangedEventArgs = e as FileConversionParametersChangedEventArgs;
			if (fileConversionParametersChangedEventArgs != null)
			{
				if (fileConversionParametersChangedEventArgs.ChangedParameter == FileConversionParametersChangedEventArgs.Parameter.DestinationFolder)
				{
					this.clfExportGrid.DisplayedFolderFileList.SetDestinationFolder(this.conversionParameters.DestinationFolder);
					this.destinationFolder = this.conversionParameters.DestinationFolder;
					this.clfExportGrid.RefreshGridView();
				}
				else if (fileConversionParametersChangedEventArgs.ChangedParameter == FileConversionParametersChangedEventArgs.Parameter.DestinationFormat)
				{
					this.clfExportGrid.DisplayedFolderFileList.SetDestinationFileFormat(this.conversionParameters);
					this.destinationFormat = this.conversionParameters.DestinationFormat;
					this.clfExportGrid.RefreshGridView();
					this.EnableConversionAndSetInfoText();
				}
				else if (fileConversionParametersChangedEventArgs.ChangedParameter == FileConversionParametersChangedEventArgs.Parameter.DestinationFormatVersion)
				{
					this.EnableConversionAndSetInfoText();
				}
				else if (fileConversionParametersChangedEventArgs.ChangedParameter == FileConversionParametersChangedEventArgs.Parameter.DestinationFormatExtension)
				{
					this.clfExportGrid.DisplayedFolderFileList.SetDestinationFileExtension(this.conversionParameters);
					this.destinationFileExtension = FileConversionHelper.GetConfiguredDestinationFormatExtension(this.conversionParameters);
					this.clfExportGrid.RefreshGridView();
				}
			}
			else if (this.conversionParameters.FilenameFormat != this.filenameFormat || this.conversionParameters.Prefix != this.filePrefix || this.conversionParameters.CustomFilename != this.customfilename)
			{
				this.filenameFormat = this.conversionParameters.FilenameFormat;
				this.filePrefix = this.conversionParameters.Prefix;
				this.customfilename = this.conversionParameters.CustomFilename;
				this.clfExportGrid.DisplayedFolderFileList.SetDestinationFilenameFormat(this.filenameFormat, this.filePrefix);
				this.clfExportGrid.RefreshGridView();
			}
			this.FireConversionEnabledChanged();
		}

		private void OnCLFExportGridItemEnabledChanged(object sender, EventArgs e)
		{
			this.EnableConversionAndSetInfoText();
		}

		private bool EnableConversionAndSetInfoText()
		{
			this.fileConversion.ConversionInfo.Links.Clear();
			if (FileConversionHelper.IsSignalOrientedDestFormat(this.destinationFormat))
			{
				bool allowFlexRay = !FileConversionHelper.UseMDFLegacyConversion(this.conversionParameters);
				if (!this.modelValidator.DatabaseServices.HasDatabasesAccessibleForSignalConversion(this.conversionParameters, allowFlexRay))
				{
					this.fileConversion.ConversionInfo.Text = string.Format(Resources.ErrorUnableToConvertToSignalOrientedFormat, this.destinationFormat.ToString());
					this.fileConversion.IsConversionEnabled = false;
					return false;
				}
			}
			this.fileConversion.ConversionInfo.Text = string.Format(Resources.ConversionInfoCLFExport, this.clfExportGrid.DisplayedFolderFileList.EnabledFiles.Count, this.clfExportGrid.DisplayedFolderFileList.FileList.Count, this.GetTotalFileSizeOfLogFiles(this.clfExportGrid.DisplayedFolderFileList.EnabledFiles));
			this.fileConversion.IsConversionEnabled = (this.clfExportGrid.DisplayedFolderFileList.EnabledFiles.Count > 0);
			this.FireConversionEnabledChanged();
			return true;
		}

		private string GetTotalFileSizeOfLogFiles(IEnumerable<DisplayedFileItem> fileList)
		{
			long num = 0L;
			foreach (DisplayedFileItem current in fileList)
			{
				num += current.FileSize;
			}
			if (num < 1048576L && num > 0L)
			{
				return GUIUtil.GetSizeStringKBForBytes(num);
			}
			return GUIUtil.GetSizeStringMBForBytes(num);
		}

		private void OnStartConversion(object sender, EventArgs e)
		{
			this.Convert();
		}

		private void OnFolderChanged(object sender, FileSystemEventArgs e)
		{
			if (this.clfExportGrid.Visible)
			{
				this.DisplayFileList(this.textBoxCurrentFolderPath.Text);
				return;
			}
			this.isFolderChanged = true;
		}

		private void groupBoxCurrentFolder_DragEnter(object sender, DragEventArgs e)
		{
			if (GUIUtil.GetKindOfDrop(e) != GUIUtil.FileDropContent.IllegalDrop)
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void groupBoxCurrentFolder_DragDrop(object sender, DragEventArgs e)
		{
			GUIUtil.FileDropContent kindOfDrop = GUIUtil.GetKindOfDrop(e);
			if (kindOfDrop == GUIUtil.FileDropContent.IllegalDrop)
			{
				e.Effect = DragDropEffects.None;
				return;
			}
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			string text = array[0];
			if (kindOfDrop != GUIUtil.FileDropContent.Folder)
			{
				text = Path.GetDirectoryName(text);
			}
			if (!GUIUtil.FolderAccessible(text))
			{
				InformMessageBox.Error(Resources.ErrorUnaccessiblePath);
				return;
			}
			this.DisplayFileList(text);
			GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.ClfSourceFolders, text);
			this.UpdateSplitButtonSelectSourceFolder();
		}

		public void ExportDatabasesStatusChanged()
		{
			this.EnableConversionAndSetInfoText();
		}

		void IUpdateObserver<DatabaseConfiguration>.Update(DatabaseConfiguration data)
		{
			this.databaseConfiguration = data;
			this.EnableConversionAndSetInfoText();
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType type)
		{
			if (this.modelValidator.LoggerSpecifics.Type != LoggerType.VN1630log)
			{
				this.fileConversion.LoggerSpecifics = this.modelValidator.LoggerSpecifics;
			}
		}

		private void UpdateSplitButtonSelectSourceFolder()
		{
			this.splitButtonSelectSourceFolder.ShowSplit = false;
			List<string> recentFolders = GlobalOptionsManager.GetRecentFolders(GlobalOptionsManager.ListSelector.ClfSourceFolders);
			if (recentFolders == null)
			{
				return;
			}
			List<string> recentFolders2 = GlobalOptionsManager.GetRecentFolders(GlobalOptionsManager.ListSelector.ClfExportFolders);
			if (recentFolders2 == null)
			{
				return;
			}
			if (recentFolders.Count > 0 || recentFolders2.Count > 0)
			{
				this.splitButtonSelectSourceFolder.SplitMenu.MenuItems.Clear();
				this.splitButtonSelectSourceFolder.ShowSplit = true;
			}
			foreach (string current in recentFolders)
			{
				MenuItem menuItem = new MenuItem();
				string shortenedPath = PathUtil.GetShortenedPath(current, this.Font, GlobalOptionsManager.RecentFolderListWidth, false);
				menuItem.Text = shortenedPath;
				menuItem.Tag = current;
				menuItem.Click += new EventHandler(this.splitButtonSelectSourceFolderMenuItem_Click);
				this.splitButtonSelectSourceFolder.SplitMenu.MenuItems.Add(menuItem);
			}
			if (recentFolders.Count > 0 && recentFolders2.Count > 0)
			{
				this.splitButtonSelectSourceFolder.SplitMenu.MenuItems.Add("-");
			}
			foreach (string current2 in recentFolders2)
			{
				MenuItem menuItem2 = new MenuItem();
				string shortenedPath2 = PathUtil.GetShortenedPath(current2, this.Font, GlobalOptionsManager.RecentFolderListWidth, false);
				menuItem2.Text = shortenedPath2;
				menuItem2.Tag = current2;
				menuItem2.Click += new EventHandler(this.splitButtonSelectSourceFolderMenuItem_Click);
				this.splitButtonSelectSourceFolder.SplitMenu.MenuItems.Add(menuItem2);
			}
		}

		private void DisplayFileList(string sourceFolderPath)
		{
			if (!Directory.Exists(sourceFolderPath))
			{
				sourceFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
			try
			{
				Directory.GetDirectories(sourceFolderPath);
			}
			catch (Exception)
			{
				sourceFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
			this.clfExportGrid.DisplayFileList(sourceFolderPath);
			this.folderWatcher.Path = sourceFolderPath;
			this.textBoxCurrentFolderPath.Text = sourceFolderPath;
			this.fileConversion.SourceFolder = sourceFolderPath;
		}

		private void Convert()
		{
			long num = 0L;
			foreach (DisplayedFileItem current in this.clfExportGrid.DisplayedFolderFileList.EnabledFiles)
			{
				try
				{
					string fileName = Path.Combine(this.clfExportGrid.DisplayedFolderFileList.SourceFolder, current.Filename);
					FileInfo fileInfo = new FileInfo(fileName);
					num += fileInfo.Length;
				}
				catch (Exception)
				{
				}
			}
			if (!this.CheckAvailableSpaceOnDestination(this.conversionParameters.DestinationFormat, num, this.conversionParameters.DestinationFolder, this.conversionParameters.OverwriteDestinationFiles))
			{
				return;
			}
			string path = "convert" + Guid.NewGuid().ToString();
			string text = Path.Combine(this.conversionParameters.DestinationFolder, path);
			try
			{
				Directory.CreateDirectory(text);
			}
			catch (Exception)
			{
				InformMessageBox.Error(Resources.ErrorUnableToWriteInDestFolder);
				return;
			}
			if (FileConversionHelper.PerformChecksForSignalOrientedDestFormat(this.destinationFormat, this.conversionParameters, this))
			{
				if (FileConversionHelper.IsSignalOrientedDestFormat(this.destinationFormat) && FileConversionHelper.UseArxmlToDBCConversion(this.conversionParameters))
				{
					IEnumerable<Database> conversionDatabases = AnalysisPackage.GetConversionDatabases(this.conversionParameters, this.databaseConfiguration.Databases, true);
					ReadOnlyCollection<string> readOnlyCollection;
					if (conversionDatabases != null && conversionDatabases.Any<Database>() && !GenerateDBCfromARXML.Execute(conversionDatabases, this.configurationFolderPath, text, out readOnlyCollection))
					{
						InformMessageBox.Info(Resources.ConversionAborted);
						return;
					}
				}
				CLexport cLexport = new CLexport();
				ProgressIndicatorForm progressIndicatorForm = new ProgressIndicatorForm();
				uint num2 = 0u;
				bool flag = false;
				foreach (DisplayedFileItem current2 in this.clfExportGrid.DisplayedFolderFileList.EnabledFiles)
				{
					string text2 = Path.Combine(this.clfExportGrid.DisplayedFolderFileList.SourceFolder, current2.Filename);
					progressIndicatorForm.Text = string.Format(Resources.TitleConvertFilesCLFExport, num2 + 1u, this.clfExportGrid.DisplayedFolderFileList.EnabledFiles.Count);
					string message;
					if (!cLexport.ExportCLFAsync(text2, this.conversionParameters, text, this.databaseConfiguration, this.ConfigurationFolderPath, out message, progressIndicatorForm, new ProcessExitedDelegate(progressIndicatorForm.ProcessExited), this.modelValidator.LoggerSpecifics, false, null))
					{
						InformMessageBox.Error(message);
						break;
					}
					progressIndicatorForm.ShowDialog();
					if (progressIndicatorForm.Cancelled())
					{
						progressIndicatorForm.Dispose();
						this.FinishConvert(text);
						return;
					}
					if (cLexport.LastExitCode != 0)
					{
						InformMessageBox.Error(cLexport.GetLastGinErrorCodeString());
						break;
					}
					Thread.Sleep(100);
					LoggerDeviceCommon.RenameAfterConvertFromCLF(this.conversionParameters, text, out message, text2);
					string[] files = Directory.GetFiles(text);
					bool flag2 = false;
					string[] array = files;
					for (int i = 0; i < array.Length; i++)
					{
						string path2 = array[i];
						string fileName2 = Path.GetFileName(path2);
						if (!string.IsNullOrEmpty(fileName2) && !fileName2.Equals("RenameHandler.bat") && string.Compare(Path.GetExtension(path2), Vocabulary.FileExtensionDotDBC, true) != 0)
						{
							string text3 = Path.Combine(text, fileName2);
							string text4 = Path.GetFileNameWithoutExtension(fileName2);
							text4 += FileConversionHelper.GetConfiguredDestinationFormatExtension(this.conversionParameters);
							string text5 = Path.Combine(this.conversionParameters.DestinationFolder, text4);
							bool flag3 = false;
							if (File.Exists(text5))
							{
								if (this.conversionParameters.OverwriteDestinationFiles || flag)
								{
									flag3 = true;
								}
								else
								{
									switch (OverwriteFilesConfirmation.Show(text5))
									{
									case OverwriteFilesResult.YesToAll:
										break;
									case OverwriteFilesResult.No:
										try
										{
											File.Delete(text3);
											goto IL_40F;
										}
										catch (Exception)
										{
											goto IL_40F;
										}
										break;
									case OverwriteFilesResult.Abort:
										progressIndicatorForm.Dispose();
										this.FinishConvert(text);
										return;
									default:
										flag3 = true;
										goto Block_30;
									}
									flag = true;
									flag3 = true;
								}
							}
							Block_30:
							try
							{
								if (flag3)
								{
									File.Delete(text5);
								}
								File.Move(text3, text5);
							}
							catch (Exception)
							{
								InformMessageBox.Error(Resources.ErrorUnableToWriteInDestFolder);
								progressIndicatorForm.Dispose();
								this.FinishConvert(text);
								return;
							}
							flag2 = true;
						}
						IL_40F:;
					}
					if (flag2)
					{
						num2 += 1u;
					}
				}
				progressIndicatorForm.Dispose();
				this.FinishConvert(text);
				InformMessageBox.Info(string.Format(Resources.CLFexportNFilesConverted, num2));
				return;
			}
		}

		protected bool CheckAvailableSpaceOnDestination(FileConversionDestFormat destinationFormat, long sizeOfSourceDataInBytes, string destFolderPath, bool isContentOfDestFolderToBeDeleted)
		{
			long num = 0L;
			long num2 = 0L;
			if (FileSystemServices.GetVolumeSizes(destFolderPath, out num2, out num))
			{
				if (isContentOfDestFolderToBeDeleted)
				{
					long num3 = 0L;
					if (FileSystemServices.GetDirectorySize(destFolderPath, out num3))
					{
						num += num3;
					}
				}
				double num4 = 2.0;
				if (destinationFormat == FileConversionDestFormat.ASC)
				{
					num4 = 4.0;
				}
				if (destinationFormat == FileConversionDestFormat.BLF)
				{
					num4 = 1.5;
				}
				if (destinationFormat == FileConversionDestFormat.TXT)
				{
					num4 = 4.0;
				}
				if (destinationFormat == FileConversionDestFormat.XLS)
				{
					num4 = 4.0;
				}
				if ((double)num <= (double)sizeOfSourceDataInBytes * num4)
				{
					long num5 = System.Convert.ToInt64(num4);
					if (InformMessageBox.Question(string.Format(Resources.DestVolHasNotReqDiskSpace, GUIUtil.GetSizeStringMBForBytes(sizeOfSourceDataInBytes * num5)) + " " + Resources.QuestionContinueAnyway) == DialogResult.No)
					{
						return false;
					}
				}
			}
			return true;
		}

		private void FinishConvert(string tempFolderPath)
		{
			try
			{
				File.Delete(Path.Combine(tempFolderPath, "RenameHandler.bat"));
				Directory.Delete(tempFolderPath, true);
			}
			catch (Exception)
			{
			}
			this.DisplayFileList(this.textBoxCurrentFolderPath.Text);
			this.clfExportGrid.RefreshGridView();
			this.fileConversion.RefreshStatus();
			this.EnableConversionAndSetInfoText();
		}

		public bool Serialize(CLFExportPage clfExportPage)
		{
			if (clfExportPage == null)
			{
				return false;
			}
			clfExportPage.FileConversionParameters = this.conversionParameters;
			clfExportPage.SourceFolderPath = this.clfExportGrid.DisplayedFolderFileList.SourceFolder;
			bool flag = true;
			return flag & this.clfExportGrid.Serialize(clfExportPage);
		}

		public bool DeSerialize(CLFExportPage clfExportPage)
		{
			if (clfExportPage == null)
			{
				return false;
			}
			if (clfExportPage.FileConversionParameters != null)
			{
				this.conversionParameters = clfExportPage.FileConversionParameters;
			}
			if (!string.IsNullOrEmpty(clfExportPage.SourceFolderPath))
			{
				this.DisplayFileList(clfExportPage.SourceFolderPath);
			}
			bool flag = true;
			return flag & this.clfExportGrid.DeSerialize(clfExportPage);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CLFExport));
			this.groupBoxCurrentFolder = new GroupBox();
			this.clfExportGrid = new CLFExportGrid();
			this.tableLayoutPanelCurrentFolder = new TableLayoutPanel();
			this.splitButtonSelectSourceFolder = new SplitButton();
			this.textBoxCurrentFolderPath = new TextBox();
			this.folderWatcher = new FileSystemWatcher();
			this.fileConversion = new FileConversion();
			this.groupBoxCurrentFolder.SuspendLayout();
			this.tableLayoutPanelCurrentFolder.SuspendLayout();
			((ISupportInitialize)this.folderWatcher).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxCurrentFolder, "groupBoxCurrentFolder");
			this.groupBoxCurrentFolder.Controls.Add(this.clfExportGrid);
			this.groupBoxCurrentFolder.Controls.Add(this.tableLayoutPanelCurrentFolder);
			this.groupBoxCurrentFolder.Name = "groupBoxCurrentFolder";
			this.groupBoxCurrentFolder.TabStop = false;
			this.groupBoxCurrentFolder.DragDrop += new DragEventHandler(this.groupBoxCurrentFolder_DragDrop);
			this.groupBoxCurrentFolder.DragEnter += new DragEventHandler(this.groupBoxCurrentFolder_DragEnter);
			componentResourceManager.ApplyResources(this.clfExportGrid, "clfExportGrid");
			this.clfExportGrid.LoggerSpecifics = null;
			this.clfExportGrid.Name = "clfExportGrid";
			componentResourceManager.ApplyResources(this.tableLayoutPanelCurrentFolder, "tableLayoutPanelCurrentFolder");
			this.tableLayoutPanelCurrentFolder.Controls.Add(this.splitButtonSelectSourceFolder, 1, 0);
			this.tableLayoutPanelCurrentFolder.Controls.Add(this.textBoxCurrentFolderPath, 0, 0);
			this.tableLayoutPanelCurrentFolder.Name = "tableLayoutPanelCurrentFolder";
			componentResourceManager.ApplyResources(this.splitButtonSelectSourceFolder, "splitButtonSelectSourceFolder");
			this.splitButtonSelectSourceFolder.Name = "splitButtonSelectSourceFolder";
			this.splitButtonSelectSourceFolder.ShowSplitAlways = true;
			this.splitButtonSelectSourceFolder.UseVisualStyleBackColor = true;
			this.splitButtonSelectSourceFolder.Click += new EventHandler(this.splitButtonSelectSourceFolder_Click);
			this.splitButtonSelectSourceFolder.Enter += new EventHandler(this.splitButtonSelectSourceFolder_Enter);
			componentResourceManager.ApplyResources(this.textBoxCurrentFolderPath, "textBoxCurrentFolderPath");
			this.textBoxCurrentFolderPath.Name = "textBoxCurrentFolderPath";
			this.textBoxCurrentFolderPath.ReadOnly = true;
			this.textBoxCurrentFolderPath.DoubleClick += new EventHandler(this.textBoxCurrentFolderPath_DoubleClick);
			this.folderWatcher.EnableRaisingEvents = true;
			this.folderWatcher.SynchronizingObject = this;
			this.folderWatcher.Changed += new FileSystemEventHandler(this.OnFolderChanged);
			this.folderWatcher.Created += new FileSystemEventHandler(this.OnFolderChanged);
			this.folderWatcher.Deleted += new FileSystemEventHandler(this.OnFolderChanged);
			componentResourceManager.ApplyResources(this.fileConversion, "fileConversion");
			this.fileConversion.FileConversionParameters = null;
			this.fileConversion.IsConversionEnabled = true;
			this.fileConversion.Name = "fileConversion";
			this.fileConversion.SourceFolder = null;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.fileConversion);
			base.Controls.Add(this.groupBoxCurrentFolder);
			base.Name = "CLFExport";
			componentResourceManager.ApplyResources(this, "$this");
			base.VisibleChanged += new EventHandler(this.CLFExport_VisibleChanged);
			this.groupBoxCurrentFolder.ResumeLayout(false);
			this.tableLayoutPanelCurrentFolder.ResumeLayout(false);
			this.tableLayoutPanelCurrentFolder.PerformLayout();
			((ISupportInitialize)this.folderWatcher).EndInit();
			base.ResumeLayout(false);
		}
	}
}
