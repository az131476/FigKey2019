/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				F_quanlity_manager.cs
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
    /// 实体类F_quanlity_manager
    /// </summary>
    [Serializable]
    public class F_quanlity_manager
    {
        #region 私有字段

        private int _except_type = 0;
        private string _material_code = String.Empty;
        private DateTime _statement_date = DateTime.Now;
        private int _except_stock = 0;
        private int _actual_stock = 0;
        private string _station_name = String.Empty;
        private int _material_state = 0;
        private string _statement_reason = String.Empty;
        private string _statement_user = String.Empty;
        private DateTime _update_date = DateTime.Now;


        #endregion

        #region 公有属性


        public int Except_type
        {
            set { this._except_type = value; }
            get { return this._except_type; }
        }


        public string Material_code
        {
            set { this._material_code = value; }
            get { return this._material_code; }
        }


        public DateTime Statement_date
        {
            set { this._statement_date = value; }
            get { return this._statement_date; }
        }


        public int Except_stock
        {
            set { this._except_stock = value; }
            get { return this._except_stock; }
        }


        public int Actual_stock
        {
            set { this._actual_stock = value; }
            get { return this._actual_stock; }
        }


        public string Station_name
        {
            set { this._station_name = value; }
            get { return this._station_name; }
        }


        public int Material_state
        {
            set { this._material_state = value; }
            get { return this._material_state; }
        }


        public string Statement_reason
        {
            set { this._statement_reason = value; }
            get { return this._statement_reason; }
        }


        public string Statement_user
        {
            set { this._statement_user = value; }
            get { return this._statement_user; }
        }


        public DateTime Update_date
        {
            set { this._update_date = value; }
            get { return this._update_date; }
        }



        #endregion	
    }
}
