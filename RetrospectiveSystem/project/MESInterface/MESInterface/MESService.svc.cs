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
                    Hashtable ht = new Hashtable();
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
                    int executeResult = SQLServer.ExecuteNonQuery(ref ht,insertString,null);
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
    }
}
