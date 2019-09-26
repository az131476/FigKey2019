using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CodeGenerator
{
    /// <summary>
    /// 配置文件操作处理类
    /// </summary>
    public class ConfigFileUtil<T>
    {
        /// <summary>
        /// 把对象序列化到指定的文件中
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <param name="file">要保存序列化数据的文件</param>
        public static void SaveToFile(T obj, string file)
        {
            BinaryFormatter fmt = new BinaryFormatter();
            FileStream fs = new FileStream(file, FileMode.CreateNew);
            fmt.Serialize(fs, obj);
            fs.Close();
        }
        /// <summary>
        /// 从指定的文件中反序列化对象
        /// </summary>
        /// <param name="file">要从此文件中反序列化数据</param>
        /// <returns>返回反序列化的对象</returns>
        public static T GetFromFile(string file)
        {
            BinaryFormatter fmt = new BinaryFormatter();
            FileStream fs = new FileStream(file, FileMode.Open);
            T obj = (T)fmt.Deserialize(fs);
            fs.Close();
            return obj;
        }
    }
}
