#define NORMAL
using OPCDAAutoTest;
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
    public partial class FormTestNew : Form
    {
        FormSerialPortHelper serialSettingDialog = new FormSerialPortHelper();
        FormSerialPortHelper physicalButtonDialog = new FormSerialPortHelper();
        bool Administrator = false;
        string txtDataFilename = @"./txtData.txt";
        string testLogDirectory = @"./log/";
        string subROMInfoFilename = "subROMInfo.txt";
        string readAddr = "0x0000	0x0010	0x0020	0x0030	0x0040	0x0050	0x0060	0x0070	0x0080	0x0090	0x00A0	0x00B0	0x00C0	0x00D0	0x00E0	0x00F0	0x0100	0x0110	0x0120	0x0130	0x01E0	0x01F0	";
        List<string> programData = new List<string> ();
        bool stopFlag = false;
        RFIDFunction rfid = new RFIDFunction();
        OPC_Client opc = new OPC_Client (); 
        bool isConnShareDir = false;//是否连接共享文件夹
        bool enablePLC = false;



        public FormTestNew()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }
        void physicalButtonCheck()
        {
            bool isOpen = physicalButtonDialog.comOpen();
            if (!isOpen)
            {
                return;
            }
            if (!physicalButtonDialog.isOpen())
            {
                MessageBox.Show("启动按钮串口未打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            while (true)
            {
                if (physicalButtonDialog.isOpen())
                {
                    if (panelSingleTest.Enabled)
                    {
                        byte[] receiveData = physicalButtonDialog.comReceiveTimeout();
                        byte[] standardData = { 0x01, 0x02, 0x01, 0x01, 0x60, 0x48 };
                        bool isReceive = true;
                        if (receiveData.Length>=6)
                        {
                            for (int i = 0; i < standardData.Length; i++)
                            {
                                if(receiveData[i]!= standardData[i])
                                {
                                    isReceive = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            isReceive = false;
                        }
                        if (isReceive)
                        {
                            stopFlag = true;
                            Thread th = new Thread(buttonStart_Thread);
                            th.IsBackground = true;
                            th.Start();
                        }
                    }
                    else
                    {
                        physicalButtonDialog.comReceiveAll();
                    }
                    Delay.DelayMillisecond(200);
                }
                else
                {
                    MessageBox.Show("启动按钮串口未打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }
        private void FormTestNew_Load(object sender, EventArgs e)
        {
            INIHelper iniHelper = new INIHelper();
            string[] enPlcSetting = new string[1];
            string[] enPlcKey = { "0"};
            iniHelper.read(enPlcKey, out enPlcSetting);
            enablePLC = enPlcSetting[0] == "a";
            try
            {
                opc.Init();
                labelResult.BackColor = Color.Green;
                labelResult.Text = "PLC已连接";
            }
            catch (Exception ex)
            {
                labelResult.BackColor = Color.Red;
                labelResult.Text = "PLC未连接";
            }

            Thread th = new Thread(physicalButtonCheck);
            th.IsBackground = true;
            th.Start();

            //checkedListBox1全部选中
            for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
            {
                this.checkedListBox1.SetItemChecked(i, true);
            }
            addProgressBar();
            controlEnable(Administrator);
        }
        //点击进度条的时候，显示这个进度条烧写的内容
        private void P1_MouseClick(object sender, MouseEventArgs e)
        {
            TextBox t = new TextBox();
            t.Width = flowLayoutPanel1.Width - 30;
            t.Multiline = true;
            t.BackColor = Color.Beige;
            t.Height = t.Height * 5;
            int index = this.flowLayoutPanel1.Controls.GetChildIndex((ProgressBar)sender);
            int textBoxNum = 0;
            for (int i = 0; i < index; i++)
            {
                if(this.flowLayoutPanel1.Controls[i] is TextBox)
                {
                    textBoxNum++;
                }
            }
            int pbIndex = index - textBoxNum;
            if (pbIndex< programData.Count)
            {
                t.Text = programData[pbIndex];
            }
            else
            {
                t.Text = "None";
            }
            //if (this.flowLayoutPanel1.Controls[index].GetType() != typeof(TextBox))
            if ((this.flowLayoutPanel1.Controls.Count == index+1)||(!(this.flowLayoutPanel1.Controls[index+1] is TextBox)))
            {
                this.flowLayoutPanel1.Controls.Add(t);
                this.flowLayoutPanel1.Controls.SetChildIndex(t, index + 1);
            }
            else
            {
                if (this.flowLayoutPanel1.Controls.Count>index+1)
                {
                    this.flowLayoutPanel1.Controls.RemoveAt(index + 1);
                }
            }
        }

        private void groupBox1_Resize(object sender, EventArgs e)
        {
            //int gap = (button5.Top - button1.Top) / 4;
            //button2.Top = button1.Top + gap;
            //button3.Top = button2.Top + gap;
            //button4.Top = button3.Top + gap;
        }

        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                flowLayoutPanel1.Controls[i].Width = flowLayoutPanel1.Width - 30;
            }
        }

        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            addProgressBar();
        }
        //draw groupBox border
        void groupBox_Paint(object sender, PaintEventArgs e)
        {
            GroupBox gBox = (GroupBox)sender;

            e.Graphics.Clear(gBox.BackColor);
            e.Graphics.DrawString(gBox.Text, gBox.Font, Brushes.Red, 10, 1);
            var vSize = e.Graphics.MeasureString(gBox.Text, gBox.Font);
            e.Graphics.DrawLine(Pens.Blue, 1, vSize.Height / 2, 8, vSize.Height / 2);
            e.Graphics.DrawLine(Pens.Blue, vSize.Width + 8, vSize.Height / 2, gBox.Width - 2, vSize.Height / 2);
            e.Graphics.DrawLine(Pens.Blue, 1, vSize.Height / 2, 1, gBox.Height - 2);
            e.Graphics.DrawLine(Pens.Blue, 1, gBox.Height - 2, gBox.Width - 2, gBox.Height - 2);
            e.Graphics.DrawLine(Pens.Blue, gBox.Width - 2, vSize.Height / 2, gBox.Width - 2, gBox.Height - 2);
        }
        void zeroProgressBarValue()
        {
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                if (flowLayoutPanel1.Controls[i] is ProgressBar)
                {
                    ProgressBar pb = (ProgressBar)flowLayoutPanel1.Controls[i];
                    pb.Value = 0;
                }
            }
        }
        void changeProgressBarValue(object iIndex)
        {
            int index = (int)iIndex;
            int pbIndex = 0;
            int realIndex = 0;
            bool find = false;
            ProgressBar pb;
            for (int i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                if (flowLayoutPanel1.Controls[i] is ProgressBar)
                {
                    if (pbIndex == index)
                    {
                        realIndex = i;
                        find = true;
                        break;
                    }
                    pbIndex++;
                }
            }
            if (find)
            {
                pb = (ProgressBar)flowLayoutPanel1.Controls[realIndex];

                for (int i = 0; i < 101; i++)
                {
                    Delay.DelayMillisecond(2);
                    pb.Value = i;
                }
            } 
        }
        //add progressbar to flowLayoutPanel
        void addProgressBar()
        {
            flowLayoutPanel1.Controls.Clear();
            for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
            {
                ProgressBar p1 = new ProgressBar();
                ToolTip tip = new ToolTip();
                tip.SetToolTip(p1, checkedListBox1.GetItemText(checkedListBox1.CheckedItems[i]));
                p1.Width = flowLayoutPanel1.Width - 30;
                p1.MouseClick += P1_MouseClick;
                flowLayoutPanel1.Controls.Add(p1);
            }
        }
        //change user
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Login l = new Login();
            l.ShowDialog();
            Administrator = l.Administrator;
            controlEnable(Administrator);
        }
        void controlEnable(bool enable)
        {
            if (enable)
            {
                toolStripStatusLabel1.Text = "role：Administrator";
                panelProductSetting.Enabled = true;
            }
            else
            {
                toolStripStatusLabel1.Text = "role：User";
                panelProductSetting.Enabled = false;
            }
        }
        //open serial port dialog
        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            serialSettingDialog.Show();
        }
        //to excel
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "请选择数据文件";
            fileDialog.Filter = "*.xlsx|*.xlsx";
            fileDialog.CheckFileExists = false;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;//鼠标为忙碌状态
                EXCELHelper2 excel = new EXCELHelper2();
                int programDataCount = programData.Count;
                List<string> s = new List<string>();
                for (int i = 0; i < programDataCount; i++)
                {
                    int rowCount = StringHelper.SubstringCount(programData[i],"\r\n");
                    string[] item = programData[i].Replace("\r\n", "!").Split('!');
                    s.AddRange(item);
                }
                int allRowCount = s.Count;
                string[][] ss = new string[allRowCount][];
                for (int i = 0; i < allRowCount; i++)
                {
                    ss[i] = s[i].Replace(":", " ").Split(' ');
                }
                excel.excelWrite(fileDialog.FileName,ss);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;//设置鼠标为正常状态
            }
        }
        //update time
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (layeredLabelTime.Text != DateTime.Now.ToString("HH:mm:ss"))
            {
                layeredLabelTime.Text = DateTime.Now.ToString("HH:mm:ss");
            }
        }
        //单步测试
        private void panelSingleTest_Click(object sender, EventArgs e)
        {
            stopFlag = true;
            Thread th = new Thread(buttonStart_Thread);
            th.IsBackground = true;
            th.Start();
        }
        //循环测试
        private void panelCycleTest_Click(object sender, EventArgs e)
        {
            stopFlag = true;
            Thread th = new Thread(buttonStart_Thread);
            th.IsBackground = true;
            th.Start();
        }
        //终止测试
        private void panelStopTest_Click(object sender, EventArgs e)
        {
            stopFlag = true;
            panelSingleTest.Enabled = true;
            panelCycleTest.Enabled = true;
            labelResult.Text = "已终止";
            //labelResult.Text = "刷写中";
            //labelResult.BackColor = Color.Green;
            //flowLayoutPanel1.BackColor = Color.Green;
        }
        //测试线程
        private void buttonStart_Thread()
        {
            panelSingleTest.Enabled = false;
            panelCycleTest.Enabled = false;

            labelResult.Text = "刷写中";
            labelResult.BackColor = Color.Transparent;
            flowLayoutPanel1.BackColor = Color.Transparent;


            Stopwatch sw = new Stopwatch();
            sw.Start();

            
            
            try
            {
                if (enablePLC)
                {
                    opc.ClearResult();
                }
                if (checkBox1.Checked)
                {
                    textBoxSerialNumber.Text = textBoxSerialNumber.Text.Substring(9);
                }
                else
                {
                    textBoxSerialNumber.Text = opc.ReadSerialNumber();
                }
                if (enablePLC)
                {
                    if (!opc.BuildAble())
                    {
                        MessageBox.Show("此工位不允许做！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

            }
            catch (Exception)
            {
                MessageBox.Show("OPC通讯失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
# if NORMAL
            if (!serialSettingDialog.isOpen())
            {
                MessageBox.Show("串口未打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
#endif
            //textBoxSerialNumber.Text = rfid.getSerialNumber();
            //bool rfidEnable = rfid.getEnable();
            //if (!rfidEnable)
            //{
            //    MessageBox.Show("此工位不允许做！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
          

            if (textBoxSerialNumber.Text == "")
            {
                MessageBox.Show("请输入序列号", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //读序列号
            //string[] ipInfo = IPTxtFileRxWx.readIP();
            //tcpClient.Init(ipInfo[0], ipInfo[1]);
            //根据序列号得到烧录数据
            INIHelper iniHelper = new INIHelper();
            string[] FolderShareSetting = new string [10];
            string[] FolderShareKey = { "FolderSharePath", "User", "Password", "Local", "FolderSharePath2", "User2", "Password2", "FolderSharePath3", "User3", "Password3" };
            iniHelper.read(FolderShareKey, out FolderShareSetting);
            //FolderShare.connectState(@"\\10.200.8.73\share", "administrator", "11111111");
            // MessageBox.Show(FolderShareSetting[0], "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //MessageBox.Show(FolderShareSetting[1], "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //MessageBox.Show(FolderShareSetting[2], "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (false.ToString()== FolderShareSetting[3])
            {
                if (!isConnShareDir)
                {
                    try
                    {
                        if (FolderShareSetting[0]!="")
                        {
                            FolderShare.connectState(@FolderShareSetting[0], FolderShareSetting[1], FolderShareSetting[2]);
                        }
                        if (FolderShareSetting[4] != "")
                        {
                            FolderShare.connectState(@FolderShareSetting[4], FolderShareSetting[5], FolderShareSetting[6]);
                        }
                        if (FolderShareSetting[7] != "")
                        {
                            FolderShare.connectState(@FolderShareSetting[7], FolderShareSetting[8], FolderShareSetting[9]);
                        }
                        isConnShareDir = true;
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("连接共享文件夹出错", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            //MessageBox.Show("okokokok", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            GetProgramData pgmData = new GetProgramData();
            string pressureDataPath = @"./pressureData.txt";
            if (!File.Exists(@pressureDataPath))
            {
                MessageBox.Show("压力数据文件不存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<List<string>> pressureData = pgmData.readPressure(pressureDataPath);
            if (pressureData.Count!=6)
            {
                MessageBox.Show("压力数据应为6行！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int[] pressureDataLineCount = {23,31,31,23,23,23 };
            for (int i = 0; i < pressureData.Count; i++)
            {
                if (pressureData[i].Count!= pressureDataLineCount[i])
                {
                    MessageBox.Show("压力数据"+(i+1)+"行数量错误,应为："+ pressureDataLineCount[i], "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string currStandardDataPath = @"./currStandardData.txt";
            if (!File.Exists(currStandardDataPath))
            {
                MessageBox.Show("标准电流数据文件不存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<List<string>> currStandardData = pgmData.readPressure(currStandardDataPath);
            if (currStandardData.Count != 6)
            {
                MessageBox.Show("标准电流数据应为6行！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int[] currStandardDataLineCount = { 23, 31, 31, 23, 23, 23 };
            for (int i = 0; i < currStandardData.Count; i++)
            {
                if (currStandardData[i].Count != currStandardDataLineCount[i])
                {
                    MessageBox.Show("标准电流数据" + (i + 1) + "行数量错误,应为：" + currStandardDataLineCount[i], "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string currStandardDataSearchPath = @"./currStandardDataSearch.txt";
            if (!File.Exists(currStandardDataSearchPath))
            {
                MessageBox.Show("是否检索数据文件不存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            List<List<string>> currStandardDataSearch = pgmData.readPressure(currStandardDataSearchPath);
            if (currStandardDataSearch.Count != 6)
            {
                MessageBox.Show("是否检索数据应为6行！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int[] currStandardDataSLineCount = { 23, 31, 31, 23, 23, 23 };
            for (int i = 0; i < currStandardDataSearch.Count; i++)
            {
                if (currStandardDataSearch[i].Count != currStandardDataSLineCount[i])
                {
                    MessageBox.Show("是否检索数据" + (i + 1) + "行数量错误,应为：" + currStandardDataSLineCount[i], "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            //string currentDataPath = @"\\10.200.8.73\share\1.csv";
            string currentDataPath = @FolderShareSetting[0]+ @"\"+ textBoxSerialNumber.Text+ ".csv";
            string currentDataPath2 = @FolderShareSetting[4] + @"\" + textBoxSerialNumber.Text + ".csv";
            string currentDataPath3 = @FolderShareSetting[7] + @"\" + textBoxSerialNumber.Text + ".csv";
            bool currentDataIsExist = false;
            if (File.Exists(@currentDataPath))
            {
                currentDataIsExist = true;
            }
            if (!currentDataIsExist)
            {
                if (File.Exists(@currentDataPath2))
                {
                    currentDataIsExist = true;
                    currentDataPath = currentDataPath2;
                }
            }
            if (!currentDataIsExist)
            {
                if (File.Exists(@currentDataPath3))
                {
                    currentDataIsExist = true;
                    currentDataPath = currentDataPath3;
                }
            }
            if (!currentDataIsExist)
            {
                MessageBox.Show("原始数据文件不存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            //MessageBox.Show(currentDataPath, "读电流前错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            List<List<float>> currentData;
            try
            {
                currentData = pgmData.readCSVgetCurrent(pressureData, @currentDataPath, currStandardData, currStandardDataSearch);
                List<float> redDiffValue = pgmData.redDiff(currentData, currStandardData);
                currentData = pgmData.greenCal(currStandardDataSearch,currentData,redDiffValue,currStandardData);
            }
            catch (Exception e)
            {
                MessageBox.Show("读原始数据："+e.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            //MessageBox.Show("读电流成功", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            pgmData.writeCurrent(@txtDataFilename,currentData);
            if(!pgmData.judgeCurr(currentData))
            {
                return;
            }


            if (!File.Exists(@txtDataFilename))
            {
                MessageBox.Show("请选择数据文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (labelSerialNumberV.Text == "")
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
                para.Add(new string[] { (addr).ToString(), "1" });//参数-地址0x110 是否回读1
                addr += (UInt32)item.Count / 2;
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
            while(true)
            {
                bool isPass = true;
                bool isAllPass = true;
                programData.Clear();
                zeroProgressBarValue();
                int pbIndex = 0;
                int checkedIndex = 0;
                for (int i = 0; i < fuc.Count; i++)
                {
                    string[] s = new  string[3];
#if NORMAL
                     s= TxRx(fuc[i], para[i], sendBytes[i], out isPass);
#else

#endif
                    if (fuc[i]==Functions.writeBytes)
                    {
                        if (checkedListBox1.GetItemChecked(pbIndex))
                        {
                            if (isPass)
                            {
                                programData.Add(programBytes(sendBytes[i], Convert.ToUInt32(para[i][0])));
                                
                            }
                            else
                            {
                                programData.Add(s[0]);
                                labelResult.Text = "刷写失败";
                                labelResult.BackColor = Color.Red;
                                flowLayoutPanel1.BackColor = Color.Red;
                                break;
                            }
                            changeProgressBarValue(checkedIndex);
                            //Thread th = new Thread(changeProgressBarValue);
                            //th.IsBackground = true;
                            //th.Start(checkedIndex);
                            checkedIndex++;
                        }
                        pbIndex++;
                    }
                    
                    //waitHandle.WaitOne();
                    if (isAllPass)
                    {
                        isAllPass = isPass;
                    }
                }
                if (isAllPass)
                {
                    isAllPass = isPass;
                }

                statistics(isAllPass);
                if (isAllPass)
                {
                    labelResult.Text = "刷写成功";
                    labelResult.BackColor = Color.Green;
                    flowLayoutPanel1.BackColor = Color.Transparent;
                }
                else
                {
                    labelResult.Text = "刷写失败";
                    labelResult.BackColor = Color.Red;
                    flowLayoutPanel1.BackColor = Color.Red;
                }

                //bool rfidSendFlag = rfid.sendResult(isAllPass);
                //if (!rfidSendFlag)
                //{
                //    MessageBox.Show("跟rfid发送结果失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}
                try
                {
                    if (enablePLC)
                    {
                        opc.SendResult(isAllPass);
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("OPC通讯失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //pictureBox1.BackgroundImage = isAllPass ? Properties.Resources.green : Properties.Resources.red;
                //textBox1.Text += "测试结果：" + isAllPass + "\r\n";
                string nowTime = System.DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss_ffff");
                //textBox1.Text += "测试时间：" + nowTime + "\r\n";
                string tempLogFilename = testLogDirectory + textBoxSerialNumber.Text + nowTime + "_" + isAllPass + ".txt";
                string tempLogFilenameCurr = testLogDirectory + textBoxSerialNumber.Text + nowTime + "_" + isAllPass +"_CurrentData" +".txt";
                // textBox1.Text += "文件记录：" + tempLogFilename + "\r\n";
                sw.Stop();
                string elapsedTicks = (sw.ElapsedTicks / (decimal)Stopwatch.Frequency).ToString("F2");
                labelTestTimeV.Text =  elapsedTicks;
                File.WriteAllText(@tempLogFilename, programBytes(allBytes,0) );
                pgmData.writeCurrent(@tempLogFilenameCurr, currentData);
                //textBox2.Text = "" + (Convert.ToInt32(textBox2.Text) + 1);
                //labelSerialNumberV.Text += "1";
                if (stopFlag)
                {
                    break;
                }
            }
            stopFlag = false;
            panelSingleTest.Enabled = true ;
            panelCycleTest.Enabled = true;
        }
        string programBytes(byte[] bytes, UInt32 addrShow)
        {
            List<List<byte>> allBytes = new List<List<byte>>();
            List<byte> byteList = new List<byte>();
            byteList.AddRange(bytes);
            allBytes.Add(byteList);
            return programBytes(allBytes, addrShow);
        }
        string programBytes(List<List<byte>> allBytes, UInt32 addrShow)
        {
            int totalNum = 0;
            foreach (var item in allBytes)
            {
                foreach (var item2 in item)
                {
                    totalNum++;
                }
            }
            string s = "";
            string s1 = "";
            int dataCount = 0;
            int u16Count = 0;
            s += "0x" + addrShow.ToString("X4") + ":";
            s1 += "0x" + addrShow.ToString("X4") + ":";
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
                        if (dataCount< totalNum)
                        {
                            s += "\r\n" + "0x" + addrShow.ToString("X4") + ":";
                            s1 += "\r\n" + "0x" + addrShow.ToString("X4") + ":";
                        }
                    }
                }
            }
            return s + "\r\n" + s1;
        }
        void statistics(bool isPass)
        {
            if (isPass)
            {
                labelPassV.Text = (Convert.ToInt32(labelPassV.Text) + 1).ToString();
            }
            else
            {
                labelFailV.Text = (Convert.ToInt32(labelFailV.Text) + 1).ToString();
            }
            labelTotalV.Text = (Convert.ToInt32(labelTotalV.Text) + 1).ToString();
        }
        string[] TxRx(Functions func, string[] para, byte[] sendBytes, out bool isPass)
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
            if (err == "ok")
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
                receiveString += item.ToString("X2");
            }
            s += "\r\n";
            sArray[0] = s;
            sArray[1] = receiveString;
            sArray[2] = err;
            return sArray;
        }

        private void panelExitSystem_Click(object sender, EventArgs e)
        {
            stopFlag = true;
            this.Close();
        }

        private void FormTestNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            stopFlag = true;
            if (enablePLC)
            {
                opc.Clean();
            }
        }

        private void panelProductSetting_Click(object sender, EventArgs e)
        {
            FormProductSetting f = new FormProductSetting();
            f.Show();
        }
        string readSUBROM()
        {
            string returnString="";
            if (!serialSettingDialog.isOpen())
            {
                MessageBox.Show("串口未打开", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "串口未打开";
            }

            List<string[]> para = new List<string[]>()
            {
                new string[]{ "0","48",""},//读          参数-地址0x110 长度80
                new string[]{ "30","64",""},//读          参数-地址0x110 长度80
                new string[]{ "70","64",""},//读          参数-地址0x110 长度80
                new string[]{ "b0","48",""},//读          参数-地址0x110 长度80
                new string[]{ "e0","48",""},//读          参数-地址0x110 长度80
                new string[]{ "110","48",""},//读          参数-地址0x110 长度80
            };
            List<byte[]> sendBytes = new List<byte[]>()
            {
                new byte[]{0x00,0x00 },
                new byte[]{0x00,0x00 },
                new byte[]{0x00,0x00 },
                new byte[]{0x00,0x00 },
                new byte[]{0x00,0x00 },
                new byte[]{0x00,0x00 },
            };
            List<Functions> fuc = new List<Functions>()
            {
                Functions.readBytes,
                Functions.readBytes,
                Functions.readBytes,
                Functions.readBytes,
                Functions.readBytes,
                Functions.readBytes,
            };
            bool isPass = false;
            for (int i = 0; i < fuc.Count; i++)
            {
                string[] s = TxRx(fuc[i], para[i], sendBytes[i], out isPass);
                if (isPass)
                {
                    //returnString += s[0] + "\r\n";
                    string answerString = s[1].Substring(8, s[1].Length - 8 - 2);
                    int num = 64;
                    int times = answerString.Length / num;
                    for (int j = 0; j < times; j++)
                    {
                        returnString += addSpace(answerString.Substring(j * num, num),0==j) + "\r\n";
                    }
                    if ((answerString.Length % num) != 0)
                    {
                        returnString += addSpace(answerString.Substring(times * num),false) + "\r\n";
                    }
                }
                else
                {
                    returnString += s[2] + "\r\n";
                }
            }
            
            string nowTime = System.DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss_ffff");
            string logFileName = testLogDirectory + "ReadSUBROM"+nowTime + ".txt";
            File.WriteAllText(@logFileName, returnString);
            return returnString;
        }
        string addSpace(string s,bool isChecksum)
        {
            string ss = "";
            for (int i = 0; i < s.Length / 8; i++)
            {
                if (isChecksum&&(0==i))
                {
                    ss += s.Substring(i * 8, 8) + " ";
                }
                else
                {
                    string sRead = s.Substring(i * 8, 8);
                    string _hexBase = sRead;
                    UInt32 _u32 = Convert.ToUInt32(_hexBase,16);
                    byte[] b = BitConverter.GetBytes(_u32);
                    float _floatBase = BitConverter.ToSingle(b,0);
                    ss += _floatBase.ToString("f6") + " ";
                }
            }
            return ss;
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            FormReadSUBROM f = new FormReadSUBROM(readSUBROM());
            f.Show();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            physicalButtonDialog.Show();
        }
    }
}
