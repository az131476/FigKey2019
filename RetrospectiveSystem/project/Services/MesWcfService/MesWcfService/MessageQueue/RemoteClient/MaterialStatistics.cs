using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonUtils.DB;
using CommonUtils.Logger;
using MesWcfService.DB;
using MesWcfService.Model;

namespace MesWcfService.MessageQueue.RemoteClient
{
    public static class MaterialStatistics
    {
        //物料统计
        //插入
        private static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ConvertMaterialStatisticsCode(MaterialStatisticsReturnCode rCode)
        {
            return "0X" + Convert.ToString((int)rCode, 16).PadLeft(2, '0');
        }

        public static string ConvertCheckMaterialStateCode(MaterialStateReturnCode msCode)
        {
            return "0X"+Convert.ToString((int)msCode,16).PadLeft(2,'0');
        }

        public static string ConvertCheckMaterialMatch(MaterialCheckMatchReturnCode mcmCode)
        {
            return "0X" + Convert.ToString((int)mcmCode,16).PadLeft(2,'0');
        }

        /// <summary>
        /// 物料号防错
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public static string CheckMaterialMatch(Queue<string[]> queue)
        {
            var materialArray = queue.Dequeue();
            var productTypeNo = materialArray[0];
            var materialPN = materialArray[1];
            var selectSQL = $"SELECT * FROM {DbTable.F_PRODUCT_MATERIAL_NAME}  WHERE " +
                $"{DbTable.F_PRODUCT_MATERIAL.TYPE_NO} = '{productTypeNo}' AND " +
                $"{DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE} = '{materialPN}'";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count == 1)
                return ConvertCheckMaterialMatch(MaterialCheckMatchReturnCode.IS_MATCH);
            return ConvertCheckMaterialMatch(MaterialCheckMatchReturnCode.IS_NOT_MATCH);
        }

        public static string CheckMaterialState(Queue<string> queue)
        {
            var materialCode = queue.Dequeue();
            var selectSQl = $"SELECT {DbTable.F_Material.MATERIAL_STATE} " +
                $"FROM {DbTable.F_MATERIAL_NAME} WHERE " +
                $"{DbTable.F_Material.MATERIAL_CODE} = '{materialCode}'";
            var dt = SQLServer.ExecuteDataSet(selectSQl).Tables[0];
            if (dt.Rows.Count < 1)
                return ConvertCheckMaterialStateCode(MaterialStateReturnCode.ERROR_NULL_QUERY);
            var queryRes = dt.Rows[0][0].ToString();
            return "0X" + Convert.ToString(int.Parse(queryRes)).PadLeft(2,'0');
        }

