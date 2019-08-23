using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    public class ClientGrantType : XPObject
    {
        public ClientGrantType() : base(Session.DefaultSession)
        {
        }

        public ClientGrantType(Session session) : base(session)
        {
        }
        [Association]
        public Client Client { get; set; }
        public string GrantType { get; set; }
    }
}
