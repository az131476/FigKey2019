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

namespace RetrospectiveManager
{
    public partial class SetProduce : Telerik.WinControls.UI.RadForm
    {
        private DataTable dataSource;
        private MesService.MesServiceClient mesService;
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
                dataSource.Columns.Add("序号");
                dataSource.Columns.Add("站位名称");
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
                    var ID = radGridView1.Rows[i].Cells[0].Value.ToString();
                    var stationName = radGridView1.Rows[i].Cells[1].Value.ToString();
                    if (!ExamineInputFormat.IsDecimal(ID))
                    {
                        MessageBox.Show("输入ID格式不正确！");
                        return;
                    }
                    keyValuePairs.Add(int.Parse(ID), stationName);
                }
                string res = mesService.InsertProduce(keyValuePairs);
                MessageBox.Show("结果：" + res);
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message+"\r\n"+ex.StackTrace);
            }
        }

        async private void Btn_select_Click(object sender, EventArgs e)
        {
            //调用查询接口
            DataSet dataSet = await mesService.SelectProduceAsync();
            DataTable dataTable = dataSet.Tables[0];
            if (dataTable.Rows.Count > 0)
            {
                //显示数据
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dr = dataSource.NewRow();
                    
                }
                dataSource = dataTable.Copy();
                radGridView1.DataSource = dataSource;
            }
            else
            {
                MessageBox.Show(dataTable.Rows.Count+"");
            }
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
    }
}
