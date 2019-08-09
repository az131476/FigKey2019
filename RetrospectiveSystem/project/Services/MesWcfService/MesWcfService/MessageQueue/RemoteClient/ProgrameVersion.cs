using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MesWcfService.Model;
using MesWcfService.DB;
using CommonUtils.Logger;
using CommonUtils.DB;

namespace MesWcfService.MessageQueue.RemoteClient
{
    public class ProgrameVersion
    {
        public static string UpdateProgrameVersion(Queue<string[]> pvqueue)
        {
            try
            {
                string[] array = pvqueue.Dequeue();
                var typeNo = array[0];
                var stationName = array[1];
                var programeName = array[2];
                var programeVersion = array[3];
                var teamLeader = array[4];
                var admin = array[5];
                var insertSQL = $"INSERT INTO {DbTable.F_TEST_PROGRAME_VERSION_NAME}(" +
                    $"{DbTable.F_TEST_PROGRAME_VERSION.TYPE_NO}," +
                    $"{DbTable.F_TEST_PROGRAME_VERSION.STATION_NAME}," +
                    $"{DbTable.F_TEST_PROGRAME_VERSION.PROGRAME_NAME}," +
                    $"{DbTable.F_TEST_PROGRAME_VERSION.PROGRAME_VERSION}," +
                    $"{DbTable.F_TEST_PROGRAME_VERSION.TEAM_LEADER})";
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