using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using Telerik.WinControls.UI;
using Telerik.WinControls.Themes;
using Telerik.WinControls.Styles;
using Telerik.WinControls;
using Telerik.WinControls.UI.Export;
using Telerik.Data;
//using Telerik.QuickStart.WinControls;
using System.IO;
using SentConfigurator.View;
using SentConfigurator.Controls;
using CommonUtils.Logger;
using CommonUtils.ByteHelper;
using CommonUtils.FileHelper;
using LEDLib;
using Sunisoft.IrisSkin;
using SentConfigurator.Model;


namespace SentConfigurator
{
    public partial class MainForm : Form
    {
        #region 成员变量
        private SerialPort serialPort;
        private bool IsSerialOpen;
        private int timer_t = 0;
        private int ReceiveByte_Cnt = 0;//串口操作，接收字节计数
        public List<byte> BufferData;
        FileStream saveDataFS = null;
        byte[] receiveBytes = new byte[10 * 1024 * 1024];   //默认10M的字节空间  
        private System.Timers.Timer timerReCon;
        private System.Timers.Timer timerOnLine;
        private bool IsSendMsg;

        private SignalConfig signalConfig;
        private CommandConfig comandCfg;
        private int groupNum;
        private SkinEngine skinEng;
        private List<string> skinList;

        private SentConfig sentConfig;
        private SentConfig.QuickSigConfig quickCfg;
        private SentConfig.BaseSigConfig baseCfg;
        private SentConfig.SlowSigConfig slowCfg;
        private List<SentConfig.SlowSigConfig> slowList;
        private string userAsPath;
        private string sysCfgPath;
        /// <summary>
        /// true-sent配置被修改，提示保存配置；false-表示未修改配置，无需提示保存
        /// </summary>
        private bool IsSentCfgChanging;
        private delegate void MessageDelegate(byte[] msg);

        #endregion

        public MainForm()
        {
            InitializeComponent();
            //LoadSkin();
            Control.CheckForIllegalCrossThreadCalls = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.dgv_groupdata.EnableHotTracking = true;
            this.dgv_groupdata.EnableAlternatingRowColor = true;
            //ThemeResolutionService.ApplicationThemeName = office2013DarkTheme1.ThemeName;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ThemeResolutionService.ApplicationThemeName = office2013LightTheme1;
            
            ThemeResolutionService.AllowAnimations = true;
            InitSerial();

            this.FormClosed += Form1_FormClosed;
            this.serialPort.DataReceived += SerialPort_DataReceived;
            DataReceiveTask();

            InitSentConfig();
            signalConfig = new SignalConfig(dgv_groupdata, groupNum);
            comandCfg = new CommandConfig();
            InitSentControlConfig();
            //刷新界面参数
            UpdateUIConfig();
            dgv_groupdata.BringToFront();

            timerReCon = new System.Timers.Timer();
            timerReCon.AutoReset = true;
            timerReCon.Interval = 2000;
            timerReCon.Elapsed += TimerReCon_Elapsed;
            timerReCon.Enabled = false;

            timerOnLine = new System.Timers.Timer();
            timerOnLine.AutoReset = true;
            timerOnLine.Interval = 2000;
            timerOnLine.Elapsed += TimerOnLine_Elapsed;
            timerOnLine.Enabled = true; 
        }

