﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Data;
using CommonUtils.DB;
using CommonUtils.Logger;
using CommonUtils.CalculateAndString;
using System.Configuration;
using System.Collections;
using MesWcfService.MessageQueue.RemoteClient;
using MesWcfService.DB;
using MesWcfService.Model;
using System.Data.SqlClient;
using SwaggerWcf.Attributes;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace MesWcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [SwaggerWcf("/MesService")]
    public class MesService : IMesService
    {
        private Queue<string[]> fcQueue = new Queue<string[]>();
        private Queue<string[]> insertDataQueue = new Queue<string[]>();
        private Queue<string[]> selectDataQueue = new Queue<string[]>();
        private Queue<string[]> insertMaterialStatisticsQueue = new Queue<string[]>();
        private Queue<string[]> updateProgrameVersionQueue = new Queue<string[]>();
        private Queue<string[]> updateLimitConfigQueue = new Queue<string[]>();
        private Queue<string[]> updateLogDataQueue = new Queue<string[]>();
        private Queue<string[]> updatePackageProductQueue = new Queue<string[]>();
        private Queue<string[]> checkMaterialStateQueue = new Queue<string[]>();
        private Queue<string[]> checkMaterialMatchQueue = new Queue<string[]>();
        private Queue<string[]> bindingSnPcbaQueue = new Queue<string[]>();
        private Queue<string[]> selectPVersionQueue = new Queue<string[]>();
        private Queue<string[]> selectSpecLimitQueue = new Queue<string[]>();
        private int materialLength = 20;

        #region 测试通讯
        [SwaggerWcfTag("MesServcie 服务")]
        public string TestCommunication(string value)
        {
            //通讯正常返回原值
            //客户端与接口异常：收不到返回值
            //接口与数据库异常：
            var testRes = SQLServer.TestSQLConnectState();
            if (testRes != "")
            {
                return "【SQL Server】"+testRes;
            }
            return value;
        }
        #endregion

        #region 更新测试结果
        //返回值为：OK-成功，FAIL-失败，ERROR-异常错误
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("OK","插入成功")]
        [SwaggerWcfResponse("FAIL","插入失败")]
        [SwaggerWcfResponse("ERROR","异常错误")]
        [SwaggerWcfResponse(HttpStatusCode.Unused)]
        public string UpdateTestResultData(string sn, string typeNo, string station,string result,string teamLeader, string admin)
        {
            string[] array = new string[] { sn, typeNo, station,result,teamLeader,admin };
            insertDataQueue.Enqueue(array);
            return TestResult.InsertTestResult(insertDataQueue);
        }
        #endregion

        #region 查询测试结果
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("ERR_LAST_STATION","查询上一站位失败")]
        [SwaggerWcfResponse("ERR_LAST_STATION_KEY","查询上一站位ID失败")]
        [SwaggerWcfResponse("QUERY_NONE", "未查询到结果")]
        [SwaggerWcfResponse("ERR_EXCEPTION", "异常错误")]
        public string[] SelectLastTestResult(string sn,string station)
        {
            string[] array = new string[] { sn,station};
            selectDataQueue.Enqueue(array);
            return TestResult.SelectTestResult(selectDataQueue);
        }
        #endregion

        #region 更新物料统计表
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("0X00", "STATUS_FAIL")]
        [SwaggerWcfResponse("0X01", "STATUS_USCCESS")]
        [SwaggerWcfResponse("0X02", "ERROR_IS_NULL_TYPNO")]
        [SwaggerWcfResponse("0X03", "ERROR_IS_NULL_STATION_NAME")]
        [SwaggerWcfResponse("0X04", "ERROR_IS_NULL_MATERIAL_CODE")]
        [SwaggerWcfResponse("0X05", "ERROR_IS_NULL_AMOUNTED")]
        [SwaggerWcfResponse("0X06", "ERROR_USE_AMOUNT_NOT_INT")]
        [SwaggerWcfResponse("0X07", "ERROR_NOT_MATCH_MATERIAL_PN")]
        [SwaggerWcfResponse("0X08", "ERROR_NOT_AMOUNT_STATE")]
        public string UpdateMaterialStatistics(string typeNo,string stationName,string materialCode,string amounted,string teamLeader,string admin)
        {
            if (string.IsNullOrEmpty(typeNo))
                return MaterialStatistics.ConvertMaterialStatisticsCode(MaterialStatisticsReturnCode.ERROR_IS_NULL_TYPNO);
            if (string.IsNullOrEmpty(stationName))
                return MaterialStatistics.ConvertMaterialStatisticsCode(MaterialStatisticsReturnCode.ERROR_IS_NULL_STATION_NAME);
            if (string.IsNullOrEmpty(materialCode))
                return MaterialStatistics.ConvertMaterialStatisticsCode(MaterialStatisticsReturnCode.ERROR_IS_NULL_MATERIAL_CODE);
            if (!ExamineInputFormat.IsDecimal(amounted))
                return MaterialStatistics.ConvertMaterialStatisticsCode(MaterialStatisticsReturnCode.ERROR_USE_AMOUNT_NOT_INT);
            insertMaterialStatisticsQueue.Enqueue(new string[] { typeNo,stationName,materialCode,amounted,teamLeader,admin});
            return MaterialStatistics.UpdateMaterialStatistics(insertMaterialStatisticsQueue);
        }
        #endregion

        #region 物料数量防错
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("0X01", "STATUS_USING")]
        [SwaggerWcfResponse("0X02", "STATUS_COMPLETE_NORMAL")]
        [SwaggerWcfResponse("0X03", "STATUS_COMPLETE_UNUSUAL")]
        [SwaggerWcfResponse("0X04", "ERROR_NULL_PRODUCT_TYPENO")]
        [SwaggerWcfResponse("0X05", "ERROR_NULL_MATERIAL_CODE")]
        [SwaggerWcfResponse("0X06", "STATUS_NULL_QUERY")]
        [SwaggerWcfResponse("0X07", "ERROR_FORMAT_MATERIAL_CODE")]
        [SwaggerWcfResponse("0X08", "ERROR_MATRIAL_CODE_IS_NOT_MATCH_WITH_PRODUCT_TYPENO")]
        public string CheckMaterialUseState(string productTypeNo, string materialCode)
        {
            if (string.IsNullOrEmpty(materialCode))
                return MaterialStatistics.ConvertCheckMaterialStateCode(MaterialStateReturnCode.ERROR_NULL_MATERIAL_CODE);
            if (string.IsNullOrEmpty(productTypeNo))
                return MaterialStatistics.ConvertCheckMaterialStateCode(MaterialStateReturnCode.ERROR_NULL_PRODUCT_TYPENO);

            checkMaterialStateQueue.Enqueue(new string[] { productTypeNo,materialCode});
            return MaterialStatistics.CheckMaterialState(checkMaterialStateQueue);
        }
        #endregion

        #region 物料号防错
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("0X00", "IS_NOT_MATCH")]
        [SwaggerWcfResponse("0X01", "IS_MATCH")]
        [SwaggerWcfResponse("0X02", "ERROR_NULL_PRODUCT_TYPENO")]
        [SwaggerWcfResponse("0X03", "ERROR_NULL_MATERIAL_PN")]
        public string CheckMaterialMatch(string productTypeNo,string materialPN)
        {
            if (string.IsNullOrEmpty(productTypeNo))
                return MaterialStatistics.ConvertCheckMaterialMatch(MaterialCheckMatchReturnCode.ERROR_NULL_PRODUCT_TYPENO);
            if (string.IsNullOrEmpty(materialPN))
                return MaterialStatistics.ConvertCheckMaterialMatch(MaterialCheckMatchReturnCode.ERROR_NULL_MATERIAL_PN);
            checkMaterialMatchQueue.Enqueue(new string[] { productTypeNo,materialPN});
            return MaterialStatistics.CheckMaterialMatch(checkMaterialMatchQueue);
        }

        #endregion

        #region 成品打包接口/成品抽检-更新绑定信息
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("OK","更新数据成功")]
        [SwaggerWcfResponse("FAIL", "更新数据失败")]
        [SwaggerWcfResponse("FULL","箱子已装满")]
        [SwaggerWcfResponse("ERROR", "异常错误")]
        public string UpdatePackageProductBindingMsg(string outCaseCode,string snOutter,string typeNo,string stationName,
            string bindingState,string remark,string temaLeader,string admin)
        {
            string[] array = new string[] { outCaseCode,snOutter,typeNo,stationName,bindingState,remark,temaLeader,admin};
            updatePackageProductQueue.Enqueue(array);
            return UPackageProduct.UpdatePackageProduct(updatePackageProductQueue);
        }
        #endregion

        #region UpdateProgrameVersion 更新程序版本
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("OK", "更新测试程序版本成功")]
        [SwaggerWcfResponse("FAIL", "更新测试程序版本失败")]
        [SwaggerWcfResponse("ERROR", "异常错误")]
        public string UpdateProgrameVersion(string typeNo,string stationName,string programeName,
            string programeVersion,string teamLeader,string admin)
        {
            string[] array = new string[] { typeNo, stationName, programeName, programeVersion, teamLeader, admin };
            updateProgrameVersionQueue.Enqueue(array);
            return ProgrameVersion.UpdateProgrameVersion(updateProgrameVersionQueue);
        }
        #endregion

        #region 更新LIMIT配置
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("OK", "更新LIMIT配置成功")]
        [SwaggerWcfResponse("FAIL", "更新LIMIT配置失败")]
        [SwaggerWcfResponse("ERROR", "异常错误")]
        public string UpdateLimitConfig(string stationName,string typeNo,string testItem,string limit,string teamLeader,string admin)
        {
            string[] array = new string[] { stationName,typeNo,testItem,limit,teamLeader,admin};
            updateLimitConfigQueue.Enqueue(array);
            return LimitConfig.UpdateLimitConfig(updateLimitConfigQueue);
        }
        #endregion

        #region 更新测试log数据
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("OK", "更新LIMIT配置成功")]
        [SwaggerWcfResponse("FAIL", "更新LIMIT配置失败")]
        [SwaggerWcfResponse("ERROR", "异常错误")]
        public string UpdateTestLog(string typeNo, string stationName,string productSN, 
            string testItem,string limit,string currentValue,string testResult, string teamLeader, string admin)
        {
            string[] array = new string[] {typeNo,stationName,productSN,testItem,limit,currentValue,testResult,teamLeader,admin};
            updateLogDataQueue.Enqueue(array);
            return TestLogData.UpdateTestLogData(updateLogDataQueue);
        }
        #endregion

        #region 查询当前工艺流程
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("NULL","查询结果为空！")]
        public string SelectCurrentTProcess()
        {
            string selectSQL = $"SELECT DISTINCT {DbTable.F_TECHNOLOGICAL_PROCESS.PROCESS_NAME} " +
                $"FROM {DbTable.F_TECHNOLOGICAL_PROCESS_NAME} WHERE {DbTable.F_TECHNOLOGICAL_PROCESS.PSTATE} = '1'";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
                return dt.Rows[0][0].ToString();
            else
                return "NULL";
        }
        #endregion

        #region 查询所有工艺流程
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("NULL", "查询结果为空！")]
        public string[] SelectAllTProcess()
        {
            string selectSQL = $"SELECT DISTINCT {DbTable.F_TECHNOLOGICAL_PROCESS.PROCESS_NAME} FROM " +
                $"{DbTable.F_TECHNOLOGICAL_PROCESS_NAME}";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string[] arrayRes = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    arrayRes[i] = dt.Rows[i][0].ToString();
                }
                return arrayRes;
            }
            return new string[] { "NULL"};
        }
        #endregion

        #region 查询所有工序列表
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("NULL", "查询结果为空！请检查传入参数是否正确")]
        public string[] SelectStationList(string processName)
        {
            var selectSQL = $"SELECT {DbTable.F_TECHNOLOGICAL_PROCESS.STATION_NAME} FROM " +
                $"{DbTable.F_TECHNOLOGICAL_PROCESS_NAME} WHERE " +
                $"{DbTable.F_TECHNOLOGICAL_PROCESS.PROCESS_NAME} = '{processName}'";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string[] arrayRes = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    arrayRes[i] = dt.Rows[i][0].ToString();
                }
                return arrayRes;
            }
            return new string[] { "NULL"};
        }
        #endregion

        #region 查询所有产品型号
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("NULL", "查询结果为空！请检查传入参数是否正确")]
        public string[] SelectTypeNoList()
        {
            var selectSQL = $"SELECT {DbTable.F_Out_Case_Storage.TYPE_NO} FROM {DbTable.F_OUT_CASE_STORAGE_NAME} ";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string[] array = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    array[i] = dt.Rows[i][0].ToString();
                }
                return array;
            }
            return new string[] { "NULL"};
        }
        #endregion

        #region 查询所有物料
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("NULL", "查询结果为空！请检查传入参数是否正确")]
        public string[] SelectMaterialList(string productTypeNo)
        {
            var selectSQL = $"SELECT {DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE} FROM {DbTable.F_PRODUCT_MATERIAL_NAME} WHERE " +
                $"{DbTable.F_PRODUCT_MATERIAL.TYPE_NO} = '{productTypeNo}'";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string[] array = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    array[i] = dt.Rows[i][0].ToString();
                }
                return array;
            }
            return new string[] { "NULL" };
        }
        #endregion

        #region PCBA与外壳的绑定
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("OK", "外壳与PCBA绑定成功")]
        [SwaggerWcfResponse("FAIL", "外壳与PCBA绑定失败")]
        public string BindingPCBA(string snPCBA,string snOutter)
        {
            if (snPCBA == "")
                return "snPCBA is not null";
            if (snOutter == "")
                return "snOutter is not null";
            bindingSnPcbaQueue.Enqueue(new string[] { snPCBA,snOutter});
            return AddBindingPCBA.BindingPCBA(bindingSnPcbaQueue);
        }
        #endregion

        #region 查询LIMIT
        [SwaggerWcfTag("MesServcie 服务")]
        public string[] SelectLimitConfig(string productTypeNo, string stationName,string item)
        {
            selectSpecLimitQueue.Enqueue(new string[] { productTypeNo,stationName,item });
            return SelectLastTestConfig.SelectSpecLimit(selectSpecLimitQueue);
        }
        #endregion

        #region 查询程序版本
        [SwaggerWcfTag("MesServcie 服务")]
        public string[] SelectProgrameVersion(string productTypeNo, string stationName)
        {
            selectPVersionQueue.Enqueue(new string[] { productTypeNo, stationName});
            return SelectLastTestConfig.SelectPVersion(selectPVersionQueue);
        }
        #endregion
    }
}
