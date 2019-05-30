using SuperSocket.SocketBase;

namespace SuperSocketServer.AppBase
{
   public class MySession:AppSession<MySession,MyRequestInfo>
    {
        public MySession()
        {

        }

        public string TdpjNo { get; set; }
        public string CaId { get; set; }
        public string OrgCode { get; set; }
    }
}
