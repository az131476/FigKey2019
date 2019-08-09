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
                var limitValue = array[2];
                var teamLeader = array[3];
                var admin = array[4];
                var insertSQL = $"INSERT INTO {DbTable.F_TEST_LIMIT_CONFIG_NAME}(" +
                    $"{DbTable.F_TEST_LIMIT_CONFIG.STATION_NAME}," +
                    $"{DbTable.F_TEST_LIMIT_CONFIG.TYPE_NO}," +
                    $"{DbTable.F_TEST_LIMIT_CONFIG.LIMIT_VALUE}," +
                    $"{DbTable.F_TEST_LIMIT_CONFIG.TEAM_LEADER}," +
                    $"{DbTable.F_TEST_LIMIT_CONFIG.ADMIN})";
                var res = SQLServer.ExecuteNonQuery(insertSQL);
                if (res > 0)
                    return "OK";
                else
                    return "FAIL";
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message);
                return "ERROR";
            }
        }
    }
}