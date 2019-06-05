using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CANManager.Test
{
    public partial class TestConMonGoose : Form
    {
        uint deviceID = 0;
        uint deviceName = 0;
        uint protocolID = 6;//ISO15765
        uint flags = 0;
        uint baudRate = 500000;
        uint ChannelID = 0;


        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_OpenDevice(UInt32 DeviceType, UInt32 DeviceInd, UInt32 Reserved);

        public TestConMonGoose()
        {
            InitializeComponent();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            //初始化
            textBox1.Text = "res_open:" + MonGoose.MonOpen(new IntPtr(deviceName), ref deviceID) + "\r\n";
            textBox1.Text += "deviceID:"+deviceID + "\r\n";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //连接
            textBox1.Text += "res_con:"+MonGoose.MonConnect(deviceID, protocolID, flags, baudRate, ref ChannelID) + "\r\n";
            textBox1.Text += "channelID:" + ChannelID + "\r\n";
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            //写数据
            PassthruMsg writeStruct = new PassthruMsg();
            writeStruct.ProtocolID = 6;
            writeStruct.RxStatus = 0;
            if (writeStruct.ProtocolID == 6)
                writeStruct.TxFlags = 0x00000040;
            else
                writeStruct.TxFlags = 0;
            writeStruct.Timestamp = 0;

            string[] strArray = new string[]{ "0x02", "0x01", "0x00", "0x55", "0x55", "0x55","0x55","0x55"};
            writeStruct.DataSize = (uint)strArray.Length;
            AddByteData(writeStruct,strArray,0,(int)writeStruct.DataSize);
            uint pNumMsg = 1;
            uint timeout = 10;
            ChannelID = 1;

            textBox1.Text += "writeMsg:" +MonGoose.MonWriteMsgs(ChannelID,ref writeStruct,ref pNumMsg,timeout)+"\r\n";
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //写数据
            PassthruMsg Msg = new PassthruMsg();
            //memset(&Msg, 0, 2 * sizeof(Msg));
            Msg.ProtocolID = 6;
            Msg.TxFlags = 0x00000040;
            Msg.ProtocolID = 6;
            Msg.TxFlags = 0x00000040;
            uint pnumMsg = 1;
            uint timeout = 100;
            textBox1.Text += "readMsg:"+ MonGoose.MonReadMsgs(ChannelID,ref Msg,ref pnumMsg,timeout)+"\r\n";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            //关闭
            textBox1.Text += "disconnect channel:" + MonGoose.MonDisconnect(ChannelID)+"\r\n";
            textBox1.Text += "device close:"+ MonGoose.MonClose(deviceID)+"\r\n";
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = ""+VCI_OpenDevice(4,0,0);
        }

        unsafe private void AddByteData(PassthruMsg writeStruct, string[] sourchData, int index, int len)
        {
            for (int i = 0; i < len; i++)
            {
                writeStruct.Data[i] = Convert.ToByte(sourchData[i], 16);
            }
        }
    }
}
