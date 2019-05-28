namespace BXHSerialPort
{
    partial class FormTestNew
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTestNew));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxSerialNumber = new System.Windows.Forms.TextBox();
            this.labelTestTime = new System.Windows.Forms.Label();
            this.labelVIN = new System.Windows.Forms.Label();
            this.labelSerialNumber = new System.Windows.Forms.Label();
            this.labelTestTimeV = new System.Windows.Forms.Label();
            this.labelVINV = new System.Windows.Forms.Label();
            this.labelSerialNumberV = new System.Windows.Forms.Label();
            this.labelProductNameV = new System.Windows.Forms.Label();
            this.labelProductName = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelFail = new System.Windows.Forms.Label();
            this.labelPass = new System.Windows.Forms.Label();
            this.labelFailV = new System.Windows.Forms.Label();
            this.labelPassV = new System.Windows.Forms.Label();
            this.labelTotalV = new System.Windows.Forms.Label();
            this.labelTotal = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.labelResult = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.layeredLabelTime = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panelProductSetting = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelProductSetting = new System.Windows.Forms.Label();
            this.panelCycleTest = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.labelCycleTest = new System.Windows.Forms.Label();
            this.panelSingleTest = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.labelSingleTest = new System.Windows.Forms.Label();
            this.panelStopTest = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.labelStopTest = new System.Windows.Forms.Label();
            this.panelExitSystem = new System.Windows.Forms.Panel();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.labelExitSystem = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.toolStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panelProductSetting.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelCycleTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.panelSingleTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.panelStopTest.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.panelExitSystem.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripButton5});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(668, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "用户切换";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "导出到EXCEL";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "串口连接";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Text = "读取数据";
            this.toolStripButton4.Click += new System.EventHandler(this.toolStripButton4_Click);
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "启动按钮串口";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.textBoxSerialNumber);
            this.groupBox2.Controls.Add(this.labelTestTime);
            this.groupBox2.Controls.Add(this.labelVIN);
            this.groupBox2.Controls.Add(this.labelSerialNumber);
            this.groupBox2.Controls.Add(this.labelTestTimeV);
            this.groupBox2.Controls.Add(this.labelVINV);
            this.groupBox2.Controls.Add(this.labelSerialNumberV);
            this.groupBox2.Controls.Add(this.labelProductNameV);
            this.groupBox2.Controls.Add(this.labelProductName);
            this.groupBox2.Location = new System.Drawing.Point(181, 99);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(247, 100);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            // 
            // textBoxSerialNumber
            // 
            this.textBoxSerialNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSerialNumber.Location = new System.Drawing.Point(74, 32);
            this.textBoxSerialNumber.Name = "textBoxSerialNumber";
            this.textBoxSerialNumber.Size = new System.Drawing.Size(167, 21);
            this.textBoxSerialNumber.TabIndex = 1;
            // 
            // labelTestTime
            // 
            this.labelTestTime.AutoSize = true;
            this.labelTestTime.Location = new System.Drawing.Point(5, 77);
            this.labelTestTime.Name = "labelTestTime";
            this.labelTestTime.Size = new System.Drawing.Size(65, 12);
            this.labelTestTime.TabIndex = 0;
            this.labelTestTime.Text = "测试时间：";
            // 
            // labelVIN
            // 
            this.labelVIN.AutoSize = true;
            this.labelVIN.Location = new System.Drawing.Point(23, 57);
            this.labelVIN.Name = "labelVIN";
            this.labelVIN.Size = new System.Drawing.Size(47, 12);
            this.labelVIN.TabIndex = 0;
            this.labelVIN.Text = "VIN码：";
            // 
            // labelSerialNumber
            // 
            this.labelSerialNumber.AutoSize = true;
            this.labelSerialNumber.Location = new System.Drawing.Point(17, 37);
            this.labelSerialNumber.Name = "labelSerialNumber";
            this.labelSerialNumber.Size = new System.Drawing.Size(53, 12);
            this.labelSerialNumber.TabIndex = 0;
            this.labelSerialNumber.Text = "序列号：";
            // 
            // labelTestTimeV
            // 
            this.labelTestTimeV.AutoSize = true;
            this.labelTestTimeV.Location = new System.Drawing.Point(72, 77);
            this.labelTestTimeV.Name = "labelTestTimeV";
            this.labelTestTimeV.Size = new System.Drawing.Size(23, 12);
            this.labelTestTimeV.TabIndex = 0;
            this.labelTestTimeV.Text = "5.6";
            // 
            // labelVINV
            // 
            this.labelVINV.AutoSize = true;
            this.labelVINV.Location = new System.Drawing.Point(72, 57);
            this.labelVINV.Name = "labelVINV";
            this.labelVINV.Size = new System.Drawing.Size(29, 12);
            this.labelVINV.TabIndex = 0;
            this.labelVINV.Text = "2213";
            // 
            // labelSerialNumberV
            // 
            this.labelSerialNumberV.AutoSize = true;
            this.labelSerialNumberV.Location = new System.Drawing.Point(72, 37);
            this.labelSerialNumberV.Name = "labelSerialNumberV";
            this.labelSerialNumberV.Size = new System.Drawing.Size(41, 12);
            this.labelSerialNumberV.TabIndex = 0;
            this.labelSerialNumberV.Text = "123456";
            // 
            // labelProductNameV
            // 
            this.labelProductNameV.AutoSize = true;
            this.labelProductNameV.Location = new System.Drawing.Point(72, 17);
            this.labelProductNameV.Name = "labelProductNameV";
            this.labelProductNameV.Size = new System.Drawing.Size(65, 12);
            this.labelProductNameV.TabIndex = 0;
            this.labelProductNameV.Text = "无级变速箱";
            // 
            // labelProductName
            // 
            this.labelProductName.AutoSize = true;
            this.labelProductName.Location = new System.Drawing.Point(5, 17);
            this.labelProductName.Name = "labelProductName";
            this.labelProductName.Size = new System.Drawing.Size(65, 12);
            this.labelProductName.TabIndex = 0;
            this.labelProductName.Text = "产品名称：";
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.labelFail);
            this.groupBox3.Controls.Add(this.labelPass);
            this.groupBox3.Controls.Add(this.labelFailV);
            this.groupBox3.Controls.Add(this.labelPassV);
            this.groupBox3.Controls.Add(this.labelTotalV);
            this.groupBox3.Controls.Add(this.labelTotal);
            this.groupBox3.Location = new System.Drawing.Point(456, 99);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(200, 100);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            // 
            // labelFail
            // 
            this.labelFail.AutoSize = true;
            this.labelFail.Location = new System.Drawing.Point(2, 69);
            this.labelFail.Name = "labelFail";
            this.labelFail.Size = new System.Drawing.Size(53, 12);
            this.labelFail.TabIndex = 0;
            this.labelFail.Text = "不合格：";
            // 
            // labelPass
            // 
            this.labelPass.AutoSize = true;
            this.labelPass.Location = new System.Drawing.Point(14, 49);
            this.labelPass.Name = "labelPass";
            this.labelPass.Size = new System.Drawing.Size(41, 12);
            this.labelPass.TabIndex = 0;
            this.labelPass.Text = "合格：";
            // 
            // labelFailV
            // 
            this.labelFailV.AutoSize = true;
            this.labelFailV.Location = new System.Drawing.Point(61, 69);
            this.labelFailV.Name = "labelFailV";
            this.labelFailV.Size = new System.Drawing.Size(11, 12);
            this.labelFailV.TabIndex = 0;
            this.labelFailV.Text = "0";
            // 
            // labelPassV
            // 
            this.labelPassV.AutoSize = true;
            this.labelPassV.Location = new System.Drawing.Point(61, 49);
            this.labelPassV.Name = "labelPassV";
            this.labelPassV.Size = new System.Drawing.Size(11, 12);
            this.labelPassV.TabIndex = 0;
            this.labelPassV.Text = "0";
            // 
            // labelTotalV
            // 
            this.labelTotalV.AutoSize = true;
            this.labelTotalV.Location = new System.Drawing.Point(61, 29);
            this.labelTotalV.Name = "labelTotalV";
            this.labelTotalV.Size = new System.Drawing.Size(11, 12);
            this.labelTotalV.TabIndex = 0;
            this.labelTotalV.Text = "0";
            // 
            // labelTotal
            // 
            this.labelTotal.AutoSize = true;
            this.labelTotal.Location = new System.Drawing.Point(14, 29);
            this.labelTotal.Name = "labelTotal";
            this.labelTotal.Size = new System.Drawing.Size(41, 12);
            this.labelTotal.TabIndex = 0;
            this.labelTotal.Text = "总数：";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.BackColor = System.Drawing.Color.PaleGreen;
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.ColumnWidth = 200;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.HorizontalExtent = 200;
            this.checkedListBox1.HorizontalScrollbar = true;
            this.checkedListBox1.Items.AddRange(new object[] {
            "LinePCtl_iSpSoln_M:0x0000-0x002F",
            "PrimCtl_iSpSoln_M:0x0030-0x006F",
            "SecCtl_iSpSoln_M:0x0070-0x00AF",
            "TccCtl_iSpSoln_M:0x00B0-0x00DF",
            "CluCtl_iSpDrvSoln_M:0x00E0-0x010F",
            "CluCtl_iSpRvsSoln_M:0x0110-0x013F",
            "EndModelPartNumber:0x01E0-0x01EF",
            "SUB-ROM Version:0x01F0-0x01FF"});
            this.checkedListBox1.Location = new System.Drawing.Point(6, 20);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(131, 164);
            this.checkedListBox1.TabIndex = 0;
            this.checkedListBox1.SelectedValueChanged += new System.EventHandler(this.checkedListBox1_SelectedValueChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.labelResult);
            this.groupBox4.Controls.Add(this.flowLayoutPanel1);
            this.groupBox4.Controls.Add(this.checkedListBox1);
            this.groupBox4.Location = new System.Drawing.Point(181, 205);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(475, 253);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            // 
            // labelResult
            // 
            this.labelResult.AutoSize = true;
            this.labelResult.Font = new System.Drawing.Font("宋体", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelResult.Location = new System.Drawing.Point(6, 198);
            this.labelResult.Name = "labelResult";
            this.labelResult.Size = new System.Drawing.Size(133, 29);
            this.labelResult.TabIndex = 2;
            this.labelResult.Text = "准备刷写";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoScroll = true;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(143, 20);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(326, 227);
            this.flowLayoutPanel1.TabIndex = 1;
            this.flowLayoutPanel1.WrapContents = false;
            this.flowLayoutPanel1.Resize += new System.EventHandler(this.flowLayoutPanel1_Resize);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("panel1.BackgroundImage")));
            this.panel1.Controls.Add(this.layeredLabelTime);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(668, 65);
            this.panel1.TabIndex = 4;
            // 
            // layeredLabelTime
            // 
            this.layeredLabelTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.layeredLabelTime.AutoSize = true;
            this.layeredLabelTime.Font = new System.Drawing.Font("华文琥珀", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.layeredLabelTime.ForeColor = System.Drawing.Color.Gold;
            this.layeredLabelTime.Location = new System.Drawing.Point(533, 17);
            this.layeredLabelTime.Name = "layeredLabelTime";
            this.layeredLabelTime.Size = new System.Drawing.Size(93, 30);
            this.layeredLabelTime.TabIndex = 1;
            this.layeredLabelTime.Text = "12:11";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("华文琥珀", 21.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Gold;
            this.label1.Location = new System.Drawing.Point(18, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(188, 30);
            this.label1.TabIndex = 0;
            this.label1.Text = "SUBROM刷写";
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("statusStrip1.BackgroundImage")));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 492);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(668, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BackColor = System.Drawing.Color.Transparent;
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Gold;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(71, 17);
            this.toolStripStatusLabel1.Text = "角色：User";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 800;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panelProductSetting
            // 
            this.panelProductSetting.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panelProductSetting.Controls.Add(this.pictureBox1);
            this.panelProductSetting.Controls.Add(this.labelProductSetting);
            this.panelProductSetting.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelProductSetting.Location = new System.Drawing.Point(12, 107);
            this.panelProductSetting.Name = "panelProductSetting";
            this.panelProductSetting.Size = new System.Drawing.Size(159, 41);
            this.panelProductSetting.TabIndex = 1;
            this.panelProductSetting.Click += new System.EventHandler(this.panelProductSetting_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(43, 41);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // labelProductSetting
            // 
            this.labelProductSetting.AutoSize = true;
            this.labelProductSetting.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelProductSetting.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelProductSetting.Location = new System.Drawing.Point(62, 13);
            this.labelProductSetting.Name = "labelProductSetting";
            this.labelProductSetting.Size = new System.Drawing.Size(76, 16);
            this.labelProductSetting.TabIndex = 0;
            this.labelProductSetting.Text = "产品配置";
            // 
            // panelCycleTest
            // 
            this.panelCycleTest.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panelCycleTest.Controls.Add(this.pictureBox2);
            this.panelCycleTest.Controls.Add(this.labelCycleTest);
            this.panelCycleTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelCycleTest.Location = new System.Drawing.Point(12, 181);
            this.panelCycleTest.Name = "panelCycleTest";
            this.panelCycleTest.Size = new System.Drawing.Size(159, 41);
            this.panelCycleTest.TabIndex = 1;
            this.panelCycleTest.Click += new System.EventHandler(this.panelCycleTest_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox2.BackgroundImage")));
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(43, 41);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // labelCycleTest
            // 
            this.labelCycleTest.AutoSize = true;
            this.labelCycleTest.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCycleTest.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelCycleTest.Location = new System.Drawing.Point(62, 13);
            this.labelCycleTest.Name = "labelCycleTest";
            this.labelCycleTest.Size = new System.Drawing.Size(76, 16);
            this.labelCycleTest.TabIndex = 0;
            this.labelCycleTest.Text = "循环测试";
            this.labelCycleTest.Click += new System.EventHandler(this.panelCycleTest_Click);
            // 
            // panelSingleTest
            // 
            this.panelSingleTest.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panelSingleTest.Controls.Add(this.pictureBox3);
            this.panelSingleTest.Controls.Add(this.labelSingleTest);
            this.panelSingleTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelSingleTest.Location = new System.Drawing.Point(12, 255);
            this.panelSingleTest.Name = "panelSingleTest";
            this.panelSingleTest.Size = new System.Drawing.Size(159, 41);
            this.panelSingleTest.TabIndex = 1;
            this.panelSingleTest.Click += new System.EventHandler(this.panelSingleTest_Click);
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox3.BackgroundImage")));
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox3.Location = new System.Drawing.Point(0, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(43, 41);
            this.pictureBox3.TabIndex = 1;
            this.pictureBox3.TabStop = false;
            // 
            // labelSingleTest
            // 
            this.labelSingleTest.AutoSize = true;
            this.labelSingleTest.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelSingleTest.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelSingleTest.Location = new System.Drawing.Point(62, 13);
            this.labelSingleTest.Name = "labelSingleTest";
            this.labelSingleTest.Size = new System.Drawing.Size(76, 16);
            this.labelSingleTest.TabIndex = 0;
            this.labelSingleTest.Text = "单件测试";
            this.labelSingleTest.Click += new System.EventHandler(this.panelSingleTest_Click);
            // 
            // panelStopTest
            // 
            this.panelStopTest.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panelStopTest.Controls.Add(this.pictureBox4);
            this.panelStopTest.Controls.Add(this.labelStopTest);
            this.panelStopTest.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelStopTest.Location = new System.Drawing.Point(12, 329);
            this.panelStopTest.Name = "panelStopTest";
            this.panelStopTest.Size = new System.Drawing.Size(159, 41);
            this.panelStopTest.TabIndex = 1;
            this.panelStopTest.Click += new System.EventHandler(this.panelStopTest_Click);
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox4.BackgroundImage")));
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox4.Location = new System.Drawing.Point(0, 0);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(43, 41);
            this.pictureBox4.TabIndex = 1;
            this.pictureBox4.TabStop = false;
            // 
            // labelStopTest
            // 
            this.labelStopTest.AutoSize = true;
            this.labelStopTest.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelStopTest.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelStopTest.Location = new System.Drawing.Point(62, 13);
            this.labelStopTest.Name = "labelStopTest";
            this.labelStopTest.Size = new System.Drawing.Size(76, 16);
            this.labelStopTest.TabIndex = 0;
            this.labelStopTest.Text = "终止测试";
            this.labelStopTest.Click += new System.EventHandler(this.panelStopTest_Click);
            // 
            // panelExitSystem
            // 
            this.panelExitSystem.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panelExitSystem.Controls.Add(this.pictureBox5);
            this.panelExitSystem.Controls.Add(this.labelExitSystem);
            this.panelExitSystem.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panelExitSystem.Location = new System.Drawing.Point(12, 403);
            this.panelExitSystem.Name = "panelExitSystem";
            this.panelExitSystem.Size = new System.Drawing.Size(159, 41);
            this.panelExitSystem.TabIndex = 1;
            this.panelExitSystem.Click += new System.EventHandler(this.panelExitSystem_Click);
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox5.BackgroundImage")));
            this.pictureBox5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.pictureBox5.Location = new System.Drawing.Point(0, 0);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(43, 41);
            this.pictureBox5.TabIndex = 1;
            this.pictureBox5.TabStop = false;
            // 
            // labelExitSystem
            // 
            this.labelExitSystem.AutoSize = true;
            this.labelExitSystem.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelExitSystem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelExitSystem.Location = new System.Drawing.Point(62, 13);
            this.labelExitSystem.Name = "labelExitSystem";
            this.labelExitSystem.Size = new System.Drawing.Size(76, 16);
            this.labelExitSystem.TabIndex = 0;
            this.labelExitSystem.Text = "退出系统";
            this.labelExitSystem.Click += new System.EventHandler(this.panelExitSystem_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 473);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 16);
            this.checkBox1.TabIndex = 6;
            this.checkBox1.Text = "手动输入序列号";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // FormTestNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(668, 514);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.panelExitSystem);
            this.Controls.Add(this.panelStopTest);
            this.Controls.Add(this.panelSingleTest);
            this.Controls.Add(this.panelCycleTest);
            this.Controls.Add(this.panelProductSetting);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormTestNew";
            this.Text = "SUBROM PROGRAMMING";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTestNew_FormClosing);
            this.Load += new System.EventHandler(this.FormTestNew_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panelProductSetting.ResumeLayout(false);
            this.panelProductSetting.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelCycleTest.ResumeLayout(false);
            this.panelCycleTest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.panelSingleTest.ResumeLayout(false);
            this.panelSingleTest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.panelStopTest.ResumeLayout(false);
            this.panelStopTest.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.panelExitSystem.ResumeLayout(false);
            this.panelExitSystem.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label labelTestTime;
        private System.Windows.Forms.Label labelVIN;
        private System.Windows.Forms.Label labelSerialNumber;
        private System.Windows.Forms.Label labelProductName;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelFail;
        private System.Windows.Forms.Label labelPass;
        private System.Windows.Forms.Label labelTotal;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Label layeredLabelTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panelProductSetting;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelProductSetting;
        private System.Windows.Forms.Panel panelCycleTest;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label labelCycleTest;
        private System.Windows.Forms.Panel panelSingleTest;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Label labelSingleTest;
        private System.Windows.Forms.Panel panelStopTest;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Label labelStopTest;
        private System.Windows.Forms.Panel panelExitSystem;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label labelExitSystem;
        private System.Windows.Forms.Label labelTestTimeV;
        private System.Windows.Forms.Label labelVINV;
        private System.Windows.Forms.Label labelSerialNumberV;
        private System.Windows.Forms.Label labelProductNameV;
        private System.Windows.Forms.Label labelFailV;
        private System.Windows.Forms.Label labelPassV;
        private System.Windows.Forms.Label labelTotalV;
        private System.Windows.Forms.TextBox textBoxSerialNumber;
        private System.Windows.Forms.Label labelResult;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}