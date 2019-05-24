using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LTLGenerationCS;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public static class GenerationUtil
	{
		private class TmpModelValidationResultCollector : IModelValidationResultCollector
		{
			private List<string> mErrorTextList = new List<string>();

			public void ResetAllModelErrors()
			{
				this.mErrorTextList.Clear();
			}

			public void ResetAllErrorsOfClass(ValidationErrorClass validationErrorClass)
			{
				throw new NotImplementedException();
			}

			public void SetErrorText(ValidationErrorClass validationErrorClass, IValidatedProperty validatedModelElementModel, string errorText)
			{
				this.mErrorTextList.Add(errorText);
			}

			public string GetErrorText(ValidationErrorClass validationErrorClass, IValidatedProperty validatedModelElementModel)
			{
				throw new NotImplementedException();
			}
		}

		private class VSysVar
		{
			public string Name;

			public string DataType;

			public string Comment;

			public int BitCount;

			public VSysVar(string name, string dataType, int bitCount)
			{
				this.Name = GenerationUtil.MakeCaplCompliantSysVarName(name);
				this.DataType = dataType;
				this.BitCount = bitCount;
				this.Comment = "";
			}
		}

		public static ConfigurationManager ConfigManager
		{
			get;
			set;
		}

		internal static AnalysisFileCollector AnalysisFileCollector
		{
			get;
			set;
		}

		internal static AppDataAccess AppDataAccess
		{
			get;
			set;
		}

		internal static CcpXcpGenerationInfo CcpXcpGenerationInfo
		{
			get;
			set;
		}

		public static string ProjectFileName
		{
			get
			{
				return GenerationUtil.ConfigManager.VLProject.GetProjectFileName();
			}
		}

		static GenerationUtil()
		{
		}

		public static Result ExportToLTL(string ltlFile, out string errorText)
		{
			string b;
			if (string.IsNullOrEmpty(GenerationUtil.ConfigManager.VLProject.FilePath))
			{
				b = string.Empty;
			}
			else
			{
				try
				{
					b = Path.GetDirectoryName(GenerationUtil.ConfigManager.VLProject.FilePath);
				}
				catch
				{
					b = string.Empty;
				}
			}
			bool generateDisconnectCode = GenerationUtil.ConfigManager.VLProject.HasStopCyclicCommunicationEventConfigured();
			Cursor.Current = Cursors.WaitCursor;
			Result result = GenerationUtil.ConvertCcpXcpDatabases(Path.GetDirectoryName(ltlFile), generateDisconnectCode, out errorText);
			Cursor.Current = Cursors.Default;
			if (result != Result.OK)
			{
				if (errorText.Length <= 0)
				{
					errorText = Resources.ErrorCannotCreateTemporaryFile;
				}
				return result;
			}
			List<Event> list = new List<Event>();
			foreach (IFeatureEvents current in VLProject.FeatureRegistration.FeaturesUsingEvents)
			{
				list.AddRange(current.GetEvents(true));
			}
			if (!GenerationUtil.ConfigManager.Service.ApplicationDatabaseManager.SetRuntimeInformationInCcpXcpEvents(list, GenerationUtil.ConfigManager.Service.DatabaseConfiguration))
			{
				errorText = Resources_CcpXcp.CcpXcpErrorTrigger;
				return Result.Error;
			}
			if (Path.GetDirectoryName(ltlFile) != b)
			{
				bool flag = false;
				ReadOnlyCollection<IncludeFile> includeFiles = GenerationUtil.ConfigManager.VLProject.ProjectRoot.LoggingConfiguration.IncludeFileConfiguration.IncludeFiles;
				List<string> list2 = (from file in includeFiles
				select file.FilePath.Value).Distinct<string>().ToList<string>();
				foreach (string current2 in list2)
				{
					if (!FileSystemServices.IsAbsolutePath(current2))
					{
						string absolutePath = FileSystemServices.GetAbsolutePath(current2, Path.GetDirectoryName(GenerationUtil.ConfigManager.VLProject.FilePath));
						string text = Path.GetDirectoryName(current2) ?? string.Empty;
						string path = text.TrimStart(new char[]
						{
							Path.DirectorySeparatorChar
						});
						string text2 = Path.Combine(Path.GetDirectoryName(ltlFile) ?? string.Empty, path);
						string text3 = Path.Combine(text2, Path.GetFileName(current2) ?? string.Empty);
						if (File.Exists(text3) && !flag)
						{
							if (InformMessageBox.Question(string.Format(Resources.OverwriteExistingIncFiles, new object[0])) != DialogResult.Yes)
							{
								errorText = "";
								Result result2 = Result.UserAbort;
								return result2;
							}
							flag = true;
						}
						if (!Directory.Exists(text2))
						{
							try
							{
								Directory.CreateDirectory(text2);
							}
							catch
							{
								errorText = string.Format(Resources.ErrorCannotCreateDir, text2);
								Result result2 = Result.Error;
								return result2;
							}
						}
						if (!GenerationUtil.TryCopyFile(absolutePath, text3, true))
						{
							errorText = string.Format(Resources.ErrorCannotCopyFileXtoY, current2, GenerationUtil.ConfigManager.VLProject.FilePath);
							Result result2 = Result.Error;
							return result2;
						}
					}
				}
			}
			LTLGenerator lTLGenerator = new LTLGenerator(GenerationUtil.ConfigManager.VLProject, GenerationUtil.ConfigManager.Service.LoggerSpecifics, GenerationUtil.ConfigManager.Service.ApplicationDatabaseManager, GenerationUtil.ConfigManager.Service.DiagSymbolsManager, GenerationUtil.CcpXcpGenerationInfo, GenerationUtil.ConfigManager.Service.ConfigFolderPath);
			LTLGenerator.LTLGenerationResult lTLGenerationResult = lTLGenerator.GenerateLTL(ltlFile, out errorText);
			if (lTLGenerationResult != LTLGenerator.LTLGenerationResult.OK)
			{
				return Result.Error;
			}
			return Result.OK;
		}

		public static Result ExportToLtlOrCodFile(string codOrLtlFilePath, out string errorText, out string compilerErrFilePath, out bool bLtlDidNotCompile)
		{
			bLtlDidNotCompile = false;
			compilerErrFilePath = string.Empty;
			bool flag = false;
			string extension = Path.GetExtension(codOrLtlFilePath);
			string path;
			if (!string.IsNullOrEmpty(extension) && extension.Equals(Vocabulary.FileExtensionDotLTL, StringComparison.OrdinalIgnoreCase))
			{
				flag = true;
				path = Path.ChangeExtension(Path.GetFileName(codOrLtlFilePath), "cod");
			}
			else
			{
				path = codOrLtlFilePath;
			}
			string tempDirectoryName;
			if (!TempDirectoryManager.Instance.CreateNewTempDirectory(out tempDirectoryName))
			{
				errorText = Resources.ErrorCannotCreateTemporaryFile;
				return Result.Error;
			}
			string text;
			if (!TempDirectoryManager.Instance.GetNewTempFilename(tempDirectoryName, Path.GetFileName(path), out text))
			{
				errorText = Resources.ErrorCannotCreateTemporaryFile;
				return Result.Error;
			}
			string text2;
			if (!TempDirectoryManager.Instance.GetNewTempFilename(tempDirectoryName, Path.GetFileName(Path.ChangeExtension(text, "ltl")), out text2))
			{
				errorText = Resources.ErrorCannotCreateTemporaryFile;
				return Result.Error;
			}
			Result result;
			if ((result = GenerationUtil.ExportToLTL(text2, out errorText)) != Result.OK)
			{
				return result;
			}
			bool includeWebServerDisplayZip = false;
			if (GenerationUtil.ConfigManager.Service.LoggerSpecifics.DataTransfer.HasWebServer && GenerationUtil.ConfigManager.Service.InterfaceModeConfiguration.UseSignalExport.Value && GenerationUtil.ConfigManager.Service.InterfaceModeConfiguration.UseCustomWebDisplay.Value)
			{
				string text3 = GenerationUtil.ConfigManager.Service.InterfaceModeConfiguration.CustomWebDisplay.FilePath.Value;
				text3 = FileSystemServices.GetAbsolutePath(text3, GenerationUtil.ConfigManager.Service.ConfigFolderPath);
				text3 = Path.GetDirectoryName(text3);
				if (!Directory.Exists(text3))
				{
					return Result.Error;
				}
				string fileName = Path.Combine(Path.GetDirectoryName(text2), "webserver.zip");
				ZipFile zipFile = new ZipFile();
				zipFile.AddDirectory(text3);
				zipFile.Save(fileName);
				includeWebServerDisplayZip = true;
			}
			string text4 = Path.ChangeExtension(codOrLtlFilePath, "err");
			bool value = GenerationUtil.ConfigManager.Service.SpecialFeaturesConfiguration.IsIncludeLTLCodeEnabled.Value;
			if (!GenerationUtil.CompileLTLCode(text2, value, GenerationUtil.AnalysisFileCollector != null, includeWebServerDisplayZip, out errorText, text4))
			{
				bLtlDidNotCompile = true;
				compilerErrFilePath = text4;
				if (!flag)
				{
					return Result.Error;
				}
				result = Result.Error;
			}
			if (!bLtlDidNotCompile && GenerationUtil.AnalysisFileCollector != null)
			{
				List<string> list;
				if (GenerationUtil.TryMoveGeneratedDBCFilesFromDirectory(Path.GetDirectoryName(text2), GenerationUtil.AnalysisFileCollector.GeneralPurposeTempDirectory, out list, out errorText) != Result.OK)
				{
					InformMessageBox.Error(errorText);
					return Result.Error;
				}
				foreach (string current in list)
				{
					uint channel = 0u;
					Match match = Regex.Match(Path.GetFileNameWithoutExtension(current), "CAN[ ]*\\d+$");
					if (match.Success && !uint.TryParse(match.ToString().Remove(0, 3), out channel))
					{
						channel = 0u;
					}
					GenerationUtil.AnalysisFileCollector.AddFile(current, "", BusType.Bt_CAN, channel, ExportDatabase.DBType.NonBus, null);
				}
			}
			if (File.Exists(codOrLtlFilePath) && !GenerationUtil.TryDeleteFile(codOrLtlFilePath, out errorText))
			{
				return Result.Error;
			}
			if (flag)
			{
				if (!GenerationUtil.TryMoveFile(text2, codOrLtlFilePath, out errorText))
				{
					return Result.Error;
				}
				string directoryName = Path.GetDirectoryName(text2);
				if (!string.IsNullOrEmpty(directoryName))
				{
					string[] files;
					try
					{
						files = Directory.GetFiles(directoryName, "*.inc", SearchOption.AllDirectories);
					}
					catch (Exception)
					{
						errorText = Resources.ErrorUnauthorizedAccess;
						Result result2 = Result.Error;
						return result2;
					}
					string directoryName2 = Path.GetDirectoryName(codOrLtlFilePath);
					List<string> list2 = new List<string>();
					string[] array = files;
					for (int i = 0; i < array.Length; i++)
					{
						string text5 = array[i];
						string text6 = text5.Substring(directoryName.Length).TrimStart(new char[]
						{
							Path.DirectorySeparatorChar
						});
						string strA = Path.Combine(directoryName2, text6);
						bool flag2 = false;
						foreach (IncludeFile current2 in GenerationUtil.ConfigManager.Service.IncludeFileConfiguration.IncludeFiles)
						{
							string absoluteFilePath = GenerationUtil.ConfigManager.Service.GetAbsoluteFilePath(current2.FilePath.Value);
							if (string.Compare(strA, absoluteFilePath, true) == 0)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							list2.Add(text6);
						}
					}
					List<string> list3 = new List<string>();
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendFormat(Resources.ErrorDirAlreadyContainsFollowingFiles, directoryName2);
					stringBuilder.Append("\n\n");
					foreach (string current3 in list2)
					{
						string text7 = Path.Combine(directoryName2, current3);
						if (File.Exists(text7))
						{
							list3.Add(text7);
							stringBuilder.AppendLine(current3);
						}
					}
					stringBuilder.AppendLine(string.Empty);
					stringBuilder.AppendLine(Resources.QuestionContinueAndReplaceFiles);
					if (list3.Count > 0 && DialogResult.Yes != InformMessageBox.Question(stringBuilder.ToString()))
					{
						return Result.UserAbort;
					}
					using (List<string>.Enumerator enumerator4 = list2.GetEnumerator())
					{
						while (enumerator4.MoveNext())
						{
							string current4 = enumerator4.Current;
							if (!FileSystemServices.ReplicateFileWithRelativePathInFolder(directoryName, current4, directoryName2, true))
							{
								errorText = string.Format(Resources.ErrorFailedWriteDestFile, current4);
								Result result2 = Result.Error;
								return result2;
							}
						}
						return result;
					}
					goto IL_53C;
				}
				return result;
			}
			IL_53C:
			if (!GenerationUtil.TryMoveFile(text, codOrLtlFilePath, out errorText))
			{
				return Result.Error;
			}
			return result;
		}

		public static string ComputeMD5Hash(string fileName)
		{
			string result = "";
			try
			{
				if (File.Exists(fileName))
				{
					MD5 mD = MD5.Create();
					using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
					{
						if (fileStream != null && fileStream.CanRead)
						{
							result = BitConverter.ToString(mD.ComputeHash(fileStream)).ToLower().Replace("-", "");
						}
					}
				}
			}
			catch
			{
			}
			return result;
		}

		private static bool CompileLTLCode(string ltlFile, bool includeLTLSourceCode, bool generateDBCFilesForChannels, bool includeWebServerDisplayZip, out string errorText, string compileErrorFile)
		{
			LTLCompiler lTLCompiler = new LTLCompiler(GenerationUtil.ConfigManager.Service.LoggerSpecifics.Type);
			return lTLCompiler.CompileLTL(ltlFile, includeLTLSourceCode, generateDBCFilesForChannels, includeWebServerDisplayZip, out errorText, compileErrorFile);
		}

		internal static Result GenerateDBCFilesForAnalogInputsAndDateTime(string destinationFolderPath, out string errorText)
		{
			if (string.IsNullOrEmpty(destinationFolderPath))
			{
				errorText = Resources.ErrorNoDirProvided;
				return Result.Error;
			}
			DirectoryInfo directoryInfo = new DirectoryInfo(destinationFolderPath);
			if (!directoryInfo.Exists)
			{
				errorText = Resources.ErrorDirDoesntExist;
				return Result.Error;
			}
			string tempDirectoryName;
			if (!TempDirectoryManager.Instance.CreateNewTempDirectory(out tempDirectoryName))
			{
				errorText = Resources.ErrorCannotCreateTemporaryFile;
				return Result.Error;
			}
			string filename = string.Format(Resources.DefaultFileNameFormat, GenerationUtil.ConfigManager.Service.LoggerSpecifics.Name, "ltl");
			if (!string.IsNullOrEmpty(GenerationUtil.ConfigManager.VLProject.GetProjectFileName()))
			{
				filename = Path.ChangeExtension(GenerationUtil.ConfigManager.VLProject.GetProjectFileName(), "ltl");
			}
			string text;
			if (!TempDirectoryManager.Instance.GetNewTempFilename(tempDirectoryName, filename, out text))
			{
				errorText = Resources.ErrorCannotCreateTemporaryFile;
				return Result.Error;
			}
			if (GenerationUtil.ExportToLTL(text, out errorText) != Result.OK)
			{
				return Result.Error;
			}
			if (!GenerationUtil.CompileLTLCode(text, false, true, false, out errorText, Path.ChangeExtension(text, "err")))
			{
				return Result.Error;
			}
			List<string> list;
			if (GenerationUtil.TryMoveGeneratedDBCFilesFromDirectory(Path.GetDirectoryName(text), destinationFolderPath, out list, out errorText) != Result.OK)
			{
				InformMessageBox.Error(errorText);
				return Result.Error;
			}
			if (GenerationUtil.AnalysisFileCollector != null)
			{
				foreach (string current in list)
				{
					uint channel = 0u;
					Match match = Regex.Match(Path.GetFileNameWithoutExtension(current), "CAN[ ]*\\d+$");
					if (match.Success && !uint.TryParse(match.ToString().Remove(0, 3), out channel))
					{
						channel = 0u;
					}
					GenerationUtil.AnalysisFileCollector.AddFile(current, "", BusType.Bt_CAN, channel, ExportDatabase.DBType.NonBus, null);
				}
			}
			return Result.OK;
		}

		internal static Result GenerateDBCFileForGPSConfiguration(string destinationFolderPath, AppDataAccess appDataAccess, out string errorText)
		{
			errorText = string.Empty;
			if (!GenerationUtil.ConfigManager.Service.GPSConfiguration.MapToCANMessage.Value)
			{
				return Result.OK;
			}
			string text = GenerationUtil.ConfigManager.VLProject.GetProjectFileName();
			if (string.IsNullOrEmpty(text))
			{
				text = string.Format(Resources.DefaultFileNameFormat, GenerationUtil.ConfigManager.VLProject.ProjectRoot.LoggerType.ToString(), "glc");
			}
			text = Path.GetFileNameWithoutExtension(text);
			string path = string.Format(Vocabulary.FileNameGpsDatabase, text, GenerationUtil.ConfigManager.Service.GPSConfiguration.CANChannel.Value);
			string text2 = Path.Combine(destinationFolderPath, path);
			string path2;
			if (appDataAccess.AppDataRoot.GlobalOptions.GenerateCANapeDBC)
			{
				path2 = Resources.GPSCANapeDatabaseTemplateFileName;
			}
			else
			{
				path2 = Resources.GPSDatabaseTemplateFileName;
			}
			if (!Directory.Exists(Application.StartupPath))
			{
				errorText = Resources.ErrorUnableToReadDBTemplForGPS;
				return Result.Error;
			}
			string path3 = Path.Combine(Application.StartupPath, path2);
			if (!File.Exists(path3))
			{
				errorText = Resources.ErrorUnableToReadDBTemplForGPS;
				return Result.Error;
			}
			Dictionary<string, uint> dictionary = new Dictionary<string, uint>();
			uint num = GenerationUtil.ConfigManager.Service.GPSConfiguration.StartCANId.Value;
			if (GenerationUtil.ConfigManager.Service.GPSConfiguration.IsExtendedStartCANId.Value)
			{
				num |= Constants.CANDbIsExtendedIdMask;
			}
			dictionary["Msg1:"] = num;
			dictionary["Msg2:"] = num + 1u;
			dictionary["Msg3:"] = num + 2u;
			dictionary["Msg4a:"] = num + 3u;
			dictionary["Msg4b:"] = num + 4u;
			dictionary["Msg5:"] = num + 5u;
			StringBuilder stringBuilder = new StringBuilder();
			try
			{
				using (StreamReader streamReader = new StreamReader(path3))
				{
					string text3;
					while ((text3 = streamReader.ReadLine()) != null)
					{
						if (text3.IndexOf("BO_") == 0)
						{
							string[] array = text3.Split(new char[]
							{
								' '
							});
							if (dictionary.ContainsKey(array[2]))
							{
								stringBuilder.Append(array[0]);
								stringBuilder.Append(string.Format(" {0:D}", dictionary[array[2]]));
								for (int i = 2; i < array.Count<string>(); i++)
								{
									stringBuilder.Append(" " + array[i]);
								}
								stringBuilder.AppendLine();
							}
							else
							{
								stringBuilder.AppendLine(text3);
							}
						}
						else
						{
							stringBuilder.AppendLine(text3);
						}
					}
				}
			}
			catch (Exception)
			{
				errorText = Resources.ErrorUnableToReadDBTemplForGPS;
				Result result = Result.Error;
				return result;
			}
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(text2))
				{
					streamWriter.Write(stringBuilder.ToString());
				}
			}
			catch (Exception)
			{
				errorText = Resources.ErrorUnableToWriteDBForGPS;
				Result result = Result.Error;
				return result;
			}
			GenerationUtil.AnalysisFileCollector.AddFile(text2, "", BusType.Bt_CAN, GenerationUtil.ConfigManager.Service.GPSConfiguration.CANChannel.Value, ExportDatabase.DBType.NonBus, null);
			return Result.OK;
		}

		internal static Result GenerateFilesForCcpXcp(string destinationFolderPath, out string errorText)
		{
			GenerationUtil.CcpXcpGenerationInfo = null;
			errorText = string.Empty;
			if (string.IsNullOrEmpty(destinationFolderPath))
			{
				errorText = Resources.ErrorNoDirProvided;
				return Result.Error;
			}
			if (GenerationUtil.AnalysisFileCollector != null && !GenerationUtil.AnalysisFileCollector.SoundnessCheckPerformed)
			{
				GenerationUtil.TmpModelValidationResultCollector resultCollector = new GenerationUtil.TmpModelValidationResultCollector();
				if (!GenerationUtil.ConfigManager.ModelValidator.Validate(GenerationUtil.ConfigManager.Service.DatabaseConfiguration, PageType.CcpXcpDescriptions, false, resultCollector) || !GenerationUtil.ConfigManager.ModelValidator.Validate(GenerationUtil.ConfigManager.VLProject.ProjectRoot.LoggingConfiguration.CcpXcpSignalConfiguration, false, resultCollector))
				{
					errorText = Resources_CcpXcp.CcpXcpErrorCannotGenerateDatabases;
					return Result.Error;
				}
			}
			GenerationUtil.CcpXcpGenerationInfo = new CcpXcpGenerationInfo();
			foreach (Database current in GenerationUtil.ConfigManager.Service.DatabaseConfiguration.ActiveCCPXCPDatabases)
			{
				if (current.FileType == DatabaseFileType.A2L)
				{
					A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(current);
					if (a2LDatabase != null && current.CcpXcpEcuList.Any<CcpXcpEcu>())
					{
						current.ExtraCPChannel = 0u;
						CcpXcpEcu ccpXcpEcu = current.CcpXcpEcuList.First<CcpXcpEcu>();
						ccpXcpEcu.GeneratedDbcOrFibexFile = null;
						ccpXcpEcu.NetworkName = null;
						CcpXcpDatabaseInfo ccpXcpDatabaseInfo = null;
						BusType value = current.BusType.Value;
						if (value == BusType.Bt_FlexRay || value == BusType.Bt_Ethernet)
						{
							uint value2 = current.ChannelNumber.Value;
							foreach (CcpXcpDatabaseInfo current2 in GenerationUtil.CcpXcpGenerationInfo.DatabaseInfos)
							{
								foreach (CcpXcpEcuInfo current3 in current2.EcuInfoList)
								{
									Database database = current3.Database;
									if (database.BusType.Value == value && database.ChannelNumber.Value == value2)
									{
										ccpXcpDatabaseInfo = current2;
									}
								}
							}
						}
						if (ccpXcpDatabaseInfo == null)
						{
							ccpXcpDatabaseInfo = new CcpXcpDatabaseInfo(current, destinationFolderPath);
							GenerationUtil.CcpXcpGenerationInfo.DatabaseInfos.Add(ccpXcpDatabaseInfo);
						}
						CcpXcpEcuInfo item = new CcpXcpEcuInfo(current, ccpXcpEcu);
						ccpXcpDatabaseInfo.EcuInfoList.Add(item);
					}
				}
			}
			bool flag = true;
			List<CcpXcpEcu> list = new List<CcpXcpEcu>();
			foreach (CcpXcpDatabaseInfo current4 in GenerationUtil.CcpXcpGenerationInfo.DatabaseInfos)
			{
				Result result = A2LDatabase.GenerateDbcOrFibexDatabase(current4, GenerationUtil.ConfigManager.Service);
				if (result == Result.OK)
				{
					if (GenerationUtil.AnalysisFileCollector != null)
					{
						string network = "";
						if (current4.EcuInfoList.Count > 0)
						{
							if (current4.BusType == BusType.Bt_FlexRay)
							{
								network = current4.FlexRayNetworkName;
							}
							else if (current4.BusType == BusType.Bt_Ethernet)
							{
								network = "Ethernet1";
							}
						}
						GenerationUtil.AnalysisFileCollector.AddFile(current4.DestinationFilePath, network, current4.BusType, current4.ChannelNumber, ExportDatabase.DBType.CCPXCP, current4.FlexRayDatabase);
					}
					using (List<CcpXcpEcuInfo>.Enumerator enumerator5 = current4.EcuInfoList.GetEnumerator())
					{
						while (enumerator5.MoveNext())
						{
							CcpXcpEcuInfo current5 = enumerator5.Current;
							current5.Ecu.GeneratedDbcOrFibexFile = current4.DestinationFilePath;
							current5.Ecu.NetworkName = current4.FlexRayNetworkName;
							current5.Database.ExtraCPChannel = current4.FlexRayXcpChannel;
							if (current4.BusType == BusType.Bt_Ethernet)
							{
								flag &= current5.PreventDeconcatinationForFirstXcpOnUdpSlave;
								list.Add(current5.Ecu);
							}
							else
							{
								current5.Ecu.PreventDeconcatinationForFirstXcpOnUdpSlave = false;
							}
						}
						continue;
					}
				}
				if (current4.ErrorEcuInfo == null)
				{
					errorText = Resources_CcpXcp.CcpXcpErrorConfiguration;
				}
				else
				{
					errorText = string.Format(Resources_CcpXcp.CcpXcpErrorEcuConfiguration, current4.ErrorEcuInfo.Ecu.CcpXcpEcuDisplayName);
				}
				errorText += "\n";
				errorText += current4.ErrorText;
				return result;
			}
			foreach (CcpXcpEcu current6 in list)
			{
				current6.PreventDeconcatinationForFirstXcpOnUdpSlave = flag;
			}
			return Result.OK;
		}

		private static Result ConvertCcpXcpDatabases(string destinationDirectory, bool generateDisconnectCode, out string errorText)
		{
			if (GenerationUtil.CcpXcpGenerationInfo == null)
			{
				errorText = string.Empty;
				return Result.OK;
			}
			Dictionary<string, List<Database>> dictionary = new Dictionary<string, List<Database>>();
			int i = 0;
			while (i < GenerationUtil.ConfigManager.Service.DatabaseConfiguration.ActiveCCPXCPDatabases.Count<Database>())
			{
				Database database = GenerationUtil.ConfigManager.Service.DatabaseConfiguration.ActiveCCPXCPDatabases[i];
				string text = null;
				switch (database.FileType)
				{
				case DatabaseFileType.A2L:
					if (database.CcpXcpEcuList.Any<CcpXcpEcu>())
					{
						CcpXcpEcu ccpXcpEcu = database.CcpXcpEcuList.First<CcpXcpEcu>();
						text = ccpXcpEcu.GeneratedDbcOrFibexFile;
						if (string.IsNullOrEmpty(text))
						{
							errorText = string.Format(Resources.ErrorFileWithNameNotFound, Path.GetFileNameWithoutExtension(database.FilePath.Value) + ".dbc/.xml");
							return Result.Error;
						}
						goto IL_D7;
					}
					break;
				case DatabaseFileType.DBC:
				case DatabaseFileType.XML:
					text = FileSystemServices.GetAbsolutePath(database.FilePath.Value, GenerationUtil.ConfigManager.Service.ConfigFolderPath);
					goto IL_D7;
				default:
					goto IL_D7;
				}
				IL_107:
				i++;
				continue;
				IL_D7:
				if (!string.IsNullOrEmpty(text))
				{
					if (!dictionary.ContainsKey(text))
					{
						dictionary[text] = new List<Database>();
					}
					dictionary[text].Add(database);
					goto IL_107;
				}
				goto IL_107;
			}
			Dbc2Inc dbc2Inc = new Dbc2Inc();
			bool flag = false;
			int num = 0;
			foreach (KeyValuePair<string, List<Database>> current in dictionary)
			{
				num++;
				string key = current.Key;
				string tempDirectoryName;
				TempDirectoryManager.Instance.CreateNewTempDirectory(out tempDirectoryName);
				string text2;
				TempDirectoryManager.Instance.GetNewTempFilename(tempDirectoryName, Path.GetFileName(key), out text2);
				if (!GenerationUtil.TryCopyFile(key, text2, true))
				{
					errorText = Resources.ErrorCannotCreateTemporaryFile;
					Result result = Result.Error;
					return result;
				}
				CPType cPType = CPType.None;
				bool preventDeconcatinationForFirstXcpOnUdpSlave = false;
				CcpXcpEcu ccpXcpEcu2 = null;
				if (current.Value.Any<Database>())
				{
					Database database2 = current.Value.First<Database>();
					ccpXcpEcu2 = (database2.CcpXcpEcuList.Any<CcpXcpEcu>() ? database2.CcpXcpEcuList[0] : null);
					if (ccpXcpEcu2 != null)
					{
						preventDeconcatinationForFirstXcpOnUdpSlave = ccpXcpEcu2.PreventDeconcatinationForFirstXcpOnUdpSlave;
					}
					cPType = database2.CPType.Value;
				}
				string text3 = Path.Combine(destinationDirectory, LTLUtil.GetXCPIncFilename(Path.GetFileName(key), num, cPType));
				foreach (Database current2 in current.Value)
				{
					string text4 = "";
					CcpXcpEcu ccpXcpEcu3 = (current2.CcpXcpEcuList.Count > 0) ? current2.CcpXcpEcuList[0] : null;
					if (current2.CpProtectionsWithSeedAndKeyRequired.Count > 0 && (ccpXcpEcu3 == null || ccpXcpEcu3.UseDbParams || ccpXcpEcu3.IsSeedAndKeyUsed))
					{
						text4 = FileSystemServices.GetAbsolutePath(current2.CpProtectionsWithSeedAndKeyRequired[0].SeedAndKeyFilePath.Value, GenerationUtil.ConfigManager.Service.ConfigFolderPath);
					}
					else if (ccpXcpEcu3 != null && !ccpXcpEcu3.UseDbParams && ccpXcpEcu3.IsSeedAndKeyUsed && current2.CpProtections.Count > 0)
					{
						text4 = FileSystemServices.GetAbsolutePath(current2.CpProtections[0].SeedAndKeyFilePath.Value, GenerationUtil.ConfigManager.Service.ConfigFolderPath);
					}
					if (!string.IsNullOrEmpty(text4))
					{
						string text5 = Path.GetFileName(text2);
						if (BusType.Bt_FlexRay == current2.BusType.Value)
						{
							if (current2.CpProtectionsWithSeedAndKeyRequired.Count > 0)
							{
								text5 = current2.CpProtectionsWithSeedAndKeyRequired[0].ECUName.Value;
								if (string.IsNullOrEmpty(text5) && ccpXcpEcu3 != null)
								{
									text5 = ccpXcpEcu3.CcpXcpEcuDisplayName;
								}
							}
							else if (ccpXcpEcu3 != null && !ccpXcpEcu3.UseDbParams && ccpXcpEcu3.IsSeedAndKeyUsed)
							{
								text5 = ccpXcpEcu3.CcpXcpEcuDisplayName;
							}
						}
						else if (BusType.Bt_Ethernet == current2.BusType.Value && ccpXcpEcu3 != null && (current2.CpProtectionsWithSeedAndKeyRequired.Count > 0 || (!ccpXcpEcu3.UseDbParams && ccpXcpEcu3.IsSeedAndKeyUsed)))
						{
							text5 = ccpXcpEcu3.CcpXcpEcuDisplayName;
						}
						string destFilename;
						TempDirectoryManager.Instance.GetNewTempFilename(tempDirectoryName, LTLUtil.GetCcpXcpSkbFilenameForDbc2Inc(text5), out destFilename);
						if (!GenerationUtil.TryCopyFile(text4, destFilename, true))
						{
							errorText = Resources.ErrorCannotCreateTemporaryFile;
							Result result = Result.Error;
							return result;
						}
					}
				}
				string fileName = Path.GetFileName(text3);
				string text6;
				TempDirectoryManager.Instance.GetNewTempFilename(tempDirectoryName, fileName, out text6);
				if (!dbc2Inc.ConvertDBCToINC(text2, text6, generateDisconnectCode, preventDeconcatinationForFirstXcpOnUdpSlave, out errorText))
				{
					Result result = Result.Error;
					return result;
				}
				if (cPType == CPType.CCP101)
				{
					if (!File.Exists(text6))
					{
						Result result = Result.Error;
						return result;
					}
					StreamReader streamReader = new StreamReader(text6);
					string text7 = streamReader.ReadToEnd();
					streamReader.Close();
					string text8 = Path.ChangeExtension(text6, "mod");
					StreamWriter streamWriter = new StreamWriter(text8);
					StringBuilder stringBuilder = new StringBuilder();
					int num2 = text7.IndexOf("CCP_CONFIGURATION", StringComparison.Ordinal);
					num2 = text7.IndexOf(")", num2, StringComparison.Ordinal);
					string value = text7.Substring(0, num2 - 1);
					stringBuilder.AppendLine(value);
					stringBuilder.Append("                       [0x06 0x01 0x00 0x00 0x00 0x00 0x00]   {added START_STOP for CCP 1.01}");
					string value2 = text7.Substring(num2, text7.Length - num2);
					stringBuilder.Append(value2);
					streamWriter.Write(stringBuilder.ToString());
					streamWriter.Close();
					File.Delete(text6);
					File.Move(text8, text6);
				}
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				StreamReader streamReader2 = new StreamReader(text6);
				string text9 = streamReader2.ReadToEnd();
				streamReader2.Close();
				int num3 = 0;
				Regex regex = new Regex("\\s(.+\\.Timeout)\\s*=\\s*FREE\\[");
				Regex regex2 = new Regex("\\s(.+\\.Okay)\\s*=\\s*FREE\\[");
				while (-1 < (num3 = text9.IndexOf("VAR", num3, StringComparison.Ordinal)))
				{
					int num4 = text9.IndexOf("VAR", num3 + 1, StringComparison.Ordinal);
					int num5 = text9.IndexOf("EVENT", num3 + 1, StringComparison.Ordinal);
					int num6 = 2147483647;
					if (num4 > num3)
					{
						num6 = num4 - num3;
					}
					if (num5 > num3 && num5 - num3 < num6)
					{
						num6 = num5 - num3;
					}
					string input;
					if (num6 != 2147483647)
					{
						input = text9.Substring(num3, num6);
					}
					else
					{
						input = text9.Substring(num3);
					}
					Match match = regex.Match(input);
					if (match.Success && match.Groups.Count > 1)
					{
						list.Add(match.Groups[1].Value.Trim());
					}
					match = regex2.Match(input);
					if (match.Success && match.Groups.Count > 1)
					{
						list2.Add(match.Groups[1].Value.Trim());
					}
					num3++;
				}
				if (File.Exists(text3) && !flag)
				{
					if (InformMessageBox.Question(string.Format(Resources_CcpXcp.OverwriteExistingXcpIncFiles, new object[0])) != DialogResult.Yes)
					{
						errorText = "";
						Result result = Result.UserAbort;
						return result;
					}
					flag = true;
				}
				if (!GenerationUtil.TryCopyFile(text6, text3, true))
				{
					errorText = Resources.ErrorCannotCreateTemporaryFile;
					Result result = Result.Error;
					return result;
				}
				string text10 = string.Format("{0}_{1:D2}.", LTLUtil.GetXCPProtocolSmallLetterString(cPType), num);
				foreach (Database current3 in current.Value)
				{
					current3.CcpXcpSlaveNamePrefix = text10;
				}
				CcpXcpDatabaseInfo ccpXcpDatabaseInfo = null;
				foreach (CcpXcpDatabaseInfo current4 in GenerationUtil.CcpXcpGenerationInfo.DatabaseInfos)
				{
					foreach (CcpXcpEcuInfo current5 in current4.EcuInfoList)
					{
						if (current5.Ecu.Equals(ccpXcpEcu2))
						{
							ccpXcpDatabaseInfo = current4;
							break;
						}
					}
					if (ccpXcpDatabaseInfo != null)
					{
						break;
					}
				}
				if (ccpXcpDatabaseInfo == null)
				{
					ccpXcpDatabaseInfo = new CcpXcpDatabaseInfo(current.Value.First<Database>(), destinationDirectory);
					if (ccpXcpDatabaseInfo.BusType == BusType.Bt_FlexRay)
					{
						ccpXcpDatabaseInfo.ChannelNumber = current.Value.First<Database>().ExtraCPChannel;
						ccpXcpDatabaseInfo.FlexRayXcpChannel = ccpXcpDatabaseInfo.ChannelNumber;
					}
					GenerationUtil.CcpXcpGenerationInfo.DatabaseInfos.Add(ccpXcpDatabaseInfo);
				}
				ccpXcpDatabaseInfo.IncludeFileInfo = new CcpXcpIncludeFileInfo(text3, list2, list, text10);
			}
			errorText = "";
			return Result.OK;
		}

		private static void GatherTriggerAndMarkerSysVarsFromConfig(IDictionary<string, IList<GenerationUtil.VSysVar>> sysVars)
		{
			bool flag = false;
			string text = "Marker";
			string text2 = "Trigger";
			Dictionary<string, GenerationUtil.VSysVar> dictionary = new Dictionary<string, GenerationUtil.VSysVar>();
			Dictionary<string, GenerationUtil.VSysVar> dictionary2 = new Dictionary<string, GenerationUtil.VSysVar>();
			int num = 1;
			int num2 = 1;
			LoggingConfiguration loggingConfiguration = GenerationUtil.ConfigManager.VLProject.ProjectRoot.LoggingConfiguration;
			foreach (TriggerConfiguration current in loggingConfiguration.TriggerConfigurationsOfActiveMemories)
			{
				ReadOnlyCollection<RecordTrigger> readOnlyCollection = null;
				switch (current.TriggerMode.Value)
				{
				case TriggerMode.Triggered:
					readOnlyCollection = current.ActiveTriggers;
					break;
				case TriggerMode.Permanent:
					readOnlyCollection = current.ActivePermanentMarkers;
					break;
				case TriggerMode.OnOff:
					readOnlyCollection = current.ActiveOnOffTriggers;
					break;
				}
				if (readOnlyCollection != null)
				{
					foreach (RecordTrigger current2 in readOnlyCollection)
					{
						string unboundColumnDataConditionString = GUIUtil.GetUnboundColumnDataConditionString(current2, GenerationUtil.ConfigManager.ModelValidator.DatabaseServices);
						GenerationUtil.VSysVar vSysVar = new GenerationUtil.VSysVar(current2.Name.Value, "string", 8);
						if (!string.IsNullOrEmpty(current2.Name.Value))
						{
							GenerationUtil.VSysVar expr_108 = vSysVar;
							string comment = expr_108.Comment;
							expr_108.Comment = string.Concat(new string[]
							{
								comment,
								Resources.Name,
								": \"",
								current2.Name.Value,
								"\", "
							});
						}
						if (!string.IsNullOrEmpty(unboundColumnDataConditionString))
						{
							GenerationUtil.VSysVar expr_177 = vSysVar;
							string comment2 = expr_177.Comment;
							expr_177.Comment = string.Concat(new string[]
							{
								comment2,
								Resources.Condition,
								": \"",
								unboundColumnDataConditionString,
								"\""
							});
						}
						if (current2.TriggerEffect.Value == TriggerEffect.Marker)
						{
							if (vSysVar.Name.Length == 0)
							{
								vSysVar.Name = text + num2;
							}
							if (!dictionary2.ContainsKey(vSysVar.Name))
							{
								dictionary2.Add(vSysVar.Name, vSysVar);
							}
							else
							{
								flag = true;
							}
							num2++;
						}
						else
						{
							if (vSysVar.Name.Length == 0)
							{
								vSysVar.Name = text2 + num;
							}
							if (!dictionary.ContainsKey(vSysVar.Name))
							{
								dictionary.Add(vSysVar.Name, vSysVar);
							}
							else
							{
								flag = true;
							}
							num++;
						}
					}
				}
			}
			if (flag)
			{
				InformMessageBox.Warning(Resources.WarningSysVarNameConflicts);
			}
			if (dictionary.Count > 0)
			{
				sysVars.Add(text2, dictionary.Values.ToList<GenerationUtil.VSysVar>());
			}
			if (dictionary2.Count > 0)
			{
				sysVars.Add(text, dictionary2.Values.ToList<GenerationUtil.VSysVar>());
			}
		}

		private static void GatherAnalogSysVarsFromConfig(IDictionary<string, IList<GenerationUtil.VSysVar>> sysVars)
		{
			int num = 1;
			AnalogInputConfiguration analogInputConfiguration = GenerationUtil.ConfigManager.VLProject.ProjectRoot.HardwareConfiguration.AnalogInputConfiguration;
			if (analogInputConfiguration.AnalogInputs.Count > 0 && analogInputConfiguration.MapToSystemChannel.Value)
			{
				foreach (AnalogInput current in analogInputConfiguration.AnalogInputs)
				{
					if (current.IsActive.Value)
					{
						if (!sysVars.ContainsKey(""))
						{
							sysVars[""] = new List<GenerationUtil.VSysVar>();
						}
						GenerationUtil.VSysVar item = new GenerationUtil.VSysVar("sysAnalog_" + num.ToString("D2"), "float", 64);
						sysVars[""].Add(item);
					}
					num++;
				}
			}
		}

		private static void GatherAnalogSysVarsFromIniFile(string iniFilePath, IDictionary<string, IList<GenerationUtil.VSysVar>> sysVars)
		{
			uint num = 0u;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			FileSystemServices.GetIniFilePropertiesAndValues(iniFilePath, ref dictionary);
			if (dictionary.ContainsKey("DevType"))
			{
				string s = dictionary["DevType"].TrimStart("0x".ToCharArray());
				uint devType = uint.Parse(s, NumberStyles.HexNumber);
				ILoggerSpecifics loggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(devType);
				if (loggerSpecifics != null)
				{
					num = loggerSpecifics.IO.NumberOfAnalogInputs;
				}
			}
			if (num > 0u)
			{
				if (!sysVars.ContainsKey(""))
				{
					sysVars[""] = new List<GenerationUtil.VSysVar>();
				}
				for (uint num2 = 1u; num2 <= num; num2 += 1u)
				{
					sysVars[""].Add(new GenerationUtil.VSysVar("sysAnalog_" + num2.ToString("D2"), "float", 64));
				}
			}
		}

		internal static Result GenerateVSysVarFileFromConfig(string destinationFolderPath, out string errorText)
		{
			errorText = string.Empty;
			IDictionary<string, IList<GenerationUtil.VSysVar>> dictionary = new Dictionary<string, IList<GenerationUtil.VSysVar>>();
			GenerationUtil.GatherTriggerAndMarkerSysVarsFromConfig(dictionary);
			GenerationUtil.GatherAnalogSysVarsFromConfig(dictionary);
			if (dictionary.Count < 1)
			{
				return Result.OK;
			}
			string text = Path.Combine(destinationFolderPath, Vocabulary.FileNameCANoeSystemVariablesConfigFile + Vocabulary.FileExtensionDotVSysVar);
			if (File.Exists(text))
			{
				File.Delete(text);
			}
			Result result = GenerationUtil.GenerateVsysvarFile(destinationFolderPath, dictionary);
			if (result == Result.OK)
			{
				GenerationUtil.AnalysisFileCollector.AddFile(text, "", BusType.Bt_None, 0u, ExportDatabase.DBType.vSysVar, null);
			}
			else if (result == Result.Error)
			{
				errorText = Resources.ErrorCouldNotGenerateVSysVarFile;
			}
			return result;
		}

		private static Result GenerateVSysVarFileToLocation(string preselectedPath, IDictionary<string, IList<GenerationUtil.VSysVar>> sysVars)
		{
			if (sysVars.Count < 1)
			{
				InformMessageBox.Error(Resources_Trigger.ErrorNoTriggersOrMarkersForVSysVarFile);
				return Result.Error;
			}
			Result result;
			using (SaveFileDialog saveFileDialog = new SaveFileDialog())
			{
				saveFileDialog.Filter = Resources_Files.FileFilterDatabaseVSysVar;
				if (!string.IsNullOrEmpty(preselectedPath))
				{
					saveFileDialog.InitialDirectory = Path.GetDirectoryName(preselectedPath);
				}
				if (DialogResult.OK != saveFileDialog.ShowDialog())
				{
					result = Result.UserAbort;
				}
				else
				{
					if (File.Exists(saveFileDialog.FileName))
					{
						File.Delete(saveFileDialog.FileName);
					}
					if (Result.Error == GenerationUtil.GenerateVsysvarFile(saveFileDialog.FileName, sysVars))
					{
						InformMessageBox.Error(Resources.ErrorCouldNotGenerateVSysVarFile);
						result = Result.Error;
					}
					else
					{
						InformMessageBox.Info(Resources.SuccessfullyGeneratedVSysVarFile);
						result = Result.OK;
					}
				}
			}
			return result;
		}

		private static void GatherTriggerAndMarkerSysVarsFromIniFile(string iniFilePath, IDictionary<string, IList<GenerationUtil.VSysVar>> sysVars)
		{
			bool flag = false;
			string text = "Marker";
			string text2 = "Trigger";
			Dictionary<string, GenerationUtil.VSysVar> dictionary = new Dictionary<string, GenerationUtil.VSysVar>();
			bool flag2 = false;
			int num = 1;
			while (!flag2)
			{
				string name;
				if (FileSystemServices.GetIniFilePropertyValue(iniFilePath, text + num, out name))
				{
					GenerationUtil.VSysVar vSysVar = new GenerationUtil.VSysVar(name, "string", 8);
					if (vSysVar.Name.Length == 0)
					{
						vSysVar.Name = text + num;
					}
					if (!dictionary.ContainsKey(vSysVar.Name))
					{
						dictionary.Add(vSysVar.Name, vSysVar);
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					flag2 = true;
				}
				num++;
			}
			Dictionary<string, GenerationUtil.VSysVar> dictionary2 = new Dictionary<string, GenerationUtil.VSysVar>();
			dictionary2.Add("Trigger0", new GenerationUtil.VSysVar("Trigger0", "string", 8));
			flag2 = false;
			num = 1;
			while (!flag2)
			{
				string name2;
				if (FileSystemServices.GetIniFilePropertyValue(iniFilePath, text2 + num, out name2))
				{
					GenerationUtil.VSysVar vSysVar2 = new GenerationUtil.VSysVar(name2, "string", 8);
					if (vSysVar2.Name.Length == 0)
					{
						vSysVar2.Name = text2 + num;
					}
					if (!dictionary2.ContainsKey(vSysVar2.Name))
					{
						dictionary2.Add(vSysVar2.Name, vSysVar2);
					}
					else
					{
						flag = true;
					}
				}
				else
				{
					flag2 = true;
				}
				num++;
			}
			if (flag)
			{
				InformMessageBox.Warning(Resources.WarningSysVarNameConflicts);
			}
			if (dictionary2.Count > 0)
			{
				sysVars.Add(text2, dictionary2.Values.ToList<GenerationUtil.VSysVar>());
			}
			if (dictionary.Count > 0)
			{
				sysVars.Add(text, dictionary.Values.ToList<GenerationUtil.VSysVar>());
			}
		}

		public static void GenerateVSysVarFileFromIniFile(string destinationFolderPath, string iniFilePath)
		{
			if (!FileProxy.Exists(iniFilePath))
			{
				InformMessageBox.Error(string.Format(Resources.ErrorFileWithNameNotFound, iniFilePath));
				return;
			}
			IDictionary<string, IList<GenerationUtil.VSysVar>> sysVars = new Dictionary<string, IList<GenerationUtil.VSysVar>>();
			GenerationUtil.GatherTriggerAndMarkerSysVarsFromIniFile(iniFilePath, sysVars);
			GenerationUtil.GatherAnalogSysVarsFromIniFile(iniFilePath, sysVars);
			if (string.IsNullOrEmpty(destinationFolderPath))
			{
				GenerationUtil.GenerateVSysVarFileToLocation(null, sysVars);
				return;
			}
			if (Result.Error == GenerationUtil.GenerateVsysvarFile(destinationFolderPath, sysVars))
			{
				InformMessageBox.Error(Resources.ErrorCouldNotGenerateVSysVarFile);
			}
		}

		private static Result GenerateVsysvarFile(string destinationPath, IDictionary<string, IList<GenerationUtil.VSysVar>> sysVars)
		{
			if (string.IsNullOrEmpty(destinationPath))
			{
				return Result.Error;
			}
			string text = destinationPath;
			string text2 = string.Empty;
			if (!string.IsNullOrEmpty(Path.GetExtension(destinationPath)))
			{
				text = Path.GetDirectoryName(destinationPath);
				text2 = Path.GetFileNameWithoutExtension(destinationPath);
			}
			if (!Directory.Exists(text))
			{
				return Result.Error;
			}
			try
			{
				destinationPath = Path.Combine(text, Vocabulary.FileNameCANoeSystemVariablesConfigFile);
				if (!string.IsNullOrEmpty(text2))
				{
					destinationPath = Path.Combine(text, text2);
				}
				StringWriter stringWriter = new StringWriter();
				XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.Indentation = 2;
				xmlTextWriter.IndentChar = ' ';
				xmlTextWriter.WriteStartDocument();
				xmlTextWriter.WriteStartElement("systemvariables", null);
				xmlTextWriter.WriteAttributeString("version", "4");
				xmlTextWriter.WriteStartElement("namespace", null);
				xmlTextWriter.WriteAttributeString("name", "");
				xmlTextWriter.WriteAttributeString("comment", "");
				foreach (KeyValuePair<string, IList<GenerationUtil.VSysVar>> current in sysVars)
				{
					bool flag = false;
					if (current.Key != "")
					{
						flag = true;
						xmlTextWriter.WriteStartElement("namespace", null);
						xmlTextWriter.WriteAttributeString("name", current.Key);
						xmlTextWriter.WriteAttributeString("comment", "");
					}
					foreach (GenerationUtil.VSysVar current2 in current.Value)
					{
						GenerationUtil.WriteSysVar(xmlTextWriter, current2);
					}
					if (flag)
					{
						xmlTextWriter.WriteEndElement();
					}
				}
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.Flush();
				if (!File.Exists(destinationPath + Vocabulary.FileExtensionDotVSysVar))
				{
					StreamWriter streamWriter = new StreamWriter(destinationPath + Vocabulary.FileExtensionDotVSysVar, false);
					streamWriter.Write(stringWriter.ToString());
					streamWriter.Close();
				}
				else if (!GenerationUtil.IsExistingFileIdentic(destinationPath + Vocabulary.FileExtensionDotVSysVar, stringWriter))
				{
					bool flag2 = false;
					int num = 2;
					while (!flag2)
					{
						string path = string.Concat(new object[]
						{
							destinationPath,
							"_(",
							num,
							")",
							Vocabulary.FileExtensionDotVSysVar
						});
						if (!File.Exists(path))
						{
							StreamWriter streamWriter2 = new StreamWriter(path, false);
							streamWriter2.Write(stringWriter.ToString());
							streamWriter2.Close();
							flag2 = true;
						}
						num++;
					}
				}
				xmlTextWriter.Close();
				stringWriter.Close();
			}
			catch (Exception)
			{
				return Result.Error;
			}
			return Result.OK;
		}

		private static void WriteSysVar(XmlTextWriter xmlTextWriter, GenerationUtil.VSysVar sysVar)
		{
			xmlTextWriter.WriteStartElement("variable", null);
			xmlTextWriter.WriteAttributeString("anlyzLocal", "2");
			xmlTextWriter.WriteAttributeString("readOnly", "false");
			xmlTextWriter.WriteAttributeString("valueSequence", "false");
			xmlTextWriter.WriteAttributeString("unit", "");
			xmlTextWriter.WriteAttributeString("name", sysVar.Name);
			xmlTextWriter.WriteAttributeString("comment", sysVar.Comment);
			xmlTextWriter.WriteAttributeString("bitcount", sysVar.BitCount.ToString());
			xmlTextWriter.WriteAttributeString("isSigned", "true");
			xmlTextWriter.WriteAttributeString("encoding", "65001");
			xmlTextWriter.WriteAttributeString("type", sysVar.DataType);
			xmlTextWriter.WriteEndElement();
		}

		private static string MakeCaplCompliantSysVarName(string origName)
		{
			string text = GenerationUtil.ReplaceBlanksInString(origName, '_');
			if (GenerationUtil.StringIsCaplCompliant(text))
			{
				return text;
			}
			return "";
		}

		private static bool IsExistingFileIdentic(string destinationPath, StringWriter XmlString)
		{
			if (!File.Exists(destinationPath))
			{
				return false;
			}
			Stream stream = GenerationUtil.GenerateStreamFromString(XmlString.GetStringBuilder().ToString());
			FileStream fileStream;
			try
			{
				fileStream = new FileStream(destinationPath, FileMode.Open);
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			if (fileStream.Length != stream.Length)
			{
				fileStream.Close();
				stream.Close();
				return false;
			}
			int num;
			int num2;
			do
			{
				num = fileStream.ReadByte();
				num2 = stream.ReadByte();
			}
			while (num == num2 && num != -1);
			fileStream.Close();
			stream.Close();
			return num - num2 == 0;
		}

		private static Stream GenerateStreamFromString(string s)
		{
			MemoryStream memoryStream = new MemoryStream();
			StreamWriter streamWriter = new StreamWriter(memoryStream);
			streamWriter.Write(s);
			streamWriter.Flush();
			memoryStream.Position = 0L;
			return memoryStream;
		}

		public static bool TryDeleteFile(string filename, out string errorText, bool isTempFile)
		{
			try
			{
				File.Delete(filename);
			}
			catch
			{
				if (isTempFile)
				{
					errorText = string.Format(Resources.CannotDeleteTemporaryFile, filename);
				}
				else
				{
					errorText = string.Format(Resources.CannotDeleteFile, filename);
				}
				return false;
			}
			errorText = "";
			return true;
		}

		public static bool TryDeleteFile(string filename, out string errorText)
		{
			return GenerationUtil.TryDeleteFile(filename, out errorText, false);
		}

		public static bool TryCopyFile(string sourceFilename, string DestFilename, bool overwrite)
		{
			try
			{
				File.Copy(sourceFilename, DestFilename, overwrite);
			}
			catch
			{
				return false;
			}
			return true;
		}

		public static bool TryMoveFile(string sourceFilePath, string destFilePath, out string errorText)
		{
			try
			{
				File.Move(sourceFilePath, destFilePath);
			}
			catch
			{
				errorText = string.Format(Resources.CannotCopyFileTo, sourceFilePath, destFilePath);
				return false;
			}
			errorText = "";
			return true;
		}

		public static bool TryDeleteDirectory(string directoryname, out string errorText)
		{
			try
			{
				Directory.Delete(directoryname);
			}
			catch
			{
				errorText = string.Format(Resources.CannotDeleteDirectory, directoryname);
				return false;
			}
			errorText = "";
			return true;
		}

		public static Result MoveFiles(List<string> files, string targetDir, bool askUser, out string errorText)
		{
			errorText = "";
			List<string> list = new List<string>();
			foreach (string current in files)
			{
				string fileName = Path.GetFileName(current);
				if (!string.IsNullOrEmpty(fileName))
				{
					string path = Path.Combine(targetDir, fileName);
					if (File.Exists(path))
					{
						list.Add(fileName);
					}
				}
			}
			if (list.Count > 0)
			{
				if (!askUser)
				{
					errorText = string.Format(Resources.ErrorDirContainsFilesNothingWillBeWritten, list.Count);
					return Result.Error;
				}
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(Resources.ErrorDirAlreadyContainsFollowingFiles, targetDir);
				stringBuilder.Append("\n\n");
				foreach (string current2 in list)
				{
					stringBuilder.AppendLine(current2.TrimStart(new char[]
					{
						Path.DirectorySeparatorChar
					}));
				}
				stringBuilder.AppendLine(string.Empty);
				stringBuilder.AppendLine(Resources.QuestionContinueAndReplaceFiles);
				if (DialogResult.Yes != InformMessageBox.Question(stringBuilder.ToString()))
				{
					return Result.UserAbort;
				}
			}
			foreach (string current3 in files)
			{
				string fileName2 = Path.GetFileName(current3);
				if (!string.IsNullOrEmpty(fileName2))
				{
					string text = Path.Combine(targetDir, fileName2);
					if (File.Exists(text) && !FileSystemServices.TryDeleteFile(text))
					{
						errorText = string.Format(Resources.ErrorFailedWriteDestFile, text);
						Result result = Result.Error;
						return result;
					}
					if (!FileSystemServices.TryMoveFile(current3, targetDir))
					{
						errorText = string.Format(Resources.ErrorFailedWriteDestFile, text);
						Result result = Result.Error;
						return result;
					}
				}
			}
			return Result.OK;
		}

		private static Result TryMoveFilesFromDirectory(string sourceFolderPath, string pattern, string targetFolderPath, out List<string> targetFileList, out string errorText)
		{
			errorText = string.Empty;
			targetFileList = new List<string>();
			string[] files;
			try
			{
				files = Directory.GetFiles(sourceFolderPath, pattern);
			}
			catch (Exception)
			{
				errorText = Resources.ErrorUnauthorizedAccess;
				return Result.Error;
			}
			for (int i = 0; i < files.Count<string>(); i++)
			{
				string fileName = Path.GetFileName(files[i]);
				if (!string.IsNullOrEmpty(fileName))
				{
					string text = Path.Combine(targetFolderPath, fileName);
					if (File.Exists(text) && !GenerationUtil.TryDeleteFile(text, out errorText))
					{
						return Result.Error;
					}
					if (!GenerationUtil.TryMoveFile(files[i], text, out errorText))
					{
						return Result.Error;
					}
					targetFileList.Add(text);
				}
			}
			return Result.OK;
		}

		private static string ReplaceBlanksInString(string str, char replacementChar)
		{
			if (!string.IsNullOrEmpty(str))
			{
				str = str.Replace(' ', replacementChar);
			}
			return str;
		}

		public static bool StringIsCaplCompliant(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			Regex regex = new Regex("[^a-zA-Z0-9_]");
			return !regex.IsMatch(str) && (str.Length <= 0 || !char.IsDigit(str[0]));
		}

		public static bool IsLtlCompliantName(string str)
		{
			return GenerationUtil.IsLtlCompliantNamePostfix(str) && (str.Length <= 0 || !char.IsDigit(str[0]));
		}

		public static bool IsLtlCompliantNamePostfix(string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return false;
			}
			Regex regex = new Regex("[^a-zA-Z0-9_]");
			return !regex.IsMatch(str);
		}

		public static bool MakeStringCaplCompliant(string strIn, out string strOut)
		{
			strOut = string.Empty;
			if (string.IsNullOrEmpty(strIn))
			{
				return false;
			}
			if (GenerationUtil.StringIsCaplCompliant(strIn))
			{
				strOut = strIn;
				return true;
			}
			strIn = strIn.Replace("Ä", "Ae").Replace("ä", "ae");
			strIn = strIn.Replace("Ü", "Ue").Replace("ü", "ue");
			strIn = strIn.Replace("Ö", "Oe").Replace("ö", "oe");
			strIn = strIn.Replace("ß", "ss");
			strIn = strIn.Replace("@", "_at_");
			strIn = strIn.Replace("€", "EUR");
			bool result;
			try
			{
				strOut = Regex.Replace(strIn, "[^a-zA-Z0-9_]", "_", RegexOptions.None);
				result = true;
			}
			catch (ArgumentException)
			{
				result = false;
			}
			return result;
		}

		public static bool IndexNameIfNecessary(string nameIn, out string nameOut, IEnumerable<string> names, string separator, bool ignoreCase)
		{
			nameOut = nameIn;
			uint num = 1u;
			if (string.IsNullOrEmpty(nameIn) || names == null)
			{
				return false;
			}
			if (names.Count<string>() < 1)
			{
				return true;
			}
			if (names.Contains(nameIn, StringComparer.OrdinalIgnoreCase))
			{
				num += 1u;
				foreach (string current in names)
				{
					if (current.StartsWith(nameIn + separator, ignoreCase, CultureInfo.InvariantCulture))
					{
						string value = current.Substring(nameIn.Length + separator.Length, current.Length - (nameIn.Length + separator.Length));
						try
						{
							uint num2 = Convert.ToUInt32(value);
							if (num2 >= num)
							{
								num = num2 + 1u;
							}
						}
						catch (Exception)
						{
						}
					}
				}
			}
			if (num > 1u)
			{
				nameOut = nameIn + separator + num;
			}
			return true;
		}

		public static bool NameHasIndex(string name, char separator)
		{
			if (name.Contains(separator))
			{
				string[] array = name.Split(new char[]
				{
					separator
				});
				try
				{
					Convert.ToUInt32(array[array.Length - 1]);
				}
				catch (Exception)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static bool RemoveIndexFromName(string indexedName, char separator, out string name, out int index)
		{
			name = indexedName;
			index = -1;
			if (!GenerationUtil.NameHasIndex(indexedName, separator))
			{
				return true;
			}
			string[] array = name.Split(new char[]
			{
				separator
			});
			try
			{
				index = Convert.ToInt32(array[array.Length - 1]);
			}
			catch (Exception)
			{
				return false;
			}
			name = string.Empty;
			for (int i = 0; i < array.Length - 1; i++)
			{
				name += array[i];
			}
			return true;
		}

		private static Result TryMoveGeneratedDBCFilesFromDirectory(string sourceFolderPath, string targetFolderPath, out List<string> targetFileList, out string errorText)
		{
			errorText = string.Empty;
			targetFileList = new List<string>();
			string[] files;
			try
			{
				files = Directory.GetFiles(sourceFolderPath, "*.dbc");
			}
			catch (Exception)
			{
				errorText = Resources.ErrorUnauthorizedAccess;
				return Result.Error;
			}
			for (int i = 0; i < files.Count<string>(); i++)
			{
				string text = Path.GetFileName(files[i]);
				if (text != null)
				{
					Match match = Regex.Match(text, "(CAN \\d+).dbc");
					if (match.Success)
					{
						text = text.Remove(match.Index + 3, 1);
					}
					if (!string.IsNullOrEmpty(text))
					{
						string text2 = Path.Combine(targetFolderPath, text);
						if (File.Exists(text2) && !GenerationUtil.TryDeleteFile(text2, out errorText))
						{
							return Result.Error;
						}
						if (!GenerationUtil.TryMoveFile(files[i], text2, out errorText))
						{
							return Result.Error;
						}
						targetFileList.Add(text2);
					}
				}
			}
			return Result.OK;
		}
	}
}
