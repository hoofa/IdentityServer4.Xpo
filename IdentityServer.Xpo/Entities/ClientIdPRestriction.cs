using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    public class ClientIdPRestriction : XPObject
    {
        public ClientIdPRestriction() : base(Session.DefaultSession)
        {
        }

        public ClientIdPRestriction(Session session) : base(session)
        {
        }
        [Association]
        public Client Client { get; set; }
        public string Provider { get; set; }
    }
}
