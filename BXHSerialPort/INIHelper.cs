using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace BXHSerialPort
{
    class INIHelper
    {
        [DllImport("kernel32.dll")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filepath);
        [DllImport("kernel32.dll")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder returnvalue, int buffersize, string filepath);
        private string IniFilePath;
        string Section = "Information";
        public INIHelper()
        {
            IniFilePath = Application.StartupPath + "\\Config.ini";
        }
        public INIHelper(string s,string sec)
        {
            IniFilePath = s;
            Section = sec;
        }
        public void write(string[] key, string[] s)
        {
            for (int i = 0; i < key.Length; i++)
            {
                try
                {
                    WritePrivateProfileString(Section, key[i], s[i], IniFilePath);
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        }
        public void read(string[] key, out string[] s)
        {
            s = new string[key.Length];
            for (int i = 0; i < key.Length; i++)
            {
                string outString;
                try
                {
                    GetValue(Section, key[i], out outString);
                    s[i] = outString;
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.Message);
                }
            }
        }



        private void GetValue(string section, string key, out string value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            GetPrivateProfileString(section, key, "", stringBuilder, 1024, IniFilePath);
            value = stringBuilder.ToString();
        }
    }
}
