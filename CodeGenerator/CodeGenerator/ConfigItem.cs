using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGenerator
{
    /// <summary>
    /// 应用配置项
    /// </summary>
    [Serializable]
    public class ConfigItem
    {
        private string _Server = "(local)";
        private string _UID = "sa";
        private string _PWD = "az13132323251..";
        private string _DataBase = "master";

        private string _CopyRight = String.Format("Copyright (C) {0} FigKey,All Rights Reserved", DateTime.Now.Year);
        private string _Author = "唐小东";
        private string _AuthorEmail = "1297953037@qq.com";
        private string _Online = "http://www.figkey.com";

        private string _EntityNameSpace = "FigKey.Entity";
        private string _EntityMapperFile = "EntityMapper.xml";
        private string _ComponentNameSpace = "FigKey.Components";
        private string _DAONameSpace = "FigKey.Data";
        private string _BIZNameSpace = "FigKey.Business";
        private string _DAOClassPostFix = "Service";
        private string _BIZClassPostFix = "Manager";
        private string _OutputPath = "C:\\aaa";
        private int _AssemblyInfo = 2;

        private string _TopComponentsNameSpace;
        private bool _IsAllowView = false;

        public string Server
        {
            get { return _Server; }
            set { _Server = value; }
        }
        public string UID
        {
            get { return _UID; }
            set { _UID = value; }
        }
        public string PWD
        {
            get { return _PWD; }
            set { _PWD = value; }
        }
        public string DataBase
        {
            get { return _DataBase; }
            set { _DataBase = value; }
        }
        public string CopyRight
        {
            get { return _CopyRight; }
            set { _CopyRight = value; }
        }
        public string Author
        {
            get { return _Author; }
            set { _Author = value; }
        }
        public string AuthorEmail
        {
            get { return _AuthorEmail; }
            set { _AuthorEmail = value; }
        }
        public string Online
        {
            get { return _Online; }
            set { _Online = value; }
        }
        public string EntityNameSpace
        {
            get { return _EntityNameSpace; }
            set { _EntityNameSpace = value; }
        }
        public string EntityMapperFile
        {
            get { return _EntityMapperFile; }
            set { _EntityMapperFile = value; }
        }
        public string ComponentNameSpace
        {
            get { return _ComponentNameSpace; }
            set { _ComponentNameSpace = value; }
        }
        public string DAONameSpace
        {
            get { return _DAONameSpace; }
            set { _DAONameSpace = value; }
        }
        public string BIZNameSpace
        {
            get { return _BIZNameSpace; }
            set { _BIZNameSpace = value; }
        }
        public string DAOClassPostFix
        {
            get { return _DAOClassPostFix; }
            set { _DAOClassPostFix = value; }
        }
        public string CamelDAOClassPostFix
        {
            get { return GenerateCodeUtil.ToCamel(this._DAOClassPostFix); }
        }
        public string BIZClassPostFix
        {
            get { return _BIZClassPostFix; }
            set { _BIZClassPostFix = value; }
        }
        public string OutputPath
        {
            get { return _OutputPath; }
            set { _OutputPath = value; }
        }
        public int AssemblyInfo
        {
            get { return _AssemblyInfo; }
            set { _AssemblyInfo = value; }
        }
        public string TopComponentsNameSpace
        {
            get { return this._ComponentNameSpace.Split(new char[] { '.' })[0]; }
            set { _TopComponentsNameSpace = value; }
        }

        public bool IsAllowView
        {
            get { return _IsAllowView; }
            set { _IsAllowView = value; }
        }

        public string DAOAssembly
        {
            get
            {
                return this._DAONameSpace.Split(new char[] { '.' })[0];
            }
        }

        public string BIZAssembly
        {
            get
            {
                return this._BIZNameSpace.Split(new char[] { '.' })[0];
            }
        }
        
    }
}
