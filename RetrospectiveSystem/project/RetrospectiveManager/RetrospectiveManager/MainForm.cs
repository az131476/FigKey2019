using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CommonUtils.Logger;
using System.Web;
using System.IO;
using System.Net;

namespace RetrospectiveManager
{
    public partial class MainForm : Telerik.WinControls.UI.RadForm
    {
        public MainForm()
        {
            InitializeComponent();
            radButton4.Click += RadButton4_Click;
        }

        private void RadButton4_Click(object sender, EventArgs e)
        {
        }

        private void RadMenuItem3_Click(object sender, EventArgs e)
        {

        }
    }
}
