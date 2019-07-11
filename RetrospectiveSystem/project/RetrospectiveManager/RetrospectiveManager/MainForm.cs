using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls.UI.Docking;
using CommonUtils.Logger;
using System.Web;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using RetrospectiveManager.RadView;
using RetrospectiveManager.Control;

namespace RetrospectiveManager
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        [DllImport("user32.dll")]
        private static extern bool AnimateWindow(IntPtr hWnd,int dwTime,int dwFlags);

        private MesService.MesServiceClient serviceClient;
        public MainForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        async private void MainForm_Load(object sender, EventArgs e)
        {
            serviceClient = new MesService.MesServiceClient();
            await serviceClient.InitConnectStringAsync();
            InitListView();
            LoadTreeView();
            InitControl();
            this.radDock1.RemoveAllDocumentWindows();
            this.radDock1.AddDocument(documentWindow_testRes);
            this.radDock1.AddDocument(documentWindow_material_select);
            this.radDock1.AddDocument(documentWindow_packageProduct);
            this.radDock1.AddDocument(documentWindow_passRes);

            if (Login.GetUserType == Login.UserType.USER_ADMIN)
            {
                tool_status_user.Text = "管理员";

            } else if (Login.GetUserType == Login.UserType.USER_ORDINARY)
            {
                tool_status_user.Text = "普通用户";
            }

            menu_set_station.Click += Menu_set_station_Click;
            menu_produce_config.Click += Menu_produce_config_Click;
            menu_productType.Click += Menu_productType_Click;
            menu_select_testRes.Click += Menu_typeno_Click;
            menu_material_msg.Click += Menu_material_msg_Click;
            menu_product_binding.Click += Menu_product_binding_Click;
            menu_product_material.Click += Menu_product_material_Click;
            menu_select_testRes.Click += Menu_select_testRes_Click;
            menu_select_material.Click += Menu_select_material_Click;
            menu_select_packageProduct.Click += Menu_select_packageProduct_Click;
            menu_select_passRate.Click += Menu_select_passRate_Click;

            btn_search_record.Click += Btn_search_record_Click;
            btn_search_lastTestRes.Click += Btn_search_LastTestRes_Click;
        }

        private void Menu_select_passRate_Click(object sender, EventArgs e)
        {
            //合格率
            this.radDock1.AddDocument(this.documentWindow_passRes);
        }

        private void Menu_select_packageProduct_Click(object sender, EventArgs e)
        {
            //产品打包
            this.radDock1.AddDocument(this.documentWindow_packageProduct);
        }

        private void Menu_select_material_Click(object sender, EventArgs e)
        {
            //物料统计
            this.radDock1.AddDocument(this.documentWindow_material_select);
        }

        private void Menu_select_testRes_Click(object sender, EventArgs e)
        {
            //测试结果
            this.radDock1.AddDocument(documentWindow_testRes);
        }

        private void Menu_product_material_Click(object sender, EventArgs e)
        {
            ProductMaterial productMaterial = new ProductMaterial();
            productMaterial.ShowDialog();
        }

        private void Menu_product_binding_Click(object sender, EventArgs e)
        {
            PackageProduct packageProduct = new PackageProduct();
            packageProduct.ShowDialog();
        }

        private void Menu_material_msg_Click(object sender, EventArgs e)
        {
            Material material = new Material();
            material.ShowDialog();
        }

        async private void Btn_search_LastTestRes_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(tb_sn.Text))
                {
                    MessageBox.Show("追溯号不能为空！","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                if (string.IsNullOrEmpty(cb_typeNo.Text))
                {
                    MessageBox.Show("零件号不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if(string.IsNullOrEmpty(cb_station.Text))
                {
                    MessageBox.Show("站位名不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //由追溯码+型号+当前站位
                DataTable dt = (await serviceClient.SelectLastTestResultUpperAsync(tb_sn.Text.Trim(), cb_typeNo.Text.Trim(),cb_station.Text.Trim())).Tables[0];
                listView_TestRes.DataSource = dt;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message+"\r\n"+ex.StackTrace);
            }
        }

        async private void Btn_search_record_Click(object sender, EventArgs e)
        {
            //由追溯码/零件号/站位名 查询历史记录
            string sn = tb_sn.Text.Trim();
            string typeNo = cb_typeNo.Text.Trim();
            string station = cb_station.Text.Trim();
            DataTable dt = null;
            dt = (await serviceClient.SelectTestResultUpperAsync(sn, typeNo, station, false)).Tables[0];

            listView_TestRes.DataSource = dt;
        }

        private void Menu_typeno_Click(object sender, EventArgs e)
        {
            this.radDock1.AddDocument(documentWindow_testRes);
        }

        private void Menu_productType_Click(object sender, EventArgs e)
        {
            ProductType productType = new ProductType();
            productType.StartPosition = FormStartPosition.CenterParent;
            productType.ShowDialog();
        }

        private void Menu_produce_config_Click(object sender, EventArgs e)
        {
            Station setProduce = new Station();
            setProduce.StartPosition = FormStartPosition.CenterParent;
            setProduce.ShowDialog();
        }

        private void Menu_set_station_Click(object sender, EventArgs e)
        {
            Login.UserType userType = Login.GetUserType;
            if (userType == Login.UserType.USER_ADMIN)
            {
                //管理员
                SetStationAdmin setStationAdmin = new SetStationAdmin();
                setStationAdmin.StartPosition = FormStartPosition.CenterParent;
                setStationAdmin.ShowDialog();
                
            }
            else if (userType == Login.UserType.USER_ORDINARY)
            {
                //普通用户
                SetStation setStation = new SetStation();
                setStation.StartPosition = FormStartPosition.CenterParent;
                setStation.ShowDialog();
            }
        }

        private void InitListView()
        {
            listView_TestRes.ViewType = ListViewType.DetailsView;
            listView_TestRes.ShowGridLines = true;
        }

        public void LoadTreeView()
        {
            TreeViewControl treeView = new TreeViewControl(radTreeView1);
            treeView.LoadTreeView();
            radTreeView1.NodeMouseClick += RadTreeView1_NodeMouseClick;
        }

        private void RadTreeView1_NodeMouseClick(object sender, RadTreeViewEventArgs e)
        {
            RadTreeNode treeNode = e.Node;
            switch (treeNode.Text)
            {
                case "产品A":
                    
                    break;
            }
        }

        private void Menu_manager_Click(object sender, EventArgs e)
        {
            this.toolWindow_left.Show();
        }

        async private void InitControl()
        {
            //type no 
            cb_typeNo.Items.Clear();
            cb_station.Items.Clear();
            try
            {
                DataTable dt = (await serviceClient.SelectProductTypeNoAsync("")).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cb_typeNo.Items.Add(dt.Rows[i][0]);
                }
                cb_typeNo.Items.Add("");
                //station
                dt = (await serviceClient.SelectStationAsync("", "")).Tables[0];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cb_station.Items.Add(dt.Rows[i][1]);
                }
                cb_station.Items.Add("");
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message+"\r\n"+ex.StackTrace);
            }
        }
    }
}
