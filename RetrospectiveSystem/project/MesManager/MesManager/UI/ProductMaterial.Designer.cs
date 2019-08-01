namespace MesManager.RadView
{
    partial class ProductMaterial
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
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn3 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.GridViewComboBoxColumn gridViewComboBoxColumn3 = new Telerik.WinControls.UI.GridViewComboBoxColumn();
            Telerik.WinControls.UI.GridViewComboBoxColumn gridViewComboBoxColumn4 = new Telerik.WinControls.UI.GridViewComboBoxColumn();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn4 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.radGroupBox_type = new Telerik.WinControls.UI.RadGroupBox();
            this.listView = new System.Windows.Forms.ListView();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.cb_type_no = new System.Windows.Forms.ComboBox();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.btn_commit = new Telerik.WinControls.UI.RadButton();
            this.btn_cancel = new Telerik.WinControls.UI.RadButton();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menu_add = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_del = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_update = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_refresh = new System.Windows.Forms.ToolStripMenuItem();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.dgview = new System.Windows.Forms.DataGridView();
            this.dgv_order = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgv_typeno = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgv_materialcode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dgv_describle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox_type)).BeginInit();
            this.radGroupBox_type.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_commit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cancel)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radGroupBox_type
            // 
            this.radGroupBox_type.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox_type.BackColor = System.Drawing.Color.Transparent;
            this.radGroupBox_type.Controls.Add(this.listView);
            this.radGroupBox_type.Controls.Add(this.radLabel1);
            this.radGroupBox_type.Controls.Add(this.cb_type_no);
            this.radGroupBox_type.Controls.Add(this.radLabel2);
            this.radGroupBox_type.HeaderText = "配置产品物料";
            this.radGroupBox_type.Location = new System.Drawing.Point(12, 328);
            this.radGroupBox_type.Name = "radGroupBox_type";
            this.radGroupBox_type.Size = new System.Drawing.Size(1002, 132);
            this.radGroupBox_type.TabIndex = 7;
            this.radGroupBox_type.Text = "配置产品物料";
            // 
            // listView
            // 
            this.listView.Location = new System.Drawing.Point(389, 36);
            this.listView.Name = "listView";
            this.listView.Size = new System.Drawing.Size(245, 75);
            this.listView.TabIndex = 6;
            this.listView.UseCompatibleStateImageBehavior = false;
            // 
            // radLabel1
            // 
            this.radLabel1.ForeColor = System.Drawing.Color.White;
            this.radLabel1.Location = new System.Drawing.Point(5, 38);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(54, 18);
            this.radLabel1.TabIndex = 2;
            this.radLabel1.Text = "产品型号";
            // 
            // cb_type_no
            // 
            this.cb_type_no.FormattingEnabled = true;
            this.cb_type_no.Location = new System.Drawing.Point(65, 36);
            this.cb_type_no.Name = "cb_type_no";
            this.cb_type_no.Size = new System.Drawing.Size(245, 20);
            this.cb_type_no.TabIndex = 3;
            // 
            // radLabel2
            // 
            this.radLabel2.ForeColor = System.Drawing.Color.White;
            this.radLabel2.Location = new System.Drawing.Point(5, 78);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(54, 18);
            this.radLabel2.TabIndex = 5;
            this.radLabel2.Text = "物料编码";
            // 
            // btn_commit
            // 
            this.btn_commit.Location = new System.Drawing.Point(77, 483);
            this.btn_commit.Name = "btn_commit";
            this.btn_commit.Size = new System.Drawing.Size(110, 24);
            this.btn_commit.TabIndex = 8;
            this.btn_commit.Text = "提交";
            this.btn_commit.Click += new System.EventHandler(this.Btn_commit_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(231, 483);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(110, 24);
            this.btn_cancel.TabIndex = 9;
            this.btn_cancel.Text = "取消";
            this.btn_cancel.Click += new System.EventHandler(this.Btn_cancel_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Font = new System.Drawing.Font("Microsoft YaHei UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_add,
            this.menu_del,
            this.menu_update,
            this.menu_refresh});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1088, 29);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menu_add
            // 
            this.menu_add.ForeColor = System.Drawing.Color.White;
            this.menu_add.Name = "menu_add";
            this.menu_add.Size = new System.Drawing.Size(54, 25);
            this.menu_add.Text = "新增";
            this.menu_add.Click += new System.EventHandler(this.Menu_add_Click);
            // 
            // menu_del
            // 
            this.menu_del.ForeColor = System.Drawing.Color.White;
            this.menu_del.Name = "menu_del";
            this.menu_del.Size = new System.Drawing.Size(54, 25);
            this.menu_del.Text = "删除";
            // 
            // menu_update
            // 
            this.menu_update.ForeColor = System.Drawing.Color.White;
            this.menu_update.Name = "menu_update";
            this.menu_update.Size = new System.Drawing.Size(54, 25);
            this.menu_update.Text = "更新";
            // 
            // menu_refresh
            // 
            this.menu_refresh.ForeColor = System.Drawing.Color.White;
            this.menu_refresh.Name = "menu_refresh";
            this.menu_refresh.Size = new System.Drawing.Size(54, 25);
            this.menu_refresh.Text = "刷新";
            // 
            // radGridView1
            // 
            this.radGridView1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.radGridView1.Cursor = System.Windows.Forms.Cursors.Default;
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radGridView1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.radGridView1.ForeColor = System.Drawing.Color.Black;
            this.radGridView1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radGridView1.Location = new System.Drawing.Point(0, 29);
            // 
            // 
            // 
            gridViewTextBoxColumn3.EnableExpressionEditor = false;
            gridViewTextBoxColumn3.HeaderText = "序号";
            gridViewTextBoxColumn3.Name = "rdvc_order";
            gridViewComboBoxColumn3.EnableExpressionEditor = false;
            gridViewComboBoxColumn3.HeaderText = "产品型号";
            gridViewComboBoxColumn3.Name = "rdvc_typeNo";
            gridViewComboBoxColumn4.EnableExpressionEditor = false;
            gridViewComboBoxColumn4.HeaderText = "物料编码";
            gridViewComboBoxColumn4.Name = "rdvc_materialCode";
            gridViewTextBoxColumn4.EnableExpressionEditor = false;
            gridViewTextBoxColumn4.HeaderText = "描述";
            gridViewTextBoxColumn4.Name = "rdvc_describle";
            this.radGridView1.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn3,
            gridViewComboBoxColumn3,
            gridViewComboBoxColumn4,
            gridViewTextBoxColumn4});
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.radGridView1.Size = new System.Drawing.Size(1088, 138);
            this.radGridView1.TabIndex = 11;
            // 
            // dgview
            // 
            this.dgview.BackgroundColor = System.Drawing.Color.White;
            this.dgview.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgview.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgv_order,
            this.dgv_typeno,
            this.dgv_materialcode,
            this.dgv_describle});
            this.dgview.Location = new System.Drawing.Point(0, 173);
            this.dgview.Name = "dgview";
            this.dgview.RowTemplate.Height = 23;
            this.dgview.Size = new System.Drawing.Size(1088, 149);
            this.dgview.TabIndex = 12;
            // 
            // dgv_order
            // 
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_order.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgv_order.HeaderText = "序号";
            this.dgv_order.Name = "dgv_order";
            // 
            // dgv_typeno
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.White;
            this.dgv_typeno.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgv_typeno.HeaderText = "产品型号";
            this.dgv_typeno.Name = "dgv_typeno";
            // 
            // dgv_materialcode
            // 
            this.dgv_materialcode.DisplayStyle = System.Windows.Forms.DataGridViewComboBoxDisplayStyle.ComboBox;
            this.dgv_materialcode.DisplayStyleForCurrentCellOnly = true;
            this.dgv_materialcode.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.dgv_materialcode.HeaderText = "物料编码";
            this.dgv_materialcode.Name = "dgv_materialcode";
            // 
            // dgv_describle
            // 
            this.dgv_describle.HeaderText = "描述";
            this.dgv_describle.Name = "dgv_describle";
            // 
            // ProductMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.ClientSize = new System.Drawing.Size(1088, 528);
            this.Controls.Add(this.dgview);
            this.Controls.Add(this.radGridView1);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_commit);
            this.Controls.Add(this.radGroupBox_type);
            this.Controls.Add(this.menuStrip1);
            this.Name = "ProductMaterial";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "物料绑定";
            this.Load += new System.EventHandler(this.ProductMaterial_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox_type)).EndInit();
            this.radGroupBox_type.ResumeLayout(false);
            this.radGroupBox_type.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_commit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cancel)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox radGroupBox_type;
        private System.Windows.Forms.ListView listView;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private System.Windows.Forms.ComboBox cb_type_no;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadButton btn_commit;
        private Telerik.WinControls.UI.RadButton btn_cancel;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menu_add;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private System.Windows.Forms.ToolStripMenuItem menu_del;
        private System.Windows.Forms.ToolStripMenuItem menu_update;
        private System.Windows.Forms.ToolStripMenuItem menu_refresh;
        private System.Windows.Forms.DataGridView dgview;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_order;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgv_typeno;
        private System.Windows.Forms.DataGridViewComboBoxColumn dgv_materialcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgv_describle;
    }
}
