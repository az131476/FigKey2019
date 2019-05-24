using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BXHSerialPort
{
    class CurrRangeCtl
    {
        public string[] read()
        {
            string[] Setting = new string[24];
            try
            {
                INIHelper ini = new INIHelper(@"./CurrStandard.ini", "Demo");
                List<string> keyList = new List<string>();
                for (int i = 1; i < 7; i++)
                {
                    string s1 = "Item" + i + "_HeadH";
                    string s2 = "Item" + i + "_HeadL";
                    string s3 = "Item" + i + "_TailH";
                    string s4 = "Item" + i + "_TailL";
                    keyList.Add(s1);
                    keyList.Add(s2);
                    keyList.Add(s3);
                    keyList.Add(s4);
                }
                string[] Key = keyList.ToArray();
                ini.read(Key, out Setting);
            }
            catch (Exception)
            {
                MessageBox.Show("读取曲线范围出错", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            return Setting;
        }
        public void write(string[] setting)
        {
            try
            {
                INIHelper ini = new INIHelper(@"./CurrStandard.ini", "Demo");
                List<string> keyList = new List<string>();
                for (int i = 1; i < 7; i++)
                {
                    string s1 = "Item" + i + "_HeadH";
                    string s2 = "Item" + i + "_HeadL";
                    string s3 = "Item" + i + "_TailH";
                    string s4 = "Item" + i + "_TailL";
                    keyList.Add(s1);
                    keyList.Add(s2);
                    keyList.Add(s3);
                    keyList.Add(s4);
                }
                string[] Key = keyList.ToArray();
                ini.write(Key, setting);
            }
            catch (Exception)
            {
                MessageBox.Show("写曲线范围出错", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        }
}
