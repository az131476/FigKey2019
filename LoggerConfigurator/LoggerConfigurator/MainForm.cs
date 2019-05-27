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

namespace LoggerConfigurator
{
    public partial class MainForm : Form
    {
        #region 私有成员变量
        private FileContent openFileContent;
        private int protocol;
        private XcpData xcpdata;
        private XcpHelper xcpHelper;
        private DBCData dbcData;
        private DbcHelper dbcHelper;
        private GridViewControl gridViewControl;
        private GridViewData gridViewData;
        private LimitTimeCfg limitCfg;
        private AnalysisData analysisData;
        private FileType analysisFileType;
        #endregion
        public MainForm()
        {
            InitializeComponent();
            Initial();
            LoadTreeView();
            LoadRadGridView();
        }

        private void Initial()
        {
            int width = Screen.PrimaryScreen.Bounds.Width;
            int height = Screen.PrimaryScreen.Bounds.Height;
            //this.Size = new Size(width, height);
            gridViewData = new GridViewData();
            gridViewData.LimitTimeListSegMent = new List<LimitTimeCfg>();
            gridViewData.LimitTimeList10ms = new List<LimitTimeCfg>();
            gridViewData.LimitTimeList100ms = new List<LimitTimeCfg>();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            openXcp.Click += OpenXcp_Click;
            rbtn_search.Click += Rbtn_search_Click;
            xcpFilterCondition.KeyDown += XcpFilterCondition_KeyDown;

            radBtnExport.Click += RadBtnExport_Click;
            radGridView1.ValueChanged += RadGridView1_ValueChanged;

            tool_exportfile.Click += Tool_exportfile_Click;
        }

