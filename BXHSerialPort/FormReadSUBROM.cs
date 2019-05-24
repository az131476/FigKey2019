using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static BXHSerialPort.Protocol.ProtocolDefine;

namespace BXHSerialPort
{
    public partial class FormReadSUBROM : Form
    {
        
        public FormReadSUBROM(string s)
        {
            InitializeComponent();
            textBox1.Text = s;
        }

        private void FormReadSUBROM_Load(object sender, EventArgs e)
        {

        }
    }
}
