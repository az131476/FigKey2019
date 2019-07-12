using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace RetrospectiveManager.RadView
{
    public partial class PackageProduct : Telerik.WinControls.UI.RadForm
    {
        private MesService.MesServiceClient serviceClient;
        private MesService.PackageProduct packageProduct;
        private DataTable dataSource;
        private const string CASE_CODE = "箱子编码";
        private const string SN_CODE = "追溯码";
        private const string TYPE_NO = "产品型号";
        private const string PICTURE = "图片";
        private const string BINDING_STATE = "绑定状态";
        private const string BINDING_DATE = "绑定日期";
        public PackageProduct()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void PackageProduct_Load(object sender, EventArgs e)
        {
            Init();
            cb_caseCode.SelectedIndexChanged += Cb_caseCode_SelectedIndexChanged;
            cb_caseCode.TextChanged += Cb_caseCode_TextChanged;
        }

        private void Cb_caseCode_TextChanged(object sender, EventArgs e)
        {
            foreach (var v in cb_caseCode.Items)
            {
                if (cb_caseCode.Text == v.ToString())
                {
                    UpdateCaseAmount(cb_caseCode.Text.Trim());
                }
            }
            if (cb_caseCode.Text.Length == 13)
            {
                //编码长度固定时有效
            }
        }

        private void Cb_caseCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCaseAmount(cb_caseCode.SelectedItem.ToString());
        }

        async private void UpdateCaseAmount(string caseCode)
        {
            //由箱子编码查询箱子容量，更新
            if (string.IsNullOrEmpty(caseCode))
                return;
            DataTable dt = (await serviceClient.SelectOutCaseBoxStorageAsync(caseCode)).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tb_case_amount.Text = dt.Rows[i][1].ToString();
                }
            }
        }

        async private void Init()
        {
            serviceClient = new MesService.MesServiceClient();
            await serviceClient.InitConnectStringAsync();
            packageProduct = new MesService.PackageProduct();
            DataTable dt = (await serviceClient.SelectOutCaseBoxStorageAsync("")).Tables[0];
            cb_caseCode.Items.Clear();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cb_caseCode.Items.Add(dt.Rows[i][0].ToString());
                }
            }
            DataSource();
        }

        private DataTable DataSource()
        {
            if (dataSource == null)
            {
                dataSource = new DataTable();
                dataSource.Columns.Add(CASE_CODE);
                dataSource.Columns.Add(SN_CODE);
                dataSource.Columns.Add(TYPE_NO);
                dataSource.Columns.Add(PICTURE);
                dataSource.Columns.Add(BINDING_STATE);
                dataSource.Columns.Add(BINDING_DATE);
            }
            return dataSource;
        }

        async private void CommitBinding()
        {
            string caseCode = cb_caseCode.Text.Trim();
            string caseAmount = tb_case_amount.Text.Trim();
            string sn = tb_sn.Text.Trim();
            string typeNo = cb_typeNo.Text.Trim();
            if (string.IsNullOrEmpty(caseCode))
            {
                tb_sn.Focus();
                MessageBox.Show("箱子编码不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(sn))
            {
                tb_sn.Focus();
                MessageBox.Show("箱子容量不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(sn))
            {
                tb_sn.Focus();
                MessageBox.Show("条码不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrEmpty(typeNo))
            {
                tb_sn.Focus();
                MessageBox.Show("零件号不能为空!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            //提交箱子容量
            await serviceClient.CommitOutCaseBoxStorageAsync(caseCode,caseAmount);
            packageProduct.CaseCode = caseCode;
            packageProduct.SnOutter = sn;
            packageProduct.TypeNo = typeNo;
            packageProduct.BindingState = 1;
            packageProduct.BindingDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            packageProduct.Picture = UpLoadImage.ProductImage;
            int x = await serviceClient.CommitPackageProductAsync(packageProduct);
            //绑定完成后，添加到显示列表
            if (x < 1)
            {
                MessageBox.Show("绑定失败！","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            SelectBindingData(packageProduct.CaseCode,packageProduct.SnOutter);
        }

        async private void SelectBindingData(string caseCode,string snOutter)
        {
            packageProduct.CaseCode = caseCode;
            packageProduct.SnOutter = snOutter;
            DataTable dt = (await serviceClient.SelectPackageProductAsync(packageProduct)).Tables[0];
            if (dt.Rows.Count < 1)
                return;
            DataRow dataRow = dt.Rows[0];
            dataSource.ImportRow(dataRow);
            this.radGridView1.DataSource = dataSource;
        }

        private void Btn_apply_Click(object sender, EventArgs e)
        {
            CommitBinding();
        }

        private void Btn_upLoad_Click(object sender, EventArgs e)
        {
            UpLoadImage upLoadImage = new UpLoadImage();
            upLoadImage.ShowDialog();
        }
    }
}
