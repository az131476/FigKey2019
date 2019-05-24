namespace BXHSerialPort
{
    partial class frmSerialPort
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClearRev = new System.Windows.Forms.Button();
            this.btnClearSend = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtShowData = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblRevCount = new System.Windows.Forms.Label();
            this.lblRev = new System.Windows.Forms.Label();
            this.rbtnUnicode = new System.Windows.Forms.RadioButton();
            this.rbtnUTF8 = new System.Windows.Forms.RadioButton();
            this.rbtnASCII = new System.Windows.Forms.RadioButton();
            this.rbtnHex = new System.Windows.Forms.RadioButton();
            this.chkAutoLine = new System.Windows.Forms.CheckBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblSendCount = new System.Windows.Forms.Label();
            this.btnSend = new System.Windows.Forms.Button();
            this.rbtnSendUnicode = new System.Windows.Forms.RadioButton();
            this.lblSend = new System.Windows.Forms.Label();
            this.rbtnSendHex = new System.Windows.Forms.RadioButton();
            this.rbtnSendASCII = new System.Windows.Forms.RadioButton();
            this.rbtnSendUTF8 = new System.Windows.Forms.RadioButton();
            this.txtSendData = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.btnClearRev);
            this.panel1.Controls.Add(this.btnClearSend);
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 472);
            this.panel1.TabIndex = 0;
            // 
            // btnClearRev
            // 
            this.btnClearRev.Font = new System.Drawing.Font("宋体", 10F);
            this.btnClearRev.Location = new System.Drawing.Point(3, 418);
            this.btnClearRev.Name = "btnClearRev";
            this.btnClearRev.Size = new System.Drawing.Size(87, 32);
            this.btnClearRev.TabIndex = 11;
            this.btnClearRev.Text = "清空接收区";
            this.btnClearRev.UseVisualStyleBackColor = true;
            this.btnClearRev.Click += new System.EventHandler(this.btnClearRev_Click);
            // 
            // btnClearSend
            // 
            this.btnClearSend.Font = new System.Drawing.Font("宋体", 10F);
            this.btnClearSend.Location = new System.Drawing.Point(97, 419);
            this.btnClearSend.Name = "btnClearSend";
            this.btnClearSend.Size = new System.Drawing.Size(91, 32);
            this.btnClearSend.TabIndex = 10;
            this.btnClearSend.Text = "清空发送区";
            this.btnClearSend.UseVisualStyleBackColor = true;
            this.btnClearSend.Click += new System.EventHandler(this.btnClearSend_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Font = new System.Drawing.Font("宋体", 10F);
            this.btnOpen.Location = new System.Drawing.Point(84, 188);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(89, 32);
            this.btnOpen.TabIndex = 9;
            this.btnOpen.Text = "打开串口";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(34, 188);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 7;
            this.pictureBox1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbbParity);
            this.groupBox1.Controls.Add(this.cbbStopBits);
            this.groupBox1.Controls.Add(this.cbbDataBits);
            this.groupBox1.Controls.Add(this.cbbBaudRate);
            this.groupBox1.Controls.Add(this.cbbComList);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10F);
            this.groupBox1.Location = new System.Drawing.Point(11, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(178, 170);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "端口设置";
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
            this.cbbParity.Location = new System.Drawing.Point(18, 126);
            this.cbbParity.Name = "cbbParity";
            this.cbbParity.Size = new System.Drawing.Size(98, 21);
            this.cbbParity.TabIndex = 9;
            // 
            // cbbStopBits
            // 
            this.cbbStopBits.FormattingEnabled = true;
            this.cbbStopBits.Items.AddRange(new object[] {
            "1",
            "2",
            "3"});
            this.cbbStopBits.Location = new System.Drawing.Point(18, 100);
            this.cbbStopBits.Name = "cbbStopBits";
            this.cbbStopBits.Size = new System.Drawing.Size(98, 21);
            this.cbbStopBits.TabIndex = 8;
            // 
            // cbbDataBits
            // 
            this.cbbDataBits.FormattingEnabled = true;
            this.cbbDataBits.Items.AddRange(new object[] {
            "8",
            "7",
            "6"});
            this.cbbDataBits.Location = new System.Drawing.Point(18, 73);
            this.cbbDataBits.Name = "cbbDataBits";
            this.cbbDataBits.Size = new System.Drawing.Size(98, 21);
            this.cbbDataBits.TabIndex = 7;
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
            this.cbbBaudRate.Location = new System.Drawing.Point(18, 44);
            this.cbbBaudRate.Name = "cbbBaudRate";
            this.cbbBaudRate.Size = new System.Drawing.Size(98, 21);
            this.cbbBaudRate.TabIndex = 6;
            this.cbbBaudRate.ValueMember = "1";
            // 
            // cbbComList
            // 
            this.cbbComList.DisplayMember = "1";
            this.cbbComList.FormattingEnabled = true;
            this.cbbComList.Location = new System.Drawing.Point(18, 18);
            this.cbbComList.Name = "cbbComList";
            this.cbbComList.Size = new System.Drawing.Size(98, 21);
            this.cbbComList.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(122, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "波特率";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(122, 129);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 14);
            this.label5.TabIndex = 4;
            this.label5.Text = "校验位";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(122, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "端口";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(122, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "停止位";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(122, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "数据位";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.txtShowData);
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(200, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(505, 363);
            this.panel2.TabIndex = 1;
            // 
            // txtShowData
            // 
            this.txtShowData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtShowData.Location = new System.Drawing.Point(0, 41);
            this.txtShowData.Multiline = true;
            this.txtShowData.Name = "txtShowData";
            this.txtShowData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtShowData.Size = new System.Drawing.Size(503, 320);
            this.txtShowData.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblRevCount);
            this.groupBox2.Controls.Add(this.lblRev);
            this.groupBox2.Controls.Add(this.rbtnUnicode);
            this.groupBox2.Controls.Add(this.rbtnUTF8);
            this.groupBox2.Controls.Add(this.rbtnASCII);
            this.groupBox2.Controls.Add(this.rbtnHex);
            this.groupBox2.Controls.Add(this.chkAutoLine);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Font = new System.Drawing.Font("宋体", 10F);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(503, 41);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据接收区";
            // 
            // lblRevCount
            // 
            this.lblRevCount.AutoSize = true;
            this.lblRevCount.Location = new System.Drawing.Point(440, 18);
            this.lblRevCount.Name = "lblRevCount";
            this.lblRevCount.Size = new System.Drawing.Size(14, 14);
            this.lblRevCount.TabIndex = 7;
            this.lblRevCount.Text = "0";
            // 
            // lblRev
            // 
            this.lblRev.AutoSize = true;
            this.lblRev.Location = new System.Drawing.Point(350, 18);
            this.lblRev.Name = "lblRev";
            this.lblRev.Size = new System.Drawing.Size(84, 14);
            this.lblRev.TabIndex = 6;
            this.lblRev.Text = "接收字节数:";
            // 
            // rbtnUnicode
            // 
            this.rbtnUnicode.AutoSize = true;
            this.rbtnUnicode.Location = new System.Drawing.Point(255, 16);
            this.rbtnUnicode.Name = "rbtnUnicode";
            this.rbtnUnicode.Size = new System.Drawing.Size(74, 18);
            this.rbtnUnicode.TabIndex = 5;
            this.rbtnUnicode.Text = "Unicode";
            this.rbtnUnicode.UseVisualStyleBackColor = true;
            // 
            // rbtnUTF8
            // 
            this.rbtnUTF8.AutoSize = true;
            this.rbtnUTF8.Location = new System.Drawing.Point(196, 16);
            this.rbtnUTF8.Name = "rbtnUTF8";
            this.rbtnUTF8.Size = new System.Drawing.Size(60, 18);
            this.rbtnUTF8.TabIndex = 4;
            this.rbtnUTF8.Text = "UTF-8";
            this.rbtnUTF8.UseVisualStyleBackColor = true;
            // 
            // rbtnASCII
            // 
            this.rbtnASCII.AutoSize = true;
            this.rbtnASCII.Checked = true;
            this.rbtnASCII.Location = new System.Drawing.Point(137, 16);
            this.rbtnASCII.Name = "rbtnASCII";
            this.rbtnASCII.Size = new System.Drawing.Size(60, 18);
            this.rbtnASCII.TabIndex = 3;
            this.rbtnASCII.TabStop = true;
            this.rbtnASCII.Text = "ASCII";
            this.rbtnASCII.UseVisualStyleBackColor = true;
            // 
            // rbtnHex
            // 
            this.rbtnHex.AutoSize = true;
            this.rbtnHex.Location = new System.Drawing.Point(90, 16);
            this.rbtnHex.Name = "rbtnHex";
            this.rbtnHex.Size = new System.Drawing.Size(46, 18);
            this.rbtnHex.TabIndex = 2;
            this.rbtnHex.Text = "Hex";
            this.rbtnHex.UseVisualStyleBackColor = true;
            // 
            // chkAutoLine
            // 
            this.chkAutoLine.AutoSize = true;
            this.chkAutoLine.Location = new System.Drawing.Point(6, 16);
            this.chkAutoLine.Name = "chkAutoLine";
            this.chkAutoLine.Size = new System.Drawing.Size(82, 18);
            this.chkAutoLine.TabIndex = 1;
            this.chkAutoLine.Text = "自动换行";
            this.chkAutoLine.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.groupBox3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(200, 363);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(505, 109);
            this.panel3.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.txtSendData);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Font = new System.Drawing.Font("宋体", 10F);
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(503, 107);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "数据发送区";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblSendCount);
            this.groupBox4.Controls.Add(this.btnSend);
            this.groupBox4.Controls.Add(this.rbtnSendUnicode);
            this.groupBox4.Controls.Add(this.lblSend);
            this.groupBox4.Controls.Add(this.rbtnSendHex);
            this.groupBox4.Controls.Add(this.rbtnSendASCII);
            this.groupBox4.Controls.Add(this.rbtnSendUTF8);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 59);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(497, 45);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            // 
            // lblSendCount
            // 
            this.lblSendCount.AutoSize = true;
            this.lblSendCount.Location = new System.Drawing.Point(341, 15);
            this.lblSendCount.Name = "lblSendCount";
            this.lblSendCount.Size = new System.Drawing.Size(14, 14);
            this.lblSendCount.TabIndex = 8;
            this.lblSendCount.Text = "0";
            // 
            // btnSend
            // 
            this.btnSend.Font = new System.Drawing.Font("宋体", 10F);
            this.btnSend.Location = new System.Drawing.Point(414, 11);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(75, 30);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "手动发送";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // rbtnSendUnicode
            // 
            this.rbtnSendUnicode.AutoSize = true;
            this.rbtnSendUnicode.Location = new System.Drawing.Point(170, 11);
            this.rbtnSendUnicode.Name = "rbtnSendUnicode";
            this.rbtnSendUnicode.Size = new System.Drawing.Size(74, 18);
            this.rbtnSendUnicode.TabIndex = 9;
            this.rbtnSendUnicode.Text = "Unicode";
            this.rbtnSendUnicode.UseVisualStyleBackColor = true;
            // 
            // lblSend
            // 
            this.lblSend.AutoSize = true;
            this.lblSend.Location = new System.Drawing.Point(251, 14);
            this.lblSend.Name = "lblSend";
            this.lblSend.Size = new System.Drawing.Size(84, 14);
            this.lblSend.TabIndex = 7;
            this.lblSend.Text = "发送字节数:";
            // 
            // rbtnSendHex
            // 
            this.rbtnSendHex.AutoSize = true;
            this.rbtnSendHex.Location = new System.Drawing.Point(5, 11);
            this.rbtnSendHex.Name = "rbtnSendHex";
            this.rbtnSendHex.Size = new System.Drawing.Size(46, 18);
            this.rbtnSendHex.TabIndex = 6;
            this.rbtnSendHex.Text = "Hex";
            this.rbtnSendHex.UseVisualStyleBackColor = true;
            // 
            // rbtnSendASCII
            // 
            this.rbtnSendASCII.AutoSize = true;
            this.rbtnSendASCII.Checked = true;
            this.rbtnSendASCII.Location = new System.Drawing.Point(52, 11);
            this.rbtnSendASCII.Name = "rbtnSendASCII";
            this.rbtnSendASCII.Size = new System.Drawing.Size(60, 18);
            this.rbtnSendASCII.TabIndex = 7;
            this.rbtnSendASCII.TabStop = true;
            this.rbtnSendASCII.Text = "ASCII";
            this.rbtnSendASCII.UseVisualStyleBackColor = true;
            // 
            // rbtnSendUTF8
            // 
            this.rbtnSendUTF8.AutoSize = true;
            this.rbtnSendUTF8.Location = new System.Drawing.Point(111, 11);
            this.rbtnSendUTF8.Name = "rbtnSendUTF8";
            this.rbtnSendUTF8.Size = new System.Drawing.Size(60, 18);
            this.rbtnSendUTF8.TabIndex = 8;
            this.rbtnSendUTF8.Text = "UTF-8";
            this.rbtnSendUTF8.UseVisualStyleBackColor = true;
            // 
            // txtSendData
            // 
            this.txtSendData.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSendData.Location = new System.Drawing.Point(3, 19);
            this.txtSendData.Multiline = true;
            this.txtSendData.Name = "txtSendData";
            this.txtSendData.Size = new System.Drawing.Size(497, 40);
            this.txtSendData.TabIndex = 0;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Location = new System.Drawing.Point(11, 235);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(177, 159);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "主要功能：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "1.数据的发送、接收";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(95, 12);
            this.label7.TabIndex = 1;
            this.label7.Text = "2.Hex发送和显示";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 92);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(113, 12);
            this.label8.TabIndex = 2;
            this.label8.Text = "3.增加常用几种编码";
            // 
            // frmSerialPort
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(705, 472);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmSerialPort";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "JiYF笨小孩串口工具";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox1;
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
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkAutoLine;
        private System.Windows.Forms.RadioButton rbtnUnicode;
        private System.Windows.Forms.RadioButton rbtnUTF8;
        private System.Windows.Forms.RadioButton rbtnASCII;
        private System.Windows.Forms.RadioButton rbtnHex;
        private System.Windows.Forms.TextBox txtShowData;
        private System.Windows.Forms.TextBox txtSendData;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.RadioButton rbtnSendUnicode;
        private System.Windows.Forms.RadioButton rbtnSendUTF8;
        private System.Windows.Forms.RadioButton rbtnSendASCII;
        private System.Windows.Forms.RadioButton rbtnSendHex;
        private System.Windows.Forms.Label lblRev;
        private System.Windows.Forms.Label lblSend;
        private System.Windows.Forms.Label lblRevCount;
        private System.Windows.Forms.Label lblSendCount;
        private System.Windows.Forms.Button btnClearSend;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button btnClearRev;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
    }
}

