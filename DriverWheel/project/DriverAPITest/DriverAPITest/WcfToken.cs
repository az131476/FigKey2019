using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.IO;
using Newtonsoft.Json;

namespace DriverAPITest
{
    class WcfToken
    {
        //        1.打开对方官网https:test.cn

        //2.用账户登陆https:test.cn

        //3.登陆成功后跳转到我们的网站https:my.cn,并且返回一个code给我们

        //4.进入我们系统后根据返回的code在后台用代码post对方的一个action 地址:https://https:test.cn/CNS-AS/OAuth/Token/1.1 获取一个Access_token

        //5.将获取到的Access_token添加到http 头文件中再去调用对方的wcf接口https:test.cn/CNS-Service/CNSServices.svc/CNSServices

        //6.最后拿到验证通过的账户信息，最后停留https:my.cn 我们站点进行相关操作

        //废话少说，直接上代码

        //1.https:test.cn 登陆成功后跳转的地址：

        //https://test.cn/CNS-AS/OAuth/Authorize?client_id=indegene_client&redirect_uri=https:my.cn/home/index&state=12321&scope=http://tempuri.org/ICNSServices/GetCNSUserName&response_type=code

        //2.进入https:my.cn后代码

        public static StringBuilder GetAccessToken()
        {
            StringBuilder stringBuilder = new StringBuilder();
            try
            {
                //1-3step(Authorization Response)
                //string code = Request["code"];
                //string state = Request["state"];

                //4. Access Token Request
                //"https://test.cn/CNS-AS/OAuth/Token/1.1"
                string url = "https://zehusidentityserverwebintegration.azurewebsites.net/connect/authorize";

                HttpWebRequest objWebRequest = (HttpWebRequest)WebRequest.Create(url); //请求地址

                //设置用户名密码的Base64编码
                string code1 = Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", "test", "test")));

                string postData = "response_type=token&client_id=SwaggerProduction&scope=ProductionApi";
                //string postData = string.Format("code={0}&redirect_uri={1}&client_id={2}&client_secret={3}", 
                //code, "http://my/home/index", "indegene_client", "indegene_secretIJH"); // 要发放的数据

                byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(postData);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                objWebRequest.Method = "POST";//提交方式

                objWebRequest.ContentType = "application/x-www-form-urlencoded";

                //byte[] data = Encoding.UTF8.GetBytes(query);
                //wReq.ContentLength = data.Length;
                //Stream reqStream = wReq.GetRequestStream();
                //reqStream.Write(data, 0, data.Length);
                //reqStream.Close();

                objWebRequest.ContentLength = byteArray.Length;
                Stream newStream = objWebRequest.GetRequestStream(); // Send the data.
                newStream.Write(byteArray, 0, byteArray.Length); //写入参数
                newStream.Close();

                //响应请求

                //5. Access Token Response

                HttpWebResponse response = (HttpWebResponse)objWebRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default))
                {
                    string result = sr.ReadToEnd();
                    //打印返回值
                    stringBuilder.AppendLine("responseURI:"+response.ResponseUri);
                    stringBuilder.AppendLine("response:" + result);
                    //AccessToken access_token = JsonConvert.DeserializeObject<AccessToken>(result);
                    //stringBuilder.AppendLine("access_token:" + access_token.Access_token);
                }
                return stringBuilder;
            }
            catch (Exception ex)
            {
                stringBuilder.AppendLine("Error:"+ex.Message+"\r\n"+ex.StackTrace);
                return stringBuilder;
            }
        }

        public class AccessToken
        {

            private string access_token;

            public string Access_token
            {
                get { return access_token; }
                set { access_token = value; }
            }
            private string token_type;

            public string Token_type
            {
                get { return token_type; }
                set { token_type = value; }
            }
            private string expires_in;

            public string Expires_in
            {
                get { return expires_in; }
                set { expires_in = value; }
            }
            private string refresh_token;

            public string Refresh_token
            {
                get { return refresh_token; }
                set { refresh_token = value; }
            }
            private string scope;

            public string Scope
            {
                get { return scope; }
                set { scope = value; }
            }
        }
    }
}
