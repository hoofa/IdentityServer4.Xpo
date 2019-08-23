using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer.Models;
using DevExpress.Xpo;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Services
{
    public class LoginService : ILoginService<ApplicationUser>
    {

        private readonly UnitOfWork _uof;
        private readonly IHttpContextAccessor _contextAccessor;
        private HttpContext _context;
        public LoginService(UnitOfWork uof, IHttpContextAccessor contextAccessor)
        {
            _uof = uof;
            _contextAccessor = contextAccessor;
        }

        /// <summary>
        /// The <see cref="HttpContext"/> used.
        /// </summary>
        public HttpContext Context
        {
            get
            {
                var context = _context ?? _contextAccessor?.HttpContext;
                if (context == null)
                {
                    throw new InvalidOperationException("HttpContext must not be null.");
                }
                return context;
            }
            set
            {
                _context = value;
            }
        }

        public async Task<ApplicationUser> FindByUsername(string user)
        {
            return await _uof.Query<ApplicationUser>().FirstOrDefaultAsync(o => o.LoginName == user);
        }


        public async Task SignInAsync(ApplicationUser user, AuthenticationProperties properties = null, string authenticationMethod = null)
        {
            var claims = new List<Claim>( ProfileService.GetUserClaims(user));
            // Review: should we guard against CreateUserPrincipal returning null?
            if (authenticationMethod != null)
            {
                claims.Add(new Claim(ClaimTypes.AuthenticationMethod, authenticationMethod));
            }
            await Context.SignInAsync(//IdentityConstants.ApplicationScheme,
                user.Id.ToString(),user.LoginName,
                properties ?? new AuthenticationProperties(), claims.ToArray());
        }

        public Task<bool> ValidateCredentials(ApplicationUser user, string password)
        {
            if(user.Password==password)
                return Task.FromResult(true);
            else
                return Task.FromResult(false);
        }
    }
}
