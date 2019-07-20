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

        async private void Init()
        {
            serviceClient = new MesService.MesServiceClient();
            await serviceClient.InitConnectStringAsync();

            ListViewCommon.InitListView(this.listView);
            //设置列
            this.listView.Columns.Add("物料编码", listView.Width - 5, HorizontalAlignment.Left);
            InitControl();
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
    }
}
