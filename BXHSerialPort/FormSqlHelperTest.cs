using BookDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BXHSerialPort
{
    public partial class FormSqlHelperTest : Form
    {
        public FormSqlHelperTest()
        {
            InitializeComponent();
        }
        private void FormSqlHelperTest_Load(object sender, EventArgs e)
        {
            
        }
        
        void dbSelect()
        {
            //连接数据库
            SqlConnection con = SqlHelper.GetConnection();
            //查询
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "select* from [dbo].[Table]");
            string s = "";
            foreach (DataRow col in ds.Tables[0].Rows)
            {
                for (int i = 0; i < 2; i++)
                {
                    Console.WriteLine(col[i].ToString());
                    s += col[i].ToString();
                }
            }
            textBox1.Text = s;
        }
        private void dbActions(object sender, EventArgs e)
        {
            //连接数据库
            SqlConnection con = SqlHelper.GetConnection();
            Console.WriteLine("数据库连接成功");

            //建立一张表
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, "create table student(id int primary key,name varchar(20),age int)");
            Console.WriteLine("建表成功");

            //插入数据
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, "insert into student values(1,'Ghazi',21)");
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, "insert into student values(2,'Jack',20)");
            Console.WriteLine("数据插入成功");

            //查询
            DataSet ds = SqlHelper.ExecuteDataset(con, CommandType.Text, "select* from student");
            foreach (DataRow col in ds.Tables[0].Rows)
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine(col[i].ToString());
                }
            }

            //修改
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, "update student set age=22 where id=2");
            Console.WriteLine("数据修改成功");

            DataSet ds1 = SqlHelper.ExecuteDataset(con, CommandType.Text, "select* from student");
            foreach (DataRow col in ds1.Tables[0].Rows)
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine(col[i].ToString());
                }
            }

            //删除
            SqlHelper.ExecuteNonQuery(con, CommandType.Text, "delete from student where id=2");
            Console.WriteLine("数据删除成功");

            DataSet ds2 = SqlHelper.ExecuteDataset(con, CommandType.Text, "select* from student");
            foreach (DataRow col in ds2.Tables[0].Rows)
            {
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine(col[i].ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Title = "请选择CSV文件";
            fileDialog.Filter = "*.txt|*.txt";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string fileName = fileDialog.FileName;
                //List<List<float>> pressure = readPressure(fileName);
                //string s = "";
                //foreach (var item in pressure)
                //{
                //    foreach (var item2 in item)
                //    {
                //        s += item2;
                //    }
                //    s += "\r\n";
                //}
                //textBox1.Text = s;

                List<List<float>> current = new List<List<float>>();
                List<float> l1 = new List<float>();
                for (int i = 0; i < 23; i++)
                {
                    l1.Add(i/3);
                }
                current.Add(l1);
                List<float> l2 = new List<float>();
                for (int i = 0; i < 31; i++)
                {
                    l2.Add(i / 3);
                }
                current.Add(l2);
                GetProgramData pgmData = new GetProgramData();
                pgmData.writeCurrent(fileName, current);
            }

        }
    }
}
