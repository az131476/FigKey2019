using System;
using System.Collections.ObjectModel;
using System.IO;
using Vector.CaplCompilerAccess;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class CaplCompiler : GenericToolInterface
	{
		private static ReadOnlyCollection<string> sKeywords;

		public static ReadOnlyCollection<string> CaplKeywords
		{
			get
			{
				if (CaplCompiler.sKeywords == null)
				{
					CaplCompilerAPI caplCompilerAPI = new CaplCompilerAPI();
					CaplCompiler.sKeywords = new ReadOnlyCollection<string>(caplCompilerAPI.Intrinsics.Keywords);
					caplCompilerAPI.Dispose();
				}
				return CaplCompiler.sKeywords;
			}
		}

		public CaplCompiler()
		{
			base.FileName = "CAPLcompExec.exe";
		}

		public Result CompileAndLink(string baseFilePath, ICaplCompilerClient caplCompilerClient, out string errorText)
		{
			string path = baseFilePath + Vocabulary.FileExtensionDotCAN;
			baseFilePath + Vocabulary.FileExtensionDotCBF;
			string path2 = baseFilePath + Vocabulary.FileExtensionDotBIN;
			if (caplCompilerClient == null || !File.Exists(path))
			{
				errorText = string.Format(Resources.InternalError, string.Format(Resources.ErrorUnableToCreateFile, Path.GetFileName(path2)));
				errorText += GenerationUtilVN.DebugInfo("'caplCompilerClient' must not be null!");
				return Result.Error;
			}
			base.DeleteCommandLineArguments();
			string commandLineArgument;
			if (!caplCompilerClient.GetComplierCommandLineArgs(baseFilePath, out commandLineArgument, out errorText))
			{
				return Result.Error;
			}
			base.AddCommandLineArgument(commandLineArgument);
			int num = base.RunSynchronous();
			if (num != 0)
			{
				errorText = string.Format(Resources.InternalError, string.Format(Resources.ErrorUnableToCreateFile, Path.GetFileName(path2)));
				errorText += GenerationUtilVN.DebugInfo(base.LastStdOut);
				return Result.Error;
			}
			base.DeleteCommandLineArguments();
			if (!caplCompilerClient.GetLinkerCommandLineArgs(baseFilePath, out commandLineArgument, out errorText))
			{
				return Result.Error;
			}
			base.AddCommandLineArgument(commandLineArgument);
			num = base.RunSynchronous();
			if (num != 0)
			{
				errorText = string.Format(Resources.InternalError, string.Format(Resources.ErrorUnableToCreateFile, Path.GetFileName(path2)));
				errorText += GenerationUtilVN.DebugInfo(base.LastStdOut);
				return Result.Error;
			}
			return Result.OK;
		}
	}
}
