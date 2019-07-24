using IdentityServer3.AccessTokenValidation;
using Owin;
using Microsoft.Owin;
using System.Web.Http;
using Swashbuckle.Application;

[assembly: OwinStartup(typeof(SwaggerDemoApi.Startup))]

namespace SwaggerDemoApi
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // token validation
            appBuilder.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = "https://localhost:51609/core"
            });
        }
    }
}