        public static string UpdateMaterialStatistics(Queue<string[]> queue)
        {
            //更新物料统计：插入/更新
            //更新产品-物料：使用物料数量
            //更新物料库存：使用数量，物料状态，使用完成更新结单状态为2
            //typeNo,stationName,materialCode,amounted,teamLeader,admin
            try
            {
                #region material params
                string[] array = queue.Dequeue();
                var productTypeNo = array[0];
                var stationName = array[1];
                var materialCode = array[2];
                var amounted = array[3];
                var teamLeader = array[4];
                var admin = array[5];
                #endregion

                #region insert sql
                var insertSQL = $"INSERT INTO {DbTable.F_MATERIAL_STATISTICS_NAME}(" +
                    $"{DbTable.F_Material_Statistics.PRODUCT_TYPE_NO}," +
                    $"{DbTable.F_Material_Statistics.STATION_NAME}," +
                    $"{DbTable.F_Material_Statistics.MATERIAL_CODE}," +
                    $"{DbTable.F_Material_Statistics.MATERIAL_AMOUNT}," +
                    $"{DbTable.F_Material_Statistics.TEAM_LEADER}," +
                    $"{DbTable.F_Material_Statistics.ADMIN}," +
                    $"{DbTable.F_Material_Statistics.UPDATE_DATE}) VALUES(" +
                    $"'{productTypeNo}','{stationName}','{materialCode}','{amounted}','{teamLeader}','{admin}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";

                #endregion

                int row = 0;
                if (!IsExistMaterialData(array))
                {
                    //插入
                    row = SQLServer.ExecuteNonQuery(insertSQL);
                    if (row > 0)
                    {
                        //插入成功
                        var iuRes = UpdateMaterialAmounted(materialCode, int.Parse(amounted));//更新计数
                        var isRes = UpdateMaterialState(materialCode);//更新状态
                        if (iuRes > 0 && isRes > 0)
                        {
                            //更新物料使用数量成功
                            return ConvertMaterialStatisticsCode(MaterialStatisticsReturnCode.STATUS_USCCESS);
                        }
                    }
                    return ConvertMaterialStatisticsCode(MaterialStatisticsReturnCode.STATUS_FAIL);
                }

                //更新物料统计
                int originNum = SelectLastInsertAmount(productTypeNo, stationName, materialCode);
                var uRes = UpdateMaterialAmounted(materialCode, int.Parse(amounted) - originNum);//更新计数
                var sRes = UpdateMaterialState(materialCode);//更新状态
                if (uRes > 0 && sRes > 0)
                {
                    //更新物料使用数量成功
                    return ConvertMaterialStatisticsCode(MaterialStatisticsReturnCode.STATUS_USCCESS);
                }
                return ConvertMaterialStatisticsCode(MaterialStatisticsReturnCode.STATUS_FAIL);
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message);
                return "ERROR";
            }
        }

        private static bool IsExistMaterialData(string[] array)
        {
            var productTypeNo = array[0];
            var stationName = array[1];
            var materialCode = array[2];
            var amounted = array[3];
            var selectSQL = $"SELECT * FROM {DbTable.F_MATERIAL_STATISTICS_NAME} WHERE " +
                $"{DbTable.F_Material_Statistics.PRODUCT_TYPE_NO} = '{productTypeNo}' AND " +
                $"{DbTable.F_Material_Statistics.STATION_NAME} = '{stationName}' AND " +
                $"{DbTable.F_Material_Statistics.MATERIAL_CODE} = '{materialCode}' AND " +
                $"{DbTable.F_Material_Statistics.MATERIAL_AMOUNT} = '{amounted}'";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            return false;
        }

        private static int SelectLastInsertAmount(string type_no, string stationName, string code)
        {
            var selectSQL = $"SELECT {DbTable.F_Material_Statistics.MATERIAL_AMOUNT} " +
                $"FROM {DbTable.F_MATERIAL_STATISTICS_NAME} WHERE " +
                $"{DbTable.F_Material_Statistics.STATION_NAME} = '{stationName}' AND " +
                $"{DbTable.F_Material_Statistics.PRODUCT_TYPE_NO} = '{type_no}' AND " +
                $"{DbTable.F_Material_Statistics.MATERIAL_CODE} = '{code}'";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                return int.Parse(dt.Rows[0][0].ToString());
            }
            return 0;
        }

        //更新产品物料数量
        private static int UpdateMaterialAmounted(string materialCode,int amounted)
        {
            var updateSQL = $"UPDATE {DbTable.F_MATERIAL_NAME} SET " +
                $"{DbTable.F_Material.MATERIAL_AMOUNTED} += '{amounted}' " +
                $"WHERE " +
                $"{DbTable.F_Material.MATERIAL_CODE} = '{materialCode}'";
            return SQLServer.ExecuteNonQuery(updateSQL);
        }

        /// <summary>
        /// 更新物料状态 0-fail,1-success
        /// </summary>
        /// <param name="materialCode"></param>
        /// <returns></returns>
        private static int UpdateMaterialState(string materialCode)
        {
            var selectSQL = $"SELECT {DbTable.F_Material.MATERIAL_STOCK},{DbTable.F_Material.MATERIAL_AMOUNTED} " +
                $"FROM {DbTable.F_MATERIAL_NAME} " +
                $"WHERE " +
                $"{DbTable.F_Material.MATERIAL_CODE} = '{materialCode}'";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                var stock = int.Parse(dt.Rows[0][0].ToString());
                var amounted = int.Parse(dt.Rows[0][1].ToString());
                if (stock == amounted)
                {
                    //物料已使用完，更新状态为2
                    var updateSQL = $"UPDATE {DbTable.F_MATERIAL_NAME} SET " +
                        $"{DbTable.F_Material.MATERIAL_STATE} = '1' WHERE " +
                        $"{DbTable.F_Material.MATERIAL_CODE} = '{materialCode}'";
                    return  SQLServer.ExecuteNonQuery(updateSQL);
                }
                return 1;
            }
            return 0;
        }
    }
    public class MaterialParams
    {
        /// <summary>
        /// PCBA
        /// </summary>
        public string MaterialPCBA { get; set; }

        /// <summary>
        /// 外壳
        /// </summary>
        public string MaterialOutterShell { get; set; }

        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductTypeNo { get; set; }

        /// <summary>
        /// 站位名称
        /// </summary>
        public string StationName { get; set; }

        /// <summary>
        /// 上盖
        /// </summary>
        public string MaterialTopCover { get; set; }

        /// <summary>
        /// 上壳
        /// </summary>
        public string MaterialUpperShell { get; set; }

        /// <summary>
        /// 下壳
        /// </summary>
        public string MaterialLowerShell { get; set; }

        /// <summary>
        /// 线束
        /// </summary>
        public string MaterialWirebean { get; set; }

        /// <summary>
        /// 支架板
        /// </summary>
        public string MaterialSupportPlate { get; set; }

        /// <summary>
        /// 泡棉
        /// </summary>
        public string MaterialBubbleCotton { get; set; }

        /// <summary>
        /// 临时支架
        /// </summary>
        public string MaterialTempStent { get; set; }

        /// <summary>
        /// 最终支架
        /// </summary>
        public string MaterialFinalStent { get; set; }

        /// <summary>
        /// 小螺钉
        /// </summary>
        public string MaterialLittleScrew { get; set; }

        /// <summary>
        /// 长螺钉
        /// </summary>
        public string MaterialLongScrew { get; set; }

        /// <summary>
        /// 螺丝/螺母
        /// </summary>
        public string MaterialScrewNut { get; set; }

        /// <summary>
        /// 防水圈
        /// </summary>
        public string MaterialWaterProofRing { get; set; }

        /// <summary>
        /// 密封圈
        /// </summary>
        public string MaterialSealRing { get; set; }

        /// <summary>
        /// 使用数量
        /// </summary>
        public string MaterialUseAmount { get; set; }

        /// <summary>
        /// 班组长
        /// </summary>
        public string TeamLeader { get; set; }

        /// <summary>
        /// 管理员
        /// </summary>
        public string Admin { get; set; }
    }
}