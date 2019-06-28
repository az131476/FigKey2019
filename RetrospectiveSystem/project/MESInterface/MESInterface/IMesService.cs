using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MESInterface.Molde;
using System.Data;

namespace MESInterface
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“IService1”。
    [ServiceContract]
    public interface IMesService
    {

        [OperationContract]
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        // TODO: 在此添加您的服务操作
        
        [OperationContract]
        LoginResult Login(string username, string password, LoginUser loginUser);

        [OperationContract]
        QueryResult GetUserInfo(string userName,out DataSet dataSet);

        [OperationContract]
        DataSet GetAllUserInfo();

        [OperationContract]
        RegisterResult Register(string username, string pwd, string phone, string email, LoginUser loginUser);

        /// <returns></returns>
        [OperationContract]
        string FirstCheck(string snInner,string snOutter, string sTypeNumber, string sStationName); 

         [OperationContract]
        string InsertWIP(string sn, string sTypeNumber, string sStationName, string sTestResult, string sTime);

        [OperationContract]
        string InsertProduce(Dictionary<int, string> dctData);

        [OperationContract]
        DataSet SelectProduce(string stationName,string stationOrder);

        [OperationContract]
        string UpdateProduce(Dictionary<int, string> data);

        [OperationContract]
        int DeleteProduce(string stationName);

        [OperationContract]
        int DeleteAllProduce();

        [OperationContract]
        int DeleteProductType(string productName);

        [OperationContract]
        int DeleteAllProductType();

        [OperationContract]
        DataSet SelectProductType(string productName);

        [OperationContract]
        string CommitProductType(Dictionary<int, string> dctData);

        [OperationContract]
        int DeleteAllTypeStation();

        [OperationContract]
        int DeleteTypeStation(string typeNumber);

        [OperationContract]
        DataSet SelectTypeStation(string typeNumber);

        [OperationContract]
        string CommitTypeStation(Dictionary<string, string[]> dctData);

        [OperationContract]
        DataSet SelectProductDataOfSN(string sn, bool IsSnFuzzy);

        [OperationContract]
        DataSet SelectProductDataOfTypeNo(string typeNo);
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
