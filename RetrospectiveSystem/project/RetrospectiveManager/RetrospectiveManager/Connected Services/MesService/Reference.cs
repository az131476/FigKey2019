﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace RetrospectiveManager.MesService {
    using System.Runtime.Serialization;
    using System;
    
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="CompositeType", Namespace="http://schemas.datacontract.org/2004/07/MESInterface")]
    [System.SerializableAttribute()]
    public partial class CompositeType : object, System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged {
        
        [System.NonSerializedAttribute()]
        private System.Runtime.Serialization.ExtensionDataObject extensionDataField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private bool BoolValueField;
        
        [System.Runtime.Serialization.OptionalFieldAttribute()]
        private string StringValueField;
        
        [global::System.ComponentModel.BrowsableAttribute(false)]
        public System.Runtime.Serialization.ExtensionDataObject ExtensionData {
            get {
                return this.extensionDataField;
            }
            set {
                this.extensionDataField = value;
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public bool BoolValue {
            get {
                return this.BoolValueField;
            }
            set {
                if ((this.BoolValueField.Equals(value) != true)) {
                    this.BoolValueField = value;
                    this.RaisePropertyChanged("BoolValue");
                }
            }
        }
        
        [System.Runtime.Serialization.DataMemberAttribute()]
        public string StringValue {
            get {
                return this.StringValueField;
            }
            set {
                if ((object.ReferenceEquals(this.StringValueField, value) != true)) {
                    this.StringValueField = value;
                    this.RaisePropertyChanged("StringValue");
                }
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LoginUser", Namespace="http://schemas.datacontract.org/2004/07/MESInterface.Molde")]
    public enum LoginUser : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ORDINARY_USER = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ADMIN_USER = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        TOURIST_USER = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="LoginResult", Namespace="http://schemas.datacontract.org/2004/07/MESInterface.Molde")]
    public enum LoginResult : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        USER_NAME_ERR = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        USER_PWD_ERR = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        USER_NAME_PWD_ERR = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        FAIL_EXCEP = 3,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        SUCCESS = 4,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="QueryResult", Namespace="http://schemas.datacontract.org/2004/07/MESInterface.Molde")]
    public enum QueryResult : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EXIST_DATA = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        NONE_DATE = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        EXCEPT_ERR = 2,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="RegisterResult", Namespace="http://schemas.datacontract.org/2004/07/MESInterface.Molde")]
    public enum RegisterResult : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        REGISTER_SUCCESS = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        REGISTER_EXIST_USER = 1,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        REGISTER_FAIL_SQL = 2,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        REGISTER_ERR = 3,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="MesService.IMesService")]
    public interface IMesService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetData", ReplyAction="http://tempuri.org/IMesService/GetDataResponse")]
        string GetData(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetData", ReplyAction="http://tempuri.org/IMesService/GetDataResponse")]
        System.Threading.Tasks.Task<string> GetDataAsync(int value);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IMesService/GetDataUsingDataContractResponse")]
        RetrospectiveManager.MesService.CompositeType GetDataUsingDataContract(RetrospectiveManager.MesService.CompositeType composite);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetDataUsingDataContract", ReplyAction="http://tempuri.org/IMesService/GetDataUsingDataContractResponse")]
        System.Threading.Tasks.Task<RetrospectiveManager.MesService.CompositeType> GetDataUsingDataContractAsync(RetrospectiveManager.MesService.CompositeType composite);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/Login", ReplyAction="http://tempuri.org/IMesService/LoginResponse")]
        RetrospectiveManager.MesService.LoginResult Login(string username, string password, RetrospectiveManager.MesService.LoginUser loginUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/Login", ReplyAction="http://tempuri.org/IMesService/LoginResponse")]
        System.Threading.Tasks.Task<RetrospectiveManager.MesService.LoginResult> LoginAsync(string username, string password, RetrospectiveManager.MesService.LoginUser loginUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetUserInfo", ReplyAction="http://tempuri.org/IMesService/GetUserInfoResponse")]
        RetrospectiveManager.MesService.GetUserInfoResponse GetUserInfo(RetrospectiveManager.MesService.GetUserInfoRequest request);
        
        // CODEGEN: 正在生成消息协定，应为该操作具有多个返回值。
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetUserInfo", ReplyAction="http://tempuri.org/IMesService/GetUserInfoResponse")]
        System.Threading.Tasks.Task<RetrospectiveManager.MesService.GetUserInfoResponse> GetUserInfoAsync(RetrospectiveManager.MesService.GetUserInfoRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetAllUserInfo", ReplyAction="http://tempuri.org/IMesService/GetAllUserInfoResponse")]
        System.Data.DataSet GetAllUserInfo();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/GetAllUserInfo", ReplyAction="http://tempuri.org/IMesService/GetAllUserInfoResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> GetAllUserInfoAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/Register", ReplyAction="http://tempuri.org/IMesService/RegisterResponse")]
        RetrospectiveManager.MesService.RegisterResult Register(string username, string pwd, string phone, string email, RetrospectiveManager.MesService.LoginUser loginUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/Register", ReplyAction="http://tempuri.org/IMesService/RegisterResponse")]
        System.Threading.Tasks.Task<RetrospectiveManager.MesService.RegisterResult> RegisterAsync(string username, string pwd, string phone, string email, RetrospectiveManager.MesService.LoginUser loginUser);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/Firstcheck", ReplyAction="http://tempuri.org/IMesService/FirstcheckResponse")]
        string Firstcheck(string sn, string sTypeNumber, string sStationName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/Firstcheck", ReplyAction="http://tempuri.org/IMesService/FirstcheckResponse")]
        System.Threading.Tasks.Task<string> FirstcheckAsync(string sn, string sTypeNumber, string sStationName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/InsertWIP", ReplyAction="http://tempuri.org/IMesService/InsertWIPResponse")]
        string InsertWIP(string sn, string sTypeNumber, string sStationName, string sTestResult, string sTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/InsertWIP", ReplyAction="http://tempuri.org/IMesService/InsertWIPResponse")]
        System.Threading.Tasks.Task<string> InsertWIPAsync(string sn, string sTypeNumber, string sStationName, string sTestResult, string sTime);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/InsertProduce", ReplyAction="http://tempuri.org/IMesService/InsertProduceResponse")]
        string InsertProduce(System.Collections.Generic.Dictionary<int, string> dctData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/InsertProduce", ReplyAction="http://tempuri.org/IMesService/InsertProduceResponse")]
        System.Threading.Tasks.Task<string> InsertProduceAsync(System.Collections.Generic.Dictionary<int, string> dctData);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectProduce", ReplyAction="http://tempuri.org/IMesService/SelectProduceResponse")]
        System.Data.DataSet SelectProduce();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/SelectProduce", ReplyAction="http://tempuri.org/IMesService/SelectProduceResponse")]
        System.Threading.Tasks.Task<System.Data.DataSet> SelectProduceAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/UpdateProduce", ReplyAction="http://tempuri.org/IMesService/UpdateProduceResponse")]
        string UpdateProduce(System.Collections.Generic.Dictionary<int, string> data);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IMesService/UpdateProduce", ReplyAction="http://tempuri.org/IMesService/UpdateProduceResponse")]
        System.Threading.Tasks.Task<string> UpdateProduceAsync(System.Collections.Generic.Dictionary<int, string> data);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetUserInfo", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetUserInfoRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public string userName;
        
        public GetUserInfoRequest() {
        }
        
        public GetUserInfoRequest(string userName) {
            this.userName = userName;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.MessageContractAttribute(WrapperName="GetUserInfoResponse", WrapperNamespace="http://tempuri.org/", IsWrapped=true)]
    public partial class GetUserInfoResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=0)]
        public RetrospectiveManager.MesService.QueryResult GetUserInfoResult;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://tempuri.org/", Order=1)]
        public System.Data.DataSet dataSet;
        
        public GetUserInfoResponse() {
        }
        
        public GetUserInfoResponse(RetrospectiveManager.MesService.QueryResult GetUserInfoResult, System.Data.DataSet dataSet) {
            this.GetUserInfoResult = GetUserInfoResult;
            this.dataSet = dataSet;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IMesServiceChannel : RetrospectiveManager.MesService.IMesService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class MesServiceClient : System.ServiceModel.ClientBase<RetrospectiveManager.MesService.IMesService>, RetrospectiveManager.MesService.IMesService {
        
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
        
        public string GetData(int value) {
            return base.Channel.GetData(value);
        }
        
        public System.Threading.Tasks.Task<string> GetDataAsync(int value) {
            return base.Channel.GetDataAsync(value);
        }
        
        public RetrospectiveManager.MesService.CompositeType GetDataUsingDataContract(RetrospectiveManager.MesService.CompositeType composite) {
            return base.Channel.GetDataUsingDataContract(composite);
        }
        
        public System.Threading.Tasks.Task<RetrospectiveManager.MesService.CompositeType> GetDataUsingDataContractAsync(RetrospectiveManager.MesService.CompositeType composite) {
            return base.Channel.GetDataUsingDataContractAsync(composite);
        }
        
        public RetrospectiveManager.MesService.LoginResult Login(string username, string password, RetrospectiveManager.MesService.LoginUser loginUser) {
            return base.Channel.Login(username, password, loginUser);
        }
        
        public System.Threading.Tasks.Task<RetrospectiveManager.MesService.LoginResult> LoginAsync(string username, string password, RetrospectiveManager.MesService.LoginUser loginUser) {
            return base.Channel.LoginAsync(username, password, loginUser);
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        RetrospectiveManager.MesService.GetUserInfoResponse RetrospectiveManager.MesService.IMesService.GetUserInfo(RetrospectiveManager.MesService.GetUserInfoRequest request) {
            return base.Channel.GetUserInfo(request);
        }
        
        public RetrospectiveManager.MesService.QueryResult GetUserInfo(string userName, out System.Data.DataSet dataSet) {
            RetrospectiveManager.MesService.GetUserInfoRequest inValue = new RetrospectiveManager.MesService.GetUserInfoRequest();
            inValue.userName = userName;
            RetrospectiveManager.MesService.GetUserInfoResponse retVal = ((RetrospectiveManager.MesService.IMesService)(this)).GetUserInfo(inValue);
            dataSet = retVal.dataSet;
            return retVal.GetUserInfoResult;
        }
        
        public System.Threading.Tasks.Task<RetrospectiveManager.MesService.GetUserInfoResponse> GetUserInfoAsync(RetrospectiveManager.MesService.GetUserInfoRequest request) {
            return base.Channel.GetUserInfoAsync(request);
        }
        
        public System.Data.DataSet GetAllUserInfo() {
            return base.Channel.GetAllUserInfo();
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> GetAllUserInfoAsync() {
            return base.Channel.GetAllUserInfoAsync();
        }
        
        public RetrospectiveManager.MesService.RegisterResult Register(string username, string pwd, string phone, string email, RetrospectiveManager.MesService.LoginUser loginUser) {
            return base.Channel.Register(username, pwd, phone, email, loginUser);
        }
        
        public System.Threading.Tasks.Task<RetrospectiveManager.MesService.RegisterResult> RegisterAsync(string username, string pwd, string phone, string email, RetrospectiveManager.MesService.LoginUser loginUser) {
            return base.Channel.RegisterAsync(username, pwd, phone, email, loginUser);
        }
        
        public string Firstcheck(string sn, string sTypeNumber, string sStationName) {
            return base.Channel.Firstcheck(sn, sTypeNumber, sStationName);
        }
        
        public System.Threading.Tasks.Task<string> FirstcheckAsync(string sn, string sTypeNumber, string sStationName) {
            return base.Channel.FirstcheckAsync(sn, sTypeNumber, sStationName);
        }
        
        public string InsertWIP(string sn, string sTypeNumber, string sStationName, string sTestResult, string sTime) {
            return base.Channel.InsertWIP(sn, sTypeNumber, sStationName, sTestResult, sTime);
        }
        
        public System.Threading.Tasks.Task<string> InsertWIPAsync(string sn, string sTypeNumber, string sStationName, string sTestResult, string sTime) {
            return base.Channel.InsertWIPAsync(sn, sTypeNumber, sStationName, sTestResult, sTime);
        }
        
        public string InsertProduce(System.Collections.Generic.Dictionary<int, string> dctData) {
            return base.Channel.InsertProduce(dctData);
        }
        
        public System.Threading.Tasks.Task<string> InsertProduceAsync(System.Collections.Generic.Dictionary<int, string> dctData) {
            return base.Channel.InsertProduceAsync(dctData);
        }
        
        public System.Data.DataSet SelectProduce() {
            return base.Channel.SelectProduce();
        }
        
        public System.Threading.Tasks.Task<System.Data.DataSet> SelectProduceAsync() {
            return base.Channel.SelectProduceAsync();
        }
        
        public string UpdateProduce(System.Collections.Generic.Dictionary<int, string> data) {
            return base.Channel.UpdateProduce(data);
        }
        
        public System.Threading.Tasks.Task<string> UpdateProduceAsync(System.Collections.Generic.Dictionary<int, string> data) {
            return base.Channel.UpdateProduceAsync(data);
        }
    }
}
