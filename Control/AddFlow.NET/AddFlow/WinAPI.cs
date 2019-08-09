using System;
using System.Runtime.InteropServices;

namespace Lassalle.Flow {
    /// <summary>
    /// WinAPI 的摘要说明。
    /// </summary>
    internal class WinAPI {
        public WinAPI() {
        }

        //EXTERNALSYM
        public const int GWL_WNDPROC = -4; 
        public const int GWL_HINSTANCE = -6; 
        public const int GWL_HWNDPARENT = -8; 
        public const int GWL_STYLE = -16; 
        public const int GWL_EXSTYLE = -20; 
        public const int GWL_USERDATA = -21; 
        public const int GWL_ID = -12;


        public const int WM_SETFOCUS = 7;

        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr GetFocus();

        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern IntPtr SetFocus(HandleRef hWnd);

        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool IsChild(HandleRef hWndParent, HandleRef hwnd);

        public const int WHEEL_DELTA      = 120;
        public const uint WHEEL_PAGESCROLL = System.UInt32.MaxValue;

        public const int WM_SETTINGCHANGE = 26;
        public const int WM_HSCROLL = 276;
        public const int WM_VSCROLL = 277;

        //Win NC Message
        public const int WM_NCCREATE         = 0x0081;
        public const int WM_NCDESTROY        = 0x0082;
        public const int WM_NCCALCSIZE       = 0x0083;
        public const int WM_NCHITTEST        = 0x0084;
        public const int WM_NCPAINT          = 0x0085;
        public const int WM_NCACTIVATE       = 0x0086;
        public const int WM_GETDLGCODE       = 0x0087;
        public const int WM_NCMOUSEMOVE      = 0x00A0;
        public const int WM_NCLBUTTONDOWN    = 0x00A1;
        public const int WM_NCLBUTTONUP      = 0x00A2;
        public const int WM_NCLBUTTONDBLCLK  = 0x00A3;
        public const int WM_NCRBUTTONDOWN    = 0x00A4;
        public const int WM_NCRBUTTONUP      = 0x00A5;
        public const int WM_NCRBUTTONDBLCLK  = 0x00A6;
        public const int WM_NCMBUTTONDOWN    = 0x00A7;
        public const int WM_NCMBUTTONUP      = 0x00A8;
        public const int WM_NCMBUTTONDBLCLK  = 0x00A9;

        //WM_NCHITTEST and MOUSEHOOKSTRUCT Mouse Position Codes
        public const int HTERROR = -2;
        public const int HTTRANSPARENT = -1;
        public const int HTNOWHERE = 0;
        public const int HTCLIENT = 1;
        public const int HTCAPTION = 2;
        public const int HTSYSMENU = 3;
        public const int HTGROWBOX = 4;
        public const int HTSIZE = HTGROWBOX;
        public const int HTMENU = 5;
        public const int HTHSCROLL = 6;
        public const int HTVSCROLL = 7;
        public const int HTMINBUTTON = 8;
        public const int HTMAXBUTTON = 9;
        public const int HTLEFT = 10;
        public const int HTRIGHT = 11;
        public const int HTTOP = 12;
        public const int HTTOPLEFT = 13;
        public const int HTTOPRIGHT = 14;
        public const int HTBOTTOM = 15;
        public const int HTBOTTOMLEFT = 16;
        public const int HTBOTTOMRIGHT = 17;
        public const int HTBORDER = 18;
        public const int HTREDUCE = HTMINBUTTON;
        public const int HTZOOM = HTMAXBUTTON;
        public const int HTSIZEFIRST = HTLEFT;
        public const int HTSIZELAST = HTBOTTOMRIGHT;
        public const int HTOBJECT = 19;
        public const int HTCLOSE = 20;
        public const int HTHELP = 21;

