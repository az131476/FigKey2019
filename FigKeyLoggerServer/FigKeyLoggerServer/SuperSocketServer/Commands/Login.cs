using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Command;
using SuperSocketServer.AppBase;
using CommonUtils.Logger;
using SuperSocketServer.Commands.BaseService;
using FigKeyLoggerServer.SuperSocketServer;
using FigKeyLoggerServer.Model;

namespace SuperSocketServer.Commands
{
  public  class Login : CommandBase<MySession, MyRequestInfo>
    {
        private const SingleBidDoc Action = SingleBidDoc.Login;
        public override string Name => ((int)Action).ToString();

        /// <summary>
        /// 上行（来自客户端的信息）
        /// </summary>
        /// <param name="session"></param>
        /// <param name="requestInfo"></param>
        public override void ExecuteCommand(MySession session, MyRequestInfo requestInfo)
        {
            //添加判断登陆是否可以成功的逻辑
            var body = requestInfo.Body;
            session.OrgCode = body;
            LogHelper.Log.Info($"收到 {session.SessionID} 用户请求查询设备信息");
            LogHelper.Log.Info("详细内容："+requestInfo.Body);
            //查询设备信息，发送给该用户
            Push(session,1);
        }

        /// <summary>
        ///  返回客户端信息：命令号+返回结果，0-失败，1-成功
        /// </summary>
        public void Push(MySession session, byte status)
        {
            //返回客户查询结果：
            //返回完整内容：命令代码+数据长度+内容
            //若未查询到内容：命令代码+0
            var response = new byte[] { 0, 1, 1, 0, 79, 75 }; //OK
            session.Send(response, 0, response.Length);
        }

        private void SelectAllDevice()
        {
            byte[] buffer = new byte[8];
            foreach (var device in SocketServerControl.deviceList)
            {
                string devid = device.DeviceId;
            }
        }
    }
}
