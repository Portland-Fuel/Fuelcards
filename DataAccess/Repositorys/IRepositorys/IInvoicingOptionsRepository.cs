
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
using System.Threading.Tasks;

namespace Portland.Data.Repository.IRepository
{
	public interface IInvoicingOptionsRepository : IRepository<InvoicingOption>
	{
		void Update(InvoicingOption source);
		Task UpdateAsync(InvoicingOption source);
	}
}