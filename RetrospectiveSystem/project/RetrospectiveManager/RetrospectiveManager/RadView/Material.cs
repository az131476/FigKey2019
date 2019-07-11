using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using RetrospectiveManager.Control;
using CommonUtils.Logger;

namespace RetrospectiveManager.RadView
{
    public partial class Material : RadForm
    {
        private MesService.MesServiceClient serviceClient;
        private DataTable dataSource;
        private const string DATA_ORDER = "序号";
        private const string DATA_MATERIAL = "物料名称";
        private const string DATA_AMOUNT = "物料数量";
        private string keyMaterialCode,keyMaterialAmount;

        public Material()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        async private void Material_Load(object sender, EventArgs e)
        {
            serviceClient = new MesService.MesServiceClient();
            await serviceClient.InitConnectStringAsync();
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1);
            //设置第一列为只读
            this.radGridView1.DataSource = DataSource();
            this.radGridView1.Columns[0].ReadOnly = true;

            this.radGridView1.CellBeginEdit += RadGridView1_CellBeginEdit;
            this.radGridView1.CellEndEdit += RadGridView1_CellEndEdit;
        }

        private void RadGridView1_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            //结束编辑，记录下value;与编辑前比较，值改变则执行修改
            string curMaterialCode = "";
            string curMaterialAmount = "";
            if (this.radGridView1.CurrentRow.Cells[1].Value != null)
                curMaterialCode = this.radGridView1.CurrentRow.Cells[1].Value.ToString();
            if (this.radGridView1.CurrentRow.Cells[2].Value != null)
                curMaterialAmount = this.radGridView1.CurrentRow.Cells[2].Value.ToString();

            if (curMaterialCode != keyMaterialCode)
            {
                
            }
            if (curMaterialAmount != keyMaterialAmount)
            {
            }
        }

        private void RadGridView1_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            //开始编辑，记录下value
            if(this.radGridView1.CurrentRow.Cells[1].Value != null)
                keyMaterialCode = this.radGridView1.CurrentRow.Cells[1].Value.ToString();
            if (this.radGridView1.CurrentRow.Cells[2].Value != null)
                keyMaterialAmount = this.radGridView1.CurrentRow.Cells[2].Value.ToString();
        }

        private DataTable DataSource()
        {
            if (dataSource == null)
            {
                dataSource = new DataTable();
                dataSource.Columns.Add(DATA_ORDER);
                dataSource.Columns.Add(DATA_MATERIAL);
                dataSource.Columns.Add(DATA_AMOUNT);
            }
            return dataSource;
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_apply_Click(object sender, EventArgs e)
        {
            CommitMesService();
        }

        private void Btn_clear_dgv_Click(object sender, EventArgs e)
        {
            dataSource.Clear();
            this.radGridView1.DataSource = dataSource;
        }

        private void Btn_clear_server_data_Click(object sender, EventArgs e)
        {

        }

        private void Btn_select_Click(object sender, EventArgs e)
        {
            SelectMaterial();
        }

        #region 调用接口
        async private void SelectMaterial()
        {
            //调用查询接口
            DataSet dataSet = await serviceClient.SelectMaterialAsync();
            DataTable dataTable = dataSet.Tables[0];
            dataSource.Clear();
            if (dataTable.Rows.Count > 0)
            {
                //显示数据
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dr = dataSource.NewRow();
                    dr[DATA_ORDER] = i + 1;
                    dr[DATA_MATERIAL] = dataTable.Rows[i][0].ToString();
                    dr[DATA_AMOUNT] = dataTable.Rows[i][1].ToString();
                    dataSource.Rows.Add(dr);
                }
                radGridView1.DataSource = dataSource;
            }
            else
            {
                dataSource.Clear();
                radGridView1.DataSource = dataSource;
            }
        }

        async private void CommitMesService()
        {
            try
            {
                //提交新增行记录、修改非主键记录
                int row = radGridView1.RowCount;
                MesService.MaterialMsg[] materialMsg = new MesService.MaterialMsg[row];
                for (int i = 0; i < row; i++)
                {
                    MesService.MaterialMsg material = new MesService.MaterialMsg();
                    var materialCode = radGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                    var amount = radGridView1.Rows[i].Cells[2].Value.ToString().Trim();
                    material.MaterialCode = materialCode;
                    material.MaterialAmount = int.Parse(amount);
                    materialMsg[i] = material;
                }
                string res = await serviceClient.CommitMaterialAsync(materialMsg);
                if (res == "1")
                {
                    MessageBox.Show("更新成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show($"更新失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message + "\r\n" + ex.StackTrace);
            }
        }
        #endregion
    }
}
