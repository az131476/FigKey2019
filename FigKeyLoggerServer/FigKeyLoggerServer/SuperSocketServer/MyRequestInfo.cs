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
        /// <summary>
        /// [不使用]
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 设备逻辑地址
        /// </summary>
        public string DeviceLogicalCode { get; set; }

        /// <summary>
        /// 命令序列号
        /// </summary>
        public string Seq { get; set; }

        /// <summary>
        /// 控制码
        /// </summary>
        public string ControlCode { get; set; }

        /// <summary>
        /// 数据长度
        /// </summary>
        public string Length { get; set; }

        /// <summary>
        /// 数据域
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// CS校验
        /// </summary>
        public string Cs { get; set; }

        /// <summary>
        /// 当前完整帧
        /// </summary>
        public string EntireFrame { get; set; }
    }
}
