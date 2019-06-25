using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using CommonUtils.Logger;

namespace RetrospectiveManager
{
    public partial class ProductType : RadForm
    {
        private MesService.MesServiceClient mesService;
        private const string DATA_ORDER_NAME = "序号";
        private const string DATA_STATION_NAME = "型号名称";
        private DataTable dataSource;

        public ProductType()
        {
            InitializeComponent();
            this.MaximizeBox = false;
        }

        private void ProductType_Load(object sender, EventArgs e)
        {
            mesService = new MesService.MesServiceClient();
            InitDataSource();
            SetRadGridViewProperty();

            btn_commit.Click += Btn_commit_Click;
            btn_select.Click += Btn_select_Click;
            btn_clear_server.Click += Btn_clear_server_Click;

            this.radGridView1.ContextMenuOpening += RadGridView1_ContextMenuOpening;
            this.radGridView1.MouseDown += RadGridView1_MouseDown;
        }

        private string curRowStationName;
        private void RadGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (this.radGridView1.CurrentRow.Index < 0)
                    return;
                curRowStationName = this.radGridView1.CurrentRow.Cells[1].Value.ToString().Trim();
            }
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
                        e.ContextMenu.Items[i].Click += SetCutProductType_Click;
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
                        e.ContextMenu.Items[i].Click += SetCutProductType_Click; ;
                        break;
                }
            }
        }

        private void SetCutProductType_Click(object sender, EventArgs e)
        {
            DeleteProduceData();
        }

        async private void DeleteProduceData()
        {
            //cut 执行delete 服务数据
            if (MessageBox.Show("是否删除该行数据", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                int del = await mesService.DeleteProductTypeAsync(curRowStationName);
            }
            SelectServiceData("");
        }

        async private void Btn_clear_server_Click(object sender, EventArgs e)
        {
            //清空服务所有数据
            if (MessageBox.Show("是否删除数据库服务所有数据", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                int del = await mesService.DeleteAllProductTypeAsync();
            }
            SelectServiceData("");
        }

        private void Btn_select_Click(object sender, EventArgs e)
        {
            //查询/搜索
            SelectServiceData(tbx_select_filter.Text.Trim());
        }

        private void Btn_commit_Click(object sender, EventArgs e)
        {
            CommitMesService();
        }

        private DataTable InitDataSource()
        {
            if (dataSource == null)
            {
                dataSource = new DataTable();
                dataSource.Columns.Add(DATA_ORDER_NAME);
                dataSource.Columns.Add(DATA_STATION_NAME);
            }
            return dataSource;
        }

        /// <summary>
        /// 设置视图属性
        /// </summary>
        private void SetRadGridViewProperty()
        {
            radGridView1.EnableGrouping = false;
            radGridView1.AllowDrop = true;
            radGridView1.AllowRowReorder = true;
            /////显示每行前面的标记
            radGridView1.AddNewRowPosition = Telerik.WinControls.UI.SystemRowPosition.Bottom;
            radGridView1.ShowRowHeaderColumn = true;
            radGridView1.AutoSizeColumnsMode = Telerik.WinControls.UI.GridViewAutoSizeColumnsMode.Fill;
            radGridView1.ReadOnly = false;
            //gridView.ColumnChooserSortOrder = RadSortOrder.Ascending;
            //dgv.AllowRowHeaderContextMenu = false;
        }

        /// <summary>
        /// 查询数据，并显示
        /// </summary>
        async private void SelectServiceData(string filterText)
        {
            //调用查询接口
            DataSet dataSet = await mesService.SelectProductTypeAsync(filterText);
            DataTable dataTable = dataSet.Tables[0];
            dataSource.Clear();
            if (dataTable.Rows.Count > 0)
            {
                //显示数据
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dr = dataSource.NewRow();
                    dr[DATA_ORDER_NAME] = i + 1;
                    dr[DATA_STATION_NAME] = dataTable.Rows[i][1].ToString();
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
            //将新增数据提交到服务
            try
            {
                int row = radGridView1.RowCount;
                Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
                for (int i = 0; i < row; i++)
                {
                    var ID = radGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                    var productName = radGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                    
                    if (keyValuePairs.ContainsValue(productName))
                    {
                        this.radGridView1.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                        this.radGridView1.Rows[i].Cells[1].BeginEdit();
                        return;
                    }

                    this.radGridView1.Rows[i].Cells[0].Style.ForeColor = Color.Black;
                    this.radGridView1.Rows[i].Cells[1].Style.ForeColor = Color.Black;
                    keyValuePairs.Add(int.Parse(ID), productName);
                }
                string res = await mesService.CommitProductTypeAsync(keyValuePairs);
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
    }
}
