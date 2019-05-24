using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static BXHSerialPort.Protocol.ProtocolDefine;

namespace BXHSerialPort.Protocol
{
    class ProtocolHelper
    {
        public static byte[] makeSendData(Functions function,string[] para,byte[] sendBytes = null)
        {
            List<byte> sendData = new List<byte>();
            sendData.Add(ProtocolDefine.SID[function]);
            switch (function)
            {
                case Functions.setBaudrate:
                    UInt16 baudrate = Convert.ToUInt16(para[0]);
                    sendData.AddRange(BitConverter.GetBytes(baudrate).Reverse());
                    break;
                case Functions.readBytes:
                    UInt16 addr = Convert.ToUInt16(para[0],16);
                    sendData.AddRange(BitConverter.GetBytes(addr).Reverse());
                    UInt16 len = Convert.ToUInt16(para[1]);
                    sendData.AddRange(BitConverter.GetBytes(len).Reverse());
                    break;
                case Functions.writeBytes:
                    UInt16 addrW = Convert.ToUInt16(para[0]);
                    sendData.AddRange(BitConverter.GetBytes(addrW).Reverse());
                    byte readBackFlag = Convert.ToByte(para[1]);
                    sendData.Add(readBackFlag);
                    sendData.AddRange(sendBytes);
                    break;
            }
            sendData.Insert(0,(byte)(sendData.Count));
            sendData.Insert(0,ProtocolDefine.frameHead);
            byte checkSum = 0;
            foreach (var item in sendData)
            {
                checkSum += item;
            }
            sendData.Add(checkSum);
            return sendData.ToArray();
        }
        public static string analysisReceiveData(Functions function,byte[] receiveData,byte[] sendData)
        {
            if (receiveData==null||receiveData.Length==0)
            {
                return "没有收到数据！";
            }
            /*校验和验证*/
            byte checksum = 0;
            for (int i = 0; i < receiveData.Length - 1; i++)
            {
                checksum += receiveData[i];
            }
            if (checksum != receiveData[receiveData.Length - 1])
            {
                return "检验和错误";
            }
            /*帧头*/
            if (receiveData[0]!=ProtocolDefine.frameHead)
            {
                return "帧头错误";
            }
            if (receiveData[1]!=receiveData.Length-3)
            {
                return "数据长度错误";
            }
            if (receiveData[2] == ProtocolDefine.negativeRetry)
            {
                return "否定应答-可重传："+ ProtocolDefine.errCode[receiveData[3]];
            }
            if (receiveData[2] == ProtocolDefine.negativeStop)
            {
                return "否定应答-不可重传：" + ProtocolDefine.errCode[receiveData[3]];
            }
            if (receiveData[2] != ProtocolDefine.RSID[function])
            {
                return "RSID错误";
            }
            if (receiveData[3] != 0)
            {
                return "出错：" + ProtocolDefine.errCode[receiveData[3]];
            }
            if (function==Functions.setBaudrate)
            {
                if (receiveData[4]!=sendData[3]||receiveData[5]!=sendData[4])
                {
                    return "波特率设置和应答的不一致";
                }
            }
            return "ok";
        }
    }
}
