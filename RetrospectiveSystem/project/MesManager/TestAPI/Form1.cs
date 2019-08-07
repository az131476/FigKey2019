using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestAPI
{
    public partial class Form1 : Form
    {
        private MesService.MesServiceClient serviceClient;
        public Form1()
        {
            InitializeComponent();
        }

        async private void Button1_Click(object sender, EventArgs e)
        {
            var sn = tb_sn.Text.Trim();
            var result = tb_result.Text.Trim();
            var typeno = tb_typeno.Text.Trim();
            var station = tb_station.Text.Trim();
            var time = tb_date.Text.Trim();
            textBox1.Text = "";
            textBox1.Text = await serviceClient.InsertTestResultDataAsync(sn,typeno,station,time,result);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tb_sn.Text = "sn_001792j";
            tb_typeno.Text = "typeNo00912";
            tb_station.Text = "FIRST";
            tb_result.Text = "PASS";
            tb_date.Text = "2019-01-09 12:12:00";
            serviceClient = new MesService.MesServiceClient();
        }

        async private void Button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            var sn = "sn_001";
            var typeno = "type_no_0001";
            var station = "station_name_001";

            string[] array = await serviceClient.SelectLastTestResultAsync(sn,typeno,station);
        }

        async private void Button3_Click(object sender, EventArgs e)
        {
            var res = await serviceClient.UpdatePackageProductAsync("20190806code","0003","0");
            textBox1.Text = res.ToString();
        }
    }
}
