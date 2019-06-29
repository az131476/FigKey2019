using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CommonUtils.Logger;
using FigKeyLoggerConfigurator.ClientSocket;
using LoggerConfigurator.ClientSocket;

namespace LoggerConfigurator
{
    public partial class Login : Telerik.WinControls.UI.RadForm
    {
        private const string USER_ADMIN = "管理员";
        private const string USER_ORDINARY = "普通用户";

        private ServiceReference1.FigKeyLoggerServiceClient remoteService;

        public Login()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        /// 解决跨线程调用UI组件问题     
        private void DelegateAction(Action action)
        {
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        private void Timer()
        {
            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += (s, x) =>
            {
                DelegateAction(() =>
                {
                    //txtAll.Text = LogHelper.SetOnLog();
                    //txtAll.Select(txtAll.TextLength, 0);
                    //txtAll.ScrollToCaret();
                });
            };
            timer.Enabled = true;
            timer.Start();
        }

        private enum UserType
        {
            USER_ADMIN,
            USER_ORDINARY
        }

        private void Login_Load(object sender, EventArgs e)
        {
            Init();
            remoteService = new ServiceReference1.FigKeyLoggerServiceClient();
        }

        private void Btn_login_Click(object sender, EventArgs e)
        {
            //登录验证:用户角色+用户名+用户密码
            //登录设置(保存本地配置)：记住密码+自动登录
            //连接远程服务

            if (cob_userType.SelectedIndex == (int)UserType.USER_ADMIN)
            {
                //管理员登录
                if (!LocalValidate())
                    return;
                RemoteValidate(ServiceReference1.LoginUser.ADMIN_USER);
            }
            else if (cob_userType.SelectedIndex == (int)UserType.USER_ORDINARY)
            {
                //普通用户登录
                if (!LocalValidate())
                    return;
                RemoteValidate(ServiceReference1.LoginUser.ORDINARY_USER);
            }
        }

        #region 本地验证
        /// <summary>
        /// 本地验证用户角色 用户名和密码
        /// </summary>
        /// <returns></returns>
        private bool LocalValidate()
        {
            if (string.IsNullOrEmpty(cob_userType.Text))
            {
                cob_userType.ForeColor = Color.Red;
                cob_userType.Focus();
                return false;
            }
            cob_userType.ForeColor = Color.Black;
            if (!cob_userType.Text.Equals(USER_ADMIN) && !cob_userType.Text.Trim().Equals(USER_ORDINARY))
            {
                cob_userType.ForeColor = Color.Red;
                cob_userType.Focus();
                return false;
            }
            cob_userType.ForeColor = Color.Black;
            if (string.IsNullOrEmpty(tbx_username.Text))
            {
                tbx_username.ForeColor = Color.Red;
                tbx_username.Focus();
                return false;
            }
            tbx_username.ForeColor = Color.Black;
            if (string.IsNullOrEmpty(tbx_pwd.Text))
            {
                tbx_pwd.ForeColor = Color.Red;
                tbx_pwd.Focus();
                return false;
            }
            return true;
        }
        #endregion

        #region 接口验证用户名 密码
        /// <summary>
        /// 调用接口验证用户名和密码
        /// </summary>
        /// <param name="loginUser"></param>
        private void RemoteValidate(ServiceReference1.LoginUser loginUser)
        {
            ServiceReference1.f_user userInfo = remoteService.GetUserInfo(tbx_username.Text.Trim());
            ServiceReference1.LoginResult loginRes = remoteService.Login(tbx_username.Text,tbx_pwd.Text,loginUser);
            if (userInfo == null)
            {
                //该用户不存在
                tbx_username.ForeColor = Color.Red;
                tbx_username.Focus();
                return;
            }
            tbx_username.ForeColor = Color.Black;

            //验证用户密码
            switch (loginRes)
            {
                case ServiceReference1.LoginResult.SUCCESS:
                    LogHelper.Log.Info("登录验证成功！");
                    //连接云服务
                    if (!ConnectCloudService())
                    {
                        MessageBox.Show("连接服务失败！","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }
                    LogHelper.Log.Info("连接服务成功！");
                    //启动主界面
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    break;
                case ServiceReference1.LoginResult.FAIL_EXCEP:
                    LogHelper.Log.Info("登录失败!");
                    break;

                case ServiceReference1.LoginResult.USER_NAME_PWD_ERR:
                    //MessageBox.Show("输入密码错误！","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    tbx_pwd.ForeColor = Color.Red;
                    tbx_pwd.Focus();
                    return;
                default:
                    break;
            }
            tbx_pwd.ForeColor = Color.Black;
        }
        #endregion

        private bool ConnectCloudService()
        {
            //SocketClient client = new SocketClient();
            //client.StartSocket();
            //client.Send();
            SuperEasyClient.ConnectServer();
            SuperEasyClient.btnLogin("this is client");
            //btnDecrypt

            return true;
        }

        private void Init()
        {
            //设置单行
            tbx_username.Multiline = false;
            tbx_pwd.Multiline = false;

            cob_userType.Items.Clear();
            cob_userType.Items.Add(USER_ADMIN);
            cob_userType.Items.Add(USER_ORDINARY);
            cob_userType.SelectedIndex = (int)UserType.USER_ADMIN;
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        //找回密码

        //注册新账号
    }
}
