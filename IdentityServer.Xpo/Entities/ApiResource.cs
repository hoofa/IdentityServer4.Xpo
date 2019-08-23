using DevExpress.Xpo;
using System;
using System.Collections.Generic;

namespace IdentityServer4.Xpo.Entities
{
    public class ApiResource : XPGuidObject
    {
        public ApiResource() : base(Session.DefaultSession)
        {
        }
        public ApiResource(Session session) : base(session)
        {
        }

        public bool Enabled { get; set; } = true;
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        [Association]
        public XPCollection<ApiSecret> Secrets => GetCollection<ApiSecret>("Secrets");

        [Association]
        public XPCollection<ApiScope> Scopes => GetCollection<ApiScope>("Scopes");
        [Association]
        public XPCollection<ApiResourceClaim> UserClaims => GetCollection<ApiResourceClaim>("UserClaims");
    }
}
