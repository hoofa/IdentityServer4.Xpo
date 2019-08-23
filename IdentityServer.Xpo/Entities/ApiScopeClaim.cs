using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    public class ApiScopeClaim: UserClaim
    {
        public ApiScopeClaim() : base(Session.DefaultSession)
        {
        }

        public ApiScopeClaim(Session session) : base(session)
        {
        }
        [Association]
        public ApiScope ApiScope { get; set; }
    }
}
