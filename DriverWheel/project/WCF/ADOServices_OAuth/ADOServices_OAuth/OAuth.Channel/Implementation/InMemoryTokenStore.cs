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
﻿using System;
using System.Web;
using DevDefined.OAuth.Framework;
using DevDefined.OAuth.Storage;
using OAuthChannel.Repositories;

namespace OAuthChannel.Implementation
{
    public class SimpleTokenStore : ITokenStore
    {
        private readonly ITokenRepository<Models.AccessToken> _accessTokenRepository;
		private readonly ITokenRepository<Models.RequestToken> _requestTokenRepository;

		public SimpleTokenStore(ITokenRepository<Models.AccessToken> accessTokenRepository, ITokenRepository<Models.RequestToken> requestTokenRepository)
        {
			_accessTokenRepository = accessTokenRepository;
			_requestTokenRepository = requestTokenRepository; 
        }

        #region ITokenStore Members

        public IToken CreateRequestToken(IOAuthContext context)
        {
            var token = new Models.RequestToken
                            {
                                ConsumerKey = context.ConsumerKey,
                                Realm = context.Realm,
                                Token = Guid.NewGuid().ToString(),
                                TokenSecret = Guid.NewGuid().ToString()
                            };

			_requestTokenRepository.SaveToken(token);
            return token;
        }

        public void ConsumeRequestToken(IOAuthContext requestContext)
        {
			Models.RequestToken requestToken = _requestTokenRepository.GetToken(requestContext.Token);

            if (requestToken.UsedUp)
            {
                throw new OAuthException(requestContext, OAuthProblems.TokenRejected,
                                         "The request token has already be consumed.");
            }

            requestToken.UsedUp = true;

			_requestTokenRepository.SaveToken(requestToken);
        }

        public void ConsumeAccessToken(IOAuthContext accessContext)
        {
			Models.AccessToken accessToken = _accessTokenRepository.GetToken(accessContext.Token);

            if (accessToken.ExpireyDate < DateTime.Now)
            {
                throw new OAuthException(accessContext, OAuthProblems.TokenExpired,
                                         "Token has expired (they're only valid for 1 minute)");
            }
        }

        public IToken GetAccessTokenAssociatedWithRequestToken(IOAuthContext requestContext)
        {
            Models.RequestToken request = _requestTokenRepository.GetToken(requestContext.Token);
            return request.AccessToken;
        }

        public RequestForAccessStatus GetStatusOfRequestForAccess(IOAuthContext accessContext)
        {
            Models.RequestToken request = _requestTokenRepository.GetToken(accessContext.Token);

            if (request.AccessDenied) return RequestForAccessStatus.Denied;
            if (request.AccessToken == null) return RequestForAccessStatus.Unknown;

            return RequestForAccessStatus.Granted;
        }

        #endregion


        public IToken CreateAccessToken(IOAuthContext context)
        {
            OAuthChannel.Models.RequestToken requestToken = _requestTokenRepository.GetToken(context.Token);
            var accessToken = new OAuthChannel.Models.AccessToken
            {
                ConsumerKey = requestToken.ConsumerKey,
                Realm = requestToken.Realm,
                Token = Guid.NewGuid().ToString(),
                TokenSecret = Guid.NewGuid().ToString(),
                UserName = HttpContext.Current.User.Identity.Name,
                ExpireyDate = DateTime.Now.AddMinutes(1),
                Roles = new string[] { }
            };

            _accessTokenRepository.SaveToken(accessToken);
            requestToken.AccessToken = accessToken;
            _requestTokenRepository.SaveToken(requestToken);
            return accessToken;
        }

        public string GetCallbackUrlForToken(IOAuthContext requestContext)
        {
            throw new NotImplementedException();
        }

        public string GetVerificationCodeForRequestToken(IOAuthContext requestContext)
        {
            throw new NotImplementedException();
        }

        public string GetRequestTokenSecret(IOAuthContext context)
        {
            return _requestTokenRepository.GetToken(context.Token).TokenSecret;
        }

        public string GetAccessTokenSecret(IOAuthContext context)
        {
            return _accessTokenRepository.GetToken(context.Token).TokenSecret;
        }

        public IToken RenewAccessToken(IOAuthContext requestContext)
        {
            throw new NotImplementedException();
        }
    }
}