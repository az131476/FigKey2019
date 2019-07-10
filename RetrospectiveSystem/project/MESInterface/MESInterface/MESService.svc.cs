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
using MESInterface.MessageQueue.RemoteClient;
using MESInterface.DB;
using MESInterface.Model;

namespace MESInterface
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“Service1”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 Service1.svc 或 Service1.svc.cs，然后开始调试。
    public class MesService : IMesService
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        private Queue<string[]> fcQueue = new Queue<string[]>();
        private Queue<string[]> insertDataQueue = new Queue<string[]>();
        private Queue<string[]> selectDataQueue = new Queue<string[]>();
        private Queue<string[]> insertMaterialStatistics = new Queue<string[]>();

        private string GetDateTimeNow()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        #region 用户信息接口

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
                QueryResult queryResult = GetUserInfo(username, out dataSet);
                if (queryResult == QueryResult.NONE_DATE)
                {
                    //用户不存在
                    LogHelper.Log.Info($"用户名{username}不存在，验证失败！");
                    return LoginResult.USER_NAME_ERR;
                }
                else if (queryResult == QueryResult.EXIST_DATA)
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
                        LogHelper.Log.Info(username + " 登录进入 " + DateTime.Now);
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
            catch (Exception ex)
            {
                LogHelper.Log.Error("获取用户信息异常..." + ex.Message + "\r\n" + ex.StackTrace + "\r\nSQL:" + sqlString);
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
                QueryResult queryResult = GetUserInfo(username, out dataSet);
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
                return RegisterResult.REGISTER_ERR;
            }
        }
        #endregion

        #region 找回密码

        #endregion

        #endregion

        #region 站位信息接口

        /// <summary>
        /// 配置产线包含哪些站位，按顺序插入
        /// </summary>
        /// <param name="dctData"></param>
        /// <returns>成功返回1，失败返回0+空格+序号+键+空格+值</returns>
        public string InsertStation(Dictionary<int, string> dctData)
        {
            LogHelper.Log.Info($"接口被调用-InsertProduce");
            foreach (var item in dctData.Keys)
            {
                string value = dctData[item];
                //插入数据库
                //插入SQL
                if (IsExistStationID(item) && !IsExistStationName(value))
                {
                    //ID已存在
                    //update
                    LogHelper.Log.Info("ID is exist,name is not exist, InsertProduce Excute UpdateProduceDB...");
                    UpdateStationDB(item, value, item.ToString(), "");
                }
                else if (!IsExistStationID(item) && IsExistStationName(value))
                {
                    //name exist
                    LogHelper.Log.Info("ID is not exist ,name is exist,InsertProduce Excute UpdateProduceDB...");
                    UpdateStationDB(item, value, "", value);
                }
                else if (IsExistStationID(item) && IsExistStationName(value))
                {
                    LogHelper.Log.Info("ID is exist,name is exist, InsertProduce Excute UpdateProduceDB...");
                    UpdateStationDB(item, value, item.ToString(), value);
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
        public DataSet SelectStation(string stationName, string stationOrder)
        {
            string selectSQL = "";
            if (string.IsNullOrEmpty(stationName) && string.IsNullOrEmpty(stationOrder))
            {
                selectSQL = $"SELECT * FROM [WT_SCL].[dbo].[Produce_Process] ORDER BY [Station_Order]";
            }
            else
            {
                selectSQL = $"SELECT * FROM [WT_SCL].[dbo].[Produce_Process] WHERE Station_Name = '{stationName}' " +
                $"or Station_Order ='{stationOrder}' ORDER BY [Station_Order]";
            }
            return SQLServer.ExecuteDataSet(selectSQL);
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        /// <returns></returns>
        public int DeleteAllStation()
        {
            string deleteSQL = "DELETE FROM [WT_SCL].[dbo].[Produce_Process]";
            return SQLServer.ExecuteNonQuery(deleteSQL);
        }

        /// <summary>
        /// 删除某条记录
        /// </summary>
        /// <param name="stationName"></param>
        /// <returns></returns>
        public int DeleteStation(string stationName)
        {
            string deleteSQL = $"DELETE FROM [WT_SCL].[dbo].[Produce_Process] WHERE [Station_Name] = '{stationName}'";
            LogHelper.Log.Info($"deleteSQL={deleteSQL}");
            return SQLServer.ExecuteNonQuery(deleteSQL);
        }

        /// <summary>
        /// 产线序号是否为空
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsExistStationID(int id)
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
        private bool IsExistStationName(string name)
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
        /// 更新产线数据表《接口函数》
        /// </summary>
        /// <param name="data">键值集合</param>
        /// <returns></returns>
        public string UpdateStation(Dictionary<int, string> data)
        {
            foreach (var item in data.Keys)
            {
                string v = data[item];

                if (IsExistStationID(item))
                {
                    //更新
                    if (!UpdateStationDB(item, v, item.ToString(), v))
                    {
                        return "0";
                    }
                }
            }
            return "1";
        }

        /// <summary>
        /// 执行更新数据库产线站位表
        /// </summary>
        /// <param name="order"></param>
        /// <param name="stationName"></param>
        /// <returns></returns>
        private bool UpdateStationDB(int order, string stationName, string oldOrder, string oldName)
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

        #region 型号信息接口

        public string CommitProductType(List<string> list)
        {
            LogHelper.Log.Info($"接口被调用-InsertProductType");
            foreach (var item in list)
            {
                if (!IsExistProductType(item))
                {
                    //insert
                    if (InsertProductType(item) < 1)
                        return "0";
                }
            }
            return "1";
        }

        private int InsertProductType(string typeNo)
        {
            string insertSQL = $"INSERT INTO {DbTable.F_PRODUCT_TYPE_NO_NAME}({DbTable.F_TypeNo.TYPE_NO}) " +
                $"VALUES('{typeNo}')";
            return SQLServer.ExecuteNonQuery(insertSQL);
        }

        /// <summary>
        /// 产品型号是否为空
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool IsExistProductType(string typeNo)
        {
            string selectSQL = $"SELECT * FROM {DbTable.F_PRODUCT_TYPE_NO_NAME} WHERE {DbTable.F_TypeNo.TYPE_NO} = '{typeNo}'";
            DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 查询当前产线的站位流程
        /// </summary>
        /// <returns></returns>
        public DataSet SelectProductType(string typeNumber)
        {
            string selectSQL = "";
            if (string.IsNullOrEmpty(typeNumber.Trim()))
            {
                selectSQL = $"SELECT * FROM [WT_SCL].[dbo].[Product_Type] ORDER BY [Product_ID]";
            }
            else
            {
                selectSQL = $"SELECT * FROM [WT_SCL].[dbo].[Product_Type] WHERE [Type_Number] like '%{typeNumber}%' ORDER BY [Product_ID]";
            }

            LogHelper.Log.Info($"SelectProductType={selectSQL}");
            return SQLServer.ExecuteDataSet(selectSQL);
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        /// <returns></returns>
        public int DeleteAllProductType()
        {
            string deleteSQL = $"DELETE FROM {DbTable.F_PRODUCT_TYPE_NO_NAME}";
            return SQLServer.ExecuteNonQuery(deleteSQL);
        }

        /// <summary>
        /// 删除某条记录
        /// </summary>
        /// <param name="stationName"></param>
        /// <returns></returns>
        public int DeleteProductType(string typeNo)
        {
            string deleteSQL = $"DELETE FROM {DbTable.F_PRODUCT_TYPE_NO_NAME} WHERE {DbTable.F_TypeNo.TYPE_NO} = '{typeNo}'";
            return SQLServer.ExecuteNonQuery(deleteSQL);
        }
        #endregion

        #region 测试结果数据接口
        public string InsertTestResultData(string sn, string typeNo, string station, string dateTime, string result)
        {
            string[] array = new string[] { sn, typeNo, station, dateTime, result };
            insertDataQueue.Enqueue(array);
            return TestResult.InsertTestResult(insertDataQueue);
        }

        public string SelectLastTestResult(string sn, string typeNo, string station)
        {
            string[] array = new string[] { sn, typeNo, station };
            selectDataQueue.Enqueue(array);
            return TestResult.SelectTestResult(selectDataQueue);
        }

        /// <summary>
        /// 根据SN查询
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="IsSnFuzzy">true-sn为模糊查询，否则完全匹配</param>
        /// <returns></returns>
        public DataSet SelectTestResultOfSN(string sn, bool IsSnFuzzy)
        {
            //查询时返回
            string selectSQL = "";
            if (IsSnFuzzy)
            {
                selectSQL = $"SELECT {DbTable.F_Test_Result.SN},{DbTable.F_Test_Result.TYPE_NO}," +
                $"{DbTable.F_Test_Result.STATION_NAME},{DbTable.F_Test_Result.TEST_RESULT}," +
                $"{DbTable.F_Test_Result.UPDATE_DATE},{DbTable.F_Test_Result.REMARK} " +
                $"FROM {DbTable.F_TEST_RESULT_NAME} " +
                $"WHERE {DbTable.F_Test_Result.SN} like '%{sn}%'";
            }
            else
            {
                selectSQL = $"SELECT {DbTable.F_Test_Result.SN},{DbTable.F_Test_Result.TYPE_NO}," +
                $"{DbTable.F_Test_Result.STATION_NAME},{DbTable.F_Test_Result.TEST_RESULT}," +
                $"{DbTable.F_Test_Result.UPDATE_DATE},{DbTable.F_Test_Result.REMARK} " +
                $"FROM {DbTable.F_TEST_RESULT_NAME} " +
                $"WHERE {DbTable.F_Test_Result.SN} = '{sn}'";
            }
            return SQLServer.ExecuteDataSet(selectSQL);
        }

        /// <summary>
        /// 根据型号查询
        /// </summary>
        /// <param name="typeNo"></param>
        /// <returns></returns>
        public DataSet SelectTestResultOfTypeNo(string typeNo)
        {
            string selectSQL = $"SELECT {DbTable.F_Test_Result.SN},{DbTable.F_Test_Result.TYPE_NO}," +
                $"{DbTable.F_Test_Result.STATION_NAME},{DbTable.F_Test_Result.TEST_RESULT}," +
                $"{DbTable.F_Test_Result.UPDATE_DATE},{DbTable.F_Test_Result.REMARK} " +
                $"FROM {DbTable.F_TEST_RESULT_NAME} " +
                $"WHERE {DbTable.F_Test_Result.TYPE_NO} like '%{typeNo}%'";
            return SQLServer.ExecuteDataSet(selectSQL);
        }
        #endregion

        #region 物料信息表
        public string CommitMaterial(List<MaterialMsg> list)
        {
            foreach (var item in list)
            {
                if (IsExistMaterial(item.MaterialCode))
                {
                    //update
                    if (InsertMaterial(item) < 1)
                        return "I0";//插入失败
                }
                else
                {
                    //insert
                    if (UpdateMaterial(item) < 1)
                        return "G0";//更新失败
                }
            }
            return "1";
        }

        public int DeleteMaterial(string materialCode)
        {
            string deleteSQL = $"DELETE FROM {DbTable.F_MATERIAL_NAME} " +
                $"WHERE {DbTable.F_Material.MATERIAL_CODE} = '{materialCode}'";
            return SQLServer.ExecuteNonQuery(deleteSQL);
        }

        private bool IsExistMaterial(string materialCode)
        {
            string selectSQL = $"SELECT * FROM {DbTable.F_MATERIAL_NAME} WHERE {DbTable.F_Material.MATERIAL_CODE} = '{materialCode}'";
            DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        private int InsertMaterial(MaterialMsg material)
        {
            string insertSQL = $"INSERT INTO {DbTable.F_MATERIAL_NAME}() VALUES('{material.MaterialCode}','{material.MaterialAmount}')";
            return SQLServer.ExecuteNonQuery(insertSQL);
        }
        private int UpdateMaterial(MaterialMsg material)
        {
            string updateSQL = $"UPDATE {DbTable.F_MATERIAL_NAME} SET {DbTable.F_Material.MATERIAL_AMOUNT} = '{material.MaterialAmount}' " +
                $"WHERE {DbTable.F_Material.MATERIAL_CODE} = '{material.MaterialCode}')";
            return SQLServer.ExecuteNonQuery(updateSQL);
        }
        #endregion

        #region 产品物料配置
        public string CommitProductMaterial(Dictionary<string, List<string>> keyValuePairs)
        {
            foreach (KeyValuePair<string, List<string>> kv in keyValuePairs)
            {
                foreach (var v in kv.Value)
                {
                    if (IsExistProductMaterial(kv.Key, v))
                    {
                        //update
                        if (InsertProductMaterial(kv.Key, v) < 1)
                            return "I0";//插入失败
                    }
                    else
                    {
                        //insert
                        if (UpdateProductMaterial(kv.Key, v) < 1)
                            return "G0";//更新失败
                    }
                }
            }
            return "1";
        }

        public int DeleteProductMaterial(string typeNo, string materialCode)
        {
            string deleteSQL = $"DELETE FROM {DbTable.F_PRODUCT_MATERIAL_NAME} " +
                $"WHERE {DbTable.F_PRODUCT_MATERIAL.TYPE_NO} = '{typeNo}' AND " +
                $"{DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE} = '{materialCode}'";
            return SQLServer.ExecuteNonQuery(deleteSQL);
        }
        private bool IsExistProductMaterial(string typeNo, string materialCode)
        {
            string selectSQL = $"SELECT * FROM {DbTable.F_PRODUCT_MATERIAL_NAME} " +
                $"WHERE {DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE} = '{materialCode}' AND " +
                $"{DbTable.F_PRODUCT_MATERIAL.TYPE_NO} = '{typeNo}'";
            DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        private int InsertProductMaterial(string typeNo, string materialCode)
        {
            string insertSQL = $"INSERT INTO {DbTable.F_PRODUCT_MATERIAL_NAME}() VALUES('{typeNo}','{materialCode}')";
            return SQLServer.ExecuteNonQuery(insertSQL);
        }
        private int UpdateProductMaterial(string typeNo, string materialCode)
        {
            string updateSQL = $"UPDATE {DbTable.F_PRODUCT_MATERIAL_NAME} SET {DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE} = '{materialCode}')";
            return SQLServer.ExecuteNonQuery(updateSQL);
        }
        #endregion

        #region 物料统计表
        /// <summary>
        /// 测试端传入装配消耗物料计数
        /// </summary>
        /// <param name="snInner"></param>
        /// <param name="snOutter"></param>
        /// <param name="typeNo"></param>
        /// <param name="stationName"></param>
        /// <param name="materialCode"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public string InsertMaterialStatistics(string snInner, string snOutter, string typeNo, string stationName, string materialCode, string amount)
        {
            string[] array = new string[] { snInner, snOutter, typeNo, stationName, materialCode, amount };
            insertMaterialStatistics.Enqueue(array);
            return MaterialStatistics.InsertMaterialStatistics(insertMaterialStatistics);
        }
        public DataSet SelectMaterialStatistics(string typeNo)
        {
            string selectSQL = "";
            if (!string.IsNullOrEmpty(typeNo))
            {
                selectSQL = $"SELECT {DbTable.F_Material_Statistics.SN_INNER},{DbTable.F_Material_Statistics.SN_OUTTER}," +
                    $"{DbTable.F_Material_Statistics.TYPE_NO},{DbTable.F_Material_Statistics.MATERIAL_CODE}," +
                    $"SUM({DbTable.F_Material_Statistics.MATERIAL_AMOUNT} as amount) " +
                    $"FROM {DbTable.F_MATERIAL_STATISTICS_NAME} " +
                    $"WHERE {DbTable.F_Material_Statistics.TYPE_NO} = '{typeNo}' " +
                    $"GROUP BY {DbTable.F_Material_Statistics.SN_INNER},{DbTable.F_Material_Statistics.SN_OUTTER}," +
                    $"{DbTable.F_Material_Statistics.TYPE_NO},{DbTable.F_Material_Statistics.MATERIAL_CODE}";
            }
            else
            {
                selectSQL = $"SELECT * FROM {DbTable.F_MATERIAL_STATISTICS_NAME}";
            }
            return SQLServer.ExecuteDataSet(selectSQL);
        }
        #endregion

        #region 产品合格率统计接口
        #endregion

        #region 验证传入工站是否可以生产/测试
        /// <summary>
        /// flash为首站，传入二维码后，查询二维码是否存在，不存在-则根据型号和二维码创建信息，存在-判断是不是在本站生产
        /// </summary>
        /// <param name="sn">追溯号/条码</param>
        /// <param name="sTypeNumber">型号/零件号</param>
        /// <param name="sStationName">工站名称/站位名称</param>
        public string FirstCheck(string sn_inner, string sn_outter, string sTypeNumber, string sStationName)
        {
            //加入队列
            string[] array = new string[] { sn_inner, sn_outter, sTypeNumber, sStationName };
            fcQueue.Enqueue(array);
            LogHelper.Log.Info($"FirstCheck接口被调用，传入参数[{sn_inner},{sn_outter},{sTypeNumber},{sStationName}] 当前队列count={fcQueue.Count}");
            return FirstCheckQueue.CheckPass(fcQueue);
        }

        #endregion

        #region 型号所属站位增删改查
        public string CommitTypeStation(Dictionary<string, string[]> dctData)
        {
            LogHelper.Log.Info($"接口被调用-CommitTypeStation");
            foreach (var typeNumber in dctData.Keys)
            {
                string[] arrayStation = dctData[typeNumber];
                //插入数据库
                //插入SQL
                if (IsExistTypeStation(typeNumber))
                {
                    //ID已存在
                    //update
                    LogHelper.Log.Info("product type_number is exist, Excute UpdateProduceDB...");
                    UpdateTypeStation(typeNumber, arrayStation);
                }
                else
                {
                    //不存在，插入
                    LogHelper.Log.Info($"product type number is not exist value={typeNumber}, Excute Insert Into Table...");
                    string insertSQL = "INSERT INTO [WT_SCL].[dbo].[Product_Process]" +
                            "([Type_Number],[Station_Name_1],[Station_Name_2],[Station_Name_3],[Station_Name_4],[Station_Name_5]," +
                            "[Station_Name_6],[Station_Name_7],[Station_Name_8],[Station_Name_9],[Station_Name_10]) " +
                            $"VALUES('{typeNumber}','{arrayStation[0]}','{arrayStation[1]}','{arrayStation[2]}','{arrayStation[3]}'," +
                            $"'{arrayStation[4]}','{arrayStation[5]}','{arrayStation[6]}','{arrayStation[7]}','{arrayStation[8]}','{arrayStation[9]}')";

                    int res = SQLServer.ExecuteNonQuery(insertSQL);
                    if (res < 1)
                    {
                        //插入失败
                        LogHelper.Log.Info($"product type number table insert fail {insertSQL}");
                        return "0" + $" {typeNumber}";
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
        private bool IsExistTypeStation(string typeNumber)
        {
            string selectSQL = "SELECT  * " +
                    "FROM [WT_SCL].[dbo].[Product_Process] " +
                    $"WHERE [Type_Number] = '{typeNumber}'";
            LogHelper.Log.Info($"Exist Product type number = {selectSQL}");
            DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 执行更新数据库产品型号所属站位表
        /// </summary>
        /// <param name="order"></param>
        /// <param name="stationName"></param>
        /// <returns></returns>
        private bool UpdateTypeStation(string typeNumber, string[] arrayStation)
        {
            string updateSQL = "UPDATE [WT_SCL].[dbo].[Product_Process] " +
                $"SET Station_Name_1 = '{arrayStation[0]}',Station_Name_2='{arrayStation[1]}',Station_Name_3='{arrayStation[2]}'," +
                $"Station_Name_4='{arrayStation[3]}',Station_Name_5='{arrayStation[4]}',Station_Name_6 = '{arrayStation[5]}'," +
                $"Station_Name_7 = '{arrayStation[6]}',Station_Name_8 = '{arrayStation[7]}',Station_Name_9 = '{arrayStation[8]}',Station_Name_10 = '{arrayStation[9]}' " +
                $"WHERE Type_Number = '{typeNumber}'";
            int r = SQLServer.ExecuteNonQuery(updateSQL);
            LogHelper.Log.Info($"Update Product Type Number={updateSQL}");
            if (r > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 查询当前型号所属站位列表
        /// </summary>
        /// <returns></returns>
        public DataSet SelectTypeStation(string typeNumber)
        {
            string selectSQL = "";
            if (string.IsNullOrEmpty(typeNumber.Trim()))
            {
                selectSQL = $"SELECT * FROM [WT_SCL].[dbo].[Product_Process] ORDER BY [Type_Number]";
            }
            else
            {
                selectSQL = $"SELECT * FROM [WT_SCL].[dbo].[Product_Process] WHERE [Type_Number] = '{typeNumber}' ORDER BY [Type_Number]";
            }

            LogHelper.Log.Info($"Select Product Type Number={selectSQL}");
            return SQLServer.ExecuteDataSet(selectSQL);
        }

        /// <summary>
        /// 清除所有数据
        /// </summary>
        /// <returns></returns>
        public int DeleteAllTypeStation()
        {
            string deleteSQL = "DELETE FROM [WT_SCL].[dbo].[Product_Process]";
            return SQLServer.ExecuteNonQuery(deleteSQL);
        }

        /// <summary>
        /// 删除某条记录
        /// </summary>
        /// <param name="stationName"></param>
        /// <returns></returns>
        public int DeleteTypeStation(string typeNumber)
        {
            string deleteSQL = $"DELETE FROM [WT_SCL].[dbo].[Product_Process] WHERE [Type_Number] = '{typeNumber}'";
            LogHelper.Log.Info($"DeleteProductType={deleteSQL}");
            return SQLServer.ExecuteNonQuery(deleteSQL);
        }
        #endregion

        #region 成品打包接口
        public int CommitPackageProduct(PackageProduct packageProduct)
        {
            string insertSQL = $"INSERT INTO {DbTable.F_OUT_CASE_PRODUCT_NAME}() " +
                $"VALUES('{packageProduct.CaseCode}','{packageProduct.SnOutter}','{packageProduct.TypeNo}','{packageProduct.Picture}'," +
                $"'{packageProduct.BindingState}','{packageProduct.BindingDate}')";
            if (IsExistPackageProduct(packageProduct.CaseCode, packageProduct.SnOutter))
            {
                //update
                return UpdatePackageProduct(packageProduct);
            }
            else
            {
                //insert
                return SQLServer.ExecuteNonQuery(insertSQL);
            }
        }
        private bool IsExistPackageProduct(string caseCode, string snOutter)
        {
            string selectSQL = $"SELECT * FROM {DbTable.F_OUT_CASE_PRODUCT_NAME} " +
                $"WHERE {DbTable.F_Out_Case_Product.OUT_CASE_CODE} = '{caseCode}' AND " +
                $"{DbTable.F_Out_Case_Product.SN_OUTTER} = '{snOutter}'";
            DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }
        public int UpdatePackageProduct(PackageProduct packageProduct)
        {
            string updateSQL = $"UPDATE {DbTable.F_OUT_CASE_PRODUCT_NAME} SET " +
                $"{DbTable.F_Out_Case_Product.BINDING_STATE} = '{packageProduct.BindingState}' " +
                $"WHERE {DbTable.F_Out_Case_Product.OUT_CASE_CODE} = '{packageProduct.CaseCode}' AND " +
                $"{DbTable.F_Out_Case_Product.SN_OUTTER} = '{packageProduct.SnOutter}' ";
            return SQLServer.ExecuteNonQuery(updateSQL);
        }
        public DataSet SelectPackageProduct(PackageProduct packageProduct)
        {
            //箱子编码/追溯码查询
            string selectSQL = "";
            if (string.IsNullOrEmpty(packageProduct.CaseCode) && string.IsNullOrEmpty(packageProduct.SnOutter))
            {
                selectSQL = $"SELECT * FROM {DbTable.F_OUT_CASE_PRODUCT_NAME}";
            }
            else
            {
                selectSQL = $"SELECT * FROM {DbTable.F_OUT_CASE_PRODUCT_NAME} WHERE " +
                    $"{DbTable.F_Out_Case_Product.OUT_CASE_CODE} = '{packageProduct.CaseCode}' or " +
                    $"{DbTable.F_Out_Case_Product.SN_OUTTER} = '{packageProduct.SnOutter}'";
            }
            return SQLServer.ExecuteDataSet(selectSQL);
        }
        #endregion

        #region 外箱容量接口
        public int CommitOutCaseBoxStorage(string out_case_code, string amount)
        {
            string insertSQL = $"INSERT INTO() VALUES('{out_case_code}','{amount}')";
            if (IsExistOutCaseBoxStorage(out_case_code))
            {
                //update
                return UpdateOutCaseBoxStorage(out_case_code, amount);
            }
            else
            {
                //insert
                return SQLServer.ExecuteNonQuery(insertSQL);
            }
        }
        public int UpdateOutCaseBoxStorage(string out_case_code, string amount)
        {
            string updateSQL = $"UPDATE {DbTable.F_OUT_CASE_STORAGE_NAME} SET {DbTable.F_Out_Case_Storage.STORAGE_CAPACITY} = '{amount}' " +
                $"WHERE {DbTable.F_Out_Case_Storage.OUT_CASE_CODE} = '{out_case_code}'";
            return SQLServer.ExecuteNonQuery(updateSQL);
        }
        private bool IsExistOutCaseBoxStorage(string out_case_code)
        {
            string selectSQL = $"SELECT * FROM {DbTable.F_OUT_CASE_STORAGE_NAME} WHERE {DbTable.F_Out_Case_Storage.OUT_CASE_CODE} = '{out_case_code}'";
            DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }
        #endregion

        #region 产品设站接口
        #endregion
    }
}
