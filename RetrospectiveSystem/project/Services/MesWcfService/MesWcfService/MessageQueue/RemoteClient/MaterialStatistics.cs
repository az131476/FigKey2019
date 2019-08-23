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

        public static string CheckMaterialState(Queue<string> queue)
        {
            var materialCode = queue.Dequeue();
            var selectSQl = $"SELECT {DbTable.F_Material.MATERIAL_STATE} FROM {DbTable.F_MATERIAL_NAME} WHERE " +
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
            try
            {
                MaterialParams materialParams = new MaterialParams();
                #region material params
                string[] array = queue.Dequeue();
                var materialPCBA = array[0];
                var materialOutterShell = array[1];
                var productTypeNo = array[2];
                var stationName = array[3];
                var materialTopCover = array[4];
                var materialUpperShell = array[5];
                var materialLowerShell = array[6];
                var materialWirebean = array[7];
                var materialSupportPlate = array[8];
                var materialBubbleCotton = array[9];
                var materialTempStent = array[10];
                var materialFinalStent = array[11];
                var materialLittleScrew = array[12];
                var materialLongScrew = array[13];
                var materialScrewNut = array[14];
                var materialWaterProofRing = array[15];
                var materialSealRing = array[16];
                var materialUseAmount = array[17];
                var teamLeader = array[18];
                var admin = array[19];
                materialParams.MaterialPCBA = materialPCBA;
                materialParams.MaterialOutterShell = materialOutterShell;
                materialParams.ProductTypeNo = productTypeNo;
                materialParams.StationName = stationName;
                materialParams.MaterialTopCover = materialTopCover;
                materialParams.MaterialUpperShell = materialUpperShell;
                materialParams.MaterialLowerShell = materialLowerShell;
                materialParams.MaterialWirebean = materialWirebean;
                materialParams.MaterialSupportPlate = materialSupportPlate;
                materialParams.MaterialBubbleCotton = materialBubbleCotton;
                materialParams.MaterialTempStent
                #endregion

                #region insert sql
                var insertSQL = $"INSERT INTO {DbTable.F_MATERIAL_STATISTICS_NAME}(" +
                    $"{DbTable.F_Material_Statistics.MATERIAL_PCBA}," +
                    $"{DbTable.F_Material_Statistics.MATERIAL_OUTTER_SHELL}," +
                    $"{DbTable.F_Material_Statistics.PRODUCT_TYPE_NO}," +
                    $"{DbTable.F_Material_Statistics.STATION_NAME}," +
                    $"{DbTable.F_Material_Statistics.MATERIAL_AMOUNT}," +
                    $"{DbTable.F_Material_Statistics.MATERIAL_AMOUNT}," +
                    $"{DbTable.F_Material_Statistics.UPDATE_DATE}," +
                    $"{DbTable.F_Material_Statistics.TEAM_LEADER}," +
                    $"{DbTable.F_Material_Statistics.ADMIN}) " +
                    $"VALUES('{materialPCBA}','{materialOutterShell}','{productTypeNo}','{stationName}'," +
                    $"'{materialTopCover}','{materialUpperShell}','{materialLowerShell}','{materialWirebean}'," +
                    $"'{materialSupportPlate}','{materialBubbleCotton}','{materialTempStent}','{materialFinalStent}'" +
                    $"'{materialLittleScrew}','{materialLongScrew}','{materialScrewNut}','{materialWaterProofRing}'," +
                    $"'{materialSealRing}','{materialUseAmount}','{teamLeader}','{admin}')";
                #endregion

                int row = 0;
                if (!IsExistMaterialData(array))
                {
                    //插入
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
                $"{DbTable.F_Material_Statistics.SN_INNER} = '{sn_inner}' AND " +
                $"{DbTable.F_Material_Statistics.SN_OUTTER} = '{sn_outter}' AND " +
                $"{DbTable.F_Material_Statistics.STATION_NAME} = '{station_name}' AND " +
                $"{DbTable.F_Material_Statistics.MATERIAL_AMOUNT} = '{material_code}'";
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
                $"{DbTable.F_Material_Statistics.MATERIAL_AMOUNT} = '{code}'";
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
                $"{DbTable.F_Material_Statistics.MATERIAL_AMOUNT} = '{code}'";
            return int.Parse(SQLServer.ExecuteDataSet(selectSQL).Tables[0].Rows[0][0].ToString());
        }

        //更新产品物料数据
        private static void UpdateProductMaterialQuantity(string typeNo,string materialCode,int amounted)
        {
            var updateSQL = $"UPDATE {DbTable.F_PRODUCT_MATERIAL_NAME} SET " +
                $"{DbTable.F_PRODUCT_MATERIAL.AMOUNTED} += '{amounted}' " +
                $"WHERE " +
                $"{DbTable.F_PRODUCT_MATERIAL.TYPE_NO} = '{typeNo}' AND " +
                $"{DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE} = '{materialCode}'";
            SQLServer.ExecuteNonQuery(updateSQL);
        }

        //更新物料
        private static void UpdateMaterial()
        {
            var updateSQL = $"UPDATE {DbTable.F_MATERIAL_NAME} SET " +
                $"{DbTable.F_Material.MATERIAL_AMOUNTED} = '{}'";
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