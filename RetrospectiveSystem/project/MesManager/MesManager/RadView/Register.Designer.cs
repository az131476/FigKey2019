namespace MesManager.RadView
{
    partial class Register
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
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.btn_register = new Telerik.WinControls.UI.RadButton();
            this.tb_username = new Telerik.WinControls.UI.RadTextBox();
            this.tb_pwd = new Telerik.WinControls.UI.RadTextBox();
            this.btn_cancel = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_register)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_username)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_pwd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(44, 55);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(42, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "用户名";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(44, 108);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(30, 18);
            this.radLabel2.TabIndex = 1;
            this.radLabel2.Text = "密码";
            // 
            // btn_register
            // 
            this.btn_register.Location = new System.Drawing.Point(123, 162);
            this.btn_register.Name = "btn_register";
            this.btn_register.Size = new System.Drawing.Size(71, 24);
            this.btn_register.TabIndex = 2;
            this.btn_register.Text = "注册";
            this.btn_register.Click += new System.EventHandler(this.Btn_register_Click);
            // 
            // tb_username
            // 
            this.tb_username.Location = new System.Drawing.Point(123, 53);
            this.tb_username.Name = "tb_username";
            this.tb_username.Size = new System.Drawing.Size(162, 20);
            this.tb_username.TabIndex = 3;
            // 
            // tb_pwd
            // 
            this.tb_pwd.Location = new System.Drawing.Point(123, 106);
            this.tb_pwd.Name = "tb_pwd";
            this.tb_pwd.Size = new System.Drawing.Size(162, 20);
            this.tb_pwd.TabIndex = 4;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(214, 162);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(71, 24);
            this.btn_cancel.TabIndex = 5;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.Click += new System.EventHandler(this.Btn_cancel_Click);
            // 
            // Register
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 240);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.tb_pwd);
            this.Controls.Add(this.tb_username);
            this.Controls.Add(this.btn_register);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.Name = "Register";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "注册";
            this.Load += new System.EventHandler(this.Register_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_register)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_username)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_pwd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadButton btn_register;
        private Telerik.WinControls.UI.RadTextBox tb_username;
        private Telerik.WinControls.UI.RadTextBox tb_pwd;
        private Telerik.WinControls.UI.RadButton btn_cancel;
    }
}
