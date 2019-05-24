using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.Properties;

namespace Vector.VLConfig.Data.ApplicationDataPersistency
{
	public class AppDataReader
	{
		public static bool ReadAppDataFromSettingsFile(ref AppDataRoot appDataRoot)
		{
			bool result = true;
			try
			{
				DatabasesPage databasesPage;
				if (AppDataReader.DeSerialize<DatabasesPage>(((IApplicationDataSettings)appDataRoot.DatabasesPage).SettingName, out databasesPage))
				{
					appDataRoot.DatabasesPage = databasesPage;
				}
				else
				{
					result = false;
				}
				FiltersPage filters1Page;
				if (AppDataReader.DeSerialize<FiltersPage>(((IApplicationDataSettings)appDataRoot.Filters1Page).SettingName, out filters1Page))
				{
					appDataRoot.Filters1Page = filters1Page;
				}
				else
				{
					result = false;
				}
				FiltersPage filters2Page;
				if (AppDataReader.DeSerialize<FiltersPage>(((IApplicationDataSettings)appDataRoot.Filters2Page).SettingName, out filters2Page))
				{
					appDataRoot.Filters2Page = filters2Page;
				}
				else
				{
					result = false;
				}
				TriggersPage triggers1Page;
				if (AppDataReader.DeSerialize<TriggersPage>(((IApplicationDataSettings)appDataRoot.Triggers1Page).SettingName, out triggers1Page))
				{
					appDataRoot.Triggers1Page = triggers1Page;
				}
				else
				{
					result = false;
				}
				TriggersPage triggers2Page;
				if (AppDataReader.DeSerialize<TriggersPage>(((IApplicationDataSettings)appDataRoot.Triggers2Page).SettingName, out triggers2Page))
				{
					appDataRoot.Triggers2Page = triggers2Page;
				}
				else
				{
					result = false;
				}
				CLFExportPage cLFExportPage;
				if (AppDataReader.DeSerialize<CLFExportPage>(((IApplicationDataSettings)appDataRoot.CLFExportPage).SettingName, out cLFExportPage))
				{
					appDataRoot.CLFExportPage = cLFExportPage;
				}
				else
				{
					result = false;
				}
				CardReaderPage cardReaderPage;
				if (AppDataReader.DeSerialize<CardReaderPage>(((IApplicationDataSettings)appDataRoot.CardReaderPage).SettingName, out cardReaderPage))
				{
					appDataRoot.CardReaderPage = cardReaderPage;
				}
				else
				{
					result = false;
				}
				CardReaderNavigatorPage cardReaderNavigatorPage;
				if (AppDataReader.DeSerialize<CardReaderNavigatorPage>(((IApplicationDataSettings)appDataRoot.CardReaderNavigatorPage).SettingName, out cardReaderNavigatorPage))
				{
					appDataRoot.CardReaderNavigatorPage = cardReaderNavigatorPage;
				}
				else
				{
					result = false;
				}
				IncludeFilesPage includeFilesPage;
				if (AppDataReader.DeSerialize<IncludeFilesPage>(((IApplicationDataSettings)appDataRoot.IncludeFilesPage).SettingName, out includeFilesPage))
				{
					appDataRoot.IncludeFilesPage = includeFilesPage;
				}
				else
				{
					result = false;
				}
				LoggerDevicePage loggerDevicePage;
				if (AppDataReader.DeSerialize<LoggerDevicePage>(((IApplicationDataSettings)appDataRoot.LoggerDevicePage).SettingName, out loggerDevicePage))
				{
					appDataRoot.LoggerDevicePage = loggerDevicePage;
				}
				else
				{
					result = false;
				}
				LoggerDeviceNavigatorPage loggerDeviceNavigatorPage;
				if (AppDataReader.DeSerialize<LoggerDeviceNavigatorPage>(((IApplicationDataSettings)appDataRoot.LoggerDeviceNavigatorPage).SettingName, out loggerDeviceNavigatorPage))
				{
					appDataRoot.LoggerDeviceNavigatorPage = loggerDeviceNavigatorPage;
				}
				else
				{
					result = false;
				}
				DiagnosticsDatabasesPage diagnosticsDatabasesPage;
				if (AppDataReader.DeSerialize<DiagnosticsDatabasesPage>(((IApplicationDataSettings)appDataRoot.DiagnosticsDatabasesPage).SettingName, out diagnosticsDatabasesPage))
				{
					appDataRoot.DiagnosticsDatabasesPage = diagnosticsDatabasesPage;
				}
				else
				{
					result = false;
				}
				DiagnosticActionsPage diagnosticActionsPage;
				if (AppDataReader.DeSerialize<DiagnosticActionsPage>(((IApplicationDataSettings)appDataRoot.DiagnosticActionsPage).SettingName, out diagnosticActionsPage))
				{
					appDataRoot.DiagnosticActionsPage = diagnosticActionsPage;
				}
				else
				{
					result = false;
				}
				SendMessagePage sendMessagePage;
				if (AppDataReader.DeSerialize<SendMessagePage>(((IApplicationDataSettings)appDataRoot.SendMessagePage).SettingName, out sendMessagePage))
				{
					appDataRoot.SendMessagePage = sendMessagePage;
				}
				else
				{
					result = false;
				}
				DigitalOutputsPage digitalOutputsPage;
				if (AppDataReader.DeSerialize<DigitalOutputsPage>(((IApplicationDataSettings)appDataRoot.DigitalOutputsPage).SettingName, out digitalOutputsPage))
				{
					appDataRoot.DigitalOutputsPage = digitalOutputsPage;
				}
				else
				{
					result = false;
				}
				GlobalOptions globalOptions;
				if (AppDataReader.DeSerialize<GlobalOptions>(((IApplicationDataSettings)appDataRoot.GlobalOptions).SettingName, out globalOptions))
				{
					appDataRoot.GlobalOptions = globalOptions;
				}
				else
				{
					result = false;
				}
				FileConversionProfileList fileConversionProfileList;
				if (AppDataReader.DeSerialize<FileConversionProfileList>(((IApplicationDataSettings)appDataRoot.FileConversionProfileList).SettingName, out fileConversionProfileList))
				{
					appDataRoot.FileConversionProfileList = fileConversionProfileList;
				}
				else
				{
					result = false;
				}
				ProjectExporterPage projectExporterPage;
				if (AppDataReader.DeSerialize<ProjectExporterPage>(((IApplicationDataSettings)appDataRoot.ProjectExporterPage).SettingName, out projectExporterPage))
				{
					appDataRoot.ProjectExporterPage = projectExporterPage;
				}
				else
				{
					result = false;
				}
				WLANSettingsGL3PlusPage wLANSettingsGL3PlusPage;
				if (AppDataReader.DeSerialize<WLANSettingsGL3PlusPage>(((IApplicationDataSettings)appDataRoot.WLANSettingsGL3PlusPage).SettingName, out wLANSettingsGL3PlusPage))
				{
					appDataRoot.WLANSettingsGL3PlusPage = wLANSettingsGL3PlusPage;
				}
				else
				{
					result = false;
				}
				WLANSettingsGL2000Page wLANSettingsGL2000Page;
				if (AppDataReader.DeSerialize<WLANSettingsGL2000Page>(((IApplicationDataSettings)appDataRoot.WLANSettingsGL2000Page).SettingName, out wLANSettingsGL2000Page))
				{
					appDataRoot.WLANSettingsGL2000Page = wLANSettingsGL2000Page;
				}
				else
				{
					result = false;
				}
				CcpXcpDescriptionsPage ccpXcpDescriptionsPage;
				if (AppDataReader.DeSerialize<CcpXcpDescriptionsPage>(((IApplicationDataSettings)appDataRoot.CcpXcpDescriptionsPage).SettingName, out ccpXcpDescriptionsPage))
				{
					appDataRoot.CcpXcpDescriptionsPage = ccpXcpDescriptionsPage;
				}
				else
				{
					result = false;
				}
				CcpXcpSignalRequestsPage ccpXcpSignalRequestsPage;
				if (AppDataReader.DeSerialize<CcpXcpSignalRequestsPage>(((IApplicationDataSettings)appDataRoot.CcpXcpSignalRequestsPage).SettingName, out ccpXcpSignalRequestsPage))
				{
					appDataRoot.CcpXcpSignalRequestsPage = ccpXcpSignalRequestsPage;
				}
				else
				{
					result = false;
				}
				SignalExportListPage signalExportListPage;
				if (AppDataReader.DeSerialize<SignalExportListPage>(((IApplicationDataSettings)appDataRoot.SignalExportListPage).SettingName, out signalExportListPage))
				{
					appDataRoot.SignalExportListPage = signalExportListPage;
				}
				else
				{
					result = false;
				}
				AnalysisPackageSettings analysisPackageSettings;
				if (AppDataReader.DeSerialize<AnalysisPackageSettings>(((IApplicationDataSettings)appDataRoot.AnalysisPackageSettings).SettingName, out analysisPackageSettings))
				{
					appDataRoot.AnalysisPackageSettings = analysisPackageSettings;
				}
				else
				{
					result = false;
				}
				ExportDatabasesPage exportDatabasesPage;
				if (AppDataReader.DeSerialize<ExportDatabasesPage>(((IApplicationDataSettings)appDataRoot.ExportDatabasesPage).SettingName, out exportDatabasesPage))
				{
					appDataRoot.ExportDatabasesPage = exportDatabasesPage;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public static bool DeSerialize<T>(string name, out T appData) where T : class, IApplicationDataSettings
		{
			appData = default(T);
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				string text = (string)Settings.Default[name];
				if (string.IsNullOrEmpty(text))
				{
					result = false;
				}
				else
				{
					UnicodeEncoding unicodeEncoding = new UnicodeEncoding();
					memoryStream = new MemoryStream(unicodeEncoding.GetBytes(text));
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
					appData = (xmlSerializer.Deserialize(memoryStream) as T);
					memoryStream.Close();
				}
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
	}
}
