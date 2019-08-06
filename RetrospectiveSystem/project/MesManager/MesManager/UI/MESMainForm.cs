using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using MesManager.UI;

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
        private void MESMainForm_Load(object sender, EventArgs e)
        {
            EventHandlers();
        }

        private void EventHandlers()
        {
            this.mainBasicInfo.Click += MainBasicInfo_Click;
            //this.mainCheckStation.Click += MainCheckStation_Click;
            this.mainGraphView.Click += MainGraphView_Click;
            this.mainMaterialManager.Click += MainMaterialManager_Click;
            //this.mainProduceManager.Click += MainProduceManager_Click;
            //this.mainProductCheck.Click += MainProductCheck_Click;
            this.mainProductPackage.Click += MainProductPackage_Click;
            this.mainQuanlityAnomaly.Click += MainQuanlityAnomaly_Click;
            //this.mainRepairCenter.Click += MainRepairCenter_Click;
            this.mainReportData.Click += MainReportData_Click;
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

        #region Event Handlers

        private void MainReportData_Click(object sender, EventArgs e)
        {
            //报表中心/追溯中心
            SNCenter sNCenter = new SNCenter();
            sNCenter.ShowDialog();
        }

        private void MainRepairCenter_Click(object sender, EventArgs e)
        {
            //维修中心
            RepairCenter repairCenter = new RepairCenter();
            repairCenter.ShowDialog();
        }

        private void MainQuanlityAnomaly_Click(object sender, EventArgs e)
        {
            //品质异常管理
            QuanlityAnomaly quanlityAnomaly = new QuanlityAnomaly();
            quanlityAnomaly.ShowDialog();
        }

        private void MainProductPackage_Click(object sender, EventArgs e)
        {
            //成品装箱
            ProductPackage productPackage = new ProductPackage();
            productPackage.ShowDialog();
        }

        private void MainProductCheck_Click(object sender, EventArgs e)
        {
            //成品抽检
            ProductCheck productCheck = new ProductCheck();
            productCheck.ShowDialog();
        }

        private void MainProduceManager_Click(object sender, EventArgs e)
        {
            //生产管理
            //建立工艺生产线
        }

        private void MainMaterialManager_Click(object sender, EventArgs e)
        {
            //物料绑定
            ProductMaterial productMaterial = new ProductMaterial();
            productMaterial.ShowDialog();
        }

        private void MainGraphView_Click(object sender, EventArgs e)
        {
            //看板中心
            //图形显示
            GraphView graphView = new GraphView();
            graphView.ShowDialog();
        }

        private void MainCheckStation_Click(object sender, EventArgs e)
        {
            //检验过站
            CheckStation checkStation = new CheckStation();
            checkStation.ShowDialog();
        }

        private void MainBasicInfo_Click(object sender, EventArgs e)
        {
            //基础信息/基础配置
            //配置所有基本的信息界面
            //如配置型号/配置产线/配置物料信息等
            BasicConfig basicConfig = new BasicConfig();
            basicConfig.ShowDialog();
        }

        void titleBar_MaximizeRestore(object sender, EventArgs args)
        {
            int left = Screen.PrimaryScreen.Bounds.Width;
            int top = Screen.PrimaryScreen.Bounds.Height;
            int right = Screen.PrimaryScreen.Bounds.Width;
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;

                this.toolsGroup.Margin = new System.Windows.Forms.Padding(left / 3, top / 4, right / 4, 0);
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                //this.toolsGroup.Margin = new System.Windows.Forms.Padding(200, 130, 200, 0);
                this.toolsGroup.Margin = new System.Windows.Forms.Padding(300, 150, 200, 0);
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

        #endregion
    }
}
