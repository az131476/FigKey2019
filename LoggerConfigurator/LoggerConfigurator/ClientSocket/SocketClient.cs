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
    class SocketClient
    {
        public Socket socketClient { get; set; }

        public void StartSocket()
        {
            //创建实例
            socketClient = new Socket(SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint point = new IPEndPoint(ip, 10020);
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
                byte[] buffer = new byte[1024 * 1024 * 2];
                var effective = socketClient.Receive(buffer);
                if (effective == 0)
                {
                    break;
                }
                var str = Encoding.UTF8.GetString(buffer, 0, effective);
                LogHelper.Log.Info("收到服务消息：" + str);
            }
        }

        private void Send()
        {
            int i = 0;
            int sum = 0;
            while (true)
            {
                i++;
                sum += i;
                var buffter = Encoding.UTF8.GetBytes($"ADD {sum} {sum + 1}" + "\r\n");
                var temp = socketClient.Send(buffter);
                Console.WriteLine(i);
                Thread.Sleep(1000);
            }
        }
    }
   
}
