using System;
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
        private Queue<string[]> insertMaterialStatistics = new Queue<string[]>();
        private Queue<string[]> updateProgrameVersionQueue = new Queue<string[]>();
        private Queue<string[]> updateLimitConfigQueue = new Queue<string[]>();
        private Queue<string[]> updatePackageProduct = new Queue<string[]>();

        #region 测试通讯
        [SwaggerWcfTag("MesServcie 服务")]
        public string TestCommunication(string value)
        {
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

        #region 物料统计表
        [SwaggerWcfTag("MesServcie 服务")]
        /// <summary>
        /// 测试端传入装配消耗物料计数
        /// </summary>
        [SwaggerWcfResponse("OK","插入数据成功")]
        [SwaggerWcfResponse("FAIL","插入数据失败")]
        [SwaggerWcfResponse("ERR_NOT_DECIMAL", "数量数据类型不为整型")]
        [SwaggerWcfResponse("ERROR", "异常错误")]
        public string UpdateMaterialStatistics(string snInner, string snOutter, string typeNo, 
            string stationName, string materialCode, int amount,string teamLeader,string admin)
        {
            if (!ExamineInputFormat.IsDecimal(amount.ToString()))
                return "ERR_NOT_DECIMAL";
            string[] array = new string[] { snInner, snOutter, typeNo, stationName, materialCode, amount+"",teamLeader,admin};
            insertMaterialStatistics.Enqueue(array);
            return MaterialStatistics.InsertMaterialStatistics(insertMaterialStatistics);
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
            updatePackageProduct.Enqueue(array);
            return UPackageProduct.UpdatePackageProduct(updatePackageProduct);
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
        public string UpdateLimitConfig(string stationName,string typeNo,string limitValue,string teamLeader,string admin)
        {
            string[] array = new string[] { stationName,typeNo,limitValue,teamLeader,admin};
            updateLimitConfigQueue.Enqueue(array);
            return LimitConfig.UpdateLimitConfig(updateLimitConfigQueue);
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
            var selectSQL = $"SELECT {DbTable.F_PRODUCT_TYPE_NO.TYPE_NO} FROM {DbTable.F_PRODUCT_TYPE_NO_NAME} ";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                string[] array = new string[dt.Rows.Count];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    array[i] = dt.Rows[0][0].ToString();
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
                    array[i] = dt.Rows[0][0].ToString();
                }
                return array;
            }
            return new string[] { "NULL" };
        }
        #endregion
    }
}
