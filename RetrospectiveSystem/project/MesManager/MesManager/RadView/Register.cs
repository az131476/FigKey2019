using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Text.RegularExpressions;
using CommonUtils.CalculateAndString;

namespace MesManager.RadView
{
    public partial class Register : Telerik.WinControls.UI.RadForm
    {
        private MesService.MesServiceClient serviceClient;
        public Register()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent; 
        }

        private void Register_Load(object sender, EventArgs e)
        {
            serviceClient = new MesService.MesServiceClient();
            serviceClient.InitConnectString();
            tb_pwd.PasswordChar = '*';
        }

        private void Btn_register_Click(object sender, EventArgs e)
        {
            RegisterUser();
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        async private void RegisterUser()
        {
            if (string.IsNullOrEmpty(tb_username.Text.Trim()))
            {
                MessageBox.Show("用户名不能为空！","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(tb_pwd.Text.Trim()))
            {
                MessageBox.Show("密码不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //校验密码复杂度
            if (!RegexHelper.IsMatchPassword(tb_pwd.Text))
            {
                //密码复杂度不满足
                MessageBox.Show("密码必须包含数字、字母","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            MesService.LoginUser loginUser = MesService.LoginUser.ADMIN_USER;
            DataSet dataSet;
            serviceClient.GetUserInfo(tb_username.Text.Trim(),out dataSet);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("用户已存在！","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                tb_username.ForeColor = Color.Red;
                return;
            }
            tb_username.ForeColor = Color.Black;
            MesService.RegisterResult registerResult = await serviceClient.RegisterAsync(tb_username.Text.Trim(),tb_pwd.Text.Trim(),"","",loginUser);
            if (registerResult == MesService.RegisterResult.REGISTER_SUCCESS)
            {
                //注册成功
                MessageBox.Show("注册成功！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
        }
    }
}
