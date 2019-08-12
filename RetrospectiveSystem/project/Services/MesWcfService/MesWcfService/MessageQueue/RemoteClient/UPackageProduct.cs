using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MesWcfService.DB;
using MesWcfService.Model;
using CommonUtils.Logger;
using CommonUtils.DB;

namespace MesWcfService.MessageQueue.RemoteClient
{
    public class UPackageProduct
    {
        private bool IsRecordExist;
        private bool IsProductBinding;
        /// <summary>
        /// 更新数据前check箱子装入总数是否超过总容量，更新成功后，同时更新箱子计数
        /// </summary>
        /// <param name="ppQueue"></param>
        /// <returns></returns>
        public static string UpdatePackageProduct(Queue<string[]> ppQueue)
        {
            try
            {
                var array = ppQueue.Dequeue();
                var outCaseCode = array[0];
                var snOutter = array[1];
                var typeNo = array[2];
                var stationName = array[3];
                var bindingState = array[4];
                var remark = array[5];
                var teamdLeader = array[6];
                var admin = array[7];

                string insertSQL = $"INSERT INTO {DbTable.F_OUT_CASE_PRODUCT_NAME}(" +
                    $"{DbTable.F_Out_Case_Product.OUT_CASE_CODE}," +
                    $"{DbTable.F_Out_Case_Product.SN_OUTTER}," +
                    $"{DbTable.F_Out_Case_Product.TYPE_NO}," +
                    $"{DbTable.F_Out_Case_Product.STATION_NAME}," +
                    $"{DbTable.F_Out_Case_Product.BINDING_STATE}," +
                    $"{DbTable.F_Out_Case_Product.REMARK}," +
                    $"{DbTable.F_Out_Case_Product.TEAM_LEADER}," +
                    $"{DbTable.F_Out_Case_Product.ADMIN}," +
                    $"{DbTable.F_Out_Case_Product.BINDING_DATE}) VALUES(" +
                    $"'{outCaseCode}','{snOutter}','{typeNo}','{stationName}'," +
                    $"'{bindingState}','{remark}',{teamdLeader},'{admin}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}')";

                string updateSQL = $"UPDATE {DbTable.F_OUT_CASE_PRODUCT_NAME} SET " +
                $"{DbTable.F_Out_Case_Product.TYPE_NO} = '{typeNo}'," +
                $"{DbTable.F_Out_Case_Product.BINDING_STATE} = '{bindingState}'," +
                $"{DbTable.F_Out_Case_Product.REMARK} = '{remark}'," +
                $"{DbTable.F_Out_Case_Product.TEAM_LEADER} = '{teamdLeader}'," +
                $"{DbTable.F_Out_Case_Product.ADMIN} = '{admin}'," +
                $"{DbTable.F_Out_Case_Product.UPDATE_DATE} = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' " +
                $"WHERE {DbTable.F_Out_Case_Product.OUT_CASE_CODE} = '{outCaseCode}' AND " +
                $"{DbTable.F_Out_Case_Product.SN_OUTTER} = '{snOutter}' ";
                LogHelper.Log.Info($"UpdatePackageProduct={updateSQL}");
                int res = 0;
                var upackageProduct = IsExist(outCaseCode, snOutter);
                if (upackageProduct.IsRecordExist)
                {
                    //update
                    //是否绑定
                    res = SQLServer.ExecuteNonQuery(updateSQL);
                    if (upackageProduct.IsProductBinding)
                    {
                        //已绑定
                        if(bindingState == "0")
                            UpdateBindingAmount(typeNo, int.Parse(bindingState));
                    }
                    else
                    {
                        //未绑定
                        if(bindingState == "1")
                            UpdateBindingAmount(typeNo, int.Parse(bindingState));
                    }
                }
                else
                {
                    //insert
                    //未满，继续
                    if (IsContinue(typeNo))
                    {
                        res = SQLServer.ExecuteNonQuery(insertSQL);
                        if (res > 0)
                        {
                            //更新计数
                            UpdateBindingAmount(typeNo,int.Parse(bindingState));
                        }
                    }
                    else
                    {
                        return "FULL";
                    }
                }
                if (res > 0)
                    return "OK";
                return "FAIL";
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message);
                return "ERROR";
            }
        }

        private static UPackageProduct IsExist(string caseCode,string snOutter)
        {
            var selectSQL = $"SELECT {DbTable.F_Out_Case_Product.BINDING_STATE} " +
                $"FROM {DbTable.F_OUT_CASE_PRODUCT_NAME} WHERE " +
                $"{DbTable.F_Out_Case_Product.OUT_CASE_CODE} = '{caseCode}' AND " +
                $"{DbTable.F_Out_Case_Product.SN_OUTTER} = '{snOutter}'";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            UPackageProduct uPackageProduct = new UPackageProduct();
            if (dt.Rows.Count > 0)
            {
                uPackageProduct.IsRecordExist = true;
                var value = dt.Rows[0][0].ToString();
                if (value == "0")
                    uPackageProduct.IsProductBinding = false;
                else if (value == "1")
                    uPackageProduct.IsProductBinding = true;
            }
            else
                uPackageProduct.IsRecordExist = false;
            return uPackageProduct;
        }

        //是否可以继续更新数据，未装满时可以继续，装满时不能继续，提示装满
        private static bool IsContinue(string typeNo)
        {
            var selectSQL = $"SELECT {DbTable.F_Out_Case_Storage.STORAGE_CAPACITY}," +
                    $"{DbTable.F_Out_Case_Storage.AMOUNTED} " +
                    $"FROM {DbTable.F_OUT_CASE_STORAGE_NAME} WHERE " +
                    $"{DbTable.F_Out_Case_Storage.TYPE_NO} = '{typeNo}'";
            var dt = SQLServer.ExecuteDataSet(selectSQL).Tables[0];
            var storage = int.Parse(dt.Rows[0][0].ToString());
            var amounted = int.Parse(dt.Rows[0][1].ToString());
            if (amounted >= storage)
                return false;
            return true;
        }

        private static void UpdateBindingAmount(string typeNo,int bindingState)
        {
            int value = 0;
            if (bindingState == 0)
                value = -1;
            else if (bindingState == 1)
                value = 1;
            var updateSQL = $"UPDATE {DbTable.F_OUT_CASE_STORAGE_NAME} SET " +
                $"{DbTable.F_Out_Case_Storage.AMOUNTED} += {value} WHERE " +
                $"{typeNo}";
            SQLServer.ExecuteNonQuery(updateSQL);
        }
    }
}