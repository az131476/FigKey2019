using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using FigKeyLoggerServer.Model;
using CommonUtils.Logger;
using CommonUtils.FigKeyCommand;

namespace FigKeyLoggerServer.SanNiuSignal
{
    public class DeviceHandler
    {
        private List<Device> devList;
        private FigKeySocketServer socketServer;

        public const int iSigTimestOffset = 12;
        public const int iSigEigenValOffset = 20;
        public const int iSigChlCountOffset = 140;
        public const int iSigDataLengthOffset = 141;
        public const int iSigDataOffset = 145;//12 + 8 + 120 + 1 + 4

        public DeviceHandler(List<Device> devList,FigKeySocketServer server)
        {
            this.devList = devList;
            this.socketServer = server;
        }

        public void parseBytes(IPEndPoint ipEndPoint, byte[] bytes)
        {
            int i = 0;
            string strRes = "";
            LogHelper.Log.Info("收到设备数据----------");
            //string ipAdr = ipEndPoint.Address.ToString();

            //if(null == ES_GetDevbyIP(ipEndPoint))
            //{
            //    dev.ipDevEP = ipEndPoint;
            //    ESDevList.Add(dev);
            //    LogHelper.WriteLog("devlist count " + ESDevList.Count);
            //}
            DEVICE_MSG_TYPE eType = (DEVICE_MSG_TYPE)DeviceSignal.GetType(bytes);
            Device device = getDevByIP(ipEndPoint);
            if (null == device)
            {
                if (DEVICE_MSG_TYPE.INST_MSG_TYPE_HEARTBEAT != eType)
                {
                    LogHelper.Log.Info("没有匹配的设备----------" + ipEndPoint);
                    return;
                }
            }
            else
            {
                //curDevIp = ipEndPoint;
                device.DeviceEndPoint.Port = ipEndPoint.Port;
                if (device.DeviceState == (int)Device.DEVICE_STATE_ENUM.STATE_DISCONNECT)
                {
                    device.DeviceState = (int)Device.DEVICE_STATE_ENUM.STATE_STOP;
                }
            }

            //INST_MSG_TYPE_ENUM eType = (INST_MSG_TYPE_ENUM)ES_DevSignal.GetType(bytes);
            int iLength = DeviceSignal.GetLength(bytes);
            LogHelper.Log.Info("type " + (int)eType);
            //if (INST_MSG_TYPE_ENUM.INST_MSG_TYPE_DATA != eType)                 
            //{
            //    strRes = ES_DevSignal.parseSignal(bytes);
            //}
            //LogHelper.WriteLog("收到模拟数据----------"+ eType);

            switch (eType)
            {
                case DEVICE_MSG_TYPE.INST_MSG_TYPE_HEARTBEAT:
                    string strDevID = DeviceSignal.GetDevID(bytes).Trim();
                    Device idDevice = getDevByID(strDevID);
                    if (null == idDevice)
                    {
                        break;
                    }
                    ///更新当前设备到数据库
                    //socketServer.config.UpdateCurDevIP(ipEndPoint, strDevID);

                    if (null == device)
                    {
                        device = idDevice;
                    }
                    if (!device.DeviceEndPoint.Equals(ipEndPoint))
                    {
                        device.DeviceEndPoint = ipEndPoint;
                        //device.UpdateDBEP();更新数据库设备IP
                    }
                    if (device.DeviceState == (int)Device.DEVICE_STATE_ENUM.STATE_DISCONNECT)
                    {
                        device.DeviceState = (int)Device.DEVICE_STATE_ENUM.STATE_STOP;
                    }
                    //device.Feed();
                    strRes = DeviceSignal.parseSignal(bytes);
                    sendStatusClient(device,strRes);
                    break;
                case DEVICE_MSG_TYPE.INST_MSG_TYPE_DATA:

                    LogHelper.Log.Info((int)DEVICE_MSG_TYPE.INST_MSG_TYPE_DATA+"下载文件---------------start");
                    long lTimeStramp = 0;
                    if (bytes.Length >= iSigDataOffset)
                    {
                        lTimeStramp = BitConverter.ToInt64(bytes, iSigTimestOffset);
                        
                    }
                    else
                    {
                        break;
                    }
                    if (device.DeviceState == (int)Device.DEVICE_STATE_ENUM.STATE_STOP)
                    {
                        device.DeviceState = (int)Device.DEVICE_STATE_ENUM.STATE_RUNNING;
                    }

                    byte[] cChlCounts = BitConverter.GetBytes(BitConverter.ToChar(bytes, iSigChlCountOffset));
                    if (3 != cChlCounts[0])
                    {
                        return;
                    }
                    int iDataNum = BitConverter.ToInt32(bytes, iSigDataLengthOffset);
                    lock (device.objLocAcp)
                    {
                        //FrontThreshold ft = new FrontThreshold();
                        //ft.fThreshold_1 = dDefThreshold;
                        //ft.fThreshold_2 = dDefMean;
                        //ft.fThreshold_3 = dDefCount;
                        float[] afCharVal = new float[30];

                        afCharVal[0] = BitConverter.ToSingle(bytes, iSigEigenValOffset);
                        afCharVal[1] = BitConverter.ToSingle(bytes, iSigEigenValOffset + 4);

                        afCharVal[4] = BitConverter.ToSingle(bytes, iSigEigenValOffset + 40);
                        afCharVal[5] = BitConverter.ToSingle(bytes, iSigEigenValOffset + 44);

                        afCharVal[8] = BitConverter.ToSingle(bytes, iSigEigenValOffset + 80);
                        afCharVal[9] = BitConverter.ToSingle(bytes, iSigEigenValOffset + 84);

                        //bool bIsTrigger = checkCharVal(afCharVal, ft);

                        //OneSignal TimeSig = new OneSignal();
                        //TimeSig.s = 1;
                        //TimeSig.t = lTimeStramp;   //采集起始毫秒时间戳
                        ////TimeSig.y = BitConverter.ToInt32(bytes, 16);   //采集起始毫秒时间戳高32位
                        //if (device.bIsNeedRefreshStartTime)
                        //{
                        //    device.lClctStartTime = TimeSig.t;
                        //    device.bIsNeedRefreshStartTime = false;
                        //}
                        ////Log.Write("     " + TimeSig.t + "    " + TimeStamp.ConvertStringToDateTime(TimeSig.t + ""));
                        //device.SigQ.Enqueue(TimeSig);
                        ////Log.Write("parseBytes " + "TimeSig" + "[" + device.SigQ.Count + "]");

                        //for (i = 0; i < 30; i++)
                        //{
                        //    OneSignal Sig = new OneSignal();
                        //    if (bIsTrigger)
                        //    {
                        //        Sig.s = 2;
                        //    }
                        //    else
                        //    {
                        //        Sig.s = 3;
                        //    }
                        //    Sig.x = BitConverter.ToSingle(bytes, iSigEigenValOffset + 4 * i);

                        //    device.SigQ.Enqueue(Sig);
                        //    //Log.Write("parseBytes " + "Cha[" + i + "]" + "[" + device.SigQ.Count + "]");
                        //}

                        //if ((ES_Device.DEVICE_CLCT_TYPE_ENUM.CLCT_TYPE_TIMECONT == device.eClctType) &&
                        //    (false == bIsTrigger))
                        //{
                        //    break;
                        //}

                        //for (i = 0; i < iDataNum; i++)
                        //{
                        //    OneSignal Sig = new OneSignal();
                        //    Sig.x = BitConverter.ToSingle(bytes, iSigDataOffset + 4 * i);
                        //    Sig.y = BitConverter.ToSingle(bytes, iSigDataOffset + 4 * (i + iDataNum));
                        //    Sig.z = BitConverter.ToSingle(bytes, iSigDataOffset + 4 * (i + 2 * iDataNum));

                        //    device.SigQ.Enqueue(Sig);
                        //    //Log.Write("parseBytes " + "Val[" + i + "]" + "[" + device.SigQ.Count + "]");
                        //}

                        //OneSignal EndSig = new OneSignal();
                        //EndSig.s = 10;
                        //device.SigQ.Enqueue(EndSig);
                    }

                    LogHelper.Log.Info("load enqueue over----------------");
                    break;
                default:
                    break;
            }
            //revRes = ES_DevSignal.SignalParse(bytes);

            //ES_Device device = ES_GetDevbyIP(ipEndPoint.Address.ToString());
            if (null != device)
            {
                //string path_save_original = pro_path + device.DevID + "\\original";
                //if (!Directory.Exists(path_save_original))
                //{
                //    Directory.CreateDirectory(path_save_original);
                //}
                //DataFile.SaveTxt(revRes, path_save_original);
            }
        }

