using Microsoft.IdentityModel.Protocols;
using Microsoft.Owin.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Test.Framework
{
    public class APIOpenIdConnectAuthenticationHandler : OpenIdConnectAuthenticationHandler
    {
        public APIOpenIdConnectAuthenticationHandler(ILogger logger) : base(logger)
        {
        }

        protected override async Task ApplyResponseChallengeAsync()
        {
            if (Response.StatusCode == 401 && Request.Path.ToString().ToLower().StartsWith("/api"))
            {
                return;
            }
            else
            {
                await base.ApplyResponseChallengeAsync();
            }
        }

    }
}