using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    public class ClientCorsOrigin : XPObject
    {
        public ClientCorsOrigin() : base(Session.DefaultSession)
        {
        }

        public ClientCorsOrigin(Session session) : base(session)
        {
        }
        [Association]
        public Client Client { get; set; }

        public string Origin { get; set; }
    }
}
