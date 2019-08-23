using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    public class ClientRedirectUri : XPObject
    {
        public ClientRedirectUri() : base(Session.DefaultSession)
        {
        }

        public ClientRedirectUri(Session session) : base(session)
        {
        }
        [Association]
        public Client Client { get; set; }
        public string RedirectUri { get; set; }
    }
}
