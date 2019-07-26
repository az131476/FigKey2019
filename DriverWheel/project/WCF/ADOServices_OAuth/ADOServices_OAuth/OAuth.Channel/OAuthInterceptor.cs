// The MIT License
//
// Copyright (c) 2006-2008 DevDefined Limited.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceModel.Web;
using System.ServiceModel.Channels;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Provider;
using System.Security.Principal;
using System.Xml.Linq;
using System.IO;
using System.Net;
using System.IdentityModel.Policy;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.IdentityModel.Claims;
using OAuthChannel.Repositories;
using System.Web;

namespace OAuthChannel
{
	public class OAuthInterceptor : RequestInterceptor
	{
		IOAuthProvider _provider;
		ITokenRepository<OAuthChannel.Models.AccessToken> _repository;
      
		public OAuthInterceptor(IOAuthProvider provider, ITokenRepository<OAuthChannel.Models.AccessToken> repository) 
			: base(false)
		{
			_provider = provider;
			_repository = repository;
		}

		public override void ProcessRequest(ref RequestContext requestContext)
		{
			if (requestContext == null || requestContext.RequestMessage == null)
			{
				return;
			}
			
			Message request = requestContext.RequestMessage;
			HttpRequestMessageProperty requestProperty = (HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name];
			
			IOAuthContext context = new OAuthContextBuilder().FromUri(requestProperty.Method, request.Headers.To);

			try
			{
				_provider.AccessProtectedResourceRequest(context);

				OAuthChannel.Models.AccessToken accessToken = _repository.GetToken(context.Token);

				TokenPrincipal principal = new TokenPrincipal(
					new GenericIdentity(accessToken.UserName, "OAuth"),
					accessToken.Roles,
					accessToken);

				InitializeSecurityContext(request, principal);
			}
			catch (OAuthException authEx)
			{
				XElement response = XElement.Load(new StringReader("<?xml version=\"1.0\" encoding=\"utf-8\"?><html xmlns=\"http://www.w3.org/1999/xhtml\" version=\"-//W3C//DTD XHTML 2.0//EN\" xml:lang=\"en\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.w3.org/1999/xhtml http://www.w3.org/MarkUp/SCHEMA/xhtml2.xsd\"><HEAD><TITLE>Request Error</TITLE></HEAD><BODY><DIV id=\"content\"><P class=\"heading1\"><B>" + HttpUtility.HtmlEncode(authEx.Report.ToString()) + "</B></P></DIV></BODY></html>"));
				Message reply = Message.CreateMessage(MessageVersion.None, null, response);
				HttpResponseMessageProperty responseProperty = new HttpResponseMessageProperty() { StatusCode = HttpStatusCode.Forbidden, StatusDescription = authEx.Report.ToString() };
				responseProperty.Headers[HttpResponseHeader.ContentType] = "text/html";
				reply.Properties[HttpResponseMessageProperty.Name] = responseProperty;
				requestContext.Reply(reply);

				requestContext = null;
			}
		}

		private void InitializeSecurityContext(Message request, IPrincipal principal)
		{
			List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>();
			policies.Add(new PrincipalAuthorizationPolicy(principal));
			ServiceSecurityContext securityContext = new ServiceSecurityContext(policies.AsReadOnly());
			
			if (request.Properties.Security != null)
			{
				request.Properties.Security.ServiceSecurityContext = securityContext;
			}
			else
			{
				request.Properties.Security = new SecurityMessageProperty() { ServiceSecurityContext = securityContext };
			}
		}

		class PrincipalAuthorizationPolicy : IAuthorizationPolicy
		{
			string id = Guid.NewGuid().ToString();
			IPrincipal user;

			public PrincipalAuthorizationPolicy(IPrincipal user)
			{
				this.user = user;
			}

			public ClaimSet Issuer
			{
				get { return ClaimSet.System; }
			}

			public string Id
			{
				get { return this.id; }
			}

			public bool Evaluate(EvaluationContext evaluationContext, ref object state)
			{
				evaluationContext.AddClaimSet(this, new DefaultClaimSet(Claim.CreateNameClaim(user.Identity.Name)));
				evaluationContext.Properties["Identities"] = new List<IIdentity>(new IIdentity[] { user.Identity });
				evaluationContext.Properties["Principal"] = user;
				return true;
			}
		}
	}
}
