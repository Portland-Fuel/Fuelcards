using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IInvoiceNumberRepository : IRepository<InvoiceNumber>
	{
		void Update(InvoiceNumber source);
		Task UpdateAsync(InvoiceNumber source);

    }
}