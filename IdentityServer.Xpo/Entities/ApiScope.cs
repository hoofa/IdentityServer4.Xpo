using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    public class ApiScope : XPObject
    {
        public ApiScope() : base(Session.DefaultSession)
        {
        }

        public ApiScope(Session session) : base(session)
        {
        }
        [Association]
        public ApiResource ApiResource { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Required { get; set; }
        public bool Emphasize { get; set; }
        public bool ShowInDiscoveryDocument { get; set; } = true;
        [Association]
        public XPCollection<ApiScopeClaim> UserClaims => GetCollection<ApiScopeClaim>("UserClaims");

    }
}
