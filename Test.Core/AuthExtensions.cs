using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace Test.Core
{

    public static class AuthExtensions
    {

        private static AuthenticationBuilder AddAuth(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<CookieAuthenticationOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<CookieAuthenticationOptions>, PostConfigureCookieAuthenticationOptions>());
            return builder.AddScheme<CookieAuthenticationOptions, AuthHandler>(authenticationScheme, displayName, configureOptions);
        }

        private static AuthenticationBuilder AddRemoteAuth(this AuthenticationBuilder builder, string authenticationScheme, string displayName, Action<OpenIdConnectOptions> configureOptions)
        {
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<OpenIdConnectOptions>, OpenIdConnectPostConfigureOptions>());
            return builder.AddRemoteScheme<OpenIdConnectOptions, AuthRemoteHandler>(authenticationScheme, displayName, configureOptions);
        }
        public static void AddBimAuthentication(this IServiceCollection services, IConfiguration config, Action<OpenIdConnectOptions> oidcOptions=null)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddAuth(CookieAuthenticationDefaults.AuthenticationScheme, null, options =>
             {
                 //options.Cookie.Name = "AuthCookie";
                 ////options.Cookie.Domain = "bim999.net";
                 //options.Cookie.Path = "/";
                 options.Cookie.HttpOnly = true;
                 options.Cookie.SameSite = SameSiteMode.Lax;
                 //仅https可登录
                 //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                 //options.AccessDeniedPath = "/Home/Login";
                 //options.LoginPath = "/Home/Login";
                 //options.LogoutPath = "/Home/Logout";

                 //options.TicketDataFormat = new JwtCookieDataFormat(jwtSettings);
                 //options.ClaimsIssuer = jwtSettings.Issuer;
             })
              .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
              {
                  o.Authority = config.GetSection("IdentityServer:Authority").Value;
                  o.RequireHttpsMetadata = false;

                  o.Audience = config.GetSection("IdentityServer:Audience").Value;
                  //o.TokenValidationParameters = new TokenValidationParameters
                  //{
                  //    //NameClaimType = JwtClaimTypes.Name,
                  //    //RoleClaimType = JwtClaimTypes.Role,

                  //    ValidIssuer = JwtSettings.Issuer,//颁发机构
                  //    ValidAudience = JwtSettings.Audience,//颁发给谁
                  //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JwtSettings.SecretKey)), //签名秘钥,【注意】 SecretKey必须大于16个,是大于，不是大于等于

                  //    /***********************************TokenValidationParameters的参数默认值***********************************/
                  //    // RequireSignedTokens = true,
                  //    // SaveSigninToken = false,
                  //    // ValidateActor = false,
                  //    // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                  //    // ValidateAudience = true,
                  //    // ValidateIssuer = true, 
                  //    // ValidateIssuerSigningKey = false,
                  //    //是否要求Token的Claims中必须包含Expires
                  //    RequireExpirationTime = true,
                  //    // 允许的服务器时间偏移量
                  //    // ClockSkew = TimeSpan.FromSeconds(300),
                  //    // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                  //    ValidateLifetime = true
                  //};
              })
              .AddRemoteAuth(OpenIdConnectDefaults.AuthenticationScheme ,null, options =>
              {
                  if (oidcOptions != null)
                  {
                      oidcOptions.Invoke(options);
                  }
                  options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                  options.Authority = config.GetSection("IdentityServer:Authority").Value;
                  options.RequireHttpsMetadata = false;

                  options.SaveTokens = true;
                  options.GetClaimsFromUserInfoEndpoint = true;
                  options.ClientId = config.GetSection("IdentityServer:Client:ClientId").Value;
                 
              })
            //.AddOpenIdConnect(o =>
            //{
            //    o.ClientId = "server.hybrid";
            //    o.ClientSecret = "secret";
            //    o.Authority = "https://demo.identityserver.io/";
            //    o.ResponseType = OpenIdConnectResponseType.CodeIdToken;
            //})
            ;

            services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    CookieAuthenticationDefaults.AuthenticationScheme, 
                    JwtBearerDefaults.AuthenticationScheme, 
                    OpenIdConnectDefaults.AuthenticationScheme );
                defaultAuthorizationPolicyBuilder =
                    defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
            });
        }


    }
}
