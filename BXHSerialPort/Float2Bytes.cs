using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BXHSerialPort
{
    class Float2Bytes
    {
        public static string[] split(string s)
        {
            string[] s1 = s.Split('\t');
            List<string> list = new List<string>();
            list.AddRange(s1);
            for (int i = list.Count-1; i >= 0; i--)
            {
                if (list[i] == "")
                {
                    list.RemoveAt(i);
                }
            }
            return list.ToArray();
        }
        static public string[][] readFile(string filename)
        {
            string[] lines = File.ReadAllLines(@filename);
            string[][] cells = new string[lines.Length][];
            for (int i = 0; i < lines.Length; i++)
            {
                cells[i] = split(lines[i]);
            }
            return cells;
        }
        public static List<List<byte>> makeBytes(string filename)
        {
            string[][] s = Float2Bytes.readFile(@filename);
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i].Length!=8)
                {
                    MessageBox.Show("EXCEL读取有误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
            List<List<byte>> bytes = new List<List<byte>>();
            for (int i = 0; i < 6; i++)
            {
                List<byte> bInit = new List<byte>();
                bytes.Add(bInit);
            }
            List<List<UInt16>> u16ArrayList = new List<List<UInt16>>();
            for (int i = 0; i < 6; i++)
            {
                List<UInt16> bInit = new List<UInt16>();
                u16ArrayList.Add(bInit);
            }
            byte[] reserve = { 0, 0,0,0 };
            byte[] checksumTag = {  0, 0 ,0,0};
            int listIndex = 0;
            for (int i = 0; i < s.Length; i++)
            {
                for (int j = 0; j < s[i].Length; j++)
                {
                    string temp = s[i][j].Trim().ToUpper();
                    if (temp == "CHECKSUM")
                    {
                        if (i!=0)
                        {
                            listIndex++;
                        }
                        bytes[listIndex].AddRange(checksumTag);
                    }
                    else if (temp == "RESERVE")
                    {
                        bytes[listIndex].AddRange(reserve);
                    }
                    else
                    {
                        byte[] floatBytes = BitConverter.GetBytes(Convert.ToSingle(temp));
                        bytes[listIndex].AddRange(floatBytes.Reverse());
                        UInt16 u16 = (UInt16)((floatBytes[1] << 8) + floatBytes[0]);
                        u16ArrayList[listIndex].Add(u16);
                        u16 = (UInt16)((floatBytes[3] << 8) + floatBytes[2]);
                        u16ArrayList[listIndex].Add(u16);
                    }
                }
            }
            for(int i = 0; i < u16ArrayList.Count; i++)
            {
                UInt16 checkSum = 0;
                foreach (var item in u16ArrayList[i])
                {
                    checkSum += item;
                }
                //bytes[i].RemoveAt(0);
                //bytes[i].RemoveAt(0);
                byte[] u16Bytes = BitConverter.GetBytes(checkSum);
                bytes[i][0]=u16Bytes[1];
                bytes[i][1] = u16Bytes[0];
                //bytes[i].Insert(0, u16Bytes[1]);
            }
            return bytes;
        }
        public static List<List<byte>> makeBytesSubROM(string filename)
        {
            string[][] s = Float2Bytes.readFile(@filename);
            List<List<byte>> bytes = new List<List<byte>>();
            for (int i = 0; i < 2; i++)
            {
                List<byte> bInit = new List<byte>();
                bytes.Add(bInit);
            }
            byte[] reserve = { 0, 0, 0, 0 };
            for (int i = 0; i < s.Length; i++)
            {
                for (int j = 0; j < s[i].Length; j++)
                {
                    string temp = s[i][j].Trim().ToUpper();
                    if (temp == "RESERVE")
                    {
                        bytes[i].AddRange(reserve);
                    }
                    else
                    {
                        if (s[i][j].Substring(0,2)=="0x")
                        {
                            UInt32 u32 = Convert.ToUInt32(s[i][j].Substring(2), 16);
                            bytes[i].AddRange(BitConverter.GetBytes(u32).Reverse());
                        }
                        else
                        {
                            byte[] floatBytes = BitConverter.GetBytes(Convert.ToSingle(temp));
                            bytes[i].AddRange(floatBytes.Reverse());
                        }
                    }
                }
            }
            return bytes;
        }
        public static string makeString(string filename)
        {
            string[][] s = Float2Bytes.readFile(@filename);
            string ss = "";
            for (int i = 0; i < s.Length; i++)
            {
                for (int j = 0; j < s[i].Length; j++)
                {
                    if (s[i][j].Trim().ToUpper() == "CHECKSUM"
                        || s[i][j].Trim().ToUpper() == "RESERVE")
                    {
                        ss += 0 + " ";
                    }
                    else
                    {
                        ss += s[i][j] + " ";
                    }
                }
                ss += "\r\n";
            }
            return ss;
        }
    }
}
