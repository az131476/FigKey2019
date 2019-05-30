using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Command;
using SuperSocketServer.AppBase;
using CommonUtils.Logger;

namespace SuperSocketServer.Commands
{
   public  class Decrypt : CommandBase<MySession, MyRequestInfo>
    {
        private int Action = 3;
        public override string Name
        {
            get { return Action.ToString(); }
        }
        /// <summary>
        /// 上行（来自客户端的信息）
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
        {
            Console.WriteLine("解密命令被执行");
            Console.WriteLine("内容是" + requestInfo.Body);
            LogHelper.Log.Info(session.OrgCode + " " + requestInfo.Body);
            Push(session, "已收到解密信息");
        }

        /// <summary>
        ///  下行(推送)
        /// </summary>
        public void Push(MySession session, string text)
        {
            var response = BitConverter.GetBytes((ushort)Action).Reverse().ToList();
            var arr = System.Text.Encoding.UTF8.GetBytes(text);
            response.AddRange(BitConverter.GetBytes((ushort)arr.Length).Reverse().ToArray());
            response.AddRange(arr);

            session.Send(response.ToArray(), 0, response.Count);
        }
    }
}
