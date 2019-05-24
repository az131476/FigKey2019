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
    public partial class FormCurrentRange : Form
    {
        public FormCurrentRange()
        {
            InitializeComponent();
        }

        private void FormCurrentRange_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 6; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i].HeaderCell.Value = (i+1).ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> list = new List<string>();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    list.Add(dataGridView1.Rows[i].Cells[j].Value.ToString());
                }
            }
            CurrRangeCtl c = new CurrRangeCtl();
            c.write(list.ToArray());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CurrRangeCtl c = new CurrRangeCtl();
            string[] setting = c.read();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    dataGridView1.Rows[i].Cells[j].Value = setting[i*4+j];
                }
            }
        }
    }
}
