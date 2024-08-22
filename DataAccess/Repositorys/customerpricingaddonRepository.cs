//using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using DataAccess.Fuelcards;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Portland.Data.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{
    class customerpricingaddonRepository(FuelcardsContext db) : Repository<CustomerPricingAddon>(db), IcustomerpricingaddonRepository
    {
        private readonly FuelcardsContext _db = db;

        public void Update(CustomerPricingAddon source)
        {
            var dbObj = _db.CustomerPricingAddons.FirstOrDefault(s => s.Id == source.Id);
            if (dbObj is null) _db.CustomerPricingAddons.Add(source);
            else UpdateDbObject(dbObj, source);
        }

        public async Task UpdateAsync(CustomerPricingAddon source)
        {
            var dbObj = _db.CustomerPricingAddons.FirstOrDefault(s => s.PortlandId == source.PortlandId && s.Network == source.Network && s.EffectiveDate == source.EffectiveDate);
            if (dbObj is null) await _db.CustomerPricingAddons.AddAsync(source);
            else UpdateDbObject(dbObj, source);
        }
       
        private void UpdateDbObject(CustomerPricingAddon dbObj, CustomerPricingAddon source)
        {
            dbObj.EffectiveDate = source.EffectiveDate;
            dbObj.PortlandId = source.PortlandId;
            dbObj.Network = source.Network;
            dbObj.Addon = source.Addon;

        }
    }
}
