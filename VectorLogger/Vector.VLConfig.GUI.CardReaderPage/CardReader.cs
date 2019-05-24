using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
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
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.GUI.Common.QuickView;
using Vector.VLConfig.GUI.DatabasesPage;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.CardReaderPage
{
	public class CardReader : UserControl, IUpdateObserver<DatabaseConfiguration>, IUpdateObserver<LoggerType>, IUpdateObserver, IPropertyWindow, IConfigClipboardClient, IHardwareFrontendClient, IFileConversionParametersClient, IQuickViewClient
	{
		public delegate void RefreshViewRequestHandler(object sender, EventArgs e);

		public delegate void EnableAllFileWatchersRequestHandler(object sender, bool isEnabled);

		public delegate void DeviceChangedDelegate(object sender, EventArgs e);

		public delegate void ConversionEnabledChangedDelegate(object sender, EventArgs e);

		private FileConversionParameters conversionParameters;

		private string destinationFolder;

		private FileConversionDestFormat destinationFormat;

		private DatabaseConfiguration databaseConfiguration;

		private string configurationFolderPath;

		private bool isCurrentDeviceInvalid;

		private string selectedDevicePath;

		private Dictionary<string, LoggerDeviceCommon> drivePathToScannedDevice;

		private ILoggerDevice customDirectoryDevice;

		private bool isConverting;

		private bool isFolderChanged;

		private ILoggerDevice currentActualDevice;

		private ILoggerDevice currentSnapshotDevice;

		private string deviceWithAnalysisPackage;

		private DeviceListPopupMenuController deviceListPopupMenuController;

		private readonly QuickView quickView;

		private IContainer components;

		private GroupBox groupBoxSourceFolder;

		private TableLayoutPanel tableLayoutPanelSourceFolder;

		private CardReaderGrid cardReaderGrid;

		private FileConversion fileConversion;

		private ToolTip toolTip;

		private Button buttonEject;

		private Button buttonShowInfo;

		private Button buttonDeleteLogData;

		private CheckBox checkBoxDisplay3GSnapshot;

		private Label labelCapacity;

		private Button buttonShowErrorFile;

		private DropDownButton dropDownButtonDeviceList;

		private PopupMenu popupMenuDeviceList;

		private BarManager barManagerDeviceList;

		private Bar bar1;

		private Bar bar2;

		private Bar bar3;

		private BarDockControl barDockControlTop;

		private BarDockControl barDockControlBottom;

		private BarDockControl barDockControlLeft;

		private BarDockControl barDockControlRight;

		private SplitButton splitButtonQuickView;

		private Label labelQuickView;

		private StorageCapacityBar storageCapacityBar;

		public event CardReader.DeviceChangedDelegate DeviceChanged;

		public event CardReader.ConversionEnabledChangedDelegate ConversionEnabledChanged;

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.cardReaderGrid.ModelValidator;
			}
			set
			{
				this.cardReaderGrid.ModelValidator = value;
				this.fileConversion.Init(this.cardReaderGrid.ModelValidator.LoggerSpecifics, false, this);
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
				return PageType.CardReader;
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
				if (value && !visible)
				{
					this.CheckAnalysisPackage();
					this.fileConversion.RefreshStatus();
				}
			}
		}

		public IHardwareFrontend HardwareFrontend
		{
			get
			{
				return this.cardReaderGrid.HardwareFrontend;
			}
			set
			{
				this.cardReaderGrid.HardwareFrontend = value;
			}
		}

		EnumViewType IFileConversionParametersClient.ViewType
		{
			get
			{
				return EnumViewType.Classic;
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
				this.OnFileConversionParametersChanged(this, new FileConversionParametersChangedEventArgs(FileConversionParametersChangedEventArgs.Parameter.All));
				this.EnableConversionAndSetInfoText(false);
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
				if (this.fileConversion.ConversionInfo.Links.Count > 0)
				{
					return this.fileConversion.ConversionInfo.Links[0].LinkData.ToString();
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

		public ILoggerDevice CurrentActualDevice
		{
			get
			{
				return this.currentActualDevice;
			}
		}

		public IConfigurationManagerService ConfigurationManagerService
		{
			get
			{
				return this.fileConversion.ConfigurationManagerService;
			}
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
				return null;
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
				return this.cardReaderGrid.ModelValidator.LoggerSpecifics;
			}
		}

		public ILoggerDevice CurrentDevice
		{
			get
			{
				return this.currentActualDevice;
			}
		}

		public string LogDataIniFile2
		{
			get
			{
				ILogFile arg_46_0;
				if (this.CurrentActualDevice == null)
				{
					arg_46_0 = null;
				}
				else
				{
					arg_46_0 = this.CurrentActualDevice.LogFileStorage.LogFiles.FirstOrDefault((ILogFile lf) => Constants.LogDataIniFile2Name.Equals(lf.DefaultName, StringComparison.OrdinalIgnoreCase));
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

		public CardReader()
		{
			this.InitializeComponent();
			this.isCurrentDeviceInvalid = false;
			this.customDirectoryDevice = null;
			this.selectedDevicePath = "";
			this.cardReaderGrid.DeleteAllFiles += new EventHandler(this.OnDeleteAllFiles);
			this.cardReaderGrid.FileIsSelectedChanged += new EventHandler(this.OnExportFileEnabledChanged);
			this.drivePathToScannedDevice = new Dictionary<string, LoggerDeviceCommon>();
			this.isConverting = false;
			this.isFolderChanged = false;
			this.groupBoxSourceFolder.AllowDrop = true;
			this.deviceWithAnalysisPackage = string.Empty;
			this.fileConversion.ExportDatabases.StatusChanged += new ExportDatabases.StatusChangedHandler(this.ExportDatabasesStatusChanged);
			this.fileConversion.ExportDatabases.FileConversionParametersChanged += new FileConversion.FileConversionParametersChangedHandler(this.OnFileConversionParametersChanged);
			this.deviceListPopupMenuController = new DeviceListPopupMenuController(this.dropDownButtonDeviceList, this.barManagerDeviceList, this.popupMenuDeviceList, false, 5u, 5u);
			this.deviceListPopupMenuController.SelectedDeviceChanged += new EventHandler(this.OnSelectedDeviceChanged);
			this.quickView = QuickView.Create(this, this, false);
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return false;
		}

		void IPropertyWindow.Init()
		{
			this.ResetSnapshotButton();
			this.ResetCapacityValues();
			this.UpdateShowErrorFileButton();
			this.buttonDeleteLogData.Enabled = false;
			this.buttonEject.Enabled = false;
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.CardReader);
			}
			if (this.conversionParameters == null)
			{
				this.conversionParameters = FileConversionHelper.GetDefaultParameters(this.fileConversion.LoggerSpecifics);
			}
			else if (!Directory.Exists(this.conversionParameters.DestinationFolder))
			{
				this.conversionParameters.DestinationFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
			this.cardReaderGrid.DestinationFolder = this.conversionParameters.DestinationFolder;
			this.fileConversion.FileConversionParameters = this.conversionParameters;
			this.destinationFolder = this.conversionParameters.DestinationFolder;
			this.destinationFormat = this.conversionParameters.DestinationFormat;
			this.ApplyLoggerSpecifics(this.cardReaderGrid.ModelValidator.LoggerSpecifics);
			this.fileConversion.ParametersChanged += new FileConversion.FileConversionParametersChangedHandler(this.OnFileConversionParametersChanged);
			this.fileConversion.StartConversion += new FileConversion.StartConversionHandler(this.OnStartConversion);
			this.fileConversion.IsConversionEnabled = false;
			this.deviceListPopupMenuController.Init(this.cardReaderGrid.ModelValidator, this.HardwareFrontend.IsScanForAllLoggerTypesEnabled);
			this.RestoreSelectionFromAppDataSettings();
			this.isFolderChanged = false;
			this.isConverting = false;
			this.fileConversion.ExportDatabases.ModelValidator = ((IPropertyWindow)this).ModelValidator;
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
			if (this.cardReaderGrid.CurrentDevice != device)
			{
				return;
			}
			if (this.isConverting)
			{
				this.isFolderChanged = true;
				return;
			}
			this.DisplayCurrentCardReaderContent();
		}

		public void DevicesAdded(IList<ILoggerDevice> devices)
		{
			foreach (ILoggerDevice current in devices)
			{
				if (!this.drivePathToScannedDevice.ContainsKey(current.HardwareKey))
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

		public void AdditionalDrivesListChanged(IList<string> additionalDrivesList)
		{
			this.deviceListPopupMenuController.AdditionalDrives.Clear();
			foreach (string current in additionalDrivesList)
			{
				this.deviceListPopupMenuController.AdditionalDrives.Add(current);
			}
		}

		void IUpdateObserver<DatabaseConfiguration>.Update(DatabaseConfiguration data)
		{
			this.databaseConfiguration = data;
			this.EnableConversionAndSetInfoText(false);
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType type)
		{
			this.ApplyLoggerSpecifics(this.cardReaderGrid.ModelValidator.LoggerSpecifics);
			this.deviceListPopupMenuController.Init(this.cardReaderGrid.ModelValidator, this.HardwareFrontend.IsScanForAllLoggerTypesEnabled);
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
			this.FireConversionEnabledChanged();
		}

		public void FireConversionEnabledChanged()
		{
			if (this.ConversionEnabledChanged != null)
			{
				this.ConversionEnabledChanged(this, new EventArgs());
			}
		}

		public void ShowDeviceInfo()
		{
			CardReaderDeviceInformation.Display(this.cardReaderGrid.CurrentDevice);
		}

		public void StartConversion()
		{
			this.OnStartConversion(this, EventArgs.Empty);
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

		public void RefreshView()
		{
			if (((IPropertyWindow)this).IsVisible)
			{
				this.fileConversion.ExportDatabases.ClearAutomaticAnalysisPackage();
				this.CheckAnalysisPackage();
			}
		}

		private void OnSelectedDeviceChanged(object sender, EventArgs e)
		{
			string text = this.deviceListPopupMenuController.SelectedDevicePath;
			this.selectedDevicePath = text;
			if (string.IsNullOrEmpty(text))
			{
				this.cardReaderGrid.CurrentDevice = null;
				this.currentActualDevice = null;
			}
			else if (this.drivePathToScannedDevice.ContainsKey(text))
			{
				LoggerDeviceCommon loggerDeviceCommon = this.drivePathToScannedDevice[text];
				if (this.cardReaderGrid.CurrentDevice == loggerDeviceCommon)
				{
					this.checkBoxDisplay3GSnapshot.Visible = loggerDeviceCommon.HasSnapshotFolderContainingLogData;
					return;
				}
				this.cardReaderGrid.CurrentDevice = loggerDeviceCommon;
				this.cardReaderGrid.IsCurrentDeviceCustomDirectory = false;
			}
			else if (this.deviceListPopupMenuController.AdditionalDrives.Contains(text))
			{
				ILoggerSpecifics loggerSpecifics = this.cardReaderGrid.ModelValidator.LoggerSpecifics;
				if (this.HardwareFrontend.IsScanForAllLoggerTypesEnabled)
				{
					loggerSpecifics = MlRtIniFile.LoadLoggerTypeFromMemoryCardContent(text);
				}
				this.cardReaderGrid.CurrentDevice = this.cardReaderGrid.HardwareFrontend.CreateCustomLoggerDevice(loggerSpecifics.Type, text, this);
				this.cardReaderGrid.IsCurrentDeviceCustomDirectory = false;
			}
			else
			{
				ILoggerSpecifics loggerSpecifics2 = this.cardReaderGrid.ModelValidator.LoggerSpecifics;
				if (this.HardwareFrontend.IsScanForAllLoggerTypesEnabled)
				{
					loggerSpecifics2 = MlRtIniFile.LoadLoggerTypeFromMemoryCardContent(text);
				}
				this.customDirectoryDevice = this.cardReaderGrid.HardwareFrontend.CreateCustomLoggerDevice(loggerSpecifics2.Type, text, this);
				this.cardReaderGrid.CurrentDevice = this.customDirectoryDevice;
				this.cardReaderGrid.IsCurrentDeviceCustomDirectory = true;
			}
			if (!this.cardReaderGrid.IsCurrentDeviceCustomDirectory)
			{
				this.HardwareFrontend.ReleaseCustomLoggerDevice(this);
			}
			if (this.cardReaderGrid.CurrentDevice != null)
			{
				this.ApplyLoggerSpecifics(this.cardReaderGrid.CurrentDevice.LoggerSpecifics);
			}
			this.fileConversion.EnableParamaterDeleteSourceFilesWhenDone(!this.cardReaderGrid.IsCurrentDeviceCustomDirectory);
			bool flag = true;
			if (this.currentActualDevice != null && this.cardReaderGrid.CurrentDevice != null && this.currentActualDevice.HardwareKey == this.cardReaderGrid.CurrentDevice.HardwareKey)
			{
				flag = false;
			}
			this.currentActualDevice = this.cardReaderGrid.CurrentDevice;
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
					this.cardReaderGrid.CurrentDevice = this.GetOrCreateSnapshotDevice(this.currentActualDevice.SnapshotFolderPath);
					this.groupBoxSourceFolder.Text = Resources.DataSource + Resources.SnapshotDataTitlePostfix;
				}
				else
				{
					this.groupBoxSourceFolder.Text = Resources.DataSource;
				}
			}
			else
			{
				this.ResetSnapshotButton();
				this.currentSnapshotDevice = null;
				this.groupBoxSourceFolder.Text = Resources.DataSource;
			}
			this.DisplayCurrentCardReaderContent();
			if (((IPropertyWindow)this).IsVisible)
			{
				this.CheckAnalysisPackage();
			}
			this.FireDeviceChanged();
		}

		private void UpdateDeviceAsync(ILoggerDevice device)
		{
			if (device == null)
			{
				return;
			}
			if (!device.LogFileStorage.IsOutdated)
			{
				return;
			}
			Cursor.Current = Cursors.WaitCursor;
			if (Application.OpenForms != null && Application.OpenForms.Count < 1)
			{
				device.LogFileStorage.UpdateFileList();
				return;
			}
			ActivityIndicatorForm activityIndicatorForm = new ActivityIndicatorForm();
			ProcessExitedDelegate processExitedDelegate = new ProcessExitedDelegate(activityIndicatorForm.ProcessExited);
			activityIndicatorForm.Text = Resources.ActivityTitle;
			activityIndicatorForm.SetStatusText(Resources.ActivityTextLoading);
			System.Timers.Timer showDialogTimer = new System.Timers.Timer(500.0);
			ManualResetEvent finishedEvent = new ManualResetEvent(false);
			bool showDialog = false;
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += delegate(object sender, DoWorkEventArgs arguments)
			{
				try
				{
					showDialogTimer.Start();
					device.LogFileStorage.UpdateFileList();
					finishedEvent.Set();
				}
				catch (Exception)
				{
					finishedEvent.Set();
				}
			};
			backgroundWorker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs arguments)
			{
				showDialogTimer.Stop();
				finishedEvent.Set();
				processExitedDelegate();
			};
			showDialogTimer.Elapsed += delegate(object sender, ElapsedEventArgs arguments)
			{
				showDialog = true;
				showDialogTimer.Stop();
				finishedEvent.Set();
			};
			backgroundWorker.RunWorkerAsync();
			finishedEvent.WaitOne();
			if (showDialog)
			{
				activityIndicatorForm.ShowDialog();
				if (activityIndicatorForm != null)
				{
					activityIndicatorForm.Dispose();
				}
			}
			Cursor.Current = Cursors.Default;
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
					this.cardReaderGrid.CurrentDevice = this.GetOrCreateSnapshotDevice(this.cardReaderGrid.CurrentDevice.SnapshotFolderPath);
				}
				else
				{
					InformMessageBox.Error(Resources.ErrorSnapshotNotAvailable);
					this.cardReaderGrid.CurrentDevice = this.currentActualDevice;
					flag = true;
					checkBox.Visible = false;
				}
			}
			else
			{
				this.cardReaderGrid.CurrentDevice = this.currentActualDevice;
				flag = true;
			}
			if (flag)
			{
				if (this.HardwareFrontend.IsCustomLoggerDevice(this.cardReaderGrid.CurrentDevice))
				{
					this.HardwareFrontend.SetCustomLoggerDevice(this.cardReaderGrid.CurrentDevice, this);
				}
				else
				{
					this.HardwareFrontend.ReleaseCustomLoggerDevice(this);
				}
			}
			Cursor.Current = Cursors.WaitCursor;
			this.DisplayCurrentCardReaderContent();
			this.fileConversion.RefreshStatus();
			Cursor.Current = Cursors.Default;
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
					if (this.destinationFormat != this.fileConversion.ExportDatabases.FileConversionParameters.DestinationFormat)
					{
						this.fileConversion.ExportDatabases.FileConversionParameters.DestinationFormat = this.destinationFormat;
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
					if (this.destinationFormat != this.fileConversion.ExportDatabases.FileConversionParameters.DestinationFormat)
					{
						this.fileConversion.ExportDatabases.FileConversionParameters.DestinationFormat = this.destinationFormat;
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

		private void OnStartConversion(object sender, EventArgs e)
		{
			this.DisplayCurrentCardReaderContent();
			bool flag = true;
			if (this.isCurrentDeviceInvalid)
			{
				InformMessageBox.Error(Resources.ErrorUnableToAccessDriveWillUpdate);
				this.HardwareFrontend.UpdateLoggerDeviceList();
				flag = false;
			}
			if (!flag)
			{
				return;
			}
			this.Convert();
			this.DisplayCurrentCardReaderContent();
			this.cardReaderGrid.Refresh();
			this.fileConversion.RefreshStatus();
		}

		private void buttonDeleteLogData_Click(object sender, EventArgs e)
		{
			this.DeleteAllFiles();
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
			this.toolTip.SetToolTip(this.buttonShowErrorFile, string.Format(Resources.ErrorFileFoundClickToOpen, Path.GetFileName(this.cardReaderGrid.ModelValidator.LoggerSpecifics.DataStorage.ErrorFilePath)));
		}

		private void groupBoxSourceFolder_DragEnter(object sender, DragEventArgs e)
		{
			if (GUIUtil.GetKindOfDrop(e) == GUIUtil.FileDropContent.IllegalDrop)
			{
				e.Effect = DragDropEffects.None;
				return;
			}
			if (this.cardReaderGrid.ModelValidator.LoggerSpecifics.DataStorage.HasPackableLogData)
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			string path = array[0];
			if (Directory.Exists(path))
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void groupBoxSourceFolder_DragDrop(object sender, DragEventArgs e)
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
			if (!GUIUtil.FolderAccessible(path))
			{
				InformMessageBox.Error(Resources.ErrorUnaccessiblePath);
				return;
			}
			if (!this.deviceListPopupMenuController.SelectDevice(path))
			{
				this.deviceListPopupMenuController.AddCustomFolderOrPackedImageDevice(path);
			}
		}

		public void ExportDatabasesStatusChanged()
		{
			this.EnableConversionAndSetInfoText(false);
		}

		private void ApplyLoggerSpecifics(ILoggerSpecifics loggerSpecifics)
		{
			if (this.HardwareFrontend.IsScanForAllLoggerTypesEnabled && this.cardReaderGrid.ModelValidator.LoggerSpecifics.Type != loggerSpecifics.Type)
			{
				this.cardReaderGrid.ModelValidator.LoggerSpecifics = loggerSpecifics;
			}
			this.fileConversion.LoggerSpecifics = loggerSpecifics;
			this.destinationFolder = this.conversionParameters.DestinationFolder;
			this.destinationFormat = this.conversionParameters.DestinationFormat;
			this.fileConversion.RefreshStatus();
			this.quickView.ApplyLoggerSpecifics();
		}

		private void RestoreSelectionFromAppDataSettings()
		{
			if (this.deviceListPopupMenuController.IsPathAvailableAndSelectable(this.selectedDevicePath))
			{
				this.deviceListPopupMenuController.SelectDevice(this.selectedDevicePath);
			}
		}

		private void DisplayCurrentCardReaderContent()
		{
			this.buttonEject.Enabled = false;
			this.buttonShowInfo.Enabled = false;
			this.buttonDeleteLogData.Enabled = false;
			if (this.checkBoxDisplay3GSnapshot.Visible && this.checkBoxDisplay3GSnapshot.Checked)
			{
				this.groupBoxSourceFolder.Text = Resources.DataSource + Resources.SnapshotDataTitlePostfix;
			}
			else
			{
				this.groupBoxSourceFolder.Text = Resources.DataSource;
			}
			this.buttonShowInfo.Visible = (this.cardReaderGrid.ModelValidator.LoggerSpecifics.Type != LoggerType.VN1630log);
			if (this.cardReaderGrid.CurrentDevice != null)
			{
				this.buttonShowInfo.Visible = (this.cardReaderGrid.CurrentDevice.LoggerSpecifics.Type != LoggerType.VN1630log);
				this.buttonEject.Enabled = !this.HardwareFrontend.IsCustomLoggerDevice(this.cardReaderGrid.CurrentDevice);
			}
			this.UpdateDeviceAsync(this.cardReaderGrid.CurrentDevice);
			this.UpdateShowErrorFileButton();
			if (this.cardReaderGrid.DisplayFileList())
			{
				this.isCurrentDeviceInvalid = false;
				this.buttonShowInfo.Enabled = true;
				this.buttonDeleteLogData.Enabled = !this.cardReaderGrid.IsCurrentDeviceCustomDirectory;
				if (this.cardReaderGrid.CurrentDevice.HasLoggerInfo)
				{
					long numberOfFiles = (long)((ulong)(this.cardReaderGrid.CurrentDevice.LogFileStorage.NumberOfTriggeredBuffers + this.cardReaderGrid.CurrentDevice.LogFileStorage.NumberOfRecordingBuffers + this.cardReaderGrid.CurrentDevice.LogFileStorage.NumberOfDriveRecorderFiles + this.cardReaderGrid.CurrentDevice.LogFileStorage.NumberOfJpegFiles + this.cardReaderGrid.CurrentDevice.LogFileStorage.NumberOfWavFiles + this.cardReaderGrid.CurrentDevice.LogFileStorage.NumberOfZipArchives));
					this.UpdateCapacityInformation(numberOfFiles, this.cardReaderGrid.CurrentDevice.LogFileStorage.FreeSpace, this.cardReaderGrid.CurrentDevice.LogFileStorage.TotalSpace);
				}
				else
				{
					long capacity = 0L;
					long freeSpace = 0L;
					long numberOfFiles2 = 0L;
					if (((IPropertyWindow)this).ModelValidator.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
					{
						try
						{
							DriveInfo driveInfo = new DriveInfo(this.cardReaderGrid.CurrentDevice.HardwareKey);
							DirectoryInfo directoryInfo = new DirectoryInfo(this.cardReaderGrid.CurrentDevice.HardwareKey);
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
			}
			else
			{
				this.ResetCapacityValues();
				this.ResetSnapshotButton();
				this.isCurrentDeviceInvalid = true;
			}
			this.EnableConversionAndSetInfoText(false);
		}

		public bool OpenSourceFolder(string folderPath, bool addToRecentlyUsedList = true)
		{
			bool flag = this.deviceListPopupMenuController.SelectDevice(folderPath);
			if (!flag)
			{
				flag = this.deviceListPopupMenuController.AddCustomFolderOrPackedImageDevice(folderPath);
			}
			return flag;
		}

		private bool EnableConversionAndSetInfoText(bool analysisPackageNotFound = false)
		{
			this.fileConversion.ConversionInfo.Links.Clear();
			if (this.cardReaderGrid.CurrentDevice != null && (this.cardReaderGrid.CurrentDevice.LogFileStorage.NumberOfTriggeredBuffers > 0u || this.cardReaderGrid.CurrentDevice.LogFileStorage.NumberOfRecordingBuffers > 0u) && !string.IsNullOrEmpty(this.cardReaderGrid.CurrentDevice.LogFileStorage.DestSubFolderNamePrimary))
			{
				if (FileConversionHelper.IsSignalOrientedDestFormat(this.destinationFormat))
				{
					if (analysisPackageNotFound)
					{
						this.fileConversion.ConversionInfo.Text = Resources.AutomaticSearchNoMatchingPackageFound;
						this.fileConversion.IsConversionEnabled = false;
						this.fileConversion.UpdateTabErrorIcons();
						return false;
					}
					bool allowFlexRay = !FileConversionHelper.UseMDFLegacyConversion(this.conversionParameters);
					if (!this.cardReaderGrid.ModelValidator.DatabaseServices.HasDatabasesAccessibleForSignalConversion(this.conversionParameters, allowFlexRay))
					{
						this.fileConversion.ConversionInfo.Text = string.Format(Resources.ErrorUnableToConvertToSignalOrientedFormat, this.destinationFormat.ToString());
						this.fileConversion.IsConversionEnabled = false;
						this.fileConversion.UpdateTabErrorIcons();
						return false;
					}
				}
				string text = Path.Combine(this.fileConversion.FileConversionParameters.DestinationFolder, this.cardReaderGrid.CurrentDevice.LogFileStorage.DestSubFolderNamePrimary);
				string text2 = string.Empty;
				bool isConversionEnabled = true;
				if (!string.IsNullOrEmpty(this.cardReaderGrid.ModelValidator.LoggerSpecifics.DataStorage.LogDataIniFileName) && !this.cardReaderGrid.CurrentDevice.HasLoggerInfo)
				{
					text2 = string.Format(Resources.ConversionInfoIniFileMissing, this.cardReaderGrid.ModelValidator.LoggerSpecifics.DataStorage.LogDataIniFileName) + " ";
				}
				else if (this.cardReaderGrid.ModelValidator.LoggerSpecifics.FileConversion.HasSelectableLogFiles)
				{
					int num = (from tmp in this.cardReaderGrid.CurrentDevice.LogFileStorage.LogFiles
					where tmp.IsSelected
					select tmp).Count<ILogFile>();
					text2 = string.Format(Resources.ConversionInfoCLFExport, num, this.cardReaderGrid.CurrentDevice.LogFileStorage.LogFiles.Count, this.GetTotalFileSizeOfLogFiles(from tmp in this.cardReaderGrid.CurrentDevice.LogFileStorage.LogFiles
					where tmp.IsSelected
					select tmp));
					isConversionEnabled = (num > 0);
				}
				else
				{
					int num2 = this.cardReaderGrid.CurrentDevice.LogFileStorage.LogFiles.Count<ILogFile>();
					text2 = string.Format(Resources.ConversionInfoFiles, num2, this.GetTotalFileSizeOfLogFiles(this.cardReaderGrid.CurrentDevice.LogFileStorage.LogFiles));
				}
				this.fileConversion.ConversionInfo.Text = string.Concat(new string[]
				{
					text2,
					" ",
					Resources.ConversionTo,
					" ",
					this.cardReaderGrid.CurrentDevice.LogFileStorage.DestSubFolderNamePrimary,
					"."
				});
				if (Directory.Exists(text))
				{
					int length = this.cardReaderGrid.CurrentDevice.LogFileStorage.DestSubFolderNamePrimary.Length;
					int start = this.fileConversion.ConversionInfo.Text.Length - length - 1;
					this.fileConversion.ConversionInfo.Links.Add(start, length, text);
				}
				this.fileConversion.IsConversionEnabled = isConversionEnabled;
				this.SplitButtonQuickView.Enabled = true;
			}
			else
			{
				this.fileConversion.ConversionInfo.Text = Resources.ConversionInfoNoFilesAvailable;
				this.fileConversion.IsConversionEnabled = false;
				this.SplitButtonQuickView.Enabled = false;
			}
			this.FireConversionEnabledChanged();
			return this.fileConversion.IsConversionEnabled;
		}

		private string GetTotalFileSizeOfLogFiles(IEnumerable<ILogFile> logFiles)
		{
			long num = 0L;
			foreach (ILogFile current in logFiles)
			{
				num += (long)((ulong)current.FileSize);
			}
			if (num < 1048576L && num > 0L)
			{
				return GUIUtil.GetSizeStringKBForBytes(num);
			}
			return GUIUtil.GetSizeStringMBForBytes(num);
		}

		public void ResetCapacityValues()
		{
			this.labelCapacity.Text = string.Empty;
			this.storageCapacityBar.Clear();
			this.storageCapacityBar.Visible = false;
		}

		private void UpdateCapacityInformation(long numberOfFiles, long freeSpace, long capacity)
		{
			if (this.HardwareFrontend != null && this.HardwareFrontend.CurrentLoggerDevices.Contains(this.cardReaderGrid.CurrentDevice))
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

		private void Convert()
		{
			if (this.cardReaderGrid.CurrentDevice.LogFileStorage.HasMixedCompUncompFiles && InformMessageBox.Question(Resources.MixedComprUncomprFilesOnCard) == DialogResult.No)
			{
				return;
			}
			if (!FileConversionHelper.PerformChecksForSignalOrientedDestFormat(this.destinationFormat, this.conversionParameters, this))
			{
				return;
			}
			this.isFolderChanged = false;
			this.isConverting = true;
			this.cardReaderGrid.CurrentDevice.LogFileStorage.ConvertAllLogFiles(this.conversionParameters, this.databaseConfiguration, this.configurationFolderPath);
			this.isConverting = false;
			if (this.isFolderChanged)
			{
				this.isFolderChanged = false;
				this.DisplayCurrentCardReaderContent();
			}
			this.FireConversionEnabledChanged();
		}

		private ILoggerDevice GetOrCreateSnapshotDevice(string snapshotFolderPath)
		{
			if (this.currentSnapshotDevice != null && string.Compare(this.currentSnapshotDevice.HardwareKey, snapshotFolderPath, true) == 0)
			{
				this.cardReaderGrid.HardwareFrontend.SetCustomLoggerDevice(this.currentSnapshotDevice, this);
				return this.currentSnapshotDevice;
			}
			this.currentSnapshotDevice = this.cardReaderGrid.HardwareFrontend.CreateCustomLoggerDevice(this.cardReaderGrid.CurrentDevice.LoggerSpecifics.Type, snapshotFolderPath, this);
			return this.currentSnapshotDevice;
		}

		private void ResetSnapshotButton()
		{
			this.checkBoxDisplay3GSnapshot.CheckedChanged -= new EventHandler(this.checkBoxDisplay3GSnapshot_CheckedChanged);
			this.checkBoxDisplay3GSnapshot.Checked = false;
			this.checkBoxDisplay3GSnapshot.Visible = false;
			this.checkBoxDisplay3GSnapshot.CheckedChanged += new EventHandler(this.checkBoxDisplay3GSnapshot_CheckedChanged);
		}

		private void OnDeleteAllFiles(object sender, EventArgs e)
		{
			this.DeleteAllFiles();
		}

		private void DeleteAllFiles()
		{
			if (this.cardReaderGrid.CurrentDevice != null)
			{
				bool flag = false;
				if (this.cardReaderGrid.CurrentDevice.HasSnapshotFolderContainingLogData)
				{
					DialogResult dialogResult = InformMessageBox.QuestionWithCancel(Resources.QuestionSnapshotFoundOnMediaDeleteToo);
					if (dialogResult == DialogResult.Yes)
					{
						if (!FileSystemServices.TryDeleteDirectory(this.cardReaderGrid.CurrentDevice.SnapshotFolderPath))
						{
							InformMessageBox.Error(string.Format(Resources.CannotDeleteDirectory, this.cardReaderGrid.CurrentDevice.SnapshotFolderPath));
						}
					}
					else if (dialogResult == DialogResult.Cancel)
					{
						return;
					}
				}
				else if (this.currentSnapshotDevice == this.cardReaderGrid.CurrentDevice)
				{
					flag = true;
				}
				this.cardReaderGrid.CurrentDevice.LogFileStorage.DeleteAllLogFiles();
				if (!this.cardReaderGrid.CurrentDevice.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
				{
					this.cardReaderGrid.HardwareFrontend.EnforceExplicitUpdateOfLoggerDevice(this.cardReaderGrid.CurrentDevice);
				}
				if (flag)
				{
					this.checkBoxDisplay3GSnapshot.Checked = false;
				}
			}
		}

		private void ShowErrorFile()
		{
			if (this.cardReaderGrid.CurrentDevice != null && this.cardReaderGrid.CurrentDevice.HasErrorFile)
			{
				string text = Path.Combine(this.cardReaderGrid.CurrentDevice.HardwareKey, this.cardReaderGrid.ModelValidator.LoggerSpecifics.DataStorage.ErrorFilePath);
				string tempDirectoryName;
				if (this.cardReaderGrid.CurrentDevice.IsZipArchive && TempDirectoryManager.Instance.CreateNewTempDirectory(out tempDirectoryName))
				{
					string text2 = Path.Combine(TempDirectoryManager.Instance.GetFullTempDirectoryPath(tempDirectoryName), this.cardReaderGrid.ModelValidator.LoggerSpecifics.DataStorage.ErrorFilePath);
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
			if (this.cardReaderGrid.CurrentDevice != null)
			{
				this.buttonShowErrorFile.Visible = this.cardReaderGrid.CurrentDevice.HasErrorFile;
			}
		}

		private void OnExportFileEnabledChanged(object sender, EventArgs e)
		{
			this.EnableConversionAndSetInfoText(false);
		}

		private void CheckAnalysisPackage()
		{
			ILoggerDevice loggerDevice = this.isCurrentDeviceInvalid ? null : this.currentActualDevice;
			bool flag = AnalysisPackage.SearchAndLoadAnalysisPackage(loggerDevice, this.fileConversion.ExportDatabases);
			this.fileConversion.ExportDatabases.ValidateInput();
			this.EnableConversionAndSetInfoText(!flag);
		}

		public bool Serialize(CardReaderPage cardReaderPage)
		{
			if (cardReaderPage == null)
			{
				return false;
			}
			cardReaderPage.FileConversionParameters = this.conversionParameters;
			cardReaderPage.SelectedDevicePath = this.selectedDevicePath;
			bool flag = true;
			return flag & this.cardReaderGrid.Serialize(cardReaderPage);
		}

		public bool DeSerialize(CardReaderPage cardReaderPage)
		{
			if (cardReaderPage == null)
			{
				return false;
			}
			if (cardReaderPage.FileConversionParameters != null)
			{
				this.conversionParameters = cardReaderPage.FileConversionParameters;
			}
			this.selectedDevicePath = cardReaderPage.SelectedDevicePath;
			bool flag = true;
			return flag & this.cardReaderGrid.DeSerialize(cardReaderPage);
		}

		public OfflineSourceConfig GetOfflineSourceConfig(bool isLocalContext, bool isCANoeCANalyzer)
		{
			OfflineSourceConfig offlineSourceConfig = new OfflineSourceConfig();
			if (!isLocalContext && this.currentActualDevice != null)
			{
				List<ILogFile> source = (from logFile in this.currentActualDevice.LogFileStorage.LogFiles
				where ".glx".Equals(Path.GetExtension(logFile.FullFilePath), StringComparison.OrdinalIgnoreCase)
				select logFile).ToList<ILogFile>();
				if (source.Any<ILogFile>())
				{
					ILogFile logFile2 = source.First<ILogFile>();
					offlineSourceConfig.OfflineSourceFiles.Add(new Tuple<string, string>(logFile2.DefaultName, logFile2.FullFilePath));
				}
			}
			return offlineSourceConfig;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CardReader));
			this.groupBoxSourceFolder = new GroupBox();
			this.cardReaderGrid = new CardReaderGrid();
			this.tableLayoutPanelSourceFolder = new TableLayoutPanel();
			this.buttonShowInfo = new Button();
			this.labelCapacity = new Label();
			this.buttonDeleteLogData = new Button();
			this.checkBoxDisplay3GSnapshot = new CheckBox();
			this.buttonShowErrorFile = new Button();
			this.buttonEject = new Button();
			this.dropDownButtonDeviceList = new DropDownButton();
			this.popupMenuDeviceList = new PopupMenu(this.components);
			this.barManagerDeviceList = new BarManager(this.components);
			this.bar1 = new Bar();
			this.bar2 = new Bar();
			this.bar3 = new Bar();
			this.barDockControlTop = new BarDockControl();
			this.barDockControlBottom = new BarDockControl();
			this.barDockControlLeft = new BarDockControl();
			this.barDockControlRight = new BarDockControl();
			this.splitButtonQuickView = new SplitButton();
			this.labelQuickView = new Label();
			this.storageCapacityBar = new StorageCapacityBar();
			this.toolTip = new ToolTip(this.components);
			this.fileConversion = new FileConversion();
			this.groupBoxSourceFolder.SuspendLayout();
			this.tableLayoutPanelSourceFolder.SuspendLayout();
			((ISupportInitialize)this.popupMenuDeviceList).BeginInit();
			((ISupportInitialize)this.barManagerDeviceList).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxSourceFolder, "groupBoxSourceFolder");
			this.groupBoxSourceFolder.Controls.Add(this.cardReaderGrid);
			this.groupBoxSourceFolder.Controls.Add(this.tableLayoutPanelSourceFolder);
			this.groupBoxSourceFolder.Name = "groupBoxSourceFolder";
			this.groupBoxSourceFolder.TabStop = false;
			this.groupBoxSourceFolder.DragDrop += new DragEventHandler(this.groupBoxSourceFolder_DragDrop);
			this.groupBoxSourceFolder.DragEnter += new DragEventHandler(this.groupBoxSourceFolder_DragEnter);
			componentResourceManager.ApplyResources(this.cardReaderGrid, "cardReaderGrid");
			this.cardReaderGrid.CurrentDevice = null;
			this.cardReaderGrid.DestinationFolder = null;
			this.cardReaderGrid.HardwareFrontend = null;
			this.cardReaderGrid.IsCurrentDeviceCustomDirectory = true;
			this.cardReaderGrid.ModelValidator = null;
			this.cardReaderGrid.Name = "cardReaderGrid";
			componentResourceManager.ApplyResources(this.tableLayoutPanelSourceFolder, "tableLayoutPanelSourceFolder");
			this.tableLayoutPanelSourceFolder.Controls.Add(this.buttonShowInfo, 6, 0);
			this.tableLayoutPanelSourceFolder.Controls.Add(this.labelCapacity, 0, 1);
			this.tableLayoutPanelSourceFolder.Controls.Add(this.buttonDeleteLogData, 6, 1);
			this.tableLayoutPanelSourceFolder.Controls.Add(this.checkBoxDisplay3GSnapshot, 5, 0);
			this.tableLayoutPanelSourceFolder.Controls.Add(this.buttonShowErrorFile, 5, 1);
			this.tableLayoutPanelSourceFolder.Controls.Add(this.buttonEject, 4, 0);
			this.tableLayoutPanelSourceFolder.Controls.Add(this.dropDownButtonDeviceList, 0, 0);
			this.tableLayoutPanelSourceFolder.Controls.Add(this.splitButtonQuickView, 3, 1);
			this.tableLayoutPanelSourceFolder.Controls.Add(this.labelQuickView, 2, 1);
			this.tableLayoutPanelSourceFolder.Controls.Add(this.storageCapacityBar, 1, 1);
			this.tableLayoutPanelSourceFolder.Name = "tableLayoutPanelSourceFolder";
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
			this.dropDownButtonDeviceList.AllowHtmlDraw = DefaultBoolean.True;
			componentResourceManager.ApplyResources(this.dropDownButtonDeviceList, "dropDownButtonDeviceList");
			this.dropDownButtonDeviceList.Appearance.Options.UseTextOptions = true;
			this.dropDownButtonDeviceList.Appearance.TextOptions.HAlignment = HorzAlignment.Near;
			this.tableLayoutPanelSourceFolder.SetColumnSpan(this.dropDownButtonDeviceList, 4);
			this.dropDownButtonDeviceList.DropDownArrowStyle = DropDownArrowStyle.Show;
			this.dropDownButtonDeviceList.DropDownControl = this.popupMenuDeviceList;
			this.dropDownButtonDeviceList.MenuManager = this.barManagerDeviceList;
			this.dropDownButtonDeviceList.Name = "dropDownButtonDeviceList";
			this.popupMenuDeviceList.Manager = this.barManagerDeviceList;
			this.popupMenuDeviceList.Name = "popupMenuDeviceList";
			this.barManagerDeviceList.Bars.AddRange(new Bar[]
			{
				this.bar1,
				this.bar2,
				this.bar3
			});
			this.barManagerDeviceList.DockControls.Add(this.barDockControlTop);
			this.barManagerDeviceList.DockControls.Add(this.barDockControlBottom);
			this.barManagerDeviceList.DockControls.Add(this.barDockControlLeft);
			this.barManagerDeviceList.DockControls.Add(this.barDockControlRight);
			this.barManagerDeviceList.Form = this;
			this.barManagerDeviceList.MainMenu = this.bar2;
			this.barManagerDeviceList.MaxItemId = 0;
			this.barManagerDeviceList.StatusBar = this.bar3;
			this.bar1.BarName = "Tools";
			this.bar1.DockCol = 0;
			this.bar1.DockRow = 1;
			this.bar1.DockStyle = BarDockStyle.Top;
			componentResourceManager.ApplyResources(this.bar1, "bar1");
			this.bar2.BarName = "Main menu";
			this.bar2.DockCol = 0;
			this.bar2.DockRow = 0;
			this.bar2.DockStyle = BarDockStyle.Top;
			this.bar2.OptionsBar.MultiLine = true;
			this.bar2.OptionsBar.UseWholeRow = true;
			componentResourceManager.ApplyResources(this.bar2, "bar2");
			this.bar3.BarName = "Status bar";
			this.bar3.CanDockStyle = BarCanDockStyle.Bottom;
			this.bar3.DockCol = 0;
			this.bar3.DockRow = 0;
			this.bar3.DockStyle = BarDockStyle.Bottom;
			this.bar3.OptionsBar.AllowQuickCustomization = false;
			this.bar3.OptionsBar.DrawDragBorder = false;
			this.bar3.OptionsBar.UseWholeRow = true;
			componentResourceManager.ApplyResources(this.bar3, "bar3");
			this.barDockControlTop.CausesValidation = false;
			componentResourceManager.ApplyResources(this.barDockControlTop, "barDockControlTop");
			this.barDockControlBottom.CausesValidation = false;
			componentResourceManager.ApplyResources(this.barDockControlBottom, "barDockControlBottom");
			this.barDockControlLeft.CausesValidation = false;
			componentResourceManager.ApplyResources(this.barDockControlLeft, "barDockControlLeft");
			this.barDockControlRight.CausesValidation = false;
			componentResourceManager.ApplyResources(this.barDockControlRight, "barDockControlRight");
			componentResourceManager.ApplyResources(this.splitButtonQuickView, "splitButtonQuickView");
			this.splitButtonQuickView.Name = "splitButtonQuickView";
			this.splitButtonQuickView.ShowSplitAlways = true;
			this.splitButtonQuickView.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.labelQuickView, "labelQuickView");
			this.labelQuickView.Name = "labelQuickView";
			componentResourceManager.ApplyResources(this.storageCapacityBar, "storageCapacityBar");
			this.storageCapacityBar.Name = "storageCapacityBar";
			this.fileConversion.AnalysisPackageParameters = null;
			componentResourceManager.ApplyResources(this.fileConversion, "fileConversion");
			this.fileConversion.ConfigurationManagerService = null;
			this.fileConversion.FileConversionParameters = null;
			this.fileConversion.IsConversionEnabled = true;
			this.fileConversion.LoggerSpecifics = null;
			this.fileConversion.Name = "fileConversion";
			this.fileConversion.SourceFolder = null;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.fileConversion);
			base.Controls.Add(this.groupBoxSourceFolder);
			base.Controls.Add(this.barDockControlLeft);
			base.Controls.Add(this.barDockControlRight);
			base.Controls.Add(this.barDockControlBottom);
			base.Controls.Add(this.barDockControlTop);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "CardReader";
			this.groupBoxSourceFolder.ResumeLayout(false);
			this.tableLayoutPanelSourceFolder.ResumeLayout(false);
			this.tableLayoutPanelSourceFolder.PerformLayout();
			((ISupportInitialize)this.popupMenuDeviceList).EndInit();
			((ISupportInitialize)this.barManagerDeviceList).EndInit();
			base.ResumeLayout(false);
		}
	}
}
