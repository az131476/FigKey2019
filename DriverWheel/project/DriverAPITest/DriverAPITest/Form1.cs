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
            //MaInterface();
            //MaInterfaceSD();
            //SendRequest2();
            SendRequest3();
        }


        private void Api1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //HttpSwaggerInterfaceTest();
            //HttpSwaggerInterfaceSD();
            //GetAuthonToken();
            //SendRequest1();
            textBox1.Text = WcfToken.GetAccessToken().ToString();
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

        //登录验证成功
        public void HttpSwaggerInterfaceTest()
        {
            string url = "http://localhost:9000/auth/Login";
            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(url);
            wReq.Method = "POST";
            //wReq.ContentType = "application/json"; 
            //wReq.ContentType = "application/x-www-form-urlencoded"; 
            wReq.ContentType = "application/json";
            //wReq.Accept = "text/plain";
            //wReq.Headers = "Authorization:bearer Token";

            string postData = "{\"UserName\":\"gsw\",\"Password\":\"111111\"}";
            //postData = "UserName=gsw&Password=111111";
            //postData = "{\"Authorization\":\"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZ3N3IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDE5LzgvMjggMTU6NDk6NDgiLCJuYmYiOjE1NjM5Nzg1ODgsImV4cCI6MTU2Njk3ODU4OCwiaXNzIjoiZ3N3IiwiYXVkIjoiZXZlcnlvbmUifQ.HQdW7N_XTWXfaT-Zj7LzPqRJEMypmS49-i61ymjkz0Q\"}";
            byte[] data = Encoding.Default.GetBytes(postData);
            wReq.ContentLength = data.Length;
            Stream reqStream = wReq.GetRequestStream();
            reqStream.Write(data, 0, data.Length);
            reqStream.Close();
            using (StreamReader sr = new StreamReader(wReq.GetResponse().GetResponseStream()))
            {
                string result = sr.ReadToEnd();
                textBox1.Text = result + "\r\n";
            }
        }

        //登录获取token
        public void HttpSwaggerInterfaceSD()
        {
            string url = "https://zehusidentityserverwebintegration.azurewebsites.net/connect/authorize";//验证的URL

            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(url);
            wReq.Method = "GET";
            //wReq.ContentType = "application/json"; 
            //wReq.ContentType = "application/x-www-form-urlencoded"; 
            //wReq.ContentType = "application/json";
            //wReq.Accept = "application/json";
            //wReq.Headers = "Authorization:bearer Token";

            string postData = "{\"UserName\":\"SelcomGroupUser@grr.la\",\"Password\":\"%7LD2GPfNjv8\",\"client_id\":\"SwaggerProduction\"}";
            //postData = "{\"client_id\":\"SwaggerProduction\"}";

            //byte[] data = Encoding.Default.GetBytes(postData);
            //wReq.ContentLength = data.Length;
            //Stream reqStream = wReq.GetRequestStream();
            //reqStream.Write(data, 0, data.Length);
            //reqStream.Close();
            using (StreamReader sr = new StreamReader(wReq.GetResponse().GetResponseStream()))
            {
                string result = sr.ReadToEnd();
                textBox1.Text = result + "\r\n";
            }
        }

        public void MaInterface()
        {
            string host = "http://localhost:9000/auth/Login";
            string path = "/sent.do";
            string method = "POST";
            string appcode = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZ3N3IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDE5LzgvMjkgMjo1Mzo1NyIsIm5iZiI6MTU2NDAxODQzNywiZXhwIjoxNTY3MDE4NDM3LCJpc3MiOiJnc3ciLCJhdWQiOiJldmVyeW9uZSJ9.dF3Fv_bG4V6cSatrZYwjFayXDVesDW2dMlj56zHRKiQ";

            host = "http://localhost:9000/api01/Values";
            string bodys = "{\"UserName\":\"gsw\",\"Password\":\"111111\",\"id\":\"2934\",\"describe\":\"this is dec..\",\"price\":\"3923\"}";
            //String querys = postData;//"content=%E6%84%9F%E5%86%92%E4%BA%86%E6%80%8E%E4%B9%88%E5%8A%9E&product=znys&uuid=1234567890";
            String url = host;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

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
            httpRequest.ContentType = "application/json";
            httpRequest.Accept = "text/plain";
            httpRequest.Headers.Add("Authorization", "Bearer " + appcode);
            //httpRequest.Headers.Add("{ \"id\": 11, \"describe\": \"this is describle ...\", \"price\": 101, \"isSure\": true}");
            byte[] data = Encoding.Default.GetBytes(bodys);
            using (Stream stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
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

        public void MaInterfaceSD()
        {
            //https://zehusdrivermanufacturer-integr.azurewebsites.net/api/v1/FirmwareProductTypes?page=5&rows=5
            string host = "https://zehusdrivermanufacturer-integr.azurewebsites.net/api/v1/FirmwareProductTypes?page=6&rows=51&TotalPages=1";
            //https://zehusidentityserverwebintegration.azurewebsites.net/connect/authorize
            host = "https://zehusdrivermanufacturer-integr.azurewebsites.net/api/v1/FirmwareProductType/00000000-0000-0000-0000-000000000000&TotalPages=1";
            host = "https://zehusdrivermanufacturer-integr.azurewebsites.net/api/v1/FirmwareType/00000000-0000-0000-0000-000000000001/Firmware&TotalPages=1";
            host = "https://zehusdrivermanufacturer-integr.azurewebsites.net/api/v1/Drivers?page=1&rows=2&TotalPages=1";
            string method = "GET";
            string appcode = "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6Ijg4RUMwRjUzRUIyODczMTM1NzlCMTM2ODc5QjREQkYxODFGNzJCQTIiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJpT3dQVS1zb2N4TlhteE5vZWJUYjhZSDNLNkkifQ.eyJuYmYiOjE1NjQwMjE5MzUsImV4cCI6MTU2NDAyNTUzNSwiaXNzIjoiaHR0cHM6Ly96ZWh1c2lkZW50aXR5c2VydmVyd2ViaW50ZWdyYXRpb24uYXp1cmV3ZWJzaXRlcy5uZXQiLCJhdWQiOlsiaHR0cHM6Ly96ZWh1c2lkZW50aXR5c2VydmVyd2ViaW50ZWdyYXRpb24uYXp1cmV3ZWJzaXRlcy5uZXQvcmVzb3VyY2VzIiwiUHJvZHVjdGlvbkFwaSJdLCJjbGllbnRfaWQiOiJTd2FnZ2VyUHJvZHVjdGlvbiIsInN1YiI6IjE1ZmNmMjY5LWUzNmEtNDQxZC1iMzNkLTMxY2ZhNzc0NTgwOSIsImF1dGhfdGltZSI6MTU2NDAyMTkzNSwiaWRwIjoibG9jYWwiLCJyb2xlIjoiQjJDVXNlciIsInNjb3BlIjpbIlByb2R1Y3Rpb25BcGkiXSwiYW1yIjpbInB3ZCJdfQ.RTJVVsNWTKpt9FtAojjwNGGbWO5e4ASsd7cEQcBQxRd5EuMMf-4I7KxnYEpKwn2-EQMh66aF7c3FuaE8J06Dog8t3E2Id4AhC6202UgFjMX0QwoygqWDzrlo3WMGVGJ6NDF5wz2B5848YkynFsIMa3nQKLgp9bU5wL_uBfOoOXiETIgojQFaRiuodUeWbwugNnhw3cqUoFVcNwD421_OJ3rOFi1ZEjQ3bw8D9XeVQmN50YIv5_D40-pJfRIswt58aWSMn7pPngpAAyEByqXDDcAk0ZdGqzEIfiyQ4Ol6O6CMz8iwHbYB9NwtwfSrdTMybjU45kKL18BB2E1O1KUEGQ";
            appcode = "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6Ijg4RUMwRjUzRUIyODczMTM1NzlCMTM2ODc5QjREQkYxODFGNzJCQTIiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJpT3dQVS1zb2N4TlhteE5vZWJUYjhZSDNLNkkifQ.eyJuYmYiOjE1NjQwMjM2NDUsImV4cCI6MTU2NDAyNzI0NSwiaXNzIjoiaHR0cHM6Ly96ZWh1c2lkZW50aXR5c2VydmVyd2ViaW50ZWdyYXRpb24uYXp1cmV3ZWJzaXRlcy5uZXQiLCJhdWQiOlsiaHR0cHM6Ly96ZWh1c2lkZW50aXR5c2VydmVyd2ViaW50ZWdyYXRpb24uYXp1cmV3ZWJzaXRlcy5uZXQvcmVzb3VyY2VzIiwiUHJvZHVjdGlvbkFwaSJdLCJjbGllbnRfaWQiOiJTd2FnZ2VyUHJvZHVjdGlvbiIsInN1YiI6IjE1ZmNmMjY5LWUzNmEtNDQxZC1iMzNkLTMxY2ZhNzc0NTgwOSIsImF1dGhfdGltZSI6MTU2NDAyMTkzNSwiaWRwIjoibG9jYWwiLCJyb2xlIjoiQjJDVXNlciIsInNjb3BlIjpbIlByb2R1Y3Rpb25BcGkiXSwiYW1yIjpbInB3ZCJdfQ.JueAHYYwymNLXtrmr65INE8L3sIZwy1YfgdDvKiVGU2Ksw6F8YN9QiGh2hb1ptQ1td4YwIewQwfcqQzEk8tMAuDHg_leI3r8cY11be2LV_p8l6FfcHkXGpLWmAlhPXJ0OerLWkeGU-03i6cVgTQUh-Kw5eTNfqa_D9Ro7XZEP_3OIhxZxNx4CTIU_yCAxNefI8yy2umzlp9gV-jfdAbz4P0NHGT4fDWMtxfl7GL_rAb6Ypt4lDO2NDd2MgztpvKVNhKThkkaVzRo5luaGAy2eLneV8rP6Z4-UFhJ6GUmbH4inM2tWYpk_gIgQuS4H60h4FWCzEhX1gGT0Wc0ZnFqTQ";
            appcode = "Bearer eyJhbGciOiJSUzI1NiIsImtpZCI6Ijg4RUMwRjUzRUIyODczMTM1NzlCMTM2ODc5QjREQkYxODFGNzJCQTIiLCJ0eXAiOiJKV1QiLCJ4NXQiOiJpT3dQVS1zb2N4TlhteE5vZWJUYjhZSDNLNkkifQ.eyJuYmYiOjE1NjQwMjM2NDUsImV4cCI6MTU2NDAyNzI0NSwiaXNzIjoiaHR0cHM6Ly96ZWh1c2lkZW50aXR5c2VydmVyd2ViaW50ZWdyYXRpb24uYXp1cmV3ZWJzaXRlcy5uZXQiLCJhdWQiOlsiaHR0cHM6Ly96ZWh1c2lkZW50aXR5c2VydmVyd2ViaW50ZWdyYXRpb24uYXp1cmV3ZWJzaXRlcy5uZXQvcmVzb3VyY2VzIiwiUHJvZHVjdGlvbkFwaSJdLCJjbGllbnRfaWQiOiJTd2FnZ2VyUHJvZHVjdGlvbiIsInN1YiI6IjE1ZmNmMjY5LWUzNmEtNDQxZC1iMzNkLTMxY2ZhNzc0NTgwOSIsImF1dGhfdGltZSI6MTU2NDAyMTkzNSwiaWRwIjoibG9jYWwiLCJyb2xlIjoiQjJDVXNlciIsInNjb3BlIjpbIlByb2R1Y3Rpb25BcGkiXSwiYW1yIjpbInB3ZCJdfQ.JueAHYYwymNLXtrmr65INE8L3sIZwy1YfgdDvKiVGU2Ksw6F8YN9QiGh2hb1ptQ1td4YwIewQwfcqQzEk8tMAuDHg_leI3r8cY11be2LV_p8l6FfcHkXGpLWmAlhPXJ0OerLWkeGU-03i6cVgTQUh-Kw5eTNfqa_D9Ro7XZEP_3OIhxZxNx4CTIU_yCAxNefI8yy2umzlp9gV-jfdAbz4P0NHGT4fDWMtxfl7GL_rAb6Ypt4lDO2NDd2MgztpvKVNhKThkkaVzRo5luaGAy2eLneV8rP6Z4-UFhJ6GUmbH4inM2tWYpk_gIgQuS4H60h4FWCzEhX1gGT0Wc0ZnFqTQ";
            string bodys = "{\"UserName\":\"SelcomGroupUser@grr.la\",\"Password\":\"%7LD2GPfNjv8\"}";
            //String querys = postData;//"content=%E6%84%9F%E5%86%92%E4%BA%86%E6%80%8E%E4%B9%88%E5%8A%9E&product=znys&uuid=1234567890";
            String url = host;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

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
            httpRequest.ContentType = "application/json";
            httpRequest.Accept = "application/json";

            httpRequest.Headers.Add("Authorization", appcode);
            //httpRequest.Headers.Add("{ \"id\": 11, \"describe\": \"this is describle ...\", \"price\": 101, \"isSure\": true}");
            byte[] data = Encoding.Default.GetBytes(bodys);
            //using (Stream stream = httpRequest.GetRequestStream())
            //{
            //    stream.Write(data, 0, data.Length);
            //}
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

        public void GetAuthonToken()
        {
            //https://zehusidentityserverwebintegration.azurewebsites.net/Account/Login?ReturnUrl=/connect/authorize/
            //callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html
            //&scope=ProductionApi&state=VGh1IEp1bCAyNSAyMDE5IDEyOjI4OjA3IEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp
            string host = "https://zehusidentityserverwebintegration.azurewebsites.net/connect/authorize?response_type=token&client_id=SwaggerProduction&redirect_uri=https://zehusdrivermanufacturer-integr.azurewebsites.net/oauth2-redirect.html&scope=ProductionApi&state=VGh1IEp1bCAyNSAyMDE5IDEyOjI4OjA3IEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";
            //host += "&Username=SelcomGroupUser@grr.la&Password=%7LD2GPfNjv8";
            //GET server.example.com/authorize?response_type=token&client_id=s6BhdRkqt3&state=xyz&redirect_uri=https%3A%2F%2Fclient%2Eexample%2Ecom%2Fcb HTTP/1.1
            //获取到token
            //登录页面前的请求URL   GET
            host = "https://zehusidentityserverwebintegration.azurewebsites.net/connect/authorize";
            host = "https://zehusidentityserverwebintegration.azurewebsites.net/Account/Login?ReturnUrl=/connect/authorize/callback";
            string method = "POST";

            string query = "response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEwOjI3OjA1IEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";

            String url = host;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

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
            httpRequest.ContentType = "application/json";
            httpRequest.Accept = "application/json";


            //httpRequest.Headers.Add("Authorization", appcode);
            //httpRequest.Headers.Add("{ \"Username\": SelcomGroupUser@grr.la, \"Password\": \"%7LD2GPfNjv8\"}");
            //httpRequest.Headers.Add("Username", "SelcomGroupUser@grr.la");
            //httpRequest.Headers.Add("Password", "%7LD2GPfNjv8");
            byte[] data = Encoding.UTF8.GetBytes(query);
            using (Stream stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }
            textBox1.Text += "responseURI:" + httpResponse.ResponseUri + "\r\n";
            textBox1.Text += "statusCode:" + httpResponse.StatusCode + "\r\n";
            textBox1.Text += "method:" + httpResponse.Method + "\r\n";
            textBox1.Text += "header:" + httpResponse.Headers + "\r\n";
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            textBox1.Text += "content:" + reader.ReadToEnd();
        }
        public void SendRequest1()
        {
            string postUrl = "https://zehusidentityserverwebintegration.azurewebsites.net/connect/authorize";
            postUrl += "?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEyOjE3OjIwIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";
            //postUrl += "connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEyOjE3OjIwIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";
            //postUrl += "/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https://zehusdrivermanufacturer-integr.azurewebsites.net/oauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEyOjE3OjIwIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";
            ///connect/authorize?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDE0OjE2OjA4IEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp
            //设置请求参数
            //Account/Login?ReturnUrl=/connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEwOjI3OjA1IEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp

            //query = "postPram=grant_type=authorization_code&code=4934712103f0b229f2657a44efd9c2d91dc628893d6bbafb5&client_id=201068656&client_secret=48a86626da1be965a3e5d1ef05e95a67&redirect_uri=oob";
            string query = "response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEwOjI3OjA1IEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";
            //query = "\"response_type\":\"token\",\"client_id\":\"SwaggerProduction\",\"redirect_uri\":\"https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html\",\"scope\":\"ProductionApi\",\"state\":\"RnJpIEp1bCAyNiAyMDE5IDEwOjI3OjA1IEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp\"";

            //query = "ReturnUrl=/connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https://zehusdrivermanufacturer-integr.azurewebsites.net/oauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEyOjE3OjIwIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp&Username=SelcomGroupUser@grr.la&Password=%7LD2GPfNjv8&button=login&__RequestVerificationToken=CfDJ8O9ONuNhZLFIrUdyA65YKLCKSWxHeyEDNqwyvglWPb12PrpRKdXKAH6dh66DfDedDEGrjvJ6xVjyEIazE6WU_YtWTFpWD9KeCMC5ChT9zABiS2y-OWTmxEKPXbzarNZrQwNLSKkLDJges5sgkQWtIk8";
            query = "ReturnUrl=/connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEyOjE3OjIwIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp&Username=SelcomGroupUser@grr.la&Password=%7LD2GPfNjv8&button=login&__RequestVerificationToken=CfDJ8O9ONuNhZLFIrUdyA65YKLCKSWxHeyEDNqwyvglWPb12PrpRKdXKAH6dh66DfDedDEGrjvJ6xVjyEIazE6WU_YtWTFpWD9KeCMC5ChT9zABiS2y-OWTmxEKPXbzarNZrQwNLSKkLDJges5sgkQWtIk8";
            SetCertificatePolicy();
            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(postUrl);
            wReq.Method = "GET";
            //wReq.ContentType = "application/json"; 
            wReq.ContentType = "application/x-www-form-urlencoded";
            //wReq.ContentType = "text/plain";
            //wReq.ContentType = "application/json";
            //wReq.Accept = "text/plain";
            //wReq.ContentType = "multipart/form-data";
            //wReq.Headers = "Authorization:bearer Token";

            //string postData = "{\"UserName\":\"gsw\",\"Password\":\"111111\"}";
            //postData = "{\"Authorization\":\"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZ3N3IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDE5LzgvMjggMTU6NDk6NDgiLCJuYmYiOjE1NjM5Nzg1ODgsImV4cCI6MTU2Njk3ODU4OCwiaXNzIjoiZ3N3IiwiYXVkIjoiZXZlcnlvbmUifQ.HQdW7N_XTWXfaT-Zj7LzPqRJEMypmS49-i61ymjkz0Q\"}";

            //byte[] data = Encoding.UTF8.GetBytes(query);
            //wReq.ContentLength = data.Length;
            //Stream reqStream = wReq.GetRequestStream();
            //reqStream.Write(data, 0, data.Length);
            //reqStream.Close();

            using (StreamReader sr = new StreamReader(wReq.GetResponse().GetResponseStream(), Encoding.Default))
            {
                string result = sr.ReadToEnd();
                textBox1.Text += wReq.RequestUri + "\r\n";
                textBox1.Text += result + "\r\n";
            }
        }

        public void SendRequest2()
        {
            string postUrl = "https://zehusidentityserverwebintegration.azurewebsites.net/connect/authorize";
            //postUrl += "?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEyOjE3OjIwIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";
            postUrl += "?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDE0OjMwOjQxIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";
            //postUrl += "connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEyOjE3OjIwIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";
            //postUrl += "/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https://zehusdrivermanufacturer-integr.azurewebsites.net/oauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEyOjE3OjIwIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";
            ///connect/authorize?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDE0OjE2OjA4IEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp
            //设置请求参数
            //Account/Login?ReturnUrl=/connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEwOjI3OjA1IEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp

            //query = "postPram=grant_type=authorization_code&code=4934712103f0b229f2657a44efd9c2d91dc628893d6bbafb5&client_id=201068656&client_secret=48a86626da1be965a3e5d1ef05e95a67&redirect_uri=oob";
            string query = "response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEwOjI3OjA1IEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";
            //query = "\"response_type\":\"token\",\"client_id\":\"SwaggerProduction\",\"redirect_uri\":\"https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html\",\"scope\":\"ProductionApi\",\"state\":\"RnJpIEp1bCAyNiAyMDE5IDEwOjI3OjA1IEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp\"";

            //query = "ReturnUrl=/connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https://zehusdrivermanufacturer-integr.azurewebsites.net/oauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEyOjE3OjIwIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp&Username=SelcomGroupUser@grr.la&Password=%7LD2GPfNjv8&button=login&__RequestVerificationToken=CfDJ8O9ONuNhZLFIrUdyA65YKLCKSWxHeyEDNqwyvglWPb12PrpRKdXKAH6dh66DfDedDEGrjvJ6xVjyEIazE6WU_YtWTFpWD9KeCMC5ChT9zABiS2y-OWTmxEKPXbzarNZrQwNLSKkLDJges5sgkQWtIk8";
            query = "ReturnUrl=/connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=RnJpIEp1bCAyNiAyMDE5IDEyOjE3OjIwIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp&Username=SelcomGroupUser@grr.la&Password=%7LD2GPfNjv8&button=login&__RequestVerificationToken=CfDJ8O9ONuNhZLFIrUdyA65YKLCKSWxHeyEDNqwyvglWPb12PrpRKdXKAH6dh66DfDedDEGrjvJ6xVjyEIazE6WU_YtWTFpWD9KeCMC5ChT9zABiS2y-OWTmxEKPXbzarNZrQwNLSKkLDJges5sgkQWtIk8";
            SetCertificatePolicy();
            HttpWebRequest wReq = (HttpWebRequest)WebRequest.Create(postUrl);
            wReq.Method = "GET";
            //wReq.ContentType = "application/json"; 
            wReq.ContentType = "application/x-www-form-urlencoded";
            //wReq.ContentType = "text/plain";
            //wReq.ContentType = "application/json";
            //wReq.Accept = "text/plain";
            //wReq.ContentType = "multipart/form-data";
            //wReq.Headers = "Authorization:bearer Token";

            //string postData = "{\"UserName\":\"gsw\",\"Password\":\"111111\"}";
            //postData = "{\"Authorization\":\"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZ3N3IiwiaHR0cDovL3NjaGVtYXMubWljcm9zb2Z0LmNvbS93cy8yMDA4LzA2L2lkZW50aXR5L2NsYWltcy9yb2xlIjoiYWRtaW4iLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL2V4cGlyYXRpb24iOiIyMDE5LzgvMjggMTU6NDk6NDgiLCJuYmYiOjE1NjM5Nzg1ODgsImV4cCI6MTU2Njk3ODU4OCwiaXNzIjoiZ3N3IiwiYXVkIjoiZXZlcnlvbmUifQ.HQdW7N_XTWXfaT-Zj7LzPqRJEMypmS49-i61ymjkz0Q\"}";

            //byte[] data = Encoding.UTF8.GetBytes(query);
            //wReq.ContentLength = data.Length;
            //Stream reqStream = wReq.GetRequestStream();
            //reqStream.Write(data, 0, data.Length);
            //reqStream.Close();

            using (StreamReader sr = new StreamReader(wReq.GetResponse().GetResponseStream(), Encoding.Default))
            {
                string result = sr.ReadToEnd();
                textBox1.Text += wReq.RequestUri + "\r\n";
                textBox1.Text += result + "\r\n";
            }
        }

        public void SendRequest3()
        {
            requestM();
        }
        /// <summary>
        /// Sets the cert policy.
        /// </summary>
        public static void SetCertificatePolicy()
        {
            ServicePointManager.ServerCertificateValidationCallback
                       += RemoteCertificateValidate;
        }

        /// <summary>
        /// Remotes the certificate validate.
        /// </summary>
        private static bool RemoteCertificateValidate(
           object sender, X509Certificate cert,
            X509Chain chain, SslPolicyErrors error)
        {
            // trust any certificate!!!
            System.Console.WriteLine("Warning, trust any certificate");
            return true;
        }

        public static CookieContainer container = null; //存储验证码cookie

        #region 登录
        public string requestM()
        {
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create("https://zehusidentityserverwebintegration.azurewebsites.net/Account/Login?ReturnUrl=/connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=" +
                    "https://zehusdrivermanufacturer-integr.azurewebsites.net/oauth2-redirect.html&scope=ProductionApi&state=VGh1IEF1ZyAwMSAyMDE5IDE4OjAzOjEzIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp");
                request.Method = "Post";
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";//application/x-www-form-urlencoded
                request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/63.0.3239.132 Safari/537.36";
                request.AllowAutoRedirect = true;
                request.CookieContainer = container;//获取验证码时候获取到的cookie会附加在这个容器里面
                request.KeepAlive = true;//建立持久性连接
                                         //整数据
                string postData = "";
                postData += "ReturnUrl =/connect/authorize/callback?response_type=token&client_id=SwaggerProduction&";
                postData += "redirect_uri = https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html";
                postData += "&scope=ProductionApi&state=VGh1IEF1ZyAwMSAyMDE5IDE4OjAzOjEzIEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp";
                postData += "&Username=SelcomGroupUser@grr.la&Password=%7LD2GPfNjv8&button=login";
                postData += "&__RequestVerificationToken=CfDJ8O9ONuNhZLFIrUdyA65YKLAieQ3HgLFuzGalYHcZjnfW5Oesc8pkxvA7VnX2HGEAAmpXsxywPpQpW8Um41lH5WEuDpWIIcOccgXDfSZYHjPW-i257YJWtGpmyn-0KTLgxEi62A25sZfTL_SBoMv-DTnQrNq8H6VKh5RiW7x5AVpMEjrtZmja6SC7MgkA2qQerQ";
                //postData += "string postData = string.Format("userName={0}&passwd={1}&validateCode={2}&rememberMe=true", uName, passwd, vaildate);

                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] bytepostData = encoding.GetBytes(postData);
                request.ContentLength = bytepostData.Length;

                //发送数据  using结束代码段释放
                using (Stream requestStm = request.GetRequestStream())
                {
                    requestStm.Write(bytepostData, 0, bytepostData.Length);
                }

                //响应
                response = (HttpWebResponse)request.GetResponse();
                string text = string.Empty;
                using (Stream responseStm = response.GetResponseStream())
                {
                    StreamReader redStm = new StreamReader(responseStm, Encoding.UTF8);
                    textBox1.Text = redStm.ReadToEnd();
                }
                return text;
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                textBox1.Text = msg;
                return msg;
            }

        }
        #endregion 

        #region 获取验证码
        public Stream getCodeStream(string codeUrl)
        {

            //验证码请求
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(codeUrl);
            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:5.0.1) Gecko/20100101 Firefox/5.0.1";
            request.Accept = "image/webp,*/*;q=0.8";
            request.CookieContainer = new CookieContainer();//!Very Important.!!!
            container = request.CookieContainer;
            var c = request.CookieContainer.GetCookies(request.RequestUri);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            response.Cookies = container.GetCookies(request.RequestUri);

            Stream stream = response.GetResponseStream();
            return stream;
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {

            //var rmsg = requestM(txtuserName.Text, txtPassword.Text, txtVaildata.Text);
            //MessageBox.Show(rmsg);
        }

        private void ELONG_LOGIN_FORM_Load(object sender, EventArgs e)
        {
            ReflshPicImage();//更新验证码
        }

        //更新验证码
        public void ReflshPicImage()
        {
            string codeUrl = "https://secure.elong.com/passport/getValidateCode";
            //ELOGN_LOGIN agent = new ELOGN_LOGIN();
            //Stream stmImage = agent.getCodeStream(codeUrl);
            //picValidate.Image = Image.FromStream(stmImage);
        }

        private void btnReValidate_Click(object sender, EventArgs e)
        {
            ReflshPicImage();//更新验证码
        }

        private void picValidate_Click(object sender, EventArgs e)
        {
            ReflshPicImage();//更新验证码
        }
    }
}
