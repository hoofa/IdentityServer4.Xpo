using IdentityServer3.AccessTokenValidation;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Cookies;

[assembly: OwinStartup(typeof(Test.Framework.App_Start.Startup))]
namespace Test.Framework.App_Start
{

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions { });

            app.Use(typeof(APIOpenIdConnectAuthenticationMiddleware), app, new OpenIdConnectAuthenticationOptions
            {
                AuthenticationType = OpenIdConnectAuthenticationDefaults.AuthenticationType,
                SignInAsAuthenticationType = CookieAuthenticationDefaults.AuthenticationType,
                Authority = "http://localhost:5343/",
                RedirectUri = "http://localhost:65061",
                PostLogoutRedirectUri = "http://localhost:65061",
                ClientId = "ro.client",
                ClientSecret = "secret",
                ResponseType = OpenIdConnectResponseType.IdToken,//"id_token",
                //Scope = "openid profile api1 offline_access",
                UseTokenLifetime = false,
                //RequireHttpsMetadata = false,
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    SecurityTokenValidated = (context) =>
                    {
                        var identity = context.AuthenticationTicket.Identity;
                        identity.AddClaim(new System.Security.Claims.Claim("id_token", context.ProtocolMessage.IdToken));
                        return Task.FromResult(0);
                    },
                    RedirectToIdentityProvider = n =>
                    {
                        // if signing out, add the id_token_hint
                        if (n.ProtocolMessage.RequestType == Microsoft.IdentityModel.Protocols.OpenIdConnectRequestType.LogoutRequest)
                        {
                            var idTokenHint = n.OwinContext.Authentication.User.FindFirst("id_token");
                            if (idTokenHint != null)
                            {
                                n.ProtocolMessage.IdTokenHint = idTokenHint.Value;
                            }
                        }
                        return Task.FromResult(0);
                    },
                    AuthenticationFailed= (notification => {
                        notification.HandleResponse();
                        notification .Response.Redirect("/");
                        return Task.FromResult(0);
                    })

                }
            });

            app.UseIdentityServerBearerTokenAuthentication
            (new IdentityServerBearerTokenAuthenticationOptions
            {
                AuthenticationMode = AuthenticationMode.Active,
                Authority = "http://localhost:5343/",
                RequiredScopes = new[] { "api1" },
            });


        }
    }
}