using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using IdentityModel;
using Microsoft.Extensions.Configuration;

namespace Test.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private static string _tokenEndPoint = null;
        private readonly IConfiguration _config;

        public TokenController(IConfiguration config)
        {
            _config = config;
        }
        private async Task<string> TokenEndPoint()
        {
            if (string.IsNullOrEmpty(_tokenEndPoint))
            {
                using (var client = new HttpClient())
                {
                    var disco = await client.GetDiscoveryDocumentAsync(_config.GetSection("IdentityServer:Authority").Value);
                    if (disco.IsError)
                    {
                        throw new Exception(disco.Error);
                    }
                    return disco.TokenEndpoint;
                }
            }
            return _tokenEndPoint;
        }

        [HttpGet]
        [Route("{username}/{password}")]
        public async Task<JObject> Token(string username, string password)
        {
            using (var client = new HttpClient())
            {
                // request token
                var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = await TokenEndPoint(),
                    ClientId = _config.GetSection("IdentityServer:Client:ClientId").Value,
                    ClientSecret = _config.GetSection("IdentityServer:Client:ClientSecret").Value,

                    UserName = username,
                    Password = password,
                    //refresh_token need scope include offline_access
                    Scope = _config.GetSection("IdentityServer:Audience").Value+" offline_access",

                    //ClientAssertion =
                    //{
                    //    Type = OidcConstants.ClientAssertionTypes.JwtBearer,
                    //    Value = CreateClientToken("client.jwt", disco.TokenEndpoint)
                    //}
                });
                if (tokenResponse.IsError)
                {
                    throw new Exception(tokenResponse.Error);
                }
                return tokenResponse.Json;
            }
        }

        [HttpGet]
        [Route("{refreshToken}")]
        public async Task<JObject> RefreshToken(string refreshToken)
        {
            using (var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync(_config.GetSection("IdentityServer:Authority").Value);
                if (disco.IsError)
                {
                    throw new Exception(disco.Error);
                }
                var response = await client.RequestRefreshTokenAsync(new RefreshTokenRequest
                {
                    Address = await TokenEndPoint(),

                    ClientId = _config.GetSection("IdentityServer:Client:ClientId").Value,
                    ClientSecret = _config.GetSection("IdentityServer:Client:ClientSecret").Value,
                    RefreshToken = refreshToken
                });

                if (response.IsError) throw new Exception(response.Error);
                return response.Json;
            }
        }
    }
}