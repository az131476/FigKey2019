/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				F_material_pn.cs
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
    /// 实体类F_material_pn
    /// </summary>
    [Serializable]
    public class F_material_pn
    {
        #region 私有字段

        private string _material_pn = String.Empty;
        private string _material_name = String.Empty;
        private string _user_name = String.Empty;
        private DateTime _update_date = DateTime.Now;
        private string _describle = String.Empty;


        #endregion

        #region 公有属性


        public string Material_pn
        {
            set { this._material_pn = value; }
            get { return this._material_pn; }
        }


        public string Material_name
        {
            set { this._material_name = value; }
            get { return this._material_name; }
        }


        public string User_name
        {
            set { this._user_name = value; }
            get { return this._user_name; }
        }


        public DateTime Update_date
        {
            set { this._update_date = value; }
            get { return this._update_date; }
        }


        public string Describle
        {
            set { this._describle = value; }
            get { return this._describle; }
        }



        #endregion	
    }
}
