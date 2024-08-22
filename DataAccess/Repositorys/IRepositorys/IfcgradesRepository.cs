
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IfcgradesRepository : IRepository<FcGrade>
	{
		void Update(FcGrade source);
		Task UpdateAsync(FcGrade source);

    }
}