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
  public   class Step : CommandBase<MySession, MyRequestInfo>
    {
        private int Action = 20000;
        public override string Name
        {
            get { return Action.ToString(); }
        }

        /// <summary>
        /// 上行
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
        {
            Console.WriteLine("切换面板的命令被执行");
            if (requestInfo.Data.Length > 5)
            {
                var time = string.Join(" ", requestInfo.Data);
                LogHelper.Log.Info(session.OrgCode + "切换面板");
                Push(session, "服务器已经收到你切换步骤的命令");
            }
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
