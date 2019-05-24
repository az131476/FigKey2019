using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Vector.VLConfig.GeneralUtil.GUI
{
	public static class TaskbarProgress
	{
		[Flags]
		public enum TaskbarStates
		{
			NoProgress = 0,
			Indeterminate = 1,
			Normal = 2,
			Error = 4,
			Paused = 8
		}

		[Guid("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		private interface ITaskbarList3
		{
			[PreserveSig]
			void HrInit();

			[PreserveSig]
			void AddTab(IntPtr hwnd);

			[PreserveSig]
			void DeleteTab(IntPtr hwnd);

			[PreserveSig]
			void ActivateTab(IntPtr hwnd);

			[PreserveSig]
			void SetActiveAlt(IntPtr hwnd);

			[PreserveSig]
			void MarkFullscreenWindow(IntPtr hwnd, [MarshalAs(UnmanagedType.Bool)] bool fFullscreen);

			[PreserveSig]
			void SetProgressValue(IntPtr hwnd, ulong ullCompleted, ulong ullTotal);

			[PreserveSig]
			void SetProgressState(IntPtr hwnd, TaskbarProgress.TaskbarStates state);
		}

		[ClassInterface(ClassInterfaceType.None), Guid("56FDF344-FD6D-11d0-958A-006097C9A090")]
		[ComImport]
		private class TaskbarInstance
		{
			[MethodImpl(MethodImplOptions.InternalCall)]
			public extern TaskbarInstance();
		}

		private static readonly TaskbarProgress.ITaskbarList3 sTaskbarInstance;

		private static readonly bool sTaskbarSupported;

		private static readonly IntPtr sMainWindowHandle;

		static TaskbarProgress()
		{
			TaskbarProgress.sTaskbarInstance = (new TaskbarProgress.TaskbarInstance() as TaskbarProgress.ITaskbarList3);
			TaskbarProgress.sTaskbarSupported = (TaskbarProgress.sTaskbarInstance != null && Environment.OSVersion.Version >= new Version(6, 1));
			TaskbarProgress.sMainWindowHandle = IntPtr.Zero;
			if (Application.OpenForms.Count > 0)
			{
				TaskbarProgress.sMainWindowHandle = Application.OpenForms[0].Handle;
			}
		}

		public static void SetState(TaskbarProgress.TaskbarStates taskbarState)
		{
			if (TaskbarProgress.sMainWindowHandle != IntPtr.Zero)
			{
				TaskbarProgress.SetState(TaskbarProgress.sMainWindowHandle, taskbarState);
			}
		}

		public static void SetState(IntPtr windowHandle, TaskbarProgress.TaskbarStates taskbarState)
		{
			if (TaskbarProgress.sTaskbarSupported)
			{
				try
				{
					TaskbarProgress.sTaskbarInstance.SetProgressState(windowHandle, taskbarState);
				}
				catch (COMException)
				{
				}
			}
		}

		public static void SetValue(double progressValue, double progressMax)
		{
			if (TaskbarProgress.sMainWindowHandle != IntPtr.Zero)
			{
				TaskbarProgress.SetValue(TaskbarProgress.sMainWindowHandle, progressValue, progressMax);
			}
		}

		public static void SetValue(IntPtr windowHandle, double progressValue, double progressMax)
		{
			if (TaskbarProgress.sTaskbarSupported)
			{
				try
				{
					TaskbarProgress.sTaskbarInstance.SetProgressValue(windowHandle, (ulong)progressValue, (ulong)progressMax);
				}
				catch (COMException)
				{
				}
			}
		}

		[Conditional("DEBUG")]
		private static void DebugInfo(COMException e)
		{
		}
	}
}
