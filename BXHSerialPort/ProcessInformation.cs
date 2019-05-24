using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BXHSerialPort
{
    public partial class ProcessInformation : Form
    {
        public ProcessInformation()
        {
            InitializeComponent();
        }
        FormSerialPortHelper serial = new FormSerialPortHelper();
        Stopwatch sw = new Stopwatch ();
        Timer timer = new Timer();

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] sendBytes = {0xEF,10 };
            serial.comSend(sendBytes);
            progressBar1.Maximum = Convert.ToInt32(textBox2.Text);
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            sw.Restart();
            timer.Start();
        }

        private void ProcessInformation_Load(object sender, EventArgs e)
        {
            timer.Interval = 1000;
            timer.Tick += new EventHandler(Timer_Tick);
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            byte[] readBytes = serial.comReceive();
            for (int i = 0; i < readBytes.Length; i++)
            {
                if (readBytes.Length > i + 3&&readBytes[i]==0xef&& readBytes[i+1] == 0x22)
                {
                    int nowPositon = (UInt16)(readBytes[i + 2] << 8) + readBytes[i + 3];
                    label3.Text = nowPositon + "/" + progressBar1.Maximum;
                    if (nowPositon>= progressBar1.Maximum)
                    {
                        progressBar1.Value = progressBar1.Maximum;
                        timer.Stop();
                        textBox1.Text += "，刷写完毕";
                        return;
                    }
                    progressBar1.Value = nowPositon;
                    i = i + 3;
                }
            }
            string elapsedTicks = (sw.ElapsedTicks / (decimal)Stopwatch.Frequency).ToString("F2");
            textBox1.Text = "测试用时：" + elapsedTicks + "秒";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            serial.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "请选择烧写文件";
            fileDialog.CheckFileExists = false;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = fileDialog.FileName;
            }
        }
    }
}
