using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;

namespace Vector.VLConfig.GUI.Common
{
	public class FolderBrowserDialogEx : CommonDialog
	{
		private class CSIDL
		{
			public const int PRINTERS = 4;

			public const int NETWORK = 18;
		}

		private class BrowseFlags
		{
			public const int BIF_DEFAULT = 0;

			public const int BIF_BROWSEFORCOMPUTER = 4096;

			public const int BIF_BROWSEFORPRINTER = 8192;

			public const int BIF_BROWSEINCLUDEFILES = 16384;

			public const int BIF_BROWSEINCLUDEURLS = 128;

			public const int BIF_DONTGOBELOWDOMAIN = 2;

			public const int BIF_EDITBOX = 16;

			public const int BIF_NEWDIALOGSTYLE = 64;

			public const int BIF_NONEWFOLDERBUTTON = 512;

			public const int BIF_RETURNFSANCESTORS = 8;

			public const int BIF_RETURNONLYFSDIRS = 1;

			public const int BIF_SHAREABLE = 32768;

			public const int BIF_STATUSTEXT = 4;

			public const int BIF_UAHINT = 256;

			public const int BIF_VALIDATE = 32;

			public const int BIF_NOTRANSLATETARGETS = 1024;
		}

		private static class BrowseForFolderMessages
		{
			public const int BFFM_INITIALIZED = 1;

			public const int BFFM_SELCHANGED = 2;

			public const int BFFM_VALIDATEFAILEDA = 3;

			public const int BFFM_VALIDATEFAILEDW = 4;

			public const int BFFM_IUNKNOWN = 5;

			public const int BFFM_SETSTATUSTEXT = 1124;

			public const int BFFM_ENABLEOK = 1125;

			public const int BFFM_SETSELECTIONA = 1126;

			public const int BFFM_SETSELECTIONW = 1127;
		}

		private static readonly int MAX_PATH = 260;

		private PInvoke.BrowseFolderCallbackProc _callback;

		private string _descriptionText;

		private Environment.SpecialFolder _rootFolder;

		private string _selectedPath;

		private bool _selectedPathNeedsCheck;

		private bool _showNewFolderButton;

		private bool _showEditBox;

		private bool _showBothFilesAndFolders;

		private bool _newStyle = true;

		private bool _showFullPathInEditBox = true;

		private bool _dontIncludeNetworkFoldersBelowDomainLevel;

		private int _uiFlags;

		private IntPtr _hwndEdit;

		private IntPtr _rootFolderLocation;

		public new event EventHandler HelpRequest
		{
			add
			{
				base.HelpRequest += value;
			}
			remove
			{
				base.HelpRequest -= value;
			}
		}

		public string Description
		{
			get
			{
				return this._descriptionText;
			}
			set
			{
				this._descriptionText = ((value == null) ? string.Empty : value);
			}
		}

