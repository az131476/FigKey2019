using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XcpDll
{
    public  class PropertyClass
    {
        /// <summary>
        /// 波特率
        /// </summary>
        public UInt32 baudrate;

        /// <summary>
        /// masterCanid
        /// </summary>
        public UInt32 MasterCANID;

        /// <summary>
        /// slave canid
        /// </summary>
        public UInt32 SlaverCANID;

        /// <summary>
        /// 大小端序
        /// </summary>
        public string byteOrder;

        /// <summary>
        /// eventChannel_sync
        /// </summary>
        public UInt16 eventChannel_sync;

        /// <summary>
        /// eventChannel_10
        /// </summary>
        public UInt16 eventChannel_10;

        /// <summary>
        /// eventChannel_100
        /// </summary>
        public UInt16 eventChannel_100;
    }
}