        // Window Styles 
        public const int WS_OVERLAPPED = 0;
        public const int WS_POPUP = -2147483648; //0x80000000;
        public const int WS_CHILD = 0x40000000;
        public const int WS_MINIMIZE = 0x20000000;
        public const int WS_VISIBLE = 0x10000000;
        public const int WS_DISABLED = 0x8000000;
        public const int WS_CLIPSIBLINGS = 0x4000000;
        public const int WS_CLIPCHILDREN = 0x2000000;
        public const int WS_MAXIMIZE = 0x1000000;
        public const int WS_CAPTION = 0xC00000; // WS_BORDER | WS_DLGFRAME  
        public const int WS_BORDER = 0x800000;
        public const int WS_DLGFRAME = 0x400000;
        public const int WS_VSCROLL = 0x200000;
        public const int WS_HSCROLL = 0x100000;
        public const int WS_SYSMENU = 0x80000;
        public const int WS_THICKFRAME = 0x40000;
        public const int WS_GROUP = 0x20000;
        public const int WS_TABSTOP = 0x10000;

        public const int WS_MINIMIZEBOX = 0x20000;
        public const int WS_MAXIMIZEBOX = 0x10000;

        public const int WS_TILED = WS_OVERLAPPED;
        public const int WS_ICONIC = WS_MINIMIZE;
        public const int WS_SIZEBOX = WS_THICKFRAME;

        // Common Window Styles 
        public const int WS_OVERLAPPEDWINDOW = 
            (WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX);
        public const int WS_TILEDWINDOW = WS_OVERLAPPEDWINDOW;
        public const int WS_POPUPWINDOW = (WS_POPUP | WS_BORDER | WS_SYSMENU);
        public const int WS_CHILDWINDOW = (WS_CHILD);

        // Extended Window Styles 
        public const int WS_EX_DLGMODALFRAME = 1;
        public const int WS_EX_NOPARENTNOTIFY = 4;
        public const int WS_EX_TOPMOST = 8;
        public const int WS_EX_ACCEPTFILES = 0x10;
        public const int WS_EX_TRANSPARENT = 0x20;
        public const int WS_EX_MDICHILD = 0x40;
        public const int WS_EX_TOOLWINDOW = 0x80;
        public const int WS_EX_WINDOWEDGE = 0x100;
        public const int WS_EX_CLIENTEDGE = 0x200;
        public const int WS_EX_CONTEXTHELP = 0x400;

        public const int WS_EX_RIGHT = 0x1000;
        public const int WS_EX_LEFT = 0;
        public const int WS_EX_RTLREADING = 0x2000;
        public const int WS_EX_LTRREADING = 0;
        public const int WS_EX_LEFTSCROLLBAR = 0x4000;
        public const int WS_EX_RIGHTSCROLLBAR = 0;

        public const int WS_EX_CONTROLPARENT = 0x10000;
        public const int WS_EX_STATICEDGE = 0x20000;
        public const int WS_EX_APPWINDOW = 0x40000;
        public const int WS_EX_OVERLAPPEDWINDOW = (WS_EX_WINDOWEDGE | WS_EX_CLIENTEDGE);
        public const int WS_EX_PALETTEWINDOW = (WS_EX_WINDOWEDGE | WS_EX_TOOLWINDOW | WS_EX_TOPMOST);

        public const int WS_EX_LAYERED = 0x00080000;
        public const int WS_EX_NOINHERITLAYOUT = 0x00100000; // Disable inheritence of mirroring by children
        public const int WS_EX_LAYOUTRTL = 0x00400000; // Right to left mirroring
        public const int WS_EX_NOACTIVATE = 0x08000000;

        // Class styles 
        public const int CS_VREDRAW = 1;//DWORD(1);
        public const int CS_HREDRAW = 2;//DWORD(2);
        public const int CS_KEYCVTWINDOW = 4;
        public const int CS_DBLCLKS = 8;
        public const int CS_OWNDC = 0x20;
        public const int CS_CLASSDC = 0x40;
        public const int CS_PARENTDC = 0x80;
        public const int CS_NOKEYCVT = 0x100;
        public const int CS_NOCLOSE = 0x200;
        public const int CS_SAVEBITS = 0x800;
        public const int CS_BYTEALIGNCLIENT = 0x1000;
        public const int CS_BYTEALIGNWINDOW = 0x2000;
        public const int CS_GLOBALCLASS = 0x4000;
        public const int CS_IME = 0x10000;

        // WM_PRINT flags 
        public const int PRF_CHECKVISIBLE = 1;
        public const int PRF_NONCLIENT = 2;
        public const int PRF_CLIENT = 4;
        public const int PRF_ERASEBKGND = 8;
        public const int PRF_CHILDREN = 0x10;
        public const int PRF_OWNED = 0x20;

