using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Command;
using SuperSocket.SocketBase.Protocol;
using FigKeyLoggerServer.SocketServerFig;

namespace FigKeyLoggerServer.SuperSocketServer
{
    class ProcessBinaryMassage
    {
        #region 在独立类中处理不同请求
        ///同时你要移除请求处理方法的注册，因为它和命令不能同时被支持：
        ///appServer.NewRequestReceived += new RequestHandler<AppSession, StringRequestInfo>(appServer_NewRequestReceived);
        /// <summary>
        /// 可以定义一个名为"ADD"的类去处理Key为"ADD"的请求:
        /// </summary>
        public class ADD : CommandBase<BinaryAppSession, BinaryRequestInfo>
        {
            public override void ExecuteCommand(BinaryAppSession session, BinaryRequestInfo requestInfo)
            {
                session.Send(requestInfo.Body.Select(p => Convert.ToInt32(p)).Sum().ToString());
            }
        }
        /// <summary>
        /// 定义一个名为"MULT"的类去处理Key为"MULT"的请求:
        /// </summary>
        public class MULT : CommandBase<AppSession, StringRequestInfo>
        {
            public override void ExecuteCommand(AppSession session, StringRequestInfo requestInfo)
            {
                var result = 1;

                foreach (var factor in requestInfo.Parameters.Select(p => Convert.ToInt32(p)))
                {
                    result *= factor;
                }

                session.Send(result.ToString());
            }
        }
        #endregion
    }
}
