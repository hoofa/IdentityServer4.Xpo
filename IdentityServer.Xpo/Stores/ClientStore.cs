using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Xpo.Mappers;
using DevExpress.Xpo;

namespace IdentityServer4.Xpo.Stores
{
	public class ClientStore : IClientStore
	{
        private readonly UnitOfWork _uow;
        public ClientStore(UnitOfWork uow)
		{
            _uow = uow;
		}
		public async Task<Client> FindClientByIdAsync(string clientId)
		{
			var client = await _uow.Query<Entities.Client>().FirstOrDefaultAsync(x => x.ClientId == clientId);
			var m= client?.ToModel();
            return m;
		}
	}
}