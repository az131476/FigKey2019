using System;
using System.IO;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public class SecWrite : GenericToolInterface
	{
		public SecWrite()
		{
			base.FileName = "SecWrite.exe";
		}

		public bool InstallLicenseFile(string licenseFilePath, uint deviceTypeCode, out string errorText)
		{
			if (!File.Exists(licenseFilePath))
			{
				errorText = string.Format(Resources.FileDoesNotExist, licenseFilePath);
				return false;
			}
			string tempDirectoryName = "";
			if (!TempDirectoryManager.Instance.CreateNewTempDirectory(out tempDirectoryName))
			{
				errorText = Resources.ErrorCannotCreateTemporaryDirectory;
				return false;
			}
			string fullTempDirectoryPath = TempDirectoryManager.Instance.GetFullTempDirectoryPath(tempDirectoryName);
			string text = Path.Combine(fullTempDirectoryPath, Path.GetFileName(licenseFilePath));
			try
			{
				File.Copy(licenseFilePath, text, true);
			}
			catch (Exception)
			{
				errorText = Resources.ErrorUnableToCopyLicenseFile;
				bool result = false;
				return result;
			}
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-f \"" + text + "\"");
			base.AddCommandLineArgument("-Mv ");
			base.AddCommandLineArgument("-T 0x" + deviceTypeCode.ToString("X"));
			int num = base.RunSynchronous(fullTempDirectoryPath);
			if (num != 0)
			{
				if (num == 30)
				{
					errorText = Resources.GiN_EC_InstallLicense_IllegalArg;
				}
				else
				{
					errorText = base.GetGinErrorCodeString(num);
				}
				return false;
			}
			errorText = "";
			return true;
		}
	}
}
