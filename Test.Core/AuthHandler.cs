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

    public class AuthHandler : CookieAuthenticationHandler
    {
        public IOptionsMonitor<CookieAuthenticationOptions> _options;
        public AuthHandler(IOptionsMonitor<CookieAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
           : base(options, logger, encoder, clock)
        {
            _options = options;
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (!Request.Path.Value.ToLower().StartsWith("/api/") && !Request.Path.Value.ToLower().StartsWith("/signalr"))
            {
                await base.HandleChallengeAsync(properties);
                //var redirectContext = new RedirectContext<CookieAuthenticationOptions>(Context, Scheme, Options, properties, BuildRedirectUri(_options.CurrentValue.LoginPath));
                //await Events.RedirectToLogin(redirectContext);
            }
        }
    }
}
