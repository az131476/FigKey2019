using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BXHSerialPort
{
    public partial class FormSerialPortHelper : Form
    {
        private SerialPort ComDevice = new SerialPort();
        public FormSerialPortHelper()
        {
            InitializeComponent();
            init();
        }
        private void init()
        {
            string[] allPortName = SerialPort.GetPortNames();
            cbbComList.Items.AddRange(allPortName);
            if (cbbComList.Items.Count > 0)
            {
                cbbComList.SelectedIndex = 0;
            }
            for (int i = 0; i < allPortName.Length; i++)
            {
                if (allPortName[i] == "COM5")
                {
                    cbbComList.SelectedIndex = i;
                    break;
                }
            }
            cbbBaudRate.SelectedIndex = 11;
            cbbDataBits.SelectedIndex = 0;
            cbbParity.SelectedIndex = 0;
            cbbStopBits.SelectedIndex = 0;
            pictureBox1.BackgroundImage = Properties.Resources.red;
        }
        public bool isOpen()
        {
            return ComDevice.IsOpen;
        }
        public bool comSend(byte[] data)
        {
            if (ComDevice.IsOpen)
            {
                try
                {
                    ComDevice.Write(data, 0, data.Length);//发送数据
                    return true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("串口未打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        public byte[] comReceive(int length)
        {
            byte[] ReDatas = new byte[length];
            ComDevice.Read(ReDatas, 0, ReDatas.Length);//读取数据
            return ReDatas;
        }
        public byte[] comReceiveTimeout()
        {
            List<byte> ReDatasList = new List<byte>();
            Timeout timeout = new Timeout();
            while (true)
            {
                byte[] ReDatas = new byte[ComDevice.BytesToRead];
                ComDevice.Read(ReDatas, 0, ReDatas.Length);//读取数据
                ReDatasList.AddRange(ReDatas);
                if (timeout.IsTimeout()|| ReDatasList.Count>=6)
                {
                    return ReDatasList.ToArray();
                }
            }
          }
        public void comReceiveAll()
        {
                byte[] ReDatas = new byte[ComDevice.BytesToRead];
                ComDevice.Read(ReDatas, 0, ReDatas.Length);//读取数据
        }
        public byte[] comReceive()
        {
            List<byte> ReDatasList = new List<byte>();
            int receivedLength = 0;
            Timeout timeout = new Timeout();
            while (true)
            {
                byte[] ReDatas = new byte[ComDevice.BytesToRead];
                ComDevice.Read(ReDatas, 0, ReDatas.Length);//读取数据
                ReDatasList.AddRange(ReDatas);
                if (ReDatasList.Count >= 2)
                {
                    if (ReDatasList[0] != Protocol.ProtocolDefine.frameHead)
                    {
                        return ReDatasList.ToArray();
                    }
                    receivedLength = ReDatasList[1];
                    if (ReDatasList.Count >= receivedLength + 3 && receivedLength > 0)
                    {
                        return ReDatasList.ToArray();
                    }
                }
                if (timeout.IsTimeout())
                {
                    return ReDatasList.ToArray();
                }
            }
          
        }
        public bool comOpen()
        {
            INIHelper ini = new INIHelper(@"./StartBtnCOM.ini", "Demo");
            string[] iniSetting = new string[1];
            string[] iniKey = { "Item1"};
            ini.read(iniKey, out iniSetting);
            bool isFindCOM = false;
            for (int i = 0; i < cbbComList.Items.Count; i++)
            {
                if (cbbComList.Items[i].ToString() == iniSetting[0])
                {
                    cbbComList.SelectedIndex = i;
                    isFindCOM = true;
                    break;
                }
            }
            if (isFindCOM)
            {
                cbbBaudRate.SelectedIndex = 5;
                cbbDataBits.SelectedIndex = 0;
                cbbParity.SelectedIndex = 0;
                cbbStopBits.SelectedIndex = 0;
                pictureBox1.BackgroundImage = Properties.Resources.red;
                btnOpen_Click(this, null);
                return true;
            }
            else
            {
                MessageBox.Show("没有找到启动按钮的："+ iniSetting[0], "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (cbbComList.Items.Count <= 0)
            {
                MessageBox.Show("没有发现串口,请检查线路！");
                return;
            }

            if (ComDevice.IsOpen == false)
            {
                ComDevice.PortName = cbbComList.SelectedItem.ToString();
                ComDevice.BaudRate = Convert.ToInt32(cbbBaudRate.SelectedItem.ToString());
                ComDevice.Parity = (Parity)Convert.ToInt32(cbbParity.SelectedIndex.ToString());
                ComDevice.DataBits = Convert.ToInt32(cbbDataBits.SelectedItem.ToString());
                ComDevice.StopBits = (StopBits)Convert.ToInt32(cbbStopBits.SelectedItem.ToString());
                try
                {
                    ComDevice.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                btnOpen.Text = "关闭串口";
                pictureBox1.BackgroundImage = Properties.Resources.green;
                //(this.Owner as FormTest).pictureBox1.BackgroundImage = Properties.Resources.green;
            }
            else
            {
                try
                {
                    ComDevice.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                btnOpen.Text = "打开串口";
                pictureBox1.BackgroundImage = Properties.Resources.red;
                //(this.Owner as FormTest).pictureBox1.BackgroundImage = Properties.Resources.red;
            }

            cbbComList.Enabled = !ComDevice.IsOpen;
            cbbBaudRate.Enabled = !ComDevice.IsOpen;
            cbbParity.Enabled = !ComDevice.IsOpen;
            cbbDataBits.Enabled = !ComDevice.IsOpen;
            cbbStopBits.Enabled = !ComDevice.IsOpen;
        }
        public void Close(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
