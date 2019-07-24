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
            //url_login = "http://localhost:9000/swagger/index.html?urls.primaryName=Authorize";
            Http http = new Http();
            CommonUtils.API.HttpItem httpItem = new CommonUtils.API.HttpItem();
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"),5000);
            httpItem.URL = url_login;
            //httpItem.IPEndPoint = iPEndPoint;
            httpItem.PostEncoding = Encoding.UTF8;
            httpItem.Method = "Post";
            //httpItem.Accept = "text/xml";
            httpItem.ContentType = "application/json";
            httpItem.ProxyUserName = "{\"UserName\":\"gsw\"";
            httpItem.ProxyPwd = "\"Password\":\"111111\"}";
            CommonUtils.API.HttpResult httpResult = http.GetHtml(httpItem);

            textBox1.Text += "status:" + httpResult.StatusCode+"\r\n";
            textBox1.Text += "result:" + httpResult.Html + "\r\n";
        }

        private void Api1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string url = "http://localhost:9000/api01/Values";
            string postData = "{\"UserName\":\"gsw\",\"Password\":\"111111\"}";
            string token = "Authorize:Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZ3N3IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDE5LzgvMjggMTU6NDk6NDgiLCJuYmYiOjE1NjM5Nzg1ODgsImV4cCI6MTU2Njk3ODU4OCwiaXNzIjoiZ3N3IiwiYXVkIjoiZXZlcnlvbmUifQ.HQdW7N_XTWXfaT-Zj7LzPqRJEMypmS49-i61ymjkz0Q";
            PostFunc(url,token,postData);
        }

        //登录验证成功
        public void HttpSwaggerInterfaceTest()
        {
            string url = "http://localhost:9000/auth/Login";
            url = "http://localhost:9000/api01/Values";
            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(url);
            wReq.Method = "POST";
            //wReq.ContentType = "application/json"; 
            //wReq.ContentType = "application/x-www-form-urlencoded"; 
            wReq.ContentType = "application/json";
            wReq.Accept = "text/plain";
            //wReq.Headers = "Authorization:bearer Token";

            string postData = "{\"UserName\":\"gsw\",\"Password\":\"111111\",\"Authorization\"}";
            //postData = "{\"Authorization\":\"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZ3N3IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDE5LzgvMjggMTU6NDk6NDgiLCJuYmYiOjE1NjM5Nzg1ODgsImV4cCI6MTU2Njk3ODU4OCwiaXNzIjoiZ3N3IiwiYXVkIjoiZXZlcnlvbmUifQ.HQdW7N_XTWXfaT-Zj7LzPqRJEMypmS49-i61ymjkz0Q\"}";
            byte[] data = Encoding.Default.GetBytes(postData);
            wReq.ContentLength = data.Length;
            Stream reqStream = wReq.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
            using (StreamReader sr = new StreamReader(wReq.GetResponse().GetResponseStream()))
            {
                string result = sr.ReadToEnd();
                textBox1.Text = result;
            }
        }

        public string PostFunc(string apiUrl, string token, string param)
        {
            string strJson = string.Empty;
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                request = WebRequest.Create(apiUrl) as HttpWebRequest;
                request.AllowWriteStreamBuffering = true;//
                //request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";

                byte[] b = Encoding.UTF8.GetBytes(param);
                //Header中添加token验证（服务器要求）
                request.Headers.Add("eg-token", token);//添加token到header
                request.Headers.Add("Content-Type", "application/json");
                //request.ContentType = "application/json";//已有token使用json类型
                request.ContentLength = b.Length;
                request.Method = "POST";
                if (b.Length > 0)
                {
                    using (Stream stream = request.GetRequestStream())
                    {
                        stream.Write(b, 0, b.Length);
                        stream.Close();
                    }
                }
                //获取服务器返回
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                    {
                        strJson = reader.ReadToEnd();
                        textBox1.Text = strJson;
                        reader.Close();
                    }
                }
                return strJson;
            }
            catch (Exception exception)
            {
                return exception.Message;
            }
            finally
            {
                if (response != null)
                    response.Close();
            }
        }
    }
}
