using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using System.Net;

namespace FigKeyLoggerServer.SocketServerFig
{
    class BinaryReceiveFactory:IReceiveFilterFactory<BinaryRequestInfo>
    {
        public IReceiveFilter<BinaryRequestInfo> CreateFilter(IAppServer appServer, IAppSession appSession, IPEndPoint remoteEndPoint)
        {
            BinaryReceiveFilter binaryReceiveFilter = new BinaryReceiveFilter();
            return binaryReceiveFilter;
        }
    }
}
