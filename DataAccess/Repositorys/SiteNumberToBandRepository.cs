using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

    public class SiteNumberToBandRepository : Repository<SiteNumberToBand>, ISiteNumberToBandRepository
    {
        private readonly FuelcardsContext _db;

        public SiteNumberToBandRepository(FuelcardsContext db) : base(db)
        {
            _db = db;
        }

        public void Update(SiteNumberToBand source)
        {
            var dbObj = _db.SiteNumberToBands.FirstOrDefault(s => s.Id == source.Id);
            if (dbObj is null) _db.Add(source);
            else UpdateDbObject(dbObj, source);
        }
        public async Task UpdateAsync(SiteNumberToBand source)
        {
            var dbObj = _db.SiteNumberToBands.FirstOrDefault(s => s.SiteNumber == source.SiteNumber && s.NetworkId == source.NetworkId && s.Active != false);
            if (dbObj is null) await _db.SiteNumberToBands.AddAsync(source);
            else UpdateDbObject(dbObj, source);
        }
        public async Task<SiteNumberToBand>? GetSpecificSurcharge(DateOnly date, int site, int network)
        {
            var Charges = _db.SiteNumberToBands.Where(e => e.NetworkId == network && e.SiteNumber == site && e.EffectiveDate <= date && e.Active != false).OrderByDescending(e => e.EffectiveDate).FirstOrDefault();
            return Charges;
        }
        private void UpdateDbObject(SiteNumberToBand dbObj, SiteNumberToBand source)
        {
            dbObj.Id = dbObj.Id;
            dbObj.EffectiveDate = source.EffectiveDate;
            dbObj.Network = source.Network;
            dbObj.SiteNumber = source.SiteNumber;
            dbObj.Brand = source.Brand;
            dbObj.Band = source.Band;
            dbObj.ShareTheBurdon = source.ShareTheBurdon;
            dbObj.Classification = source.Classification;
            dbObj.Surcharge = source.Surcharge;
            dbObj.Active = source.Active;
            dbObj.Name = source.Name;

        }
    }
}