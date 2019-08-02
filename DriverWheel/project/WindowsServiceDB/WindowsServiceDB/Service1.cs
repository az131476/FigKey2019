using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using CommonUtils.Logger;


namespace WindowsServiceDB
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            LogHelper.Log.Info("服务启动...");
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings["sqlconstring"].ToString();
                var deleteSQL = ConfigurationManager.ConnectionStrings["deleteSQL"].ToString();
                Task task = new Task(() =>
                {
                    int row = MySqlHelper.ExecuteNonQuery(connectionString, CommandType.Text, deleteSQL);
                });
                task.Start();
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message);
            }
        }

        protected override void OnStop()
        {
            LogHelper.Log.Info("服务停止...");
        }
    }
}
