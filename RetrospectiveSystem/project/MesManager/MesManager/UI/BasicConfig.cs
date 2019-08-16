﻿using System;
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
using MesManager.RadView;

namespace MesManager.UI
{
    /// <summary>
    /// 管理产品型号、基础物料、产线站位信息
    /// </summary>
    public partial class BasicConfig : RadForm
    {
        private MesService.MesServiceClient serviceClient;
        private MesServiceTest.MesServiceClient serviceClientTest;
        private DataTable typeNoData,materialData;
        private List<string> modifyTypeNoTemp;
        private List<string> materialCodeTemp;//存储用户修改的物料编码
        private string keyTypeNo;
        private string keyMaterialCode;//记录修改前的编码
        private string keyStation;
        private string curMaterialCode;//记录鼠标右键选中行编码
        private string curRowTypeNo;
        private const string DATA_ORDER = "序号";
        private const string DATA_MATERIAL_CODE = "物料编码";
        private const string DATA_MATERIAL_NAME = "物料名称";
        private const string DATA_TYPENO_NAME = "型号名称";
        private const string DATA_CONTAINER_CAPACITY = "容器容量";
        private const string DATA_USER_NAME = "用户名";
        private const string DATA_UPDATE_DATE = "更新日期";

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
            serviceClientTest = new MesServiceTest.MesServiceClient();
            modifyTypeNoTemp = new List<string>();
            materialCodeTemp = new List<string>();
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1,true);
            this.radGridView1.AllowRowHeaderContextMenu = false;
            DataSource();
            cb_cfgType.Items.Clear();
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
                materialData.Columns.Add(DATA_MATERIAL_CODE);
                materialData.Columns.Add(DATA_MATERIAL_NAME);
                materialData.Columns.Add(DATA_USER_NAME);
                materialData.Columns.Add(DATA_UPDATE_DATE);
            }
            if (typeNoData == null)
            {
                typeNoData = new DataTable();
                typeNoData.Columns.Add(DATA_ORDER);
                typeNoData.Columns.Add(DATA_TYPENO_NAME);
                typeNoData.Columns.Add(DATA_CONTAINER_CAPACITY);
                typeNoData.Columns.Add(DATA_USER_NAME);
                typeNoData.Columns.Add(DATA_UPDATE_DATE);
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
            this.radGridView1.Rows.AddNew();
        }

        async private void Menu_del_Click(object sender, EventArgs e)
        {
            //删除当前行
            var stationName = this.radGridView1.CurrentRow.Cells[1].Value.ToString();
            if (string.IsNullOrEmpty(stationName))
            {
                this.radGridView1.CurrentRow.Delete();
            }
            else
            {
                if (MessageBox.Show("确认要删除当前行记录？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (cb_cfgType.SelectedIndex == 0)
                    {
                        var typeNo = this.radGridView1.CurrentRow.Cells[1].Value.ToString();
                        int row = await serviceClient.DeleteProductContinairCapacityAsync(typeNo);
                        tool_status.Text = "【型号】删除1行记录 【删除】完成";
                        if (row > 0)
                        {
                            RefreshData();
                        }
                    }
                    else if (cb_cfgType.SelectedIndex == 1)
                    {
                        var materialCode = this.radGridView1.CurrentRow.Cells[1].Value.ToString();
                        int row = await serviceClient.DeleteMaterialAsync(materialCode);
                        tool_status.Text = "【物料】删除1行记录 【删除】完成";
                        if (row > 0)
                        {
                            RefreshData();
                        }
                    }
                }
            }
        }

        async private void Menu_clear_db_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定要清空服务所有数据？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                return;
            if (cb_cfgType.SelectedIndex == 0)
            {
                //型号
                int row = 0;// await serviceClient.DeleteAllProductTypeNoAsync();
                tool_status.Text = $"【型号】删除服务数据【{row}】条  【清空数据】完成";
            }
            else if (cb_cfgType.SelectedIndex == 1)
            {
                //物料
                int row = await serviceClient.DeleteAllMaterialAsync();
                tool_status.Text = $"【物料】删除服务数据【{row}】条  【清空数据】完成";
            }
            RefreshData();
        }

        private void Menu_grid_Click(object sender, EventArgs e)
        {
            for (int i = this.radGridView1.Rows.Count - 1; i >= 0; i--)
            {
                this.radGridView1.Rows[i].Delete();
            }
        }

        private void RadGridView1_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            var key = this.radGridView1.CurrentRow.Cells[1].Value;
            if (key == null)
                return;
            if (cb_cfgType.SelectedIndex == 0)
            {
                if (keyTypeNo != key.ToString())
                {
                    modifyTypeNoTemp.Add(keyTypeNo);
                }
            }
            else if (cb_cfgType.SelectedIndex == 1)
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
                //型号
                SelectProductTypeData();
            }
            else if (cb_cfgType.SelectedIndex == 1)
            {
                //物料
                SelectMaterial();
            }
        }

        private void CommitData()
        {
            if (cb_cfgType.SelectedIndex == 0)
            {
                CommitTypeNoMesService();
            }
            else if (cb_cfgType.SelectedIndex == 1)
            {
                CommitMaterialMesService();
            }
        }

        #region 调用接口

        async private void SelectProductTypeData()
        {
            //调用查询接口
            radGridView1.DataSource = null;
            DataSet dataSet = await serviceClient.SelectProductContinairCapacityAsync("");
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
                    dr[DATA_CONTAINER_CAPACITY] = dataTable.Rows[i][1].ToString();
                    dr[DATA_USER_NAME] = dataTable.Rows[i][2].ToString();
                    dr[DATA_UPDATE_DATE] = dataTable.Rows[i][3].ToString();
                    typeNoData.Rows.Add(dr);
                }
                radGridView1.DataSource = typeNoData;
                SetGridReadOnly();
            }
            else
            {
                typeNoData.Clear();
                radGridView1.DataSource = typeNoData;
            }
            DataGridViewCommon.SetRadGridViewProperty(this.radGridView1, true);
            SetGridReadOnly();
        }

        private void SetGridReadOnly()
        {
            this.radGridView1.Columns[0].ReadOnly = true;
            this.radGridView1.Columns[3].ReadOnly = true;
            this.radGridView1.Columns[4].ReadOnly = true;
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
                    dr[DATA_MATERIAL_CODE] = dataTable.Rows[i][0].ToString();
                    dr[DATA_MATERIAL_NAME] = dataTable.Rows[i][1].ToString();
                    dr[DATA_USER_NAME]     = dataTable.Rows[i][2].ToString();
                    dr[DATA_UPDATE_DATE]   = dataTable.Rows[i][3].ToString();
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
            materialCodeTemp.Clear();
            modifyTypeNoTemp.Clear();
        }

        async private void CommitTypeNoMesService()
        {
            try
            {
                //修改行数据
                if (modifyTypeNoTemp.Count > 0)
                {
                    foreach (var val in this.modifyTypeNoTemp)
                    {
                        await serviceClient.DeleteProductContinairCapacityAsync(val);
                    }
                }

                int row = radGridView1.RowCount;
                string[] array = new string[row];
                //新增行数据
                for (int i = 0; i < row; i++)
                {
                    var ID = radGridView1.Rows[i].Cells[0].Value.ToString().Trim();
                    var productName = radGridView1.Rows[i].Cells[1].Value.ToString().Trim();
                    var storage = radGridView1.Rows[i].Cells[2].Value.ToString().Trim();
                    if (!string.IsNullOrEmpty(productName))
                    {
                        array[i] = productName;
                        var res = await serviceClient.CommitProductContinairCapacityAsync(productName,storage,MESMainForm.currentUser);
                        if (res < 1)
                        {
                            MessageBox.Show($"【{productName}】更新失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
                MessageBox.Show("更新成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshData();
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show($"{ex.Message}", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    var materialName = radGridView1.Rows[i].Cells[2].Value.ToString().Trim();
                    if (!string.IsNullOrEmpty(materialCode))
                    {
                        material.MaterialCode = materialCode;
                        material.MaterialName = materialName;
                        material.UserName = MESMainForm.currentUser;

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
                MesService.MaterialMsg[] materialList = await serviceClient.CommitMaterialAsync(materialMsg);
                foreach(var material in materialList)
                {
                    if(material.Result != 1)
                    {
                        MessageBox.Show($"【{material.MaterialCode}】更新失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                MessageBox.Show("更新成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RefreshData();
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message + "\r\n" + ex.StackTrace);
                MessageBox.Show($"{ex.Message}","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
