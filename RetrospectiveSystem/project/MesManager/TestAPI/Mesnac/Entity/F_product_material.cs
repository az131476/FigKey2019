/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				F_product_material.cs
 *      Description:
 *		
 *      Author:
 *				����
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2019��09��25��
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace Mesnac.Entity
{
    /// <summary>
    /// ʵ����F_product_material
    /// </summary>
    [Serializable]
    public class F_product_material
    {
        #region ˽���ֶ�

        private string _type_no = String.Empty;
        private string _material_code = String.Empty;
        private int _amounted = 0;
        private string _describle = String.Empty;
        private DateTime _update_date = DateTime.Now;
        private string _username = String.Empty;


        #endregion

        #region ��������


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
