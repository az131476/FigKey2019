using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CANManager.Test
{

    class TestMonGoose
    {
        [DllImport("MongooseProISO2")]
        public static extern long Open();

        [DllImport("MongooseProISO2")]
        public static extern long MongooseProISO2Setup();

        [DllImport("MongooseProISO2")]
        public static extern long MongooseProISO2FastInit(uint[] pInputMsg, int pInputMsgSize, uint[] pOnputMsg, ref int pOnputMsgSize);

        [DllImport("MongooseProISO2")]
        public static extern long MongooseProISO2WriteMsg(uint[] pInputMsg, int pInputMsgSize, uint Timeout);
        [DllImport("MongooseProISO2")]
        public static extern long MongooseProISO2ReadMsg(uint[] pOnputMsg, ref int pOnputMsgSize, uint Timeout);
        [DllImport("MongooseProISO2")]
        public static extern long MongooseProISO2Close();
        //long WINAPI MongooseProISO2Setup();
        //long WINAPI MongooseProISO2FastInit(unsigned char pInputMsg[], int pInputMsgSize, unsigned char pOnputMsg[], int * pOnputMsgSize);
        //long WINAPI MongooseProISO2WriteMsg(unsigned char pInputMsg[], int pInputMsgSize, unsigned long Timeout);
        //long WINAPI MongooseProISO2ReadMsg(unsigned char pOnputMsg[], int * pOnputMsgSize, unsigned long Timeout);
        //long WINAPI MongooseProISO2Close();
    }
}
