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

namespace Vector.VLConfig.GUI.LoggerDevicePage
{
	public class LoggerDevice : UserControl, IUpdateObserver<DatabaseConfiguration>, IUpdateObserver<LoggerType>, IUpdateObserver, IPropertyWindow, IConfigClipboardClient, IHardwareFrontendClient, IFileConversionParametersClient, IQuickViewClient
	{
		public delegate void DeviceChangedDelegate(object sender, EventArgs e);

		public delegate void ConversionEnabledChangedDelegate(object sender, EventArgs e);

		private FileConversionParameters conversionParameters;

		private string destinationFolder;

		private FileConversionDestFormat destinationFormat;

		private DatabaseConfiguration databaseConfiguration;

		private string configurationFolderPath;

		private bool isCurrentDeviceInvalid;

		private ILoggerDevice currentActualDevice;

		private ILoggerDevice currentSnapshotDevice;

		private string deviceWithAnalysisPackage;

		private readonly QuickView quickView;

		private IContainer components;

		private FileConversion fileConversion;

		private GroupBox groupBoxLoggerDevice;

		private LoggerDeviceGrid loggerDeviceGrid;

		private TableLayoutPanel tableLayoutPanel1;

		private Label labelCapacity;

		private Button buttonDeleteLogData;

		private CheckBox checkBoxDisplay3GSnapshot;

		private Button buttonEject;

		private Button buttonShowErrorFile;

		private ToolTip toolTip;

		private SplitButton splitButtonQuickView;

		private Label labelQuickView;

		private StorageCapacityBar storageCapacityBar;

		public event LoggerDevice.DeviceChangedDelegate DeviceChanged;

		public event LoggerDevice.ConversionEnabledChangedDelegate ConversionEnabledChanged;

		IModelValidator IPropertyWindow.ModelValidator
		{
			get
			{
				return this.loggerDeviceGrid.ModelValidator;
			}
			set
			{
				this.loggerDeviceGrid.ModelValidator = value;
				this.fileConversion.Init(this.loggerDeviceGrid.ModelValidator.LoggerSpecifics, false, this);
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
				return PageType.LoggerDevice;
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
				}
			}
		}

