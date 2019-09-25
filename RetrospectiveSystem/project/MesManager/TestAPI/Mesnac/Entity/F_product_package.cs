/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				F_product_package.cs
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
    /// ʵ����F_product_package
    /// </summary>
    [Serializable]
    public class F_product_package
    {
        #region ˽���ֶ�

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

        #region ��������


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
