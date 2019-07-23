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

            textBox1.Text += "statusCode:"+httpResponse.StatusCode+"\r\n";
            textBox1.Text += "method:"+httpResponse.Method+"\r\n";
            textBox1.Text += "header:"+httpResponse.Headers+"\r\n";
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            textBox1.Text += "content:"+reader.ReadToEnd();

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
                        //https://zehusidentityserverwebintegration.azurewebsites.net/Account/Login?ReturnUrl=/connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=VHVlIEp1bCAyMyAyMDE5IDE0OjAwOjA5IEdNVCswODAwICjkuK3lm73moIflh4bml7bpl7Qp
                        //https://zehusidentityserverwebintegration.azurewebsites.net/Account/Login?ReturnUrl=/connect/authorize/callback?response_type=token&client_id=SwaggerProduction&redirect_uri=https%3A%2F%2Fzehusdrivermanufacturer-integr.azurewebsites.net%2Foauth2-redirect.html&scope=ProductionApi&state=VHVlIEp1bCAyMyAyMDE5IDA5OjI5OjU4IEdNVCswODAwICjkuK3lm73
            string url = "https://zehusdrivermanufacturer-integr.azurewebsites.net/authorize?response_type=code&scope=openid%20profile%20email&client_id=s6BhdRkqt3&state=af0ifjsldkj&redirect_uri=https%3A%2F%2Fclient.example.org%2Fcb HTTP/ 1.1";
            string url_1 = "https://login.microsoftonline.com/contoso.onmicrosoft.com/oauth2/authorize?client_id=6731de76-14a6-49ae-97bc-6eba6914391e&response_type=id_token&redirect_uri=http%3A%2F%2Flocalhost%3a12345&response_mode=form_post&scope=openid&state=12345&nonce=7362CAEA-9CA5-4B43-9BA3-34D7C303EBA7";
            this.webBrowser1.Navigate(url_1);
            textBox1.Text += "browserUrl:"+ this.webBrowser1.Url+"\r\n";
            HttpHelps httpHelps = new HttpHelps();
            var resStr = httpHelps.GetHttpRequestStringByNUll_Get(url_1, Encoding.UTF8);

            textBox1.Text += "result:" + resStr+"\r\n";
            this.webBrowser1.CanGoForwardChanged += WebBrowser1_CanGoForwardChanged;
        }

        private void WebBrowser1_CanGoForwardChanged(object sender, EventArgs e)
        {
            this.textBox1.Clear();
            this.textBox1.Text += "webURL:"+this.webBrowser1.Url;

        }

        private void Api1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TestAPI();
        }
    }
    @Configuration
@EnableWebMvc
@EnableSwagger2
public class SwaggerConfig
    {

        @Bean
    public Docket platformApi()
        {

            return new Docket(DocumentationType.SWAGGER_2).apiInfo(apiInfo()).forCodeGeneration(true)
                    .select().apis(RequestHandlerSelectors.withMethodAnnotation(ApiOperation.class))
                .apis(RequestHandlerSelectors.any())
                .paths(regex("^.*(?<!error)$"))
                .build()
                .securitySchemes(securitySchemes())
                .securityContexts(securityContexts());


    }
    private List<ApiKey> securitySchemes()
    {
        List<ApiKey> apiKeyList = new ArrayList();
        apiKeyList.add(new ApiKey("x-auth-token", "x-auth-token", "header"));
        return apiKeyList;
    }

    private List<SecurityContext> securityContexts()
    {
        List<SecurityContext> securityContexts = new ArrayList<>();
        securityContexts.add(
                SecurityContext.builder()
                        .securityReferences(defaultAuth())
                        .forPaths(PathSelectors.regex("^(?!auth).*$"))
                        .build());
        return securityContexts;
    }

    List<SecurityReference> defaultAuth()
    {
        AuthorizationScope authorizationScope = new AuthorizationScope("global", "accessEverything");
        AuthorizationScope[] authorizationScopes = new AuthorizationScope[1];
        authorizationScopes[0] = authorizationScope;
        List<SecurityReference> securityReferences = new ArrayList<>();
        securityReferences.add(new SecurityReference("Authorization", authorizationScopes));
        return securityReferences;
    }
    private ApiInfo apiInfo()
    {
        return new ApiInfoBuilder().title("starmark-API").description("©2018 Copyright. Powered By starmark.")
                // .termsOfServiceUrl("")
                .contact(new Contact("Starmark", "", "947618@163.com")).license("Apache License Version 2.0")
                .licenseUrl("https://github.com/springfox/springfox/blob/master/LICENSE").version("2.0").build();
    }

}
}
