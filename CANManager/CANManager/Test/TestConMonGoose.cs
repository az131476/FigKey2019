using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace CANManager.Test
{
    public partial class TestConMonGoose : Form
    {
        [DllImport("controlcan.dll")]
        static extern UInt32 VCI_OpenDevice(UInt32 DeviceType, UInt32 DeviceInd, UInt32 Reserved);

        public TestConMonGoose()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            //连接
            textBox1.Text = "" + TestMonGoose.MongooseProISO2Setup();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            //初始化
            int m = 4;
            textBox1.Text = TestMonGoose.MongooseProISO2FastInit(new uint[] { 1, 2 }, 2, new uint[] { 2, 1 }, ref m) + "";
        }

        private void Button5_Click(object sender, EventArgs e)
        {
            //写数据
            uint timeOut = 10;
            textBox1.Text = TestMonGoose.MongooseProISO2WriteMsg(new uint[] { 1,2},2,timeOut)+"";
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            //写数据
            int input = 2;
            textBox1.Text = TestMonGoose.MongooseProISO2ReadMsg(new uint[] { 1,2},ref input,10)+"";
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = TestMonGoose.MongooseProISO2Close()+"";
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = ""+VCI_OpenDevice(4,0,0);
        }
    }
}
