using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace BXHSerialPort
{
    class EXCELHelper
    {
        //把EXCEL文件当做一个数据源来进行数据的读取操作
        public DataSet ExcelToDS(string Path)
        {
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [sheet1$]";
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");
            return ds;
        }
        public string getTableName(string Path)
        {
            //string strConn = "Provider=Microsoft.Jet.OLEDB.12.0;" + "Data Source=" + Path + ";" + "Extended Properties='Excel 12.0; HDR=Yes; IMEX=1'";
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + Path + ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1;'";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable schemaTable = conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Tables, null);
            string tableName = schemaTable.Rows[0][2].ToString().Trim();
            return tableName;
        }
        public void DSToExcel(string Path, DataSet oldds)
        {
            //先得到汇总EXCEL的DataSet 主要目的是获得EXCEL在DataSet中的结构   
            string strCon = " Provider = Microsoft.Jet.OLEDB.4.0 ; Data Source =" + Path + ";Extended Properties=Excel 8.0; HDR=Yes; IMEX=1'";
            OleDbConnection myConn = new OleDbConnection(strCon);
            string strCom = "select * from [Sheet1$]";
            myConn.Open();
            OleDbDataAdapter myCommand = new OleDbDataAdapter(strCom, myConn);
            System.Data.OleDb.OleDbCommandBuilder builder = new OleDbCommandBuilder(myCommand);
            //QuotePrefix和QuoteSuffix主要是对builder生成InsertComment命令时使用。   
            builder.QuotePrefix = "[";     //获取insert语句中保留字符（起始位置）   
            builder.QuoteSuffix = "]"; //获取insert语句中保留字符（结束位置）   
            DataSet newds = new DataSet();
            myCommand.Fill(newds, "Table1");
            for (int i = 0; i < oldds.Tables[0].Rows.Count; i++)
            {
                //在这里不能使用ImportRow方法将一行导入到news中，因为ImportRow将保留原来DataRow的所有设置(DataRowState状态不变)。  
                //在使用ImportRow后newds内有值，但不能更新到Excel中因为所有导入行的DataRowState != Added
                DataRow nrow = newds.Tables["Table1"].NewRow();
                for (int j = 0; j < newds.Tables[0].Columns.Count; j++)
                {
                    nrow[j] = oldds.Tables[0].Rows[i][j];
                }
                newds.Tables["Table1"].Rows.Add(nrow);
            }
            myCommand.Update(newds, "Table1");
            myConn.Close();
        }
    }
}
