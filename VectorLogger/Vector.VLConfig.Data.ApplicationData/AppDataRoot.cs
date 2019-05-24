using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public class AppDataRoot
	{
		public PathConfiguration PathConfiguration
		{
			get;
			set;
		}

		public AppDefaults AppDefaults
		{
			get;
			set;
		}

		public GlobalOptions GlobalOptions
		{
			get;
			set;
		}

		public FileConversionProfileList FileConversionProfileList
		{
			get;
			set;
		}

		public DatabasesPage DatabasesPage
		{
			get;
			set;
		}

		public FiltersPage Filters1Page
		{
			get;
			set;
		}

		public FiltersPage Filters2Page
		{
			get;
			set;
		}

		public TriggersPage Triggers1Page
		{
			get;
			set;
		}

		public TriggersPage Triggers2Page
		{
			get;
			set;
		}

		public CLFExportPage CLFExportPage
		{
			get;
			set;
		}

		public CardReaderPage CardReaderPage
		{
			get;
			set;
		}

		public CardReaderNavigatorPage CardReaderNavigatorPage
		{
			get;
			set;
		}

		public IncludeFilesPage IncludeFilesPage
		{
			get;
			set;
		}

		public CcpXcpDescriptionsPage CcpXcpDescriptionsPage
		{
			get;
			set;
		}

		public CcpXcpSignalRequestsPage CcpXcpSignalRequestsPage
		{
			get;
			set;
		}

		public LoggerDevicePage LoggerDevicePage
		{
			get;
			set;
		}

		public LoggerDeviceNavigatorPage LoggerDeviceNavigatorPage
		{
			get;
			set;
		}

		public DiagnosticsDatabasesPage DiagnosticsDatabasesPage
		{
			get;
			set;
		}

		public DiagnosticActionsPage DiagnosticActionsPage
		{
			get;
			set;
		}

		public SendMessagePage SendMessagePage
		{
			get;
			set;
		}

		public DigitalOutputsPage DigitalOutputsPage
		{
			get;
			set;
		}

		public ProjectExporterPage ProjectExporterPage
		{
			get;
			set;
		}

		public WLANSettingsGL3PlusPage WLANSettingsGL3PlusPage
		{
			get;
			set;
		}

		public WLANSettingsGL2000Page WLANSettingsGL2000Page
		{
			get;
			set;
		}

		public SignalExportListPage SignalExportListPage
		{
			get;
			set;
		}

		public AnalysisPackageSettings AnalysisPackageSettings
		{
			get;
			set;
		}

		public ExportDatabasesPage ExportDatabasesPage
		{
			get;
			set;
		}

		public AppDataRoot()
		{
			this.PathConfiguration = new PathConfiguration();
			this.AppDefaults = new AppDefaults();
			this.GlobalOptions = new GlobalOptions();
			this.FileConversionProfileList = new FileConversionProfileList();
			this.DatabasesPage = new DatabasesPage();
			this.Filters1Page = new FiltersPage();
			this.Filters1Page.MemoryNr = 1;
			this.Filters2Page = new FiltersPage();
			this.Filters2Page.MemoryNr = 2;
			this.Triggers1Page = new TriggersPage();
			this.Triggers1Page.MemoryNr = 1;
			this.Triggers2Page = new TriggersPage();
			this.Triggers2Page.MemoryNr = 2;
			this.CLFExportPage = new CLFExportPage();
			this.CardReaderPage = new CardReaderPage();
			this.CardReaderNavigatorPage = new CardReaderNavigatorPage();
			this.IncludeFilesPage = new IncludeFilesPage();
			this.CcpXcpDescriptionsPage = new CcpXcpDescriptionsPage();
			this.CcpXcpSignalRequestsPage = new CcpXcpSignalRequestsPage();
			this.LoggerDevicePage = new LoggerDevicePage();
			this.LoggerDeviceNavigatorPage = new LoggerDeviceNavigatorPage();
			this.DiagnosticsDatabasesPage = new DiagnosticsDatabasesPage();
			this.DiagnosticActionsPage = new DiagnosticActionsPage();
			this.SendMessagePage = new SendMessagePage();
			this.DigitalOutputsPage = new DigitalOutputsPage();
			this.ProjectExporterPage = new ProjectExporterPage();
			this.WLANSettingsGL3PlusPage = new WLANSettingsGL3PlusPage();
			this.WLANSettingsGL2000Page = new WLANSettingsGL2000Page();
			this.SignalExportListPage = new SignalExportListPage();
			this.AnalysisPackageSettings = new AnalysisPackageSettings();
			this.ExportDatabasesPage = new ExportDatabasesPage();
		}
	}
}
