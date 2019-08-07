using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonUtils.DB;
using CommonUtils.Logger;
using MESWebService.DB;
using System;

namespace MESWebService.MessageQueue.RemoteClient
{
    public class TestResult
    {
        public static string InsertTestResult(Queue<string[]> queue)
        {
            string[] array = queue.Dequeue();
            string sn = array[0];
            string typeNo = array[1];
            string station = array[2];
            string dateTime = array[3];
            string result = array[4];
            string insertSQL = $"INSERT INTO {DbTable.F_TEST_RESULT_NAME}({DbTable.F_Test_Result.SN}," +
                $"{DbTable.F_Test_Result.TYPE_NO},{DbTable.F_Test_Result.STATION_NAME}," +
                $"{DbTable.F_Test_Result.TEST_RESULT},{DbTable.F_Test_Result.UPDATE_DATE},{DbTable.F_Test_Result.REMARK}) " +
                $"VALUES('{sn}','{typeNo}','{station}','{result}','{dateTime}','测试结果')";
            LogHelper.Log.Info(insertSQL);
            try
            {
                int row = SQLServer.ExecuteNonQuery(insertSQL);
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

        public static string SelectTestResult(Queue<string[]> queue)
        {
            string[] array = queue.Dequeue();
            string sn = array[0];
            string typeNo = array[1];
            string station = array[2];
            LogHelper.Log.Info("测试端查询测试结果,站位为"+station);
            //根据型号与站位，查询其上一站位
            string selectOrderSQL = $"SELECT {DbTable.F_Product_Station.STATION_ORDER} FROM {DbTable.F_PRODUCT_STATION_NAME} " +
                $"WHERE {DbTable.F_Product_Station.STATION_NAME} = '{station}'";
            LogHelper.Log.Info(selectOrderSQL);
            DataTable dt = SQLServer.ExecuteDataSet(selectOrderSQL).Tables[0];
            if (dt.Rows.Count < 1)
                return "";
            int lastOrder = int.Parse(dt.Rows[0][0].ToString()) - 1;
            selectOrderSQL = $"SELECT {DbTable.F_Product_Station.STATION_NAME} FROM {DbTable.F_PRODUCT_STATION_NAME} " +
                $"WHERE {DbTable.F_Product_Station.STATION_ORDER} = '{lastOrder}'";
            dt = SQLServer.ExecuteDataSet(selectOrderSQL).Tables[0];
            if (dt.Rows.Count < 1)
                return "";
            station = dt.Rows[0][0].ToString();
            LogHelper.Log.Info("测试端查询测试结果,上一站位为" + station);
            //根据上一站位在查询该站位的最后一条记录
            string selectSQL = $"SELECT {DbTable.F_Test_Result.TEST_RESULT} " +
                $"FROM " +
                $"{DbTable.F_TEST_RESULT_NAME} " +
                $"WHERE " +
                $"{DbTable.F_Test_Result.SN} = '{sn}' AND " +
                $"{DbTable.F_Test_Result.TYPE_NO} = '{typeNo}' AND " +
                $"{DbTable.F_Test_Result.STATION_NAME} = '{station}' " +
                $"ORDER BY " +
                $"{DbTable.F_Test_Result.UPDATE_DATE} " +
                $"DESC ";
            LogHelper.Log.Info(selectSQL);
            dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count < 1)
            {
                return "";//查询失败
            }
            string testRes = dt.Rows[0][0].ToString();
            //返回上一个站位的最后一条记录
            return sn + "|" + typeNo + "|" + station + "|" + testRes;
        }
    }
}