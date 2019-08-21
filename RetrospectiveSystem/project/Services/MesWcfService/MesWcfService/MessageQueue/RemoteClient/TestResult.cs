﻿using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonUtils.DB;
using CommonUtils.Logger;
using MesWcfService.DB;
using System;
using MesWcfService;

namespace MesWcfService.MessageQueue.RemoteClient
{
    public class TestResult
    {
        public static string InsertTestResult(Queue<string[]> queue)
        {
            string[] array = queue.Dequeue();
            var sn = array[0];
            var typeNo = array[1];
            var station = array[2];
            var result = array[3];
            var teamLeader = array[4];
            var admin = array[5];
            var processName = new MesService().SelectCurrentTProcess();

            string insertSQL = $"INSERT INTO {DbTable.F_TEST_RESULT_NAME}({DbTable.F_Test_Result.SN}," +
                $"{DbTable.F_Test_Result.TYPE_NO},{DbTable.F_Test_Result.STATION_NAME}," +
                $"{DbTable.F_Test_Result.TEST_RESULT},{DbTable.F_Test_Result.REMARK}," +
                $"{DbTable.F_Test_Result.TEAM_LEADER},{DbTable.F_Test_Result.ADMIN}," +
                $"{DbTable.F_Test_Result.PROCESS_NAME},{DbTable.F_Test_Result.UPDATE_DATE}) " +
                $"VALUES('{sn}','{typeNo}','{station}','{result}','测试结果'," +
                $"'{teamLeader}','{admin}','{processName}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";

            LogHelper.Log.Info(insertSQL);
            try
            {
                int row = 0;
                //if (!IsExistTestResult(typeNo,station,dateTime,result))
                //{
                    row = SQLServer.ExecuteNonQuery(insertSQL);
                //}
                if (row > 0)
                {
                    return "OK";
                }
                else
                {
                    return "FAIL";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message);
                return "ERROR";
            }
        }

        private static bool IsExistTestResult(string typeNo,string station,string dateTime,string result)
        {
            string selectSQL = $"SELECT * FROM {DbTable.F_TEST_RESULT_NAME} WHER " +
                    $"{DbTable.F_Test_Result.TYPE_NO} = '{typeNo}' AND " +
                    $"{DbTable.F_Test_Result.STATION_NAME} = '{station}' AND " +
                    $"{DbTable.F_Test_Result.UPDATE_DATE} = '{dateTime}' AND " +
                    $"{DbTable.F_Test_Result.TEST_RESULT} = '{result}'";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }

        public static string[] SelectTestResult(Queue<string[]> queue)
        {
            string[] queryResult;
            try
            {
                string[] array = queue.Dequeue();
                string sn = array[0];
                string station = array[1];
                LogHelper.Log.Info("测试端查询测试结果,站位为" + station);
                //根据当前工艺与站位，查询其上一站位
                //查询当前工艺流程
                MesService mesService = new MesService();
                var processName = mesService.SelectCurrentTProcess();
                string selectOrderSQL = $"SELECT {DbTable.F_TECHNOLOGICAL_PROCESS.STATION_ORDER} " +
                    $"FROM {DbTable.F_TECHNOLOGICAL_PROCESS_NAME} " +
                    $"WHERE {DbTable.F_TECHNOLOGICAL_PROCESS.STATION_NAME} = '{station}' AND " +
                    $"{DbTable.F_TECHNOLOGICAL_PROCESS.PROCESS_NAME} = '{processName}'";
                LogHelper.Log.Info(selectOrderSQL);
                DataTable dt = SQLServer.ExecuteDataSet(selectOrderSQL).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    queryResult = new string[1];
                    queryResult[0] = "ERR_LAST_STATION";
                    return queryResult;
                }
                int lastOrder = int.Parse(dt.Rows[0][0].ToString()) - 1;
                selectOrderSQL = $"SELECT {DbTable.F_TECHNOLOGICAL_PROCESS.STATION_NAME} FROM {DbTable.F_TECHNOLOGICAL_PROCESS_NAME} " +
                    $"WHERE {DbTable.F_TECHNOLOGICAL_PROCESS.STATION_ORDER} = '{lastOrder}' AND " +
                    $"{DbTable.F_TECHNOLOGICAL_PROCESS.PROCESS_NAME} = '{processName}'";
                dt = SQLServer.ExecuteDataSet(selectOrderSQL).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    queryResult = new string[1];
                    queryResult[0] = "ERR_LAST_STATION_KEY";
                    return queryResult;
                }
                station = dt.Rows[0][0].ToString();
                LogHelper.Log.Info("测试端查询测试结果,上一站位为" + station);
                //根据上一站位在查询该站位的最后一条记录
                string selectSQL = $"SELECT {DbTable.F_Test_Result.TEST_RESULT} " +
                    $"FROM " +
                    $"{DbTable.F_TEST_RESULT_NAME} " +
                    $"WHERE " +
                    $"{DbTable.F_Test_Result.SN} = '{sn}' AND " +
                    $"{DbTable.F_Test_Result.STATION_NAME} = '{station}' " +
                    $"ORDER BY " +
                    $"{DbTable.F_Test_Result.UPDATE_DATE} " +
                    $"DESC ";
                LogHelper.Log.Info(selectSQL);
                dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
                if (dt.Rows.Count < 1)
                {
                    //查询失败
                    queryResult = new string[1];
                    queryResult[0] = "QUERY_NONE";
                    return queryResult;
                }
                string testRes = dt.Rows[0][0].ToString();
                //返回上一个站位的最后一条记录
                queryResult = new string[3];
                queryResult[0] = sn;
                queryResult[1] = station;
                queryResult[2] = testRes;
                return queryResult;
            }
            catch (Exception ex)
            {
                queryResult = new string[1];
                queryResult[0] = "ERR_EXCEPTION";
                LogHelper.Log.Error(ex.Message+"\r\n"+ex.StackTrace);
                return queryResult;
            }
        }
    }
}