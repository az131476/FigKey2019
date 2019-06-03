using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System;

namespace CANManager.Test
{

    class TestMonGoose
    {
        [DllImport("MongooseProISO2")]
        private static extern long MongooseProISO2Setup();

        [DllImport("MongooseProISO2")]
        private static extern long MongooseProISO2FastInit(Int32[] pInputMsg, int pInputMsgSize, int[] pOnputMsg, int pOnputMsgSize);

        [DllImport("MongooseProISO2")]
        private static extern long MongooseProISO2WriteMsg(int[] pInputMsg, int pInputMsgSize, long Timeout);
        //long WINAPI MongooseProISO2Setup();
        //long WINAPI MongooseProISO2FastInit(unsigned char pInputMsg[], int pInputMsgSize, unsigned char pOnputMsg[], int * pOnputMsgSize);
        //long WINAPI MongooseProISO2WriteMsg(unsigned char pInputMsg[], int pInputMsgSize, unsigned long Timeout);
        //long WINAPI MongooseProISO2ReadMsg(unsigned char pOnputMsg[], int * pOnputMsgSize, unsigned long Timeout);
        //long WINAPI MongooseProISO2Close();
    }
}