        // 3D border styles 
        public const int BDR_RAISEDOUTER = 1;
        public const int BDR_SUNKENOUTER = 2;
        public const int BDR_RAISEDINNER = 4;
        public const int BDR_SUNKENINNER = 8;

        public const int BDR_OUTER = 3;
        public const int BDR_INNER = 12;
        public const int BDR_RAISED = 5;
        public const int BDR_SUNKEN = 10;

        public const int EDGE_RAISED = (BDR_RAISEDOUTER | BDR_RAISEDINNER);
        public const int EDGE_SUNKEN = (BDR_SUNKENOUTER | BDR_SUNKENINNER);
        public const int EDGE_ETCHED = (BDR_SUNKENOUTER | BDR_RAISEDINNER);
        public const int EDGE_BUMP = (BDR_RAISEDOUTER | BDR_SUNKENINNER);

        // Border flags 
        public const int BF_LEFT = 1;
        public const int BF_TOP = 2;
        public const int BF_RIGHT = 4;
        public const int BF_BOTTOM = 8;
        public const int BF_TOPLEFT = (BF_TOP | BF_LEFT);
        public const int BF_TOPRIGHT = (BF_TOP | BF_RIGHT);
        public const int BF_BOTTOMLEFT = (BF_BOTTOM | BF_LEFT);
        public const int BF_BOTTOMRIGHT = (BF_BOTTOM | BF_RIGHT);
        public const int BF_RECT = (BF_LEFT | BF_TOP | BF_RIGHT | BF_BOTTOM);
        public const int BF_DIAGONAL = 0x10;

        // For diagonal lines, the BF_RECT flags specify the end point of the
        // vector bounded by the rectangle parameter.
        public const int BF_DIAGONAL_ENDTOPRIGHT = (BF_DIAGONAL | BF_TOP | BF_RIGHT);
        public const int BF_DIAGONAL_ENDTOPLEFT = (BF_DIAGONAL | BF_TOP | BF_LEFT);
        public const int BF_DIAGONAL_ENDBOTTOMLEFT = (BF_DIAGONAL | BF_BOTTOM | BF_LEFT);
        public const int BF_DIAGONAL_ENDBOTTOMRIGHT = (BF_DIAGONAL | BF_BOTTOM | BF_RIGHT);
        public const int BF_MIDDLE = 0x800;   // Fill in the middle 
        public const int BF_SOFT = 0x1000;    // For softer buttons 
        public const int BF_ADJUST = 0x2000;  // Calculate the space left over 
        public const int BF_FLAT = 0x4000;    // For flat rather than 3D borders 
        public const int BF_MONO = 0x8000;    // For monochrome borders 


        //Scroll
        public const int SB_HORZ = 0;
        public const int SB_VERT = 1;
        public const int SB_CTL = 2;
        public const int SB_BOTH = 3;

        public const int SIF_RANGE = 1;
        public const int SIF_PAGE = 2;
        public const int SIF_POS = 4;
        public const int SIF_DISABLENOSCROLL = 8;
        public const int SIF_TRACKPOS = 0x10;
        public const int SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS);

        public const int SB_LINEUP = 0;
        public const int SB_LINELEFT = 0;
        public const int SB_LINEDOWN = 1;
        public const int SB_LINERIGHT = 1;
        public const int SB_PAGEUP = 2;
        public const int SB_PAGELEFT = 2;
        public const int SB_PAGEDOWN = 3;
        public const int SB_PAGERIGHT = 3;
        public const int SB_THUMBPOSITION = 4;
        public const int SB_THUMBTRACK = 5;
        public const int SB_TOP = 6;
        public const int SB_LEFT = 6;
        public const int SB_BOTTOM = 7;
        public const int SB_RIGHT = 7;
        public const int SB_ENDSCROLL = 8;

        //[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        //public static extern int GetWindowLongA(HandleRef hWnd, int index);

        [DllImport("user32.dll", EntryPoint="GetWindowLong")]
        public static extern int GetWindowLong (HandleRef hWnd, int nIndex);

        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern bool GetScrollInfo(HandleRef hWnd, int fnBar, [In, Out] WinAPI.SCROLLINFO si);
 
        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int SetScrollInfo(HandleRef hWnd, int fnBar, SCROLLINFO si, bool redraw);

