using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using FigKeyLoggerServer.Model;
using CommonUtils.FigKeyCommand;

namespace FigKeyLoggerServer.SanNiuSignal
{
    class ClientHandler
    {
        private List<Client> clnList;
        private FigKeySocketServer socketServer;
        private DeviceHandler devHandler;

        public ClientHandler(List<Client> clnList,FigKeySocketServer socketServer)
        {
            this.clnList = clnList;
            this.socketServer = socketServer;
            devHandler = socketServer.devHandler;
        }

        #region 处理字符串消息
        /// <summary>
        /// 处理字符串消息
        /// </summary>
        /// <param name="ipEndPoint"></param>
        /// <param name="revMsg"></param>
        public void ParseString(IPEndPoint ipEndPoint,string revMsg)
        {
            string[] strRev = revMsg.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;
            string strRet = "";
            Client client = getClientByIP(ipEndPoint);

            if (strRev[0] != ClientCommand.CON_SERVER)
            {
                if (null == client)
                {
                    socketServer.SendMsg(strRev[0] + "|0|no match client", client.ipEndPoint);
                    return;
                }
                else
                {
                    client.DeviceId = strRev[1];
                    strRet = strRev[0] + "|" + client.DeviceId + "|";
                }
            }
            ///默认仪器 连接仪器时 增加判断仪器ID
            ///当收到客户端对仪器参数修改时，仪器更新参数成功后，继续发送数据
            ///TODO:: 设置命令下发失败后应该还原内存原始值
            switch (strRev[0])
            {
                case ClientCommand.CON_SERVER:
                    if (strRev[2].Equals(ClientCommand.ClientConfirmStr))
                    {
                        if (null == client)
                        {
                            client = new Client();
                            client.ipEndPoint = ipEndPoint;
                            client.DeviceId = strRev[1];
                            clnList.Add(client);
                        }
                        else
                        {
                            client.DeviceId = strRev[1];
                        }
                        socketServer.SendMsg(strRev[0] + "|" + client.DeviceId + "|1", client.ipEndPoint);
                        return;
                    }
                    return;

                case ClientCommand.DEVICE_STATUS_ALL:
                    //获取所有设备的状态
                    string strStates = strRet + "1" + devHandler.getDevStates();
                    socketServer.SendMsg(strStates, client.ipEndPoint);
                    break;
                case ClientCommand.DEVICE_STATUS:
                    //查询当前设备状态

                    break;
            }
            //devProc(client, strRev, strRet);
        }
        #endregion

        #region 发送消息
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="strDevID"></param>
        /// <param name="strMsg"></param>
        /// <returns></returns>
        public int sendDevStatus2Client(string strDevID, string strMsg)
        {
            for (int i = 0; i < clnList.Count; i++)
            {
                if (clnList[i].DeviceId.Equals(strDevID))
                {
                    socketServer.SendMsg(strMsg, clnList[i].ipEndPoint);
                }
            }
            return 1;
        }
        #endregion

        #region 处理bytes
        /// <summary>
        /// 处理byte消息
        /// </summary>
        /// <param name="ipEndPoint"></param>
        /// <param name="revByte"></param>
        public void ParseByte(IPEndPoint ipEndPoint,byte[] revByte)
        {
            int libIndex = 0;
            int i = 0;
            string strRet = "";
            string[] strRes = new string[1];

            string head = Encoding.ASCII.GetString(revByte, 0, 3);
            if (head.Equals("*24"))
            {
                for (i = 4; i < revByte.Length; i++)
                {
                    if ('|' == revByte[i])
                    {
                        libIndex = i;
                        break;
                    }
                }
            }
            string deviceID = Encoding.ASCII.GetString(revByte, 4, libIndex - 4);

            Client client = getClientByIP(ipEndPoint);
            if (null == client)
            {
                socketServer.SendMsg(head + "|0|no match client", client.ipEndPoint);
                return;
            }
            else
            {
                client.DeviceId = deviceID;
                strRet = head + "|" + client.DeviceId + "|";
            }
            Device device = devHandler.getDevByID(client.DeviceId);
            if (null == device)
            {
                socketServer.SendMsg(strRet + "0|no match device", client.ipEndPoint);
                return;
            }
            strRes[0] = head;
            //devHandler.setUpdateLib(client.strSelDevID, bytes, libIndex + 1, strRet);
            //setDevProc(client, strRes, strRet);
        }
        #endregion

        #region 查找设备
        /// <summary>
        /// ipendpoint查找设备
        /// </summary>
        /// <param name="ipEndPoint"></param>
        /// <returns></returns>
        internal Client getClientByIP(IPEndPoint ipEndPoint)
        {
            try
            {
                return clnList.Find(delegate (Client client) { return client.ipEndPoint.Equals(ipEndPoint); });
            }
            catch { return null; }
        }
        #endregion
    }
}
