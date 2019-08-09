using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Drawing.Printing;
using System.Reflection;
using System.Resources;
using System.Diagnostics;

namespace Lassalle.Flow {

    //[LicenseProvider(typeof(AddFlowLicenseProvider)), Designer(typeof(AddFlowDesigner)), Description("Lassalle AddFlow Control"), DefaultEvent("MouseDown")]
    [Designer(typeof(AddFlowDesigner)), Description("Lassalle AddFlow Control"), DefaultEvent("MouseDown")]
    public class AddFlow : UserControl {
  
        private void InitializeComponent() {
            base.Name = "AddFlow";
            base.Size = new Size(300, 200);
        }

        public delegate void AfterAddLinkEventHandler(object sender, AfterAddLinkEventArgs e);

        public delegate void AfterAddNodeEventHandler(object sender, AfterAddNodeEventArgs e);

        public delegate void AfterEditEventHandler(object sender, AfterEditEventArgs e);

        public delegate void AfterMoveEventHandler(object sender, EventArgs e);

        public delegate void AfterResizeEventHandler(object sender, EventArgs e);

        public delegate void AfterSelectEventHandler(object sender, EventArgs e);

        public delegate void AfterStretchEventHandler(object sender, EventArgs e);

        public delegate void BeforeAddLinkEventHandler(object sender, BeforeAddLinkEventArgs e);

        public delegate void BeforeAddNodeEventHandler(object sender, BeforeAddNodeEventArgs e);

        public delegate void BeforeChangeDstEventHandler(object sender, BeforeChangeDstEventArgs e);

        public delegate void BeforeChangeOrgEventHandler(object sender, BeforeChangeOrgEventArgs e);

        public delegate void BeforeEditEventHandler(object sender, BeforeEditEventArgs e);
 
        private enum ClickZone {
            // Fields
            CurLink = 10,
            CurLinkShift = 12,
            CurNode = 4,
            CurNodeShift = 6,
            Link = 9,
            LinkShift = 13,
            Node = 3,
            NodeShift = 7,
            Out = 0,
            OutShift = 1,
            SelLink = 11,
            SelLinkShift = 14,
            SelNode = 5,
            SelNodeShift = 8,
            Square = 2
        }
 
        internal delegate void DesignModeChangeEventHandler(object sender, EventArgs e);
 
        public delegate void DiagramOwnerDrawEventHandler(object sender, DiagramOwnerDrawEventArgs e);
 
        public delegate void ErrorEventHandler(object sender, ErrorEventArgs e);
 
        internal enum ExceptionType {
            // Fields
            AdjustDst = 1,
            AdjustOrg = 0,
            ArgumentsNotValid = 6,
            ChangePoints = 2,
            DrawWidthCannotBeNegative = 11,
            GridSizeCannotBeNegative = 10,
            ItemAlreadyOwned = 3,
            ItemMustBeInGraph = 4,
            OutOfAreaError = 5,
            ShadowSizeCannotBeNegative = 9,
            TextMarginLimits = 8,
            ZoomMustBePositive = 7
        }
 
        public delegate void ExtentChangeEventHandler(object sender, EventArgs e);
 
        private enum HandlePixelSize {
            // Fields
            Large = 12,
            Medium = 9,
            Small = 6
        }
 
        public delegate void LinkOwnerDrawEventHandler(object sender, LinkOwnerDrawEventArgs e);
 
        public delegate void NodeOwnerDrawEventHandler(object sender, NodeOwnerDrawEventArgs e);
 
        private enum ScrollDir {
            // Fields
            Bottom = 1,
            Left = 4,
            None = 0,
            Right = 3,
            Top = 2
        }
 
        public delegate void ScrollEventHandler(object sender, EventArgs e);
 
        private enum ScrollTime {
            // Fields
            Time1 = 300,
            Time2 = 50
        }
 
        private enum SizeDir {
            // Fields
            Down = 6,
            Left = 3,
            LeftDown = 5,
            LeftUp = 0,
            Right = 4,
            RightDown = 7,
            RightUp = 2,
            Up = 1
        }
 
        private enum Stretch {
            // Fields
            Add = 1,
            Change = 5,
            Del = 2,
            First = 3,
            Last = 4,
            None = 0
        }
 
        static AddFlow() {
            AddFlow.m_licenseKey = null;
        }
 
        public AddFlow() {
            this.m_textBox = null;
            this.m_defnode = null;
            this.m_deflink = null;
            this.m_connect = null;
            this.m_undo = null;
            this.m_timerScroll = null;
            this.m_timerEdit = null;
            this.m_tooltipCtrl = null;
            this.components = null;
            this.license = null;
            this.m_licensed = false;
            this.InitializeComponent();
            this.CheckLicense();
            this.Reset();
        }

        private BorderStyle borderStyle=BorderStyle.Fixed3D;
        [DefaultValue(BorderStyle.Fixed3D)]
        public BorderStyle BorderStyle {
            get {
                return this.borderStyle;
            }
            set {
                if (this.borderStyle != value) {
                    if (!Enum.IsDefined(typeof(BorderStyle), value)) {
                        throw new InvalidEnumArgumentException("value", (int) value, typeof(BorderStyle));
                    }
                    this.borderStyle = value;
                    base.UpdateStyles();
                }
            }
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams params1 = base.CreateParams;
                //if (this.HScroll) {
                //    params1.Style |= WinAPI.WS_HSCROLL;
                //}
                //else {
                //    params1.Style &= ~WinAPI.WS_HSCROLL;
                //}
                //if (this.VScroll) {
                //    params1.Style |= WinAPI.WS_VSCROLL;
                //}
                //else {
                //    params1.Style &= ~WinAPI.WS_VSCROLL;
                //}
                                            
                params1.ExStyle |= WinAPI.WS_EX_CONTROLPARENT;
                params1.ExStyle &= ~WinAPI.WS_EX_CLIENTEDGE;
                params1.Style &= ~WinAPI.WS_BORDER;
                switch (this.borderStyle) {
                case BorderStyle.FixedSingle: {
                    params1.Style |= WinAPI.WS_BORDER;
                    return params1;
                }
                case BorderStyle.Fixed3D: {
                    params1.ExStyle |= WinAPI.WS_EX_CLIENTEDGE;
                    return params1;
                }
                }

                return params1;
            }
        }

        public void AddLink(Link link, Node org, Node dst) {
            this.AddLink2(link, org, dst, false);
        }
 
        internal void AddLink2(Link link, Node org, Node dst, bool interactive) {
            if ((link != null) && !link.Existing) {
                if (interactive) {
                    BeforeAddLinkEventArgs args1 = new BeforeAddLinkEventArgs(org, dst);
                    this.OnBeforeAddLink(args1);
                    if (args1.Cancel.Cancel) {
                        return;
                    }
                }
                link.m_org = org;
                link.m_dst = dst;
                if (link.Reflexive && !link.m_line.NewPointsAllowed) {
                    link.m_line.m_style = LineStyle.Bezier;
                }
                this.AddLinkToGraph(link);
                if (interactive && (((this.m_cycleMode == CycleMode.NoDirectedCycle) && this.m_connect.IsCycleDirected()) || ((this.m_cycleMode == CycleMode.NoCycle) && this.m_connect.IsCycle(link.m_org)))) {
                    if (this.m_canFireError) {
                        if (this.m_cycleMode == CycleMode.NoDirectedCycle) {
                            this.OnError(new ErrorEventArgs(ErrorEventType.DirectedCycleError));
                        }
                        else {
                            this.OnError(new ErrorEventArgs(ErrorEventType.CycleError));
                        }
                    }
                    bool flag1 = this.m_undo.CanUndoRedo;
                    this.m_undo.CanUndoRedo = false;
                    this.RemoveLink(link);
                    this.m_undo.CanUndoRedo = flag1;
                }
                else {
                    PointF[] tfArray1 = link.m_aptf;
                    link.InitPoints();
                    if (link.m_line.HVStyle) {
                        link.ChangeLineStyle();
                        link.Adjust(link.m_org);
                        link.Adjust(link.m_dst);
                    }
                    link.CalcRect();
                    if (interactive && !this.m_deflink.m_line.HVStyle) {
                        link.Invalidate(false);
                        if (this.m_deflink.m_adjustOrg && (this.m_linkCreationMode == LinkCreationMode.AllNodeArea)) {
                            link.ChangePoint(0, this.m_ptOrg);
                        }
                        if (this.m_deflink.m_adjustDst) {
                            link.ChangePoint(link.m_aptf.Length - 1, this.m_ptDst);
                        }
                        link.Invalidate(true);
                    }
                    if (this.m_undo.CanUndoRedo && !this.m_undo.SkipUndo) {
                        this.m_undo.SubmitTask(new AddLinkTask(link));
                    }
                    this.UpdateRect2(link.m_rcSeg);
                    link.InvalidateItem();
                    this.IncrementChangedFlag(1);
                    if (interactive) {
                        this.OnAfterAddLink(new AfterAddLinkEventArgs(link));
                    }
                }
            }
        }
 
        internal void AddLinkToGraph(Link link) {
            link.m_af = this;
            this.m_items.Add(link);
            link.m_org.m_aLinks.Add(link);
            link.m_dst.m_aLinks.Add(link);
        }
 
        internal void AddNode(Node node, bool interactive) {
            if ((node != null) && !node.Existing) {
                if (interactive) {
                    BeforeAddNodeEventArgs args1 = new BeforeAddNodeEventArgs(node.m_rc);
                    this.OnBeforeAddNode(args1);
                    if (args1.Cancel.Cancel) {
                        return;
                    }
                    if (!args1.Rect.Equals(node.m_rc)) {
                        Graphics graphics1 = base.CreateGraphics();
                        this.CoordinatesDeviceToWorld(graphics1);
                        RectangleF ef1 = Misc.RectWorldToDevice(graphics1, node.m_rc);
                        SizeF ef2 = new SizeF((float) (this.m_selGrabSize + 2), (float) (this.m_selGrabSize + 2));
                        ef1.Inflate(ef2);
                        graphics1.Dispose();
                        base.Invalidate(Rectangle.Round(ef1));
                        node.m_rc = args1.Rect;
                    }
                }
                this.AddNodeToGraph(node);
                if (this.m_undo.CanUndoRedo && !this.m_undo.SkipUndo) {
                    this.m_undo.SubmitTask(new AddNodeTask(node));
                }
                this.UpdateRect2(node.GetRectShadow());
                node.InvalidateItem();
                this.IncrementChangedFlag(1);
                if (interactive) {
                    this.OnAfterAddNode(new AfterAddNodeEventArgs(node));
                }
            }
        }
 
        private void AddNodeToDragList(Node node) {
            if (!node.m_drag) {
                node.m_drag = true;
                this.m_aDragNodes.Add(node);
            }
            foreach (Link link1 in node.m_aLinks) {
                if (!link1.Existing || !link1.m_rigid) {
                    continue;
                }
                Node node1 = link1.m_dst;
                if ((node1 != node) && !node1.m_drag) {
                    node1.m_b1 = node.m_xMoveable;
                    node1.m_b2 = node.m_yMoveable;
                    this.AddNodeToDragList(node1);
                }
            }
        }
 
        internal void AddNodeToGraph(Node node) {
            node.m_af = this;
            this.m_items.Add(node);
            this.m_aNodes.Add(node);
            if ((node.m_autoSize != AutoSize.None) && (node.m_autoSize != AutoSize.ImageToNode)) {
                node.AdjustNodeSize();
            }
        }
 
        public void BeginAction(int code) {
            this.m_undo.BeginAction(code);
        }
 
        private void BeginSelWork(Graphics grfx, PointF pt) {
            switch (this.m_mouseAction) {
            case MouseAction.DrawRectangle:
            case MouseAction.Zoom:
            case MouseAction.ZoomIsotropic: {
                this.m_interactiveAction = InteractiveAction.Select;
                break;
            }
            case MouseAction.None: {
                this.m_interactiveAction = this.m_canDrawNode ? InteractiveAction.Node : InteractiveAction.None;
                break;
            }
            case MouseAction.Selection:
            case MouseAction.Selection2: {
                this.m_interactiveAction = this.m_multiSel ? InteractiveAction.Select : InteractiveAction.None;
                break;
            }
            }
            this.m_priorMode = this.m_interactiveAction;
            bool flag1 = this.m_multiSel && ((Control.ModifierKeys == Keys.Shift) || (Control.ModifierKeys == Keys.Control));
            Item item1 = this.SelectedItem;
            bool flag2 = true;
            MouseArea area1 = this.CheckArea(grfx, pt);
            switch (area1) {
            case MouseArea.StretchSquare: {
                this.m_interactiveAction = InteractiveAction.Stretch;
                break;
            }
            case MouseArea.LinkSquare: {
                this.m_org = (Node) item1;
                this.m_interactiveAction = InteractiveAction.Link;
                this.m_out = false;
                break;
            }
            case MouseArea.NodeDragFrame: {
                this.m_interactiveAction = this.m_canMoveNode ? InteractiveAction.Drag : InteractiveAction.None;
                break;
            }
            default: {
                if ((area1 >= MouseArea.LeftUpSquare) && (area1 < MouseArea.StretchSquare)) {
                    this.m_interactiveAction = InteractiveAction.Size;
                }
                else {
                    flag2 = false;
                }
                break;
            }
            }
            if (flag2) {
                item1.InvalidateHandles();
                this.m_clickEvent = AddFlow.ClickZone.Square;
            }
            else {
                Item item2 = this.m_pointedItem;
                if (((item2 != null) && item2.Existing) && item2.m_selectable) {
                    if (item2 is Link) {
                        if (item2 == item1) {
                            this.m_clickEvent = flag1 ? AddFlow.ClickZone.CurLinkShift : AddFlow.ClickZone.CurLink;
                        }
                        else if (item2.m_selected) {
                            this.m_clickEvent = flag1 ? AddFlow.ClickZone.SelLinkShift : AddFlow.ClickZone.SelLink;
                        }
                        else {
                            this.m_clickEvent = flag1 ? AddFlow.ClickZone.LinkShift : AddFlow.ClickZone.Link;
                        }
                    }
                    else if (item2 == item1) {
                        this.m_clickEvent = flag1 ? AddFlow.ClickZone.CurNodeShift : AddFlow.ClickZone.CurNode;
                    }
                    else if (item2.m_selected) {
                        this.m_clickEvent = flag1 ? AddFlow.ClickZone.SelNodeShift : AddFlow.ClickZone.SelNode;
                    }
                    else {
                        this.m_clickEvent = flag1 ? AddFlow.ClickZone.NodeShift : AddFlow.ClickZone.Node;
                    }
                }
                else {
                    this.m_clickEvent = flag1 ? AddFlow.ClickZone.OutShift : AddFlow.ClickZone.Out;
                }
                switch (this.m_clickEvent) {
                case AddFlow.ClickZone.Out: {
                    this.UnSelect();
                    break;
                }
                case AddFlow.ClickZone.Node:
                case AddFlow.ClickZone.Link: {
                    this.UnSelect();
                    if (item2 != null) {
                        item2.Selected = true;
                    }
                    break;
                }
                case AddFlow.ClickZone.CurNode:
                case AddFlow.ClickZone.CurNodeShift:
                case AddFlow.ClickZone.NodeShift:
                case AddFlow.ClickZone.SelNodeShift:
                case AddFlow.ClickZone.CurLink:
                case AddFlow.ClickZone.CurLinkShift:
                case AddFlow.ClickZone.LinkShift:
                case AddFlow.ClickZone.SelLinkShift: {
                    this.SetSelChangedFlag(true);
                    break;
                }
                }
                Node node1 = null;
                switch (this.m_clickEvent) {
                case AddFlow.ClickZone.Out: {
                    if (this.m_interactiveAction != InteractiveAction.Select) {
                        this.m_interactiveAction = this.m_canDrawNode ? InteractiveAction.Node : InteractiveAction.None;
                    }
                    break;
                }
                case AddFlow.ClickZone.OutShift: {
                    if (this.m_interactiveAction != InteractiveAction.Select) {
                        this.m_interactiveAction = InteractiveAction.None;
                    }
                    break;
                }
                case AddFlow.ClickZone.Node:
                case AddFlow.ClickZone.CurNode: {
                    if (item2 != null) {
                        node1 = (Node) item2;
                        if (node1 != null) {
                            if (((this.m_linkCreationMode != LinkCreationMode.AllNodeArea) || !node1.OutLinkable) || !this.m_canDrawLink) {
                                if (!this.m_canMoveNode || (!node1.m_xMoveable && !node1.m_yMoveable)) {
                                    this.m_interactiveAction = InteractiveAction.None;
                                    break;
                                }
                                this.m_interactiveAction = InteractiveAction.Drag;
                                break;
                            }
                            this.m_org = node1;
                            this.m_interactiveAction = InteractiveAction.Link;
                            this.m_out = false;
                        }
                    }
                    break;
                }
                case AddFlow.ClickZone.SelNode: {
                    if (item2 != null) {
                        node1 = (Node) item2;
                        if (!this.m_canMoveNode || (!node1.m_xMoveable && !node1.m_yMoveable)) {
                            this.m_interactiveAction = InteractiveAction.None;
                            break;
                        }
                        this.m_interactiveAction = InteractiveAction.Drag;
                    }
                    break;
                }
                case AddFlow.ClickZone.CurNodeShift:
                case AddFlow.ClickZone.NodeShift:
                case AddFlow.ClickZone.SelNodeShift:
                case AddFlow.ClickZone.Link:
                case AddFlow.ClickZone.CurLink:
                case AddFlow.ClickZone.CurLinkShift:
                case AddFlow.ClickZone.LinkShift:
                case AddFlow.ClickZone.SelLinkShift: {
                    this.m_interactiveAction = InteractiveAction.None;
                    break;
                }
                case AddFlow.ClickZone.SelLink: {
                    this.m_interactiveAction = this.m_canMoveNode ? InteractiveAction.Drag : InteractiveAction.None;
                    break;
                }
                }
                switch (this.m_clickEvent) {
                case AddFlow.ClickZone.CurNodeShift:
                case AddFlow.ClickZone.SelNodeShift:
                case AddFlow.ClickZone.CurLinkShift:
                case AddFlow.ClickZone.SelLinkShift: {
                    if ((item2 != null) && item2.Existing) {
                        item2.Selected = false;
                    }
                    return;
                }
                case AddFlow.ClickZone.NodeShift:
                case AddFlow.ClickZone.LinkShift: {
                    if ((item2 != null) && item2.Existing) {
                        item2.Selected = true;
                    }
                    return;
                }
                case AddFlow.ClickZone.Link:
                case AddFlow.ClickZone.CurLink:
                case AddFlow.ClickZone.SelLink: {
                    return;
                }
                }
            }
        }
 
