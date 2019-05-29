using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using FigKeyLoggerWcf.DB;
using FigKeyLoggerWcf.Molde;

namespace FigKeyLoggerWcf
{
    [ServiceContract]
    public interface IFigKeyLoggerService
    {
        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: 在此添加您的服务操作
        [OperationContract]
        LoginResult Login(string username, string password, LoginUser loginUser);
        [OperationContract]
        f_user GetUserInfo(string userName);

        [OperationContract]
        bool SQLConnection();

        //[OperationContract]
        //bool SaveOption(string option_name, string option_value);
        //[OperationContract]
        //string GetOptionValue(string option_name);
        //[OperationContract]
        //bool SaveOptionByUser(string option_name, string option_value, int userid);
        //[OperationContract]
        //string GetOptionValueByUser(string option_name, int userid);
    }

    // 使用下面示例中说明的数据约定将复合类型添加到服务操作。
    // 可以将 XSD 文件添加到项目中。在生成项目后，可以通过命名空间“FigKeyLoggerWcf.ContractType”直接使用其中定义的数据类型。
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
