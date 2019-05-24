using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.Properties;

namespace Vector.VLConfig.Data.ApplicationDataPersistency
{
	public class AppDataWriter
	{
		public static bool SaveAppDataToSettingsFile(AppDataRoot appDataRoot)
		{
			bool flag = true;
			try
			{
				flag &= AppDataWriter.Serialize<DatabasesPage>(appDataRoot.DatabasesPage);
				flag &= AppDataWriter.Serialize<FiltersPage>(appDataRoot.Filters1Page);
				flag &= AppDataWriter.Serialize<FiltersPage>(appDataRoot.Filters2Page);
				flag &= AppDataWriter.Serialize<TriggersPage>(appDataRoot.Triggers1Page);
				flag &= AppDataWriter.Serialize<TriggersPage>(appDataRoot.Triggers2Page);
				flag &= AppDataWriter.Serialize<CLFExportPage>(appDataRoot.CLFExportPage);
				flag &= AppDataWriter.Serialize<CardReaderPage>(appDataRoot.CardReaderPage);
				flag &= AppDataWriter.Serialize<CardReaderNavigatorPage>(appDataRoot.CardReaderNavigatorPage);
				flag &= AppDataWriter.Serialize<IncludeFilesPage>(appDataRoot.IncludeFilesPage);
				flag &= AppDataWriter.Serialize<GlobalOptions>(appDataRoot.GlobalOptions);
				flag &= AppDataWriter.Serialize<FileConversionProfileList>(appDataRoot.FileConversionProfileList);
				flag &= AppDataWriter.Serialize<LoggerDevicePage>(appDataRoot.LoggerDevicePage);
				flag &= AppDataWriter.Serialize<LoggerDeviceNavigatorPage>(appDataRoot.LoggerDeviceNavigatorPage);
				flag &= AppDataWriter.Serialize<DiagnosticsDatabasesPage>(appDataRoot.DiagnosticsDatabasesPage);
				flag &= AppDataWriter.Serialize<DiagnosticActionsPage>(appDataRoot.DiagnosticActionsPage);
				flag &= AppDataWriter.Serialize<SendMessagePage>(appDataRoot.SendMessagePage);
				flag &= AppDataWriter.Serialize<DigitalOutputsPage>(appDataRoot.DigitalOutputsPage);
				flag &= AppDataWriter.Serialize<ProjectExporterPage>(appDataRoot.ProjectExporterPage);
				flag &= AppDataWriter.Serialize<WLANSettingsGL3PlusPage>(appDataRoot.WLANSettingsGL3PlusPage);
				flag &= AppDataWriter.Serialize<WLANSettingsGL2000Page>(appDataRoot.WLANSettingsGL2000Page);
				flag &= AppDataWriter.Serialize<CcpXcpDescriptionsPage>(appDataRoot.CcpXcpDescriptionsPage);
				flag &= AppDataWriter.Serialize<CcpXcpSignalRequestsPage>(appDataRoot.CcpXcpSignalRequestsPage);
				flag &= AppDataWriter.Serialize<SignalExportListPage>(appDataRoot.SignalExportListPage);
				flag &= AppDataWriter.Serialize<AnalysisPackageSettings>(appDataRoot.AnalysisPackageSettings);
				flag &= AppDataWriter.Serialize<ExportDatabasesPage>(appDataRoot.ExportDatabasesPage);
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		public static bool Serialize<T>(T appData) where T : IApplicationDataSettings
		{
			if (appData == null)
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
				memoryStream = new MemoryStream();
				StreamWriter textWriter = new StreamWriter(memoryStream, Encoding.Unicode);
				xmlSerializer.Serialize(textWriter, appData);
				memoryStream.Position = 0L;
				using (StreamReader streamReader = new StreamReader(memoryStream, Encoding.Unicode))
				{
					string value = streamReader.ReadToEnd();
					Settings.Default[appData.SettingName] = value;
				}
				Settings.Default.Save();
			}
			catch (Exception)
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
