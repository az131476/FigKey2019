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

namespace MesManager.UI
{
    public partial class ReportCenter : RadForm
    {
        private MesService.MesServiceClient serviceClient;
        public ReportCenter()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            Init();
            EventHandlers();
        }

        private void EventHandlers()
        {
            this.radButtonElementSN.Click += RadButtonElementSN_Click;
            this.radButtonElementQuanlity.Click += RadButtonElementQuanlity_Click;
            this.radButtonElementPackage.Click += RadButtonElementPackage_Click;
            this.radButtonElementMaterial.Click += RadButtonElementMaterial_Click;
        }

        private void Init()
        {
            this.radDock1.RemoveAllWindows();
            rbtn_materialed.Checked = true;
            serviceClient = new MesService.MesServiceClient();
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewSn,false);
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewPackage, false);
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewMaterial, false);
        }

        #region Event Handlers
        private void RadButtonElementMaterial_Click(object sender, EventArgs e)
        {
            this.radDock1.AddDocument(this.dw_material);
            this.radDock1.ActiveWindow = this.dw_material;
        }

        private void RadButtonElementPackage_Click(object sender, EventArgs e)
        {
            this.radDock1.AddDocument(this.dw_package);
            this.radDock1.ActiveWindow = this.dw_package;
        }

        private void RadButtonElementQuanlity_Click(object sender, EventArgs e)
        {
            this.radDock1.AddDocument(this.dw_quanlity);
            this.radDock1.ActiveWindow = this.dw_quanlity;
        }

        private void RadButtonElementSN_Click(object sender, EventArgs e)
        {
            this.radDock1.AddDocument(this.dw_sn);
            this.radDock1.ActiveWindow = this.dw_sn;
        }
        #endregion

        private void Btn_selectOfSn_Click(object sender, EventArgs e)
        {
            SelectOfSn();
        }

        private void Btn_selectOfPackage_Click(object sender, EventArgs e)
        {
            SelectOfPackage();
        }

        private void Btn_materialSelect_Click(object sender, EventArgs e)
        {
            SelectOfMaterial();
        }

        async private void SelectOfSn()
        {
            var filter = tb_sn.Text;
            DataSet dt = await serviceClient.SelectTestResultUpperAsync(filter, filter, filter, true);
            radGridViewSn.DataSource = dt.Tables[0];
        }

        async private void SelectOfPackage()
        {
            var filter = tb_package.Text;
            //箱子编码/追溯码/型号
            MesService.PackageProduct packageProduct = new MesService.PackageProduct();
            packageProduct.BindingState = 1;
            packageProduct.CaseCode = tb_package.Text;
            packageProduct.TypeNo = tb_package.Text;
            packageProduct.SnOutter = tb_package.Text;
            DataTable dt = (await serviceClient.SelectPackageProductAsync(packageProduct)).Tables[0];
            this.radGridViewPackage.DataSource = dt;
        }

        async private void SelectOfMaterial()
        {
            //物料信息表
            //物料编码+物料名称+所属型号+在哪个工序/站位消耗+该位置消耗数量

            MesService.MaterialMsg materialMsg = new MesService.MaterialMsg();
            materialMsg.MaterialCode = tb_material.Text;
            materialMsg.Sn_Inner = tb_material.Text;
            materialMsg.Sn_Outter = tb_material.Text;
            materialMsg.StationName = tb_material.Text;
            materialMsg.TypeNo = tb_material.Text;
            DataTable dt = null;
            if (rbtn_material.Checked)
            {
                //查询所有
                dt = (await serviceClient.SelectMaterialMsgAsync(materialMsg,true)).Tables[0];

            }
            else if (rbtn_materialed.Checked)
            {
                //查询已使用
                dt = (await serviceClient.SelectMaterialMsgAsync(materialMsg, false)).Tables[0];
            }
            this.radGridViewMaterial.DataSource = dt;
        }
    }
}
