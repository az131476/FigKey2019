namespace CANManager.View
{
    partial class DeviceConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeviceConfig));
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.device_id = new System.Windows.Forms.ComboBox();
            this.device_channelID = new System.Windows.Forms.ComboBox();
            this.device_protocol = new System.Windows.Forms.ComboBox();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.applyOk = new Telerik.WinControls.UI.RadButton();
            this.applyCancel = new Telerik.WinControls.UI.RadButton();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.device_name = new System.Windows.Forms.ComboBox();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel6 = new Telerik.WinControls.UI.RadLabel();
            this.device_bauRate = new System.Windows.Forms.ComboBox();
            this.device_flags = new Telerik.WinControls.UI.RadTextBox();
            this.applyAstart = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applyOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applyCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.device_flags)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.applyAstart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(12, 81);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(77, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "设备索引号：";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(35, 120);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(54, 18);
            this.radLabel2.TabIndex = 1;
            this.radLabel2.Text = "通道号：";
            // 
            // device_id
            // 
            this.device_id.FormattingEnabled = true;
            this.device_id.Location = new System.Drawing.Point(109, 81);
            this.device_id.Name = "device_id";
            this.device_id.Size = new System.Drawing.Size(135, 20);
            this.device_id.TabIndex = 2;
            // 
            // device_channelID
            // 
            this.device_channelID.FormattingEnabled = true;
            this.device_channelID.Location = new System.Drawing.Point(109, 120);
            this.device_channelID.Name = "device_channelID";
            this.device_channelID.Size = new System.Drawing.Size(135, 20);
            this.device_channelID.TabIndex = 3;
            // 
            // device_protocol
            // 
            this.device_protocol.FormattingEnabled = true;
            this.device_protocol.Location = new System.Drawing.Point(364, 48);
            this.device_protocol.Name = "device_protocol";
            this.device_protocol.Size = new System.Drawing.Size(135, 20);
            this.device_protocol.TabIndex = 4;
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(294, 120);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(54, 18);
            this.radLabel3.TabIndex = 5;
            this.radLabel3.Text = "波特率：";
            // 
            // applyOk
            // 
            this.applyOk.Location = new System.Drawing.Point(344, 199);
            this.applyOk.Name = "applyOk";
            this.applyOk.Size = new System.Drawing.Size(63, 24);
            this.applyOk.TabIndex = 6;
            this.applyOk.Text = "应用";
            // 
            // applyCancel
            // 
            this.applyCancel.Location = new System.Drawing.Point(436, 199);
            this.applyCancel.Name = "applyCancel";
            this.applyCancel.Size = new System.Drawing.Size(63, 24);
            this.applyCancel.TabIndex = 7;
            this.applyCancel.Text = "取消";
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(26, 48);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(65, 18);
            this.radLabel4.TabIndex = 8;
            this.radLabel4.Text = "设备名称：";
            // 
            // device_name
            // 
            this.device_name.FormattingEnabled = true;
            this.device_name.Location = new System.Drawing.Point(109, 46);
            this.device_name.Name = "device_name";
            this.device_name.Size = new System.Drawing.Size(135, 20);
            this.device_name.TabIndex = 9;
            // 
            // radLabel5
            // 
            this.radLabel5.Location = new System.Drawing.Point(283, 48);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(65, 18);
            this.radLabel5.TabIndex = 10;
            this.radLabel5.Text = "协议类型：";
            // 
            // radLabel6
            // 
            this.radLabel6.Location = new System.Drawing.Point(259, 83);
            this.radLabel6.Name = "radLabel6";
            this.radLabel6.Size = new System.Drawing.Size(89, 18);
            this.radLabel6.TabIndex = 11;
            this.radLabel6.Text = "协议特定选项：";
            // 
            // device_bauRate
            // 
            this.device_bauRate.FormattingEnabled = true;
            this.device_bauRate.Location = new System.Drawing.Point(364, 120);
            this.device_bauRate.Name = "device_bauRate";
            this.device_bauRate.Size = new System.Drawing.Size(135, 20);
            this.device_bauRate.TabIndex = 13;
            // 
            // device_flags
            // 
            this.device_flags.Location = new System.Drawing.Point(364, 83);
            this.device_flags.Name = "device_flags";
            this.device_flags.Size = new System.Drawing.Size(135, 20);
            this.device_flags.TabIndex = 14;
            // 
            // applyAstart
            // 
            this.applyAstart.Location = new System.Drawing.Point(208, 199);
            this.applyAstart.Name = "applyAstart";
            this.applyAstart.Size = new System.Drawing.Size(105, 24);
            this.applyAstart.TabIndex = 15;
            this.applyAstart.Text = "应用并启动设备";
            // 
            // DeviceConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(540, 258);
            this.Controls.Add(this.applyAstart);
            this.Controls.Add(this.device_flags);
            this.Controls.Add(this.device_bauRate);
            this.Controls.Add(this.radLabel6);
            this.Controls.Add(this.radLabel5);
            this.Controls.Add(this.device_name);
            this.Controls.Add(this.radLabel4);
            this.Controls.Add(this.applyCancel);
            this.Controls.Add(this.applyOk);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.device_protocol);
            this.Controls.Add(this.device_channelID);
            this.Controls.Add(this.device_id);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DeviceConfig";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "设备配置";
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applyOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applyCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.device_flags)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.applyAstart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private System.Windows.Forms.ComboBox device_id;
        private System.Windows.Forms.ComboBox device_channelID;
        private System.Windows.Forms.ComboBox device_protocol;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadButton applyOk;
        private Telerik.WinControls.UI.RadButton applyCancel;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private System.Windows.Forms.ComboBox device_name;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadLabel radLabel6;
        private System.Windows.Forms.ComboBox device_bauRate;
        private Telerik.WinControls.UI.RadTextBox device_flags;
        private Telerik.WinControls.UI.RadButton applyAstart;
    }
}
