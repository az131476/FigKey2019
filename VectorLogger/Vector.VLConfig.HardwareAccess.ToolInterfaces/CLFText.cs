using System;
using System.IO;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class CLFText : GenericToolInterface
	{
		public CLFText()
		{
			base.FileName = "CLFText";
		}

		public string RetrieveMetaInfos(string inFile)
		{
			string result = Resources.MetaInfosNotAvailable;
			string tempDirectoryName;
			if (TempDirectoryManager.Instance.CreateNewTempDirectory(out tempDirectoryName))
			{
				string text = Path.Combine(TempDirectoryManager.Instance.GetFullTempDirectoryPath(tempDirectoryName), Path.GetFileNameWithoutExtension(inFile)) + ".txt";
				base.AddCommandLineArgument("-R");
				base.AddCommandLineArgument(inFile);
				base.AddCommandLineArgument(text);
				try
				{
					base.RunSynchronous();
					if (File.Exists(text))
					{
						result = File.ReadAllText(text);
						TempDirectoryManager.Instance.ReleaseTempDirectory(tempDirectoryName);
					}
				}
				catch
				{
				}
			}
			return result;
		}
	}
}
