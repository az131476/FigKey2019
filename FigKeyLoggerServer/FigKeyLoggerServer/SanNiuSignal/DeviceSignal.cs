using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtils.Logger;

namespace FigKeyLoggerServer.SanNiuSignal
{
    public class DeviceSignal
    {
        public const int TYPE_OFFSET = 8;
        public const int iHeadLength = 12;
        public const int iDefaultSign = 0x55AAAA55;
        public const string strFlagBit = "55AAAA55";

        public static bool isHeadData(byte[] buffer, int iParseStartIndex)
        {
            if (buffer.Length < iParseStartIndex + 12)
            {
                return false;
            }

            return BitConverter.ToString(buffer, iParseStartIndex, 4).Trim().Replace("-", "").Equals(strFlagBit);
        }

        public static int getDataLength(byte[] buffer, int iStartIndex)
        {
            return BitConverter.ToInt32(buffer, iStartIndex + 8) + iHeadLength;
        }

        /// <summary>
        /// head
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="iStartIndex"></param>
        /// <returns></returns>
        public static int haveHead(byte[] buffer, int iStartIndex)
        {
            for (int nI = iStartIndex; nI + 3 < buffer.Length; nI++)
            {
                if ((buffer[nI] == 0x55) && (buffer[nI + 1] == 0xAA) && (buffer[nI + 2] == 0xAA) && (buffer[nI + 3] == 0x55))
                {
                    return nI;
                }
            }
            if (iStartIndex + 3 == buffer.Length)
            {
                if ((buffer[iStartIndex] == 0x55) && (buffer[iStartIndex + 1] == 0xAA) && (buffer[iStartIndex + 2] == 0xAA))
                {
                    return iStartIndex;
                }
            }
            else if (iStartIndex + 2 == buffer.Length)
            {
                if ((buffer[iStartIndex] == 0x55) && (buffer[iStartIndex + 1] == 0xAA))
                {
                    return iStartIndex;
                }
            }
            else if (iStartIndex + 1 == buffer.Length)
            {
                if (buffer[iStartIndex] == 0x55)
                {
                    return iStartIndex;
                }
            }
            return -1;
        }

        public static int GetType(byte[] buffer)
        {
            return BitConverter.ToInt32(buffer, 4);
        }

        public static int GetLength(byte[] buffer)
        {
            return BitConverter.ToInt32(buffer, 8);
        }

        public static string GetDevID(byte[] buffer)
        {
            return Encoding.ASCII.GetString(buffer, 12, 16).TrimEnd('\0');
        }

        public static string parseSignal(byte[] buffer)
        {
            string result = BitConverter.ToString(buffer, 0, 11).Trim().Replace("-", "") + "|";
            string type = result.Substring(TYPE_OFFSET, 2);
            if (type.Equals("01"))
            {
                ///仪器编码
                string deviceID = Encoding.ASCII.GetString(buffer, 12, 16).TrimEnd('\0');
                result += deviceID + "|";
                ///采样状态
                result += BitConverter.ToBoolean(buffer, 28) + "|";
                ///电池电量
                //result += Convert.ToInt16(BitConverter.ToString(buffer, 29, 1), 16) + "|";
                result += Encoding.ASCII.GetString(buffer, 29, 64).TrimEnd('\0') + "|";
                ///传感器状态
                result += BitConverter.ToBoolean(buffer, 93) + "|";
                ///GPS连接状态
                result += BitConverter.ToBoolean(buffer, 94) + "|";
                ///经度
                result += BitConverter.ToSingle(buffer, 95) + "|";
                ///纬度
                result += BitConverter.ToSingle(buffer, 99) + "|";
                ///可见卫星数
                result += BitConverter.ToInt16(buffer, 103) + "|";
                ///追踪卫星数
                result += BitConverter.ToInt16(buffer, 105) + "|";
                ///信号强度
                result += BitConverter.ToInt32(buffer, 107) + "|";
            }
            else if (type.Equals("03"))
            {
                ///包头
                result = BitConverter.ToString(buffer, 0, 11) + "|";
                ///IP地址或动态域名
                result += Encoding.ASCII.GetString(buffer, 12, 16) + "|";
                ///端口号
                result += BitConverter.ToInt32(buffer, 28) + "|";
            }
            else if (type.Equals("0E"))
            {
                ///GPS时间
                int nGPSTime = BitConverter.ToInt32(buffer, 12);
                result += nGPSTime + "|";
                ///GPS毫秒
                result += BitConverter.ToInt32(buffer, 16) + "|";
                ///计算结果
                int nMs = 100;
                long time = (long)nGPSTime * 10000000 + (long)nMs * 1000000 + 621355968000000000;

                DateTime now = new DateTime(time);
                DateTime localTime = now.ToLocalTime();
                result += localTime + "";
            }
            else if (type.Equals("0F"))
            {
                result += BitConverter.ToString(buffer, 0, 12) + "\r\n";
                int nGPSTime = BitConverter.ToInt32(buffer, 12);
                result += nGPSTime + "\r\n";
                ///GPS毫秒
                result += BitConverter.ToInt32(buffer, 16) + "\r\n";
                ///计算结果
                int nMs = 200;
                long time = (long)nGPSTime * 10000000 + (long)nMs * 1000000 + 621355968000000000;
                DateTime now = new DateTime(time);
                DateTime localTime = now.ToLocalTime();
                result = localTime + "";
                ///特征值
                result += BitConverter.ToSingle(buffer, 20) + "|";
                result += BitConverter.ToSingle(buffer, 24) + "|";
                result += BitConverter.ToSingle(buffer, 28) + "|";
                result += BitConverter.ToSingle(buffer, 32) + "|";
                result += BitConverter.ToSingle(buffer, 36) + "|";
                result += BitConverter.ToSingle(buffer, 40) + "|";
                result += BitConverter.ToSingle(buffer, 44) + "|";
                result += BitConverter.ToSingle(buffer, 48) + "|";
            }
            LogHelper.Log.Info(result);
            return result.Trim();
        }
    }
}
