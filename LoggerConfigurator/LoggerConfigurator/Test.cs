using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtils.DBHelper;
using System.Configuration;
using MySql.Data.MySqlClient;
using CommonUtils.Logger;

namespace LoggerConfigurator
{
    public partial class Test : Form
    {
        public Test()
        {
            InitializeComponent();
        }

        private void Test_Load(object sender, EventArgs e)
        {
            //string sql = "SELECT * from t_user";
            string sql = ConfigurationManager.AppSettings["sql"].ToString();

            dataGridView1.DataSource = Query(sql,50).Tables["ds"];
        }

        public static DataSet Query(string SQLString, int outTime)
        {
            string constr = ConfigurationManager.ConnectionStrings["localdb"].ToString();
            using (MySqlConnection connection = new MySqlConnection(constr))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    MySqlDataAdapter command = new MySqlDataAdapter(SQLString, connection);
                    command.SelectCommand.CommandTimeout = outTime;
                    command.Fill(ds, "ds");
                }
                catch (MySqlException ex)
                {
                    LogHelper.Log.Error("查询数据失败！" + ex.Message + ex.StackTrace);
                }
                if (ds.Tables["ds"] != null)
                {
                    int cellCount = ds.Tables["ds"].Columns.Count;
                    for (int i = 0; i < cellCount; i++)
                    {
                        LogHelper.Log.Info("【信息】字段数量：" + cellCount + " 字段名称：" + ds.Tables[0].Columns[i].ToString());
                        LogHelper.Log.Info("【数量：】" + ds.Tables["ds"].Rows.Count);
                    }
                }
                return ds;
            }
        }
    }
}
