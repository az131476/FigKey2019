using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.GUI.DatabasesPage;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Conversion
{
	public class AnalysisPackage
	{
		private class AutomaticSearchProgressIndicatorJob : GenericBackgroundWorkerJob
		{
			private ILoggerDevice loggerDevice;

			public List<string> MatchingFiles
			{
				get;
				private set;
			}

			public AutomaticSearchProgressIndicatorJob(ILoggerDevice loggerDevice)
			{
				this.loggerDevice = loggerDevice;
				this.MatchingFiles = new List<string>();
			}

			protected override void OnDoWork(object sender, DoWorkEventArgs e)
			{
				BackgroundWorker backgroundWorker = sender as BackgroundWorker;
				AnalysisPackage.AutomaticSearchProgressIndicatorJob automaticSearchProgressIndicatorJob = e.Argument as AnalysisPackage.AutomaticSearchProgressIndicatorJob;
				if (backgroundWorker == null || automaticSearchProgressIndicatorJob != this || this.loggerDevice == null)
				{
					return;
				}
				DateTime compileDateTime = this.loggerDevice.CompileDateTime;
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.loggerDevice.Name);
				string compileDateTime2 = string.Format(Resources.FormatDateTimeForFilenames, new object[]
				{
					compileDateTime.Year,
					compileDateTime.Month,
					compileDateTime.Day,
					compileDateTime.Hour,
					compileDateTime.Minute,
					compileDateTime.Second
				});
				if (AnalysisPackage.IsAnalysisPoolPathConfigured())
				{
					IEnumerable<string> accessibleFiles = this.GetAccessibleFiles(AnalysisPackage.Parameters.PoolPath, "*analysis.zip", backgroundWorker);
					if (backgroundWorker.CancellationPending)
					{
						e.Cancel = true;
						return;
					}
					foreach (string current in accessibleFiles)
					{
						if (AnalysisPackage.CheckPackage(fileNameWithoutExtension, compileDateTime2, current))
						{
							this.MatchingFiles.Add(current);
						}
					}
				}
			}

			private IEnumerable<string> GetAccessibleFiles(string path, string pattern, BackgroundWorker bgw)
			{
				IEnumerable<string> enumerable = Enumerable.Empty<string>();
				if (bgw.CancellationPending)
				{
					return enumerable;
				}
				try
				{
					if ((File.GetAttributes(path) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
					{
						enumerable = enumerable.Concat(Directory.EnumerateFiles(path, pattern));
						IEnumerable<string> enumerable2 = Directory.EnumerateDirectories(path);
						foreach (string current in enumerable2)
						{
							enumerable = enumerable.Concat(this.GetAccessibleFiles(current, pattern, bgw));
						}
					}
				}
				catch
				{
				}
				return enumerable;
			}
		}

		private class AutomaticSearchProgressIndicator : GenericBackgroundWorker
		{
			public override ProgressBarStyle ProgressBarStyle
			{
				get
				{
					return ProgressBarStyle.Marquee;
				}
			}

			public override bool ShowInTaskbar
			{
				get
				{
					return true;
				}
			}

			public override string ProgressFormTitle
			{
				get
				{
					return Resources.AutomaticSearchProgressBarTitle;
				}
			}

			public override string GetProgressStatusText(int finishedJobs, IList<GenericBackgroundWorkerJob> runningJobs)
			{
				return string.Format(Resources.AutomaticSearchFolder, AnalysisPackage.Parameters.PoolPath);
			}

			public IList<string> GetMatchingPackages(ILoggerDevice loggerDevice)
			{
				base.Jobs.Clear();
				AnalysisPackage.AutomaticSearchProgressIndicatorJob automaticSearchProgressIndicatorJob = new AnalysisPackage.AutomaticSearchProgressIndicatorJob(loggerDevice);
				base.Jobs.Add(automaticSearchProgressIndicatorJob);
				if (DialogResult.Cancel == base.ExecuteJobs())
				{
					return null;
				}
				return automaticSearchProgressIndicatorJob.MatchingFiles;
			}
		}

		private static readonly string glaFileName;

		private static string tempDir;

		private static List<Database> extractedExportDatabases;

		private static List<Database> notExtractedExportDatabases;

		private static bool searchInProgress;

		private static Dictionary<ExportDatabases, string> lastSearchedDevices;

		public static AnalysisPackageParameters Parameters
		{
			get;
			set;
		}

		static AnalysisPackage()
		{
			AnalysisPackage.glaFileName = "Analysis.gla";
			AnalysisPackage.extractedExportDatabases = new List<Database>();
			AnalysisPackage.notExtractedExportDatabases = new List<Database>();
			TempDirectoryManager.Instance.CreateNewTempDirectory(out AnalysisPackage.tempDir);
			AnalysisPackage.searchInProgress = false;
			AnalysisPackage.lastSearchedDevices = new Dictionary<ExportDatabases, string>();
		}

		public static Result Create(string packagePath, string configName, VLProject project, IList<AnalysisFileCollector.AnalysisFile> generatedFiles, DateTime compileDateTime)
		{
			if (project == null || AnalysisPackage.Parameters == null)
			{
				return Result.Error;
			}
			string projectFolder = project.GetProjectFolder();
			string glaFilePath = TempDirectoryManager.Instance.GetFullTempDirectoryPath(AnalysisPackage.tempDir) + Path.DirectorySeparatorChar + AnalysisPackage.glaFileName;
			List<Vector.VLConfig.Data.ConfigurationDataModel.IFile> list = new List<Vector.VLConfig.Data.ConfigurationDataModel.IFile>();
			foreach (IFeatureReferencedFiles current in ((IFeatureRegistration)project).FeaturesUsingReferencedFiles)
			{
				list.AddRange(current.ReferencedFiles);
			}
			List<ExportDatabase> list2 = new List<ExportDatabase>();
			ReadOnlyCollection<Database> busDatabases = AnalysisPackage.GetBusDatabases(new ReadOnlyCollection<Vector.VLConfig.Data.ConfigurationDataModel.IFile>(list), configName);
			foreach (Database current2 in busDatabases)
			{
				bool flag = true;
				foreach (AnalysisFileCollector.AnalysisFile current3 in generatedFiles)
				{
					if (current2 == current3.FlexrayDB)
					{
						flag = false;
						break;
					}
				}
				if (flag)
				{
					ExportDatabase exportDatabase = new ExportDatabase(current2);
					exportDatabase.OriginalPath = FileSystemServices.GetAbsolutePath(current2.FilePath.Value, projectFolder);
					if (AnalysisPackage.Parameters.StoreBusDatabases)
					{
						exportDatabase.RefType = ExportDatabase.ReferenceType.AnalysisRelative;
					}
					else
					{
						exportDatabase.RefType = ExportDatabase.ReferenceType.AnalysisAbsolute;
					}
					exportDatabase.Type = ExportDatabase.DBType.Bus;
					list2.Add(exportDatabase);
				}
			}
			ReadOnlyCollection<Database> nonBusDatabases = AnalysisPackage.GetNonBusDatabases(new ReadOnlyCollection<Vector.VLConfig.Data.ConfigurationDataModel.IFile>(list), configName);
			foreach (Database current4 in nonBusDatabases)
			{
				list2.Add(new ExportDatabase(current4)
				{
					OriginalPath = FileSystemServices.GetAbsolutePath(current4.FilePath.Value, projectFolder),
					RefType = ExportDatabase.ReferenceType.AnalysisRelative,
					Type = ExportDatabase.DBType.NonBus
				});
			}
			ReadOnlyCollection<Database> cCPXCPDatabases = AnalysisPackage.GetCCPXCPDatabases(new ReadOnlyCollection<Vector.VLConfig.Data.ConfigurationDataModel.IFile>(list));
			foreach (Database current5 in cCPXCPDatabases)
			{
				list2.Add(new ExportDatabase(current5)
				{
					OriginalPath = FileSystemServices.GetAbsolutePath(current5.FilePath.Value, projectFolder),
					RefType = ExportDatabase.ReferenceType.AnalysisRelative,
					Type = ExportDatabase.DBType.CCPXCP
				});
			}
			foreach (AnalysisFileCollector.AnalysisFile current6 in generatedFiles)
			{
				list2.Add(new ExportDatabase
				{
					FilePath = 
					{
						Value = current6.Path
					},
					OriginalPath = "",
					RefType = ExportDatabase.ReferenceType.AnalysisRelative,
					IsGenerated = true,
					NetworkName = 
					{
						Value = current6.Network
					},
					BusType = 
					{
						Value = current6.BusType
					},
					ChannelNumber = 
					{
						Value = current6.Channel
					},
					Type = current6.Type
				});
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (compileDateTime.Ticks > 0L)
			{
				stringBuilder.AppendFormat(Resources.FormatDateTimeForFilenames, new object[]
				{
					compileDateTime.Year,
					compileDateTime.Month,
					compileDateTime.Day,
					compileDateTime.Hour,
					compileDateTime.Minute,
					compileDateTime.Second
				});
			}
			Result result = Result.OK;
			if (list2.Count > 0)
			{
				ZipFile zipFile = null;
				result = AnalysisPackage.CreateZip(packagePath, out zipFile);
				if (result == Result.OK && zipFile != null)
				{
					if (AnalysisPackage.CreateChannelMappingFile(list2, glaFilePath, configName, stringBuilder.ToString(), zipFile))
					{
						try
						{
							zipFile.Save();
							return result;
						}
						catch
						{
							result = Result.Error;
							return result;
						}
					}
					result = Result.Error;
				}
			}
			return result;
		}

		public static bool CreateChannelMappingFile(IList<ExportDatabase> dbList, string glaFilePath, string projectName = "", string compileDateTime = "", ZipFile zip = null)
		{
			try
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null));
				XmlNode xmlNode = xmlDocument.CreateElement("AnalysisPackage");
				xmlDocument.AppendChild(xmlNode);
				XmlNode xmlNode2 = xmlDocument.CreateElement("CompileTime");
				xmlNode2.InnerText = compileDateTime;
				XmlNode xmlNode3 = xmlDocument.CreateElement("ConfigurationName");
				xmlNode3.InnerText = projectName;
				XmlNode xmlNode4 = xmlDocument.CreateElement("DatabaseList");
				xmlNode.AppendChild(xmlNode2);
				xmlNode.AppendChild(xmlNode3);
				xmlNode.AppendChild(xmlNode4);
				foreach (ExportDatabase current in dbList)
				{
					XmlNode xmlNode5 = xmlDocument.CreateElement("Database");
					xmlNode4.AppendChild(xmlNode5);
					XmlNode xmlNode6 = xmlDocument.CreateElement("ReferenceType");
					xmlNode5.AppendChild(xmlNode6);
					xmlNode6.InnerText = current.RefType.ToString();
					XmlNode xmlNode7 = xmlDocument.CreateElement("Path");
					xmlNode5.AppendChild(xmlNode7);
					if (current.RefType == ExportDatabase.ReferenceType.AnalysisRelative)
					{
						xmlNode7.InnerText = AnalysisPackage.GetDBNameAndAddDBToZip(zip, current);
						if (!string.IsNullOrEmpty(current.AnalysisPackagePath.Value) || !string.IsNullOrEmpty(current.OriginalPath))
						{
							XmlNode xmlNode8 = xmlDocument.CreateElement("OriginalPath");
							xmlNode5.AppendChild(xmlNode8);
							if (!string.IsNullOrEmpty(current.AnalysisPackagePath.Value))
							{
								xmlNode8.InnerText = current.AnalysisPackagePath.Value;
							}
							else
							{
								xmlNode8.InnerText = current.OriginalPath;
							}
						}
					}
					else if (!string.IsNullOrEmpty(current.OriginalPath))
					{
						xmlNode7.InnerText = current.OriginalPath;
					}
					else
					{
						xmlNode7.InnerText = current.FilePath.Value;
					}
					if (current.BusType.Value != BusType.Bt_None)
					{
						XmlNode xmlNode9 = xmlDocument.CreateElement("BusType");
						xmlNode5.AppendChild(xmlNode9);
						xmlNode9.InnerText = current.BusType.Value.ToString();
						XmlNode xmlNode10 = xmlDocument.CreateElement("NetworkName");
						xmlNode5.AppendChild(xmlNode10);
						xmlNode10.InnerText = current.NetworkName.Value;
						XmlNode xmlNode11 = xmlDocument.CreateElement("Channel");
						xmlNode5.AppendChild(xmlNode11);
						xmlNode11.InnerText = current.ChannelNumber.Value.ToString();
					}
					XmlNode xmlNode12 = xmlDocument.CreateElement("Type");
					xmlNode5.AppendChild(xmlNode12);
					xmlNode12.InnerText = current.Type.ToString();
				}
				xmlDocument.Save(glaFilePath);
				if (zip != null)
				{
					ZipEntry zipEntry = zip.AddFile(glaFilePath, ".");
					zipEntry.Password = null;
				}
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static IEnumerable<Database> GetConversionDatabases(FileConversionParameters fileConversionParameters, ReadOnlyCollection<Database> configurationDatabases, bool extract = false)
		{
			ExportDatabaseConfiguration exportDatabaseConfiguration = fileConversionParameters.ExportDatabaseConfiguration;
			if (exportDatabaseConfiguration.IsExportDatabaseConfigurationEnabled && exportDatabaseConfiguration.ExportDatabases.Count > 0)
			{
				if (!extract && AnalysisPackage.extractedExportDatabases.Count<Database>() == 0)
				{
					if (AnalysisPackage.notExtractedExportDatabases.Count<Database>() == 0)
					{
						foreach (ExportDatabase current in exportDatabaseConfiguration.ExportDatabases)
						{
							Database database = new Database(current);
							if (current.RefType == ExportDatabase.ReferenceType.AnalysisRelative)
							{
								database.FilePath.Value = Path.Combine(current.AnalysisPackagePath.Value, current.FilePath.Value);
							}
							AnalysisPackage.notExtractedExportDatabases.Add(database);
						}
					}
					return AnalysisPackage.notExtractedExportDatabases;
				}
				AnalysisPackage.Extract(exportDatabaseConfiguration);
				return AnalysisPackage.extractedExportDatabases;
			}
			else
			{
				if (!exportDatabaseConfiguration.IsExportDatabaseConfigurationEnabled)
				{
					return configurationDatabases;
				}
				return Enumerable.Empty<ExportDatabase>();
			}
		}

		public static Result Extract(ExportDatabaseConfiguration exportConfig)
		{
			Result result = Result.OK;
			if (AnalysisPackage.extractedExportDatabases.Count<Database>() == 0)
			{
				PasswordDialog passwordDialog = new PasswordDialog(PasswordDialog.Context.AnalysisPackage, false);
				passwordDialog.Text = Resources.AnalysisPackagePasswordDialogTitle;
				string currentPassword = string.Empty;
				if (AnalysisPackage.Parameters.ProtectWithPassword && !string.IsNullOrEmpty(AnalysisPackage.Parameters.Password))
				{
					currentPassword = AnalysisPackage.Parameters.Password;
				}
				FileProxy.PasswordHandler value = () => currentPassword;
				FileProxy.PasswordHook += value;
				foreach (ExportDatabase current in exportConfig.ExportDatabases)
				{
					if (current.RefType == ExportDatabase.ReferenceType.AnalysisRelative && current.AnalysisPackagePath.Value.ToLower().EndsWith(".zip"))
					{
						string text = Path.Combine(current.AnalysisPackagePath.Value, current.FilePath.Value);
						string text2 = Path.Combine(TempDirectoryManager.Instance.GetFullTempDirectoryPath(AnalysisPackage.tempDir), current.FilePath.Value);
						string text3 = (Path.GetExtension(text2) ?? string.Empty).ToLower();
						bool flag = true;
						bool flag2 = true;
						bool flag3 = false;
						while (flag)
						{
							try
							{
								if (!FileProxy.Exists(text))
								{
									result = Result.Error;
									break;
								}
								FileProxy.Copy(text, text2, true);
								flag = false;
								Database database = new Database(current);
								database.FilePath.Value = text2;
								AnalysisPackage.extractedExportDatabases.Add(database);
								if (text3.IndexOf("arxml") == 1)
								{
									IDictionary<string, BusType> dictionary = null;
									if (GenerationUtil.ConfigManager.Service.ApplicationDatabaseManager.IsAutosarDescriptionFile(text2, out dictionary))
									{
										database.IsAUTOSARFile = true;
									}
								}
								if (passwordDialog.RememberPassword)
								{
									AnalysisPackage.Parameters.RememberPassword(passwordDialog.Password);
								}
							}
							catch (BadPasswordException)
							{
								if (!flag2)
								{
									passwordDialog.Message = Resources.EnteredPasswordInvalid;
								}
								else
								{
									flag2 = false;
									if (!string.IsNullOrEmpty(currentPassword))
									{
										passwordDialog.Message = Resources.StoredPasswordInvalid;
									}
								}
								if (flag && DialogResult.OK == passwordDialog.ShowDialog())
								{
									currentPassword = passwordDialog.Password;
								}
								else
								{
									currentPassword = string.Empty;
									flag = false;
									flag3 = true;
									AnalysisPackage.extractedExportDatabases.Clear();
									result = Result.UserAbort;
								}
							}
							catch
							{
								result = Result.Error;
								break;
							}
						}
						if (flag3)
						{
							break;
						}
					}
					else
					{
						AnalysisPackage.extractedExportDatabases.Add(current);
					}
				}
				FileProxy.PasswordHook -= value;
			}
			return result;
		}

		public static void ExportDatabasesConfigurationChanged()
		{
			AnalysisPackage.extractedExportDatabases.Clear();
			AnalysisPackage.notExtractedExportDatabases.Clear();
		}

		public static Stream OpenChannelMappingFile(string packageFilePath)
		{
			if (AnalysisPackage.Parameters == null)
			{
				return null;
			}
			Stream result = null;
			string[] files = DirectoryProxy.GetFiles(packageFilePath, AnalysisPackage.glaFileName);
			if (files != null && files.Count<string>() > 0)
			{
				string currentPassword = string.Empty;
				if (AnalysisPackage.Parameters.ProtectWithPassword && !string.IsNullOrEmpty(AnalysisPackage.Parameters.Password))
				{
					currentPassword = AnalysisPackage.Parameters.Password;
				}
				bool flag = true;
				bool flag2 = true;
				PasswordDialog passwordDialog = new PasswordDialog(PasswordDialog.Context.AnalysisPackage, false);
				passwordDialog.Text = Resources.AnalysisPackagePasswordDialogTitle;
				FileProxy.PasswordHandler value = () => currentPassword;
				FileProxy.PasswordHook += value;
				while (flag)
				{
					try
					{
						result = FileProxy.OpenRead(files[0]);
						flag = false;
						if (passwordDialog.RememberPassword)
						{
							AnalysisPackage.Parameters.RememberPassword(passwordDialog.Password);
						}
					}
					catch (BadPasswordException)
					{
						if (!flag2)
						{
							passwordDialog.Message = Resources.EnteredPasswordInvalid;
						}
						else
						{
							flag2 = false;
							if (!string.IsNullOrEmpty(currentPassword))
							{
								passwordDialog.Message = (passwordDialog.Message = Resources.StoredPasswordInvalid);
							}
						}
						if (flag && DialogResult.OK == passwordDialog.ShowDialog())
						{
							currentPassword = passwordDialog.Password;
						}
						else
						{
							flag = false;
						}
					}
				}
				FileProxy.PasswordHook -= value;
			}
			return result;
		}

		public static IList<string> GetMatchingPackages(ILoggerDevice loggerDevice)
		{
			IList<string> list = null;
			if (loggerDevice != null)
			{
				DateTime compileDateTime = loggerDevice.CompileDateTime;
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(loggerDevice.Name);
				string compileDateTime2 = string.Format(Resources.FormatDateTimeForFilenames, new object[]
				{
					compileDateTime.Year,
					compileDateTime.Month,
					compileDateTime.Day,
					compileDateTime.Hour,
					compileDateTime.Minute,
					compileDateTime.Second
				});
				string text = loggerDevice.GetAnalysisPackagePath();
				if (loggerDevice.LoggerType == LoggerType.GL1000 && !string.IsNullOrEmpty(text))
				{
					string fullTempDirectoryPath = TempDirectoryManager.Instance.GetFullTempDirectoryPath(AnalysisPackage.tempDir);
					text = Path.Combine(fullTempDirectoryPath, text);
					if (File.Exists(text))
					{
						File.Delete(text);
					}
					loggerDevice.CopyAnalysisPackage(fullTempDirectoryPath);
				}
				if (AnalysisPackage.CheckPackage(fileNameWithoutExtension, compileDateTime2, text))
				{
					list = new List<string>();
					list.Add(text);
				}
				else if (AnalysisPackage.IsAnalysisPoolPathConfigured())
				{
					AnalysisPackage.AutomaticSearchProgressIndicator automaticSearchProgressIndicator = new AnalysisPackage.AutomaticSearchProgressIndicator();
					list = automaticSearchProgressIndicator.GetMatchingPackages(loggerDevice);
				}
			}
			return list;
		}

		public static string GetMatchingPackage(string configName, DateTime dt, string searchPath)
		{
			IEnumerable<string> accessibleFiles = FileSystemServices.GetAccessibleFiles(searchPath, "*analysis.zip", SearchOption.AllDirectories);
			string compileDateTime = string.Format(Resources.FormatDateTimeForFilenames, new object[]
			{
				dt.Year,
				dt.Month,
				dt.Day,
				dt.Hour,
				dt.Minute,
				dt.Second
			});
			foreach (string current in accessibleFiles)
			{
				if (AnalysisPackage.CheckPackage(configName, compileDateTime, current))
				{
					return current;
				}
			}
			return string.Empty;
		}

		public static bool MatchesDeviceConfiguration(ILoggerDevice loggerDevice, string analysisPackage)
		{
			bool result = false;
			if (loggerDevice != null && !string.IsNullOrEmpty(analysisPackage))
			{
				DateTime compileDateTime = loggerDevice.CompileDateTime;
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(loggerDevice.Name);
				string compileDateTime2 = string.Format(Resources.FormatDateTimeForFilenames, new object[]
				{
					compileDateTime.Year,
					compileDateTime.Month,
					compileDateTime.Day,
					compileDateTime.Hour,
					compileDateTime.Minute,
					compileDateTime.Second
				});
				result = AnalysisPackage.CheckPackage(fileNameWithoutExtension, compileDateTime2, analysisPackage);
			}
			return result;
		}

		public static bool SearchAndLoadAnalysisPackage(ILoggerDevice loggerDevice, ExportDatabases exportDatabases)
		{
			bool flag = true;
			if (AnalysisPackage.searchInProgress || exportDatabases == null || exportDatabases.FileConversionParameters == null)
			{
				return true;
			}
			AnalysisPackage.searchInProgress = true;
			if (loggerDevice == null || (loggerDevice.LogFileStorage.NumberOfTriggeredBuffers == 0u && loggerDevice.LogFileStorage.NumberOfRecordingBuffers == 0u) || (!FileConversionHelper.IsSignalOrientedDestFormat(exportDatabases.FileConversionParameters.DestinationFormat) && !loggerDevice.LoggerSpecifics.FileConversion.IsQuickViewSupported))
			{
				exportDatabases.ClearAutomaticAnalysisPackage();
				AnalysisPackage.searchInProgress = false;
				return true;
			}
			if (!AnalysisPackage.lastSearchedDevices.ContainsKey(exportDatabases))
			{
				AnalysisPackage.lastSearchedDevices[exportDatabases] = "";
			}
			if (loggerDevice.HardwareKey != AnalysisPackage.lastSearchedDevices[exportDatabases])
			{
				exportDatabases.ClearAutomaticAnalysisPackage();
			}
			AnalysisPackage.lastSearchedDevices[exportDatabases] = loggerDevice.HardwareKey;
			if (exportDatabases.FileConversionParameters.ExportDatabaseConfiguration.Mode == ExportDatabaseConfiguration.DBSelectionMode.FromAnalysisPackage)
			{
				bool flag2 = AnalysisPackage.MatchesDeviceConfiguration(loggerDevice, exportDatabases.GetAnalysisPackagePath());
				if (!flag2)
				{
					IList<string> matchingPackages = AnalysisPackage.GetMatchingPackages(loggerDevice);
					string text = string.Empty;
					if (matchingPackages != null)
					{
						if (matchingPackages.Count > 1)
						{
							FileChooserDialog fileChooserDialog = new FileChooserDialog();
							fileChooserDialog.AddItems(matchingPackages);
							fileChooserDialog.Text = Resources.AutomaticSearchResultDialogTitle;
							fileChooserDialog.Message = Resources.AutomaticSearchResultMsg;
							if (fileChooserDialog.ShowDialog() == DialogResult.OK)
							{
								text = fileChooserDialog.SelectedItem;
							}
						}
						else if (matchingPackages.Count == 1)
						{
							text = matchingPackages[0];
						}
					}
					if (!string.IsNullOrEmpty(text))
					{
						exportDatabases.LoadAnalysisPackageAutomatically(text);
						flag2 = true;
					}
				}
				flag = flag2;
				if (!flag)
				{
					exportDatabases.ClearAutomaticAnalysisPackage();
				}
			}
			AnalysisPackage.searchInProgress = false;
			return flag;
		}

		public static bool IsAnalysisPoolPathConfigured()
		{
			return AnalysisPackage.Parameters != null && !string.IsNullOrEmpty(AnalysisPackage.Parameters.PoolPath);
		}

		public static bool IsPasswordProtected(string packageFilePath)
		{
			bool result = false;
			try
			{
				if (File.Exists(packageFilePath) && ZipFile.IsZipFile(packageFilePath))
				{
					using (ZipFile zipFile = ZipFile.Read(packageFilePath))
					{
						foreach (ZipEntry current in zipFile)
						{
							if (current.UsesEncryption)
							{
								result = true;
								break;
							}
						}
						goto IL_ED;
					}
				}
				if (packageFilePath.EndsWith(Vocabulary.FileExtensionDotAnalysisPackageZip, StringComparison.OrdinalIgnoreCase))
				{
					Stream zipStream = FileProxy.OpenRead(packageFilePath);
					using (ZipFile zipFile2 = ZipFile.Read(zipStream))
					{
						foreach (ZipEntry current2 in zipFile2)
						{
							if (current2.UsesEncryption)
							{
								result = true;
								break;
							}
						}
					}
				}
				IL_ED:;
			}
			catch
			{
			}
			return result;
		}

		private static bool CheckPackage(string configName, string compileDateTime, string analysisPackage)
		{
			if (string.IsNullOrEmpty(configName) || string.IsNullOrEmpty(compileDateTime) || string.IsNullOrEmpty(analysisPackage))
			{
				return false;
			}
			string fileName = Path.GetFileName(analysisPackage);
			return !string.IsNullOrEmpty(fileName) && (fileName.Contains(configName) && fileName.Contains(compileDateTime)) && AnalysisPackage.CheckPackage(AnalysisPackage.OpenChannelMappingFile(analysisPackage), configName, compileDateTime);
		}

		private static bool CheckPackage(Stream readStream, string configName, string compileDateTime)
		{
			bool result = false;
			if (readStream != null && !string.IsNullOrEmpty(configName) && !string.IsNullOrEmpty(compileDateTime))
			{
				XmlDocument xmlDocument = new XmlDocument();
				try
				{
					xmlDocument.Load(readStream);
					readStream.Close();
					if (xmlDocument.GetElementsByTagName("AnalysisPackage").Count != 1)
					{
						bool result2 = false;
						return result2;
					}
					XmlNodeList elementsByTagName = xmlDocument.GetElementsByTagName("CompileTime");
					if (elementsByTagName.Count != 1)
					{
						bool result2 = false;
						return result2;
					}
					XmlNodeList elementsByTagName2 = xmlDocument.GetElementsByTagName("ConfigurationName");
					if (elementsByTagName2.Count != 1)
					{
						bool result2 = false;
						return result2;
					}
					result = (Path.GetFileNameWithoutExtension(elementsByTagName2[0].InnerText) == configName && elementsByTagName[0].InnerText == compileDateTime);
				}
				catch (Exception)
				{
					readStream.Close();
					bool result2 = false;
					return result2;
				}
				return result;
			}
			return result;
		}

		private static string GetDBNameAndAddDBToZip(ZipFile zip, ExportDatabase db)
		{
			string text = Path.GetFileName(db.FilePath.Value);
			if (zip != null)
			{
				string text2;
				if (!string.IsNullOrEmpty(db.OriginalPath))
				{
					text2 = db.OriginalPath;
				}
				else
				{
					text2 = db.FilePath.Value;
				}
				if (zip.ContainsEntry(text))
				{
					ZipEntry zipEntry = zip[text];
					FileStream fileStream = zipEntry.InputStream as FileStream;
					if (fileStream != null && fileStream.Name == text2)
					{
						return text;
					}
				}
				int num = 1;
				string extension = Path.GetExtension(text);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(text);
				while (zip.ContainsEntry(text))
				{
					num++;
					text = fileNameWithoutExtension + "_" + num.ToString() + extension;
				}
				zip.AddEntry(text, new FileStream(text2, FileMode.Open, FileAccess.Read));
			}
			return text;
		}

		private static Result CreateZip(string zipPath, out ZipFile zip)
		{
			zip = new ZipFile(zipPath);
			try
			{
				if (AnalysisPackage.Parameters.ProtectWithPassword)
				{
					if (AnalysisPackage.Parameters.SetPasswordOnDemand)
					{
						PasswordDialog passwordDialog = new PasswordDialog(PasswordDialog.Context.AnalysisPackage, true);
						if (DialogResult.OK != passwordDialog.ShowDialog())
						{
							Result result = Result.UserAbort;
							return result;
						}
						zip.Password = passwordDialog.Password;
						if (passwordDialog.RememberPassword)
						{
							AnalysisPackage.Parameters.RememberPassword(passwordDialog.Password);
						}
					}
					else if (!string.IsNullOrEmpty(AnalysisPackage.Parameters.Password))
					{
						zip.Password = AnalysisPackage.Parameters.Password;
					}
				}
				zip.AlternateEncoding = Encoding.UTF8;
				zip.AlternateEncodingUsage = ZipOption.Always;
			}
			catch
			{
				Result result = Result.Error;
				return result;
			}
			return Result.OK;
		}

		private static ReadOnlyCollection<Database> GetBusDatabases(ReadOnlyCollection<Vector.VLConfig.Data.ConfigurationDataModel.IFile> fileCollection, string projectName)
		{
			List<Database> list = new List<Database>();
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.IFile current in fileCollection)
			{
				Database database = current as Database;
				if (database != null && database.IsBusDatabase && !AnalysisPackage.IsNonBusDb(database, projectName))
				{
					list.Add(database);
				}
			}
			return new ReadOnlyCollection<Database>(list);
		}

		private static ReadOnlyCollection<Database> GetNonBusDatabases(ReadOnlyCollection<Vector.VLConfig.Data.ConfigurationDataModel.IFile> fileCollection, string projectName)
		{
			List<Database> list = new List<Database>();
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.IFile current in fileCollection)
			{
				Database database = current as Database;
				if (database != null && AnalysisPackage.IsNonBusDb(database, projectName))
				{
					list.Add(database);
				}
				GPSConfiguration gPSConfiguration = current as GPSConfiguration;
				if (gPSConfiguration != null && !string.IsNullOrEmpty(gPSConfiguration.Database.Value) && gPSConfiguration.MapToSystemChannel.Value)
				{
					list.Add(new Database
					{
						FilePath = gPSConfiguration.Database,
						ChannelNumber = gPSConfiguration.CANChannel,
						BusType = 
						{
							Value = BusType.Bt_CAN
						}
					});
				}
			}
			return new ReadOnlyCollection<Database>(list);
		}

		private static ReadOnlyCollection<Database> GetCCPXCPDatabases(ReadOnlyCollection<Vector.VLConfig.Data.ConfigurationDataModel.IFile> fileCollection)
		{
			List<Database> list = new List<Database>();
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.IFile current in fileCollection)
			{
				Database database = current as Database;
				bool flag = (Path.GetExtension(current.FilePath.Value) ?? string.Empty).ToLower() == Vocabulary.FileExtensionDotA2L;
				if (database != null && AnalysisPackage.IsCCPXCP(database) && !flag)
				{
					list.Add(database);
				}
			}
			return new ReadOnlyCollection<Database>(list);
		}

		private static bool IsNonBusDb(Database db, string projectName)
		{
			if (db != null && db.IsBusDatabase)
			{
				string input = Path.GetFileName(db.FilePath.Value) ?? string.Empty;
				string pattern = string.Format("{0}_((CAN \\d+)|GPS_CAN\\d+).dbc", projectName);
				Match match = Regex.Match(input, pattern);
				return match.Success;
			}
			return false;
		}

		private static bool IsCCPXCP(Database db)
		{
			return db != null && db.CPType.Value != CPType.None;
		}
	}
}
