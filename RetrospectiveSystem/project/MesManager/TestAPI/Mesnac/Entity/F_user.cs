/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				F_user.cs
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
    /// ʵ����F_user
    /// </summary>
    [Serializable]
    public class F_user
    {
        #region ˽���ֶ�

        private string _username = String.Empty;
        private string _password = String.Empty;
        private string _role_name = String.Empty;
        private string _phone = String.Empty;
        private string _email = String.Empty;
        private DateTime _create_date = DateTime.Now;
        private DateTime _update_date = DateTime.Now;
        private int _status = 0;


        #endregion

        #region ��������


        public string Username
        {
            set { this._username = value; }
            get { return this._username; }
        }


        public string Password
        {
            set { this._password = value; }
            get { return this._password; }
        }


        public string Role_name
        {
            set { this._role_name = value; }
            get { return this._role_name; }
        }


        public string Phone
        {
            set { this._phone = value; }
            get { return this._phone; }
        }


        public string Email
        {
            set { this._email = value; }
            get { return this._email; }
        }


        public DateTime Create_date
        {
            set { this._create_date = value; }
            get { return this._create_date; }
        }


        public DateTime Update_date
        {
            set { this._update_date = value; }
            get { return this._update_date; }
        }


        public int Status
        {
            set { this._status = value; }
            get { return this._status; }
        }



        #endregion	
    }
}
