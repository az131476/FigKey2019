using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXHSerialPort
{
    class StringHelper
    {
        public static int SubstringCount(string str, string substring)
       {  
           if (str.Contains(substring))  
           {  
               string strReplaced = str.Replace(substring, "");  
               return (str.Length - strReplaced.Length) / substring.Length;  
           }  
  
           return 0;  
       } 

    }
}
