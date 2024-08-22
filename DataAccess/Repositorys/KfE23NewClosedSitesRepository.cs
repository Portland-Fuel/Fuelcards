using DataAccess.Fuelcards;
using DataAccess.Repository;
using Portland.Data.Repository.IRepository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Portland.Data.Repository
{

	public class KfE23NewClosedSitesRepository : Repository<KfE23NewClosedSite>, IKfE23NewClosedSitesRepository
    {
		private readonly FuelcardsContext _db;

		public KfE23NewClosedSitesRepository(FuelcardsContext db) : base(db)
		{
			_db = db;
		}

		public void Update(KfE23NewClosedSite source)
		{
			var dbObj = _db.KfE23NewClosedSites.FirstOrDefault(s => s.SiteAccountCode == source.SiteAccountCode);
			if (dbObj is null) _db.Add(source);
			else UpdateDbObject(dbObj, source);
		}

        public KfE23NewClosedSite Find(int code)
        {
            return _db.KfE23NewClosedSites.Find(code);
        }
        public async Task UpdateAsync(KfE23NewClosedSite source)
		{
			var dbObj = _db.KfE23NewClosedSites.FirstOrDefault(e=>e.SiteAccountCode == source.SiteAccountCode);
			if (dbObj is null) await _db.KfE23NewClosedSites.AddAsync(source);
			else UpdateDbObject(dbObj, source);
		}
        public async Task<KfE23NewClosedSite> FindAsync(int code)
        {
            var dbObj = await _db.KfE23NewClosedSites.FindAsync(code);
            return dbObj;
        }

        public IQueryable<KfE23NewClosedSite> Where(Func<KfE23NewClosedSite, bool> predicate)
        {
            return _db.KfE23NewClosedSites.Where(predicate).AsQueryable();
        }
        public KfE23NewClosedSite First(Func<KfE23NewClosedSite, bool> predicate)
        {
            return _db.KfE23NewClosedSites.Where(predicate).First();
        }
        private void UpdateDbObject(KfE23NewClosedSite dbObj, KfE23NewClosedSite source)
		{
            dbObj.SiteAccountCode = source.SiteAccountCode;
            dbObj.SiteAccountSuffix = source.SiteAccountSuffix;
            dbObj.SiteStatus = source.SiteStatus;
            dbObj.Name = source.Name;
            dbObj.AddressLine1 = source.AddressLine1;
            dbObj.AddressLine2 = source.AddressLine2;
            dbObj.Town = source.Town;
            dbObj.County = source.County;
            dbObj.Postcode = source.Postcode;
            dbObj.TelephoneNumber = source.TelephoneNumber;
            dbObj.ContactName = source.ContactName;
            dbObj.RetailSite = source.RetailSite;
            dbObj.Canopy = source.Canopy;
            dbObj.MachineType = source.MachineType;
            dbObj.OpeningHours1 = source.OpeningHours1;
            dbObj.OpeningHours2 = source.OpeningHours2;
            dbObj.OpeningHours3 = source.OpeningHours3;
            dbObj.Directions = source.Directions;
            dbObj.PoleSignSupplier = source.PoleSignSupplier;
            dbObj.Parking = source.Parking;
            dbObj.Payphone = source.Payphone;
            dbObj.Gasoil = source.Gasoil;
            dbObj.Showers = source.Showers;
            dbObj.OvernightAccomodation = source.OvernightAccomodation;
            dbObj.CafeRestaurant = source.CafeRestaurant;
            dbObj.Toilets = source.Toilets;
            dbObj.Shop = source.Shop;
            dbObj.Lubricants = source.Lubricants;
            dbObj.SleeperCabsWelcome = source.SleeperCabsWelcome;
            dbObj.TankCleaning = source.TankCleaning;
            dbObj.Repairs = source.Repairs;
            dbObj.WindscreenReplacement = source.WindscreenReplacement;
            dbObj.Bar = source.Bar;
            dbObj.CashpointMachines = source.CashpointMachines;
            dbObj.VehicleClearanceAccepted = source.VehicleClearanceAccepted;
            dbObj.MotorwayJunction = source.MotorwayJunction;
            dbObj.MotorwayNumber = source.MotorwayNumber;
            dbObj.JunctionNumber = source.JunctionNumber;
        }
    }
}