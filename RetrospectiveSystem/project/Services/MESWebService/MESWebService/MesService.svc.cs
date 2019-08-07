using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Data;
using CommonUtils.DB;
using CommonUtils.Logger;
using System.Configuration;
using System.Collections;
using MESWebService.MessageQueue.RemoteClient;
using MESWebService.DB;
using MESWebService.Model;
using System.Data.SqlClient;
using SwaggerWcf.Attributes;

namespace MESWebService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“MesService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 MesService.svc 或 MesService.svc.cs，然后开始调试。
    public class MesService : IMesService
    {
        private Queue<string[]> fcQueue = new Queue<string[]>();
        private Queue<string[]> insertDataQueue = new Queue<string[]>();
        private Queue<string[]> selectDataQueue = new Queue<string[]>();
        private Queue<string[]> insertMaterialStatistics = new Queue<string[]>();

        #region 测试结果数据接口
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse(HttpStatusCode.Created, "Book created, value in the response body with id updated")]
        [SwaggerWcfResponse(HttpStatusCode.BadRequest, "Bad request", true)]
        [SwaggerWcfResponse(HttpStatusCode.InternalServerError, "Internal error (can be forced using ERROR_500 as book title)", true)]
        /// <summary>
        /// 测试端插入测试结果
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="typeNo"></param>
        /// <param name="station"></param>
        /// <param name="dateTime"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string InsertTestResultData(string sn, string typeNo, string station, string dateTime, string result)
        {
            string[] array = new string[] { sn, typeNo, station, dateTime, result };
            insertDataQueue.Enqueue(array);
            return TestResult.InsertTestResult(insertDataQueue);
        }

        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse(HttpStatusCode.Created, "Book created, value in the response body with id updated")]
        [SwaggerWcfResponse(HttpStatusCode.BadRequest, "Bad request", true)]
        [SwaggerWcfResponse(HttpStatusCode.InternalServerError, "Internal error (can be forced using ERROR_500 as book title)", true)]
        /// <summary>
        /// 测试端查询上一站位的最后一条记录
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="typeNo"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        public string SelectLastTestResult(string sn, string typeNo, string station)
        {
            string[] array = new string[] { sn, typeNo, station };
            selectDataQueue.Enqueue(array);
            return TestResult.SelectTestResult(selectDataQueue);
        }
        #endregion

        #region 物料统计表
        [SwaggerWcfTag("MesServcie 服务")]
        /// <summary>
        /// 测试端传入装配消耗物料计数
        /// </summary>
        /// <param name="snInner"></param>
        /// <param name="snOutter"></param>
        /// <param name="typeNo"></param>
        /// <param name="stationName"></param>
        /// <param name="materialCode"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public string InsertMaterialStatistics(string snInner, string snOutter, string typeNo, string stationName, string materialCode, string amount)
        {
            string[] array = new string[] { snInner, snOutter, typeNo, stationName, materialCode, amount };
            insertMaterialStatistics.Enqueue(array);
            return MaterialStatistics.InsertMaterialStatistics(insertMaterialStatistics);
        }

        #endregion

        #region 成品打包接口/成品抽检-更新解绑信息
        public int UpdatePackageProduct(PackageProduct packageProduct)
        {
            string updateSQL = $"UPDATE {DbTable.F_OUT_CASE_PRODUCT_NAME} SET " +
                $"{DbTable.F_Out_Case_Product.BINDING_STATE} = '{packageProduct.BindingState}' " +
                $"WHERE {DbTable.F_Out_Case_Product.OUT_CASE_CODE} = '{packageProduct.CaseCode}' AND " +
                $"{DbTable.F_Out_Case_Product.SN_OUTTER} = '{packageProduct.SnOutter}' ";
            LogHelper.LogInfo($"UpdatePackageProduct={updateSQL}");
            return SQLServer.ExecuteNonQuery(updateSQL);
        }
        #endregion
    }
}
