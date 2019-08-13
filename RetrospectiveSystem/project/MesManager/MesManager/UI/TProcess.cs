using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using MesManager.Control;
using CommonUtils.Logger;
using Telerik.WinControls.UI;

namespace MesManager.UI
{
    public partial class TProcess : RadForm
    {
        private MesService.MesServiceClient serviceClient;
        private MesServiceTest.MesServiceClient serviceClientTest;
        private DataTable stationData;
        private List<string> stationListTemp;
        private string keyStation;        //记录修改前的编码
        private string curRowStationName;//记录鼠标右键选中行编码
        private const string DATA_ORDER = "序号";
        private const string DATA_STATION_NAME = "工序名称";
        private const string DATA_USER_NAME = "操作用户";
        private const string DATA_UPDATE_DATE = "更新日期";

        public TProcess()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }
        private void TProcess_Load(object sender, EventArgs e)
        {
            Init();
            EventHandlers();
        }


        private void Init()
        {
            serviceClient = new MesService.MesServiceClient();
            serviceClientTest = new MesServiceTest.MesServiceClient();
            stationListTemp = new List<string>();
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1, true);
            this.radGridView1.AllowRowHeaderContextMenu = false;
            DataSource();
            menu_del.Enabled = false;
            UpdateProcesList();
            SelectStationList(this.cb_processItem.Text.Trim());
        }

        private void DataSource()
        {
            if (stationData == null)
            {
                stationData = new DataTable();
                stationData.Columns.Add(DATA_ORDER);
                stationData.Columns.Add(DATA_STATION_NAME);
                stationData.Columns.Add(DATA_USER_NAME);
                stationData.Columns.Add(DATA_UPDATE_DATE);
            }
        }

        private void EventHandlers()
        {
            menu_refresh.Click += Menu_refresh_Click;
            menu_grid.Click += Menu_grid_Click;
            menu_clear_db.Click += Menu_clear_db_Click;
            menu_del.Click += Menu_del_Click;
            menu_add.Click += Menu_add_Click;
            this.cb_processItem.SelectedIndexChanged += Cb_processItem_SelectedIndexChanged;

            this.radGridView1.CellBeginEdit += RadGridView1_CellBeginEdit;
            this.radGridView1.CellEndEdit += RadGridView1_CellEndEdit;
        }

        private void Cb_processItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectStationList(this.cb_processItem.Text.Trim());
        }

        private void Menu_add_Click(object sender, EventArgs e)
        {
            this.radGridView1.Rows.NewRow();
        }

        async private void Menu_del_Click(object sender, EventArgs e)
        {
            //删除当前行
            if (MessageBox.Show("确认要删除当前行记录？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
            {
                var stationName = this.radGridView1.CurrentRow.Cells[1].Value.ToString();
                int row = await serviceClient.DeleteStationAsync(cb_processItem.Text.Trim(),stationName);
                //tool_status.Text = "【型号】删除1行记录 【删除】完成";
            }
        }

        async private void Menu_clear_db_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"确定要清空工艺【{this.cb_processItem.Text}】的所有数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                return;
            await serviceClient.DeleteAllStationAsync(this.cb_processItem.Text.Trim());
            SelectStationList(this.cb_processItem.Text.Trim());
        }

        private void Menu_grid_Click(object sender, EventArgs e)
        {
            this.radGridView1.DataSource = null;
        }

        private void RadGridView1_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            var key = this.radGridView1.CurrentRow.Cells[1].Value;
            if (key == null)
                return;
            if (keyStation != key.ToString())
            {
                stationListTemp.Add(keyStation);
            }
        }

        private void RadGridView1_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            var key = this.radGridView1.CurrentRow.Cells[1].Value;
            if (key != null)
            {
                this.keyStation = key.ToString();
            }
        }

        private void Menu_refresh_Click(object sender, EventArgs e)
        {
            UpdateProcesList();
            SelectStationList(this.cb_processItem.Text.Trim());
        }

        private void Menu_commit_Click(object sender, EventArgs e)
        {
            this.cb_processItem.Focus();
            CommitStationMesService();
        }

        async private void SelectStationList(string processName)
        {
            //调用查询接口
            if (string.IsNullOrEmpty(processName))
                return;
            radGridView1.DataSource = null;
            DataSet dataSet = await serviceClient.SelectStationListAsync(processName);
            DataTable dataTable = dataSet.Tables[0];
            stationData.Clear();
            if (dataTable.Rows.Count > 0)
            {
                //显示数据
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dr = stationData.NewRow();
                    dr[DATA_ORDER] = dataTable.Rows[i][0].ToString();
                    dr[DATA_STATION_NAME] = dataTable.Rows[i][1].ToString();
                    dr[DATA_USER_NAME] = dataTable.Rows[i][2].ToString();
                    dr[DATA_UPDATE_DATE] = dataTable.Rows[i][3].ToString();
                    stationData.Rows.Add(dr);
                    //this.radGridView1.Rows[i].Cells[0].Value = dataTable.Rows[i][0].ToString();
                }
                radGridView1.DataSource = stationData;
            }
            else
            {
                stationData.Clear();
                radGridView1.DataSource = stationData;
            }
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1, true);
            this.radGridView1.Columns[0].ReadOnly = true;
            this.radGridView1.Columns[2].ReadOnly = true;
            this.radGridView1.Columns[3].ReadOnly = true;
        }

        async private void UpdateProcesList()
        {
            this.cb_processItem.Items.Clear();
            this.cb_curprocess.Items.Clear();
            DataSet dataSet = await serviceClient.SelectProcessListAsync();
            DataTable dataTable = dataSet.Tables[0];
            stationData.Clear();
            if (dataTable.Rows.Count > 0)
            {
                //显示数据
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    this.cb_processItem.Items.Add(dataTable.Rows[i][0]);
                    this.cb_curprocess.Items.Add(dataTable.Rows[i][0]);
                }
            }
            this.cb_curprocess.Text = await serviceClientTest.SelectCurrentTProcessAsync();
            this.cb_processItem.Text = this.cb_curprocess.Text;
        }

        async private void CommitStationMesService()
        {
            try
            {
                int row = radGridView1.RowCount;
                MesService.Station[] stationsArray = new MesService.Station[row];
                //将新增数据
                for (int i = 0; i < row; i++)
                {
                    MesService.Station station = new MesService.Station();
                    var ID = radGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                    var stationName = radGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                    if (!string.IsNullOrEmpty(stationName))
                    {
                        station.StationID = int.Parse(ID);
                        station.StationName = stationName;
                        stationsArray[i] = station;
                    }
                }
                //修改数据
                if (stationListTemp.Count > 0)
                {
                    foreach (var station in stationListTemp)
                    {
                        await serviceClient.DeleteStationAsync(this.cb_processItem.Text.Trim(),station);
                    }
                }
                int res = await serviceClient.InsertStationAsync(stationsArray);
                if (res == 1)
                {
                    UpdateProcesList();//更新完成后，更新工艺流程可选项
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

        private void Btn_setprocess_Click(object sender, EventArgs e)
        {
            SetCurrentPorcess();
        }

        async private void SetCurrentPorcess()
        {
            if (!this.cb_curprocess.Items.Contains(this.cb_curprocess.Text))
            {
                MessageBox.Show(this.cb_curprocess.Text+"不存在，请重新选择！","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }
            foreach (var process in this.cb_curprocess.Items)
            {
                if (process.ToString() == this.cb_curprocess.Text)
                {
                    //设置为当前工艺
                    int upt = await serviceClient.SetCurrentProcessAsync(this.cb_curprocess.Text.Trim(),1);
                    if (upt > 0)
                    {
                        UpdateProcesList();
                        MessageBox.Show("设置成功！","提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    }
                }
                else
                {
                    await serviceClient.SetCurrentProcessAsync(process.ToString(), 0);
                }
            }
        }
    }
}
