using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonUtils.DB;
using CommonUtils.Logger;
using MesWcfService.DB;

namespace MesWcfService.MessageQueue.RemoteClient
{
    public class LimitConfig
    {
        public static string UpdateLimitConfig(Queue<string[]> pvqueue)
        {
            try
            {
                string[] array = pvqueue.Dequeue();
                var stationName = array[0];
                var typeNo = array[1];
                var testItem = array[2];
                var limitValue = array[3];
                var teamLeader = array[4];
                var admin = array[5];
                var insertSQL = $"INSERT INTO {DbTable.F_TEST_LIMIT_CONFIG_NAME}(" +
                    $"{DbTable.F_TEST_LIMIT_CONFIG.STATION_NAME}," +
                    $"{DbTable.F_TEST_LIMIT_CONFIG.TYPE_NO}," +
                    $"{DbTable.F_TEST_LIMIT_CONFIG.TEST_ITEM}" +
                    $"{DbTable.F_TEST_LIMIT_CONFIG.LIMIT}," +
                    $"{DbTable.F_TEST_LIMIT_CONFIG.TEAM_LEADER}," +
                    $"{DbTable.F_TEST_LIMIT_CONFIG.ADMIN}," +
                    $"{DbTable.F_TEST_LIMIT_CONFIG.UPDATE_DATE}) VALUES(" +
                    $"'{stationName}','{typeNo}','{testItem}','{limitValue}','{teamLeader}','{admin}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
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