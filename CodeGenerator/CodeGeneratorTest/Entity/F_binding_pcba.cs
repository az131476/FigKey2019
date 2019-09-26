/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				F_binding_pcba.cs
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
    /// 实体类F_binding_pcba
    /// </summary>
    [Serializable]
    public class F_binding_pcba
    {
        #region 私有字段

        private string _sn_pcba = String.Empty;
        private string _sn_outter = String.Empty;
        private string _material_code = String.Empty;
        private DateTime _update_date = DateTime.Now;
        private string _type_no = String.Empty;


        #endregion

        #region 公有属性


        public string Sn_pcba
        {
            set { this._sn_pcba = value; }
            get { return this._sn_pcba; }
        }


        public string Sn_outter
        {
            set { this._sn_outter = value; }
            get { return this._sn_outter; }
        }


        public string Material_code
        {
            set { this._material_code = value; }
            get { return this._material_code; }
        }


        public DateTime Update_date
        {
            set { this._update_date = value; }
            get { return this._update_date; }
        }


        public string Type_no
        {
            set { this._type_no = value; }
            get { return this._type_no; }
        }



        #endregion	
    }
}
