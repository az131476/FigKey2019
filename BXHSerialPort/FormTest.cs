using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static BXHSerialPort.Protocol.ProtocolDefine;

namespace BXHSerialPort
{
    public partial class FormTest : Form
    {
        FormSerialPortHelper serialSettingDialog = new FormSerialPortHelper();
        EventWaitHandle waitHandle = new AutoResetEvent(false);
        string txtDataFilename = @"./txtData.txt";
        string testLogDirectory = @"./log/";
        string subROMInfoFilename = "subROMInfo.txt";
        string readAddr = "0x0000	0x0010	0x0020	0x0030	0x0040	0x0050	0x0060	0x0070	0x0080	0x0090	0x00A0	0x00B0	0x00C0	0x00D0	0x00E0	0x00F0	0x0100	0x0110	0x0120	0x0130	0x01E0	0x01F0	";
        public FormTest()
        {
            InitializeComponent();
            init();
            Control.CheckForIllegalCrossThreadCalls = false;
            textBox2.Focus();
        }
        void init()
        {
            pictureBox1.BackgroundImage = Properties.Resources.red;
            serialSettingDialog.Owner = this;
            Directory.CreateDirectory(@testLogDirectory);
        }
        string programBytes(List<List<byte>> allBytes)
        {
            UInt32 addrShow = 0;
            string s = "0x" + addrShow.ToString("X4") + ":";
            string s1 = "0x" + addrShow.ToString("X4") + ":";
            int dataCount = 0;
            int u16Count = 0;
            foreach (var item in allBytes)
            {
                u16Count = 0;
                foreach (var item2 in item)
                {
                    dataCount++;
                    s += item2.ToString("X2") + " ";
                    u16Count++;
                    if (u16Count % 2 == 0)
                    {
                        s1 += ((item[u16Count - 2] << 8) + item2).ToString("X4") + " ";
                    }
                    if (dataCount % 32 == 0)
                    {
                        addrShow += 0x10;
                        if (addrShow != 0x140)
                        {
                            s += "\r\n" + "0x" + addrShow.ToString("X4") + ":";
                            s1 += "\r\n" + "0x" + addrShow.ToString("X4") + ":";
                        }
                    }
                }
            }
            return s + "\r\n" + s1;
        }
       

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            serialSettingDialog.Show();
        }
        string[] TxRx(Functions func,string[] para,byte[] sendBytes,out bool isPass)
        {
            string[] sArray = new string[3];
            string s = "";
            byte[] sendData = Protocol.ProtocolHelper.makeSendData(func, para, sendBytes);
            serialSettingDialog.comSend(sendData);

            s += "Tx:";
            foreach (var item in sendData)
            {
                s += item.ToString("X2");
            }
            s += "\r\n";

            byte[] receiveData = serialSettingDialog.comReceive();
            string err = Protocol.ProtocolHelper.analysisReceiveData(func, receiveData, sendData);
            if (err=="ok")
            {
                isPass = true;
            }
            else
            {
                isPass = false;
            }

            s += "Rx:" + err + ",";
            string receiveString = "";
            foreach (var item in receiveData)
            {
                s += item.ToString("X2");
                receiveString+= item.ToString("X2");
            }
            s += "\r\n";
            sArray[0] = s;
            sArray[1] = receiveString;
            sArray[2] = err;
            return sArray;
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            Thread th = new Thread(buttonStart_Thread);
            th.IsBackground = true;
            th.Start();
        }
        private void buttonStart_Thread()
        {
            //Timeout t = new Timeout();
            //for (int i = 0; i < 9; i++)
            //{
            //    textBox1.Text += t.IsTimeout().ToString();
            //    Thread.Sleep(500);//休眠时间
            //}
            //    return;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (!serialSettingDialog.isOpen())
            {
                MessageBox.Show("串口未打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!File.Exists(@txtDataFilename))
            {
                MessageBox.Show("请选择数据文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("请输入流水号", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<string[]> para = new List<string[]>()
            {
                //new string[]{ "9200","99",""},//SPI_Speed 参数-9200
                //new string[]{ "9200","99",""},//保护刷写   参数-无
                new string[]{ "9200","99",""},//初始化刷写 参数-无
                //new string[]{ "110","30",""},//读          参数-地址0x110 长度6
            };
            List<byte[]> sendBytes = new List<byte[]>()
            {
                //new byte[]{0x00,0x00 },
                //new byte[]{0x00,0x00 },
                new byte[]{0x00,0x00 },
                //new byte[]{0x00,0x00 }
            };
            List<Functions> fuc = new List<Functions>()
            {
                //Functions.setBaudrate,
                //Functions.writeDisable,
                Functions.writeEnable,
                //Functions.readBytes
            };
           
            UInt32 addr = 0;
            List<List<byte>> allBytes = Float2Bytes.makeBytes(@txtDataFilename);
            foreach (var item in allBytes)
            {
                fuc.Add(Functions.writeBytes);
                para.Add(new string[]{ (addr).ToString(),"1" });//参数-地址0x110 是否回读1
                addr += (UInt32)item.Count/2;
                sendBytes.Add(item.ToArray());
            }
            UInt32 addrROM = 0x01E0;
            List<List<byte>> allBytesROM = Float2Bytes.makeBytesSubROM(@subROMInfoFilename);
            foreach (var item in allBytesROM)
            {
                fuc.Add(Functions.writeBytes);
                para.Add(new string[] { addrROM.ToString(), "1" });//参数-地址0x110 是否回读1
                addrROM = 0x01F0;
                sendBytes.Add(item.ToArray());
            }
            for (int testi = 0; testi < 1; testi++)
            {

            
            bool isPass = true;
            bool isAllPass = true;
            textBox1.Text = "";

            for (int i = 0; i < fuc.Count; i++)
            {
                string[] s = TxRx(fuc[i], para[i], sendBytes[i],out isPass);
                textBox1.Text += s[0]+"\r\n";
                //waitHandle.WaitOne();
                if (isAllPass)
                {
                    isAllPass = isPass;
                }
            }
            statistics(isAllPass);
            pictureBox1.BackgroundImage = isAllPass ? Properties.Resources.green : Properties.Resources.red;
            textBox1.Text += "测试结果：" + isAllPass + "\r\n";
            string nowTime = System.DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss_ffff");
            textBox1.Text += "测试时间：" + nowTime + "\r\n";
            string tempLogFilename = testLogDirectory + textBox2.Text+nowTime+"_"+isAllPass+".txt";
            textBox1.Text += "文件记录：" + tempLogFilename+ "\r\n";
            sw.Stop();
            string elapsedTicks = (sw.ElapsedTicks / (decimal)Stopwatch.Frequency).ToString("F2");
            textBox1.Text += "测试用时：" + elapsedTicks + "\r\n";
            File.WriteAllText(@tempLogFilename, programBytes(allBytes) + "\r\n" + textBox1.Text);
            //textBox2.Text = "" + (Convert.ToInt32(textBox2.Text) + 1);
            textBox2.Clear();
            textBox2.Focus();
            }
        }
        void statistics(bool isPass)
        {
            if (isPass)
            {
                textBoxPassed.Text = (Convert.ToInt32(textBoxPassed.Text)+1).ToString();
            }
            else
            {
                textBoxFailed.Text = (Convert.ToInt32(textBoxFailed.Text) + 1).ToString();
            }
            textBoxTotal.Text = (Convert.ToInt32(textBoxTotal.Text) + 1).ToString();
        }
        private void FormTest_FormClosed(object sender, FormClosedEventArgs e)
        {
            serialSettingDialog.Close();
        }
        private void FormTest_Load(object sender, EventArgs e)
        {

            string[] s = new string[]
           {
                textBox5.Text,
                textBox6.Text,
                textBox7.Text,
                textBox8.Text,
                textBox9.Text
           };
            string[] key = new string[]
              {
                  label5.Text,
                  label6.Text,
                  label7.Text,
                  label8.Text,
                  label9.Text
              };

            INIHelper iniHelper = new INIHelper();
            iniHelper.read(key, out s);
            textBox5.Text = s[0];
            textBox6.Text = s[1];
            textBox7.Text = s[2];
            textBox8.Text = s[3];
            textBox9.Text = s[4];

        }
        private void FormTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            string[] s = new string[] 
            {
                textBox5.Text,
                textBox6.Text,
                textBox7.Text,
                textBox8.Text,
                textBox9.Text
            };
            string[] key = new string[]
              {
                  label5.Text,
                  label6.Text,
                  label7.Text,
                  label8.Text,
                  label9.Text
              };
            
            INIHelper iniHelper = new INIHelper();
            iniHelper.write(key,s);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "请选择数据文件";
            fileDialog.Filter = "*.xlsx|*.xlsx";
            //fileDialog.CheckFileExists = false;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//鼠标为忙碌状态
                textBox3.Text = fileDialog.FileName;
                textBox1.Text = "正在读取EXCEL数据...";
                EXCELHelper2 excelRead = new EXCELHelper2();
                string s = excelRead.excelRead(@textBox3.Text);
                string[] sArray = s.Split('@');
                if (readAddr!= sArray[2])
                {
                    MessageBox.Show("EXCEL格式有误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                File.WriteAllText(@subROMInfoFilename, sArray[1]);
                File.WriteAllText(@txtDataFilename, sArray[0]);
                textBox1.Text = "读取EXCEL数据成功，数据没有改变就不用再次选择";
                this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxPassed.Text = "0";
            textBoxFailed.Text = "0";
            textBoxTotal.Text = "0";
        }
        string addSpace(string s)
        {
            string ss = "";
            for (int i = 0; i < s.Length/2; i++)
            {
                ss += s.Substring(i*2,2)+" ";
            }
            return ss;
        }
        private void buttonRead_Click(object sender, EventArgs e)
        {
            if (!serialSettingDialog.isOpen())
            {
                MessageBox.Show("串口未打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                Convert.ToUInt16(textBoxStartAddress.Text,16);
                UInt16 l = Convert.ToUInt16(textBoxReadLength.Text);
                if (l>126)
                {
                    MessageBox.Show("长度需要小于126", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("请填写正确的地址与长度，如110,9", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<string[]> para = new List<string[]>()
            {
                new string[]{ textBoxStartAddress.Text,textBoxReadLength.Text,""},//读          参数-地址0x110 长度6
            };
            List<byte[]> sendBytes = new List<byte[]>()
            {
                new byte[]{0x00,0x00 },
            };
            List<Functions> fuc = new List<Functions>()
            {
                Functions.readBytes
            };
            bool isPass = false;
            for (int i = 0; i < fuc.Count; i++)
            {
                string[] s = TxRx(fuc[i], para[i], sendBytes[i], out isPass);
                if (isPass)
                {
                    textBox1.Text += s[0] + "\r\n";
                    string answerString = s[1].Substring(8, s[1].Length - 8 - 2);
                    int num = 64;
                    int times = answerString.Length / num;
                    for (int j = 0; j < times; j++)
                    {
                        textBox1.Text += addSpace(answerString.Substring(j* num, num))+ "\r\n";
                    }
                    if ((answerString.Length % num) != 0)
                    {
                        textBox1.Text += addSpace(answerString.Substring(times * num))+ "\r\n";
                    }
                }
                else
                {
                    textBox1.Text += s[2] + "\r\n";
                }
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
