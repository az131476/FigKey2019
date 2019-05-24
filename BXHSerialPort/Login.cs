using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BXHSerialPort
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        string passwordPath = "./password.txt";
        string md5Tail = "Abcoo";
        public bool Administrator = false;
        private void button1_Click(object sender, EventArgs e)
        {
            string[] allLines = File.ReadAllLines(passwordPath);
            string username = textBox1.Text;
            string password = textBox2.Text;
            if (password.Length >= 6 && username.Length >= 2)
            {
            }
            else
            {
                MessageBox.Show("用户名与密码长度需要分别大于2和6！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string passwordMD5 = MD5Helper.GetMD5(password+md5Tail);
            bool isTrue = false;
            for (int i = 0; i < allLines.Length/2; i++)
            {
                if (username == allLines[i*2]&& passwordMD5==allLines[i*2+1])
                {
                    isTrue = true;
                    break;
                }
            }
            if (isTrue)
            {
                MessageBox.Show("登录成功！", "欢迎使用", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Administrator = true;
                button2.Enabled = true;
                this.Close();
            }
            else
            {
                Administrator = false;
                button2.Enabled = false;
                MessageBox.Show("用户名或密码错误！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //File.WriteAllLines(passwordPath,new string[]{ username,passwordMD5});
            //File.AppendAllLines(passwordPath, new string[] { username, passwordMD5 });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            if (password.Length >=6&&username.Length>=2)
            {
                string passwordMD5 = MD5Helper.GetMD5(password + md5Tail);
                File.AppendAllLines(passwordPath, new string[] { username, passwordMD5 });
                MessageBox.Show("注册成功！", "欢迎使用", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("用户名与密码长度需要分别大于2和6！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            button2.Enabled = false;
        }
    }
}
