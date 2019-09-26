/************************************************************************************
 *      Copyright (C) 2019 FigKey,All Rights Reserved
 *      File:
 *				EntityMapperHandler.cs
 *      Description:
 *				 实体类映射文件解析类
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
using System.Xml;
using System.IO;
using System.Configuration;
using System.Reflection;

namespace CodeGeneratorTest
{
    using CodeGeneratorTest.Components;
    /// <summary>
    /// 实体类映射文件解析类，使用单例设计模式实现
    /// </summary>
    public class EntityMapperHandler
    {
        #region 私有字段

        private static EntityMapperHandler instance;
        private static Dictionary<string, XmlClassMap> mapperDictionary=new Dictionary<string,XmlClassMap>();

        #endregion

        #region 构造方法

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
                        s = Assembly.Load(assemblyName).GetManifestResourceStream(fileName);
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
            //解析实体类与表的映射
            XmlNodeList nl = doc.GetElementsByTagName("class");
            foreach (XmlNode node in nl)
            {
                string className = node.Attributes["name"].Value;
                string tableName = node.Attributes["table"].Value;

                //解析属性与字段的映射
                XmlClassMap classMap = new XmlClassMap(className, tableName);
                XmlNodeList childNl = node.ChildNodes;
                foreach (XmlNode childNode in childNl)
                {
                    if (childNode.Name == "property")
                    {
                        #region 解析属性

                        string propertyName = childNode.Attributes["name"].Value;
                        string columnName = childNode.Attributes["column"].Value;

                        XmlPropertyMap propertyMap = new XmlPropertyMap(propertyName, columnName);
                        classMap.Properties.Add(propertyName, propertyMap);

                        #endregion
                        #region 解析自增列

                        XmlAttribute attrIdentity = childNode.Attributes["isIdentity"];
                        if (attrIdentity != null && attrIdentity.Value == "true")
                        {
                            classMap.Identity = propertyMap;
                        }

                        #endregion
                        #region 解析主键

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
        /// 获取本类实例的静态方法
        /// </summary>
        /// <param name="mapperFile">映射文件路径</param>
        /// <returns>返回本类实例</returns>
        public static EntityMapperHandler GetInstance(string mapperFile)
        {
            if (instance == null)
            {
                instance = new EntityMapperHandler(mapperFile);
            }
            return instance;
        }

        /// <summary>
        /// 获取本类实例的静态方法
        /// </summary>
        /// <param name="mapperStream">映射文件流</param>
        /// <returns>返回本类实例</returns>
        public static EntityMapperHandler GetInstance(Stream mapperStream)
        {
            if (instance == null)
            {
                instance = new EntityMapperHandler(mapperStream);
            }
            return instance;
        }
        /// <summary>
        /// 获取本类实例的静态方法，默认读取appSetting中entityMapperFile对应的文件,可以配置多个以逗号分隔
        /// </summary>
        /// <returns>返回本类实例</returns>
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
        /// 获取映射字典
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, XmlClassMap> GetMapDictionary()
        {
            return mapperDictionary;
        }

        /// <summary>
        /// 根据试题类型获取对应的表名
        /// </summary>
        /// <param name="type">实体类型</param>
        /// <returns>返回对应的表名称</returns>
        public string GetTableNameByClassType(Type type)
        {
            return mapperDictionary[type.Name].TableName;
        }
    }
}
