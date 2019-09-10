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

        #region 上位机发送参数常量
        private const string PAGE_DAC_VOLTAGE   = "page pageDAC";
        private const string PAGE_DAC_PWM       = "page pagePWM";
        private const string FF_END             = "FFFFFF";
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
                sendString = PAGE_DAC_VOLTAGE + FF_END + PAGE_DAC_VOLTAGE + ".x" + (index - 1) + ".val=" + value * 10;
            }
            else
            {
                sendString = PAGE_DAC_VOLTAGE + "1" + FF_END + PAGE_DAC_VOLTAGE +"1"+ ".x" + (index - 1 - 10) + ".val=" + value * 10;
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
                sendString = PAGE_DAC_PWM + FF_END + PAGE_DAC_PWM + ".n" + (index + 10 - 1) + ".val=" + value;
            }
            else if (index <= 20 && index > 10)
            {
                sendString = PAGE_DAC_PWM + "1" + FF_END + PAGE_DAC_PWM + "1" + ".x" + (index - 1) + ".val=" + value;
            }
            else if (index <= 30 && index > 20)
            {
                sendString = PAGE_DAC_PWM + "2" + FF_END + PAGE_DAC_PWM + "2" + ".x" + (index - 10 - 1) + ".val=" + value;
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
                sendString = PAGE_DAC_PWM + FF_END + PAGE_DAC_PWM + ".n" + (index - 1) + ".val=" + value;
            }
            else if (index <= 20 && index > 10)
            {
                sendString = PAGE_DAC_PWM + "1" + FF_END + PAGE_DAC_PWM + "1" + ".x" + (index - 10 - 1) + ".val=" + value;
            }
            else if (index <= 30 && index > 20)
            {
                sendString = PAGE_DAC_PWM + "2" + FF_END + PAGE_DAC_PWM + "2" + ".x" + (index - 20 - 1) + ".val=" + value;
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

        private void ShowData(byte[] data)
        {
            LogHelper.Log.Info($"【收到下位机返回消息】len={data.Length} " + BitConverter.ToString(data));
            //解析数据
            /*
             * 数据格式：数据接收方+数据发送方+数据长度+标识+子标识+信息数据+数据有效性判断
             *  总长=126          C1 + C0 +len + sid + subid + data + crc(len+sid+subid+data)
             */
            //if (data[0] == 0XC1 && data[1] == 0XC0 && data[2] == 0X7B)
            //{
            //    //频率和占空比数据

            //}
            //else if (data[0] == 0XC1 && data[1] == 0XC0 && data[2] == 0X2B)
            //{
            //    //电压数据

            //}
            //else
            //{
            //    LogHelper.Log.Info("【数据不完整】");
            //}
            //AnalysisVoltageData(data);
        }

        private void AnalysisVoltageData(byte[] buffer)
        {
            //完整性判断
            while (buffer.Length >= 5) 
            {
                //查找数据头
                if (buffer[0] == 0XC1) //传输数据有帧头，用于判断
                {
                    int len = buffer[2];
                    if (buffer.Length < len + 3) //数据区尚未接收完整
                    {
                        LogHelper.Log.Info("【数据不完整】"+buffer.Length);
                        break;
                    }
                    //数据包完成，检查校验
                    var validValue = CrcValid(buffer);
                    if (validValue != buffer[len + 3]) //校验失败，最后一个字节是校验位
                    {
                        LogHelper.Log.Info("校验失败！数据包不正确！");
                        break;
                    }
                    if (buffer[2] == 0X2B)
                    {
                        //电压数据
                        LogHelper.Log.Info("【电压完整数据】"+BitConverter.ToString(buffer));
                    }
                    else if (buffer[2] == 0X7B)
                    {
                        //频率和占空比
                        LogHelper.Log.Info("【频率和占空比完整数据】" + BitConverter.ToString(buffer));
                    }
                    //执行其他代码，对数据进行处理。
                }
                else //帧头不正确时，清除
                {
                    
                }
            }

        }
        private byte CrcValid(byte[] receiveData)
        {
            //C1 + C0 +len + sid + subid + data + crc(len+sid+subid+data)
            byte sum = 0;
            for (int i = 0 + 2; i < receiveData.Length - 1; i++)
            {
                sum += receiveData[i];
            }
            return sum;
        }
    }
}
