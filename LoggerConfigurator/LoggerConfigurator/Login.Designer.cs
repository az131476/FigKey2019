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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.lbx_username = new Telerik.WinControls.UI.RadLabel();
            this.lbx_pwd = new Telerik.WinControls.UI.RadLabel();
            this.cb_autoLogin = new Telerik.WinControls.UI.RadCheckBox();
            this.radCheckBox1 = new Telerik.WinControls.UI.RadCheckBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.btn_login = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_username)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_pwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_autoLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_login)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(92, 75);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(212, 28);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(92, 126);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(212, 28);
            this.richTextBox2.TabIndex = 1;
            this.richTextBox2.Text = "";
            // 
            // lbx_username
            // 
            this.lbx_username.Location = new System.Drawing.Point(21, 77);
            this.lbx_username.Name = "lbx_username";
            this.lbx_username.Size = new System.Drawing.Size(42, 18);
            this.lbx_username.TabIndex = 2;
            this.lbx_username.Text = "用户名";
            // 
            // lbx_pwd
            // 
            this.lbx_pwd.Location = new System.Drawing.Point(21, 136);
            this.lbx_pwd.Name = "lbx_pwd";
            this.lbx_pwd.Size = new System.Drawing.Size(30, 18);
            this.lbx_pwd.TabIndex = 3;
            this.lbx_pwd.Text = "密码";
            // 
            // cb_autoLogin
            // 
            this.cb_autoLogin.Location = new System.Drawing.Point(92, 177);
            this.cb_autoLogin.Name = "cb_autoLogin";
            this.cb_autoLogin.Size = new System.Drawing.Size(68, 18);
            this.cb_autoLogin.TabIndex = 4;
            this.cb_autoLogin.Text = "记住密码";
            // 
            // radCheckBox1
            // 
            this.radCheckBox1.Location = new System.Drawing.Point(166, 177);
            this.radCheckBox1.Name = "radCheckBox1";
            this.radCheckBox1.Size = new System.Drawing.Size(68, 18);
            this.radCheckBox1.TabIndex = 5;
            this.radCheckBox1.Text = "自动登录";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(240, 178);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(64, 13);
            this.linkLabel1.TabIndex = 6;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "忘记密码?";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(1, 299);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(54, 18);
            this.radLabel1.TabIndex = 7;
            this.radLabel1.Text = "注册账号";
            // 
            // btn_login
            // 
            this.btn_login.BackColor = System.Drawing.Color.DodgerBlue;
            this.btn_login.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_login.Location = new System.Drawing.Point(92, 218);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(212, 32);
            this.btn_login.TabIndex = 9;
            this.btn_login.Text = "登录";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 314);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.radCheckBox1);
            this.Controls.Add(this.cb_autoLogin);
            this.Controls.Add(this.lbx_pwd);
            this.Controls.Add(this.lbx_username);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.richTextBox1);
            this.Name = "Login";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "登录";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.lbx_username)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_pwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_autoLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radCheckBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_login)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private Telerik.WinControls.UI.RadLabel lbx_username;
        private Telerik.WinControls.UI.RadLabel lbx_pwd;
        private Telerik.WinControls.UI.RadCheckBox cb_autoLogin;
        private Telerik.WinControls.UI.RadCheckBox radCheckBox1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadButton btn_login;
    }
}
