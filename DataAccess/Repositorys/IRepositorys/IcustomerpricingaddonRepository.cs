using System.Threading.Tasks;
using Portland.Data.Repository.IRepository;
using System.Collections.Generic;
using System;
using DataAccess.Fuelcards;
using DataAccess.Repositorys.IRepositorys;
using System.Linq;

namespace Portland.Data.Repository.IRepository
{
    public interface IcustomerpricingaddonRepository : IRepository<CustomerPricingAddon>
    {
        void Update(CustomerPricingAddon customerpricingaddon);
        Task UpdateAsync(CustomerPricingAddon fgCard);
    }
}
