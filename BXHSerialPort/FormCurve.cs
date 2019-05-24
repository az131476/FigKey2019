using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace BXHSerialPort
{
    public partial class FormCurve : Form
    {
        public FormCurve()
        {
            InitializeComponent();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            chart1.Series.Clear();
            List<List<float>> dataList;
            string fileName = textBox1.Text;
            string[] tag = File.ReadAllLines(@".\SearchString.txt");
            dataList = CSVHelper.OpenCSV(@fileName,tag);
            //start
            int iStart = Convert.ToInt32(textBox2.Text)*4;
            for (int i = 0; i < 2; i++)
            {
                Series s1 = new Series("s"+i);
                s1.ChartType = SeriesChartType.Spline;
                s1.IsValueShownAsLabel = true;

                float[] x = dataList[iStart+i*2].ToArray();
                float[] y = dataList[iStart+i*2+1].ToArray();
                for (int j = 0; j < x.Length; j++)
                {
                    s1.Points.AddXY(x[j], y[j]);
                }
                chart1.Series.Add(s1);
            }



        }
    }
}
