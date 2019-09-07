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
            var materialCode = array[2];
            var insertSQL = $"INSERT INTO {DbTable.F_BINDING_PCBA_NAME}(" +
                $"{DbTable.F_BINDING_PCBA.SN_PCBA}," +
                $"{DbTable.F_BINDING_PCBA.SN_OUTTER}," +
                $"{DbTable.F_BINDING_PCBA.UPDATE_DATE}," +
                $"{DbTable.F_BINDING_PCBA.MATERIAL_CODE}) VALUES(" +
                $"'{sn_pcba}','{sn_outter}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{materialCode}')";

            var upDateSQL = $"UPDATE {DbTable.F_BINDING_PCBA_NAME} SET " +
                $"{DbTable.F_BINDING_PCBA.SN_OUTTER} = '{sn_outter}' WHERE " +
                $"{DbTable.F_BINDING_PCBA.SN_PCBA} = '{sn_pcba}'";

            if (IsExistPCBA(sn_pcba,sn_outter,materialCode))
            {
                //update
                //绑定添加，无更新
                LogHelper.Log.Info("【PCBA绑定-已经绑定-重复绑定】");
                return "OK";//已经绑定
            }
            else
            {
                //insert
                int isRes = SQLServer.ExecuteNonQuery(insertSQL);
                if (isRes > 0)
                    return "OK";
                else
                {
                    LogHelper.Log.Info("【PCB绑定失败】" + insertSQL);
                    return "FAIL";
                }
            }
        }

        private static bool IsExistPCBA(string snPcba,string snOutter,string materialCode)
        {
            var selectSQL = $"SELECT * FROM {DbTable.F_BINDING_PCBA_NAME} WHERE " +
                $"{DbTable.F_BINDING_PCBA.SN_PCBA} = '{snPcba}' AND " +
                $"{DbTable.F_BINDING_PCBA.SN_OUTTER}= '{snOutter}' AND " +
                $"{DbTable.F_BINDING_PCBA.MATERIAL_CODE} = '{materialCode}'";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }
    }
}