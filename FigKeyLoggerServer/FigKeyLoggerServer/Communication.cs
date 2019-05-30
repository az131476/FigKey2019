using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using CommonUtils.Logger;
using System.ServiceProcess;
using System.ServiceModel;
using FigKeyLoggerWcf;
using System.IO;
using System.Configuration;
using FigKeyLoggerServer.Model;
using FigKeyLoggerServer.SuperSocketServer;
using SuperSocketServer.AppBase;

namespace FigKeyLoggerServer
{
    public partial class Communication : ServiceBase
    {
        public Communication()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 承载WCF的宿主程序
        /// </summary>
        private ServiceHost host;
        /// <summary>
        /// socket对象
        /// </summary>
        //private SocketServerBinary socketServer;
        /// <summary>
        /// 服务端口
        /// </summary>
        private int port { get; set; }

        private List<Device> deviceList;
        private List<Client> clientList;


        protected override void OnStart(string[] args)
        {
            InitConfig();
            LogHelper.Log.Info("on start...");
            host = new ServiceHost(typeof(FigKeyLoggerService));
            //socketServer = new SocketHelper(port, deviceList, clientList);
            //socketServer = new FigKeySocketServer(figkeyConfig, deviceList, clientList);
            SocketServerControl.StartServer();


            host.Opening += Host_Opening;
            host.Opened += Host_Opened;
            host.Closing += Host_Closing;
            host.Closed += Host_Closed;
            host.Faulted += Host_Faulted;

            host.Open(TimeSpan.FromSeconds(25));
        }

        protected override void OnStop()
        {
            try
            {
                host.Close(TimeSpan.FromSeconds(25));
                SocketServerControl.StopServer();
                //LogHelper.Log.Info("Windows Communication Foundtion Is Stop " + host.State);
            }
            finally
            {
                host = null;
            }
        }

        #region host event
        private void Host_Faulted(object sender, EventArgs e)
        {
            LogHelper.Log.Info("Host is faulted");
        }

        private void Host_Closed(object sender, EventArgs e)
        {
            LogHelper.Log.Info("Host is closed");
        }

        private void Host_Closing(object sender, EventArgs e)
        {
            LogHelper.Log.Info("Host is closing");
        }

        private void Host_Opened(object sender, EventArgs e)
        {
            LogHelper.Log.Info("Host is opened");
        }

        private void Host_Opening(object sender, EventArgs e)
        {
            LogHelper.Log.Info("host is opening");
        }
        #endregion

        private void InitConfig()
        {
            deviceList = new List<Device>();
            clientList = new List<Client>();
            //figkeyConfig = new FigKeyConfig();
        }
    }
}
