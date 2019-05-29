using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using CommonUtils.Logger;
using FigKeyLoggerWcf.DB;
using FigKeyLoggerWcf.Molde;

namespace FigKeyLoggerWcf
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的类名“Service1”。
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class FigKeyLoggerService : IFigKeyLoggerService
    {
        public FigKeyLoggerService()
        {

        }

        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        #region getdatausingdatacontract
        /// <summary>
        /// 
        /// </summary>
        /// <param name="composite"></param>
        /// <returns></returns>
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
        #endregion

        #region 用户登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="username">用户名/手机号/邮箱</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public LoginResult Login(string username, string password, LoginUser loginUser)
        {
            try
            {
                using (var db = new SQLServerDataContext()) 
                {
                    var entity = (from x in db.f_user
                                  where (x.username == username || x.phone == username || x.email == username) && x.password == password
                                  select x).SingleOrDefault() ?? null;
                    db.SubmitChanges();
                    if (null == entity)
                    {
                        LogHelper.Log.Info("用户名或密码错误");
                        return LoginResult.USER_NAME_PWD_ERR;
                    }
                    //entity.LastLoginTime = DateTime.Now;
                    LogHelper.Log.Info(entity.user_id+" "+entity.username+" 登录进入 "+DateTime.Now);
                    return LoginResult.SUCCESS;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error("用户登录异常..."+ex.Message);
                return LoginResult.FAIL_EXCEP;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public f_user GetUserInfo(string userName)
        {
            try
            {
                using (var db = new SQLServerDataContext())
                {
                    var entity = (from x in db.f_user
                                  where x.username == userName
                                  select x).SingleOrDefault() ?? null;
                    return entity;
                }
            }
            catch
            {
                LogHelper.Log.Error("获取用户信息异常...");
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
        private bool Register(string username,string pwd,string phone,string email)
        {
            try
            {
                using (var db = new SQLServerDataContext())
                {
                    f_user option = null;
                    option = (from x in db.f_user
                              where x.username == username || x.phone == phone || x.email == email
                              select x).SingleOrDefault() ?? null;
                    if (null != option)
                    {
                        option.username = username;
                        option.password = pwd;
                        option.email = email;
                        option.phone = phone;
                        option.create_date = DateTime.Now;
                        option.last_update_date = DateTime.Now;
                    }
                    else
                    {
                        option = new f_user()
                        {
                            username = username,
                            password = pwd,
                            phone = phone,
                            email = email,
                            create_date = DateTime.Now,
                            last_update_date = DateTime.Now
                        };
                        db.f_user.InsertOnSubmit(option);
                    }
                    db.SubmitChanges();
                    return true;
                } 
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error("注册失败..."+ex.Message);
                return false;
            }
        }
        #endregion

        #region 找回密码
        /// <summary>
        /// 查询用户是否存在
        /// username  \  phone  \ email 返回为true时，验证成功
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private bool ExistUser(string username)
        {
            try
            {
                using (var db = new SQLServerDataContext())
                {
                    f_user user = null;
                    user = (from x in db.f_user
                                  where x.username == username || x.phone == username || x.email == username
                                  select x).SingleOrDefault() ?? null;
                    if (user == null)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error("用户不存在 "+ex.Message);
                return false;
            }
        }
        #endregion

        #region SQLServer 连接
        /// <summary>
        /// 连接SQL Server
        /// </summary>
        /// <returns></returns>
        public bool SQLConnection()
        {
            SQLServerDataContext db = new SQLServerDataContext();
            try
            {
                db.Connection.Open();
                LogHelper.Log.Info("SQLServer connection successful!");
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error("无法连接SQL Server数据库\r" + ex.Message);
                return false;
            }
            finally
            {
                db.Dispose();
            }
        }
        #endregion

        #region 数据存储
        //public bool SaveOption(string option_name, string option_value)
        //{
        //    try
        //    {
        //        using (dbmls.BasicDataContext db = new dbmls.BasicDataContext())
        //        {
        //            dbmls.Options option = null;
        //            option = (from x in db.Options
        //                      where x.OptionName == option_name && x.UserID == 0
        //                      select x).SingleOrDefault() ?? null;
        //            if (null != option)
        //            {
        //                option.OptionValue = option_value;
        //                option.UpdateTime = DateTime.Now;
        //            }
        //            else
        //            {
        //                option = new Options()
        //                {
        //                    OptionName = option_name,
        //                    OptionValue = option_value,
        //                    UpdateTime = DateTime.Now,
        //                    CreateTime = DateTime.Now,
        //                    UserID = 0,
        //                };
        //                db.Options.InsertOnSubmit(option);
        //            }
        //            db.SubmitChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public string GetOptionValue(string option_name)
        //{
        //    try
        //    {
        //        using (dbmls.BasicDataContext db = new dbmls.BasicDataContext())
        //        {
        //            dbmls.Options option = null;
        //            option = (from x in db.Options
        //                      where x.OptionName == option_name && x.UserID == 0
        //                      select x).SingleOrDefault() ?? null;
        //            if (null != option)
        //            {
        //                return option.OptionValue;
        //            }
        //        }
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }
        //}


        //public bool SaveOptionByUser(string option_name, string option_value, int userid)
        //{
        //    try
        //    {
        //        using (dbmls.BasicDataContext db = new dbmls.BasicDataContext())
        //        {
        //            dbmls.Options option = null;
        //            option = (from x in db.Options
        //                      where x.OptionName == option_name && x.UserID == userid
        //                      select x).SingleOrDefault() ?? null;
        //            if (null != option)
        //            {
        //                option.OptionValue = option_value;
        //                option.UpdateTime = DateTime.Now;
        //            }
        //            else
        //            {
        //                option = new Options()
        //                {
        //                    OptionName = option_name,
        //                    OptionValue = option_value,
        //                    UpdateTime = DateTime.Now,
        //                    CreateTime = DateTime.Now,
        //                    UserID = userid
        //                };
        //                db.Options.InsertOnSubmit(option);
        //            }
        //            db.SubmitChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        //public string GetOptionValueByUser(string option_name, int userid)
        //{
        //    try
        //    {
        //        using (dbmls.BasicDataContext db = new dbmls.BasicDataContext())
        //        {
        //            dbmls.Options option = null;
        //            option = (from x in db.Options
        //                      where x.OptionName == option_name && x.UserID == userid
        //                      select x).SingleOrDefault() ?? null;
        //            if (null != option)
        //            {
        //                return option.OptionValue;
        //            }
        //        }
        //        return "";
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }
        //}
        #endregion
    }
}
