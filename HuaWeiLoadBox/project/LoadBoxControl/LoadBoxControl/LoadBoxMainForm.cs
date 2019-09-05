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

namespace LoadBoxControl
{
    public partial class LoadBoxMainForm : RadForm
    {
        private RadGridView radGridView1;
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

        #region 上位机发送参数常量
        private const string PAGE_DAC_VOLTAGE   = "page pageDAC";
        private const string PAGE_DAC_PWM       = "page pagePWM";
        private const string FF_END             = "FFFFFF";
        #endregion

        public LoadBoxMainForm()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            InitDataSource();
            //BindingDataSource();
        }

        private enum DataTypeEnum
        {
            Voltage = 0,
            PVM = 1
        }

        private void InitDataSource()
        {
            if (dtVoltage == null)
            {
                dtVoltage = new DataTable();
                dtVoltage.Columns.Add(V_CHANNEL);
                dtVoltage.Columns.Add(V_SIMULATION);
            }
            if (dtPVM == null)
            {
                dtPVM = new DataTable();
                dtPVM.Columns.Add(F_CHANNEL);
                dtPVM.Columns.Add(F_FREQUENCY);
                dtPVM.Columns.Add(F_PERCENT);
            }
        }

        private void BindingDataSource()
        {
            if (dataType == DataTypeEnum.Voltage)
            {
                for (int i = 0; i < 20; i++)
                {
                    DataRow dr = dtVoltage.NewRow();
                    dr[V_CHANNEL] = "I" + (i + 1) + "(V)";
                    dr[V_SIMULATION] = 1;
                    dtVoltage.Rows.Add(dr);
                }
                this.radGridView1.DataSource = dtVoltage;
            }
            else if (dataType == DataTypeEnum.PVM)
            {
                for (int i = 0; i < 30; i++)
                {
                    DataRow dr = dtPVM.NewRow();
                    dr[F_CHANNEL] = "I" + i + 1 + "(Pvm)";
                    dr[F_FREQUENCY] = 1000;
                    dr[F_PERCENT] = 20;
                    dtPVM.Rows.Add(dr);
                }
                this.radGridView1.DataSource = dtPVM;
            }
            this.radGridView1.Columns[0].ReadOnly = true;
            this.radGridView1.Columns[0].BestFit();
        }

        private void LoadBoxMainForm_Load(object sender, EventArgs e)
        {
            serialPort = new SerialPort();
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
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                byte[] receiveData = new byte[serialPort.BytesToRead];
                int readCount = serialPort.Read(receiveData, 0, receiveData.Length);
                serialPort.ReadLine();
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

        private void Tool_setParams_Click(object sender, EventArgs e)
        {
            SendDevConfigMsg(,);
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
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte str in data)
            {
                stringBuilder.AppendFormat("{0:X2} ", str);
            }
            string[] hexStr = stringBuilder.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            //LogHelper.Log.Info(" 【接收设备返回值HEX】" + stringBuilder.ToString());
            //AnalysisReceiveData(hexStr, stringBuilder.ToString());
        }
    }
}
