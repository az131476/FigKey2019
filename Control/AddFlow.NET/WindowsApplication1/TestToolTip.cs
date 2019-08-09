//using System;
//using System.Drawing;
//using System.Collections;
//using System.ComponentModel;
//using System.Windows.Forms;
//using System.Windows.Forms.Design;
//using System.ComponentModel.Design;
//using System.Drawing.Drawing2D;
//using System.Globalization;
//using System.Security.Cryptography;
//using System.Text;
//using System.IO;D:\work\project\FigKey\Control\AddFlow.NET\WindowsApplication1\TestToolTip.cs
//using System.Xml;
//using System.Reflection;
//using System.ComponentModel.Design.Serialization;
//using System.Net;
//
//namespace WindowsApplication1
//{
//    [ToolboxItemFilter("System.Windows.Forms"), ProvideProperty("ToolTip", typeof(Control))]
//    public sealed class ToolTip : Component, IExtenderProvider {
//        private class ToolTipNativeWindow : NativeWindow {
//            // Methods
//            internal ToolTipNativeWindow(ToolTip control) {
//                this.control = control;
//            }
// 
//            protected override void WndProc(ref Message m) {
//                if (this.control != null) {
//                    this.control.WndProc(ref m);
//                }
//            }
// 
//            // Fields
//            private ToolTip control;
//        }
// 
//        public ToolTip() {
//            this.tools = new Hashtable();
//            this.delayTimes = new int[4];
//            this.auto = true;
//            this.showAlways = false;
//            this.window = null;
//            this.topLevelControl = null;
//            this.active = true;
//            this.created = new Hashtable();
//            this.window = new ToolTip.ToolTipNativeWindow(this);
//            this.auto = true;
//            this.delayTimes[0] = 500;
//            this.AdjustBaseFromAuto();
//        }
// 
//        public ToolTip(IContainer cont) : this() {
//            cont.Add(this);
//        }
// 
//        private void AdjustBaseFromAuto() {
//            this.delayTimes[1] = this.delayTimes[0] / 5;
//            this.delayTimes[2] = this.delayTimes[0] * 10;
//            this.delayTimes[3] = this.delayTimes[0];
//        }
// 
//        public bool CanExtend(object target) {
//            if ((target is Control) && !(target is ToolTip)) {
//                return true;
//            }
//            return false;
//        }
// 
//        private void CreateAllRegions() {
//            Control[] controlArray1 = new Control[this.tools.Keys.Count];
//            this.tools.Keys.CopyTo(controlArray1, 0);
//            for (int num1 = 0; num1 < controlArray1.Length; num1++) {
//                this.CreateRegion(controlArray1[num1]);
//            }
//        }
// 
//        private void CreateHandle() {
//            if (!this.GetHandleCreated()) {
//                NativeMethods.INITCOMMONCONTROLSEX initcommoncontrolsex1 = new NativeMethods.INITCOMMONCONTROLSEX();
//                initcommoncontrolsex1.dwICC = 8;
//                SafeNativeMethods.InitCommonControlsEx(initcommoncontrolsex1);
//                this.window.CreateHandle(this.CreateParams);
//                SafeNativeMethods.SetWindowPos(new HandleRef(this, this.Handle), NativeMethods.HWND_TOPMOST, 0, 0, 0, 0, 0x13);
//                UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 0x418, 0, SystemInformation.MaxWindowTrackSize.Width);
//                if (this.auto) {
//                    this.SetDelayTime(0, this.delayTimes[0]);
//                    this.delayTimes[2] = this.GetDelayTime(2);
//                    this.delayTimes[3] = this.GetDelayTime(3);
//                    this.delayTimes[1] = this.GetDelayTime(1);
//                }
//                else {
//                    for (int num1 = 1; num1 < this.delayTimes.Length; num1++) {
//                        if (this.delayTimes[num1] >= 1) {
//                            this.SetDelayTime(num1, this.delayTimes[num1]);
//                        }
//                    }
//                }
//                UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 0x401, this.active ? 1 : 0, 0);
//            }
//        }
// 
//        private void CreateRegion(Control ctl) {
//            string text1 = this.GetToolTip(ctl);
//            bool flag1 = (text1 != null) && (text1.Length > 0);
//            bool flag2 = (ctl.IsHandleCreated && (this.TopLevelControl != null)) && this.TopLevelControl.IsHandleCreated;
//            if ((!this.created.ContainsKey(ctl) && flag1) && (flag2 && !base.DesignMode)) {
//                int num1 = (int) UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_ADDTOOL, 0, this.GetTOOLINFO(ctl, text1));
//                if (num1 == 0) {
//                    throw new InvalidOperationException(SR.GetString("ToolTipAddFailed"));
//                }
//                this.created[ctl] = ctl;
//            }
//            if (ctl.IsHandleCreated && (this.topLevelControl == null)) {
//                ctl.MouseMove -= new MouseEventHandler(this.MouseMove);
//                ctl.MouseMove += new MouseEventHandler(this.MouseMove);
//            }
//        }
// 
//        private void DestroyHandle() {
//            if (this.GetHandleCreated()) {
//                this.window.DestroyHandle();
//            }
//        }
// 
//        private void DestroyRegion(Control ctl) {
//            bool flag1 = ((ctl.IsHandleCreated && this.GetHandleCreated()) && (this.topLevelControl != null)) && this.topLevelControl.IsHandleCreated;
//            if ((this.created.ContainsKey(ctl) && flag1) && !base.DesignMode) {
//                UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_DELTOOL, 0, this.GetMinTOOLINFO(ctl));
//                this.created.Remove(ctl);
//            }
//        }
// 
//        protected override void Dispose(bool disposing) {
//            this.DestroyHandle();
//            base.Dispose(disposing);
//        }
// 
//        ~ToolTip() {
//            this.DestroyHandle();
//        }
// 
//        private int GetDelayTime(int type) {
//            if (this.GetHandleCreated()) {
//                return (int) UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 0x415, type, 0);
//            }
//            return this.delayTimes[type];
//        }
// 
//        private bool GetHandleCreated() {
//            return (this.window.Handle != IntPtr.Zero);
//        }
// 
//        private NativeMethods.TOOLINFO_T GetMinTOOLINFO(Control ctl) {
//            NativeMethods.TOOLINFO_T toolinfo_t1 = new NativeMethods.TOOLINFO_T();
//            toolinfo_t1.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_T));
//            toolinfo_t1.hwnd = IntPtr.Zero;
//            toolinfo_t1.uFlags |= 1;
//            toolinfo_t1.uId = ctl.Handle;
//            return toolinfo_t1;
//        }
// 
//        private NativeMethods.TOOLINFO_T GetTOOLINFO(Control ctl, string caption) {
//            NativeMethods.TOOLINFO_T toolinfo_t1 = this.GetMinTOOLINFO(ctl);
//            toolinfo_t1.cbSize = Marshal.SizeOf(typeof(NativeMethods.TOOLINFO_T));
//            toolinfo_t1.uFlags |= 0x110;
//            Control control1 = this.TopLevelControl;
//            if ((control1 != null) && (control1.RightToLeft == RightToLeft.Yes)) {
//                toolinfo_t1.uFlags |= 4;
//            }
//            toolinfo_t1.lpszText = caption;
//            return toolinfo_t1;
//        }
// 
//        [Localizable(true), DefaultValue(""), Description("ToolTipToolTipDescr")]
//        public string GetToolTip(Control control) {
//            string text1 = (string) this.tools[control];
//            if (text1 == null) {
//                return "";
//            }
//            return text1;
//        }
// 
//        private IntPtr GetWindowFromPoint(Point screenCoords, ref bool success) {
//            Control control1 = this.TopLevelControl;
//            IntPtr ptr1 = IntPtr.Zero;
//            if (control1 != null) {
//                ptr1 = control1.Handle;
//            }
//            IntPtr ptr2 = IntPtr.Zero;
//            bool flag1 = false;
//            while (!flag1) {
//                Point point1 = screenCoords;
//                if (control1 != null) {
//                    point1 = control1.PointToClientInternal(screenCoords);
//                }
//                IntPtr ptr3 = UnsafeNativeMethods.ChildWindowFromPointEx(new HandleRef(null, ptr1), point1.X, point1.Y, 1);
//                if (ptr3 == ptr1) {
//                    ptr2 = ptr3;
//                    flag1 = true;
//                    continue;
//                }
//                if (ptr3 == IntPtr.Zero) {
//                    flag1 = true;
//                    continue;
//                }
//                control1 = Control.FromHandleInternal(ptr3);
//                if (control1 == null) {
//                    control1 = Control.FromChildHandleInternal(ptr3);
//                    if (control1 != null) {
//                        ptr2 = control1.Handle;
//                    }
//                    flag1 = true;
//                    continue;
//                }
//                ptr1 = control1.Handle;
//            }
//            if (ptr2 != IntPtr.Zero) {
//                Control control2 = Control.FromHandleInternal(ptr2);
//                if (control2 == null) {
//                    return ptr2;
//                }
//                Control control3 = control2;
//                while ((control3 != null) && control3.Visible) {
//                    control3 = control3.ParentInternal;
//                }
//                if (control3 != null) {
//                    ptr2 = IntPtr.Zero;
//                }
//                success = true;
//            }
//            return ptr2;
//        }
// 
//        private void HandleCreated(object sender, EventArgs eventargs) {
//            this.CreateRegion((Control) sender);
//        }
// 
//        private void HandleDestroyed(object sender, EventArgs eventargs) {
//            this.DestroyRegion((Control) sender);
//        }
// 
//        private void MouseMove(object sender, MouseEventArgs me) {
//            Control control1 = (Control) sender;
//            if ((!this.created.ContainsKey(control1) && control1.IsHandleCreated) && (this.TopLevelControl != null)) {
//                this.CreateRegion(control1);
//            }
//            if (this.created.ContainsKey(control1)) {
//                control1.MouseMove -= new MouseEventHandler(this.MouseMove);
//            }
//        }
// 
//        private void OnTopLevelPropertyChanged(object s, EventArgs e) {
//            this.topLevelControl.ParentChanged -= new EventHandler(this.OnTopLevelPropertyChanged);
//            this.topLevelControl.HandleCreated -= new EventHandler(this.TopLevelCreated);
//            this.topLevelControl.HandleDestroyed -= new EventHandler(this.TopLevelDestroyed);
//            this.topLevelControl = null;
//            this.topLevelControl = this.TopLevelControl;
//        }
// 
//        private void RecreateHandle() {
//            if (!base.DesignMode) {
//                if (this.GetHandleCreated()) {
//                    this.DestroyHandle();
//                }
//                this.created.Clear();
//                this.CreateHandle();
//                this.CreateAllRegions();
//            }
//        }
// 
//        public void RemoveAll() {
//            Control[] controlArray1 = new Control[this.tools.Keys.Count];
//            this.tools.Keys.CopyTo(controlArray1, 0);
//            for (int num1 = 0; num1 < controlArray1.Length; num1++) {
//                if (controlArray1[num1].IsHandleCreated) {
//                    this.DestroyRegion(controlArray1[num1]);
//                }
//            }
//            this.tools.Clear();
//        }
// 
//        private void SetDelayTime(int type, int time) {
//            if (type == 0) {
//                this.auto = true;
//            }
//            else {
//                this.auto = false;
//            }
//            this.delayTimes[type] = time;
//            if (this.GetHandleCreated() && (time >= 0)) {
//                UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 0x403, type, time);
//                if (!this.auto) {
//                    return;
//                }
//                this.delayTimes[2] = this.GetDelayTime(2);
//                this.delayTimes[3] = this.GetDelayTime(3);
//                this.delayTimes[1] = this.GetDelayTime(1);
//            }
//            else if (this.auto) {
//                this.AdjustBaseFromAuto();
//            }
//        }
// 
//        public void SetToolTip(Control control, string caption) {
//            if (control == null) {
//                object[] objArray1 = new object[2] { "control", "null" } ;
//                throw new ArgumentException(SR.GetString("InvalidArgument", objArray1));
//            }
//            bool flag1 = false;
//            bool flag2 = false;
//            if (this.tools.ContainsKey(control)) {
//                flag1 = true;
//            }
//            if ((caption == null) || (caption.Length == 0)) {
//                flag2 = true;
//            }
//            if (flag1 && flag2) {
//                this.tools.Remove(control);
//            }
//            else if (!flag2) {
//                this.tools[control] = caption;
//            }
//            if (!flag2 && !flag1) {
//                control.HandleCreated += new EventHandler(this.HandleCreated);
//                control.HandleDestroyed += new EventHandler(this.HandleDestroyed);
//                if (control.IsHandleCreated) {
//                    this.HandleCreated(control, EventArgs.Empty);
//                }
//            }
//            else {
//                bool flag3 = (control.IsHandleCreated && (this.TopLevelControl != null)) && this.TopLevelControl.IsHandleCreated;
//                if ((flag1 && !flag2) && (flag3 && !base.DesignMode)) {
//                    int num1 = (int) UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), NativeMethods.TTM_SETTOOLINFO, 0, this.GetTOOLINFO(control, caption));
//                    if (num1 != 0) {
//                        throw new InvalidOperationException(SR.GetString("ToolTipAddFailed"));
//                    }
//                }
//                else if ((flag2 && flag1) && !base.DesignMode) {
//                    control.HandleCreated += new EventHandler(this.HandleCreated);
//                    control.HandleDestroyed += new EventHandler(this.HandleDestroyed);
//                    if (control.IsHandleCreated) {
//                        this.HandleDestroyed(control, EventArgs.Empty);
//                    }
//                }
//            }
//        }
// 
//        private bool ShouldSerializeAutomaticDelay() {
//            if (this.auto && (this.AutomaticDelay != 500)) {
//                return true;
//            }
//            return false;
//        }
// 
//        private bool ShouldSerializeAutoPopDelay() {
//            return !this.auto;
//        }
// 
//        private bool ShouldSerializeInitialDelay() {
//            return !this.auto;
//        }
// 
//        private bool ShouldSerializeReshowDelay() {
//            return !this.auto;
//        }
// 
//        private void TopLevelCreated(object sender, EventArgs eventargs) {
//            this.CreateHandle();
//            this.CreateAllRegions();
//        }
// 
//        private void TopLevelDestroyed(object sender, EventArgs eventargs) {
//            this.DestroyHandle();
//        }
// 
//        public override string ToString() {
//            string text1 = base.ToString();
//            string[] textArray1 = new string[5] { text1, " InitialDelay: ", this.InitialDelay.ToString(), ", ShowAlways: ", this.ShowAlways.ToString() } ;
//            return string.Concat(textArray1);
//        }
// 
//        private void WmWindowFromPoint(ref Message msg) {
//            NativeMethods.POINT point1 = (NativeMethods.POINT) msg.GetLParam(typeof(NativeMethods.POINT));
//            Point point2 = new Point(point1.x, point1.y);
//            bool flag1 = false;
//            msg.Result = this.GetWindowFromPoint(point2, ref flag1);
//        }
// 
//        private void WndProc(ref Message msg) {
//            if (msg.Msg == 1040) {
//                this.WmWindowFromPoint(ref msg);
//            }
//            else {
//                this.window.DefWndProc(ref msg);
//            }
//        }
// 
//        [Description("ToolTipActiveDescr"), DefaultValue(true)]
//        public bool Active {
//            get {
//                return this.active;
//            }
//            set {
//                if (this.active != value) {
//                    this.active = value;
//                    if (!base.DesignMode && this.GetHandleCreated()) {
//                        UnsafeNativeMethods.SendMessage(new HandleRef(this, this.Handle), 0x401, value ? 1 : 0, 0);
//                    }
//                }
//            }
//        }
// 
//        [DefaultValue(500), RefreshProperties(RefreshProperties.All), Description("ToolTipAutomaticDelayDescr")]
//        public int AutomaticDelay {
//            get {
//                return this.delayTimes[0];
//            }
//            set {
//                if (value < 0) {
//                    object[] objArray1 = new object[3] { "value", value.ToString(), "0" } ;
//                    throw new ArgumentException(SR.GetString("InvalidLowBoundArgumentEx", objArray1));
//                }
//                this.SetDelayTime(0, value);
//            }
//        }
// 
//        [Description("ToolTipAutoPopDelayDescr"), RefreshProperties(RefreshProperties.All)]
//        public int AutoPopDelay {
//            get {
//                return this.delayTimes[2];
//            }
//            set {
//                if (value < 0) {
//                    object[] objArray1 = new object[3] { "value", value.ToString(), "0" } ;
//                    throw new ArgumentException(SR.GetString("InvalidLowBoundArgumentEx", objArray1));
//                }
//                this.SetDelayTime(2, value);
//            }
//        }
// 
//        private CreateParams CreateParams {
//            get {
//                CreateParams params1 = new CreateParams();
//                params1.Parent = this.TopLevelControl.Handle;
//                params1.ClassName = "tooltips_class32";
//                if (this.showAlways) {
//                    params1.Style = 1;
//                }
//                params1.ExStyle = 0;
//                params1.Caption = null;
//                return params1;
//            }
//        }
// 
//        private IntPtr Handle {
//            get {
//                if (!this.GetHandleCreated()) {
//                    this.CreateHandle();
//                }
//                return this.window.Handle;
//            }
//        }
// 
//        [Description("ToolTipInitialDelayDescr"), RefreshProperties(RefreshProperties.All)]
//        public int InitialDelay {
//            get {
//                return this.delayTimes[3];
//            }
//            set {
//                if (value < 0) {
//                    object[] objArray1 = new object[3] { "value", value.ToString(), "0" } ;
//                    throw new ArgumentException(SR.GetString("InvalidLowBoundArgumentEx", objArray1));
//                }
//                this.SetDelayTime(3, value);
//            }
//        }
// 
//        [RefreshProperties(RefreshProperties.All), Description("ToolTipReshowDelayDescr")]
//        public int ReshowDelay {
//            get {
//                return this.delayTimes[1];
//            }
//            set {
//                if (value < 0) {
//                    object[] objArray1 = new object[3] { "value", value.ToString(), "0" } ;
//                    throw new ArgumentException(SR.GetString("InvalidLowBoundArgumentEx", objArray1));
//                }
//                this.SetDelayTime(1, value);
//            }
//        }
// 
//        [Description("ToolTipShowAlwaysDescr"), DefaultValue(false)]
//        public bool ShowAlways {
//            get {
//                return this.showAlways;
//            }
//            set {
//                if (this.showAlways != value) {
//                    this.showAlways = value;
//                    if (this.GetHandleCreated()) {
//                        this.RecreateHandle();
//                    }
//                }
//            }
//        }
// 
//        private Control TopLevelControl {
//            get {
//                Control control1 = null;
//                if (this.topLevelControl == null) {
//                    Control[] controlArray1 = new Control[this.tools.Keys.Count];
//                    this.tools.Keys.CopyTo(controlArray1, 0);
//                    if ((controlArray1 != null) && (controlArray1.Length > 0)) {
//                        for (int num1 = 0; num1 < controlArray1.Length; num1++) {
//                            Control control2 = controlArray1[num1];
//                            if ((control2 != null) && (control2 is Form)) {
//                                Form form1 = (Form) control2;
//                                if (form1.TopLevel) {
//                                    control1 = control2;
//                                    break;
//                                }
//                            }
//                            if ((control2 != null) && (control2.ParentInternal != null)) {
//                                while (control2.ParentInternal != null) {
//                                    control2 = control2.ParentInternal;
//                                }
//                                control1 = control2;
//                                if (control1 != null) {
//                                    break;
//                                }
//                            }
//                        }
//                    }
//                    this.topLevelControl = control1;
//                    if (control1 != null) {
//                        control1.HandleCreated += new EventHandler(this.TopLevelCreated);
//                        control1.HandleDestroyed += new EventHandler(this.TopLevelDestroyed);
//                        if (control1.IsHandleCreated) {
//                            this.TopLevelCreated(control1, EventArgs.Empty);
//                        }
//                        control1.ParentChanged += new EventHandler(this.OnTopLevelPropertyChanged);
//                    }
//                    return control1;
//                }
//                return this.topLevelControl;
//            }
//        }
// 
//        // Fields
//        private bool active;
//        private bool auto;
//        private const int AUTOPOP_RATIO = 10;
//        private Hashtable created;
//        private const int DEFAULT_DELAY = 500;
//        private int[] delayTimes;
//        private const int RESHOW_RATIO = 5;
//        private bool showAlways;
//        private Hashtable tools;
//        private Control topLevelControl;
//        private ToolTipNativeWindow window;
//
// 
//    }
//}
