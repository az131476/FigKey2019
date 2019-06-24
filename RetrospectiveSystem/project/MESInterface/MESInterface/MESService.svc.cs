using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using MESInterface.Molde;
using System.Data;
using CommonUtils.DB;
using CommonUtils.Logger;
using System.Configuration;
using System.Collections;

namespace MESInterface
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    public class MesService : IMesService
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

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


        #region 用户登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名/手机号/邮箱</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public LoginResult Login(string username, string password, LoginUser loginUser)
        {
            //暂未处理用户角色
            try
            {
                DataSet dataSet;
                QueryResult queryResult = GetUserInfo(username,out dataSet);
                if (queryResult == QueryResult.NONE_DATE)
                {
                    //用户不存在
                    LogHelper.Log.Info($"用户名{username}不存在，验证失败！");
                    return LoginResult.USER_NAME_ERR;
                }
                else if(queryResult == QueryResult.EXIST_DATA)
                {
                    //用户存在
                    //验证登录密码
                    string sql = "SELECT * " +
                        "FROM " +
                        "[WT_SCL].[dbo].[f_user] " +
                        "WHERE " +
                        $"[password] = '{password}' ";
                    DataTable dtRes = SQLServer.ExecuteDataSet(sql).Tables[0];
                    if (dtRes.Rows.Count < 1)
                    {
                        //密码验证失败
                        LogHelper.Log.Info($"用户{username}密码验证失败！");
                        return LoginResult.USER_PWD_ERR;
                    }
                    else
                    {
                        //通过验证
                        LogHelper.Log.Info(username +" 登录进入 " + DateTime.Now);
                        return LoginResult.SUCCESS;
                    }
                }
                return LoginResult.FAIL_EXCEP;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error("用户登录异常..." + ex.Message);
                return LoginResult.FAIL_EXCEP;
            }
        }

        #endregion

        #region 查询用户信息
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public QueryResult GetUserInfo(string username, out DataSet dataSet)
        {
            string sqlString = "SELECT * " +
            "FROM [WT_SCL].[dbo].[f_user] " +
            "WHERE " +
            $"[username] = '{username}' or [phone] = '{username}' or [email] = '{username}' ";
            try
            {
                SQLServer.SqlConnectionString = connectionString;
                dataSet = SQLServer.ExecuteDataSet(sqlString);
                if (dataSet.Tables[0].Rows.Count < 1)
                {
                    return QueryResult.NONE_DATE;
                }
                else
                {
                    return QueryResult.EXIST_DATA;
                }
            }
            catch(Exception ex)
            {
                LogHelper.Log.Error("获取用户信息异常..."+ex.Message+"\r\n"+ex.StackTrace+"\r\nSQL:"+sqlString);
                dataSet = null;
                return QueryResult.EXCEPT_ERR;
            }
        }
        #endregion

        #region 查询所有用户
        /// <summary>
        /// 查询所有用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public DataSet GetAllUserInfo()
        {
            try
            {
                SQLServer.SqlConnectionString = connectionString;
                string sqlString = "SELECT * " +
                            "FROM [WT_SCL].[dbo].[f_user] ";
                return SQLServer.ExecuteDataSet(sqlString);
            }
            catch
            {
                LogHelper.Log.Error("获取所有用户信息异常...");
                return null;
            }
        }
        #endregion

        #region 注册
        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <param name="phone"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public RegisterResult Register(string username, string pwd, string phone, string email, LoginUser loginUser)
        {
            try
            {
                DataSet dataSet;
                QueryResult queryResult = GetUserInfo(username,out dataSet);
                if (queryResult == QueryResult.EXIST_DATA)
                {
                    //用户已存在
                    return RegisterResult.REGISTER_EXIST_USER;
                }
                else if (queryResult == QueryResult.NONE_DATE)
                {
                    //用户不存在，可以注册
                    string dateTimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string insertString = "INSERT INTO [WT_SCL].[dbo].[f_user]" +
                        "([username]," +
                        "[password] ," +
                        "[phone]," +
                        "[email] ," +
                        "[picture]," +
                        "[create_date]," +
                        "[last_update_date] ," +
                        "[status]," +
                        "[user_type]) " +
                        $"VALUES('{username}', '{pwd}', '{phone}', '{email}', '', '', '{dateTimeNow}', '', '{(int)loginUser}')";
                    int executeResult = SQLServer.ExecuteNonQuery(insertString);
                    if (executeResult < 1)
                    {
                        return RegisterResult.REGISTER_FAIL_SQL;
                    }
                    return RegisterResult.REGISTER_SUCCESS;
                }
                //查询失败
                return RegisterResult.REGISTER_ERR;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error("注册失败..." + ex.Message);
                return  RegisterResult.REGISTER_ERR;
            }
        }
        #endregion

        #region 找回密码

        #endregion

        #region 验证传入工站是否可以生产/测试
        /// <summary>
        /// flash为首站，传入二维码后，查询二维码是否存在，不存在-则根据型号和二维码创建信息，存在-判断是不是在本站生产
        /// </summary>
        /// <param name="sn">追溯号/条码</param>
        /// <param name="sTypeNumber">型号/零件号</param>
        /// <param name="sStationName">工站名称/站位名称</param>
        public string Firstcheck(string sn, string sTypeNumber, string sStationName)
        {
            /*
             * 1、判断传入站是不是首站，首站的判断：根据设置的首站判断
             * 2、判断上一站是否通过
             * 3、判断传入站追溯号是否存在
             * 4、判断传入站型号是否存在
             * 5、判断传入站站位名称是否存在
             * 
             */
            //根据传入站-查询该站的产线流程中的上一站
            //
            return "";
        }
        #endregion

        #region 接收传入参数，保存数据到数据库
        /// <summary>
        /// 传递质检工位过站、生产工位过站信息给MES
        /// </summary>
        /// <param name="sn">追溯号/条码号</param>
        /// <param name="sTypeNumber">型号/零件号</param>
        /// <param name="sStationName">工站名称</param>
        /// <param name="sTestResult">测试结果：PASS/FAIL</param>
        /// <param name="sTime">测试日期</param>
        /// <returns></returns>
        public string InsertWIP(string sn, string sTypeNumber, string sStationName, string sTestResult, string sTime)
        {
            return "";
        }
        #endregion

        #region 产线站位增删改查

        /// <summary>
        /// 配置产线包含哪些站位，按顺序插入
        /// </summary>
        /// <param name="dctData"></param>
        /// <returns>成功返回1，失败返回0+空格+序号+键+空格+值</returns>
        public string InsertProduce(Dictionary<int,string> dctData)
        {
            LogHelper.Log.Info($"接口被调用-InsertProduce");
            foreach (var item in dctData.Keys)
            {
                string value = dctData[item];
                //插入数据库
                //插入SQL
                if (IsExistProduceID(item) && !IsExistProduceStation(value))
                {
                    //ID已存在
                    //update
                    LogHelper.Log.Info("ID is exist,name is not exist, InsertProduce Excute UpdateProduceDB...");
                    UpdateProduceDB(item, value, item.ToString(), "");
                }
                else if (!IsExistProduceID(item) && IsExistProduceStation(value))
                {
                    //name exist
                    LogHelper.Log.Info("ID is not exist ,name is exist,InsertProduce Excute UpdateProduceDB...");
                    UpdateProduceDB(item, value, "", value);
                }
                else if (IsExistProduceID(item) && IsExistProduceStation(value))
                {
                    LogHelper.Log.Info("ID is exist,name is exist, InsertProduce Excute UpdateProduceDB...");
                    UpdateProduceDB(item, value,item.ToString(), value);
                }
                else
                {
                    //不存在，插入
                    string insertSQL = "INSERT INTO [WT_SCL].[dbo].[Produce_Process] " +
                    "([Station_Order],[Station_Name]) " +
                    $"VALUES('{item}','{value}')";

                    int res = SQLServer.ExecuteNonQuery(insertSQL);
                    if (res < 1)
                    {
                        //插入失败
                        LogHelper.Log.Info($"insert fail {insertSQL}");
                        return "0" + $" {item} {dctData[item]}";
                    }
                }
            }
            return "1";
        }

        /// <summary>
        /// 查询当前产线的站位流程
        /// </summary>
        /// <returns></returns>
        public DataSet SelectProduce()
        {
            string selectSQL = "SELECT * FROM [WT_SCL].[dbo].[Produce_Process] ORDER BY [Station_Order]";
            return SQLServer.ExecuteDataSet(selectSQL);
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        /// <returns></returns>
        public int DeleteAllProduce()
        {
            string deleteSQL = "DELETE FROM [WT_SCL].[dbo].[Produce_Process]";
            return SQLServer.ExecuteNonQuery(deleteSQL);
        }

        /// <summary>
        /// 删除某条记录
        /// </summary>
        /// <param name="stationName"></param>
        /// <returns></returns>
        public int DeleteProduce(string stationName)
        {
            string deleteSQL = $"DELETE FROM [WT_SCL].[dbo].[Produce_Process] WHERE [Station_Name] = '{stationName}'";
            LogHelper.Log.Info($"deleteSQL={deleteSQL}");
            return SQLServer.ExecuteNonQuery(deleteSQL);
        }

        /// <summary>
        /// 更新产线数据表《接口函数》
        /// </summary>
        /// <param name="data">键值集合</param>
        /// <returns></returns>
        public string UpdateProduce(Dictionary<int,string> data)
        {
            foreach (var item in data.Keys)
            {
                string v = data[item];
                
                if (IsExistProduceID(item))
                {
                    //更新
                    if (!UpdateProduceDB(item, v,item.ToString(),v))
                    {
                        return "0";
                    }
                }
            }
            return "1";
        }

        /// <summary>
        /// 产线序号是否为空
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsExistProduceID(int id)
        {
            string selectSQL = "SELECT  * " +
                    "FROM [WT_SCL].[dbo].[Produce_Process] " +
                    $"WHERE [Station_Order] = '{id}'";
            LogHelper.Log.Info($"ExistID = {selectSQL}");
            DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 产线站位名称是否为空
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsExistProduceStation(string name)
        {
            string selectSQL = "SELECT  * " +
                    "FROM [WT_SCL].[dbo].[Produce_Process] " +
                    $"WHERE [Station_Name] = '{name}'";
            LogHelper.Log.Info($"ExistStation = {selectSQL}");
            DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 执行更新数据库产线站位表
        /// </summary>
        /// <param name="order"></param>
        /// <param name="stationName"></param>
        /// <returns></returns>
        private bool UpdateProduceDB(int order,string stationName,string oldOrder,string oldName)
        {
            string updateSQL = "UPDATE [WT_SCL].[dbo].[Produce_Process] " +
                "SET " +
                $"[Station_Order]='{order}' ,[Station_Name]='{stationName}' " +
                $"WHERE " +
                $"[Station_Name] = '{oldName}' or [Station_Order] = '{oldOrder}'";
            int r = SQLServer.ExecuteNonQuery(updateSQL);
            LogHelper.Log.Info($"update={updateSQL}");
            if (r > 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        #region 产品型号增删改查

        public string CommitProductType(Dictionary<int, string> dctData)
        {
            LogHelper.Log.Info($"接口被调用-InsertProductType");
            foreach (var item in dctData.Keys)
            {
                string value = dctData[item];
                //插入数据库
                //插入SQL
                if (IsExistProductType(item))
                {
                    //ID已存在
                    //update
                    LogHelper.Log.Info("productName is exist, InsertProduce Excute UpdateProduceDB...");
                    UpdateProductType(item, value, "");
                }
                else
                {
                    //不存在，插入
                    string insertSQL = "INSERT INTO [WT_SCL].[dbo].[Product_Type] " +
                    "([Product_ID],[Product_Name]) " +
                    $"VALUES('{item}','{value}')";

                    int res = SQLServer.ExecuteNonQuery(insertSQL);
                    if (res < 1)
                    {
                        //插入失败
                        LogHelper.Log.Info($"product type table insert fail {insertSQL}");
                        return "0" + $" {item} {dctData[item]}";
                    }
                }
            }
            return "1";
        }

        /// <summary>
        /// 产品型号是否为空
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsExistProductType(int id)
        {
            string selectSQL = "SELECT  * " +
                    "FROM [WT_SCL].[dbo].[Product_Type] " +
                    $"WHERE [Product_Name] = '{id}'";
            LogHelper.Log.Info($"ExistProductName = {selectSQL}");
            DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 执行更新数据库产品型号表
        /// </summary>
        /// <param name="order"></param>
        /// <param name="stationName"></param>
        /// <returns></returns>
        private bool UpdateProductType(int ID, string productName,string oldProductName)
        {
            string updateSQL = "UPDATE [WT_SCL].[dbo].[Product_Type] " +
                "SET " +
                $"[Product_ID]='{ID}' ,[Station_Name]='{productName}' " +
                $"WHERE " +
                $"[Station_Name] = '{oldProductName}'";
            int r = SQLServer.ExecuteNonQuery(updateSQL);
            LogHelper.Log.Info($"UpdateProductType={updateSQL}");
            if (r > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 查询当前产线的站位流程
        /// </summary>
        /// <returns></returns>
        public DataSet SelectProductType()
        {
            string selectSQL = "SELECT * FROM [WT_SCL].[dbo].[Product_Type] ORDER BY [Product_ID]";
            LogHelper.Log.Info($"SelectProductType={selectSQL}");
            return SQLServer.ExecuteDataSet(selectSQL);
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        /// <returns></returns>
        public int DeleteAllProductType()
        {
            string deleteSQL = "DELETE FROM [WT_SCL].[dbo].[Product_Type]";
            return SQLServer.ExecuteNonQuery(deleteSQL);
        }

        /// <summary>
        /// 删除某条记录
        /// </summary>
        /// <param name="stationName"></param>
        /// <returns></returns>
        public int DeleteProductType(string productName)
        {
            string deleteSQL = $"DELETE FROM [WT_SCL].[dbo].[Product_Type] WHERE [Product_Name] = '{productName}'";
            LogHelper.Log.Info($"DeleteProductType={deleteSQL}");
            return SQLServer.ExecuteNonQuery(deleteSQL);
        }
        #endregion

    }
}
