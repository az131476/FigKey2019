using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace LoadBoxControl
{
    public partial class EditInput : RadForm
    {
        public static double inputValue;
        private DataType currentDaTaType;
        public enum DataType
        {
            Voltage,
            PwmFrequency,
            PwmFrequencyPersent
        }
        public EditInput(string inputString,DataType dataType)
        {
            InitializeComponent();
            if (inputString.Trim() != "")
            {
                double.TryParse(inputString.Trim(),out inputValue);
            }
            this.currentDaTaType = dataType;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void EditInput_Load(object sender, EventArgs e)
        {
            if (currentDaTaType == DataType.Voltage)
            {
                ltb_title.Text = "【电压设置】";
                lbx_limit.Text = "请输入0-5V电压";
            }
            else if (currentDaTaType == DataType.PwmFrequency)
            {
                ltb_title.Text = "【频率设置】";
                lbx_limit.Text = "请输入0-20000Hz频率";
            }
            else if (currentDaTaType == DataType.PwmFrequencyPersent)
            {
                ltb_title.Text = "【频率占空比】";
                lbx_limit.Text = "请输入0-100的频率占空比";
            }
            EventHandlers();
        }

        private void EventHandlers()
        {
            this.input_0.Click += Input_0_Click;
            this.input_1.Click += Input_1_Click;
            this.input_2.Click += Input_2_Click;
            this.input_3.Click += Input_3_Click;
            this.input_4.Click += Input_4_Click;
            this.input_5.Click += Input_5_Click;
            this.input_6.Click += Input_6_Click;
            this.input_7.Click += Input_7_Click;
            this.input_8.Click += Input_8_Click;
            this.input_9.Click += Input_9_Click;
            this.input_d.Click += Input_d_Click;
            this.btn_clear.Click += Btn_clear_Click;
            this.btn_del.Click += Btn_del_Click;
            this.btn_ok.Click += Btn_ok_Click1;
            this.btn_cancel.Click += Btn_cancel_Click;
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Input_d_Click(object sender, EventArgs e)
        {
            this.tb_input.Text += ".";
        }

        private void Btn_ok_Click1(object sender, EventArgs e)
        {
            try
            {
                var inputString = this.tb_input.Text.Trim();
                if (inputString == "")
                {
                    MessageBox.Show("输入参数不能为空！", "warn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                inputValue = double.Parse(inputString);
                //判断范围
                if (currentDaTaType == DataType.Voltage)
                {
                    //0-5
                    if (inputValue < 0 || inputValue > 5)
                    {
                        MessageBox.Show("请输入0-5V电压","提示",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                        return;
                    }
                }
                else if (currentDaTaType == DataType.PwmFrequency)
                {
                    //0-20000
                    if (inputValue < 0 || inputValue > 20000)
                    {
                        MessageBox.Show("请输入0-20000Hz频率", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else if (currentDaTaType == DataType.PwmFrequencyPersent)
                {
                    //0-100
                    if (inputValue < 0 || inputValue > 100)
                    {
                        MessageBox.Show("请输入0-100频率占空比", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"输入参数格式错误={ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_del_Click(object sender, EventArgs e)
        {
            //删除最后一个字符
            var inputString = this.tb_input.Text;
            if (inputString.Length < 1)
                return;
            this.tb_input.Text = inputString.Substring(0,inputString.Length - 1);
        }

        private void Btn_clear_Click(object sender, EventArgs e)
        {
            this.tb_input.Clear();
        }

        private void Input_9_Click(object sender, EventArgs e)
        {
            this.tb_input.Text += "9";
        }

        private void Input_8_Click(object sender, EventArgs e)
        {
            this.tb_input.Text += "8";
        }

        private void Input_7_Click(object sender, EventArgs e)
        {
            this.tb_input.Text += "7";
        }

        private void Input_6_Click(object sender, EventArgs e)
        {
            this.tb_input.Text += "6";
        }

        private void Input_5_Click(object sender, EventArgs e)
        {
            this.tb_input.Text += "5";
        }

        private void Input_4_Click(object sender, EventArgs e)
        {
            this.tb_input.Text += "4";
        }

        private void Input_3_Click(object sender, EventArgs e)
        {
            this.tb_input.Text += "3";
        }

        private void Input_2_Click(object sender, EventArgs e)
        {
            this.tb_input.Text += "2";
        }

        private void Input_1_Click(object sender, EventArgs e)
        {
            this.tb_input.Text += "1";
        }

        private void Input_0_Click(object sender, EventArgs e)
        {
            this.tb_input.Text += "0";
        }
    }
}
