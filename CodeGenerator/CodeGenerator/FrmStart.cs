using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

/// <summary>
/// 代码生成器，目前支持SQL Server，后续增加MYSQL、ORACLE等
/// </summary>
namespace CodeGenerator
{
    public partial class FrmStart : Form
    {
        public FrmStart()
        {
            InitializeComponent();
            this.Init();
        }

        #region 初始化
        public void Init()
        {
            try
            {
                string file = Application.ExecutablePath;
                file = Path.GetDirectoryName(file) + "/config.txt";
                if (File.Exists(file))
                {
                    GlobalConfig.Item = ConfigFileUtil<ConfigItem>.GetFromFile(file);
                }
                this.txtServer.Text = GlobalConfig.Item.Server;
                this.txtUid.Text = GlobalConfig.Item.UID;
                this.txtPwd.Text = GlobalConfig.Item.PWD;
                this.cbDataBase.Text = GlobalConfig.Item.DataBase;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 刷新配置
        private void RefreshConfig()
        {
            GlobalConfig.Item.Server = this.txtServer.Text;
            GlobalConfig.Item.UID = this.txtUid.Text;
            GlobalConfig.Item.PWD = this.txtPwd.Text;
            GlobalConfig.Item.DataBase = this.cbDataBase.Text;
        }
        #endregion
        private void btnNext_Click(object sender, EventArgs e)
        {
            this.RefreshConfig();
            Thread t = new Thread(new ThreadStart(DoTask));
            t.Start();
            this.Close();
        }

        public void DoTask()
        {
            FrmConfig fc = new FrmConfig();
            Application.Run(fc);
        }

        private void cbDataBase_Enter(object sender, EventArgs e)
        {
            try
            {
                this.RefreshConfig();
                string constr = "Server={0};Database={1};uid={2};pwd={3}";
                constr = String.Format(constr, GlobalConfig.Item.Server, "master", GlobalConfig.Item.UID, GlobalConfig.Item.PWD);
                SqlConnection con = new SqlConnection(constr);
                SqlDataAdapter adapter = new SqlDataAdapter("sp_helpdb", con);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                this.cbDataBase.DataSource = ds.Tables[0];
                this.cbDataBase.DisplayMember = "name";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTestConnect_Click(object sender, EventArgs e)
        {
            this.RefreshConfig();
            this.Connect();
        }

        /// <summary>
        /// 测试连接的方法
        /// </summary>
        private void Connect()
        {
            try
            {
                string constr = "Server={0};Database={1};uid={2};pwd={3}";
                constr = String.Format(constr, GlobalConfig.Item.Server, GlobalConfig.Item.DataBase, GlobalConfig.Item.UID, GlobalConfig.Item.PWD);
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                bool flag = false;
                if (con.State == ConnectionState.Open)
                {
                    flag = true;
                }
                con.Close();
                if (flag)
                {
                    MessageBox.Show("测试连接成功!");
                    string file = Application.ExecutablePath;
                    file = Path.GetDirectoryName(file) + "/config.txt";
                    if (File.Exists(file)) File.Delete(file);
                    ConfigFileUtil<ConfigItem>.SaveToFile(GlobalConfig.Item, file);
                    this.btnNext.Enabled = true;
                }
                else
                {
                    MessageBox.Show("测试连接失败!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}