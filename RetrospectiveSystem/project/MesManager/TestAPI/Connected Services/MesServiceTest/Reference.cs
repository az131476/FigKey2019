﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestAPI.MesServiceTest {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MesServiceTest.IMesService")]
    public interface IMesService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/TestCommunication", ReplyAction="http://tempuri.org/IMesService/TestCommunicationResponse")]
        string TestCommunication(string value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/TestCommunication", ReplyAction="http://tempuri.org/IMesService/TestCommunicationResponse")]
        System.Threading.Tasks.Task<string> TestCommunicationAsync(string value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/UpdateTestResultData", ReplyAction="http://tempuri.org/IMesService/UpdateTestResultDataResponse")]
        string UpdateTestResultData(string sn, string typeNo, string station, string result, string teamLeader, string admin);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/UpdateTestResultData", ReplyAction="http://tempuri.org/IMesService/UpdateTestResultDataResponse")]
        System.Threading.Tasks.Task<string> UpdateTestResultDataAsync(string sn, string typeNo, string station, string result, string teamLeader, string admin);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectLastTestResult", ReplyAction="http://tempuri.org/IMesService/SelectLastTestResultResponse")]
        string[] SelectLastTestResult(string sn, string station);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectLastTestResult", ReplyAction="http://tempuri.org/IMesService/SelectLastTestResultResponse")]
        System.Threading.Tasks.Task<string[]> SelectLastTestResultAsync(string sn, string station);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/UpdateMaterialStatistics", ReplyAction="http://tempuri.org/IMesService/UpdateMaterialStatisticsResponse")]
        string[] UpdateMaterialStatistics(string[] materialArray);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/UpdateMaterialStatistics", ReplyAction="http://tempuri.org/IMesService/UpdateMaterialStatisticsResponse")]
        System.Threading.Tasks.Task<string[]> UpdateMaterialStatisticsAsync(string[] materialArray);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/UpdatePackageProductBindingMsg", ReplyAction="http://tempuri.org/IMesService/UpdatePackageProductBindingMsgResponse")]
        string UpdatePackageProductBindingMsg(string outCaseCode, string snOutter, string typeNo, string stationName, string bindingState, string remark, string teamLeader, string admin);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/UpdatePackageProductBindingMsg", ReplyAction="http://tempuri.org/IMesService/UpdatePackageProductBindingMsgResponse")]
        System.Threading.Tasks.Task<string> UpdatePackageProductBindingMsgAsync(string outCaseCode, string snOutter, string typeNo, string stationName, string bindingState, string remark, string teamLeader, string admin);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/UpdateProgrameVersion", ReplyAction="http://tempuri.org/IMesService/UpdateProgrameVersionResponse")]
        string UpdateProgrameVersion(string typeNo, string stationName, string programeName, string programeVersion, string teamLeader, string admin);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/UpdateProgrameVersion", ReplyAction="http://tempuri.org/IMesService/UpdateProgrameVersionResponse")]
        System.Threading.Tasks.Task<string> UpdateProgrameVersionAsync(string typeNo, string stationName, string programeName, string programeVersion, string teamLeader, string admin);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/UpdateLimitConfig", ReplyAction="http://tempuri.org/IMesService/UpdateLimitConfigResponse")]
        string UpdateLimitConfig(string stationName, string typeNo, string limitValue, string teamLeader, string admin);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/UpdateLimitConfig", ReplyAction="http://tempuri.org/IMesService/UpdateLimitConfigResponse")]
        System.Threading.Tasks.Task<string> UpdateLimitConfigAsync(string stationName, string typeNo, string limitValue, string teamLeader, string admin);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectCurrentTProcess", ReplyAction="http://tempuri.org/IMesService/SelectCurrentTProcessResponse")]
        string SelectCurrentTProcess();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectCurrentTProcess", ReplyAction="http://tempuri.org/IMesService/SelectCurrentTProcessResponse")]
        System.Threading.Tasks.Task<string> SelectCurrentTProcessAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectAllTProcess", ReplyAction="http://tempuri.org/IMesService/SelectAllTProcessResponse")]
        string[] SelectAllTProcess();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectAllTProcess", ReplyAction="http://tempuri.org/IMesService/SelectAllTProcessResponse")]
        System.Threading.Tasks.Task<string[]> SelectAllTProcessAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectStationList", ReplyAction="http://tempuri.org/IMesService/SelectStationListResponse")]
        string[] SelectStationList(string processName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectStationList", ReplyAction="http://tempuri.org/IMesService/SelectStationListResponse")]
        System.Threading.Tasks.Task<string[]> SelectStationListAsync(string processName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectTypeNoList", ReplyAction="http://tempuri.org/IMesService/SelectTypeNoListResponse")]
        string[] SelectTypeNoList();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectTypeNoList", ReplyAction="http://tempuri.org/IMesService/SelectTypeNoListResponse")]
        System.Threading.Tasks.Task<string[]> SelectTypeNoListAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectMaterialList", ReplyAction="http://tempuri.org/IMesService/SelectMaterialListResponse")]
        string[] SelectMaterialList(string productTypeNo);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectMaterialList", ReplyAction="http://tempuri.org/IMesService/SelectMaterialListResponse")]
        System.Threading.Tasks.Task<string[]> SelectMaterialListAsync(string productTypeNo);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMesServiceChannel : TestAPI.MesServiceTest.IMesService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MesServiceClient : System.ServiceModel.ClientBase<TestAPI.MesServiceTest.IMesService>, TestAPI.MesServiceTest.IMesService {
        
        public MesServiceClient() {
        }
        
        public MesServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public MesServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MesServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public MesServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public string TestCommunication(string value) {
            return base.Channel.TestCommunication(value);
        }
        
        public System.Threading.Tasks.Task<string> TestCommunicationAsync(string value) {
            return base.Channel.TestCommunicationAsync(value);
        }
        
        public string UpdateTestResultData(string sn, string typeNo, string station, string result, string teamLeader, string admin) {
            return base.Channel.UpdateTestResultData(sn, typeNo, station, result, teamLeader, admin);
        }
        
        public System.Threading.Tasks.Task<string> UpdateTestResultDataAsync(string sn, string typeNo, string station, string result, string teamLeader, string admin) {
            return base.Channel.UpdateTestResultDataAsync(sn, typeNo, station, result, teamLeader, admin);
        }
        
        public string[] SelectLastTestResult(string sn, string station) {
            return base.Channel.SelectLastTestResult(sn, station);
        }
        
        public System.Threading.Tasks.Task<string[]> SelectLastTestResultAsync(string sn, string station) {
            return base.Channel.SelectLastTestResultAsync(sn, station);
        }
        
        public string[] UpdateMaterialStatistics(string[] materialArray) {
            return base.Channel.UpdateMaterialStatistics(materialArray);
        }
        
        public System.Threading.Tasks.Task<string[]> UpdateMaterialStatisticsAsync(string[] materialArray) {
            return base.Channel.UpdateMaterialStatisticsAsync(materialArray);
        }
        
        public string UpdatePackageProductBindingMsg(string outCaseCode, string snOutter, string typeNo, string stationName, string bindingState, string remark, string teamLeader, string admin) {
            return base.Channel.UpdatePackageProductBindingMsg(outCaseCode, snOutter, typeNo, stationName, bindingState, remark, teamLeader, admin);
        }
        
        public System.Threading.Tasks.Task<string> UpdatePackageProductBindingMsgAsync(string outCaseCode, string snOutter, string typeNo, string stationName, string bindingState, string remark, string teamLeader, string admin) {
            return base.Channel.UpdatePackageProductBindingMsgAsync(outCaseCode, snOutter, typeNo, stationName, bindingState, remark, teamLeader, admin);
        }
        
        public string UpdateProgrameVersion(string typeNo, string stationName, string programeName, string programeVersion, string teamLeader, string admin) {
            return base.Channel.UpdateProgrameVersion(typeNo, stationName, programeName, programeVersion, teamLeader, admin);
        }
        
        public System.Threading.Tasks.Task<string> UpdateProgrameVersionAsync(string typeNo, string stationName, string programeName, string programeVersion, string teamLeader, string admin) {
            return base.Channel.UpdateProgrameVersionAsync(typeNo, stationName, programeName, programeVersion, teamLeader, admin);
        }
        
        public string UpdateLimitConfig(string stationName, string typeNo, string limitValue, string teamLeader, string admin) {
            return base.Channel.UpdateLimitConfig(stationName, typeNo, limitValue, teamLeader, admin);
        }
        
        public System.Threading.Tasks.Task<string> UpdateLimitConfigAsync(string stationName, string typeNo, string limitValue, string teamLeader, string admin) {
            return base.Channel.UpdateLimitConfigAsync(stationName, typeNo, limitValue, teamLeader, admin);
        }
        
        public string SelectCurrentTProcess() {
            return base.Channel.SelectCurrentTProcess();
        }
        
        public System.Threading.Tasks.Task<string> SelectCurrentTProcessAsync() {
            return base.Channel.SelectCurrentTProcessAsync();
        }
        
        public string[] SelectAllTProcess() {
            return base.Channel.SelectAllTProcess();
        }
        
        public System.Threading.Tasks.Task<string[]> SelectAllTProcessAsync() {
            return base.Channel.SelectAllTProcessAsync();
        }
        
        public string[] SelectStationList(string processName) {
            return base.Channel.SelectStationList(processName);
        }
        
        public System.Threading.Tasks.Task<string[]> SelectStationListAsync(string processName) {
            return base.Channel.SelectStationListAsync(processName);
        }
        
        public string[] SelectTypeNoList() {
            return base.Channel.SelectTypeNoList();
        }
        
        public System.Threading.Tasks.Task<string[]> SelectTypeNoListAsync() {
            return base.Channel.SelectTypeNoListAsync();
        }
        
        public string[] SelectMaterialList(string productTypeNo) {
            return base.Channel.SelectMaterialList(productTypeNo);
        }
        
        public System.Threading.Tasks.Task<string[]> SelectMaterialListAsync(string productTypeNo) {
            return base.Channel.SelectMaterialListAsync(productTypeNo);
        }
    }
}