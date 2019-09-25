using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace MesnacCoding
{
    /// <summary>
    /// �����ļ�����������
    /// </summary>
    public class ConfigFileUtil<T>
    {
        /// <summary>
        /// �Ѷ������л���ָ�����ļ���
        /// </summary>
        /// <param name="obj">Ҫ���л��Ķ���</param>
        /// <param name="file">Ҫ�������л����ݵ��ļ�</param>
        public static void SaveToFile(T obj, string file)
        {
            BinaryFormatter fmt = new BinaryFormatter();
            FileStream fs = new FileStream(file, FileMode.CreateNew);
            fmt.Serialize(fs, obj);
            fs.Close();
        }
        /// <summary>
        /// ��ָ�����ļ��з����л�����
        /// </summary>
        /// <param name="file">Ҫ�Ӵ��ļ��з����л�����</param>
        /// <returns>���ط����л��Ķ���</returns>
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
