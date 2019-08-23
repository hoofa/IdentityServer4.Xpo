using DevExpress.Xpo;
using System;

namespace IdentityServer4.Xpo.Entities
{
    public class PersistedGrant : XPGuidObject
    {
        public PersistedGrant() : base(Session.DefaultSession)
        {
        }
        public PersistedGrant(Session session) : base(session)
        {
        }

        public string Key { get; set; }
        public string Type { get; set; }
        public string SubjectId { get; set; }
        public string ClientId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime Expiration { get; set; }
        [Size(-1)]
        public string Data { get; set; }
    }
}
