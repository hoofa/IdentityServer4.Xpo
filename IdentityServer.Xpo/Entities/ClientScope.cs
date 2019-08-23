using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    public class ClientScope : XPObject
    {
        public ClientScope() : base(Session.DefaultSession)
        {
        }

        public ClientScope(Session session) : base(session)
        {
        }
        [Association]
        public Client Client { get; set; }
        public string Scope { get; set; }
    }
}
