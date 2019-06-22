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
using System.IO;

namespace CANManager.Test
{
    public partial class TestConMonGoose : Form
    {
        public delegate void PDelegate(string msg);
        PDelegate myDelegate;
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

        unsafe private void Button5_Click(object sender, EventArgs e)
        {
            //写数据
            PassthruMsg writeStruct = new PassthruMsg();
            writeStruct.ProtocolID = 6;
            writeStruct.RxStatus = 0;
            if (writeStruct.ProtocolID == 6)
                writeStruct.TxFlags = 0x00000040;
            else
                writeStruct.TxFlags = 0x00;
            writeStruct.Timestamp = 0;

            string[] strArray = new string[]{ "0x01", "0x00", "0x07", "0xDF", "0x01", "0x00"};
            //AddByteData(writeStruct,strArray,0, (int)writeStruct.DataSize);
            writeStruct.Data[0] = 0x00;
            writeStruct.Data[1] = 0x00;
            writeStruct.Data[2] = 0x07;
            writeStruct.Data[3] = 0xdf;//7df  7e0
            writeStruct.Data[4] = 0x01;
            writeStruct.Data[5] = 0x00;
            writeStruct.DataSize = 6;
            uint pNumMsg = 1;
            uint timeout = 100;
            //ChannelID = 1;
            textBox1.Text += "monIcotlRes1:" + MonGoose.MonIcotl(ChannelID, 0x07, new IntPtr(0), new IntPtr(0)) + "\r\n";
            textBox1.Text += "writeMsg:" +MonGoose.MonWriteMsgs(ChannelID,ref writeStruct,ref pNumMsg,timeout)+"\r\n";
            textBox1.Text += writeStruct.Data[0] + "|" + writeStruct.Data[1]+"|"+writeStruct.Data[2]+"|"+writeStruct.Data[3]
                + "|" + writeStruct.Data[4] + "|" + writeStruct.Data[5]+ "\r\n";
        }

        unsafe private void Button4_Click(object sender, EventArgs e)
        {
            //读数据
            PassthruMsg Msg = new PassthruMsg();
            //memset(&Msg, 0, 2 * sizeof(Msg));
            Msg.TxFlags = 0x00000040;
            Msg.ProtocolID = 6;
            uint pnumMsg = 1;
            uint timeout = 100;
            textBox1.Text += "readMsg:"+ MonGoose.MonReadMsgs(ChannelID,ref Msg,ref pnumMsg,timeout)+"\r\n";
            textBox1.Text += "datasize:" + Msg.DataSize+"\r\n";
            textBox1.Text += "data:"+Msg.Data[0] + "|" + Msg.Data[1]+"|"+Msg.Data[2]+"|"+Msg.Data[3]+"|"+Msg.Data[4]+"|"+Msg.Data[5]+"|"
                + Msg.Data[6] + "|" + Msg.Data[7] + "|" + Msg.Data[8] + "|" + Msg.Data[9]+"\r\n";
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
                byte v = Convert.ToByte(sourchData[i], 16);
                writeStruct.Data[i] = v;
            }
        }

        unsafe private void Button7_Click(object sender, EventArgs e)
        {
            uint fileType = 0;
            uint msgID = 0;
            uint protocol = 6;
            uint FLOW_CONTROL_FILTER = 0x00000003;
            PassthruMsg pMaskMsg = new PassthruMsg();
            pMaskMsg.ProtocolID = protocol;
            if (protocol == 6)
                pMaskMsg.TxFlags = 0x00000040;
            else
                pMaskMsg.TxFlags = 0;
            pMaskMsg.DataSize = 4;

            PassthruMsg pPatternMsg = new PassthruMsg();
            pPatternMsg.ProtocolID = protocol;
            if (protocol == 6)
                pPatternMsg.TxFlags = 0x00000040;
            else
                pPatternMsg.TxFlags = 0;
            pPatternMsg.DataSize = 4;
            pPatternMsg.Data[0] = 0;
            pPatternMsg.Data[1] = 0;
            pPatternMsg.Data[2] = 7;
            pPatternMsg.Data[3] = 0xe8;

            uint pMsgID = 1;

            uint handleID = 0;
            uint loctlID = 0;
            int pInput = 0;
            int pOutput = 0;
            uint CLEAR_RX_BUFFER = 0x08;
            uint CLEAR_TX_BUFFER = 0x07;
            textBox1.Text += "channelID:"+ChannelID+"\r\n";
            textBox1.Text += "monIcotlRes1:"+ MonGoose.MonIcotl(ChannelID, CLEAR_RX_BUFFER,new IntPtr(0),new IntPtr(0))+"\r\n";
            textBox1.Text += "monIcotlRes2:"+ MonGoose.MonIcotl(ChannelID, CLEAR_TX_BUFFER, new IntPtr(0), new IntPtr(0))+"\r\n";

            if (6 == protocol)               //当选择的是UDS时，过滤ID
            {
                pMaskMsg.Data[0] = 0xFF;
                pMaskMsg.Data[1] = 0xFF;
                pMaskMsg.Data[2] = 0xFF;
                pMaskMsg.Data[3] = 0xFF;

                //GetTxMsg(iRxID, str_Tx, pPatternMsg.Data, protocol);
                PassthruMsg pFlowControlMsg = new PassthruMsg();
                pFlowControlMsg.ProtocolID = protocol;
                pFlowControlMsg.TxFlags = 0x00000040;
                pFlowControlMsg.DataSize = 4;
                pFlowControlMsg.Data[0] = 0;
                pFlowControlMsg.Data[1] = 0;
                pFlowControlMsg.Data[2] = 7;
                pFlowControlMsg.Data[3] = 0xe0;

                //GetTxMsg(iTxID, str_Tx, pFlowControlMsg.Data, protocol);

                textBox1.Text += "filterRes:" + MonGoose.MonStartMsgFilter(ChannelID, FLOW_CONTROL_FILTER, 
                    ref pMaskMsg, ref pPatternMsg, ref pFlowControlMsg, ref pMsgID)+"\r\n";
            }
            else if (4 == protocol)                  //当选择的是KWP时，只过滤源地址和目标地址
            {
                pMaskMsg.Data[0] = 0x00;
                pMaskMsg.Data[1] = 0xFF;
                pMaskMsg.Data[2] = 0xFF;
                pMaskMsg.Data[3] = 0x00;

                pPatternMsg.Data[0] = 0x00;
                pPatternMsg.Data[1] = 0x00;//iRxID;
                pPatternMsg.Data[2] = 0x00;// iTxID;
                pPatternMsg.Data[3] = 0x00;

                //MonGoose.MonStartMsgFilter(ChannelID, FLOW_CONTROL_FILTER, ref pMaskMsg, ref pPatternMsg, ref pFlowControlMsg, ref pMsgID);
            }
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            //
            this.Invoke(new Action(() =>
            {

            }));
        }

        private void GridControl1_Click(object sender, EventArgs e)
        {

        }

        private void TestConMonGoose_Load(object sender, EventArgs e)
        {
            
        }

        public void RefreshText(string msg)
        {
            textBox1.Text += msg;
            //dsl1807200473
            //123456
        }
    }
}
