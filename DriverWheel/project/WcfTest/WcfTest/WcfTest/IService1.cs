using SwaggerWcf.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfTest
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        [SwaggerWcfPath("标题GetData", "方法详细说明")]
        [WebInvoke(Method = "GET", UriTemplate = "GetData?value={value}",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string GetData(int value);

        [OperationContract]
        [SwaggerWcfPath("标题GetDataT", "方法详细说明")]
        [WebInvoke(Method = "POST", UriTemplate = "GetDataT?value={value}",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string GetDataT(Interface1 composite,int value);

        [OperationContract]
        string GetDataA(CompositeType composite);

        [OperationContract]
        [SwaggerWcfPath("标题GetDataB", "方法详细说明")]
        [WebInvoke(Method = "POST", UriTemplate = "GetDataB",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string GetDataB(CompositeType composite, CompositeType compositea);

        [OperationContract]
        [SwaggerWcfPath("标题GetDataTT", "方法详细说明")]
        [WebInvoke(Method = "GET", UriTemplate = "GetDataTT?str={str}&value={value}",
            BodyStyle = WebMessageBodyStyle.Wrapped,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string GetDataTT(string str, int value);

        [OperationContract]
        [SwaggerWcfPath("标题GetDataUsingDataContract", "方法详细说明")]
        [WebInvoke(Method = "POST", UriTemplate = "GetDataUsingDataContract",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        CompositeType GetDataUsingDataContract([SwaggerWcfParameter(Description = "Book to be created, the id will be replaced")]CompositeType composite);

        // TODO: 在此添加您的服务操作
    }

    // 使用下面示例中说明的数据约定将复合类型添加到服务操作。
    [DataContract]
    [Description("CompositeType-实体类")]
    //[SwaggerWcfDefinition(ExternalDocsUrl = "http://en.wikipedia.org/wiki/Book", ExternalDocsDescription = "CompositeType实体")]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        [Description("布尔值")]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        [Description("字符串 1：a<br/>2：b")]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
        }

        [DataMember]
        public Guid Test { get; set; }
    }
}
