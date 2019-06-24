using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace RetrospectiveManager
{
    public partial class SetStationAdmin : RadForm
    {
        private MesService.MesServiceClient mesService;
        public SetStationAdmin()
        {
            InitializeComponent();
            this.MaximizeBox = false;
        }

        private void SetStationAdmin_Load(object sender, EventArgs e)
        {

            rdb_sn.CheckStateChanged += Rdb_sn_CheckStateChanged;
            rdb_type_no.CheckStateChanged += Rdb_type_no_CheckStateChanged;
        }

        private void OptionRemoteData()
        {
            mesService = new MesService.MesServiceClient();
            //获取零件号可选项

        }

        private void Rdb_type_no_CheckStateChanged(object sender, EventArgs e)
        {
            if (rdb_type_no.CheckState == CheckState.Checked)
            {
                radGroupBox_type.Visible = true;
                radGroupBox_sn.Visible = false;
            }
        }

        private void Rdb_sn_CheckStateChanged(object sender, EventArgs e)
        {
            if (rdb_sn.CheckState == CheckState.Checked)
            {
                radGroupBox_sn.Visible = true;
                radGroupBox_type.Visible = false;
            }
        }
    }
}
