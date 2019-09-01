using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using MesManager.Control;
using MesManager.TelerikWinform.GridViewCommon.GridViewDataExport;
using Telerik.WinControls.UI.Export;
using Telerik.WinControls.UI;
using CommonUtils.Logger;
using CommonUtils.FileHelper;

namespace MesManager.UI
{
    public partial class TestLogDetail : RadForm
    {
        private string startTime;
        private string productSn;
        private string endTime;
        private MesService.MesServiceClient serviceClient;
        public TestLogDetail(string sn)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.productSn = sn;
        }

        async private void TestLogDetail_Load(object sender, EventArgs e)
        {
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1, false);
            this.radGridView1.ReadOnly = true;
            this.pickerStartTime.Text = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            this.pickerEndTime.Text = DateTime.Now.ToString("yyyy-MM-dd") + " 23:59:59";
            serviceClient = new MesService.MesServiceClient();
            var dt = (await serviceClient.SelectTestLogDataDetailAsync(productSn,"","")).Tables[0];
            this.radGridView1.DataSource = null;
            this.radGridView1.DataSource = dt;
        }

        async private void Btn_search_Click(object sender, EventArgs e)
        {
            var startTime = Convert.ToDateTime(this.pickerStartTime.Text);
            var endTime = Convert.ToDateTime(this.pickerEndTime.Text).AddDays(1);
            var dt = (await serviceClient.SelectTestLogDataDetailAsync(productSn,startTime.ToString(),endTime.ToString())).Tables[0];
            this.radGridView1.DataSource = null;
            this.radGridView1.DataSource = dt;
            this.radGridView1.Columns[0].BestFit();
        }

        private void Btn_export_Click(object sender, EventArgs e)
        {
            ExportGridViewData(0,this.radGridView1);
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
