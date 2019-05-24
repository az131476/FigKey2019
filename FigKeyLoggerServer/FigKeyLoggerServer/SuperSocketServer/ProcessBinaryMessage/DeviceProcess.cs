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
    class DeviceProcess:CommandBase<BinaryAppSession,BinaryRequestInfo>
    {
        public override void ExecuteCommand(BinaryAppSession session, BinaryRequestInfo requestInfo)
        {
            
        }
    }
}
