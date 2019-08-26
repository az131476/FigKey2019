using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonUtils.DB;
using CommonUtils.Logger;
using MesWcfService.DB;

namespace MesWcfService.MessageQueue.RemoteClient
{
    public class TestLogData
    {
        public static string UpdateTestLogData(Queue<string[]> tlQueue)
        {
            try
            {//typeNo,stationName,productSN,testItem,limit,currentValue,testResult,teamLeader,admin
                string[] array = tlQueue.Dequeue();
                var typeNo = array[0];
                var stationName = array[1];
                var productSN = array[2];
                var testItem = array[3];
                var limit = array[4];
                var currentValue = array[5];
                var testResult = array[6];
                var teamLeader = array[7];
                var admin = array[8];

                var insertSQL = $"INSERT INTO {DbTable.F_TEST_LOG_DATA_NAME}(" +
                    $"{DbTable.F_TEST_LOG_DATA.TYPE_NO}," +
                    $"{DbTable.F_TEST_LOG_DATA.STATION_NAME}," +
                    $"{DbTable.F_TEST_LOG_DATA.PRODUCT_SN}," +
                    $"{DbTable.F_TEST_LOG_DATA.TEST_ITEM}," +
                    $"{DbTable.F_TEST_LOG_DATA.LIMIT}," +
                    $"{DbTable.F_TEST_LOG_DATA.CURRENT_VALUE}," +
                    $"{DbTable.F_TEST_LOG_DATA.TEST_RESULT}," +
                    $"{DbTable.F_TEST_LOG_DATA.TEAM_LEADER}," +
                    $"{DbTable.F_TEST_LOG_DATA.ADMIN}," +
                    $"{DbTable.F_TEST_LOG_DATA.UPDATE_DATE}) VALUES(" +
                    $"'{typeNo}','{stationName}','{productSN}','{testItem}','{limit}'," +
                    $"'{currentValue}','{testResult}','{teamLeader}','{admin}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
                var res = SQLServer.ExecuteNonQuery(insertSQL);
                if (res > 0)
                    return "OK";
                else
                {
                    LogHelper.Log.Info(insertSQL);
                    return "FAIL";
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message);
                return "ERROR";
            }
        }
    }
}