using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    public class IdentityClaim: UserClaim
    {
        public IdentityClaim() : base(Session.DefaultSession)
        {
        }

        public IdentityClaim(Session session) : base(session)
        {
        }

        [Association]
        public IdentityResource IdentityResource { get; set; }
    }
}
