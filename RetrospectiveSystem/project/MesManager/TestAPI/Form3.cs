using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using MySql.Data.MySqlClient;
using CommonUtils.Logger;
using System.Data;

namespace TestAPI
{
    public partial class Form3 : Form
    {
        private MesServiceT.MesServiceClient serviceClient;
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            MesServiceTest.MesServiceClient mesServiceClient = new MesServiceTest.MesServiceClient();
            //serviceClient = new MesServiceT.MesServiceClient();
            //var res = mesServiceClient.UpdateTestResultData("1111111111","A01", "烧录工站", "fail","","");
            //var res = mesServiceClient.SelectLastTestResult("1111111111", "灵敏度测试工站");
            var pcbaSN = "017 B19823003801";
            var stationName = "外壳装配工站";
            var materialcode = "A19083000080&S2.118&1.2.12.159&50&20190830&1T20190830001";
            mesServiceClient.SelectLastTestResult(pcbaSN,stationName);
            mesServiceClient.UpdateMaterialStatistics("A01",stationName,materialcode,"3","","");
            System.Threading.Thread.Sleep(10000);
            mesServiceClient.UpdateMaterialStatistics("A01", stationName, materialcode, "4", "", "");
        }

        public void LSOSQL()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["sqlconstring"].ToString();
                var selectSQL = ConfigurationManager.ConnectionStrings["selectPrescNo"].ToString();
                Task task = new Task(() =>
                {
                    while (true)
                    {
                        int index = 0;
                        //LogHelper.Log.Info("开始执行...");
                        MySqlDataReader mySqlDataReader = MySqlHelper.ExecuteReader(connectionString, CommandType.Text, selectSQL);
                        while (mySqlDataReader.Read())
                        {
                            var prescNo = mySqlDataReader["PrescriptionNo"].ToString();
                            //LogHelper.Log.Info("prescNo:" + prescNo);
                            //执行删除
                            var deletePrescList = $"delete from prescriptionlist where PrescriptionNo='{prescNo}'";
                            var deletePrescDetail = $"delete from prescriptiondetail where PrescriptionNo = '{prescNo}'";
                            MySqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, deletePrescList);
                            MySqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, deletePrescDetail);
                            index++;
                        }
                        //int row = MySqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, selectSQL);

                    }
                });
                task.Start();
            }
            catch (Exception ex)
            {
                //LogHelper.Log.Error("异常：" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            var materialCode = this.textBox1.Text;
            //this.textBox2.Text = serviceClient.CheckMaterialMatch("A02","1.2.11.111","1.2.11.111",
              //  "A19083100006&S2.118&1.2.11.111&20&20190831&1T20190831001");
            this.textBox2.Text = serviceClient.CheckMaterialUseState("A02", "A19083100006&S2.118&1.2.11.111&20&20190831&1T20190831001");
        }
    }
}
