using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase.Protocol;

namespace FigKeyLoggerServer.SocketServerFig
{
    public class MyRequestInfo:IRequestInfo
    {
        public string Key { get; set; }

        public string DeviceId { get; set; }
    }
}
