using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CANManager.Model
{
    public class DeviceInfo
    {
        public enum SelectDeviceType
        {
            NONE,
            USB_CAN_1,
            USB_CAN_2
        }
        public enum ProtocolType
        {
            /// <summary>
            /// Mongoose VPW/CAN, SCI/CAN and GM
            /// </summary>
            J1850VPW = 1,
            /// <summary>
            /// Mongoose PWM/CAN and PWMF
            /// </summary>
            J1850PWM = 2,
            /// <summary>
            /// Mongoose ISO/CAN and MFC
            /// </summary>
            ISO9141 = 3,
            /// <summary>
            /// Mongoose ISO/CAN and MFC
            /// </summary>
            ISO14230 = 4,
            /// <summary>
            /// Mongoose all
            /// </summary>
            CAN = 5,
            /// <summary>
            /// Mongoose All
            /// </summary>
            ISO15765 = 6,
            /// <summary>
            /// Mongoose SCI/CAN
            /// </summary>
            SCI_A_ENGINE = 7,
            /// <summary>
            /// Mongoose SCI/CAN
            /// </summary>
            SCI_A_TRANS = 8,
            /// <summary>
            /// Mongoose SCI/CAN
            /// </summary>
            SCI_B_ENGINE = 9,
            /// <summary>
            /// Mongoose SCI/CAN
            /// </summary>
            SCI_B_TRANS = 10
        }

        public enum DeviceStatusEnum
        {
            DEF,
            OPEN,
            CONNECTION,
            DISCONNECTION,
            CLOSE
        }

        public enum ModelType
        {
            NONE,
            MODEL1,
            MODEL2,
            MODEL3,
            MODEL4,
            MODEL5,
            MODEL6,
            MODEL7,
            MODEL8,
            MODEL9,
            MODELA
        }

        public SelectDeviceType SelectedDevice { get; set; }

        public ModelType ModelSidType { get; set; }

        /// <summary>
        /// 设备NAME
        /// </summary>
        public IntPtr DeviceName { get; set; }

        /// <summary>
        /// 设备ID
        /// </summary>
        public uint DeviceID { get; set; }

        public DeviceStatusEnum DeviceStatus { get; set; }

        /// <summary>
        /// 协议类型
        /// </summary>
        public ProtocolType ProtocolID { get; set; }

        /// <summary>
        /// Flags：协议特定选项由位字段定义。这个参数通常设置为零。
        /// </summary>
        public uint Flags { get; set; }

        /// <summary>
        /// 波特率
        /// </summary>
        public uint BaudRate { get; set; }

        /// <summary>
        /// 通道ID
        /// </summary>
        public uint ChannelID { get; set; }

        public StringBuilder TempBuffer { get; set; }
    }
}
