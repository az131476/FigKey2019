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
using MesManager.Control;

namespace MesManager.UI
{
    /// <summary>
    /// 管理产品型号、基础物料、产线站位信息
    /// </summary>
    public partial class BasicConfig : RadForm
    {
        private MesService.MesServiceClient serviceClient;
        private DataTable typeNoData,stationData,materialData;
        private List<string> modifyTypeNoTemp;
        private List<string> materialCodeTemp;//存储用户修改的物料编码
        private List<string> stationListTemp;
        private string keyTypeNo;
        private string keyMaterialCode;//记录修改前的编码
        private string keyStation;
        private string curMaterialCode;//记录鼠标右键选中行编码
        private string curRowStationName;
        private string curRowTypeNo;
        private const string DATA_ORDER = "序号";
        private const string DATA_MATERIAL = "物料编码";
        private const string DATA_AMOUNT = "物料库存";
        private const string DATA_TYPENO_NAME = "型号名称";
        private const string DATA_STATION_NAME = "站位名称";

        public BasicConfig()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void BasicConfig_Load(object sender, EventArgs e)
        {
            Init();
            EventHandlers();
        }

        private void Init()
        {
            serviceClient = new MesService.MesServiceClient();
            modifyTypeNoTemp = new List<string>();
            materialCodeTemp = new List<string>();
            stationListTemp = new List<string>();
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1,true);
            this.radGridView1.AllowRowHeaderContextMenu = false;
            DataSource();
            cb_cfgType.Items.Clear();
            cb_cfgType.Items.Add("工艺配置");
            cb_cfgType.Items.Add("型号配置");
            cb_cfgType.Items.Add("物料配置");
            cb_cfgType.SelectedIndex = 0;
            menu_del.Enabled = false;
            RefreshData();
        }

        private void DataSource()
        {
            if (materialData == null)
            {
                materialData = new DataTable();
                materialData.Columns.Add(DATA_ORDER);
                materialData.Columns.Add(DATA_MATERIAL);
                materialData.Columns.Add(DATA_AMOUNT);
            }
            if (stationData == null)
            {
                stationData = new DataTable();
                stationData.Columns.Add(DATA_ORDER);
                stationData.Columns.Add(DATA_STATION_NAME);
            }
            if (typeNoData == null)
            {
                typeNoData = new DataTable();
                typeNoData.Columns.Add(DATA_ORDER);
                typeNoData.Columns.Add(DATA_TYPENO_NAME);
            }
        }

        private void EventHandlers()
        {
            menu_refresh.Click += Menu_refresh_Click;
            menu_grid.Click += Menu_grid_Click;
            menu_clear_db.Click += Menu_clear_db_Click;
            menu_del.Click += Menu_del_Click;
            menu_add.Click += Menu_add_Click;

            cb_cfgType.SelectedIndexChanged += Cb_cfgType_SelectedIndexChanged;
            this.radGridView1.CellBeginEdit += RadGridView1_CellBeginEdit;
            this.radGridView1.CellEndEdit += RadGridView1_CellEndEdit;
        }

        private void Menu_add_Click(object sender, EventArgs e)
        {
            this.radGridView1.Rows.NewRow();
        }

        async private void Menu_del_Click(object sender, EventArgs e)
        {
            //删除当前行
            if (cb_cfgType.SelectedIndex == 1)
            {
                if (MessageBox.Show("确认要删除当前行记录？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    var typeNo = this.radGridView1.CurrentRow.Cells[1].Value.ToString();
                    int row = await serviceClient.DeleteProductTypeNoAsync(typeNo);
                    tool_status.Text = "【型号】删除1行记录 【删除】完成";
                }
            }
            else if (cb_cfgType.SelectedIndex == 2)
            {
                if (MessageBox.Show("确认要删除当前行记录？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    var materialCode = this.radGridView1.CurrentRow.Cells[1].Value.ToString();
                    int row = await serviceClient.DeleteMaterialAsync(materialCode);
                    tool_status.Text = "【物料】删除1行记录 【删除】完成";
                }
            }
        }

        async private void Menu_clear_db_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要清空服务所有数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                return;
            if (cb_cfgType.SelectedIndex == 0)
            {
                //站位
                int row = await serviceClient.DeleteAllStationAsync();
                tool_status.Text = $"【站位】删除服务数据【{row}】条  【清空数据】完成";
            }
            else if (cb_cfgType.SelectedIndex == 1)
            {
                //型号
                int row = await serviceClient.DeleteAllProductTypeNoAsync();
                tool_status.Text = $"【型号】删除服务数据【{row}】条  【清空数据】完成";
            }
            else if (cb_cfgType.SelectedIndex == 2)
            {
                //物料
                int row = await serviceClient.DeleteAllMaterialAsync();
                tool_status.Text = $"【物料】删除服务数据【{row}】条  【清空数据】完成";
            }
            RefreshData();
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
            if (cb_cfgType.SelectedIndex == 0)
            {
                if (keyStation != key.ToString())
                {
                    stationListTemp.Add(keyStation);
                }
            }
            else if (cb_cfgType.SelectedIndex == 1)
            {
                if (keyTypeNo != key.ToString())
                {
                    modifyTypeNoTemp.Add(keyTypeNo);
                }
            }
            else if (cb_cfgType.SelectedIndex == 2)
            {
                if (keyMaterialCode != key.ToString())
                {
                    materialCodeTemp.Add(keyMaterialCode);
                }
            }
        }

        private void RadGridView1_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            var key = this.radGridView1.CurrentRow.Cells[1].Value;
            if (key != null)
            {
                if (cb_cfgType.SelectedIndex == 0)
                {
                    this.keyStation = key.ToString();
                }
                else if (cb_cfgType.SelectedIndex == 1)
                {
                    this.keyTypeNo = key.ToString();
                }
                else if (cb_cfgType.SelectedIndex == 2)
                {
                    this.keyMaterialCode = key.ToString();
                }
            }
        }

