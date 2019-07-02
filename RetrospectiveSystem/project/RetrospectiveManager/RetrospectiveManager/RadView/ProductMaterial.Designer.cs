namespace RetrospectiveManager.RadView
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
            this.radGroupBox_type = new Telerik.WinControls.UI.RadGroupBox();
            this.listView_select_station = new System.Windows.Forms.ListView();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.cb_type_no = new System.Windows.Forms.ComboBox();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radButton2 = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox_type)).BeginInit();
            this.radGroupBox_type.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radGroupBox_type
            // 
            this.radGroupBox_type.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox_type.Controls.Add(this.listView_select_station);
            this.radGroupBox_type.Controls.Add(this.radLabel1);
            this.radGroupBox_type.Controls.Add(this.cb_type_no);
            this.radGroupBox_type.Controls.Add(this.radLabel2);
            this.radGroupBox_type.HeaderText = "配置产品物料";
            this.radGroupBox_type.Location = new System.Drawing.Point(12, 12);
            this.radGroupBox_type.Name = "radGroupBox_type";
            this.radGroupBox_type.Size = new System.Drawing.Size(329, 449);
            this.radGroupBox_type.TabIndex = 7;
            this.radGroupBox_type.Text = "配置产品物料";
            // 
            // listView_select_station
            // 
            this.listView_select_station.Location = new System.Drawing.Point(65, 102);
            this.listView_select_station.Name = "listView_select_station";
            this.listView_select_station.Size = new System.Drawing.Size(245, 335);
            this.listView_select_station.TabIndex = 6;
            this.listView_select_station.UseCompatibleStateImageBehavior = false;
            // 
            // radLabel1
            // 
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
            this.radLabel2.Location = new System.Drawing.Point(5, 78);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(54, 18);
            this.radLabel2.TabIndex = 5;
            this.radLabel2.Text = "物料编码";
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(77, 483);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(110, 24);
            this.radButton1.TabIndex = 8;
            this.radButton1.Text = "提交";
            // 
            // radButton2
            // 
            this.radButton2.Location = new System.Drawing.Point(231, 483);
            this.radButton2.Name = "radButton2";
            this.radButton2.Size = new System.Drawing.Size(110, 24);
            this.radButton2.TabIndex = 9;
            this.radButton2.Text = "取消";
            // 
            // ProductMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(360, 528);
            this.Controls.Add(this.radButton2);
            this.Controls.Add(this.radButton1);
            this.Controls.Add(this.radGroupBox_type);
            this.Name = "ProductMaterial";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "产品物料";
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox_type)).EndInit();
            this.radGroupBox_type.ResumeLayout(false);
            this.radGroupBox_type.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGroupBox radGroupBox_type;
        private System.Windows.Forms.ListView listView_select_station;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private System.Windows.Forms.ComboBox cb_type_no;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadButton radButton2;
    }
}
