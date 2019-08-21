using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using MesManager.Control;
using MesManager.Control.TreeViewUI;
using MesManager.Properties;

namespace MesManager.UI
{
    public partial class TestStand : RadForm
    {
        private MesService.MesServiceClient serviceClient;
        private TestStandDataType currentDataType;
        public TestStand()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        enum TestStandDataType
        {
            TEST_LIMIT_CONFIG,
            TEST_PROGRAME_VERSION,
            TEST_LOG_DATA
        }

        private void TestStand_Load(object sender, EventArgs e)
        {
            Init();
            EventHandlers();
        }

        private void EventHandlers()
        {
            menu_limit_cfg.Click += Menu_limit_cfg_Click;
            menu_log_data.Click += Menu_log_data_Click;
            menu_programe_version.Click += Menu_programe_version_Click;
            tool_refresh.Click += Tool_refresh_Click;
            tool_productTypeNo.SelectedIndexChanged += Tool_productTypeNo_SelectedIndexChanged;
        }

        private void Tool_productTypeNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshUI();
        }

        private void Tool_refresh_Click(object sender, EventArgs e)
        {
            RefreshUI();
        }

        private void Menu_programe_version_Click(object sender, EventArgs e)
        {
            currentDataType = TestStandDataType.TEST_PROGRAME_VERSION;
            RefreshUI();
        }

        private void Menu_log_data_Click(object sender, EventArgs e)
        {
            currentDataType = TestStandDataType.TEST_LOG_DATA;
            RefreshUI();
        }

        private void Menu_limit_cfg_Click(object sender, EventArgs e)
        {
            currentDataType = TestStandDataType.TEST_LIMIT_CONFIG;
            RefreshUI();
        }

        private void RefreshUI()
        {
            if (currentDataType == TestStandDataType.TEST_LIMIT_CONFIG)
            {
                SelectTestLimitConfig(this.tool_productTypeNo.Text);
                this.radGridView1.Dock = DockStyle.Fill;
                this.radGridView1.Visible = true;
                this.panel1.Visible = false;
            }
            else if (currentDataType == TestStandDataType.TEST_LOG_DATA)
            {
                this.panel1.Dock = DockStyle.Fill;
                this.radGridView1.Visible = false;
                this.panel1.Visible = true;
            }
            else if (currentDataType == TestStandDataType.TEST_PROGRAME_VERSION)
            {
                SelectTestProgrameVersion(this.tool_productTypeNo.Text);
                this.radGridView1.Dock = DockStyle.Fill;
                this.radGridView1.Visible = true;
                this.panel1.Visible = false;
            }
        }

        async private void Init()
        {
            serviceClient = new MesService.MesServiceClient();
            this.panel1.Visible = false;
            this.radGridView1.Dock = DockStyle.Fill;
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1,false);
            this.radGridView1.ReadOnly = true;
            var dt = (await serviceClient.SelectProductContinairCapacityAsync("")).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    this.tool_productTypeNo.Items.Add(dt.Rows[i][0].ToString());
                }
            }
            //init treeview
            string path = @"D:\work\project\FigKey\RetrospectiveSystem\project\IIS";
            ImageList imageList = new ImageList();
            imageList.Images.Add("open", Resources.FolderList32);
            LoadTreeView.SetTreeNoByFilePath(this.treeView1,path,new ImageList());
            //TreeViewData.PopulateTreeView(path, this.treeView1);
        }

        async private void SelectTestLimitConfig(string productTypeNo)
        {
            var dt = (await serviceClient.SelectTestLimitConfigAsync(productTypeNo)).Tables[0];
            this.radGridView1.DataSource = null;
            this.radGridView1.DataSource = dt;
        }

        async private void SelectTestProgrameVersion(string productTypeNo)
        {
            var dt = (await serviceClient.SelectTestProgrameVersionAsync(productTypeNo)).Tables[0];
            this.radGridView1.DataSource = null;
            this.radGridView1.DataSource = dt;
        }
    }
}
