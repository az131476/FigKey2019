using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CommonUtils.Logger;
using CommonUtils.FileHelper;
using System.Web;
using System.Net;
using System.IO;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using MesManager.RadView;
using Telerik.WinControls.UI;

namespace MesManager
{
    public partial class Login : RadForm
    {
        private RadTitleBarElement titleBar;
        private const string USER_ADMIN = "管理员";
        private const string USER_ORDINARY = "普通用户";
        private const string INI_CONFIG_NAME = "userConfig.ini";
        private const string INI_CONFIG_SECTION = "usercfg";
        private const string INI_CONFIG_USER = "username";
        private const string INI_CONFIG_PWD = "password";
        private const string INI_CONFIG_REMBER = "remberpwd";
        private string configPath;
        private MesService.MesServiceClient mesService;
        private bool isFormMoving = false;
        public static UserType GetUserType { get; set; }
        public static string GetUserName { get; set; }

        [DllImport("user32.dll", EntryPoint = "HideCaret")]
        private static extern bool HideCaret(IntPtr hWnd);
        public Login()
        {
            InitializeComponent();
            PrepareTitleBar();
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.None;
        }    
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
                    //
                });
            };
            timer.Enabled = true;
            timer.Start();
        }

        public enum UserType
        {
            USER_ADMIN,
            USER_ORDINARY
        }

        private void Login_Load(object sender, EventArgs e)
        {

            Init();
            InitServiceInstance();
            tbx_username.KeyDown += Tbx_username_KeyDown;
        }

        private void Tbx_username_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
            {
                tbx_pwd.Focus();
            }
        }

        async private void InitServiceInstance()
        {
            try
            {
                mesService = new MesService.MesServiceClient();
            }
            catch (Exception ex)
            {
                MessageBox.Show("连接服务异常", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogHelper.Log.Error("获取服务异常！" + ex.Message);
            }
        }

        private void Btn_login_Click(object sender, EventArgs e)
        {
            //登录验证:用户角色+用户名+用户密码
            //登录设置(保存本地配置)：记住密码+自动登录
            //连接远程服务

            //if (cob_userType.SelectedIndex == (int)UserType.USER_ADMIN)
            //{
            //    //管理员登录
            //    if (!LocalValidate())
            //        return;
            //    RemoteValidate(MesService.LoginUser.ADMIN_USER);
            //    GetUserType = UserType.USER_ADMIN;
            //}
            //else if (cob_userType.SelectedIndex == (int)UserType.USER_ORDINARY)
            //{
            //    //普通用户登录
            //    if (!LocalValidate())
            //        return;
            //    RemoteValidate(MesService.LoginUser.ORDINARY_USER);
            //    GetUserType = UserType.USER_ORDINARY;
            //}
            if (!LocalValidate())
                return;
            RemoteValidate(MesService.LoginUser.ADMIN_USER);
            GetUserName = tbx_username.Text;

            UpdateUserCfg();
        }

        #region 本地验证
        /// <summary>
        /// 本地验证用户角色 用户名和密码
        /// </summary>
        /// <returns></returns>
        private bool LocalValidate()
        {
            //if (string.IsNullOrEmpty(cob_userType.Text))
            //{
            //    cob_userType.ForeColor = Color.Red;
            //    cob_userType.Focus();
            //    return false;
            //}
            //cob_userType.ForeColor = Color.Black;
            //if (!cob_userType.Text.Equals(USER_ADMIN) && !cob_userType.Text.Trim().Equals(USER_ORDINARY))
            //{
            //    cob_userType.ForeColor = Color.Red;
            //    cob_userType.Focus();
            //    return false;
            //}
            //cob_userType.ForeColor = Color.Black;
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
        private void RemoteValidate(MesService.LoginUser loginUser)
        {
            try
            {
                MesService.LoginResult loginRes = mesService.Login(tbx_username.Text, tbx_pwd.Text, loginUser);
                //验证用户密码
                switch (loginRes)
                {
                    case MesService.LoginResult.SUCCESS:
                        LogHelper.Log.Info("登录验证成功！");
                        //连接云服务
                        if (!ConnectCloudService())
                        {
                            MessageBox.Show("连接服务失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        LogHelper.Log.Info("连接服务成功！");
                        //启动主界面
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                        break;
                    case MesService.LoginResult.FAIL_EXCEP:
                        LogHelper.Log.Info("登录失败!");
                        break;
                    case MesService.LoginResult.USER_NAME_ERR:
                        //该用户不存在
                        tbx_username.ForeColor = Color.Red;
                        tbx_username.Focus();
                        break;
                    case MesService.LoginResult.USER_PWD_ERR:
                        tbx_pwd.ForeColor = Color.Red;
                        tbx_pwd.Focus();
                        return;
                    default:
                        break;
                }
                tbx_pwd.ForeColor = Color.Black;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message);
                MessageBox.Show(ex.Message,"Err");
            }
        }
        #endregion

        private bool ConnectCloudService()
        {
            //SuperEasyClient.ConnectServer();
            //SuperEasyClient.btnLogin("this is client");
            //btnDecrypt

            return true;
        }

        async private void Init()
        {
            mesService = new MesService.MesServiceClient();
            //设置单行
            //tbx_username.Multiline = false;
            tbx_pwd.Multiline = false;
            tbx_username.Items.Clear();
            DataTable dt = (await mesService.GetAllUserInfoAsync()).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                tbx_username.Items.Add(dt.Rows[i][0].ToString());
            }
            tbx_username.Text = "Admin";
            configPath = AppDomain.CurrentDomain.BaseDirectory+INI_CONFIG_NAME;
            ReadLastCfg();
        }
        private void ReadLastCfg()
        {
            try
            {
                var checkState = INIFile.GetValue(INI_CONFIG_SECTION,INI_CONFIG_REMBER,configPath);
                if (!string.IsNullOrEmpty(checkState))
                {
                    var curCbxState = (CheckState)Enum.Parse(typeof(CheckState),checkState);
                    cb_memberpwd.CheckState = curCbxState;
                    if (curCbxState == CheckState.Checked)
                    {
                        tbx_username.Text = INIFile.GetValue(INI_CONFIG_SECTION, INI_CONFIG_USER, configPath);
                        tbx_pwd.Text = INIFile.GetValue(INI_CONFIG_SECTION, INI_CONFIG_PWD, configPath);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message);
            }
        }

        private void UpdateUserCfg()
        {
            try
            {
                INIFile.SetValue(INI_CONFIG_SECTION,INI_CONFIG_REMBER,cb_memberpwd.CheckState+"",configPath);
                INIFile.SetValue(INI_CONFIG_SECTION,INI_CONFIG_USER,tbx_username.Text,configPath);
                INIFile.SetValue(INI_CONFIG_SECTION,INI_CONFIG_PWD,tbx_pwd.Text,configPath);
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message);
            }
        }

        private void Lbx_regist_Click(object sender, EventArgs e)
        {
            Register register = new Register();
            register.ShowDialog();
        }

        private void PrepareTitleBar()
        {
            titleBar = new RadTitleBarElement();
            titleBar.Text = "登录";
            titleBar.ForeColor = Color.White;
            titleBar.Font = new Font("Segoe UI Light", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(204)));
            titleBar.FillPrimitive.Visibility = ElementVisibility.Hidden;
            titleBar.MaxSize = new Size(0, 50);
            titleBar.Children[1].Visibility = ElementVisibility.Hidden;

            titleBar.CloseButton.Parent.PositionOffset = new SizeF(0, 10);//与top距离
            titleBar.CloseButton.MinSize = new Size(50, 20);
            titleBar.CloseButton.ButtonFillElement.Visibility = ElementVisibility.Collapsed;

            titleBar.MinimizeButton.MinSize = new Size(50, 50);
            titleBar.MinimizeButton.ButtonFillElement.Visibility = ElementVisibility.Collapsed;

            titleBar.MaximizeButton.MinSize = new Size(50, 50);
            titleBar.MaximizeButton.ButtonFillElement.Visibility = ElementVisibility.Collapsed;

            titleBar.CloseButton.SetValue(RadFormElement.IsFormActiveProperty, true);
            titleBar.MinimizeButton.SetValue(RadFormElement.IsFormActiveProperty, true);
            titleBar.MaximizeButton.SetValue(RadFormElement.IsFormActiveProperty, true);

            titleBar.Close += new TitleBarSystemEventHandler(titleBar_Close);
            titleBar.Minimize += new TitleBarSystemEventHandler(titleBar_Minimize);
            titleBar.MaximizeRestore += new TitleBarSystemEventHandler(titleBar_MaximizeRestore);
            this.radPanorama1.PanoramaElement.PanGesture += new PanGestureEventHandler(radTilePanel1_PanGesture);
            this.radPanorama1.PanoramaElement.Children.Add(titleBar);
        }

        void radTilePanel1_PanGesture(object sender, PanGestureEventArgs e)
        {
            if (e.IsBegin && this.titleBar.ControlBoundingRectangle.Contains(e.Location))
            {
                isFormMoving = true;
            }

            if (isFormMoving)
            {
                this.Location = new Point(this.Location.X + e.Offset.Width, this.Location.Y + e.Offset.Height);
            }
            else
            {
                e.Handled = false;
            }

            if (e.IsEnd)
            {
                isFormMoving = false;
            }
        }

        #region Event Handlers
        void titleBar_MaximizeRestore(object sender, EventArgs args)
        {
            int left = Screen.PrimaryScreen.Bounds.Width;
            int top = Screen.PrimaryScreen.Bounds.Height;
            int right = Screen.PrimaryScreen.Bounds.Width;
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        void titleBar_Minimize(object sender, EventArgs args)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        void titleBar_Close(object sender, EventArgs args)
        {
            Application.Exit();
        }

        #endregion
    }
}
