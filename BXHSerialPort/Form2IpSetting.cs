using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BXHSerialPort
{
    public partial class Form2IpSetting : Form
    {
        public Form2IpSetting()
        {
            InitializeComponent();
        }

        private void Form2IpSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            IPTxtFileRxWx.writeIP(textBox1.Text,textBox2.Text);
        }

        private void Form2IpSetting_Load(object sender, EventArgs e)
        {
            string[] ipSetting = IPTxtFileRxWx.readIP();
            if (ipSetting!=null)
            {
                textBox1.Text = ipSetting[0];
                textBox2.Text = ipSetting[1];
            }
        }
    }
}