        public void BeginUpdate() {
            this.m_repaint++;
        }
 
        private void ChangeLinkHandleSize() {
            this.m_linkGrabSize = 6;
            switch (this.m_linkHandleSize) {
            case HandleSize.Small: {
                this.m_linkGrabSize = 6;
                return;
            }
            case HandleSize.Medium: {
                this.m_linkGrabSize = 9;
                return;
            }
            case HandleSize.Large: {
                this.m_linkGrabSize = 12;
                return;
            }
            }
        }
 
        private void ChangeSelectionHandleSize() {
            this.m_selGrabSize = 6;
            switch (this.m_selHandleSize) {
            case HandleSize.Small: {
                this.m_selGrabSize = 6;
                return;
            }
            case HandleSize.Medium: {
                this.m_selGrabSize = 9;
                return;
            }
            case HandleSize.Large: {
                this.m_selGrabSize = 12;
                return;
            }
            }
        }
 
        private MouseArea CheckArea(Graphics grfx, PointF ptWorld) {
            this.m_checkedAreaPoint = ptWorld;
            this.m_stretchingPoint = -1;
            this.m_pointedArea = MouseArea.OutSide;
            MouseArea area1 = MouseArea.OutSide;
            PointF tf1 = Misc.PointWorldToDevice(grfx, ptWorld);
            Item item1 = this.GetItemByPoint(ptWorld, ItemSet.SelectableItems);
            if (this.m_pointedItem != item1) {
                this.m_pointedItem = item1;
                if ((this.m_showTooltips && (item1 != null)) && ((item1.Tooltip != null) && (item1.Tooltip.Length > 0))) {
                    this.m_tooltipCtrl.SetToolTip(this, item1.Tooltip);
                }
                else {
                    this.m_tooltipCtrl.SetToolTip(this, null);
                }
            }
            if (item1 != null) {
                this.m_pointedArea = (item1 is Link) ? MouseArea.Link : MouseArea.Node;
            }
            if ((!this.m_out && (this.m_interactiveAction == InteractiveAction.Link)) && ((item1 == null) || (item1 is Link))) {
                this.m_out = true;
            }
            if ((this.m_linkCreationMode == LinkCreationMode.AllNodeArea) && this.IsMouseOverDragFrame(grfx, tf1)) {
                this.m_pointedArea = MouseArea.NodeDragFrame;
            }
            Item item2 = this.SelectedItem;
            if (this.m_interHandles && (item2 != null)) {
                RectangleF ef1;
                SizeF ef2;
                if (item2 is Link) {
                    if (this.m_interactiveAction != InteractiveAction.Stretch) {
                        Link link1 = (Link) item2;
                        int num1 = link1.m_aptf.Length;
                        if (num1 >= 2) {
                            PointF tf2 = Misc.PointWorldToDevice(grfx, link1.m_aptf[0]);
                            if (this.m_canStretchLink && link1.m_stretchable) {
                                ef2 = new SizeF((float) this.m_selGrabSize, (float) this.m_selGrabSize);
                                if (link1.m_line.NewPointsAllowed) {
                                    int num2 = (2 * num1) - 1;
                                    for (int num3 = 0; num3 < num2; num3++) {
                                        if ((((num3 != 0) || this.m_canChangeOrg) || link1.m_adjustOrg) && (((num3 != (num2 - 1)) || this.m_canChangeDst) || link1.m_adjustDst)) {
                                            if ((num3 % 2) == 0) {
                                                tf2 = link1.m_aptf[num3 / 2];
                                            }
                                            else {
                                                tf2 = Misc.MiddlePoint(link1.m_aptf[num3 / 2], link1.m_aptf[(num3 / 2) + 1]);
                                            }
                                            PointF tf3 = Misc.PointWorldToDevice(grfx, tf2);
                                            ef1 = RectangleF.FromLTRB(tf3.X - (ef2.Width / 2f), tf3.Y - (ef2.Height / 2f), tf3.X + (ef2.Width / 2f), tf3.Y + (ef2.Height / 2f));
                                            if (ef1.Contains(tf1)) {
                                                this.m_pointedArea = MouseArea.StretchSquare;
                                                if (num3 == 0) {
                                                    this.m_stretchType = AddFlow.Stretch.First;
                                                }
                                                else if (num3 == (num2 - 1)) {
                                                    this.m_stretchType = AddFlow.Stretch.Last;
                                                }
                                                else if ((num3 % 2) != 0) {
                                                    this.m_stretchType = AddFlow.Stretch.Add;
                                                }
                                                else {
                                                    this.m_stretchType = AddFlow.Stretch.Del;
                                                }
                                                this.m_handle = num3 / 2;
                                                this.m_stretchingPoint = num3;
                                                break;
                                            }
                                        }
                                    }
                                }
                                else {
                                    for (int num4 = 0; num4 < num1; num4++) {
                                        tf2 = link1.m_aptf[num4];
                                        PointF tf4 = Misc.PointWorldToDevice(grfx, tf2);
                                        ef1 = RectangleF.FromLTRB(tf4.X - (ef2.Width / 2f), tf4.Y - (ef2.Height / 2f), tf4.X + (ef2.Width / 2f), tf4.Y + (ef2.Height / 2f));
                                        if (ef1.Contains(tf1)) {
                                            this.m_pointedArea = MouseArea.StretchSquare;
                                            if (num4 == 0) {
                                                this.m_stretchType = AddFlow.Stretch.First;
                                            }
                                            else if (num4 == (num1 - 1)) {
                                                this.m_stretchType = AddFlow.Stretch.Last;
                                            }
                                            else {
                                                this.m_stretchType = AddFlow.Stretch.Change;
                                            }
                                            this.m_handle = num4;
                                            this.m_stretchingPoint = num4;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else {
                    Node node1 = (Node) item2;
                    PointF[] tfArray1 = node1.GetHandlePoints(grfx);
                    int num5 = tfArray1.Length - 1;
                    ef2 = new SizeF((float) this.m_selGrabSize, (float) this.m_selGrabSize);
                    for (int num6 = 0; num6 < num5; num6++) {
                        ef1 = RectangleF.FromLTRB(tfArray1[num6].X - (ef2.Width / 2f), tfArray1[num6].Y - (ef2.Height / 2f), tfArray1[num6].X + (ef2.Width / 2f), tfArray1[num6].Y + (ef2.Height / 2f));
                        if (!ef1.Contains(tf1)) {
                            goto Label_0527;
                        }
                        this.m_pointedArea = (MouseArea) (num6 + 1);
                        this.m_sizeDir = (AddFlow.SizeDir) num6;
                        switch (this.m_sizeDir) {
                        case AddFlow.SizeDir.LeftUp:
                        case AddFlow.SizeDir.RightUp:
                        case AddFlow.SizeDir.LeftDown:
                        case AddFlow.SizeDir.RightDown: {
                            if (this.m_canSizeNode) {
                                goto Label_04F6;
                            }
                            area1 = MouseArea.NodeDragFrame;
                            break;
                        }
                        case AddFlow.SizeDir.Up:
                        case AddFlow.SizeDir.Down: {
                            goto Label_04D3;
                        }
                        case AddFlow.SizeDir.Left:
                        case AddFlow.SizeDir.Right: {
                            goto Label_04BD;
                        }
                        }
                        break;
                    Label_04BD:
                        if (!this.m_canSizeNode || !node1.m_xSizeable) {
                            area1 = MouseArea.NodeDragFrame;
                        }
                        break;
                    Label_04D3:
                        if (!this.m_canSizeNode || !node1.m_ySizeable) {
                            area1 = MouseArea.NodeDragFrame;
                        }
                        break;
                    Label_04F6:
                        if (!node1.m_xSizeable && !node1.m_ySizeable) {
                            area1 = MouseArea.NodeDragFrame;
                            break;
                        }
                        if (!node1.m_xSizeable) {
                            area1 = MouseArea.UpSquare;
                            break;
                        }
                        if (!node1.m_ySizeable) {
                            area1 = MouseArea.LeftSquare;
                        }
                        break;
                    Label_0527:;
                    }
                    if (((this.m_pointedArea == MouseArea.Node) && this.m_canDrawLink) && (node1.OutLinkable && (this.m_linkCreationMode == LinkCreationMode.MiddleHandle))) {
                        ef2 = new SizeF((float) this.m_linkGrabSize, (float) this.m_linkGrabSize);
                        ef1 = RectangleF.FromLTRB(tfArray1[num5].X - (ef2.Width / 2f), tfArray1[num5].Y - (ef2.Height / 2f), tfArray1[num5].X + (ef2.Width / 2f), tfArray1[num5].Y + (ef2.Height / 2f));
                        if (ef1.Contains(tf1)) {
                            this.m_pointedArea = MouseArea.LinkSquare;
                        }
                    }
                }
            }
            if (area1 == MouseArea.OutSide) {
                area1 = this.m_pointedArea;
            }
            if (this.m_cursorSetting != CursorSetting.None) {
                this.SetCursor(area1);
            }
            return area1;
        }
 
        private void CheckLicense() {
            //this.m_licensed = false;
            //try {
            //    this.license = LicenseManager.Validate(typeof(AddFlow), this);
            //    this.m_licensed = true;
            //}
            //catch {
            //    this.m_licensed = (AddFlow.m_licenseKey != null) && (AddFlowLicenseProvider.CheckLicNum(AddFlow.m_licenseKey) == 1);
            //}
            //if (!this.m_licensed) {
            //    AboutDialogBox box1 = new AboutDialogBox(this);
            //    box1.ShowDialog();
            //}
        }
 
        private void ClearDragData() {
            foreach (Node node1 in this.m_aDragNodes) {
                if (!node1.Existing) {
                    continue;
                }
                foreach (Link link1 in node1.m_aLinks) {
                    if (link1.Existing) {
                        link1.m_flag = false;
                        link1.m_drag = false;
                    }
                }
                node1.m_drag = false;
            }
            this.m_aDragNodes.Clear();
        }
 
        private void composedProperty_Change(object sender, EventArgs e) {
            if (base.DesignMode) {
                this.OnDesignModeChange(EventArgs.Empty);
            }
        }
 
        internal void ConvertSomeSizesInWorldCoordinates() {
            Graphics graphics1 = base.CreateGraphics();
            this.CoordinatesDeviceToWorld(graphics1);
            this.m_minSizeWorld = Misc.SizeDeviceToWorld(graphics1, new SizeF(3f, 3f));
            this.m_linkWidthWorld = Misc.DistanceDeviceToWorld(graphics1, (float) this.m_linkWidth);
            graphics1.Dispose();
        }
 
        internal void CoordinatesDeviceToWorld(Graphics grfx) {
            grfx.PageScale = this.m_pageScale;
            grfx.PageUnit = this.m_pageUnit;
            grfx.ScaleTransform(this.m_zoom.X, this.m_zoom.Y);
            PointF tf1 = Misc.PointDeviceToWorld(grfx, (PointF) this.ScrollPosition);
            grfx.TranslateTransform(-tf1.X, -tf1.Y);
        }
 
        internal void CoordinatesWorldToDevice(Graphics grfx) {
            grfx.ResetTransform();
            grfx.PageScale = 1f;
            grfx.PageUnit = GraphicsUnit.Pixel;
        }
 
        public Link CreateLink(Node org, Node dst) {
            Link link1 = new Link(this.m_deflink);
            this.AddLink2(link1, org, dst, false);
            return link1;
        }
 
        internal void DeleteNodes() {
            bool flag1 = false;
            if (this.m_undo.CanUndoRedo && !this.m_undo.SkipUndo) {
                flag1 = this.m_undo.IsCurrentActionGroup();
                if (!flag1) {
                    this.m_undo.BeginActionInternal(Action.ClearNodes);
                }
            }
            Node[] nodeArray1 = new Node[this.m_aNodes.Count];
            this.m_aNodes.CopyTo(nodeArray1, 0);
            Node[] nodeArray2 = nodeArray1;
            for (int num1 = 0; num1 < nodeArray2.Length; num1++) {
                Node node1 = nodeArray2[num1];
                this.RemoveNode(node1);
            }
            this.UpdateRect();
            if ((this.m_undo.CanUndoRedo && !this.m_undo.SkipUndo) && !flag1) {
                this.m_undo.EndActionInternal();
            }
        }
 
        public void DeleteSel() {
            if (this.m_selItems.Count > 0) {
                bool flag1 = false;
                if (this.m_undo.CanUndoRedo && !this.m_undo.SkipUndo) {
                    flag1 = this.m_undo.IsCurrentActionGroup();
                    if (!flag1) {
                        this.m_undo.BeginActionInternal(Action.DeleteSel);
                    }
                }
                Item[] itemArray1 = new Item[this.m_selItems.Count];
                this.m_selItems.CopyTo(itemArray1, 0);
                Item[] itemArray2 = itemArray1;
                for (int num1 = 0; num1 < itemArray2.Length; num1++) {
                    Item item1 = itemArray2[num1];
                    if (item1.Existing) {
                        if (item1 is Link) {
                            this.RemoveLink((Link) item1);
                        }
                        else {
                            this.RemoveNode((Node) item1);
                        }
                    }
                }
                this.UpdateRect();
                if ((this.m_undo.CanUndoRedo && !this.m_undo.SkipUndo) && !flag1) {
                    this.m_undo.EndActionInternal();
                }
            }
        }
 
        private void DisplaySelection(Graphics grfx) {
            Rectangle rectangle1 = new Rectangle(0, 0, this.m_selGrabSize, this.m_selGrabSize);
            Rectangle rectangle2 = new Rectangle(0, 0, this.m_linkGrabSize, this.m_linkGrabSize);
            bool flag1 = true;
            bool flag2 = true;
            Rectangle rectangle3 = Rectangle.Empty;
            Rectangle rectangle4 = Rectangle.Empty;
            foreach (Item item1 in this.m_selItems) {
                if (item1 is Node) {
                    Node node1 = (Node) item1;
                    if (this.m_linkCreationMode == LinkCreationMode.AllNodeArea) {
                        node1.DrawSelectionFrame(grfx);
                    }
                    if (!this.m_interHandles) {
                        continue;
                    }
                    PointF[] tfArray1 = node1.GetHandlePoints(grfx);
                    this.CoordinatesWorldToDevice(grfx);
                    if (item1 == this.SelectedItem) {
                        for (int num1 = 0; num1 < (tfArray1.Length - 1); num1++) {
                            AddFlow.SizeDir dir1 = (AddFlow.SizeDir) num1;
                            flag1 = (((node1.XSizeable || ((dir1 != AddFlow.SizeDir.Left) && (dir1 != AddFlow.SizeDir.Right))) && (node1.YSizeable || ((dir1 != AddFlow.SizeDir.Up) && (dir1 != AddFlow.SizeDir.Down)))) && (node1.XSizeable || node1.YSizeable)) && this.m_canSizeNode;
                            rectangle1.X = ((int) tfArray1[num1].X) - (this.m_selGrabSize / 2);
                            rectangle1.Y = ((int) tfArray1[num1].Y) - (this.m_selGrabSize / 2);
                            ControlPaint.DrawGrabHandle(grfx, rectangle1, true, flag1);
                        }
                        if ((this.m_canDrawLink && node1.OutLinkable) && (this.m_linkCreationMode == LinkCreationMode.MiddleHandle)) {
                            rectangle2.X = ((int) tfArray1[tfArray1.Length - 1].X) - (this.m_linkGrabSize / 2);
                            rectangle2.Y = ((int) tfArray1[tfArray1.Length - 1].Y) - (this.m_linkGrabSize / 2);
                            ControlPaint.DrawGrabHandle(grfx, rectangle2, true, true);
                        }
                    }
                    else {
                        for (int num2 = 0; num2 < (tfArray1.Length - 1); num2++) {
                            rectangle1.X = ((int) tfArray1[num2].X) - (this.m_selGrabSize / 2);
                            rectangle1.Y = ((int) tfArray1[num2].Y) - (this.m_selGrabSize / 2);
                            ControlPaint.DrawGrabHandle(grfx, rectangle1, false, true);
                        }
                    }
                    this.CoordinatesDeviceToWorld(grfx);
                    continue;
                }
                if (this.m_interHandles) {
                    Link link1 = (Link) item1;
                    if (item1 == this.SelectedItem) {
                        flag1 = this.m_canStretchLink && link1.m_stretchable;
                        flag2 = true;
                    }
                    else {
                        flag1 = true;
                        flag2 = false;
                    }
                    bool flag3 = link1.m_line.NewPointsAllowed;
                    int num3 = flag3 ? ((2 * link1.m_aptf.Length) - 1) : link1.m_aptf.Length;
                    for (int num4 = 0; num4 < num3; num4++) {
                        PointF tf1;
                        bool flag4 = flag1;
                        if (flag3) {
                            tf1 = ((num4 % 2) == 0) ? link1.m_aptf[num4 / 2] : Misc.MiddlePoint(link1.m_aptf[num4 / 2], link1.m_aptf[(num4 / 2) + 1]);
                        }
                        else {
                            tf1 = link1.m_aptf[num4];
                        }
                        tf1 = Misc.PointWorldToDevice(grfx, tf1);
                        rectangle1.X = ((int) tf1.X) - (this.m_selGrabSize / 2);
                        rectangle1.Y = ((int) tf1.Y) - (this.m_selGrabSize / 2);
                        if (((num4 == 0) && !this.m_canChangeOrg) && !link1.m_adjustOrg) {
                            flag4 = false;
                        }
                        if (((num4 == (num3 - 1)) && !this.m_canChangeDst) && !link1.m_adjustDst) {
                            flag4 = false;
                        }
                        this.CoordinatesWorldToDevice(grfx);
                        ControlPaint.DrawGrabHandle(grfx, rectangle1, flag2, flag4);
                        this.CoordinatesDeviceToWorld(grfx);
                    }
                }
            }
        }
 
        protected override void Dispose(bool disposing) {
            if (disposing) {
                if (this.license != null) {
                    this.license.Dispose();
                    this.license = null;
                }
                if (this.components != null) {
                    this.components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
 
        private void DoDrag(PointF pt) {
            if (this.m_selItems.Count == 0) {
                this.EndDrag();
            }
            else {
                if (this.m_beginMove) {
                    this.m_beginMove = false;
                    this.ClearDragData();
                    this.m_selrc = RectangleF.Empty;
                    foreach (Item item1 in this.m_selItems) {
                        if (!item1.Existing || !(item1 is Node)) {
                            continue;
                        }
                        Node node1 = (Node) item1;
                        if (!node1.m_drag) {
                            node1.m_b1 = node1.m_xMoveable;
                            node1.m_b2 = node1.m_yMoveable;
                            if (node1.m_b1 || node1.m_b2) {
                                this.AddNodeToDragList(node1);
                            }
                        }
                    }
                    foreach (Node node2 in this.m_aDragNodes) {
                        node2.m_task = new NodePositionTask(node2, Action.NodeMove);
                        if (this.m_selrc.IsEmpty) {
                            this.m_selrc = new RectangleF(node2.RC.Location, node2.RC.Size);
                        }
                        else {
                            this.m_selrc = RectangleF.Union(this.m_selrc, node2.m_rc);
                        }
                        foreach (Link link1 in node2.m_aLinks) {
                            if (((link1.Existing && !link1.m_drag) && (link1.m_org.m_drag && link1.m_dst.m_drag)) && ((link1.m_org.m_b1 && link1.m_org.m_b2) && (link1.m_dst.m_b1 && link1.m_dst.m_b2))) {
                                link1.m_drag = true;
                                link1.InvalidateItem();
                                link1.InvalidateHandles();
                                this.m_selrc = RectangleF.Union(this.m_selrc, link1.m_rcSeg);
                                if (link1.m_aptf.Length > 2) {
                                    link1.m_task = new LinkPositionTask(link1);
                                }
                            }
                        }
                    }
                    this.m_oldselrc = this.m_selrc;
                }
                float single1 = pt.X - this.m_ptPrior.X;
                float single2 = pt.Y - this.m_ptPrior.Y;
                if ((this.m_selrc.Left + single1) < 0f) {
                    single1 -= (this.m_selrc.Left + single1);
                }
                if ((this.m_selrc.Top + single2) < 0f) {
                    single2 -= (this.m_selrc.Top + single2);
                }
                this.m_selrc.Offset(single1, single2);
                foreach (Node node3 in this.m_aDragNodes) {
                    if (node3.Existing) {
                        foreach (Link link2 in node3.m_aLinks) {
                            link2.m_flag = false;
                        }
                        continue;
                    }
                }
                foreach (Node node4 in this.m_aDragNodes) {
                    if (node4.Existing) {
                        if (!node4.m_b1) {
                            single1 = 0f;
                        }
                        if (!node4.m_b2) {
                            single2 = 0f;
                        }
                        node4.Move(new PointF(single1, single2));
                    }
                }
                this.UpdateRect();
            }
        }
 
        private void DoLink(Graphics grfx, PointF pt) {
            if (!this.m_canDrawLink) {
                this.m_interactiveAction = InteractiveAction.None;
            }
            else {
                RectangleF ef1 = Misc.Rect(this.m_ptOrg, this.m_ptPrior);
                ef1 = Misc.RectWorldToDevice(grfx, ef1);
                ef1.Inflate((float) ((2 * this.m_deflink.DrawWidth) + 2), (float) ((2 * this.m_deflink.DrawWidth) + 2));
                base.Invalidate(Rectangle.Round(ef1));
                ef1 = Misc.Rect(this.m_ptOrg, pt);
                ef1 = Misc.RectWorldToDevice(grfx, ef1);
                ef1.Inflate((float) ((2 * this.m_deflink.DrawWidth) + 2), (float) ((2 * this.m_deflink.DrawWidth) + 2));
                base.Invalidate(Rectangle.Round(ef1));
            }
        }
 
        private void DoNode(Graphics grfx, PointF pt) {
            if (!this.m_canDrawNode) {
                this.m_interactiveAction = InteractiveAction.None;
            }
            else {
                RectangleF ef1 = Misc.Rect(this.m_ptOrg, this.m_ptPrior);
                ef1 = Misc.RectWorldToDevice(grfx, ef1);
                ef1.Inflate(this.m_outlineSize.Width, this.m_outlineSize.Height);
                base.Invalidate(Rectangle.Round(ef1));
                ef1 = Misc.Rect(this.m_ptOrg, pt);
                ef1 = Misc.RectWorldToDevice(grfx, ef1);
                ef1.Inflate(this.m_outlineSize.Width, this.m_outlineSize.Height);
                base.Invalidate(Rectangle.Round(ef1));
            }
        }
 
        private void DoSelect(Graphics grfx, PointF pt) {
            RectangleF ef1 = this.m_tmprc;
            ef1 = Misc.RectWorldToDevice(grfx, ef1);
            ef1.Inflate(this.m_outlineSize.Width, this.m_outlineSize.Height);
            base.Invalidate(Rectangle.Round(ef1));
            this.m_tmprc = Misc.Rect(this.m_ptOrg, pt);
            ef1 = this.m_tmprc;
            ef1 = Misc.RectWorldToDevice(grfx, ef1);
            ef1.Inflate(this.m_outlineSize.Width, this.m_outlineSize.Height);
            base.Invalidate(Rectangle.Round(ef1));
        }
 
        private void DoSize(PointF pt) {
            if (this.SelectedItem == null) {
                this.EndSize();
            }
            else {
                if (pt.X < 0f) {
                    pt.X = 0f;
                }
                if (pt.Y < 0f) {
                    pt.Y = 0f;
                }
                if (this.m_beginMove) {
                    this.m_beginMove = false;
                    this.m_resizedNode = (Node) this.SelectedItem;
                    if ((this.m_resizedNode != null) && this.m_resizedNode.Existing) {
                        this.m_resizedNode.m_task = new NodePositionTask(this.m_resizedNode, Action.NodeResize);
                        this.m_savedAutoSize = this.m_resizedNode.m_autoSize;
                        if ((this.m_savedAutoSize == AutoSize.NodeToImage) || (this.m_savedAutoSize == AutoSize.NodeToText)) {
                            this.m_resizedNode.m_autoSize = AutoSize.None;
                        }
                    }
                }
                if ((this.m_resizedNode != null) && this.m_resizedNode.Existing) {
                    Node node1 = this.m_resizedNode;
                    node1.InvalidateItem();
                    node1.InvalidateHandles();
                    RectangleF ef1 = new RectangleF(node1.RC.Location, node1.RC.Size);
                    float single1 = ef1.Left;
                    float single2 = ef1.Top;
                    float single3 = ef1.Right;
                    float single4 = ef1.Bottom;
                    if (node1.m_xSizeable) {
                        switch (this.m_sizeDir) {
                        case AddFlow.SizeDir.LeftUp:
                        case AddFlow.SizeDir.Left:
                        case AddFlow.SizeDir.LeftDown: {
                            single1 = Math.Min(pt.X, (float) (ef1.Right - this.m_minSizeWorld.Width));
                            if (single1 < 0f) {
                                single1 = 0f;
                            }
                            break;
                        }
                        case AddFlow.SizeDir.RightUp:
                        case AddFlow.SizeDir.Right:
                        case AddFlow.SizeDir.RightDown: {
                            single3 = Math.Max(pt.X, (float) (ef1.Left + this.m_minSizeWorld.Height));
                            break;
                        }
                        }
                    }
                    if (node1.m_ySizeable) {
                        switch (this.m_sizeDir) {
                        case AddFlow.SizeDir.LeftUp:
                        case AddFlow.SizeDir.Up:
                        case AddFlow.SizeDir.RightUp: {
                            single2 = Math.Min(pt.Y, (float) (ef1.Bottom - this.m_minSizeWorld.Width));
                            if (single2 < 0f) {
                                single2 = 0f;
                            }
                            break;
                        }
                        case AddFlow.SizeDir.LeftDown:
                        case AddFlow.SizeDir.Down:
                        case AddFlow.SizeDir.RightDown: {
                            single4 = Math.Max(pt.Y, (float) (ef1.Top + this.m_minSizeWorld.Height));
                            break;
                        }
                        }
                    }
                    node1.m_rc = RectangleF.FromLTRB(Math.Min(single1, single3), Math.Min(single2, single4), Math.Max(single1, single3), Math.Max(single2, single4));
                    foreach (Link link1 in node1.m_aLinks) {
                        if (!link1.m_flag) {
                            if (link1.m_adjustDst && (node1 == link1.m_dst)) {
                                float single5;
                                float single6;
                                int num1 = link1.m_aptf.Length;
                                if (ef1.Width == 0f) {
                                    single5 = 0f;
                                }
                                else {
                                    single5 = ((link1.m_aptf[num1 - 1].X - ef1.Left) * node1.RC.Width) / ef1.Width;
                                }
                                if (ef1.Height == 0f) {
                                    single6 = 0f;
                                }
                                else {
                                    single6 = ((link1.m_aptf[num1 - 1].Y - ef1.Top) * node1.RC.Height) / ef1.Height;
                                }
                                link1.m_aptf[num1 - 1].X = node1.RC.Left + single5;
                                link1.m_aptf[num1 - 1].Y = node1.RC.Top + single6;
                            }
                            if (link1.m_adjustOrg && (node1 == link1.m_org)) {
                                float single7;
                                float single8;
                                if (ef1.Width == 0f) {
                                    single7 = 0f;
                                }
                                else {
                                    single7 = ((link1.m_aptf[0].X - ef1.Left) * node1.RC.Width) / ef1.Width;
                                }
                                if (ef1.Height == 0f) {
                                    single8 = 0f;
                                }
                                else {
                                    single8 = ((link1.m_aptf[0].Y - ef1.Top) * node1.RC.Height) / ef1.Height;
                                }
                                link1.m_aptf[0].X = node1.RC.Left + single7;
                                link1.m_aptf[0].Y = node1.RC.Top + single8;
                            }
                        }
                        if (link1.Reflexive) {
                            link1.m_flag = !link1.m_flag;
                        }
                    }
                    node1.AdjustNodeLinks();
                    node1.InvalidateItem();
                    node1.InvalidateHandles();
                    this.UpdateRect();
                }
            }
        }
 
        private void DoStretch(PointF pt) {
            if (this.SelectedItem == null) {
                this.EndStretch();
            }
            else {
                if (pt.X < 0f) {
                    pt.X = 0f;
                }
                if (pt.Y < 0f) {
                    pt.Y = 0f;
                }
                if (this.m_beginMove) {
                    this.m_stretchedLink = (Link) this.SelectedItem;
                    if ((this.m_stretchedLink != null) && this.m_stretchedLink.Existing) {
                        this.m_stretchedLink.m_task = new StretchLinkTask(this.m_stretchedLink);
                    }
                }
                if ((this.m_stretchedLink != null) && this.m_stretchedLink.Existing) {
                    this.m_stretchedLink.InvalidateItem();
                    this.m_stretchedLink.InvalidateHandles();
                    if (!this.m_stretchedLink.m_line.HVStyle) {
                        this.StretchPoly(pt, this.m_stretchedLink);
                    }
                    else if (this.m_stretchedLink.m_line.m_orthogonalDynamic) {
                        this.StretchDyn(pt, this.m_stretchedLink);
                    }
                    else {
                        this.StretchVH(pt, this.m_stretchedLink);
                    }
                    this.m_stretchedLink.CalcRect();
                    this.m_stretchedLink.InvalidateItem();
                    this.m_stretchedLink.InvalidateHandles();
                    this.UpdateRect();
                }
            }
        }
 
        private void DragScroll() {
            if (this.m_nTimer < this.m_aTimerDuration.Length) {
                this.m_timerScroll.Interval = this.m_aTimerDuration[this.m_nTimer++];
            }
            this.DrawMoveShape(this.m_ptOut);
            Point point1 = this.ScrollPosition;
            if (this.m_xScrollDir != AddFlow.ScrollDir.None) {
                int num1;
                if ((this.m_grid.m_snap && (this.m_interactiveAction != InteractiveAction.Select)) && (this.m_interactiveAction != InteractiveAction.Link)) {
                    num1 = (this.m_scrollUnit.Width / this.m_grid.m_size.Width) * this.m_grid.m_size.Width;
                    if (num1 == 0) {
                        num1 = this.m_grid.m_size.Width;
                    }
                }
                else {
                    num1 = this.m_scrollUnit.Width;
                }
                if (this.m_xScrollDir == AddFlow.ScrollDir.Right) {
                    point1.X += num1;
                }
                else {
                    point1.X -= num1;
                }
            }
            if (this.m_yScrollDir != AddFlow.ScrollDir.None) {
                int num2;
                if ((this.m_grid.m_snap && (this.m_interactiveAction != InteractiveAction.Select)) && (this.m_interactiveAction != InteractiveAction.Link)) {
                    num2 = (this.m_scrollUnit.Height / this.m_grid.m_size.Height) * this.m_grid.m_size.Height;
                    if (num2 == 0) {
                        num2 = this.m_grid.m_size.Height;
                    }
                }
                else {
                    num2 = this.m_scrollUnit.Height;
                }
                if (this.m_yScrollDir == AddFlow.ScrollDir.Bottom) {
                    point1.Y += num2;
                }
                else {
                    point1.Y -= num2;
                }
            }
            this.ScrollPosition = point1;
            this.OnScroll(EventArgs.Empty);
            this.DrawMoveShape(this.m_ptOut);
        }
 
        private void DrawMoveShape(PointF pt) {
            Graphics graphics1 = base.CreateGraphics();
            this.CoordinatesDeviceToWorld(graphics1);
            AddFlow.SizeDir dir1 = this.m_sizeDir;
            pt = Misc.PointDeviceToWorld(graphics1, pt);
            this.CheckArea(graphics1, pt);
            if (this.m_mouseDown) {
                this.m_sizeDir = dir1;
                if ((this.m_grid.m_snap && (this.m_interactiveAction != InteractiveAction.Select)) && (this.m_interactiveAction != InteractiveAction.Link)) {
                    pt = this.m_grid.Adjust(pt);
                }
                switch (this.m_interactiveAction) {
                case InteractiveAction.Node: {
                    this.DoNode(graphics1, pt);
                    break;
                }
                case InteractiveAction.Link: {
                    this.DoLink(graphics1, pt);
                    break;
                }
                case InteractiveAction.Drag: {
                    this.DoDrag(pt);
                    break;
                }
                case InteractiveAction.Size: {
                    this.DoSize(pt);
                    break;
                }
                case InteractiveAction.Stretch: {
                    this.DoStretch(pt);
                    break;
                }
                case InteractiveAction.Select: {
                    this.DoSelect(graphics1, pt);
                    break;
                }
                }
            }
            graphics1.Dispose();
            this.m_ptPrior = pt;
        }
 
        private void DrawOutLine(Graphics grfx) {
            switch (this.m_interactiveAction) {
            case InteractiveAction.Node: {
                RectangleF ef1 = Misc.Rect(this.m_ptOrg, this.m_ptPrior);
                GraphicsPath path1 = this.m_defnode.m_shape.GetPathOfShape(ef1);
                grfx.DrawPath(new Pen(this.ForeColor, 0f), path1);
                return;
            }
            case InteractiveAction.Link: {
                grfx.DrawLine(new Pen(this.ForeColor, 0f), this.m_ptOrg, this.m_ptPrior);
                return;
            }
            case InteractiveAction.Select: {
                Rectangle rectangle1 = Rectangle.Round(this.m_tmprc);
                rectangle1 = Misc.RectWorldToDevice(grfx, rectangle1);
                this.CoordinatesWorldToDevice(grfx);
                ControlPaint.DrawFocusRectangle(grfx, rectangle1);
                this.CoordinatesDeviceToWorld(grfx);
                return;
            }
            }
        }
 
        public void EndAction() {
            this.m_undo.EndAction();
        }
 
        private void EndDrag() {
            if (!this.m_selrc.Equals(this.m_oldselrc)) {
                this.m_undo.BeginActionInternal(Action.MoveItems);
                foreach (Node node1 in this.m_aDragNodes) {
                    if (!node1.Existing) {
                        continue;
                    }
                    if (this.m_undo.CanUndoRedo && !this.m_undo.SkipUndo) {
                        this.m_undo.SubmitTask(node1.m_task);
                    }
                    foreach (Link link1 in node1.m_aLinks) {
                        if (link1.Existing && link1.m_drag) {
                            link1.m_drag = false;
                            if (((link1.m_aptf.Length > 2) && this.m_undo.CanUndoRedo) && !this.m_undo.SkipUndo) {
                                this.m_undo.SubmitTask(link1.m_task);
                            }
                        }
                    }
                    node1.InvalidateItem();
                    node1.InvalidateHandles();
                }
                this.m_undo.EndActionInternal();
                this.ClearDragData();
                this.UpdateRect();
                this.OnAfterMove(EventArgs.Empty);
                this.IncrementChangedFlag(1);
            }
        }
 
        private void EndLink() {
            RectangleF ef1 = Misc.Rect(this.m_ptOrg, this.m_ptPrior);
            Graphics graphics1 = base.CreateGraphics();
            this.CoordinatesDeviceToWorld(graphics1);
            ef1 = Misc.RectWorldToDevice(graphics1, ef1);
            ef1.Inflate((float) ((2 * this.m_deflink.DrawWidth) + 2), (float) ((2 * this.m_deflink.DrawWidth) + 2));
            graphics1.Dispose();
            base.Invalidate(Rectangle.Round(ef1));
            if (this.m_canDrawLink) {
                this.m_ptDst = this.m_ptPrior;
                Node node1 = (Node) this.GetItemByPoint(this.m_ptPrior, ItemSet.Nodes);
                if ((node1 != null) && (this.m_org != null)) {
                    if (!node1.InLinkable) {
                        if (this.m_canFireError) {
                            this.OnError(new ErrorEventArgs(ErrorEventType.IncomingLinkError));
                        }
                    }
                    else {
                        if (this.m_org == node1) {
                            if (!this.m_out) {
                                return;
                            }
                            if (!this.m_canReflexLink) {
                                if (this.m_canFireError) {
                                    this.OnError(new ErrorEventArgs(ErrorEventType.ReflexiveError));
                                }
                                return;
                            }
                        }
                        if (!this.m_canMultiLink && this.m_org.IsOriginOf(node1)) {
                            if (this.m_canFireError) {
                                this.OnError(new ErrorEventArgs(ErrorEventType.MultilinkError));
                            }
                        }
                        else {
                            Link link1 = new Link(this.m_deflink);
                            this.AddLink2(link1, this.m_org, node1, true);
                            if (((link1 != null) && link1.Existing) && link1.m_selectable) {
                                this.SelectedItem = link1;
                            }
                        }
                    }
                }
            }
        }
 
        private void EndNode() {
            RectangleF ef1 = Misc.Rect(this.m_ptOrg, this.m_ptPrior);
            if (ef1.X < 0f) {
                ef1.X = 0f;
            }
            if (ef1.Y < 0f) {
                ef1.Y = 0f;
            }
            this.m_rc = ef1;
            Graphics graphics1 = base.CreateGraphics();
            this.CoordinatesDeviceToWorld(graphics1);
            ef1 = Misc.RectWorldToDevice(graphics1, ef1);
            ef1.Inflate(this.m_outlineSize.Width, this.m_outlineSize.Height);
            graphics1.Dispose();
            base.Invalidate(Rectangle.Round(ef1));
            if (this.m_canDrawNode) {
                Node node1 = new Node(this.m_rc, this.m_defnode);
                this.AddNode(node1, true);
                if (((node1 != null) && node1.Existing) && node1.m_selectable) {
                    this.SelectedItem = node1;
                }
            }
        }
 
        private void EndSelect() {
            Item item1 = this.SelectedItem;
            if (item1 != null) {
                item1.InvalidateHandles();
            }
            RectangleF ef1 = this.m_tmprc;
            Graphics graphics1 = base.CreateGraphics();
            this.CoordinatesDeviceToWorld(graphics1);
            ef1 = Misc.RectWorldToDevice(graphics1, ef1);
            graphics1.Dispose();
            ef1.Inflate(this.m_outlineSize.Width, this.m_outlineSize.Height);
            base.Invalidate(Rectangle.Round(ef1));
            switch (this.m_mouseAction) {
            case MouseAction.Selection: {
                this.SelectRectangle(this.m_tmprc, true);
                break;
            }
            case MouseAction.Selection2: {
                this.SelectRectangle(this.m_tmprc, false);
                break;
            }
            case MouseAction.Zoom: {
                this.ZoomRectangle(this.m_tmprc, ZoomType.Anisotropic);
                break;
            }
            case MouseAction.ZoomIsotropic: {
                this.ZoomRectangle(this.m_tmprc, ZoomType.Isotropic);
                break;
            }
            }
            this.OnAfterSelect(EventArgs.Empty);
        }
 
        private void EndSelWork() {
            this.m_interactiveAction = this.m_priorMode;
        }
 
        private void EndSize() {
            if ((this.m_resizedNode != null) && this.m_resizedNode.Existing) {
                Node node1 = this.m_resizedNode;
                this.m_resizedNode = null;
                NodePositionTask task1 = (NodePositionTask) node1.m_task;
                if (!task1.m_oldrc.Equals(node1.m_rc)) {
                    RectangleF ef1 = node1.m_rc;
                    Graphics graphics1 = base.CreateGraphics();
                    this.CoordinatesDeviceToWorld(graphics1);
                    SizeF ef2 = Misc.SizeDeviceToWorld(graphics1, new SizeF((float) this.m_selGrabSize, (float) this.m_selGrabSize));
                    graphics1.Dispose();
                    if ((ef1.Right - ef1.Left) < ef2.Width) {
                        ef1.Width = this.m_minSizeWorld.Width;
                    }
                    if ((ef1.Bottom - ef1.Top) < ef2.Height) {
                        ef1.Height = this.m_minSizeWorld.Height;
                    }
                    node1.m_rc = ef1;
                    if (this.m_savedAutoSize >= AutoSize.NodeToImage) {
                        node1.m_autoSize = this.m_savedAutoSize;
                    }
                    if ((node1.m_autoSize != AutoSize.None) && (node1.m_autoSize != AutoSize.ImageToNode)) {
                        node1.InvalidateItem();
                        node1.InvalidateHandles();
                        switch (node1.m_autoSize) {
                        case AutoSize.NodeToImage: {
                            node1.AdjustNodeSizeToImage();
                            break;
                        }
                        case AutoSize.NodeToText: {
                            node1.AdjustNodeSizeToText();
                            break;
                        }
                        }
                        node1.AdjustNodeLinks();
                    }
                    if (this.m_undo.CanUndoRedo && !this.m_undo.SkipUndo) {
                        this.m_undo.SubmitTask(node1.m_task);
                    }
                    else {
                        node1.m_task = null;
                    }
                    node1.InvalidateItem();
                    node1.InvalidateHandles();
                    this.UpdateRect2(node1.GetRectShadow());
                    this.OnAfterResize(EventArgs.Empty);
                    this.IncrementChangedFlag(1);
                }
            }
        }
 
        private void EndStretch() {
            if ((this.m_stretchedLink != null) && this.m_stretchedLink.Existing) {
                Link link1 = this.m_stretchedLink;
                this.m_stretchedLink = null;
                link1.InvalidateItem();
                link1.InvalidateHandles();
                this.m_handle = Math.Max(Math.Min(this.m_handle, (int) (link1.m_aptf.Length - 1)), 0);
                PointF tf1 = link1.m_aptf[this.m_handle];
                if ((link1.m_line.NewPointsAllowed && ((this.m_stretchType == AddFlow.Stretch.Add) || (this.m_stretchType == AddFlow.Stretch.Del))) && ((this.m_handle > 0) && (this.m_handle < (link1.m_aptf.Length - 1)))) {
                    float single2;
                    float single1 = Misc.GetSegDist(link1.m_aptf[this.m_handle - 1], link1.m_aptf[this.m_handle + 1], link1.m_aptf[this.m_handle]);
                    switch (this.m_removePointAngle) {
                    case RemovePointAngle.None: {
                        single2 = 0f;
                        break;
                    }
                    case RemovePointAngle.Small: {
                        single2 = this.m_linkWidthWorld / 2f;
                        break;
                    }
                    case RemovePointAngle.Large: {
                        single2 = 2f * this.m_linkWidthWorld;
                        break;
                    }
                    default: {
                        single2 = this.m_linkWidthWorld;
                        break;
                    }
                    }
                    if (single1 <= single2) {
                        link1.m_aptf = link1.RemovePoint(this.m_handle);
                    }
                }
                if (link1.m_line.HVStyle) {
                    link1.FixLinkPoints();
                }
                link1.CalcLinkTips(!link1.m_adjustOrg, !link1.m_adjustDst);
                if ((this.m_stretchType == AddFlow.Stretch.First) && this.m_canChangeOrg) {
                    Node node1 = (Node) this.GetItemByPoint(tf1, ItemSet.Nodes);
                    if ((node1 != null) && (node1 != link1.m_org)) {
                        if (!this.IsNewOrgAllowed(link1, node1)) {
                            link1.m_task.Undo();
                            link1.m_aptf2 = null;
                            return;
                        }
                        BeforeChangeOrgEventArgs args1 = new BeforeChangeOrgEventArgs(link1, node1);
                        this.OnBeforeChangeOrg(args1);
                        if (args1.Cancel.Cancel) {
                            link1.m_task.Undo();
                            link1.m_aptf2 = null;
                            return;
                        }
                        link1.SetOrg2(node1);
                        if (((this.m_cycleMode == CycleMode.NoDirectedCycle) && this.m_connect.IsCycleDirected()) || ((this.m_cycleMode == CycleMode.NoCycle) && this.m_connect.IsCycle(link1.m_dst))) {
                            if (this.m_canFireError) {
                                if (this.m_cycleMode == CycleMode.NoDirectedCycle) {
                                    this.OnError(new ErrorEventArgs(ErrorEventType.DirectedCycleError));
                                }
                                else {
                                    this.OnError(new ErrorEventArgs(ErrorEventType.CycleError));
                                }
                            }
                            link1.m_task.Undo();
                            link1.m_aptf2 = null;
                            return;
                        }
                    }
                }
                if ((this.m_stretchType == AddFlow.Stretch.Last) && this.m_canChangeDst) {
                    Node node2 = (Node) this.GetItemByPoint(tf1, ItemSet.Nodes);
                    if ((node2 != null) && (node2 != link1.m_dst)) {
                        if (!this.IsNewDstAllowed(link1, node2)) {
                            link1.m_task.Undo();
                            link1.m_aptf2 = null;
                            return;
                        }
                        BeforeChangeDstEventArgs args2 = new BeforeChangeDstEventArgs(link1, node2);
                        this.OnBeforeChangeDst(args2);
                        if (args2.Cancel.Cancel) {
                            link1.m_task.Undo();
                            link1.m_aptf2 = null;
                            return;
                        }
                        link1.SetDst2(node2);
                        if (((this.m_cycleMode == CycleMode.NoDirectedCycle) && this.m_connect.IsCycleDirected()) || ((this.m_cycleMode == CycleMode.NoCycle) && this.m_connect.IsCycle(link1.m_org))) {
                            if (this.m_canFireError) {
                                if (this.m_cycleMode == CycleMode.NoDirectedCycle) {
                                    this.OnError(new ErrorEventArgs(ErrorEventType.DirectedCycleError));
                                }
                                else {
                                    this.OnError(new ErrorEventArgs(ErrorEventType.CycleError));
                                }
                            }
                            link1.m_task.Undo();
                            link1.m_aptf2 = null;
                            return;
                        }
                    }
                }
                if (this.m_undo.CanUndoRedo && !this.m_undo.SkipUndo) {
                    this.m_undo.SubmitTask(link1.m_task);
                }
                link1.m_aptf2 = null;
                link1.CalcRect();
                link1.InvalidateItem();
                link1.InvalidateHandles();
                this.UpdateRect2(link1.m_rcSeg);
                this.OnAfterStretch(EventArgs.Empty);
                this.IncrementChangedFlag(1);
            }
        }
 
        public void EndUpdate() {
            this.m_repaint--;
            if (this.m_repaint == 0) {
                if (this.AutoScroll) {
                    this.UpdateScrollInfo();
                }
                base.Invalidate();
            }
        }
 
        public Metafile ExportMetafile(bool selected, bool includeBackColor, bool zeroOrigin) {
            return this.ExportMetafile(selected, includeBackColor, zeroOrigin, false, false);
        }
 
        public Metafile ExportMetafile(bool selected, bool includeBackColor, bool zeroOrigin, bool keepZoom) {
            return this.ExportMetafile(selected, includeBackColor, zeroOrigin, keepZoom, false);
        }
 
        public Metafile ExportMetafile(bool selected, bool includeBackColor, bool zeroOrigin, bool keepZoom, bool printerResolution) {
            Graphics graphics1;
            this.m_drawAll = true;
            if (printerResolution) {
                PrinterSettings settings1 = new PrinterSettings();
                graphics1 = settings1.CreateMeasurementGraphics();
            }
            else {
                graphics1 = base.CreateGraphics();
            }
            IntPtr ptr1 = graphics1.GetHdc();
            Metafile metafile1 = new Metafile(ptr1, EmfType.EmfPlusDual, "Created with AddFlow");
            graphics1.ReleaseHdc(ptr1);
            if (!printerResolution) {
                graphics1.Dispose();
            }
            graphics1 = Graphics.FromImage(metafile1);
            if (keepZoom) {
                this.CoordinatesDeviceToWorld(graphics1);
            }
            else {
                graphics1.PageScale = this.m_pageScale;
                graphics1.PageUnit = this.m_pageUnit;
            }
            if (this.m_antiAliasing) {
                graphics1.SmoothingMode = SmoothingMode.HighQuality;
                graphics1.PixelOffsetMode = PixelOffsetMode.HighQuality;
            }
            if (includeBackColor) {
                graphics1.Clear(this.BackColor);
            }
            if (zeroOrigin) {
                graphics1.DrawRectangle(Pens.Transparent, 0, 0, 1, 1);
            }
            foreach (Item item1 in this.m_items) {
                if (selected && !item1.m_selected) {
                    continue;
                }
                if (!item1.m_hidden) {
                    item1.Draw(graphics1);
                }
            }
            if (this.m_antiAliasing) {
                graphics1.SmoothingMode = SmoothingMode.None;
                graphics1.PixelOffsetMode = PixelOffsetMode.None;
            }
            graphics1.Dispose();
            this.m_drawAll = false;
            return metafile1;
        }
 
        internal int GetChangedFlag() {
            return this.m_changed;
        }
 
        internal Rectangle GetClientRectangleInWorldCoordinates() {
            Graphics graphics1 = base.CreateGraphics();
            this.CoordinatesDeviceToWorld(graphics1);
            Rectangle rectangle1 = Misc.RectDeviceToWorld(graphics1, base.ClientRectangle);
            graphics1.Dispose();
            return rectangle1;
        }
 
        private DrawFlags GetDrawFlags(Graphics grfx) {
            DiagramOwnerDrawEventArgs args1 = new DiagramOwnerDrawEventArgs(grfx);
            this.OnDiagramOwnerDraw(args1);
            return args1.Flags;
        }
 
        internal static string GetExceptionText(AddFlow.ExceptionType e) {
            Assembly assembly1 = Assembly.GetExecutingAssembly();
            ResourceManager manager1 = new ResourceManager("Lassalle.Flow.af", assembly1);
            string text1 = "";
            switch (e) {
            case AddFlow.ExceptionType.AdjustOrg: {
                return (string) manager1.GetObject("AdjustOrgError");
            }
            case AddFlow.ExceptionType.AdjustDst: {
                return (string) manager1.GetObject("AdjustDstError");
            }
            case AddFlow.ExceptionType.ChangePoints: {
                return (string) manager1.GetObject("ChangePointsError");
            }
            case AddFlow.ExceptionType.ItemAlreadyOwned: {
                return (string) manager1.GetObject("ItemAlreadyOwnedError");
            }
            case AddFlow.ExceptionType.ItemMustBeInGraph: {
                return (string) manager1.GetObject("ItemMustBeInGraphError");
            }
            case AddFlow.ExceptionType.OutOfAreaError: {
                return (string) manager1.GetObject("OutOfAreaError");
            }
            case AddFlow.ExceptionType.ArgumentsNotValid: {
                return (string) manager1.GetObject("ArgumentsNotValid");
            }
            case AddFlow.ExceptionType.ZoomMustBePositive: {
                return (string) manager1.GetObject("ZoomMustBePositive");
            }
            case AddFlow.ExceptionType.TextMarginLimits: {
                return (string) manager1.GetObject("TextMarginLimits");
            }
            case AddFlow.ExceptionType.ShadowSizeCannotBeNegative: {
                return (string) manager1.GetObject("ShadowSizeCannotBeNegative");
            }
            }
            return "";
        }
 
        public Item GetItemAt(Point pt) {
            return this.GetItemAt(pt, ItemSet.SelectableItems);
        }
 
        public Item GetItemAt(Point pt, ItemSet itemSet) {
            return this.GetItemByPoint((PointF) pt, itemSet);
        }
 
        private Item GetItemByPoint(PointF pt, ItemSet itemSet) {
            Item item1 = null;
            float single1 = this.m_linkWidthWorld;
            foreach (Item item2 in this.m_items) {
                switch (itemSet) {
                case ItemSet.Items: {
                    if (!(item2 is Node)) {
                        goto Label_0071;
                    }
                    Node node1 = (Node) item2;
                    if (node1.HitTest(pt)) {
                        item1 = node1;
                    }
                    continue;
                }
                case ItemSet.Nodes: {
                    if (item2 is Node) {
                        Node node2 = (Node) item2;
                        if (node2.HitTest(pt)) {
                            item1 = node2;
                        }
                    }
                    continue;
                }
                case ItemSet.Links: {
                    if (item2 is Link) {
                        Link link2 = (Link) item2;
                        if (link2.HitTest(pt) && (Link.m_distance < single1)) {
                            single1 = Link.m_distance;
                            item1 = link2;
                        }
                    }
                    continue;
                }
                case ItemSet.SelectableItems: {
                    if (item2.m_selectable) {
                        if (!(item2 is Node)) {
                            goto Label_013B;
                        }
                        Node node3 = (Node) item2;
                        if (node3.HitTest(pt)) {
                            item1 = node3;
                        }
                    }
                    continue;
                }
                case ItemSet.SelectableNodes: {
                    if (item2.m_selectable && (item2 is Node)) {
                        Node node4 = (Node) item2;
                        if (node4.HitTest(pt)) {
                            item1 = node4;
                        }
                    }
                    continue;
                }
                case ItemSet.SelectableLinks: {
                    goto Label_018F;
                }
                default: {
                    continue;
                }
                }
            Label_0071:
                if (item2 is Link) {
                    Link link1 = (Link) item2;
                    if (link1.HitTest(pt) && (Link.m_distance < single1)) {
                        single1 = Link.m_distance;
                        item1 = link1;
                    }
                }
                continue;
            Label_013B:
                if (item2 is Link) {
                    Link link3 = (Link) item2;
                    if (link3.HitTest(pt) && (Link.m_distance < single1)) {
                        single1 = Link.m_distance;
                        item1 = link3;
                    }
                }
                continue;
            Label_018F:
                if (item2.m_selectable && (item2 is Link)) {
                    Link link4 = (Link) item2;
                    if (link4.HitTest(pt) && (Link.m_distance < single1)) {
                        single1 = Link.m_distance;
                        item1 = link4;
                    }
                }
            }
            return item1;
        }
 
        private ArrayList GetItemsInRect(RectangleF selrc, bool partial) {
            ArrayList list1 = new ArrayList();
            foreach (Item item1 in this.m_items) {
                bool flag1 = false;
                if (item1.RC.IsEmpty) {
                    PointF tf1 = item1.RC.Location;
                    flag1 = selrc.Contains(tf1);
                }
                else {
                    RectangleF ef1 = new RectangleF(item1.RC.Location, item1.RC.Size);
                    if (item1 is Link) {
                        ef1.Inflate(-this.m_linkWidthWorld, -this.m_linkWidthWorld);
                    }
                    if (partial) {
                        RectangleF ef2 = RectangleF.Intersect(selrc, ef1);
                        flag1 = !ef2.IsEmpty;
                        if (flag1 && (item1 is Link)) {
                            Link link1 = (Link) item1;
                            if (((Math.Abs((float) (ef2.Left - ef1.Left)) > 0.001) || (Math.Abs((float) (ef2.Top - ef1.Top)) > 0.001)) || ((Math.Abs((float) (ef2.Width - ef1.Width)) > 0.001) || (Math.Abs((float) (ef2.Height - ef1.Height)) > 0.001))) {
                                flag1 = Misc.InterPolylineRect(link1.m_aptf, link1.m_aptf.Length, selrc);
                            }
                        }
                    }
                    else {
                        flag1 = selrc.Contains(ef1);
                    }
                }
                if (flag1) {
                    list1.Add(item1);
                }
            }
            return list1;
        }
 
        public ArrayList GetItemsInRectangle(RectangleF selRectangle) {
            return this.GetItemsInRect(selRectangle, true);
        }
 
        public ArrayList GetItemsInRectangle(RectangleF selRectangle, bool partial) {
            return this.GetItemsInRect(selRectangle, partial);
        }
 
        internal LinkDrawFlags GetLinkDrawFlags(Graphics grfx, Link link) {
            LinkOwnerDrawEventArgs args1 = new LinkOwnerDrawEventArgs(link, grfx);
            this.OnLinkOwnerDraw(args1);
            return args1.Flags;
        }
 
        internal NodeDrawFlags GetNodeDrawFlags(Graphics grfx, Node node) {
            NodeOwnerDrawEventArgs args1 = new NodeOwnerDrawEventArgs(node, grfx);
            this.OnNodeOwnerDraw(args1);
            return args1.Flags;
        }
 
        internal void IncrementChangedFlag(int inc) {
            this.m_changed += inc;
        }
 
        internal void internalRender(Graphics grfx) {
            if (!this.m_ownerDraw || (this.GetDrawFlags(grfx) != DrawFlags.None)) {
                if (this.m_antiAliasing) {
                    grfx.SmoothingMode = SmoothingMode.HighQuality;
                    grfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
                }
                foreach (Item item1 in this.m_items) {
                    if (!item1.m_hidden) {
                        item1.Draw(grfx);
                    }
                }
                if (this.m_antiAliasing) {
                    grfx.SmoothingMode = SmoothingMode.None;
                    grfx.PixelOffsetMode = PixelOffsetMode.None;
                }
            }
        }
 
        internal void internalRender(Graphics grfx, Rectangle clipRect) {
            if (!this.m_ownerDraw || (this.GetDrawFlags(grfx) != DrawFlags.None)) {
                if (this.m_antiAliasing) {
                    grfx.SmoothingMode = SmoothingMode.HighQuality;
                    grfx.PixelOffsetMode = PixelOffsetMode.HighQuality;
                }
                foreach (Item item1 in this.m_items) {
                    RectangleF ef1 = (item1 is Node) ? ((Node) item1).GetRectShadow() : item1.m_rc;
                    ef1.Inflate((float) item1.m_drawWidth, (float) item1.m_drawWidth);
                    if (!item1.m_hidden && ef1.IntersectsWith((RectangleF) clipRect)) {
                        item1.Draw(grfx);
                    }
                }
                if (this.m_antiAliasing) {
                    grfx.SmoothingMode = SmoothingMode.None;
                    grfx.PixelOffsetMode = PixelOffsetMode.None;
                }
            }
        }
 
        private bool IsMouseOverDragFrame(Graphics grfx, PointF ptDev) {
            foreach (Item item1 in this.m_selItems) {
                if (!(item1 is Node)) {
                    continue;
                }
                Node node1 = (Node) item1;
                RectangleF ef1 = Misc.RectWorldToDevice(grfx, node1.m_rc);
                RectangleF ef2 = new RectangleF(ef1.Location, ef1.Size);
                ef2.Inflate((float) this.m_selGrabSize, (float) this.m_selGrabSize);
                if (ef2.Contains(ptDev) && !ef1.Contains(ptDev)) {
                    return true;
                }
            }
            return false;
        }
 
        private bool IsMouseOverSquare() {
            if (this.m_pointedArea <= MouseArea.LinkSquare) {
                return (this.m_pointedArea >= MouseArea.LeftUpSquare);
            }
            return false;
        }
 
        private bool IsNewDstAllowed(Link link, Node newDst) {
            if (!newDst.InLinkable) {
                if (this.m_canFireError) {
                    this.OnError(new ErrorEventArgs(ErrorEventType.IncomingLinkError));
                }
                return false;
            }
            if (!this.m_canReflexLink && (link.m_org == newDst)) {
                if (this.m_canFireError) {
                    this.OnError(new ErrorEventArgs(ErrorEventType.ReflexiveError));
                }
                return false;
            }
            if (this.m_canMultiLink || !link.m_org.IsOriginOf(newDst)) {
                return true;
            }
            if (this.m_canFireError) {
                this.OnError(new ErrorEventArgs(ErrorEventType.MultilinkError));
            }
            return false;
        }
 
        private bool IsNewOrgAllowed(Link link, Node newOrg) {
            if (!newOrg.OutLinkable) {
                if (this.m_canFireError) {
                    this.OnError(new ErrorEventArgs(ErrorEventType.OutgoingLinkError));
                }
                return false;
            }
            if (!this.m_canReflexLink && (link.m_dst == newOrg)) {
                if (this.m_canFireError) {
                    this.OnError(new ErrorEventArgs(ErrorEventType.ReflexiveError));
                }
                return false;
            }
            if (this.m_canMultiLink || !link.m_dst.IsDestinationOf(newOrg)) {
                return true;
            }
            if (this.m_canFireError) {
                this.OnError(new ErrorEventArgs(ErrorEventType.MultilinkError));
            }
            return false;
        }
 
        private void items_ClearColl(object sender, EventArgs e) {
            this.DeleteNodes();
        }
 
        private void items_RemoveAtIndex(object sender, RemoveAtIndexEventArgs e) {
            Item item1 = this.m_items[e.Index];
            if (item1 is Link) {
                this.RemoveLink((Link) item1);
            }
            else {
                this.RemoveNode((Node) item1);
            }
        }
 
        internal void MouseDownHandler(MouseEventArgs mea) {
            if (mea.Button == MouseButtons.Left) {
                this.m_doubleClick = mea.Clicks == 2;
                if (this.m_doubleClick) {
                    this.m_timerEdit.Stop();
                }
                if (!this.Focused) {
                    base.Focus();
                }
                this.m_mouseDown = true;
                this.m_startMove = true;
                this.m_beginMove = true;
                PointF tf1 = new PointF((float) mea.X, (float) mea.Y);
                this.m_ptStart = tf1;
                Graphics graphics1 = base.CreateGraphics();
                this.CoordinatesDeviceToWorld(graphics1);
                tf1 = Misc.PointDeviceToWorld(graphics1, tf1);
                this.m_ptPrior = tf1;
                this.BeginSelWork(graphics1, tf1);
                graphics1.Dispose();
                if ((this.m_grid.m_snap && (this.m_interactiveAction != InteractiveAction.Select)) && (this.m_interactiveAction != InteractiveAction.Link)) {
                    this.m_ptPrior = this.m_grid.Adjust(this.m_ptPrior);
                }
                switch (this.m_interactiveAction) {
                case InteractiveAction.Node:
                case InteractiveAction.Select: {
                    this.m_ptOrg = this.m_ptPrior;
                    this.m_tmprc = new RectangleF(this.m_ptOrg.X, this.m_ptOrg.Y, 0f, 0f);
                    return;
                }
                case InteractiveAction.Link: {
                    if (!this.m_deflink.m_adjustOrg) {
                        this.m_ptPrior = Misc.CenterPoint(this.m_org.m_rc);
                    }
                    this.m_ptOrg = this.m_ptPrior;
                    return;
                }
                case InteractiveAction.Size:
                case InteractiveAction.Stretch: {
                    return;
                }
                }
                this.m_ptOrg = this.m_ptPrior;
            }
        }
 
        internal void MouseMoveHandler(MouseEventArgs mea) {
            PointF tf1 = new PointF((float) mea.X, (float) mea.Y);
            if (this.m_startMove) {
                if ((Math.Max((float) (tf1.X - this.m_ptStart.X), (float) (this.m_ptStart.X - tf1.X)) < this.m_moveStartDist.Width) && (Math.Max((float) (tf1.Y - this.m_ptStart.Y), (float) (this.m_ptStart.Y - tf1.Y)) < this.m_moveStartDist.Height)) {
                    return;
                }
                this.m_startMove = false;
            }
            RectangleF ef1 = new RectangleF((PointF) base.ClientRectangle.Location, (SizeF) base.ClientRectangle.Size);
            ef1.Inflate(base.HScroll ? ((float) -SystemInformation.HorizontalScrollBarHeight) : ((float) (-1)), base.VScroll ? ((float) -SystemInformation.VerticalScrollBarWidth) : ((float) (-1)));
            if (!ef1.Contains(tf1)) {
                if ((this.m_mouseDown && (this.m_interactiveAction != InteractiveAction.None)) && this.m_canDragScroll) {
                    this.m_ptOut = tf1;
                    this.m_xScrollDir = AddFlow.ScrollDir.None;
                    this.m_yScrollDir = AddFlow.ScrollDir.None;
                    if (tf1.X > ef1.Right) {
                        this.m_xScrollDir = AddFlow.ScrollDir.Right;
                    }
                    else if (tf1.X < ef1.Left) {
                        this.m_xScrollDir = AddFlow.ScrollDir.Left;
                    }
                    if (tf1.Y > ef1.Bottom) {
                        this.m_yScrollDir = AddFlow.ScrollDir.Bottom;
                    }
                    else if (tf1.Y < ef1.Top) {
                        this.m_yScrollDir = AddFlow.ScrollDir.Top;
                    }
                    if (((this.m_xScrollDir != AddFlow.ScrollDir.None) || (this.m_yScrollDir != AddFlow.ScrollDir.None)) && !this.m_timerScroll.Enabled) {
                        this.m_nTimer = 1;
                        this.m_timerScroll.Interval = this.m_aTimerDuration[this.m_nTimer - 1];
                        this.m_timerScroll.Start();
                    }
                }
                if (tf1.X > ef1.Right) {
                    tf1.X = ef1.Right;
                }
                else if (tf1.X < ef1.Left) {
                    tf1.X = ef1.Left;
                }
                if (tf1.Y > ef1.Bottom) {
                    tf1.Y = ef1.Bottom;
                }
                else if (tf1.Y < ef1.Top) {
                    tf1.Y = ef1.Top;
                }
                if (this.m_timerScroll.Enabled) {
                    return;
                }
            }
            if (this.m_timerScroll.Enabled) {
                this.m_timerScroll.Stop();
            }
            this.DrawMoveShape(tf1);
        }
 
        internal void MouseUpHandler(MouseEventArgs mea) {
            Graphics graphics1 = base.CreateGraphics();
            this.CoordinatesDeviceToWorld(graphics1);
            PointF tf1 = new PointF((float) mea.X, (float) mea.Y);
            tf1 = Misc.PointDeviceToWorld(graphics1, tf1);
            this.CheckArea(graphics1, tf1);
            graphics1.Dispose();
            if (this.m_mouseDown) {
                this.m_mouseDown = false;
                if (this.m_timerScroll.Enabled) {
                    this.m_timerScroll.Stop();
                }
                if (!this.m_startMove) {
                    switch (this.m_interactiveAction) {
                    case InteractiveAction.Node: {
                        this.EndNode();
                        break;
                    }
                    case InteractiveAction.Link: {
                        this.EndLink();
                        break;
                    }
                    case InteractiveAction.Drag: {
                        this.EndDrag();
                        break;
                    }
                    case InteractiveAction.Size: {
                        this.EndSize();
                        break;
                    }
                    case InteractiveAction.Stretch: {
                        this.EndStretch();
                        break;
                    }
                    case InteractiveAction.Select: {
                        this.EndSelect();
                        break;
                    }
                    }
                }
                this.EndSelWork();
                if ((this.m_canLabelEdit && (this.m_clickEvent == AddFlow.ClickZone.CurNode)) && (this.m_startMove && !this.m_doubleClick)) {
                    this.m_timerEdit.Start();
                }
            }
        }
 
        protected virtual void OnAfterAddLink(AfterAddLinkEventArgs e) {
            if (this.AfterAddLink != null) {
                this.AfterAddLink(this, e);
            }
        }
 
        protected virtual void OnAfterAddNode(AfterAddNodeEventArgs e) {
            if (this.AfterAddNode != null) {
                this.AfterAddNode(this, e);
            }
        }
 
        protected virtual void OnAfterEdit(AfterEditEventArgs e) {
            if (this.AfterEdit != null) {
                this.AfterEdit(this, e);
            }
        }
 
        protected virtual void OnAfterMove(EventArgs e) {
            if (this.AfterMove != null) {
                this.AfterMove(this, e);
            }
        }
 
        protected virtual void OnAfterResize(EventArgs e) {
            if (this.AfterResize != null) {
                this.AfterResize(this, e);
            }
        }
 
        protected virtual void OnAfterSelect(EventArgs e) {
            if (this.AfterSelect != null) {
                this.AfterSelect(this, e);
            }
        }
 
        protected virtual void OnAfterStretch(EventArgs e) {
            if (this.AfterStretch != null) {
                this.AfterStretch(this, e);
            }
        }
 
        protected virtual void OnBeforeAddLink(BeforeAddLinkEventArgs e) {
            if (this.BeforeAddLink != null) {
                this.BeforeAddLink(this, e);
            }
        }
 
        protected virtual void OnBeforeAddNode(BeforeAddNodeEventArgs e) {
            if (this.BeforeAddNode != null) {
                this.BeforeAddNode(this, e);
            }
        }
 
        protected virtual void OnBeforeChangeDst(BeforeChangeDstEventArgs e) {
            if (this.BeforeChangeDst != null) {
                this.BeforeChangeDst(this, e);
            }
        }
 
        protected virtual void OnBeforeChangeOrg(BeforeChangeOrgEventArgs e) {
            if (this.BeforeChangeOrg != null) {
                this.BeforeChangeOrg(this, e);
            }
        }
 
        protected virtual void OnBeforeEdit(BeforeEditEventArgs e) {
            if (this.BeforeEdit != null) {
                this.BeforeEdit(this, e);
            }
        }
 
        protected virtual void OnDesignModeChange(EventArgs e) {
            if (this.DesignModeChange != null) {
                this.DesignModeChange(this, e);
            }
        }
 
        protected virtual void OnDiagramOwnerDraw(DiagramOwnerDrawEventArgs e) {
            if (this.DiagramOwnerDraw != null) {
                this.DiagramOwnerDraw(this, e);
            }
        }
 
        protected virtual void OnError(ErrorEventArgs e) {
            if (this.Error != null) {
                this.Error(this, e);
            }
        }
 
        protected virtual void OnExtentChange(EventArgs e) {
            if (this.ExtentChange != null) {
                this.ExtentChange(this, e);
            }
        }
 
        protected virtual void OnLinkOwnerDraw(LinkOwnerDrawEventArgs e) {
            if (this.LinkOwnerDraw != null) {
                this.LinkOwnerDraw(this, e);
            }
        }
 
        protected override void OnMouseDown(MouseEventArgs e) {
            this.MouseDownHandler(e);
            base.OnMouseDown(e);
        }
 
        protected override void OnMouseMove(MouseEventArgs e) {
            this.MouseMoveHandler(e);
            base.OnMouseMove(e);
        }
 
        protected override void OnMouseUp(MouseEventArgs e) {
            this.MouseUpHandler(e);
            base.OnMouseUp(e);
        }
 
        protected virtual void OnNodeOwnerDraw(NodeOwnerDrawEventArgs e) {
            if (this.NodeOwnerDraw != null) {
                this.NodeOwnerDraw(this, e);
            }
        }
 
        protected override void OnPaint(PaintEventArgs e) {
            this.PaintHandler(e);
            base.OnPaint(e);
        }
 
        protected override void OnResize(EventArgs e) {
            this.ResizeHandler(e);
            base.OnResize(e);
        }
 
        protected virtual void OnScroll(EventArgs e) {
            if (this.Scroll != null) {
                this.Scroll(this, e);
            }
        }
 
        internal void PaintHandler(PaintEventArgs pea) {
            Graphics graphics1 = pea.Graphics;
            this.CoordinatesDeviceToWorld(graphics1);
            this.m_rcinv = Misc.RectDeviceToWorld(graphics1, pea.ClipRectangle);
            if (this.m_grid.Draw) {
                this.m_grid.Display(graphics1, this.m_rcinv);
            }
            this.internalRender(graphics1, this.m_rcinv);
            if (!this.m_inPlaceEdition) {
                this.DisplaySelection(graphics1);
            }
            if (this.m_mouseDown) {
                this.DrawOutLine(graphics1);
            }
        }
 
        public PointF PointFToAddFlow(PointF pt) {
            Graphics graphics1 = base.CreateGraphics();
            this.CoordinatesDeviceToWorld(graphics1);
            PointF tf1 = Misc.PointDeviceToWorld(graphics1, pt);
            graphics1.Dispose();
            return tf1;
        }
 
        public PointF PointFToDevice(PointF pt) {
            Graphics graphics1 = base.CreateGraphics();
            this.CoordinatesDeviceToWorld(graphics1);
            PointF tf1 = Misc.PointWorldToDevice(graphics1, pt);
            graphics1.Dispose();
            return tf1;
        }
 
        public Point PointToAddFlow(Point pt) {
            Graphics graphics1 = base.CreateGraphics();
            this.CoordinatesDeviceToWorld(graphics1);
            PointF tf1 = Misc.PointDeviceToWorld(graphics1, new PointF((float) pt.X, (float) pt.Y));
            graphics1.Dispose();
            return new Point((int) tf1.X, (int) tf1.Y);
        }
 
        public Point PointToDevice(Point pt) {
            Graphics graphics1 = base.CreateGraphics();
            this.CoordinatesDeviceToWorld(graphics1);
            PointF tf1 = Misc.PointWorldToDevice(graphics1, new PointF((float) pt.X, (float) pt.Y));
            graphics1.Dispose();
            return new Point((int) tf1.X, (int) tf1.Y);
        }
 
        public void Redo() {
            this.m_undo.Redo();
        }
 
        internal void RemoveLink(Link link) {
            link.InvalidateItem();
            if (link.m_selected) {
                link.Selected = false;
            }
            if (this.m_undo.CanUndoRedo && !this.m_undo.SkipUndo) {
                this.m_undo.SubmitTask(new DeleteLinkTask(link));
            }
            this.RemoveLinkFromGraph(link);
            if (link == this.m_pointedItem) {
                Graphics graphics1 = base.CreateGraphics();
                this.CoordinatesDeviceToWorld(graphics1);
                this.CheckArea(graphics1, this.m_checkedAreaPoint);
                graphics1.Dispose();
            }
            if (link == this.m_stretchedLink) {
                this.m_stretchedLink = null;
            }
            this.IncrementChangedFlag(1);
        }
 
        internal void RemoveLinkFromGraph(Link link) {
            link.m_org.m_aLinks.Remove(link);
            link.m_dst.m_aLinks.Remove(link);
            this.m_items.Remove(link);
            link.m_af = null;
        }
 
        internal void RemoveNode(Node node) {
            if (node.m_isEditing) {
                this.StopEdition(node, false);
            }
            node.InvalidateItem();
            if (node.m_selected) {
                node.Selected = false;
            }
            int num1 = (this.m_undo.CanUndoRedo && !this.m_undo.SkipUndo) ? node.GetZOrder() : 0;
            for (int num3 = node.m_aLinks.Count - 1; num3 >= 0; num3--) {
                Link link1 = (Link) node.m_aLinks[num3];
                if (link1 != null) {
                    if (link1.Reflexive && !link1.m_flag) {
                        link1.m_flag = true;
                    }
                    else {
                        this.RemoveLink(link1);
                    }
                }
            }
            if (this.m_undo.CanUndoRedo && !this.m_undo.SkipUndo) {
                this.m_undo.SubmitTask(new DeleteNodeTask(node, num1));
            }
            this.RemoveNodeFromGraph(node);
            if (node == this.m_pointedItem) {
                Graphics graphics1 = base.CreateGraphics();
                this.CoordinatesDeviceToWorld(graphics1);
                this.CheckArea(graphics1, this.m_checkedAreaPoint);
                graphics1.Dispose();
            }
            if (node == this.m_resizedNode) {
                this.m_resizedNode = null;
            }
            this.IncrementChangedFlag(1);
        }
 
        internal void RemoveNodeFromGraph(Node node) {
            this.m_aNodes.Remove(node);
            this.m_items.Remove(node);
            node.m_af = null;
        }
 
        private void Reset() {
            base.SetStyle(ControlStyles.DoubleBuffer | (ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint), true);
            base.ResizeRedraw = true;
            this.m_connect = new Connect(this);
            this.m_undo = new Undoman(this);
            this.m_zoom = new Zoom();
            this.m_zoom.Change += new Zoom.ChangeEventHandler(this.composedProperty_Change);
            this.m_grid = new Grid();
            this.m_grid.Change += new Grid.ChangeEventHandler(this.composedProperty_Change);
            this.m_defnode = new DefNode();
            this.m_defnode.Change += new DefNode.ChangeEventHandler(this.composedProperty_Change);
            this.m_deflink = new DefLink();
            this.m_deflink.Change += new DefLink.ChangeEventHandler(this.composedProperty_Change);
            this.m_items = new ItemsCollection();
            this.m_items.RemoveAtIndex += new ItemsCollection.RemoveAtIndexEventHandler(this.items_RemoveAtIndex);
            this.m_items.ClearColl += new ItemsCollection.ClearCollEventHandler(this.items_ClearColl);
            this.m_selItems = new ItemsCollection();
            this.m_selItems.RemoveAtIndex += new ItemsCollection.RemoveAtIndexEventHandler(this.selectedItems_RemoveAtIndex);
            this.m_selItems.ClearColl += new ItemsCollection.ClearCollEventHandler(this.selectedItems_ClearColl);
            this.m_internalUnselect = false;
            this.m_aNodes = new ArrayList();
            this.m_nodes = new NodesCollection(this, this.m_aNodes);
            this.m_aDragNodes = new ArrayList();
            this.m_images = new ImagesCollection();
            this.m_oldScrollPos = new Point(0, 0);
            this.m_scrollPos = new Point(0, 0);
            this.m_xScrollDir = AddFlow.ScrollDir.None;
            this.m_yScrollDir = AddFlow.ScrollDir.None;
            this.m_scrollUnit = new Size(SystemInformation.IconSize.Width / 2, SystemInformation.IconSize.Height / 2);
            this.m_moveStartDist = new SizeF(5f, 5f);
            this.m_outlineSize = new SizeF(2f, 2f);
            this.m_linkWidth = SystemInformation.IconSize.Width / 3;
            this.m_linkWidthWorld = 0f;
            this.m_jumpSize = SystemInformation.IconSize.Width / 6;
            this.m_roundSize = SystemInformation.IconSize.Width / 6;
            this.m_nTimer = 0;
            int[] numArray1 = new int[2] { 300, 50 } ;
            this.m_aTimerDuration = numArray1;
            this.m_timerScroll = new Timer();
            this.m_timerScroll.Tick += new EventHandler(this.TimerScrollOnTick);
            this.m_timerEdit = new Timer();
            this.m_timerEdit.Tick += new EventHandler(this.TimerEditOnTick);
            this.m_timerEdit.Interval = SystemInformation.DoubleClickTime + 10;
            this.m_tooltipCtrl = new ToolTip();
            this.m_interactiveAction = InteractiveAction.None;
            this.m_handle = 0;
            this.m_stretchingPoint = -1;
            this.m_doubleClick = false;
            this.m_mouseDown = false;
            this.m_startMove = false;
            this.m_beginMove = false;
            this.m_out = false;
            this.m_rc = new RectangleF();
            this.m_tmprc = new RectangleF();
            this.m_netrc = new RectangleF();
            this.m_selrc = new RectangleF();
            this.m_rcinv = new Rectangle();
            this.m_org = null;
            this.m_pointedItem = null;
            this.m_stretchedLink = null;
            this.m_resizedNode = null;
            this.m_changed = 0;
            this.m_inPlaceEdition = false;
            this.m_drawAll = false;
            this.m_antiAliasing = true;
            this.m_pageUnit = GraphicsUnit.Point;
            this.m_pageScale = 1f;
            this.m_ownerDraw = false;
            this.m_repaint = 0;
            this.m_interHandles = true;
            this.m_selHandleSize = HandleSize.Small;
            this.m_linkHandleSize = HandleSize.Small;
            this.m_linkCreationMode = LinkCreationMode.AllNodeArea;
            this.m_cursorSetting = CursorSetting.Resize;
            this.m_removePointAngle = RemovePointAngle.Medium;
            this.m_pointedArea = MouseArea.OutSide;
            this.m_mouseAction = MouseAction.None;
            this.m_stretchType = AddFlow.Stretch.None;
            this.m_scrollbarsDisplayMode = ScrollbarsDisplayMode.AddControlSize;
            this.m_multiSel = true;
            this.m_canDrawNode = true;
            this.m_canMoveNode = true;
            this.m_canSizeNode = true;
            this.m_canDrawLink = true;
            this.m_canStretchLink = true;
            this.m_canMultiLink = true;
            this.m_canReflexLink = true;
            this.m_canDragScroll = true;
            this.m_canChangeOrg = true;
            this.m_canChangeDst = true;
            this.m_canFireError = false;
            this.m_canLabelEdit = true;
            this.m_showTooltips = true;
            this.m_cycleMode = CycleMode.CycleAllowed;
            this.ConvertSomeSizesInWorldCoordinates();
            this.ChangeSelectionHandleSize();
            this.ChangeLinkHandleSize();
        }
 
        public void ResetDefLinkProp() {
            this.m_deflink.Reset();
        }
 
        public void ResetDefNodeProp() {
            this.m_defnode.Reset();
        }
 
        public void ResetGrid() {
            this.m_grid.Reset();
            base.Invalidate();
        }
 
        public void ResetUndoRedo() {
            this.m_undo.Clear();
        }
 
        public void ResetZoom() {
            this.m_zoom.Reset();
            base.Invalidate();
        }
 
        internal void ResizeHandler(EventArgs ea) {
            if (this.AutoScroll && (this.m_repaint == 0)) {
                this.UpdateScrollInfo();
            }
            if (this.m_inPlaceEdition) {
                this.StopEdition(this.m_textBox.Node, true);
            }
        }
 
        private void selectedItems_ClearColl(object sender, EventArgs e) {
            if (!this.m_internalUnselect && (this.m_selItems.Count > 0)) {
                foreach (Item item1 in this.m_selItems) {
                    item1.InvalidateHandles();
                    item1.m_selected = false;
                }
                this.SetSelChangedFlag(true);
            }
        }
 
        private void selectedItems_RemoveAtIndex(object sender, RemoveAtIndexEventArgs e) {
            Item item1 = this.m_selItems[e.Index];
            item1.Selected = false;
        }
 
        private void SelectRectangle(RectangleF selrc, bool partial) {
            ArrayList list1 = this.GetItemsInRect(selrc, partial);
            foreach (Item item1 in list1) {
                item1.Selected = true;
            }
        }
 
        public void SetChangedFlag(bool changed) {
            if (!changed) {
                this.IncrementChangedFlag(-this.m_changed);
            }
            else {
                this.IncrementChangedFlag(1);
            }
        }
 
        internal void SetCursor(MouseArea area) {
            switch (area) {
            case MouseArea.LeftUpSquare: {
                this.Cursor = Cursors.SizeNWSE;
                return;
            }
            case MouseArea.UpSquare: {
                this.Cursor = Cursors.SizeNS;
                return;
            }
            case MouseArea.RightUpSquare: {
                this.Cursor = Cursors.SizeNESW;
                return;
            }
            case MouseArea.LeftSquare: {
                this.Cursor = Cursors.SizeWE;
                return;
            }
            case MouseArea.RightSquare: {
                this.Cursor = Cursors.SizeWE;
                return;
            }
            case MouseArea.LeftDownSquare: {
                this.Cursor = Cursors.SizeNESW;
                return;
            }
            case MouseArea.DownSquare: {
                this.Cursor = Cursors.SizeNS;
                return;
            }
            case MouseArea.RightDownSquare: {
                this.Cursor = Cursors.SizeNWSE;
                return;
            }
            case MouseArea.LinkSquare: {
                if (this.m_cursorSetting != CursorSetting.All) {
                    this.Cursor = Cursors.Arrow;
                    return;
                }
                this.Cursor = Cursors.UpArrow;
                return;
            }
            case MouseArea.NodeDragFrame: {
                if ((this.m_cursorSetting != CursorSetting.ResizeAndDrag) && (this.m_cursorSetting != CursorSetting.All)) {
                    this.Cursor = Cursors.Arrow;
                    return;
                }
                this.Cursor = Cursors.SizeAll;
                return;
            }
            case MouseArea.Node: {
                if (this.m_linkCreationMode != LinkCreationMode.AllNodeArea) {
                    if ((this.m_cursorSetting == CursorSetting.ResizeAndDrag) || (this.m_cursorSetting == CursorSetting.All)) {
                        this.Cursor = Cursors.SizeAll;
                        return;
                    }
                    this.Cursor = Cursors.Arrow;
                    return;
                }
                if (this.m_cursorSetting != CursorSetting.All) {
                    this.Cursor = Cursors.Arrow;
                    return;
                }
                this.Cursor = Cursors.UpArrow;
                return;
            }
            }
            this.Cursor = Cursors.Arrow;
        }
 
        public void SetSelChangedFlag(bool selChanged) {
            this.m_selChanged = selChanged;
        }
 
        public bool ShouldSerializeDefLinkProp() {
            return !this.m_deflink.IsDefault();
        }
 
        public bool ShouldSerializeDefNodeProp() {
            return !this.m_defnode.IsDefault();
        }
 
        public bool ShouldSerializeGrid() {
            return !this.m_grid.IsDefault();
        }
 
        public bool ShouldSerializeZoom() {
            return !this.m_zoom.IsDefault();
        }
 
        internal void StartEdition(Node node) {
            BeforeEditEventArgs args1 = new BeforeEditEventArgs(node);
            this.OnBeforeEdit(args1);
            if (!args1.Cancel.Cancel && ((node != null) && node.Existing)) {
                this.m_inPlaceEdition = true;
                node.InvalidateHandles();
                this.m_textBox = new InPlaceTextBox(node);
                node.m_isEditing = true;
            }
        }
 
        internal void StopEdition(Node node, bool valid) {
            if (((node != null) && node.Existing) && node.m_isEditing) {
                node.m_isEditing = false;
                if (this.m_textBox != null) {
                    this.m_inPlaceEdition = false;
                    string text1 = this.m_textBox.Text;
                    this.m_textBox.Dispose();
                    this.m_textBox = null;
                    if (valid) {
                        AfterEditEventArgs args1 = new AfterEditEventArgs(text1, node);
                        this.OnAfterEdit(args1);
                        if (!args1.Cancel.Cancel && (node.m_text != args1.Text)) {
                            if (this.m_undo.CanUndoRedo && !this.m_undo.SkipUndo) {
                                this.m_undo.SubmitTask(new NodeTextTask(node));
                            }
                            node.SetText2(args1.Text);
                            this.IncrementChangedFlag(1);
                        }
                    }
                    node.InvalidateHandles();
                    node.InvalidateItem();
                }
            }
        }
 
        private void StretchDyn(PointF pt, Link link) {
            if (this.m_beginMove) {
                this.m_beginMove = false;
                link.m_aptf2 = new PointF[link.m_aptf.Length];
                link.m_aptf.CopyTo(link.m_aptf2, 0);
            }
            if ((this.m_handle > 1) && (this.m_handle < (link.m_aptf.Length - 2))) {
                link.UpdateVHLink(pt, this.m_handle);
            }
            else {
                link.UpdateDynaLink(pt, this.m_handle);
            }
        }
 
        private void StretchPoly(PointF pt, Link link) {
            if (this.m_beginMove) {
                this.m_beginMove = false;
                link.m_aptf2 = link.m_aptf;
                if (this.m_stretchType == AddFlow.Stretch.Add) {
                    PointF tf1 = Misc.MiddlePoint(link.m_aptf[this.m_handle], link.m_aptf[this.m_handle + 1]);
                    link.m_aptf = link.InsertPoint(this.m_handle, tf1);
                    this.m_handle++;
                }
            }
            if ((this.m_handle >= 0) && (this.m_handle <= (link.m_aptf.Length - 1))) {
                link.m_aptf[this.m_handle] = pt;
            }
            if (!link.m_adjustOrg && (this.m_stretchType != AddFlow.Stretch.First)) {
                link.CalcLinkTips(true, false);
            }
            if (!link.m_adjustDst && (this.m_stretchType != AddFlow.Stretch.Last)) {
                link.CalcLinkTips(false, true);
            }
        }
 
        private void StretchVH(PointF pt, Link link) {
            if (this.m_beginMove) {
                this.m_beginMove = false;
                link.m_aptf2 = new PointF[link.m_aptf.Length];
                link.m_aptf.CopyTo(link.m_aptf2, 0);
            }
            link.UpdateVHLink(pt, this.m_handle);
        }
 
        private void TimerEditOnTick(object obj, EventArgs ea) {
            this.m_timerEdit.Stop();
            Node node1 = (Node) this.SelectedItem;
            if (((node1 != null) && node1.Existing) && node1.m_labelEdit) {
                this.StartEdition(node1);
            }
        }
 
        private void TimerScrollOnTick(object obj, EventArgs ea) {
            this.DragScroll();
        }
 
        public void Undo() {
            this.m_undo.Undo();
        }
 
        internal void UnSelect() {
            if (this.m_selItems.Count > 0) {
                foreach (Item item1 in this.m_selItems) {
                    item1.InvalidateHandles();
                    item1.m_selected = false;
                }
                this.m_internalUnselect = true;
                this.m_selItems.Clear();
                this.m_internalUnselect = false;
                this.SetSelChangedFlag(true);
            }
        }
 
        private RectangleF UpdateAllRect() {
            this.UpdateRect();
            return new RectangleF(this.GraphRect.Location, this.GraphRect.Size);
        }
 
        internal void UpdateRect() {
            RectangleF ef1 = new RectangleF();
            foreach (Item item1 in this.m_items) {
                if (item1 is Node) {
                    Node node1 = (Node) item1;
                    ef1 = RectangleF.Union(ef1, node1.GetRectShadow());
                    continue;
                }
                Link link1 = (Link) item1;
                ef1 = RectangleF.Union(ef1, link1.m_rcSeg);
            }
            if (!ef1.Equals(this.GraphRect)) {
                this.GraphRect = ef1;
                if (this.m_repaint == 0) {
                    if (this.AutoScroll) {
                        this.UpdateScrollInfo();
                    }
                    this.OnExtentChange(EventArgs.Empty);
                }
            }
        }
 
        internal void UpdateRect2(RectangleF rc) {
            RectangleF ef1 = RectangleF.Union(this.GraphRect, rc);
            if (!ef1.Equals(this.GraphRect)) {
                this.GraphRect = ef1;
                if (this.m_repaint == 0) {
                    if (this.AutoScroll) {
                        this.UpdateScrollInfo();
                    }
                    this.OnExtentChange(EventArgs.Empty);
                }
            }
        }
 
        internal void UpdateScrollInfo() {
            SizeF ef1 = this.Extent;
            Graphics graphics1 = base.CreateGraphics();
            this.CoordinatesDeviceToWorld(graphics1);
            if (this.m_scrollbarsDisplayMode == ScrollbarsDisplayMode.AddControlSize) {
                SizeF ef2 = Misc.SizeDeviceToWorld(graphics1, (SizeF) base.ClientSize);
                ef1.Width += ef2.Width;
                ef1.Height += ef2.Height;
            }
            ef1 = Misc.SizeWorldToDevice(graphics1, ef1);
            graphics1.Dispose();
            base.AutoScrollMinSize = Size.Round(ef1);
        }
 
        private RectangleF UpdateSelRect() {
            RectangleF ef1 = RectangleF.Empty;
            foreach (Item item1 in this.m_selItems) {
                if (!item1.m_selected) {
                    continue;
                }
                if (item1 is Node) {
                    Node node1 = (Node) item1;
                    RectangleF ef2 = node1.GetRectShadow();
                    ef1 = RectangleF.Union(ef1, ef2);
                    continue;
                }
                Link link1 = (Link) item1;
                ef1 = RectangleF.Union(ef1, link1.m_rcSeg);
            }
            return ef1;
        }
 
        public void ZoomRectangle(RectangleF zoomRectangle, ZoomType zoomType) {
            if (!zoomRectangle.IsEmpty) {
                Rectangle rectangle1 = this.GetClientRectangleInWorldCoordinates();
                if (zoomType == ZoomType.Isotropic) {
                    if (zoomRectangle.Width > zoomRectangle.Height) {
                        zoomRectangle.Height = zoomRectangle.Width;
                    }
                    else {
                        zoomRectangle.Width = zoomRectangle.Height;
                    }
                    if (rectangle1.Width < rectangle1.Height) {
                        rectangle1.Height = rectangle1.Width;
                    }
                    else {
                        rectangle1.Width = rectangle1.Height;
                    }
                }
                this.Zoom = new Zoom((this.m_zoom.X * rectangle1.Width) / zoomRectangle.Width, (this.m_zoom.Y * rectangle1.Height) / zoomRectangle.Height);
                PointF tf1 = zoomRectangle.Location;
                RectangleF ef1 = RectangleF.FromLTRB(0f, 0f, tf1.X, tf1.Y);
                this.ScrollPosition = Point.Empty;
                Graphics graphics1 = base.CreateGraphics();
                this.CoordinatesDeviceToWorld(graphics1);
                ef1 = Misc.RectWorldToDevice(graphics1, ef1);
                graphics1.Dispose();
                tf1 = new PointF(ef1.Right, ef1.Bottom);
                this.ScrollPosition = Point.Round(tf1);
            }
        }
 
        [Category("AddFlow Interactivity"), DefaultValue(true), Description("Determines whether anti-aliasing graphics rendering technique is used to display the graph.")]
        public bool AntiAliasing {
            get {
                return this.m_antiAliasing;
            }
            set {
                if (value != this.m_antiAliasing) {
                    this.m_antiAliasing = value;
                    if (this.m_repaint == 0) {
                        base.Invalidate();
                    }
                }
            }
        }
 
        [Description("Determines whether the user can interactively change the destination of a link"), DefaultValue(true), Category("AddFlow Capabilities")]
        public bool CanChangeDst {
            get {
                return this.m_canChangeDst;
            }
            set {
                this.m_canChangeDst = value;
            }
        }
 
        [Category("AddFlow Capabilities"), Description("Determines whether the user can interactively change the origin of a link."), DefaultValue(true)]
        public bool CanChangeOrg {
            get {
                return this.m_canChangeOrg;
            }
            set {
                this.m_canChangeOrg = value;
            }
        }
 
        [Category("AddFlow Capabilities"), Description("Determines whether scrolling is started when the user drags the mouse outside the client area."), DefaultValue(true)]
        public bool CanDragScroll {
            get {
                return this.m_canDragScroll;
            }
            set {
                this.m_canDragScroll = value;
            }
        }
 
        [Description("Determines whether interactive creation of links is allowed or not."), DefaultValue(true), Category("AddFlow Capabilities")]
        public bool CanDrawLink {
            get {
                return this.m_canDrawLink;
            }
            set {
                this.m_canDrawLink = value;
            }
        }
 
        [Category("AddFlow Capabilities"), Description("Determines whether interactive creation of nodes is allowed or not."), DefaultValue(true)]
        public bool CanDrawNode {
            get {
                return this.m_canDrawNode;
            }
            set {
                this.m_canDrawNode = value;
            }
        }
 
        [DefaultValue(false), Category("AddFlow Capabilities"), Description("Determines whether the Error event is fired or not.")]
        public bool CanFireError {
            get {
                return this.m_canFireError;
            }
            set {
                this.m_canFireError = value;
            }
        }
 
        [DefaultValue(true), Category("AddFlow Capabilities"), Description("Determines whether the user can edit the text of nodes.")]
        public bool CanLabelEdit {
            get {
                return this.m_canLabelEdit;
            }
            set {
                this.m_canLabelEdit = value;
            }
        }
 
        [Category("AddFlow Capabilities"), Description("Determines whether interactive dragging of nodes is allowed or not."), DefaultValue(true)]
        public bool CanMoveNode {
            get {
                return this.m_canMoveNode;
            }
            set {
                this.m_canMoveNode = value;
            }
        }
 
        [Category("AddFlow Capabilities"), DefaultValue(true), Description("Determines whether you can define several links between two nodes.")]
        public bool CanMultiLink {
            get {
                return this.m_canMultiLink;
            }
            set {
                this.m_canMultiLink = value;
            }
        }
 
        [Description("Indicates if there is an action that can be redone."), Browsable(false)]
        public bool CanRedo {
            get {
                return this.m_undo.CanRedo();
            }
        }
 
        [Description("Determines whether interactive creation of reflexive links is allowed or not."), DefaultValue(true), Category("AddFlow Capabilities")]
        public bool CanReflexLink {
            get {
                return this.m_canReflexLink;
            }
            set {
                this.m_canReflexLink = value;
            }
        }
 
        [Description("Determines whether interactive resizing of nodes is allowed or not."), DefaultValue(true), Category("AddFlow Capabilities")]
        public bool CanSizeNode {
            get {
                return this.m_canSizeNode;
            }
            set {
                this.m_canSizeNode = value;
            }
        }
 
        [Category("AddFlow Capabilities"), DefaultValue(true), Description("Determines whether interactive stretching of links is allowed or not.")]
        public bool CanStretchLink {
            get {
                return this.m_canStretchLink;
            }
            set {
                this.m_canStretchLink = value;
            }
        }
 
        [Description("Indicates if there is an action that can be undone."), Browsable(false)]
        public bool CanUndo {
            get {
                return this.m_undo.CanUndo();
            }
        }
 
        [Description("Determines whether undo/redo is allowed."), DefaultValue(true), Category("AddFlow Capabilities")]
        public bool CanUndoRedo {
            get {
                return this.m_undo.CanUndoRedo;
            }
            set {
                this.m_undo.CanUndoRedo = value;
            }
        }
 
        [DefaultValue(1), Description("Returns/sets a value that determines whether default cursors are displayed."), Category("AddFlow Interactivity")]
        public CursorSetting CursorSetting {
            get {
                return this.m_cursorSetting;
            }
            set {
                this.m_cursorSetting = value;
            }
        }
 
        [DefaultValue(0), Description("Returns/sets a value that determines whether cycles are accepted or not."), Category("AddFlow Capabilities")]
        public CycleMode CycleMode {
            get {
                return this.m_cycleMode;
            }
            set {
                this.m_cycleMode = value;
                if ((this.m_cycleMode == CycleMode.NoDirectedCycle) && !this.m_connect.IsDirectedAcyclicGraph()) {
                    this.OnError(new ErrorEventArgs(ErrorEventType.DirectedCycleError));
                }
                if ((this.m_cycleMode == CycleMode.NoCycle) && !this.m_connect.IsAcyclicGraph()) {
                    this.OnError(new ErrorEventArgs(ErrorEventType.CycleError));
                }
            }
        }
 
        [Category("AddFlow items"), Description("Default properties for links"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DefLink DefLinkProp {
            get {
                return this.m_deflink;
            }
            set {
                this.m_deflink = value;
            }
        }
 
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("AddFlow items"), Description("Default properties for nodes")]
        public DefNode DefNodeProp {
            get {
                return this.m_defnode;
            }
            set {
                this.m_defnode = value;
            }
        }
 
        [Category("AddFlow Interactivity"), DefaultValue(true), Description("Determines whether the handles used for selection are displayed or not.")]
        public bool DisplayHandles {
            get {
                return this.m_interHandles;
            }
            set {
                this.m_interHandles = value;
            }
        }
 
        [Description("Returns the diagram size."), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SizeF Extent {
            get {
                return new SizeF(this.GraphRect.Width, this.GraphRect.Height);
            }
        }
 
        internal RectangleF GraphRect {
            get {
                return this.m_netrc;
            }
            set {
                this.m_netrc = value;
            }
        }
 
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Category("AddFlow Interactivity"), Description("Grid management.")]
        public Grid Grid {
            get {
                return this.m_grid;
            }
            set {
                if (!this.m_grid.Equals(value)) {
                    this.m_grid = value;
                    base.Invalidate();
                }
            }
        }
 
        [Browsable(false), Description("Returns the collection of images that can be used to display an image in a node"), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ImagesCollection Images {
            get {
                return this.m_images;
            }
            set {
                this.m_images = value;
            }
        }
 
        [Description("Returns the interactive user action."), DefaultValue(0), Browsable(false)]
        public InteractiveAction InteractiveAction {
            get {
                return this.m_interactiveAction;
            }
        }
 
        [Browsable(false), Description("Indicates if the AddFlow diagram has changed.")]
        public bool IsChanged {
            get {
                return (this.m_changed != 0);
            }
        }
 
        [Browsable(false), Description("Indicates if the selection in the AddFlow diagram has changed.")]
        public bool IsSelChanged {
            get {
                return this.m_selChanged;
            }
        }
 
        [Browsable(false), Description("Returns the collection of all the items of the diagram.")]
        public ItemsCollection Items {
            get {
                return this.m_items;
            }
        }
 
        public static string LicenseKey {
            set {
                AddFlow.m_licenseKey = value;
            }
        }
 
        [DefaultValue(0), Category("AddFlow Interactivity"), Description("Returns/sets the link creation mode.")]
        public LinkCreationMode LinkCreationMode {
            get {
                return this.m_linkCreationMode;
            }
            set {
                this.m_linkCreationMode = value;
                foreach (Item item1 in this.m_selItems) {
                    if (item1 is Node) {
                        item1.InvalidateHandles();
                    }
                }
            }
        }
 
        [DefaultValue(0), Category("AddFlow Interactivity"), Description("Returns/sets the size of the linking handle.")]
        public HandleSize LinkHandleSize {
            get {
                return this.m_linkHandleSize;
            }
            set {
                this.m_linkHandleSize = value;
                this.ChangeLinkHandleSize();
            }
        }
 
        [DefaultValue(1), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Returns/sets the selection rectangle action (select items, zoom,...)."), Browsable(false)]
        public MouseAction MouseAction {
            get {
                return this.m_mouseAction;
            }
            set {
                this.m_mouseAction = value;
            }
        }
 
        [Category("AddFlow Capabilities"), Description("Determines whether multiselection is allowed or not."), DefaultValue(true)]
        public bool MultiSel {
            get {
                return this.m_multiSel;
            }
            set {
                this.m_multiSel = value;
            }
        }
 
        [Description("Returns the collection of all the nodes of the diagram."), Browsable(false)]
        public NodesCollection Nodes {
            get {
                return this.m_nodes;
            }
        }
 
        [Description("Determines whether repainting customization is allowed or not."), DefaultValue(false), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool OwnerDraw {
            get {
                return this.m_ownerDraw;
            }
            set {
                this.m_ownerDraw = value;
            }
        }
 
        [Description("Returns/sets a value that determines the scaling used to display the graph."), Category("AddFlow Interactivity"), DefaultValue((float) 1f)]
        public float PageScale {
            get {
                return this.m_pageScale;
            }
            set {
                if (value != this.m_pageScale) {
                    this.m_pageScale = value;
                    this.ConvertSomeSizesInWorldCoordinates();
                    if (this.m_repaint == 0) {
                        if (this.AutoScroll) {
                            this.UpdateScrollInfo();
                        }
                        base.Invalidate();
                    }
                }
            }
        }
 
        [Category("AddFlow Interactivity"), Description("Returns/sets a value that determines the unit of measure used to display the graph."), DefaultValue(3)]
        public GraphicsUnit PageUnit {
            get {
                return this.m_pageUnit;
            }
            set {
                if ((value != this.m_pageUnit) && (value != GraphicsUnit.World)) {
                    this.m_pageUnit = value;
                    this.ConvertSomeSizesInWorldCoordinates();
                    if (this.m_repaint == 0) {
                        if (this.AutoScroll) {
                            this.UpdateScrollInfo();
                        }
                        base.Invalidate();
                    }
                }
            }
        }
 
        [Browsable(false), Description("Returns the area pointed by the mouse.")]
        public MouseArea PointedArea {
            get {
                return this.m_pointedArea;
            }
        }
 
        [Description("Returns the item (node or link) pointed by the mouse."), Browsable(false)]
        public Item PointedItem {
            get {
                if ((this.m_pointedItem != null) && this.m_pointedItem.Existing) {
                    return this.m_pointedItem;
                }
                return null;
            }
        }
 
        [Browsable(false), Description("Returns the code of the next redoable action.")]
        public Action RedoCode {
            get {
                return this.m_undo.RedoCode;
            }
        }
 
        [Description("Returns/sets a value that determines if the user can remove a link point by dragging the handle to a position where it has a very obtuse angle to its surrounding link points."), DefaultValue(2), Category("AddFlow Interactivity")]
        public RemovePointAngle RemovePointAngle {
            get {
                return this.m_removePointAngle;
            }
            set {
                this.m_removePointAngle = value;
            }
        }
 
        [Category("AddFlow Interactivity"), Description("Returns/sets the scrollbars display mode"), DefaultValue(0)]
        public ScrollbarsDisplayMode ScrollbarsDisplayMode {
            get {
                return this.m_scrollbarsDisplayMode;
            }
            set {
                if (this.m_scrollbarsDisplayMode != value) {
                    this.m_scrollbarsDisplayMode = value;
                    if (this.AutoScroll) {
                        this.UpdateScrollInfo();
                    }
                }
            }
        }
 
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Returns/sets the scrolling offset.")]
        public Point ScrollPosition {
            get {
                if (this.AutoScroll) {
                    Point point1 = base.AutoScrollPosition;
                    point1.X = -point1.X;
                    point1.Y = -point1.Y;
                    return point1;
                }
                return this.m_scrollPos;
            }
            set {
                this.m_oldScrollPos = this.m_scrollPos;
                if (this.AutoScroll) {
                    base.AutoScrollPosition = value;
                    this.m_scrollPos = base.AutoScrollPosition;
                }
                else {
                    this.m_scrollPos = value;
                    if (this.m_scrollPos != this.m_oldScrollPos) {
                        base.Invalidate();
                    }
                }
            }
        }
 
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DefaultValue((string) null), Browsable(false), Description("Returns/sets the selected item (node or link).")]
        public Item SelectedItem {
            get {
                if (this.m_selItems.Count != 0) {
                    return this.m_selItems[0];
                }
                return null;
            }
            set {
                if (value != this.SelectedItem) {
                    this.UnSelect();
                    if (value != null) {
                        value.Selected = true;
                    }
                }
            }
        }
 
        [Description("Returns the collection of all the selected items of the diagram."), Browsable(false)]
        public ItemsCollection SelectedItems {
            get {
                return this.m_selItems;
            }
        }
 
        [Description("Returns/sets the size of the selection handles."), Category("AddFlow Interactivity"), DefaultValue(0)]
        public HandleSize SelectionHandleSize {
            get {
                return this.m_selHandleSize;
            }
            set {
                this.m_selHandleSize = value;
                this.ChangeSelectionHandleSize();
            }
        }
 
        [Description("Determines whether the tooltips are displayed or not."), Category("AddFlow Capabilities"), DefaultValue(true)]
        public bool ShowTooltips {
            get {
                return this.m_showTooltips;
            }
            set {
                if (this.m_showTooltips != value) {
                    this.m_showTooltips = value;
                    if (this.m_showTooltips) {
                        this.m_tooltipCtrl = new ToolTip();
                    }
                    else {
                        this.m_tooltipCtrl.Active = false;
                        this.m_tooltipCtrl.Dispose();
                        this.m_tooltipCtrl = null;
                    }
                }
            }
        }
 
        [Description("Determines whether the following actions are recorded in the undo manager."), Category("AddFlow Capabilities"), DefaultValue(false)]
        public bool SkipUndo {
            get {
                return this.m_undo.SkipUndo;
            }
            set {
                this.m_undo.SkipUndo = value;
            }
        }
 
        [Browsable(false), Description("Returns the index of the current stretching point.")]
        public int StretchingPoint {
            get {
                return this.m_stretchingPoint;
            }
        }
 
        [Description("Returns the code of the next undoable action."), Browsable(false)]
        public Action UndoCode {
            get {
                return this.m_undo.UndoCode;
            }
        }
 
        [Description("Sets and returns the number of undo commands that can be performed."), DefaultValue(0), Category("AddFlow Capabilities")]
        public int UndoSize {
            get {
                return this.m_undo.UndoSize;
            }
            set {
                this.m_undo.UndoSize = value;
            }
        }
 
        [Category("AddFlow Interactivity"), Description("Returns/sets the horizontal and vertical zooming factors."), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Zoom Zoom {
            get {
                return this.m_zoom;
            }
            set {
                if ((value.X <= 0f) || (value.Y <= 0f)) {
                    throw new AddFlowException(AddFlow.GetExceptionText(AddFlow.ExceptionType.ZoomMustBePositive));
                }
                if (!this.m_zoom.Equals(value)) {
                    this.m_zoom = value;
                    this.ConvertSomeSizesInWorldCoordinates();
                    if (this.m_repaint == 0) {
                        if (this.AutoScroll) {
                            this.UpdateScrollInfo();
                        }
                        base.Invalidate();
                    }
                }
            }
        }
 
        public event AddFlow.AfterAddLinkEventHandler AfterAddLink;
 
        public event AddFlow.AfterAddNodeEventHandler AfterAddNode;
 
        public event AddFlow.AfterEditEventHandler AfterEdit;
 
        public event AddFlow.AfterMoveEventHandler AfterMove;
 
        public event AddFlow.AfterResizeEventHandler AfterResize;
 
        public event AddFlow.AfterSelectEventHandler AfterSelect;
 
        public event AddFlow.AfterStretchEventHandler AfterStretch;
 
        public event AddFlow.BeforeAddLinkEventHandler BeforeAddLink;
 
        public event AddFlow.BeforeAddNodeEventHandler BeforeAddNode;
 
        public event AddFlow.BeforeChangeDstEventHandler BeforeChangeDst;
 
        public event AddFlow.BeforeChangeOrgEventHandler BeforeChangeOrg;
 
        public event AddFlow.BeforeEditEventHandler BeforeEdit;
 
        internal event AddFlow.DesignModeChangeEventHandler DesignModeChange;
 
        public event AddFlow.DiagramOwnerDrawEventHandler DiagramOwnerDraw;
 
        public event AddFlow.ErrorEventHandler Error;
 
        public event AddFlow.ExtentChangeEventHandler ExtentChange;
 
        public event AddFlow.LinkOwnerDrawEventHandler LinkOwnerDraw;
 
        public event AddFlow.NodeOwnerDrawEventHandler NodeOwnerDraw;
 
        public event AddFlow.ScrollEventHandler Scroll;
 
        //private AddFlow.AfterAddLinkEventHandler AfterAddLink;
        //private AddFlow.AfterAddNodeEventHandler AfterAddNode;
        //private AddFlow.AfterEditEventHandler AfterEdit;
        //private AddFlow.AfterMoveEventHandler AfterMove;
        //private AddFlow.AfterResizeEventHandler AfterResize;
        //private AddFlow.AfterSelectEventHandler AfterSelect;
        //private AddFlow.AfterStretchEventHandler AfterStretch;
        //private AddFlow.BeforeAddLinkEventHandler BeforeAddLink;
        //private AddFlow.BeforeAddNodeEventHandler BeforeAddNode;
        //private AddFlow.BeforeChangeDstEventHandler BeforeChangeDst;
        //private AddFlow.BeforeChangeOrgEventHandler BeforeChangeOrg;
        //private AddFlow.BeforeEditEventHandler BeforeEdit;
        //private AddFlow.DesignModeChangeEventHandler DesignModeChange;
        //private AddFlow.DiagramOwnerDrawEventHandler DiagramOwnerDraw;
        //private AddFlow.ErrorEventHandler Error;
        //private AddFlow.ExtentChangeEventHandler ExtentChange;
        //private AddFlow.LinkOwnerDrawEventHandler LinkOwnerDraw;

        private Container components;

        internal License license;
 
        private ArrayList m_aDragNodes;
 
        private ArrayList m_aNodes;
 
        private bool m_antiAliasing;
 
        private int[] m_aTimerDuration;
 
        private bool m_beginMove;
 
        private bool m_canChangeDst;
 
        private bool m_canChangeOrg;
 
        private bool m_canDragScroll;
 
        private bool m_canDrawLink;
 
        private bool m_canDrawNode;
 
        private bool m_canFireError;
 
        private bool m_canLabelEdit;
 
        private bool m_canMoveNode;
 
        private bool m_canMultiLink;
 
        private bool m_canReflexLink;
 
        private bool m_canSizeNode;
 
        private bool m_canStretchLink;
 
        private int m_changed;
 
        private PointF m_checkedAreaPoint;
 
        private AddFlow.ClickZone m_clickEvent;
 
        internal Connect m_connect;
 
        private CursorSetting m_cursorSetting;
 
        private CycleMode m_cycleMode;
 
        internal DefLink m_deflink;
 
        internal DefNode m_defnode;
 
        private bool m_doubleClick;
 
        internal bool m_drawAll;
 
        private Grid m_grid;
 
        private int m_handle;
 
        internal ImagesCollection m_images;
 
        internal bool m_inPlaceEdition;
 
        private InteractiveAction m_interactiveAction;
 
        private bool m_interHandles;
 
        private bool m_internalUnselect;
 
        internal ItemsCollection m_items;
 
        internal int m_jumpSize;
 
        internal bool m_licensed;
 
        private static string m_licenseKey;
 
        private LinkCreationMode m_linkCreationMode;
 
        internal int m_linkGrabSize;
 
        private HandleSize m_linkHandleSize;
 
        internal int m_linkWidth;
 
        internal float m_linkWidthWorld;
 
        private SizeF m_minSizeWorld;
 
        private MouseAction m_mouseAction;
 
        private bool m_mouseDown;
 
        private SizeF m_moveStartDist;
 
        private bool m_multiSel;
 
        private RectangleF m_netrc;
 
        internal NodesCollection m_nodes;
 
        private int m_nTimer;
 
        private Point m_oldScrollPos;
 
        private RectangleF m_oldselrc;
 
        private Node m_org;
 
        internal bool m_out;
 
        private SizeF m_outlineSize;
 
        private bool m_ownerDraw;
 
        internal float m_pageScale;
 
        internal GraphicsUnit m_pageUnit;
 
        private MouseArea m_pointedArea;
 
        private Item m_pointedItem;
 
        private InteractiveAction m_priorMode;
 
        private PointF m_ptDst;
 
        private PointF m_ptOrg;
 
        private PointF m_ptOut;
 
        private PointF m_ptPrior;
 
        private PointF m_ptStart;
 
        private RectangleF m_rc;
 
        internal Rectangle m_rcinv;
 
        private RemovePointAngle m_removePointAngle;
 
        internal int m_repaint;
 
        private Node m_resizedNode;
 
        internal int m_roundSize;
 
        private AutoSize m_savedAutoSize;
 
        private ScrollbarsDisplayMode m_scrollbarsDisplayMode;
 
        private Point m_scrollPos;
 
        private Size m_scrollUnit;
 
        private bool m_selChanged;
 
        internal int m_selGrabSize;
 
        private HandleSize m_selHandleSize;
 
        internal ItemsCollection m_selItems;
 
        private RectangleF m_selrc;
 
        private bool m_showTooltips;
 
        private AddFlow.SizeDir m_sizeDir;
 
        private bool m_startMove;
 
        private Link m_stretchedLink;
 
        private int m_stretchingPoint;
 
        private AddFlow.Stretch m_stretchType;
 
        private InPlaceTextBox m_textBox;
 
        private Timer m_timerEdit;
 
        private Timer m_timerScroll;
 
        private RectangleF m_tmprc;
 
        private ToolTip m_tooltipCtrl;
 
        internal Undoman m_undo;
 
        private AddFlow.ScrollDir m_xScrollDir;
 
        private AddFlow.ScrollDir m_yScrollDir;
 
        private Zoom m_zoom;
 
        private const int MinSize = 3;
 
        private const uint MoveStartDist = 5;
 
        //private AddFlow.NodeOwnerDrawEventHandler NodeOwnerDraw;
 
        private const int OutLineSize = 2;
 
        //private AddFlow.ScrollEventHandler Scroll;
 
 


    }


    internal class AboutDialogBox : Form {
        // Methods
        public AboutDialogBox(AddFlow addflow) {
            this.button = null;
            Assembly assembly1 = Assembly.GetExecutingAssembly();
            ResourceManager manager1 = new ResourceManager("Lassalle.Flow.af", assembly1);
            string text1 = ((string) manager1.GetObject("Version")) + " " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string text2 = "";
            string text3 = "";
            object[] objArray1 = assembly1.GetCustomAttributes(false);
            for (int num1 = 0; num1 < objArray1.Length; num1++) {
                string text4 = objArray1[num1].GetType().ToString();
                if (text4 == "System.Reflection.AssemblyCopyrightAttribute") {
                    AssemblyCopyrightAttribute attribute1 = (AssemblyCopyrightAttribute) objArray1[num1];
                    text2 = attribute1.Copyright.ToString();
                }
                else if (text4 == "System.Reflection.AssemblyProductAttribute") {
                    AssemblyProductAttribute attribute2 = (AssemblyProductAttribute) objArray1[num1];
                    text3 = attribute2.Product.ToString();
                }
            }
            this.Text = (string) manager1.GetObject("About");
            base.StartPosition = FormStartPosition.CenterParent;
            base.FormBorderStyle = FormBorderStyle.FixedDialog;
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.ShowInTaskbar = false;
            base.ControlBox = false;
            Label label1 = new Label();
            label1.Parent = this;
            label1.Font = new Font("Verdana", 14f);
            label1.AutoSize = true;
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Text = " " + text3 + " ";
            Image image1 = (Image) manager1.GetObject("AddFlowIcon");
            PictureBox box1 = new PictureBox();
            box1.Parent = this;
            box1.Image = image1;
            box1.SizeMode = PictureBoxSizeMode.AutoSize;
            box1.Location = new Point(label1.Font.Height / 2, label1.Font.Height / 2);
            label1.Location = new Point(box1.Right, label1.Font.Height / 2);
            int num2 = label1.Right;
            Label label2 = new Label();
            label2.Parent = this;
            label2.Font = new Font("Verdana", 10f);
            label2.Location = new Point(0, label1.Bottom + (label2.Font.Height / 2));
            label2.Size = new Size(num2, label2.Font.Height);
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.Text = text1;
            Label label3 = new Label();
            label3.Parent = this;
            label3.Font = new Font("Verdana", 10f);
            label3.Location = new Point(0, label2.Bottom + (label3.Font.Height / 2));
            label3.Size = new Size(num2, label3.Font.Height);
            label3.TextAlign = ContentAlignment.MiddleCenter;
            label3.Text = text2;
            LinkLabel label4 = new LinkLabel();
            label4.Parent = this;
            label4.Font = new Font("Verdana", 10f);
            label4.Location = new Point(0, label3.Bottom + (label4.Font.Height / 2));
            label4.Size = new Size(num2, label4.Font.Height);
            label4.TextAlign = ContentAlignment.MiddleCenter;
            label4.Text = (string) manager1.GetObject("Web");
            label4.LinkClicked += new LinkLabelLinkClickedEventHandler(this.labelWeb_LinkClicked);
            label4.TabStop = false;
            Label label5 = new Label();
            label5.Parent = this;
            label4.Font = new Font("Verdana", 10f);
            label5.Location = new Point(0, label4.Bottom + (label5.Font.Height / 2));
            label5.Size = new Size(num2, label5.Font.Height);
            label5.TextAlign = ContentAlignment.MiddleCenter;
            string text5 = addflow.m_licensed ? ((AddFlowLicense) addflow.license).HumanLicenseKey : null;
            if (text5 != null) {
                label5.Text = ((string) manager1.GetObject("LicenseNumberLabel")) + text5;
            }
            else {
                label5.ForeColor = Color.Red;
                label5.Text = (string) manager1.GetObject("NoLicenseWarning");
            }
            this.button = new Button();
            this.button.Parent = this;
            this.button.Text = "OK";
            this.button.Size = new Size(5 * this.button.Font.Height, 2 * this.button.Font.Height);
            this.button.Location = new Point((num2 - this.button.Size.Width) / 2, label5.Bottom + (2 * this.button.Font.Height));
            this.button.DialogResult = DialogResult.OK;
            base.CancelButton = this.button;
            base.AcceptButton = this.button;
            base.ClientSize = new Size(num2, this.button.Bottom + this.button.Font.Height);
            //if (text5 == null) {
            //    Timer timer1 = new Timer();
            //    timer1.Interval = 3000;
            //    timer1.Tick += new EventHandler(this.TimerOnTick);
            //    timer1.Start();
            //    this.button.Enabled = false;
            //}
        }
 
        private void labelWeb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("http://www.lassalle.com");
        }
 
        private void TimerOnTick(object obj, EventArgs ea) {
            Timer timer1 = (Timer) obj;
            timer1.Stop();
            timer1.Tick -= new EventHandler(this.TimerOnTick);
            this.button.Enabled = true;
            this.button.Focus();
        }
 
        private Button button;
    }
 



}


