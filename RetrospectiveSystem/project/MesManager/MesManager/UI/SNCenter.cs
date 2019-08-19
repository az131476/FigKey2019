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
using Microsoft.Office.Interop.Excel;
using CommonUtils.Logger;
using CommonUtils.FileHelper;

namespace MesManager.UI
{
    public partial class SNCenter : RadForm
    {
        private MesService.MesServiceClient serviceClient;

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
                RunExportToExcelML(path, radGridView);
            }
            else if (selectIndex == (int)ExportFormat.HTML)
            {
                filter = "Html File (*.htm)|*.htm";
                var path = FileSelect.SaveAs(filter, "C:\\");
                if (path == "")
                    return;
                RunExportToHTML(path, radGridView);
            }
            else if (selectIndex == (int)ExportFormat.PDF)
            {
                filter = "PDF file (*.pdf)|*.pdf";
                var path = FileSelect.SaveAs(filter, "C:\\");
                if (path == "")
                    return;
                RunExportToPDF(path, radGridView);
            }
            else if (selectIndex == (int)ExportFormat.CSV)
            {
                filter = "PDF file (*.pdf)|*.csv";
                var path = FileSelect.SaveAs(filter, "C:\\");
                if (path == "")
                    return;
                RunExportToCSV(path, radGridView);
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
            this.tool_sn_exportFilter.Items.Clear();
            this.tool_sn_exportFilter.Items.Add(ExportFormat.EXCEL);
            this.tool_sn_exportFilter.Items.Add(ExportFormat.HTML);
            this.tool_sn_exportFilter.Items.Add(ExportFormat.PDF);
            this.tool_sn_exportFilter.Items.Add(ExportFormat.CSV);
            this.tool_package_exportFilter.Items.Clear();
            this.tool_package_exportFilter.Items.Add(ExportFormat.EXCEL);
            this.tool_package_exportFilter.Items.Add(ExportFormat.HTML);
            this.tool_package_exportFilter.Items.Add(ExportFormat.PDF);
            this.tool_package_exportFilter.Items.Add(ExportFormat.CSV);
            this.tool_material_exportFilter.Items.Clear();
            this.tool_material_exportFilter.Items.Add(ExportFormat.EXCEL);
            this.tool_material_exportFilter.Items.Add(ExportFormat.HTML);
            this.tool_material_exportFilter.Items.Add(ExportFormat.PDF);
            this.tool_material_exportFilter.Items.Add(ExportFormat.CSV);
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
            var dt = (await serviceClient.SelectMaterialUserProductAsync(this.tb_material.Text)).Tables[0];
            this.radGridViewMaterial.DataSource = dt;
        }

        private void Btn_selectOfSn_Click(object sender, EventArgs e)
        {
            SelectOfSn();
        }

        private void Btn_selectOfPackage_Click(object sender, EventArgs e)
        {
            SelectOfPackage();
        }

