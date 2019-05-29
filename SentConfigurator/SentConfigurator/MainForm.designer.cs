namespace SentConfigurator
{
    partial class MainForm
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition1 = new Telerik.WinControls.UI.TableViewDefinition();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tool_dev_status = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.status_sendcount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.status_receivecount = new System.Windows.Forms.ToolStripStatusLabel();
            this.radMenuItem1 = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_open_file = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_save_as = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_app_exist = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem4 = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_searchDev = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_sent_cfg_read = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_sent_cfg_write = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem7 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem8 = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenuItem9 = new Telerik.WinControls.UI.RadMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tool_open_config = new System.Windows.Forms.ToolStripButton();
            this.tool_saveas = new System.Windows.Forms.ToolStripButton();
            this.tool_readcfg = new System.Windows.Forms.ToolStripButton();
            this.tool_writecfg = new System.Windows.Forms.ToolStripButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lbx_group_count_limit = new Telerik.WinControls.UI.RadLabel();
            this.btn_cleardgv = new System.Windows.Forms.Button();
            this.dgv_groupdata = new Telerik.WinControls.UI.RadGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.cob_group_num = new System.Windows.Forms.ComboBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btn_sig_cfg_read = new System.Windows.Forms.Button();
            this.btn_sig_cfg_set = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btn_open = new System.Windows.Forms.Button();
            this.cb_handshake = new System.Windows.Forms.ComboBox();
            this.cb_stop = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbx_recon = new System.Windows.Forms.CheckBox();
            this.cb_check = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.ledControl1 = new LEDLib.LEDControl();
            this.cb_data = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cb_port = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cb_baud = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rdb_dec = new Telerik.WinControls.UI.RadRadioButton();
            this.rdb_hex = new Telerik.WinControls.UI.RadRadioButton();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.radLabel8 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.lbx_quickd2_limit = new Telerik.WinControls.UI.RadLabel();
            this.tbx_quicksig_data2 = new System.Windows.Forms.TextBox();
            this.cob_quicksig_type = new System.Windows.Forms.ComboBox();
            this.tbx_quicksig_data1 = new System.Windows.Forms.TextBox();
            this.lbx_quickd1_limit = new Telerik.WinControls.UI.RadLabel();
            this.radLabel10 = new Telerik.WinControls.UI.RadLabel();
            this.lbx_hexorder_notes = new System.Windows.Forms.Label();
            this.chx_hex_order = new Telerik.WinControls.UI.RadCheckBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.tbx_timeframe = new System.Windows.Forms.TextBox();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.cob_dataframe_type = new System.Windows.Forms.ComboBox();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.cob_battery_state = new System.Windows.Forms.ComboBox();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.cob_serial_msg = new System.Windows.Forms.ComboBox();
            this.lbx_ticksLimit = new Telerik.WinControls.UI.RadLabel();
            this.radMenuItem11 = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_help = new Telerik.WinControls.UI.RadMenuItem();
            this.menu_abort = new Telerik.WinControls.UI.RadMenuItem();
            this.radMenu1 = new Telerik.WinControls.UI.RadMenu();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_group_count_limit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_groupdata)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_groupdata.MasterTemplate)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdb_dec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdb_hex)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel8)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_quickd2_limit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_quickd1_limit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chx_hex_order)).BeginInit();
            this.groupBox7.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_ticksLimit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel3,
            this.tool_dev_status,
            this.toolStripStatusLabel1,
            this.status_sendcount,
            this.toolStripStatusLabel2,
            this.status_receivecount});
            this.statusStrip1.Location = new System.Drawing.Point(0, 889);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(830, 22);
            this.statusStrip1.TabIndex = 27;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(68, 17);
            this.toolStripStatusLabel3.Text = "设备状态：";
            // 
            // tool_dev_status
            // 
            this.tool_dev_status.Name = "tool_dev_status";
            this.tool_dev_status.Size = new System.Drawing.Size(32, 17);
            this.tool_dev_status.Text = "离线";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(80, 17);
            this.toolStripStatusLabel1.Text = "发送字节数：";
            // 
            // status_sendcount
            // 
            this.status_sendcount.Name = "status_sendcount";
            this.status_sendcount.Size = new System.Drawing.Size(15, 17);
            this.status_sendcount.Text = "0";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(80, 17);
            this.toolStripStatusLabel2.Text = "接收字节数：";
            // 
            // status_receivecount
            // 
            this.status_receivecount.Name = "status_receivecount";
            this.status_receivecount.Size = new System.Drawing.Size(15, 17);
            this.status_receivecount.Text = "0";
            // 
            // radMenuItem1
            // 
            this.radMenuItem1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menu_open_file,
            this.menu_save_as,
            this.menu_app_exist});
            this.radMenuItem1.Name = "radMenuItem1";
            this.radMenuItem1.Text = "文件";
            // 
            // menu_open_file
            // 
            this.menu_open_file.Name = "menu_open_file";
            this.menu_open_file.Text = "打开配置文件";
            this.menu_open_file.Click += new System.EventHandler(this.Menu_open_file_Click);
            // 
            // menu_save_as
            // 
            this.menu_save_as.Name = "menu_save_as";
            this.menu_save_as.Text = "保存配置文件";
            this.menu_save_as.Click += new System.EventHandler(this.Menu_save_as_Click);
            // 
            // menu_app_exist
            // 
            this.menu_app_exist.Name = "menu_app_exist";
            this.menu_app_exist.Text = "退出";
            this.menu_app_exist.Click += new System.EventHandler(this.Menu_app_exist_Click);
            // 
            // radMenuItem4
            // 
            this.radMenuItem4.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menu_searchDev,
            this.menu_sent_cfg_read,
            this.menu_sent_cfg_write});
            this.radMenuItem4.Name = "radMenuItem4";
            this.radMenuItem4.Text = "操作";
            // 
            // menu_searchDev
            // 
            this.menu_searchDev.Name = "menu_searchDev";
            this.menu_searchDev.Text = "查询设备";
            this.menu_searchDev.Click += new System.EventHandler(this.RadMenuItem5_Click);
            // 
            // menu_sent_cfg_read
            // 
            this.menu_sent_cfg_read.Name = "menu_sent_cfg_read";
            this.menu_sent_cfg_read.Text = "SENT配置读取";
            this.menu_sent_cfg_read.Click += new System.EventHandler(this.Menu_sent_cfg_read_Click);
            // 
            // menu_sent_cfg_write
            // 
            this.menu_sent_cfg_write.Name = "menu_sent_cfg_write";
            this.menu_sent_cfg_write.Text = "SENT配置写入";
            this.menu_sent_cfg_write.Click += new System.EventHandler(this.Menu_sent_cfg_write_Click);
            // 
            // radMenuItem7
            // 
            this.radMenuItem7.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem8,
            this.radMenuItem9});
            this.radMenuItem7.Name = "radMenuItem7";
            this.radMenuItem7.Text = "工具";
            // 
            // radMenuItem8
            // 
            this.radMenuItem8.Name = "radMenuItem8";
            this.radMenuItem8.Text = "设备信息";
            // 
            // radMenuItem9
            // 
            this.radMenuItem9.Name = "radMenuItem9";
            this.radMenuItem9.Text = "扫描设备";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tool_open_config,
            this.tool_saveas,
            this.tool_readcfg,
            this.tool_writecfg});
            this.toolStrip1.Location = new System.Drawing.Point(0, 20);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(830, 25);
            this.toolStrip1.TabIndex = 34;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tool_open_config
            // 
            this.tool_open_config.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tool_open_config.Image = global::SentConfigurator.Properties.Resources.folder_vertical_open;
            this.tool_open_config.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tool_open_config.Name = "tool_open_config";
            this.tool_open_config.Size = new System.Drawing.Size(23, 22);
            this.tool_open_config.Text = "打开配置文件";
            this.tool_open_config.Click += new System.EventHandler(this.Tool_open_config_Click);
            // 
            // tool_saveas
            // 
            this.tool_saveas.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tool_saveas.Image = global::SentConfigurator.Properties.Resources.save;
            this.tool_saveas.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tool_saveas.Name = "tool_saveas";
            this.tool_saveas.Size = new System.Drawing.Size(23, 22);
            this.tool_saveas.Text = "保存配置文件";
            this.tool_saveas.Click += new System.EventHandler(this.Tool_saveas_Click);
            // 
            // tool_readcfg
            // 
            this.tool_readcfg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tool_readcfg.Image = ((System.Drawing.Image)(resources.GetObject("tool_readcfg.Image")));
            this.tool_readcfg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tool_readcfg.Name = "tool_readcfg";
            this.tool_readcfg.Size = new System.Drawing.Size(23, 22);
            this.tool_readcfg.Text = "读配置";
            // 
            // tool_writecfg
            // 
            this.tool_writecfg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tool_writecfg.Image = global::SentConfigurator.Properties.Resources.SendDefault;
            this.tool_writecfg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tool_writecfg.Name = "tool_writecfg";
            this.tool_writecfg.Size = new System.Drawing.Size(23, 22);
            this.tool_writecfg.Text = "写配置";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 45);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(830, 844);
            this.tabControl1.TabIndex = 30;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel5);
            this.tabPage2.Controls.Add(this.panel4);
            this.tabPage2.Controls.Add(this.panel3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(822, 818);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "SENT配置";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.groupBox5);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 310);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(816, 462);
            this.panel5.TabIndex = 30;
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.lbx_group_count_limit);
            this.groupBox5.Controls.Add(this.btn_cleardgv);
            this.groupBox5.Controls.Add(this.dgv_groupdata);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.cob_group_num);
            this.groupBox5.Location = new System.Drawing.Point(3, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(808, 456);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "慢信号";
            // 
            // lbx_group_count_limit
            // 
            this.lbx_group_count_limit.ForeColor = System.Drawing.Color.Blue;
            this.lbx_group_count_limit.Location = new System.Drawing.Point(131, 23);
            this.lbx_group_count_limit.Name = "lbx_group_count_limit";
            this.lbx_group_count_limit.Size = new System.Drawing.Size(42, 18);
            this.lbx_group_count_limit.TabIndex = 35;
            this.lbx_group_count_limit.Text = "0-0X32";
            // 
            // btn_cleardgv
            // 
            this.btn_cleardgv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_cleardgv.Location = new System.Drawing.Point(731, 15);
            this.btn_cleardgv.Name = "btn_cleardgv";
            this.btn_cleardgv.Size = new System.Drawing.Size(71, 26);
            this.btn_cleardgv.TabIndex = 34;
            this.btn_cleardgv.Text = "清空";
            this.btn_cleardgv.UseVisualStyleBackColor = true;
            this.btn_cleardgv.Click += new System.EventHandler(this.Btn_cleardgv_Click);
            // 
            // dgv_groupdata
            // 
            this.dgv_groupdata.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_groupdata.Location = new System.Drawing.Point(2, 48);
            // 
            // 
            // 
            this.dgv_groupdata.MasterTemplate.ViewDefinition = tableViewDefinition1;
            this.dgv_groupdata.Name = "dgv_groupdata";
            this.dgv_groupdata.Size = new System.Drawing.Size(806, 408);
            this.dgv_groupdata.TabIndex = 33;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 25);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 14;
            this.label9.Text = "组数：";
            // 
            // cob_group_num
            // 
            this.cob_group_num.FormattingEnabled = true;
            this.cob_group_num.Location = new System.Drawing.Point(51, 22);
            this.cob_group_num.Name = "cob_group_num";
            this.cob_group_num.Size = new System.Drawing.Size(74, 20);
            this.cob_group_num.TabIndex = 12;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.btn_sig_cfg_read);
            this.panel4.Controls.Add(this.btn_sig_cfg_set);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(3, 772);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(816, 43);
            this.panel4.TabIndex = 30;
            // 
            // btn_sig_cfg_read
            // 
            this.btn_sig_cfg_read.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_sig_cfg_read.Location = new System.Drawing.Point(638, 6);
            this.btn_sig_cfg_read.Name = "btn_sig_cfg_read";
            this.btn_sig_cfg_read.Size = new System.Drawing.Size(75, 23);
            this.btn_sig_cfg_read.TabIndex = 0;
            this.btn_sig_cfg_read.Text = "读取配置";
            this.btn_sig_cfg_read.UseVisualStyleBackColor = true;
            this.btn_sig_cfg_read.Click += new System.EventHandler(this.Btn_sig_cfg_read_Click);
            // 
            // btn_sig_cfg_set
            // 
            this.btn_sig_cfg_set.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_sig_cfg_set.Location = new System.Drawing.Point(730, 6);
            this.btn_sig_cfg_set.Name = "btn_sig_cfg_set";
            this.btn_sig_cfg_set.Size = new System.Drawing.Size(75, 23);
            this.btn_sig_cfg_set.TabIndex = 3;
            this.btn_sig_cfg_set.Text = "设置配置";
            this.btn_sig_cfg_set.UseVisualStyleBackColor = true;
            this.btn_sig_cfg_set.Click += new System.EventHandler(this.Btn_sig_cfg_set_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox6);
            this.panel3.Controls.Add(this.groupBox4);
            this.panel3.Controls.Add(this.groupBox8);
            this.panel3.Controls.Add(this.groupBox7);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(816, 307);
            this.panel3.TabIndex = 30;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btn_open);
            this.groupBox6.Controls.Add(this.cb_handshake);
            this.groupBox6.Controls.Add(this.cb_stop);
            this.groupBox6.Controls.Add(this.label12);
            this.groupBox6.Controls.Add(this.cbx_recon);
            this.groupBox6.Controls.Add(this.cb_check);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.ledControl1);
            this.groupBox6.Controls.Add(this.cb_data);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.cb_port);
            this.groupBox6.Controls.Add(this.label1);
            this.groupBox6.Controls.Add(this.cb_baud);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Location = new System.Drawing.Point(5, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(806, 95);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "串口配置";
            // 
            // btn_open
            // 
            this.btn_open.Location = new System.Drawing.Point(126, 66);
            this.btn_open.Name = "btn_open";
            this.btn_open.Size = new System.Drawing.Size(45, 23);
            this.btn_open.TabIndex = 38;
            this.btn_open.Text = "打开";
            this.btn_open.UseVisualStyleBackColor = true;
            this.btn_open.Click += new System.EventHandler(this.Btn_open_Click);
            // 
            // cb_handshake
            // 
            this.cb_handshake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_handshake.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_handshake.FormattingEnabled = true;
            this.cb_handshake.Location = new System.Drawing.Point(648, 52);
            this.cb_handshake.Name = "cb_handshake";
            this.cb_handshake.Size = new System.Drawing.Size(115, 20);
            this.cb_handshake.TabIndex = 7;
            // 
            // cb_stop
            // 
            this.cb_stop.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_stop.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_stop.FormattingEnabled = true;
            this.cb_stop.Location = new System.Drawing.Point(649, 23);
            this.cb_stop.Name = "cb_stop";
            this.cb_stop.Size = new System.Drawing.Size(115, 20);
            this.cb_stop.TabIndex = 1;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(601, 55);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(41, 12);
            this.label12.TabIndex = 8;
            this.label12.Text = "流控：";
            // 
            // cbx_recon
            // 
            this.cbx_recon.AutoSize = true;
            this.cbx_recon.Location = new System.Drawing.Point(126, 34);
            this.cbx_recon.Name = "cbx_recon";
            this.cbx_recon.Size = new System.Drawing.Size(72, 16);
            this.cbx_recon.TabIndex = 37;
            this.cbx_recon.Text = "自动重连";
            this.cbx_recon.UseVisualStyleBackColor = true;
            // 
            // cb_check
            // 
            this.cb_check.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_check.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_check.FormattingEnabled = true;
            this.cb_check.Location = new System.Drawing.Point(287, 47);
            this.cb_check.Name = "cb_check";
            this.cb_check.Size = new System.Drawing.Size(115, 20);
            this.cb_check.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 31;
            this.label3.Text = "串口状态：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(601, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 2;
            this.label6.Text = "停止位：";
            // 
            // ledControl1
            // 
            this.ledControl1.LEDCenterColor = System.Drawing.Color.LightYellow;
            this.ledControl1.LEDCircleColor = System.Drawing.Color.Gray;
            this.ledControl1.LEDClickEnable = true;
            this.ledControl1.LEDSurroundColor = System.Drawing.Color.Yellow;
            this.ledControl1.LEDSwitch = true;
            this.ledControl1.Location = new System.Drawing.Point(89, 28);
            this.ledControl1.Name = "ledControl1";
            this.ledControl1.Size = new System.Drawing.Size(19, 22);
            this.ledControl1.TabIndex = 32;
            // 
            // cb_data
            // 
            this.cb_data.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_data.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_data.FormattingEnabled = true;
            this.cb_data.Location = new System.Drawing.Point(480, 50);
            this.cb_data.Name = "cb_data";
            this.cb_data.Size = new System.Drawing.Size(115, 20);
            this.cb_data.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(421, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "数据位：";
            // 
            // cb_port
            // 
            this.cb_port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_port.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_port.FormattingEnabled = true;
            this.cb_port.Location = new System.Drawing.Point(287, 20);
            this.cb_port.Name = "cb_port";
            this.cb_port.Size = new System.Drawing.Size(115, 20);
            this.cb_port.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(228, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "串口号：";
            // 
            // cb_baud
            // 
            this.cb_baud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_baud.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cb_baud.FormattingEnabled = true;
            this.cb_baud.Location = new System.Drawing.Point(480, 20);
            this.cb_baud.Name = "cb_baud";
            this.cb_baud.Size = new System.Drawing.Size(115, 20);
            this.cb_baud.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(421, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "波特率：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(228, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "校验位：";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rdb_dec);
            this.groupBox4.Controls.Add(this.rdb_hex);
            this.groupBox4.Location = new System.Drawing.Point(3, 104);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(808, 42);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "输入类型";
            // 
            // rdb_dec
            // 
            this.rdb_dec.Location = new System.Drawing.Point(168, 17);
            this.rdb_dec.Name = "rdb_dec";
            this.rdb_dec.Size = new System.Drawing.Size(56, 18);
            this.rdb_dec.TabIndex = 1;
            this.rdb_dec.Text = "十进制";
            // 
            // rdb_hex
            // 
            this.rdb_hex.Location = new System.Drawing.Point(86, 17);
            this.rdb_hex.Name = "rdb_hex";
            this.rdb_hex.Size = new System.Drawing.Size(68, 18);
            this.rdb_hex.TabIndex = 0;
            this.rdb_hex.Text = "十六进制";
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.tableLayoutPanel6);
            this.groupBox8.Controls.Add(this.lbx_hexorder_notes);
            this.groupBox8.Controls.Add(this.chx_hex_order);
            this.groupBox8.Location = new System.Drawing.Point(439, 152);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(374, 152);
            this.groupBox8.TabIndex = 2;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "快信号";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 28.46715F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 71.53284F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 73F));
            this.tableLayoutPanel6.Controls.Add(this.radLabel8, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.radLabel5, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.lbx_quickd2_limit, 0, 2);
            this.tableLayoutPanel6.Controls.Add(this.tbx_quicksig_data2, 1, 2);
            this.tableLayoutPanel6.Controls.Add(this.cob_quicksig_type, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.tbx_quicksig_data1, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.lbx_quickd1_limit, 2, 1);
            this.tableLayoutPanel6.Controls.Add(this.radLabel10, 2, 2);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(6, 17);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 3;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(366, 88);
            this.tableLayoutPanel6.TabIndex = 15;
            // 
            // radLabel8
            // 
            this.radLabel8.Location = new System.Drawing.Point(3, 3);
            this.radLabel8.Name = "radLabel8";
            this.radLabel8.Size = new System.Drawing.Size(65, 18);
            this.radLabel8.TabIndex = 9;
            this.radLabel8.Text = "快信号类型";
            // 
            // radLabel5
            // 
            this.radLabel5.Location = new System.Drawing.Point(3, 32);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(70, 18);
            this.radLabel5.TabIndex = 10;
            this.radLabel5.Text = "快信号data1";
            // 
            // lbx_quickd2_limit
            // 
            this.lbx_quickd2_limit.Location = new System.Drawing.Point(3, 61);
            this.lbx_quickd2_limit.Name = "lbx_quickd2_limit";
            this.lbx_quickd2_limit.Size = new System.Drawing.Size(70, 18);
            this.lbx_quickd2_limit.TabIndex = 12;
            this.lbx_quickd2_limit.Text = "快信号data2";
            // 
            // tbx_quicksig_data2
            // 
            this.tbx_quicksig_data2.Location = new System.Drawing.Point(86, 61);
            this.tbx_quicksig_data2.Name = "tbx_quicksig_data2";
            this.tbx_quicksig_data2.Size = new System.Drawing.Size(203, 21);
            this.tbx_quicksig_data2.TabIndex = 13;
            // 
            // cob_quicksig_type
            // 
            this.cob_quicksig_type.FormattingEnabled = true;
            this.cob_quicksig_type.Location = new System.Drawing.Point(86, 3);
            this.cob_quicksig_type.Name = "cob_quicksig_type";
            this.cob_quicksig_type.Size = new System.Drawing.Size(203, 20);
            this.cob_quicksig_type.TabIndex = 8;
            // 
            // tbx_quicksig_data1
            // 
            this.tbx_quicksig_data1.Location = new System.Drawing.Point(86, 32);
            this.tbx_quicksig_data1.Name = "tbx_quicksig_data1";
            this.tbx_quicksig_data1.Size = new System.Drawing.Size(203, 21);
            this.tbx_quicksig_data1.TabIndex = 11;
            // 
            // lbx_quickd1_limit
            // 
            this.lbx_quickd1_limit.ForeColor = System.Drawing.Color.Blue;
            this.lbx_quickd1_limit.Location = new System.Drawing.Point(295, 32);
            this.lbx_quickd1_limit.Name = "lbx_quickd1_limit";
            this.lbx_quickd1_limit.Size = new System.Drawing.Size(52, 18);
            this.lbx_quickd1_limit.TabIndex = 14;
            this.lbx_quickd1_limit.Text = "0-0X0FFF";
            // 
            // radLabel10
            // 
            this.radLabel10.ForeColor = System.Drawing.Color.Blue;
            this.radLabel10.Location = new System.Drawing.Point(295, 61);
            this.radLabel10.Name = "radLabel10";
            this.radLabel10.Size = new System.Drawing.Size(52, 18);
            this.radLabel10.TabIndex = 15;
            this.radLabel10.Text = "0-0X0FFF";
            // 
            // lbx_hexorder_notes
            // 
            this.lbx_hexorder_notes.AutoSize = true;
            this.lbx_hexorder_notes.Location = new System.Drawing.Point(7, 128);
            this.lbx_hexorder_notes.Name = "lbx_hexorder_notes";
            this.lbx_hexorder_notes.Size = new System.Drawing.Size(335, 12);
            this.lbx_hexorder_notes.TabIndex = 14;
            this.lbx_hexorder_notes.Text = "快信号data2说明：勾选后表示配置SENT时低位在前，高位在后";
            // 
            // chx_hex_order
            // 
            this.chx_hex_order.Location = new System.Drawing.Point(97, 105);
            this.chx_hex_order.Name = "chx_hex_order";
            this.chx_hex_order.Size = new System.Drawing.Size(127, 18);
            this.chx_hex_order.TabIndex = 2;
            this.chx_hex_order.Text = "低位在前，高位在后";
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.tableLayoutPanel5);
            this.groupBox7.Location = new System.Drawing.Point(3, 152);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(433, 152);
            this.groupBox7.TabIndex = 1;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "基础信号";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 48.57143F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 51.42857F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.tableLayoutPanel5.Controls.Add(this.tbx_timeframe, 1, 3);
            this.tableLayoutPanel5.Controls.Add(this.radLabel2, 0, 1);
            this.tableLayoutPanel5.Controls.Add(this.cob_dataframe_type, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.radLabel4, 0, 3);
            this.tableLayoutPanel5.Controls.Add(this.cob_battery_state, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.radLabel3, 0, 2);
            this.tableLayoutPanel5.Controls.Add(this.radLabel1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.cob_serial_msg, 1, 2);
            this.tableLayoutPanel5.Controls.Add(this.lbx_ticksLimit, 2, 3);
            this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 4;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(427, 132);
            this.tableLayoutPanel5.TabIndex = 11;
            // 
            // tbx_timeframe
            // 
            this.tbx_timeframe.Location = new System.Drawing.Point(163, 102);
            this.tbx_timeframe.Name = "tbx_timeframe";
            this.tbx_timeframe.Size = new System.Drawing.Size(164, 21);
            this.tbx_timeframe.TabIndex = 14;
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(3, 36);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(104, 18);
            this.radLabel2.TabIndex = 2;
            this.radLabel2.Text = "SENT空闲电平状态";
            // 
            // cob_dataframe_type
            // 
            this.cob_dataframe_type.FormattingEnabled = true;
            this.cob_dataframe_type.Location = new System.Drawing.Point(163, 3);
            this.cob_dataframe_type.Name = "cob_dataframe_type";
            this.cob_dataframe_type.Size = new System.Drawing.Size(164, 20);
            this.cob_dataframe_type.TabIndex = 6;
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(3, 102);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(148, 18);
            this.radLabel4.TabIndex = 4;
            this.radLabel4.Text = "SENT一帧时间(ppTicks个数)";
            // 
            // cob_battery_state
            // 
            this.cob_battery_state.FormattingEnabled = true;
            this.cob_battery_state.Location = new System.Drawing.Point(163, 36);
            this.cob_battery_state.Name = "cob_battery_state";
            this.cob_battery_state.Size = new System.Drawing.Size(164, 20);
            this.cob_battery_state.TabIndex = 5;
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(3, 69);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(104, 18);
            this.radLabel3.TabIndex = 3;
            this.radLabel3.Text = "SENT扩展串行消息";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(3, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(92, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "SENT数据帧类型";
            // 
            // cob_serial_msg
            // 
            this.cob_serial_msg.FormattingEnabled = true;
            this.cob_serial_msg.Location = new System.Drawing.Point(163, 69);
            this.cob_serial_msg.Name = "cob_serial_msg";
            this.cob_serial_msg.Size = new System.Drawing.Size(164, 20);
            this.cob_serial_msg.TabIndex = 1;
            // 
            // lbx_ticksLimit
            // 
            this.lbx_ticksLimit.ForeColor = System.Drawing.Color.Blue;
            this.lbx_ticksLimit.Location = new System.Drawing.Point(333, 102);
            this.lbx_ticksLimit.Name = "lbx_ticksLimit";
            this.lbx_ticksLimit.Size = new System.Drawing.Size(83, 18);
            this.lbx_ticksLimit.TabIndex = 15;
            this.lbx_ticksLimit.Text = "0X0110-0X0FFF";
            // 
            // radMenuItem11
            // 
            this.radMenuItem11.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.menu_help,
            this.menu_abort});
            this.radMenuItem11.Name = "radMenuItem11";
            this.radMenuItem11.Text = "帮助";
            // 
            // menu_help
            // 
            this.menu_help.Name = "menu_help";
            this.menu_help.Text = "帮助";
            this.menu_help.Click += new System.EventHandler(this.Menu_help_Click);
            // 
            // menu_abort
            // 
            this.menu_abort.Name = "menu_abort";
            this.menu_abort.Text = "关于";
            this.menu_abort.Click += new System.EventHandler(this.Menu_abort_Click);
            // 
            // radMenu1
            // 
            this.radMenu1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.radMenuItem1,
            this.radMenuItem4,
            this.radMenuItem7,
            this.radMenuItem11});
            this.radMenu1.Location = new System.Drawing.Point(0, 0);
            this.radMenu1.Name = "radMenu1";
            this.radMenu1.Size = new System.Drawing.Size(830, 20);
            this.radMenu1.TabIndex = 33;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 911);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.radMenu1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Sent Configurator";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_group_count_limit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_groupdata.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_groupdata)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rdb_dec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rdb_hex)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel8)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_quickd2_limit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_quickd1_limit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chx_hex_order)).EndInit();
            this.groupBox7.ResumeLayout(false);
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbx_ticksLimit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radMenu1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel status_sendcount;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem1;
        private Telerik.WinControls.UI.RadMenuItem menu_open_file;
        private Telerik.WinControls.UI.RadMenuItem menu_save_as;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem4;
        private Telerik.WinControls.UI.RadMenuItem menu_searchDev;
        private Telerik.WinControls.UI.RadMenuItem menu_sent_cfg_read;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem7;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem8;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem9;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tool_open_config;
        private System.Windows.Forms.ToolStripButton tool_saveas;
        private System.Windows.Forms.ToolStripButton tool_readcfg;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cob_group_num;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private System.Windows.Forms.ComboBox cob_dataframe_type;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private System.Windows.Forms.ComboBox cob_battery_state;
        private System.Windows.Forms.ComboBox cob_serial_msg;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private System.Windows.Forms.TextBox tbx_quicksig_data2;
        private Telerik.WinControls.UI.RadLabel lbx_quickd2_limit;
        private System.Windows.Forms.TextBox tbx_quicksig_data1;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadLabel radLabel8;
        private System.Windows.Forms.ComboBox cob_quicksig_type;
        private System.Windows.Forms.Button btn_sig_cfg_set;
        private System.Windows.Forms.Button btn_sig_cfg_read;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel status_receivecount;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private Telerik.WinControls.UI.RadGridView dgv_groupdata;
        private Telerik.WinControls.UI.RadMenuItem menu_app_exist;
        private Telerik.WinControls.UI.RadMenuItem menu_sent_cfg_write;
        private Telerik.WinControls.UI.RadMenuItem radMenuItem11;
        private Telerik.WinControls.UI.RadMenuItem menu_help;
        private Telerik.WinControls.UI.RadMenuItem menu_abort;
        private System.Windows.Forms.ToolStripButton tool_writecfg;
        private System.Windows.Forms.GroupBox groupBox4;
        private Telerik.WinControls.UI.RadCheckBox chx_hex_order;
        private Telerik.WinControls.UI.RadRadioButton rdb_dec;
        private Telerik.WinControls.UI.RadRadioButton rdb_hex;
        private System.Windows.Forms.TextBox tbx_timeframe;
        private System.Windows.Forms.Label lbx_hexorder_notes;
        private System.Windows.Forms.Button btn_cleardgv;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private Telerik.WinControls.UI.RadLabel lbx_quickd1_limit;
        private Telerik.WinControls.UI.RadLabel lbx_ticksLimit;
        private Telerik.WinControls.UI.RadLabel radLabel10;
        private Telerik.WinControls.UI.RadLabel lbx_group_count_limit;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label3;
        private LEDLib.LEDControl ledControl1;
        private System.Windows.Forms.ComboBox cb_handshake;
        private System.Windows.Forms.ComboBox cb_stop;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox cbx_recon;
        private System.Windows.Forms.ComboBox cb_check;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cb_data;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cb_port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cb_baud;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_open;
        private Telerik.WinControls.UI.RadMenu radMenu1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tool_dev_status;
    }
}

