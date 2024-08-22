
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IFcEmailsRepository : IRepository<FcEmail>
	{
		void Update(FcEmail source);
		Task UpdateAsync(FcEmail source);

    }
}