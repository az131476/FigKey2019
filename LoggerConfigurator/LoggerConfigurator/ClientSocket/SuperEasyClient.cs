using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using SuperSocket.ClientEngine;
using SuperSocketClient.AppBase;
using CommonUtils.ByteHelper;
using CommonUtils.Logger;

namespace LoggerConfigurator.ClientSocket
{
    public class SuperEasyClient
    {
        private static EasyClient<MyPackageInfo> client;

        /// <summary>
        /// 连接服务器
        /// </summary>
        public static async void ConnectServer()
        {
            //if (client != null || !client.IsConnected)
            //    return;
            client = new EasyClient<MyPackageInfo>();

            LogHelper.Log.Info("开始连接服务...");
            client = new EasyClient<MyPackageInfo>();
            client.Initialize(new MyReceiveFilter());
            client.Connected += OnClientConnected;
            client.NewPackageReceived += OnPagckageReceived;
            client.Error += OnClientError;
            client.Closed += OnClientClosed;

            var webSocketUrl = System.Configuration.ConfigurationManager.AppSettings["WebSocketURL"];//ip
            var webSocketPort = System.Configuration.ConfigurationManager.AppSettings["WebSocketPort"];//port
            var connected =
                await client.ConnectAsync(new IPEndPoint(IPAddress.Parse(webSocketUrl), int.Parse(webSocketPort)));
        }

        private static void OnClientClosed(object sender, EventArgs e)
        {
            int attmpts = 5;
            do
            {
                LogHelper.Log.Info("已断开与服务的连接...");
                LogHelper.Log.Info("等待5秒中后重新连接...");
                Thread.Sleep(5000);
                ConnectServer();
                attmpts--;
            } while (!client.IsConnected && attmpts > 0);
        }

        private static void OnClientError(object sender, ErrorEventArgs e)
        {
            LogHelper.Log.Info("客户端错误：" + e.Exception.Message);
        }

        private static void OnPagckageReceived(object sender, PackageEventArgs<MyPackageInfo> e)
        {
            LogHelper.Log.Info($"收到文本下行:{e.Package.Body}");
            LogHelper.Log.Info("收到服务消息【Byte】:"+"head:"+BitConverter.ToString(e.Package.Header)+" body:"+BitConverter.ToString(e.Package.Data));
        }

        private static void OnClientConnected(object sender, EventArgs e)
        {
            LogHelper.Log.Info("已连接到服务器...");
        }

        public static void btnSendMsg(string msg)
        {
            SendMessage(SingleBidDoc.Announce, msg);
        }

        public static void btnLogin(string loginMsg)
        {
            SendMessage(SingleBidDoc.Login, loginMsg);

        }

        private static void btnDecrypt(string decrypt)
        {
            SendMessage(SingleBidDoc.DecSendTime,decrypt);
        }

        private static void btnAnnounce(string announce)
        {
            SendMessage(SingleBidDoc.Announce, announce);
        }

        private static void btnSignature(string signature)
        {
            SendMessage(SingleBidDoc.Signature, signature);
        }

        private static void btnSwitchPanel(string switchpanel)
        {
            SendMessage(SingleBidDoc.Dissent,switchpanel);
        }

        /// <summary>
        /// 发送命令和消息到服务器
        /// </summary>
        /// <param name="command"></param>
        /// <param name="message"></param>
        private static void SendMessage(SingleBidDoc command, string message)
        {
            if (client == null || !client.IsConnected || message.Length <= 0)
                return;
            var response = BitConverter.GetBytes((ushort)command).Reverse().ToList();
            var arr = System.Text.Encoding.UTF8.GetBytes(message);
            response.AddRange(BitConverter.GetBytes((ushort)arr.Length).Reverse().ToArray());
            response.AddRange(arr);
            client.Send(response.ToArray());

            LogHelper.Log.Info($"发送{command.GetDescription()}数据：" + message+" byte:"+BitConverter.ToString(response.ToArray()));
        }
    }
}
