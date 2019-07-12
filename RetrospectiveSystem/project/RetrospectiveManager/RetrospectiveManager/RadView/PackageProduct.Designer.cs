namespace RetrospectiveManager.RadView
{
    partial class PackageProduct
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition3 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.tb_case_amount = new Telerik.WinControls.UI.RadTextBox();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.tb_sn = new Telerik.WinControls.UI.RadTextBox();
            this.btn_apply = new Telerik.WinControls.UI.RadButton();
            this.btn_cancel = new Telerik.WinControls.UI.RadButton();
            this.radGroupBox1 = new Telerik.WinControls.UI.RadGroupBox();
            this.cb_caseCode = new System.Windows.Forms.ComboBox();
            this.radGroupBox2 = new Telerik.WinControls.UI.RadGroupBox();
            this.btn_upLoad = new Telerik.WinControls.UI.RadButton();
            this.ch_auto_bingding = new Telerik.WinControls.UI.RadCheckBox();
            this.radGridView1 = new Telerik.WinControls.UI.RadGridView();
            this.cb_typeNo = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_case_amount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_sn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_apply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).BeginInit();
            this.radGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).BeginInit();
            this.radGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_upLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ch_auto_bingding)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(16, 36);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(54, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "箱子编码";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(289, 38);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(54, 18);
            this.radLabel2.TabIndex = 1;
            this.radLabel2.Text = "箱子容量";
            // 
            // tb_case_amount
            // 
            this.tb_case_amount.Location = new System.Drawing.Point(349, 38);
            this.tb_case_amount.Name = "tb_case_amount";
            this.tb_case_amount.Size = new System.Drawing.Size(121, 20);
            this.tb_case_amount.TabIndex = 3;
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(16, 38);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(54, 18);
            this.radLabel3.TabIndex = 4;
            this.radLabel3.Text = "产品条码";
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(289, 39);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(54, 18);
            this.radLabel4.TabIndex = 5;
            this.radLabel4.Text = "产品型号";
            // 
            // tb_sn
            // 
            this.tb_sn.Location = new System.Drawing.Point(76, 38);
            this.tb_sn.Name = "tb_sn";
            this.tb_sn.Size = new System.Drawing.Size(175, 20);
            this.tb_sn.TabIndex = 6;
            // 
            // btn_apply
            // 
            this.btn_apply.Location = new System.Drawing.Point(238, 510);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(120, 24);
            this.btn_apply.TabIndex = 8;
            this.btn_apply.Text = "绑定";
            this.btn_apply.Click += new System.EventHandler(this.Btn_apply_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(384, 510);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(120, 24);
            this.btn_cancel.TabIndex = 10;
            this.btn_cancel.Text = "解绑";
            // 
            // radGroupBox1
            // 
            this.radGroupBox1.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox1.Controls.Add(this.cb_caseCode);
            this.radGroupBox1.Controls.Add(this.radLabel1);
            this.radGroupBox1.Controls.Add(this.radLabel2);
            this.radGroupBox1.Controls.Add(this.tb_case_amount);
            this.radGroupBox1.HeaderText = "箱子信息";
            this.radGroupBox1.Location = new System.Drawing.Point(15, 12);
            this.radGroupBox1.Name = "radGroupBox1";
            this.radGroupBox1.Size = new System.Drawing.Size(489, 65);
            this.radGroupBox1.TabIndex = 11;
            this.radGroupBox1.Text = "箱子信息";
            // 
            // cb_caseCode
            // 
            this.cb_caseCode.FormattingEnabled = true;
            this.cb_caseCode.Location = new System.Drawing.Point(76, 36);
            this.cb_caseCode.Name = "cb_caseCode";
            this.cb_caseCode.Size = new System.Drawing.Size(175, 20);
            this.cb_caseCode.TabIndex = 4;
            // 
            // radGroupBox2
            // 
            this.radGroupBox2.AccessibleRole = System.Windows.Forms.AccessibleRole.Grouping;
            this.radGroupBox2.Controls.Add(this.cb_typeNo);
            this.radGroupBox2.Controls.Add(this.btn_upLoad);
            this.radGroupBox2.Controls.Add(this.ch_auto_bingding);
            this.radGroupBox2.Controls.Add(this.radLabel3);
            this.radGroupBox2.Controls.Add(this.radLabel4);
            this.radGroupBox2.Controls.Add(this.tb_sn);
            this.radGroupBox2.HeaderText = "产品信息";
            this.radGroupBox2.Location = new System.Drawing.Point(15, 99);
            this.radGroupBox2.Name = "radGroupBox2";
            this.radGroupBox2.Size = new System.Drawing.Size(489, 99);
            this.radGroupBox2.TabIndex = 12;
            this.radGroupBox2.Text = "产品信息";
            // 
            // btn_upLoad
            // 
            this.btn_upLoad.Location = new System.Drawing.Point(349, 70);
            this.btn_upLoad.Name = "btn_upLoad";
            this.btn_upLoad.Size = new System.Drawing.Size(121, 24);
            this.btn_upLoad.TabIndex = 15;
            this.btn_upLoad.Text = "上传图片";
            this.btn_upLoad.Click += new System.EventHandler(this.Btn_upLoad_Click);
            // 
            // ch_auto_bingding
            // 
            this.ch_auto_bingding.Location = new System.Drawing.Point(76, 76);
            this.ch_auto_bingding.Name = "ch_auto_bingding";
            this.ch_auto_bingding.Size = new System.Drawing.Size(91, 18);
            this.ch_auto_bingding.TabIndex = 14;
            this.ch_auto_bingding.Text = "扫码自动绑定";
            // 
            // radGridView1
            // 
            this.radGridView1.Location = new System.Drawing.Point(15, 204);
            // 
            // 
            // 
            this.radGridView1.MasterTemplate.ViewDefinition = tableViewDefinition3;
            this.radGridView1.Name = "radGridView1";
            this.radGridView1.Size = new System.Drawing.Size(489, 300);
            this.radGridView1.TabIndex = 13;
            // 
            // cb_typeNo
            // 
            this.cb_typeNo.FormattingEnabled = true;
            this.cb_typeNo.Location = new System.Drawing.Point(349, 38);
            this.cb_typeNo.Name = "cb_typeNo";
            this.cb_typeNo.Size = new System.Drawing.Size(121, 20);
            this.cb_typeNo.TabIndex = 16;
            // 
            // PackageProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 546);
            this.Controls.Add(this.radGridView1);
            this.Controls.Add(this.radGroupBox2);
            this.Controls.Add(this.radGroupBox1);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_apply);
            this.Name = "PackageProduct";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "产品装箱";
            this.Load += new System.EventHandler(this.PackageProduct_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_case_amount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tb_sn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_apply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btn_cancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox1)).EndInit();
            this.radGroupBox1.ResumeLayout(false);
            this.radGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radGroupBox2)).EndInit();
            this.radGroupBox2.ResumeLayout(false);
            this.radGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btn_upLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ch_auto_bingding)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadTextBox tb_case_amount;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadTextBox tb_sn;
        private Telerik.WinControls.UI.RadButton btn_apply;
        private Telerik.WinControls.UI.RadButton btn_cancel;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox1;
        private Telerik.WinControls.UI.RadGroupBox radGroupBox2;
        private Telerik.WinControls.UI.RadCheckBox ch_auto_bingding;
        private Telerik.WinControls.UI.RadButton btn_upLoad;
        private Telerik.WinControls.UI.RadGridView radGridView1;
        private System.Windows.Forms.ComboBox cb_caseCode;
        private System.Windows.Forms.ComboBox cb_typeNo;
    }
}
