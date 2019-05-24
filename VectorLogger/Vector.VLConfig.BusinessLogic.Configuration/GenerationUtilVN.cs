using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Vector.VLConfig.CaplGeneration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction.VN16XXlog;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	internal static class GenerationUtilVN
	{
		private const string cDAIOSysvarFileResourceName = "Vector.VLConfig.Resources.VN1630_IO.vsysvar";

		public static AnalysisFileCollector AnalysisFileCollector
		{
			get;
			set;
		}

		public static ConfigurationManager ConfigManager
		{
			get;
			set;
		}

		public static ICaplCompilerClient CaplCompilerClient
		{
			get;
			set;
		}

		public static Result GenerateVSysVarFileForDAIO(out string errorText)
		{
			string generalPurposeTempDirectory = GenerationUtilVN.AnalysisFileCollector.GeneralPurposeTempDirectory;
			string fullPath;
			Result result = GenerationUtilVN.GenerateVSysVarFileForDAIO(generalPurposeTempDirectory, out errorText, out fullPath);
			if (result == Result.OK)
			{
				GenerationUtilVN.AnalysisFileCollector.AddFile(fullPath, "", BusType.Bt_None, 0u, ExportDatabase.DBType.vSysVar, null);
			}
			return result;
		}

		public static Result GenerateVSysVarFileForDAIO(string targetFolderPath, out string errorText, out string targetFilePath)
		{
			errorText = string.Empty;
			targetFilePath = string.Empty;
			targetFilePath = targetFolderPath + Path.DirectorySeparatorChar + Vocabulary.FileNameIoSysVars;
			if (!FileSystemServices.WriteEmbeddedResourceToFile("Vector.VLConfig.Resources.VN1630_IO.vsysvar", targetFilePath, FileSystemServices.EnumAssemblyType.CallingAssembly))
			{
				errorText = string.Format(Resources.InternalError, string.Format(Resources.ErrorUnableToCreateFile, Vocabulary.FileNameIoSysVars));
				return Result.Error;
			}
			return Result.OK;
		}

		public static Result ExportToVNBinary(string baseFilePath, out string errorText)
		{
			string path = baseFilePath + Vocabulary.FileExtensionDotBIN;
			if (GenerationUtilVN.CaplCompilerClient == null)
			{
				errorText = string.Format(Resources.InternalError, string.Format(Resources.ErrorUnableToCreateFile, Path.GetFileName(path)));
				errorText += GenerationUtilVN.DebugInfo("'CaplCompilerClient' must not be null!");
				return Result.Error;
			}
			Result result;
			if ((result = GenerationUtilVN.GenerateCaplCode(baseFilePath, out errorText)) != Result.OK)
			{
				return result;
			}
			CaplCompiler caplCompiler = new CaplCompiler();
			if ((result = caplCompiler.CompileAndLink(baseFilePath, GenerationUtilVN.CaplCompilerClient, out errorText)) != Result.OK)
			{
				return result;
			}
			if ((result = VN16XXlogUtils.EncryptCodeFile(baseFilePath, out errorText)) != Result.OK)
			{
				return result;
			}
			return Result.OK;
		}

		public static string ComputeMD5Hash(string fileName)
		{
			string result = string.Empty;
			try
			{
				if (File.Exists(fileName))
				{
					MD5 mD = MD5.Create();
					using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
					{
						if (fileStream.CanRead)
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

		private static Result GenerateCaplCode(string baseFilePath, out string errorText)
		{
			string caplFile = baseFilePath + Vocabulary.FileExtensionDotCAN;
			CaplGenerator caplGenerator = new CaplGenerator(GenerationUtilVN.ConfigManager.VLProject, GenerationUtilVN.ConfigManager.Service.LoggerSpecifics, GenerationUtilVN.ConfigManager.Service.ApplicationDatabaseManager, GenerationUtilVN.ConfigManager.Service.ConfigFolderPath, GenerationUtil.AppDataAccess.AppDataRoot.GlobalOptions);
			Result result = caplGenerator.GenerateCapl(caplFile) ? Result.OK : Result.Error;
			GenerationUtilVN.ComposeErrorText(caplGenerator, out errorText);
			return result;
		}

		private static void ComposeErrorText(CaplGenerator gen, out string errorText)
		{
			errorText = (gen.GenerationErrors.Any<CaplGenerator.EnumCaplGenerationError>() ? Resources.ErrorCaplGenerationHint : string.Empty);
			foreach (CaplGenerator.EnumCaplGenerationError current in gen.GenerationErrors)
			{
				errorText = errorText + Environment.NewLine + "- ";
				CaplGenerator.EnumCaplGenerationError enumCaplGenerationError = current;
				if (enumCaplGenerationError == CaplGenerator.EnumCaplGenerationError.InvalidTriggerCondition)
				{
					errorText += Resources_Trigger.ErrorCaplGenerationIInvalidTriggerConfiguration;
				}
				else
				{
					errorText += Resources.ErrorUnknown;
				}
			}
		}

		public static string DebugInfo(string additionalDebugInfo)
		{
			return string.Empty;
		}
	}
}
