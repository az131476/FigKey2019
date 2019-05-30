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
using SuperSocketServer.AppBase;

namespace FigKeyLoggerServer.SuperSocketServer
{
    public class SocketServerBinary
    {
        private MyServer appServer;
        private SuperSocketConfig superConfig;

        public SocketServerBinary()
        {
            appServer = new MyServer();
            superConfig = new SuperSocketConfig();
        }
        /// <summary>
        /// 启动socket 监听
        /// </summary>
        public void StartSocketListen()
        {
            var appServer = new MyServer();
            //Setup the appServer
            if (!appServer.Setup(10050)) //Setup with listening port
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
            LogHelper.Log.Info("SuperSocket Server Start Success.....");
        }

        public void AppServerStop()
        {
            appServer.Stop();
        }
    }
}
