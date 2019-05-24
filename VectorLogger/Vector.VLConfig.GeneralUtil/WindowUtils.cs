using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Vector.VLConfig.GeneralUtil
{
	public class WindowUtils
	{
		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		public static bool SetForegroundWindow()
		{
			Process currentProcess = Process.GetCurrentProcess();
			return currentProcess.MainWindowHandle != IntPtr.Zero && WindowUtils.SetForegroundWindow(currentProcess.MainWindowHandle);
		}

		public static bool SetForegroundWindow(string processName)
		{
			List<Process> source = ProcessUtils.GetRunningProcesses(processName).ToList<Process>();
			return source.Any<Process>() && source.First<Process>().MainWindowHandle != IntPtr.Zero && WindowUtils.SetForegroundWindow(source.First<Process>().MainWindowHandle);
		}
	}
}
