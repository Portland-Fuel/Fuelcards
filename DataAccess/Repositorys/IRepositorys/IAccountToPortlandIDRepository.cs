using System.Threading.Tasks;
using Portland.Data.Repository.IRepository;
using System.Collections.Generic;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
    public interface IAccountToPortlandIDRepository : IRepository<FcNetworkAccNoToPortlandId>
    {
        void Update(FcNetworkAccNoToPortlandId accNoToPortlandId);
        Task UpdateAsync(FcNetworkAccNoToPortlandId accNoToPortlandId);
        Task<int> GetLatestID();
        Task<int> GetLatestPortlandID();
        Task<List<int>> GetAllPortlandIds();
        Task<List<int>> GetAllAccounts(int? PortlandID);

    }
}
