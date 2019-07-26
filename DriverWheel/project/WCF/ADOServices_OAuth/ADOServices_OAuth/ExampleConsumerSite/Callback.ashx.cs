using System;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;

namespace OAuthConsumerSample
{
    public partial class Callback : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(System.Web.HttpContext context)
        {
            var session = OAuthSessionFactory.CreateSession();
            string requestTokenString = context.Request["oauth_token"];
            var requestToken = (IToken)context.Session[requestTokenString];
            IToken accessToken = session.ExchangeRequestTokenForAccessToken(requestToken);
            context.Session[requestTokenString] = null;
            context.Session[accessToken.Token] = accessToken;
            context.Response.Redirect("ViewData.ashx?oauth_token=" + accessToken.Token);
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}