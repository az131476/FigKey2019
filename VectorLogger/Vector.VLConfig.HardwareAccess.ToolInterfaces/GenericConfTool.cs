using System;
using System.IO;
using System.Reflection;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public abstract class GenericConfTool : GenericToolInterface
	{
		public const string WebServerDisplayZipName = "webserver.zip";

		protected string lastOutput;

		protected string iniFilename;

		protected bool hasWebServer;

		public GenericConfTool(string iniFilename)
		{
			this.iniFilename = iniFilename;
			this.hasWebServer = false;
		}

		public bool Compile(string ltlFile, bool includeLTLSourceCode, bool generateDBCFilesForChannels, bool includeWebServerDisplayZip, out string errorText, string compileErrorFile)
		{
			string directoryName = Path.GetDirectoryName(ltlFile);
			string text = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), this.iniFilename);
			string text2 = Path.Combine(directoryName, this.iniFilename);
			if (!File.Exists(text2))
			{
				try
				{
					File.Copy(text, text2);
				}
				catch
				{
					errorText = string.Format(Resources.FileDoesNotExist, Path.GetFileName(text));
					return false;
				}
			}
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("-v");
			base.AddCommandLineArgument("\"" + ltlFile + "\"");
			if (!includeLTLSourceCode)
			{
				base.AddCommandLineArgument("-n");
			}
			if (generateDBCFilesForChannels)
			{
				base.AddCommandLineArgument("-T");
			}
			if (this.hasWebServer && includeWebServerDisplayZip)
			{
				base.AddCommandLineArgument("-D \"webserver.zip\"");
			}
			base.RunSynchronous(directoryName);
			this.lastOutput = base.LastStdOut;
			if (compileErrorFile != null && compileErrorFile.Length > 0 && base.LastStdErr.Length > 0)
			{
				File.WriteAllText(compileErrorFile, base.LastStdOut + "\n\n" + base.LastStdErr);
			}
			errorText = base.GetGinErrorCodeString(base.LastExitCode);
			return base.LastExitCode == 0;
		}

		public string GetLastOutput()
		{
			return this.lastOutput;
		}
	}
}
