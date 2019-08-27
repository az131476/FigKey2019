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
using System.Threading;

namespace MesManager.UI
{
    public partial class SNCenter : RadForm
    {
        private MesService.MesServiceClient serviceClient;
        private DataTable dataSourceMaterialBasic;
        private DataTable dataSourceProductCheck;
        #region 物料统计字段
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
        #endregion

        #region 成品抽检字段
        private const string CHECK_ORDER = "序号";
        private const string CHECK_SN = "产品SN";
        private const string CHECK_CASE_CODE = "箱子编码";
        private const string CHECK_TYPE_NO = "产品型号";
        private const string CHECK_BINDING_DATE = "修改日期";
        private const string CHECK_BINDING_STATE = "产品状态";
        private const string CHECK_REMARK = "描述";
        private const string CHECK_LEADER = "班组长";
        private const string CHECK_ADMIN = "管理员";
        #endregion

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
            SelectOfSn();
            EventHandlers();
        }

        private void EventHandlers()
        {
            this.menu_sn_result.Click += Menu_sn_result_Click;
            this.menu_material.Click += Menu_material_Click;
            this.menu_package.Click += Menu_package_Click;
            this.menu_productCheck.Click += Menu_productCheck_Click;

            this.btn_materialSelect.Click += Btn_materialSelect_Click;
            this.btn_selectOfSn.Click += Btn_selectOfSn_Click;
            this.btn_selectOfPackage.Click += Btn_selectOfPackage_Click;
            this.btn_productCheck.Click += Btn_productCheck_Click;

            this.tool_sn_export.Click += Tool_sn_export_Click;
            this.tool_material_export.Click += Tool_material_export_Click;
            this.tool_package_export.Click += Tool_package_export_Click;
            this.tool_productCheck_export.Click += Tool_productCheck_export_Click;

            this.radGridViewMaterial.CellDoubleClick += RadGridViewMaterial_CellDoubleClick;
        }

        private void Btn_productCheck_Click(object sender, EventArgs e)
        {
            SelectOfPackageCheck("0");
        }

