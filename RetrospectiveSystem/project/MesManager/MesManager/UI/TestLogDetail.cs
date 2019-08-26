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
        private string productTypeNo;
        private string productSn;
        private string stationName;
        private MesService.MesServiceClient serviceClient;
        public TestLogDetail(string typeNo,string sn,string stationName)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.productTypeNo = typeNo;
            this.productSn = sn;
            this.stationName = stationName;
        }

        async private void TestLogDetail_Load(object sender, EventArgs e)
        {
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1, false);
            this.radGridView1.ReadOnly = true;
            serviceClient = new MesService.MesServiceClient();
            var dt = (await serviceClient.SelectTestLogDataDetailAsync(productSn,"","")).Tables[0];
            if (dt.Rows.Count < 1)
                return;
            this.radGridView1.DataSource = null;
            this.radGridView1.DataSource = dt;
        }

        async private void Btn_search_Click(object sender, EventArgs e)
        {
            var startTime = Convert.ToDateTime(this.radDateTimePicker1.Text);
            var endTime = Convert.ToDateTime(this.radDateTimePicker2.Text).AddDays(1);
            var dt = (await serviceClient.SelectTestLogDataDetailAsync(productSn,startTime.ToString(),endTime.ToString())).Tables[0];
            if (dt.Rows.Count < 1)
                return;
            this.radGridView1.DataSource = null;
            this.radGridView1.DataSource = dt;
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
