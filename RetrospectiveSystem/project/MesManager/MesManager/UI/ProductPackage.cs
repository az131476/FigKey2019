using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using MesManager.Properties;
using MesManager.Control;

namespace MesManager.UI
{
    public partial class ProductPackage : RadForm
    {
        private MesService.MesServiceClient serviceClient;
        private int snLength1, snLength2;
        public ProductPackage()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void ProductPackage_Load(object sender, EventArgs e)
        {
            Init();
            EventHandlers();
        }

        private void Init()
        {
            serviceClient = new MesService.MesServiceClient();
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1,false);
            this.radGridView1.Columns[0].Width = 15;
            
            var len1 = ConfigurationManager.AppSettings["snLength1"].ToString();
            var len2 = ConfigurationManager.AppSettings["snLength2"].ToString();
            if (!string.IsNullOrEmpty(len1) && !string.IsNullOrEmpty(len2))
            {
                if (!int.TryParse(len1, out snLength1) || !int.TryParse(len2,out snLength2))
                {
                    MessageBox.Show("配置文件格式错误！","提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
            }
        }

        private void EventHandlers()
        {
            this.tb_sn.TextChanged += Tb_sn_TextChanged;
            this.radGridView1.CellClick += RadGridView1_CellClick;
            this.menu_update.Click += Menu_update_Click;
        }

        private void Menu_update_Click(object sender, EventArgs e)
        {
            
        }

        private void RadGridView1_CellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;
            if (e.ColumnIndex == 0)
            {
                this.radGridView1.Rows[e.RowIndex].Delete();
            }
        }

        private void Tb_sn_TextChanged(object sender, EventArgs e)
        {
            tb_sn.Text = tb_sn.Text.Trim();
            if (tb_sn.TextLength == snLength1 || tb_sn.TextLength == snLength2)
            {
                AddBindingData();
            }
        }

        /// <summary>
        /// 扫描SN，查询该产品信息，添加到列表
        /// </summary>
        private void AddBindingData()
        {
            //查询信息：序号+SN+型号+绑定状态
            this.radGridView1.Rows.AddNew();
            int startIndex = this.radGridView1.Rows.Count - 1;
            this.radGridView1.Rows[startIndex].Cells[0].Value = Resources.bullet_delete;
            this.radGridView1.Rows[startIndex].Cells[1].Value = startIndex + 1;
            this.radGridView1.Rows[startIndex].Cells[2].Value = tb_sn.Text.Trim();
            this.tool_curNumber.Text = this.radGridView1.Rows.Count.ToString();
            this.tool_materialCode.Text = tb_materialCode.Text;
        }
    }
}
