/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				F_material.cs
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
    /// 实体类F_material
    /// </summary>
    [Serializable]
    public class F_material
    {
        #region 私有字段

        private string _material_code = String.Empty;
        private string _material_name = String.Empty;
        private int _material_stock = 0;
        private int _material_amounted = 0;
        private int _material_actualAmount = 0;
        private int _material_breakAmount = 0;
        private int _material_state = 0;
        private string _material_describle = String.Empty;
        private string _material_username = String.Empty;
        private DateTime _material_update_date = DateTime.Now;
        private string _team_leader = String.Empty;
        private string _admin = String.Empty;


        #endregion

        #region 公有属性


        public string Material_code
        {
            set { this._material_code = value; }
            get { return this._material_code; }
        }


        public string Material_name
        {
            set { this._material_name = value; }
            get { return this._material_name; }
        }


        public int Material_stock
        {
            set { this._material_stock = value; }
            get { return this._material_stock; }
        }


        public int Material_amounted
        {
            set { this._material_amounted = value; }
            get { return this._material_amounted; }
        }


        public int Material_actualAmount
        {
            set { this._material_actualAmount = value; }
            get { return this._material_actualAmount; }
        }


        public int Material_breakAmount
        {
            set { this._material_breakAmount = value; }
            get { return this._material_breakAmount; }
        }


        public int Material_state
        {
            set { this._material_state = value; }
            get { return this._material_state; }
        }


        public string Material_describle
        {
            set { this._material_describle = value; }
            get { return this._material_describle; }
        }


        public string Material_username
        {
            set { this._material_username = value; }
            get { return this._material_username; }
        }


        public DateTime Material_update_date
        {
            set { this._material_update_date = value; }
            get { return this._material_update_date; }
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



        #endregion	
    }
}
