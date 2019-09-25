using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace MesnacCoding
{
    /// <summary>
    /// ���ɴ��빤����
    /// </summary>
    public class GenerateCodeUtil
    {
        #region ��������

        #region Pascal������
        /// <summary>
        /// ת��ΪPascal������
        /// </summary>
        /// <param name="s">Ҫת�����ַ���</param>
        /// <returns>����ת������ַ���</returns>
        public static string ToPascal(string s)
        {
            return s.Substring(0, 1).ToUpper() + s.Substring(1);
        }
        #endregion

        #region ����������
        /// <summary>
        /// ת��Ϊ����������
        /// </summary>
        /// <param name="s">Ҫת�����ַ���</param>
        /// <returns>����ת������ַ���</returns>
        public static string ToCamel(string s)
        {
            return s.Substring(0, 1).ToLower() + s.Substring(1);
        }
        #endregion

        #region ��ȡʵ���������
        /// <summary>
        /// ���ݱ�����ȡʵ���������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <returns>ʵ���������</returns>
        public static string GetEntityClassName(string tableName)
        {
            string s = tableName;
            if (s.EndsWith("s"))
            {
                s = s.Substring(0, s.Length - 1);
            }
            return ToPascal(s);
        }
        #endregion

        #region ��ȡʵ�����ļ���
        /// <summary>
        /// ���ݱ�����ȡʵ�����ļ�������
        /// </summary>
        /// <param name="tableName">����</param>
        /// <returns>ʵ�����ļ���</returns>
        public static string GetEntityFileName(string tableName)
        {
            return GetEntityClassName(tableName) + ".cs";
        }
        #endregion

        #region ��ȡCSharp��������
        /// <summary>
        /// �����е����ͻ�ȡCSharp����������
        /// </summary>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static string GetCSharpVariableType(Type columnType)
        {
            switch (columnType.Name)
            {
                case "AnsiString": return "string";
                case "AnsiStringFixedLength": return "string";
                case "Binary": return "byte[]";
                case "Boolean": return "bool";
                case "Byte": return "byte";
                case "Currency": return "decimal";
                case "Date": return "DateTime";
                case "DateTime": return "DateTime";
                case "Decimal": return "decimal";
                case "Double": return "double";
                case "Guid": return "Guid";
                case "Int16": return "short";
                case "Int32": return "int";
                case "Int64": return "long";
                case "Object": return "object";
                case "SByte": return "sbyte";
                case "Single": return "float";
                case "String": return "string";
                case "StringFixedLength": return "string";
                case "Time": return "TimeSpan";
                case "UInt16": return "ushort";
                case "UInt32": return "uint";
                case "UInt64": return "ulong";
                case "VarNumeric": return "decimal";
                default:
                    {
                        return "__UNKNOWN__Type";
                    }
            }
        }
        #endregion

        #region �����е����ͻ�ȡĬ��ֵ
        /// <summary>
        /// �����е����ͻ�ȡĬ��ֵ
        /// </summary>
        /// <param name="columnType"></param>
        /// <returns></returns>
        public static string GetDefaultValue(string columnType)
        {
            switch (columnType)
            {
                case "string": return "String.Empty";
                case "byte[]": return "{ 0 }";
                case "bool": return "false";
                case "byte": return "0";
                case "decimal": return "0";
                case "DateTime": return "DateTime.Now";
                case "Double": return "0.0";
                case "Guid": return "Guid.Empty";
                case "short": return "0";
                case "int": return "0";
                case "long": return "0";
                case "object": return "null";
                case "sbyte": return "0";
                case "float": return "0";
                case "TimeSpan": return "TimeSpan.Zero";
                case "ushort": return "0";
                case "uint": return "0";
                case "ulong": return "0";
                default:
                    {
                        return "null";
                    }
            }
        }
        #endregion

        #region ��ȡ�����û���
        /// <summary>
        /// ��ȡ���ݿ��������û���������б�
        /// </summary>
        /// <returns>�������ݿ��������</returns>
        public static List<string> GetTableNames()
        {
            //DBHelper.ConnectionString = String.Format("Server={0};Database={1};uid={2};pwd={3}", GlobalConfig.Item.Server, GlobalConfig.Item.DataBase, GlobalConfig.Item.UID, GlobalConfig.Item.PWD);
            List<string> tables = new List<string>();
            string sql = String.Empty;
            if (GlobalConfig.Item.IsAllowView)
            {
                sql = "select name from sysobjects where xtype in('U','V') order by crdate";
            }
            else
            {
                sql = "select name from sysobjects where xtype='U' order by crdate";
            }
            using (SqlDataReader reader = DBHelper.GetReader(DBHelper.ConnectionString, CommandType.Text, sql, null))
            {
                while (reader.Read())
                {
                    tables.Add(reader["name"] as string);
                }
                reader.Close();
            }
            return tables;
        }
        #endregion

        #region ��ȡ����ֶκ��ֶ����ͼ���
        /// <summary>
        /// ���ݱ�����ȡ����ֶκ��ֶ����͵ļ���
        /// </summary>
        /// <param name="tableName">����</param>
        /// <returns>�ֶκ��ֶ����͵ļ���</returns>
        public static Dictionary<string, string> GetColumnAndCType(string tableName)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string sql = String.Format("select * from {0} where 1<>1", tableName);
            DataSet ds = DBHelper.GetDataSet(DBHelper.ConnectionString, CommandType.Text, sql, null);
            foreach (DataColumn col in ds.Tables[0].Columns)
            {
                dic.Add(col.ColumnName, GetCSharpVariableType(col.DataType));
            }
            return dic;
        }
        #endregion

        #region ��ȡһ����������ֶ��б�
        /// <summary>
        /// ��ȡһ����������ֶ��б�
        /// </summary>
        /// <param name="tableName">����</param>
        /// <returns>���������ֶ��б�</returns>
        public static List<string> GetPrimaryKeys(string tableName)
        {
            List<string> lst = new List<string>();
            string sql = "sp_help";
            SqlParameter[] values ={
                new SqlParameter("@objname",tableName)
            };
            DataSet ds = DBHelper.GetDataSet(DBHelper.ConnectionString, CommandType.StoredProcedure, sql, values);
            if (ds.Tables.Count >= 7)
            {
                foreach (DataRow row in ds.Tables[6].Rows)
                {
                    //if (row["constraint_type"].ToString() == "PRIMARY KEY (clustered)")
                    //2011-9-9�����������˶ԷǾۼ������Ľ���֧��
                    if (row["constraint_type"].ToString().StartsWith("PRIMARY KEY"))
                    {
                        string[] keys = row["constraint_keys"].ToString().Split(new char[] { ',' });
                        foreach (string key in keys)
                        {
                            lst.Add(key.Trim());
                        }
                        break;
                    }
                }
            }
            return lst;
        }
        #endregion

        #region ��ȡһ����������ֶ��������û�������ֶ��򷵻�No identity column defined.
        /// <summary>
        /// ��ȡһ����������ֶ��������û�������ֶ��򷵻�No identity column defined.
        /// </summary>
        /// <param name="tableName">����</param>
        /// <returns>���������ֶ���</returns>
        public static string GetIdentityColumn(string tableName)
        {
            string sql = "sp_help";
            SqlParameter[] values ={
                new SqlParameter("@objname",tableName)
            };
            DataSet ds = DBHelper.GetDataSet(DBHelper.ConnectionString, CommandType.StoredProcedure, sql, values);
            string columnName = ds.Tables[2].Rows[0]["Identity"].ToString();
            return columnName;
        }
        #endregion

        #region ͨ��ģ���������ļ�
        /// <summary>
        /// ͨ��ģ���������ļ�
        /// </summary>
        /// <param name="templateFile">ģ���ļ�����������չ��</param>
        /// <param name="nameSpace">Ҫ���ɵ���������ռ�</param>
        /// <param name="csharpFile">��ѡ��CSharp�ļ���</param>
        private static void GenerateClassFromTemplateFile(string templateFile, string nameSpace, string csharpFile,string entityClassName)
        {
            //��ȡʵ����ģ���ļ�������
            Assembly assembly = typeof(GenerateCodeUtil).Assembly;
            Stream s = assembly.GetManifestResourceStream(assembly.GetName().Name + ".template." + templateFile + ".txt");
            StreamReader sr = new StreamReader(s, System.Text.Encoding.Default);
            StringBuilder sb = new StringBuilder();
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                sb.AppendLine(line);
            }
            sr.Close();
            s.Close();

            sb = sb.Replace("${CopyRight}", GlobalConfig.Item.CopyRight);
            sb = sb.Replace("${Author}", GlobalConfig.Item.Author);
            sb = sb.Replace("${AuthorEmail}", GlobalConfig.Item.AuthorEmail);
            sb = sb.Replace("${Online}", GlobalConfig.Item.Online);
            sb = sb.Replace("${Date}", String.Format("{0:yyyy��MM��dd��}", DateTime.Now));
            sb = sb.Replace("${EntityNameSpace}", GlobalConfig.Item.EntityNameSpace);
            sb = sb.Replace("${ComponentNameSpace}", GlobalConfig.Item.ComponentNameSpace);
            sb = sb.Replace("${TopComponentsNameSpace}", GlobalConfig.Item.TopComponentsNameSpace);

            sb = sb.Replace("${DAOClassPostFix}", GlobalConfig.Item.DAOClassPostFix);
            sb = sb.Replace("${CamelDAOClassPostFix}", GlobalConfig.Item.CamelDAOClassPostFix);
            sb = sb.Replace("${DAONameSpace}", GlobalConfig.Item.DAONameSpace);
            sb = sb.Replace("${BIZNameSpace}", GlobalConfig.Item.BIZNameSpace);
            sb = sb.Replace("${BIZClassPostFix}", GlobalConfig.Item.BIZClassPostFix);

            if (!String.IsNullOrEmpty(entityClassName)) sb = sb.Replace("${EntityClassName}", entityClassName);

            string path = GlobalConfig.Item.OutputPath + "/" + nameSpace.Replace(".", "/");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = path + "/" + templateFile + ".cs";
            if (!String.IsNullOrEmpty(csharpFile)) fileName = path + "/" + csharpFile + ".cs";
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.Default);
            sw.Write(sb.ToString());
            sw.Close();
        }
        #endregion

        #endregion

        #region ����ʵ����
        /// <summary>
        /// ����ʵ����
        /// </summary>
        /// <returns></returns>
        public static int GenerateEntityClass()
        {
            //��ȡʵ����ģ���ļ�������
            Assembly assembly = typeof(GenerateCodeUtil).Assembly;
            Stream s = assembly.GetManifestResourceStream(assembly.GetName().Name + ".template.EntityClassTemplate.txt");
            StreamReader sr = new StreamReader(s, System.Text.Encoding.Default);
            StringBuilder sb1 = new StringBuilder();
            string line = String.Empty;
            while ((line = sr.ReadLine()) != null)
            {
                sb1.AppendLine(line);
            }
            sr.Close();
            s.Close();
            List<string> tables = GetTableNames();  //��ȡ���ݿ������е����ݱ�
            foreach (string tableName in tables)
            {
                StringBuilder sb = new StringBuilder(sb1.ToString());
                sb = sb.Replace("${CopyRight}", GlobalConfig.Item.CopyRight);
                sb = sb.Replace("${EntityClassName}", GetEntityClassName(tableName));
                sb = sb.Replace("${Author}", GlobalConfig.Item.Author);
                sb = sb.Replace("${AuthorEmail}", GlobalConfig.Item.AuthorEmail);
                sb = sb.Replace("${Online}", GlobalConfig.Item.Online);
                sb = sb.Replace("${Date}", String.Format("{0:yyyy��MM��dd��}", DateTime.Now));
                sb = sb.Replace("${EntityNameSpace}", GlobalConfig.Item.EntityNameSpace);
                
                Dictionary<string, string> fields = GetColumnAndCType(tableName);
                StringBuilder sbPrivateField = new StringBuilder();
                StringBuilder sbPublicAttribute = new StringBuilder();
                foreach (KeyValuePair<string, string> kvp in fields)
                {
                    sbPrivateField.AppendLine(String.Format("        private {0} _{1} = {2};", kvp.Value, ToCamel(kvp.Key), GetDefaultValue(kvp.Value)));
                    sbPublicAttribute.AppendLine();
                    sbPublicAttribute.AppendLine(String.Format("        public {0} {1}", kvp.Value, ToPascal(kvp.Key)));
                    sbPublicAttribute.AppendLine("        {");
                    sbPublicAttribute.AppendLine("            set { this._" + ToCamel(kvp.Key) + " = value; }");
                    sbPublicAttribute.AppendLine("            get { return this._" + ToCamel(kvp.Key) + "; }");
                    sbPublicAttribute.AppendLine("        }");
                    sbPublicAttribute.AppendLine();
                }
                sb = sb.Replace("${PrivateFields}", sbPrivateField.ToString());
                sb = sb.Replace("${PublicAttribute}", sbPublicAttribute.ToString());

                string entityPath = GlobalConfig.Item.OutputPath + "/" + GlobalConfig.Item.EntityNameSpace.Replace(".", "/");
                if (!Directory.Exists(entityPath))
                {
                    Directory.CreateDirectory(entityPath);
                }
                string fileName = entityPath + "/" + GetEntityClassName(tableName) + ".cs";
                StreamWriter sw = new StreamWriter(fileName, false, Encoding.Default);
                sw.Write(sb.ToString());
                sw.Close();
            }
            return tables.Count;
            
        }

        #endregion

        #region ����ʵ����ӳ���ļ�
        /// <summary>
        /// ����ʵ����ӳ���ļ�
        /// </summary>
        public static int GenerateEntityMapperFile()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sb.AppendLine("<entityMapper>");
            List<string> tables = GetTableNames();  //��ȡ���ݿ������е����ݱ�
            foreach (string tableName in tables)
            {
                sb.AppendLine(String.Format("\t<class name=\"{0}\" table=\"{1}\">", GetEntityClassName(tableName), tableName));
                Dictionary<string, string> fields = GetColumnAndCType(tableName);
                List<string> lstPK = GetPrimaryKeys(tableName);
                string identityColumn = GetIdentityColumn(tableName);
                foreach (KeyValuePair<string, string> kvp in fields)
                {
                    if (kvp.Key == identityColumn)
                    {
                        if (lstPK.Contains(kvp.Key))
                        {
                            sb.AppendLine(String.Format("\t\t<property name=\"{0}\" column=\"{1}\" isPK=\"true\" isIdentity=\"true\" />", ToPascal(kvp.Key), kvp.Key));
                        }
                        else
                        {
                            sb.AppendLine(String.Format("\t\t<property name=\"{0}\" column=\"{1}\" isIdentity=\"true\" />", ToPascal(kvp.Key), kvp.Key));
                        }
                    }
                    else
                    {
                        if (lstPK.Contains(kvp.Key))
                        {
                            sb.AppendLine(String.Format("\t\t<property name=\"{0}\" column=\"{1}\" isPK=\"true\" />", ToPascal(kvp.Key), kvp.Key));
                        }
                        else
                        {
                            sb.AppendLine(String.Format("\t\t<property name=\"{0}\" column=\"{1}\" />", ToPascal(kvp.Key), kvp.Key));
                        }
                    }
                }
                sb.AppendLine("\t</class>");
            }
            sb.AppendLine("</entityMapper>");

            string entityMapperFilePath = GlobalConfig.Item.OutputPath + "/" + GlobalConfig.Item.EntityNameSpace.Replace(".", "/");
            if (!Directory.Exists(entityMapperFilePath))
            {
                Directory.CreateDirectory(entityMapperFilePath);
            }
            string fileName = entityMapperFilePath + "/" + GlobalConfig.Item.EntityMapperFile;
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.Default);
            sw.Write(sb.ToString());
            sw.Close();

            return 1;
        }

        #endregion

        #region ���ɳ������
        /// <summary>
        /// ���ɳ������
        /// </summary>
        public static int GenerateComponents()
        {
            int cnt = 0;

            #region ����XmlClassMap.cs

            GenerateClassFromTemplateFile("XmlClassMap", GlobalConfig.Item.ComponentNameSpace, String.Empty, String.Empty);

            #endregion

            #region ����XmlPropertyMap.cs

            GenerateClassFromTemplateFile("XmlPropertyMap", GlobalConfig.Item.ComponentNameSpace, String.Empty, String.Empty);

            #endregion

            #region ����PageResult.cs

            GenerateClassFromTemplateFile("PageResult", GlobalConfig.Item.ComponentNameSpace, String.Empty, String.Empty);

            #endregion

            #region ����EntityMapperHandler.cs

            GenerateClassFromTemplateFile("EntityMapperHandler", GlobalConfig.Item.TopComponentsNameSpace, String.Empty, String.Empty);

            #endregion

            cnt = 4;
            return cnt;
        }

        #endregion

        #region �������ݷ��ʲ����

        public static int GenerateDaoLayer()
        {
            int cnt = 0;

            #region ����DBHelper��

            GenerateClassFromTemplateFile("DBHelper", GlobalConfig.Item.DAONameSpace, String.Empty, String.Empty);
            cnt++;

            #endregion

            #region �������ݷ��ʻ����ӿ�

            GenerateClassFromTemplateFile("IBaseService", GlobalConfig.Item.DAONameSpace, String.Empty, String.Empty);
            cnt++;

            #endregion

            #region �������ݷ��ʳ������

            GenerateClassFromTemplateFile("BaseService", GlobalConfig.Item.DAONameSpace, String.Empty, String.Empty);
            cnt++;

            #endregion

            #region �������ݷ��ʽӿں����ݷ���ʵ����

            List<string> tables = GetTableNames();
            foreach (string tableName in tables)
            {
                GenerateClassFromTemplateFile("IDAOTemplate", GlobalConfig.Item.DAONameSpace + ".Interface", "I" + GetEntityClassName(tableName) + GlobalConfig.Item.DAOClassPostFix, GetEntityClassName(tableName));
                GenerateClassFromTemplateFile("DAOTemplate", GlobalConfig.Item.DAONameSpace + ".Implements", GetEntityClassName(tableName) + GlobalConfig.Item.DAOClassPostFix, GetEntityClassName(tableName));
            }
            cnt += tables.Count * 2;
            #endregion

            return cnt;
        }

        #endregion

        #region ����ҵ���߼������

        public static int GenerateBusinessLayer()
        {
            int cnt = 0;

            #region ����ҵ������ӿ�

            GenerateClassFromTemplateFile("IBaseManager", GlobalConfig.Item.BIZNameSpace, String.Empty, String.Empty);
            cnt++;

            #endregion

            #region ����ҵ��������

            GenerateClassFromTemplateFile("BaseManager", GlobalConfig.Item.BIZNameSpace, String.Empty, String.Empty);
            cnt++;

            #endregion

            #region ����ҵ��ӿں�ҵ��ʵ����

            List<string> tables = GetTableNames();
            foreach (string tableName in tables)
            {
                GenerateClassFromTemplateFile("IBIZTemplate", GlobalConfig.Item.BIZNameSpace + ".Interface", "I" + GetEntityClassName(tableName) + GlobalConfig.Item.BIZClassPostFix, GetEntityClassName(tableName));
                GenerateClassFromTemplateFile("BIZTemplate", GlobalConfig.Item.BIZNameSpace + ".Implements", GetEntityClassName(tableName) + GlobalConfig.Item.BIZClassPostFix, GetEntityClassName(tableName));
            }

            cnt += tables.Count * 2;

            #endregion

            return cnt;
        }

        #endregion

        #region ����App.configʾ��
        /// <summary>
        /// ����App.config�ļ�
        /// </summary>
        /// <returns>�����ļ�������</returns>
        public static int GenerateAppConfig()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sb.AppendLine("<configuration>");
            sb.AppendLine("\t<connectionStrings>");
            sb.AppendLine(String.Format("\t\t<add name=\"CONSTR\" connectionString=\"Server={0};Database={1};uid={2};pwd={3}\"/>", GlobalConfig.Item.Server, GlobalConfig.Item.DataBase, GlobalConfig.Item.UID, GlobalConfig.Item.PWD));
            sb.AppendLine("\t</connectionStrings>");
            sb.AppendLine("\t<appSettings>");
            sb.AppendLine("\t\t<add key=\"entityMapperFile\" value=\"assembly://Mesnac/Mesnac.Entity.EntityMapper.xml\"/>");
            sb.AppendLine("\t</appSettings>");
            sb.AppendLine("</configuration>");

            if (!Directory.Exists(GlobalConfig.Item.OutputPath))
            {
                Directory.CreateDirectory(GlobalConfig.Item.OutputPath);
            }
            string fileName = GlobalConfig.Item.OutputPath + "/App.config";
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.Default);
            sw.Write(sb.ToString());
            sw.Close();

            return 1;
        }

        #endregion

        #region ����Web.configʾ��
        /// <summary>
        /// ����Web.config�ļ�
        /// </summary>
        /// <returns>�����ļ�������</returns>
        public static int GenerateWebConfig()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<configuration>");
            sb.AppendLine("\t<connectionStrings>");
            sb.AppendLine(String.Format("\t\t<add name=\"CONSTR\" connectionString=\"Server={0};Database={1};uid={2};pwd={3}\"/>", GlobalConfig.Item.Server, GlobalConfig.Item.DataBase, GlobalConfig.Item.UID, GlobalConfig.Item.PWD));
            sb.AppendLine("\t</connectionStrings>");
            sb.AppendLine("\t<appSettings>");
            sb.AppendLine("\t\t<add key=\"entityMapperFile\" value=\"assembly://Mesnac/Mesnac.Entity.EntityMapper.xml\"/>");
            sb.AppendLine("\t</appSettings>");
            sb.AppendLine("\t<system.web>");
            sb.AppendLine("\t\t<compilation debug=\"false\" targetFramework=\"4.0\" />");
            sb.AppendLine("\t</system.web>");
            sb.AppendLine("</configuration>");

            if (!Directory.Exists(GlobalConfig.Item.OutputPath))
            {
                Directory.CreateDirectory(GlobalConfig.Item.OutputPath);
            }
            string fileName = GlobalConfig.Item.OutputPath + "/Web.config";
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.Default);
            sw.Write(sb.ToString());
            sw.Close();

            return 1;
        }

        #endregion
    }
}
