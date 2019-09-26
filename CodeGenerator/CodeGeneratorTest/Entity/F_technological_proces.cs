/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				F_technological_proces.cs
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
    /// 实体类F_technological_proces
    /// </summary>
    [Serializable]
    public class F_technological_proces
    {
        #region 私有字段

        private string _process_name = String.Empty;
        private int _station_order = 0;
        private string _station_name = String.Empty;
        private DateTime _update_date = DateTime.Now;
        private string _username = String.Empty;
        private int _pstate = 0;


        #endregion

        #region 公有属性


        public string Process_name
        {
            set { this._process_name = value; }
            get { return this._process_name; }
        }


        public int Station_order
        {
            set { this._station_order = value; }
            get { return this._station_order; }
        }


        public string Station_name
        {
            set { this._station_name = value; }
            get { return this._station_name; }
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


        public int Pstate
        {
            set { this._pstate = value; }
            get { return this._pstate; }
        }



        #endregion	
    }
}
