using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXHSerialPort
{
    class IPTxtFileRxWx
    {
        public static string[] readIP()
        {
            try
            {
                string[] ipSetting = File.ReadAllText(@".\IP.txt").Split('+');
                return ipSetting;
            }
            catch
            {
                return null;
            }
        }
        public static void writeIP(string ip,string point)
        {
            File.WriteAllText(@".\IP.txt", ip + "+" + point);
        }
    }
}
