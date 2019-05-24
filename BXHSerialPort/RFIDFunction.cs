using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BXHSerialPort
{
    class RFIDFunction
    {
        TcpConnectionClient tcpClient;
        public RFIDFunction()
        {
            tcpClient = new TcpConnectionClient();
        }
        public string getSerialNumber()
        {
            string[] rfidIP = IPTxtFileRxWx.readIP();
            bool InitResult = tcpClient.Init(rfidIP[0], rfidIP[1]);
            if (!InitResult)
            {
                MessageBox.Show("连接RFID失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "";
            }
            tcpClient.Send(new byte[] { 1 });//1
            byte[] buff = new byte[1024];
            int n = tcpClient.Receive(buff);
            string tcpReceiveStr = Encoding.UTF8.GetString(buff, 0, n);
            return tcpReceiveStr;
        }
        public bool getEnable()
        {
            string[] rfidIP = IPTxtFileRxWx.readIP();
            bool InitResult = tcpClient.Init(rfidIP[0], rfidIP[1]);
            if (!InitResult)
            {
                MessageBox.Show("连接RFID失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            tcpClient.Send(new byte[] { 2 });//2
            byte[] buff = new byte[1024];
            int n = tcpClient.Receive(buff);
            bool result = buff[0] == 1;
            return result;
        }
        public bool sendResult(bool result)
        {
            string[] rfidIP = IPTxtFileRxWx.readIP();
            bool InitResult = tcpClient.Init(rfidIP[0], rfidIP[1]);
            if (!InitResult)
            {
                MessageBox.Show("连接RFID失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            byte[] sendByte = result ? new byte[] { 3 } : new byte[] { 4 };//3 or 4
            tcpClient.Send(sendByte);
            byte[] buff = new byte[1024];
            int n = tcpClient.Receive(buff);
            bool returnValue = result?buff[0] == 10: buff[0] == 11;
            return returnValue;
        }
    }
}
