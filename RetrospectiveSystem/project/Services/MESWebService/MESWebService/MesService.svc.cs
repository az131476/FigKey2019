﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Data;
using CommonUtils.DB;
using CommonUtils.Logger;
using System.Configuration;
using System.Collections;
using MESWebService.MessageQueue.RemoteClient;
using MESWebService.DB;
using MESWebService.Model;
using System.Data.SqlClient;
using SwaggerWcf.Attributes;

namespace MESWebService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“MesService”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 MesService.svc 或 MesService.svc.cs，然后开始调试。
    public class MesService : IMesService
    {
        private Queue<string[]> fcQueue = new Queue<string[]>();
        private Queue<string[]> insertDataQueue = new Queue<string[]>();
        private Queue<string[]> selectDataQueue = new Queue<string[]>();
        private Queue<string[]> insertMaterialStatistics = new Queue<string[]>();

        #region 用户信息接口

        //#region 用户登录
        ///// <summary>
        ///// 登录
        ///// </summary>
        ///// <param name="username">用户名/手机号/邮箱</param>
        ///// <param name="password">密码</param>
        ///// <returns></returns>
        //public LoginResult Login(string username, string password, LoginUser loginUser)
        //{
        //    //暂未处理用户角色
        //    try
        //    {
        //        DataSet dataSet;
        //        QueryResult queryResult = GetUserInfo(username, out dataSet);
        //        if (queryResult == QueryResult.NONE_DATE)
        //        {
        //            //用户不存在
        //            LogHelper.Log.Info($"用户名{username}不存在，验证失败！");
        //            return LoginResult.USER_NAME_ERR;
        //        }
        //        else if (queryResult == QueryResult.EXIST_DATA)
        //        {
        //            //用户存在
        //            //验证登录密码
        //            string sql = "SELECT * " +
        //                "FROM " +
        //                "[WT_SCL].[dbo].[f_user] " +
        //                "WHERE " +
        //                $"[password] = '{password}' ";
        //            DataTable dtRes = SQLServer.ExecuteDataSet(sql).Tables[0];
        //            if (dtRes.Rows.Count < 1)
        //            {
        //                //密码验证失败
        //                LogHelper.Log.Info($"用户{username}密码验证失败！");
        //                return LoginResult.USER_PWD_ERR;
        //            }
        //            else
        //            {
        //                //通过验证
        //                LogHelper.Log.Info(username + " 登录进入 " + DateTime.Now);
        //                return LoginResult.SUCCESS;
        //            }
        //        }
        //        return LoginResult.FAIL_EXCEP;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Log.Error("用户登录异常..." + ex.Message);
        //        return LoginResult.FAIL_EXCEP;
        //    }
        //}

        //#endregion

        //#region 查询用户信息
        ///// <summary>
        ///// 查询用户信息
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <returns></returns>
        //public QueryResult GetUserInfo(string username, out DataSet dataSet)
        //{
        //    string sqlString = "SELECT * " +
        //    "FROM [WT_SCL].[dbo].[f_user] " +
        //    "WHERE " +
        //    $"[username] = '{username}' or [phone] = '{username}' or [email] = '{username}' ";
        //    try
        //    {
        //        dataSet = SQLServer.ExecuteDataSet(sqlString);
        //        if (dataSet.Tables[0].Rows.Count < 1)
        //        {
        //            return QueryResult.NONE_DATE;
        //        }
        //        else
        //        {
        //            return QueryResult.EXIST_DATA;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Log.Error("获取用户信息异常..." + ex.Message + "\r\n" + ex.StackTrace + "\r\nSQL:" + sqlString);
        //        dataSet = null;
        //        return QueryResult.EXCEPT_ERR;
        //    }
        //}
        //#endregion

        //#region 查询所有用户
        ///// <summary>
        ///// 查询所有用户信息
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <returns></returns>
        //public DataSet GetAllUserInfo()
        //{
        //    try
        //    {
        //        string sqlString = $"SELECT {DbTable.F_User.USER_NAME} " +
        //                    "FROM [WT_SCL].[dbo].[f_user] ";
        //        return SQLServer.ExecuteDataSet(sqlString);
        //    }
        //    catch
        //    {
        //        LogHelper.Log.Error("获取所有用户信息异常...");
        //        return null;
        //    }
        //}
        //#endregion

        //#region 注册
        ///// <summary>
        ///// 用户注册
        ///// </summary>
        ///// <param name="username"></param>
        ///// <param name="pwd"></param>
        ///// <param name="phone"></param>
        ///// <param name="email"></param>
        ///// <returns></returns>
        //public RegisterResult Register(string username, string pwd, string phone, string email, LoginUser loginUser)
        //{
        //    try
        //    {
        //        DataSet dataSet;
        //        QueryResult queryResult = GetUserInfo(username, out dataSet);
        //        if (queryResult == QueryResult.EXIST_DATA)
        //        {
        //            //用户已存在
        //            return RegisterResult.REGISTER_EXIST_USER;
        //        }
        //        else if (queryResult == QueryResult.NONE_DATE)
        //        {
        //            //用户不存在，可以注册
        //            string dateTimeNow = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        //            string insertString = "INSERT INTO [WT_SCL].[dbo].[f_user]" +
        //                "([username]," +
        //                "[password] ," +
        //                "[phone]," +
        //                "[email] ," +
        //                "[picture]," +
        //                "[create_date]," +
        //                "[last_update_date] ," +
        //                "[status]," +
        //                "[user_type]) " +
        //                $"VALUES('{username}', '{pwd}', '{phone}', '{email}', '', '', '{dateTimeNow}', '', '{(int)loginUser}')";
        //            int executeResult = SQLServer.ExecuteNonQuery(insertString);
        //            if (executeResult < 1)
        //            {
        //                return RegisterResult.REGISTER_FAIL_SQL;
        //            }
        //            return RegisterResult.REGISTER_SUCCESS;
        //        }
        //        //查询失败
        //        return RegisterResult.REGISTER_ERR;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.Log.Error("注册失败..." + ex.Message);
        //        return RegisterResult.REGISTER_ERR;
        //    }
        //}
        //#endregion

        //#region 找回密码

        //#endregion

        #endregion

        #region 站位信息接口

        ///// <summary>
        ///// 配置产线包含哪些站位，按顺序插入
        ///// </summary>
        ///// <param name="dctData"></param>
        ///// <returns>成功返回1，失败返回0+空格+序号+键+空格+值</returns>
        //public int InsertStation(List<Station> stationList)
        //{
        //    LogHelper.Log.Info($"接口被调用-InsertProduce");
        //    foreach (var station in stationList)
        //    {
        //        if (!IsExistStation(station))
        //        {
        //            //不存在，插入
        //            string insertSQL = $"INSERT INTO {DbTable.F_STATION_NAME}({DbTable.F_Station.STATION_ORDER}," +
        //                $"{DbTable.F_Station.STATION_NAME}) " +
        //            $"VALUES('{station.StationID}','{station.StationName}')";
        //            LogHelper.Log.Info(insertSQL);
        //            if (SQLServer.ExecuteNonQuery(insertSQL) < 1)
        //                return 0;
        //        }
        //    }
        //    return 1;
        //}

        ///// <summary>
        ///// 查询当前产线的站位流程
        ///// </summary>
        ///// <returns></returns>
        //public DataSet SelectStation(string stationName, string stationOrder)
        //{
        //    string selectSQL = "";
        //    if (string.IsNullOrEmpty(stationName) && string.IsNullOrEmpty(stationOrder))
        //    {
        //        selectSQL = $"SELECT * FROM {DbTable.F_STATION_NAME} ORDER BY {DbTable.F_Station.STATION_ORDER}";
        //    }
        //    else
        //    {
        //        selectSQL = $"SELECT * FROM {DbTable.F_STATION_NAME} WHERE {DbTable.F_Station.STATION_NAME} = '{stationName}' OR " +
        //            $"{DbTable.F_Station.STATION_ORDER} ORDER BY {DbTable.F_Station.STATION_ORDER}";
        //    }
        //    return SQLServer.ExecuteDataSet(selectSQL);
        //}

        ///// <summary>
        ///// 删除某条记录
        ///// </summary>
        ///// <param name="stationName"></param>
        ///// <returns></returns>
        //public int DeleteStation(string stationName)
        //{
        //    string deleteSQL = $"DELETE FROM {DbTable.F_STATION_NAME} " +
        //        $"WHERE {DbTable.F_Station.STATION_NAME} = '{stationName}'";
        //    return SQLServer.ExecuteNonQuery(deleteSQL);
        //}

        ///// <summary>
        ///// 删除所有站位记录
        ///// </summary>
        ///// <returns></returns>
        //public int DeleteAllStation()
        //{
        //    string deleteSQL = $"DELETE FROM {DbTable.F_STATION_NAME} ";
        //    return SQLServer.ExecuteNonQuery(deleteSQL);
        //}

        ///// <summary>
        ///// 产线序号是否为空
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //private bool IsExistStation(Station station)
        //{
        //    string selectSQL = $"SELECT * FROM {DbTable.F_STATION_NAME} WHERE " +
        //        $"{DbTable.F_Station.STATION_NAME} = '{station.StationName}'";
        //    DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
        //    if (dt.Rows.Count > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        #endregion

        #region 型号信息接口

        //public string CommitProductTypeNo(List<string> list)
        //{
        //    LogHelper.Log.Info($"接口被调用-InsertProductType");
        //    foreach (var item in list)
        //    {
        //        if (!IsExistProductType(item))
        //        {
        //            //insert
        //            if (InsertProductType(item) < 1)
        //                return "0";
        //        }
        //    }
        //    return "1";
        //}

        //private int InsertProductType(string typeNo)
        //{
        //    string insertSQL = $"INSERT INTO {DbTable.F_PRODUCT_TYPE_NO_NAME}({DbTable.F_TypeNo.TYPE_NO}) " +
        //        $"VALUES('{typeNo}')";
        //    return SQLServer.ExecuteNonQuery(insertSQL);
        //}

        ///// <summary>
        ///// 产品型号是否为空
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //private bool IsExistProductType(string typeNo)
        //{
        //    string selectSQL = $"SELECT * FROM {DbTable.F_PRODUCT_TYPE_NO_NAME} WHERE {DbTable.F_TypeNo.TYPE_NO} = '{typeNo}'";
        //    DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
        //    if (dt.Rows.Count > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// 查询当前产线的站位流程
        ///// </summary>
        ///// <returns></returns>
        //public DataSet SelectProductTypeNo(string typeNo)
        //{
        //    string selectSQL = "";
        //    if (string.IsNullOrEmpty(typeNo.Trim()))
        //    {
        //        selectSQL = $"SELECT {DbTable.F_TypeNo.TYPE_NO} FROM {DbTable.F_PRODUCT_TYPE_NO_NAME} ";
        //    }
        //    else
        //    {
        //        selectSQL = $"SELECT {DbTable.F_TypeNo.TYPE_NO} FROM {DbTable.F_PRODUCT_TYPE_NO_NAME} WHERE {DbTable.F_TypeNo.TYPE_NO} like '%{typeNo}%'";
        //    }

        //    LogHelper.Log.Info($"SelectProductType={selectSQL}");
        //    return SQLServer.ExecuteDataSet(selectSQL);
        //}

        ///// <summary>
        ///// 清除所有数据
        ///// </summary>
        ///// <returns></returns>
        //public int DeleteAllProductTypeNo()
        //{
        //    string deleteSQL = $"DELETE FROM {DbTable.F_PRODUCT_TYPE_NO_NAME}";
        //    return SQLServer.ExecuteNonQuery(deleteSQL);
        //}

        ///// <summary>
        ///// 删除某条记录
        ///// </summary>
        ///// <param name="stationName"></param>
        ///// <returns></returns>
        //public int DeleteProductTypeNo(string typeNo)
        //{
        //    string deleteSQL = $"DELETE FROM {DbTable.F_PRODUCT_TYPE_NO_NAME} WHERE {DbTable.F_TypeNo.TYPE_NO} = '{typeNo}'";
        //    return SQLServer.ExecuteNonQuery(deleteSQL);
        //}
        #endregion

        #region 测试结果数据接口
        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse(HttpStatusCode.Created, "Book created, value in the response body with id updated")]
        [SwaggerWcfResponse(HttpStatusCode.BadRequest, "Bad request", true)]
        [SwaggerWcfResponse(HttpStatusCode.InternalServerError, "Internal error (can be forced using ERROR_500 as book title)", true)]
        /// <summary>
        /// 测试端插入测试结果
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="typeNo"></param>
        /// <param name="station"></param>
        /// <param name="dateTime"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string InsertTestResultData(string sn, string typeNo, string station, string dateTime, string result)
        {
            string[] array = new string[] { sn, typeNo, station, dateTime, result };
            insertDataQueue.Enqueue(array);
            return TestResult.InsertTestResult(insertDataQueue);
        }

        [SwaggerWcfTag("MesServcie 服务")]
        [SwaggerWcfResponse(HttpStatusCode.Created, "Book created, value in the response body with id updated")]
        [SwaggerWcfResponse(HttpStatusCode.BadRequest, "Bad request", true)]
        [SwaggerWcfResponse(HttpStatusCode.InternalServerError, "Internal error (can be forced using ERROR_500 as book title)", true)]
        /// <summary>
        /// 测试端查询上一站位的最后一条记录
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="typeNo"></param>
        /// <param name="station"></param>
        /// <returns></returns>
        public string SelectLastTestResult(string sn, string typeNo, string station)
        {
            string[] array = new string[] { sn, typeNo, station };
            selectDataQueue.Enqueue(array);
            return TestResult.SelectTestResult(selectDataQueue);
        }

        ///// <summary>
        ///// 上位机查询测试结果
        ///// </summary>
        ///// <param name="sn">追溯号，可为空</param>
        ///// <param name="typeNo">型号，可为空</param>
        ///// <param name="station">站位名，可为空</param>
        ///// <param name="IsSnFuzzy">true-模糊查询，false-非模糊查询</param>
        ///// <returns></returns>
        //public DataSet SelectTestResultUpper(string sn, string typeNo, string station, bool IsSnFuzzy)
        //{
        //    //查询时返回
        //    string selectSQL = "";
        //    if (string.IsNullOrEmpty(sn) && string.IsNullOrEmpty(typeNo) && string.IsNullOrEmpty(station))
        //    {
        //        selectSQL = $"SELECT {DbTable.F_Test_Result.SN} AS 产品追溯码,{DbTable.F_Test_Result.TYPE_NO} AS 产品型号," +
        //        $"{DbTable.F_Test_Result.STATION_NAME} AS 站位名称,{DbTable.F_Test_Result.TEST_RESULT} AS 测试结果," +
        //        $"{DbTable.F_Test_Result.UPDATE_DATE} AS 更新日期,{DbTable.F_Test_Result.REMARK} AS 备注 " +
        //        $"FROM {DbTable.F_TEST_RESULT_NAME}";
        //    }
        //    else
        //    {
        //        if (IsSnFuzzy)
        //        {
        //            selectSQL = $"SELECT {DbTable.F_Test_Result.SN} 产品追溯码,{DbTable.F_Test_Result.TYPE_NO} 产品型号," +
        //            $"{DbTable.F_Test_Result.STATION_NAME} 站位名称,{DbTable.F_Test_Result.TEST_RESULT} 测试结果," +
        //            $"{DbTable.F_Test_Result.UPDATE_DATE} 更新日期,{DbTable.F_Test_Result.REMARK} 备注 " +
        //            $"FROM {DbTable.F_TEST_RESULT_NAME} " +
        //            $"WHERE {DbTable.F_Test_Result.SN} like '%{sn}%' OR " +
        //            $"{DbTable.F_Test_Result.TYPE_NO} like '%{typeNo}%' OR " +
        //            $"{DbTable.F_Test_Result.STATION_NAME} like '%{station}%'";
        //        }
        //        else
        //        {
        //            selectSQL = $"SELECT {DbTable.F_Test_Result.SN} 产品追溯码,{DbTable.F_Test_Result.TYPE_NO} 产品型号," +
        //            $"{DbTable.F_Test_Result.STATION_NAME} 站位名称,{DbTable.F_Test_Result.TEST_RESULT} 测试结果," +
        //            $"{DbTable.F_Test_Result.UPDATE_DATE} 更新日期,{DbTable.F_Test_Result.REMARK} 备注 " +
        //            $"FROM {DbTable.F_TEST_RESULT_NAME} " +
        //            $"WHERE {DbTable.F_Test_Result.SN} = '{sn}' OR " +
        //            $"{DbTable.F_Test_Result.TYPE_NO} = '{typeNo}' OR " +
        //            $"{DbTable.F_Test_Result.STATION_NAME} = '{station}'";
        //        }
        //    }
        //    LogHelper.Log.Info(selectSQL);
        //    return SQLServer.ExecuteDataSet(selectSQL);
        //}

        ///// <summary>
        ///// 上位机查询上一站位的所有记录
        ///// </summary>
        ///// <param name="sn"></param>
        ///// <param name="typeNo"></param>
        ///// <param name="station"></param>
        ///// <returns></returns>
        //public DataSet SelectLastTestResultUpper(string sn, string typeNo, string station)
        //{
        //    //根据型号与站位，查询其上一站位
        //    LogHelper.Log.Info("上位机查询测试结果,传入站位为" + station);
        //    string selectOrderSQL = $"SELECT {DbTable.F_Product_Station.STATION_ORDER} FROM {DbTable.F_PRODUCT_STATION_NAME} " +
        //        $"WHERE {DbTable.F_Product_Station.STATION_NAME} = '{station}'";
        //    LogHelper.Log.Info(selectOrderSQL);
        //    DataTable dt = SQLServer.ExecuteDataSet(selectOrderSQL).Tables[0];
        //    int lastOrder = int.Parse(dt.Rows[0][0].ToString()) - 1;
        //    selectOrderSQL = $"SELECT {DbTable.F_Product_Station.STATION_NAME} FROM {DbTable.F_PRODUCT_STATION_NAME} " +
        //        $"WHERE {DbTable.F_Product_Station.STATION_ORDER} = '{lastOrder}'";
        //    dt = SQLServer.ExecuteDataSet(selectOrderSQL).Tables[0];
        //    station = dt.Rows[0][0].ToString();
        //    LogHelper.Log.Info("测试端查询测试结果,上一站位为" + station);
        //    //由上一站位查询记录
        //    string selectSQL = $"SELECT {DbTable.F_Test_Result.SN},{DbTable.F_Test_Result.TYPE_NO}," +
        //         $"{DbTable.F_Test_Result.STATION_NAME},{DbTable.F_Test_Result.TEST_RESULT}," +
        //         $"{DbTable.F_Test_Result.UPDATE_DATE},{DbTable.F_Test_Result.REMARK} " +
        //         $"FROM {DbTable.F_TEST_RESULT_NAME} " +
        //         $"WHERE {DbTable.F_Test_Result.SN} = '{sn}' AND {DbTable.F_Test_Result.TYPE_NO} = '{typeNo}' AND " +
        //         $"{DbTable.F_Test_Result.STATION_NAME} = '{station}'" +
        //         $"ORDER BY {DbTable.F_Test_Result.UPDATE_DATE}";
        //    return SQLServer.ExecuteDataSet(selectSQL);
        //}
        #endregion

        #region 物料信息表
        //public string CommitMaterial(List<MaterialMsg> list)
        //{
        //    foreach (var item in list)
        //    {
        //        if (!IsExistMaterial(item.MaterialCode))
        //        {
        //            //update
        //            if (InsertMaterial(item) < 1)
        //                return "I0";//插入失败
        //        }
        //        else
        //        {
        //            //insert
        //            if (UpdateMaterial(item) < 1)
        //                return "G0";//更新失败
        //        }
        //    }
        //    return "1";
        //}
        //public int DeleteMaterial(string materialCode)
        //{
        //    string deleteSQL = $"DELETE FROM {DbTable.F_MATERIAL_NAME} " +
        //        $"WHERE {DbTable.F_Material.MATERIAL_CODE} = '{materialCode}'";
        //    return SQLServer.ExecuteNonQuery(deleteSQL);
        //}
        //public int DeleteAllMaterial()
        //{
        //    string deleteSQL = $"DELETE FROM {DbTable.F_MATERIAL_NAME}";
        //    return SQLServer.ExecuteNonQuery(deleteSQL);
        //}
        //public DataSet SelectMaterial()
        //{
        //    string updateSQL = $"SELECT * FROM {DbTable.F_MATERIAL_NAME}";
        //    return SQLServer.ExecuteDataSet(updateSQL);
        //}
        //private bool IsExistMaterial(string materialCode)
        //{
        //    string selectSQL = $"SELECT * FROM {DbTable.F_MATERIAL_NAME} WHERE {DbTable.F_Material.MATERIAL_CODE} = '{materialCode}'";
        //    DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
        //    if (dt.Rows.Count > 0)
        //        return true;
        //    else
        //        return false;
        //}
        //private int InsertMaterial(MaterialMsg material)
        //{
        //    string insertSQL = $"INSERT INTO {DbTable.F_MATERIAL_NAME}({DbTable.F_Material.MATERIAL_CODE},{DbTable.F_Material.MATERIAL_AMOUNT}) " +
        //        $"VALUES('{material.MaterialCode}','{material.MaterialAmount}')";
        //    LogHelper.Log.Info($"InsertMaterial={insertSQL}");
        //    return SQLServer.ExecuteNonQuery(insertSQL);
        //}
        //private int UpdateMaterial(MaterialMsg material)
        //{
        //    string updateSQL = $"UPDATE {DbTable.F_MATERIAL_NAME} SET {DbTable.F_Material.MATERIAL_AMOUNT} = '{material.MaterialAmount}' " +
        //        $"WHERE {DbTable.F_Material.MATERIAL_CODE} = '{material.MaterialCode}'";
        //    LogHelper.Log.Info($"UpdateMaterial={updateSQL}");
        //    return SQLServer.ExecuteNonQuery(updateSQL);
        //}

        #endregion

        #region 产品物料绑定
        //public int CommitProductMaterial(List<ProductMaterial> pmList)
        //{
        //    foreach (var material in pmList)
        //    {
        //        if (IsExistMaterial(material))
        //        {
        //            //更新
        //            string updateSQL = $"UPDATE {DbTable.F_PRODUCT_MATERIAL_NAME} SET " +
        //                $"{DbTable.F_PRODUCT_MATERIAL.Describle} = '{material.Describle}' " +
        //                $"WHERE {DbTable.F_PRODUCT_MATERIAL.TYPE_NO} = '{material.TypeNo}' AND " +
        //                $"{DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE} = '{material.MaterialCode}'";
        //            LogHelper.Log.Info(updateSQL);
        //            if (SQLServer.ExecuteNonQuery(updateSQL) < 1)
        //                return 0;
        //        }
        //        else
        //        {
        //            //insert
        //            if (InsertProductMaterial(material) < 1)
        //                return 0;
        //        }
        //    }
        //    return 1;
        //}
        //public int DeleteProductMaterial(ProductMaterial material)
        //{
        //    if (material.MaterialCode == "" || material.TypeNo == "")
        //        return 0;
        //    string deleteSQL = $"DELETE FROM {DbTable.F_PRODUCT_MATERIAL_NAME} " +
        //        $"WHERE {DbTable.F_PRODUCT_MATERIAL.TYPE_NO} = '{material.TypeNo}' AND " +
        //        $"{DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE} = '{material.MaterialCode}'";
        //    LogHelper.Log.Info(deleteSQL);
        //    return SQLServer.ExecuteNonQuery(deleteSQL);
        //}
        //public DataSet SelectProductMaterial(ProductMaterial material)
        //{
        //    string selectSQL = "";
        //    if (string.IsNullOrEmpty(material.TypeNo) && string.IsNullOrEmpty(material.MaterialCode))
        //    {
        //        selectSQL = $"SELECT " +
        //           $"{DbTable.F_PRODUCT_MATERIAL.TYPE_NO},{DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE}," +
        //           $"{DbTable.F_PRODUCT_MATERIAL.Describle} " +
        //           $"FROM {DbTable.F_PRODUCT_MATERIAL_NAME} ORDER BY {DbTable.F_PRODUCT_MATERIAL.UpdateDate} ASC";
        //    }
        //    else
        //        selectSQL = $"SELECT " +
        //            $"{DbTable.F_PRODUCT_MATERIAL.TYPE_NO},{DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE}," +
        //            $"{DbTable.F_PRODUCT_MATERIAL.Describle} " +
        //            $"FROM {DbTable.F_PRODUCT_MATERIAL_NAME} " +
        //            $"WHERE {DbTable.F_PRODUCT_MATERIAL.TYPE_NO} = '{material.TypeNo}' OR " +
        //            $"{DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE} = '{material.MaterialCode}' " +
        //            $"ORDER BY {DbTable.F_PRODUCT_MATERIAL.UpdateDate} ASC";
        //    return SQLServer.ExecuteDataSet(selectSQL);
        //}
        //private bool IsExistMaterial(ProductMaterial material)
        //{
        //    string selectSQL = $"SELECT * FROM {DbTable.F_PRODUCT_MATERIAL_NAME} " +
        //        $"WHERE {DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE} = '{material.MaterialCode}' AND " +
        //        $"{DbTable.F_PRODUCT_MATERIAL.TYPE_NO} = '{material.TypeNo}'";
        //    DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
        //    if (dt.Rows.Count > 0)
        //        return true;
        //    else
        //        return false;
        //}
        //private int InsertProductMaterial(ProductMaterial material)
        //{
        //    string insertSQL = $"INSERT INTO {DbTable.F_PRODUCT_MATERIAL_NAME}({DbTable.F_PRODUCT_MATERIAL.TYPE_NO},{DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE}) " +
        //        $"VALUES('{material.TypeNo}','{material.MaterialCode}')";
        //    LogHelper.Log.Info(insertSQL);
        //    return SQLServer.ExecuteNonQuery(insertSQL);
        //}
        #endregion

        #region 物料统计表
        [SwaggerWcfTag("MesServcie 服务")]
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
        //public DataSet SelectMaterialStatistics(string typeNo)
        //{
        //    //按型号与物料查
        //    string selectSQL = "";
        //    if (!string.IsNullOrEmpty(typeNo))
        //    {
        //        selectSQL = $"SELECT {DbTable.F_Material_Statistics.SN_INNER},{DbTable.F_Material_Statistics.SN_OUTTER}," +
        //            $"{DbTable.F_Material_Statistics.TYPE_NO},{DbTable.F_Material_Statistics.MATERIAL_CODE}," +
        //            $"SUM({DbTable.F_Material_Statistics.MATERIAL_AMOUNT} as amount) " +
        //            $"FROM {DbTable.F_MATERIAL_STATISTICS_NAME} " +
        //            $"WHERE {DbTable.F_Material_Statistics.TYPE_NO} = '{typeNo}' " +
        //            $"GROUP BY {DbTable.F_Material_Statistics.SN_INNER},{DbTable.F_Material_Statistics.SN_OUTTER}," +
        //            $"{DbTable.F_Material_Statistics.TYPE_NO},{DbTable.F_Material_Statistics.MATERIAL_CODE}";
        //    }
        //    else
        //    {
        //        selectSQL = $"SELECT * FROM {DbTable.F_MATERIAL_STATISTICS_NAME}";
        //    }
        //    return SQLServer.ExecuteDataSet(selectSQL);
        //}
        #endregion

        #region 物料综合查询
        //public DataSet SelectMaterialMsg(MaterialMsg materialMsg, bool IsSelectAll)
        //{
        //    string selectSQL = "";
        //    if (IsSelectAll)
        //    {
        //        selectSQL = "SELECT a.material_code 物料编码,a.amount 物料总数,b.type_no 产品型号,b.update_date 绑定日期 " +
        //            "FROM [WT_SCL].[dbo].[f_material] a,[WT_SCL].[dbo].[f_product_material] b " +
        //            $"WHERE a.material_code = b.material_code ";
        //    }
        //    else
        //    {
        //        if (materialMsg.MaterialCode == "" && materialMsg.Sn_Inner == "" && materialMsg.Sn_Outter == "" && materialMsg.TypeNo == "" && materialMsg.StationName == "")
        //        {
        //            selectSQL = "SELECT a.material_code 物料编码,a.amount 物料总数,b.sn_inner 内壳码," +
        //            "b.sn_outter 外壳码,b.type_no 产品型号, b.station_name 站位名称, b.material_amount 消耗数量," +
        //            "(a.amount - b.material_amount) 剩余数量,b.update_date 更新日期 " +
        //            "FROM[WT_SCL].[dbo].[f_material] a,[WT_SCL].[dbo].f_material_statistics b " +
        //            $"WHERE a.material_code = b.material_code ";
        //        }
        //        else
        //        {
        //            selectSQL = "SELECT a.material_code 物料编码,a.amount 物料总数,b.sn_inner 内壳码," +
        //            "b.sn_outter 外壳码,b.type_no 产品型号, b.station_name 站位名称, b.material_amount 消耗数量," +
        //            "(a.amount - b.material_amount) 剩余数量,b.update_date 更新日期 " +
        //            "FROM[WT_SCL].[dbo].[f_material] a,[WT_SCL].[dbo].f_material_statistics b " +
        //            $"WHERE a.material_code = b.material_code AND " +
        //            $"{DbTable.F_Material.MATERIAL_CODE} = '{materialMsg.MaterialCode}' OR " +
        //            $"{DbTable.F_Material_Statistics.SN_INNER} = '{materialMsg.Sn_Inner}' OR " +
        //            $"{DbTable.F_Material_Statistics.SN_OUTTER} = '{materialMsg.Sn_Outter}' OR " +
        //            $"{DbTable.F_Material_Statistics.TYPE_NO} = '{materialMsg.TypeNo}' OR " +
        //            $"{DbTable.F_Material_Statistics.STATION_NAME} = '{materialMsg.StationName}'";
        //        }
        //    }
        //    LogHelper.Log.Info(selectSQL);
        //    return SQLServer.ExecuteDataSet(selectSQL);
        //}
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
        //public string FirstCheck(string sn_inner, string sn_outter, string sTypeNumber, string sStationName)
        //{
        //    //加入队列
        //    string[] array = new string[] { sn_inner, sn_outter, sTypeNumber, sStationName };
        //    fcQueue.Enqueue(array);
        //    LogHelper.Log.Info($"FirstCheck接口被调用，传入参数[{sn_inner},{sn_outter},{sTypeNumber},{sStationName}] 当前队列count={fcQueue.Count}");
        //    return FirstCheckQueue.CheckPass(fcQueue);
        //}

        #endregion

        #region 型号所属站位增删改查
        //public string CommitTypeStation(Dictionary<string, string[]> dctData)
        //{
        //    LogHelper.Log.Info($"接口被调用-CommitTypeStation");
        //    foreach (var typeNumber in dctData.Keys)
        //    {
        //        string[] arrayStation = dctData[typeNumber];
        //        //插入数据库
        //        //插入SQL
        //        if (IsExistTypeStation(typeNumber))
        //        {
        //            //ID已存在
        //            //update
        //            LogHelper.Log.Info("product type_number is exist, Excute UpdateProduceDB...");
        //            UpdateTypeStation(typeNumber, arrayStation);
        //        }
        //        else
        //        {
        //            //不存在，插入
        //            LogHelper.Log.Info($"product type number is not exist value={typeNumber}, Excute Insert Into Table...");
        //            string insertSQL = "INSERT INTO [WT_SCL].[dbo].[Product_Process]" +
        //                    "([Type_Number],[Station_Name_1],[Station_Name_2],[Station_Name_3],[Station_Name_4],[Station_Name_5]," +
        //                    "[Station_Name_6],[Station_Name_7],[Station_Name_8],[Station_Name_9],[Station_Name_10]) " +
        //                    $"VALUES('{typeNumber}','{arrayStation[0]}','{arrayStation[1]}','{arrayStation[2]}','{arrayStation[3]}'," +
        //                    $"'{arrayStation[4]}','{arrayStation[5]}','{arrayStation[6]}','{arrayStation[7]}','{arrayStation[8]}','{arrayStation[9]}')";

        //            int res = SQLServer.ExecuteNonQuery(insertSQL);
        //            if (res < 1)
        //            {
        //                //插入失败
        //                LogHelper.Log.Info($"product type number table insert fail {insertSQL}");
        //                return "0" + $" {typeNumber}";
        //            }
        //        }
        //    }
        //    return "1";
        //}

        ///// <summary>
        ///// 产品型号是否为空
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //private bool IsExistTypeStation(string typeNumber)
        //{
        //    string selectSQL = "SELECT  * " +
        //            "FROM [WT_SCL].[dbo].[Product_Process] " +
        //            $"WHERE [Type_Number] = '{typeNumber}'";
        //    LogHelper.Log.Info($"Exist Product type number = {selectSQL}");
        //    DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
        //    if (dt.Rows.Count > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// 执行更新数据库产品型号所属站位表
        ///// </summary>
        ///// <param name="order"></param>
        ///// <param name="stationName"></param>
        ///// <returns></returns>
        //private bool UpdateTypeStation(string typeNumber, string[] arrayStation)
        //{
        //    string updateSQL = "UPDATE [WT_SCL].[dbo].[Product_Process] " +
        //        $"SET Station_Name_1 = '{arrayStation[0]}',Station_Name_2='{arrayStation[1]}',Station_Name_3='{arrayStation[2]}'," +
        //        $"Station_Name_4='{arrayStation[3]}',Station_Name_5='{arrayStation[4]}',Station_Name_6 = '{arrayStation[5]}'," +
        //        $"Station_Name_7 = '{arrayStation[6]}',Station_Name_8 = '{arrayStation[7]}',Station_Name_9 = '{arrayStation[8]}',Station_Name_10 = '{arrayStation[9]}' " +
        //        $"WHERE Type_Number = '{typeNumber}'";
        //    int r = SQLServer.ExecuteNonQuery(updateSQL);
        //    LogHelper.Log.Info($"Update Product Type Number={updateSQL}");
        //    if (r > 0)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// 查询当前型号所属站位列表
        ///// </summary>
        ///// <returns></returns>
        //public DataSet SelectTypeStation(string typeNumber)
        //{
        //    string selectSQL = "";
        //    if (string.IsNullOrEmpty(typeNumber.Trim()))
        //    {
        //        selectSQL = $"SELECT * FROM [WT_SCL].[dbo].[Product_Process] ORDER BY [Type_Number]";
        //    }
        //    else
        //    {
        //        selectSQL = $"SELECT * FROM [WT_SCL].[dbo].[Product_Process] WHERE [Type_Number] = '{typeNumber}' ORDER BY [Type_Number]";
        //    }

        //    LogHelper.Log.Info($"Select Product Type Number={selectSQL}");
        //    return SQLServer.ExecuteDataSet(selectSQL);
        //}

        ///// <summary>
        ///// 清除所有数据
        ///// </summary>
        ///// <returns></returns>
        //public int DeleteAllTypeStation()
        //{
        //    string deleteSQL = "DELETE FROM [WT_SCL].[dbo].[Product_Process]";
        //    return SQLServer.ExecuteNonQuery(deleteSQL);
        //}

        ///// <summary>
        ///// 删除某条记录
        ///// </summary>
        ///// <param name="stationName"></param>
        ///// <returns></returns>
        //public int DeleteTypeStation(string typeNumber)
        //{
        //    string deleteSQL = $"DELETE FROM [WT_SCL].[dbo].[Product_Process] WHERE [Type_Number] = '{typeNumber}'";
        //    LogHelper.Log.Info($"DeleteProductType={deleteSQL}");
        //    return SQLServer.ExecuteNonQuery(deleteSQL);
        //}
        #endregion

        #region 成品打包接口
        //[SwaggerWcfTag("MesServcie 服务")]
        //public int CommitPackageProduct(List<PackageProduct> packageProductList)
        //{
        //    string imageName = "@imageData";
        //    for (int i = 0; i < packageProductList.Count; i++)
        //    {
        //        string insertSQL = $"INSERT INTO {DbTable.F_OUT_CASE_PRODUCT_NAME}({DbTable.F_Out_Case_Product.OUT_CASE_CODE}," +
        //        $"{DbTable.F_Out_Case_Product.SN_OUTTER},{DbTable.F_Out_Case_Product.TYPE_NO}," +
        //        $"{DbTable.F_Out_Case_Product.PICTURE},{DbTable.F_Out_Case_Product.BINDING_STATE}," +
        //        $"{DbTable.F_Out_Case_Product.BINDING_DATE},{DbTable.F_Out_Case_Product.REMARK}) " +
        //        $"VALUES('{packageProductList[i].CaseCode}','{packageProductList[i].SnOutter}','{packageProductList[i].TypeNo}',{imageName}," +
        //        $"'{packageProductList[i].BindingState}','{packageProductList[i].BindingDate}','{packageProductList[i].Remark}')";

        //        LogHelper.Log.Info($"CommitPackageProduct Init Insert={insertSQL}");
        //        if (IsExistPackageProduct(packageProductList[i].CaseCode, packageProductList[i].SnOutter))
        //        {
        //            //update
        //            UpdatePackageProduct(packageProductList[i]);
        //        }
        //        else
        //        {
        //            try
        //            {
        //                SqlParameter[] sqlParameters = new SqlParameter[1];
        //                SqlParameter sqlParameter = new SqlParameter();
        //                sqlParameter.ParameterName = imageName;
        //                sqlParameter.SqlDbType = SqlDbType.Binary;
        //                sqlParameter.Value = packageProductList[i].Picture;
        //                sqlParameters[0] = sqlParameter;
        //                SQLServer.ExecuteNonQuery(insertSQL, sqlParameters);
        //            }
        //            catch (Exception ex)
        //            {
        //                LogHelper.Log.Error($"Err start ExecuteNonQuery={ex.Message}\r\n{ex.StackTrace}");
        //                return -1;
        //            }
        //            //return ExecuteSQL(insertSQL,packageProduct.Picture);
        //        }
        //    }
        //    return 1;
        //}
        //private bool IsExistPackageProduct(string caseCode, string snOutter)
        //{
        //    string selectSQL = $"SELECT * FROM {DbTable.F_OUT_CASE_PRODUCT_NAME} " +
        //        $"WHERE {DbTable.F_Out_Case_Product.OUT_CASE_CODE} = '{caseCode}' AND " +
        //        $"{DbTable.F_Out_Case_Product.SN_OUTTER} = '{snOutter}'";
        //    DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
        //    if (dt.Rows.Count > 0)
        //        return true;
        //    return false;
        //}
        public int UpdatePackageProduct(PackageProduct packageProduct)
        {
            string updateSQL = $"UPDATE {DbTable.F_OUT_CASE_PRODUCT_NAME} SET " +
                $"{DbTable.F_Out_Case_Product.BINDING_STATE} = '{packageProduct.BindingState}' " +
                $"WHERE {DbTable.F_Out_Case_Product.OUT_CASE_CODE} = '{packageProduct.CaseCode}' AND " +
                $"{DbTable.F_Out_Case_Product.SN_OUTTER} = '{packageProduct.SnOutter}' ";
            LogHelper.LogInfo($"UpdatePackageProduct={updateSQL}");
            return SQLServer.ExecuteNonQuery(updateSQL);
        }

        //[SwaggerWcfTag("MesServcie 服务")]
        //public int DeletePackageProduct(PackageProduct packageProduct)
        //{
        //    string deleteSQL = "";
        //    if (packageProduct.SnOutter == "" && packageProduct.CaseCode == "")
        //    {
        //        deleteSQL = $"DELETE FROM {DbTable.F_OUT_CASE_PRODUCT_NAME} ";
        //    }
        //    else
        //    {
        //        deleteSQL = $"DELETE FROM {DbTable.F_OUT_CASE_PRODUCT_NAME} " +
        //        $"WHERE {DbTable.F_Out_Case_Product.OUT_CASE_CODE} = '{packageProduct.CaseCode}' AND " +
        //        $"{DbTable.F_Out_Case_Product.SN_OUTTER} = '{packageProduct.SnOutter}'";
        //    }
        //    return SQLServer.ExecuteNonQuery(deleteSQL);
        //}

        //[SwaggerWcfTag("MesServcie 服务")]
        //public DataSet SelectPackageProduct(PackageProduct packageProduct)
        //{
        //    //箱子编码/追溯码查询/产品型号
        //    string selectSQL = "";
        //    if (string.IsNullOrEmpty(packageProduct.CaseCode) && string.IsNullOrEmpty(packageProduct.SnOutter) && string.IsNullOrEmpty(packageProduct.TypeNo))
        //    {
        //        //查询所有已绑定记录
        //        selectSQL = $"SELECT OUT_CASE_CODE 包装箱编码,SN_OUTTER 产品追溯码," +
        //            $"TYPE_NO 产品型号,BINDING_STATE 绑定状态,BINDING_DATE 绑定日期 FROM {DbTable.F_OUT_CASE_PRODUCT_NAME} " +
        //            $"WHERE {DbTable.F_Out_Case_Product.BINDING_STATE} = '{packageProduct.BindingState}'";
        //    }
        //    else
        //    {
        //        selectSQL = $"SELECT OUT_CASE_CODE 包装箱编码,SN_OUTTER 产品追溯码," +
        //            $"TYPE_NO 产品型号,BINDING_STATE 绑定状态,BINDING_DATE 绑定日期 " +
        //            $"FROM {DbTable.F_OUT_CASE_PRODUCT_NAME} WHERE " +
        //            $"{DbTable.F_Out_Case_Product.OUT_CASE_CODE} = '{packageProduct.CaseCode}' AND " +
        //            $"{DbTable.F_Out_Case_Product.SN_OUTTER} = '{packageProduct.SnOutter}' OR " +
        //            $"{DbTable.F_Out_Case_Product.BINDING_STATE} = '{packageProduct.BindingState}'";
        //    }
        //    return SQLServer.ExecuteDataSet(selectSQL);
        //}

        //public DataSet SelectProductBindingState(string sn)
        //{
        //    //箱子编码/追溯码查询/产品型号
        //    string selectSQL = $"SELECT BINDING_STATE 绑定状态 " +
        //            $"FROM {DbTable.F_OUT_CASE_PRODUCT_NAME} WHERE " +
        //            $"{DbTable.F_Out_Case_Product.SN_OUTTER} = '{sn}'";
        //    return SQLServer.ExecuteDataSet(selectSQL);
        //}

        ///// <summary>
        ///// 由箱子编码查询，该箱子已装数量；已绑定/绑定后解绑
        ///// </summary>
        ///// <param name="casecode"></param>
        ///// <param name="bindingState"></param>
        ///// <returns></returns>
        //public DataSet SelectProductBindingCount(string casecode, string bindingState)
        //{
        //    string selectSQL = $"SELECT {DbTable.F_Out_Case_Product.OUT_CASE_CODE},{DbTable.F_Out_Case_Product.SN_OUTTER} " +
        //        $"FROM {DbTable.F_OUT_CASE_PRODUCT_NAME} " +
        //        $"WHERE {DbTable.F_Out_Case_Product.OUT_CASE_CODE} = '{casecode}' AND {DbTable.F_Out_Case_Product.BINDING_STATE} = '{bindingState}'";
        //    return SQLServer.ExecuteDataSet(selectSQL);
        //}

        ///// <summary>
        ///// 由箱子编码删除所有绑定记录
        ///// </summary>
        ///// <param name="casecode"></param>
        ///// <returns></returns>
        //public int DeleteProductBindingData(string casecode)
        //{
        //    string deleteSQL = $"DELETE FROM {DbTable.F_OUT_CASE_PRODUCT_NAME} WHERE " +
        //        $"{DbTable.F_Out_Case_Product.OUT_CASE_CODE} = '{casecode}'";
        //    return SQLServer.ExecuteNonQuery(deleteSQL);
        //}
        #endregion

        #region 外箱容量接口
        //public int CommitOutCaseBoxStorage(string out_case_code, string amount)
        //{
        //    string insertSQL = $"INSERT INTO {DbTable.F_OUT_CASE_STORAGE_NAME}({DbTable.F_Out_Case_Storage.OUT_CASE_CODE},{DbTable.F_Out_Case_Storage.STORAGE_CAPACITY}) " +
        //        $"VALUES('{out_case_code}','{amount}')";
        //    if (IsExistOutCaseBoxStorage(out_case_code))
        //    {
        //        //update
        //        return UpdateOutCaseBoxStorage(out_case_code, amount);
        //    }
        //    else
        //    {
        //        //insert
        //        return SQLServer.ExecuteNonQuery(insertSQL);
        //    }
        //}
        //public int UpdateOutCaseBoxStorage(string out_case_code, string amount)
        //{
        //    string updateSQL = $"UPDATE {DbTable.F_OUT_CASE_STORAGE_NAME} SET {DbTable.F_Out_Case_Storage.STORAGE_CAPACITY} = '{amount}' " +
        //        $"WHERE {DbTable.F_Out_Case_Storage.OUT_CASE_CODE} = '{out_case_code}'";
        //    return SQLServer.ExecuteNonQuery(updateSQL);
        //}
        //public DataSet SelectOutCaseBoxStorage(string caseCode)
        //{
        //    string selectSQL = "";
        //    if (string.IsNullOrEmpty(caseCode))
        //    {
        //        selectSQL = $"SELECT * FROM {DbTable.F_OUT_CASE_STORAGE_NAME}";
        //    }
        //    else
        //    {
        //        selectSQL = $"SELECT * FROM {DbTable.F_OUT_CASE_STORAGE_NAME} WHERE {DbTable.F_Out_Case_Storage.OUT_CASE_CODE} = '{caseCode}'";
        //    }
        //    return SQLServer.ExecuteDataSet(selectSQL);
        //}
        //private bool IsExistOutCaseBoxStorage(string out_case_code)
        //{
        //    string selectSQL = $"SELECT * FROM {DbTable.F_OUT_CASE_STORAGE_NAME} WHERE {DbTable.F_Out_Case_Storage.OUT_CASE_CODE} = '{out_case_code}'";
        //    DataTable dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
        //    if (dt.Rows.Count > 0)
        //        return true;
        //    return false;
        //}
        #endregion

        #region 产品设站接口
        #endregion
    }
}