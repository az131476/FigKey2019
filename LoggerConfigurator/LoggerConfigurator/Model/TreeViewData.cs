using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FigKeyLoggerConfigurator.Model
{
    public static class TreeViewData
    {
        #region 硬件
        /// <summary>
        /// 硬件
        /// </summary>
        public static class HardWare
        {
            public const string ROOT = "HardWare";
            public const string COMMENT = "内容";
            public const string SETTING = "设置";
            public const string CAN_CHANNELS = "CAN Channels";
            public const string LIN_CHANNELS = "LIN Channels";
            public const string FLEXRAY_CHANNELS = "FlexRay Channels";
            public const string MOST150_CHANNELS = "Most150 Channels";
            public const string ANALOG_INPUTS = "Analog Inputs";
            public const string DIGITAL_INPUTS = "Digital Inputs";
            public const string CAN_GPS = "CANgps";
            public const string MONITORING = "Monitoring";
            public const string WLAN_3G = "WLAN/3G";

            public const string CAN_CHILD = "CAN ";
            public const string CAN_1_DATA = "CAN1数据";
            public const string CAN_2_DATA = "CAN2数据";
            public const string CAN_HARDWARE_CONFIG = "硬件配置";
        }
        #endregion

        #region 一般
        /// <summary>
        /// 一般
        /// </summary>
        public static class General
        {
            public const string ROOT = "General";
            public const string DATABASE = "Databases";
            public const string SPECIAL_FEATURES = "Special Features";
            public const string INCLUDE_FILES = "Include Files";
        }
        #endregion

        #region 内存记录1
        /// <summary>
        /// 内存记录
        /// </summary>
        public static class LoggingMemory1
        {
            public const string ROOT = "Logging Memory1";
            public const string TRIGGERS = "Triggers";
            public const string FILTERS = "Filters";
        }
        #endregion

        #region 内存记录2
        /// <summary>
        /// 内存记录
        /// </summary>
        public static class LoggingMemory2
        {
            public const string ROOT = "Logging Memory2";
            public const string TRIGGERS = "Triggers";
            public const string FILTERS = "Filters";
        }
        #endregion

        #region XCP/CCP
        /// <summary>
        /// CCP/XCP
        /// </summary>
        public static class CcpOrXcp
        {
            public const string ROOT = "CCP/XCP";
            public const string DESCRIPTIONS = "Descriptions";
            public const string SIGNAL_REQUESTS = "Signal Requests";
        }
        #endregion

        #region 诊断
        /// <summary>
        /// 诊断
        /// </summary>
        public static class Diagnostics
        {
            public const string ROOT = "Diagnostics";
            public const string DIAGNOSTIC_DESCRIPTIONS = "Diagnostic Descriptions";
            public const string REQUESTS = "Requests";
        }
        #endregion

        #region 输出
        /// <summary>
        /// 输出
        /// </summary>
        public static class Output
        {
            public const string ROOT = "OutPut";
            public const string LEDS = "LEDs";
            public const string TRANSMIT_MESSAGE = "Transmit Message";
            public const string SET_IGITAL_OUTPUT = "Set Digital Output";
        }
        #endregion

        #region 文件管理
        /// <summary>
        /// 文件管理
        /// </summary>
        public static class FileManager1
        {
            //一级
            public const string ROOT = "File Manager";
            //二级
            public const string LOGGER_DEVICE = "Logger Device";
            public const string CARD_READER = "Card Reader";
            //三级
            public const string DEVICE_INFORMATION = "Device Information";

            public const string CLASSIC_VIEW = "Classic View";
            public const string NAVIGATOR_VIEW = "Navigator View";
        }
        #endregion
    }
}
