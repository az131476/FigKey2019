using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SanNiuSignal;
using SanNiuSignal.Basics;
using SanNiuSignal.FileCenter;
using CommonUtils.Logger;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using FigKeyLoggerServer.Model;

namespace FigKeyLoggerServer.SanNiuSignal
{
    public class FigKeySocketServer
    {
        private ITxServer txServer;
        private FigKeyConfig fkconfig;
        private List<Device> devList;
        private List<Client> clnList;
        internal ClientHandler clnHandler;
        internal DeviceHandler devHandler;
        private Socket devSocketServer;
        private string devicePort;
        //所有客户端
        private List<DeviceTcpState> tcpStateList;
        private int TcpBufferSize = 65536;//缓冲区大小

        private FigKeySocketServer(FigKeyConfig config,List<Device> devList,List<Client> clnList)
        {
            this.fkconfig = config;
            this.devList = devList;
            this.clnList = clnList;
            clnHandler = new ClientHandler(clnList,this);
            devHandler = new DeviceHandler(devList,this);

            //监听设备socket
            tcpStateList = new List<DeviceTcpState>();
            devSocketServer = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            devicePort = ConfigurationManager.AppSettings["devicePort"].ToString();
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, Convert.ToInt32(devicePort));
            devSocketServer.Bind(ipEndPoint);
        }
         
        public void startSocketListen()
        {
            try
            {
                txServer = TxStart.startServer(fkconfig.TcpPort);
                txServer.AcceptString += TxServer_AcceptString;
                txServer.AcceptByte += TxServer_AcceptByte;
                txServer.Connect += TxServer_Connect;
                txServer.dateSuccess += TxServer_dateSuccess;
                txServer.Disconnection += TxServer_Disconnection;
                txServer.EngineClose += TxServer_EngineClose;
                txServer.EngineLost += TxServer_EngineLost;
                txServer.BufferSize = 100000;
                txServer.StartEngine();
                LogHelper.Log.Info("socket server start...");

                devSocketServer.Listen(200);
                devSocketServer.BeginAccept(new AsyncCallback(acceptCallback), devicePort);
            }
            catch (Exception Ex)
            {
                LogHelper.Log.Error("[ERROR] start socket server error ", Ex);
            }
        }

        #region client socket listen

        public void TxServerStop()
        {
            txServer.CloseEngine();
            txServer.clientAllClose();
        }

        public void SendMsg(string msg, IPEndPoint ipEndPoint)
        {
            try
            {
                if (ipEndPoint == null)
                {
                    LogHelper.Log.Info("客户端"+ipEndPoint+"不存在,发送消息失败");
                    return;
                }
                if (!this.txServer.clientCheck(ipEndPoint))
                {
                    LogHelper.Log.Info("目标客户端不在线！" + ipEndPoint);
                    return;
                }
                txServer.sendMessage(ipEndPoint, msg);
            }
            catch (Exception Ex)
            {
                LogHelper.Log.Info("发送消息失败:" + Ex.Message);
            }
        }

        public void SendMsg(byte[] msgByte, IPEndPoint ipEndPoint)
        {
            try
            {
                if (ipEndPoint == null)
                {
                    LogHelper.Log.Info("客户端" + ipEndPoint + "不存在,发送消息失败");
                    return;
                }
                if (!this.txServer.clientCheck(ipEndPoint))
                {
                    LogHelper.Log.Info("目标客户端不在线！" + ipEndPoint);
                    return;
                }
                txServer.sendMessage(ipEndPoint, msgByte);
            }
            catch (Exception Ex)
            {
                LogHelper.Log.Info("发送消息失败:" + Ex.Message);
            }
        }

        /// <summary>
        /// 当服务器非正常原因断开的时候
        /// </summary>
        /// <param name="str"></param>
        private void TxServer_EngineLost(string str)
        {
            LogHelper.Log.Info("socket server 非正常断开..."+str);
        }

        /// <summary>
        /// 服务完全关闭
        /// </summary>
        private void TxServer_EngineClose()
        {
            LogHelper.Log.Info("socket server closed...");
        }

        private void TxServer_Disconnection(IPEndPoint ipEndPoint, string str)
        {
            ///停止所有与客户端数据的发送
            LogHelper.Log.Info(ipEndPoint + " 断开连接：" + str);
            if (null != clnHandler.getClientByIP(ipEndPoint))
            {
                for (int i = 0; i < clnList.Count; i++)
                {
                    if (clnList[i].ipEndPoint.Equals(ipEndPoint))
                        clnList.Remove(clnList[i]);
                }
            }

            if (null != devHandler.getDevByIP(ipEndPoint))
            {
                for (int j = 0; j < devList.Count; j++)
                {
                    if (devList[j].DeviceEndPoint.Address.Equals(ipEndPoint))
                        devList.Remove(devList[j]);
                }
            }
        }