		public IHardwareFrontend HardwareFrontend
		{
			get
			{
				return this.loggerDeviceGrid.HardwareFrontend;
			}
			set
			{
				this.loggerDeviceGrid.HardwareFrontend = value;
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
				this.OnFileConversionParametersChanged(this, new FileConversionParametersChangedEventArgs(FileConversionParametersChangedEventArgs.Parameter.All));
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
				return this.loggerDeviceGrid.ModelValidator.LoggerSpecifics;
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

		public LoggerDevice()
		{
			this.InitializeComponent();
			this.loggerDeviceGrid.DeleteAllFiles += new EventHandler(this.OnDeleteAllFiles);
			this.loggerDeviceGrid.FileIsSelectedChanged += new EventHandler(this.OnExportFileEnabledChanged);
			this.fileConversion.ExportDatabases.StatusChanged += new ExportDatabases.StatusChangedHandler(this.ExportDatabasesStatusChanged);
			this.fileConversion.ExportDatabases.FileConversionParametersChanged += new FileConversion.FileConversionParametersChangedHandler(this.OnFileConversionParametersChanged);
			this.deviceWithAnalysisPackage = string.Empty;
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
			this.loggerDeviceGrid.DestinationFolder = this.conversionParameters.DestinationFolder;
			this.fileConversion.FileConversionParameters = this.conversionParameters;
			this.destinationFolder = this.conversionParameters.DestinationFolder;
			this.destinationFormat = this.conversionParameters.DestinationFormat;
			this.ApplyLoggerSpecifics(this.loggerDeviceGrid.ModelValidator.LoggerSpecifics);
			this.fileConversion.ParametersChanged += new FileConversion.FileConversionParametersChangedHandler(this.OnFileConversionParametersChanged);
			this.fileConversion.StartConversion += new FileConversion.StartConversionHandler(this.OnStartConversion);
			this.fileConversion.IsConversionEnabled = false;
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
			if (this.loggerDeviceGrid.CurrentDevice != device)
			{
				return;
			}
			this.DisplayCurrentLoggerDeviceContent();
		}

		public void DevicesAdded(IList<ILoggerDevice> devices)
		{
			if (this.currentActualDevice == null && this.HardwareFrontend.PrimaryOnlineDevice != null && devices.Contains(this.HardwareFrontend.PrimaryOnlineDevice))
			{
				this.SetCurrentActualLoggerDevice(this.HardwareFrontend.PrimaryOnlineDevice);
			}
		}

		public void DevicesRemoved(IList<ILoggerDevice> devices)
		{
			if (this.currentActualDevice != null && devices.Contains(this.currentActualDevice))
			{
				if (this.HardwareFrontend.PrimaryOnlineDevice != null)
				{
					this.SetCurrentActualLoggerDevice(this.HardwareFrontend.PrimaryOnlineDevice);
					return;
				}
				this.SetCurrentActualLoggerDevice(null);
			}
		}

		public void AdditionalDrivesListChanged(IList<string> additionalDrivesList)
		{
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

		void IUpdateObserver<DatabaseConfiguration>.Update(DatabaseConfiguration data)
		{
			this.databaseConfiguration = data;
			this.EnableConversionAndSetInfoText(false);
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType type)
		{
			this.ApplyLoggerSpecifics(this.loggerDeviceGrid.ModelValidator.LoggerSpecifics);
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

		public void FireConversionEnabledChanged()
		{
			if (this.ConversionEnabledChanged != null)
			{
				this.ConversionEnabledChanged(this, new EventArgs());
			}
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
					this.loggerDeviceGrid.DestinationFolder = this.conversionParameters.DestinationFolder;
					this.destinationFolder = this.conversionParameters.DestinationFolder;
				}
				else if (fileConversionParametersChangedEventArgs.ChangedParameter == FileConversionParametersChangedEventArgs.Parameter.All)
				{
					if (this.destinationFormat != this.fileConversion.ExportDatabases.FileConversionParameters.DestinationFormat)
					{
						this.fileConversion.ExportDatabases.FileConversionParameters.DestinationFormat = this.destinationFormat;
					}
					this.loggerDeviceGrid.DestinationFolder = this.conversionParameters.DestinationFolder;
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
			this.DisplayCurrentLoggerDeviceContent();
			bool flag = true;
			if (this.isCurrentDeviceInvalid)
			{
				InformMessageBox.Error(Resources.ErrorUnableToAccessDeviceWillUpdate);
				this.HardwareFrontend.UpdateLoggerDeviceList();
				flag = false;
			}
			if (!flag)
			{
				return;
			}
			this.Convert();
			this.loggerDeviceGrid.Refresh();
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

		private void checkBoxDisplay3GSnapshot_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox == null)
			{
				return;
			}
			if (checkBox.Checked)
			{
				if (this.currentActualDevice.HasSnapshotFolderContainingLogData)
				{
					this.loggerDeviceGrid.CurrentDevice = this.GetOrCreateSnapshotDevice(this.loggerDeviceGrid.CurrentDevice.SnapshotFolderPath);
				}
				else
				{
					InformMessageBox.Error(Resources.ErrorSnapshotNotAvailable);
					this.loggerDeviceGrid.CurrentDevice = this.currentActualDevice;
					this.HardwareFrontend.ReleaseCustomLoggerDevice(this);
					this.checkBoxDisplay3GSnapshot.Visible = false;
				}
			}
			else
			{
				this.loggerDeviceGrid.CurrentDevice = this.currentActualDevice;
				this.HardwareFrontend.ReleaseCustomLoggerDevice(this);
			}
			this.DisplayCurrentLoggerDeviceContent();
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

		private void buttonEject_Click(object sender, EventArgs e)
		{
			if (!this.isCurrentDeviceInvalid)
			{
				this.HardwareFrontend.Eject(this.currentActualDevice);
			}
		}

		private void buttonEject_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.buttonEject, Resources.EjectDevice);
		}

		private void buttonShowErrorFile_Click(object sender, EventArgs e)
		{
			this.ShowErrorFile();
		}

		private void buttonShowErrorFile_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.buttonShowErrorFile, string.Format(Resources.ErrorFileFoundClickToOpen, Path.GetFileName(this.loggerDeviceGrid.ModelValidator.LoggerSpecifics.DataStorage.ErrorFilePath)));
		}

		public void ExportDatabasesStatusChanged()
		{
			this.EnableConversionAndSetInfoText(false);
		}

