/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				BaseService.cs
 *      Description:
 *				 ���ڷ������ݷ��ʳ������
 *      Author:
 *				��С��
 *				1297953037@qq.com
 *				http://www.figkey.com
 *      Finish DateTime:
 *				2019��09��26��
 *      History:
 *      
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using System.Data.SqlClient;
namespace CodeGeneratorTest.Data
{
    using CodeGeneratorTest.Components;
    /// <summary>
    /// ���ڷ������ݷ��ʳ�����࣬��װ�˻������ݷ��ʲ���CRUD
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseService<T> : IBaseService<T>
    {
        #region ˽���ֶ�

        private XmlClassMap classMap;   //ʵ�����ӳ����Ϣ
        private Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();  //ʵ�����������Ϣ
        private string procedureName = "MesnacPaging";   //��ҳ�洢������

        #endregion

        #region ���췽��

        public BaseService()
        {
            //��ȡʵ����T��ӳ����Ϣ
            this.classMap = EntityMapperHandler.GetInstance().GetMapDictionary()[typeof(T).Name];
            //��ȡʵ�����������Ϣ
            PropertyInfo[] pis = typeof(T).GetProperties();
            foreach (PropertyInfo pi in pis)
            {
                if (this.classMap.Properties.ContainsKey(pi.Name))
                {
                    this.properties.Add(this.classMap.Properties[pi.Name].ColumnName, pi);
                }
            }
        }

        #endregion

        #region �ܱ�������

        /// <summary>
        /// ��װ���ݱ����ݵ�ʵ�弯�ϵķ���
        /// </summary>
        /// <param name="cmdType">��������</param>
        /// <param name="cmdText">Ҫִ�е�SQL����洢��������</param>
        /// <param name="values">SQL����洢���̵Ĳ����б�</param>
        /// <returns>����ʵ����ļ���</returns>
        protected List<T> GetBySql(CommandType cmdType, string cmdText, SqlParameter[] values)
        {
            using (SqlDataReader reader = DBHelper.GetReader(DBHelper.CONSTR, cmdType, cmdText, values))
            {
                List<T> lst = new List<T>();
                Type entityType = typeof(T);
                PropertyInfo[] properties = entityType.GetProperties();
                Dictionary<string, XmlClassMap> dic = EntityMapperHandler.GetInstance().GetMapDictionary();
                XmlClassMap classMap = dic[entityType.Name];
                while (reader.Read())
                {
                    T entity = (T)entityType.Assembly.CreateInstance(entityType.FullName);
                    foreach (PropertyInfo property in properties)
                    {
                        if (property.CanWrite && classMap.Properties.ContainsKey(property.Name))
                        {
                            object value = reader[classMap.Properties[property.Name].ColumnName];
                            if (value != null && value != DBNull.Value)
                            {
                                property.SetValue(entity, value, null);
                            }
                        }
                    }
                    lst.Add(entity);
                }
                reader.Close();
                return lst;
            }
        }

        #endregion

        #region IBaseService<T> ��Ա
        /// <summary>
        /// ��ס�����ʶ�в��ң�ֻ���ǵ��ֶ�����������ϼ���ʱ�Ű���������
        /// </summary>
        /// <param name="id">��Ӧ���Ҽ�¼������ֵ���ʶֵ</param>
        /// <returns>���ض�Ӧ��¼��ʵ����Ϣ</returns>
        public T GetById(object id)
        {
            string cmdText = "select * from {0} where {1}";
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (this.classMap.Ids.Keys.Count == 1)
            {
                //��������ֶ���һ���ֶ��������ֶβ�ѯ
                foreach (XmlPropertyMap pm in this.classMap.Ids.Values)
                {
                    where = pm.ColumnName + "=@" + pm.ColumnName;
                    parameters.Add(new SqlParameter("@" + pm.ColumnName, id));
                    break;
                }
            }
            else if (this.classMap.Identity != null)
            {
                //���û�����������������ֶβ�ֹһ�����򰴱�ʶ�в�ѯ
                where = this.classMap.Identity.ColumnName + "=@" + this.classMap.Identity.ColumnName;
                parameters.Add(new SqlParameter("@" + this.classMap.Identity.ColumnName, id));
            }
            else
            {
                //���û�б�ʶ�У�����û�����������������ֶβ�ֻһ���򰴱��еĵ�һ���ֶβ�ѯ
                foreach (XmlPropertyMap pm in this.classMap.Properties.Values)
                {
                    where = pm.ColumnName + "=@" + pm.ColumnName;
                    parameters.Add(new SqlParameter("@" + pm.ColumnName, id));
                    break;
                }
            }
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            T entity = default(T);
            List<T> lst = this.GetBySql(CommandType.Text, cmdText, parameters.ToArray());
            if (lst != null && lst.Count > 0) entity = lst[0];
            return entity;
        }

        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="values">��ѯ�����б�KeyValuePair��Key���ֶ���,KeyValuePair��Value���ֶ�ֵ</param>
        /// <returns>����ʵ����ļ���</returns>
        public List<T> GetListByParam(params KeyValuePair<string, object>[] values)
        {
            string cmdText = "select * from {0} {1}";
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> kvp in values)
            {
                where = String.IsNullOrEmpty(where) ? "where " + kvp.Key + "=@" + kvp.Key : where + " and " + kvp.Key + "=@" + kvp.Key;
                parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            List<T> lst = this.GetBySql(CommandType.Text, cmdText, parameters.ToArray());
            return lst;
        }

        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="values">��ѯ�����б�</param>
        /// <returns>����ʵ����ļ���</returns>
        public List<T> GetListByParam(Dictionary<string, object> values)
        {
            string cmdText = "select * from {0} {1}";
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> kvp in values)
            {
                where = String.IsNullOrEmpty(where) ? "where " + kvp.Key + "=@" + kvp.Key : where + " and " + kvp.Key + "=@" + kvp.Key;
                parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            List<T> lst = this.GetBySql(CommandType.Text, cmdText, parameters.ToArray());
            return lst;
        }
        /// <summary>
        /// ָ��������ѯ������
        /// </summary>
        /// <param name="order">��������</param>
        /// <param name="values">��ѯ�����б�</param>
        /// <returns>����ʵ����ļ���</returns>
        public List<T> GetListOrderByParam(string order, params KeyValuePair<string, object>[] values)
        {
            string cmdText = "select * from {0} {1} {2}";
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> kvp in values)
            {
                where = String.IsNullOrEmpty(where) ? "where " + kvp.Key + "=@" + kvp.Key : where + " and " + kvp.Key + "=@" + kvp.Key;
                parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            if (!String.IsNullOrEmpty(order))
            {
                cmdText = String.Format(cmdText, this.classMap.TableName, where, "order by " + order);
            }
            else
            {
                cmdText = String.Format(cmdText, this.classMap.TableName, where, String.Empty);
            }
            List<T> lst = this.GetBySql(CommandType.Text, cmdText, parameters.ToArray());
            return lst;
        }
        /// <summary>
        /// ָ��������ѯ������
        /// </summary>
        /// <param name="order">��������</param>
        /// <param name="values">��ѯ�����б�</param>
        /// <returns>����ʵ����ļ���</returns>
        public List<T> GetListOrderByParam(string order, Dictionary<string, object> values)
        {
            string cmdText = "select * from {0} {1} {2}";
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> kvp in values)
            {
                where = String.IsNullOrEmpty(where) ? "where " + kvp.Key + "=@" + kvp.Key : where + " and " + kvp.Key + "=@" + kvp.Key;
                parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            if (!String.IsNullOrEmpty(order))
            {
                cmdText = String.Format(cmdText, this.classMap.TableName, where, "order by " + order);
            }
            else
            {
                cmdText = String.Format(cmdText, this.classMap.TableName, where, String.Empty);
            }
            List<T> lst = this.GetBySql(CommandType.Text, cmdText, parameters.ToArray());
            return lst;
        }
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>����ʵ����ļ���</returns>
        public List<T> GetListByWhere(string where)
        {
            string cmdText = "select * from {0} {1}";
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            List<T> lst = this.GetBySql(CommandType.Text, cmdText, null);
            return lst;
        }
        /// <summary>
        /// ָ������������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        public List<T> GetListByWhereAndOrder(string where, string order)
        {
            string cmdText = "select * from {0} {1} {2}";
            if (!String.IsNullOrEmpty(order))
            {
                cmdText = String.Format(cmdText, this.classMap.TableName, where, "order by " + order);
            }
            else
            {
                cmdText = String.Format(cmdText, this.classMap.TableName, where, order);
            }
            List<T> lst = this.GetBySql(CommandType.Text, cmdText, null);
            return lst;
        }
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>�������ݼ�</returns>
        public DataSet GetDataSetByWhere(string where)
        {
            string cmdText = "select * from {0} {1}";
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            DataSet ds = DBHelper.GetDataSet(DBHelper.CONSTR, CommandType.Text, cmdText, null);
            return ds;
        }
        /// <summary>
        /// ָ�������ֶκͲ����Ĳ�ѯ
        /// </summary>
        /// <param name="returnFields">��ѯ�����Ӧ�������ֶ�,*���������ֶ�</param>
        /// <param name="values">��ѯ�����б�KeyValuePair��Key���ֶ���,KeyValuePair��Value���ֶ�ֵ</param>
        /// <returns>�������ݼ�</returns>
        public DataSet GetDataSetByFieldsAndParams(string returnFields, params KeyValuePair<string, object>[] values)
        {
            string cmdText = "select {0} from {1} {2}";
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> kvp in values)
            {
                where = String.IsNullOrEmpty(where) ? "where " + kvp.Key + "=@" + kvp.Key : where + " and " + kvp.Key + "=@" + kvp.Key;
                parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            cmdText = String.Format(cmdText, returnFields, this.classMap.TableName, where);
            DataSet ds = DBHelper.GetDataSet(DBHelper.CONSTR, CommandType.Text, cmdText, parameters.ToArray());
            return ds;
        }
        /// <summary>
        /// ָ�������ֶκͲ����Ĳ�ѯ
        /// </summary>
        /// <param name="returnFields">��ѯ�����Ӧ�������ֶ�,*���������ֶ�</param>
        /// <param name="values">��ѯ�����б�</param>
        /// <returns>�������ݼ�</returns>
        public DataSet GetDataSetByFieldAndParams(string returnFields, Dictionary<string, object> values)
        {
            string cmdText = "select {0} from {1} {2}";
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> kvp in values)
            {
                where = String.IsNullOrEmpty(where) ? "where " + kvp.Key + "=@" + kvp.Key : where + " and " + kvp.Key + "=@" + kvp.Key;
                parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            cmdText = String.Format(cmdText, returnFields, this.classMap.TableName, where);
            DataSet ds = DBHelper.GetDataSet(DBHelper.CONSTR, CommandType.Text, cmdText, parameters.ToArray());
            return ds;
        }
        /// <summary>
        /// ָ�������ֶκ������Ĳ�ѯ
        /// </summary>
        /// <param name="returnFields">��ѯ�����Ӧ�������ֶ�,*���������ֶ�</param>
        /// <param name="where">��ѯ����</param>
        /// <returns>�������ݼ�</returns>
        public DataSet GetDataSetByFieldsAndWhere(string returnFields, string where)
        {
            string cmdText = "select {0} from {1} {2}";
            cmdText = String.Format(cmdText, returnFields, this.classMap.TableName, where);
            DataSet ds = DBHelper.GetDataSet(DBHelper.CONSTR, CommandType.Text, cmdText, null);
            return ds;
        }
        /// <summary>
        /// ���ұ������м�¼
        /// </summary>
        /// <returns>���ض�Ӧ���ʵ����ļ���</returns>
        public List<T> GetAllList()
        {
            string cmdText = String.Format("select * from {0}", this.classMap.TableName);
            List<T> result = this.GetBySql(CommandType.Text, cmdText, null);
            return result;
        }
        /// <summary>
        /// ���ұ��еļ�¼������
        /// </summary>
        /// <param name="order">�����ֶ�</param>
        /// <returns>���ض�Ӧ���ʵ����ļ���</returns>
        public List<T> GetAllListOrder(string order)
        {
            string cmdText = "select * from {0} {1}";
            if (String.IsNullOrEmpty(order))
            {
                cmdText = String.Format(cmdText, this.classMap.TableName, order);
            }
            else
            {
                cmdText = String.Format(cmdText, this.classMap.TableName, "order by " + order);
            }
            List<T> result = this.GetBySql(CommandType.Text, cmdText, null);
            return result;
        }
        /// <summary>
        /// ����ָ�������ǰN����¼
        /// </summary>
        /// <param name="n">���ؽ���е�����¼��</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        public List<T> GetTopNListOrder(int n, string order)
        {
            string cmdText = "select top {0} * from {1} {2}";
            if (String.IsNullOrEmpty(order))
            {
                cmdText = String.Format(cmdText, n, this.classMap.TableName, order);
            }
            else
            {
                cmdText = String.Format(cmdText, n, this.classMap.TableName, "order by " + order);
            }
            List<T> result = this.GetBySql(CommandType.Text, cmdText, null);
            return result;
        }
        /// <summary>
        /// ����ָ�������������ǰN����¼
        /// </summary>
        /// <param name="n">���ؽ���е�����¼��</param>
        /// <param name="where">ɸѡ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        public List<T> GetTopNListWhereOrder(int n, string where, string order)
        {
            string cmdText = "select top {0} * from {1} {2} {3}";
            if (String.IsNullOrEmpty(order))
            {
                cmdText = String.Format(cmdText, n, this.classMap.TableName, where, order);
            }
            else
            {
                cmdText = String.Format(cmdText, n, this.classMap.TableName, where, "order by " + order);
            }
            List<T> result = this.GetBySql(CommandType.Text, cmdText, null);
            return result;
        }
        /// <summary>
        /// ���ұ������м�¼
        /// </summary>
        /// <returns>���ض�Ӧ�����ݼ�</returns>
        public DataSet GetAllDataSet()
        {
            string cmdText = String.Format("select * from {0}", this.classMap.TableName);
            DataSet ds = DBHelper.GetDataSet(DBHelper.CONSTR, CommandType.Text, cmdText, null);
            return ds;
        }
        /// <summary>
        /// ��ҳ��ѯ���������ڷ�ҳ�洢����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        public PageResult<T> GetPageData(PageResult<T> pageResult)
        {
            SqlParameter[] values ={
                                        new SqlParameter("TableName",pageResult.TableName),
                                        new SqlParameter("ReturnFields",pageResult.ReturnFields),
                                        new SqlParameter("PageSize",pageResult.PageSize),
                                        new SqlParameter("PageIndex",pageResult.PageIndex),
                                        new SqlParameter("Where",pageResult.Where),
                                        new SqlParameter("Orderfld",pageResult.Orderfld),
                                        new SqlParameter("OrderType",pageResult.OrderType)
                                   };
            using (SqlDataReader reader = DBHelper.GetReader(DBHelper.CONSTR, CommandType.StoredProcedure, this.procedureName, values))
            {
                List<T> lst = new List<T>();
                Type entityType = typeof(T);
                PropertyInfo[] properties = entityType.GetProperties();
                Dictionary<string, XmlClassMap> dic = EntityMapperHandler.GetInstance().GetMapDictionary();
                XmlClassMap classMap = dic[entityType.Name];
                //��ȡ��ǰҳ������
                while (reader.Read())
                {
                    T entity = (T)entityType.Assembly.CreateInstance(entityType.FullName);
                    foreach (PropertyInfo property in properties)
                    {
                        if (property.CanWrite && classMap.Properties.ContainsKey(property.Name))
                        {
                            object value = reader[classMap.Properties[property.Name].ColumnName];
                            if (value != null && value != DBNull.Value)
                            {
                                property.SetValue(entity, value, null);
                            }
                        }
                    }
                    lst.Add(entity);
                }
                pageResult.Data = lst;
                //��ȡ�ܼ�¼��
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        pageResult.RecordCount = Convert.ToInt32(reader["RecordCount"]);
                        break;
                    }
                }
                reader.Close();
            }
            return pageResult;
        }
        /// <summary>
        /// ��ҳ��ѯ���������ڷ�ҳ�洢����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ���ҳ�����ܼ�¼���Ľ���������ݼ�</returns>
        public DataSet GetPageDataSet(PageResult<T> pageResult)
        {
            SqlParameter[] values ={
                                        new SqlParameter("TableName",pageResult.TableName),
                                        new SqlParameter("ReturnFields",pageResult.ReturnFields),
                                        new SqlParameter("PageSize",pageResult.PageSize),
                                        new SqlParameter("PageIndex",pageResult.PageIndex),
                                        new SqlParameter("Where",pageResult.Where),
                                        new SqlParameter("Orderfld",pageResult.Orderfld),
                                        new SqlParameter("OrderType",pageResult.OrderType)
                                   };
            DataSet ds = DBHelper.GetDataSet(DBHelper.CONSTR, CommandType.StoredProcedure, this.procedureName, values);
            return ds;
        }
        /// <summary>
        /// ִ�д洢���̵ķ���
        /// </summary>
        /// <param name="storeProcedureName">�洢���̵�����</param>
        /// <param name="values">�洢���̵Ĳ���</param>
        /// <returns>���ش洢����ִ�к��Ӧ�����ݼ�</returns>
        public DataSet GetDataSetByStoreProcedure(string storeProcedureName, params KeyValuePair<string, object>[] values)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> kvp in values)
            {
                parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            DataSet ds = DBHelper.GetDataSet(DBHelper.CONSTR, CommandType.StoredProcedure, storeProcedureName, parameters.ToArray());
            return ds;
        }
        /// <summary>
        /// ִ�д洢���̵ķ���
        /// </summary>
        /// <param name="storeProcedureName">�洢���̵�����</param>
        /// <param name="values">�洢���̵Ĳ���</param>
        /// <returns>���ش洢����ִ�к��Ӧ�����ݼ�</returns>
        public DataSet GetDataSetByStoreProcedure(string storeProcedureName, Dictionary<string, object> values)
        {
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> kvp in values)
            {
                parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            DataSet ds = DBHelper.GetDataSet(DBHelper.CONSTR, CommandType.StoredProcedure, storeProcedureName, parameters.ToArray());
            return ds;
        }
        /// <summary>
        /// ��ȡ���е��ܼ�¼��
        /// </summary>
        /// <returns>�����ܼ�¼��</returns>
        public int GetRowCount()
        {
            string cmdText = "select count(*) from {0}";
            cmdText = String.Format(cmdText, this.classMap.TableName);
            int rowCount = Convert.ToInt32(DBHelper.GetScalar(DBHelper.CONSTR, CommandType.Text, cmdText, null));
            return rowCount;
        }
        /// <summary>
        /// ��ȡָ�����������ļ�¼��
        /// </summary>
        /// <param name="values">�����б�</param>
        /// <returns>���ؼ�¼��</returns>
        public int GetRowCountByParams(params KeyValuePair<string, object>[] values)
        {
            string cmdText = "select count(*) from {0} {1}";
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> kvp in values)
            {
                where = String.IsNullOrEmpty(where) ? "where " + kvp.Key + "=@" + kvp.Key : where + " and " + kvp.Key + "=@" + kvp.Key;
                parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            int rowCount = Convert.ToInt32(DBHelper.GetScalar(DBHelper.CONSTR, CommandType.Text, cmdText, parameters.ToArray()));
            return rowCount;
        }
        /// <summary>
        /// ��ȡָ�����������ļ�¼��
        /// </summary>
        /// <param name="values">�����б�</param>
        /// <returns>���ؼ�¼��</returns>
        public int GetRowCountByParams(Dictionary<string, object> values)
        {
            string cmdText = "select count(*) from {0} {1}";
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> kvp in values)
            {
                where = String.IsNullOrEmpty(where) ? "where " + kvp.Key + "=@" + kvp.Key : where + " and " + kvp.Key + "=@" + kvp.Key;
                parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            int rowCount = Convert.ToInt32(DBHelper.GetScalar(DBHelper.CONSTR, CommandType.Text, cmdText, parameters.ToArray()));
            return rowCount;
        }
        /// <summary>
        /// ��ȡִ�������ļ�¼��
        /// </summary>
        /// <param name="where">ָ������</param>
        /// <returns>���ؼ�¼��</returns>
        public int GetRowCountByWhere(string where)
        {
            string cmdText = "select count(*) from {0} {1}";
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            int rowCount = Convert.ToInt32(DBHelper.GetScalar(DBHelper.CONSTR, CommandType.Text, cmdText, null));
            return rowCount;
        }
        /// <summary>
        /// �����׷��һ����¼
        /// </summary>
        /// <param name="entity">��װ��¼��ʵ��</param>
        /// <returns>����������У��򷵻ض�Ӧ�������е�ֵ�����򷵻���Ӱ�������</returns>
        public int Insert(T entity)
        {
            string cmdText = "insert into {0}({1}) values({2});select @@identity";
            string columns = String.Empty;
            string ps = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (XmlPropertyMap pm in this.classMap.Properties.Values)
            {
                //����ֶ��б�Ͳ����б�ȥ���Զ�������
                if (pm != this.classMap.Identity)
                {
                    columns = String.IsNullOrEmpty(columns) ? pm.ColumnName : columns + "," + pm.ColumnName;
                    ps = String.IsNullOrEmpty(ps) ? "@" + pm.ColumnName : ps + ",@" + pm.ColumnName;
                    parameters.Add(new SqlParameter("@" + pm.ColumnName, this.properties[pm.ColumnName].GetValue(entity, null)));
                }
            }
            cmdText = String.Format(cmdText, this.classMap.TableName, columns, ps);
            int result = 0;
            if (this.classMap.Identity == null)
            {
                result = DBHelper.ExecuteCommand(DBHelper.CONSTR, CommandType.Text, cmdText, parameters.ToArray());
            }
            else
            {
                result = Convert.ToInt32(DBHelper.GetScalar(DBHelper.CONSTR,CommandType.Text,cmdText,parameters.ToArray()));
            }
            return result;
        }
        /// <summary>
        /// ���±��е�һ����¼
        /// </summary>
        /// <param name="entity">��װ��¼��ʵ��</param>
        /// <returns>������Ӱ�������</returns>
        public int Update(T entity)
        {
            string cmdText = "update {0} set {1} where {2}";
            string setValues = String.Empty;
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            //��ϸ����ֶκ��ֶ�ֵ����
            foreach (XmlPropertyMap pm in this.classMap.Properties.Values)
            {
                //ȥ���Զ������к������У������в��������
                if (pm != this.classMap.Identity && !this.classMap.Ids.ContainsKey(pm.PropertyName))
                {
                    setValues = String.IsNullOrEmpty(setValues) ? pm.ColumnName + "=@" + pm.ColumnName : setValues + "," + pm.ColumnName + "=@" + pm.ColumnName;
                    parameters.Add(new SqlParameter("@" + pm.ColumnName, this.properties[pm.ColumnName].GetValue(entity, null)));
                }
            }
            //��������ֶκ��ֶβ���ֵ
            if (this.classMap.Identity != null)
            {
                where = this.classMap.Identity.ColumnName + "=@" + this.classMap.Identity.ColumnName;
                parameters.Add(new SqlParameter("@" + this.classMap.Identity.ColumnName, this.properties[this.classMap.Identity.ColumnName].GetValue(entity, null)));
            }
            else if (this.classMap.Ids.Keys.Count > 0)
            {
                foreach (XmlPropertyMap pm in this.classMap.Ids.Values)
                {
                    where = String.IsNullOrEmpty(where) ? pm.ColumnName + "=@" + pm.ColumnName : where + " and " + pm.ColumnName + "=@" + pm.ColumnName;
                    parameters.Add(new SqlParameter("@" + pm.ColumnName, this.properties[pm.ColumnName].GetValue(entity, null)));
                }
            }
            else
            {
                throw new Exception("��û�������У�Ҳû������������û�и��²���ֵ�������и���Update����!");
            }
            cmdText = String.Format(cmdText, this.classMap.TableName, setValues, where);
            int result = DBHelper.ExecuteCommand(DBHelper.CONSTR, CommandType.Text, cmdText, parameters.ToArray());
            return result;
        }
        /// <summary>
        /// �������������ֶ�ֵ
        /// </summary>
        /// <param name="fields">Ҫ���µ��ֶκͶ�Ӧ��ֵ</param>
        /// <param name="where">�����ֶκ�ֵ</param>
        /// <returns>������Ӱ�������</returns>
        public int UpdateFields(string fields, string where)
        {
            string cmdText = "Update {0} set {1} {2}";
            if (String.IsNullOrEmpty(where))
            {
                cmdText = String.Format(cmdText, this.classMap.TableName, fields, where);
            }
            else
            {
                cmdText = String.Format(cmdText, this.classMap.TableName, fields, "where " + where);
            }
            int result = DBHelper.ExecuteCommand(DBHelper.CONSTR, CommandType.Text, cmdText, null);
            return result;
        }
        /// <summary>
        /// �������������ֶ�ֵ
        /// </summary>
        /// <param name="fields">Ҫ���µ��ֶκ�ֵ�ļ���</param>
        /// <param name="where">������������</param>
        /// <returns>������Ӱ�������</returns>
        public int UpdateFields(Dictionary<string, object> fields, Dictionary<string, object> where)
        {
            string cmdText = "Update {0} set {1} {2}";
            string strSet = String.Empty;
            string strWhere = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> kvp in fields)
            {
                strSet = String.IsNullOrEmpty(strSet) ? kvp.Key + "=@" + kvp.Key : strSet + "," + kvp.Key + "=@" + kvp.Key;
                parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            foreach (KeyValuePair<string, object> kvp in where)
            {
                strWhere = String.IsNullOrEmpty(strWhere) ? "Where " + kvp.Key + "=@" + kvp.Key : strWhere + " and " + kvp.Key + "=@" + kvp.Key;
                parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            cmdText = String.Format(cmdText, this.classMap.TableName, strSet, strWhere);
            int result = DBHelper.ExecuteCommand(DBHelper.CONSTR, CommandType.Text, cmdText, parameters.ToArray());
            return result;
        }
        /// <summary>
        /// ɾ�����е�һ����¼
        /// </summary>
        /// <param name="id">Ҫɾ����¼������ֵ�ͱ�ʶֵ</param>
        /// <returns>������Ӱ�������</returns>
        public int Delete(object id)
        {
            string cmdText = "delete from {0} where {1}";
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (this.classMap.Identity != null)
            {
                //��������ֶ�ֵ����һ��(0������)���򰴱�ʶ�е�ֵɾ��
                where = this.classMap.Identity.ColumnName + "=@" + this.classMap.Identity.ColumnName;
                parameters.Add(new SqlParameter("@" + this.classMap.Identity.ColumnName, id));
            }
            else if (this.classMap.Ids.Keys.Count == 1)
            {
                //��������ֶ�ֻ��һ�����������ֶ�ֵɾ��
                foreach (XmlPropertyMap pm in this.classMap.Ids.Values)
                {
                    where = pm.ColumnName + "=@" + pm.ColumnName;
                    parameters.Add(new SqlParameter("@" + pm.ColumnName, id));
                    break;
                }
            }
            else
            {
                //��������ֶ�ֵ����һ��(0������)������û�б�ʶ�У��򰴵�һ���ֶ�ֵɾ��
                foreach (XmlPropertyMap pm in this.classMap.Properties.Values)
                {
                    where = pm.ColumnName + "=@" + pm.ColumnName;
                    parameters.Add(new SqlParameter("@" + pm.ColumnName, id));
                    break;
                }
            }
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            int result = DBHelper.ExecuteCommand(DBHelper.CONSTR, CommandType.Text, cmdText, parameters.ToArray());
            return result;
        }
        /// <summary>
        /// ɾ�����еĶ�����¼
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="ids"></param>
        /// <returns></returns>
        public int DeleteByIds(string columnName, string ids)
        {
            string cmdText = "delete from {0} where {1} in({2})";
            cmdText = String.Format(cmdText, this.classMap.TableName, columnName, ids);
            int result = DBHelper.ExecuteCommand(DBHelper.CONSTR, CommandType.Text, cmdText, null);
            return result;
        }
        /// <summary>
        /// ��ָ���Ĳ���ɾ������
        /// </summary>
        /// <param name="values">����</param>
        /// <returns>����ɾ���ļ�¼��</returns>
        public int DeleteByParam(params KeyValuePair<string, object>[] values)
        {
            string cmdText = "delete from {0} {1}";
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (KeyValuePair<string, object> kvp in values)
            {
                where = String.IsNullOrEmpty(where) ? "where " + kvp.Key + "=@" + kvp.Key : where + " and " + kvp.Key + "=@" + kvp.Key;
                parameters.Add(new SqlParameter("@" + kvp.Key, kvp.Value));
            }
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            int result = DBHelper.ExecuteCommand(DBHelper.CONSTR, CommandType.Text, cmdText, parameters.ToArray());
            return result;
        }
        /// <summary>
        /// ��ָ��������ɾ������
        /// </summary>
        /// <param name="where">����</param>
        /// <returns>����ɾ���ļ�¼��</returns>
        public int DeleteByWhere(string where)
        {
            string cmdText = "delete from {0} {1}";
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            int result = DBHelper.ExecuteCommand(DBHelper.CONSTR, CommandType.Text, cmdText, null);
            return result;
        }
        /// <summary>
        /// ��ձ��е�����
        /// </summary>
        /// <returns></returns>
        public int ClearData()
        {
            string cmdText = "truncate table {0}";
            cmdText = String.Format(cmdText, this.classMap.TableName);
            int result = DBHelper.ExecuteCommand(DBHelper.CONSTR, CommandType.Text, cmdText, null);
            return result;
        }

        #endregion
    }
}
