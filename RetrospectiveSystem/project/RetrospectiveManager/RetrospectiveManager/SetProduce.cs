using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CommonUtils.CalculateAndString;
using CommonUtils.Logger;
using Telerik.WinControls.UI;

namespace RetrospectiveManager
{
    public partial class SetProduce : Telerik.WinControls.UI.RadForm
    {
        private DataTable dataSource;
        private MesService.MesServiceClient mesService;
        private const string DATA_ORDER_NAME = "序号";
        private const string DATA_STATION_NAME = "站位名称";
        public SetProduce()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            rlbx_explain.Text = "按顺序添加生产线包含的站位，也可修改站位信息";
        }

        private DataTable DataSource()
        {
            if (dataSource == null)
            {
                dataSource = new DataTable();
                dataSource.Columns.Add(DATA_ORDER_NAME);
                dataSource.Columns.Add(DATA_STATION_NAME);
            }
            return dataSource;
        }

        private void SetProduce_Load(object sender, EventArgs e)
        {
            mesService = new MesService.MesServiceClient();
            DataSource();
            SetRadGridViewProperty();
            radGridView1.DataSource = dataSource;


            btn_cancel.Click += Btn_cancel_Click;
            btn_select.Click += Btn_select_Click;
            btn_apply.Click += Btn_apply_Click;
            btn_clear_dgv.Click += Btn_clear_dgv_Click;
            btn_clear_server_data.Click += Btn_clear_server_data_Click;

            radGridView1.MouseDown += RadGridView1_MouseDown;
            radGridView1.ContextMenuOpening += RadGridView1_ContextMenuOpening;
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
                        e.ContextMenu.Items[i].Click += SetCutProduce_Click;
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
                        e.ContextMenu.Items[i].Click += SetDeleteProduce_Click; ;
                        break;
                }
            }
        }

        private void SetDeleteProduce_Click(object sender, EventArgs e)
        {
            DeleteProduceData();
        }

        private void SetCutProduce_Click(object sender, EventArgs e)
        {
            DeleteProduceData();
        }

        private void Btn_clear_dgv_Click(object sender, EventArgs e)
        {
            dataSource.Clear();
            this.radGridView1.DataSource = dataSource;
        }

        async private void DeleteProduceData()
        {
            //cut 执行delete 服务数据
            if (MessageBox.Show("是否删除该行数据", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                int del = await mesService.DeleteProduceAsync(curRowStationName);
            }
            SelectData();
        }

        async private void Btn_clear_server_data_Click(object sender, EventArgs e)
        {
            //清除所有数据
            if (dataSource.Rows.Count == 0)
                return;
            DialogResult dialogResult = MessageBox.Show("是否删除数据库服务中得数据", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (dialogResult == DialogResult.OK)
            {
                int del = await mesService.DeleteAllProduceAsync();
                //更新显示
                SelectData();
            }
        }


        /*
* 其他事件处理
* 1、移动到下一行时，自动增加序列号
* 2、提交应用时，验证是否按顺序排列
* 3、验证提交内容是否为空
*/

        private void Btn_apply_Click(object sender, EventArgs e)
        {
            //将新增数据提交到服务
            //将修改数据更新到服务
            CommitMesService();
        }

        private void CommitMesService()
        {
            //将新增数据提交到服务
            try
            {
                int row = radGridView1.RowCount;
                Dictionary<int, string> keyValuePairs = new Dictionary<int, string>();
                for (int i = 0; i < row; i++)
                {
                    var ID = radGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                    var stationName = radGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                    if (!ExamineInputFormat.IsDecimal(ID))
                    {
                        MessageBox.Show("输入ID格式不正确！");
                        return;
                    }
                    if (keyValuePairs.ContainsKey(int.Parse(ID)))
                    {
                        //已包含该ID
                        this.radGridView1.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                        this.radGridView1.Rows[i].Cells[0].BeginEdit();
                        return;
                    }
                    if (keyValuePairs.ContainsValue(stationName))
                    {
                        this.radGridView1.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                        this.radGridView1.Rows[i].Cells[1].BeginEdit();
                        return;
                    }

                    this.radGridView1.Rows[i].Cells[0].Style.ForeColor = Color.Black;
                    this.radGridView1.Rows[i].Cells[1].Style.ForeColor = Color.Black;
                    keyValuePairs.Add(int.Parse(ID), stationName);
                }
                string res = mesService.InsertProduce(keyValuePairs);
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
                LogHelper.Log.Error(ex.Message+"\r\n"+ex.StackTrace);
            }
        }

        private void Btn_select_Click(object sender, EventArgs e)
        {
            SelectData();
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
        async private void SelectData()
        {
            //调用查询接口
            DataSet dataSet = await mesService.SelectProduceAsync();
            DataTable dataTable = dataSet.Tables[0];
            dataSource.Clear();
            if (dataTable.Rows.Count > 0)
            {
                //显示数据
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dr = dataSource.NewRow();
                    dr[DATA_ORDER_NAME] = dataTable.Rows[i][0].ToString();
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
    }
}
