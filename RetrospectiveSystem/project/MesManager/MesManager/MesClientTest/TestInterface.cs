using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI.Docking;


namespace MesClientTest
{
    public partial class TestInterface : Form
    {
        private MesService.MesServiceClient mesService;
        public TestInterface()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mesService = new MesService.MesServiceClient();
        }

        private void Btn_commit_Click(object sender, EventArgs e)
        {

        }

        private void RadGroupBox1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
