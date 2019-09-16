using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace LoadBoxControl
{
    public partial class EditInput : RadForm
    {
        public static string inputValue;
        public EditInput()
        {
            InitializeComponent();
        }

        private void Btn_ok_Click(object sender, EventArgs e)
        {
            //判断
            inputValue = this.tb_input.Text.Trim();
            this.Close();
        }
    }
}
