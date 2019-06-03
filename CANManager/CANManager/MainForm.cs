using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CANManager
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void xtraTabControl1_CloseButtonClick(object sender, EventArgs e)
        {
            //DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs EArg = (DevExpress.XtraTab.ViewInfo.ClosePageButtonEventArgs)e;
            //string tbpName = EArg.Page.Text;
            //foreach (DevExpress.XtraTab.XtraTabPage xtp in xtraTabControl1.TabPages)
            //{
            //    if (xtp.Text == tbpName)
            //    {
            //        xtraTabControl1.TabPages.Remove(xtp);
            //        xtp.Dispose();
            //        return;
            //    }
            //}
        }
    }
}
