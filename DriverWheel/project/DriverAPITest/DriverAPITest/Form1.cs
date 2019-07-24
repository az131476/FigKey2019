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
using CommonUtils.API;
using CommonUtils.Logger;
using System.Net;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;

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

        }

        private const String host = "https://znys.market.alicloudapi.com";
        private const String path = "/sent.do";
        private const String method = "GET";
        private const String appcode = "7f4251934db54cb890afd00d91c09fb3";

        public void MaInterface()
        {
            String querys = "content=%E6%84%9F%E5%86%92%E4%BA%86%E6%80%8E%E4%B9%88%E5%8A%9E&product=znys&uuid=1234567890";
            String bodys = "";
            String url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.Method = method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            textBox1.Text += "statusCode:" + httpResponse.StatusCode + "\r\n";
            textBox1.Text += "method:" + httpResponse.Method + "\r\n";
            textBox1.Text += "header:" + httpResponse.Headers + "\r\n";
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            textBox1.Text += "content:" + reader.ReadToEnd();

        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        private void TestAPI()
        {
            //var url = "https://zehusdrivermanufacturer-integr.azurewebsites.net/index.html";
            //url = "https://zehusdrivermanufacturer-integr.azurewebsites.net/api/v1/FirmwareProductTypes?page=1&rows=5";
            //var url_authorize = "https://zehusidentityserverwebintegration.azurewebsites.net/{common}/connect/authorize?response_type=code&scope=ProductionApi&client_id=SwaggerProduction";
            //var url_login = "https://zehusidentityserverwebintegration.azurewebsites.net/Account/Login?ReturnUrl=/connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAxOSAyMDE5IDE2OjM3OjAxIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";

            textBox1.Clear();
            string loginpage = "https://zehusidentityserverwebintegration.azurewebsites.net/Account/Login?ReturnUrl=%2Fgrants";
            string loginOld = "https://zehusidentityserverwebintegration.azurewebsites.net/Account/Login?ReturnUrl=/connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=VHVlIEp1bCAyMyAyMDE5IDA5OjI5OjU4IEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";
            HttpHelps httpHelps = new HttpHelps();
            var resStr = httpHelps.GetHttpRequestStringByNUll_Get(loginOld, Encoding.UTF8);
            LogHelper.Log.Info(resStr + "\r\n\r\n");
            textBox1.Text = resStr;
        }

        private void Api2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            //MaInterface();
            string url_login = "http://localhost:9000/auth/Login";
            //http://localhost:9000/swagger/index.html?urls.primaryName=Authorize%20-%E3%80%90SwaggerAuthorize%E3%80%91
            //http://localhost:9000/swagger/index.html?urls.primaryName=API01%20-%E3%80%90SwaggerAPI01%E3%80%91
            Http http = new Http();
            CommonUtils.API.HttpItem httpItem = new CommonUtils.API.HttpItem();
            httpItem.URL = "http://localhost:9000/auth/Login";
            httpItem.Method = "POST";
            CommonUtils.API.HttpResult httpResult = http.GetHtml(httpItem);

            textBox1.Text += "status:" + httpResult.StatusCode+"\r\n";
            textBox1.Text += "result:" + httpResult.Html + "\r\n";
        }

        private void Api1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestAPI();
        }
    }

}
