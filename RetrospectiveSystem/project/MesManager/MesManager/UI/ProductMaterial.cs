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
using CommonUtils.Logger;
using MesManager.Common;

namespace MesManager.UI
{
    //物料绑定信息：序号+物料编码（可选）+ 产品型号（可选）+ 工站名称（可选）+描述
    public partial class ProductMaterial : RadForm
    {
        MesService.MesServiceClient serviceClient;
        private string keyMaterialCode;
        private string keyTypeNo;
        private string keyDescrible;

        List<ProductMaterial> pmListTemp;
        private int editRowIndex;
        public ProductMaterial()
        {
            InitializeComponent();
            //this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void ProductMaterial_Load(object sender, EventArgs e)
        {
            Init();
            EventHandlers();
        }

        private void Init()
        {
            serviceClient = new MesService.MesServiceClient();
            pmListTemp = new List<ProductMaterial>();
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1, true);
            this.radGridView1.DataSource = null;
            BindingDataSource();
        }

        private void EventHandlers()
        {
            menu_add_row.Click += Menu_add_row_Click;
            menu_delete.Click += Menu_delete_Click;
            menu_update.Click += Menu_update_Click;
            menu_refresh.Click += Menu_refresh_Click;
            menu_clear_db.Click += Menu_clear_db_Click;
            menu_grid.Click += Menu_grid_Click;

            this.radGridView1.ContextMenuOpening += RadGridView1_ContextMenuOpening;
            this.radGridView1.CellBeginEdit += RadGridView1_CellBeginEdit;
            this.radGridView1.CellEndEdit += RadGridView1_CellEndEdit;
        }

        private void RadGridView1_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            //结束编辑，记录下value;与编辑前比较，值改变则执行修改
            var typeNo = this.radGridView1.CurrentRow.Cells[1].Value;
            var materialPN = this.radGridView1.CurrentRow.Cells[2].Value;
            var describle = this.radGridView1.CurrentRow.Cells[3].Value;
            if (typeNo == null || materialPN == null || describle == null)
                return;
            if (materialPN.ToString().Contains("("))
                materialPN = materialPN.ToString().Substring(0, materialPN.ToString().IndexOf('('));

            if (materialPN.ToString() != keyMaterialCode || typeNo.ToString() != keyTypeNo || this.keyDescrible != describle.ToString())
            {
                ProductMaterial productMaterial = new ProductMaterial();
                productMaterial.keyMaterialCode = keyMaterialCode;

                productMaterial.keyTypeNo = keyTypeNo;
                pmListTemp.Add(productMaterial);
            }
        }

