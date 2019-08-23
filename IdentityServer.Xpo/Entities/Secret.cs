using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityServer4.Xpo.Entities
{
    [NonPersistent]
    public abstract class Secret : XPObject
    {
        public Secret() : base(Session.DefaultSession)
        {
        }

        public Secret(Session session) : base(session)
        {
        }

        public string Description { get; set; }
        public string Value { get; set; }
        public DateTime? Expiration { get; set; }
        public string Type { get; set; } = SecretTypes.SharedSecret;
    }
}
