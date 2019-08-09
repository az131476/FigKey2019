using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GDIDrawFlow;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private DrawFlowGroup drawFlowGroup1;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.drawFlowGroup1 = new DrawFlowGroup();

            this.drawFlowGroup1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.drawFlowGroup1.Location = new System.Drawing.Point(0, 0);
            this.drawFlowGroup1.Name = "drawFlowGroup1";
            this.drawFlowGroup1.Size = new System.Drawing.Size(704, 502);
            this.drawFlowGroup1.TabIndex = 0;

            this.Controls.Add(this.drawFlowGroup1);
        }
    }
}
