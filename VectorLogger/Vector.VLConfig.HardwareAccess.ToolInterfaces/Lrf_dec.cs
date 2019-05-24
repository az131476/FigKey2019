using System;
using System.IO;
using System.Linq;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class Lrf_dec : GenericToolInterface
	{
		private const int GiN_ErrorCode_FilForm = 32;

		public Lrf_dec()
		{
			base.FileName = "lrf_dec.exe";
		}

		public bool ProcessRawDataFolder(string folder, FileConversionParameters conversionParameters, out string errorText, IProgressIndicator pi, ProcessExitedDelegate processExitedDelegate)
		{
			return this.ProcessRawDataFolder(folder, false, false, conversionParameters, out errorText, pi, processExitedDelegate);
		}

		public bool ProcessRawDataFolder(string folder, bool isKeepingRawData, bool isKeepingOriginalIndex, FileConversionParameters conversionParameters, out string errorText, IProgressIndicator pi, ProcessExitedDelegate processExitedDelegate)
		{
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-~");
			if (isKeepingRawData)
			{
				base.AddCommandLineArgument("-k");
			}
			if (isKeepingOriginalIndex)
			{
				base.AddCommandLineArgument("-n");
			}
			base.AddCommandLineArgument("-v");
			base.AddCommandLineArgument(string.Format("\"{0}\"", folder));
			if (conversionParameters.SuppressBufferConcat)
			{
				base.AddCommandLineArgument("-0");
			}
			if (!Directory.Exists(folder))
			{
				errorText = string.Format(Resources.ErrorFolderNotFound, folder);
				return false;
			}
			int num = 0;
			string[] directories = Directory.GetDirectories(folder, "!*");
			if (directories.Length > 0)
			{
				string[] array = directories;
				for (int i = 0; i < array.Length; i++)
				{
					string path = array[i];
					string[] files = Directory.GetFiles(path);
					num += files.Length;
				}
			}
			else
			{
				string[] files2 = Directory.GetFiles(folder);
				string[] array2 = files2;
				for (int j = 0; j < array2.Length; j++)
				{
					string path2 = array2[j];
					if (!Path.GetFileName(path2).Contains('.'))
					{
						num++;
					}
				}
			}
			pi.SetMinMax(0, num);
			Lrf_decValueParser progressIndicatorValueParser = new Lrf_decValueParser(num);
			base.RunAsynchronousWithProgressBar(pi, progressIndicatorValueParser, processExitedDelegate);
			if (base.LastExitCode != 0)
			{
				errorText = base.GetGinErrorCodeString(base.LastExitCode);
				return false;
			}
			errorText = "";
			return true;
		}

		public override string GetLastGinErrorCodeString()
		{
			if (base.LastExitCode == 32)
			{
				return Resources.Lrf_dec_InvalidInputFileFormat;
			}
			return base.GetLastGinErrorCodeString();
		}
	}
}
