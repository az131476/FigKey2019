using System;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using Microsoft.ServiceModel.Web;
using DevDefined.OAuth.Provider;
using OAuthChannel.Repositories;

namespace OAuthChannel
{
    public class OAuthWebServiceHostFactory : WebServiceHostFactory
    {
        public IOAuthProvider OAuthProvider { get; set; }
        public ITokenRepository<OAuthChannel.Models.AccessToken> AccessTokenRepository { get; set; }

        protected override System.ServiceModel.ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            var serviceHost = new WebServiceHost2(serviceType, true, baseAddresses);
            var interceptor = new OAuthChannel.OAuthInterceptor(OAuthProvider, AccessTokenRepository);
            serviceHost.Interceptors.Add(interceptor);
            return serviceHost;
        }
    }
}