using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Config;
using System.Configuration;
using CommonUtils.Logger;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocketServer.AppBase;
using FigKeyLoggerServer.Model;

namespace FigKeyLoggerServer.SuperSocketServer
{
    public class SocketServerControl
    {
        private static MyServer appServer;
        private static SuperSocketConfig superConfig;
        public static List<Client> clientList;
        public static List<Device> deviceList;

        public SocketServerControl()
        {
            appServer = new MyServer();
            superConfig = new SuperSocketConfig();
            clientList = new List<Client>();
            deviceList = new List<Device>();
        }

        /// <summary>
        /// 启动socket 监听
        /// </summary>
        public static void StartServer()
        {
            //Setup the appServer
            if (!appServer.Setup(superConfig.TcpPort)) //Setup with listening port
            {
                LogHelper.Log.Info("Failed to setup!");
                return;
            }

            //Try to start the appServer
            if (!appServer.Start())
            {
                LogHelper.Log.Info("Failed to start!");
                return;
            }
            appServer.NewSessionConnected += MyServer_NewSessionConnected;
            appServer.SessionClosed += MyServer_SessionClosed;

            LogHelper.Log.Info("SuperSocket Server Start Success.....");
        }

        private static void MyServer_NewSessionConnected(MySession session)
        {
            var count = session.AppServer.SessionCount;
            LogHelper.Log.Info($"客户端连接进入，连接数量 {count} SessionID:{session.SessionID} RemoteEndPoint: {session.RemoteEndPoint} canID:{session.CaId}");
            //客户端连接加入列表
            Client client = new Client();
            client.SessionId = session.SessionID;
            client.ipEndPoint = session.RemoteEndPoint;
            clientList.Add(client);
        }

        private static void MyServer_SessionClosed(MySession session, CloseReason closeReason)
        {
            var count = session.AppServer.SessionCount;
            LogHelper.Log.Info($"服务端 失去 来自客户端的连接,sessionID:" + session.SessionID + " closeReason:" + closeReason.ToString() + " count:" + count);
            //客户端失去连接，删除client
            var client = clientList.Find(obj => obj.SessionId == session.SessionID);
            clientList.Remove(client);
        }

        public static void StopServer()
        {
            appServer.Stop();
        }
    }
}
