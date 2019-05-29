using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Telerik.Data;
//using Telerik.QuickStart.WinControls;
using Telerik.WinControls.UI;
using Telerik.WinControls;
using Telerik.WinControls.UI.Export;

namespace SentConfigurator.Controls
{
    class Export
    {
        //private void RunExportToExcelML(string fileName, ref bool openExportFile,RadGridView radGridView1)
        //{
        //    ExportToExcelML excelExporter = new ExportToExcelML(radGridView1);

        //    if (this.radTextBoxSheet.Text != String.Empty)
        //    {
        //        excelExporter.SheetName = this.radTextBoxSheet.Text;

        //    }

        //    switch (this.radComboBoxSummaries.SelectedIndex)
        //    {
        //        case 0:
        //            excelExporter.SummariesExportOption = SummariesOption.ExportAll;
        //            break;
        //        case 1:
        //            excelExporter.SummariesExportOption = SummariesOption.ExportOnlyTop;
        //            break;
        //        case 2:
        //            excelExporter.SummariesExportOption = SummariesOption.ExportOnlyBottom;
        //            break;
        //        case 3:
        //            excelExporter.SummariesExportOption = SummariesOption.DoNotExport;
        //            break;
        //    }

        //    //set max sheet rows
        //    if (this.radRadioButton1.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
        //    {
        //        excelExporter.SheetMaxRows = ExcelMaxRows._1048576;
        //    }
        //    else if (this.radRadioButton2.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
        //    {
        //        excelExporter.SheetMaxRows = ExcelMaxRows._65536;
        //    }

        //    //set exporting visual settings
        //    excelExporter.ExportVisualSettings = this.exportVisualSettings;

        //    try
        //    {
        //        excelExporter.RunExport(fileName);

        //        RadMessageBox.SetThemeName(this.radGridView1.ThemeName);
        //        DialogResult dr = RadMessageBox.Show("The data in the grid was exported successfully. Do you want to open the file?",
        //            "Export to Excel", MessageBoxButtons.YesNo, RadMessageIcon.Question);
        //        if (dr == DialogResult.Yes)
        //        {
        //            openExportFile = true;
        //        }
        //    }
        //    catch (IOException ex)
        //    {
        //        RadMessageBox.SetThemeName(this.radGridView1.ThemeName);
        //        RadMessageBox.Show(this, ex.Message, "I/O Error", MessageBoxButtons.OK, RadMessageIcon.Error);
        //    }
        //}
    }
}
