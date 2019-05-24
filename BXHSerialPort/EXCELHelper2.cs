using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace BXHSerialPort
{
    class EXCELHelper2
    {
        public string  excelRead(string filename)
        {
            Excel.Application excelApp = new Excel.Application();
            if (excelApp == null)
            {
                // if equal null means EXCEL is not installed.  
                MessageBox.Show("Excel is not properly installed!");
                return "Excel is not properly installed!";
            }

            // open a workbook,if not exist, create a new one  
            Excel.Workbook workBook;
            if (File.Exists(filename))
            {
                workBook = excelApp.Workbooks.Open(filename, 0, false, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            }
            else
            {
                workBook = excelApp.Workbooks.Add(true);
            }
            //new a worksheet  
            Excel.Worksheet workSheet = workBook.ActiveSheet as Excel.Worksheet;

            //write data  
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);//获得第i个sheet，准备写入  
            //workSheet.Cells[1, 3] = "(1,3)Content";
            string s = "";
            string sAddr = "";
            for (int i = 0; i < 23; i++)
            {
                if (i==20)
                {
                    s += "@";
                    continue;
                }
                sAddr+= ((Excel.Range)workSheet.Cells[i + 4, 2]).Text + "\t";
                for (int j = 0; j < 8; j++)
                {
                    s+=((Excel.Range)workSheet.Cells[i+4, j*2+3]).Text+"\t\t";
                }
                s += "\r\n";
            }
            
            //set visible the Excel will run in background  
            excelApp.Visible = false;
            //set false the alerts will not display  
            excelApp.DisplayAlerts = false;

            //workBook.SaveAs(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);  
            //workBook.SaveAs(filename);
            workBook.Close(false, Missing.Value, Missing.Value);
            //quit and clean up objects  
            excelApp.Quit();
            workSheet = null;
            workBook = null;
            excelApp = null;
            GC.Collect();
            return s+"@"+sAddr;
        }
        public void excelWrite(string filename,string[][] dirString)
        {
            Excel.Application excelApp = new Excel.Application();
            if (excelApp == null)
            {
                // if equal null means EXCEL is not installed.  
                MessageBox.Show("Excel is not properly installed!");
                return;
            }

            // open a workbook,if not exist, create a new one  
            Excel.Workbook workBook;
            if (File.Exists(filename))
            {
                workBook = excelApp.Workbooks.Open(filename, 0, false, 5, "", "", true, Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            }
            else
            {
                workBook = excelApp.Workbooks.Add(true);
            }
            //new a worksheet  
            Excel.Worksheet workSheet = workBook.ActiveSheet as Excel.Worksheet;

            //write data  
            workSheet = (Excel.Worksheet)workBook.Worksheets.get_Item(1);//获得第i个sheet，准备写入 
            //Excel.Worksheet workSheet = workBook.Worksheets[1];
            //Excel.Worksheet workSheet = workBook.Sheets.Add();
                
            for (int i = 0; i < dirString.Length; i++)
            {
                for (int j = 0; j < dirString[i].Length; j++)
                {
                    workSheet.Cells[i+1, j+1] = dirString[i][j];
                }
            }
            //workSheet.Cells[1, 3] = "(1,3)Content";
            //set visible the Excel will run in background  
            excelApp.Visible = false;
            //set false the alerts will not display  
            excelApp.DisplayAlerts = false;

            workBook.SaveAs(filename, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Excel.XlSaveAsAccessMode.xlNoChange, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);  
            //workBook.SaveAs(filename);
            workBook.Close(false, Missing.Value, Missing.Value);
            //quit and clean up objects  
            excelApp.Quit();
            workSheet = null;
            workBook = null;
            excelApp = null;
            GC.Collect();
        }
    }
}
