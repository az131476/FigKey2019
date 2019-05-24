using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.GUI;

namespace Vector.VLConfig
{
	internal static class Program
	{
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		[STAThread]
		private static void Main(string[] args)
		{
			bool flag = true;
			using (new Mutex(true, "AFD7327B-C4A3-4321-B9DC-5E5E14289451", ref flag))
			{
				if (flag || ProgramUtils.CheckParametersForAllowMultipleInstances(args))
				{
					AppDataAccess appDataAccess = new AppDataAccess();
					if (ProgramUtils.CheckParametersForGLC(args))
					{
						string directoryName = Path.GetDirectoryName(Application.ExecutablePath);
						if (string.Compare(directoryName, Directory.GetCurrentDirectory(), true) != 0)
						{
							Directory.SetCurrentDirectory(directoryName);
						}
					}
					if (!appDataAccess.LoadAppDataSettings())
					{
						appDataAccess.AppDataRoot.GlobalOptions.Language = ProgramUtils.GetCurrCultureLanguage();
					}
					ProgramUtils.SetCulture(appDataAccess.AppDataRoot.GlobalOptions.Language);
					if (ProgramUtils.DisplayDisclaimer())
					{
						Application.EnableVisualStyles();
						Application.SetCompatibleTextRenderingDefault(false);
						Application.Run(new MainWindow(args, appDataAccess));
					}
				}
				else
				{
					Process currentProcess = Process.GetCurrentProcess();
					Process[] processesByName = Process.GetProcessesByName(currentProcess.ProcessName);
					for (int i = 0; i < processesByName.Length; i++)
					{
						Process process = processesByName[i];
						if (process.Id != currentProcess.Id)
						{
							Program.SetForegroundWindow(process.MainWindowHandle);
							break;
						}
					}
				}
			}
		}
	}
}
