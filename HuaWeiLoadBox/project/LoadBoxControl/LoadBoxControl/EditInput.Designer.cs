namespace LoadBoxControl
{
    partial class EditInput
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
            this.tb_input = new System.Windows.Forms.RichTextBox();
            this.btn_ok = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.btn_ok)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tb_input
            // 
            this.tb_input.Location = new System.Drawing.Point(67, 93);
            this.tb_input.Name = "tb_input";
            this.tb_input.Size = new System.Drawing.Size(155, 37);
            this.tb_input.TabIndex = 0;
            this.tb_input.Text = "";
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(77, 177);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(110, 24);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "确定";
            this.btn_ok.Click += new System.EventHandler(this.Btn_ok_Click);
            // 
            // EditInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 270);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.tb_input);
            this.Name = "EditInput";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "EditInput";
            ((System.ComponentModel.ISupportInitialize)(this.btn_ok)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox tb_input;
        private Telerik.WinControls.UI.RadButton btn_ok;
    }
}
