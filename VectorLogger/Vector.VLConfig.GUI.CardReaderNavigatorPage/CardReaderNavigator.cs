using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Conversion;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GeneralUtil.QuickView;
using Vector.VLConfig.GUI.CardReaderPage;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.GUI.Common.QuickView;
using Vector.VLConfig.GUI.DatabasesPage;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LoggingNavigator;
using Vector.VLConfig.LoggingNavigator.Data;
using Vector.VLConfig.LoggingNavigator.Export;
using Vector.VLConfig.LoggingNavigator.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.CardReaderNavigatorPage
{
	public class CardReaderNavigator : UserControl, IUpdateObserver<DatabaseConfiguration>, IUpdateObserver<Vector.VLConfig.LoggerSpecifics.LoggerType>, IUpdateObserver, IPropertyWindow, IConfigClipboardClient, IHardwareFrontendClient, IFileConversionParametersExClient, IFileConversionParametersClient, IQuickViewClient
	{
		public delegate void RefreshViewRequestHandler(object sender, EventArgs e);

		public delegate void EnableAllFileWatchersRequestHandler(object sender, bool isEnabled);

		public delegate void DeviceChangedDelegate(object sender, EventArgs e);

		public delegate void ConversionEnabledChangedDelegate(object sender, EventArgs e);

		public delegate void BatchConversionDestinationChangedDelegate(object sender, EventArgs e);

		public delegate void ClassicModeRequiredDelegate(object sender, string path);

		private IModelValidator modelValidator;

		private FileConversionParameters conversionParameters;

		private string destinationFolder;

		private FileConversionDestFormat destinationFormat;

		private DatabaseConfiguration databaseConfiguration;

		private string configurationFolderPath;

		private ILoggerDevice currentDevice;

		private bool isCurrentDeviceInvalid;

		private string selectedDevicePath;

		private Dictionary<string, LoggerDeviceCommon> drivePathToScannedDevice;

		private ILoggerDevice customDirectoryDevice;

		private INavigationGUI navigator;

		private ILoggerDevice currentActualDevice;

		private ILoggerDevice currentSnapshotDevice;

		private bool isCurrentDeviceCustomDirectory;

		private string batchConversionDestinationPath;

		private bool isConverting;

		private bool isFolderChanged;

		private string deviceWithAnalysisPackage;

		private DeviceListPopupMenuController deviceListPopupMenuController;

		private readonly QuickView quickView;

		private IContainer components;

		private TabControl tabControl;

		private TabPage tabPageNavigator;

		private TabPage tabPageSettings;

		private TableLayoutPanel tableLayoutPanel1;

		private Label labelSourceFolder;

		private Button buttonEject;

		private Button buttonShowInfo;

		private TableLayoutPanel tableLayoutPanel2;

		private SplitButton mSplitButtonConvert;

		private FilemanagerGui fileNavigator;

		private FileConversionSettings fileConversion;

		private ToolTip toolTip;

		private Button buttonDeleteLogData;

		private CheckBox checkBoxDisplay3GSnapshot;

		private Label labelCapacity;

		private SplitButton splitButtonBatchExport;

		private LinkLabel labelConversionInfo;

		private TabPage tabPageChannelMapping;

		private ChannelMapping mChannelMapping;

		private TabPage tabPageExportDatabases;

		private ExportDatabases exportDatabases;

		private Button buttonShowErrorFile;

		private ImageList imageListErrorIcons;

		private DropDownButton dropDownButtonDeviceList;

		private PopupMenu popupMenuDeviceList;

		private BarManager barManagerDeviceList;

		private BarDockControl barDockControlTop;

		private BarDockControl barDockControlBottom;

		private BarDockControl barDockControlLeft;

		private BarDockControl barDockControlRight;

		private SplitButton splitButtonQuickView;

		private Label labelQuickView;

		private StorageCapacityBar storageCapacityBar;

		public event CardReaderNavigator.DeviceChangedDelegate DeviceChanged;

		public event CardReaderNavigator.ClassicModeRequiredDelegate ClassicModeRequired;

		public event CardReaderNavigator.ConversionEnabledChangedDelegate ConversionEnabledChanged;

		public event CardReaderNavigator.BatchConversionDestinationChangedDelegate BatchConversionDestinationChanged;

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.modelValidator;
			}
			set
			{
				this.modelValidator = value;
				this.fileConversion.Init(this.modelValidator.LoggerSpecifics, false);
				this.mChannelMapping.LoggerSpecifics = this.modelValidator.LoggerSpecifics;
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
				return PageType.CardReaderNavigator;
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
				if (value)
				{
					this.splitButtonBatchExport.Visible = (GlobalRuntimeSettings.GetInstance().ActiveApplicationMode == GlobalRuntimeSettings.ApplicationMode.BatchMode);
					if (!visible)
					{
						this.CheckAnalysisPackage();
					}
				}
			}
		}

		public IHardwareFrontend HardwareFrontend
		{
			get;
			set;
		}

		EnumViewType IFileConversionParametersClient.ViewType
		{
			get
			{
				return EnumViewType.Navigator;
			}
		}

		Vector.VLConfig.LoggerSpecifics.LoggerType IFileConversionParametersClient.LoggerType
		{
			get
			{
				if (this.modelValidator == null)
				{
					return Vector.VLConfig.LoggerSpecifics.LoggerType.Unknown;
				}
				return this.modelValidator.LoggerSpecifics.Type;
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
				this.mChannelMapping.FileConversionParameters = value;
				this.exportDatabases.FileConversionParameters = value;
				this.OnFileConversionParametersChanged(this, new FileConversionParametersChangedEventArgs(FileConversionParametersChangedEventArgs.Parameter.All));
				this.EnableConversionAndSetInfoText(false);
			}
		}

		bool IFileConversionParametersClient.CanConvert
		{
			get
			{
				return this.mSplitButtonConvert.Enabled;
			}
		}

		public string ConversionTargetFolder
		{
			get
			{
				if (this.labelConversionInfo.Links.Count > 0)
				{
					return this.labelConversionInfo.Links[0].LinkData.ToString();
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

		public IApplicationDatabaseManager ApplicationDatabaseManager
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

		public string BatchConversionDestinationPath
		{
			get
			{
				return this.batchConversionDestinationPath;
			}
		}

		public IConfigurationManagerService ConfigurationManagerService
		{
			get
			{
				return this.exportDatabases.ConfigurationManagerService;
			}
			set
			{
				this.exportDatabases.ConfigurationManagerService = value;
			}
		}

		public AnalysisPackageParameters AnalysisPackageParameters
		{
			get
			{
				return this.exportDatabases.AnalysisPackageParameters;
			}
			set
			{
				this.exportDatabases.AnalysisPackageParameters = value;
			}
		}

		public SplitButton SplitButtonQuickView
		{
			get
			{
				return this.splitButtonQuickView;
			}
		}

		public ContextMenuStrip ContextMenuStripQuickView
		{
			get
			{
				return this.fileNavigator.ContextMenuStripQuickView;
			}
		}

		public IEnumerable<Control> OtherFeatureControls
		{
			get
			{
				return new List<Control>
				{
					this.labelQuickView
				};
			}
		}

		public ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return this.modelValidator.LoggerSpecifics;
			}
		}

		public string LogDataIniFile2
		{
			get
			{
				ILogFile arg_46_0;
				if (this.currentActualDevice == null)
				{
					arg_46_0 = null;
				}
				else
				{
					arg_46_0 = this.currentActualDevice.LogFileStorage.LogFiles.FirstOrDefault((ILogFile lf) => Constants.LogDataIniFile2Name.Equals(lf.DefaultName, StringComparison.OrdinalIgnoreCase));
				}
				ILogFile logFile = arg_46_0;
				if (logFile == null)
				{
					return string.Empty;
				}
				return logFile.FullFilePath;
			}
		}

		public DatabaseConfiguration DatabaseConfiguration
		{
			get
			{
				return this.databaseConfiguration;
			}
		}

		public IPropertyWindow PropertyWindow
		{
			get
			{
				return this;
			}
		}

		public CardReaderNavigator()
		{
			this.InitializeComponent();
			this.isCurrentDeviceInvalid = true;
			this.customDirectoryDevice = null;
			this.isCurrentDeviceCustomDirectory = false;
			this.selectedDevicePath = "";
			this.navigator = this.fileNavigator.GetIndexNavigation();
			this.drivePathToScannedDevice = new Dictionary<string, LoggerDeviceCommon>();
			this.exportDatabases.StatusChanged += new ExportDatabases.StatusChangedHandler(this.ExportDatabasesStatusChanged);
			this.exportDatabases.FileConversionParametersChanged += new FileConversion.FileConversionParametersChangedHandler(this.OnFileConversionParametersChanged);
			this.deviceListPopupMenuController = new DeviceListPopupMenuController(this.dropDownButtonDeviceList, this.barManagerDeviceList, this.popupMenuDeviceList, true, 5u, 5u);
			this.deviceListPopupMenuController.SelectedDeviceChanged += new EventHandler(this.OnSelectedDeviceChanged);
			this.deviceListPopupMenuController.DeviceSelectableOnlyForClassicView += new DeviceListPopupMenuController.DeviceSelectableOnlyForClassicViewDelegate(this.OnDeviceSelectableOnlyForClassicView);
			this.InitSplitButtonConvert();
			this.isConverting = false;
			this.isFolderChanged = false;
			this.deviceWithAnalysisPackage = string.Empty;
			this.exportDatabases.Validating += new EventHandler(this.OnExportDatabaseValidating);
			this.imageListErrorIcons.Images.Add(Resources.IconError.ToBitmap());
			this.imageListErrorIcons.Images.Add(Resources.ImageWarning);
			this.imageListErrorIcons.Images.Add(Resources.ImageOK);
			this.imageListErrorIcons.Images.Add(Resources.ImageOKLock);
			this.quickView = QuickView.Create(this, this, true);
			this.ResetCapacityValues();
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return false;
		}

		void IPropertyWindow.Init()
		{
			this.ResetSnapshotButton();
			this.ResetCapacityValues();
			this.buttonDeleteLogData.Enabled = false;
			this.buttonEject.Enabled = false;
			this.buttonShowInfo.Enabled = false;
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.CardReaderNavigator);
			}
			if (this.conversionParameters == null)
			{
				this.conversionParameters = FileConversionHelper.GetDefaultParameters(this.modelValidator.LoggerSpecifics);
			}
			else if (!Directory.Exists(this.conversionParameters.DestinationFolder))
			{
				this.conversionParameters.DestinationFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
			this.fileConversion.FileConversionParameters = this.conversionParameters;
			this.mChannelMapping.FileConversionParameters = this.conversionParameters;
			this.destinationFolder = this.conversionParameters.DestinationFolder;
			this.destinationFormat = this.conversionParameters.DestinationFormat;
			this.ApplyLoggerSpecifics(this.modelValidator.LoggerSpecifics);
			this.fileConversion.ParametersChanged += new FileConversionSettings.FileConversionParametersChangedHandler(this.OnFileConversionParametersChanged);
			this.navigator.IsValidChanged += new EventHandler(this.OnFileNavigatorChanged);
			this.deviceListPopupMenuController.Init(this.modelValidator, this.HardwareFrontend.IsScanForAllLoggerTypesEnabled);
			this.RestoreSelectionFromAppDataSettings();
			this.splitButtonBatchExport.Visible = (GlobalRuntimeSettings.GetInstance().ActiveApplicationMode == GlobalRuntimeSettings.ApplicationMode.BatchMode);
			this.exportDatabases.ModelValidator = this.modelValidator;
			this.exportDatabases.ApplicationDatabaseManager = this.ApplicationDatabaseManager;
			this.exportDatabases.FileConversionParameters = this.conversionParameters;
			this.exportDatabases.SemanticChecker = ((IPropertyWindow)this).SemanticChecker;
			this.exportDatabases.AnalysisPackageParameters = this.AnalysisPackageParameters;
			this.exportDatabases.ValidateInput();
		}

		void IPropertyWindow.Reset()
		{
		}

		bool IPropertyWindow.ValidateInput()
		{
			if (this.tabControl.SelectedTab == this.tabPageExportDatabases)
			{
				this.exportDatabases.ValidateInput();
			}
			this.EnableConversionAndSetInfoText(false);
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

		public void DeviceUpdated(ILoggerDevice device)
		{
			if (this.CurrentDevice != device)
			{
				return;
			}
			if (this.isConverting)
			{
				this.isFolderChanged = true;
				return;
			}
			this.ApplyCurrentDeviceToControls();
		}

		public void DevicesAdded(IList<ILoggerDevice> devices)
		{
			foreach (ILoggerDevice current in devices)
			{
				if (current.HasIndexFile && !this.drivePathToScannedDevice.ContainsKey(current.HardwareKey))
				{
					this.drivePathToScannedDevice[current.HardwareKey] = (current as LoggerDeviceCommon);
					if (current.IsOnline)
					{
						this.deviceListPopupMenuController.AddOnlineDevice(current.HardwareKey, current.LoggerType);
					}
					else
					{
						this.deviceListPopupMenuController.AddMemoryCardDevice(current.HardwareKey, current.LoggerType);
					}
				}
			}
		}

		public void DevicesRemoved(IList<ILoggerDevice> devices)
		{
			foreach (ILoggerDevice current in devices)
			{
				if (this.drivePathToScannedDevice.ContainsKey(current.HardwareKey))
				{
					this.drivePathToScannedDevice.Remove(current.HardwareKey);
					this.deviceListPopupMenuController.RemoveDevice(current.HardwareKey);
				}
				if (current == this.customDirectoryDevice)
				{
					this.customDirectoryDevice = null;
				}
			}
		}

		public void AdditionalDrivesListChanged(IList<string> addtionalDrivesList)
		{
			this.deviceListPopupMenuController.AdditionalDrives.Clear();
			foreach (string current in addtionalDrivesList)
			{
				this.deviceListPopupMenuController.AdditionalDrives.Add(current);
			}
		}

		IEnumerable<string> IFileConversionParametersExClient.GetMarkerTypeSelection()
		{
			return this.fileNavigator.GetMarkerTypeSelection();
		}

		IEnumerable<string> IFileConversionParametersExClient.RestoreMarkerTypeSelection(IEnumerable<string> markers)
		{
			return this.fileNavigator.RestoreMarkerTypeSelection(markers);
		}

		IEnumerable<string> IFileConversionParametersExClient.GetTriggerTypeSelection()
		{
			return this.fileNavigator.GetTriggerTypeSelection();
		}

		IEnumerable<string> IFileConversionParametersExClient.RestoreTriggerTypeSelection(IEnumerable<string> triggers)
		{
			return this.fileNavigator.RestoreTriggerTypeSelection(triggers);
		}

		void IUpdateObserver<DatabaseConfiguration>.Update(DatabaseConfiguration data)
		{
			this.databaseConfiguration = data;
			this.EnableConversionAndSetInfoText(false);
		}

		void IUpdateObserver<Vector.VLConfig.LoggerSpecifics.LoggerType>.Update(Vector.VLConfig.LoggerSpecifics.LoggerType type)
		{
			this.ApplyLoggerSpecifics(this.modelValidator.LoggerSpecifics);
			this.deviceListPopupMenuController.Init(this.modelValidator, this.HardwareFrontend.IsScanForAllLoggerTypesEnabled);
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

		public void FireDeviceChanged()
		{
			if (this.DeviceChanged != null)
			{
				this.DeviceChanged(this, new EventArgs());
			}
		}

		public void FireClassicModeRequired(string path)
		{
			if (this.ClassicModeRequired != null)
			{
				this.ClassicModeRequired(this, path);
			}
		}

		public void FireConversionEnabledChanged()
		{
			if (this.ConversionEnabledChanged != null)
			{
				this.ConversionEnabledChanged(this, new EventArgs());
			}
		}

		public void BatchFireConversionDestinationChanged()
		{
			if (this.BatchConversionDestinationChanged != null)
			{
				this.BatchConversionDestinationChanged(this, new EventArgs());
			}
		}

		public void ShowDeviceInfo()
		{
			CardReaderDeviceInformation.Display(this.currentDevice);
		}

		public void StartConversion()
		{
			this.SplitButtonConvert_Click(this, EventArgs.Empty);
		}

		public void ShowExportDatabaseTab(bool show)
		{
			this.exportDatabases.Visible = false;
			this.tabControl.TabPages.Remove(this.tabPageExportDatabases);
		}

		public void SetExportDatabaseConfiguration(ExportDatabaseConfiguration config, ExportDatabases exportDatabases)
		{
			this.conversionParameters.ExportDatabaseConfiguration = config;
			this.exportDatabases = exportDatabases;
		}

		public void RefreshView()
		{
			if (((IPropertyWindow)this).IsVisible)
			{
				this.exportDatabases.ClearAutomaticAnalysisPackage();
				this.CheckAnalysisPackage();
			}
		}

		private void OnSelectedDeviceChanged(object sender, EventArgs e)
		{
			string text = this.deviceListPopupMenuController.SelectedDevicePath;
			this.selectedDevicePath = text;
			if (string.IsNullOrEmpty(text))
			{
				this.currentDevice = null;
				this.currentActualDevice = null;
			}
			else if (this.drivePathToScannedDevice.ContainsKey(text))
			{
				LoggerDeviceCommon loggerDeviceCommon = this.drivePathToScannedDevice[text];
				if (this.currentDevice == loggerDeviceCommon)
				{
					this.checkBoxDisplay3GSnapshot.Visible = loggerDeviceCommon.HasSnapshotFolderContainingLogData;
					return;
				}
				this.currentDevice = loggerDeviceCommon;
				this.isCurrentDeviceCustomDirectory = false;
			}
			else if (this.deviceListPopupMenuController.AdditionalDrives.Contains(text))
			{
				ILoggerSpecifics loggerSpecifics = this.modelValidator.LoggerSpecifics;
				if (this.HardwareFrontend.IsScanForAllLoggerTypesEnabled)
				{
					loggerSpecifics = MlRtIniFile.LoadLoggerTypeFromMemoryCardContent(text);
				}
				this.currentDevice = this.HardwareFrontend.CreateCustomLoggerDevice(loggerSpecifics.Type, text, this);
				this.isCurrentDeviceCustomDirectory = false;
			}
			else
			{
				ILoggerSpecifics loggerSpecifics2 = this.modelValidator.LoggerSpecifics;
				if (this.HardwareFrontend.IsScanForAllLoggerTypesEnabled)
				{
					loggerSpecifics2 = MlRtIniFile.LoadLoggerTypeFromMemoryCardContent(text);
				}
				this.customDirectoryDevice = this.HardwareFrontend.CreateCustomLoggerDevice(loggerSpecifics2.Type, text, this);
				this.currentDevice = this.customDirectoryDevice;
				this.isCurrentDeviceCustomDirectory = true;
			}
			if (this.isCurrentDeviceCustomDirectory)
			{
				this.HardwareFrontend.ReleaseCustomLoggerDevice(this);
			}
			if (this.currentDevice != null)
			{
				this.ApplyLoggerSpecifics(this.currentDevice.LoggerSpecifics);
			}
			bool flag = true;
			if (this.currentActualDevice != null && this.currentDevice != null && this.currentActualDevice.HardwareKey == this.currentDevice.HardwareKey)
			{
				flag = false;
			}
			this.currentActualDevice = this.currentDevice;
			if (flag)
			{
				this.ResetSnapshotButton();
				this.currentSnapshotDevice = null;
			}
			this.checkBoxDisplay3GSnapshot.Visible = false;
			if (this.currentActualDevice != null && this.currentActualDevice.HasSnapshotFolderContainingLogData && Directory.Exists(this.currentActualDevice.SnapshotFolderPath))
			{
				this.checkBoxDisplay3GSnapshot.Visible = true;
				if (this.checkBoxDisplay3GSnapshot.Checked)
				{
					this.currentDevice = this.GetOrCreateSnapshotDevice(this.currentActualDevice.SnapshotFolderPath);
					this.tabPageNavigator.Text = Resources.NavigatorTabLabel + Resources.SnapshotDataTitlePostfix;
				}
				else
				{
					this.tabPageNavigator.Text = Resources.NavigatorTabLabel;
				}
			}
			else
			{
				this.ResetSnapshotButton();
				this.currentSnapshotDevice = null;
				this.tabPageNavigator.Text = Resources.NavigatorTabLabel;
			}
			this.ApplyCurrentDeviceToControls();
			if (((IPropertyWindow)this).IsVisible)
			{
				this.CheckAnalysisPackage();
			}
		}

		private void OnDeviceSelectableOnlyForClassicView(object sender, string selectedPath)
		{
			if (this.HardwareFrontend.IsScanForAllLoggerTypesEnabled)
			{
				this.FireClassicModeRequired(selectedPath);
			}
		}

		private void buttonEject_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.deviceListPopupMenuController.SelectedDevicePath) && !this.isCurrentDeviceInvalid && this.drivePathToScannedDevice.ContainsKey(this.deviceListPopupMenuController.SelectedDevicePath))
			{
				this.HardwareFrontend.Eject(this.drivePathToScannedDevice[this.deviceListPopupMenuController.SelectedDevicePath]);
			}
		}

		private void buttonEject_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.buttonEject, Resources.EjectDevice);
		}

		private void buttonShowInfo_Click(object sender, EventArgs e)
		{
			this.ShowDeviceInfo();
		}

		private void buttonShowInfo_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.buttonShowInfo, Resources.DisplayDeviceInformation);
		}

		public void OnFileConversionParametersChanged(object sender, EventArgs e)
		{
			FileConversionParametersChangedEventArgs fileConversionParametersChangedEventArgs = e as FileConversionParametersChangedEventArgs;
			if (fileConversionParametersChangedEventArgs != null)
			{
				if (fileConversionParametersChangedEventArgs.ChangedParameter == FileConversionParametersChangedEventArgs.Parameter.DestinationFormat)
				{
					this.destinationFormat = this.conversionParameters.DestinationFormat;
					if (this.destinationFormat != this.exportDatabases.FileConversionParameters.DestinationFormat)
					{
						this.exportDatabases.FileConversionParameters.DestinationFormat = this.destinationFormat;
					}
					this.CheckAnalysisPackage();
				}
				else if (fileConversionParametersChangedEventArgs.ChangedParameter == FileConversionParametersChangedEventArgs.Parameter.DestinationFormatVersion)
				{
					this.CheckAnalysisPackage();
				}
				else if (fileConversionParametersChangedEventArgs.ChangedParameter == FileConversionParametersChangedEventArgs.Parameter.DestinationFolder)
				{
					this.destinationFolder = this.conversionParameters.DestinationFolder;
					this.EnableConversionAndSetInfoText(false);
				}
				else if (fileConversionParametersChangedEventArgs.ChangedParameter == FileConversionParametersChangedEventArgs.Parameter.All)
				{
					this.destinationFormat = this.conversionParameters.DestinationFormat;
					if (this.destinationFormat != this.exportDatabases.FileConversionParameters.DestinationFormat)
					{
						this.exportDatabases.FileConversionParameters.DestinationFormat = this.destinationFormat;
					}
					this.destinationFolder = this.conversionParameters.DestinationFolder;
					this.CheckAnalysisPackage();
				}
			}
			else if (sender is ExportDatabases)
			{
				this.CheckAnalysisPackage();
			}
			this.FireConversionEnabledChanged();
		}

		private void OnFileNavigatorChanged(object sender, EventArgs e)
		{
			this.EnableConversionAndSetInfoText(false);
		}

		private void fileNavigator_Click(object sender, EventArgs e)
		{
			this.EnableConversionAndSetInfoText(false);
		}

		private void checkBoxDisplay3GSnapshot_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox == null)
			{
				return;
			}
			bool flag = false;
			if (checkBox.Checked)
			{
				if (this.currentActualDevice.HasSnapshotFolderContainingLogData)
				{
					this.currentDevice = this.GetOrCreateSnapshotDevice(this.currentDevice.SnapshotFolderPath);
				}
				else
				{
					InformMessageBox.Error(Resources.ErrorSnapshotNotAvailable);
					this.currentDevice = this.currentActualDevice;
					flag = true;
					this.checkBoxDisplay3GSnapshot.Visible = false;
				}
			}
			else
			{
				this.currentDevice = this.currentActualDevice;
				flag = true;
			}
			if (flag)
			{
				if (this.HardwareFrontend.IsCustomLoggerDevice(this.currentDevice))
				{
					this.HardwareFrontend.SetCustomLoggerDevice(this.currentDevice, this);
				}
				else
				{
					this.HardwareFrontend.ReleaseCustomLoggerDevice(this);
				}
			}
			this.ApplyCurrentDeviceToControls();
			this.fileConversion.RefreshStatus();
		}

		private void checkBoxDisplay3GSnapshot_MouseEnter(object sender, EventArgs e)
		{
			if (!this.checkBoxDisplay3GSnapshot.Checked)
			{
				this.toolTip.SetToolTip(this.checkBoxDisplay3GSnapshot, Resources.SwitchToSnapshotLogData);
				return;
			}
			this.toolTip.SetToolTip(this.checkBoxDisplay3GSnapshot, Resources.SwitchToMainLogData);
		}

		public void ButtonBatchExport(object sender, EventArgs e)
		{
			this.splitButtonBatchExport_Click(sender, e);
		}

		private void splitButtonBatchExport_Click(object sender, EventArgs e)
		{
			if (!this.navigator.IsValid)
			{
				return;
			}
			string pathToExportBatchFile = "";
			if (sender is ToolStripMenuItem && (sender as ToolStripMenuItem).Tag is string)
			{
				pathToExportBatchFile = ((sender as ToolStripMenuItem).Tag as string);
			}
			else
			{
				pathToExportBatchFile = GlobalRuntimeSettings.GetInstance().BatchModePath;
			}
			ConversionJob conversionJob = new ConversionJob("allInOne", ConversionJobType.File, 0u);
			conversionJob.FileConversionParameters = this.conversionParameters;
			IList<ExportJob> list;
			if (this.navigator.GetExportJobs(out list))
			{
				foreach (ExportJob current in list)
				{
					foreach (Vector.VLConfig.LoggingNavigator.Data.LogFile current2 in current.LogFileList)
					{
						if (!this.currentDevice.LogFileStorage.IsPrimaryFileGroupCompressed)
						{
							conversionJob.SelectedFileNames.Add(current2.Name);
						}
						else
						{
							conversionJob.SelectedFileNames.Add(current2.Name + Vocabulary.FileExtensionDotGZ);
						}
					}
					foreach (string current3 in current.IndexFileList)
					{
						conversionJob.SelectedFileNames.Add(current3);
					}
				}
				if (this.currentDevice.LogFileStorage.NumberOfDriveRecorderFiles > 0u)
				{
					IList<string> driveRecorderFileNamesOfPrimaryGroup = this.currentDevice.LogFileStorage.GetDriveRecorderFileNamesOfPrimaryGroup();
					foreach (string current4 in driveRecorderFileNamesOfPrimaryGroup)
					{
						conversionJob.SelectedFileNames.Add(Path.GetFileName(current4));
					}
				}
			}
			this.currentDevice.LogFileStorage.CopyAndBatchExportSelectedLogFiles(this.conversionParameters, conversionJob, pathToExportBatchFile, ref this.batchConversionDestinationPath);
			this.BatchFireConversionDestinationChanged();
		}

		private string PrintExportJobList(IList<ExportJob> jobs, string title)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(title);
			foreach (ExportJob current in jobs)
			{
				stringBuilder.AppendFormat(current.Name + ":", new object[0]);
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (Vector.VLConfig.LoggingNavigator.Data.LogFile current2 in current.LogFileList)
				{
					stringBuilder2.Append(current2.Name + ", ");
				}
				if (stringBuilder2.Length > 2)
				{
					stringBuilder2.Length -= 2;
				}
				stringBuilder.AppendLine(stringBuilder2.ToString());
			}
			return stringBuilder.ToString();
		}

		private void buttonDeleteLogData_Click(object sender, EventArgs e)
		{
			if (this.currentDevice != null)
			{
				bool flag = false;
				if (this.CurrentDevice.HasSnapshotFolderContainingLogData)
				{
					DialogResult dialogResult = InformMessageBox.QuestionWithCancel(Resources.QuestionSnapshotFoundOnMediaDeleteToo);
					if (dialogResult == DialogResult.Yes)
					{
						if (!FileSystemServices.TryDeleteDirectory(this.CurrentDevice.SnapshotFolderPath))
						{
							InformMessageBox.Error(string.Format(Resources.CannotDeleteDirectory, this.CurrentDevice.SnapshotFolderPath));
						}
					}
					else if (dialogResult == DialogResult.Cancel)
					{
						return;
					}
				}
				else if (this.currentSnapshotDevice == this.CurrentDevice)
				{
					flag = true;
				}
				this.currentDevice.LogFileStorage.DeleteAllLogFiles();
				if (flag)
				{
					this.checkBoxDisplay3GSnapshot.Checked = false;
				}
			}
		}

		private void buttonDeleteLogData_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.buttonDeleteLogData, Resources.DeleteLogData);
		}

		private void buttonShowErrorFile_Click(object sender, EventArgs e)
		{
			this.ShowErrorFile();
		}

		private void buttonShowErrorFile_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.buttonShowErrorFile, string.Format(Resources.ErrorFileFoundClickToOpen, Path.GetFileName(this.modelValidator.LoggerSpecifics.DataStorage.ErrorFilePath)));
		}

		private void tabPageNavigator_DragEnter(object sender, DragEventArgs e)
		{
			if (GUIUtil.GetKindOfDrop(e) != GUIUtil.FileDropContent.IllegalDrop)
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void tabPageNavigator_DragDrop(object sender, DragEventArgs e)
		{
			GUIUtil.FileDropContent kindOfDrop = GUIUtil.GetKindOfDrop(e);
			if (kindOfDrop == GUIUtil.FileDropContent.IllegalDrop)
			{
				e.Effect = DragDropEffects.None;
				return;
			}
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			string path = array[0];
			if (kindOfDrop != GUIUtil.FileDropContent.Folder)
			{
				path = Path.GetDirectoryName(path);
			}
			if (GUIUtil.FolderAccessible(path))
			{
				if (!this.deviceListPopupMenuController.SelectDevice(path))
				{
					this.deviceListPopupMenuController.AddCustomFolderOrPackedImageDevice(path);
				}
				return;
			}
			if (GUIUtil.IsZipFile(path))
			{
				InformMessageBox.Error(Resources.ErrorUnaccessibleZipFile);
				return;
			}
			InformMessageBox.Error(Resources.ErrorUnaccessiblePath);
		}

		private void labelConversionInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string text = e.Link.LinkData.ToString();
			if (Directory.Exists(text))
			{
				FileSystemServices.LaunchDirectoryBrowser(text);
				return;
			}
			InformMessageBox.Error(string.Format(Resources.ErrorFolderNotFound, text));
		}

		private void SplitMenuStripConvert_Opening(object sender, CancelEventArgs e)
		{
			FileConversionProfileManager.InitDropDownItems(this.mSplitButtonConvert.SplitMenuStrip.Items, this);
		}

		private void SplitMenuStripConvert_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (FileConversionProfileManager.OnDropDownItemClicked(e, this, true))
			{
				this.SplitButtonConvert_Click(this, EventArgs.Empty);
			}
		}

		private void SplitButtonConvert_Click(object sender, EventArgs e)
		{
			if (!this.navigator.IsValid)
			{
				return;
			}
			if (this.fileConversion.HasError)
			{
				if (this.tabControl.SelectedTab != this.tabPageSettings)
				{
					this.tabControl.SelectTab(this.tabPageSettings);
				}
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				return;
			}
			if (this.mChannelMapping.HasError)
			{
				if (this.tabControl.SelectedTab != this.tabPageChannelMapping)
				{
					this.tabControl.SelectTab(this.tabPageChannelMapping);
				}
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				return;
			}
			if (!FileConversionHelper.PerformChecksForSignalOrientedDestFormat(this.destinationFormat, this.conversionParameters, this))
			{
				return;
			}
			List<ConversionJob> conversionJobs;
			if (NavigatorHelper.GetConversionJobs(this.currentDevice, this.navigator, this.conversionParameters, out conversionJobs))
			{
				this.isConverting = true;
				this.currentDevice.LogFileStorage.ConvertSelectedLogFiles(this.conversionParameters, conversionJobs, this.databaseConfiguration, this.configurationFolderPath);
				this.isConverting = false;
				if (this.isFolderChanged)
				{
					this.isFolderChanged = false;
					this.ApplyCurrentDeviceToControls();
				}
			}
			this.EnableConversionAndSetInfoText(false);
		}

		public void ExportDatabasesStatusChanged()
		{
			this.EnableConversionAndSetInfoText(false);
		}

		private void ApplyLoggerSpecifics(ILoggerSpecifics loggerSpecifics)
		{
			if (this.HardwareFrontend.IsScanForAllLoggerTypesEnabled && this.modelValidator.LoggerSpecifics.Type != loggerSpecifics.Type)
			{
				this.modelValidator.LoggerSpecifics = loggerSpecifics;
			}
			this.fileConversion.LoggerSpecifics = this.modelValidator.LoggerSpecifics;
			this.destinationFolder = this.conversionParameters.DestinationFolder;
			this.destinationFormat = this.conversionParameters.DestinationFormat;
			NavigatorHelper.SetFileNavigatorLoggerType(this.navigator, loggerSpecifics.Type);
			this.mChannelMapping.LoggerSpecifics = this.modelValidator.LoggerSpecifics;
			this.fileConversion.RefreshStatus();
			this.exportDatabases.ValidateInput();
			this.UpdateShowErrorFileButton();
			if (this.modelValidator.LoggerSpecifics.FileConversion.HasChannelMapping && !this.tabControl.TabPages.Contains(this.tabPageChannelMapping))
			{
				this.tabControl.TabPages.Add(this.tabPageChannelMapping);
			}
			else if (!this.modelValidator.LoggerSpecifics.FileConversion.HasChannelMapping && this.tabControl.TabPages.Contains(this.tabPageChannelMapping))
			{
				this.tabControl.TabPages.Remove(this.tabPageChannelMapping);
			}
			this.quickView.ApplyLoggerSpecifics();
		}

		private void RestoreSelectionFromAppDataSettings()
		{
			if (this.deviceListPopupMenuController.IsPathAvailableAndSelectable(this.selectedDevicePath))
			{
				this.deviceListPopupMenuController.SelectDevice(this.selectedDevicePath);
			}
		}

		public void OpenSourceFolder(string folderPath, bool addToRecentlyUsedList = true)
		{
			if (!this.deviceListPopupMenuController.SelectDevice(folderPath))
			{
				this.deviceListPopupMenuController.AddCustomFolderOrPackedImageDevice(folderPath);
			}
		}

		private void ApplyCurrentDeviceToControls()
		{
			this.buttonEject.Enabled = false;
			this.buttonShowInfo.Enabled = false;
			this.fileNavigator.Enabled = false;
			this.buttonDeleteLogData.Enabled = false;
			this.isCurrentDeviceInvalid = true;
			this.buttonShowErrorFile.Visible = false;
			this.ResetCapacityValues();
			if (this.currentDevice != null)
			{
				NavigatorHelper.OpenIndexFilesFromSource(this.fileNavigator, this.currentDevice, this.navigator, this);
				this.UpdateShowErrorFileButton();
				this.isCurrentDeviceInvalid = false;
				this.buttonShowInfo.Enabled = true;
				this.buttonEject.Enabled = !this.HardwareFrontend.IsCustomLoggerDevice(this.currentDevice);
				if (this.currentDevice.HasLoggerInfo)
				{
					long numberOfFiles = (long)((ulong)(this.currentDevice.LogFileStorage.NumberOfTriggeredBuffers + this.currentDevice.LogFileStorage.NumberOfRecordingBuffers + this.currentDevice.LogFileStorage.NumberOfDriveRecorderFiles + this.currentDevice.LogFileStorage.NumberOfJpegFiles + this.currentDevice.LogFileStorage.NumberOfWavFiles + this.currentDevice.LogFileStorage.NumberOfZipArchives));
					this.UpdateCapacityInformation(numberOfFiles, this.currentDevice.LogFileStorage.FreeSpace, this.currentDevice.LogFileStorage.TotalSpace);
				}
				else
				{
					long capacity = 0L;
					long freeSpace = 0L;
					long numberOfFiles2 = 0L;
					if (this.modelValidator.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
					{
						try
						{
							DriveInfo driveInfo = new DriveInfo(this.currentDevice.HardwareKey);
							DirectoryInfo directoryInfo = new DirectoryInfo(this.currentDevice.HardwareKey);
							capacity = driveInfo.TotalSize;
							freeSpace = driveInfo.TotalFreeSpace;
							numberOfFiles2 = (long)directoryInfo.GetFiles().Count<FileInfo>();
						}
						catch (Exception)
						{
						}
					}
					this.UpdateCapacityInformation(numberOfFiles2, freeSpace, capacity);
				}
				this.buttonDeleteLogData.Enabled = (this.currentDevice.HasIndexFile && !this.isCurrentDeviceCustomDirectory);
			}
			else
			{
				this.ResetSnapshotButton();
				this.navigator.Close();
			}
			this.EnableConversionAndSetInfoText(false);
			this.FireDeviceChanged();
		}

		private bool EnableConversionAndSetInfoText(bool analysisPackageNotFound = false)
		{
			if (this.isCurrentDeviceInvalid)
			{
				this.labelConversionInfo.Text = Resources.ConversionInfoNoDataAvailable;
				this.labelConversionInfo.Links.Clear();
				this.mSplitButtonConvert.Enabled = false;
				this.splitButtonQuickView.Enabled = false;
				this.EnableBatchExport(false);
				this.FireConversionEnabledChanged();
				this.UpdateTabErrorIcons();
				return false;
			}
			if (!this.currentDevice.HasIndexFile)
			{
				this.labelConversionInfo.Text = Resources.ErrorUnableToConvertNoNaviFilesOnCard;
				this.labelConversionInfo.Links.Clear();
				this.mSplitButtonConvert.Enabled = false;
				this.splitButtonQuickView.Enabled = false;
				this.EnableBatchExport(false);
				this.FireConversionEnabledChanged();
				this.UpdateTabErrorIcons();
				return false;
			}
			if ((this.currentDevice.LogFileStorage.NumberOfTriggeredBuffers > 0u || this.currentDevice.LogFileStorage.NumberOfRecordingBuffers > 0u) && !string.IsNullOrEmpty(this.currentDevice.LogFileStorage.DestSubFolderNamePrimary) && FileConversionHelper.IsSignalOrientedDestFormat(this.destinationFormat))
			{
				if (analysisPackageNotFound)
				{
					this.labelConversionInfo.Text = Resources.AutomaticSearchNoMatchingPackageFound;
					this.labelConversionInfo.Links.Clear();
					this.mSplitButtonConvert.Enabled = false;
					this.EnableBatchExport(false);
					this.FireConversionEnabledChanged();
					this.UpdateTabErrorIcons();
					return false;
				}
				bool allowFlexRay = !FileConversionHelper.UseMDFLegacyConversion(this.conversionParameters);
				if (!this.modelValidator.DatabaseServices.HasDatabasesAccessibleForSignalConversion(this.conversionParameters, allowFlexRay))
				{
					this.labelConversionInfo.Text = string.Format(Resources.ErrorUnableToConvertToSignalOrientedFormat, this.destinationFormat.ToString());
					this.labelConversionInfo.Links.Clear();
					this.mSplitButtonConvert.Enabled = false;
					this.EnableBatchExport(false);
					this.FireConversionEnabledChanged();
					this.UpdateTabErrorIcons();
					return false;
				}
			}
			IList<ExportJob> list = null;
			if (!this.navigator.IsValid || !this.navigator.GetExportJobs(out list))
			{
				this.labelConversionInfo.Text = Resources.NoElementsSelected;
				this.labelConversionInfo.Links.Clear();
				this.mSplitButtonConvert.Enabled = false;
				this.EnableBatchExport(false);
				this.FireConversionEnabledChanged();
				this.UpdateTabErrorIcons();
				return false;
			}
			string text;
			switch (this.navigator.GetExportType())
			{
			case ExportType.Measurements:
				text = string.Format(Resources.NumMeasureConversionsSelected, list.Count);
				break;
			case ExportType.Marker:
				text = string.Format(Resources.NumMarkerConversionsSelected, list.Count);
				break;
			case ExportType.Files:
			{
				int num = 0;
				foreach (ExportJob current in list)
				{
					num += current.LogFileList.Count;
				}
				text = string.Format(Resources.NumFileConversionsSelected, num);
				break;
			}
			case ExportType.Trigger:
				text = string.Format(Resources.NumTriggerConversionsSelected, list.Count);
				break;
			default:
				text = string.Format(Resources.NumOfElementsSelected, list.Count);
				break;
			}
			text = text + " (" + NavigatorHelper.GetTotalFileSizeOfExportJobs(this.currentDevice, list) + ")";
			this.labelConversionInfo.Links.Clear();
			int start = text.Length + Resources.ConversionTo.Length + 3;
			string text2 = Path.Combine(this.destinationFolder, this.currentDevice.LogFileStorage.DestSubFolderNamePrimary);
			string destSubFolderNamePrimary = this.currentDevice.LogFileStorage.DestSubFolderNamePrimary;
			if (Directory.Exists(text2))
			{
				this.labelConversionInfo.Links.Add(start, destSubFolderNamePrimary.Length, text2);
			}
			this.labelConversionInfo.Text = string.Concat(new string[]
			{
				text,
				" (",
				Resources.ConversionTo,
				" ",
				destSubFolderNamePrimary,
				")"
			});
			this.mSplitButtonConvert.Enabled = (list.Count > 0);
			this.splitButtonQuickView.Enabled = !this.navigator.IsEmpty();
			this.EnableBatchExport(this.mSplitButtonConvert.Enabled);
			this.FireConversionEnabledChanged();
			this.UpdateTabErrorIcons();
			return true;
		}

		private void EnableBatchExport(bool enabled)
		{
			this.splitButtonBatchExport.Enabled = (enabled && GlobalRuntimeSettings.GetInstance().ActiveApplicationMode == GlobalRuntimeSettings.ApplicationMode.BatchMode);
			this.UpdateSplitButtonBatchExport();
		}

		private void UpdateSplitButtonBatchExport()
		{
			if (!this.splitButtonBatchExport.Enabled)
			{
				return;
			}
			ToolTip toolTip = new ToolTip();
			toolTip.SetToolTip(this.splitButtonBatchExport, GlobalRuntimeSettings.GetInstance().BatchModePath);
			if (GlobalRuntimeSettings.GetInstance().HasMultipleBatchFiles)
			{
				if (this.splitButtonBatchExport.SplitMenuStrip == null)
				{
					this.splitButtonBatchExport.SplitMenuStrip = new ContextMenuStrip();
				}
				this.splitButtonBatchExport.SplitMenuStrip.Items.Clear();
				foreach (KeyValuePair<string, string> current in GlobalRuntimeSettings.GetInstance().BatchModePathList)
				{
					ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem();
					toolStripMenuItem.Text = current.Key;
					toolStripMenuItem.Tag = current.Value;
					toolStripMenuItem.ToolTipText = current.Value;
					toolStripMenuItem.Click += new EventHandler(this.splitButtonBatchExport_Click);
					this.splitButtonBatchExport.SplitMenuStrip.Items.Add(toolStripMenuItem);
				}
				this.splitButtonBatchExport.ShowSplit = true;
				return;
			}
			this.splitButtonBatchExport.ShowSplit = false;
		}

		public void ResetCapacityValues()
		{
			this.labelCapacity.Text = string.Empty;
			this.storageCapacityBar.Clear();
			this.storageCapacityBar.Visible = false;
		}

		private void UpdateCapacityInformation(long numberOfFiles, long freeSpace, long capacity)
		{
			if (this.HardwareFrontend != null && this.HardwareFrontend.CurrentLoggerDevices.Contains(this.currentDevice))
			{
				this.labelCapacity.Text = string.Format(Resources.InfoFilesCapacity, numberOfFiles, GUIUtil.GetSizeStringMBForBytes(capacity));
				this.storageCapacityBar.SetCapacityValues(freeSpace, capacity);
				this.storageCapacityBar.Visible = true;
				return;
			}
			this.labelCapacity.Text = string.Format(Resources.InfoFiles, numberOfFiles);
			this.storageCapacityBar.Clear();
			this.storageCapacityBar.Visible = false;
		}

		private ILoggerDevice GetOrCreateSnapshotDevice(string snapshotFolderPath)
		{
			if (this.currentSnapshotDevice != null && string.Compare(this.currentSnapshotDevice.HardwareKey, snapshotFolderPath, true) == 0)
			{
				this.HardwareFrontend.SetCustomLoggerDevice(this.currentSnapshotDevice, this);
				return this.currentSnapshotDevice;
			}
			this.currentSnapshotDevice = this.HardwareFrontend.CreateCustomLoggerDevice(this.currentDevice.LoggerSpecifics.Type, snapshotFolderPath, this);
			return this.currentSnapshotDevice;
		}

		private void ResetSnapshotButton()
		{
			this.checkBoxDisplay3GSnapshot.CheckedChanged -= new EventHandler(this.checkBoxDisplay3GSnapshot_CheckedChanged);
			this.checkBoxDisplay3GSnapshot.Checked = false;
			this.checkBoxDisplay3GSnapshot.Visible = false;
			this.checkBoxDisplay3GSnapshot.CheckedChanged += new EventHandler(this.checkBoxDisplay3GSnapshot_CheckedChanged);
		}

		private void InitSplitButtonConvert()
		{
			this.mSplitButtonConvert.AutoSize = false;
			this.mSplitButtonConvert.SplitMenuStrip = new ContextMenuStrip();
			this.mSplitButtonConvert.SplitMenuStrip.Opening += new CancelEventHandler(this.SplitMenuStripConvert_Opening);
			this.mSplitButtonConvert.SplitMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.SplitMenuStripConvert_ItemClicked);
			FileConversionProfileManager.InitDropDownItems(this.mSplitButtonConvert.SplitMenuStrip.Items, this);
		}

		private void ShowErrorFile()
		{
			if (this.CurrentDevice != null && this.CurrentDevice.HasErrorFile)
			{
				string text = Path.Combine(this.CurrentDevice.HardwareKey, this.modelValidator.LoggerSpecifics.DataStorage.ErrorFilePath);
				string tempDirectoryName;
				if (this.CurrentDevice.IsZipArchive && TempDirectoryManager.Instance.CreateNewTempDirectory(out tempDirectoryName))
				{
					string text2 = Path.Combine(TempDirectoryManager.Instance.GetFullTempDirectoryPath(tempDirectoryName), this.modelValidator.LoggerSpecifics.DataStorage.ErrorFilePath);
					FileProxy.Copy(text, text2);
					text = text2;
				}
				if (File.Exists(text))
				{
					FileSystemServices.LaunchFile(text);
				}
			}
		}

		private void UpdateShowErrorFileButton()
		{
			this.buttonShowErrorFile.Visible = false;
			if (this.CurrentDevice != null)
			{
				this.buttonShowErrorFile.Visible = this.CurrentDevice.HasErrorFile;
			}
		}

		private void CheckAnalysisPackage()
		{
			ILoggerDevice loggerDevice = this.isCurrentDeviceInvalid ? null : this.currentDevice;
			bool flag = AnalysisPackage.SearchAndLoadAnalysisPackage(loggerDevice, this.exportDatabases);
			this.exportDatabases.ValidateInput();
			this.EnableConversionAndSetInfoText(!flag);
		}

		public bool Serialize(CardReaderNavigatorPage cardReaderNaviPage)
		{
			if (cardReaderNaviPage == null)
			{
				return false;
			}
			cardReaderNaviPage.FileConversionParameters = this.conversionParameters;
			cardReaderNaviPage.SelectedDevicePath = this.selectedDevicePath;
			LayoutSerializationContainer layoutSerializationContainer = this.navigator.SerializeGrid();
			cardReaderNaviPage.NavigatorGridLogfilesLayout = layoutSerializationContainer.GridControlLogFilesLayout;
			cardReaderNaviPage.NavigatorGridMarkerLayout = layoutSerializationContainer.GridControlMarkerLayout;
			cardReaderNaviPage.NavigatorGridMarkerSelectionLayout = layoutSerializationContainer.GridControlMarkerSelectionTableLayout;
			cardReaderNaviPage.NavigatorGridMeasurementsLayout = layoutSerializationContainer.GridViewMeasurementsLayout;
			return true;
		}

		public bool DeSerialize(CardReaderNavigatorPage cardReaderNaviPage)
		{
			if (cardReaderNaviPage == null)
			{
				return false;
			}
			if (cardReaderNaviPage.FileConversionParameters != null)
			{
				this.conversionParameters = cardReaderNaviPage.FileConversionParameters;
			}
			if (!string.IsNullOrEmpty(cardReaderNaviPage.SelectedDevicePath))
			{
				this.selectedDevicePath = cardReaderNaviPage.SelectedDevicePath;
			}
			LayoutSerializationContainer layoutSerializationContainer = new LayoutSerializationContainer();
			layoutSerializationContainer.GridControlLogFilesLayout = cardReaderNaviPage.NavigatorGridLogfilesLayout;
			layoutSerializationContainer.GridControlMarkerLayout = cardReaderNaviPage.NavigatorGridMarkerLayout;
			layoutSerializationContainer.GridControlMarkerSelectionTableLayout = cardReaderNaviPage.NavigatorGridMarkerSelectionLayout;
			layoutSerializationContainer.GridViewMeasurementsLayout = cardReaderNaviPage.NavigatorGridMeasurementsLayout;
			this.navigator.DeSerializeGrid(layoutSerializationContainer);
			return true;
		}

		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			((IPropertyWindow)this).ValidateInput();
		}

		private void OnExportDatabaseValidating(object sender, EventArgs e)
		{
			this.UpdateTabErrorIcons();
		}

		private void UpdateTabErrorIcons()
		{
			if (this.exportDatabases.HasValidationError())
			{
				this.tabPageExportDatabases.ImageIndex = 1;
				return;
			}
			if (this.conversionParameters != null && this.conversionParameters.ExportDatabaseConfiguration != null)
			{
				string analysisPackagePath = this.conversionParameters.ExportDatabaseConfiguration.GetAnalysisPackagePath();
				if (this.conversionParameters.ExportDatabaseConfiguration.Mode == ExportDatabaseConfiguration.DBSelectionMode.FromAnalysisPackage && !string.IsNullOrEmpty(analysisPackagePath))
				{
					if (AnalysisPackage.IsPasswordProtected(analysisPackagePath))
					{
						this.tabPageExportDatabases.ImageIndex = 3;
						return;
					}
					this.tabPageExportDatabases.ImageIndex = 2;
					return;
				}
				else
				{
					this.tabPageExportDatabases.ImageIndex = -1;
				}
			}
		}

		public OfflineSourceConfig GetOfflineSourceConfig(bool isLocalContext, bool isCANoeCANalyzer)
		{
			return this.fileNavigator.GetOfflineSourceConfig(isLocalContext, isCANoeCANalyzer);
		}

		public IEnumerable<Database> GetConversionDatabases()
		{
			return AnalysisPackage.GetConversionDatabases(this.conversionParameters, this.databaseConfiguration.Databases, true);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CardReaderNavigator));
			this.tabControl = new TabControl();
			this.tabPageNavigator = new TabPage();
			this.fileNavigator = new FilemanagerGui();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.labelSourceFolder = new Label();
			this.buttonShowInfo = new Button();
			this.labelCapacity = new Label();
			this.buttonDeleteLogData = new Button();
			this.checkBoxDisplay3GSnapshot = new CheckBox();
			this.buttonShowErrorFile = new Button();
			this.buttonEject = new Button();
			this.splitButtonQuickView = new SplitButton();
			this.labelQuickView = new Label();
			this.dropDownButtonDeviceList = new DropDownButton();
			this.popupMenuDeviceList = new PopupMenu(this.components);
			this.barManagerDeviceList = new BarManager(this.components);
			this.barDockControlTop = new BarDockControl();
			this.barDockControlBottom = new BarDockControl();
			this.barDockControlLeft = new BarDockControl();
			this.barDockControlRight = new BarDockControl();
			this.storageCapacityBar = new StorageCapacityBar();
			this.tabPageSettings = new TabPage();
			this.fileConversion = new FileConversionSettings();
			this.tabPageChannelMapping = new TabPage();
			this.mChannelMapping = new ChannelMapping();
			this.tabPageExportDatabases = new TabPage();
			this.exportDatabases = new ExportDatabases();
			this.imageListErrorIcons = new ImageList(this.components);
			this.tableLayoutPanel2 = new TableLayoutPanel();
			this.splitButtonBatchExport = new SplitButton();
			this.mSplitButtonConvert = new SplitButton();
			this.labelConversionInfo = new LinkLabel();
			this.toolTip = new ToolTip(this.components);
			this.tabControl.SuspendLayout();
			this.tabPageNavigator.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((ISupportInitialize)this.popupMenuDeviceList).BeginInit();
			((ISupportInitialize)this.barManagerDeviceList).BeginInit();
			this.tabPageSettings.SuspendLayout();
			this.tabPageChannelMapping.SuspendLayout();
			this.tabPageExportDatabases.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			base.SuspendLayout();
			this.tableLayoutPanel2.SetColumnSpan(this.tabControl, 3);
			this.tabControl.Controls.Add(this.tabPageNavigator);
			this.tabControl.Controls.Add(this.tabPageSettings);
			this.tabControl.Controls.Add(this.tabPageChannelMapping);
			this.tabControl.Controls.Add(this.tabPageExportDatabases);
			componentResourceManager.ApplyResources(this.tabControl, "tabControl");
			this.tabControl.ImageList = this.imageListErrorIcons;
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.SelectedIndexChanged += new EventHandler(this.tabControl_SelectedIndexChanged);
			this.tabPageNavigator.AllowDrop = true;
			this.tabPageNavigator.BackColor = SystemColors.Control;
			this.tabPageNavigator.Controls.Add(this.fileNavigator);
			this.tabPageNavigator.Controls.Add(this.tableLayoutPanel1);
			componentResourceManager.ApplyResources(this.tabPageNavigator, "tabPageNavigator");
			this.tabPageNavigator.Name = "tabPageNavigator";
			this.tabPageNavigator.DragDrop += new DragEventHandler(this.tabPageNavigator_DragDrop);
			this.tabPageNavigator.DragEnter += new DragEventHandler(this.tabPageNavigator_DragEnter);
			componentResourceManager.ApplyResources(this.fileNavigator, "fileNavigator");
			this.fileNavigator.Name = "fileNavigator";
			this.fileNavigator.RecoverMeasurements = false;
			this.fileNavigator.StandaloneMode = false;
			this.fileNavigator.Click += new EventHandler(this.fileNavigator_Click);
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.labelSourceFolder, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonShowInfo, 6, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelCapacity, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.buttonDeleteLogData, 6, 1);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDisplay3GSnapshot, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonShowErrorFile, 5, 1);
			this.tableLayoutPanel1.Controls.Add(this.buttonEject, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.splitButtonQuickView, 3, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelQuickView, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.dropDownButtonDeviceList, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.storageCapacityBar, 1, 1);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.labelSourceFolder, "labelSourceFolder");
			this.labelSourceFolder.Name = "labelSourceFolder";
			componentResourceManager.ApplyResources(this.buttonShowInfo, "buttonShowInfo");
			this.buttonShowInfo.Image = Resources.ImageInfo;
			this.buttonShowInfo.Name = "buttonShowInfo";
			this.toolTip.SetToolTip(this.buttonShowInfo, componentResourceManager.GetString("buttonShowInfo.ToolTip"));
			this.buttonShowInfo.UseVisualStyleBackColor = true;
			this.buttonShowInfo.Click += new EventHandler(this.buttonShowInfo_Click);
			this.buttonShowInfo.MouseEnter += new EventHandler(this.buttonShowInfo_MouseEnter);
			componentResourceManager.ApplyResources(this.labelCapacity, "labelCapacity");
			this.labelCapacity.Name = "labelCapacity";
			componentResourceManager.ApplyResources(this.buttonDeleteLogData, "buttonDeleteLogData");
			this.buttonDeleteLogData.Image = Resources.ImageDelLogFile;
			this.buttonDeleteLogData.Name = "buttonDeleteLogData";
			this.buttonDeleteLogData.UseVisualStyleBackColor = true;
			this.buttonDeleteLogData.Click += new EventHandler(this.buttonDeleteLogData_Click);
			this.buttonDeleteLogData.MouseEnter += new EventHandler(this.buttonDeleteLogData_MouseEnter);
			componentResourceManager.ApplyResources(this.checkBoxDisplay3GSnapshot, "checkBoxDisplay3GSnapshot");
			this.checkBoxDisplay3GSnapshot.Image = Resources.ImageSnapshotRecord;
			this.checkBoxDisplay3GSnapshot.Name = "checkBoxDisplay3GSnapshot";
			this.checkBoxDisplay3GSnapshot.UseVisualStyleBackColor = true;
			this.checkBoxDisplay3GSnapshot.CheckedChanged += new EventHandler(this.checkBoxDisplay3GSnapshot_CheckedChanged);
			this.checkBoxDisplay3GSnapshot.MouseEnter += new EventHandler(this.checkBoxDisplay3GSnapshot_MouseEnter);
			componentResourceManager.ApplyResources(this.buttonShowErrorFile, "buttonShowErrorFile");
			this.buttonShowErrorFile.Image = Resources.ImageWarning;
			this.buttonShowErrorFile.Name = "buttonShowErrorFile";
			this.buttonShowErrorFile.UseVisualStyleBackColor = true;
			this.buttonShowErrorFile.Click += new EventHandler(this.buttonShowErrorFile_Click);
			this.buttonShowErrorFile.MouseEnter += new EventHandler(this.buttonShowErrorFile_MouseEnter);
			componentResourceManager.ApplyResources(this.buttonEject, "buttonEject");
			this.buttonEject.Image = Resources.ImageEject;
			this.buttonEject.Name = "buttonEject";
			this.buttonEject.UseVisualStyleBackColor = true;
			this.buttonEject.Click += new EventHandler(this.buttonEject_Click);
			this.buttonEject.MouseEnter += new EventHandler(this.buttonEject_MouseEnter);
			componentResourceManager.ApplyResources(this.splitButtonQuickView, "splitButtonQuickView");
			this.splitButtonQuickView.Name = "splitButtonQuickView";
			this.splitButtonQuickView.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.labelQuickView, "labelQuickView");
			this.labelQuickView.Name = "labelQuickView";
			this.dropDownButtonDeviceList.AllowHtmlDraw = DefaultBoolean.True;
			componentResourceManager.ApplyResources(this.dropDownButtonDeviceList, "dropDownButtonDeviceList");
			this.dropDownButtonDeviceList.Appearance.Options.UseTextOptions = true;
			this.dropDownButtonDeviceList.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
			this.tableLayoutPanel1.SetColumnSpan(this.dropDownButtonDeviceList, 3);
			this.dropDownButtonDeviceList.DropDownArrowStyle = DropDownArrowStyle.Show;
			this.dropDownButtonDeviceList.DropDownControl = this.popupMenuDeviceList;
			this.dropDownButtonDeviceList.MenuManager = this.barManagerDeviceList;
			this.dropDownButtonDeviceList.Name = "dropDownButtonDeviceList";
			this.popupMenuDeviceList.Manager = this.barManagerDeviceList;
			this.popupMenuDeviceList.Name = "popupMenuDeviceList";
			this.barManagerDeviceList.DockControls.Add(this.barDockControlTop);
			this.barManagerDeviceList.DockControls.Add(this.barDockControlBottom);
			this.barManagerDeviceList.DockControls.Add(this.barDockControlLeft);
			this.barManagerDeviceList.DockControls.Add(this.barDockControlRight);
			this.barManagerDeviceList.Form = this;
			this.barManagerDeviceList.MaxItemId = 0;
			this.barDockControlTop.CausesValidation = false;
			componentResourceManager.ApplyResources(this.barDockControlTop, "barDockControlTop");
			this.barDockControlBottom.CausesValidation = false;
			componentResourceManager.ApplyResources(this.barDockControlBottom, "barDockControlBottom");
			this.barDockControlLeft.CausesValidation = false;
			componentResourceManager.ApplyResources(this.barDockControlLeft, "barDockControlLeft");
			this.barDockControlRight.CausesValidation = false;
			componentResourceManager.ApplyResources(this.barDockControlRight, "barDockControlRight");
			componentResourceManager.ApplyResources(this.storageCapacityBar, "storageCapacityBar");
			this.storageCapacityBar.Name = "storageCapacityBar";
			this.tabPageSettings.BackColor = SystemColors.Control;
			this.tabPageSettings.Controls.Add(this.fileConversion);
			componentResourceManager.ApplyResources(this.tabPageSettings, "tabPageSettings");
			this.tabPageSettings.Name = "tabPageSettings";
			componentResourceManager.ApplyResources(this.fileConversion, "fileConversion");
			this.fileConversion.FileConversionParameters = null;
			this.fileConversion.LoggerSpecifics = null;
			this.fileConversion.Name = "fileConversion";
			this.tabPageChannelMapping.BackColor = SystemColors.Control;
			this.tabPageChannelMapping.Controls.Add(this.mChannelMapping);
			componentResourceManager.ApplyResources(this.tabPageChannelMapping, "tabPageChannelMapping");
			this.tabPageChannelMapping.Name = "tabPageChannelMapping";
			componentResourceManager.ApplyResources(this.mChannelMapping, "mChannelMapping");
			this.mChannelMapping.FileConversionParameters = null;
			this.mChannelMapping.IsCLFConversionMode = false;
			this.mChannelMapping.Name = "mChannelMapping";
			this.tabPageExportDatabases.Controls.Add(this.exportDatabases);
			componentResourceManager.ApplyResources(this.tabPageExportDatabases, "tabPageExportDatabases");
			this.tabPageExportDatabases.Name = "tabPageExportDatabases";
			this.tabPageExportDatabases.UseVisualStyleBackColor = true;
			this.exportDatabases.AllowDrop = true;
			this.exportDatabases.AnalysisPackageParameters = null;
			this.exportDatabases.ApplicationDatabaseManager = null;
			this.exportDatabases.BackColor = SystemColors.Control;
			this.exportDatabases.ConfigurationManagerService = null;
			this.exportDatabases.DisplayMode = null;
			componentResourceManager.ApplyResources(this.exportDatabases, "exportDatabases");
			this.exportDatabases.FileConversionParameters = null;
			this.exportDatabases.IsVLExportMode = false;
			this.exportDatabases.ModelValidator = null;
			this.exportDatabases.Name = "exportDatabases";
			this.exportDatabases.SemanticChecker = null;
			this.imageListErrorIcons.ColorDepth = ColorDepth.Depth8Bit;
			componentResourceManager.ApplyResources(this.imageListErrorIcons, "imageListErrorIcons");
			this.imageListErrorIcons.TransparentColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.splitButtonBatchExport, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.mSplitButtonConvert, 2, 1);
			this.tableLayoutPanel2.Controls.Add(this.tabControl, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.labelConversionInfo, 0, 1);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			componentResourceManager.ApplyResources(this.splitButtonBatchExport, "splitButtonBatchExport");
			this.splitButtonBatchExport.Name = "splitButtonBatchExport";
			this.splitButtonBatchExport.ShowSplitAlways = true;
			this.splitButtonBatchExport.UseVisualStyleBackColor = true;
			this.splitButtonBatchExport.Click += new EventHandler(this.splitButtonBatchExport_Click);
			componentResourceManager.ApplyResources(this.mSplitButtonConvert, "mSplitButtonConvert");
			this.mSplitButtonConvert.Name = "mSplitButtonConvert";
			this.mSplitButtonConvert.ShowSplitAlways = true;
			this.mSplitButtonConvert.UseVisualStyleBackColor = true;
			this.mSplitButtonConvert.Click += new EventHandler(this.SplitButtonConvert_Click);
			componentResourceManager.ApplyResources(this.labelConversionInfo, "labelConversionInfo");
			this.labelConversionInfo.Name = "labelConversionInfo";
			this.labelConversionInfo.TabStop = true;
			this.labelConversionInfo.LinkClicked += new LinkLabelLinkClickedEventHandler(this.labelConversionInfo_LinkClicked);
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.tableLayoutPanel2);
			base.Controls.Add(this.barDockControlLeft);
			base.Controls.Add(this.barDockControlRight);
			base.Controls.Add(this.barDockControlBottom);
			base.Controls.Add(this.barDockControlTop);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "CardReaderNavigator";
			this.tabControl.ResumeLayout(false);
			this.tabPageNavigator.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((ISupportInitialize)this.popupMenuDeviceList).EndInit();
			((ISupportInitialize)this.barManagerDeviceList).EndInit();
			this.tabPageSettings.ResumeLayout(false);
			this.tabPageChannelMapping.ResumeLayout(false);
			this.tabPageExportDatabases.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
