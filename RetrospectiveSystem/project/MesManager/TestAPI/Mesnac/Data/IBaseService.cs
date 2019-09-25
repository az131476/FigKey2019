/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				IBaseService.cs
 *      Description:
 *				 ���ݷ��ʻ����ӿ�
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
using System.Data;

namespace Mesnac.Data
{
    using Mesnac.Components;
    public interface IBaseService<T>
    {
        /// <summary>
        /// ������������
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T GetById(object id);
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="values">��ѯ�����б�KeyValuePair��Key���ֶ���,KeyValuePair��Value���ֶ�ֵ</param>
        /// <returns>����ʵ����ļ���</returns>
        List<T> GetListByParam(params KeyValuePair<string, object>[] values);
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="values">��ѯ�����б�</param>
        /// <returns>����ʵ����ļ���</returns>
        List<T> GetListByParam(Dictionary<string, object> values);
        /// <summary>
        /// ָ��������ѯ������
        /// </summary>
        /// <param name="order">��������</param>
        /// <param name="values">��ѯ�����б�</param>
        /// <returns>����ʵ����ļ���</returns>
        List<T> GetListOrderByParam(string order, params KeyValuePair<string, object>[] values);
        /// <summary>
        /// ָ��������ѯ������
        /// </summary>
        /// <param name="order">��������</param>
        /// <param name="values">��ѯ�����б�</param>
        /// <returns>����ʵ����ļ���</returns>
        List<T> GetListOrderByParam(string order, Dictionary<string, object> values);
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>����ʵ����ļ���</returns>
        List<T> GetListByWhere(string where);
        /// <summary>
        /// ָ������������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        List<T> GetListByWhereAndOrder(string where, string order);
        /// <summary>
        /// ָ�������Ĳ�ѯ
        /// </summary>
        /// <param name="where">��ѯ����</param>
        /// <returns>�������ݼ�</returns>
        DataSet GetDataSetByWhere(string where);
        /// <summary>
        /// ָ�������ֶκͲ����Ĳ�ѯ
        /// </summary>
        /// <param name="returnFields">��ѯ�����Ӧ�������ֶ�,*���������ֶ�</param>
        /// <param name="values"></param>
        /// <returns></returns>
        DataSet GetDataSetByFieldsAndParams(string returnFields, params KeyValuePair<string, object>[] values);
        /// <summary>
        /// ָ�������ֶκͲ����Ĳ�ѯ
        /// </summary>
        /// <param name="returnFields">��ѯ�����Ӧ�������ֶ�,*���������ֶ�</param>
        /// <param name="values">��ѯ�����б�</param>
        /// <returns>�������ݼ�</returns>
        DataSet GetDataSetByFieldAndParams(string returnFields, Dictionary<string, object> values);
        /// <summary>
        /// ָ�������ֶκ������Ĳ�ѯ
        /// </summary>
        /// <param name="returnFields">��ѯ�����Ӧ�������ֶ�,*���������ֶ�</param>
        /// <param name="where">��ѯ����</param>
        /// <returns>�������ݼ�</returns>
        DataSet GetDataSetByFieldsAndWhere(string returnFields, string where);
        /// <summary>
        /// ��ѯ���м�¼����List��ʽ����
        /// </summary>
        /// <returns></returns>
        List<T> GetAllList();
        /// <summary>
        /// ���ұ��еļ�¼������
        /// </summary>
        /// <param name="order">�����ֶ�</param>
        /// <returns>���ض�Ӧ���ʵ����ļ���</returns>
        List<T> GetAllListOrder(string order);
        /// <summary>
        /// ����ָ�������ǰN����¼
        /// </summary>
        /// <param name="n">���ؽ���еļ�¼��</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        List<T> GetTopNListOrder(int n, string order);
        /// <summary>
        /// ����ָ�������������ǰN����¼
        /// </summary>
        /// <param name="n">���ؽ���е�����¼��</param>
        /// <param name="where">ɸѡ����</param>
        /// <param name="order">�����ֶ�</param>
        /// <returns>����ʵ����ļ���</returns>
        List<T> GetTopNListWhereOrder(int n, string where, string order);
        /// <summary>
        /// ��ѯ���м�¼����DataSet��ʽ��������
        /// </summary>
        /// <returns></returns>
        DataSet GetAllDataSet();
        /// <summary>
        /// ��ҳ��ѯ����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ��ܼ�¼���ݵķ�ҳ�����</returns>
        PageResult<T> GetPageData(PageResult<T> pageResult);
        /// <summary>
        /// ��ҳ��ѯ���������ڷ�ҳ�洢����
        /// </summary>
        /// <param name="pageResult">���ڴ��ݲ�ѯ�����ķ�ҳ��Ķ���</param>
        /// <returns>���ط�װ��ҳ�����ݺ���ҳ�����ܼ�¼���Ľ���������ݼ�</returns>
        DataSet GetPageDataSet(PageResult<T> pageResult);
        /// <summary>
        /// ִ�д洢���̵ķ���
        /// </summary>
        /// <param name="storeProcedureName">�洢���̵�����</param>
        /// <param name="values">�洢���̵Ĳ���</param>
        /// <returns>���ش洢����ִ�к��Ӧ�����ݼ�</returns>
        DataSet GetDataSetByStoreProcedure(string storeProcedureName, params KeyValuePair<string, object>[] values);
        /// <summary>
        /// ִ�д洢���̵ķ���
        /// </summary>
        /// <param name="storeProcedureName">�洢���̵�����</param>
        /// <param name="values">�洢���̵Ĳ���</param>
        /// <returns>���ش洢����ִ�к��Ӧ�����ݼ�</returns>
        DataSet GetDataSetByStoreProcedure(string storeProcedureName, Dictionary<string, object> values);
        /// <summary>
        /// ��ȡ���е��ܼ�¼��
        /// </summary>
        /// <returns>�����ܼ�¼��</returns>
        int GetRowCount();
        /// <summary>
        /// ��ȡָ�����������ļ�¼��
        /// </summary>
        /// <param name="values">�����б�</param>
        /// <returns>���ؼ�¼��</returns>
        int GetRowCountByParams(params KeyValuePair<string, object>[] values);
        /// <summary>
        /// ��ȡָ�����������ļ�¼��
        /// </summary>
        /// <param name="values">�����б�</param>
        /// <returns>���ؼ�¼��</returns>
        int GetRowCountByParams(Dictionary<string, object> values);
        /// <summary>
        /// ��ȡִ�������ļ�¼��
        /// </summary>
        /// <param name="where">ָ������</param>
        /// <returns>���ؼ�¼��</returns>
        int GetRowCountByWhere(string where);
        /// <summary>
        /// ����¼�¼
        /// </summary>
        /// <param name="entity">��Ӧ�¼�¼��ʵ������</param>
        /// <returns>����׷�Ӽ�¼������ֵ</returns>
        int Insert(T entity);
        /// <summary>
        /// ���¼�¼
        /// </summary>
        /// <param name="entity">��Ҫ���¼�¼��Ӧ��ʵ������</param>
        /// <returns>���ظ��µļ�¼��</returns>
        int Update(T entity);
        /// <summary>
        /// �������������ֶ�ֵ
        /// </summary>
        /// <param name="fields">Ҫ���µ��ֶκͶ�Ӧ��ֵ</param>
        /// <param name="where">�����ֶκ�ֵ</param>
        /// <returns>������Ӱ�������</returns>
        int UpdateFields(string fields, string where);
        /// <summary>
        /// �������������ֶ�ֵ
        /// </summary>
        /// <param name="fields">Ҫ���µ��ֶκ�ֵ�ļ���</param>
        /// <param name="where">������������</param>
        /// <returns>������Ӱ�������</returns>
        int UpdateFields(Dictionary<string, object> fields, Dictionary<string, object> where);
        /// <summary>
        /// ɾ��������idֵ�ü�¼
        /// </summary>
        /// <param name="id">Ҫɾ����¼������ֵ</param>
        /// <returns>����ɾ���ļ�¼����</returns>
        int Delete(object id);
        /// <summary>
        /// ɾ��ָ���������б������
        /// </summary>
        /// <param name="ids">�����б�1,2,4</param>
        /// <returns>����ɾ���ļ�¼����</returns>
        int DeleteByIds(string columnName, string ids);
        /// <summary>
        /// ��ָ���Ĳ���ɾ������
        /// </summary>
        /// <param name="values">����</param>
        /// <returns>����ɾ���ļ�¼��</returns>
        int DeleteByParam(params KeyValuePair<string, object>[] values);
        /// <summary>
        /// ��ָ��������ɾ������
        /// </summary>
        /// <param name="where">����</param>
        /// <returns>����ɾ���ļ�¼��</returns>
        int DeleteByWhere(string where);
        /// <summary>
        /// ��ձ��е�����
        /// </summary>
        /// <returns>��������ļ�¼����</returns>
        int ClearData();
    }
}
