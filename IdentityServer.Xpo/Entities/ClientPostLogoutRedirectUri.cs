using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    public class ClientPostLogoutRedirectUri : XPObject
    {
        public ClientPostLogoutRedirectUri() : base(Session.DefaultSession)
        {
        }

        public ClientPostLogoutRedirectUri(Session session) : base(session)
        {
        }
        [Association]
        public Client Client { get; set; }
        public string PostLogoutRedirectUri { get; set; }
    }
}
