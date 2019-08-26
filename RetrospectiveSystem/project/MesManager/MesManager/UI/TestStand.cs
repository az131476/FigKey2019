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
using MesManager.TelerikWinform.GridViewCommon.GridViewDataExport;
using Telerik.WinControls.UI.Export;
using CommonUtils.Logger;
using CommonUtils.FileHelper;

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
            tool_logData.Click += Tool_logData_Click;
            tool_specCfg.Click += Tool_specCfg_Click;
            tool_programv.Click += Tool_refresh_Click;
            tool_queryCondition.SelectedIndexChanged += Tool_productTypeNo_SelectedIndexChanged;
            this.radGridView1.CellDoubleClick += RadGridView1_CellDoubleClick;
            this.tool_export.Click += Tool_export_Click;
        }

        private void Tool_export_Click(object sender, EventArgs e)
        {
            ExportGridViewData(0,this.radGridView1);
        }

        private void RadGridView1_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            var productTypeNo = this.radGridView1.CurrentRow.Cells[0].Value.ToString();
            var productSN = this.radGridView1.CurrentRow.Cells[1].Value.ToString();
            var stationName = this.radGridView1.CurrentRow.Cells[2].Value.ToString();
            TestLogDetail testLogDetail = new TestLogDetail(productTypeNo,productSN,stationName);
            testLogDetail.ShowDialog();
        }

        private void Tool_productTypeNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshUI();
        }

        private void Tool_refresh_Click(object sender, EventArgs e)
        {
            currentDataType = TestStandDataType.TEST_PROGRAME_VERSION;
            RefreshUI();
        }

        private void Tool_logData_Click(object sender, EventArgs e)
        {
            currentDataType = TestStandDataType.TEST_LOG_DATA;
            RefreshUI();
        }

        private void Tool_specCfg_Click(object sender, EventArgs e)
        {
            currentDataType = TestStandDataType.TEST_LIMIT_CONFIG;
            RefreshUI();
        }

        private void RefreshUI()
        {
            if (currentDataType == TestStandDataType.TEST_LIMIT_CONFIG)
            {
                SelectTestLimitConfig(this.tool_queryCondition.Text);
                this.radGridView1.Dock = DockStyle.Fill;
                this.radGridView1.Visible = true;
                this.panel1.Visible = false;
            }
            else if (currentDataType == TestStandDataType.TEST_LOG_DATA)
            {
                SelectTestLogData(this.tool_queryCondition.Text);
                this.radGridView1.Dock = DockStyle.Fill;
                this.radGridView1.Visible = true;
                this.panel1.Visible = false;
            }
            else if (currentDataType == TestStandDataType.TEST_PROGRAME_VERSION)
            {
                SelectTestProgrameVersion(this.tool_queryCondition.Text);
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
                    this.tool_queryCondition.Items.Add(dt.Rows[i][0].ToString());
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
            if (dt.Rows.Count < 1)
                return;
            this.radGridView1.DataSource = null;
            this.radGridView1.DataSource = dt;
        }

        async private void SelectTestProgrameVersion(string productTypeNo)
        {
            var dt = (await serviceClient.SelectTestProgrameVersionAsync(productTypeNo)).Tables[0];
            if (dt.Rows.Count < 1)
                return;
            this.radGridView1.DataSource = null;
            this.radGridView1.DataSource = dt;
        }

        async private void SelectTestLogData(string queryFilter)
        {
            var dt = (await serviceClient.SelectTodayTestLogDataAsync(queryFilter)).Tables[0];
            if (dt.Rows.Count < 1)
                return;
            this.radGridView1.DataSource = null;
            this.radGridView1.DataSource = dt;
        }

        private void ExportGridViewData(int selectIndex, RadGridView radGridView)
        {
            var filter = "Excel (*.xls)|*.xls";
            if (selectIndex == (int)ExportFormat.EXCEL)
            {
                filter = "Excel (*.xls)|*.xls";
                var path = FileSelect.SaveAs(filter, "C:\\");
                if (path == "")
                    return;
                ExportData.RunExportToExcelML(path, radGridView);
            }
            else if (selectIndex == (int)ExportFormat.HTML)
            {
                filter = "Html File (*.htm)|*.htm";
                var path = FileSelect.SaveAs(filter, "C:\\");
                if (path == "")
                    return;
                ExportData.RunExportToHTML(path, radGridView);
            }
            else if (selectIndex == (int)ExportFormat.PDF)
            {
                filter = "PDF file (*.pdf)|*.pdf";
                var path = FileSelect.SaveAs(filter, "C:\\");
                if (path == "")
                    return;
                ExportData.RunExportToPDF(path, radGridView);
            }
            else if (selectIndex == (int)ExportFormat.CSV)
            {
                filter = "PDF file (*.pdf)|*.csv";
                var path = FileSelect.SaveAs(filter, "C:\\");
                if (path == "")
                    return;
                ExportData.RunExportToCSV(path, radGridView);
            }
        }

        private enum ExportFormat
        {
            EXCEL,
            HTML,
            PDF,
            CSV
        }
    }
}
