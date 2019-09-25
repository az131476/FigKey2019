/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				PageResult.cs
 *      Description:
 *				 ��ҳ��
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

namespace Mesnac.Components
{
    /// <summary>
    /// ��ҳ���װ��ҳ��Ϣ
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class PageResult<T>
    {
        #region ˽���ֶ�

        private string tableName = String.Empty;  //����
        private string returnFields = "*";          //�����ֶ��б�
        private string where = String.Empty;        //��������
        private int pageIndex = 1;                  //��ǰҳ������
        private int pageSize = 10;                  //ÿҳ��¼��
        private int recordCount = 0;                //�ܼ�¼��
        private string orderfld = "id";             //�����ֶ�
        private int orderType = 1;                  //����ʽ,1Ϊ��������Ϊ����
        private List<T> data = new List<T>();

        #endregion

        #region ���췽��

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

        #region ��������

        /// <summary>
        /// ����
        /// </summary>
        public string TableName
        {
            get { return tableName; }
            set { tableName = value; }
        }
        /// <summary>
        /// ���ؽ���������ֶ��б��м��ö��ŷָ���Ĭ��Ϊ*
        /// </summary>
        public string ReturnFields
        {
            get { return returnFields; }
            set { returnFields = value; }
        }
        /// <summary>
        /// ��ҳ������������Ĭ��Ϊ���ַ���
        /// </summary>
        public string Where
        {
            get { return where; }
            set { where = value; }
        }
        /// <summary>
        /// ��ǰҳ��������Ĭ��Ϊ1,�����һҳ
        /// </summary>
        public int PageIndex
        {
            get { return pageIndex; }
            set { pageIndex = value; }
        }
        /// <summary>
        /// ÿҳ�ļ�¼����Ĭ��10
        /// </summary>
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value; }
        }
        /// <summary>
        /// ������Χ���ܼ�¼��
        /// </summary>
        public int RecordCount
        {
            get { return recordCount; }
            set { recordCount = value; }
        }
        /// <summary>
        /// ��ҳ��
        /// </summary>
        public int PageCount
        {
            get
            {
                return this.recordCount % this.PageSize == 0 ? this.recordCount / this.pageSize : this.recordCount / this.PageSize + 1;
            }
        }
        /// <summary>
        /// ��ҳ��ѯʱ�������ֶΣ����������
        /// </summary>
        public string Orderfld
        {
            get { return orderfld; }
            set { orderfld = value; }
        }
        /// <summary>
        /// �������ͣ�����ʽ,1Ϊ��������Ϊ����
        /// </summary>
        public int OrderType
        {
            get { return orderType; }
            set { orderType = value; }
        }
        /// <summary>
        /// ��Ӧ��ǰҳ������
        /// </summary>
        public List<T> Data
        {
            get { return data; }
            set { data = value; }
        }

        #endregion
    }
}
