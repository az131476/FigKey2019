/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				BaseManager.cs
 *      Description:
 *				 业务逻辑抽象基类
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
using System.Data;

namespace CodeGeneratorTest.Business
{
    using CodeGeneratorTest.Components;
    using CodeGeneratorTest.Data;
    public abstract class BaseManager<T> : IBaseManager<T>
    {
        private IBaseService<T> baseService;

        public IBaseService<T> BaseService
        {
            set { baseService = value; }
        }

        #region 封装数据访问层的常规数据访问方法

        public T GetById(object id)
        {
            return this.baseService.GetById(id);
        }

        public List<T> GetListByParam(params KeyValuePair<string, object>[] values)
        {
            return this.baseService.GetListByParam(values);
        }

        public List<T> GetListByParam(Dictionary<string, object> values)
        {
            return this.baseService.GetListByParam(values);
        }
        
        public List<T> GetListOrderByParam(string order, params KeyValuePair<string, object>[] values)
        {
            return this.baseService.GetListOrderByParam(order, values);
        }

        public List<T> GetListOrderByParam(string order, Dictionary<string, object> values)
        {
            return this.baseService.GetListOrderByParam(order, values);
        }

        public List<T> GetListByWhere(string where)
        {
            return this.baseService.GetListByWhere(where);
        }

        public List<T> GetListByWhereAndOrder(string where, string order)
        {
            return this.baseService.GetListByWhereAndOrder(where, order);
        }

        public DataSet GetDataSetByWhere(string where)
        {
            return this.baseService.GetDataSetByWhere(where);
        }

        public DataSet GetDataSetByFieldsAndParams(string returnFields, params KeyValuePair<string, object>[] values)
        {
            return this.baseService.GetDataSetByFieldsAndParams(returnFields, values);
        }

        public DataSet GetDataSetByFieldAndParams(string returnFields, Dictionary<string, object> values)
        {
            return this.baseService.GetDataSetByFieldAndParams(returnFields, values);
        }

        public DataSet GetDataSetByFieldsAndWhere(string returnFields, string where)
        {
            return this.baseService.GetDataSetByFieldsAndWhere(returnFields, where);
        }

        public List<T> GetAllList()
        {
            return this.baseService.GetAllList();
        }

        public List<T> GetAllListOrder(string order)
        {
            return this.baseService.GetAllListOrder(order);
        }

        public List<T> GetTopNListOrder(int n, string order)
        {
            return this.baseService.GetTopNListOrder(n, order);
        }

        public List<T> GetTopNListWhereOrder(int n, string where, string order)
        {
            return this.baseService.GetTopNListWhereOrder(n, where, order);
        }

        public DataSet GetAllDataSet()
        {
            return this.baseService.GetAllDataSet();
        }

        public PageResult<T> GetPageData(PageResult<T> pageResult)
        {
            return this.baseService.GetPageData(pageResult);
        }

        public DataSet GetPageDataSet(PageResult<T> pageResult)
        {
            return this.baseService.GetPageDataSet(pageResult);
        }
        
        public DataSet GetDataSetByStoreProcedure(string storeProcedureName, params KeyValuePair<string, object>[] values)
        {
            return this.baseService.GetDataSetByStoreProcedure(storeProcedureName, values);
        }

        public DataSet GetDataSetByStoreProcedure(string storeProcedureName, Dictionary<string, object> values)
        {
            return this.baseService.GetDataSetByStoreProcedure(storeProcedureName, values);
        }

        public int GetRowCount()
        {
            return this.baseService.GetRowCount();
        }

        public int GetRowCountByParams(params KeyValuePair<string, object>[] values)
        {
            return this.baseService.GetRowCountByParams(values);
        }

        public int GetRowCountByParams(Dictionary<string, object> values)
        {
            return this.baseService.GetRowCountByParams(values);
        }

        public int GetRowCountByWhere(string where)
        {
            return this.baseService.GetRowCountByWhere(where);
        }

        public int Insert(T entity)
        {
            return this.baseService.Insert(entity);
        }

        public int Update(T entity)
        {
            return this.baseService.Update(entity);
        }

        public int UpdateFields(string fields, string where)
        {
            return this.baseService.UpdateFields(fields, where);
        }

        public int UpdateFields(Dictionary<string, object> fields, Dictionary<string, object> where)
        {
            return this.baseService.UpdateFields(fields, where);
        }

        public int Delete(object id)
        {
            return this.baseService.Delete(id);
        }

        public int DeleteByIds(string columnName, string ids)
        {
            return this.baseService.DeleteByIds(columnName, ids);
        }

        public int DeleteByParam(params KeyValuePair<string, object>[] values)
        {
            return this.baseService.DeleteByParam(values);
        }

        public int DeleteByWhere(string where)
        {
            return this.baseService.DeleteByWhere(where);
        }

        public int ClearData()
        {
            return this.baseService.ClearData();
        }

        #endregion
    }
}
