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
using System.IO;
using Telerik.WinControls.UI.Export;
using CommonUtils.Logger;
using CommonUtils.FileHelper;
using MesManager.TelerikWinform.GridViewCommon.GridViewDataExport;

namespace MesManager.UI
{
    public partial class SNCenter : RadForm
    {
        private MesService.MesServiceClient serviceClient;
        private DataTable dataSourceMaterialBasic;
        private DataTable dataSourceMaterialDetail;
        private const string MATERIAL_PN = "物料号";
        private const string MATERIAL_LOT = "批次号";
        private const string MATERIAL_RID = "料盘号";
        private const string MATERIAL_DC = "有效期";
        private const string MATERIAL_QTY = "库存";
        private const string MATERIAL_NAME = "物料名称";
        private const string PRODUCT_TYPENO = "产品型号";
        private const string STATION_NAME = "工站名称";
        private const string USE_AMOUNTED = "使用数量";
        private const string TEAM_LEADER = "班组长";
        private const string ADMIN = "管理员";
        private const string UPDATE_DATE = "更新日期";

        private enum ExportFormat
        {
            EXCEL,
            HTML,
            PDF,
            CSV
        }

        public SNCenter()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void SNCenter_Load(object sender, EventArgs e)
        {
            Init();
            EventHandlers();
        }

        private void InitDataTable()
        {
            if (dataSourceMaterialBasic == null)
            {
                dataSourceMaterialBasic = new DataTable();
                dataSourceMaterialBasic.Columns.Add(MATERIAL_PN);
                dataSourceMaterialBasic.Columns.Add(MATERIAL_LOT);
                dataSourceMaterialBasic.Columns.Add(MATERIAL_RID);
                dataSourceMaterialBasic.Columns.Add(MATERIAL_DC);
                dataSourceMaterialBasic.Columns.Add(MATERIAL_QTY);
                dataSourceMaterialBasic.Columns.Add(PRODUCT_TYPENO);
                dataSourceMaterialBasic.Columns.Add(USE_AMOUNTED);
            }
            if (dataSourceMaterialDetail == null)
            {
                dataSourceMaterialDetail = new DataTable();
                dataSourceMaterialDetail.Columns.Add(MATERIAL_PN);
                dataSourceMaterialDetail.Columns.Add(MATERIAL_LOT);
                dataSourceMaterialDetail.Columns.Add(MATERIAL_RID);
                dataSourceMaterialDetail.Columns.Add(MATERIAL_DC);
                dataSourceMaterialDetail.Columns.Add(MATERIAL_QTY);
                dataSourceMaterialDetail.Columns.Add(PRODUCT_TYPENO);
                dataSourceMaterialDetail.Columns.Add(STATION_NAME);
                dataSourceMaterialDetail.Columns.Add(USE_AMOUNTED);
                dataSourceMaterialDetail.Columns.Add(TEAM_LEADER);
                dataSourceMaterialDetail.Columns.Add(ADMIN);
                dataSourceMaterialDetail.Columns.Add(UPDATE_DATE);
            }
        }

        private void EventHandlers()
        {
            this.menu_sn_result.Click += Menu_sn_result_Click;
            this.menu_material.Click += Menu_material_Click;
            this.menu_package.Click += Menu_package_Click;
            this.menu_quanlity.Click += Menu_quanlity_Click;
            this.menu_productCheck.Click += Menu_productCheck_Click;

            this.btn_materialSelect.Click += Btn_materialSelect_Click;
            this.btn_selectOfSn.Click += Btn_selectOfSn_Click;
            this.btn_selectOfPackage.Click += Btn_selectOfPackage_Click;

            this.tool_sn_export.Click += Tool_sn_export_Click;
            this.tool_material_export.Click += Tool_material_export_Click;
            this.tool_package_export.Click += Tool_package_export_Click;
        }

        private void Tool_package_export_Click(object sender, EventArgs e)
        {
            ExportGridViewData(this.tool_package_exportFilter.SelectedIndex, this.radGridViewPackage);
        }

        private void Tool_material_export_Click(object sender, EventArgs e)
        {
            ExportGridViewData(this.tool_material_exportFilter.SelectedIndex, this.radGridViewMaterial);
        }

        private void Tool_sn_export_Click(object sender, EventArgs e)
        {
            ExportGridViewData(this.tool_sn_exportFilter.SelectedIndex,this.radGridViewSn);
        }

        private void ExportGridViewData(int selectIndex,RadGridView radGridView)
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

