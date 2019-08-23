using DevExpress.Xpo;
using System;
using System.Collections.Generic;

namespace IdentityServer4.Xpo.Entities
{
    public class IdentityResource : XPGuidObject
    {
        public IdentityResource() : base(Session.DefaultSession)
        {
        }

        public IdentityResource(Session session) : base(session)
        {
        }

        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        [Association]
        public XPCollection<IdentityClaim> UserClaims => GetCollection<IdentityClaim>("UserClaims");
    }

}
