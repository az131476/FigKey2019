/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				F_test_programe_version.cs
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
    /// 实体类F_test_programe_version
    /// </summary>
    [Serializable]
    public class F_test_programe_version
    {
        #region 私有字段

        private string _type_no = String.Empty;
        private string _station_name = String.Empty;
        private string _programe_name = String.Empty;
        private string _programe_version = String.Empty;
        private string _team_leader = String.Empty;
        private string _admin = String.Empty;
        private DateTime _update_date = DateTime.Now;


        #endregion

        #region 公有属性


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


        public string Programe_name
        {
            set { this._programe_name = value; }
            get { return this._programe_name; }
        }


        public string Programe_version
        {
            set { this._programe_version = value; }
            get { return this._programe_version; }
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
