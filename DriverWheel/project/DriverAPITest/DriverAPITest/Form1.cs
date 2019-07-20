using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtils.API.HTTP;
using CommonUtils.Logger;

namespace DriverAPITest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /*
         * GET /authorize?
    response_type=code
    &scope=openid%20profile%20email
    &client_id=s6BhdRkqt3
    &state=af0ifjsldkj
    &redirect_uri=https%3A%2F%2Fclient.example.org%2Fcb HTTP/1.1
    Host: server.example.com
    */

        private void Form1_Load(object sender, EventArgs e)
        {
            var url = "https://zehusdrivermanufacturer-integr.azurewebsites.net/index.html";
            url = "https://zehusdrivermanufacturer-integr.azurewebsites.net/api/v1/FirmwareProductTypes?page=1&rows=5";
            var url_authorize = "https://zehusidentityserverwebintegration.azurewebsites.net/{common}/connect/authorize?response_type=code&scope=ProductionApi&client_id=SwaggerProduction";
            var url_login = "https://zehusidentityserverwebintegration.azurewebsites.net/Account/Login?ReturnUrl=/connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAxOSAyMDE5IDE2OjM3OjAxIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";
            HttpHelps httpHelps = new HttpHelps();
            var resStr = httpHelps.GetHttpRequestStringByNUll_Get(url_login, Encoding.UTF8);
            LogHelper.Log.Info(resStr+"\r\n\r\n");
            textBox1.Text = resStr;
        }
    }
}