        private void RadGridView1_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            //开始编辑，记录下value
            var typeNo = this.radGridView1.CurrentRow.Cells[1].Value;
            var materialPN = this.radGridView1.CurrentRow.Cells[2].Value;
            var describle = this.radGridView1.CurrentRow.Cells[3].Value;
            if (typeNo == null || materialPN == null || describle == null)
                return;
            this.keyTypeNo = typeNo.ToString();
            if (materialPN.ToString().Contains("("))
                materialPN = materialPN.ToString().Substring(0, materialPN.ToString().IndexOf('('));
            this.keyMaterialCode = materialPN.ToString();
            this.keyDescrible = describle.ToString();
        }

        private void RadGridView1_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            for (int i = 0; i < e.ContextMenu.Items.Count; i++)
            {
                String contextMenuText = e.ContextMenu.Items[i].Text;
                switch (contextMenuText)
                {
                    case "Conditional Formatting":
                        e.ContextMenu.Items[i].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                        e.ContextMenu.Items[i + 1].Visibility = ElementVisibility.Collapsed;
                        break;
                    case "Hide Column":
                        e.ContextMenu.Items[i].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                        break;
                    case "Pinned state":
                        e.ContextMenu.Items[i].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                        break;
                    case "Best Fit":
                        e.ContextMenu.Items[i].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                        break;
                    case "Cut":
                        e.ContextMenu.Items[i].Click += Delete_Row_Click;
                        break;
                    case "Copy":
                        break;
                    case "Paste":
                        break;
                    case "Edit":
                        break;
                    case "Clear Value":
                        break;
                    case "Delete Row":
                        e.ContextMenu.Items[i].Click += Delete_Row_Click;
                        break;
                }
            }
        }

        private void Delete_Row_Click(object sender, EventArgs e)
        {
            DeleteRowData();
        }

        private void Menu_grid_Click(object sender, EventArgs e)
        {
            for (int i = this.radGridView1.Rows.Count - 1;i>=0; i--)
            {
                this.radGridView1.Rows[i].Delete();
            }
        }

        private void Menu_clear_db_Click(object sender, EventArgs e)
        {
            DeleteAllRowData();
        }

        private void Menu_refresh_Click(object sender, EventArgs e)
        {
            SelectData();
        }

        private void Menu_update_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void Menu_delete_Click(object sender, EventArgs e)
        {
            DeleteRowData();
        }

        private void Menu_add_row_Click(object sender, EventArgs e)
        {
            //新增空行
            this.radGridView1.CurrentRow = this.radGridView1.Rows[this.radGridView1.Rows.Count-1];
            this.radGridView1.Rows.AddNew();
        }

        public enum DataGridViewColumnName
        {
            rdvc_order,
            rdvc_materialCode,
            rdvc_typeNo,
            rdvc_station,
            rdvc_describle
        }
        async private void BindingDataSource()
        {
            GridViewTextBoxColumn order = this.radGridView1.Columns[DataGridViewColumnName.rdvc_order.ToString()] as GridViewTextBoxColumn;
            //GridViewComboBoxColumn materialCode = this.radGridView1.Columns[DataGridViewColumnName.rdvc_materialCode.ToString()] as GridViewComboBoxColumn;
            GridViewComboBoxColumn productTypeNo = this.radGridView1.Columns[DataGridViewColumnName.rdvc_typeNo.ToString()] as GridViewComboBoxColumn;
            GridViewTextBoxColumn describle = this.radGridView1.Columns[DataGridViewColumnName.rdvc_describle.ToString()] as GridViewTextBoxColumn;
            DataTable materialCodeDt = (await serviceClient.SelectMaterialPNAsync()).Tables[0];//0
            DataTable typeNoDt = (await serviceClient.SelectProductContinairCapacityAsync("")).Tables[0];//1

            List<string> materialListTemp = new List<string>();
            List<string> typeNoListTemp = new List<string>();

            this.radGridView1.BeginEdit();

            if (materialCodeDt.Rows.Count > 0)
            {
                for (int i = 0; i < materialCodeDt.Rows.Count; i++)
                {
                    var materialPN = materialCodeDt.Rows[i][0].ToString();
                    var materialName = materialCodeDt.Rows[i][1].ToString();

                    if (!materialListTemp.Contains(materialPN))
                    {
                        if (!string.IsNullOrEmpty(materialName))
                        {
                            materialListTemp.Add(materialPN + "(" + materialName + ")");
                        }
                        else
                        {
                            materialListTemp.Add(materialPN);
                        }
                    }
                }
            }
            if (typeNoDt.Rows.Count > 0)
            {
                for (int i = 0; i < typeNoDt.Rows.Count; i++)
                {
                    typeNoListTemp.Add(typeNoDt.Rows[i][0].ToString());
                }
            }
            //materialCode.DataSource = materialListTemp;
            productTypeNo.DataSource = typeNoListTemp;
            SelectData();
            this.radGridView1.EndEdit();
            //设置第一列不可编辑
            this.radGridView1.Columns[0].ReadOnly = true;
            this.radGridView1.Columns[4].ReadOnly = true;
            this.radGridView1.Columns[5].ReadOnly = true;
            this.pmListTemp.Clear();
        }

        async private void UpdateData()
        {
            this.radGridView1.CurrentRow = this.radGridView1.Rows[this.radGridView1.Rows.Count - 1];
            int rowCount = CalRowCount();
            MesService.ProductMaterial[] productMaterialList = new MesService.ProductMaterial[rowCount];
            int row = 0;
            foreach (var rowInfo in this.radGridView1.Rows)
            {
                MesService.ProductMaterial productMaterial = new MesService.ProductMaterial();
                if (rowInfo.Cells[1].Value != null)
                    productMaterial.TypeNo = rowInfo.Cells[1].Value.ToString();
                if (rowInfo.Cells[2].Value != null)
                {
                    var materialPn = rowInfo.Cells[2].Value.ToString();
                    if(materialPn.Contains("("))
                        materialPn = materialPn.Substring(0, materialPn.IndexOf('('));
                    productMaterial.MaterialCode = materialPn;
                    //更新编码库存
                    if (productMaterial.MaterialCode.Contains("&"))
                    {
                        productMaterial.Stock = AnalysisMaterialCode.GetMaterialDetail(productMaterial.MaterialCode).MaterialQTY;
                    }
                }
                if (rowInfo.Cells[3].Value != null)
                    productMaterial.Describle = rowInfo.Cells[3].Value.ToString();
                productMaterial.UserName = MESMainForm.currentUser;

                if (rowInfo.Cells[1].Value != null && rowInfo.Cells[2].Value != null)
                    productMaterialList[row] = productMaterial;
                row++;
            }
            //delete修改数据
            foreach (var item in pmListTemp)
            {
                MesService.ProductMaterial productMaterial = new MesService.ProductMaterial();
                productMaterial.MaterialCode = item.keyMaterialCode;
                productMaterial.TypeNo = item.keyTypeNo;
                int del = await serviceClient.DeleteProductMaterialAsync(productMaterial);
            }
            pmListTemp.Clear();
            MesService.ProductMaterial[] materialList = await serviceClient.CommitProductMaterialAsync(productMaterialList);
            foreach (var material in materialList)
            {
                if (material.Result != 1)
                {
                    MessageBox.Show("更新失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            MessageBox.Show("更新成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            SelectData();
        }

        async private void DeleteRowData()
        {
            MesService.ProductMaterial productMaterial = new MesService.ProductMaterial();
            if(this.radGridView1.CurrentRow.Cells[2].Value != null)
                productMaterial.MaterialCode = this.radGridView1.CurrentRow.Cells[2].Value.ToString();
            if(this.radGridView1.CurrentRow.Cells[1].Value != null)
                productMaterial.TypeNo = this.radGridView1.CurrentRow.Cells[1].Value.ToString();
            if (this.radGridView1.CurrentRow.Cells[1].Value == null && this.radGridView1.CurrentRow.Cells[2].Value == null 
                && this.radGridView1.CurrentRow.Cells[3].Value == null)
            {
                this.radGridView1.CurrentRow.Delete();
                return;
            }
            if (MessageBox.Show($"确认解除物料【{productMaterial.MaterialCode}】与产品【{productMaterial.TypeNo}】的绑定？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                int res = await serviceClient.DeleteProductMaterialAsync(productMaterial);
                if (res > 0)
                {
                    MessageBox.Show("解除绑定成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("解除绑定失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                SelectData();
            }
        }

        async private void DeleteAllRowData()
        {
            if (MessageBox.Show("确认删除所有数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.Cancel)
                return;
            int res = await serviceClient.DeleteAllProductContinairCapacityAsync();
            if (res > 0)
            {
                MessageBox.Show("清除服务数据完成！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("清除服务数据失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            SelectData();
        }

        async private void SelectData()
        {
            DataTable dt = (await serviceClient.SelectProductMaterialAsync()).Tables[0];
            this.radGridView1.DataSource = null;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.radGridView1.Rows.AddNew();//序号/型号/物料号/描述/操作员/更新日期
                this.radGridView1.Rows[i].Cells[0].Value = i + 1;
                this.radGridView1.Rows[i].Cells[1].Value = dt.Rows[i][0].ToString();
                var materialPN = dt.Rows[i][1].ToString();
                this.radGridView1.Rows[i].Cells[3].Value = dt.Rows[i][2].ToString();
                this.radGridView1.Rows[i].Cells[4].Value = dt.Rows[i][3].ToString();//用户
                this.radGridView1.Rows[i].Cells[5].Value = dt.Rows[i][4].ToString();//日期
                var materialName = serviceClient.SelectMaterialName(materialPN);
                if (materialName != "")
                {
                    materialName = "(" + materialName + ")";
                }
                this.radGridView1.Rows[i].Cells[2].Value = materialPN + materialName;
            }
            //删除空行
            int startIndex = dt.Rows.Count;
            int rowCount = this.radGridView1.Rows.Count;
            if (this.radGridView1.Rows.Count > dt.Rows.Count)
            {
                for (int i = rowCount - 1; i >= startIndex; i--)
                {
                    this.radGridView1.Rows[i].Delete();
                }
            }    
        }

        private int CalRowCount()
        {
            int count = 0;
            this.radGridView1.CurrentRow = this.radGridView1.Rows[this.radGridView1.Rows.Count - 1];

            foreach (var rowInfo in this.radGridView1.Rows)
            {
                if (rowInfo.Cells[1].Value != null && rowInfo.Cells[2].Value != null)
                {
                    count++;
                }
            }
            return count;
        }
    }
}
