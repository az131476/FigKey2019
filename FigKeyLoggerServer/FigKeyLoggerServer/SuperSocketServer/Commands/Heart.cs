using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Command;
using SuperSocketServer.AppBase;

namespace SuperSocketServer.Commands
{
    public class Heart : CommandBase<MySession, MyRequestInfo>
    {
        private int Action = 7;

        public override string Name
        {
            get
            {
                return Action.ToString();
            }
        }
        /// <summary>
        /// 上行（来自客户端的信息）
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
        {
            Console.WriteLine("收到心跳包");
            Push(session,"OK");
        }
        /// <summary>
        /// 下行
        /// </summary>
        /// <param name="session"></param>
        /// <param name="text"></param>
        public void Push(MySession session, string text)
        {
            var respone = BitConverter.GetBytes((ushort) Action).Reverse().ToList();
            var content = Encoding.UTF8.GetBytes(text);
            respone.AddRange(BitConverter.GetBytes((ushort)content.Length).Reverse().ToArray());
            respone.AddRange(content);

            session.Send(respone.ToArray(),0,respone.Count);
        }
    }
}
