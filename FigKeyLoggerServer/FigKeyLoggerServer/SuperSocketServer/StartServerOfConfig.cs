using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketEngine;
using CommonUtils.Logger;

namespace SuperSocketServer
{
    public class SuperServer
    {
        private static IBootstrap bootstrap;
        public static void StartSuperServer()
        {
            //通过读取配置文件启动
            bootstrap = BootstrapFactory.CreateBootstrap();
            if (!bootstrap.Initialize())
            {
                LogHelper.Log.Info("初始化失败！");
                return;
            }

            LogHelper.Log.Info("启动中");

            var result = bootstrap.Start();
            foreach (var server in bootstrap.AppServers)
            {
                switch (server.State)
                {
                    case ServerState.Running:
                        LogHelper.Log.Info($"{server.Name} 运行中");
                        break;
                    case ServerState.NotInitialized:
                        break;
                    case ServerState.Initializing:
                        break;
                    case ServerState.NotStarted:
                        break;
                    case ServerState.Starting:
                        break;
                    case ServerState.Stopping:
                        break;
                    default:
                        LogHelper.Log.Info($"{server.Name} 启动失败");
                        break;
                }
            }

            switch (result)
            {
                case StartResult.Failed:
                    LogHelper.Log.Info("无法启动服务，更多错误信息请查看日志");
                    break;
                case StartResult.None:
                    LogHelper.Log.Info("没有服务器配置，请检查你的配置！");
                    break;
                case StartResult.PartialSuccess:
                    LogHelper.Log.Info("一些服务启动成功，但是还有一些启动失败，更多错误信息请查看日志");
                    break;
                case StartResult.Success:
                    LogHelper.Log.Info("服务已经开始！");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static void StopSuperServer()
        {
            bootstrap.Stop();

            LogHelper.Log.Info("The SuperSocket ServiceEndine has been stopped!");
        }
    }
}
