using SwaggerWcf.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace WcfTest
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [SwaggerWcf("/Service1")]
    public class Service1 : IService1
    {
        [SwaggerWcfTag("Service1 服务")]
        [SwaggerWcfResponse(HttpStatusCode.Created, "Book created, value in the response body with id updated")]
        [SwaggerWcfResponse(HttpStatusCode.BadRequest, "Bad request", true)]
        [SwaggerWcfResponse(HttpStatusCode.InternalServerError,
            "Internal error (can be forced using ERROR_500 as book title)", true)]
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        [SwaggerWcfTag("Service1 服务")]
        public string GetDataT(Interface1 composite, int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public string GetDataA(CompositeType composite)
        {
            return string.Format("You entered: {0}", composite.StringValue);
        }

        [SwaggerWcfTag("Service1 服务")]
        public string GetDataB(CompositeType composite, CompositeType compositea)
        {
            return string.Format("You entered: {0}", composite.StringValue);
        }

        [SwaggerWcfTag("Service1 服务")]
        public string GetDataTT(string str, int value)
        {
            return string.Format("You entered: {0}", value);
        }

        [SwaggerWcfTag("Service1 服务")]
        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
