using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using AnalysisAgreeMent.Model.XCP;
using Telerik.WinControls.UI;
using FigKeyLoggerConfigurator.Model;
using CommonUtils.FileHelper;
using CommonUtils.Logger;
//using CommonUtils.tool;
using AnalysisAgreeMent.Model;
using AnalysisAgreeMent.Model.DBC;
using AnalysisAgreeMent.Analysis;
using System.Windows.Forms;
using System.Configuration;

namespace FigKeyLoggerConfigurator.Control
{
    public class ExportFile
    {
        #region 私有成员变量
        private static object obj = new object();
        private static object objAppend = new object();
        private const string HEADER_NAME = "static ";
        private const string FUN_TYPE = "XCPDataRecordType ";
        private const string SEGMENT_NAME = "xcpSegMent";
        private const string _10MS_NAME = "xcp10ms";
        private const string _100MS_NAME = "xcp100ms";
        private const string _A2L_ARRAY_CHAR = "[] = ";
        private const string _SPLIT_CHAR = "\r\n{\r\n";
        private const string CONTENT_LEN = "static ExInfoType ";
        private const string LEN_SEG_NAME = "xcpSegLen = ";
        private const string LEN_10MS_NAME = "xcp10msLen = ";
        private const string LEN_100MS_NAME = "xcp100msLen = ";
        private static string _segmentHeadName;
        private static string _10msHeadName;
        private static string _100msHeadName;
        private static string _lensegHeaderName;
        private static string _len10msHeaderName;
        private static string _len100msHeaderName;

        #region DBC
        private static List<string> frameIdList;//用于存储DBC明细时的分组ID
        private const string DBC_DETAIL_HEAD = "static XCPDataRecordType ";
        private const string DBC_DEATIL_METHOLD_NAME = "DBCTab";

        private const string EXINFO_TYPE_HEAD = "static ExInfoType ";
        private const string EXINFO_FUN_NAME_CAN1 = "ExInfoCan1";
        private const string EXINFO_FUN_NAME_CAN2 = "ExInfoCan2";
        private const string EXINFO_TYPE_METHOLD_NAME = "[] = \r\n{";

        private static List<StringBuilder> allDbcGroupData;
        private static List<AnalysisSignal> acturalDBCList;
        private static StringBuilder stringBuilderHead;

        private static int exInfoLen;
        #endregion

        enum ExInfoType
        {
            SLAVER_ID_TYPE = 0,
            MASTER_ID_TYPE = 1,
            DAQ10_ID_TYPE = 2,
            DAQ100_ID_TYPE = 3,
            DAQ10_TAB_TYPE = 4,
            DAQ100_TAB_TYPE = 5,
            MORNITOR_TAB_TYPE = 6,
            CCP_ECUADDR_TYPE = 7
        }

        enum ExportProtocolType
        {
            CHANNEL_CLOSE = 0,
            CCP_OPEN = 1,
            XCP_OPEN = 2,
            CAN_MONITOR =3
        }
        #endregion

        enum ProtocolType
        {
            XCP_CCP,
            DBC,
            ALL
        }

