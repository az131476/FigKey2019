using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CANManager.View;
using CANManager.Model;
using CommonUtils.Logger;

namespace CANManager
{
    public partial class MainForm : Form
    {
        private DeviceInfo deviceInfo;
        public MainForm()
        {
            InitializeComponent();
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Init();
            EventControl();
        }

        private void Init()
        {
            deviceInfo = new DeviceInfo();
        }

        private void EventControl()
        {
            device_usb_can1.Click += Device_usb_can1_Click;
            device_usb_can2.Click += Device_usb_can2_Click;
        }

        /// <summary>
        /// 选择设备usb_can1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Device_usb_can1_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 选择设备usb_can_2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Device_usb_can2_Click(object sender, EventArgs e)
        {
            DeviceConfig deviceCfg = new DeviceConfig(deviceInfo);
            uint deviceID = deviceInfo.DeviceID;
            uint channelID = deviceInfo.ChannelID;
            if (deviceCfg.ShowDialog() == DialogResult.OK)
            {
                StartDevice(deviceID,channelID);
            }
        }

        /// <summary>
        /// 启动设备:初始化+连接设备
        /// </summary>
        /// <param name="deviceID"></param>
        /// <param name="channelID"></param>
        private void StartDevice(uint deviceID,uint channelID)
        {
            //初始化设备
            if (deviceInfo.DeviceStatus == DeviceInfo.DeviceStatusEnum.OPEN || deviceInfo.DeviceStatus == DeviceInfo.DeviceStatusEnum.CONNECTION)
            {
                LogHelper.Log.Info("当前设备已经初始化或建立连接！");
                return;
            }
            int openRes = MonGoose.MonOpen(deviceInfo.DeviceName, ref deviceID);
            deviceInfo.DeviceID = deviceID;
            RES_MONGOOSE_HEX openResEnum = (RES_MONGOOSE_HEX)Enum.Parse(typeof(RES_MONGOOSE_HEX), Convert.ToString(openRes, 16));
            if (RES_MONGOOSE_HEX.STATUS_NOERROR == openResEnum)
            {
                LogHelper.Log.Info("初始化设备成功！");
                deviceInfo.DeviceStatus = DeviceInfo.DeviceStatusEnum.OPEN;
                //连接设备
                int conRes = MonGoose.MonConnect(deviceInfo.DeviceID, (uint)deviceInfo.ProtocolID, deviceInfo.Flags, deviceInfo.BaudRate, ref channelID);
                deviceInfo.ChannelID = channelID;
                RES_MONGOOSE_HEX conResEnum = (RES_MONGOOSE_HEX)Enum.Parse(typeof(RES_MONGOOSE_HEX), Convert.ToString(conRes, 16));
                if (conResEnum == RES_MONGOOSE_HEX.STATUS_NOERROR)
                {
                    deviceInfo.DeviceStatus = DeviceInfo.DeviceStatusEnum.CONNECTION;
                    MessageBox.Show("启动设备成功！");
                    LogHelper.Log.Info("启动设备成功！");
                }
                else
                {
                    MessageBox.Show("启动设备失败！" + conResEnum);
                    LogHelper.Log.Info("启动设备失败！");
                }
            }
            else
            {
                MessageBox.Show("初始化设备失败！" + openResEnum);
                LogHelper.Log.Info("初始化设备失败！");
            }
        }

        /// <summary>
        /// 关闭设备：断开连接+关闭设备
        /// </summary>
        private void StopDevice()
        {
            if (deviceInfo.DeviceStatus == DeviceInfo.DeviceStatusEnum.DISCONNECTION)
            {
                LogHelper.Log.Info("已经断开与设备的连接，无须再次重复操作！");
                return;
            }
            int disconRes = MonGoose.MonDisconnect(deviceInfo.ChannelID);
            RES_MONGOOSE_HEX disconResEnum = (RES_MONGOOSE_HEX)Enum.Parse(typeof(RES_MONGOOSE_HEX),Convert.ToString(disconRes,16));
            if (disconResEnum == RES_MONGOOSE_HEX.STATUS_NOERROR)
            {
                deviceInfo.DeviceStatus = DeviceInfo.DeviceStatusEnum.DISCONNECTION;
                LogHelper.Log.Info("成功断开与设备的连接！");
                if (deviceInfo.DeviceStatus == DeviceInfo.DeviceStatusEnum.CLOSE)
                {
                    LogHelper.Log.Info("已经关闭设备，请勿重复操作！");
                    return;
                }
                int closeRes = MonGoose.MonClose(deviceInfo.DeviceID);
                RES_MONGOOSE_HEX closeResEnum = (RES_MONGOOSE_HEX)Enum.Parse(typeof(RES_MONGOOSE_HEX),Convert.ToString(closeRes,16));
                if (closeResEnum == RES_MONGOOSE_HEX.STATUS_NOERROR)
                {
                    deviceInfo.DeviceStatus = DeviceInfo.DeviceStatusEnum.CLOSE;
                    LogHelper.Log.Info("成功关闭设备！");
                }
                else
                {
                    LogHelper.Log.Info("关闭设备失败!");
                }
            }
            else
            {
                LogHelper.Log.Info("断开与设备的连接失败！");
            }
        }
    }
}
