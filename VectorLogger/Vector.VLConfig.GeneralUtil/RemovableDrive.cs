using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace Vector.VLConfig.GeneralUtil
{
	public class RemovableDrive
	{
		private const int INVALID_HANDLE_VALUE = -1;

		private const int GENERIC_READ = -2147483648;

		private const int GENERIC_WRITE = 1073741824;

		private const int FILE_SHARE_READ = 1;

		private const int FILE_SHARE_WRITE = 2;

		private const int OPEN_EXISTING = 3;

		private const int FSCTL_LOCK_VOLUME = 589848;

		private const int FSCTL_DISMOUNT_VOLUME = 589856;

		private const int IOCTL_STORAGE_EJECT_MEDIA = 2967560;

		private const int IOCTL_STORAGE_MEDIA_REMOVAL = 2967556;

		private const int CreateFile_NO_SUCCESS = -1;

		private static bool driveEjected = false;

		private static Cursor cursor = Cursors.WaitCursor;

		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, EntryPoint = "CreateFileW", SetLastError = true)]
		private static extern IntPtr CreateFile(string lpFileName, int dwDesiredAccess, int dwShareMode, IntPtr lpSecurityAttributes, int dwCreationDisposition, int dwFlagsAndAttributes, IntPtr hTemplateFile);

		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool CloseHandle(IntPtr handle);

		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool DeviceIoControl(IntPtr hDevice, int dwIoControlCode, byte[] lpInBuffer, int nInBufferSize, byte[] lpOutBuffer, int nOutBufferSize, out int lpBytesReturned, IntPtr lpOverlapped);

		public static bool Eject(string driveName)
		{
			if (driveName.Length > 2)
			{
				driveName = driveName.Substring(0, 2);
			}
			else if (driveName.Length == 1)
			{
				driveName += ":";
			}
			Thread thread = new Thread(new ParameterizedThreadStart(RemovableDrive.EjectDrive));
			thread.Start(driveName);
			while (thread.IsAlive)
			{
				Cursor.Current = RemovableDrive.cursor;
				Thread.Sleep(1000);
				Application.DoEvents();
			}
			return RemovableDrive.driveEjected;
		}

		public static bool Eject(string driveName, Cursor cursor)
		{
			Cursor.Current = cursor;
			RemovableDrive.cursor = cursor;
			RemovableDrive.Eject(driveName);
			RemovableDrive.cursor = Cursors.Default;
			Cursor.Current = RemovableDrive.cursor;
			return RemovableDrive.driveEjected;
		}

		private static void EjectDrive(object driveName)
		{
			bool ok = false;
			bool flag = false;
			IntPtr intPtr;
			if (!RemovableDrive.OpenVolume(driveName.ToString(), out intPtr))
			{
				RemovableDrive.driveEjected = false;
				return;
			}
			int num;
			if (RemovableDrive.LockVolume(ok, intPtr) && RemovableDrive.DismountVolume(ref ok, intPtr, out num))
			{
				flag = true;
				if (RemovableDrive.PreventRemovalOfVolume(ok, intPtr))
				{
					RemovableDrive.EjectMedia(ref ok, intPtr, ref num);
				}
			}
			ok = RemovableDrive.CloseHandle(intPtr);
			RemovableDrive.driveEjected = flag;
		}

		private static void EjectMedia(ref bool ok, IntPtr h, ref int xout)
		{
			ok = RemovableDrive.DeviceIoControl(h, 2967560, null, 0, null, 0, out xout, IntPtr.Zero);
		}

		private static bool PreventRemovalOfVolume(bool ok, IntPtr h)
		{
			byte[] lpInBuffer = new byte[]
			{
				0
			};
			int num = 0;
			return RemovableDrive.DeviceIoControl(h, 2967556, lpInBuffer, 1, null, 0, out num, IntPtr.Zero);
		}

		private static bool DismountVolume(ref bool ok, IntPtr h, out int xout)
		{
			xout = 0;
			ok = RemovableDrive.DeviceIoControl(h, 589856, null, 0, null, 0, out xout, IntPtr.Zero);
			return ok;
		}

		private static bool LockVolume(bool ok, IntPtr h)
		{
			for (int i = 0; i < 10; i++)
			{
				int num = 0;
				ok = RemovableDrive.DeviceIoControl(h, 589848, null, 0, null, 0, out num, IntPtr.Zero);
				if (ok)
				{
					break;
				}
				Thread.Sleep(500);
			}
			return ok;
		}

		private static bool OpenVolume(string driveName, out IntPtr h)
		{
			h = RemovableDrive.CreateFile("\\\\.\\" + driveName, -2147483648, 3, IntPtr.Zero, 3, 0, IntPtr.Zero);
			return h.ToInt32() != -1;
		}
	}
}