        [DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        public static extern int SetScrollPos(HandleRef hWnd, int nBar, int nPos, bool redraw);

        [StructLayout(LayoutKind.Sequential)]
            public class SCROLLINFO {
            public SCROLLINFO() {
                this.cbSize = Marshal.SizeOf(typeof(WinAPI.SCROLLINFO));
            }
 
            public SCROLLINFO(int mask, int min, int max, int page, int pos) {
                this.cbSize = Marshal.SizeOf(typeof(WinAPI.SCROLLINFO));
                this.fMask = mask;
                this.nMin = min;
                this.nMax = max;
                this.nPage = page;
                this.nPos = pos;
            }
 
            public int cbSize;
            public int fMask;
            public int nMin;
            public int nMax;
            public int nPage;
            public int nPos;
            public int nTrackPos;
        }

        public static int SendGetStructMessage(uint Handle, uint Msg, int WParam, ref object LParam, bool InitFromLParam /* = false */) {
            int num2;
            uint num1 = (uint) Marshal.SizeOf(LParam);
            IntPtr ptr1 = Marshal.AllocHGlobal((int) num1);
            try {
                if (InitFromLParam) {
                    Marshal.StructureToPtr(LParam, ptr1, false);
                }
                num2 = SendMessage(Handle, Msg, WParam, (int) ptr1);
                LParam = Marshal.PtrToStructure(ptr1, LParam.GetType());
            }
            finally {
                Marshal.DestroyStructure(ptr1, LParam.GetType());
            }
            return num2;
        }
 
        public static int SendStructMessage(uint Handle, uint Msg, int WParam, [In] object LParam) {
            int num1;
            IntPtr ptr1 = Marshal.AllocHGlobal(Marshal.SizeOf(LParam));
            try {
                Marshal.StructureToPtr(LParam, ptr1, false);
                num1 = SendMessage(Handle, Msg, WParam, (int) ptr1);
            }
            finally {
                Marshal.DestroyStructure(ptr1, LParam.GetType());
            }
            return num1;
        }

        //public const int WM_SETFOCUS = 7;

        //[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        //public static extern IntPtr GetFocus();
        //
        //[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        //public static extern IntPtr SetFocus(HandleRef hWnd);
        //
        //[DllImport("user32.dll", CharSet=CharSet.Auto, ExactSpelling=true)]
        //public static extern bool IsChild(HandleRef hWndParent, HandleRef hwnd);


        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto, Pack=1)]
            public struct LVCOLUMN {
            public int mask;
            public int fmt;
            public int cx;
            public IntPtr pszText;
            public int cchTextMax;
            public int iSubItem;
            public int iImage;
            public int iOrder;
            public LVCOLUMN(int mask){
                this.mask = mask;                    
                this.fmt = 0;
                this.cx = 0;
                this.pszText = IntPtr.Zero;
                this.cchTextMax = 0;
                this.iSubItem = 0;
                this.iImage = 0;
                this.iOrder = 0;
            }
        }
 
        [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto, Pack=1)]
            public struct LVCOLUMN_T {
            public int mask;
            public int fmt;
            public int cx;
            public string pszText;
            public int cchTextMax;
            public int iSubItem;
            public int iImage;
            public int iOrder;
            public LVCOLUMN_T(int mask){
                this.mask = mask;
                this.fmt = 0;
                this.cx = 0;
                this.pszText = "";
                this.cchTextMax = 0;
                this.iSubItem = 0;
                this.iImage = 0;
                this.iOrder = 0;
            }
        }

        public const int LVCF_FMT     = 0x0001; 
        public const int LVCF_WIDTH   = 0x0002; 
        public const int LVCF_TEXT    = 0x0004; 
        public const int LVCF_SUBITEM = 0x0008; 
        public const int LVCF_IMAGE   = 0x0010; 
        public const int LVCF_ORDER   = 0x0020;

        public const int LVM_FIRST =             0x1000;     // ListView messages
        public const int LVM_GETCOLUMNA =         (LVM_FIRST + 25);
        public const int LVM_GETCOLUMNW =         (LVM_FIRST + 95);
        public const int LVM_GETCOLUMN =         LVM_GETCOLUMNA;
        public const int LVM_SETCOLUMN = 0x101a;

        //#define ListView_GetColumn(hwnd, iCol, pcol) \
        //  (BOOL)SNDMSG((hwnd), LVM_GETCOLUMN, (WPARAM)(int)(iCol), (LPARAM)(LV_COLUMN FAR*)(pcol))
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, out LVCOLUMN_T lParam);
 
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, out LVCOLUMN lParam);
 
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, int wParam, LVCOLUMN lParam);
 
        [DllImport("user32.dll", EntryPoint="SendMessage", CharSet=CharSet.Auto)]
        internal static extern int SendMessage(uint handle, uint Msg, int wParam, int lParam);

        public int ListView_GetColumnPos(System.Windows.Forms.ListView listView, int columnIndex) {
            LVCOLUMN lvCol = new LVCOLUMN();
            lvCol.mask = LVCF_ORDER;
            HandleRef hWnd =new HandleRef(listView, listView.Handle);
            SendMessage(hWnd, LVM_GETCOLUMN, columnIndex , out lvCol);            
            return lvCol.iOrder;
        }


        //**************************************************

        public class Util{
            public Util() {
            }
            private static int GetEmbededNullStringLengthAnsi(string s) {
                int num1 = s.IndexOf('\0');
                if (num1 > -1) {
                    string text1 = s.Substring(0, num1);
                    string text2 = s.Substring((num1 + 1));
                    return ((WinAPI.Util.GetPInvokeStringLength(text1) + WinAPI.Util.GetEmbededNullStringLengthAnsi(text2)) + 1);
                }
                return WinAPI.Util.GetPInvokeStringLength(s);
            }
 
            public static int GetPInvokeStringLength(string s) {
                if (s == null) {
                    return 0;
                }
                if (Marshal.SystemDefaultCharSize == 2) {
                    return s.Length;
                }
                if (s.Length == 0) {
                    return 0;
                }
                if (s.IndexOf('\0') > -1) {
                    return WinAPI.Util.GetEmbededNullStringLengthAnsi(s);
                }
                return WinAPI.Util.lstrlen(s);
            }
 
            public static int HIWORD(int n) {
                return ((n >> 16) & 65535);
            }
 
            public static int HIWORD(IntPtr n) {
                return WinAPI.Util.HIWORD(((int) n));
            }
 
            public static int LOWORD(int n) {
                return (n & 65535);
            }
 
            public static int LOWORD(IntPtr n) {
                return WinAPI.Util.LOWORD(((int) n));
            }
 
            [DllImport("kernel32.dll", CharSet=CharSet.Auto)]
            private static extern int lstrlen(string s);
 
            public static int MAKELONG(int low, int high) {
                return ((high << 16) | (low & 65535));
            }
 
            public static IntPtr MAKELPARAM(int low, int high) {
                return ((IntPtr) ((high << 16) | (low & 65535)));
            }
 
            [DllImport("user32.dll", CharSet=CharSet.Auto)]
            internal static extern int RegisterWindowMessage(string msg);
 
            public static int SignedHIWORD(int n) {
                return ((short) ((n >> 16) & 65535));
            }
 
            public static int SignedHIWORD(IntPtr n) {
                return WinAPI.Util.SignedHIWORD(((int) n));
            }
 
            public static int SignedLOWORD(int n) {
                return ((short) (n & 65535));
            }
 
            public static int SignedLOWORD(IntPtr n) {
                return WinAPI.Util.SignedLOWORD(((int) n));
            }
 
            public struct RECT{
                public RECT(int left, int top, int right, int bottom) {
                    this.left = left;
                    this.top = top;
                    this.right = right;
                    this.bottom = bottom;
                }
 
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            public struct WINDOWPOS{
                long hwnd;
                long hwndInsertAfter;
                int x;
                int y;
                int cx;
                int cy;
                uint flags;
            }

            //            public struct NCCALCSIZE_PARAMS{
            //                RECT[] rgrc = new RECT[3];
            //                WINDOWPOS lppos;
            //            }
        }
        //**************************************************


    }



}



