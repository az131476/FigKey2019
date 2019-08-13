namespace MesManager.UI
{
    partial class TProcess
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TProcess));
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radMenu1 = new Telerik.WinControls.UI.RadMenu();
            this.menu_add = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_del = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_commit = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_refresh = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_grid = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_clear_db = new Telerik.WinControls.UI.RadMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbx_process = new System.Windows.Forms.Label();
            this.cb_processItem = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cb_curprocess = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.btn_setprocess = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_setprocess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radMenu1
            // 
            this.radMenu1.BackColor = System.Drawing.Color.Transparent;
            this.radMenu1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radMenu1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menu_add,
            this.menu_del,
            this.menu_commit,
            this.menu_refresh,
            this.menu_grid,
            this.menu_clear_db});
            this.radMenu1.Location = new System.Drawing.Point(0, 0);
            this.radMenu1.Name = "radMenu1";
            this.radMenu1.Size = new System.Drawing.Size(792, 36);
            this.radMenu1.TabIndex = 10;
            // 
            // menu_add
            // 
            this.menu_add.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.menu_add.Image = global::MesManager.Properties.Resources.bullet_add;
            this.menu_add.Name = "menu_add";
            this.menu_add.Text = "新增";
            this.menu_add.UseCompatibleTextRendering = false;
            // 
            // menu_del
            // 
            this.menu_del.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.menu_del.Image = ((System.Drawing.Image)(resources.GetObject("menu_del.Image")));
            this.menu_del.Name = "menu_del";
            this.menu_del.Shape = null;
            this.menu_del.Text = "删除";
            this.menu_del.UseCompatibleTextRendering = false;
            // 
            // menu_commit
            // 
            this.menu_commit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.menu_commit.Image = global::MesManager.Properties.Resources.upload_for_cloud;
            this.menu_commit.Name = "menu_commit";
            this.menu_commit.Text = "修改";
            this.menu_commit.UseCompatibleTextRendering = false;
            // 
            // menu_refresh
            // 
            this.menu_refresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.menu_refresh.Image = global::MesManager.Properties.Resources.update;
            this.menu_refresh.Name = "menu_refresh";
            this.menu_refresh.Text = "刷新";
            this.menu_refresh.UseCompatibleTextRendering = false;
            // 
            // menu_grid
            // 
            this.menu_grid.ForeColor = System.Drawing.Color.White;
            this.menu_grid.Image = global::MesManager.Properties.Resources.ClearGrid;
            this.menu_grid.Name = "menu_grid";
            this.menu_grid.Text = "清空显示";
            this.menu_grid.UseCompatibleTextRendering = false;
            // 
            // menu_clear_db
            // 
            this.menu_clear_db.ForeColor = System.Drawing.Color.White;
            this.menu_clear_db.Image = global::MesManager.Properties.Resources.DeleteDataSource_16x16;
            this.menu_clear_db.Name = "menu_clear_db";
            this.menu_clear_db.Text = "清空数据";
            this.menu_clear_db.UseCompatibleTextRendering = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbx_process);
            this.panel1.Controls.Add(this.cb_processItem);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(792, 65);
            this.panel1.TabIndex = 12;
            // 
            // lbx_process
            // 
            this.lbx_process.AutoSize = true;
            this.lbx_process.ForeColor = System.Drawing.Color.White;
            this.lbx_process.Location = new System.Drawing.Point(12, 31);
            this.lbx_process.Name = "lbx_process";
            this.lbx_process.Size = new System.Drawing.Size(59, 13);
            this.lbx_process.TabIndex = 11;
            this.lbx_process.Text = "工艺名称";
            // 
            // cb_processItem
            // 
            this.cb_processItem.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_processItem.FormattingEnabled = true;
            this.cb_processItem.Location = new System.Drawing.Point(81, 23);
            this.cb_processItem.Name = "cb_processItem";
            this.cb_processItem.Size = new System.Drawing.Size(183, 28);
            this.cb_processItem.TabIndex = 10;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_setprocess);
            this.groupBox1.Controls.Add(this.cb_curprocess);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.ForeColor = System.Drawing.Color.White;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(792, 73);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "工艺设置";
            // 
            // cb_curprocess
            // 
            this.cb_curprocess.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cb_curprocess.FormattingEnabled = true;
            this.cb_curprocess.Location = new System.Drawing.Point(173, 30);
            this.cb_curprocess.Name = "cb_curprocess";
            this.cb_curprocess.Size = new System.Drawing.Size(183, 28);
            this.cb_curprocess.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(78, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "当前工艺：";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 615);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(792, 73);
            this.panel2.TabIndex = 17;
            // 
            // radGridView1
            // 
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.Location = new System.Drawing.Point(0, 101);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.Size = new System.Drawing.Size(792, 514);
            this.radGridView1.TabIndex = 0;
            // 
            // btn_setprocess
            // 
            this.btn_setprocess.Location = new System.Drawing.Point(417, 30);
            this.btn_setprocess.Name = "btn_setprocess";
            this.btn_setprocess.Size = new System.Drawing.Size(63, 28);
            this.btn_setprocess.TabIndex = 12;
            this.btn_setprocess.Text = "应用";
            this.btn_setprocess.Click += new System.EventHandler(this.Btn_setprocess_Click);
            // 
            // TProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(792, 688);
            this.Controls.Add(this.radGridView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.radMenu1);
            this.Name = "TProcess";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "工艺流程";
            this.Load += new System.EventHandler(this.TProcess_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_setprocess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadMenu radMenu1;
        private Telerik.WinControls.UI.RadMenuItem menu_add;
        private Telerik.WinControls.UI.RadMenuItem menu_del;
        private Telerik.WinControls.UI.RadMenuItem menu_commit;
        private Telerik.WinControls.UI.RadMenuItem menu_refresh;
        private Telerik.WinControls.UI.RadMenuItem menu_grid;
        private Telerik.WinControls.UI.RadMenuItem menu_clear_db;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbx_process;
        private System.Windows.Forms.ComboBox cb_processItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cb_curprocess;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadButton btn_setprocess;
    }
}
