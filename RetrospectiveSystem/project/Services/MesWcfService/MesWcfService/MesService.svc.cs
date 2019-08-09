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
        public string[] SelectLastTestResult(string sn, string typeNo, string station,string teamLeader,string admin)
        {
            string[] array = new string[] { sn, typeNo, station ,teamLeader,admin};
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
        public string UpdateMaterialStatistics(string snInner, string snOutter, string typeNo, string stationName, string materialCode, int amount)
        {
            if (!ExamineInputFormat.IsDecimal(amount.ToString()))
                return "ERR_NOT_DECIMAL";
            string[] array = new string[] { snInner, snOutter, typeNo, stationName, materialCode, amount+"" };
            insertMaterialStatistics.Enqueue(array);
            return MaterialStatistics.InsertMaterialStatistics(insertMaterialStatistics);
        }

        #endregion

        #region 成品打包接口/成品抽检-更新解绑信息
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse("OK","更新抽检信息成功")]
        [SwaggerWcfResponse("FAIL", "更新抽检信息失败")]
        [SwaggerWcfResponse("ERROR", "异常错误")]
        public string UpdatePackageProduct(string outCaseCode,string snOutter,string bindingState)
        {
            string updateSQL = $"UPDATE {DbTable.F_OUT_CASE_PRODUCT_NAME} SET " +
                $"{DbTable.F_Out_Case_Product.BINDING_STATE} = '{bindingState}' " +
                $"WHERE {DbTable.F_Out_Case_Product.OUT_CASE_CODE} = '{outCaseCode}' AND " +
                $"{DbTable.F_Out_Case_Product.SN_OUTTER} = '{snOutter}' ";
            LogHelper.Log.Info($"UpdatePackageProduct={updateSQL}");
            try
            {
                int upt = SQLServer.ExecuteNonQuery(updateSQL);
                if (upt > 0)
                    return "OK";
                return "FAIL";
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message);
                return "ERROR";
            }
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
    }
}
