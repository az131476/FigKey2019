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
using MesAPI.Model;

namespace MesAPI
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IMesService
    {
        //用户信息
        [OperationContract]
        LoginResult Login(string username, string password, LoginUser loginUser);

        [OperationContract]
        QueryResult GetUserInfo(string userName, out DataSet dataSet);

        [OperationContract]
        DataSet GetAllUserInfo();

        [OperationContract]
        RegisterResult Register(string username, string pwd, string phone, string email, LoginUser loginUser);

        /// <returns></returns>
        [OperationContract]
        string FirstCheck(string snInner, string snOutter, string sTypeNumber, string sStationName);

        // [OperationContract]
        //string InsertWIP(string sn, string sTypeNumber, string sStationName, string sTestResult, string sTime);

        //站位信息
        [OperationContract]
        int DeleteStation(string stationName);

        [OperationContract]
        DataSet SelectStation(string stationName, string stationOrder);

        [OperationContract]
        int DeleteAllStation();

        [OperationContract]
        int InsertStation(List<Station> stationList);

        //产品型号
        [OperationContract]
        int DeleteProductTypeNo(string typeNo);

        [OperationContract]
        int DeleteAllProductTypeNo();

        [OperationContract]
        DataSet SelectProductTypeNo(string typeNo);

        [OperationContract]
        string CommitProductTypeNo(List<string> list);

        //站位接口
        [OperationContract]
        int DeleteAllTypeStation();

        [OperationContract]
        int DeleteTypeStation(string typeNumber);

        [OperationContract]
        DataSet SelectTypeStation(string typeNumber);

        [OperationContract]
        string CommitTypeStation(Dictionary<string, string[]> dctData);

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
        [OperationContract]
        DataSet SelectLastTestResultUpper(string sn, string typeNo, string station);

        [OperationContract]
        DataSet SelectTestResultUpper(string sn, string typeNo, string station, bool IsSnFuzzy);

        //物料信息接口
        [OperationContract]
        DataSet SelectMaterial();
        [OperationContract]
        string CommitMaterial(List<MaterialMsg> list);

        [OperationContract]
        int DeleteMaterial(string materialCode);

        [OperationContract]
        int DeleteAllMaterial();

        [OperationContract]
        int CommitProductMaterial(List<ProductMaterial> pmList);
        [OperationContract]
        DataSet SelectProductMaterial(ProductMaterial material);

        [OperationContract]
        int DeleteProductMaterial(ProductMaterial material);

        //物料统计
        [OperationContract]
        [SwaggerWcfPath("InsertMaterialStatistics", "查询上一站位最新记录")]
        [WebInvoke(Method = "GET", UriTemplate = "InsertMaterialStatistics?snInner={snInner}&snOutter={snOutter}&typeNo={typeNo}&materialCode={materialCode}&amount={amount}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string InsertMaterialStatistics(string snInner, string snOutter, string typeNo, string stationName,
            string materialCode, string amount);

        [OperationContract]
        DataSet SelectMaterialStatistics(string typeNo);

        [OperationContract]
        DataSet SelectMaterialMsg(MaterialMsg materialMsg, bool IsSelectAll);

        //外箱容量
        [OperationContract]
        int CommitOutCaseBoxStorage(string out_case_code, string amount);
        [OperationContract]
        DataSet SelectOutCaseBoxStorage(string caseCode);

        //成品打包
        [OperationContract]
        [SwaggerWcfPath("CommitPackageProduct", "产品绑定到箱子")]
        [WebInvoke(Method = "GET", UriTemplate = "CommitPackageProduct",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int CommitPackageProduct(List<PackageProduct> packageProductList);

        [OperationContract]
        [SwaggerWcfPath("UpdatePackageProduct", "成品抽检时数据更新（解除绑定）")]
        [WebInvoke(Method = "GET", UriTemplate = "UpdatePackageProduct",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        int UpdatePackageProduct(PackageProduct packageProduct);
        [OperationContract]
        DataSet SelectProductBindingState(string sn);

        [OperationContract]
        DataSet SelectProductBindingCount(string casecode, string bindingState);

        [OperationContract]
        int DeleteProductBindingData(string casecode);

        [OperationContract]
        DataSet SelectPackageProduct(PackageProduct packageProduct);

        [OperationContract]
        int DeletePackageProduct(PackageProduct packageProduct);
    }
}
