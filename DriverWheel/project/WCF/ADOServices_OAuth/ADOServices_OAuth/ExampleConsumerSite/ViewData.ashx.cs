﻿using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;
using DevDefined.OAuth.Consumer;
using DevDefined.OAuth.Framework;
using OAuthConsumerSample.Properties;
using System.Xml;
using System.Text;

namespace OAuthConsumerSample
{
    public partial class ViewData : System.Web.IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            OAuthSession session = OAuthSessionFactory.CreateSession();
            string accessTokenString = context.Request[Parameters.OAuth_Token];
            session.AccessToken = (IToken)context.Session[accessTokenString];

            try
            {
                var results = session.Request()
                    .Get()
                    .ForUrl("http://localhost:1897/OAuthService/Contacts")
                    .SignWithToken()
                    .ToWebResponse();

                context.Response.Write(ReadToString(results));
                context.Response.ContentType = results.ContentType;
            }
            catch (WebException webEx)
            {
                var response = (HttpWebResponse)webEx.Response;

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    context.Response.Write(ReadToString(response));
                    context.Response.ContentType = response.ContentType;
                }
            }
        }

        private static string ReadToString(WebResponse response)
        {
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }

    }
}