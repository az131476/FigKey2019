using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using AnalysisAgreeMent.Model.XCP;

namespace LoggerConfigurator.View
{
    public partial class XcpProtocol : RadForm
    {
        private XcpData xcpData;
        private const string XCP_ON_CAN = "XCP ON CAN-";
        public XcpProtocol(XcpData data)
        {
            InitializeComponent();
            this.xcpData = data;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void XcpProtocol_Load(object sender, EventArgs e)
        {
            cb_list.Items.Clear();
            cb_list.Items.Add(XCP_ON_CAN+xcpData.XcpOnCanData.VehicleApplData.CanName);
            cb_list.Items.Add(XCP_ON_CAN+xcpData.XcpOnCanData.VehicleApplRamData.CanName);
            cb_list.Items.Add(XCP_ON_CAN+xcpData.XcpOnCanData.CalibrationLeData.CanName);
            cb_list.Items.Add(XCP_ON_CAN+xcpData.XcpOnCanData.CalibrationLeRamData.CanName);
            cb_list.SelectedIndex = 0;
            cb_list.Focus();
        }

        private void Btn_apply_Click(object sender, EventArgs e)
        {
            string str = cb_list.SelectedItem.ToString();
            xcpData.XcpOnCanData.CurrentSelectItem = str.Substring(XCP_ON_CAN.Length);
            this.Close();
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