		private void ApplyLoggerSpecifics(ILoggerSpecifics loggerSpecifics)
		{
			if (this.HardwareFrontend.IsScanForAllLoggerTypesEnabled && this.loggerDeviceGrid.ModelValidator.LoggerSpecifics.Type != loggerSpecifics.Type)
			{
				this.loggerDeviceGrid.ModelValidator.LoggerSpecifics = loggerSpecifics;
			}
			this.fileConversion.LoggerSpecifics = loggerSpecifics;
			this.destinationFolder = this.conversionParameters.DestinationFolder;
			this.destinationFormat = this.conversionParameters.DestinationFormat;
			this.quickView.ApplyLoggerSpecifics();
		}

		private void SetCurrentActualLoggerDevice(ILoggerDevice device)
		{
			this.currentActualDevice = device;
			this.loggerDeviceGrid.CurrentDevice = device;
			this.ResetSnapshotButton();
			this.currentSnapshotDevice = null;
			if (this.currentActualDevice != null)
			{
				this.checkBoxDisplay3GSnapshot.Visible = this.currentActualDevice.HasSnapshotFolderContainingLogData;
				this.ApplyLoggerSpecifics(this.currentActualDevice.LoggerSpecifics);
			}
			this.DisplayCurrentLoggerDeviceContent();
			this.fileConversion.RefreshStatus();
		}

