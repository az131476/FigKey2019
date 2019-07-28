using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace MesManager.RadView
{
    public partial class MESMainForm : Telerik.WinControls.UI.RadForm
    {
        private RadTitleBarElement titleBar;
        private bool isFormMoving = false;
        public MESMainForm()
        {
            InitializeComponent();
            PrepareTitleBar();
        }

        private void PrepareTitleBar()
        {
            titleBar = new RadTitleBarElement();
            titleBar.Text = "万通智控产线追溯MES系统";
            titleBar.ForeColor = Color.White;
            titleBar.Font = new Font("Segoe UI Light", 21.75F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            titleBar.FillPrimitive.Visibility = ElementVisibility.Hidden;
            titleBar.MaxSize = new Size(0, 50);
            titleBar.Children[1].Visibility = ElementVisibility.Hidden;

            titleBar.CloseButton.Parent.PositionOffset = new SizeF(0, 10);//与top距离
            titleBar.CloseButton.MinSize = new Size(50, 20);
            titleBar.CloseButton.ButtonFillElement.Visibility = ElementVisibility.Collapsed;

            titleBar.MinimizeButton.MinSize = new Size(50, 50);
            titleBar.MinimizeButton.ButtonFillElement.Visibility = ElementVisibility.Collapsed;

            titleBar.MaximizeButton.MinSize = new Size(50, 50);
            titleBar.MaximizeButton.ButtonFillElement.Visibility = ElementVisibility.Collapsed;

            titleBar.CloseButton.SetValue(RadFormElement.IsFormActiveProperty, true);
            titleBar.MinimizeButton.SetValue(RadFormElement.IsFormActiveProperty, true);
            titleBar.MaximizeButton.SetValue(RadFormElement.IsFormActiveProperty, true);

            titleBar.Close += new TitleBarSystemEventHandler(titleBar_Close);
            titleBar.Minimize += new TitleBarSystemEventHandler(titleBar_Minimize);
            titleBar.MaximizeRestore += new TitleBarSystemEventHandler(titleBar_MaximizeRestore);
            this.radPanorama1.PanoramaElement.PanGesture += new PanGestureEventHandler(radTilePanel1_PanGesture);
            this.radPanorama1.PanoramaElement.Children.Add(titleBar);
        }

        void titleBar_MaximizeRestore(object sender, EventArgs args)
        {
            //int left = this.Width / 6;
            //int top = this.Height / 3;

            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;

                this.toolsGroup.Margin = new System.Windows.Forms.Padding(500, 300, 200, 0);
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.toolsGroup.Margin = new System.Windows.Forms.Padding(100, 130, 65, 0);
            }
        }

        void titleBar_Minimize(object sender, EventArgs args)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        void titleBar_Close(object sender, EventArgs args)
        {
            Application.Exit();
        }

        void radTilePanel1_PanGesture(object sender, PanGestureEventArgs e)
        {
            if (e.IsBegin && this.titleBar.ControlBoundingRectangle.Contains(e.Location))
            {
                isFormMoving = true;
            }

            if (isFormMoving)
            {
                this.Location = new Point(this.Location.X + e.Offset.Width, this.Location.Y + e.Offset.Height);
            }
            else
            {
                e.Handled = false;
            }

            if (e.IsEnd)
            {
                isFormMoving = false;
            }
        }
    }
}
