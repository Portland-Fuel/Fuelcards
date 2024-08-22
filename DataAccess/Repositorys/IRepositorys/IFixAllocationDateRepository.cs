
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
namespace DataAccess.Repositorys.IRepositorys
{
    public interface IFixAllocationDateRepository : IRepository<FixAllocationDate>
    {
        void Update(FixAllocationDate source);
        Task UpdateAsync(FixAllocationDate source);
    }
}