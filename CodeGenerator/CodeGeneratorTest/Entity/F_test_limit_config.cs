/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				F_test_limit_config.cs
 *      Description:
 *		
 *      Author:
 *				��С��
 *				1297953037@qq.com
 *				http://www.figkey.com
 *      Finish DateTime:
 *				2019��09��26��
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Entity
{
    /// <summary>
    /// ʵ����F_test_limit_config
    /// </summary>
    [Serializable]
    public class F_test_limit_config
    {
        #region ˽���ֶ�

        private string _type_no = String.Empty;
        private string _station_name = String.Empty;
        private string _test_item = String.Empty;
        private string _limit = String.Empty;
        private string _team_leader = String.Empty;
        private string _admin = String.Empty;
        private DateTime _update_date = DateTime.Now;


        #endregion

        #region ��������


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


        public string Test_item
        {
            set { this._test_item = value; }
            get { return this._test_item; }
        }


        public string Limit
        {
            set { this._limit = value; }
            get { return this._limit; }
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
