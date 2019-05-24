using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BXHSerialPort
{
    class CSVHelper
    {
        public static List<List<float>> OpenCSV(string filePath,string[] tag)
        {
            List<List<float>> list = new List<List<float>>();
            int lineCount = tag.Length;
            for (int i = 0; i < lineCount; i++)
            {
                List<float> l = new List<float>();
                list.Add(l);
            }
            FileStream fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string strLine;
            while ((strLine=sr.ReadLine())!=null)
            {
                //MessageBox.Show(strLine, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                for (int i = 0; i < lineCount; i++)
                {
                    if (strLine.Contains(tag[i]))
                    {
                        try
                        {
                            string[] strLineArr = strLine.Split(',');
                            if (strLineArr.Length<7)
                            {
                                string errMessage = "原始数据列数应大于7,每列以逗号隔开";
                                throw new Exception(errMessage);
                            }
                            string valueString = strLineArr[6];
                            float value = Convert.ToSingle(valueString);
                            list[i].Add(value);
                            break;
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }
                }
            }
            sr.Close();
            fs.Close();
            return list;
        }
    }
}
