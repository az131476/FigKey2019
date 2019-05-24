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
    public partial class FormProductSetting : Form
    {
        public FormProductSetting()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                INIHelper ini = new BXHSerialPort.INIHelper();
                string[] FolderShareKey = { "FolderSharePath", "User", "Password", "Local", "FolderSharePath2", "User2", "Password2", "FolderSharePath3", "User3", "Password3" };
                string[] FolderShareValue = { textBox1.Text, textBox2.Text, textBox3.Text ,radioButton1.Checked.ToString(), textBox6.Text, textBox5.Text, textBox4.Text, textBox9.Text, textBox8.Text, textBox7.Text};
                ini.write(FolderShareKey, FolderShareValue);
            }
            catch (Exception)
            {
                MessageBox.Show("配置失败！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            MessageBox.Show("配置成功！");
        }

        private void FormProductSetting_Load(object sender, EventArgs e)
        {
            INIHelper iniHelper = new INIHelper();
            string[] FolderShareSetting = new string[10];
            string[] FolderShareKey = { "FolderSharePath", "User", "Password", "Local" ,"FolderSharePath2", "User2", "Password2", "FolderSharePath3", "User3", "Password3"};
            iniHelper.read(FolderShareKey, out FolderShareSetting);
            textBox1.Text = FolderShareSetting[0];
            textBox2.Text = FolderShareSetting[1];
            textBox3.Text = FolderShareSetting[2];
            radioButton1.Checked = FolderShareSetting[3]=="True";
            radioButton2.Checked = FolderShareSetting[3] == false.ToString();
            textBox6.Text = FolderShareSetting[4];
            textBox5.Text = FolderShareSetting[5];
            textBox4.Text = FolderShareSetting[6];
            textBox9.Text = FolderShareSetting[7];
            textBox8.Text = FolderShareSetting[8];
            textBox7.Text = FolderShareSetting[9];
        }

        private void 曲线范围配置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormCurrentRange f = new BXHSerialPort.FormCurrentRange();
            f.Show();
        }
    }
}
