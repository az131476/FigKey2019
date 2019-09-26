/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				BaseService.cs
 *      Description:
 *				 基于泛型数据访问抽象基类
 *      Author:
 *				唐小东
 *				1297953037@qq.com
 *				http://www.figkey.com
 *      Finish DateTime:
 *				2019年09月26日
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
    /// 基于泛型数据访问抽象基类，封装了基本数据访问操作CRUD
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseService<T> : IBaseService<T>
    {
        #region 私有字段

        private XmlClassMap classMap;   //实体类的映射信息
        private Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();  //实体类的属性信息
        private string procedureName = "MesnacPaging";   //分页存储过程名

        #endregion

        #region 构造方法

        public BaseService()
        {
            //获取实体类T的映射信息
            this.classMap = EntityMapperHandler.GetInstance().GetMapDictionary()[typeof(T).Name];
            //获取实体类的属性信息
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

        #region 受保护方法

        /// <summary>
        /// 封装数据表数据到实体集合的方法
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">要执行的SQL语句或存储过程名称</param>
        /// <param name="values">SQL语句或存储过程的参数列表</param>
        /// <returns>返回实体类的集合</returns>
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

        #region IBaseService<T> 成员
        /// <summary>
        /// 按住键或标识列查找，只有是单字段主键（非组合键）时才按主键查找
        /// </summary>
        /// <param name="id">对应查找记录的主键值或标识值</param>
        /// <returns>返回对应记录的实体信息</returns>
        public T GetById(object id)
        {
            string cmdText = "select * from {0} where {1}";
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (this.classMap.Ids.Keys.Count == 1)
            {
                //如果主键字段是一个字段则按主键字段查询
                foreach (XmlPropertyMap pm in this.classMap.Ids.Values)
                {
                    where = pm.ColumnName + "=@" + pm.ColumnName;
                    parameters.Add(new SqlParameter("@" + pm.ColumnName, id));
                    break;
                }
            }
            else if (this.classMap.Identity != null)
            {
                //如果没有主键，或者主键字段不止一个，则按标识列查询
                where = this.classMap.Identity.ColumnName + "=@" + this.classMap.Identity.ColumnName;
                parameters.Add(new SqlParameter("@" + this.classMap.Identity.ColumnName, id));
            }
            else
            {
                //如果没有标识列，或者没有主键，或者主键字段不只一个则按表中的第一个字段查询
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
        /// 指定参数的查询
        /// </summary>
        /// <param name="values">查询参数列表，KeyValuePair的Key是字段名,KeyValuePair的Value是字段值</param>
        /// <returns>返回实体类的集合</returns>
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
        /// 指定参数的查询
        /// </summary>
        /// <param name="values">查询参数列表</param>
        /// <returns>返回实体类的集合</returns>
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
        /// 指定参数查询并排序
        /// </summary>
        /// <param name="order">排序条件</param>
        /// <param name="values">查询参数列表</param>
        /// <returns>返回实体类的集合</returns>
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
        /// 指定参数查询并排序
        /// </summary>
        /// <param name="order">排序条件</param>
        /// <param name="values">查询参数列表</param>
        /// <returns>返回实体类的集合</returns>
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
        /// 指定条件的查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>返回实体类的集合</returns>
        public List<T> GetListByWhere(string where)
        {
            string cmdText = "select * from {0} {1}";
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            List<T> lst = this.GetBySql(CommandType.Text, cmdText, null);
            return lst;
        }
        /// <summary>
        /// 指定条件和排序的查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <param name="order">排序字段</param>
        /// <returns>返回实体类的集合</returns>
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
        /// 指定条件的查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns>返回数据集</returns>
        public DataSet GetDataSetByWhere(string where)
        {
            string cmdText = "select * from {0} {1}";
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            DataSet ds = DBHelper.GetDataSet(DBHelper.CONSTR, CommandType.Text, cmdText, null);
            return ds;
        }
        /// <summary>
        /// 指定返回字段和参数的查询
        /// </summary>
        /// <param name="returnFields">查询结果中应包含的字段,*代表所有字段</param>
        /// <param name="values">查询参数列表，KeyValuePair的Key是字段名,KeyValuePair的Value是字段值</param>
        /// <returns>返回数据集</returns>
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
        /// 指定返回字段和参数的查询
        /// </summary>
        /// <param name="returnFields">查询结果中应包含的字段,*代表所有字段</param>
        /// <param name="values">查询参数列表</param>
        /// <returns>返回数据集</returns>
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
        /// 指定返回字段和条件的查询
        /// </summary>
        /// <param name="returnFields">查询结果中应包含的字段,*代表所有字段</param>
        /// <param name="where">查询条件</param>
        /// <returns>返回数据集</returns>
        public DataSet GetDataSetByFieldsAndWhere(string returnFields, string where)
        {
            string cmdText = "select {0} from {1} {2}";
            cmdText = String.Format(cmdText, returnFields, this.classMap.TableName, where);
            DataSet ds = DBHelper.GetDataSet(DBHelper.CONSTR, CommandType.Text, cmdText, null);
            return ds;
        }
        /// <summary>
        /// 查找表中所有记录
        /// </summary>
        /// <returns>返回对应表的实体类的集合</returns>
        public List<T> GetAllList()
        {
            string cmdText = String.Format("select * from {0}", this.classMap.TableName);
            List<T> result = this.GetBySql(CommandType.Text, cmdText, null);
            return result;
        }
        /// <summary>
        /// 查找表中的记录并排序
        /// </summary>
        /// <param name="order">排序字段</param>
        /// <returns>返回对应表的实体类的集合</returns>
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
        /// 返回指定排序的前N条记录
        /// </summary>
        /// <param name="n">返回结果中的最大记录数</param>
        /// <param name="order">排序字段</param>
        /// <returns>返回实体类的集合</returns>
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
        /// 返回指定条件和排序的前N条记录
        /// </summary>
        /// <param name="n">返回结果中的最大记录数</param>
        /// <param name="where">筛选条件</param>
        /// <param name="order">排序字段</param>
        /// <returns>返回实体类的集合</returns>
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
        /// 查找表中所有记录
        /// </summary>
        /// <returns>返回对应的数据集</returns>
        public DataSet GetAllDataSet()
        {
            string cmdText = String.Format("select * from {0}", this.classMap.TableName);
            DataSet ds = DBHelper.GetDataSet(DBHelper.CONSTR, CommandType.Text, cmdText, null);
            return ds;
        }
        /// <summary>
        /// 分页查询方法，基于分页存储过程
        /// </summary>
        /// <param name="pageResult">用于传递查询条件的分页类的对象</param>
        /// <returns>返回封装了页面数据和总记录数据的分页类对象</returns>
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
                //提取当前页的数据
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
                //提取总记录数
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
        /// 分页查询方法，基于分页存储过程
        /// </summary>
        /// <param name="pageResult">用于传递查询条件的分页类的对象</param>
        /// <returns>返回封装了页面数据和总页数、总记录数的结果集的数据集</returns>
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
        /// 执行存储过程的方法
        /// </summary>
        /// <param name="storeProcedureName">存储过程的名称</param>
        /// <param name="values">存储过程的参数</param>
        /// <returns>返回存储过程执行后对应的数据集</returns>
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
        /// 执行存储过程的方法
        /// </summary>
        /// <param name="storeProcedureName">存储过程的名称</param>
        /// <param name="values">存储过程的参数</param>
        /// <returns>返回存储过程执行后对应的数据集</returns>
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
        /// 获取表中的总记录数
        /// </summary>
        /// <returns>返回总记录数</returns>
        public int GetRowCount()
        {
            string cmdText = "select count(*) from {0}";
            cmdText = String.Format(cmdText, this.classMap.TableName);
            int rowCount = Convert.ToInt32(DBHelper.GetScalar(DBHelper.CONSTR, CommandType.Text, cmdText, null));
            return rowCount;
        }
        /// <summary>
        /// 获取指定参数条件的记录数
        /// </summary>
        /// <param name="values">参数列表</param>
        /// <returns>返回记录数</returns>
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
        /// 获取指定参数条件的记录数
        /// </summary>
        /// <param name="values">参数列表</param>
        /// <returns>返回记录数</returns>
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
        /// 获取执行条件的记录数
        /// </summary>
        /// <param name="where">指定条件</param>
        /// <returns>返回记录数</returns>
        public int GetRowCountByWhere(string where)
        {
            string cmdText = "select count(*) from {0} {1}";
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            int rowCount = Convert.ToInt32(DBHelper.GetScalar(DBHelper.CONSTR, CommandType.Text, cmdText, null));
            return rowCount;
        }
        /// <summary>
        /// 向表中追加一条记录
        /// </summary>
        /// <param name="entity">封装记录的实体</param>
        /// <returns>如果有自增列，则返回对应的自增列的值，否则返回受影响的行数</returns>
        public int Insert(T entity)
        {
            string cmdText = "insert into {0}({1}) values({2});select @@identity";
            string columns = String.Empty;
            string ps = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            foreach (XmlPropertyMap pm in this.classMap.Properties.Values)
            {
                //组合字段列表和参数列表，去掉自动增长列
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
        /// 更新表中的一条记录
        /// </summary>
        /// <param name="entity">封装记录的实体</param>
        /// <returns>返回受影响的行数</returns>
        public int Update(T entity)
        {
            string cmdText = "update {0} set {1} where {2}";
            string setValues = String.Empty;
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            //组合更新字段和字段值参数
            foreach (XmlPropertyMap pm in this.classMap.Properties.Values)
            {
                //去除自动增长列和主键列，主键列不允许更新
                if (pm != this.classMap.Identity && !this.classMap.Ids.ContainsKey(pm.PropertyName))
                {
                    setValues = String.IsNullOrEmpty(setValues) ? pm.ColumnName + "=@" + pm.ColumnName : setValues + "," + pm.ColumnName + "=@" + pm.ColumnName;
                    parameters.Add(new SqlParameter("@" + pm.ColumnName, this.properties[pm.ColumnName].GetValue(entity, null)));
                }
            }
            //组合条件字段和字段参数值
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
                throw new Exception("表没有自增列，也没有主键，导致没有更新参照值，请自行覆盖Update方法!");
            }
            cmdText = String.Format(cmdText, this.classMap.TableName, setValues, where);
            int result = DBHelper.ExecuteCommand(DBHelper.CONSTR, CommandType.Text, cmdText, parameters.ToArray());
            return result;
        }
        /// <summary>
        /// 按照条件更新字段值
        /// </summary>
        /// <param name="fields">要更新的字段和对应的值</param>
        /// <param name="where">条件字段和值</param>
        /// <returns>返回受影响的行数</returns>
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
        /// 按照条件更新字段值
        /// </summary>
        /// <param name="fields">要更新的字段和值的集合</param>
        /// <param name="where">条件参数集合</param>
        /// <returns>返回受影响的行数</returns>
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
        /// 删除表中的一条记录
        /// </summary>
        /// <param name="id">要删除记录的主键值和标识值</param>
        /// <returns>返回受影响的行数</returns>
        public int Delete(object id)
        {
            string cmdText = "delete from {0} where {1}";
            string where = String.Empty;
            List<SqlParameter> parameters = new List<SqlParameter>();
            if (this.classMap.Identity != null)
            {
                //如果主键字段值不是一个(0个或多个)，则按标识列的值删除
                where = this.classMap.Identity.ColumnName + "=@" + this.classMap.Identity.ColumnName;
                parameters.Add(new SqlParameter("@" + this.classMap.Identity.ColumnName, id));
            }
            else if (this.classMap.Ids.Keys.Count == 1)
            {
                //如果主键字段只有一个，则按主键字段值删除
                foreach (XmlPropertyMap pm in this.classMap.Ids.Values)
                {
                    where = pm.ColumnName + "=@" + pm.ColumnName;
                    parameters.Add(new SqlParameter("@" + pm.ColumnName, id));
                    break;
                }
            }
            else
            {
                //如果主键字段值不是一个(0个或多个)，并且没有标识列，则按第一个字段值删除
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
        /// 删除表中的多条记录
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
        /// 按指定的参数删除数据
        /// </summary>
        /// <param name="values">参数</param>
        /// <returns>返回删除的记录数</returns>
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
        /// 按指定的条件删除数据
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns>返回删除的记录数</returns>
        public int DeleteByWhere(string where)
        {
            string cmdText = "delete from {0} {1}";
            cmdText = String.Format(cmdText, this.classMap.TableName, where);
            int result = DBHelper.ExecuteCommand(DBHelper.CONSTR, CommandType.Text, cmdText, null);
            return result;
        }
        /// <summary>
        /// 清空表中的数据
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
