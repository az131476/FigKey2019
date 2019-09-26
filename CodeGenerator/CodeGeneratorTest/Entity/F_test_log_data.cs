/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				F_test_log_data.cs
 *      Description:
 *		
 *      Author:
 *				唐小东
 *				1297953037@qq.com
 *				http://www.figkey.com
 *      Finish DateTime:
 *				2019年09月26日
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Entity
{
    /// <summary>
    /// 实体类F_test_log_data
    /// </summary>
    [Serializable]
    public class F_test_log_data
    {
        #region 私有字段

        private string _productTypeNo = String.Empty;
        private string _productSn = String.Empty;
        private string _stationName = String.Empty;
        private string _testItem = String.Empty;
        private string _limit = String.Empty;
        private string _currentValue = String.Empty;
        private string _testResult = String.Empty;
        private string _teamLeader = String.Empty;
        private string _admin = String.Empty;
        private DateTime _updateDate = DateTime.Now;
        private string _joinDateTime = String.Empty;


        #endregion

        #region 公有属性


        public string ProductTypeNo
        {
            set { this._productTypeNo = value; }
            get { return this._productTypeNo; }
        }


        public string ProductSn
        {
            set { this._productSn = value; }
            get { return this._productSn; }
        }


        public string StationName
        {
            set { this._stationName = value; }
            get { return this._stationName; }
        }


        public string TestItem
        {
            set { this._testItem = value; }
            get { return this._testItem; }
        }


        public string Limit
        {
            set { this._limit = value; }
            get { return this._limit; }
        }


        public string CurrentValue
        {
            set { this._currentValue = value; }
            get { return this._currentValue; }
        }


        public string TestResult
        {
            set { this._testResult = value; }
            get { return this._testResult; }
        }


        public string TeamLeader
        {
            set { this._teamLeader = value; }
            get { return this._teamLeader; }
        }


        public string Admin
        {
            set { this._admin = value; }
            get { return this._admin; }
        }


        public DateTime UpdateDate
        {
            set { this._updateDate = value; }
            get { return this._updateDate; }
        }


        public string JoinDateTime
        {
            set { this._joinDateTime = value; }
            get { return this._joinDateTime; }
        }



        #endregion	
    }
}
