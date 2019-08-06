using System;
using System.Web.UI;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Provider;

namespace ADOServices.OAuth
{
    public partial class RequestToken : System.Web.IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(System.Web.HttpContext context)
        {
            IOAuthContext oauthContext = new OAuthContextBuilder().FromHttpRequest(context.Request);
            IOAuthProvider provider = OAuthServicesLocator.Provider;
            IToken token = provider.GrantRequestToken(oauthContext);
            context.Response.Write(token);
            context.Response.End();
        }
    }
}