		public Environment.SpecialFolder RootFolder
		{
			get
			{
				return this._rootFolder;
			}
			set
			{
				if (!Enum.IsDefined(typeof(Environment.SpecialFolder), value))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(Environment.SpecialFolder));
				}
				this._rootFolder = value;
			}
		}

		public string SelectedPath
		{
			get
			{
				if (this._selectedPath != null && this._selectedPath.Length != 0 && this._selectedPathNeedsCheck)
				{
					new FileIOPermission(FileIOPermissionAccess.PathDiscovery, this._selectedPath).Demand();
					this._selectedPathNeedsCheck = false;
				}
				return this._selectedPath;
			}
			set
			{
				this._selectedPath = ((value == null) ? string.Empty : value);
				this._selectedPathNeedsCheck = true;
			}
		}

		public bool ShowNewFolderButton
		{
			get
			{
				return this._showNewFolderButton;
			}
			set
			{
				this._showNewFolderButton = value;
			}
		}

		public bool ShowEditBox
		{
			get
			{
				return this._showEditBox;
			}
			set
			{
				this._showEditBox = value;
			}
		}

		public bool NewStyle
		{
			get
			{
				return this._newStyle;
			}
			set
			{
				this._newStyle = value;
			}
		}

		public bool DontIncludeNetworkFoldersBelowDomainLevel
		{
			get
			{
				return this._dontIncludeNetworkFoldersBelowDomainLevel;
			}
			set
			{
				this._dontIncludeNetworkFoldersBelowDomainLevel = value;
			}
		}

		public bool ShowFullPathInEditBox
		{
			get
			{
				return this._showFullPathInEditBox;
			}
			set
			{
				this._showFullPathInEditBox = value;
			}
		}

		public bool ShowBothFilesAndFolders
		{
			get
			{
				return this._showBothFilesAndFolders;
			}
			set
			{
				this._showBothFilesAndFolders = value;
			}
		}

		public FolderBrowserDialogEx()
		{
			this.Reset();
		}

		public static FolderBrowserDialogEx PrinterBrowser()
		{
			FolderBrowserDialogEx folderBrowserDialogEx = new FolderBrowserDialogEx();
			folderBrowserDialogEx.BecomePrinterBrowser();
			return folderBrowserDialogEx;
		}

		public static FolderBrowserDialogEx ComputerBrowser()
		{
			FolderBrowserDialogEx folderBrowserDialogEx = new FolderBrowserDialogEx();
			folderBrowserDialogEx.BecomeComputerBrowser();
			return folderBrowserDialogEx;
		}

		private void BecomePrinterBrowser()
		{
			this._uiFlags += 8192;
			this.Description = "Select a printer:";
			PInvoke.Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, 4, ref this._rootFolderLocation);
			this.ShowNewFolderButton = false;
			this.ShowEditBox = false;
		}

		private void BecomeComputerBrowser()
		{
			this._uiFlags += 4096;
			this.Description = "Select a computer:";
			PInvoke.Shell32.SHGetSpecialFolderLocation(IntPtr.Zero, 18, ref this._rootFolderLocation);
			this.ShowNewFolderButton = false;
			this.ShowEditBox = false;
		}

		private int FolderBrowserCallback(IntPtr hwnd, int msg, IntPtr lParam, IntPtr lpData)
		{
			switch (msg)
			{
			case 1:
				if (this._selectedPath.Length != 0)
				{
					PInvoke.User32.SendMessage(new HandleRef(null, hwnd), 1127, 1, this._selectedPath);
					if (this._showEditBox && this._showFullPathInEditBox)
					{
						this._hwndEdit = PInvoke.User32.FindWindowEx(new HandleRef(null, hwnd), IntPtr.Zero, "Edit", null);
						PInvoke.User32.SetWindowText(this._hwndEdit, this._selectedPath);
					}
				}
				break;
			case 2:
				if (lParam != IntPtr.Zero)
				{
					if ((this._uiFlags & 8192) == 8192 || (this._uiFlags & 4096) == 4096)
					{
						PInvoke.User32.SendMessage(new HandleRef(null, hwnd), 1125, 0, 1);
					}
					else
					{
						IntPtr intPtr = Marshal.AllocHGlobal(FolderBrowserDialogEx.MAX_PATH * Marshal.SystemDefaultCharSize);
						bool flag = PInvoke.Shell32.SHGetPathFromIDList(lParam, intPtr);
						string text = Marshal.PtrToStringAuto(intPtr);
						Marshal.FreeHGlobal(intPtr);
						PInvoke.User32.SendMessage(new HandleRef(null, hwnd), 1125, 0, flag ? 1 : 0);
						if (flag && !string.IsNullOrEmpty(text))
						{
							if (this._showEditBox && this._showFullPathInEditBox && this._hwndEdit != IntPtr.Zero)
							{
								PInvoke.User32.SetWindowText(this._hwndEdit, text);
							}
							if ((this._uiFlags & 4) == 4)
							{
								PInvoke.User32.SendMessage(new HandleRef(null, hwnd), 1124, 0, text);
							}
						}
					}
				}
				break;
			}
			return 0;
		}

		private static PInvoke.IMalloc GetSHMalloc()
		{
			PInvoke.IMalloc[] array = new PInvoke.IMalloc[1];
			PInvoke.Shell32.SHGetMalloc(array);
			return array[0];
		}

		public override void Reset()
		{
			this._rootFolder = Environment.SpecialFolder.Desktop;
			this._descriptionText = string.Empty;
			this._selectedPath = string.Empty;
			this._selectedPathNeedsCheck = false;
			this._showNewFolderButton = true;
			this._showEditBox = true;
			this._newStyle = true;
			this._dontIncludeNetworkFoldersBelowDomainLevel = false;
			this._hwndEdit = IntPtr.Zero;
			this._rootFolderLocation = IntPtr.Zero;
		}

		protected override bool RunDialog(IntPtr hWndOwner)
		{
			bool result = false;
			if (this._rootFolderLocation == IntPtr.Zero)
			{
				PInvoke.Shell32.SHGetSpecialFolderLocation(hWndOwner, (int)this._rootFolder, ref this._rootFolderLocation);
				if (this._rootFolderLocation == IntPtr.Zero)
				{
					PInvoke.Shell32.SHGetSpecialFolderLocation(hWndOwner, 0, ref this._rootFolderLocation);
					if (this._rootFolderLocation == IntPtr.Zero)
					{
						throw new InvalidOperationException("FolderBrowserDialogNoRootFolder");
					}
				}
			}
			this._hwndEdit = IntPtr.Zero;
			if (this._dontIncludeNetworkFoldersBelowDomainLevel)
			{
				this._uiFlags += 2;
			}
			if (this._newStyle)
			{
				this._uiFlags += 64;
			}
			if (!this._showNewFolderButton)
			{
				this._uiFlags += 512;
			}
			if (this._showEditBox)
			{
				this._uiFlags += 16;
			}
			if (this._showBothFilesAndFolders)
			{
				this._uiFlags += 16384;
			}
			if (Control.CheckForIllegalCrossThreadCalls && Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException("DebuggingException: ThreadMustBeSTA");
			}
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			try
			{
				PInvoke.BROWSEINFO bROWSEINFO = new PInvoke.BROWSEINFO();
				intPtr2 = Marshal.AllocHGlobal(FolderBrowserDialogEx.MAX_PATH * Marshal.SystemDefaultCharSize);
				intPtr3 = Marshal.AllocHGlobal(FolderBrowserDialogEx.MAX_PATH * Marshal.SystemDefaultCharSize);
				this._callback = new PInvoke.BrowseFolderCallbackProc(this.FolderBrowserCallback);
				bROWSEINFO.pidlRoot = this._rootFolderLocation;
				bROWSEINFO.Owner = hWndOwner;
				bROWSEINFO.pszDisplayName = intPtr2;
				bROWSEINFO.Title = this._descriptionText;
				bROWSEINFO.Flags = this._uiFlags;
				bROWSEINFO.callback = this._callback;
				bROWSEINFO.lParam = IntPtr.Zero;
				bROWSEINFO.iImage = 0;
				intPtr = PInvoke.Shell32.SHBrowseForFolder(bROWSEINFO);
				if ((this._uiFlags & 8192) == 8192 || (this._uiFlags & 4096) == 4096)
				{
					this._selectedPath = Marshal.PtrToStringAuto(bROWSEINFO.pszDisplayName);
					result = true;
				}
				else if (intPtr != IntPtr.Zero)
				{
					PInvoke.Shell32.SHGetPathFromIDList(intPtr, intPtr3);
					this._selectedPathNeedsCheck = true;
					this._selectedPath = Marshal.PtrToStringAuto(intPtr3);
					result = true;
				}
			}
			finally
			{
				PInvoke.IMalloc sHMalloc = FolderBrowserDialogEx.GetSHMalloc();
				sHMalloc.Free(this._rootFolderLocation);
				this._rootFolderLocation = IntPtr.Zero;
				if (intPtr != IntPtr.Zero)
				{
					sHMalloc.Free(intPtr);
				}
				if (intPtr3 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr3);
				}
				if (intPtr2 != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(intPtr2);
				}
				this._callback = null;
			}
			return result;
		}
	}
}
