using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Test.Core
{

    public class AuthRemoteHandler : OpenIdConnectHandler
    {
        public readonly IOptionsMonitor<OpenIdConnectOptions> _options;

        public AuthRemoteHandler(IOptionsMonitor<OpenIdConnectOptions> options, ILoggerFactory logger, HtmlEncoder htmlEncoder, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, htmlEncoder, encoder, clock)
        {
            _options = options;
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (!Request.Path.Value.ToLower().StartsWith("/api/") && !Request.Path.Value.ToLower().StartsWith("/signalr"))
            {
                await base.HandleChallengeAsync(properties);
            }
        }
    }
}
