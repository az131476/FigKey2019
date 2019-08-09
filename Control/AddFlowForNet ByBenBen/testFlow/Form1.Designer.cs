namespace testFlow
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.addFlow1 = new Lassalle.Flow.AddFlow();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsb_NewFile = new System.Windows.Forms.ToolStripButton();
            this.tsb_Load = new System.Windows.Forms.ToolStripButton();
            this.tsb_ClearNodes = new System.Windows.Forms.ToolStripButton();
            this.tsb_Save = new System.Windows.Forms.ToolStripButton();
            this.tsb_Print00 = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsb_打印预览 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_打印设置 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_打印 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_Line = new System.Windows.Forms.ToolStripButton();
            this.tsb_Line_Style1 = new System.Windows.Forms.ToolStripComboBox();
            this.tsb_ORG = new System.Windows.Forms.ToolStripButton();
            this.tsb_ORG_Style1 = new System.Windows.Forms.ToolStripComboBox();
            this.tsb_MouseP = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ttsb_Cut = new System.Windows.Forms.ToolStripButton();
            this.tsb_Copy = new System.Windows.Forms.ToolStripButton();
            this.tsb_Post = new System.Windows.Forms.ToolStripButton();
            this.tsb_Delete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_UnDo = new System.Windows.Forms.ToolStripButton();
            this.tsb_ReDo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsb_Zomm150 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_Zomm100 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_Zomm70 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsb_Property = new System.Windows.Forms.ToolStripButton();
            this.tsb_附加属性 = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // addFlow1
            // 
            this.addFlow1.AutoScroll = true;
            this.addFlow1.AutoScrollMinSize = new System.Drawing.Size(853, 424);
            this.addFlow1.BackColor = System.Drawing.SystemColors.HighlightText;
            this.addFlow1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.addFlow1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addFlow1.Location = new System.Drawing.Point(0, 0);
            this.addFlow1.Name = "addFlow1";
            this.addFlow1.Size = new System.Drawing.Size(713, 304);
            this.addFlow1.TabIndex = 0;
            this.addFlow1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.addFlow1_KeyPress);
            this.addFlow1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.addFlow1_MouseDown);
            this.addFlow1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.addFlow1_MouseDoubleClick);
            this.addFlow1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.addFlow1_MouseClick);
            this.addFlow1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.addFlow1_MouseUp);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_NewFile,
            this.tsb_Load,
            this.tsb_ClearNodes,
            this.tsb_Save,
            this.tsb_Print00,
            this.toolStripSeparator1,
            this.tsb_Line,
            this.tsb_Line_Style1,
            this.tsb_ORG,
            this.tsb_ORG_Style1,
            this.tsb_MouseP,
            this.toolStripSeparator2,
            this.ttsb_Cut,
            this.tsb_Copy,
            this.tsb_Post,
            this.tsb_Delete,
            this.toolStripSeparator3,
            this.toolStripSeparator4,
            this.tsb_UnDo,
            this.tsb_ReDo,
            this.toolStripSeparator5,
            this.toolStripDropDownButton1,
            this.toolStripSeparator6,
            this.tsb_Property,
            this.tsb_附加属性});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(922, 35);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsb_NewFile
            // 
            this.tsb_NewFile.Image = ((System.Drawing.Image)(resources.GetObject("tsb_NewFile.Image")));
            this.tsb_NewFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_NewFile.Name = "tsb_NewFile";
            this.tsb_NewFile.Size = new System.Drawing.Size(33, 32);
            this.tsb_NewFile.Text = "新建";
            this.tsb_NewFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_NewFile.Click += new System.EventHandler(this.tsb_NewFile_Click);
            // 
            // tsb_Load
            // 
            this.tsb_Load.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Load.Image")));
            this.tsb_Load.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Load.Name = "tsb_Load";
            this.tsb_Load.Size = new System.Drawing.Size(33, 32);
            this.tsb_Load.Text = "读取";
            this.tsb_Load.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_Load.Click += new System.EventHandler(this.tsb_Load_Click);
            // 
            // tsb_ClearNodes
            // 
            this.tsb_ClearNodes.Image = ((System.Drawing.Image)(resources.GetObject("tsb_ClearNodes.Image")));
            this.tsb_ClearNodes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_ClearNodes.Name = "tsb_ClearNodes";
            this.tsb_ClearNodes.Size = new System.Drawing.Size(33, 32);
            this.tsb_ClearNodes.Text = "清空";
            this.tsb_ClearNodes.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_ClearNodes.Click += new System.EventHandler(this.tsb_ClearNodes_Click);
            // 
            // tsb_Save
            // 
            this.tsb_Save.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Save.Image")));
            this.tsb_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Save.Name = "tsb_Save";
            this.tsb_Save.Size = new System.Drawing.Size(33, 32);
            this.tsb_Save.Text = "保存";
            this.tsb_Save.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_Save.Click += new System.EventHandler(this.tsb_Save_Click);
            // 
            // tsb_Print00
            // 
            this.tsb_Print00.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_打印预览,
            this.tsb_打印设置,
            this.tsb_打印});
            this.tsb_Print00.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Print00.Image")));
            this.tsb_Print00.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Print00.Name = "tsb_Print00";
            this.tsb_Print00.Size = new System.Drawing.Size(42, 32);
            this.tsb_Print00.Text = "打印";
            this.tsb_Print00.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // tsb_打印预览
            // 
            this.tsb_打印预览.Name = "tsb_打印预览";
            this.tsb_打印预览.Size = new System.Drawing.Size(118, 22);
            this.tsb_打印预览.Text = "打印预览";
            this.tsb_打印预览.Click += new System.EventHandler(this.tsb_打印预览_Click);
            // 
            // tsb_打印设置
            // 
            this.tsb_打印设置.Name = "tsb_打印设置";
            this.tsb_打印设置.Size = new System.Drawing.Size(118, 22);
            this.tsb_打印设置.Text = "打印设置";
            this.tsb_打印设置.Click += new System.EventHandler(this.tsb_打印设置_Click);
            // 
            // tsb_打印
            // 
            this.tsb_打印.Name = "tsb_打印";
            this.tsb_打印.Size = new System.Drawing.Size(118, 22);
            this.tsb_打印.Text = "打印";
            this.tsb_打印.Click += new System.EventHandler(this.tsb_打印_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
            // 
            // tsb_Line
            // 
            this.tsb_Line.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Line.Image")));
            this.tsb_Line.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Line.Name = "tsb_Line";
            this.tsb_Line.Size = new System.Drawing.Size(33, 32);
            this.tsb_Line.Text = "画线";
            this.tsb_Line.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_Line.Click += new System.EventHandler(this.tsb_Line_Click);
            // 
            // tsb_Line_Style1
            // 
            this.tsb_Line_Style1.AutoToolTip = true;
            this.tsb_Line_Style1.DropDownWidth = 40;
            this.tsb_Line_Style1.Items.AddRange(new object[] {
            "虚线",
            "实线"});
            this.tsb_Line_Style1.Name = "tsb_Line_Style1";
            this.tsb_Line_Style1.Size = new System.Drawing.Size(75, 35);
            this.tsb_Line_Style1.SelectedIndexChanged += new System.EventHandler(this.tsb_Line_Style1_SelectedIndexChanged);
            // 
            // tsb_ORG
            // 
            this.tsb_ORG.Image = ((System.Drawing.Image)(resources.GetObject("tsb_ORG.Image")));
            this.tsb_ORG.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_ORG.Name = "tsb_ORG";
            this.tsb_ORG.Size = new System.Drawing.Size(33, 32);
            this.tsb_ORG.Text = "对象";
            this.tsb_ORG.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_ORG.Click += new System.EventHandler(this.tsb_ORG_Click);
            // 
            // tsb_ORG_Style1
            // 
            this.tsb_ORG_Style1.Items.AddRange(new object[] {
            "圆形",
            "矩形",
            "菱形",
            "图片",
            "p_判断条件",
            "p_活动"});
            this.tsb_ORG_Style1.Name = "tsb_ORG_Style1";
            this.tsb_ORG_Style1.Size = new System.Drawing.Size(75, 35);
            this.tsb_ORG_Style1.SelectedIndexChanged += new System.EventHandler(this.tsb_ORG_Style1_SelectedIndexChanged);
            // 
            // tsb_MouseP
            // 
            this.tsb_MouseP.Image = ((System.Drawing.Image)(resources.GetObject("tsb_MouseP.Image")));
            this.tsb_MouseP.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_MouseP.Name = "tsb_MouseP";
            this.tsb_MouseP.Size = new System.Drawing.Size(33, 32);
            this.tsb_MouseP.Text = "鼠标";
            this.tsb_MouseP.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_MouseP.Click += new System.EventHandler(this.tsb_MouseP_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
            // 
            // ttsb_Cut
            // 
            this.ttsb_Cut.Image = ((System.Drawing.Image)(resources.GetObject("ttsb_Cut.Image")));
            this.ttsb_Cut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ttsb_Cut.Name = "ttsb_Cut";
            this.ttsb_Cut.Size = new System.Drawing.Size(33, 32);
            this.ttsb_Cut.Text = "剪切";
            this.ttsb_Cut.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.ttsb_Cut.Click += new System.EventHandler(this.ttsb_Cut_Click);
            // 
            // tsb_Copy
            // 
            this.tsb_Copy.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Copy.Image")));
            this.tsb_Copy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Copy.Name = "tsb_Copy";
            this.tsb_Copy.Size = new System.Drawing.Size(33, 32);
            this.tsb_Copy.Text = "拷贝";
            this.tsb_Copy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_Copy.Click += new System.EventHandler(this.tsb_Copy_Click);
            // 
            // tsb_Post
            // 
            this.tsb_Post.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Post.Image")));
            this.tsb_Post.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Post.Name = "tsb_Post";
            this.tsb_Post.Size = new System.Drawing.Size(33, 32);
            this.tsb_Post.Text = "粘贴";
            this.tsb_Post.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_Post.Click += new System.EventHandler(this.tsb_Post_Click);
            // 
            // tsb_Delete
            // 
            this.tsb_Delete.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Delete.Image")));
            this.tsb_Delete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Delete.Name = "tsb_Delete";
            this.tsb_Delete.Size = new System.Drawing.Size(33, 32);
            this.tsb_Delete.Text = "删除";
            this.tsb_Delete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_Delete.Click += new System.EventHandler(this.tsb_Delete_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 35);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 35);
            // 
            // tsb_UnDo
            // 
            this.tsb_UnDo.Image = ((System.Drawing.Image)(resources.GetObject("tsb_UnDo.Image")));
            this.tsb_UnDo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_UnDo.Name = "tsb_UnDo";
            this.tsb_UnDo.Size = new System.Drawing.Size(33, 32);
            this.tsb_UnDo.Text = "回退";
            this.tsb_UnDo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_UnDo.Click += new System.EventHandler(this.tsb_UNDo_Click);
            // 
            // tsb_ReDo
            // 
            this.tsb_ReDo.Image = ((System.Drawing.Image)(resources.GetObject("tsb_ReDo.Image")));
            this.tsb_ReDo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_ReDo.Name = "tsb_ReDo";
            this.tsb_ReDo.Size = new System.Drawing.Size(33, 32);
            this.tsb_ReDo.Text = "前进";
            this.tsb_ReDo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_ReDo.Click += new System.EventHandler(this.tsb_ReDo_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 35);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsb_Zomm150,
            this.tsb_Zomm100,
            this.tsb_Zomm70});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(42, 32);
            this.toolStripDropDownButton1.Text = "放大";
            this.toolStripDropDownButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // tsb_Zomm150
            // 
            this.tsb_Zomm150.Name = "tsb_Zomm150";
            this.tsb_Zomm150.Size = new System.Drawing.Size(124, 22);
            this.tsb_Zomm150.Text = "Zoom 150%";
            this.tsb_Zomm150.Click += new System.EventHandler(this.tsb_Zomm150_Click);
            // 
            // tsb_Zomm100
            // 
            this.tsb_Zomm100.Name = "tsb_Zomm100";
            this.tsb_Zomm100.Size = new System.Drawing.Size(124, 22);
            this.tsb_Zomm100.Text = "Zoom 100%";
            this.tsb_Zomm100.Click += new System.EventHandler(this.tsb_Zomm100_Click);
            // 
            // tsb_Zomm70
            // 
            this.tsb_Zomm70.Name = "tsb_Zomm70";
            this.tsb_Zomm70.Size = new System.Drawing.Size(124, 22);
            this.tsb_Zomm70.Text = "Zoom 75%";
            this.tsb_Zomm70.Click += new System.EventHandler(this.tsb_Zomm70_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 35);
            // 
            // tsb_Property
            // 
            this.tsb_Property.Image = ((System.Drawing.Image)(resources.GetObject("tsb_Property.Image")));
            this.tsb_Property.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Property.Name = "tsb_Property";
            this.tsb_Property.Size = new System.Drawing.Size(33, 32);
            this.tsb_Property.Text = "属性";
            this.tsb_Property.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_Property.Click += new System.EventHandler(this.tsb_Property_Click);
            // 
            // tsb_附加属性
            // 
            this.tsb_附加属性.Image = ((System.Drawing.Image)(resources.GetObject("tsb_附加属性.Image")));
            this.tsb_附加属性.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_附加属性.Name = "tsb_附加属性";
            this.tsb_附加属性.Size = new System.Drawing.Size(57, 32);
            this.tsb_附加属性.Text = "附加属性";
            this.tsb_附加属性.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsb_附加属性.Click += new System.EventHandler(this.tsb_附加属性_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 35);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.addFlow1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
            this.splitContainer1.Size = new System.Drawing.Size(922, 304);
            this.splitContainer1.SplitterDistance = 713;
            this.splitContainer1.TabIndex = 2;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(205, 304);
            this.propertyGrid1.TabIndex = 0;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageList2
            // 
            this.imageList2.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList2.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 339);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Lassalle.Flow.AddFlow addFlow1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsb_Save;
        private System.Windows.Forms.ToolStripButton tsb_Load;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsb_Line;
        private System.Windows.Forms.ToolStripButton tsb_ORG;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripComboBox tsb_Line_Style1;
        private System.Windows.Forms.ToolStripComboBox tsb_ORG_Style1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsb_Delete;
        private System.Windows.Forms.ToolStripButton tsb_NewFile;
        private System.Windows.Forms.ToolStripButton tsb_UnDo;
        private System.Windows.Forms.ToolStripButton tsb_ReDo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsb_Post;
        private System.Windows.Forms.ToolStripButton tsb_Copy;
        private System.Windows.Forms.ToolStripButton tsb_MouseP;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton ttsb_Cut;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem tsb_Zomm150;
        private System.Windows.Forms.ToolStripMenuItem tsb_Zomm100;
        private System.Windows.Forms.ToolStripMenuItem tsb_Zomm70;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsb_Property;
        private System.Windows.Forms.ToolStripButton tsb_ClearNodes;
        private System.Windows.Forms.ToolStripDropDownButton tsb_Print00;
        private System.Windows.Forms.ToolStripMenuItem tsb_打印预览;
        private System.Windows.Forms.ToolStripMenuItem tsb_打印设置;
        private System.Windows.Forms.ToolStripMenuItem tsb_打印;
        private System.Windows.Forms.ToolStripButton tsb_附加属性;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ImageList imageList2;
    }
}