        private void RadGridViewMaterial_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            var ridCode = this.radGridViewMaterial.CurrentRow.Cells[2].Value.ToString();
            MaterialDetailMsg materialDetailMsg = new MaterialDetailMsg(ridCode);
            materialDetailMsg.ShowDialog();
        }

        private void Tool_package_export_Click(object sender, EventArgs e)
        {
            ExportGridViewData(this.tool_package_exportFilter.SelectedIndex, this.radGridViewPackage);
        }

        private void Tool_productCheck_export_Click(object sender, EventArgs e)
        {
            ExportGridViewData(this.tool_productCheck_exportFilter.SelectedIndex, this.radGridViewCheck);
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
            this.panel_productCheck.Dock = DockStyle.Fill;
            SetPanelFalse();
            this.panel_productCheck.Visible = true;
            SelectOfPackageCheck("0");
        }

        private void Menu_package_Click(object sender, EventArgs e)
        {
            this.panel_package.Dock = DockStyle.Fill;
            SetPanelFalse();
            this.panel_package.Visible = true;
            SelectOfPackage("1");
        }

        private void Menu_material_Click(object sender, EventArgs e)
        {
            this.panel_material.Dock = DockStyle.Fill;
            SetPanelFalse();
            this.panel_material.Visible = true;
            SelectOfMaterial();
        }

        private void Menu_sn_result_Click(object sender, EventArgs e)
        {
            this.panel_sn.Dock = DockStyle.Fill;
            SetPanelFalse();
            this.panel_sn.Visible = true;
            SelectOfSn();
        }

        private void Init()
        {
            serviceClient = new MesService.MesServiceClient();
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewSn, false);
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewPackage, false);
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewMaterial, false);
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewCheck,false);
            this.radGridViewSn.ReadOnly = true;
            this.radGridViewPackage.ReadOnly = true;
            this.radGridViewMaterial.ReadOnly = true;
            this.radGridViewCheck.ReadOnly = true;
            SetPanelFalse();
            InitDataTable();
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
            this.tool_productCheck_exportFilter.Items.Clear();
            this.tool_productCheck_exportFilter.Items.Add(ExportFormat.EXCEL);
            this.tool_productCheck_exportFilter.Items.Add(ExportFormat.HTML);
            this.tool_productCheck_exportFilter.Items.Add(ExportFormat.PDF);
            this.tool_productCheck_exportFilter.Items.Add(ExportFormat.CSV);
            this.tool_productCheck_exportFilter.SelectedIndex = 0;
        }

        private void SetPanelFalse()
        {
            this.panel_sn.Visible = false;
            this.panel_material.Visible = false;
            this.panel_package.Visible = false;
            this.panel_productCheck.Visible = false;
        }

        async private void SelectOfSn()
        {
            var filter = tb_sn.Text;
            DataTable dt = (await serviceClient.SelectTestResultUpperAsync(filter, filter, filter, true)).Tables[0];
            if (dt.Rows.Count < 1)
                return;
            radGridViewSn.DataSource = dt;
        }

        async private void SelectOfPackage(string state)
        {
            var filter = tb_package.Text;
            //箱子编码/追溯码/型号
            System.Data.DataTable dt = (await serviceClient.SelectPackageProductAsync(filter,state)).Tables[0];
            if (dt.Rows.Count < 1)
                return;
            this.radGridViewPackage.DataSource = dt;
        }

        async private void SelectOfPackageCheck(string state)
        {
            var filter = tb_productCheck.Text;
            //箱子编码/追溯码/型号
            DataTable dt = (await serviceClient.SelectPackageProductAsync(filter, state)).Tables[0];
            if (dt.Rows.Count < 1)
                return;
            this.dataSourceProductCheck.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var orderID = i + 1;
                var caseCode = dt.Rows[i][0].ToString();
                var sn = dt.Rows[i][1].ToString();
                var typeNo = dt.Rows[i][2].ToString();
                var bindingDate = dt.Rows[i][3].ToString();
                var teamLeader = dt.Rows[i][4].ToString();
                var admin = dt.Rows[i][5].ToString();
                var remark = dt.Rows[i][6].ToString();
                var productState = "已解包";
                DataRow dr = dataSourceProductCheck.NewRow();
                dr[CHECK_ORDER] = orderID;
                dr[CHECK_CASE_CODE] = caseCode;
                dr[CHECK_SN] = sn;
                dr[CHECK_TYPE_NO] = typeNo;
                dr[CHECK_BINDING_DATE] = bindingDate;
                dr[CHECK_LEADER] = teamLeader;
                dr[CHECK_ADMIN] = admin;
                dr[CHECK_REMARK] = remark;
                dr[CHECK_BINDING_STATE] = productState;
                dataSourceProductCheck.Rows.Add(dr);
            }
            this.radGridViewCheck.DataSource = dataSourceProductCheck;
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
                dataSourceMaterialBasic.Columns.Add(MATERIAL_NAME);
                dataSourceMaterialBasic.Columns.Add(PRODUCT_TYPENO);
                dataSourceMaterialBasic.Columns.Add(USE_AMOUNTED);
            }
            if (dataSourceProductCheck == null)
            {
                dataSourceProductCheck = new DataTable();
                dataSourceProductCheck.Columns.Add(CHECK_ORDER);
                dataSourceProductCheck.Columns.Add(CHECK_CASE_CODE);
                dataSourceProductCheck.Columns.Add(CHECK_SN);
                dataSourceProductCheck.Columns.Add(CHECK_TYPE_NO);
                dataSourceProductCheck.Columns.Add(CHECK_BINDING_STATE);
                dataSourceProductCheck.Columns.Add(CHECK_REMARK);
                dataSourceProductCheck.Columns.Add(CHECK_LEADER);
                dataSourceProductCheck.Columns.Add(CHECK_ADMIN);
                dataSourceProductCheck.Columns.Add(CHECK_BINDING_DATE);
            }
        }

        /// <summary>
        /// 根据物料编码查询物料被使用于哪些产品
        /// </summary>
        async private void SelectOfMaterial()
        {
            //物料信息表
            //物料编码+物料名称+所属型号+在哪个工序/站位消耗+该位置消耗数量
            this.dataSourceMaterialBasic.Clear();
            var dt = (await serviceClient.SelectMaterialBasicMsgAsync(this.tb_material.Text)).Tables[0];
            if (dt.Rows.Count < 1)
                return;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dataSourceMaterialBasic.NewRow();
                var materialCode = dt.Rows[i][0].ToString();//pn/lot/rid/dc/qty
                var materialName = dt.Rows[i][1].ToString();
                var productTypeNo = dt.Rows[i][2].ToString();
                var useAmounted = dt.Rows[i][3].ToString();
                var pnCode = materialCode.Substring(0,materialCode.IndexOf('@'));
                materialCode = materialCode.Substring(materialCode.IndexOf('@')+1);
                var lotCode = materialCode.Substring(0,materialCode.IndexOf('@'));
                materialCode = materialCode.Substring(materialCode.IndexOf('@')+1);
                var ridCode = materialCode.Substring(0,materialCode.IndexOf('@'));
                materialCode = materialCode.Substring(materialCode.IndexOf('@')+1);
                var dcCode = materialCode.Substring(0,materialCode.IndexOf('@'));
                materialCode = materialCode.Substring(materialCode.IndexOf('@')+1);
                var qtyCode = materialCode;
                dr[MATERIAL_PN] = pnCode;
                dr[MATERIAL_LOT] = lotCode;
                dr[MATERIAL_RID] = ridCode;
                dr[MATERIAL_DC] = dcCode;
                dr[MATERIAL_QTY] = qtyCode;
                dr[MATERIAL_NAME] = materialName;
                dr[PRODUCT_TYPENO] = productTypeNo;
                dr[USE_AMOUNTED] = useAmounted;
                dataSourceMaterialBasic.Rows.Add(dr);
            }
            this.radGridViewMaterial.DataSource = dataSourceMaterialBasic;
        }

        private void Btn_selectOfSn_Click(object sender, EventArgs e)
        {
            SelectOfSn();
        }

        private void Btn_selectOfPackage_Click(object sender, EventArgs e)
        {
            SelectOfPackage("1");
        }
    }
}