        #region 定时检测是否断开连接
        /// <summary>
        /// 监控在线状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerOnLine_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (IsSerialOpen)
            {
                //定时检测是否离线，是否启动离线自动重连
                if (!serialPort.IsOpen)
                {
                    LogHelper.Log.Info("$$$$$$$$$$$$$$$$$$$$$$设备已掉线！");
                    tool_dev_status.Text = "已掉线，离线时间-" + DateTime.Now;
                    tool_dev_status.ForeColor = Color.Red;
                    ledControl1.LEDSwitch = false;
                    ledControl1.Invalidate();
                    if (cbx_recon.Checked)
                    {
                        LogHelper.Log.Info("尝试重新获取连接...");
                        timerReCon.Enabled = true;
                    }
                }
                else
                {
                    //在线
                    tool_dev_status.Text = "OnLine";
                    tool_dev_status.ForeColor = Color.Black;
                }
            }
        }
        /// <summary>
        /// 定时检测是否断开连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerReCon_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (!serialPort.IsOpen)
            {
                if (IsSerialOpen)
                {
                    //自动重连
                    LogHelper.Log.Info("正在重连...");
                    tool_dev_status.Text = "正在重连...";
                    if (SerialOpen())
                    {
                        LogHelper.Log.Info("重连成功");
                        tool_dev_status.Text = "重连成功";
                    }
                    else
                    {
                        LogHelper.Log.Error("重连失败！");
                        tool_dev_status.Text = "重连失败";
                    }
                }
            }
        }
        #endregion

        #region 皮肤
        /// <summary>
        /// 设置皮肤
        /// </summary>
        private void LoadSkin()
        {
            skinEng = new SkinEngine();
            skinList = new List<string>();
            string path = Application.StartupPath + @"\Skins\mp10pink.ssk";
            skinList = Directory.GetFiles(Application.StartupPath + @"\Skins\", "*.ssk").ToList();
            skinEng.Active = true;
            skinEng.SkinAllForm = true;
            skinEng.SkinDialogs = true;
            skinEng.Enable3rdControl = true;
            skinEng.SkinFile = skinList[23];
            //ThemeResolutionService.ApplicationThemeName = office2013DarkTheme1.ThemeName;
        }
        #endregion

        #region 接收下位机消息
        

        /// <summary>
        /// 下发指令后能否正常接收数据
        /// </summary>
        private void DataReceiveTask()
        {
            Task task = new Task(()=>
            {
                //命令发送成功后，是否能读取数据
                while (true)
                {
                    if (IsSendMsg)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            if (!status_receivecount.Text.Equals("0"))
                            {
                                IsSendMsg = false;
                                return;
                            }
                            Thread.Sleep(800);
                        }
                        IsSendMsg = false;
                        MessageBox.Show("命令发送失败！请检测串口是否正确配置", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LogHelper.Log.Info("【发送命令，未接收到反馈，请检查串口配置！】");
                    }
                }
            });
            task.Start();
        }
        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte[] receiveData = new byte[serialPort.BytesToRead];
                int readCount = serialPort.Read(receiveData, 0, receiveData.Length);
                status_receivecount.Text = readCount + "";
                if (readCount < 1)
                    return;
                MessageDelegate myDelegate = new MessageDelegate(ShowData);
                //ShowData(receiveData);
                this.BeginInvoke(myDelegate, new object[] { receiveData });
            }
            else
            {
                MessageBox.Show("请打开某个串口", "错误提示");
            }
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="data"></param>
        private void ShowData(byte[] data)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte str in data)
            {
                stringBuilder.AppendFormat("{0:X2} ", str);
            }
            string[] hexStr = stringBuilder.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            LogHelper.Log.Info(" 【接收设备返回值HEX】" + stringBuilder.ToString());
            btn_sig_cfg_read.Enabled = true;
            btn_sig_cfg_set.Enabled = true;
            AnalysisReceiveData(hexStr, stringBuilder.ToString());
        }

        private string cacheString;
        private int surPlusLen;
        private void ReturnResult(string[] hexStr,string strRec)
        {
            //FF EE 01 E0 E1 设置命令返回值
            if (hexStr.Length == 5)//接收正常时，为设置配置时的返回数据
            {

                if (hexStr[2].Equals("01"))
                {
                    LogHelper.Log.Info(" 【设置命令】接收设备返回值HEX】" + strRec);
                    MessageBox.Show("SENT配置设置成功！", "提示");
                }
                else if (hexStr[2].Equals("00"))
                {
                    LogHelper.Log.Info(" 【设置命令】接收设备返回值HEX】" + strRec);
                    MessageBox.Show("SENT配置设置失败！", "提示");
                }
            }
            else if (hexStr.Length > 5)
            {
                LogHelper.Log.Info(" 【读取命令】接收设备返回值HEX】" + strRec);
                //更新界面配置
                UpdateReceiveConfig(hexStr);
                //MessageBox.Show("SENT配置更新成功", "提示");
                LogHelper.Log.Info("【Sent 配置更新成功！】");
            }
        }
        private void AnalysisReceiveData(string[] hexStr,string strRev)
        {
            //FF EE 3C F0 01 00 00 10 01 10 00 A2 00 01 F0 00 02 58 00 03 71 00 04 47 00 05 EF 00 06 28 00 07 DE 00 08 E2 00 09 62 00 10 6A 00 11 18 00 12 64 00 13 CE 00 14 A2 00 15 68 00 00 45 03 32 04 01
            //FF EE 分开
            //FF EE未分开
            //长度未分开
            //判断传入数据是否分包接收
            ///接收数据类型：
            ///1、下发指令成功后的返回值，长度固定
            ///2、下发读取指令后接收的配置信息，长度不固定
            ///
            int dataCodeLen;
            if (hexStr[0] == "FF" && hexStr.Length == 1)
            {
                cacheString = strRev;
                LogHelper.Log.Info("返回指令只包含FF，拼接下一包数据！");
                return;
            }
            else if (hexStr[0] == "EE" && hexStr.Length > 1)
            {
                strRev = cacheString + strRev;
                hexStr = strRev.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (hexStr.Length > 2)
                {
                    dataCodeLen = Convert.ToInt32(hexStr[2], 16);
                    if (hexStr.Length - 4 != dataCodeLen)
                    {
                        //不完整
                        surPlusLen = dataCodeLen - (hexStr.Length - 3) + 1;
                        LogHelper.Log.Info("拼接上一包数据只包含FF的情况，本次拼接仍不完整，计算剩余数据长度为"+surPlusLen);
                        return;
                    }
                    else
                    {
                        //接收指令正确
                        LogHelper.Log.Info("拼接上一包数据只包含FF的情况，本地拼接完成！\r\n");
                        ReturnResult(hexStr, strRev);//判断返回结果
                    }
                }
            }
            else if (hexStr[0] == "FF")
            {
                if (hexStr.Length > 2)
                {
                    if (hexStr[1] == "EE")
                    {
                        dataCodeLen = Convert.ToInt32(hexStr[2], 16);
                        if (hexStr.Length - 4 != dataCodeLen)
                        {
                            //不完整
                            surPlusLen = dataCodeLen - (hexStr.Length - 3) + 1;
                            LogHelper.Log.Info("FF EE第一包数据不完整，待下一包数据拼接；剩余数据长度为"+surPlusLen);
                            //下一包接收长度（剩余长度） = 计算数据理论长度-(实际数据长度=接收总长度-3)+1
                            cacheString = strRev;
                            return;
                        }
                        else
                        {
                            //接收指令正确
                            LogHelper.Log.Info("FF EE 第一包数据信息完整！\r\n");
                            ReturnResult(hexStr, strRev);//判断返回结果
                        }
                    }
                }
            }
            else if (hexStr.Length == surPlusLen)
            {
                strRev = cacheString + strRev;
                hexStr = strRev.Split(new char[] { ' '},StringSplitOptions.RemoveEmptyEntries);
                if (hexStr[0] == "FF")
                {
                    if (hexStr.Length > 2)
                    {
                        if (hexStr[1] == "EE")
                        {
                            dataCodeLen = Convert.ToInt32(hexStr[2], 16);
                            if (hexStr.Length - 4 != dataCodeLen)
                            {
                                //不完整
                                surPlusLen = dataCodeLen - (hexStr.Length - 3) + 1;
                                LogHelper.Log.Info("FF EE+下一包数据拼接信息不完整，剩余数据长度为"+surPlusLen);
                                return;
                            }
                            else
                            {
                                //接收指令正确
                                LogHelper.Log.Info("FF EE+下一包数据拼接信息正确！\r\n");
                                ReturnResult(hexStr, strRev);//判断返回结果
                            }
                        }
                    }
                }
            }
            else
            {
                return;
            }
        }
        #endregion

        #region 解析接收的配置数据
        /// <summary>
        /// 读取下位机配置后，更新界面参数
        /// 十进制与十六进制不同情况的转换更新
        /// 接收数据已转为十六进制
        /// </summary>
        /// <param name="strRes"></param>
        private void UpdateReceiveConfig(string[] strRes)
        {
            //FF EE 0C F0 01 01 01 01 10 00 0F 0E 01 00 00 2E 
            //FF EE 0F F0 (01 00 00 10 01) (01 -F4 39 04) 00 23 01 32 04 9D
            //头2+长度(服务码+数据)1+服务码1（原服务码+40）+数据+校验和1（长度+服务码+数据）
            //数据：基本信息5+慢消息组数1+慢消息3*n+快消息5
            try
            {
                if (strRes.Length < 10)
                {
                    LogHelper.Log.Info("慢消息接收异常！");
                    return;
                }
                int dataCodeLen = Convert.ToInt32(strRes[2], 16);
                string serverCode = strRes[3];
                //FF EE 54 F0 00 00 00 26 01 18 01 00
                #region 基础信息
                if (strRes[4] == "00")
                {
                    cob_dataframe_type.SelectedIndex = 0;
                }
                else if (strRes[4] == "01")
                {
                    cob_dataframe_type.SelectedIndex = 1;
                }
                if (strRes[5] == "00")
                {
                    cob_battery_state.SelectedIndex = 0;
                }
                else if (strRes[5] == "01")
                {
                    cob_battery_state.SelectedIndex = 1;
                }
                if (strRes[6] == "00")
                {
                    cob_serial_msg.SelectedIndex = 0;
                }
                else if (strRes[6] == "01")
                {
                    cob_serial_msg.SelectedIndex = 1;
                }
                if (rdb_dec.CheckState == CheckState.Checked)
                {
                    tbx_timeframe.Text = ConvertString.ConvertToDec(strRes[8] + strRes[7]);
                }
                else if (rdb_hex.CheckState == CheckState.Checked)
                {
                    tbx_timeframe.Text = "0X" + strRes[8] + strRes[7];
                }

                #endregion

                #region 慢消息配置
                //len = datacodelen - 1-5-5
                int j = 10;
                cob_group_num.Text = "0X"+strRes[9];
                groupNum = int.Parse(ConvertString.ConvertToDec(strRes[9]));
                DataTable dt = new DataTable();
                if (rdb_hex.CheckState == CheckState.Checked)
                {
                    dt = signalConfig.GetDataTatableHex;
                }
                else if (rdb_dec.CheckState == CheckState.Checked)
                {
                    dt = signalConfig.GetDataTableDec;
                }
                dgv_groupdata.BeginUpdate();
                for (int i = 0; i < groupNum; i++)
                {
                    DataRow dr = dt.NewRow();
                    if (rdb_dec.CheckState == CheckState.Checked)
                    {
                        dr[SignalConfig.GROUP_ORDER] = i + 1;
                        dr[SignalConfig.GROUP_ID_DEC] = ConvertString.ConvertToDec(strRes[j]);
                        dr[SignalConfig.GROUP_DATA_DEC] = ConvertString.ConvertToDec(strRes[j + 2] + strRes[j + 1]);
                    }
                    else if (rdb_hex.CheckState == CheckState.Checked)
                    {
                        dr[SignalConfig.GROUP_ORDER] = i + 1;
                        dr[SignalConfig.GROUP_ID_HEX] = strRes[j];
                        dr[SignalConfig.GROUP_DATA_HEX] = strRes[j + 2] + strRes[j + 1];
                    }

                    dt.Rows.Add(dr);
                    j += 3;
                }
                dgv_groupdata.DataSource = dt;
                dgv_groupdata.EndUpdate();
                #endregion

                #region 快消息配置
                int startSlowIndex = 2 + 1 + dataCodeLen - 5;
                if (strRes[startSlowIndex] == "00")
                {
                    cob_quicksig_type.SelectedIndex = 0;
                }
                else if (strRes[startSlowIndex] == "01")
                {
                    cob_quicksig_type.SelectedIndex = 1;
                }
                int quicksigdata1 = Convert.ToInt32((strRes[startSlowIndex + 2] + strRes[startSlowIndex + 1]).Trim(), 16);
                int quicksigdata2 = Convert.ToInt32((strRes[startSlowIndex + 4] + strRes[startSlowIndex + 3]).Trim(), 16);

                if (rdb_hex.CheckState == CheckState.Checked)
                {
                    tbx_quicksig_data1.Text = Convert.ToString(quicksigdata1, 16);
                    tbx_quicksig_data2.Text = Convert.ToString(quicksigdata2, 16);
                }
                else if (rdb_dec.CheckState == CheckState.Checked)
                {
                    tbx_quicksig_data1.Text = quicksigdata1 + "";
                    tbx_quicksig_data2.Text = quicksigdata2 + "";
                }

                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error("刷新参数失败！"+ex.Message+ex.StackTrace);
            }
        }
        #endregion

        #region 窗口关闭
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SerialClose();
        }
        #endregion

        #region 初始化串口
        /// <summary>
        /// 初始化串口
        /// </summary>
        private void InitSerial()
        {
            ledControl1.LEDSwitch = false;
            ledControl1.Invalidate();
            serialPort = new SerialPort();
            BufferData = new List<byte>();//串口数据数据帧识别缓存空间
            try
            {
                foreach (string item in SerialPort.GetPortNames())
                {
                    cb_port.Items.Add(item);
                }
                if (cb_port.Items.Count > 0)
                {
                    cb_port.SelectedIndex = 0;
                }
                //波特率
                int[] baud = { 115200, 57600, 38400, 28800, 19200, 14400, 9600, 4800, 2400, 1200 };
                for (int i = 0; i < 10; i++)
                {
                    cb_baud.Items.Add(baud[i]);
                }
                cb_baud.SelectedIndex = 0;
                //校验位
                foreach (string item in Enum.GetNames(typeof(Parity)))
                {
                    cb_check.Items.Add(item);
                }
                cb_check.SelectedIndex = 0;
                //数据位
                for (int i = 8; i > 3; i--)
                {
                    cb_data.Items.Add(i);
                }
                cb_data.SelectedIndex = 0;
                //停止位
                foreach (string item in Enum.GetNames(typeof(StopBits)))
                {
                    cb_stop.Items.Add(item);
                }
                cb_stop.SelectedIndex = 1;
                ///流控
                foreach (string item in Enum.GetNames(typeof(Handshake)))
                {
                    cb_handshake.Items.Add(item);
                }
                cb_handshake.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
            cb_baud.Enabled = false;
            cb_check.Enabled = false;
            cb_data.Enabled = false;
            cb_stop.Enabled = false;
            cb_handshake.Enabled = false;
        }
        #endregion

        #region 点击打开串口
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_open_Click(object sender, EventArgs e)
        {
            if (IsSerialOpen == false)
            {
                if (SerialOpen())
                {
                    IsSerialOpen = true;
                    btn_open.Text = "关闭";
                    if (cbx_recon.Checked)
                        timerReCon.Enabled = true;
                    tool_dev_status.Text = "OnLine";
                }
                else
                {
                    tool_dev_status.Text = "OffLine";
                    MessageBox.Show("串口打开失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else if (IsSerialOpen == true)
            {
                if (SerialClose())
                {
                    IsSerialOpen = false;
                    btn_open.Text = "打开";
                    timerReCon.Enabled = false;
                    tool_dev_status.Text = "OffLine";
                }
                else
                {
                    MessageBox.Show("串口关闭失败！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            status_receivecount.Text = "0";
        }
        #endregion

        #region 打开串口函数
        /// <summary>
        /// 打开串口
        /// </summary>
        private bool SerialOpen()
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    ///串口参数设置
                    //串口号
                    if (string.IsNullOrEmpty(cb_port.Text))
                    {
                        LogHelper.Log.Info("串口未选择......");
                        return false;
                    }
                    serialPort.PortName = cb_port.SelectedItem.ToString();
                    //串口设置
                    serialPort.BaudRate = Convert.ToInt32(cb_baud.Text);//波特率
                    serialPort.DataBits = Convert.ToInt32(cb_data.Text);//数据位
                    serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cb_stop.Text);
                    serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), cb_check.Text);
                    serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), cb_handshake.Text);

                    serialPort.ReadTimeout = 500;
                    serialPort.WriteTimeout = 500;
                    serialPort.Open();
                    //设置必要控件不可用
                    cb_port.Enabled = false;
                    cb_data.Enabled = false;
                    cb_baud.Enabled = false;
                    cb_check.Enabled = false;
                    cb_stop.Enabled = false;
                    //指示灯亮
                    ledControl1.LEDSwitch = true;
                    ledControl1.Invalidate();
                    tool_dev_status.Text = "OnLine";
                }
                return true;
            }
            catch (Exception Err)
            {
                LogHelper.Log.Error(Err.Message+Err.StackTrace);
                return false;
            }
        }
        #endregion

        #region 下发sent设备时，判断串口配置是否正确
        /// <summary>
        /// 下发sent设备时，判断串口配置是否正确
        /// </summary>
        private void TestSerialConfig()
        {
            bool IsRight = false;
            if (serialPort.BaudRate != 115200)
            {
                SerialClose();
                serialPort.BaudRate = 115200;
                IsRight = true;
            }
            if (serialPort.DataBits != 8)
            {
                SerialClose();
                serialPort.DataBits = 8;
                IsRight = true;
            }
            if (serialPort.Parity != Parity.None)
            {
                SerialClose();
                serialPort.Parity = Parity.None;
                IsRight = true;
            }
            if (serialPort.StopBits != StopBits.One)
            {
                SerialClose();
                serialPort.StopBits = StopBits.One;
                IsRight = true;
            }
            if (IsRight)
                SerialOpen();
        }
        #endregion

        #region 关闭串口函数
        /// <summary>
        /// 关闭串口
        /// </summary>
        private bool SerialClose()
        {
            try
            {
                serialPort.Close();
                ledControl1.LEDSwitch = false;
                ledControl1.Invalidate();

                cb_port.Enabled = true;
                cb_data.Enabled = true;
                cb_baud.Enabled = true;
                cb_check.Enabled = true;
                cb_stop.Enabled = true;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                LogHelper.Log.Info("关闭串口失败！" + ex.Message);
                return false;
            }
        }
        #endregion

        #region 发送数据
        /// <summary>
        /// 串口数据发送处理
        /// </summary>
        /// <param name="InputString"></param>
        private void SendDevConfigMsg(string[] strArray)
        {
            ///发送hex格式 
            try
            {
                //发送指令前校验串口配置
                TestSerialConfig();
                byte[] cmdSendMsg = comandCfg.HexToByte(strArray);
                status_sendcount.Text = cmdSendMsg.Length.ToString();

                if (cmdSendMsg.Length < 1)
                {
                    MessageBox.Show("发送数据为空", "提示");
                }
                else
                {
                    if (serialPort.IsOpen)
                    {
                        serialPort.Write(cmdSendMsg, 0, cmdSendMsg.Length);
                        LogHelper.Log.Info("【发送指令成功！】" + BitConverter.ToString(cmdSendMsg));
                        IsSendMsg = true;
                    }
                    else
                    {
                        IsSendMsg = false;
                        MessageBox.Show("串口未打开！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info(ex.Message);
                MessageBox.Show(ex.Message, "错误");
            }
        }
        #endregion

        #region hex转byte
        //16进制字符串到byte字节类型的转换,形式是没有0x的
        private byte[] HexStringToByte(string InString)
        {
            if (InString.Length % 2 != 0)
                InString = InString + "0";
            byte[] buffer = new byte[InString.Length / 2];
            for (int i = 0; i < InString.Length; i += 2)
            {
                buffer[i / 2] = (byte)Convert.ToByte(InString.Substring(i, 2), 16);
            }
            return buffer;
        }
        #endregion

        #region 字符串十六进制/十进制互转
        /// <summary>
        /// 字符串十六进制/十进制互相转换
        /// </summary>
        private void ConvertInputString()
        {
            try
            {
                DataTable dxt = signalConfig.GetDataTatableHex;
                DataTable dct = signalConfig.GetDataTableDec;

                if (rdb_hex.CheckState == CheckState.Checked)
                {
                    //转换为十六进制
                    lbx_ticksLimit.Text = SignalConfig.TICKS_LIMIT_HEX;
                    lbx_quickd1_limit.Text = SignalConfig.QUICK_DATA1_LIMIT_HEX;
                    lbx_quickd2_limit.Text = SignalConfig.QUICK_DATA2_LIMIT_HEX;
                    lbx_group_count_limit.Text = SignalConfig.SLOW_GROUP_COUNT_LIMIT_HEX;

                    tbx_timeframe.Text = ConvertString.ConvertToHex(tbx_timeframe.Text,4);
                    tbx_quicksig_data1.Text = ConvertString.ConvertToHex(tbx_quicksig_data1.Text,4);
                    tbx_quicksig_data2.Text = ConvertString.ConvertToHex(tbx_quicksig_data2.Text,4);
                    cob_group_num.Text = ConvertString.ConvertToHex(cob_group_num.Text,2);
                    SignalConfig.ItemValueToHex(cob_group_num);
                    dxt.Clear();
                    for (int i = 0; i < groupNum; i++)
                    {
                        DataRow dr = dxt.NewRow();
                        dr[SignalConfig.GROUP_ORDER] = i + 1;
                        dr[SignalConfig.GROUP_ID_HEX] = ConvertString.ConvertToHex(dgv_groupdata.Rows[i].Cells[1].Value.ToString(),2);
                        dr[SignalConfig.GROUP_DATA_HEX] = ConvertString.ConvertToHex(dgv_groupdata.Rows[i].Cells[2].Value.ToString(),4);
                        dxt.Rows.Add(dr);
                    }
                    dgv_groupdata.DataSource = dxt;
                    dgv_groupdata.EndUpdate();
                }
                else if (rdb_dec.CheckState == CheckState.Checked)
                {
                    //转换为十进制
                    lbx_ticksLimit.Text = SignalConfig.TICKS_LIMIT_DEC;
                    lbx_quickd1_limit.Text = SignalConfig.QUICK_DATA1_LIMIT_DEC;
                    lbx_quickd2_limit.Text = SignalConfig.QUICK_DATA2_LIMIT_DEC;
                    lbx_group_count_limit.Text = SignalConfig.SLOW_GROUP_COUNT_LIMIT_DEC;

                    tbx_timeframe.Text = ConvertString.ConvertToDec(tbx_timeframe.Text);
                    tbx_quicksig_data1.Text = ConvertString.ConvertToDec(tbx_quicksig_data1.Text);
                    tbx_quicksig_data2.Text = ConvertString.ConvertToDec(tbx_quicksig_data2.Text);
                    SignalConfig.ItemValueToDec(cob_group_num);
                    cob_group_num.Text = ConvertString.ConvertToDec(cob_group_num.Text);
                    dct.Clear();
                    for (int i = 0; i < groupNum; i++)
                    {
                        DataRow dr = dct.NewRow();
                        dr[SignalConfig.GROUP_ORDER] = i + 1;
                        dr[SignalConfig.GROUP_ID_DEC] = ConvertString.ConvertToDec(dgv_groupdata.Rows[i].Cells[1].Value.ToString());
                        dr[SignalConfig.GROUP_DATA_DEC] = ConvertString.ConvertToDec(dgv_groupdata.Rows[i].Cells[2].Value.ToString());
                        dct.Rows.Add(dr);
                    }
                    dgv_groupdata.DataSource = dct;
                    dgv_groupdata.EndUpdate();
                }

                dgv_groupdata.Columns[0].BestFit();
            }
            catch (Exception ex)
            {
                LogHelper.Log.Info("转换hex/dec格式错误!"+ex.Message+"\r\n"+ex.StackTrace+" num:"+groupNum);
            }
        }
        #endregion

        #region 信号配置/event
        /// <summary>
        /// 信号配置 / event
        /// </summary>
        private void InitSentControlConfig()
        {
            //splitContainer1.FixedPanel = FixedPanel.Panel1;
            rdb_hex.IsChecked = true;

            signalConfig.InitSlowSignal(cob_group_num,rdb_hex, 50);
            signalConfig.InitBaseSignal(cob_serial_msg, cob_battery_state, cob_dataframe_type, tbx_timeframe);
            signalConfig.InitQuickSignal(cob_quicksig_type,chx_hex_order,lbx_hexorder_notes);
            if (rdb_dec.CheckState == CheckState.Checked)
            {
                tbx_quicksig_data1.Text = "01";
                tbx_quicksig_data2.Text = "02";
            }
            else if (rdb_hex.CheckState == CheckState.Checked)
            {
                tbx_quicksig_data1.Text = "0X01";
                tbx_quicksig_data2.Text = "0X02";
            }

            signalConfig.AddGridViewDataSource();
            if (rdb_hex.CheckState == CheckState.Checked)
            {
                groupNum = int.Parse(ConvertString.ConvertToDec(cob_group_num.Text));
            }
            else
            {
                groupNum = int.Parse(cob_group_num.Text.Trim());
            }
            cob_group_num.SelectedIndexChanged += Cob_group_num_SelectedIndexChanged;
            cob_group_num.TextChanged += Cob_group_num_TextChanged;
            cob_dataframe_type.TextChanged += Cob_dataframe_type_TextChanged;
            cob_battery_state.TextChanged += Cob_battery_state_TextChanged;
            cob_serial_msg.TextChanged += Cob_serial_msg_TextChanged;
            tbx_timeframe.TextChanged += Tbx_timeframe_TextChanged;
            cob_quicksig_type.TextChanged += Cob_quicksig_type_TextChanged;
            tbx_quicksig_data1.TextChanged += Tbx_quicksig_data1_TextChanged;
            tbx_quicksig_data2.TextChanged += Tbx_quicksig_data2_TextChanged;
            rdb_hex.CheckStateChanged += Rdb_hex_CheckStateChanged;
            rdb_dec.CheckStateChanged += Rdb_dec_CheckStateChanged;

            dgv_groupdata.ValueChanged += Dgv_groupdata_ValueChanged;
            dgv_groupdata.CellBeginEdit += Dgv_groupdata_CellBeginEdit;
            dgv_groupdata.CellEndEdit += Dgv_groupdata_CellEndEdit;
            cbx_recon.CheckedChanged += Cbx_recon_CheckedChanged;
            dgv_groupdata.RowsChanged += Dgv_groupdata_RowsChanged;
            dgv_groupdata.ContextMenuOpening += Dgv_groupdata_ContextMenuOpening;
        }

        #region sent 配置值发生改变
        #region radgridview contextmenu event
        /// <summary>
        /// radgridview右键菜单事件处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dgv_groupdata_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            for (int i = 0; i < e.ContextMenu.Items.Count; i++)
            {
                String contextMenuText = e.ContextMenu.Items[i].Text;
                switch (contextMenuText)
                {
                    case "Conditional Formatting":
                        e.ContextMenu.Items[i].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                        e.ContextMenu.Items[i + 1].Visibility = ElementVisibility.Collapsed;
                        break;
                    case "Hide Column":
                        e.ContextMenu.Items[i].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                        break;
                    case "Pinned state":
                        e.ContextMenu.Items[i].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                        break;
                    case "Best Fit":
                        e.ContextMenu.Items[i].Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
                        break;
                    case "Cut":
                        e.ContextMenu.Items[i].Click += RadGridViewMenuCutRow_Click;
                        break;
                    case "Copy":
                        break;
                    case "Paste":
                        break;
                    case "Edit":
                        break;
                    case "Clear Value":
                        break;
                    case "Delete Row":
                        e.ContextMenu.Items[i].Click += RadGridViewMenuDeleteRow_Click;
                        break;
                }
            }
        }

        private void RadGridViewMenuCutRow_Click(object sender, EventArgs e)
        {
            RefreshGridViewIDOrder();
        }

        private void RadGridViewMenuDeleteRow_Click(object sender, EventArgs e)
        {
            RefreshGridViewIDOrder();
        }

        private void RefreshGridViewIDOrder()
        {
            int row = dgv_groupdata.Rows.Count;
            //执行剪切后改变行数，重新排序
            DataTable dt = new DataTable();
            dt.Clear();
            dgv_groupdata.BeginUpdate();
            if (rdb_dec.CheckState == CheckState.Checked)
            {
                dt = signalConfig.GetDataTableDec;
                for (int i = 0; i < row; i++)
                {
                    DataRow dRow = dt.NewRow();
                    dRow.BeginEdit();
                    dRow[SignalConfig.GROUP_ORDER] = i + 1;
                    dRow[SignalConfig.GROUP_ID_DEC] = dgv_groupdata.Rows[i].Cells[1].Value.ToString();
                    dRow[SignalConfig.GROUP_DATA_DEC] = dgv_groupdata.Rows[i].Cells[2].Value.ToString();
                    dRow.EndEdit();
                    dt.Rows.Add(dRow);
                }
            }
            else
            {
                dt = signalConfig.GetDataTatableHex;
                for (int i = 0; i < row; i++)
                {
                    DataRow dRow = dt.NewRow();
                    dRow.BeginEdit();
                    dRow[SignalConfig.GROUP_ORDER] = i + 1;
                    dRow[SignalConfig.GROUP_ID_HEX] = dgv_groupdata.Rows[i].Cells[1].Value.ToString();
                    dRow[SignalConfig.GROUP_DATA_HEX] = dgv_groupdata.Rows[i].Cells[2].Value.ToString();
                    dRow.EndEdit();
                    dt.Rows.Add(dRow);
                }
            }

            dgv_groupdata.DataSource = dt;
            dgv_groupdata.EndUpdate();
        }
        #endregion

        #region radgridview 行数改变时，id自增
        private bool IsEditEnd;
        private int curRowCount;
        private bool IsUpdateGroupCount;//手动添加行时,更新组数

        private void Dgv_groupdata_CellEndEdit(object sender, GridViewCellEventArgs e)
        {
            IsEditEnd = true;
            curRowCount = dgv_groupdata.Rows.Count;
        }

        private void Dgv_groupdata_CellBeginEdit(object sender, GridViewCellCancelEventArgs e)
        {
            IsEditEnd = false;
        }

        private void Dgv_groupdata_RowsChanged(object sender, Telerik.WinControls.UI.GridViewCollectionChangedEventArgs e)
        {
            GridViewTemplate m = e.GridViewTemplate;
            int row = m.Rows.Count;
            if (IsEditEnd)
            {
                //编辑完成
                if (curRowCount < row)
                {
                    //更新ID
                    string tempId = "0";
                    if (row == 1)
                    {
                        tempId = "1";
                    } else
                    {
                        tempId = (int.Parse(dgv_groupdata.Rows[row - 2].Cells[0].Value.ToString()) + 1) + "";
                    }
                    dgv_groupdata.Rows[row - 1].Cells[0].Value = tempId;
                    //同时更新组数
                    IsUpdateGroupCount = true;
                    if (rdb_dec.CheckState == CheckState.Checked)
                    {
                        cob_group_num.Text = row + "";
                    }
                    else
                    {
                        cob_group_num.Text = ConvertString.ConvertToHex(row+"",2);
                    }
                }
            }
        }
        #endregion
        private void Rdb_dec_CheckStateChanged(object sender, EventArgs e)
        {
            if (rdb_dec.CheckState == CheckState.Checked)
            {
                ConvertInputString();
            }
        }

        private void Rdb_hex_CheckStateChanged(object sender, EventArgs e)
        {
            if (rdb_hex.CheckState == CheckState.Checked)
            {
                ConvertInputString();
            }
        }
        private void Cbx_recon_CheckedChanged(object sender, EventArgs e)
        {
            if (cbx_recon.Checked)
            {
                timerReCon.Enabled = true;
            }
            else
            {
                timerReCon.Enabled = false;
            }
        }

        private void Dgv_groupdata_ValueChanged(object sender, EventArgs e)
        {
            IsSentCfgChanging = true;
        }

        private void Tbx_quicksig_data2_TextChanged(object sender, EventArgs e)
        {
            IsSentCfgChanging = true;
        }

        private void Tbx_quicksig_data1_TextChanged(object sender, EventArgs e)
        {
            IsSentCfgChanging = true;
        }

        private void Tbx_timeframe_TextChanged(object sender, EventArgs e)
        {
            IsSentCfgChanging = true;
        }

        private void Cob_quicksig_type_TextChanged(object sender, EventArgs e)
        {
            IsSentCfgChanging = true;
            if (cob_quicksig_type.SelectedIndex == 0)
            {
                chx_hex_order.Visible = true;
                lbx_hexorder_notes.Visible = true;
            }
            else if (cob_quicksig_type.SelectedIndex == 1)
            {
                chx_hex_order.Visible = false;
                lbx_hexorder_notes.Visible = false;
            }
        }

        private void Cob_serial_msg_TextChanged(object sender, EventArgs e)
        {
            IsSentCfgChanging = true;
        }

        private void Cob_battery_state_TextChanged(object sender, EventArgs e)
        {
            IsSentCfgChanging = true;
        }

        private void Cob_dataframe_type_TextChanged(object sender, EventArgs e)
        {
            IsSentCfgChanging = true;
        }

        #region 组数事件
        /// <summary>
        /// 组数文本改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cob_group_num_TextChanged(object sender, EventArgs e)
        {
            if (IsUpdateGroupCount)
            {
                IsUpdateGroupCount = false;
                return;//正在更新组数
            }
            GroupCountChange();
            IsSentCfgChanging = true;
        }

        /// <summary>
        /// 组数选择改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cob_group_num_SelectedIndexChanged(object sender, EventArgs e)
        {
            GroupCountChange();
        }
        #endregion

        #region 组数变化，更新数据显示
        /// <summary>
        /// 组数输入改变
        /// </summary>
        private void GroupCountChange()
        {
            string groupnum = cob_group_num.Text.ToLower().Replace("0x", "");
            if (string.IsNullOrEmpty(groupnum))
                return;
            bool IsHex = true;

            if (rdb_hex.CheckState == CheckState.Checked)
            {
                IsHex = true;
                groupnum = ConvertString.ConvertToDec(groupnum);
                if (int.Parse(groupnum) > (int)SignalConfig.InputLimit.SLOW_GROUP_COUNT_MAX)
                {
                    cob_group_num.ForeColor = Color.Red;
                    MessageBox.Show("请输入小于或等于0X32(50)的组数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (int.Parse(groupnum) < 0)
                {
                    cob_group_num.ForeColor = Color.Red;
                    MessageBox.Show("请输入大于或等于0的组数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    cob_group_num.ForeColor = Color.Black;
                }
            }
            else if (rdb_dec.CheckState == CheckState.Checked)
            {
                IsHex = false;
                if (int.Parse(groupnum) > (int)SignalConfig.InputLimit.SLOW_GROUP_COUNT_MAX)
                {
                    cob_group_num.ForeColor = Color.Red;
                    MessageBox.Show("请输入小于或等于50的组数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else if (int.Parse(groupnum) < 0)
                {
                    cob_group_num.ForeColor = Color.Red;
                    MessageBox.Show("请输入大于或等于0的组数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                else
                {
                    cob_group_num.ForeColor = Color.Black;
                }
            }
            if (groupnum.Equals(groupNum+""))
                return;
            int groupNumTemp;
            if (ExamineInputFormat.IsDecimal(groupnum))
            {
                if (!int.TryParse(groupnum, out groupNumTemp))
                {
                    MessageBox.Show("格式转换错误");
                }
                if (groupNumTemp > groupNum)
                {
                    signalConfig.AddGridViewDataSource(groupNumTemp - groupNum, dgv_groupdata, IsHex);
                    groupNum = groupNumTemp;
                }
            }
        }
        #endregion

        #endregion

        #endregion

        #region 下发配置设置命令
        /// <summary>
        /// 发送配置命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_sig_cfg_set_Click(object sender, EventArgs e)
        {
            btn_sig_cfg_set.Enabled = false;
            SendWriteConfig();
        }
        private void Menu_sent_cfg_write_Click(object sender, EventArgs e)
        {
            btn_sig_cfg_set.Enabled = false;
            SendWriteConfig();
        }
        private void SendWriteConfig()
        {
            if (!JudgeInputLimit())
                return;
            SignalConfig.InputStringType inputType = SignalConfig.InputStringType.HEX;
            if (rdb_hex.CheckState == CheckState.Checked)
                inputType = SignalConfig.InputStringType.HEX;
            else if (rdb_dec.CheckState == CheckState.Checked)
                inputType = SignalConfig.InputStringType.DEC;
            comandCfg.CalInitParams(groupNum,inputType);
            if (!comandCfg.UnionBaseCommand(cob_dataframe_type, cob_battery_state, cob_serial_msg, tbx_timeframe))
                return;
            if (!comandCfg.UnionSlowCommand(dgv_groupdata, cob_group_num.Text.Trim()))
                return;
            if (!comandCfg.UnionQuickCommand(cob_quicksig_type, tbx_quicksig_data1, tbx_quicksig_data2,chx_hex_order))
                return;
            //下发命令
            SendDevConfigMsg(comandCfg.SetCfgHexByte());
        }
        #endregion

        #region 判断输入合法
        /// <summary>
        /// 判断输入限制，判断输入合法
        /// </summary>
        /// <returns></returns>
        private bool JudgeInputLimit()
        {
            try
            {
                if (string.IsNullOrEmpty(tbx_timeframe.Text))
                {
                    tbx_timeframe.ForeColor = Color.Red;
                    MessageBox.Show("ticks个数不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrEmpty(tbx_quicksig_data1.Text))
                {
                    tbx_quicksig_data1.ForeColor = Color.Red;
                    MessageBox.Show("data1不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrEmpty(tbx_quicksig_data2.Text))
                {
                    tbx_quicksig_data2.ForeColor = Color.Red;
                    MessageBox.Show("data2不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (string.IsNullOrEmpty(cob_group_num.Text))
                {
                    cob_group_num.ForeColor = Color.Red;
                    MessageBox.Show("组数不能为空！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                if (rdb_dec.CheckState == CheckState.Checked)
                {
                    #region 一帧时间ticks个数
                    if (ExamineInputFormat.IsDecimal(tbx_timeframe.Text))
                    {
                        if (int.Parse(tbx_timeframe.Text) < (int)SignalConfig.InputLimit.BASE_TICKS_MIN)
                        {
                            tbx_timeframe.ForeColor = Color.Red;
                            MessageBox.Show("请输入大于或等于272的ticks个数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else if (int.Parse(tbx_timeframe.Text.Trim()) > (int)SignalConfig.InputLimit.BASE_TICKS_MAX)
                        {
                            tbx_timeframe.ForeColor = Color.Red;
                            MessageBox.Show("请输入小于或等于4095的ticks个数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else
                        {
                            tbx_timeframe.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        tbx_timeframe.ForeColor = Color.Red;
                        MessageBox.Show("请输入十进制ticks数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    #endregion

                    #region 快信号数据
                    if (ExamineInputFormat.IsDecimal(tbx_quicksig_data1.Text))
                    {
                        if (int.Parse(tbx_quicksig_data1.Text.Trim()) > (int)SignalConfig.InputLimit.QUICK_DATA_MAX)
                        {
                            tbx_quicksig_data1.ForeColor = Color.Red;
                            MessageBox.Show("请输入小于或等于4095的data1！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else if (int.Parse(tbx_quicksig_data1.Text.Trim()) < 0)
                        {
                            tbx_quicksig_data1.ForeColor = Color.Red;
                            MessageBox.Show("请输入大于或等于0的data1！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else
                        {
                            tbx_quicksig_data1.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        tbx_quicksig_data1.ForeColor = Color.Red;
                        MessageBox.Show("请输入十进制data1数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    if (ExamineInputFormat.IsDecimal(tbx_quicksig_data2.Text))
                    {
                        if (int.Parse(tbx_quicksig_data2.Text.Trim()) > (int)SignalConfig.InputLimit.QUICK_DATA_MAX)
                        {
                            tbx_quicksig_data1.ForeColor = Color.Red;
                            MessageBox.Show("请输入小于或等于4095的data2！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else if (int.Parse(tbx_quicksig_data2.Text.Trim()) < 0)
                        {
                            tbx_quicksig_data2.ForeColor = Color.Red;
                            MessageBox.Show("请输入大于或等于0的data2！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else
                        {
                            tbx_quicksig_data2.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        tbx_quicksig_data2.ForeColor = Color.Red;
                        MessageBox.Show("请输入十进制data2数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    #endregion

                    #region 慢信号
                    if (ExamineInputFormat.IsDecimal(cob_group_num.Text))
                    {
                        if (int.Parse(cob_group_num.Text) > (int)SignalConfig.InputLimit.SLOW_GROUP_COUNT_MAX)
                        {
                            cob_group_num.ForeColor = Color.Red;
                            MessageBox.Show("请输入小于或等于50的组数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else if (int.Parse(cob_group_num.Text) < 0)
                        {
                            cob_group_num.ForeColor = Color.Red;
                            MessageBox.Show("请输入大于或等于0的组数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else
                        {
                            cob_group_num.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        cob_group_num.ForeColor = Color.Red;
                        MessageBox.Show("请输入十进制组数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    for (int i = 0; i < dgv_groupdata.Rows.Count; i++)
                    {
                        string groupID = dgv_groupdata.Rows[i].Cells[1].Value.ToString();
                        string groupData = dgv_groupdata.Rows[i].Cells[2].Value.ToString();

                        if (string.IsNullOrEmpty(groupID))
                        {
                            dgv_groupdata.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                            MessageBox.Show("第"+(i+1)+"行2列id不能为空","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return false;
                        }
                        if (string.IsNullOrEmpty(groupData))
                        {
                            dgv_groupdata.Rows[i].Cells[2].Style.ForeColor = Color.Red;
                            MessageBox.Show("第" + (i + 1) + "行3列数据不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }

                        #region id
                        if (ExamineInputFormat.IsDecimal(groupID))
                        {
                            if (int.Parse(groupID) > (int)SignalConfig.InputLimit.SLOW_GROUP_ID_MAX)
                            {
                                dgv_groupdata.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                                MessageBox.Show("请在" + (i + 1) + "行" + 2 + "列输入小于或等于255的ID!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            else if (int.Parse(groupID) < 0)
                            {
                                dgv_groupdata.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                                MessageBox.Show("请在" + (i + 1) + "行" + 2 + "输入大于或等于0的ID!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            else
                            {
                                dgv_groupdata.Rows[i].Cells[1].Style.ForeColor = Color.Black;
                            }
                        }
                        else
                        {
                            dgv_groupdata.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                            MessageBox.Show("请在"+(i+1)+"行"+2+"列输入正确的十进制ID!","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                            return false;
                        }
                        #endregion

                        #region data1
                        if (ExamineInputFormat.IsDecimal(groupData))
                        {
                            if (int.Parse(groupData) > (int)SignalConfig.InputLimit.SLOW_GROUP_DATA_MAX)
                            {
                                dgv_groupdata.Rows[i].Cells[2].Style.ForeColor = Color.Red;
                                MessageBox.Show("请在" + (i + 1) + "行" + 3 + "列输入小于或等于4095的十进制数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            else if (int.Parse(groupData) < 0)
                            {
                                dgv_groupdata.Rows[i].Cells[2].Style.ForeColor = Color.Red;
                                MessageBox.Show("请在" + (i + 1) + "行" + 3 + "列输入大于或等于0的十进制数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            else
                            {
                                dgv_groupdata.Rows[i].Cells[2].Style.ForeColor = Color.Black;
                            }
                        }
                        else
                        {
                            dgv_groupdata.Rows[i].Cells[2].Style.ForeColor = Color.Red;
                            MessageBox.Show("请在" + (i + 1) + "行" + 3 + "列输入正确的十进制数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        #endregion
                    }
                    #endregion
                }
                else if (rdb_hex.CheckState == CheckState.Checked)
                {
                    #region 一帧时间ticks个数
                    if (ExamineInputFormat.IsHexadecimal(tbx_timeframe.Text))
                    {
                        int ticks = Convert.ToInt32(tbx_timeframe.Text.Trim().ToLower().Replace("0x",""), 16);
                        if (ticks < (int)SignalConfig.InputLimit.BASE_TICKS_MIN)
                        {
                            tbx_timeframe.ForeColor = Color.Red;
                            MessageBox.Show("请输入大于或等于0X0110(272)的ticks个数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else if (ticks > (int)SignalConfig.InputLimit.BASE_TICKS_MAX)
                        {
                            tbx_timeframe.ForeColor = Color.Red;
                            MessageBox.Show("请输入小于或等于0X0FFF(4095)的ticks个数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else
                        {
                            tbx_timeframe.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        tbx_timeframe.ForeColor = Color.Red;
                        MessageBox.Show("请输入十六进制ticks数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    #endregion

                    #region 快信号数据
                    if (ExamineInputFormat.IsHexadecimal(tbx_quicksig_data1.Text))
                    {
                        int data1 = Convert.ToInt32(tbx_quicksig_data1.Text.Trim().ToLower().Replace("0x",""),16);
                        if (data1 > (int)SignalConfig.InputLimit.QUICK_DATA_MAX)
                        {
                            tbx_quicksig_data1.ForeColor = Color.Red;
                            MessageBox.Show("请输入小于或等于0X0FFF(4095)的data1！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else if (data1 < 0)
                        {
                            tbx_quicksig_data1.ForeColor = Color.Red;
                            MessageBox.Show("请输入大于或等于0的data1！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else
                        {
                            tbx_quicksig_data1.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        tbx_quicksig_data1.ForeColor = Color.Red;
                        MessageBox.Show("请输入十六进制data1数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    if (ExamineInputFormat.IsDecimal(tbx_quicksig_data2.Text))
                    {
                        int data2 = Convert.ToInt32(tbx_quicksig_data2.Text.Trim().ToLower().Replace("0X",""), 16);
                        if (data2 > (int)SignalConfig.InputLimit.QUICK_DATA_MAX)
                        {
                            tbx_quicksig_data1.ForeColor = Color.Red;
                            MessageBox.Show("请输入小于或等于0X0FFF(4095)的data2！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else if (data2 < 0)
                        {
                            tbx_quicksig_data2.ForeColor = Color.Red;
                            MessageBox.Show("请输入大于或等于0的data2！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else
                        {
                            tbx_quicksig_data2.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        tbx_quicksig_data2.ForeColor = Color.Red;
                        MessageBox.Show("请输入十六进制data2数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    #endregion

                    #region 慢信号
                    if (ExamineInputFormat.IsHexadecimal(cob_group_num.Text))
                    {
                        int group_count = Convert.ToInt32(cob_group_num.Text.Trim().ToLower().Replace("0x",""),16);
                        if (group_count > (int)SignalConfig.InputLimit.SLOW_GROUP_COUNT_MAX)
                        {
                            cob_group_num.ForeColor = Color.Red;
                            MessageBox.Show("请输入小于或等于0X32(50)的组数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else if (group_count < 0)
                        {
                            cob_group_num.ForeColor = Color.Red;
                            MessageBox.Show("请输入大于或等于0的组数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        else
                        {
                            cob_group_num.ForeColor = Color.Black;
                        }
                    }
                    else
                    {
                        cob_group_num.ForeColor = Color.Red;
                        MessageBox.Show("请输入十六进制组数！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    for (int i = 0; i < dgv_groupdata.Rows.Count; i++)
                    {
                        string groupID = dgv_groupdata.Rows[i].Cells[1].Value.ToString();
                        string groupData = dgv_groupdata.Rows[i].Cells[2].Value.ToString();


                        if (string.IsNullOrEmpty(groupID))
                        {
                            dgv_groupdata.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                            MessageBox.Show("第" + (i + 1) + "行2列id不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        if (string.IsNullOrEmpty(groupData))
                        {
                            dgv_groupdata.Rows[i].Cells[2].Style.ForeColor = Color.Red;
                            MessageBox.Show("第" + (i + 1) + "行3列data1不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }

                        #region id

                        if (ExamineInputFormat.IsHexadecimal(groupID))
                        {
                            groupID = ConvertString.ConvertToDec(groupID); ;
                            if (int.Parse(groupID) > (int)SignalConfig.InputLimit.SLOW_GROUP_ID_MAX)
                            {
                                dgv_groupdata.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                                MessageBox.Show("请在" + (i + 1) + "行" + 2 + "列输入小于或等于0XFF(255)的ID!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            else if (int.Parse(groupID) < 0)
                            {
                                dgv_groupdata.Rows[i].Cells[1].Style.ForeColor = Color.Red;
                                MessageBox.Show("请在" + (i + 1) + "行" + 2 + "输入大于或等于0的ID!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            else
                            {
                                dgv_groupdata.Rows[i].Cells[1].Style.ForeColor = Color.Black;
                            }
                        }
                        else
                        {
                            dgv_groupdata.Rows[i].Cells[0].Style.ForeColor = Color.Red;
                            MessageBox.Show("请在" + (i + 1) + "行" + 1 + "列输入正确的十六进制ID!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        #endregion

                        #region data1
                        if (ExamineInputFormat.IsHexadecimal(groupData))
                        {
                            groupData = ConvertString.ConvertToDec(groupData);
                            if (int.Parse(groupData) > (int)SignalConfig.InputLimit.SLOW_GROUP_DATA_MAX)
                            {
                                dgv_groupdata.Rows[i].Cells[2].Style.ForeColor = Color.Red;
                                MessageBox.Show("请在" + (i + 1) + "行" + 3 + "列输入小于或等于0XFF(255)的十进制数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            else if (int.Parse(groupData) < 0)
                            {
                                dgv_groupdata.Rows[i].Cells[2].Style.ForeColor = Color.Red;
                                MessageBox.Show("请在" + (i + 1) + "行" + 3 + "列输入大于或等于0的十进制数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
                            }
                            else
                            {
                                dgv_groupdata.Rows[i].Cells[2].Style.ForeColor = Color.Black;
                            }
                        }
                        else
                        {
                            dgv_groupdata.Rows[i].Cells[2].Style.ForeColor = Color.Red;
                            MessageBox.Show("请在" + (i + 1) + "行" + 3 + "列输入正确的十六进制数据!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                        #endregion
                    }
                    #endregion
                }
                return true;
            }
            catch(Exception ex)
            {
                LogHelper.Log.Error("判断输入异常！"+ex.Message+ex.StackTrace);
                return false;
            }
            
        }
        #endregion

        #region 下发配置读取命令
        /// <summary>
        /// 下发配置读取命令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_sig_cfg_read_Click(object sender, EventArgs e)
        {
            //btn_sig_cfg_read.Enabled = false;
            SendReadConfig();
        }

        private void Menu_sent_cfg_read_Click(object sender, EventArgs e)
        {
            //btn_sig_cfg_read.Enabled = false;
            SendReadConfig();
        }

        private void SendReadConfig()
        {
            if (IsSentCfgChanging)
            {
                if (MessageBox.Show("配置已修改，是否保存当前配置", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    userAsPath = FileSelect.SaveAs("ini file|*.ini");
                    SaveSentConfig();
                }
                else
                {
                    IsSentCfgChanging = false;
                }
            }
            SendDevConfigMsg(comandCfg.ReadCfgHexByte());
        }
        #endregion

        #region 初始化Sent配置
        /// <summary>
        /// 初始化sent配置
        /// </summary>
        private void InitSentConfig()
        {
            sentConfig = new SentConfig();
            baseCfg = new SentConfig.BaseSigConfig();
            slowCfg = new SentConfig.SlowSigConfig();
            slowList = new List<SentConfig.SlowSigConfig>();
            quickCfg = new SentConfig.QuickSigConfig();
            sysCfgPath = AppDomain.CurrentDomain.BaseDirectory+"Config\\";
            //读配置信息
            string lastSentPath = INIFile.GetValue(LocalConfig.SENT_PATH_HEAD,LocalConfig.SENT_CONFIG_LAST_PATH,sysCfgPath+LocalConfig.SENT_CONFIG_INI_NAME);
            if (string.IsNullOrEmpty(lastSentPath))
                return;
            if (!File.Exists(lastSentPath))
            {
                LogHelper.Log.Info(" 配置文件被删除或移动 ，文件路径为 "+lastSentPath);
                return;
            }
            LocalConfig.ReadBaseConfig(lastSentPath, baseCfg, slowList, quickCfg, sentConfig);
        }
        #endregion

        #region 更新UI配置参数
        /// <summary>
        /// 更新UI配置参数
        /// </summary>
        private void UpdateUIConfig()
        {
            #region 更新基础信息
            //数据帧类型
            if (baseCfg.DataType == null)
            {
                LogHelper.Log.Info(" UI更新数据帧类型失败...........");
            }
            else
            {
                if (baseCfg.DataType.Equals(SignalConfig.DATA_FRAME_SHORT))
                {
                    cob_dataframe_type.SelectedIndex = 0;
                }
                else if (baseCfg.DataType.Equals(SignalConfig.DATA_FRAME_LONG))
                {
                    cob_dataframe_type.SelectedIndex = 1;
                }
            }
            //电平状态
            if (baseCfg.BatteryState != null)
            {
                if (baseCfg.BatteryState.Equals(SignalConfig.BATTERY_LOW))
                {
                    cob_battery_state.SelectedIndex = 0;
                }
                else if (baseCfg.BatteryState.Equals(SignalConfig.BATTERY_HIGH))
                {
                    cob_battery_state.SelectedIndex = 1;
                }
            }
            else
            {
                LogHelper.Log.Info(" UI更新电平状态失败...........");
            }

            //串行消息
            if (baseCfg.SerialMsg != null)
            {
                if (baseCfg.SerialMsg.Equals(SignalConfig.SERIAL_MESSAGE_12))
                {
                    cob_serial_msg.SelectedIndex = 0;
                }
                else if (baseCfg.SerialMsg.Equals(SignalConfig.SERIAL_MESSAGE_16))
                {
                    cob_serial_msg.SelectedIndex = 1;
                }
            }
            else
            {
                LogHelper.Log.Info(" UI更新串行消息失败...........");
            }
            //帧时间次数
            if (baseCfg.TimeLong != null)
            {
                tbx_timeframe.Text = baseCfg.TimeLong;
            }
            else
            {
                LogHelper.Log.Info(" UI更新帧时间次数失败...........");
            }


            #endregion

            #region 更新慢消息信息
            if (slowList.Count > 0)
            {
                LocalConfig.Storage_Data_Type dataType = sentConfig.StorageDataType;
                string lastGroupCount = slowList[0].GroupCount;
                if (dataType == LocalConfig.Storage_Data_Type.HEX)
                {
                    groupNum = int.Parse(ConvertString.ConvertToDec(lastGroupCount));
                }
                else
                {
                    groupNum = int.Parse(slowList[0].GroupCount);
                }
                DataTable dt = new DataTable();
                if (rdb_dec.CheckState == CheckState.Checked)
                {
                    dt = signalConfig.GetDataTableDec;

                    for (int i = 0; i < slowList.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr[SignalConfig.GROUP_ORDER] = i + 1;
                        dr[SignalConfig.GROUP_ID_DEC] = slowList[i].GroupSerialID;
                        dr[SignalConfig.GROUP_DATA_DEC] = slowList[i].GroupData;
                        dt.Rows.Add(dr);
                    }
                }
                else if (rdb_hex.CheckState == CheckState.Checked)
                {
                    dt = signalConfig.GetDataTatableHex;

                    for (int i = 0; i < slowList.Count; i++)
                    {
                        DataRow dr = dt.NewRow();
                        dr[SignalConfig.GROUP_ORDER] = i + 1;
                        dr[SignalConfig.GROUP_ID_HEX] = slowList[i].GroupSerialID;
                        dr[SignalConfig.GROUP_DATA_HEX] = slowList[i].GroupData;
                        dt.Rows.Add(dr);
                    }
                }
                
                if (groupNum > 0)
                {
                    if (rdb_hex.CheckState == CheckState.Checked)
                    {
                        cob_group_num.Text = lastGroupCount;
                        SignalConfig.ItemValueToHex(cob_group_num);
                    }
                    else if (rdb_dec.CheckState == CheckState.Checked)
                    {
                        SignalConfig.ItemValueToDec(cob_group_num);
                        cob_group_num.Text = slowList[0].GroupCount;
                    }
                }
                dgv_groupdata.DataSource = dt;
                dgv_groupdata.EndUpdate();
            }
            else
            {
                LogHelper.Log.Info(" UI更新慢消息失败...........");
            }
            #endregion

            #region 更新快消息信息
            if (quickCfg.QuickSigType != null)
            {
                if (quickCfg.QuickSigType.Equals(SignalConfig.SIGNAL_TYPE1))
                {
                    cob_quicksig_type.SelectedIndex = 0;
                }
                else if (quickCfg.QuickSigType.Equals(SignalConfig.SIGNAL_TYPE2))
                {
                    cob_quicksig_type.SelectedIndex = 1;
                }
            }
            else
            {
                LogHelper.Log.Info(" UI更新快消息数据类型失败...........");
            }
            if (quickCfg.QuickSigData1 != null)
            {
                tbx_quicksig_data1.Text = quickCfg.QuickSigData1;
            }
            else
            {
                LogHelper.Log.Info(" UI更新快消息数据1失败...........");
            }
            if (quickCfg.QuickSigData2 != null)
            {
                tbx_quicksig_data2.Text = quickCfg.QuickSigData2;
            }
            else
            {
                LogHelper.Log.Info(" UI更新快消息数据2失败...........");
            }
            if (quickCfg.QuickDataCheck == SentConfig.QuickDataOrder.CHECKED)
            {
                chx_hex_order.CheckState = CheckState.Checked;
            }
            else if(quickCfg.QuickDataCheck == SentConfig.QuickDataOrder.UNCHECKED)
            {
                chx_hex_order.CheckState = CheckState.Unchecked;
            }
            #endregion

            //更新完成
            IsSentCfgChanging = false;
        }
        #endregion

        #region 保存sent配置
        /// <summary>
        /// 保存sent配置
        /// </summary>
        private void SaveSentConfig()
        {
            //保存配置
            if (string.IsNullOrEmpty(userAsPath))//绝对路径
            {
                return;
            }
            File.Delete(userAsPath);

            if (!Directory.Exists(sysCfgPath))
            {
                Directory.CreateDirectory(sysCfgPath);
            }
            INIFile.SetValue(LocalConfig.SENT_PATH_HEAD, LocalConfig.SENT_CONFIG_LAST_PATH, userAsPath, sysCfgPath+LocalConfig.SENT_CONFIG_INI_NAME);

            if (rdb_dec.CheckState == CheckState.Checked)
                sentConfig.StorageDataType = LocalConfig.Storage_Data_Type.DEC;
            else if (rdb_hex.CheckState == CheckState.Checked)
                sentConfig.StorageDataType = LocalConfig.Storage_Data_Type.HEX;

            baseCfg.DataType = cob_dataframe_type.Text;
            baseCfg.BatteryState = cob_battery_state.Text;
            baseCfg.SerialMsg = cob_serial_msg.Text;
            baseCfg.TimeLong = tbx_timeframe.Text;

            slowList.Clear();
            for (int i = 0; i < groupNum; i++)
            {
                slowCfg = new SentConfig.SlowSigConfig();
                slowCfg.GroupCount = cob_group_num.Text;
                slowCfg.GroupOrder = dgv_groupdata.Rows[i].Cells[0].Value.ToString();
                slowCfg.GroupSerialID = dgv_groupdata.Rows[i].Cells[1].Value.ToString();
                slowCfg.GroupData = dgv_groupdata.Rows[i].Cells[2].Value.ToString();
                
                slowList.Add(slowCfg);
            }

            quickCfg.QuickSigType = cob_quicksig_type.Text;
            quickCfg.QuickSigData1 = tbx_quicksig_data1.Text;
            quickCfg.QuickSigData2 = tbx_quicksig_data2.Text;
            if (chx_hex_order.CheckState == CheckState.Checked)
            {
                quickCfg.QuickDataCheck = SentConfig.QuickDataOrder.CHECKED;
            }
            else
            {
                quickCfg.QuickDataCheck = SentConfig.QuickDataOrder.UNCHECKED;
            }

            LocalConfig.SaveUpdateConfig(userAsPath, baseCfg,slowList,quickCfg,IsSentCfgChanging,sentConfig);
        }
        #endregion

        #region 保存-菜单/工具栏
        /// <summary>
        /// 工具栏-配置另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tool_saveas_Click(object sender, EventArgs e)
        {
            userAsPath = FileSelect.SaveAs("ini file|*.ini");
            SaveSentConfig();
        }

        private void Menu_save_as_Click(object sender, EventArgs e)
        {
            userAsPath = FileSelect.SaveAs("ini file|*.ini");
            SaveSentConfig();
        }
        #endregion

        #region 查询设备
        /// <summary>
        /// 查询设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadMenuItem5_Click(object sender, EventArgs e)
        {
            cb_port.Items.Clear();
            foreach (string item in SerialPort.GetPortNames())
            {
                cb_port.Items.Add(item);
            }
        }
        #endregion

        #region 打开文件
        /// <summary>
        /// 打开配置文件，默认选择上次路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tool_open_config_Click(object sender, EventArgs e)
        {
            OpenSentConfig();
        }

        private void Menu_open_file_Click(object sender, EventArgs e)
        {
            OpenSentConfig();
        }

        private void OpenSentConfig()
        {
            FileContent fileContent = FileSelect.GetSelectFileContent("(*.ini)|*.ini", "打开");
            if (fileContent == null)
                return;
            if (string.IsNullOrEmpty(fileContent.FileName))
                return;
            LocalConfig.ReadBaseConfig(fileContent.FileName, baseCfg, slowList, quickCfg,sentConfig);
            UpdateUIConfig();
        }
        #endregion

        #region 帮助菜单
        /// <summary>
        /// 帮助
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Menu_help_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("官方网站：www.baidu.com","帮助",MessageBoxButtons.OK,MessageBoxIcon.Information);
            AbortDialog();
        }

        private void Menu_abort_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox1 = new AboutBox1();
            aboutBox1.StartPosition = FormStartPosition.CenterScreen;
            aboutBox1.ShowDialog();
        }

        private void AbortDialog()
        {
            SentHelp sh = new SentHelp();
            sh.StartPosition = FormStartPosition.CenterScreen;
            sh.MaximizeBox = false;
            sh.MinimizeBox = false;
            sh.ShowDialog();
        }
        #endregion

        #region 退出系统
        private void ApplyExit()
        {
            if (MessageBox.Show("确定要退出系统吗？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Application.Exit();
                serialPort.Close();
            }
        }
        private void Menu_app_exist_Click(object sender, EventArgs e)
        {
            ApplyExit();
        }
        #endregion

        #region 清空发送区数据
        private void Menu_send_clear_Click(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region 清空接收区数据
        private void Menu_receive_clear_Click(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region 清空gridview数据
        /// <summary>
        /// 清空gridview数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_cleardgv_Click(object sender, EventArgs e)
        {
            DataTable dt = dgv_groupdata.DataSource as DataTable;
            dt.Clear();
            dgv_groupdata.DataSource = dt;
            groupNum = 0;
            cob_group_num.Text = "";
        }
        #endregion

        #region 串口调试-发送数据
        /// <summary>
        /// 串口发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_send_Click(object sender, EventArgs e)
        {
            //if (rb_hex_s.Checked)
            //{
            //    ///发送hex格式 
            //    try
            //    {
            //        if (rtb_snedMsg.Text.Length % 2 != 0 || rtb_snedMsg.Text.Length < 1)
            //        {
            //            MessageBox.Show("输入长度不正确！","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            //            return;
            //        }
            //        string[] strArray = rtb_snedMsg.Text.ToLower().Replace("0x","").Split(new char[] { ' ','|',',','-'},StringSplitOptions.RemoveEmptyEntries);
            //        byte[] cmdSendMsg = comandCfg.HexToByte(strArray);
            //        status_sendcount.Text = cmdSendMsg.Length.ToString();

            //        if (cmdSendMsg.Length < 1)
            //        {
            //            MessageBox.Show("发送数据为空", "提示");
            //        }
            //        else
            //        {
            //            if (serialPort.IsOpen)
            //            {
            //                serialPort.Write(cmdSendMsg, 0, cmdSendMsg.Length);
            //                LogHelper.Log.Info("【发送字节】"+BitConverter.ToString(cmdSendMsg));
            //            }
            //            else
            //            {
            //                MessageBox.Show("串口未打开！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        LogHelper.Log.Info(ex.Message);
            //        MessageBox.Show(ex.Message, "错误");
            //    }
            //}
            //else
            //{
            //    //ASCII
            //    try
            //    {
            //        char[] SendChars = comandCfg.ASCIISendMsg().ToString().ToCharArray();
            //        serialPort.Encoding = System.Text.Encoding.GetEncoding("ASCII");
            //        serialPort.Write(SendChars, 0, SendChars.Length);
            //    }
            //    catch (Exception er)
            //    {
            //        MessageBox.Show("错误：SendData" + er.Message, "错误");
            //    }
            //}
        }
        #endregion
    }
}

