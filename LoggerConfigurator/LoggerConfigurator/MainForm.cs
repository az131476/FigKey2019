﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using Telerik;
using Telerik.WinControls;
using Telerik.Data;
using Telerik.Collections;
using Telerik.WinControls.Themes;
using Telerik.WinControls.Enumerations;
using Telerik.WinControls.UI.Docking;
using FigKeyLoggerConfigurator.Control;
using FigKeyLoggerConfigurator.Model;
using CommonUtils.FileHelper;
using AnalysisAgreeMent;
using CommonUtils.Logger;
using System.Diagnostics;
using AnalysisAgreeMent.Model.XCP;
using AnalysisAgreeMent.Model.DBC;
using AnalysisAgreeMent.Model;
using AnalysisAgreeMent.Analysis;
using System.IO;
using LoggerConfigurator.Model;
using LoggerConfigurator.View;

namespace LoggerConfigurator
{
    public partial class MainForm : RadForm
    {
        #region 私有成员变量
        private FileContent openFileContent;
        private XcpData xcpdataCan1;
        private XcpHelper xcpHelperCan1;
        private DBCData dbcDataCan1;
        private DbcHelper dbcHelperCan1;
        private XcpData xcpdataCan2;
        private XcpHelper xcpHelperCan2;
        private DBCData dbcDataCan2;
        private DbcHelper dbcHelperCan2;
        private GridViewControl gridViewControl;
        private GridViewData gridViewData;
        private LimitTimeCfg limitCfg;
        private AnalysisData analysisData;
        private FileType analysisFileType;
        private DataTable dataSourceCan1;
        private DataTable dataSourceCan2;
        private SelectedCan selectedCan;
        private Dictionary<string, string> can1DicCheckState_12;
        private Dictionary<string, string> can1DicCheckState_13;
        private Dictionary<string, string> can2DicCheckState_12;
        #endregion
        public MainForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public void Init()
        {
            Initial();
            LoadTreeView();
            LoadRadGridView();
        }

        private void RadForm1_Load(object sender, EventArgs e)
        {
            Login login = new Login();
            login.ShowDialog();

            btn_openFile_Can1.Click += Btn_openFile_Can1_Click;
            btn_openFile_can2.Click += Btn_openFile_can2_Click;
            btn_search_can1.Click += Rbtn_search_Click;
            btn_search_can2.Click += Btn_search_can2_Click;
            tb_filter_can1.KeyDown += XcpFilterCondition_KeyDown;
            tb_filter_can2.KeyDown += Tb_filter_can2_KeyDown;

            radGridView_can1.ValueChanged += RadGridView1_ValueChanged;
            radGridView_can2.ValueChanged += RadGridView_can2_ValueChanged;

            tool_exportfile.Click += Tool_exportfile_Click;
            menu_logger_manager.Click += Menu_logger_manager_Click;

            cb_protocol_can1.SelectedIndexChanged += Cb_protocol_can1_SelectedIndexChanged;
            cb_protocol_can2.SelectedIndexChanged += Cb_protocol_can2_SelectedIndexChanged;
        }