        /// <summary>
        /// 转发设备心跳给客户端
        /// </summary>
        /// <param name="device"></param>
        public void sendStatusClient(Device device,string strRes)
        {
            string[] ds = strRes.Split(new char['|'],StringSplitOptions.RemoveEmptyEntries);
            device.DeviceId = ds[1];
            //
            string strMsg = ServerCommand.HEAET_CON + "|" + device.DeviceId + "|" + device.DeviceState + "|" + device.DeviceEndPoint;
            socketServer.clnHandler.sendDevStatus2Client(device.DeviceId, strMsg);
        }

        #region 查找设备
        /// <summary>
        /// deviceid查找设备
        /// </summary>
        /// <param name="strDevID"></param>
        /// <returns></returns>
        public Device getDevByID(string strDevID)
        {
            try
            {
                return devList.Find(delegate (Device device) { return device.DeviceId.Equals(strDevID); });
            }
            catch { return null; }
        }

        /// <summary>
        /// IP Endpoint查找设备
        /// </summary>
        /// <param name="ipEndPoint"></param>
        /// <returns></returns>
        public Device getDevByIP(IPEndPoint ipEndPoint)
        {
            try
            {
                return devList.Find(delegate (Device device) { return device.DeviceEndPoint.Address.Equals(ipEndPoint.Address); });
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 获取设备状态
        /// <summary>
        /// 获取所有设备的状态
        /// </summary>
        /// <returns></returns>
        public string getDevStates()
        {
            int i = 0;
            string str = "";

            for (i = 0; i < devList.Count; i++)
            {
                str += "|" + devList[i].DeviceId + "|" + (int)devList[i].DeviceState;
            }

            return str;
        }
        #endregion
    }
}
