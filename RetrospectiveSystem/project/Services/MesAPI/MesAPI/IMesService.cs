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

        //站位信息
        [OperationContract]
        int DeleteStation(string processName, string stationName);

        [OperationContract]
        DataSet SelectStationList(string processName);

        [OperationContract]
        DataSet SelectProcessList();

        [OperationContract]
        int DeleteAllStation(string processName);

        [OperationContract]
        int InsertStation(List<Station> stationList);

        [OperationContract]
        int SetCurrentProcess(string processName, int state);

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

       //测试数据
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
        int CommitProductContinairCapacity(string productTypeNo, string amount);
        [OperationContract]
        DataSet SelectProductContinairCapacity(string productTypeNo);
        
        [OperationContract]
        DataSet SelectProductBindingState(string sn);

        #region 【接口】查询已绑定数据
        [OperationContract]
        [SwaggerWcfPath("SelectProductBindingCount", "查询打包产品记录")]
        [WebInvoke(Method = "GET", UriTemplate = "SelectProductBindingCount&casecode={casecode}&bindingState={bindingState}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        DataSet SelectProductBindingRecord(string casecode, string bindingState);
        #endregion

        #region 【接口】SelectPackageProduct 查询打包产品记录
        [OperationContract]
        [SwaggerWcfPath("SelectPackageProduct", "查询打包产品记录")]
        [WebInvoke(Method = "POST", UriTemplate = "SelectPackageProduct",
            BodyStyle = WebMessageBodyStyle.WrappedRequest, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        DataSet SelectPackageProduct(PackageProduct packageProduct);
        #endregion
    }
}
