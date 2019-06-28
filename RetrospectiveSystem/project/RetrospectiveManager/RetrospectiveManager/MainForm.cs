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
            this.radDock1.RemoveAllDocumentWindows();
            this.radDock1.AddDocument(documentWindow_typeNo);
            this.radDock1.AddDocument(documentWindow_sn);
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
            menu_sn.Click += Menu_sn_Click;
            menu_typeno.Click += Menu_typeno_Click;

            btn_search_record.Click += Btn_search_record_Click;
            btn_search_typeNo.Click += Btn_search_typeNo_Click;
            btn_search_last_station.Click += Btn_search_last_station_Click;
        }

        private void Btn_search_last_station_Click(object sender, EventArgs e)
        {
            //由追溯码 查询上一站位

        }

        async private void Btn_search_typeNo_Click(object sender, EventArgs e)
        {
            //由零件号 查询记录
            DataTable dt = (await serviceClient.SelectProductDataOfTypeNoAsync(tb_typeNo.Text.Trim())).Tables[0];
            radListView2.DataSource = dt;
        }

        async private void Btn_search_record_Click(object sender, EventArgs e)
        {
            //由追溯码 查询历史记录
            DataTable dt = (await serviceClient.SelectProductDataOfSNAsync(tb_sn.Text.Trim(), true)).Tables[0];
            radListView1.DataSource = dt;
        }

        private void Menu_typeno_Click(object sender, EventArgs e)
        {
            this.radDock1.AddDocument(documentWindow_typeNo);
        }

        private void Menu_sn_Click(object sender, EventArgs e)
        {
            this.radDock1.AddDocument(documentWindow_sn);
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

            radListView2.ViewType = ListViewType.DetailsView;
            radListView2.ShowGridLines = true;
        }
    }
}
