using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using CommonUtils.Logger;

namespace FigKeyLoggerConfigurator.ClientSocket
{
    public class SocketClient
    {
        public Socket socketClient { get; set; }

        public void StartSocket()
        {
            //创建实例
            socketClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint point = new IPEndPoint(ip, 10050);
            //进行连接
            socketClient.Connect(point);

            //不停的接收服务器端发送的消息
            Thread thread = new Thread(ReciveMsg);
            thread.IsBackground = true;
            thread.Start();
        }

        /// <summary>
        /// 接收消息
        /// </summary>
        /// <param name="o"></param>
        private void ReciveMsg()
        {
            while (true)
            {
                //获取发送过来的消息
                byte[] buffer = new byte[50];
                var effective = socketClient.Receive(buffer);
                if (effective == 0)
                {
                    break;
                }
                var str = Encoding.ASCII.GetString(buffer, 0, effective);
                LogHelper.Log.Info("收到服务消息：" + str);
            }
        }

        public void Send()
        {
            string clientID = "2019sfjkadlk1234 ";
            clientID += "this is client request!";

            var buffter = Encoding.ASCII.GetBytes(clientID);
            socketClient.Send(buffter);
        }
    }
   
}
