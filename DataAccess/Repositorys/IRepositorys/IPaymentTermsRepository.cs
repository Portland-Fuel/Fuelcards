
using Portland.Data.Repository.IRepository;
using System.Linq;
using System;
using System.Threading.Tasks;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;

namespace Portland.Data.Repository.IRepository
{
	public interface IPaymentTermsRepository : IRepository<PaymentTerm>
	{
		void Update(PaymentTerm source);
		Task UpdateAsync(PaymentTerm source);
    }
}