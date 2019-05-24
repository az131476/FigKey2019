using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace FigKeyLoggerServer.Model
{
    public class Device
    {
        public object objLocAcp { get;}

        /// <summary>
        /// 设备状态枚举
        /// </summary>
        public enum DEVICE_STATE_ENUM
        {
            STATE_STOP = 1,

            STATE_RUNNING = 2,

            STATE_PREPARATION = 3,

            STATE_DISCONNECT = 4,

            STATE_BUTT
        }

        /// <summary>
        /// 与设备的协议
        /// </summary>
        public enum DeviceSession
        {
            heart_beat = 1,
            select_direct = 2,
            downLoad_file = 3,
            upLoad_file = 4
        }

        #region device 参数
        /// <summary>
        /// 设备ID
        /// </summary>
        public string DeviceId { get; set; }

        public IPEndPoint DeviceEndPoint { get; set; }

        /// <summary>
        /// 设备连接数
        /// </summary>
        public int DeviceConNum { get; set; }

        /// <summary>
        /// 设备状态，0-离线，1-在线
        /// </summary>
        public int DeviceState { get; set; }

        /// <summary>
        /// 设备上的文件名
        /// </summary>
        public string DFileName { get; set; }

        /// <summary>
        /// 设备上的文件名大小
        /// </summary>
        public string DFileSize { get; set; }
        #endregion

        public Device()
        {
            objLocAcp = new object();
        }
    }
}
