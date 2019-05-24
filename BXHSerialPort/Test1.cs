using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BXHSerialPort
{
    public partial class Test1 : Form
    {
        public Test1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GetProgramData pgmData = new GetProgramData();

            string pressureDataPath = @"./pressureData.txt";
            List<List<string>> pressureData = pgmData.readPressure(pressureDataPath);

            //string currentDataPath = @"\\10.200.8.73\share\1.csv";
            string currentDataPath = @textBox3.Text;
            //MessageBox.Show(currentDataPath, "读电流前错误", MessageBoxButtons.OK, MessageBoxIcon.Error);

            string currStandardDataPath = @"./currStandardData.txt";
            List<List<string>> currStandardData = pgmData.readPressure(currStandardDataPath);

            string currStandardDataSearchPath = @"./currStandardDataSearch.txt";
            List<List<string>> currStandardDataSearch = pgmData.readPressure(currStandardDataSearchPath);

            List<List<float>> currentData;
            try
            {
                currentData = pgmData.readCSVgetCurrent(pressureData, @currentDataPath, currStandardData, currStandardDataSearch);
                //MessageBox.Show("读电流成功", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ee)
            {
                MessageBox.Show("读原始数据：" + ee.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string nowTime = System.DateTime.Now.ToString("_yyyy_MM_dd_HH_mm_ss_ffff");
            string p = @"./CURRENT" + nowTime + ".txt";
            pgmData.writeCurrent(@p, currentData);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "请选择电流数据文件";
            fileDialog.Filter = "*.csv|*.csv";
            //fileDialog.CheckFileExists = false;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = fileDialog.FileName;
            }
        }
    }
}
