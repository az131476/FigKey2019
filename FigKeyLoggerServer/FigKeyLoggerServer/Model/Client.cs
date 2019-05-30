using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace FigKeyLoggerServer.Model
{
    public class Client
    {

        public string SessionId { get; set; }

        /// <summary>
        /// 客户端地址
        /// </summary>
        public IPEndPoint ipEndPoint { get; set; } 

        /// <summary>
        /// 客户端状态，0-在线，1-离线
        /// </summary>
        public int State { get; set; }
    }
}
