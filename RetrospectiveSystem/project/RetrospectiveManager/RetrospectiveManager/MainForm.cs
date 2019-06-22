using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CommonUtils.Logger;
using System.Web;
using System.IO;
using System.Net;

namespace RetrospectiveManager
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        public MainForm()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            menu_set_station.Click += Menu_set_station_Click;
            menu_produce_config.Click += Menu_produce_config_Click;

        }

        private void Menu_produce_config_Click(object sender, EventArgs e)
        {
            SetProduce setProduce = new SetProduce();
            setProduce.StartPosition = FormStartPosition.CenterParent;
            setProduce.ShowDialog();
        }

        private void Menu_set_station_Click(object sender, EventArgs e)
        {
            Login login = new Login();
            Login.UserType userType = login.GetUserType;
            if (userType == Login.UserType.USER_ADMIN)
            {
                //管理员
                SetStationAdmin setStationAdmin = new SetStationAdmin();
                setStationAdmin.StartPosition = FormStartPosition.CenterParent;
                setStationAdmin.ShowDialog();
                
            }
            else if (userType == Login.UserType.USER_ORDINARY)
            {
                //普通用户
                SetStation setStation = new SetStation();
                setStation.StartPosition = FormStartPosition.CenterParent;
                setStation.ShowDialog();
            }
        }
    }
}
