using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Models;
using IdentityServer4.Xpo.Mappers;
using IdentityServer.Models;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InitDataController : ControllerBase
    {
        private UnitOfWork _uof;
        public InitDataController( UnitOfWork uof)
        {
            _uof = uof;
        }

        [HttpGet]
        public void Init()
        {
            if (!_uof.Query<IdentityServer4.Xpo.Entities.ApiResource>().Any())
            {
                
                var resources = new List<IdentityServer4.Xpo.Entities.ApiResource> {
                     new IdentityServer4.Xpo.Entities.ApiResource{ Name = "api1" , Description = "Api" , DisplayName ="api1"  },

                    };
                resources[0].Scopes.AddRange( new XPCollection<IdentityServer4.Xpo.Entities.ApiScope> { new IdentityServer4.Xpo.Entities.ApiScope { ApiResource=resources[0], Name = "api1", DisplayName = "api1" } });
                Session.DefaultSession.Save(resources);
            }

            if (!_uof.Query<IdentityServer4.Xpo.Entities.IdentityResource>().Any())
            {
                var resources = new List<IdentityServer4.Xpo.Entities.IdentityResource> {
                        new IdentityResources.OpenId().ToEntity(),
                        new IdentityResources.Profile().ToEntity(),
                        new IdentityResources.Email().ToEntity(),
                        new IdentityResources.Phone().ToEntity()
                    };
                Session.DefaultSession.Save(resources);
            }
            if (!_uof.Query<IdentityServer4.Xpo.Entities.Client>().Any())
            {
                var clients = new List<IdentityServer4.Xpo.Entities.Client>
                    {
                          new IdentityServer4.Xpo.Entities.Client
                            {
                              AllowOfflineAccess = true,
                                ClientId ="ro.client",
                                ClientName = "mvc",
                                RequireClientSecret = true,
                                RequireConsent = false,
                          }
                        };
                //GrantType.Hybrid和GrantType.Implicit冲突，只能选一个
                clients[0].AllowedGrantTypes.AddRange(new XPCollection<IdentityServer4.Xpo.Entities.ClientGrantType> {
                    //new IdentityServer4.Xpo.Entities.ClientGrantType {GrantType = IdentityServer4.Models.GrantType.Hybrid },
                    new IdentityServer4.Xpo.Entities.ClientGrantType {GrantType = IdentityServer4.Models.GrantType.Implicit },
                    new IdentityServer4.Xpo.Entities.ClientGrantType { GrantType = IdentityServer4.Models.GrantType.ResourceOwnerPassword}  });

                clients[0].AllowedCorsOrigins.AddRange(new XPCollection<IdentityServer4.Xpo.Entities.ClientCorsOrigin> { new IdentityServer4.Xpo.Entities.ClientCorsOrigin { Origin = "http://localhost:5003" } });
                clients[0].ClientSecrets.AddRange(new XPCollection<IdentityServer4.Xpo.Entities.ClientSecret> { new IdentityServer4.Xpo.Entities.ClientSecret { Value = "secret".Sha256() } });

                clients[0].AllowedScopes.AddRange(new XPCollection<IdentityServer4.Xpo.Entities.ClientScope>{
                        new IdentityServer4.Xpo.Entities.ClientScope { Scope = IdentityServer4.IdentityServerConstants.StandardScopes.OpenId },
                        new IdentityServer4.Xpo.Entities.ClientScope { Scope = IdentityServer4.IdentityServerConstants.StandardScopes.Profile },
                        new IdentityServer4.Xpo.Entities.ClientScope { Scope ="api1" }
                });

                clients[0].RedirectUris.AddRange(new XPCollection<IdentityServer4.Xpo.Entities.ClientRedirectUri> {
                    new IdentityServer4.Xpo.Entities.ClientRedirectUri { RedirectUri = "http://localhost:5003/signin-oidc" },
                    new IdentityServer4.Xpo.Entities.ClientRedirectUri { RedirectUri = "http://localhost:65061" }
                });

                clients[0].PostLogoutRedirectUris.AddRange(new XPCollection<IdentityServer4.Xpo.Entities.ClientPostLogoutRedirectUri> {
                    new IdentityServer4.Xpo.Entities.ClientPostLogoutRedirectUri { PostLogoutRedirectUri = "http://localhost:5003/signout-callback-oidc" },
                    new IdentityServer4.Xpo.Entities.ClientPostLogoutRedirectUri { PostLogoutRedirectUri = "http://localhost:65061" }
                });

                Session.DefaultSession.Save(clients);
            }
            if (!_uof.Query<ApplicationUser>().Any())
            {
                new ApplicationUser
                {
                    LoginName = "test",
                    Password = "test"
                }.Save();
            }
            
        }

    }
}
