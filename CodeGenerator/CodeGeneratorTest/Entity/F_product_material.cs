/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				F_product_material.cs
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
    /// 实体类F_product_material
    /// </summary>
    [Serializable]
    public class F_product_material
    {
        #region 私有字段

        private string _type_no = String.Empty;
        private string _material_code = String.Empty;
        private int _amounted = 0;
        private string _describle = String.Empty;
        private DateTime _update_date = DateTime.Now;
        private string _username = String.Empty;


        #endregion

        #region 公有属性


        public string Type_no
        {
            set { this._type_no = value; }
            get { return this._type_no; }
        }


        public string Material_code
        {
            set { this._material_code = value; }
            get { return this._material_code; }
        }


        public int Amounted
        {
            set { this._amounted = value; }
            get { return this._amounted; }
        }


        public string Describle
        {
            set { this._describle = value; }
            get { return this._describle; }
        }


        public DateTime Update_date
        {
            set { this._update_date = value; }
            get { return this._update_date; }
        }


        public string Username
        {
            set { this._username = value; }
            get { return this._username; }
        }



        #endregion	
    }
}
