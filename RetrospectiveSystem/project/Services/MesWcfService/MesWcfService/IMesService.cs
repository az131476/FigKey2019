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
using MesWcfService.Model;

namespace MesWcfService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IMesService
    {
        // TODO: 在此添加您的服务操作
        //测试数据接口

        [OperationContract]
        [SwaggerWcfPath("InsertTestResultData", "传入相关参数，插入测试数据;")]
        [WebInvoke(Method = "GET", UriTemplate = "InsertTestResultData?sn={sn}&typeNO={typeNo}&station={station}&dateTime={dateTime}&result={result}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string InsertTestResultData([SwaggerWcfParameter(Description = "追溯码")]string sn, 
            [SwaggerWcfParameter(Description = "产品型号")]string typeNo, 
            [SwaggerWcfParameter(Description = "站位名称")]string station, 
            [SwaggerWcfParameter(Description = "测试日期，日期格式：yyyy-MM-dd HH:mm:ss")]string dateTime, 
            [SwaggerWcfParameter(Description = "测试结果，PASS/FAIL")]string result);

        [OperationContract]
        [SwaggerWcfPath("SelectLastTestResult", "查询上一站位最新记录;测试结果：【成功】返回数组,len = 4;array[0] = sn;array[1] = typeNo;array[2] = station;array[3] = testRes;")]
        [WebInvoke(Method = "GET", UriTemplate = "SelectLastTestResult?sn={sn}&typeNo={typeNo}&station={station}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json),Description]
        string[] SelectLastTestResult([SwaggerWcfParameter(Description = "追溯码")]string sn, 
            [SwaggerWcfParameter(Description = "产品型号")]string typeNo, 
            [SwaggerWcfParameter(Description = "站位名称")]string station);

        //物料统计
        [OperationContract]
        [SwaggerWcfPath("InsertMaterialStatistics", "查询上一站位最新记录")]
        [WebInvoke(Method = "GET", UriTemplate = "InsertMaterialStatistics?snInner={snInner}&snOutter={snOutter}&typeNo={typeNo}&stationName={stationName}&materialCode={materialCode}&amount={amount}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string InsertMaterialStatistics([SwaggerWcfParameter(Description = "追溯码(内壳)")]string snInner, 
            [SwaggerWcfParameter(Description = "追溯码(外壳)")]string snOutter, 
            [SwaggerWcfParameter(Description = "产品型号")]string typeNo, 
            [SwaggerWcfParameter(Description = "站位名称")]string stationName,
            [SwaggerWcfParameter(Description = "物料编码")]string materialCode, 
            [SwaggerWcfParameter(Description = "物料数量(使用数量)")]int amount);

        [OperationContract]
        [SwaggerWcfPath("UpdatePackageProduct", "成品抽检时数据更新（解除绑定）")]
        [WebInvoke(Method = "GET", UriTemplate = "UpdatePackageProduct?outCaseCode={outCaseCode}&snOutter={snOutter}&bindingState={bindingState}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string UpdatePackageProduct([SwaggerWcfParameter(Description = "箱子编码")]string outCaseCode, 
            [SwaggerWcfParameter(Description = "追溯码")]string snOutter, 
            [SwaggerWcfParameter(Description = "是否解除绑定,0-解除绑定,1-重新绑定")]string bindingState);
    }

    // 使用下面示例中说明的数据约定将复合类型添加到服务操作。
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }
    }
}
