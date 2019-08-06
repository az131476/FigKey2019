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
    public partial class SNCenter : RadForm
    {
        private MesService.MesServiceClient serviceClient;
        public SNCenter()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void SNCenter_Load(object sender, EventArgs e)
        {
            Init();
            EventHandlers();
        }

        private void EventHandlers()
        {
            this.menu_sn_result.Click += Menu_sn_result_Click;
            this.menu_material.Click += Menu_material_Click;
            this.menu_package.Click += Menu_package_Click;
            this.menu_quanlity.Click += Menu_quanlity_Click;
            this.menu_productCheck.Click += Menu_productCheck_Click;
        }

        private void Menu_productCheck_Click(object sender, EventArgs e)
        {
            
        }

        private void Menu_quanlity_Click(object sender, EventArgs e)
        {
            this.panel_quanlity.Dock = DockStyle.Fill;
            SetPanelFalse();
            this.panel_quanlity.Visible = true;
        }

        private void Menu_package_Click(object sender, EventArgs e)
        {
            this.panel_package.Dock = DockStyle.Fill;
            SetPanelFalse();
            this.panel_package.Visible = true;
        }

        private void Menu_material_Click(object sender, EventArgs e)
        {
            this.panel_material.Dock = DockStyle.Fill;
            SetPanelFalse();
            this.panel_material.Visible = true;
        }

        private void Menu_sn_result_Click(object sender, EventArgs e)
        {
            this.panel_sn.Dock = DockStyle.Fill;
            SetPanelFalse();
            this.panel_sn.Visible = true;
        }

        private void Init()
        {
            rbtn_materialed.Checked = true;
            serviceClient = new MesService.MesServiceClient();
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewSn, false);
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewPackage, false);
            DataGridViewCommon.SetRadGridViewProperty(this.radGridViewMaterial, false);
            SetPanelFalse();
        }

        private void SetPanelFalse()
        {
            this.panel_sn.Visible = false;
            this.panel_material.Visible = false;
            this.panel_package.Visible = false;
            this.panel_quanlity.Visible = false;
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
                dt = (await serviceClient.SelectMaterialMsgAsync(materialMsg, true)).Tables[0];

            }
            else if (rbtn_materialed.Checked)
            {
                //查询已使用
                dt = (await serviceClient.SelectMaterialMsgAsync(materialMsg, false)).Tables[0];
            }
            this.radGridViewMaterial.DataSource = dt;
        }

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
    }
}
