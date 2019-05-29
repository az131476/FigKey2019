using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using CommonUtils.Logger;

namespace FigKeyLoggerServer.SanNiuSignal
{
    class DeviceTcpState
    {
        public int iBakHeadIndex;
        public int iOneDataIndex;//命令已到达长度
        public int iOneDataLength;//命令长度
        public int iDataStartIndex;//命令在buffer起始位置
        public byte[] abOneData;
        public byte[] abBakHeadData;
        public IPEndPoint ipEndPoint;

        internal Socket WorkSocket { get; set; }

        internal byte[] Buffer { get; set; }

        internal DeviceTcpState(Socket socket, int bufferSize)
        {
            this.Buffer = new byte[bufferSize];
            this.WorkSocket = socket;

            try
            {
                ipEndPoint = (IPEndPoint)socket.RemoteEndPoint;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error("!!!!!!!!!构造TcpState失败：" + ex.Message + "-----------------------");
            }
        }

        internal void clearBuffer()
        {
            Array.Clear(Buffer, 0, Buffer.Length);
            iDataStartIndex = 0;
        }

        internal void clearData()
        {
            if (null != abOneData)
            {
                Array.Clear(abOneData, 0, abOneData.Length);
                abOneData = null;
            }
            if (null != abBakHeadData)
            {
                Array.Clear(abBakHeadData, 0, abBakHeadData.Length);
                abBakHeadData = null;
            }
        }
    }
}
