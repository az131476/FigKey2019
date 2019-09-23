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
        private const string FF_END                     = "FF FF FF";
        #endregion

        public LoadBoxMainForm()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
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
            this.serialPort.DataReceived += SerialPort_DataReceived;
            this.FormClosed += LoadBoxMainForm_FormClosed;
            this.tool_help.Click += Tool_help_Click;
            this.tool_abort.Click += Tool_abort_Click;

            #region voltage click event hander
            this.tb_v1.Click += Tb_v1_Click;
            this.tb_v2.Click += Tb_v2_Click;
            this.tb_v3.Click += Tb_v3_Click;
            this.tb_v4.Click += Tb_v4_Click;
            this.tb_v5.Click += Tb_v5_Click;
            this.tb_v6.Click += Tb_v6_Click;
            this.tb_v7.Click += Tb_v7_Click;
            this.tb_v8.Click += Tb_v8_Click;
            this.tb_v9.Click += Tb_v9_Click;
            this.tb_v10.Click += Tb_v10_Click;
            this.tb_v11.Click += Tb_v11_Click;
            this.tb_v12.Click += Tb_v12_Click;
            this.tb_v13.Click += Tb_v13_Click;
            this.tb_v14.Click += Tb_v14_Click;
            this.tb_v15.Click += Tb_v15_Click;
            this.tb_v16.Click += Tb_v16_Click;
            this.tb_v17.Click += Tb_v17_Click;
            this.tb_v18.Click += Tb_v18_Click;
            this.tb_v19.Click += Tb_v19_Click;
            this.tb_v20.Click += Tb_v20_Click;
            #endregion

            #region pwm frequency 
            this.tb_pf1.Click += Tb_pf1_Click;
            this.tb_pf2.Click += Tb_pf2_Click;
            this.tb_pf3.Click += Tb_pf3_Click;
            this.tb_pf4.Click += Tb_pf4_Click;
            this.tb_pf5.Click += Tb_pf5_Click;
            this.tb_pf6.Click += Tb_pf6_Click;
            this.tb_pf7.Click += Tb_pf7_Click;
            this.tb_pf8.Click += Tb_pf8_Click;
            this.tb_pf9.Click += Tb_pf9_Click;
            this.tb_pf10.Click += Tb_pf10_Click;
            this.tb_pf11.Click += Tb_pf11_Click;
            this.tb_pf12.Click += Tb_pf12_Click;
            this.tb_pf13.Click += Tb_pf13_Click;
            this.tb_pf14.Click += Tb_pf14_Click;
            this.tb_pf15.Click += Tb_pf15_Click;
            this.tb_pf16.Click += Tb_pf16_Click;
            this.tb_pf17.Click += Tb_pf17_Click;
            this.tb_pf18.Click += Tb_pf18_Click;
            this.tb_pf19.Click += Tb_pf19_Click;
            this.tb_pf20.Click += Tb_pf20_Click;
            this.tb_pf21.Click += Tb_pf21_Click;
            this.tb_pf22.Click += Tb_pf22_Click;
            this.tb_pf23.Click += Tb_pf23_Click;
            this.tb_pf24.Click += Tb_pf24_Click;
            this.tb_pf25.Click += Tb_pf25_Click;
            this.tb_pf26.Click += Tb_pf26_Click;
            this.tb_pf27.Click += Tb_pf27_Click;
            this.tb_pf28.Click += Tb_pf28_Click;
            this.tb_pf29.Click += Tb_pf29_Click;
            this.tb_pf30.Click += Tb_pf30_Click;
            #endregion

            #region pwm frequency persent
            this.tb_pp1.Click += Tb_pp1_Click;
            this.tb_pp2.Click += Tb_pp2_Click;
            this.tb_pp3.Click += Tb_pp3_Click;
            this.tb_pp4.Click += Tb_pp4_Click;
            this.tb_pp5.Click += Tb_pp5_Click;
            this.tb_pp6.Click += Tb_pp6_Click;
            this.tb_pp7.Click += Tb_pp7_Click;
            this.tb_pp8.Click += Tb_pp8_Click;
            this.tb_pp9.Click += Tb_pp9_Click;
            this.tb_pp10.Click += Tb_pp10_Click;
            this.tb_pp11.Click += Tb_pp11_Click;
            this.tb_pp12.Click += Tb_pp12_Click;
            this.tb_pp13.Click += Tb_pp13_Click;
            this.tb_pp14.Click += Tb_pp14_Click;
            this.tb_pp15.Click += Tb_pp15_Click;
            this.tb_pp16.Click += Tb_pp16_Click;
            this.tb_pp17.Click += Tb_pp17_Click;
            this.tb_pp18.Click += Tb_pp18_Click;
            this.tb_pp19.Click += Tb_pp19_Click;
            this.tb_pp20.Click += Tb_pp20_Click;
            this.tb_pp21.Click += Tb_pp21_Click;
            this.tb_pp22.Click += Tb_pp22_Click;
            this.tb_pp23.Click += Tb_pp23_Click;
            this.tb_pp24.Click += Tb_pp24_Click;
            this.tb_pp25.Click += Tb_pp25_Click;
            this.tb_pp26.Click += Tb_pp26_Click;
            this.tb_pp27.Click += Tb_pp27_Click;
            this.tb_pp28.Click += Tb_pp28_Click;
            this.tb_pp29.Click += Tb_pp29_Click;
            this.tb_pp30.Click += Tb_pp30_Click;

            #endregion
        }

        private void Tool_abort_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox1 = new AboutBox1();
            aboutBox1.ShowDialog();
        }

        private void Tool_help_Click(object sender, EventArgs e)
        {
            FHelp fHelp = new FHelp();
            fHelp.ShowDialog();
        }

        #region pwm frequency persent
        private void Tb_pp30_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(30, this.tb_pp30.Text))
                this.tb_pp30.Text = EditInput.inputValue.ToString(); 
        }

        private void Tb_pp29_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(29, this.tb_pp29.Text))
                this.tb_pp29.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp28_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(28, this.tb_pp28.Text))
                this.tb_pp28.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp27_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(27, this.tb_pp27.Text))
                this.tb_pp27.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp26_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(26, this.tb_pp26.Text))
                this.tb_pp26.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp25_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(25, this.tb_pp25.Text))
                this.tb_pp25.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp24_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(24, this.tb_pp24.Text))
                this.tb_pp24.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp23_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(23, this.tb_pp23.Text))
                this.tb_pp23.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp22_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(22, this.tb_pp22.Text))
                this.tb_pp22.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp21_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(21, this.tb_pp21.Text))
                this.tb_pp21.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp20_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(20, this.tb_pp20.Text))
                this.tb_pp20.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp19_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(19, this.tb_pp19.Text))
                this.tb_pp19.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp18_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(18, this.tb_pp18.Text))
                this.tb_pp18.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp17_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(17, this.tb_pp17.Text))
                this.tb_pp17.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp16_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(16, this.tb_pp16.Text))
                this.tb_pp16.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp15_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(15, this.tb_pp15.Text))
                this.tb_pp15.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp14_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(14, this.tb_pp14.Text))
                this.tb_pp14.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp13_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(13, this.tb_pp13.Text))
                this.tb_pp13.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp12_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(12, this.tb_pp12.Text))
                this.tb_pp12.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp11_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(11, this.tb_pp11.Text))
                this.tb_pp11.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp10_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(10, this.tb_pp10.Text))
                this.tb_pp10.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp9_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(9, this.tb_pp9.Text))
                this.tb_pp9.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp8_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(8, this.tb_pp8.Text))
                this.tb_pp8.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp7_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(7, this.tb_pp7.Text))
                this.tb_pp7.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp6_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(6, this.tb_pp6.Text))
                this.tb_pp6.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp5_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(5, this.tb_pp5.Text))
                this.tb_pp5.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp4_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(4, this.tb_pp4.Text))
                this.tb_pp4.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp3_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(3, this.tb_pp3.Text))
                this.tb_pp3.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp2_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(2, this.tb_pp2.Text))
                this.tb_pp2.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pp1_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqPersentSendString(1, this.tb_pp1.Text))
                this.tb_pp1.Text = EditInput.inputValue.ToString();
        }
        #endregion

        #region pwm frequency
        private void Tb_pf30_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(30, this.tb_pf30.Text))
                this.tb_pf30.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf29_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(29, this.tb_pf29.Text))
                this.tb_pf29.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf28_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(28, this.tb_pf28.Text))
                this.tb_pf28.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf27_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(27, this.tb_pf27.Text))
                this.tb_pf27.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf26_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(26, this.tb_pf26.Text))
                this.tb_pf26.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf25_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(25, this.tb_pf25.Text))
                this.tb_pf25.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf24_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(24, this.tb_pf24.Text))
                this.tb_pf24.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf23_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(23, this.tb_pf23.Text))
                this.tb_pf23.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf22_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(22, this.tb_pf22.Text))
                this.tb_pf22.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf21_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(21, this.tb_pf21.Text))
                this.tb_pf21.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf20_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(20, this.tb_pf20.Text))
                this.tb_pf20.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf19_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(19, this.tb_pf19.Text))
                this.tb_pf19.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf18_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(18, this.tb_pf18.Text))
                this.tb_pf18.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf17_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(17, this.tb_pf17.Text))
                this.tb_pf17.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf16_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(16, this.tb_pf16.Text))
                this.tb_pf16.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf15_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(15, this.tb_pf15.Text))
                this.tb_pf15.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf14_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(14, this.tb_pf14.Text))
                this.tb_pf14.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf13_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(13, this.tb_pf13.Text))
                this.tb_pf13.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf12_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(12, this.tb_pf12.Text))
                this.tb_pf12.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf11_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(11, this.tb_pf11.Text))
                this.tb_pf11.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf10_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(10, this.tb_pf10.Text))
                this.tb_pf10.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf9_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(9, this.tb_pf9.Text))
                this.tb_pf9.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf8_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(8, this.tb_pf8.Text))
                this.tb_pf8.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf7_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(7, this.tb_pf7.Text))
                this.tb_pf7.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf6_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(6, this.tb_pf6.Text))
                this.tb_pf6.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf5_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(5, this.tb_pf5.Text))
                this.tb_pf5.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf4_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(4, this.tb_pf4.Text))
                this.tb_pf4.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf3_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(3, this.tb_pf3.Text))
                this.tb_pf3.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf2_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(2, this.tb_pf2.Text))
                this.tb_pf2.Text = EditInput.inputValue.ToString();
        }

        private void Tb_pf1_Click(object sender, EventArgs e)
        {
            if (EditPwdFreqSendString(1, this.tb_pf1.Text))
                this.tb_pf1.Text = EditInput.inputValue.ToString();
        }
        #endregion

        #region voltage
        private void Tb_v20_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(20, this.tb_v20.Text))
                this.tb_v20.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v19_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(19, this.tb_v19.Text))
                this.tb_v19.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v18_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(18, this.tb_v18.Text))
                this.tb_v18.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v17_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(17, this.tb_v17.Text))
                this.tb_v17.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v16_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(16, this.tb_v16.Text))
                this.tb_v16.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v15_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(15, this.tb_v15.Text))
                this.tb_v15.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v14_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(14, this.tb_v14.Text))
                this.tb_v14.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v13_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(13, this.tb_v13.Text))
                this.tb_v13.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v12_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(12, this.tb_v12.Text))
                this.tb_v12.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v11_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(11, this.tb_v11.Text))
                this.tb_v11.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v10_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(10, this.tb_v10.Text))
                this.tb_v10.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v9_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(9, this.tb_v9.Text))
                this.tb_v9.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v8_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(8, this.tb_v8.Text))
                this.tb_v8.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v7_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(7, this.tb_v7.Text))
                this.tb_v7.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v6_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(6, this.tb_v6.Text))
                this.tb_v6.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v5_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(5, this.tb_v5.Text))
                this.tb_v5.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v4_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(4, this.tb_v4.Text))
                this.tb_v4.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v3_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(3, this.tb_v3.Text))
                this.tb_v3.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v2_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(2, this.tb_v2.Text))
                this.tb_v2.Text = EditInput.inputValue.ToString();
        }

        private void Tb_v1_Click(object sender, EventArgs e)
        {
            if (EditVoltageSendString(1, this.tb_v1.Text))
                this.tb_v1.Text = EditInput.inputValue.ToString();
        }
        #endregion

        /// <summary>
        /// 起始位置从1开始
        /// </summary>
        /// <param name="startIndex"></param>
        private bool EditVoltageSendString(int startIndex,string inputString)
        {
            EditInput editInput = new EditInput(inputString,EditInput.DataType.Voltage);
            editInput.ShowDialog();
            if (editInput.DialogResult != DialogResult.OK)
            {
                return false;
            }
            var sendByte = SendVoltageString(startIndex, EditInput.inputValue);
            LogHelper.Log.Info($"【发送字符串】index={startIndex} " + BitConverter.ToString(sendByte));
            if (SendDevConfigMsg(sendByte))
                return true;
            return false;
        }

        private bool EditPwdFreqSendString(int startIndex,string inputString)
        {
            EditInput editInput = new EditInput(inputString,EditInput.DataType.PwmFrequency);
            editInput.ShowDialog();
            if (editInput.DialogResult != DialogResult.OK)
            {
                return false;
            }
            var sendByte = SendPwmFreqString(startIndex, EditInput.inputValue);
            LogHelper.Log.Info($"【pwd-freq】index={startIndex} " + BitConverter.ToString(sendByte));
            if (SendDevConfigMsg(sendByte))
                return true;
            return false;
        }

        private bool EditPwdFreqPersentSendString(int startIndex,string inputString)
        {
            EditInput editInput = new EditInput(inputString,EditInput.DataType.PwmFrequencyPersent);
            editInput.ShowDialog();
            if (editInput.DialogResult != DialogResult.OK)
            {
                return false;
            }
            var sendByte = SendPwmFreqPersentString(startIndex, EditInput.inputValue);
            LogHelper.Log.Info($"【pwd-persent】index={startIndex} " + BitConverter.ToString(sendByte));
            if (SendDevConfigMsg(sendByte))
                return true;
            return false;
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

        private byte[] SendVoltageString(int index,double value)
        {
            var part1 = "";
            var part2 = "";
            var part3 = "";
            var part4 = "";
            if (index <= 10)
            {
                part1 = PAGE_DAC_VOLTAGE_BEFORE;
                part2 = FF_END;
                part3 = PAGE_DAC_VOLTAGE_BACK + ".x" + (index - 1) + ".val=" + value * 10;
                part4 = FF_END;
            }
            else
            {
                part1 = PAGE_DAC_VOLTAGE_BEFORE + "1";
                part2 = FF_END;
                part3 = PAGE_DAC_VOLTAGE_BACK + "1" + ".x" + (index - 1 - 10) + ".val=" + value * 10;
                part4 = FF_END;
            }
            return ConvertSendByte(part1,part2,part3,part4);
        }

        private byte[] CharToByte(string inputString)
        {
            char[] strArray = inputString.Replace(" ", "").ToCharArray();
            byte[] btArray = new byte[strArray.Length];
            for (int i = 0; i < strArray.Length; i++)
            {
                btArray[i] = Convert.ToByte(strArray[i]);
            }
            return btArray;
        }

        /// <summary>
        /// byte连接16进制字符串，以空格隔开
        /// </summary>
        /// <param name="sourchByte"></param>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private byte[] JoinToByte(byte[] sourchByte,string hexString)
        {
            string[] stringArray = hexString.Split(' ');
            byte[] unionByte = new byte[sourchByte.Length + stringArray.Length];
            byte[] hexByte = new byte[stringArray.Length];
            for (int i = 0; i < stringArray.Length; i++)
            {
                hexByte[i] = Convert.ToByte(stringArray[i],16);
            }
            sourchByte.CopyTo(unionByte, 0);
            Array.Copy(hexByte,0,unionByte,sourchByte.Length,hexByte.Length);
            return unionByte;
        }

        private byte[] ConvertSendByte(string part1String,string hexString1,string part2String,string hexString2)
        {
            var part1Byte = CharToByte(part1String);
            var part2Byte = CharToByte(part2String);
            var joinPart1Byte = JoinToByte(part1Byte,hexString1);
            byte[] bothPart1AndPart2 = new byte[joinPart1Byte.Length + part2Byte.Length];
            joinPart1Byte.CopyTo(bothPart1AndPart2, 0);
            Array.Copy(part2Byte,0, bothPart1AndPart2, joinPart1Byte.Length,part2Byte.Length);
            var joinPart2Byte = JoinToByte(bothPart1AndPart2, hexString2);
            return joinPart2Byte;
        }

        /// <summary>
        /// PWM-频率
        /// </summary>
        /// <param name="index">当前传入的序号，起始位置为1</param>
        /// <param name="value"></param>
        /// <returns></returns>
        private byte[] SendPwmFreqString(int index, double value)
        {
            var part1 = "";
            var part2 = "";
            var part3 = "";
            var part4 = "";
            if (index <= 10 && index > 0)
            {
                part1 = PAGE_DAC_PWM_BEFORE;
                part2 = FF_END;
                part3 = PAGE_DAC_PWM_BACK + ".n" + (index + 10 - 1) + ".val=" + value;
                part4 = FF_END;
            }
            else if (index <= 20 && index > 10)
            {
                part1 = PAGE_DAC_PWM_BEFORE + "1";
                part2 = FF_END;
                part3 = PAGE_DAC_PWM_BACK + "1" + ".n" + (index - 1) + ".val=" + value;
                part4 = FF_END;
            }
            else if (index <= 30 && index > 20)
            {
                part1 = PAGE_DAC_PWM_BEFORE + "2";
                part2 = FF_END;
                part3 = PAGE_DAC_PWM_BACK + "2" + ".n" + (index - 10 - 1) + ".val=" + value;
                part4 = FF_END;
            }
            return ConvertSendByte(part1,part2,part3,part4);
        }

        /// <summary>
        /// PVM-频率占空比
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private byte[] SendPwmFreqPersentString(int index, double value)
        {
            var part1 = "";
            var part2 = "";
            var part3 = "";
            var part4 = "";
            if (index <= 10 && index > 0)
            {
                part1 = PAGE_DAC_PWM_BEFORE;
                part2 = FF_END;
                part3 = PAGE_DAC_PWM_BACK + ".n" + (index - 1) + ".val=" + value;
                part4 = FF_END;
            }
            else if (index <= 20 && index > 10)
            {
                part1 = PAGE_DAC_PWM_BEFORE + "1";
                part2 = FF_END;
                part3 = PAGE_DAC_PWM_BACK + "1" + ".n" + (index - 10 - 1) + ".val=" + value;
                part4 = FF_END;
            }
            else if (index <= 30 && index > 20)
            {
                part1 = PAGE_DAC_PWM_BEFORE + "2";
                part2 = FF_END;
                part3 = PAGE_DAC_PWM_BACK + "2" + ".n" + (index - 20 - 1) + ".val=" + value;
                part4 = FF_END;
            }
            return ConvertSendByte(part1,part2,part3,part4);
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
        private bool SendDevConfigMsg(byte[] sendContent)
        {
            ///发送hex格式 
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Write(sendContent,0,sendContent.Length);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
                return false;
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