		private void DisplayCurrentLoggerDeviceContent()
		{
			this.buttonDeleteLogData.Enabled = false;
			this.buttonEject.Enabled = false;
			if (!this.loggerDeviceGrid.ModelValidator.LoggerSpecifics.DeviceAccess.IsUSBConnectionSupported)
			{
				this.loggerDeviceGrid.Enabled = false;
				this.groupBoxLoggerDevice.Text = Resources.NotAvailableForCurrentLoggerType;
				this.fileConversion.Enabled = false;
			}
			else
			{
				this.loggerDeviceGrid.Enabled = true;
				if (this.checkBoxDisplay3GSnapshot.Visible && this.checkBoxDisplay3GSnapshot.Checked)
				{
					this.groupBoxLoggerDevice.Text = Resources.LoggerDevice + Resources.SnapshotDataTitlePostfix;
				}
				else
				{
					this.groupBoxLoggerDevice.Text = Resources.LoggerDevice;
				}
				this.fileConversion.Enabled = true;
			}
			this.UpdateDeviceAsync(this.loggerDeviceGrid.CurrentDevice);
			this.UpdateShowErrorFileButton();
			if (this.loggerDeviceGrid.DisplayFileList())
			{
				this.isCurrentDeviceInvalid = false;
				this.buttonDeleteLogData.Enabled = true;
				this.buttonEject.Enabled = true;
				if (this.loggerDeviceGrid.CurrentDevice.HasLoggerInfo)
				{
					long numberOfFiles = (long)((ulong)(this.loggerDeviceGrid.CurrentDevice.LogFileStorage.NumberOfTriggeredBuffers + this.loggerDeviceGrid.CurrentDevice.LogFileStorage.NumberOfRecordingBuffers + this.loggerDeviceGrid.CurrentDevice.LogFileStorage.NumberOfDriveRecorderFiles + this.loggerDeviceGrid.CurrentDevice.LogFileStorage.NumberOfJpegFiles + this.loggerDeviceGrid.CurrentDevice.LogFileStorage.NumberOfWavFiles + this.loggerDeviceGrid.CurrentDevice.LogFileStorage.NumberOfZipArchives));
					this.UpdateCapacityInformation(numberOfFiles, this.loggerDeviceGrid.CurrentDevice.LogFileStorage.FreeSpace, this.loggerDeviceGrid.CurrentDevice.LogFileStorage.TotalSpace);
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
							DriveInfo driveInfo = new DriveInfo(this.loggerDeviceGrid.CurrentDevice.HardwareKey);
							DirectoryInfo directoryInfo = new DirectoryInfo(this.loggerDeviceGrid.CurrentDevice.HardwareKey);
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
				this.isCurrentDeviceInvalid = true;
			}
			if (((IPropertyWindow)this).IsVisible)
			{
				this.CheckAnalysisPackage();
			}
			this.FireDeviceChanged();
		}

		private void OnDeleteAllFiles(object sender, EventArgs e)
		{
			this.DeleteAllFiles();
		}

		private void DeleteAllFiles()
		{
			if (this.loggerDeviceGrid.CurrentDevice != null)
			{
				bool flag = false;
				if (this.loggerDeviceGrid.CurrentDevice.HasSnapshotFolderContainingLogData)
				{
					DialogResult dialogResult = InformMessageBox.QuestionWithCancel(Resources.QuestionSnapshotFoundOnMediaDeleteToo);
					if (dialogResult == DialogResult.Yes)
					{
						if (!FileSystemServices.TryDeleteDirectory(this.loggerDeviceGrid.CurrentDevice.SnapshotFolderPath))
						{
							InformMessageBox.Error(string.Format(Resources.CannotDeleteDirectory, this.loggerDeviceGrid.CurrentDevice.SnapshotFolderPath));
						}
					}
					else if (dialogResult == DialogResult.Cancel)
					{
						return;
					}
				}
				else if (this.currentSnapshotDevice == this.loggerDeviceGrid.CurrentDevice)
				{
					flag = true;
				}
				this.loggerDeviceGrid.CurrentDevice.LogFileStorage.DeleteAllLogFiles();
				if (!this.loggerDeviceGrid.CurrentDevice.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
				{
					this.loggerDeviceGrid.HardwareFrontend.EnforceExplicitUpdateOfLoggerDevice(this.loggerDeviceGrid.CurrentDevice);
				}
				if (flag)
				{
					this.checkBoxDisplay3GSnapshot.Checked = false;
				}
			}
		}

		private void ShowErrorFile()
		{
			if (this.loggerDeviceGrid.CurrentDevice != null && this.loggerDeviceGrid.CurrentDevice.HasErrorFile)
			{
				string text = Path.Combine(this.loggerDeviceGrid.CurrentDevice.HardwareKey, this.loggerDeviceGrid.ModelValidator.LoggerSpecifics.DataStorage.ErrorFilePath);
				if (File.Exists(text))
				{
					FileSystemServices.LaunchFile(text);
				}
			}
		}

		private void UpdateShowErrorFileButton()
		{
			this.buttonShowErrorFile.Visible = false;
			if (this.loggerDeviceGrid.CurrentDevice != null)
			{
				this.buttonShowErrorFile.Visible = this.loggerDeviceGrid.CurrentDevice.HasErrorFile;
			}
		}

		private void OnExportFileEnabledChanged(object sender, EventArgs e)
		{
			this.EnableConversionAndSetInfoText(false);
		}

		private bool EnableConversionAndSetInfoText(bool analysisPackageNotFound = false)
		{
			this.fileConversion.ConversionInfo.Links.Clear();
			if (this.loggerDeviceGrid.CurrentDevice != null && (this.loggerDeviceGrid.CurrentDevice.LogFileStorage.NumberOfTriggeredBuffers > 0u || this.loggerDeviceGrid.CurrentDevice.LogFileStorage.NumberOfRecordingBuffers > 0u))
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
					if (!this.loggerDeviceGrid.ModelValidator.DatabaseServices.HasDatabasesAccessibleForSignalConversion(this.conversionParameters, allowFlexRay))
					{
						this.fileConversion.ConversionInfo.Text = string.Format(Resources.ErrorUnableToConvertToSignalOrientedFormat, this.destinationFormat.ToString());
						this.fileConversion.IsConversionEnabled = false;
						this.fileConversion.UpdateTabErrorIcons();
						return false;
					}
				}
				string text = Path.Combine(this.fileConversion.FileConversionParameters.DestinationFolder, this.loggerDeviceGrid.CurrentDevice.LogFileStorage.DestSubFolderNamePrimary);
				string text2 = string.Empty;
				bool isConversionEnabled = true;
				if (!string.IsNullOrEmpty(this.loggerDeviceGrid.ModelValidator.LoggerSpecifics.DataStorage.LogDataIniFileName) && !this.loggerDeviceGrid.CurrentDevice.HasLoggerInfo)
				{
					text2 = string.Format(Resources.ConversionInfoIniFileMissing, this.loggerDeviceGrid.ModelValidator.LoggerSpecifics.DataStorage.LogDataIniFileName) + " ";
				}
				else if (this.loggerDeviceGrid.ModelValidator.LoggerSpecifics.FileConversion.HasSelectableLogFiles)
				{
					int num = (from tmp in this.loggerDeviceGrid.CurrentDevice.LogFileStorage.LogFiles
					where tmp.IsSelected
					select tmp).Count<ILogFile>();
					text2 = string.Format(Resources.ConversionInfoCLFExport, num, this.loggerDeviceGrid.CurrentDevice.LogFileStorage.LogFiles.Count, this.GetTotalFileSizeOfLogFiles(from tmp in this.loggerDeviceGrid.CurrentDevice.LogFileStorage.LogFiles
					where tmp.IsSelected
					select tmp));
					isConversionEnabled = (num > 0);
				}
				else
				{
					int num2 = this.loggerDeviceGrid.CurrentDevice.LogFileStorage.LogFiles.Count<ILogFile>();
					text2 = string.Format(Resources.ConversionInfoFiles, num2, this.GetTotalFileSizeOfLogFiles(this.loggerDeviceGrid.CurrentDevice.LogFileStorage.LogFiles));
				}
				this.fileConversion.ConversionInfo.Text = string.Concat(new string[]
				{
					text2,
					" ",
					Resources.ConversionTo,
					" ",
					this.loggerDeviceGrid.CurrentDevice.LogFileStorage.DestSubFolderNamePrimary,
					"."
				});
				if (Directory.Exists(text))
				{
					int length = this.loggerDeviceGrid.CurrentDevice.LogFileStorage.DestSubFolderNamePrimary.Length;
					int start = this.fileConversion.ConversionInfo.Text.Length - length - 1;
					this.fileConversion.ConversionInfo.Links.Add(start, length, text);
				}
				this.fileConversion.IsConversionEnabled = isConversionEnabled;
				this.SplitButtonQuickView.Enabled = true;
			}
			else
			{
				this.fileConversion.ConversionInfo.Text = Resources.ConversionInfoNoFilesAvailable;
				this.fileConversion.ConversionInfo.Links.Clear();
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
			if (this.HardwareFrontend != null && this.HardwareFrontend.CurrentLoggerDevices.Contains(this.loggerDeviceGrid.CurrentDevice))
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
			if (this.loggerDeviceGrid.CurrentDevice.LogFileStorage.HasMixedCompUncompFiles && InformMessageBox.Question(Resources.MixedComprUncomprFilesOnCard) == DialogResult.No)
			{
				return;
			}
			if (!FileConversionHelper.PerformChecksForSignalOrientedDestFormat(this.destinationFormat, this.conversionParameters, this))
			{
				return;
			}
			this.loggerDeviceGrid.CurrentDevice.LogFileStorage.ConvertAllLogFiles(this.conversionParameters, this.databaseConfiguration, this.ConfigurationFolderPath);
		}

		private ILoggerDevice GetOrCreateSnapshotDevice(string snapshotFolderPath)
		{
			if (this.currentSnapshotDevice != null && string.Compare(this.currentSnapshotDevice.HardwareKey, snapshotFolderPath, true) == 0)
			{
				this.HardwareFrontend.SetCustomLoggerDevice(this.currentSnapshotDevice, this);
				return this.currentSnapshotDevice;
			}
			this.currentSnapshotDevice = this.loggerDeviceGrid.HardwareFrontend.CreateCustomLoggerDevice(this.loggerDeviceGrid.CurrentDevice.LoggerSpecifics.Type, snapshotFolderPath, this);
			return this.currentSnapshotDevice;
		}

		private void ResetSnapshotButton()
		{
			this.checkBoxDisplay3GSnapshot.CheckedChanged -= new EventHandler(this.checkBoxDisplay3GSnapshot_CheckedChanged);
			this.checkBoxDisplay3GSnapshot.Checked = false;
			this.checkBoxDisplay3GSnapshot.Visible = false;
			this.checkBoxDisplay3GSnapshot.CheckedChanged += new EventHandler(this.checkBoxDisplay3GSnapshot_CheckedChanged);
		}

		private void CheckAnalysisPackage()
		{
			ILoggerDevice loggerDevice = this.isCurrentDeviceInvalid ? null : this.currentActualDevice;
			bool flag = AnalysisPackage.SearchAndLoadAnalysisPackage(loggerDevice, this.fileConversion.ExportDatabases);
			this.fileConversion.ExportDatabases.ValidateInput();
			this.EnableConversionAndSetInfoText(!flag);
		}

		public bool Serialize(LoggerDevicePage loggerDevicePage)
		{
			if (loggerDevicePage == null)
			{
				return false;
			}
			loggerDevicePage.FileConversionParameters = this.conversionParameters;
			bool flag = true;
			return flag & this.loggerDeviceGrid.Serialize(loggerDevicePage);
		}

		public bool DeSerialize(LoggerDevicePage loggerDevicePage)
		{
			if (loggerDevicePage == null)
			{
				return false;
			}
			if (loggerDevicePage.FileConversionParameters != null)
			{
				this.conversionParameters = loggerDevicePage.FileConversionParameters;
			}
			bool flag = true;
			return flag & this.loggerDeviceGrid.DeSerialize(loggerDevicePage);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(LoggerDevice));
			this.groupBoxLoggerDevice = new GroupBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.buttonEject = new Button();
			this.buttonDeleteLogData = new Button();
			this.checkBoxDisplay3GSnapshot = new CheckBox();
			this.buttonShowErrorFile = new Button();
			this.splitButtonQuickView = new SplitButton();
			this.labelQuickView = new Label();
			this.labelCapacity = new Label();
			this.storageCapacityBar = new StorageCapacityBar();
			this.loggerDeviceGrid = new LoggerDeviceGrid();
			this.toolTip = new ToolTip(this.components);
			this.fileConversion = new FileConversion();
			this.groupBoxLoggerDevice.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxLoggerDevice, "groupBoxLoggerDevice");
			this.groupBoxLoggerDevice.Controls.Add(this.tableLayoutPanel1);
			this.groupBoxLoggerDevice.Controls.Add(this.loggerDeviceGrid);
			this.groupBoxLoggerDevice.Name = "groupBoxLoggerDevice";
			this.groupBoxLoggerDevice.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.buttonEject, 6, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonDeleteLogData, 7, 0);
			this.tableLayoutPanel1.Controls.Add(this.checkBoxDisplay3GSnapshot, 5, 0);
			this.tableLayoutPanel1.Controls.Add(this.buttonShowErrorFile, 4, 0);
			this.tableLayoutPanel1.Controls.Add(this.splitButtonQuickView, 3, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelQuickView, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.labelCapacity, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.storageCapacityBar, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.buttonEject, "buttonEject");
			this.buttonEject.Image = Resources.ImageEject;
			this.buttonEject.Name = "buttonEject";
			this.buttonEject.UseVisualStyleBackColor = true;
			this.buttonEject.Click += new EventHandler(this.buttonEject_Click);
			this.buttonEject.MouseEnter += new EventHandler(this.buttonEject_MouseEnter);
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
			componentResourceManager.ApplyResources(this.splitButtonQuickView, "splitButtonQuickView");
			this.splitButtonQuickView.Name = "splitButtonQuickView";
			this.splitButtonQuickView.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.labelQuickView, "labelQuickView");
			this.labelQuickView.Name = "labelQuickView";
			componentResourceManager.ApplyResources(this.labelCapacity, "labelCapacity");
			this.labelCapacity.Name = "labelCapacity";
			componentResourceManager.ApplyResources(this.storageCapacityBar, "storageCapacityBar");
			this.storageCapacityBar.Name = "storageCapacityBar";
			componentResourceManager.ApplyResources(this.loggerDeviceGrid, "loggerDeviceGrid");
			this.loggerDeviceGrid.CurrentDevice = null;
			this.loggerDeviceGrid.DestinationFolder = null;
			this.loggerDeviceGrid.HardwareFrontend = null;
			this.loggerDeviceGrid.ModelValidator = null;
			this.loggerDeviceGrid.Name = "loggerDeviceGrid";
			this.fileConversion.AnalysisPackageParameters = null;
			componentResourceManager.ApplyResources(this.fileConversion, "fileConversion");
			this.fileConversion.ConfigurationManagerService = null;
			this.fileConversion.FileConversionParameters = null;
			this.fileConversion.IsConversionEnabled = true;
			this.fileConversion.LoggerSpecifics = null;
			this.fileConversion.Name = "fileConversion";
			this.fileConversion.SourceFolder = null;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxLoggerDevice);
			base.Controls.Add(this.fileConversion);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "LoggerDevice";
			this.groupBoxLoggerDevice.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
