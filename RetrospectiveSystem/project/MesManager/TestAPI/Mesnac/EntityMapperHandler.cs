/************************************************************************************
 *      Copyright (C) 2011 mesnac.com,All Rights Reserved
 *      File:
 *				EntityMapperHandler.cs
 *      Description:
 *				 ʵ����ӳ���ļ�������
 *      Author:
 *				����
 *				zhenglb@mesnac.com
 *				http://www.mesnac.com
 *      Finish DateTime:
 *				2019��09��25��
 *      History:    
 *      
 ***********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;
using System.Reflection;

namespace Mesnac
{
    using Mesnac.Components;
    /// <summary>
    /// ʵ����ӳ���ļ������࣬ʹ�õ������ģʽʵ��
    /// </summary>
    public class EntityMapperHandler
    {
        #region ˽���ֶ�

        private static EntityMapperHandler instance;
        private static Dictionary<string, XmlClassMap> mapperDictionary=new Dictionary<string,XmlClassMap>();

        #endregion

        #region ���췽��

        private EntityMapperHandler() { }

        private EntityMapperHandler(string mapperFiles)
        {
            lock (this)
            {
                string[] files = mapperFiles.Split(new char[] { ',' });
                foreach (string file in files)
                {
                    Stream s = null;
                    if (file.StartsWith("assembly://"))
                    {
                        string prefixStr = "assembly://";
                        string assemblyName = file.Substring(prefixStr.Length, file.LastIndexOf("/") - prefixStr.Length);
                        string fileName = file.Substring(file.LastIndexOf("/") + 1);
                        try
                        {
                            s = Assembly.Load(assemblyName).GetManifestResourceStream(fileName);
                        }
                        catch (Exception ex)
                        {
                            var entityMapperFile = ConfigurationManager.AppSettings["entityMapperFileName"].ToString();
                            var xmlPath = AppDomain.CurrentDomain.BaseDirectory + entityMapperFile;
                            s = new FileStream(xmlPath,FileMode.Open);
                        }
                    }
                    else
                    {
                        s = new FileStream(file, FileMode.Open);
                    }
                    this.CreateMapperDictionary(s);
                }
            }
        }

        private EntityMapperHandler(Stream mapperStream)
        {
            lock (this)
            {
                this.CreateMapperDictionary(mapperStream);
            }
        }

        #endregion

        private void CreateMapperDictionary(Stream mapperStream)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(mapperStream);
            //����ʵ��������ӳ��
            XmlNodeList nl = doc.GetElementsByTagName("class");
            foreach (XmlNode node in nl)
            {
                string className = node.Attributes["name"].Value;
                string tableName = node.Attributes["table"].Value;

                //�����������ֶε�ӳ��
                XmlClassMap classMap = new XmlClassMap(className, tableName);
                XmlNodeList childNl = node.ChildNodes;
                foreach (XmlNode childNode in childNl)
                {
                    if (childNode.Name == "property")
                    {
                        #region ��������

                        string propertyName = childNode.Attributes["name"].Value;
                        string columnName = childNode.Attributes["column"].Value;

                        XmlPropertyMap propertyMap = new XmlPropertyMap(propertyName, columnName);
                        classMap.Properties.Add(propertyName, propertyMap);

                        #endregion
                        #region ����������

                        XmlAttribute attrIdentity = childNode.Attributes["isIdentity"];
                        if (attrIdentity != null && attrIdentity.Value == "true")
                        {
                            classMap.Identity = propertyMap;
                        }

                        #endregion
                        #region ��������

                        XmlAttribute attrPK = childNode.Attributes["isPK"];
                        if (attrPK != null && attrPK.Value == "true")
                        {
                            classMap.Ids.Add(propertyName, propertyMap);
                        }

                        #endregion
                    }
                }
                mapperDictionary.Add(className, classMap);
            }
            mapperStream.Close();
            mapperStream.Dispose();
        }

        /// <summary>
        /// ��ȡ����ʵ���ľ�̬����
        /// </summary>
        /// <param name="mapperFile">ӳ���ļ�·��</param>
        /// <returns>���ر���ʵ��</returns>
        public static EntityMapperHandler GetInstance(string mapperFile)
        {
            if (instance == null)
            {
                instance = new EntityMapperHandler(mapperFile);
            }
            return instance;
        }

        /// <summary>
        /// ��ȡ����ʵ���ľ�̬����
        /// </summary>
        /// <param name="mapperStream">ӳ���ļ���</param>
        /// <returns>���ر���ʵ��</returns>
        public static EntityMapperHandler GetInstance(Stream mapperStream)
        {
            if (instance == null)
            {
                instance = new EntityMapperHandler(mapperStream);
            }
            return instance;
        }
        /// <summary>
        /// ��ȡ����ʵ���ľ�̬������Ĭ�϶�ȡappSetting��entityMapperFile��Ӧ���ļ�,�������ö���Զ��ŷָ�
        /// </summary>
        /// <returns>���ر���ʵ��</returns>
        public static EntityMapperHandler GetInstance()
        {
            if (instance == null)
            {
                string mapperFiles = ConfigurationManager.AppSettings["entityMapperFile"];
                instance = new EntityMapperHandler(mapperFiles);
            }
            return instance;
        }

        /// <summary>
        /// ��ȡӳ���ֵ�
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, XmlClassMap> GetMapDictionary()
        {
            return mapperDictionary;
        }

        /// <summary>
        /// �����������ͻ�ȡ��Ӧ�ı���
        /// </summary>
        /// <param name="type">ʵ������</param>
        /// <returns>���ض�Ӧ�ı�����</returns>
        public string GetTableNameByClassType(Type type)
        {
            return mapperDictionary[type.Name].TableName;
        }
    }
}
