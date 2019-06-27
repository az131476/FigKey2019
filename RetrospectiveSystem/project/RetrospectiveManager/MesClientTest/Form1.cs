using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MesClientTest
{
    public partial class Form1 : Form
    {
        private MesService.MesServiceClient mesService;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            mesService = new MesService.MesServiceClient();
        }

        private void Btn_commit_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Convert.ToString((int)testEnum.first,16));
        }

        enum testEnum
        {
            first = 0X11
        }
    }
}
