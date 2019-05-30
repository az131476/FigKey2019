using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.Facility.Protocol;

namespace SuperSocketServer.AppBase
{
  public   class MyReceiveFilter:FixedHeaderReceiveFilter<MyRequestInfo>
    {
        /// +-------+---+-------------------------------+
        /// |request| l |                               |
        /// | name  | e |    request body               |
        /// |  (2)  | n |                               |
        /// |       |(2)|                               |
        /// +-------+---+-------------------------------+
        public MyReceiveFilter() : base(4)
        {
            //00 02 00 06 01 ...   功能:00 02 字节数:00 06 数据:01 02 03 04 05 06
        }

        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            return (int)header[offset + 2] * 256 + (int)header[offset + 3];
        }

        protected override MyRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            var body = bodyBuffer.Skip(offset).Take(length).ToArray();
            return new MyRequestInfo(header.Array, body);
        }
    }
}
