using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CodeGenerator
{
    /// <summary>
    /// 数据访问辅助类
    /// </summary>
    public class DBHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static string CONSTR = String.Format("Server={0};Database={1};uid={2};pwd={3}", GlobalConfig.Item.Server, GlobalConfig.Item.DataBase, GlobalConfig.Item.UID, GlobalConfig.Item.PWD);

        public static string ConnectionString
        {
            get
            {
                return String.Format("Server={0};Database={1};uid={2};pwd={3}", GlobalConfig.Item.Server, GlobalConfig.Item.DataBase, GlobalConfig.Item.UID, GlobalConfig.Item.PWD);
            }
        }
        /// <summary>
        /// 执行查询的方法
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">执行的命令类型</param>
        /// <param name="sql">要执行的查询语句</param>
        /// <param name="values">查询语句中的参数列表</param>
        /// <returns>返回数据读取器</returns>
        public static SqlDataReader GetReader(string connectionString, CommandType cmdType, string sql, SqlParameter[] values)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = cmdType;
            cmd.CommandText = sql;
            if (values != null) cmd.Parameters.AddRange(values);
            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return reader;
        }
        /// <summary>
        /// 执行查询的方法
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="cmdType">执行的命令类型</param>
        /// <param name="sql">要执行的查询语句</param>
        /// <param name="values">查询语句中的参数列表</param>
        /// <returns>返回数据集</returns>
        public static DataSet GetDataSet(string connectionString, CommandType cmdType, string sql, SqlParameter[] values)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand();
                adapter.SelectCommand.Connection = con;
                adapter.SelectCommand.CommandType = cmdType;
                adapter.SelectCommand.CommandText = sql;
                if (values != null) adapter.SelectCommand.Parameters.AddRange(values);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
        }
    }
}
