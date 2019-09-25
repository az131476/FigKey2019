using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace MesnacCoding
{
    /// <summary>
    /// ���ݷ��ʸ�����
    /// </summary>
    public class DBHelper
    {
        /// <summary>
        /// �����ַ���
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
        /// ִ�в�ѯ�ķ���
        /// </summary>
        /// <param name="connectionString">���ݿ������ַ���</param>
        /// <param name="cmdType">ִ�е���������</param>
        /// <param name="sql">Ҫִ�еĲ�ѯ���</param>
        /// <param name="values">��ѯ����еĲ����б�</param>
        /// <returns>�������ݶ�ȡ��</returns>
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
        /// ִ�в�ѯ�ķ���
        /// </summary>
        /// <param name="connectionString">���ݿ������ַ���</param>
        /// <param name="cmdType">ִ�е���������</param>
        /// <param name="sql">Ҫִ�еĲ�ѯ���</param>
        /// <param name="values">��ѯ����еĲ����б�</param>
        /// <returns>�������ݼ�</returns>
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
