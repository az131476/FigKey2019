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

namespace MesManager.RadView
{
    //物料绑定信息：序号+物料编码（可选）+ 产品型号（可选）+ 工站名称（可选）+描述
    public partial class ProductMaterial : RadForm
    {
        MesService.MesServiceClient serviceClient;
        public ProductMaterial()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void ProductMaterial_Load(object sender, EventArgs e)
        {
            Init();
            cb_type_no.SelectedIndexChanged += Cb_type_no_SelectedIndexChanged;
        }

        private void Cb_type_no_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCheckItem(cb_type_no.Text);
        }

        private void Init()
        {
            serviceClient = new MesService.MesServiceClient();
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1, true);
            this.radGridView1.DataSource = null;
            ListViewCommon.InitListView(this.listView);
            //设置列
            this.listView.Columns.Add("物料编码", listView.Width - 5, HorizontalAlignment.Left);
            InitControl();
            //BindingDataSource();
            Test();
        }
        private void Test()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("TextBoxColumn");
            for (int i = 0; i < 5; i++)
            {
                DataRow dr = dt.NewRow();
                dr["TextBoxColumn"] = i+"testdata";
                dt.Rows.Add(dr);
            }
            DataGridViewTextBoxColumn col1 = new DataGridViewTextBoxColumn();
            col1.Name = "Name";
            col1.HeaderText = "姓名" ;
            dgview.Columns.Add(col1);

            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(this.dgview);
            row.SetValues("vale");
            dgview.Rows.Add(row);
            //this.dgview.Columns["TextBoxColumn"].DataPropertyName = dt.Columns["TextBoxColumn"].ToString();
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
            GridViewTextBoxColumn order = this.radGridView1.Columns["rdvc_order"] as GridViewTextBoxColumn;
            GridViewComboBoxColumn materialCode = this.radGridView1.Columns[DataGridViewColumnName.rdvc_materialCode.ToString()] as GridViewComboBoxColumn;
            GridViewComboBoxColumn productTypeNo = this.radGridView1.Columns[DataGridViewColumnName.rdvc_typeNo.ToString()] as GridViewComboBoxColumn;
            GridViewTextBoxColumn describle = this.radGridView1.Columns[DataGridViewColumnName.rdvc_describle.ToString()] as GridViewTextBoxColumn;
            

            DataTable materialCodeDt = (await serviceClient.SelectMaterialAsync()).Tables[0];//0
            DataTable typeNoDt = (await serviceClient.SelectProductTypeNoAsync("")).Tables[0];//1

            List<string> materialListTemp = new List<string>();
            List<string> typeNoListTemp = new List<string>();

            this.radGridView1.BeginEdit();

            for (int i = 0; i < materialCodeDt.Rows.Count; i++)
            {
                materialListTemp.Add(materialCodeDt.Rows[i][0].ToString());
                this.dgv_materialcode.Items.Add(materialCodeDt.Rows[i][0].ToString());
            }
            for (int i = 0; i < typeNoDt.Rows.Count; i++)
            {
                typeNoListTemp.Add(typeNoDt.Rows[i][0].ToString());
                this.dgv_typeno.Items.Add(typeNoDt.Rows[i][0].ToString());
            }
            materialCode.DataSource = materialListTemp;
            productTypeNo.DataSource = typeNoListTemp;

            DataTable dt = (await serviceClient.SelectProductMaterialAsync("")).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.radGridView1.Rows[i].Cells["rdvc_order"].Value = i + 1;
                this.radGridView1.Rows[i].Cells[1].Value = dt.Rows[i][1].ToString();
                this.radGridView1.Rows[i].Cells[2].Value = dt.Rows[i][0].ToString();
                this.radGridView1.Rows[i].Cells[3].Value = dt.Rows[i][2].ToString();
                
                //this.radGridView1.Columns[""].GetValueSource();
            }
            this.radGridView1.EndEdit();
        }

        async private void InitControl()
        {
            //获取型号
            cb_type_no.Items.Clear();
            DataTable dt = (await serviceClient.SelectProductTypeNoAsync("")).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cb_type_no.Items.Add(dt.Rows[i][0].ToString());
            }
            cb_type_no.Items.Add("");
            cb_type_no.SelectedIndex = 0;
            //获取物料编码
            dt = (await serviceClient.SelectMaterialAsync()).Tables[0];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                this.listView.Items.Add(dt.Rows[i][0].ToString());
            }
            UpdateCheckItem(cb_type_no.Text);
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Btn_commit_Click(object sender, EventArgs e)
        {
            CommitRemote();
        }

        async private void CommitRemote()
        {
            if (string.IsNullOrEmpty(cb_type_no.Text.Trim()))
            {
                MessageBox.Show("零件号不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int j = 0;
            int checkCount = 0;
            Dictionary<string, string[]> keyValuePairs = new Dictionary<string, string[]>();
            foreach (ListViewItem item in this.listView.Items)
            {
                if (item.Checked)
                {
                    checkCount++;
                }
            }
            string[] array = new string[checkCount];
            foreach (ListViewItem item in this.listView.Items)
            {
                if (item.Checked)
                {
                    array[j] = item.Text;
                    j++;
                }
            }
            keyValuePairs.Add(cb_type_no.Text.Trim(), array);
            string res = await serviceClient.CommitProductMaterialAsync(keyValuePairs);

            if (res == "0")
                MessageBox.Show("更新失败！");
            else if (res == "1")
                MessageBox.Show("更新成功！");
            else
                MessageBox.Show("更新状态：" + res);
        }

        async private void UpdateCheckItem(string typeNo)
        {
            if (this.listView.Items.Count == 0)
                return;
           DataTable dt = (await serviceClient.SelectProductMaterialAsync(typeNo)).Tables[0];
            if (dt.Rows.Count < 1 || string.IsNullOrEmpty(typeNo))
            {
                foreach (ListViewItem listViewItem in this.listView.Items)
                {
                    listViewItem.Checked = false;
                }
                return;
            }

            for (int i = 0; i < this.listView.Items.Count; i++)
            {
                var val = this.listView.Items[i].Text;
                if (IsExistMaterialCode(val, dt))
                {
                    this.listView.Items[i].Checked = true;
                }
                else
                {
                    this.listView.Items[i].Checked = false;
                }
            }
        }

        private bool IsExistMaterialCode(string val,DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (val.Equals(dt.Rows[i][0].ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        private void Menu_add_Click(object sender, EventArgs e)
        {
            
        }
    }
}
