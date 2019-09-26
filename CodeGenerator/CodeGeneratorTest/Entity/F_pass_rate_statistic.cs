/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				F_pass_rate_statistic.cs
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
    /// 实体类F_pass_rate_statistic
    /// </summary>
    [Serializable]
    public class F_pass_rate_statistic
    {
        #region 私有字段

        private string _out_case_code = String.Empty;
        private string _sn_outter = String.Empty;
        private string _type_no = String.Empty;
        private int _priority = 0;
        private int _amount = 0;
        private string _storage_capacity = String.Empty;
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


        public int Priority
        {
            set { this._priority = value; }
            get { return this._priority; }
        }


        public int Amount
        {
            set { this._amount = value; }
            get { return this._amount; }
        }


        public string Storage_capacity
        {
            set { this._storage_capacity = value; }
            get { return this._storage_capacity; }
        }


        public DateTime Update_date
        {
            set { this._update_date = value; }
            get { return this._update_date; }
        }



        #endregion	
    }
}
