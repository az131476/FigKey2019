namespace LoggerConfigurator
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
            this.tbx_username = new System.Windows.Forms.RichTextBox();
            this.tbx_pwd = new System.Windows.Forms.RichTextBox();
            this.lbx_username = new Telerik.WinControls.UI.RadLabel();
            this.lbx_pwd = new Telerik.WinControls.UI.RadLabel();
            this.cb_memberpwd = new Telerik.WinControls.UI.RadCheckBox();
            this.cb_autologin = new Telerik.WinControls.UI.RadCheckBox();
            this.lbx_ToFindPwd = new System.Windows.Forms.LinkLabel();
            this.lbx_regist = new Telerik.WinControls.UI.RadLabel();
            this.btn_login = new Telerik.WinControls.UI.RadButton();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.cob_userType = new System.Windows.Forms.ComboBox();
            this.btn_cancel = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_username)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_pwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_memberpwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_autologin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_regist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_login)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tbx_username
            // 
            this.tbx_username.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbx_username.Location = new System.Drawing.Point(92, 86);
            this.tbx_username.Name = "tbx_username";
            this.tbx_username.Size = new System.Drawing.Size(212, 28);
            this.tbx_username.TabIndex = 0;
            this.tbx_username.Text = "";
            // 
            // tbx_pwd
            // 
            this.tbx_pwd.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbx_pwd.Location = new System.Drawing.Point(92, 137);
            this.tbx_pwd.Name = "tbx_pwd";
            this.tbx_pwd.Size = new System.Drawing.Size(212, 28);
            this.tbx_pwd.TabIndex = 1;
            this.tbx_pwd.Text = "";
            // 
            // lbx_username
            // 
            this.lbx_username.Location = new System.Drawing.Point(21, 88);
            this.lbx_username.Name = "lbx_username";
            this.lbx_username.Size = new System.Drawing.Size(42, 18);
            this.lbx_username.TabIndex = 2;
            this.lbx_username.Text = "用户名";
            // 
            // lbx_pwd
            // 
            this.lbx_pwd.Location = new System.Drawing.Point(21, 147);
            this.lbx_pwd.Name = "lbx_pwd";
            this.lbx_pwd.Size = new System.Drawing.Size(30, 18);
            this.lbx_pwd.TabIndex = 3;
            this.lbx_pwd.Text = "密码";
            // 
            // cb_memberpwd
            // 
            this.cb_memberpwd.Location = new System.Drawing.Point(92, 188);
            this.cb_memberpwd.Name = "cb_memberpwd";
            this.cb_memberpwd.Size = new System.Drawing.Size(68, 18);
            this.cb_memberpwd.TabIndex = 4;
            this.cb_memberpwd.Text = "记住密码";
            // 
            // cb_autologin
            // 
            this.cb_autologin.Location = new System.Drawing.Point(166, 188);
            this.cb_autologin.Name = "cb_autologin";
            this.cb_autologin.Size = new System.Drawing.Size(68, 18);
            this.cb_autologin.TabIndex = 5;
            this.cb_autologin.Text = "自动登录";
            // 
            // lbx_ToFindPwd
            // 
            this.lbx_ToFindPwd.AutoSize = true;
            this.lbx_ToFindPwd.Location = new System.Drawing.Point(240, 189);
            this.lbx_ToFindPwd.Name = "lbx_ToFindPwd";
            this.lbx_ToFindPwd.Size = new System.Drawing.Size(64, 13);
            this.lbx_ToFindPwd.TabIndex = 6;
            this.lbx_ToFindPwd.TabStop = true;
            this.lbx_ToFindPwd.Text = "忘记密码?";
            // 
            // lbx_regist
            // 
            this.lbx_regist.Location = new System.Drawing.Point(1, 299);
            this.lbx_regist.Name = "lbx_regist";
            this.lbx_regist.Size = new System.Drawing.Size(54, 18);
            this.lbx_regist.TabIndex = 7;
            this.lbx_regist.Text = "注册账号";
            // 
            // btn_login
            // 
            this.btn_login.BackColor = System.Drawing.Color.DodgerBlue;
            this.btn_login.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_login.Location = new System.Drawing.Point(141, 285);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(68, 32);
            this.btn_login.TabIndex = 9;
            this.btn_login.Text = "登录";
            this.btn_login.ThemeName = "Breeze";
            this.btn_login.Click += new System.EventHandler(this.Btn_login_Click);
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(21, 38);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(30, 18);
            this.radLabel1.TabIndex = 11;
            this.radLabel1.Text = "用户";
            // 
            // cob_userType
            // 
            this.cob_userType.FormattingEnabled = true;
            this.cob_userType.Location = new System.Drawing.Point(92, 38);
            this.cob_userType.Name = "cob_userType";
            this.cob_userType.Size = new System.Drawing.Size(212, 20);
            this.cob_userType.TabIndex = 12;
            // 
            // btn_cancel
            // 
            this.btn_cancel.BackColor = System.Drawing.Color.DodgerBlue;
            this.btn_cancel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_cancel.Location = new System.Drawing.Point(230, 285);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(68, 32);
            this.btn_cancel.TabIndex = 13;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.ThemeName = "Breeze";
            this.btn_cancel.Click += new System.EventHandler(this.Btn_cancel_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(344, 341);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.cob_userType);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.lbx_regist);
            this.Controls.Add(this.lbx_ToFindPwd);
            this.Controls.Add(this.cb_autologin);
            this.Controls.Add(this.cb_memberpwd);
            this.Controls.Add(this.lbx_pwd);
            this.Controls.Add(this.lbx_username);
            this.Controls.Add(this.tbx_pwd);
            this.Controls.Add(this.tbx_username);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
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
            ((System.ComponentModel.ISupportInitialize)(this.cb_autologin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_regist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_login)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox tbx_username;
        private System.Windows.Forms.RichTextBox tbx_pwd;
        private Telerik.WinControls.UI.RadLabel lbx_username;
        private Telerik.WinControls.UI.RadLabel lbx_pwd;
        private Telerik.WinControls.UI.RadCheckBox cb_memberpwd;
        private Telerik.WinControls.UI.RadCheckBox cb_autologin;
        private System.Windows.Forms.LinkLabel lbx_ToFindPwd;
        private Telerik.WinControls.UI.RadLabel lbx_regist;
        private Telerik.WinControls.UI.RadButton btn_login;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private System.Windows.Forms.ComboBox cob_userType;
        private Telerik.WinControls.UI.RadButton btn_cancel;
    }
}