        private void RunExportToExcelML(string fileName,RadGridView radGridView)
        {
            ExportToExcelML excelExporter = new ExportToExcelML(radGridView);
            excelExporter.SummariesExportOption = SummariesOption.ExportAll;

            //set export settings
            //excelExporter.ExportVisualSettings = this.radCheckBoxExportVisual.IsChecked;
            //excelExporter.ExportHierarchy = this.radCheckBoxExportHierarchy.IsChecked;
            excelExporter.HiddenColumnOption = HiddenOption.DoNotExport;
            try
            {
                this.Cursor = Cursors.WaitCursor;

                excelExporter.RunExport(fileName);

                RadMessageBox.SetThemeName(radGridView.ThemeName);
                DialogResult dr = RadMessageBox.Show("The data in the grid was exported successfully. Do you want to open the file?",
                    "Export to Excel", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(fileName);
                    }
                    catch (Exception ex)
                    {
                        string message = String.Format("The file cannot be opened on your system.\nError message: {0}", ex.Message);
                        RadMessageBox.Show(message, "Open File", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (IOException ex)
            {
                RadMessageBox.SetThemeName(radGridView.ThemeName);
                RadMessageBox.Show(this, ex.Message, "I/O Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void RunExportToCSV(string fileName,RadGridView radGridView)
        {
            ExportToCSV csvExporter = new ExportToCSV(radGridView);
            csvExporter.CSVCellFormatting += csvExporter_CSVCellFormatting;
            csvExporter.SummariesExportOption = SummariesOption.ExportAll;

            //set export settings
            //csvExporter.ExportHierarchy = this.radCheckBoxExportHierarchy.IsChecked;
            csvExporter.HiddenColumnOption = HiddenOption.DoNotExport;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                csvExporter.RunExport(fileName);

                RadMessageBox.SetThemeName(radGridView.ThemeName);
                DialogResult dr = RadMessageBox.Show("The data in the grid was exported successfully. Do you want to open the file?",
                    "Export to CSV", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(fileName);
                    }
                    catch (Exception ex)
                    {
                        string message = String.Format("The file cannot be opened on your system.\nError message: {0}", ex.Message);
                        RadMessageBox.Show(message, "Open File", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (IOException ex)
            {
                RadMessageBox.SetThemeName(radGridView.ThemeName);
                RadMessageBox.Show(this, ex.Message, "I/O Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        void csvExporter_CSVCellFormatting(object sender, Telerik.WinControls.UI.Export.CSV.CSVCellFormattingEventArgs e)
        {
            if (e.GridCellInfo.ColumnInfo is GridViewDateTimeColumn)
            {
                e.CSVCellElement.Value = this.FormatDate(e.CSVCellElement.Value);
            }
        }

        private string FormatDate(object value)
        {
            DateTime date;
            if (DateTime.TryParse(value.ToString(), out date))
            {
                return date.ToString("d MMM yyyy");
            }

            return value.ToString();
        }

        private void RunExportToHTML(string fileName,RadGridView radGridView)
        {
            ExportToHTML htmlExporter = new ExportToHTML(radGridView);
            htmlExporter.HTMLCellFormatting += htmlExporter_HTMLCellFormatting;

            htmlExporter.SummariesExportOption = SummariesOption.ExportAll;

            //set export settings
            //htmlExporter.ExportVisualSettings = this.radCheckBoxExportVisual.IsChecked;
            //htmlExporter.ExportHierarchy = this.radCheckBoxExportHierarchy.IsChecked;
            htmlExporter.HiddenColumnOption = HiddenOption.DoNotExport;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                htmlExporter.RunExport(fileName);

                RadMessageBox.SetThemeName(radGridView.ThemeName);
                DialogResult dr = RadMessageBox.Show("The data in the grid was exported successfully. Do you want to open the file?",
                    "Export to HTML", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(fileName);
                    }
                    catch (Exception ex)
                    {
                        string message = String.Format("The file cannot be opened on your system.\nError message: {0}", ex.Message);
                        RadMessageBox.Show(message, "Open File", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }
            }
            catch (IOException ex)
            {
                RadMessageBox.SetThemeName(radGridView.ThemeName);
                RadMessageBox.Show(this, ex.Message, "I/O Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        void htmlExporter_HTMLCellFormatting(object sender, Telerik.WinControls.UI.Export.HTML.HTMLCellFormattingEventArgs e)
        {
            if (e.GridCellInfo.ColumnInfo is GridViewDateTimeColumn)
            {
                e.HTMLCellElement.Value = this.FormatDate(e.HTMLCellElement.Value);
            }
        }

        private void RunExportToPDF(string fileName,RadGridView radGridView)
        {
            ExportToPDF pdfExporter = new ExportToPDF(radGridView);
            pdfExporter.PdfExportSettings.Title = "My PDF Title";
            pdfExporter.PdfExportSettings.PageWidth = 297;
            pdfExporter.PdfExportSettings.PageHeight = 210;
            pdfExporter.FitToPageWidth = true;
            pdfExporter.HTMLCellFormatting += pdfExporter_HTMLCellFormatting;

            pdfExporter.SummariesExportOption = SummariesOption.ExportAll;

            //set export settings
            //pdfExporter.ExportVisualSettings = this.radCheckBoxExportVisual.IsChecked;
            //pdfExporter.ExportHierarchy = this.radCheckBoxExportHierarchy.IsChecked;
            pdfExporter.HiddenColumnOption = HiddenOption.DoNotExport;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                pdfExporter.RunExport(fileName);

                RadMessageBox.SetThemeName(radGridView.ThemeName);
                DialogResult dr = RadMessageBox.Show("The data in the grid was exported successfully. Do you want to open the file?",
                    "Export to PDF", MessageBoxButtons.YesNo, RadMessageIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    try
                    {
                        System.Diagnostics.Process.Start(fileName);
                    }
                    catch (Exception ex)
                    {
                        string message = String.Format("The file cannot be opened on your system.\nError message: {0}", ex.Message);
                        RadMessageBox.Show(message, "Open File", MessageBoxButtons.OK, RadMessageIcon.Error);
                    }
                }

            }
            catch (IOException ex)
            {
                RadMessageBox.SetThemeName(radGridView.ThemeName);
                RadMessageBox.Show(this, ex.Message, "I/O Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        void pdfExporter_HTMLCellFormatting(object sender, Telerik.WinControls.UI.Export.HTML.HTMLCellFormattingEventArgs e)
        {
            if (e.GridCellInfo.ColumnInfo is GridViewDateTimeColumn)
            {
                e.HTMLCellElement.Value = this.FormatDate(e.HTMLCellElement.Value);
            }
        }
    }
}
