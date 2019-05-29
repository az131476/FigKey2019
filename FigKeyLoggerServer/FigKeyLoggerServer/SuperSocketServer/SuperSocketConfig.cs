using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using SuperSocket.SocketBase.Config;
using CommonUtils.Logger;

namespace FigKeyLoggerServer.SuperSocketServer
{
    class SuperSocketConfig
    {
        public int TcpPort { get; set; }
        public SuperSocketConfig()
        {
            try
            {
                string sport = ConfigurationManager.AppSettings["serverPort"].ToString();
                if (!string.IsNullOrEmpty(sport))
                {
                    TcpPort = int.Parse(sport);
                    SocketServerConfig();
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex.Message + " " + Diagnostis.GetLineNum());
            }
        }

        /// <summary>
        /// 设置supersocket config's port
        /// </summary>
        /// <returns></returns>
        public ServerConfig SocketServerConfig()
        {
            var serverConfig = new ServerConfig();
            serverConfig.Port = TcpPort;
            return serverConfig;
        }
    }
}
