using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BXHSerialPort
{
    class TcpConnectionClient
    {
        private Socket socket = null;
        public bool Init(string ip, string point)
        {
            if (socket == null)
            {
                try
                {
                    socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress ipAddress = IPAddress.Parse(ip);
                    IPEndPoint ipEndPoint = new IPEndPoint(ipAddress, Convert.ToInt32(point));
                    socket.Connect(ipEndPoint);
                    socket.ReceiveTimeout = 2000;
                }
                catch (Exception)
                {
                    socket = null;
                    return false;
                }
            }
            return true;
        }
        public void Send(byte[] buffer)
        {
            socket.Send(buffer);
        }
        public int Receive(byte[] buffer)
        {
            byte[] exceptionBuffer = {1 };
            try
            {
                int receiveCount = socket.Receive(buffer);
                return receiveCount;
            }
            catch (SocketException e)
            {
                if (e.ErrorCode == 10060)
                    // 超时的时候错误号码是10060
                    return 0;
                return 0;
            }
        }
        public void Close()
        {
            if (socket != null)
            {
                socket.Close();
            }
            socket = null;
        }
            
    }
}
