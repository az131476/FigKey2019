using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XcpDll
{
    public class ReadClass
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string name;

        /// <summary>
        /// 长度
        /// </summary>
        public UInt32 length;

        /// <summary>
        /// 地址
        /// </summary>
        public UInt32 address;

        /// <summary>
        /// 表达
        /// </summary>
        public string expression;

        /// <summary>
        /// mask
        /// </summary>
        public UInt32 mask;

        /// <summary>
        /// 类型
        /// </summary>
        public string type;

        /// <summary>
        /// 方法类型
        /// </summary>
        public string metholdType;

        /// <summary>
        /// 键值
        /// </summary>
        public Dictionary<UInt32, string> tableDictionary;

        /// <summary>
        /// 存储参数
        /// </summary>
        public List<float> parameter;
    }
}
