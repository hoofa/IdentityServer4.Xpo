using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    public class ApiResourceClaim : UserClaim
    {
        public ApiResourceClaim() : base(Session.DefaultSession)
        {
        }

        public ApiResourceClaim(Session session) : base(session)
        {
        }
        [Association]
        public ApiResource ApiResource { get; set; }
    }
}
