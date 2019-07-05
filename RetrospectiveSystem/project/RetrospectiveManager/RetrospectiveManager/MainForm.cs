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

        private void MainForm_Load(object sender, EventArgs e)
        {
            serviceClient = new MesService.MesServiceClient();
            InitListView();
            LoadTreeView();
            this.radDock1.RemoveAllDocumentWindows();
            this.radDock1.AddDocument(documentWindow_typeNo);
            this.radDock1.AddDocument(documentWindow_sn);
            this.radDock1.AddDocument(documentWindow_packageProduct);
            this.radDock1.AddDocument(documentWindow_statistic);

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
            menu_outbox_material.Click += Menu_outbox_material_Click;
            menu_product_material.Click += Menu_product_material_Click;

            btn_search_record.Click += Btn_search_record_Click;
            btn_search_testRes.Click += Btn_search_testRes_Click;

            rdb_sn.CheckStateChanged += Rdb_sn_CheckStateChanged;
            rdb_typeNo.CheckStateChanged += Rdb_typeNo_CheckStateChanged;
        }

        private void Rdb_typeNo_CheckStateChanged(object sender, EventArgs e)
        {
            if (rdb_typeNo.CheckState == CheckState.Checked)
            {
                btn_search_testRes.Text = "查询";
                btn_search_record.Visible = false;
            }
        }

        private void Rdb_sn_CheckStateChanged(object sender, EventArgs e)
        {
            if (rdb_sn.CheckState == CheckState.Checked)
            {
                btn_search_testRes.Text = "查询上一站位";
                btn_search_record.Visible = true;
            }
        }

        private void Menu_product_material_Click(object sender, EventArgs e)
        {
            ProductMaterial productMaterial = new ProductMaterial();
            productMaterial.ShowDialog();
        }

        private void Menu_outbox_material_Click(object sender, EventArgs e)
        {
            MaterialOutBox materialOutBox = new MaterialOutBox();
            materialOutBox.ShowDialog();
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

        async private void Btn_search_testRes_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdb_typeNo.CheckState == CheckState.Checked)
                {
                    //由零件号 查询记录
                    DataTable dt = (await serviceClient.SelectProductDataOfTypeNoAsync(tb_input.Text.Trim())).Tables[0];
                    listView_TestRes.DataSource = dt;
                }
                else if (rdb_sn.CheckState == CheckState.Checked)
                {
                    //查询上一站位
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message+"\r\n"+ex.StackTrace);
            }
        }

        async private void Btn_search_record_Click(object sender, EventArgs e)
        {
            //由追溯码 查询历史记录
            DataTable dt = (await serviceClient.SelectProductDataOfSNAsync(tb_input.Text.Trim(), true)).Tables[0];
            listView_TestRes.DataSource = dt;
        }

        private void Menu_typeno_Click(object sender, EventArgs e)
        {
            this.radDock1.AddDocument(documentWindow_typeNo);
        }

        private void Menu_productType_Click(object sender, EventArgs e)
        {
            ProductType productType = new ProductType();
            productType.StartPosition = FormStartPosition.CenterParent;
            productType.ShowDialog();
        }

        private void Menu_produce_config_Click(object sender, EventArgs e)
        {
            SetProduce setProduce = new SetProduce();
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
            radListView1.ViewType = ListViewType.DetailsView;
            radListView1.ShowGridLines = true;

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
    }
}
