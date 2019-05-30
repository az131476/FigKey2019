using System;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using CommonUtils.Logger;

namespace SuperSocketServer.AppBase
{
   public  class MyServer:AppServer<MySession,MyRequestInfo>
    {
       
        /// <summary>
        /// 通过配置文件安装服务从这里启动
        /// </summary>
        public MyServer()
            : base(new DefaultReceiveFilterFactory<MyReceiveFilter, MyRequestInfo>())
        {
           // this.NewRequestReceived += MyServerNewRequestReceived;
        }

        //private void MyServerNewRequestReceived(MySession session, MyRequestInfo requestinfo)
        //{
        //    Console.WriteLine("新的请求到达");
        //}


        protected override void OnStarted()
        {
            LogHelper.Log.Info("服务启动成功");
        }
    }
}
