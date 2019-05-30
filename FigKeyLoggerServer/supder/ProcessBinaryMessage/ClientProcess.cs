using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using FigKeyLoggerServer.SocketServerFig;
using CommonUtils.Logger;

namespace FigKeyLoggerServer.SuperSocketServer.ProcessBinaryMessage
{
    public class ClientProcess : CommandBase<BinaryAppSession, BinaryRequestInfo>
    {
        public override void ExecuteCommand(BinaryAppSession session, BinaryRequestInfo requestInfo)
        {
            //LogHelper.Log.Info(" 接收到客户端消息内容：" + requestInfo.Key + " len:" + requestInfo.Body);
            //session.Send(requestInfo.Body);
            //LogHelper.Log.Info(" 发送给客户端的消息：" + requestInfo.Body);

            StringBuilder sb = new StringBuilder();
            Array.ForEach(requestInfo.Body, b => sb.Append($"{b} "));
            LogHelper.Log.Info($"接收到客户端 {session.Config.Ip}:{session.Config.Port} 的数据：");
            LogHelper.Log.Info($"字节数组形式：{sb.ToString()}");
            LogHelper.Log.Info($" ASCII码转换：{Encoding.ASCII.GetString(requestInfo.Body)}");
        }
    }

}
