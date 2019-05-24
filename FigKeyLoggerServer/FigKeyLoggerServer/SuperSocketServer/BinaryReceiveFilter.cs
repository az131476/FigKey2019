using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace FigKeyLoggerServer.SocketServerFig
{
    class BinaryReceiveFilter:IReceiveFilter<BinaryRequestInfo>
    {

        public BinaryRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;
            byte[] byt = new byte[length];
            Array.Copy(readBuffer, offset, byt, 0, length);
            return new BinaryRequestInfo("test", byt);
        }

        public void Reset()
        {
            //throw new NotImplementedException();
        }

        public int LeftBufferSize
        {
            get { return 0; }
        }

        public IReceiveFilter<BinaryRequestInfo> NextReceiveFilter
        {
            get { return this; }
        }

        public FilterState State { get; private set; }
    }
}
