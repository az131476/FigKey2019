using System;
using System.IO;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class Dbc2Inc : GenericToolInterface
	{
		public Dbc2Inc()
		{
			base.FileName = "Dbc2Inc.exe";
		}

		public bool ConvertDBCToINC(string dbcFile, string incFile, bool generateDisconnectCode, bool preventDeconcatinationForFirstXcpOnUdpSlave, out string errorText)
		{
			if (!File.Exists(dbcFile))
			{
				errorText = string.Format(Resources.FileDoesNotExist, dbcFile);
				return false;
			}
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("\"" + dbcFile + "\"");
			base.AddCommandLineArgument("\"" + incFile + "\"");
			base.AddCommandLineArgument("\"%2%\"");
			base.AddCommandLineArgument("-c");
			base.AddCommandLineArgument("-C");
			if (generateDisconnectCode)
			{
				base.AddCommandLineArgument("-d");
			}
			if (preventDeconcatinationForFirstXcpOnUdpSlave)
			{
				base.AddCommandLineArgument("-b");
			}
			base.AddCommandLineArgument("-o");
			int num = base.RunSynchronous();
			if (num != 0)
			{
				errorText = base.GetGinErrorCodeString(num);
				return false;
			}
			if (base.LastStdErr.Contains("DBC2INC: Warning: The file contains no CCP/XCP configurations."))
			{
				InformMessageBox.Info(string.Format(Resources_CcpXcp.WarningDatabaseContainsNoCCPXCPInformation, Path.GetFileName(dbcFile)));
			}
			errorText = "";
			return true;
		}
	}
}
