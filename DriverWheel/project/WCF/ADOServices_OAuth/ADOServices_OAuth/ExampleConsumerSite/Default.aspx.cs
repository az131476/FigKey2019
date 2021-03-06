using System;
using System.Web;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using System.Web.UI;
using DevDefined.OAuth.Tests;
using OAuthConsumerSample.Properties;

namespace OAuthConsumerSample
{
    public partial class _Default : Page
    {
        protected void oauthRequest_Click(object sender, EventArgs e)
        {
			OAuthSession session = OAuthSessionFactory.CreateSession();
            IToken requestToken = session.GetRequestToken();
            if (string.IsNullOrEmpty(requestToken.Token))
            {
                throw new Exception("The request token was null or empty");
            }
            Session[requestToken.Token] = requestToken;
            string callBackUrl = "http://localhost:" + HttpContext.Current.Request.Url.Port + "/Callback.ashx";
            string authorizationUrl = session.GetUserAuthorizationUrlForToken(requestToken, callBackUrl);
            Response.Redirect(authorizationUrl, true);
        }
    }
}   