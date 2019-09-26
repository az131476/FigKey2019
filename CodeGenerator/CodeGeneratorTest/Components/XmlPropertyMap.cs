/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				XmlPropertyMap.cs
 *      Description:
 *				 ����ӳ��ʵ����
 *      Author:
 *				��С��
 *				1297953037@qq.com
 *				http://www.figkey.com
 *      Finish DateTime:
 *				2019��09��26��
 *      History:
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGeneratorTest.Components
{
    /// <summary>
    /// ����ӳ��ʵ����
    /// </summary>
    [Serializable]
    public class XmlPropertyMap
    {
        #region ˽���ֶ�

        private string propertyName;
        private string columnName;

        #endregion

        #region ���췽��

        public XmlPropertyMap() { }

        public XmlPropertyMap(string propertyName, string columnName)
        {
            this.propertyName = propertyName;
            this.columnName = columnName;
        }

        #endregion

        #region ��������

        /// <summary>
        /// ʵ�������������
        /// </summary>
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }
        /// <summary>
        /// ��Ӧ���������
        /// </summary>
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; }
        }

        #endregion
    }
}