        private void TxServer_dateSuccess(IPEndPoint ipEndPoint)
        {
            string sendMsgStatus = "已向" + ipEndPoint.ToString() + "发送成功" + "\r\n";
            LogHelper.Log.Info(sendMsgStatus);
        }

        private void TxServer_Connect(IPEndPoint ipEndPoint)
        {
            LogHelper.Log.Info("客户端连接进入:"+ipEndPoint);
        }

        private void TxServer_AcceptByte(IPEndPoint ipEndPoint, byte[] bytes)
        {
            //parseBytes(ipEndPoint, bytes);

            //clntHandler.parseBytes(ipEndPoint, bytes);
            return;
        }

        private void TxServer_AcceptString(IPEndPoint ipEndPoint, string revStr)
        {
            LogHelper.Log.Info("收到客户端消息：" + revStr);

            //clntHandler.parseString(ipEndPoint, revStr);
            return;
        }
        #endregion

        #region device socket listen
        private void acceptCallback(IAsyncResult ar)
        {
            DeviceTcpState stateOne = null;
            try
            {
                devSocketServer.BeginAccept(new AsyncCallback(acceptCallback), devicePort);
            }
            catch (Exception Ex)
            {
                //OnEngineLost(Ex.Message);//当服务器突然断开触发此事件
                //CloseEngine();
                LogHelper.Log.Error(Ex.Message+Ex.StackTrace);
            }
            try
            {
                Socket socketClient = devSocketServer.EndAccept(ar);
                stateOne = new DeviceTcpState(socketClient, TcpBufferSize);
                tcpStateList.Add(stateOne);

                stateOne.WorkSocket.BeginReceive(stateOne.Buffer, 0, stateOne.Buffer.Length, 0, new AsyncCallback(receiveCallback), stateOne);
                LogHelper.Log.Info("设备连接地址 " + stateOne.ipEndPoint);
            }
            catch (Exception Ex)
            {
                LogHelper.Log.Info(Ex.Message + Ex.StackTrace);
            }
        }

        private void receiveCallback(IAsyncResult ar)
        {
            DeviceTcpState stateOne = (DeviceTcpState)ar.AsyncState;
            Socket handler = stateOne.WorkSocket;
            try
            {
                int bytesRead = handler.EndReceive(ar);
                if (bytesRead > 0)
                {
                    //byte[] haveDate = ReceiveDateOne.DateOneManage(stateOne, bytesRead);//接收完成之后对数组进行重置
                    receiveParse(stateOne);
                    handler.BeginReceive(stateOne.Buffer, 0, stateOne.Buffer.Length, 0, new AsyncCallback(receiveCallback), stateOne);
                    //receiveParse(stateOne);
                    //TcpDateOne(stateOne, haveDate);
                }
                else
                {
                    stateOne.WorkSocket.BeginReceive(stateOne.Buffer, 0, stateOne.Buffer.Length, 0, new AsyncCallback(receiveCallback), stateOne);
                }
            }
            catch (Exception Ex)
            {
                int i = Ex.Message.IndexOf("远程主机强迫关闭了一个现有的连接");
                if (stateOne != null && i != -1)
                {
                    tcpStateList.Remove(stateOne);
                    LogHelper.Log.Info(" remove stateOne.... " + Ex.Message + Ex.StackTrace);
                }
            }
        }