        private void Cb_cfgType_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void Menu_refresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void Menu_commit_Click(object sender, EventArgs e)
        {
            this.cb_cfgType.Focus();
            CommitData();
        }

        private void RefreshData()
        {
            if (cb_cfgType.SelectedIndex == 0)
            {
                //站位
                //不能删除，只能修改
                menu_del.Enabled = false;
                SelectStationData();
            }
            else if (cb_cfgType.SelectedIndex == 1)
            {
                //型号
                menu_del.Enabled = true;
                SelectProductTypeData();
            }
            else if (cb_cfgType.SelectedIndex == 2)
            {
                //物料
                menu_del.Enabled = true;
                SelectMaterial();
            }
        }

        private void CommitData()
        {
            if (cb_cfgType.SelectedIndex == 0)
            {
                CommitStationMesService();
            }
            else if (cb_cfgType.SelectedIndex == 1)
            {
                CommitTypeNoMesService();
            }
            else if (cb_cfgType.SelectedIndex == 2)
            {
                CommitMaterialMesService();
            }
        }

        #region 调用接口

        async private void SelectProductTypeData()
        {
            //调用查询接口
            radGridView1.DataSource = null;
            DataSet dataSet = await serviceClient.SelectProductTypeNoAsync("");
            DataTable dataTable = dataSet.Tables[0];
            typeNoData.Clear();
            if (dataTable.Rows.Count > 0)
            {
                //显示数据
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dr = typeNoData.NewRow();
                    dr[DATA_ORDER] = i + 1;
                    dr[DATA_TYPENO_NAME] = dataTable.Rows[i][0].ToString();
                    typeNoData.Rows.Add(dr);
                }
                radGridView1.DataSource = typeNoData;
                this.radGridView1.Columns[0].ReadOnly = true;
            }
            else
            {
                typeNoData.Clear();
                radGridView1.DataSource = typeNoData;
            }
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1, true);
            this.radGridView1.Columns[0].ReadOnly = true;
        }

        async private void SelectStationData()
        {
            //调用查询接口
            radGridView1.DataSource = null;
            DataSet dataSet = await serviceClient.SelectStationAsync("", "");
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
        }

        async private void SelectMaterial()
        {
            //调用查询接口
            radGridView1.DataSource = null;
            DataSet dataSet = await serviceClient.SelectMaterialAsync();
            DataTable dataTable = dataSet.Tables[0];
            materialData.Clear();
            if (dataTable.Rows.Count > 0)
            {
                //显示数据
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    DataRow dr = materialData.NewRow();
                    dr[DATA_ORDER] = i + 1;
                    dr[DATA_MATERIAL] = dataTable.Rows[i][0].ToString();
                    dr[DATA_AMOUNT] = dataTable.Rows[i][1].ToString();
                    materialData.Rows.Add(dr);
                }
                radGridView1.DataSource = materialData;
            }
            else
            {
                materialData.Clear();
                radGridView1.DataSource = materialData;
            }
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1, true);
            this.radGridView1.Columns[0].ReadOnly = true;
        }

        async private void CommitTypeNoMesService()
        {
            try
            {
                int row = radGridView1.RowCount;
                string[] array = new string[row];
                //新增行数据
                for (int i = 0; i < row; i++)
                {
                    var ID = radGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                    var productName = radGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                    if (!string.IsNullOrEmpty(productName))
                    {
                        array[i] = productName;
                    }
                }
                //修改行数据
                if (modifyTypeNoTemp.Count > 0)
                {
                    foreach (var val in this.modifyTypeNoTemp)
                    {
                        await serviceClient.DeleteProductTypeNoAsync(val);
                    }
                }
                string res = await serviceClient.CommitProductTypeNoAsync(array);
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

        async private void CommitMaterialMesService()
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
                    if (!string.IsNullOrEmpty(materialCode))
                    {
                        material.MaterialCode = materialCode;
                        material.MaterialAmount = int.Parse(amount);
                        materialMsg[i] = material;
                    }
                }
                //判断主键是否有修改，将原记录删除后，再执行其他更新
                if (materialCodeTemp.Count > 0)
                {
                    foreach (var code in materialCodeTemp)
                    {
                        await serviceClient.DeleteMaterialAsync(code);
                    }
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

        async private void CommitStationMesService()
        {
            //将新增数据提交到服务
            try
            {
                int row = radGridView1.RowCount;
                MesService.Station[] stationsArray = new MesService.Station[row];
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
                if (stationListTemp.Count > 0)
                {
                    foreach (var station in stationListTemp)
                    {
                        await serviceClient.DeleteStationAsync(station);
                    }
                }
                int res = await serviceClient.InsertStationAsync(stationsArray);
                if (res == 1)
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
