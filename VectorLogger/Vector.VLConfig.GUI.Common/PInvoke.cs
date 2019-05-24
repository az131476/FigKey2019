using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Vector.VLConfig.GUI.Common
{
	internal static class PInvoke
	{
		public delegate int BrowseFolderCallbackProc(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData);

		internal static class User32
		{
			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, string lParam);

			[DllImport("user32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, int lParam);

			[DllImport("user32.dll", SetLastError = true)]
			public static extern IntPtr FindWindowEx(HandleRef hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

			[DllImport("user32.dll", SetLastError = true)]
			public static extern bool SetWindowText(IntPtr hWnd, string text);
		}

		[Guid("00000002-0000-0000-c000-000000000046"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), SuppressUnmanagedCodeSecurity]
		[ComImport]
		public interface IMalloc
		{
			[PreserveSig]
			IntPtr Alloc(int cb);

			[PreserveSig]
			IntPtr Realloc(IntPtr pv, int cb);

			[PreserveSig]
			void Free(IntPtr pv);

			[PreserveSig]
			int GetSize(IntPtr pv);

			[PreserveSig]
			int DidAlloc(IntPtr pv);

			[PreserveSig]
			void HeapMinimize();
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
		public class BROWSEINFO
		{
			public IntPtr Owner;

			public IntPtr pidlRoot;

			public IntPtr pszDisplayName;

			public string Title;

			public int Flags;

			public PInvoke.BrowseFolderCallbackProc callback;

			public IntPtr lParam;

			public int iImage;
		}

		[SuppressUnmanagedCodeSecurity]
		internal static class Shell32
		{
			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
			public static extern IntPtr SHBrowseForFolder([In] PInvoke.BROWSEINFO lpbi);

			[DllImport("shell32.dll")]
			public static extern int SHGetMalloc([MarshalAs(UnmanagedType.LPArray)] [Out] PInvoke.IMalloc[] ppMalloc);

			[DllImport("shell32.dll", CharSet = CharSet.Auto)]
			public static extern bool SHGetPathFromIDList(IntPtr pidl, IntPtr pszPath);

			[DllImport("shell32.dll")]
			public static extern int SHGetSpecialFolderLocation(IntPtr hwnd, int csidl, ref IntPtr ppidl);
		}

		static PInvoke()
		{
		}
	}
}
