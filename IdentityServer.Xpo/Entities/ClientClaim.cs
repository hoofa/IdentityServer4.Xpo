using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    public class ClientClaim : XPObject
    {
        public ClientClaim() : base(Session.DefaultSession)
        {
        }

        public ClientClaim(Session session) : base(session)
        {
        }
        [Association]
        public Client Client { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
