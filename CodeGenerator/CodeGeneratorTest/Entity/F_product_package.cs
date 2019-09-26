/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				F_product_package.cs
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
    /// 实体类F_product_package
    /// </summary>
    [Serializable]
    public class F_product_package
    {
        #region 私有字段

        private string _out_case_code = String.Empty;
        private string _sn_outter = String.Empty;
        private string _type_no = String.Empty;
        private string _station_name = String.Empty;
        private string _binding_state = String.Empty;
        private DateTime _binding_date = DateTime.Now;
        private string _remark = String.Empty;
        private string _team_leader = String.Empty;
        private string _admin = String.Empty;
        private DateTime _update_date = DateTime.Now;


        #endregion

        #region 公有属性


        public string Out_case_code
        {
            set { this._out_case_code = value; }
            get { return this._out_case_code; }
        }


        public string Sn_outter
        {
            set { this._sn_outter = value; }
            get { return this._sn_outter; }
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


        public string Binding_state
        {
            set { this._binding_state = value; }
            get { return this._binding_state; }
        }


        public DateTime Binding_date
        {
            set { this._binding_date = value; }
            get { return this._binding_date; }
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


        public DateTime Update_date
        {
            set { this._update_date = value; }
            get { return this._update_date; }
        }



        #endregion	
    }
}
