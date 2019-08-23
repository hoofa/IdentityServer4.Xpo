using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using IdentityServer4.Xpo.Entities;
using System.Linq;
using System.Threading.Tasks;
using System;
using DevExpress.Xpo;

namespace IdentityServer4.Xpo.Services
{
    public class CorsPolicyService : ICorsPolicyService
    {
        private readonly ILogger<CorsPolicyService> _logger;
        private readonly UnitOfWork _uow;
        public CorsPolicyService(ILogger<CorsPolicyService> logger, UnitOfWork uow)
        {
            _logger = logger;
            _uow = uow;
        }
        public async Task<bool> IsOriginAllowedAsync(string origin)
        {
            var origins = await _uow.Query<Client>().SelectMany(x => x.AllowedCorsOrigins).Select(y => y.Origin).ToListAsync().ConfigureAwait(false);
            var distinctOrigins = origins.Where(x => x != null).Distinct();
            var isAllowed = distinctOrigins.Any(x => x.ToLower() == origin.ToLower());

            _logger.LogDebug("Origin {origin} is allowed: {originAllowed}", origin, isAllowed);

            return isAllowed;

        }
    }
}