        #region 复选框行值处理
        /// <summary>
        /// 复选框状态添加行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadGridView1_ValueChanged(object sender, EventArgs e)
        {
            //设置不可选
            switch (this.radGridView1.CurrentCell.ColumnIndex)
            {
                case 12:
                    if (this.radGridView1.CurrentRow.Cells[13].Value.ToString() is "1")
                        this.radGridView1.CurrentRow.Cells[13].Value = 0;
                    if (this.radGridView1.CurrentRow.Cells[14].Value.ToString() is "1")
                        this.radGridView1.CurrentRow.Cells[14].Value = 0;
                    break;
                case 13:
                    if (this.radGridView1.CurrentRow.Cells[12].Value.ToString() is "1")
                        this.radGridView1.CurrentRow.Cells[12].Value = 0;
                    if (this.radGridView1.CurrentRow.Cells[14].Value.ToString() is "1")
                        this.radGridView1.CurrentRow.Cells[14].Value = 0;
                    break;
                case 14:
                    if (this.radGridView1.CurrentRow.Cells[12].Value.ToString() is "1")
                        this.radGridView1.CurrentRow.Cells[12].Value = 0;
                    if (this.radGridView1.CurrentRow.Cells[13].Value.ToString() is "1")
                        this.radGridView1.CurrentRow.Cells[13].Value = 0;
                    break;
            }

            if (this.radGridView1.ActiveEditor is RadCheckBoxEditor)
            {
                int rowIndex = this.radGridView1.CurrentCell.RowIndex;
                int columnIndex = this.radGridView1.CurrentCell.ColumnIndex;

                if (this.radGridView1.ActiveEditor.Value.ToString() is "On")
                {
                    limitCfg = new LimitTimeCfg();
                    limitCfg.RowIndex = rowIndex;
                    if (columnIndex == 12)
                    {
                        if (!ExistCurCheckBoxRow(rowIndex, gridViewData.LimitTimeListSegMent))
                        {
                            limitCfg.LimitType = TimeLimitType._segMent;
                            gridViewData.LimitTimeListSegMent.Add(limitCfg);
                        }
                    }
                    else if (columnIndex == 13)
                    {
                        if (!ExistCurCheckBoxRow(rowIndex, gridViewData.LimitTimeList10ms))
                        {
                            limitCfg.LimitType = TimeLimitType._10ms;
                            gridViewData.LimitTimeList10ms.Add(limitCfg);
                        }
                    }
                    else if (columnIndex == 14)
                    {
                        if (!ExistCurCheckBoxRow(rowIndex, gridViewData.LimitTimeList100ms))
                        {
                            limitCfg.LimitType = TimeLimitType._100ms;
                            gridViewData.LimitTimeList100ms.Add(limitCfg);
                        }
                    }
                }
                else if (this.radGridView1.ActiveEditor.Value.ToString() is "Off")
                {
                    if (columnIndex == 12)
                    {
                        if (ExistCurCheckBoxRow(rowIndex, gridViewData.LimitTimeListSegMent))
                        {
                            gridViewData.LimitTimeListSegMent.Remove(limitCfg);
                        }
                    }
                    else if (columnIndex == 13)
                    {
                        if (ExistCurCheckBoxRow(rowIndex, gridViewData.LimitTimeList10ms))
                        {
                            gridViewData.LimitTimeList10ms.Remove(limitCfg);
                        }
                    }
                    else if (columnIndex == 14)
                    {
                        if (ExistCurCheckBoxRow(rowIndex, gridViewData.LimitTimeList100ms))
                        {
                            gridViewData.LimitTimeList100ms.Remove(limitCfg);
                        }
                    }
                }
                ////三列CheckBox 只能选择一列
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
        /// 导出选择行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadBtnExport_Click(object sender, EventArgs e)
        {
            if (gridViewData.LimitTimeListSegMent.Count < 1 && gridViewData.LimitTimeList10ms.Count < 1 && gridViewData.LimitTimeList100ms.Count < 1)
            {
                MessageBox.Show("未选择行","提示");
                return;
            }
            string path = FileSelect.SaveAs("(*.c)|*.c");

            if (analysisFileType == FileType.A2L)
            {
                ExportFile.ExportFileToLocal(path, radGridView1, gridViewData,analysisData,analysisFileType);
            }
            else if (analysisFileType == FileType.DBC)
            {
                ExportFile.ExportFileToLocal(path, radGridView1, gridViewData, analysisData,analysisFileType);
            }
        }
        #endregion

        #region 导出数据/快捷菜单
        /// <summary>
        /// 导出所有数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tool_exportfile_Click(object sender, EventArgs e)
        {
            if (gridViewData.LimitTimeListSegMent.Count < 1 && gridViewData.LimitTimeList10ms.Count < 1 && gridViewData.LimitTimeList100ms.Count < 1)
            {
                MessageBox.Show("未选择行", "提示");
                return;
            }
            string path = FileSelect.SaveAs("*.c)|*.c");
            ExportFile.ExportFileToLocal(path, radGridView1, gridViewData, analysisData,analysisFileType);
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

        /// <summary>
        /// 关键字搜索data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rbtn_search_Click(object sender, EventArgs e)
        {
            gridViewControl.SearchRadGridView(xcpdata,xcpFilterCondition.Text.Trim());
        }
        #endregion

        #region 打开文件
        /// <summary>
        /// 打开文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenXcp_Click(object sender, EventArgs e)
        {
            openFileContent = FileSelect.GetSelectFileContent("(*.dbc)|*.dbc|(*.A2L)|*.a2l","选择文件");
            if (openFileContent == null)
                return;
            if (string.IsNullOrEmpty(openFileContent.FileName))
                return;
            xcpFilePathCur.Text = openFileContent.FileName;
            if (!string.IsNullOrEmpty(openFileContent.FileName))
            {
                ///解析文件
                ///
                protocol = 1;//设置为1，目前用不到，需要时从新取值
                //判断文件类型
                try
                {
                    switch (openFileContent.OpenFileResult.FilterIndex)
                    {
                        case (int)FileType.DBC:
                            //case = 1 解析DBC文件
                            dbcData = new DBCData();
                            dbcHelper = new DbcHelper(dbcData);
                            analysisFileType = FileType.DBC;
                            DbcResultEnum dbcResult = dbcHelper.AnalysisDbc(openFileContent.FileName);
                            if (dbcResult == DbcResultEnum.SUCCESS)
                            {
                                LogHelper.Log.Info("【解析DBC文件成功！】");
                                analysisData = AnalysisDataSet.UnionXcpDbc(FileType.DBC, null, dbcData);
                                if (analysisData != null)
                                {
                                    LogHelper.Log.Info("DBC合并数据完成!" + analysisData.AnalysisDbcDataList.Count);
                                }
                                else
                                {
                                    LogHelper.Log.Info("DBC合并数据失败!");
                                }
                                gridViewControl.BindRadGridView(analysisData.AnalysisDbcDataList);
                                //gridViewControl.BindRadGridView(xcpdata);
                                LogHelper.Log.Info("DBC加载完成！");
                            }
                            else if (dbcResult == DbcResultEnum.FAILT)
                            {
                                LogHelper.Log.Info("【解析DBC文件失败！】");
                            }
                            break;
                        case (int)FileType.A2L:
                            //case = 2 解析a2l文件
                            xcpdata = new XcpData();
                            xcpHelper = new XcpHelper(xcpdata);
                            analysisFileType = FileType.A2L;
                            LogHelper.Log.Info("开始解析A2L...");
                            CodeCommand res = xcpHelper.AnalyzeXcpFile(openFileContent.FileName, protocol);
                            if (res == CodeCommand.RESULT)
                            {
                                LogHelper.Log.Info("**********解析a2l文件成功************" + res);
                                //将解析后的数据绑定到gridview显示
                                analysisData = AnalysisDataSet.UnionXcpDbc(FileType.A2L,xcpdata,null);
                                if (analysisData != null)
                                {
                                    LogHelper.Log.Info("a2l合并数据完成!" + analysisData.AnalysisiXcpDataList.Count);
                                }
                                else
                                {
                                    LogHelper.Log.Info("a2l合并数据失败!");
                                }
                                gridViewControl.BindRadGridView(analysisData.AnalysisiXcpDataList);
                                //gridViewControl.BindRadGridView(xcpdata);
                                LogHelper.Log.Info("a2l加载完成！");
                            }
                            else
                            {
                                LogHelper.Log.Error("********解析a2l文件失败********** " + res);
                            }
                            break;
                        default:
                            break;
                    }
                    
                }
                catch (Exception ex)
                {
                    LogHelper.Log.Error(ex.Message+"\r\n"+ex.StackTrace);
                }
            }
        }
        #endregion

        #region load tree view
        /// <summary>
        /// 加载TreeView
        /// </summary>
        public void LoadTreeView()
        {
            TreeViewControl treeView = new TreeViewControl(rtreeViewList);
            treeView.InitTreeView();
            rtreeViewList.NodeMouseClick += RtreeViewList_NodeMouseClick;
        }
        #endregion

        #region load grid view
        /// <summary>
        /// 加载grid view
        /// </summary>
        private void LoadRadGridView()
        {
            gridViewControl = new GridViewControl(radGridView1);
            gridViewControl.InitGridView();
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
            if (treeNode.Text.Equals(TreeViewData.CcpOrXcp.DESCRIPTIONS))
            {
                
                
            }
        }
        #endregion
    }
}