        private void Btn_materialSelect_Click(object sender, EventArgs e)
        {
            SelectOfMaterial();
        }

        private void Menu_productCheck_Click(object sender, EventArgs e)
        {
            
        }

        private void Menu_quanlity_Click(object sender, EventArgs e)
        {
            this.panel_quanlity.Dock = DockStyle.Fill;
            SetPanelFalse();
            this.panel_quanlity.Visible = true;
        }

        private void Menu_package_Click(object sender, EventArgs e)
        {
            this.panel_package.Dock = DockStyle.Fill;
            SetPanelFalse();
            this.panel_package.Visible = true;
        }

        private void Menu_material_Click(object sender, EventArgs e)
        {
            this.panel_material.Dock = DockStyle.Fill;
            SetPanelFalse();
            this.panel_material.Visible = true;
        }

        private void Menu_sn_result_Click(object sender, EventArgs e)
        {
            this.panel_sn.Dock = DockStyle.Fill;
            SetPanelFalse();
            this.panel_sn.Visible = true;
        }

        private void Init()
        {
            serviceClient = new MesService.MesServiceClient();
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewSn, false);
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewPackage, false);
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewMaterial, false);
            SetPanelFalse();
            this.panel_sn.Visible = true;
            this.panel_sn.Dock = DockStyle.Fill;
            this.tool_sn_exportFilter.Items.Clear();
            this.tool_sn_exportFilter.Items.Add(ExportFormat.EXCEL);
            this.tool_sn_exportFilter.Items.Add(ExportFormat.HTML);
            this.tool_sn_exportFilter.Items.Add(ExportFormat.PDF);
            this.tool_sn_exportFilter.Items.Add(ExportFormat.CSV);
            this.tool_sn_exportFilter.SelectedIndex = 0;
            this.tool_package_exportFilter.Items.Clear();
            this.tool_package_exportFilter.Items.Add(ExportFormat.EXCEL);
            this.tool_package_exportFilter.Items.Add(ExportFormat.HTML);
            this.tool_package_exportFilter.Items.Add(ExportFormat.PDF);
            this.tool_package_exportFilter.Items.Add(ExportFormat.CSV);
            this.tool_package_exportFilter.SelectedIndex = 0;
            this.tool_material_exportFilter.Items.Clear();
            this.tool_material_exportFilter.Items.Add(ExportFormat.EXCEL);
            this.tool_material_exportFilter.Items.Add(ExportFormat.HTML);
            this.tool_material_exportFilter.Items.Add(ExportFormat.PDF);
            this.tool_material_exportFilter.Items.Add(ExportFormat.CSV);
            this.tool_material_exportFilter.SelectedIndex = 0;
        }

        private void SetPanelFalse()
        {
            this.panel_sn.Visible = false;
            this.panel_material.Visible = false;
            this.panel_package.Visible = false;
            this.panel_quanlity.Visible = false;
        }

        async private void SelectOfSn()
        {
            var filter = tb_sn.Text;
            DataSet dt = await serviceClient.SelectTestResultUpperAsync(filter, filter, filter, true);
            radGridViewSn.DataSource = dt.Tables[0];
        }

        async private void SelectOfPackage()
        {
            var filter = tb_package.Text;
            //箱子编码/追溯码/型号
            MesService.PackageProduct packageProduct = new MesService.PackageProduct();
            packageProduct.BindingState = 1;
            packageProduct.CaseCode = tb_package.Text;
            packageProduct.TypeNo = tb_package.Text;
            packageProduct.SnOutter = tb_package.Text;
            System.Data.DataTable dt = (await serviceClient.SelectPackageProductAsync(packageProduct)).Tables[0];
            this.radGridViewPackage.DataSource = dt;
        }

        /// <summary>
        /// 根据物料编码查询物料被使用于哪些产品
        /// </summary>
        async private void SelectOfMaterial()
        {
            //物料信息表
            //物料编码+物料名称+所属型号+在哪个工序/站位消耗+该位置消耗数量
            var dt = (await serviceClient.SelectMaterialBasicMsgAsync(this.tb_material.Text)).Tables[0];
            this.radGridViewMaterial.DataSource = dt;
        }

        private void Btn_selectOfSn_Click(object sender, EventArgs e)
        {
            SelectOfSn();
        }

        private void Btn_selectOfPackage_Click(object sender, EventArgs e)
        {
            SelectOfPackage();MessageBox.Show("","",MessageBoxButtons.OK);
        }
    }
}
