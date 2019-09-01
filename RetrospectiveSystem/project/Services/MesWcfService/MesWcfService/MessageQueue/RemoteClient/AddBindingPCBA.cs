using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonUtils.DB;
using CommonUtils.Logger;
using MesWcfService.DB;

namespace MesWcfService.MessageQueue.RemoteClient
{
    public class AddBindingPCBA
    {
        public static string BindingPCBA(Queue<string[]> queue)
        {
            var array = queue.Dequeue();
            var sn_pcba = array[0];
            var sn_outter = array[1];
            var insertSQL = $"INSERT INTO {DbTable.F_BINDING_PCBA_NAME}(" +
                $"{DbTable.F_BINDING_PCBA.SN_PCBA}," +
                $"{DbTable.F_BINDING_PCBA.SN_OUTTER}," +
                $"{DbTable.F_BINDING_PCBA.UPDATE_DATE}) VALUES(" +
                $"'{sn_pcba}','{sn_outter}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";
            var upDateSQL = $"UPDATE {DbTable.F_BINDING_PCBA_NAME} SET " +
                $"{DbTable.F_BINDING_PCBA.SN_OUTTER} = '{sn_outter}' WHERE " +
                $"{DbTable.F_BINDING_PCBA.SN_PCBA} = '{sn_pcba}'";
            if (IsExistPCBA(sn_pcba))
            {
                //update
                int upRes = SQLServer.ExecuteNonQuery(upDateSQL);
                if (upRes > 0)
                    return "OK";
                else
                    return "FAIL";
            }
            else
            {
                //insert
                int isRes = SQLServer.ExecuteNonQuery(insertSQL);
                if (isRes > 0)
                    return "OK";
                else
                    return "FAIL";
            }
        }

        private static bool IsExistPCBA(string sn_pcba)
        {
            var selectSQL = $"SELECT * FROM {DbTable.F_BINDING_PCBA_NAME} WHERE " +
                $"{DbTable.F_BINDING_PCBA.SN_PCBA} = '{sn_pcba}'";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }
    }
}