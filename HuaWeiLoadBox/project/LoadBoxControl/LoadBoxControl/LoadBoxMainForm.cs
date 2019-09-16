using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.IO.Ports;
using LoadBoxControl.Model;
using CommonUtils.Logger;


namespace LoadBoxControl
{
    public partial class LoadBoxMainForm : RadForm
    {
        private DataTable dtVoltage;
        private DataTable dtPVM;
        private DataTypeEnum dataType;
        private const string V_CHANNEL = "通道";
        private const string V_SIMULATION = "模拟量";

        private const string F_CHANNEL = "通道";
        private const string F_FREQUENCY = "频率";
        private const string F_PERCENT = "占空比";

        private SerialPort serialPort;
        private delegate void MessageDelegate(byte[] msg);
        private VoltageParams voltageParams;
        private PwmParams pwmParams;
        private byte[] pwdbufferTemp = new byte[1024];
        private int lastBufferLen;
        private bool IsFirstReceive = true;

        #region 上位机发送参数常量
        private const string PAGE_DAC_VOLTAGE_BEFORE    = "page pageDAC";
        private const string PAGE_DAC_VOLTAGE_BACK      = "pageDAC";
        private const string PAGE_DAC_PWM_BEFORE        = "page pagePWM";
        private const string PAGE_DAC_PWM_BACK          = "pagePWM";
        private const string FF_END                     = "FFFFFF";
        #endregion

        public LoadBoxMainForm()
        {
            InitializeComponent();
            this.MaximizeBox = false;
        }

        private enum DataTypeEnum
        {
            Voltage = 0,
            PVM = 1
        }

        private void LoadBoxMainForm_Load(object sender, EventArgs e)
        {
            serialPort = new SerialPort();
            voltageParams = new VoltageParams();
            pwmParams = new PwmParams();
            InitControl();
            EventHandlers();
        }

        private void InitControl()
        {
            //获取窗口序列
            this.tool_cb_serialItem.Items.Clear();
            foreach (var portName in SerialPort.GetPortNames())
            {
                this.tool_cb_serialItem.Items.Add(portName);
            }
            if (this.tool_cb_serialItem.Items.Count < 1)
                return;
            this.tool_cb_serialItem.SelectedIndex = 0;
        }

        private void EventHandlers()
        {
            this.tool_refresh.Click += Tool_refresh_Click;
            this.tool_open_searial.Click += Tool_open_searial_Click;
            this.tool_close_serial.Click += Tool_close_serial_Click;
            this.tool_setParams.Click += Tool_setParams_Click;
            this.serialPort.DataReceived += SerialPort_DataReceived;
            this.FormClosed += LoadBoxMainForm_FormClosed;
            this.tb_v1.Click += Tb_v1_Click;
        }

        private void Tb_v1_Click(object sender, EventArgs e)
        {
            EditInput editInput = new EditInput();
            editInput.ShowDialog();
            var inputValue = int.Parse(EditInput.inputValue);

            var sendString = SendVoltageString(1,inputValue);
            LogHelper.Log.Info("【发送字符串】"+sendString);
            SendDevConfigMsg(sendString);
        }

        private void LoadBoxMainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort.IsOpen)
                serialPort.Close();
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte[] receiveData = new byte[serialPort.BytesToRead];
                int readCount = serialPort.Read(receiveData, 0, receiveData.Length);
                if (readCount < 1)
                    return;
                MessageDelegate myDelegate = new MessageDelegate(ShowData);