        private void Cb_protocol_can2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_protocol_can2.SelectedIndex == 2)
            {
                //select monitor can,do not set it
                cb_baud_can2.Enabled = false;
            }
            else
            {
                cb_baud_can2.Enabled = true;
            }
        }

        private void Cb_protocol_can1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_protocol_can1.SelectedIndex == 2)
            {
                //select monitor can,do not set it
                cb_baud_can1.Enabled = false;
            }
            else
            {
                cb_baud_can1.Enabled = true;
            }
        }

        private void Menu_logger_manager_Click(object sender, EventArgs e)
        {
            this.toolWindow_left.Show();
        }

        enum SelectedCan
        {
            CAN_1,
            CAN_2
        }

        private void Initial()
        {
            gridViewData = new GridViewData();
            gridViewData.LimitTimeListSegMent = new List<int>();
            gridViewData.LimitTimeList10ms = new List<int>();
            gridViewData.LimitTimeList100ms = new List<int>();
            gridViewData.DbcCheckIndex = new List<int>();

            can1DicCheckState_12 = new Dictionary<string, string>();//12cell row-state
            can1DicCheckState_13 = new Dictionary<string, string>();//13cell row-state
            can2DicCheckState_12 = new Dictionary<string, string>();//can2 13cell row-state
            //配置
            //can1
            cb_baud_can1.Items.Clear();
            cb_baud_can1.Items.Add("100000");
            cb_baud_can1.Items.Add("500000");
            cb_baud_can1.Items.Add("1000000");
            cb_baud_can1.SelectedIndex = 1;

            cb_protocol_can1.Items.Clear();
            cb_protocol_can1.Items.Add(ProtocolTypeEnum.CCP);
            cb_protocol_can1.Items.Add(ProtocolTypeEnum.XCP);
            cb_protocol_can1.Items.Add("");
            //cb_protocol_can1.Items.Add(ProtocolTypeEnum.CanMonnitor);
            cb_protocol_can1.SelectedIndex = 0;

            //can2
            cb_baud_can2.Items.Clear();
            cb_baud_can2.Items.Add("100000");
            cb_baud_can2.Items.Add("500000");
            cb_baud_can2.Items.Add("1000000");
            cb_baud_can2.SelectedIndex = 1;

            cb_protocol_can2.Items.Clear();
            //cb_protocol_can2.Items.Add(ProtocolTypeEnum.CCP);
            //cb_protocol_can2.Items.Add(ProtocolTypeEnum.XCP);
            cb_protocol_can2.Items.Add(ProtocolTypeEnum.CanMonnitor);
            cb_protocol_can2.Items.Add("");
            cb_protocol_can2.SelectedIndex = 0;
            //document set
            this.radDock1.RemoveAllDocumentWindows();

            lbx_protocol_remark.Text += "DBC时配置波特率有效，XCP直接从文件读取，配置无效";
        }

        #region 复选框行值处理
        bool IsCheck10msCan1;
        bool IsCheck100msCan1;

        /// <summary>
        /// 复选框状态添加行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadGridView1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                //设置不可选
                switch (this.radGridView_can1.CurrentCell.ColumnIndex)
                {
                    case 12:
                        if (!IsCheck10msCan1)
                        {
                            this.radGridView_can1.CurrentRow.Cells[12].Value = 1;
                            IsCheck10msCan1 = !IsCheck10msCan1;
                        }
                        else
                        {
                            this.radGridView_can1.CurrentRow.Cells[12].Value = 0;
                            IsCheck10msCan1 = !IsCheck10msCan1;
                        }
                        if (this.radGridView_can1.CurrentRow.Cells[13].Value.ToString() is "1")
                            this.radGridView_can1.CurrentRow.Cells[13].Value = 0;
                        break;
                    case 13:
                        if (!IsCheck100msCan1)
                        {
                            this.radGridView_can1.CurrentRow.Cells[13].Value = 1;
                            IsCheck100msCan1 = !IsCheck100msCan1;
                        }
                        else
                        {
                            this.radGridView_can1.CurrentRow.Cells[13].Value = 0;
                            IsCheck100msCan1 = !IsCheck100msCan1;
                        }
                        if (this.radGridView_can1.CurrentRow.Cells[12].Value.ToString() is "1")
                            this.radGridView_can1.CurrentRow.Cells[12].Value = 0;
                        break;
                }
                #region
                //if (this.radGridView1.ActiveEditor is RadCheckBoxEditor)
                //{
                //    int rowIndex = this.radGridView1.CurrentCell.RowIndex;
                //    int columnIndex = this.radGridView1.CurrentCell.ColumnIndex;

                //    if (this.radGridView1.ActiveEditor.Value.ToString() is "On")
                //    {
                //        limitCfg = new LimitTimeCfg();
                //        limitCfg.RowIndex = rowIndex;
                //        if (columnIndex == 12)
                //        {
                //            if (!ExistCurCheckBoxRow(rowIndex, gridViewData.LimitTimeListSegMent))
                //            {
                //                limitCfg.LimitType = TimeLimitType._segMent;
                //                gridViewData.LimitTimeListSegMent.Add(limitCfg);
                //            }
                //        }
                //        else if (columnIndex == 13)
                //        {
                //            if (!ExistCurCheckBoxRow(rowIndex, gridViewData.LimitTimeList10ms))
                //            {
                //                limitCfg.LimitType = TimeLimitType._10ms;
                //                gridViewData.LimitTimeList10ms.Add(limitCfg);
                //            }
                //        }
                //        else if (columnIndex == 14)
                //        {
                //            if (!ExistCurCheckBoxRow(rowIndex, gridViewData.LimitTimeList100ms))
                //            {
                //                limitCfg.LimitType = TimeLimitType._100ms;
                //                gridViewData.LimitTimeList100ms.Add(limitCfg);
                //            }
                //        }
                //    }
                //    else if (this.radGridView1.ActiveEditor.Value.ToString() is "Off")
                //    {
                //        if (columnIndex == 12)
                //        {
                //            if (ExistCurCheckBoxRow(rowIndex, gridViewData.LimitTimeListSegMent))
                //            {
                //                gridViewData.LimitTimeListSegMent.Remove(limitCfg);
                //            }
                //        }
                //        else if (columnIndex == 13)
                //        {
                //            if (ExistCurCheckBoxRow(rowIndex, gridViewData.LimitTimeList10ms))
                //            {
                //                gridViewData.LimitTimeList10ms.Remove(limitCfg);
                //            }
                //        }
                //        else if (columnIndex == 14)
                //        {
                //            if (ExistCurCheckBoxRow(rowIndex, gridViewData.LimitTimeList100ms))
                //            {
                //                gridViewData.LimitTimeList100ms.Remove(limitCfg);
                //            }
                //        }
                //    }
                //    ////三列CheckBox 只能选择一列
                //}
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message + "\r\n" + ex.StackTrace);
            }
        }
        bool IsCheckCan2;
        private void RadGridView_can2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.radGridView_can2.CurrentCell.ColumnIndex)
                {
                    case 12:
                        if (!IsCheckCan2)
                        {
                            this.radGridView_can2.CurrentRow.Cells[12].Value = 1;
                            IsCheckCan2 = !IsCheckCan2;
                            
                        }
                        else
                        {
                            this.radGridView_can2.CurrentRow.Cells[12].Value = 0;
                            IsCheckCan2 = !IsCheckCan2;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private bool ExistCurCheckBoxRow(int row, List<LimitTimeCfg> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].RowIndex == row)
                    return true;
            }
            return false;
        }
        #endregion

        #region 导出行数据
        /// <summary>
        /// 导出选中行数据
        /// </summary>
        private void ExportData()
        {
            SelectedRowCal();
            string can1 = cb_protocol_can1.Text;
            string can2 = cb_protocol_can2.Text;
            int sectCan = 0;
            if (!string.IsNullOrEmpty(cb_protocol_can1.Text))
            {
                if (gridViewData.LimitTimeList10ms.Count < 1 && gridViewData.LimitTimeList100ms.Count < 1)
                {
                    MessageBox.Show("未选择行数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            if (!string.IsNullOrEmpty(cb_protocol_can2.Text))
            {
                if (gridViewData.DbcCheckIndex.Count < 1)
                {
                    MessageBox.Show("未选择行数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            string path = FileSelect.SaveAs("(*.c)|*.c");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            if (!string.IsNullOrEmpty(can1) && string.IsNullOrEmpty(can2))
                sectCan = 1;
            else if (!string.IsNullOrEmpty(can2) && string.IsNullOrEmpty(can1))
                sectCan = 2;
            else if (!string.IsNullOrEmpty(can1) && !string.IsNullOrEmpty(can2))
            {
                sectCan = 3;
            }
            ExportFile.ExportFileToLocal(path, radGridView_can1, radGridView_can2, gridViewData, analysisData,xcpdataCan1,sectCan);
        }
        #endregion

        /// <summary>
        /// 遍历行，查询CHECKBOX选中行
        /// </summary>
        private void SelectedRowCal()
        {
            int rdex = 0;
            int can2Index = 0;
            gridViewData.LimitTimeListSegMent.Clear();
            gridViewData.LimitTimeList10ms.Clear();
            gridViewData.LimitTimeList100ms.Clear();
            gridViewData.DbcCheckIndex.Clear();

            foreach (GridViewDataRowInfo row in this.radGridView_can1.Rows)
            {
                //将选中得行添加到集合
                if (this.radGridView_can1.Rows[rdex].Cells[12].Value.ToString() is "1")
                {
                    gridViewData.LimitTimeList10ms.Add(rdex);
                }
                if (this.radGridView_can1.Rows[rdex].Cells[13].Value.ToString() is "1")
                {
                    gridViewData.LimitTimeList100ms.Add(rdex);
                }
                rdex++;
            }
            foreach (GridViewRowInfo row in this.radGridView_can2.Rows)
            {
                if (this.radGridView_can2.Rows[can2Index].Cells[12].Value.ToString() is "1")
                {
                    gridViewData.DbcCheckIndex.Add(can2Index);
                }
                can2Index++;
            }
        }

        #region 导出数据/快捷菜单
        /// <summary>
        /// 导出所有数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tool_exportfile_Click(object sender, EventArgs e)
        {
            ExportData();
        }
        #endregion

        #region 搜索 gridView data
        /// <summary>
        /// 回车事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XcpFilterCondition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Rbtn_search_Click(sender, e);
            }
        }

        private void Tb_filter_can2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Btn_search_can2_Click(sender,e);
            }
        }

        /// <summary>
        /// 关键字搜索data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rbtn_search_Click(object sender, EventArgs e)
        {
            int dex = 0;
            //将选中add
            foreach (GridViewRowInfo row in this.radGridView_can1.Rows)
            {
                var key = this.radGridView_can1.Rows[dex].Cells[1].Value.ToString();
                var value12 = this.radGridView_can1.Rows[dex].Cells[12].Value.ToString();
                var value13 = this.radGridView_can1.Rows[dex].Cells[13].Value.ToString();
                if (!can1DicCheckState_12.ContainsKey(key))
                {
                    if (value12 == "1")
                    {
                        can1DicCheckState_12.Add(key, value12);
                    }
                }
                else
                {
                    can1DicCheckState_12.Remove(key);
                    can1DicCheckState_12.Add(key,value12);
                }
                if (!can1DicCheckState_13.ContainsKey(key))
                {
                    if (value13 == "1")
                    {
                        can1DicCheckState_13.Add(key, value13);
                    }
                }
                else
                {
                    can1DicCheckState_13.Remove(key);
                    can1DicCheckState_13.Add(key,value13);
                }
                dex++;
            }

            //DataTable dt = dataSourceCan1;//radGridView_can1.DataSource as DataTable;
            DataTable updateDt = dataSourceCan1.Clone();
            if (dataSourceCan1.Rows.Count < 1)
                return;
            string filter = tb_filter_can1.Text.Trim();
            //can2
            string condition = "";
            if (string.IsNullOrEmpty(filter))
            {
                condition = "";
            }
            else
            {
                condition = $"名称 like '%{filter}%' or 名称 like '%{filter.ToLower()}%' or 名称 like '%{filter.ToUpper()}%' or 数据地址 like '%{filter}%'";
            }
            DataRow[] dataRows = dataSourceCan1.Select(condition);
            int dx = 0;
            foreach (var row in dataRows)
            {
                updateDt.Rows.Add(row.ItemArray);
            }
            //update end
            radGridView_can1.BeginEdit();
            radGridView_can1.DataSource = updateDt;
            gridViewControl.RefreshRadViewColumnCan1();
            radGridView_can1.EndUpdate();
            //radGridView_can1.Update();

            tb_filter_can1.Clear();
            tb_filter_can1.Focus();
            //设置选中状态
            try
            {
                int rIndex12 = 0;
                foreach (GridViewDataRowInfo row in this.radGridView_can1.Rows)
                {
                    foreach (KeyValuePair<string, string> kv in can1DicCheckState_12)
                    {
                        var k = kv.Key;
                        var v = kv.Value;
                        if (row.Cells[1].Value.ToString().Equals(k))
                        {
                            row.Cells[12].Value = v;
                        }
                    }
                    rIndex12++;
                }

                int rIndex13 = 0;
                foreach (GridViewDataRowInfo row in this.radGridView_can1.Rows)
                {
                    foreach (KeyValuePair<string, string> kv in can1DicCheckState_13)
                    {
                        var k = kv.Key;
                        var v = kv.Value;
                        if (row.Cells[1].Value.ToString().Equals(k))
                        {
                            row.Cells[13].Value = v;
                        }
                    }
                    rIndex13++;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message + "\r\n" + ex.StackTrace);
            }
        }
        private void Btn_search_can2_Click(object sender, EventArgs e)
        {
            int dex =0;
            foreach (GridViewRowInfo row in this.radGridView_can2.Rows)
            {
                var key = this.radGridView_can2.Rows[dex].Cells[1].Value.ToString();
                var value = this.radGridView_can2.Rows[dex].Cells[12].Value.ToString();
                if (!can2DicCheckState_12.ContainsKey(key))
                    can2DicCheckState_12.Add(key, value);
                else
                {
                    can2DicCheckState_12.Remove(key);
                    can2DicCheckState_12.Add(key,value);
                }
                dex++;
            }

            //DataTable dt = radGridView_can2.DataSource as DataTable;
            DataTable updateDt = dataSourceCan2.Clone();
            string filter = tb_filter_can2.Text.Trim();
            if (dataSourceCan2.Rows.Count < 1)
                return;
            //can2
            string condition = "";
            if (string.IsNullOrEmpty(filter))
            {
                condition = "";
            }
            else
            {
                condition = $"名称 like '%{filter}%' or 名称 like '%{filter.ToLower()}%' or 名称 like '%{filter.ToUpper()}%' or 数据地址 like '%{filter}%'";
            }
            DataRow[] dataRows = dataSourceCan2.Select(condition);
            foreach (var row in dataRows)
            {
                updateDt.Rows.Add(row.ItemArray);
            }
            //update end
            radGridView_can2.BeginEdit();
            radGridView_can2.DataSource = updateDt;
            //gridViewControl.RefreshRadViewColumnCan2();
            radGridView_can2.EndEdit();
            try
            {
                int rIndex = 0;
                foreach (KeyValuePair<string, string> kv in can2DicCheckState_12)
                {
                    var k = kv.Key;
                    var v = kv.Value;
                    foreach (GridViewDataRowInfo row in this.radGridView_can2.Rows)
                    {
                        if (row.Cells[1].Value.ToString().Equals(k))
                        {
                            row.Cells[12].Value = v;
                        }
                        rIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message + "\r\n" + ex.StackTrace);
            }

            tb_filter_can2.Clear();
            tb_filter_can2.Focus();
        }
        #endregion

        #region 打开文件
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_openFile_Can1_Click(object sender, EventArgs e)
        {
            selectedCan = SelectedCan.CAN_1;
            OpenAnalysisFile(SelectedCan.CAN_1);
        }

        private void Btn_openFile_can2_Click(object sender, EventArgs e)
        {
            selectedCan = SelectedCan.CAN_2;
            OpenAnalysisFile(SelectedCan.CAN_2);
        }

        private void OpenAnalysisFile(SelectedCan selectedCan)
        {
            string openFilter = "(*.*)|*.*";
            ProtocolTypeEnum selectProtocol = ProtocolTypeEnum.CCP;
            if (selectedCan == SelectedCan.CAN_1)
            {
                //CAN1 判断协议类型是否选择正确
                if (string.IsNullOrEmpty(cb_protocol_can1.Text))
                {
                    MessageBox.Show("未选择协议类型","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return;
                }
                if (!Enum.TryParse(cb_protocol_can1.Text, out selectProtocol))
                {
                    LogHelper.Log.Info("CAN1选择协议后类型转换失败！协议类型错误=>"+cb_protocol_can1.Text);
                    MessageBox.Show("协议类型不存在！当前支持CCP/XCP/CanMonnitor", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (selectProtocol == ProtocolTypeEnum.CanMonnitor)
                {
                    openFilter = "(*.dbc)|*.dbc";
                    dbcDataCan1 = new DBCData();
                    dbcHelperCan1 = new DbcHelper(dbcDataCan1);
                }
                else if ((selectProtocol == ProtocolTypeEnum.CCP) || (selectProtocol == ProtocolTypeEnum.XCP))
                {
                    openFilter = "(*.a2l)|*.a2l";
                    xcpdataCan1 = new XcpData();
                    xcpHelperCan1 = new XcpHelper(xcpdataCan1);
                }
            }
            else if (selectedCan == SelectedCan.CAN_2)
            {
                //CAN2 判断协议类型选择是否正确
                if (string.IsNullOrEmpty(cb_protocol_can2.Text))
                {
                    MessageBox.Show("未选择协议类型", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!Enum.TryParse(cb_protocol_can2.Text, out selectProtocol))
                {
                    LogHelper.Log.Info("CAN2选择协议后类型转换失败！协议类型错误=>" + cb_protocol_can2.Text);
                    MessageBox.Show("协议类型不存在！当前支持CCP/XCP/CanMonnitor", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (selectProtocol == ProtocolTypeEnum.CanMonnitor)
                {
                    openFilter = "(*.dbc)|*.dbc";
                    dbcDataCan2 = new DBCData();
                    dbcHelperCan2 = new DbcHelper(dbcDataCan2);
                }
                else if ((selectProtocol == ProtocolTypeEnum.CCP) || (selectProtocol == ProtocolTypeEnum.XCP))
                {
                    openFilter = "(*.a2l)|*.a2l";
                    xcpdataCan2 = new XcpData();
                    xcpHelperCan2 = new XcpHelper(xcpdataCan2);
                }
            }

            openFileContent = FileSelect.GetSelectFileContent(openFilter, "选择文件");
            if (openFileContent == null)
                return;
            if (string.IsNullOrEmpty(openFileContent.FileName))
                return;
            if (!string.IsNullOrEmpty(openFileContent.FileName))
            {
                //开始解析
                StartAnalysis(selectProtocol, selectedCan);
            }
        }
        private void StartAnalysis(ProtocolTypeEnum protocolType,SelectedCan selectedCan)
        {
            if (protocolType == ProtocolTypeEnum.XCP)
            {
                LogHelper.Log.Info("开始解析XCP...");
                A2lAnalysis(ProtocolTypeEnum.XCP);
            }
            else if (protocolType == ProtocolTypeEnum.CCP)
            {
                LogHelper.Log.Info("开始解析CCP...");
                A2lAnalysis(ProtocolTypeEnum.CCP);
            }
            else if (protocolType == ProtocolTypeEnum.CanMonnitor)
            {
                LogHelper.Log.Info("开始解析DBC...");
                DbcAnalysis();
            }
        }
        private void DbcAnalysis()
        {
            //case = 1 解析DBC文件
            analysisFileType = FileType.DBC;
            DbcResultEnum dbcResult = DbcResultEnum.FAILT;
            if (selectedCan == SelectedCan.CAN_1)
            {
                dbcResult = dbcHelperCan1.AnalysisDbc(openFileContent.FileName);
            }
            else if (selectedCan == SelectedCan.CAN_2)
            {
                dbcResult = dbcHelperCan2.AnalysisDbc(openFileContent.FileName);
            }
             
            if (dbcResult == DbcResultEnum.SUCCESS)
            {
                LogHelper.Log.Info("【解析DBC文件成功！】");
                if (selectedCan == SelectedCan.CAN_1)
                {
                    analysisData = AnalysisDataSet.UnionXcpDbc(FileType.DBC, null, dbcDataCan1,cb_baud_can2.Text);
                }
                else if (selectedCan == SelectedCan.CAN_2)
                {
                    analysisData = AnalysisDataSet.UnionXcpDbc(FileType.DBC, null, dbcDataCan2,cb_baud_can2.Text);
                }
                
                if (analysisData != null)
                {
                    LogHelper.Log.Info("DBC合并数据完成!" + analysisData.AnalysisDbcDataList.Count);
                }
                else
                {
                    LogHelper.Log.Info("DBC合并数据失败!");
                }
                if (selectedCan == SelectedCan.CAN_1)
                {
                    if (dataSourceCan1 != null)
                    {
                        dataSourceCan1.Clear();
                    }
                    dataSourceCan1 = gridViewControl.BindRadGridView(analysisData.AnalysisDbcDataList);
                    GridViewLoadDataSourceCan1();
                }
                else if (selectedCan == SelectedCan.CAN_2)
                {
                    if (dataSourceCan2 != null)
                    {
                        dataSourceCan2.Clear();
                    }
                    dataSourceCan2 = gridViewControl.BindRadGridView(analysisData.AnalysisDbcDataList);
                    GridViewLoadDataSourceCan2();
                }
                //gridViewControl.BindRadGridView(xcpdata);
                LogHelper.Log.Info("DBC加载完成！");
            }
            else if (dbcResult == DbcResultEnum.FAILT)
            {
                LogHelper.Log.Info("【解析DBC文件失败！】");
            }
        }
        private void A2lAnalysis(ProtocolTypeEnum protocol)
        {
            //case = 2 解析a2l文件
            analysisFileType = FileType.A2L;
            CodeCommand res = CodeCommand.DEAFUALT_FAIL;
            if (selectedCan == SelectedCan.CAN_1)
            {
                res = xcpHelperCan1.AnalyzeXcpFile(openFileContent.FileName);
            }
            else if (selectedCan == SelectedCan.CAN_2)
            {
                res = xcpHelperCan2.AnalyzeXcpFile(openFileContent.FileName);
            }
            
            if (res == CodeCommand.RESULT)
            {
                LogHelper.Log.Info("**********解析a2l文件成功************" + res);
                //判断选择协议与打开文件协议是否一致
                AgreementType agreement = AgreementType.other;
                if (selectedCan == SelectedCan.CAN_1)
                {
                    agreement = xcpdataCan1.AgreeMentType;
                }
                else if (selectedCan == SelectedCan.CAN_2)
                {
                    agreement = xcpdataCan2.AgreeMentType;
                }
                
                if ((int)protocol != (int)agreement)
                {
                    MessageBox.Show("选择协议与a2l文件不匹配","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    LogHelper.Log.Info($"选择协议与a2l文件不匹配，选择协议={protocol} 文件协议={agreement}");
                    return;
                }
                //判断协议类型是否为XCP,提示选择xcp on can
                if (selectedCan == SelectedCan.CAN_1)
                {
                    if (xcpdataCan1.AgreeMentType == AgreementType.XCP)
                    {
                        XcpProtocol xcpProtocol = new XcpProtocol(xcpdataCan1);
                        xcpProtocol.ShowDialog();
                    }
                }
                else if (selectedCan == SelectedCan.CAN_2)
                {
                    if (xcpdataCan2.AgreeMentType == AgreementType.XCP)
                    {
                        XcpProtocol xcpProtocol = new XcpProtocol(xcpdataCan2);
                        xcpProtocol.ShowDialog();
                    }
                }

                //将解析后的数据绑定到gridview显示
                if (selectedCan == SelectedCan.CAN_1)
                {
                    analysisData = AnalysisDataSet.UnionXcpDbc(FileType.A2L, xcpdataCan1, null,"");
                }
                else if (selectedCan == SelectedCan.CAN_2)
                {
                    analysisData = AnalysisDataSet.UnionXcpDbc(FileType.A2L, xcpdataCan2, null,"");
                }
                
                if (analysisData != null)
                {
                    LogHelper.Log.Info("a2l合并数据完成!" + analysisData.AnalysisiXcpDataList.Count);
                }
                else
                {
                    LogHelper.Log.Info("a2l合并数据失败!");
                }
                if (selectedCan == SelectedCan.CAN_1)
                {
                    if (dataSourceCan1 != null)
                    {
                        dataSourceCan1.Clear();
                    }
                    dataSourceCan1 = gridViewControl.BindRadGridView(analysisData.AnalysisiXcpDataList);
                    GridViewLoadDataSourceCan1();
                }
                else if (selectedCan == SelectedCan.CAN_2)
                {
                    if (dataSourceCan2 != null)
                    {
                        dataSourceCan2.Clear();
                    }
                    dataSourceCan2 = gridViewControl.BindRadGridView(analysisData.AnalysisiXcpDataList);
                    GridViewLoadDataSourceCan2();
                }
                //gridViewControl.BindRadGridView(xcpdata);
                LogHelper.Log.Info("a2l加载完成！");
                //MockIntegerDataSource
            }
            else
            {
                LogHelper.Log.Error("********解析a2l文件失败********** " + res);
            }
        }
        private void GridViewLoadDataSourceCan1()
        {
            //设置虚模式
            //radGridView1.VirtualMode = true;
            //radGridView1.ColumnCount = dataSource.Columns.Count;
            //radGridView1.RowCount = dataSource.Rows.Count;
            radGridView_can1.BeginEdit();
            radGridView_can1.DataSource = dataSourceCan1;
            gridViewControl.RefreshRadViewColumnCan1();
            radGridView_can1.EndUpdate();
        }

        private void GridViewLoadDataSourceCan2()
        {
            //设置虚模式
            //radGridView1.VirtualMode = true;
            //radGridView1.ColumnCount = dataSource.Columns.Count;
            //radGridView1.RowCount = dataSource.Rows.Count;
            radGridView_can2.BeginEdit();
            radGridView_can2.DataSource = dataSourceCan2;
            gridViewControl.RefreshRadViewColumnCan2();
            radGridView_can2.EndUpdate();
        }
        #endregion

        #region load tree view
        /// <summary>
        /// 加载TreeView
        /// </summary>
        public void LoadTreeView()
        {
            TreeViewControl treeView = new TreeViewControl(rTreeViewList);
            treeView.InitTreeView();
            rTreeViewList.NodeMouseClick += RtreeViewList_NodeMouseClick;
        }
        #endregion

        #region load grid view
        /// <summary>
        /// 加载grid view
        /// </summary>
        private void LoadRadGridView()
        {
            gridViewControl = new GridViewControl(radGridView_can1,radGridView_can2);
            gridViewControl.InitGridView();
            radGridView_can1.CellValueNeeded += RadGridView1_CellValueNeeded;
            radGridView_can1.CellValuePushed += RadGridView1_CellValuePushed;
        }

        private void RadGridView1_CellValuePushed(object sender, GridViewCellValueEventArgs e)
        {
            //编辑数据
        }

        private void RadGridView1_CellValueNeeded(object sender, GridViewCellValueEventArgs e)
        {
            if (e.RowIndex == radGridView_can1.RowCount)
            {
                return;
            }
            /// 从记录集中读取数据
            if (selectedCan == SelectedCan.CAN_1)
            {
                e.Value = dataSourceCan1.Rows[e.RowIndex][e.ColumnIndex].ToString();
            }
            else if (selectedCan == SelectedCan.CAN_2)
            {
                e.Value = dataSourceCan2.Rows[e.RowIndex][e.ColumnIndex].ToString();
            }
        }
        #endregion

        #region tree view node events 
        /// <summary>
        /// tree view node 点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RtreeViewList_NodeMouseClick(object sender, RadTreeViewEventArgs e)
        {
            RadTreeNode treeNode = e.Node;
            switch (treeNode.Text)
            {
                case TreeViewData.HardWare.CAN_1_DATA:
                    this.radDock1.AddDocument(documentWindow_can1);
                    break;
                case TreeViewData.HardWare.CAN_HARDWARE_CONFIG:
                    this.radDock1.AddDocument(documentWindow_hardWare);
                    break;
                case TreeViewData.HardWare.CAN_2_DATA:
                    this.radDock1.AddDocument(documentWindow_can2);
                    break;

                case TreeViewData.CcpOrXcp.DESCRIPTIONS:

                    break;
            }
        }
        #endregion
    }
}
