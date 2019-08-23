using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    [NonPersistent]
    public abstract class UserClaim : XPObject
    {
        public UserClaim() : base(Session.DefaultSession)
        {
        }

        public UserClaim(Session session) : base(session)
        {
        }
   
        public string Type { get; set; }
    }
}
