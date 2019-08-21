﻿using SwaggerWcf.Attributes;
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

        #region 【接口】TestCommunication 测试通讯
        [OperationContract]
        [SwaggerWcfPath("TestCommunication", "测试通讯")]
        [WebInvoke(Method = "GET", UriTemplate = "TestCommunication?value={value}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string TestCommunication([SwaggerWcfParameter(Description = "传入任意字符串=返回值")]string value);
        #endregion

        #region 【接口】UpdateTestResultData 更新测试数据
        [OperationContract]
        [SwaggerWcfPath("UpdateTestResultData", "更新测试数据")]
        [WebInvoke(Method = "GET", UriTemplate = "UpdateTestResultData?sn={sn}&typeNO={typeNo}&station={station}&result={result}&teamLeader={teamLeader}&admin={admin}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string UpdateTestResultData([SwaggerWcfParameter(Description = "追溯码*")]string sn, 
            [SwaggerWcfParameter(Description = "产品型号*")]string typeNo, 
            [SwaggerWcfParameter(Description = "站位名称*")]string station,  
            [SwaggerWcfParameter(Description = "测试结果*，PASS/FAIL")]string result,
            [SwaggerWcfParameter(Description = "班组长")]string teamLeader,
            [SwaggerWcfParameter(Description = "管理员")]string admin);
        #endregion

        #region 【接口】SelectLastTestResult 查询上一站位最新记录
        [OperationContract]
        [SwaggerWcfPath("SelectLastTestResult", "查询上一站位最新记录;测试结果：【成功】返回数组,len = 4;array[0] = sn;array[1] = station;array[2] = testRes;")]
        [WebInvoke(Method = "GET", UriTemplate = "SelectLastTestResult?sn={sn}&station={station}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json),Description]
        string[] SelectLastTestResult([SwaggerWcfParameter(Description = "追溯码*")]string sn, 
            [SwaggerWcfParameter(Description = "当前站位名称*")]string station);
        #endregion

        #region【接口】UpdateMaterialStatistics 更新物料计数
        [OperationContract]
        [SwaggerWcfPath("UpdateMaterialStatistics", "更新物料计数")]
        [WebInvoke(Method = "GET", UriTemplate = "UpdateMaterialStatistics?snInner={snInner}&snOutter={snOutter}&typeNo={typeNo}" +
            "&stationName={stationName}&materialCode={materialCode}&amount={amount}&teamLeader={teamLeader}&admin={admin}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string UpdateMaterialStatistics([SwaggerWcfParameter(Description = "追溯码(内壳)*")]string snInner, 
            [SwaggerWcfParameter(Description = "追溯码(外壳)*")]string snOutter, 
            [SwaggerWcfParameter(Description = "产品型号*")]string typeNo, 
            [SwaggerWcfParameter(Description = "站位名称*")]string stationName,
            [SwaggerWcfParameter(Description = "物料编码*")]string materialCode, 
            [SwaggerWcfParameter(Description = "物料数量(使用数量)*")]int amount,
            [SwaggerWcfParameter(Description = "班组长")]string teamLeader,
            [SwaggerWcfParameter(Description = "管理员")]string admin);
        #endregion

        #region【接口】UpdatePackageProductBindingMsg 【打包/抽检】添加绑定信息/更新绑定信息                
        [OperationContract]
        [SwaggerWcfPath("UpdatePackageProductBindingMsg", "成品打包/成品抽检-更新数据/绑定/解绑")]
        [WebInvoke(Method = "GET", UriTemplate = "UpdatePackageProductBindingMsg?outCaseCode={outCaseCode}&snOutter={snOutter}" +
            "&typeNo={typeNo}&stationName={stationName}&bindingState={bindingState}&remark={remark}&teamLeader={teamLeader}&admin={admin}",
            BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string UpdatePackageProductBindingMsg([SwaggerWcfParameter(Description = "箱子编码*")]string outCaseCode, 
            [SwaggerWcfParameter(Description = "追溯码*")]string snOutter, 
            [SwaggerWcfParameter(Description = "产品型号*")]string typeNo,
            [SwaggerWcfParameter(Description = "工序名称*")]string stationName,
            [SwaggerWcfParameter(Description = "绑定或解绑,0-解除绑定,1-添加绑定*")]string bindingState,
            [SwaggerWcfParameter(Description = "备注(解绑时要写明原因)")]string remark,
            [SwaggerWcfParameter(Description = "班组长")]string teamLeader,
            [SwaggerWcfParameter(Description = "管理员")]string admin);
        #endregion

        #region 【接口】 UpdateProgrameVersion 更新测试程序版本号
        [OperationContract]
        [SwaggerWcfPath("UpdateProgrameVersion", "更新测试程序版本号")]
        [WebInvoke(Method = "GET",UriTemplate = "UpdateProgrameVersion?typeNo={typeNo}&stationName={stationName}" +
            "&programeName={programeName}&programeVersion={programeVersion}&teamLeader={teamLeader}&admin={admin}",
            BodyStyle = WebMessageBodyStyle.Bare,ResponseFormat = WebMessageFormat.Json,RequestFormat = WebMessageFormat.Json)]
        string UpdateProgrameVersion([SwaggerWcfParameter(Description = "产品型号*")]string typeNo,
            [SwaggerWcfParameter(Description = "工站名称*")]string stationName,
            [SwaggerWcfParameter(Description = "程序名称*")]string programeName,
            [SwaggerWcfParameter(Description = "程序版本*")]string programeVersion,
            [SwaggerWcfParameter(Description = "班组长")]string teamLeader,
            [SwaggerWcfParameter(Description = "管理员")]string admin);
        #endregion

        #region 【接口】 UpdateLimitConfig 更新limit配置
        [OperationContract]
        [SwaggerWcfPath("UpdateProgrameVersion", "更新测试程序版本号")]
        [WebInvoke(Method = "GET", UriTemplate = "UpdateLimitConfig?stationName={stationName}&typeNo={typeNo}&" +
            "limitValue={limitValue}&teamLeader={teamLeader}&admin={admin}",
            BodyStyle = WebMessageBodyStyle.Bare,RequestFormat = WebMessageFormat.Json,ResponseFormat = WebMessageFormat.Json)]
        string UpdateLimitConfig([SwaggerWcfParameter(Description = "工站名称*")]string stationName,
            [SwaggerWcfParameter(Description = "产品型号*")]string typeNo,
            [SwaggerWcfParameter(Description = "limit值*")]string limitValue,
            [SwaggerWcfParameter(Description = "班组长")]string teamLeader,
            [SwaggerWcfParameter(Description = "管理员")]string admin);
        #endregion

        #region 【接口】 SelectCurrentTProcess 查询当前工艺流程
        [OperationContract]
        [SwaggerWcfPath("SelectCurrentTProcess", "查询当前工艺流程")]
        [WebInvoke(Method = "GET", UriTemplate = "SelectCurrentTProcess",BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,ResponseFormat = WebMessageFormat.Json)]
        string SelectCurrentTProcess();
        #endregion

        #region 【接口】 SelectAllTProcess 查询所有工艺流程
        [OperationContract]
        [SwaggerWcfPath("SelectAllTProcess", "查询所有工艺流程")]
        [WebInvoke(Method = "GET", UriTemplate = "SelectAllTProcess", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string[] SelectAllTProcess();
        #endregion

        #region 【接口】 SelectStationList 查询所有工序列表
        [OperationContract]
        [SwaggerWcfPath("SelectStationList", "查询所有工艺流程")]
        [WebInvoke(Method = "GET", UriTemplate = "SelectStationList?processName={processName}", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string[] SelectStationList([SwaggerWcfParameter(Description = "工序名称")]string processName);
        #endregion

        #region 【接口】 SelectTypeNoList 查询所有产品型号
        [OperationContract]
        [SwaggerWcfPath("SelectTypeNoList", "查询所有产品型号")]
        [WebInvoke(Method = "GET", UriTemplate = "SelectTypeNoList", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string[] SelectTypeNoList();
        #endregion

        #region 【接口】 SelectMaterialList 查询所有物料
        [OperationContract]
        [SwaggerWcfPath("SelectMaterialList", "产线所有物料/根据产品型号查询")]
        [WebInvoke(Method = "GET", UriTemplate = "SelectMaterialList?productTypeNo={productTypeNo}", BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string[] SelectMaterialList([SwaggerWcfParameter(Description = "产品型号")]string productTypeNo);
        #endregion
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