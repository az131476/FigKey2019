using System;
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
        private XcpData xcpdataCan;
        private XcpHelper xcpHelperCan;
        private DBCData dbcDataCan;
        private DbcHelper dbcHelperCan;
        private GridViewControl gridViewControl;
        private GridViewData a2lGridViewSelectRow;
        private GridViewData dbcGrieViewSelectRow;
        private LimitTimeCfg limitCfg;
        private AnalysisData analysisData;
        private ExportCANDocument exportCanDocument;
        //private FileType analysisFileType;
        private AgreementType agreementTypeCan1 = AgreementType.other, agreementTypeCan2 = AgreementType.other;
        private AgreementType lastAgreementCan1 = AgreementType.other, lastAgreementCan2 = AgreementType.other;
        private DataTable dataSourceCan1;
        private DataTable dataSourceCan2;
        private Dictionary<string, string> canDicCheckState_12;
        private Dictionary<string, string> canDicCheckState_13;
        //private Dictionary<string, string> can2DicCheckState_12;
        private List<Dictionary<CurrentCanType, CanProtocolDataEntity>> canProtocolDataSourchList;
        
        private CurrentCanType selectedCan = CurrentCanType.NONE;//当前选择的CAN，CAN1或CAN2两种情况
        private bool IsFirstOppendCan1 = true, IsFirstOppendCan2 = true;
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

            this.radGridView_can1.ValueChanged += RadGridView1_ValueChanged;
            this.radGridView_can2.ValueChanged += RadGridView_can2_ValueChanged;

            this.tool_exportfile.Click += Tool_exportfile_Click;
            this.tool_openFile.Click += Tool_openFile_Click;
            this.tool_search.Click += Tool_search_Click;
            this.menu_logger_manager.Click += Menu_logger_manager_Click;
            this.tool_searchText.KeyDown += Tool_searchText_KeyDown;

            this.cb_protocol_can1.SelectedIndexChanged += Cb_protocol_can1_SelectedIndexChanged;
            this.cb_protocol_can2.SelectedIndexChanged += Cb_protocol_can2_SelectedIndexChanged;
            this.radDock1.DockStateChanged += RadDock1_DockStateChanged;
        }

        private void RadDock1_DockStateChanged(object sender, DockWindowEventArgs e)
        {
            if (this.documentWindow_can1.DockState != DockState.TabbedDocument && this.documentWindow_can2.DockState != DockState.TabbedDocument)
            {
                this.selectedCan = CurrentCanType.NONE;
            }
            else if (this.documentWindow_can1.DockState != DockState.TabbedDocument && this.documentWindow_can2.DockState == DockState.TabbedDocument)
                this.selectedCan = CurrentCanType.CAN2;
            else if (this.documentWindow_can1.DockState == DockState.TabbedDocument && this.documentWindow_can2.DockState != DockState.TabbedDocument)
                this.selectedCan = CurrentCanType.CAN1;
        }

        private void Tool_searchText_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (selectedCan == CurrentCanType.CAN1)
                {
                    SearchGridViewCAN(this.radGridView_can1, agreementTypeCan1, dataSourceCan1);
                }
                else if (selectedCan == CurrentCanType.CAN2)
                {
                    SearchGridViewCAN(this.radGridView_can2, agreementTypeCan2, dataSourceCan2);
                }
            }
        }

        private void Tool_search_Click(object sender, EventArgs e)
        {
            if (selectedCan == CurrentCanType.CAN1)
            {
                SearchGridViewCAN(this.radGridView_can1,agreementTypeCan1,dataSourceCan1);
            }
            else if (selectedCan == CurrentCanType.CAN2)
            {
                SearchGridViewCAN(this.radGridView_can2, agreementTypeCan2, dataSourceCan2);
            }
        }

        private void Tool_openFile_Click(object sender, EventArgs e)
        {
            if (selectedCan == CurrentCanType.CAN1)
            {
                OpenAnalysisFile(selectedCan, this.cb_protocol_can1.Text);
            }
            else if (selectedCan == CurrentCanType.CAN2)
            {
                OpenAnalysisFile(selectedCan, this.cb_protocol_can2.Text);
            }
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

        public enum ExportCANDocument
        {
            CAN_1 = 1,
            CAN_2 = 2,
            CAN_1_2 = 3
        }

        private void Initial()
        {
            a2lGridViewSelectRow = new GridViewData();
            dbcGrieViewSelectRow = new GridViewData();
            a2lGridViewSelectRow.LimitTimeListSegMent = new List<int>();
            a2lGridViewSelectRow.LimitTimeList10ms = new List<int>();
            a2lGridViewSelectRow.LimitTimeList100ms = new List<int>();
            dbcGrieViewSelectRow.DbcCheckIndex = new List<int>();

            canDicCheckState_12 = new Dictionary<string, string>();//12cell row-state
            canDicCheckState_13 = new Dictionary<string, string>();//13cell row-state
            //配置
            //can1
            cb_baud_can1.Items.Clear();
            cb_baud_can1.Items.Add("100000");
            cb_baud_can1.Items.Add("500000");
            cb_baud_can1.Items.Add("1000000");
            cb_baud_can1.SelectedIndex = 1;

            cb_protocol_can1.Items.Clear();
            cb_protocol_can1.Items.Add(AgreementType.CCP);
            cb_protocol_can1.Items.Add(AgreementType.XCP);
            cb_protocol_can1.Items.Add(AgreementType.DBC);
            cb_protocol_can1.Items.Add("");
            cb_protocol_can1.SelectedIndex = 0;

            //can2
            cb_baud_can2.Items.Clear();
            cb_baud_can2.Items.Add("100000");
            cb_baud_can2.Items.Add("500000");
            cb_baud_can2.Items.Add("1000000");
            cb_baud_can2.SelectedIndex = 1;

            cb_protocol_can2.Items.Clear();
            cb_protocol_can2.Items.Add(AgreementType.CCP);
            cb_protocol_can2.Items.Add(AgreementType.XCP);
            cb_protocol_can2.Items.Add(AgreementType.DBC);
            cb_protocol_can2.Items.Add("");
            cb_protocol_can2.SelectedIndex = 0;
            //document set
            this.radDock1.RemoveAllDocumentWindows();

            lbx_protocol_remark.Text += "DBC时配置波特率有效，XCP直接从文件读取，配置无效";
            canProtocolDataSourchList = new List<Dictionary<CurrentCanType, CanProtocolDataEntity>>();
        }

        #region 复选框行值处理
        private static bool IsCheckCan1_12;
        private static bool IsCheckCan1_13;

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
                        if (!IsCheckCan1_12)
                        {
                            this.radGridView_can1.CurrentRow.Cells[12].Value = 1;
                            IsCheckCan1_12 = !IsCheckCan1_12;
                        }
                        else
                        {
                            this.radGridView_can1.CurrentRow.Cells[12].Value = 0;
                            IsCheckCan1_12 = !IsCheckCan1_12;
                        }
                        if (agreementTypeCan1 == AgreementType.CCP || agreementTypeCan1 == AgreementType.XCP)
                        {
                            if (this.radGridView_can1.CurrentRow.Cells[13].Value.ToString() is "1")
                                this.radGridView_can1.CurrentRow.Cells[13].Value = 0;
                        }
                        break;
                    case 13:
                        if (!IsCheckCan1_13)
                        {
                            this.radGridView_can1.CurrentRow.Cells[13].Value = 1;
                            IsCheckCan1_13 = !IsCheckCan1_13;
                        }
                        else
                        {
                            this.radGridView_can1.CurrentRow.Cells[13].Value = 0;
                            IsCheckCan1_13 = !IsCheckCan1_13;
                        }
                        if (agreementTypeCan1 == AgreementType.CCP || agreementTypeCan1 == AgreementType.XCP)
                        {
                            if (this.radGridView_can1.CurrentRow.Cells[12].Value.ToString() is "1")
                                this.radGridView_can1.CurrentRow.Cells[12].Value = 0;
                        }
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
        private static bool IsCheckCan2_12,IsCheckCan2_13;
        private void RadGridView_can2_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                switch (this.radGridView_can2.CurrentCell.ColumnIndex)
                {
                    case 12:
                        if (!IsCheckCan2_12)
                        {
                            this.radGridView_can2.CurrentRow.Cells[12].Value = 1;
                            IsCheckCan2_12 = !IsCheckCan2_12;
                        }
                        else
                        {
                            this.radGridView_can2.CurrentRow.Cells[12].Value = 0;
                            IsCheckCan2_12 = !IsCheckCan2_12;
                        }
                        if (agreementTypeCan2 == AgreementType.CCP || agreementTypeCan2 == AgreementType.XCP)
                        {
                            if (this.radGridView_can2.CurrentRow.Cells[13].Value.ToString() is "1")
                                this.radGridView_can2.CurrentRow.Cells[13].Value = 0;
                        }
                        break;
                    case 13:
                        if (!IsCheckCan2_13)
                        {
                            this.radGridView_can2.CurrentRow.Cells[13].Value = 1;
                            IsCheckCan2_13 = !IsCheckCan2_13;
                        }
                        else
                        {
                            this.radGridView_can2.CurrentRow.Cells[13].Value = 0;
                            IsCheckCan2_13 = !IsCheckCan2_13;
                        }
                        if (agreementTypeCan2 == AgreementType.CCP || agreementTypeCan2 == AgreementType.XCP)
                        {
                            if (this.radGridView_can2.CurrentRow.Cells[12].Value.ToString() is "1")
                                this.radGridView_can2.CurrentRow.Cells[12].Value = 0;
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
        private void ExportData(CurrentCanType currentCanType)
        {
            SelectedRowCal();
            string sourcePath = AppDomain.CurrentDomain.BaseDirectory + @"编译器\";
            if (!Directory.Exists(sourcePath))
                sourcePath = AppDomain.CurrentDomain.BaseDirectory;
            //string path = FileSelect.SaveAs("(*.c)|*.c",sourcePath);
            string path = sourcePath + "data.c";
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            XcpData xcpDataCan1 = null,xcpDataCan2 = null;
            DBCData dBCDataCan1 = null,dBCDataCan2 = null;
            AnalysisData analysisDataCan1 = null, analysisDataCan2 = null;
            AgreementType agreementType1 = AgreementType.other, agreementType2 = AgreementType.other;
            foreach (var dicList in canProtocolDataSourchList)
            {
                foreach (var keyValuePair in dicList)
                {
                    if (keyValuePair.Key == CurrentCanType.CAN1)
                    {
                        agreementType1 = keyValuePair.Value.AgreementType;
                        if (keyValuePair.Value.AgreementType == AgreementType.XCP || keyValuePair.Value.AgreementType == AgreementType.CCP)
                        {
                            xcpDataCan1 = keyValuePair.Value.CanXcpData;
                        }
                        else if (keyValuePair.Value.AgreementType == AgreementType.DBC)
                        {
                            dBCDataCan1 = keyValuePair.Value.CanDbcData;
                        }
                        analysisDataCan1 = keyValuePair.Value.CanAnalysisData;
                    }
                    else if (keyValuePair.Key == CurrentCanType.CAN2)
                    {
                        agreementType2 = keyValuePair.Value.AgreementType;
                        if (keyValuePair.Value.AgreementType == AgreementType.XCP || keyValuePair.Value.AgreementType == AgreementType.CCP)
                        {
                            xcpDataCan2 = keyValuePair.Value.CanXcpData;
                        }
                        else if (keyValuePair.Value.AgreementType == AgreementType.DBC)
                        {
                            dBCDataCan2 = keyValuePair.Value.CanDbcData;
                        }
                        analysisDataCan2 = keyValuePair.Value.CanAnalysisData;
                    }
                }
            }
            GridExportContent gridExportContent = new GridExportContent();
            gridExportContent.GridViewCan1 = this.radGridView_can1;
            gridExportContent.GridViewCan2 = this.radGridView_can2;
            gridExportContent.GridDataA2lSelectRow = a2lGridViewSelectRow;
            gridExportContent.GridDataDbcSelectRow = dbcGrieViewSelectRow;
            gridExportContent.AnalysisDataCan1 = analysisDataCan1;
            gridExportContent.AnalysisDataCan2 = analysisDataCan2;
            gridExportContent.XcpDataCan1 = xcpDataCan1;
            gridExportContent.XcpDataCan2 = xcpDataCan2;
            gridExportContent.AgreementTypeCan1 = agreementType1;
            gridExportContent.AgreementTypeCan2 = agreementType2;
            gridExportContent.CurrentSelectCanType = currentCanType;
            ExportFile.ExportCanFile(path,sourcePath,gridExportContent);
        }
        #endregion

        /// <summary>
        /// 遍历行，查询CHECKBOX选中行
        /// </summary>
        private void SelectedRowCal()
        {
            int a2lIndex = 0;
            int dbcIndex = 0;
            a2lGridViewSelectRow.LimitTimeListSegMent.Clear();
            a2lGridViewSelectRow.LimitTimeList10ms.Clear();
            a2lGridViewSelectRow.LimitTimeList100ms.Clear();
            dbcGrieViewSelectRow.DbcCheckIndex.Clear();

            //can1
            if (agreementTypeCan1 == AgreementType.DBC)
            {
                foreach (GridViewRowInfo row in this.radGridView_can1.Rows)
                {
                    var v = this.radGridView_can1.Rows[dbcIndex].Cells[12].Value;
                    if (v == null)
                        continue;
                    if (v.ToString() is "1")
                    {
                        dbcGrieViewSelectRow.DbcCheckIndex.Add(dbcIndex);
                    }
                    dbcIndex++;
                }
            }
            else if (agreementTypeCan1 == AgreementType.CCP || agreementTypeCan1 == AgreementType.XCP)
            {
                foreach (GridViewDataRowInfo row in this.radGridView_can1.Rows)
                {
                    //将选中得行添加到集合
                    var v12 = this.radGridView_can1.Rows[a2lIndex].Cells[12].Value;
                    var v13 = this.radGridView_can1.Rows[a2lIndex].Cells[13].Value;
                    if (v12 == null && v13 == null)
                        continue;
                    if (v12.ToString() is "1")
                    {
                        a2lGridViewSelectRow.LimitTimeList10ms.Add(a2lIndex);
                    }
                    if (v13.ToString() is "1")
                    {
                        a2lGridViewSelectRow.LimitTimeList100ms.Add(a2lIndex);
                    }
                    a2lIndex++;
                }
            }
            //can2
            if (agreementTypeCan2 == AgreementType.CCP || agreementTypeCan2 == AgreementType.XCP)
            {
                foreach (GridViewDataRowInfo row in this.radGridView_can2.Rows)
                {
                    //将选中得行添加到集合
                    var v12 = this.radGridView_can2.Rows[a2lIndex].Cells[12].Value;
                    var v13 = this.radGridView_can2.Rows[a2lIndex].Cells[13].Value;
                    if (v12 == null && v13 == null)
                        continue;
                    if (v12.ToString() is "1")
                    {
                        a2lGridViewSelectRow.LimitTimeList10ms.Add(a2lIndex);
                    }
                    if (v13.ToString() is "1")
                    {
                        a2lGridViewSelectRow.LimitTimeList100ms.Add(a2lIndex);
                    }
                    a2lIndex++;
                }
            }
            else if (agreementTypeCan2 == AgreementType.DBC)
            {
                foreach (GridViewRowInfo row in this.radGridView_can2.Rows)
                {
                    var v12 = this.radGridView_can2.Rows[dbcIndex].Cells[12].Value;
                    if (v12 == null)
                        continue;
                    if (v12.ToString() is "1")
                    {
                        dbcGrieViewSelectRow.DbcCheckIndex.Add(dbcIndex);
                    }
                    dbcIndex++;
                }
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
            ExportSet exportSet = new ExportSet();
            if (exportSet.ShowDialog() == DialogResult.OK)
            {
                if (ExportSet.can1Check && !ExportSet.can2Check)
                {
                    if (this.documentWindow_can1.DockState != DockState.TabbedDocument)
                    {
                        MessageBox.Show("CAN1通道没有数据","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }
                    ExportData(CurrentCanType.CAN1);
                }
                else if (ExportSet.can2Check && !ExportSet.can1Check)
                {
                    if (this.documentWindow_can2.DockState != DockState.TabbedDocument)
                    {
                        MessageBox.Show("CAN2通道没有数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ExportData(CurrentCanType.CAN2);
                }
                else if (ExportSet.can1Check && ExportSet.can2Check)
                {
                    if (this.documentWindow_can1.DockState != DockState.TabbedDocument)
                    {
                        MessageBox.Show("CAN1通道没有数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    if (this.documentWindow_can2.DockState != DockState.TabbedDocument)
                    {
                        MessageBox.Show("CAN2通道没有数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    ExportData(CurrentCanType.CAN1_CAN2);
                }
                else if(!ExportSet.can1Check && !ExportSet.can2Check)
                {
                    MessageBox.Show("未选择CAN通道", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        #endregion

        #region 搜索 gridView data

        /// <summary>
        /// 关键字搜索data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SearchGridViewCAN(RadGridView radGridView,AgreementType agreementType,DataTable dataTable)
        {
            int dex = 0;
            //查询选中行，加入缓存
            foreach (GridViewRowInfo row in radGridView.Rows)
            {
                //判断
                var key = radGridView.Rows[dex].Cells[1].Value.ToString();
                object value12 = radGridView.Rows[dex].Cells[12].Value;
                object value13 = null;
                if (agreementType == AgreementType.CCP || agreementType == AgreementType.XCP)
                {
                    value13 = radGridView.Rows[dex].Cells[13].Value;
                }
                if (!canDicCheckState_12.ContainsKey(key))
                {
                    if (value12 != null)
                    {
                        if (value12.ToString() == "1")
                        {
                            canDicCheckState_12.Add(key, value12.ToString());
                        }
                    }
                }
                else
                {
                    canDicCheckState_12.Remove(key);
                    canDicCheckState_12.Add(key,value12.ToString());
                }
                if (!canDicCheckState_13.ContainsKey(key))
                {
                    if (value13 != null)
                    {
                        if (value13.ToString() == "1")
                        {
                            canDicCheckState_13.Add(key, value13.ToString());
                        }
                    }
                }
                else
                {
                    canDicCheckState_13.Remove(key);
                    canDicCheckState_13.Add(key,value13.ToString());
                }
                dex++;
            }
            DataTable updateDt = dataSourceCan1.Clone();
            if (dataSourceCan1.Rows.Count < 1)
                return;
            string filter = tool_searchText.Text.Trim();
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
            DataRow[] dataRows = dataTable.Select(condition);
            foreach (var row in dataRows)
            {
                updateDt.Rows.Add(row.ItemArray);
            }
            radGridView.BeginEdit();
            radGridView.DataSource = updateDt;
            //gridViewControl.RefreshRadViewColumnCan1();
            radGridView.EndUpdate();
            tool_searchText.Text = "";
            tool_searchText.Focus();
            //恢复选中状态
            int rIndex12 = 0;
            foreach (GridViewDataRowInfo row in radGridView.Rows)
            {
                foreach (KeyValuePair<string, string> kv in canDicCheckState_12)
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
            if (agreementType == AgreementType.XCP || agreementType == AgreementType.CCP)
            {
                int rIndex13 = 0;
                foreach (GridViewDataRowInfo row in radGridView.Rows)
                {
                    foreach (KeyValuePair<string, string> kv in canDicCheckState_13)
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
        }
        #endregion

        #region 打开文件
        /// <summary>
        /// 根据选择的协议类型打开文件
        /// </summary>
        /// <param name="selectedCan"></param>
        private void OpenAnalysisFile(CurrentCanType selectedCan, string inputProtocol)
        {
            if (selectedCan == CurrentCanType.NONE)
                return;//未选择CAN，打开文件无效
            AgreementType currentProtocol;
            if (inputProtocol == "")
            {
                MessageBox.Show("未选择协议", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!Enum.TryParse(inputProtocol, out currentProtocol))
            {
                MessageBox.Show($"不支持协议【{inputProtocol}】，请重新选择协议", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var openFilter = "(*.*)|*.*";
            if (currentProtocol == AgreementType.CCP || currentProtocol == AgreementType.XCP)
            {
                openFilter = "(*.a2l)|*.a2l";
            }
            else if (currentProtocol == AgreementType.DBC)
            {
                openFilter = "(*.dbc)|*.dbc";
            }
            openFileContent = FileSelect.GetSelectFileContent(openFilter, "选择文件");
            if (openFileContent == null)
                return;
            if (string.IsNullOrEmpty(openFileContent.FileName))
                return;
            //开始解析
            StartAnalysis(selectedCan,currentProtocol);
        }
        private void StartAnalysis(CurrentCanType selectedCan, AgreementType protocolType)
        {
            if (protocolType == AgreementType.XCP || protocolType == AgreementType.CCP)
            {
                LogHelper.Log.Info("开始解析XCP/CCP...");
                xcpdataCan = new XcpData();
                xcpHelperCan = new XcpHelper(xcpdataCan);
                if(!A2lAnalysis(protocolType))
                    return;
            }
            else if (protocolType == AgreementType.DBC)
            {
                LogHelper.Log.Info("开始解析DBC...");
                dbcDataCan = new DBCData();
                dbcHelperCan = new DbcHelper(dbcDataCan);
                DbcAnalysis();
            }
            //全部解析完成，添加到集合
            CanProtocolDataEntity canProtocolDataEntity = new CanProtocolDataEntity();
            Dictionary<CurrentCanType, CanProtocolDataEntity> keyValuePairs = new Dictionary<CurrentCanType, CanProtocolDataEntity>();
            if(xcpdataCan != null)
                canProtocolDataEntity.CanXcpData = xcpdataCan;
            if(dbcDataCan != null)
                canProtocolDataEntity.CanDbcData = dbcDataCan;
            if(xcpHelperCan != null)
                canProtocolDataEntity.CanXcpHelper = xcpHelperCan;
            if(dbcHelperCan != null)
                canProtocolDataEntity.CanDbcHelper = dbcHelperCan;
            canProtocolDataEntity.CanAnalysisData = analysisData;
            canProtocolDataEntity.AgreementType = protocolType;
            keyValuePairs.Add(selectedCan, canProtocolDataEntity);
            canProtocolDataSourchList.Add(keyValuePairs);
            LoadAnalysisDataSourch(canProtocolDataSourchList,protocolType);
        }
        private void LoadAnalysisDataSourch(List<Dictionary<CurrentCanType, CanProtocolDataEntity>> list,AgreementType agreementType)
        {
            foreach (var dicList in list)
            {
                foreach (var keyValuePair in dicList)
                {
                    if (keyValuePair.Key == CurrentCanType.CAN1)
                    {
                        agreementTypeCan1 = keyValuePair.Value.AgreementType;
                        if (keyValuePair.Value.AgreementType == AgreementType.XCP || keyValuePair.Value.AgreementType == AgreementType.CCP)
                        {
                            dataSourceCan1 = gridViewControl.BindRadGridView(keyValuePair.Value.CanAnalysisData.AnalysisiXcpDataList);
                        }
                        else if (keyValuePair.Value.AgreementType == AgreementType.DBC)
                        {
                            dataSourceCan1 = gridViewControl.BindRadGridView(keyValuePair.Value.CanAnalysisData.AnalysisDbcDataList);
                        }
                    }
                    else if (keyValuePair.Key == CurrentCanType.CAN2)
                    {
                        agreementTypeCan2 = keyValuePair.Value.AgreementType;
                        if (keyValuePair.Value.AgreementType == AgreementType.CCP || keyValuePair.Value.AgreementType == AgreementType.XCP)
                        {
                            dataSourceCan2 = gridViewControl.BindRadGridView(keyValuePair.Value.CanAnalysisData.AnalysisiXcpDataList);
                        }
                        else if (keyValuePair.Value.AgreementType == AgreementType.DBC)
                        {
                            dataSourceCan2 = gridViewControl.BindRadGridView(keyValuePair.Value.CanAnalysisData.AnalysisDbcDataList);
                        }
                    }
                }
            }
            GridViewLoadDataSourceCan(agreementType);
        }
        private void DbcAnalysis()
        {
            //case = 1 解析DBC文件
            DbcResultEnum dbcResult = DbcResultEnum.FAILT;
            dbcResult = dbcHelperCan.AnalysisDbc(openFileContent.FileName);

            if (dbcResult == DbcResultEnum.SUCCESS)
            {
                LogHelper.Log.Info("【解析DBC文件成功！】");
                analysisData = AnalysisDataSet.UnionXcpDbc(FileType.DBC, null, dbcDataCan, cb_baud_can2.Text);

                if (analysisData != null)
                {
                    LogHelper.Log.Info("【DBC波特率=】"+analysisData.BaudRateDbc);
                    LogHelper.Log.Info("DBC合并数据完成!" + analysisData.AnalysisDbcDataList.Count);
                }
                else
                {
                    LogHelper.Log.Info("DBC合并数据失败!");
                }
            }
            else if (dbcResult == DbcResultEnum.FAILT)
            {
                MessageBox.Show($"解析失败，异常代码{dbcResult}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private bool A2lAnalysis(AgreementType protocol)
        {
            //case = 2 解析a2l文件
            CodeCommand res = CodeCommand.DEAFUALT_FAIL;
            res = xcpHelperCan.AnalyzeXcpFile(openFileContent.FileName);

            if (res == CodeCommand.RESULT)
            {
                LogHelper.Log.Info("**********解析a2l文件成功************" + res);
                //判断选择协议与打开文件协议是否一致
                AgreementType readFileProtocol = xcpdataCan.AgreeMentType;
                if (protocol != readFileProtocol)
                {
                    MessageBox.Show("选择协议与打开文件协议类型不匹配，请重新配置协议或文件！","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    return false;
                }
                //判断协议类型是否为XCP,提示选择xcp on can
                if (xcpdataCan.AgreeMentType == AgreementType.XCP)
                {
                    XcpProtocol xcpProtocol = new XcpProtocol(xcpdataCan);
                    xcpProtocol.ShowDialog();
                }

                //将解析后的数据绑定到gridview显示
                analysisData = AnalysisDataSet.UnionXcpDbc(FileType.A2L, xcpdataCan, null, "");

                if (analysisData != null)
                {
                    LogHelper.Log.Info("a2l合并数据完成!" + analysisData.AnalysisiXcpDataList.Count);
                }
                else
                {
                    LogHelper.Log.Info("a2l合并数据失败!");
                }
            }
            else
            {
                MessageBox.Show($"解析失败，异常代码{res}","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
            return true;
        }
        private void GridViewLoadDataSourceCan(AgreementType agreementType)
        {
            if (selectedCan == CurrentCanType.CAN1)
            {
                radGridView_can1.BeginEdit();
                radGridView_can1.DataSource = dataSourceCan1;
                gridViewControl.SetRadViewColumnCheckCAN1(agreementType);
                radGridView_can1.EndUpdate();
                if (IsFirstOppendCan1)
                {
                    IsFirstOppendCan1 = false;
                    lastAgreementCan1 = agreementType;
                }
                else
                {
                    if (agreementType != lastAgreementCan1)
                    {
                        if (agreementType == AgreementType.CCP || agreementType == AgreementType.XCP)
                        {
                            if (lastAgreementCan1 == AgreementType.DBC)
                            {
                                //DBC在前，隐藏
                                this.radGridView_can1.Columns[12].IsVisible = false;
                            }
                        }
                        else if (agreementType == AgreementType.DBC)
                        {
                            if (lastAgreementCan1 == AgreementType.CCP || lastAgreementCan1 == AgreementType.XCP)
                            {
                                //a2l before hide
                                this.radGridView_can1.Columns[12].IsVisible = false;
                                this.radGridView_can1.Columns[13].IsVisible = false;
                            }
                        }
                    }
                    lastAgreementCan1 = agreementType;
                }
                this.radDock1.ActiveWindow = this.documentWindow_can1;
            }
            else if (selectedCan == CurrentCanType.CAN2)
            {
                radGridView_can2.BeginEdit();
                radGridView_can2.DataSource = dataSourceCan2;
                gridViewControl.SetRadViewColumnCheckCAN2(agreementType);
                radGridView_can2.EndUpdate();
                if (IsFirstOppendCan2)
                {
                    IsFirstOppendCan2 = false;
                    lastAgreementCan2 = agreementType;
                }
                else
                {
                    if (agreementType != lastAgreementCan2)
                    {
                        if (agreementType == AgreementType.CCP || agreementType == AgreementType.XCP)
                        {
                            if (lastAgreementCan2 == AgreementType.DBC)
                            {
                                //DBC在前，隐藏
                                this.radGridView_can2.Columns[12].IsVisible = false;
                            }
                        }
                        else if (agreementType == AgreementType.DBC)
                        {
                            if (lastAgreementCan2 == AgreementType.CCP || lastAgreementCan2 == AgreementType.XCP)
                            {
                                //a2l before hide
                                this.radGridView_can2.Columns[12].IsVisible = false;
                                this.radGridView_can2.Columns[13].IsVisible = false;
                            }
                        }
                    }
                    lastAgreementCan2 = agreementType;
                }
                this.radDock1.ActiveWindow = this.documentWindow_can2;
            }
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
            //if (selectedCan == SelectedCan.CAN_1)
            //{
            //    e.Value = dataSourceCan1.Rows[e.RowIndex][e.ColumnIndex].ToString();
            //}
            //else if (selectedCan == SelectedCan.CAN_2)
            //{
            //    e.Value = dataSourceCan2.Rows[e.RowIndex][e.ColumnIndex].ToString();
            //}
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
                    this.selectedCan = CurrentCanType.CAN1;
                    break;
                case TreeViewData.HardWare.CAN_HARDWARE_CONFIG:
                    this.radDock1.AddDocument(documentWindow_hardWare);
                    break;
                case TreeViewData.HardWare.CAN_2_DATA:
                    this.radDock1.AddDocument(documentWindow_can2);
                    this.selectedCan = CurrentCanType.CAN2;
                    break;

                case TreeViewData.CcpOrXcp.DESCRIPTIONS:

                    break;
            }
        }
        #endregion

        private void ExportGridViewData(int selectIndex, RadGridView radGridView)
        {
            var filter = "Excel (*.xls)|*.xls";
            if (selectIndex == (int)ExportFormat.EXCEL)
            {
                filter = "Excel (*.xls)|*.xls";
                var path = FileSelect.SaveAs(filter);//, "C:\\");
                if (path == "")
                    return;
                RadGridViewExport.RunExportToExcelML(path, radGridView);
            }
            else if (selectIndex == (int)ExportFormat.HTML)
            {
                filter = "Html File (*.htm)|*.htm";
                var path = FileSelect.SaveAs(filter);//, "C:\\");
                if (path == "")
                    return;
                RadGridViewExport.RunExportToHTML(path, radGridView);
            }
            else if (selectIndex == (int)ExportFormat.PDF)
            {
                filter = "PDF file (*.pdf)|*.pdf";
                var path = FileSelect.SaveAs(filter);//, "C:\\");
                if (path == "")
                    return;
                RadGridViewExport.RunExportToPDF(path, radGridView);
            }
            else if (selectIndex == (int)ExportFormat.CSV)
            {
                filter = "PDF file (*.pdf)|*.csv";
                var path = FileSelect.SaveAs(filter);//, "C:\\");
                if (path == "")
                    return;
                RadGridViewExport.RunExportToCSV(path, radGridView);
            }
        }
        private enum ExportFormat
        {
            EXCEL,
            HTML,
            PDF,
            CSV
        }
    }
}
