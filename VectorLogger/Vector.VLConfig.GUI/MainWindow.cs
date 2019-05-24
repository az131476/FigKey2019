using Nini.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Vector.InfoWindow.NET;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.BusinessLogic.Conversion;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.ConfigurationPersistency;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.AnalogInputsPage;
using Vector.VLConfig.GUI.CANChannelsPage;
using Vector.VLConfig.GUI.CardReaderNavigatorPage;
using Vector.VLConfig.GUI.CardReaderPage;
using Vector.VLConfig.GUI.CcpXcpDescriptionsPage;
using Vector.VLConfig.GUI.CcpXcpSignalRequestsPage;
using Vector.VLConfig.GUI.CLFExportPage;
using Vector.VLConfig.GUI.CombinedAnalogDigitalInputsPage;
using Vector.VLConfig.GUI.CommentPage;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.GUI.Common.DeviceInteraction;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.GUI.Common.QuickView;
using Vector.VLConfig.GUI.DatabasesPage;
using Vector.VLConfig.GUI.DeviceInformationPage;
using Vector.VLConfig.GUI.DiagnosticActionsPage;
using Vector.VLConfig.GUI.DiagnosticsDatabasesPage;
using Vector.VLConfig.GUI.DigitalInputsPage;
using Vector.VLConfig.GUI.DigitalOutputsPage;
using Vector.VLConfig.GUI.FiltersPage;
using Vector.VLConfig.GUI.FlexrayChannelsPage;
using Vector.VLConfig.GUI.GPSPage;
using Vector.VLConfig.GUI.HardwareSettingsPage;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.GUI.ImportSolver;
using Vector.VLConfig.GUI.IncludeFilesPage;
using Vector.VLConfig.GUI.InterfaceModePage;
using Vector.VLConfig.GUI.LEDsPage;
using Vector.VLConfig.GUI.LINChannelsPage;
using Vector.VLConfig.GUI.LoggerDeviceNavigatorPage;
using Vector.VLConfig.GUI.LoggerDevicePage;
using Vector.VLConfig.GUI.MOST150ChannelsPage;
using Vector.VLConfig.GUI.MultibusChannelsPage;
using Vector.VLConfig.GUI.Options;
using Vector.VLConfig.GUI.SendMessagePage;
using Vector.VLConfig.GUI.SpecialFeaturesPage;
using Vector.VLConfig.GUI.TriggersPage;
using Vector.VLConfig.GUI.WLANSettingsPage;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction.VN16XXlog;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI
{
	public class MainWindow : Form, IUpdateService, IUpdateServiceForFeature, IHardwareFrontendClient
	{
		private const int propertyWindowMinSize = 360;

		private LRUFileHandler lruFileHandler;

		private ConfigurationManager configManager;

		private VLProject vlProject;

		private AppDataAccess mAppDataAccess;

		private IApplicationDatabaseManager applicationDatabaseManager;

		private DiagSymbolsManager diagSymbolsManager;

		private IHardwareFrontend hardwareFrontend;

		private ILoggerDevice device;

		private IList<IPropertyWindow> propertyWindowList;

		private InfoWindow infoWindow;

		private SetRealtimeClock setRealtimeClockDialog;

		private SetEthernetAddress setEthernetAddressDialog;

		private InputSplitSize splitSizeDialog;

		private IPropertyWindow activePropertyWindow;

		private Dictionary<IUpdateObserver, UpdateContext> updateObserver2UpdateContext;

		private LoggerType loggerTypeToStartWith;

		private bool isAutoEjectEnabled;

		private string filePathFromCommandLine;

		private static Form _dummyHelpParent = new Form();

		private string appPathToLINprobe;

		private string appPathToCANshunt;

		private string appPathToCANgps;

		private string appPathToMlSetup;

		private bool isSetProjectDirtyEnabled;

		private static List<UpdateContext> sUpdateContextOrder = new List<UpdateContext>
		{
			UpdateContext.BusinessLogic,
			UpdateContext.CcpXcpManager,
			UpdateContext.AnalogInputs,
			UpdateContext.DigitalInputs,
			UpdateContext.CombinedAnalogDigitalInputs,
			UpdateContext.CANChannels,
			UpdateContext.MultibusChannels,
			UpdateContext.Databases,
			UpdateContext.DiagnosticsDatabases,
			UpdateContext.DiagnosticActions,
			UpdateContext.HardwareSettings,
			UpdateContext.LEDs,
			UpdateContext.LINChannels,
			UpdateContext.FlexrayChannels,
			UpdateContext.MOST150Channels,
			UpdateContext.Triggers1,
			UpdateContext.Triggers2,
			UpdateContext.Filters1,
			UpdateContext.Filters2,
			UpdateContext.SendMessage,
			UpdateContext.DigitalOutputs,
			UpdateContext.IncludeFiles,
			UpdateContext.SpecialFeatures,
			UpdateContext.Comment,
			UpdateContext.LoggerDevice,
			UpdateContext.LoggerDeviceNavigator,
			UpdateContext.CardReader,
			UpdateContext.CardReaderNavigator,
			UpdateContext.CLFExport,
			UpdateContext.DeviceInformation,
			UpdateContext.WLANSettings,
			UpdateContext.InterfaceMode,
			UpdateContext.GPS,
			UpdateContext.TreeControl
		};

		private IContainer components;

		private ToolStripMenuItem newProjectToolStripMenuItem;

		private ToolStripMenuItem openProjectToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator1;

		private ToolStripMenuItem saveToolStripMenuItem;

		private ToolStripMenuItem saveAsToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator3;

		private ToolStripSeparator toolStripSeparator2;

		private ToolStripMenuItem exitToolStripMenuItem;

		private ToolStripContainer toolStripContainer;

		private ToolStripButton toolStripButtonOpen;

		private ToolStripButton toolStripButtonSave;

		private ToolStripButton toolStripButtonSaveAs;

		private StatusStrip statusStrip;

		private ToolStripStatusLabel toolStripStatusLabel1;

		private SplitContainer splitContainer1;

		private TreeControl treeControl;

		private MenuStrip mainMenuStrip;

		private ToolStripMenuItem fileToolStripMenuItem;

		private ToolStripMenuItem newProjectToolStripMenuItem1;

		private ToolStripMenuItem openProjectToolStripMenuItem1;

		private ToolStripSeparator toolStripSeparator4;

		private ToolStripMenuItem saveProjectToolStripMenuItem;

		private ToolStripMenuItem saveProjectAsToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator5;

		private ToolStripSeparator toolStripSeparator6;

		private ToolStripMenuItem exitToolStripMenuItem1;

		private ToolStripMenuItem configurationToolStripMenuItem;

		private ToolStripMenuItem deviceToolStripMenuItem;

		private ToolStripMenuItem windowToolStripMenuItem;

		private ToolStripMenuItem helpToolStripMenuItem;

		private ToolStrip mainMenuToolStrip;

		private ToolStripButton toolStripButtonNewProject;

		private ToolStripButton toolStripButtonOpenProject;

		private ToolStripButton toolStripButtonSaveProject;

		private ToolStripButton toolStripButtonSaveProjectAs;

		private CANChannels canChannels;

		private Triggers triggers1;

		private Databases databases;

		private ToolStripMenuItem exportToLTLFileToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator7;

		private ToolStripButton toolStripButtonExportToLTL;

		private HardwareSettings hardwareSettings;

		private ToolStripStatusLabel toolStripStatusLabelSelectedLoggerType;

		private LINChannels linChannels;

		private LEDs leds;

		private AnalogInputs analogInputs;

		private ToolStripSeparator toolStripSeparator8;

		private ToolStripButton toolStripButtonHexMode;

		private ToolStripMenuItem exportToCODFileToolStripMenuItem;

		private Filters filters1;

		private CLFExport clfExport;

		private CardReader cardReader;

		private ToolStripMenuItem refreshToolStripMenuItem;

		private IncludeFiles includeFiles;

		private SpecialFeatures specialFeatures;

		private ToolStripMenuItem writeToMemoryCardToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator9;

		private ToolStripMenuItem contentsToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator11;

		private ToolStripMenuItem aboutToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator12;

		private ToolStripButton toolStripButtonAbout;

		private ToolStripButton packLoggingDataFromDeviceToolStripButton;

		private ToolStripButton toolStripButtonHelp;

		private ToolStripButton toolStripButtonWriteToCard;

		private ToolStripSeparator toolStripSeparator13;

		private ToolStripMenuItem writeCODFileToMemoryCardToolStripMenuItem;

		private LoggerDevice loggerDevice;

		private DeviceInformation deviceInformation;

		private ToolStripStatusLabel toolStripStatusLabelOnOffline;

		private ToolStripMenuItem writeToDeviceToolStripMenuItem;

		private ToolStripMenuItem createDatabaseForToolStripMenuItem;

		private ToolStripButton toolStripButtonWriteToDevice;

		private ToolStripMenuItem writeCODFileToDeviceToolStripMenuItem;

		private ToolStripMenuItem setRealTimeClockToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator15;

		private ToolStripMenuItem writeLicenseToDeviceToolStripMenuItem;

		private ToolStripMenuItem writeLicenseToMemoryCardToolStripMenuItem;

		private ToolStripButton toolStripButtonRefresh;

		private ToolStripButton toolStripButtonSetRealtimeClock;

		private ToolStripSeparator toolStripSeparator14;

		private ToolStripMenuItem hexadecimalDisplayToolStripMenuItem;

		private ToolStripMenuItem setEthernetAddressToolStripMenuItem;

		private WLANSettings wlanSettings;

		private FlexrayChannels flexrayChannels;

		private ToolStripMenuItem toolsToolStripMenuItem;

		private ToolStripMenuItem linProbeConfiguratorToolStripMenuItem;

		private ToolStripMenuItem canShuntConfiguratorToolStripMenuItem;

		private ToolStripMenuItem canGpsConfiguratorToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator16;

		private ToolStripMenuItem splitFileToolStripMenuItem;

		private ToolStripMenuItem mergeFileToolStripMenuItem;

		private ToolStripMenuItem mlSetupToolStripMenuItem;

		private Comment comment;

		private DiagnosticsDatabases diagnosticsDatabases;

		private DiagnosticActions diagnosticActions;

		private InterfaceMode interfaceMode;

		private DigitalInputs digitalInputs;

		private ToolStripSeparator toolStripSeparator17;

		private MOST150Channels most150Channels;

		private ToolStripMenuItem formatMemoryCardToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator18;

		private ToolStripMenuItem clearDeviceToolStripMenuItem;

		private ToolStripMenuItem clearMemoryCardToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator19;

		private ToolStripMenuItem formatMemoryCardInDeviceToolStripMenuItem;

		private Filters filters2;

		private Triggers triggers2;

		private SendMessage sendMessage;

		private DigitalOutputs digitalOutputs;

		private GPS gps;

		private ToolStripMenuItem optionsToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator10;

		private ToolStripMenuItem exportProjectToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator20;

		private ToolStripMenuItem importProjectToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator21;

		private ToolStripMenuItem importProjectFromDeviceToolStripMenuItem;

		private ToolStripMenuItem importProjectFromMemoryCardToolStripMenuItem;

		private CardReaderNavigator cardReaderNavigator;

		private LoggerDeviceNavigator loggerDeviceNavigator;

		private ToolStripMenuItem setVehicleNameToolStripMenuItem;

		private MultibusChannels multibusChannels;

		private CombinedAnalogDigitalInputs combinedAnalogDigitalInputs;

		private ToolStripMenuItem createVSysVarFromIniToolStripMenuItem;

		private CcpXcpDescriptions ccpXcpDescriptions;

		private CcpXcpSignalRequests ccpXcpSignalRequests;

		private ToolStrip mToolStripFileConversionSettings;

		private ToolStripButton mToolStripButtonSaveFileConversionSettings;

		private ToolStripDropDownButton mToolStripDropDownLoadFileConversionSettings;

		private ToolStripMenuItem updateLoggerFirmwareToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator22;

		private ToolStripMenuItem packLoggingDataFromDeviceToolStripMenuItem;

		private ToolStripMenuItem packLoggingDataFromMemoryCardToolStripMenuItem;

		private ToolStripSeparator toolStripSeparator23;

		private ToolStripButton packLoggingDataFromCardReaderToolStripButton;

		private ToolStripSeparator toolStripSeparator24;

		private ToolStripMenuItem analysisPackageImportFromDeviceToolStripMenuItem;

		private ToolStripMenuItem analysisPackageImportFromMemoryCardToolStripMenuItem;

		private VLProject VLProject
		{
			get
			{
				return this.vlProject;
			}
		}

		private AppDataAccess AppDataAccess
		{
			get
			{
				return this.mAppDataAccess;
			}
		}

		private IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.applicationDatabaseManager;
			}
		}

		public IHardwareFrontend HardwareFrontend
		{
			get
			{
				return this.hardwareFrontend;
			}
			set
			{
				this.hardwareFrontend = value;
			}
		}

		public MainWindow(string[] args, AppDataAccess appDataAccess)
		{
			FileConversionProfileManager.Instance.Init(appDataAccess);
			this.InitializeComponent();
			this.mToolStripFileConversionSettings.Dock = DockStyle.Right;
			this.mainMenuToolStrip.Dock = DockStyle.Right;
			this.mainMenuStrip.Dock = DockStyle.Right;
			GUIUtil.InitGuiScaling(this);
			this.DPIScaleToolbars();
			this.DPIScalePropertyPagePanel();
			this.ParseCommandLineArguments(args);
			this.isSetProjectDirtyEnabled = true;
			this.FillPropertyWindowList();
			this.Init(appDataAccess);
			using (new WaitCursor())
			{
				this.ApplicationDatabaseManager.UpdateApplicationDatabaseConfiguration(this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration, this.vlProject.ProjectRoot.HardwareConfiguration.MultibusChannelConfiguration, this.vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration, FileSystemServices.GetFolderPathFromFilePath(this.vlProject.FilePath));
				this.diagSymbolsManager.UpdateDatabaseConfiguration(this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticsDatabaseConfiguration, this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticActionsConfiguration, FileSystemServices.GetFolderPathFromFilePath(this.vlProject.FilePath));
			}
			this.SubscribeTreeControlEvents();
			this.treeControl.SelectDefaultPropertyPage();
			FileSystemServices.OpenProtocol();
		}

		private void DPIScaleToolbars()
		{
			this.mainMenuToolStrip.ImageScalingSize = GUIUtil.GuiScaleSize_DPI(this.mainMenuToolStrip.ImageScalingSize);
			this.mainMenuToolStrip.Size = GUIUtil.GuiScaleSize(this.mainMenuToolStrip.Size);
			this.mToolStripFileConversionSettings.ImageScalingSize = GUIUtil.GuiScaleSize_DPI(this.mToolStripFileConversionSettings.ImageScalingSize);
			this.mToolStripFileConversionSettings.Size = GUIUtil.GuiScaleSize(this.mToolStripFileConversionSettings.Size);
		}

		private void DPIScalePropertyPagePanel()
		{
			this.splitContainer1.Panel2.AutoScrollMinSize = GUIUtil.GuiScaleSize(this.splitContainer1.Panel2.AutoScrollMinSize);
		}

		private void ParseCommandLineArguments(string[] args)
		{
			this.filePathFromCommandLine = string.Empty;
			GlobalRuntimeSettings instance = GlobalRuntimeSettings.GetInstance();
			if (args != null && args.Length > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					if (args[i].ToLower().EndsWith(Vocabulary.FileExtensionDotGLC))
					{
						this.filePathFromCommandLine = args[i];
					}
					else if (!args[i].ToUpper().Equals("-AMI") && !args[i].Equals("-AllowMultipleInstances"))
					{
						if (args[i].ToLower().StartsWith("-batch"))
						{
							instance.ActiveApplicationMode = (instance.ActiveApplicationMode = GlobalRuntimeSettings.ApplicationMode.BatchMode);
							if (args[i].Contains("="))
							{
								instance.BatchModePath = args[i].Split(new char[]
								{
									'='
								})[1];
							}
							else if (args.Length > i + 1)
							{
								instance.BatchModePath = args[++i];
							}
							else
							{
								instance.BatchModePath = string.Empty;
							}
							if (File.Exists(instance.BatchModePath) && instance.BatchModePath.ToLower().EndsWith(Vocabulary.FileExtensionDotINI))
							{
								this.LoadBatchFilesFromINI(instance.BatchModePath);
							}
							if (!File.Exists(instance.BatchModePath) || !instance.BatchModePath.ToLower().EndsWith(".bat"))
							{
								InformMessageBox.Error(Resources.CliErrorMissingBatch, new string[]
								{
									instance.BatchModePath
								});
								instance.ActiveApplicationMode = GlobalRuntimeSettings.ApplicationMode.Default;
								instance.BatchModePath = string.Empty;
							}
						}
						else if (args[i].ToLower().StartsWith("-loggertype"))
						{
							string text;
							if (args[i].Contains("="))
							{
								text = args[i].Split(new char[]
								{
									'='
								})[1];
							}
							else if (args.Length > i + 1)
							{
								text = args[++i];
							}
							else
							{
								text = string.Empty;
							}
							LoggerType[] array = (LoggerType[])Enum.GetValues(typeof(LoggerType));
							for (int j = 0; j < array.Length; j++)
							{
								LoggerType loggerType = array[j];
								if (text.ToLower().Equals(loggerType.ToString().ToLower()))
								{
									instance.LoggerType = loggerType;
								}
							}
							if (instance.LoggerType == LoggerType.Unknown)
							{
								InformMessageBox.Error(Resources.CliErrorInvalidLoggerType, new string[]
								{
									text
								});
							}
						}
						else
						{
							InformMessageBox.Error(Resources.CliErrorInvalidParameter, new string[]
							{
								args[i]
							});
						}
					}
				}
			}
		}

		private void LoadBatchFilesFromINI(string iniPath)
		{
			Dictionary<string, string> batchModePathList = GlobalRuntimeSettings.GetInstance().BatchModePathList;
			batchModePathList.Clear();
			string directoryName = Path.GetDirectoryName(Path.GetFullPath(iniPath));
			try
			{
				IConfigSource configSource = new IniConfigSource(iniPath);
				IConfig config = configSource.Configs["default"];
				string @string = config.GetString("batch_path_default");
				if (@string != null && @string.Length > 0)
				{
					string text = directoryName + Path.DirectorySeparatorChar + @string;
					if (File.Exists(text) && text.ToLower().EndsWith(".bat"))
					{
						GlobalRuntimeSettings.GetInstance().BatchModePath = text;
					}
					else if (File.Exists(@string) && @string.ToLower().EndsWith(".bat"))
					{
						GlobalRuntimeSettings.GetInstance().BatchModePath = @string;
					}
				}
				config = configSource.Configs["additional_batch_files"];
				if (config != null)
				{
					int @int = config.GetInt("number_of_additional_batch_files");
					for (int i = 1; i <= @int; i++)
					{
						string text2 = config.GetString("batch_name_" + i);
						string string2 = config.GetString("batch_path_" + i);
						if (string2 != null)
						{
							if (text2 == null || text2.Length < 1)
							{
								text2 = string2;
							}
							string text3 = directoryName + Path.DirectorySeparatorChar + string2;
							if (!batchModePathList.ContainsKey(text2) && File.Exists(text3) && text3.ToLower().EndsWith(".bat"))
							{
								if (GlobalRuntimeSettings.GetInstance().BatchModePath.ToLower().EndsWith(Vocabulary.FileExtensionDotINI))
								{
									GlobalRuntimeSettings.GetInstance().BatchModePath = text3;
								}
								else
								{
									batchModePathList.Add(text2, text3);
								}
							}
							else if (!batchModePathList.ContainsKey(text2) && File.Exists(string2) && string2.ToLower().EndsWith(".bat"))
							{
								if (GlobalRuntimeSettings.GetInstance().BatchModePath.ToLower().EndsWith(Vocabulary.FileExtensionDotINI))
								{
									GlobalRuntimeSettings.GetInstance().BatchModePath = string2;
								}
								else
								{
									batchModePathList.Add(text2, string2);
								}
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		private void Init(AppDataAccess appDataAccess)
		{
			this.InitInfoWindow();
			this.infoWindow.ShowInfoWindow(this, false);
			this.lruFileHandler = new LRUFileHandler(this.fileToolStripMenuItem.DropDownItems, new EventHandler(this.OnLRUFilesClicked), this.Font);
			this.lruFileHandler.NumberOfFixedMenuItems = 8;
			this.splitContainer1.Panel2MinSize = 360;
			this.vlProject = new VLProject();
			this.mAppDataAccess = appDataAccess;
			GenerationUtil.AppDataAccess = this.mAppDataAccess;
			this.applicationDatabaseManager = new ApplicationDatabaseManager();
			this.applicationDatabaseManager.DatabaseFileChanged += new EventHandler(this.OnDatabaseFileChanged);
			string directoryName = Path.GetDirectoryName(Application.ExecutablePath);
			string text = Path.Combine(directoryName, Resources.HelpFileName);
			this.diagSymbolsManager = this.InitDiagSymbolsManager(text);
			this.applicationDatabaseManager.SymSelHelpFileName = text;
			this.hardwareFrontend = new HardwareFrontend();
			this.loggerTypeToStartWith = LoggerType.GL3000;
			this.isAutoEjectEnabled = true;
			bool flag = this.LoadAppDataSettings();
			if (GlobalRuntimeSettings.GetInstance().LoggerType != LoggerType.Unknown)
			{
				this.loggerTypeToStartWith = GlobalRuntimeSettings.GetInstance().LoggerType;
			}
			ILoggerSpecifics loggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(this.loggerTypeToStartWith);
			if (!flag)
			{
				if (!Settings.Default.IsGL1020FTEInitialLogger)
				{
					LoggerTypeSelection loggerTypeSelection = new LoggerTypeSelection(this.loggerTypeToStartWith, true);
					if (DialogResult.OK == loggerTypeSelection.ShowDialog())
					{
						this.loggerTypeToStartWith = loggerTypeSelection.SelectedLoggerType;
						loggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(loggerTypeSelection.SelectedLoggerType);
					}
				}
				else
				{
					this.loggerTypeToStartWith = LoggerType.GL1020FTE;
					loggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(this.loggerTypeToStartWith);
				}
			}
			appDataAccess.AppDataRoot.GlobalOptions.LoggerTypeToStartWith = this.loggerTypeToStartWith;
			Vector.VLConfig.BusinessLogic.Conversion.AnalysisPackage.Parameters = appDataAccess.AppDataRoot.AnalysisPackageSettings.AnalysisPackageParameters;
			this.configManager = new ConfigurationManager(this, this.vlProject, this.applicationDatabaseManager, this.diagSymbolsManager);
			this.configManager.InitializeDefaultConfiguration(loggerSpecifics);
			this.setRealtimeClockDialog = new SetRealtimeClock(loggerSpecifics, this.device);
			this.setEthernetAddressDialog = new SetEthernetAddress();
			this.splitSizeDialog = new InputSplitSize();
			this.InitCcpXcpManager();
			this.InitTreeControl();
			this.hardwareFrontend.LoggerTypeToScan = this.loggerTypeToStartWith;
			this.hardwareFrontend.AdditionalDrives = appDataAccess.AppDataRoot.GlobalOptions.AdditionalDrivesList;
			this.hardwareFrontend.RegisterClient(this);
			this.InitPropertyWindows();
			this.SubscribePropertyWindowEvents();
			this.infoWindow.CloseInfoWindow(false);
			this.infoWindow.SetInfoButton(true);
			this.infoWindow.MoreInfoClicked += new MoreInfoClickedHandler(this.InfoWindow_MoreInfoClicked);
			this.InitToolsMenu();
			this.InitNewFile(loggerSpecifics);
		}

		private void InitInfoWindow()
		{
			bool flag = AssemblyAttributeServicePack.IsServicePack();
			int servicePackNumber = AssemblyAttributeServicePack.GetServicePackNumber();
			string text = "";
			if (flag)
			{
				text = string.Format(Resources.ServicePack, servicePackNumber);
			}
			this.infoWindow = new InfoWindow();
			if (string.Compare(Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName, "de", true) == 0)
			{
				this.infoWindow.SetLanguage("49");
			}
			this.infoWindow.SetCompanyName(Resources.InfoCompanyName);
			this.infoWindow.SetProductLine(Vocabulary.VLConfigApplicationTitle);
			if (!Settings.Default.IsUSA)
			{
				this.infoWindow.SetLegalityNotice(Resources.InfoLegalityNotice);
			}
			this.infoWindow.SetCopyright(Resources.InfoCopyright);
			this.infoWindow.SetProductDescription(Resources.InfoSupportedLoggerTypes);
			this.infoWindow.SetProductName(Vocabulary.VLConfigApplicationTitle);
			if (ProgramUtils.IsBeta)
			{
				this.infoWindow.SetVersionLine(Application.ProductVersion + Vocabulary.BetaPostfix + text);
			}
			else
			{
				this.infoWindow.SetVersionLine(Application.ProductVersion + text);
			}
			this.infoWindow.ShowRegistration(false);
			this.infoWindow.SelectDefaultBmpProdLogo(0);
		}

		private void FillPropertyWindowList()
		{
			if (this.propertyWindowList == null)
			{
				this.propertyWindowList = new List<IPropertyWindow>();
			}
			this.propertyWindowList.Add(this.canChannels);
			this.propertyWindowList.Add(this.linChannels);
			this.propertyWindowList.Add(this.multibusChannels);
			this.propertyWindowList.Add(this.leds);
			this.propertyWindowList.Add(this.databases);
			this.propertyWindowList.Add(this.filters1);
			this.propertyWindowList.Add(this.filters2);
			this.propertyWindowList.Add(this.triggers1);
			this.propertyWindowList.Add(this.triggers2);
			this.propertyWindowList.Add(this.hardwareSettings);
			this.propertyWindowList.Add(this.analogInputs);
			this.propertyWindowList.Add(this.digitalInputs);
			this.propertyWindowList.Add(this.combinedAnalogDigitalInputs);
			this.propertyWindowList.Add(this.clfExport);
			this.propertyWindowList.Add(this.cardReader);
			this.propertyWindowList.Add(this.cardReaderNavigator);
			this.propertyWindowList.Add(this.includeFiles);
			this.propertyWindowList.Add(this.specialFeatures);
			this.propertyWindowList.Add(this.loggerDevice);
			this.propertyWindowList.Add(this.loggerDeviceNavigator);
			this.propertyWindowList.Add(this.deviceInformation);
			this.propertyWindowList.Add(this.wlanSettings);
			this.propertyWindowList.Add(this.flexrayChannels);
			this.propertyWindowList.Add(this.most150Channels);
			this.propertyWindowList.Add(this.comment);
			this.propertyWindowList.Add(this.interfaceMode);
			this.propertyWindowList.Add(this.diagnosticsDatabases);
			this.propertyWindowList.Add(this.diagnosticActions);
			this.propertyWindowList.Add(this.digitalOutputs);
			this.propertyWindowList.Add(this.sendMessage);
			this.propertyWindowList.Add(this.gps);
			this.propertyWindowList.Add(this.ccpXcpDescriptions);
			this.propertyWindowList.Add(this.ccpXcpSignalRequests);
		}

		private void InitTreeControl()
		{
			this.treeControl.UpdateService = this;
			this.treeControl.Init();
		}

		private void InitCcpXcpManager()
		{
			CcpXcpManager.Instance().UpdateService = this;
			CcpXcpManager.Instance().ConfigurationManagerService = this.configManager;
			CcpXcpManager.Instance().Init();
		}

		private DiagSymbolsManager InitDiagSymbolsManager(string helpFilePath)
		{
			DiagSymbolsManager.Instance().Init(Path.Combine(Application.ExecutablePath, helpFilePath), GUIUtil.HelpPageID_DiagnosticsServiceParameterDialog.ToString());
			return DiagSymbolsManager.Instance();
		}

		private void InitPropertyWindows()
		{
			this.hardwareFrontend.RegisterClient(this.cardReaderNavigator);
			this.hardwareFrontend.RegisterClient(this.loggerDeviceNavigator);
			this.hardwareFrontend.RegisterClient(this.cardReader);
			this.hardwareFrontend.RegisterClient(this.loggerDevice);
			this.hardwareFrontend.RegisterClient(this.deviceInformation);
			this.multibusChannels.HardwareFrontend = this.hardwareFrontend;
			this.filters1.GlobalOptions = this.mAppDataAccess.AppDataRoot.GlobalOptions;
			this.filters2.GlobalOptions = this.mAppDataAccess.AppDataRoot.GlobalOptions;
			QuickView.OnGlobalOptionsChanged();
			this.comment.VLProject = this.vlProject;
			this.filters1.MemoryNr = 1;
			this.filters2.MemoryNr = 2;
			this.triggers1.MemoryNr = 1;
			this.triggers2.MemoryNr = 2;
			foreach (IPropertyWindow current in this.propertyWindowList)
			{
				current.ModelValidator = this.configManager.ModelValidator;
				current.SemanticChecker = this.configManager.SemanticChecker;
				current.ModelEditor = this.configManager.ModelEditor;
				current.UpdateService = this;
				current.Init();
				current.IsVisible = false;
				this.treeControl.BindPropertyWindow2Node(current);
			}
			this.multibusChannels.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.linChannels.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.hardwareSettings.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.filters1.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.filters2.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.triggers1.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.triggers2.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.triggers1.ConfigurationManagerService = this.configManager;
			this.triggers2.ConfigurationManagerService = this.configManager;
			this.interfaceMode.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.databases.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.diagnosticsDatabases.DiagSymbolsManager = this.diagSymbolsManager;
			this.diagnosticActions.DiagSymbolsManager = this.diagSymbolsManager;
			this.diagnosticActions.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.cardReader.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.cardReaderNavigator.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.loggerDevice.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.loggerDeviceNavigator.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.clfExport.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.sendMessage.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.digitalOutputs.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.ccpXcpDescriptions.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.ccpXcpSignalRequests.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.ccpXcpDescriptions.ConfigManager = this.configManager.Service;
			this.canChannels.ConfigurationManagerService = this.configManager.Service;
			this.gps.ApplicationDatabaseManager = this.applicationDatabaseManager;
			this.cardReaderNavigator.ConfigurationManagerService = this.configManager.Service;
			this.cardReaderNavigator.AnalysisPackageParameters = this.mAppDataAccess.AppDataRoot.AnalysisPackageSettings.AnalysisPackageParameters;
			this.cardReader.ConfigurationManagerService = this.configManager.Service;
			this.cardReader.AnalysisPackageParameters = this.mAppDataAccess.AppDataRoot.AnalysisPackageSettings.AnalysisPackageParameters;
			this.loggerDevice.ConfigurationManagerService = this.configManager.Service;
			this.loggerDevice.AnalysisPackageParameters = this.mAppDataAccess.AppDataRoot.AnalysisPackageSettings.AnalysisPackageParameters;
			this.loggerDeviceNavigator.ConfigurationManagerService = this.configManager.Service;
			this.loggerDeviceNavigator.AnalysisPackageParameters = this.mAppDataAccess.AppDataRoot.AnalysisPackageSettings.AnalysisPackageParameters;
			this.clfExport.ConfigurationManagerService = this.configManager.Service;
			this.clfExport.AnalysisPackageParameters = this.mAppDataAccess.AppDataRoot.AnalysisPackageSettings.AnalysisPackageParameters;
		}

		private void ResetPropertyWindows()
		{
			foreach (IPropertyWindow current in this.propertyWindowList)
			{
				current.Reset();
			}
		}

		private void InitToolsMenu()
		{
			this.linProbeConfiguratorToolStripMenuItem.Enabled = RegistryServices.IsGinLINprobeInstalled(out this.appPathToLINprobe);
			this.canShuntConfiguratorToolStripMenuItem.Enabled = RegistryServices.IsGinCANshuntInstalled(out this.appPathToCANshunt);
			this.canGpsConfiguratorToolStripMenuItem.Enabled = RegistryServices.IsGinCANgpsInstalled(out this.appPathToCANgps);
			this.mlSetupToolStripMenuItem.Enabled = RegistryServices.IsGinMLSetupInstalled(out this.appPathToMlSetup);
		}

		private void SubscribePropertyWindowEvents()
		{
			this.deviceInformation.FormatMemoryCardClicked += new EventHandler(this.OnFormatMemoryCardInDeviceClicked);
			this.configManager.DataModelChanged += new ConfigurationManager.DataModelChangedHandler(this.OnDataModelChanged);
		}

		public void OnDatabaseFileChanged(object sender, EventArgs args)
		{
			if (this.vlProject.ProjectRoot != null)
			{
				this.ValidateAllPropertyPages();
			}
		}

		public void DeviceUpdated(ILoggerDevice device)
		{
			if (device == this.device)
			{
				this.UpdateMenusAndToolbars();
			}
		}

		public void DevicesAdded(IList<ILoggerDevice> devices)
		{
			this.UpdateMenusAndToolbars();
		}

		public void DevicesRemoved(IList<ILoggerDevice> devices)
		{
			this.UpdateMenusAndToolbars();
		}

		public void AdditionalDrivesListChanged(IList<string> additionalDrivesList)
		{
		}

		private void SubscribeTreeControlEvents()
		{
			TreeControl expr_06 = this.treeControl;
			expr_06.OnBeforeChangeSelect = (TreeControl.BeforeChangeSelect)Delegate.Combine(expr_06.OnBeforeChangeSelect, new TreeControl.BeforeChangeSelect(this.OnTreeControlBeforeChangeSelect));
			TreeControl expr_2D = this.treeControl;
			expr_2D.OnTreeViewAndSelectedNodeChange = (EventHandler)Delegate.Combine(expr_2D.OnTreeViewAndSelectedNodeChange, new EventHandler(this.OnTreeViewAndSelectedNodeChange));
			TreeControl expr_54 = this.treeControl;
			expr_54.OnSelectTreeNode = (TreeControl.SelectTreeNode)Delegate.Combine(expr_54.OnSelectTreeNode, new TreeControl.SelectTreeNode(this.OnSelectTreeNode));
		}

		private void OnDataModelChanged(Feature changedFeature)
		{
			this.SetProjectDirty();
			if (changedFeature is DatabaseConfiguration)
			{
				this.applicationDatabaseManager.UpdateApplicationDatabaseConfiguration(changedFeature as DatabaseConfiguration, this.vlProject.ProjectRoot.HardwareConfiguration.MultibusChannelConfiguration, this.vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration, FileSystemServices.GetFolderPathFromFilePath(this.vlProject.FilePath));
			}
			else if (changedFeature is DiagnosticsDatabaseConfiguration)
			{
				using (new WaitCursor())
				{
					this.diagSymbolsManager.UpdateDatabaseConfiguration(changedFeature as DiagnosticsDatabaseConfiguration, this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticActionsConfiguration, this.configManager.Service.ConfigFolderPath);
				}
			}
			this.treeControl.RefreshView();
		}

		private void OnFormatMemoryCardInDeviceClicked(object sender, EventArgs e)
		{
			this.FormatMemoryCardInDevice();
		}

		private void OnTreeControlBeforeChangeSelect(TreeViewCancelEventArgs e)
		{
			if (this.activePropertyWindow != null)
			{
				if (!this.activePropertyWindow.ValidateInput() && this.activePropertyWindow.HasLocalErrors())
				{
					InformMessageBox.Warning(Resources.ErrorInvalidInputInDialog);
					e.Cancel = true;
					return;
				}
				this.activePropertyWindow.IsVisible = false;
			}
		}

		private void OnTreeViewAndSelectedNodeChange(object sender, EventArgs e)
		{
			if (this.activePropertyWindow != null)
			{
				this.activePropertyWindow.IsVisible = false;
			}
		}

		private void OnSelectTreeNode(object sender, IPropertyWindow pw)
		{
			if (pw != null)
			{
				this.SetActivePropertyWindow(pw);
			}
		}

		private void SetActivePropertyWindow(IPropertyWindow propertyWindow)
		{
			this.activePropertyWindow = propertyWindow;
			this.activePropertyWindow.IsVisible = true;
			this.UpdateToolStripFileConversionSettings();
			ConfigClipboardManager.ActiveClient = propertyWindow;
		}

		private void mainMenuStrip_MenuActivate(object sender, EventArgs e)
		{
			this.mainMenuStrip.Focus();
			if (this.activePropertyWindow != null)
			{
				this.activePropertyWindow.ValidateInput();
			}
		}

		private void newProjectToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			this.NewFile();
		}

		private void openProjectToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			this.OpenFile();
		}

		private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SaveFile();
		}

		private void saveProjectAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SaveFileAs();
		}

		private void importProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ImportProject("");
		}

		private void exportProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			ProjectExporterParameters projectExporterParameters = new ProjectExporterParameters(this.mAppDataAccess.AppDataRoot.ProjectExporterPage.ProjectExporterParameters);
			this.ExportProject("", projectExporterParameters);
		}

		private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			this.Exit();
		}

		private void writeToMemoryCardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.WriteToMemoryCard();
		}

		private void exportToLTLFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ExportToLtlOrCodFile(true);
		}

		private void exportToCODFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ExportToLtlOrCodFile(false);
		}

		private void clearMemoryCardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ClearMemoryCard();
		}

		private void writeLicenseToDeviceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.WriteLicenseToDevice();
		}

		private void writeLicenseToMemoryCardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.WriteLicenseToMemoryCard();
		}

		private void contentsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ShowHelp();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.infoWindow.ShowInfoWindow(this, true);
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OptionsDialog optionsDialog = new OptionsDialog(this.mAppDataAccess.AppDataRoot, this.VLProject.ProjectRoot.LoggerType, false);
			DialogResult dialogResult = optionsDialog.ShowDialog(this);
			this.loggerTypeToStartWith = optionsDialog.GlobalOptions.LoggerTypeToStartWith;
			if (dialogResult == DialogResult.OK)
			{
				using (new WaitCursor())
				{
					this.hardwareFrontend.AdditionalDrives = optionsDialog.GlobalOptions.AdditionalDrivesList;
					optionsDialog.Dispose();
				}
			}
		}

		private void InfoWindow_MoreInfoClicked(object sender, MoreInfoClickedEventArgs e)
		{
			string applicationBase = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
			InformMessageBox.Info(string.Format(Resources.MoreInfoInstallFolder, applicationBase));
		}

		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.RefreshPropertyView();
		}

		private void writeCODFileToMemoryCardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.CopyCODFileToMemoryCard();
		}

		private void writeToDeviceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.WriteConfigurationToDevice();
		}

		private void clearDeviceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ClearDevice();
		}

		private void createDatabaseForToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.GenerateAdditionalDatabases(true);
		}

		private void createVSysVarFromIniToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.GenerateVSysVarFileFromIni();
		}

		private void writeCODFileToDeviceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.WriteCODFileToDevice();
		}

		private void setRealTimeClockToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SetRealTimeClock();
		}

		private void setVehicleNameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SetVehicleName();
		}

		private void hexadecimalDisplayToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ToggleHexDecDisplay();
		}

		private void setEthernetAddressToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SetEthernetAddress();
		}

		private void updateLoggerFirmwareToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.configManager.LoggerSpecifics.Type != LoggerType.VN1630log)
			{
				return;
			}
			VN16XXlogDevice vN16XXlogDevice = this.device as VN16XXlogDevice;
			if (vN16XXlogDevice == null)
			{
				return;
			}
			if (vN16XXlogDevice.UpdateFirmware(this.AppDataAccess.AppDataRoot.GlobalOptions))
			{
				this.hardwareFrontend.EnforceExplicitUpdateOfLoggerDevice(vN16XXlogDevice);
			}
		}

		private void linProbeConfiguratorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FileSystemServices.LaunchFile(this.appPathToLINprobe);
		}

		private void canShuntConfiguratorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FileSystemServices.LaunchFile(this.appPathToCANshunt);
		}

		private void canGpsConfiguratorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FileSystemServices.LaunchFile(this.appPathToCANgps);
		}

		private void mlSetupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FileSystemServices.LaunchFile(this.appPathToMlSetup);
		}

		private void splitFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(FileType.ASCorBLFLogFiles) && DialogResult.OK == this.splitSizeDialog.ShowDialog())
			{
				ProgressIndicatorForm progressIndicatorForm = new ProgressIndicatorForm();
				progressIndicatorForm.Text = Resources.SplitFileTitle;
				if (!FileSplitMerge.SplitFile(GenericOpenFileDialog.FileName, this.splitSizeDialog.FileSizeInKB, progressIndicatorForm, new FileSplitMerge.ProcessExitedDelegate(progressIndicatorForm.ProcessExited)))
				{
					InformMessageBox.Error(FileSplitMerge.GetLastErrorText());
					return;
				}
				progressIndicatorForm.ShowDialog();
				if (progressIndicatorForm.Cancelled())
				{
					InformMessageBox.Info(Resources.FileSplittingAborted);
					progressIndicatorForm.Dispose();
					return;
				}
				progressIndicatorForm.Dispose();
				string lastErrorText = FileSplitMerge.GetLastErrorText();
				if (!string.IsNullOrEmpty(lastErrorText))
				{
					InformMessageBox.Error(lastErrorText);
					return;
				}
				InformMessageBox.Info(string.Format(Resources.SplittingOfFileCompleted, GenericOpenFileDialog.FileName));
			}
		}

		private void mergeFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(FileType.SplitFile))
			{
				string str;
				if (!FileSplitMerge.PeekStartSplitFile(GenericOpenFileDialog.FileName, out str))
				{
					InformMessageBox.Error(FileSplitMerge.GetLastErrorText());
					return;
				}
				using (SaveFileDialog saveFileDialog = new SaveFileDialog())
				{
					string projectFolder = this.vlProject.GetProjectFolder();
					if (!string.IsNullOrEmpty(projectFolder))
					{
						saveFileDialog.CustomPlaces.Add(projectFolder);
					}
					saveFileDialog.Filter = Resources_Files.FileFilterAllFiles;
					string path = Path.GetFileNameWithoutExtension(GenericOpenFileDialog.FileName) + str;
					saveFileDialog.FileName = Path.Combine(Path.GetDirectoryName(GenericOpenFileDialog.FileName), path);
					if (DialogResult.OK == saveFileDialog.ShowDialog())
					{
						ProgressIndicatorForm progressIndicatorForm = new ProgressIndicatorForm();
						progressIndicatorForm.Text = Resources.MergeFilesTitle;
						if (!FileSplitMerge.MergeFiles(GenericOpenFileDialog.FileName, saveFileDialog.FileName, progressIndicatorForm, new FileSplitMerge.ProcessExitedDelegate(progressIndicatorForm.ProcessExited)))
						{
							InformMessageBox.Error(FileSplitMerge.GetLastErrorText());
						}
						else
						{
							progressIndicatorForm.ShowDialog();
							if (progressIndicatorForm.Cancelled())
							{
								InformMessageBox.Info(Resources.FileMergingAborted);
								progressIndicatorForm.Dispose();
							}
							else
							{
								progressIndicatorForm.Dispose();
								string lastErrorText = FileSplitMerge.GetLastErrorText();
								if (!string.IsNullOrEmpty(lastErrorText))
								{
									InformMessageBox.Error(lastErrorText);
								}
								else
								{
									InformMessageBox.Info(string.Format(Resources.MergingOfFilesCompleted, saveFileDialog.FileName));
								}
							}
						}
					}
				}
			}
		}

		private void formatMemoryCardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.FormatMemoryCard();
		}

		private void formatMemoryCardInDeviceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.FormatMemoryCardInDevice();
		}

		private void importProjectFromDeviceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ImportProjectFromDevice();
		}

		private void importProjectFromMemoryCardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ImportProjectFromMemoryCard();
		}

		private void analysisPackageImportFromDeviceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ImportAnalysisPackageFromDevice();
		}

		private void analysisPackageImportFromMemoryCardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ImportAnalysisPackageFromMemoryCard();
		}

		private void mainMenuToolStrip_Click(object sender, EventArgs e)
		{
			this.mainMenuToolStrip.Focus();
			if (this.activePropertyWindow != null)
			{
				this.activePropertyWindow.ValidateInput();
			}
		}

		private void toolStripButtonNewProject_Click(object sender, EventArgs e)
		{
			this.mainMenuToolStrip.Enabled = false;
			this.NewFile();
			this.mainMenuToolStrip.Enabled = true;
		}

		private void toolStripButtonOpenProject_Click(object sender, EventArgs e)
		{
			this.mainMenuToolStrip.Enabled = false;
			this.OpenFile();
			this.mainMenuToolStrip.Enabled = true;
		}

		private void toolStripButtonSaveProject_Click(object sender, EventArgs e)
		{
			this.mainMenuToolStrip.Enabled = false;
			this.SaveFile();
			this.mainMenuToolStrip.Enabled = true;
		}

		private void toolStripButtonSaveProjectAs_Click(object sender, EventArgs e)
		{
			this.mainMenuToolStrip.Enabled = false;
			this.SaveFileAs();
			this.mainMenuToolStrip.Enabled = true;
		}

		private void toolStripButtonWriteToDevice_Click(object sender, EventArgs e)
		{
			this.mainMenuToolStrip.Enabled = false;
			this.WriteConfigurationToDevice();
			this.mainMenuToolStrip.Enabled = true;
		}

		private void toolStripButtonWriteToCard_Click(object sender, EventArgs e)
		{
			this.mainMenuToolStrip.Enabled = false;
			this.WriteToMemoryCard();
			this.mainMenuToolStrip.Enabled = true;
		}

		private void toolStripButtonExportToLTL_Click(object sender, EventArgs e)
		{
			this.mainMenuToolStrip.Enabled = false;
			this.ExportToLtlOrCodFile(true);
			this.mainMenuToolStrip.Enabled = true;
		}

		private void toolStripButtonSetRealtimeClock_Click(object sender, EventArgs e)
		{
			this.mainMenuToolStrip.Enabled = false;
			this.SetRealTimeClock();
			this.mainMenuToolStrip.Enabled = true;
		}

		private void toolStripButtonRefresh_Click(object sender, EventArgs e)
		{
			this.RefreshPropertyView();
		}

		private void toolStripButtonHexMode_Click(object sender, EventArgs e)
		{
			this.ToggleHexDecDisplay();
		}

		private void toolStripButtonAbout_Click(object sender, EventArgs e)
		{
			this.infoWindow.ShowInfoWindow(this, true);
		}

		private void toolStripButtonHelp_Click(object sender, EventArgs e)
		{
			this.ShowHelp();
		}

		private void ToolStripDropDownButtonExportSettingsLoad_DropDownOpening(object sender, EventArgs e)
		{
			FileConversionProfileManager.InitDropDownItems(this.mToolStripDropDownLoadFileConversionSettings.DropDownItems, this.activePropertyWindow as IFileConversionParametersClient);
		}

		private void ToolStripDropDownButtonExportSettingsLoad_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			FileConversionProfileManager.OnDropDownItemClicked(e, this.activePropertyWindow as IFileConversionParametersClient, false);
		}

		private void ToolStripButtonExportSettingsSave_Click(object sender, EventArgs e)
		{
			IFileConversionParametersClient fileConversionParametersClient = this.activePropertyWindow as IFileConversionParametersClient;
			if (fileConversionParametersClient == null)
			{
				return;
			}
			FileConversionProfileManager.Instance.SaveFrom(fileConversionParametersClient);
		}

		private void UpdateToolStripFileConversionSettings()
		{
			IFileConversionParametersClient fileConversionParametersClient = this.activePropertyWindow as IFileConversionParametersClient;
			this.mToolStripFileConversionSettings.Visible = (fileConversionParametersClient != null);
			this.mToolStripButtonSaveFileConversionSettings.Enabled = (fileConversionParametersClient != null && fileConversionParametersClient.FileConversionParameters != null);
		}

		private bool Exit()
		{
			if (!this.OnBeforeUnloadingCurrentConfiguration())
			{
				return false;
			}
			this.lruFileHandler.SaveToolStripItem2Settings();
			this.SaveAppDataSettings();
			this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.ClearDatabases();
			FileSystemServices.CloseProtocol();
			Application.Exit();
			return true;
		}

		private void NewFile()
		{
			if (!this.OnBeforeUnloadingCurrentConfiguration())
			{
				return;
			}
			LoggerTypeSelection loggerTypeSelection = new LoggerTypeSelection(this.loggerTypeToStartWith);
			if (DialogResult.OK == loggerTypeSelection.ShowDialog())
			{
				this.InitNewFile(LoggerSpecificsFactory.CreateLoggerSpecifics(loggerTypeSelection.SelectedLoggerType));
			}
		}

		private void OpenFile()
		{
			if (!this.OnBeforeUnloadingCurrentConfiguration())
			{
				return;
			}
			string text;
			this.lruFileHandler.GetMostRecentlyUsedFile(out text);
			if (string.IsNullOrEmpty(text))
			{
				GenericOpenFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
			else
			{
				GenericOpenFileDialog.InitialDirectory = Path.GetDirectoryName(text);
			}
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(FileType.ProjectFile))
			{
				bool checkForMissingDiagDescriptions = true;
				this.LoadFile(GenericOpenFileDialog.FileName, checkForMissingDiagDescriptions);
			}
		}

		private void OpenFile(string fileAndPath)
		{
			if (!this.OnBeforeUnloadingCurrentConfiguration())
			{
				return;
			}
			bool checkForMissingDiagDescriptions = true;
			this.LoadFile(fileAndPath, checkForMissingDiagDescriptions);
		}

		private void LoadFile(string fileAndPath, bool checkForMissingDiagDescriptions)
		{
			this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.ClearDatabases();
			CcpXcpManager.Instance().ReleaseUnusedA2lDatabases();
			this.PropagateConfigurationFolderPath("");
			ILoggerSpecifics loggerSpecifics;
			bool flag;
			if (!this.configManager.LoadFile(fileAndPath, out loggerSpecifics, out flag))
			{
				this.InitNewFile(this.configManager.LoggerSpecifics);
				return;
			}
			bool flag2 = this.ConfigurationLoaded(checkForMissingDiagDescriptions);
			this.UpdateLoggerTypeStatusBar();
			if (!flag2)
			{
				this.saveProjectToolStripMenuItem.Enabled = false;
				this.toolStripButtonSaveProject.Enabled = false;
			}
			else
			{
				this.SetProjectDirty();
			}
			this.UpdateApplicationTitleBarText();
			this.UpdateLRU(fileAndPath);
			this.hardwareFrontend.LoggerTypeToScan = loggerSpecifics.Type;
			this.hardwareFrontend.UpdateLoggerDeviceList();
		}

		private bool SaveProject()
		{
			if (this.vlProject.IsDirty || this.VLProject.FilePath.Length == 0)
			{
				DialogResult dialogResult = InformMessageBox.Show(EnumQuestionType.Question, Resources.SaveCurrentProjectPriorExportQuestion);
				return dialogResult == DialogResult.Yes && this.SaveFile();
			}
			return true;
		}

		private bool OnBeforeImportProject()
		{
			if (this.vlProject.IsDirty)
			{
				DialogResult dialogResult = InformMessageBox.Show(EnumQuestionType.Question, Resources.SaveCurrentProjectQuestion);
				if (dialogResult == DialogResult.Yes)
				{
					this.SaveFile();
				}
			}
			return true;
		}

		private bool OnBeforeUnloadingCurrentConfiguration()
		{
			if (!this.vlProject.IsDirty)
			{
				return true;
			}
			DialogResult dialogResult = InformMessageBox.Show(EnumQuestionType.QuestionWithCancel, Resources.SaveCurrentProjectQuestion);
			if (dialogResult == DialogResult.No)
			{
				return true;
			}
			if (dialogResult != DialogResult.Yes)
			{
				return false;
			}
			if (this.VLProject.FilePath.Length > 0)
			{
				return this.SaveFile();
			}
			return this.SaveFileAs();
		}

		private bool SaveFile()
		{
			if (!this.CheckActivePageForFormatErrorsAndAskForDiscard())
			{
				return false;
			}
			if (string.IsNullOrEmpty(this.VLProject.FilePath))
			{
				return this.SaveFileAs();
			}
			this.vlProject.ProjectRoot.ApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			if (string.Compare(Path.GetExtension(this.VLProject.FilePath), Vocabulary.FileExtensionDotVLC, true) == 0)
			{
				string text = this.vlProject.GetProjectFileName();
				text = Path.ChangeExtension(text, Vocabulary.FileExtensionDotVLC);
				return this.SaveFileAs(text);
			}
			if (File.Exists(this.VLProject.FilePath))
			{
				FileAttributes attributes = File.GetAttributes(this.VLProject.FilePath);
				if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				{
					InformMessageBox.Error(Resources.ErrorFileWriteProtected, new string[]
					{
						this.VLProject.FilePath
					});
					return false;
				}
			}
			if (!this.configManager.SaveFile())
			{
				return false;
			}
			this.UpdateApplicationTitleBarText();
			this.NotifyConfigurationLoaded();
			this.saveProjectToolStripMenuItem.Enabled = false;
			this.toolStripButtonSaveProject.Enabled = false;
			return true;
		}

		private bool SaveFileAs()
		{
			return this.SaveFileAs(this.vlProject.GetProjectFileName());
		}

		private bool SaveFileAs(string suggestedFileName)
		{
			if (!this.CheckActivePageForFormatErrorsAndAskForDiscard())
			{
				return false;
			}
			bool flag = this.configManager.SaveFileAs(suggestedFileName);
			if (flag)
			{
				this.PropagateConfigurationFolderPath(this.vlProject.FilePath);
				this.UpdateApplicationTitleBarText();
				this.NotifyConfigurationLoaded();
				this.saveProjectToolStripMenuItem.Enabled = false;
				this.toolStripButtonSaveProject.Enabled = false;
				this.UpdateLRU(this.vlProject.FilePath);
				this.SelectFirstInvalidPage();
			}
			return flag;
		}

		private void ProjectImporterFileOverwriteHandler(string fileName, ref bool overwrite)
		{
			if (InformMessageBox.Question(string.Format(Resources.FileAlreadyExistsOverwrite, fileName)) == DialogResult.Yes)
			{
				overwrite = true;
			}
		}

		private void ProjectImporterErrorHandler(string fileName, string errorMsg, bool isFatal)
		{
			if (isFatal)
			{
				string message = string.Format(Resources.ProjectImportFailedFatal, fileName, errorMsg);
				InformMessageBox.Error(message);
				return;
			}
			string message2 = string.Format(Resources.ProjectImportFailed, fileName, errorMsg);
			InformMessageBox.Error(message2);
		}

		private void ProjectExporterErrorHandler(string fileName, string errorMsg, bool isFatal)
		{
			string message = string.Format(Resources.ProjectExportFailed, fileName, errorMsg);
			InformMessageBox.Error(message);
		}

		private bool ImportProject(string waterMark)
		{
			bool result = false;
			this.OnBeforeImportProject();
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(FileType.PackAndGoFile))
			{
				this.ImportProject(GenericOpenFileDialog.FileName, waterMark);
			}
			return result;
		}

		private bool ImportProject(string zipFilePath, string waterMark)
		{
			bool flag = false;
			using (BrowseFolderDialog browseFolderDialog = new BrowseFolderDialog())
			{
				browseFolderDialog.Title = Resources.SelectDestFolderImportedFiles;
				if (!string.IsNullOrEmpty(this.vlProject.FilePath))
				{
					browseFolderDialog.SelectedPath = Path.GetDirectoryName(this.vlProject.FilePath);
				}
				if (DialogResult.OK != browseFolderDialog.ShowDialog())
				{
					bool result = false;
					return result;
				}
				ProjectImporter projectImporter = new ProjectImporter(zipFilePath, browseFolderDialog.SelectedPath, waterMark);
				projectImporter.FileOverwriteHandler += new ProjectImportFileOverwriteHandler(this.ProjectImporterFileOverwriteHandler);
				projectImporter.ErrorHandler += new ErrorHandler(this.ProjectImporterErrorHandler);
				if (waterMark.Length > 0 && !projectImporter.MatchesWaterMark && InformMessageBox.Question(Resources.ProjectImportZIPAndCODDontMatch) == DialogResult.No)
				{
					bool result = false;
					return result;
				}
				if (projectImporter.IsPasswordProtected)
				{
					bool flag2 = true;
					PasswordDialog passwordDialog = new PasswordDialog(PasswordDialog.Context.PackAndGo, false);
					if (this.AppDataAccess.AppDataRoot.ProjectExporterPage.ProjectExporterParameters.ProtectWithPassword && string.IsNullOrEmpty(this.AppDataAccess.AppDataRoot.ProjectExporterPage.ProjectExporterParameters.Password))
					{
						if (projectImporter.CheckAndSetPassword(this.AppDataAccess.AppDataRoot.ProjectExporterPage.ProjectExporterParameters.Password))
						{
							flag2 = false;
						}
						else
						{
							passwordDialog.Message = Resources.StoredPasswordInvalid;
						}
					}
					while (flag2)
					{
						if (DialogResult.OK != passwordDialog.ShowDialog())
						{
							bool result = false;
							return result;
						}
						if (projectImporter.CheckAndSetPassword(passwordDialog.Password))
						{
							flag2 = false;
							if (passwordDialog.RememberPassword)
							{
								this.AppDataAccess.AppDataRoot.ProjectExporterPage.ProjectExporterParameters.RememberPassword(passwordDialog.Password);
							}
						}
						else
						{
							passwordDialog.Message = Resources.EnteredPasswordInvalid;
						}
					}
				}
				flag = projectImporter.Import();
				if (flag)
				{
					bool checkForMissingDiagDescriptions = false;
					this.LoadFile(projectImporter.GLCFilePath, checkForMissingDiagDescriptions);
					ImportSolver importSolver = new ImportSolver(ref projectImporter, ref this.configManager);
					if (importSolver.InconsistenciesExist)
					{
						importSolver.ShowDialog(this);
						this.OnDataModelChanged(this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration);
						this.OnDataModelChanged(this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration);
						this.OnDataModelChanged(this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticsDatabaseConfiguration);
						this.OnDataModelChanged(this.vlProject.ProjectRoot.LoggingConfiguration.IncludeFileConfiguration);
						this.OnDataModelChanged(this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration);
					}
				}
				projectImporter.FileOverwriteHandler -= new ProjectImportFileOverwriteHandler(this.ProjectImporterFileOverwriteHandler);
				projectImporter.ErrorHandler -= new ErrorHandler(this.ProjectImporterErrorHandler);
			}
			return flag;
		}

		private bool ImportProjectFromDevice()
		{
			this.OnBeforeImportProject();
			if (this.device != null)
			{
				if (this.device.LoggerType == LoggerType.GL1000 || this.device.LoggerType == LoggerType.GL1020FTE)
				{
					this.device.DownloadProjectZIPFile();
				}
				string[] projectZIPFilePath = this.device.GetProjectZIPFilePath();
				if (projectZIPFilePath != null)
				{
					if (projectZIPFilePath.Length == 1)
					{
						string[] cODFilePath = this.device.GetCODFilePath();
						string waterMark = string.Empty;
						if (cODFilePath != null && cODFilePath.Length > 0)
						{
							waterMark = GenerationUtil.ComputeMD5Hash(cODFilePath[0]);
						}
						return this.ImportProject(projectZIPFilePath[0], waterMark);
					}
					if (projectZIPFilePath.Length > 1)
					{
						InformMessageBox.Error(Resources.ProjectImportMultipleFilesOnDevice);
					}
				}
				if (projectZIPFilePath == null || projectZIPFilePath.Length == 0)
				{
					InformMessageBox.Error(Resources.ProjectImportNoZIPOnDevice);
				}
			}
			return false;
		}

		private bool ImportProjectFromMemoryCard()
		{
			this.OnBeforeImportProject();
			string text;
			if (!CardReaderDriveSelection.SelectCardReaderDrive(this.configManager.LoggerSpecifics, this.hardwareFrontend, this.hardwareFrontend.AdditionalDrives, out text, true, LoggerType.Unknown))
			{
				return false;
			}
			if (!this.IsMemoryCardAccessible(text))
			{
				InformMessageBox.Error(Resources.ErrorUnableToAccessDriveWillUpdate);
				this.hardwareFrontend.UpdateLoggerDeviceList();
				return false;
			}
			ILoggerDevice loggerDeviceByDriveName = this.hardwareFrontend.GetLoggerDeviceByDriveName(text);
			if (loggerDeviceByDriveName == null)
			{
				InformMessageBox.Error(string.Format(Resources.ErrorCannotAccessMemoryCardinDrive, text));
				return false;
			}
			if (loggerDeviceByDriveName.LoggerType == LoggerType.GL1000 || loggerDeviceByDriveName.LoggerType == LoggerType.GL1020FTE)
			{
				loggerDeviceByDriveName.DownloadProjectZIPFile();
			}
			string[] projectZIPFilePath = loggerDeviceByDriveName.GetProjectZIPFilePath();
			if (projectZIPFilePath != null)
			{
				if (projectZIPFilePath.Length == 1)
				{
					string[] cODFilePath = loggerDeviceByDriveName.GetCODFilePath();
					string waterMark = string.Empty;
					if (cODFilePath != null && cODFilePath.Length > 0)
					{
						waterMark = GenerationUtil.ComputeMD5Hash(cODFilePath[0]);
					}
					this.ImportProject(projectZIPFilePath[0], waterMark);
					return true;
				}
				if (projectZIPFilePath.Length > 1)
				{
					InformMessageBox.Error(Resources.ProjectImportMultipleFilesOnMemoryCard);
				}
			}
			if (projectZIPFilePath == null || projectZIPFilePath.Length == 0)
			{
				InformMessageBox.Info(Resources.ProjectImportNoZIPOnMemoryCard);
			}
			return false;
		}

		private bool ExportProject(string waterMark, ProjectExporterParameters projectExporterParameters)
		{
			bool result = false;
			if (this.SaveProject())
			{
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.vlProject.GetProjectFileName());
				if (DialogResult.OK == GenericSaveFileDialog.ShowDialog(FileType.PackAndGoFile, fileNameWithoutExtension, this.configManager.LoggerSpecifics.Name, this.vlProject.GetProjectFolder()))
				{
					this.ExportProject(GenericSaveFileDialog.FileName, waterMark, projectExporterParameters);
				}
			}
			return result;
		}

		private bool ExportProject(string zipFileName, string waterMark, ProjectExporterParameters projectExporterParameters)
		{
			bool result = false;
			if (projectExporterParameters.ProtectWithPassword && projectExporterParameters.SetPasswordOnDemand)
			{
				PasswordDialog passwordDialog = new PasswordDialog(PasswordDialog.Context.PackAndGo, true);
				if (DialogResult.OK != passwordDialog.ShowDialog())
				{
					return result;
				}
				projectExporterParameters.Password = passwordDialog.Password;
				if (passwordDialog.RememberPassword)
				{
					this.AppDataAccess.AppDataRoot.ProjectExporterPage.ProjectExporterParameters.RememberPassword(passwordDialog.Password);
				}
			}
			ProjectExporter projectExporter = new ProjectExporter(zipFileName, projectExporterParameters);
			projectExporter.ErrorHandler += new ErrorHandler(this.ProjectExporterErrorHandler);
			result = projectExporter.Export(ref this.vlProject, waterMark);
			projectExporter.ErrorHandler -= new ErrorHandler(this.ProjectExporterErrorHandler);
			return result;
		}

		private void ImportAnalysisPackageFromDevice()
		{
			if (this.device != null)
			{
				if (this.device.HasAnalysisPackage())
				{
					using (BrowseFolderDialog browseFolderDialog = new BrowseFolderDialog())
					{
						browseFolderDialog.Title = Resources.SelectDestFolder;
						if (DialogResult.OK == browseFolderDialog.ShowDialog() && this.device.CopyAnalysisPackage(browseFolderDialog.SelectedPath))
						{
							InformMessageBox.Info(Resources.ImportAnalysisPackageFinished);
						}
						return;
					}
				}
				InformMessageBox.Error(Resources.AnalysisPackageNotFoundOnDevice);
			}
		}

		private void ImportAnalysisPackageFromMemoryCard()
		{
			string text;
			if (!CardReaderDriveSelection.SelectCardReaderDrive(this.configManager.LoggerSpecifics, this.hardwareFrontend, this.hardwareFrontend.AdditionalDrives, out text, true, LoggerType.Unknown))
			{
				return;
			}
			if (!this.IsMemoryCardAccessible(text))
			{
				InformMessageBox.Error(Resources.ErrorUnableToAccessDriveWillUpdate);
				this.hardwareFrontend.UpdateLoggerDeviceList();
				return;
			}
			ILoggerDevice loggerDeviceByDriveName = this.hardwareFrontend.GetLoggerDeviceByDriveName(text);
			if (loggerDeviceByDriveName == null)
			{
				InformMessageBox.Error(string.Format(Resources.ErrorCannotAccessMemoryCardinDrive, text));
				return;
			}
			if (loggerDeviceByDriveName.HasAnalysisPackage())
			{
				using (BrowseFolderDialog browseFolderDialog = new BrowseFolderDialog())
				{
					browseFolderDialog.Title = Resources.SelectDestFolder;
					if (DialogResult.OK == browseFolderDialog.ShowDialog() && loggerDeviceByDriveName.CopyAnalysisPackage(browseFolderDialog.SelectedPath))
					{
						InformMessageBox.Info(Resources.ImportAnalysisPackageFinished);
					}
					return;
				}
			}
			InformMessageBox.Error(Resources.AnalysisPackageNotFoundOnDevice);
		}

		private bool SaveAppDataSettings()
		{
			bool flag = true;
			flag &= this.databases.Serialize(this.mAppDataAccess.AppDataRoot.DatabasesPage);
			flag &= this.filters1.Serialize(this.mAppDataAccess.AppDataRoot.Filters1Page);
			flag &= this.filters2.Serialize(this.mAppDataAccess.AppDataRoot.Filters2Page);
			flag &= this.triggers1.Serialize(this.mAppDataAccess.AppDataRoot.Triggers1Page);
			flag &= this.triggers2.Serialize(this.mAppDataAccess.AppDataRoot.Triggers2Page);
			flag &= this.clfExport.Serialize(this.mAppDataAccess.AppDataRoot.CLFExportPage);
			flag &= this.cardReader.Serialize(this.mAppDataAccess.AppDataRoot.CardReaderPage);
			flag &= this.cardReaderNavigator.Serialize(this.mAppDataAccess.AppDataRoot.CardReaderNavigatorPage);
			flag &= this.includeFiles.Serialize(this.mAppDataAccess.AppDataRoot.IncludeFilesPage);
			flag &= this.ccpXcpDescriptions.Serialize(this.mAppDataAccess.AppDataRoot.CcpXcpDescriptionsPage);
			flag &= this.ccpXcpSignalRequests.Serialize(this.mAppDataAccess.AppDataRoot.CcpXcpSignalRequestsPage);
			flag &= this.loggerDevice.Serialize(this.mAppDataAccess.AppDataRoot.LoggerDevicePage);
			flag &= this.loggerDeviceNavigator.Serialize(this.mAppDataAccess.AppDataRoot.LoggerDeviceNavigatorPage);
			flag &= this.diagnosticsDatabases.Serialize(this.mAppDataAccess.AppDataRoot.DiagnosticsDatabasesPage);
			flag &= this.diagnosticActions.Serialize(this.mAppDataAccess.AppDataRoot.DiagnosticActionsPage);
			flag &= this.sendMessage.Serialize(this.mAppDataAccess.AppDataRoot.SendMessagePage);
			flag &= this.digitalOutputs.Serialize(this.mAppDataAccess.AppDataRoot.DigitalOutputsPage);
			flag &= this.wlanSettings.Serialize<WLANSettingsGL3PlusPage>(this.mAppDataAccess.AppDataRoot.WLANSettingsGL3PlusPage);
			flag &= this.wlanSettings.Serialize<WLANSettingsGL2000Page>(this.mAppDataAccess.AppDataRoot.WLANSettingsGL2000Page);
			flag &= this.interfaceMode.Serialize(this.mAppDataAccess.AppDataRoot.SignalExportListPage);
			this.mAppDataAccess.AppDataRoot.GlobalOptions.MainWindowTop = base.Top;
			this.mAppDataAccess.AppDataRoot.GlobalOptions.MainWindowLeft = base.Left;
			this.mAppDataAccess.AppDataRoot.GlobalOptions.MainWindowHeight = base.Height;
			this.mAppDataAccess.AppDataRoot.GlobalOptions.MainWindowWidth = base.Width;
			this.mAppDataAccess.AppDataRoot.GlobalOptions.IsAutoEjectEnabled = this.isAutoEjectEnabled;
			this.mAppDataAccess.AppDataRoot.GlobalOptions.MemoryCardSize = RingBufferMemoriesManager.CurrentCardSizeMB;
			return flag & this.mAppDataAccess.SaveAppDataSettings();
		}

		private bool LoadAppDataSettings()
		{
			bool flag = true;
			flag &= this.databases.DeSerialize(this.mAppDataAccess.AppDataRoot.DatabasesPage);
			flag &= this.filters1.DeSerialize(this.mAppDataAccess.AppDataRoot.Filters1Page);
			flag &= this.filters2.DeSerialize(this.mAppDataAccess.AppDataRoot.Filters2Page);
			flag &= this.triggers1.DeSerialize(this.mAppDataAccess.AppDataRoot.Triggers1Page);
			flag &= this.triggers2.DeSerialize(this.mAppDataAccess.AppDataRoot.Triggers2Page);
			flag &= this.clfExport.DeSerialize(this.mAppDataAccess.AppDataRoot.CLFExportPage);
			flag &= this.cardReader.DeSerialize(this.mAppDataAccess.AppDataRoot.CardReaderPage);
			flag &= this.cardReaderNavigator.DeSerialize(this.mAppDataAccess.AppDataRoot.CardReaderNavigatorPage);
			flag &= this.includeFiles.DeSerialize(this.mAppDataAccess.AppDataRoot.IncludeFilesPage);
			flag &= this.ccpXcpDescriptions.DeSerialize(this.mAppDataAccess.AppDataRoot.CcpXcpDescriptionsPage);
			flag &= this.ccpXcpSignalRequests.DeSerialize(this.mAppDataAccess.AppDataRoot.CcpXcpSignalRequestsPage);
			flag &= this.loggerDevice.DeSerialize(this.mAppDataAccess.AppDataRoot.LoggerDevicePage);
			flag &= this.loggerDeviceNavigator.DeSerialize(this.mAppDataAccess.AppDataRoot.LoggerDeviceNavigatorPage);
			flag &= this.diagnosticsDatabases.DeSerialize(this.mAppDataAccess.AppDataRoot.DiagnosticsDatabasesPage);
			flag &= this.diagnosticActions.DeSerialize(this.mAppDataAccess.AppDataRoot.DiagnosticActionsPage);
			flag &= this.sendMessage.DeSerialize(this.mAppDataAccess.AppDataRoot.SendMessagePage);
			flag &= this.digitalOutputs.DeSerialize(this.mAppDataAccess.AppDataRoot.DigitalOutputsPage);
			flag &= this.wlanSettings.DeSerialize<WLANSettingsGL3PlusPage>(this.mAppDataAccess.AppDataRoot.WLANSettingsGL3PlusPage);
			flag &= this.wlanSettings.DeSerialize<WLANSettingsGL2000Page>(this.mAppDataAccess.AppDataRoot.WLANSettingsGL2000Page);
			flag &= this.interfaceMode.DeSerialize(this.mAppDataAccess.AppDataRoot.SignalExportListPage);
			this.loggerTypeToStartWith = this.AppDataAccess.AppDataRoot.GlobalOptions.LoggerTypeToStartWith;
			this.isAutoEjectEnabled = this.AppDataAccess.AppDataRoot.GlobalOptions.IsAutoEjectEnabled;
			RingBufferMemoriesManager.CurrentCardSizeMB = this.AppDataAccess.AppDataRoot.GlobalOptions.MemoryCardSize;
			this.toolStripButtonHexMode.Checked = this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode.IsHexadecimal;
			this.hexadecimalDisplayToolStripMenuItem.Checked = this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode.IsHexadecimal;
			this.applicationDatabaseManager.IsHexDisplay = this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode.IsHexadecimal;
			GUIUtil.IsHexadecimal = this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode.IsHexadecimal;
			((IUpdateService)this).NotifyDisplayMode(this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode);
			this.hardwareFrontend.AdditionalDrives = this.mAppDataAccess.AppDataRoot.GlobalOptions.AdditionalDrivesList;
			GlobalOptionsManager.GlobalOptions = this.mAppDataAccess.AppDataRoot.GlobalOptions;
			if (this.mAppDataAccess.AppDataRoot.ProjectExporterPage.ProjectExporterParameters == null)
			{
				this.mAppDataAccess.AppDataRoot.ProjectExporterPage.ProjectExporterParameters = new ProjectExporterParameters();
			}
			if (this.mAppDataAccess.AppDataRoot.AnalysisPackageSettings.AnalysisPackageParameters == null)
			{
				this.mAppDataAccess.AppDataRoot.AnalysisPackageSettings.AnalysisPackageParameters = new AnalysisPackageParameters();
			}
			return flag;
		}

		private void UpdateLRU(string fileName)
		{
			this.lruFileHandler.AddFileToLRUList(fileName);
		}

		private void OnLRUFilesClicked(object sender, EventArgs e)
		{
			ToolStripItem toolStripItem = sender as ToolStripItem;
			if (toolStripItem != null)
			{
				string filenameForMenuItem = this.lruFileHandler.GetFilenameForMenuItem(toolStripItem.Text);
				FileInfo fileInfo = new FileInfo(filenameForMenuItem);
				if (!fileInfo.Exists)
				{
					this.lruFileHandler.RemoveFileFromLRUList(filenameForMenuItem);
					InformMessageBox.Error(Resources.ErrorSelectedLRUFileNotFound);
					return;
				}
				if (!this.OnBeforeUnloadingCurrentConfiguration())
				{
					return;
				}
				bool checkForMissingDiagDescriptions = true;
				this.LoadFile(filenameForMenuItem, checkForMissingDiagDescriptions);
			}
		}

		private void ExportToLtlOrCodFile(bool bLtl)
		{
			if (!this.CheckAnyPageForErrorsAndDenyAction())
			{
				return;
			}
			using (AnalysisFileCollector analysisFileCollector = AnalysisFileCollector.Create())
			{
				analysisFileCollector.SoundnessCheckPerformed = true;
				List<string> errorTextList = new List<string>();
				Result result = this.GenerateCompilerIndependentFilesForAnalysis(analysisFileCollector, errorTextList, true);
				if (!MainWindow.ShowErrorsOfWriteAction(result, errorTextList))
				{
					FileType fileType = bLtl ? FileType.LTLFile : FileType.CODFile;
					if (DialogResult.OK == GenericSaveFileDialog.ShowDialog(fileType, this.vlProject.GetProjectFileName(), this.configManager.LoggerSpecifics.Name, this.vlProject.GetProjectFolder()))
					{
						string fileName = GenericSaveFileDialog.FileName;
						if (this.vlProject.ProjectRoot.LoggerType != LoggerType.VN1630log)
						{
							string text;
							string compilerErrFilePath;
							bool flag;
							result = GenerationUtil.ExportToLtlOrCodFile(fileName, out text, out compilerErrFilePath, out flag);
							if (result == Result.OK)
							{
								DateTime dt = DateTime.Now;
								if (!bLtl && File.Exists(fileName))
								{
									dt = FileSystemServices.ExtractCODCompileTime(fileName);
								}
								string text2 = string.Format(Resources.FileWriteSuccess, Path.GetFileName(fileName));
								string text3;
								Result result2 = this.ProcessAnalysisFilesAfterWriteAction(analysisFileCollector, Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName), dt, out text3);
								if (result2 == Result.OK && File.Exists(text3))
								{
									this.ShowResultOfWriteAction(text2, text3, false, null);
								}
								else
								{
									if (result2 == Result.OK && !File.Exists(text3))
									{
										text2 = text2 + "\n\n" + Resources.FileGeneratedForAnalysisNotWritten;
									}
									else if (result2 == Result.Error)
									{
										text2 = text2 + "\n\n" + string.Format(Resources.ErrorFailedToCreateAnalysisPackage, text3);
									}
									this.ShowResultOfWriteAction(text2, null, false, null);
								}
							}
							else if (result == Result.Error)
							{
								if (bLtl)
								{
									if (flag)
									{
										GUIUtil.ReportFileGenerationResult(EnumInfoType.Info, string.Format(Resources.FileCreatedButNotCompilable, Path.GetFileName(fileName)), compilerErrFilePath);
									}
									else
									{
										InformMessageBox.Error(text);
									}
								}
								else
								{
									GUIUtil.ReportFileGenerationResult(EnumInfoType.Error, text, compilerErrFilePath);
								}
							}
						}
					}
				}
			}
		}

		private Result GenerateCompilerIndependentFilesForAnalysis(AnalysisFileCollector analysisFileCollector, List<string> errorTextList, bool bAbortOnError = true)
		{
			Result result = Result.OK;
			using (new WaitCursor())
			{
				string item;
				Result result2 = GenerationUtil.GenerateFilesForCcpXcp(analysisFileCollector.GeneralPurposeTempDirectory, out item);
				if (result2 == Result.Error)
				{
					errorTextList.Add(item);
					result = Result.Error;
				}
				if ((result == Result.OK || !bAbortOnError) && this.configManager.LoggerSpecifics.GPS.HasSerialGPS)
				{
					result2 = GenerationUtil.GenerateDBCFileForGPSConfiguration(analysisFileCollector.GeneralPurposeTempDirectory, this.mAppDataAccess, out item);
					if (result2 == Result.Error)
					{
						errorTextList.Add(item);
						result = Result.Error;
					}
				}
				string message;
				if ((result == Result.OK || !bAbortOnError) && this.configManager.LoggerSpecifics.Type != LoggerType.GL1000 && this.configManager.LoggerSpecifics.Type != LoggerType.GL1020FTE && this.configManager.LoggerSpecifics.Type != LoggerType.VN1630log && (analysisFileCollector.SoundnessCheckPerformed || this.configManager.SemanticChecker.IsTriggerConfigurationCaplCompliant(out message) || InformMessageBox.Question(message) != DialogResult.No))
				{
					result2 = GenerationUtil.GenerateVSysVarFileFromConfig(analysisFileCollector.GeneralPurposeTempDirectory, out item);
					if (result2 == Result.Error)
					{
						errorTextList.Add(item);
						result = Result.Error;
					}
				}
			}
			return result;
		}

		private void WriteToMemoryCard()
		{
			if (!this.CheckAnyPageForErrorsAndDenyAction())
			{
				return;
			}
			using (AnalysisFileCollector analysisFileCollector = AnalysisFileCollector.Create())
			{
				analysisFileCollector.SoundnessCheckPerformed = true;
				List<string> errorTextList = new List<string>();
				Result result = this.GenerateCompilerIndependentFilesForAnalysis(analysisFileCollector, errorTextList, true);
				if (!MainWindow.ShowErrorsOfWriteAction(result, errorTextList))
				{
					string text;
					if (CardReaderDriveSelection.SelectCardReaderDrive(this.configManager.LoggerSpecifics, this.hardwareFrontend, this.hardwareFrontend.AdditionalDrives, out text, false, LoggerType.Unknown))
					{
						if (!this.IsMemoryCardAccessible(text))
						{
							InformMessageBox.Error(Resources.ErrorUnableToAccessDriveWillUpdate);
							this.hardwareFrontend.UpdateLoggerDeviceList();
						}
						else
						{
							ILoggerDevice loggerDeviceByDriveName = this.hardwareFrontend.GetLoggerDeviceByDriveName(text);
							if (loggerDeviceByDriveName == null)
							{
								InformMessageBox.Error(string.Format(Resources.ErrorCannotAccessMemoryCardinDrive, text));
							}
							else
							{
								if (this.configManager.LoggerSpecifics.DeviceAccess.IsMemoryCardFormattingSupported)
								{
									if (!loggerDeviceByDriveName.IsFAT32Formatted)
									{
										InformMessageBox.Error(Resources.ErrorDriveIsNotFAT32Formatted + " " + Resources.UseFormatMemCardForThis);
										return;
									}
									if (!loggerDeviceByDriveName.HasProperClusterSize && InformMessageBox.Question(Resources.MemCardNotOptimalClusterSize + Resources.RecomUseFormatMemCard + Resources.QuestionContinueAnyway) == DialogResult.No)
									{
										return;
									}
								}
								string waterMark;
								bool flag = loggerDeviceByDriveName.WriteConfiguration(out waterMark, true);
								this.hardwareFrontend.UpdateLoggerDeviceList();
								if (flag)
								{
									DateTime dt = DateTime.Now;
									string[] cODFilePath = loggerDeviceByDriveName.GetCODFilePath();
									if (cODFilePath.Count<string>() > 0 && !string.IsNullOrEmpty(cODFilePath[0]) && File.Exists(cODFilePath[0]))
									{
										dt = FileSystemServices.ExtractCODCompileTime(cODFilePath[0]);
									}
									string text2;
									Result result2 = this.ProcessAnalysisFilesAfterWriteAction(analysisFileCollector, null, null, dt, out text2);
									bool flag2 = this.CanWritePackAndGoAndAnalysisPackage(loggerDeviceByDriveName);
									if (flag2)
									{
										this.WriteProjectZIPFileToDeviceOrMemoryCard(loggerDeviceByDriveName, waterMark);
									}
									string text3 = Resources.ConfigSuccessfullyWritten;
									string analysisPath = null;
									bool analysisPackageWrittenToMemoryCard = false;
									if (result2 == Result.OK && File.Exists(text2))
									{
										if (flag2)
										{
											analysisPackageWrittenToMemoryCard = this.WriteAnalysisPackageToDeviceOrMemoryCard(loggerDeviceByDriveName, text2);
										}
										else
										{
											text3 += "\n\n";
											text3 += Resources.ErrorCannotWritePackAndGoAndAnalysisPackage;
										}
										analysisPath = text2;
									}
									else if (result2 == Result.OK && !File.Exists(text2))
									{
										text3 = text3 + "\n\n" + Resources.FileGeneratedForAnalysisNotWritten;
									}
									else if (result2 == Result.Error)
									{
										text3 = text3 + "\n\n" + string.Format(Resources.ErrorFailedToCreateAnalysisPackage, text2);
									}
									bool flag3 = this.hardwareFrontend.AdditionalDrives.Contains(text);
									if (flag3)
									{
										this.ShowResultOfWriteAction(text3, analysisPath, analysisPackageWrittenToMemoryCard, null);
										this.hardwareFrontend.UpdateLoggerDeviceList();
									}
									else
									{
										this.ShowResultOfWriteAction(text3, analysisPath, analysisPackageWrittenToMemoryCard, Resources.EjectMemoryCard);
										if (this.isAutoEjectEnabled && this.configManager.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
										{
											this.hardwareFrontend.Eject(loggerDeviceByDriveName);
										}
										else
										{
											this.hardwareFrontend.UpdateLoggerDeviceList();
										}
									}
								}
							}
						}
					}
				}
			}
		}

		private void CopyCODFileToMemoryCard()
		{
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK != GenericOpenFileDialog.ShowDialog(FileType.CODFile))
			{
				return;
			}
			string text;
			if (!CardReaderDriveSelection.SelectCardReaderDrive(this.configManager.LoggerSpecifics, this.hardwareFrontend, this.hardwareFrontend.AdditionalDrives, out text, false, LoggerType.Unknown))
			{
				return;
			}
			if (!this.IsMemoryCardAccessible(text))
			{
				InformMessageBox.Error(Resources.ErrorUnableToAccessDriveWillUpdate);
				this.hardwareFrontend.UpdateLoggerDeviceList();
				return;
			}
			ILoggerDevice loggerDeviceByDriveName = this.hardwareFrontend.GetLoggerDeviceByDriveName(text);
			if (loggerDeviceByDriveName == null)
			{
				InformMessageBox.Error(string.Format(Resources.ErrorCannotAccessMemoryCardinDrive, text));
				return;
			}
			if (this.configManager.LoggerSpecifics.DeviceAccess.IsMemoryCardFormattingSupported)
			{
				if (!loggerDeviceByDriveName.IsFAT32Formatted)
				{
					InformMessageBox.Error(Resources.ErrorDriveIsNotFAT32Formatted + " " + Resources.UseFormatMemCardForThis);
					return;
				}
				if (!loggerDeviceByDriveName.HasProperClusterSize && InformMessageBox.Question(Resources.MemCardNotOptimalClusterSize + Resources.RecomUseFormatMemCard + Resources.QuestionContinueAnyway) == DialogResult.No)
				{
					return;
				}
			}
			if (!loggerDeviceByDriveName.WriteConfiguration(GenericOpenFileDialog.FileName, true))
			{
				this.hardwareFrontend.UpdateLoggerDeviceList();
				return;
			}
			string fileName = Path.GetFileName(GenericOpenFileDialog.FileName);
			DateTime dt = FileSystemServices.ExtractCODCompileTime(GenericOpenFileDialog.FileName);
			string matchingPackage = Vector.VLConfig.BusinessLogic.Conversion.AnalysisPackage.GetMatchingPackage(Path.GetFileNameWithoutExtension(fileName), dt, Path.GetDirectoryName(GenericOpenFileDialog.FileName));
			bool flag = false;
			if (!string.IsNullOrEmpty(matchingPackage) && File.Exists(matchingPackage))
			{
				flag = this.WriteAnalysisPackageToDeviceOrMemoryCard(loggerDeviceByDriveName, matchingPackage);
			}
			bool flag2 = this.hardwareFrontend.AdditionalDrives.Contains(text);
			if (flag2)
			{
				if (flag)
				{
					this.ShowResultOfWriteAction(Resources.ConfigAndAnalysisPackageSuccessfullyWritten, null, false, null);
					return;
				}
				this.ShowResultOfWriteAction(Resources.ConfigSuccessfullyWritten, null, false, null);
				return;
			}
			else
			{
				string successMsg;
				if (flag)
				{
					successMsg = string.Format(Resources.ConfigAndAnalysisPackageSuccessfullyWrittenToCard, fileName, text);
				}
				else
				{
					successMsg = string.Format(Resources.ConfigSuccessfullyWrittenToCard, fileName, text);
				}
				this.ShowResultOfWriteAction(successMsg, null, false, Resources.EjectDevice);
				if (this.isAutoEjectEnabled && this.configManager.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
				{
					this.hardwareFrontend.Eject(loggerDeviceByDriveName);
					return;
				}
				this.hardwareFrontend.UpdateLoggerDeviceList();
				return;
			}
		}

		private void WriteCODFileToDevice()
		{
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK != GenericOpenFileDialog.ShowDialog(FileType.CODFile))
			{
				return;
			}
			this.hardwareFrontend.UpdateLoggerDeviceList();
			if (this.device == null)
			{
				InformMessageBox.Error(Resources.ErrorNoLoggerDevConnected);
				return;
			}
			if (this.configManager.LoggerSpecifics.DeviceAccess.IsMemoryCardFormattingSupported)
			{
				if (!this.device.IsFAT32Formatted)
				{
					InformMessageBox.Error(Resources.ErrorDriveIsNotFAT32Formatted + " " + Resources.UseFormatMemCardForThis);
					return;
				}
				if (!this.device.HasProperClusterSize && InformMessageBox.Question(Resources.MemCardNotOptimalClusterSize + Resources.RecomUseFormatMemCardInDevice + Resources.QuestionContinueAnyway) == DialogResult.No)
				{
					return;
				}
			}
			bool flag = this.device.WriteConfiguration(GenericOpenFileDialog.FileName, true);
			if (flag)
			{
				string fileName = Path.GetFileName(GenericOpenFileDialog.FileName);
				DateTime dt = FileSystemServices.ExtractCODCompileTime(GenericOpenFileDialog.FileName);
				string matchingPackage = Vector.VLConfig.BusinessLogic.Conversion.AnalysisPackage.GetMatchingPackage(Path.GetFileNameWithoutExtension(fileName), dt, Path.GetDirectoryName(GenericOpenFileDialog.FileName));
				bool flag2 = false;
				if (!string.IsNullOrEmpty(matchingPackage) && File.Exists(matchingPackage))
				{
					flag2 = this.WriteAnalysisPackageToDeviceOrMemoryCard(this.device, matchingPackage);
				}
				if (flag2)
				{
					this.ShowResultOfWriteAction(string.Format(Resources.CODAndAnalysisPackageFileSuccessfullyWrittenToDev, fileName), null, false, null);
				}
				else
				{
					this.ShowResultOfWriteAction(string.Format(Resources.CODFileSuccessfullyWrittenToDev, fileName), null, false, null);
				}
			}
			this.hardwareFrontend.UpdateLoggerDeviceList();
		}

		private void WriteConfigurationToDevice()
		{
			if (!this.CheckAnyPageForErrorsAndDenyAction())
			{
				return;
			}
			using (AnalysisFileCollector analysisFileCollector = AnalysisFileCollector.Create())
			{
				analysisFileCollector.SoundnessCheckPerformed = true;
				List<string> errorTextList = new List<string>();
				Result result = this.GenerateCompilerIndependentFilesForAnalysis(analysisFileCollector, errorTextList, true);
				if (MainWindow.ShowErrorsOfWriteAction(result, errorTextList))
				{
					return;
				}
				if (this.device == null)
				{
					InformMessageBox.Error(Resources.ErrorNoLoggerDevConnected);
					return;
				}
				if (this.configManager.LoggerSpecifics.DeviceAccess.IsMemoryCardFormattingSupported)
				{
					if (!this.device.IsFAT32Formatted)
					{
						InformMessageBox.Error(Resources.ErrorDriveIsNotFAT32Formatted + " " + Resources.UseFormatMemCardForThis);
						return;
					}
					if (!this.device.HasProperClusterSize && InformMessageBox.Question(Resources.MemCardNotOptimalClusterSize + Resources.RecomUseFormatMemCardInDevice + Resources.QuestionContinueAnyway) == DialogResult.No)
					{
						return;
					}
				}
				string str;
				if (!this.configManager.SemanticChecker.IsConfigurationSoundForOnlineLogger(this.device, out str) && InformMessageBox.Question(str + Environment.NewLine + Resources.WriteConfigToDevAnyway) == DialogResult.No)
				{
					return;
				}
				string waterMark;
				bool flag = this.device.WriteConfiguration(out waterMark, true);
				if (flag)
				{
					DateTime dt = DateTime.Now;
					string[] cODFilePath = this.device.GetCODFilePath();
					if (cODFilePath.Count<string>() > 0 && !string.IsNullOrEmpty(cODFilePath[0]) && File.Exists(cODFilePath[0]))
					{
						dt = FileSystemServices.ExtractCODCompileTime(cODFilePath[0]);
					}
					string text;
					Result result2 = this.ProcessAnalysisFilesAfterWriteAction(analysisFileCollector, null, null, dt, out text);
					bool flag2 = this.CanWritePackAndGoAndAnalysisPackage(this.device);
					if (flag2)
					{
						this.WriteProjectZIPFileToDeviceOrMemoryCard(this.device, waterMark);
					}
					string text2 = Resources.ConfigSuccessfullyWrittenToDev;
					string analysisPath = null;
					bool analysisPackageWrittenToMemoryCard = false;
					if (result2 == Result.OK && File.Exists(text))
					{
						if (flag2)
						{
							analysisPackageWrittenToMemoryCard = this.WriteAnalysisPackageToDeviceOrMemoryCard(this.device, text);
						}
						else
						{
							text2 += "\n\n";
							text2 += Resources.ErrorCannotWritePackAndGoAndAnalysisPackage;
						}
						analysisPath = text;
					}
					else if (result2 == Result.OK && !File.Exists(text))
					{
						text2 = text2 + "\n\n" + Resources.FileGeneratedForAnalysisNotWritten;
					}
					else if (result2 == Result.Error)
					{
						text2 = text2 + "\n\n" + string.Format(Resources.ErrorFailedToCreateAnalysisPackage, text);
					}
					if (this.device.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
					{
						this.ShowResultOfWriteAction(text2, analysisPath, analysisPackageWrittenToMemoryCard, Resources.EjectDevice);
						if (this.device != null && this.isAutoEjectEnabled)
						{
							this.hardwareFrontend.Eject(this.device);
						}
					}
					else
					{
						this.ShowResultOfWriteAction(text2, analysisPath, false, null);
					}
				}
			}
			this.hardwareFrontend.UpdateLoggerDeviceList();
		}

		private bool CanWritePackAndGoAndAnalysisPackage(ILoggerDevice device2Write)
		{
			return device2Write.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem || !this.AppDataAccess.AppDataRoot.AnalysisPackageSettings.AnalysisPackageParameters.ExportToMemoryCard || !this.mAppDataAccess.AppDataRoot.ProjectExporterPage.ProjectExporterParameters.ExportToMemoryCard;
		}

		private bool WriteAnalysisPackageToDeviceOrMemoryCard(ILoggerDevice device2Write, string analysisPackagePath)
		{
			if (this.AppDataAccess.AppDataRoot.AnalysisPackageSettings.AnalysisPackageParameters.ExportToMemoryCard && device2Write.LoggerSpecifics.Configuration.SupportsAnalysisPackage)
			{
				if (this.AppDataAccess.AppDataRoot.AnalysisPackageSettings.AnalysisPackageParameters.AskPriorToExport && InformMessageBox.Question(string.Format(Resources.QuestionWritetAnalysisPackageToMemoryCard, new object[0])) == DialogResult.No)
				{
					return false;
				}
				try
				{
					return device2Write.WriteAnalysisPackage(analysisPackagePath);
				}
				catch
				{
					InformMessageBox.Error(Resources.ErrorFailedToWriteAnalysisPackageToMemoryCard);
				}
				return false;
			}
			return false;
		}

		private void WriteProjectZIPFileToDeviceOrMemoryCard(ILoggerDevice device2Write, string waterMark)
		{
			if (this.mAppDataAccess.AppDataRoot.ProjectExporterPage.ProjectExporterParameters.ExportToMemoryCard && this.SaveProject())
			{
				if (this.mAppDataAccess.AppDataRoot.ProjectExporterPage.ProjectExporterParameters.AskPriorToExport && InformMessageBox.Question(string.Format(Resources.QuestionExportProjectToMemoryCard, new object[0])) == DialogResult.No)
				{
					return;
				}
				ProjectExporterParameters projectExporterParameters = new ProjectExporterParameters(this.mAppDataAccess.AppDataRoot.ProjectExporterPage.ProjectExporterParameters);
				string text = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(this.vlProject.GetProjectFileName()) + ".zip");
				try
				{
					if (this.ExportProject(text, waterMark, projectExporterParameters))
					{
						device2Write.WriteProjectZIPFile(text);
					}
					else
					{
						InformMessageBox.Error(Resources.ErrorFailedExportProjectToMemoryCard);
					}
				}
				catch (IOException ex)
				{
					if (ex.Message == Resources.GIN_EC_File2Large4UserData)
					{
						string message = ex.Message + "\n" + Resources.ProjectExportExcludeDatabases;
						if (InformMessageBox.Question(message) != DialogResult.Yes)
						{
							goto IL_158;
						}
						try
						{
							projectExporterParameters.ExportBusDatabases = false;
							projectExporterParameters.ExportCCPXCPDatabases = false;
							projectExporterParameters.ExportDiagnosticDescriptions = false;
							projectExporterParameters.ExportNonBusDatabases = false;
							if (this.ExportProject(text, waterMark, projectExporterParameters))
							{
								device2Write.WriteProjectZIPFile(text);
							}
							goto IL_158;
						}
						catch
						{
							InformMessageBox.Error(Resources.ErrorFailedExportProjectToMemoryCard);
							goto IL_158;
						}
					}
					InformMessageBox.Error(Resources.ErrorFailedExportProjectToMemoryCard);
					IL_158:;
				}
				catch
				{
					InformMessageBox.Error(Resources.ErrorFailedExportProjectToMemoryCard);
				}
				string text2;
				GenerationUtil.TryDeleteFile(text, out text2, true);
			}
		}

		private void ClearDevice()
		{
			if (this.device == null)
			{
				InformMessageBox.Error(Resources.ErrorNoLoggerDevConnected);
				return;
			}
			if (this.device.Clear())
			{
				InformMessageBox.Info(Resources.DeviceSuccessfullyCleared);
				this.hardwareFrontend.EnforceExplicitUpdateOfLoggerDevice(this.device);
			}
		}

		private void ClearMemoryCard()
		{
			string text;
			if (!CardReaderDriveSelection.SelectCardReaderDrive(this.configManager.LoggerSpecifics, this.hardwareFrontend, this.hardwareFrontend.AdditionalDrives, out text, false, LoggerType.Unknown))
			{
				return;
			}
			if (!this.IsMemoryCardAccessible(text))
			{
				InformMessageBox.Error(Resources.ErrorUnableToAccessDriveWillUpdate);
				this.hardwareFrontend.UpdateLoggerDeviceList();
				return;
			}
			ILoggerDevice loggerDeviceByDriveName = this.hardwareFrontend.GetLoggerDeviceByDriveName(text);
			if (loggerDeviceByDriveName == null)
			{
				InformMessageBox.Error(string.Format(Resources.ErrorCannotAccessMemoryCardinDrive, text));
				return;
			}
			if (!loggerDeviceByDriveName.Clear())
			{
				return;
			}
			InformMessageBox.Info(string.Format(Resources.MemCardSuccessfullyCleared, text));
			this.hardwareFrontend.EnforceExplicitUpdateOfLoggerDevice(loggerDeviceByDriveName);
		}

		private void FormatMemoryCardInDevice()
		{
			if (!this.configManager.LoggerSpecifics.DeviceAccess.IsMemoryCardFormattingSupported)
			{
				return;
			}
			if (this.device != null)
			{
				using (new WaitCursor())
				{
					this.device.FormatCard();
				}
			}
			this.hardwareFrontend.EnforceExplicitUpdateOfLoggerDevice(this.device);
		}

		private void FormatMemoryCard()
		{
			if (!this.configManager.LoggerSpecifics.DeviceAccess.IsMemoryCardFormattingSupported)
			{
				return;
			}
			string text;
			if (!CardReaderDriveSelection.SelectCardReaderDrive(this.configManager.LoggerSpecifics, this.hardwareFrontend, this.hardwareFrontend.AdditionalDrives, out text, false, LoggerType.Unknown))
			{
				return;
			}
			if (!this.IsMemoryCardAccessible(text))
			{
				InformMessageBox.Error(Resources.ErrorUnableToAccessDriveWillUpdate);
				this.hardwareFrontend.UpdateLoggerDeviceList();
				return;
			}
			ILoggerDevice loggerDeviceByDriveName = this.hardwareFrontend.GetLoggerDeviceByDriveName(text);
			if (loggerDeviceByDriveName == null)
			{
				InformMessageBox.Error(string.Format(Resources.ErrorCannotAccessMemoryCardinDrive, text));
				return;
			}
			using (new WaitCursor())
			{
				loggerDeviceByDriveName.FormatCard();
			}
			this.hardwareFrontend.EnforceExplicitUpdateOfLoggerDevice(loggerDeviceByDriveName);
		}

		private void WriteLicenseToMemoryCard()
		{
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(FileType.LICFile) && this.configManager.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
			{
				string text;
				if (!CardReaderDriveSelection.SelectCardReaderDrive(this.configManager.LoggerSpecifics, this.hardwareFrontend, this.hardwareFrontend.AdditionalDrives, out text, false, LoggerType.Unknown))
				{
					return;
				}
				if (!this.IsMemoryCardAccessible(text))
				{
					InformMessageBox.Error(Resources.ErrorUnableToAccessDriveWillUpdate);
					this.hardwareFrontend.UpdateLoggerDeviceList();
					return;
				}
				ILoggerDevice loggerDeviceByDriveName = this.hardwareFrontend.GetLoggerDeviceByDriveName(text);
				if (loggerDeviceByDriveName == null)
				{
					InformMessageBox.Error(string.Format(Resources.ErrorCannotAccessMemoryCardinDrive, text));
					return;
				}
				loggerDeviceByDriveName.WriteLicense(GenericOpenFileDialog.FileName);
				this.hardwareFrontend.UpdateLoggerDeviceList();
			}
		}

		private void WriteLicenseToDevice()
		{
			if (this.device == null)
			{
				InformMessageBox.Error(Resources.ErrorNoLoggerDevConnected);
				return;
			}
			GenericOpenFileDialog.FileName = "";
			if (DialogResult.OK != GenericOpenFileDialog.ShowDialog(FileType.LICFile))
			{
				return;
			}
			string hardwareKey = this.device.HardwareKey;
			this.hardwareFrontend.EnableDeviceWatcher(this.device, false);
			this.device.WriteLicense(GenericOpenFileDialog.FileName);
			if (this.device != null && hardwareKey == this.device.HardwareKey && Directory.Exists(hardwareKey))
			{
				this.hardwareFrontend.EnableDeviceWatcher(this.device, true);
			}
			this.hardwareFrontend.UpdateLoggerDeviceList();
		}

		private void SetRealTimeClock()
		{
			this.setRealtimeClockDialog.LoggerSpecifics = this.configManager.LoggerSpecifics;
			this.setRealtimeClockDialog.LoggerDevice = this.device;
			this.setRealtimeClockDialog.SelectedCOMPort = this.AppDataAccess.AppDataRoot.GlobalOptions.SelectedCOMPort;
			if (!this.configManager.LoggerSpecifics.DeviceAccess.HasRealtimeClockAccessBySerialPort && this.device == null)
			{
				InformMessageBox.Error(Resources.ErrorNoLoggerDevConnected);
				return;
			}
			string text = string.Empty;
			if (this.device != null)
			{
				text = this.device.HardwareKey;
				this.hardwareFrontend.EnableDeviceWatcher(this.device, false);
			}
			this.setRealtimeClockDialog.ShowDialog();
			if (this.device != null && text == this.device.HardwareKey && Directory.Exists(text))
			{
				this.hardwareFrontend.EnableDeviceWatcher(this.device, true);
			}
			this.AppDataAccess.AppDataRoot.GlobalOptions.SelectedCOMPort = this.setRealtimeClockDialog.SelectedCOMPort;
		}

		private void SetVehicleName()
		{
			if (!this.configManager.LoggerSpecifics.DeviceAccess.IsSetVehicleNameSupported)
			{
				return;
			}
			using (SetVehicleName setVehicleName = new SetVehicleName(this.device))
			{
				string hardwareKey = this.device.HardwareKey;
				this.hardwareFrontend.EnableDeviceWatcher(this.device, false);
				setVehicleName.ShowDialog();
				if (this.device != null && hardwareKey == this.device.HardwareKey && Directory.Exists(hardwareKey))
				{
					this.hardwareFrontend.EnableDeviceWatcher(this.device, true);
				}
			}
		}

		private void SetEthernetAddress()
		{
			if (this.configManager.LoggerSpecifics.Recording.HasEthernet)
			{
				this.setEthernetAddressDialog.SelectedCOMPort = this.AppDataAccess.AppDataRoot.GlobalOptions.SelectedCOMPort;
				this.setEthernetAddressDialog.ShowDialog();
				this.AppDataAccess.AppDataRoot.GlobalOptions.SelectedCOMPort = this.setEthernetAddressDialog.SelectedCOMPort;
			}
		}

		private void GenerateAdditionalDatabases(bool bAbortOnError = true)
		{
			if (bAbortOnError && !this.CheckAnyPageForErrorsAndDenyAction())
			{
				return;
			}
			using (AnalysisFileCollector analysisFileCollector = AnalysisFileCollector.Create())
			{
				analysisFileCollector.SoundnessCheckPerformed = bAbortOnError;
				Result result = Result.OK;
				List<string> list = new List<string>();
				Result result2 = this.GenerateCompilerIndependentFilesForAnalysis(analysisFileCollector, list, bAbortOnError);
				if (result2 == Result.Error)
				{
					result = Result.Error;
				}
				if (this.configManager.LoggerSpecifics.Configuration.CodeLanguage == EnumCodeLanguage.LTL && (result == Result.OK || !bAbortOnError))
				{
					string generalPurposeTempDirectory = GenerationUtil.AnalysisFileCollector.GeneralPurposeTempDirectory;
					string item;
					result2 = GenerationUtil.GenerateDBCFilesForAnalogInputsAndDateTime(generalPurposeTempDirectory, out item);
					if (result2 == Result.Error)
					{
						list.Add(item);
						result = Result.Error;
					}
				}
				if (result == Result.OK)
				{
					string text;
					Result result3 = this.ProcessAnalysisFilesAfterWriteAction(analysisFileCollector, null, null, DateTime.Now, out text);
					if (result3 == Result.OK && FileProxy.Exists(text))
					{
						this.ShowResultOfWriteAction(null, text, false, null);
					}
					else
					{
						if (result3 == Result.OK && !FileProxy.Exists(text))
						{
							list.Add(Resources.FileGeneratedForAnalysisNotWritten);
						}
						else if (Result.Error == result3)
						{
							list.Add(string.Format(Resources.ErrorFailedToCreateAnalysisPackage, text));
						}
						result = Result.Error;
					}
				}
				if (result == Result.Error && list.Count > 0)
				{
					MainWindow.ShowErrorsOfWriteAction(result, list);
				}
			}
		}

		private void GenerateVSysVarFileFromIni()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			if (!string.IsNullOrEmpty(this.vlProject.FilePath) && Directory.Exists(this.vlProject.FilePath))
			{
				openFileDialog.InitialDirectory = this.vlProject.FilePath;
			}
			openFileDialog.Filter = Resources_Files.FileFilterMlrt2INI;
			openFileDialog.FilterIndex = 2;
			openFileDialog.RestoreDirectory = true;
			openFileDialog.Multiselect = false;
			if (openFileDialog.ShowDialog() == DialogResult.OK)
			{
				GenerationUtil.GenerateVSysVarFileFromIniFile(null, openFileDialog.FileName);
			}
		}

		private Result ProcessAnalysisFilesAfterWriteAction(AnalysisFileCollector analysisFileCollector, string analysisFilesFolderPreferred, string analysePackageBaseNamePreferred, DateTime dt, out string analysisFilesPathFinal)
		{
			analysisFilesPathFinal = string.Empty;
			if (string.IsNullOrEmpty(this.vlProject.GetProjectFileName()) && string.IsNullOrEmpty(analysisFilesFolderPreferred) && !this.SaveProject())
			{
				return Result.UserAbort;
			}
			string text;
			if (!string.IsNullOrEmpty(analysisFilesFolderPreferred) && !string.IsNullOrEmpty(analysePackageBaseNamePreferred))
			{
				analysisFilesPathFinal = analysisFilesFolderPreferred + Path.DirectorySeparatorChar + Vocabulary.FolderNameAnalysisPackage;
				text = analysePackageBaseNamePreferred;
			}
			else
			{
				if (string.IsNullOrEmpty(this.vlProject.GetProjectFolder()) || string.IsNullOrEmpty(this.vlProject.GetProjectFileName()))
				{
					return Result.Error;
				}
				analysisFilesPathFinal = this.vlProject.GetProjectFolder() + Path.DirectorySeparatorChar + Vocabulary.FolderNameAnalysisPackage;
				text = Path.GetFileNameWithoutExtension(this.vlProject.GetProjectFileName());
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(analysisFilesPathFinal);
			if (!Directory.Exists(stringBuilder.ToString()) && !FileSystemServices.TryCreateDirectory(stringBuilder.ToString()))
			{
				return Result.Error;
			}
			stringBuilder.Append(Path.DirectorySeparatorChar);
			stringBuilder.Append(text);
			stringBuilder.Append("_");
			stringBuilder.AppendFormat(Resources.FormatDateTimeForFilenames, new object[]
			{
				dt.Year,
				dt.Month,
				dt.Day,
				dt.Hour,
				dt.Minute,
				dt.Second
			});
			stringBuilder.Append(".");
			stringBuilder.Append("analysis.zip");
			analysisFilesPathFinal = stringBuilder.ToString();
			return Vector.VLConfig.BusinessLogic.Conversion.AnalysisPackage.Create(analysisFilesPathFinal, text, this.vlProject, analysisFileCollector.GeneratedFiles, dt);
		}

		private static bool ShowErrorsOfWriteAction(Result result, IEnumerable<string> errorTextList)
		{
			if (result != Result.OK)
			{
				if (result == Result.Error)
				{
					StringBuilder stringBuilder = new StringBuilder();
					foreach (string current in errorTextList)
					{
						stringBuilder.AppendLine(current);
						stringBuilder.AppendLine();
					}
					InformMessageBox.Error(stringBuilder.ToString());
				}
				return true;
			}
			return false;
		}

		private void ShowResultOfWriteAction(string successMsg, string analysisPath = null, bool analysisPackageWrittenToMemoryCard = false, string ejectOptionMsg = null)
		{
			List<string> analysisFiles = string.IsNullOrEmpty(analysisPath) ? null : DirectoryProxy.GetFiles(analysisPath).ToList<string>();
			if (string.IsNullOrEmpty(ejectOptionMsg))
			{
				WrittenToDialog.Display(successMsg, analysisPath, analysisFiles, analysisPackageWrittenToMemoryCard);
				return;
			}
			WrittenToDialog.DisplayWithEjectOption(successMsg, ejectOptionMsg, ref this.isAutoEjectEnabled, analysisPath, analysisFiles, analysisPackageWrittenToMemoryCard);
		}

		public static void ShowHelpForDialog(Form form)
		{
			MainWindow.ShowHelpForDialog(GUIUtil.GetHelpIdForFormType(form));
		}

		public static void ShowHelpForDialog(int id)
		{
			if (id == 0)
			{
				Help.ShowHelp(MainWindow._dummyHelpParent, Resources.HelpFileName, HelpNavigator.TableOfContents);
				return;
			}
			Help.ShowHelp(MainWindow._dummyHelpParent, Resources.HelpFileName, HelpNavigator.TopicId, id.ToString());
		}

		private void ShowHelp()
		{
			int helpIdForPropertyPage = GUIUtil.GetHelpIdForPropertyPage(this.activePropertyWindow.Type);
			if (helpIdForPropertyPage == 0)
			{
				Help.ShowHelp(MainWindow._dummyHelpParent, Resources.HelpFileName);
				return;
			}
			Help.ShowHelp(MainWindow._dummyHelpParent, Resources.HelpFileName, HelpNavigator.TopicId, helpIdForPropertyPage.ToString());
		}

		void IUpdateService.AddUpdateObserver(IUpdateObserver observer, UpdateContext updateContext)
		{
			if (this.updateObserver2UpdateContext == null)
			{
				this.updateObserver2UpdateContext = new Dictionary<IUpdateObserver, UpdateContext>();
			}
			if (!this.updateObserver2UpdateContext.ContainsKey(observer))
			{
				this.updateObserver2UpdateContext.Add(observer, updateContext);
			}
		}

		void IUpdateService.RemoveUpdateObserver(IUpdateObserver observer)
		{
			if (this.updateObserver2UpdateContext != null && this.updateObserver2UpdateContext.ContainsKey(observer))
			{
				this.updateObserver2UpdateContext.Remove(observer);
			}
		}

		void IUpdateService.Notify<T>(T entity, IUpdateObserver observer)
		{
			if (observer == null)
			{
				((IUpdateServiceForFeature)this).Notify<T>(entity);
				return;
			}
			if (this.updateObserver2UpdateContext.ContainsKey(observer))
			{
				IUpdateObserver<T> updateObserver = observer as IUpdateObserver<T>;
				if (updateObserver != null)
				{
					updateObserver.Update(entity);
				}
			}
		}

		void IUpdateServiceForFeature.Notify<T>(T entity)
		{
			this.NotifyInternal<T>(entity);
		}

		void IUpdateService.NotifyDisplayMode(DisplayMode mode)
		{
			this.NotifyInternal<DisplayMode>(mode);
		}

		void IUpdateService.NotifyLoggerType(LoggerType type)
		{
			this.NotifyInternal<LoggerType>(type);
		}

		private void NotifyInternal<T>(T entity)
		{
			if (this.updateObserver2UpdateContext == null || this.updateObserver2UpdateContext.Count == 0)
			{
				return;
			}
			using (new WaitCursor())
			{
				foreach (UpdateContext current in MainWindow.sUpdateContextOrder)
				{
					foreach (KeyValuePair<IUpdateObserver, UpdateContext> current2 in this.updateObserver2UpdateContext)
					{
						if (current2.Value == current)
						{
							IUpdateObserver<T> updateObserver = current2.Key as IUpdateObserver<T>;
							if (updateObserver != null)
							{
								updateObserver.Update(entity);
							}
						}
					}
				}
			}
		}

		private void InitNewFile(ILoggerSpecifics loggerSpecs)
		{
			if (this.vlProject != null && this.vlProject.ProjectRoot != null && this.vlProject.ProjectRoot.LoggingConfiguration != null && this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration != null)
			{
				this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.ClearDatabases();
			}
			this.configManager.InitializeDefaultConfiguration(loggerSpecs);
			this.saveProjectToolStripMenuItem.Enabled = true;
			this.toolStripButtonSaveProject.Enabled = true;
			this.UpdateApplicationTitleBarText();
			this.UpdateLoggerTypeStatusBar();
			bool checkForMissingDiagDescriptions = true;
			this.ConfigurationLoaded(checkForMissingDiagDescriptions);
			this.hardwareFrontend.LoggerTypeToScan = loggerSpecs.Type;
			this.hardwareFrontend.UpdateLoggerDeviceList();
			this.UpdateMenusAndToolbars();
		}

		private void SetProjectDirty()
		{
			if (!this.isSetProjectDirtyEnabled)
			{
				return;
			}
			if (!this.vlProject.IsDirty)
			{
				this.Text = string.Format(Resources.DirtyFileName, this.Text);
			}
			this.vlProject.IsDirty = true;
			this.saveProjectToolStripMenuItem.Enabled = true;
			this.toolStripButtonSaveProject.Enabled = true;
		}

		private bool CheckActivePageForFormatErrorsAndAskForDiscard()
		{
			if (this.activePropertyWindow != null && !this.activePropertyWindow.ValidateInput() && this.activePropertyWindow.HasFormatErrors() && InformMessageBox.Question(Resources.AskForDiscardingFormatErrors) == DialogResult.No)
			{
				this.treeControl.Focus();
				return false;
			}
			return true;
		}

		private bool CheckAnyPageForErrorsAndDenyAction()
		{
			if (!this.activePropertyWindow.ValidateInput() && this.activePropertyWindow.HasErrors())
			{
				InformMessageBox.Warning(Resources.CorrectInputBeforeExecutingCommand);
				return false;
			}
			foreach (IPropertyWindow current in this.propertyWindowList)
			{
				if (!current.ValidateInput() && current.HasErrors())
				{
					InformMessageBox.Warning(Resources.CorrectInputBeforeExecutingCommand);
					this.treeControl.SelectPropertyPage(current.Type);
					this.treeControl.Focus();
					this.activePropertyWindow = current;
					return false;
				}
			}
			IList<Feature> list;
			if (UsedIdsManager.AreFiltersRemovingLogOnlyMsgs(this.vlProject.ProjectRoot.LoggingConfiguration.FilterConfigurations, this.vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations, this.applicationDatabaseManager, this.vlProject.GetProjectFolder(), this.configManager.Service.LoggerSpecifics, out list))
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(Resources.PagesWithCANIdsFilteredOut);
				foreach (Feature current2 in list)
				{
					PageType pageTypeForFeature = this.GetPageTypeForFeature(current2);
					stringBuilder.AppendLine(this.treeControl.GetPageNameForPageType(pageTypeForFeature));
				}
				stringBuilder.AppendLine(Resources.QuestionContinueAnyway);
				if (InformMessageBox.Question(stringBuilder.ToString()) == DialogResult.No)
				{
					return false;
				}
			}
			string message;
			return this.configManager.SemanticChecker.IsConfigurationSound(this.mAppDataAccess.AppDataRoot.GlobalOptions, out message) || InformMessageBox.Question(message) != DialogResult.No;
		}

		private PageType GetPageTypeForFeature(Feature feature)
		{
			foreach (IPropertyWindow current in this.propertyWindowList)
			{
				if (current.IsDisplayingFeature(feature))
				{
					return current.Type;
				}
			}
			return PageType._None_;
		}

		private bool ConfigurationLoaded(bool checkForMissingDiagDescriptions)
		{
			this.applicationDatabaseManager.DatabaseFileChanged -= new EventHandler(this.OnDatabaseFileChanged);
			bool result = this.configManager.UpdateDatabaseManagers(checkForMissingDiagDescriptions);
			this.applicationDatabaseManager.DatabaseFileChanged += new EventHandler(this.OnDatabaseFileChanged);
			using (new WaitCursor())
			{
				UsedIdsManager.Reset();
				UsedLimitFilterIdsCount.Reset();
				RingBufferMemoriesManager.Update(this.vlProject.ProjectRoot.LoggingConfiguration, this.configManager.LoggerSpecifics);
				this.NotifyConfigurationLoaded();
				this.PropagateConfigurationFolderPath(this.vlProject.FilePath);
			}
			this.SelectFirstInvalidPage();
			return result;
		}

		private bool CheckForAndReplaceMissingDiagDescriptions()
		{
			bool flag = false;
			foreach (DiagnosticsDatabase current in this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticsDatabaseConfiguration.Databases)
			{
				string absolutePath = FileSystemServices.GetAbsolutePath(current.FilePath.Value, this.vlProject.GetProjectFolder());
				if (!File.Exists(absolutePath))
				{
					string fileName = Path.GetFileName(current.FilePath.Value);
					if (InformMessageBox.Question(string.Format(Resources.DiagDescFileNotFoundReplace, fileName, FileSystemServices.GetFolderPathFromFilePath(absolutePath))) == DialogResult.Yes)
					{
						GenericOpenFileDialog.InitialDirectory = this.vlProject.GetProjectFolder();
						GenericOpenFileDialog.FileName = fileName;
						if (GenericOpenFileDialog.ShowDialog(GUIUtil.GetFileTypeFor(current.Type.Value)) == DialogResult.OK)
						{
							string value = current.FilePath.Value;
							string fileName2 = GenericOpenFileDialog.FileName;
							if (FileSystemServices.TryMakeFilePathRelativeToConfiguration(this.vlProject.GetProjectFolder(), ref fileName2))
							{
								current.FilePath.Value = fileName2;
							}
							else
							{
								current.FilePath.Value = GenericOpenFileDialog.FileName;
							}
							foreach (DiagnosticAction current2 in this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticActionsConfiguration.DiagnosticActions)
							{
								if (current2.DatabasePath.Value == value)
								{
									current2.DatabasePath.Value = current.FilePath.Value;
								}
							}
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				this.SetProjectDirty();
			}
			return flag;
		}

		private void NotifyConfigurationLoaded()
		{
			this.NotifyConfigurationLoadedToTarget(null);
		}

		private void NotifyConfigurationLoadedToTarget(IUpdateObserver observer)
		{
			this.isSetProjectDirtyEnabled = false;
			this.ResetPropertyWindows();
			((IUpdateService)this).NotifyLoggerType(this.vlProject.ProjectRoot.LoggerType);
			((IUpdateService)this).Notify<InterfaceModeConfiguration>(this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration, observer);
			((IUpdateService)this).Notify<CANChannelConfiguration>(this.vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration, observer);
			((IUpdateService)this).Notify<LINChannelConfiguration>(this.vlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration, observer);
			((IUpdateServiceForFeature)this).Notify<MultibusChannelConfiguration>(this.vlProject.ProjectRoot.HardwareConfiguration.MultibusChannelConfiguration);
			((IUpdateService)this).Notify<FlexrayChannelConfiguration>(this.vlProject.ProjectRoot.HardwareConfiguration.FlexrayChannelConfiguration, observer);
			((IUpdateService)this).Notify<MOST150ChannelConfiguration>(this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration, observer);
			((IUpdateService)this).Notify<DatabaseConfiguration>(this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration, observer);
			((IUpdateService)this).Notify<CcpXcpSignalConfiguration>(this.vlProject.ProjectRoot.LoggingConfiguration.CcpXcpSignalConfiguration, observer);
			((IUpdateService)this).Notify<LogDataStorage>(this.vlProject.ProjectRoot.HardwareConfiguration.LogDataStorage, observer);
			((IUpdateService)this).Notify<DiagnosticsDatabaseConfiguration>(this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticsDatabaseConfiguration, observer);
			foreach (TriggerConfiguration current in this.vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurations)
			{
				((IUpdateService)this).Notify<TriggerConfiguration>(current, observer);
			}
			((IUpdateService)this).Notify<AnalogInputConfiguration>(this.vlProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration, observer);
			((IUpdateService)this).Notify<DigitalInputConfiguration>(this.vlProject.ProjectRoot.HardwareConfiguration.DigitalInputConfiguration, observer);
			((IUpdateService)this).Notify<IncludeFileConfiguration>(this.vlProject.ProjectRoot.LoggingConfiguration.IncludeFileConfiguration, observer);
			((IUpdateService)this).Notify<SpecialFeaturesConfiguration>(this.vlProject.ProjectRoot.LoggingConfiguration.SpecialFeaturesConfiguration, observer);
			((IUpdateService)this).Notify<WLANConfiguration>(this.vlProject.ProjectRoot.LoggingConfiguration.WLANConfiguration, observer);
			((IUpdateService)this).Notify<EthernetConfiguration>(this.vlProject.ProjectRoot.HardwareConfiguration.EthernetConfiguration, observer);
			((IUpdateService)this).Notify<CommentConfiguration>(this.vlProject.ProjectRoot.LoggingConfiguration.CommentConfiguration, observer);
			((IUpdateService)this).Notify<DiagnosticActionsConfiguration>(this.vlProject.ProjectRoot.LoggingConfiguration.DiagnosticActionsConfiguration, observer);
			foreach (FilterConfiguration current2 in this.vlProject.ProjectRoot.LoggingConfiguration.FilterConfigurations)
			{
				((IUpdateService)this).Notify<FilterConfiguration>(current2, observer);
			}
			((IUpdateService)this).Notify<LEDConfiguration>(this.vlProject.ProjectRoot.OutputConfiguration.LEDConfiguration, observer);
			((IUpdateService)this).Notify<DigitalOutputsConfiguration>(this.vlProject.ProjectRoot.OutputConfiguration.DigitalOutputsConfiguration, observer);
			((IUpdateService)this).Notify<SendMessageConfiguration>(this.vlProject.ProjectRoot.OutputConfiguration.SendMessageConfiguration, observer);
			((IUpdateService)this).Notify<GPSConfiguration>(this.vlProject.ProjectRoot.HardwareConfiguration.GPSConfiguration, observer);
			((IUpdateService)this).Notify<MetaInformation>(this.vlProject.ProjectRoot.MetaInformation, observer);
			this.isSetProjectDirtyEnabled = true;
			this.treeControl.RefreshView();
		}

		private void UpdateApplicationTitleBarText()
		{
			StringBuilder stringBuilder = new StringBuilder(Vocabulary.VLConfigApplicationTitle, 200);
			if (this.vlProject.FilePath.Length > 0)
			{
				stringBuilder.Append(" - ");
				int num = this.vlProject.FilePath.LastIndexOf(Path.DirectorySeparatorChar);
				if (num == -1)
				{
					stringBuilder.Append(this.vlProject.FilePath);
				}
				else
				{
					stringBuilder.Append(this.vlProject.FilePath.Substring(num + 1, this.vlProject.FilePath.Length - num - 1));
				}
			}
			this.Text = stringBuilder.ToString();
			if (this.vlProject.IsDirty)
			{
				this.Text = string.Format(Resources.DirtyFileName, this.Text);
			}
		}

		private void UpdateLoggerTypeStatusBar()
		{
			this.toolStripStatusLabelSelectedLoggerType.Text = Resources.LoggerTypeCaption + GUIUtil.MapLoggerType2String(this.configManager.LoggerSpecifics.Type);
		}

		private void PropagateConfigurationFolderPath(string configFilePath)
		{
			string folderPathFromFilePath = FileSystemServices.GetFolderPathFromFilePath(configFilePath);
			this.cardReader.ConfigurationFolderPath = folderPathFromFilePath;
			this.cardReaderNavigator.ConfigurationFolderPath = folderPathFromFilePath;
			this.clfExport.ConfigurationFolderPath = folderPathFromFilePath;
			this.loggerDevice.ConfigurationFolderPath = folderPathFromFilePath;
			this.loggerDeviceNavigator.ConfigurationFolderPath = folderPathFromFilePath;
			this.ccpXcpDescriptions.ConfigurationFolderPath = folderPathFromFilePath;
			this.ccpXcpSignalRequests.ConfigurationFolderPath = configFilePath;
			this.interfaceMode.ConfigurationFolderPath = configFilePath;
			GenericOpenFileDialog.ProjectFolder = folderPathFromFilePath;
		}

		private bool SelectFirstInvalidPage()
		{
			foreach (IPropertyWindow current in this.propertyWindowList)
			{
				if (!current.ValidateInput() && (current.HasFormatErrors() || current.HasLocalErrors()))
				{
					this.treeControl.SelectPropertyPage(current.Type);
					this.treeControl.Focus();
					this.activePropertyWindow = current;
					return true;
				}
			}
			return false;
		}

		private bool IsMemoryCardAccessible(string driveName)
		{
			if (string.IsNullOrEmpty(driveName))
			{
				return false;
			}
			if (this.configManager.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
			{
				DriveInfo driveInfo = new DriveInfo(driveName);
				try
				{
					if (!driveInfo.IsReady || (driveInfo.DriveFormat != Constants.FileSystemFormatFAT && driveInfo.DriveFormat != Constants.FileSystemFormatFAT32))
					{
						bool result = false;
						return result;
					}
				}
				catch (Exception)
				{
					bool result = false;
					return result;
				}
				return true;
			}
			ILoggerDevice loggerDeviceByDriveName = this.hardwareFrontend.GetLoggerDeviceByDriveName(driveName);
			return loggerDeviceByDriveName != null && loggerDeviceByDriveName.Update();
		}

		private void OnMainWindowLoad(object sender, EventArgs e)
		{
			if (this.mAppDataAccess.AppDataRoot.GlobalOptions.MainWindowTop >= 0 && this.mAppDataAccess.AppDataRoot.GlobalOptions.MainWindowLeft >= 0 && this.mAppDataAccess.AppDataRoot.GlobalOptions.MainWindowHeight > 0 && this.mAppDataAccess.AppDataRoot.GlobalOptions.MainWindowWidth > 0)
			{
				base.Top = this.mAppDataAccess.AppDataRoot.GlobalOptions.MainWindowTop;
				base.Left = this.mAppDataAccess.AppDataRoot.GlobalOptions.MainWindowLeft;
				base.Height = this.mAppDataAccess.AppDataRoot.GlobalOptions.MainWindowHeight;
				base.Width = this.mAppDataAccess.AppDataRoot.GlobalOptions.MainWindowWidth;
				int top = base.Top;
				int left = base.Left;
				int height = base.Height;
				int width = base.Width;
				if (GUIUtil.AdjustScreenPositionToVisibleArea(ref top, ref left, ref height, ref width))
				{
					base.Top = top;
					base.Left = left;
					base.Height = height;
					base.Width = width;
				}
			}
			if (this.applicationDatabaseManager != null)
			{
				this.applicationDatabaseManager.IsHexDisplay = this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode.IsHexadecimal;
			}
			GUIUtil.IsHexadecimal = this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode.IsHexadecimal;
			((IUpdateService)this).NotifyDisplayMode(this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode);
			this.lruFileHandler.LoadSettings2ToolStripItem();
			if (!string.IsNullOrEmpty(this.filePathFromCommandLine))
			{
				bool checkForMissingDiagDescriptions = true;
				this.LoadFile(this.filePathFromCommandLine, checkForMissingDiagDescriptions);
			}
		}

		private void OnMainWindowShown(object sender, EventArgs e)
		{
			if (GlobalRuntimeSettings.GetInstance().ActiveApplicationMode == GlobalRuntimeSettings.ApplicationMode.BatchMode)
			{
				this.treeControl.SelectPropertyPage(PageType.CardReaderNavigator);
			}
			this.hardwareFrontend.EnableClientNotification(true);
		}

		private void OnMainWindowFormClosing(object sender, FormClosingEventArgs e)
		{
			this.hardwareFrontend.Dispose();
			if (e.CloseReason == CloseReason.UserClosing)
			{
				if (this.Exit())
				{
					base.Dispose();
					return;
				}
				e.Cancel = true;
			}
		}

		private void MainWindow_DragEnter(object sender, DragEventArgs e)
		{
			if (this.AcceptFileDrop(e))
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void MainWindow_DragDrop(object sender, DragEventArgs e)
		{
			if (this.AcceptFileDrop(e))
			{
				string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
				this.OpenFile(array[0]);
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private bool AcceptFileDrop(DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (array.Count<string>() != 1)
				{
					return false;
				}
				string strA = "";
				try
				{
					strA = Path.GetExtension(array[0]);
				}
				catch
				{
					bool result = false;
					return result;
				}
				if (string.Compare(strA, Vocabulary.FileExtensionDotGLC, true) == 0 || string.Compare(strA, Vocabulary.FileExtensionDotVLC, true) == 0)
				{
					return true;
				}
				return false;
			}
			return false;
		}

		private void RefreshPropertyView()
		{
			if (this.activePropertyWindow.Type == PageType.CardReader || this.activePropertyWindow.Type == PageType.CardReaderNavigator || this.activePropertyWindow.Type == PageType.LoggerDevice || this.activePropertyWindow.Type == PageType.LoggerDeviceNavigator || this.activePropertyWindow.Type == PageType.DeviceInformation || this.activePropertyWindow.Type == PageType.CLFExport || this.activePropertyWindow.Type == PageType.IncludeFiles)
			{
				this.hardwareFrontend.UpdateLoggerDeviceList();
				if (this.activePropertyWindow.Type == PageType.CardReader)
				{
					this.cardReader.RefreshView();
					return;
				}
				if (this.activePropertyWindow.Type == PageType.CardReaderNavigator)
				{
					this.cardReaderNavigator.RefreshView();
					return;
				}
				if (this.activePropertyWindow.Type == PageType.LoggerDevice)
				{
					this.loggerDevice.RefreshView();
					return;
				}
				if (this.activePropertyWindow.Type == PageType.LoggerDeviceNavigator)
				{
					this.loggerDeviceNavigator.RefreshView();
					return;
				}
				if (this.activePropertyWindow.Type == PageType.CLFExport)
				{
					this.clfExport.RefreshView();
					return;
				}
				if (this.activePropertyWindow.Type == PageType.IncludeFiles)
				{
					this.includeFiles.RefreshView();
					return;
				}
			}
			else
			{
				this.ValidateAllPropertyPages();
			}
		}

		private void ValidateAllPropertyPages()
		{
			using (new WaitCursor())
			{
				foreach (IPropertyWindow current in this.propertyWindowList)
				{
					current.ValidateInput();
				}
				this.treeControl.RefreshView();
			}
		}

		private void UpdateMenusAndToolbars()
		{
			using (new WaitCursor())
			{
				bool flag = this.hardwareFrontend.CurrentOnlineLoggerDevices.Count > 0;
				this.device = (flag ? this.hardwareFrontend.CurrentOnlineLoggerDevices.First<ILoggerDevice>() : null);
				bool flag2 = this.device != null && this.device.IsMemoryCardReady;
				ILoggerSpecifics loggerSpecifics = this.configManager.LoggerSpecifics;
				this.toolStripButtonNewProject.Enabled = true;
				this.toolStripButtonOpenProject.Enabled = true;
				this.toolStripButtonSaveProject.Enabled = true;
				this.toolStripButtonSaveProjectAs.Enabled = true;
				this.toolStripButtonWriteToDevice.Enabled = (flag && flag2);
				this.toolStripButtonWriteToCard.Enabled = true;
				this.toolStripButtonExportToLTL.Enabled = (loggerSpecifics.Configuration.CodeLanguage == EnumCodeLanguage.LTL);
				this.toolStripButtonSetRealtimeClock.Enabled = (flag ? flag2 : loggerSpecifics.DeviceAccess.HasRealtimeClockAccessBySerialPort);
				this.toolStripButtonRefresh.Enabled = true;
				this.toolStripButtonHexMode.Enabled = true;
				this.toolStripButtonAbout.Enabled = true;
				this.toolStripButtonHelp.Enabled = true;
				this.writeToDeviceToolStripMenuItem.Enabled = (flag && flag2);
				this.writeToMemoryCardToolStripMenuItem.Enabled = true;
				this.writeCODFileToDeviceToolStripMenuItem.Enabled = (flag && flag2 && loggerSpecifics.Configuration.CodeLanguage == EnumCodeLanguage.LTL);
				this.writeCODFileToMemoryCardToolStripMenuItem.Enabled = (loggerSpecifics.Configuration.CodeLanguage == EnumCodeLanguage.LTL);
				this.exportToLTLFileToolStripMenuItem.Enabled = (loggerSpecifics.Configuration.CodeLanguage == EnumCodeLanguage.LTL);
				this.exportToCODFileToolStripMenuItem.Enabled = (loggerSpecifics.Configuration.CodeLanguage == EnumCodeLanguage.LTL);
				this.createDatabaseForToolStripMenuItem.Enabled = true;
				this.setRealTimeClockToolStripMenuItem.Enabled = (flag ? flag2 : loggerSpecifics.DeviceAccess.HasRealtimeClockAccessBySerialPort);
				this.setVehicleNameToolStripMenuItem.Enabled = (flag && loggerSpecifics.DeviceAccess.IsSetVehicleNameSupported);
				this.setEthernetAddressToolStripMenuItem.Enabled = loggerSpecifics.Recording.HasEthernet;
				this.updateLoggerFirmwareToolStripMenuItem.Enabled = (flag && loggerSpecifics.DeviceAccess.IsUpdateFirmwareSupported);
				this.writeLicenseToDeviceToolStripMenuItem.Enabled = (flag && flag2 && loggerSpecifics.DeviceAccess.IsWriteLicenseSupported);
				this.writeLicenseToMemoryCardToolStripMenuItem.Enabled = (!loggerSpecifics.DeviceAccess.IsUsingSecWrite && loggerSpecifics.DeviceAccess.IsWriteLicenseSupported);
				this.clearDeviceToolStripMenuItem.Enabled = (flag && flag2);
				this.clearMemoryCardToolStripMenuItem.Enabled = true;
				this.formatMemoryCardInDeviceToolStripMenuItem.Enabled = (flag && flag2 && loggerSpecifics.DeviceAccess.IsMemoryCardFormattingSupported);
				this.formatMemoryCardToolStripMenuItem.Enabled = loggerSpecifics.DeviceAccess.IsMemoryCardFormattingSupported;
				this.importProjectFromDeviceToolStripMenuItem.Enabled = (flag && flag2 && loggerSpecifics.Configuration.SupportsPackAndGoImport);
				this.importProjectFromMemoryCardToolStripMenuItem.Enabled = loggerSpecifics.Configuration.SupportsPackAndGoImport;
				this.packLoggingDataFromDeviceToolStripMenuItem.Enabled = (flag && flag2 && loggerSpecifics.DataStorage.HasPackableLogData);
				this.packLoggingDataFromMemoryCardToolStripMenuItem.Enabled = loggerSpecifics.DataStorage.HasPackableLogData;
				this.packLoggingDataFromDeviceToolStripButton.Enabled = (flag && flag2 && loggerSpecifics.DataStorage.HasPackableLogData);
				this.packLoggingDataFromCardReaderToolStripButton.Enabled = loggerSpecifics.DataStorage.HasPackableLogData;
				this.analysisPackageImportFromDeviceToolStripMenuItem.Enabled = (flag && flag2 && loggerSpecifics.Configuration.SupportsAnalysisPackage);
				this.analysisPackageImportFromMemoryCardToolStripMenuItem.Enabled = loggerSpecifics.Configuration.SupportsAnalysisPackage;
				this.toolStripStatusLabelOnOffline.Text = string.Format(Resources.ConnectionStatus, flag ? Resources.StatusOnline : Resources.StatusOffline);
				this.createVSysVarFromIniToolStripMenuItem.Enabled = (loggerSpecifics.Type != LoggerType.VN1630log);
				this.clfExport.RefreshView();
				this.comment.RefreshView();
			}
			Application.DoEvents();
		}

		private void ToggleHexDecDisplay()
		{
			this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode.IsHexadecimal = !this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode.IsHexadecimal;
			this.toolStripButtonHexMode.Checked = this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode.IsHexadecimal;
			this.hexadecimalDisplayToolStripMenuItem.Checked = this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode.IsHexadecimal;
			this.applicationDatabaseManager.IsHexDisplay = this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode.IsHexadecimal;
			GUIUtil.IsHexadecimal = this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode.IsHexadecimal;
			((IUpdateService)this).NotifyDisplayMode(this.mAppDataAccess.AppDataRoot.GlobalOptions.DisplayMode);
		}

		private void packLoggingDataFromDeviceToolStrip_Click(object sender, EventArgs e)
		{
			if (this.device != null && this.device.LoggerSpecifics.DataStorage.HasPackableLogData)
			{
				this.PackLoggingData(this.device);
			}
		}

		private void packLoggingDataFromMemoryCardToolStrip_Click(object sender, EventArgs e)
		{
			string text;
			if (!CardReaderDriveSelection.SelectCardReaderDrive(this.configManager.LoggerSpecifics, this.hardwareFrontend, this.hardwareFrontend.AdditionalDrives, out text, true, LoggerType.Unknown))
			{
				return;
			}
			if (!this.IsMemoryCardAccessible(text))
			{
				InformMessageBox.Error(Resources.ErrorUnableToAccessDriveWillUpdate);
				this.hardwareFrontend.UpdateLoggerDeviceList();
				return;
			}
			ILoggerDevice loggerDeviceByDriveName = this.hardwareFrontend.GetLoggerDeviceByDriveName(text);
			if (loggerDeviceByDriveName == null)
			{
				InformMessageBox.Error(string.Format(Resources.ErrorCannotAccessMemoryCardinDrive, text));
				return;
			}
			if (!loggerDeviceByDriveName.LoggerSpecifics.DataStorage.HasPackableLogData)
			{
				InformMessageBox.Error(string.Format(Resources.ErrorCannotAccessMemoryCardinDrive, text));
				return;
			}
			this.PackLoggingData(loggerDeviceByDriveName);
		}

		private void PackLoggingData(ILoggerDevice selectedDevice)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(string.Concat(new string[]
			{
				selectedDevice.LoggerSpecifics.Name,
				"_",
				selectedDevice.VehicleName,
				"_",
				selectedDevice.CompileDateTime.Date.ToString("yyyy-MM-dd")
			}));
			if (DialogResult.OK == GenericSaveFileDialog.ShowDialog(FileType.ZIPFile, fileNameWithoutExtension, this.configManager.LoggerSpecifics.Name, this.vlProject.GetProjectFolder()))
			{
				ILoggerDevice loggerDevice = null;
				string fileName = GenericSaveFileDialog.FileName;
				bool flag = false;
				string text = string.Concat(new string[]
				{
					Path.GetDirectoryName(fileName),
					"\\",
					Path.GetFileNameWithoutExtension(fileName),
					"_snapshot",
					Path.GetExtension(fileName)
				});
				List<string> list = (from file in selectedDevice.LogFileStorage.LogFiles
				select file.FullFilePath).ToList<string>();
				if (selectedDevice.HasSnapshotFolderContainingLogData)
				{
					loggerDevice = this.hardwareFrontend.CreateCustomLoggerDevice(selectedDevice.LoggerType, selectedDevice.SnapshotFolderPath, this);
					loggerDevice.LogFileStorage.UpdateFileList();
					flag = (DialogResult.Yes == InformMessageBox.Show(EnumQuestionType.Question, string.Format(Resources.QuestionPackSnapshotData, Path.GetFileName(text))));
				}
				if (!flag && loggerDevice != null)
				{
					list.AddRange(from file in loggerDevice.LogFileStorage.LogFiles
					select file.FullFilePath);
				}
				string analysisPackagePath = selectedDevice.GetAnalysisPackagePath();
				if (!string.IsNullOrEmpty(analysisPackagePath) && FileProxy.Exists(analysisPackagePath))
				{
					list.Add(analysisPackagePath + "*");
				}
				string path = Path.Combine(selectedDevice.HardwareKey, Vocabulary.ConfigureFolder);
				if (Directory.Exists(path))
				{
					list.AddRange(Directory.GetFiles(path, "*.*", SearchOption.AllDirectories));
				}
				using (ZipFilesWithProgressIndicator zipFilesWithProgressIndicator = new ZipFilesWithProgressIndicator())
				{
					bool flag2 = !zipFilesWithProgressIndicator.Zip(list, fileName);
					if (flag && !flag2)
					{
						flag2 &= !zipFilesWithProgressIndicator.Zip((from file in loggerDevice.LogFileStorage.LogFiles
						select file.FullFilePath).ToList<string>(), text);
					}
					if (flag2)
					{
						string text2;
						GenerationUtil.TryDeleteFile(fileName, out text2, false);
						GenerationUtil.TryDeleteFile(text, out text2, false);
					}
				}
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			TempDirectoryManager.Instance.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MainWindow));
			this.toolStripContainer = new ToolStripContainer();
			this.statusStrip = new StatusStrip();
			this.toolStripStatusLabelSelectedLoggerType = new ToolStripStatusLabel();
			this.toolStripStatusLabelOnOffline = new ToolStripStatusLabel();
			this.splitContainer1 = new SplitContainer();
			this.treeControl = new TreeControl();
			this.combinedAnalogDigitalInputs = new CombinedAnalogDigitalInputs();
			this.multibusChannels = new MultibusChannels();
			this.cardReaderNavigator = new CardReaderNavigator();
			this.loggerDeviceNavigator = new LoggerDeviceNavigator();
			this.sendMessage = new SendMessage();
			this.digitalOutputs = new DigitalOutputs();
			this.triggers2 = new Triggers();
			this.triggers1 = new Triggers();
			this.canChannels = new CANChannels();
			this.digitalInputs = new DigitalInputs();
			this.comment = new Comment();
			this.most150Channels = new MOST150Channels();
			this.interfaceMode = new InterfaceMode();
			this.diagnosticActions = new DiagnosticActions();
			this.diagnosticsDatabases = new DiagnosticsDatabases();
			this.wlanSettings = new WLANSettings();
			this.flexrayChannels = new FlexrayChannels();
			this.deviceInformation = new DeviceInformation();
			this.loggerDevice = new LoggerDevice();
			this.specialFeatures = new SpecialFeatures();
			this.includeFiles = new IncludeFiles();
			this.cardReader = new CardReader();
			this.clfExport = new CLFExport();
			this.filters2 = new Filters();
			this.filters1 = new Filters();
			this.analogInputs = new AnalogInputs();
			this.leds = new LEDs();
			this.linChannels = new LINChannels();
			this.hardwareSettings = new HardwareSettings();
			this.databases = new Databases();
			this.gps = new GPS();
			this.ccpXcpDescriptions = new CcpXcpDescriptions();
			this.ccpXcpSignalRequests = new CcpXcpSignalRequests();
			this.mainMenuStrip = new MenuStrip();
			this.fileToolStripMenuItem = new ToolStripMenuItem();
			this.newProjectToolStripMenuItem1 = new ToolStripMenuItem();
			this.openProjectToolStripMenuItem1 = new ToolStripMenuItem();
			this.toolStripSeparator4 = new ToolStripSeparator();
			this.saveProjectToolStripMenuItem = new ToolStripMenuItem();
			this.saveProjectAsToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator5 = new ToolStripSeparator();
			this.importProjectToolStripMenuItem = new ToolStripMenuItem();
			this.exportProjectToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator20 = new ToolStripSeparator();
			this.toolStripSeparator6 = new ToolStripSeparator();
			this.exitToolStripMenuItem1 = new ToolStripMenuItem();
			this.configurationToolStripMenuItem = new ToolStripMenuItem();
			this.writeToDeviceToolStripMenuItem = new ToolStripMenuItem();
			this.writeToMemoryCardToolStripMenuItem = new ToolStripMenuItem();
			this.writeCODFileToDeviceToolStripMenuItem = new ToolStripMenuItem();
			this.writeCODFileToMemoryCardToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator9 = new ToolStripSeparator();
			this.exportToLTLFileToolStripMenuItem = new ToolStripMenuItem();
			this.exportToCODFileToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator13 = new ToolStripSeparator();
			this.createDatabaseForToolStripMenuItem = new ToolStripMenuItem();
			this.deviceToolStripMenuItem = new ToolStripMenuItem();
			this.setRealTimeClockToolStripMenuItem = new ToolStripMenuItem();
			this.setVehicleNameToolStripMenuItem = new ToolStripMenuItem();
			this.setEthernetAddressToolStripMenuItem = new ToolStripMenuItem();
			this.updateLoggerFirmwareToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator15 = new ToolStripSeparator();
			this.writeLicenseToDeviceToolStripMenuItem = new ToolStripMenuItem();
			this.writeLicenseToMemoryCardToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator18 = new ToolStripSeparator();
			this.clearDeviceToolStripMenuItem = new ToolStripMenuItem();
			this.clearMemoryCardToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator19 = new ToolStripSeparator();
			this.formatMemoryCardInDeviceToolStripMenuItem = new ToolStripMenuItem();
			this.formatMemoryCardToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator21 = new ToolStripSeparator();
			this.importProjectFromDeviceToolStripMenuItem = new ToolStripMenuItem();
			this.importProjectFromMemoryCardToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator24 = new ToolStripSeparator();
			this.analysisPackageImportFromDeviceToolStripMenuItem = new ToolStripMenuItem();
			this.analysisPackageImportFromMemoryCardToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator22 = new ToolStripSeparator();
			this.packLoggingDataFromDeviceToolStripMenuItem = new ToolStripMenuItem();
			this.packLoggingDataFromMemoryCardToolStripMenuItem = new ToolStripMenuItem();
			this.windowToolStripMenuItem = new ToolStripMenuItem();
			this.refreshToolStripMenuItem = new ToolStripMenuItem();
			this.hexadecimalDisplayToolStripMenuItem = new ToolStripMenuItem();
			this.toolsToolStripMenuItem = new ToolStripMenuItem();
			this.optionsToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator10 = new ToolStripSeparator();
			this.linProbeConfiguratorToolStripMenuItem = new ToolStripMenuItem();
			this.canShuntConfiguratorToolStripMenuItem = new ToolStripMenuItem();
			this.canGpsConfiguratorToolStripMenuItem = new ToolStripMenuItem();
			this.mlSetupToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator17 = new ToolStripSeparator();
			this.createVSysVarFromIniToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator16 = new ToolStripSeparator();
			this.splitFileToolStripMenuItem = new ToolStripMenuItem();
			this.mergeFileToolStripMenuItem = new ToolStripMenuItem();
			this.helpToolStripMenuItem = new ToolStripMenuItem();
			this.contentsToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator11 = new ToolStripSeparator();
			this.aboutToolStripMenuItem = new ToolStripMenuItem();
			this.mainMenuToolStrip = new ToolStrip();
			this.toolStripButtonNewProject = new ToolStripButton();
			this.toolStripButtonOpenProject = new ToolStripButton();
			this.toolStripButtonSaveProject = new ToolStripButton();
			this.toolStripButtonSaveProjectAs = new ToolStripButton();
			this.toolStripSeparator7 = new ToolStripSeparator();
			this.toolStripButtonWriteToDevice = new ToolStripButton();
			this.toolStripButtonWriteToCard = new ToolStripButton();
			this.toolStripButtonExportToLTL = new ToolStripButton();
			this.toolStripSeparator23 = new ToolStripSeparator();
			this.packLoggingDataFromDeviceToolStripButton = new ToolStripButton();
			this.packLoggingDataFromCardReaderToolStripButton = new ToolStripButton();
			this.toolStripSeparator12 = new ToolStripSeparator();
			this.toolStripButtonSetRealtimeClock = new ToolStripButton();
			this.toolStripSeparator8 = new ToolStripSeparator();
			this.toolStripButtonRefresh = new ToolStripButton();
			this.toolStripButtonHexMode = new ToolStripButton();
			this.toolStripSeparator14 = new ToolStripSeparator();
			this.toolStripButtonAbout = new ToolStripButton();
			this.toolStripButtonHelp = new ToolStripButton();
			this.mToolStripFileConversionSettings = new ToolStrip();
			this.mToolStripDropDownLoadFileConversionSettings = new ToolStripDropDownButton();
			this.mToolStripButtonSaveFileConversionSettings = new ToolStripButton();
			this.newProjectToolStripMenuItem = new ToolStripMenuItem();
			this.openProjectToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator1 = new ToolStripSeparator();
			this.saveToolStripMenuItem = new ToolStripMenuItem();
			this.saveAsToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripSeparator3 = new ToolStripSeparator();
			this.toolStripSeparator2 = new ToolStripSeparator();
			this.exitToolStripMenuItem = new ToolStripMenuItem();
			this.toolStripStatusLabel1 = new ToolStripStatusLabel();
			this.toolStripButtonOpen = new ToolStripButton();
			this.toolStripButtonSave = new ToolStripButton();
			this.toolStripButtonSaveAs = new ToolStripButton();
			this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
			this.toolStripContainer.ContentPanel.SuspendLayout();
			this.toolStripContainer.TopToolStripPanel.SuspendLayout();
			this.toolStripContainer.SuspendLayout();
			this.statusStrip.SuspendLayout();
			((ISupportInitialize)this.splitContainer1).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.mainMenuStrip.SuspendLayout();
			this.mainMenuToolStrip.SuspendLayout();
			this.mToolStripFileConversionSettings.SuspendLayout();
			base.SuspendLayout();
			this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.statusStrip);
			this.toolStripContainer.ContentPanel.Controls.Add(this.splitContainer1);
			componentResourceManager.ApplyResources(this.toolStripContainer.ContentPanel, "toolStripContainer.ContentPanel");
			componentResourceManager.ApplyResources(this.toolStripContainer, "toolStripContainer");
			this.toolStripContainer.Name = "toolStripContainer";
			this.toolStripContainer.TopToolStripPanel.Controls.Add(this.mainMenuStrip);
			this.toolStripContainer.TopToolStripPanel.Controls.Add(this.mainMenuToolStrip);
			this.toolStripContainer.TopToolStripPanel.Controls.Add(this.mToolStripFileConversionSettings);
			componentResourceManager.ApplyResources(this.statusStrip, "statusStrip");
			this.statusStrip.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripStatusLabelSelectedLoggerType,
				this.toolStripStatusLabelOnOffline
			});
			this.statusStrip.Name = "statusStrip";
			this.toolStripStatusLabelSelectedLoggerType.BorderSides = ToolStripStatusLabelBorderSides.Right;
			this.toolStripStatusLabelSelectedLoggerType.Name = "toolStripStatusLabelSelectedLoggerType";
			componentResourceManager.ApplyResources(this.toolStripStatusLabelSelectedLoggerType, "toolStripStatusLabelSelectedLoggerType");
			this.toolStripStatusLabelOnOffline.Name = "toolStripStatusLabelOnOffline";
			componentResourceManager.ApplyResources(this.toolStripStatusLabelOnOffline, "toolStripStatusLabelOnOffline");
			componentResourceManager.ApplyResources(this.splitContainer1, "splitContainer1");
			this.splitContainer1.FixedPanel = FixedPanel.Panel1;
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Panel1.Controls.Add(this.treeControl);
			componentResourceManager.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
			this.splitContainer1.Panel2.Controls.Add(this.combinedAnalogDigitalInputs);
			this.splitContainer1.Panel2.Controls.Add(this.multibusChannels);
			this.splitContainer1.Panel2.Controls.Add(this.cardReaderNavigator);
			this.splitContainer1.Panel2.Controls.Add(this.loggerDeviceNavigator);
			this.splitContainer1.Panel2.Controls.Add(this.sendMessage);
			this.splitContainer1.Panel2.Controls.Add(this.digitalOutputs);
			this.splitContainer1.Panel2.Controls.Add(this.triggers2);
			this.splitContainer1.Panel2.Controls.Add(this.triggers1);
			this.splitContainer1.Panel2.Controls.Add(this.canChannels);
			this.splitContainer1.Panel2.Controls.Add(this.digitalInputs);
			this.splitContainer1.Panel2.Controls.Add(this.comment);
			this.splitContainer1.Panel2.Controls.Add(this.most150Channels);
			this.splitContainer1.Panel2.Controls.Add(this.interfaceMode);
			this.splitContainer1.Panel2.Controls.Add(this.diagnosticActions);
			this.splitContainer1.Panel2.Controls.Add(this.diagnosticsDatabases);
			this.splitContainer1.Panel2.Controls.Add(this.wlanSettings);
			this.splitContainer1.Panel2.Controls.Add(this.flexrayChannels);
			this.splitContainer1.Panel2.Controls.Add(this.deviceInformation);
			this.splitContainer1.Panel2.Controls.Add(this.loggerDevice);
			this.splitContainer1.Panel2.Controls.Add(this.specialFeatures);
			this.splitContainer1.Panel2.Controls.Add(this.includeFiles);
			this.splitContainer1.Panel2.Controls.Add(this.cardReader);
			this.splitContainer1.Panel2.Controls.Add(this.clfExport);
			this.splitContainer1.Panel2.Controls.Add(this.filters2);
			this.splitContainer1.Panel2.Controls.Add(this.filters1);
			this.splitContainer1.Panel2.Controls.Add(this.analogInputs);
			this.splitContainer1.Panel2.Controls.Add(this.leds);
			this.splitContainer1.Panel2.Controls.Add(this.linChannels);
			this.splitContainer1.Panel2.Controls.Add(this.hardwareSettings);
			this.splitContainer1.Panel2.Controls.Add(this.databases);
			this.splitContainer1.Panel2.Controls.Add(this.gps);
			this.splitContainer1.Panel2.Controls.Add(this.ccpXcpDescriptions);
			this.splitContainer1.Panel2.Controls.Add(this.ccpXcpSignalRequests);
			componentResourceManager.ApplyResources(this.treeControl, "treeControl");
			this.treeControl.Name = "treeControl";
			this.treeControl.UpdateService = null;
			componentResourceManager.ApplyResources(this.combinedAnalogDigitalInputs, "combinedAnalogDigitalInputs");
			this.combinedAnalogDigitalInputs.Name = "combinedAnalogDigitalInputs";
			this.multibusChannels.ApplicationDatabaseManager = null;
			componentResourceManager.ApplyResources(this.multibusChannels, "multibusChannels");
			this.multibusChannels.HardwareFrontend = null;
			this.multibusChannels.Name = "multibusChannels";
			this.cardReaderNavigator.AnalysisPackageParameters = null;
			this.cardReaderNavigator.ApplicationDatabaseManager = null;
			this.cardReaderNavigator.ConfigurationFolderPath = null;
			this.cardReaderNavigator.ConfigurationManagerService = null;
			this.cardReaderNavigator.CurrentDevice = null;
			this.cardReaderNavigator.DisplayMode = null;
			componentResourceManager.ApplyResources(this.cardReaderNavigator, "cardReaderNavigator");
			this.cardReaderNavigator.HardwareFrontend = null;
			this.cardReaderNavigator.Name = "cardReaderNavigator";
			this.loggerDeviceNavigator.AnalysisPackageParameters = null;
			this.loggerDeviceNavigator.ApplicationDatabaseManager = null;
			this.loggerDeviceNavigator.ConfigurationFolderPath = null;
			this.loggerDeviceNavigator.ConfigurationManagerService = null;
			this.loggerDeviceNavigator.DisplayMode = null;
			componentResourceManager.ApplyResources(this.loggerDeviceNavigator, "loggerDeviceNavigator");
			this.loggerDeviceNavigator.HardwareFrontend = null;
			this.loggerDeviceNavigator.Name = "loggerDeviceNavigator";
			this.loggerDeviceNavigator.UseWaitCursor = true;
			this.sendMessage.ApplicationDatabaseManager = null;
			componentResourceManager.ApplyResources(this.sendMessage, "sendMessage");
			this.sendMessage.Name = "sendMessage";
			this.digitalOutputs.ApplicationDatabaseManager = null;
			componentResourceManager.ApplyResources(this.digitalOutputs, "digitalOutputs");
			this.digitalOutputs.Name = "digitalOutputs";
			this.triggers2.ApplicationDatabaseManager = null;
			this.triggers2.AutoValidate = AutoValidate.EnableAllowFocusChange;
			this.triggers2.ConfigurationManagerService = null;
			this.triggers2.DiagnosticActionsConfiguration = null;
			this.triggers2.DiagnosticsDatabaseConfiguration = null;
			componentResourceManager.ApplyResources(this.triggers2, "triggers2");
			this.triggers2.MemoryNr = 0;
			this.triggers2.Name = "triggers2";
			this.triggers1.ApplicationDatabaseManager = null;
			this.triggers1.AutoValidate = AutoValidate.EnableAllowFocusChange;
			this.triggers1.ConfigurationManagerService = null;
			this.triggers1.DiagnosticActionsConfiguration = null;
			this.triggers1.DiagnosticsDatabaseConfiguration = null;
			componentResourceManager.ApplyResources(this.triggers1, "triggers1");
			this.triggers1.MemoryNr = 0;
			this.triggers1.Name = "triggers1";
			this.canChannels.ConfigurationManagerService = null;
			componentResourceManager.ApplyResources(this.canChannels, "canChannels");
			this.canChannels.Name = "canChannels";
			componentResourceManager.ApplyResources(this.digitalInputs, "digitalInputs");
			this.digitalInputs.Name = "digitalInputs";
			componentResourceManager.ApplyResources(this.comment, "comment");
			this.comment.LoggerType = LoggerType.Unknown;
			this.comment.Name = "comment";
			this.comment.VLProject = null;
			componentResourceManager.ApplyResources(this.most150Channels, "most150Channels");
			this.most150Channels.Name = "most150Channels";
			this.interfaceMode.ApplicationDatabaseManager = null;
			this.interfaceMode.ConfigurationFolderPath = null;
			componentResourceManager.ApplyResources(this.interfaceMode, "interfaceMode");
			this.interfaceMode.Name = "interfaceMode";
			this.diagnosticActions.ApplicationDatabaseManager = null;
			this.diagnosticActions.DiagSymbolsManager = null;
			componentResourceManager.ApplyResources(this.diagnosticActions, "diagnosticActions");
			this.diagnosticActions.Name = "diagnosticActions";
			this.diagnosticsDatabases.DiagSymbolsManager = null;
			componentResourceManager.ApplyResources(this.diagnosticsDatabases, "diagnosticsDatabases");
			this.diagnosticsDatabases.Name = "diagnosticsDatabases";
			componentResourceManager.ApplyResources(this.wlanSettings, "wlanSettings");
			this.wlanSettings.Name = "wlanSettings";
			componentResourceManager.ApplyResources(this.flexrayChannels, "flexrayChannels");
			this.flexrayChannels.Name = "flexrayChannels";
			this.deviceInformation.DisplayMode = null;
			componentResourceManager.ApplyResources(this.deviceInformation, "deviceInformation");
			this.deviceInformation.HardwareFrontend = null;
			this.deviceInformation.Name = "deviceInformation";
			this.loggerDevice.AnalysisPackageParameters = null;
			this.loggerDevice.ApplicationDatabaseManager = null;
			this.loggerDevice.ConfigurationFolderPath = null;
			this.loggerDevice.ConfigurationManagerService = null;
			componentResourceManager.ApplyResources(this.loggerDevice, "loggerDevice");
			this.loggerDevice.HardwareFrontend = null;
			this.loggerDevice.Name = "loggerDevice";
			componentResourceManager.ApplyResources(this.specialFeatures, "specialFeatures");
			this.specialFeatures.Name = "specialFeatures";
			this.includeFiles.DisplayMode = null;
			componentResourceManager.ApplyResources(this.includeFiles, "includeFiles");
			this.includeFiles.Name = "includeFiles";
			this.cardReader.AnalysisPackageParameters = null;
			this.cardReader.ApplicationDatabaseManager = null;
			this.cardReader.ConfigurationFolderPath = null;
			this.cardReader.ConfigurationManagerService = null;
			this.cardReader.DisplayMode = null;
			componentResourceManager.ApplyResources(this.cardReader, "cardReader");
			this.cardReader.HardwareFrontend = null;
			this.cardReader.Name = "cardReader";
			this.clfExport.AnalysisPackageParameters = null;
			this.clfExport.ApplicationDatabaseManager = null;
			this.clfExport.ConfigurationFolderPath = null;
			this.clfExport.DisplayMode = null;
			componentResourceManager.ApplyResources(this.clfExport, "clfExport");
			this.clfExport.Name = "clfExport";
			this.filters2.ApplicationDatabaseManager = null;
			componentResourceManager.ApplyResources(this.filters2, "filters2");
			this.filters2.GlobalOptions = null;
			this.filters2.MemoryNr = 0;
			this.filters2.Name = "filters2";
			this.filters1.ApplicationDatabaseManager = null;
			componentResourceManager.ApplyResources(this.filters1, "filters1");
			this.filters1.GlobalOptions = null;
			this.filters1.MemoryNr = 0;
			this.filters1.Name = "filters1";
			componentResourceManager.ApplyResources(this.analogInputs, "analogInputs");
			this.analogInputs.Name = "analogInputs";
			componentResourceManager.ApplyResources(this.leds, "leds");
			this.leds.Name = "leds";
			this.linChannels.ApplicationDatabaseManager = null;
			componentResourceManager.ApplyResources(this.linChannels, "linChannels");
			this.linChannels.Name = "linChannels";
			this.hardwareSettings.ApplicationDatabaseManager = null;
			this.hardwareSettings.AutoValidate = AutoValidate.EnableAllowFocusChange;
			componentResourceManager.ApplyResources(this.hardwareSettings, "hardwareSettings");
			this.hardwareSettings.Name = "hardwareSettings";
			this.databases.ApplicationDatabaseManager = null;
			this.databases.DisplayMode = null;
			componentResourceManager.ApplyResources(this.databases, "databases");
			this.databases.Name = "databases";
			this.gps.ApplicationDatabaseManager = null;
			componentResourceManager.ApplyResources(this.gps, "gps");
			this.gps.Name = "gps";
			this.ccpXcpDescriptions.ApplicationDatabaseManager = null;
			this.ccpXcpDescriptions.ConfigurationFolderPath = null;
			this.ccpXcpDescriptions.DisplayMode = null;
			componentResourceManager.ApplyResources(this.ccpXcpDescriptions, "ccpXcpDescriptions");
			this.ccpXcpDescriptions.Name = "ccpXcpDescriptions";
			this.ccpXcpSignalRequests.ApplicationDatabaseManager = null;
			this.ccpXcpSignalRequests.ConfigurationFolderPath = null;
			this.ccpXcpSignalRequests.DisplayMode = null;
			componentResourceManager.ApplyResources(this.ccpXcpSignalRequests, "ccpXcpSignalRequests");
			this.ccpXcpSignalRequests.Name = "ccpXcpSignalRequests";
			componentResourceManager.ApplyResources(this.mainMenuStrip, "mainMenuStrip");
			this.mainMenuStrip.Items.AddRange(new ToolStripItem[]
			{
				this.fileToolStripMenuItem,
				this.configurationToolStripMenuItem,
				this.deviceToolStripMenuItem,
				this.windowToolStripMenuItem,
				this.toolsToolStripMenuItem,
				this.helpToolStripMenuItem
			});
			this.mainMenuStrip.Name = "mainMenuStrip";
			this.mainMenuStrip.MenuActivate += new EventHandler(this.mainMenuStrip_MenuActivate);
			this.fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.newProjectToolStripMenuItem1,
				this.openProjectToolStripMenuItem1,
				this.toolStripSeparator4,
				this.saveProjectToolStripMenuItem,
				this.saveProjectAsToolStripMenuItem,
				this.toolStripSeparator5,
				this.importProjectToolStripMenuItem,
				this.exportProjectToolStripMenuItem,
				this.toolStripSeparator20,
				this.toolStripSeparator6,
				this.exitToolStripMenuItem1
			});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			componentResourceManager.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
			this.newProjectToolStripMenuItem1.Image = Resources.ImageNewProject;
			this.newProjectToolStripMenuItem1.Name = "newProjectToolStripMenuItem1";
			componentResourceManager.ApplyResources(this.newProjectToolStripMenuItem1, "newProjectToolStripMenuItem1");
			this.newProjectToolStripMenuItem1.Click += new EventHandler(this.newProjectToolStripMenuItem1_Click);
			this.openProjectToolStripMenuItem1.Image = Resources.ImageOpenProject;
			this.openProjectToolStripMenuItem1.Name = "openProjectToolStripMenuItem1";
			componentResourceManager.ApplyResources(this.openProjectToolStripMenuItem1, "openProjectToolStripMenuItem1");
			this.openProjectToolStripMenuItem1.Click += new EventHandler(this.openProjectToolStripMenuItem1_Click);
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			componentResourceManager.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
			this.saveProjectToolStripMenuItem.Image = Resources.ImageSave;
			this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
			componentResourceManager.ApplyResources(this.saveProjectToolStripMenuItem, "saveProjectToolStripMenuItem");
			this.saveProjectToolStripMenuItem.Click += new EventHandler(this.saveProjectToolStripMenuItem_Click);
			this.saveProjectAsToolStripMenuItem.Image = Resources.ImageSaveAs;
			this.saveProjectAsToolStripMenuItem.Name = "saveProjectAsToolStripMenuItem";
			componentResourceManager.ApplyResources(this.saveProjectAsToolStripMenuItem, "saveProjectAsToolStripMenuItem");
			this.saveProjectAsToolStripMenuItem.Click += new EventHandler(this.saveProjectAsToolStripMenuItem_Click);
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			componentResourceManager.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
			this.importProjectToolStripMenuItem.Name = "importProjectToolStripMenuItem";
			componentResourceManager.ApplyResources(this.importProjectToolStripMenuItem, "importProjectToolStripMenuItem");
			this.importProjectToolStripMenuItem.Click += new EventHandler(this.importProjectToolStripMenuItem_Click);
			this.exportProjectToolStripMenuItem.Name = "exportProjectToolStripMenuItem";
			componentResourceManager.ApplyResources(this.exportProjectToolStripMenuItem, "exportProjectToolStripMenuItem");
			this.exportProjectToolStripMenuItem.Click += new EventHandler(this.exportProjectToolStripMenuItem_Click);
			this.toolStripSeparator20.Name = "toolStripSeparator20";
			componentResourceManager.ApplyResources(this.toolStripSeparator20, "toolStripSeparator20");
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			componentResourceManager.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
			this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
			componentResourceManager.ApplyResources(this.exitToolStripMenuItem1, "exitToolStripMenuItem1");
			this.exitToolStripMenuItem1.Click += new EventHandler(this.exitToolStripMenuItem1_Click);
			this.configurationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.writeToDeviceToolStripMenuItem,
				this.writeToMemoryCardToolStripMenuItem,
				this.writeCODFileToDeviceToolStripMenuItem,
				this.writeCODFileToMemoryCardToolStripMenuItem,
				this.toolStripSeparator9,
				this.exportToLTLFileToolStripMenuItem,
				this.exportToCODFileToolStripMenuItem,
				this.toolStripSeparator13,
				this.createDatabaseForToolStripMenuItem
			});
			this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
			componentResourceManager.ApplyResources(this.configurationToolStripMenuItem, "configurationToolStripMenuItem");
			this.writeToDeviceToolStripMenuItem.Image = Resources.ImageWriteToDevice;
			this.writeToDeviceToolStripMenuItem.Name = "writeToDeviceToolStripMenuItem";
			componentResourceManager.ApplyResources(this.writeToDeviceToolStripMenuItem, "writeToDeviceToolStripMenuItem");
			this.writeToDeviceToolStripMenuItem.Click += new EventHandler(this.writeToDeviceToolStripMenuItem_Click);
			this.writeToMemoryCardToolStripMenuItem.Image = Resources.ImageWriteToCard;
			this.writeToMemoryCardToolStripMenuItem.Name = "writeToMemoryCardToolStripMenuItem";
			componentResourceManager.ApplyResources(this.writeToMemoryCardToolStripMenuItem, "writeToMemoryCardToolStripMenuItem");
			this.writeToMemoryCardToolStripMenuItem.Click += new EventHandler(this.writeToMemoryCardToolStripMenuItem_Click);
			this.writeCODFileToDeviceToolStripMenuItem.Name = "writeCODFileToDeviceToolStripMenuItem";
			componentResourceManager.ApplyResources(this.writeCODFileToDeviceToolStripMenuItem, "writeCODFileToDeviceToolStripMenuItem");
			this.writeCODFileToDeviceToolStripMenuItem.Click += new EventHandler(this.writeCODFileToDeviceToolStripMenuItem_Click);
			this.writeCODFileToMemoryCardToolStripMenuItem.Name = "writeCODFileToMemoryCardToolStripMenuItem";
			componentResourceManager.ApplyResources(this.writeCODFileToMemoryCardToolStripMenuItem, "writeCODFileToMemoryCardToolStripMenuItem");
			this.writeCODFileToMemoryCardToolStripMenuItem.Click += new EventHandler(this.writeCODFileToMemoryCardToolStripMenuItem_Click);
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			componentResourceManager.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
			this.exportToLTLFileToolStripMenuItem.Image = Resources.ImageExportToLTL;
			componentResourceManager.ApplyResources(this.exportToLTLFileToolStripMenuItem, "exportToLTLFileToolStripMenuItem");
			this.exportToLTLFileToolStripMenuItem.Name = "exportToLTLFileToolStripMenuItem";
			this.exportToLTLFileToolStripMenuItem.Click += new EventHandler(this.exportToLTLFileToolStripMenuItem_Click);
			this.exportToCODFileToolStripMenuItem.Name = "exportToCODFileToolStripMenuItem";
			componentResourceManager.ApplyResources(this.exportToCODFileToolStripMenuItem, "exportToCODFileToolStripMenuItem");
			this.exportToCODFileToolStripMenuItem.Click += new EventHandler(this.exportToCODFileToolStripMenuItem_Click);
			this.toolStripSeparator13.Name = "toolStripSeparator13";
			componentResourceManager.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
			this.createDatabaseForToolStripMenuItem.Name = "createDatabaseForToolStripMenuItem";
			componentResourceManager.ApplyResources(this.createDatabaseForToolStripMenuItem, "createDatabaseForToolStripMenuItem");
			this.createDatabaseForToolStripMenuItem.Click += new EventHandler(this.createDatabaseForToolStripMenuItem_Click);
			this.deviceToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.setRealTimeClockToolStripMenuItem,
				this.setVehicleNameToolStripMenuItem,
				this.setEthernetAddressToolStripMenuItem,
				this.updateLoggerFirmwareToolStripMenuItem,
				this.toolStripSeparator15,
				this.writeLicenseToDeviceToolStripMenuItem,
				this.writeLicenseToMemoryCardToolStripMenuItem,
				this.toolStripSeparator18,
				this.clearDeviceToolStripMenuItem,
				this.clearMemoryCardToolStripMenuItem,
				this.toolStripSeparator19,
				this.formatMemoryCardInDeviceToolStripMenuItem,
				this.formatMemoryCardToolStripMenuItem,
				this.toolStripSeparator21,
				this.importProjectFromDeviceToolStripMenuItem,
				this.importProjectFromMemoryCardToolStripMenuItem,
				this.toolStripSeparator24,
				this.analysisPackageImportFromDeviceToolStripMenuItem,
				this.analysisPackageImportFromMemoryCardToolStripMenuItem,
				this.toolStripSeparator22,
				this.packLoggingDataFromDeviceToolStripMenuItem,
				this.packLoggingDataFromMemoryCardToolStripMenuItem
			});
			this.deviceToolStripMenuItem.Name = "deviceToolStripMenuItem";
			componentResourceManager.ApplyResources(this.deviceToolStripMenuItem, "deviceToolStripMenuItem");
			this.setRealTimeClockToolStripMenuItem.Name = "setRealTimeClockToolStripMenuItem";
			componentResourceManager.ApplyResources(this.setRealTimeClockToolStripMenuItem, "setRealTimeClockToolStripMenuItem");
			this.setRealTimeClockToolStripMenuItem.Click += new EventHandler(this.setRealTimeClockToolStripMenuItem_Click);
			this.setVehicleNameToolStripMenuItem.Name = "setVehicleNameToolStripMenuItem";
			componentResourceManager.ApplyResources(this.setVehicleNameToolStripMenuItem, "setVehicleNameToolStripMenuItem");
			this.setVehicleNameToolStripMenuItem.Click += new EventHandler(this.setVehicleNameToolStripMenuItem_Click);
			this.setEthernetAddressToolStripMenuItem.Name = "setEthernetAddressToolStripMenuItem";
			componentResourceManager.ApplyResources(this.setEthernetAddressToolStripMenuItem, "setEthernetAddressToolStripMenuItem");
			this.setEthernetAddressToolStripMenuItem.Click += new EventHandler(this.setEthernetAddressToolStripMenuItem_Click);
			componentResourceManager.ApplyResources(this.updateLoggerFirmwareToolStripMenuItem, "updateLoggerFirmwareToolStripMenuItem");
			this.updateLoggerFirmwareToolStripMenuItem.Name = "updateLoggerFirmwareToolStripMenuItem";
			this.updateLoggerFirmwareToolStripMenuItem.Click += new EventHandler(this.updateLoggerFirmwareToolStripMenuItem_Click);
			this.toolStripSeparator15.Name = "toolStripSeparator15";
			componentResourceManager.ApplyResources(this.toolStripSeparator15, "toolStripSeparator15");
			this.writeLicenseToDeviceToolStripMenuItem.Name = "writeLicenseToDeviceToolStripMenuItem";
			componentResourceManager.ApplyResources(this.writeLicenseToDeviceToolStripMenuItem, "writeLicenseToDeviceToolStripMenuItem");
			this.writeLicenseToDeviceToolStripMenuItem.Click += new EventHandler(this.writeLicenseToDeviceToolStripMenuItem_Click);
			this.writeLicenseToMemoryCardToolStripMenuItem.Name = "writeLicenseToMemoryCardToolStripMenuItem";
			componentResourceManager.ApplyResources(this.writeLicenseToMemoryCardToolStripMenuItem, "writeLicenseToMemoryCardToolStripMenuItem");
			this.writeLicenseToMemoryCardToolStripMenuItem.Click += new EventHandler(this.writeLicenseToMemoryCardToolStripMenuItem_Click);
			this.toolStripSeparator18.Name = "toolStripSeparator18";
			componentResourceManager.ApplyResources(this.toolStripSeparator18, "toolStripSeparator18");
			this.clearDeviceToolStripMenuItem.Name = "clearDeviceToolStripMenuItem";
			componentResourceManager.ApplyResources(this.clearDeviceToolStripMenuItem, "clearDeviceToolStripMenuItem");
			this.clearDeviceToolStripMenuItem.Click += new EventHandler(this.clearDeviceToolStripMenuItem_Click);
			this.clearMemoryCardToolStripMenuItem.Name = "clearMemoryCardToolStripMenuItem";
			componentResourceManager.ApplyResources(this.clearMemoryCardToolStripMenuItem, "clearMemoryCardToolStripMenuItem");
			this.clearMemoryCardToolStripMenuItem.Click += new EventHandler(this.clearMemoryCardToolStripMenuItem_Click);
			this.toolStripSeparator19.Name = "toolStripSeparator19";
			componentResourceManager.ApplyResources(this.toolStripSeparator19, "toolStripSeparator19");
			this.formatMemoryCardInDeviceToolStripMenuItem.Name = "formatMemoryCardInDeviceToolStripMenuItem";
			componentResourceManager.ApplyResources(this.formatMemoryCardInDeviceToolStripMenuItem, "formatMemoryCardInDeviceToolStripMenuItem");
			this.formatMemoryCardInDeviceToolStripMenuItem.Click += new EventHandler(this.formatMemoryCardInDeviceToolStripMenuItem_Click);
			this.formatMemoryCardToolStripMenuItem.Name = "formatMemoryCardToolStripMenuItem";
			componentResourceManager.ApplyResources(this.formatMemoryCardToolStripMenuItem, "formatMemoryCardToolStripMenuItem");
			this.formatMemoryCardToolStripMenuItem.Click += new EventHandler(this.formatMemoryCardToolStripMenuItem_Click);
			this.toolStripSeparator21.Name = "toolStripSeparator21";
			componentResourceManager.ApplyResources(this.toolStripSeparator21, "toolStripSeparator21");
			this.importProjectFromDeviceToolStripMenuItem.Name = "importProjectFromDeviceToolStripMenuItem";
			componentResourceManager.ApplyResources(this.importProjectFromDeviceToolStripMenuItem, "importProjectFromDeviceToolStripMenuItem");
			this.importProjectFromDeviceToolStripMenuItem.Click += new EventHandler(this.importProjectFromDeviceToolStripMenuItem_Click);
			this.importProjectFromMemoryCardToolStripMenuItem.Name = "importProjectFromMemoryCardToolStripMenuItem";
			componentResourceManager.ApplyResources(this.importProjectFromMemoryCardToolStripMenuItem, "importProjectFromMemoryCardToolStripMenuItem");
			this.importProjectFromMemoryCardToolStripMenuItem.Click += new EventHandler(this.importProjectFromMemoryCardToolStripMenuItem_Click);
			this.toolStripSeparator24.Name = "toolStripSeparator24";
			componentResourceManager.ApplyResources(this.toolStripSeparator24, "toolStripSeparator24");
			this.analysisPackageImportFromDeviceToolStripMenuItem.Name = "analysisPackageImportFromDeviceToolStripMenuItem";
			componentResourceManager.ApplyResources(this.analysisPackageImportFromDeviceToolStripMenuItem, "analysisPackageImportFromDeviceToolStripMenuItem");
			this.analysisPackageImportFromDeviceToolStripMenuItem.Click += new EventHandler(this.analysisPackageImportFromDeviceToolStripMenuItem_Click);
			this.analysisPackageImportFromMemoryCardToolStripMenuItem.Name = "analysisPackageImportFromMemoryCardToolStripMenuItem";
			componentResourceManager.ApplyResources(this.analysisPackageImportFromMemoryCardToolStripMenuItem, "analysisPackageImportFromMemoryCardToolStripMenuItem");
			this.analysisPackageImportFromMemoryCardToolStripMenuItem.Click += new EventHandler(this.analysisPackageImportFromMemoryCardToolStripMenuItem_Click);
			this.toolStripSeparator22.Name = "toolStripSeparator22";
			componentResourceManager.ApplyResources(this.toolStripSeparator22, "toolStripSeparator22");
			this.packLoggingDataFromDeviceToolStripMenuItem.Image = Resources.ImageZipLogger;
			this.packLoggingDataFromDeviceToolStripMenuItem.Name = "packLoggingDataFromDeviceToolStripMenuItem";
			componentResourceManager.ApplyResources(this.packLoggingDataFromDeviceToolStripMenuItem, "packLoggingDataFromDeviceToolStripMenuItem");
			this.packLoggingDataFromDeviceToolStripMenuItem.Click += new EventHandler(this.packLoggingDataFromDeviceToolStrip_Click);
			this.packLoggingDataFromMemoryCardToolStripMenuItem.Image = Resources.ImageZipCardReader;
			this.packLoggingDataFromMemoryCardToolStripMenuItem.Name = "packLoggingDataFromMemoryCardToolStripMenuItem";
			componentResourceManager.ApplyResources(this.packLoggingDataFromMemoryCardToolStripMenuItem, "packLoggingDataFromMemoryCardToolStripMenuItem");
			this.packLoggingDataFromMemoryCardToolStripMenuItem.Click += new EventHandler(this.packLoggingDataFromMemoryCardToolStrip_Click);
			this.windowToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.refreshToolStripMenuItem,
				this.hexadecimalDisplayToolStripMenuItem
			});
			this.windowToolStripMenuItem.Name = "windowToolStripMenuItem";
			componentResourceManager.ApplyResources(this.windowToolStripMenuItem, "windowToolStripMenuItem");
			this.refreshToolStripMenuItem.Image = Resources.ImageRefresh;
			this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
			componentResourceManager.ApplyResources(this.refreshToolStripMenuItem, "refreshToolStripMenuItem");
			this.refreshToolStripMenuItem.Click += new EventHandler(this.refreshToolStripMenuItem_Click);
			this.hexadecimalDisplayToolStripMenuItem.CheckOnClick = true;
			this.hexadecimalDisplayToolStripMenuItem.Name = "hexadecimalDisplayToolStripMenuItem";
			componentResourceManager.ApplyResources(this.hexadecimalDisplayToolStripMenuItem, "hexadecimalDisplayToolStripMenuItem");
			this.hexadecimalDisplayToolStripMenuItem.Click += new EventHandler(this.hexadecimalDisplayToolStripMenuItem_Click);
			this.toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.optionsToolStripMenuItem,
				this.toolStripSeparator10,
				this.linProbeConfiguratorToolStripMenuItem,
				this.canShuntConfiguratorToolStripMenuItem,
				this.canGpsConfiguratorToolStripMenuItem,
				this.mlSetupToolStripMenuItem,
				this.toolStripSeparator17,
				this.createVSysVarFromIniToolStripMenuItem,
				this.toolStripSeparator16,
				this.splitFileToolStripMenuItem,
				this.mergeFileToolStripMenuItem
			});
			this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
			componentResourceManager.ApplyResources(this.toolsToolStripMenuItem, "toolsToolStripMenuItem");
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			componentResourceManager.ApplyResources(this.optionsToolStripMenuItem, "optionsToolStripMenuItem");
			this.optionsToolStripMenuItem.Click += new EventHandler(this.optionsToolStripMenuItem_Click);
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			componentResourceManager.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
			this.linProbeConfiguratorToolStripMenuItem.Name = "linProbeConfiguratorToolStripMenuItem";
			componentResourceManager.ApplyResources(this.linProbeConfiguratorToolStripMenuItem, "linProbeConfiguratorToolStripMenuItem");
			this.linProbeConfiguratorToolStripMenuItem.Click += new EventHandler(this.linProbeConfiguratorToolStripMenuItem_Click);
			this.canShuntConfiguratorToolStripMenuItem.Name = "canShuntConfiguratorToolStripMenuItem";
			componentResourceManager.ApplyResources(this.canShuntConfiguratorToolStripMenuItem, "canShuntConfiguratorToolStripMenuItem");
			this.canShuntConfiguratorToolStripMenuItem.Click += new EventHandler(this.canShuntConfiguratorToolStripMenuItem_Click);
			this.canGpsConfiguratorToolStripMenuItem.Name = "canGpsConfiguratorToolStripMenuItem";
			componentResourceManager.ApplyResources(this.canGpsConfiguratorToolStripMenuItem, "canGpsConfiguratorToolStripMenuItem");
			this.canGpsConfiguratorToolStripMenuItem.Click += new EventHandler(this.canGpsConfiguratorToolStripMenuItem_Click);
			this.mlSetupToolStripMenuItem.Name = "mlSetupToolStripMenuItem";
			componentResourceManager.ApplyResources(this.mlSetupToolStripMenuItem, "mlSetupToolStripMenuItem");
			this.mlSetupToolStripMenuItem.Click += new EventHandler(this.mlSetupToolStripMenuItem_Click);
			this.toolStripSeparator17.Name = "toolStripSeparator17";
			componentResourceManager.ApplyResources(this.toolStripSeparator17, "toolStripSeparator17");
			this.createVSysVarFromIniToolStripMenuItem.Name = "createVSysVarFromIniToolStripMenuItem";
			componentResourceManager.ApplyResources(this.createVSysVarFromIniToolStripMenuItem, "createVSysVarFromIniToolStripMenuItem");
			this.createVSysVarFromIniToolStripMenuItem.Click += new EventHandler(this.createVSysVarFromIniToolStripMenuItem_Click);
			this.toolStripSeparator16.Name = "toolStripSeparator16";
			componentResourceManager.ApplyResources(this.toolStripSeparator16, "toolStripSeparator16");
			this.splitFileToolStripMenuItem.Name = "splitFileToolStripMenuItem";
			componentResourceManager.ApplyResources(this.splitFileToolStripMenuItem, "splitFileToolStripMenuItem");
			this.splitFileToolStripMenuItem.Click += new EventHandler(this.splitFileToolStripMenuItem_Click);
			this.mergeFileToolStripMenuItem.Name = "mergeFileToolStripMenuItem";
			componentResourceManager.ApplyResources(this.mergeFileToolStripMenuItem, "mergeFileToolStripMenuItem");
			this.mergeFileToolStripMenuItem.Click += new EventHandler(this.mergeFileToolStripMenuItem_Click);
			this.helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[]
			{
				this.contentsToolStripMenuItem,
				this.toolStripSeparator11,
				this.aboutToolStripMenuItem
			});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			componentResourceManager.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
			this.contentsToolStripMenuItem.Image = Resources.ImageHelp;
			this.contentsToolStripMenuItem.Name = "contentsToolStripMenuItem";
			componentResourceManager.ApplyResources(this.contentsToolStripMenuItem, "contentsToolStripMenuItem");
			this.contentsToolStripMenuItem.Click += new EventHandler(this.contentsToolStripMenuItem_Click);
			this.toolStripSeparator11.Name = "toolStripSeparator11";
			componentResourceManager.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
			this.aboutToolStripMenuItem.Image = Resources.ImageAbout;
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			componentResourceManager.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
			this.aboutToolStripMenuItem.Click += new EventHandler(this.aboutToolStripMenuItem_Click);
			this.mainMenuToolStrip.AllowMerge = false;
			componentResourceManager.ApplyResources(this.mainMenuToolStrip, "mainMenuToolStrip");
			this.mainMenuToolStrip.Items.AddRange(new ToolStripItem[]
			{
				this.toolStripButtonNewProject,
				this.toolStripButtonOpenProject,
				this.toolStripButtonSaveProject,
				this.toolStripButtonSaveProjectAs,
				this.toolStripSeparator7,
				this.toolStripButtonWriteToDevice,
				this.toolStripButtonWriteToCard,
				this.toolStripButtonExportToLTL,
				this.toolStripSeparator23,
				this.packLoggingDataFromDeviceToolStripButton,
				this.packLoggingDataFromCardReaderToolStripButton,
				this.toolStripSeparator12,
				this.toolStripButtonSetRealtimeClock,
				this.toolStripSeparator8,
				this.toolStripButtonRefresh,
				this.toolStripButtonHexMode,
				this.toolStripSeparator14,
				this.toolStripButtonAbout,
				this.toolStripButtonHelp
			});
			this.mainMenuToolStrip.Name = "mainMenuToolStrip";
			this.mainMenuToolStrip.Click += new EventHandler(this.mainMenuToolStrip_Click);
			this.toolStripButtonNewProject.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonNewProject.Image = Resources.ImageNewProject;
			componentResourceManager.ApplyResources(this.toolStripButtonNewProject, "toolStripButtonNewProject");
			this.toolStripButtonNewProject.Name = "toolStripButtonNewProject";
			this.toolStripButtonNewProject.Click += new EventHandler(this.toolStripButtonNewProject_Click);
			this.toolStripButtonOpenProject.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonOpenProject.Image = Resources.ImageOpenProject;
			componentResourceManager.ApplyResources(this.toolStripButtonOpenProject, "toolStripButtonOpenProject");
			this.toolStripButtonOpenProject.Name = "toolStripButtonOpenProject";
			this.toolStripButtonOpenProject.Click += new EventHandler(this.toolStripButtonOpenProject_Click);
			this.toolStripButtonSaveProject.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSaveProject.Image = Resources.ImageSave;
			componentResourceManager.ApplyResources(this.toolStripButtonSaveProject, "toolStripButtonSaveProject");
			this.toolStripButtonSaveProject.Name = "toolStripButtonSaveProject";
			this.toolStripButtonSaveProject.Click += new EventHandler(this.toolStripButtonSaveProject_Click);
			this.toolStripButtonSaveProjectAs.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSaveProjectAs.Image = Resources.ImageSaveAs;
			componentResourceManager.ApplyResources(this.toolStripButtonSaveProjectAs, "toolStripButtonSaveProjectAs");
			this.toolStripButtonSaveProjectAs.Name = "toolStripButtonSaveProjectAs";
			this.toolStripButtonSaveProjectAs.Click += new EventHandler(this.toolStripButtonSaveProjectAs_Click);
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			componentResourceManager.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
			this.toolStripButtonWriteToDevice.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonWriteToDevice.Image = Resources.ImageWriteToDevice;
			componentResourceManager.ApplyResources(this.toolStripButtonWriteToDevice, "toolStripButtonWriteToDevice");
			this.toolStripButtonWriteToDevice.Name = "toolStripButtonWriteToDevice";
			this.toolStripButtonWriteToDevice.Click += new EventHandler(this.toolStripButtonWriteToDevice_Click);
			this.toolStripButtonWriteToCard.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonWriteToCard.Image = Resources.ImageWriteToCard;
			componentResourceManager.ApplyResources(this.toolStripButtonWriteToCard, "toolStripButtonWriteToCard");
			this.toolStripButtonWriteToCard.Name = "toolStripButtonWriteToCard";
			this.toolStripButtonWriteToCard.Click += new EventHandler(this.toolStripButtonWriteToCard_Click);
			this.toolStripButtonExportToLTL.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonExportToLTL.Image = Resources.ImageExportToLTL;
			componentResourceManager.ApplyResources(this.toolStripButtonExportToLTL, "toolStripButtonExportToLTL");
			this.toolStripButtonExportToLTL.Name = "toolStripButtonExportToLTL";
			this.toolStripButtonExportToLTL.Click += new EventHandler(this.toolStripButtonExportToLTL_Click);
			this.toolStripSeparator23.Name = "toolStripSeparator23";
			componentResourceManager.ApplyResources(this.toolStripSeparator23, "toolStripSeparator23");
			this.packLoggingDataFromDeviceToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.packLoggingDataFromDeviceToolStripButton.Image = Resources.ImageZipLogger;
			componentResourceManager.ApplyResources(this.packLoggingDataFromDeviceToolStripButton, "packLoggingDataFromDeviceToolStripButton");
			this.packLoggingDataFromDeviceToolStripButton.Name = "packLoggingDataFromDeviceToolStripButton";
			this.packLoggingDataFromDeviceToolStripButton.Click += new EventHandler(this.packLoggingDataFromDeviceToolStrip_Click);
			this.packLoggingDataFromCardReaderToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.packLoggingDataFromCardReaderToolStripButton.Image = Resources.ImageZipCardReader;
			componentResourceManager.ApplyResources(this.packLoggingDataFromCardReaderToolStripButton, "packLoggingDataFromCardReaderToolStripButton");
			this.packLoggingDataFromCardReaderToolStripButton.Name = "packLoggingDataFromCardReaderToolStripButton";
			this.packLoggingDataFromCardReaderToolStripButton.Click += new EventHandler(this.packLoggingDataFromMemoryCardToolStrip_Click);
			this.toolStripSeparator12.Name = "toolStripSeparator12";
			componentResourceManager.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
			this.toolStripButtonSetRealtimeClock.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonSetRealtimeClock.Image = Resources.ImageClock;
			componentResourceManager.ApplyResources(this.toolStripButtonSetRealtimeClock, "toolStripButtonSetRealtimeClock");
			this.toolStripButtonSetRealtimeClock.Name = "toolStripButtonSetRealtimeClock";
			this.toolStripButtonSetRealtimeClock.Click += new EventHandler(this.toolStripButtonSetRealtimeClock_Click);
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			componentResourceManager.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
			this.toolStripButtonRefresh.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonRefresh.Image = Resources.ImageRefresh;
			componentResourceManager.ApplyResources(this.toolStripButtonRefresh, "toolStripButtonRefresh");
			this.toolStripButtonRefresh.Name = "toolStripButtonRefresh";
			this.toolStripButtonRefresh.Click += new EventHandler(this.toolStripButtonRefresh_Click);
			this.toolStripButtonHexMode.CheckOnClick = true;
			this.toolStripButtonHexMode.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonHexMode.Image = Resources.ImageHexMode;
			componentResourceManager.ApplyResources(this.toolStripButtonHexMode, "toolStripButtonHexMode");
			this.toolStripButtonHexMode.Name = "toolStripButtonHexMode";
			this.toolStripButtonHexMode.Click += new EventHandler(this.toolStripButtonHexMode_Click);
			this.toolStripSeparator14.Name = "toolStripSeparator14";
			componentResourceManager.ApplyResources(this.toolStripSeparator14, "toolStripSeparator14");
			this.toolStripButtonAbout.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonAbout.Image = Resources.ImageAbout;
			componentResourceManager.ApplyResources(this.toolStripButtonAbout, "toolStripButtonAbout");
			this.toolStripButtonAbout.Name = "toolStripButtonAbout";
			this.toolStripButtonAbout.Click += new EventHandler(this.toolStripButtonAbout_Click);
			this.toolStripButtonHelp.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonHelp.Image = Resources.ImageHelp;
			this.toolStripButtonHelp.Name = "toolStripButtonHelp";
			componentResourceManager.ApplyResources(this.toolStripButtonHelp, "toolStripButtonHelp");
			this.toolStripButtonHelp.Click += new EventHandler(this.toolStripButtonHelp_Click);
			componentResourceManager.ApplyResources(this.mToolStripFileConversionSettings, "mToolStripFileConversionSettings");
			this.mToolStripFileConversionSettings.Items.AddRange(new ToolStripItem[]
			{
				this.mToolStripDropDownLoadFileConversionSettings,
				this.mToolStripButtonSaveFileConversionSettings
			});
			this.mToolStripFileConversionSettings.Name = "mToolStripFileConversionSettings";
			this.mToolStripDropDownLoadFileConversionSettings.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.mToolStripDropDownLoadFileConversionSettings.Image = Resources.ImageImportConvertSettings;
			componentResourceManager.ApplyResources(this.mToolStripDropDownLoadFileConversionSettings, "mToolStripDropDownLoadFileConversionSettings");
			this.mToolStripDropDownLoadFileConversionSettings.Name = "mToolStripDropDownLoadFileConversionSettings";
			this.mToolStripDropDownLoadFileConversionSettings.DropDownOpening += new EventHandler(this.ToolStripDropDownButtonExportSettingsLoad_DropDownOpening);
			this.mToolStripDropDownLoadFileConversionSettings.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.ToolStripDropDownButtonExportSettingsLoad_DropDownItemClicked);
			this.mToolStripButtonSaveFileConversionSettings.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.mToolStripButtonSaveFileConversionSettings.Image = Resources.ImageExportConvertSettings;
			componentResourceManager.ApplyResources(this.mToolStripButtonSaveFileConversionSettings, "mToolStripButtonSaveFileConversionSettings");
			this.mToolStripButtonSaveFileConversionSettings.Name = "mToolStripButtonSaveFileConversionSettings";
			this.mToolStripButtonSaveFileConversionSettings.Click += new EventHandler(this.ToolStripButtonExportSettingsSave_Click);
			this.newProjectToolStripMenuItem.Image = Resources.ImageNewProject;
			this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
			componentResourceManager.ApplyResources(this.newProjectToolStripMenuItem, "newProjectToolStripMenuItem");
			this.openProjectToolStripMenuItem.Image = Resources.ImageOpenProject;
			this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
			componentResourceManager.ApplyResources(this.openProjectToolStripMenuItem, "openProjectToolStripMenuItem");
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			componentResourceManager.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
			componentResourceManager.ApplyResources(this.saveToolStripMenuItem, "saveToolStripMenuItem");
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			componentResourceManager.ApplyResources(this.saveAsToolStripMenuItem, "saveAsToolStripMenuItem");
			this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			componentResourceManager.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			componentResourceManager.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			componentResourceManager.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			componentResourceManager.ApplyResources(this.toolStripStatusLabel1, "toolStripStatusLabel1");
			this.toolStripButtonOpen.DisplayStyle = ToolStripItemDisplayStyle.Image;
			this.toolStripButtonOpen.Image = Resources.ImageOpenProject;
			componentResourceManager.ApplyResources(this.toolStripButtonOpen, "toolStripButtonOpen");
			this.toolStripButtonOpen.Name = "toolStripButtonOpen";
			this.toolStripButtonSave.DisplayStyle = ToolStripItemDisplayStyle.Image;
			componentResourceManager.ApplyResources(this.toolStripButtonSave, "toolStripButtonSave");
			this.toolStripButtonSave.Name = "toolStripButtonSave";
			this.toolStripButtonSaveAs.DisplayStyle = ToolStripItemDisplayStyle.Image;
			componentResourceManager.ApplyResources(this.toolStripButtonSaveAs, "toolStripButtonSaveAs");
			this.toolStripButtonSaveAs.Name = "toolStripButtonSaveAs";
			this.AllowDrop = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.toolStripContainer);
			base.Name = "MainWindow";
			base.FormClosing += new FormClosingEventHandler(this.OnMainWindowFormClosing);
			base.Load += new EventHandler(this.OnMainWindowLoad);
			base.Shown += new EventHandler(this.OnMainWindowShown);
			base.DragDrop += new DragEventHandler(this.MainWindow_DragDrop);
			base.DragEnter += new DragEventHandler(this.MainWindow_DragEnter);
			this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
			this.toolStripContainer.BottomToolStripPanel.PerformLayout();
			this.toolStripContainer.ContentPanel.ResumeLayout(false);
			this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
			this.toolStripContainer.TopToolStripPanel.PerformLayout();
			this.toolStripContainer.ResumeLayout(false);
			this.toolStripContainer.PerformLayout();
			this.statusStrip.ResumeLayout(false);
			this.statusStrip.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((ISupportInitialize)this.splitContainer1).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.mainMenuStrip.ResumeLayout(false);
			this.mainMenuStrip.PerformLayout();
			this.mainMenuToolStrip.ResumeLayout(false);
			this.mainMenuToolStrip.PerformLayout();
			this.mToolStripFileConversionSettings.ResumeLayout(false);
			this.mToolStripFileConversionSettings.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
