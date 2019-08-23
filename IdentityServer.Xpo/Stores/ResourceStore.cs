using IdentityServer4.Stores;
using System;
using System.Collections.Generic;
using IdentityServer4.Xpo.Mappers;
using System.Threading.Tasks;
using IdentityServer4.Models;
using System.Linq;
using DevExpress.Xpo;

namespace IdentityServer4.Xpo.Stores
{
	public class ResourceStore : IResourceStore
	{
		private readonly UnitOfWork _uow;
        public ResourceStore(UnitOfWork uow)
		{
            _uow = uow;
        }
		public async Task<ApiResource> FindApiResourceAsync(string name)
		{
			var resource = await _uow.Query<Entities.ApiResource>().FirstOrDefaultAsync(_ => _.Name == name).ConfigureAwait(false);
			return resource.ToModel();
		}

		public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
		{
			if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

			var resources = _uow.Query<Entities.ApiResource>().ToList();
			return Task.FromResult(resources.Where(x => scopeNames.Contains(x.Name)).Select(x => x.ToModel()).AsEnumerable());
		}

		public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
		{
			if (scopeNames == null) throw new ArgumentNullException(nameof(scopeNames));

			var identities = _uow.Query<Entities.IdentityResource>().ToList();
			return Task.FromResult(identities.Where(x => scopeNames.Contains(x.Name)).Select(x => x.ToModel()).AsEnumerable());
		}

		public Task<Resources> GetAllResourcesAsync()
		{
			var identityResources = _uow.Query<Entities.IdentityResource>().ToList();
			var apiResources = _uow.Query<Entities.ApiResource>().ToList();
			return Task.FromResult(new Resources(identityResources.Select(x => x.ToModel()), apiResources.Select(x => x.ToModel())));
		}
	}
}
