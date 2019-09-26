/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				F_test_result_data.cs
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
    /// 实体类F_test_result_data
    /// </summary>
    [Serializable]
    public class F_test_result_data
    {
        #region 私有字段

        private string _process_name = String.Empty;
        private string _sn = String.Empty;
        private string _type_no = String.Empty;
        private string _station_name = String.Empty;
        private string _test_result = String.Empty;
        private DateTime _station_in_date = DateTime.Now;
        private DateTime _station_out_date = DateTime.Now;
        private DateTime _create_date = DateTime.Now;
        private DateTime _update_date = DateTime.Now;
        private string _remark = String.Empty;
        private string _team_leader = String.Empty;
        private string _admin = String.Empty;
        private string _joinDateTime = String.Empty;


        #endregion

        #region 公有属性


        public string Process_name
        {
            set { this._process_name = value; }
            get { return this._process_name; }
        }


        public string Sn
        {
            set { this._sn = value; }
            get { return this._sn; }
        }


        public string Type_no
        {
            set { this._type_no = value; }
            get { return this._type_no; }
        }


        public string Station_name
        {
            set { this._station_name = value; }
            get { return this._station_name; }
        }


        public string Test_result
        {
            set { this._test_result = value; }
            get { return this._test_result; }
        }


        public DateTime Station_in_date
        {
            set { this._station_in_date = value; }
            get { return this._station_in_date; }
        }


        public DateTime Station_out_date
        {
            set { this._station_out_date = value; }
            get { return this._station_out_date; }
        }


        public DateTime Create_date
        {
            set { this._create_date = value; }
            get { return this._create_date; }
        }


        public DateTime Update_date
        {
            set { this._update_date = value; }
            get { return this._update_date; }
        }


        public string Remark
        {
            set { this._remark = value; }
            get { return this._remark; }
        }


        public string Team_leader
        {
            set { this._team_leader = value; }
            get { return this._team_leader; }
        }


        public string Admin
        {
            set { this._admin = value; }
            get { return this._admin; }
        }


        public string JoinDateTime
        {
            set { this._joinDateTime = value; }
            get { return this._joinDateTime; }
        }



        #endregion	
    }
}
