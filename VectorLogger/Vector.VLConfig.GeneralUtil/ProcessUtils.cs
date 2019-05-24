using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Vector.VLConfig.GeneralUtil
{
	public class ProcessUtils
	{
		public static IEnumerable<Process> GetRunningProcesses()
		{
			List<Process> list = new List<Process>();
			list.AddRange(Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName));
			return list;
		}

		public static IEnumerable<Process> GetRunningProcesses(string processName)
		{
			List<Process> list = new List<Process>();
			list.AddRange(Process.GetProcessesByName(processName));
			return list;
		}
	}
}
