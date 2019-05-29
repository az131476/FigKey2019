using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace SentConfigurator.View
{
    public partial class SentHelp : Form
    {
        public SentHelp()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void SentHelp_Load(object sender, EventArgs e)
        {
            linkLabel1.Text = "www.figkey.com";
            linkLabel1.LinkVisited = true;
            linkLabel1.LinkClicked += LinkLabel1_LinkClicked;
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("iexplore.exe", linkLabel1.Text);
        }

        private void Btn_ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
