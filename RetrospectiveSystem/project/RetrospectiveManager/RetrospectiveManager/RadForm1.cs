using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI.Docking;
using Telerik.WinControls.UI;

namespace RetrospectiveManager
{
    public partial class RadForm1 : RadForm
    {
        public RadForm1()
        {
            InitializeComponent();
        }

        private void RadButton1_Click(object sender, EventArgs e)
        {
            this.documentWindow1.Hide();
        }

        private void RadButton2_Click(object sender, EventArgs e)
        {
            this.radDock1.Show();
            this.documentTabStrip1.Show();
            this.documentWindow1.Show();
        }

        private void RadForm1_Load(object sender, EventArgs e)
        {
            this.documentTabStrip1.VisibleChanged += DocumentTabStrip1_VisibleChanged;
        }

        private void DocumentTabStrip1_VisibleChanged(object sender, EventArgs e)
        {
            
        }
    }
}
