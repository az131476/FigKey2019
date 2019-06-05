using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CANManager.CAN
{
    public struct Diagnose
    {
        public uint dignostypes;  //诊断协议 		0:15031 1:14229 2:KWP2000
        public uint commtypes;        //通讯类型 		0:CAN 1:K-Line
        public uint frameType;        //帧类型	 		0:标准帧 1:扩展帧
        public uint initType;         //初始化方式	0:快速初始化 1:5Buad初始化

        public uint tgtAddr;               //ECU标识符		
        public uint srcAddr;               //诊断仪标识符
        public uint funAddr;               //功能寻址
        public uint Baud;                  //波特率

        public uint server_id;        //服务ID
        public uint sub_function; //服务子功能
        public uint client;               //客户
    }

    class PidServer
    {
        [DllImport("dll")]
        public static extern int UdsRequest(ref Diagnose diaInput,int v, int[] commbuf, int txlen, out int[] rxbuf, out int rxlen);

        private int[] outArray = new int[256];
        private int subCodeLen = 0;
        private int[] rxbuf;
        private int rxlen;
        public enum FunType
        {
            query,
            write,
            def
        }
        //ret = UdsRequest1(diagnose.funAddr, 0, (unsigned char *)commbuf, txlen, rxbuf, (unsigned short *)rxlen);
        /// <summary>
        /// 单步查询支持的服务码
        /// </summary>
        public void SignalQueryPid(int cmd)
        {
            
        }
        /// <summary>
        /// 查询子功能服务
        /// </summary>
        //public int QuerySubCode(FunType funType,out int[] outArray, out int subCodeLen)
        //{
            
        //    return 0;
        //}

        private int ReadCAN(byte[] input,int size,int timeout)
        {
            //02 01 00 55 55 55 55 55 
            return 0;
        }

        private int WriteCAN(byte[] input,int size,int timeout)
        {
            //06 41 00 be 3e a8 13 00 
            return 0;
        }
    }
}
