using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using CANManager.Model;
using CommonUtils.Logger;

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

    public enum ModelSID
    {
        SID_01 = 01,
        SID_02 = 02,
        SID_03 = 03,
        SID_04 = 04,
        SID_05 = 05,
        SID_06 = 06,
        SID_07 = 07,
        SID_08 = 08,
        SID_09 = 09,
        SID_0A = 0X0A
    }
    public enum ModelRebackValueSID
    {
        NONE,
        PID_OX41 = 0X41

    }

    public enum Model_PID
    {
        PID_0X01 = 0X01,
        PID_OX02 = 0X02,
        PID_OX03 = 0X03,
        PID_OX04 = 0X04,
        PID_0X05 = 0X05,
        PID_OX05 = 0X06
    }

    public struct PID_MEAN
    {
        public const string PID_01_DATAA_0_6 = "sid=01;pid=01;dataA binaryBit=0-6 意义：与排放相关的DTC数量 结果：";
        public const string PID_01_DATAA_07 = "sid=01;pid=01;dataA binaryBit=07 意义：故障指示灯状态 结果：";
        public const string PID_01_DATAB_00 = "sid=01;pid=01;dataB binaryBit=00 意义：失火监控支持 结果：";
        public const string PID_01_DATAB_01 = "sid=01;pid=01;dataB binaryBit=01 意义：燃油系统监控支持 结果：";
        public const string PID_01_DATAB_02 = "sid=01;pid=01;dataB binaryBit=02 意义：支持全面组件监控 结果：";
        public const string PID_01_DATAB_03 = "保留";
        public const string PID_01_DATAB_04 = "sid=01;pid=01;dataB binaryBit=04 意义：失火检测就绪 结果：";
        public const string PID_01_DATAB_05 = "sid=01;pid=01;dataB binaryBit=05 意义：燃油系统检测就绪 结果：";
        public const string PID_01_DATAB_06 = "sid=01;pid=01;dataB binaryBit=06 意义：综合组件检测就绪 结果：";
        public const string PID_01_DATAB_07 = "保留";
    }

    public class PidServer
    {
        private static DeviceInfo device;
        public delegate void MsgDelegate(StringBuilder msg);
        private static uint FLOW_CONTROL_FILTER = 0x00000003;
        private static uint ISO15765_FRAME_PAD = 0x00000040;
        private static uint pMsgID = 1;
        private static uint CLEAR_RX_BUFFER = 0x08;
        private static uint CLEAR_TX_BUFFER = 0x07;
        private static byte sid = 0x01;
        private static byte pid = 0x00;
        private static List<byte> funCodeList = new List<byte>();
        private static int frameAddressDef = 1;
        private static int frameAddressIndex;
        private static int frameSupportNext = 0;//是否继续支持下一帧,1-support
        private static bool IsSearchAllAddress;//查询所有支持的服务地址
        private static StringBuilder analysisContent = new StringBuilder();//解析数据缓存
        

        unsafe public static void PassThruStartMsgFilter(DeviceInfo device)
        {
            PassthruMsg pMaskMsg = new PassthruMsg();
            pMaskMsg.ProtocolID = (uint)device.ProtocolID;
            pMaskMsg.DataSize = 4;
            if (pMaskMsg.ProtocolID == (uint)DeviceInfo.ProtocolType.ISO15765)
            {
                pMaskMsg.TxFlags = ISO15765_FRAME_PAD;
                pMaskMsg.Data[0] = 0xFF;
                pMaskMsg.Data[1] = 0xFF;
                pMaskMsg.Data[2] = 0xFF;
                pMaskMsg.Data[3] = 0xFF;
            }
            else if(pMaskMsg.ProtocolID == (uint)DeviceInfo.ProtocolType.ISO14230)
            {
                pMaskMsg.TxFlags = 0;
                pMaskMsg.Data[0] = 0x00;
                pMaskMsg.Data[1] = 0xFF;
                pMaskMsg.Data[2] = 0xFF;
                pMaskMsg.Data[3] = 0x00;
            }

            PassthruMsg pPatternMsg = new PassthruMsg();
            pPatternMsg.ProtocolID = (uint)device.ProtocolID;
            if (pPatternMsg.ProtocolID == (uint)DeviceInfo.ProtocolType.ISO15765)
                pPatternMsg.TxFlags = ISO15765_FRAME_PAD;
            else
                pPatternMsg.TxFlags = 0;
            pPatternMsg.DataSize = 4;
            pPatternMsg.Data[0] = 0;
            pPatternMsg.Data[1] = 0;
            pPatternMsg.Data[2] = 7;
            pPatternMsg.Data[3] = 0xe8;

            int res_Icotl_rx = MonGoose.MonIcotl(device.ChannelID, CLEAR_RX_BUFFER, new IntPtr(0), new IntPtr(0));
            int res_Icotl_tx = MonGoose.MonIcotl(device.ChannelID, CLEAR_TX_BUFFER, new IntPtr(0), new IntPtr(0));

            LogHelper.Log.Info($"res_Icotl_rx:{res_Icotl_rx}+ res_Icotl_tx:{res_Icotl_tx}");

            PassthruMsg pFlowControlMsg = new PassthruMsg();
            pFlowControlMsg.ProtocolID = (uint)device.ProtocolID;
            pFlowControlMsg.TxFlags = ISO15765_FRAME_PAD;
            pFlowControlMsg.DataSize = 4;
            if (device.ProtocolID == DeviceInfo.ProtocolType.ISO15765)//当选择的是UDS时，过滤ID
            {
                pFlowControlMsg.Data[0] = 0;
                pFlowControlMsg.Data[1] = 0;
                pFlowControlMsg.Data[2] = 7;
                pFlowControlMsg.Data[3] = 0xe0;
            }
            else if (device.ProtocolID == DeviceInfo.ProtocolType.ISO14230)//当选择的是KWP时，只过滤源地址和目标地址
            {
                pPatternMsg.Data[0] = 0x00;
                pPatternMsg.Data[1] = 0x00;//iRxID;
                pPatternMsg.Data[2] = 0x00;// iTxID;
                pPatternMsg.Data[3] = 0x00;
            }
            int res_startMsgFilter = MonGoose.MonStartMsgFilter(device.ChannelID, FLOW_CONTROL_FILTER,ref pMaskMsg, ref pPatternMsg, ref pFlowControlMsg, ref pMsgID);
            LogHelper.Log.Info($"res_startMsgFilter:{res_startMsgFilter}");
        }

        unsafe public static void CommandMode(DeviceInfo deviceInfo)
        {
            device = deviceInfo;

            switch (device.ModelSidType)
            {
                case DeviceInfo.ModelType.MODEL1:
                    PassThruWriteMsgs(false);
                    break;
                case DeviceInfo.ModelType.MODEL2:

                    break;
                case DeviceInfo.ModelType.MODEL3:

                    break;
                case DeviceInfo.ModelType.MODEL4:

                    break;
                case DeviceInfo.ModelType.MODEL5:

                    break;
                case DeviceInfo.ModelType.MODEL6:

                    break;
                case DeviceInfo.ModelType.MODEL7:

                    break;
                case DeviceInfo.ModelType.MODEL8:

                    break;
                case DeviceInfo.ModelType.MODEL9:

                    break;
                case DeviceInfo.ModelType.MODELA:

                    break;
            }
        }
        unsafe private static void PassThruWriteMsgs(bool IsAllSupport)
        {
            //写数据
            PassthruMsg writeStruct = new PassthruMsg();

            writeStruct.ProtocolID = (uint)device.ProtocolID;
            writeStruct.RxStatus = 0;
            if (device.ProtocolID == DeviceInfo.ProtocolType.ISO15765)
                writeStruct.TxFlags = ISO15765_FRAME_PAD;
            else
                writeStruct.TxFlags = 0x00;
            writeStruct.Timestamp = 0;

            writeStruct.Data[0] = 0x00;
            writeStruct.Data[1] = 0x00;
            writeStruct.Data[2] = 0x07;
            writeStruct.Data[3] = 0xdf;//7df  7e0

            writeStruct.Data[4] = sid;
            writeStruct.Data[5] = pid;

            writeStruct.DataSize = 6;
            uint pNumMsg = 1;
            uint timeout = 100;

            int res_Icotl_tx = MonGoose.MonIcotl(device.ChannelID, CLEAR_TX_BUFFER, new IntPtr(0), new IntPtr(0));
            LogHelper.Log.Info($"res_Icotl_tx:{res_Icotl_tx}");
            int res_send = MonGoose.MonWriteMsgs(device.ChannelID, ref writeStruct, ref pNumMsg, timeout);
            LogHelper.Log.Info($"res_send:{res_send}");
            //发送完成后，去读返回值
            if (!IsAllSupport)
            {
                PassThruReadMsgs();
            }
            else
            {
                AnalysisReceive();
            }
        }

        unsafe private static void PassThruReadMsgs()
        {
            //读数据
            string framePerBinary = "";
            PassthruMsg Msg = new PassthruMsg();
            Msg.TxFlags = ISO15765_FRAME_PAD;
            Msg.ProtocolID = (uint)device.ProtocolID;
            uint pnumMsg = 1;
            uint timeout = 100;
            int res_read = MonGoose.MonReadMsgs(device.ChannelID, ref Msg, ref pnumMsg, timeout);
            if (Msg.Data[4] == (int)ModelRebackValueSID.PID_OX41)
            {
                if (Msg.Data[5] == pid)
                {
                    framePerBinary = Msg.Data[6].ToString() + Msg.Data[7].ToString() + Msg.Data[8].ToString() + Msg.Data[9].ToString();
                    LogHelper.Log.Info($"framePerString:" + framePerBinary);
                    framePerBinary = Convert.ToString(Convert.ToInt32(framePerBinary),2);
                    LogHelper.Log.Info($"framePerBinary:"+framePerBinary);
                    char[] curFrameArray = framePerBinary.ToCharArray();
                    //将该帧中支持的地址添加到集合
                    for (frameAddressIndex = frameAddressDef; frameAddressIndex <= curFrameArray.Length; frameAddressIndex++)
                    {
                        if (curFrameArray[frameAddressIndex] == 1)//1-support,0-not support address
                        {
                            funCodeList.Add(Convert.ToByte(Convert.ToString(frameAddressIndex, 16)));
                        }
                    }
                    //判断帧最后一位是否支持
                    if (frameAddressIndex == curFrameArray.Length)
                    {
                        if (curFrameArray[frameAddressIndex] == 1)
                        {
                            frameAddressDef += 32;
                            frameSupportNext = 1;//继续
                            pid += 0x20;
                            PassThruWriteMsgs(false);
                        }
                        else
                        {
                            //查询不到下一帧有支持的地址
                            //查询所有已知的服务地址，并解析
                            frameSupportNext = 0;
                            ExcuteAllSupportAddress();
                        }
                    }
                }
            }
        }

        unsafe private static void AnalysisReceive()
        {
            //读数据
            PassthruMsg Msg = new PassthruMsg();
            Msg.TxFlags = ISO15765_FRAME_PAD;
            Msg.ProtocolID = (uint)device.ProtocolID;
            uint pnumMsg = 1;
            uint timeout = 100;
            int res_read = MonGoose.MonReadMsgs(device.ChannelID, ref Msg, ref pnumMsg, timeout);
            if (Msg.Data[4] == (int)ModelRebackValueSID.PID_OX41)
            {
                if (Msg.Data[5] == pid)
                {
                    string framePerBinaryDataA = Msg.Data[6].ToString();
                    string framePerBinaryDataB = Msg.Data[7].ToString();
                    string framePerBinaryDataC = Msg.Data[8].ToString();
                    string framePerBinaryDataD = Msg.Data[9].ToString();

                    framePerBinaryDataA = Convert.ToString(Convert.ToInt32(framePerBinaryDataA), 2);
                    framePerBinaryDataB = Convert.ToString(Convert.ToInt32(framePerBinaryDataB), 2);
                    framePerBinaryDataC = Convert.ToString(Convert.ToInt32(framePerBinaryDataC), 2);
                    framePerBinaryDataD = Convert.ToString(Convert.ToInt32(framePerBinaryDataD), 2);

                    switch (pid)
                    {
                        case (int)Model_PID.PID_0X01:
                            PID_0XO1(framePerBinaryDataA.ToCharArray(),framePerBinaryDataB.ToCharArray(),framePerBinaryDataC.ToCharArray(),framePerBinaryDataD.ToCharArray());
                            break;
                    }
                }
            }
        }

        private static void PID_0XO1(char[] dataA,char[] dataB,char[] dataC,char[] dataD)
        {
            for (int i = 0; i < dataA.Length; i++)
            {
                switch (i)
                {

                    case 7:
                        if (dataA[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAA_07} ON");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAA_07} OFF");
                        }
                        break;
                }
            }
            for (int i = 0; i < dataB.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine();
                        }
                        break;
                }
            }
        }
        private static void ExcuteAllSupportAddress()
        {
            for (int i = 0; i < funCodeList.Count; i++)
            {
                pid = funCodeList[i];
                PassThruWriteMsgs(true);
            }
        }
    }
}