        private void receiveParse(DeviceTcpState stateOne)
        {
            Device device = devHandler.getDevByIP(stateOne.ipEndPoint);
            try
            {
                //Log.Write(" rev:" + BitConverter.ToString(stateOne.Buffer));
                if (DeviceSignal.isHeadData(stateOne.Buffer, stateOne.iDataStartIndex))
                {
                    if (stateOne.iDataStartIndex + DeviceSignal.iHeadLength <= stateOne.Buffer.Length)
                    {
                        stateOne.iOneDataLength = DeviceSignal.getDataLength(stateOne.Buffer, stateOne.iDataStartIndex);
                        stateOne.abOneData = new byte[stateOne.iOneDataLength];

                        if (stateOne.Buffer.Length - stateOne.iDataStartIndex >= stateOne.iOneDataLength)
                        {
                            Array.Copy(stateOne.Buffer, stateOne.iDataStartIndex, stateOne.abOneData, 0, stateOne.iOneDataLength);
                            parseData(stateOne);
                        }
                        else
                        {
                            //stateOne.abAllData = new byte[stateOne.iDataLength];
                            //stateOne.Buffer.CopyTo(stateOne.abOneData, stateOne.iDataStartIndex);
                            Array.Copy(stateOne.Buffer, stateOne.iDataStartIndex, stateOne.abOneData,
                                stateOne.iOneDataIndex, stateOne.Buffer.Length - stateOne.iDataStartIndex);
                            stateOne.iOneDataIndex += stateOne.Buffer.Length - stateOne.iDataStartIndex;
                            stateOne.clearBuffer();
                        }
                    }
                    else
                    {
                        stateOne.abBakHeadData = new byte[DeviceSignal.iHeadLength];
                        //stateOne.Buffer.CopyTo(stateOne.abBakHeadData, stateOne.iDataStartIndex);
                        Array.Copy(stateOne.Buffer, stateOne.iDataStartIndex, stateOne.abBakHeadData,
                            0, stateOne.Buffer.Length - stateOne.iDataStartIndex);
                        stateOne.iBakHeadIndex = stateOne.Buffer.Length - stateOne.iDataStartIndex;
                        //stateOne.iOneDataIndex += stateOne.Buffer.Length - stateOne.iDataStartIndex;
                        stateOne.clearBuffer();
                    }
                }
                else
                {
                    if ((stateOne.iOneDataLength > 0) && (stateOne.iOneDataIndex > 0))
                    {
                        //stateOne.Buffer.CopyTo(stateOne.abOneData, stateOne.iOneDataIndex);

                        if (stateOne.Buffer.Length + stateOne.iOneDataIndex >= stateOne.iOneDataLength)
                        {
                            Array.Copy(stateOne.Buffer, stateOne.iDataStartIndex, stateOne.abOneData,
                                stateOne.iOneDataIndex, stateOne.iOneDataLength - stateOne.iOneDataIndex);
                            parseData(stateOne);
                        }
                        else
                        {
                            Array.Copy(stateOne.Buffer, stateOne.iDataStartIndex, stateOne.abOneData,
                                stateOne.iOneDataIndex, stateOne.Buffer.Length - stateOne.iDataStartIndex);
                            stateOne.iOneDataIndex += stateOne.Buffer.Length;
                            stateOne.clearBuffer();
                        }
                    }
                    else if (null != stateOne.abBakHeadData)
                    {
                        Array.Copy(stateOne.Buffer, stateOne.iDataStartIndex, stateOne.abBakHeadData, stateOne.iBakHeadIndex, DeviceSignal.iHeadLength - stateOne.iBakHeadIndex);
                        if (DeviceSignal.isHeadData(stateOne.abBakHeadData, 0))
                        {
                            stateOne.iOneDataLength = DeviceSignal.getDataLength(stateOne.abBakHeadData, 0);
                            stateOne.abOneData = new byte[stateOne.iOneDataLength];
                            stateOne.abBakHeadData.CopyTo(stateOne.abOneData, 0);

                            if (stateOne.Buffer.Length + stateOne.iBakHeadIndex >= stateOne.iOneDataLength)
                            {
                                Array.Copy(stateOne.Buffer, stateOne.iDataStartIndex, stateOne.abOneData, stateOne.iBakHeadIndex,
                                    stateOne.iOneDataLength - stateOne.iBakHeadIndex);
                                parseData(stateOne);
                            }
                            else
                            {
                                Array.Clear(stateOne.abBakHeadData, 0, stateOne.abBakHeadData.Length);
                                stateOne.abBakHeadData = null;
                                Array.Copy(stateOne.Buffer, stateOne.iDataStartIndex, stateOne.abOneData, stateOne.iBakHeadIndex, stateOne.Buffer.Length);
                                stateOne.iOneDataIndex = stateOne.Buffer.Length + stateOne.iBakHeadIndex;
                                stateOne.clearBuffer();
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                stateOne.clearBuffer();
                LogHelper.Log.Info("[ERROR] " + Ex.Message + Ex.StackTrace);
            }
        }

        private void parseData(DeviceTcpState stateOne)
        {
            int iHeadIndex = 0;

            //parseBytes(stateOne.ipEndPoint, stateOne.abOneData);
            devHandler.parseBytes(stateOne.ipEndPoint, stateOne.abOneData);
            stateOne.clearData();

            stateOne.iDataStartIndex += stateOne.iOneDataLength - stateOne.iOneDataIndex;
            stateOne.iOneDataLength = 0;
            stateOne.iOneDataIndex = 0;
            iHeadIndex = DeviceSignal.haveHead(stateOne.Buffer, stateOne.iDataStartIndex);
            if (iHeadIndex != -1)
            {
                if (iHeadIndex + DeviceSignal.iHeadLength > stateOne.Buffer.Length)
                {
                    stateOne.abBakHeadData = new byte[DeviceSignal.iHeadLength];
                    Array.Copy(stateOne.Buffer, iHeadIndex, stateOne.abBakHeadData,
                        0, stateOne.Buffer.Length - iHeadIndex);
                    stateOne.iBakHeadIndex = stateOne.Buffer.Length - iHeadIndex;
                    stateOne.clearBuffer();
                }
                else
                {
                    stateOne.iDataStartIndex = iHeadIndex;
                    receiveParse(stateOne);
                }
            }
            else
            {
                stateOne.clearBuffer();
            }
        }
        #endregion
    }
}
