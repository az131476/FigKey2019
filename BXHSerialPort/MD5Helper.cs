using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BXHSerialPort
{
    class MD5Helper
    {
        public static string GetMD5(string myString)
          {
              MD5 md5 = new MD5CryptoServiceProvider();
              byte[] fromData = System.Text.Encoding.Unicode.GetBytes(myString);
              byte[] targetData = md5.ComputeHash(fromData);
              string byte2String = null;
  
              for (int i = 0; i<targetData.Length; i++)
              {
                 byte2String += targetData[i].ToString("x");
              }
 
             return byte2String;
         }
    }
}
