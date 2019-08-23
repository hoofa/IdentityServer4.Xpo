using Microsoft.Owin;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security.Infrastructure;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Test.Framework
{
    public class APIOpenIdConnectAuthenticationMiddleware: OpenIdConnectAuthenticationMiddleware
    {
        private readonly ILogger _logger;
        private readonly OpenIdConnectAuthenticationOptions _options;
        public APIOpenIdConnectAuthenticationMiddleware(OwinMiddleware next, IAppBuilder app, OpenIdConnectAuthenticationOptions options) : base(next,app,options)
        {
            _logger = app.CreateLogger<APIOpenIdConnectAuthenticationMiddleware>();
            _options = options;
        }

        protected override AuthenticationHandler<OpenIdConnectAuthenticationOptions> CreateHandler()
        {
            return new APIOpenIdConnectAuthenticationHandler(_logger);
        }
    }
}
