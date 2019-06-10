using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using CANManager.Model;
using CommonUtils.Logger;

namespace CANManager.View
{
    public partial class DeviceConfig : Telerik.WinControls.UI.RadForm
    {
        private DeviceInfo device;
        public DeviceConfig(DeviceInfo deviceInfo)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.device = deviceInfo;
            device.SelectedDevice = DeviceInfo.SelectDeviceType.USB_CAN_2;
            InitControl();
            EventControl();
        }

        private void InitControl()
        {
            device_name.Text = "0";
            device_id.Text = "0";
            device_protocol.Text = "6";
            device_flags.Text = "0";
            device_bauRate.Text = "500000";
            device_channelID.Text = "0";
        }

        private void EventControl()
        {
            applyCancel.Click += ApplyCancel_Click;
            applyOk.Click += ApplyOk_Click;
            applyAstart.Click += ApplyAstart_Click;
        }

        private void ApplyAstart_Click(object sender, EventArgs e)
        {
            SetConfig();
            this.DialogResult = DialogResult.OK;
        }

        private void ApplyOk_Click(object sender, EventArgs e)
        {
            SetConfig();
        }

        private void ApplyCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SetConfig()
        {
            try
            {

                if (string.IsNullOrEmpty(device_name.Text.Trim()))
                {
                    MessageBox.Show("设备名称不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                device.DeviceName = (IntPtr)int.Parse(device_name.Text.Trim());

                if (string.IsNullOrEmpty(device_id.Text.Trim()))
                {
                    MessageBox.Show("设备索引不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                device.DeviceID = uint.Parse(device_id.Text.Trim());

                if (string.IsNullOrEmpty(device_channelID.Text.Trim()))
                {
                    MessageBox.Show("通道ID不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                device.ChannelID = uint.Parse(device_channelID.Text.Trim());

                if (string.IsNullOrEmpty(device_protocol.Text.Trim()))
                {
                    MessageBox.Show("协议类型不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                device.ProtocolID = (DeviceInfo.ProtocolType)Enum.Parse(typeof(DeviceInfo.ProtocolType), device_protocol.Text.Trim());

                if (string.IsNullOrEmpty(device_flags.Text.Trim()))
                {
                    MessageBox.Show("特定选项不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                device.Flags = uint.Parse(device_flags.Text.Trim());

                if (string.IsNullOrEmpty(device_bauRate.Text.Trim()))
                {
                    MessageBox.Show("波特率不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                device.BaudRate = uint.Parse(device_bauRate.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入格式错误", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                LogHelper.Log.Info(ex.Message + ex.StackTrace);
                return;
            }
        }
    }
}
