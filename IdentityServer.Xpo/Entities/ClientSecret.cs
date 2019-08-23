using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    public class ClientSecret : Secret
    {
        public ClientSecret() : base(Session.DefaultSession)
        {
        }

        public ClientSecret(Session session) : base(session)
        {
        }
        [Association]
        public Client Client { get; set; }
    }
}
