using SwaggerWcf.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using MESWebService.Model;

namespace MESWebService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IMesService”。
    [ServiceContract]
    public interface IMesService
    {
        //测试数据接口
        [OperationContract]
        [SwaggerWcfPath("InsertTestResultData", "传入相关参数，插入测试数据")]
        [WebInvoke(Method = "GET", UriTemplate = "InsertTestResultData?sn={sn}&typeNO={typeNo}&station={station}&dateTime={dateTime}&result={result}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string InsertTestResultData(string sn, string typeNo, string station, string dateTime, string result);

        [OperationContract]
        [SwaggerWcfPath("SelectLastTestResult", "查询上一站位最新记录")]
        [WebInvoke(Method = "GET", UriTemplate = "SelectLastTestResult?sn={sn}&typeNo={typeNo}&station={station}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string SelectLastTestResult(string sn, string typeNo, string station);

        ////物料统计
        [OperationContract]
        [SwaggerWcfPath("InsertMaterialStatistics", "查询上一站位最新记录")]
        [WebInvoke(Method = "GET", UriTemplate = "InsertMaterialStatistics?snInner={snInner}&snOutter={snOutter}&typeNo={typeNo}&materialCode={materialCode}&amount={amount}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string InsertMaterialStatistics(string snInner, string snOutter, string typeNo, string stationName,
            string materialCode, string amount);

        [OperationContract]
        [SwaggerWcfPath("UpdatePackageProduct", "成品抽检时数据更新（解除绑定）")]
        [WebInvoke(Method = "GET", UriTemplate = "UpdatePackageProduct",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int UpdatePackageProduct(PackageProduct packageProduct);
    }
}
