using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;

namespace OAuthWcfRestService
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            var oauthWebServiceHostFactory = new OAuthChannel.OAuthWebServiceHostFactory 
            {
                AccessTokenRepository = OAuthManager.AccessTokenRepository,
                OAuthProvider = OAuthManager.Provider 
            };
            RouteTable.Routes.Add(new ServiceRoute("OAuthService", oauthWebServiceHostFactory, typeof(OAuthService)));
        }
    }
}
