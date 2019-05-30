using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Web;
using System.Web.Services;
using System.ServiceModel;


namespace LoggerConfigurator
{
    public partial class TestServer : Form
    {
        public TestServer()
        {
            InitializeComponent();
            this.Disposed += TestServer_Disposed;
        }

        private void TestServer_Disposed(object sender, EventArgs e)
        {
            server.Close();
        }

        ServiceReference1.FigKeyLoggerServiceClient server;

        private void Button1_Click(object sender, EventArgs e)
        {
            //string res = server.GetData(2309);
            //ServiceReference1.CompositeType comType = new ServiceReference1.CompositeType();
            //comType.StringValue = "100";
            //comType.BoolValue = false;
            //server.GetDataUsingDataContract(comType);

            //MessageBox.Show(res);
            //bool b = server.Login("Fig","key");
            //MessageBox.Show(" login status:"+b);
        }

        private void TestServer_Load(object sender, EventArgs e)
        {
            //server = new ServiceReference1.FigKeyLoggerServiceClient();
            //bool b = server.SQLConnection();
            //MessageBox.Show(b + " sql connection");
        }
    }
}
