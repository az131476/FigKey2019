/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				PageResult.cs
 *      Description:
 *				 分页类
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

namespace CodeGeneratorTest.Components
{
    /// <summary>
    /// 分页类封装分页信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PageResult<T>
    {
        #region 私有字段

        private string tableName = String.Empty;  //表名
        private string returnFields = "*";          //返回字段列表
        private string where = String.Empty;        //检索条件
        private int pageIndex = 1;                  //当前页的索引
        private int pageSize = 10;                  //每页记录数
        private int recordCount = 0;                //总记录数
        private string orderfld = "id";             //排序字段
        private int orderType = 1;                  //排序方式,1为降序，其他为升序
        private List<T> data = new List<T>();

        #endregion

        #region 构造方法

        public PageResult()
        {
            //this.tableName = EntityMapperHandler.GetInstance().GetTableNameByClassType(typeof(T));
            XmlClassMap classMap=EntityMapperHandler.GetInstance().GetMapDictionary()[typeof(T).Name];
            this.tableName=classMap.TableName;
            if (classMap.Identity != null)
            {
                this.orderfld = classMap.Identity.ColumnName;
            }
            else if (classMap.Ids.Keys.Count > 0)
            {
                foreach (XmlPropertyMap pm in classMap.Ids.Values)
                {
                    this.orderfld = pm.ColumnName;
                    break;
                }
            }
        }

        public PageResult(string tableName)
        {
            this.tableName = tableName;
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
        /// <summary>
        /// 返回结果包括的字段列表，中间用逗号分隔，默认为*
        /// </summary>
        public string ReturnFields
        {
            get { return returnFields; }
            set { returnFields = value; }
        }
        /// <summary>
        /// 分页检索的条件，默认为空字符串
        /// </summary>
        public string Where
        {
            get { return where; }
            set { where = value; }
        }
        /// <summary>
        /// 当前页的索引，默认为1,代表第一页
        /// </summary>
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }
        /// <summary>
        /// 每页的记录数，默认10
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        /// <summary>
        /// 检索范围的总记录数
        /// </summary>
        public int RecordCount
        {
            get { return recordCount; }
            set { recordCount = value; }
        }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get
            {
                return this.recordCount % this.PageSize == 0 ? this.recordCount / this.pageSize : this.recordCount / this.PageSize + 1;
            }
        }
        /// <summary>
        /// 分页查询时的排序字段，最好是主键
        /// </summary>
        public string Orderfld
        {
            get { return orderfld; }
            set { orderfld = value; }
        }
        /// <summary>
        /// 排序类型，排序方式,1为降序，其他为升序
        /// </summary>
        public int OrderType
        {
            get { return orderType; }
            set { orderType = value; }
        }
        /// <summary>
        /// 对应当前页的数据
        /// </summary>
        public List<T> Data
        {
            get { return data; }
            set { data = value; }
        }

        #endregion
    }
}
