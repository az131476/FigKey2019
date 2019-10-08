using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace LoggerConfigurator
{
    public partial class ExportSet : Telerik.WinControls.UI.RadForm
    {
        public static bool can1Check, can2Check;
        public ExportSet()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void ExportSet_Load(object sender, EventArgs e)
        {
            this.cb_can1.CheckState = CheckState.Checked;
            this.cb_can2.CheckState = CheckState.Checked;
            this.cb_save.CheckState = CheckState.Checked;
            this.btn_ok.Click += Btn_ok_Click;
            this.btn_cancel.Click += Btn_cancel_Click;
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Btn_ok_Click(object sender, EventArgs e)
        {
            can1Check = this.cb_can1.Checked;
            can2Check = this.cb_can2.Checked;
            //保存配置
            this.DialogResult = DialogResult.OK;
        }
    }
}
