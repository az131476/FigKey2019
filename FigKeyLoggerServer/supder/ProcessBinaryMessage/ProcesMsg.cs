using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonUtils.Logger;
using SuperSocket.SocketBase.Protocol;
using FigKeyLoggerServer.SocketServerFig;

namespace FigKeyLoggerServer.SuperSocketServer.ProcessBinaryMessage
{
    public class ProcesMsg
    {
        public static void ProcessMsg(BinaryAppSession session, BinaryRequestInfo requestInfo)
        {
            //LogHelper.Log.Info(" 接收到客户端消息内容：" + requestInfo.Key+" len:"+requestInfo.Body);
            //session.Send(requestInfo.Body);
            //LogHelper.Log.Info(" 发送给客户端的消息：" + requestInfo.Body);

            //StringBuilder sb = new StringBuilder();
            //Array.ForEach(requestInfo.Body, b => sb.Append($"{b} "));

            LogHelper.Log.Info($" ASCII码转换：{Encoding.ASCII.GetString(requestInfo.Body)}");
            byte[] msg = requestInfo.Body;
            string clientID = Encoding.ASCII.GetString(msg,0,16);
            string[] revMsg = BitConverter.ToString(msg).Split(new char[] { ' '},StringSplitOptions.RemoveEmptyEntries);
            switch (clientID)
            {
                case "ECH":
                    LogHelper.Log.Info("ECH Revice:"+BitConverter.ToString(msg));
                    break;
            }
        }
    }
}
