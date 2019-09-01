namespace MesManager.UI
{
    partial class TestLogDetail
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestLogDetail));
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pickerStartTime = new Telerik.WinControls.UI.RadDateTimePicker();
            this.pickerEndTime = new Telerik.WinControls.UI.RadDateTimePicker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_export = new Telerik.WinControls.UI.RadButton();
            this.btn_search = new Telerik.WinControls.UI.RadButton();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.breezeTheme1 = new Telerik.WinControls.Themes.BreezeTheme();
            ((System.ComponentModel.ISupportInitialize)(this.pickerStartTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickerEndTime)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_export)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_search)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(267, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "结束日期";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "开始日期";
            // 
            // pickerStartTime
            // 
            this.pickerStartTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.pickerStartTime.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pickerStartTime.ForeColor = System.Drawing.Color.Black;
            this.pickerStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.pickerStartTime.Location = new System.Drawing.Point(90, 29);
            this.pickerStartTime.Name = "pickerStartTime";
            this.pickerStartTime.Size = new System.Drawing.Size(164, 23);
            this.pickerStartTime.TabIndex = 4;
            this.pickerStartTime.TabStop = false;
            this.pickerStartTime.Text = "2019-08-26 17:09:35";
            this.pickerStartTime.Value = new System.DateTime(2019, 8, 26, 17, 9, 35, 395);
            // 
            // pickerEndTime
            // 
            this.pickerEndTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.pickerEndTime.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pickerEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.pickerEndTime.Location = new System.Drawing.Point(345, 29);
            this.pickerEndTime.Name = "pickerEndTime";
            this.pickerEndTime.Size = new System.Drawing.Size(164, 23);
            this.pickerEndTime.TabIndex = 5;
            this.pickerEndTime.TabStop = false;
            this.pickerEndTime.Text = "2019-08-26 17:09:41";
            this.pickerEndTime.Value = new System.DateTime(2019, 8, 26, 17, 9, 41, 544);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btn_export);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.pickerEndTime);
            this.panel1.Controls.Add(this.pickerStartTime);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1272, 62);
            this.panel1.TabIndex = 8;
            // 
            // btn_export
            // 
            this.btn_export.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_export.ForeColor = System.Drawing.Color.White;
            this.btn_export.Image = global::MesManager.Properties.Resources.Export_16x16;
            this.btn_export.Location = new System.Drawing.Point(717, 27);
            this.btn_export.Name = "btn_export";
            this.btn_export.Size = new System.Drawing.Size(72, 24);
            this.btn_export.TabIndex = 9;
            this.btn_export.Text = "导出";
            this.btn_export.ThemeName = "Breeze";
            this.btn_export.Click += new System.EventHandler(this.Btn_export_Click);
            // 
            // btn_search
            // 
            this.btn_search.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_search.ForeColor = System.Drawing.Color.White;
            this.btn_search.Image = global::MesManager.Properties.Resources.Search_16x16;
            this.btn_search.Location = new System.Drawing.Point(589, 27);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(74, 24);
            this.btn_search.TabIndex = 8;
            this.btn_search.Text = "查询";
            this.btn_search.ThemeName = "Breeze";
            this.btn_search.Click += new System.EventHandler(this.Btn_search_Click);
            // 
            // radGridView1
            // 
            this.radGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radGridView1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radGridView1.Location = new System.Drawing.Point(0, 62);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.Size = new System.Drawing.Size(1272, 522);
            this.radGridView1.TabIndex = 9;
            // 
            // TestLogDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(1272, 584);
            this.Controls.Add(this.radGridView1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestLogDetail";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "LOG详细记录";
            this.Load += new System.EventHandler(this.TestLogDetail_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pickerStartTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pickerEndTime)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_export)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_search)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private Telerik.WinControls.UI.RadDateTimePicker pickerStartTime;
        private Telerik.WinControls.UI.RadDateTimePicker pickerEndTime;
        private System.Windows.Forms.Panel panel1;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private Telerik.WinControls.UI.RadButton btn_search;
        private Telerik.WinControls.UI.RadButton btn_export;
        private Telerik.WinControls.Themes.BreezeTheme breezeTheme1;
    }
}
