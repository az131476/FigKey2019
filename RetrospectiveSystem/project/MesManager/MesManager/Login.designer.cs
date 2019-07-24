using System.Windows.Forms;

namespace MesManager
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.lbx_username = new Telerik.WinControls.UI.RadLabel();
            this.lbx_pwd = new Telerik.WinControls.UI.RadLabel();
            this.cb_memberpwd = new Telerik.WinControls.UI.RadCheckBox();
            this.lbx_ToFindPwd = new System.Windows.Forms.LinkLabel();
            this.lbx_regist = new Telerik.WinControls.UI.RadLabel();
            this.btn_login = new Telerik.WinControls.UI.RadButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tbx_pwd = new System.Windows.Forms.TextBox();
            this.tbx_username = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_username)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_pwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_memberpwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_regist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_login)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // lbx_username
            // 
            this.lbx_username.BackColor = System.Drawing.Color.Transparent;
            this.lbx_username.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbx_username.Location = new System.Drawing.Point(36, 108);
            this.lbx_username.Name = "lbx_username";
            this.lbx_username.Size = new System.Drawing.Size(56, 24);
            this.lbx_username.TabIndex = 2;
            this.lbx_username.Text = "用户名";
            // 
            // lbx_pwd
            // 
            this.lbx_pwd.BackColor = System.Drawing.Color.Transparent;
            this.lbx_pwd.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbx_pwd.Location = new System.Drawing.Point(36, 165);
            this.lbx_pwd.Name = "lbx_pwd";
            this.lbx_pwd.Size = new System.Drawing.Size(53, 24);
            this.lbx_pwd.TabIndex = 3;
            this.lbx_pwd.Text = "密   码";
            // 
            // cb_memberpwd
            // 
            this.cb_memberpwd.BackColor = System.Drawing.Color.Transparent;
            this.cb_memberpwd.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_memberpwd.Location = new System.Drawing.Point(107, 218);
            this.cb_memberpwd.Name = "cb_memberpwd";
            this.cb_memberpwd.Size = new System.Drawing.Size(77, 21);
            this.cb_memberpwd.TabIndex = 4;
            this.cb_memberpwd.Text = "记住密码";
            // 
            // lbx_ToFindPwd
            // 
            this.lbx_ToFindPwd.AutoSize = true;
            this.lbx_ToFindPwd.BackColor = System.Drawing.Color.Transparent;
            this.lbx_ToFindPwd.Location = new System.Drawing.Point(249, 216);
            this.lbx_ToFindPwd.Name = "lbx_ToFindPwd";
            this.lbx_ToFindPwd.Size = new System.Drawing.Size(85, 21);
            this.lbx_ToFindPwd.TabIndex = 6;
            this.lbx_ToFindPwd.TabStop = true;
            this.lbx_ToFindPwd.Text = "忘记密码?";
            // 
            // lbx_regist
            // 
            this.lbx_regist.BackColor = System.Drawing.Color.Transparent;
            this.lbx_regist.Location = new System.Drawing.Point(12, 292);
            this.lbx_regist.Name = "lbx_regist";
            this.lbx_regist.Size = new System.Drawing.Size(54, 18);
            this.lbx_regist.TabIndex = 7;
            this.lbx_regist.Text = "注册账号";
            this.lbx_regist.Click += new System.EventHandler(this.Lbx_regist_Click);
            // 
            // btn_login
            // 
            this.btn_login.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(240)))), ((int)(((byte)(113)))));
            this.btn_login.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_login.Location = new System.Drawing.Point(107, 272);
            this.btn_login.Name = "btn_login";
            // 
            // 
            // 
            this.btn_login.RootElement.ApplyShapeToControl = true;
            this.btn_login.RootElement.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren;
            this.btn_login.RootElement.CanFocus = true;
            this.btn_login.RootElement.CustomFont = "TelerikWebUI";
            this.btn_login.Size = new System.Drawing.Size(227, 38);
            this.btn_login.TabIndex = 9;
            this.btn_login.Text = "立  即  登  录";
            this.btn_login.Click += new System.EventHandler(this.Btn_login_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(81, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(251, 28);
            this.label1.TabIndex = 10;
            this.label1.Text = "万通智控产线追溯MES系统";
            // 
            // tbx_pwd
            // 
            this.tbx_pwd.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbx_pwd.Location = new System.Drawing.Point(107, 165);
            this.tbx_pwd.Name = "tbx_pwd";
            this.tbx_pwd.PasswordChar = '*';
            this.tbx_pwd.Size = new System.Drawing.Size(227, 29);
            this.tbx_pwd.TabIndex = 11;
            // 
            // tbx_username
            // 
            this.tbx_username.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbx_username.Location = new System.Drawing.Point(107, 111);
            this.tbx_username.Name = "tbx_username";
            this.tbx_username.Size = new System.Drawing.Size(227, 29);
            this.tbx_username.TabIndex = 12;
            // 
            // Login
            // 
            this.AcceptButton = this.btn_login;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.BackgroundImage = global::MesManager.Properties.Resources.背景_01;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(431, 354);
            this.ControlBox = false;
            this.Controls.Add(this.tbx_username);
            this.Controls.Add(this.tbx_pwd);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.lbx_regist);
            this.Controls.Add(this.lbx_ToFindPwd);
            this.Controls.Add(this.cb_memberpwd);
            this.Controls.Add(this.lbx_pwd);
            this.Controls.Add(this.lbx_username);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "登录";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lbx_username)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_pwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_memberpwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_regist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_login)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Telerik.WinControls.UI.RadLabel lbx_username;
        private Telerik.WinControls.UI.RadLabel lbx_pwd;
        private Telerik.WinControls.UI.RadCheckBox cb_memberpwd;
        private System.Windows.Forms.LinkLabel lbx_ToFindPwd;
        private Telerik.WinControls.UI.RadLabel lbx_regist;
        private Telerik.WinControls.UI.RadButton btn_login;
        private Label label1;
        private TextBox tbx_pwd;
        private TextBox tbx_username;
    }
}
