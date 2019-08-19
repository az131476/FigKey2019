namespace MesManager.UI
{
    partial class TestStand
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tool_productTypeNo = new System.Windows.Forms.ToolStripComboBox();
            this.radMenuItem5 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem6 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem7 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem8 = new Telerik.WinControls.UI.RadMenuItem();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.radMenu1 = new Telerik.WinControls.UI.RadMenu();
            this.tool_export = new System.Windows.Forms.ToolStripButton();
            this.tool_refresh = new System.Windows.Forms.ToolStripButton();
            this.radMenuItem1 = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_programe_version = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_limit_cfg = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_log_data = new Telerik.WinControls.UI.RadMenuItem();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.toolStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.tool_productTypeNo,
            this.tool_export,
            this.tool_refresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 23);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1187, 26);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(61, 23);
            this.toolStripLabel1.Text = "产品类型";
            // 
            // tool_productTypeNo
            // 
            this.tool_productTypeNo.Name = "tool_productTypeNo";
            this.tool_productTypeNo.Size = new System.Drawing.Size(121, 26);
            // 
            // radMenuItem5
            // 
            this.radMenuItem5.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem6,
            this.radMenuItem7,
            this.radMenuItem8});
            this.radMenuItem5.Name = "radMenuItem5";
            this.radMenuItem5.Text = "文件";
            // 
            // radMenuItem6
            // 
            this.radMenuItem6.Name = "radMenuItem6";
            this.radMenuItem6.Text = "打开";
            // 
            // radMenuItem7
            // 
            this.radMenuItem7.Name = "radMenuItem7";
            this.radMenuItem7.Text = "保存";
            // 
            // radMenuItem8
            // 
            this.radMenuItem8.Name = "radMenuItem8";
            this.radMenuItem8.Text = "退出";
            // 
            // radGridView1
            // 
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.Location = new System.Drawing.Point(0, 49);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.Size = new System.Drawing.Size(1187, 605);
            this.radGridView1.TabIndex = 2;
            // 
            // radMenu1
            // 
            this.radMenu1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radMenu1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem5,
            this.radMenuItem1});
            this.radMenu1.Location = new System.Drawing.Point(0, 0);
            this.radMenu1.Name = "radMenu1";
            this.radMenu1.Size = new System.Drawing.Size(1187, 23);
            this.radMenu1.TabIndex = 0;
            // 
            // tool_export
            // 
            this.tool_export.Image = global::MesManager.Properties.Resources.Export_16x16;
            this.tool_export.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tool_export.Name = "tool_export";
            this.tool_export.Size = new System.Drawing.Size(55, 23);
            this.tool_export.Text = "导出";
            // 
            // tool_refresh
            // 
            this.tool_refresh.Image = global::MesManager.Properties.Resources.Refresh_16x16;
            this.tool_refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tool_refresh.Name = "tool_refresh";
            this.tool_refresh.Size = new System.Drawing.Size(55, 23);
            this.tool_refresh.Text = "刷新";
            // 
            // radMenuItem1
            // 
            this.radMenuItem1.Image = null;
            this.radMenuItem1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menu_programe_version,
            this.menu_limit_cfg,
            this.menu_log_data});
            this.radMenuItem1.Name = "radMenuItem1";
            this.radMenuItem1.Text = "数据类型";
            // 
            // menu_programe_version
            // 
            this.menu_programe_version.Name = "menu_programe_version";
            this.menu_programe_version.Text = "程序版本";
            // 
            // menu_limit_cfg
            // 
            this.menu_limit_cfg.Name = "menu_limit_cfg";
            this.menu_limit_cfg.Text = "LIMIT配置";
            // 
            // menu_log_data
            // 
            this.menu_log_data.Name = "menu_log_data";
            this.menu_log_data.Text = "LOG数据";
            // 
            // TestStand
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(1187, 654);
            this.Controls.Add(this.radGridView1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.radMenu1);
            this.Name = "TestStand";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "测试台";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadMenu radMenu1;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem1;
        private Telerik.WinControls.UI.RadMenuItem menu_programe_version;
        private Telerik.WinControls.UI.RadMenuItem menu_limit_cfg;
        private Telerik.WinControls.UI.RadMenuItem menu_log_data;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox tool_productTypeNo;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem5;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem6;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem7;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem8;
        private System.Windows.Forms.ToolStripButton tool_export;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private System.Windows.Forms.ToolStripButton tool_refresh;
    }
}
