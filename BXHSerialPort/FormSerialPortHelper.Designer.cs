namespace BXHSerialPort
{
    partial class FormSerialPortHelper
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
            //if (disposing && (components != null))
            //{
            //    components.Dispose();
            //}
            //base.Dispose(disposing);
            Hide();
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOpen = new System.Windows.Forms.Button();
            this.cbbParity = new System.Windows.Forms.ComboBox();
            this.cbbStopBits = new System.Windows.Forms.ComboBox();
            this.cbbDataBits = new System.Windows.Forms.ComboBox();
            this.cbbBaudRate = new System.Windows.Forms.ComboBox();
            this.cbbComList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Font = new System.Drawing.Font("宋体", 10F);
            this.btnOpen.Location = new System.Drawing.Point(108, 165);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(89, 32);
            this.btnOpen.TabIndex = 19;
            this.btnOpen.Text = "打开串口";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // cbbParity
            // 
            this.cbbParity.FormattingEnabled = true;
            this.cbbParity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            this.cbbParity.Location = new System.Drawing.Point(52, 124);
            this.cbbParity.Name = "cbbParity";
            this.cbbParity.Size = new System.Drawing.Size(98, 20);
            this.cbbParity.TabIndex = 20;
            // 
            // cbbStopBits
            // 
            this.cbbStopBits.FormattingEnabled = true;
            this.cbbStopBits.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cbbStopBits.Location = new System.Drawing.Point(52, 98);
            this.cbbStopBits.Name = "cbbStopBits";
            this.cbbStopBits.Size = new System.Drawing.Size(98, 20);
            this.cbbStopBits.TabIndex = 18;
            // 
            // cbbDataBits
            // 
            this.cbbDataBits.FormattingEnabled = true;
            this.cbbDataBits.Items.AddRange(new object[] {
            "8",
            "7",
            "6"});
            this.cbbDataBits.Location = new System.Drawing.Point(52, 71);
            this.cbbDataBits.Name = "cbbDataBits";
            this.cbbDataBits.Size = new System.Drawing.Size(98, 20);
            this.cbbDataBits.TabIndex = 17;
            // 
            // cbbBaudRate
            // 
            this.cbbBaudRate.DisplayMember = "1";
            this.cbbBaudRate.FormattingEnabled = true;
            this.cbbBaudRate.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "43000",
            "56000",
            "57600",
            "115200"});
            this.cbbBaudRate.Location = new System.Drawing.Point(52, 42);
            this.cbbBaudRate.Name = "cbbBaudRate";
            this.cbbBaudRate.Size = new System.Drawing.Size(98, 20);
            this.cbbBaudRate.TabIndex = 16;
            this.cbbBaudRate.ValueMember = "1";
            // 
            // cbbComList
            // 
            this.cbbComList.DisplayMember = "1";
            this.cbbComList.FormattingEnabled = true;
            this.cbbComList.Location = new System.Drawing.Point(52, 16);
            this.cbbComList.Name = "cbbComList";
            this.cbbComList.Size = new System.Drawing.Size(98, 20);
            this.cbbComList.TabIndex = 15;
            this.cbbComList.Text = "COM6";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(156, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 11;
            this.label2.Text = "波特率";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(156, 127);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "校验位";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(156, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "端口";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(156, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "停止位";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(156, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "数据位";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(52, 165);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 21;
            this.pictureBox1.TabStop = false;
            // 
            // FormSerialPortHelper
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 206);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.cbbParity);
            this.Controls.Add(this.cbbStopBits);
            this.Controls.Add(this.cbbDataBits);
            this.Controls.Add(this.cbbBaudRate);
            this.Controls.Add(this.cbbComList);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Name = "FormSerialPortHelper";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "串口设置";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ComboBox cbbParity;
        private System.Windows.Forms.ComboBox cbbStopBits;
        private System.Windows.Forms.ComboBox cbbDataBits;
        private System.Windows.Forms.ComboBox cbbBaudRate;
        private System.Windows.Forms.ComboBox cbbComList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}