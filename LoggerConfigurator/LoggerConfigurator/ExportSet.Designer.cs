namespace LoggerConfigurator
{
    partial class ExportSet
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
            this.cb_can1 = new Telerik.WinControls.UI.RadCheckBox();
            this.cb_can2 = new Telerik.WinControls.UI.RadCheckBox();
            this.cb_save = new Telerik.WinControls.UI.RadCheckBox();
            this.btn_ok = new Telerik.WinControls.UI.RadButton();
            this.btn_cancel = new Telerik.WinControls.UI.RadButton();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.cb_can1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_can2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_save)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_ok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // cb_can1
            // 
            this.cb_can1.Location = new System.Drawing.Point(51, 31);
            this.cb_can1.Name = "cb_can1";
            this.cb_can1.Size = new System.Drawing.Size(90, 18);
            this.cb_can1.TabIndex = 0;
            this.cb_can1.Text = "CAN1数据";
            this.cb_can1.ThemeName = "Crystal";
            // 
            // cb_can2
            // 
            this.cb_can2.Location = new System.Drawing.Point(185, 31);
            this.cb_can2.Name = "cb_can2";
            this.cb_can2.Size = new System.Drawing.Size(90, 18);
            this.cb_can2.TabIndex = 1;
            this.cb_can2.Text = "CAN2数据";
            this.cb_can2.ThemeName = "Crystal";
            // 
            // cb_save
            // 
            this.cb_save.Location = new System.Drawing.Point(51, 84);
            this.cb_save.Name = "cb_save";
            this.cb_save.Size = new System.Drawing.Size(68, 18);
            this.cb_save.TabIndex = 2;
            this.cb_save.Text = "保存设置";
            this.cb_save.ThemeName = "Breeze";
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(144, 119);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(58, 24);
            this.btn_ok.TabIndex = 3;
            this.btn_ok.Text = "确定";
            this.btn_ok.ThemeName = "Breeze";
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(236, 119);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(58, 24);
            this.btn_cancel.TabIndex = 4;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.ThemeName = "Breeze";
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.cb_can1);
            this.radGroupBox1.Controls.Add(this.cb_can2);
            this.radGroupBox1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radGroupBox1.HeaderText = "导出选择";
            this.radGroupBox1.Location = new System.Drawing.Point(0, 12);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(294, 66);
            this.radGroupBox1.TabIndex = 5;
            this.radGroupBox1.Text = "导出选择";
            this.radGroupBox1.ThemeName = "Breeze";
            // 
            // ExportSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(306, 145);
            this.Controls.Add(this.radGroupBox1);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.cb_save);
            this.Name = "ExportSet";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "导出设置";
            this.Load += new System.EventHandler(this.ExportSet_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cb_can1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_can2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cb_save)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_ok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadCheckBox cb_can1;
        private Telerik.WinControls.UI.RadCheckBox cb_can2;
        private Telerik.WinControls.UI.RadCheckBox cb_save;
        private Telerik.WinControls.UI.RadButton btn_ok;
        private Telerik.WinControls.UI.RadButton btn_cancel;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
    }
}
