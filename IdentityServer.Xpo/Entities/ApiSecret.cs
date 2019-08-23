using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer4.Xpo.Entities
{
    public class ApiSecret : Secret 
    {
        public ApiSecret() : base(Session.DefaultSession)
        {
        }

        public ApiSecret(Session session) : base(session)
        {
        }

        [Association]
        public ApiResource ApiResource { get; set; }
    }
}
