using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using MesManager.Control;
using System.IO;
using Telerik.WinControls.UI.Export;
using CommonUtils.Logger;
using CommonUtils.FileHelper;
using MesManager.TelerikWinform.GridViewCommon.GridViewDataExport;
using System.Threading;
using Telerik.WinControls.Themes;

namespace MesManager.UI
{
    public partial class ProductPackageDetail : RadForm
    {
        private string outCaseCode;
        private MesService.MesServiceClient serviceClient;
        public ProductPackageDetail(string outcasecode)
        {
            InitializeComponent();
            this.outCaseCode = outcasecode;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void ProductPackageDetail_Load(object sender, EventArgs e)
        {
            serviceClient = new MesService.MesServiceClient();
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewPackage,false);
            this.radGridViewPackage.ReadOnly = true;
            LoadDataSource(this.outCaseCode);
        }

        private void Btn_selectOfPackage_Click(object sender, EventArgs e)
        {
            LoadDataSource(this.tb_package.Text);
        }

        private void LoadDataSource(string queryFilter)
        {
            var dt = serviceClient.SelectPackageProduct(queryFilter, "1", true).Tables[0];
            this.radGridViewPackage.DataSource = null;
            this.radGridViewPackage.DataSource = dt;
        }
    }
}
