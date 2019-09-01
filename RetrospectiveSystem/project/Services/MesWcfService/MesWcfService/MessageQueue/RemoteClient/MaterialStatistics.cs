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
            return CheckMaterialTypeMatch(productTypeNo,materialPN);
        }

        private static string CheckMaterialTypeMatch(string productTypeNo,string materialPN)
        {
            var selectSQL = $"SELECT * FROM {DbTable.F_PRODUCT_MATERIAL_NAME}  WHERE " +
                $"{DbTable.F_PRODUCT_MATERIAL.TYPE_NO} = '{productTypeNo}' AND " +
                $"{DbTable.F_PRODUCT_MATERIAL.MATERIAL_CODE} = '{materialPN}'";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            if (dt.Rows.Count == 1)
                return ConvertCheckMaterialMatch(MaterialCheckMatchReturnCode.IS_MATCH);
            return ConvertCheckMaterialMatch(MaterialCheckMatchReturnCode.IS_NOT_MATCH);
        }

            /*物料数量防错：即该种物料使用完后才能继续扫描新的物料使用
             * 防错原理：
             * 1）传入产品型号+物料完整编码
             * 2）根据传入的产品型号+物料号=》查询产品统计表，是否有使用记录？
             *      （1）有使用记录：
             *              则继续查询出使用记录中的所有物料编码
             *              将传入物料编码与查询出的编码匹配，是否有一样的？
             *                  （1）匹配到相同的编码：则说明正在使用该物料编码；此时，在物料信息表中查询该物料状态反馈即可
             *                  （2）没有匹配到相同编码：说明传入的物料编码为新扫描的物料编码，
             *                       此时，将物料统计表中查询出的所有物料编码，查询出不一致的状态反馈
             *      （2）无使用记录：则为第一次扫描该物料，只需在物料信息表查询该物料编码的状态反馈即可
             */ 
        public static string CheckMaterialState(Queue<string[]> queue)
        {
            var array = queue.Dequeue();
            var productTypeNo = array[0];
            var materialCode = array[1];
            if(!materialCode.Contains("@"))
                return ConvertCheckMaterialStateCode(MaterialStateReturnCode.ERROR_FORMAT_MATERIAL_CODE);
            var materialPN = materialCode.Substring(0,materialCode.IndexOf('@'));

            //根据物料号+产品型号查询是否有统计计数记录
            var selectRecordSQL = $"SELECT {DbTable.F_Material_Statistics.MATERIAL_CODE} FROM " +
                $"{DbTable.F_MATERIAL_STATISTICS_NAME} WHERE " +
                $"{DbTable.F_Material_Statistics.PRODUCT_TYPE_NO} = '{productTypeNo}' AND " +
                $"{DbTable.F_Material_Statistics.MATERIAL_CODE} like '%{materialPN}'";
            var dt = SQLServer.ExecuteDataSet(selectRecordSQL).Tables[0];
            if (dt.Rows.Count > 0)
            {
                /* 有统计记录
                 * 查询统计记录中的所有物料编码
                 * 比对传入物料编码在统计记录中是否存在
                 */
                bool IsSameMaterialCode = false;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var mCode = dt.Rows[i][0].ToString();
                    if (materialCode == mCode)
                    {
                        IsSameMaterialCode = true;
                    }
                }
                if (IsSameMaterialCode)
                {
                    //存在相同编码
                    return SelectMaterialState(materialCode);
                }
                else
                {
                    /* 不存在相同编码
                     * 则说明传入物料编码为新扫描编码
                     * 则查询所有已知编码的状态，是否都是使用完成状态，否则提示不能继续使用新扫描的物料
                     */
                    bool IsUseComplete = true;//默认使用完成
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var mCode = dt.Rows[i][0].ToString();

                        var selectSQl = $"SELECT {DbTable.F_Material.MATERIAL_STATE} " +
                                        $"FROM {DbTable.F_MATERIAL_NAME} WHERE " +
                                        $"{DbTable.F_Material.MATERIAL_CODE} = '{mCode}'";
                        var mdt = SQLServer.ExecuteDataSet(selectSQl).Tables[0];
                        if (mdt.Rows.Count > 0)
                        {
                            var mState = mdt.Rows[0][0].ToString();
                            if (mState == "1")
                            {
                                //有物料未使用完，请使用完了在扫描别的箱使用
                                IsUseComplete = false;
                            }
                        }
                    }
                    if (IsUseComplete)
                    {
                        //使用完成
                        return ConvertCheckMaterialStateCode(MaterialStateReturnCode.STATUS_COMPLETE_NORMAL);
                    }
                    else
                    {
                        //有未使用完成物料
                        return ConvertCheckMaterialStateCode(MaterialStateReturnCode.STATUS_USING);
                    }
                }
            }
            else
            {
                /* 无统计记录/为防止改物料在之前未进行物料号防错，此步骤可再次验证物料防错
                 * 则为第一次扫描该物料编码
                 * 直接查询物料信息表中该物料状态反馈即可
                 */
                LogHelper.Log.Info("【物料防错-状态-无统计记录-直接查询状态】");
                string cRes = CheckMaterialTypeMatch(productTypeNo, materialPN);
                if (cRes == ConvertCheckMaterialMatch(MaterialCheckMatchReturnCode.IS_NOT_MATCH))
                {
                    LogHelper.Log.Info("【物料防错-状态-物料号与当前产品不匹配】");
                    return ConvertCheckMaterialStateCode(MaterialStateReturnCode.ERROR_MATRIAL_CODE_IS_NOT_MATCH_WITH_PRODUCT_TYPENO);
                }
                return SelectMaterialState(materialCode);
            }
        }

        private static string SelectMaterialState(string materialCode)
        {
            var selectSQl = $"SELECT {DbTable.F_Material.MATERIAL_STATE} " +
               $"FROM {DbTable.F_MATERIAL_NAME} WHERE " +
               $"{DbTable.F_Material.MATERIAL_CODE} = '{materialCode}'";
            LogHelper.Log.Info(selectSQl);
            var dt = SQLServer.ExecuteDataSet(selectSQl).Tables[0];
            if (dt.Rows.Count < 1)
                return ConvertCheckMaterialStateCode(MaterialStateReturnCode.STATUS_NULL_QUERY);
            var queryRes = dt.Rows[0][0].ToString();
            return "0X" + Convert.ToString(int.Parse(queryRes),16).PadLeft(2, '0');
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