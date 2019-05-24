using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BXHSerialPort.Protocol
{
    class ProtocolDefine
    {
        public const byte frameHead = 0xEF;
        public const byte negativeRetry = 0x78;
        public const byte negativeStop = 0x7F;

        public static string[] errCode = 
        {
            "ok",
            "没有检测到帧头",
            "校验和错误",
            "传输长度与数据不符",
            "波特率超出参数设置范围 500-10000",
            "地址超出范围 0-3fff",
            "长度超出范围",
            "写入超时",
            "写入数据与读回数据不符合",
            "要写入的数据长度为基数"
        };
        public enum Functions
        {
            setBaudrate,
            writeEnable,
            writeDisable,
            readBytes,
            writeBytes
        }
        public static Dictionary<Functions, byte> SID = new Dictionary<Functions, byte>
        {
            { Functions.setBaudrate,0x14},
            { Functions.writeEnable,0x15},
            { Functions.writeDisable,0x16},
            { Functions.readBytes,0x22},
            { Functions.writeBytes,0x2E}
        };
        public static Dictionary<Functions, byte> RSID = new Dictionary<Functions, byte>
        {
            { Functions.setBaudrate,0x54},
            { Functions.writeEnable,0x55},
            { Functions.writeDisable,0x56},
            { Functions.readBytes,0x62},
            { Functions.writeBytes,0x6E}
        };
    }
}
