namespace IdentityServer4.Xpo.Stores
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DevExpress.Xpo;
    using IdentityServer4.Models;
    using IdentityServer4.Stores;
    using IdentityServer4.Xpo.Mappers;
    using XpoBatch;

    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly UnitOfWork _uow;
        public PersistedGrantStore(UnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
           var grants = await _uow.Query<Entities.PersistedGrant>().Where(x => x.SubjectId == subjectId).ToListAsync().ConfigureAwait(false);
            return grants.Select(y => y.ToModel());
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var grant =  await _uow.Query<Entities.PersistedGrant>().FirstOrDefaultAsync(x => x.Key == key).ConfigureAwait(false);
            return grant?.ToModel();
        }

        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            _uow.DeleteWhere<Entities.PersistedGrant>(grant => grant.SubjectId == subjectId && grant.ClientId == clientId);
            return _uow.CommitChangesAsync();
        }

        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            _uow.DeleteWhere<Entities.PersistedGrant>(grant => grant.Type == type && grant.SubjectId == subjectId && grant.ClientId == clientId);
            return _uow.CommitChangesAsync();
        }

        public Task RemoveAsync(string key)
        {
            _uow.DeleteWhere<Entities.PersistedGrant>(grant => grant.Key == key);
            return _uow.CommitChangesAsync();
        }

        public Task StoreAsync(PersistedGrant grant)
        {
            grant.ToEntity().Save();
            return _uow.CommitChangesAsync();
        }
    }
}