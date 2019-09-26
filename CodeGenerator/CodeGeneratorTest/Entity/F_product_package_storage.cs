/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				F_product_package_storage.cs
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
    /// 实体类F_product_package_storage
    /// </summary>
    [Serializable]
    public class F_product_package_storage
    {
        #region 私有字段

        private string _type_no = String.Empty;
        private int _storage_capacity = 0;
        private int _amounted = 0;
        private string _username = String.Empty;
        private DateTime _update_date_u = DateTime.Now;
        private string _team_leader = String.Empty;
        private string _admin = String.Empty;
        private DateTime _update_date_t = DateTime.Now;
        private string _describle = String.Empty;


        #endregion

        #region 公有属性


        public string Type_no
        {
            set { this._type_no = value; }
            get { return this._type_no; }
        }


        public int Storage_capacity
        {
            set { this._storage_capacity = value; }
            get { return this._storage_capacity; }
        }


        public int Amounted
        {
            set { this._amounted = value; }
            get { return this._amounted; }
        }


        public string Username
        {
            set { this._username = value; }
            get { return this._username; }
        }


        public DateTime Update_date_u
        {
            set { this._update_date_u = value; }
            get { return this._update_date_u; }
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


        public DateTime Update_date_t
        {
            set { this._update_date_t = value; }
            get { return this._update_date_t; }
        }


        public string Describle
        {
            set { this._describle = value; }
            get { return this._describle; }
        }



        #endregion	
    }
}
