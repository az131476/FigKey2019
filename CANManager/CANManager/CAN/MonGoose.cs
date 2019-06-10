using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace CANManager
{
    unsafe public struct PassthruMsg
    {
        public uint ProtocolID;//def=6(ISO15765)
        public uint RxStatus;//def:0
        public uint TxFlags;//def=0,if protocolID=6 txFlags=0x00000040 else = 0
        public uint Timestamp;
        public uint DataSize;
        public uint ExtraDataIndex;
        public fixed byte Data[4128];
    }
    public class MonGoose
    {
        #region import extern dll
        [DllImport("MongooseProISO2")]
        private static extern int PassThruOpen(IntPtr name,ref uint deviceID);

        [DllImport("MongooseProISO2")]
        private static extern int PassThruConnect(uint DeviceID, uint ProtocolID, uint Flags, uint BaudRate, ref uint pChannelID);

        [DllImport("MongooseProISO2")]
        private static extern int PassThruDisconnect(uint ChannelID);

        [DllImport("MongooseProISO2")]
        private static extern int PassThruClose(uint DeviceID);

        [DllImport("MongooseProISO2")]
        private static extern int PassThruReadVersion(uint DeviceID, string pFirmwareVersion, string pDllVersion, string pApiVersion);

        [DllImport("MongooseProISO2")]
        public static extern int PassThruGetLastError(string pErrorDescription);

        [DllImport("MongooseProISO2")]
        private static extern int PassThruIoctl(uint HandleID, uint IoctlID, IntPtr pInput, IntPtr pOutput);

        [DllImport("MongooseProISO2")]
        private static extern int PassThruReadMsgs(uint ChannelID, ref PassthruMsg pMsg, ref uint pNumMsgs,uint Timeout);

        [DllImport("MongooseProISO2")]
        private static extern int PassThruStartMsgFilter(uint ChannelID, uint FilterType, ref PassthruMsg pMaskMsg, ref PassthruMsg pPatternMsg, ref PassthruMsg pFlowControlMsg, ref uint pMsgID);


        [DllImport("MongooseProISO2")]
        private static extern int PassThruWriteMsgs(uint ChannelID, ref PassthruMsg pMsg, ref uint pNumMsgs, uint Timeout);

        [DllImport("MongooseProISO2")]
        private static extern int PassThruStartPeriodicMsg(uint ChannelID, ref PassthruMsg pMsg, ref uint pMsgID, uint TimeInterval);

        [DllImport("MongooseProISO2")]
        private static extern int PassThruStopPeriodicMsg(uint ChannelID, uint MsgID);
        #endregion

        #region func

        /// <summary>
        /// 初始化并连接到Cardaq
        /// </summary>
        /// <param name="name"></param>
        /// <param name="deviceID"></param>
        /// <returns></returns>
        public static int MonOpen(IntPtr name,ref uint deviceID)
        {
            return PassThruOpen(name, ref deviceID);
        }

        /// <summary>
        /// 连接到车辆网络
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <param name="ProtocolID"></param>
        /// <param name="Flags"></param>
        /// <param name="BaudRate"></param>
        /// <param name="pChannelID"></param>
        /// <returns></returns>
        public static int MonConnect(uint DeviceID, uint ProtocolID, uint Flags, uint BaudRate,ref uint pChannelID)
        {
            return PassThruConnect(DeviceID,ProtocolID,Flags,BaudRate,ref pChannelID);
        }

        /// <summary>
        /// 断开到车辆网络
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <returns></returns>
        public static int MonDisconnect(uint ChannelID)
        {
            return PassThruDisconnect(ChannelID);
        }

        /// <summary>
        /// 与Cardaq的断开连接
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <returns></returns>
        public static int MonClose(uint DeviceID)
        {
            return PassThruClose(DeviceID);
        }

        /// <summary>
        /// 报表版本信息
        /// </summary>
        /// <param name="DeviceID"></param>
        /// <param name="pFirmwareVersion"></param>
        /// <param name="pDllVersion"></param>
        /// <param name="pApiVersion"></param>
        /// <returns></returns>
        public static int MonReadVersion(uint DeviceID, string pFirmwareVersion, string pDllVersion, string pApiVersion)
        {
            return PassThruReadVersion(DeviceID,pFirmwareVersion,pDllVersion,pApiVersion);
        }

        /// <summary>
        /// 描述最近的错误
        /// </summary>
        /// <param name="pErrorDescription"></param>
        /// <returns></returns>
        public static int MonGetLastError(string pErrorDescription)
        {
            return PassThruGetLastError(pErrorDescription);
        }

        /// <summary>
        /// 多重意义，传入参数内容不同，导致不同结果
        /// </summary>
        /// <param name="HandleID"></param>
        /// <param name="IoctlID"></param>
        /// <param name="pInput"></param>
        /// <param name="pOutput"></param>
        /// <returns></returns>
        public static int MonIcotl(uint HandleID, uint IoctlID, IntPtr pInput, IntPtr pOutput)
        {
            return PassThruIoctl(HandleID,IoctlID,pInput,pOutput);
        }

        /// <summary>
        /// 接收网络消息
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="pMsg"></param>
        /// <param name="pNumMsgs"></param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public static int MonReadMsgs(uint ChannelID, ref PassthruMsg pMsg, ref uint pNumMsgs, uint Timeout)
        {
            return PassThruReadMsgs(ChannelID,ref pMsg,ref pNumMsgs,Timeout);
        }

        /// <summary>
        /// 应用网路讯息过滤器
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="FilterType"></param>
        /// <param name="pMaskMsg"></param>
        /// <param name="pPatternMsg"></param>
        /// <param name="pFlowControlMsg"></param>
        /// <param name="pMsgID"></param>
        /// <returns></returns>
        public static int MonStartMsgFilter(uint ChannelID, uint FilterType,ref PassthruMsg pMaskMsg, 
            ref PassthruMsg pPatternMsg,ref PassthruMsg pFlowControlMsg, ref uint pMsgID)
        {
            return PassThruStartMsgFilter(ChannelID,FilterType,ref pMaskMsg,ref pPatternMsg,ref pFlowControlMsg,ref pMsgID);
        }

        /// <summary>
        /// 发送网络消息
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="pMsg"></param>
        /// <param name="pNumMsgs">指示该通道_msg数组仅包含几条消息</param>
        /// <param name="Timeout"></param>
        /// <returns></returns>
        public static int MonWriteMsgs(uint ChannelID, ref PassthruMsg pMsg, ref uint pNumMsgs, uint Timeout)
        {
            return PassThruWriteMsgs(ChannelID,ref pMsg,ref pNumMsgs,Timeout);
        }

        /// <summary>
        /// 持续传送讯息
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="pMsg"></param>
        /// <param name="pMsgID"></param>
        /// <param name="TimeInterval"></param>
        /// <returns></returns>
        public static int MonStartPeriodicMsg(uint ChannelID, ref PassthruMsg pMsg, ref uint pMsgID, uint TimeInterval)
        {
            return PassThruStartPeriodicMsg(ChannelID,ref pMsg,ref pMsgID,TimeInterval); 
        }

        /// <summary>
        /// 停止连续传送讯息
        /// </summary>
        /// <param name="ChannelID"></param>
        /// <param name="MsgID"></param>
        /// <returns></returns>
        public static int MonStopPeriodicMsg(uint ChannelID, uint MsgID)
        {
            return PassThruStopPeriodicMsg(ChannelID,MsgID);
        }
        #endregion
    }
}