        #region 导出数据
        /// <summary>
        /// 导出a2l与dbc文件到本地
        /// </summary>
        public static void ExportCanFile(string targetPath,string excutePath, GridExportContent gridExportContent)
        {
            //判断是否选择CAN
            //选择CAN后是否有数据
            //有数据后，是否有选择要执行的行
            stringBuilderHead = new StringBuilder();
            File.Delete(targetPath);
            stringBuilderHead.AppendLine($"#include\"datatype.h\"");
            WriteData.WriteString(stringBuilderHead, targetPath);

            if (gridExportContent.CurrentSelectCanType == CurrentCanType.CAN1)
            {
                //是否加载数据
                if (gridExportContent.GridViewCan1.RowCount < 1)
                {
                    MessageBox.Show("CAN1通道没有可导出的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    File.Delete(targetPath);
                    return;
                }
                //是否选择数据
                if (gridExportContent.AgreementTypeCan1 == AgreementType.CCP || gridExportContent.AgreementTypeCan1 == AgreementType.XCP)
                {
                    //判断CAN1是否有数据
                    if (gridExportContent.GridDataA2lSelectRow.LimitTimeList10ms.Count < 1 && gridExportContent.GridDataA2lSelectRow.LimitTimeList100ms.Count < 1)
                    {
                        MessageBox.Show("CAN1通道未选择导出数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        File.Delete(targetPath);
                        return;
                    }
                    ExportXCP(targetPath, gridExportContent.GridViewCan1, gridExportContent.GridDataA2lSelectRow, gridExportContent.XcpDataCan1);
                }
                else if (gridExportContent.AgreementTypeCan1 == AgreementType.DBC)
                {
                    if (gridExportContent.GridDataDbcSelectRow.DbcCheckIndex.Count < 1)
                    {
                        MessageBox.Show("CAN1通道未选择导出数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        File.Delete(targetPath);
                        return;
                    }
                    ExportDBC(targetPath, gridExportContent.GridViewCan1, gridExportContent.GridDataDbcSelectRow, gridExportContent.XcpDataCan1, gridExportContent.AnalysisDataCan1);
                }
            }
            else if (gridExportContent.CurrentSelectCanType == CurrentCanType.CAN2)
            {
                //是否有数据
                if (gridExportContent.GridViewCan2.RowCount < 1)
                {
                    MessageBox.Show("CAN2通道没有可导出的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    File.Delete(targetPath);
                    return;
                }
                //是否选择数据
                if (gridExportContent.AgreementTypeCan2 == AgreementType.CCP || gridExportContent.AgreementTypeCan2 == AgreementType.XCP)
                {
                    //判断CAN2是否有数据
                    if (gridExportContent.GridDataA2lSelectRow.LimitTimeList10ms.Count < 1 && gridExportContent.GridDataA2lSelectRow.LimitTimeList100ms.Count < 1)
                    {
                        MessageBox.Show("CAN2通道未选择导出数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        File.Delete(targetPath);
                        return;
                    }
                    ExportXCP(targetPath, gridExportContent.GridViewCan2, gridExportContent.GridDataA2lSelectRow, gridExportContent.XcpDataCan2);
                }
                else if (gridExportContent.AgreementTypeCan2 == AgreementType.DBC)
                {
                    if (gridExportContent.GridDataDbcSelectRow.DbcCheckIndex.Count < 1)
                    {
                        MessageBox.Show("CAN2通道未选择导出数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        File.Delete(targetPath);
                        return;
                    }
                    ExportDBC(targetPath, gridExportContent.GridViewCan2, gridExportContent.GridDataDbcSelectRow, gridExportContent.XcpDataCan2, gridExportContent.AnalysisDataCan2);
                }
            }
            else if (gridExportContent.CurrentSelectCanType == CurrentCanType.CAN1_CAN2)
            {
                //是否有数据
                if (gridExportContent.GridViewCan1.RowCount < 1)
                {
                    MessageBox.Show("CAN1通道没有可导出的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    File.Delete(targetPath);
                    return;
                }
                if (gridExportContent.GridViewCan2.RowCount < 1)
                {
                    MessageBox.Show("CAN2通道没有可导出的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    File.Delete(targetPath);
                    return;
                }
                //是否选择数据
                if (gridExportContent.AgreementTypeCan1 == AgreementType.CCP || gridExportContent.AgreementTypeCan1 == AgreementType.XCP)
                {
                    //判断CAN1是否选择数据
                    if (gridExportContent.GridDataA2lSelectRow.LimitTimeList10ms.Count < 1 && gridExportContent.GridDataA2lSelectRow.LimitTimeList100ms.Count < 1)
                    {
                        MessageBox.Show("CAN1通道未选择导出数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        File.Delete(targetPath);
                        return;
                    }
                    if (gridExportContent.AgreementTypeCan2 == AgreementType.CCP || gridExportContent.AgreementTypeCan2 == AgreementType.XCP)
                    {
                        if (gridExportContent.GridDataA2lSelectRow.LimitTimeList10ms.Count < 1 && gridExportContent.GridDataA2lSelectRow.LimitTimeList100ms.Count < 1)
                        {
                            MessageBox.Show("CAN2通道未选择导出数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            File.Delete(targetPath);
                            return;
                        }
                        //xcp+xcp待验证
                        ExportXCP(targetPath, gridExportContent.GridViewCan1, gridExportContent.GridDataA2lSelectRow, gridExportContent.XcpDataCan1);
                        ExportXCP(targetPath, gridExportContent.GridViewCan2, gridExportContent.GridDataDbcSelectRow, gridExportContent.XcpDataCan2);
                    }
                    else if (gridExportContent.AgreementTypeCan2 == AgreementType.DBC)
                    {
                        if (gridExportContent.GridDataDbcSelectRow.DbcCheckIndex.Count < 1)
                        {
                            MessageBox.Show("CAN2通道未选择导出数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            File.Delete(targetPath);
                            return;
                        }
                        ExportDBCXCP(targetPath, gridExportContent);
                    }
                }
                else if (gridExportContent.AgreementTypeCan1 == AgreementType.DBC)
                {
                    //判断CAN1是否有选择数据
                    if (gridExportContent.GridDataDbcSelectRow.DbcCheckIndex.Count < 1)
                    {
                        MessageBox.Show("CAN1通道未选择导出数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        File.Delete(targetPath);
                        return;
                    }
                    if (gridExportContent.AgreementTypeCan2 == AgreementType.CCP || gridExportContent.AgreementTypeCan2 == AgreementType.XCP)
                    {
                        if (gridExportContent.GridDataA2lSelectRow.LimitTimeList10ms.Count < 1 && gridExportContent.GridDataA2lSelectRow.LimitTimeList100ms.Count < 1)
                        {
                            MessageBox.Show("CAN2通道未选择导出数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            File.Delete(targetPath);
                            return;
                        }
                        ExportDBCXCP(targetPath, gridExportContent);
                    }
                    else if (gridExportContent.AgreementTypeCan2 == AgreementType.DBC)
                    {
                        if (gridExportContent.GridDataDbcSelectRow.DbcCheckIndex.Count < 1)
                        {
                            MessageBox.Show("CAN2通道未选择导出数据", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            File.Delete(targetPath);
                            return;
                        }
                        //待验证
                        ExportDBC(targetPath, gridExportContent.GridViewCan1, gridExportContent.GridDataDbcSelectRow, gridExportContent.XcpDataCan1, gridExportContent.AnalysisDataCan1);
                        ExportDBC(targetPath, gridExportContent.GridViewCan2, gridExportContent.GridDataDbcSelectRow, gridExportContent.XcpDataCan2, gridExportContent.AnalysisDataCan2);
                    }
                }
            }
            else
            {
                //未选择CAN
                MessageBox.Show("没有可导出的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //导出完成后执行.bat
            var batName = ConfigurationManager.AppSettings["batName"].ToString();
            if (batName == "")
                batName = "xcpmake.bat";
            if (!File.Exists(excutePath + batName))
                return;
            return;
            var executeResult = true;//= Execute.ExecuteApply(excutepath, batName);
            if (executeResult)
            {
                if (MessageBox.Show("已成功生成DLL，是否打开DLL路径！", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    System.Diagnostics.Process.Start(excutePath);//仅打开文件目录
                    //System.Diagnostics.Process.Start("Explorer", "/select," + excutepath + "\\" + batName);//打开目录，定位文件
                }
            }
            else
            {
                MessageBox.Show("生成DLL失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private static void ExportDBC(string targetPath,RadGridView gridView, GridViewData gridData,
            XcpData xcpData,AnalysisData analysisData)
        {
            DbcDetailData(targetPath, gridView, gridData);
            AddDBCDetailGroup(targetPath);
            AddCanChInfo(targetPath, xcpData, analysisData, ProtocolType.DBC);
        }

        private static void ExportXCP(string targetPath,RadGridView gridView,GridViewData gridData,
            XcpData xcpData)
        {
            A2lDetailData(targetPath, gridView, gridData);
            AddA2lDetailGroup(gridData, targetPath, xcpData);
            AddCanChInfo(targetPath, xcpData, null, ProtocolType.XCP_CCP);
        }

        private static void ExportDBCXCP(string targetPath,GridExportContent gridExportContent)
        {
            if (gridExportContent.AgreementTypeCan1 == AgreementType.CCP || gridExportContent.AgreementTypeCan1 == AgreementType.XCP)
            {
                A2lDetailData(targetPath, gridExportContent.GridViewCan1, gridExportContent.GridDataA2lSelectRow);
                DbcDetailData(targetPath, gridExportContent.GridViewCan2, gridExportContent.GridDataDbcSelectRow);
                AddA2lDetailGroup(gridExportContent.GridDataA2lSelectRow, targetPath, gridExportContent.XcpDataCan1);
                AddDBCDetailGroup(targetPath);
                AddCanChInfo(targetPath, gridExportContent.XcpDataCan1, gridExportContent.AnalysisDataCan2, ProtocolType.ALL);
            }
            else if (gridExportContent.AgreementTypeCan1 == AgreementType.DBC)
            {
                A2lDetailData(targetPath, gridExportContent.GridViewCan2, gridExportContent.GridDataA2lSelectRow);
                DbcDetailData(targetPath, gridExportContent.GridViewCan1, gridExportContent.GridDataDbcSelectRow);
                AddA2lDetailGroup(gridExportContent.GridDataA2lSelectRow, targetPath, gridExportContent.XcpDataCan2);
                AddDBCDetailGroup(targetPath);
                AddCanChInfo(targetPath, gridExportContent.XcpDataCan2, gridExportContent.AnalysisDataCan1, ProtocolType.ALL);
            }
        }

        private static void A2lDetailData(string targetPath, RadGridView gridView, GridViewData listData)
        {
            try
            {
                lock (obj)
                {
                    StringBuilder _segmentStr = new StringBuilder();
                    StringBuilder _10msStr = new StringBuilder();
                    StringBuilder _100msStr = new StringBuilder();

                    //定义XCP时间分类限制数组名
                    _segmentHeadName = HEADER_NAME + FUN_TYPE + SEGMENT_NAME + _A2L_ARRAY_CHAR + _SPLIT_CHAR;
                    _10msHeadName = HEADER_NAME + FUN_TYPE + _10MS_NAME + _A2L_ARRAY_CHAR+ _SPLIT_CHAR;
                    _100msHeadName = HEADER_NAME + FUN_TYPE + _100MS_NAME + _A2L_ARRAY_CHAR+ _SPLIT_CHAR;

                    if (listData.LimitTimeListSegMent.Count > 0)
                    {
                        //添加头
                        _segmentStr.Append(_segmentHeadName);
                        //添加数据
                        AppendData(listData.LimitTimeListSegMent, _segmentStr, gridView);
                    }
                    if (listData.LimitTimeList10ms.Count > 0)
                    {
                        _10msStr.Append(_10msHeadName);
                        AppendData(listData.LimitTimeList10ms, _10msStr, gridView);
                    }
                    if (listData.LimitTimeList100ms.Count > 0)
                    {
                        _100msStr.Append(_100msHeadName);
                        AppendData(listData.LimitTimeList100ms, _100msStr, gridView);
                    }
                    //写入数据
                    if (_segmentStr.Length > 0)
                    {
                        WriteData.WriteString(_segmentStr, targetPath);
                    }
                    if (_10msStr.Length > 0)
                    {
                        WriteData.WriteString(_10msStr, targetPath);
                    }
                    if (_100msStr.Length > 0)
                    {
                        WriteData.WriteString(_100msStr, targetPath);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private static void AddA2lDetailGroup(GridViewData listData, string path, XcpData dataCan1)
        {
            StringBuilder sbExInfo = new StringBuilder();
            sbExInfo.Append(EXINFO_TYPE_HEAD);
            sbExInfo.AppendLine(EXINFO_FUN_NAME_CAN1 + EXINFO_TYPE_METHOLD_NAME);
            if (dataCan1.AgreeMentType == AgreementType.CCP)
            {
                sbExInfo.AppendLine($"\t\t{(int)ExInfoType.SLAVER_ID_TYPE},{dataCan1.IF_DATA_ASAP1B_CCP_DATA.CAN_MSG_ID_RECE},0,");
                sbExInfo.AppendLine($"\t\t{(int)ExInfoType.MASTER_ID_TYPE},{dataCan1.IF_DATA_ASAP1B_CCP_DATA.CAN_MSG_ID_SEND},0,");
                sbExInfo.AppendLine($"\t\t{(int)ExInfoType.DAQ100_ID_TYPE},{dataCan1.IF_DATA_ASAP1B_CCP_DATA.CCP_100MS_DATA.CAN_ID_FIXED},0,");
                sbExInfo.AppendLine($"\t\t{(int)ExInfoType.DAQ10_ID_TYPE},{dataCan1.IF_DATA_ASAP1B_CCP_DATA.CCP_10MS_DATA.CAN_ID_FIXED},0,");
                exInfoLen = 4;
                if (listData.LimitTimeListSegMent.Count > 0)
                {
                    sbExInfo.AppendLine("\t\t" + "2" + "," + SEGMENT_NAME + "," + listData.LimitTimeListSegMent.Count + ",");
                    exInfoLen += 1;
                }
                if (listData.LimitTimeList100ms.Count > 0)
                {
                    sbExInfo.AppendLine($"\t\t{(int)ExInfoType.DAQ100_TAB_TYPE},(uint32_t){_100MS_NAME},{listData.LimitTimeList100ms.Count},");
                    exInfoLen++;
                }
                if (listData.LimitTimeList10ms.Count > 0)
                {
                    sbExInfo.AppendLine($"\t\t{(int)ExInfoType.DAQ10_TAB_TYPE},(uint32_t){_10MS_NAME},{listData.LimitTimeList10ms.Count},");
                    exInfoLen++;
                }
                sbExInfo.AppendLine($"\t\t{(int)ExInfoType.CCP_ECUADDR_TYPE},{dataCan1.IF_DATA_ASAP1B_CCP_DATA.STATION_ADDRESS},0,");
                exInfoLen++;
                sbExInfo.AppendLine("};");
            }
            else if (dataCan1.AgreeMentType == AgreementType.XCP)
            {
                exInfoLen = 2;
                if (dataCan1.XcpOnCanData.CurrentSelectItem == dataCan1.XcpOnCanData.VehicleApplData.CanName)
                {
                    sbExInfo.AppendLine($"\t\t{(int)ExInfoType.SLAVER_ID_TYPE},{dataCan1.XcpOnCanData.VehicleApplData.SlaveID},0,");
                    sbExInfo.AppendLine($"\t\t{(int)ExInfoType.MASTER_ID_TYPE},{dataCan1.XcpOnCanData.VehicleApplData.MasterID},0,");
                } else if (dataCan1.XcpOnCanData.CurrentSelectItem == dataCan1.XcpOnCanData.VehicleApplRamData.CanName)
                {
                    sbExInfo.AppendLine($"\t\t{(int)ExInfoType.SLAVER_ID_TYPE},{dataCan1.XcpOnCanData.VehicleApplRamData.SlaveID},0,");
                    sbExInfo.AppendLine($"\t\t{(int)ExInfoType.MASTER_ID_TYPE},{dataCan1.XcpOnCanData.VehicleApplRamData.MasterID},0,");
                } else if (dataCan1.XcpOnCanData.CurrentSelectItem == dataCan1.XcpOnCanData.CalibrationLeData.CanName)
                {
                    sbExInfo.AppendLine($"\t\t{(int)ExInfoType.SLAVER_ID_TYPE},{dataCan1.XcpOnCanData.CalibrationLeData.SlaveID},0,");
                    sbExInfo.AppendLine($"\t\t{(int)ExInfoType.MASTER_ID_TYPE},{dataCan1.XcpOnCanData.CalibrationLeData.MasterID},0,");
                } else if (dataCan1.XcpOnCanData.CurrentSelectItem == dataCan1.XcpOnCanData.CalibrationLeRamData.CanName)
                {
                    sbExInfo.AppendLine($"\t\t{(int)ExInfoType.SLAVER_ID_TYPE},{dataCan1.XcpOnCanData.CalibrationLeRamData.SlaveID},0,");
                    sbExInfo.AppendLine($"\t\t{(int)ExInfoType.MASTER_ID_TYPE},{dataCan1.XcpOnCanData.CalibrationLeRamData.MasterID},0,");
                }
                //sbExInfo.AppendLine($"\t\t{(int)ExInfoType.DAQ100_ID_TYPE},{dataCan1.IF_DATA_ASAP1B_CCP_DATA.CCP_100MS_DATA.CAN_ID_FIXED},0,");
                //sbExInfo.AppendLine($"\t\t{(int)ExInfoType.DAQ10_ID_TYPE},{dataCan1.IF_DATA_ASAP1B_CCP_DATA.CCP_10MS_DATA.CAN_ID_FIXED},0,");
                if (listData.LimitTimeListSegMent.Count > 0)
                {
                    sbExInfo.AppendLine("\t\t" + "2" + "," + SEGMENT_NAME + "," + listData.LimitTimeListSegMent.Count + ",");
                }
                if (listData.LimitTimeList100ms.Count > 0)
                {
                    sbExInfo.AppendLine($"\t\t{(int)ExInfoType.DAQ100_TAB_TYPE},(uint32_t){_100MS_NAME},{listData.LimitTimeList100ms.Count},");
                    exInfoLen++;
                }
                if (listData.LimitTimeList10ms.Count > 0)
                {
                    sbExInfo.AppendLine($"\t\t{(int)ExInfoType.DAQ10_TAB_TYPE},(uint32_t){_10MS_NAME},{listData.LimitTimeList10ms.Count},");
                    exInfoLen++;
                }
                //sbExInfo.AppendLine($"\t\t{(int)ExInfoType.CCP_ECUADDR_TYPE},{dataCan1.IF_DATA_ASAP1B_CCP_DATA.STATION_ADDRESS},0,");
                sbExInfo.AppendLine("};");
            }
            WriteData.WriteString(sbExInfo,path);
        }

        private static void DbcDetailData(string targetPath, RadGridView gridView, GridViewData listData)
        {
            try
            {
                //遍历行数据
                StringBuilder dbcBuilder = new StringBuilder();
                acturalDBCList = new List<AnalysisSignal>();
                frameIdList = new List<string>();
                allDbcGroupData = new List<StringBuilder>();
                //遍历选择行数据
                List<AnalysisSignal> analysisDbcDataList = DbcDataToSignal(listData, gridView);

                //根据分组ID查询该ID对应所有数据行
                for (int i = 0; i < frameIdList.Count; i++)
                {
                    var resdbcList = acturalDBCList.FindAll(dbc => dbc.DataAddress == frameIdList[i]);
                    StringBuilder dbcGroupData = new StringBuilder();
                    dbcGroupData.Append(DBC_DETAIL_HEAD);
                    string metholdName = DBC_DEATIL_METHOLD_NAME + "_" + frameIdList[i];
                    dbcGroupData.AppendLine(metholdName+"[] = \r\n{ ");
                    //保存格式内容：名称+描述+单位+数据类型+数据长度+字节顺序+截取开始地址(dbc有用)+截取长度+数据地址(a2l-ecu地址，monitor-canid)+系数+偏移量
                    for (int j = 0; j < resdbcList.Count; j++)
                    {
                        dbcGroupData.Append("\t\t"+'"'+resdbcList[j].Name+'"'+",");
                        var dbcMsg = analysisDbcDataList.Find(dbc => dbc.DataAddress == resdbcList[j].DataAddress);
                        dbcGroupData.Append('"'+dbcMsg.Describle+'"'+",");
                        dbcGroupData.Append('"'+resdbcList[j].Unit+'"'+",");
                        dbcGroupData.Append(resdbcList[j].SaveDataType+",");
                        dbcGroupData.Append(resdbcList[j].SaveDataLen+",");
                        dbcGroupData.Append(resdbcList[j].IsMotorola+",");
                        dbcGroupData.Append(resdbcList[j].StartIndex+",");
                        dbcGroupData.Append(resdbcList[j].DataBitLen+",");
                        dbcGroupData.Append(resdbcList[j].DataAddress+",");
                        dbcGroupData.Append(resdbcList[j].Factor+",");
                        dbcGroupData.AppendLine(resdbcList[j].OffSet+",");
                    }
                    dbcGroupData.AppendLine("};");

                    LogHelper.Log.Info("DBC开始写入数据！");
                    allDbcGroupData.Add(dbcGroupData);
                    LogHelper.Log.Info("DBC写数据完成！");
                }

                //写数据
                WriteData.WriteString(allDbcGroupData,targetPath);
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.Message+ex.StackTrace);
            }
        }

        private static void AddDBCDetailGroup(string targPath)
        {
            StringBuilder sbExInfo = new StringBuilder();
            sbExInfo.Append(EXINFO_TYPE_HEAD);
            sbExInfo.AppendLine(EXINFO_FUN_NAME_CAN2 + EXINFO_TYPE_METHOLD_NAME);
            //sbExInfo.AppendLine("\t\t0" + "," + "0" + "," + "0" + ",");
            //sbExInfo.AppendLine("\t\t1" + "," + "0" + "," + "0" + ",");
            for (int i = 0; i < frameIdList.Count; i++)
            {
                var resdbcList = acturalDBCList.FindAll(dbc => dbc.DataAddress == frameIdList[i]);
                string metholdName = DBC_DEATIL_METHOLD_NAME + "_" + frameIdList[i];
                sbExInfo.AppendLine($"\t\t{(int)ExInfoType.MORNITOR_TAB_TYPE},(uint32_t){metholdName},{resdbcList.Count},");
            }
            sbExInfo.AppendLine("};");
            WriteData.WriteString(sbExInfo, targPath);
        }

        private static void AddFrameGroupID(AnalysisSignal dbcSignal)
        {
            if (!frameIdList.Contains(dbcSignal.DataAddress))
            {
                frameIdList.Add(dbcSignal.DataAddress);
            }
        }

        private static void AddCanChInfo(string targPath,XcpData xcpData, AnalysisData analysisData,ProtocolType protocolType)
        {
            string infoHead = "CanChInfo INFO[2] = \r\n{";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(infoHead);
            if (protocolType == ProtocolType.XCP_CCP)
            {
                if (xcpData.AgreeMentType == AgreementType.CCP)
                {
                    sb.AppendLine($"\t{(int)ExportProtocolType.CCP_OPEN},{xcpData.IF_DATA_ASAP1B_CCP_DATA.BAUDRATE},(uint32_t){EXINFO_FUN_NAME_CAN1},{exInfoLen},");
                }
                else if (xcpData.AgreeMentType == AgreementType.XCP)
                {
                    if (xcpData.XcpOnCanData.CurrentSelectItem == xcpData.XcpOnCanData.VehicleApplData.CanName)
                    {
                        sb.AppendLine($"\t{(int)ExportProtocolType.XCP_OPEN},{xcpData.XcpOnCanData.VehicleApplData.Baudrate},(uint32_t){EXINFO_FUN_NAME_CAN1},{exInfoLen},");
                    }
                    else if (xcpData.XcpOnCanData.CurrentSelectItem == xcpData.XcpOnCanData.VehicleApplRamData.CanName)
                    {
                        sb.AppendLine($"\t{(int)ExportProtocolType.XCP_OPEN},{xcpData.XcpOnCanData.VehicleApplRamData.Baudrate},(uint32_t){EXINFO_FUN_NAME_CAN1},{exInfoLen},");
                    }
                    else if (xcpData.XcpOnCanData.CurrentSelectItem == xcpData.XcpOnCanData.CalibrationLeData.CanName)
                    {
                        sb.AppendLine($"\t{(int)ExportProtocolType.XCP_OPEN},{xcpData.XcpOnCanData.CalibrationLeData.Baudrate},(uint32_t){EXINFO_FUN_NAME_CAN1},{exInfoLen},");
                    }
                    else if (xcpData.XcpOnCanData.CurrentSelectItem == xcpData.XcpOnCanData.CalibrationLeRamData.CanName)
                    {
                        sb.AppendLine($"\t{(int)ExportProtocolType.XCP_OPEN},{xcpData.XcpOnCanData.CalibrationLeRamData.Baudrate},(uint32_t){EXINFO_FUN_NAME_CAN1},{exInfoLen},");
                    }
                }
                sb.AppendLine($"\t{0},{0},{0},{0},");
            }
            else if (protocolType == ProtocolType.DBC)
            {
                sb.AppendLine($"\t{0},{0},{0},{0},");
                sb.AppendLine($"\t{(int)ExportProtocolType.CAN_MONITOR},{analysisData.BaudRateDbc},(uint32_t){EXINFO_FUN_NAME_CAN2},{frameIdList.Count},");
            }
            else if (protocolType == ProtocolType.ALL)
            {
                if (xcpData.AgreeMentType == AgreementType.CCP)
                {
                    sb.AppendLine($"\t{(int)ExportProtocolType.CCP_OPEN},{xcpData.IF_DATA_ASAP1B_CCP_DATA.BAUDRATE},(uint32_t){EXINFO_FUN_NAME_CAN1},{exInfoLen},");
                }
                else if (xcpData.AgreeMentType == AgreementType.XCP)
                {
                    if (xcpData.XcpOnCanData.CurrentSelectItem == xcpData.XcpOnCanData.VehicleApplData.CanName)
                    {
                        sb.AppendLine($"\t{(int)ExportProtocolType.XCP_OPEN},{xcpData.XcpOnCanData.VehicleApplData.Baudrate},(uint32_t){EXINFO_FUN_NAME_CAN1},{exInfoLen},");
                    }
                    else if (xcpData.XcpOnCanData.CurrentSelectItem == xcpData.XcpOnCanData.VehicleApplRamData.CanName)
                    {
                        sb.AppendLine($"\t{(int)ExportProtocolType.XCP_OPEN},{xcpData.XcpOnCanData.VehicleApplRamData.Baudrate},(uint32_t){EXINFO_FUN_NAME_CAN1},{exInfoLen},");
                    }
                    else if (xcpData.XcpOnCanData.CurrentSelectItem == xcpData.XcpOnCanData.CalibrationLeData.CanName)
                    {
                        sb.AppendLine($"\t{(int)ExportProtocolType.XCP_OPEN},{xcpData.XcpOnCanData.CalibrationLeData.Baudrate},(uint32_t){EXINFO_FUN_NAME_CAN1},{exInfoLen},");
                    }
                    else if (xcpData.XcpOnCanData.CurrentSelectItem == xcpData.XcpOnCanData.CalibrationLeRamData.CanName)
                    {
                        sb.AppendLine($"\t{(int)ExportProtocolType.XCP_OPEN},{xcpData.XcpOnCanData.CalibrationLeRamData.Baudrate},(uint32_t){EXINFO_FUN_NAME_CAN1},{exInfoLen},");
                    }
                }
                sb.AppendLine($"\t{(int)ExportProtocolType.CAN_MONITOR},{analysisData.BaudRateDbc},(uint32_t){EXINFO_FUN_NAME_CAN2},{frameIdList.Count},");
            }
            sb.AppendLine("};");
            WriteData.WriteString(sb, targPath);
        }

        private static void ProduceFile()
        {
            /*
             * 导出数组类型：
             * 一、导出数据内容：XCPDataRecordType，分类显示；DBC文件
             * 二、导出数据类型：ExInfoType
             * 三、RxidTab1
             * 四、CanChInfo
             */
            //计算长度
            //_segmentStr.Append(_lensegHeaderName + listData.LimitTimeListSegMent.Count * 7 + "\r\n");
            //计算长度
            //_10msStr.Append(_len10msHeaderName + listData.LimitTimeList10ms.Count * 7 + "\r\n");
            //计算长度
            //_100msStr.Append(_len100msHeaderName + listData.LimitTimeList100ms.Count * 7 + "\r\n");
        }
        #endregion

        #region 追加数据
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="list"></param>
        /// <param name="builder"></param>
        /// <param name="gridView"></param>
        private static void AppendData(List<int> list,StringBuilder builder,RadGridView gridView)
        {
            lock(objAppend)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    //数据格式：
                    //名称+描述+单位+数据类型+数据长度+字节顺序+截取开始地址(dbc有用)+
                    //截取长度+数据地址(a2l-ecu地址，monitor-canid)+系数+偏移量
                    var describle = gridView.Rows[list[i]].Cells[2].Value.ToString().Replace('"',' ').Trim();
                    if (describle.Length >= 12)
                    {
                        describle = describle.Substring(0, 12);
                        describle = "\"" + describle + "\"";
                    }
                    else
                    {
                        describle = "\"" + describle + "\"";
                    }
                    builder.Append("\t" + '"'+gridView.Rows[list[i]].Cells[1].Value.ToString() +'"'+ "," +
                                   describle + "," +//描述
                                   gridView.Rows[list[i]].Cells[3].Value.ToString() + "," +
                                   gridView.Rows[list[i]].Cells[4].Value.ToString() + "," +
                                   gridView.Rows[list[i]].Cells[5].Value.ToString() + "," +
                                   gridView.Rows[list[i]].Cells[6].Value.ToString() + "," +
                                   gridView.Rows[list[i]].Cells[7].Value.ToString() + "," +
                                   gridView.Rows[list[i]].Cells[8].Value.ToString() + "," +
                                   gridView.Rows[list[i]].Cells[9].Value.ToString() + "," +
                                   gridView.Rows[list[i]].Cells[10].Value.ToString() + "," +
                                   gridView.Rows[list[i]].Cells[11].Value.ToString() + ",\r\n");
                }
                builder.Append("};\r\n");
            }
        }

        private static List<AnalysisSignal> DbcDataToSignal(GridViewData listData,RadGridView gridView)
        {
            if (listData.DbcCheckIndex.Count < 1)
                return null;
            List<AnalysisSignal> analysisSignalList = new List<AnalysisSignal>();
            for (int i = 0; i < listData.DbcCheckIndex.Count; i++)
            {
                AnalysisSignal analysisSignal = new AnalysisSignal();
                analysisSignal.Name = gridView.Rows[listData.DbcCheckIndex[i]].Cells[1].Value.ToString();
                var describle = gridView.Rows[listData.DbcCheckIndex[i]].Cells[2].Value.ToString().Replace('"',' ').Trim();
                if (describle.Length >= 12)
                {
                    describle = describle.Substring(0, 12);
                    //describle = "\"" + describle + "\"";
                }

                analysisSignal.Describle = describle;
                analysisSignal.Unit = gridView.Rows[listData.DbcCheckIndex[i]].Cells[3].Value.ToString();
                analysisSignal.SaveDataType = (SaveDataTypeEnum)Enum.Parse(typeof(SaveDataTypeEnum),gridView.Rows[listData.DbcCheckIndex[i]].Cells[4].Value.ToString());
                analysisSignal.SaveDataLen = int.Parse(gridView.Rows[listData.DbcCheckIndex[i]].Cells[5].Value.ToString());
                analysisSignal.IsMotorola = int.Parse(gridView.Rows[listData.DbcCheckIndex[i]].Cells[6].Value.ToString());
                analysisSignal.StartIndex = int.Parse(gridView.Rows[listData.DbcCheckIndex[i]].Cells[7].Value.ToString());
                analysisSignal.DataBitLen = int.Parse(gridView.Rows[listData.DbcCheckIndex[i]].Cells[8].Value.ToString());
                analysisSignal.DataAddress =  gridView.Rows[listData.DbcCheckIndex[i]].Cells[9].Value.ToString();
                analysisSignal.Factor = gridView.Rows[listData.DbcCheckIndex[i]].Cells[10].Value.ToString();
                analysisSignal.OffSet = gridView.Rows[listData.DbcCheckIndex[i]].Cells[11].Value.ToString();

                analysisSignalList.Add(analysisSignal);
                AddFrameGroupID(analysisSignal);//添加分组ID
                acturalDBCList.Add(analysisSignal);//添加实际保存数据
            }
            return analysisSignalList;
        }
        #endregion

        #region 导出所有数据，未限制记录时间
        /// <summary>
        /// 导出所有数据
        /// </summary>
        /// <param name="_allContent"></param>
        /// <param name="xcpdata"></param>
        private static void ExportAll(StringBuilder _allContent,XcpData xcpdata)
        {
            var measureList = xcpdata.MeasureData;
            var metholdList = xcpdata.MetholdData;
            for (int i = 0; i < measureList.Count; i++)
            {
                _allContent.Append("\t" + measureList[i].Name + "," + measureList[i].Type + "," + measureList[i].ReferenceMethod + "," +
                    measureList[i].EcuAddress + ",");
                var res = metholdList.Find(str => str.name == measureList[i].ReferenceMethod);
                _allContent.Append(res.funType + "," + res.unit + "," + res.coeffsValue + "\r\n");
            }
        }
        #endregion

    }
}
