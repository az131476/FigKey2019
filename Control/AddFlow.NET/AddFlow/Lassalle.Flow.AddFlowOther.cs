using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Xml;
using System.Reflection;
using System.ComponentModel.Design.Serialization;
using System.Net;
//using System.Data;

namespace Lassalle.Flow{
    internal class InPlaceTextBox : TextBox {

        private InPlaceTextBox() {
            this.m_node = null;
            this.m_end = false;
        }
 
        internal InPlaceTextBox(Node node) {
            this.m_node = null;
            this.m_end = false;
            this.m_end = false;
            this.m_node = node;
            RectangleF ef1 = this.m_node.RC;
            float single1 = (this.m_node.m_drawWidth == 0) ? ((float) 1) : ((float) this.m_node.m_drawWidth);
            ef1.Inflate((-single1 / 2f) - 1f, (-single1 / 2f) - 1f);
            float single2 = (this.m_node.TextMargin.Width * this.m_node.RC.Width) / 100f;
            float single3 = (this.m_node.TextMargin.Height * this.m_node.RC.Height) / 100f;
            ef1.Inflate(-single2, -single3);
            Graphics graphics1 = this.m_node.m_af.CreateGraphics();
            this.m_node.m_af.CoordinatesDeviceToWorld(graphics1);
            RectangleF ef2 = Misc.RectWorldToDevice(graphics1, ef1);
            graphics1.Dispose();
            base.Visible = false;
            base.Parent = this.m_node.m_af;
            base.BorderStyle = BorderStyle.None;
            this.Multiline = true;
            base.Name = "textBoxInPlace";
            base.Location = new Point((int) ef2.Location.X, (int) ef2.Location.Y);
            base.Size = new Size((int) ef2.Size.Width, (int) ef2.Size.Height);
            this.Text = this.m_node.Text;
            this.ForeColor = this.m_node.TextColor;
            this.BackColor = this.m_node.FillColor;
            if (this.m_node.Text != null) {
                base.Select(0, this.m_node.Text.Length);
            }
            switch (this.m_node.Alignment) {
            case Alignment.LeftJustifyTOP:
            case Alignment.LeftJustifyMIDDLE:
            case Alignment.LeftJustifyBOTTOM: {
                base.TextAlign = HorizontalAlignment.Left;
                break;
            }
            case Alignment.RightJustifyTOP:
            case Alignment.RightJustifyMIDDLE:
            case Alignment.RightJustifyBOTTOM: {
                base.TextAlign = HorizontalAlignment.Right;
                break;
            }
            default: {
                base.TextAlign = HorizontalAlignment.Center;
                break;
            }
            }
            base.Visible = true;
            base.Focus();
        }
 
        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.KeyCode == Keys.Return) {
                if (e.Control) {
                    goto Label_0084;
                }
                this.m_end = true;
                if ((this.m_node != null) && this.m_node.Existing) {
                    this.m_node.m_af.StopEdition(this.m_node, true);
                }
                return;
            }
            if (e.KeyCode == Keys.Escape) {
                this.m_end = true;
                if ((this.m_node != null) && this.m_node.Existing) {
                    this.m_node.m_af.StopEdition(this.m_node, false);
                }
                return;
            }
            Label_0084:
                base.OnKeyDown(e);
        }
 
        protected override void OnLostFocus(EventArgs e) {
            base.OnLostFocus(e);
            if ((!this.m_end && (this.m_node != null)) && this.m_node.Existing) {
                this.m_node.m_af.StopEdition(this.m_node, true);
            }
        }
 
        internal Node Node {
            get {
                return this.m_node;
            }
            set {
                this.m_node = value;
            }
        }
 
        private bool m_end;
 
        private Node m_node;
    }

    


    public enum Action {
        // Fields
        AdjustDst = 1,
        AdjustOrg = 0,
        Alignment = 2,
        ArrowDst = 5,
        ArrowMid = 4,
        ArrowOrg = 3,
        AutoSize = 6,
        BackMode = 7,
        ClearLinks = 9,
        ClearNodes = 8,
        CustomEndCap = 10,
        CustomStartCap = 11,
        DashStyle = 12,
        DeleteSel = 13,
        DrawColor = 14,
        DrawWidth = 15,
        Dst = 0x10,
        EndCap = 0x11,
        FillColor = 0x12,
        Font = 0x13,
        Gradient = 20,
        GradientColor = 0x15,
        GradientMode = 0x16,
        Hidden = 0x17,
        ImageIndex = 0x18,
        ImageLocation = 0x19,
        ImagePosition = 0x1a,
        InLinkable = 0x1b,
        Jump = 0x1d,
        LabelEdit = 30,
        Line = 0x1f,
        LinkAdd = 0x20,
        LinkMove = 0x21,
        LinkRemove = 0x22,
        LinkStretch = 0x23,
        Logical = 0x24,
        MoveItems = 0x25,
        NodeAdd = 0x26,
        NodeMove = 0x27,
        NodeMoveAndSize = 40,
        NodeRemove = 0x29,
        NodeResize = 0x2a,
        None = 0x2b,
        Org = 0x2c,
        OrientedText = 0x2d,
        OutLinkable = 0x1c,
        OwnerDraw = 0x2e,
        Points = 0x2f,
        Reverse = 0x30,
        Rigid = 0x31,
        Selectable = 50,
        Shadow = 0x33,
        Shape = 0x34,
        StartCap = 0x35,
        Stretchable = 0x36,
        Tag = 0x37,
        Text = 0x38,
        TextColor = 0x39,
        TextMargin = 0x3a,
        Tooltip = 0x3b,
        Transparent = 60,
        Trimming = 0x3d,
        Url = 0x3e,
        XMoveable = 0x3f,
        XSizeable = 0x40,
        YMoveable = 0x41,
        YSizeable = 0x42,
        ZOrder = 0x43
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>
    public enum Alignment {
        // Fields
        CenterBOTTOM = 8,
        CenterMIDDLE = 7,
        CenterTOP = 6,
        LeftJustifyBOTTOM = 2,
        LeftJustifyMIDDLE = 1,
        LeftJustifyTOP = 0,
        RightJustifyBOTTOM = 5,
        RightJustifyMIDDLE = 4,
        RightJustifyTOP = 3
    }

    public enum ArrowAngle {
        // Fields
        deg15 = 0,
        deg30 = 1,
        deg45 = 2
    }
 
    public enum ArrowSize {
        // Fields
        Large = 2,
        Medium = 1,
        Small = 0
    }
 
    public enum ArrowStyle {
        // Fields
        Arrow = 2,
        Circle = 1,
        ClosedFork = 4,
        Fork = 3,
        HalfArrow = 12,
        Losange = 5,
        Many = 6,
        ManyOptional = 7,
        None = 0,
        One = 8,
        OneOptional = 9,
        OneOrMany = 10,
        OpenedArrow = 11,
        OpenedHalfArrow = 13
    }
 
    public enum AutoSize {
        // Fields
        ImageToNode = 1,
        NodeToImage = 2,
        NodeToText = 3,
        None = 0
    }
 
    public enum BackMode {
        // Fields
        Opaque = 1,
        Transparent = 0
    }

    public enum CursorSetting {
        // Fields
        All = 3,
        None = 0,
        Resize = 1,
        ResizeAndDrag = 2
    }
 
    public enum CycleMode {
        // Fields
        CycleAllowed = 0,
        NoCycle = 1,
        NoDirectedCycle = 2
    }

    /// <summary>
    /// /////////////////////////////////////////////////////
    /// /////////////////////////////////////////////////////
    /// </summary>
    internal class AddFlowDesigner : ControlDesigner {
        // Methods
        internal AddFlowDesigner() {
            this.m_af = null;
            this.m_showBorder = true;
        }

        private void addflow_DesignModeChange(object sender, EventArgs e) {
            this.DesignModeDisplay();
        }

        private void DesignModeDisplay() {
            try {
                this.m_af.Nodes.Clear();
                Node node1 = new Node(5f, 5f, 30f, 30f, this.m_af.m_defnode);
                node1.Text = "node1";
                Node node2 = new Node(100f, 5f, 30f, 30f, this.m_af.m_defnode);
                node2.Text = "node2";
                Node node3 = new Node(100f, 80f, 30f, 30f, this.m_af.m_defnode);
                node3.Text = "node3";
                Link link1 = new Link(this.m_af.m_deflink);
                link1.Text = "link1";
                Link link2 = new Link(this.m_af.m_deflink);
                link2.Text = "link2";
                Link link3 = new Link(this.m_af.m_deflink);
                link3.Text = "link3";
                this.m_af.AddNodeToGraph(node1);
                this.m_af.AddNodeToGraph(node2);
                this.m_af.AddNodeToGraph(node3);
                this.m_af.AddLink(link1, node1, node2);
                this.m_af.AddLink(link2, node2, node3);
                this.m_af.AddLink(link3, node1, node3);
                link3.Points.Add(new PointF(40f, 60f));
                link3.Points.Add(new PointF(40f, 95f));
                this.m_af.Invalidate();
            }
            catch (AddFlowException) {
                this.m_af.Invalidate();
            }
        }
 
        protected override void Dispose(bool disposing) {
            if (disposing) {
                this.m_af.DesignModeChange -= new AddFlow.DesignModeChangeEventHandler(this.addflow_DesignModeChange);
            }
            base.Dispose(disposing);
        }
 
        public override void Initialize(IComponent component) {
            base.Initialize(component);
            this.m_af = (AddFlow) component;
            this.DesignModeDisplay();
            this.m_af.DesignModeChange += new AddFlow.DesignModeChangeEventHandler(this.addflow_DesignModeChange);
        }
 
        protected void OnAboutVerb(object sender, EventArgs e) {
            AboutDialogBox box1 = new AboutDialogBox(this.m_af);
            box1.ShowDialog();
        }
 
        protected override void OnPaintAdornments(PaintEventArgs e) {
            base.OnPaintAdornments(e);
            if (this.m_showBorder) {
                Graphics graphics1 = e.Graphics;
                using (Pen pen1 = new Pen(Color.Gray, 1f)) {
                    pen1.DashStyle = DashStyle.Dash;
                    graphics1.DrawRectangle(pen1, 0, 0, (int) (this.m_af.Width - 1), (int) (this.m_af.Height - 1));
                }
            }
        }
 
        protected override void PreFilterProperties(IDictionary properties) {
            base.PreFilterProperties(properties);
            Attribute[] attributeArray1 = new Attribute[2] { CategoryAttribute.Design, DesignOnlyAttribute.Yes } ;
            properties["ShowBorder"] = TypeDescriptor.CreateProperty(typeof(AddFlowDesigner), "ShowBorder", typeof(bool), attributeArray1);
        }
 

        // Properties

        [DefaultValue(true)]
        private bool ShowBorder {
            get {
                return this.m_showBorder;
            }
            set {
                this.m_showBorder = value;
                this.m_af.Refresh();
            }
        }
 
        public override DesignerVerbCollection Verbs {
            get {
                DesignerVerb[] verbArray1 = new DesignerVerb[1] { new DesignerVerb("About", new EventHandler(this.OnAboutVerb)) } ;
                return new DesignerVerbCollection(verbArray1);
            }
        }

        // Fields
        private AddFlow m_af;
        private bool m_showBorder;
    }
 

    ////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////

    public class AddFlowException : ApplicationException {
        // Methods
        public AddFlowException(string message) : base(message) {
        }
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>
    public class AddFlowLicense : License {
        // Methods
        static AddFlowLicense() {
            AddFlowLicense.lengthKey = 12;
        }
 
        public AddFlowLicense(AddFlowLicenseProvider owner, string key) {
            this.licenseKey = null;
            this.owner = null;
            this.owner = owner;
            this.licenseKey = key;
        }
 
        public override void Dispose() {
        }
 
        // Properties
        public string HumanLicenseKey {
            get {
                return this.licenseKey.Substring(0, AddFlowLicense.lengthKey);
            }
        }
 
        public override string LicenseKey {
            get {
                return this.licenseKey;
            }
        }
 
        // Fields
        public static int lengthKey;
        private string licenseKey;
        private AddFlowLicenseProvider owner;
    }
 
    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>

    public class AddFlowLicenseProvider : LicenseProvider {
        // Methods
        public AddFlowLicenseProvider() {
        }
 
        private static bool CheckLicense(string lic, string hash) {
            CspParameters parameters1 = new CspParameters();
            parameters1.Flags = CspProviderFlags.UseMachineKeyStore;
            RSACryptoServiceProvider provider1 = new RSACryptoServiceProvider(parameters1);
            string text1 = "<RSAKeyValue><Modulus>wjq05PO96aKtEpmjqaiA3Z3pVNkPUXo93gDzo8G1g5xjxS9qBeQygIz+ByhkqWaL91BVZw7i2UN36nNShQ4OdJwivd8nshAPfcDHG90nJON3G2X68SyWYT2MbV8z0mS6TnRYp2GOlZ7cqwuk/EZz04Ujv+KZPhgU9PdAA3vcbT8=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            provider1.FromXmlString(text1);
            byte[] buffer1 = Convert.FromBase64String(hash);
            byte[] buffer2 = Encoding.UTF8.GetBytes(lic);
            return provider1.VerifyData(buffer2, new SHA1CryptoServiceProvider(), buffer1);
        }
 
        internal static int CheckLicNum(string lic) {
            lic = lic.Replace(" ", (string) null).Replace("\r", (string) null).Replace("\n", (string) null);
            if (lic.Substring(0, 1) == "A") {
                if (lic.Substring(0, 12) == "A010C0100004") {
                    return 0;
                }
                if (AddFlowLicenseProvider.CheckLicense(lic.Substring(0, AddFlowLicense.lengthKey), lic.Substring(AddFlowLicense.lengthKey + 1))) {
                    return 1;
                }
            }
            return 0;
        }
 
        public override License GetLicense(LicenseContext context, Type type, object instance, bool allowExceptions) {
            if (context == null) {
                return null;
            }
            AddFlowLicense license1 = null;
            if (context.UsageMode == LicenseUsageMode.Runtime) {
                string text1 = context.GetSavedLicenseKey(type, null);
                if ((text1 != null) && this.IsKeyValid(text1, type)) {
                    license1 = new AddFlowLicense(this, text1);
                }
            }
            if (license1 == null) {
                try {
                    string text2 = null;
                    ITypeResolutionService service1 = (ITypeResolutionService) context.GetService(typeof(ITypeResolutionService));
                    if (service1 != null) {
                        text2 = service1.GetPathOfAssembly(type.Assembly.GetName());
                    }
                    if (text2 == null) {
                        text2 = type.Module.FullyQualifiedName;
                    }
                    string text3 = Path.GetDirectoryName(text2) + @"\" + type.FullName + ".lic";
                    XmlDocument document1 = new XmlDocument();
                    document1.Load(text3);
                    string text4 = string.Format("descendant::component[@name='{0}']", type.FullName);
                    XmlNode node1 = document1.SelectSingleNode(text4);
                    if (node1 != null) {
                        string text5 = node1.Attributes.GetNamedItem("license").Value;
                        if ((text5 == null) || !this.IsKeyValid(text5, type)) {
                            return license1;
                        }
                        license1 = new AddFlowLicense(this, text5);
                        context.SetSavedLicenseKey(type, license1.LicenseKey);
                    }
                }
                catch {
                    return null;
                }
            }
            return license1;
        }
 
        private bool IsKeyValid(string key, Type type) {
            if (key != null) {
                return (AddFlowLicenseProvider.CheckLicNum(key) == 1);
            }
            return false;
        }
    }
 

    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>

    internal class AddLinkTask : Task {
        // Methods
        private AddLinkTask() {
        }
 
        internal AddLinkTask(Link link) {
            this.m_link = link;
            this.m_af = this.m_link.m_af;
            this.m_oldPtScroll = this.m_af.ScrollPosition;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.LinkAdd;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.m_af.AddLinkToGraph(this.m_link);
            this.m_link.InvalidateItem();
            this.m_link.InvalidateHandles();
            this.m_af.UpdateRect2(this.m_link.m_rc);
            Point point1 = this.m_af.ScrollPosition;
            this.m_af.ScrollPosition = this.m_oldPtScroll;
            this.m_oldPtScroll = point1;
        }
 
        internal override void Undo() {
            if (this.m_link.m_selected) {
                this.m_link.Selected = false;
            }
            this.m_link.InvalidateItem();
            this.m_af.RemoveLinkFromGraph(this.m_link);
            this.m_af.UpdateRect();
        }
 
        // Fields
        private AddFlow m_af;
        private Link m_link;
        private Point m_oldPtScroll;
    }
    
    
    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>
    internal class AddNodeTask : Task {
        // Methods
        private AddNodeTask() {
        }
 
        internal AddNodeTask(Node node) {
            this.m_node = node;
            this.m_af = this.m_node.m_af;
            this.m_oldPtScroll = this.m_af.ScrollPosition;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.NodeAdd;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.m_af.AddNodeToGraph(this.m_node);
            this.m_node.InvalidateItem();
            this.m_node.InvalidateHandles();
            this.m_af.UpdateRect2(this.m_node.GetRectShadow());
        }
 
        internal override void Undo() {
            if (this.m_node.m_selected) {
                this.m_node.Selected = false;
            }
            this.m_node.InvalidateItem();
            this.m_af.RemoveNodeFromGraph(this.m_node);
            this.m_af.UpdateRect();
            Point point1 = this.m_af.ScrollPosition;
            this.m_af.ScrollPosition = this.m_oldPtScroll;
            this.m_oldPtScroll = point1;
        }
 
        // Fields
        private AddFlow m_af;
        private Node m_node;
        private Point m_oldPtScroll;
    }
 

    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>
    public class AfterAddLinkEventArgs : EventArgs {
        // Methods
        public AfterAddLinkEventArgs(Link link) {
            this.link = link;
        }
 
        // Properties
        public Link Link {
            get {
                return this.link;
            }
        }
 
        // Fields
        private Link link;
    }
 
    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>

    public class AfterAddNodeEventArgs : EventArgs {
        // Methods
        public AfterAddNodeEventArgs(Node node) {
            this.node = node;
        }
 
        // Properties
        public Node Node {
            get {
                return this.node;
            }
        }
 
        // Fields
        private Node node;
    }
 
    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>

    public class AfterEditEventArgs : EventArgs {
        // Methods
        public AfterEditEventArgs(string text, Node node) {
            this.cancel = new CancelEventArgs(false);
            this.str = text;
            this.node = node;
        }
 
        // Properties
        public CancelEventArgs Cancel {
            get {
                return this.cancel;
            }
            set {
                this.cancel = value;
            }
        }
 
        public Node Node {
            get {
                return this.node;
            }
        }
 
        public string Text {
            get {
                return this.str;
            }
            set {
                this.str = value;
            }
        }
 
        // Fields
        private CancelEventArgs cancel;
        private Node node;
        private string str;
    }
 

    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>

    [Serializable, TypeConverter(typeof(ArrowConverter))]
    public class Arrow : ICloneable {
        // Events
        internal event AfterChangeEventHandler AfterChange;
        internal event BeforeChangeEventHandler BeforeChange;

        // Methods
        static Arrow() {
            Arrow.None = new Arrow();
        }
 
        public Arrow() {
            this.m_lngArr = SystemInformation.IconSize.Width / 4;
            this.Reset();
        }
 
        public Arrow(string format) {
            this.m_lngArr = SystemInformation.IconSize.Width / 4;
            char[] chArray1 = new char[1] { ';' } ;
            string[] textArray1 = format.Split(chArray1);
            this.m_style = (ArrowStyle) Enum.Parse(typeof(ArrowStyle), textArray1[0].Trim(), false);
            this.m_size = (ArrowSize) Enum.Parse(typeof(ArrowSize), textArray1[1].Trim(), false);
            this.m_angle = (ArrowAngle) Enum.Parse(typeof(ArrowAngle), textArray1[2].Trim(), false);
            chArray1 = new char[1] { '=' } ;
            string[] textArray2 = textArray1[3].Split(chArray1);
            this.m_filled = textArray2[1].Trim() == bool.TrueString;
        }
 
        public Arrow(ArrowStyle style, ArrowSize size, ArrowAngle angle, bool filled) {
            this.m_lngArr = SystemInformation.IconSize.Width / 4;
            this.m_style = style;
            this.m_size = size;
            this.m_angle = angle;
            this.m_filled = filled;
        }
 
        public object Clone() {
            return new Arrow(this.m_style, this.m_size, this.m_angle, this.m_filled);
        }
 
        internal bool Equals(object obj) {
            if (obj is Arrow) {
                Arrow arrow1 = (Arrow) obj;
                if (((this.m_style == arrow1.m_style) && (this.m_size == arrow1.m_size)) && (this.m_angle == arrow1.m_angle)) {
                    return (this.m_filled == arrow1.m_filled);
                }
            }
            return false;
        }
 
        private void GetArrowPoints(PointF pt1, PointF pt2, PointF[] aptArr, float nSizeArr) {
            float single1 = pt1.X - pt2.X;
            float single2 = pt2.Y - pt1.Y;
            float single3 = (float) Math.Sqrt((double) ((single1 * single1) + (single2 * single2)));
            if (single3 == 0f) {
                for (int num1 = 0; num1 < aptArr.Length; num1++) {
                    aptArr[num1] = pt1;
                }
            }
            else {
                float single6;
                float single7;
                PointF tf1;
                float single4 = single1 / single3;
                float single5 = single2 / single3;
                switch (this.m_angle) {
                case ArrowAngle.deg30: {
                    single7 = 0.866f;
                    single6 = 0.5f;
                    break;
                }
                case ArrowAngle.deg45: {
                    single7 = 0.707f;
                    single6 = 0.707f;
                    break;
                }
                default: {
                    single7 = 0.966f;
                    single6 = 0.259f;
                    break;
                }
                }
                int num2 = (int) ((nSizeArr * single7) * single4);
                int num3 = (int) ((nSizeArr * single6) * single5);
                int num4 = (int) ((nSizeArr * single6) * single4);
                int num5 = (int) ((nSizeArr * single7) * single5);
                aptArr[0] = pt2;
                aptArr[1].X = (pt2.X + num2) + num3;
                aptArr[1].Y = (pt2.Y + num4) - num5;
                aptArr[2].X = (pt2.X + num2) - num3;
                aptArr[2].Y = (pt2.Y - num4) - num5;
                switch (this.m_style) {
                case ArrowStyle.Fork:
                case ArrowStyle.ClosedFork:
                case ArrowStyle.Many:
                case ArrowStyle.ManyOptional:
                case ArrowStyle.One:
                case ArrowStyle.OneOrMany: {
                    aptArr[3] = aptArr[1];
                    aptArr[4] = aptArr[2];
                    tf1 = Misc.MiddlePoint(aptArr[1], aptArr[2]);
                    aptArr[0] = tf1;
                    aptArr[1].X = (tf1.X - num2) - num3;
                    aptArr[1].Y = (tf1.Y - num4) + num5;
                    aptArr[2].X = (tf1.X - num2) + num3;
                    aptArr[2].Y = (tf1.Y + num4) + num5;
                    return;
                }
                case ArrowStyle.Losange: {
                    tf1 = Misc.MiddlePoint(aptArr[1], aptArr[2]);
                    aptArr[4].X = pt2.X + (2f * (tf1.X - pt2.X));
                    aptArr[4].Y = pt2.Y + (2f * (tf1.Y - pt2.Y));
                    aptArr[3] = aptArr[2];
                    aptArr[2] = aptArr[4];
                    return;
                }
                case ArrowStyle.OneOptional: {
                    aptArr[3] = aptArr[1];
                    aptArr[4] = aptArr[2];
                    tf1 = Misc.MiddlePoint(aptArr[1], aptArr[2]);
                    aptArr[0] = tf1;
                    aptArr[1].X = (tf1.X - num2) - num3;
                    aptArr[1].Y = (tf1.Y - num4) + num5;
                    aptArr[2].X = (tf1.X - num2) + num3;
                    aptArr[2].Y = (tf1.Y + num4) + num5;
                    tf1 = Misc.MiddlePoint(aptArr[1], aptArr[4]);
                    aptArr[4] = Misc.MiddlePoint(aptArr[2], aptArr[3]);
                    aptArr[3] = tf1;
                    return;
                }
                case ArrowStyle.OpenedArrow: {
                    return;
                }
                case ArrowStyle.HalfArrow:
                case ArrowStyle.OpenedHalfArrow: {
                    aptArr[2] = Misc.MiddlePoint(aptArr[1], aptArr[2]);
                    return;
                }
                }
            }
        }
 
        private RectangleF GetCircleRect(PointF pt1, PointF pt2, float nSizeArr) {
            RectangleF ef1 = RectangleF.Empty;
            float single1 = pt1.X - pt2.X;
            float single2 = pt2.Y - pt1.Y;
            float single3 = (float) Math.Sqrt((double) ((single1 * single1) + (single2 * single2)));
            if (single3 != 0f) {
                float single4 = nSizeArr / 2f;
                ef1 = RectangleF.FromLTRB(pt2.X - single4, pt2.Y - single4, pt2.X + single4, pt2.Y + single4);
                ef1.Offset((single4 * single1) / single3, (-single4 * single2) / single3);
            }
            return ef1;
        }
 
        public GraphicsPath GetPath(PointF pt1, PointF pt2) {
            PointF[] tfArray1;
            RectangleF ef1;
            PointF[] tfArray2;

            //??????????????????????????????
            float single1 = (((float) ((int)this.m_size + ArrowSize.Large)) * this.m_lngArrWorld) / 2f;
            GraphicsPath path1 = new GraphicsPath();
            switch (this.m_style) {
            case ArrowStyle.Circle: {
                ef1 = this.GetCircleRect(pt1, pt2, single1);
                path1.AddEllipse(ef1);
                return path1;
            }
            case ArrowStyle.Arrow:
            case ArrowStyle.HalfArrow: {
                tfArray1 = new PointF[3];
                this.GetArrowPoints(pt1, pt2, tfArray1, single1);
                path1.AddPolygon(tfArray1);
                return path1;
            }
            case ArrowStyle.Fork: {
                tfArray1 = new PointF[5];
                this.GetArrowPoints(pt1, pt2, tfArray1, single1);
                tfArray2 = new PointF[3] { tfArray1[0], tfArray1[1], tfArray1[2] } ;
                path1.AddPolygon(tfArray2);
                return path1;
            }
            case ArrowStyle.ClosedFork: {
                tfArray1 = new PointF[5];
                this.GetArrowPoints(pt1, pt2, tfArray1, single1);
                tfArray2 = new PointF[3] { tfArray1[0], tfArray1[1], tfArray1[2] } ;
                path1.AddPolygon(tfArray2);
                path1.AddLine(tfArray1[3], tfArray1[4]);
                path1.AddLine(tfArray1[4], tfArray1[3]);
                return path1;
            }
            case ArrowStyle.Losange: {
                tfArray1 = new PointF[5];
                this.GetArrowPoints(pt1, pt2, tfArray1, single1);
                tfArray2 = new PointF[4] { tfArray1[0], tfArray1[1], tfArray1[2], tfArray1[3] } ;
                path1.AddPolygon(tfArray2);
                return path1;
            }
            case ArrowStyle.Many: {
                tfArray1 = new PointF[5];
                this.GetArrowPoints(pt1, pt2, tfArray1, single1);
                path1.AddLine(tfArray1[0], tfArray1[1]);
                path1.AddLine(tfArray1[0], tfArray1[2]);
                path1.AddLine(tfArray1[0], pt2);
                return path1;
            }
            case ArrowStyle.ManyOptional: {
                tfArray1 = new PointF[5];
                this.GetArrowPoints(pt1, pt2, tfArray1, single1);
                path1.AddLine(tfArray1[0], tfArray1[1]);
                path1.AddLine(tfArray1[0], tfArray1[2]);
                path1.AddLine(tfArray1[0], pt2);
                ef1 = this.GetCircleRect(pt1, tfArray1[0], single1);
                path1.AddEllipse(ef1);
                return path1;
            }
            case ArrowStyle.One: {
                tfArray1 = new PointF[5];
                this.GetArrowPoints(pt1, pt2, tfArray1, single1);
                path1.AddLine(tfArray1[3], tfArray1[4]);
                path1.AddLine(tfArray1[4], tfArray1[3]);
                return path1;
            }
            case ArrowStyle.OneOptional: {
                tfArray1 = new PointF[5];
                this.GetArrowPoints(pt1, pt2, tfArray1, single1);
                path1.AddLine(tfArray1[3], tfArray1[4]);
                path1.AddLine(tfArray1[4], tfArray1[3]);
                ef1 = this.GetCircleRect(pt1, tfArray1[0], single1);
                path1.AddEllipse(ef1);
                return path1;
            }
            case ArrowStyle.OneOrMany: {
                tfArray1 = new PointF[5];
                this.GetArrowPoints(pt1, pt2, tfArray1, single1);
                path1.AddLine(tfArray1[0], tfArray1[1]);
                path1.AddLine(tfArray1[0], tfArray1[2]);
                path1.AddLine(tfArray1[0], pt2);
                path1.AddLine(tfArray1[0], tfArray1[4]);
                path1.AddLine(tfArray1[0], tfArray1[3]);
                return path1;
            }
            case ArrowStyle.OpenedArrow: {
                tfArray1 = new PointF[3];
                this.GetArrowPoints(pt1, pt2, tfArray1, single1);
                path1.AddLine(tfArray1[0], tfArray1[1]);
                path1.AddLine(tfArray1[0], tfArray1[2]);
                return path1;
            }
            case ArrowStyle.OpenedHalfArrow: {
                tfArray1 = new PointF[3];
                this.GetArrowPoints(pt1, pt2, tfArray1, single1);
                path1.AddLine(tfArray1[0], tfArray1[1]);
                return path1;
            }
            }
            return path1;
        }
 
        internal GraphicsPath GetPath2(PointF pt1, PointF pt2, Graphics grfx) {
            this.m_lngArrWorld = Misc.DistanceDeviceToWorld(grfx, (float) this.m_lngArr);
            return this.GetPath(pt1, pt2);
        }
 
        internal bool IsDefault() {
            if (((this.m_style == ArrowStyle.None) && (this.m_size == ArrowSize.Small)) && (this.m_angle == ArrowAngle.deg15)) {
                return !this.m_filled;
            }
            return false;
        }
 
        internal bool IsDefault2() {
            if (((this.m_style == ArrowStyle.Arrow) && (this.m_size == ArrowSize.Small)) && (this.m_angle == ArrowAngle.deg15)) {
                return !this.m_filled;
            }
            return false;
        }
 
        internal virtual void OnAfterChange(EventArgs e) {
            if (this.AfterChange != null) {
                this.AfterChange(this, e);
            }
        }
 
        internal virtual void OnBeforeChange(EventArgs e) {
            if (this.BeforeChange != null) {
                this.BeforeChange(this, e);
            }
        }
 
        internal void Reset() {
            this.m_style = ArrowStyle.None;
            this.m_size = ArrowSize.Small;
            this.m_angle = ArrowAngle.deg15;
            this.m_filled = false;
            this.m_lngArrWorld = this.m_lngArr;
        }
 
        internal void Reset2() {
            this.m_style = ArrowStyle.Arrow;
            this.m_size = ArrowSize.Small;
            this.m_angle = ArrowAngle.deg15;
            this.m_filled = false;
            this.m_lngArrWorld = this.m_lngArr;
        }
 
        public override string ToString() {
            StringBuilder builder1 = new StringBuilder("");
            object[] objArray1 = new object[7] { this.m_style, "; ", this.m_size, "; ", this.m_angle, "; Filled = ", this.m_filled } ;
            builder1.Append(string.Concat(objArray1));
            return builder1.ToString();
        }
 
        // Properties

        [Description("Returns/sets the angle of the arrow."), NotifyParentProperty(true), DefaultValue(0)]
        public ArrowAngle Angle {
            get {
                return this.m_angle;
            }
            set {
                if (this.m_angle != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_angle = value;
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), Description("Determines if the arrow is filled with the drawing color or empty."), DefaultValue(false)]
        public bool Filled {
            get {
                return this.m_filled;
            }
            set {
                if (this.m_filled != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_filled = value;
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the size of the arrow."), NotifyParentProperty(true), DefaultValue(0)]
        public ArrowSize Size {
            get {
                return this.m_size;
            }
            set {
                if (this.m_size != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_size = value;
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), Description("Returns/sets the shape of the arrow head."), DefaultValue(0)]
        public ArrowStyle Style {
            get {
                return this.m_style;
            }
            set {
                if (this.m_style != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_style = value;
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }

        // Fields
        //private AfterChangeEventHandler AfterChange;
        //private BeforeChangeEventHandler BeforeChange;
        private const float COS_15 = 0.966f;
        private const float COS_30 = 0.866f;
        private const float COS_45 = 0.707f;
        internal ArrowAngle m_angle;
        internal bool m_filled;
        private int m_lngArr;
        private float m_lngArrWorld;
        internal ArrowSize m_size;
        internal ArrowStyle m_style;
        public static readonly Arrow None;
        private const float SIN_15 = 0.259f;
        private const float SIN_30 = 0.5f;
        private const float SIN_45 = 0.707f;

        // Nested Types
        internal delegate void AfterChangeEventHandler(object sender, EventArgs e);


        internal delegate void BeforeChangeEventHandler(object sender, EventArgs e);

    }
 
    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>

    internal class ArrowConverter : ExpandableObjectConverter {
        // Methods
        public ArrowConverter() {
        }
 
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
 
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(InstanceDescriptor)) {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }
 
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value) {
            if (value is string) {
                try {
                    return new Arrow((string) value);
                }
                catch {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ArgumentsNotValid));
                }
            }
            return base.ConvertFrom(context, info, value);
        }
 
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (value is Arrow) {
                Arrow arrow1 = (Arrow) value;
                if (destinationType == typeof(string)) {
                    return arrow1.ToString();
                }
                if (destinationType == typeof(InstanceDescriptor)) {
                    object[] objArray2 = new object[4] { arrow1.Style, arrow1.Size, arrow1.Angle, arrow1.Filled } ;
                    object[] objArray1 = objArray2;
                    Type[] typeArray2 = new Type[4] { typeof(ArrowStyle), typeof(ArrowSize), typeof(ArrowAngle), typeof(bool) } ;
                    Type[] typeArray1 = typeArray2;
                    ConstructorInfo info1 = typeof(Arrow).GetConstructor(typeArray1);
                    return new InstanceDescriptor(info1, objArray1);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
 
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
            return new Arrow((ArrowStyle) propertyValues["Style"], (ArrowSize) propertyValues["Size"], (ArrowAngle) propertyValues["Angle"], (bool) propertyValues["Filled"]);
        }
 
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
            return true;
        }
 

    }
 
    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>

    public class BeforeAddLinkEventArgs : EventArgs {
        // Methods
        public BeforeAddLinkEventArgs(Node org, Node dst) {
            this.org = org;
            this.dst = dst;
            this.Cancel = new CancelEventArgs(false);
        }
 
        // Properties
        public CancelEventArgs Cancel {
            get {
                return this.cancel;
            }
            set {
                this.cancel = value;
            }
        }
 
        public Node Dst {
            get {
                return this.dst;
            }
        }
 
        public Node Org {
            get {
                return this.org;
            }
        }
 
        // Fields
        private CancelEventArgs cancel;
        private Node dst;
        private Node org;
    }
 

    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>

    public class BeforeAddNodeEventArgs : EventArgs {

        public BeforeAddNodeEventArgs(RectangleF rectangle) {
            this.rcNode = new RectangleF(rectangle.Location, rectangle.Size);
            this.Cancel = new CancelEventArgs(false);
        }
 
        public CancelEventArgs Cancel {
            get {
                return this.cancel;
            }
            set {
                this.cancel = value;
            }
        }
 
        public PointF Location {
            get {
                return this.rcNode.Location;
            }
            set {
                this.rcNode.Location = value;
            }
        }
 
        public RectangleF Rect {
            get {
                return this.rcNode;
            }
            set {
                this.rcNode = value;
            }
        }
 
        public SizeF Size {
            get {
                return this.rcNode.Size;
            }
            set {
                this.rcNode.Size = value;
            }
        }
 
        private CancelEventArgs cancel;
 
        private RectangleF rcNode;
    }

    public class BeforeChangeDstEventArgs : EventArgs {

        public BeforeChangeDstEventArgs(Link link, Node dst) {
            this.cancel = new CancelEventArgs(false);
            this.link = link;
            this.dst = dst;
        }
 
        public CancelEventArgs Cancel {
            get {
                return this.cancel;
            }
            set {
                this.cancel = value;
            }
        }
 
        public Node Dst {
            get {
                return this.dst;
            }
        }
 
        public Link Link {
            get {
                return this.link;
            }
        }
 
        private CancelEventArgs cancel;
 
        private Node dst;
 
        private Link link;
    }

    public class BeforeChangeOrgEventArgs : EventArgs {
 
        public BeforeChangeOrgEventArgs(Link link, Node org) {
            this.cancel = new CancelEventArgs(false);
            this.link = link;
            this.org = org;
        }
 
        public CancelEventArgs Cancel {
            get {
                return this.cancel;
            }
            set {
                this.cancel = value;
            }
        }
 
        public Link Link {
            get {
                return this.link;
            }
        }
 
        public Node Org {
            get {
                return this.org;
            }
        }
 
        private CancelEventArgs cancel;
 
        private Link link;
 
        private Node org;
    }

    public class BeforeEditEventArgs : EventArgs {

        public BeforeEditEventArgs(Node node) {
            this.Cancel = new CancelEventArgs(false);
            this.node = node;
        }
 
        public CancelEventArgs Cancel {
            get {
                return this.cancel;
            }
            set {
                this.cancel = value;
            }
        }
 
        public Node Node {
            get {
                return this.node;
            }
        }
 
        private CancelEventArgs cancel;
 
        private Node node;
 
    }

    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>

    internal class ComparePoints : IComparer {
        // Methods
        public ComparePoints() {
        }
 
        public int Compare(object o1, object o2) {
            PointF tf1 = (PointF) o1;
            PointF tf2 = (PointF) o2;
            int num1 = (int) ((tf1.X * tf1.X) + (tf1.Y * tf1.Y));
            int num2 = (int) ((tf2.X * tf2.X) + (tf2.Y * tf2.Y));
            if (num1 == num2) {
                return 0;
            }
            if (num1 <= num2) {
                return -1;
            }
            return 1;
        }
    }
 
    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>

    internal class Connect {

        private Connect() {
        }
 
        internal Connect(AddFlow af) {
            this.m_af = af;
            this.m_postOrderNumber = 0;
            this.m_connectedItems = new ItemsCollection();
        }
 
        private void DFS(Node node) {
            node.m_visited = true;
            foreach (Link link1 in node.Links) {
                if ((link1.Reflexive || !link1.m_logical) || ((link1.m_org != node) || link1.m_visited)) {
                    continue;
                }
                link1.m_visited = true;
                Node node1 = link1.m_dst;
                if (node1.m_logical && !node1.m_visited) {
                    this.DFS(node1);
                }
            }
            node.m_postOrderNumber = this.m_postOrderNumber++;
        }
 
        internal ItemsCollection GetConnectedPart(Node startNode) {
            this.ResetGraph();
            this.m_connectedItems.Clear();
            this.Visit(startNode);
            return this.m_connectedItems;
        }
 
        internal bool IsAcyclicGraph() {
            bool flag1 = true;
            this.ResetGraph();
            foreach (Node node1 in this.m_af.Nodes) {
                if ((node1.m_logical && !node1.m_visited) && this.VisitNode(node1)) {
                    return false;
                }
            }
            return flag1;
        }
 
        internal bool IsCycle(Node node) {
            if (!node.m_logical) {
                return false;
            }
            this.ResetGraph();
            return this.VisitNode(node);
        }
 
        internal bool IsCycleDirected() {
            bool flag1 = false;
            this.ResetGraph();
            this.m_postOrderNumber = 0;
            foreach (Node node1 in this.m_af.Nodes) {
                if (node1.Logical && !node1.m_visited) {
                    this.DFS(node1);
                }
            }
            foreach (Node node2 in this.m_af.Nodes) {
                if (node2.m_logical) {
                    foreach (Link link1 in node2.Links) {
                        if (((!link1.Reflexive && link1.m_logical) && (link1.m_dst.m_logical && link1.m_org.m_logical)) && (link1.m_dst.m_postOrderNumber > link1.m_org.m_postOrderNumber)) {
                            flag1 = true;
                            break;
                        }
                    }
                }
                if (flag1) {
                    return flag1;
                }
            }
            return flag1;
        }
 
        internal bool IsDirectedAcyclicGraph() {
            return !this.IsCycleDirected();
        }
 
        private void ResetGraph() {
            foreach (Item item1 in this.m_af.Items) {
                item1.m_visited = false;
            }
        }
 
        private void Visit(Node node) {
            node.m_visited = true;
            this.m_connectedItems.Add(node);
            foreach (Link link1 in node.Links) {
                if ((link1.Reflexive || !link1.m_logical) || link1.m_visited) {
                    continue;
                }
                link1.m_visited = true;
                this.m_connectedItems.Add(link1);
                Node node1 = (node == link1.m_org) ? link1.m_dst : link1.m_org;
                if (node1.m_logical && !node1.m_visited) {
                    this.Visit(node1);
                }
            }
        }
 
        private bool VisitNode(Node node) {
            bool flag1 = false;
            node.m_visited = true;
            foreach (Link link1 in node.Links) {
                if ((link1.Reflexive || !link1.m_logical) || link1.m_visited) {
                    continue;
                }
                link1.m_visited = true;
                Node node1 = (node == link1.m_org) ? link1.m_dst : link1.m_org;
                if (node1.m_logical) {
                    if (node1.m_visited) {
                        flag1 = true;
                    }
                    else {
                        flag1 = this.VisitNode(node1);
                    }
                    if (flag1) {
                        return flag1;
                    }
                }
            }
            return flag1;
        }
 
        private AddFlow m_af;
 
        private ItemsCollection m_connectedItems;
 
        private int m_postOrderNumber;
    }
 
    /// <summary>
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// ////////////////////////////////////////////////////////////////
    /// </summary>

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class DefLink : ICloneable {
 
        internal delegate void ChangeEventHandler(object sender, EventArgs e);

 
        public DefLink() {
            this.Reset();
        }
 
        public object Clone() {
            DefLink link1 = new DefLink();
            link1.m_font = (Font) this.m_font.Clone();
            link1.m_textColor = this.m_textColor;
            link1.m_drawColor = this.m_drawColor;
            link1.m_hidden = this.m_hidden;
            link1.m_logical = this.m_logical;
            link1.m_maxPointsCount = this.m_maxPointsCount;
            link1.m_selectable = this.m_selectable;
            link1.m_ownerDraw = this.m_ownerDraw;
            link1.m_dashStyle = this.m_dashStyle;
            link1.m_drawWidth = this.m_drawWidth;
            link1.m_backMode = this.m_backMode;
            link1.m_jump = this.m_jump;
            link1.m_line = (Line) this.m_line.Clone();
            link1.m_startCap = this.m_startCap;
            link1.m_endCap = this.m_endCap;
            link1.m_customStartCap = this.m_customStartCap;
            link1.m_customEndCap = this.m_customEndCap;
            link1.m_arrowDst = (Arrow) this.m_arrowDst.Clone();
            link1.m_arrowOrg = (Arrow) this.m_arrowOrg.Clone();
            link1.m_arrowMid = (Arrow) this.m_arrowMid.Clone();
            link1.m_rigid = this.m_rigid;
            link1.m_adjustOrg = this.m_adjustOrg;
            link1.m_adjustDst = this.m_adjustDst;
            link1.m_orientedText = this.m_orientedText;
            link1.m_stretchable = this.m_stretchable;
            link1.m_text = this.m_text;
            link1.m_tooltip = this.m_tooltip;
            link1.m_url = this.m_url;
            return link1;
        }
 
        internal bool IsDefault() {
            if (((((this.m_textColor.Equals(SystemColors.WindowText) 
                && this.m_drawColor.Equals(SystemColors.WindowText)) 
                && ((this.m_dashStyle == DashStyle.Solid) && (this.m_drawWidth == 1))) 
                && (((this.m_jump == Jump.None) && this.m_line.IsDefault()) && ((this.m_startCap == LineCap.Flat) && (this.m_endCap == LineCap.Flat)))) 
                && ((((this.m_customStartCap == null) && (this.m_customEndCap == null)) && (this.m_arrowDst.IsDefault2() && this.m_arrowOrg.IsDefault())) 
                && ((this.m_arrowMid.IsDefault() && (this.m_backMode == BackMode.Transparent)) && (this.m_font.Equals(Control.DefaultFont) && !this.m_hidden)))) 
                && ((((this.m_logical && this.m_selectable) && (!this.m_ownerDraw && !this.m_rigid)) && ((!this.m_adjustOrg && !this.m_adjustDst) && (!this.m_orientedText && this.m_stretchable))) 
                && ((this.m_text == null) && (this.m_tooltip == null)))
                && (this.m_maxPointsCount == 0)
                ) {
                return (this.m_url == null);
            }
            return false;
        }
 
        internal void OnChange(EventArgs e) {
            if (this.Change != null) {
                this.Change(this, e);
            }
        }
 
        internal void Reset() {
            this.m_textColor = SystemColors.WindowText;
            this.m_drawColor = SystemColors.WindowText;
            this.m_dashStyle = DashStyle.Solid;
            this.m_drawWidth = 1;
            this.m_jump = Jump.None;
            this.m_line = new Line();
            this.m_startCap = LineCap.Flat;
            this.m_endCap = LineCap.Flat;
            this.m_customStartCap = null;
            this.m_customEndCap = null;
            this.m_arrowDst = new Arrow();
            this.m_arrowDst.m_style = ArrowStyle.Arrow;
            this.m_arrowOrg = new Arrow();
            this.m_arrowMid = new Arrow();
            this.m_backMode = BackMode.Transparent;
            this.m_font = Control.DefaultFont;
            this.m_hidden = false;
            this.m_logical = true;
            this.m_maxPointsCount = 0;
            this.m_selectable = true;
            this.m_ownerDraw = false;
            this.m_rigid = false;
            this.m_adjustOrg = false;
            this.m_adjustDst = false;
            this.m_orientedText = false;
            this.m_stretchable = true;
            this.m_text = null;
            this.m_tooltip = null;
            this.m_url = null;
        }
 
        public void ResetArrowDst() {
            this.m_arrowDst.Reset2();
            this.OnChange(EventArgs.Empty);
        }
 
        public void ResetArrowMid() {
            this.m_arrowMid.Reset();
            this.OnChange(EventArgs.Empty);
        }
 
        public void ResetArrowOrg() {
            this.m_arrowOrg.Reset();
            this.OnChange(EventArgs.Empty);
        }
 
        public void ResetDrawColor() {
            this.m_drawColor = SystemColors.WindowText;
            this.OnChange(EventArgs.Empty);
        }
 
        public void ResetFont() {
            this.m_font = Control.DefaultFont;
        }
 
        public void ResetLine() {
            this.m_line.Reset();
            this.OnChange(EventArgs.Empty);
        }
 
        public void ResetTextColor() {
            this.m_textColor = SystemColors.WindowText;
            this.OnChange(EventArgs.Empty);
        }
 
        public bool ShouldSerializeArrowDst() {
            return !this.m_arrowDst.IsDefault2();
        }
 
        public bool ShouldSerializeArrowMid() {
            return !this.m_arrowMid.IsDefault();
        }
 
        public bool ShouldSerializeArrowOrg() {
            return !this.m_arrowOrg.IsDefault();
        }
 
        public bool ShouldSerializeDrawColor() {
            return (this.m_drawColor != SystemColors.WindowText);
        }
 
        public bool ShouldSerializeFont() {
            return !this.m_font.Equals(Control.DefaultFont);
        }
 
        public bool ShouldSerializeLine() {
            return !this.m_line.IsDefault();
        }
 
        public bool ShouldSerializeTextColor() {
            return (this.m_textColor != SystemColors.WindowText);
        }
 
        [Description("Determines whether, by default, you can adjust the destination point of links."), DefaultValue(false), NotifyParentProperty(true)]
        public bool AdjustDst {
            get {
                return this.m_adjustDst;
            }
            set {
                if (this.m_adjustDst != value) {
                    this.m_adjustDst = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), Description("Determines whether, by default, you can adjust the origin point of links."), DefaultValue(false)]
        public bool AdjustOrg {
            get {
                return this.m_adjustOrg;
            }
            set {
                if (this.m_adjustOrg != value) {
                    this.m_adjustOrg = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true), Description("Returns/sets the default destination arrow for links.")]
        public Arrow ArrowDst {
            get {
                return this.m_arrowDst;
            }
            set {
                if (this.m_arrowDst != value) {
                    this.m_arrowDst = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default middle segment arrow shape for links."), NotifyParentProperty(true), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Arrow ArrowMid {
            get {
                return this.m_arrowMid;
            }
            set {
                if (this.m_arrowMid != value) {
                    this.m_arrowMid = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default origin arrow for links."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true)]
        public Arrow ArrowOrg {
            get {
                return this.m_arrowOrg;
            }
            set {
                if (this.m_arrowOrg != value) {
                    this.m_arrowOrg = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default background mode used for the text of links."), DefaultValue(0), NotifyParentProperty(true)]
        public BackMode BackMode {
            get {
                return this.m_backMode;
            }
            set {
                if (this.m_backMode != value) {
                    this.m_backMode = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false), NotifyParentProperty(true), Description("Returns/sets the default user-defined ending cap for links."), DefaultValue((string) null)]
        public CustomLineCap CustomEndCap {
            get {
                return this.m_customEndCap;
            }
            set {
                if (this.m_customEndCap != value) {
                    this.m_customEndCap = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue((string) null), Description("Returns/sets the default user-defined starting cap for links."), NotifyParentProperty(true)]
        public CustomLineCap CustomStartCap {
            get {
                return this.m_customStartCap;
            }
            set {
                if (this.m_customStartCap != value) {
                    this.m_customStartCap = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(0), Description("Returns/sets the default style (solid, dot, ...) of the pen used to draw links."), NotifyParentProperty(true)]
        public DashStyle DashStyle {
            get {
                return this.m_dashStyle;
            }
            set {
                if ((this.m_dashStyle != value) && (value != DashStyle.Custom)) {
                    this.m_dashStyle = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), Description("Returns/sets the default drawing color for links.")]
        public Color DrawColor {
            get {
                return this.m_drawColor;
            }
            set {
                if (this.m_drawColor != value) {
                    this.m_drawColor = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), Description("Returns/sets the default width of the pen used to draw links."), DefaultValue(1)]
        public int DrawWidth {
            get {
                return this.m_drawWidth;
            }
            set {
                if (value < 0) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.DrawWidthCannotBeNegative));
                }
                if (this.m_drawWidth != value) {
                    this.m_drawWidth = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), Description("Returns/sets the default cap used at the end of links."), DefaultValue(0)]
        public LineCap EndCap {
            get {
                return this.m_endCap;
            }
            set {
                if (this.m_endCap != value) {
                    this.m_endCap = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default font used for links."), NotifyParentProperty(true)]
        public Font Font {
            get {
                return this.m_font;
            }
            set {
                if (this.m_font != value) {
                    this.m_font = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), DefaultValue(false), Description("Determines whether, by default, links are hidden or not.")]
        public bool Hidden {
            get {
                return this.m_hidden;
            }
            set {
                if (this.m_hidden != value) {
                    this.m_hidden = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), DefaultValue(0), Description("Returns/sets a value that determines whether jumps are, by default, displayed at the intersection of links.")]
        public Jump Jump {
            get {
                return this.m_jump;
            }
            set {
                if (this.m_jump != value) {
                    this.m_jump = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default link line."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content), NotifyParentProperty(true)]
        public Line Line {
            get {
                return this.m_line;
            }
            set {
                if (this.m_line != value) {
                    this.m_line = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), DefaultValue(true), Description("Determines whether, by default, links are logical or not.")]
        public bool Logical {
            get {
                return this.m_logical;
            }
            set {
                if (this.m_logical != value) {
                    this.m_logical = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(0), Description("")]
        public int MaxPointsCount {
            get {
                return this.m_maxPointsCount;
            }
            set {
                this.m_maxPointsCount = value;
                if (this.m_maxPointsCount<0) this.m_maxPointsCount = 0;
            }
        }
 
        [DefaultValue(false), Description("Determines whether, by default, the text of links is oriented or not."), NotifyParentProperty(true)]
        public bool OrientedText {
            get {
                return this.m_orientedText;
            }
            set {
                if (this.m_orientedText != value) {
                    this.m_orientedText = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(false), Description("Determines whether, by default, links are ownerdraw or not."), NotifyParentProperty(true)]
        public bool OwnerDraw {
            get {
                return this.m_ownerDraw;
            }
            set {
                if (this.m_ownerDraw != value) {
                    this.m_ownerDraw = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(false), NotifyParentProperty(true), Description("Determines whether links are by default rigid or not.")]
        public bool Rigid {
            get {
                return this.m_rigid;
            }
            set {
                if (this.m_rigid != value) {
                    this.m_rigid = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Determines whether, by default, links are selectable or not."), DefaultValue(true), NotifyParentProperty(true)]
        public bool Selectable {
            get {
                return this.m_selectable;
            }
            set {
                if (this.m_selectable != value) {
                    this.m_selectable = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default cap used at the start of links."), NotifyParentProperty(true), DefaultValue(0)]
        public LineCap StartCap {
            get {
                return this.m_startCap;
            }
            set {
                if (this.m_startCap != value) {
                    this.m_startCap = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Determines whether, by default, you can stretch the links."), NotifyParentProperty(true), DefaultValue(true)]
        public bool Stretchable {
            get {
                return this.m_stretchable;
            }
            set {
                if (this.m_stretchable != value) {
                    this.m_stretchable = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        public string Text {
            get {
                return this.m_text;
            }
            set {
                if (this.m_text != value) {
                    this.m_text = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default text color for links."), NotifyParentProperty(true)]
        public Color TextColor {
            get {
                return this.m_textColor;
            }
            set {
                if (this.m_textColor != value) {
                    this.m_textColor = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        public string Tooltip {
            get {
                return this.m_tooltip;
            }
            set {
                if (this.m_tooltip != value) {
                    this.m_tooltip = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        internal event DefLink.ChangeEventHandler Change;
 
        //private DefLink.ChangeEventHandler Change;
 
        internal bool m_adjustDst;
 
        internal bool m_adjustOrg;
 
        internal Arrow m_arrowDst;
 
        internal Arrow m_arrowMid;
 
        internal Arrow m_arrowOrg;
 
        internal BackMode m_backMode;
 
        internal CustomLineCap m_customEndCap;
 
        internal CustomLineCap m_customStartCap;
 
        internal DashStyle m_dashStyle;
 
        internal Color m_drawColor;
 
        internal int m_drawWidth;
 
        internal LineCap m_endCap;
 
        internal Font m_font;
 
        internal bool m_hidden;
 
        internal Jump m_jump;
 
        internal Line m_line;
 
        internal bool m_logical;

        internal int m_maxPointsCount;
 
        internal bool m_orientedText;
 
        internal bool m_ownerDraw;
 
        internal bool m_rigid;
 
        internal bool m_selectable;
 
        internal LineCap m_startCap;
 
        internal bool m_stretchable;
 
        internal string m_text;
 
        internal Color m_textColor;
 
        internal string m_tooltip;
 
        internal string m_url;
    }

    [Serializable, TypeConverter(typeof(ExpandableObjectConverter))]
    public class DefNode : ICloneable {

        internal delegate void ChangeEventHandler(object sender, EventArgs e);
 
        public DefNode() {
            this.Reset();
        }
 
        public object Clone() {
            DefNode node1 = new DefNode();
            node1.m_font = (Font) this.m_font.Clone();
            node1.m_textColor = this.m_textColor;
            node1.m_drawColor = this.m_drawColor;
            node1.m_fillColor = this.m_fillColor;
            node1.m_gradientColor = this.m_gradientColor;
            node1.m_gradientMode = this.m_gradientMode;
            node1.m_hidden = this.m_hidden;
            node1.m_logical = this.m_logical;
            node1.m_selectable = this.m_selectable;
            node1.m_ownerDraw = this.m_ownerDraw;
            node1.m_labelEdit = this.m_labelEdit;
            node1.m_dashStyle = this.m_dashStyle;
            node1.m_drawWidth = this.m_drawWidth;
            node1.m_backMode = this.m_backMode;
            node1.m_transparent = this.m_transparent;
            node1.m_gradient = this.m_gradient;
            node1.m_shape = (Shape) this.m_shape.Clone();
            node1.m_shadow = (Shadow) this.m_shadow.Clone();
            node1.m_alignment = this.m_alignment;
            node1.m_autoSize = this.m_autoSize;
            node1.m_imagePosition = this.m_imagePosition;
            node1.m_trimming = this.m_trimming;
            node1.m_xMoveable = this.m_xMoveable;
            node1.m_yMoveable = this.m_yMoveable;
            node1.m_xSizeable = this.m_xSizeable;
            node1.m_ySizeable = this.m_ySizeable;
            node1.m_inLinkable = this.m_inLinkable;
            node1.m_outLinkable = this.m_outLinkable;
            node1.m_textMargin = this.m_textMargin;
            node1.m_imageIndex = this.m_imageIndex;
            node1.m_imageLocation = this.m_imageLocation;
            node1.m_text = this.m_text;
            node1.m_tooltip = this.m_tooltip;
            node1.m_url = this.m_url;
            return node1;
        }
 
        internal bool IsDefault() {
            if ((((((this.m_textColor.Equals(SystemColors.WindowText) && this.m_drawColor.Equals(SystemColors.WindowText)) && (this.m_fillColor.Equals(SystemColors.Window) && this.m_gradientColor.Equals(SystemColors.Window))) && (((this.m_gradientMode == LinearGradientMode.BackwardDiagonal) && (this.m_dashStyle == DashStyle.Solid)) && ((this.m_drawWidth == 1) && this.m_shape.IsDefault()))) && (((this.m_shadow.IsDefault() && (this.m_backMode == BackMode.Transparent)) && ((this.m_alignment == Alignment.CenterMIDDLE) && (this.m_autoSize == AutoSize.None))) && (((this.m_imagePosition == ImagePosition.RelativeToText) && (this.m_trimming == StringTrimming.None)) && (!this.ShouldSerializeTextMargin() && this.m_font.Equals(Control.DefaultFont))))) && ((((!this.m_hidden && !this.m_transparent) && (!this.m_gradient && this.m_logical)) && ((this.m_selectable && !this.m_ownerDraw) && (this.m_labelEdit && this.m_xMoveable))) && (((this.m_yMoveable && this.m_xSizeable) && (this.m_ySizeable && this.m_inLinkable)) && ((this.m_outLinkable && (this.m_imageIndex == -1)) && (!this.ShouldSerializeImageLocation() && (this.m_text == null)))))) && (this.m_tooltip == null)) {
                return (this.m_url == null);
            }
            return false;
        }
 
        internal virtual void OnChange(EventArgs e) {
            if (this.Change != null) {
                this.Change(this, e);
            }
        }
 
        internal void Reset() {
            this.m_textColor = SystemColors.WindowText;
            this.m_drawColor = SystemColors.WindowText;
            this.m_fillColor = SystemColors.Window;
            this.m_gradientColor = SystemColors.Control;
            this.m_gradientMode = LinearGradientMode.BackwardDiagonal;
            this.m_dashStyle = DashStyle.Solid;
            this.m_drawWidth = 1;
            this.m_shape = new Shape();
            this.m_shadow = new Shadow();
            this.m_backMode = BackMode.Transparent;
            this.m_alignment = Alignment.CenterMIDDLE;
            this.m_autoSize = AutoSize.None;
            this.m_imagePosition = ImagePosition.RelativeToText;
            this.m_trimming = StringTrimming.None;
            this.ResetTextMargin();
            this.m_font = Control.DefaultFont;
            this.m_hidden = false;
            this.m_transparent = false;
            this.m_gradient = false;
            this.m_logical = true;
            this.m_selectable = true;
            this.m_ownerDraw = false;
            this.m_labelEdit = true;
            this.m_xMoveable = true;
            this.m_yMoveable = true;
            this.m_xSizeable = true;
            this.m_ySizeable = true;
            this.m_inLinkable = true;
            this.m_outLinkable = true;
            this.m_imageIndex = -1;
            this.m_imageLocation = new PointF(0f, 0f);
            this.m_text = null;
            this.m_tooltip = null;
            this.m_url = null;
        }
 
        public void ResetDrawColor() {
            this.m_drawColor = SystemColors.WindowText;
            this.OnChange(EventArgs.Empty);
        }
 
        public void ResetFillColor() {
            this.m_fillColor = SystemColors.Window;
            this.OnChange(EventArgs.Empty);
        }
 
        public void ResetFont() {
            this.m_font = Control.DefaultFont;
        }
 
        public void ResetGradientColor() {
            this.m_gradientColor = SystemColors.Window;
            this.OnChange(EventArgs.Empty);
        }
 
        public void ResetImageLocation() {
            this.m_imageLocation = new PointF(0f, 0f);
        }
 
        public void ResetShadow() {
            this.m_shadow.Reset();
            this.OnChange(EventArgs.Empty);
        }
 
        public void ResetShape() {
            this.m_shape.Reset();
            this.OnChange(EventArgs.Empty);
        }
 
        public void ResetTextColor() {
            this.m_textColor = SystemColors.WindowText;
            this.OnChange(EventArgs.Empty);
        }
 
        public void ResetTextMargin() {
            this.m_textMargin = new Size(0, 0);
            this.OnChange(EventArgs.Empty);
        }
 
        public bool ShouldSerializeDrawColor() {
            return (this.m_drawColor != SystemColors.WindowText);
        }
 
        public bool ShouldSerializeFillColor() {
            return (this.m_fillColor != SystemColors.Window);
        }
 
        public bool ShouldSerializeFont() {
            return !this.m_font.Equals(Control.DefaultFont);
        }
 
        public bool ShouldSerializeGradientColor() {
            return (this.m_gradientColor != SystemColors.Window);
        }
 
        public bool ShouldSerializeImageLocation() {
            if (this.m_imageLocation.X == 0f) {
                return (this.m_imageLocation.Y != 0f);
            }
            return true;
        }
 
        public bool ShouldSerializeShadow() {
            return !this.m_shadow.IsDefault();
        }
 
        public bool ShouldSerializeShape() {
            return !this.m_shape.IsDefault();
        }
 
        public bool ShouldSerializeTextColor() {
            return (this.m_textColor != SystemColors.WindowText);
        }
 
        public bool ShouldSerializeTextMargin() {
            if (this.m_textMargin.Width == 0) {
                return (this.m_textMargin.Height != 0);
            }
            return true;
        }
 
        [Description("Returns/sets the default node text alignment style."), NotifyParentProperty(true), DefaultValue(7)]
        public Alignment Alignment {
            get {
                return this.m_alignment;
            }
            set {
                if (this.m_alignment != value) {
                    this.m_alignment = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(0), NotifyParentProperty(true), Description("Returns/sets the default node AutoSize style.")]
        public AutoSize AutoSize {
            get {
                return this.m_autoSize;
            }
            set {
                if (this.m_autoSize != value) {
                    this.m_autoSize = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(0), NotifyParentProperty(true), Description("Returns/sets the default background mode used for the text of nodes.")]
        public BackMode BackMode {
            get {
                return this.m_backMode;
            }
            set {
                if (this.m_backMode != value) {
                    this.m_backMode = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default style (solid, dot, ...) of the pen used to draw nodes."), NotifyParentProperty(true), DefaultValue(0)]
        public DashStyle DashStyle {
            get {
                return this.m_dashStyle;
            }
            set {
                if ((this.m_dashStyle != value) && (value != DashStyle.Custom)) {
                    this.m_dashStyle = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default drawing color used for nodes."), NotifyParentProperty(true)]
        public Color DrawColor {
            get {
                return this.m_drawColor;
            }
            set {
                if (this.m_drawColor != value) {
                    this.m_drawColor = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(1), Description("Returns/sets the default width of the pen used to draw nodes."), NotifyParentProperty(true)]
        public int DrawWidth {
            get {
                return this.m_drawWidth;
            }
            set {
                if (value < 0) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.DrawWidthCannotBeNegative));
                }
                if (this.m_drawWidth != value) {
                    this.m_drawWidth = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default node filling color."), NotifyParentProperty(true)]
        public Color FillColor {
            get {
                return this.m_fillColor;
            }
            set {
                if (this.m_fillColor != value) {
                    this.m_fillColor = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default font used for nodes."), NotifyParentProperty(true)]
        public Font Font {
            get {
                return this.m_font;
            }
            set {
                if (this.m_font != value) {
                    this.m_font = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(false), NotifyParentProperty(true), Description("Determines whether, by default, nodes are filled with a linear gradient color.")]
        public bool Gradient {
            get {
                return this.m_gradient;
            }
            set {
                if (this.m_gradient != value) {
                    this.m_gradient = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), Description("Returns/sets the default gradient color used for nodes.")]
        public Color GradientColor {
            get {
                return this.m_gradientColor;
            }
            set {
                if (this.m_gradientColor != value) {
                    this.m_gradientColor = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default node direction of the linear gradient."), DefaultValue(3), NotifyParentProperty(true)]
        public LinearGradientMode GradientMode {
            get {
                return this.m_gradientMode;
            }
            set {
                if (this.m_gradientMode != value) {
                    this.m_gradientMode = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), DefaultValue(false), Description("Determines whether, by default, nodes are hidden or not.")]
        public bool Hidden {
            get {
                return this.m_hidden;
            }
            set {
                if (this.m_hidden != value) {
                    this.m_hidden = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the index of the image used by default for nodes."), DefaultValue(-1), NotifyParentProperty(true)]
        public int ImageIndex {
            get {
                return this.m_imageIndex;
            }
            set {
                if (this.m_imageIndex != value) {
                    this.m_imageIndex = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the relative location of the image in the node."), NotifyParentProperty(true)]
        public PointF ImageLocation {
            get {
                return this.m_imageLocation;
            }
            set {
                if (this.m_imageLocation != value) {
                    this.m_imageLocation = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default position of an image in a node."), DefaultValue(9), NotifyParentProperty(true)]
        public ImagePosition ImagePosition {
            get {
                return this.m_imagePosition;
            }
            set {
                if (this.m_imagePosition != value) {
                    this.m_imagePosition = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(true), Description("Determines whether, by default, nodes accept incoming links."), NotifyParentProperty(true)]
        public bool InLinkable {
            get {
                return this.m_inLinkable;
            }
            set {
                if (this.m_inLinkable != value) {
                    this.m_inLinkable = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(true), NotifyParentProperty(true), Description("Determines whether, by default, the text of the nodes is editable or not.")]
        public bool LabelEdit {
            get {
                return this.m_labelEdit;
            }
            set {
                if (this.m_labelEdit != value) {
                    this.m_labelEdit = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(true), Description("Determines whether, by default, nodes are logical or not."), NotifyParentProperty(true)]
        public bool Logical {
            get {
                return this.m_logical;
            }
            set {
                if (this.m_logical != value) {
                    this.m_logical = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Determines whether, by default, nodes accept outgoing links."), DefaultValue(true), NotifyParentProperty(true)]
        public bool OutLinkable {
            get {
                return this.m_outLinkable;
            }
            set {
                if (this.m_outLinkable != value) {
                    this.m_outLinkable = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Determines whether, by default, nodes are ownerdraw or not."), NotifyParentProperty(true), DefaultValue(false)]
        public bool OwnerDraw {
            get {
                return this.m_ownerDraw;
            }
            set {
                if (this.m_ownerDraw != value) {
                    this.m_ownerDraw = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(true), NotifyParentProperty(true), Description("Determines whether, by default, nodes are selectable or not.")]
        public bool Selectable {
            get {
                return this.m_selectable;
            }
            set {
                if (this.m_selectable != value) {
                    this.m_selectable = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Returns/sets the default node shadow.")]
        public Shadow Shadow {
            get {
                return this.m_shadow;
            }
            set {
                if (this.m_shadow != value) {
                    this.m_shadow = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default node shape."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Shape Shape {
            get {
                return this.m_shape;
            }
            set {
                if (this.m_shape != value) {
                    this.m_shape = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        public string Text {
            get {
                return this.m_text;
            }
            set {
                if (this.m_text != value) {
                    this.m_text = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default text color used for nodes."), NotifyParentProperty(true)]
        public Color TextColor {
            get {
                return this.m_textColor;
            }
            set {
                if (this.m_textColor != value) {
                    this.m_textColor = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), Description("Returns/sets the default margins used to display the text inside nodes.")]
        public Size TextMargin {
            get {
                return this.m_textMargin;
            }
            set {
                if (((value.Width < 0) || (value.Width > 50)) || ((value.Height < 0) || (value.Height > 50))) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.TextMarginLimits));
                }
                if (this.m_textMargin != value) {
                    this.m_textMargin = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        public string Tooltip {
            get {
                return this.m_tooltip;
            }
            set {
                if (this.m_tooltip != value) {
                    this.m_tooltip = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(false), NotifyParentProperty(true), Description("Determines whether, by default, nodes are transparent or not.")]
        public bool Transparent {
            get {
                return this.m_transparent;
            }
            set {
                if (this.m_transparent != value) {
                    this.m_transparent = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the default string trimming of the text in nodes."), NotifyParentProperty(true), DefaultValue(0)]
        public StringTrimming Trimming {
            get {
                return this.m_trimming;
            }
            set {
                if (this.m_trimming != value) {
                    this.m_trimming = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), DefaultValue(true), Description("Determines whether, by default, nodes can be moved horizontally or not.")]
        public bool XMoveable {
            get {
                return this.m_xMoveable;
            }
            set {
                if (this.m_xMoveable != value) {
                    this.m_xMoveable = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(true), NotifyParentProperty(true), Description("Determines whether, by default, nodes can be resized horizontally or not.")]
        public bool XSizeable {
            get {
                return this.m_xSizeable;
            }
            set {
                if (this.m_xSizeable != value) {
                    this.m_xSizeable = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), DefaultValue(true), Description("Determines whether, by default, nodes can be moved vertically or not.")]
        public bool YMoveable {
            get {
                return this.m_yMoveable;
            }
            set {
                if (this.m_yMoveable != value) {
                    this.m_yMoveable = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), DefaultValue(true), Description("Determines whether, by default, nodes can be resized vertically or not.")]
        public bool YSizeable {
            get {
                return this.m_ySizeable;
            }
            set {
                if (this.m_ySizeable != value) {
                    this.m_ySizeable = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        internal event DefNode.ChangeEventHandler Change;
 
        //private DefNode.ChangeEventHandler Change;
 
        internal Alignment m_alignment;
 
        internal AutoSize m_autoSize;
 
        internal BackMode m_backMode;
 
        internal DashStyle m_dashStyle;
 
        internal Color m_drawColor;
 
        internal int m_drawWidth;
 
        internal Color m_fillColor;
 
        internal Font m_font;
 
        internal bool m_gradient;
 
        internal Color m_gradientColor;
 
        internal LinearGradientMode m_gradientMode;
 
        internal bool m_hidden;
 
        internal int m_imageIndex;
 
        internal PointF m_imageLocation;
 
        internal ImagePosition m_imagePosition;
 
        internal bool m_inLinkable;
 
        internal bool m_labelEdit;
 
        internal bool m_logical;
 
        internal bool m_outLinkable;
 
        internal bool m_ownerDraw;
 
        internal bool m_selectable;
 
        internal Shadow m_shadow;
 
        internal Shape m_shape;
 
        internal string m_text;
 
        internal Color m_textColor;
 
        internal Size m_textMargin;
 
        internal string m_tooltip;
 
        internal bool m_transparent;
 
        internal StringTrimming m_trimming;
 
        internal string m_url;
 
        internal bool m_xMoveable;
 
        internal bool m_xSizeable;
 
        internal bool m_yMoveable;
 
        internal bool m_ySizeable;
    }

    internal class DeleteLinkTask : Task {
 
        private DeleteLinkTask() {
        }
 
        internal DeleteLinkTask(Link link) {
            this.m_link = link;
            this.m_oldZorder = this.m_link.GetZOrder();
            this.m_af = this.m_link.m_af;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.LinkRemove;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            if (this.m_link.m_selected) {
                this.m_link.Selected = false;
            }
            this.m_link.InvalidateItem();
            this.m_af.RemoveLinkFromGraph(this.m_link);
        }
 
        internal override void Undo() {
            this.m_af.AddLinkToGraph(this.m_link);
            this.m_link.SetZOrder2(this.m_oldZorder);
        }
 
        private AddFlow m_af;
 
        private Link m_link;
 
        private int m_oldZorder;
    }

    internal class DeleteNodeTask : Task {

        private DeleteNodeTask() {
        }
 
        internal DeleteNodeTask(Node node, int zorder) {
            this.m_node = node;
            this.m_oldZorder = zorder;
            this.m_af = this.m_node.m_af;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.NodeRemove;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            if (this.m_node.m_selected) {
                this.m_node.Selected = false;
            }
            this.m_node.InvalidateItem();
            this.m_af.RemoveNodeFromGraph(this.m_node);
        }
 
        internal override void Undo() {
            this.m_af.AddNodeToGraph(this.m_node);
            this.m_node.SetZOrder2(this.m_oldZorder);
        }
 
        private AddFlow m_af;
 
        private Node m_node;
 
        private int m_oldZorder;
    }

    public class DiagramOwnerDrawEventArgs : EventArgs {

        public DiagramOwnerDrawEventArgs(Graphics graphics) {
            this.grfx = graphics;
            this.Flags = DrawFlags.All;
        }
 
        public DrawFlags Flags {
            get {
                return this.flags;
            }
            set {
                this.flags = value;
            }
        }
 
        public Graphics Graphics {
            get {
                return this.grfx;
            }
        }
 
        private DrawFlags flags;
 
        private Graphics grfx;
    }
 
    public enum DrawFlags {
        // Fields
        All = 1,
        None = 0
    }

    public class ErrorEventArgs : EventArgs {
 
        public ErrorEventArgs(ErrorEventType errorEventType) {
            this.errorEventType = errorEventType;
        }
 
        public ErrorEventType ErrorEventType {
            get {
                return this.errorEventType;
            }
        }
 
        private ErrorEventType errorEventType;
    }

    public enum ErrorEventType {
        // Fields
        CycleError = 0,
        DirectedCycleError = 1,
        IncomingLinkError = 4,
        MultilinkError = 2,
        OutgoingLinkError = 5,
        ReflexiveError = 3
    }
 
    public class FlowImage {
 
        internal delegate void AfterChangeEventHandler(object sender, EventArgs e);
 
        internal delegate void BeforeChangeEventHandler(object sender, EventArgs e);
 
        public FlowImage() {
            this.m_image = null;
            this.m_url = null;
        }
 
        public FlowImage(Image image) {
            this.m_image = image;
            this.m_url = null;
        }
 
        public FlowImage(string url) {
            this.m_url = url;
            if ((this.m_url != null) && (this.m_url.Length > 0)) {
                if (this.m_image != null) {
                    this.m_image.Dispose();
                }
                this.m_image = this.GetImage(this.m_url);
            }
        }
 
        private Image GetImage(string url) {
            WebRequest request1 = WebRequest.Create(url);
            WebResponse response1 = request1.GetResponse();
            Stream stream1 = response1.GetResponseStream();
            Image image1 = Image.FromStream(stream1);
            stream1.Close();
            return image1;
        }
 
        internal virtual void OnAfterChange(EventArgs e) {
            if (this.AfterChange != null) {
                this.AfterChange(this, e);
            }
        }
 
        internal virtual void OnBeforeChange(EventArgs e) {
            if (this.BeforeChange != null) {
                this.BeforeChange(this, e);
            }
        }
 
        [NotifyParentProperty(true), Description("Returns/sets the Image object.")]
        public Image Image {
            get {
                return this.m_image;
            }
            set {
                if (this.m_image != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_image = value;
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), Description("Returns/sets the url of the image object.")]
        public string Url {
            get {
                return this.m_url;
            }
            set {
                if (this.m_url != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_url = value;
                    if (this.m_image != null) {
                        this.m_image.Dispose();
                    }
                    this.m_image = this.GetImage(this.m_url);
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }
 
        internal event FlowImage.AfterChangeEventHandler AfterChange;
 
        internal event FlowImage.BeforeChangeEventHandler BeforeChange;
 
        //private FlowImage.AfterChangeEventHandler AfterChange;
 
        //private FlowImage.BeforeChangeEventHandler BeforeChange;
 
        internal Image m_image;
 
        internal string m_url;
    }

    [Serializable, TypeConverter(typeof(GridConverter))]
    public class Grid : ICloneable {
 
        internal delegate void ChangeEventHandler(object sender, EventArgs e);
 
        static Grid() {
            Grid.None = new Grid();
        }
 
        public Grid() {
            this.Reset();
        }
 
        public Grid(string format) {
            char[] chArray1 = new char[1] { ';' } ;
            string[] textArray1 = format.Split(chArray1);
            string text1 = textArray1[4].Trim();
            text1 = text1.Substring(1, text1.Length - 2);
            chArray1 = new char[1] { ',' } ;
            string[] textArray2 = text1.Split(chArray1);
            text1 = textArray2[0].Trim();
            chArray1 = new char[1] { '=' } ;
            string[] textArray3 = text1.Split(chArray1);
            text1 = textArray2[1].Trim();
            chArray1 = new char[1] { '=' } ;
            string[] textArray4 = text1.Split(chArray1);
            Size size1 = new Size(Convert.ToInt32(textArray3[1].Trim()), Convert.ToInt32(textArray4[1].Trim()));
            if ((size1.Width < 0) || (size1.Height < 0)) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.GridSizeCannotBeNegative));
            }
            this.m_size = size1;
            chArray1 = new char[1] { '=' } ;
            textArray2 = textArray1[0].Split(chArray1);
            this.m_draw = textArray2[1].Trim() == bool.TrueString;
            chArray1 = new char[1] { '=' } ;
            textArray2 = textArray1[1].Split(chArray1);
            this.m_snap = textArray2[1].Trim() == bool.TrueString;
            this.m_style = (GridStyle) Enum.Parse(typeof(GridStyle), textArray1[2].Trim(), false);
            this.m_color = Color.FromName(textArray1[3].Trim());
        }
 
        public Grid(bool draw, bool snap, GridStyle style, Color color, Size size) {
            if ((size.Width < 0) || (size.Height < 0)) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.GridSizeCannotBeNegative));
            }
            this.m_draw = draw;
            this.m_snap = snap;
            this.m_style = style;
            this.m_color = color;
            this.m_size = size;
        }
 
        internal PointF Adjust(PointF pt) {
            Point point1 = new Point((int) pt.X, (int) pt.Y);
            if (this.m_size.Width > 0) {
                int num1 = point1.X % this.m_size.Width;
                if (num1 < (this.m_size.Width / 2)) {
                    point1.X -= num1;
                }
                else {
                    point1.X += (this.m_size.Width - num1);
                }
            }
            if (this.m_size.Height > 0) {
                int num2 = point1.Y % this.m_size.Height;
                if (num2 < (this.m_size.Height / 2)) {
                    point1.Y -= num2;
                }
                else {
                    point1.Y += (this.m_size.Height - num2);
                }
            }
            return new PointF((float) point1.X, (float) point1.Y);
        }
 
        public object Clone() {
            return new Grid(this.m_draw, this.m_snap, this.m_style, this.m_color, this.m_size);
        }
 
        internal void Display(Graphics grfx, Rectangle rc) {
            Size size1;
            Pen pen1;
            float single2;
            float single3;
            float single4;
            float single5;
            if ((this.m_size.Width != 0) && (this.m_size.Height != 0)) {
                size1 = this.m_size;
                pen1 = new Pen(this.m_color);
                float single1 = (rc.Left / size1.Width) * size1.Width;
                single2 = (rc.Top / size1.Height) * size1.Height;
                single3 = rc.Right;
                single4 = rc.Bottom;
                switch (this.m_style) {
                case GridStyle.Pixels: {
                    float[] singleArray2 = new float[2] { 1f, (float) (size1.Height - 1) } ;
                    float[] singleArray1 = singleArray2;
                    pen1.DashPattern = singleArray1;
                    single5 = single1;
                    goto Label_00DB;
                }
                case GridStyle.Lines: {
                    pen1.Width = 0f;
                    for (float single6 = single1; single6 <= single3; single6 += size1.Width) {
                        grfx.DrawLine(pen1, single6, single2, single6, single4);
                    }
                    for (float single7 = single2; single7 <= single4; single7 += size1.Height) {
                        grfx.DrawLine(pen1, single1, single7, single3, single7);
                    }
                    return;
                }
                case GridStyle.DottedLines: {
                    pen1.Width = 0f;
                    pen1.DashStyle = DashStyle.Dot;
                    for (float single8 = single1; single8 <= single3; single8 += size1.Width) {
                        grfx.DrawLine(pen1, single8, single2, single8, single4);
                    }
                    for (float single9 = single2; single9 <= single4; single9 += size1.Height) {
                        grfx.DrawLine(pen1, single1, single9, single3, single9);
                    }
                    return;
                }
                }
            }
            return;
            Label_00DB:
                if (single5 > single3) {
                    return;
                }
            grfx.DrawLine(pen1, single5, single2, single5, single4);
            single5 += size1.Width;
            goto Label_00DB;
        }
 
        internal bool Equals(object obj) {
            if (obj is Grid) {
                Grid grid1 = (Grid) obj;
                if ((((this.m_draw == grid1.m_draw) && (this.m_snap == grid1.m_snap)) && ((this.m_style == grid1.m_style) && (this.m_color == grid1.m_color))) && (this.m_size.Width == grid1.m_size.Width)) {
                    return (this.m_size.Height == grid1.m_size.Height);
                }
            }
            return false;
        }
 
        internal bool IsDefault() {
            if (((!this.m_draw && !this.m_snap) && ((this.m_style == GridStyle.DottedLines) && (this.m_color == SystemColors.GrayText))) && (this.m_size.Width == 0x10)) {
                return (this.m_size.Height == 0x10);
            }
            return false;
        }
 
        internal virtual void OnChange(EventArgs e) {
            if (this.Change != null) {
                this.Change(this, e);
            }
        }
 
        internal void Reset() {
            this.m_draw = false;
            this.m_snap = false;
            this.m_style = GridStyle.DottedLines;
            this.m_color = SystemColors.GrayText;
            this.m_size = new Size(0x10, 0x10);
        }
 
        public void ResetColor() {
            this.m_color = SystemColors.GrayText;
        }
 
        public void ResetSize() {
            this.m_size.Width = 0x10;
            this.m_size.Height = 0x10;
        }
 
        public bool ShouldSerializeColor() {
            return (this.m_color != SystemColors.GrayText);
        }
 
        public bool ShouldSerializeSize() {
            if (this.m_size.Width == 0x10) {
                return (this.m_size.Height != 0x10);
            }
            return true;
        }
 
        public override string ToString() {
            StringBuilder builder1 = new StringBuilder("");
            builder1.Append("Draw = " + this.m_draw + "; ");
            builder1.Append("Snap = " + this.m_snap + "; ");
            builder1.Append(this.m_style.ToString() + "; ");
            string text1 = this.m_color.IsNamedColor ? this.m_color.Name : string.Concat(new object[5]);
            builder1.Append(text1 + "; ");
            builder1.Append(this.m_size.ToString());
            return builder1.ToString();
        }
 
        [NotifyParentProperty(true), Description("Returns/sets the grid color.")]
        public Color Color {
            get {
                return this.m_color;
            }
            set {
                if (this.m_color != value) {
                    this.m_color = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Determines whether the grid is displayed or not."), DefaultValue(false), NotifyParentProperty(true)]
        public bool Draw {
            get {
                return this.m_draw;
            }
            set {
                if (this.m_draw != value) {
                    this.m_draw = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), Description("Returns/sets the horizontal and vertical grid size.")]
        public Size Size {
            get {
                return this.m_size;
            }
            set {
                if ((value.Width < 0) || (value.Height < 0)) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.GridSizeCannotBeNegative));
                }
                if (!this.m_size.Equals(value)) {
                    this.m_size = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(false), NotifyParentProperty(true), Description("Determines whether nodes are aligned on the grid.")]
        public bool Snap {
            get {
                return this.m_snap;
            }
            set {
                this.m_snap = value;
            }
        }
 
        [NotifyParentProperty(true), DefaultValue(2), Description("Returns/sets the grid style.")]
        public GridStyle Style {
            get {
                return this.m_style;
            }
            set {
                if (this.m_style != value) {
                    this.m_style = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        internal event Grid.ChangeEventHandler Change;
 
        //private Grid.ChangeEventHandler Change;
 
        private const int DefXGrid = 0x10;
 
        private const int DefYGrid = 0x10;
 
        internal Color m_color;
 
        internal bool m_draw;
 
        internal Size m_size;
 
        internal bool m_snap;
 
        internal GridStyle m_style;
 
        public static readonly Grid None;
    }

    internal class GridConverter : ExpandableObjectConverter {

        public GridConverter() {
        }
 
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
 
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(InstanceDescriptor)) {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }
 
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value) {
            if (value is string) {
                try {
                    return new Grid((string) value);
                }
                catch {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ArgumentsNotValid));
                }
            }
            return base.ConvertFrom(context, info, value);
        }
 
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (value is Grid) {
                Grid grid1 = (Grid) value;
                if (destinationType == typeof(string)) {
                    return grid1.ToString();
                }
                if (destinationType == typeof(InstanceDescriptor)) {
                    object[] objArray2 = new object[5] { grid1.Draw, grid1.Snap, grid1.Style, grid1.Color, grid1.Size } ;
                    object[] objArray1 = objArray2;
                    Type[] typeArray2 = new Type[5] { typeof(bool), typeof(bool), typeof(GridStyle), typeof(Color), typeof(Size) } ;
                    Type[] typeArray1 = typeArray2;
                    ConstructorInfo info1 = typeof(Grid).GetConstructor(typeArray1);
                    return new InstanceDescriptor(info1, objArray1);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
 
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
            return new Grid((bool) propertyValues["Draw"], (bool) propertyValues["Snap"], (GridStyle) propertyValues["Style"], (Color) propertyValues["Color"], (Size) propertyValues["Size"]);
        }
 
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
            return true;
        }
    }

    public enum GridStyle {
        // Fields
        DottedLines = 2,
        Lines = 1,
        Pixels = 0
    }
 
    public enum HandleSize {
        // Fields
        Large = 2,
        Medium = 1,
        Small = 0
    }
 
    public enum ImagePosition {
        // Fields
        CenterBottom = 8,
        CenterMiddle = 7,
        CenterTop = 6,
        Custom = 10,
        LeftBottom = 2,
        LeftMiddle = 1,
        LeftTop = 0,
        RelativeToText = 9,
        RightBottom = 5,
        RightMiddle = 4,
        RightTop = 3
    }
 
    public class ImagesCollection : CollectionBase {

        public ImagesCollection() {
        }
 
        public int Add(FlowImage flowImage) {
            return base.List.Add(flowImage);
        }
 
        public int Add(Image image) {
            return base.List.Add(new FlowImage(image));
        }
 
        public int Add(string urlImage) {
            return base.List.Add(new FlowImage(urlImage));
        }
 
        public bool Contains(FlowImage flowImage) {
            return base.List.Contains(flowImage);
        }
 
        public bool Contains(Image image) {
            //????????????????????????????????????
            //foreach (FlowImage image1 in base) {
            foreach (FlowImage image1 in this) {
                if ((image1.m_image != null) && image1.m_image.Equals(image)) {
                    return true;
                }
            }
            return false;
        }
 
        public bool Contains(string urlImage) {
            //?????????????????????????????????
            //foreach (FlowImage image1 in base) {
            foreach (FlowImage image1 in this) {
                if ((image1.m_url != null) && image1.m_url.Equals(urlImage)) {
                    return true;
                }
            }
            return false;
        }
 
        public void CopyTo(FlowImage[] array, int index) {
            base.List.CopyTo(array, index);
        }
 
        public int IndexOf(FlowImage flowImage) {
            return base.List.IndexOf(flowImage);
        }
 
        public int IndexOf(Image image) {
            //?????????????????????????????????
            //foreach (FlowImage image1 in base) {
            foreach (FlowImage image1 in this) {
                if ((image1.m_image != null) && image1.m_image.Equals(image)) {
                    return base.List.IndexOf(image1);
                }
            }
            return -1;
        }
 
        public int IndexOf(string urlImage) {
            //?????????????????????????????????
            //foreach (FlowImage image1 in base) {
            foreach (FlowImage image1 in this) {
                if ((image1.m_url != null) && image1.m_url.Equals(urlImage)) {
                    return base.List.IndexOf(image1);
                }
            }
            return -1;
        }
 
        public void Insert(int index, FlowImage flowImage) {
            base.List.Insert(index, flowImage);
        }
 
        public void Insert(int index, Image image) {
            base.List.Insert(index, new FlowImage(image));
        }
 
        public void Insert(int index, string urlImage) {
            base.List.Insert(index, new FlowImage(urlImage));
        }
 
        public void Remove(FlowImage flowImage) {
            base.List.Remove(flowImage);
        }
 
        public void Remove(Image image) {
            //?????????????????????????????????
            //foreach (FlowImage image1 in base) {
            foreach (FlowImage image1 in this) {
                if ((image1.m_image != null) && image1.m_image.Equals(image)) {
                    base.List.Remove(image1);
                    return;
                }
            }
        }
 
        public void Remove(string urlImage) {
            //?????????????????????????????????
            //foreach (FlowImage image1 in base) {
            foreach (FlowImage image1 in this) {
                if ((image1.m_url != null) && image1.m_url.Equals(urlImage)) {
                    base.List.Remove(image1);
                    return;
                }
            }
        }
 
        public FlowImage this[int index] {
            get {
                return (FlowImage) base.List[index];
            }
            set {
                base.List[index] = value;
            }
        }
    }



    public enum InteractiveAction {
        // Fields
        Drag = 3,
        Link = 2,
        Node = 1,
        None = 0,
        Select = 6,
        Size = 4,
        Stretch = 5
    }
 
    public abstract class Item : MarshalByRefObject {

        public Item() {
            this.m_af = null;
            this.m_task = null;
            this.m_rc = new RectangleF();
            this.m_text = null;
            this.m_tooltip = null;
            this.m_tag = null;
            this.m_selected = false;
            this.m_visited = false;
            this.m_flag = false;
            this.m_drag = false;
        }
 
        public void BringIntoView() {
            Rectangle rectangle1 = this.m_af.GetClientRectangleInWorldCoordinates();
            PointF tf1 = Misc.CenterPoint(this.m_rc);
            tf1 = new PointF(tf1.X - (rectangle1.Width / 2), tf1.Y - (rectangle1.Height / 2));
            RectangleF ef1 = RectangleF.FromLTRB(0f, 0f, tf1.X, tf1.Y);
            this.m_af.ScrollPosition = Point.Empty;
            Graphics graphics1 = this.m_af.CreateGraphics();
            this.m_af.CoordinatesDeviceToWorld(graphics1);
            ef1 = Misc.RectWorldToDevice(graphics1, ef1);
            graphics1.Dispose();
            tf1 = new PointF(ef1.Right, ef1.Bottom);
            this.m_af.ScrollPosition = Point.Round(tf1);
        }
 
        internal virtual void Draw(Graphics grfx) {
        }
 
        internal int GetZOrder() {
            return this.m_af.m_items.IndexOf(this);
        }
 
        internal virtual void InvalidateHandles() {
        }
 
        internal virtual void InvalidateItem() {
        }
 
        public void Refresh() {
            this.InvalidateItem();
            this.InvalidateHandles();
        }
 
        internal void SetSelectable2(bool newValue) {
            if (!newValue) {
                this.InvalidateHandles();
            }
            this.m_selectable = newValue;
            if (!this.m_selectable && this.m_selected) {
                this.Selected = false;
            }
        }
 
        internal void SetZOrder(int newValue) {
            if (this.m_af.m_items.Count != 0) {
                if ((newValue < 0) || (newValue >= this.m_af.m_items.Count)) {
                    throw new ArgumentOutOfRangeException();
                }
                if (this.GetZOrder() != newValue) {
                    if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                        this.m_af.m_undo.SubmitTask(new ZOrderIndexTask(this));
                    }
                    this.SetZOrder2(newValue);
                    this.m_af.IncrementChangedFlag(1);
                }
            }
        }
 
        internal void SetZOrder2(int newValue) {
            this.m_af.m_items.Remove(this);
            this.m_af.m_items.Insert(newValue, this);
            this.InvalidateItem();
        }
 
        public AddFlow AddFlow {
            get {
                return this.m_af;
            }
        }
 
        public BackMode BackMode {
            get {
                return this.m_backMode;
            }
            set {
                if (this.m_backMode != value) {
                    if (!this.Existing) {
                        this.m_backMode = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new ItemBackModeTask(this));
                        }
                        this.m_backMode = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public DashStyle DashStyle {
            get {
                return this.m_dashStyle;
            }
            set {
                if ((this.m_dashStyle != value) && (value != DashStyle.Custom)) {
                    if (!this.Existing) {
                        this.m_dashStyle = value;
                    }
                    else {
                        if (this is Node) {
                            if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                                this.m_af.m_undo.SubmitTask(new NodeDashStyleTask((Node) this));
                            }
                            ((Node) this).SetDashStyle2(value);
                        }
                        else {
                            if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                                this.m_af.m_undo.SubmitTask(new LinkDashStyleTask((Link) this));
                            }
                            ((Link) this).SetDashStyle2(value);
                        }
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public Color DrawColor {
            get {
                return this.m_drawColor;
            }
            set {
                if (this.m_drawColor != value) {
                    if (!this.Existing) {
                        this.m_drawColor = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new ItemDrawColorTask(this));
                        }
                        this.m_drawColor = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public int DrawWidth {
            get {
                return this.m_drawWidth;
            }
            set {
                if (value < 0) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.DrawWidthCannotBeNegative));
                }
                if (this.m_drawWidth != value) {
                    if (!this.Existing) {
                        this.m_drawWidth = value;
                    }
                    else {
                        if (this is Node) {
                            if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                                this.m_af.m_undo.SubmitTask(new NodeDrawWidthTask((Node) this));
                            }
                            ((Node) this).SetDrawWidth2(value);
                        }
                        else {
                            if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                                this.m_af.m_undo.SubmitTask(new LinkDrawWidthTask((Link) this));
                            }
                            ((Link) this).SetDrawWidth2(value);
                        }
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        internal bool Existing {
            get {
                return (this.m_af != null);
            }
        }
 
        public Font Font {
            get {
                return this.m_font;
            }
            set {
                if (!this.m_font.Equals(value)) {
                    if (!this.Existing) {
                        this.m_font = value;
                    }
                    else {
                        if (this is Node) {
                            if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                                this.m_af.m_undo.SubmitTask(new NodeFontTask((Node) this));
                            }
                            ((Node) this).SetFont2(value);
                        }
                        else {
                            if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                                this.m_af.m_undo.SubmitTask(new LinkFontTask((Link) this));
                            }
                            ((Link) this).SetFont2(value);
                        }
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public bool Hidden {
            get {
                return this.m_hidden;
            }
            set {
                if (this.m_hidden != value) {
                    if (!this.Existing) {
                        this.m_hidden = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new ItemFlagTask(this, Action.Hidden));
                        }
                        this.m_hidden = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public bool IsInView {
            get {
                Rectangle rectangle1 = this.m_af.GetClientRectangleInWorldCoordinates();
                RectangleF ef1 = RectangleF.Intersect((RectangleF) rectangle1, this.m_rc);
                return !ef1.IsEmpty;
            }
        }
 
        public bool Logical {
            get {
                return this.m_logical;
            }
            set {
                if (this.m_logical != value) {
                    if (!this.Existing) {
                        this.m_logical = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new ItemFlagTask(this, Action.Logical));
                        }
                        this.m_logical = value;
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public bool OwnerDraw {
            get {
                return this.m_ownerDraw;
            }
            set {
                if (this.m_ownerDraw != value) {
                    if (!this.Existing) {
                        this.m_ownerDraw = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new ItemFlagTask(this, Action.OwnerDraw));
                        }
                        this.m_ownerDraw = value;
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        internal RectangleF RC {
            get {
                return this.m_rc;
            }
            set {
                this.m_rc = value;
            }
        }
 
        public bool Selectable {
            get {
                return this.m_selectable;
            }
            set {
                if (this.m_selectable != value) {
                    if (!this.Existing) {
                        this.m_selectable = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new ItemFlagTask(this, Action.Selectable));
                        }
                        this.SetSelectable2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public bool Selected {
            get {
                return this.m_selected;
            }
            set {
                if (this.m_selectable && (this.m_selected != value)) {
                    this.m_selected = value;
                    if (this.m_selected) {
                        this.m_af.m_selItems.Add(this);
                    }
                    else {
                        this.m_af.m_selItems.Remove(this);
                    }
                    this.m_af.SetSelChangedFlag(true);
                    this.InvalidateHandles();
                    if ((!this.m_selected && (this.m_af.m_selItems.Count > 0)) && (this.m_af.m_selItems[0] != this)) {
                        Item item1 = this.m_af.m_selItems[0];
                        item1.InvalidateHandles();
                    }
                }
            }
        }
 
        public object Tag {
            get {
                return this.m_tag;
            }
            set {
                if (this.m_tag != value) {
                    if (!this.Existing) {
                        this.m_tag = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new TagTask(this));
                        }
                        this.m_tag = value;
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public virtual string Text {
            get {
                return this.m_text;
            }
            set {
            }
        }
 
        public Color TextColor {
            get {
                return this.m_textColor;
            }
            set {
                if (this.m_textColor != value) {
                    if (!this.Existing) {
                        this.m_textColor = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new ItemTextColorTask(this));
                        }
                        this.m_textColor = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public virtual string Tooltip {
            get {
                return this.m_tooltip;
            }
            set {
            }
        }
 
        public int ZOrder {
            get {
                return this.GetZOrder();
            }
            set {
                this.SetZOrder(value);
            }
        }
 
        internal AddFlow m_af;
 
        internal BackMode m_backMode;
 
        internal DashStyle m_dashStyle;
 
        internal bool m_drag;
 
        internal Color m_drawColor;
 
        internal int m_drawWidth;
 
        internal bool m_flag;
 
        internal Font m_font;
 
        internal bool m_hidden;
 
        internal bool m_logical;
 
        internal bool m_ownerDraw;
 
        internal RectangleF m_rc;
 
        internal bool m_selectable;
 
        internal bool m_selected;
 
        internal object m_tag;
 
        internal Task m_task;
 
        internal string m_text;
 
        internal Color m_textColor;
 
        internal string m_tooltip;
 
        internal bool m_visited;
    }

    internal class ItemBackModeTask : Task {

        private ItemBackModeTask() {
        }
 
        internal ItemBackModeTask(Item item) {
            this.m_item = item;
            this.m_oldBackMode = this.m_item.m_backMode;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.BackMode;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            BackMode mode1 = this.m_item.m_backMode;
            this.m_item.m_backMode = this.m_oldBackMode;
            this.m_item.InvalidateItem();
            this.m_oldBackMode = mode1;
        }
 
        private Item m_item;
 
        private BackMode m_oldBackMode;
    }

    internal class ItemDrawColorTask : Task {

        private ItemDrawColorTask() {
        }
 
        internal ItemDrawColorTask(Item item) {
            this.m_item = item;
            this.m_oldValue = this.m_item.m_drawColor;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.DrawColor;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Color color1 = this.m_item.m_drawColor;
            this.m_item.m_drawColor = this.m_oldValue;
            this.m_item.InvalidateItem();
            this.m_oldValue = color1;
        }
 
        private Item m_item;
 
        private Color m_oldValue;
    }

    internal class ItemFlagTask : Task {

        private ItemFlagTask() {
        }
 
        internal ItemFlagTask(Item item, Action code) {
            this.m_item = item;
            this.m_code = code;
            Action action1 = this.m_code;
            if (action1 <= Action.Logical) {
                if (action1 == Action.Hidden) {
                    this.m_oldValue = this.m_item.m_hidden;
                    return;
                }
                if (action1 != Action.Logical) {
                    return;
                }
            }
            else {
                if (action1 == Action.OwnerDraw) {
                    this.m_oldValue = this.m_item.m_ownerDraw;
                    return;
                }
                if (action1 == Action.Selectable) {
                    this.m_oldValue = this.m_item.m_selectable;
                }
                return;
            }
            this.m_oldValue = this.m_item.m_logical;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return this.m_code;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            bool flag1;
            Action action1 = this.m_code;
            if (action1 <= Action.Logical) {
                if (action1 == Action.Hidden) {
                    flag1 = this.m_item.m_hidden;
                    this.m_item.m_hidden = this.m_oldValue;
                    this.m_item.InvalidateItem();
                    goto Label_00B3;
                }
                if (action1 == Action.Logical) {
                    flag1 = this.m_item.m_logical;
                    this.m_item.m_logical = this.m_oldValue;
                    goto Label_00B3;
                }
            }
            else {
                if (action1 == Action.OwnerDraw) {
                    flag1 = this.m_item.m_ownerDraw;
                    this.m_item.m_ownerDraw = this.m_oldValue;
                    goto Label_00B3;
                }
                if (action1 == Action.Selectable) {
                    flag1 = this.m_item.m_selectable;
                    this.m_item.SetSelectable2(this.m_oldValue);
                    goto Label_00B3;
                }
            }
            flag1 = false;
            Label_00B3:
                this.m_oldValue = flag1;
        }
 
        private Action m_code;
 
        private Item m_item;
 
        private bool m_oldValue;
    }

    public class ItemsCollection : CollectionBase {
 
        internal delegate void ClearCollEventHandler(object sender, EventArgs e);
 
        internal delegate void RemoveAtIndexEventHandler(object sender, RemoveAtIndexEventArgs e);
 
        internal ItemsCollection() {
        }
 
        internal int Add(Item item) {
            return base.List.Add(item);
        }
 
        public bool Contains(Item item) {
            return base.List.Contains(item);
        }
 
        public void CopyTo(Item[] array, int index) {
            base.List.CopyTo(array, index);
        }
 
        public int IndexOf(Item item) {
            return base.List.IndexOf(item);
        }
 
        internal void Insert(int index, Item item) {
            base.List.Insert(index, item);
        }
 
        protected override void OnClear() {
            this.OnClearColl(EventArgs.Empty);
        }
 
        internal virtual void OnClearColl(EventArgs e) {
            if (this.ClearColl != null) {
                this.ClearColl(this, e);
            }
        }
 
        internal virtual void OnRemoveAtIndex(RemoveAtIndexEventArgs e) {
            if (this.RemoveAtIndex != null) {
                this.RemoveAtIndex(this, e);
            }
        }
 
        internal void Remove(Item item) {
            base.List.Remove(item);
        }
 
        public void RemoveAt(int index) {
            this.OnRemoveAtIndex(new RemoveAtIndexEventArgs(index));
        }
 
        public Item this[int index] {
            get {
                if ((index < 0) || (index > (base.Count - 1))) {
                    throw new IndexOutOfRangeException();
                }
                return (Item) base.List[index];
            }
        }
 
        internal event ItemsCollection.ClearCollEventHandler ClearColl;
 
        internal event ItemsCollection.RemoveAtIndexEventHandler RemoveAtIndex;
 
        //private ItemsCollection.ClearCollEventHandler ClearColl;
 
        //private ItemsCollection.RemoveAtIndexEventHandler RemoveAtIndex;
    }

    public enum ItemSet {
        // Fields
        Items = 0,
        Links = 2,
        Nodes = 1,
        SelectableItems = 3,
        SelectableLinks = 5,
        SelectableNodes = 4
    }
 
    internal class ItemTextColorTask : Task {

        private ItemTextColorTask() {
        }
 
        internal ItemTextColorTask(Item item) {
            this.m_item = item;
            this.m_oldValue = this.m_item.m_textColor;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.TextColor;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Color color1 = this.m_item.m_textColor;
            this.m_item.m_textColor = this.m_oldValue;
            this.m_item.InvalidateItem();
            this.m_oldValue = color1;
        }
 
        private Item m_item;
 
        private Color m_oldValue;
    }

    public enum Jump {
        // Fields
        Arc = 1,
        Break = 2,
        None = 0
    }
 
    [Serializable, TypeConverter(typeof(LineConverter))]
    public class Line : ICloneable {
 
        internal delegate void AfterChangeEventHandler(object sender, EventArgs e);
 
        internal delegate void BeforeChangeEventHandler(object sender, EventArgs e);
 
        public Line() {
            this.Reset();
        }
 
        public Line(LineStyle style) {
            this.m_style = style;
            this.m_orthogonalDynamic = false;
            this.m_roundedCorner = false;
        }
 
        public Line(string format) {
            char[] chArray1 = new char[1] { ';' } ;
            string[] textArray1 = format.Split(chArray1);
            this.m_style = (LineStyle) Enum.Parse(typeof(LineStyle), textArray1[0].Trim(), false);
            chArray1 = new char[1] { '=' } ;
            string[] textArray2 = textArray1[1].Split(chArray1);
            this.m_orthogonalDynamic = textArray2[1].Trim() == bool.TrueString;
            chArray1 = new char[1] { '=' } ;
            textArray2 = textArray1[2].Split(chArray1);
            this.m_roundedCorner = textArray2[1].Trim() == bool.TrueString;
        }
 
        public Line(LineStyle style, bool orthogonalDynamic, bool roundedCorner) {
            this.m_style = style;
            this.m_orthogonalDynamic = orthogonalDynamic;
            this.m_roundedCorner = roundedCorner;
        }
 
        public object Clone() {
            return new Line(this.m_style, this.m_orthogonalDynamic, this.m_roundedCorner);
        }
 
        internal bool Equals(object obj) {
            if (obj is Line) {
                Line line1 = (Line) obj;
                if ((this.m_style == line1.m_style) && (this.m_orthogonalDynamic == line1.m_orthogonalDynamic)) {
                    return (this.m_roundedCorner == line1.m_roundedCorner);
                }
            }
            return false;
        }
 
        internal bool IsDefault() {
            if ((this.m_style == LineStyle.Polyline) && !this.m_orthogonalDynamic) {
                return !this.m_roundedCorner;
            }
            return false;
        }
 
        internal virtual void OnAfterChange(EventArgs e) {
            if (this.AfterChange != null) {
                this.AfterChange(this, e);
            }
        }
 
        internal virtual void OnBeforeChange(EventArgs e) {
            if (this.BeforeChange != null) {
                this.BeforeChange(this, e);
            }
        }
 
        internal void Reset() {
            this.m_style = LineStyle.Polyline;
            this.m_orthogonalDynamic = false;
            this.m_roundedCorner = false;
        }
 
        public override string ToString() {
            StringBuilder builder1 = new StringBuilder("");
            builder1.Append(this.m_style.ToString() + "; ");
            builder1.Append("OrthogonalDynamic = " + this.m_orthogonalDynamic + "; ");
            builder1.Append("RoundedCorner = " + this.m_roundedCorner);
            return builder1.ToString();
        }
 
        internal bool CurveStyle {
            get {
                if (this.m_style != LineStyle.Bezier) {
                    return (this.m_style == LineStyle.Spline);
                }
                return true;
            }
        }
 
        internal bool HVStyle {
            get {
                if ((this.m_style != LineStyle.Spline) && (this.m_style != LineStyle.Bezier)) {
                    return (this.m_style != LineStyle.Polyline);
                }
                return false;
            }
        }
 
        internal bool NewPointsAllowed {
            get {
                if (this.m_style != LineStyle.Polyline) {
                    return (this.m_style == LineStyle.Spline);
                }
                return true;
            }
        }
 
        [NotifyParentProperty(true), Description("Determines whether the link segments are orthogonal."), DefaultValue(false)]
        public bool OrthogonalDynamic {
            get {
                return this.m_orthogonalDynamic;
            }
            set {
                if (this.m_orthogonalDynamic != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_orthogonalDynamic = value;
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }
 
        internal bool Polyline {
            get {
                if (this.m_style != LineStyle.Bezier) {
                    return (this.m_style != LineStyle.Spline);
                }
                return false;
            }
        }
 
        [DefaultValue(false), Description("Determines whether there is a rounded corner at the intersection point ."), NotifyParentProperty(true)]
        public bool RoundedCorner {
            get {
                return this.m_roundedCorner;
            }
            set {
                if (this.m_roundedCorner != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_roundedCorner = value;
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }
 
        [DefaultValue(0), Description("Returns/sets the style of the line."), NotifyParentProperty(true)]
        public LineStyle Style {
            get {
                return this.m_style;
            }
            set {
                if (this.m_style != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_style = value;
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }
 
        internal event Line.AfterChangeEventHandler AfterChange;
 
        internal event Line.BeforeChangeEventHandler BeforeChange;
 
        //private Line.AfterChangeEventHandler AfterChange;
 
        //private Line.BeforeChangeEventHandler BeforeChange;
 
        internal bool m_orthogonalDynamic;
 
        internal bool m_roundedCorner;
 
        internal LineStyle m_style;
    }

    internal class LineConverter : ExpandableObjectConverter {

        public LineConverter() {
        }
 
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
 
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(InstanceDescriptor)) {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }
 
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value) {
            if (value is string) {
                try {
                    return new Line((string) value);
                }
                catch {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ArgumentsNotValid));
                }
            }
            return base.ConvertFrom(context, info, value);
        }
 
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (value is Line) {
                Line line1 = (Line) value;
                if (destinationType == typeof(string)) {
                    return line1.ToString();
                }
                if (destinationType == typeof(InstanceDescriptor)) {
                    object[] objArray2 = new object[3] { line1.Style, line1.OrthogonalDynamic, line1.RoundedCorner } ;
                    object[] objArray1 = objArray2;
                    Type[] typeArray2 = new Type[3] { typeof(LineStyle), typeof(bool), typeof(bool) } ;
                    Type[] typeArray1 = typeArray2;
                    ConstructorInfo info1 = typeof(Line).GetConstructor(typeArray1);
                    return new InstanceDescriptor(info1, objArray1);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
 
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
            return new Line((LineStyle) propertyValues["Style"], (bool) propertyValues["OrthogonalDynamic"], (bool) propertyValues["RoundedCorner"]);
        }
 
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
            return true;
        }
    }

    public enum LineStyle {
        // Fields
        Bezier = 1,
        HV = 4,
        HVH = 6,
        HVHV = 8,
        HVHVH = 10,
        HVHVHV = 12,
        HVHVHVH = 14,
        Polyline = 0,
        Spline = 2,
        VH = 3,
        VHV = 5,
        VHVH = 7,
        VHVHV = 9,
        VHVHVH = 11,
        VHVHVHV = 13
    }
 
    public class Link : Item, ICloneable {

        private enum StretchDir {
            // Fields
            East = 1,
            North = 0,
            South = 2,
            West = 3
        }
 
        private enum StretchPos {
            // Fields
            LeftBottom = 6,
            LeftMid = 7,
            LeftTop = 8,
            MidBottom = 5,
            MidMid = 0,
            MidTop = 1,
            RightBottom = 4,
            RightMid = 3,
            RightTop = 2
        }
 
        static Link() {
            Link.m_distance = 0f;
        }
 
        public Link() {
            this.m_customStartCap = null;
            this.m_customEndCap = null;
            this.m_rcSeg = new RectangleF();
            this.Init();
        }
 
        public Link(DefLink deflink) {
            this.m_customStartCap = null;
            this.m_customEndCap = null;
            this.m_rcSeg = new RectangleF();
            this.InitWithDefLink(deflink);
        }
 
        public Link(string text) {
            this.m_customStartCap = null;
            this.m_customEndCap = null;
            this.m_rcSeg = new RectangleF();
            this.Init();
            this.m_text = text;
        }
 
        public Link(string text, DefLink deflink) {
            this.m_customStartCap = null;
            this.m_customEndCap = null;
            this.m_rcSeg = new RectangleF();
            this.InitWithDefLink(deflink);
            this.m_text = text;
        }
 
        private void AddLineInter(GraphicsPath path) {
            Size size1 = new Size(this.m_af.m_jumpSize, this.m_af.m_jumpSize);
            for (int num1 = 0; num1 < (this.m_aptf.Length - 1); num1++) {
                ArrayList list1 = new ArrayList();
                int num2 = 0;
                PointF tf1 = this.m_aptf[num1];
                PointF tf2 = this.m_aptf[num1 + 1];
                bool flag1 = false;
                for (int num3 = 0; num3 < this.m_af.Items.Count; num3++) {
                    Item item1 = this.m_af.Items[num3];
                    if (flag1) {
                        break;
                    }
                    if (item1 is Link) {
                        Link link1 = (Link) item1;
                        if (!link1.m_hidden) {
                            if (link1 == this) {
                                flag1 = true;
                            }
                            RectangleF ef1 = new RectangleF(link1.RC.Location, link1.RC.Size);
                            if ((this.m_af.m_drawAll || ef1.IntersectsWith((RectangleF) this.m_af.m_rcinv)) && ef1.IntersectsWith(this.m_rc)) {
                                for (int num4 = 0; num4 < (link1.m_aptf.Length - 1); num4++) {
                                    if (flag1) {
                                        if (num4 == num1) {
                                            break;
                                        }
                                        if (Math.Abs((int) (num4 - num1)) == 1) {
                                            goto Label_0324;
                                        }
                                    }
                                    PointF tf3 = link1.m_aptf[num4];
                                    PointF tf4 = link1.m_aptf[num4 + 1];
                                    if (Misc.InterSeg(tf1, tf2, tf3, tf4)) {
                                        float single1;
                                        float single4;
                                        bool flag2 = true;
                                        float single3 = single4 = 0f;
                                        if (tf1.X == tf2.X) {
                                            if (tf3.X != tf4.X) {
                                                single1 = (tf3.Y - tf4.Y) / (tf3.X - tf4.X);
                                                single3 = tf1.X;
                                                single4 = (single1 * (tf1.X - tf4.X)) + tf4.Y;
                                            }
                                            else {
                                                flag2 = false;
                                            }
                                        }
                                        else {
                                            float single2 = (tf2.Y - tf1.Y) / (tf2.X - tf1.X);
                                            if (tf4.X == tf3.X) {
                                                single3 = tf3.X;
                                                single4 = (single2 * (tf3.X - tf2.X)) + tf2.Y;
                                            }
                                            else {
                                                single1 = (tf3.Y - tf4.Y) / (tf3.X - tf4.X);
                                                if (single1 != single2) {
                                                    single3 = -(((tf2.Y - (single2 * tf2.X)) - tf4.Y) + (single1 * tf4.X)) / (single2 - single1);
                                                    single4 = (single1 * (single3 - tf4.X)) + tf4.Y;
                                                }
                                                else {
                                                    flag2 = false;
                                                }
                                            }
                                        }
                                        if (((flag2 && ((single3 != tf1.X) || (single4 != tf1.Y))) && (((single3 != tf2.X) || (single4 != tf2.Y)) && ((single3 != tf3.X) || (single4 != tf3.Y)))) && ((single3 != tf4.X) || (single4 != tf4.Y))) {
                                            list1.Add(new PointF(single3, single4));
                                            num2++;
                                        }
                                    }
                                Label_0324:;
                                }
                            }
                        }
                    }
                }
                PointF tf5 = new PointF(tf1.X, tf1.Y);
                if (num2 > 0) {
                    if (num2 > 1) {
                        for (int num5 = 0; num5 < num2; num5++) {
                            PointF tf6 = (PointF) list1[num5];
                            tf6.X -= tf1.X;
                            tf6.Y -= tf1.Y;
                            list1[num5] = tf6;
                        }
                        list1.Sort(new ComparePoints());
                        for (int num6 = 0; num6 < num2; num6++) {
                            PointF tf7 = (PointF) list1[num6];
                            tf7.X += tf1.X;
                            tf7.Y += tf1.Y;
                            list1[num6] = tf7;
                        }
                    }
                    float single5 = tf2.X - tf1.X;
                    float single6 = tf1.Y - tf2.Y;
                    float single7 = (float) Math.Sqrt((double) ((single5 * single5) + (single6 * single6)));
                    if (single7 == 0f) {
                        goto Label_0753;
                    }
                    float single8 = single5 / single7;
                    float single9 = single6 / single7;
                    float single10 = size1.Height * single9;
                    float single11 = size1.Width * single8;
                    float single12 = (float) (57.295827908797776 * Math.Acos((double) single8));
                    for (int num7 = 0; num7 < num2; num7++) {
                        PointF tf8 = (PointF) list1[num7];
                        PointF tf9 = new PointF();
                        PointF tf10 = new PointF();
                        RectangleF ef2 = new RectangleF(tf8.X - size1.Width, tf8.Y - size1.Height, (float) (2 * size1.Width), (float) (2 * size1.Height));
                        if (Math.Abs(single5) >= Math.Abs(single6)) {
                            if (single5 >= 0f) {
                                tf9.X = tf8.X - size1.Width;
                                tf9.Y = tf8.Y + single10;
                                tf10.X = tf8.X + size1.Width;
                                tf10.Y = tf8.Y - single10;
                            }
                            else {
                                tf9.X = tf8.X + size1.Width;
                                tf9.Y = tf8.Y + single10;
                                tf10.X = tf8.X - size1.Width;
                                tf10.Y = tf8.Y - single10;
                            }
                        }
                        else if (single6 >= 0f) {
                            tf9.X = tf8.X - single11;
                            tf9.Y = tf8.Y + size1.Height;
                            tf10.X = tf8.X + single11;
                            tf10.Y = tf8.Y - size1.Height;
                        }
                        else {
                            tf9.X = tf8.X - single11;
                            tf9.Y = tf8.Y - size1.Height;
                            tf10.X = tf8.X + single11;
                            tf10.Y = tf8.Y + size1.Height;
                        }
                        path.AddLine(tf5, tf9);
                        tf5 = new PointF(tf10.X, tf10.Y);
                        switch (this.m_jump) {
                        case Jump.Arc: {
                            float single13 = (tf9.Y < tf10.Y) ? (single12 + 180f) : (-single12 + 180f);
                            float single14 = (tf9.X < tf10.X) ? ((float) 180) : ((float) (-180));
                            path.AddArc(ef2, single13, single14);
                            goto Label_072A;
                        }
                        case Jump.Break: {
                            break;
                        }
                        default: {
                            goto Label_072A;
                        }
                        }
                        path.CloseFigure();
                    Label_072A:;
                    }
                }
                path.AddLine(tf5, this.m_aptf[num1 + 1]);
            Label_0753:;
            }
        }
 
        private void AddLineRounded(GraphicsPath path) {
            PointF tf1 = this.m_aptf[0];
            RectangleF ef1 = new RectangleF(0f, 0f, (float) this.m_af.m_roundSize, (float) this.m_af.m_roundSize);
            Graphics graphics1 = this.m_af.CreateGraphics();
            this.m_af.CoordinatesDeviceToWorld(graphics1);
            SizeF ef2 = Misc.SizeDeviceToWorld(graphics1, new SizeF(1f, 1f));
            graphics1.Dispose();
            for (int num1 = 0; num1 < (this.m_aptf.Length - 2); num1++) {
                if (((Math.Abs((float) (this.m_aptf[num1].Y - this.m_aptf[num1 + 1].Y)) <= ef2.Width) && (Math.Abs((float) (this.m_aptf[num1 + 1].X - this.m_aptf[num1 + 2].X)) <= ef2.Height)) || ((Math.Abs((float) (this.m_aptf[num1].X - this.m_aptf[num1 + 1].X)) <= ef2.Width) && (Math.Abs((float) (this.m_aptf[num1 + 1].Y - this.m_aptf[num1 + 2].Y)) <= ef2.Height))) {
                    PointF tf2;
                    if (Math.Abs((float) (this.m_aptf[num1].Y - this.m_aptf[num1 + 1].Y)) <= ef2.Height) {
                        if (this.m_aptf[num1].X < this.m_aptf[num1 + 1].X) {
                            if (this.m_aptf[num1 + 1].Y < this.m_aptf[num1 + 2].Y) {
                                tf2 = new PointF(this.m_aptf[num1 + 1].X - this.m_af.m_roundSize, this.m_aptf[num1 + 1].Y);
                                path.AddLine(tf1, tf2);
                                ef1.Location = new PointF(tf2.X, tf2.Y);
                                path.AddArc(ef1, (float) -90f, (float) 90f);
                                tf1 = new PointF(this.m_aptf[num1 + 1].X, this.m_aptf[num1 + 1].Y + this.m_af.m_roundSize);
                            }
                            else {
                                tf2 = new PointF(this.m_aptf[num1 + 1].X - this.m_af.m_roundSize, this.m_aptf[num1 + 1].Y);
                                path.AddLine(tf1, tf2);
                                ef1.Location = new PointF(tf2.X, tf2.Y - this.m_af.m_roundSize);
                                path.AddArc(ef1, (float) 90f, (float) -90f);
                                tf1 = new PointF(this.m_aptf[num1 + 1].X, this.m_aptf[num1 + 1].Y - this.m_af.m_roundSize);
                            }
                        }
                        else if (this.m_aptf[num1 + 1].Y < this.m_aptf[num1 + 2].Y) {
                            tf2 = new PointF(this.m_aptf[num1 + 1].X + this.m_af.m_roundSize, this.m_aptf[num1 + 1].Y);
                            path.AddLine(tf1, tf2);
                            ef1.Location = new PointF(tf2.X - this.m_af.m_roundSize, tf2.Y);
                            path.AddArc(ef1, (float) -90f, (float) -90f);
                            tf1 = new PointF(this.m_aptf[num1 + 1].X, this.m_aptf[num1 + 1].Y + this.m_af.m_roundSize);
                        }
                        else {
                            tf2 = new PointF(this.m_aptf[num1 + 1].X + this.m_af.m_roundSize, this.m_aptf[num1 + 1].Y);
                            path.AddLine(tf1, tf2);
                            ef1.Location = new PointF(tf2.X - this.m_af.m_roundSize, tf2.Y - this.m_af.m_roundSize);
                            path.AddArc(ef1, (float) 90f, (float) 90f);
                            tf1 = new PointF(this.m_aptf[num1 + 1].X, this.m_aptf[num1 + 1].Y - this.m_af.m_roundSize);
                        }
                    }
                    else if (this.m_aptf[num1].Y < this.m_aptf[num1 + 1].Y) {
                        if (this.m_aptf[num1 + 1].X < this.m_aptf[num1 + 2].X) {
                            tf2 = new PointF(this.m_aptf[num1 + 1].X, this.m_aptf[num1 + 1].Y - this.m_af.m_roundSize);
                            path.AddLine(tf1, tf2);
                            ef1.Location = new PointF(tf2.X, tf2.Y);
                            path.AddArc(ef1, (float) 180f, (float) -90f);
                            tf1 = new PointF(this.m_aptf[num1 + 1].X + this.m_af.m_roundSize, this.m_aptf[num1 + 1].Y);
                        }
                        else {
                            tf2 = new PointF(this.m_aptf[num1 + 1].X, this.m_aptf[num1 + 1].Y - this.m_af.m_roundSize);
                            path.AddLine(tf1, tf2);
                            ef1.Location = new PointF(tf2.X - this.m_af.m_roundSize, tf2.Y);
                            path.AddArc(ef1, (float) 0f, (float) 90f);
                            tf1 = new PointF(this.m_aptf[num1 + 1].X - this.m_af.m_roundSize, this.m_aptf[num1 + 1].Y);
                        }
                    }
                    else if (this.m_aptf[num1 + 1].X < this.m_aptf[num1 + 2].X) {
                        tf2 = new PointF(this.m_aptf[num1 + 1].X, this.m_aptf[num1 + 1].Y + this.m_af.m_roundSize);
                        path.AddLine(tf1, tf2);
                        ef1.Location = new PointF(tf2.X, tf2.Y - this.m_af.m_roundSize);
                        path.AddArc(ef1, (float) 180f, (float) 90f);
                        tf1 = new PointF(this.m_aptf[num1 + 1].X + this.m_af.m_roundSize, this.m_aptf[num1 + 1].Y);
                    }
                    else {
                        tf2 = new PointF(this.m_aptf[num1 + 1].X, this.m_aptf[num1 + 1].Y + this.m_af.m_roundSize);
                        path.AddLine(tf1, tf2);
                        ef1.Location = new PointF(tf2.X - this.m_af.m_roundSize, tf2.Y - this.m_af.m_roundSize);
                        path.AddArc(ef1, (float) 0f, (float) -90f);
                        tf1 = new PointF(this.m_aptf[num1 + 1].X - this.m_af.m_roundSize, this.m_aptf[num1 + 1].Y);
                    }
                }
                else {
                    path.AddLine(this.m_aptf[num1], this.m_aptf[num1 + 1]);
                    tf1 = this.m_aptf[num1 + 1];
                }
            }
            path.AddLine(tf1, this.m_aptf[this.m_aptf.Length - 1]);
        }
 
        internal void Adjust(Node node) {
            if (this.m_line.HVStyle) {
                this.FixLinkPoints();
            }
            if (this.m_org == node) {
                this.CalcLinkTips(!this.m_adjustOrg, !this.m_adjustDst);
            }
            else if (this.m_dst == node) {
                this.CalcLinkTips(!this.m_adjustOrg, !this.m_adjustDst);
            }
        }
 
        private void arrow_AfterChange(object sender, EventArgs e) {
            if (this.m_af != null) {
                this.InvalidateItem();
                this.m_af.IncrementChangedFlag(1);
            }
        }
 
        private void arrowDst_BeforeChange(object sender, EventArgs e) {
            if (((this.m_af != null) && this.m_af.m_undo.CanUndoRedo) && !this.m_af.m_undo.SkipUndo) {
                this.m_af.m_undo.SubmitTask(new LinkArrowDstTask(this));
            }
        }
 
        private void arrowMid_BeforeChange(object sender, EventArgs e) {
            if (((this.m_af != null) && this.m_af.m_undo.CanUndoRedo) && !this.m_af.m_undo.SkipUndo) {
                this.m_af.m_undo.SubmitTask(new LinkArrowMidTask(this));
            }
        }
 
        private void arrowOrg_BeforeChange(object sender, EventArgs e) {
            if (((this.m_af != null) && this.m_af.m_undo.CanUndoRedo) && !this.m_af.m_undo.SkipUndo) {
                this.m_af.m_undo.SubmitTask(new LinkArrowOrgTask(this));
            }
        }
 
        internal void CalcLinkTips(bool first, bool last) {
            GraphicsPath path1;
            PointF tf1 = first ? Misc.CenterPoint(this.m_org.m_rc) : this.m_aptf[0];
            PointF tf2 = last ? Misc.CenterPoint(this.m_dst.m_rc) : this.m_aptf[this.m_aptf.Length - 1];
            if (!first) {
                goto Label_0148;
            }
            if (this.m_org.RC.IsEmpty) {
                this.m_aptf[0] = tf1;
                goto Label_0148;
            }
            PointF tf3 = (this.m_aptf.Length == 2) ? tf2 : this.m_aptf[1];
            ShapeStyle style1 = this.m_org.m_shape.m_style;
            if (style1 <= ShapeStyle.Ellipse) {
                if ((style1 == ShapeStyle.Connector) || (style1 == ShapeStyle.Ellipse)) {
                    goto Label_00DC;
                }
                goto Label_0102;
            }
            if ((style1 != ShapeStyle.Or) && (style1 != ShapeStyle.SummingJunction)) {
                goto Label_0102;
            }
            Label_00DC:
                this.m_aptf[0] = Misc.GetEllipseNearestPt(this.m_org.m_rc, tf3, tf1);
            goto Label_0148;
            Label_0102:
                path1 = this.m_org.m_shape.GetPathOfShape(this.m_org.m_rc);
            path1.Flatten();
            PointF[] tfArray1 = path1.PathPoints;
            this.m_aptf[0] = Misc.GetPolyNearestPt(tfArray1, tfArray1.Length, tf3, tf1);
            Label_0148:
                if (!last) {
                    return;
                }
            if (this.m_dst.RC.IsEmpty) {
                this.m_aptf[this.m_aptf.Length - 1] = tf2;
                return;
            }
            PointF tf4 = (this.m_aptf.Length == 2) ? tf1 : this.m_aptf[this.m_aptf.Length - 2];
            style1 = this.m_dst.m_shape.m_style;
            if (style1 <= ShapeStyle.Ellipse) {
                if ((style1 == ShapeStyle.Connector) || (style1 == ShapeStyle.Ellipse)) {
                    goto Label_01DB;
                }
                goto Label_0209;
            }
            if ((style1 != ShapeStyle.Or) && (style1 != ShapeStyle.SummingJunction)) {
                goto Label_0209;
            }
            Label_01DB:
                this.m_aptf[this.m_aptf.Length - 1] = Misc.GetEllipseNearestPt(this.m_dst.m_rc, tf4, tf2);
            return;
            Label_0209:
                path1 = this.m_dst.m_shape.GetPathOfShape(this.m_dst.m_rc);
            path1.Flatten();
            tfArray1 = path1.PathPoints;
            this.m_aptf[this.m_aptf.Length - 1] = Misc.GetPolyNearestPt(tfArray1, tfArray1.Length, tf4, tf2);
        }
 
        internal void CalcRect() {
            RectangleF ef1;
            if (this.m_line.m_style == LineStyle.Bezier) {
                PointF[] tfArray1 = new PointF[11];
                int num1 = 0;
                for (float single1 = 0f; single1 < 1f; single1 += 0.1f) {
                    tfArray1[num1].X = (int) (((((((1f - single1) * (1f - single1)) * (1f - single1)) * this.m_aptf[0].X) + ((((3f * single1) * (1f - single1)) * (1f - single1)) * this.m_aptf[1].X)) + ((((3f * single1) * single1) * (1f - single1)) * this.m_aptf[2].X)) + (((single1 * single1) * single1) * this.m_aptf[3].X));
                    tfArray1[num1].Y = (int) (((((((1f - single1) * (1f - single1)) * (1f - single1)) * this.m_aptf[0].Y) + ((((3f * single1) * (1f - single1)) * (1f - single1)) * this.m_aptf[1].Y)) + ((((3f * single1) * single1) * (1f - single1)) * this.m_aptf[2].Y)) + (((single1 * single1) * single1) * this.m_aptf[3].Y));
                    num1++;
                }
                tfArray1[num1] = this.m_aptf[3];
                ef1 = Misc.Rect(tfArray1[0], tfArray1[0]);
                for (int num2 = 1; num2 < 11; num2++) {
                    ef1 = RectangleF.FromLTRB(Math.Min(ef1.Left, tfArray1[num2].X), Math.Min(ef1.Top, tfArray1[num2].Y), Math.Max(ef1.Right, tfArray1[num2].X), Math.Max(ef1.Bottom, tfArray1[num2].Y));
                }
            }
            else if ((this.m_line.m_style == LineStyle.Spline) && (this.m_aptf.Length > 2)) {
                PointF[] tfArray2 = new PointF[10 * (this.m_aptf.Length - 1)];
                this.CanonicalSpline(tfArray2);
                ef1 = Misc.Rect(tfArray2[0], tfArray2[0]);
                for (int num3 = 1; num3 < tfArray2.Length; num3++) {
                    ef1 = RectangleF.FromLTRB(Math.Min(ef1.Left, tfArray2[num3].X), Math.Min(ef1.Top, tfArray2[num3].Y), Math.Max(ef1.Right, tfArray2[num3].X), Math.Max(ef1.Bottom, tfArray2[num3].Y));
                }
            }
            else {
                ef1 = Misc.Rect(this.m_aptf[0], this.m_aptf[0]);
                for (int num4 = 1; num4 < this.m_aptf.Length; num4++) {
                    ef1 = RectangleF.FromLTRB(Math.Min(ef1.Left, this.m_aptf[num4].X), Math.Min(ef1.Top, this.m_aptf[num4].Y), Math.Max(ef1.Right, this.m_aptf[num4].X), Math.Max(ef1.Bottom, this.m_aptf[num4].Y));
                }
            }
            RectangleF ef2 = new RectangleF(ef1.Location, ef1.Size);
            float single2 = (this.m_drawWidth == 0) ? ((float) 1) : ((float) this.m_drawWidth);
            Graphics graphics1 = this.m_af.CreateGraphics();
            this.m_af.CoordinatesDeviceToWorld(graphics1);
            float single3 = Misc.DistanceDeviceToWorld(graphics1, single2);
            ef2.Inflate(single3, single3);
            float single4 = (single3 + this.m_af.m_linkWidthWorld) + 2f;
            ef1.Inflate(single4, single4);
            if ((this.m_text != null) && (this.m_text.Length > 0)) {
                PointF tf1 = this.GetTextPosition();
                SizeF ef3 = graphics1.MeasureString(this.m_text, this.m_font);
                RectangleF ef4 = RectangleF.FromLTRB(tf1.X - (ef3.Width / 2f), tf1.Y - (ef3.Height / 2f), tf1.X + (ef3.Width / 2f), tf1.Y + (ef3.Height / 2f));
                ef1 = RectangleF.FromLTRB(Math.Min(ef1.Left, ef4.Left), Math.Min(ef1.Top, ef4.Top), Math.Max(ef1.Right, ef4.Right), Math.Max(ef1.Bottom, ef4.Bottom));
                ef2 = RectangleF.FromLTRB(Math.Min(ef2.Left, ef4.Left), Math.Min(ef2.Top, ef4.Top), Math.Max(ef2.Right, ef4.Right), Math.Max(ef2.Bottom, ef4.Bottom));
            }
            graphics1.Dispose();
            this.m_rc = ef1;
            this.m_rcSeg = ef2;
        }
 
        private void CanonicalSegment(PointF[] apt, int start, PointF pt0, PointF pt1, PointF pt2, PointF pt3) {
            float single1 = 0.5f;
            float single2 = single1 * (pt2.X - pt0.X);
            float single3 = single1 * (pt2.Y - pt0.Y);
            float single4 = single1 * (pt3.X - pt1.X);
            float single5 = single1 * (pt3.Y - pt1.Y);
            float single6 = ((single2 + single4) + (2f * pt1.X)) - (2f * pt2.X);
            float single7 = ((single3 + single5) + (2f * pt1.Y)) - (2f * pt2.Y);
            float single8 = (((-2f * single2) - single4) - (3f * pt1.X)) + (3f * pt2.X);
            float single9 = (((-2f * single3) - single5) - (3f * pt1.Y)) + (3f * pt2.Y);
            float single10 = single2;
            float single11 = single3;
            float single12 = pt1.X;
            float single13 = pt1.Y;
            for (int num1 = 0; num1 < 10; num1++) {
                float single14 = ((float) num1) / 9f;
                apt[num1 + start].X = (int) ((((((single6 * single14) * single14) * single14) + ((single8 * single14) * single14)) + (single10 * single14)) + single12);
                apt[num1 + start].Y = (int) ((((((single7 * single14) * single14) * single14) + ((single9 * single14) * single14)) + (single11 * single14)) + single13);
            }
        }
 
        private void CanonicalSpline(PointF[] aptf) {
            if (this.m_aptf.Length == 3) {
                this.CanonicalSegment(aptf, 0, this.m_aptf[0], this.m_aptf[0], this.m_aptf[1], this.m_aptf[2]);
                this.CanonicalSegment(aptf, 10, this.m_aptf[0], this.m_aptf[1], this.m_aptf[2], this.m_aptf[2]);
            }
            else {
                this.CanonicalSegment(aptf, 0, this.m_aptf[0], this.m_aptf[0], this.m_aptf[1], this.m_aptf[2]);
                for (int num1 = 0; num1 < (this.m_aptf.Length - 3); num1++) {
                    this.CanonicalSegment(aptf, 10 * (num1 + 1), this.m_aptf[num1], this.m_aptf[num1 + 1], this.m_aptf[num1 + 2], this.m_aptf[num1 + 3]);
                }
                this.CanonicalSegment(aptf, 10 * (this.m_aptf.Length - 2), this.m_aptf[this.m_aptf.Length - 3], this.m_aptf[this.m_aptf.Length - 2], this.m_aptf[this.m_aptf.Length - 1], this.m_aptf[this.m_aptf.Length - 1]);
            }
        }
 
        internal void ChangeLineStyle() {
            RectangleF ef1;
            RectangleF ef2;
            if (this.m_adjustOrg) {
                ef1 = RectangleF.FromLTRB(this.m_aptf[0].X, this.m_aptf[0].Y, this.m_aptf[0].X, this.m_aptf[0].Y);
            }
            else {
                ef1 = this.m_org.m_rc;
            }
            if (this.m_adjustDst) {
                ef2 = RectangleF.FromLTRB(this.m_aptf[this.m_aptf.Length - 1].X, this.m_aptf[this.m_aptf.Length - 1].Y, this.m_aptf[this.m_aptf.Length - 1].X, this.m_aptf[this.m_aptf.Length - 1].Y);
            }
            else {
                ef2 = this.m_dst.m_rc;
            }
            PointF tf1 = Misc.CenterPoint(ef1);
            PointF tf2 = Misc.CenterPoint(ef2);
            switch (this.m_line.m_style) {
            case LineStyle.VH: {
                this.m_aptf = new PointF[3];
                this.m_aptf[0].X = tf1.X;
                this.m_aptf[0].Y = (tf2.Y > tf1.Y) ? ef1.Bottom : ef1.Top;
                this.m_aptf[2].X = (tf2.X > tf1.X) ? ef2.Left : ef2.Right;
                this.m_aptf[2].Y = tf2.Y;
                this.m_aptf[1].X = this.m_aptf[0].X;
                this.m_aptf[1].Y = this.m_aptf[2].Y;
                return;
            }
            case LineStyle.HV: {
                this.m_aptf = new PointF[3];
                this.m_aptf[0].X = (tf2.X > tf1.X) ? ef1.Right : ef1.Left;
                this.m_aptf[0].Y = tf1.Y;
                this.m_aptf[2].X = tf2.X;
                this.m_aptf[2].Y = (tf2.Y > tf1.Y) ? ef2.Top : ef2.Bottom;
                this.m_aptf[1].X = this.m_aptf[2].X;
                this.m_aptf[1].Y = this.m_aptf[0].Y;
                return;
            }
            case LineStyle.VHV: {
                this.m_aptf = new PointF[4];
                this.m_aptf[0].X = tf1.X;
                this.m_aptf[3].X = tf2.X;
                this.m_aptf[1].X = this.m_aptf[0].X;
                this.m_aptf[2].X = this.m_aptf[3].X;
                if (ef2.Bottom <= ef1.Bottom) {
                    this.m_aptf[0].Y = ef1.Top;
                    if (ef2.Bottom < ef1.Top) {
                        this.m_aptf[3].Y = ef2.Bottom;
                        this.m_aptf[1].Y = (this.m_aptf[0].Y + this.m_aptf[3].Y) / 2f;
                    }
                    else {
                        this.m_aptf[3].Y = ef2.Top;
                        this.m_aptf[1].Y = this.m_aptf[3].Y - (ef2.Bottom - ef2.Top);
                        if (this.m_aptf[1].Y < 0f) {
                            this.m_aptf[1].Y = 0f;
                        }
                    }
                    break;
                }
                this.m_aptf[0].Y = ef1.Bottom;
                if (ef2.Top <= ef1.Bottom) {
                    this.m_aptf[3].Y = ef2.Bottom;
                    this.m_aptf[1].Y = this.m_aptf[3].Y + (ef2.Bottom - ef2.Top);
                    break;
                }
                this.m_aptf[3].Y = ef2.Top;
                this.m_aptf[1].Y = (this.m_aptf[0].Y + this.m_aptf[3].Y) / 2f;
                break;
            }
            case LineStyle.HVH: {
                this.m_aptf = new PointF[4];
                if (ef2.Right <= ef1.Right) {
                    this.m_aptf[0].X = ef1.Left;
                    if (ef2.Right < ef1.Left) {
                        this.m_aptf[3].X = ef2.Right;
                        this.m_aptf[1].X = (this.m_aptf[0].X + this.m_aptf[3].X) / 2f;
                    }
                    else {
                        this.m_aptf[3].X = ef2.Left;
                        this.m_aptf[1].X = this.m_aptf[3].X - (ef2.Right - ef2.Left);
                        if (this.m_aptf[1].X < 0f) {
                            this.m_aptf[1].X = 0f;
                        }
                    }
                    goto Label_0758;
                }
                this.m_aptf[0].X = ef1.Right;
                if (ef2.Left <= ef1.Right) {
                    this.m_aptf[3].X = ef2.Right;
                    this.m_aptf[1].X = this.m_aptf[3].X + (ef2.Right - ef2.Left);
                    goto Label_0758;
                }
                this.m_aptf[3].X = ef2.Left;
                this.m_aptf[1].X = (this.m_aptf[0].X + this.m_aptf[3].X) / 2f;
                goto Label_0758;
            }
            case LineStyle.VHVH: {
                this.m_aptf = new PointF[5];
                this.m_aptf[0].X = tf1.X;
                this.m_aptf[0].Y = (tf2.Y > tf1.Y) ? ef1.Bottom : ef1.Top;
                this.m_aptf[4].X = (tf2.X > tf1.X) ? ef2.Left : ef2.Right;
                this.m_aptf[4].Y = tf2.Y;
                this.m_aptf[1].X = this.m_aptf[0].X;
                this.m_aptf[1].Y = (this.m_aptf[0].Y + this.m_aptf[4].Y) / 2f;
                this.m_aptf[2].X = (this.m_aptf[0].X + this.m_aptf[4].X) / 2f;
                this.m_aptf[2].Y = this.m_aptf[1].Y;
                this.m_aptf[3].X = this.m_aptf[2].X;
                this.m_aptf[3].Y = this.m_aptf[4].Y;
                if (!ef2.Contains(this.m_aptf[3])) {
                    goto Label_0A48;
                }
                if (this.m_aptf[4].Y != ef2.Right) {
                    this.m_aptf[3].X = this.m_aptf[4].X - (ef2.Right - ef2.Left);
                    goto Label_0A26;
                }
                this.m_aptf[3].X = this.m_aptf[4].X + (ef2.Right - ef2.Left);
                goto Label_0A26;
            }
            case LineStyle.HVHV: {
                this.m_aptf = new PointF[5];
                this.m_aptf[0].X = (tf2.X > tf1.X) ? ef1.Right : ef1.Left;
                this.m_aptf[0].Y = tf1.Y;
                this.m_aptf[4].X = tf2.X;
                this.m_aptf[4].Y = (tf2.Y > tf1.Y) ? ef2.Top : ef2.Bottom;
                this.m_aptf[1].X = (this.m_aptf[0].X + this.m_aptf[4].X) / 2f;
                this.m_aptf[1].Y = this.m_aptf[0].Y;
                this.m_aptf[2].X = this.m_aptf[1].X;
                this.m_aptf[2].Y = (this.m_aptf[0].Y + this.m_aptf[4].Y) / 2f;
                this.m_aptf[3].X = this.m_aptf[4].X;
                this.m_aptf[3].Y = this.m_aptf[2].Y;
                if (!ef2.Contains(this.m_aptf[3])) {
                    goto Label_0D61;
                }
                if (this.m_aptf[4].Y != ef2.Bottom) {
                    this.m_aptf[3].Y = this.m_aptf[4].Y - (ef2.Bottom - ef2.Top);
                    goto Label_0D3F;
                }
                this.m_aptf[3].Y = this.m_aptf[4].Y + (ef2.Bottom - ef2.Top);
                goto Label_0D3F;
            }
            case LineStyle.VHVHV: {
                this.m_aptf = new PointF[6];
                this.m_aptf[0].X = tf1.X;
                this.m_aptf[0].Y = (tf2.Y > tf1.Y) ? ef1.Top : ef1.Bottom;
                this.m_aptf[5].X = tf2.X;
                this.m_aptf[5].Y = (tf2.Y > tf1.Y) ? ef2.Bottom : ef2.Top;
                this.m_aptf[1].X = this.m_aptf[0].X;
                this.m_aptf[1].Y = this.m_aptf[0].Y - (ef1.Bottom - ef1.Top);
                this.m_aptf[2].X = (this.m_aptf[0].X + this.m_aptf[5].X) / 2f;
                this.m_aptf[2].Y = this.m_aptf[1].Y;
                this.m_aptf[4].X = this.m_aptf[5].X;
                this.m_aptf[4].Y = this.m_aptf[5].Y + (ef2.Bottom - ef2.Top);
                this.m_aptf[3].X = this.m_aptf[2].X;
                this.m_aptf[3].Y = this.m_aptf[4].Y;
                return;
            }
            case LineStyle.HVHVH: {
                this.m_aptf = new PointF[6];
                this.m_aptf[0].Y = tf1.Y;
                this.m_aptf[0].X = (tf2.X > tf1.X) ? ef1.Left : ef1.Right;
                this.m_aptf[5].Y = tf2.Y;
                this.m_aptf[5].X = (tf2.X > tf1.X) ? ef2.Right : ef2.Left;
                this.m_aptf[1].Y = this.m_aptf[0].Y;
                this.m_aptf[1].X = this.m_aptf[0].X - (ef1.Right - ef1.Left);
                this.m_aptf[2].Y = (this.m_aptf[0].Y + this.m_aptf[5].Y) / 2f;
                this.m_aptf[2].X = this.m_aptf[1].X;
                this.m_aptf[4].Y = this.m_aptf[5].Y;
                this.m_aptf[4].X = this.m_aptf[5].X + (ef2.Right - ef2.Left);
                this.m_aptf[3].Y = this.m_aptf[2].Y;
                this.m_aptf[3].X = this.m_aptf[4].X;
                return;
            }
            case LineStyle.VHVHVH: {
                this.m_aptf = new PointF[7];
                this.m_aptf[0].X = tf1.X;
                this.m_aptf[0].Y = (tf2.Y > tf1.Y) ? ef1.Top : ef1.Bottom;
                this.m_aptf[6].X = (tf2.X > tf1.X) ? ef2.Right : ef2.Left;
                this.m_aptf[6].Y = tf2.Y;
                this.m_aptf[1].X = this.m_aptf[0].X;
                this.m_aptf[1].Y = this.m_aptf[0].Y - (ef1.Bottom - ef1.Top);
                this.m_aptf[5].X = this.m_aptf[6].X + (ef2.Right - ef2.Left);
                this.m_aptf[5].Y = this.m_aptf[6].Y;
                this.m_aptf[2].X = (tf1.X + tf2.X) / 2f;
                this.m_aptf[2].Y = this.m_aptf[1].Y;
                this.m_aptf[4].X = this.m_aptf[5].X;
                this.m_aptf[4].Y = this.m_aptf[5].Y + (ef2.Bottom - ef2.Top);
                this.m_aptf[3].X = this.m_aptf[2].X;
                this.m_aptf[3].Y = this.m_aptf[4].Y;
                return;
            }
            case LineStyle.HVHVHV: {
                this.m_aptf = new PointF[7];
                this.m_aptf[0].X = (tf2.X > tf1.X) ? ef1.Right : ef1.Top;
                this.m_aptf[0].Y = tf1.Y;
                this.m_aptf[6].X = tf2.X;
                this.m_aptf[6].Y = (tf2.Y > tf1.Y) ? ef2.Bottom : ef2.Top;
                this.m_aptf[1].X = this.m_aptf[0].X + (ef1.Right - ef1.Left);
                this.m_aptf[1].Y = this.m_aptf[0].Y;
                this.m_aptf[5].X = this.m_aptf[6].X;
                this.m_aptf[5].Y = this.m_aptf[6].Y + (ef2.Bottom - ef2.Top);
                this.m_aptf[2].X = this.m_aptf[1].X;
                this.m_aptf[2].Y = (tf1.Y + tf2.Y) / 2f;
                this.m_aptf[4].X = this.m_aptf[5].X + (ef2.Right - ef2.Left);
                this.m_aptf[4].Y = this.m_aptf[5].Y;
                this.m_aptf[3].X = this.m_aptf[4].X;
                this.m_aptf[3].Y = this.m_aptf[2].Y;
                return;
            }
            case LineStyle.VHVHVHV: {
                this.m_aptf = new PointF[8];
                this.m_aptf[0].X = tf1.X;
                this.m_aptf[0].Y = (tf2.Y > tf1.Y) ? ef1.Top : ef1.Bottom;
                this.m_aptf[1].X = this.m_aptf[0].X;
                this.m_aptf[1].Y = this.m_aptf[0].Y - (ef1.Bottom - ef1.Top);
                this.m_aptf[2].X = this.m_aptf[1].X - (ef1.Right - ef1.Left);
                this.m_aptf[2].Y = this.m_aptf[1].Y;
                this.m_aptf[7].Y = (tf2.Y > tf1.Y) ? ef2.Bottom : ef2.Top;
                this.m_aptf[7].X = tf2.X;
                this.m_aptf[6].X = this.m_aptf[7].X;
                this.m_aptf[6].Y = this.m_aptf[7].Y + (ef2.Bottom - ef2.Top);
                this.m_aptf[5].X = this.m_aptf[6].X + (ef2.Right - ef2.Left);
                this.m_aptf[5].Y = this.m_aptf[6].Y;
                this.m_aptf[3].X = this.m_aptf[2].X;
                this.m_aptf[3].Y = (tf1.Y + tf2.Y) / 2f;
                this.m_aptf[4].X = this.m_aptf[5].X;
                this.m_aptf[4].Y = (tf1.Y + tf2.Y) / 2f;
                return;
            }
            case LineStyle.HVHVHVH: {
                this.m_aptf = new PointF[8];
                this.m_aptf[0].Y = tf1.Y;
                this.m_aptf[0].X = (tf2.X > tf1.X) ? ef1.Left : ef1.Right;
                this.m_aptf[1].Y = this.m_aptf[0].Y;
                this.m_aptf[1].X = this.m_aptf[0].X - (ef1.Right - ef1.Left);
                this.m_aptf[2].Y = this.m_aptf[1].Y - (ef1.Bottom - ef1.Top);
                this.m_aptf[2].X = this.m_aptf[1].X;
                this.m_aptf[7].X = (tf2.X > tf1.X) ? ef2.Right : ef2.Left;
                this.m_aptf[7].Y = tf2.Y;
                this.m_aptf[6].Y = this.m_aptf[7].Y;
                this.m_aptf[6].X = this.m_aptf[7].X + (ef2.Right - ef2.Left);
                this.m_aptf[5].Y = this.m_aptf[6].Y + (ef2.Bottom - ef2.Top);
                this.m_aptf[5].X = this.m_aptf[6].X;
                this.m_aptf[3].Y = this.m_aptf[2].Y;
                this.m_aptf[3].X = (tf1.X + tf2.X) / 2f;
                this.m_aptf[4].Y = this.m_aptf[5].Y;
                this.m_aptf[4].X = (tf1.X + tf2.X) / 2f;
                return;
            }
            default: {
                return;
            }
            }
            this.m_aptf[2].Y = this.m_aptf[1].Y;
            return;
            Label_0758:
                this.m_aptf[2].X = this.m_aptf[1].X;
            this.m_aptf[0].Y = tf1.Y;
            this.m_aptf[3].Y = tf2.Y;
            this.m_aptf[1].Y = this.m_aptf[0].Y;
            this.m_aptf[2].Y = this.m_aptf[3].Y;
            return;
            Label_0A26:
                this.m_aptf[2].X = this.m_aptf[3].X;
            Label_0A48:
                if (ef1.Contains(this.m_aptf[1])) {
                    if (this.m_aptf[0].Y == ef1.Bottom) {
                        this.m_aptf[1].Y = this.m_aptf[0].Y + (ef1.Bottom - ef1.Top);
                    }
                    else {
                        this.m_aptf[1].Y = this.m_aptf[0].Y - (ef1.Bottom - ef1.Top);
                    }
                    this.m_aptf[2].Y = this.m_aptf[1].Y;
                }
            return;
            Label_0D3F:
                this.m_aptf[2].Y = this.m_aptf[3].Y;
            Label_0D61:
                if (!ef1.Contains(this.m_aptf[1])) {
                    return;
                }
            if (this.m_aptf[0].X == ef1.Right) {
                this.m_aptf[1].X = this.m_aptf[0].X + (ef1.Right - ef1.Left);
            }
            else {
                this.m_aptf[1].X = this.m_aptf[0].X - (ef1.Right - ef1.Left);
            }
            this.m_aptf[2].X = this.m_aptf[1].X;
        }
 
        internal void ChangePoint(int idx, PointF pt) {
            this.m_aptf[idx] = pt;
            if (!this.m_line.HVStyle) {
                return;
            }
            if ((idx > 1) || !this.m_adjustOrg) {
                if ((idx < (this.m_aptf.Length - 2)) || !this.m_adjustDst) {
                    this.FixLinkPoints();
                    goto Label_01FB;
                }
                if (idx == (this.m_aptf.Length - 2)) {
                    switch (this.m_line.m_style) {
                    case LineStyle.VH:
                    case LineStyle.HVH:
                    case LineStyle.VHVH:
                    case LineStyle.HVHVH:
                    case LineStyle.VHVHVH:
                    case LineStyle.HVHVHVH: {
                        this.m_aptf[this.m_aptf.Length - 1].Y = pt.Y;
                        goto Label_01ED;
                    }
                    case LineStyle.HV:
                    case LineStyle.VHV:
                    case LineStyle.HVHV:
                    case LineStyle.VHVHV:
                    case LineStyle.HVHVHV:
                    case LineStyle.VHVHVHV: {
                        this.m_aptf[this.m_aptf.Length - 1].X = pt.X;
                        goto Label_01ED;
                    }
                    }
                }
            }
            else {
                if (idx == 1) {
                    switch (this.m_line.m_style) {
                    case LineStyle.VH: {
                        this.m_aptf[0].X = pt.X;
                        if (this.m_adjustDst) {
                            this.m_aptf[2].Y = pt.Y;
                        }
                        break;
                    }
                    case LineStyle.HV: {
                        this.m_aptf[0].Y = pt.Y;
                        if (this.m_adjustDst) {
                            this.m_aptf[2].X = pt.X;
                        }
                        break;
                    }
                    case LineStyle.VHV:
                    case LineStyle.VHVH:
                    case LineStyle.VHVHV:
                    case LineStyle.VHVHVH:
                    case LineStyle.VHVHVHV: {
                        this.m_aptf[0].X = pt.X;
                        break;
                    }
                    case LineStyle.HVH:
                    case LineStyle.HVHV:
                    case LineStyle.HVHVH:
                    case LineStyle.HVHVHV:
                    case LineStyle.HVHVHVH: {
                        this.m_aptf[0].Y = pt.Y;
                        break;
                    }
                    }
                }
                this.FixLinkPoints();
                goto Label_01FB;
            }
            Label_01ED:
                this.FixLinkPoints();
            Label_01FB:
                this.UpdateLinkPoints(pt, idx);
        }
 
        public object Clone() {
            Link link1 = new Link();
            link1.CloneProperties(this);
            return link1;
        }
 
        internal void CloneProperties(Link link) {
            this.m_font = link.m_font;
            this.m_drawColor = link.m_drawColor;
            this.m_textColor = link.m_textColor;
            this.m_dashStyle = link.m_dashStyle;
            this.m_drawWidth = link.m_drawWidth;
            this.m_jump = link.m_jump;
            this.m_line = (Line) link.m_line.Clone();
            this.m_startCap = link.m_startCap;
            this.m_endCap = link.m_endCap;
            this.m_arrowDst = (Arrow) link.m_arrowDst.Clone();
            this.m_arrowOrg = (Arrow) link.m_arrowOrg.Clone();
            this.m_arrowMid = (Arrow) link.m_arrowMid.Clone();
            this.m_rigid = link.m_rigid;
            this.m_hidden = link.m_hidden;
            this.m_adjustOrg = link.m_adjustOrg;
            this.m_adjustDst = link.m_adjustDst;
            this.m_orientedText = link.m_orientedText;
            this.m_stretchable = link.m_stretchable;
            this.m_selectable = link.m_selectable;
            this.m_logical = link.m_logical;
            this.m_maxPointsCount = link.m_maxPointsCount;
            this.m_text = link.m_text;
            this.m_tooltip = link.m_tooltip;
            this.m_tag = link.m_tag;
            if (link.m_aptf != null) {
                this.m_aptf = (PointF[]) link.m_aptf.Clone();
            }
            else {
                this.m_aptf = null;
            }
        }
 
        internal override void Draw(Graphics grfx) {
            LinkDrawFlags flags1 = this.m_ownerDraw ? this.m_af.GetLinkDrawFlags(grfx, this) : LinkDrawFlags.All;
            if (flags1 != LinkDrawFlags.None) {
                Pen pen1 = new Pen(this.m_drawColor);
                float single1 = (this.m_drawWidth == 0) ? ((float) 1) : ((float) this.m_drawWidth);
                float single2 = Misc.DistanceDeviceToWorld(grfx, single1);
                pen1.Width = single2;
                pen1.DashStyle = this.m_dashStyle;
                if ((flags1 & LinkDrawFlags.Shape) == LinkDrawFlags.Shape) {
                    GraphicsPath path1 = this.GetLinePath();
                    pen1.EndCap = this.m_endCap;
                    if ((pen1.EndCap == LineCap.Custom) && (this.m_customEndCap != null)) {
                        pen1.CustomEndCap = this.m_customEndCap;
                    }
                    pen1.StartCap = this.m_startCap;
                    if ((pen1.StartCap == LineCap.Custom) && (this.m_customStartCap != null)) {
                        pen1.CustomStartCap = this.m_customStartCap;
                    }
                    grfx.DrawPath(pen1, path1);
                    pen1.EndCap = LineCap.Flat;
                    pen1.StartCap = LineCap.Flat;
                }
                pen1.DashStyle = DashStyle.Solid;
                if ((flags1 & LinkDrawFlags.Arrows) == LinkDrawFlags.Arrows) {
                    if ((this.m_endCap == LineCap.Flat) && (this.m_arrowDst.m_style != ArrowStyle.None)) {
                        GraphicsPath path2 = this.m_arrowDst.GetPath2(this.m_aptf[this.m_aptf.Length - 2], this.m_aptf[this.m_aptf.Length - 1], grfx);
                        Color color1 = this.m_arrowDst.Filled ? this.m_drawColor : this.m_af.BackColor;
                        grfx.FillPath(new SolidBrush(color1), path2);
                        grfx.DrawPath(pen1, path2);
                    }
                    if ((this.m_startCap == LineCap.Flat) && (this.m_arrowOrg.m_style != ArrowStyle.None)) {
                        GraphicsPath path3 = this.m_arrowOrg.GetPath2(this.m_aptf[1], this.m_aptf[0], grfx);
                        Color color2 = this.m_arrowOrg.Filled ? this.m_drawColor : this.m_af.BackColor;
                        grfx.FillPath(new SolidBrush(color2), path3);
                        grfx.DrawPath(pen1, path3);
                    }
                    if ((this.m_arrowMid.m_style != ArrowStyle.None) && !this.m_line.CurveStyle) {
                        for (int num1 = 0; num1 < (this.m_aptf.Length - 1); num1++) {
                            GraphicsPath path4 = this.m_arrowMid.GetPath2(this.m_aptf[num1], Misc.MiddlePoint(this.m_aptf[num1], this.m_aptf[num1 + 1]), grfx);
                            Color color3 = this.m_arrowMid.Filled ? this.m_drawColor : this.m_af.BackColor;
                            grfx.FillPath(new SolidBrush(color3), path4);
                            grfx.DrawPath(pen1, path4);
                        }
                    }
                }
                if (((this.m_text != null) && (this.m_text.Length > 0)) && ((flags1 & LinkDrawFlags.Text) == LinkDrawFlags.Text)) {
                    PointF tf2;
                    PointF tf1 = this.GetTextPosition();
                    grfx.TranslateTransform(tf1.X, tf1.Y);
                    int num2 = this.m_orientedText ? this.GetTextAngle() : 0;
                    if (num2 != 0) {
                        grfx.RotateTransform((float) -num2);
                    }
                    Brush brush1 = new SolidBrush(this.m_textColor);
                    SizeF ef1 = grfx.MeasureString(this.m_text, this.m_font);
                    if (this.m_backMode == BackMode.Opaque) {
                        tf2 = new PointF(-ef1.Width / 2f, -ef1.Height / 2f);
                        RectangleF ef2 = new RectangleF(tf2, ef1);
                        grfx.FillRectangle(new SolidBrush(this.m_af.BackColor), ef2);
                    }
                    else {
                        tf2 = new PointF(-ef1.Width / 2f, 0f);
                    }
                    grfx.DrawString(this.m_text, this.m_font, brush1, tf2);
                    if (num2 != 0) {
                        grfx.RotateTransform((float) num2);
                    }
                    grfx.TranslateTransform(-tf1.X, -tf1.Y);
                }
            }
        }
 
        internal void FixLinkPoints() {
            PointF tf1 = this.m_adjustOrg ? this.m_aptf[0] : Misc.CenterPoint(this.m_org.m_rc);
            PointF tf2 = this.m_adjustDst ? this.m_aptf[this.m_aptf.Length - 1] : Misc.CenterPoint(this.m_dst.m_rc);
            switch (this.m_line.m_style) {
            case LineStyle.VH:
            case LineStyle.VHV:
            case LineStyle.VHVH:
            case LineStyle.VHVHV:
            case LineStyle.VHVHVH:
            case LineStyle.VHVHVHV: {
                this.m_aptf[1].X = tf1.X;
                if ((this.m_aptf.Length % 2) != 0) {
                    this.m_aptf[this.m_aptf.Length - 2].Y = tf2.Y;
                    return;
                }
                this.m_aptf[this.m_aptf.Length - 2].X = tf2.X;
                return;
            }
            case LineStyle.HV:
            case LineStyle.HVH:
            case LineStyle.HVHV:
            case LineStyle.HVHVH:
            case LineStyle.HVHVHV:
            case LineStyle.HVHVHVH: {
                this.m_aptf[1].Y = tf1.Y;
                if ((this.m_aptf.Length % 2) != 0) {
                    this.m_aptf[this.m_aptf.Length - 2].X = tf2.X;
                    return;
                }
                this.m_aptf[this.m_aptf.Length - 2].Y = tf2.Y;
                return;
            }
            }
        }
 
        private float GetDistance(PointF[] apt, int nbPt, PointF pt, float linkWidth) {
            float single1 = float.PositiveInfinity;
            for (int num1 = 0; num1 < (nbPt - 1); num1++) {
                if ((num1 == 0) || (num1 == (nbPt - 2))) {
                    float single2 = apt[num1].X;
                    float single3 = apt[num1].Y;
                    float single4 = apt[num1 + 1].X;
                    float single5 = apt[num1 + 1].Y;
                    if (single2 == single4) {
                        if ((pt.Y >= Math.Min(single3, single5)) && (pt.Y <= Math.Max(single3, single5))) {
                            goto Label_00EB;
                        }
                        goto Label_0112;
                    }
                    if (single3 == single5) {
                        if ((pt.X >= Math.Min(single2, single4)) && (pt.X <= Math.Max(single2, single4))) {
                            goto Label_00EB;
                        }
                        goto Label_0112;
                    }
                    PointF tf1 = new PointF(single2, single3);
                    PointF tf2 = new PointF(single4, single5);
                    RectangleF ef1 = Misc.Rect(tf1, tf2);
                    ef1.Inflate(linkWidth / 2f, linkWidth / 2f);
                    if (!ef1.Contains(pt)) {
                        goto Label_0112;
                    }
                }
            Label_00EB:
                single1 = Math.Min(single1, Misc.GetSegDist(apt[num1], apt[num1 + 1], pt));
            Label_0112:;
            }
            return single1;
        }
 
        private GraphicsPath GetLinePath() {
            GraphicsPath path1 = new GraphicsPath();
            switch (this.m_line.m_style) {
            case LineStyle.Bezier: {
                path1.AddBeziers(this.m_aptf);
                return path1;
            }
            case LineStyle.Spline: {
                path1.AddCurve(this.m_aptf);
                return path1;
            }
            }
            if (this.m_jump != Jump.None) {
                this.AddLineInter(path1);
                return path1;
            }
            if (this.m_line.m_roundedCorner) {
                this.AddLineRounded(path1);
                return path1;
            }
            path1.AddLines(this.m_aptf);
            return path1;
        }
 
        public GraphicsPath GetPath() {
            return this.GetLinePath();
        }
 
        private Link.StretchPos GetStretchPos(PointF point, RectangleF rc) {
            Link.StretchPos pos1 = Link.StretchPos.MidMid;
            if (point.Y < rc.Top) {
                if (point.X < rc.Left) {
                    return Link.StretchPos.LeftTop;
                }
                if ((point.X >= rc.Left) && (point.X <= rc.Right)) {
                    return Link.StretchPos.MidTop;
                }
                if (point.X > rc.Right) {
                    pos1 = Link.StretchPos.RightTop;
                }
                return pos1;
            }
            if ((point.Y >= rc.Top) && (point.Y <= rc.Bottom)) {
                if (point.X < rc.Left) {
                    return Link.StretchPos.LeftMid;
                }
                if ((point.X >= rc.Left) && (point.X <= rc.Right)) {
                    return Link.StretchPos.MidMid;
                }
                if (point.X > rc.Right) {
                    pos1 = Link.StretchPos.RightMid;
                }
                return pos1;
            }
            if (point.Y > rc.Bottom) {
                if (point.X < rc.Left) {
                    return Link.StretchPos.LeftBottom;
                }
                if ((point.X >= rc.Left) && (point.X <= rc.Right)) {
                    return Link.StretchPos.MidBottom;
                }
                if (point.X > rc.Right) {
                    pos1 = Link.StretchPos.RightBottom;
                }
            }
            return pos1;
        }
 
        private int GetTextAngle() {
            int num1 = 0;
            int num2 = this.m_aptf.Length / 2;
            float single1 = this.m_aptf[num2 - 1].X;
            float single2 = this.m_aptf[num2 - 1].Y;
            float single3 = this.m_aptf[num2].X;
            float single4 = this.m_aptf[num2].Y;
            float single5 = single3 - single1;
            float single6 = -(single4 - single2);
            float single7 = (float) Math.Sqrt((double) ((single5 * single5) + (single6 * single6)));
            if (single7 == 0f) {
                return 0;
            }
            float single8 = single5 / single7;
            float single9 = single6 / single7;
            num1 = (int) (57.295827908797776 * Math.Acos((double) single8));
            if (single9 < 0f) {
                num1 = -num1;
            }
            if (single8 < 0f) {
                num1 += 180;
            }
            return num1;
        }
 
        private PointF GetTextPosition() {
            int num1 = this.m_aptf.Length / 2;
            return Misc.MiddlePoint(this.m_aptf[num1 - 1], this.m_aptf[num1]);
        }
 
        internal bool HitTest(PointF pt) {
            bool flag1 = false;
            RectangleF ef1 = this.m_rc;
            float single1 = 0f;
            if (ef1.Contains(pt)) {
                PointF[] tfArray1;
                if (this.m_line.m_style == LineStyle.Bezier) {
                    tfArray1 = new PointF[11];
                    int num1 = 0;
                    for (float single2 = 0f; single2 < 1f; single2 += 0.1f) {
                        tfArray1[num1].X = (int) (((((((1f - single2) * (1f - single2)) * (1f - single2)) * this.m_aptf[0].X) + ((((3f * single2) * (1f - single2)) * (1f - single2)) * this.m_aptf[1].X)) + ((((3f * single2) * single2) * (1f - single2)) * this.m_aptf[2].X)) + (((single2 * single2) * single2) * this.m_aptf[3].X));
                        tfArray1[num1].Y = (int) (((((((1f - single2) * (1f - single2)) * (1f - single2)) * this.m_aptf[0].Y) + ((((3f * single2) * (1f - single2)) * (1f - single2)) * this.m_aptf[1].Y)) + ((((3f * single2) * single2) * (1f - single2)) * this.m_aptf[2].Y)) + (((single2 * single2) * single2) * this.m_aptf[3].Y));
                        num1++;
                    }
                    tfArray1[num1] = this.m_aptf[3];
                }
                else if ((this.m_line.m_style == LineStyle.Spline) && (this.m_aptf.Length > 2)) {
                    tfArray1 = new PointF[10 * (this.m_aptf.Length - 1)];
                    this.CanonicalSpline(tfArray1);
                }
                else {
                    tfArray1 = this.m_aptf;
                }
                single1 = this.GetDistance(tfArray1, tfArray1.Length, pt, this.m_af.m_linkWidthWorld);
                if (single1 < this.m_af.m_linkWidthWorld) {
                    Link.m_distance = single1;
                    flag1 = true;
                }
            }
            return flag1;
        }
 
        private void Init() {
            this.m_font = Control.DefaultFont;
            this.m_drawColor = SystemColors.WindowText;
            this.m_textColor = SystemColors.WindowText;
            this.m_hidden = false;
            this.m_logical = true;
            this.m_maxPointsCount = 0;
            this.m_selectable = true;
            this.m_ownerDraw = false;
            this.m_rigid = false;
            this.m_adjustOrg = false;
            this.m_adjustDst = false;
            this.m_orientedText = false;
            this.m_stretchable = true;
            this.m_startCap = LineCap.Flat;
            this.m_endCap = LineCap.Flat;
            this.m_customStartCap = null;
            this.m_customEndCap = null;
            this.m_arrowDst = new Arrow(ArrowStyle.Arrow, ArrowSize.Small, ArrowAngle.deg15, false);
            this.m_arrowDst.BeforeChange += new Arrow.BeforeChangeEventHandler(this.arrowDst_BeforeChange);
            this.m_arrowDst.AfterChange += new Arrow.AfterChangeEventHandler(this.arrow_AfterChange);
            this.m_arrowOrg = new Arrow(ArrowStyle.None, ArrowSize.Small, ArrowAngle.deg15, false);
            this.m_arrowOrg.BeforeChange += new Arrow.BeforeChangeEventHandler(this.arrowOrg_BeforeChange);
            this.m_arrowOrg.AfterChange += new Arrow.AfterChangeEventHandler(this.arrow_AfterChange);
            this.m_arrowMid = new Arrow(ArrowStyle.None, ArrowSize.Small, ArrowAngle.deg15, false);
            this.m_arrowMid.BeforeChange += new Arrow.BeforeChangeEventHandler(this.arrowMid_BeforeChange);
            this.m_arrowMid.AfterChange += new Arrow.AfterChangeEventHandler(this.arrow_AfterChange);
            this.m_line = new Line(LineStyle.Polyline, false, false);
            this.m_line.BeforeChange += new Line.BeforeChangeEventHandler(this.line_BeforeChange);
            this.m_line.AfterChange += new Line.AfterChangeEventHandler(this.line_AfterChange);
            this.m_jump = Jump.None;
            this.m_drawWidth = 1;
            this.m_dashStyle = DashStyle.Solid;
            this.m_backMode = BackMode.Transparent;
            this.m_org = null;
            this.m_dst = null;
            this.m_aptf2 = null;
            this.m_aptf = null;
        }
 
        internal void InitPoints() {
            this.m_aptf = new PointF[this.Reflexive ? 4 : 2];
            if (!this.Reflexive) {
                this.CalcLinkTips(true, true);
                if (this.m_line.m_style == LineStyle.Bezier) {
                    PointF tf1 = this.m_aptf[0];
                    PointF tf2 = this.m_aptf[1];
                    PointF tf3 = new PointF();
                    PointF tf4 = new PointF();
                    float single1 = tf2.X - tf1.X;
                    float single2 = tf2.Y - tf1.Y;
                    float single3 = (float) Math.Sqrt((double) ((single1 * single1) + (single2 * single2)));
                    tf3.X = (tf1.X + (single1 / 2f)) - (single3 / 5f);
                    tf3.Y = (tf1.Y + (single2 / 2f)) - (single3 / 5f);
                    tf4.X = (tf1.X + (single1 / 2f)) + (single3 / 5f);
                    tf4.Y = (tf1.Y + (single2 / 2f)) + (single3 / 5f);
                    PointF[] tfArray2 = new PointF[4] { tf1, tf3, tf4, tf2 } ;
                    PointF[] tfArray1 = tfArray2;
                    this.m_aptf = tfArray1;
                }
            }
            else {
                RectangleF ef1 = this.m_org.m_rc;
                this.m_aptf[0] = Misc.CenterPoint(ef1);
                this.m_aptf[1].X = ef1.Right + (ef1.Width / 2f);
                this.m_aptf[2].X = ef1.Left + (ef1.Width / 2f);
                if ((ef1.Top - ef1.Height) >= 0f) {
                    this.m_aptf[1].Y = ef1.Top - (ef1.Height / 2f);
                    this.m_aptf[2].Y = ef1.Top - ef1.Height;
                }
                else {
                    this.m_aptf[1].Y = ef1.Bottom + (ef1.Height / 2f);
                    this.m_aptf[2].Y = ef1.Bottom + ef1.Height;
                }
                this.m_aptf[3] = this.m_aptf[0];
                this.CalcLinkTips(true, false);
                this.CalcLinkTips(false, true);
            }
        }
 
        internal void InitWithDefLink(DefLink deflink) {
            if (deflink != null) {
                this.m_font = deflink.m_font;
                this.m_drawColor = deflink.m_drawColor;
                this.m_textColor = deflink.m_textColor;
                this.m_hidden = deflink.m_hidden;
                this.m_logical = deflink.m_logical;
                this.m_maxPointsCount = deflink.m_maxPointsCount;
                this.m_selectable = deflink.m_selectable;
                this.m_ownerDraw = deflink.m_ownerDraw;
                this.m_backMode = deflink.m_backMode;
                this.m_rigid = deflink.m_rigid;
                this.m_adjustOrg = deflink.m_adjustOrg;
                this.m_adjustDst = deflink.m_adjustDst;
                this.m_orientedText = deflink.m_orientedText;
                this.m_stretchable = deflink.m_stretchable;
                this.m_startCap = deflink.m_startCap;
                this.m_endCap = deflink.m_endCap;
                if (deflink.m_customStartCap != null) {
                    this.m_customStartCap = (CustomLineCap) deflink.m_customStartCap.Clone();
                }
                if (deflink.m_customEndCap != null) {
                    this.m_customEndCap = (CustomLineCap) deflink.m_customEndCap.Clone();
                }
                this.m_arrowDst = (Arrow) deflink.m_arrowDst.Clone();
                this.m_arrowDst.BeforeChange += new Arrow.BeforeChangeEventHandler(this.arrowDst_BeforeChange);
                this.m_arrowDst.AfterChange += new Arrow.AfterChangeEventHandler(this.arrow_AfterChange);
                this.m_arrowOrg = (Arrow) deflink.m_arrowOrg.Clone();
                this.m_arrowOrg.BeforeChange += new Arrow.BeforeChangeEventHandler(this.arrowOrg_BeforeChange);
                this.m_arrowOrg.AfterChange += new Arrow.AfterChangeEventHandler(this.arrow_AfterChange);
                this.m_arrowMid = (Arrow) deflink.m_arrowMid.Clone();
                this.m_arrowMid.BeforeChange += new Arrow.BeforeChangeEventHandler(this.arrowMid_BeforeChange);
                this.m_arrowMid.AfterChange += new Arrow.AfterChangeEventHandler(this.arrow_AfterChange);
                this.m_line = (Line) deflink.m_line.Clone();
                this.m_line.BeforeChange += new Line.BeforeChangeEventHandler(this.line_BeforeChange);
                this.m_line.AfterChange += new Line.AfterChangeEventHandler(this.line_AfterChange);
                this.m_jump = deflink.m_jump;
                this.m_drawWidth = deflink.m_drawWidth;
                this.m_dashStyle = deflink.m_dashStyle;
                this.m_text = deflink.m_text;
                this.m_tooltip = deflink.m_tooltip;
                this.m_org = null;
                this.m_dst = null;
                this.m_aptf2 = null;
                this.m_aptf = null;
            }
        }
 
        internal PointF[] InsertPoint(int pos, PointF pt) {
            if (!(this.m_maxPointsCount <= 0) ) {
                if ((this.m_aptf != null) && (this.m_aptf.Length >= this.m_maxPointsCount)) {
                    return this.m_aptf;
                }
            }

            PointF[] tfArray1 = null;
            if (this.m_aptf == null) {
                return new PointF[1] { pt } ;
            }
            tfArray1 = new PointF[this.m_aptf.Length + 1];
            for (int num1 = 0; num1 <= pos; num1++) {
                tfArray1[num1] = this.m_aptf[num1];
            }
            tfArray1[pos + 1] = pt;
            for (int num2 = pos + 2; num2 < (this.m_aptf.Length + 1); num2++) {
                tfArray1[num2] = this.m_aptf[num2 - 1];
            }
            return tfArray1;
        }
 
        internal int m_maxPointsCount = 0;
        public int MaxPointsCount{
            get  {
                return this.m_maxPointsCount;
            }
            set {
                this.m_maxPointsCount = value;
                if (this.m_maxPointsCount < 0) this.m_maxPointsCount = 0;
            }
        }

        private float InterX(float y, Shape shape, RectangleF rc, bool bLeft) {
            float single5;
            PointF tf2;
            float single2 = (rc.Right - rc.Left) / 2f;
            float single3 = (rc.Bottom - rc.Top) / 2f;
            if (single3 == 0f) {
                if (!bLeft) {
                    return rc.Right;
                }
                return rc.Left;
            }
            PointF tf1 = Misc.CenterPoint(rc);
            float single4 = y - tf1.Y;
            int num1 = bLeft ? -1 : 1;
            ShapeStyle style1 = shape.m_style;
            if (style1 <= ShapeStyle.Ellipse) {
                if ((style1 == ShapeStyle.Connector) || (style1 == ShapeStyle.Ellipse)) {
                    goto Label_00DB;
                }
                goto Label_012B;
            }
            if (style1 == ShapeStyle.InternalStorage) {
                goto Label_0114;
            }
            switch (style1) {
            case ShapeStyle.Or:
            case ShapeStyle.SummingJunction: {
                goto Label_00DB;
            }
            case ShapeStyle.OrGate:
            case ShapeStyle.Pentagon:
            case ShapeStyle.Preparation:
            case ShapeStyle.ProcessIso9000:
            case ShapeStyle.PunchedTape:
            case ShapeStyle.RoundRect:
            case ShapeStyle.SequentialAccessStorage:
            case ShapeStyle.StoredData: {
                goto Label_012B;
            }
            case ShapeStyle.PredefinedProcess:
            case ShapeStyle.Process:
            case ShapeStyle.Rectangle:
            case ShapeStyle.RectEdgeBump:
            case ShapeStyle.RectEdgeEtched:
            case ShapeStyle.RectEdgeRaised:
            case ShapeStyle.RectEdgeSunken: {
                goto Label_0114;
            }
            default: {
                goto Label_012B;
            }
            }
            Label_00DB:
                single5 = Math.Max((float) ((single3 * single3) - (single4 * single4)), (float) ((single4 * single4) - (single3 * single3)));
            float single6 = (single2 / single3) * ((float) Math.Sqrt((double) single5));
            return (tf1.X + (num1 * single6));
            Label_0114:
                return (bLeft ? rc.Left : rc.Right);
            Label_012B:
                tf2 = new PointF(bLeft ? rc.Left : rc.Right, y);
            tf1.Y = y;
            GraphicsPath path1 = shape.GetPathOfShape(rc);
            path1.Flatten();
            PointF[] tfArray1 = path1.PathPoints;
            tf2 = Misc.GetPolyNearestPt(tfArray1, tfArray1.Length, tf2, tf1);
            return tf2.X;
        }
 
        private float InterY(float x, Shape shape, RectangleF rc, bool bTop) {
            float single5;
            PointF tf2;
            float single2 = (rc.Right - rc.Left) / 2f;
            float single3 = (rc.Bottom - rc.Top) / 2f;
            if (single3 == 0f) {
                if (!bTop) {
                    return rc.Bottom;
                }
                return rc.Top;
            }
            PointF tf1 = Misc.CenterPoint(rc);
            float single4 = x - tf1.X;
            int num1 = bTop ? -1 : 1;
            ShapeStyle style1 = shape.m_style;
            if (style1 <= ShapeStyle.Ellipse) {
                if ((style1 == ShapeStyle.Connector) || (style1 == ShapeStyle.Ellipse)) {
                    goto Label_00DB;
                }
                goto Label_012B;
            }
            if (style1 == ShapeStyle.InternalStorage) {
                goto Label_0114;
            }
            switch (style1) {
            case ShapeStyle.Or:
            case ShapeStyle.SummingJunction: {
                goto Label_00DB;
            }
            case ShapeStyle.OrGate:
            case ShapeStyle.Pentagon:
            case ShapeStyle.Preparation:
            case ShapeStyle.ProcessIso9000:
            case ShapeStyle.PunchedTape:
            case ShapeStyle.RoundRect:
            case ShapeStyle.SequentialAccessStorage:
            case ShapeStyle.StoredData: {
                goto Label_012B;
            }
            case ShapeStyle.PredefinedProcess:
            case ShapeStyle.Process:
            case ShapeStyle.Rectangle:
            case ShapeStyle.RectEdgeBump:
            case ShapeStyle.RectEdgeEtched:
            case ShapeStyle.RectEdgeRaised:
            case ShapeStyle.RectEdgeSunken: {
                goto Label_0114;
            }
            default: {
                goto Label_012B;
            }
            }
            Label_00DB:
                single5 = Math.Max((float) ((single2 * single2) - (single4 * single4)), (float) ((single4 * single4) - (single2 * single2)));
            float single6 = (single3 / single2) * ((float) Math.Sqrt((double) single5));
            return (tf1.Y + (num1 * single6));
            Label_0114:
                return (bTop ? rc.Top : rc.Bottom);
            Label_012B:
                tf2 = new PointF(x, bTop ? rc.Top : rc.Bottom);
            tf1.X = x;
            GraphicsPath path1 = shape.GetPathOfShape(rc);
            path1.Flatten();
            PointF[] tfArray1 = path1.PathPoints;
            tf2 = Misc.GetPolyNearestPt(tfArray1, tfArray1.Length, tf2, tf1);
            return tf2.Y;
        }
 
        internal void Invalidate(bool change) {
            if (change) {
                if (!this.m_adjustOrg || !this.m_adjustDst) {
                    this.CalcLinkTips(!this.m_adjustOrg, !this.m_adjustDst);
                }
                this.CalcRect();
                this.m_af.UpdateRect2(this.m_rcSeg);
            }
            this.InvalidateItem();
            this.InvalidateHandles();
        }
 
        internal override void InvalidateHandles() {
            if ((base.Existing && (this.m_af.m_repaint == 0)) && this.m_selectable) {
                Graphics graphics1 = this.m_af.CreateGraphics();
                this.m_af.CoordinatesDeviceToWorld(graphics1);
                SizeF ef1 = new SizeF((float) (this.m_af.m_selGrabSize + 2), (float) (this.m_af.m_selGrabSize + 2));
                bool flag1 = this.m_line.NewPointsAllowed;
                int num1 = flag1 ? ((2 * this.m_aptf.Length) - 1) : this.m_aptf.Length;
                for (int num2 = 0; num2 < num1; num2++) {
                    PointF tf1;
                    if (flag1) {
                        tf1 = ((num2 % 2) == 0) ? this.m_aptf[num2 / 2] : Misc.MiddlePoint(this.m_aptf[num2 / 2], this.m_aptf[(num2 / 2) + 1]);
                    }
                    else {
                        tf1 = this.m_aptf[num2];
                    }
                    tf1 = Misc.PointWorldToDevice(graphics1, tf1);
                    RectangleF ef2 = RectangleF.FromLTRB(tf1.X - (ef1.Width / 2f), tf1.Y - (ef1.Height / 2f), tf1.X + (ef1.Width / 2f), tf1.Y + (ef1.Height / 2f));
                    this.m_af.Invalidate(Rectangle.Round(ef2));
                }
                graphics1.Dispose();
            }
        }
 
        internal override void InvalidateItem() {
            if (base.Existing && (this.m_af.m_repaint == 0)) {
                Graphics graphics1 = this.m_af.CreateGraphics();
                this.m_af.CoordinatesDeviceToWorld(graphics1);
                RectangleF ef1 = new RectangleF(base.RC.Location, base.RC.Size);
                ef1 = Misc.RectWorldToDevice(graphics1, ef1);
                ef1.Inflate((float) this.m_drawWidth, (float) this.m_drawWidth);
                ef1.Inflate((float) this.m_af.m_selGrabSize, (float) this.m_af.m_selGrabSize);
                this.m_af.Invalidate(Rectangle.Round(ef1));
                graphics1.Dispose();
            }
        }
 
        private void line_AfterChange(object sender, EventArgs e) {
            if (this.m_af != null) {
                this.Invalidate(false);
                if (this.m_line.HVStyle) {
                    this.ChangeLineStyle();
                }
                else if ((this.m_line.m_style == LineStyle.Bezier) && (this.m_aptf.Length != 4)) {
                    this.InitPoints();
                }
                this.Invalidate(true);
                this.m_af.IncrementChangedFlag(1);
            }
        }
 
        private void line_BeforeChange(object sender, EventArgs e) {
            if (((this.m_af != null) && this.m_af.m_undo.CanUndoRedo) && !this.m_af.m_undo.SkipUndo) {
                this.m_af.m_undo.SubmitTask(new LinkLineTask(this));
            }
        }
 
        internal void OnAdjustChanged() {
            this.Invalidate(false);
            if (this.m_line.HVStyle) {
                this.FixLinkPoints();
            }
            this.Invalidate(true);
        }
 
        public void Remove() {
            if (base.Existing) {
                AddFlow flow1 = this.m_af;
                this.m_af.RemoveLink(this);
                flow1.UpdateRect();
            }
        }
 
        internal PointF[] RemovePoint(int pos) {
            if (this.m_aptf == null) {
                return null;
            }
            int num1 = this.m_aptf.Length - 1;
            PointF[] tfArray1 = new PointF[num1];
            for (int num2 = 0; num2 < pos; num2++) {
                tfArray1[num2] = this.m_aptf[num2];
            }
            for (int num3 = pos; num3 < num1; num3++) {
                tfArray1[num3] = this.m_aptf[num3 + 1];
            }
            return tfArray1;
        }
 
        public void Reverse() {
            if (base.Existing) {
                if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                    this.m_af.m_undo.SubmitTask(new ReverseLinkTask(this));
                }
                this.Reverse2();
                this.m_af.IncrementChangedFlag(1);
            }
        }
 
        internal void Reverse2() {
            this.m_org.m_aLinks.Remove(this);
            this.m_dst.m_aLinks.Remove(this);
            Node node1 = this.m_org;
            Node node2 = this.m_dst;
            this.m_org = node2;
            this.m_dst = node1;
            PointF[] tfArray1 = this.m_aptf;
            int num1 = this.m_aptf.Length;
            for (int num2 = 0; num2 < (num1 / 2); num2++) {
                PointF tf1 = tfArray1[num2];
                tfArray1[num2] = tfArray1[(num1 - num2) - 1];
                tfArray1[(num1 - num2) - 1] = tf1;
            }
            node2.m_aLinks.Add(this);
            node1.m_aLinks.Add(this);
            this.InvalidateItem();
            if (this.m_line.HVStyle) {
                this.FixLinkPoints();
            }
            this.CalcLinkTips(!this.m_adjustOrg, !this.m_adjustDst);
            this.CalcRect();
            this.InvalidateItem();
        }
 
        internal void SetArrowDst2(Arrow newValue) {
            if (newValue != null) {
                this.m_arrowDst = newValue;
                this.m_arrowDst.BeforeChange += new Arrow.BeforeChangeEventHandler(this.arrowDst_BeforeChange);
                this.m_arrowDst.AfterChange += new Arrow.AfterChangeEventHandler(this.arrow_AfterChange);
                this.InvalidateItem();
            }
        }
 
        internal void SetArrowMid2(Arrow newValue) {
            if (newValue != null) {
                this.m_arrowMid = newValue;
                this.m_arrowMid.BeforeChange += new Arrow.BeforeChangeEventHandler(this.arrowMid_BeforeChange);
                this.m_arrowMid.AfterChange += new Arrow.AfterChangeEventHandler(this.arrow_AfterChange);
                this.InvalidateItem();
            }
        }
 
        internal void SetArrowOrg2(Arrow newValue) {
            if (newValue != null) {
                this.m_arrowOrg = newValue;
                this.m_arrowOrg.BeforeChange += new Arrow.BeforeChangeEventHandler(this.arrowOrg_BeforeChange);
                this.m_arrowOrg.AfterChange += new Arrow.AfterChangeEventHandler(this.arrow_AfterChange);
                this.InvalidateItem();
            }
        }
 
        internal void SetDashStyle2(DashStyle newValue) {
            this.m_dashStyle = newValue;
            this.InvalidateItem();
            this.InvalidateHandles();
        }
 
        internal void SetDrawWidth2(int newValue) {
            this.m_drawWidth = newValue;
            this.InvalidateItem();
            this.InvalidateHandles();
        }
 
        internal void SetDst2(Node newDst) {
            this.m_dst.m_aLinks.Remove(this);
            this.m_dst = newDst;
            newDst.m_aLinks.Add(this);
            if (this.Reflexive && (this.m_aptf.Length == 2)) {
                this.InitPoints();
            }
            this.InvalidateItem();
            if (this.m_line.HVStyle) {
                this.FixLinkPoints();
            }
            this.CalcLinkTips(!this.AdjustDst, !this.AdjustDst);
            this.CalcRect();
            this.InvalidateItem();
        }
 
        internal void SetFont2(Font newValue) {
            this.InvalidateItem();
            this.InvalidateHandles();
            this.m_font = newValue;
            this.CalcRect();
            this.InvalidateItem();
            this.InvalidateHandles();
        }
 
        internal void SetLine2(Line newValue) {
            if (newValue != null) {
                this.Invalidate(false);
                bool flag1 = newValue.m_style != this.m_line.m_style;
                this.m_line = newValue;
                this.m_line.BeforeChange += new Line.BeforeChangeEventHandler(this.line_BeforeChange);
                this.m_line.AfterChange += new Line.AfterChangeEventHandler(this.line_AfterChange);
                if (flag1) {
                    if (this.m_line.HVStyle) {
                        this.ChangeLineStyle();
                    }
                    else if ((this.m_line.m_style == LineStyle.Bezier) && (this.m_aptf.Length != 4)) {
                        this.InitPoints();
                    }
                }
                this.Invalidate(true);
            }
        }
 
        internal void SetOrg2(Node newOrg) {
            this.m_org.m_aLinks.Remove(this);
            this.m_org = newOrg;
            newOrg.m_aLinks.Add(this);
            if (this.Reflexive && (this.m_aptf.Length == 2)) {
                this.InitPoints();
            }
            this.InvalidateItem();
            if (this.m_line.HVStyle) {
                this.FixLinkPoints();
            }
            this.CalcLinkTips(!this.AdjustDst, !this.AdjustDst);
            this.CalcRect();
            this.InvalidateItem();
        }
 
        internal void SetOrientedText2(bool newValue) {
            this.Invalidate(false);
            this.m_orientedText = newValue;
            this.Invalidate(true);
        }
 
        internal void SetText2(string newValue) {
            this.Invalidate(false);
            this.m_text = newValue;
            this.Invalidate(true);
        }
 
        internal void UpdateDynaLink(PointF point, int nHandle) {
            Link.StretchPos pos1;
            int num1;
            float single1;
            Link.StretchDir dir1;
            Link.StretchDir dir2;
            int num6;
            int num7;
            int num10;
            Link.StretchPos pos2;
            Link.StretchDir dir3;
            int num11;
            PointF tf1 = new PointF();
            RectangleF ef1 = new RectangleF();
            int num2 = this.m_aptf.Length;
            RectangleF ef2 = this.m_org.m_rc;
            RectangleF ef3 = this.m_dst.m_rc;
            float single2 = ef2.Left;
            float single3 = ef2.Right;
            float single4 = ef2.Top;
            float single5 = ef2.Bottom;
            float single6 = ef3.Left;
            float single7 = ef3.Right;
            float single8 = ef3.Top;
            float single9 = ef3.Bottom;
            float single10 = (single2 + single3) / 2f;
            float single11 = (single4 + single5) / 2f;
            float single12 = (single6 + single7) / 2f;
            float single13 = (single8 + single9) / 2f;
            if (this.m_aptf2[0].X == this.m_aptf2[1].X) {
                dir1 = (this.m_aptf2[0].Y > this.m_aptf2[1].Y) ? Link.StretchDir.North : Link.StretchDir.South;
            }
            else {
                dir1 = (this.m_aptf2[0].X < this.m_aptf2[1].X) ? Link.StretchDir.East : Link.StretchDir.West;
            }
            if (this.m_aptf2[this.m_aptf2.Length - 1].X == this.m_aptf2[this.m_aptf2.Length - 2].X) {
                dir2 = (this.m_aptf2[this.m_aptf2.Length - 1].Y > this.m_aptf2[this.m_aptf2.Length - 2].Y) ? Link.StretchDir.North : Link.StretchDir.South;
            }
            else {
                dir2 = (this.m_aptf2[this.m_aptf2.Length - 1].X < this.m_aptf2[this.m_aptf2.Length - 2].X) ? Link.StretchDir.East : Link.StretchDir.West;
            }
            if ((single12 < single10) && (single13 < single11)) {
                num1 = 1;
            }
            else if (single12 < single10) {
                num1 = 2;
            }
            else if (single13 < single11) {
                num1 = 3;
            }
            else {
                num1 = 0;
            }
            PointF[] tfArray1 = new PointF[0x10];
            this.m_aptf.CopyTo(tfArray1, 0);
            if (single12 < single10) {
                single2 = -single2;
                single3 = -single3;
                single6 = -single6;
                single7 = -single7;
                single10 = -single10;
                single12 = -single12;
                point.X = -point.X;
                single1 = single2;
                single2 = single3;
                single3 = single1;
                single1 = single6;
                single6 = single7;
                single7 = single1;
                for (int num3 = 0; num3 < 0x10; num3++) {
                    tfArray1[num3].X = -tfArray1[num3].X;
                }
            }
            if (single13 < single11) {
                single4 = -single4;
                single5 = -single5;
                single8 = -single8;
                single9 = -single9;
                single11 = -single11;
                single13 = -single13;
                point.Y = -point.Y;
                single1 = single4;
                single4 = single5;
                single5 = single1;
                single1 = single8;
                single8 = single9;
                single9 = single1;
                for (int num4 = 0; num4 < 0x10; num4++) {
                    tfArray1[num4].Y = -tfArray1[num4].Y;
                }
            }
            if (nHandle > 1) {
                if (nHandle >= (this.m_aptf.Length - 2)) {
                    ef1 = RectangleF.FromLTRB(single6, single8, single7, single9);
                    pos1 = this.GetStretchPos(point, ef1);
                    pos2 = pos1;
                    switch (pos2) {
                    case Link.StretchPos.MidMid: {
                        goto Label_1815;
                    }
                    case Link.StretchPos.MidTop: {
                        dir3 = dir1;
                        switch (dir3) {
                        case Link.StretchDir.North: {
                            num2 = 4;
                            goto Label_119F;
                        }
                        case Link.StretchDir.East: {
                            num2 = 5;
                            goto Label_119F;
                        }
                        case Link.StretchDir.South: {
                            num2 = 4;
                            goto Label_119F;
                        }
                        case Link.StretchDir.West: {
                            num2 = 5;
                            goto Label_119F;
                        }
                        }
                        goto Label_119F;
                    }
                    case Link.StretchPos.RightTop: {
                        if ((dir2 != Link.StretchDir.East) && (dir2 != Link.StretchDir.West)) {
                            dir3 = dir1;
                            switch (dir3) {
                            case Link.StretchDir.North: {
                                num2 = 5;
                                goto Label_10D9;
                            }
                            case Link.StretchDir.East: {
                                num2 = 6;
                                goto Label_10D9;
                            }
                            case Link.StretchDir.South: {
                                num2 = 5;
                                goto Label_10D9;
                            }
                            case Link.StretchDir.West: {
                                num2 = 6;
                                goto Label_10D9;
                            }
                            }
                            goto Label_10D9;
                        }
                        dir3 = dir1;
                        switch (dir3) {
                        case Link.StretchDir.North: {
                            num2 = 6;
                            goto Label_1013;
                        }
                        case Link.StretchDir.East: {
                            num2 = 5;
                            goto Label_1013;
                        }
                        case Link.StretchDir.South: {
                            num2 = 6;
                            goto Label_1013;
                        }
                        case Link.StretchDir.West: {
                            num2 = 5;
                            goto Label_1013;
                        }
                        }
                        goto Label_1013;
                    }
                    case Link.StretchPos.RightMid: {
                        dir3 = dir1;
                        switch (dir3) {
                        case Link.StretchDir.North: {
                            num2 = 5;
                            goto Label_135B;
                        }
                        case Link.StretchDir.East: {
                            num2 = 4;
                            goto Label_135B;
                        }
                        case Link.StretchDir.South: {
                            num2 = 5;
                            goto Label_135B;
                        }
                        case Link.StretchDir.West: {
                            num2 = 6;
                            goto Label_135B;
                        }
                        }
                        goto Label_135B;
                    }
                    case Link.StretchPos.RightBottom: {
                        if ((dir2 != Link.StretchDir.East) && (dir2 != Link.StretchDir.West)) {
                            dir3 = dir1;
                            switch (dir3) {
                            case Link.StretchDir.North: {
                                num2 = 5;
                                goto Label_16A5;
                            }
                            case Link.StretchDir.East: {
                                num2 = 6;
                                goto Label_16A5;
                            }
                            case Link.StretchDir.South: {
                                num2 = 5;
                                goto Label_16A5;
                            }
                            case Link.StretchDir.West: {
                                num2 = 6;
                                goto Label_16A5;
                            }
                            }
                            goto Label_16A5;
                        }
                        dir3 = dir1;
                        switch (dir3) {
                        case Link.StretchDir.North: {
                            num2 = 6;
                            goto Label_15DF;
                        }
                        case Link.StretchDir.East: {
                            num2 = 5;
                            goto Label_15DF;
                        }
                        case Link.StretchDir.South: {
                            num2 = 6;
                            goto Label_15DF;
                        }
                        case Link.StretchDir.West: {
                            num2 = 5;
                            goto Label_15DF;
                        }
                        }
                        goto Label_15DF;
                    }
                    case Link.StretchPos.MidBottom: {
                        dir3 = dir1;
                        switch (dir3) {
                        case Link.StretchDir.North: {
                            num2 = 6;
                            goto Label_176B;
                        }
                        case Link.StretchDir.East: {
                            num2 = 5;
                            goto Label_176B;
                        }
                        case Link.StretchDir.South: {
                            num2 = 4;
                            goto Label_176B;
                        }
                        case Link.StretchDir.West: {
                            num2 = 5;
                            goto Label_176B;
                        }
                        }
                        goto Label_176B;
                    }
                    case Link.StretchPos.LeftBottom: {
                        if ((dir2 != Link.StretchDir.East) && (dir2 != Link.StretchDir.West)) {
                            dir3 = dir1;
                            switch (dir3) {
                            case Link.StretchDir.North: {
                                num2 = 5;
                                goto Label_150C;
                            }
                            case Link.StretchDir.East: {
                                num2 = 6;
                                goto Label_150C;
                            }
                            case Link.StretchDir.South: {
                                num2 = 7;
                                goto Label_150C;
                            }
                            case Link.StretchDir.West: {
                                num2 = 6;
                                goto Label_150C;
                            }
                            }
                            goto Label_150C;
                        }
                        dir3 = dir1;
                        switch (dir3) {
                        case Link.StretchDir.North: {
                            num2 = 6;
                            goto Label_1446;
                        }
                        case Link.StretchDir.East: {
                            num2 = 5;
                            goto Label_1446;
                        }
                        case Link.StretchDir.South: {
                            num2 = 6;
                            goto Label_1446;
                        }
                        case Link.StretchDir.West: {
                            num2 = 5;
                            goto Label_1446;
                        }
                        }
                        goto Label_1446;
                    }
                    case Link.StretchPos.LeftMid: {
                        dir3 = dir1;
                        switch (dir3) {
                        case Link.StretchDir.North: {
                            num2 = 5;
                            goto Label_127D;
                        }
                        case Link.StretchDir.East: {
                            num2 = 4;
                            goto Label_127D;
                        }
                        case Link.StretchDir.South: {
                            num2 = 5;
                            goto Label_127D;
                        }
                        case Link.StretchDir.West: {
                            num2 = 4;
                            goto Label_127D;
                        }
                        }
                        goto Label_127D;
                    }
                    case Link.StretchPos.LeftTop: {
                        if ((dir2 != Link.StretchDir.East) && (dir2 != Link.StretchDir.West)) {
                            dir3 = dir1;
                            switch (dir3) {
                            case Link.StretchDir.North: {
                                num2 = 5;
                                goto Label_0F40;
                            }
                            case Link.StretchDir.East: {
                                num2 = 6;
                                goto Label_0F40;
                            }
                            case Link.StretchDir.South: {
                                num2 = 5;
                                goto Label_0F40;
                            }
                            case Link.StretchDir.West: {
                                num2 = 6;
                                goto Label_0F40;
                            }
                            }
                            goto Label_0F40;
                        }
                        dir3 = dir1;
                        switch (dir3) {
                        case Link.StretchDir.North: {
                            num2 = 6;
                            goto Label_0E7A;
                        }
                        case Link.StretchDir.East: {
                            num2 = 5;
                            goto Label_0E7A;
                        }
                        case Link.StretchDir.South: {
                            num2 = 6;
                            goto Label_0E7A;
                        }
                        case Link.StretchDir.West: {
                            num2 = 5;
                            goto Label_0E7A;
                        }
                        }
                        goto Label_0E7A;
                    }
                    }
                }
                goto Label_1815;
            }
            for (int num5 = 0; num5 < (num2 / 2); num5++) {
                tf1 = tfArray1[num5];
                tfArray1[num5] = tfArray1[(num2 - num5) - 1];
                tfArray1[(num2 - num5) - 1] = tf1;
            }
            ef1 = RectangleF.FromLTRB(single2, single4, single3, single5);
            pos1 = this.GetStretchPos(point, ef1);
            pos2 = pos1;
            switch (pos2) {
            case Link.StretchPos.MidMid: {
                goto Label_0D88;
            }
            case Link.StretchPos.MidTop: {
                dir3 = dir2;
                switch (dir3) {
                case Link.StretchDir.North: {
                    num2 = 4;
                    goto Label_0712;
                }
                case Link.StretchDir.East: {
                    num2 = 5;
                    goto Label_0712;
                }
                case Link.StretchDir.South: {
                    num2 = 6;
                    goto Label_0712;
                }
                case Link.StretchDir.West: {
                    num2 = 5;
                    goto Label_0712;
                }
                }
                goto Label_0712;
            }
            case Link.StretchPos.RightTop: {
                if ((dir1 != Link.StretchDir.East) && (dir1 != Link.StretchDir.West)) {
                    dir3 = dir2;
                    switch (dir3) {
                    case Link.StretchDir.North: {
                        num2 = 5;
                        goto Label_064C;
                    }
                    case Link.StretchDir.East: {
                        num2 = 6;
                        goto Label_064C;
                    }
                    case Link.StretchDir.South: {
                        num2 = 5;
                        goto Label_064C;
                    }
                    case Link.StretchDir.West: {
                        num2 = 6;
                        goto Label_064C;
                    }
                    }
                    goto Label_064C;
                }
                dir3 = dir2;
                switch (dir3) {
                case Link.StretchDir.North: {
                    num2 = 6;
                    goto Label_0586;
                }
                case Link.StretchDir.East: {
                    num2 = 5;
                    goto Label_0586;
                }
                case Link.StretchDir.South: {
                    num2 = 6;
                    goto Label_0586;
                }
                case Link.StretchDir.West: {
                    num2 = 5;
                    goto Label_0586;
                }
                }
                goto Label_0586;
            }
            case Link.StretchPos.RightMid: {
                dir3 = dir2;
                switch (dir3) {
                case Link.StretchDir.North: {
                    num2 = 5;
                    goto Label_08CE;
                }
                case Link.StretchDir.East: {
                    num2 = 4;
                    goto Label_08CE;
                }
                case Link.StretchDir.South: {
                    num2 = 5;
                    goto Label_08CE;
                }
                case Link.StretchDir.West: {
                    num2 = 4;
                    goto Label_08CE;
                }
                }
                goto Label_08CE;
            }
            case Link.StretchPos.RightBottom: {
                if ((dir1 != Link.StretchDir.East) && (dir1 != Link.StretchDir.West)) {
                    dir3 = dir2;
                    switch (dir3) {
                    case Link.StretchDir.North: {
                        num2 = 5;
                        goto Label_0C18;
                    }
                    case Link.StretchDir.East: {
                        num2 = 6;
                        goto Label_0C18;
                    }
                    case Link.StretchDir.South: {
                        num2 = 5;
                        goto Label_0C18;
                    }
                    case Link.StretchDir.West: {
                        num2 = 6;
                        goto Label_0C18;
                    }
                    }
                    goto Label_0C18;
                }
                dir3 = dir2;
                switch (dir3) {
                case Link.StretchDir.North: {
                    num2 = 6;
                    goto Label_0B52;
                }
                case Link.StretchDir.East: {
                    num2 = 5;
                    goto Label_0B52;
                }
                case Link.StretchDir.South: {
                    num2 = 6;
                    goto Label_0B52;
                }
                case Link.StretchDir.West: {
                    num2 = 5;
                    goto Label_0B52;
                }
                }
                goto Label_0B52;
            }
            case Link.StretchPos.MidBottom: {
                dir3 = dir2;
                switch (dir3) {
                case Link.StretchDir.North: {
                    num2 = 4;
                    goto Label_0CDE;
                }
                case Link.StretchDir.East: {
                    num2 = 5;
                    goto Label_0CDE;
                }
                case Link.StretchDir.South: {
                    num2 = 4;
                    goto Label_0CDE;
                }
                case Link.StretchDir.West: {
                    num2 = 5;
                    goto Label_0CDE;
                }
                }
                goto Label_0CDE;
            }
            case Link.StretchPos.LeftBottom: {
                if ((dir1 != Link.StretchDir.East) && (dir1 != Link.StretchDir.West)) {
                    dir3 = dir2;
                    switch (dir3) {
                    case Link.StretchDir.North: {
                        num2 = 5;
                        goto Label_0A7F;
                    }
                    case Link.StretchDir.East: {
                        num2 = 6;
                        goto Label_0A7F;
                    }
                    case Link.StretchDir.South: {
                        num2 = 5;
                        goto Label_0A7F;
                    }
                    case Link.StretchDir.West: {
                        num2 = 6;
                        goto Label_0A7F;
                    }
                    }
                    goto Label_0A7F;
                }
                dir3 = dir2;
                switch (dir3) {
                case Link.StretchDir.North: {
                    num2 = 6;
                    goto Label_09B9;
                }
                case Link.StretchDir.East: {
                    num2 = 5;
                    goto Label_09B9;
                }
                case Link.StretchDir.South: {
                    num2 = 6;
                    goto Label_09B9;
                }
                case Link.StretchDir.West: {
                    num2 = 5;
                    goto Label_09B9;
                }
                }
                goto Label_09B9;
            }
            case Link.StretchPos.LeftMid: {
                dir3 = dir2;
                switch (dir3) {
                case Link.StretchDir.North: {
                    num2 = 5;
                    goto Label_07F0;
                }
                case Link.StretchDir.East: {
                    num2 = 6;
                    goto Label_07F0;
                }
                case Link.StretchDir.South: {
                    num2 = 5;
                    goto Label_07F0;
                }
                case Link.StretchDir.West: {
                    num2 = 4;
                    goto Label_07F0;
                }
                }
                goto Label_07F0;
            }
            case Link.StretchPos.LeftTop: {
                if ((dir1 != Link.StretchDir.East) && (dir1 != Link.StretchDir.West)) {
                    dir3 = dir2;
                    switch (dir3) {
                    case Link.StretchDir.North: {
                        num2 = 5;
                        goto Label_04B3;
                    }
                    case Link.StretchDir.East: {
                        num2 = 6;
                        goto Label_04B3;
                    }
                    case Link.StretchDir.South: {
                        num2 = 5;
                        goto Label_04B3;
                    }
                    case Link.StretchDir.West: {
                        num2 = 6;
                        goto Label_04B3;
                    }
                    }
                    goto Label_04B3;
                }
                dir3 = dir2;
                switch (dir3) {
                case Link.StretchDir.North: {
                    num2 = 6;
                    break;
                }
                case Link.StretchDir.East: {
                    num2 = 5;
                    break;
                }
                case Link.StretchDir.South: {
                    num2 = 6;
                    break;
                }
                case Link.StretchDir.West: {
                    num2 = 5;
                    break;
                }
                }
                break;
            }
            default: {
                goto Label_0D88;
            }
            }
            tfArray1[num2 - 4].X = point.X;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = single10;
            tfArray1[num2 - 2].Y = point.Y;
            tfArray1[num2 - 1].X = single10;
            tfArray1[num2 - 1].Y = single4;
            goto Label_0D88;
            Label_04B3:
                tfArray1[num2 - 4].Y = point.Y;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = point.X;
            tfArray1[num2 - 2].Y = single11;
            tfArray1[num2 - 1].X = single2;
            tfArray1[num2 - 1].Y = single11;
            goto Label_0D88;
            Label_0586:
                tfArray1[num2 - 4].X = point.X;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = single10;
            tfArray1[num2 - 2].Y = point.Y;
            tfArray1[num2 - 1].X = single10;
            tfArray1[num2 - 1].Y = single4;
            goto Label_0D88;
            Label_064C:
                tfArray1[num2 - 4].Y = point.Y;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = point.X;
            tfArray1[num2 - 2].Y = single11;
            tfArray1[num2 - 1].X = single3;
            tfArray1[num2 - 1].Y = single11;
            goto Label_0D88;
            Label_0712:
                tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = this.m_adjustOrg ? point.X : single10;
            tfArray1[num2 - 2].Y = point.Y;
            tfArray1[num2 - 1].X = this.m_adjustOrg ? point.X : single10;
            tfArray1[num2 - 1].Y = this.m_adjustOrg ? this.InterY(point.X, this.m_org.m_shape, ef1, true) : single4;
            goto Label_0D88;
            Label_07F0:
                tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 2].X = point.X;
            tfArray1[num2 - 2].Y = this.m_adjustOrg ? point.Y : single11;
            tfArray1[num2 - 1].X = this.m_adjustOrg ? this.InterX(point.Y, this.m_org.m_shape, ef1, true) : single2;
            tfArray1[num2 - 1].Y = this.m_adjustOrg ? point.Y : single11;
            goto Label_0D88;
            Label_08CE:
                tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 2].X = point.X;
            tfArray1[num2 - 2].Y = this.m_adjustOrg ? point.Y : single11;
            tfArray1[num2 - 1].X = this.m_adjustOrg ? this.InterX(point.Y, this.m_org.m_shape, ef1, false) : single3;
            tfArray1[num2 - 1].Y = this.m_adjustOrg ? point.Y : single11;
            goto Label_0D88;
            Label_09B9:
                tfArray1[num2 - 4].X = point.X;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = single10;
            tfArray1[num2 - 2].Y = point.Y;
            tfArray1[num2 - 1].X = single10;
            tfArray1[num2 - 1].Y = single5;
            goto Label_0D88;
            Label_0A7F:
                tfArray1[num2 - 4].Y = point.Y;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = point.X;
            tfArray1[num2 - 2].Y = single11;
            tfArray1[num2 - 1].X = single2;
            tfArray1[num2 - 1].Y = single11;
            goto Label_0D88;
            Label_0B52:
                tfArray1[num2 - 4].X = point.X;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = single10;
            tfArray1[num2 - 2].Y = point.Y;
            tfArray1[num2 - 1].X = single10;
            tfArray1[num2 - 1].Y = single5;
            goto Label_0D88;
            Label_0C18:
                tfArray1[num2 - 4].Y = point.Y;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = point.X;
            tfArray1[num2 - 2].Y = single11;
            tfArray1[num2 - 1].X = single3;
            tfArray1[num2 - 1].Y = single11;
            goto Label_0D88;
            Label_0CDE:
                tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = this.m_adjustOrg ? point.X : single10;
            tfArray1[num2 - 2].Y = point.Y;
            tfArray1[num2 - 1].X = this.m_adjustOrg ? point.X : single10;
            tfArray1[num2 - 1].Y = this.m_adjustOrg ? this.InterY(point.X, this.m_org.m_shape, ef1, false) : single5;
            Label_0D88:
                num6 = 0;
            while (num6 < (num2 / 2)) {
                tf1 = tfArray1[num6];
                tfArray1[num6] = tfArray1[(num2 - num6) - 1];
                tfArray1[(num2 - num6) - 1] = tf1;
                num6++;
            }
            goto Label_1815;
            Label_0E7A:
                tfArray1[num2 - 4].X = point.X;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = single12;
            tfArray1[num2 - 2].Y = point.Y;
            tfArray1[num2 - 1].X = single12;
            tfArray1[num2 - 1].Y = single8;
            goto Label_1815;
            Label_0F40:
                tfArray1[num2 - 4].Y = point.Y;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = point.X;
            tfArray1[num2 - 2].Y = single13;
            tfArray1[num2 - 1].X = single6;
            tfArray1[num2 - 1].Y = single13;
            goto Label_1815;
            Label_1013:
                tfArray1[num2 - 4].X = point.X;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = single12;
            tfArray1[num2 - 2].Y = point.Y;
            tfArray1[num2 - 1].X = single12;
            tfArray1[num2 - 1].Y = single8;
            goto Label_1815;
            Label_10D9:
                tfArray1[num2 - 4].Y = point.Y;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = point.X;
            tfArray1[num2 - 2].Y = single13;
            tfArray1[num2 - 1].X = single7;
            tfArray1[num2 - 1].Y = single13;
            goto Label_1815;
            Label_119F:
                tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = this.m_adjustDst ? point.X : single12;
            tfArray1[num2 - 2].Y = point.Y;
            tfArray1[num2 - 1].X = this.m_adjustDst ? point.X : single12;
            tfArray1[num2 - 1].Y = this.m_adjustDst ? this.InterY(point.X, this.m_dst.m_shape, ef1, true) : single8;
            goto Label_1815;
            Label_127D:
                tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 2].X = point.X;
            tfArray1[num2 - 2].Y = this.m_adjustDst ? point.Y : single13;
            tfArray1[num2 - 1].X = this.m_adjustDst ? this.InterX(point.Y, this.m_dst.m_shape, ef1, true) : single6;
            tfArray1[num2 - 1].Y = this.m_adjustDst ? point.Y : single13;
            goto Label_1815;
            Label_135B:
                tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 2].X = point.X;
            tfArray1[num2 - 2].Y = this.m_adjustDst ? point.Y : single13;
            tfArray1[num2 - 1].X = this.m_adjustDst ? this.InterX(point.Y, this.m_dst.m_shape, ef1, false) : single7;
            tfArray1[num2 - 1].Y = this.m_adjustDst ? point.Y : single13;
            goto Label_1815;
            Label_1446:
                tfArray1[num2 - 4].X = point.X;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = single12;
            tfArray1[num2 - 2].Y = point.Y;
            tfArray1[num2 - 1].X = single12;
            tfArray1[num2 - 1].Y = single9;
            goto Label_1815;
            Label_150C:
                tfArray1[num2 - 4].Y = point.Y;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = point.X;
            tfArray1[num2 - 2].Y = single13;
            tfArray1[num2 - 1].X = single6;
            tfArray1[num2 - 1].Y = single13;
            goto Label_1815;
            Label_15DF:
                tfArray1[num2 - 4].X = point.X;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = single12;
            tfArray1[num2 - 2].Y = point.Y;
            tfArray1[num2 - 1].X = single12;
            tfArray1[num2 - 1].Y = single9;
            goto Label_1815;
            Label_16A5:
                tfArray1[num2 - 4].Y = point.Y;
            tfArray1[num2 - 3].X = point.X;
            tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = point.X;
            tfArray1[num2 - 2].Y = single13;
            tfArray1[num2 - 1].X = single7;
            tfArray1[num2 - 1].Y = single13;
            goto Label_1815;
            Label_176B:
                tfArray1[num2 - 3].Y = point.Y;
            tfArray1[num2 - 2].X = this.m_adjustDst ? point.X : single12;
            tfArray1[num2 - 2].Y = point.Y;
            tfArray1[num2 - 1].X = this.m_adjustDst ? point.X : single12;
            tfArray1[num2 - 1].Y = this.m_adjustDst ? this.InterY(point.X, this.m_dst.m_shape, ef1, false) : single9;
            Label_1815:
                num11 = num2;
            switch (num11) {
            case 3: {
                this.m_line.m_style = (tfArray1[0].Y == tfArray1[1].Y) ? LineStyle.HV : LineStyle.VH;
                break;
            }
            case 4: {
                this.m_line.m_style = (tfArray1[0].Y == tfArray1[1].Y) ? LineStyle.HVH : LineStyle.VHV;
                break;
            }
            case 5: {
                this.m_line.m_style = (tfArray1[0].Y == tfArray1[1].Y) ? LineStyle.HVHV : LineStyle.VHVH;
                break;
            }
            case 6: {
                this.m_line.m_style = (tfArray1[0].Y == tfArray1[1].Y) ? LineStyle.HVHVH : LineStyle.VHVHV;
                break;
            }
            case 7: {
                this.m_line.m_style = (tfArray1[0].Y == tfArray1[1].Y) ? LineStyle.HVHVHV : LineStyle.VHVHVH;
                break;
            }
            case 8: {
                this.m_line.m_style = (tfArray1[0].Y == tfArray1[1].Y) ? LineStyle.HVHVHVH : LineStyle.VHVHVHV;
                break;
            }
            default: {
                this.m_line.m_style = (tfArray1[0].Y == tfArray1[1].Y) ? ((LineStyle) 0x10) : ((LineStyle) 0x11);
                break;
            }
            }
            this.m_aptf = new PointF[num2];
            num11 = num1;
            switch (num11) {
            case 0: {
                num7 = 0;
                goto Label_19E0;
            }
            case 1: {
                for (int num8 = 0; num8 < num2; num8++) {
                    this.m_aptf[num8].X = -tfArray1[num8].X;
                    this.m_aptf[num8].Y = -tfArray1[num8].Y;
                }
                return;
            }
            case 2: {
                for (int num9 = 0; num9 < num2; num9++) {
                    this.m_aptf[num9].X = -tfArray1[num9].X;
                    this.m_aptf[num9].Y = tfArray1[num9].Y;
                }
                return;
            }
            case 3: {
                num10 = 0;
                goto Label_1ADA;
            }
            default: {
                return;
            }
            }
            Label_19E0:
                if (num7 >= num2) {
                    return;
                }
            this.m_aptf[num7] = tfArray1[num7];
            num7++;
            goto Label_19E0;
            Label_1ADA:
                if (num10 >= num2) {
                    return;
                }
            this.m_aptf[num10].X = tfArray1[num10].X;
            this.m_aptf[num10].Y = -tfArray1[num10].Y;
            num10++;
            goto Label_1ADA;
        }
 
        internal void UpdateLinkPoints(PointF pt, int nHandle) {
            switch (this.m_line.m_style) {
            case LineStyle.VH:
            case LineStyle.VHV:
            case LineStyle.VHVH:
            case LineStyle.VHVHV:
            case LineStyle.VHVHVH:
            case LineStyle.VHVHVHV: {
                if (this.m_aptf.Length > 3) {
                    if (nHandle != 0) {
                        if (nHandle == 1) {
                            this.m_aptf[1].Y = pt.Y;
                            this.m_aptf[2].Y = pt.Y;
                        }
                        else if (nHandle == (this.m_aptf.Length - 1)) {
                            if ((this.m_aptf.Length % 2) == 0) {
                                this.m_aptf[this.m_aptf.Length - 2].X = pt.X;
                            }
                            else {
                                this.m_aptf[this.m_aptf.Length - 2].Y = pt.Y;
                            }
                        }
                        else if (nHandle == (this.m_aptf.Length - 2)) {
                            if ((this.m_aptf.Length % 2) == 0) {
                                this.m_aptf[this.m_aptf.Length - 3].Y = pt.Y;
                                this.m_aptf[this.m_aptf.Length - 2].Y = pt.Y;
                            }
                            else {
                                this.m_aptf[this.m_aptf.Length - 3].X = pt.X;
                                this.m_aptf[this.m_aptf.Length - 2].X = pt.X;
                            }
                        }
                        else {
                            this.m_aptf[nHandle] = pt;
                            if ((nHandle % 2) == 0) {
                                this.m_aptf[nHandle - 1].Y = pt.Y;
                                this.m_aptf[nHandle + 1].X = pt.X;
                            }
                            else {
                                this.m_aptf[nHandle - 1].X = pt.X;
                                this.m_aptf[nHandle + 1].Y = pt.Y;
                            }
                        }
                        return;
                    }
                    this.m_aptf[1].X = pt.X;
                }
                return;
            }
            case LineStyle.HV:
            case LineStyle.HVH:
            case LineStyle.HVHV:
            case LineStyle.HVHVH:
            case LineStyle.HVHVHV:
            case LineStyle.HVHVHVH: {
                if (this.m_aptf.Length > 3) {
                    if (nHandle != 0) {
                        if (nHandle == 1) {
                            this.m_aptf[1].X = pt.X;
                            this.m_aptf[2].X = pt.X;
                            return;
                        }
                        if (nHandle == (this.m_aptf.Length - 1)) {
                            if ((this.m_aptf.Length % 2) == 0) {
                                this.m_aptf[this.m_aptf.Length - 2].Y = pt.Y;
                                return;
                            }
                            this.m_aptf[this.m_aptf.Length - 2].X = pt.X;
                            return;
                        }
                        if (nHandle == (this.m_aptf.Length - 2)) {
                            if ((this.m_aptf.Length % 2) == 0) {
                                this.m_aptf[this.m_aptf.Length - 3].X = pt.X;
                                this.m_aptf[this.m_aptf.Length - 2].X = pt.X;
                                return;
                            }
                            this.m_aptf[this.m_aptf.Length - 3].Y = pt.Y;
                            this.m_aptf[this.m_aptf.Length - 2].Y = pt.Y;
                            return;
                        }
                        this.m_aptf[nHandle] = pt;
                        if ((nHandle % 2) == 0) {
                            this.m_aptf[nHandle - 1].X = pt.X;
                            this.m_aptf[nHandle + 1].Y = pt.Y;
                            return;
                        }
                        this.m_aptf[nHandle - 1].Y = pt.Y;
                        this.m_aptf[nHandle + 1].X = pt.X;
                        return;
                    }
                    this.m_aptf[1].Y = pt.Y;
                }
                return;
            }
            }
        }
 
        internal void UpdateVHLink(PointF pt, int nHandle) {
            if (nHandle <= 1) {
                this.m_aptf[nHandle] = pt;
                if (nHandle == 1) {
                    switch (this.m_line.m_style) {
                    case LineStyle.VH: {
                        this.m_aptf[0].X = pt.X;
                        this.m_aptf[2].Y = pt.Y;
                        goto Label_01C9;
                    }
                    case LineStyle.HV: {
                        this.m_aptf[0].Y = pt.Y;
                        this.m_aptf[2].X = pt.X;
                        goto Label_01C9;
                    }
                    case LineStyle.VHV:
                    case LineStyle.VHVH:
                    case LineStyle.VHVHV:
                    case LineStyle.VHVHVH:
                    case LineStyle.VHVHVHV: {
                        this.m_aptf[0].X = pt.X;
                        goto Label_01C9;
                    }
                    case LineStyle.HVH:
                    case LineStyle.HVHV:
                    case LineStyle.HVHVH:
                    case LineStyle.HVHVHV:
                    case LineStyle.HVHVHVH: {
                        this.m_aptf[0].Y = pt.Y;
                        goto Label_01C9;
                    }
                    }
                }
            }
            else if (nHandle >= (this.m_aptf.Length - 2)) {
                this.m_aptf[nHandle] = pt;
                if (nHandle == (this.m_aptf.Length - 2)) {
                    switch (this.m_line.m_style) {
                    case LineStyle.VH:
                    case LineStyle.HVH:
                    case LineStyle.VHVH:
                    case LineStyle.HVHVH:
                    case LineStyle.VHVHVH:
                    case LineStyle.HVHVHVH: {
                        this.m_aptf[this.m_aptf.Length - 1].Y = pt.Y;
                        goto Label_01C9;
                    }
                    case LineStyle.HV:
                    case LineStyle.VHV:
                    case LineStyle.HVHV:
                    case LineStyle.VHVHV:
                    case LineStyle.HVHVHV:
                    case LineStyle.VHVHVHV: {
                        this.m_aptf[this.m_aptf.Length - 1].X = pt.X;
                        goto Label_01C9;
                    }
                    }
                }
            }
            Label_01C9:
                this.UpdateLinkPoints(pt, nHandle);
        }
 
        public bool AdjustDst {
            get {
                return this.m_adjustDst;
            }
            set {
                if (this.m_adjustDst != value) {
                    if (!base.Existing) {
                        this.m_adjustDst = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkAdjustDstTask(this));
                        }
                        this.m_adjustDst = value;
                        this.OnAdjustChanged();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public bool AdjustOrg {
            get {
                return this.m_adjustOrg;
            }
            set {
                if (this.m_adjustOrg != value) {
                    if (!base.Existing) {
                        this.m_adjustOrg = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkAdjustOrgTask(this));
                        }
                        this.m_adjustOrg = value;
                        this.OnAdjustChanged();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        [NotifyParentProperty(true)]
        public Arrow ArrowDst {
            get {
                return this.m_arrowDst;
            }
            set {
                if (!this.m_arrowDst.Equals(value)) {
                    if (!base.Existing) {
                        this.m_arrowDst = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkArrowDstTask(this));
                        }
                        this.SetArrowDst2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        [NotifyParentProperty(true)]
        public Arrow ArrowMid {
            get {
                return this.m_arrowMid;
            }
            set {
                if (!this.m_arrowMid.Equals(value)) {
                    if (!base.Existing) {
                        this.m_arrowMid = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkArrowMidTask(this));
                        }
                        this.SetArrowMid2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        [NotifyParentProperty(true)]
        public Arrow ArrowOrg {
            get {
                return this.m_arrowOrg;
            }
            set {
                if (!this.m_arrowOrg.Equals(value)) {
                    if (!base.Existing) {
                        this.m_arrowOrg = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkArrowOrgTask(this));
                        }
                        this.SetArrowOrg2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public CustomLineCap CustomEndCap {
            get {
                return this.m_customEndCap;
            }
            set {
                if (this.m_customEndCap != value) {
                    if (!base.Existing) {
                        this.m_customEndCap = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkCustomEndCapTask(this));
                        }
                        this.m_customEndCap = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public CustomLineCap CustomStartCap {
            get {
                return this.m_customStartCap;
            }
            set {
                if (this.m_customStartCap != value) {
                    if (!base.Existing) {
                        this.m_customStartCap = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkCustomStartCapTask(this));
                        }
                        this.m_customStartCap = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public Node Dst {
            get {
                return this.m_dst;
            }
            set {
                if (!base.Existing) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ItemMustBeInGraph));
                }
                Node node1 = value;
                if (node1 != this.m_dst) {
                    if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                        this.m_af.m_undo.SubmitTask(new SetLinkDstTask(this));
                    }
                    this.SetDst2(node1);
                    this.m_af.IncrementChangedFlag(1);
                }
            }
        }
 
        public LineCap EndCap {
            get {
                return this.m_endCap;
            }
            set {
                if (this.m_endCap != value) {
                    if (!base.Existing) {
                        this.m_endCap = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkEndCapTask(this));
                        }
                        this.m_endCap = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public Jump Jump {
            get {
                return this.m_jump;
            }
            set {
                if (this.m_jump != value) {
                    if (!base.Existing) {
                        this.m_jump = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkJumpTask(this));
                        }
                        this.m_jump = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        [NotifyParentProperty(true)]
        public Line Line {
            get {
                return this.m_line;
            }
            set {
                if (!this.m_line.Equals(value)) {
                    if (!base.Existing) {
                        this.m_line = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkLineTask(this));
                        }
                        this.SetLine2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public Node Org {
            get {
                return this.m_org;
            }
            set {
                if (!base.Existing) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ItemMustBeInGraph));
                }
                Node node1 = value;
                if (node1 != this.m_org) {
                    if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                        this.m_af.m_undo.SubmitTask(new SetLinkOrgTask(this));
                    }
                    this.SetOrg2(node1);
                    this.m_af.IncrementChangedFlag(1);
                }
            }
        }
 
        public bool OrientedText {
            get {
                return this.m_orientedText;
            }
            set {
                if (this.m_orientedText != value) {
                    if (!base.Existing) {
                        this.m_orientedText = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkFlagTask(this, Action.OrientedText));
                        }
                        this.SetOrientedText2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public LinkPointsCollection Points {
            get {
                if (!base.Existing) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ItemMustBeInGraph));
                }
                return new LinkPointsCollection(this);
            }
        }
 
        internal bool Reflexive {
            get {
                return (this.m_org == this.m_dst);
            }
        }
 
        public bool Rigid {
            get {
                return this.m_rigid;
            }
            set {
                if (this.m_rigid != value) {
                    if (!base.Existing) {
                        this.m_rigid = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkFlagTask(this, Action.Rigid));
                        }
                        this.m_rigid = value;
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public LineCap StartCap {
            get {
                return this.m_startCap;
            }
            set {
                if (this.m_startCap != value) {
                    if (!base.Existing) {
                        this.m_startCap = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkStartCapTask(this));
                        }
                        this.m_startCap = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public bool Stretchable {
            get {
                return this.m_stretchable;
            }
            set {
                if (this.m_stretchable != value) {
                    if (!base.Existing) {
                        this.m_stretchable = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkFlagTask(this, Action.Stretchable));
                        }
                        this.m_stretchable = value;
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public override string Text {
            get {
                return this.m_text;
            }
            set {
                if (this.m_text != value) {
                    if (!base.Existing) {
                        this.m_text = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkTextTask(this));
                        }
                        this.SetText2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public override string Tooltip {
            get {
                return this.m_tooltip;
            }
            set {
                if (this.m_tooltip != value) {
                    if (!base.Existing) {
                        this.m_tooltip = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new LinkTooltipTask(this));
                        }
                        this.m_tooltip = value;
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        internal bool m_adjustDst;
 
        internal bool m_adjustOrg;
 
        internal PointF[] m_aptf;
 
        internal PointF[] m_aptf2;
 
        internal Arrow m_arrowDst;
 
        internal Arrow m_arrowMid;
 
        internal Arrow m_arrowOrg;
 
        internal CustomLineCap m_customEndCap;
 
        internal CustomLineCap m_customStartCap;
 
        internal static float m_distance;
 
        internal Node m_dst;
 
        internal LineCap m_endCap;
 
        internal Jump m_jump;
 
        internal Line m_line;
 
        internal Node m_org;
 
        internal bool m_orientedText;
 
        internal RectangleF m_rcSeg;
 
        internal bool m_rigid;
 
        internal LineCap m_startCap;
 
        internal bool m_stretchable;
 
        private const int NbPtBezierHit = 11;
 
        private const int NbPtDyn = 0x10;
    }

    internal class LinkAdjustDstTask : Task {

        private LinkAdjustDstTask() {
        }
 
        internal LinkAdjustDstTask(Link link) {
            this.m_link = link;
            this.m_oldValue = this.m_link.m_adjustDst;
            int num1 = this.m_link.m_aptf.Length;
            this.m_pt = new PointF(this.m_link.m_aptf[num1 - 1].X, this.m_link.m_aptf[num1 - 1].Y);
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.AdjustDst;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            int num1 = this.m_link.m_aptf.Length;
            bool flag1 = this.m_link.m_adjustDst;
            PointF tf1 = this.m_link.m_aptf[num1 - 1];
            this.m_link.m_adjustDst = this.m_oldValue;
            this.m_link.m_aptf[num1 - 1] = this.m_pt;
            this.m_link.OnAdjustChanged();
            this.m_oldValue = flag1;
            this.m_pt = tf1;
        }
 
        private Link m_link;
 
        private bool m_oldValue;
 
        private PointF m_pt;
    }

    internal class LinkAdjustOrgTask : Task {

        private LinkAdjustOrgTask() {
        }
 
        internal LinkAdjustOrgTask(Link link) {
            this.m_link = link;
            this.m_oldValue = this.m_link.m_adjustOrg;
            this.m_pt = new PointF(this.m_link.m_aptf[0].X, this.m_link.m_aptf[0].Y);
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.AdjustOrg;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            bool flag1 = this.m_link.m_adjustOrg;
            PointF tf1 = this.m_link.m_aptf[0];
            this.m_link.m_adjustOrg = this.m_oldValue;
            this.m_link.m_aptf[0] = this.m_pt;
            this.m_link.OnAdjustChanged();
            this.m_oldValue = flag1;
            this.m_pt = tf1;
        }
 
        private Link m_link;
 
        private bool m_oldValue;
 
        private PointF m_pt;
    }

    internal class LinkArrowDstTask : Task {

        private LinkArrowDstTask() {
            this.m_oldArrow = null;
        }
 
        internal LinkArrowDstTask(Link link) {
            this.m_oldArrow = null;
            this.m_link = link;
            this.m_oldArrow = (Arrow) this.m_link.m_arrowDst.Clone();
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.ArrowDst;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Arrow arrow1 = (Arrow) this.m_link.m_arrowDst.Clone();
            this.m_link.SetArrowDst2(this.m_oldArrow);
            this.m_oldArrow = arrow1;
        }
 
        private Link m_link;
 
        private Arrow m_oldArrow;
    }

    internal class LinkArrowMidTask : Task {

        private LinkArrowMidTask() {
            this.m_oldArrow = null;
        }
 
        internal LinkArrowMidTask(Link link) {
            this.m_oldArrow = null;
            this.m_link = link;
            this.m_oldArrow = (Arrow) this.m_link.m_arrowMid.Clone();
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.ArrowMid;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Arrow arrow1 = (Arrow) this.m_link.m_arrowMid.Clone();
            this.m_link.SetArrowMid2(this.m_oldArrow);
            this.m_oldArrow = arrow1;
        }
 
        private Link m_link;
 
        private Arrow m_oldArrow;
    }

    internal class LinkArrowOrgTask : Task {

        private LinkArrowOrgTask() {
            this.m_oldArrow = null;
        }
 
        internal LinkArrowOrgTask(Link link) {
            this.m_oldArrow = null;
            this.m_link = link;
            this.m_oldArrow = (Arrow) this.m_link.m_arrowOrg.Clone();
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.ArrowOrg;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Arrow arrow1 = (Arrow) this.m_link.m_arrowOrg.Clone();
            this.m_link.SetArrowOrg2(this.m_oldArrow);
            this.m_oldArrow = arrow1;
        }
 
        private Link m_link;
 
        private Arrow m_oldArrow;
    }

    public enum LinkCreationMode {
        // Fields
        AllNodeArea = 0,
        MiddleHandle = 1
    }
 
    internal class LinkCustomEndCapTask : Task {

        private LinkCustomEndCapTask() {
        }
 
        internal LinkCustomEndCapTask(Link link) {
            this.m_link = link;
            this.m_oldCustomEndCap = this.m_link.m_customEndCap;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.CustomStartCap;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            CustomLineCap cap1 = this.m_link.m_customEndCap;
            this.m_link.m_customEndCap = this.m_oldCustomEndCap;
            this.m_link.InvalidateItem();
            this.m_oldCustomEndCap = cap1;
        }
 
        private Link m_link;
 
        private CustomLineCap m_oldCustomEndCap;
    }

    internal class LinkCustomStartCapTask : Task {

        private LinkCustomStartCapTask() {
        }
 
        internal LinkCustomStartCapTask(Link link) {
            this.m_link = link;
            this.m_oldCustomStartCap = this.m_link.m_customStartCap;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.CustomStartCap;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            CustomLineCap cap1 = this.m_link.m_customStartCap;
            this.m_link.m_customStartCap = this.m_oldCustomStartCap;
            this.m_link.InvalidateItem();
            this.m_oldCustomStartCap = cap1;
        }
 
        private Link m_link;
 
        private CustomLineCap m_oldCustomStartCap;
    }

    internal class LinkDashStyleTask : Task {

        private LinkDashStyleTask() {
        }
 
        internal LinkDashStyleTask(Link link) {
            this.m_link = link;
            this.m_oldDashStyle = this.m_link.m_dashStyle;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.DashStyle;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            DashStyle style1 = this.m_link.m_dashStyle;
            this.m_link.m_dashStyle = this.m_oldDashStyle;
            this.m_link.InvalidateItem();
            this.m_oldDashStyle = style1;
        }
 
        private Link m_link;
 
        private DashStyle m_oldDashStyle;
    }

    public enum LinkDrawFlags {
        // Fields
        All = 7,
        Arrows = 2,
        None = 0,
        Shape = 1,
        Text = 4
    }
 
    internal class LinkDrawWidthTask : Task {

        private LinkDrawWidthTask() {
        }
 
        internal LinkDrawWidthTask(Link link) {
            this.m_link = link;
            this.m_oldValue = this.m_link.m_drawWidth;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.DrawWidth;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            int num1 = this.m_link.m_drawWidth;
            this.m_link.SetDrawWidth2(this.m_oldValue);
            this.m_oldValue = num1;
        }
 
        private Link m_link;
 
        private int m_oldValue;
    }

    internal class LinkEndCapTask : Task {

        private LinkEndCapTask() {
        }
 
        internal LinkEndCapTask(Link link) {
            this.m_link = link;
            this.m_oldEndCap = this.m_link.m_endCap;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.EndCap;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            LineCap cap1 = this.m_link.m_endCap;
            this.m_link.m_endCap = this.m_oldEndCap;
            this.m_link.InvalidateItem();
            this.m_oldEndCap = cap1;
        }
 
        private Link m_link;
 
        private LineCap m_oldEndCap;
    }

    internal class LinkEnumerator : IEnumerator {

        internal LinkEnumerator(Node node, LinksCollection.Type lct) {
            int num1;
            int num3;
            this.m_aLinks = new ArrayList();
            this.m_node = node;
            this.m_type = lct;
            this.index = -1;
            switch (this.m_type) {
            case LinksCollection.Type.In: {
                for (int num2 = 0; num2 < this.m_node.m_aLinks.Count; num2++) {
                    Link link2 = (Link) this.m_node.m_aLinks[num2];
                    if (((!link2.Reflexive || (num2 >= (this.m_node.m_aLinks.Count - 1))) || (((Link) this.m_node.m_aLinks[num2 + 1]) != link2)) && (link2.m_dst == this.m_node)) {
                        this.m_aLinks.Add(link2);
                    }
                }
                return;
            }
            case LinksCollection.Type.Out: {
                num1 = 0;
                goto Label_00B4;
            }
            case LinksCollection.Type.InOut: {
                num3 = 0;
                goto Label_01C2;
            }
            default: {
                return;
            }
            }
            Label_00B4:
                if (num1 >= this.m_node.m_aLinks.Count) {
                    return;
                }
            Link link1 = (Link) this.m_node.m_aLinks[num1];
            if (((!link1.Reflexive || (num1 >= (this.m_node.m_aLinks.Count - 1))) || (((Link) this.m_node.m_aLinks[num1 + 1]) != link1)) && (link1.m_org == this.m_node)) {
                this.m_aLinks.Add(link1);
            }
            num1++;
            goto Label_00B4;
            Label_01C2:
                if (num3 >= this.m_node.m_aLinks.Count) {
                    return;
                }
            Link link3 = (Link) this.m_node.m_aLinks[num3];
            if ((!link3.Reflexive || (num3 >= (this.m_node.m_aLinks.Count - 1))) || (((Link) this.m_node.m_aLinks[num3 + 1]) != link3)) {
                this.m_aLinks.Add(link3);
            }
            num3++;
            goto Label_01C2;
        }
 
        public bool MoveNext() {
            this.index++;
            return (this.index != this.m_aLinks.Count);
        }
 
        public void Reset() {
            this.index = -1;
            this.m_aLinks.Clear();
        }
 
        public object Current {
            get {
                if ((this.index < 0) || (this.index == this.m_aLinks.Count)) {
                    throw new InvalidOperationException();
                }
                return this.m_aLinks[this.index];
            }
        }
 
        private int index;
 
        private ArrayList m_aLinks;
 
        private Node m_node;
 
        private LinksCollection.Type m_type;
    }

    internal class LinkFlagTask : Task {
 
        private LinkFlagTask() {
        }
 
        internal LinkFlagTask(Link link, Action code) {
            this.m_link = link;
            this.m_code = code;
            Action action1 = this.m_code;
            if (action1 != Action.OrientedText) {
                if (action1 == Action.Rigid) {
                    this.m_oldValue = this.m_link.m_rigid;
                }
                else if (action1 == Action.Stretchable) {
                    this.m_oldValue = this.m_link.m_stretchable;
                }
            }
            else {
                this.m_oldValue = this.m_link.m_orientedText;
            }
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return this.m_code;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            bool flag1 = false;
            Action action1 = this.m_code;
            if (action1 != Action.OrientedText) {
                if (action1 == Action.Rigid) {
                    flag1 = this.m_link.m_rigid;
                    this.m_link.m_rigid = this.m_oldValue;
                }
                else if (action1 != Action.Stretchable) {
                    flag1 = false;
                }
                else {
                    flag1 = this.m_link.m_stretchable;
                    this.m_link.m_stretchable = this.m_oldValue;
                }
            }
            else {
                flag1 = this.m_link.m_orientedText;
                this.m_link.SetOrientedText2(this.m_oldValue);
            }
            this.m_oldValue = flag1;
        }
 
        private Action m_code;
 
        private Link m_link;
 
        private bool m_oldValue;
    }

    internal class LinkFontTask : Task {

        private LinkFontTask() {
        }
 
        internal LinkFontTask(Link link) {
            this.m_link = link;
            this.m_oldFont = (Font) this.m_link.m_font.Clone();
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Font;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Font font1 = (Font) this.m_link.m_font.Clone();
            this.m_link.SetFont2(this.m_oldFont);
            this.m_oldFont = font1;
        }
 
        private Link m_link;
 
        private Font m_oldFont;
    }

    internal class LinkJumpTask : Task {

        private LinkJumpTask() {
        }
 
        internal LinkJumpTask(Link link) {
            this.m_link = link;
            this.m_oldjump = this.m_link.m_jump;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Jump;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Jump jump1 = this.m_link.m_jump;
            this.m_link.m_jump = this.m_oldjump;
            this.m_link.InvalidateItem();
            this.m_oldjump = jump1;
        }
 
        private Link m_link;
 
        private Jump m_oldjump;
    }

    internal class LinkLineTask : Task {

        private LinkLineTask() {
        }
 
        internal LinkLineTask(Link link) {
            this.m_link = link;
            this.m_oldLine = (Line) this.m_link.m_line.Clone();
            this.m_oldAptf = new PointF[this.m_link.m_aptf.Length];
            this.m_link.m_aptf.CopyTo(this.m_oldAptf, 0);
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Line;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Line line1 = (Line) this.m_link.m_line.Clone();
            PointF[] tfArray1 = new PointF[this.m_link.m_aptf.Length];
            this.m_link.m_aptf.CopyTo(tfArray1, 0);
            this.m_link.m_aptf = this.m_oldAptf;
            this.m_link.SetLine2(this.m_oldLine);
            if (this.m_link.m_line.HVStyle) {
                this.m_link.m_aptf = this.m_oldAptf;
            }
            this.m_link.Invalidate(true);
            this.m_oldAptf = tfArray1;
            this.m_oldLine = line1;
        }
 
        private Link m_link;
 
        private PointF[] m_oldAptf;
 
        private Line m_oldLine;
    }

    public class LinkOwnerDrawEventArgs : EventArgs {

        public LinkOwnerDrawEventArgs(Link link, Graphics graphics) {
            this.link = link;
            this.grfx = graphics;
            this.flags = LinkDrawFlags.All;
        }
 
        public LinkDrawFlags Flags {
            get {
                return this.flags;
            }
            set {
                this.flags = value;
            }
        }
 
        public Graphics Graphics {
            get {
                return this.grfx;
            }
        }
 
        public Link Link {
            get {
                return this.link;
            }
        }
 
        private LinkDrawFlags flags;
 
        private Graphics grfx;
 
        private Link link;
    }

    public class LinkPointsCollection : CollectionBase {
 
        private LinkPointsCollection() {
        }
 
        internal LinkPointsCollection(Link link) {
            this.m_link = link;
        }
 
        public void Add(PointF pt) {
            if (!this.m_link.m_line.NewPointsAllowed) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ChangePoints));
            }
            if ((pt.X < 0f) || (pt.Y < 0f)) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.OutOfAreaError));
            }
            this.m_link.Invalidate(false);
            if (this.m_link.m_af.m_undo.CanUndoRedo) {
                this.m_link.m_af.m_undo.SubmitTask(new StretchLinkTask(this.m_link));
            }
            this.m_link.m_aptf = this.m_link.InsertPoint(this.m_link.m_aptf.Length - 2, pt);
            this.m_link.Invalidate(true);
            if (this.m_link.m_af.GraphRect.Contains(pt)) {
                this.m_link.m_af.UpdateRect();
            }
        }
 
        public void CopyTo(Array array, int arrayIndex) {
            this.m_link.m_aptf.CopyTo(array, arrayIndex);
        }
 
        public IEnumerator GetEnumerator() {
            return this.m_link.m_aptf.GetEnumerator();
        }
 
        protected override void OnClear() {
            if (!this.m_link.m_line.NewPointsAllowed) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ChangePoints));
            }
            this.m_link.Invalidate(false);
            if (this.m_link.m_af.m_undo.CanUndoRedo) {
                this.m_link.m_af.m_undo.SubmitTask(new StretchLinkTask(this.m_link));
            }
            this.m_link.InitPoints();
            this.m_link.CalcRect();
            this.m_link.Invalidate(true);
            this.m_link.m_af.UpdateRect();
        }
 
        public void Remove(int index) {
            if (!this.m_link.m_line.NewPointsAllowed) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ChangePoints));
            }
            if (((this.m_link.m_aptf == null) || (index <= 0)) || (index >= (this.m_link.m_aptf.Length - 1))) {
                throw new IndexOutOfRangeException();
            }
            this.m_link.Invalidate(false);
            if (this.m_link.m_af.m_undo.CanUndoRedo) {
                this.m_link.m_af.m_undo.SubmitTask(new StretchLinkTask(this.m_link));
            }
            this.m_link.m_aptf = this.m_link.RemovePoint(index);
            this.m_link.Invalidate(true);
            this.m_link.m_af.UpdateRect();
        }
 
        public int Count {
            get {
                if (this.m_link.m_aptf == null) {
                    return 0;
                }
                return this.m_link.m_aptf.Length;
            }
        }
 
        public bool IsFixedSize {
            get {
                return false;
            }
        }
 
        public bool IsReadOnly {
            get {
                return false;
            }
        }
 
        public bool IsSynchronized {
            get {
                return this.m_link.m_aptf.IsSynchronized;
            }
        }
 
        public PointF this[int index] {
            get {
                if (((this.m_link.m_aptf == null) || (index < 0)) || (index > (this.m_link.m_aptf.Length - 1))) {
                    throw new IndexOutOfRangeException();
                }
                return this.m_link.m_aptf[index];
            }
            set {
                if (((this.m_link.m_aptf == null) || (index < 0)) || (index > (this.m_link.m_aptf.Length - 1))) {
                    throw new IndexOutOfRangeException();
                }
                if ((index == 0) && !this.m_link.m_adjustOrg) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.AdjustOrg));
                }
                if ((index == (this.m_link.m_aptf.Length - 1)) && !this.m_link.m_adjustDst) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.AdjustDst));
                }
                if (this.m_link.m_af.m_undo.CanUndoRedo) {
                    this.m_link.m_af.m_undo.SubmitTask(new LinkPointTask(this.m_link, index));
                }
                this.m_link.Invalidate(false);
                this.m_link.m_aptf[index] = value;
                this.m_link.Invalidate(true);
                if (this.m_link.m_af.GraphRect.Contains(this.m_link.m_aptf[index])) {
                    this.m_link.m_af.UpdateRect();
                }
            }
        }
 
        public object SyncRoot {
            get {
                return this.m_link.m_aptf.SyncRoot;
            }
        }
 
        private Link m_link;
    }

    internal class LinkPointTask : Task {

        private LinkPointTask() {
        }
 
        internal LinkPointTask(Link link, int idx) {
            this.m_link = link;
            this.m_idx = idx;
            this.m_oldPt = new PointF(this.m_link.m_aptf[this.m_idx].X, this.m_link.m_aptf[this.m_idx].Y);
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Points;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            this.m_link.Invalidate(true);
            PointF tf1 = this.m_link.m_aptf[this.m_idx];
            this.m_link.m_aptf[this.m_idx] = this.m_oldPt;
            this.m_oldPt = tf1;
            this.m_link.CalcRect();
            this.m_link.Invalidate(true);
        }
 
        private int m_idx;
 
        private Link m_link;
 
        private PointF m_oldPt;
    }

    internal class LinkPositionTask : Task {

        private LinkPositionTask() {
        }
 
        internal LinkPositionTask(Link link) {
            this.m_link = link;
            this.m_aptf = new PointF[link.m_aptf.Length];
            link.m_aptf.CopyTo(this.m_aptf, 0);
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.LinkMove;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            this.m_link.Invalidate(true);
            PointF[] tfArray1 = this.m_link.m_aptf;
            this.m_link.m_aptf = this.m_aptf;
            this.m_aptf = tfArray1;
            this.m_link.Invalidate(true);
        }
 
        private PointF[] m_aptf;
 
        private Link m_link;
    }

    public class LinksCollection : ICollection, IEnumerable {
 
        internal enum Type {
            // Fields
            In = 0,
            InOut = 2,
            Out = 1
        }
 
        private LinksCollection() {
        }
 
        internal LinksCollection(Node node, LinksCollection.Type lct) {
            this.m_node = node;
            this.m_type = lct;
        }
 
        public Link Add(Node node) {
            Link link1 = new Link(this.m_node.m_af.m_deflink);
            switch (this.m_type) {
            case LinksCollection.Type.In: {
                this.m_node.m_af.AddLink2(link1, node, this.m_node, false);
                return link1;
            }
            case LinksCollection.Type.Out: {
                this.m_node.m_af.AddLink2(link1, this.m_node, node, false);
                return link1;
            }
            case LinksCollection.Type.InOut: {
                this.m_node.m_af.AddLink2(link1, this.m_node, node, false);
                return link1;
            }
            }
            return link1;
        }
 
        public void Add(Link link, Node node) {
            if (link.Existing) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ItemAlreadyOwned));
            }
            switch (this.m_type) {
            case LinksCollection.Type.In: {
                this.m_node.m_af.AddLink2(link, node, this.m_node, false);
                return;
            }
            case LinksCollection.Type.Out: {
                this.m_node.m_af.AddLink2(link, this.m_node, node, false);
                return;
            }
            case LinksCollection.Type.InOut: {
                this.m_node.m_af.AddLink2(link, this.m_node, node, false);
                return;
            }
            }
        }
 
        public Link Add(Node node, string text) {
            Link link1 = new Link(text, this.m_node.m_af.m_deflink);
            switch (this.m_type) {
            case LinksCollection.Type.In: {
                this.m_node.m_af.AddLink2(link1, node, this.m_node, false);
                return link1;
            }
            case LinksCollection.Type.Out: {
                this.m_node.m_af.AddLink2(link1, this.m_node, node, false);
                return link1;
            }
            case LinksCollection.Type.InOut: {
                this.m_node.m_af.AddLink2(link1, this.m_node, node, false);
                return link1;
            }
            }
            return link1;
        }
 
        public void Clear() {
            this.m_node.DeleteLinks(this.m_type);
        }
 
        public void CopyTo(Array array, int arrayIndex) {
            switch (this.m_type) {
            case LinksCollection.Type.In: {
                ArrayList list2 = new ArrayList();
                foreach (Link link2 in this.m_node.m_aLinks) {
                    if (link2.m_dst == this.m_node) {
                        list2.Add(link2);
                    }
                }
                list2.CopyTo(array, arrayIndex);
                return;
            }
            case LinksCollection.Type.Out: {
                ArrayList list1 = new ArrayList();
                foreach (Link link1 in this.m_node.m_aLinks) {
                    if (link1.m_org == this.m_node) {
                        list1.Add(link1);
                    }
                }
                list1.CopyTo(array, arrayIndex);
                return;
            }
            case LinksCollection.Type.InOut: {
                this.m_node.m_aLinks.CopyTo(array, arrayIndex);
                return;
            }
            }
        }
 
        public IEnumerator GetEnumerator() {
            return new LinkEnumerator(this.m_node, this.m_type);
        }
 
        public void Remove(Link link) {
            this.m_node.m_af.RemoveLink(link);
            this.m_node.m_af.UpdateRect();
        }
 
        public int Count {
            get {
                int num1 = 0;
                switch (this.m_type) {
                case LinksCollection.Type.In: {
                    break;
                }
                case LinksCollection.Type.Out: {
                    foreach (Link link1 in this.m_node.m_aLinks) {
                        if (link1.m_org == this.m_node) {
                            num1++;
                        }
                    }
                    return num1;
                }
                case LinksCollection.Type.InOut: {
                    for (int num2 = 0; num2 < this.m_node.m_aLinks.Count; num2++) {
                        Link link3 = (Link) this.m_node.m_aLinks[num2];
                        if ((!link3.Reflexive || (num2 >= (this.m_node.m_aLinks.Count - 1))) || (((Link) this.m_node.m_aLinks[num2 + 1]) != link3)) {
                            num1++;
                        }
                    }
                    return num1;
                }
                default: {
                    return num1;
                }
                }
                foreach (Link link2 in this.m_node.m_aLinks) {
                    if (link2.m_dst == this.m_node) {
                        num1++;
                    }
                }
                return num1;
            }
        }
 
        public bool IsFixedSize {
            get {
                return false;
            }
        }
 
        public bool IsReadOnly {
            get {
                return false;
            }
        }
 
        public bool IsSynchronized {
            get {
                return this.m_node.m_aLinks.IsSynchronized;
            }
        }
 
        public Link this[int index] {
            get {
                int num2;
                int num4;
                int num1 = 0;
                Link link1 = null;
                switch (this.m_type) {
                case LinksCollection.Type.In: {
                    for (int num3 = 0; num3 < this.m_node.m_aLinks.Count; num3++) {
                        link1 = (Link) this.m_node.m_aLinks[num3];
                        if (((!link1.Reflexive || (num3 >= (this.m_node.m_aLinks.Count - 1))) || (((Link) this.m_node.m_aLinks[num3 + 1]) != link1)) && (link1.m_dst == this.m_node)) {
                            if (num1 == index) {
                                return link1;
                            }
                            num1++;
                        }
                    }
                    return link1;
                }
                case LinksCollection.Type.Out: {
                    num2 = 0;
                    goto Label_0094;
                }
                case LinksCollection.Type.InOut: {
                    num4 = 0;
                    goto Label_0196;
                }
                default: {
                    return link1;
                }
                }
                Label_0094:
                    if (num2 >= this.m_node.m_aLinks.Count) {
                        return link1;
                    }
                link1 = (Link) this.m_node.m_aLinks[num2];
                if (((!link1.Reflexive || (num2 >= (this.m_node.m_aLinks.Count - 1))) || (((Link) this.m_node.m_aLinks[num2 + 1]) != link1)) && (link1.m_org == this.m_node)) {
                    if (num1 == index) {
                        return link1;
                    }
                    num1++;
                }
                num2++;
                goto Label_0094;
                Label_0196:
                    if (num4 >= this.m_node.m_aLinks.Count) {
                        return link1;
                    }
                link1 = (Link) this.m_node.m_aLinks[num4];
                if ((!link1.Reflexive || (num4 >= (this.m_node.m_aLinks.Count - 1))) || (((Link) this.m_node.m_aLinks[num4 + 1]) != link1)) {
                    if (num1 == index) {
                        return link1;
                    }
                    num1++;
                }
                num4++;
                goto Label_0196;
            }
        }
 
        public object SyncRoot {
            get {
                return this.m_node.m_aLinks.SyncRoot;
            }
        }
 
        private Node m_node;
 
        private LinksCollection.Type m_type;
    }

    internal class LinkStartCapTask : Task {
        private LinkStartCapTask() {
        }
 
        internal LinkStartCapTask(Link link) {
            this.m_link = link;
            this.m_oldStartCap = this.m_link.m_startCap;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.StartCap;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            LineCap cap1 = this.m_link.m_startCap;
            this.m_link.m_startCap = this.m_oldStartCap;
            this.m_link.InvalidateItem();
            this.m_oldStartCap = cap1;
        }
 
        private Link m_link;
 
        private LineCap m_oldStartCap;
    }

    internal class LinkTextTask : Task {

        private LinkTextTask() {
        }
 
        internal LinkTextTask(Link link) {
            this.m_link = link;
            this.m_oldValue = this.m_link.m_text;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Text;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            string text1 = this.m_link.m_text;
            this.m_link.SetText2(this.m_oldValue);
            this.m_oldValue = text1;
        }
 
        private Link m_link;
 
        private string m_oldValue;
    }

    internal class LinkTooltipTask : Task {
        private LinkTooltipTask() {
        }
 
        internal LinkTooltipTask(Link link) {
            this.m_link = link;
            this.m_oldValue = this.m_link.m_tooltip;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Tooltip;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            string text1 = this.m_link.m_tooltip;
            this.m_link.m_tooltip = this.m_oldValue;
            this.m_oldValue = text1;
        }
 
        private Link m_link;
 
        private string m_oldValue;
    }

    internal class Misc {

        public Misc() {
        }
 
        private static int ccw(PointF p0, PointF p1, PointF p2) {
            float single1 = p1.X - p0.X;
            float single2 = p1.Y - p0.Y;
            float single3 = p2.X - p0.X;
            float single4 = p2.Y - p0.Y;
            if ((single1 * single4) > (single2 * single3)) {
                return 1;
            }
            if ((single1 * single4) < (single2 * single3)) {
                return -1;
            }
            if (((single1 * single3) < 0f) || ((single2 * single4) < 0f)) {
                return -1;
            }
            if (((single1 * single1) + (single2 * single2)) < ((single3 * single3) + (single4 * single4))) {
                return 1;
            }
            return 0;
        }
 
        internal static PointF CenterPoint(RectangleF rc) {
            return new PointF(rc.Left + (rc.Width / 2f), rc.Top + (rc.Height / 2f));
        }
 
        internal static float DistanceDeviceToWorld(Graphics grfx, float d) {
            PointF[] tfArray2 = new PointF[2] { new PointF(0f, 0f), new PointF(d, d) } ;
            PointF[] tfArray1 = tfArray2;
            grfx.TransformPoints(CoordinateSpace.World, CoordinateSpace.Device, tfArray1);
            return (float) Math.Sqrt((double) (((tfArray1[1].X - tfArray1[0].X) * (tfArray1[1].X - tfArray1[0].X)) + ((tfArray1[1].Y - tfArray1[0].Y) * (tfArray1[1].Y - tfArray1[0].Y))));
        }
 
        internal static PointF GetEllipseNearestPt(RectangleF rc, PointF pt, PointF ctr) {
            PointF tf1 = new PointF(ctr.X, ctr.Y);
            float single1 = rc.Width / 2f;
            float single2 = rc.Height / 2f;
            if ((single1 != 0f) && (single2 != 0f)) {
                float single3 = ctr.X - pt.X;
                float single4 = pt.Y - ctr.Y;
                float single5 = (float) Math.Sqrt((double) ((single3 * single3) + (single4 * single4)));
                if (single5 != 0f) {
                    float single6 = single3 / single5;
                    float single7 = single4 / single5;
                    single3 = single1 * single7;
                    single4 = single2 * single6;
                    single5 = (float) Math.Sqrt((double) ((single3 * single3) + (single4 * single4)));
                    if (single5 == 0f) {
                        return tf1;
                    }
                    float single8 = (single1 * single2) / single5;
                    tf1.X = ctr.X - (single8 * single6);
                    tf1.Y = ctr.Y + (single8 * single7);
                }
            }
            return tf1;
        }
 
        internal static PointF GetPolyNearestPt(PointF[] aptf, int nbPt, PointF pt, PointF ctr) {
            PointF tf1 = PointF.Empty;
            float single1 = 0f;
            for (int num1 = 0; num1 < nbPt; num1++) {
                float single2;
                PointF tf2 = PointF.Empty;
                PointF tf3 = aptf[num1];
                PointF tf4 = (num1 < (nbPt - 1)) ? aptf[num1 + 1] : aptf[0];
                if (tf3.X == tf4.X) {
                    if (Math.Abs((float) (ctr.X - pt.X)) < 0.001) {
                        goto Label_0288;
                    }
                    single2 = (pt.Y - ctr.Y) / (pt.X - ctr.X);
                    tf2.X = tf3.X;
                    tf2.Y = (single2 * (tf3.X - ctr.X)) + ctr.Y;
                }
                else {
                    float single3 = (tf4.Y - tf3.Y) / (tf4.X - tf3.X);
                    if (Math.Abs((float) (ctr.X - pt.X)) < 0.001) {
                        tf2.X = pt.X;
                        tf2.Y = (single3 * (pt.X - tf4.X)) + tf4.Y;
                    }
                    else {
                        single2 = (pt.Y - ctr.Y) / (pt.X - ctr.X);
                        if (Math.Abs((float) (single2 - single3)) < 0.001) {
                            goto Label_0288;
                        }
                        tf2.X = -(((tf4.Y - (single3 * tf4.X)) - ctr.Y) + (single2 * ctr.X)) / (single3 - single2);
                        tf2.Y = (single2 * (tf2.X - ctr.X)) + ctr.Y;
                    }
                }
                RectangleF ef1 = RectangleF.FromLTRB(Math.Min(tf3.X, tf4.X), Math.Min(tf3.Y, tf4.Y), Math.Max(tf3.X, tf4.X), Math.Max(tf3.Y, tf4.Y));
                ef1.Inflate(1f, 1f);
                if (ef1.Contains(tf2)) {
                    float single4 = tf2.X - pt.X;
                    float single5 = tf2.Y - pt.Y;
                    float single6 = (float) Math.Sqrt((double) ((single4 * single4) + (single5 * single5)));
                    if ((single1 == 0f) || (single6 < single1)) {
                        single1 = single6;
                        tf1 = tf2;
                    }
                }
            Label_0288:;
            }
            return tf1;
        }
 
        internal static float GetSegDist(PointF A, PointF B, PointF M) {
            float single2 = A.X - M.X;
            float single3 = A.Y - M.Y;
            float single4 = B.X - M.X;
            float single5 = B.Y - M.Y;
            float single6 = B.X - A.X;
            float single7 = B.Y - A.Y;
            float single8 = (single6 * single6) + (single7 * single7);
            float single9 = (single2 * single2) + (single3 * single3);
            float single10 = (single4 * single4) + (single5 * single5);
            float single11 = (float) Math.Sqrt((double) single8);
            if (single11 < 1f) {
                return (float) Math.Sqrt((double) single9);
            }
            float single1 = Math.Abs((float) ((single7 * single2) - (single6 * single3))) / single11;
            PointF tf1 = new PointF(M.X - ((int) ((single1 * single7) / single11)), M.Y + ((int) ((single1 * single6) / single11)));
            PointF tf2 = new PointF((B.X + A.X) / 2f, (B.Y + A.Y) / 2f);
            float single12 = ((tf1.X - tf2.X) * (tf1.X - tf2.X)) + ((tf1.Y - tf2.Y) * (tf1.Y - tf2.Y));
            if (single12 > (single8 / 4f)) {
                single1 = (float) Math.Sqrt((double) Math.Min(single9, single10));
            }
            return single1;
        }
 
        internal static bool InterPolylineRect(PointF[] apt, int nbPt, RectangleF rc) {
            bool flag1 = false;
            for (int num1 = 0; num1 < (nbPt - 1); num1++) {
                if (Misc.InterSegRect(apt[num1], apt[num1 + 1], rc)) {
                    flag1 = true;
                    break;
                }
            }
            return flag1;
        }
 
        internal static bool InterSeg(PointF s1p1, PointF s1p2, PointF s2p1, PointF s2p2) {
            if ((Misc.ccw(s1p1, s1p2, s2p1) * Misc.ccw(s1p1, s1p2, s2p2)) <= 0) {
                return ((Misc.ccw(s2p1, s2p2, s1p1) * Misc.ccw(s2p1, s2p2, s1p2)) <= 0);
            }
            return false;
        }
 
        private static bool InterSegRect(PointF p1, PointF p2, RectangleF rc) {
            PointF tf1 = new PointF();
            PointF tf2 = new PointF();
            tf1.X = rc.Left;
            tf1.Y = rc.Top;
            tf2.X = rc.Left;
            tf2.Y = rc.Bottom;
            if (Misc.InterSeg(p1, p2, tf1, tf2)) {
                return true;
            }
            tf1.X = rc.Right;
            tf1.Y = rc.Bottom;
            if (Misc.InterSeg(p1, p2, tf2, tf1)) {
                return true;
            }
            tf2.X = rc.Right;
            tf2.Y = rc.Top;
            if (Misc.InterSeg(p1, p2, tf1, tf2)) {
                return true;
            }
            tf1.X = rc.Left;
            tf1.Y = rc.Top;
            if (Misc.InterSeg(p1, p2, tf2, tf1)) {
                return true;
            }
            return false;
        }
 
        internal static PointF MiddlePoint(PointF pt1, PointF pt2) {
            return new PointF((pt1.X + pt2.X) / 2f, (pt1.Y + pt2.Y) / 2f);
        }
 
        internal static PointF PointDeviceToWorld(Graphics grfx, PointF pt) {
            PointF[] tfArray2 = new PointF[1] { new PointF(pt.X, pt.Y) } ;
            PointF[] tfArray1 = tfArray2;
            grfx.TransformPoints(CoordinateSpace.World, CoordinateSpace.Device, tfArray1);
            return tfArray1[0];
        }
 
        internal static PointF PointWorldToDevice(Graphics grfx, PointF pt) {
            PointF[] tfArray2 = new PointF[1] { new PointF(pt.X, pt.Y) } ;
            PointF[] tfArray1 = tfArray2;
            grfx.TransformPoints(CoordinateSpace.Device, CoordinateSpace.World, tfArray1);
            return tfArray1[0];
        }
 
        internal static RectangleF Rect(PointF ptBeg, PointF ptEnd) {
            return new RectangleF(Math.Min(ptBeg.X, ptEnd.X), Math.Min(ptBeg.Y, ptEnd.Y), Math.Abs((float) (ptEnd.X - ptBeg.X)), Math.Abs((float) (ptEnd.Y - ptBeg.Y)));
        }
 
        internal static Rectangle RectDeviceToWorld(Graphics grfx, Rectangle rc) {
            Point[] pointArray2 = new Point[2] { new Point(rc.Left, rc.Top), new Point(rc.Right, rc.Bottom) } ;
            Point[] pointArray1 = pointArray2;
            grfx.TransformPoints(CoordinateSpace.World, CoordinateSpace.Device, pointArray1);
            return Rectangle.FromLTRB(pointArray1[0].X, pointArray1[0].Y, pointArray1[1].X, pointArray1[1].Y);
        }
 
        internal static RectangleF RectDeviceToWorld(Graphics grfx, RectangleF rc) {
            PointF[] tfArray2 = new PointF[2] { new PointF(rc.Left, rc.Top), new PointF(rc.Right, rc.Bottom) } ;
            PointF[] tfArray1 = tfArray2;
            grfx.TransformPoints(CoordinateSpace.World, CoordinateSpace.Device, tfArray1);
            return RectangleF.FromLTRB(tfArray1[0].X, tfArray1[0].Y, tfArray1[1].X, tfArray1[1].Y);
        }
 
        internal static Rectangle RectPageToDevice(Graphics grfx, Rectangle rc) {
            Point[] pointArray2 = new Point[2] { new Point(rc.Left, rc.Top), new Point(rc.Right, rc.Bottom) } ;
            Point[] pointArray1 = pointArray2;
            grfx.TransformPoints(CoordinateSpace.Device, CoordinateSpace.Page, pointArray1);
            return Rectangle.FromLTRB(pointArray1[0].X, pointArray1[0].Y, pointArray1[1].X, pointArray1[1].Y);
        }
 
        internal static Rectangle RectWorldToDevice(Graphics grfx, Rectangle rc) {
            Point[] pointArray2 = new Point[2] { new Point(rc.Left, rc.Top), new Point(rc.Right, rc.Bottom) } ;
            Point[] pointArray1 = pointArray2;
            grfx.TransformPoints(CoordinateSpace.Device, CoordinateSpace.World, pointArray1);
            return Rectangle.FromLTRB(pointArray1[0].X, pointArray1[0].Y, pointArray1[1].X, pointArray1[1].Y);
        }
 
        internal static RectangleF RectWorldToDevice(Graphics grfx, RectangleF rc) {
            PointF[] tfArray2 = new PointF[2] { new PointF(rc.Left, rc.Top), new PointF(rc.Right, rc.Bottom) } ;
            PointF[] tfArray1 = tfArray2;
            grfx.TransformPoints(CoordinateSpace.Device, CoordinateSpace.World, tfArray1);
            return RectangleF.FromLTRB(tfArray1[0].X, tfArray1[0].Y, tfArray1[1].X, tfArray1[1].Y);
        }
 
        internal static Size SizeDeviceToPage(Graphics grfx, Size size) {
            Point[] pointArray2 = new Point[2] { new Point(0, 0), new Point(size.Width, size.Height) } ;
            Point[] pointArray1 = pointArray2;
            grfx.TransformPoints(CoordinateSpace.Page, CoordinateSpace.Device, pointArray1);
            return new Size(pointArray1[1].X - pointArray1[0].X, pointArray1[1].Y - pointArray1[0].Y);
        }
 
        internal static SizeF SizeDeviceToPage(Graphics grfx, SizeF sizef) {
            PointF[] tfArray2 = new PointF[2] { new PointF(0f, 0f), new PointF(sizef.Width, sizef.Height) } ;
            PointF[] tfArray1 = tfArray2;
            grfx.TransformPoints(CoordinateSpace.Page, CoordinateSpace.Device, tfArray1);
            return new SizeF(tfArray1[1].X - tfArray1[0].X, tfArray1[1].Y - tfArray1[0].Y);
        }
 
        internal static SizeF SizeDeviceToWorld(Graphics grfx, SizeF sizef) {
            PointF[] tfArray2 = new PointF[2] { new PointF(0f, 0f), new PointF(sizef.Width, sizef.Height) } ;
            PointF[] tfArray1 = tfArray2;
            grfx.TransformPoints(CoordinateSpace.World, CoordinateSpace.Device, tfArray1);
            return new SizeF(tfArray1[1].X - tfArray1[0].X, tfArray1[1].Y - tfArray1[0].Y);
        }
 
        internal static Size SizePageToDevice(Graphics grfx, Size size) {
            Point[] pointArray2 = new Point[2] { new Point(0, 0), new Point(size.Width, size.Height) } ;
            Point[] pointArray1 = pointArray2;
            grfx.TransformPoints(CoordinateSpace.Device, CoordinateSpace.Page, pointArray1);
            return new Size(pointArray1[1].X - pointArray1[0].X, pointArray1[1].Y - pointArray1[0].Y);
        }
 
        internal static Size SizeWorldToDevice(Graphics grfx, Size size) {
            Point[] pointArray2 = new Point[2] { new Point(0, 0), new Point(size.Width, size.Height) } ;
            Point[] pointArray1 = pointArray2;
            grfx.TransformPoints(CoordinateSpace.Device, CoordinateSpace.World, pointArray1);
            return new Size(pointArray1[1].X - pointArray1[0].X, pointArray1[1].Y - pointArray1[0].Y);
        }
 
        internal static SizeF SizeWorldToDevice(Graphics grfx, SizeF sizef) {
            PointF[] tfArray2 = new PointF[2] { new PointF(0f, 0f), new PointF(sizef.Width, sizef.Height) } ;
            PointF[] tfArray1 = tfArray2;
            grfx.TransformPoints(CoordinateSpace.Device, CoordinateSpace.World, tfArray1);
            return new SizeF(tfArray1[1].X - tfArray1[0].X, tfArray1[1].Y - tfArray1[0].Y);
        }
    }

    public enum MouseAction {
        // Fields
        DrawRectangle = 0,
        None = 1,
        Selection = 2,
        Selection2 = 3,
        Zoom = 4,
        ZoomIsotropic = 5
    }
 
    public enum MouseArea {
        // Fields
        DownSquare = 7,
        LeftDownSquare = 6,
        LeftSquare = 4,
        LeftUpSquare = 1,
        Link = 12,
        LinkSquare = 10,
        Node = 13,
        NodeDragFrame = 11,
        OutSide = 0,
        RightDownSquare = 8,
        RightSquare = 5,
        RightUpSquare = 3,
        StretchSquare = 9,
        UpSquare = 2
    }
 
    public class Node : Item, ICloneable {

        public Node() {
            this.Init();
        }
 
        public Node(DefNode defnode) {
            this.InitWithDefNode(defnode);
        }
 
        public Node(RectangleF rectangle) {
            if (((rectangle.Left < 0f) || (rectangle.Top < 0f)) || ((rectangle.Width < 0f) || (rectangle.Height < 0f))) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.OutOfAreaError));
            }
            this.m_rc = new RectangleF(rectangle.Location, rectangle.Size);
            this.Init();
        }
 
        public Node(RectangleF rectangle, DefNode defnode) {
            if (((rectangle.Left < 0f) || (rectangle.Top < 0f)) || ((rectangle.Width < 0f) || (rectangle.Height < 0f))) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.OutOfAreaError));
            }
            this.m_rc = new RectangleF(rectangle.Location, rectangle.Size);
            this.InitWithDefNode(defnode);
        }
 
        public Node(float left, float top, float width, float height) {
            if (((left < 0f) || (top < 0f)) || ((width < 0f) || (height < 0f))) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.OutOfAreaError));
            }
            this.m_rc = new RectangleF(left, top, width, height);
            this.Init();
        }
 
        public Node(float left, float top, float width, float height, DefNode defnode) {
            if (((left < 0f) || (top < 0f)) || ((width < 0f) || (height < 0f))) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.OutOfAreaError));
            }
            this.m_rc = new RectangleF(left, top, width, height);
            this.InitWithDefNode(defnode);
        }
 
        public Node(float left, float top, float width, float height, string text) {
            if (((left < 0f) || (top < 0f)) || ((width < 0f) || (height < 0f))) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.OutOfAreaError));
            }
            this.m_rc = new RectangleF(left, top, width, height);
            this.Init();
            this.m_text = text;
        }
 
        public Node(float left, float top, float width, float height, string text, DefNode defnode) {
            if (((left < 0f) || (top < 0f)) || ((width < 0f) || (height < 0f))) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.OutOfAreaError));
            }
            this.m_rc = new RectangleF(left, top, width, height);
            this.InitWithDefNode(defnode);
            this.m_text = text;
        }
 
        internal void AdjustNodeLinks() {
            foreach (Link link1 in this.m_aLinks) {
                link1.Adjust(this);
                link1.CalcRect();
            }
        }
 
        internal void AdjustNodeSize() {
            switch (this.m_autoSize) {
            case AutoSize.NodeToImage: {
                this.AdjustNodeSizeToImage();
                return;
            }
            case AutoSize.NodeToText: {
                this.AdjustNodeSizeToText();
                return;
            }
            }
        }
 
        internal void AdjustNodeSizeToImage() {
            Image image1 = this.GetImage(this.m_imageIndex);
            if (image1 != null) {
                SizeF ef1 = (SizeF) image1.Size;
                float single1 = (this.m_drawWidth == 0) ? ((float) 1) : ((float) this.m_drawWidth);
                Graphics graphics1 = this.m_af.CreateGraphics();
                this.m_af.CoordinatesDeviceToWorld(graphics1);
                float single2 = Misc.DistanceDeviceToWorld(graphics1, single1);
                graphics1.Dispose();
                ef1.Width += single2;
                ef1.Height += single2;
                RectangleF ef2 = this.m_rc;
                ef2.Size = ef1;
                this.m_rc = ef2;
            }
        }
 
        internal void AdjustNodeSizeToText() {
            if ((this.m_text != null) && (this.m_text.Length > 0)) {
                Graphics graphics1 = this.m_af.CreateGraphics();
                this.m_af.CoordinatesDeviceToWorld(graphics1);
                SizeF ef1 = graphics1.MeasureString(this.m_text, this.m_font, new PointF(0f, 0f), this.GetFormatDT());
                float single1 = (this.m_drawWidth == 0) ? ((float) 1) : ((float) this.m_drawWidth);
                this.m_af.CoordinatesDeviceToWorld(graphics1);
                float single2 = Misc.DistanceDeviceToWorld(graphics1, single1);
                graphics1.Dispose();
                ef1.Width += single2;
                ef1.Height += single2;
                RectangleF ef2 = new RectangleF(base.RC.Location, ef1);
                float single3 = (this.m_textMargin.Width < 50) ? ((this.m_textMargin.Width * ef2.Width) / ((float) (100 - (2 * this.m_textMargin.Width)))) : (ef2.Width / 2f);
                float single4 = (this.m_textMargin.Height < 50) ? ((this.m_textMargin.Height * ef2.Height) / ((float) (100 - (2 * this.m_textMargin.Height)))) : (ef2.Height / 2f);
                ef2.Width += (2f * single3);
                ef2.Height += (2f * single4);
                this.m_rc = ef2;
            }
        }
 
        public void BeginEdit() {
            if (base.Existing && !this.m_af.m_inPlaceEdition) {
                this.m_af.StartEdition(this);
            }
        }
 
        public object Clone() {
            Node node1 = new Node();
            node1.CloneProperties(this);
            return node1;
        }
 
        internal void CloneProperties(Node node) {
            this.m_rc = new RectangleF(node.RC.Location, node.RC.Size);
            this.m_hidden = node.m_hidden;
            this.m_transparent = node.m_transparent;
            this.m_gradient = node.m_gradient;
            this.m_labelEdit = node.m_labelEdit;
            this.m_shape = (Shape) node.m_shape.Clone();
            this.m_shadow = node.m_shadow;
            this.m_dashStyle = node.m_dashStyle;
            this.m_drawWidth = node.m_drawWidth;
            this.m_autoSize = node.m_autoSize;
            this.m_alignment = node.m_alignment;
            this.m_imagePosition = node.m_imagePosition;
            this.m_trimming = node.m_trimming;
            this.m_drawColor = node.m_drawColor;
            this.m_textColor = node.m_textColor;
            this.m_fillColor = node.m_fillColor;
            this.m_gradientColor = node.m_gradientColor;
            this.m_gradientMode = node.m_gradientMode;
            this.m_font = node.m_font;
            this.m_text = node.m_text;
            this.m_tooltip = node.m_tooltip;
            this.m_tag = node.m_tag;
            this.m_imageIndex = node.m_imageIndex;
            this.m_imageLocation = node.m_imageLocation;
            this.m_xMoveable = node.m_xMoveable;
            this.m_yMoveable = node.m_yMoveable;
            this.m_xSizeable = node.m_xSizeable;
            this.m_ySizeable = node.m_ySizeable;
            this.m_inLinkable = node.m_inLinkable;
            this.m_outLinkable = node.m_outLinkable;
            this.m_selectable = node.m_selectable;
            this.m_logical = node.m_logical;
            this.m_textMargin = node.m_textMargin;
        }
 
        internal void DeleteLinks(LinksCollection.Type type) {
            if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.IsCurrentActionGroup()) {
                this.m_af.m_undo.BeginActionInternal(Action.ClearNodes);
            }
            for (int num1 = this.m_aLinks.Count - 1; num1 >= 0; num1--) {
                Link link1 = (Link) this.m_aLinks[num1];
                if (link1.Reflexive && !link1.m_flag) {
                    link1.m_flag = true;
                }
                else {
                    switch (type) {
                    case LinksCollection.Type.In: {
                        break;
                    }
                    case LinksCollection.Type.Out: {
                        if (link1.m_org == this) {
                            this.m_af.RemoveLink(link1);
                        }
                        goto Label_00C0;
                    }
                    case LinksCollection.Type.InOut: {
                        this.m_af.RemoveLink(link1);
                        goto Label_00C0;
                    }
                    default: {
                        goto Label_00C0;
                    }
                    }
                    if (link1.m_dst == this) {
                        this.m_af.RemoveLink(link1);
                    }
                }
            Label_00C0:;
            }
            if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.IsCurrentActionGroup()) {
                this.m_af.m_undo.EndActionInternal();
                this.m_af.IncrementChangedFlag(1);
            }
        }
 
        internal override void Draw(Graphics grfx) {
            bool flag1;
            NodeDrawFlags flags1 = this.m_ownerDraw ? this.m_af.GetNodeDrawFlags(grfx, this) : NodeDrawFlags.All;
            if (flags1 == NodeDrawFlags.None) {
                return;
            }
            GraphicsPath path1 = this.m_shape.GetPathOfShape(this.m_rc);
            if (((((flags1 & NodeDrawFlags.Shadow) == NodeDrawFlags.Shadow) && (this.m_shadow.m_style != ShadowStyle.None)) && !this.m_transparent) && ((this.m_shape.m_style != ShapeStyle.Custom) || (this.m_shape.m_customGraphicsPath != null))) {
                this.m_shadow.Draw(grfx, path1);
            }
            if ((((flags1 & NodeDrawFlags.Fill) == NodeDrawFlags.Fill) && (path1 != null)) && ((this.m_shape.m_style != ShapeStyle.Custom) || (this.m_shape.m_customGraphicsPath != null))) {
                if (this.m_transparent) {
                    grfx.FillPath(Brushes.Transparent, path1);
                }
                else if (!this.m_gradient) {
                    grfx.FillPath(new SolidBrush(this.m_fillColor), path1);
                }
                else {
                    LinearGradientBrush brush1 = new LinearGradientBrush(this.m_rc, this.m_fillColor, this.m_gradientColor, this.m_gradientMode);
                    grfx.FillPath(brush1, path1);
                }
            }
            float single1 = (this.m_drawWidth == 0) ? ((float) 1) : ((float) this.m_drawWidth);
            float single2 = Misc.DistanceDeviceToWorld(grfx, single1);
            if (((flags1 & NodeDrawFlags.Shape) == NodeDrawFlags.Shape) && (path1 != null)) {
                Rectangle rectangle1;
                Border3DStyle style1;
                Pen pen1 = new Pen(this.m_drawColor);
                pen1.Width = single2;
                pen1.DashStyle = this.m_dashStyle;
                switch (this.m_shape.m_style) {
                case ShapeStyle.RectEdgeBump:
                case ShapeStyle.RectEdgeEtched:
                case ShapeStyle.RectEdgeRaised:
                case ShapeStyle.RectEdgeSunken: {
                    rectangle1 = Rectangle.Round(base.RC);
                    rectangle1 = Misc.RectWorldToDevice(grfx, rectangle1);
                    style1 = Border3DStyle.Bump;
                    switch (this.m_shape.m_style) {
                    case ShapeStyle.RectEdgeBump: {
                        style1 = Border3DStyle.Bump;
                        break;
                    }
                    case ShapeStyle.RectEdgeEtched: {
                        style1 = Border3DStyle.Etched;
                        break;
                    }
                    case ShapeStyle.RectEdgeRaised: {
                        style1 = Border3DStyle.Raised;
                        break;
                    }
                    case ShapeStyle.RectEdgeSunken: {
                        style1 = Border3DStyle.Sunken;
                        break;
                    }
                    }
                    break;
                }
                default: {
                    grfx.DrawPath(pen1, path1);
                    goto Label_01F4;
                }
                }
                GraphicsState state1 = grfx.Save();
                this.m_af.CoordinatesWorldToDevice(grfx);
                ControlPaint.DrawBorder3D(grfx, rectangle1, style1);
                grfx.Restore(state1);
            }
            Label_01F4:
                flag1 = ((this.m_text != null) && (this.m_text.Length > 0)) && ((flags1 & NodeDrawFlags.Text) == NodeDrawFlags.Text);
            Image image1 = this.GetImage(this.m_imageIndex);
            bool flag2 = (image1 != null) && ((flags1 & NodeDrawFlags.Image) == NodeDrawFlags.Image);
            if (flag1 || flag2) {
                RectangleF ef1 = base.RC;
                ef1.Inflate(-single2 / 2f, -single2 / 2f);
                RectangleF ef2 = RectangleF.Empty;
                RectangleF ef3 = RectangleF.Empty;
                if (flag2 && flag1) {
                    this.GetTextAndImageRectangles(grfx, ref ef2, ref ef3, ef1, image1);
                }
                else if (flag2) {
                    ef3 = this.GetImageRectangle(grfx, ef1, image1);
                }
                else if (flag1) {
                    ef2 = this.GetTextRectangle(grfx, ef1);
                }
                GraphicsState state2 = grfx.Save();
                grfx.SetClip(ef1);
                if (flag2) {
                    if ((this.m_af.m_pageUnit == GraphicsUnit.Pixel) && (this.m_af.m_pageScale == 1f)) {
                        grfx.DrawImage(image1, Rectangle.Round(ef3));
                    }
                    else {
                        grfx.DrawImage(image1, ef3);
                    }
                }
                if (flag1) {
                    StringFormat format1 = this.GetFormatDT();
                    if (format1.Trimming != StringTrimming.None) {
                        float single3 = this.m_font.GetHeight(grfx);
                        ef2.Height = ((int) (ef2.Height / single3)) * single3;
                    }
                    BackMode mode1 = this.m_backMode;
                    grfx.DrawString(this.m_text, this.m_font, new SolidBrush(this.m_textColor), ef2, format1);
                }
                grfx.Restore(state2);
            }
        }
 
        internal void DrawSelectionFrame(Graphics grfx) {
            RectangleF ef1 = Misc.RectWorldToDevice(grfx, this.m_rc);
            Rectangle rectangle1 = Rectangle.Round(ef1);
            Rectangle rectangle2 = new Rectangle(rectangle1.Location, rectangle1.Size);
            rectangle2.Inflate(this.m_af.m_selGrabSize, this.m_af.m_selGrabSize);
            this.m_af.CoordinatesWorldToDevice(grfx);
            ControlPaint.DrawSelectionFrame(grfx, false, rectangle2, rectangle1, this.m_af.BackColor);
            this.m_af.CoordinatesDeviceToWorld(grfx);
        }
 
        public void EndEdit(bool cancel) {
            if (base.Existing) {
                this.m_af.StopEdition(this, !cancel);
            }
        }
 
        private StringFormat GetFormatDT() {
            StringFormat format1 = new StringFormat();
            switch (this.m_alignment) {
            case Alignment.LeftJustifyTOP: {
                format1.Alignment = StringAlignment.Near;
                format1.LineAlignment = StringAlignment.Near;
                break;
            }
            case Alignment.LeftJustifyMIDDLE: {
                format1.Alignment = StringAlignment.Near;
                format1.LineAlignment = StringAlignment.Center;
                break;
            }
            case Alignment.LeftJustifyBOTTOM: {
                format1.Alignment = StringAlignment.Near;
                format1.LineAlignment = StringAlignment.Far;
                break;
            }
            case Alignment.RightJustifyTOP: {
                format1.Alignment = StringAlignment.Far;
                format1.LineAlignment = StringAlignment.Near;
                break;
            }
            case Alignment.RightJustifyMIDDLE: {
                format1.Alignment = StringAlignment.Far;
                format1.LineAlignment = StringAlignment.Center;
                break;
            }
            case Alignment.RightJustifyBOTTOM: {
                format1.Alignment = StringAlignment.Far;
                format1.LineAlignment = StringAlignment.Far;
                break;
            }
            case Alignment.CenterTOP: {
                format1.Alignment = StringAlignment.Center;
                format1.LineAlignment = StringAlignment.Near;
                break;
            }
            case Alignment.CenterMIDDLE: {
                format1.Alignment = StringAlignment.Center;
                format1.LineAlignment = StringAlignment.Center;
                break;
            }
            case Alignment.CenterBOTTOM: {
                format1.Alignment = StringAlignment.Center;
                format1.LineAlignment = StringAlignment.Far;
                break;
            }
            default: {
                format1.Alignment = StringAlignment.Center;
                format1.LineAlignment = StringAlignment.Center;
                break;
            }
            }
            format1.Trimming = this.m_trimming;
            return format1;
        }
 
        internal PointF[] GetHandlePoints(Graphics grfx) {
            SizeF ef1 = new SizeF((float) this.m_af.m_selGrabSize, (float) this.m_af.m_selGrabSize);
            ef1 = Misc.SizeDeviceToWorld(grfx, ef1);
            PointF[] tfArray1 = new PointF[9];
            RectangleF ef2 = new RectangleF(base.RC.Location, base.RC.Size);
            ef2.Inflate(ef1.Width / 2f, ef1.Height / 2f);
            tfArray1[0].X = ef2.Left;
            tfArray1[0].Y = ef2.Top;
            tfArray1[1].X = (ef2.Left + ef2.Right) / 2f;
            tfArray1[1].Y = ef2.Top;
            tfArray1[2].X = ef2.Right;
            tfArray1[2].Y = ef2.Top;
            tfArray1[3].X = ef2.Left;
            tfArray1[3].Y = (ef2.Top + ef2.Bottom) / 2f;
            tfArray1[4].X = ef2.Right;
            tfArray1[4].Y = (ef2.Top + ef2.Bottom) / 2f;
            tfArray1[5].X = ef2.Left;
            tfArray1[5].Y = ef2.Bottom;
            tfArray1[6].X = (ef2.Left + ef2.Right) / 2f;
            tfArray1[6].Y = ef2.Bottom;
            tfArray1[7].X = ef2.Right;
            tfArray1[7].Y = ef2.Bottom;
            tfArray1[8].X = (ef2.Left + ef2.Right) / 2f;
            tfArray1[8].Y = (ef2.Top + ef2.Bottom) / 2f;
            grfx.TransformPoints(CoordinateSpace.Device, CoordinateSpace.World, tfArray1);
            return tfArray1;
        }
 
        private Image GetImage(int imageIndex) {
            Image image1 = null;
            if ((imageIndex >= 0) && (imageIndex < this.m_af.m_images.Count)) {
                image1 = this.m_af.m_images[imageIndex].Image;
            }
            return image1;
        }
 
        internal RectangleF GetImageRectangle(Graphics grfx, RectangleF rcClip, Image image) {
            RectangleF ef1 = rcClip;
            switch (this.m_autoSize) {
            case AutoSize.None:
            case AutoSize.NodeToText: {
                SizeF ef2 = new SizeF((float) image.Size.Width, (float) image.Size.Height);
                ef2 = Misc.SizeDeviceToPage(grfx, ef2);
                ef1.Width = ef2.Width;
                ef1.Height = ef2.Height;
                switch (this.m_imagePosition) {
                case ImagePosition.LeftTop: {
                    return ef1;
                }
                case ImagePosition.LeftMiddle: {
                    ef1.Y += ((rcClip.Height / 2f) - (ef1.Height / 2f));
                    return ef1;
                }
                case ImagePosition.LeftBottom: {
                    ef1.Y += (rcClip.Height - ef1.Height);
                    return ef1;
                }
                case ImagePosition.RightTop: {
                    ef1.X += (rcClip.Width - ef1.Width);
                    return ef1;
                }
                case ImagePosition.RightMiddle: {
                    ef1.X += (rcClip.Width - ef1.Width);
                    ef1.Y += ((rcClip.Height / 2f) - (ef1.Height / 2f));
                    return ef1;
                }
                case ImagePosition.RightBottom: {
                    ef1.X += (rcClip.Width - ef1.Width);
                    ef1.Y += (rcClip.Height - ef1.Height);
                    return ef1;
                }
                case ImagePosition.CenterTop: {
                    ef1.X += ((rcClip.Width / 2f) - (ef1.Width / 2f));
                    return ef1;
                }
                case ImagePosition.CenterMiddle: {
                    ef1.X += ((rcClip.Width / 2f) - (ef1.Width / 2f));
                    ef1.Y += ((rcClip.Height / 2f) - (ef1.Height / 2f));
                    return ef1;
                }
                case ImagePosition.CenterBottom: {
                    ef1.X += ((rcClip.Width / 2f) - (ef1.Width / 2f));
                    ef1.Y += (rcClip.Height - ef1.Height);
                    return ef1;
                }
                case ImagePosition.RelativeToText: {
                    ef1.X += ((rcClip.Width / 2f) - (ef1.Width / 2f));
                    ef1.Y += ((rcClip.Height / 2f) - (ef1.Height / 2f));
                    ef1.Y = Math.Min(Math.Max(ef1.Y, rcClip.Top), rcClip.Bottom);
                    return ef1;
                }
                case ImagePosition.Custom: {
                    ef1.X = this.m_imageLocation.X + base.RC.Left;
                    ef1.Y = this.m_imageLocation.Y + base.RC.Top;
                    return ef1;
                }
                }
                return ef1;
            }
            case AutoSize.ImageToNode:
            case AutoSize.NodeToImage: {
                return ef1;
            }
            }
            return ef1;
        }
 
        public Node GetLinkedNode(Link link) {
            Node node1 = null;
            if (link != null) {
                if (link.m_org == this) {
                    return link.m_dst;
                }
                if (link.m_dst == this) {
                    node1 = link.m_org;
                }
            }
            return node1;
        }
 
        public GraphicsPath GetPath() {
            return this.m_shape.GetPathOfShape(this.m_rc);
        }
 
        internal RectangleF GetRectShadow() {
            RectangleF ef1 = new RectangleF(base.RC.Location, base.RC.Size);
            if (this.m_shadow.m_style != ShadowStyle.None) {
                ef1.Width += this.m_shadow.Size.Width;
                ef1.Height += this.m_shadow.Size.Height;
                switch (this.m_shadow.m_style) {
                case ShadowStyle.RightBottom: {
                    return ef1;
                }
                case ShadowStyle.RightTop: {
                    ef1.Y -= this.m_shadow.Size.Height;
                    return ef1;
                }
                case ShadowStyle.LeftTop: {
                    ef1.X -= this.m_shadow.Size.Width;
                    ef1.Y -= this.m_shadow.Size.Height;
                    return ef1;
                }
                case ShadowStyle.LeftBottom: {
                    ef1.X -= this.m_shadow.Size.Width;
                    return ef1;
                }
                }
            }
            return ef1;
        }
 
        internal void GetTextAndImageRectangles(Graphics grfx, ref RectangleF rcText, ref RectangleF rcImage, RectangleF rcClip, Image image) {
            rcText = rcClip;
            rcImage = rcClip;
            float single1 = (this.m_textMargin.Width * base.RC.Width) / 100f;
            float single2 = (this.m_textMargin.Height * base.RC.Height) / 100f;
            rcText.Inflate(-single1, -single2);
            SizeF ef1 = grfx.MeasureString(this.m_text, this.m_font, rcText.Size, this.GetFormatDT());
            float single3 = ef1.Height;
            switch (this.m_autoSize) {
            case AutoSize.None:
            case AutoSize.NodeToText: {
                SizeF ef2 = new SizeF((float) image.Size.Width, (float) image.Size.Height);
                ef2 = Misc.SizeDeviceToPage(grfx, ef2);
                rcImage.Width = ef2.Width;
                rcImage.Height = ef2.Height;
                switch (this.m_imagePosition) {
                case ImagePosition.LeftMiddle: {
                    rcImage.Y += ((rcClip.Height / 2f) - (rcImage.Height / 2f));
                    break;
                }
                case ImagePosition.LeftBottom: {
                    rcImage.Y += (rcClip.Height - rcImage.Height);
                    break;
                }
                case ImagePosition.RightTop: {
                    rcImage.X += (rcClip.Width - rcImage.Width);
                    break;
                }
                case ImagePosition.RightMiddle: {
                    rcImage.X += (rcClip.Width - rcImage.Width);
                    rcImage.Y += ((rcClip.Height / 2f) - (rcImage.Height / 2f));
                    break;
                }
                case ImagePosition.RightBottom: {
                    rcImage.X += (rcClip.Width - rcImage.Width);
                    rcImage.Y += (rcClip.Height - rcImage.Height);
                    break;
                }
                case ImagePosition.CenterTop: {
                    rcImage.X += ((rcClip.Width / 2f) - (rcImage.Width / 2f));
                    break;
                }
                case ImagePosition.CenterMiddle: {
                    rcImage.X += ((rcClip.Width / 2f) - (rcImage.Width / 2f));
                    rcImage.Y += ((rcClip.Height / 2f) - (rcImage.Height / 2f));
                    break;
                }
                case ImagePosition.CenterBottom: {
                    rcImage.X += ((rcClip.Width / 2f) - (rcImage.Width / 2f));
                    rcImage.Y += (rcClip.Height - rcImage.Height);
                    break;
                }
                case ImagePosition.RelativeToText: {
                    rcImage.X += ((rcClip.Width / 2f) - (rcImage.Width / 2f));
                    rcImage.Y += (((rcClip.Height / 2f) - (single3 / 2f)) - (rcImage.Height / 2f));
                    rcImage.Y = Math.Min(Math.Max(rcImage.Y, rcClip.Top), rcClip.Bottom);
                    break;
                }
                case ImagePosition.Custom: {
                    rcImage.X = this.m_imageLocation.X + base.RC.Left;
                    rcImage.Y = this.m_imageLocation.Y + base.RC.Top;
                    break;
                }
                }
                break;
            }
            }
            switch (this.m_alignment) {
            case Alignment.LeftJustifyTOP:
            case Alignment.RightJustifyTOP:
            case Alignment.CenterTOP: {
                if ((this.m_autoSize == AutoSize.None) && (this.m_imagePosition == ImagePosition.RelativeToText)) {
                    rcImage.Y += single3;
                    rcImage.Y = Math.Min(Math.Max(rcImage.Y, rcText.Top), rcText.Bottom);
                }
                return;
            }
            case Alignment.LeftJustifyMIDDLE:
            case Alignment.RightJustifyMIDDLE:
            case Alignment.CenterMIDDLE: {
                float single4 = ((this.m_autoSize == AutoSize.None) && (this.m_imagePosition == ImagePosition.RelativeToText)) ? rcImage.Height : 0f;
                rcText.Y = Math.Max((float) ((((rcText.Top + rcText.Bottom) - single3) + single4) / 2f), rcText.Top);
                rcText.Height = single3;
                return;
            }
            case Alignment.LeftJustifyBOTTOM:
            case Alignment.RightJustifyBOTTOM:
            case Alignment.CenterBOTTOM: {
                rcText.Y = Math.Max((float) (rcText.Bottom - single3), rcText.Top);
                rcText.Height = single3;
                return;
            }
            }
        }
 
        internal RectangleF GetTextRectangle(Graphics grfx, RectangleF rcClip) {
            RectangleF ef1 = rcClip;
            float single1 = (this.m_textMargin.Width * base.RC.Width) / 100f;
            float single2 = (this.m_textMargin.Height * base.RC.Height) / 100f;
            ef1.Inflate(-single1, -single2);
            SizeF ef2 = grfx.MeasureString(this.m_text, this.m_font, ef1.Size, this.GetFormatDT());
            float single3 = ef2.Height;
            switch (this.m_alignment) {
            case Alignment.LeftJustifyTOP:
            case Alignment.RightJustifyTOP:
            case Alignment.CenterTOP: {
                return ef1;
            }
            case Alignment.LeftJustifyMIDDLE:
            case Alignment.RightJustifyMIDDLE:
            case Alignment.CenterMIDDLE: {
                ef1.Y = Math.Max((float) (((ef1.Top + ef1.Bottom) - single3) / 2f), ef1.Top);
                ef1.Height = single3;
                return ef1;
            }
            case Alignment.LeftJustifyBOTTOM:
            case Alignment.RightJustifyBOTTOM:
            case Alignment.CenterBOTTOM: {
                ef1.Y = Math.Max((float) (ef1.Bottom - single3), ef1.Top);
                ef1.Height = single3;
                return ef1;
            }
            }
            return ef1;
        }
 
        internal bool HitTest(PointF pt) {
            return this.m_shape.HitTest(pt, this.m_rc);
        }
 
        private void Init() {
            this.m_font = Control.DefaultFont;
            this.m_fillColor = SystemColors.Window;
            this.m_drawColor = SystemColors.WindowText;
            this.m_textColor = SystemColors.WindowText;
            this.m_gradientColor = SystemColors.Control;
            this.m_gradientMode = LinearGradientMode.BackwardDiagonal;
            this.m_hidden = false;
            this.m_logical = true;
            this.m_selectable = true;
            this.m_ownerDraw = false;
            this.m_xMoveable = true;
            this.m_yMoveable = true;
            this.m_xSizeable = true;
            this.m_ySizeable = true;
            this.m_inLinkable = true;
            this.m_outLinkable = true;
            this.m_transparent = false;
            this.m_gradient = false;
            this.m_labelEdit = true;
            this.m_alignment = Alignment.CenterMIDDLE;
            this.m_autoSize = AutoSize.None;
            this.m_imagePosition = ImagePosition.RelativeToText;
            this.m_shadow = new Shadow(ShadowStyle.None, SystemColors.GrayText, new Size(SystemInformation.IconSize.Width / 8, SystemInformation.IconSize.Height / 8));
            this.m_shadow.BeforeChange += new Shadow.BeforeChangeEventHandler(this.shadow_BeforeChange);
            this.m_shadow.AfterChange += new Shadow.AfterChangeEventHandler(this.shadow_AfterChange);
            this.m_shape = new Shape(ShapeStyle.Ellipse, ShapeOrientation.so_0);
            this.m_shape.BeforeChange += new Shape.BeforeChangeEventHandler(this.shape_BeforeChange);
            this.m_shape.AfterChange += new Shape.AfterChangeEventHandler(this.shape_AfterChange);
            this.m_trimming = StringTrimming.None;
            this.m_drawWidth = 1;
            this.m_dashStyle = DashStyle.Solid;
            this.m_backMode = BackMode.Transparent;
            this.m_textMargin = new Size(0, 0);
            this.m_imageIndex = -1;
            this.m_imageLocation = new PointF(0f, 0f);
            this.m_b1 = true;
            this.m_b2 = true;
            this.m_isEditing = false;
            this.m_postOrderNumber = 0;
            this.m_aLinks = new ArrayList();
        }
 
        internal void InitWithDefNode(DefNode defnode) {
            if (defnode != null) {
                this.m_font = defnode.m_font;
                this.m_fillColor = defnode.m_fillColor;
                this.m_drawColor = defnode.m_drawColor;
                this.m_textColor = defnode.m_textColor;
                this.m_gradientColor = defnode.m_gradientColor;
                this.m_gradientMode = defnode.m_gradientMode;
                this.m_hidden = defnode.m_hidden;
                this.m_logical = defnode.m_logical;
                this.m_selectable = defnode.m_selectable;
                this.m_ownerDraw = defnode.m_ownerDraw;
                this.m_xMoveable = defnode.m_xMoveable;
                this.m_yMoveable = defnode.m_yMoveable;
                this.m_xSizeable = defnode.m_xSizeable;
                this.m_ySizeable = defnode.m_ySizeable;
                this.m_inLinkable = defnode.m_inLinkable;
                this.m_outLinkable = defnode.m_outLinkable;
                this.m_transparent = defnode.m_transparent;
                this.m_gradient = defnode.m_gradient;
                this.m_backMode = defnode.m_backMode;
                this.m_labelEdit = defnode.m_labelEdit;
                this.m_alignment = defnode.m_alignment;
                this.m_autoSize = defnode.m_autoSize;
                this.m_imagePosition = defnode.m_imagePosition;
                this.m_shadow = (Shadow) defnode.m_shadow.Clone();
                this.m_shadow.BeforeChange += new Shadow.BeforeChangeEventHandler(this.shadow_BeforeChange);
                this.m_shadow.AfterChange += new Shadow.AfterChangeEventHandler(this.shadow_AfterChange);
                this.m_shape = (Shape) defnode.m_shape.Clone();
                this.m_shape.BeforeChange += new Shape.BeforeChangeEventHandler(this.shape_BeforeChange);
                this.m_shape.AfterChange += new Shape.AfterChangeEventHandler(this.shape_AfterChange);
                this.m_trimming = defnode.m_trimming;
                this.m_drawWidth = defnode.m_drawWidth;
                this.m_dashStyle = defnode.m_dashStyle;
                this.m_textMargin = defnode.m_textMargin;
                this.m_imageIndex = defnode.m_imageIndex;
                this.m_imageLocation = defnode.m_imageLocation;
                this.m_text = defnode.m_text;
                this.m_tooltip = defnode.m_tooltip;
                this.m_b1 = true;
                this.m_b2 = true;
                this.m_isEditing = false;
                this.m_postOrderNumber = 0;
                this.m_aLinks = new ArrayList();
            }
        }
 
        internal override void InvalidateHandles() {
            if ((base.Existing && (this.m_af.m_repaint == 0)) && this.m_selectable) {
                Graphics graphics1 = this.m_af.CreateGraphics();
                this.m_af.CoordinatesDeviceToWorld(graphics1);
                RectangleF ef1 = new RectangleF(base.RC.Location, base.RC.Size);
                ef1 = Misc.RectWorldToDevice(graphics1, ef1);
                SizeF ef2 = new SizeF((float) (this.m_af.m_selGrabSize + 2), (float) (this.m_af.m_selGrabSize + 2));
                ef1.Inflate(ef2);
                this.m_af.Invalidate(Rectangle.Round(ef1));
                graphics1.Dispose();
            }
        }
 
        internal override void InvalidateItem() {
            if (base.Existing && (this.m_af.m_repaint == 0)) {
                RectangleF ef1;
                Graphics graphics1 = this.m_af.CreateGraphics();
                this.m_af.CoordinatesDeviceToWorld(graphics1);
                foreach (Link link1 in this.m_aLinks) {
                    ef1 = new RectangleF(link1.RC.Location, link1.RC.Size);
                    ef1 = Misc.RectWorldToDevice(graphics1, ef1);
                    float single1 = (link1.m_drawWidth == 0) ? ((float) 1) : ((float) link1.m_drawWidth);
                    ef1.Inflate(single1, single1);
                    ef1.Inflate((float) this.m_af.m_selGrabSize, (float) this.m_af.m_selGrabSize);
                    this.m_af.Invalidate(Rectangle.Round(ef1));
                }
                ef1 = this.GetRectShadow();
                ef1 = Misc.RectWorldToDevice(graphics1, ef1);
                float single2 = (this.m_drawWidth == 0) ? ((float) 1) : ((float) this.m_drawWidth);
                ef1.Inflate(single2, single2);
                ef1.Inflate((float) this.m_af.m_selGrabSize, (float) this.m_af.m_selGrabSize);
                this.m_af.Invalidate(Rectangle.Round(ef1));
                graphics1.Dispose();
            }
        }
 
        internal bool IsDestinationOf(Node node) {
            bool flag1 = false;
            foreach (Link link1 in this.m_aLinks) {
                if (link1.m_org == node) {
                    return true;
                }
            }
            return flag1;
        }
 
        internal bool IsOriginOf(Node node) {
            bool flag1 = false;
            foreach (Link link1 in this.m_aLinks) {
                if (link1.m_dst == node) {
                    return true;
                }
            }
            return flag1;
        }
 
        internal void Move(PointF offset) {
            this.InvalidateItem();
            this.InvalidateHandles();
            foreach (Link link1 in this.m_aLinks) {
                if (!link1.m_drag) {
                    if (link1.m_adjustDst && (this == link1.m_dst)) {
                        link1.m_aptf[link1.m_aptf.Length - 1].X += offset.X;
                        link1.m_aptf[link1.m_aptf.Length - 1].Y += offset.Y;
                    }
                    if (link1.m_adjustOrg && (this == link1.m_org)) {
                        link1.m_aptf[0].X += offset.X;
                        link1.m_aptf[0].Y += offset.Y;
                    }
                    continue;
                }
                if (!link1.m_flag) {
                    for (int num1 = 0; num1 < link1.m_aptf.Length; num1++) {
                        link1.m_aptf[num1].X += offset.X;
                        link1.m_aptf[num1].Y += offset.Y;
                    }
                    link1.m_flag = true;
                }
            }
            RectangleF ef1 = base.RC;
            ef1.Offset(offset);
            base.RC = ef1;
            this.AdjustNodeLinks();
            this.InvalidateItem();
            this.InvalidateHandles();
        }
 
        public void Remove() {
            if (base.Existing) {
                AddFlow flow1 = this.m_af;
                flow1.m_undo.BeginActionInternal(Action.NodeRemove);
                flow1.RemoveNode(this);
                flow1.UpdateRect();
                flow1.m_undo.EndActionInternal();
            }
        }
 
        public void ResetImageLocation() {
            this.m_imageLocation = new PointF(0f, 0f);
        }
 
        internal void SetAutoSize2(AutoSize newValue) {
            this.m_autoSize = newValue;
            this.InvalidateItem();
            this.InvalidateHandles();
            if ((this.m_autoSize != AutoSize.None) && (this.m_autoSize != AutoSize.ImageToNode)) {
                this.AdjustNodeSize();
                this.AdjustNodeLinks();
                this.InvalidateItem();
                this.InvalidateHandles();
            }
            this.m_af.UpdateRect();
        }
 
        internal void SetDashStyle2(DashStyle newValue) {
            this.m_dashStyle = newValue;
            this.InvalidateItem();
            this.InvalidateHandles();
            if ((this.m_autoSize != AutoSize.None) && (this.m_autoSize != AutoSize.ImageToNode)) {
                this.AdjustNodeSize();
                this.AdjustNodeLinks();
                this.InvalidateItem();
                this.InvalidateHandles();
                this.m_af.UpdateRect();
            }
        }
 
        internal void SetDrawWidth2(int newValue) {
            this.m_drawWidth = newValue;
            this.InvalidateItem();
            this.InvalidateHandles();
            if ((this.m_autoSize != AutoSize.None) && (this.m_autoSize != AutoSize.ImageToNode)) {
                this.AdjustNodeSize();
                this.AdjustNodeLinks();
                this.InvalidateItem();
                this.InvalidateHandles();
                this.m_af.UpdateRect();
            }
        }
 
        internal void SetFont2(Font newValue) {
            this.InvalidateItem();
            this.InvalidateHandles();
            this.m_font = newValue;
            if (this.m_autoSize == AutoSize.NodeToText) {
                this.AdjustNodeSizeToText();
                this.AdjustNodeLinks();
                this.m_af.UpdateRect();
            }
            this.InvalidateItem();
            this.InvalidateHandles();
        }
 
        internal void SetLocation2(PointF location) {
            this.InvalidateItem();
            this.InvalidateHandles();
            float single1 = location.X - base.RC.Left;
            float single2 = location.Y - base.RC.Top;
            RectangleF ef1 = base.RC;
            ef1.Location = location;
            base.RC = ef1;
            this.AdjustNodeSize();
            this.AdjustNodeLinks();
            this.InvalidateItem();
            this.InvalidateHandles();
            if (((single1 > 0f) && (base.RC.Right > this.m_af.GraphRect.Right)) || ((single2 > 0f) && (base.RC.Bottom > this.m_af.GraphRect.Bottom))) {
                this.m_af.UpdateRect2(this.GetRectShadow());
            }
            else if (((single1 < 0f) && ((base.RC.Right - single1) >= this.m_af.GraphRect.Right)) || ((single2 < 0f) && ((base.RC.Bottom - single2) >= this.m_af.GraphRect.Bottom))) {
                this.m_af.UpdateRect();
            }
        }
 
        internal void SetRect2(RectangleF rc) {
            this.InvalidateItem();
            this.InvalidateHandles();
            float single1 = rc.Left - base.RC.Left;
            float single2 = rc.Top - base.RC.Top;
            float single3 = rc.Width - base.RC.Width;
            float single4 = rc.Height - base.RC.Height;
            this.m_rc = rc;
            this.AdjustNodeSize();
            this.AdjustNodeLinks();
            this.InvalidateItem();
            this.InvalidateHandles();
            if ((((single1 > 0f) && (base.RC.Right > this.m_af.GraphRect.Right)) || ((single2 > 0f) && (base.RC.Bottom > this.m_af.GraphRect.Bottom))) || (((single3 > 0f) && (base.RC.Right > this.m_af.GraphRect.Right)) || ((single4 > 0f) && (base.RC.Bottom > this.m_af.GraphRect.Bottom)))) {
                this.m_af.UpdateRect2(this.GetRectShadow());
            }
            else if ((((single1 < 0f) && ((base.RC.Right - single1) >= this.m_af.GraphRect.Right)) || ((single2 < 0f) && ((base.RC.Bottom - single2) >= this.m_af.GraphRect.Bottom))) || (((single3 < 0f) && ((base.RC.Right - single3) >= this.m_af.GraphRect.Right)) || ((single4 < 0f) && ((base.RC.Bottom - single4) >= this.m_af.GraphRect.Bottom)))) {
                this.m_af.UpdateRect();
            }
        }
 
        internal void SetShadow2(Shadow newValue) {
            if (newValue != null) {
                RectangleF ef1 = this.GetRectShadow();
                this.m_shadow = newValue;
                this.m_shadow.BeforeChange += new Shadow.BeforeChangeEventHandler(this.shadow_BeforeChange);
                this.m_shadow.AfterChange += new Shadow.AfterChangeEventHandler(this.shadow_AfterChange);
                this.InvalidateItem();
                RectangleF ef2 = this.GetRectShadow();
                float single1 = ef2.Left - ef1.Left;
                float single2 = ef2.Top - ef1.Top;
                float single3 = ef2.Width - ef1.Width;
                float single4 = ef2.Height - ef1.Height;
                if ((((single1 > 0f) && (ef2.Right > this.m_af.GraphRect.Right)) || ((single2 > 0f) && (ef2.Bottom > this.m_af.GraphRect.Bottom))) || (((single3 > 0f) && (ef2.Right > this.m_af.GraphRect.Right)) || ((single4 > 0f) && (ef2.Bottom > this.m_af.GraphRect.Bottom)))) {
                    this.m_af.UpdateRect2(ef2);
                }
                else if ((((single1 < 0f) && ((ef2.Right - single1) >= this.m_af.GraphRect.Right)) || ((single2 < 0f) && ((ef2.Bottom - single2) >= this.m_af.GraphRect.Bottom))) || (((single3 < 0f) && ((ef2.Right - single3) >= this.m_af.GraphRect.Right)) || ((single4 < 0f) && ((ef2.Bottom - single4) >= this.m_af.GraphRect.Bottom)))) {
                    this.m_af.UpdateRect();
                }
            }
        }
 
        internal void SetShape2(Shape newValue) {
            if (newValue != null) {
                this.m_shape = newValue;
                this.m_shape.BeforeChange += new Shape.BeforeChangeEventHandler(this.shape_BeforeChange);
                this.m_shape.AfterChange += new Shape.AfterChangeEventHandler(this.shape_AfterChange);
                this.AdjustNodeLinks();
                this.InvalidateItem();
            }
        }
 
        internal void SetSize2(SizeF size) {
            float single1 = size.Width - base.RC.Width;
            float single2 = size.Height - base.RC.Height;
            this.InvalidateItem();
            this.InvalidateHandles();
            RectangleF ef1 = base.RC;
            ef1.Size = size;
            base.RC = ef1;
            this.AdjustNodeSize();
            this.AdjustNodeLinks();
            this.InvalidateItem();
            this.InvalidateHandles();
            if (((single1 > 0f) && (base.RC.Right > this.m_af.GraphRect.Right)) || ((single2 > 0f) && (base.RC.Bottom > this.m_af.GraphRect.Bottom))) {
                this.m_af.UpdateRect2(this.GetRectShadow());
            }
            else if (((single1 < 0f) && ((base.RC.Right - single1) >= this.m_af.GraphRect.Right)) || ((single2 < 0f) && ((base.RC.Bottom - single2) >= this.m_af.GraphRect.Bottom))) {
                this.m_af.UpdateRect();
            }
        }
 
        internal void SetText2(string newValue) {
            this.m_text = newValue;
            this.InvalidateItem();
            if (this.m_autoSize == AutoSize.NodeToText) {
                this.AdjustNodeSizeToText();
                this.InvalidateItem();
                this.InvalidateHandles();
                this.AdjustNodeLinks();
            }
        }
 
        internal void SetTextMargin2(Size newValue) {
            this.m_textMargin = newValue;
            this.InvalidateItem();
            if (this.m_autoSize == AutoSize.NodeToText) {
                this.AdjustNodeSizeToText();
                this.InvalidateItem();
                this.InvalidateHandles();
                this.AdjustNodeLinks();
            }
        }
 
        private void shadow_AfterChange(object sender, EventArgs e) {
            if (this.m_af != null) {
                this.InvalidateItem();
                this.m_af.IncrementChangedFlag(1);
                RectangleF ef1 = this.GetRectShadow();
                float single1 = ef1.Left - this.m_rc2.Left;
                float single2 = ef1.Top - this.m_rc2.Top;
                float single3 = ef1.Width - this.m_rc2.Width;
                float single4 = ef1.Height - this.m_rc2.Height;
                if ((((single1 > 0f) && (ef1.Right > this.m_af.GraphRect.Right)) || ((single2 > 0f) && (ef1.Bottom > this.m_af.GraphRect.Bottom))) || (((single3 > 0f) && (ef1.Right > this.m_af.GraphRect.Right)) || ((single4 > 0f) && (ef1.Bottom > this.m_af.GraphRect.Bottom)))) {
                    this.m_af.UpdateRect2(ef1);
                }
                else if ((((single1 < 0f) && ((ef1.Right - single1) >= this.m_af.GraphRect.Right)) || ((single2 < 0f) && ((ef1.Bottom - single2) >= this.m_af.GraphRect.Bottom))) || (((single3 < 0f) && ((ef1.Right - single3) >= this.m_af.GraphRect.Right)) || ((single4 < 0f) && ((ef1.Bottom - single4) >= this.m_af.GraphRect.Bottom)))) {
                    this.m_af.UpdateRect();
                }
            }
        }
 
        private void shadow_BeforeChange(object sender, EventArgs e) {
            if (this.m_af != null) {
                this.m_rc2 = this.GetRectShadow();
                if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                    this.m_af.m_undo.SubmitTask(new NodeShadowTask(this));
                }
            }
        }
 
        private void shape_AfterChange(object sender, EventArgs e) {
            if (this.m_af != null) {
                this.AdjustNodeLinks();
                this.InvalidateItem();
                this.m_af.IncrementChangedFlag(1);
            }
        }
 
        private void shape_BeforeChange(object sender, EventArgs e) {
            if (((this.m_af != null) && this.m_af.m_undo.CanUndoRedo) && !this.m_af.m_undo.SkipUndo) {
                this.m_af.m_undo.SubmitTask(new NodeShapeTask(this));
            }
        }
 
        public bool ShouldSerializeImageLocation() {
            if (this.m_imageLocation.X == 0f) {
                return (this.m_imageLocation.Y != 0f);
            }
            return true;
        }
 
        public Alignment Alignment {
            get {
                return this.m_alignment;
            }
            set {
                if (this.m_alignment != value) {
                    if (!base.Existing) {
                        this.m_alignment = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeAlignmentTask(this));
                        }
                        this.m_alignment = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public AutoSize AutoSize {
            get {
                return this.m_autoSize;
            }
            set {
                if (this.m_autoSize != value) {
                    if (!base.Existing) {
                        this.m_autoSize = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeAutoSizeTask(this));
                        }
                        this.SetAutoSize2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public ItemsCollection ConnectedItems {
            get {
                if (!base.Existing) {
                    return null;
                }
                return this.m_af.m_connect.GetConnectedPart(this);
            }
        }
 
        public Color FillColor {
            get {
                return this.m_fillColor;
            }
            set {
                if (this.m_fillColor != value) {
                    if (!base.Existing) {
                        this.m_fillColor = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeFillColorTask(this));
                        }
                        this.m_fillColor = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public bool Gradient {
            get {
                return this.m_gradient;
            }
            set {
                if (this.m_gradient != value) {
                    if (!base.Existing) {
                        this.m_gradient = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeFlagTask(this, Action.Gradient));
                        }
                        this.m_gradient = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public Color GradientColor {
            get {
                return this.m_gradientColor;
            }
            set {
                if (this.m_gradientColor != value) {
                    if (!base.Existing) {
                        this.m_gradientColor = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeGradientColorTask(this));
                        }
                        this.m_gradientColor = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public LinearGradientMode GradientMode {
            get {
                return this.m_gradientMode;
            }
            set {
                if (this.m_gradientMode != value) {
                    if (!base.Existing) {
                        this.m_gradientMode = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeGradientModeTask(this));
                        }
                        this.m_gradientMode = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public int ImageIndex {
            get {
                return this.m_imageIndex;
            }
            set {
                if (this.m_imageIndex != value) {
                    if (!base.Existing) {
                        this.m_imageIndex = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeImageIndexTask(this));
                        }
                        this.m_imageIndex = value;
                        this.InvalidateItem();
                        this.InvalidateHandles();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public PointF ImageLocation {
            get {
                return this.m_imageLocation;
            }
            set {
                if (this.m_imageLocation != value) {
                    if (!base.Existing) {
                        this.m_imageLocation = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeImageLocationTask(this));
                        }
                        this.m_imageLocation = value;
                        this.InvalidateItem();
                        this.InvalidateHandles();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public ImagePosition ImagePosition {
            get {
                return this.m_imagePosition;
            }
            set {
                if (this.m_imagePosition != value) {
                    if (!base.Existing) {
                        this.m_imagePosition = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeImagePositionTask(this));
                        }
                        this.m_imagePosition = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public int Index {
            get {
                if (!base.Existing) {
                    return 0;
                }
                return this.m_af.m_nodes.IndexOf(this);
            }
        }
 
        public bool InLinkable {
            get {
                return this.m_inLinkable;
            }
            set {
                if (this.m_inLinkable != value) {
                    if (!base.Existing) {
                        this.m_inLinkable = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeFlagTask(this, Action.InLinkable));
                        }
                        this.m_inLinkable = value;
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public LinksCollection InLinks {
            get {
                if (!base.Existing) {
                    return null;
                }
                return new LinksCollection(this, LinksCollection.Type.In);
            }
        }
 
        public bool IsEditing {
            get {
                return this.m_isEditing;
            }
        }
 
        public bool LabelEdit {
            get {
                return this.m_labelEdit;
            }
            set {
                if (this.m_labelEdit != value) {
                    if (!base.Existing) {
                        this.m_labelEdit = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeFlagTask(this, Action.LabelEdit));
                        }
                        this.m_labelEdit = value;
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public LinksCollection Links {
            get {
                if (!base.Existing) {
                    return null;
                }
                return new LinksCollection(this, LinksCollection.Type.InOut);
            }
        }
 
        public PointF Location {
            get {
                return base.RC.Location;
            }
            set {
                if (base.RC.Location != value) {
                    if ((value.X < 0f) || (value.Y < 0f)) {
                        throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.OutOfAreaError));
                    }
                    if (!base.Existing) {
                        RectangleF ef1 = base.RC;
                        ef1.Location = value;
                        base.RC = ef1;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodePositionTask(this, Action.NodeMove));
                        }
                        this.SetLocation2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public bool OutLinkable {
            get {
                return this.m_outLinkable;
            }
            set {
                if (this.m_outLinkable != value) {
                    if (!base.Existing) {
                        this.m_outLinkable = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeFlagTask(this, Action.OutLinkable));
                        }
                        this.m_outLinkable = value;
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public LinksCollection OutLinks {
            get {
                if (!base.Existing) {
                    return null;
                }
                return new LinksCollection(this, LinksCollection.Type.Out);
            }
        }
 
        public RectangleF Rect {
            get {
                return this.m_rc;
            }
            set {
                if (!base.RC.Equals(value)) {
                    if (((value.Left < 0f) || (value.Top < 0f)) || ((value.Width < 0f) || (value.Height < 0f))) {
                        throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.OutOfAreaError));
                    }
                    if (!base.Existing) {
                        this.m_rc = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodePositionTask(this, Action.NodeMoveAndSize));
                        }
                        this.SetRect2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public Shadow Shadow {
            get {
                return this.m_shadow;
            }
            set {
                if (!this.m_shadow.Equals(value)) {
                    if (!base.Existing) {
                        this.m_shadow = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeShadowTask(this));
                        }
                        this.SetShadow2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public Shape Shape {
            get {
                return this.m_shape;
            }
            set {
                if (!this.m_shape.Equals(value)) {
                    if (!base.Existing) {
                        this.m_shape = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeShapeTask(this));
                        }
                        this.SetShape2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public SizeF Size {
            get {
                return base.RC.Size;
            }
            set {
                if (base.RC.Size != value) {
                    if ((value.Width < 0f) || (value.Height < 0f)) {
                        throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.OutOfAreaError));
                    }
                    if (!base.Existing) {
                        RectangleF ef1 = base.RC;
                        ef1.Size = value;
                        base.RC = ef1;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodePositionTask(this, Action.NodeResize));
                        }
                        this.SetSize2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public override string Text {
            get {
                return this.m_text;
            }
            set {
                if (this.m_text != value) {
                    if (!base.Existing) {
                        this.m_text = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeTextTask(this));
                        }
                        this.SetText2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public Size TextMargin {
            get {
                return this.m_textMargin;
            }
            set {
                if (((value.Width < 0) || (value.Width > 50)) || ((value.Height < 0) || (value.Height > 50))) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.TextMarginLimits));
                }
                if (this.m_textMargin != value) {
                    if (!base.Existing) {
                        this.m_textMargin = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeTextMarginTask(this));
                        }
                        this.SetTextMargin2(value);
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public override string Tooltip {
            get {
                return this.m_tooltip;
            }
            set {
                if (this.m_tooltip != value) {
                    if (!base.Existing) {
                        this.m_tooltip = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeTooltipTask(this));
                        }
                        this.m_tooltip = value;
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public bool Transparent {
            get {
                return this.m_transparent;
            }
            set {
                if (this.m_transparent != value) {
                    if (!base.Existing) {
                        this.m_transparent = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeFlagTask(this, Action.Transparent));
                        }
                        this.m_transparent = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public StringTrimming Trimming {
            get {
                return this.m_trimming;
            }
            set {
                if (this.m_trimming != value) {
                    if (!base.Existing) {
                        this.m_trimming = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeTrimmingTask(this));
                        }
                        this.m_trimming = value;
                        this.InvalidateItem();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public bool XMoveable {
            get {
                return this.m_xMoveable;
            }
            set {
                if (this.m_xMoveable != value) {
                    if (!base.Existing) {
                        this.m_xMoveable = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeFlagTask(this, Action.XMoveable));
                        }
                        this.m_xMoveable = value;
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public bool XSizeable {
            get {
                return this.m_xSizeable;
            }
            set {
                if (this.m_xSizeable != value) {
                    if (!base.Existing) {
                        this.m_xSizeable = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeFlagTask(this, Action.XSizeable));
                        }
                        this.m_xSizeable = value;
                        this.InvalidateHandles();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public bool YMoveable {
            get {
                return this.m_yMoveable;
            }
            set {
                if (this.m_yMoveable != value) {
                    if (!base.Existing) {
                        this.m_yMoveable = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeFlagTask(this, Action.YMoveable));
                        }
                        this.m_yMoveable = value;
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        public bool YSizeable {
            get {
                return this.m_ySizeable;
            }
            set {
                if (this.m_ySizeable != value) {
                    if (!base.Existing) {
                        this.m_ySizeable = value;
                    }
                    else {
                        if (this.m_af.m_undo.CanUndoRedo && !this.m_af.m_undo.SkipUndo) {
                            this.m_af.m_undo.SubmitTask(new NodeFlagTask(this, Action.YSizeable));
                        }
                        this.m_ySizeable = value;
                        this.InvalidateHandles();
                        this.m_af.IncrementChangedFlag(1);
                    }
                }
            }
        }
 
        internal const int HandlesNode = 8;
 
        internal Alignment m_alignment;
 
        internal ArrayList m_aLinks;
 
        internal AutoSize m_autoSize;
 
        internal bool m_b1;
 
        internal bool m_b2;
 
        internal Color m_fillColor;
 
        internal bool m_gradient;
 
        internal Color m_gradientColor;
 
        internal LinearGradientMode m_gradientMode;
 
        internal int m_imageIndex;
 
        internal PointF m_imageLocation;
 
        internal ImagePosition m_imagePosition;
 
        internal bool m_inLinkable;
 
        internal bool m_isEditing;
 
        internal bool m_labelEdit;
 
        internal bool m_outLinkable;
 
        internal int m_postOrderNumber;
 
        internal RectangleF m_rc2;
 
        internal Shadow m_shadow;
 
        internal Shape m_shape;
 
        internal Size m_textMargin;
 
        internal bool m_transparent;
 
        internal StringTrimming m_trimming;
 
        internal bool m_xMoveable;
 
        internal bool m_xSizeable;
 
        internal bool m_yMoveable;
 
        internal bool m_ySizeable;
 
        internal const int MaxTextMargin = 50;
 
        internal const int MinTextMargin = 0;
    }

    internal class NodeAlignmentTask : Task {

        private NodeAlignmentTask() {
        }
 
        internal NodeAlignmentTask(Node node) {
            this.m_node = node;
            this.m_oldAlignment = this.m_node.m_alignment;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Alignment;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Alignment alignment1 = this.m_node.m_alignment;
            this.m_node.m_alignment = this.m_oldAlignment;
            this.m_node.InvalidateItem();
            this.m_oldAlignment = alignment1;
        }
 
        private Node m_node;
 
        private Alignment m_oldAlignment;
    }

    internal class NodeAutoSizeTask : Task {

        private NodeAutoSizeTask() {
        }
 
        internal NodeAutoSizeTask(Node node) {
            this.m_node = node;
            this.m_oldValue = this.m_node.m_autoSize;
            this.m_oldrc = new RectangleF(this.m_node.RC.Location, this.m_node.RC.Size);
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.AutoSize;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            AutoSize size1 = this.m_node.m_autoSize;
            this.m_node.SetAutoSize2(this.m_oldValue);
            this.m_oldValue = size1;
            this.m_node.InvalidateItem();
            RectangleF ef1 = this.m_node.m_rc;
            this.m_node.m_rc = this.m_oldrc;
            this.m_node.AdjustNodeLinks();
            this.m_oldrc = ef1;
            this.m_node.InvalidateItem();
        }
 
        private Node m_node;
 
        private RectangleF m_oldrc;
 
        private AutoSize m_oldValue;
    }

    internal class NodeDashStyleTask : Task {

        private NodeDashStyleTask() {
        }
 
        internal NodeDashStyleTask(Node node) {
            this.m_node = node;
            this.m_oldDashStyle = this.m_node.m_dashStyle;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.DashStyle;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            DashStyle style1 = this.m_node.m_dashStyle;
            this.m_node.SetDashStyle2(this.m_oldDashStyle);
            this.m_oldDashStyle = style1;
        }
 
        private Node m_node;
 
        private DashStyle m_oldDashStyle;
    }

    public enum NodeDrawFlags {
        // Fields
        All = 0x1f,
        Fill = 2,
        Image = 8,
        None = 0,
        Shadow = 0x10,
        Shape = 1,
        Text = 4
    }
 
    internal class NodeDrawWidthTask : Task {

        private NodeDrawWidthTask() {
        }
 
        internal NodeDrawWidthTask(Node node) {
            this.m_node = node;
            this.m_oldValue = this.m_node.m_drawWidth;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.DrawWidth;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            int num1 = this.m_node.m_drawWidth;
            this.m_node.SetDrawWidth2(this.m_oldValue);
            this.m_oldValue = num1;
        }
 
        private Node m_node;
 
        private int m_oldValue;
    }

    internal class NodeFillColorTask : Task {

        private NodeFillColorTask() {
        }
 
        internal NodeFillColorTask(Node node) {
            this.m_node = node;
            this.m_oldValue = this.m_node.m_fillColor;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.FillColor;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Color color1 = this.m_node.m_fillColor;
            this.m_node.m_fillColor = this.m_oldValue;
            this.m_node.InvalidateItem();
            this.m_oldValue = color1;
        }
 
        private Node m_node;
 
        private Color m_oldValue;
    }

    internal class NodeFlagTask : Task {

        private NodeFlagTask() {
        }
 
        internal NodeFlagTask(Node node, Action code) {
            this.m_node = node;
            this.m_code = code;
            Action action1 = this.m_code;
            if (action1 != Action.Gradient) {
                switch (action1) {
                case Action.InLinkable: {
                    this.m_oldValue = this.m_node.InLinkable;
                    return;
                }
                case Action.OutLinkable: {
                    this.m_oldValue = this.m_node.OutLinkable;
                    return;
                }
                case Action.Jump: {
                    return;
                }
                case Action.LabelEdit: {
                    this.m_oldValue = this.m_node.m_labelEdit;
                    return;
                }
                case Action.Transparent: {
                    this.m_oldValue = this.m_node.m_transparent;
                    return;
                }
                case Action.Trimming:
                case Action.Url: {
                    return;
                }
                case Action.XMoveable: {
                    this.m_oldValue = this.m_node.m_xMoveable;
                    return;
                }
                case Action.XSizeable: {
                    this.m_oldValue = this.m_node.m_xSizeable;
                    return;
                }
                case Action.YMoveable: {
                    this.m_oldValue = this.m_node.m_yMoveable;
                    return;
                }
                case Action.YSizeable: {
                    this.m_oldValue = this.m_node.m_ySizeable;
                    return;
                }
                }
            }
            else {
                this.m_oldValue = this.m_node.m_gradient;
            }
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return this.m_code;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            bool flag1 = false;
            Action action1 = this.m_code;
            if (action1 != Action.Gradient) {
                switch (action1) {
                case Action.InLinkable: {
                    flag1 = this.m_node.InLinkable;
                    this.m_node.InLinkable = this.m_oldValue;
                    goto Label_01A8;
                }
                case Action.OutLinkable: {
                    flag1 = this.m_node.OutLinkable;
                    this.m_node.OutLinkable = this.m_oldValue;
                    goto Label_01A8;
                }
                case Action.Jump:
                case Action.Trimming:
                case Action.Url: {
                    goto Label_01A6;
                }
                case Action.LabelEdit: {
                    flag1 = this.m_node.m_labelEdit;
                    this.m_node.m_labelEdit = this.m_oldValue;
                    goto Label_01A8;
                }
                case Action.Transparent: {
                    flag1 = this.m_node.m_transparent;
                    this.m_node.m_transparent = this.m_oldValue;
                    this.m_node.InvalidateItem();
                    goto Label_01A8;
                }
                case Action.XMoveable: {
                    flag1 = this.m_node.m_xMoveable;
                    this.m_node.m_xMoveable = this.m_oldValue;
                    goto Label_01A8;
                }
                case Action.XSizeable: {
                    flag1 = this.m_node.m_xSizeable;
                    this.m_node.m_xSizeable = this.m_oldValue;
                    this.m_node.InvalidateHandles();
                    goto Label_01A8;
                }
                case Action.YMoveable: {
                    flag1 = this.m_node.m_yMoveable;
                    this.m_node.m_yMoveable = this.m_oldValue;
                    goto Label_01A8;
                }
                case Action.YSizeable: {
                    flag1 = this.m_node.m_ySizeable;
                    this.m_node.m_ySizeable = this.m_oldValue;
                    this.m_node.InvalidateHandles();
                    goto Label_01A8;
                }
                }
            }
            else {
                flag1 = this.m_node.m_gradient;
                this.m_node.m_gradient = this.m_oldValue;
                this.m_node.InvalidateItem();
                goto Label_01A8;
            }
            Label_01A6:
                flag1 = false;
            Label_01A8:
                this.m_oldValue = flag1;
        }
 
        private Action m_code;
 
        private Node m_node;
 
        private bool m_oldValue;
    }

    internal class NodeFontTask : Task {

        private NodeFontTask() {
        }
 
        internal NodeFontTask(Node node) {
            this.m_node = node;
            this.m_oldFont = (Font) this.m_node.m_font.Clone();
            this.m_oldrc = new RectangleF(this.m_node.RC.Location, this.m_node.RC.Size);
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Font;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Font font1 = (Font) this.m_node.m_font.Clone();
            this.m_node.SetFont2(this.m_oldFont);
            this.m_oldFont = font1;
        }
 
        private Node m_node;
 
        private Font m_oldFont;
 
        private RectangleF m_oldrc;
    }

    internal class NodeGradientColorTask : Task {

        private NodeGradientColorTask() {
        }
 
        internal NodeGradientColorTask(Node node) {
            this.m_node = node;
            this.m_oldValue = this.m_node.m_gradientColor;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.GradientColor;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Color color1 = this.m_node.m_gradientColor;
            this.m_node.m_gradientColor = this.m_oldValue;
            this.m_node.InvalidateItem();
            this.m_oldValue = color1;
        }
 
        private Node m_node;
 
        private Color m_oldValue;
    }

    internal class NodeGradientModeTask : Task {

        private NodeGradientModeTask() {
        }
 
        internal NodeGradientModeTask(Node node) {
            this.m_node = node;
            this.m_oldGradientMode = this.m_node.m_gradientMode;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.GradientMode;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            LinearGradientMode mode1 = this.m_node.m_gradientMode;
            this.m_node.m_gradientMode = this.m_oldGradientMode;
            this.m_node.InvalidateItem();
            this.m_oldGradientMode = mode1;
        }
 
        private Node m_node;
 
        private LinearGradientMode m_oldGradientMode;
    }

    internal class NodeImageIndexTask : Task {

        private NodeImageIndexTask() {
        }
 
        internal NodeImageIndexTask(Node node) {
            this.m_node = node;
            this.m_oldImageIndex = this.m_node.m_imageIndex;
            this.m_oldrc = new RectangleF(this.m_node.RC.Location, this.m_node.RC.Size);
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.ImageIndex;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            int num1 = this.m_node.m_imageIndex;
            this.m_node.m_imageIndex = this.m_oldImageIndex;
            this.m_node.InvalidateItem();
            this.m_node.InvalidateHandles();
            this.m_oldImageIndex = num1;
            if (this.m_node.m_autoSize == AutoSize.NodeToImage) {
                this.m_node.InvalidateItem();
                RectangleF ef1 = this.m_node.m_rc;
                this.m_node.m_rc = this.m_oldrc;
                this.m_node.AdjustNodeLinks();
                this.m_oldrc = ef1;
                this.m_node.InvalidateItem();
            }
        }
 
        private Node m_node;
 
        private int m_oldImageIndex;
 
        private RectangleF m_oldrc;
    }

    internal class NodeImageLocationTask : Task {

        private NodeImageLocationTask() {
        }
 
        internal NodeImageLocationTask(Node node) {
            this.m_node = node;
            this.m_oldPt = this.m_node.m_imageLocation;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.ImageLocation;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            PointF tf1 = this.m_node.m_imageLocation;
            this.m_node.m_imageLocation = this.m_oldPt;
            this.m_node.InvalidateItem();
            this.m_oldPt = tf1;
        }
 
        private Node m_node;
 
        private PointF m_oldPt;
    }

    internal class NodeImagePositionTask : Task {

        private NodeImagePositionTask() {
        }
 
        internal NodeImagePositionTask(Node node) {
            this.m_node = node;
            this.m_oldImagePosition = this.m_node.m_imagePosition;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.ImagePosition;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            ImagePosition position1 = this.m_node.m_imagePosition;
            this.m_node.m_imagePosition = this.m_oldImagePosition;
            this.m_node.InvalidateItem();
            this.m_oldImagePosition = position1;
        }
 
        private Node m_node;
 
        private ImagePosition m_oldImagePosition;
    }

    public class NodeOwnerDrawEventArgs : EventArgs {

        public NodeOwnerDrawEventArgs(Node node, Graphics graphics) {
            this.node = node;
            this.grfx = graphics;
            this.flags = NodeDrawFlags.All;
        }
 
        public NodeDrawFlags Flags {
            get {
                return this.flags;
            }
            set {
                this.flags = value;
            }
        }
 
        public Graphics Graphics {
            get {
                return this.grfx;
            }
        }
 
        public Node Node {
            get {
                return this.node;
            }
        }
 
        private NodeDrawFlags flags;
 
        private Graphics grfx;
 
        private Node node;
    }

    internal class NodePositionTask : Task {

        private NodePositionTask() {
            this.m_apt = new ArrayList();
        }
 
        internal NodePositionTask(Node node, Action code) {
            this.m_apt = new ArrayList();
            this.m_node = node;
            this.m_code = code;
            this.m_oldrc = new RectangleF(this.m_node.RC.Location, this.m_node.RC.Size);
            this.m_oldPtScroll = this.m_node.m_af.ScrollPosition;
            foreach (Link link1 in this.m_node.m_aLinks) {
                if ((link1.m_dst == this.m_node) && link1.m_adjustDst) {
                    this.m_apt.Add(link1.m_aptf[link1.m_aptf.Length - 1]);
                }
                if ((link1.m_org == this.m_node) && link1.m_adjustOrg) {
                    this.m_apt.Add(link1.m_aptf[0]);
                }
            }
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return this.m_code;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            ArrayList list1 = new ArrayList();
            foreach (Link link1 in this.m_node.m_aLinks) {
                if ((link1.m_dst == this.m_node) && link1.m_adjustDst) {
                    list1.Add(link1.m_aptf[link1.m_aptf.Length - 1]);
                }
                if ((link1.m_org == this.m_node) && link1.m_adjustOrg) {
                    list1.Add(link1.m_aptf[0]);
                }
            }
            this.m_node.InvalidateItem();
            RectangleF ef1 = this.m_node.m_rc;
            this.m_node.m_rc = this.m_oldrc;
            this.m_node.AdjustNodeLinks();
            this.m_oldrc = ef1;
            this.m_node.InvalidateItem();
            int num1 = 0;
            foreach (Link link2 in this.m_node.m_aLinks) {
                if ((link2.m_dst == this.m_node) && link2.m_adjustDst) {
                    if (link2.m_line.HVStyle) {
                        link2.ChangePoint(link2.m_aptf.Length - 1, (PointF) this.m_apt[num1]);
                    }
                    link2.m_aptf[link2.m_aptf.Length - 1] = (PointF) this.m_apt[num1];
                    this.m_apt[num1] = list1[num1];
                    num1++;
                    link2.CalcRect();
                    link2.InvalidateItem();
                }
                if ((link2.m_org == this.m_node) && link2.m_adjustOrg) {
                    if (link2.m_line.HVStyle) {
                        link2.ChangePoint(0, (PointF) this.m_apt[num1]);
                    }
                    link2.m_aptf[0] = (PointF) this.m_apt[num1];
                    this.m_apt[num1] = list1[num1];
                    num1++;
                    link2.CalcRect();
                    link2.InvalidateItem();
                }
            }
            Point point1 = this.m_node.m_af.ScrollPosition;
            this.m_node.m_af.ScrollPosition = this.m_oldPtScroll;
            this.m_oldPtScroll = point1;
        }
 
        private ArrayList m_apt;
 
        private Action m_code;
 
        private Node m_node;
 
        private Point m_oldPtScroll;
 
        internal RectangleF m_oldrc;
    }

    public class NodesCollection : ICollection, IEnumerable {

        private NodesCollection() {
            this.m_af = null;
            this.m_arrayList = null;
        }
 
        internal NodesCollection(AddFlow af, ArrayList arrayList) {
            this.m_af = null;
            this.m_arrayList = null;
            this.m_af = af;
            this.m_arrayList = arrayList;
        }
 
        public void Add(Node node) {
            if (node.Existing) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ItemAlreadyOwned));
            }
            this.m_af.AddNode(node, false);
        }
 
        public Node Add(RectangleF rectangle) {
            Node node1 = new Node(rectangle, this.m_af.m_defnode);
            this.m_af.AddNode(node1, false);
            return node1;
        }
 
        public Node Add(PointF location, SizeF size) {
            Node node1 = new Node(new RectangleF(location, size), this.m_af.m_defnode);
            this.m_af.AddNode(node1, false);
            return node1;
        }
 
        public Node Add(float left, float top, float width, float height) {
            if (((left < 0f) || (top < 0f)) || ((width < 0f) || (height < 0f))) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.OutOfAreaError));
            }
            Node node1 = new Node(new RectangleF(left, top, width, height), this.m_af.m_defnode);
            this.m_af.AddNode(node1, false);
            return node1;
        }
 
        public Node Add(float left, float top, float width, float height, string text) {
            if (((left < 0f) || (top < 0f)) || ((width < 0f) || (height < 0f))) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.OutOfAreaError));
            }
            Node node1 = new Node(left, top, width, height, text, this.m_af.m_defnode);
            this.m_af.AddNode(node1, false);
            return node1;
        }
 
        public void Clear() {
            this.m_af.DeleteNodes();
        }
 
        private bool Contains(object o) {
            return this.m_arrayList.Contains(o);
        }
 
        public void CopyTo(Array array, int arrayIndex) {
            this.m_arrayList.CopyTo(array, arrayIndex);
        }
 
        public IEnumerator GetEnumerator() {
            return this.m_arrayList.GetEnumerator();
        }
 
        public int IndexOf(object o) {
            return this.m_arrayList.IndexOf(o);
        }
 
        public void Remove(Node node) {
            this.m_af.m_undo.BeginActionInternal(Action.NodeRemove);
            this.m_af.RemoveNode(node);
            this.m_af.UpdateRect();
            this.m_af.m_undo.EndActionInternal();
        }
 
        public int Count {
            get {
                return this.m_arrayList.Count;
            }
        }
 
        public bool IsFixedSize {
            get {
                return false;
            }
        }
 
        public bool IsReadOnly {
            get {
                return false;
            }
        }
 
        public bool IsSynchronized {
            get {
                return this.m_arrayList.IsSynchronized;
            }
        }
 
        public Node this[int index] {
            get {
                if ((index < 0) || (index > (this.Count - 1))) {
                    throw new IndexOutOfRangeException();
                }
                return (Node) this.m_arrayList[index];
            }
        }
 
        public object SyncRoot {
            get {
                return this.m_arrayList.SyncRoot;
            }
        }
 
        private AddFlow m_af;
 
        private ArrayList m_arrayList;
    }

    internal class NodeShadowTask : Task {

        private NodeShadowTask() {
        }
 
        internal NodeShadowTask(Node node) {
            this.m_node = node;
            this.m_oldShadow = (Shadow) this.m_node.m_shadow.Clone();
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Shadow;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Shadow shadow1 = (Shadow) this.m_node.m_shadow.Clone();
            this.m_node.InvalidateItem();
            this.m_node.SetShadow2(this.m_oldShadow);
            this.m_oldShadow = shadow1;
        }
 
        private Node m_node;
 
        private Shadow m_oldShadow;
    }

    internal class NodeShapeTask : Task {

        private NodeShapeTask() {
        }
 
        internal NodeShapeTask(Node node) {
            this.m_node = node;
            this.m_oldShape = (Shape) this.m_node.m_shape.Clone();
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Shape;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Shape shape1 = (Shape) this.m_node.m_shape.Clone();
            this.m_node.SetShape2(this.m_oldShape);
            this.m_oldShape = shape1;
        }
 
        private Node m_node;
 
        private Shape m_oldShape;
    }

    internal class NodeTextMarginTask : Task {

        private NodeTextMarginTask() {
        }
 
        internal NodeTextMarginTask(Node node) {
            this.m_node = node;
            this.m_oldValue = this.m_node.m_textMargin;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.TextMargin;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Size size1 = this.m_node.m_textMargin;
            this.m_node.SetTextMargin2(this.m_oldValue);
            this.m_oldValue = size1;
        }
 
        private Node m_node;
 
        private Size m_oldValue;
    }

    internal class NodeTextTask : Task {

        private NodeTextTask() {
        }
 
        internal NodeTextTask(Node node) {
            this.m_node = node;
            this.m_oldValue = this.m_node.m_text;
            this.m_oldrc = new RectangleF(this.m_node.RC.Location, this.m_node.RC.Size);
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Text;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            string text1 = this.m_node.m_text;
            this.m_node.SetText2(this.m_oldValue);
            this.m_oldValue = text1;
            if (this.m_node.m_autoSize == AutoSize.NodeToText) {
                this.m_node.InvalidateItem();
                RectangleF ef1 = this.m_node.m_rc;
                this.m_node.m_rc = this.m_oldrc;
                this.m_node.AdjustNodeLinks();
                this.m_oldrc = ef1;
                this.m_node.InvalidateItem();
            }
        }
 
        private Node m_node;
 
        private RectangleF m_oldrc;
 
        private string m_oldValue;
    }

    internal class NodeTooltipTask : Task {

        private NodeTooltipTask() {
        }
 
        internal NodeTooltipTask(Node node) {
            this.m_node = node;
            this.m_oldValue = this.m_node.m_tooltip;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Tooltip;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            string text1 = this.m_node.m_tooltip;
            this.m_node.m_tooltip = this.m_oldValue;
            this.m_oldValue = text1;
        }
 
        private Node m_node;
 
        private string m_oldValue;
    }

    internal class NodeTrimmingTask : Task {

        private NodeTrimmingTask() {
        }
 
        internal NodeTrimmingTask(Node node) {
            this.m_node = node;
            this.m_oldTrimming = this.m_node.m_trimming;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Trimming;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            StringTrimming trimming1 = this.m_node.m_trimming;
            this.m_node.m_trimming = this.m_oldTrimming;
            this.m_node.InvalidateItem();
            this.m_oldTrimming = trimming1;
        }
 
        private Node m_node;
 
        private StringTrimming m_oldTrimming;
    }

    internal class RemoveAtIndexEventArgs : EventArgs {

        internal RemoveAtIndexEventArgs(int index) {
            this.index = index;
        }
 
        internal int Index {
            get {
                return this.index;
            }
        }
 
        private int index;
    }

    public enum RemovePointAngle {
        // Fields
        Large = 3,
        Medium = 2,
        None = 0,
        Small = 1
    }
 
    internal class ReverseLinkTask : Task {
 
        private ReverseLinkTask() {
        }
 
        internal ReverseLinkTask(Link link) {
            this.m_link = link;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Reverse;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            this.m_link.Reverse2();
        }
 
        private Link m_link;
    }

    public enum ScrollbarsDisplayMode {
        // Fields
        AddControlSize = 0,
        SizeOfDiagramOnly = 1
    }
 
    internal class SetLinkDstTask : Task {

        private SetLinkDstTask() {
        }
 
        internal SetLinkDstTask(Link link) {
            this.m_link = link;
            this.m_dst = this.m_link.m_dst;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Dst;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Node node1 = this.m_link.m_dst;
            this.m_link.SetDst2(this.m_dst);
            this.m_dst = node1;
        }
 
        private Node m_dst;
 
        private Link m_link;
    }

    internal class SetLinkOrgTask : Task {

        private SetLinkOrgTask() {
        }
 
        internal SetLinkOrgTask(Link link) {
            this.m_link = link;
            this.m_org = this.m_link.m_org;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Org;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            Node node1 = this.m_link.m_org;
            this.m_link.SetOrg2(this.m_org);
            this.m_org = node1;
        }
 
        private Link m_link;
 
        private Node m_org;
    }

    [Serializable, TypeConverter(typeof(ShadowConverter))]
    public class Shadow : ICloneable {
        internal delegate void AfterChangeEventHandler(object sender, EventArgs e);
 
        internal delegate void BeforeChangeEventHandler(object sender, EventArgs e);
 
        static Shadow() {
            Shadow.None = new Shadow();
        }
 
        public Shadow() {
            this.Reset();
        }
 
        public Shadow(string format) {
            char[] chArray1 = new char[1] { ';' } ;
            string[] textArray1 = format.Split(chArray1);
            string text1 = textArray1[2].Trim();
            text1 = text1.Substring(1, text1.Length - 2);
            chArray1 = new char[1] { ',' } ;
            string[] textArray2 = text1.Split(chArray1);
            text1 = textArray2[0].Trim();
            chArray1 = new char[1] { '=' } ;
            string[] textArray3 = text1.Split(chArray1);
            text1 = textArray2[1].Trim();
            chArray1 = new char[1] { '=' } ;
            string[] textArray4 = text1.Split(chArray1);
            Size size1 = new Size(Convert.ToInt32(textArray3[1].Trim()), Convert.ToInt32(textArray4[1].Trim()));
            if ((size1.Width < 0) || (size1.Height < 0)) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ShadowSizeCannotBeNegative));
            }
            this.m_size = size1;
            this.m_style = (ShadowStyle) Enum.Parse(typeof(ShadowStyle), textArray1[0].Trim(), false);
            this.m_color = Color.FromName(textArray1[1].Trim());
        }
 
        public Shadow(ShadowStyle style, Color color, Size size) {
            if ((size.Width < 0) || (size.Height < 0)) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ShadowSizeCannotBeNegative));
            }
            this.m_style = style;
            this.m_color = color;
            this.m_size = size;
        }
 
        public object Clone() {
            return new Shadow(this.m_style, this.m_color, this.m_size);
        }
 
        internal void Draw(Graphics grfx, GraphicsPath path) {
            if (path != null) {
                Matrix matrix1 = new Matrix();
                switch (this.m_style) {
                case ShadowStyle.RightBottom: {
                    matrix1.Translate((float) this.m_size.Width, (float) this.m_size.Height);
                    break;
                }
                case ShadowStyle.RightTop: {
                    matrix1.Translate((float) this.m_size.Width, (float) -this.m_size.Height);
                    break;
                }
                case ShadowStyle.LeftTop: {
                    matrix1.Translate((float) -this.m_size.Width, (float) -this.m_size.Height);
                    break;
                }
                case ShadowStyle.LeftBottom: {
                    matrix1.Translate((float) -this.m_size.Width, (float) this.m_size.Height);
                    break;
                }
                }
                path.Transform(matrix1);
                Brush brush1 = new SolidBrush(this.m_color);
                grfx.FillPath(brush1, path);
                matrix1.Reset();
                switch (this.m_style) {
                case ShadowStyle.RightBottom: {
                    matrix1.Translate((float) -this.m_size.Width, (float) -this.m_size.Height);
                    break;
                }
                case ShadowStyle.RightTop: {
                    matrix1.Translate((float) -this.m_size.Width, (float) this.m_size.Height);
                    break;
                }
                case ShadowStyle.LeftTop: {
                    matrix1.Translate((float) this.m_size.Width, (float) this.m_size.Height);
                    break;
                }
                case ShadowStyle.LeftBottom: {
                    matrix1.Translate((float) this.m_size.Width, (float) -this.m_size.Height);
                    break;
                }
                }
                path.Transform(matrix1);
            }
        }
 
        internal bool Equals(object obj) {
            if (obj is Shadow) {
                Shadow shadow1 = (Shadow) obj;
                if (((this.m_style == shadow1.m_style) && (this.m_color == shadow1.m_color)) && (this.m_size.Width == shadow1.m_size.Width)) {
                    return (this.m_size.Height == shadow1.m_size.Height);
                }
            }
            return false;
        }
 
        internal bool IsDefault() {
            if (((this.m_style == ShadowStyle.None) && (this.m_color == SystemColors.GrayText)) && (this.m_size.Width == (SystemInformation.IconSize.Width / 8))) {
                return (this.m_size.Height == (SystemInformation.IconSize.Height / 8));
            }
            return false;
        }
 
        internal virtual void OnAfterChange(EventArgs e) {
            if (this.AfterChange != null) {
                this.AfterChange(this, e);
            }
        }
 
        internal virtual void OnBeforeChange(EventArgs e) {
            if (this.BeforeChange != null) {
                this.BeforeChange(this, e);
            }
        }
 
        internal void Reset() {
            this.m_style = ShadowStyle.None;
            this.m_color = SystemColors.GrayText;
            this.m_size = new Size(SystemInformation.IconSize.Width / 8, SystemInformation.IconSize.Height / 8);
        }
 
        public void ResetColor() {
            this.m_color = SystemColors.GrayText;
        }
 
        public void ResetSize() {
            this.m_size = new Size(SystemInformation.IconSize.Width / 4, SystemInformation.IconSize.Height / 4);
        }
 
        public bool ShouldSerializeColor() {
            return (this.m_color != SystemColors.GrayText);
        }
 
        public bool ShouldSerializeSize() {
            if (this.m_size.Width == (SystemInformation.IconSize.Width / 4)) {
                return (this.m_size.Height != (SystemInformation.IconSize.Height / 4));
            }
            return true;
        }
 
        public override string ToString() {
            StringBuilder builder1 = new StringBuilder("");
            builder1.Append(this.m_style.ToString() + "; ");
            string text1 = this.m_color.IsNamedColor ? this.m_color.Name : string.Concat(new object[5]);
            builder1.Append(text1 + "; ");
            builder1.Append(this.m_size.ToString());
            return builder1.ToString();
        }
 
        [Description("Returns/sets the shadow color."), NotifyParentProperty(true)]
        public Color Color {
            get {
                return this.m_color;
            }
            set {
                if (this.m_color != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_color = value;
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), Description("Returns/sets the horizontal and vertical shadow offset.")]
        public Size Size {
            get {
                return this.m_size;
            }
            set {
                if ((value.Width < 0) || (value.Height < 0)) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ShadowSizeCannotBeNegative));
                }
                if (this.m_size != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_size = value;
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), DefaultValue(0), Description("Returns/sets the shadow style.")]
        public ShadowStyle Style {
            get {
                return this.m_style;
            }
            set {
                if (this.m_style != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_style = value;
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }
 
        internal event Shadow.AfterChangeEventHandler AfterChange;
 
        internal event Shadow.BeforeChangeEventHandler BeforeChange;
 
        //private Shadow.AfterChangeEventHandler AfterChange;
 
        //private Shadow.BeforeChangeEventHandler BeforeChange;
 
        internal Color m_color;
 
        internal Size m_size;
 
        internal ShadowStyle m_style;
 
        public static readonly Shadow None;
    }

    internal class ShadowConverter : ExpandableObjectConverter {

        public ShadowConverter() {
        }
 
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
 
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(InstanceDescriptor)) {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }
 
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value) {
            if (value is string) {
                try {
                    return new Shadow((string) value);
                }
                catch {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ArgumentsNotValid));
                }
            }
            return base.ConvertFrom(context, info, value);
        }
 
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (value is Shadow) {
                Shadow shadow1 = (Shadow) value;
                if (destinationType == typeof(string)) {
                    return shadow1.ToString();
                }
                if (destinationType == typeof(InstanceDescriptor)) {
                    object[] objArray2 = new object[3] { shadow1.Style, shadow1.Color, shadow1.Size } ;
                    object[] objArray1 = objArray2;
                    Type[] typeArray2 = new Type[3] { typeof(ShadowStyle), typeof(Color), typeof(Size) } ;
                    Type[] typeArray1 = typeArray2;
                    ConstructorInfo info1 = typeof(Shadow).GetConstructor(typeArray1);
                    return new InstanceDescriptor(info1, objArray1);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
 
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
            return new Shadow((ShadowStyle) propertyValues["Style"], (Color) propertyValues["Color"], (Size) propertyValues["Size"]);
        }
 
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
            return true;
        }
    }

    public enum ShadowStyle {
        // Fields
        LeftBottom = 4,
        LeftTop = 3,
        None = 0,
        RightBottom = 1,
        RightTop = 2
    }
 
    [Serializable, TypeConverter(typeof(ShapeConverter))]
    public class Shape : ICloneable {

        internal delegate void AfterChangeEventHandler(object sender, EventArgs e);
 
        internal delegate void BeforeChangeEventHandler(object sender, EventArgs e);
 
        public Shape() {
            this.m_style = ShapeStyle.Ellipse;
            this.m_orientation = ShapeOrientation.so_0;
            this.m_customGraphicsPath = null;
        }
 
        public Shape(string format) {
            char[] chArray1 = new char[1] { ';' } ;
            string[] textArray1 = format.Split(chArray1);
            this.m_style = (ShapeStyle) Enum.Parse(typeof(ShapeStyle), textArray1[0].Trim(), false);
            this.m_orientation = (ShapeOrientation) Enum.Parse(typeof(ShapeOrientation), textArray1[1].Trim(), false);
            this.m_customGraphicsPath = null;
        }
 
        public Shape(ShapeStyle style, ShapeOrientation orientation) {
            this.m_style = style;
            this.m_orientation = orientation;
            this.m_customGraphicsPath = null;
        }
 
        public object Clone() {
            Shape shape1 = new Shape(this.m_style, this.m_orientation);
            if (this.m_customGraphicsPath != null) {
                shape1.m_customGraphicsPath = (GraphicsPath) this.m_customGraphicsPath.Clone();
                return shape1;
            }
            shape1.m_customGraphicsPath = null;
            return shape1;
        }
 
        internal bool Equals(object obj) {
            if (obj is Shape) {
                Shape shape1 = (Shape) obj;
                if (this.m_style == shape1.m_style) {
                    return (this.m_orientation == shape1.m_orientation);
                }
            }
            return false;
        }
 
        internal GraphicsPath GetPathOfShape(RectangleF rc) {
            GraphicsPath path2;
            float single1;
            GraphicsPath path1 = null;
            Matrix matrix1 = new Matrix();
            if (this.m_style == ShapeStyle.Custom) {
                if (this.m_customGraphicsPath != null) {
                    path1 = (GraphicsPath) this.m_customGraphicsPath.Clone();
                    RectangleF ef1 = path1.GetBounds();
                    matrix1.Scale(1f / ef1.Right, 1f / ef1.Bottom);
                    path1.Transform(matrix1);
                    matrix1.Reset();
                }
                else {
                    path1 = new GraphicsPath();
                    path1.AddRectangle(new RectangleF(0f, 0f, 1f, 1f));
                }
            }
            else {
                path1 = Shape.GetPredefinedPath(this.m_style);
            }
            if (path1 == null) {
                return path1;
            }
            ShapeStyle style1 = this.m_style;
            if (style1 <= ShapeStyle.Hexagon) {
                if ((style1 == ShapeStyle.Document) || (style1 == ShapeStyle.Hexagon)) {
                    goto Label_00E3;
                }
                goto Label_010B;
            }
            switch (style1) {
            case ShapeStyle.OrGate:
            case ShapeStyle.Pentagon:
            case ShapeStyle.Preparation:
            case ShapeStyle.PunchedTape:
            case ShapeStyle.MultiDocument: {
                goto Label_00E3;
            }
            case ShapeStyle.PredefinedProcess:
            case ShapeStyle.Process:
            case ShapeStyle.ProcessIso9000: {
                goto Label_010B;
            }
            default: {
                goto Label_010B;
            }
            }
            Label_00E3:
                path2 = (GraphicsPath) path1.Clone();
            path2.Flatten(new Matrix(), 0.05f);
            RectangleF ef2 = path2.GetBounds();
            goto Label_0112;
            Label_010B:
                ef2 = path1.GetBounds();
            Label_0112:
                ef2.Width = ef2.Right;
            ef2.Height = ef2.Bottom;
            ef2.Y = single1 = 0f;
            ef2.X = single1;
            switch (this.m_orientation) {
            case ShapeOrientation.so_90: {
                matrix1.RotateAt(-90f, Misc.CenterPoint(ef2));
                break;
            }
            case ShapeOrientation.so_180: {
                matrix1.RotateAt(-180f, Misc.CenterPoint(ef2));
                break;
            }
            case ShapeOrientation.so_270: {
                matrix1.RotateAt(-270f, Misc.CenterPoint(ef2));
                break;
            }
            }
            path1.Transform(matrix1);
            matrix1.Reset();
            matrix1.Translate(rc.Left, rc.Top);
            matrix1.Scale(rc.Width, rc.Height);
            path1.Transform(matrix1);
            return path1;
        }
 
        private static PointF[] GetPolyPoint(ShapeStyle style, RectangleF rc) {
            float single1;
            float single2;
            float single4;
            PointF[] tfArray1;
            ShapeStyle style1 = style;
            if (style1 <= ShapeStyle.ProcessIso9000) {
                switch (style1) {
                case ShapeStyle.Card: {
                    tfArray1 = new PointF[5];
                    tfArray1[0].X = rc.Left + (rc.Width / 6f);
                    tfArray1[0].Y = rc.Top;
                    tfArray1[1].X = rc.Left + rc.Width;
                    tfArray1[1].Y = rc.Top;
                    tfArray1[2].X = rc.Left + rc.Width;
                    tfArray1[2].Y = rc.Top + rc.Height;
                    tfArray1[3].X = rc.Left;
                    tfArray1[3].Y = rc.Top + rc.Height;
                    tfArray1[4].X = rc.Left;
                    tfArray1[4].Y = rc.Top + (rc.Height / 6f);
                    return tfArray1;
                }
                case ShapeStyle.Collate: {
                    tfArray1 = new PointF[5];
                    tfArray1[0].X = rc.Left;
                    tfArray1[0].Y = rc.Top;
                    tfArray1[1].X = rc.Left + rc.Width;
                    tfArray1[1].Y = rc.Top;
                    tfArray1[2].X = rc.Left;
                    tfArray1[2].Y = rc.Top + rc.Height;
                    tfArray1[3].X = rc.Left + rc.Width;
                    tfArray1[3].Y = rc.Top + rc.Height;
                    tfArray1[4].X = rc.Left;
                    tfArray1[4].Y = rc.Top;
                    return tfArray1;
                }
                case ShapeStyle.Connector:
                case ShapeStyle.Custom:
                case ShapeStyle.InternalStorage:
                case ShapeStyle.MagneticDisk:
                case ShapeStyle.Merge:
                case ShapeStyle.MultiDocument:
                case ShapeStyle.Or:
                case ShapeStyle.OrGate:
                case ShapeStyle.PredefinedProcess:
                case ShapeStyle.Process: {
                    goto Label_0A2A;
                }
                case ShapeStyle.Data: {
                    tfArray1 = new PointF[4];
                    tfArray1[0].X = rc.Left + (rc.Width / 4f);
                    tfArray1[0].Y = rc.Top;
                    tfArray1[1].X = rc.Left + rc.Width;
                    tfArray1[1].Y = rc.Top;
                    tfArray1[2].X = (rc.Left + rc.Width) - (rc.Width / 4f);
                    tfArray1[2].Y = rc.Top + rc.Height;
                    tfArray1[3].X = rc.Left;
                    tfArray1[3].Y = rc.Top + rc.Height;
                    return tfArray1;
                }
                case ShapeStyle.Decision:
                case ShapeStyle.Hexagon:
                case ShapeStyle.Losange:
                case ShapeStyle.Octogon:
                case ShapeStyle.Pentagon:
                case ShapeStyle.Preparation: {
                    goto Label_008F;
                }
                case ShapeStyle.ManualInput: {
                    tfArray1 = new PointF[4];
                    tfArray1[0].X = rc.Left;
                    tfArray1[0].Y = rc.Top + (rc.Height / 5f);
                    tfArray1[1].X = rc.Left + rc.Width;
                    tfArray1[1].Y = rc.Top;
                    tfArray1[2].X = rc.Left + rc.Width;
                    tfArray1[2].Y = rc.Top + rc.Height;
                    tfArray1[3].X = rc.Left;
                    tfArray1[3].Y = rc.Top + rc.Height;
                    return tfArray1;
                }
                case ShapeStyle.ManualOperation: {
                    tfArray1 = new PointF[4];
                    tfArray1[0].X = rc.Left;
                    tfArray1[0].Y = rc.Top;
                    tfArray1[1].X = rc.Left + rc.Width;
                    tfArray1[1].Y = rc.Top + (rc.Height / 5f);
                    tfArray1[2].X = rc.Left + rc.Width;
                    tfArray1[2].Y = (rc.Top + rc.Height) - (rc.Height / 5f);
                    tfArray1[3].X = rc.Left;
                    tfArray1[3].Y = rc.Top + rc.Height;
                    return tfArray1;
                }
                case ShapeStyle.OffPageConnection: {
                    tfArray1 = new PointF[5];
                    tfArray1[0].X = rc.Left + rc.Width;
                    tfArray1[0].Y = rc.Top + (rc.Height / 2f);
                    tfArray1[1].X = rc.Left + (rc.Width / 2f);
                    tfArray1[1].Y = rc.Top + rc.Height;
                    tfArray1[2].X = rc.Left;
                    tfArray1[2].Y = rc.Top + rc.Height;
                    tfArray1[3].X = rc.Left;
                    tfArray1[3].Y = rc.Top;
                    tfArray1[4].X = rc.Left + (rc.Width / 2f);
                    tfArray1[4].Y = rc.Top;
                    return tfArray1;
                }
                case ShapeStyle.ProcessIso9000: {
                    tfArray1 = new PointF[6];
                    tfArray1[0].X = rc.Left + rc.Width;
                    tfArray1[0].Y = rc.Top + (rc.Height / 2f);
                    tfArray1[1].X = (rc.Left + rc.Width) - (rc.Width / 4f);
                    tfArray1[1].Y = rc.Top + rc.Height;
                    tfArray1[2].X = rc.Left;
                    tfArray1[2].Y = rc.Top + rc.Height;
                    tfArray1[3].X = rc.Left + (rc.Width / 4f);
                    tfArray1[3].Y = rc.Top + (rc.Height / 2f);
                    tfArray1[4].X = rc.Left;
                    tfArray1[4].Y = rc.Top;
                    tfArray1[5].X = (rc.Left + rc.Width) - (rc.Width / 4f);
                    tfArray1[5].Y = rc.Top;
                    return tfArray1;
                }
                }
                goto Label_0A2A;
            }
            if (style1 == ShapeStyle.SequentialAccessStorage) {
                tfArray1 = new PointF[0x18];
                single4 = 0.01745333f;
                single1 = -45f;
                for (int num3 = 0; num3 < 0x16; num3++) {
                    single2 = single1 * single4;
                    tfArray1[num3].X = (rc.Left + (rc.Width / 2f)) + ((rc.Width / 2f) * ((float) Math.Cos((double) single2)));
                    tfArray1[num3].Y = (rc.Top + (rc.Height / 2f)) - ((rc.Height / 2f) * ((float) Math.Sin((double) single2)));
                    single1 += 15f;
                }
                tfArray1[0x16].X = rc.Left + rc.Width;
                tfArray1[0x16].Y = tfArray1[0x15].Y;
                tfArray1[0x17].X = rc.Left + rc.Width;
                tfArray1[0x17].Y = tfArray1[0].Y;
                return tfArray1;
            }
            if (style1 != ShapeStyle.Sort) {
                goto Label_0A2A;
            }
            Label_008F:
                single1 = 0f;
            int num1 = 0;
            style1 = style;
            if (style1 <= ShapeStyle.Losange) {
                if (style1 == ShapeStyle.Decision) {
                    goto Label_00E0;
                }
                switch (style1) {
                case ShapeStyle.Hexagon: {
                    goto Label_0104;
                }
                case ShapeStyle.InternalStorage: {
                    goto Label_0128;
                }
                case ShapeStyle.Losange: {
                    goto Label_00E0;
                }
                }
                goto Label_0128;
            }
            if (style1 == ShapeStyle.Octogon) {
                num1 = 8;
                tfArray1 = new PointF[num1];
                goto Label_012E;
            }
            switch (style1) {
            case ShapeStyle.Pentagon: {
                num1 = 5;
                tfArray1 = new PointF[num1];
                single1 = 90f;
                goto Label_012E;
            }
            case ShapeStyle.PredefinedProcess: {
                goto Label_0128;
            }
            case ShapeStyle.Preparation: {
                goto Label_0104;
            }
            default: {
                if (style1 != ShapeStyle.Sort) {
                    goto Label_0128;
                }
                goto Label_00E0;
            }
            }
            Label_00E0:
                num1 = 4;
            tfArray1 = new PointF[num1];
            goto Label_012E;
            Label_0104:
                num1 = 6;
            tfArray1 = new PointF[num1];
            single1 = 90f;
            goto Label_012E;
            Label_0128:
                num1 = 0;
            tfArray1 = null;
            Label_012E:
                single4 = 0.01745333f;
            float single3 = 360 / num1;
            for (int num2 = 0; num2 < num1; num2++) {
                single2 = single1 * single4;
                tfArray1[num2].X = (rc.Left + (rc.Width / 2f)) + ((rc.Width / 2f) * ((float) Math.Sin((double) single2)));
                tfArray1[num2].Y = (rc.Top + (rc.Height / 2f)) + ((rc.Height / 2f) * ((float) Math.Cos((double) single2)));
                single1 += single3;
            }
            return tfArray1;
            Label_0A2A:
                return null;
        }
 
        internal static GraphicsPath GetPredefinedPath(ShapeStyle style) {
            GraphicsPath path1 = new GraphicsPath();
            RectangleF ef1 = new RectangleF(0f, 0f, 1f, 1f);
            switch (style) {
            case ShapeStyle.AlternateProcess:
            case ShapeStyle.RoundRect: {
                ef1.Width *= 16f;
                ef1.Height *= 16f;
                Shape.RoundedRectangle(path1, ef1, new SizeF(2f, 2f));
                Matrix matrix1 = new Matrix();
                if ((ef1.Width != 0f) && (ef1.Height != 0f)) {
                    matrix1.Scale(1f / ef1.Width, 1f / ef1.Height);
                }
                path1.Transform(matrix1);
                return path1;
            }
            case ShapeStyle.Connector:
            case ShapeStyle.Ellipse:
            case ShapeStyle.Or:
            case ShapeStyle.SummingJunction: {
                path1.AddEllipse(ef1);
                if (style != ShapeStyle.Or) {
                    if (style == ShapeStyle.SummingJunction) {
                        PointF tf2 = Misc.CenterPoint(ef1);
                        float single1 = (ef1.Width / 2f) * 0.707f;
                        float single2 = (ef1.Height / 2f) * 0.707f;
                        path1.AddLine((float) (tf2.X - single1), (float) (tf2.Y - single2), (float) (tf2.X + single1), (float) (tf2.Y + single2));
                        path1.CloseFigure();
                        path1.AddLine((float) (tf2.X + single1), (float) (tf2.Y - single2), (float) (tf2.X - single1), (float) (tf2.Y + single2));
                    }
                    return path1;
                }
                PointF tf1 = Misc.CenterPoint(ef1);
                path1.AddLine(tf1.X, ef1.Top, tf1.X, ef1.Bottom);
                path1.CloseFigure();
                path1.AddLine(ef1.Left, tf1.Y, ef1.Right, tf1.Y);
                return path1;
            }
            case ShapeStyle.Delay: {
                if ((ef1.Width > 0f) && (ef1.Height > 0f)) {
                    path1.AddLine((float) (ef1.Left + (ef1.Width / 2f)), ef1.Bottom, ef1.Left, ef1.Bottom);
                    path1.AddLine(ef1.Left, ef1.Bottom, ef1.Left, ef1.Top);
                    path1.AddLine(ef1.Left, ef1.Top, (float) (ef1.Left + (ef1.Width / 2f)), ef1.Top);
                    path1.AddArc(ef1.Left, ef1.Top, ef1.Width, ef1.Height, (float) 270f, (float) 180f);
                }
                return path1;
            }
            case ShapeStyle.DirectAccessStorage: {
                if ((ef1.Width > 0f) && (ef1.Height > 0f)) {
                    RectangleF ef3 = new RectangleF(ef1.Location, ef1.Size);
                    ef3.Width = ef1.Width / 4f;
                    path1.AddEllipse(ef3);
                    path1.AddLine((float) (ef1.Left + (ef1.Width / 8f)), ef1.Top, (float) (ef1.Right - (ef1.Width / 4f)), ef1.Top);
                    path1.AddArc((float) (ef1.Right - (ef1.Width / 4f)), ef1.Top, (float) (ef1.Width / 4f), ef1.Height, (float) -90f, (float) 180f);
                    path1.AddLine((float) (ef1.Right - (ef1.Width / 4f)), ef1.Bottom, (float) (ef1.Left + (ef1.Width / 8f)), ef1.Bottom);
                    path1.FillMode = FillMode.Winding;
                }
                return path1;
            }
            case ShapeStyle.Display: {
                if ((ef1.Width > 0f) && (ef1.Height > 0f)) {
                    path1.AddLine((float) (ef1.Right - (ef1.Width / 4f)), ef1.Bottom, (float) (ef1.Left + (ef1.Width / 4f)), ef1.Bottom);
                    path1.AddLine((float) (ef1.Left + (ef1.Width / 4f)), ef1.Bottom, ef1.Left, (float) (ef1.Top + (ef1.Height / 2f)));
                    path1.AddLine(ef1.Left, (float) (ef1.Top + (ef1.Height / 2f)), (float) (ef1.Left + (ef1.Width / 4f)), ef1.Top);
                    path1.AddLine((float) (ef1.Left + (ef1.Width / 4f)), ef1.Top, (float) (ef1.Right - (ef1.Width / 4f)), ef1.Top);
                    path1.AddArc((float) (ef1.Right - (ef1.Width / 2f)), ef1.Top, (float) (ef1.Width / 2f), ef1.Height, (float) 270f, (float) 180f);
                }
                return path1;
            }
            case ShapeStyle.Document: {
                Shape.MakeDocumentPath(path1, ef1);
                return path1;
            }
            case ShapeStyle.Extract:
            case ShapeStyle.Triangle: {
                path1.AddLine(ef1.Left, ef1.Bottom, ef1.Right, ef1.Bottom);
                path1.AddLine(ef1.Right, ef1.Bottom, (float) (ef1.Left + (ef1.Width / 2f)), ef1.Top);
                path1.CloseFigure();
                return path1;
            }
            case ShapeStyle.InternalStorage:
            case ShapeStyle.PredefinedProcess:
            case ShapeStyle.Process:
            case ShapeStyle.Rectangle:
            case ShapeStyle.RectEdgeBump:
            case ShapeStyle.RectEdgeEtched:
            case ShapeStyle.RectEdgeRaised:
            case ShapeStyle.RectEdgeSunken: {
                path1.AddRectangle(ef1);
                if (style != ShapeStyle.PredefinedProcess) {
                    if (style == ShapeStyle.InternalStorage) {
                        path1.FillMode = FillMode.Winding;
                        path1.AddRectangle(ef1);
                        path1.AddLine((float) (ef1.Left + (ef1.Width / 8f)), ef1.Top, (float) (ef1.Left + (ef1.Width / 8f)), ef1.Bottom);
                        path1.CloseFigure();
                        path1.AddLine(ef1.Left, (float) (ef1.Top + (ef1.Height / 8f)), ef1.Right, (float) (ef1.Top + (ef1.Height / 8f)));
                    }
                    return path1;
                }
                path1.FillMode = FillMode.Winding;
                path1.AddRectangle(ef1);
                path1.AddLine((float) (ef1.Left + (ef1.Width / 8f)), ef1.Top, (float) (ef1.Left + (ef1.Width / 8f)), ef1.Bottom);
                path1.CloseFigure();
                path1.AddLine((float) (ef1.Right - (ef1.Width / 8f)), ef1.Top, (float) (ef1.Right - (ef1.Width / 8f)), ef1.Bottom);
                return path1;
            }
            case ShapeStyle.MagneticDisk: {
                if ((ef1.Width > 0f) && (ef1.Height > 0f)) {
                    RectangleF ef2 = new RectangleF(ef1.Location, ef1.Size);
                    ef2.Height = ef1.Height / 4f;
                    path1.AddEllipse(ef2);
                    path1.AddLine(ef1.Right, (float) (ef1.Top + (ef1.Height / 8f)), ef1.Right, (float) (ef1.Bottom - (ef1.Height / 4f)));
                    path1.AddArc(ef1.Left, (float) (ef1.Bottom - (ef1.Height / 4f)), ef1.Width, (float) (ef1.Height / 4f), (float) 0f, (float) 180f);
                    path1.AddLine(ef1.Left, (float) (ef1.Bottom - (ef1.Height / 4f)), ef1.Left, (float) (ef1.Top + (ef1.Height / 8f)));
                    path1.FillMode = FillMode.Winding;
                }
                return path1;
            }
            case ShapeStyle.Merge: {
                path1.AddLine(ef1.Left, ef1.Top, ef1.Right, ef1.Top);
                path1.AddLine(ef1.Right, ef1.Top, (float) (ef1.Left + (ef1.Width / 2f)), ef1.Bottom);
                path1.CloseFigure();
                return path1;
            }
            case ShapeStyle.MultiDocument: {
                Shape.MakeMultiDocumentPath(path1, ef1);
                return path1;
            }
            case ShapeStyle.OrGate: {
                Shape.MakeOrGatePath(path1, ef1);
                return path1;
            }
            case ShapeStyle.PunchedTape: {
                Shape.MakePunchedTapePath(path1, ef1);
                return path1;
            }
            case ShapeStyle.StoredData: {
                if ((ef1.Width > 0f) && (ef1.Height > 0f)) {
                    path1.AddLine(ef1.Left, ef1.Top, (float) (ef1.Right - (ef1.Width / 4f)), ef1.Top);
                    path1.AddArc((float) (ef1.Right - (ef1.Width / 2f)), ef1.Top, (float) (ef1.Width / 2f), ef1.Height, (float) 270f, (float) 180f);
                    path1.AddLine((float) (ef1.Right - (ef1.Width / 4f)), ef1.Bottom, ef1.Left, ef1.Bottom);
                    path1.AddArc((float) (ef1.Left - (ef1.Width / 4f)), ef1.Top, (float) (ef1.Width / 2f), ef1.Height, (float) 90f, (float) -180f);
                }
                return path1;
            }
            case ShapeStyle.Sort: {
                PointF[] tfArray1 = Shape.GetPolyPoint(style, ef1);
                path1.AddLines(tfArray1);
                path1.AddLine(tfArray1[1], tfArray1[3]);
                path1.CloseFigure();
                return path1;
            }
            case ShapeStyle.Termination: {
                if ((ef1.Width > 0f) && (ef1.Height > 0f)) {
                    path1.AddArc((float) (ef1.Left + (ef1.Width / 2f)), ef1.Top, (float) (ef1.Width / 2f), ef1.Height, (float) -90f, (float) 180f);
                    path1.AddArc(ef1.Left, ef1.Top, (float) (ef1.Width / 2f), ef1.Height, (float) 90f, (float) 180f);
                    path1.CloseFigure();
                }
                return path1;
            }
            case ShapeStyle.Transport: {
                Shape.MakeTransportPath(path1, ef1);
                return path1;
            }
            case ShapeStyle.TriangleRectangle: {
                path1.AddLine(ef1.Left, ef1.Top, ef1.Left, ef1.Bottom);
                path1.AddLine(ef1.Left, ef1.Bottom, ef1.Right, ef1.Bottom);
                path1.CloseFigure();
                return path1;
            }
            }
            path1.AddLines(Shape.GetPolyPoint(style, ef1));
            path1.CloseFigure();
            return path1;
        }
 
        internal bool HitTest(PointF pt, RectangleF rc) {
            bool flag1 = false;
            if (rc.Contains(pt)) {
                if ((this.m_style == ShapeStyle.Custom) && (this.m_customGraphicsPath == null)) {
                    return true;
                }
                GraphicsPath path1 = this.GetPathOfShape(rc);
                if (path1.IsVisible(pt)) {
                    flag1 = true;
                }
            }
            return flag1;
        }
 
        internal bool IsDefault() {
            if ((this.m_style == ShapeStyle.Ellipse) && (this.m_orientation == ShapeOrientation.so_0)) {
                return (this.m_customGraphicsPath == null);
            }
            return false;
        }
 
        private static void MakeDocumentPath(GraphicsPath path, RectangleF rc) {
            path.AddLine(rc.Left, (float) (rc.Bottom - (rc.Height / 16f)), rc.Left, rc.Top);
            path.AddLine(rc.Left, rc.Top, rc.Right, rc.Top);
            path.AddLine(rc.Right, rc.Top, rc.Right, (float) (rc.Bottom - (rc.Height / 16f)));
            PointF tf1 = new PointF(rc.Right, rc.Bottom - (rc.Height / 16f));
            PointF tf2 = new PointF(rc.Right - (rc.Width / 4f), rc.Bottom - (rc.Height / 4f));
            PointF tf3 = new PointF(rc.Left + (rc.Width / 4f), rc.Bottom + (rc.Height / 8f));
            PointF tf4 = new PointF(rc.Left, rc.Bottom - (rc.Height / 16f));
            path.AddBezier(tf1, tf2, tf3, tf4);
            path.CloseFigure();
        }
 
        private static void MakeMultiDocumentPath(GraphicsPath path, RectangleF rc) {
            PointF[] tfArray1 = new PointF[15];
            tfArray1[0].X = rc.Left;
            tfArray1[0].Y = rc.Bottom - (rc.Height / 16f);
            tfArray1[1].X = rc.Left;
            tfArray1[1].Y = rc.Top + (rc.Height / 4f);
            tfArray1[2].X = rc.Left + (rc.Width / 8f);
            tfArray1[2].Y = rc.Top + (rc.Height / 4f);
            tfArray1[3].X = rc.Left + (rc.Width / 8f);
            tfArray1[3].Y = (rc.Top + (rc.Height / 4f)) - (rc.Height / 8f);
            tfArray1[4].X = rc.Left + (rc.Width / 4f);
            tfArray1[4].Y = (rc.Top + (rc.Height / 4f)) - (rc.Height / 8f);
            tfArray1[5].X = rc.Left + (rc.Width / 4f);
            tfArray1[5].Y = rc.Top;
            tfArray1[6].X = rc.Right;
            tfArray1[6].Y = rc.Top;
            tfArray1[7].X = rc.Right;
            tfArray1[7].Y = rc.Bottom - (rc.Height / 4f);
            tfArray1[8].X = rc.Right - (rc.Width / 8f);
            tfArray1[8].Y = rc.Bottom - (rc.Height / 4f);
            tfArray1[9].X = rc.Right - (rc.Width / 8f);
            tfArray1[9].Y = rc.Bottom - (rc.Height / 8f);
            tfArray1[10].X = rc.Right - (rc.Width / 4f);
            tfArray1[10].Y = rc.Bottom - (rc.Height / 8f);
            tfArray1[11].X = rc.Right - (rc.Width / 4f);
            tfArray1[11].Y = rc.Bottom - ((3f * rc.Height) / 64f);
            tfArray1[12].X = (rc.Right - (rc.Width / 4f)) - ((3f * rc.Width) / 16f);
            tfArray1[12].Y = rc.Bottom - ((3f * rc.Height) / 32f);
            tfArray1[13].X = rc.Left + ((3f * rc.Width) / 16f);
            tfArray1[13].Y = rc.Bottom;
            tfArray1[14].X = rc.Left;
            tfArray1[14].Y = rc.Bottom - ((3f * rc.Height) / 64f);
            tfArray1[12].Y -= ((3f * rc.Height) / 32f);
            tfArray1[13].Y += ((3f * rc.Height) / 32f);
            PointF[] tfArray2 = new PointF[12];
            Array.Copy(tfArray1, tfArray2, 12);
            path.AddLines(tfArray2);
            path.AddBezier(tfArray1[11], tfArray1[12], tfArray1[13], tfArray1[14]);
            path.CloseFigure();
            path.AddLine(tfArray1[2].X, tfArray1[2].Y, tfArray1[10].X, tfArray1[2].Y);
            path.CloseFigure();
            path.AddLine(tfArray1[10].X, tfArray1[2].Y, tfArray1[10].X, tfArray1[10].Y);
            path.CloseFigure();
            path.AddLine(tfArray1[4].X, tfArray1[4].Y, tfArray1[8].X, tfArray1[4].Y);
            path.CloseFigure();
            path.AddLine(tfArray1[8].X, tfArray1[4].Y, tfArray1[8].X, tfArray1[8].Y);
            path.CloseFigure();
        }
 
        private static void MakeOrGatePath(GraphicsPath path, RectangleF rc) {
            PointF tf1 = new PointF(rc.Left, rc.Top);
            PointF tf2 = new PointF((rc.Left + rc.Width) + (rc.Width / 3f), rc.Top);
            PointF tf3 = new PointF((rc.Left + rc.Width) + (rc.Width / 3f), rc.Top + rc.Height);
            PointF tf4 = new PointF(rc.Left, rc.Top + rc.Height);
            PointF tf5 = new PointF(rc.Left + ((8f * rc.Width) / 15f), (rc.Top + rc.Height) - (rc.Height / 6f));
            PointF tf6 = new PointF(rc.Left + ((8f * rc.Width) / 15f), rc.Top + (rc.Height / 6f));
            path.AddBezier(tf1, tf2, tf3, tf4);
            path.AddBezier(tf4, tf5, tf6, tf1);
            path.CloseFigure();
        }
 
        private static void MakePunchedTapePath(GraphicsPath path, RectangleF rc) {
            PointF tf1 = new PointF(rc.Left, rc.Top + (rc.Height / 16f));
            PointF tf2 = new PointF(rc.Left + (rc.Width / 4f), rc.Top + (rc.Height / 4f));
            PointF tf3 = new PointF(rc.Right - (rc.Width / 4f), rc.Top - (rc.Height / 8f));
            PointF tf4 = new PointF(rc.Right, rc.Top + (rc.Height / 16f));
            PointF tf5 = new PointF(rc.Right, rc.Bottom - (rc.Height / 16f));
            PointF tf6 = new PointF(rc.Right - (rc.Width / 4f), rc.Bottom - (rc.Height / 4f));
            PointF tf7 = new PointF(rc.Left + (rc.Width / 4f), rc.Bottom + (rc.Height / 8f));
            PointF tf8 = new PointF(rc.Left, rc.Bottom - (rc.Height / 16f));
            path.AddLine(rc.Left, (float) (rc.Bottom - (rc.Height / 16f)), rc.Left, (float) (rc.Top + (rc.Height / 16f)));
            path.AddBezier(tf1, tf2, tf3, tf4);
            path.AddLine(rc.Right, (float) (rc.Top + (rc.Height / 16f)), rc.Right, (float) (rc.Bottom - (rc.Height / 16f)));
            path.AddBezier(tf5, tf6, tf7, tf8);
        }
 
        private static void MakeTransportPath(GraphicsPath path, RectangleF rc) {
            PointF[] tfArray1 = new PointF[10];
            tfArray1[0].X = rc.Left;
            tfArray1[0].Y = rc.Top + (rc.Height / 2f);
            tfArray1[1].X = rc.Left + (rc.Width / 4f);
            tfArray1[1].Y = rc.Top;
            tfArray1[2].X = rc.Left + (rc.Width / 4f);
            tfArray1[2].Y = rc.Top + (rc.Height / 4f);
            tfArray1[3].X = rc.Right - (rc.Width / 4f);
            tfArray1[3].Y = rc.Top + (rc.Height / 4f);
            tfArray1[4].X = rc.Right - (rc.Width / 4f);
            tfArray1[4].Y = rc.Top;
            tfArray1[5].X = rc.Right;
            tfArray1[5].Y = rc.Top + (rc.Height / 2f);
            tfArray1[6].X = rc.Right - (rc.Width / 4f);
            tfArray1[6].Y = rc.Bottom;
            tfArray1[7].X = rc.Right - (rc.Width / 4f);
            tfArray1[7].Y = rc.Bottom - (rc.Height / 4f);
            tfArray1[8].X = rc.Left + (rc.Width / 4f);
            tfArray1[8].Y = rc.Bottom - (rc.Height / 4f);
            tfArray1[9].X = rc.Left + (rc.Width / 4f);
            tfArray1[9].Y = rc.Bottom;
            path.AddLines(tfArray1);
            path.CloseFigure();
        }
 
        internal virtual void OnAfterChange(EventArgs e) {
            if (this.AfterChange != null) {
                this.AfterChange(this, e);
            }
        }
 
        internal virtual void OnBeforeChange(EventArgs e) {
            if (this.BeforeChange != null) {
                this.BeforeChange(this, e);
            }
        }
 
        internal void Reset() {
            this.m_style = ShapeStyle.Ellipse;
            this.m_orientation = ShapeOrientation.so_0;
            this.m_customGraphicsPath = null;
        }
 
        private static void RoundedRectangle(GraphicsPath path, RectangleF rc, SizeF size) {
            if ((rc.Width > 0f) && (rc.Height > 0f)) {
                path.AddArc((float) (rc.Right - size.Width), rc.Top, size.Width, size.Height, (float) 270f, (float) 90f);
                path.AddArc((float) (rc.Right - size.Width), (float) (rc.Bottom - size.Height), size.Width, size.Height, (float) 0f, (float) 90f);
                path.AddArc(rc.Left, (float) (rc.Bottom - size.Height), size.Width, size.Height, (float) 90f, (float) 90f);
                path.AddArc(rc.Left, rc.Top, size.Width, size.Height, (float) 180f, (float) 90f);
                path.CloseFigure();
            }
        }
 
        public override string ToString() {
            StringBuilder builder1 = new StringBuilder("");
            builder1.Append(this.m_style + "; " + this.m_orientation);
            return builder1.ToString();
        }
 
        [Browsable(false), NotifyParentProperty(true), DefaultValue((string) null), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Returns/sets the path information which defines how the outline of the shape is drawn.")]
        public GraphicsPath GraphicsPath {
            get {
                if (this.m_style == ShapeStyle.Custom) {
                    return this.m_customGraphicsPath;
                }
                return Shape.GetPredefinedPath(this.m_style);
            }
            set {
                this.OnBeforeChange(EventArgs.Empty);
                this.m_customGraphicsPath = value;
                this.OnAfterChange(EventArgs.Empty);
            }
        }
 
        [NotifyParentProperty(true), Description("Returns/sets the shadow orientation of the Shape object."), DefaultValue(0)]
        public ShapeOrientation Orientation {
            get {
                return this.m_orientation;
            }
            set {
                if (this.m_orientation != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_orientation = value;
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }
 
        [Description("Returns/sets the style of the Shape object."), DefaultValue(11), NotifyParentProperty(true)]
        public ShapeStyle Style {
            get {
                return this.m_style;
            }
            set {
                if (this.m_style != value) {
                    this.OnBeforeChange(EventArgs.Empty);
                    this.m_style = value;
                    this.OnAfterChange(EventArgs.Empty);
                }
            }
        }
 
        internal event Shape.AfterChangeEventHandler AfterChange;
 
        internal event Shape.BeforeChangeEventHandler BeforeChange;
 
        //private Shape.AfterChangeEventHandler AfterChange;
 
        //private Shape.BeforeChangeEventHandler BeforeChange;
 
        private const float COS_45 = 0.707f;
 
        internal GraphicsPath m_customGraphicsPath;
 
        internal ShapeOrientation m_orientation;
 
        internal ShapeStyle m_style;
 
        private const float SIN_45 = 0.707f;
    }

    internal class ShapeConverter : ExpandableObjectConverter {

        public ShapeConverter() {
        }
 
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
 
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(InstanceDescriptor)) {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }
 
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value) {
            if (value is string) {
                try {
                    return new Shape((string) value);
                }
                catch {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ArgumentsNotValid));
                }
            }
            return base.ConvertFrom(context, info, value);
        }
 
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (value is Shape) {
                Shape shape1 = (Shape) value;
                if (destinationType == typeof(string)) {
                    return shape1.ToString();
                }
                if (destinationType == typeof(InstanceDescriptor)) {
                    object[] objArray2 = new object[2] { shape1.Style, shape1.Orientation } ;
                    object[] objArray1 = objArray2;
                    Type[] typeArray2 = new Type[2] { typeof(ShapeStyle), typeof(ShapeOrientation) } ;
                    Type[] typeArray1 = typeArray2;
                    ConstructorInfo info1 = typeof(Shape).GetConstructor(typeArray1);
                    return new InstanceDescriptor(info1, objArray1);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
 
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
            return new Shape((ShapeStyle) propertyValues["Style"], (ShapeOrientation) propertyValues["Orientation"]);
        }
 
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
            return true;
        }
    }

    public enum ShapeOrientation {
        // Fields
        so_0 = 0,
        so_180 = 2,
        so_270 = 3,
        so_90 = 1
    }
 
    public enum ShapeStyle {
        // Fields
        AlternateProcess = 0,
        Card = 1,
        Collate = 2,
        Connector = 3,
        Custom = 4,
        Data = 5,
        Decision = 6,
        Delay = 7,
        DirectAccessStorage = 8,
        Display = 9,
        Document = 10,
        Ellipse = 11,
        Extract = 12,
        Hexagon = 13,
        InternalStorage = 14,
        Losange = 15,
        MagneticDisk = 0x10,
        ManualInput = 0x11,
        ManualOperation = 0x12,
        Merge = 0x13,
        MultiDocument = 20,
        Octogon = 0x15,
        OffPageConnection = 0x16,
        Or = 0x17,
        OrGate = 0x18,
        Pentagon = 0x19,
        PredefinedProcess = 0x1a,
        Preparation = 0x1b,
        Process = 0x1c,
        ProcessIso9000 = 0x1d,
        PunchedTape = 30,
        Rectangle = 0x1f,
        RectEdgeBump = 0x20,
        RectEdgeEtched = 0x21,
        RectEdgeRaised = 0x22,
        RectEdgeSunken = 0x23,
        RoundRect = 0x24,
        SequentialAccessStorage = 0x25,
        Sort = 40,
        StoredData = 0x26,
        SummingJunction = 0x27,
        Termination = 0x29,
        Transport = 0x2a,
        Triangle = 0x2b,
        TriangleRectangle = 0x2c
    }
 
    internal class StretchLinkTask : Task {

        private StretchLinkTask() {
        }
 
        internal StretchLinkTask(Link link) {
            this.m_link = link;
            this.m_aptf = new PointF[link.m_aptf.Length];
            link.m_aptf.CopyTo(this.m_aptf, 0);
            this.m_org = this.m_link.m_org;
            this.m_dst = this.m_link.m_dst;
            this.m_oldPtScroll = this.m_link.m_af.ScrollPosition;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.LinkStretch;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            this.m_link.Invalidate(true);
            PointF[] tfArray1 = this.m_link.m_aptf;
            Node node1 = this.m_link.m_org;
            Node node2 = this.m_link.m_dst;
            this.m_link.m_aptf = this.m_aptf;
            if (this.m_link.m_org != this.m_org) {
                this.m_link.SetOrg2(this.m_org);
                this.m_org = node1;
            }
            if (this.m_link.m_dst != this.m_dst) {
                this.m_link.SetDst2(this.m_dst);
                this.m_dst = node2;
            }
            this.m_aptf = tfArray1;
            this.m_link.Invalidate(true);
            Point point1 = this.m_link.m_af.ScrollPosition;
            this.m_link.m_af.ScrollPosition = this.m_oldPtScroll;
            this.m_oldPtScroll = point1;
        }
 
        private PointF[] m_aptf;
 
        private Node m_dst;
 
        private Link m_link;
 
        private Point m_oldPtScroll;
 
        private Node m_org;
    }

    internal class TagTask : Task {

        private TagTask() {
        }
 
        internal TagTask(Item item) {
            this.m_item = item;
            this.m_oldValue = this.m_item.m_tag;
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.Tag;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            object obj1 = this.m_item.m_tag;
            this.m_item.m_tag = this.m_oldValue;
            this.m_oldValue = obj1;
        }
 
        private Item m_item;
 
        private object m_oldValue;
    }

    internal class Task {

        internal Task() {
        }
 
        internal virtual Action GetCode() {
            return Action.AdjustOrg;
        }
 
        internal Action GetGroupCode() {
            return this.m_groupCode;
        }
 
        internal int GetGroupIndex() {
            return this.m_groupIndex;
        }
 
        internal virtual void Redo() {
        }
 
        internal void SetGroupCode(Action iGroupCode) {
            this.m_groupCode = iGroupCode;
        }
 
        internal void SetGroupIndex(int iGroupIndex) {
            this.m_groupIndex = iGroupIndex;
        }
 
        internal virtual void Undo() {
        }
 
        protected Action m_groupCode;
 
        protected int m_groupIndex;
    }

    internal class Undoman {

        private Undoman() {
            this.m_aTask = new ArrayList();
        }
 
        internal Undoman(AddFlow af) {
            this.m_aTask = new ArrayList();
            this.m_af = af;
            this.m_canUndoRedo = true;
            this.m_undoSize = 0;
            this.m_groupCode = Action.AdjustOrg;
            this.m_index = -1;
            this.m_groupIndex = 0;
            this.m_inActionGroupExternal = false;
            this.m_inActionGroupInternal = false;
            this.m_skipUndo = false;
        }
 
        internal void BeginAction(int code) {
            if ((code > 0) && (code < 1000)) {
                throw new IndexOutOfRangeException();
            }
            this.BeginActionExternal((Action) code);
        }
 
        internal bool BeginActionExternal(Action code) {
            bool flag1 = false;
            if (!this.m_inActionGroupExternal) {
                this.m_saveChanged = this.m_af.GetChangedFlag();
                this.m_inActionGroupExternal = true;
                this.m_groupIndex++;
                this.m_groupCode = code;
                flag1 = true;
            }
            return flag1;
        }
 
        internal void BeginActionInternal(Action code) {
            if (!this.m_inActionGroupExternal) {
                this.m_saveChanged = this.m_af.GetChangedFlag();
                this.m_inActionGroupInternal = true;
                this.m_groupIndex++;
                this.m_groupCode = code;
            }
        }
 
        internal bool CanRedo() {
            if (!this.m_canUndoRedo) {
                return false;
            }
            return (this.m_index < (this.m_aTask.Count - 1));
        }
 
        internal bool CanUndo() {
            if (!this.m_canUndoRedo) {
                return false;
            }
            return (this.m_index >= 0);
        }
 
        internal void Clear() {
            this.m_aTask.Clear();
            this.m_index = -1;
            this.m_groupIndex = 0;
            this.m_inActionGroupExternal = false;
            this.m_inActionGroupInternal = false;
        }
 
        internal void EndAction() {
            this.EndActionExternal();
        }
 
        internal void EndActionExternal() {
            if (this.m_inActionGroupExternal) {
                if (this.m_af.GetChangedFlag() > (this.m_saveChanged + 1)) {
                    this.m_af.IncrementChangedFlag((this.m_saveChanged + 1) - this.m_af.GetChangedFlag());
                }
                this.m_inActionGroupExternal = false;
                this.m_groupCode = Action.None;
            }
        }
 
        internal void EndActionInternal() {
            if (!this.m_inActionGroupExternal) {
                if (this.m_af.GetChangedFlag() > (this.m_saveChanged + 1)) {
                    this.m_af.IncrementChangedFlag((this.m_saveChanged + 1) - this.m_af.GetChangedFlag());
                }
                this.m_inActionGroupInternal = false;
                this.m_groupCode = Action.None;
            }
        }
 
        internal bool IsCurrentActionGroup() {
            if (!this.m_inActionGroupInternal) {
                return this.m_inActionGroupExternal;
            }
            return true;
        }
 
        internal void Redo() {
            int num1 = 0;
            num1 = (num1 > 0) ? 0 : num1++;
            if (this.m_canUndoRedo && this.CanRedo()) {
                this.m_index++;
                Task task1 = (Task) this.m_aTask[this.m_index];
                task1.Redo();
                this.m_af.IncrementChangedFlag(1);
                int num2 = task1.GetGroupIndex();
                while (true) {
                    if (!this.CanRedo()) {
                        return;
                    }
                    this.m_index++;
                    Task task2 = (Task) this.m_aTask[this.m_index];
                    if (task2.GetGroupIndex() != num2) {
                        break;
                    }
                    task2.Redo();
                }
                this.m_index--;
            }
        }
 
        internal void RemoveLastTask() {
            this.m_aTask.RemoveAt(this.m_aTask.Count - 1);
            this.m_index--;
        }
 
        internal void SubmitTask(Task task) {
            int num1 = 0;
            num1 = (num1 > 0) ? 0 : num1++;
            while ((this.m_index + 1) < this.m_aTask.Count) {
                this.m_aTask.RemoveAt(this.m_aTask.Count - 1);
            }
            if (this.m_undoSize > 0) {
                while (this.m_index >= (this.m_undoSize - 1)) {
                    this.m_aTask.RemoveAt(0);
                    this.m_index--;
                }
            }
            this.m_aTask.Add(task);
            this.m_index++;
            if (!this.m_inActionGroupExternal && !this.m_inActionGroupInternal) {
                this.m_groupIndex++;
            }
            task.SetGroupIndex(this.m_groupIndex);
            task.SetGroupCode(this.m_groupCode);
        }
 
        internal void Undo() {
            int num1 = 0;
            num1 = (num1 > 0) ? 0 : num1++;
            if (this.m_canUndoRedo && this.CanUndo()) {
                Task task1 = (Task) this.m_aTask[this.m_index];
                this.m_index--;
                task1.Undo();
                this.m_af.IncrementChangedFlag(-1);
                int num2 = task1.GetGroupIndex();
                while (this.CanUndo()) {
                    Task task2 = (Task) this.m_aTask[this.m_index];
                    if (task2.GetGroupIndex() != num2) {
                        return;
                    }
                    this.m_index--;
                    task2.Undo();
                }
            }
        }
 
        internal bool CanUndoRedo {
            get {
                return this.m_canUndoRedo;
            }
            set {
                this.m_canUndoRedo = value;
                if (!this.m_canUndoRedo) {
                    this.Clear();
                }
            }
        }
 
        internal Action RedoCode {
            get {
                if (this.CanRedo()) {
                    Task task1 = (Task) this.m_aTask[this.m_index + 1];
                    return task1.GetCode();
                }
                return Action.None;
            }
        }
 
        internal bool SkipUndo {
            get {
                return this.m_skipUndo;
            }
            set {
                this.m_skipUndo = value;
            }
        }
 
        internal Action UndoCode {
            get {
                if (this.CanUndo()) {
                    Task task1 = (Task) this.m_aTask[this.m_index];
                    return task1.GetCode();
                }
                return Action.None;
            }
        }
 
        internal int UndoSize {
            get {
                return this.m_undoSize;
            }
            set {
                this.m_undoSize = value;
            }
        }
 
        internal AddFlow m_af;
 
        private ArrayList m_aTask;
 
        private bool m_canUndoRedo;
 
        private Action m_groupCode;
 
        private int m_groupIndex;
 
        private bool m_inActionGroupExternal;
 
        private bool m_inActionGroupInternal;
 
        private int m_index;
 
        private int m_saveChanged;
 
        private bool m_skipUndo;
 
        private int m_undoSize;
    }

    [Serializable, TypeConverter(typeof(ZoomConverter))]
    public class Zoom : ICloneable {

        internal delegate void ChangeEventHandler(object sender, EventArgs e);
 
        public Zoom() {
            this.Reset();
        }
 
        public Zoom(string format) {
            char[] chArray1 = new char[1] { ';' } ;
            string[] textArray1 = format.Split(chArray1);
            float single1 = Convert.ToSingle(textArray1[0]);
            float single2 = Convert.ToSingle(textArray1[1]);
            if ((single1 <= 0f) || (single2 <= 0f)) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ZoomMustBePositive));
            }
            this.m_X = single1;
            this.m_Y = single2;
        }
 
        public Zoom(float X, float Y) {
            if ((X <= 0f) || (Y <= 0f)) {
                throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ZoomMustBePositive));
            }
            this.m_X = X;
            this.m_Y = Y;
        }
 
        public object Clone() {
            return new Zoom(this.m_X, this.m_Y);
        }
 
        internal bool Equals(object obj) {
            if (obj is Zoom) {
                Zoom zoom1 = (Zoom) obj;
                if (this.m_X == zoom1.m_X) {
                    return (this.m_Y == zoom1.m_Y);
                }
            }
            return false;
        }
 
        internal bool IsDefault() {
            if (this.m_X == 1f) {
                return (this.m_Y == 1f);
            }
            return false;
        }
 
        internal virtual void OnChange(EventArgs e) {
            if (this.Change != null) {
                this.Change(this, e);
            }
        }
 
        internal void Reset() {
            this.m_X = 1f;
            this.m_Y = 1f;
        }
 
        public override string ToString() {
            StringBuilder builder1 = new StringBuilder("");
            builder1.Append(this.m_X + "; " + this.m_Y);
            return builder1.ToString();
        }
 
        [NotifyParentProperty(true), DefaultValue(1)]
        public float X {
            get {
                return this.m_X;
            }
            set {
                if (value <= 0f) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ZoomMustBePositive));
                }
                if (this.m_X != value) {
                    this.m_X = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        [NotifyParentProperty(true), DefaultValue(1)]
        public float Y {
            get {
                return this.m_Y;
            }
            set {
                if (value <= 0f) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ZoomMustBePositive));
                }
                if (this.m_Y != value) {
                    this.m_Y = value;
                    this.OnChange(EventArgs.Empty);
                }
            }
        }
 
        internal event Zoom.ChangeEventHandler Change;
 
        //private Zoom.ChangeEventHandler Change;
 
        private float m_X;
 
        private float m_Y;
    }

    internal class ZoomConverter : ExpandableObjectConverter {

        public ZoomConverter() {
        }
 
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) {
            if (sourceType == typeof(string)) {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
 
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            if (destinationType == typeof(InstanceDescriptor)) {
                return true;
            }
            return base.CanConvertTo(context, destinationType);
        }
 
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo info, object value) {
            if (value is string) {
                try {
                    return new Zoom((string) value);
                }
                catch {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ArgumentsNotValid));
                }
            }
            return base.ConvertFrom(context, info, value);
        }
 
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType) {
            if (value is Zoom) {
                Zoom zoom1 = (Zoom) value;
                if (destinationType == typeof(string)) {
                    return zoom1.ToString();
                }
                if (destinationType == typeof(InstanceDescriptor)) {
                    object[] objArray2 = new object[2] { zoom1.X, zoom1.Y } ;
                    object[] objArray1 = objArray2;
                    Type[] typeArray2 = new Type[2] { typeof(float), typeof(float) } ;
                    Type[] typeArray1 = typeArray2;
                    ConstructorInfo info1 = typeof(Zoom).GetConstructor(typeArray1);
                    return new InstanceDescriptor(info1, objArray1);
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
 
        public override object CreateInstance(ITypeDescriptorContext context, IDictionary propertyValues) {
            return new Zoom((float) propertyValues["X"], (float) propertyValues["Y"]);
        }
 
        public override bool GetCreateInstanceSupported(ITypeDescriptorContext context) {
            return true;
        }
    }

    public enum ZoomType {
        // Fields
        Anisotropic = 1,
        Isotropic = 0
    }
 
    internal class ZOrderIndexTask : Task {

        private ZOrderIndexTask() {
        }
 
        internal ZOrderIndexTask(Item item) {
            this.m_item = item;
            this.m_oldIdx = this.m_item.GetZOrder();
        }
 
        internal override Action GetCode() {
            if (this.m_groupCode <= Action.AdjustOrg) {
                return Action.ZOrder;
            }
            return this.m_groupCode;
        }
 
        internal override void Redo() {
            this.Undo();
        }
 
        internal override void Undo() {
            int num1 = this.m_item.GetZOrder();
            this.m_item.SetZOrder2(this.m_oldIdx);
            this.m_oldIdx = num1;
        }
 
        private Item m_item;
 
        private int m_oldIdx;
 
    }

}




















