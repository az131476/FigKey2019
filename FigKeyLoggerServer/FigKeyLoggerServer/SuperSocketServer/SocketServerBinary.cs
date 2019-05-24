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
        private ClientProcess clientProcess;
        private DeviceProcess deviceProcess;

        public SocketServerBinary()
        {
            revFactory = new BinaryReceiveFactory();
            appServer = new BinaryAppServer(revFactory);

            superConfig = new SuperSocketConfig();
        }
        /// <summary>
        /// 启动socket 监听
        /// </summary>
        public void StartSocketListen()
        {
            if (appServer != null)
            {
                var serverConfig = superConfig.SocketServerConfig();
                if (!appServer.Setup(serverConfig))
                {
                    LogHelper.log.Info("Failed To Setup port...");
                    return;
                }

                if (!appServer.Start())
                {
                    LogHelper.log.Info("Failed To Start appserver...");
                    return;
                }
                appServer.NewSessionConnected += AppServer_NewSessionConnected;
                //appServer.NewRequestReceived += AppServer_NewRequestReceived;
                appServer.SessionClosed += AppServer_SessionClosed;
                ///处理接收到的消息
                clientProcess = new ClientProcess();
                deviceProcess = new DeviceProcess();

                LogHelper.log.Info("SuperSocket Server Start Success.....");
            }
        }

        #region server listen
        private void AppServer_SessionClosed(BinaryAppSession session, SuperSocket.SocketBase.CloseReason value)
        {
            LogHelper.log.Info($"服务端 失去 来自客户端的连接" + session.SessionID + value.ToString());
            var count = appServer.GetAllSessions().Count();
            LogHelper.log.Info("连接数量 " + count);
        }

        private void AppServer_NewRequestReceived(BinaryAppSession session,BinaryRequestInfo requestInfo)
        {
            LogHelper.log.Info(" 接收到客户端消息内容：" + requestInfo.Key+" len:"+requestInfo.Body);
            //session.Send(requestInfo.Body);
            LogHelper.log.Info(" 发送给客户端的消息：" + requestInfo.Body);

            StringBuilder sb = new StringBuilder();
            Array.ForEach(requestInfo.Body, b => sb.Append($"{b} "));
            LogHelper.log.Info($"接收到客户端 {session.Config.Ip}:{session.Config.Port} 的数据：");
            LogHelper.log.Info($"字节数组形式：{sb.ToString()}");
            LogHelper.log.Info($" ASCII码转换：{Encoding.ASCII.GetString(requestInfo.Body)}");
        }

        private void AppServer_NewSessionConnected(BinaryAppSession session)
        {
            var count = appServer.GetAllSessions().Count();
            string sessionId = session.SessionID;
            LogHelper.log.Info("连接数量 " + count);
            session.Send("Welcome to SuperSocket Telnet Server");
        }
        #endregion

        public void AppServerStop()
        {
            appServer.Stop();
        }
    }
}
