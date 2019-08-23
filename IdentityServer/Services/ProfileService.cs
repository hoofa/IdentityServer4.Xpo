using IdentityServer.Models;
using DevExpress.Xpo;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Services
{
    public class ProfileService : IProfileService
    {
        public UnitOfWork _uof;
        public ProfileService(UnitOfWork uof)
        {
            _uof = uof;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                //depending on the scope accessing the user data.
                if (!string.IsNullOrEmpty(context.Subject.Identity.Name))
                {
                    //get user from db (in my case this is by email)
                    var user = await _uof.Query<ApplicationUser>().FirstOrDefaultAsync(o => o.LoginName == context.Subject.Identity.Name);

                    if (user != null)
                    {
                        var claims = GetUserClaims(user);

                        //set issued claims to return
                        context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                    }
                }
                else
                {
                    //get subject from context (this was set ResourceOwnerPasswordValidator.ValidateAsync),
                    //where and subject was set to my user id.
                    var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "sub");

                    if (!string.IsNullOrEmpty(userId?.Value))
                    {
                        //get user from db (find user by user id)
                        var user = await _uof.GetObjectByKeyAsync<ApplicationUser>(userId);

                        // issue the claims for the user
                        if (user != null)
                        {
                            var claims = GetUserClaims(user);

                            context.IssuedClaims = claims.Where(x => context.RequestedClaimTypes.Contains(x.Type)).ToList();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //log your error
            }

            return;
        }

        //build claims array from user data
        public static Claim[] GetUserClaims(ApplicationUser user)
        {
            return new Claim[]
            {
            new Claim("user_id", user.Id.ToString() ),
            new Claim(JwtClaimTypes.Name, user.Name ?? ""),
            new Claim(JwtClaimTypes.Email, user.Email  ?? ""),

            //roles
            //new Claim(JwtClaimTypes.Role, user.Role)
            };
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            try
            {
                //get subject from context (set in ResourceOwnerPasswordValidator.ValidateAsync),
                var userId = context.Subject.Claims.FirstOrDefault(x => x.Type == "user_id");

                if (!string.IsNullOrEmpty(userId?.Value))
                {
                    var user = await _uof.GetObjectByKeyAsync<ApplicationUser>(userId);

                    if (user != null)
                    {
                        context.IsActive = !user.Invalid;
                    }
                }
            }
            catch (Exception ex)
            {
                //handle error logging
            }
            return;
        }


    }
}
