﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesAPI.DB
{
    public class DbTable
    {
        public const string F_USER_NAME                         = "[WT_SCL].[dbo].[f_user]";
        public const string F_TECHNOLOGICAL_PROCESS_NAME        = "[WT_SCL].[dbo].[f_technological_process]";
        public const string F_TEST_RESULT_NAME                  = "[WT_SCL].[dbo].[f_test_result_data]";
        public const string F_PRODUCT_STATION_NAME              = "[WT_SCL].[dbo].[f_product_station]";
        //public const string F_PRODUCT_TYPE_NO_NAME = "[WT_SCL].[dbo].[f_product_typeNo]";
        public const string F_PRODUCT_MATERIAL_NAME             = "[WT_SCL].[dbo].[f_product_material]";
        public const string F_MATERIAL_NAME                     = "[WT_SCL].[dbo].[f_material]";
        public const string F_MATERIAL_STATISTICS_NAME          = "[WT_SCL].[dbo].[f_material_statistics]";
        public const string F_OUT_CASE_STORAGE_NAME             = "[WT_SCL].[dbo].[f_product_package_storage]";
        public const string F_OUT_CASE_PRODUCT_NAME             = "[WT_SCL].[dbo].[f_product_package]";
        public const string F_PRODUCT_CHECK_RECORD_NAME         = "[WT_SCL].[dbo].[f_product_check_record]";
        public const string F_PASS_RATE_STATISTICS_NAME         = "[WT_SCL].[dbo].[f_pass_rate_statistics]";
        public const string F_TEST_PROGRAME_VERSION_NAME        = "[WT_SCL].[dbo].[f_test_programe_version]";
        public const string F_TEST_LIMIT_CONFIG_NAME            = "[WT_SCL].[dbo].[f_test_limit_config]";

        public class F_User
        {
            public const string USER_NAME = "[username]";
            public const string PASS_WORD = "[password]";
            public const string PHONE = "[phone]";
            public const string EMAIL = "[email]";
            public const string CREATE_DATE = "[create_date]";
            public const string UPDATE_DATE = "[update_date]";
            public const string STATUS = "[status]";
            public const string ROLE_NAME = "[role_name]";
        }

        public class F_TECHNOLOGICAL_PROCESS
        {
            public const string PROCESS_NAME = "[process_name]";
            public const string STATION_ORDER = "[station_order]";
            public const string STATION_NAME = "[station_name]";
            public const string UPDATE_DATE = "[update_date]";
            public const string USER_NAME = "[username]";
            public const string PROCESS_STATE = "[pstate]";
        }

        public class F_Product_Station
        {
            public const string TYPE_NO = "[type_no]";
            public const string STATION_ORDER = "[station_order]";
            public const string STATION_NAME = "[station_name]";
        }

        public class F_Test_Result
        {
            public const string PROCESS_NAME = "[process_name]";
            public const string SN = "[sn]";
            public const string TYPE_NO = "[type_no]";
            public const string STATION_NAME = "[station_name]";
            public const string TEST_RESULT = "[test_result]";
            public const string CREATE_DATE = "[create_date]";
            public const string UPDATE_DATE = "[update_date]";
            public const string REMARK = "[remark]";
            public const string TEAM_LEADER = "team_leader";
            public const string ADMIN = "[admin]";
        }

        public class F_TypeNo
        {
            public const string ID = "[id]";
            public const string TYPE_NO = "[type_no]";
        }

        public class F_Material
        {
            public const string MATERIAL_CODE = "[material_code]";
            public const string MATERIAL_NAME = "[material_name]";
            public const string MATERIAL_USER = "[username]";
            public const string UPDATE_DATE   = "[update_date]";
        }

        public class F_PRODUCT_MATERIAL
        {
            public const string TYPE_NO = "[type_no]";
            public const string MATERIAL_CODE = "[material_code]";
            public const string STOCK = "[stock]";
            public const string AMOUNTED = "[amounted]";
            public const string Describle = "[describle]";
            public const string UpdateDate = "[update_date]";
            public const string USERNAME = "[username]";
        }

        public class F_Material_Statistics
        {
            public const string SN_INNER = "[sn_inner]";
            public const string SN_OUTTER = "[sn_outter]";
            public const string TYPE_NO = "[type_no]";
            public const string STATION_NAME = "[station_name]";
            public const string MATERIAL_CODE = "[material_code]";
            public const string MATERIAL_AMOUNT = "[material_amount]";
            public const string UPDATE_DATE = "[update_date]";
        }

        public class F_Out_Case_Storage
        {
            public const string PRODUCT_TYPE_NO = "[type_no]";
            public const string USER_NAME = "[username]";
            public const string STORAGE_CAPACITY = "[storage_capacity]";
            public const string UPDATE_DATE_U = "[update_date_u]";
        }

        public class F_Out_Case_Product
        {
            public const string OUT_CASE_CODE = "[out_case_code]";
            public const string SN_OUTTER = "[sn_outter]";
            public const string TYPE_NO = "[type_no]";
            public const string PICTURE = "[picture]";
            public const string BINDING_STATE = "[binding_state]";
            public const string BINDING_DATE = "[binding_date]";
            public const string REMARK = "[remark]";
        }

        public class F_Product_Check_Record
        {
            public const string OUT_CASE_CODE = "[out_case_code]";
            public const string SN_OUTTER = "[sn_outter]";
            public const string TYPE_NO = "[type_no]";
            public const string STATION_NAME = "station_name";
            public const string PICTURE = "[picture]";
            public const string BINDING_STATE = "[binding_state]";
            public const string BINDING_DATE = "[binding_date]";
            public const string REMARK = "[remark]";
            public const string TEAM_LEADER = "[team_leader]";
            public const string ADMIN = "[admin]";
            public const string UPDATE_DATE = "[update_date]";
        }

        public class F_Pass_Rate_Statistics
        {
            public const string OUT_CASE_CODE = "[out_case_code]";
            public const string SN_OUTTER = "[sn_outter]";
            public const string TYPE_NO = "[type_no]";
            public const string PRIORITY = "[priority]";
            public const string AMOUNT = "[amount]";
            public const string STORAGE_CAPACITY = "[storage_capacity]";
            public const string UPDATE_DATE = "[update_date]";
        }

        public class F_TEST_PROGRAME_VERSION
        {
            public const string TYPE_NO = "[type_no]";
            public const string STATION_NAME = "[station_name]";
            public const string PROGRAME_NAME = "[programe_name]";
            public const string PROGRAME_VERSION = "[programe_version]";
            public const string TEAM_LEADER = "[team_leader]";
            public const string ADMIN = "[admin]";
            public const string UPDATE_DATE = "[update_date]";
        }

        public class F_TEST_LIMIT_CONFIG
        {
            public const string STATION_NAME = "station_name";
            public const string TYPE_NO = "type_no";
            public const string LIMIT_VALUE = "limit_value";
            public const string TEAM_LEADER = "team_leader";
            public const string ADMIN = "admin";
            public const string UPDATE_DATE = "update_date";
        }
    }
}