using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonUtils.DB;
using CommonUtils.Logger;
using MesWcfService.DB;

namespace MesWcfService.MessageQueue.RemoteClient
{
    public class MaterialStatistics
    {
        //物料统计
        //插入
        private static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public static string InsertMaterialStatistics(Queue<string[]> queue)
        {
            //更新装配物料数量前，check物料是否使用完：
            //用完提示，未用完继续添加
            //添加完成，更新已使用数量物料总数
            try
            {
                string[] array = queue.Dequeue();
                string sn_inner = array[0];
                string sn_outter = array[1];
                string type_no = array[2];
                string station_name = array[3];
                string material_code = array[4];
                string material_amount = array[5];
                string teamLeader = array[6];
                string admin = array[7];
                string insertSQL = $"INSERT INTO {DbTable.F_MATERIAL_STATISTICS_NAME}({DbTable.F_Material_Statistics.SN_INNER}," +
                    $"{DbTable.F_Material_Statistics.SN_OUTTER},{DbTable.F_Material_Statistics.TYPE_NO}," +
                    $"{DbTable.F_Material_Statistics.STATION_NAME},{DbTable.F_Material_Statistics.MATERIAL_CODE}," +
                    $"{DbTable.F_Material_Statistics.MATERIAL_AMOUNT},{DbTable.F_Material_Statistics.UPDATE_DATE}," +
                    $"{DbTable.F_Material_Statistics.TEAM_LEADER},{DbTable.F_Material_Statistics.ADMIN}) " +
                    $"VALUES('{sn_inner}','{sn_outter}','{type_no}','{station_name}','{material_code}'," +
                    $"'{material_amount}','{GetDateTime()}','{teamLeader}','{admin}')";
                LogHelper.Log.Info(insertSQL);
                int row = 0;
                if (!IsExistMaterialData(array))
                {
                    row = SQLServer.ExecuteNonQuery(insertSQL);
                    UpdateMaterialQuantity(type_no,material_code,int.Parse(material_amount));
                }
                else
                {
                    int originNum = SelectLastInsertAmount(sn_inner,sn_outter,type_no,station_name,material_code);
                    row = UpdateMaterialAmount(sn_inner,sn_outter,type_no,station_name,material_code, material_amount);
                    UpdateMaterialQuantity(type_no,material_code,int.Parse(material_amount) - originNum);
                }
                if (row > 0)
                    return "OK";
                return "FAIL";
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message);
                return "ERROR";
            }
        }

        private static bool IsExistMaterialData(string[] array)
        {
            string sn_inner = array[0];
            string sn_outter = array[1];
            string type_no = array[2];
            string station_name = array[3];
            string material_code = array[4];
            string material_amount = array[5];
            string teamLeader = array[6];
            string admin = array[7];
            var selectSQL = $"SELECT * FROM {DbTable.F_MATERIAL_STATISTICS_NAME} WHERE " +
                $"{DbTable.F_Material_Statistics.TYPE_NO} = '{type_no}' AND " +
                $"{DbTable.F_Material_Statistics.MATERIAL_CODE} = '{material_amount}' AND " +
                $"{DbTable.F_Material_Statistics.UPDATE_DATE} = '{GetDateTime()}' AND " +
                $"{DbTable.F_Material_Statistics.TEAM_LEADER} = '{teamLeader}' AND " +
                $"{DbTable.F_Material_Statistics.ADMIN} = '{admin}' " +
                $"WHERE " +
                $"{DbTable.F_Material_Statistics.SN_INNER} = '{sn_inner}' AND " +
                $"{DbTable.F_Material_Statistics.SN_OUTTER} = '{sn_outter}' AND " +
                $"{DbTable.F_Material_Statistics.STATION_NAME} = {station_name} AND " +
                $"{DbTable.F_Material_Statistics.MATERIAL_CODE} = {material_code}";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }

        private static int UpdateMaterialAmount(string sn_inner, string sn_outter, string type_no, string stationName, string code, string amount)
        {
            var updateSQL = $"UPDATE {DbTable.F_MATERIAL_STATISTICS_NAME} SET " +
                $"{DbTable.F_Material_Statistics.MATERIAL_AMOUNT} = '{amount}' ," +
                $"{DbTable.F_Material_Statistics.UPDATE_DATE} = '{GetDateTime()}' WHERE " +
                $"{DbTable.F_Material_Statistics.SN_INNER} = '{sn_inner}' AND " +
                $"{DbTable.F_Material_Statistics.SN_OUTTER} = '{sn_outter}' AND " +
                $"{DbTable.F_Material_Statistics.TYPE_NO} = '{type_no}' AND " +
                $"{DbTable.F_Material_Statistics.STATION_NAME} = '{stationName}' AND " +
                $"{DbTable.F_Material_Statistics.MATERIAL_CODE} = '{code}'";
            return SQLServer.ExecuteNonQuery(updateSQL);
        }

        private static int SelectLastInsertAmount(string sn_inner, string sn_outter, string type_no, string stationName, string code)
        {
            var selectSQL = $"SELECT {DbTable.F_Material_Statistics.MATERIAL_AMOUNT} " +
                $"FROM {DbTable.F_MATERIAL_STATISTICS_NAME} WHERE " +
                $"{DbTable.F_Material_Statistics.SN_INNER} = '{sn_inner}' AND " +
                $"{DbTable.F_Material_Statistics.SN_OUTTER} = '{sn_outter}' AND " +
                $"{DbTable.F_Material_Statistics.TYPE_NO} = '{type_no}' AND " +
                $"{DbTable.F_Material_Statistics.STATION_NAME} = '{stationName}' AND " +
                $"{DbTable.F_Material_Statistics.MATERIAL_CODE} = '{code}'";
            return int.Parse(SQLServer.ExecuteDataSet(selectSQL).Tables[0].Rows[0][0].ToString());
        }

        private static void UpdateMaterialQuantity(string typeNo,string materialCode,int amounted)
        {
            var updateSQL = $"UPDATE {DbTable.F_PRODUCT_MATERIAL_NAME} SET " +
                $"{DbTable.F_PRODUCT_MATERIAL.AMOUNTED} += '{amounted}' " +
                $"WHERE " +
                $"{DbTable.F_PRODUCT_MATERIAL.TYPE_NO} = '{typeNo}' AND " +
                $"{DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE} = '{materialCode}'";
            SQLServer.ExecuteNonQuery(updateSQL);
        }
    }
}