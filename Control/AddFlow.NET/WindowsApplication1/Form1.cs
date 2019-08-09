using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Lassalle.Flow;

namespace WindowsApplication1
{
	/// <summary>
	/// Form1 的摘要说明。
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
        private Lassalle.Flow.AddFlow addFlow1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btnNode;
        private System.Windows.Forms.Button btnLink;
        private System.Windows.Forms.Button btnSelete;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private WindowsApplication1.TextBoxProp textBoxProp1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button button2;
        private System.ComponentModel.IContainer components;

		public Form1()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.btnNode = new System.Windows.Forms.Button();
            this.addFlow1 = new Lassalle.Flow.AddFlow();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSelete = new System.Windows.Forms.Button();
            this.btnLink = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBoxProp1 = new WindowsApplication1.TextBoxProp(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNode
            // 
            this.btnNode.Location = new System.Drawing.Point(12, 12);
            this.btnNode.Name = "btnNode";
            this.btnNode.TabIndex = 1;
            this.btnNode.Text = "Node";
            this.btnNode.Click += new System.EventHandler(this.btnNode_Click);
            // 
            // addFlow1
            // 
            this.addFlow1.AutoScroll = true;
            this.addFlow1.AutoScrollMinSize = new System.Drawing.Size(441, 567);
            this.addFlow1.CursorSetting = Lassalle.Flow.CursorSetting.Resize;
            this.addFlow1.CycleMode = Lassalle.Flow.CycleMode.CycleAllowed;
            this.addFlow1.DefNodeProp.Alignment = Lassalle.Flow.Alignment.CenterMIDDLE;
            this.addFlow1.DefNodeProp.AutoSize = Lassalle.Flow.AutoSize.None;
            this.addFlow1.DefNodeProp.BackMode = Lassalle.Flow.BackMode.Transparent;
            this.addFlow1.DefNodeProp.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            this.addFlow1.DefNodeProp.GradientColor = System.Drawing.SystemColors.Control;
            this.addFlow1.DefNodeProp.GradientMode = System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal;
            this.addFlow1.DefNodeProp.ImagePosition = Lassalle.Flow.ImagePosition.RelativeToText;
            this.addFlow1.DefNodeProp.Text = null;
            this.addFlow1.DefNodeProp.Tooltip = null;
            this.addFlow1.DefNodeProp.Trimming = System.Drawing.StringTrimming.None;
            this.addFlow1.LinkCreationMode = Lassalle.Flow.LinkCreationMode.AllNodeArea;
            this.addFlow1.LinkHandleSize = Lassalle.Flow.HandleSize.Small;
            this.addFlow1.Location = new System.Drawing.Point(0, 0);
            this.addFlow1.Name = "addFlow1";
            this.addFlow1.PageUnit = System.Drawing.GraphicsUnit.Point;
            this.addFlow1.RemovePointAngle = Lassalle.Flow.RemovePointAngle.Medium;
            this.addFlow1.ScrollbarsDisplayMode = Lassalle.Flow.ScrollbarsDisplayMode.AddControlSize;
            this.addFlow1.SelectionHandleSize = Lassalle.Flow.HandleSize.Small;
            this.addFlow1.Size = new System.Drawing.Size(288, 440);
            this.addFlow1.TabIndex = 2;
            this.addFlow1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.addFlow1_KeyDown);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnSelete);
            this.panel1.Controls.Add(this.btnLink);
            this.panel1.Controls.Add(this.btnNode);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(476, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(96, 440);
            this.panel1.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 184);
            this.button1.Name = "button1";
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 248);
            this.button5.Name = "button5";
            this.button5.TabIndex = 5;
            this.button5.Text = "button5";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(12, 108);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSelete
            // 
            this.btnSelete.Location = new System.Drawing.Point(12, 76);
            this.btnSelete.Name = "btnSelete";
            this.btnSelete.TabIndex = 3;
            this.btnSelete.Text = "Select";
            this.btnSelete.Click += new System.EventHandler(this.btnSelete_Click);
            // 
            // btnLink
            // 
            this.btnLink.Location = new System.Drawing.Point(12, 44);
            this.btnLink.Name = "btnLink";
            this.btnLink.TabIndex = 2;
            this.btnLink.Text = "Link";
            this.btnLink.Click += new System.EventHandler(this.btnLink_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(580, 465);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.addFlow1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 21);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(572, 440);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.textBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 21);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(572, 440);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(240, 80);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(148, 21);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "textBox1";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(352, 84);
            this.button2.Name = "button2";
            this.button2.TabIndex = 4;
            this.button2.Text = "button2";
            // 
            // Form1
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(580, 465);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new Form1());
		}

        private void Form1_Load(object sender, System.EventArgs e) {
            this.addFlow1.BackColor = SystemColors.Window;
            this.addFlow1.CursorSetting = Lassalle.Flow.CursorSetting.All;
//            this.addFlow1.MouseAction = MouseAction.
            this.addFlow1.DefNodeProp.Shape.Style = ShapeStyle.RectEdgeRaised;
            this.addFlow1.DefNodeProp.FillColor = SystemColors.Control;
            this.addFlow1.DefLinkProp.Jump = Jump.Arc;
            this.addFlow1.DefLinkProp.MaxPointsCount = 3;
         
        
        }

        private void button1_Click(object sender, System.EventArgs e) {
            Lassalle.Flow.Node n1 = this.addFlow1.Nodes.Add(10, 10, 40, 40, "aaa");
            Lassalle.Flow.Node n2 = this.addFlow1.Nodes.Add(60, 60, 40, 40, "bbb");

            //Lassalle.Flow.Link l = new Lassalle.Flow.Link()

            Lassalle.Flow.Link l = n1.Links.Add(n2, "lll");
//            l.MaxPointsCount = 3;
            
            //n1.Shape = Lassalle.Flow.Shape.
        }

        private void btnNode_Click(object sender, System.EventArgs e) {
            //this.addFlow1.CanStretchLink = false;
            this.addFlow1.CanDrawLink = false;
            this.addFlow1.CanDrawNode = true;

        }

        private void addFlow1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            //this.addFlow1.Nodes.Remove()
            foreach (Lassalle.Flow.Item item in this.addFlow1.SelectedItems) {
                //this.addFlow1.Nodes.Remove(item);
                //this.addFlow1.SelectedItems.RemoveAt(0);
            }

        }

        private void btnLink_Click(object sender, System.EventArgs e) {
            this.addFlow1.CanDrawLink = true;
            this.addFlow1.CanDrawNode = false;
        }

        private void btnSelete_Click(object sender, System.EventArgs e) {
            this.addFlow1.CanDrawLink = false;
            this.addFlow1.CanDrawNode = false;
        }

        private void btnDelete_Click(object sender, System.EventArgs e) {
        
        }

	}
}
