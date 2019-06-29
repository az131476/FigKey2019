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
using AnalysisAgreeMent.Model;
using AnalysisAgreeMent.Model.DBC;
using AnalysisAgreeMent.Analysis;

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
        private static List<long> frameIdList;//用于存储DBC明细时的分组ID
        private const string DBC_DETAIL_HEAD = "static XCPDataRecordType ";
        private const string DBC_DEATIL_METHOLD_NAME = "DBCTab";

        private const string EXINFO_TYPE_HEAD = "static ExInfoType ";
        private const string EXINFO_Head = "ExInfo";
        private const string EXINFO_TYPE_METHOLD_NAME = "[] = \r\n{";

        private static List<StringBuilder> allDbcGroupData;
        private static List<AnalysisSignal> acturalDBCList;

        private static int exInfoLen;
        #endregion
        #endregion

        #region 导出数据
        /// <summary>
        /// 导出文件到本地
        /// </summary>
        public static void ExportFileToLocal(string targetPath, RadGridView gridView, GridViewData gridData, AnalysisData analysisData, FileType fileType)
        {
            if (fileType == FileType.A2L)
            {
                A2lDetailData(targetPath, gridView, gridData);
                AddA2lDetailGroup(gridData,targetPath);
                AddA2lRxidTab(targetPath);
                AddA2lCanChInfo(targetPath);
            }
            else if (fileType == FileType.DBC)
            {
                DbcDetailData(targetPath, gridView, analysisData);
                AddDBCDetailGroup(targetPath);
                AddDBCRxidTab(targetPath);
                AddDBCCanChInfo(targetPath);
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

        private static void AddA2lDetailGroup(GridViewData listData, string path)
        {
            StringBuilder sbExInfo = new StringBuilder();
            sbExInfo.Append(EXINFO_TYPE_HEAD);
            sbExInfo.AppendLine(EXINFO_Head+EXINFO_TYPE_METHOLD_NAME);
            sbExInfo.AppendLine("\t\t" + "0" + "," + "0x7f1" + "," + "0"+",");
            sbExInfo.AppendLine("\t\t" + "1" + "," + "0x7f2" + "," + "0"+",");
            exInfoLen = 2;
            if (listData.LimitTimeListSegMent.Count > 0)
            {
                sbExInfo.AppendLine("\t\t" + "2" + "," + SEGMENT_NAME + "," + listData.LimitTimeListSegMent.Count+",");
                exInfoLen += 1;
            }
            if (listData.LimitTimeList10ms.Count > 0)
            {
                sbExInfo.AppendLine($"\t\t2,{_10MS_NAME},{listData.LimitTimeList10ms.Count},");
                exInfoLen += 1;
            }
            if (listData.LimitTimeList100ms.Count > 0)
            {
                sbExInfo.AppendLine($"\t\t2,{_100MS_NAME},{listData.LimitTimeList100ms.Count},");
                exInfoLen += 1;
            }
            sbExInfo.AppendLine("};");
            WriteData.WriteString(sbExInfo,path);
        }

        private static void DbcDetailData(string targetPath, RadGridView gridView,AnalysisData dbcData)
        {
            try
            {
                //遍历行数据
                acturalDBCList = new List<AnalysisSignal>();
                frameIdList = new List<long>();
                allDbcGroupData = new List<StringBuilder>();
                //遍历选择行数据
                for (int i = 0; i < gridView.Rows.Count; i++)
                {
                    string signalName = gridView.Rows[i].Cells[1].Value.ToString();
                    AnalysisSignal dbcSignal = dbcData.AnalysisDbcDataList.Find(dbc => dbc.Name == signalName);
                    AddFrameGroupID(dbcSignal);//添加分组ID
                    acturalDBCList.Add(dbcSignal);//添加实际保存数据
                }
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
                        var dbcMsg = dbcData.AnalysisDbcDataList.Find(dbc => dbc.DataAddress == resdbcList[j].DataAddress);
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
            sbExInfo.AppendLine(EXINFO_Head+EXINFO_TYPE_METHOLD_NAME);
            //sbExInfo.AppendLine("\t\t0" + "," + "0" + "," + "0" + ",");
            //sbExInfo.AppendLine("\t\t1" + "," + "0" + "," + "0" + ",");
            for (int i = 0; i < frameIdList.Count; i++)
            {
                var resdbcList = acturalDBCList.FindAll(dbc => dbc.DataAddress == frameIdList[i]);
                string metholdName = DBC_DEATIL_METHOLD_NAME + "_" + frameIdList[i];
                sbExInfo.AppendLine("\t\t2"+","+metholdName+","+resdbcList.Count+",");
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

        //static   uint32_t RxidTab1[]={(uint32_t)(&ExInfo[0]),(uint32_t)(&ExInfo[1]),(uint32_t)(&ExInfo[2])};

        private static void AddDBCRxidTab(string path)
        {
            string rxidHead = "static uint32_t RxidTab[] = {";
            StringBuilder sb = new StringBuilder();
            sb.Append(rxidHead);
            for (int i = 0; i < frameIdList.Count; i++)
            {
                if (i < frameIdList.Count - 1)
                {
                    sb.Append($"(uint32_t)(&{EXINFO_Head}[{i}]),");
                }
                else
                {
                    sb.Append($"(uint32_t)(&{EXINFO_Head}[{i}])");
                }
            }
            sb.AppendLine("};");
            WriteData.WriteString(sb,path);
        }

        private static void AddA2lRxidTab(string path)
        {
            string rxidHead = "static uint32_t RxidTab[] = {";
            StringBuilder sb = new StringBuilder();
            sb.Append(rxidHead);
            for (int i = 0; i < exInfoLen; i++)
            {
                if (i < exInfoLen - 1)
                {
                    sb.Append($"(uint32_t)(&{EXINFO_Head}[{i}]),");
                }
                else
                {
                    sb.Append($"(uint32_t)(&{EXINFO_Head}[{i}])");
                }
            }
            sb.AppendLine("};");
            WriteData.WriteString(sb, path);
        }
        private static void AddDBCCanChInfo(string path)
        {
            string infoHead = "\r\nCanChInfo INFO[1] = \r\n{";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(infoHead);
            sb.AppendLine($"\t3,500000,(uint32_t)RxidTab,{frameIdList.Count}");
            sb.AppendLine("};");
            WriteData.WriteString(sb,path);
        }

        private static void AddA2lCanChInfo(string targPath)
        {
            string infoHead = "CanChInfo INFO[1] = \r\n{";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(infoHead);
            sb.AppendLine($"\t2,500000,(uint32_t)RxidTab,{exInfoLen}");
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
                    builder.Append("\t" + '"'+gridView.Rows[list[i]].Cells[1].Value.ToString() +'"'+ "," +
                                   gridView.Rows[list[i]].Cells[2].Value.ToString() + "," +
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
