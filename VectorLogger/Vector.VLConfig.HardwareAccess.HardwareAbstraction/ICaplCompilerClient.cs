using System;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public interface ICaplCompilerClient
	{
		bool GetComplierCommandLineArgs(string baseFilePath, out string commandLineArgs, out string errorText);

		bool GetLinkerCommandLineArgs(string baseFilePath, out string commandLineArgs, out string errorText);
	}
}