                this.BeginInvoke(myDelegate, new object[] { receiveData });
                //this.Invoke((EventHandler)delegate
                //{
                //    ShowData(receiveData);
                //});
            }
            else
            {
                MessageBox.Show("请打开某个串口", "错误提示");
            }
        }

        private void Tool_setParams_Click(object sender, EventArgs e)
        {
            CheckChangedParamsToSend();
        }

        private void CheckChangedParamsToSend()
        {
            //是否排队延时发送？
            //检查修改的参数，添加到队列，先取出第一个队列发送；当接收到消息，更新数据后发送下一组修改的数据？
            var voltageV1 = this.tb_v1.Text;
            if (voltageV1 == "")
            {
                this.tb_v1.ForeColor = Color.Red;
                MessageBox.Show("值不能为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.tb_v1.ForeColor = Color.White;
            if (this.tb_v1.Text != voltageParams.VoltageChannel1)
                SendVoltageString(1,int.Parse(voltageV1));
        }

        private string SendVoltageString(int index,int value)
        {
            var sendString = "";
            if (index <= 10)
            {
                sendString = PAGE_DAC_VOLTAGE_BEFORE + FF_END + PAGE_DAC_VOLTAGE_BACK + ".x" + (index - 1) + ".val=" + value * 10 + FF_END;
            }
            else
            {
                sendString = PAGE_DAC_VOLTAGE_BEFORE + "1" + FF_END + PAGE_DAC_VOLTAGE_BACK +"1"+ ".x" + (index - 1 - 10) + ".val=" + value * 10 + FF_END;
            }
            return sendString;
        }

        /// <summary>
        /// PWM-频率
        /// </summary>
        /// <param name="index">当前传入的序号，起始位置为1</param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string SendPwmFreqString(int index, string value)
        {
            var sendString = "";
            if (index <= 10 && index > 0)
            {
                sendString = PAGE_DAC_PWM_BEFORE + FF_END + PAGE_DAC_PWM_BACK + ".n" + (index + 10 - 1) + ".val=" + value + FF_END;
            }
            else if (index <= 20 && index > 10)
            {
                sendString = PAGE_DAC_PWM_BEFORE + "1" + FF_END + PAGE_DAC_PWM_BACK + "1" + ".n" + (index - 1) + ".val=" + value + FF_END;
            }
            else if (index <= 30 && index > 20)
            {
                sendString = PAGE_DAC_PWM_BEFORE + "2" + FF_END + PAGE_DAC_PWM_BACK + "2" + ".n" + (index - 10 - 1) + ".val=" + value + FF_END;
            }
            return sendString;
        }

        /// <summary>
        /// PVM-频率占空比
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private string SendPwmFreqPersentString(int index, string value)
        {
            var sendString = "";
            if (index <= 10 && index > 0)
            {
                sendString = PAGE_DAC_PWM_BEFORE + FF_END + PAGE_DAC_PWM_BACK + ".n" + (index - 1) + ".val=" + value + FF_END;
            }
            else if (index <= 20 && index > 10)
            {
                sendString = PAGE_DAC_PWM_BEFORE + "1" + FF_END + PAGE_DAC_PWM_BACK + "1" + ".n" + (index - 10 - 1) + ".val=" + value + FF_END;
            }
            else if (index <= 30 && index > 20)
            {
                sendString = PAGE_DAC_PWM_BEFORE + "2" + FF_END + PAGE_DAC_PWM_BACK + "2" + ".n" + (index - 20 - 1) + ".val=" + value + FF_END;
            }
            return sendString;
        }

        private void Tool_close_serial_Click(object sender, EventArgs e)
        {
            if (serialPort.IsOpen)
                serialPort.Close();
        }

        private void Tool_open_searial_Click(object sender, EventArgs e)
        {
            SerialOpen();
        }

        private void Tool_refresh_Click(object sender, EventArgs e)
        {
            //重新刷新串口
            this.tool_cb_serialItem.Items.Clear();
            foreach (var portName in SerialPort.GetPortNames())
            {
                this.tool_cb_serialItem.Items.Add(portName);
            }
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        private bool SerialOpen()
        {
            try
            {
                if (!serialPort.IsOpen)
                {
                    ///串口参数设置
                    //串口号
                    if (string.IsNullOrEmpty(this.tool_cb_serialItem.Text))
                    {
                        MessageBox.Show("串口未选择！","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return false;
                    }
                    serialPort.PortName = this.tool_cb_serialItem.Text;
                    //设置串口参数
                    this.serialPort.BaudRate = 115200;
                    this.serialPort.DataBits = 8;
                    this.serialPort.StopBits = StopBits.One;
                    this.serialPort.Parity = Parity.None;

                    serialPort.ReadTimeout = 500;
                    serialPort.WriteTimeout = 500;
                    serialPort.Open();
                }
                return true;
            }
            catch (Exception Err)
            {
                MessageBox.Show($"{Err.Message}","ERROR",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sendContent"></param>
        private void SendDevConfigMsg(string sendContent)
        {
            ///发送hex格式 
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Write(sendContent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }

        private void ShowData(byte[] buffer)
        {
            LogHelper.Log.Info($"********【收到下位机返回消息】len={buffer.Length} " + BitConverter.ToString(buffer));
            //解析数据
            /*
             * 数据格式：数据接收方+数据发送方+数据长度+标识+子标识+信息数据+数据有效性判断
             *  总长=126          C1 + C0 +len + sid + subid + data + crc(len+sid+subid+data)
             */
            if (IsFirstReceive)
            {
                if (buffer.Length <= 1)
                {
                    if (buffer[0] != 0XC1)
                    {
                        IsFirstReceive = false;
                        return;
                    }
                }
                else if (buffer.Length >= 2)
                {
                    if (buffer[0] != 0XC1 && buffer[1] != 0XC0)
                    {
                        IsFirstReceive = false;
                        return;
                    }
                }
                IsFirstReceive = false;

            }
            if (buffer[0] == 0XC1)
            {
                //可能不完整
                //进一步判断
                if (buffer.Length < 3)
                {
                    //数据头不完整
                    LogHelper.Log.Info("帧头不完整"+BitConverter.ToString(buffer));
                    buffer.CopyTo(pwdbufferTemp, lastBufferLen);
                    lastBufferLen = buffer.Length;
                    return;
                }
                else if (buffer.Length < buffer[2] + 3) 
                {
                    //数据区尚未接收完整
                    LogHelper.Log.Info("【第一次接收不完整数据】" + buffer.Length + "  " + BitConverter.ToString(buffer));
                    //第一次不完整数组
                    buffer.CopyTo(pwdbufferTemp, lastBufferLen);
                    lastBufferLen = buffer.Length;
                    return;
                }
                else if (buffer.Length > buffer[2] + 3)
                {
                    //一次接收多组数据
                    LogHelper.Log.Info("【接收缓存大于需要的一帧数据-循环取出】");

                    while (buffer.Length >= buffer[2] + 3)
                    {
                        //取出需要的长度，进行解析
                        LogHelper.Log.Info($"【长度满足条件-取出数据-开始解析】lastBufferLen={lastBufferLen} pwdbufferTemp={pwdbufferTemp[2] + 3}");
                        byte[] needBuffer = new byte[buffer[2] + 3];
                        Array.Copy(buffer, 0, needBuffer, 0, buffer[2] + 3);
                        AnalysisVoltageData(needBuffer);

                        //更新剩余部分
                        int nextIndex = buffer.Length - needBuffer.Length;
                        byte[] nbuffer = new byte[nextIndex];
                        Array.Copy(buffer, needBuffer.Length, nbuffer,0,nextIndex);
                        buffer = nbuffer;
                        if (buffer.Length <= 2)
                            break;
                    }
                }
                else if (buffer.Length == buffer[2] + 3)
                {
                    //数据完整
                    AnalysisVoltageData(buffer);
                }
            }
            else
            {
                //继续缓存不完整数据
                LogHelper.Log.Info("【继续接收不完整数据】" + buffer.Length + "  " + BitConverter.ToString(buffer));
                //buffer.CopyTo(pwdbufferTemp, lastBufferLen + 1);
                Array.Copy(buffer,0,pwdbufferTemp,lastBufferLen,buffer.Length);
                lastBufferLen += buffer.Length;
                //每次缓存后进行完整性判断
                while (lastBufferLen >= pwdbufferTemp[2] + 3)
                {
                    //取出需要的长度，进行解析
                    LogHelper.Log.Info($"【长度满足条件-取出数据-开始解析】lastBufferLen={lastBufferLen} pwdbufferTemp={pwdbufferTemp[2] + 3}");
                    byte[] needBuffer = new byte[pwdbufferTemp[2] + 3];
                    Array.Copy(pwdbufferTemp,0,needBuffer,0,pwdbufferTemp[2] + 3);
                    AnalysisVoltageData(needBuffer);
                    //更新原buffer缓存
                    int sourchIndex = pwdbufferTemp[2] + 3;
                    byte[] nbuffer = new byte[1024];
                    //int destinLen = pwdbufferTemp.Length - pwdbufferTemp[2] - 3;
                    Array.Copy(pwdbufferTemp,sourchIndex,nbuffer,0,pwdbufferTemp[2] + 3);
                    lastBufferLen -= pwdbufferTemp[2] + 3;
                    pwdbufferTemp = nbuffer;
                    LogHelper.Log.Info($"取出数据后-更新lastBufferLen={lastBufferLen} pwdbufferTemp={pwdbufferTemp[2] + 3}");
                }
            }
        }

        private void AnalysisVoltageData(byte[] buffer)
        {
            //数据包完成，检查校验
            var validValue = CrcValid(buffer);
            //if (validValue != buffer[buffer.Length - 1]) //校验失败，最后一个字节是校验位
            //{
            //    LogHelper.Log.Info("校验失败！数据包不正确！");
            //    break;
            //}
            if (buffer[2] == 0X2B)
            {
                //电压数据
                LogHelper.Log.Info("【电压完整数据】" + BitConverter.ToString(buffer));
                #region 1-20
                voltageParams.VoltageChannel1 = ((double)BitConverter.ToInt16(buffer, 5) / 10).ToString();
                voltageParams.VoltageChannel2 = ((double)BitConverter.ToInt16(buffer, 7) / 10).ToString();
                voltageParams.VoltageChannel3 = ((double)BitConverter.ToInt16(buffer, 9) / 10).ToString();
                voltageParams.VoltageChannel4 = ((double)BitConverter.ToInt16(buffer, 11) / 10).ToString();
                voltageParams.VoltageChannel5 = ((double)BitConverter.ToInt16(buffer, 13) / 10).ToString();
                voltageParams.VoltageChannel6 = ((double)BitConverter.ToInt16(buffer, 15) / 10).ToString();
                voltageParams.VoltageChannel7 = ((double)BitConverter.ToInt16(buffer, 17) / 10).ToString();
                voltageParams.VoltageChannel8 = ((double)BitConverter.ToInt16(buffer, 19) / 10).ToString();
                voltageParams.VoltageChannel9 = ((double)BitConverter.ToInt16(buffer, 21) / 10).ToString();
                voltageParams.VoltageChannel10 = ((double)BitConverter.ToInt16(buffer, 23) / 10).ToString();
                voltageParams.VoltageChannel11 = ((double)BitConverter.ToInt16(buffer, 25) / 10).ToString();
                voltageParams.VoltageChannel12 = ((double)BitConverter.ToInt16(buffer, 27) / 10).ToString();
                voltageParams.VoltageChannel13 = ((double)BitConverter.ToInt16(buffer, 29) / 10).ToString();
                voltageParams.VoltageChannel14 = ((double)BitConverter.ToInt16(buffer, 31) / 10).ToString();
                voltageParams.VoltageChannel15 = ((double)BitConverter.ToInt16(buffer, 33) / 10).ToString();
                voltageParams.VoltageChannel16 = ((double)BitConverter.ToInt16(buffer, 35) / 10).ToString();
                voltageParams.VoltageChannel17 = ((double)BitConverter.ToInt16(buffer, 37) / 10).ToString();
                voltageParams.VoltageChannel18 = ((double)BitConverter.ToInt16(buffer, 39) / 10).ToString();
                voltageParams.VoltageChannel19 = ((double)BitConverter.ToInt16(buffer, 41) / 10).ToString();
                voltageParams.VoltageChannel20 = ((double)BitConverter.ToInt16(buffer, 43) / 10).ToString();
                #endregion

                RefreshVoltageUI(voltageParams);
            }
            else if (buffer[2] == 0X7B)
            {
                //频率和占空比
                LogHelper.Log.Info("【频率和占空比完整数据】" + BitConverter.ToString(buffer));
                //传入格式为：频率1-10+占空比1-10；频率11-20+占空比11-20；频率21-30+占空比21-30；
                #region 1-10
                pwmParams.PwmFrequencyCh1 = BitConverter.ToInt16(buffer, 5).ToString();
                pwmParams.PwmFrequencyCh2 = BitConverter.ToInt16(buffer, 7).ToString();
                pwmParams.PwmFrequencyCh3 = BitConverter.ToInt16(buffer, 9).ToString();
                pwmParams.PwmFrequencyCh4 = BitConverter.ToInt16(buffer, 11).ToString();
                pwmParams.PwmFrequencyCh5 = BitConverter.ToInt16(buffer, 13).ToString();
                pwmParams.PwmFrequencyCh6 = BitConverter.ToInt16(buffer, 15).ToString();
                pwmParams.PwmFrequencyCh7 = BitConverter.ToInt16(buffer, 17).ToString();
                pwmParams.PwmFrequencyCh8 = BitConverter.ToInt16(buffer, 19).ToString();
                pwmParams.PwmFrequencyCh9 = BitConverter.ToInt16(buffer, 21).ToString();
                pwmParams.PwmFrequencyCh10 = BitConverter.ToInt16(buffer, 23).ToString();

                pwmParams.PwmFreqPersentCh1 = BitConverter.ToInt16(buffer, 25).ToString();
                pwmParams.PwmFreqPersentCh2 = BitConverter.ToInt16(buffer, 27).ToString();
                pwmParams.PwmFreqPersentCh3 = BitConverter.ToInt16(buffer, 29).ToString();
                pwmParams.PwmFreqPersentCh4 = BitConverter.ToInt16(buffer, 31).ToString();
                pwmParams.PwmFreqPersentCh5 = BitConverter.ToInt16(buffer, 33).ToString();
                pwmParams.PwmFreqPersentCh6 = BitConverter.ToInt16(buffer, 35).ToString();
                pwmParams.PwmFreqPersentCh7 = BitConverter.ToInt16(buffer, 37).ToString();
                pwmParams.PwmFreqPersentCh8 = BitConverter.ToInt16(buffer, 39).ToString();
                pwmParams.PwmFreqPersentCh9 = BitConverter.ToInt16(buffer, 41).ToString();
                pwmParams.PwmFreqPersentCh10 = BitConverter.ToInt16(buffer, 43).ToString();
                #endregion

                #region 11-20
                pwmParams.PwmFrequencyCh11 = BitConverter.ToInt16(buffer, 45).ToString();
                pwmParams.PwmFrequencyCh12 = BitConverter.ToInt16(buffer, 47).ToString();
                pwmParams.PwmFrequencyCh13 = BitConverter.ToInt16(buffer, 49).ToString();
                pwmParams.PwmFrequencyCh14 = BitConverter.ToInt16(buffer, 51).ToString();
                pwmParams.PwmFrequencyCh15 = BitConverter.ToInt16(buffer, 53).ToString();
                pwmParams.PwmFrequencyCh16 = BitConverter.ToInt16(buffer, 55).ToString();
                pwmParams.PwmFrequencyCh17 = BitConverter.ToInt16(buffer, 57).ToString();
                pwmParams.PwmFrequencyCh18 = BitConverter.ToInt16(buffer, 59).ToString();
                pwmParams.PwmFrequencyCh19 = BitConverter.ToInt16(buffer, 61).ToString();
                pwmParams.PwmFrequencyCh20 = BitConverter.ToInt16(buffer, 63).ToString();

                pwmParams.PwmFreqPersentCh11 = BitConverter.ToInt16(buffer, 65).ToString();
                pwmParams.PwmFreqPersentCh12 = BitConverter.ToInt16(buffer, 67).ToString();
                pwmParams.PwmFreqPersentCh13 = BitConverter.ToInt16(buffer, 69).ToString();
                pwmParams.PwmFreqPersentCh14 = BitConverter.ToInt16(buffer, 71).ToString();
                pwmParams.PwmFreqPersentCh15 = BitConverter.ToInt16(buffer, 73).ToString();
                pwmParams.PwmFreqPersentCh16 = BitConverter.ToInt16(buffer, 75).ToString();
                pwmParams.PwmFreqPersentCh17 = BitConverter.ToInt16(buffer, 77).ToString();
                pwmParams.PwmFreqPersentCh18 = BitConverter.ToInt16(buffer, 79).ToString();
                pwmParams.PwmFreqPersentCh19 = BitConverter.ToInt16(buffer, 81).ToString();
                pwmParams.PwmFreqPersentCh20 = BitConverter.ToInt16(buffer, 83).ToString();
                #endregion

                #region 21-30
                pwmParams.PwmFrequencyCh21 = BitConverter.ToInt16(buffer, 85).ToString();
                pwmParams.PwmFrequencyCh22 = BitConverter.ToInt16(buffer, 87).ToString();
                pwmParams.PwmFrequencyCh23 = BitConverter.ToInt16(buffer, 89).ToString();
                pwmParams.PwmFrequencyCh24 = BitConverter.ToInt16(buffer, 91).ToString();
                pwmParams.PwmFrequencyCh25 = BitConverter.ToInt16(buffer, 93).ToString();
                pwmParams.PwmFrequencyCh26 = BitConverter.ToInt16(buffer, 95).ToString();
                pwmParams.PwmFrequencyCh27 = BitConverter.ToInt16(buffer, 97).ToString();
                pwmParams.PwmFrequencyCh28 = BitConverter.ToInt16(buffer, 99).ToString();
                pwmParams.PwmFrequencyCh29 = BitConverter.ToInt16(buffer, 101).ToString();
                pwmParams.PwmFrequencyCh30 = BitConverter.ToInt16(buffer, 103).ToString();

                pwmParams.PwmFreqPersentCh21 = BitConverter.ToInt16(buffer, 105).ToString();
                pwmParams.PwmFreqPersentCh22 = BitConverter.ToInt16(buffer, 107).ToString();
                pwmParams.PwmFreqPersentCh23 = BitConverter.ToInt16(buffer, 109).ToString();
                pwmParams.PwmFreqPersentCh24 = BitConverter.ToInt16(buffer, 111).ToString();
                pwmParams.PwmFreqPersentCh25 = BitConverter.ToInt16(buffer, 113).ToString();
                pwmParams.PwmFreqPersentCh26 = BitConverter.ToInt16(buffer, 115).ToString();
                pwmParams.PwmFreqPersentCh27 = BitConverter.ToInt16(buffer, 117).ToString();
                pwmParams.PwmFreqPersentCh28 = BitConverter.ToInt16(buffer, 119).ToString();
                pwmParams.PwmFreqPersentCh29 = BitConverter.ToInt16(buffer, 121).ToString();
                pwmParams.PwmFreqPersentCh30 = BitConverter.ToInt16(buffer, 123).ToString();
                #endregion

                RefreshPwdUI(pwmParams);

                //byte[] restBuffer = new byte[buffer.Length - buffer[2] - 3];
                ////restBuffer.CopyTo(buffer,buffer[2] + 3 + 1);//复制剩余长度到新数组继续解析
                //Array.Copy(buffer,buffer[2] + 3 +1,restBuffer,0,restBuffer.Length);
                //AnalysisVoltageData(restBuffer);
            }
        }

        private void RefreshVoltageUI(VoltageParams voltageParams)
        {
            this.tb_v1.Text = voltageParams.VoltageChannel1;
            this.tb_v2.Text = voltageParams.VoltageChannel2;
            this.tb_v3.Text = voltageParams.VoltageChannel3;
            this.tb_v4.Text = voltageParams.VoltageChannel4;
            this.tb_v5.Text = voltageParams.VoltageChannel5;
            this.tb_v6.Text = voltageParams.VoltageChannel6;
            this.tb_v7.Text = voltageParams.VoltageChannel7;
            this.tb_v8.Text = voltageParams.VoltageChannel8;
            this.tb_v9.Text = voltageParams.VoltageChannel9;
            this.tb_v10.Text = voltageParams.VoltageChannel10;
            this.tb_v11.Text = voltageParams.VoltageChannel11;
            this.tb_v12.Text = voltageParams.VoltageChannel12;
            this.tb_v13.Text = voltageParams.VoltageChannel13;
            this.tb_v14.Text = voltageParams.VoltageChannel14;
            this.tb_v15.Text = voltageParams.VoltageChannel15;
            this.tb_v16.Text = voltageParams.VoltageChannel16;
            this.tb_v17.Text = voltageParams.VoltageChannel17;
            this.tb_v18.Text = voltageParams.VoltageChannel18;
            this.tb_v19.Text = voltageParams.VoltageChannel19;
            this.tb_v20.Text = voltageParams.VoltageChannel20;
        }

        private void RefreshPwdUI(PwmParams pwmParams)
        {
            this.tb_pf1.Text = pwmParams.PwmFrequencyCh1;
            this.tb_pf2.Text = pwmParams.PwmFrequencyCh2;
            this.tb_pf3.Text = pwmParams.PwmFrequencyCh3;
            this.tb_pf4.Text = pwmParams.PwmFrequencyCh4;
            this.tb_pf5.Text = pwmParams.PwmFrequencyCh5;
            this.tb_pf6.Text = pwmParams.PwmFrequencyCh6;
            this.tb_pf7.Text = pwmParams.PwmFrequencyCh7;
            this.tb_pf8.Text = pwmParams.PwmFrequencyCh8;
            this.tb_pf9.Text = pwmParams.PwmFrequencyCh9;
            this.tb_pf10.Text = pwmParams.PwmFrequencyCh10;
            this.tb_pf11.Text = pwmParams.PwmFrequencyCh11;
            this.tb_pf12.Text = pwmParams.PwmFrequencyCh12;
            this.tb_pf13.Text = pwmParams.PwmFrequencyCh13;
            this.tb_pf14.Text = pwmParams.PwmFrequencyCh14;
            this.tb_pf15.Text = pwmParams.PwmFrequencyCh15;
            this.tb_pf16.Text = pwmParams.PwmFrequencyCh16;
            this.tb_pf17.Text = pwmParams.PwmFrequencyCh17;
            this.tb_pf18.Text = pwmParams.PwmFrequencyCh18;
            this.tb_pf19.Text = pwmParams.PwmFrequencyCh19;
            this.tb_pf20.Text = pwmParams.PwmFrequencyCh20;
            this.tb_pf21.Text = pwmParams.PwmFrequencyCh21;
            this.tb_pf22.Text = pwmParams.PwmFrequencyCh22;
            this.tb_pf23.Text = pwmParams.PwmFrequencyCh23;
            this.tb_pf24.Text = pwmParams.PwmFrequencyCh24;
            this.tb_pf25.Text = pwmParams.PwmFrequencyCh25;
            this.tb_pf26.Text = pwmParams.PwmFrequencyCh26;
            this.tb_pf27.Text = pwmParams.PwmFrequencyCh27;
            this.tb_pf28.Text = pwmParams.PwmFrequencyCh28;
            this.tb_pf29.Text = pwmParams.PwmFrequencyCh29;
            this.tb_pf30.Text = pwmParams.PwmFrequencyCh30;

            this.tb_pp1.Text = pwmParams.PwmFreqPersentCh1;
            this.tb_pp2.Text = pwmParams.PwmFreqPersentCh2;
            this.tb_pp3.Text = pwmParams.PwmFreqPersentCh3;
            this.tb_pp4.Text = pwmParams.PwmFreqPersentCh4;
            this.tb_pp5.Text = pwmParams.PwmFreqPersentCh5;
            this.tb_pp6.Text = pwmParams.PwmFreqPersentCh6;
            this.tb_pp7.Text = pwmParams.PwmFreqPersentCh7;
            this.tb_pp8.Text = pwmParams.PwmFreqPersentCh8;
            this.tb_pp9.Text = pwmParams.PwmFreqPersentCh9;
            this.tb_pp10.Text = pwmParams.PwmFreqPersentCh10;
            this.tb_pp11.Text = pwmParams.PwmFreqPersentCh11;
            this.tb_pp12.Text = pwmParams.PwmFreqPersentCh12;
            this.tb_pp13.Text = pwmParams.PwmFreqPersentCh13;
            this.tb_pp14.Text = pwmParams.PwmFreqPersentCh14;
            this.tb_pp15.Text = pwmParams.PwmFreqPersentCh15;
            this.tb_pp16.Text = pwmParams.PwmFreqPersentCh16;
            this.tb_pp17.Text = pwmParams.PwmFreqPersentCh17;
            this.tb_pp18.Text = pwmParams.PwmFreqPersentCh18;
            this.tb_pp19.Text = pwmParams.PwmFreqPersentCh19;
            this.tb_pp20.Text = pwmParams.PwmFreqPersentCh20;
            this.tb_pp21.Text = pwmParams.PwmFreqPersentCh21;
            this.tb_pp22.Text = pwmParams.PwmFreqPersentCh22;
            this.tb_pp23.Text = pwmParams.PwmFreqPersentCh23;
            this.tb_pp24.Text = pwmParams.PwmFreqPersentCh24;
            this.tb_pp25.Text = pwmParams.PwmFreqPersentCh25;
            this.tb_pp26.Text = pwmParams.PwmFreqPersentCh26;
            this.tb_pp27.Text = pwmParams.PwmFreqPersentCh27;
            this.tb_pp28.Text = pwmParams.PwmFreqPersentCh28;
            this.tb_pp29.Text = pwmParams.PwmFreqPersentCh29;
            this.tb_pp30.Text = pwmParams.PwmFreqPersentCh30;
        }

        private byte CrcValid(byte[] receiveData)
        {
            //C1 + C0 +len + sid + subid + data + crc(len+sid+subid+data)
            byte sum = 0;
            for (int i = 0 + 2; i < receiveData.Length - 2; i++)
            {
                sum += receiveData[i];
            }
            return sum;
        }
    }
}
