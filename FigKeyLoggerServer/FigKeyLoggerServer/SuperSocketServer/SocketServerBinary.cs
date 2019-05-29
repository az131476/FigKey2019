using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FigKeyLoggerServer.SocketServerFig;
using SuperSocket.SocketBase.Config;
using System.Configuration;
using CommonUtils.Logger;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using FigKeyLoggerServer.SuperSocketServer.ProcessBinaryMessage;

namespace FigKeyLoggerServer.SuperSocketServer
{
    public class SocketServerBinary
    {
        private BinaryAppServer appServer;
        private BinaryAppSession appSession;
        private SuperSocketConfig superConfig;
        private BinaryReceiveFactory revFactory;

        public SocketServerBinary()
        {
            revFactory = new BinaryReceiveFactory();
            appServer = new BinaryAppServer(revFactory);
            appSession = new BinaryAppSession();

            superConfig = new SuperSocketConfig();
        }
        /// <summary>
        /// 启动socket 监听
        /// </summary>
        public void StartSocketListen()
        {
            if (appServer != null)
            {
                //设置端口
                var serverConfig = superConfig.SocketServerConfig();
                if (!appServer.Setup(serverConfig))
                {
                    LogHelper.Log.Info("Failed To Setup port...");
                    return;
                }
                //启动服务
                if (!appServer.Start())
                {
                    LogHelper.Log.Info("Failed To Start appserver...");
                    return;
                }
                appServer.NewSessionConnected += AppServer_NewSessionConnected;
                appServer.NewRequestReceived += AppServer_NewRequestReceived;
                appServer.SessionClosed += AppServer_SessionClosed;

                LogHelper.Log.Info("SuperSocket Server Start Success.....");
            }
        }

        #region server listen

        private void AppServer_NewRequestReceived(BinaryAppSession session,BinaryRequestInfo requestInfo)
        {
            ProcesMsg.ProcessMsg(session,requestInfo);
        }
        /// <summary>
        /// 监听客户端断开连接
        /// </summary>
        /// <param name="session"></param>
        /// <param name="value"></param>
        private void AppServer_SessionClosed(BinaryAppSession session, SuperSocket.SocketBase.CloseReason closeReason)
        {
            var count = appServer.GetAllSessions().Count();
            LogHelper.Log.Info($"服务端 失去 来自客户端的连接,sessionID:" + session.SessionID + " closeReason:" + closeReason.ToString()+" count:"+count);
        }

        /// <summary>
        /// 监听客户端连接
        /// </summary>
        /// <param name="session"></param>
        private void AppServer_NewSessionConnected(BinaryAppSession session)
        {
            var count = appServer.GetAllSessions().Count();
            string sessionId = session.SessionID;
            LogHelper.Log.Info("客户端连接进入，连接数量 " + count+ " SessionID:"+session.SessionID + " LocalEndPoint:" + session.LocalEndPoint+ " RemoteEndPoint:" + session.RemoteEndPoint);
            session.Send("Welcome to SuperSocket Telnet Server");
        }
        #endregion

        public void AppServerStop()
        {
            appServer.Stop();
        }
    }
}
