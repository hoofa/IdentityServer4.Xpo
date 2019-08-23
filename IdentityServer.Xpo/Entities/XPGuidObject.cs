using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    [NonPersistent]
    public abstract class XPGuidObject:  XPBaseObject
    {
        public XPGuidObject() : base(Session.DefaultSession)
        {
        }

        public XPGuidObject(Session session) : base(session)
        {
        }

        [Persistent("Oid"), Key(true)]
        public Guid Id { get; set; }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Id = new Guid();
        }
    }
}
