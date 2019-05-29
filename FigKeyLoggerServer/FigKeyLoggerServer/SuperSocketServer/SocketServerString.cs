using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.SocketBase.Command;
using CommonUtils.Logger;
using FigKeyLoggerServer.Model;

namespace FigKeyLoggerServer.SocketServerFig
{
    public class SocketHelper
    {
        public AppServer appServer { get; set; }
        private int port;
        private List<Device> deviceList;
        private List<Client> clientList;

        public SocketHelper(int port,List<Device> devList,List<Client> clnList)
        {
            appServer = new AppServer();
            this.deviceList = devList;
            this.clientList = clnList;
            this.port = port;
        }
        public void StartServer()
        {
            //Setup the appServer
            if (!appServer.Setup(port)) //Setup with listening port
            {
                LogHelper.Log.Error("Failed to setup!");
                return;
            }
            LogHelper.Log.Info("[[socket server port setup successfull]] ,this is port:"+port);
            //Try to start the appServer
            if (!appServer.Start())
            {
                LogHelper.Log.Error("Failed to start!");
                return;
            }
            LogHelper.Log.Info("[[The socket server started successfully]]");

            //1.监听客户端连接
            appServer.NewSessionConnected += new SessionHandler<AppSession>(appServer_NewSessionConnected);
            //断开与客户端的连接
            appServer.SessionClosed += appServer_NewSessionClosed;

            //2.监听数据接收
            appServer.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(appServer_NewRequestReceived);
            //接收=》原始过滤=》协议解析=》命令路由并执行=》找不到命令则直接一分不动发给客户端
            //requestInfo.Key 是请求的命令行用空格分隔开的第一部分
            //requestInfo.Parameters 是用空格分隔开的其余部分
            ProcessStringMassage.ADD add = new ProcessStringMassage.ADD();
        }
        //1.
        private void appServer_NewSessionConnected(AppSession session)
        {
            LogHelper.Log.Info($"服务端得到来自客户端的连接成功");
            Device device = new Device();
            device.DeviceId = session.SessionID;
            device.DeviceEndPoint = session.RemoteEndPoint;
            deviceList.Add(device);

            var count = appServer.GetAllSessions().Count();
            string sessionId = session.SessionID;
            LogHelper.Log.Info("连接数量 " + count);
            session.Send("Welcome to SuperSocket Telnet Server");
        }

        private void appServer_NewSessionClosed(AppSession session, CloseReason aaa)
        {
            LogHelper.Log.Info($"服务端 失去 来自客户端的连接" + session.SessionID + aaa.ToString());
            var count = appServer.GetAllSessions().Count();
            LogHelper.Log.Info("连接数量 " + count);
        }
        private void appServer_NewRequestReceived(AppSession session, StringRequestInfo requestInfo)
        {
            LogHelper.Log.Info(" 接收到客户端消息内容S："+requestInfo.Key);
            session.Send(requestInfo.Body);
            LogHelper.Log.Info(" 发送给客户端的消息："+requestInfo.Body);

            switch (requestInfo.Key.ToUpper())
            {
                case ("ECHO"):
                    session.Send(requestInfo.Body);
                    break;
                case ("ADD"):
                    session.Send(requestInfo.Parameters.Select(p => Convert.ToInt32(p)).Sum().ToString());
                    break;
                case ("MULT"):
                    var result = 1;
                    foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
                    {
                        result *= factor;
                    }
                    session.Send(result.ToString());
                    break;
            }
        }
    }
}
